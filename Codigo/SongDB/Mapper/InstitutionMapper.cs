using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for InstitutionMapper.
    /// </summary>
    public class InstitutionMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Institution to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Institution.</returns>
        public static int Save(MySqlTransaction trans, Institution institution)
        {
            return Access.InstitutionAccess.Save(trans, GetParameters(institution));
        }

        /// <summary>
        /// Delete Institution by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Institution.</param>
        /// <returns>
        /// True if selected Institution was deleted.
        /// False if selected Institution was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.InstitutionAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Institution by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Institution.</param>
        /// <param name="inactivationReason">
        /// The reason why the institution is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Institution was inactivated.
        /// False if selected Institution was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id, string inactivationReason)
        {
            return Access.InstitutionAccess.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all Institution.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Institution objects.
        /// Null if no Institution was found.
        /// </returns>
        public static List<Institution> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.InstitutionAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Institution by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Institution.
        /// Null if selected Institution was not found.
        /// </returns>
        public static Institution Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.InstitutionAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Count institutions by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterInstitutionStatus">
        /// The institution status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <returns>
        /// The number of institutions.
        /// </returns>
        public static int CountByFilter(
            MySqlTransaction trans, int filterInstitutionStatus)
        {
            return Access.InstitutionAccess.CountByFilter(
                trans, filterInstitutionStatus);
        }

        /// <summary>
        /// Find institutions by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterInstitutionStatus">
        /// The institution status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <returns>
        /// List of Institution objects.
        /// Null if no Institution was found.
        /// </returns>
        public static List<Institution> FindByFilter(
            MySqlTransaction trans, int filterInstitutionStatus)
        {
            DataRow[] dr = Access.InstitutionAccess.FindByFilter(
                trans, filterInstitutionStatus);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Institution objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Institution objects.</returns>
        private static List<Institution> Map(DataRow[] rows)
        {
            List<Institution> institutions = new List<Institution>();

            for (int i = 0; i < rows.Length; i++)
                institutions.Add(Map(rows[i]));

            return institutions;
        }

        /// <summary>
        /// Map database row to a Institution object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Institution</returns>
        private static Institution Map(DataRow row)
        {
            Institution institution = new Institution((int)(row["institutionId"]));
            institution.EntityName = (string)DataAccessCommon.HandleDBNull(row, "entityName", typeof(string));
            institution.ProjectName = (string)DataAccessCommon.HandleDBNull(row, "projectName", typeof(string));
            institution.LocalInitiative = (string)DataAccessCommon.HandleDBNull(row, "localInitiative", typeof(string));
            institution.Institutionalized = (bool)DataAccessCommon.HandleDBNull(row, "institutionalized", typeof(bool));
            institution.TaxId = (string)DataAccessCommon.HandleDBNull(row, "taxId", typeof(string));
            institution.CoordinatorId = (int)DataAccessCommon.HandleDBNull(row, "coordinatorId", typeof(int));
            institution.Address = (string)DataAccessCommon.HandleDBNull(row, "address", typeof(string));
            institution.District = (string)DataAccessCommon.HandleDBNull(row, "district", typeof(string));
            institution.City = (string)DataAccessCommon.HandleDBNull(row, "city", typeof(string));
            institution.State = (string)DataAccessCommon.HandleDBNull(row, "state", typeof(string));
            institution.ZipCode = (string)DataAccessCommon.HandleDBNull(row, "zipCode", typeof(string));
            institution.Phone = (string)DataAccessCommon.HandleDBNull(row, "phone", typeof(string));
            institution.Mobile = (string)DataAccessCommon.HandleDBNull(row, "mobile", typeof(string));
            institution.Site = (string)DataAccessCommon.HandleDBNull(row, "site", typeof(string));
            institution.Email = (string)DataAccessCommon.HandleDBNull(row, "email", typeof(string));
            institution.Description = (string)DataAccessCommon.HandleDBNull(row, "description", typeof(string));
            institution.InstitutionStatus = (int)DataAccessCommon.HandleDBNull(row, "institutionStatus", typeof(int));
            institution.CreationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "creationTime", typeof(DateTime));
            institution.InactivationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "inactivationTime", typeof(DateTime));
            institution.InactivationReason = (string)DataAccessCommon.HandleDBNull(row, "inactivationReason", typeof(string));

            return institution;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Institution
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Institution institution)
        {
            MySqlParameter[] parameters = new MySqlParameter[21];
            parameters[0] = new MySqlParameter("institutionId", institution.Id);
            parameters[1] = new MySqlParameter("entityName", institution.EntityName);
            parameters[2] = new MySqlParameter("projectName", institution.ProjectName);
            parameters[3] = new MySqlParameter("localInitiative", DataAccessCommon.HandleDBNull(institution.LocalInitiative));
            parameters[4] = new MySqlParameter("institutionalized", institution.Institutionalized);
            parameters[5] = new MySqlParameter("taxId", DataAccessCommon.HandleDBNull(institution.TaxId));
            parameters[6] = new MySqlParameter("coordinatorId", institution.CoordinatorId);
            parameters[7] = new MySqlParameter("address", institution.Address);
            parameters[8] = new MySqlParameter("district", institution.District);
            parameters[9] = new MySqlParameter("city", institution.City);
            parameters[10] = new MySqlParameter("state", institution.State);
            parameters[11] = new MySqlParameter("zipCode", institution.ZipCode);
            parameters[12] = new MySqlParameter("phone", institution.Phone);
            parameters[13] = new MySqlParameter("mobile", institution.Mobile);
            parameters[14] = new MySqlParameter("site", DataAccessCommon.HandleDBNull(institution.Site));
            parameters[15] = new MySqlParameter("email", institution.Email);
            parameters[16] = new MySqlParameter("description", institution.Description);
            parameters[17] = new MySqlParameter("institutionStatus", institution.InstitutionStatus);
            parameters[18] = new MySqlParameter("creationTime", institution.Id == -1 ? DateTime.Now : institution.CreationTime);
            parameters[19] = new MySqlParameter("inactivationTime", DataAccessCommon.HandleDBNull(institution.InactivationTime));
            parameters[20] = new MySqlParameter("inactivationReason", DataAccessCommon.HandleDBNull(institution.InactivationReason));

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class InstitutionMapper

} //end of namespace PnT.SongDB.Mapper
