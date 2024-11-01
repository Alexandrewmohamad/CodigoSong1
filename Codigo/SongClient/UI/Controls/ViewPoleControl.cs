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
    /// List and display poles to user.
    /// </summary>
    public partial class ViewPoleControl : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The song child control.
        /// </summary>
        ISongControl childControl = null;

        /// <summary>
        /// List of poles shown on the control.
        /// </summary>
        private Dictionary<long, Pole> poles = null;

        /// <summary>
        /// The last found pole.
        /// Used to improve the find method.
        /// </summary>
        private Pole lastFoundPole = null;

        /// <summary>
        /// DataTable for poles.
        /// </summary>
        private DataTable dtPoles = null;

        /// <summary>
        /// The item displayable name.
        /// </summary>
        private string itemTypeDescription = Properties.Resources.item_Pole;

        /// <summary>
        /// The item displayable plural name.
        /// </summary>
        protected string itemTypeDescriptionPlural = Properties.Resources.item_plural_Pole;

        /// <summary>
        /// Indicates if the control is being loaded.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// Right-clicked pole. The pole of the displayed context menu.
        /// </summary>
        private Pole clickedPole = null;

        /// <summary>
        /// The pole ID column index in the datagridview.
        /// </summary>
        private int columnIndexPoleId;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ViewPoleControl()
        {
            //set loading flag
            isLoading = true;

            //init ui components
            InitializeComponent();

            //create list of poles
            poles = new Dictionary<long, Pole>();

            //create pole data table
            CreatePoleDataTable();

            //get pole ID column index
            columnIndexPoleId = dgvPoles.Columns[PoleId.Name].Index;

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

            //subscribe settings property changed event
            Manager.Settings.PropertyChanged += Settings_PropertyChanged;
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get list of poles.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public List<Pole> ListPoles
        {
            get
            {
                //lock list of poles
                lock (poles)
                {
                    return new List<Pole>(poles.Values);
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
            //select pole
            return "Pole";
        }

        #endregion ISong Methods


        #region Public Methods ********************************************************

        /// <summary>
        /// Set columns visibility according to application settings.
        /// </summary>
        public void SetVisibleColumns()
        {
            //split visible column settings
            string[] columns = Manager.Settings.PoleGridDisplayedColumns.Split(',');

            //hide all columns
            foreach (DataGridViewColumn dgColumn in dgvPoles.Columns)
            {
                //hide column
                dgColumn.Visible = false;
            }

            //set invisible last index
            int lastIndex = dgvPoles.Columns.Count - 1;

            //check each column and set visibility
            foreach (DataGridViewColumn dgvColumn in dgvPoles.Columns)
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

                        //set column display index pole
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
        /// Create Pole data table.
        /// </summary>
        private void CreatePoleDataTable()
        {
            //create data table
            dtPoles = new DataTable();

            //PoleId
            DataColumn dcPoleId = new DataColumn("PoleId", typeof(int));
            dtPoles.Columns.Add(dcPoleId);

            //Name
            DataColumn dcName = new DataColumn("Name", typeof(string));
            dtPoles.Columns.Add(dcName);

            //InstitutionId
            DataColumn dcInstitutionName = new DataColumn("InstitutionName", typeof(string));
            dtPoles.Columns.Add(dcInstitutionName);

            //Location
            DataColumn dcLocation = new DataColumn("Location", typeof(string));
            dtPoles.Columns.Add(dcLocation);

            //Phones
            DataColumn dcPhone = new DataColumn("Phones", typeof(string));
            dtPoles.Columns.Add(dcPhone);

            //Email
            DataColumn dcEmail = new DataColumn("Email", typeof(string));
            dtPoles.Columns.Add(dcEmail);

            //Description
            DataColumn dcDescription = new DataColumn("Description", typeof(string));
            dtPoles.Columns.Add(dcDescription);

            //PoleStatusName
            DataColumn dcPoleStatus = new DataColumn("PoleStatusName", typeof(string));
            dtPoles.Columns.Add(dcPoleStatus);

            //CreationTime
            DataColumn dcCreationTime = new DataColumn("CreationTime", typeof(DateTime));
            dtPoles.Columns.Add(dcCreationTime);

            //InactivationTime
            DataColumn dcInactivationTime = new DataColumn("InactivationTime", typeof(DateTime));
            dtPoles.Columns.Add(dcInactivationTime);

            //InactivationReason
            DataColumn dcInactivationReason = new DataColumn("InactivationReason", typeof(string));
            dtPoles.Columns.Add(dcInactivationReason);

            //set primary key column
            dtPoles.PrimaryKey = new DataColumn[] { dcPoleId };
        }

        /// <summary>
        /// Display selected poles.
        /// Clear currently displayed poles before loading selected poles.
        /// </summary>
        /// <param name="selectedPoles">
        /// The selected poles to be loaded.
        /// </param>
        private void DisplayPoles(List<Pole> selectedPoles)
        {
            //lock list of poles
            lock (this.poles)
            {
                //clear list
                this.poles.Clear();

                //reset last found pole
                lastFoundPole = null;
            }

            //lock datatable of poles
            lock (dtPoles)
            {
                //clear datatable
                dtPoles.Clear();
            }

            //check number of selected poles
            if (selectedPoles != null && selectedPoles.Count > 0 &&
                selectedPoles[0].Result == (int)SelectResult.Success)
            {
                //lock list of poles
                lock (poles)
                {
                    //add selected poles
                    foreach (Pole pole in selectedPoles)
                    {
                        //check if pole is not in the list
                        if (!poles.ContainsKey(pole.PoleId))
                        {
                            //add pole to the list
                            poles.Add(pole.PoleId, pole);

                            //set last found pole
                            lastFoundPole = pole;
                        }
                        else
                        {
                            //should never happen
                            //log message
                            Manager.Log.WriteError(
                                "Error while loading poles. Two poles with same PoleID " +
                                pole.PoleId + ".");

                            //throw exception
                            throw new ApplicationException(
                                "Error while loading poles. Two poles with same PoleID " +
                                pole.PoleId + ".");
                        }
                    }
                }

                //lock data table of poles
                lock (dtPoles)
                {
                    //check each pole in the list
                    foreach (Pole pole in ListPoles)
                    {
                        //create, set and add pole row
                        DataRow dr = dtPoles.NewRow();
                        SetPoleDataRow(dr, pole);
                        dtPoles.Rows.Add(dr);
                    }
                }
            }

            //refresh displayed UI
            RefreshUI(false);
        }

        /// <summary>
        /// Find pole in the list of poles.
        /// </summary>
        /// <param name="poleID">
        /// The ID of the selected pole.
        /// </param>
        /// <returns>
        /// The pole of the selected pole ID.
        /// Null if pole was not found.
        /// </returns>
        private Pole FindPole(long poleID)
        {
            //lock list of poles
            lock (poles)
            {
                //check last found pole
                if (lastFoundPole != null &&
                    lastFoundPole.PoleId == poleID)
                {
                    //same pole
                    return lastFoundPole;
                }

                //try to find selected pole
                poles.TryGetValue(poleID, out lastFoundPole);

                //return result
                return lastFoundPole;
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
        /// Load and display filtered poles.
        /// </summary>
        /// <returns>
        /// True if poles were loaded.
        /// False otherwise.
        /// </returns>
        private bool LoadPoles()
        {
            //filter and load poles
            List<Pole> filteredPoles = null;

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
                //get list of poles
                filteredPoles = songChannel.FindPolesByFilter(
                    true, (int)mcbStatus.SelectedValue, (int)mcbInstitution.SelectedValue);

                //check result
                if (filteredPoles[0].Result == (int)SelectResult.Empty)
                {
                    //no pole was found
                    //clear list
                    filteredPoles.Clear();
                }
                else if (filteredPoles[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting poles
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredPoles[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredPoles[0].ErrorMessage));

                    //could not load poles
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

                //database error while getting poles
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load poles
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

            //sort poles by name
            filteredPoles.Sort((x, y) => x.Name.CompareTo(y.Name));

            //display filtered poles
            DisplayPoles(filteredPoles);

            //sort poles by name by default
            dgvPoles.Sort(DisplayName, ListSortDirection.Ascending);

            //poles were loaded
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
            if (dgvPoles.DataSource == null)
            {
                //set source to datagrid
                dgvPoles.DataSource = dtPoles;
            }

            //check if last row should be displayed
            //and if the is at least one row
            if (displayLastRow && dgvPoles.Rows.Count > 0)
            {
                //refresh grid by displaying last row
                dgvPoles.FirstDisplayedScrollingRowIndex = (dgvPoles.Rows.Count - 1);
            }

            //refresh grid
            dgvPoles.Refresh();

            //set number of poles
            mlblItemCount.Text = poles.Count + " " + 
                (poles.Count > 1 ? itemTypeDescriptionPlural : itemTypeDescription);
        }

        /// <summary>
        /// Set data row with selected Pole data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="pole">The selected pole.</param>
        private void SetPoleDataRow(DataRow dataRow, Pole pole)
        {
            dataRow["PoleId"] = pole.PoleId;
            dataRow["Name"] = pole.Name;
            dataRow["InstitutionName"] = pole.InstitutionName;
            dataRow["Location"] = pole.State + " - " + pole.City + ", " + pole.District;
            dataRow["Phones"] = GetPolePhones(pole);
            dataRow["Email"] = pole.Email;
            dataRow["Description"] = pole.Description;
            dataRow["PoleStatusName"] = Properties.Resources.ResourceManager.GetString(
                "ItemStatus_" + ((ItemStatus)pole.PoleStatus).ToString());
            dataRow["CreationTime"] = pole.CreationTime;
            dataRow["InactivationReason"] = pole.InactivationReason;

            //set inactivation time
            if (pole.InactivationTime != DateTime.MinValue)
                dataRow["InactivationTime"] = pole.InactivationTime;
            else
                dataRow["InactivationTime"] = DBNull.Value;
        }

        /// <summary>
        /// Get string that represents the selected pole phone numbers.
        /// </summary>
        /// <param name="pole">
        /// The selected pole.
        /// </param>
        /// <returns>
        /// A string with phone numbers.
        /// </returns>
        private string GetPolePhones(Pole pole)
        {
            //get pole phones
            StringBuilder sbPhones = new StringBuilder(32);

            //check pole phone
            if (pole.Phone != null && pole.Phone.Length > 0)
            {
                //add phone
                sbPhones.Append(pole.Phone);
            }

            //check pole mobile
            if (pole.Mobile != null && pole.Mobile.Length > 0)
            {
                //check if there is a previous number
                if (sbPhones.Length > 0)
                {
                    //add new line
                    sbPhones.AppendLine();
                }

                //add mobile
                sbPhones.Append(pole.Mobile);
            }

            //return created text
            return sbPhones.ToString();
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Remove displayed pole.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the pole to be removed.
        /// </param>
        public void RemovePole(int poleId)
        {
            //lock list of poles
            lock (poles)
            {
                //check if pole is not in the list
                if (!poles.ContainsKey(poleId))
                {
                    //no need to remove pole
                    //exit
                    return;
                }

                //remove pole
                poles.Remove(poleId);
            }

            //lock data table of poles
            lock (dtPoles)
            {
                //get displayed data row
                DataRow dr = dtPoles.Rows.Find(poleId);

                //remove displayed data row
                dtPoles.Rows.Remove(dr);
            }

            //refresh user interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update the status of a displayed pole.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// </param>
        /// <param name="poleStatus">
        /// The updated status of the pole.
        /// </param>
        /// <param name="inactivationTime">
        /// The time the pole was inactivated.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the pole is being inactivated.
        /// </param>
        public void UpdatePole(int poleId, int poleStatus,
            DateTime inactivationTime, string inactivationReason)
        {
            //the pole to be updated
            Pole pole = null;

            //lock list of poles
            lock (poles)
            {
                //try to find pole
                if (!poles.TryGetValue(poleId, out pole))
                {
                    //pole was not found
                    //no need to update pole
                    //exit
                    return;
                }
            }

            //update status
            pole.PoleStatus = poleStatus;

            //update inactivation
            pole.InactivationTime = inactivationTime;
            pole.InactivationReason = inactivationReason;

            //update displayed pole
            UpdatePole(pole);
        }

        /// <summary>
        /// Update a displayed pole. 
        /// Add pole if it is a new pole.
        /// </summary>
        /// <param name="pole">
        /// The updated pole.
        /// </param>
        public void UpdatePole(Pole pole)
        {
            //check pole should be displayed
            //status filter
            if (mcbStatus.SelectedIndex > 0 &&
                (int)mcbStatus.SelectedValue != pole.PoleStatus)
            {
                //pole should not be displayed
                //remove pole if it is being displayed
                RemovePole(pole.PoleId);

                //exit
                return;
            }

            //institution filter
            if (mcbInstitution.SelectedIndex > 0 &&
                (int)mcbInstitution.SelectedValue != pole.InstitutionId)
            {
                //pole should not be displayed
                //remove pole if it is being displayed
                RemovePole(pole.PoleId);

                //exit
                return;
            }

            //lock list of poles
            lock (poles)
            {
                //set pole
                poles[pole.PoleId] = pole;
            }

            //lock data table of poles
            lock (dtPoles)
            {
                //get displayed data row
                DataRow dr = dtPoles.Rows.Find(pole.PoleId);

                //check if there was no data row yet
                if (dr == null)
                {
                    //create, set and add data row
                    dr = dtPoles.NewRow();
                    SetPoleDataRow(dr, pole);
                    dtPoles.Rows.Add(dr);
                }
                else
                {
                    //set data
                    SetPoleDataRow(dr, pole);
                }
            }

            //refresh user interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed poles with institution data.
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

            //gather list of updated poles
            List<Pole> updatedPoles = new List<Pole>();

            //lock list
            lock (poles)
            {
                //check all displayed poles
                foreach (Pole pole in poles.Values)
                {
                    //check pole institution
                    if (pole.InstitutionId == institution.InstitutionId &&
                        !pole.InstitutionName.Equals(institution.ProjectName))
                    {
                        //update pole
                        pole.InstitutionName = institution.ProjectName;

                        //add pole to he list of updated poles
                        updatedPoles.Add(pole);
                    }
                }
            }

            //check result
            if (updatedPoles.Count == 0)
            {
                //no pole was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of poles
            lock (dtPoles)
            {
                //check each updated role
                foreach (Pole pole in updatedPoles)
                {
                    //get displayed data row
                    DataRow dr = dtPoles.Rows.Find(pole.PoleId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetPoleDataRow(dr, pole);
                    }
                }
            }

            //refresh user interface
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
                dgvPoles.ColumnHeadersDefaultCellStyle.Font =
                    MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
                dgvPoles.DefaultCellStyle.Font =
                    MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            }
            else if (e.PropertyName.Equals("PoleGridDisplayedColumns"))
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
        private void ViewPoleControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //avoid auto generated columns
            dgvPoles.AutoGenerateColumns = false;

            //set fonts
            dgvPoles.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvPoles.DefaultCellStyle.Font =
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

            //clear number of poles
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

            //load poles
            LoadPoles();
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

            //load poles
            LoadPoles();
        }

        /// <summary>
        /// Edit tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlEdit_Click(object sender, EventArgs e)
        {
            //create control
            RegisterPoleControl registerControl =
                new UI.Controls.RegisterPoleControl();
            registerControl.ParentControl = this;

            //check if there is any selected pole
            if (dgvPoles.SelectedCells.Count > 0)
            {
                //select first selected pole in the register control
                registerControl.FirstSelectedId =
                    (int)dgvPoles.Rows[dgvPoles.SelectedCells[0].RowIndex].Cells[columnIndexPoleId].Value;
            }

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Poles datagridview mouse up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPoles_MouseUp(object sender, MouseEventArgs e)
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
            DataGridView.HitTestInfo hitInfo = dgvPoles.HitTest(e.X, e.Y);

            //check selected row index
            if (hitInfo.RowIndex < 0)
            {
                //no row selected
                //exit
                return;
            }

            //check if there are selected rows and if pole clicked on them
            if (dgvPoles.SelectedRows.Count > 0 &&
                dgvPoles.Rows[hitInfo.RowIndex].Selected != true)
            {
                //pole did not click in the selected rows
                //clear selection
                dgvPoles.ClearSelection();

                //select clicked row
                dgvPoles.Rows[hitInfo.RowIndex].Selected = true;
            }
            //check if there are selected cells
            else if (dgvPoles.SelectedCells.Count > 0)
            {
                //get list of selected cell rows
                HashSet<int> selectedRowIndexes = new HashSet<int>();

                //check each selected cell
                foreach (DataGridViewCell cell in dgvPoles.SelectedCells)
                {
                    //add cell row index to the list
                    selectedRowIndexes.Add(cell.RowIndex);
                }

                //check if pole clicked on a row of a selected cell
                if (selectedRowIndexes.Contains(hitInfo.RowIndex))
                {
                    //pole clicked on a row of a selected cell
                    //select all rows
                    foreach (int selectedRowIndex in selectedRowIndexes)
                    {
                        //select row
                        dgvPoles.Rows[selectedRowIndex].Selected = true;
                    }
                }
                else
                {
                    //pole did not click on a row of a selected cell
                    //clear selected cells
                    dgvPoles.ClearSelection();

                    //select clicked row
                    dgvPoles.Rows[hitInfo.RowIndex].Selected = true;
                }
            }
            else
            {
                //select clicked row
                dgvPoles.Rows[hitInfo.RowIndex].Selected = true;
            }

            //get selected or clicked pole
            clickedPole = null;

            //check if there is a selected pole
            if (dgvPoles.SelectedRows.Count > 0)
            {
                //there is one or more poles selected
                //get first selected pole
                for (int index = 0; index < dgvPoles.SelectedRows.Count; index++)
                {
                    //get pole using its pole id
                    int poleId = (int)dgvPoles.SelectedRows[index].Cells[columnIndexPoleId].Value;
                    Pole pole = FindPole(poleId);

                    //check result
                    if (pole != null)
                    {
                        //add pole to list
                        clickedPole = pole;

                        //exit loop
                        break;
                    }
                }

                //check result
                if (clickedPole == null)
                {
                    //no pole was found
                    //should never happen
                    //do not display context menu
                    //exit
                    return;
                }
            }
            else
            {
                //there is no pole selected
                //should never happen
                //do not display context menu
                //exit
                return;
            }

            //set context menu items visibility
            if (dgvPoles.SelectedRows.Count == 1)
            {
                //display view menu items according to permissions
                mnuViewPole.Visible = true;
                mnuViewInstitution.Visible = Manager.HasLogonPermission("Institution.View");
                tssSeparator.Visible = true;
            }
            else
            {
                //hide view menu items
                mnuViewPole.Visible = false;
                mnuViewInstitution.Visible = false;
                tssSeparator.Visible = false;
            }

            //show pole context menu on the clicked point
            mcmPole.Show(this.dgvPoles, p);
        }

        /// <summary>
        /// View pole menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewPole_Click(object sender, EventArgs e)
        {
            //check clicked pole
            if (clickedPole == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected pole
            RegisterPoleControl registerControl =
                new UI.Controls.RegisterPoleControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedPole.PoleId;

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
            //check clicked pole
            if (clickedPole == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected pole institution
            RegisterInstitutionControl registerControl =
                new UI.Controls.RegisterInstitutionControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedPole.InstitutionId;

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
            if (this.dgvPoles.GetCellCount(DataGridViewElementStates.Selected) <= 0)
            {
                //should never happen
                //exit
                return;
            }

            try
            {
                //include header text 
                dgvPoles.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                //copy selection to the clipboard
                Clipboard.SetDataObject(this.dgvPoles.GetClipboardContent());
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
        
    } //end of class ViewPoleControl

} //end of namespace PnT.SongClient.UI.Controls
