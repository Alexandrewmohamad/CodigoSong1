using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace PnT.SongDB.Logic
{
    /// <summary>
    /// Contains the the result of an email send operation.
    /// </summary>
    [DataContract]
    public class SendResult
    {

        #region Fields *****************************************************************

        /// <summary>
        /// The send result.
        /// </summary>
        private int result;

        /// <summary>
        /// The send error message.
        /// </summary>
        private string errorMessage = null;

        #endregion Fields


        #region Properties ************************************************************

        /// <summary>
        /// Get/set the send result.
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
        /// Get/set the send error message.
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

    } //end of class SendResult

} //end of namespace PnT.SongDB.Logic
