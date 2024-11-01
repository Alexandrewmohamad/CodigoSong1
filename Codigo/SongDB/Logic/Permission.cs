using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for Permission.
    /// </summary>
    [DataContract]
    public class Permission
    {

        #region Fields *****************************************************************

        private int permissionId;
        private string name;
        private string description;

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
        public Permission()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="permissionId">The id of the Permission.</param>
        public Permission(int permissionId)
        {
            this.permissionId = permissionId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.permissionId;}
            set{ this.permissionId = value;}
        }

        [DataMember]
        public int PermissionId
        {
            get{ return this.permissionId;}
            set{ this.permissionId = value;}
        }

        [DataMember]
        public string Name
        {
            get{ return this.name;}
            set{ this.name = value;}
        }

        [DataMember]
        public string Description
        {
            get{ return this.description;}
            set{ this.description = value;}
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
        /// Save Permission to database.
        /// </summary>
        /// <returns>The id of the saved Permission.</returns>
        public int Save()
        {
            permissionId = Mapper.PermissionMapper.Save(null, this);
            return permissionId;
        }

        /// <summary>
        /// Save Permission to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Permission.</returns>
        public int Save(MySqlTransaction trans)
        {
            permissionId = Mapper.PermissionMapper.Save(trans, this);
            return permissionId;
        }

        /// <summary>
        /// Delete Permission by id.
        /// </summary>
        /// <param name="id">The id of the selected Permission.</param>
        /// <returns>
        /// True if selected Permission was deleted.
        /// False if selected Permission was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.PermissionMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Permission by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Permission.</param>
        /// <returns>
        /// True if selected Permission was deleted.
        /// False if selected Permission was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.PermissionMapper.Delete(trans, id);
        }

        /// <summary>
        /// Find all Permission.
        /// </summary>
        /// <returns>
        /// List of Permission objects.
        /// Null if no Permission was found.
        /// </returns>
        public static List<Permission> Find()
        {
            return Mapper.PermissionMapper.Find(null);
        }

        /// <summary>
        /// Find all Permission with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Permission objects.
        /// Null if no Permission was found.
        /// </returns>
        public static List<Permission> Find(MySqlTransaction trans)
        {
            return Mapper.PermissionMapper.Find(trans);
        }

        /// <summary>
        /// Find Permission by id.
        /// </summary>
        /// <param name="id">The id of the selected Permission</param>
        /// <returns>
        /// The selected Permission.
        /// Null if selected Permission was not found.
        /// </returns>
        public static Permission Find(int id)
        {
            return Mapper.PermissionMapper.Find(null, id);
        }

        /// <summary>
        /// Find Permission by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Permission</param>
        /// <returns>
        /// The selected Permission.
        /// Null if selected Permission was not found.
        /// </returns>
        public static Permission Find(MySqlTransaction trans, int id)
        {
            return Mapper.PermissionMapper.Find(trans, id);
        }

        /// <summary>
        /// Find all assigned Permission for selected Role.
        /// </summary>
        /// <param name="roleId">The ID of the selected Role.</param>
        /// <returns>
        /// List of Permission objects.
        /// Null if no Permission was found.
        /// </returns>
        public static List<Permission> FindByRole(int roleId)
        {
            return Mapper.PermissionMapper.FindByRole(null, roleId);
        }

        /// <summary>
        /// Find all assigned Permission for selected Role with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="roleId">The ID of the selected Role.</param>
        /// <returns>
        /// List of Permission objects.
        /// Null if no Permission was found.
        /// </returns>
        public static List<Permission> FindByRole(MySqlTransaction trans, int roleId)
        {
            return Mapper.PermissionMapper.FindByRole(trans, roleId);
        }

        /// <summary>
        /// Get description for this instrument.
        /// </summary>
        /// <returns>
        /// The created description.
        /// </returns>
        public IdDescriptionStatus GetDescription()
        {
            //create and return description.
            return new IdDescriptionStatus(
                this.PermissionId, this.name, 0);
        }

        #endregion Methods

    } //end of class Permission

} //end of namespace PnT.SongDB.Logic
