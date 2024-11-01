using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace PnT.SongDB.Logic
{
    /// <summary>
    /// Contains the the result of a save (insert or update) operation.
    /// </summary>
    [DataContract]
    public class SaveResult
    {

        #region Fields *****************************************************************

        /// <summary>
        /// The ID of the saved row in the database.
        /// </summary>
        private int savedId = -1;

        /// <summary>
        /// The database select result.
        /// </summary>
        private int result;

        /// <summary>
        /// The database select error message.
        /// </summary>
        private string errorMessage = null;

        #endregion Fields


        #region Properties ************************************************************

        /// <summary>
        /// Get/set the ID of the saved row in the database.
        /// </summary>
        [DataMember]
        public int SavedId
        {
            get
            {
                return savedId;
            }

            set
            {
                savedId = value;
            }
        }

        /// <summary>
        /// Get/set the database select result.
        /// </summary>
        [DataMember]
        public int Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }

        /// <summary>
        /// Get/set the database select error message.
        /// </summary>
        [DataMember]
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }

            set
            {
                errorMessage = value;
            }
        }

        #endregion Properties

    } //end of class SaveResult

} //end of namespace PnT.SongDB.Logic
