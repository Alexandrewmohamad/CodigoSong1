using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Ionic.Zip;

using PnT.SongDB.Logic;

using PnT.SongClient.Data;
using PnT.SongClient.UI;

namespace PnT.SongClient.Logic
{

    #region Manager Class *************************************************************

    /// <summary>
    /// Manages all resources of this server application.
    /// Static class. Can't instantiate it.
    /// </summary>
    public static class Manager
    {

        #region Constants *************************************************************

        /// <summary>
        /// The application directory path.
        /// Used as the base path for all other directory paths.
        /// </summary>
        public const string APP_BASE_DIR_PATH = "";

        /// <summary>
        /// Output cache directory path.
        /// </summary>
        public const string CACHE_DIR_PATH = APP_BASE_DIR_PATH + "cache";

        /// <summary>
        /// Output file directory path.
        /// </summary>
        public const string FILE_DIR_PATH = APP_BASE_DIR_PATH + "files";

        /// <summary>
        /// Input image directory path.
        /// </summary>
        public const string IMAGE_DIR_PATH = APP_BASE_DIR_PATH + "images";

        /// <summary>
        /// Setting XML file path.
        /// </summary>
        public const string SETTING_FILE_PATH = APP_BASE_DIR_PATH + "settings\\settings.xml";

        /// <summary>
        /// Output log directory path.
        /// </summary>
        public const string LOG_DIR_PATH = APP_BASE_DIR_PATH + "logs";

        /// <summary>
        /// Output log file base name.
        /// </summary>
        public const string LOG_FILE_NAME = "log_{0}.txt";

        /// <summary>
        /// Output log file base name.
        /// </summary>
        public const string LOG_ZIP_FILE_NAME = "log_{0}.zip";

        /// <summary>
        /// Output log file date format.
        /// </summary>
        public const string LOG_DATE_FORMAT = "yyyy_MM_dd";

        #endregion Constants


        #region Fields ****************************************************************

        /// <summary>
        /// The log manager responsable for outputting the log messages.
        /// </summary>
        private static ThreadedLogManager logManager = null;

        /// <summary>
        /// The application web service manager responsable for 
        /// monitoring the song web service.
        /// </summary>
        private static WebServiceManager webServiceManager = null;

        /// <summary>
        /// The downloaded file manager responsable for storing these files.
        /// </summary>
        private static FileManager fileManager = null;

        /// <summary>
        /// The application main form.
        /// </summary>
        private static MainForm mainForm = null;

        /// <summary>
        /// The settings manager. Created using VS IDE.
        /// Store settings specific for each user.
        /// </summary>
        private static PnT.SongClient.Properties.Settings settingsManager = null;

        /// <summary>
        /// The hard settings manager.
        /// Store settings common to all users on this machine.
        /// </summary>
        private static PnT.SongClient.Data.SettingManager hardSettingsManager = null;

        /// <summary>
        /// Timer indicates when a day has elapsed. Generate time event.
        /// Event can be used to reset sequence number for all sessions 
        /// every midnight for example.
        /// </summary>
        private static System.Timers.Timer timDayElapsed = null;

        /// <summary>
        /// The current semester.
        /// </summary>
        private static Semester currentSemester = null;

        /// <summary>
        /// The logged on user.
        /// </summary>
        private static User logonUser = null;

        /// <summary>
        /// The list of assigned role permissions of the logged on user.
        /// </summary>
        private static List<Permission> logonPermissions = null;

        /// <summary>
        /// The impersonating user.
        /// </summary>
        private static User impersonatingUser = null;

        /// <summary>
        /// The list of assigned role permissions of the impersonating user.
        /// </summary>
        private static List<Permission> impersonatingPermissions = null;

        /// <summary>
        /// The logged on teacher. Null if logged on user is not a teacher.
        /// </summary>
        private static Teacher logonTeacher = null;

        #endregion Fields


        #region Static Constructor ****************************************************

        /// <summary>
        /// Static constructor. Create all resources.
        /// </summary>
        static Manager()
        {
            //create empty semester
            currentSemester = new Semester();
            currentSemester.SemesterId = -1;
            currentSemester.Result = (int)SelectResult.Empty;
            currentSemester.ReferenceDate = new DateTime(
                DateTime.Now.Year, DateTime.Now.Month >= 7 ? 7 : 1, 1);

            //set settings manager same as VS settings manager
            settingsManager = Properties.Settings.Default;

            //create a log manager and start it
            //log manager must be created first because other resources might
            //check if log directory exists
            DirectoryInfo logDir = new DirectoryInfo(LOG_DIR_PATH);
            if (!logDir.Exists)
            {
                try
                {
                    //create directory
                    logDir.Create();
                }
                catch
                {
                    //do nothing
                }
            }

            //create log manager
            logManager = new ThreadedLogManager(LOG_DIR_PATH + "/" + string.Format(
                LOG_FILE_NAME, DateTime.Now.ToString(LOG_DATE_FORMAT)), false, true);

            //start log manager and stop it only when closing application
            logManager.Start();

            //register to aplication thread exception event
            System.Windows.Forms.Application.ThreadException +=
                new ThreadExceptionEventHandler(Application_ThreadException);

            //register to current domain unhandled exception event
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //create hard settings manager
            hardSettingsManager = new SettingManager();

            //load hard settings
            if (!hardSettingsManager.Load())
            {
                //could not load settings
                logManager.WriteError(Properties.Resources.errorLoadSettings);

                //exit application
                System.Windows.Forms.Application.Exit();

                //exit constructor
                return;
            }

            //create the web service manager
            webServiceManager = new Data.WebServiceManager();

            //create the file manager
            fileManager = new Data.FileManager();

            //create day elapsed timer, set elapsed time event handler,
            //set initial time interval and start it
            timDayElapsed = new System.Timers.Timer();
            timDayElapsed.Elapsed +=
                new System.Timers.ElapsedEventHandler(timDayElapsed_Elapsed);
            timDayElapsed.Interval = 30000.0;
            timDayElapsed.Start();
        }

        #endregion Static Constructor


        #region Properties ************************************************************

        /// <summary>
        /// Get/set the current semester.
        /// </summary>
        public static Semester CurrentSemester
        {
            get
            {
                return currentSemester;
            }
            set
            {
                //check value
                if (value != null && value.Result == (int)SelectResult.Success)
                {
                    //update current semester
                    currentSemester = value;
                }
            }
        }

        /// <summary>
        /// Get the downloaded file manager.
        /// </summary>
        public static FileManager FileManager
        {
            get
            {
                return Manager.fileManager;
            }
        }

        /// <summary>
        /// Get the application hard settings manager.
        /// </summary>
        internal static PnT.SongClient.Data.SettingManager HardSettings
        {
            get { return Manager.hardSettingsManager; }
        }

        /// <summary>
        /// Get the log manager responsable for outputting the log messages.
        /// </summary>
        public static ThreadedLogManager Log
        {
            get { return Manager.logManager; }
        }

        /// <summary>
        /// Get/set the application main form.
        /// </summary>
        public static MainForm MainForm
        {
            get { return Manager.mainForm; }
            set { Manager.mainForm = value; }
        }

        /// <summary>
        /// Get the application settings manager.
        /// </summary>
        internal static PnT.SongClient.Properties.Settings Settings
        {
            get { return Manager.settingsManager; }
        }

        /// <summary>
        /// Get the current application version.
        /// </summary>
        public static string Version
        {
            get
            {
                //get assembly version
                Version version = Assembly.GetExecutingAssembly().GetName().Version;

                //return current version
                return version.Major + "." + version.Minor + "." + version.Build;
            }
        }

        /// <summary>
        /// Get the application web serivce manager.
        /// </summary>
        public static WebServiceManager WebServiceManager
        {
            get { return Manager.webServiceManager; }
        }

        /// <summary>
        /// Get the logged on user.
        /// </summary>
        public static User LogonUser
        {
            get
            {
                return logonUser;
            }
        }

        /// <summary>
        /// Get the impersonating user.
        /// </summary>
        public static User ImpersonatingUser
        {
            get
            {
                return impersonatingUser;
            }
        }

        /// <summary>
        /// Get the user accounted for audit records.
        /// </summary>
        public static User AccountedUser
        {
            get
            {
                return (impersonatingUser != null) ? impersonatingUser : logonUser;
            }
        }

        /// <summary>
        /// The logged on teacher. 
        /// Null if logged on user is not a teacher.
        /// </summary>
        public static Teacher LogonTeacher
        {
            get
            {
                return logonTeacher;
            }
        }

        #endregion Properties


        #region Public Methods ********************************************************

        /// <summary>
        /// Dispose application resources. Used before closing application.
        /// </summary>
        public static void DisposeResources()
        {
            //stop day elapsed timer
            timDayElapsed.Stop();

            //set culture to selected restart culture
            settingsManager.Culture = settingsManager.RestartCulture;

            //save application settings so it can be loaded on restart
            settingsManager.Save();

            //save hard settings
            hardSettingsManager.Save();

            //stop web service manager
            WebServiceManager.Stop();

            //stop log manager
            //must be the last one sice other managers might write to log
            logManager.Stop();
        }

        /// <summary>
        /// Format instrument description to display its type name instead of it type code.
        /// </summary>
        /// <param name="instrument">
        /// The instrument description to be set.
        /// </param>
        public static void FormatInstrumentDescription(IdDescriptionStatus instrument)
        {
            //check if instrument is already set
            if (instrument.Description.IndexOf('#') < 0)
            {
                //no need to set instrument
                //exit
                return;
            }

            //split description
            string[] words = instrument.Description.Split(new char[] { '#' });

            //check result
            if (words.Length != 2)
            {
                //should never happen
                //cannot set instrument
                //exit
                return;
            }

            //get instrument type value
            int instrumentType = (int)Enum.Parse(typeof(InstrumentsType), words[0]);

            //set new description
            instrument.Description = Properties.Resources.ResourceManager.GetString(
                "InstrumentsType_" + ((InstrumentsType)instrumentType).ToString()) +
                " " + words[1];
        }

        /// <summary>
        /// Get complete class description to display for selected class.
        /// </summary>
        /// <param name="classObj">
        /// The selected class.
        /// </param>
        /// <param name="addTeacher">
        /// True to add teacher name.
        /// False otherwise.
        /// </param>
        /// <returns>
        /// The complete class description.
        /// </returns>
        public static string GetClassDescription(Class classObj, bool addTeacher)
        {
            //check class
            if (classObj == null)
            {
                //no class
                return string.Empty;
            }

            //gather week days
            StringBuilder sbDays = new StringBuilder(8);
            if (classObj.WeekMonday)
            {
                sbDays.Append(Properties.Resources.dayShortMondays);
                sbDays.Append(", ");
            }
            if (classObj.WeekTuesday)
            {
                sbDays.Append(Properties.Resources.dayShortTuesdays);
                sbDays.Append(", ");
            }
            if (classObj.WeekWednesday)
            {
                sbDays.Append(Properties.Resources.dayShortWednesdays);
                sbDays.Append(", ");
            }
            if (classObj.WeekThursday)
            {
                sbDays.Append(Properties.Resources.dayShortThursdays);
                sbDays.Append(", ");
            }
            if (classObj.WeekFriday)
            {
                sbDays.Append(Properties.Resources.dayShortFridays);
                sbDays.Append(", ");
            }
            if (classObj.WeekSaturday)
            {
                sbDays.Append(Properties.Resources.dayShortSaturdays);
                sbDays.Append(", ");
            }
            if (classObj.WeekSunday)
            {
                sbDays.Append(Properties.Resources.dayShortSundays);
                sbDays.Append(", ");
            }

            //check result
            if (sbDays.Length > 2)
            {
                //remove last ", "
                sbDays.Length -= 2;
            }

            //create description
            StringBuilder sbDescription = new StringBuilder(64);

            //add class code
            sbDescription.Append(classObj.Code);
            sbDescription.Append(" | ");

            //add class or instrument type
            sbDescription.Append(classObj.ClassType == (int)ClassType.Instrument ?
                Properties.Resources.ResourceManager.GetString(
                    "InstrumentsType_" + ((InstrumentsType)classObj.InstrumentType).ToString()) :
                Properties.Resources.ResourceManager.GetString(
                    "ClassType_" + ((ClassType)classObj.ClassType).ToString()));
            sbDescription.Append(" | ");

            //check if teacher should be added
            if (addTeacher && classObj.TeacherName != null && classObj.TeacherName.Length > 0)
            {
                //add teacher
                sbDescription.Append(classObj.TeacherName);
                sbDescription.Append(" | ");
            }

            //add class level
            sbDescription.Append(Properties.Resources.ResourceManager.GetString(
                "ClassLevel_" + ((ClassLevel)classObj.ClassLevel).ToString()));
            sbDescription.Append(" | ");

            //add class days and start time
            sbDescription.Append(sbDays.ToString());
            sbDescription.Append(" ");
            sbDescription.Append(classObj.StartTime.ToString("HH:mm"));

            //return class description
            return sbDescription.ToString();
        }

        /// <summary>
        /// Set the current logged on user.
        /// </summary>
        /// <param name="user">
        /// The current logged on user.
        /// </param>
        /// <param name="permissions">
        /// The assigned role permissions of the current logged on user.
        /// </param>
        public static void SetLogonUser(User user, List<Permission> permissions)
        {
            //set fields
            SetLogonUser(user, permissions, null, null);
        }

        /// <summary>
        /// Set the current logged on user with impersonating user.
        /// </summary>
        /// <param name="user">
        /// The current logged on user.
        /// </param>
        /// <param name="permissions">
        /// The assigned role permissions of the current logged on user.
        /// </param>
        /// <param name="impersonatingUser">
        /// The impersonating user.
        /// </param>
        /// <param name="impersonatingPermissions">
        /// The assigned role permissions of the current impersonating user.
        /// </param>
        public static void SetLogonUser(
            User user, List<Permission> permissions, 
            User impersonatingUser, List<Permission> impersonatingPermissions)
        {
            //set fields
            Manager.logonUser = user;
            Manager.logonPermissions = permissions != null ? 
                permissions : new List<Permission>();
            Manager.impersonatingUser = impersonatingUser;
            Manager.impersonatingPermissions = impersonatingPermissions != null ?
                impersonatingPermissions : new List<Permission>();

            //reset user teacher
            Manager.logonTeacher = null;

            //check if user is a teacher
            if (HasLogonPermission("Class.Teacher"))
            {
                //load user teacher
                #region load teacher

                //get song channel
                SongServer.ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

                //check result
                if (songChannel == null)
                {
                    //channel is not available at the moment
                    //could not load data
                    return;
                }

                try
                {
                    //find teacher by user id and load all data
                    Teacher teacher = songChannel.FindTeacherByUser(
                        logonUser.UserId, true, true);

                    //check result
                    if (teacher.Result == (int)SelectResult.Success)
                    {
                        //set logon user
                        Manager.logonTeacher = teacher;
                    }
                }
                catch (Exception ex)
                {
                    //log exception
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorChannelLoadData,
                        Properties.Resources.item_Teacher, ex.Message));
                    Manager.Log.WriteException(ex);
                }
                finally
                {
                    //check channel
                    if (songChannel != null)
                    {
                        //close channel
                        ((System.ServiceModel.IClientChannel)songChannel).Close();
                    }
                }

                #endregion load teacher
            }
        }

        /// <summary>
        /// Reset current logged on user.
        /// </summary>
        public static void ResetLogonUser()
        {
            //reset fields
            logonUser = null;
            impersonatingUser = null;
            logonPermissions = new List<Permission>();
            Manager.impersonatingPermissions = new List<Permission>();
            Manager.logonTeacher = null;
        }

        /// <summary>
        /// Check if selected permission is granted to current logged on user.
        /// </summary>
        /// <param name="name">
        /// The name of the selected permission.
        /// </param>
        /// <returns>
        /// True if permissions is granted to logged on user.
        /// False otherwise.
        /// </returns>
        public static bool HasLogonPermission(string name)
        {
            //get reference to list of permissions
            List<Permission> permissions = logonPermissions;

            //check list
            if (permissions == null || permissions.Count == 0)
            {
                //no permission
                return false;
            }

            //return if permission is granted
            return (permissions.Find(p => p.Name.Equals(name)) != null);
        }

        /// <summary>
        /// Get the list of assigned role permissions of the logged on user.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public static List<Permission> ListLogonPermissions
        {
            get
            {
                //get reference to list of permissions
                List<Permission> permissions = logonPermissions;

                //check list
                if (permissions == null || permissions.Count == 0)
                {
                    //no permission
                    return new List<Permission>();
                }

                //copy and return list of permissions
                return new List<Permission>(permissions);
            }
        }

        /// <summary>
        /// Get the list of assigned role permissions of the impersonating user.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public static List<Permission> ListImpersonatingPermissions
        {
            get
            {
                //get reference to list of permissions
                List<Permission> permissions = impersonatingPermissions;

                //check list
                if (permissions == null || permissions.Count == 0)
                {
                    //no permission
                    return new List<Permission>();
                }

                //copy and return list of permissions
                return new List<Permission>(permissions);
            }
        }

        #endregion Public Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Day elapsed timer has elapsed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void timDayElapsed_Elapsed(
            object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //set thread culture
                Thread.CurrentThread.CurrentCulture = Manager.Settings.Culture;
                Thread.CurrentThread.CurrentUICulture = Manager.Settings.Culture;

                //stop timer
                timDayElapsed.Stop();

                try
                {
                    //check if current time is midnight on local time
                    //margin of five minutes
                    //check elapsed time since midnight
                    if (DateTime.Now.TimeOfDay.TotalSeconds < 300)
                    {
                        //it is midnight at local time
                        //change log file everyday
                        #region change log file

                        //create new application log file name
                        string logFileName = string.Format(
                            LOG_FILE_NAME, DateTime.Now.ToString(LOG_DATE_FORMAT));

                        //change application log file
                        logManager.ChangeLogFile(LOG_DIR_PATH + "/" + logFileName);

                        try
                        {
                            //zip old log files
                            //get list of all txt files on the log directory
                            string[] filePaths = Directory.GetFiles(
                                LOG_DIR_PATH + "/", "*.txt");

                            //check each txt file
                            foreach (string filePath in filePaths)
                            {
                                //check if it is the current log file
                                if (filePath.EndsWith(logFileName))
                                {
                                    //it is the current log file
                                    //no need to zip it
                                    //delete matching zip file if it exists already
                                    if (System.IO.File.Exists(filePath.Replace(".txt", ".zip")))
                                    {
                                        //delete matching zip file
                                        System.IO.File.Delete(filePath.Replace(".txt", ".zip"));
                                    }

                                    //go to the next txt file
                                    continue;
                                }

                                //check if file already has a zip file
                                if (System.IO.File.Exists(filePath.Replace(".txt", ".zip")))
                                {
                                    //there is already a corresponding zip file
                                    //no need to zip it again
                                    continue;
                                }

                                //create new zip file
                                using (ZipFile zipLog = new ZipFile())
                                {
                                    //set option to use zip 64 format for larger files
                                    zipLog.UseZip64WhenSaving = Zip64Option.AsNecessary;

                                    //add old txt file to zip file
                                    zipLog.AddFile(filePath, "");

                                    //save zip file
                                    zipLog.Save(
                                        filePath.Replace(".txt", ".zip"));
                                }

                                //delete txt file
                                System.IO.File.Delete(filePath);
                            }
                        }
                        catch (Exception ex)
                        {
                            //error file zipping old log file
                            //unsolved error that can happen with Ionic Zip
                            Log.WriteException(Properties.Resources.errorOnZipOldLog, ex);
                        }

                        #endregion change log file
                    }
                }
                catch (Exception ex)
                {
                    //an exception occurred while resetting sessions
                    //log exception
                    Manager.Log.WriteException(
                        "Error while executing tasks at 00:00:00 GMT.", ex);
                }
                finally
                {
                    //restart day elapsed timer no matter what
                    //set interval to (24h - deltaTime)
                    timDayElapsed.Interval = (24.0 * 60.0 * 60.0 * 1000.0) -
                        DateTime.Now.TimeOfDay.TotalMilliseconds;

                    //restart timer
                    timDayElapsed.Start();
                }
            }
            catch (Exception ex)
            {
                Log.WriteException("Error while setting day timer " +
                    "to elapse at 00:00:00 GMT.", ex);
            }
        }

        /// <summary>
        /// Application thread exception event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(
            object sender, ThreadExceptionEventArgs args)
        {
            //check if log manager is available
            if (Manager.Log == null)
            {
                //log manager is not available
                //exit
                return;
            }

            //log exception
            Manager.Log.WriteException(
                "An application thread exception was caught by " +
                "Manager. Contact support immediately.", args.Exception);
        }

        /// <summary>
        /// Current domain unhandled exception event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(
            object sender, UnhandledExceptionEventArgs args)
        {
            //check if log manager is available
            if (Manager.Log == null)
            {
                //log manager is not available
                //exit
                return;
            }

            //log exception
            Manager.Log.WriteError(
                "An unhandled exception was caught by Manager " +
                "on current domain. Contact support immediately.");

            //cast exception
            Exception ex = args.ExceptionObject as Exception;

            //check result
            if (ex != null)
            {
                //log exception
                Manager.Log.WriteException(ex);
            }
            else
            {
                //log generic messages
                Manager.Log.WriteError(args.ExceptionObject.ToString());
                Manager.Log.WriteError(args.ExceptionObject.GetType().ToString());
            }
        }

        #endregion Event Handlers


    } //end of class Manager

    #endregion Manager Class

} //end of namespace PnT.SongClient.Logic
