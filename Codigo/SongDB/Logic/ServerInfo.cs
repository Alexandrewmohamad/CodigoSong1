using System;
using System.Runtime.Serialization;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Contains server related information of the Song Server.
    /// </summary>
    [DataContract]
    public class ServerInfo
    {

        #region Fields *****************************************************************

        /// <summary>
        /// The version of the server.
        /// </summary>
        private string version = string.Empty;

        /// <summary>
        /// The database connection string.
        /// </summary>
        private string dbConnectionString = string.Empty;

        #endregion Fields


        #region Constructors ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ServerInfo()
        {
        }

        #endregion Constructors


        #region Properties *************************************************************

        /// <summary>
        /// Get/set the database connection string.
        /// </summary>
        [DataMember]
        public string DbConnectionString
        {
            get
            {
                return dbConnectionString;
            }

            set
            {
                dbConnectionString = value;
            }
        }

        /// <summary>
        /// Get/set the version of the server.
        /// </summary>
        [DataMember]
        public string Version
        {
            get
            {
                return version;
            }

            set
            {
                version = value;
            }
        }

        #endregion Properties

    } //end of class ServerInfo

} //end of namespace PnT.SongDB.Logic
