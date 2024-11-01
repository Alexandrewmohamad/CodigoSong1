using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Enumerates the possible report rapporteurs.
    /// </summary>
    public enum ReportRapporteur
    {
        Teacher = 0,
        Coordinator
    }

    /// <summary>
    /// Enumerates the possible report targets.
    /// </summary>
    public enum ReportTarget
    {
        Class = 0,
        Institution
    }

    /// <summary>
    /// Enumerates the possible report periodicities.
    /// </summary>
    public enum ReportPeriodicity
    {
        Month = 0,
        Semester
    }

    /// <summary>
    /// Enumerates the possible report statuses.
    /// </summary>
    public enum ReportStatus
    {
        Completed = 0,
        Pending
    }

    /// <summary>
    /// Summary description for Report.
    /// </summary>
    [DataContract]
    public class Report
    {

        #region Constants **************************************************************

        /// <summary>
        /// REFERENCE separator.
        /// </summary>
        public const string REFERENCE_SEPARATOR = "|";

        #endregion Constants


        #region Fields *****************************************************************

        private int reportId;
        private int semesterId;
        private int classId;
        private int institutionId;
        private int teacherId;
        private int coordinatorId;
        private int reportRapporteur;
        private int reportTarget;
        private int reportPeriodicity;
        private DateTime referenceDate;
        private string referenceList;
        private int reportStatus;

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
        /// The ID of the coordinator user.
        /// </summary>
        private int coordinatorUserId;

        /// <summary>
        /// The attended class
        /// </summary>
        private Class objClass;

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
        public Report()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="reportId">The id of the Report.</param>
        public Report(int reportId)
        {
            this.reportId = reportId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get { return this.reportId; }
            set { this.reportId = value; }
        }

        [DataMember]
        public int ReportId
        {
            get { return this.reportId; }
            set { this.reportId = value; }
        }

        [DataMember]
        public int SemesterId
        {
            get { return this.semesterId; }
            set { this.semesterId = value; }
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
        public int ReportRapporteur
        {
            get { return this.reportRapporteur; }
            set { this.reportRapporteur = value; }
        }

        [DataMember]
        public int ReportTarget
        {
            get { return this.reportTarget; }
            set { this.reportTarget = value; }
        }

        [DataMember]
        public int ReportPeriodicity
        {
            get { return this.reportPeriodicity; }
            set { this.reportPeriodicity = value; }
        }

        [DataMember]
        public DateTime ReferenceDate
        {
            get { return this.referenceDate; }
            set { this.referenceDate = value; }
        }

        [DataMember]
        public string ReferenceList
        {
            get { return this.referenceList; }
            set { this.referenceList = value; }
        }

        [DataMember]
        public int ReportStatus
        {
            get { return this.reportStatus; }
            set { this.reportStatus = value; }
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
        /// Get/set the ID of the coordinator user.
        /// </summary>
        [DataMember]
        public int CoordinatorUserId
        {
            get
            {
                return coordinatorUserId;
            }

            set
            {
                coordinatorUserId = value;
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
        /// Save Report to database.
        /// </summary>
        /// <returns>The id of the saved Report.</returns>
        public int Save()
        {
            reportId = Mapper.ReportMapper.Save(null, this);
            return reportId;
        }

        /// <summary>
        /// Save Report to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Report.</returns>
        public int Save(MySqlTransaction trans)
        {
            reportId = Mapper.ReportMapper.Save(trans, this);
            return reportId;
        }

        /// <summary>
        /// Delete Report by id.
        /// </summary>
        /// <param name="id">The id of the selected Report.</param>
        /// <returns>
        /// True if selected Report was deleted.
        /// False if selected Report was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.ReportMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Report by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Report.</param>
        /// <returns>
        /// True if selected Report was deleted.
        /// False if selected Report was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.ReportMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Report by id.
        /// </summary>
        /// <param name="id">The id of the selected Report.</param>
        /// <returns>
        /// True if selected Report was inactivated.
        /// False if selected Report was not found.
        /// </returns>
        public static bool Inactivate(int id)
        {
            return Mapper.ReportMapper.Inactivate(null, id);
        }

        /// <summary>
        /// Inactivate Report by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Report.</param>
        /// <returns>
        /// True if selected Report was inactivated.
        /// False if selected Report was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Mapper.ReportMapper.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Report.
        /// </summary>
        /// <returns>
        /// List of Report objects.
        /// Null if no Report was found.
        /// </returns>
        public static List<Report> Find()
        {
            return Mapper.ReportMapper.Find(null);
        }

        /// <summary>
        /// Find all Report with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Report objects.
        /// Null if no Report was found.
        /// </returns>
        public static List<Report> Find(MySqlTransaction trans)
        {
            return Mapper.ReportMapper.Find(trans);
        }

        /// <summary>
        /// Find reports by filter.
        /// </summary>
        /// <param name="filterReportStatus">
        /// The report status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterReportRapporteur">
        /// The report rapporteur filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterReportPeriodicity">
        /// The report periodicity filter.
        /// -1 to select all instrument types.
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
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of Report objects.
        /// Null if no Report was found.
        /// </returns>
        public static List<Report> FindByFilter(
           int filterReportStatus, int filterReportRapporteur, int filterReportPeriodicity,
            int filterSemester, DateTime filterReferenceDate, int filterInstitution, 
            int filterTeacher, int filterClass)
        {
            return Mapper.ReportMapper.FindByFilter(
                null, filterReportStatus, filterReportRapporteur, filterReportPeriodicity,
                filterSemester, filterReferenceDate, filterInstitution, filterTeacher, filterClass);
        }

        /// <summary>
        /// Find reports by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="filterReportStatus">
        /// The report status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterReportRapporteur">
        /// The report rapporteur filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterReportPeriodicity">
        /// The report periodicity filter.
        /// -1 to select all instrument types.
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
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of Report objects.
        /// Null if no Report was found.
        /// </returns>
        public static List<Report> FindByFilter(
            MySqlTransaction trans, int filterReportStatus, int filterReportRapporteur, 
            int filterReportPeriodicity, int filterSemester, DateTime filterReferenceDate, 
            int filterInstitution, int filterTeacher, int filterClass)
        {
            return Mapper.ReportMapper.FindByFilter(
                trans, filterReportStatus, filterReportRapporteur, filterReportPeriodicity,
                filterSemester, filterReferenceDate, filterInstitution, filterTeacher, filterClass);
        }

        /// <summary>
        /// Find Report by id.
        /// </summary>
        /// <param name="id">The id of the selected Report</param>
        /// <returns>
        /// The selected Report.
        /// Null if selected Report was not found.
        /// </returns>
        public static Report Find(int id)
        {
            return Mapper.ReportMapper.Find(null, id);
        }

        /// <summary>
        /// Find Report by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Report</param>
        /// <returns>
        /// The selected Report.
        /// Null if selected Report was not found.
        /// </returns>
        public static Report Find(MySqlTransaction trans, int id)
        {
            return Mapper.ReportMapper.Find(trans, id);
        }

        /// <summary>
        /// Get list of references.
        /// </summary>
        /// <returns>
        /// The list of references.
        /// </returns>
        public List<int> GetReferences()
        {
            try
            {
                //check reference list
                if (this.referenceList == null ||
                    this.referenceList.Length == 0)
                {
                    //no comment
                    //return empty list
                    return new List<int>();
                }

                //create list of references
                List<int> parsedReferences = new List<int>();

                //split reference list
                string[] words = this.referenceList.Split(
                    new string[] { REFERENCE_SEPARATOR },
                    StringSplitOptions.RemoveEmptyEntries);

                //parse each comment
                foreach (string word in words)
                {
                    //parse and add reference
                    parsedReferences.Add(int.Parse(word));
                }

                //return list of parsed references
                return parsedReferences;
            }
            catch
            {
                //error while getting references
                //should never happen
                //return empty list
                return new List<int>();
            }
        }

        /// <summary>
        /// Set reference list with list of references.
        /// </summary>
        /// <param name="references">
        /// The list of references.
        /// </param>
        public void SetReferenceList(List<int> references)
        {
            //check referenceList
            if (references == null || references.Count == 0)
            {
                //set empty reference list
                this.referenceList = string.Empty;

                //exit
                return;
            }

            //create string with reference list
            StringBuilder sbReferenceList = new StringBuilder(32 * references.Count);

            //cheack each comment
            foreach (int reference in references)
            {
                //add comment
                sbReferenceList.Append(reference.ToString());

                //add separator
                sbReferenceList.Append(REFERENCE_SEPARATOR);
            }

            //remove last separator
            sbReferenceList.Length -= REFERENCE_SEPARATOR.Length;

            //set reference list with result
            this.referenceList = sbReferenceList.ToString();
        }

        #endregion Methods

    } //end of class Report

} //end of namespace PnT.SongDB.Logic
