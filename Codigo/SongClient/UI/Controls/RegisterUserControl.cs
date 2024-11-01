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
    /// This control is used to manage user registries in web service.
    /// Inherits from base register control.
    /// </summary>
    public partial class RegisterUserControl : RegisterBaseControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The inactivation reason of the selected item.
        /// </summary>
        private string inactivationReason = string.Empty;

        /// <summary>
        /// The inactivation time of the selected item.
        /// </summary>
        private DateTime inactivationTime = DateTime.MinValue;

        #endregion Fields


        #region Constructors **********************************************************


        public RegisterUserControl() : base("User", Manager.Settings.HideInactiveUsers)
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
            this.displayCopyButton = false;

            //user cannot be deleted
            this.classHasDeletion = false;

            //set permissions
            this.allowAddItem = Manager.HasLogonPermission("User.Insert");
            this.allowEditItem = Manager.HasLogonPermission("User.Edit");
            this.allowDeleteItem = Manager.HasLogonPermission("User.Inactivate");

            //load combos
            //list statuses
            List<KeyValuePair<int, string>> statuses = new List<KeyValuePair<int, string>>();
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Active, Properties.Resources.ResourceManager.GetString("ItemStatus_Active")));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Inactive, Properties.Resources.ResourceManager.GetString("ItemStatus_Inactive")));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Blocked, Properties.Resources.ResourceManager.GetString("ItemStatus_Blocked")));
            mcbStatus.ValueMember = "Key";
            mcbStatus.DisplayMember = "Value";
            mcbStatus.DataSource = statuses;

            //list roles
            ListRoles();

            //list institutions
            ListInstitutions();
        }

        #endregion Constructors


        #region Properties ************************************************************

        #endregion Properties


        #region Private Methods *******************************************************

        /// <summary>
        /// List institutions into UI.
        /// </summary>
        private void ListInstitutions()
        {
            //create none option
            IdDescriptionStatus noneOption = new IdDescriptionStatus(
                int.MinValue, Properties.Resources.wordNone, 0);

            //create default empty list with just none option
            List<IdDescriptionStatus> emptyList = new List<IdDescriptionStatus>();
            emptyList.Add(noneOption);

            //set default empty list to UI
            mcbInstitution.ValueMember = "Id";
            mcbInstitution.DisplayMember = "Description";
            mcbInstitution.DataSource = emptyList;

            //load institutions
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
                //list of institutions
                List<IdDescriptionStatus> institutions = null;

                //check if logged on user has an assigned institution
                if (Manager.LogonUser != null &&
                    Manager.LogonUser.InstitutionId > 0)
                {
                    //create institution description
                    IdDescriptionStatus institution = new IdDescriptionStatus(
                        Manager.LogonUser.InstitutionId,
                        Manager.LogonUser.InstitutionName,
                        (int)ItemStatus.Inactive);
                    institution.Result = (int)SelectResult.Success;

                    //create list of institutions
                    institutions = new List<IdDescriptionStatus>();

                    //add institution description
                    institutions.Add(institution);
                }
                else
                {
                    //get list of active institutions
                    institutions = songChannel.ListInstitutionsByStatus((int)ItemStatus.Active);
                }

                //check result
                if (institutions[0].Result == (int)SelectResult.Success)
                {
                    //sort institutions by description
                    institutions.Sort((x, y) => x.Description.CompareTo(y.Description));

                    //check if logged on user has no assigned institution
                    if (Manager.LogonUser == null ||
                        Manager.LogonUser.InstitutionId <= 0)
                    {
                        //add none option
                        institutions.Insert(0, noneOption);
                    }

                    //set institutions to UI
                    mcbInstitution.ValueMember = "Id";
                    mcbInstitution.DisplayMember = "Description";
                    mcbInstitution.DataSource = institutions;
                }
                else if (institutions[0].Result == (int)SelectResult.Empty)
                {
                    //no institution is available
                    //exit
                    return;
                }
                else if (institutions[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting institutions
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Institution, institutions[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Institution,
                        institutions[0].ErrorMessage));

                    //could not get institutions
                    //exit
                    return;
                }
            }
            catch (Exception ex)
            {
                //database error while getting institutions
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Institution), ex);

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
        /// List roles into UI.
        /// </summary>
        private void ListRoles()
        {
            //set default empty list to UI
            mcbRole.ValueMember = "Id";
            mcbRole.DisplayMember = "Description";
            mcbRole.DataSource = new List<IdDescriptionStatus>();

            //load roles
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
                //get list of active roles
                List<IdDescriptionStatus> roles = songChannel.ListRoles();

                //check result
                if (roles[0].Result == (int)SelectResult.Success)
                {
                    //sort roles by description
                    roles.Sort((x, y) => x.Description.CompareTo(y.Description));

                    //set roles to UI
                    mcbRole.ValueMember = "Id";
                    mcbRole.DisplayMember = "Description";
                    mcbRole.DataSource = roles;
                }
                else if (roles[0].Result == (int)SelectResult.Empty)
                {
                    //no role is available
                    //exit
                    return;
                }
                else if (roles[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting roles
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Role, roles[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Role,
                        roles[0].ErrorMessage));

                    //could not get roles
                    //exit
                    return;
                }
            }
            catch (Exception ex)
            {
                //database error while getting roles
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Role), ex);

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
        /// Validate input data.
        /// </summary>
        /// <returns>
        /// True if data is valid.
        /// </returns>
        private bool ValidateData()
        {
            //validate name
            if (!ValidateRequiredField(mtxtName, mlblName.Text, null, null))
            {
                //invalid field
                return false;
            }

            //validate login
            if (!ValidateRequiredField(mtxtLogin, mlblLogin.Text, null, null) ||
                !ValidateDescriptionField(mtxtLogin, mlblLogin.Text, null, null))
            {
                //invalid field
                return false;
            }

            //validate email
            if (!ValidateRequiredField(mtxtEmail, mlblEmail.Text, null, null) ||
                !ValidateEmailField(mtxtEmail, null, null))
            {
                //invalid field
                return false;
            }

            //check if this is a new user
            if (selectedId == -1)
            {
                //validate password
                if (!ValidateRequiredField(mtxtPassword, mlblPassword.Text, null, null))
                {
                    //invalid field
                    return false;
                }
            }

            //check if password is set
            if (mtxtPassword.Text.Length > 0)
            {
                //validate confirm password
                if (!ValidateRequiredField(mtxtConfirmPassword, mlblConfirmPassword.Text, null, null) ||
                    !ValidatePasswordField(mtxtPassword, mtxtConfirmPassword, null, null))
                {
                    //invalid field
                    return false;
                }

                //check if passwords match
                if (!mtxtPassword.Text.Equals(mtxtConfirmPassword.Text))
                {
                    //invalid field
                    return false;
                }
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
            //select first status
            mcbStatus.SelectedIndex = 0;

            //clear inactivation fields
            inactivationReason = string.Empty;
            inactivationTime = DateTime.MinValue;

            //clear text fields
            mtxtName.Text = string.Empty;
            mtxtLogin.Text = string.Empty;
            mtxtPassword.Text = string.Empty;
            mtxtConfirmPassword.Text = string.Empty;
            mtxtEmail.Text = string.Empty;

            //check number of roles
            if (mcbRole.Items.Count > 0)
            {
                //select first role
                mcbRole.SelectedIndex = 0;
            }

            //check number of institutions
            if (mcbInstitution.Items.Count > 0)
            {
                //select first institution
                mcbInstitution.SelectedIndex = 0;
            }

            //reset request password change option
            mcbRequestPasswordChange.Checked = false;
        }

        /// <summary>
        /// Dispose used resources from user control.
        /// </summary>
        public override void DisposeControl()
        {
            //update option to hide inactive items
            Manager.Settings.HideInactiveUsers = this.hideInactiveItems;
        }

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        /// <returns></returns>
        public override string SelectMenuOption()
        {
            //select user option
            return "User";
        }

        /// <summary>
        /// Enable all UI fields for edition.
        /// </summary>
        /// <param name="enable">True to enable fields. False to disable them.</param>
        public override void EnableFields(bool enable)
        {
            //set status list
            mcbStatus.Enabled = enable;

            //set text fields
            mtxtName.Enabled = enable;
            mtxtLogin.Enabled = enable;
            mtxtPassword.Enabled = enable;
            mtxtEmail.Enabled = enable;

            //set role list
            mcbRole.Enabled = enable;

            //set institution list
            mcbInstitution.Enabled = enable;

            //set request password change option
            mcbRequestPasswordChange.Enabled = enable;
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
                //get selected user from web service
                User user = songChannel.FindUser(itemId);

                //check result
                if (user.Result == (int)SelectResult.Empty)
                {
                    //should never happen
                    //could not load data
                    return false;
                }
                else if (user.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting user
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, user.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, user.ErrorMessage));

                    //could not load data
                    return false;
                }

                //set selected user ID
                selectedId = user.UserId;

                //select status
                mcbStatus.SelectedValue = user.UserStatus;

                //set inactivation fields
                inactivationReason = user.InactivationReason;
                inactivationTime = user.InactivationTime;

                //set text fields
                mtxtName.Text = user.Name;
                mtxtLogin.Text = user.Login;
                mtxtEmail.Text = user.Email;

                //password is never loaded
                //load hidden value instead
                mtxtPassword.Text = "hidden";
                mtxtConfirmPassword.Text = "hidden";
                
                //set request password change option
                mcbRequestPasswordChange.Checked = user.RequestPasswordChange;

                //set role
                mcbRole.SelectedValue = user.RoleId;

                //check selected index
                if (mcbRole.SelectedIndex < 0)
                {
                    try
                    {
                        //role is not available
                        //it might be inactive
                        //must load role from web service
                        Role role = songChannel.FindRole(user.RoleId);

                        //check result
                        if (role.Result == (int)SelectResult.Success)
                        {
                            //add role to list of roles
                            List<IdDescriptionStatus> roles =
                                (List<IdDescriptionStatus>)mcbRole.DataSource;
                            roles.Add(role.GetDescription());

                            //update displayed list
                            mcbRole.DataSource = null;
                            mcbRole.ValueMember = "Id";
                            mcbRole.DisplayMember = "Description";
                            mcbRole.DataSource = roles;

                            //set role
                            mcbRole.SelectedValue = user.RoleId;
                        }
                    }
                    catch
                    {
                        //do nothing
                    }
                }

                //check if user has a designated institution
                if (user.InstitutionId > 0)
                {
                    //set institution
                    mcbInstitution.SelectedValue = user.InstitutionId;

                    //check selected index
                    if (mcbInstitution.SelectedIndex < 0)
                    {
                        try
                        {
                            //institution is not available
                            //it might be inactive
                            //must load institution from web service
                            Institution institution = songChannel.FindInstitution(user.InstitutionId);

                            //check result
                            if (institution.Result == (int)SelectResult.Success)
                            {
                                //add institution to list of institutions
                                List<IdDescriptionStatus> institutions =
                                    (List<IdDescriptionStatus>)mcbInstitution.DataSource;
                                institutions.Add(institution.GetDescription());

                                //update displayed list
                                mcbInstitution.DataSource = null;
                                mcbInstitution.ValueMember = "Id";
                                mcbInstitution.DisplayMember = "Description";
                                mcbInstitution.DataSource = institutions;

                                //set institution
                                mcbInstitution.SelectedValue = user.InstitutionId;
                            }
                        }
                        catch
                        {
                            //do nothing
                        }
                    }
                }
                else
                {
                    //select none option
                    mcbInstitution.SelectedIndex = 0;
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

            //data were loaded
            return true;
        }

        /// <summary>
        /// Load user list from database.
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
                //list of users
                List<IdDescriptionStatus> users = null;

                //check if logged on user has an assigned institution
                if (Manager.LogonUser != null &&
                    Manager.LogonUser.InstitutionId > 0)
                {
                    //get list of users for assigned institution
                    users = songChannel.ListUsersByInstitution(
                        Manager.LogonUser.InstitutionId, -1);
                }
                else
                {
                    //get list of all users
                    users = songChannel.ListUsers();
                }

                //check result
                if (users[0].Result == (int)SelectResult.Success)
                {
                    //users were found
                    return users;
                }
                else if (users[0].Result == (int)SelectResult.Empty)
                {
                    //no user is available
                    return null;
                }
                else if (users[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting users
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        itemTypeDescription, users[0].ErrorMessage));

                    //could not get users
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

            //could not get users
            return null;
        }

        /// <summary>
        /// Delete current selected item.
        /// </summary>
        public override bool DeleteItem()
        {
            //create inactivation reason form
            InactivationReasonForm reasonForm = new InactivationReasonForm(
                itemTypeDescription, (int)ItemStatus.Inactive, inactivationReason);

            //let user input an inactivation reason
            reasonForm.ShowDialog(this);

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
                //inactivate selected user and get result
                DeleteResult result = songChannel.InactivateUser(
                    SelectedItemId, reasonForm.InactivationReason);

                //check result
                if (result.Result == (int)SelectResult.Success)
                {
                    //item was inactivated
                    //check if there is a parent control
                    //and if it is an user register control
                    if (parentControl != null && parentControl is ViewUserControl)
                    {
                        //update user to inactive in parent control
                        ((ViewUserControl)parentControl).UpdateUser(
                            SelectedItemId, (int)ItemStatus.Inactive,
                            DateTime.Now, reasonForm.InactivationReason);
                    }

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

            //create an user and set data
            User user = new User();

            //set selected user ID
            user.UserId = selectedId;

            //check selected status
            if (mcbStatus.SelectedIndex >= 0)
            {
                //set status
                user.UserStatus = (int)mcbStatus.SelectedValue;

                //check if selected status is inactive
                if ((int)mcbStatus.SelectedValue == (int)ItemStatus.Inactive)
                {
                    //create inactivation reason form
                    InactivationReasonForm reasonForm = new InactivationReasonForm(
                        itemTypeDescription, (int)mcbStatus.SelectedValue, inactivationReason);

                    //let user input an inactivation reason
                    reasonForm.ShowDialog(this);

                    //set inactivation reason with result
                    user.InactivationReason = reasonForm.InactivationReason;

                    //set inactivation time
                    user.InactivationTime = (inactivationTime != DateTime.MinValue) ?
                        inactivationTime : DateTime.Now;
                }
                else
                {
                    //reset inactivation
                    user.InactivationReason = string.Empty;
                    user.InactivationTime = DateTime.MinValue;
                }
            }
            else
            {
                //should never happen
                //set default status
                user.UserStatus = (int)ItemStatus.Active;

                //reset inactivation
                user.InactivationTime = DateTime.MinValue;
                user.InactivationReason = string.Empty;
            }

            //set text fields
            user.Name = mtxtName.Text.Trim();
            user.Login = mtxtLogin.Text.Trim();
            user.Email = mtxtEmail.Text.Trim();

            //check password
            if (mtxtPassword.Text.Equals(string.Empty) ||
                mtxtPassword.Text.Equals("hidden"))
            {
                //password was not entered
                user.Password = string.Empty;
            }
            else
            {
                //set password
                user.Password = mtxtPassword.Text;
            }
            
            //set request password change option
            user.RequestPasswordChange = mcbRequestPasswordChange.Checked;

            //set institution
            user.InstitutionId = (int)mcbInstitution.SelectedValue;

            //set institution name to properly display student in datagridview
            user.InstitutionName = (mcbInstitution.SelectedIndex >= 1) ?
                ((IdDescriptionStatus)mcbInstitution.SelectedItem).Description : string.Empty;

            //set role
            user.RoleId = (int)mcbRole.SelectedValue;

            //set role name to properly display student in datagridview
            user.RoleName = ((IdDescriptionStatus)mcbRole.SelectedItem).Description;

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not save user
                return null;
            }

            try
            {
                //save user and get result
                SaveResult saveResult = songChannel.SaveUser(user);

                //check result
                if (saveResult.Result == (int)SelectResult.FatalError)
                {
                    //user was not saved
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

                    //could not save user
                    return null;
                }

                //set saved ID to user ID
                user.UserId = saveResult.SavedId;

                //set selected ID
                selectedId = saveResult.SavedId;

                //check if there is a parent control
                //check if it is an user register control
                if (parentControl != null && parentControl is ViewUserControl)
                {
                    //update user in parent control
                    ((ViewUserControl)parentControl).UpdateUser(user);
                }
                //check if it is a teacher register control
                if (parentControl != null && parentControl is ViewTeacherControl)
                {
                    //update user in parent control
                    ((ViewTeacherControl)parentControl).UpdateUser(user);
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

            //user was saved
            //return updated description
            return user.GetDescription();
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
            return null;
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
        private void RegisterUser_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }
        }

        #endregion UI Event Handlers

    } //end of class RegisterUserControl 

} //end of namespace PnT.SongClient.UI.Controls
