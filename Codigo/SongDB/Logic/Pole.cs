using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for Pole.
    /// </summary>
    [DataContract]
    public class Pole
    {

        #region Fields *****************************************************************

        private int poleId;
        private int institutionId;
        private string name;
        private string address;
        private string district;
        private string city;
        private string state;
        private string zipCode;
        private string phone;
        private string mobile;
        private string email;
        private string description;
        private int poleStatus;
        private DateTime creationTime;
        private DateTime inactivationTime;
        private string inactivationReason;

        /// <summary>
        /// The name of the institution.
        /// </summary>
        private string institutionName;

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
        public Pole()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="poleId">The id of the Pole.</param>
        public Pole(int poleId)
        {
            this.poleId = poleId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.poleId;}
            set{ this.poleId = value;}
        }

        [DataMember]
        public int PoleId
        {
            get{ return this.poleId;}
            set{ this.poleId = value;}
        }

        [DataMember]
        public int InstitutionId
        {
            get{ return this.institutionId;}
            set{ this.institutionId = value;}
        }

        [DataMember]
        public string Name
        {
            get{ return this.name;}
            set{ this.name = value;}
        }

        [DataMember]
        public string Address
        {
            get{ return this.address;}
            set{ this.address = value;}
        }

        [DataMember]
        public string District
        {
            get{ return this.district;}
            set{ this.district = value;}
        }

        [DataMember]
        public string City
        {
            get{ return this.city;}
            set{ this.city = value;}
        }

        [DataMember]
        public string State
        {
            get{ return this.state;}
            set{ this.state = value;}
        }

        [DataMember]
        public string ZipCode
        {
            get{ return this.zipCode;}
            set{ this.zipCode = value;}
        }

        [DataMember]
        public string Phone
        {
            get{ return this.phone;}
            set{ this.phone = value;}
        }

        [DataMember]
        public string Mobile
        {
            get{ return this.mobile;}
            set{ this.mobile = value;}
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
            get{ return this.description;}
            set{ this.description = value;}
        }

        [DataMember]
        public int PoleStatus
        {
            get{ return this.poleStatus;}
            set{ this.poleStatus = value;}
        }

        [DataMember]
        public DateTime CreationTime
        {
            get{ return this.creationTime;}
            set{ this.creationTime = value;}
        }

        [DataMember]
        public DateTime InactivationTime
        {
            get{ return this.inactivationTime;}
            set{ this.inactivationTime = value;}
        }

        [DataMember]
        public string InactivationReason
        {
            get{ return this.inactivationReason;}
            set{ this.inactivationReason = value;}
        }

        /// <summary>
        /// Get/set the name of the institution.
        /// </summary>
        [DataMember]
        public string InstitutionName
        {
            get
            {
                return institutionName;
            }

            set
            {
                institutionName = value;
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
        /// Save Pole to database.
        /// </summary>
        /// <returns>The id of the saved Pole.</returns>
        public int Save()
        {
            poleId = Mapper.PoleMapper.Save(null, this);
            return poleId;
        }

        /// <summary>
        /// Save Pole to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Pole.</returns>
        public int Save(MySqlTransaction trans)
        {
            poleId = Mapper.PoleMapper.Save(trans, this);
            return poleId;
        }

        /// <summary>
        /// Delete Pole by id.
        /// </summary>
        /// <param name="id">The id of the selected Pole.</param>
        /// <returns>
        /// True if selected Pole was deleted.
        /// False if selected Pole was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.PoleMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Pole by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Pole.</param>
        /// <returns>
        /// True if selected Pole was deleted.
        /// False if selected Pole was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.PoleMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Pole by id.
        /// </summary>
        /// <param name="id">The id of the selected Pole.</param>
        /// <param name="inactivationReason">
        /// The reason why the pole is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Pole was inactivated.
        /// False if selected Pole was not found.
        /// </returns>
        public static bool Inactivate(int id, string inactivationReason)
        {
            return Mapper.PoleMapper.Inactivate(null, id, inactivationReason);
        }

        /// <summary>
        /// Inactivate Pole by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Pole.</param>
        /// <param name="inactivationReason">
        /// The reason why the pole is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Pole was inactivated.
        /// False if selected Pole was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id, string inactivationReason)
        {
            return Mapper.PoleMapper.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all Pole.
        /// </summary>
        /// <returns>
        /// List of Pole objects.
        /// Null if no Pole was found.
        /// </returns>
        public static List<Pole> Find()
        {
            return Mapper.PoleMapper.Find(null);
        }

        /// <summary>
        /// Find all Pole with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Pole objects.
        /// Null if no Pole was found.
        /// </returns>
        public static List<Pole> Find(MySqlTransaction trans)
        {
            return Mapper.PoleMapper.Find(trans);
        }

        /// <summary>
        /// Find all Pole by teacher.
        /// </summary>
        /// <param name="teacherId">
        /// The ID of the selected teacher.
        /// </param>
        /// <returns>
        /// List of Pole objects.
        /// Null if no Pole was found.
        /// </returns>
        public static List<Pole> FindByTeacher(int teacherId)
        {
            return Mapper.PoleMapper.FindByTeacher(null, teacherId);
        }

        /// <summary>
        /// Find all Pole by teacher with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="teacherId">
        /// The ID of the selected teacher.
        /// </param>
        /// <returns>
        /// List of Pole objects.
        /// Null if no Pole was found.
        /// </returns>
        public static List<Pole> FindByTeacher(MySqlTransaction trans, int teacherId)
        {
            return Mapper.PoleMapper.FindByTeacher(trans, teacherId);
        }

        /// <summary>
        /// Count poles by filter.
        /// </summary>
        /// <param name="filterPoleStatus">
        /// The pole status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <returns>
        /// The number of poles.
        /// </returns>
        public static int CountByFilter(int filterPoleStatus, int filterInstitution)
        {
            return Mapper.PoleMapper.CountByFilter(null, filterPoleStatus, filterInstitution);
        }

        /// <summary>
        /// Find poles by filter.
        /// </summary>
        /// <param name="filterPoleStatus">
        /// The pole status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <returns>
        /// List of Pole objects.
        /// Null if no Pole was found.
        /// </returns>
        public static List<Pole> FindByFilter(int filterPoleStatus, int filterInstitution)
        {
            return Mapper.PoleMapper.FindByFilter(null, filterPoleStatus, filterInstitution);
        }

        /// <summary>
        /// Find poles by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="filterPoleStatus">
        /// The pole status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <returns>
        /// List of Pole objects.
        /// Null if no Pole was found.
        /// </returns>
        public static List<Pole> FindByFilter(
            MySqlTransaction trans, int filterPoleStatus, int filterInstitution)
        {
            return Mapper.PoleMapper.FindByFilter(trans, filterPoleStatus, filterInstitution);
        }

        /// <summary>
        /// Find Pole by id.
        /// </summary>
        /// <param name="id">The id of the selected Pole</param>
        /// <returns>
        /// The selected Pole.
        /// Null if selected Pole was not found.
        /// </returns>
        public static Pole Find(int id)
        {
            return Mapper.PoleMapper.Find(null, id);
        }

        /// <summary>
        /// Find Pole by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Pole</param>
        /// <returns>
        /// The selected Pole.
        /// Null if selected Pole was not found.
        /// </returns>
        public static Pole Find(MySqlTransaction trans, int id)
        {
            return Mapper.PoleMapper.Find(trans, id);
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
                this.poleId, this.name, this.poleStatus);
        }

        #endregion Methods

    } //end of class Pole

} //end of namespace PnT.SongDB.Logic
