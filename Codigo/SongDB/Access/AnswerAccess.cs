using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;


namespace PnT.SongDB.Access
{

    /// <summary>
    /// Summary description for AnswerAccess.
    /// </summary>
    public class AnswerAccess
    {

        #region Queries ****************************************************************

        private const string FIND_ANSWER = @"SELECT * FROM answer {0} {1} {2}";

        private const string SAVE_ANSWER = @"INSERT INTO answer (questionId, reportId, semesterId, classId, institutionId, teacherId, coordinatorId, answerRapporteur, answerTarget, answerPeriodicity, answerMetric, referenceDate, score, comments) 
            VALUES (@questionId, @reportId, @semesterId, @classId, @institutionId, @teacherId, @coordinatorId, @answerRapporteur, @answerTarget, @answerPeriodicity, @answerMetric, @referenceDate, @score, @comments); 
            SELECT @@IDENTITY;";

        private const string UPDATE_ANSWER = @"UPDATE answer 
            SET questionId=@questionId, reportId=@reportId, semesterId=@semesterId, classId=@classId, institutionId=@institutionId, teacherId=@teacherId, coordinatorId=@coordinatorId, answerRapporteur=@answerRapporteur, answerTarget=@answerTarget, answerPeriodicity=@answerPeriodicity, answerMetric=@answerMetric, referenceDate=@referenceDate, score=@score, comments=@comments
             WHERE answerId=@answerId";

        private const string DELETE_ANSWER = @"DELETE FROM answer WHERE answerId=@answerId";

        private const string INACTIVATE_ANSWER = @"UPDATE answer SET answerStatus=1 WHERE answerId=@answerId";

        #endregion Queries


        #region Methods ****************************************************************

        /// <summary>
        /// Save Answer to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Answer.</returns>
        public static int Save(MySqlTransaction trans, MySqlParameter[] parameters)
        {
            MySqlTransaction transaction = null;
            MySqlConnection connection = null;
            bool closeTransaction = true;
            bool closeConnection = true;
            int n;
            int id;

            try
            {
                //checks if there is a selected transaction
                if (trans != null)
                {
                    transaction = trans;
                    connection = trans.Connection;
                    closeConnection = false;
                    closeTransaction = false;
                }
                else
                {
                    connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                    connection.Open();
                    transaction = connection.BeginTransaction();
                }

                //check if it is a new row
                if ((int)parameters[0].Value == -1)
                {
                    //insert new row
                    MySqlCommand comm = new MySqlCommand();
                    comm.CommandText = SAVE_ANSWER;
                    comm.Connection = connection;
                    comm.Transaction = transaction;
                    comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                    comm.Parameters.AddRange(parameters);

                    id = (int)Convert.ToUInt64(comm.ExecuteScalar());
                }
                else
                {
                    //update existing row
                    MySqlCommand comm = new MySqlCommand();
                    comm.CommandText = UPDATE_ANSWER;
                    comm.Connection = connection;
                    comm.Transaction = transaction;
                    comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                    comm.Parameters.AddRange(parameters);

                    n = comm.ExecuteNonQuery();
                    id = (int)parameters[0].Value;
                }

                if (closeTransaction && transaction != null)
                    transaction.Commit();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                return id;
            }
            catch (Exception ex)
            {
                if (closeTransaction && transaction != null)
                    transaction.Rollback();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                throw (ex);
            }
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
            MySqlTransaction transaction = null;
            MySqlConnection connection = null;
            bool closeTransaction = true;
            bool closeConnection = true;
            int n;

            try
            {
                //checks if there is a selected transaction
                if (trans != null)
                {
                    transaction = trans;
                    connection = trans.Connection;
                    closeConnection = false;
                    closeTransaction = false;
                }
                else
                {
                    connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                    connection.Open();
                    transaction = connection.BeginTransaction();
                }

                //delete existing row
                MySqlCommand comm = new MySqlCommand();
                comm.CommandText = DELETE_ANSWER;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@answerId", id);

                n = comm.ExecuteNonQuery();

                if (closeTransaction && transaction != null)
                    transaction.Commit();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                if (n > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                if (closeTransaction && transaction != null)
                    transaction.Rollback();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                throw (ex);
            }
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
            MySqlTransaction transaction = null;
            MySqlConnection connection = null;
            bool closeTransaction = true;
            bool closeConnection = true;
            int n;

            try
            {
                //checks if there is a selected transaction
                if (trans != null)
                {
                    transaction = trans;
                    connection = trans.Connection;
                    closeConnection = false;
                    closeTransaction = false;
                }
                else
                {
                    connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                    connection.Open();
                    transaction = connection.BeginTransaction();
                }

                //inactivate existing row
                MySqlCommand comm = new MySqlCommand();
                comm.CommandText = INACTIVATE_ANSWER;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@answerId", id);

                n = comm.ExecuteNonQuery();

                if (closeTransaction && transaction != null)
                    transaction.Commit();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                if (n > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                if (closeTransaction && transaction != null)
                    transaction.Rollback();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                throw (ex);
            }
        }

        /// <summary>
        /// Find all Answer.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no Answer was found.
        /// </returns>
        public static DataRow[] Find(MySqlTransaction trans)
        {
            MySqlTransaction transaction = null;
            MySqlConnection connection = null;
            bool closeTransaction = true;
            bool closeConnection = true;
            int n;

            try
            {
                DataTable dt = new DataTable();

                using (MySqlDataAdapter da = new MySqlDataAdapter())
                {

                    //checks if there is a selected transaction
                    if (trans != null)
                    {
                        transaction = trans;
                        connection = trans.Connection;
                        closeConnection = false;
                        closeTransaction = false;
                    }
                    else
                    {
                        closeTransaction = false;
                        connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                        connection.Open();
                    }

                    string orderBy = " ORDER BY answerId";
                    string query = string.Format(FIND_ANSWER, string.Empty, orderBy, string.Empty);

                    da.SelectCommand = new MySqlCommand();
                    da.SelectCommand.Connection = connection;
                    da.SelectCommand.Transaction = transaction;
                    da.SelectCommand.CommandText = query;
                    da.SelectCommand.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;

                    n = da.Fill(dt);
                }

                if (closeTransaction && transaction != null)
                    transaction.Commit();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                if (n > 0)
                    return dt.Select();
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (closeTransaction && transaction != null)
                    transaction.Rollback();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                throw (ex);
            }
        }

        /// <summary>
        /// Find Answer by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected database row.
        /// Null if no Answer was found.
        /// </returns>
        public static DataRow Find(MySqlTransaction trans, int id)
        {
            MySqlTransaction transaction = null;
            MySqlConnection connection = null;
            bool closeTransaction = true;
            bool closeConnection = true;
            int n;

            try
            {
                DataTable dt = new DataTable();

                using (MySqlDataAdapter da = new MySqlDataAdapter())
                {

                    //checks if there is a selected transaction
                    if (trans != null)
                    {
                        transaction = trans;
                        connection = trans.Connection;
                        closeConnection = false;
                        closeTransaction = false;
                    }
                    else
                    {
                        connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                        connection.Open();
                        closeTransaction = false;
                    }

                    string where = " WHERE answerId = @id ";
                    string query = string.Format(FIND_ANSWER, where, string.Empty, string.Empty);

                    da.SelectCommand = new MySqlCommand();
                    da.SelectCommand.Connection = connection;
                    da.SelectCommand.Transaction = transaction;
                    da.SelectCommand.CommandText = query;
                    da.SelectCommand.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                    da.SelectCommand.Parameters.AddWithValue("@id", id);

                    n = da.Fill(dt);
                }

                if (closeTransaction && transaction != null)
                    transaction.Commit();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                if (n > 0)
                    return dt.Rows[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (closeTransaction && transaction != null)
                    transaction.Rollback();
                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                throw (ex);
            }
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
        /// List of database rows.
        /// Null if no Answer was found.
        /// </returns>
        public static DataRow[] FindByFilter(
            MySqlTransaction trans, int filterAnswerRapporteur, int filterAnswerTarget, 
            int filterAnswerPeriodicity, int filterAnswerMetric, int filterReport, 
            int filterSemester, DateTime filterReferenceDate, int filterInstitution, 
            int filterTeacher, int filterCoordinator, int filterClass)
        {
            MySqlTransaction transaction = null;
            MySqlConnection connection = null;
            bool closeTransaction = true;
            bool closeConnection = true;
            int n;

            try
            {
                DataTable dt = new DataTable();

                using (MySqlDataAdapter da = new MySqlDataAdapter())
                {

                    //checks if there is a selected transaction
                    if (trans != null)
                    {
                        transaction = trans;
                        connection = trans.Connection;
                        closeConnection = false;
                        closeTransaction = false;
                    }
                    else
                    {
                        closeTransaction = false;
                        connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                        connection.Open();
                    }

                    //create command
                    da.SelectCommand = new MySqlCommand();
                    da.SelectCommand.Connection = connection;
                    da.SelectCommand.Transaction = transaction;
                    da.SelectCommand.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;

                    //create select clause
                    string select = "SELECT a.* FROM answer a ";

                    //create where clause
                    string where = string.Empty;

                    //check filters
                    //check answer rapporteur filter
                    if (filterAnswerRapporteur > -1)
                    {
                        //add answer type filter and parameter
                        where += " a.answerRapporteur=@answerRapporteur AND";
                        da.SelectCommand.Parameters.AddWithValue("@answerRapporteur", filterAnswerRapporteur);
                    }

                    //check answer target filter
                    if (filterAnswerTarget > -1)
                    {
                        //add answer type filter and parameter
                        where += " a.answerTarget=@answerTarget AND";
                        da.SelectCommand.Parameters.AddWithValue("@answerTarget", filterAnswerTarget);
                    }

                    //check answer periodicity filter
                    if (filterAnswerPeriodicity > -1)
                    {
                        //add answer type filter and parameter
                        where += " a.answerPeriodicity=@answerPeriodicity AND";
                        da.SelectCommand.Parameters.AddWithValue("@answerPeriodicity", filterAnswerPeriodicity);
                    }

                    //check answer subject filter
                    if (filterAnswerMetric > -1)
                    {
                        //add answer type filter and parameter
                        where += " a.answerMetric=@answerMetric AND";
                        da.SelectCommand.Parameters.AddWithValue("@answerMetric", filterAnswerMetric);
                    }

                    //check semester filter
                    if (filterSemester > -1)
                    {
                        //add semester filter and parameter
                        where += " a.semesterId=@semesterId AND";
                        da.SelectCommand.Parameters.AddWithValue("@semesterId", filterSemester);
                    }

                    //check reference date filter
                    if (filterReferenceDate != DateTime.MinValue)
                    {
                        //add reference date filter and parameter
                        where += " a.referenceDate=@referenceDate AND";
                        da.SelectCommand.Parameters.AddWithValue("@referenceDate", filterReferenceDate);
                    }

                    //check report filter
                    if (filterReport > -1)
                    {
                        //add report filter and parameter
                        where += " a.reportId=@reportId AND";
                        da.SelectCommand.Parameters.AddWithValue("@reportId", filterReport);
                    }

                    //check institution filter
                    if (filterInstitution > -1)
                    {
                        //add institution filter and parameter
                        where += " a.institutionId=@institutionId AND";
                        da.SelectCommand.Parameters.AddWithValue("@institutionId", filterInstitution);
                    }

                    //check teacher filter
                    if (filterTeacher > -1)
                    {
                        //add teacher filter and parameter
                        where += " a.teacherId=@teacherId AND";
                        da.SelectCommand.Parameters.AddWithValue("@teacherId", filterTeacher);
                    }

                    //check coordinator filter
                    if (filterCoordinator > -1)
                    {
                        //add coordinator filter and parameter
                        where += " a.coordinatorId=@coordinatorId AND";
                        da.SelectCommand.Parameters.AddWithValue("@coordinatorId", filterCoordinator);
                    }

                    //check class filter
                    if (filterClass > -1)
                    {
                        //add class filter and parameter
                        where += " a.classId=@classId AND";
                        da.SelectCommand.Parameters.AddWithValue("@classId", filterClass);
                    }

                    //check where clause
                    if (where.Length > 0)
                    {
                        //add WHERE keyword
                        where = " WHERE " + where;

                        //remove last AND keyword
                        where = where.Substring(0, where.Length - 3);
                    }

                    //set query
                    string orderBy = " ORDER BY a.answerId ";
                    string query = select + where + orderBy;
                    da.SelectCommand.CommandText = query;

                    n = da.Fill(dt);
                }

                if (closeTransaction && transaction != null)
                    transaction.Commit();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                if (n > 0)
                    return dt.Select();
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (closeTransaction && transaction != null)
                    transaction.Rollback();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                throw (ex);
            }
        }

        #endregion Methods

    } //end of class AnswerAccess

} //end of namespace PnT.SongDB.Access
