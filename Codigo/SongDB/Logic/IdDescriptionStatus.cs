using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Enumerates the possible item statuses.
    /// Active - The item is active in the system.
    /// Inactive - The item is inactive in the system.
    /// Blocked - The item is blocked in the system.
    /// Maintenance - The item is under maintenance and is not available in the system.
    /// Evaded - The item is evaded.
    /// Lost - The item was lost.
    /// </summary>
    public enum ItemStatus
    {
        Active = 0,
        Inactive = 1,
        Blocked = 2,
        Maintenance = 3,
        Evaded = 4,
        Lost = 5
    }

    /// <summary>
    /// Enumerates the possible select results
    /// </summary>
    public enum SelectResult
    {
        Success,
        Empty,
        FatalError
    }

    /// <summary>
    /// Contains the ID, description and status of an object in the database.
    /// </summary>
    [DataContract]
    public class IdDescriptionStatus
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The ID of the object in the database.
        /// </summary>
        private int id;

        /// <summary>
        /// The description of the object.
        /// </summary>
        private string description;

        /// <summary>
        /// The status of the object.
        /// </summary>
        private int status;

        /// <summary>
        /// The database select result.
        /// </summary>
        private int result;

        /// <summary>
        /// The database select error message.
        /// </summary>
        private string errorMessage = null;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IdDescriptionStatus()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="id">The ID of the object in the database.</param>
        /// <param name="description">The description of the object.</param>
        /// <param name="status">The status of the object.</param>
        public IdDescriptionStatus(int id, string description, int status)
        {
            //set fields
            this.id = id;
            this.description = description;
            this.status = status;
            this.result = (int)SelectResult.Success;
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get/set the ID of the object in the database.
        /// </summary>
        [DataMember]
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Get/set the description of the object.
        /// </summary>
        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Get/set the status of the object.
        /// </summary>
        [DataMember]
        public int Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
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


        #region Public Methods ********************************************************

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            //check description
            if (description != null && description.Length > 0)
            {
                //use description
                return description;
            }

            //use default value
            return base.ToString();
        }

        /// <summary>
        /// Copy this description object.
        /// </summary>
        /// <returns>The returned copy.</returns>
        public IdDescriptionStatus Copy()
        {
            return (IdDescriptionStatus)this.MemberwiseClone();
        }

        #endregion Public Methods

    } //end of class IdDescriptionStatus

} //end of namespace PnT.SongDB.Logic