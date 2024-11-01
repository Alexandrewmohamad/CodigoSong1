using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for Role.
    /// </summary>
    [DataContract]
    public class Role
    {

        #region Fields *****************************************************************

        private int roleId;
        private string name;
        private string description;
        private DateTime creationTime;

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
        public Role()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="roleId">The id of the Role.</param>
        public Role(int roleId)
        {
            this.roleId = roleId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.roleId;}
            set{ this.roleId = value;}
        }

        [DataMember]
        public int RoleId
        {
            get{ return this.roleId;}
            set{ this.roleId = value;}
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

        [DataMember]
        public DateTime CreationTime
        {
            get { return this.creationTime; }
            set { this.creationTime = value; }
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
        /// Save Role to database.
        /// </summary>
        /// <returns>The id of the saved Role.</returns>
        public int Save()
        {
            roleId = Mapper.RoleMapper.Save(null, this);
            return roleId;
        }

        /// <summary>
        /// Save Role to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Role.</returns>
        public int Save(MySqlTransaction trans)
        {
            roleId = Mapper.RoleMapper.Save(trans, this);
            return roleId;
        }

        /// <summary>
        /// Delete Role by id.
        /// </summary>
        /// <param name="id">The id of the selected Role.</param>
        /// <returns>
        /// True if selected Role was deleted.
        /// False if selected Role was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.RoleMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Role by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Role.</param>
        /// <returns>
        /// True if selected Role was deleted.
        /// False if selected Role was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.RoleMapper.Delete(trans, id);
        }

        /// <summary>
        /// Find all Role.
        /// </summary>
        /// <returns>
        /// List of Role objects.
        /// Null if no Role was found.
        /// </returns>
        public static List<Role> Find()
        {
            return Mapper.RoleMapper.Find(null);
        }

        /// <summary>
        /// Find all Role with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Role objects.
        /// Null if no Role was found.
        /// </returns>
        public static List<Role> Find(MySqlTransaction trans)
        {
            return Mapper.RoleMapper.Find(trans);
        }

        /// <summary>
        /// Find Role by id.
        /// </summary>
        /// <param name="id">The id of the selected Role</param>
        /// <returns>
        /// The selected Role.
        /// Null if selected Role was not found.
        /// </returns>
        public static Role Find(int id)
        {
            return Mapper.RoleMapper.Find(null, id);
        }

        /// <summary>
        /// Find Role by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Role</param>
        /// <returns>
        /// The selected Role.
        /// Null if selected Role was not found.
        /// </returns>
        public static Role Find(MySqlTransaction trans, int id)
        {
            return Mapper.RoleMapper.Find(trans, id);
        }

        /// <summary>
        /// Get description for this role.
        /// </summary>
        /// <returns>
        /// The created description.
        /// </returns>
        public IdDescriptionStatus GetDescription()
        {
            //create and return description.
            return new IdDescriptionStatus(
                this.roleId, this.name, (int)ItemStatus.Active);
        }

        #endregion Methods

    } //end of class Role

} //end of namespace PnT.SongDB.Logic
