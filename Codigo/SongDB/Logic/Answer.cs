using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for Answer.
    /// </summary>
    [DataContract]
    public class Answer
    {

        #region Fields *****************************************************************

        private int answerId;
        private int questionId;
        private int reportId;
        private int semesterId;
        private int classId;
        private int institutionId;
        private int teacherId;
        private int coordinatorId;
        private int answerRapporteur;
        private int answerTarget;
        private int answerPeriodicity;
        private int answerMetric;
        private DateTime referenceDate;
        private int score;
        private string comments;

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
        public Answer()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="answerId">The id of the Answer.</param>
        public Answer(int answerId)
        {
            this.answerId = answerId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get { return this.answerId; }
            set { this.answerId = value; }
        }

        [DataMember]
        public int AnswerId
        {
            get { return this.answerId; }
            set { this.answerId = value; }
        }

        [DataMember]
        public int QuestionId
        {
            get { return this.questionId; }
            set { this.questionId = value; }
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
        public int AnswerRapporteur
        {
            get { return this.answerRapporteur; }
            set { this.answerRapporteur = value; }
        }

        [DataMember]
        public int AnswerTarget
        {
            get { return this.answerTarget; }
            set { this.answerTarget = value; }
        }

        [DataMember]
        public int AnswerPeriodicity
        {
            get { return this.answerPeriodicity; }
            set { this.answerPeriodicity = value; }
        }

        [DataMember]
        public int AnswerMetric
        {
            get { return this.answerMetric; }
            set { this.answerMetric = value; }
        }

        [DataMember]
        public DateTime ReferenceDate
        {
            get { return this.referenceDate; }
            set { this.referenceDate = value; }
        }

        [DataMember]
        public int Score
        {
            get { return this.score; }
            set { this.score = value; }
        }

        [DataMember]
        public string Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
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
        /// Save Answer to database.
        /// </summary>
        /// <returns>The id of the saved Answer.</returns>
        public int Save()
        {
            answerId = Mapper.AnswerMapper.Save(null, this);
            return answerId;
        }

        /// <summary>
        /// Save Answer to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Answer.</returns>
        public int Save(MySqlTransaction trans)
        {
            answerId = Mapper.AnswerMapper.Save(trans, this);
            return answerId;
        }

        /// <summary>
        /// Delete Answer by id.
        /// </summary>
        /// <param name="id">The id of the selected Answer.</param>
        /// <returns>
        /// True if selected Answer was deleted.
        /// False if selected Answer was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.AnswerMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Answer by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Answer.</param>
        /// <returns>
        /// True if selected Answer was deleted.
        /// False if selected Answer was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.AnswerMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Answer by id.
        /// </summary>
        /// <param name="id">The id of the selected Answer.</param>
        /// <returns>
        /// True if selected Answer was inactivated.
        /// False if selected Answer was not found.
        /// </returns>
        public static bool Inactivate(int id)
        {
            return Mapper.AnswerMapper.Inactivate(null, id);
        }

        /// <summary>
        /// Inactivate Answer by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Answer.</param>
        /// <returns>
        /// True if selected Answer was inactivated.
        /// False if selected Answer was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Mapper.AnswerMapper.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Answer.
        /// </summary>
        /// <returns>
        /// List of Answer objects.
        /// Null if no Answer was found.
        /// </returns>
        public static List<Answer> Find()
        {
            return Mapper.AnswerMapper.Find(null);
        }

        /// <summary>
        /// Find all Answer with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Answer objects.
        /// Null if no Answer was found.
        /// </returns>
        public static List<Answer> Find(MySqlTransaction trans)
        {
            return Mapper.AnswerMapper.Find(trans);
        }

        /// <summary>
        /// Find answers by filter.
        /// </summary>
        /// <param name="filterAnswerRapporteur">
        /// The answer rapporteur filter.
        /// -1 to select all rapporteurs.
        /// </param>
        /// <param name="filterAnswerTarget">
        /// The answer target filter.
        /// -1 to select all targets.
        /// </param>
        /// <param name="filterAnswerPeriodicity">
        /// The answer periodicity filter.
        /// -1 to select all periodicities.
        /// </param>
        /// <param name="filterAnswerMetric">
        /// The answer metric filter.
        /// -1 to select all metrics.
        /// </param>
        /// <param name="filterReport">
        /// The report filter.
        /// -1 to select all reports.
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
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of Answer objects.
        /// Null if no Answer was found.
        /// </returns>
        public static List<Answer> FindByFilter(
            int filterAnswerRapporteur, int filterAnswerTarget, int filterAnswerPeriodicity,
            int filterAnswerMetric, int filterReport, int filterSemester, DateTime filterReferenceDate,
            int filterInstitution, int filterTeacher, int filterCoordinator, int filterClass)
        {
            return Mapper.AnswerMapper.FindByFilter(
                null, filterAnswerRapporteur, filterAnswerTarget, filterAnswerPeriodicity,
                filterAnswerMetric, filterReport, filterSemester, filterReferenceDate,
                filterInstitution, filterTeacher, filterCoordinator, filterClass);
        }

        /// <summary>
        /// Find answers by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="filterAnswerRapporteur">
        /// The answer rapporteur filter.
        /// -1 to select all rapporteurs.
        /// </param>
        /// <param name="filterAnswerTarget">
        /// The answer target filter.
        /// -1 to select all targets.
        /// </param>
        /// <param name="filterAnswerPeriodicity">
        /// The answer periodicity filter.
        /// -1 to select all periodicities.
        /// </param>
        /// <param name="filterAnswerMetric">
        /// The answer metric filter.
        /// -1 to select all metrics.
        /// </param>
        /// <param name="filterReport">
        /// The report filter.
        /// -1 to select all reports.
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
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of Answer objects.
        /// Null if no Answer was found.
        /// </returns>
        public static List<Answer> FindByFilter(
            MySqlTransaction trans, int filterAnswerRapporteur, int filterAnswerTarget,
            int filterAnswerPeriodicity, int filterAnswerMetric, int filterReport,
            int filterSemester, DateTime filterReferenceDate, int filterInstitution,
            int filterTeacher, int filterCoordinator, int filterClass)
        {
            return Mapper.AnswerMapper.FindByFilter(
                trans, filterAnswerRapporteur, filterAnswerTarget, filterAnswerPeriodicity,
                filterAnswerMetric, filterReport, filterSemester, filterReferenceDate,
                filterInstitution, filterTeacher, filterCoordinator, filterClass);
        }

        /// <summary>
        /// Find Answer by id.
        /// </summary>
        /// <param name="id">The id of the selected Answer</param>
        /// <returns>
        /// The selected Answer.
        /// Null if selected Answer was not found.
        /// </returns>
        public static Answer Find(int id)
        {
            return Mapper.AnswerMapper.Find(null, id);
        }

        /// <summary>
        /// Find Answer by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Answer</param>
        /// <returns>
        /// The selected Answer.
        /// Null if selected Answer was not found.
        /// </returns>
        public static Answer Find(MySqlTransaction trans, int id)
        {
            return Mapper.AnswerMapper.Find(trans, id);
        }

        #endregion Methods

    } //end of class Answer

} //end of namespace PnT.SongDB.Logic
