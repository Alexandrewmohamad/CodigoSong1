using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for GradeMapper.
    /// </summary>
    public class GradeMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Grade to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Grade.</returns>
        public static int Save(MySqlTransaction trans, Grade grade)
        {
            return Access.GradeAccess.Save(trans, GetParameters(grade));
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
            return Access.GradeAccess.Delete(trans, id);
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
            return Access.GradeAccess.DeleteForClassStudent(
                trans, classId, studentId);
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
            return Access.GradeAccess.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Grade.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Grade objects.
        /// Null if no Grade was found.
        /// </returns>
        public static List<Grade> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.GradeAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Grade by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Grade.
        /// Null if selected Grade was not found.
        /// </returns>
        public static Grade Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.GradeAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
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
            return Access.GradeAccess.AverageByFilter(
                trans, filterGradeRapporteur, filterGradeTarget, filterGradePeriodicity,
                filterGradeSubject, filterSemester, filterReferenceDate, filterInstitution,
                filterTeacher, filterCoordinator, filterStudent, filterClass);
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
        /// List of Grade objects.
        /// Null if no Grade was found.
        /// </returns>
        public static List<Grade> FindByFilter(
            MySqlTransaction trans, int filterGradeRapporteur, int filterGradeTarget, 
            int filterGradePeriodicity, int filterGradeSubject, int filterSemester, 
            DateTime filterReferenceDate, int filterInstitution, int filterPole, 
            int filterTeacher, int filterCoordinator, int filterStudent, int filterClass)
        {
            DataRow[] dr = Access.GradeAccess.FindByFilter(
                trans, filterGradeRapporteur, filterGradeTarget, filterGradePeriodicity,
                filterGradeSubject, filterSemester, filterReferenceDate, filterInstitution,
                filterPole, filterTeacher, filterCoordinator, filterStudent, filterClass);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Grade objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Grade objects.</returns>
        private static List<Grade> Map(DataRow[] rows)
        {
            List<Grade> grades = new List<Grade>();

            for (int i = 0; i < rows.Length; i++)
                grades.Add(Map(rows[i]));

            return grades;
        }

        /// <summary>
        /// Map database row to a Grade object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Grade</returns>
        private static Grade Map(DataRow row)
        {
            Grade grade = new Grade((int)(row["gradeId"]));
            grade.SemesterId = (int)DataAccessCommon.HandleDBNull(row, "semesterId", typeof(int));
            grade.StudentId = (int)DataAccessCommon.HandleDBNull(row, "studentId", typeof(int));
            grade.TeacherId = (int)DataAccessCommon.HandleDBNull(row, "teacherId", typeof(int));
            grade.CoordinatorId = (int)DataAccessCommon.HandleDBNull(row, "coordinatorId", typeof(int));
            grade.ClassId = (int)DataAccessCommon.HandleDBNull(row, "classId", typeof(int));
            grade.InstitutionId = (int)DataAccessCommon.HandleDBNull(row, "institutionId", typeof(int));
            grade.GradeRapporteur = (int)DataAccessCommon.HandleDBNull(row, "gradeRapporteur", typeof(int));
            grade.GradeTarget = (int)DataAccessCommon.HandleDBNull(row, "gradeTarget", typeof(int));
            grade.GradePeriodicity = (int)DataAccessCommon.HandleDBNull(row, "gradePeriodicity", typeof(int));
            grade.ReferenceDate = (DateTime)DataAccessCommon.HandleDBNull(row, "referenceDate", typeof(DateTime));
            grade.GradeSubject = (int)DataAccessCommon.HandleDBNull(row, "gradeSubject", typeof(int));
            grade.Score = (int)DataAccessCommon.HandleDBNull(row, "score", typeof(int));

            return grade;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Grade
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Grade grade)
        {
            MySqlParameter[] parameters = new MySqlParameter[13];
            parameters[0] = new MySqlParameter("gradeId", grade.Id);
            parameters[1] = new MySqlParameter("semesterId", grade.SemesterId);
            parameters[2] = new MySqlParameter("studentId", DataAccessCommon.HandleDBNull(grade.StudentId));
            parameters[3] = new MySqlParameter("teacherId", grade.TeacherId);
            parameters[4] = new MySqlParameter("coordinatorId", DataAccessCommon.HandleDBNull(grade.CoordinatorId));
            parameters[5] = new MySqlParameter("classId", DataAccessCommon.HandleDBNull(grade.ClassId));
            parameters[6] = new MySqlParameter("institutionId", grade.InstitutionId);
            parameters[7] = new MySqlParameter("gradeRapporteur", grade.GradeRapporteur);
            parameters[8] = new MySqlParameter("gradeTarget", grade.GradeTarget);
            parameters[9] = new MySqlParameter("gradePeriodicity", grade.GradePeriodicity);
            parameters[10] = new MySqlParameter("referenceDate", grade.ReferenceDate);
            parameters[11] = new MySqlParameter("gradeSubject", grade.GradeSubject);
            parameters[12] = new MySqlParameter("score", grade.Score);

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class GradeMapper

} //end of namespace PnT.SongDB.Mapper
