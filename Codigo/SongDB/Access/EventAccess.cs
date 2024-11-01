using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;


namespace PnT.SongDB.Access
{

    /// <summary>
    /// Summary description for EventAccess.
    /// </summary>
    public class EventAccess
    {

        #region Queries ****************************************************************

        private const string FIND_EVENT = @"SELECT * FROM event {0} {1} {2}";

        private const string SAVE_EVENT = @"INSERT INTO event (institutionId, startTime, duration, location, eventSendOption, name, description, creationTime) 
            VALUES (@institutionId, @startTime, @duration, @location, @eventSendOption, @name, @description, @creationTime); 
            SELECT @@IDENTITY;";

        private const string UPDATE_EVENT = @"UPDATE event 
            SET institutionId=@institutionId, startTime=@startTime, duration=@duration, location=@location, eventSendOption=@eventSendOption, name=@name, description=@description
             WHERE eventId=@eventId";

        private const string DELETE_EVENT = @"DELETE FROM event WHERE eventId=@eventId";

        private const string INACTIVATE_EVENT = @"UPDATE event SET eventStatus=1 WHERE eventId=@eventId";

        #endregion Queries


        #region Methods ****************************************************************

        /// <summary>
        /// Save Event to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Event.</returns>
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
                    comm.CommandText = SAVE_EVENT;
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
                    comm.CommandText = UPDATE_EVENT;
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
        /// Delete Event by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Event.</param>
        /// <returns>
        /// True if selected Event was deleted.
        /// False if selected Event was not found.
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
                comm.CommandText = DELETE_EVENT;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@eventId", id);

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
        /// Inactivate Event by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Event.</param>
        /// <returns>
        /// True if selected Event was inactivated.
        /// False if selected Event was not found.
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
                comm.CommandText = INACTIVATE_EVENT;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@eventId", id);

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
        /// Find all Event.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no Event was found.
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

                    string orderBy = " ORDER BY eventId";
                    string query = string.Format(FIND_EVENT, string.Empty, orderBy, string.Empty);

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
        /// Find Event by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected database row.
        /// Null if no Event was found.
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

                    string where = " WHERE eventId = @id ";
                    string query = string.Format(FIND_EVENT, where, string.Empty, string.Empty);

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
        /// Count events by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterStartDate">
        /// The start date filter. The start time must be later than selected date.
        /// DateTime.MinValue to set no start date.
        /// </param>
        /// <param name="filterEndDate">
        /// The end date filter. The start time must be sooner than selected date.
        /// DateTime.MinValue to set no end date.
        /// </param>
        /// <returns>
        /// The number of events.
        /// </returns>
        public static int CountByFilter(
            MySqlTransaction trans, int filterInstitution,
            DateTime filterStartDate, DateTime filterEndDate)
        {
            MySqlTransaction transaction = null;
            MySqlConnection connection = null;
            bool closeTransaction = true;
            bool closeConnection = true;
            object n;

            try
            {
                DataTable dt = new DataTable();

                using (MySqlCommand da = new MySqlCommand())
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
                    da.Connection = connection;
                    da.Transaction = transaction;
                    da.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;

                    //create select clause
                    string select = "SELECT COUNT(*) FROM event ";

                    //create where clause
                    string where = string.Empty;

                    //check filters
                    //check institution filter
                    if (filterInstitution > -1)
                    {
                        //add institution filter and parameter
                        where += " institutionId=@institutionId AND";
                        da.Parameters.AddWithValue("@institutionId", filterInstitution);
                    }

                    //check start date filter
                    if (filterStartDate != DateTime.MinValue)
                    {
                        //add start date filter and parameter
                        where += " startTime>=@startDate AND";
                        da.Parameters.AddWithValue("@startDate", filterStartDate);
                    }

                    //check end date filter
                    if (filterEndDate != DateTime.MinValue)
                    {
                        //add end date filter and parameter
                        where += " startTime<=@endDate AND";
                        da.Parameters.AddWithValue("@endDate", filterEndDate);
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
                    string query = select + where;
                    da.CommandText = query;

                    n = (object)da.ExecuteScalar();
                }

                if (closeTransaction && transaction != null)
                    transaction.Commit();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                //check result
                if (n != System.DBNull.Value)
                {
                    return Convert.ToInt32(n);
                }
                else
                {
                    return 0;
                }
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
        /// Find events by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterStartDate">
        /// The start date filter. The start time must be later than selected date.
        /// DateTime.MinValue to set no start date.
        /// </param>
        /// <param name="filterEndDate">
        /// The end date filter. The start time must be sooner than selected date.
        /// DateTime.MinValue to set no end date.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no Event was found.
        /// </returns>
        public static DataRow[] FindByFilter(
            MySqlTransaction trans, int filterInstitution,
            DateTime filterStartDate, DateTime filterEndDate)
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

                    //create where clause
                    string where = string.Empty;

                    //check filters
                    //check institution filter
                    if (filterInstitution > -1)
                    {
                        //add institution filter and parameter
                        where += " institutionId=@institutionId AND";
                        da.SelectCommand.Parameters.AddWithValue("@institutionId", filterInstitution);
                    }

                    //check start date filter
                    if (filterStartDate != DateTime.MinValue)
                    {
                        //add start date filter and parameter
                        where += " startTime>=@startDate AND";
                        da.SelectCommand.Parameters.AddWithValue("@startDate", filterStartDate);
                    }

                    //check end date filter
                    if (filterEndDate != DateTime.MinValue)
                    {
                        //add end date filter and parameter
                        where += " startTime<=@endDate AND";
                        da.SelectCommand.Parameters.AddWithValue("@endDate", filterEndDate);
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
                    string orderBy = " ORDER BY startTime";
                    string query = string.Format(FIND_EVENT, where, orderBy, string.Empty);
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

    } //end of class EventAccess

} //end of namespace PnT.SongDB.Access
