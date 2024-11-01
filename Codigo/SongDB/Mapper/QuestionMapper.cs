using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for QuestionMapper.
    /// </summary>
    public class QuestionMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Question to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Question.</returns>
        public static int Save(MySqlTransaction trans, Question question)
        {
            return Access.QuestionAccess.Save(trans, GetParameters(question));
        }

        /// <summary>
        /// Delete Question by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Question.</param>
        /// <returns>
        /// True if selected Question was deleted.
        /// False if selected Question was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.QuestionAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Question by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Question.</param>
        /// <returns>
        /// True if selected Question was inactivated.
        /// False if selected Question was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Access.QuestionAccess.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Question.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Question objects.
        /// Null if no Question was found.
        /// </returns>
        public static List<Question> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.QuestionAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Question by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Question.
        /// Null if selected Question was not found.
        /// </returns>
        public static Question Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.QuestionAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Question objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Question objects.</returns>
        private static List<Question> Map(DataRow[] rows)
        {
            List<Question> questions = new List<Question>();

            for (int i = 0; i < rows.Length; i++)
                questions.Add(Map(rows[i]));

            return questions;
        }

        /// <summary>
        /// Map database row to a Question object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Question</returns>
        private static Question Map(DataRow row)
        {
            Question question = new Question((int)(row["questionId"]));
            question.QuestionRapporteur = (int)DataAccessCommon.HandleDBNull(row, "questionRapporteur", typeof(int));
            question.QuestionTarget = (int)DataAccessCommon.HandleDBNull(row, "questionTarget", typeof(int));
            question.QuestionPeriodicity = (int)DataAccessCommon.HandleDBNull(row, "questionPeriodicity", typeof(int));
            question.QuestionMetric = (int)DataAccessCommon.HandleDBNull(row, "questionMetric", typeof(int));
            question.CommentsRequired = (bool)DataAccessCommon.HandleDBNull(row, "commentsRequired", typeof(bool));
            question.Text = (string)DataAccessCommon.HandleDBNull(row, "text", typeof(string));
            question.PlusLabel = (string)DataAccessCommon.HandleDBNull(row, "plusLabel", typeof(string));
            question.MinusLabel = (string)DataAccessCommon.HandleDBNull(row, "minusLabel", typeof(string));

            return question;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Question
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Question question)
        {
            MySqlParameter[] parameters = new MySqlParameter[9];
            parameters[0] = new MySqlParameter("questionId", question.Id);
            parameters[1] = new MySqlParameter("questionRapporteur", question.QuestionRapporteur);
            parameters[2] = new MySqlParameter("questionTarget", question.QuestionTarget);
            parameters[3] = new MySqlParameter("questionPeriodicity", question.QuestionPeriodicity);
            parameters[4] = new MySqlParameter("questionMetric", question.QuestionMetric);
            parameters[5] = new MySqlParameter("commentsRequired", question.CommentsRequired);
            parameters[6] = new MySqlParameter("text", DataAccessCommon.HandleDBNull(question.Text));
            parameters[7] = new MySqlParameter("plusLabel", DataAccessCommon.HandleDBNull(question.PlusLabel));
            parameters[8] = new MySqlParameter("minusLabel", DataAccessCommon.HandleDBNull(question.MinusLabel));

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class QuestionMapper

} //end of namespace PnT.SongDB.Mapper
