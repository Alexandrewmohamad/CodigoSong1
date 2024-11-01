using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for Registration.
    /// </summary>
    [DataContract]
    public class Registration
    {

        #region Fields *****************************************************************

        private int registrationId;
        private int studentId;
        private int classId;
        private int position;
        private bool autoRenewal;
        private int registrationStatus;
        private DateTime creationTime;
        private DateTime inactivationTime;
        private string inactivationReason;

        /// <summary>
        /// The attended class
        /// </summary>
        private Class objClass;

        /// <summary>
        /// The name of the student.
        /// </summary>
        private string studentName;

        /// <summary>
        /// The name of the teacher.
        /// </summary>
        private string teacherName;

        /// <summary>
        /// The ID of the class pole.
        /// </summary>
        private int poleId;

        /// <summary>
        /// The name of the class pole.
        /// </summary>
        private string poleName;

        /// <summary>
        /// The semester of the class.
        /// </summary>
        private Semester semester;

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
        public Registration()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="registrationId">The id of the Registration.</param>
        public Registration(int registrationId)
        {
            this.registrationId = registrationId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.registrationId;}
            set{ this.registrationId = value;}
        }

        [DataMember]
        public int RegistrationId
        {
            get{ return this.registrationId;}
            set{ this.registrationId = value;}
        }

        [DataMember]
        public int StudentId
        {
            get{ return this.studentId;}
            set{ this.studentId = value;}
        }

        [DataMember]
        public int ClassId
        {
            get{ return this.classId;}
            set{ this.classId = value;}
        }

        [DataMember]
        public int Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        [DataMember]
        public bool AutoRenewal
        {
            get { return this.autoRenewal; }
            set { this.autoRenewal = value; }
        }

        [DataMember]
        public int RegistrationStatus
        {
            get{ return this.registrationStatus;}
            set{ this.registrationStatus = value;}
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
        /// Get/set the attended class.
        /// </summary>
        [DataMember]
        public Class Class
        {
            get
            {
                return objClass;
            }

            set
            {
                objClass = value;
            }
        }
        
        /// <summary>
        /// Get/set the name of the student.
        /// </summary>
        [DataMember]
        public string StudentName
        {
            get
            {
                return studentName;
            }

            set
            {
                studentName = value;
            }
        }

        /// <summary>
        /// Get/set the name of the teacher.
        /// </summary>
        [DataMember]
        public string TeacherName
        {
            get
            {
                return teacherName;
            }

            set
            {
                teacherName = value;
            }
        }

        /// <summary>
        /// Get/set the ID of the class pole.
        /// </summary>
        [DataMember]
        public int PoleId
        {
            get
            {
                return poleId;
            }

            set
            {
                poleId = value;
            }
        }

        /// <summary>
        /// Get/set the name of the class pole.
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
        /// Get/set the semester of the class.
        /// </summary>
        [DataMember]
        public Semester Semester
        {
            get
            {
                return semester;
            }

            set
            {
                semester = value;
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
        /// Save Registration to database.
        /// </summary>
        /// <returns>The id of the saved Registration.</returns>
        public int Save()
        {
            registrationId = Mapper.RegistrationMapper.Save(null, this);
            return registrationId;
        }

        /// <summary>
        /// Save Registration to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Registration.</returns>
        public int Save(MySqlTransaction trans)
        {
            registrationId = Mapper.RegistrationMapper.Save(trans, this);
            return registrationId;
        }

        /// <summary>
        /// Delete Registration by id.
        /// </summary>
        /// <param name="id">The id of the selected Registration.</param>
        /// <returns>
        /// True if selected Registration was deleted.
        /// False if selected Registration was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.RegistrationMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Registration by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Registration.</param>
        /// <returns>
        /// True if selected Registration was deleted.
        /// False if selected Registration was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.RegistrationMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Registration by id.
        /// </summary>
        /// <param name="id">The id of the selected Registration.</param>
        /// <returns>
        /// True if selected Registration was inactivated.
        /// False if selected Registration was not found.
        /// </returns>
        public static bool Inactivate(int id)
        {
            return Mapper.RegistrationMapper.Inactivate(null, id);
        }

        /// <summary>
        /// Inactivate Registration by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Registration.</param>
        /// <returns>
        /// True if selected Registration was inactivated.
        /// False if selected Registration was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Mapper.RegistrationMapper.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Registration.
        /// </summary>
        /// <returns>
        /// List of Registration objects.
        /// Null if no Registration was found.
        /// </returns>
        public static List<Registration> Find()
        {
            return Mapper.RegistrationMapper.Find(null);
        }

        /// <summary>
        /// Find all Registration with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Registration objects.
        /// Null if no Registration was found.
        /// </returns>
        public static List<Registration> Find(MySqlTransaction trans)
        {
            return Mapper.RegistrationMapper.Find(trans);
        }

        /// <summary>
        /// Count registrations for selected class.
        /// </summary>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <param name="status">
        /// The status of the selected registrations.
        /// -1 to count all registrations.
        /// </param>
        /// <param name="maxPosition">
        /// The maximum registration position to consider.
        /// -1 to count all registrations.
        /// </param>
        /// <returns>
        /// The number of registrations.
        /// </returns>
        public static int CountByClass(int classId, int status, int maxPosition)
        {
            return Mapper.RegistrationMapper.CountByClass(null, classId, status, maxPosition);
        }

        /// <summary>
        /// Find all Registration for selected Class.
        /// </summary>
        /// <param name="classId">The id of the selected class.</param>
        /// <param name="status">
        /// The status of the returned registrations.
        /// -1 to return all registrations.
        /// </param>
        /// <returns>
        /// List of Registration objects.
        /// Null if no Registration was found.
        /// </returns>
        public static List<Registration> FindByClass(int classId, int status)
        {
            return Mapper.RegistrationMapper.FindByClass(null, classId, status);
        }

        /// <summary>
        /// Find all Registration for selected Class with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="classId">The id of the selected class.</param>
        /// <param name="status">
        /// The status of the returned registrations.
        /// -1 to return all registrations.
        /// </param>
        /// <returns>
        /// List of Registration objects.
        /// Null if no Registration was found.
        /// </returns>
        public static List<Registration> FindByClass(MySqlTransaction trans, int classId, int status)
        {
            return Mapper.RegistrationMapper.FindByClass(trans, classId, status);
        }


        /// <summary>
        /// Count registration evations by filter.
        /// </summary>
        /// <param name="filterSemester">
        /// The semester filter.
        /// -1 to select all semesters.
        /// </param>
        /// <param name="filterReferenceDate">
        /// The reference date filter.
        /// DateTime.MinValue to selct all dates.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <returns>
        /// The number of evations.
        /// </returns>
        public static int CountEvationsByFilter(
            int filterSemester, DateTime filterReferenceDate,
            int filterInstitution, int filterTeacher)
        {
            return Mapper.RegistrationMapper.CountEvationsByFilter(
                null, filterSemester, filterReferenceDate, filterInstitution, filterTeacher);
        }

        /// <summary>
        /// Count registrations by filter.
        /// </summary>
        /// <param name="filterRegistrationStatus">
        /// The registration status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterSemester">
        /// The semester filter.
        /// -1 to select all semesters.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// The number of registrations.
        /// </returns>
        public static int CountByFilter(
            int filterRegistrationStatus, int filterSemester,
            int filterInstitution, int filterPole, int filterTeacher, int filterClass)
        {
            return Mapper.RegistrationMapper.CountByFilter(
                null, filterRegistrationStatus, filterSemester,
                filterInstitution, filterPole, filterTeacher, filterClass);
        }

        /// <summary>
        /// Find registrations by filter.
        /// </summary>
        /// <param name="filterRegistrationStatus">
        /// The registration status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterSemester">
        /// The semester filter.
        /// -1 to select all semesters.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of Registration objects.
        /// Null if no Registration was found.
        /// </returns>
        public static List<Registration> FindByFilter(
            int filterRegistrationStatus, int filterSemester, int filterInstitution, 
            int filterPole, int filterTeacher, int filterClass)
        {
            return Mapper.RegistrationMapper.FindByFilter(
                null, filterRegistrationStatus, filterSemester, 
                filterInstitution, filterPole, filterTeacher, filterClass);
        }

        /// <summary>
        /// Find registrations by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="filterRegistrationStatus">
        /// The registration status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterSemester">
        /// The semester filter.
        /// -1 to select all semesters.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of Registration objects.
        /// Null if no Registration was found.
        /// </returns>
        public static List<Registration> FindByFilter(
            MySqlTransaction trans, int filterRegistrationStatus, int filterSemester, 
            int filterInstitution, int filterPole, int filterTeacher, int filterClass)
        {
            return Mapper.RegistrationMapper.FindByFilter(
                trans, filterRegistrationStatus, filterSemester, 
                filterInstitution, filterPole, filterTeacher, filterClass);
        }

        /// <summary>
        /// Find all Registration for selected Student.
        /// </summary>
        /// <param name="studentId">The id of the selected student.</param>
        /// <param name="status">
        /// The status of the returned registrations.
        /// -1 to return all registrations.
        /// </param>
        /// <returns>
        /// List of Registration objects.
        /// Null if no Registration was found.
        /// </returns>
        public static List<Registration> FindByStudent(int studentId, int status)
        {
            return Mapper.RegistrationMapper.FindByStudent(null, studentId, status);
        }

        /// <summary>
        /// Find all Registration for selected Student with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="studentId">The id of the selected student.</param>
        /// <param name="status">
        /// The status of the returned registrations.
        /// -1 to return all registrations.
        /// </param>
        /// <returns>
        /// List of Registration objects.
        /// Null if no Registration was found.
        /// </returns>
        public static List<Registration> FindByStudent(MySqlTransaction trans, int studentId, int status)
        {
            return Mapper.RegistrationMapper.FindByStudent(trans, studentId, status);
        }

        /// <summary>
        /// Find Registration by id.
        /// </summary>
        /// <param name="id">The id of the selected Registration</param>
        /// <returns>
        /// The selected Registration.
        /// Null if selected Registration was not found.
        /// </returns>
        public static Registration Find(int id)
        {
            return Mapper.RegistrationMapper.Find(null, id);
        }

        /// <summary>
        /// Find Registration by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Registration</param>
        /// <returns>
        /// The selected Registration.
        /// Null if selected Registration was not found.
        /// </returns>
        public static Registration Find(MySqlTransaction trans, int id)
        {
            return Mapper.RegistrationMapper.Find(trans, id);
        }

        /// <summary>
        /// Get description for this registration.
        /// </summary>
        /// <returns>
        /// The created description.
        /// </returns>
        public IdDescriptionStatus GetDescription()
        {
            //create and return description.
            return new IdDescriptionStatus(
                this.registrationId, this.registrationId.ToString(), this.registrationStatus);
        }

        /// <summary>
        /// Copy this registration object.
        /// </summary>
        /// <returns>The returned copy.</returns>
        public Registration Copy()
        {
            return (Registration)this.MemberwiseClone();
        }

        #endregion Methods

    } //end of class Registration

} //end of namespace PnT.SongDB.Logic
