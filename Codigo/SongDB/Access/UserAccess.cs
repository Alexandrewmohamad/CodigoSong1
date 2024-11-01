using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;


namespace PnT.SongDB.Access
{

    /// <summary>
    /// Summary description for UserAccess.
    /// </summary>
    public class UserAccess
    {

        #region Queries ****************************************************************

        private const string FIND_USER = @"SELECT * FROM user {0} {1} {2}";

        private const string SAVE_USER = @"INSERT INTO user (roleId, institutionId, login, password, requestPasswordChange, name, email, userStatus, creationTime, inactivationTime, inactivationReason) 
            VALUES (@roleId, @institutionId, @login, @password, @requestPasswordChange,@name, @email, @userStatus, @creationTime, @inactivationTime, @inactivationReason); 
            SELECT @@IDENTITY;";

        private const string UPDATE_USER = @"UPDATE user 
            SET roleId=@roleId, institutionId=@institutionId, login=@login, password=@password, requestPasswordChange=@requestPasswordChange, name=@name, email=@email, userStatus=@userStatus, inactivationTime=@inactivationTime, inactivationReason=@inactivationReason
             WHERE userId=@userId";

        private const string DELETE_USER = @"DELETE FROM user WHERE userId=@userId";

        private const string INACTIVATE_USER = @"UPDATE user SET userStatus=1, inactivationTime=@inactivationTime, inactivationReason=@inactivationReason WHERE userId=@userId";

        #endregion Queries


        #region Methods ****************************************************************

        /// <summary>
        /// Save User to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved User.</returns>
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
                    comm.CommandText = SAVE_USER;
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
                    comm.CommandText = UPDATE_USER;
                    comm.Connection = connection;
                    comm.Transaction = transaction;
                    comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                    comm.Parameters.AddRange(parameters);

                    //check if password parameter was not included
                    if (new List<MySqlParameter>(parameters).Find(p => p.ParameterName.Equals("password")) == null)
                    {
                        //remove password from query
                        comm.CommandText = UPDATE_USER.Replace("password=@password, ", string.Empty);
                    }

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
        /// Delete User by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected User.</param>
        /// <returns>
        /// True if selected User was deleted.
        /// False if selected User was not found.
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
                comm.CommandText = DELETE_USER;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@userId", id);

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
        /// Inactivate User by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected User.</param>
        /// <param name="inactivationReason">
        /// The reason why the user is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected User was inactivated.
        /// False if selected User was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id, string inactivationReason)
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
                comm.CommandText = INACTIVATE_USER;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@userId", id);
                comm.Parameters.AddWithValue("@inactivationTime", DateTime.Now);
                comm.Parameters.AddWithValue("@inactivationReason", inactivationReason);

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
        /// Find all User.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no User was found.
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

                    string orderBy = " ORDER BY userId";
                    string query = string.Format(FIND_USER, string.Empty, orderBy, string.Empty);

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
        /// Find all coordinator User.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no User was found.
        /// </returns>
        public static DataRow[] FindCoordinator(MySqlTransaction trans)
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

                    string query = "SELECT u.* FROM user u " +
                        "INNER JOIN role r ON u.roleId = r.roleId " +
                        "INNER JOIN rolepermission rp ON r.roleId = rp.roleId " +
                        "INNER JOIN permission p ON rp.permissionId = p.permissionId " +
                        "WHERE p.name = 'Institution.Coordinator' " +
                        "ORDER BY u.userId ";

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
        /// Find all student User.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no User was found.
        /// </returns>
        public static DataRow[] FindStudent(MySqlTransaction trans)
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

                    string query = "SELECT u.* FROM user u " +
                        "INNER JOIN student s ON s.userId = u.userId " +
                        "ORDER BY u.userId";

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
        /// Find all teacher User.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no User was found.
        /// </returns>
        public static DataRow[] FindTeacher(MySqlTransaction trans)
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

                    string query = "SELECT u.* FROM user u " +
                        "INNER JOIN teacher t ON t.userId = u.userId " +
                        "ORDER BY u.userId";

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
        /// Find User by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected database row.
        /// Null if no User was found.
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

                    string where = " WHERE userId = @id ";
                    string query = string.Format(FIND_USER, where, string.Empty, string.Empty);

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
        /// Find User by login.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected database row.
        /// Null if no User was found.
        /// </returns>
        public static DataRow Find(MySqlTransaction trans, string login)
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

                    string where = " WHERE login = @login ";
                    string query = string.Format(FIND_USER, where, string.Empty, string.Empty);

                    da.SelectCommand = new MySqlCommand();
                    da.SelectCommand.Connection = connection;
                    da.SelectCommand.Transaction = transaction;
                    da.SelectCommand.CommandText = query;
                    da.SelectCommand.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                    da.SelectCommand.Parameters.AddWithValue("@login", login);

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
        /// Find User by login and password.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="login">
        /// The login of the selected user.
        /// </param>
        /// <param name="password">
        /// The password of the selected user.
        /// </param>
        /// <returns>
        /// The selected database row.
        /// Null if no User was found.
        /// </returns>
        public static DataRow Find(MySqlTransaction trans, string login, byte[] password)
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

                    string where = " WHERE login = @login ";
                    string and = " AND password = @password ";
                    string query = string.Format(FIND_USER, where, and, string.Empty);

                    da.SelectCommand = new MySqlCommand();
                    da.SelectCommand.Connection = connection;
                    da.SelectCommand.Transaction = transaction;
                    da.SelectCommand.CommandText = query;
                    da.SelectCommand.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                    da.SelectCommand.Parameters.AddWithValue("@login", login);
                    da.SelectCommand.Parameters.AddWithValue("@password", password);

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
        /// Find User by login and recovery password.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="login">
        /// The login of the selected user.
        /// </param>
        /// <param name="recoveryPassword">
        /// The recovery password of the selected user.
        /// </param>
        /// <returns>
        /// The selected database row.
        /// Null if no User was found.
        /// </returns>
        public static DataRow FindByRecoveryPassword(
            MySqlTransaction trans, string login, byte[] recoveryPassword)
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

                    string where = " WHERE login = @login ";
                    string and = " AND recoveryPassword = @recoveryPassword ";
                    string query = string.Format(FIND_USER, where, and, string.Empty);

                    da.SelectCommand = new MySqlCommand();
                    da.SelectCommand.Connection = connection;
                    da.SelectCommand.Transaction = transaction;
                    da.SelectCommand.CommandText = query;
                    da.SelectCommand.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                    da.SelectCommand.Parameters.AddWithValue("@login", login);
                    da.SelectCommand.Parameters.AddWithValue("@recoveryPassword", recoveryPassword);

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
        /// Find users by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterUserStatus">
        /// The user status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterRole">
        /// The role filter.
        /// -1 to select all roles.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no User was found.
        /// </returns>
        public static DataRow[] FindByFilter(
            MySqlTransaction trans, int filterUserStatus, int filterInstitution, int filterRole)
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
                    //check status filter
                    if (filterUserStatus > -1)
                    {
                        //add status filter and parameter
                        where += " userStatus=@userStatus AND";
                        da.SelectCommand.Parameters.AddWithValue("@userStatus", filterUserStatus);
                    }

                    //check institution filter
                    if (filterInstitution > -1)
                    {
                        //add institution filter and parameter
                        where += " institutionId=@institutionId AND";
                        da.SelectCommand.Parameters.AddWithValue("@institutionId", filterInstitution);
                    }

                    //check role filter
                    if (filterRole > -1)
                    {
                        //add role filter and parameter
                        where += " roleId=@roleId AND";
                        da.SelectCommand.Parameters.AddWithValue("@roleId", filterRole);
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
                    string orderBy = " ORDER BY userId";
                    string query = string.Format(FIND_USER, where, orderBy, string.Empty);
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

        /// <summary>
        /// Find all coordinator User that are assigned to an institution.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no User was found.
        /// </returns>
        public static DataRow[] FindAssignedCoordinators(MySqlTransaction trans)
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
                    
                    //write query
                    string query = "SELECT u.* FROM user u " +
                        "INNER JOIN institution i " +
                        "ON u.userId = i.coordinatorId " +
                        "GROUP BY u.userId";

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
        /// Set new password to user by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">
        /// The id of the selected User to whom the new password will be set.
        /// </param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>
        /// True if selected User had the password set.
        /// False if selected User was not found.
        /// </returns>
        public static bool SetPassword(MySqlTransaction trans, int id, byte[] newPassword)
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

                //update existing row
                MySqlCommand comm = new MySqlCommand();
                comm.CommandText = "UPDATE user SET password=@password, " +
                    "requestPasswordChange=0, recoveryPassword=null WHERE userId=@userId";
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@userId", id);
                comm.Parameters.AddWithValue("@password", newPassword);

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
        /// Set new recovery password to user by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">
        /// The id of the selected User to whom the new password will be set.
        /// </param>
        /// <param name="newPassword">The new recovery password.</param>
        /// <returns>
        /// True if selected User had the recovery password set.
        /// False if selected User was not found.
        /// </returns>
        public static bool SetRecoveryPassword(MySqlTransaction trans, int id, byte[] newPassword)
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

                //update existing row
                MySqlCommand comm = new MySqlCommand();
                comm.CommandText = "UPDATE user SET recoveryPassword=@recoveryPassword WHERE userId=@userId";
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@userId", id);
                comm.Parameters.AddWithValue("@recoveryPassword", newPassword);

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
        /// Set role to user by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">
        /// The id of the selected User to whom the role will be assigned.
        /// </param>
        /// <param name="roleId">The id of the assigned Role.</param>
        /// <returns>
        /// True if selected User had the role assigned.
        /// False if selected User was not found.
        /// </returns>
        public static bool SetRole(MySqlTransaction trans, int id, int roleId)
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

                //update existing row
                MySqlCommand comm = new MySqlCommand();
                comm.CommandText = "UPDATE user SET roleId=@roleId WHERE userId=@userId";
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@userId", id);
                comm.Parameters.AddWithValue("@roleId", roleId);

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

        #endregion Methods

    } //end of class UserAccess

} //end of namespace PnT.SongDB.Access
