using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for Institution.
    /// </summary>
    [DataContract]
    public class Institution
    {

        #region Fields *****************************************************************

        private int institutionId;
        private string entityName;
        private string projectName;
        private string localInitiative;
        private bool institutionalized;
        private string taxId;
        private int coordinatorId;
        private string address;
        private string district;
        private string city;
        private string state;
        private string zipCode;
        private string phone;
        private string mobile;
        private string site;
        private string email;
        private string description;
        private int institutionStatus;
        private DateTime creationTime;
        private DateTime inactivationTime;
        private string inactivationReason;

        /// <summary>
        /// The name of the coordinator.
        /// </summary>
        private string coordinatorName;

        /// <summary>
        /// The database select result.
        /// </summary>
        private int result;

        /// <summary>
        /// The database select error message.
        /// </summary>
        private string errorMessage = null;

        #endregion Fields


        #region Constructors ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Institution()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="institutionId">The id of the Institution.</param>
        public Institution(int institutionId)
        {
            this.institutionId = institutionId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get { return this.institutionId; }
            set { this.institutionId = value; }
        }

        [DataMember]
        public int InstitutionId
        {
            get { return this.institutionId; }
            set { this.institutionId = value; }
        }

        [DataMember]
        public string EntityName
        {
            get { return this.entityName; }
            set { this.entityName = value; }
        }

        [DataMember]
        public string ProjectName
        {
            get { return this.projectName; }
            set { this.projectName = value; }
        }

        [DataMember]
        public string LocalInitiative
        {
            get { return this.localInitiative; }
            set { this.localInitiative = value; }
        }

        [DataMember]
        public bool Institutionalized
        {
            get { return this.institutionalized; }
            set { this.institutionalized = value; }
        }

        [DataMember]
        public string TaxId
        {
            get { return this.taxId; }
            set { this.taxId = value; }
        }

        [DataMember]
        public int CoordinatorId
        {
            get { return this.coordinatorId; }
            set { this.coordinatorId = value; }
        }

        [DataMember]
        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }

        [DataMember]
        public string District
        {
            get { return this.district; }
            set { this.district = value; }
        }

        [DataMember]
        public string City
        {
            get { return this.city; }
            set { this.city = value; }
        }

        [DataMember]
        public string State
        {
            get { return this.state; }
            set { this.state = value; }
        }

        [DataMember]
        public string ZipCode
        {
            get { return this.zipCode; }
            set { this.zipCode = value; }
        }

        [DataMember]
        public string Phone
        {
            get { return this.phone; }
            set { this.phone = value; }
        }

        [DataMember]
        public string Mobile
        {
            get { return this.mobile; }
            set { this.mobile = value; }
        }

        [DataMember]
        public string Site
        {
            get { return this.site; }
            set { this.site = value; }
        }

        [DataMember]
        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }

        [DataMember]
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        [DataMember]
        public int InstitutionStatus
        {
            get { return this.institutionStatus; }
            set { this.institutionStatus = value; }
        }

        [DataMember]
        public DateTime CreationTime
        {
            get { return this.creationTime; }
            set { this.creationTime = value; }
        }

        [DataMember]
        public DateTime InactivationTime
        {
            get { return this.inactivationTime; }
            set { this.inactivationTime = value; }
        }

        [DataMember]
        public string InactivationReason
        {
            get { return this.inactivationReason; }
            set { this.inactivationReason = value; }
        }

        /// <summary>
        /// Get/set the name of the coordinator.
        /// </summary>
        [DataMember]
        public string CoordinatorName
        {
            get
            {
                return coordinatorName;
            }

            set
            {
                coordinatorName = value;
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


        #region Methods ****************************************************************

        /// <summary>
        /// Save Institution to database.
        /// </summary>
        /// <returns>The id of the saved Institution.</returns>
        public int Save()
        {
            institutionId = Mapper.InstitutionMapper.Save(null, this);
            return institutionId;
        }

        /// <summary>
        /// Save Institution to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Institution.</returns>
        public int Save(MySqlTransaction trans)
        {
            institutionId = Mapper.InstitutionMapper.Save(trans, this);
            return institutionId;
        }

        /// <summary>
        /// Delete Institution by id.
        /// </summary>
        /// <param name="id">The id of the selected Institution.</param>
        /// <returns>
        /// True if selected Institution was deleted.
        /// False if selected Institution was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.InstitutionMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Institution by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Institution.</param>
        /// <returns>
        /// True if selected Institution was deleted.
        /// False if selected Institution was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.InstitutionMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Institution by id.
        /// </summary>
        /// <param name="id">The id of the selected Institution.</param>
        /// <param name="inactivationReason">
        /// The reason why the institution is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Institution was inactivated.
        /// False if selected Institution was not found.
        /// </returns>
        public static bool Inactivate(int id, string inactivationReason)
        {
            return Mapper.InstitutionMapper.Inactivate(null, id, inactivationReason);
        }

        /// <summary>
        /// Inactivate Institution by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Institution.</param>
        /// <param name="inactivationReason">
        /// The reason why the institution is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Institution was inactivated.
        /// False if selected Institution was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id, string inactivationReason)
        {
            return Mapper.InstitutionMapper.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all Institution.
        /// </summary>
        /// <returns>
        /// List of Institution objects.
        /// Null if no Institution was found.
        /// </returns>
        public static List<Institution> Find()
        {
            return Mapper.InstitutionMapper.Find(null);
        }

        /// <summary>
        /// Find all Institution with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Institution objects.
        /// Null if no Institution was found.
        /// </returns>
        public static List<Institution> Find(MySqlTransaction trans)
        {
            return Mapper.InstitutionMapper.Find(trans);
        }

        /// <summary>
        /// Count institutions by filter.
        /// </summary>
        /// <param name="filterInstitutionStatus">
        /// The institution status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <returns>
        /// The number of institutions.
        /// </returns>
        public static int CountByFilter(int filterInstitutionStatus)
        {
            return Mapper.InstitutionMapper.CountByFilter(
                null, filterInstitutionStatus);
        }

        /// <summary>
        /// Find institutions by filter.
        /// </summary>
        /// <param name="filterInstitutionStatus">
        /// The institution status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <returns>
        /// List of Institution objects.
        /// Null if no Institution was found.
        /// </returns>
        public static List<Institution> FindByFilter(int filterInstitutionStatus)
        {
            return Mapper.InstitutionMapper.FindByFilter(
                null, filterInstitutionStatus);
        }

        /// <summary>
        /// Find institutions by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="filterInstitutionStatus">
        /// The institution status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <returns>
        /// List of Institution objects.
        /// Null if no Institution was found.
        /// </returns>
        public static List<Institution> FindByFilter(
            MySqlTransaction trans, int filterInstitutionStatus)
        {
            return Mapper.InstitutionMapper.FindByFilter(
                trans, filterInstitutionStatus);
        }

        /// <summary>
        /// Find Institution by id.
        /// </summary>
        /// <param name="id">The id of the selected Institution</param>
        /// <returns>
        /// The selected Institution.
        /// Null if selected Institution was not found.
        /// </returns>
        public static Institution Find(int id)
        {
            return Mapper.InstitutionMapper.Find(null, id);
        }

        /// <summary>
        /// Find Institution by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Institution</param>
        /// <returns>
        /// The selected Institution.
        /// Null if selected Institution was not found.
        /// </returns>
        public static Institution Find(MySqlTransaction trans, int id)
        {
            return Mapper.InstitutionMapper.Find(trans, id);
        }

        /// <summary>
        /// Get description for this institution.
        /// </summary>
        /// <returns>
        /// The created description.
        /// </returns>
        public IdDescriptionStatus GetDescription()
        {
            //create and return description.
            return new IdDescriptionStatus(
                this.institutionId, this.projectName, this.institutionStatus);
        }

        #endregion Methods

    } //end of class Institution

} //end of namespace PnT.SongDB.Logic
