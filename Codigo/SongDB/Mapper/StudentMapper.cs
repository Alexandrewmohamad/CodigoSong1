using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for StudentMapper.
    /// </summary>
    public class StudentMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Student to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Student.</returns>
        public static int Save(MySqlTransaction trans, Student student)
        {
            return Access.StudentAccess.Save(trans, GetParameters(student));
        }

        /// <summary>
        /// Delete Student by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Student.</param>
        /// <returns>
        /// True if selected Student was deleted.
        /// False if selected Student was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.StudentAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Student by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Student.</param>
        /// <param name="inactivationReason">
        /// The reason why the student is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Student was inactivated.
        /// False if selected Student was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id, string inactivationReason)
        {
            return Access.StudentAccess.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all Student.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.StudentAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Student by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Student.
        /// Null if selected Student was not found.
        /// </returns>
        public static Student Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.StudentAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Count students by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterStudentStatus">
        /// The student status filter.
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
        /// The number of students.
        /// </returns>
        public static int CountByFilter(
            MySqlTransaction trans, int filterStudentStatus, int filterInstitution, int filterPole)
        {
            return Access.StudentAccess.CountByFilter(
                trans, filterStudentStatus, filterInstitution, filterPole);
        }

        /// <summary>
        /// Find students by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterStudentStatus">
        /// The student status filter.
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
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> FindByFilter(
            MySqlTransaction trans, int filterStudentStatus, int filterInstitution, int filterPole)
        {
            DataRow[] dr = Access.StudentAccess.FindByFilter(
                trans, filterStudentStatus, filterInstitution, filterPole);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all students that loaned selected class.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <returns>
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> FindByClass(
            MySqlTransaction trans, int classId)
        {
            DataRow[] dr = Access.StudentAccess.FindByClass(trans, classId);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all students that loaned selected instrument.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="instrumentId">
        /// The ID of the selected instrument.
        /// </param>
        /// <returns>
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> FindByInstrument(
            MySqlTransaction trans, int instrumentId)
        {
            DataRow[] dr = Access.StudentAccess.FindByInstrument(trans, instrumentId);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all students that currently have an active loan.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Student objects.
        /// Null if no Student was found.
        /// </returns>
        public static List<Student> FindWithLoan(MySqlTransaction trans)
        {
            DataRow[] dr = Access.StudentAccess.FindWithLoan(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Student objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Student objects.</returns>
        private static List<Student> Map(DataRow[] rows)
        {
            List<Student> students = new List<Student>();

            for (int i = 0; i < rows.Length; i++)
                students.Add(Map(rows[i]));

            return students;
        }

        /// <summary>
        /// Map database row to a Student object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Student</returns>
        private static Student Map(DataRow row)
        {
            Student student = new Student((int)(row["studentId"]));
            student.UserId = (int)DataAccessCommon.HandleDBNull(row, "userId", typeof(int));
            student.PoleId = (int)DataAccessCommon.HandleDBNull(row, "poleId", typeof(int));
            student.Name = (string)DataAccessCommon.HandleDBNull(row, "name", typeof(string));
            student.Birthdate = (DateTime)DataAccessCommon.HandleDBNull(row, "birthdate", typeof(DateTime));
            student.GuardianName = (string)DataAccessCommon.HandleDBNull(row, "guardianName", typeof(string));
            student.GuardianIdentity = (string)DataAccessCommon.HandleDBNull(row, "guardianIdentity", typeof(string));
            student.GuardianIdentityAgency = (string)DataAccessCommon.HandleDBNull(row, "guardianIdentityAgency", typeof(string));
            student.GuardianIdentityDate = (DateTime)DataAccessCommon.HandleDBNull(row, "guardianIdentityDate", typeof(DateTime));
            student.GuardianTaxId = (string)DataAccessCommon.HandleDBNull(row, "guardianTaxId", typeof(string));
            student.Address = (string)DataAccessCommon.HandleDBNull(row, "address", typeof(string));
            student.District = (string)DataAccessCommon.HandleDBNull(row, "district", typeof(string));
            student.City = (string)DataAccessCommon.HandleDBNull(row, "city", typeof(string));
            student.State = (string)DataAccessCommon.HandleDBNull(row, "state", typeof(string));
            student.ZipCode = (string)DataAccessCommon.HandleDBNull(row, "zipCode", typeof(string));
            student.Phone = (string)DataAccessCommon.HandleDBNull(row, "phone", typeof(string));
            student.Mobile = (string)DataAccessCommon.HandleDBNull(row, "mobile", typeof(string));
            student.Email = (string)DataAccessCommon.HandleDBNull(row, "email", typeof(string));
            student.Photo = (byte[])DataAccessCommon.HandleDBNull(row, "photo", typeof(byte[]));
            student.AssignmentFile = (string)DataAccessCommon.HandleDBNull(row, "assignmentFile", typeof(string));
            student.PhotoFile = (string)DataAccessCommon.HandleDBNull(row, "photoFile", typeof(string));
            student.HasDisability = (bool)DataAccessCommon.HandleDBNull(row, "hasDisability", typeof(bool));
            student.SpecialCare = (string)DataAccessCommon.HandleDBNull(row, "specialCare", typeof(string));
            student.StudentStatus = (int)DataAccessCommon.HandleDBNull(row, "studentStatus", typeof(int));
            student.CreationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "creationTime", typeof(DateTime));
            student.InactivationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "inactivationTime", typeof(DateTime));
            student.InactivationReason = (string)DataAccessCommon.HandleDBNull(row, "inactivationReason", typeof(string));

            return student;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Student
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Student student)
        {
            MySqlParameter[] parameters = new MySqlParameter[27];
            parameters[0] = new MySqlParameter("studentId", student.Id);
            parameters[1] = new MySqlParameter("userId", DataAccessCommon.HandleDBNull(student.UserId));
            parameters[2] = new MySqlParameter("poleId", student.PoleId);
            parameters[3] = new MySqlParameter("name", student.Name);
            parameters[4] = new MySqlParameter("birthdate", DataAccessCommon.HandleDBNull(student.Birthdate));
            parameters[5] = new MySqlParameter("guardianName", student.GuardianName);
            parameters[6] = new MySqlParameter("guardianIdentity", DataAccessCommon.HandleDBNull(student.GuardianIdentity));
            parameters[7] = new MySqlParameter("guardianIdentityAgency", DataAccessCommon.HandleDBNull(student.GuardianIdentityAgency));
            parameters[8] = new MySqlParameter("guardianIdentityDate", DataAccessCommon.HandleDBNull(student.GuardianIdentityDate));
            parameters[9] = new MySqlParameter("guardianTaxId", DataAccessCommon.HandleDBNull(student.GuardianTaxId));
            parameters[10] = new MySqlParameter("address", student.Address);
            parameters[11] = new MySqlParameter("district", student.District);
            parameters[12] = new MySqlParameter("city", student.City);
            parameters[13] = new MySqlParameter("state", student.State);
            parameters[14] = new MySqlParameter("zipCode", student.ZipCode);
            parameters[15] = new MySqlParameter("phone", DataAccessCommon.HandleDBNull(student.Phone));
            parameters[16] = new MySqlParameter("mobile", DataAccessCommon.HandleDBNull(student.Mobile));
            parameters[17] = new MySqlParameter("email", DataAccessCommon.HandleDBNull(student.Email));
            parameters[18] = new MySqlParameter("photo", DataAccessCommon.HandleDBNull(student.Photo));
            parameters[19] = new MySqlParameter("assignmentFile", DataAccessCommon.HandleDBNull(student.AssignmentFile));
            parameters[20] = new MySqlParameter("photoFile", DataAccessCommon.HandleDBNull(student.PhotoFile));
            parameters[21] = new MySqlParameter("hasDisability", student.HasDisability);
            parameters[22] = new MySqlParameter("specialCare", DataAccessCommon.HandleDBNull(student.SpecialCare));
            parameters[23] = new MySqlParameter("studentStatus", student.StudentStatus);
            parameters[24] = new MySqlParameter("creationTime", student.Id == -1 ? DateTime.Now : student.CreationTime);
            parameters[25] = new MySqlParameter("inactivationTime", DataAccessCommon.HandleDBNull(student.InactivationTime));
            parameters[26] = new MySqlParameter("inactivationReason", DataAccessCommon.HandleDBNull(student.InactivationReason));

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class StudentMapper

} //end of namespace PnT.SongDB.Mapper
