using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for ReportMapper.
    /// </summary>
    public class ReportMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Report to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Report.</returns>
        public static int Save(MySqlTransaction trans, Report report)
        {
            return Access.ReportAccess.Save(trans, GetParameters(report));
        }

        /// <summary>
        /// Delete Report by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Report.</param>
        /// <returns>
        /// True if selected Report was deleted.
        /// False if selected Report was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.ReportAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Report by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Report.</param>
        /// <returns>
        /// True if selected Report was inactivated.
        /// False if selected Report was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Access.ReportAccess.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Report.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Report objects.
        /// Null if no Report was found.
        /// </returns>
        public static List<Report> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.ReportAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Report by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Report.
        /// Null if selected Report was not found.
        /// </returns>
        public static Report Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.ReportAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find reports by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterReportStatus">
        /// The report status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterReportRapporteur">
        /// The report rapporteur filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterReportPeriodicity">
        /// The report periodicity filter.
        /// -1 to select all instrument types.
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
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of Report objects.
        /// Null if no Report was found.
        /// </returns>
        public static List<Report> FindByFilter(
            MySqlTransaction trans, int filterReportStatus, int filterReportRapporteur, 
            int filterReportPeriodicity, int filterSemester, DateTime filterReferenceDate, 
            int filterInstitution, int filterTeacher, int filterClass)
        {
            DataRow[] dr = Access.ReportAccess.FindByFilter(
                trans, filterReportStatus, filterReportRapporteur, filterReportPeriodicity,
                filterSemester, filterReferenceDate, filterInstitution, filterTeacher, filterClass);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Report objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Report objects.</returns>
        private static List<Report> Map(DataRow[] rows)
        {
            List<Report> reports = new List<Report>();

            for (int i = 0; i < rows.Length; i++)
                reports.Add(Map(rows[i]));

            return reports;
        }

        /// <summary>
        /// Map database row to a Report object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Report</returns>
        private static Report Map(DataRow row)
        {
            Report report = new Report((int)(row["reportId"]));
            report.SemesterId = (int)DataAccessCommon.HandleDBNull(row, "semesterId", typeof(int));
            report.ClassId = (int)DataAccessCommon.HandleDBNull(row, "classId", typeof(int));
            report.InstitutionId = (int)DataAccessCommon.HandleDBNull(row, "institutionId", typeof(int));
            report.TeacherId = (int)DataAccessCommon.HandleDBNull(row, "teacherId", typeof(int));
            report.CoordinatorId = (int)DataAccessCommon.HandleDBNull(row, "coordinatorId", typeof(int));
            report.ReportRapporteur = (int)DataAccessCommon.HandleDBNull(row, "reportRapporteur", typeof(int));
            report.ReportTarget = (int)DataAccessCommon.HandleDBNull(row, "reportTarget", typeof(int));
            report.ReportPeriodicity = (int)DataAccessCommon.HandleDBNull(row, "reportPeriodicity", typeof(int));
            report.ReferenceDate = (DateTime)DataAccessCommon.HandleDBNull(row, "referenceDate", typeof(DateTime));
            report.ReferenceList = (string)DataAccessCommon.HandleDBNull(row, "referenceList", typeof(string));
            report.ReportStatus = (int)DataAccessCommon.HandleDBNull(row, "reportStatus", typeof(int));

            return report;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Report
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Report report)
        {
            MySqlParameter[] parameters = new MySqlParameter[12];
            parameters[0] = new MySqlParameter("reportId", report.Id);
            parameters[1] = new MySqlParameter("semesterId", report.SemesterId);
            parameters[2] = new MySqlParameter("classId", DataAccessCommon.HandleDBNull(report.ClassId));
            parameters[3] = new MySqlParameter("institutionId", report.InstitutionId);
            parameters[4] = new MySqlParameter("teacherId", DataAccessCommon.HandleDBNull(report.TeacherId));
            parameters[5] = new MySqlParameter("coordinatorId", DataAccessCommon.HandleDBNull(report.CoordinatorId));
            parameters[6] = new MySqlParameter("reportRapporteur", report.ReportRapporteur);
            parameters[7] = new MySqlParameter("reportTarget", report.ReportTarget);
            parameters[8] = new MySqlParameter("reportPeriodicity", report.ReportPeriodicity);
            parameters[9] = new MySqlParameter("referenceDate", report.ReferenceDate);
            parameters[10] = new MySqlParameter("referenceList", DataAccessCommon.HandleDBNull(report.ReferenceList));
            parameters[11] = new MySqlParameter("reportStatus", report.ReportStatus);

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class ReportMapper

} //end of namespace PnT.SongDB.Mapper
