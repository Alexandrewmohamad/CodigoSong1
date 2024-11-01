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
    /// List and display instruments to instrument.
    /// </summary>
    public partial class ViewInstrumentControl : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The song child control.
        /// </summary>
        ISongControl childControl = null;

        /// <summary>
        /// List of instruments shown on the control.
        /// </summary>
        private Dictionary<long, Instrument> instruments = null;

        /// <summary>
        /// The last found instrument.
        /// Used to improve the find method.
        /// </summary>
        private Instrument lastFoundInstrument = null;

        /// <summary>
        /// DataTable for instruments.
        /// </summary>
        private DataTable dtInstruments = null;

        /// <summary>
        /// The item displayable name.
        /// </summary>
        private string itemTypeDescription = Properties.Resources.item_Instrument;

        /// <summary>
        /// The item displayable plural name.
        /// </summary>
        protected string itemTypeDescriptionPlural = Properties.Resources.item_plural_Instrument;

        /// <summary>
        /// Indicates if the control is being loaded.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// Indicates if the control is loading poles.
        /// </summary>
        private bool isLoadingPoles = false;

        /// <summary>
        /// Right-clicked instrument. The instrument of the displayed context menu.
        /// </summary>
        private Instrument clickedInstrument = null;

        /// <summary>
        /// The instrument ID column index in the datagridview.
        /// </summary>
        private int columnIndexInstrumentId;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ViewInstrumentControl()
        {
            //set loading flag
            isLoading = true;

            //init ui components
            InitializeComponent();

            //create list of instruments
            instruments = new Dictionary<long, Instrument>();

            //create instrument data table
            CreateInstrumentDataTable();

            //get instrument ID column index
            columnIndexInstrumentId = dgvInstruments.Columns[InstrumentId.Name].Index;

            //load combos
            //list statuses
            List<KeyValuePair<int, string>> statuses = new List<KeyValuePair<int, string>>();
            statuses.Add(new KeyValuePair<int, string>(
                -1, Properties.Resources.wordAll));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Active, Properties.Resources.ResourceManager.GetString("ItemStatus_Active")));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Inactive, Properties.Resources.ResourceManager.GetString("ItemStatus_Inactive")));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Maintenance, Properties.Resources.ResourceManager.GetString("ItemStatus_Maintenance")));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Lost, Properties.Resources.ResourceManager.GetString("ItemStatus_Lost")));
            mcbStatus.ValueMember = "Key";
            mcbStatus.DisplayMember = "Value";
            mcbStatus.DataSource = statuses;

            //list instrument types
            ListInstrumentTypes();

            //list institutions
            ListInstitutions();

            //check if logged on user has an assigned institution
            if (Manager.LogonUser != null &&
                Manager.LogonUser.InstitutionId > 0)
            {
                //list assigned institution poles
                ListPoles(Manager.LogonUser.InstitutionId);
            }
            else
            {
                //list all poles
                ListPoles(-1);
            }

            //subscribe settings property changed event
            Manager.Settings.PropertyChanged += Settings_PropertyChanged;
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get list of instruments.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public List<Instrument> ListInstruments
        {
            get
            {
                //lock list of instruments
                lock (instruments)
                {
                    return new List<Instrument>(instruments.Values);
                }
            }
        }

        #endregion Properties


        #region ISong Methods *********************************************************

        /// <summary>
        /// Dispose used resources from instrument control.
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
            //select instrument
            return "Instrument";
        }

        #endregion ISong Methods


        #region Public Methods ********************************************************

        /// <summary>
        /// Set columns visibility according to application settings.
        /// </summary>
        public void SetVisibleColumns()
        {
            //split visible column settings
            string[] columns = Manager.Settings.InstrumentGridDisplayedColumns.Split(',');

            //hide all columns
            foreach (DataGridViewColumn dgColumn in dgvInstruments.Columns)
            {
                //hide column
                dgColumn.Visible = false;
            }

            //set invisible last index
            int lastIndex = dgvInstruments.Columns.Count - 1;

            //check each column and set visibility
            foreach (DataGridViewColumn dgvColumn in dgvInstruments.Columns)
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

                        //set column display index instrument
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
        /// Create Instrument data table.
        /// </summary>
        private void CreateInstrumentDataTable()
        {
            //create data table
            dtInstruments = new DataTable();

            //InstrumentId
            DataColumn dcInstrumentId = new DataColumn("InstrumentId", typeof(int));
            dtInstruments.Columns.Add(dcInstrumentId);

            //Code
            DataColumn dcCode = new DataColumn("Code", typeof(string));
            dtInstruments.Columns.Add(dcCode);

            //InstrumentTypeName
            DataColumn dcInstrumentTypeName = new DataColumn("InstrumentTypeName", typeof(string));
            dtInstruments.Columns.Add(dcInstrumentTypeName);

            //Pole
            DataColumn dcPole = new DataColumn("PoleName", typeof(string));
            dtInstruments.Columns.Add(dcPole);

            //Location
            DataColumn dcLocation = new DataColumn("Location", typeof(string));
            dtInstruments.Columns.Add(dcLocation);

            //InstrumentStatusName
            DataColumn dcInstrumentStatus = new DataColumn("InstrumentStatusName", typeof(string));
            dtInstruments.Columns.Add(dcInstrumentStatus);

            //CreationTime
            DataColumn dcCreationTime = new DataColumn("CreationTime", typeof(DateTime));
            dtInstruments.Columns.Add(dcCreationTime);

            //InactivationTime
            DataColumn dcInactivationTime = new DataColumn("InactivationTime", typeof(DateTime));
            dtInstruments.Columns.Add(dcInactivationTime);

            //InactivationReason
            DataColumn dcInactivationReason = new DataColumn("InactivationReason", typeof(string));
            dtInstruments.Columns.Add(dcInactivationReason);

            //set primary key column
            dtInstruments.PrimaryKey = new DataColumn[] { dcInstrumentId };
        }

        /// <summary>
        /// Display selected instruments.
        /// Clear currently displayed instruments before loading selected instruments.
        /// </summary>
        /// <param name="selectedInstruments">
        /// The selected instruments to be loaded.
        /// </param>
        private void DisplayInstruments(List<Instrument> selectedInstruments)
        {
            //lock list of instruments
            lock (this.instruments)
            {
                //clear list
                this.instruments.Clear();

                //reset last found instrument
                lastFoundInstrument = null;
            }

            //lock datatable of instruments
            lock (dtInstruments)
            {
                //clear datatable
                dtInstruments.Clear();
            }

            //check number of selected instruments
            if (selectedInstruments != null && selectedInstruments.Count > 0 &&
                selectedInstruments[0].Result == (int)SelectResult.Success)
            {
                //lock list of instruments
                lock (instruments)
                {
                    //add selected instruments
                    foreach (Instrument instrument in selectedInstruments)
                    {
                        //check if instrument is not in the list
                        if (!instruments.ContainsKey(instrument.InstrumentId))
                        {
                            //add instrument to the list
                            instruments.Add(instrument.InstrumentId, instrument);

                            //set last found instrument
                            lastFoundInstrument = instrument;
                        }
                        else
                        {
                            //should never happen
                            //log message
                            Manager.Log.WriteError(
                                "Error while loading instruments. Two instruments with same InstrumentID " +
                                instrument.InstrumentId + ".");

                            //throw exception
                            throw new ApplicationException(
                                "Error while loading instruments. Two instruments with same InstrumentID " +
                                instrument.InstrumentId + ".");
                        }
                    }
                }

                //lock data table of instruments
                lock (dtInstruments)
                {
                    //check each instrument in the list
                    foreach (Instrument instrument in ListInstruments)
                    {
                        //create, set and add instrument row
                        DataRow dr = dtInstruments.NewRow();
                        SetInstrumentDataRow(dr, instrument);
                        dtInstruments.Rows.Add(dr);
                    }
                }
            }

            //refresh displayed UI
            RefreshUI(false);
        }

        /// <summary>
        /// Find instrument in the list of instruments.
        /// </summary>
        /// <param name="instrumentID">
        /// The ID of the selected instrument.
        /// </param>
        /// <returns>
        /// The instrument of the selected instrument ID.
        /// Null if instrument was not found.
        /// </returns>
        private Instrument FindInstrument(long instrumentID)
        {
            //lock list of instruments
            lock (instruments)
            {
                //check last found instrument
                if (lastFoundInstrument != null &&
                    lastFoundInstrument.InstrumentId == instrumentID)
                {
                    //same instrument
                    return lastFoundInstrument;
                }

                //try to find selected instrument
                instruments.TryGetValue(instrumentID, out lastFoundInstrument);

                //return result
                return lastFoundInstrument;
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
        /// List poles into UI for selected institution
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// -1 to select all institutions.
        /// </param>
        private void ListPoles(int institutionId)
        {
            //set default empty list to UI
            mcbPole.ValueMember = "Id";
            mcbPole.DisplayMember = "Description";
            mcbPole.DataSource = new List<IdDescriptionStatus>();

            //load poles
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
                //list of poles to be displayed
                List<IdDescriptionStatus> poles = null;

                //check selected institution
                if (institutionId <= 0)
                {
                    //get list of all active poles
                    poles = songChannel.ListPolesByStatus((int)ItemStatus.Active);
                }
                else
                {
                    //get list of institution active poles
                    poles = songChannel.ListPolesByInstitution(
                        institutionId, (int)ItemStatus.Active);
                }

                //check result
                if (poles[0].Result == (int)SelectResult.Success)
                {
                    //sort poles by description
                    poles.Sort((x, y) => x.Description.CompareTo(y.Description));
                }
                else if (poles[0].Result == (int)SelectResult.Empty)
                {
                    //no pole is available
                    //clear list
                    poles.Clear();
                }
                else if (poles[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting poles
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Pole, poles[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Pole,
                        poles[0].ErrorMessage));

                    //clear list
                    poles.Clear();
                }

                //create all option and add it to list
                IdDescriptionStatus allOption = new IdDescriptionStatus(
                    -1, Properties.Resources.wordAll, 0);
                poles.Insert(0, allOption);

                //set poles to UI
                mcbPole.ValueMember = "Id";
                mcbPole.DisplayMember = "Description";
                mcbPole.DataSource = poles;
            }
            catch (Exception ex)
            {
                //database error while getting poles
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Pole), ex);

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
        /// List instruments into UI.
        /// </summary>
        private void ListInstrumentTypes()
        {
            //create list of instrument types
            List<KeyValuePair<int, string>> instrumentTypes = new List<KeyValuePair<int, string>>();

            //check each instrument type
            foreach (InstrumentsType instrumentType in Enum.GetValues(typeof(InstrumentsType)))
            {
                //add converted instrument type
                instrumentTypes.Add(new KeyValuePair<int, string>(
                    (int)instrumentType, Properties.Resources.ResourceManager.GetString(
                        "InstrumentsType_" + instrumentType.ToString())
                    ));
            }

            //create all option and add it to list
            instrumentTypes.Insert(0, new KeyValuePair<int, string>(
                -1, Properties.Resources.wordAll));

            //set instrument types to UI
            mcbInstrumentType.ValueMember = "Key";
            mcbInstrumentType.DisplayMember = "Value";
            mcbInstrumentType.DataSource = instrumentTypes;
        }

        /// <summary>
        /// Load and display filtered instruments.
        /// </summary>
        /// <returns>
        /// True if instruments were loaded.
        /// False otherwise.
        /// </returns>
        private bool LoadInstruments()
        {
            //filter and load instruments
            List<Instrument> filteredInstruments = null;

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
                //get list of instruments
                filteredInstruments = songChannel.FindInstrumentsByFilter(
                    true, true, (int)mcbStatus.SelectedValue, 
                    (int)mcbInstrumentType.SelectedValue,
                    (int)mcbInstitution.SelectedValue,
                    (int)mcbPole.SelectedValue);

                //check result
                if (filteredInstruments[0].Result == (int)SelectResult.Empty)
                {
                    //no instrument was found
                    //clear list
                    filteredInstruments.Clear();
                }
                else if (filteredInstruments[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting instruments
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredInstruments[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredInstruments[0].ErrorMessage));

                    //could not load instruments
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

                //database error while getting instruments
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load instruments
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

            //sort instruments by code
            filteredInstruments.Sort((x, y) => x.Code.CompareTo(y.Code));

            //display filtered instruments
            DisplayInstruments(filteredInstruments);

            //sort instruments by code by default
            dgvInstruments.Sort(Code, ListSortDirection.Ascending);

            //instruments were loaded
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
            if (dgvInstruments.DataSource == null)
            {
                //set source to datagrid
                dgvInstruments.DataSource = dtInstruments;
            }

            //check if last row should be displayed
            //and if the is at least one row
            if (displayLastRow && dgvInstruments.Rows.Count > 0)
            {
                //refresh grid by displaying last row
                dgvInstruments.FirstDisplayedScrollingRowIndex = (dgvInstruments.Rows.Count - 1);
            }

            //refresh grid
            dgvInstruments.Refresh();

            //set number of instruments
            mlblItemCount.Text = instruments.Count + " " +
                (instruments.Count > 1 ? itemTypeDescriptionPlural : itemTypeDescription);
        }

        /// <summary>
        /// Set data row with selected Instrument data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="instrument">The selected instrument.</param>
        private void SetInstrumentDataRow(DataRow dataRow, Instrument instrument)
        {
            dataRow["InstrumentId"] = instrument.InstrumentId;
            dataRow["Code"] = instrument.Code;
            dataRow["InstrumentTypeName"] = Properties.Resources.ResourceManager.GetString(
                    "InstrumentsType_" + ((InstrumentsType)instrument.InstrumentType).ToString());
            dataRow["PoleName"] = instrument.PoleName;
            dataRow["InstrumentStatusName"] = Properties.Resources.ResourceManager.GetString(
                "ItemStatus_" + ((ItemStatus)instrument.InstrumentStatus).ToString());
            dataRow["CreationTime"] = instrument.CreationTime;
            dataRow["InactivationReason"] = instrument.InactivationReason;

            //set inactivation time
            if (instrument.InactivationTime != DateTime.MinValue)
                dataRow["InactivationTime"] = instrument.InactivationTime;
            else
                dataRow["InactivationTime"] = DBNull.Value;

            //set location
            if (instrument.InstrumentStatus == (int)ItemStatus.Active)
            {
                //check if instrument is loaned to student
                if (instrument.StudentName != null &&
                    instrument.StudentName.Length > 0)
                {
                    //loaned
                    dataRow["Location"] = string.Format(
                        Properties.Resources.locationWithStudent, instrument.StudentName);
                }
                else
                {
                    //available
                    dataRow["Location"] = string.Format(
                        Properties.Resources.locationAvailableAt, instrument.StorageLocation);
                }
            }
            else
            {
                dataRow["Location"] = Properties.Resources.ResourceManager.GetString(
                    "ItemStatus_" + ((ItemStatus)instrument.InstrumentStatus).ToString());
            }
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Remove displayed instrument.
        /// </summary>
        /// <param name="instrumentId">
        /// The ID of the instrument to be removed.
        /// </param>
        public void RemoveInstrument(int instrumentId)
        {
            //lock list of instruments
            lock (instruments)
            {
                //check if instrument is not in the list
                if (!instruments.ContainsKey(instrumentId))
                {
                    //no need to remove instrument
                    //exit
                    return;
                }

                //remove instrument
                instruments.Remove(instrumentId);
            }

            //lock data table of instruments
            lock (dtInstruments)
            {
                //get displayed data row
                DataRow dr = dtInstruments.Rows.Find(instrumentId);

                //remove displayed data row
                dtInstruments.Rows.Remove(dr);
            }

            //refresh instrument interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update the status of a displayed instrument.
        /// </summary>
        /// <param name="instrumentId">
        /// The ID of the selected instrument.
        /// </param>
        /// <param name="instrumentStatus">
        /// The updated status of the instrument.
        /// </param>
        /// <param name="inactivationTime">
        /// The time the instrument was inactivated.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the instrument is being inactivated.
        /// </param>
        public void UpdateInstrument(int instrumentId, int instrumentStatus,
            DateTime inactivationTime, string inactivationReason)
        {
            //the instrument to be updated
            Instrument instrument = null;

            //lock list of instruments
            lock (instruments)
            {
                //try to find instrument
                if (!instruments.TryGetValue(instrumentId, out instrument))
                {
                    //instrument was not found
                    //no need to update instrument
                    //exit
                    return;
                }
            }

            //update status
            instrument.InstrumentStatus = instrumentStatus;

            //update inactivation
            instrument.InactivationTime = inactivationTime;
            instrument.InactivationReason = inactivationReason;

            //update displayed instrument
            UpdateInstrument(instrument);
        }

        /// <summary>
        /// Update a displayed instrument. 
        /// Add instrument if it is a new instrument.
        /// </summary>
        /// <param name="instrument">
        /// The updated instrument.
        /// </param>
        public void UpdateInstrument(Instrument instrument)
        {
            //check instrument should be displayed
            //status filter
            if (mcbStatus.SelectedIndex > 0 &&
                (int)mcbStatus.SelectedValue != instrument.InstrumentStatus)
            {
                //instrument should not be displayed
                //remove instrument if it is being displayed
                RemoveInstrument(instrument.InstrumentId);

                //exit
                return;
            }

            //instrument type filter
            if (mcbInstrumentType.SelectedIndex > 0 &&
                (int)mcbInstrumentType.SelectedValue != instrument.InstrumentType)
            {
                //instrument should not be displayed
                //remove instrument if it is being displayed
                RemoveInstrument(instrument.InstrumentId);

                //exit
                return;
            }

            //pole filter
            if (mcbPole.SelectedIndex > 0 &&
                (int)mcbPole.SelectedValue != instrument.PoleId)
            {
                //instrument should not be displayed
                //remove instrument if it is being displayed
                RemoveInstrument(instrument.InstrumentId);

                //exit
                return;
            }

            ////institution filter
            ////no pole should be selected
            //if (mcbPole.SelectedIndex == -1 && 
            //    mcbInstitution.SelectedIndex > 0 &&
            //    (int)mcbInstitution.SelectedValue != instrument.InstitutionId)
            //{
            //    //instrument should not be displayed
            //    //remove instrument if it is being displayed
            //    RemoveInstrument(instrument.InstrumentId);

            //    //exit
            //    return;
            //}

            //lock list of instruments
            lock (instruments)
            {
                //set instrument
                instruments[instrument.InstrumentId] = instrument;
            }

            //lock data table of instruments
            lock (dtInstruments)
            {
                //get displayed data row
                DataRow dr = dtInstruments.Rows.Find(instrument.InstrumentId);

                //check if there was no data row yet
                if (dr == null)
                {
                    //create, set and add data row
                    dr = dtInstruments.NewRow();
                    SetInstrumentDataRow(dr, instrument);
                    dtInstruments.Rows.Add(dr);
                }
                else
                {
                    //set data
                    SetInstrumentDataRow(dr, instrument);
                }
            }

            //refresh instrument interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed instruments with pole data.
        /// </summary>
        /// <param name="pole">
        /// The updated pole.
        /// </param>
        public void UpdatePole(Pole pole)
        {
            //check each pole item
            foreach (IdDescriptionStatus item in mcbPole.Items)
            {
                //compare data
                if (item.Id == pole.PoleId &&
                    !item.Description.Equals(pole.Name))
                {
                    //update item
                    item.Description = pole.Name;

                    //check if item is selected
                    if (mcbPole.SelectedItem == item)
                    {
                        //set loading flag
                        isLoading = true;

                        //clear selection and reselect item
                        mcbPole.SelectedIndex = -1;
                        mcbPole.SelectedItem = item;

                        //reset loading flag
                        isLoading = false;
                    }

                    //exit loop
                    break;
                }
            }

            //gather list of updated instruments
            List<Instrument> updatedInstruments = new List<Instrument>();

            //lock list
            lock (instruments)
            {
                //check all displayed instruments
                foreach (Instrument instrument in instruments.Values)
                {
                    //check instrument pole
                    if (instrument.PoleId == pole.PoleId &&
                        !instrument.PoleName.Equals(pole.Name))
                    {
                        //update instrument
                        instrument.PoleName = pole.Name;

                        //add instrument to the list of updated instruments
                        updatedInstruments.Add(instrument);
                    }
                }
            }

            //check result
            if (updatedInstruments.Count == 0)
            {
                //no instrument was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of instruments
            lock (dtInstruments)
            {
                //check each updated role
                foreach (Instrument instrument in updatedInstruments)
                {
                    //get displayed data row
                    DataRow dr = dtInstruments.Rows.Find(instrument.InstrumentId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetInstrumentDataRow(dr, instrument);
                    }
                }
            }

            //refresh pole interface
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
                dgvInstruments.ColumnHeadersDefaultCellStyle.Font =
                    MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
                dgvInstruments.DefaultCellStyle.Font =
                    MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            }
            else if (e.PropertyName.Equals("InstrumentGridDisplayedColumns"))
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
        private void ViewInstrumentControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //avoid auto generated columns
            dgvInstruments.AutoGenerateColumns = false;

            //set fonts
            dgvInstruments.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvInstruments.DefaultCellStyle.Font =
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

            //check list of poles
            if (mcbPole.Items.Count > 0)
            {
                //select all poles for pole filter
                mcbPole.SelectedIndex = 0;
            }

            //clear number of instruments
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

            //load instruments
            LoadInstruments();
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

            //set flag is loading pole
            isLoadingPoles = true;

            //get id of current selected pole
            int selectedPoleId = mcbPole.SelectedIndex >= 0 ?
                (int)mcbPole.SelectedValue : int.MinValue;

            //reload pole list
            ListPoles(mcbInstitution.SelectedIndex >= 0 ?
                (int)mcbInstitution.SelectedValue : -1);

            //try to reselet previous selected pole
            mcbPole.SelectedValue = selectedPoleId;

            //check result
            if (mcbPole.SelectedIndex < 0)
            {
                //select all option
                mcbPole.SelectedIndex = 0;
            }

            //reset flag is loading pole
            isLoadingPoles = false;

            //load instruments
            LoadInstruments();
        }

        /// <summary>
        /// Pole combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbPole_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading or if poles are being loaded
            if (isLoading || isLoadingPoles)
            {
                //no need to handle this event
                //exit
                return;
            }

            //load instruments
            LoadInstruments();
        }

        /// <summary>
        /// Instrument type combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbInstrumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading)
            {
                //no need to handle this event
                //exit
                return;
            }

            //load instruments
            LoadInstruments();
        }

        /// <summary>
        /// Edit tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlEdit_Click(object sender, EventArgs e)
        {
            //create control
            RegisterInstrumentControl registerControl =
                new UI.Controls.RegisterInstrumentControl();
            registerControl.ParentControl = this;

            //check if there is any selected instrument
            if (dgvInstruments.SelectedCells.Count > 0)
            {
                //select first selected instrument in the register control
                registerControl.FirstSelectedId =
                    (int)dgvInstruments.Rows[dgvInstruments.SelectedCells[0].RowIndex].Cells[columnIndexInstrumentId].Value;
            }

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Instruments datagridview mouse up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvInstruments_MouseUp(object sender, MouseEventArgs e)
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
            DataGridView.HitTestInfo hitInfo = dgvInstruments.HitTest(e.X, e.Y);

            //check selected row index
            if (hitInfo.RowIndex < 0)
            {
                //no row selected
                //exit
                return;
            }

            //check if there are selected rows and if instrument clicked on them
            if (dgvInstruments.SelectedRows.Count > 0 &&
                dgvInstruments.Rows[hitInfo.RowIndex].Selected != true)
            {
                //instrument did not click in the selected rows
                //clear selection
                dgvInstruments.ClearSelection();

                //select clicked row
                dgvInstruments.Rows[hitInfo.RowIndex].Selected = true;
            }
            //check if there are selected cells
            else if (dgvInstruments.SelectedCells.Count > 0)
            {
                //get list of selected cell rows
                HashSet<int> selectedRowIndexes = new HashSet<int>();

                //check each selected cell
                foreach (DataGridViewCell cell in dgvInstruments.SelectedCells)
                {
                    //add cell row index to the list
                    selectedRowIndexes.Add(cell.RowIndex);
                }

                //check if instrument clicked on a row of a selected cell
                if (selectedRowIndexes.Contains(hitInfo.RowIndex))
                {
                    //instrument clicked on a row of a selected cell
                    //select all rows
                    foreach (int selectedRowIndex in selectedRowIndexes)
                    {
                        //select row
                        dgvInstruments.Rows[selectedRowIndex].Selected = true;
                    }
                }
                else
                {
                    //instrument did not click on a row of a selected cell
                    //clear selected cells
                    dgvInstruments.ClearSelection();

                    //select clicked row
                    dgvInstruments.Rows[hitInfo.RowIndex].Selected = true;
                }
            }
            else
            {
                //select clicked row
                dgvInstruments.Rows[hitInfo.RowIndex].Selected = true;
            }

            //get selected or clicked instrument
            clickedInstrument = null;

            //check if there is a selected instrument
            if (dgvInstruments.SelectedRows.Count > 0)
            {
                //there is one or more instruments selected
                //get first selected instrument
                for (int index = 0; index < dgvInstruments.SelectedRows.Count; index++)
                {
                    //get instrument using its instrument id
                    int instrumentId = (int)dgvInstruments.SelectedRows[index].Cells[columnIndexInstrumentId].Value;
                    Instrument instrument = FindInstrument(instrumentId);

                    //check result
                    if (instrument != null)
                    {
                        //add instrument to list
                        clickedInstrument = instrument;

                        //exit loop
                        break;
                    }
                }

                //check result
                if (clickedInstrument == null)
                {
                    //no instrument was found
                    //should never happen
                    //do not display context menu
                    //exit
                    return;
                }
            }
            else
            {
                //there is no instrument selected
                //should never happen
                //do not display context menu
                //exit
                return;
            }

            //set context menu items visibility
            if (dgvInstruments.SelectedRows.Count == 1)
            {
                //display view menu items according to permissions
                mnuViewInstrument.Visible = true;
                mnuViewPole.Visible = Manager.HasLogonPermission("Pole.View");
                tssSeparator.Visible = true;
            }
            else
            {
                //hide view menu items
                mnuViewInstrument.Visible = false;
                mnuViewPole.Visible = false;
                tssSeparator.Visible = false;
            }

            //show instrument context menu on the clicked point
            mcmInstrument.Show(this.dgvInstruments, p);
        }

        /// <summary>
        /// View instrument menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewInstrument_Click(object sender, EventArgs e)
        {
            //check clicked instrument
            if (clickedInstrument == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected instrument
            RegisterInstrumentControl registerControl =
                new UI.Controls.RegisterInstrumentControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedInstrument.InstrumentId;

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// View pole menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewPole_Click(object sender, EventArgs e)
        {
            //check clicked instrument
            if (clickedInstrument == null)
            {
                //should never happen
                //exit
                return;
            }

            //check instrument pole
            if (clickedInstrument.PoleId <= 0)
            {
                //no pole
                //should never happen
                //exit
                return;
            }

            //create control to display selected pole
            RegisterPoleControl registerControl =
                new UI.Controls.RegisterPoleControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedInstrument.PoleId;

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
            if (this.dgvInstruments.GetCellCount(DataGridViewElementStates.Selected) <= 0)
            {
                //should never happen
                //exit
                return;
            }

            try
            {
                //include header text 
                dgvInstruments.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                //copy selection to the clipboard
                Clipboard.SetDataObject(this.dgvInstruments.GetClipboardContent());
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

    } //end of class ViewInstrumentControl

} //end of namespace PnT.SongClient.UI.Controls
