using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for RolepermissionMapper.
    /// </summary>
    public class RolepermissionMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Rolepermission to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Rolepermission.</returns>
        public static int Save(MySqlTransaction trans, Rolepermission rolepermission)
        {
            return Access.RolepermissionAccess.Save(trans, GetParameters(rolepermission));
        }

        /// <summary>
        /// Delete Rolepermission by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Rolepermission.</param>
        /// <returns>
        /// True if selected Rolepermission was deleted.
        /// False if selected Rolepermission was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.RolepermissionAccess.Delete(trans, id);
        }

        /// <summary>
        /// Find all Rolepermission.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Rolepermission objects.
        /// Null if no Rolepermission was found.
        /// </returns>
        public static List<Rolepermission> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.RolepermissionAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Rolepermission by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Rolepermission.
        /// Null if selected Rolepermission was not found.
        /// </returns>
        public static Rolepermission Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.RolepermissionAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all Rolepermission for selected Role.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="roleId">The id of the selected Role.</param>
        /// <returns>
        /// List of Rolepermission objects.
        /// Null if no Rolepermission was found.
        /// </returns>
        public static List<Rolepermission> FindByRole(MySqlTransaction trans, int roleId)
        {
            DataRow[] dr = Access.RolepermissionAccess.FindByRole(trans, roleId);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Rolepermission objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Rolepermission objects.</returns>
        private static List<Rolepermission> Map(DataRow[] rows)
        {
            List<Rolepermission> rolepermissions = new List<Rolepermission>();

            for (int i = 0; i < rows.Length; i++)
                rolepermissions.Add(Map(rows[i]));

            return rolepermissions;
        }

        /// <summary>
        /// Map database row to a Rolepermission object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Rolepermission</returns>
        private static Rolepermission Map(DataRow row)
        {
            Rolepermission rolepermission = new Rolepermission((int)(row["rolePermissionId"]));
            rolepermission.RoleId = (int)DataAccessCommon.HandleDBNull(row,"roleId", typeof(int));
            rolepermission.PermissionId = (int)DataAccessCommon.HandleDBNull(row,"permissionId", typeof(int));

            return rolepermission;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Rolepermission
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Rolepermission rolepermission)
        {
            MySqlParameter[] parameters = new MySqlParameter[3];
            parameters[0] = new MySqlParameter("rolePermissionId", rolepermission.Id);
            parameters[1] = new MySqlParameter("roleId", rolepermission.RoleId);
            parameters[2] = new MySqlParameter("permissionId", rolepermission.PermissionId);

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class RolepermissionMapper

} //end of namespace PnT.SongDB.Mapper
