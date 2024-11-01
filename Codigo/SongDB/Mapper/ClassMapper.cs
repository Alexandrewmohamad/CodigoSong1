using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for ClassMapper.
    /// </summary>
    public class ClassMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Class to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Class.</returns>
        public static int Save(MySqlTransaction trans, Class classObj)
        {
            return Access.ClassAccess.Save(trans, GetParameters(classObj));
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
            return Access.ClassAccess.Delete(trans, id);
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
            return Access.ClassAccess.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all Class.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Class objects.
        /// Null if no Class was found.
        /// </returns>
        public static List<Class> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.ClassAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Class by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Class.
        /// Null if selected Class was not found.
        /// </returns>
        public static Class Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.ClassAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
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
            return Access.ClassAccess.CountByFilter(
                trans, filterClassStatus, filterClassType, filterInstrumentType,
                filterClassLevel, filterSemester, filterInstitution, filterPole, filterTeacher);
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
        /// List of Class objects.
        /// Null if no Class was found.
        /// </returns>
        public static List<Class> FindByFilter(
            MySqlTransaction trans, int filterClassStatus, int filterClassType,
            int filterInstrumentType, int filterClassLevel, int filterSemester, 
            int filterInstitution, int filterPole, int filterTeacher)
        {
            DataRow[] dr = Access.ClassAccess.FindByFilter(
                trans, filterClassStatus, filterClassType, filterInstrumentType, 
                filterClassLevel, filterSemester, filterInstitution, filterPole, filterTeacher);

            if (dr != null)
                return Map(dr);
            else
                return null;
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
        /// List of Class objects.
        /// Null if no Class was found.
        /// </returns>
        public static List<Class> FindByStudent(
            MySqlTransaction trans, int studentId)
        {
            DataRow[] dr = Access.ClassAccess.FindByStudent(trans, studentId);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find next available subject code.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The next available subject code.
        /// </returns>
        public static int FindNextAvailableSubjectCode(MySqlTransaction trans)
        {
            return Access.ClassAccess.FindNextAvailableSubjectCode(trans);
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Class objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Class objects.</returns>
        private static List<Class> Map(DataRow[] rows)
        {
            List<Class> classObjs = new List<Class>();

            for (int i = 0; i < rows.Length; i++)
                classObjs.Add(Map(rows[i]));

            return classObjs;
        }

        /// <summary>
        /// Map database row to a Class object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Class</returns>
        private static Class Map(DataRow row)
        {
            Class classObj = new Class((int)(row["classId"]));
            classObj.SemesterId = (int)DataAccessCommon.HandleDBNull(row,"semesterId", typeof(int));
            classObj.PoleId = (int)DataAccessCommon.HandleDBNull(row,"poleId", typeof(int));
            classObj.TeacherId = (int)DataAccessCommon.HandleDBNull(row,"teacherId", typeof(int));
            classObj.SubjectCode = (int)DataAccessCommon.HandleDBNull(row,"subjectCode", typeof(int));
            classObj.Code = (string)DataAccessCommon.HandleDBNull(row,"code", typeof(string));
            classObj.ClassType = (int)DataAccessCommon.HandleDBNull(row,"classType", typeof(int));
            classObj.InstrumentType = (int)DataAccessCommon.HandleDBNull(row,"instrumentType", typeof(int));
            classObj.ClassLevel = (int)DataAccessCommon.HandleDBNull(row,"classLevel", typeof(int));
            classObj.Capacity = (int)DataAccessCommon.HandleDBNull(row,"capacity", typeof(int));
            classObj.WeekMonday = (bool)DataAccessCommon.HandleDBNull(row,"weekMonday", typeof(bool));
            classObj.WeekTuesday = (bool)DataAccessCommon.HandleDBNull(row,"weekTuesday", typeof(bool));
            classObj.WeekWednesday = (bool)DataAccessCommon.HandleDBNull(row,"weekWednesday", typeof(bool));
            classObj.WeekThursday = (bool)DataAccessCommon.HandleDBNull(row,"weekThursday", typeof(bool));
            classObj.WeekFriday = (bool)DataAccessCommon.HandleDBNull(row,"weekFriday", typeof(bool));
            classObj.WeekSaturday = (bool)DataAccessCommon.HandleDBNull(row,"weekSaturday", typeof(bool));
            classObj.WeekSunday = (bool)DataAccessCommon.HandleDBNull(row,"weekSunday", typeof(bool));
            classObj.StartTime = (DateTime)DataAccessCommon.HandleDBNull(row,"startTime", typeof(DateTime));
            classObj.Duration = (int)DataAccessCommon.HandleDBNull(row,"duration", typeof(int));
            classObj.ClassStatus = (int)DataAccessCommon.HandleDBNull(row,"classStatus", typeof(int));
            classObj.CreationTime = (DateTime)DataAccessCommon.HandleDBNull(row,"creationTime", typeof(DateTime));
            classObj.InactivationTime = (DateTime)DataAccessCommon.HandleDBNull(row,"inactivationTime", typeof(DateTime));
            classObj.InactivationReason = (string)DataAccessCommon.HandleDBNull(row,"inactivationReason", typeof(string));

            return classObj;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Class
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Class classObj)
        {
            MySqlParameter[] parameters = new MySqlParameter[23];
            parameters[0] = new MySqlParameter("classId",classObj.Id);
            parameters[1] = new MySqlParameter("semesterId",classObj.SemesterId);
            parameters[2] = new MySqlParameter("poleId",classObj.PoleId);
            parameters[3] = new MySqlParameter("teacherId",classObj.TeacherId);
            parameters[4] = new MySqlParameter("subjectCode", classObj.SubjectCode);
            parameters[5] = new MySqlParameter("code", classObj.Code);
            parameters[6] = new MySqlParameter("classType", classObj.ClassType);
            parameters[7] = new MySqlParameter("instrumentType", DataAccessCommon.HandleDBNull(classObj.InstrumentType));
            parameters[8] = new MySqlParameter("classLevel", classObj.ClassLevel);
            parameters[9] = new MySqlParameter("capacity", classObj.Capacity);
            parameters[10] = new MySqlParameter("weekMonday", classObj.WeekMonday);
            parameters[11] = new MySqlParameter("weekTuesday", classObj.WeekTuesday);
            parameters[12] = new MySqlParameter("weekWednesday", classObj.WeekWednesday);
            parameters[13] = new MySqlParameter("weekThursday", classObj.WeekThursday);
            parameters[14] = new MySqlParameter("weekFriday", classObj.WeekFriday);
            parameters[15] = new MySqlParameter("weekSaturday", classObj.WeekSaturday);
            parameters[16] = new MySqlParameter("weekSunday", classObj.WeekSunday);
            parameters[17] = new MySqlParameter("startTime", classObj.StartTime);
            parameters[18] = new MySqlParameter("duration", classObj.Duration);
            parameters[19] = new MySqlParameter("classStatus", classObj.ClassStatus);
            parameters[20] = new MySqlParameter("creationTime", classObj.Id == -1 ? DateTime.Now : classObj.CreationTime);
            parameters[21] = new MySqlParameter("inactivationTime", DataAccessCommon.HandleDBNull(classObj.InactivationTime));
            parameters[22] = new MySqlParameter("inactivationReason", DataAccessCommon.HandleDBNull(classObj.InactivationReason));

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class ClassMapper

} //end of namespace PnT.SongDB.Mapper
