using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;


namespace PnT.SongDB.Access
{

    /// <summary>
    /// Summary description for GradeAccess.
    /// </summary>
    public class GradeAccess
    {

        #region Queries ****************************************************************

        private const string FIND_GRADE = @"SELECT * FROM grade {0} {1} {2}";

        private const string SAVE_GRADE = @"INSERT INTO grade (semesterId, studentId, teacherId, coordinatorId, classId, institutionId, gradeRapporteur, gradeTarget, gradePeriodicity, referenceDate, gradeSubject, score) 
            VALUES (@semesterId, @studentId, @teacherId, @coordinatorId, @classId, @institutionId, @gradeRapporteur, @gradeTarget, @gradePeriodicity, @referenceDate, @gradeSubject, @score); 
            SELECT @@IDENTITY;";

        private const string UPDATE_GRADE = @"UPDATE grade 
            SET semesterId=@semesterId, studentId=@studentId, teacherId=@teacherId, coordinatorId=@coordinatorId, classId=@classId, institutionId=@institutionId, gradeRapporteur=@gradeRapporteur, gradeTarget=@gradeTarget, gradePeriodicity=@gradePeriodicity, referenceDate=@referenceDate, gradeSubject=@gradeSubject, score=@score
             WHERE gradeId=@gradeId";

        private const string DELETE_GRADE = @"DELETE FROM grade WHERE gradeId=@gradeId";

        private const string DELETE_GRADE_CLASS_STUDENT = @"DELETE FROM grade WHERE classId=@classId AND studentId=@studentId";

        private const string INACTIVATE_GRADE = @"UPDATE grade SET gradeStatus=1 WHERE gradeId=@gradeId";

        #endregion Queries


        #region Methods ****************************************************************

        /// <summary>
        /// Save Grade to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Grade.</returns>
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
                    comm.CommandText = SAVE_GRADE;
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
                    comm.CommandText = UPDATE_GRADE;
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
        /// Delete Grade by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Grade.</param>
        /// <returns>
        /// True if selected Grade was deleted.
        /// False if selected Grade was not found.
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
                comm.CommandText = DELETE_GRADE;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@gradeId", id);

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
        /// Delete all Grade for selected class student.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="classId">The id of the selected class.</param>
        /// <param name="studentId">The id of the selected student.</param>
        /// <returns>
        /// The number of deleted Grades.
        /// </returns>
        public static int DeleteForClassStudent(
            MySqlTransaction trans, int classId, int studentId)
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

                //delete existing rows
                MySqlCommand comm = new MySqlCommand();
                comm.CommandText = DELETE_GRADE_CLASS_STUDENT;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@classId", classId);
                comm.Parameters.AddWithValue("@studentId", studentId);

                n = comm.ExecuteNonQuery();

                if (closeTransaction && transaction != null)
                    transaction.Commit();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                return n;
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
        /// Inactivate Grade by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Grade.</param>
        /// <returns>
        /// True if selected Grade was inactivated.
        /// False if selected Grade was not found.
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
                comm.CommandText = INACTIVATE_GRADE;
                comm.Connection = connection;
                comm.Transaction = transaction;
                comm.CommandTimeout = PnT.SongDB.ConnectionSettings.SongDBTimeout;
                comm.Parameters.AddWithValue("@gradeId", id);

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
        /// Find all Grade.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no Grade was found.
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

                    string orderBy = " ORDER BY gradeId";
                    string query = string.Format(FIND_GRADE, string.Empty, orderBy, string.Empty);

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
        /// Find Grade by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected database row.
        /// Null if no Grade was found.
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

                    string where = " WHERE gradeId = @id ";
                    string query = string.Format(FIND_GRADE, where, string.Empty, string.Empty);

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
        /// Average attendance score by filter.
        /// </summary>
        /// <param name="filterGradeRapporteur">
        /// The grade rapporteur filter.
        /// -1 to select all rapporteurs.
        /// </param>
        /// <param name="filterGradeTarget">
        /// The grade target filter.
        /// -1 to select all targets.
        /// </param>
        /// <param name="filterGradePeriodicity">
        /// The grade periodicity filter.
        /// -1 to select all periodicities.
        /// </param>
        /// <param name="filterGradeSubject">
        /// The grade subject filter.
        /// -1 to select all subjects.
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
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// The average score of filtered grades.
        /// </returns>
        public static double AverageByFilter(
            MySqlTransaction trans, int filterGradeRapporteur, int filterGradeTarget,
            int filterGradePeriodicity, int filterGradeSubject, int filterSemester,
            DateTime filterReferenceDate, int filterInstitution, int filterTeacher,
            int filterCoordinator, int filterStudent, int filterClass)
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

                    //create average clause
                    string select = "SELECT AVG(r.score) FROM grade r ";

                    //create where clause
                    //remove special values
                    string where = " r.score >= 0 AND";

                    //check filters
                    //check grade rapporteur filter
                    if (filterGradeRapporteur > -1)
                    {
                        //add grade type filter and parameter
                        where += " r.gradeRapporteur=@gradeRapporteur AND";
                        da.Parameters.AddWithValue("@gradeRapporteur", filterGradeRapporteur);
                    }

                    //check grade target filter
                    if (filterGradeTarget > -1)
                    {
                        //add grade type filter and parameter
                        where += " r.gradeTarget=@gradeTarget AND";
                        da.Parameters.AddWithValue("@gradeTarget", filterGradeTarget);
                    }

                    //check grade periodicity filter
                    if (filterGradePeriodicity > -1)
                    {
                        //add grade type filter and parameter
                        where += " r.gradePeriodicity=@gradePeriodicity AND";
                        da.Parameters.AddWithValue("@gradePeriodicity", filterGradePeriodicity);
                    }

                    //check grade subject filter
                    if (filterGradeSubject > -1)
                    {
                        //add grade type filter and parameter
                        where += " r.gradeSubject=@gradeSubject AND";
                        da.Parameters.AddWithValue("@gradeSubject", filterGradeSubject);
                    }

                    //check semester filter
                    if (filterSemester > -1)
                    {
                        //add semester filter and parameter
                        where += " r.semesterId=@semesterId AND";
                        da.Parameters.AddWithValue("@semesterId", filterSemester);
                    }

                    //check reference date filter
                    if (filterReferenceDate != DateTime.MinValue)
                    {
                        //add reference date filter and parameter
                        where += " r.referenceDate=@referenceDate AND";
                        da.Parameters.AddWithValue("@referenceDate", filterReferenceDate);
                    }

                    //check institution filter
                    if (filterInstitution > -1)
                    {
                        //add institution filter and parameter
                        where += " r.institutionId=@institutionId AND";
                        da.Parameters.AddWithValue("@institutionId", filterInstitution);
                    }

                    //check teacher filter
                    if (filterTeacher > -1)
                    {
                        //add teacher filter and parameter
                        where += " r.teacherId=@teacherId AND";
                        da.Parameters.AddWithValue("@teacherId", filterTeacher);
                    }

                    //check coordinator filter
                    if (filterCoordinator > -1)
                    {
                        //add coordinator filter and parameter
                        where += " r.coordinatorId=@coordinatorId AND";
                        da.Parameters.AddWithValue("@coordinatorId", filterCoordinator);
                    }

                    //check student filter
                    if (filterStudent > -1)
                    {
                        //add student filter and parameter
                        where += " r.studentId=@studentId AND";
                        da.Parameters.AddWithValue("@studentId", filterStudent);
                    }

                    //check class filter
                    if (filterClass > -1)
                    {
                        //add class filter and parameter
                        where += " r.classId=@classId AND";
                        da.Parameters.AddWithValue("@classId", filterClass);
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

                    //execute query
                    n = (object)da.ExecuteScalar();
                }

                if (closeTransaction && transaction != null)
                    transaction.Commit();

                if (closeConnection && connection.State == ConnectionState.Open)
                    connection.Close();

                //check result
                if (n != System.DBNull.Value)
                {
                    return Convert.ToDouble(n);
                }
                else
                {
                    return double.MinValue;
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
        /// Find grades by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterGradeRapporteur">
        /// The grade rapporteur filter.
        /// -1 to select all rapporteurs.
        /// </param>
        /// <param name="filterGradeTarget">
        /// The grade target filter.
        /// -1 to select all targets.
        /// </param>
        /// <param name="filterGradePeriodicity">
        /// The grade periodicity filter.
        /// -1 to select all periodicities.
        /// </param>
        /// <param name="filterGradeSubject">
        /// The grade subject filter.
        /// -1 to select all subjects.
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
        /// <param name="filterPole">
        /// The pole filter
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterCoordinator">
        /// The coordinator filter.
        /// -1 to select all coordinators.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of database rows.
        /// Null if no Grade was found.
        /// </returns>
        public static DataRow[] FindByFilter(
            MySqlTransaction trans, int filterGradeRapporteur, int filterGradeTarget,
            int filterGradePeriodicity, int filterGradeSubject, int filterSemester,
            DateTime filterReferenceDate, int filterInstitution, int filterPole, 
            int filterTeacher, int filterCoordinator, int filterStudent, int filterClass)
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
                    string select = "SELECT r.* FROM grade r ";

                    //check if pole filter is set
                    if (filterPole > 0)
                    {
                        //extend select
                        select += " INNER JOIN class c on c.classId = r.classId ";
                    }

                    //create where clause
                    string where = string.Empty;

                    //check filters
                    //check grade rapporteur filter
                    if (filterGradeRapporteur > -1)
                    {
                        //add grade type filter and parameter
                        where += " r.gradeRapporteur=@gradeRapporteur AND";
                        da.SelectCommand.Parameters.AddWithValue("@gradeRapporteur", filterGradeRapporteur);
                    }

                    //check grade target filter
                    if (filterGradeTarget > -1)
                    {
                        //add grade type filter and parameter
                        where += " r.gradeTarget=@gradeTarget AND";
                        da.SelectCommand.Parameters.AddWithValue("@gradeTarget", filterGradeTarget);
                    }

                    //check grade periodicity filter
                    if (filterGradePeriodicity > -1)
                    {
                        //add grade type filter and parameter
                        where += " r.gradePeriodicity=@gradePeriodicity AND";
                        da.SelectCommand.Parameters.AddWithValue("@gradePeriodicity", filterGradePeriodicity);
                    }

                    //check grade subject filter
                    if (filterGradeSubject > -1)
                    {
                        //add grade type filter and parameter
                        where += " r.gradeSubject=@gradeSubject AND";
                        da.SelectCommand.Parameters.AddWithValue("@gradeSubject", filterGradeSubject);
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

                    //check pole filter
                    if (filterPole > -1)
                    {
                        //add pole filter and parameter
                        where += " c.poleId=@poleId AND";
                        da.SelectCommand.Parameters.AddWithValue("@poleId", filterPole);
                    }

                    //check teacher filter
                    if (filterTeacher > -1)
                    {
                        //add teacher filter and parameter
                        where += " r.teacherId=@teacherId AND";
                        da.SelectCommand.Parameters.AddWithValue("@teacherId", filterTeacher);
                    }

                    //check coordinator filter
                    if (filterCoordinator > -1)
                    {
                        //add coordinator filter and parameter
                        where += " r.coordinatorId=@coordinatorId AND";
                        da.SelectCommand.Parameters.AddWithValue("@coordinatorId", filterCoordinator);
                    }

                    //check student filter
                    if (filterStudent > -1)
                    {
                        //add student filter and parameter
                        where += " r.studentId=@studentId AND";
                        da.SelectCommand.Parameters.AddWithValue("@studentId", filterStudent);
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
                    string orderBy = " ORDER BY r.gradeId ";
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

    } //end of class GradeAccess

} //end of namespace PnT.SongDB.Access
