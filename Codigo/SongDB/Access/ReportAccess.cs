using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;


namespace PnT.SongDB.Access
{

    /// <summary>
    /// Summary description for ReportAccess.
    /// </summary>
    public class ReportAccess
    {

        #region Queries ****************************************************************

        private const string FIND_REPORT = @"SELECT * FROM report {0} {1} {2}";

        private const string SAVE_REPORT = @"INSERT INTO report (semesterId, classId, institutionId, teacherId, coordinatorId, reportRapporteur, reportTarget, reportPeriodicity, referenceDate, referenceList, reportStatus) 
            VALUES (@semesterId, @classId, @institutionId, @teacherId, @coordinatorId, @reportRapporteur, @reportTarget, @reportPeriodicity, @referenceDate, @referenceList, @reportStatus); 
            SELECT @@IDENTITY;";

        private const string UPDATE_REPORT = @"UPDATE report 
            SET semesterId=@semesterId, classId=@classId, institutionId=@institutionId, teacherId=@teacherId, coordinatorId=@coordinatorId, reportRapporteur=@reportRapporteur, reportTarget=@reportTarget, reportPeriodicity=@reportPeriodicity, referenceDate=@referenceDate, referenceList=@referenceList, reportStatus=@reportStatus
             WHERE reportId=@reportId";

        private const string DELETE_REPORT = @"DELETE FROM report WHERE reportId=@reportId";

        private const string INACTIVATE_REPORT = @"UPDATE report SET reportStatus=1 WHERE reportId=@reportId";

        #endregion Queries


        #region Methods ****************************************************************

        /// <summary>
        /// Save Report to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Report.</returns>
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
                    comm.CommandText = SAVE_REPORT;
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
                    comm.CommandText = UPDATE_REPORT;
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
        /// Delete Report by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Report.</param>
        /// <returns>
        /// True if selected Report was deleted.
        /// False if selected Report was not found.
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
                comm.CommandText = DELETE_REPORT;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@reportId", id);

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
        /// Inactivate Report by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Report.</param>
        /// <returns>
        /// True if selected Report was inactivated.
        /// False if selected Report was not found.
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
                comm.CommandText = INACTIVATE_REPORT;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@reportId", id);

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
        /// Find all Report.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no Report was found.
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

                    string orderBy = " ORDER BY reportId";
                    string query = string.Format(FIND_REPORT, string.Empty, orderBy, string.Empty);

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
        /// Find Report by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected database row.
        /// Null if no Report was found.
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

                    string where = " WHERE reportId = @id ";
                    string query = string.Format(FIND_REPORT, where, string.Empty, string.Empty);

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
        /// Find reports by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
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
        /// List of database rows.
        /// Null if no Report was found.
        /// </returns>
        public static DataRow[] FindByFilter(
            MySqlTransaction trans, int filterReportStatus, int filterReportRapporteur,
            int filterReportPeriodicity, int filterSemester, DateTime filterReferenceDate, 
            int filterInstitution, int filterTeacher, int filterClass)
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
                    string select = "SELECT r.* FROM report r ";

                    //create where clause
                    string where = string.Empty;

                    //check filters
                    //check report status filter
                    if (filterReportStatus > -1)
                    {
                        //add report status filter and parameter
                        where += " r.reportStatus=@reportStatus AND";
                        da.SelectCommand.Parameters.AddWithValue("@reportStatus", filterReportStatus);
                    }

                    //check report rapporteur filter
                    if (filterReportRapporteur > -1)
                    {
                        //add report type filter and parameter
                        where += " r.reportRapporteur=@reportRapporteur AND";
                        da.SelectCommand.Parameters.AddWithValue("@reportRapporteur", filterReportRapporteur);
                    }

                    //check report periodicity filter
                    if (filterReportPeriodicity > -1)
                    {
                        //add report type filter and parameter
                        where += " r.reportPeriodicity=@reportPeriodicity AND";
                        da.SelectCommand.Parameters.AddWithValue("@reportPeriodicity", filterReportPeriodicity);
                    }

                    //check semester filter
                    if (filterSemester > -1)
                    {
                        //add semester filter and parameter
                        where += " r.semesterId=@semesterId AND";
                        da.SelectCommand.Parameters.AddWithValue("@semesterId", filterSemester);
                    }

                    //check reference date filter
                    if (filterReferenceDate != DateTime.MinValue)
                    {
                        //add reference date filter and parameter
                        where += " r.referenceDate=@referenceDate AND";
                        da.SelectCommand.Parameters.AddWithValue("@referenceDate", filterReferenceDate);
                    }

                    //check institution filter
                    if (filterInstitution > -1)
                    {
                        //add institution filter and parameter
                        where += " r.institutionId=@institutionId AND";
                        da.SelectCommand.Parameters.AddWithValue("@institutionId", filterInstitution);
                    }

                    //check teacher filter
                    if (filterTeacher > -1)
                    {
                        //add teacher filter and parameter
                        where += " r.teacherId=@teacherId AND";
                        da.SelectCommand.Parameters.AddWithValue("@teacherId", filterTeacher);
                    }

                    //check class filter
                    if (filterClass > -1)
                    {
                        //add class filter and parameter
                        where += " r.classId=@classId AND";
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
                    string orderBy = " ORDER BY r.reportId ";
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

    } //end of class ReportAccess

} //end of namespace PnT.SongDB.Access
