using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for UserMapper.
    /// </summary>
    public class UserMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save User to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved User.</returns>
        public static int Save(MySqlTransaction trans, User user)
        {
            return Access.UserAccess.Save(trans, GetParameters(user));
        }

        /// <summary>
        /// Delete User by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected User.</param>
        /// <returns>
        /// True if selected User was deleted.
        /// False if selected User was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.UserAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate User by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected User.</param>
        /// <param name="inactivationReason">
        /// The reason why the user is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected User was inactivated.
        /// False if selected User was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id, string inactivationReason)
        {
            return Access.UserAccess.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all User.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of User objects.
        /// Null if no User was found.
        /// </returns>
        public static List<User> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.UserAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all coordinator User.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of coordinator User objects.
        /// Null if no coordinator User was found.
        /// </returns>
        public static List<User> FindCoordinator(MySqlTransaction trans)
        {
            DataRow[] dr = Access.UserAccess.FindCoordinator(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all User assigned to a student.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of student User objects.
        /// Null if no student User was found.
        /// </returns>
        public static List<User> FindStudent(MySqlTransaction trans)
        {
            DataRow[] dr = Access.UserAccess.FindStudent(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all User assigned to a teacher.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of teacher User objects.
        /// Null if no teacher User was found.
        /// </returns>
        public static List<User> FindTeacher(MySqlTransaction trans)
        {
            DataRow[] dr = Access.UserAccess.FindTeacher(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find User by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected User.
        /// Null if selected User was not found.
        /// </returns>
        public static User Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.UserAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find User by login.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected User.
        /// Null if selected User was not found.
        /// </returns>
        public static User Find(MySqlTransaction trans, string login)
        {
            DataRow dr = Access.UserAccess.Find(trans, login);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find User by login and password.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="login">
        /// The login of the selected user.
        /// </param>
        /// <param name="password">
        /// The password of the selected user.
        /// </param>
        /// <returns>
        /// The selected User.
        /// Null if selected User was not found.
        /// </returns>
        public static User Find(MySqlTransaction trans, string login, byte[] password)
        {
            DataRow dr = Access.UserAccess.Find(trans, login, password);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find User by login and recovery password.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="login">
        /// The login of the selected user.
        /// </param>
        /// <param name="recoveryPassword">
        /// The recovery password of the selected user.
        /// </param>
        /// <returns>
        /// The selected User.
        /// Null if selected User was not found.
        /// </returns>
        public static User FindByRecoveryPassword(MySqlTransaction trans, string login, byte[] recoveryPassword)
        {
            DataRow dr = Access.UserAccess.FindByRecoveryPassword(trans, login, recoveryPassword);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find users by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterUserStatus">
        /// The user status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterRole">
        /// The role filter.
        /// -1 to select all roles.
        /// </param>
        /// <returns>
        /// List of User objects.
        /// Null if no User was found.
        /// </returns>
        public static List<User> FindByFilter(
            MySqlTransaction trans, int filterUserStatus, int filterInstitution, int filterRole)
        {
            DataRow[] dr = Access.UserAccess.FindByFilter(
                trans, filterUserStatus, filterInstitution, filterRole);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all coordinator User that are assigned to an institution.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of User objects.
        /// Null if no User was found.
        /// </returns>
        public static List<User> FindAssignedCoordinators(MySqlTransaction trans)
        {
            DataRow[] dr = Access.UserAccess.FindAssignedCoordinators(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Set new password to user by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">
        /// The id of the selected User to whom the new password will be set.
        /// </param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>
        /// True if selected User had the password set.
        /// False if selected User was not found.
        /// </returns>
        public static bool SetPassword(MySqlTransaction trans, int id, byte[] newPassword)
        {
            return Access.UserAccess.SetPassword(trans, id, newPassword);
        }

        /// <summary>
        /// Set new recovery password to user by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">
        /// The id of the selected User to whom the new password will be set.
        /// </param>
        /// <param name="newPassword">The new recovery password.</param>
        /// <returns>
        /// True if selected User had the recovery password set.
        /// False if selected User was not found.
        /// </returns>
        public static bool SetRecoveryPassword(MySqlTransaction trans, int id, byte[] newPassword)
        {
            return Access.UserAccess.SetRecoveryPassword(trans, id, newPassword);
        }

        /// <summary>
        /// Set role to user by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">
        /// The id of the selected User to whom the role will be assigned.
        /// </param>
        /// <param name="roleId">The id of the assigned Role.</param>
        /// <returns>
        /// True if selected User had the role assigned.
        /// False if selected User was not found.
        /// </returns>
        public static bool SetRole(MySqlTransaction trans, int id, int roleId)
        {
            return Access.UserAccess.SetRole(trans, id, roleId);
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of User objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of User objects.</returns>
        private static List<User> Map(DataRow[] rows)
        {
            List<User> users = new List<User>();

            for (int i = 0; i < rows.Length; i++)
                users.Add(Map(rows[i]));

            return users;
        }

        /// <summary>
        /// Map database row to a User object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>User</returns>
        private static User Map(DataRow row)
        {
            User user = new User((int)(row["userId"]));
            user.RoleId = (int)DataAccessCommon.HandleDBNull(row,"roleId", typeof(int));
            user.InstitutionId = (int)DataAccessCommon.HandleDBNull(row,"institutionId", typeof(int));
            user.Login = (string)DataAccessCommon.HandleDBNull(row,"login", typeof(string));
            user.Password = string.Empty; //cannot reveal password
            user.RequestPasswordChange = (bool)DataAccessCommon.HandleDBNull(row, "requestPasswordChange", typeof(bool));
            user.Name = (string)DataAccessCommon.HandleDBNull(row,"name", typeof(string));
            user.Email = (string)DataAccessCommon.HandleDBNull(row,"email", typeof(string));
            user.UserStatus = (int)DataAccessCommon.HandleDBNull(row,"userStatus", typeof(int));
            user.CreationTime = (DateTime)DataAccessCommon.HandleDBNull(row,"creationTime", typeof(DateTime));
            user.InactivationTime = (DateTime)DataAccessCommon.HandleDBNull(row,"inactivationTime", typeof(DateTime));
            user.InactivationReason = (string)DataAccessCommon.HandleDBNull(row,"inactivationReason", typeof(string));

            return user;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of User
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(User user)
        {
            //get if user has password
            bool hasPassword = (user.Password != null && user.Password != string.Empty);

            //create parameters
            MySqlParameter[] parameters = hasPassword ? new MySqlParameter[12] : new MySqlParameter[11];
            parameters[0] = new MySqlParameter("userId", user.Id);
            parameters[1] = new MySqlParameter("roleId", user.RoleId);
            parameters[2] = new MySqlParameter("institutionId", DataAccessCommon.HandleDBNull(user.InstitutionId));
            parameters[3] = new MySqlParameter("login", user.Login);
            parameters[4] = new MySqlParameter("requestPasswordChange", DataAccessCommon.HandleDBNull(user.RequestPasswordChange));
            parameters[5] = new MySqlParameter("name", user.Name);
            parameters[6] = new MySqlParameter("email", user.Email);
            parameters[7] = new MySqlParameter("userStatus", user.UserStatus);
            parameters[8] = new MySqlParameter("creationTime", user.Id == -1 ? DateTime.Now : user.CreationTime);
            parameters[9] = new MySqlParameter("inactivationTime", DataAccessCommon.HandleDBNull(user.InactivationTime));
            parameters[10] = new MySqlParameter("inactivationReason", DataAccessCommon.HandleDBNull(user.InactivationReason));

            //check if password should be added
            if (hasPassword)
            {
                //add encrypted password parameter
                parameters[11] = new MySqlParameter("password", Criptography.Encrypt(user.Password));
            }


            //return gathered parameters
            return parameters;
        }

        #endregion Parameter Methods

    } //end of class UserMapper

} //end of namespace PnT.SongDB.Mapper
