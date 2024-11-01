using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for Student.
    /// </summary>
    [DataContract]
    public class Student
    {

        #region Fields *****************************************************************

        private int studentId;
        private int userId;
        private int poleId;
        private string name;
        private DateTime birthdate;
        private string guardianName;
        private string guardianIdentity;
        private string guardianIdentityAgency;
        private DateTime guardianIdentityDate;
        private string guardianTaxId;
        private string address;
        private string district;
        private string city;
        private string state;
        private string zipCode;
        private string phone;
        private string mobile;
        private string email;
        private byte[] photo;
        private string assignmentFile;
        private string photoFile;
        private bool hasDisability;
        private string specialCare;
        private int studentStatus;
        private DateTime creationTime;
        private DateTime inactivationTime;
        private string inactivationReason;

        /// <summary>
        /// The name of the pole.
        /// </summary>
        private string poleName;

        /// <summary>
        /// The login of the user.
        /// </summary>
        private string userLogin;

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
        public Student()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="studentId">The id of the Student.</param>
        public Student(int studentId)
        {
            this.studentId = studentId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.studentId;}
            set{ this.studentId = value;}
        }

        [DataMember]
        public int StudentId
        {
            get{ return this.studentId;}
            set{ this.studentId = value;}
        }

        [DataMember]
        public int UserId
        {
            get{ return this.userId;}
            set{ this.userId = value;}
        }

        [DataMember]
        public int PoleId
        {
            get{ return this.poleId;}
            set{ this.poleId = value;}
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
        public string GuardianName
        {
            get{ return this.guardianName;}
            set{ this.guardianName = value;}
        }

        [DataMember]
        public string GuardianIdentity
        {
            get{ return this.guardianIdentity;}
            set{ this.guardianIdentity = value;}
        }

        [DataMember]
        public string GuardianIdentityAgency
        {
            get{ return this.guardianIdentityAgency;}
            set{ this.guardianIdentityAgency = value;}
        }

        [DataMember]
        public DateTime GuardianIdentityDate
        {
            get{ return this.guardianIdentityDate;}
            set{ this.guardianIdentityDate = value;}
        }

        [DataMember]
        public string GuardianTaxId
        {
            get{ return this.guardianTaxId;}
            set{ this.guardianTaxId = value;}
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

        public byte[] Photo
        {
            get{ return this.photo;}
            set{ this.photo = value;}
        }

        [DataMember]
        public string AssignmentFile
        {
            get { return this.assignmentFile; }
            set { this.assignmentFile = value; }
        }

        [DataMember]
        public string PhotoFile
        {
            get { return this.photoFile; }
            set { this.photoFile = value; }
        }

        [DataMember]
        public bool HasDisability
        {
            get{ return this.hasDisability;}
            set{ this.hasDisability = value;}
        }

        [DataMember]
        public string SpecialCare
        {
            get{ return this.specialCare;}
            set{ this.specialCare = value;}
        }

        [DataMember]
        public int StudentStatus
        {
            get{ return this.studentStatus;}
            set{ this.studentStatus = value;}
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
        /// Get/set the name of the pole.
        /// </summary>
        [DataMember]
        public string PoleName
        {
            get
            {
                return poleName;
            }

            set
            {
                poleName = value;
            }
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
        /// Save Student to database.
        /// </summary>
        /// <returns>The id of the saved Student.</returns>
        public int Save()
        {
            studentId = Mapper.StudentMapper.Save(null, this);
            return studentId;
        }

        /// <summary>
        /// Save Student to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Student.</returns>
        public int Save(MySqlTransaction trans)
        {
            studentId = Mapper.StudentMapper.Save(trans, this);
            return studentId;
        }

        /// <summary>
        /// Delete Student by id.
        /// </summary>
        /// <param name="id">The id of the selected Student.</param>
        /// <returns>
        /// True if selected Student was deleted.
        /// False if selected Student was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.StudentMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Student by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Student.</param>
        /// <returns>
        /// True if selected Student was deleted.
        /// False if selected Student was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.StudentMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Student by id.
        /// </summary>
        /// <param name="id">The id of the selected Student.</param>
        /// <param name="inactivationReason">
        /// The reason why the student is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Student was inactivated.
        /// False if selected Student was not found.
        /// </returns>
        public static bool Inactivate(int id, string inactivationReason)
        {
            return Mapper.StudentMapper.Inactivate(null, id, inactivationReason);
        }

        /// <summary>
        /// Inactivate Student by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Student.</param>
        /// <param name="inactivationReason">
        /// The reason why the student is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Student was inactivated.
        /// False if selected Student was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id, string inactivationReason)
        {
            return Mapper.StudentMapper.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all Student.
        /// </summary>
        /// <returns>
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> Find()
        {
            return Mapper.StudentMapper.Find(null);
        }

        /// <summary>
        /// Find all Student with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> Find(MySqlTransaction trans)
        {
            return Mapper.StudentMapper.Find(trans);
        }

        /// <summary>
        /// Count students by filter.
        /// </summary>
        /// <param name="filterStudentStatus">
        /// The student status filter.
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
        /// The number of students.
        /// </returns>
        public static int CountByFilter(
            int filterStudentStatus, int filterInstitution, int filterPole)
        {
            return Mapper.StudentMapper.CountByFilter(
                null, filterStudentStatus, filterInstitution, filterPole);
        }

        /// <summary>
        /// Find students by filter.
        /// </summary>
        /// <param name="filterStudentStatus">
        /// The student status filter.
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
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> FindByFilter(
            int filterStudentStatus, int filterInstitution, int filterPole)
        {
            return Mapper.StudentMapper.FindByFilter(
                null, filterStudentStatus, filterInstitution, filterPole);
        }

        /// <summary>
        /// Find students by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="filterStudentStatus">
        /// The student status filter.
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
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> FindByFilter(
            MySqlTransaction trans, int filterStudentStatus, int filterInstitution, int filterPole)
        {
            return Mapper.StudentMapper.FindByFilter(
                trans, filterStudentStatus, filterInstitution, filterPole);
        }

        /// <summary>
        /// Find all students that loaned selected class.
        /// </summary>
        /// <param name="classId">The ID of the selected class.</param>
        /// <returns>
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> FindByClass(int classId)
        {
            return Mapper.StudentMapper.FindByClass(null, classId);
        }

        /// <summary>
        /// Find all students that loaned selected class with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="classId">The ID of the selected class.</param>
        /// <returns>
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> FindByClass(
            MySqlTransaction trans, int classId)
        {
            return Mapper.StudentMapper.FindByClass(trans, classId);
        }

        /// <summary>
        /// Find all students that loaned selected instrument.
        /// </summary>
        /// <param name="instrumentId">The ID of the selected instrument.</param>
        /// <returns>
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> FindByInstrument(int instrumentId)
        {
            return Mapper.StudentMapper.FindByInstrument(null, instrumentId);
        }

        /// <summary>
        /// Find all students that loaned selected instrument with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="instrumentId">The ID of the selected instrument.</param>
        /// <returns>
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> FindByInstrument(
            MySqlTransaction trans, int instrumentId)
        {
            return Mapper.StudentMapper.FindByInstrument(trans, instrumentId);
        }

        /// <summary>
        /// Find all students that currently have an active loan.
        /// </summary>
        /// <returns>
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> FindWithLoan()
        {
            return Mapper.StudentMapper.FindWithLoan(null);
        }

        /// <summary>
        /// Find all students that currently have an active loan with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> FindWithLoan(
            MySqlTransaction trans, int instrumentId)
        {
            return Mapper.StudentMapper.FindWithLoan(trans);
        }

        /// <summary>
        /// Find Student by id.
        /// </summary>
        /// <param name="id">The id of the selected Student</param>
        /// <returns>
        /// The selected Student.
        /// Null if selected Student was not found.
        /// </returns>
        public static Student Find(int id)
        {
            return Mapper.StudentMapper.Find(null, id);
        }

        /// <summary>
        /// Find Student by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Student</param>
        /// <returns>
        /// The selected Student.
        /// Null if selected Student was not found.
        /// </returns>
        public static Student Find(MySqlTransaction trans, int id)
        {
            return Mapper.StudentMapper.Find(trans, id);
        }

        /// <summary>
        /// Get description for this student.
        /// </summary>
        /// <returns>
        /// The created description.
        /// </returns>
        public IdDescriptionStatus GetDescription()
        {
            //create and return description.
            return new IdDescriptionStatus(
                this.studentId, this.name, this.studentStatus);
        }

        #endregion Methods

    } //end of class Student

} //end of namespace PnT.SongDB.Logic
