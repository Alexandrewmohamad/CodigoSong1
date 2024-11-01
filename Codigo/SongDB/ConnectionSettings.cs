using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PnT.SongDB
{

    /// <summary>
    /// This class manages the DB connection settings.
    /// </summary>
    public static class ConnectionSettings
    {

        /// <summary>
        /// The connection string for the Song DB.
        /// </summary>
        private static string songDBConnectionString = "Must be passed by application";

        /// <summary>
        /// The command timeout for the Song DB. In seconds.
        /// </summary>
        private static int songDBTimeout = 60;

        /// <summary>
        /// Get/set the connection string for the Song DB.
        /// </summary>
        public static string SongDBConnectionString
        {
            get
            {
                //get connection string
                return songDBConnectionString;
            }
            set
            {
                //set connection string
                songDBConnectionString = value;
            }
        }

        /// <summary>
        /// Get/set the command timeout for the Song DB. In seconds.
        /// </summary>
        public static int SongDBTimeout
        {
            get
            {
                //get command timeout
                return songDBTimeout;
            }
            set
            {
                //set command timeout
                songDBTimeout = value;
            }
        }

    } //end of  public static class ConnectionSettings

} //end of namespace PnT.SongDB
