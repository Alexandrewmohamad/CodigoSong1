using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace PnT.SongDB.Logic
{
    /// <summary>
    /// Contains the the result of an average operation.
    /// </summary>
    [DataContract]
    public class AverageResult
    {

        #region Fields *****************************************************************

        /// <summary>
        /// The average result.
        /// </summary>
        private double average = -1;

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
        /// Get/set the average result.
        /// </summary>
        [DataMember]
        public double Average
        {
            get
            {
                return average;
            }

            set
            {
                average = value;
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

    } //end of class AverageResult

} //end of namespace PnT.SongDB.Logic
