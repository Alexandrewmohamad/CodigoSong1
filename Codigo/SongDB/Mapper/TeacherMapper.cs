using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for TeacherMapper.
    /// </summary>
    public class TeacherMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Teacher to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Teacher.</returns>
        public static int Save(MySqlTransaction trans, Teacher teacher)
        {
            return Access.TeacherAccess.Save(trans, GetParameters(teacher));
        }

        /// <summary>
        /// Delete Teacher by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Teacher.</param>
        /// <returns>
        /// True if selected Teacher was deleted.
        /// False if selected Teacher was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.TeacherAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Teacher by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Teacher.</param>
        /// <param name="inactivationReason">
        /// The reason why the teacher is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Teacher was inactivated.
        /// False if selected Teacher was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id, string inactivationReason)
        {
            return Access.TeacherAccess.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all Teacher.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Teacher objects.
        /// Null if no Teacher was found.
        /// </returns>
        public static List<Teacher> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.TeacherAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Teacher by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Teacher.
        /// Null if selected Teacher was not found.
        /// </returns>
        public static Teacher Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.TeacherAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Teacher by user id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected user Teacher.
        /// Null if selected user has no Teacher.
        /// </returns>
        public static Teacher FindByUser(MySqlTransaction trans, int userId)
        {
            DataRow dr = Access.TeacherAccess.FindByUser(trans, userId);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Count teachers by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterTeacherStatus">
        /// The teacher status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <returns>
        /// The number of teachers.
        /// </returns>
        public static int CountByFilter(
            MySqlTransaction trans, int filterTeacherStatus, int filterInstitution, int filterPole)
        {
            return Access.TeacherAccess.CountByFilter(
                trans, filterTeacherStatus, filterInstitution, filterPole);
        }

        /// <summary>
        /// Find teachers by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterTeacherStatus">
        /// The teacher status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <returns>
        /// List of Teacher objects.
        /// Null if no Teacher was found.
        /// </returns>
        public static List<Teacher> FindByFilter(
            MySqlTransaction trans, int filterTeacherStatus, int filterInstitution, int filterPole)
        {
            DataRow[] dr = Access.TeacherAccess.FindByFilter(
                trans, filterTeacherStatus, filterInstitution, filterPole);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Teacher objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Teacher objects.</returns>
        private static List<Teacher> Map(DataRow[] rows)
        {
            List<Teacher> teachers = new List<Teacher>();

            for (int i = 0; i < rows.Length; i++)
                teachers.Add(Map(rows[i]));

            return teachers;
        }

        /// <summary>
        /// Map database row to a Teacher object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Teacher</returns>
        private static Teacher Map(DataRow row)
        {
            Teacher teacher = new Teacher((int)(row["teacherId"]));
            teacher.UserId = (int)DataAccessCommon.HandleDBNull(row, "userId", typeof(int));
            teacher.Name = (string)DataAccessCommon.HandleDBNull(row, "name", typeof(string));
            teacher.Birthdate = (DateTime)DataAccessCommon.HandleDBNull(row, "birthdate", typeof(DateTime));
            teacher.Identity = (string)DataAccessCommon.HandleDBNull(row, "identity", typeof(string));
            teacher.IdentityAgency = (string)DataAccessCommon.HandleDBNull(row, "identityAgency", typeof(string));
            teacher.IdentityDate = (DateTime)DataAccessCommon.HandleDBNull(row, "identityDate", typeof(DateTime));
            teacher.TaxId = (string)DataAccessCommon.HandleDBNull(row, "taxId", typeof(string));
            teacher.PisId = (string)DataAccessCommon.HandleDBNull(row, "pisId", typeof(string));
            teacher.Address = (string)DataAccessCommon.HandleDBNull(row, "address", typeof(string));
            teacher.District = (string)DataAccessCommon.HandleDBNull(row, "district", typeof(string));
            teacher.City = (string)DataAccessCommon.HandleDBNull(row, "city", typeof(string));
            teacher.State = (string)DataAccessCommon.HandleDBNull(row, "state", typeof(string));
            teacher.ZipCode = (string)DataAccessCommon.HandleDBNull(row, "zipCode", typeof(string));
            teacher.Phone = (string)DataAccessCommon.HandleDBNull(row, "phone", typeof(string));
            teacher.Mobile = (string)DataAccessCommon.HandleDBNull(row, "mobile", typeof(string));
            teacher.Email = (string)DataAccessCommon.HandleDBNull(row, "email", typeof(string));
            teacher.AcademicBackground = (string)DataAccessCommon.HandleDBNull(row, "academicBackground", typeof(string));
            teacher.WorkExperience = (string)DataAccessCommon.HandleDBNull(row, "workExperience", typeof(string));
            teacher.Photo = (byte[])DataAccessCommon.HandleDBNull(row, "photo", typeof(byte[]));
            teacher.TeacherStatus = (int)DataAccessCommon.HandleDBNull(row, "teacherStatus", typeof(int));
            teacher.CreationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "creationTime", typeof(DateTime));
            teacher.InactivationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "inactivationTime", typeof(DateTime));
            teacher.InactivationReason = (string)DataAccessCommon.HandleDBNull(row, "inactivationReason", typeof(string));

            return teacher;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Teacher
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Teacher teacher)
        {
            MySqlParameter[] parameters = new MySqlParameter[24];
            parameters[0] = new MySqlParameter("teacherId", teacher.Id);
            parameters[1] = new MySqlParameter("userId", teacher.UserId);
            parameters[2] = new MySqlParameter("name", teacher.Name);
            parameters[3] = new MySqlParameter("birthdate", DataAccessCommon.HandleDBNull(teacher.Birthdate));
            parameters[4] = new MySqlParameter("identity", teacher.Identity);
            parameters[5] = new MySqlParameter("identityAgency", teacher.IdentityAgency);
            parameters[6] = new MySqlParameter("identityDate", DataAccessCommon.HandleDBNull(teacher.IdentityDate));
            parameters[7] = new MySqlParameter("taxId", DataAccessCommon.HandleDBNull(teacher.TaxId));
            parameters[8] = new MySqlParameter("pisId", DataAccessCommon.HandleDBNull(teacher.PisId));
            parameters[9] = new MySqlParameter("address", teacher.Address);
            parameters[10] = new MySqlParameter("district", teacher.District);
            parameters[11] = new MySqlParameter("city", teacher.City);
            parameters[12] = new MySqlParameter("state", teacher.State);
            parameters[13] = new MySqlParameter("zipCode", teacher.ZipCode);
            parameters[14] = new MySqlParameter("phone", DataAccessCommon.HandleDBNull(teacher.Phone));
            parameters[15] = new MySqlParameter("mobile", teacher.Mobile);
            parameters[16] = new MySqlParameter("email", teacher.Email);
            parameters[17] = new MySqlParameter("academicBackground", teacher.AcademicBackground);
            parameters[18] = new MySqlParameter("workExperience", teacher.WorkExperience);
            parameters[19] = new MySqlParameter("photo", DataAccessCommon.HandleDBNull(teacher.Photo));
            parameters[20] = new MySqlParameter("teacherStatus", teacher.TeacherStatus);
            parameters[21] = new MySqlParameter("creationTime", teacher.Id == -1 ? DateTime.Now : teacher.CreationTime);
            parameters[22] = new MySqlParameter("inactivationTime", DataAccessCommon.HandleDBNull(teacher.InactivationTime));
            parameters[23] = new MySqlParameter("inactivationReason", DataAccessCommon.HandleDBNull(teacher.InactivationReason));

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class TeacherMapper

} //end of namespace PnT.SongDB.Mapper
