using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PnT.SongDB.Logic;
using PnT.SongServer;

using PnT.SongClient.Logic;


namespace PnT.SongClient.UI.Controls
{

    /// <summary>
    /// List and display institutions to user.
    /// </summary>
    public partial class ViewInstitutionControl : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The song child control.
        /// </summary>
        ISongControl childControl = null;

        /// <summary>
        /// List of institutions shown on the control.
        /// </summary>
        private Dictionary<long, Institution> institutions = null;

        /// <summary>
        /// The last found institution.
        /// Used to improve the find method.
        /// </summary>
        private Institution lastFoundInstitution = null;

        /// <summary>
        /// DataTable for institutions.
        /// </summary>
        private DataTable dtInstitutions = null;

        /// <summary>
        /// The item displayable name.
        /// </summary>
        private string itemTypeDescription = Properties.Resources.item_Institution;

        /// <summary>
        /// The item displayable plural name.
        /// </summary>
        protected string itemTypeDescriptionPlural = Properties.Resources.item_plural_Institution;

        /// <summary>
        /// Indicates if the control is being loaded.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// Right-clicked institution. The institution of the displayed context menu.
        /// </summary>
        private Institution clickedInstitution = null;

        /// <summary>
        /// The institution ID column index in the datagridview.
        /// </summary>
        private int columnIndexInstitutionId;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ViewInstitutionControl()
        {
            //set loading flag
            isLoading = true;

            //init ui components
            InitializeComponent();

            //create list of institutions
            institutions = new Dictionary<long, Institution>();

            //create institution data table
            CreateInstitutionDataTable();

            //get institution ID column index
            columnIndexInstitutionId = dgvInstitutions.Columns[InstitutionId.Name].Index;

            //load combos
            //load statuses
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

            //subscribe settings property changed event
            Manager.Settings.PropertyChanged += Settings_PropertyChanged;
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get list of institutions.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public List<Institution> ListInstitutions
        {
            get
            {
                //lock list of institutions
                lock (institutions)
                {
                    return new List<Institution>(institutions.Values);
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
            //select institution
            return "Institution";
        }

        #endregion ISong Methods


        #region Public Methods ********************************************************

        /// <summary>
        /// Set columns visibility according to application settings.
        /// </summary>
        public void SetVisibleColumns()
        {
            //split visible column settings
            string[] columns = Manager.Settings.InstitutionGridDisplayedColumns.Split(',');

            //hide all columns
            foreach (DataGridViewColumn dgColumn in dgvInstitutions.Columns)
            {
                //hide column
                dgColumn.Visible = false;
            }

            //set invisible last index
            int lastIndex = dgvInstitutions.Columns.Count - 1;

            //check each column and set visibility
            foreach (DataGridViewColumn dgvColumn in dgvInstitutions.Columns)
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

                        //set column display index institution
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
        /// Create Institution data table.
        /// </summary>
        private void CreateInstitutionDataTable()
        {
            //create data table
            dtInstitutions = new DataTable();

            //InstitutionId
            DataColumn dcInstitutionId = new DataColumn("InstitutionId", typeof(int));
            dtInstitutions.Columns.Add(dcInstitutionId);

            //EntityName
            DataColumn dcEntityName = new DataColumn("EntityName", typeof(string));
            dtInstitutions.Columns.Add(dcEntityName);

            //ProjectName
            DataColumn dcProjectName = new DataColumn("ProjectName", typeof(string));
            dtInstitutions.Columns.Add(dcProjectName);

            //LocalInitiative
            DataColumn dcLocalInitiative = new DataColumn("LocalInitiative", typeof(string));
            dtInstitutions.Columns.Add(dcLocalInitiative);

            //Institutionalized
            DataColumn dcInstitutionalized = new DataColumn("Institutionalized", typeof(string));
            dtInstitutions.Columns.Add(dcInstitutionalized);

            //TaxId
            DataColumn dcTaxId = new DataColumn("TaxId", typeof(string));
            dtInstitutions.Columns.Add(dcTaxId);

            //CoordinatorName
            DataColumn dcCoordinatorId = new DataColumn("CoordinatorName", typeof(string));
            dtInstitutions.Columns.Add(dcCoordinatorId);

            //Location
            DataColumn dcAddress = new DataColumn("Location", typeof(string));
            dtInstitutions.Columns.Add(dcAddress);

            //Phones
            DataColumn dcPhone = new DataColumn("Phones", typeof(string));
            dtInstitutions.Columns.Add(dcPhone);

            //Site
            DataColumn dcSite = new DataColumn("Site", typeof(string));
            dtInstitutions.Columns.Add(dcSite);

            //Email
            DataColumn dcEmail = new DataColumn("Email", typeof(string));
            dtInstitutions.Columns.Add(dcEmail);

            //InstitutionStatus
            DataColumn dcInstitutionStatus = new DataColumn("InstitutionStatusName", typeof(string));
            dtInstitutions.Columns.Add(dcInstitutionStatus);

            //CreationTime
            DataColumn dcCreationTime = new DataColumn("CreationTime", typeof(DateTime));
            dtInstitutions.Columns.Add(dcCreationTime);

            //InactivationTime
            DataColumn dcInactivationTime = new DataColumn("InactivationTime", typeof(DateTime));
            dtInstitutions.Columns.Add(dcInactivationTime);

            //InactivationReason
            DataColumn dcInactivationReason = new DataColumn("InactivationReason", typeof(string));
            dtInstitutions.Columns.Add(dcInactivationReason);

            //set primary key column
            dtInstitutions.PrimaryKey = new DataColumn[] { dcInstitutionId };
        }

        /// <summary>
        /// Display selected institutions.
        /// Clear currently displayed institutions before loading selected institutions.
        /// </summary>
        /// <param name="selectedInstitutions">
        /// The selected institutions to be loaded.
        /// </param>
        private void DisplayInstitutions(List<Institution> selectedInstitutions)
        {
            //lock list of institutions
            lock (this.institutions)
            {
                //clear list
                this.institutions.Clear();

                //reset last found institution
                lastFoundInstitution = null;
            }

            //lock datatable of institutions
            lock (dtInstitutions)
            {
                //clear datatable
                dtInstitutions.Clear();
            }

            //check number of selected institutions
            if (selectedInstitutions != null && selectedInstitutions.Count > 0 &&
                selectedInstitutions[0].Result == (int)SelectResult.Success)
            {
                //lock list of institutions
                lock (institutions)
                {
                    //add selected institutions
                    foreach (Institution institution in selectedInstitutions)
                    {
                        //check if institution is not in the list
                        if (!institutions.ContainsKey(institution.InstitutionId))
                        {
                            //add institution to the list
                            institutions.Add(institution.InstitutionId, institution);

                            //set last found institution
                            lastFoundInstitution = institution;
                        }
                        else
                        {
                            //should never happen
                            //log message
                            Manager.Log.WriteError(
                                "Error while loading institutions. Two institutions with same InstitutionID " +
                                institution.InstitutionId + ".");

                            //throw exception
                            throw new ApplicationException(
                                "Error while loading institutions. Two institutions with same InstitutionID " +
                                institution.InstitutionId + ".");
                        }
                    }
                }

                //lock data table of institutions
                lock (dtInstitutions)
                {
                    //check each institution in the list
                    foreach (Institution institution in ListInstitutions)
                    {
                        //create, set and add institution row
                        DataRow dr = dtInstitutions.NewRow();
                        SetInstitutionDataRow(dr, institution);
                        dtInstitutions.Rows.Add(dr);
                    }
                }
            }

            //refresh displayed UI
            RefreshUI(false);
        }

        /// <summary>
        /// Find institution in the list of institutions.
        /// </summary>
        /// <param name="institutionID">
        /// The ID of the selected institution.
        /// </param>
        /// <returns>
        /// The institution of the selected institution ID.
        /// Null if institution was not found.
        /// </returns>
        private Institution FindInstitution(long institutionID)
        {
            //lock list of institutions
            lock (institutions)
            {
                //check last found institution
                if (lastFoundInstitution != null &&
                    lastFoundInstitution.InstitutionId == institutionID)
                {
                    //same institution
                    return lastFoundInstitution;
                }

                //try to find selected institution
                institutions.TryGetValue(institutionID, out lastFoundInstitution);

                //return result
                return lastFoundInstitution;
            }
        }

        /// <summary>
        /// Load and display filtered institutions.
        /// </summary>
        /// <returns>
        /// True if institutions were loaded.
        /// False otherwise.
        /// </returns>
        private bool LoadInstitutions()
        {
            //filter and load institutions
            List<Institution> filteredInstitutions = null;

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
                //check if logged on user has an assigned institution
                if (Manager.LogonUser != null &&
                    Manager.LogonUser.InstitutionId > 0)
                {
                    //get assigned institution
                    Institution institution = songChannel.FindInstitution(
                        Manager.LogonUser.InstitutionId);

                    //check result
                    if (institution.Result == (int)SelectResult.Success)
                    {
                        //filter assigned institution
                        if (mcbStatus.SelectedIndex > 0 &&
                            (int)mcbStatus.SelectedValue != institution.InstitutionStatus)
                        {
                            //remove assigned institution
                            institution.Result = (int)SelectResult.Empty;
                        }
                    }

                    //create list
                    filteredInstitutions = new List<Institution>();

                    //add assigned institution
                    filteredInstitutions.Add(institution);
                }
                else
                {
                    //get list of institutions
                    filteredInstitutions = songChannel.FindInstitutionsByFilter(
                        true, (int)mcbStatus.SelectedValue);
                }

                //check result
                if (filteredInstitutions[0].Result == (int)SelectResult.Empty)
                {
                    //no institution was found
                    //clear list
                    filteredInstitutions.Clear();
                }
                else if (filteredInstitutions[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting institutions
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredInstitutions[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredInstitutions[0].ErrorMessage));

                    //could not load institutions
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

                //database error while getting institutions
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load institutions
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

            //sort institutions by name
            filteredInstitutions.Sort((x, y) => x.ProjectName.CompareTo(y.ProjectName));

            //display filtered institutions
            DisplayInstitutions(filteredInstitutions);

            //sort institutions by name by default
            dgvInstitutions.Sort(DisplayName, ListSortDirection.Ascending);

            //institutions were loaded
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
            if (dgvInstitutions.DataSource == null)
            {
                //set source to datagrid
                dgvInstitutions.DataSource = dtInstitutions;
            }

            //check if last row should be displayed
            //and if the is at least one row
            if (displayLastRow && dgvInstitutions.Rows.Count > 0)
            {
                //refresh grid by displaying last row
                dgvInstitutions.FirstDisplayedScrollingRowIndex = (dgvInstitutions.Rows.Count - 1);
            }

            //refresh grid
            dgvInstitutions.Refresh();

            //set number of institutions
            mlblItemCount.Text = institutions.Count + " " +
                (institutions.Count > 1 ? itemTypeDescriptionPlural : itemTypeDescription);
        }

        /// <summary>
        /// Set data row with selected Institution data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="institution">The selected institution.</param>
        private void SetInstitutionDataRow(DataRow dataRow, Institution institution)
        {
            //set data row
            dataRow["InstitutionId"] = institution.InstitutionId;
            dataRow["EntityName"] = institution.EntityName;
            dataRow["ProjectName"] = institution.ProjectName;
            dataRow["LocalInitiative"] = institution.LocalInitiative;
            dataRow["Institutionalized"] = institution.Institutionalized ? 
                Properties.Resources.wordYes : Properties.Resources.wordNo;
            dataRow["TaxId"] = institution.TaxId;
            dataRow["CoordinatorName"] = institution.CoordinatorName;
            dataRow["Location"] = institution.State + " - " + institution.City + ", " + institution.District;
            dataRow["Phones"] = GetInstitutionPhones(institution);
            dataRow["Site"] = institution.Site;
            dataRow["Email"] = institution.Email;
            dataRow["InstitutionStatusName"] = Properties.Resources.ResourceManager.GetString(
                "ItemStatus_" + ((ItemStatus)institution.InstitutionStatus).ToString());
            dataRow["CreationTime"] = institution.CreationTime;
            dataRow["InactivationReason"] = institution.InactivationReason;

            //set inactivation time
            if (institution.InactivationTime != DateTime.MinValue)
                dataRow["InactivationTime"] = institution.InactivationTime;
            else
                dataRow["InactivationTime"] = DBNull.Value;
        }

        /// <summary>
        /// Get string that represents the selected institution phone numbers.
        /// </summary>
        /// <param name="institution">
        /// The selected institution.
        /// </param>
        /// <returns>
        /// A string with phone numbers.
        /// </returns>
        private string GetInstitutionPhones(Institution institution)
        {
            //get institution phones
            StringBuilder sbPhones = new StringBuilder(32);

            //check institution phone
            if (institution.Phone != null && institution.Phone.Length > 0)
            {
                //add phone
                sbPhones.Append(institution.Phone);
            }

            //check institution mobile
            if (institution.Mobile != null && institution.Mobile.Length > 0)
            {
                //check if there is a previous number
                if (sbPhones.Length > 0)
                {
                    //add new line
                    sbPhones.AppendLine();
                }

                //add mobile
                sbPhones.Append(institution.Mobile);
            }

            //return created text
            return sbPhones.ToString();
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Remove displayed institution.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the institution to be removed.
        /// </param>
        public void RemoveInstitution(int institutionId)
        {
            //lock list of institutions
            lock (institutions)
            {
                //check if institution is not in the list
                if (!institutions.ContainsKey(institutionId))
                {
                    //no need to remove institution
                    //exit
                    return;
                }

                //remove institution
                institutions.Remove(institutionId);
            }

            //lock data table of institutions
            lock (dtInstitutions)
            {
                //get displayed data row
                DataRow dr = dtInstitutions.Rows.Find(institutionId);

                //remove displayed data row
                dtInstitutions.Rows.Remove(dr);
            }

            //refresh user interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update the status of a displayed institution.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <param name="institutionStatus">
        /// The updated status of the institution.
        /// </param>
        /// <param name="inactivationTime">
        /// The time the institution was inactivated.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the institution is being inactivated.
        /// </param>
        public void UpdateInstitution(int institutionId, int institutionStatus, 
            DateTime inactivationTime, string inactivationReason)
        {
            //the institution to be updated
            Institution institution = null;

            //lock list of institutions
            lock (institutions)
            {
                //try to find institution
                if (!institutions.TryGetValue(institutionId, out institution))
                {
                    //institution was not found
                    //no need to update institution
                    //exit
                    return;
                }
            }

            //update status
            institution.InstitutionStatus = institutionStatus;

            //update inactivation
            institution.InactivationTime = inactivationTime;
            institution.InactivationReason = inactivationReason;

            //update displayed institution
            UpdateInstitution(institution);
        }

        /// <summary>
        /// Update a displayed institution. 
        /// Add institution if it is a new institution.
        /// </summary>
        /// <param name="institution">
        /// The updated institution.
        /// </param>
        public void UpdateInstitution(Institution institution)
        {
            //check institution should be displayed
            //status filter
            if (mcbStatus.SelectedIndex > 0 &&
                (int)mcbStatus.SelectedValue != institution.InstitutionStatus)
            {
                //institution should not be displayed
                //remove institution if it is being displayed
                RemoveInstitution(institution.InstitutionId);

                //exit
                return;
            }
            
            //lock list of institutions
            lock (institutions)
            {
                //set institution
                institutions[institution.InstitutionId] = institution;
            }

            //lock data table of institutions
            lock (dtInstitutions)
            {
                //get displayed data row
                DataRow dr = dtInstitutions.Rows.Find(institution.InstitutionId);

                //check if there was no data row yet
                if (dr == null)
                {
                    //create, set and add data row
                    dr = dtInstitutions.NewRow();
                    SetInstitutionDataRow(dr, institution);
                    dtInstitutions.Rows.Add(dr);
                }
                else
                {
                    //set data
                    SetInstitutionDataRow(dr, institution);
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
                dgvInstitutions.ColumnHeadersDefaultCellStyle.Font =
                    MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
                dgvInstitutions.DefaultCellStyle.Font =
                    MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            }
            else if (e.PropertyName.Equals("InstitutionGridDisplayedColumns"))
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
        private void ViewInstitutionControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //avoid auto generated columns
            dgvInstitutions.AutoGenerateColumns = false;

            //set fonts
            dgvInstitutions.ColumnHeadersDefaultCellStyle.Font = 
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvInstitutions.DefaultCellStyle.Font = 
                MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);

            //set visible columns
            SetVisibleColumns();

            //set control item heading
            mlblItemHeading.Text = itemTypeDescriptionPlural;

            //clear number of institutions
            mlblItemCount.Text = string.Empty;

            //clear selected status filter 
            //so event will be handled down below
            mcbStatus.SelectedIndex = -1;

            //reset loading flag
            isLoading = false;

            //load data for the first time by selecting status
            //check if logged on user has an assigned institution
            if (Manager.LogonUser != null &&
                Manager.LogonUser.InstitutionId > 0)
            {
                //select all for status filter
                mcbStatus.SelectedIndex = 0;
            }
            else
            {
                //select active for status filter
                mcbStatus.SelectedIndex = 1;
            }
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

            //load institutions
            LoadInstitutions();
        }

        /// <summary>
        /// Edit tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlEdit_Click(object sender, EventArgs e)
        {
            //create control
            RegisterInstitutionControl registerControl = 
                new UI.Controls.RegisterInstitutionControl();
            registerControl.ParentControl = this;

            //check if there is any selected institution
            if (dgvInstitutions.SelectedCells.Count > 0)
            {
                //select first selected institution in the register control
                registerControl.FirstSelectedId = 
                    (int)dgvInstitutions.Rows[dgvInstitutions.SelectedCells[0].RowIndex].Cells[columnIndexInstitutionId].Value;
            }

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Institutions datagridview mouse up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvInstitutions_MouseUp(object sender, MouseEventArgs e)
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
            DataGridView.HitTestInfo hitInfo = dgvInstitutions.HitTest(e.X, e.Y);

            //check selected row index
            if (hitInfo.RowIndex < 0)
            {
                //no row selected
                //exit
                return;
            }

            //check if there are selected rows and if institution clicked on them
            if (dgvInstitutions.SelectedRows.Count > 0 &&
                dgvInstitutions.Rows[hitInfo.RowIndex].Selected != true)
            {
                //institution did not click in the selected rows
                //clear selection
                dgvInstitutions.ClearSelection();

                //select clicked row
                dgvInstitutions.Rows[hitInfo.RowIndex].Selected = true;
            }
            //check if there are selected cells
            else if (dgvInstitutions.SelectedCells.Count > 0)
            {
                //get list of selected cell rows
                HashSet<int> selectedRowIndexes = new HashSet<int>();

                //check each selected cell
                foreach (DataGridViewCell cell in dgvInstitutions.SelectedCells)
                {
                    //add cell row index to the list
                    selectedRowIndexes.Add(cell.RowIndex);
                }

                //check if institution clicked on a row of a selected cell
                if (selectedRowIndexes.Contains(hitInfo.RowIndex))
                {
                    //institution clicked on a row of a selected cell
                    //select all rows
                    foreach (int selectedRowIndex in selectedRowIndexes)
                    {
                        //select row
                        dgvInstitutions.Rows[selectedRowIndex].Selected = true;
                    }
                }
                else
                {
                    //institution did not click on a row of a selected cell
                    //clear selected cells
                    dgvInstitutions.ClearSelection();

                    //select clicked row
                    dgvInstitutions.Rows[hitInfo.RowIndex].Selected = true;
                }
            }
            else
            {
                //select clicked row
                dgvInstitutions.Rows[hitInfo.RowIndex].Selected = true;
            }

            //get selected or clicked institution
            clickedInstitution = null;

            //check if there is a selected institution
            if (dgvInstitutions.SelectedRows.Count > 0)
            {
                //there is one or more institutions selected
                //get first selected institution
                for (int index = 0; index < dgvInstitutions.SelectedRows.Count; index++)
                {
                    //get institution using its institution id
                    int institutionId = (int)dgvInstitutions.SelectedRows[index].Cells[columnIndexInstitutionId].Value;
                    Institution institution = FindInstitution(institutionId);

                    //check result
                    if (institution != null)
                    {
                        //add institution to list
                        clickedInstitution = institution;

                        //exit loop
                        break;
                    }
                }

                //check result
                if (clickedInstitution == null)
                {
                    //no institution was found
                    //should never happen
                    //do not display context menu
                    //exit
                    return;
                }
            }
            else
            {
                //there is no institution selected
                //should never happen
                //do not display context menu
                //exit
                return;
            }

            //set context menu items visibility
            if (dgvInstitutions.SelectedRows.Count == 1)
            {
                //display view menu items according to permissions
                mnuViewInstitution.Visible = true;
                mnuViewCoordinator.Visible = Manager.HasLogonPermission("User.View");
                tssSeparator.Visible = true;

                //display impersonate option
                mnuImpersonateCoordinator.Visible = Manager.ImpersonatingUser == null &&
                    Manager.HasLogonPermission("User.Impersonate");
                tssSeparatorImpersonate.Visible = Manager.ImpersonatingUser == null &&
                    Manager.HasLogonPermission("User.Impersonate");
            }
            else
            {
                //hide view menu items
                mnuViewInstitution.Visible = false;
                mnuViewCoordinator.Visible = false;
                tssSeparator.Visible = false;

                //hide impersonate option
                mnuImpersonateCoordinator.Visible = false;
                tssSeparatorImpersonate.Visible = false;
            }

            //show institution context menu on the clicked point
            mcmInstitution.Show(this.dgvInstitutions, p);
        }

        /// <summary>
        /// View institution menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewInstitution_Click(object sender, EventArgs e)
        {
            //check clicked institution
            if (clickedInstitution == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected institution
            RegisterInstitutionControl registerControl =
                new UI.Controls.RegisterInstitutionControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedInstitution.InstitutionId;

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// View coordinator menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewCoordinator_Click(object sender, EventArgs e)
        {
            //check clicked institution
            if (clickedInstitution == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected user
            RegisterUserControl registerControl =
                new UI.Controls.RegisterUserControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedInstitution.CoordinatorId;

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Impersonate coordinator menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuImpersonateCoordinator_Click(object sender, EventArgs e)
        {
            //check clicked institution
            if (clickedInstitution == null)
            {
                //should never happen
                //exit
                return;
            }

            //let user impersonate coordinator user
            //display impersonation confirmation
            Manager.MainForm.ConfirmAndImpersonateUser(
                clickedInstitution.CoordinatorId, clickedInstitution.CoordinatorName);
        }

        /// <summary>
        /// Copy menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCopy_Click(object sender, EventArgs e)
        {
            //check if any cell is selected
            if (this.dgvInstitutions.GetCellCount(DataGridViewElementStates.Selected) <= 0)
            {
                //should never happen
                //exit
                return;
            }

            try
            {
                //include header text 
                dgvInstitutions.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                //copy selection to the clipboard
                Clipboard.SetDataObject(this.dgvInstitutions.GetClipboardContent());
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

    } //end of class ViewInstitutionControl

} //end of namespace PnT.SongClient.UI.Controls
