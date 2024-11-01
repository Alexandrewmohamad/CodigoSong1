using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PnT.SongServer;

using PnT.SongDB.Logic;
using PnT.SongClient.Logic;

namespace PnT.SongClient.Data
{

    #region Enumerations **************************************************************

    /// <summary>
    /// Enumerates the possible web service status.
    /// </summary>
    public enum WebServiceStatus
    {
        Off, Connected, Disconnected, FatalError, Starting, Stopping, StartError
    };

    #endregion Enumerations


    #region Delegates *****************************************************************

    /// <summary>
    /// Delegate used with web service status event handler methods.
    /// </summary>
    /// <param name="status">
    /// The new web service status.
    /// </param>
    public delegate void WebServiceStatusEventHandler(WebServiceStatus status);

    #endregion Delegates


    /// <summary>
    /// Manages the web service connection.
    /// </summary>
    public class WebServiceManager
    {

        #region Constants *************************************************************

        /// <summary>
        /// Waiting time until a new attempt to reconnect 
        /// to web service is performed. In milliseconds.
        /// </summary>
        private const double RECONNECT_INTERVAL = 10000.0;

        /// <summary>
        /// Waiting time until a new connection test is performed. In milliseconds.
        /// </summary>
        private const double HEARTBEAT_INTERVAL = 10000.0;

        #endregion Constants


        #region Events ****************************************************************

        /// <summary>
        /// This event is raised whenever protoBuf client status has changed.
        /// </summary>
        public event WebServiceStatusEventHandler StatusChanged;

        #endregion Events


        #region Fields ****************************************************************

        /// <summary>
        /// The channel factory used to create clients.
        /// </summary>
        ChannelFactory<ISongService> channelFactory = null;

        /// <summary>
        /// The current web service status.
        /// </summary>
        private WebServiceStatus status = WebServiceStatus.Off;

        /// <summary>
        /// The previous web service status.
        /// </summary>
        private WebServiceStatus previousStatus = WebServiceStatus.Off;

        /// <summary>
        /// Tells the running thread to keep connecting to web service.
        /// Used to stop the thread in a clean way without killing it.
        /// </summary>
        private bool keepRunning = false;

        /// <summary>
        /// The runnning thread responsable for connecting 
        /// and monitoring the connection to the web service.
        /// </summary>
        private Thread thrRun = null;

        /// <summary>
        /// Tells the running thread to wait before trying to reconnect again.
        /// Avoid reconnecting immediately after the connection is lost.
        /// </summary>
        private bool waitBeforeReconnecting = false;

        /// <summary>
        /// The port of the web service server.
        /// </summary>
        private int serverPort = int.MinValue;

        /// <summary>
        /// The IP of the web service server.
        /// </summary>
        private string serverIP = string.Empty;

        /// <summary>
        /// The information of the song server.
        /// </summary>
        private ServerInfo serverInfo = null;

        /// <summary>
        /// Indicates if this is the first connection to server.
        /// </summary>
        private bool isFirstConnection = true;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WebServiceManager()
        {
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get the port of the web service server.
        /// </summary>
        public int ServerPort
        {
            get
            {
                return serverPort;
            }
        }

        /// <summary>
        /// Get the information of the song server.
        /// </summary>
        public ServerInfo ServerInfo
        {
            get
            {
                return serverInfo;
            }
        }

        /// <summary>
        /// Get the IP of the web service server.
        /// </summary>
        public string ServerIP
        {
            get
            {
                return serverIP;
            }
        }

        /// <summary>
        /// Get/set the current web service status.
        /// </summary>
        public WebServiceStatus Status
        {
            get { return status; }
            private set
            {
                //check if value has changed
                if (value != this.status)
                {
                    //store current value
                    previousStatus = this.status;

                    //update status with new value
                    status = value;

                    //raise status change event
                    OnStatusChanged(status);
                }
            }
        }

        #endregion Properties


        #region Public Management Methods *********************************************

        /// <summary>
        /// Start web service manager.
        /// </summary>
        /// <param name="errorMessage">
        /// A message describing the error when start fails.
        /// </param>
        /// <returns>
        /// True if manager was started and web service is running.
        /// False otherwise.
        /// </returns>
        public bool Start(ref string errorMessage)
        {
            //check status
            if (this.Status == WebServiceStatus.Connected ||
                this.Status == WebServiceStatus.Disconnected)
            {
                //manager is already started
                Manager.Log.WriteInfo(
                    "Web service manager is already running.");

                //manager is running
                return true;
            }
            else if (this.Status == WebServiceStatus.Starting)
            {
                //manager is already starting
                //log message
                Manager.Log.WriteError("Web service manager is already starting.");

                //manager is not running yet
                return false;
            }
            else if (this.Status == WebServiceStatus.Stopping)
            {
                //manager is stopping
                //log message
                Manager.Log.WriteError("Web service manager is stopping.");

                //manager is not running
                return false;
            }

            //write message
            Manager.Log.WriteInfo("Starting web service manager...");

            try
            {
                //set status to starting
                this.Status = WebServiceStatus.Starting;

                //signals that run thread should keep running
                keepRunning = true;

                //try to reconnect as soon as possible
                waitBeforeReconnecting = false;

                //will connect to server for the first time
                isFirstConnection = true;

                //reset song server info
                serverInfo = null;

                //start thread and keep running client
                thrRun = new Thread(new ThreadStart(KeepRunning));
                thrRun.IsBackground = true;
                thrRun.Start();

                //signals that the thread is running by changing status
                this.Status = WebServiceStatus.Disconnected;

                //write message
                Manager.Log.WriteInfo("Web service manager was started and is running.");

                //client was started
                return true;
            }
            catch (Exception ex)
            {
                //should never happen
                //log exception
                Manager.Log.WriteException("Unexpected error while starting web service manager.", ex);

                //set status to start error
                this.Status = WebServiceStatus.StartError;

                //stop running thread if it is running
                keepRunning = false;

                //could not start client
                return false;
            }
        }

        /// <summary>
        /// Stop web service manager.
        /// </summary>
        public void Stop()
        {
            //check status
            if (this.Status == WebServiceStatus.Off ||
                this.Status == WebServiceStatus.StartError ||
                this.Status == WebServiceStatus.FatalError)
            {
                //manager is already starting
                //log message
                Manager.Log.WriteError("Web service manager is already stopped.");

                //manager is not running
                return;
            }
            else if (this.Status == WebServiceStatus.Stopping)
            {
                //manager is stopping
                //log message
                Manager.Log.WriteError("Web service manager is already stopping.");

                //manager is not running
                return;
            }

            //write message
            Manager.Log.WriteInfo("Stopping web service manager...");

            try
            {
                //set status to stopping
                this.Status = WebServiceStatus.Stopping;

                //signals that run thread should stop
                keepRunning = false;

                //wait small intervals
                DateTime startTime = DateTime.Now;

                //wait for thread to stop running
                //does not wait forever
                while (thrRun != null &&
                        DateTime.Now.Subtract(startTime).TotalMilliseconds < 2 * HEARTBEAT_INTERVAL)
                {
                    //wait some time
                    Thread.Sleep(100);
                }

                //set status to off
                this.Status = WebServiceStatus.Off;
            }
            catch (Exception ex)
            {
                //should never happen
                //log exception
                Manager.Log.WriteException("Unexpected error while stopping web service manager.", ex);

                //set status to fatal error
                this.Status = WebServiceStatus.FatalError;
            }
        }

        #endregion Public Management Methods


        #region Public Data Methods ***************************************************

        /// <summary>
        /// Get Song web service channel.
        /// </summary>
        /// <returns>
        /// The requested channel.
        /// Null if no channel is available.
        /// </returns>
        public ISongService GetSongChannel()
        {
            //check current status
            if (this.Status != WebServiceStatus.Connected)
            {
                //not connected to service
                return null;
            }

            //check if should keep running
            if (!keepRunning)
            {
                //not connected to service
                return null;
            }

            //song service channel
            ISongService songChannel = null;

            try
            {
                //create a channel to the running service
                songChannel = channelFactory.CreateChannel();
            }
            catch (Exception ex)
            {
                //error while testing web service connection
                //log exception
                Manager.Log.WriteException("Error while creating channel.", ex);

                //update status to disconnected
                this.Status = WebServiceStatus.Disconnected;

                //close connection
                CloseConnection();
            }

            //return channel
            return songChannel;
        }

        #endregion Public Data Methods


        #region Protected Methods *****************************************************

        /// <summary>
        /// Raises the web service status changed event.
        /// </summary>
        /// <param name="status">
        /// The new protoBuf client status.
        /// </param>
        protected virtual void OnStatusChanged(WebServiceStatus status)
        {
            // make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            WebServiceStatusEventHandler handler = StatusChanged;

            //check event
            if (handler != null)
            {
                //raise the event
                handler(status);
            }
        }

        #endregion Protected Methods


        #region Private Methods *******************************************************

        /// <summary>
        /// Close current web service connection.
        /// </summary>
        private void CloseConnection()
        {
            //get old channel factory
            ChannelFactory<ISongService> oldChannelFactory = channelFactory;

            //remove reference to channel factory
            channelFactory = null;

            try
            {
                //check olf channel factory
                if (oldChannelFactory != null)
                {
                    //lock channel factory
                    lock (oldChannelFactory)
                    {
                        //close channel
                        oldChannelFactory.Close();
                    }
                }
            }
            catch
            {
                //abort channel factory
                oldChannelFactory.Abort();
            }

            //log message
            Manager.Log.WriteInfo("Web service connection was closed.");

            //wait some time before trying to reconnect
            waitBeforeReconnecting = true;
        }

        /// <summary>
        /// Run manager by connecting to web service.
        /// </summary>
        private void KeepRunning()
        {
            //wait until client has started
            while (this.Status == WebServiceStatus.Starting)
            {
                //wait some time
                Thread.Sleep(5);
            }

            //keep running client while told so
            while (keepRunning)
            {
                //check status
                if (this.Status == WebServiceStatus.Disconnected)
                {
                    //try to connect client to web service
                    #region connect to web service

                    //set status to disconnected
                    this.Status = WebServiceStatus.Disconnected;
                    
                    //song service client
                    ISongService songClient = null;

                    try
                    {
                        //check if should wait some time before reconnecting
                        if (waitBeforeReconnecting)
                        {
                            //clear flag
                            waitBeforeReconnecting = false;

                            //wait small intervals
                            DateTime startTime = DateTime.Now;

                            //keep waiting until the reconnect interval has elapsed
                            //and while running thread should keep going on
                            while (keepRunning && this.Status == WebServiceStatus.Disconnected &&
                                DateTime.Now.Subtract(startTime).TotalMilliseconds < RECONNECT_INTERVAL)
                            {
                                //wait some time
                                Thread.Sleep(100);
                            }

                            //check if should keep running
                            if (!keepRunning)
                            {
                                //no need to reconnect
                                continue;
                            }
                        }

                        //close connection if it is open
                        CloseConnection();

                        //create host base address
                        Uri tcpBaseAddress = new Uri(string.Format(
                            "http://{0}:{1}/SongServer/SongService.svc",
                            Manager.HardSettings.ServerIP,
                            Manager.HardSettings.ServerPort));

#if (Homologation || Certification)
                        //create homologation host base address instead
                        tcpBaseAddress = new Uri(string.Format(
                            "http://{0}:{1}/SongServer2/SongService.svc",
                            Manager.HardSettings.ServerIP,
                            Manager.HardSettings.ServerPort));
#endif

                        //write message
                        Manager.Log.WriteInfo(
                            "Connecting to web service at " + tcpBaseAddress + "...");

                        //create binding for larger data transfer
                        BasicHttpBinding httpBinding = new BasicHttpBinding();
                        httpBinding.MaxReceivedMessageSize *= 1000;
                        httpBinding.MaxBufferSize = (int)httpBinding.MaxReceivedMessageSize;
                        
                        //create a channel factory
                        channelFactory = new ChannelFactory<ISongService>(
                            httpBinding, new EndpointAddress(tcpBaseAddress.ToString()));

                        //create a channel to the running service
                        songClient = channelFactory.CreateChannel();

                        //test client by getting server info
                        serverInfo = songClient.GetServerInfo();

                        //get current semester and set it to manager
                        Manager.CurrentSemester = songClient.FindCurrentSemester();

                        //update status to connected
                        this.Status = WebServiceStatus.Connected;

                        //write message
                        Manager.Log.WriteInfo("Application is connected to web service.");

                        //check if it is first connection
                        if (isFirstConnection)
                        {
                            //log server info
                            Manager.Log.WriteInfo("Song Server " + serverInfo.Version);
                            Manager.Log.WriteInfo("Database: " + serverInfo.DbConnectionString);

                            //reset flag
                            isFirstConnection = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        //could not connect client to server
                        //log message
                        Manager.Log.WriteError(
                            "Could not connect to web service.");

                        //get specific error message according to socket error code
                        string errorMessage = null;

                        //check exception type
                        if (ex is EndpointNotFoundException)
                        {
                            //get specific exception
                            EndpointNotFoundException exEnd = (EndpointNotFoundException)ex;
                            
                            //web service server was not reached
                            //log message
                            errorMessage = "Could not connect to web service. " + exEnd.Message;
                        }

                        //check specific error message
                        if (errorMessage != null)
                        {
                            //write specific error message
                            Manager.Log.WriteError(errorMessage);
                        }
                        else
                        {
                            //could not connect to service for some other reason    
                            //write generic error message and exception    
                            Manager.Log.WriteException("Unexpected error while connecting to web service. ", ex);
                        }

                        //set status to disconnected
                        this.Status = WebServiceStatus.Disconnected;

                        //ensure that connection is closed
                        CloseConnection();

                        //wait some time before trying again
                        waitBeforeReconnecting = true;
                    }
                    finally
                    {
                        //check client
                        if (songClient != null)
                        {
                            //close client
                            ((IClientChannel)songClient).Close();
                        }
                    }

                    #endregion connect to web service
                }
                else if (this.Status == WebServiceStatus.Connected)
                {
                    //test web service connection
                    #region test web service

                    //song service client
                    ISongService songClient = null;

                    try
                    {
                        //wait small intervals
                        DateTime startTime = DateTime.Now;

                        //keep waiting until the heartbest interval has elapsed
                        //and while running thread should keep going on
                        while (keepRunning && this.Status == WebServiceStatus.Connected &&
                            DateTime.Now.Subtract(startTime).TotalMilliseconds < HEARTBEAT_INTERVAL)
                        {
                            //wait some time
                            Thread.Sleep(100);
                        }

                        //check if should keep running
                        if (!keepRunning)
                        {
                            //no need to test connection
                            continue;
                        }

                        //create a channel to the running service
                        songClient = channelFactory.CreateChannel();

                        //test client
                        DateTime serverTime = songClient.GetHeartbeat();

                        //check if current semester was not set yet
                        if (Manager.CurrentSemester.Result == (int)SelectResult.Empty)
                        {
                            //try finding current semester again
                            Manager.CurrentSemester = songClient.FindCurrentSemester();
                        }
                    }
                    catch (Exception ex)
                    {
                        //error while testing web service connection
                        //log exception
                        Manager.Log.WriteException("Error while testing web service connection.", ex);

                        //update status to disconnected
                        this.Status = WebServiceStatus.Disconnected;

                        //close connection
                        CloseConnection();
                    }
                    finally
                    {
                        //check client
                        if (songClient != null)
                        {
                            //close client
                            ((IClientChannel)songClient).Close();
                        }
                    }

                    #endregion test web service
                }
                else
                {
                    //exit loop
                    break;
                }
            }

            //close connection
            CloseConnection();

            //write message
            Manager.Log.WriteVerbose("Web service manager running thread was stopped.");

            //remove reference to this thread
            thrRun = null;
        }

        #endregion Private Methods

    } //end of class WebServiceManager

} //end of namespace PnT.SongClient.Data
