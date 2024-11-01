using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for PermissionMapper.
    /// </summary>
    public class PermissionMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Permission to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Permission.</returns>
        public static int Save(MySqlTransaction trans, Permission permission)
        {
            return Access.PermissionAccess.Save(trans, GetParameters(permission));
        }

        /// <summary>
        /// Delete Permission by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Permission.</param>
        /// <returns>
        /// True if selected Permission was deleted.
        /// False if selected Permission was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.PermissionAccess.Delete(trans, id);
        }

        /// <summary>
        /// Find all Permission.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Permission objects.
        /// Null if no Permission was found.
        /// </returns>
        public static List<Permission> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.PermissionAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Permission by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Permission.
        /// Null if selected Permission was not found.
        /// </returns>
        public static Permission Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.PermissionAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all assigned Permission for selected Role.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param> 
        /// <param name="roleId">The ID of the selected Role.</param>
        /// <returns>
        /// List of Permission objects.
        /// Null if no Permission was found.
        /// </returns>
        public static List<Permission> FindByRole(MySqlTransaction trans, int roleId)
        {
            DataRow[] dr = Access.PermissionAccess.FindByRole(trans, roleId);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Permission objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Permission objects.</returns>
        private static List<Permission> Map(DataRow[] rows)
        {
            List<Permission> permissions = new List<Permission>();

            for (int i = 0; i < rows.Length; i++)
                permissions.Add(Map(rows[i]));

            return permissions;
        }

        /// <summary>
        /// Map database row to a Permission object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Permission</returns>
        private static Permission Map(DataRow row)
        {
            Permission permission = new Permission((int)(row["permissionId"]));
            permission.Name = (string)DataAccessCommon.HandleDBNull(row,"name", typeof(string));
            permission.Description = (string)DataAccessCommon.HandleDBNull(row,"description", typeof(string));

            return permission;
        }

        #endregion Mapper Methods

        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Permission
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Permission permission)
        {
            MySqlParameter[] parameters = new MySqlParameter[3];
            parameters[0] = new MySqlParameter("permissionId",permission.Id);
            parameters[1] = new MySqlParameter("name",permission.Name);
            parameters[2] = new MySqlParameter("description",permission.Description);

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class PermissionMapper

} //end of namespace PnT.SongDB.Mapper
