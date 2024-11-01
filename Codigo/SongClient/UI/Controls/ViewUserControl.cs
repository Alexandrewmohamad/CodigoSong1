using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetroFramework;

using PnT.SongDB.Logic;
using PnT.SongServer;

using PnT.SongClient.Logic;


namespace PnT.SongClient.UI.Controls
{

    /// <summary>
    /// List and display users to user.
    /// </summary>
    public partial class ViewUserControl : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The song child control.
        /// </summary>
        ISongControl childControl = null;

        /// <summary>
        /// List of users shown on the control.
        /// </summary>
        private Dictionary<long, User> users = null;

        /// <summary>
        /// The last found user.
        /// Used to improve the find method.
        /// </summary>
        private User lastFoundUser = null;

        /// <summary>
        /// DataTable for users.
        /// </summary>
        private DataTable dtUsers = null;

        /// <summary>
        /// The item displayable name.
        /// </summary>
        private string itemTypeDescription = Properties.Resources.item_User;

        /// <summary>
        /// The item displayable plural name.
        /// </summary>
        protected string itemTypeDescriptionPlural = Properties.Resources.item_plural_User;

        /// <summary>
        /// Indicates if the control is being loaded.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// Right-clicked user. The user of the displayed context menu.
        /// </summary>
        private User clickedUser = null;

        /// <summary>
        /// The user ID column index in the datagridview.
        /// </summary>
        private int columnIndexUserId;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ViewUserControl()
        {
            //set loading flag
            isLoading = true;

            //init ui components
            InitializeComponent();

            //create list of users
            users = new Dictionary<long, User>();

            //create user data table
            CreateUserDataTable();

            //get user ID column index
            columnIndexUserId = dgvUsers.Columns[UserId.Name].Index;

            //load combos
            //list statuses
            List<KeyValuePair<int, string>> statuses = new List<KeyValuePair<int, string>>();
            statuses.Add(new KeyValuePair<int, string>(
                -1, Properties.Resources.wordAll));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Active, Properties.Resources.ResourceManager.GetString("ItemStatus_Active")));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Inactive, Properties.Resources.ResourceManager.GetString("ItemStatus_Inactive")));
            mcbStatus.ValueMember = "Key";
            mcbStatus.DisplayMember = "Value";
            mcbStatus.DataSource = statuses;

            //list institutions
            ListInstitutions();

            //list roles
            ListRoles();

            //subscribe settings property changed event
            Manager.Settings.PropertyChanged += Settings_PropertyChanged;
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get list of users.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public List<User> ListUsers
        {
            get
            {
                //lock list of users
                lock (users)
                {
                    return new List<User>(users.Values);
                }
            }
        }

        #endregion Properties


        #region ISong Methods *********************************************************

        /// <summary>
        /// Dispose used resources from user control.
        /// </summary>
        public void DisposeControl()
        {
            //dispose child control if any
            DisposeChildControl();

            //unsubscribe from settings property changed event
            Manager.Settings.PropertyChanged -= Settings_PropertyChanged;
        }

        /// <summary>
        /// Dispose child Song control.
        /// </summary>
        public void DisposeChildControl()
        {
            //check child control
            if (childControl == null)
            {
                //should never happen
                //exit
                return;
            }

            //display this control in main form
            Manager.MainForm.AddAndDisplayControl(this);

            //remove child control from main form
            Manager.MainForm.RemoveAndDisposeControl((UserControl)childControl);

            //remove reference to child control
            childControl = null;
        }

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        /// <returns></returns>
        public string SelectMenuOption()
        {
            //select user
            return "User";
        }

        #endregion ISong Methods


        #region Public Methods ********************************************************

        /// <summary>
        /// Set columns visibility according to application settings.
        /// </summary>
        public void SetVisibleColumns()
        {
            //split visible column settings
            string[] columns = Manager.Settings.UserGridDisplayedColumns.Split(',');

            //hide all columns
            foreach (DataGridViewColumn dgColumn in dgvUsers.Columns)
            {
                //hide column
                dgColumn.Visible = false;
            }

            //set invisible last index
            int lastIndex = dgvUsers.Columns.Count - 1;

            //check each column and set visibility
            foreach (DataGridViewColumn dgvColumn in dgvUsers.Columns)
            {
                //check each visible column setting
                for (int i = 0; i < columns.Length; i++)
                {
                    //get column name
                    string col = columns[i];

                    if (dgvColumn.Name.Equals(col))
                    {
                        //found column
                        //set it visible
                        dgvColumn.Visible = true;

                        //set column display index user
                        dgvColumn.DisplayIndex = i;

                        //exit loop
                        break;
                    }
                }

                //check if column has not become visible
                if (!dgvColumn.Visible)
                {
                    //set display index to last available
                    dgvColumn.DisplayIndex = lastIndex;

                    //decrement last index
                    lastIndex--;
                }
            }
        }

        #endregion Public Methods


        #region Private Methods *******************************************************

        /// <summary>
        /// Create User data table.
        /// </summary>
        private void CreateUserDataTable()
        {
            //create data table
            dtUsers = new DataTable();

            //UserId
            DataColumn dcUserId = new DataColumn("UserId", typeof(int));
            dtUsers.Columns.Add(dcUserId);

            //Name
            DataColumn dcName = new DataColumn("Name", typeof(string));
            dtUsers.Columns.Add(dcName);

            //Login
            DataColumn dcLogin = new DataColumn("Login", typeof(string));
            dtUsers.Columns.Add(dcLogin);

            //Institution
            DataColumn dcInstitution = new DataColumn("InstitutionName", typeof(string));
            dtUsers.Columns.Add(dcInstitution);

            //Role
            DataColumn dcRole = new DataColumn("RoleName", typeof(string));
            dtUsers.Columns.Add(dcRole);

            //Email
            DataColumn dcEmail = new DataColumn("Email", typeof(string));
            dtUsers.Columns.Add(dcEmail);

            //UserStatusName
            DataColumn dcUserStatus = new DataColumn("UserStatusName", typeof(string));
            dtUsers.Columns.Add(dcUserStatus);

            //CreationTime
            DataColumn dcCreationTime = new DataColumn("CreationTime", typeof(DateTime));
            dtUsers.Columns.Add(dcCreationTime);

            //InactivationTime
            DataColumn dcInactivationTime = new DataColumn("InactivationTime", typeof(DateTime));
            dtUsers.Columns.Add(dcInactivationTime);

            //InactivationReason
            DataColumn dcInactivationReason = new DataColumn("InactivationReason", typeof(string));
            dtUsers.Columns.Add(dcInactivationReason);

            //set primary key column
            dtUsers.PrimaryKey = new DataColumn[] { dcUserId };
        }

        /// <summary>
        /// Display selected users.
        /// Clear currently displayed users before loading selected users.
        /// </summary>
        /// <param name="selectedUsers">
        /// The selected users to be loaded.
        /// </param>
        private void DisplayUsers(List<User> selectedUsers)
        {
            //lock list of users
            lock (this.users)
            {
                //clear list
                this.users.Clear();

                //reset last found user
                lastFoundUser = null;
            }

            //lock datatable of users
            lock (dtUsers)
            {
                //clear datatable
                dtUsers.Clear();
            }

            //check number of selected users
            if (selectedUsers != null && selectedUsers.Count > 0 &&
                selectedUsers[0].Result == (int)SelectResult.Success)
            {
                //lock list of users
                lock (users)
                {
                    //add selected users
                    foreach (User user in selectedUsers)
                    {
                        //check if user is not in the list
                        if (!users.ContainsKey(user.UserId))
                        {
                            //add user to the list
                            users.Add(user.UserId, user);

                            //set last found user
                            lastFoundUser = user;
                        }
                        else
                        {
                            //should never happen
                            //log message
                            Manager.Log.WriteError(
                                "Error while loading users. Two users with same UserID " +
                                user.UserId + ".");

                            //throw exception
                            throw new ApplicationException(
                                "Error while loading users. Two users with same UserID " +
                                user.UserId + ".");
                        }
                    }
                }

                //lock data table of users
                lock (dtUsers)
                {
                    //check each user in the list
                    foreach (User user in ListUsers)
                    {
                        //create, set and add user row
                        DataRow dr = dtUsers.NewRow();
                        SetUserDataRow(dr, user);
                        dtUsers.Rows.Add(dr);
                    }
                }
            }

            //refresh displayed UI
            RefreshUI(false);
        }

        /// <summary>
        /// Find user in the list of users.
        /// </summary>
        /// <param name="userID">
        /// The ID of the selected user.
        /// </param>
        /// <returns>
        /// The user of the selected user ID.
        /// Null if user was not found.
        /// </returns>
        private User FindUser(long userID)
        {
            //lock list of users
            lock (users)
            {
                //check last found user
                if (lastFoundUser != null &&
                    lastFoundUser.UserId == userID)
                {
                    //same user
                    return lastFoundUser;
                }

                //try to find selected user
                users.TryGetValue(userID, out lastFoundUser);

                //return result
                return lastFoundUser;
            }
        }

        /// <summary>
        /// List institutions into UI.
        /// </summary>
        private void ListInstitutions()
        {
            //set default empty list to UI
            mcbInstitution.ValueMember = "Id";
            mcbInstitution.DisplayMember = "Description";
            mcbInstitution.DataSource = new List<IdDescriptionStatus>();

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
                }
                else if (institutions[0].Result == (int)SelectResult.Empty)
                {
                    //no institution is available
                    //clear list
                    institutions.Clear();
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

                    //clear list
                    institutions.Clear();
                }

                //check if logged on user has no assigned institution
                if (Manager.LogonUser == null ||
                    Manager.LogonUser.InstitutionId <= 0)
                {
                    //user can view any institution data
                    //create all option and add it to list
                    IdDescriptionStatus allOption = new IdDescriptionStatus(
                        -1, Properties.Resources.wordAll, 0);
                    institutions.Insert(0, allOption);
                }

                //set institutions to UI
                mcbInstitution.ValueMember = "Id";
                mcbInstitution.DisplayMember = "Description";
                mcbInstitution.DataSource = institutions;
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
                //get list of all roles
                List<IdDescriptionStatus> roles = songChannel.ListRoles();

                //check result
                if (roles[0].Result == (int)SelectResult.Success)
                {
                    //sort roles by description
                    roles.Sort((x, y) => x.Description.CompareTo(y.Description));
                }
                else if (roles[0].Result == (int)SelectResult.Empty)
                {
                    //no role is available
                    //clear list
                    roles.Clear();
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

                    //clear list
                    roles.Clear();
                }

                //create all option and add it to list
                IdDescriptionStatus allOption = new IdDescriptionStatus(
                    -1, Properties.Resources.wordAll, 0);
                roles.Insert(0, allOption);

                //set roles to UI
                mcbRole.ValueMember = "Id";
                mcbRole.DisplayMember = "Description";
                mcbRole.DataSource = roles;
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
        /// Load and display filtered users.
        /// </summary>
        /// <returns>
        /// True if users were loaded.
        /// False otherwise.
        /// </returns>
        private bool LoadUsers()
        {
            //filter and load users
            List<User> filteredUsers = null;

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                return false;
            }

            try
            {
                //get list of users
                filteredUsers = songChannel.FindUsersByFilter(
                    true, true, (int)mcbStatus.SelectedValue,
                    (int)mcbInstitution.SelectedValue, (int)mcbRole.SelectedValue);

                //check result
                if (filteredUsers[0].Result == (int)SelectResult.Empty)
                {
                    //no user was found
                    //clear list
                    filteredUsers.Clear();
                }
                else if (filteredUsers[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting users
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredUsers[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredUsers[0].ErrorMessage));

                    //could not load users
                    return false;
                }
            }
            catch (Exception ex)
            {
                //show error message
                MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //database error while getting users
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load users
                return false;
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

            //sort users by name
            filteredUsers.Sort((x, y) => x.Name.CompareTo(y.Name));

            //display filtered users
            DisplayUsers(filteredUsers);

            //sort users by name by default
            dgvUsers.Sort(UserName, ListSortDirection.Ascending);

            //users were loaded
            return true;
        }

        /// <summary>
        /// Refresh displayed datagrid.
        /// </summary>
        /// <param name="displayLastLine">
        /// True if last row must be displayed.
        /// False if no specific row must be displayed.
        /// </param>
        private void RefreshUI(bool displayLastRow)
        {
            //check if datagrid has not a source yet
            if (dgvUsers.DataSource == null)
            {
                //set source to datagrid
                dgvUsers.DataSource = dtUsers;
            }

            //check if last row should be displayed
            //and if the is at least one row
            if (displayLastRow && dgvUsers.Rows.Count > 0)
            {
                //refresh grid by displaying last row
                dgvUsers.FirstDisplayedScrollingRowIndex = (dgvUsers.Rows.Count - 1);
            }

            //refresh grid
            dgvUsers.Refresh();

            //set number of users
            mlblItemCount.Text = users.Count + " " +
                (users.Count > 1 ? itemTypeDescriptionPlural : itemTypeDescription);
        }

        /// <summary>
        /// Set data row with selected User data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="user">The selected user.</param>
        private void SetUserDataRow(DataRow dataRow, User user)
        {
            dataRow["UserId"] = user.UserId;
            dataRow["Name"] = user.Name;
            dataRow["Login"] = user.Login;
            dataRow["InstitutionName"] = user.InstitutionName;
            dataRow["RoleName"] = user.RoleName;
            dataRow["Email"] = user.Email;
            dataRow["UserStatusName"] = Properties.Resources.ResourceManager.GetString(
                "ItemStatus_" + ((ItemStatus)user.UserStatus).ToString());
            dataRow["CreationTime"] = user.CreationTime;
            dataRow["InactivationReason"] = user.InactivationReason;

            //set inactivation time
            if (user.InactivationTime != DateTime.MinValue)
                dataRow["InactivationTime"] = user.InactivationTime;
            else
                dataRow["InactivationTime"] = DBNull.Value;
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Remove displayed user.
        /// </summary>
        /// <param name="userId">
        /// The ID of the user to be removed.
        /// </param>
        public void RemoveUser(int userId)
        {
            //lock list of users
            lock (users)
            {
                //check if user is not in the list
                if (!users.ContainsKey(userId))
                {
                    //no need to remove user
                    //exit
                    return;
                }

                //remove user
                users.Remove(userId);
            }

            //lock data table of users
            lock (dtUsers)
            {
                //get displayed data row
                DataRow dr = dtUsers.Rows.Find(userId);

                //remove displayed data row
                dtUsers.Rows.Remove(dr);
            }

            //refresh user interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update the status of a displayed user.
        /// </summary>
        /// <param name="userId">
        /// The ID of the selected user.
        /// </param>
        /// <param name="userStatus">
        /// The updated status of the user.
        /// </param>
        /// <param name="inactivationTime">
        /// The time the user was inactivated.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the user is being inactivated.
        /// </param>
        public void UpdateUser(int userId, int userStatus,
            DateTime inactivationTime, string inactivationReason)
        {
            //the user to be updated
            User user = null;

            //lock list of users
            lock (users)
            {
                //try to find user
                if (!users.TryGetValue(userId, out user))
                {
                    //user was not found
                    //no need to update user
                    //exit
                    return;
                }
            }

            //update status
            user.UserStatus = userStatus;

            //update inactivation
            user.InactivationTime = inactivationTime;
            user.InactivationReason = inactivationReason;

            //update displayed user
            UpdateUser(user);
        }

        /// <summary>
        /// Update a displayed user. 
        /// Add user if it is a new user.
        /// </summary>
        /// <param name="user">
        /// The updated user.
        /// </param>
        public void UpdateUser(User user)
        {
            //check user should be displayed
            //status filter
            if (mcbStatus.SelectedIndex > 0 &&
                (int)mcbStatus.SelectedValue != user.UserStatus)
            {
                //user should not be displayed
                //remove user if it is being displayed
                RemoveUser(user.UserId);

                //exit
                return;
            }

            //institution filter
            if (mcbInstitution.SelectedIndex > 0 &&
                (int)mcbInstitution.SelectedValue != user.InstitutionId)
            {
                //user should not be displayed
                //remove user if it is being displayed
                RemoveUser(user.UserId);

                //exit
                return;
            }

            //role filter
            if (mcbRole.SelectedIndex > 0 &&
                (int)mcbRole.SelectedValue != user.RoleId)
            {
                //user should not be displayed
                //remove user if it is being displayed
                RemoveUser(user.UserId);

                //exit
                return;
            }

            //lock list of users
            lock (users)
            {
                //set user
                users[user.UserId] = user;
            }

            //lock data table of users
            lock (dtUsers)
            {
                //get displayed data row
                DataRow dr = dtUsers.Rows.Find(user.UserId);

                //check if there was no data row yet
                if (dr == null)
                {
                    //create, set and add data row
                    dr = dtUsers.NewRow();
                    SetUserDataRow(dr, user);
                    dtUsers.Rows.Add(dr);
                }
                else
                {
                    //set data
                    SetUserDataRow(dr, user);
                }
            }

            //refresh user interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed users with institution data.
        /// </summary>
        /// <param name="institution">
        /// The updated institution.
        /// </param>
        public void UpdateInstitution(Institution institution)
        {
            //check each institution item
            foreach (IdDescriptionStatus item in mcbInstitution.Items)
            {
                //compare data
                if (item.Id == institution.InstitutionId &&
                    !item.Description.Equals(institution.ProjectName))
                {
                    //update item
                    item.Description = institution.ProjectName;

                    //check if item is selected
                    if (mcbInstitution.SelectedItem == item)
                    {
                        //set loading flag
                        isLoading = true;

                        //clear selection and reselect item
                        mcbInstitution.SelectedIndex = -1;
                        mcbInstitution.SelectedItem = item;

                        //reset loading flag
                        isLoading = false;
                    }

                    //exit loop
                    break;
                }
            }

            //gather list of updated users
            List<User> updatedUsers = new List<User>();

            //lock list
            lock (users)
            {
                //check all displayed users
                foreach (User user in users.Values)
                {
                    //check user institution
                    if (user.InstitutionId == institution.InstitutionId &&
                        !user.InstitutionName.Equals(institution.ProjectName))
                    {
                        //update user
                        user.InstitutionName = institution.ProjectName;

                        //add user to the list of updated users
                        updatedUsers.Add(user);
                    }
                }
            }

            //check result
            if (updatedUsers.Count == 0)
            {
                //no user was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of users
            lock (dtUsers)
            {
                //check each updated role
                foreach (User user in updatedUsers)
                {
                    //get displayed data row
                    DataRow dr = dtUsers.Rows.Find(user.UserId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetUserDataRow(dr, user);
                    }
                }
            }

            //refresh institution interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed users with role data.
        /// </summary>
        /// <param name="role">
        /// The updated role.
        /// </param>
        public void UpdateRole(Role role)
        {
            //check each role item
            foreach (IdDescriptionStatus item in mcbRole.Items)
            {
                //compare data
                if (item.Id == role.RoleId &&
                    !item.Description.Equals(role.Name))
                {
                    //update item
                    item.Description = role.Name;

                    //check if item is selected
                    if (mcbRole.SelectedItem == item)
                    {
                        //set loading flag
                        isLoading = true;

                        //clear selection and reselect item
                        mcbRole.SelectedIndex = -1;
                        mcbRole.SelectedItem = item;

                        //reset loading flag
                        isLoading = false;
                    }

                    //exit loop
                    break;
                }
            }

            //gather list of updated users
            List<User> updatedUsers = new List<User>();

            //lock list
            lock (users)
            {
                //check all displayed users
                foreach (User user in users.Values)
                {
                    //check user role
                    if (user.RoleId == role.RoleId &&
                        !user.RoleName.Equals(role.Name))
                    {
                        //update user
                        user.RoleName = role.Name;

                        //add user to the list of updated users
                        updatedUsers.Add(user);
                    }
                }
            }

            //check result
            if (updatedUsers.Count == 0)
            {
                //no user was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of users
            lock (dtUsers)
            {
                //check each updated role
                foreach (User user in updatedUsers)
                {
                    //get displayed data row
                    DataRow dr = dtUsers.Rows.Find(user.UserId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetUserDataRow(dr, user);
                    }
                }
            }

            //refresh role interface
            RefreshUI(false);
        }

        /// <summary>
        /// Settings property changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //check control status
            if (!this.Created || this.Disposing || this.IsDisposed)
            {
                //no neeed to handle event
                //exit
                return;
            }

            //check which setting is changing
            if (e.PropertyName.Equals("GridFontSize"))
            {
                //grid font size is changing
                //set fonts
                dgvUsers.ColumnHeadersDefaultCellStyle.Font =
                    MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
                dgvUsers.DefaultCellStyle.Font =
                    MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            }
            else if (e.PropertyName.Equals("UserGridDisplayedColumns"))
            {
                //displayed order columns is changing
                //update columns
                SetVisibleColumns();
            }
        }

        #endregion Event Handlers


        #region UI Event Handlers *****************************************************

        /// <summary>
        /// Control load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewUserControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //avoid auto generated columns
            dgvUsers.AutoGenerateColumns = false;

            //set fonts
            dgvUsers.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvUsers.DefaultCellStyle.Font =
                MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);

            //set visible columns
            SetVisibleColumns();

            //set control item heading
            mlblItemHeading.Text = itemTypeDescriptionPlural;

            //check list of institutions
            if (mcbInstitution.Items.Count > 0)
            {
                //select all institutions for institution filter
                mcbInstitution.SelectedIndex = 0;
            }

            //check list of roles
            if (mcbRole.Items.Count > 0)
            {
                //select all roles for role filter
                mcbRole.SelectedIndex = 0;
            }

            //clear number of users
            mlblItemCount.Text = string.Empty;

            //reset loading flag
            isLoading = false;

            //load data for the first time by selecting status
            //select active for status filter
            mcbStatus.SelectedIndex = 1;
        }

        /// <summary>
        /// Status combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading)
            {
                //no need to handle this event
                //exit
                return;
            }

            //load users
            LoadUsers();
        }

        /// <summary>
        /// Institution combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading)
            {
                //no need to handle this event
                //exit
                return;
            }

            //load users
            LoadUsers();
        }

        /// <summary>
        /// Role combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading)
            {
                //no need to handle this event
                //exit
                return;
            }

            //load users
            LoadUsers();
        }

        /// <summary>
        /// Edit tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlEdit_Click(object sender, EventArgs e)
        {
            //create control
            RegisterUserControl registerControl =
                new UI.Controls.RegisterUserControl();
            registerControl.ParentControl = this;

            //check if there is any selected user
            if (dgvUsers.SelectedCells.Count > 0)
            {
                //select first selected user in the register control
                registerControl.FirstSelectedId =
                    (int)dgvUsers.Rows[dgvUsers.SelectedCells[0].RowIndex].Cells[columnIndexUserId].Value;
            }

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Users datagridview mouse up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvUsers_MouseUp(object sender, MouseEventArgs e)
        {
            //show menu only if the right mouse button is clicked.
            if (!(e.Button == MouseButtons.Right))
            {
                //exit
                return;
            }

            //get clicked point and which index was selected
            Point p = new Point(e.X, e.Y);

            //get clicked cell
            DataGridView.HitTestInfo hitInfo = dgvUsers.HitTest(e.X, e.Y);

            //check selected row index
            if (hitInfo.RowIndex < 0)
            {
                //no row selected
                //exit
                return;
            }

            //check if there are selected rows and if user clicked on them
            if (dgvUsers.SelectedRows.Count > 0 &&
                dgvUsers.Rows[hitInfo.RowIndex].Selected != true)
            {
                //user did not click in the selected rows
                //clear selection
                dgvUsers.ClearSelection();

                //select clicked row
                dgvUsers.Rows[hitInfo.RowIndex].Selected = true;
            }
            //check if there are selected cells
            else if (dgvUsers.SelectedCells.Count > 0)
            {
                //get list of selected cell rows
                HashSet<int> selectedRowIndexes = new HashSet<int>();

                //check each selected cell
                foreach (DataGridViewCell cell in dgvUsers.SelectedCells)
                {
                    //add cell row index to the list
                    selectedRowIndexes.Add(cell.RowIndex);
                }

                //check if user clicked on a row of a selected cell
                if (selectedRowIndexes.Contains(hitInfo.RowIndex))
                {
                    //user clicked on a row of a selected cell
                    //select all rows
                    foreach (int selectedRowIndex in selectedRowIndexes)
                    {
                        //select row
                        dgvUsers.Rows[selectedRowIndex].Selected = true;
                    }
                }
                else
                {
                    //user did not click on a row of a selected cell
                    //clear selected cells
                    dgvUsers.ClearSelection();

                    //select clicked row
                    dgvUsers.Rows[hitInfo.RowIndex].Selected = true;
                }
            }
            else
            {
                //select clicked row
                dgvUsers.Rows[hitInfo.RowIndex].Selected = true;
            }

            //get selected or clicked user
            clickedUser = null;

            //check if there is a selected user
            if (dgvUsers.SelectedRows.Count > 0)
            {
                //there is one or more users selected
                //get first selected user
                for (int index = 0; index < dgvUsers.SelectedRows.Count; index++)
                {
                    //get user using its user id
                    int userId = (int)dgvUsers.SelectedRows[index].Cells[columnIndexUserId].Value;
                    User user = FindUser(userId);

                    //check result
                    if (user != null)
                    {
                        //add user to list
                        clickedUser = user;

                        //exit loop
                        break;
                    }
                }

                //check result
                if (clickedUser == null)
                {
                    //no user was found
                    //should never happen
                    //do not display context menu
                    //exit
                    return;
                }
            }
            else
            {
                //there is no user selected
                //should never happen
                //do not display context menu
                //exit
                return;
            }

            //set context menu items visibility
            if (dgvUsers.SelectedRows.Count == 1)
            {
                //display view menu items according to permissions
                mnuViewUser.Visible = true;
                mnuViewRole.Visible = Manager.HasLogonPermission("Role.View");
                tssSeparator.Visible = true;

                //display view institution menu if clicked user has an institution
                mnuViewInstitution.Visible = (clickedUser.InstitutionId > 0) &&
                    Manager.HasLogonPermission("Institution.View");

                //display impersonate option
                mnuImpersonateUser.Visible = Manager.ImpersonatingUser == null &&
                    Manager.HasLogonPermission("User.Impersonate");
                tssSeparatorImpersonate.Visible = Manager.ImpersonatingUser == null &&
                    Manager.HasLogonPermission("User.Impersonate");
            }
            else
            {
                //hide view menu items
                mnuViewUser.Visible = false;
                mnuViewInstitution.Visible = false;
                mnuViewRole.Visible = false;
                tssSeparator.Visible = false;

                //hide impersonate option
                mnuImpersonateUser.Visible = false;
                tssSeparatorImpersonate.Visible = false;
            }
            
            //show user context menu on the clicked point
            mcmUser.Show(this.dgvUsers, p);
        }

        /// <summary>
        /// Impersonate user menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuImpersonateUser_Click(object sender, EventArgs e)
        {
            //check clicked user
            if (clickedUser == null)
            {
                //should never happen
                //exit
                return;
            }

            //let user impersonate user
            //display impersonation confirmation
            Manager.MainForm.ConfirmAndImpersonateUser(
                clickedUser.UserId, clickedUser.Name);
        }

        /// <summary>
        /// View user menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewUser_Click(object sender, EventArgs e)
        {
            //check clicked user
            if (clickedUser == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected user
            RegisterUserControl registerControl =
                new UI.Controls.RegisterUserControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedUser.UserId;

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// View institution menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewInstitution_Click(object sender, EventArgs e)
        {
            //check clicked user
            if (clickedUser == null)
            {
                //should never happen
                //exit
                return;
            }

            //check user institution
            if (clickedUser.InstitutionId <= 0)
            {
                //no institution
                //should never happen
                //exit
                return;
            }

            //create control to display selected institution
            RegisterInstitutionControl registerControl =
                new UI.Controls.RegisterInstitutionControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedUser.InstitutionId;

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// View role menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewRole_Click(object sender, EventArgs e)
        {
            //check clicked user
            if (clickedUser == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected role
            RegisterRoleControl registerControl =
                new UI.Controls.RegisterRoleControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedUser.RoleId;

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Copy menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCopy_Click(object sender, EventArgs e)
        {
            //check if any cell is selected
            if (this.dgvUsers.GetCellCount(DataGridViewElementStates.Selected) <= 0)
            {
                //should never happen
                //exit
                return;
            }

            try
            {
                //include header text 
                dgvUsers.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                //copy selection to the clipboard
                Clipboard.SetDataObject(this.dgvUsers.GetClipboardContent());
            }
            catch (Exception ex)
            {
                //log error
                Manager.Log.WriteException(
                    "Unexpected error while copying selected cells to clipboard.", ex);
            }
        }

        /// <summary>
        /// Display columns menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDisplayColumns_Click(object sender, EventArgs e)
        {
            //show options form as a dialog and display grid options
            OptionsForm optionsForm = new OptionsForm(itemTypeDescriptionPlural);
            optionsForm.ShowDialog(Manager.MainForm);

            //dispose window
            optionsForm.Dispose();
            optionsForm = null;
        }

        #endregion UI Event Handlers

    } //end of class ViewUserControl

} //end of namespace PnT.SongClient.UI.Controls
