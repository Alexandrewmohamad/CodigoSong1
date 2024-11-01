using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetroFramework;

using PnT.SongDB.Logic;
using PnT.SongServer;

using PnT.SongClient.Logic;


namespace PnT.SongClient.UI.Controls
{

    /// <summary>
    /// This control is used to manage role registries in web service.
    /// Inherits from base register control.
    /// </summary>
    public partial class RegisterRoleControl : RegisterBaseControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The list of all permissions.
        /// </summary>
        private List<Permission> allPermissions = null;

        /// <summary>
        /// The list of permissions currently saved in database.
        /// </summary>
        private List<Permission> databasePermissions = null;

        /// <summary>
        /// The list of available permissions.
        /// </summary>
        private List<Permission> availablePermissions = null;

        /// <summary>
        /// The list of role selected permissions.
        /// </summary>
        private List<Permission> selectedPermissions = null;

        /// <summary>
        /// The list of all users.
        /// </summary>
        private List<IdDescriptionStatus> allUsers = null;

        /// <summary>
        /// The list of role users currently saved in database.
        /// </summary>
        private List<IdDescriptionStatus> databaseUsers = null;

        /// <summary>
        /// The list of available users.
        /// </summary>
        private List<IdDescriptionStatus> availableUsers = null;

        /// <summary>
        /// The list of role selected users.
        /// </summary>
        private List<IdDescriptionStatus> selectedUsers = null;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RegisterRoleControl() : base("Role", Manager.Settings.HideInactiveRoles)
        {
            //init UI components
            InitializeComponent();

            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //hide copy button
            this.displayCopyButton = true;

            //role cannot be deleted
            this.classHasDeletion = true;

            //role does not have status
            this.classHasStatus = false;

            //set permissions
            this.allowAddItem = Manager.HasLogonPermission("Role.Insert");
            this.allowEditItem = Manager.HasLogonPermission("Role.Edit");
            this.allowDeleteItem = Manager.HasLogonPermission("Role.Delete");

            //load all permissions
            LoadPermissions();

            //load all users
            LoadUsers();
        }

        #endregion Constructors


        #region Properties ************************************************************

        #endregion Properties


        #region Private Methods *******************************************************

        /// <summary>
        /// Load all permissions.
        /// </summary>
        private void LoadPermissions()
        {
            //set default empty list
            allPermissions = new List<Permission>();

            //load permissions
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //exit
                return;
            }

            try
            {
                //get list of permissions
                List<Permission> permissions = songChannel.FindPermissions();

                //check result
                if (permissions[0].Result == (int)SelectResult.Success)
                {
                    //sort permissions by description
                    permissions.Sort((x, y) => x.Description.CompareTo(y.Description));

                    //set permissions
                    allPermissions = permissions;
                }
                else if (permissions[0].Result == (int)SelectResult.Empty)
                {
                    //no permission is available
                    //should never happen
                    //exit
                    return;
                }
                else if (permissions[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting permissions
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Permission, permissions[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Permission,
                        permissions[0].ErrorMessage));

                    //could not get permissions
                    //exit
                    return;
                }
            }
            catch (Exception ex)
            {
                //database error while getting permissions
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Permission), ex);

                //log exception
                Manager.Log.WriteException(ex);
            }
            finally
            {
                //check channel
                if (songChannel != null)
                {
                    //close channel
                    ((System.ServiceModel.IClientChannel)songChannel).Close();
                }
            }
        }

        /// <summary>
        /// Load all users.
        /// </summary>
        private void LoadUsers()
        {
            //set default empty list
            allUsers = new List<IdDescriptionStatus>();

            //load users
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //exit
                return;
            }

            try
            {
                //get list of active users
                List<IdDescriptionStatus> users =
                    songChannel.ListUsersByStatus((int)ItemStatus.Active);

                //check result
                if (users[0].Result == (int)SelectResult.Success)
                {
                    //sort users by description
                    users.Sort((x, y) => x.Description.CompareTo(y.Description));

                    //set users
                    allUsers = users;
                }
                else if (users[0].Result == (int)SelectResult.Empty)
                {
                    //no user is available
                    //should never happen
                    //exit
                    return;
                }
                else if (users[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting users
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_User, users[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_User,
                        users[0].ErrorMessage));

                    //could not get users
                    //exit
                    return;
                }
            }
            catch (Exception ex)
            {
                //database error while getting users
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_User), ex);

                //log exception
                Manager.Log.WriteException(ex);
            }
            finally
            {
                //check channel
                if (songChannel != null)
                {
                    //close channel
                    ((System.ServiceModel.IClientChannel)songChannel).Close();
                }
            }
        }

        /// <summary>
        /// Refresh displayed permission lists and its items.
        /// </summary>
        private void RefreshPermissionLists()
        {
            //refresh displayed list of selected permissions
            lbSelectedPermissions.DataSource = null;
            lbSelectedPermissions.DisplayMember = "Description";
            lbSelectedPermissions.ValueMember = "PermissionId";
            lbSelectedPermissions.DataSource = selectedPermissions;
            lbSelectedPermissions.SelectedIndex = -1;

            //refresh displayed list of available permissions
            lbAvailablePermissions.DataSource = null;
            lbAvailablePermissions.DisplayMember = "Description";
            lbAvailablePermissions.ValueMember = "PermissionId";
            lbAvailablePermissions.DataSource = availablePermissions;
            lbAvailablePermissions.SelectedIndex = -1;
        }

        /// <summary>
        /// Refresh displayed user lists and its items.
        /// </summary>
        private void RefreshUserLists()
        {
            //refresh displayed list of selected users
            lbSelectedUsers.DataSource = null;
            lbSelectedUsers.DisplayMember = "Description";
            lbSelectedUsers.ValueMember = "Id";
            lbSelectedUsers.DataSource = selectedUsers;
            lbSelectedUsers.SelectedIndex = -1;

            //refresh displayed list of available users
            lbAvailableUsers.DataSource = null;
            lbAvailableUsers.DisplayMember = "Description";
            lbAvailableUsers.ValueMember = "Id";
            lbAvailableUsers.DataSource = availableUsers;
            lbAvailableUsers.SelectedIndex = -1;
        }
        
        /// <summary>
        /// Validate input data.
        /// </summary>
        /// <returns>
        /// True if data is valid.
        /// </returns>
        private bool ValidateData()
        {
            //validate name
            if (!ValidateRequiredField(mtxtName, mlblName.Text, null, null) ||
                !ValidateDescriptionField(mtxtName, mlblName.Text, null, null))
            {
                //invalid field
                return false;
            }

            //validate description
            if (!ValidateRequiredField(mtxtDescription, mlblDescription.Text, null, null))
            {
                //invalid field
                return false;
            }

            //data is valid
            return true;
        }

        #endregion Private Methods


        #region Base Class Overriden Methods ******************************************

        /// <summary>
        /// Clear value for all UI fields.
        /// </summary>
        public override void ClearFields()
        {
            //clear text fields
            mtxtName.Text = string.Empty;
            mtxtDescription.Text = string.Empty;

            //clear lists
            lbAvailableUsers.DataSource = null;
            lbSelectedUsers.DataSource = null;

            //display permissions
            availablePermissions = new List<Permission>(allPermissions);
            selectedPermissions = new List<Permission>();
            RefreshPermissionLists();

            //display users
            availableUsers = new List<IdDescriptionStatus>(allUsers);
            selectedUsers = new List<IdDescriptionStatus>();
            RefreshUserLists();
        }

        /// <summary>
        /// Dispose used resources from user control.
        /// </summary>
        public override void DisposeControl()
        {
            //update option to hide inactive items
            Manager.Settings.HideInactiveRoles = this.hideInactiveItems;
        }

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        /// <returns></returns>
        public override string SelectMenuOption()
        {
            //select role option
            return "Role";
        }

        /// <summary>
        /// Enable all UI fields for edition.
        /// </summary>
        /// <param name="enable">True to enable fields. False to disable them.</param>
        public override void EnableFields(bool enable)
        {
            //set text fields
            mtxtName.Enabled = enable;
            mtxtDescription.Enabled = enable;
            
            ////set lists
            //lbAvailableUsers.Enabled = enable;
            //lbSelectedUsers.Enabled = enable;
            //lbAvailablePermissions.Enabled = enable;
            //lbSelectedPermissions.Enabled = enable;

            //set buttons
            mbtnAddPermissions.Enabled = enable;
            mbtnAddAllPermissions.Enabled = enable;
            mbtnRemovePermissions.Enabled = enable;
            mbtnRemoveAllPermissions.Enabled = enable;
            mbtnAddUsers.Enabled = enable;
            mbtnRemoveUsers.Enabled = enable;

            //call list event handlers and so setting buttons
            lbAvailablePermissions_SelectedIndexChanged(this, new EventArgs());
            lbSelectedPermissions_SelectedIndexChanged(this, new EventArgs());
            lbAvailableUsers_SelectedIndexChanged(this, new EventArgs());
            lbSelectedUsers_SelectedIndexChanged(this, new EventArgs());
        }

        /// <summary>
        /// Load data for selected item.
        /// </summary>
        /// <param name="itemId">the id of the selected item.</param>
        public override bool LoadItemData(int itemId)
        {
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not load data
                return false;
            }

            try
            {
                //get selected role from web service
                Role role = songChannel.FindRole(itemId);

                //check result
                if (role.Result == (int)SelectResult.Empty)
                {
                    //should never happen
                    //could not load data
                    return false;
                }
                else if (role.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting role
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, role.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, role.ErrorMessage));

                    //could not load data
                    return false;
                }

                //set selected role ID
                selectedId = role.RoleId;

                //set text fields
                mtxtName.Text = role.Name;
                mtxtDescription.Text = role.Description;

                #region load permissions

                //get assigned permissions for selected role
                List<Permission> permissions = songChannel.FindPermissionsByRole(role.RoleId);

                //check result
                if (permissions[0].Result == (int)SelectResult.Empty)
                {
                    //role has no permission
                    //clear list
                    permissions.Clear();
                }
                else if (permissions[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting permission
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadPermissions,
                        itemTypeDescription, role.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadPermissions,
                        itemTypeDescription, role.ErrorMessage));

                    //could not load data
                    return false;
                }

                //sort permissions by description
                permissions.Sort((x, y) => x.Description.CompareTo(y.Description));

                //copy list and set database permissions
                databasePermissions = new List<Permission>(permissions);

                //set selected permissions
                selectedPermissions = permissions;
                
                //gather list of available permissions
                availablePermissions = new List<Permission>();

                //check each permission
                foreach (Permission permission in allPermissions)
                {
                    //check if permission is not selected
                    if (selectedPermissions.Find(p => p.PermissionId == permission.PermissionId) == null)
                    {
                        //add permission
                        availablePermissions.Add(permission);
                    }
                }

                //refresh displayed permission lists
                RefreshPermissionLists();

                #endregion load permissions

                #region load users

                //get assigned users for selected role
                List<IdDescriptionStatus> users = songChannel.ListUsersByRole(role.RoleId);

                //check result
                if (users[0].Result == (int)SelectResult.Empty)
                {
                    //role has no user
                    //clear list
                    users.Clear();
                }
                else if (users[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting users
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadUsers,
                        itemTypeDescription, users[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadUsers,
                        itemTypeDescription, users[0].ErrorMessage));

                    //could not load data
                    return false;
                }

                //sort users by description
                users.Sort((x, y) => x.Description.CompareTo(y.Description));

                //copy list and set database users
                databaseUsers = new List<IdDescriptionStatus>(users);

                //set selected users
                selectedUsers = users;

                //gather list of available users
                availableUsers = new List<IdDescriptionStatus>();

                //check each user
                foreach (IdDescriptionStatus user in allUsers)
                {
                    //check if user is not selected
                    if (selectedUsers.Find(p => p.Id == user.Id) == null)
                    {
                        //add user
                        availableUsers.Add(user);
                    }
                }

                //refresh displayed user lists
                RefreshUserLists();

                #endregion load users
            }
            finally
            {
                //check channel
                if (songChannel != null)
                {
                    //close channel
                    ((System.ServiceModel.IClientChannel)songChannel).Close();
                }
            }

            //data were loaded
            return true;
        }

        /// <summary>
        /// Load role list from database.
        /// </summary>
        /// <returns></returns>
        protected override List<IdDescriptionStatus> LoadItemsFromDatabase()
        {
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                return null;
            }

            try
            {
                //get list of roles;
                List<IdDescriptionStatus> roles = songChannel.ListRoles();

                //check result
                if (roles[0].Result == (int)SelectResult.Success)
                {
                    //roles were found
                    return roles;
                }
                else if (roles[0].Result == (int)SelectResult.Empty)
                {
                    //no role is available
                    return null;
                }
                else if (roles[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting roles
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        itemTypeDescription, roles[0].ErrorMessage));

                    //could not get roles
                    return null;
                }
            }
            finally
            {
                //check channel
                if (songChannel != null)
                {
                    //close channel
                    ((System.ServiceModel.IClientChannel)songChannel).Close();
                }
            }

            //could not get roles
            return null;
        }

        /// <summary>
        /// Delete current selected item.
        /// </summary>
        public override bool DeleteItem()
        {
            //check if selected role has any assigned user
            if (lbSelectedUsers.Items.Count > 0)
            {
                //cannot delete role
                //display message
                MetroMessageBox.Show(Manager.MainForm,
                    Properties.Resources.msgCannotDeleteRoleWithUser,
                    Properties.Resources.wordError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //exit
                return false;
            }

            //get song channel
                ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not delete item
                return false;
            }

            try
            {
                //inactivate selected role and get result
                DeleteResult result = songChannel.DeleteRole(SelectedItemId);

                //check result
                if (result.Result == (int)SelectResult.Success)
                {
                    //register was inactivated
                    return true;
                }
                else if (result.Result == (int)SelectResult.FatalError)
                {
                    //error while inactivating item
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceDeleteItem,
                        itemTypeDescription, result.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceDeleteItem,
                        itemTypeDescription, result.ErrorMessage));
                }
                else if (result.Result == (int)SelectResult.FatalError)
                {
                    //should never happen
                }
            }
            finally
            {
                //check channel
                if (songChannel != null)
                {
                    //close channel
                    ((System.ServiceModel.IClientChannel)songChannel).Close();
                }
            }

            //could not inactivate item
            return false;
        }

        /// <summary>
        /// Start editing current selected item.
        /// </summary>
        public override void EditItem()
        {
        }

        /// <summary>
        /// Start creating a new item from scratch.
        /// </summary>
        public override void CreateItem()
        {
            //focus project name field
            mtxtName.Focus();
        }

        /// <summary>
        /// Save the data of the current edited item.
        /// </summary>
        /// <returns>
        /// The updated description of the saved item.
        /// Null if item could not be saved.
        /// </returns>
        public override IdDescriptionStatus SaveItem()
        {
            //validate field controls and check result
            if (!this.ValidateData())
            {
                //data is not valid
                //cannot save institution
                return null;
            }

            //create an role and set data
            Role role = new Role();

            //set selected role ID
            role.RoleId = selectedId;

            //set text fields
            role.Name = mtxtName.Text.Trim();
            role.Description = mtxtDescription.Text.Trim();

            //gather list of selected permission ids
            List<int> selectedPermissionIds = new List<int>();

            //check each selected permission
            foreach (Permission permission in selectedPermissions)
            {
                //add permission id
                selectedPermissionIds.Add(permission.PermissionId);
            }

            //gather list of added user ids
            List<int> addedUserIds = new List<int>();

            //check each selected user
            foreach (IdDescriptionStatus user in selectedUsers)
            {
                //check if user is not in database yet
                if (!databaseUsers.Contains(user))
                {
                    //add user id
                    addedUserIds.Add(user.Id);
                }
            }

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not save role
                return null;
            }

            try
            {
                //save role and get result
                SaveResult saveResult = songChannel.SaveRole(
                    role, selectedPermissionIds, addedUserIds);

                //check result
                if (saveResult.Result == (int)SelectResult.FatalError)
                {
                    //role was not saved
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceSaveItem,
                        itemTypeDescription, saveResult.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceSaveItem,
                        itemTypeDescription, saveResult.ErrorMessage));

                    //could not save role
                    return null;
                }

                //set saved ID to role ID
                role.RoleId = saveResult.SavedId;

                //set selected ID
                selectedId = saveResult.SavedId;

                //check if there is a parent control
                //check if it is an user register control
                if (parentControl != null && parentControl is ViewUserControl)
                {
                    //update role in parent control
                    ((ViewUserControl)parentControl).UpdateRole(role);
                }
            }
            finally
            {
                //check channel
                if (songChannel != null)
                {
                    //close channel
                    ((System.ServiceModel.IClientChannel)songChannel).Close();
                }
            }

            //role was saved
            //return updated description
            return role.GetDescription();
        }

        /// <summary>
        /// Copy current selected item.
        /// </summary>
        /// <returns>
        /// The description of the copied item.
        /// Null if item could not be copied.
        /// </returns>
        public override IdDescriptionStatus CopyItem()
        {
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not copy role
                return null;
            }

            try
            {
                //copy selected role and get result
                Role role = songChannel.CopyRole(SelectedItemId);

                //check role copy
                if (role.Result == (int)SelectResult.FatalError)
                {
                    //role was not copied
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceCopyItem,
                        itemTypeDescription, role.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceCopyItem,
                        itemTypeDescription, role.ErrorMessage));

                    //could not copy role
                    return null;
                }

                //role was copied
                return role.GetDescription();
            }
            finally
            {
                //check channel
                if (songChannel != null)
                {
                    //close channel
                    ((System.ServiceModel.IClientChannel)songChannel).Close();
                }
            }
        }

        /// <summary>
        /// Cancel changes from current edited item.
        /// </summary>
        public override void CancelChanges()
        {
        }

        #endregion Base Class Overriden Methods


        #region UI Event Handlers *****************************************************

        /// <summary>
        /// Register insitution 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterRole_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set font to UI controls
            lbAvailableUsers.Font = MetroFramework.MetroFonts.DefaultLight(12.0F);
            lbSelectedUsers.Font = MetroFramework.MetroFonts.DefaultLight(12.0F);
        }

        /// <summary>
        /// Available users listbox selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbAvailableUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if status is consulting and user has selected an item
            if (this.Status == RegisterStatus.Consulting && lbAvailableUsers.SelectedIndex >= 0)
            {
                //cannot select while consulting
                //clear selection
                lbAvailableUsers.SelectedIndex = -1;

                //exit
                return;
            }

            //check if is not consulting and the number of selected available users
                if (this.Status != RegisterStatus.Consulting && lbAvailableUsers.SelectedIndex >= 0)
            {
                //enable button
                mbtnAddUsers.Enabled = true;
                mbtnAddUsers.BackgroundImage = Properties.Resources.IconMoveRightOne;
            }
            else
            {
                //disable button
                mbtnAddUsers.Enabled = false;
                mbtnAddUsers.BackgroundImage = Properties.Resources.IconMoveRightOneDisabled;
            }
        }
        
        /// <summary>
        /// Selected users listbox selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbSelectedUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if status is consulting and user has selected an item
            if (this.Status == RegisterStatus.Consulting && lbSelectedUsers.SelectedIndex >= 0)
            {
                //cannot select while consulting
                //clear selection
                lbSelectedUsers.SelectedIndex = -1;

                //exit
                return;
            }

            //get selected item
            IdDescriptionStatus selectedUser = lbSelectedUsers.SelectedIndex > -1 ?
                (IdDescriptionStatus)lbSelectedUsers.SelectedItem : null;

            //check if is not consulting and if selected user is not saved in database
            if (this.Status != RegisterStatus.Consulting && 
                selectedUser != null && !databaseUsers.Contains(selectedUser))
            {
                //enable button
                mbtnRemoveUsers.Enabled = true;
                mbtnRemoveUsers.BackgroundImage = Properties.Resources.IconMoveLeftOne;
            }
            else
            {
                //disable button
                mbtnRemoveUsers.Enabled = false;
                mbtnRemoveUsers.BackgroundImage = Properties.Resources.IconMoveLeftOneDisabled;
            }
        }

        /// <summary>
        /// Available permissions listbox selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbAvailablePermissions_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if status is consulting and user has selected an item
            if (this.Status == RegisterStatus.Consulting && lbAvailablePermissions.SelectedIndex >= 0)
            {
                //cannot select while consulting
                //clear selection
                lbAvailablePermissions.SelectedIndex = -1;

                //exit
                return;
            }

            //check if is not consulting and the number of selected available permissions
            if (this.Status != RegisterStatus.Consulting && lbAvailablePermissions.SelectedIndex >= 0)
            {
                //enable button
                mbtnAddPermissions.Enabled = true;
                mbtnAddPermissions.BackgroundImage = Properties.Resources.IconMoveRightOne;
            }
            else
            {
                //disable button
                mbtnAddPermissions.Enabled = false;
                mbtnAddPermissions.BackgroundImage = Properties.Resources.IconMoveRightOneDisabled;
            }

            //check if is not consulting and the number of available permissions
            if (this.Status != RegisterStatus.Consulting && lbAvailablePermissions.Items.Count > 0)
            {
                //enable button
                mbtnAddAllPermissions.Enabled = true;
                mbtnAddAllPermissions.BackgroundImage = Properties.Resources.IconMoveRightAll;
            }
            else
            {
                //disable button
                mbtnAddAllPermissions.Enabled = false;
                mbtnAddAllPermissions.BackgroundImage = Properties.Resources.IconMoveRightAllDisabled;
            }
        }

        /// <summary>
        /// Selected permissions listbox selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbSelectedPermissions_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if status is consulting and user has selected an item
            if (this.Status == RegisterStatus.Consulting && lbSelectedPermissions.SelectedIndex >= 0)
            {
                //cannot select while consulting
                //clear selection
                lbSelectedPermissions.SelectedIndex = -1;

                //exit
                return;
            }

            //check if is not consulting and the number of selected selected permissions
            if (this.Status != RegisterStatus.Consulting && lbSelectedPermissions.SelectedIndex >= 0)
            {
                //enable button
                mbtnRemovePermissions.Enabled = true;
                mbtnRemovePermissions.BackgroundImage = Properties.Resources.IconMoveLeftOne;
            }
            else
            {
                //disable button
                mbtnRemovePermissions.Enabled = false;
                mbtnRemovePermissions.BackgroundImage = Properties.Resources.IconMoveLeftOneDisabled;
            }

            //check if is not consulting and the number of available permissions
            if (this.Status != RegisterStatus.Consulting && lbSelectedPermissions.Items.Count > 0)
            {
                //enable button
                mbtnRemoveAllPermissions.Enabled = true;
                mbtnRemoveAllPermissions.BackgroundImage = Properties.Resources.IconMoveLeftAll;
            }
            else
            {
                //disable button
                mbtnRemoveAllPermissions.Enabled = false;
                mbtnRemoveAllPermissions.BackgroundImage = Properties.Resources.IconMoveLeftAllDisabled;
            }
        }

        /// <summary>
        /// General permission listbox draw item event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbPermissions_DrawItem(object sender, DrawItemEventArgs e)
        {
            //check if it is on design mode
            if (this.DesignMode == true)
            {
                //exit
                return;
            }

            //get sender listbox
            ListBox senderList = (ListBox)sender;

            //check index and number of items
            if (e.Index < 0 || senderList.Items.Count <= e.Index)
            {
                //exit
                return;
            }

            //get item
            Permission permission = (Permission)senderList.Items[e.Index];

            //draw item
            e.DrawBackground();
            e.Graphics.DrawString(permission.Description, e.Font, 
                databasePermissions != null && databasePermissions.Contains(permission) ? Brushes.Black : Brushes.DarkCyan, 
                e.Bounds.Left, e.Bounds.Top);
            e.DrawFocusRectangle();
        }

        /// <summary>
        /// General user listbox draw item event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbUsers_DrawItem(object sender, DrawItemEventArgs e)
        {
            //check if it is on design mode
            if (this.DesignMode == true)
            {
                //exit
                return;
            }

            //get sender listbox
            ListBox senderList = (ListBox)sender;

            //check index and number of items
            if (e.Index < 0 || senderList.Items.Count <= e.Index)
            {
                //exit
                return;
            }

            //get user item
            IdDescriptionStatus user = (IdDescriptionStatus)senderList.Items[e.Index];

            //draw item
            e.DrawBackground();
            e.Graphics.DrawString(user.Description, e.Font,
                databaseUsers != null && databaseUsers.Contains(user) ? Brushes.Black : Brushes.DarkCyan, 
                e.Bounds.Left, e.Bounds.Top);
            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Button add permissions click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddPermissions_Click(object sender, EventArgs e)
        {
            //check if there is any selected available permission
            if (lbAvailablePermissions.SelectedItems.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbAvailablePermissions_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected available permission
            for (int i = 0; i < lbAvailablePermissions.SelectedItems.Count; i++)
            {
                //get selected permission
                Permission permission = (Permission)lbAvailablePermissions.SelectedItems[i];

                //remove permission from available permissions
                availablePermissions.Remove(permission);

                //add permission to selected permissions and sort list
                selectedPermissions.Add(permission);
            }

            //sort selected permission list
            selectedPermissions.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed permission lists
            RefreshPermissionLists();
        }

        /// <summary>
        /// Button add all permissions click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddAllPermissions_Click(object sender, EventArgs e)
        {
            //check if there is any available permission
            if (lbAvailablePermissions.Items.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbAvailablePermissions_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each available permission
            for (int i = 0; i < lbAvailablePermissions.Items.Count; i++)
            {
                //get available permission
                Permission permission = (Permission)lbAvailablePermissions.Items[i];

                //remove permission from available permissions
                availablePermissions.Remove(permission);

                //add permission to selected permissions and sort list
                selectedPermissions.Add(permission);
            }

            //sort selected permission list
            selectedPermissions.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed permission lists
            RefreshPermissionLists();
        }

        /// <summary>
        /// Button remove permissions click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnRemovePermissions_Click(object sender, EventArgs e)
        {
            //check if there is any selected selected permission
            if (lbSelectedPermissions.SelectedItems.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedPermissions_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected selected permission
            for (int i = 0; i < lbSelectedPermissions.SelectedItems.Count; i++)
            {
                //get selected permission
                Permission permission = (Permission)lbSelectedPermissions.SelectedItems[i];

                //remove permission from selected permissions
                selectedPermissions.Remove(permission);

                //add permission to available permissions and sort list
                availablePermissions.Add(permission);
            }

            //sort available permission list
            availablePermissions.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed permission lists
            RefreshPermissionLists();
        }

        /// <summary>
        /// Button remove all permissions click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnRemoveAllPermissions_Click(object sender, EventArgs e)
        {
            //check if there is any selected permission
            if (lbSelectedPermissions.Items.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedPermissions_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected permission
            for (int i = 0; i < lbSelectedPermissions.Items.Count; i++)
            {
                //get selected permission
                Permission permission = (Permission)lbSelectedPermissions.Items[i];

                //remove permission from selected permissions
                selectedPermissions.Remove(permission);

                //add permission to available permissions and sort list
                availablePermissions.Add(permission);
            }

            //sort available permission list
            availablePermissions.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed permission lists
            RefreshPermissionLists();
        }

        /// <summary>
        /// Button add users click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddUsers_Click(object sender, EventArgs e)
        {
            //check if there is any selected available user
            if (lbAvailableUsers.SelectedItems.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbAvailableUsers_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected available user
            for (int i = 0; i < lbAvailableUsers.SelectedItems.Count; i++)
            {
                //get selected user
                IdDescriptionStatus user = (IdDescriptionStatus)lbAvailableUsers.SelectedItems[i];

                //remove user from available users
                availableUsers.Remove(user);

                //add user to selected users and sort list
                selectedUsers.Add(user);
            }

            //sort selected user list
            selectedUsers.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed user lists
            RefreshUserLists();
        }

        /// <summary>
        /// Button remove users click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnRemoveUsers_Click(object sender, EventArgs e)
        {
            //check if there is any selected selected user
            if (lbSelectedUsers.SelectedItems.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedUsers_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected selected user
            for (int i = 0; i < lbSelectedUsers.SelectedItems.Count; i++)
            {
                //get selected user
                IdDescriptionStatus user = (IdDescriptionStatus)lbSelectedUsers.SelectedItems[i];

                //remove user from selected users
                selectedUsers.Remove(user);

                //add user to available users and sort list
                availableUsers.Add(user);
            }

            //sort available user list
            availableUsers.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed user lists
            RefreshUserLists();
        }

        #endregion UI Event Handlers

    } //end of class RegisterRoleControl

} //end of namespace PnT.SongClient.UI.Controls
