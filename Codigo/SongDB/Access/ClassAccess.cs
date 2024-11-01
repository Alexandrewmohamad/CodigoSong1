using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;


namespace PnT.SongDB.Access
{

    /// <summary>
    /// Summary description for ClassAccess.
    /// </summary>
    public class ClassAccess
    {

        #region Queries ****************************************************************

        private const string FIND_CLASS = @"SELECT * FROM class {0} {1} {2}";

        private const string SAVE_CLASS = @"INSERT INTO class (semesterId, poleId, teacherId, subjectCode, code, classType, instrumentType, classLevel, capacity, weekMonday, weekTuesday, weekWednesday, weekThursday, weekFriday, weekSaturday, weekSunday, startTime, duration, classStatus, creationTime, inactivationTime, inactivationReason) 
            VALUES (@semesterId, @poleId, @teacherId, @subjectCode, @code, @classType, @instrumentType, @classLevel, @capacity, @weekMonday, @weekTuesday, @weekWednesday, @weekThursday, @weekFriday, @weekSaturday, @weekSunday, @startTime, @duration, @classStatus, @creationTime, @inactivationTime, @inactivationReason); 
            SELECT @@IDENTITY;";

        private const string UPDATE_CLASS = @"UPDATE class 
            SET semesterId=@semesterId, poleId=@poleId, teacherId=@teacherId, subjectCode=@subjectCode, code=@code, classType=@classType, instrumentType=@instrumentType, classLevel=@classLevel, capacity=@capacity, weekMonday=@weekMonday, weekTuesday=@weekTuesday, weekWednesday=@weekWednesday, weekThursday=@weekThursday, weekFriday=@weekFriday, weekSaturday=@weekSaturday, weekSunday=@weekSunday, startTime=@startTime, duration=@duration, classStatus=@classStatus, inactivationTime=@inactivationTime, inactivationReason=@inactivationReason
             WHERE classId=@classId";

        private const string DELETE_CLASS = @"DELETE FROM class WHERE classId=@classId";

        private const string INACTIVATE_CLASS = @"UPDATE class SET classStatus=1, inactivationTime=@inactivationTime, inactivationReason=@inactivationReason WHERE classId=@classId";

        #endregion Queries


        #region Methods ****************************************************************

        /// <summary>
        /// Save Class to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Class.</returns>
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
                    comm.CommandText = SAVE_CLASS;
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
                    comm.CommandText = UPDATE_CLASS;
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
        /// Delete Class by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Class.</param>
        /// <returns>
        /// True if selected Class was deleted.
        /// False if selected Class was not found.
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
                comm.CommandText = DELETE_CLASS;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@classId", id);

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
        /// Inactivate Class by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Class.</param>
        /// <param name="inactivationReason">
        /// The reason why the class is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Class was inactivated.
        /// False if selected Class was not found.
        /// </returns>
        public static bool Inactivate(
            MySqlTransaction trans, int id, string inactivationReason)
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
                comm.CommandText = INACTIVATE_CLASS;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@classId", id);
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
        /// Find all Class.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no Class was found.
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

                    string orderBy = " ORDER BY classId";
                    string query = string.Format(FIND_CLASS, string.Empty, orderBy, string.Empty);

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
        /// Find Class by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected database row.
        /// Null if no Class was found.
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

                    string where = " WHERE classId = @id ";
                    string query = string.Format(FIND_CLASS, where, string.Empty, string.Empty);

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
        /// Find classs by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterClassStatus">
        /// The class status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterClassType">
        /// The class type filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type.
        /// -1 to select all instrument types.
        /// </param>
        /// <param name="filterClassLevel">
        /// The class level filter.
        /// -1 to select all levels.
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
        /// <returns>
        /// The number of classes.
        /// </returns>
        public static int CountByFilter(
            MySqlTransaction trans, int filterClassStatus, int filterClassType,
            int filterInstrumentType, int filterClassLevel, int filterSemester,
            int filterInstitution, int filterPole, int filterTeacher)
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
                    string select = "SELECT COUNT(*) FROM class c ";

                    //create where clause
                    string where = string.Empty;

                    //check filters
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

                    //check class status filter
                    if (filterClassStatus > -1)
                    {
                        //add class status filter and parameter
                        where += " c.classStatus=@classStatus AND";
                        da.Parameters.AddWithValue("@classStatus", filterClassStatus);
                    }

                    //check class type filter
                    if (filterClassType > -1)
                    {
                        //add class type filter and parameter
                        where += " c.classType=@classType AND";
                        da.Parameters.AddWithValue("@classType", filterClassType);
                    }

                    //check class level filter
                    if (filterClassLevel > -1)
                    {
                        //add class level filter and parameter
                        where += " c.classLevel=@classLevel AND";
                        da.Parameters.AddWithValue("@classLevel", filterClassLevel);
                    }

                    //check instrument type filter
                    if (filterInstrumentType > -1)
                    {
                        //add instrument type filter and parameter
                        where += " c.instrumentType=@instrumentType AND";
                        da.Parameters.AddWithValue("@instrumentType", filterInstrumentType);
                    }

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
        /// Find next available subject code.
        /// </summary>
        /// <returns>
        /// The next available subject code.
        /// </returns>
        public static int FindNextAvailableSubjectCode(MySqlTransaction trans)
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
                    string select = "SELECT MAX(c.subjectCode) FROM class c ";
                    
                    //set query
                    da.CommandText = select;

                    n = (object)da.ExecuteScalar();
                }

                if (closeTransaction && transaction != null)
                    transaction.Commit();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                //check result
                if (n != System.DBNull.Value)
                {
                    //increment result for next available class
                    return (Convert.ToInt32(n) + 1);
                }
                else
                {
                    //no class is registered
                    //first available code is one
                    return 1;
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
        /// Find classs by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterClassStatus">
        /// The class status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterClassType">
        /// The class type filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type.
        /// -1 to select all instrument types.
        /// </param>
        /// <param name="filterClassLevel">
        /// The class level filter.
        /// -1 to select all levels.
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
        /// <returns>
        /// List of database rows.
        /// Null if no Class was found.
        /// </returns>
        public static DataRow[] FindByFilter(
            MySqlTransaction trans, int filterClassStatus, int filterClassType,
            int filterInstrumentType, int filterClassLevel, int filterSemester, 
            int filterInstitution, int filterPole, int filterTeacher)
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
                    string select = "SELECT c.* FROM class c ";

                    //create where clause
                    string where = string.Empty;

                    //check filters
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

                    //check class status filter
                    if (filterClassStatus > -1)
                    {
                        //add class status filter and parameter
                        where += " c.classStatus=@classStatus AND";
                        da.SelectCommand.Parameters.AddWithValue("@classStatus", filterClassStatus);
                    }

                    //check class type filter
                    if (filterClassType > -1)
                    {
                        //add class type filter and parameter
                        where += " c.classType=@classType AND";
                        da.SelectCommand.Parameters.AddWithValue("@classType", filterClassType);
                    }

                    //check class level filter
                    if (filterClassLevel > -1)
                    {
                        //add class level filter and parameter
                        where += " c.classLevel=@classLevel AND";
                        da.SelectCommand.Parameters.AddWithValue("@classLevel", filterClassLevel);
                    }

                    //check instrument type filter
                    if (filterInstrumentType > -1)
                    {
                        //add instrument type filter and parameter
                        where += " c.instrumentType=@instrumentType AND";
                        da.SelectCommand.Parameters.AddWithValue("@instrumentType", filterInstrumentType);
                    }

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

                    //check where clause
                    if (where.Length > 0)
                    {
                        //add WHERE keyword
                        where = " WHERE " + where;

                        //remove last AND keyword
                        where = where.Substring(0, where.Length - 3);
                    }

                    //set query
                    string orderBy = " ORDER BY c.classId ";
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

        /// <summary>
        /// Find all classes that selected student is registered to.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="studentId">
        /// The ID of the selected student.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no Class was found.
        /// </returns>
        public static DataRow[] FindByStudent(MySqlTransaction trans, int studentId)
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
                    string select = "SELECT c.* FROM class c ";

                    //extend select
                    select += " INNER JOIN registration r on r.classId = c.classId ";

                    //create where clause
                    string where = " WHERE r.studentId=@studentId ";
                    da.SelectCommand.Parameters.AddWithValue("@studentId", studentId);

                    //set query
                    string orderBy = " ORDER BY c.classId ";
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

    } //end of class ClassAccess

} //end of namespace PnT.SongDB.Access
