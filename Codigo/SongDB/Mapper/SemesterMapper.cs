using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for SemesterMapper.
    /// </summary>
    public class SemesterMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Semester to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Semester.</returns>
        public static int Save(MySqlTransaction trans, Semester semester)
        {
            return Access.SemesterAccess.Save(trans, GetParameters(semester));
        }

        /// <summary>
        /// Delete Semester by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Semester.</param>
        /// <returns>
        /// True if selected Semester was deleted.
        /// False if selected Semester was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.SemesterAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Semester by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Semester.</param>
        /// <returns>
        /// True if selected Semester was inactivated.
        /// False if selected Semester was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Access.SemesterAccess.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Semester.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Semester objects.
        /// Null if no Semester was found.
        /// </returns>
        public static List<Semester> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.SemesterAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Semester by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Semester.
        /// Null if selected Semester was not found.
        /// </returns>
        public static Semester Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.SemesterAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Semester objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Semester objects.</returns>
        private static List<Semester> Map(DataRow[] rows)
        {
            List<Semester> semesters = new List<Semester>();

            for (int i = 0; i < rows.Length; i++)
                semesters.Add(Map(rows[i]));

            return semesters;
        }

        /// <summary>
        /// Map database row to a Semester object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Semester</returns>
        private static Semester Map(DataRow row)
        {
            Semester semester = new Semester((int)(row["semesterId"]));
            semester.ReferenceDate = (DateTime)DataAccessCommon.HandleDBNull(row,"referenceDate", typeof(DateTime));
            semester.StartDate = (DateTime)DataAccessCommon.HandleDBNull(row,"startDate", typeof(DateTime));
            semester.EndDate = (DateTime)DataAccessCommon.HandleDBNull(row,"endDate", typeof(DateTime));

            return semester;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Semester
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Semester semester)
        {
            MySqlParameter[] parameters = new MySqlParameter[4];
            parameters[0] = new MySqlParameter("semesterId",semester.Id);
            parameters[1] = new MySqlParameter("referenceDate",semester.ReferenceDate);
            parameters[2] = new MySqlParameter("startDate",semester.StartDate);
            parameters[3] = new MySqlParameter("endDate",semester.EndDate);

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class SemesterMapper

} //end of namespace PnT.SongDB.Mapper
