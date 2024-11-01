using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Enumerates the roll call values.
    /// </summary>
    public enum RollCall
    {
        Empty = -1,
        Absent = 0,
        Present = 1,
        Sick = 2,
        Justified = 3,
        Evaded = 4,
        NoClass = 5,
        TeacherAbsent = 6,
        NotRegistered = 7
    }

    /// <summary>
    /// Summary description for Attendance.
    /// </summary>
    [DataContract]
    public class Attendance
    {

        #region Fields *****************************************************************

        private int attendanceId;
        private int studentId;
        private int teacherId;
        private int classId;
        private int classDay;
        private DateTime date;
        private int rollCall;

        /// <summary>
        /// The name of the student.
        /// </summary>
        private string studentName;

        /// <summary>
        /// The code of the class.
        /// </summary>
        private string classCode;

        /// <summary>
        /// Indicates if the attendance was updated.
        /// </summary>
        private bool updated = false;

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
        public Attendance()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="attendanceId">The id of the Attendance.</param>
        public Attendance(int attendanceId)
        {
            this.attendanceId = attendanceId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get { return this.attendanceId; }
            set { this.attendanceId = value; }
        }

        [DataMember]
        public int AttendanceId
        {
            get { return this.attendanceId; }
            set { this.attendanceId = value; }
        }

        [DataMember]
        public int StudentId
        {
            get { return this.studentId; }
            set { this.studentId = value; }
        }

        [DataMember]
        public int TeacherId
        {
            get { return this.teacherId; }
            set { this.teacherId = value; }
        }

        [DataMember]
        public int ClassId
        {
            get { return this.classId; }
            set { this.classId = value; }
        }

        [DataMember]
        public int ClassDay
        {
            get { return this.classDay; }
            set { this.classDay = value; }
        }

        [DataMember]
        public DateTime Date
        {
            get { return this.date; }
            set { this.date = value; }
        }

        [DataMember]
        public int RollCall
        {
            get { return this.rollCall; }
            set { this.rollCall = value; }
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
        /// Get/set the code of the class.
        /// </summary>
        [DataMember]
        public string ClassCode
        {
            get
            {
                return classCode;
            }

            set
            {
                classCode = value;
            }
        }

        /// <summary>
        /// Get/set if the attendance was updated.
        /// </summary>
        public bool Updated
        {
            get
            {
                return updated;
            }

            set
            {
                updated = value;
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
        /// Save Attendance to database.
        /// </summary>
        /// <returns>The id of the saved Attendance.</returns>
        public int Save()
        {
            attendanceId = Mapper.AttendanceMapper.Save(null, this);
            return attendanceId;
        }

        /// <summary>
        /// Save Attendance to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Attendance.</returns>
        public int Save(MySqlTransaction trans)
        {
            attendanceId = Mapper.AttendanceMapper.Save(trans, this);
            return attendanceId;
        }

        /// <summary>
        /// Delete Attendance by id.
        /// </summary>
        /// <param name="id">The id of the selected Attendance.</param>
        /// <returns>
        /// True if selected Attendance was deleted.
        /// False if selected Attendance was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.AttendanceMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Attendance by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Attendance.</param>
        /// <returns>
        /// True if selected Attendance was deleted.
        /// False if selected Attendance was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.AttendanceMapper.Delete(trans, id);
        }

        /// <summary>
        /// Delete all Attendance for selected class student.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="classId">The id of the selected class.</param>
        /// <param name="studentId">The id of the selected student.</param>
        /// <returns>
        /// The number of deleted Attendances.
        /// </returns>
        public static int DeleteForClassStudent(
            MySqlTransaction trans, int classId, int studentId)
        {
            return Mapper.AttendanceMapper.DeleteForClassStudent(
                trans, classId, studentId);
        }

        /// <summary>
        /// Inactivate Attendance by id.
        /// </summary>
        /// <param name="id">The id of the selected Attendance.</param>
        /// <returns>
        /// True if selected Attendance was inactivated.
        /// False if selected Attendance was not found.
        /// </returns>
        public static bool Inactivate(int id)
        {
            return Mapper.AttendanceMapper.Inactivate(null, id);
        }

        /// <summary>
        /// Inactivate Attendance by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Attendance.</param>
        /// <returns>
        /// True if selected Attendance was inactivated.
        /// False if selected Attendance was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Mapper.AttendanceMapper.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Attendance.
        /// </summary>
        /// <returns>
        /// List of Attendance objects.
        /// Null if no Attendance was found.
        /// </returns>
        public static List<Attendance> Find()
        {
            return Mapper.AttendanceMapper.Find(null);
        }

        /// <summary>
        /// Find all Attendance with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Attendance objects.
        /// Null if no Attendance was found.
        /// </returns>
        public static List<Attendance> Find(MySqlTransaction trans)
        {
            return Mapper.AttendanceMapper.Find(trans);
        }

        /// <summary>
        /// Count attendances by filter.
        /// </summary>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterRollCall">
        /// The roll call filter.
        /// -1 to selct all roll calls.
        /// </param>
        /// <param name="filterStartDate">
        /// The start date filter.
        /// DateTime.MinValue to select all dates.
        /// </param>
        /// <param name="filterEndDate">
        /// The end date filter.
        /// DateTime.MinValue to select all dates.
        /// </param>
        /// <returns>
        /// The number of attendances.
        /// </returns>
        public static int CountByFilter(
            int filterClass, int filterStudent, int filterTeacher,
            int filterRollCall, DateTime filterStartDate, DateTime filterEndDate)
        {
            return Mapper.AttendanceMapper.CountByFilter(
                null, filterClass, filterStudent, filterTeacher, 
                filterRollCall, filterStartDate, filterEndDate);
        }

        /// <summary>
        /// Find attendances by filter.
        /// </summary>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <returns>
        /// List of Attendance objects.
        /// Null if no Attendance was found.
        /// </returns>
        public static List<Attendance> FindByFilter(int filterClass, int filterStudent)
        {
            return Mapper.AttendanceMapper.FindByFilter(null, filterClass, filterStudent);
        }

        /// <summary>
        /// Find attendances by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <returns>
        /// List of Attendance objects.
        /// Null if no Attendance was found.
        /// </returns>
        public static List<Attendance> FindByFilter(
            MySqlTransaction trans, int filterClass, int filterStudent)
        {
            return Mapper.AttendanceMapper.FindByFilter(trans, filterClass, filterStudent);
        }

        /// <summary>
        /// Find attendances by class filter.
        /// </summary>
        /// <param name="filterSemester">
        /// The class semester filter.
        /// -1 to select all semesters.
        /// </param>
        /// <param name="filterInstitution">
        /// The class institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The class pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterTeacher">
        /// The class teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of Attendance objects.
        /// Null if no Attendance was found.
        /// </returns>
        public static List<Attendance> FindByClassFilter(
            int filterSemester, int filterInstitution, 
            int filterPole, int filterTeacher, int filterClass)
        {
            return Mapper.AttendanceMapper.FindByClassFilter(
                null, filterSemester, filterInstitution, 
                filterPole, filterTeacher, filterClass);
        }

        /// <summary>
        /// Find Attendance by id.
        /// </summary>
        /// <param name="id">The id of the selected Attendance</param>
        /// <returns>
        /// The selected Attendance.
        /// Null if selected Attendance was not found.
        /// </returns>
        public static Attendance Find(int id)
        {
            return Mapper.AttendanceMapper.Find(null, id);
        }

        /// <summary>
        /// Find Attendance by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Attendance</param>
        /// <returns>
        /// The selected Attendance.
        /// Null if selected Attendance was not found.
        /// </returns>
        public static Attendance Find(MySqlTransaction trans, int id)
        {
            return Mapper.AttendanceMapper.Find(trans, id);
        }

        #endregion Methods

    } //end of class Attendance

} //end of namespace PnT.SongDB.Logic
