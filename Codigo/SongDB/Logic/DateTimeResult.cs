using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace PnT.SongDB.Logic
{
    /// <summary>
    /// Contains the the result of a date time operation.
    /// </summary>
    [DataContract]
    public class DateTimeResult
    {

        #region Fields *****************************************************************

        /// <summary>
        /// The date time result.
        /// </summary>
        private DateTime time = DateTime.MinValue;

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
        /// Get/set the date time result.
        /// </summary>
        [DataMember]
        public DateTime Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
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

    } //end of class DateResult

} //end of namespace PnT.SongDB.Logic
