using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

using PnT.SongDB;


namespace PnT.SongServer
{

    /// <summary>
    /// This class holds the application settings.
    /// </summary>
    public static class ApplicationSettings
    {
        
        #region Fields ****************************************************************

        /// <summary>
        /// The database connection string.
        /// </summary>
        private static string dbConnectionString = string.Empty;

        /// <summary>
        /// The database timeout.
        /// </summary>
        private static int dbTimeout = 60;

        /// <summary>
        /// The SMTP server address.
        /// </summary>
        private static string smtpAddress = string.Empty;

        /// <summary>
        /// The SMTP server port number.
        /// </summary>
        private static int smtpPort = int.MinValue;

        /// <summary>
        /// The option to use SSL while connecting to SMTP server.
        /// </summary>
        private static bool smtpSSL = true;

        /// <summary>
        /// The SMTP server user name.
        /// </summary>
        private static string smtpUser = string.Empty;

        /// <summary>
        /// The SMTP server user password.
        /// </summary>
        private static string smtpPassword = string.Empty;

        /// <summary>
        /// The SMTP from address.
        /// </summary>
        private static string smtpFrom = string.Empty;

        /// <summary>
        /// The SMTP display Name.
        /// </summary>
        private static string smtpDisplayName = string.Empty;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ApplicationSettings()
        {
            //load song database settings
            #region database settings

            try
            {
                //set database connection string
                dbConnectionString = ConfigurationManager.AppSettings["SongDBConnectionString"];
            }
            catch
            {
                //reset database connection string
                dbConnectionString = string.Empty;
            }

            try
            {
                //set database timeout
                dbTimeout = int.Parse(ConfigurationManager.AppSettings["SongDBTimeout"]);
            }
            catch
            {
                //set default database timeout
                dbTimeout = 60;
            }

            #endregion database settings

            //load smtp email settings
            #region email settings

            try
            {
                //set smtp server address
                smtpAddress = ConfigurationManager.AppSettings["SMTPAddress"];
            }
            catch
            {
                //reset smtp server address
                smtpAddress = string.Empty;
            }

            try
            {
                //set smtp server port
                smtpPort = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
            }
            catch
            {
                //set default smtp server port
                smtpPort = 587;
            }

            try
            {
                //set smtp ssl option
                smtpSSL = bool.Parse(ConfigurationManager.AppSettings["SMTPSSL"]);
            }
            catch
            {
                //set default smtp ssl option
                smtpSSL = true;
            }

            try
            {
                //set smtp user name
                smtpUser = ConfigurationManager.AppSettings["SMTPUser"];
            }
            catch
            {
                //reset smtp user name
                smtpUser = string.Empty;
            }

            try
            {
                //set smtp user password
                smtpPassword = ConfigurationManager.AppSettings["SMTPPassword"];
            }
            catch
            {
                //reset smtp user password
                smtpPassword = string.Empty;
            }

            try
            {
                //set smtp from address
                smtpFrom = ConfigurationManager.AppSettings["SMTPFrom"];
            }
            catch
            {
                //reset smtp from address
                smtpFrom = string.Empty;
            }

            try
            {
                //set smtp display Name
                smtpDisplayName = ConfigurationManager.AppSettings["SMTPDisplayName"];
            }
            catch
            {
                //reset smtp display Name
                smtpDisplayName = string.Empty;
            }

            #endregion email settings
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get the SMTP server address.
        /// </summary>
        public static string SmtpAddress
        {
            get
            {
                return smtpAddress;
            }
        }

        /// <summary>
        /// Get the SMTP server port number.
        /// </summary>
        public static int SmtpPort
        {
            get
            {
                return smtpPort;
            }
        }

        /// <summary>
        /// Get the option to use SSL while connecting to SMTP server.
        /// </summary>
        public static bool SmtpSSL
        {
            get
            {
                return smtpSSL;
            }
        }

        /// <summary>
        /// Get the SMTP server user name.
        /// </summary>
        public static string SmtpUser
        {
            get
            {
                return smtpUser;
            }
        }

        /// <summary>
        /// Get the SMTP server user password.
        /// </summary>
        public static string SmtpPassword
        {
            get
            {
                return smtpPassword;
            }
        }

        /// <summary>
        /// Get the SMTP from address.
        /// </summary>
        public static string SmtpFrom
        {
            get
            {
                return smtpFrom;
            }
        }

        /// <summary>
        /// Get the SMTP display Name.
        /// </summary>
        public static string SmtpDisplayName
        {
            get
            {
                return smtpDisplayName;
            }
        }

        #endregion Properties


        #region Public Methods ********************************************************

        /// <summary>
        /// Apply settings to service.
        /// </summary>
        /// <returns></returns>
        public static bool ApplySettings()
        {
            //check if connection settings is already set
            if (ConnectionSettings.SongDBConnectionString != null &&
                ConnectionSettings.SongDBConnectionString.Length > 0 &&
                ConnectionSettings.SongDBConnectionString.Equals(dbConnectionString))
            {
                //settings were applied
                return true;
            }

            //check if settings were not loaded
            if (dbConnectionString == null || dbConnectionString.Length == 0)
            {
                //settings were not loaded
                return false;
            }

            //apply settings
            ConnectionSettings.SongDBConnectionString = dbConnectionString;
            ConnectionSettings.SongDBTimeout = dbTimeout;

            //settings were applied
            return true;
        }

        #endregion Public Methods

    } //end of static class DatabaseSettings

} //end of namespace PnT.SongServer