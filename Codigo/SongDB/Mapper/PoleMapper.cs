using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for PoleMapper.
    /// </summary>
    public class PoleMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Pole to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Pole.</returns>
        public static int Save(MySqlTransaction trans, Pole pole)
        {
            return Access.PoleAccess.Save(trans, GetParameters(pole));
        }

        /// <summary>
        /// Delete Pole by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Pole.</param>
        /// <returns>
        /// True if selected Pole was deleted.
        /// False if selected Pole was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.PoleAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Pole by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Pole.</param>
        /// <param name="inactivationReason">
        /// The reason why the pole is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Pole was inactivated.
        /// False if selected Pole was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id, string inactivationReason)
        {
            return Access.PoleAccess.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all Pole.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Pole objects.
        /// Null if no Pole was found.
        /// </returns>
        public static List<Pole> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.PoleAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all Pole by teacher.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="teacherId">
        /// The ID of the selected teacher.
        /// </param>
        /// <returns>
        /// List of Pole objects.
        /// Null if no Pole was found.
        /// </returns>
        public static List<Pole> FindByTeacher(MySqlTransaction trans, int teacherId)
        {
            DataRow[] dr = Access.PoleAccess.FindByTeacher(trans, teacherId);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Pole by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Pole.
        /// Null if selected Pole was not found.
        /// </returns>
        public static Pole Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.PoleAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Count poles by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterPoleStatus">
        /// The pole status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <returns>
        /// The number of poles.
        /// </returns>
        public static int CountByFilter(
            MySqlTransaction trans, int filterPoleStatus, int filterInstitution)
        {
            return Access.PoleAccess.CountByFilter(
                trans, filterPoleStatus, filterInstitution);
        }

        /// <summary>
        /// Find poles by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterPoleStatus">
        /// The pole status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <returns>
        /// List of Pole objects.
        /// Null if no Pole was found.
        /// </returns>
        public static List<Pole> FindByFilter(
            MySqlTransaction trans, int filterPoleStatus, int filterInstitution)
        {
            DataRow[] dr = Access.PoleAccess.FindByFilter(
                trans, filterPoleStatus, filterInstitution);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Pole objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Pole objects.</returns>
        private static List<Pole> Map(DataRow[] rows)
        {
            List<Pole> poles = new List<Pole>();

            for (int i = 0; i < rows.Length; i++)
                poles.Add(Map(rows[i]));

            return poles;
        }

        /// <summary>
        /// Map database row to a Pole object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Pole</returns>
        private static Pole Map(DataRow row)
        {
            Pole pole = new Pole((int)(row["poleId"]));
            pole.InstitutionId = (int)DataAccessCommon.HandleDBNull(row, "institutionId", typeof(int));
            pole.Name = (string)DataAccessCommon.HandleDBNull(row, "name", typeof(string));
            pole.Address = (string)DataAccessCommon.HandleDBNull(row, "address", typeof(string));
            pole.District = (string)DataAccessCommon.HandleDBNull(row, "district", typeof(string));
            pole.City = (string)DataAccessCommon.HandleDBNull(row, "city", typeof(string));
            pole.State = (string)DataAccessCommon.HandleDBNull(row, "state", typeof(string));
            pole.ZipCode = (string)DataAccessCommon.HandleDBNull(row, "zipCode", typeof(string));
            pole.Phone = (string)DataAccessCommon.HandleDBNull(row, "phone", typeof(string));
            pole.Mobile = (string)DataAccessCommon.HandleDBNull(row, "mobile", typeof(string));
            pole.Email = (string)DataAccessCommon.HandleDBNull(row, "email", typeof(string));
            pole.Description = (string)DataAccessCommon.HandleDBNull(row, "description", typeof(string));
            pole.PoleStatus = (int)DataAccessCommon.HandleDBNull(row, "poleStatus", typeof(int));
            pole.CreationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "creationTime", typeof(DateTime));
            pole.InactivationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "inactivationTime", typeof(DateTime));
            pole.InactivationReason = (string)DataAccessCommon.HandleDBNull(row, "inactivationReason", typeof(string));

            return pole;
        }

        #endregion Mapper Methods

        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Pole
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Pole pole)
        {
            MySqlParameter[] parameters = new MySqlParameter[16];
            parameters[0] = new MySqlParameter("poleId", pole.Id);
            parameters[1] = new MySqlParameter("institutionId", pole.InstitutionId);
            parameters[2] = new MySqlParameter("name", pole.Name);
            parameters[3] = new MySqlParameter("address", pole.Address);
            parameters[4] = new MySqlParameter("district", pole.District);
            parameters[5] = new MySqlParameter("city", pole.City);
            parameters[6] = new MySqlParameter("state", pole.State);
            parameters[7] = new MySqlParameter("zipCode", pole.ZipCode);
            parameters[8] = new MySqlParameter("phone", pole.Phone);
            parameters[9] = new MySqlParameter("mobile", DataAccessCommon.HandleDBNull(pole.Mobile));
            parameters[10] = new MySqlParameter("email", DataAccessCommon.HandleDBNull(pole.Email));
            parameters[11] = new MySqlParameter("description", pole.Description);
            parameters[12] = new MySqlParameter("poleStatus", pole.PoleStatus);
            parameters[13] = new MySqlParameter("creationTime", pole.Id == -1 ? DateTime.Now : pole.CreationTime);
            parameters[14] = new MySqlParameter("inactivationTime", DataAccessCommon.HandleDBNull(pole.InactivationTime));
            parameters[15] = new MySqlParameter("inactivationReason", DataAccessCommon.HandleDBNull(pole.InactivationReason));

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class PoleMapper

} //end of namespace PnT.SongDB.Mapper
