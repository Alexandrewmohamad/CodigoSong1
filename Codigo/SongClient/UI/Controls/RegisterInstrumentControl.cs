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
    /// This control is used to manage instrument registries in web service.
    /// Inherits from base register control.
    /// </summary>
    public partial class RegisterInstrumentControl : RegisterBaseControl
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

        /// <summary>
        /// The list of loans for the selected instrument.
        /// </summary>
        private Dictionary<int, Loan> loans = null;

        /// <summary>
        /// DataTable for loans.
        /// </summary>
        private DataTable dtLoans = null;

        /// <summary>
        /// The last found loan.
        /// Used to improve the find method.
        /// </summary>
        private Loan lastFoundLoan = null;

        /// <summary>
        /// True if a loan is being loaded into UI.
        /// </summary>
        private bool isLoadingLoan = false;

        /// <summary>
        /// The list of all loaded student lists.
        /// Keep lists for better performance.
        /// </summary>
        private Dictionary<int, List<IdDescriptionStatus>> studentLists = null;

        /// <summary>
        /// The loan ID column index in the datagridview.
        /// </summary>
        private int columnIndexLoanId;

        #endregion Fields


        #region Constructors **********************************************************


        public RegisterInstrumentControl() : base("Instrument", Manager.Settings.HideInactiveInstruments)
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

            //instrument cannot be deleted
            this.classHasDeletion = false;

            //set permissions
            this.allowAddItem = Manager.HasLogonPermission("Instrument.Insert");
            this.allowEditItem = Manager.HasLogonPermission("Instrument.Edit");
            this.allowDeleteItem = Manager.HasLogonPermission("Instrument.Inactivate");

            //create list of loans
            loans = new Dictionary<int, Loan>();

            //create list of loaded students
            studentLists = new Dictionary<int, List<IdDescriptionStatus>>();

            //create loan data table
            CreateLoanDataTable();

            //get loan ID column index
            columnIndexLoanId = dgvLoans.Columns[LoanId.Name].Index;

            //avoid auto generated columns
            dgvComments.AutoGenerateColumns = false;
            dgvLoans.AutoGenerateColumns = false;

            //load combos
            //list statuses
            List<KeyValuePair<int, string>> statuses = new List<KeyValuePair<int, string>>();
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

            //list loan statuses
            statuses = new List<KeyValuePair<int, string>>();
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Active, Properties.Resources.wordOpen));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Inactive, Properties.Resources.wordClosed));
            mcbLoanStatus.ValueMember = "Key";
            mcbLoanStatus.DisplayMember = "Value";
            mcbLoanStatus.DataSource = statuses;

            //list instrument types
            ListInstrumentTypes();

            //list poles
            ListPoles();

            //subscribe settings property changed event
            Manager.Settings.PropertyChanged += Settings_PropertyChanged;
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get list of loans.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public List<Loan> ListLoans
        {
            get
            {
                //lock list of loans
                lock (loans)
                {
                    return new List<Loan>(loans.Values);
                }
            }
        }

        #endregion Properties


        #region Private Methods *******************************************************

        /// <summary>
        /// Clear loan fields.
        /// </summary>
        private void ClearLoanFields()
        {
            //clear student selection
            mcbLoanStudent.SelectedIndex = -1;

            //clear status selection
            mcbLoanStatus.SelectedIndex = -1;

            //clear dates
            mtxtLoanStartDate.Text = string.Empty;
            mtxtLoanEndDate.Text = string.Empty;
        }

        /// <summary>
        /// Create Loan data table.
        /// </summary>
        private void CreateLoanDataTable()
        {
            //create data table
            dtLoans = new DataTable();

            //LoanId
            DataColumn dcLoanId = new DataColumn("LoanId", typeof(int));
            dtLoans.Columns.Add(dcLoanId);

            //Student
            DataColumn dcStudent = new DataColumn("StudentName", typeof(string));
            dtLoans.Columns.Add(dcStudent);

            //StartDate
            DataColumn dcStartDate = new DataColumn("StartDate", typeof(DateTime));
            dtLoans.Columns.Add(dcStartDate);

            //EndDate
            DataColumn dcEndDate = new DataColumn("EndDate", typeof(DateTime));
            dtLoans.Columns.Add(dcEndDate);

            //Comments
            DataColumn dcComments = new DataColumn("Comments", typeof(string));
            dtLoans.Columns.Add(dcComments);

            //LoanStatusName
            DataColumn dcLoanStatus = new DataColumn("LoanStatusName", typeof(string));
            dtLoans.Columns.Add(dcLoanStatus);

            //set primary key column
            dtLoans.PrimaryKey = new DataColumn[] { dcLoanId };
        }

        /// <summary>
        /// Display selected loans.
        /// Clear currently displayed loans before loading selected loans.
        /// </summary>
        /// <param name="selectedLoans">
        /// The selected loans to be loaded.
        /// </param>
        private void DisplayLoans(List<Loan> selectedLoans)
        {
            //lock list of loans
            lock (this.loans)
            {
                //clear list
                this.loans.Clear();

                //reset last found loan
                lastFoundLoan = null;
            }

            //lock datatable of loans
            lock (dtLoans)
            {
                //clear datatable
                dtLoans.Clear();
            }

            //check number of selected loans
            if (selectedLoans != null && selectedLoans.Count > 0 &&
                selectedLoans[0].Result == (int)SelectResult.Success)
            {
                //lock list of loans
                lock (loans)
                {
                    //add selected loans
                    foreach (Loan loan in selectedLoans)
                    {
                        //check if loan is not in the list
                        if (!loans.ContainsKey(loan.LoanId))
                        {
                            //add loan to the list
                            loans.Add(loan.LoanId, loan);

                            //set last found loan
                            lastFoundLoan = loan;
                        }
                        else
                        {
                            //should never happen
                            //log message
                            Manager.Log.WriteError(
                                "Error while loading loans. Two loans with same LoanID " +
                                loan.LoanId + ".");

                            //throw exception
                            throw new ApplicationException(
                                "Error while loading loans. Two loans with same LoanID " +
                                loan.LoanId + ".");
                        }
                    }
                }

                //lock data table of loans
                lock (dtLoans)
                {
                    //check each loan in the list
                    foreach (Loan loan in ListLoans)
                    {
                        //create, set and add loan row
                        DataRow dr = dtLoans.NewRow();
                        SetLoanDataRow(dr, loan);
                        dtLoans.Rows.Add(dr);
                    }
                }
            }

            //refresh displayed loans
            RefreshLoans(-1, true);
        }

        /// <summary>
        /// Enable loan fields according to current context.
        /// </summary>
        private void EnableLoanFields()
        {
            //selected loan
            Loan loan = null;

            //check if there is a selected loan
            if (dgvLoans.SelectedRows.Count > 0)
            {
                //get loan using its loan id
                loan = FindLoan((int)dgvLoans.SelectedRows[0].Cells[columnIndexLoanId].Value);
            }

            //can only if loan is the most recent loan

            //set delete loan button
            //only let user delete before edition threshold
            mbtnDeleteLoan.Enabled = mbtnAddLoan.Enabled &&
                loan != null && dgvLoans.SelectedRows[0].Index == 0 &&
                loan.CreationTime.Date >= DateTime.Today.AddDays(-Loan.EDITION_THRESHOLD);

            //set student combo box
            //only let user edit student before edition threshold
            mcbLoanStudent.Enabled = mbtnAddLoan.Enabled &&
                loan != null && dgvLoans.SelectedRows[0].Index == 0 &&
                loan.CreationTime.Date >= DateTime.Today.AddDays(-Loan.EDITION_THRESHOLD);

            //set status combo box
            mcbLoanStatus.Enabled = mbtnAddLoan.Enabled && 
                loan != null && dgvLoans.SelectedRows[0].Index == 0;

            //set start date
            //only let user edit start date before edition threshold
            mtxtLoanStartDate.Enabled = mbtnAddLoan.Enabled &&
                loan != null && dgvLoans.SelectedRows[0].Index == 0 &&
                loan.CreationTime.Date >= DateTime.Today.AddDays(-Loan.EDITION_THRESHOLD);

            //set end date
            mtxtLoanEndDate.Enabled = mbtnAddLoan.Enabled &&
                loan != null && dgvLoans.SelectedRows[0].Index == 0;

            //hide loan ID column
            LoanId.Visible = false;
        }

        /// <summary>
        /// Find loan in the list of loans.
        /// </summary>
        /// <param name="loanID">
        /// The ID of the selected loan.
        /// </param>
        /// <returns>
        /// The loan of the selected loan ID.
        /// Null if loan was not found.
        /// </returns>
        private Loan FindLoan(int loanID)
        {
            //lock list of loans
            lock (loans)
            {
                //check last found loan
                if (lastFoundLoan != null &&
                    lastFoundLoan.LoanId == loanID)
                {
                    //same loan
                    return lastFoundLoan;
                }

                //try to find selected loan
                loans.TryGetValue(loanID, out lastFoundLoan);

                //return result
                return lastFoundLoan;
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

            //set instrument types to UI
            mcbInstrumentType.ValueMember = "Key";
            mcbInstrumentType.DisplayMember = "Value";
            mcbInstrumentType.DataSource = instrumentTypes;
        }

        /// <summary>
        /// List poles into UI.
        /// </summary>
        private void ListPoles()
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
                //list of poles
                List<IdDescriptionStatus> poles = null;
                
                //check if logged on user has an assigned institution
                if (Manager.LogonUser != null &&
                    Manager.LogonUser.InstitutionId > 0)
                {
                    //get list of active pole for the assigned institution
                    poles = songChannel.ListPolesByInstitution(
                        Manager.LogonUser.InstitutionId, (int)ItemStatus.Active);
                }
                else
                {
                    //get list of all active poles
                    poles = songChannel.ListPolesByStatus((int)ItemStatus.Active);
                }

                //check result
                if (poles[0].Result == (int)SelectResult.Success)
                {
                    //sort poles by description
                    poles.Sort((x, y) => x.Description.CompareTo(y.Description));

                    //set poles to UI
                    mcbPole.ValueMember = "Id";
                    mcbPole.DisplayMember = "Description";
                    mcbPole.DataSource = poles;
                }
                else if (poles[0].Result == (int)SelectResult.Empty)
                {
                    //no pole is available
                    //exit
                    return;
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

                    //could not get poles
                    //exit
                    return;
                }
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
        /// List students into UI for selected pole.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// -1 to select all poles.
        /// </param>
        private void ListStudents(int poleId)
        {
            //set default empty list to UI
            mcbLoanStudent.ValueMember = "Id";
            mcbLoanStudent.DisplayMember = "Description";
            mcbLoanStudent.DataSource = new List<IdDescriptionStatus>();

            //check if there is a list of students is for selected pole
            if (studentLists.ContainsKey(poleId))
            {
                //set stored students to UI
                mcbLoanStudent.ValueMember = "Id";
                mcbLoanStudent.DisplayMember = "Description";
                mcbLoanStudent.DataSource = studentLists[poleId];

                //exit
                return;
            }

            //load students
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
                //list of students to be displayed
                List<IdDescriptionStatus> students = null;

                //check selected pole
                if (poleId <= 0)
                {
                    //get list of all active students
                    students = songChannel.ListStudentsByStatus((int)ItemStatus.Active);
                }
                else
                {
                    //get list of pole active students
                    students = songChannel.ListStudentsByPole(
                        poleId, (int)ItemStatus.Active);
                }

                //check result
                if (students[0].Result == (int)SelectResult.Success)
                {
                    //sort students by description
                    students.Sort((x, y) => x.Description.CompareTo(y.Description));
                }
                else if (students[0].Result == (int)SelectResult.Empty)
                {
                    //no student is available
                    //clear list
                    students.Clear();
                }
                else if (students[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting students
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Student, students[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Student,
                        students[0].ErrorMessage));

                    //clear list
                    students.Clear();
                }

                //set students to UI
                mcbLoanStudent.ValueMember = "Id";
                mcbLoanStudent.DisplayMember = "Description";
                mcbLoanStudent.DataSource = students;

                //store list for faster performance
                studentLists[poleId] = students;
            }
            catch (Exception ex)
            {
                //database error while getting students
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Student), ex);

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
        /// Refresh displayed loan datagrid.
        /// </summary>
        /// <param name="selectedRow">
        /// The index of the row to be selected.
        /// -1 if no row should be selected.
        /// </param>
        /// <param name="displayFirstRow">
        /// True if first row must be displayed.
        /// False if no specific row must be displayed.
        /// </param>
        private void RefreshLoans(int selectedRow, bool displayFirstRow)
        {
            //check if datagrid has not a source yet
            if (dgvLoans.DataSource == null)
            {
                //set source to datagrid
                dgvLoans.DataSource = dtLoans;
            }

            //check if first row should be displayed
            //and if the is at least one row
            if (displayFirstRow && dgvLoans.Rows.Count > 0)
            {
                //refresh grid by displaying first row
                dgvLoans.FirstDisplayedScrollingRowIndex = 0;
            }

            //refresh grid
            dgvLoans.Refresh();

            //clear default selection
            dgvLoans.ClearSelection();

            //check row to be selected
            if (selectedRow > -1)
            {
                //select row
                dgvLoans.Rows[selectedRow].Selected = true;
            }
        }

        /// <summary>
        /// Remove loan from the list of loans.
        /// </summary>
        /// <param name="loanID">
        /// The ID of the selected loan.
        /// </param>
        private void RemoveLoan(int loanID)
        {
            //lock list of loans
            lock (loans)
            {
                //remove loan
                loans.Remove(loanID);
            }
        }

        /// <summary>
        /// Set data row with selected Loan data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="loan">The selected loan.</param>
        private void SetLoanDataRow(DataRow dataRow, Loan loan)
        {
            dataRow["LoanId"] = loan.LoanId;
            dataRow["StudentName"] = loan.StudentName;
            dataRow["StartDate"] = loan.StartDate;
            dataRow["EndDate"] = loan.EndDate == DateTime.MinValue ? System.DBNull.Value : (object)loan.EndDate;
            dataRow["Comments"] = loan.Comments;
            dataRow["LoanStatusName"] = ((ItemStatus)loan.LoanStatus) == ItemStatus.Active ?
                Properties.Resources.wordOpen : Properties.Resources.wordClosed;
        }

        /// <summary>
        /// Validate input data.
        /// </summary>
        /// <returns>
        /// True if data is valid.
        /// </returns>
        private bool ValidateData()
        {
            //validate code
            if (!ValidateRequiredField(mtxtCode, mlblCode.Text, mtbTabManager, tbGeneralData))
            {
                //invalid field
                return false;
            }

            //validate model
            if (!ValidateRequiredField(mtxtModel, mlblModel.Text, mtbTabManager, tbGeneralData))
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
            //select first status
            mcbStatus.SelectedIndex = 0;

            //clear inactivation fields
            inactivationReason = string.Empty;
            inactivationTime = DateTime.MinValue;

            //clear text fields
            mtxtModel.Text = string.Empty;
            mtxtCode.Text = string.Empty;
            mtxtStorageLocation.Text = string.Empty;

            //select first instrument type
            mcbInstrumentType.SelectedIndex = 0;

            //check number of poles
            if (mcbPole.Items.Count > 0)
            {
                //select first pole
                mcbPole.SelectedIndex = 0;
            }

            //clear comment fields
            mtxtComment.Text = string.Empty;
            dgvComments.DataSource = new List<Comment>();
            
            //clear loans
            //load empty list
            DisplayLoans(new List<Loan>());
        }

        /// <summary>
        /// Dispose used resources from user control.
        /// </summary>
        public override void DisposeControl()
        {
            //update option to hide inactive items
            Manager.Settings.HideInactiveInstruments = this.hideInactiveItems;

            //unsubscribe from settings property changed event
            Manager.Settings.PropertyChanged -= Settings_PropertyChanged;
        }

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        /// <returns></returns>
        public override string SelectMenuOption()
        {
            //select instrument option
            return "Instrument";
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
            mtxtModel.Enabled = enable;
            mtxtCode.Enabled = enable;
            mtxtStorageLocation.Enabled = enable;

            //set instrument type
            mcbInstrumentType.Enabled = enable;

            //set pole list
            mcbPole.Enabled = enable;

            //set comment fields
            mtxtComment.Enabled = enable;
            mbtnAddComment.Enabled = enable && (mtxtComment.Text.Length > 0);
            dgvComments.Enabled = true;

            //set loan fields
            mbtnAddLoan.Enabled = enable;
            EnableLoanFields();
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
                //get selected instrument from web service
                Instrument instrument = songChannel.FindInstrument(itemId);

                //check result
                if (instrument.Result == (int)SelectResult.Empty)
                {
                    //should never happen
                    //could not load data
                    return false;
                }
                else if (instrument.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting instrument
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, instrument.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, instrument.ErrorMessage));

                    //could not load data
                    return false;
                }

                //set selected instrument ID
                selectedId = instrument.InstrumentId;

                //set inactivation fields
                inactivationReason = instrument.InactivationReason;
                inactivationTime = instrument.InactivationTime;

                //select status
                mcbStatus.SelectedValue = instrument.InstrumentStatus;

                //set text fields
                mtxtCode.Text = instrument.Code;
                mtxtModel.Text = instrument.Model;
                mtxtStorageLocation.Text = instrument.StorageLocation;

                //set instrument type
                mcbInstrumentType.SelectedValue = instrument.InstrumentType;

                //set pole
                mcbPole.SelectedValue = instrument.PoleId;

                //check selected index
                if (mcbPole.SelectedIndex < 0)
                {
                    try
                    {
                        //pole is not available
                        //it might be inactive
                        //must load pole from web service
                        Pole pole = songChannel.FindPole(instrument.PoleId);

                        //check result
                        if (pole.Result == (int)SelectResult.Success)
                        {
                            //add pole to list of poles
                            List<IdDescriptionStatus> poles =
                                (List<IdDescriptionStatus>)mcbPole.DataSource;
                            poles.Add(pole.GetDescription());

                            //update displayed list
                            mcbPole.DataSource = null;
                            mcbPole.ValueMember = "Id";
                            mcbPole.DisplayMember = "Description";
                            mcbPole.DataSource = poles;

                            //set pole
                            mcbPole.SelectedValue = instrument.PoleId;
                        }
                    }
                    catch
                    {
                        //do nothing
                    }
                }

                //get and display comments
                dgvComments.DataSource = instrument.GetComments();

                //clear default selection
                dgvComments.ClearSelection();

                #region load loans

                //list students for instrument pole
                ListStudents(instrument.PoleId);

                //get loans for selected instrument
                List<Loan> loans = songChannel.FindLoansByInstrument(
                    instrument.InstrumentId, -1);

                //check result
                if (loans[0].Result == (int)SelectResult.Empty)
                {
                    //instrument has no loan
                    //clear list
                    loans.Clear();
                }
                else if (loans[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting loans
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadLoans,
                        itemTypeDescription, loans[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadLoans,
                        itemTypeDescription, loans[0].ErrorMessage));

                    //could not load data
                    return false;
                }

                //sort loans by description
                loans.Sort((x, y) => y.StartDate.CompareTo(x.StartDate));

                //display loans
                DisplayLoans(loans);

                //clear selection
                mcbLoanStudent.SelectedIndex = -1;

                #endregion load loans
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
        /// Load instrument list from database.
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
                //list of instruments
                List<IdDescriptionStatus> instruments = null;

                //check if logged on user has an assigned institution
                if (Manager.LogonUser != null &&
                    Manager.LogonUser.InstitutionId > 0)
                {
                    //get list of instruments for assigned institution
                    instruments = songChannel.ListInstrumentsByInstitution(
                        Manager.LogonUser.InstitutionId, -1);
                }
                else
                {
                    //get list of all instruments
                    instruments = songChannel.ListInstruments();
                }

                //check result
                if (instruments[0].Result == (int)SelectResult.Success)
                {
                    //instruments were found
                    //check each instrument
                    foreach (IdDescriptionStatus instrument in instruments)
                    {
                        //set instrument description
                        Manager.FormatInstrumentDescription(instrument);
                    }

                    //return instruments
                    return instruments;
                }
                else if (instruments[0].Result == (int)SelectResult.Empty)
                {
                    //no instrument is available
                    //clear list
                    instruments.Clear();

                    //return instruments
                    return instruments;
                }
                else if (instruments[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting instruments
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        itemTypeDescription, instruments[0].ErrorMessage));

                    //could not get instruments
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

            //could not get instruments
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
                //inactivate selected instrument and get result
                DeleteResult result = songChannel.InactivateInstrument(
                    SelectedItemId, reasonForm.InactivationReason);

                //check result
                if (result.Result == (int)SelectResult.Success)
                {
                    //item was inactivated
                    //check if there is a parent control
                    //and if it is an instrument register control
                    if (parentControl != null && parentControl is ViewInstrumentControl)
                    {
                        //update instrument to inactive in parent control
                        ((ViewInstrumentControl)parentControl).UpdateInstrument(
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
            //select first tab
            mtbTabManager.SelectedIndex = 0;
            
            //focus instrument type field
            mcbInstrumentType.Focus();
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

            //create an instrument and set data
            Instrument instrument = new Instrument();

            //set selected instrument ID
            instrument.InstrumentId = selectedId;

            //check selected status
            if (mcbStatus.SelectedIndex >= 0)
            {
                //set status
                instrument.InstrumentStatus = (int)mcbStatus.SelectedValue;

                //check if selected status is inactive or lost
                if ((int)mcbStatus.SelectedValue == (int)ItemStatus.Inactive ||
                    (int)mcbStatus.SelectedValue == (int)ItemStatus.Lost)
                {
                    //create inactivation reason form
                    InactivationReasonForm reasonForm = new InactivationReasonForm(
                        itemTypeDescription, (int)mcbStatus.SelectedValue, inactivationReason);

                    //let user input an inactivation reason
                    reasonForm.ShowDialog(this);

                    //set inactivation reason with result
                    instrument.InactivationReason = reasonForm.InactivationReason;

                    //set inactivation time
                    instrument.InactivationTime = (inactivationTime != DateTime.MinValue) ?
                        inactivationTime : DateTime.Now;
                }
                else
                {
                    //reset inactivation
                    instrument.InactivationReason = string.Empty;
                    instrument.InactivationTime = DateTime.MinValue;
                }
            }
            else
            {
                //should never happen
                //set default status
                instrument.InstrumentStatus = (int)ItemStatus.Active;

                //reset inactivation
                instrument.InactivationTime = DateTime.MinValue;
                instrument.InactivationReason = string.Empty;
            }

            //set text fields
            instrument.Code = mtxtCode.Text.Trim();
            instrument.Model = mtxtModel.Text.Trim();
            instrument.StorageLocation = mtxtStorageLocation.Text.Trim();

            //set instrument type
            instrument.InstrumentType = (int)mcbInstrumentType.SelectedValue;

            //set pole
            instrument.PoleId = (int)mcbPole.SelectedValue;

            //set pole name to properly display instrument in datagridview
            instrument.PoleName = ((IdDescriptionStatus)mcbPole.SelectedItem).Description;

            //set comments
            instrument.SetComments((List<Comment>)dgvComments.DataSource);
            
            //gather list of loans to be saved
            List<Loan> saveLoans = new List<Loan>();

            //check each loan row
            for (int i = 0; i < dtLoans.Rows.Count; i++)
            {
                //get loan for current row
                Loan loan = FindLoan(
                    (int)dtLoans.Rows[i][columnIndexLoanId]);

                //add loan to be saved
                saveLoans.Add(loan);
            }
            
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not save instrument
                return null;
            }

            try
            {
                //save instrument and get result
                SaveResult saveResult = songChannel.SaveInstrument(instrument, saveLoans);

                //check result
                if (saveResult.Result == (int)SelectResult.FatalError)
                {
                    //instrument was not saved
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

                    //could not save instrument
                    return null;
                }

                //set saved ID to instrument ID
                instrument.InstrumentId = saveResult.SavedId;

                //set selected ID
                selectedId = saveResult.SavedId;

                //check if there is a parent control
                //and if it is an instrument register control
                if (parentControl != null && parentControl is ViewInstrumentControl)
                {
                    //update instrument in parent control
                    ((ViewInstrumentControl)parentControl).UpdateInstrument(instrument);
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

            //instrument was saved
            //get description
            IdDescriptionStatus instrumentDescription = instrument.GetDescription();

            //set description
            Manager.FormatInstrumentDescription(instrumentDescription);

            //return updated description
            return instrumentDescription;
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
                //could not copy instrument
                return null;
            }

            try
            {
                //copy selected instrument and get result
                Instrument instrument = songChannel.CopyInstrument(SelectedItemId);

                //check instrument copy
                if (instrument.Result == (int)SelectResult.FatalError)
                {
                    //instrument was not copied
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceCopyItem,
                        itemTypeDescription, instrument.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceCopyItem,
                        itemTypeDescription, instrument.ErrorMessage));

                    //could not copy instrument
                    return null;
                }

                //check if there is a parent control
                //and if it is an instrument register control
                if (parentControl != null && parentControl is ViewInstrumentControl)
                {
                    //update instrument in parent control
                    ((ViewInstrumentControl)parentControl).UpdateInstrument(instrument);
                }

                //get description
                IdDescriptionStatus description = instrument.GetDescription();

                //format description
                Manager.FormatInstrumentDescription(description);

                //instrument was copied
                //return description
                return description;
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


        #region Event Handlers ********************************************************

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
                dgvComments.ColumnHeadersDefaultCellStyle.Font =
                    MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
                dgvComments.DefaultCellStyle.Font =
                    MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
                dgvLoans.ColumnHeadersDefaultCellStyle.Font =
                    MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
                dgvLoans.DefaultCellStyle.Font =
                    MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            }
        }

        #endregion Event Handlers


        #region UI Event Handlers *****************************************************

        /// <summary>
        /// Register insitution 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterInstrument_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set font to UI controls
            mtxtLoanEndDate.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtLoanStartDate.Font = MetroFramework.MetroFonts.Default(13.0F);

            //set font to datagridviews
            dgvComments.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvComments.DefaultCellStyle.Font =
                MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            dgvLoans.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvLoans.DefaultCellStyle.Font =
                MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);

            //display first tab
            mtbTabManager.SelectedIndex = 0;
        }

        /// <summary>
        /// Add comment button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddComment_Click(object sender, EventArgs e)
        {
            //create a new comment to the list of comments
            Comment comment = new Comment();
            comment.Date = DateTime.Today;
            comment.Text = mtxtComment.Text;

            //get list of comments
            List<Comment> comments = (List<Comment>)dgvComments.DataSource;

            //add new comment
            comments.Insert(0, comment);

            //update displayed comments
            dgvComments.DataSource = null;
            dgvComments.DataSource = comments;

            //clear comment text
            mtxtComment.Text = string.Empty;
        }

        /// <summary>
        /// Comment textbox text changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtComment_TextChanged(object sender, EventArgs e)
        {
            //enable add comment button if text is set
            mbtnAddComment.Enabled = mtxtComment.Enabled && (mtxtComment.Text.Length > 0);
        }

        /// <summary>
        /// Comments datagridview mouse up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvComments_MouseUp(object sender, MouseEventArgs e)
        {
            //check control status
            if (this.Status == RegisterStatus.Consulting)
            {
                //can't delete while consulting
                //exit
                return;
            }

            //show menu only if the right mouse button is clicked.
            if (!(e.Button == MouseButtons.Right))
            {
                //exit
                return;
            }

            //get clicked point and which index was selected
            Point p = new Point(e.X, e.Y);

            //get clicked cell
            DataGridView.HitTestInfo hitInfo = dgvComments.HitTest(e.X, e.Y);

            //check selected row index
            if (hitInfo.RowIndex < 0)
            {
                //no row selected
                //exit
                return;
            }

            //check if there are selected rows and if comment clicked on them
            if (dgvComments.SelectedRows.Count > 0 &&
                dgvComments.Rows[hitInfo.RowIndex].Selected != true)
            {
                //comment did not click in the selected rows
                //clear selection
                dgvComments.ClearSelection();

                //select clicked row
                dgvComments.Rows[hitInfo.RowIndex].Selected = true;
            }
            //check if there are selected cells
            else if (dgvComments.SelectedCells.Count > 0)
            {
                //get list of selected cell rows
                HashSet<int> selectedRowIndexes = new HashSet<int>();

                //check each selected cell
                foreach (DataGridViewCell cell in dgvComments.SelectedCells)
                {
                    //add cell row index to the list
                    selectedRowIndexes.Add(cell.RowIndex);
                }

                //check if comment clicked on a row of a selected cell
                if (selectedRowIndexes.Contains(hitInfo.RowIndex))
                {
                    //comment clicked on a row of a selected cell
                    //select all rows
                    foreach (int selectedRowIndex in selectedRowIndexes)
                    {
                        //select row
                        dgvComments.Rows[selectedRowIndex].Selected = true;
                    }
                }
                else
                {
                    //comment did not click on a row of a selected cell
                    //clear selected cells
                    dgvComments.ClearSelection();

                    //select clicked row
                    dgvComments.Rows[hitInfo.RowIndex].Selected = true;
                }
            }
            else
            {
                //select clicked row
                dgvComments.Rows[hitInfo.RowIndex].Selected = true;
            }

            //check if there is no selected comment
            if (dgvComments.SelectedRows.Count == 0)
            {
                //there is no comment selected
                //should never happen
                //do not display context menu
                //exit
                return;
            }

            //show comment context menu on the clicked point
            mcmComment.Show(this.dgvComments, p);
        }

        /// <summary>
        /// Comments datagridview key up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvComments_KeyUp(object sender, KeyEventArgs e)
        {
            //check control status
            if (this.Status == RegisterStatus.Consulting)
            {
                //can't delete while consulting
                //exit
                return;
            }

            //check key
            if (e.KeyCode != Keys.Delete &&
                e.KeyCode != Keys.Back)
            {
                //ignore key
                //exit
                return;
            }

            //check selected comment
            if (dgvComments.SelectedCells.Count == 0)
            {
                //no comment is selected
                //exit
                return;
            }

            //get list of comments
            List<Comment> comments = (List<Comment>)dgvComments.DataSource;

            //remove comment
            comments.RemoveAt(dgvComments.SelectedCells[0].RowIndex);

            //update displayed comments
            dgvComments.DataSource = null;
            dgvComments.DataSource = comments;
            dgvComments.ClearSelection();
        }

        /// <summary>
        /// Edit comment menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuEditComment_Click(object sender, EventArgs e)
        {
            //check control status
            if (this.Status == RegisterStatus.Consulting)
            {
                //can't delete while consulting
                //exit
                return;
            }

            //check selected comment
            if (dgvComments.SelectedCells.Count == 0)
            {
                //no comment is selected
                //exit
                return;
            }

            //get selected comment index
            int commentIndex = dgvComments.SelectedCells[0].RowIndex;

            //get list of comments
            List<Comment> comments = (List<Comment>)dgvComments.DataSource;

            //create form to edit comment
            EditCommentForm editForm = new EditCommentForm(comments[commentIndex].Text);

            //display form and check result
            if (editForm.ShowDialog(Manager.MainForm) != DialogResult.OK)
            {
                //user canceled operation
                //exit
                return;
            }

            //update comment
            comments[commentIndex].Text = editForm.Comment;

            //update displayed comments
            dgvComments.DataSource = null;
            dgvComments.DataSource = comments;
            dgvComments.Rows[commentIndex].Selected = true;

            //focus comments
            dgvComments.Focus();
        }

        /// <summary>
        /// Delete comment menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDeleteComment_Click(object sender, EventArgs e)
        {
            //check control status
            if (this.Status == RegisterStatus.Consulting)
            {
                //can't delete while consulting
                //exit
                return;
            }

            //check selected comment
            if (dgvComments.SelectedCells.Count == 0)
            {
                //no comment is selected
                //exit
                return;
            }

            //ask user to confirm operation
            if (MetroMessageBox.Show(Manager.MainForm,
                Properties.Resources.msgDeleteComment,
                Properties.Resources.titleDeleteComment,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                //user canceled operation
                //exit
                return;
            }

            //get list of comments
            List <Comment> comments = (List<Comment>)dgvComments.DataSource;

            //remove comment
            comments.RemoveAt(dgvComments.SelectedCells[0].RowIndex);

            //update displayed comments
            dgvComments.DataSource = null;
            dgvComments.DataSource = comments;
            dgvComments.ClearSelection();

            //focus comments
            dgvComments.Focus();
        }

        /// <summary>
        /// Pole combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbPole_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check selected index
            if (mcbPole.SelectedIndex == -1)
            {
                //should never happen
                //exit
                return;
            }

            //check register status
            if (this.Status == RegisterStatus.Consulting)
            {
                //just consulting
                //list is already set when loading item
                //exit
                return;
            }

            //clear any loan selection
            dgvLoans.ClearSelection();

            //list students for selected pole
            ListStudents((int)mcbPole.SelectedValue);
        }

        /// <summary>
        /// Add loan click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddLoan_Click(object sender, EventArgs e)
        {
            //check if there is an active loan
            foreach (Loan previousLoan in loans.Values)
            {
                //check loan status
                if (previousLoan.LoanStatus == (int)ItemStatus.Active)
                {
                    //there is already an active loan
                    //display message
                    MetroMessageBox.Show(Manager.MainForm,
                        Properties.Resources.msgPreviousOpenLoan,
                        Properties.Resources.titleUnavailableInstrument,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //exit
                    return;
                }
            }

            //check if there is no listed
            if (mcbLoanStudent.Items.Count == 0)
            {
                //no student is available for selection
                //no student must have been created
                //should hardly ever happen
                //display message
                MetroMessageBox.Show(Manager.MainForm,
                    Properties.Resources.msgPoleNoStudent,
                    Properties.Resources.titleNoStudent,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //exit
                return;
            }

            //next available student without loan
            //must consult database
            Student nextStudent = null;

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            try
            {
                //check result and if there is a selected pole
                if (songChannel != null && mcbPole.SelectedIndex >= 0)
                {
                    //get next available student without loan
                    nextStudent = songChannel.FindNextStudentWithoutLoan(
                        (int)mcbPole.SelectedValue);

                    //check result
                    if (nextStudent.Result == (int)SelectResult.Success)
                    {
                        //next student was found
                        //will select student for new loan down below
                    }
                    else if (nextStudent.Result == (int)SelectResult.Empty)
                    {
                        //no student is available
                        //display message
                        MetroMessageBox.Show(Manager.MainForm,
                            Properties.Resources.msgPoleNoStudentWithoutLoan,
                            Properties.Resources.titleNoStudent,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        //exit
                        return;
                    }
                    else
                    {
                        //error while getting next student
                        //should never happen
                        //just select first student
                        nextStudent = null;
                    }
                }
            }
            catch (Exception ex)
            {
                //database error while getting poles
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelGetNextAvailableItem,
                    Properties.Resources.item_Student), ex);

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

            //create and set loan
            Loan loan = new Loan();
            loan.LoanId = -1;
            loan.InstrumentId = selectedId;
            loan.CreationTime = DateTime.Now;
            loan.LoanStatus = (int)ItemStatus.Active;
            loan.StartDate = DateTime.Today;
            loan.EndDate = DateTime.MinValue;

            //check if next student was found
            if (nextStudent != null)
            {
                //set next student to loan
                loan.StudentId = nextStudent.Id;
                loan.StudentName = nextStudent.Name;
            }
            else
            {
                //set first student to loan
                IdDescriptionStatus student = (IdDescriptionStatus)mcbLoanStudent.Items[0];
                loan.StudentId = student.Id;
                loan.StudentName = student.Description;
            }

            //lock list of loans
            lock (loans)
            {
                //check if id is taken
                while (loans.ContainsKey(loan.LoanId))
                {
                    //decrement id
                    loan.LoanId--;
                }

                //add loan to the list
                loans.Add(loan.LoanId, loan);

                //set last found loan
                lastFoundLoan = loan;
            }

            //lock data table of loans
            lock (dtLoans)
            {
                //create, set and add loan row
                DataRow dr = dtLoans.NewRow();
                SetLoanDataRow(dr, loan);
                dtLoans.Rows.InsertAt(dr, 0);
            }

            //refresh displayed loans
            //select inserted loan
            RefreshLoans(0, true);
        }
        
        /// <summary>
        /// Delete loan click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnDeleteLoan_Click(object sender, EventArgs e)
        {
            //check if there is a selected loan
            if (dgvLoans.SelectedRows.Count == 0)
            {
                //no selected loan
                //exit
                return;
            }

            //get selected index
            int selectedIndex = dgvLoans.SelectedRows[0].Index;

            //get row
            DataRow row = dtLoans.Rows[selectedIndex];

            //get loan
            Loan loan = FindLoan((int)row[columnIndexLoanId]);

            //ask user to confirm deletion and get result
            DialogResult dr = MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgConfirmDeleteLoan, loan.InstrumentCode, loan.StudentName),
                Properties.Resources.cptConfirm,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            //check result
            if (dr == DialogResult.Cancel)
            {
                //user canceled operation
                //exit
                return;
            }

            //remove loan
            RemoveLoan(loan.LoanId);
            dtLoans.Rows.RemoveAt(selectedIndex);

            //clear selection
            dgvLoans.ClearSelection();

            //refresh grid
            dgvLoans.Refresh();
        }

        /// <summary>
        /// Loans datagridview selection changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLoans_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //set loading flag
                isLoadingLoan = true;

                //check if there is a selected loan
                if (dgvLoans.SelectedRows.Count > 0)
                {
                    //get loan using its id
                    Loan loan = FindLoan((int)dgvLoans.SelectedRows[0].Cells[columnIndexLoanId].Value);

                    //check result
                    if (loan != null)
                    {
                        //select student
                        mcbLoanStudent.SelectedValue = loan.StudentId;

                        //select status
                        mcbLoanStatus.SelectedValue = (int)loan.LoanStatus;

                        //set start date
                        mtxtLoanStartDate.Text = loan.StartDate.ToShortDateString();

                        //set end date
                        mtxtLoanEndDate.Text = loan.EndDate == DateTime.MinValue ?
                            string.Empty : loan.EndDate.ToShortDateString();
                    }
                    else
                    {
                        //loan was not found
                        //should never happen
                        //clear fields
                        ClearLoanFields();
                    }
                }
                else
                {
                    //clear fields
                    ClearLoanFields();
                }

                //enable/disable loan fields
                EnableLoanFields();
            }
            finally
            {
                //reset loading flag
                isLoadingLoan = false;
            }
        }

        /// <summary>
        /// Loan student combo box index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbLoanStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if is loading loan 
            if (isLoadingLoan)
            {
                //is loading
                //exit
                return;
            }

            //check selected index
            if (mcbLoanStudent.SelectedIndex == -1)
            {
                //no student is selected
                //exit
                return;
            }

            //check if there is a selected loan
            if (dgvLoans.SelectedRows.Count == 0)
            {
                //no selected
                //exit
                return;
            }

            //get loan using its id
            Loan loan = FindLoan((int)dgvLoans.SelectedRows[0].Cells[columnIndexLoanId].Value);

            //check result
            if (loan == null)
            {
                //loan was not found
                //should never happen
                //exit
                return;
            }

            //check selected student
            if (loan.StudentId == (int)mcbLoanStudent.SelectedValue)
            {
                //same selected student
                //exit
                return;
            }

            //check if student has an ongoing loan
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel != null)
            {
                try
                {
                    //get list of active loans for selected student
                    List<Loan> studentLoans = songChannel.FindLoansByStudent(
                        (int)mcbLoanStudent.SelectedValue, (int)ItemStatus.Active);

                    //check result
                    if (studentLoans[0].Result == (int)SelectResult.Success)
                    {
                        //student has an ongoing loan
                        //display message
                        MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.msgStudentAlreadyHasLoan, 
                            studentLoans[0].InstrumentCode,
                            ((IdDescriptionStatus)mcbLoanStudent.SelectedItem).Description),
                            Properties.Resources.wordLoan,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        //set loading flag
                        isLoadingLoan = true;

                        //reselect original student
                        mcbLoanStudent.SelectedValue = loan.StudentId;

                        //reset loading flag
                        isLoadingLoan = false;

                        //no need to update student
                        //exit
                        return;
                    }
                }
                catch (Exception ex)
                {
                    //database error while getting loans
                    //write error
                    Manager.Log.WriteException(string.Format(
                        Properties.Resources.errorChannelListItem,
                        Properties.Resources.item_Loan), ex);

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


            //update loan student
            loan.StudentId = (int)mcbLoanStudent.SelectedValue;
            loan.StudentName = ((IdDescriptionStatus)mcbLoanStudent.SelectedItem).Description;

            //lock data table of loans
            lock (dtLoans)
            {
                //get data row for selected loan
                DataRow dr = dtLoans.Rows[dgvLoans.SelectedRows[0].Index];

                //set data row
                SetLoanDataRow(dr, loan);
            }

            //refresh grid
            dgvLoans.Refresh();
        }

        /// <summary>
        /// Loan status combo box index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbLoanStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if is loading loan 
            if (isLoadingLoan)
            {
                //is loading
                //exit
                return;
            }

            //check if there is a selected loan
            if (dgvLoans.SelectedRows.Count == 0)
            {
                //no selected
                //exit
                return;
            }

            //get loan using its id
            Loan loan = FindLoan((int)dgvLoans.SelectedRows[0].Cells[columnIndexLoanId].Value);

            //check result
            if (loan == null)
            {
                //loan was not found
                //should never happen
                //exit
                return;
            }

            //check selected status
            if (loan.LoanStatus == (int)mcbLoanStatus.SelectedValue)
            {
                //same selected status
                //exit
                return;
            }
            else if ((int)ItemStatus.Inactive == (int)mcbLoanStatus.SelectedValue)
            {
                //user is closing loan
                //ask user to confirm action
                MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.msgConfirmCloseLoan, loan.StudentName),
                    Properties.Resources.titleCloseLoan,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }

            //update loan status
            loan.LoanStatus = (int)mcbLoanStatus.SelectedValue;

            //check if loan needs an end date
            if (loan.LoanStatus == (int)ItemStatus.Inactive &&
                loan.EndDate == DateTime.MinValue)
            {
                //check start date
                if (loan.StartDate < DateTime.Today)
                {
                    //set end date to today
                    loan.EndDate = DateTime.Today;
                }
                else
                {
                    //set end date to start date + 1
                    loan.EndDate = loan.StartDate.AddDays(1);
                }

                try
                {
                    //set loading flag
                    isLoadingLoan = true;

                    //load loan end date
                    mtxtLoanEndDate.Text = loan.EndDate.ToShortDateString();
                }
                finally
                {
                    //reset loading flag
                    isLoadingLoan = false;
                }
            }

            //lock data table of loans
            lock (dtLoans)
            {
                //get data row for selected loan
                DataRow dr = dtLoans.Rows[dgvLoans.SelectedRows[0].Index];

                //set data row
                SetLoanDataRow(dr, loan);
            }

            //refresh grid
            dgvLoans.Refresh();
        }

        /// <summary>
        /// Date masked text box click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Date_Click(object sender, EventArgs e)
        {
            //get sender textbox
            MaskedTextBox mtxtDate = (MaskedTextBox)sender;

            //check text
            if (mtxtDate.Text.Equals("  /  /"))
            {
                //set cursor position
                mtxtDate.Select(0, 0);
            }
        }

        /// <summary>
        /// Loan start date text changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtLoanStartDate_TextChanged(object sender, EventArgs e)
        {
            //check if is loading loan 
            if (isLoadingLoan)
            {
                //is loading
                //exit
                return;
            }

            //check if there is a selected loan
            if (dgvLoans.SelectedRows.Count == 0)
            {
                //no selected
                //exit
                return;
            }

            //check if a date was entered
            if (mtxtLoanStartDate.Text.Length < 10)
            {
                //incomplete date
                //exit
                return;
            }

            //get entered date
            DateTime newStartDate;

            try
            {
                //check if entered data is valid
                newStartDate = DateTime.Parse(mtxtLoanStartDate.Text);
            }
            catch
            {
                //invalid date
                //exit
                return;
            }

            //get loan using its id
            Loan loan = FindLoan((int)dgvLoans.SelectedRows[0].Cells[columnIndexLoanId].Value);

            //check result
            if (loan == null)
            {
                //loan was not found
                //should never happen
                //exit
                return;
            }

            //check entered start date
            if (loan.StartDate == newStartDate)
            {
                //same entered start date
                //exit
                return;
            }

            //previous loan
            Loan previousLoan = null;

            //check if there is a previous loan in the datagrid
            if (dgvLoans.SelectedRows[0].Index < dgvLoans.Rows.Count - 1)
            {
                //get previous loan
                previousLoan = FindLoan(
                    (int)dgvLoans.Rows[dgvLoans.SelectedRows[0].Index + 1].Cells[columnIndexLoanId].Value);
            }
            
            //check if there is a previous loan
            //and if new start date is earlier than previous loan end date
            if (previousLoan != null && newStartDate <= previousLoan.EndDate)
            {
                //display message
                MetroMessageBox.Show(Manager.MainForm,
                    Properties.Resources.msgStartDateLaterThanPreviousEndDate,
                    Properties.Resources.wordLoan,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //update new end date to previous loan end date plus one day
                newStartDate = previousLoan.EndDate.AddDays(1);

                //set loading flag
                isLoadingLoan = true;

                //update start date textbox
                mtxtLoanStartDate.Text = newStartDate.ToShortDateString();

                //reset loading flag
                isLoadingLoan = false;
            }

            //update start date
            loan.StartDate = newStartDate;

            //lock data table of loans
            lock (dtLoans)
            {
                //get data row for selected loan
                DataRow dr = dtLoans.Rows[dgvLoans.SelectedRows[0].Index];

                //set data row
                SetLoanDataRow(dr, loan);
            }

            //refresh grid
            dgvLoans.Refresh();
        }

        /// <summary>
        /// Loan end date text changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtLoanEndDate_TextChanged(object sender, EventArgs e)
        {
            //check if is loading loan 
            if (isLoadingLoan)
            {
                //is loading
                //exit
                return;
            }

            //check if there is a selected loan
            if (dgvLoans.SelectedRows.Count == 0)
            {
                //no selected
                //exit
                return;
            }

            //get entered date
            DateTime newEndDate;

            //check if a date was entered
            if (mtxtLoanEndDate.Text.Length < 10)
            {
                //incomplete date
                //check if user entered empty data
                //and if loan status is active
                if (mtxtLoanEndDate.Text.Equals("  /  /") &&
                    (int)mcbLoanStatus.SelectedValue == (int)ItemStatus.Active)
                {
                    //user entered empty date
                    newEndDate = DateTime.MinValue;
                }
                else
                {
                    //invalid date
                    //exit
                    return;
                }
            }
            else
            {

                try
                {
                    //check if entered data is valid
                    newEndDate = DateTime.Parse(mtxtLoanEndDate.Text);
                }
                catch
                {
                    //invalid date
                    //exit
                    return;
                }
            }

            //get loan using its id
            Loan loan = FindLoan((int)dgvLoans.SelectedRows[0].Cells[columnIndexLoanId].Value);

            //check result
            if (loan == null)
            {
                //loan was not found
                //should never happen
                //exit
                return;
            }

            //check entered end date
            if (loan.EndDate == newEndDate)
            {
                //same entered end date
                //exit
                return;
            }

            //check if new end date is earlier than start date
            if (newEndDate != DateTime.MinValue && newEndDate <= loan.StartDate)
            {
                //display message
                MetroMessageBox.Show(Manager.MainForm,
                    Properties.Resources.msgEndDateLaterThanStartDate,
                    Properties.Resources.wordLoan,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //update new end date to start date plus one day
                newEndDate = loan.StartDate.AddDays(1);

                //set loading flag
                isLoadingLoan = true;

                //update end date textbox
                mtxtLoanEndDate.Text = newEndDate.ToShortDateString();

                //reset loading flag
                isLoadingLoan = false;
            }

            //update end date
            loan.EndDate = newEndDate;

            //lock data table of loans
            lock (dtLoans)
            {
                //get data row for selected loan
                DataRow dr = dtLoans.Rows[dgvLoans.SelectedRows[0].Index];

                //set data row
                SetLoanDataRow(dr, loan);
            }

            //refresh grid
            dgvLoans.Refresh();
        }

        #endregion UI Event Handlers

    } //end of class RegisterInstrumentControl

} //end of namespace PnT.SongClient.UI.Controls
