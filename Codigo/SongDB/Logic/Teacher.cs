using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for Teacher.
    /// </summary>
    [DataContract]
    public class Teacher
    {

        #region Fields *****************************************************************

        private int teacherId;
        private int userId;
        private string name;
        private DateTime birthdate;
        private string identity;
        private string identityAgency;
        private DateTime identityDate;
        private string taxId;
        private string pisId;
        private string address;
        private string district;
        private string city;
        private string state;
        private string zipCode;
        private string phone;
        private string mobile;
        private string email;
        private string academicBackground;
        private string workExperience;
        private byte[] photo;
        private int teacherStatus;
        private DateTime creationTime;
        private DateTime inactivationTime;
        private string inactivationReason;

        /// <summary>
        /// The login of the user.
        /// </summary>
        private string userLogin;

        /// <summary>
        /// The list of assigned pole names.
        /// </summary>
        private List<string> poleNames;

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
        public Teacher()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="teacherId">The id of the Teacher.</param>
        public Teacher(int teacherId)
        {
            this.teacherId = teacherId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.teacherId;}
            set{ this.teacherId = value;}
        }

        [DataMember]
        public int TeacherId
        {
            get{ return this.teacherId;}
            set{ this.teacherId = value;}
        }

        [DataMember]
        public int UserId
        {
            get{ return this.userId;}
            set{ this.userId = value;}
        }

        [DataMember]
        public string Name
        {
            get{ return this.name;}
            set{ this.name = value;}
        }

        [DataMember]
        public DateTime Birthdate
        {
            get{ return this.birthdate;}
            set{ this.birthdate = value;}
        }

        [DataMember]
        public string Identity
        {
            get{ return this.identity;}
            set{ this.identity = value;}
        }

        [DataMember]
        public string IdentityAgency
        {
            get{ return this.identityAgency;}
            set{ this.identityAgency = value;}
        }

        [DataMember]
        public DateTime IdentityDate
        {
            get{ return this.identityDate;}
            set{ this.identityDate = value;}
        }

        [DataMember]
        public string TaxId
        {
            get{ return this.taxId;}
            set{ this.taxId = value;}
        }

        [DataMember]
        public string PisId
        {
            get{ return this.pisId;}
            set{ this.pisId = value;}
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
            get{ return this.email;}
            set{ this.email = value;}
        }

        [DataMember]
        public string AcademicBackground
        {
            get{ return this.academicBackground;}
            set{ this.academicBackground = value;}
        }

        [DataMember]
        public string WorkExperience
        {
            get{ return this.workExperience;}
            set{ this.workExperience = value;}
        }

        public byte[] Photo
        {
            get{ return this.photo;}
            set{ this.photo = value;}
        }

        [DataMember]
        public int TeacherStatus
        {
            get{ return this.teacherStatus;}
            set{ this.teacherStatus = value;}
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
        /// Get/set the login of the user.
        /// </summary>
        [DataMember]
        public string UserLogin
        {
            get
            {
                return userLogin;
            }

            set
            {
                userLogin = value;
            }
        }

        /// <summary>
        /// Get/set the list of assigned pole names.
        /// </summary>
        [DataMember]
        public List<string> PoleNames
        {
            get
            {
                return poleNames;
            }

            set
            {
                poleNames = value;
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
        /// Save Teacher to database.
        /// </summary>
        /// <returns>The id of the saved Teacher.</returns>
        public int Save()
        {
            teacherId = Mapper.TeacherMapper.Save(null, this);
            return teacherId;
        }

        /// <summary>
        /// Save Teacher to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Teacher.</returns>
        public int Save(MySqlTransaction trans)
        {
            teacherId = Mapper.TeacherMapper.Save(trans, this);
            return teacherId;
        }

        /// <summary>
        /// Delete Teacher by id.
        /// </summary>
        /// <param name="id">The id of the selected Teacher.</param>
        /// <returns>
        /// True if selected Teacher was deleted.
        /// False if selected Teacher was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.TeacherMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Teacher by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Teacher.</param>
        /// <returns>
        /// True if selected Teacher was deleted.
        /// False if selected Teacher was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.TeacherMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Teacher by id.
        /// </summary>
        /// <param name="id">The id of the selected Teacher.</param>
        /// <param name="inactivationReason">
        /// The reason why the teacher is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Teacher was inactivated.
        /// False if selected Teacher was not found.
        /// </returns>
        public static bool Inactivate(int id, string inactivationReason)
        {
            return Mapper.TeacherMapper.Inactivate(null, id, inactivationReason);
        }

        /// <summary>
        /// Inactivate Teacher by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Teacher.</param>
        /// <param name="inactivationReason">
        /// The reason why the teacher is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Teacher was inactivated.
        /// False if selected Teacher was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id, string inactivationReason)
        {
            return Mapper.TeacherMapper.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all Teacher.
        /// </summary>
        /// <returns>
        /// List of Teacher objects.
        /// Null if no Teacher was found.
        /// </returns>
        public static List<Teacher> Find()
        {
            return Mapper.TeacherMapper.Find(null);
        }

        /// <summary>
        /// Find all Teacher with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Teacher objects.
        /// Null if no Teacher was found.
        /// </returns>
        public static List<Teacher> Find(MySqlTransaction trans)
        {
            return Mapper.TeacherMapper.Find(trans);
        }

        /// <summary>
        /// Count teachers by filter.
        /// </summary>
        /// <param name="filterTeacherStatus">
        /// The teacher status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <returns>
        /// The number of teachers.
        /// </returns>
        public static int CountByFilter(
            int filterTeacherStatus, int filterInstitution, int filterPole)
        {
            return Mapper.TeacherMapper.CountByFilter(
                null, filterTeacherStatus, filterInstitution, filterPole);
        }

        /// <summary>
        /// Find teachers by filter.
        /// </summary>
        /// <param name="filterTeacherStatus">
        /// The teacher status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <returns>
        /// List of Teacher objects.
        /// Null if no Teacher was found.
        /// </returns>
        public static List<Teacher> FindByFilter(
            int filterTeacherStatus, int filterInstitution, int filterPole)
        {
            return Mapper.TeacherMapper.FindByFilter(
                null, filterTeacherStatus, filterInstitution, filterPole);
        }

        /// <summary>
        /// Find teachers by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="filterTeacherStatus">
        /// The teacher status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <returns>
        /// List of Teacher objects.
        /// Null if no Teacher was found.
        /// </returns>
        public static List<Teacher> FindByFilter(
            MySqlTransaction trans, int filterTeacherStatus, int filterInstitution, int filterPole)
        {
            return Mapper.TeacherMapper.FindByFilter(
                trans, filterTeacherStatus, filterInstitution, filterPole);
        }

        /// <summary>
        /// Find Teacher by id.
        /// </summary>
        /// <param name="id">The id of the selected Teacher</param>
        /// <returns>
        /// The selected Teacher.
        /// Null if selected Teacher was not found.
        /// </returns>
        public static Teacher Find(int id)
        {
            return Mapper.TeacherMapper.Find(null, id);
        }

        /// <summary>
        /// Find Teacher by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Teacher</param>
        /// <returns>
        /// The selected Teacher.
        /// Null if selected Teacher was not found.
        /// </returns>
        public static Teacher Find(MySqlTransaction trans, int id)
        {
            return Mapper.TeacherMapper.Find(trans, id);
        }

        /// <summary>
        /// Find Teacher by user id.
        /// </summary>
        /// <param name="userId">The id of the selected user.</param>
        /// <returns>
        /// The selected user Teacher.
        /// Null if selected user has no Teacher.
        /// </returns>
        public static Teacher FindByUser(int userId)
        {
            return Mapper.TeacherMapper.FindByUser(null, userId);
        }

        /// <summary>
        /// Find Teacher by user id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="userId">The id of the selected user.</param>
        /// <returns>
        /// The selected user Teacher.
        /// Null if selected user has no Teacher.
        /// </returns>
        public static Teacher FindByUser(MySqlTransaction trans, int userId)
        {
            return Mapper.TeacherMapper.FindByUser(trans, userId);
        }

        /// <summary>
        /// Get description for this teacher.
        /// </summary>
        /// <returns>
        /// The created description.
        /// </returns>
        public IdDescriptionStatus GetDescription()
        {
            //create and return description.
            return new IdDescriptionStatus(
                this.teacherId, this.name, this.teacherStatus);
        }

        #endregion Methods

    } //end of class Teacher

} //end of namespace PnT.SongDB.Logic
