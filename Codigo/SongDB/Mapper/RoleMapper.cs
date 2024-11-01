using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for RoleMapper.
    /// </summary>
    public class RoleMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Role to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Role.</returns>
        public static int Save(MySqlTransaction trans, Role role)
        {
            return Access.RoleAccess.Save(trans, GetParameters(role));
        }

        /// <summary>
        /// Delete Role by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Role.</param>
        /// <returns>
        /// True if selected Role was deleted.
        /// False if selected Role was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.RoleAccess.Delete(trans, id);
        }

        /// <summary>
        /// Find all Role.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Role objects.
        /// Null if no Role was found.
        /// </returns>
        public static List<Role> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.RoleAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Role by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Role.
        /// Null if selected Role was not found.
        /// </returns>
        public static Role Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.RoleAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Role objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Role objects.</returns>
        private static List<Role> Map(DataRow[] rows)
        {
            List<Role> roles = new List<Role>();

            for (int i = 0; i < rows.Length; i++)
                roles.Add(Map(rows[i]));

            return roles;
        }

        /// <summary>
        /// Map database row to a Role object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Role</returns>
        private static Role Map(DataRow row)
        {
            Role role = new Role((int)(row["roleId"]));
            role.Name = (string)DataAccessCommon.HandleDBNull(row, "name", typeof(string));
            role.Description = (string)DataAccessCommon.HandleDBNull(row, "description", typeof(string));
            role.CreationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "creationTime", typeof(DateTime));

            return role;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Role
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Role role)
        {
            MySqlParameter[] parameters = new MySqlParameter[4];
            parameters[0] = new MySqlParameter("roleId", role.Id);
            parameters[1] = new MySqlParameter("name", role.Name);
            parameters[2] = new MySqlParameter("description", role.Description);
            parameters[3] = new MySqlParameter("creationTime", role.Id == -1 ? DateTime.Now : role.CreationTime);

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class RoleMapper

} //end of namespace PnT.SongDB.Mapper
