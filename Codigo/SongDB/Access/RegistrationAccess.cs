using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;


namespace PnT.SongDB.Access
{

    /// <summary>
    /// Summary description for RegistrationAccess.
    /// </summary>
    public class RegistrationAccess
    {

        #region Queries ****************************************************************

        private const string FIND_REGISTRATION = @"SELECT * FROM registration {0} {1} {2}";

        private const string SAVE_REGISTRATION = @"INSERT INTO registration (studentId, classId, position, autoRenewal, registrationStatus, creationTime, inactivationTime, inactivationReason) 
            VALUES (@studentId, @classId, @position, @autoRenewal, @registrationStatus, @creationTime, @inactivationTime, @inactivationReason); 
            SELECT @@IDENTITY;";

        private const string UPDATE_REGISTRATION = @"UPDATE registration 
            SET position=@position, autoRenewal=@autoRenewal, registrationStatus=@registrationStatus, inactivationTime=@inactivationTime, inactivationReason=@inactivationReason
             WHERE registrationId=@registrationId";

        private const string DELETE_REGISTRATION = @"DELETE FROM registration WHERE registrationId=@registrationId";

        private const string INACTIVATE_REGISTRATION = @"UPDATE registration SET registrationStatus=1 WHERE registrationId=@registrationId";

        #endregion Queries


        #region Methods ****************************************************************

        /// <summary>
        /// Save Registration to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Registration.</returns>
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
                if ((int)parameters[0].Value <= -1)
                {
                    //insert new row
                    MySqlCommand comm = new MySqlCommand();
                    comm.CommandText = SAVE_REGISTRATION;
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
                    comm.CommandText = UPDATE_REGISTRATION;
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
        /// Delete Registration by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Registration.</param>
        /// <returns>
        /// True if selected Registration was deleted.
        /// False if selected Registration was not found.
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
                comm.CommandText = DELETE_REGISTRATION;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@registrationId", id);

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
        /// Inactivate Registration by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Registration.</param>
        /// <returns>
        /// True if selected Registration was inactivated.
        /// False if selected Registration was not found.
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
                comm.CommandText = INACTIVATE_REGISTRATION;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@registrationId", id);

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
        /// Find all Registration.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no Registration was found.
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

                using(MySqlDataAdapter da = new MySqlDataAdapter())
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

                    string orderBy = " ORDER BY registrationId";
                    string query = string.Format(FIND_REGISTRATION, string.Empty, orderBy, string.Empty);

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

                if (closeConnection  && connection.State == ConnectionState.Open)
                    connection.Close();

                throw (ex);
            }
        }

        /// <summary>
        /// Count registrations for selected class.
        /// </summary>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <param name="status">
        /// The status of the selected registrations.
        /// -1 to count all registrations.
        /// </param>
        /// <param name="maxPosition">
        /// The maximum registration position to consider.
        /// -1 to count all registrations.
        /// </param>
        /// <returns>
        /// The number of registrations.
        /// </returns>
        public static int CountByClass(
            MySqlTransaction trans, int classId, int status, int maxPosition)
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

                    string where = " WHERE classId = @classId ";
                    string and = status != -1 ? " AND registrationStatus = @registrationStatus " : string.Empty;
                    string and2 = maxPosition != -1 ? " AND position <= @maxPosition " : string.Empty;
                    string query = string.Format(
                        FIND_REGISTRATION.Replace("*", "COUNT(*)"), where, and, and2);
                    
                    da.Connection = connection;
                    da.Transaction = transaction;
                    da.CommandText = query;
                    da.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                    da.Parameters.AddWithValue("@classId", classId);
                    if (status != -1)
                    {
                        da.Parameters.AddWithValue("@registrationStatus", status);
                    }
                    if (maxPosition != -1)
                    {
                        da.Parameters.AddWithValue("@maxPosition", maxPosition);
                    }

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
        /// Find all Registration for selected class.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="classId">The id of the selected class.</param
        /// <param name="status">
        /// The status of the returned registrations.
        /// -1 to return all registrations.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no Registration was found.
        /// </returns>
        public static DataRow[] FindByClass(MySqlTransaction trans, int classId, int status)
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

                    string where = " WHERE classId = @classId ";
                    string and = status != -1 ? " AND registrationStatus = @registrationStatus " : string.Empty;
                    string orderBy = " ORDER BY position";
                    string query = string.Format(FIND_REGISTRATION, where, and, orderBy);

                    da.SelectCommand = new MySqlCommand();
                    da.SelectCommand.Connection = connection;
                    da.SelectCommand.Transaction = transaction;
                    da.SelectCommand.CommandText = query;
                    da.SelectCommand.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                    da.SelectCommand.Parameters.AddWithValue("@classId", classId);
                    if (status != -1)
                    {
                        da.SelectCommand.Parameters.AddWithValue("@registrationStatus", status);
                    }

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
        /// Find all Registration for selected student.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="studentId">The id of the selected student.</param
        /// <param name="status">
        /// The status of the returned registrations.
        /// -1 to return all registrations.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no Registration was found.
        /// </returns>
        public static DataRow[] FindByStudent(MySqlTransaction trans, int studentId, int status)
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

                    string where = " WHERE studentId = @studentId ";
                    string and = status != -1 ? " AND registrationStatus = @registrationStatus " : string.Empty;
                    string orderBy = " ORDER BY position";
                    string query = string.Format(FIND_REGISTRATION, where, and, orderBy);

                    da.SelectCommand = new MySqlCommand();
                    da.SelectCommand.Connection = connection;
                    da.SelectCommand.Transaction = transaction;
                    da.SelectCommand.CommandText = query;
                    da.SelectCommand.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                    da.SelectCommand.Parameters.AddWithValue("@studentId", studentId);
                    if (status != -1)
                    {
                        da.SelectCommand.Parameters.AddWithValue("@registrationStatus", status);
                    }

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
        /// Find Registration by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected database row.
        /// Null if no Registration was found.
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

                using(MySqlDataAdapter da = new MySqlDataAdapter())
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

                    string where = " WHERE registrationId = @id ";
                    string query = string.Format(FIND_REGISTRATION, where, string.Empty, string.Empty);

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
        /// Count registrations by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
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
        /// <returns>
        /// The number of registrations.
        /// </returns>
        public static int CountEvationsByFilter(
            MySqlTransaction trans, int filterSemester,
            DateTime filterReferenceDate, int filterInstitution, int filterTeacher)
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
                    string select = "SELECT COUNT(*) FROM registration r ";

                    //create where clause
                    string where = string.Empty;

                    //add registration status filter set to evaded
                    where += " r.registrationStatus=" + ((int)Logic.ItemStatus.Evaded) + " AND";

                    //check reference date filter
                    if (filterReferenceDate != DateTime.MinValue)
                    {
                        //add reference date filter and parameters
                        where += " r.inactivationTime>=@minInactivationTime AND" +
                            " r.inactivationTime<@maxInactivationTime AND";
                        da.Parameters.AddWithValue("@minInactivationTime", filterReferenceDate);
                        da.Parameters.AddWithValue("@maxInactivationTime", filterReferenceDate.AddMonths(1));
                    }

                    //check filters
                    //check multi table filters
                    if (filterInstitution > -1 || filterTeacher > -1 ||
                        filterSemester > -1 || filterReferenceDate != DateTime.MinValue)
                    {
                        //extend select
                        select += " INNER JOIN class c on c.classId = r.classId ";

                        //check semester filter
                        if (filterSemester > -1)
                        {
                            //add semester filter and parameter
                            where += " c.semesterId=@semesterId AND";
                            da.Parameters.AddWithValue("@semesterId", filterSemester);
                        }

                        //check teacher filter
                        if (filterTeacher > -1)
                        {
                            //add teacher filter and parameter
                            where += " c.teacherId=@teacherId AND";
                            da.Parameters.AddWithValue("@teacherId", filterTeacher);
                        }

                        //check institution filter
                        if (filterInstitution > -1)
                        {
                            //extend select
                            select += " INNER JOIN pole p on p.poleId = c.poleId ";

                            //add institution filter and parameter
                            where += " p.institutionId=@institutionId AND";
                            da.Parameters.AddWithValue("@institutionId", filterInstitution);
                        }
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
                    string orderBy = " ORDER BY r.registrationId ";
                    string query = select + where + orderBy;
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
        /// Count registrations by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterRegistrationStatus">
        /// The registration status filter.
        /// -1 to select all statuses.
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
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// The number of registrations.
        /// </returns>
        public static int CountByFilter(
            MySqlTransaction trans, int filterRegistrationStatus, int filterSemester,
            int filterInstitution, int filterPole, int filterTeacher, int filterClass)
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
                    string select = "SELECT COUNT(*) FROM registration r ";

                    //create where clause
                    string where = string.Empty;

                    //check filters
                    //check multi table filters
                    if (filterClass > -1 || filterSemester > -1 ||
                        filterTeacher > -1 || filterPole > -1 || filterInstitution > -1)
                    {
                        //extend select
                        select += " INNER JOIN class c on c.classId = r.classId ";

                        //check class filter
                        if (filterClass > -1)
                        {
                            //add class filter and parameter
                            where += " c.classId=@classId AND";
                            da.Parameters.AddWithValue("@classId", filterClass);
                        }
                        else
                        {
                            //check semester filter filter
                            if (filterSemester > -1)
                            {
                                //add semester filter and parameter
                                where += " c.semesterId=@semesterId AND";
                                da.Parameters.AddWithValue("@semesterId", filterSemester);
                            }

                            //check teacher filter
                            if (filterTeacher > -1)
                            {
                                //add teacher filter and parameter
                                where += " c.teacherId=@teacherId AND";
                                da.Parameters.AddWithValue("@teacherId", filterTeacher);
                            }

                            //check pole filter
                            if (filterPole > -1)
                            {
                                //add pole filter and parameter
                                where += " c.poleId=@poleId AND";
                                da.Parameters.AddWithValue("@poleId", filterPole);
                            }
                            //check institution filter
                            else if (filterInstitution > -1)
                            {
                                //extend select
                                select += " INNER JOIN pole p on p.poleId = c.poleId ";

                                //add institution filter and parameter
                                where += " p.institutionId=@institutionId AND";
                                da.Parameters.AddWithValue("@institutionId", filterInstitution);
                            }
                        }
                    }

                    //check registration status filter
                    if (filterRegistrationStatus > -1)
                    {
                        //add registration status filter and parameter
                        where += " r.registrationStatus=@registrationStatus AND";
                        da.Parameters.AddWithValue("@registrationStatus", filterRegistrationStatus);
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
                    string orderBy = " ORDER BY r.registrationId ";
                    string query = select + where + orderBy;
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
        /// Find registrations by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterRegistrationStatus">
        /// The registration status filter.
        /// -1 to select all statuses.
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
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no Registration was found.
        /// </returns>
        public static DataRow[] FindByFilter(
            MySqlTransaction trans, int filterRegistrationStatus, int filterSemester,
            int filterInstitution, int filterPole, int filterTeacher, int filterClass)
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
                    string select = "SELECT r.* FROM registration r ";

                    //create where clause
                    string where = string.Empty;

                    //check filters
                    //check multi table filters
                    if (filterClass > -1 || filterSemester > -1 ||
                        filterTeacher > -1 || filterPole > -1 || filterInstitution > -1)
                    {
                        //extend select
                        select += " INNER JOIN class c on c.classId = r.classId ";

                        //check class filter
                        if (filterClass > -1)
                        {
                            //add class filter and parameter
                            where += " c.classId=@classId AND";
                            da.SelectCommand.Parameters.AddWithValue("@classId", filterClass);
                        }
                        else
                        {
                            //check semester filter
                            if (filterSemester > -1)
                            {
                                //add semester filter and parameter
                                where += " c.semesterId=@semesterId AND";
                                da.SelectCommand.Parameters.AddWithValue("@semesterId", filterSemester);
                            }

                            //check teacher filter
                            if (filterTeacher > -1)
                            {
                                //add teacher filter and parameter
                                where += " c.teacherId=@teacherId AND";
                                da.SelectCommand.Parameters.AddWithValue("@teacherId", filterTeacher);
                            }

                            //check pole filter
                            if (filterPole > -1)
                            {
                                //add pole filter and parameter
                                where += " c.poleId=@poleId AND";
                                da.SelectCommand.Parameters.AddWithValue("@poleId", filterPole);
                            }
                            //check institution filter
                            else if (filterInstitution > -1)
                            {
                                //extend select
                                select += " INNER JOIN pole p on p.poleId = c.poleId ";

                                //add institution filter and parameter
                                where += " p.institutionId=@institutionId AND";
                                da.SelectCommand.Parameters.AddWithValue("@institutionId", filterInstitution);
                            }
                        }
                    }

                    //check registration status filter
                    if (filterRegistrationStatus > -1)
                    {
                        //add registration status filter and parameter
                        where += " r.registrationStatus=@registrationStatus AND";
                        da.SelectCommand.Parameters.AddWithValue("@registrationStatus", filterRegistrationStatus);
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
                    string orderBy = " ORDER BY r.registrationId ";
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

    } //end of class RegistrationAccess

} //end of namespace PnT.SongDB.Access
