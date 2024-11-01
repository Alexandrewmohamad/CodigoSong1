using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for Rolepermission.
    /// </summary>
    public class Rolepermission
    {

        #region Fields *****************************************************************

        private int rolePermissionId;
        private int roleId;
        private int permissionId;

        #endregion Fields


        #region Constructors ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Rolepermission()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="rolePermissionId">The id of the Rolepermission.</param>
        public Rolepermission(int rolePermissionId)
        {
            this.rolePermissionId = rolePermissionId;
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        public Rolepermission(
            int rolePermissionId, int roleId, int permissionId)
        {
            this.rolePermissionId = rolePermissionId;
            this.roleId = roleId;
            this.permissionId = permissionId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.rolePermissionId;}
            set{ this.rolePermissionId = value;}
        }

        public int RolePermissionId
        {
            get{ return this.rolePermissionId;}
            set{ this.rolePermissionId = value;}
        }

        public int RoleId
        {
            get{ return this.roleId;}
            set{ this.roleId = value;}
        }

        public int PermissionId
        {
            get{ return this.permissionId;}
            set{ this.permissionId = value;}
        }

        #endregion Properties


        #region Methods ****************************************************************

        /// <summary>
        /// Save Rolepermission to database.
        /// </summary>
        /// <returns>The id of the saved Rolepermission.</returns>
        public int Save()
        {
            rolePermissionId = Mapper.RolepermissionMapper.Save(null, this);
            return rolePermissionId;
        }

        /// <summary>
        /// Save Rolepermission to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Rolepermission.</returns>
        public int Save(MySqlTransaction trans)
        {
            rolePermissionId = Mapper.RolepermissionMapper.Save(trans, this);
            return rolePermissionId;
        }

        /// <summary>
        /// Delete Rolepermission by id.
        /// </summary>
        /// <param name="id">The id of the selected Rolepermission.</param>
        /// <returns>
        /// True if selected Rolepermission was deleted.
        /// False if selected Rolepermission was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.RolepermissionMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Rolepermission by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Rolepermission.</param>
        /// <returns>
        /// True if selected Rolepermission was deleted.
        /// False if selected Rolepermission was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.RolepermissionMapper.Delete(trans, id);
        }

        /// <summary>
        /// Find all Rolepermission.
        /// </summary>
        /// <returns>
        /// List of Rolepermission objects.
        /// Null if no Rolepermission was found.
        /// </returns>
        public static List<Rolepermission> Find()
        {
            return Mapper.RolepermissionMapper.Find(null);
        }

        /// <summary>
        /// Find all Rolepermission with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Rolepermission objects.
        /// Null if no Rolepermission was found.
        /// </returns>
        public static List<Rolepermission> Find(MySqlTransaction trans)
        {
            return Mapper.RolepermissionMapper.Find(trans);
        }

        /// <summary>
        /// Find Rolepermission by id.
        /// </summary>
        /// <param name="id">The id of the selected Rolepermission</param>
        /// <returns>
        /// The selected Rolepermission.
        /// Null if selected Rolepermission was not found.
        /// </returns>
        public static Rolepermission Find(int id)
        {
            return Mapper.RolepermissionMapper.Find(null, id);
        }

        /// <summary>
        /// Find Rolepermission by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Rolepermission</param>
        /// <returns>
        /// The selected Rolepermission.
        /// Null if selected Rolepermission was not found.
        /// </returns>
        public static Rolepermission Find(MySqlTransaction trans, int id)
        {
            return Mapper.RolepermissionMapper.Find(trans, id);
        }

        /// <summary>
        /// Find all Rolepermission for selected Role.
        /// </summary>
        /// <param name="roleId">The id of the selected Role.</param>
        /// <returns>
        /// List of Rolepermission objects.
        /// Null if no Rolepermission was found.
        /// </returns>
        public static List<Rolepermission> FindByRole(int roleId)
        {
            return Mapper.RolepermissionMapper.FindByRole(null, roleId);
        }

        /// <summary>
        /// Find all Rolepermission for selected Role with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="roleId">The id of the selected Role.</param>
        /// <returns>
        /// List of Rolepermission objects.
        /// Null if no Rolepermission was found.
        /// </returns>
        public static List<Rolepermission> FindByRole(MySqlTransaction trans, int roleId)
        {
            return Mapper.RolepermissionMapper.FindByRole(trans, roleId);
        }

        #endregion Methods

    } //end of class Rolepermission

} //end of namespace PnT.SongDB.Logic
