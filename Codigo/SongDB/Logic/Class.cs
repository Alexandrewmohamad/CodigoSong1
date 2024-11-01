using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Enumerates the possible class levels.
    /// </summary>
    public enum ClassLevel
    {
        Level1 = 1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Level7,
        Level8,
        Level9,
        Level10
    }

    /// <summary>
    /// Enumerates the possible class types.
    /// </summary>
    public enum ClassType
    {
        Instrument = 0,
        MusicTheory,
        Choir,
        CreativeWorkshop,
        OrchestralPractice
    }

    /// <summary>
    /// Enumerates the possible class progress.
    /// </summary>
    public enum ClassProgress
    {
        Unknown,
        Registration,
        InProgress,
        Completed
    }

    /// <summary>
    /// Summary description for Class.
    /// </summary>
    [DataContract]
    public class Class
    {

        #region Fields *****************************************************************

        private int classId;
        private int semesterId;
        private int poleId;
        private int teacherId;
        private int subjectCode;
        private string code;
        private int classType;
        private int instrumentType;
        private int classLevel;
        private int capacity;
        private bool weekMonday;
        private bool weekTuesday;
        private bool weekWednesday;
        private bool weekThursday;
        private bool weekFriday;
        private bool weekSaturday;
        private bool weekSunday;
        private DateTime startTime;
        private int duration;
        private int classStatus;
        private DateTime creationTime;
        private DateTime inactivationTime;
        private string inactivationReason;

        /// <summary>
        /// The semester of the class.
        /// </summary>
        private Semester semester;

        /// <summary>
        /// The name of the pole.
        /// </summary>
        private string poleName;

        /// <summary>
        /// The name of the teacher.
        /// </summary>
        private string teacherName;

        /// <summary>
        /// The user id of the teacher.
        /// </summary>
        private int teacherUserId;

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
        public Class()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="classId">The id of the Class.</param>
        public Class(int classId)
        {
            this.classId = classId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.classId;}
            set{ this.classId = value;}
        }

        [DataMember]
        public int ClassId
        {
            get{ return this.classId;}
            set{ this.classId = value;}
        }

        [DataMember]
        public int SemesterId
        {
            get{ return this.semesterId;}
            set{ this.semesterId = value;}
        }

        [DataMember]
        public int PoleId
        {
            get{ return this.poleId;}
            set{ this.poleId = value;}
        }

        [DataMember]
        public int TeacherId
        {
            get{ return this.teacherId;}
            set{ this.teacherId = value;}
        }

        [DataMember]
        public int SubjectCode
        {
            get { return this.subjectCode; }
            set { this.subjectCode = value; }
        }

        [DataMember]
        public string Code
        {
            get{ return this.code;}
            set{ this.code = value;}
        }

        [DataMember]
        public int ClassType
        {
            get{ return this.classType;}
            set{ this.classType = value;}
        }

        [DataMember]
        public int InstrumentType
        {
            get{ return this.instrumentType;}
            set{ this.instrumentType = value;}
        }

        [DataMember]
        public int ClassLevel
        {
            get { return this.classLevel; }
            set { this.classLevel = value; }
        }

        [DataMember]
        public int Capacity
        {
            get{ return this.capacity;}
            set{ this.capacity = value;}
        }

        [DataMember]
        public bool WeekMonday
        {
            get { return this.weekMonday; }
            set { this.weekMonday = value; }
        }

        [DataMember]
        public bool WeekTuesday
        {
            get { return this.weekTuesday; }
            set { this.weekTuesday = value; }
        }

        [DataMember]
        public bool WeekWednesday
        {
            get { return this.weekWednesday; }
            set { this.weekWednesday = value; }
        }

        [DataMember]
        public bool WeekThursday
        {
            get { return this.weekThursday; }
            set { this.weekThursday = value; }
        }

        [DataMember]
        public bool WeekFriday
        {
            get { return this.weekFriday; }
            set { this.weekFriday = value; }
        }

        [DataMember]
        public bool WeekSaturday
        {
            get { return this.weekSaturday; }
            set { this.weekSaturday = value; }
        }

        [DataMember]
        public bool WeekSunday
        {
            get { return this.weekSunday; }
            set { this.weekSunday = value; }
        }

        [DataMember]
        public DateTime StartTime
        {
            get{ return this.startTime;}
            set{ this.startTime = value;}
        }

        [DataMember]
        public int Duration
        {
            get{ return this.duration;}
            set{ this.duration = value;}
        }

        [DataMember]
        public int ClassStatus
        {
            get{ return this.classStatus;}
            set{ this.classStatus = value;}
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
        /// Get/set the user id of the teacher.
        /// </summary>
        [DataMember]
        public int TeacherUserId
        {
            get
            {
                return teacherUserId;
            }

            set
            {
                teacherUserId = value;
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

        /// <summary>
        /// True if class type is theoretical.
        /// </summary>
        public bool IsTheoretical
        {
            get
            {
                //return if this class is theoretical
                return (
                    this.classType == (int)PnT.SongDB.Logic.ClassType.MusicTheory ||
                    this.classType == (int)PnT.SongDB.Logic.ClassType.CreativeWorkshop);
            }
        }

        /// <summary>
        /// Get the class progress.
        /// </summary>
        public int ClassProgress
        {
            get
            {
                //check if semester is set
                if (this.semester == null)
                {
                    //cannot calculate progress
                    return (int)PnT.SongDB.Logic.ClassProgress.Unknown;
                }

                //compare dates
                if (DateTime.Today < this.semester.StartDate)
                {
                    //semester has not started yet
                    return (int)PnT.SongDB.Logic.ClassProgress.Registration;
                }
                else if (DateTime.Today >= this.semester.EndDate)
                {
                    //semester is completed
                    return (int)PnT.SongDB.Logic.ClassProgress.Completed;
                }
                else
                {
                    //semester is in progress
                    return (int)PnT.SongDB.Logic.ClassProgress.InProgress;
                }
            }
        }

        #endregion Properties


        #region Methods ****************************************************************

        /// <summary>
        /// Save Class to database.
        /// </summary>
        /// <returns>The id of the saved Class.</returns>
        public int Save()
        {
            classId = Mapper.ClassMapper.Save(null, this);
            return classId;
        }

        /// <summary>
        /// Save Class to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Class.</returns>
        public int Save(MySqlTransaction trans)
        {
            classId = Mapper.ClassMapper.Save(trans, this);
            return classId;
        }

        /// <summary>
        /// Delete Class by id.
        /// </summary>
        /// <param name="id">The id of the selected Class.</param>
        /// <returns>
        /// True if selected Class was deleted.
        /// False if selected Class was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.ClassMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Class by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Class.</param>
        /// <returns>
        /// True if selected Class was deleted.
        /// False if selected Class was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.ClassMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Class by id.
        /// </summary>
        /// <param name="id">The id of the selected Class.</param>
        /// <param name="inactivationReason">
        /// The reason why the class is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Class was inactivated.
        /// False if selected Class was not found.
        /// </returns>
        public static bool Inactivate(int id, string inactivationReason)
        {
            return Mapper.ClassMapper.Inactivate(null, id, inactivationReason);
        }

        /// <summary>
        /// Inactivate Class by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Class.</param>
        /// <param name="inactivationReason">
        /// The reason why the class is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Class was inactivated.
        /// False if selected Class was not found.
        /// </returns>
        public static bool Inactivate(
            MySqlTransaction trans, int id, string inactivationReason)
        {
            return Mapper.ClassMapper.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all Class.
        /// </summary>
        /// <returns>
        /// List of Class objects.
        /// Null if no Class was found.
        /// </returns>
        public static List<Class> Find()
        {
            return Mapper.ClassMapper.Find(null);
        }

        /// <summary>
        /// Find all Class with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Class objects.
        /// Null if no Class was found.
        /// </returns>
        public static List<Class> Find(MySqlTransaction trans)
        {
            return Mapper.ClassMapper.Find(trans);
        }

        /// <summary>
        /// Count classes by filter.
        /// </summary>
        /// <param name="filterClassStatus">
        /// The class status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterClassType">
        /// The class type filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type.
        /// -1 to select all instrument types.
        /// </param>
        /// <param name="filterClassLevel">
        /// The class level filter.
        /// -1 to select all levels.
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
        /// <returns>
        /// The number of classes.
        /// </returns>
        public static int CountByFilter(
            int filterClassStatus, int filterClassType, int filterInstrumentType,
            int filterClassLevel, int filterSemester, int filterInstitution,
            int filterPole, int filterTeacher)
        {
            return Mapper.ClassMapper.CountByFilter(
                null, filterClassStatus, filterClassType, filterInstrumentType,
                filterClassLevel, filterSemester, filterInstitution, filterPole, filterTeacher);
        }

        /// <summary>
        /// Find classes by filter.
        /// </summary>
        /// <param name="filterClassStatus">
        /// The class status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterClassType">
        /// The class type filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type.
        /// -1 to select all instrument types.
        /// </param>
        /// <param name="filterClassLevel">
        /// The class level filter.
        /// -1 to select all levels.
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
        /// <returns>
        /// List of Class objects.
        /// Null if no Class was found.
        /// </returns>
        public static List<Class> FindByFilter(
            int filterClassStatus, int filterClassType, int filterInstrumentType, 
            int filterClassLevel, int filterSemester, int filterInstitution, 
            int filterPole, int filterTeacher)
        {
            return Mapper.ClassMapper.FindByFilter(
                null, filterClassStatus, filterClassType, filterInstrumentType, 
                filterClassLevel, filterSemester, filterInstitution, filterPole, filterTeacher);
        }

        /// <summary>
        /// Find classs by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="filterClassStatus">
        /// The class status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterClassType">
        /// The class type filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type.
        /// -1 to select all instrument types.
        /// </param>
        /// <param name="filterClassLevel">
        /// The class level filter.
        /// -1 to select all levels.
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
        /// <returns>
        /// List of Class objects.
        /// Null if no Class was found.
        /// </returns>
        public static List<Class> FindByFilter(
            MySqlTransaction trans, int filterClassStatus, int filterClassType,
            int filterInstrumentType, int filterClassLevel, int filterSemester, 
            int filterInstitution, int filterPole, int filterTeacher)
        {
            return Mapper.ClassMapper.FindByFilter(
                trans, filterClassStatus, filterClassType, filterInstrumentType, 
                filterClassLevel, filterSemester, filterInstitution, filterPole, filterTeacher);
        }

        /// <summary>
        /// Find all classes that selected student is registered to.
        /// </summary>
        /// <param name="studentId">The ID of the selected student.</param>
        /// <returns>
        /// List of Class objects.
        /// Null if no Class was found.
        /// </returns>
        public static List<Class> FindByStudent(int studentId)
        {
            return Mapper.ClassMapper.FindByStudent(null, studentId);
        }

        /// <summary>
        /// Find all classes that selected student is registered to.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="studentId">The ID of the selected student.</param>
        /// <returns>
        /// List of Class objects.
        /// Null if no Class was found.
        /// </returns>
        public static List<Class> FindByStudent(
            MySqlTransaction trans, int studentId)
        {
            return Mapper.ClassMapper.FindByStudent(trans, studentId);
        }

        /// <summary>
        /// Find Class by id.
        /// </summary>
        /// <param name="id">The id of the selected Class</param>
        /// <returns>
        /// The selected Class.
        /// Null if selected Class was not found.
        /// </returns>
        public static Class Find(int id)
        {
            return Mapper.ClassMapper.Find(null, id);
        }

        /// <summary>
        /// Find Class by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Class</param>
        /// <returns>
        /// The selected Class.
        /// Null if selected Class was not found.
        /// </returns>
        public static Class Find(MySqlTransaction trans, int id)
        {
            return Mapper.ClassMapper.Find(trans, id);
        }

        /// <summary>
        /// Find next available subject code.
        /// </summary>
        /// <returns>
        /// The next available subject code.
        /// </returns>
        public static int FindNextAvailableSubjectCode()
        {
            return Mapper.ClassMapper.FindNextAvailableSubjectCode(null);
        }

        /// <summary>
        /// Get description for this class.
        /// </summary>
        /// <returns>
        /// The created description.
        /// </returns>
        public IdDescriptionStatus GetDescription()
        {
            //create and return description.
            return new IdDescriptionStatus(
                this.classId, this.code, this.classStatus);
        }

        /// <summary>
        /// Get all class days for selected month.
        /// </summary>
        /// <param name="month">
        /// The selected month.
        /// </param>
        /// <returns>
        /// The list of class days. One date for each day.
        /// </returns>
        public List<DateTime> GetClassDays(DateTime month)
        {
            //normalize month
            month = new DateTime(month.Year, month.Month, 1);

            //check if semester is set
            if (semester == null)
            {
                //cannot calculate list withou semester
                return new List<DateTime>();
            }

            //check semester limits
            //compare with start month
            if (month < semester.StartMonth)
            {
                //no class day
                return new List<DateTime>();
            }

            //compare with end month
            if (month > semester.EndMonth)
            {
                //no class day
                return new List<DateTime>();
            }

            //check if month is the start month
            if (month == semester.StartMonth)
            {
                //use start date
                month = semester.StartDate;
            }

            //gather list of class days
            List<DateTime> classDays = new List<DateTime>();

            //check each day in month
            while (true)
            {
                //check day of the week
                if (month.DayOfWeek == DayOfWeek.Monday)
                {
                    //check if monday is class day
                    if (weekMonday)
                    {
                        //copy and add day to list
                        classDays.Add(new DateTime(month.Ticks));
                    }
                }
                else if (month.DayOfWeek == DayOfWeek.Tuesday)
                {
                    //check if tuesday is class day
                    if (weekTuesday)
                    {
                        //copy and add day to list
                        classDays.Add(new DateTime(month.Ticks));
                    }
                }
                else if (month.DayOfWeek == DayOfWeek.Wednesday)
                {
                    //check if wednesday is class day
                    if (weekWednesday)
                    {
                        //copy and add day to list
                        classDays.Add(new DateTime(month.Ticks));
                    }
                }
                else if (month.DayOfWeek == DayOfWeek.Thursday)
                {
                    //check if thursday is class day
                    if (weekThursday)
                    {
                        //copy and add day to list
                        classDays.Add(new DateTime(month.Ticks));
                    }
                }
                else if (month.DayOfWeek == DayOfWeek.Friday)
                {
                    //check if friday is class day
                    if (weekFriday)
                    {
                        //copy and add day to list
                        classDays.Add(new DateTime(month.Ticks));
                    }
                }
                else if (month.DayOfWeek == DayOfWeek.Saturday)
                {
                    //check if saturday is class day
                    if (weekSaturday)
                    {
                        //copy and add day to list
                        classDays.Add(new DateTime(month.Ticks));
                    }
                }
                else if (month.DayOfWeek == DayOfWeek.Sunday)
                {
                    //check if sunday is class day
                    if (weekSunday)
                    {
                        //copy and add day to list
                        classDays.Add(new DateTime(month.Ticks));
                    }
                }

                //increment day
                month = month.AddDays(1);

                //check month end
                if (month.Day == 1)
                {
                    //the month has reached its end
                    //exit loop
                    break;
                }

                //check semester end
                if (month > Semester.EndDate)
                {
                    //the month has reached the semester end
                    //exit loop
                    break;
                }
            }

            //create list of class days
            return classDays;
        }

        /// <summary>
        /// Copy this class object.
        /// </summary>
        /// <returns>The returned copy.</returns>
        public Class Copy()
        {
            return (Class)this.MemberwiseClone();
        }

        #endregion Methods

    } //end of class Class

} //end of namespace PnT.SongDB.Logic
