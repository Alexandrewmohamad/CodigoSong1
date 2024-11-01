using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for User.
    /// </summary>
    [DataContract]
    public class User
    {

        #region Fields *****************************************************************

        private int userId;
        private int roleId;
        private int institutionId;
        private string login;
        private string password;
        private bool requestPasswordChange;
        private string name;
        private string email;
        private int userStatus;
        private DateTime creationTime;
        private DateTime inactivationTime;
        private string inactivationReason;

        /// <summary>
        /// The name of the institution.
        /// </summary>
        private string institutionName;

        /// <summary>
        /// The name of the role.
        /// </summary>
        private string roleName;

        /// <summary>
        /// The database select result.
        /// </summary>
        private int result;

        /// <summary>
        /// The database select error message.
        /// </summary>
        private string errorMessage = null;

        #endregion Fields


        #region Constructors ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="userId">The id of the User.</param>
        public User(int userId)
        {
            this.userId = userId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.userId;}
            set{ this.userId = value;}
        }

        [DataMember]
        public int UserId
        {
            get{ return this.userId;}
            set{ this.userId = value;}
        }

        [DataMember]
        public int RoleId
        {
            get{ return this.roleId;}
            set{ this.roleId = value;}
        }

        [DataMember]
        public int InstitutionId
        {
            get{ return this.institutionId;}
            set{ this.institutionId = value;}
        }

        [DataMember]
        public string Login
        {
            get{ return this.login;}
            set{ this.login = value;}
        }

        [DataMember]
        public string Password
        {
            get{ return this.password;}
            set{ this.password = value;}
        }

        [DataMember]
        public bool RequestPasswordChange
        {
            get { return this.requestPasswordChange; }
            set { this.requestPasswordChange = value; }
        }

        [DataMember]
        public string Name
        {
            get{ return this.name;}
            set{ this.name = value;}
        }

        [DataMember]
        public string Email
        {
            get{ return this.email;}
            set{ this.email = value;}
        }

        [DataMember]
        public int UserStatus
        {
            get{ return this.userStatus;}
            set{ this.userStatus = value;}
        }

        [DataMember]
        public DateTime CreationTime
        {
            get{ return this.creationTime;}
            set{ this.creationTime = value;}
        }

        [DataMember]
        public DateTime InactivationTime
        {
            get{ return this.inactivationTime;}
            set{ this.inactivationTime = value;}
        }

        [DataMember]
        public string InactivationReason
        {
            get{ return this.inactivationReason;}
            set{ this.inactivationReason = value;}
        }

        /// <summary>
        /// Get/set the name of the institution.
        /// </summary>
        [DataMember]
        public string InstitutionName
        {
            get
            {
                return institutionName;
            }

            set
            {
                institutionName = value;
            }
        }

        /// <summary>
        /// Get/set the name of the role.
        /// </summary>
        [DataMember]
        public string RoleName
        {
            get
            {
                return roleName;
            }

            set
            {
                roleName = value;
            }
        }

        /// <summary>
        /// Get/set the database select result.
        /// </summary>
        [DataMember]
        public int Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }

        /// <summary>
        /// Get/set the database select error message.
        /// </summary>
        [DataMember]
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }

            set
            {
                errorMessage = value;
            }
        }

        #endregion Properties


        #region Methods ****************************************************************

        /// <summary>
        /// Save User to database.
        /// </summary>
        /// <returns>The id of the saved User.</returns>
        public int Save()
        {
            userId = Mapper.UserMapper.Save(null, this);
            return userId;
        }

        /// <summary>
        /// Save User to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved User.</returns>
        public int Save(MySqlTransaction trans)
        {
            userId = Mapper.UserMapper.Save(trans, this);
            return userId;
        }

        /// <summary>
        /// Delete User by id.
        /// </summary>
        /// <param name="id">The id of the selected User.</param>
        /// <returns>
        /// True if selected User was deleted.
        /// False if selected User was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.UserMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete User by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected User.</param>
        /// <returns>
        /// True if selected User was deleted.
        /// False if selected User was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.UserMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate User by id.
        /// </summary>
        /// <param name="id">The id of the selected User.</param>
        /// <param name="inactivationReason">
        /// The reason why the user is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected User was inactivated.
        /// False if selected User was not found.
        /// </returns>
        public static bool Inactivate(int id, string inactivationReason)
        {
            return Mapper.UserMapper.Inactivate(null, id, inactivationReason);
        }

        /// <summary>
        /// Inactivate User by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
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
            return Mapper.UserMapper.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all User.
        /// </summary>
        /// <returns>
        /// List of User objects.
        /// Null if no User was found.
        /// </returns>
        public static List<User> Find()
        {
            return Mapper.UserMapper.Find(null);
        }

        /// <summary>
        /// Find all User with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of User objects.
        /// Null if no User was found.
        /// </returns>
        public static List<User> Find(MySqlTransaction trans)
        {
            return Mapper.UserMapper.Find(trans);
        }

        /// <summary>
        /// Find users by filter.
        /// </summary>
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
            int filterUserStatus, int filterInstitution, int filterRole)
        {
            return Mapper.UserMapper.FindByFilter(
                null, filterUserStatus, filterInstitution, filterRole);
        }

        /// <summary>
        /// Find users by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
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
            return Mapper.UserMapper.FindByFilter(
                trans, filterUserStatus, filterInstitution, filterRole);
        }

        /// <summary>
        /// Find all coordinator User.
        /// </summary>
        /// <returns>
        /// List of coordinator User objects.
        /// Null if no coordinator User was found.
        /// </returns>
        public static List<User> FindCoordinator()
        {
            return Mapper.UserMapper.FindCoordinator(null);
        }

        /// <summary>
        /// Find all coordinator User with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of coordinator User objects.
        /// Null if no coordinator User was found.
        /// </returns>
        public static List<User> FindCoordinator(MySqlTransaction trans)
        {
            return Mapper.UserMapper.FindCoordinator(trans);
        }

        /// <summary>
        /// Find all User assigned to a student.
        /// </summary>
        /// <returns>
        /// List of student User objects.
        /// Null if no student User was found.
        /// </returns>
        public static List<User> FindStudent()
        {
            return Mapper.UserMapper.FindStudent(null);
        }

        /// <summary>
        /// Find all User assigned to a student with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of student User objects.
        /// Null if no student User was found.
        /// </returns>
        public static List<User> FindStudent(MySqlTransaction trans)
        {
            return Mapper.UserMapper.FindStudent(trans);
        }

        /// <summary>
        /// Find all User assigned to a teacher.
        /// </summary>
        /// <returns>
        /// List of teacher User objects.
        /// Null if no teacher User was found.
        /// </returns>
        public static List<User> FindTeacher()
        {
            return Mapper.UserMapper.FindTeacher(null);
        }

        /// <summary>
        /// Find all User assigned to a teacher with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of teacher User objects.
        /// Null if no teacher User was found.
        /// </returns>
        public static List<User> FindTeacher(MySqlTransaction trans)
        {
            return Mapper.UserMapper.FindTeacher(trans);
        }

        /// <summary>
        /// Find User by id.
        /// </summary>
        /// <param name="id">The id of the selected User</param>
        /// <returns>
        /// The selected User.
        /// Null if selected User was not found.
        /// </returns>
        public static User Find(int id)
        {
            return Mapper.UserMapper.Find(null, id);
        }

        /// <summary>
        /// Find User by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected User</param>
        /// <returns>
        /// The selected User.
        /// Null if selected User was not found.
        /// </returns>
        public static User Find(MySqlTransaction trans, int id)
        {
            return Mapper.UserMapper.Find(trans, id);
        }

        /// <summary>
        /// Find User by login.
        /// </summary>
        /// <param name="login">The login of the selected User.</param>
        /// <returns>
        /// The selected User.
        /// Null if selected User was not found.
        /// </returns>
        public static User Find(string login)
        {
            return Mapper.UserMapper.Find(null, login);
        }

        /// <summary>
        /// Find User by login with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="login">The login of the selected User.</param>
        /// <returns>
        /// The selected User.
        /// Null if selected User was not found.
        /// </returns>
        public static User Find(MySqlTransaction trans, string login)
        {
            return Mapper.UserMapper.Find(trans, login);
        }

        /// <summary>
        /// Find User by login and password.
        /// </summary>
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
        public static User Find(string login, byte[] password)
        {
            return Mapper.UserMapper.Find(null, login, password);
        }

        /// <summary>
        /// Find User by login and password with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
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
            return Mapper.UserMapper.Find(trans, login, password);
        }

        /// <summary>
        /// Find User by login and recovery password.
        /// </summary>
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
        public static User FindByRecoveryPassword(string login, byte[] recoveryPassword)
        {
            return Mapper.UserMapper.FindByRecoveryPassword(null, login, recoveryPassword);
        }

        /// <summary>
        /// Find User by login and recovery password with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
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
        public static User FindByRecoveryPassword(
            MySqlTransaction trans, string login, byte[] recoveryPassword)
        {
            return Mapper.UserMapper.FindByRecoveryPassword(trans, login, recoveryPassword);
        }

        /// <summary>
        /// Find all coordinator User that are assigned to an institution.
        /// </summary>
        /// <returns>
        /// List of User objects.
        /// Null if no User was found.
        /// </returns>
        public static List<User> FindAssignedCoordinators()
        {
            return Mapper.UserMapper.FindAssignedCoordinators(null);
        }

        /// <summary>
        /// Find all coordinator User that are assigned to an institution 
        /// with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of User objects.
        /// Null if no User was found.
        /// </returns>
        public static List<User> FindAssignedCoordinators(MySqlTransaction trans)
        {
            return Mapper.UserMapper.FindAssignedCoordinators(trans);
        }

        /// <summary>
        /// Get description for this user.
        /// </summary>
        /// <returns>
        /// The created description.
        /// </returns>
        public IdDescriptionStatus GetDescription()
        {
            //create and return description.
            return new IdDescriptionStatus(
                this.userId, this.login, this.userStatus);
        }

        /// <summary>
        /// Get description for this user using its name.
        /// </summary>
        /// <returns>
        /// The created description.
        /// </returns>
        public IdDescriptionStatus GetDescriptionWithName()
        {
            //create and return description.
            return new IdDescriptionStatus(
                this.userId, this.name, this.userStatus);
        }

        /// <summary>
        /// Set new password to user by id.
        /// </summary>
        /// <param name="id">
        /// The id of the selected User to whom the new password will be set.
        /// </param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>
        /// True if selected User had the password set.
        /// False if selected User was not found.
        /// </returns>
        public static bool SetPassword(int id, byte[] newPassword)
        {
            return Mapper.UserMapper.SetPassword(null, id, newPassword);
        }

        /// <summary>
        /// Set new password to user by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
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
            return Mapper.UserMapper.SetPassword(trans, id, newPassword);
        }

        /// <summary>
        /// Set new recovery password to user by id.
        /// </summary>
        /// <param name="id">
        /// The id of the selected User to whom the new password will be set.
        /// </param>
        /// <param name="newPassword">The new recovery password.</param>
        /// <returns>
        /// True if selected User had the recovery password set.
        /// False if selected User was not found.
        /// </returns>
        public static bool SetRecoveryPassword(int id, byte[] newPassword)
        {
            return Mapper.UserMapper.SetRecoveryPassword(null, id, newPassword);
        }

        /// <summary>
        /// Set new recovery password to user by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
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
            return Mapper.UserMapper.SetRecoveryPassword(trans, id, newPassword);
        }

        /// <summary>
        /// Set role to user by id.
        /// </summary>
        /// <param name="id">
        /// The id of the selected User to whom the role will be assigned.
        /// </param>
        /// <param name="roleId">The id of the assigned Role.</param>
        /// <returns>
        /// True if selected User had the role assigned.
        /// False if selected User was not found.
        /// </returns>
        public static bool SetRole(int id, int roleId)
        {
            return Mapper.UserMapper.SetRole(null, id, roleId);
        }

        /// <summary>
        /// Set role to user by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
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
            return Mapper.UserMapper.SetRole(trans, id, roleId);
        }

        #endregion Methods

    } //end of class User

} //end of namespace PnT.SongDB.Logic
