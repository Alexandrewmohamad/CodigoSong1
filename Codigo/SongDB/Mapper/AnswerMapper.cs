using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for AnswerMapper.
    /// </summary>
    public class AnswerMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Answer to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Answer.</returns>
        public static int Save(MySqlTransaction trans, Answer answer)
        {
            return Access.AnswerAccess.Save(trans, GetParameters(answer));
        }

        /// <summary>
        /// Delete Answer by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Answer.</param>
        /// <returns>
        /// True if selected Answer was deleted.
        /// False if selected Answer was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.AnswerAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Answer by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Answer.</param>
        /// <returns>
        /// True if selected Answer was inactivated.
        /// False if selected Answer was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Access.AnswerAccess.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Answer.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Answer objects.
        /// Null if no Answer was found.
        /// </returns>
        public static List<Answer> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.AnswerAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Answer by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Answer.
        /// Null if selected Answer was not found.
        /// </returns>
        public static Answer Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.AnswerAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find answers by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
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
            DataRow[] dr = Access.AnswerAccess.FindByFilter(
                trans, filterAnswerRapporteur, filterAnswerTarget, filterAnswerPeriodicity,
                filterAnswerMetric, filterReport, filterSemester, filterReferenceDate,
                filterInstitution, filterTeacher, filterCoordinator, filterClass);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Answer objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Answer objects.</returns>
        private static List<Answer> Map(DataRow[] rows)
        {
            List<Answer> answers = new List<Answer>();

            for (int i = 0; i < rows.Length; i++)
                answers.Add(Map(rows[i]));

            return answers;
        }

        /// <summary>
        /// Map database row to a Answer object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Answer</returns>
        private static Answer Map(DataRow row)
        {
            Answer answer = new Answer((int)(row["answerId"]));
            answer.QuestionId = (int)DataAccessCommon.HandleDBNull(row, "questionId", typeof(int));
            answer.ReportId = (int)DataAccessCommon.HandleDBNull(row, "reportId", typeof(int));
            answer.SemesterId = (int)DataAccessCommon.HandleDBNull(row, "semesterId", typeof(int));
            answer.ClassId = (int)DataAccessCommon.HandleDBNull(row, "classId", typeof(int));
            answer.InstitutionId = (int)DataAccessCommon.HandleDBNull(row, "institutionId", typeof(int));
            answer.TeacherId = (int)DataAccessCommon.HandleDBNull(row, "teacherId", typeof(int));
            answer.CoordinatorId = (int)DataAccessCommon.HandleDBNull(row, "coordinatorId", typeof(int));
            answer.AnswerRapporteur = (int)DataAccessCommon.HandleDBNull(row, "answerRapporteur", typeof(int));
            answer.AnswerTarget = (int)DataAccessCommon.HandleDBNull(row, "answerTarget", typeof(int));
            answer.AnswerPeriodicity = (int)DataAccessCommon.HandleDBNull(row, "answerPeriodicity", typeof(int));
            answer.AnswerMetric = (int)DataAccessCommon.HandleDBNull(row, "answerMetric", typeof(int));
            answer.ReferenceDate = (DateTime)DataAccessCommon.HandleDBNull(row, "referenceDate", typeof(DateTime));
            answer.Score = (int)DataAccessCommon.HandleDBNull(row, "score", typeof(int));
            answer.Comments = (string)DataAccessCommon.HandleDBNull(row, "comments", typeof(string));

            return answer;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Answer
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Answer answer)
        {
            MySqlParameter[] parameters = new MySqlParameter[15];
            parameters[0] = new MySqlParameter("answerId", answer.Id);
            parameters[1] = new MySqlParameter("questionId", answer.QuestionId);
            parameters[2] = new MySqlParameter("reportId", DataAccessCommon.HandleDBNull(answer.ReportId));
            parameters[3] = new MySqlParameter("semesterId", answer.SemesterId);
            parameters[4] = new MySqlParameter("classId", DataAccessCommon.HandleDBNull(answer.ClassId));
            parameters[5] = new MySqlParameter("institutionId", DataAccessCommon.HandleDBNull(answer.InstitutionId));
            parameters[6] = new MySqlParameter("teacherId", DataAccessCommon.HandleDBNull(answer.TeacherId));
            parameters[7] = new MySqlParameter("coordinatorId", DataAccessCommon.HandleDBNull(answer.CoordinatorId));
            parameters[8] = new MySqlParameter("answerRapporteur", answer.AnswerRapporteur);
            parameters[9] = new MySqlParameter("answerTarget", answer.AnswerTarget);
            parameters[10] = new MySqlParameter("answerPeriodicity", answer.AnswerPeriodicity);
            parameters[11] = new MySqlParameter("answerMetric", answer.AnswerMetric);
            parameters[12] = new MySqlParameter("referenceDate", answer.ReferenceDate);
            parameters[13] = new MySqlParameter("score", DataAccessCommon.HandleDBNull(answer.Score));
            parameters[14] = new MySqlParameter("comments", DataAccessCommon.HandleDBNull(answer.Comments));

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class AnswerMapper

} //end of namespace PnT.SongDB.Mapper
