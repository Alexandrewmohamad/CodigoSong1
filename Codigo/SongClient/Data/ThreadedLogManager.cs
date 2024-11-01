using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;


namespace PnT.SongClient.Data
{

    /// <summary>
    /// Enumerate all message types.
    /// Error - used when an error or unexpected exception is raised.
    /// Warning - used when a pontential error situation is detected.
    /// Message - important messages about the system.
    /// Verbose - verbose messages with minor importance.
    /// Execution - execution detail messages, does not go into log textbox.
    /// </summary>
    public enum LogLevel { Error, Warning, Information, Verbose, Execution };


    /// <summary>
    /// Manages log writing. Log messages are written to a text file 
    /// by another thread.
    /// </summary>
    public class ThreadedLogManager
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The output file message streamwriter.
        /// </summary>
        private StreamWriter swFileMessages = null;
        
        /// <summary>
        /// The output log file path.
        /// </summary>
		private string logFilePath = null;

        /// <summary>
        /// Option to write date to each log entry.
        /// </summary>
        private bool printDate = true;

        /// <summary>
        /// Option to print miliseconds to each log entry.
        /// </summary>
        private bool printMiliSeconds = false;

        /// <summary>
        /// The queue of log messages that will be written.
        /// </summary>
        private Queue<string> logMessages = null;

        /// <summary>
        /// The queue of log levels of the messages that will be written.
        /// </summary>
        private Queue<LogLevel> logLevels = null;

        /// <summary>
        /// The thread responsable for writting the log messages.
        /// </summary>
        private Thread thrWriter = null;

        /// <summary>
        /// Flag that indicates that the writer thread should keep writing.
        /// </summary>
        private volatile bool keepWriting = false;

        #endregion Fields


        #region Delegates *************************************************************

        /// <summary>
        /// This delegate can be used to invoke WriteLogMessage with invoke from other threads.
        /// </summary>
        public delegate void WriteLogMessageDelegate(string message, LogLevel level);

        #endregion Delegates


        #region Constructors **********************************************************

        /// <summary>
		/// Default constructor.
		/// </summary>
        public ThreadedLogManager()
        {
            //create queue of log messages
            logMessages = new Queue<string>();

            //create related queue of log levels
            logLevels = new Queue<LogLevel>();
        }

        /// <summary>
        /// Constructor. Set ouput log file path.
        /// </summary>
        public ThreadedLogManager(string logFilePath)
        {
            //create queue of log messages
            logMessages = new Queue<string>();

            //create related queue of log levels
            logLevels = new Queue<LogLevel>();

            //set log file path
            this.logFilePath = logFilePath;
        }

        /// <summary>
        /// Constructor. Set ouput log file path, option to print date and
        /// option to print miliseconds
        /// </summary>
        public ThreadedLogManager(string logFilePath,
            bool printDate, bool printMiliSeconds)
        {
            //create queue of log messages
            logMessages = new Queue<string>();

            //create related queue of log levels
            logLevels = new Queue<LogLevel>();

            //set log file path
            this.logFilePath = logFilePath;

            //set option to print date
            this.printDate = printDate;

            //set option to print miliseconds
            this.printMiliSeconds = printMiliSeconds;
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get the number of queued log messages.
        /// </summary>
        private int CountLogMessages
        {
            get
            {
                //lock queue
                lock (logMessages)
                {
                    //clear queue
                    return logMessages.Count;
                }
            }
        }

        /// <summary>
        /// Check if log is open for writing.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                //return is there is a stream writer
                return (swFileMessages != null);
            }
        }

        /// <summary>
        /// Get the log file path.
        /// </summary>
        public string LogFilePath
        {
            get { return logFilePath; }
        }

        /// <summary>
        /// Get/set the option to print date to each log entry.
        /// </summary>
        public bool PrintDate
        {
            get { return printDate; }
            set { printDate = value; }
        }

        /// <summary>
        /// Get/set the option to print miliseconds to each log entry.
        /// </summary>
        public bool PrintMiliSeconds
        {
            get { return printMiliSeconds; }
            set { printMiliSeconds = value; }
        }

        #endregion Properties 


        #region Public Administrative Methods *****************************************

        /// <summary>
		/// Start the service by opening the log file.
		/// </summary>
		/// <returns>False if an error occurred while initializing resource resources.</returns>
		public bool Start()
        {
            //check stream writer
            if (swFileMessages != null)
            {
                //log manager is already running
                return true;
            }

            //check log file path
            if (logFilePath != null)
            {
                //open stream
                try
                {
                    //clear log messages queue
                    ClearLogMessages();

                    //append to end of file
                    swFileMessages = new StreamWriter(logFilePath, true);

                    //set keep writing flag
                    keepWriting = true;

                    //create, set and start writer thread
                    thrWriter = new Thread(WriteMessages);
                    thrWriter.IsBackground = true;
                    thrWriter.Start();
                }
                catch
                {
                    //could not write to file
                    if (swFileMessages != null)
                    {
                        swFileMessages.Close();
                    }
                    swFileMessages = null;

                    //can't start log manager
                    return false;
                }
            }
            else
            {
                //no log file path
                //can't start log manager
                return false;
            }

            //log is ready
            return true;
        }

        /// <summary>
        /// Stop log manager and close output log file.
        /// </summary>
        public void Stop()
        {
            //wait until writer thread is done, but not forever
            //count total waited time
            int totalTime = 0;

            //check writer thread is running, if manager is writing
            //and queue has any message
            while (thrWriter != null && keepWriting && CountLogMessages > 0 && totalTime < 2000)
            {
                //wait until all messages are written
                Thread.Sleep(50);

                //add waited time
                totalTime += 50;
            }

            //reset keep writing flag and wait some time
            keepWriting = false;
            Thread.Sleep(50);

            //get current reference to writer
            StreamWriter swOldFileMessages = swFileMessages;

            //remove reference to writer
            swFileMessages = null;

            //check old writer
            if (swOldFileMessages != null)
            {
                //close output file log
                //lock writer to prevent other threads from using it
                lock (swOldFileMessages)
                {
                    //close old writer
                    swOldFileMessages.Flush();
                    swOldFileMessages.Close();
                }
            }
        }

        /// <summary>
        /// Change log file by opening a new file and closing the old one.
        /// </summary>
        /// <param name="newLogFilePath">The new log file path to be used.</param>
        /// <returns>True if log file was replaced. False if it was not replaced and old one is still in use.</returns>
        public bool ChangeLogFile(string newLogFilePath)
        {
            //check new file path
            if (newLogFilePath == null ||
                newLogFilePath.Length == 0)
            {
                //no new log file path
                //could not change log file
                return false;
            }

            //check if new file is the same old file
            if (newLogFilePath.Equals(logFilePath))
            {
                //same file
                //no need to change log file
                return true;
            }

            //check stream writer is not open
            if (swFileMessages == null)
            {
                //no need to create another stream writer
                //just set log file path
                logFilePath = newLogFilePath;

                //new log file path was set
                return true;
            }

            //create stream writer for new file
            StreamWriter swNewFileMessages = null;

            //close old stream writer
            StreamWriter swOldFileMessages = null;

            try
            {
                //get old stream writer
                swOldFileMessages = swFileMessages;

                //open and append to end of new file
                swNewFileMessages = new StreamWriter(newLogFilePath, true);

                //set new stream writer
                swFileMessages = swNewFileMessages;

                //lock writer to prevent other threads from using it
                lock (swOldFileMessages)
                {
                    //close stream
                    swOldFileMessages.Flush();
                    swOldFileMessages.Close();
                }

                //update log file path
                logFilePath = newLogFilePath;

                //log file was replaced
                return true;
            }
            catch
            {
                //could not write to file
                if (swNewFileMessages != null)
                {
                    swNewFileMessages.Close();
                }
                swNewFileMessages = null;

                //could not change log file
                return false;
            }
        }
        #endregion


        #region Public Methods ********************************************************

        /// <summary>
        /// Write the message with a given level into log system.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        /// <param name="level">The message's level.</param>
        public void WriteLogMessage(string message, LogLevel level)
        {
            //create a string builder to format message
            StringBuilder sbMessage = new StringBuilder(message.Length + 32);

            //check if should print date to message
            if (printDate)
            {
                //add date to message
                sbMessage.Append(DateTime.Now.ToShortDateString());
                sbMessage.Append(", ");
            }

            //add time to message
            //check if should add miliseconds to it
            if (printMiliSeconds)
            {
                sbMessage.Append(DateTime.Now.ToString("HH:mm:ss.fff - "));
            }
            else
            {
                sbMessage.Append(DateTime.Now.ToString("HH:mm:ss - "));
            }

            //add message
            sbMessage.Append(message);

            //add created log message to the queue
            AddLogMessage(sbMessage.ToString(), level);
        }

        /// <summary>
        /// Write an error message into log system.
        /// </summary>
        /// <param name="message">The error message.</param>
        public void WriteError(string message)
        {
            WriteLogMessage(message, LogLevel.Error);
        }

        /// <summary>
        /// Write an execution message into log system.
        /// Execution messages are not displayed in log textbox control.
        /// </summary>
        /// <param name="message">The error message.</param>
        public void WriteExecution(string message)
        {
            WriteLogMessage(message, LogLevel.Execution);
        }

        /// <summary>
        /// Write a information message into log system.
        /// </summary>
        /// <param name="message">The information message.</param>
        public void WriteInfo(string message)
        {
            WriteLogMessage(message, LogLevel.Information);
        }

        /// <summary>
        /// Write a verbose message into log system.
        /// </summary>
        /// <param name="message">The verbose message.</param>
        public void WriteVerbose(string message)
        {
            WriteLogMessage(message, LogLevel.Verbose);
        }

        /// <summary>
        /// Write a warning message into log system.
        /// </summary>
        /// <param name="message">The warning message.</param>
        public void WriteWarning(string message)
        {
            WriteLogMessage(message, LogLevel.Warning);
        }

        /// <summary>
        /// Write messages for a given exception.
        /// </summary>
        /// <param name="ex">The raised exception.</param>
        public void WriteException(Exception ex)
        {
            //write exception with empty custom message
            WriteException(string.Empty, ex);
        }

        /// <summary>
        /// Write messages for a given exception.
        /// </summary>
        /// <param name="message">A custom message to the exception.</param>
        /// <param name="ex">The raised exception.</param>
        public void WriteException(string message, Exception ex)
        {
            //log exception
            //write custom message as an error message
            if (message != null && !message.Equals(string.Empty))
            {
                WriteError(message);
            }

            //write exception message as an error message
            WriteError(ex.Message);

            //write exception type and stack trace as verbose message
            WriteVerbose(ex.GetType().ToString() + ex.StackTrace);

            //get inner exception
            Exception innerExpcetion = ex.InnerException;

            //log inner exception while there is any
            while (innerExpcetion != null)
            {
                //write exception message as an error message
                WriteError(innerExpcetion.Message);

                //write exception type and stack trace as verbose message
                WriteVerbose(innerExpcetion.GetType().ToString() + innerExpcetion.StackTrace);

                //update inner exception
                innerExpcetion = innerExpcetion.InnerException;
            }
        }

        #endregion


        #region Private Methods *******************************************************

        /// <summary>
        /// Add a log message to the queue of log messages to be written.
        /// </summary>
        /// <param name="logMessage">
        /// The new log message to be written.
        /// </param>
        private void AddLogMessage(string message, LogLevel level)
        {
            //lock queue
            lock (logMessages)
            {
                //add message
                logMessages.Enqueue(message);

                //add log level ro related queue
                logLevels.Enqueue(level);
            }
        }

        /// <summary>
        /// Clear queued log messages.
        /// </summary>
        private void ClearLogMessages()
        {
            //lock queue
            lock (logMessages)
            {
                //clear queue
                logMessages.Clear();
            }
        }

        /// <summary>
        /// Write queued log messages.
        /// </summary>
        private void WriteMessages()
        {
            try
            {
                //write each log message to its due log level
                string logMessage = null;
                LogLevel logLevel;

                //check stream writer
                while (keepWriting)
                {
                    //check if there is any log message
                    if (CountLogMessages == 0)
                    {
                        //there is no log message
                        //wait some time
                        Thread.Sleep(50);

                        //check again
                        continue;
                    }

                    //lock queue
                    lock (logMessages)
                    {
                        //get next log message and its log level
                        logMessage = logMessages.Dequeue();
                        logLevel = logLevels.Dequeue();
                    }

                    //write to file
                    if (swFileMessages != null)
                    {
                        //lock writer to other threads
                        lock (swFileMessages)
                        {
                            swFileMessages.WriteLine(logMessage);
                            swFileMessages.Flush();
                        }
                    }
                }
            }
            catch
            {
                //unexpected error while writing message
                //signal that this thread will stop
                keepWriting = false;

                try
                {
                    //get current reference to writer
                    StreamWriter swOldFileMessages = swFileMessages;

                    //remove reference to writer
                    swFileMessages = null;

                    //check old writer
                    if (swOldFileMessages != null)
                    {
                        //close output file log
                        //lock writer to prevent other threads from using it
                        lock (swOldFileMessages)
                        {
                            //close old writer
                            swOldFileMessages.Flush();
                            swOldFileMessages.Close();
                        }
                    }
                }
                catch
                {
                    //do nothing
                }
            }
            finally
            {
                //clear remaining messages
                ClearLogMessages();

                //remove reference to this thread
                thrWriter = null;
            }
        }

        #endregion Private Methods

    } //end of class ThreadedLogManager

} //end of PnT.SongClient.Data