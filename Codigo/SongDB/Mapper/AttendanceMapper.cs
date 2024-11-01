using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for AttendanceMapper.
    /// </summary>
    public class AttendanceMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Attendance to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Attendance.</returns>
        public static int Save(MySqlTransaction trans, Attendance attendance)
        {
            return Access.AttendanceAccess.Save(trans, GetParameters(attendance));
        }

        /// <summary>
        /// Delete Attendance by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Attendance.</param>
        /// <returns>
        /// True if selected Attendance was deleted.
        /// False if selected Attendance was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.AttendanceAccess.Delete(trans, id);
        }

        /// <summary>
        /// Delete all Attendance for selected class student.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="classId">The id of the selected class.</param>
        /// <param name="studentId">The id of the selected student.</param>
        /// <returns>
        /// The number of deleted Attendances.
        /// </returns>
        public static int DeleteForClassStudent(
            MySqlTransaction trans, int classId, int studentId)
        {
            return Access.AttendanceAccess.DeleteForClassStudent(
                trans, classId, studentId);
        }

        /// <summary>
        /// Inactivate Attendance by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Attendance.</param>
        /// <returns>
        /// True if selected Attendance was inactivated.
        /// False if selected Attendance was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Access.AttendanceAccess.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Attendance.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Attendance objects.
        /// Null if no Attendance was found.
        /// </returns>
        public static List<Attendance> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.AttendanceAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Attendance by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Attendance.
        /// Null if selected Attendance was not found.
        /// </returns>
        public static Attendance Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.AttendanceAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Count attendances by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterRollCall">
        /// The roll call filter.
        /// -1 to selct all roll calls.
        /// </param>
        /// <param name="filterStartDate">
        /// The start date filter.
        /// DateTime.MinValue to select all dates.
        /// </param>
        /// <param name="filterEndDate">
        /// The end date filter.
        /// DateTime.MinValue to select all dates.
        /// </param>
        /// <returns>
        /// The number of attendances.
        /// </returns>
        public static int CountByFilter(
            MySqlTransaction trans, int filterClass, int filterStudent, 
            int filterTeacher, int filterRollCall, DateTime filterStartDate, DateTime filterEndDate)
        {
            return Access.AttendanceAccess.CountByFilter(
                trans, filterClass, filterStudent, filterTeacher, 
                filterRollCall, filterStartDate, filterEndDate);
        }

        /// <summary>
        /// Find attendances by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <returns>
        /// List of Attendance objects.
        /// Null if no Attendance was found.
        /// </returns>
        public static List<Attendance> FindByFilter(
            MySqlTransaction trans, int filterClass, int filterStudent)
        {
            DataRow[] dr = Access.AttendanceAccess.FindByFilter(
                trans, filterClass, filterStudent);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find attendances by class filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterSemester">
        /// The class semester filter.
        /// -1 to select all semesters.
        /// </param>
        /// <param name="filterInstitution">
        /// The class institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The class pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterTeacher">
        /// The class teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of Attendance objects.
        /// Null if no Attendance was found.
        /// </returns>
        public static List<Attendance> FindByClassFilter(
            MySqlTransaction trans, int filterSemester, int filterInstitution,
            int filterPole, int filterTeacher, int filterClass)
        {
            DataRow[] dr = Access.AttendanceAccess.FindByClassFilter(
                trans, filterSemester, filterInstitution, 
                filterPole, filterTeacher, filterClass);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Attendance objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Attendance objects.</returns>
        private static List<Attendance> Map(DataRow[] rows)
        {
            List<Attendance> attendances = new List<Attendance>();

            for (int i = 0; i < rows.Length; i++)
                attendances.Add(Map(rows[i]));

            return attendances;
        }

        /// <summary>
        /// Map database row to a Attendance object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Attendance</returns>
        private static Attendance Map(DataRow row)
        {
            Attendance attendance = new Attendance((int)(row["attendanceId"]));
            attendance.StudentId = (int)DataAccessCommon.HandleDBNull(row, "studentId", typeof(int));
            attendance.TeacherId = (int)DataAccessCommon.HandleDBNull(row, "teacherId", typeof(int));
            attendance.ClassId = (int)DataAccessCommon.HandleDBNull(row, "classId", typeof(int));
            attendance.ClassDay = (int)DataAccessCommon.HandleDBNull(row, "classDay", typeof(int));
            attendance.Date = (DateTime)DataAccessCommon.HandleDBNull(row, "date", typeof(DateTime));
            attendance.RollCall = (int)DataAccessCommon.HandleDBNull(row, "rollCall", typeof(int));

            return attendance;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Attendance
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Attendance attendance)
        {
            MySqlParameter[] parameters = new MySqlParameter[7];
            parameters[0] = new MySqlParameter("attendanceId", attendance.Id);
            parameters[1] = new MySqlParameter("studentId", attendance.StudentId);
            parameters[2] = new MySqlParameter("teacherId", attendance.TeacherId);
            parameters[3] = new MySqlParameter("classId", attendance.ClassId);
            parameters[4] = new MySqlParameter("classDay", attendance.ClassDay);
            parameters[5] = new MySqlParameter("date", attendance.Date);
            parameters[6] = new MySqlParameter("rollCall", attendance.RollCall);

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class AttendanceMapper

} //end of namespace PnT.SongDB.Mapper
