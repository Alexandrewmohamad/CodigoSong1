using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Enumerates the possible grade rapporteurs.
    /// </summary>
    public enum GradeRapporteur
    {
        Teacher = 0,
        Coordinator
    }

    /// <summary>
    /// Enumerates the possible grade targets.
    /// </summary>
    public enum GradeTarget
    {
        Teacher = 0,
        Student
    }

    /// <summary>
    /// Enumerates the possible grade periodicities.
    /// </summary>
    public enum GradePeriodicity
    {
        Month = 0,
        Semester
    }

    /// <summary>
    /// Enumerates the possible grade subjects.
    /// </summary>
    public enum GradeSubject
    {
        Discipline = 0, 
        Performance,
        Dedication,
        Punctuality,
        Charisma,
        Proactivity,
        Creativity,
        Organization,
        Didactic,
        Tuning,
        Pulse,
        Expressiveness,
        Posture,
        Concentration,
        Teamwork,
        ObjectiveReached,
        StudentsLearningCurve,
        AuditionOrganization,
        ProjectCommitment
    }

    /// <summary>
    /// Enumerates the roll call values.
    /// </summary>
    public enum GradeSpecialScore
    {
        Empty = -1,
        Ungraded = -2
    }

    /// <summary>
    /// Summary description for Grade.
    /// </summary>
    [DataContract]
    public class Grade
    {

        #region Fields *****************************************************************

        private int gradeId;
        private int semesterId;
        private int studentId;
        private int teacherId;
        private int coordinatorId;
        private int classId;
        private int institutionId;
        private int gradeRapporteur;
        private int gradeTarget;
        private int gradePeriodicity;
        private DateTime referenceDate;
        private int gradeSubject;
        private int score;

        /// <summary>
        /// The description of the semester.
        /// </summary>
        private string semesterDescription;

        /// <summary>
        /// The name of the institution.
        /// </summary>
        private string institutionName;

        /// <summary>
        /// The name of the teacher.
        /// </summary>
        private string teacherName;

        /// <summary>
        /// The ID of the teacher user.
        /// </summary>
        private int teacherUserId;

        /// <summary>
        /// The name of the coordinator.
        /// </summary>
        private string coordinatorName;

        /// <summary>
        /// The name of the student.
        /// </summary>
        private string studentName;

        /// <summary>
        /// The attended class
        /// </summary>
        private Class objClass;

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
        public Grade()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="gradeId">The id of the Grade.</param>
        public Grade(int gradeId)
        {
            this.gradeId = gradeId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get { return this.gradeId; }
            set { this.gradeId = value; }
        }

        [DataMember]
        public int GradeId
        {
            get { return this.gradeId; }
            set { this.gradeId = value; }
        }

        [DataMember]
        public int SemesterId
        {
            get { return this.semesterId; }
            set { this.semesterId = value; }
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
        public int CoordinatorId
        {
            get { return this.coordinatorId; }
            set { this.coordinatorId = value; }
        }

        [DataMember]
        public int ClassId
        {
            get { return this.classId; }
            set { this.classId = value; }
        }

        [DataMember]
        public int InstitutionId
        {
            get { return this.institutionId; }
            set { this.institutionId = value; }
        }

        [DataMember]
        public int GradeRapporteur
        {
            get { return this.gradeRapporteur; }
            set { this.gradeRapporteur = value; }
        }

        [DataMember]
        public int GradeTarget
        {
            get { return this.gradeTarget; }
            set { this.gradeTarget = value; }
        }

        [DataMember]
        public int GradePeriodicity
        {
            get { return this.gradePeriodicity; }
            set { this.gradePeriodicity = value; }
        }

        [DataMember]
        public DateTime ReferenceDate
        {
            get { return this.referenceDate; }
            set { this.referenceDate = value; }
        }

        [DataMember]
        public int GradeSubject
        {
            get { return this.gradeSubject; }
            set { this.gradeSubject = value; }
        }

        [DataMember]
        public int Score
        {
            get { return this.score; }
            set { this.score = value; }
        }

        /// <summary>
        /// Get/set the description of the semester.
        /// </summary>
        [DataMember]
        public string SemesterDescription
        {
            get
            {
                return semesterDescription;
            }

            set
            {
                semesterDescription = value;
            }
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
        /// Get/set the ID of the teacher user.
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
        /// Save Grade to database.
        /// </summary>
        /// <returns>The id of the saved Grade.</returns>
        public int Save()
        {
            gradeId = Mapper.GradeMapper.Save(null, this);
            return gradeId;
        }

        /// <summary>
        /// Save Grade to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Grade.</returns>
        public int Save(MySqlTransaction trans)
        {
            gradeId = Mapper.GradeMapper.Save(trans, this);
            return gradeId;
        }

        /// <summary>
        /// Delete Grade by id.
        /// </summary>
        /// <param name="id">The id of the selected Grade.</param>
        /// <returns>
        /// True if selected Grade was deleted.
        /// False if selected Grade was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.GradeMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Grade by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Grade.</param>
        /// <returns>
        /// True if selected Grade was deleted.
        /// False if selected Grade was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.GradeMapper.Delete(trans, id);
        }

        /// <summary>
        /// Delete all Grade for selected class student.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="classId">The id of the selected class.</param>
        /// <param name="studentId">The id of the selected student.</param>
        /// <returns>
        /// The number of deleted Grades.
        /// </returns>
        public static int DeleteForClassStudent(
            MySqlTransaction trans, int classId, int studentId)
        {
            return Mapper.GradeMapper.DeleteForClassStudent(
                trans, classId, studentId);
        }

        /// <summary>
        /// Inactivate Grade by id.
        /// </summary>
        /// <param name="id">The id of the selected Grade.</param>
        /// <returns>
        /// True if selected Grade was inactivated.
        /// False if selected Grade was not found.
        /// </returns>
        public static bool Inactivate(int id)
        {
            return Mapper.GradeMapper.Inactivate(null, id);
        }

        /// <summary>
        /// Inactivate Grade by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Grade.</param>
        /// <returns>
        /// True if selected Grade was inactivated.
        /// False if selected Grade was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Mapper.GradeMapper.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Grade.
        /// </summary>
        /// <returns>
        /// List of Grade objects.
        /// Null if no Grade was found.
        /// </returns>
        public static List<Grade> Find()
        {
            return Mapper.GradeMapper.Find(null);
        }

        /// <summary>
        /// Find all Grade with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Grade objects.
        /// Null if no Grade was found.
        /// </returns>
        public static List<Grade> Find(MySqlTransaction trans)
        {
            return Mapper.GradeMapper.Find(trans);
        }

        /// <summary>
        /// Average attendance score by filter.
        /// </summary>
        /// <param name="filterGradeRapporteur">
        /// The grade rapporteur filter.
        /// -1 to select all rapporteurs.
        /// </param>
        /// <param name="filterGradeTarget">
        /// The grade target filter.
        /// -1 to select all targets.
        /// </param>
        /// <param name="filterGradePeriodicity">
        /// The grade periodicity filter.
        /// -1 to select all periodicities.
        /// </param>
        /// <param name="filterGradeSubject">
        /// The grade subject filter.
        /// -1 to select all subjects.
        /// </param>
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
        /// <param name="filterCoordinator">
        /// The coordinator filter.
        /// -1 to select all coordinators.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// The average score of filtered grades.
        /// </returns>
        public static double AverageByFilter(
            int filterGradeRapporteur, int filterGradeTarget, int filterGradePeriodicity,
            int filterGradeSubject, int filterSemester, DateTime filterReferenceDate,
            int filterInstitution, int filterTeacher, int filterCoordinator,
            int filterStudent, int filterClass)
        {
            return Mapper.GradeMapper.AverageByFilter(
                null, filterGradeRapporteur, filterGradeTarget, filterGradePeriodicity,
                filterGradeSubject, filterSemester, filterReferenceDate, filterInstitution,
                filterTeacher, filterCoordinator, filterStudent, filterClass);
        }

        /// <summary>
        /// Find grades by filter.
        /// </summary>
        /// <param name="filterGradeRapporteur">
        /// The grade rapporteur filter.
        /// -1 to select all rapporteurs.
        /// </param>
        /// <param name="filterGradeTarget">
        /// The grade target filter.
        /// -1 to select all targets.
        /// </param>
        /// <param name="filterGradePeriodicity">
        /// The grade periodicity filter.
        /// -1 to select all periodicities.
        /// </param>
        /// <param name="filterGradeSubject">
        /// The grade subject filter.
        /// -1 to select all subjects.
        /// </param>
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
        /// <param name="filterPole">
        /// The pole filter
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterCoordinator">
        /// The coordinator filter.
        /// -1 to select all coordinators.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of Grade objects.
        /// Null if no Grade was found.
        /// </returns>
        public static List<Grade> FindByFilter(
            int filterGradeRapporteur, int filterGradeTarget, int filterGradePeriodicity,
            int filterGradeSubject, int filterSemester, DateTime filterReferenceDate,
            int filterInstitution, int filterPole, int filterTeacher, 
            int filterCoordinator, int filterStudent, int filterClass)
        {
            return Mapper.GradeMapper.FindByFilter(
                null, filterGradeRapporteur, filterGradeTarget, filterGradePeriodicity,
                filterGradeSubject, filterSemester, filterReferenceDate, filterInstitution,
                filterPole, filterTeacher, filterCoordinator, filterStudent, filterClass);
        }

        /// <summary>
        /// Find grades by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="filterGradeRapporteur">
        /// The grade rapporteur filter.
        /// -1 to select all rapporteurs.
        /// </param>
        /// <param name="filterGradeTarget">
        /// The grade target filter.
        /// -1 to select all targets.
        /// </param>
        /// <param name="filterGradePeriodicity">
        /// The grade periodicity filter.
        /// -1 to select all periodicities.
        /// </param>
        /// <param name="filterGradeSubject">
        /// The grade subject filter.
        /// -1 to select all subjects.
        /// </param>
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
        /// <param name="filterPole">
        /// The pole filter
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterCoordinator">
        /// The coordinator filter.
        /// -1 to select all coordinators.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of Grade objects.
        /// Null if no Grade was found.
        /// </returns>
        public static List<Grade> FindByFilter(
            MySqlTransaction trans, int filterGradeRapporteur, int filterGradeTarget, int filterGradePeriodicity,
            int filterGradeSubject, int filterSemester, DateTime filterReferenceDate,
            int filterInstitution, int filterPole, int filterTeacher, 
            int filterCoordinator, int filterStudent, int filterClass)
        {
            return Mapper.GradeMapper.FindByFilter(
                trans, filterGradeRapporteur, filterGradeTarget, filterGradePeriodicity,
                filterGradeSubject, filterSemester, filterReferenceDate, filterInstitution, 
                filterPole, filterTeacher, filterCoordinator, filterStudent, filterClass);
        }

        /// <summary>
        /// Find Grade by id.
        /// </summary>
        /// <param name="id">The id of the selected Grade</param>
        /// <returns>
        /// The selected Grade.
        /// Null if selected Grade was not found.
        /// </returns>
        public static Grade Find(int id)
        {
            return Mapper.GradeMapper.Find(null, id);
        }

        /// <summary>
        /// Find Grade by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Grade</param>
        /// <returns>
        /// The selected Grade.
        /// Null if selected Grade was not found.
        /// </returns>
        public static Grade Find(MySqlTransaction trans, int id)
        {
            return Mapper.GradeMapper.Find(trans, id);
        }

        #endregion Methods

    } //end of class Grade

} //end of namespace PnT.SongDB.Logic
