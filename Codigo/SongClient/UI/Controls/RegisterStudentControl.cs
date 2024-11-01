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
    /// This control is used to manage student registries in web service.
    /// Inherits from base register control.
    /// </summary>
    public partial class RegisterStudentControl : RegisterBaseControl
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
        /// The file path of the student assignment file.
        /// </summary>
        private string assignmentFile = string.Empty;

        /// <summary>
        /// The file path of the student photo.
        /// </summary>
        private string photoFile = string.Empty;

        /// <summary>
        /// The list of registrations for the selected student.
        /// </summary>
        private Dictionary<int, Registration> registrations = null;

        /// <summary>
        /// DataTable for registrations.
        /// </summary>
        private DataTable dtRegistrations = null;

        /// <summary>
        /// The last found registration.
        /// Used to improve the find method.
        /// </summary>
        private Registration lastFoundRegistration = null;

        /// <summary>
        /// True if a registration is being loaded into UI.
        /// </summary>
        private bool isLoadingRegistration = false;

        /// <summary>
        /// The registration ID column index in the datagridview.
        /// </summary>
        private int columnIndexRegistrationId;

        /// <summary>
        /// The list of loans for the selected student.
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
        /// The list of all loaded instrument lists.
        /// Keep lists for better performance.
        /// </summary>
        private Dictionary<int, List<IdDescriptionStatus>> instrumentLists = null;

        /// <summary>
        /// The loan ID column index in the datagridview.
        /// </summary>
        private int columnIndexLoanId;

        #endregion Fields


        #region Constructors **********************************************************
        
        public RegisterStudentControl() : base("Student", Manager.Settings.HideInactiveStudents)
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

            //student cannot be deleted
            this.classHasDeletion = false;

            //set permissions
            this.allowAddItem = Manager.HasLogonPermission("Student.Insert");
            this.allowEditItem = Manager.HasLogonPermission("Student.Edit");
            this.allowDeleteItem = Manager.HasLogonPermission("Student.Inactivate");

            //create list of registrations
            registrations = new Dictionary<int, Registration>();

            //create registration data table
            CreateRegistrationDataTable();

            //get registration ID column index
            columnIndexRegistrationId = dgvRegistrations.Columns[RegistrationId.Name].Index;

            //avoid auto generated columns
            dgvRegistrations.AutoGenerateColumns = false;

            //create list of loans
            loans = new Dictionary<int, Loan>();

            //create list of loaded instruments
            instrumentLists = new Dictionary<int, List<IdDescriptionStatus>>();

            //create loan data table
            CreateLoanDataTable();

            //get loan ID column index
            columnIndexLoanId = dgvLoans.Columns[LoanId.Name].Index;

            //avoid auto generated columns
            dgvLoans.AutoGenerateColumns = false;

            //load combos
            //list statuses
            List<KeyValuePair<int, string>> statuses = new List<KeyValuePair<int, string>>();
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Active, Properties.Resources.ResourceManager.GetString("ItemStatus_Active")));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Inactive, Properties.Resources.ResourceManager.GetString("ItemStatus_Inactive")));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Evaded, Properties.Resources.ResourceManager.GetString("ItemStatus_Evaded")));
            mcbStatus.ValueMember = "Key";
            mcbStatus.DisplayMember = "Value";
            mcbStatus.DataSource = statuses;

            //list registration statuses
            statuses = new List<KeyValuePair<int, string>>();
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Active, Properties.Resources.ResourceManager.GetString("ItemStatus_Active")));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Evaded, Properties.Resources.ResourceManager.GetString("ItemStatus_Evaded")));
            mcbRegistrationStatus.ValueMember = "Key";
            mcbRegistrationStatus.DisplayMember = "Value";
            mcbRegistrationStatus.DataSource = statuses;

            //list loan statuses
            statuses = new List<KeyValuePair<int, string>>();
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Active, Properties.Resources.wordOpen));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Inactive, Properties.Resources.wordClosed));
            mcbLoanStatus.ValueMember = "Key";
            mcbLoanStatus.DisplayMember = "Value";
            mcbLoanStatus.DataSource = statuses;

            //load states
            mcbState.ValueMember = "Key";
            mcbState.DisplayMember = "Value";
            mcbState.DataSource = ListStates();

            //list poles
            ListPoles();
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

        /// <summary>
        /// Get list of registrations.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public List<Registration> ListRegistrations
        {
            get
            {
                //lock list of registrations
                lock (registrations)
                {
                    return new List<Registration>(registrations.Values);
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
            //clear instrument selection
            mcbLoanInstrument.SelectedIndex = -1;

            //clear status selection
            mcbLoanStatus.SelectedIndex = -1;

            //clear dates
            mtxtLoanStartDate.Text = string.Empty;
            mtxtLoanEndDate.Text = string.Empty;
        }

        /// <summary>
        /// Clear registration fields.
        /// </summary>
        private void ClearRegistrationFields()
        {
            //clear class code 
            mtxtRegistrationClass.Text = string.Empty;

            //clear status selection
            mcbRegistrationStatus.SelectedIndex = -1;

            //clear auto renewal option
            mcbRegistrationAutoRenewal.Checked = false;
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

            //Instrument
            DataColumn dcInstrument = new DataColumn("InstrumentCode", typeof(string));
            dtLoans.Columns.Add(dcInstrument);

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
        /// Create Registration data table.
        /// </summary>
        private void CreateRegistrationDataTable()
        {
            //create data table
            dtRegistrations = new DataTable();

            //RegistrationId
            DataColumn dcRegistrationId = new DataColumn("RegistrationId", typeof(int));
            dtRegistrations.Columns.Add(dcRegistrationId);

            //Class
            DataColumn dcClass = new DataColumn("ClassCode", typeof(string));
            dtRegistrations.Columns.Add(dcClass);

            //PositionStatus
            DataColumn dcPositionStatus = new DataColumn("PositionStatus", typeof(string));
            dtRegistrations.Columns.Add(dcPositionStatus);

            //PositionWaiting
            DataColumn dcPositionWaiting = new DataColumn("PositionWaiting", typeof(bool));
            dtRegistrations.Columns.Add(dcPositionWaiting);

            //RegistrationStatusValue
            DataColumn dcRegistrationStatusValue = new DataColumn("RegistrationStatusValue", typeof(int));
            dtRegistrations.Columns.Add(dcRegistrationStatusValue);

            //RegistrationStatusName
            DataColumn dcRegistrationStatusName = new DataColumn("RegistrationStatusName", typeof(string));
            dtRegistrations.Columns.Add(dcRegistrationStatusName);

            //AutoRenewal
            DataColumn dcAutoRenewal = new DataColumn("AutoRenewal", typeof(string));
            dtRegistrations.Columns.Add(dcAutoRenewal);

            //set primary key column
            dtRegistrations.PrimaryKey = new DataColumn[] { dcRegistrationId };
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
        /// Display selected registrations.
        /// Clear currently displayed registrations before loading selected registrations.
        /// </summary>
        /// <param name="selectedRegistrations">
        /// The selected registrations to be loaded.
        /// </param>
        private void DisplayRegistrations(List<Registration> selectedRegistrations)
        {
            //lock list of registrations
            lock (this.registrations)
            {
                //clear list
                this.registrations.Clear();

                //reset last found registration
                lastFoundRegistration = null;
            }

            //lock datatable of registrations
            lock (dtRegistrations)
            {
                //clear datatable
                dtRegistrations.Clear();
            }

            //check number of selected registrations
            if (selectedRegistrations != null && selectedRegistrations.Count > 0 &&
                selectedRegistrations[0].Result == (int)SelectResult.Success)
            {
                //lock list of registrations
                lock (registrations)
                {
                    //add selected registrations
                    foreach (Registration registration in selectedRegistrations)
                    {
                        //check if registration is not in the list
                        if (!registrations.ContainsKey(registration.RegistrationId))
                        {
                            //add registration to the list
                            registrations.Add(registration.RegistrationId, registration);

                            //set last found registration
                            lastFoundRegistration = registration;
                        }
                        else
                        {
                            //should never happen
                            //log message
                            Manager.Log.WriteError(
                                "Error while loading registrations. Two registrations with same RegistrationID " +
                                registration.RegistrationId + ".");

                            //throw exception
                            throw new ApplicationException(
                                "Error while loading registrations. Two registrations with same RegistrationID " +
                                registration.RegistrationId + ".");
                        }
                    }
                }

                //lock data table of registrations
                lock (dtRegistrations)
                {
                    //check each registration in the list
                    foreach (Registration registration in ListRegistrations)
                    {
                        //create, set and add registration row
                        DataRow dr = dtRegistrations.NewRow();
                        SetRegistrationDataRow(dr, registration);
                        dtRegistrations.Rows.Add(dr);
                    }
                }
            }

            //refresh displayed registrations
            RefreshRegistrations(-1, false);
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

            //set instrument combo box
            //only let user edit instrument before edition threshold
            mcbLoanInstrument.Enabled = mbtnAddLoan.Enabled &&
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
        /// Enable registration fields according to current context.
        /// </summary>
        private void EnableRegistrationFields()
        {
            //selected registration
            Registration registration = null;

            //check if there is a selected registration
            if (dgvRegistrations.SelectedRows.Count > 0)
            {
                //get registration using its registration id
                registration = FindRegistration((int)dgvRegistrations.SelectedRows[0].Cells[columnIndexRegistrationId].Value);
            }

            //class code is read only
            mtxtRegistrationClass.Enabled = false;

            //set status combo box
            mcbRegistrationStatus.Enabled = 
                (Status != RegisterStatus.Consulting) && (registration != null) &&
                DateTime.Today >= registration.Semester.StartDate &&
                DateTime.Today < registration.Semester.EndDate;

            //set auto renewal
            mcbRegistrationAutoRenewal.Enabled = 
                (Status != RegisterStatus.Consulting) && (registration != null);

            //set delete registration button
            mbtnDeleteRegistration.Enabled =
                (Status != RegisterStatus.Consulting) && (registration != null) && (
                (DateTime.Today < registration.Semester.StartDate) ||
                (DateTime.Today < registration.Semester.EndDate && Manager.HasLogonPermission("Registration.Delete")));

            //hide registration ID column
            RegistrationId.Visible = false;
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
        /// Find registration in the list of registrations.
        /// </summary>
        /// <param name="registrationID">
        /// The ID of the selected registration.
        /// </param>
        /// <returns>
        /// The registration of the selected registration ID.
        /// Null if registration was not found.
        /// </returns>
        private Registration FindRegistration(int registrationID)
        {
            //lock list of registrations
            lock (registrations)
            {
                //check last found registration
                if (lastFoundRegistration != null &&
                    lastFoundRegistration.RegistrationId == registrationID)
                {
                    //same registration
                    return lastFoundRegistration;
                }

                //try to find selected registration
                registrations.TryGetValue(registrationID, out lastFoundRegistration);

                //return result
                return lastFoundRegistration;
            }
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
        /// List instruments into UI for selected pole.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// -1 to select all poles.
        /// </param>
        private void ListInstruments(int poleId)
        {
            //set default empty list to UI
            mcbLoanInstrument.ValueMember = "Id";
            mcbLoanInstrument.DisplayMember = "Description";
            mcbLoanInstrument.DataSource = new List<IdDescriptionStatus>();

            //check if there is a list of instruments is for selected pole
            if (instrumentLists.ContainsKey(poleId))
            {
                //set stored instruments to UI
                mcbLoanInstrument.ValueMember = "Id";
                mcbLoanInstrument.DisplayMember = "Description";
                mcbLoanInstrument.DataSource = instrumentLists[poleId];

                //exit
                return;
            }

            //load instruments
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
                //list of instruments to be displayed
                List<IdDescriptionStatus> instruments = null;

                //check selected pole
                if (poleId <= 0)
                {
                    //get list of all active instruments
                    instruments = songChannel.ListInstrumentsByStatus((int)ItemStatus.Active);
                }
                else
                {
                    //get list of pole active instruments
                    instruments = songChannel.ListInstrumentsByPole(
                        poleId, (int)ItemStatus.Active);
                }

                //check result
                if (instruments[0].Result == (int)SelectResult.Success)
                {
                    //check each instrument description
                    foreach (IdDescriptionStatus instrument in instruments)
                    {
                        //remove instrument type from description
                        instrument.Description = instrument.Description.Substring(
                            instrument.Description.IndexOf('#') + 1);
                    }

                    //sort instruments by description
                    instruments.Sort((x, y) => x.Description.CompareTo(y.Description));
                }
                else if (instruments[0].Result == (int)SelectResult.Empty)
                {
                    //no instrument is available
                    //clear list
                    instruments.Clear();
                }
                else if (instruments[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting instruments
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Instrument, instruments[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Instrument,
                        instruments[0].ErrorMessage));

                    //clear list
                    instruments.Clear();
                }

                //set instruments to UI
                mcbLoanInstrument.ValueMember = "Id";
                mcbLoanInstrument.DisplayMember = "Description";
                mcbLoanInstrument.DataSource = instruments;

                //store list for faster performance
                instrumentLists[poleId] = instruments;
            }
            catch (Exception ex)
            {
                //database error while getting instruments
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Instrument), ex);

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
        /// Refresh displayed registration datagrid.
        /// </summary>
        /// <param name="selectedRow">
        /// The index of the row to be selected.
        /// -1 if no row should be selected.
        /// </param>
        /// <param name="displayFirstRow">
        /// True if last row must be displayed.
        /// False if no specific row must be displayed.
        /// </param>
        private void RefreshRegistrations(int selectedRow, bool displayFirstRow)
        {
            //check if datagrid has not a source yet
            if (dgvRegistrations.DataSource == null)
            {
                //set source to datagrid
                dgvRegistrations.DataSource = dtRegistrations;
            }

            //check if first row should be displayed
            //and if the is at least one row
            if (displayFirstRow && dgvRegistrations.Rows.Count > 0)
            {
                //refresh grid by displaying last row
                dgvRegistrations.FirstDisplayedScrollingRowIndex = 0;
            }

            //refresh grid
            dgvRegistrations.Refresh();

            //clear default selection
            dgvRegistrations.ClearSelection();

            //check row to be selected
            if (selectedRow > -1)
            {
                //select row
                dgvRegistrations.Rows[selectedRow].Selected = true;
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
        /// Remove registration from the list of registrations.
        /// </summary>
        /// <param name="registrationID">
        /// The ID of the selected registration.
        /// </param>
        private void RemoveRegistration(int registrationID)
        {
            //lock list of registrations
            lock (registrations)
            {
                //remove registration
                registrations.Remove(registrationID);
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
            dataRow["InstrumentCode"] = loan.InstrumentCode;
            dataRow["StartDate"] = loan.StartDate;
            dataRow["EndDate"] = loan.EndDate == DateTime.MinValue ? System.DBNull.Value : (object)loan.EndDate;
            dataRow["Comments"] = loan.Comments;
            dataRow["LoanStatusName"] = ((ItemStatus)loan.LoanStatus) == ItemStatus.Active ?
                Properties.Resources.wordOpen : Properties.Resources.wordClosed;
        }

        /// <summary>
        /// Set data row with selected Registration data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="registration">The selected registration.</param>
        private void SetRegistrationDataRow(DataRow dataRow, Registration registration)
        {
            dataRow["RegistrationId"] = registration.RegistrationId;
            dataRow["ClassCode"] = registration.Class.Code;
            dataRow["PositionStatus"] = (registration.Position >= registration.Class.Capacity) ?
                Properties.Resources.classWaitingList : Properties.Resources.classRegistered;
            dataRow["PositionWaiting"] = (registration.Position >= registration.Class.Capacity);
            dataRow["AutoRenewal"] = registration.AutoRenewal ? "Auto" : string.Empty;
            dataRow["RegistrationStatusValue"] = registration.RegistrationStatus;
            dataRow["RegistrationStatusName"] = Properties.Resources.ResourceManager.GetString(
                "ItemStatus_" + ((ItemStatus)registration.RegistrationStatus).ToString());

            //set position status
            if (registration.Position >= registration.Class.Capacity)
            {
                //is waiting
                dataRow["PositionStatus"] = Properties.Resources.classWaitingList;
            }
            else
            {
                //check class status
                if (registration.Class.ClassProgress == (int)ClassProgress.Registration)
                {
                    //is registered
                    dataRow["PositionStatus"] = Properties.Resources.classRegistered;
                }
                else
                {
                    //class is in progress or completed
                    dataRow["PositionStatus"] = Properties.Resources.ResourceManager.GetString(
                    "ClassProgress_" + ((ClassProgress)registration.Class.ClassProgress).ToString());
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
            if (!ValidateRequiredField(mtxtName, mlblName.Text, mtbTabManager, tbPersonalData) ||
                !ValidateDescriptionField(mtxtName, mlblName.Text, mtbTabManager, tbPersonalData))
            {
                //invalid field
                return false;
            }

            //validate birthdate
            if (!ValidateRequiredField(mtxtBirthdate, mlblBirthdate.Text, mtbTabManager, tbPersonalData) ||
                !ValidateDateField(mtxtBirthdate, mtbTabManager, tbPersonalData))
            {
                //invalid field
                return false;
            }

            //validate guardian name
            if (!ValidateRequiredField(mtxtGuardianName, mlblGuardianName.Text, mtbTabManager, tbPersonalData))
            {
                //invalid field
                return false;
            }

            //validate tax id if set
            if (!mtxtGuardianTaxId.Text.Equals("   .   .   -") &&
                !ValidateCpfField(mtxtGuardianTaxId, null, null))
            {
                //invalid field
                return false;
            }

            //validate guardian identity date if set
            if (!mtxtGuardianIdentityDate.Text.Equals("  /  /") &&
                !ValidateDateField(mtxtGuardianIdentityDate, mtbTabManager, tbPersonalData))
            {
                //invalid field
                return false;
            }

            //validate special care if required
            if (mckHasDisability.Checked &&
                !ValidateRequiredField(mtxtSpecialCare, mlblSpecialCare.Text, mtbTabManager, tbPersonalData))
            {
                //invalid field
                return false;
            }

            //validate address
            if (!ValidateRequiredField(mtxtAddress, mlblAddress.Text, mtbTabManager, tbAddressContact))
            {
                //invalid field
                return false;
            }

            //validate district
            if (!ValidateRequiredField(mtxtDistrict, mlblDistrict.Text, mtbTabManager, tbAddressContact))
            {
                //invalid field
                return false;
            }

            //validate city
            if (!ValidateRequiredField(mtxtCity, mlblCity.Text, mtbTabManager, tbAddressContact))
            {
                //invalid field
                return false;
            }

            //validate zip code
            if (!ValidateRequiredField(mtxtZipCode, mlblZipCode.Text, mtbTabManager, tbAddressContact) ||
                !ValidateZipField(mtxtZipCode, null, null))
            {
                //invalid field
                return false;
            }

            //validate phone if set
            if (!mtxtPhone.Text.Equals("(  )     -") &&
                !ValidatePhoneField(mtxtPhone, mtbTabManager, tbAddressContact))
            {
                //invalid field
                return false;
            }

            //validate mobile if set
            if (!mtxtMobile.Text.Equals("(  )     -") &&
                !ValidatePhoneField(mtxtMobile, mtbTabManager, tbAddressContact))
            {
                //invalid field
                return false;
            }

            //validate email if set
            if (mtxtEmail.Text.Length > 0 && !ValidateEmailField(mtxtEmail, mtbTabManager, tbAddressContact))
            {
                //invalid field
                return false;
            }

            //check if there is any selected status
            //and if selected status is inactive or evaded
            if (mcbStatus.SelectedIndex >= 0 && (
                (int)mcbStatus.SelectedValue == (int)ItemStatus.Inactive ||
                (int)mcbStatus.SelectedValue == (int)ItemStatus.Evaded))
            {
                //gather list of registrations to be saved
                List<Registration> saveRegistrations = new List<Registration>();

                //check each registration row
                for (int i = 0; i < dtRegistrations.Rows.Count; i++)
                {
                    //get registration for current row
                    Registration registration = FindRegistration(
                        (int)dtRegistrations.Rows[i][columnIndexRegistrationId]);

                    //add registration to be saved
                    saveRegistrations.Add(registration);
                }

                //filter registrations for classes that are not completed
                saveRegistrations = saveRegistrations.FindAll(
                    r => r.Semester.EndDate > DateTime.Now);

                //check result
                if (saveRegistrations != null && saveRegistrations.Count > 0)
                {
                    //registrations will be deleted or set to evaded
                    //user must confirm operation
                    DialogResult dialogResult = DialogResult.Cancel;

                    //gather register class codes
                    StringBuilder sbClassCodes = new StringBuilder(64);

                    //check each registration
                    foreach (Registration registration in saveRegistrations)
                    {
                        //add registration class code
                        sbClassCodes.AppendLine(registration.Class.Code);
                    }

                    //display tab
                    mtbTabManager.SelectedTab = tbPersonalData;

                    //focus status field again
                    mcbStatus.Focus();

                    //check selected status
                    if ((int)mcbStatus.SelectedValue == (int)ItemStatus.Inactive)
                    {
                        //selected status is inactive
                        //ask user to confirm status and get result
                        dialogResult = MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.msgConfirmStudentStatusInactive,
                            sbClassCodes.ToString()), Properties.Resources.cptConfirm,
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        //selected status is evaded
                        //ask user to confirm status and get result
                        dialogResult = MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.msgConfirmStudentStatusEvaded,
                            sbClassCodes.ToString()), Properties.Resources.cptConfirm,
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    }

                    //check result
                    if (dialogResult == DialogResult.Cancel)
                    {
                        //user canceled the save operation
                        //focus status field again
                        mcbStatus.Focus();

                        //cancel save operation
                        return false;
                    }
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
            mtxtBirthdate.Text = string.Empty;
            mtxtGuardianName.Text = string.Empty;
            mtxtGuardianTaxId.Text = string.Empty;
            mtxtGuardianIdentity.Text = string.Empty;
            mtxtGuardianIdentityAgency.Text = string.Empty;
            mtxtGuardianIdentityDate.Text = string.Empty;
            mtxtSpecialCare.Text = string.Empty;

            mtxtAddress.Text = string.Empty;
            mtxtDistrict.Text = string.Empty;
            mtxtCity.Text = string.Empty;
            mtxtZipCode.Text = string.Empty;
            mtxtPhone.Text = string.Empty;
            mtxtMobile.Text = string.Empty;
            mtxtEmail.Text = string.Empty;

            //clear special fields
            assignmentFile = string.Empty;
            photoFile = string.Empty;

            //clear 
            pbAssignment.Image = null;
            pbPhoto.Image = null;

            //select Rio de Janeiro
            mcbState.SelectedIndex = 17;

            //set has disability option
            mckHasDisability.Checked = false;

            //check number of poles
            if (mcbPole.Items.Count > 0)
            {
                //select first pole
                mcbPole.SelectedIndex = 0;
            }

            //clear registrations
            //load empty list
            DisplayRegistrations(new List<Registration>());

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
            Manager.Settings.HideInactiveStudents = this.hideInactiveItems;
        }

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        /// <returns></returns>
        public override string SelectMenuOption()
        {
            //select student option
            return "Student";
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
            mtxtBirthdate.Enabled = enable;
            mtxtGuardianName.Enabled = enable;
            mtxtGuardianTaxId.Enabled = enable;
            mtxtGuardianIdentity.Enabled = enable;
            mtxtGuardianIdentityAgency.Enabled = enable;
            mtxtGuardianIdentityDate.Enabled = enable;
            mtxtSpecialCare.Enabled = enable;

            mtxtAddress.Enabled = enable;
            mtxtDistrict.Enabled = enable;
            mtxtCity.Enabled = enable;
            mtxtZipCode.Enabled = enable;
            mtxtPhone.Enabled = enable;
            mtxtMobile.Enabled = enable;
            mtxtEmail.Enabled = enable;

            //set state list
            mcbState.Enabled = enable;

            //set has disability option
            mckHasDisability.Enabled = enable;

            //set pole list
            mcbPole.Enabled = enable;

            //set registration fields
            mbtnAddRegistration.Enabled = enable;
            EnableRegistrationFields();

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
            //enable picture boxes only after first student is loaded
            pbPhoto.Enabled = true;
            pbAssignment.Enabled = true;

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
                //get selected student from web service
                Student student = songChannel.FindStudent(itemId);

                //check result
                if (student.Result == (int)SelectResult.Empty)
                {
                    //should never happen
                    //could not load data
                    return false;
                }
                else if (student.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting student
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, student.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, student.ErrorMessage));

                    //could not load data
                    return false;
                }

                //set selected student ID
                selectedId = student.StudentId;

                //select status
                mcbStatus.SelectedValue = student.StudentStatus;

                //set inactivation fields
                inactivationReason = student.InactivationReason;
                inactivationTime = student.InactivationTime;

                //set text fields
                mtxtName.Text = student.Name;
                mtxtBirthdate.Text = student.Birthdate.ToShortDateString();
                mtxtGuardianName.Text = student.GuardianName;
                mtxtGuardianTaxId.Text = student.GuardianTaxId;
                mtxtGuardianIdentity.Text = student.GuardianIdentity;
                mtxtGuardianIdentityAgency.Text = student.GuardianIdentityAgency;
                mtxtGuardianIdentityDate.Text = student.GuardianIdentityDate.ToShortDateString();
                mtxtSpecialCare.Text = student.SpecialCare;

                mtxtAddress.Text = student.Address;
                mtxtDistrict.Text = student.District;
                mtxtCity.Text = student.City;
                mtxtZipCode.Text = student.ZipCode;
                mtxtPhone.Text = student.Phone;
                mtxtMobile.Text = student.Mobile;
                mtxtEmail.Text = student.Email;

                //set state
                mcbState.SelectedValue = student.State;

                //check selected state
                if (mcbState.SelectedIndex < 0)
                {
                    //should never happen
                    //select default state
                    mcbState.SelectedIndex = 17;
                }

                //set has disability option
                mckHasDisability.Checked = student.HasDisability;

                //set special fields
                assignmentFile = student.AssignmentFile;
                photoFile = student.PhotoFile;

                //load thumbnail for photo
                LoadFileThumbnail(photoFile, pbPhoto, songChannel);

                //load thumbnail for assignment
                LoadFileThumbnail(assignmentFile, pbAssignment, songChannel);

                //set pole
                mcbPole.SelectedValue = student.PoleId;

                //check selected index
                if (mcbPole.SelectedIndex < 0)
                {
                    try
                    {
                        //pole is not available
                        //it might be inactive
                        //must load pole from web service
                        Pole pole = songChannel.FindPole(student.PoleId);

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
                            mcbPole.SelectedValue = student.PoleId;
                        }
                    }
                    catch
                    {
                        //do nothing
                    }
                }

                #region load registrations

                //get registrations for selected instrument
                List<Registration> registrations = songChannel.FindRegistrationsByStudent(
                    true, true, student.StudentId, -1);

                //check result
                if (registrations[0].Result == (int)SelectResult.Empty)
                {
                    //instrument has no registration
                    //clear list
                    registrations.Clear();
                }
                else if (registrations[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting registrations
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadRegistrations,
                        itemTypeDescription, registrations[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadRegistrations,
                        itemTypeDescription, registrations[0].ErrorMessage));

                    //could not load data
                    return false;
                }

                //sort registrations by class code desc
                registrations.Sort((x, y) => y.Class.Code.CompareTo(x.Class.Code));

                //check each registration
                foreach (Registration registration in registrations)
                {
                    //set registration class to display extra data
                    SetRegistrationClass(registration);
                }

                //display registrations
                DisplayRegistrations(registrations);

                #endregion load registrations

                #region load loans

                //list instruments for student pole
                ListInstruments(student.PoleId);

                //get loans for selected student
                List<Loan> loans = songChannel.FindLoansByStudent(
                    student.StudentId, -1);

                //check result
                if (loans[0].Result == (int)SelectResult.Empty)
                {
                    //student has no loan
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
                mcbLoanInstrument.SelectedIndex = -1;

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
        /// Load student list from database.
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
                //list of students
                List<IdDescriptionStatus> students = null;

                //check if logged on user has an assigned institution
                if (Manager.LogonUser != null &&
                    Manager.LogonUser.InstitutionId > 0)
                {
                    //get list of students for assigned institution
                    students = songChannel.ListStudentsByInstitution(
                        Manager.LogonUser.InstitutionId, -1);
                }
                else
                {
                    //get list of all students
                    students = songChannel.ListStudents();
                }

                //check result
                if (students[0].Result == (int)SelectResult.Success)
                {
                    //students were found
                    return students;
                }
                else if (students[0].Result == (int)SelectResult.Empty)
                {
                    //no student is available
                    return null;
                }
                else if (students[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting students
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        itemTypeDescription, students[0].ErrorMessage));

                    //could not get students
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

            //could not get students
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
                //inactivate selected student and get result
                DeleteResult result = songChannel.InactivateStudent(
                    SelectedItemId, reasonForm.InactivationReason);

                //check result
                if (result.Result == (int)SelectResult.Success)
                {
                    //item was inactivated
                    //check if there is a parent control
                    //and if it is an student register control
                    if (parentControl != null && parentControl is ViewStudentControl)
                    {
                        //update student to inactive in parent control
                        ((ViewStudentControl)parentControl).UpdateStudent(
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
            //check selected tab
            if (mtbTabManager.SelectedIndex == 0)
            {
                //focus project name field
                mtxtName.Focus();
                mtxtName.SelectionLength = 0;
            }
            else if (mtbTabManager.SelectedIndex == 1)
            {
                //focus address field
                mtxtAddress.Focus();
                mtxtAddress.SelectionLength = 0;
            }
        }

        /// <summary>
        /// Start creating a new item from scratch.
        /// </summary>
        public override void CreateItem()
        {
            //select first tab
            mtbTabManager.SelectedIndex = 0;

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

            //create an student and set data
            Student student = new Student();

            //set selected student ID
            student.StudentId = selectedId;

            //check selected status
            if (mcbStatus.SelectedIndex >= 0)
            {
                //set status
                student.StudentStatus = (int)mcbStatus.SelectedValue;

                //check if selected status is inactive or evaded
                if ((int)mcbStatus.SelectedValue == (int)ItemStatus.Inactive ||
                    (int)mcbStatus.SelectedValue == (int)ItemStatus.Evaded)
                {
                    //create inactivation reason form
                    InactivationReasonForm reasonForm = new InactivationReasonForm(
                        itemTypeDescription, (int)mcbStatus.SelectedValue, inactivationReason);

                    //let user input an inactivation reason
                    reasonForm.ShowDialog(this);

                    //set inactivation reason with result
                    student.InactivationReason = reasonForm.InactivationReason;

                    //set inactivation time
                    student.InactivationTime = (inactivationTime != DateTime.MinValue) ?
                        inactivationTime : DateTime.Now;
                }
                else
                {
                    //reset inactivation
                    student.InactivationReason = string.Empty;
                    student.InactivationTime = DateTime.MinValue;
                }
            }
            else
            {
                //should never happen
                //set default status
                student.StudentStatus = (int)ItemStatus.Active;

                //reset inactivation
                student.InactivationTime = DateTime.MinValue;
                student.InactivationReason = string.Empty;
            }

            //set text fields
            student.Name = mtxtName.Text;
            student.Birthdate = DateTime.Parse(mtxtBirthdate.Text);
            student.GuardianName = mtxtGuardianName.Text;
            student.GuardianTaxId = mtxtGuardianTaxId.Text.Equals("   .   .   -") ? 
                string.Empty : mtxtGuardianTaxId.Text;
            student.GuardianIdentity = mtxtGuardianIdentity.Text;
            student.GuardianIdentityAgency = mtxtGuardianIdentityAgency.Text;
            student.GuardianIdentityDate = mtxtGuardianIdentityDate.Text.Equals("  /  /") ? 
                DateTime.MinValue : DateTime.Parse(mtxtGuardianIdentityDate.Text);
            student.SpecialCare = mtxtSpecialCare.Text;

            student.Address = mtxtAddress.Text;
            student.District = mtxtDistrict.Text;
            student.City = mtxtCity.Text;
            student.State = mcbState.SelectedValue.ToString();
            student.ZipCode = mtxtZipCode.Text;
            student.Phone = mtxtPhone.Text.Equals("(  )     -") ? 
                string.Empty : mtxtPhone.Text;
            student.Mobile = mtxtMobile.Text.Equals("(  )     -") ? 
                string.Empty : mtxtMobile.Text;
            student.Email = mtxtEmail.Text;

            //set has disability option
            student.HasDisability = mckHasDisability.Checked;

            //set photo
            student.Photo = null;

            //set pole
            student.PoleId = (int)mcbPole.SelectedValue;

            //set pole name to properly display student in datagridview
            student.PoleName = ((IdDescriptionStatus)mcbPole.SelectedItem).Description;

            //set user
            student.UserId = int.MinValue;

            //set user login to properly display student in datagridview
            student.UserLogin = string.Empty;

            //set special fields
            student.AssignmentFile = assignmentFile;
            student.PhotoFile = photoFile;

            //must load photo file is photo was updated
            SongDB.Logic.File photo = null;

            //check if photo was updated
            if (photoFile.Contains("tempFile"))
            {
                //load file from disk
                photo = Manager.FileManager.GetFile(photoFile);
            }

            //must load assignment file is assignment was updated
            SongDB.Logic.File assignment = null;

            //check if assignment was updated
            if (assignmentFile.Contains("tempFile"))
            {
                //load file from disk
                assignment = Manager.FileManager.GetFile(assignmentFile);
            }

            //gather list of registrations to be saved
            List<Registration> saveRegistrations = new List<Registration>();

            //check each registration row
            for (int i = 0; i < dtRegistrations.Rows.Count; i++)
            {
                //get registration for current row
                Registration registration = FindRegistration(
                    (int)dtRegistrations.Rows[i][columnIndexRegistrationId]);

                //add registration to be saved
                saveRegistrations.Add(registration);
            }
            
            //check if selected status is inactive or evaded
            if (student.StudentStatus == (int)ItemStatus.Inactive ||
                student.StudentStatus == (int)ItemStatus.Evaded)
            {
                //must edit status for all registrations before saving
                //check each registration
                for (int i = saveRegistrations.Count - 1; i >= 0; i--)
                {
                    //get current registration
                    Registration registration = saveRegistrations[i];

                    //check registration semester end date
                    if (registration.Semester.EndDate <= DateTime.Today)
                    {
                        //registration class is completed
                        //go to next registration
                        continue;
                    }
                    else if (registration.Semester.StartDate <= DateTime.Today)
                    {
                        //registration class is in progress
                        //check selected status
                        if (student.StudentStatus == (int)ItemStatus.Inactive)
                        {
                            //status is inactive
                            //remove registration
                            saveRegistrations.RemoveAt(i);
                        }
                        else
                        {
                            //status is evaded
                            //update registration to evaded
                            registration.RegistrationStatus = (int)ItemStatus.Evaded;
                        }
                    }
                    else
                    {
                        //registration class has not started yet
                        //remove registration
                        saveRegistrations.RemoveAt(i);
                    }
                    
                }
            }

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
                //could not save student
                return null;
            }

            try
            {
                //save student and get result
                SaveResult saveResult = songChannel.SaveStudent(
                    student, saveRegistrations, saveLoans, photo, assignment);

                //check result
                if (saveResult.Result == (int)SelectResult.FatalError)
                {
                    //student was not saved
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

                    //could not save student
                    return null;
                }

                //set saved ID to student ID
                student.StudentId = saveResult.SavedId;

                //set selected ID
                selectedId = saveResult.SavedId;

                //check if there is a parent control and its type
                if (parentControl != null && parentControl is ViewStudentControl)
                {
                    //update student in parent control
                    ((ViewStudentControl)parentControl).UpdateStudent(student);
                }
                else if (parentControl != null && parentControl is ViewRegistrationControl)
                {
                    //update student in parent control
                    ((ViewRegistrationControl)parentControl).UpdateStudent(student);
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

            //student was saved
            //return updated description
            return student.GetDescription();
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
                //could not copy student
                return null;
            }

            try
            {
                //copy selected student and get result
                Student student = songChannel.CopyStudent(SelectedItemId);

                //check student copy
                if (student.Result == (int)SelectResult.FatalError)
                {
                    //student was not copied
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceCopyItem,
                        itemTypeDescription, student.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceCopyItem,
                        itemTypeDescription, student.ErrorMessage));

                    //could not copy student
                    return null;
                }

                //check if there is a parent control
                //and if it is an student register control
                if (parentControl != null && parentControl is ViewStudentControl)
                {
                    //update student in parent control
                    ((ViewStudentControl)parentControl).UpdateStudent(student);
                }

                //student was copied
                //return description
                return student.GetDescription(); ;
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


        #region Private Methods *******************************************************

        /// <summary>
        /// Load thumbnail for selected file and display it in the picture box.
        /// </summary>
        /// <param name="file">
        /// The selected file.
        /// </param>
        /// <param name="pictureBox">
        /// The picture box to display the loaded thumbnail.
        /// </param>
        /// <param name="songChannel">
        /// The channel to be used when loading thumbnail from server.
        /// </param>
        private void LoadFileThumbnail(
            string file, PictureBox pictureBox, ISongService songChannel)
        {
            //check photo file
            if (file == null || file.Length == 0)
            {
                //no file is selected
                //clear photo thumbnail
                pictureBox.Image = null;

                //exit
                return;
            }

            //check format
            if (file.ToLower().EndsWith(".pdf"))
            {
                //display pdf thumbnail
                DisplayImage(Properties.Resources.IconPDF, pictureBox, false);

                //exit
                return;
            }

            //try to get file thumbnail
            Bitmap photoThumbnail = Manager.FileManager.GetThumbnail(file);

            //check result
            if (photoThumbnail != null)
            {
                //display thumbnail
                DisplayImage(photoThumbnail, pictureBox, true);
            }
            else
            {
                //thumbnail is not available
                //clear file thumbnail
                pictureBox.Image = null;

                //check if song channel is available
                if (songChannel == null)
                {
                    //channel was not provided
                    //should never happen
                    //could not load thumbnail
                    return;
                }

                //download thumbnail file
                SongDB.Logic.File thumbnailFile = songChannel.GetFileThumbnail(file);

                //check result
                if (thumbnailFile.Result == (int)SelectResult.Success)
                {
                    //save thumbnail file
                    if (Manager.FileManager.SaveFile(thumbnailFile))
                    {
                        //get file thumbnail
                        photoThumbnail = Manager.FileManager.GetThumbnail(file);

                        //check result
                        if (photoThumbnail != null)
                        {
                            //display thumbnail
                            DisplayImage(photoThumbnail, pictureBox, true);
                        }
                    }
                    else
                    {
                        //error while saving thumbnail file to disk
                        //display message
                        MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.errorWriteSongFile, 
                            thumbnailFile.FilePath),
                            Properties.Resources.wordError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (thumbnailFile.Result == (int)SelectResult.Empty)
                {
                    //file was not found
                    //do nothing
                }
                else if (thumbnailFile.Result == (int)SelectResult.FatalError)
                {
                    //file error while getting thumbnail
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadFileThumbnail,
                        itemTypeDescription, file, thumbnailFile.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadFileThumbnail,
                        itemTypeDescription, file, thumbnailFile.ErrorMessage));
                }
            }
        }

        /// <summary>
        /// Display image in the selected picture box.
        /// Resample image according to picture box size.
        /// </summary>
        /// <param name="image">
        /// The image to be displayed.
        /// </param>
        /// <param name="pictureBox">
        /// The selected picture box.
        /// </param>
        /// <param name="isPhoto">
        /// True if image is a photo.
        /// False otherwise.
        /// </param>
        private void DisplayImage(Bitmap image, PictureBox pictureBox, bool isPhoto)
        {
            //get image ration
            double imgRation = (double)image.Width / (double)image.Height;

            //get picture box ratio
            double pbRatio = (double)pictureBox.Width / (double)pictureBox.Height;

            //must calculate ration
            double ratio;

            //compare rations
            if (imgRation >= pbRatio)
            {
                //image is wider
                ratio = (double)pictureBox.Width / (double)image.Width;
            }
            else
            {
                //image is taller
                ratio = (double)pictureBox.Height / (double)image.Height;
            }

            //check if image is a photo
            if (isPhoto)
            {
                //set a little zoom
                ratio *= 1.1;
            }

            //create a resampled image
            System.Drawing.Bitmap resampledImage = new System.Drawing.Bitmap(
                (int)(image.Width * ratio), (int)(image.Height * ratio));

            //paint on the resampled image
            using (System.Drawing.Graphics G =
                System.Drawing.Graphics.FromImage(resampledImage))
            {
                //draw original image with maximum quality
                G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                G.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                G.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                G.DrawImage(image, 0, 0, resampledImage.Width, resampledImage.Height);
            }

            //display resampled image
            pictureBox.Image = resampledImage;
        }

        /// <summary>
        /// Set selected registration to display class data
        /// </summary>
        /// <param name="registration">
        /// The selected registration.
        /// </param>
        private void SetRegistrationClass(Registration registration)
        {
            //get class
            Class classObj = registration.Class;

            //gather week days
            StringBuilder sbDays = new StringBuilder(8);
            if (classObj.WeekMonday)
            {
                sbDays.Append(Properties.Resources.dayShortMondays);
                sbDays.Append(", ");
            }
            if (classObj.WeekTuesday)
            {
                sbDays.Append(Properties.Resources.dayShortTuesdays);
                sbDays.Append(", ");
            }
            if (classObj.WeekWednesday)
            {
                sbDays.Append(Properties.Resources.dayShortWednesdays);
                sbDays.Append(", ");
            }
            if (classObj.WeekThursday)
            {
                sbDays.Append(Properties.Resources.dayShortThursdays);
                sbDays.Append(", ");
            }
            if (classObj.WeekFriday)
            {
                sbDays.Append(Properties.Resources.dayShortFridays);
                sbDays.Append(", ");
            }
            if (classObj.WeekSaturday)
            {
                sbDays.Append(Properties.Resources.dayShortSaturdays);
                sbDays.Append(", ");
            }
            if (classObj.WeekSunday)
            {
                sbDays.Append(Properties.Resources.dayShortSundays);
                sbDays.Append(", ");
            }

            //check result
            if (sbDays.Length > 2)
            {
                //remove last ", "
                sbDays.Length -= 2;
            }

            //create description
            StringBuilder sbDescription = new StringBuilder(64);
            sbDescription.Append(" | ");

            //add class or instrument type
            sbDescription.Append(classObj.ClassType == (int)ClassType.Instrument ?
                Properties.Resources.ResourceManager.GetString(
                    "InstrumentsType_" + ((InstrumentsType)classObj.InstrumentType).ToString()) :
                Properties.Resources.ResourceManager.GetString(
                    "ClassType_" + ((ClassType)classObj.ClassType).ToString()));
            sbDescription.Append(" | ");

            //add class teacher
            sbDescription.Append(classObj.TeacherName);
            sbDescription.Append(" | ");

            //add class level
            sbDescription.Append(Properties.Resources.ResourceManager.GetString(
                "ClassLevel_" + ((ClassLevel)classObj.ClassLevel).ToString()));
            sbDescription.Append(" | ");

            //add class days and start time
            sbDescription.Append(sbDays.ToString());
            sbDescription.Append(" ");
            sbDescription.Append(classObj.StartTime.ToString("HH:mm"));

            //add description to class code
            classObj.Code += sbDescription.ToString();
        }

        #endregion Private Methods


        #region UI Event Handlers *****************************************************

        /// <summary>
        /// Register insitution 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterStudent_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set font to UI controls
            mtxtBirthdate.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtGuardianTaxId.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtGuardianIdentityDate.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtZipCode.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtPhone.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtMobile.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtLoanEndDate.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtLoanStartDate.Font = MetroFramework.MetroFonts.Default(13.0F);

            //set font to datagridviews
            dgvRegistrations.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.DefaultLight(12);
            dgvRegistrations.DefaultCellStyle.Font =
                MetroFramework.MetroFonts.DefaultLight(12);
            dgvLoans.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvLoans.DefaultCellStyle.Font =
                MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            
            //display first tab
            mtbTabManager.SelectedIndex = 0;
        }

        /// <summary>
        /// Has disability checked changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mckHasDisability_CheckedChanged(object sender, EventArgs e)
        {
            //enable special care field
            mtxtSpecialCare.Enabled = mckHasDisability.Checked && mckHasDisability.Enabled;
        }

        /// <summary>
        /// Phone masked text box click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Phone_Click(object sender, EventArgs e)
        {
            //get sender textbox
            MaskedTextBox mtxtPhone = (MaskedTextBox)sender;

            //check text
            if (mtxtPhone.Text.Equals("(  )     -"))
            {
                //set cursor position
                mtxtPhone.Select(1, 0);
            }
        }

        /// <summary>
        /// Zip code masked text box click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtZipCode_Click(object sender, EventArgs e)
        {
            //check text
            if (mtxtZipCode.Text.Equals("     -"))
            {
                //set cursor position
                mtxtZipCode.Select(0, 0);
            }
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
        /// Guardian tax id masked text box click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtGuardianTaxId_Click(object sender, EventArgs e)
        {
            //check text
            if (mtxtGuardianTaxId.Text.Equals("   .   .   -"))
            {
                //set cursor position
                mtxtGuardianTaxId.Select(0, 0);
            }
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

            //list instruments for selected pole
            ListInstruments((int)mcbPole.SelectedValue);

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
                        Properties.Resources.msgPreviousOpenLoanStudent,
                        Properties.Resources.titleUnavailableStudent,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //exit
                    return;
                }
            }

            //check if there is no listed
            if (mcbLoanInstrument.Items.Count == 0)
            {
                //no instrument is available for selection
                //no instrument must have been created
                //should hardly ever happen
                //display message
                MetroMessageBox.Show(Manager.MainForm,
                    Properties.Resources.msgPoleNoInstrument,
                    Properties.Resources.titleNoInstrument,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //exit
                return;
            }

            //next available instrument without loan
            //must consult database
            Instrument nextInstrument = null;

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            try
            {
                //check result and if there is a selected pole
                if (songChannel != null && mcbPole.SelectedIndex >= 0)
                {
                    //get next available instrument without loan
                    nextInstrument = songChannel.FindNextInstrumentWithoutLoan(
                        (int)mcbPole.SelectedValue);

                    //check result
                    if (nextInstrument.Result == (int)SelectResult.Success)
                    {
                        //next instrument was found
                        //will select instrument for new loan down below
                    }
                    else if (nextInstrument.Result == (int)SelectResult.Empty)
                    {
                        //no instrument is available
                        //display message
                        MetroMessageBox.Show(Manager.MainForm,
                            Properties.Resources.msgPoleNoInstrumentWithoutLoan,
                            Properties.Resources.titleNoInstrument,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        //exit
                        return;
                    }
                    else
                    {
                        //error while getting next instrument
                        //should never happen
                        //just select first instrument
                        nextInstrument = null;
                    }
                }
            }
            catch (Exception ex)
            {
                //database error while getting poles
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelGetNextAvailableItem,
                    Properties.Resources.item_Instrument), ex);

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
            loan.StudentId = selectedId;
            loan.CreationTime = DateTime.Now;
            loan.LoanStatus = (int)ItemStatus.Active;
            loan.StartDate = DateTime.Today;
            loan.EndDate = DateTime.MinValue;

            //check if next instrument was found
            if (nextInstrument != null)
            {
                //set next instrument to loan
                loan.InstrumentId = nextInstrument.Id;
                loan.InstrumentCode = nextInstrument.Code;
            }
            else
            {
                //set first instrument to loan
                IdDescriptionStatus instrument = (IdDescriptionStatus)mcbLoanInstrument.Items[0];
                loan.InstrumentId = instrument.Id;
                loan.InstrumentCode = instrument.Description;
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
                        //select instrument
                        mcbLoanInstrument.SelectedValue = loan.InstrumentId;

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
        /// Loan instrument combo box index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbLoanInstrument_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if is loading loan 
            if (isLoadingLoan)
            {
                //is loading
                //exit
                return;
            }

            //check selected index
            if (mcbLoanInstrument.SelectedIndex == -1)
            {
                //no instrument is selected
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

            //check selected instrument
            if (loan.InstrumentId == (int)mcbLoanInstrument.SelectedValue)
            {
                //same selected instrument
                //exit
                return;
            }

            //check if instrument has an ongoing loan
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel != null)
            {
                try
                {
                    //get list of active loans for selected instrument
                    List<Loan> instrumentLoans = songChannel.FindLoansByInstrument(
                        (int)mcbLoanInstrument.SelectedValue, (int)ItemStatus.Active);

                    //check result
                    if (instrumentLoans[0].Result == (int)SelectResult.Success)
                    {
                        //instrument has an ongoing loan
                        //display message
                        MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.msgInstrumentAlreadyHasLoan,
                            ((IdDescriptionStatus)mcbLoanInstrument.SelectedItem).Description,
                            instrumentLoans[0].StudentName),
                            Properties.Resources.wordLoan,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        //set loading flag
                        isLoadingLoan = true;

                        //reselect original instrument
                        mcbLoanInstrument.SelectedValue = loan.InstrumentId;

                        //reset loading flag
                        isLoadingLoan = false;

                        //no need to update instrument
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


            //update loan instrument
            loan.InstrumentId = (int)mcbLoanInstrument.SelectedValue;
            loan.InstrumentCode = ((IdDescriptionStatus)mcbLoanInstrument.SelectedItem).Description;

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
                    Properties.Resources.msgConfirmCloseLoan, loan.InstrumentCode),
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

        /// <summary>
        /// Add registration click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddRegistration_Click(object sender, EventArgs e)
        {
            //check selected pole
            if (mcbPole.SelectedIndex < 0)
            {
                //should never happen
                //class must have a pole
                //disable add button
                mbtnAddRegistration.Enabled = false;

                //exit
                return;
            }

            //let user select class
            //only display classes that are accepting registrations
            SelectClassForm classForm = new SelectClassForm(
                (int)mcbPole.SelectedValue, 
                Manager.HasLogonPermission("Registration.InsertAnyStudent"),
                true);

            //display form as a dialog
            if (classForm.ShowDialog(Manager.MainForm) == DialogResult.Cancel)
            {
                //user canceled operation
                //exit
                return;
            }

            //get selected class
            Class selectedClass = classForm.SelectedClass;

            //lock list of registrations
            lock (registrations)
            {
                //check if student is already registered to selected class
                if (new List<Registration>(registrations.Values).Find(
                    r => r.ClassId == selectedClass.Id) != null)
                {
                    //student is already registered to selected class
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.msgStudentAlreadyRegistered,
                        mtxtName.Text, selectedClass.Code),
                        Properties.Resources.item_Registration,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //find selected class previous registration
                    //lock data table of registrations
                    lock (dtRegistrations)
                    {
                        //check each registration row
                        for (int i = 0; i < dtRegistrations.Rows.Count; i++)
                        {
                            //get next row registration
                            Registration previousRegistration = FindRegistration(
                                (int)dtRegistrations.Rows[i][columnIndexRegistrationId]);

                            //compare classes
                            if (previousRegistration.ClassId == selectedClass.Id)
                            {
                                //found previous registration
                                //clear default selection
                                dgvRegistrations.ClearSelection();

                                //select current row
                                dgvRegistrations.Rows[i].Selected = true;

                                //refresh grid by displaying selected row
                                dgvRegistrations.FirstDisplayedScrollingRowIndex = i > 3 ? (i - 4) : 0;

                                //exit loop
                                break;
                            }
                        }
                    }

                    //exit
                    return;
                }
            }

            //create and set registration
            Registration registration = new Registration();
            registration.RegistrationId = -1;
            registration.RegistrationStatus = (int)ItemStatus.Active;
            registration.ClassId = selectedClass.Id;
            registration.Class = selectedClass;
            registration.StudentId = selectedId;
            registration.TeacherName = selectedClass.TeacherName;
            registration.Position = int.MinValue;
            registration.Semester = selectedClass.Semester;
            registration.AutoRenewal = true;
            registration.CreationTime = DateTime.Now;
            registration.InactivationReason = string.Empty;
            registration.InactivationTime = DateTime.MinValue;

            //set registration class to display extra data
            SetRegistrationClass(registration);

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not create registration
                return;
            }

            try
            {
                //get number of regsitrations for class
                CountResult count = songChannel.CountRegistrationsByClass(
                    selectedClass.Id, -1);

                //check result
                if (count.Result == (int)SelectResult.Success)
                {
                    //set position according to number of registrations
                    registration.Position = count.Count;
                }
                else if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while counting registrations
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceCountItem,
                        Properties.Resources.item_Registration, count.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceCountItem,
                        Properties.Resources.item_Registration, count.ErrorMessage));

                    //could not get poles
                    //exit
                    return;
                }
            }
            catch (Exception ex)
            {
                //database error while getting users
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelCountItem,
                    Properties.Resources.item_Registration), ex);

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

            //lock list of registrations
            lock (registrations)
            {
                //check if id is taken
                while (registrations.ContainsKey(registration.RegistrationId))
                {
                    //decrement id
                    registration.RegistrationId--;
                }

                //add registration to the list
                registrations.Add(registration.RegistrationId, registration);

                //set last found registration
                lastFoundRegistration = registration;
            }

            //lock data table of registrations
            lock (dtRegistrations)
            {
                //create, set and add registration row
                DataRow dr = dtRegistrations.NewRow();
                SetRegistrationDataRow(dr, registration);
                dtRegistrations.Rows.InsertAt(dr, 0);
            }

            //refresh displayed registrations
            //select and display last row
            RefreshRegistrations(0, true);
        }

        /// <summary>
        /// Remove registration click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnDeleteRegistration_Click(object sender, EventArgs e)
        {
            //check if there is a selected registration
            if (dgvRegistrations.SelectedRows.Count == 0)
            {
                //no selected registration
                //exit
                return;
            }

            //get selected index
            int selectedIndex = dgvRegistrations.SelectedRows[0].Index;

            //get row
            DataRow row = dtRegistrations.Rows[selectedIndex];

            //get registration
            Registration registration = FindRegistration((int)row[columnIndexRegistrationId]);

            //check if registration class has already started
            if (DateTime.Today >= registration.Semester.StartDate)
            {
                //ask user to confirm deletion and get result
                DialogResult dr = MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgConfirmDeleteRegistrationLoseData, 
                registration.StudentName), Properties.Resources.cptConfirm,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                //check result
                if (dr == DialogResult.Cancel)
                {
                    //user canceled operation
                    //exit
                    return;
                }
            }
            else
            {
                //ask user to confirm deletion and get result
                DialogResult dr = MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgConfirmDeleteRegistration, registration.StudentName),
                Properties.Resources.cptConfirm,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                //check result
                if (dr == DialogResult.Cancel)
                {
                    //user canceled operation
                    //exit
                    return;
                }
            }

            //remove registration
            RemoveRegistration(registration.RegistrationId);
            dtRegistrations.Rows.RemoveAt(selectedIndex);

            //clear selection
            dgvRegistrations.ClearSelection();

            //refresh grid
            dgvRegistrations.Refresh();
        }

        /// <summary>
        /// Registrations datagridview selection changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRegistrations_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //set loading flag
                isLoadingRegistration = true;

                //check if there is a selected registration
                if (dgvRegistrations.SelectedRows.Count > 0)
                {
                    //get registration using its id
                    Registration registration = FindRegistration(
                        (int)dgvRegistrations.SelectedRows[0].Cells[columnIndexRegistrationId].Value);

                    //check result
                    if (registration != null)
                    {
                        //check class code
                        if (registration.Class.Code.IndexOf(" | ") >= 0)
                        {
                            //remove added data and set class code
                            mtxtRegistrationClass.Text = registration.Class.Code.Substring(
                                0, registration.Class.Code.IndexOf(" | "));
                        }
                        else
                        {
                            //just set class code
                            mtxtRegistrationClass.Text = registration.Class.Code;
                        }

                        //set status
                        mcbRegistrationStatus.SelectedValue = (int)registration.RegistrationStatus;

                        //set auto renewal option
                        mcbRegistrationAutoRenewal.Checked = registration.AutoRenewal;
                    }
                    else
                    {
                        //registration was not found
                        //should never happen
                        //clear fields
                        ClearRegistrationFields();
                    }
                }
                else
                {
                    //clear fields
                    ClearRegistrationFields();
                }

                //enable/disable registration fields
                EnableRegistrationFields();
            }
            finally
            {
                //reset loading flag
                isLoadingRegistration = false;
            }
        }

        /// <summary>
        /// Registrations datagridview cell painting event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRegistrations_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //check row index
            if (e.RowIndex < 0)
            {
                //exit
                return;
            }

            //get row
            DataRow row = dtRegistrations.Rows[e.RowIndex];

            //check row registration status
            if ((int)row["RegistrationStatusValue"] == (int)ItemStatus.Evaded)
            {
                //row is evaded
                //change cell color to light red
                if (e.RowIndex % 2 == 0)
                {
                    //light light red
                    e.CellStyle.BackColor = Color.FromArgb(244, 91, 91);
                }
                else
                {
                    //light red
                    e.CellStyle.BackColor = Color.FromArgb(244, 81, 81);
                }
            }
            //check if student is waiting
            else if ((bool)row["PositionWaiting"])
            {
                //exceeded capacity
                //change cell color to light yellow
                if (e.RowIndex % 2 == 0)
                {
                    //light light yellow
                    e.CellStyle.BackColor = Color.FromArgb(244, 251, 160);
                }
                else
                {
                    //light yellow
                    e.CellStyle.BackColor = Color.FromArgb(244, 251, 140);
                }
            }
        }

        /// <summary>
        /// Registration status selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbRegistrationStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if is loading registration 
            if (isLoadingRegistration)
            {
                //is loading
                //exit
                return;
            }

            //check if there is a selected registration
            if (dgvRegistrations.SelectedRows.Count == 0)
            {
                //no selected
                //exit
                return;
            }

            //get selected registration index
            int selectedIndex = dgvRegistrations.SelectedRows[0].Index;

            //get row
            DataRow row = dtRegistrations.Rows[selectedIndex];

            //get registration
            Registration registration = FindRegistration((int)row[columnIndexRegistrationId]);

            //update registration status
            registration.RegistrationStatus = (int)mcbRegistrationStatus.SelectedValue;

            //check if selected registration status is inactive
            if (registration.RegistrationStatus == (int)ItemStatus.Evaded)
            {
                //create inactivation reason form
                InactivationReasonForm reasonForm = new InactivationReasonForm(
                    Properties.Resources.item_Registration,
                    (int)mcbRegistrationStatus.SelectedValue,
                    registration.InactivationReason);

                //let user input an inactivation reason
                reasonForm.ShowDialog(this);

                //set inactivation reason with result
                registration.InactivationReason = reasonForm.InactivationReason;

                //check inactivation time
                if (registration.InactivationTime == DateTime.MinValue)
                {
                    //set inactivation time
                    registration.InactivationTime = DateTime.Now;
                }
            }
            else
            {
                //reset inactivation
                registration.InactivationReason = string.Empty;
                registration.InactivationTime = DateTime.MinValue;
            }

            //set row
            SetRegistrationDataRow(row, registration);

            //refresh grid
            dgvRegistrations.Refresh();
        }

        /// <summary>
        /// Registration auto renewal checked changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbRegistrationAutoRenewal_CheckedChanged(object sender, EventArgs e)
        {
            //check if is loading registration 
            if (isLoadingRegistration)
            {
                //is loading
                //exit
                return;
            }

            //check if there is a selected registration
            if (dgvRegistrations.SelectedRows.Count == 0)
            {
                //no selected
                //exit
                return;
            }

            //get selected registration index
            int selectedIndex = dgvRegistrations.SelectedRows[0].Index;

            //get row
            DataRow row = dtRegistrations.Rows[selectedIndex];

            //get registration
            Registration registration = FindRegistration((int)row[columnIndexRegistrationId]);

            //update registration auto renewal option
            registration.AutoRenewal = mcbRegistrationAutoRenewal.Checked;

            //set row
            SetRegistrationDataRow(row, registration);

            //refresh grid
            dgvRegistrations.Refresh();
        }

        /// <summary>
        /// Photo click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbPhoto_Click(object sender, EventArgs e)
        {
            //check if user is consulting
            if (this.Status == RegisterStatus.Consulting)
            {
                //check if any student is selected
                if (lsItems.SelectedIndex == -1)
                {
                    //no student is selected
                    //no need to select file
                    //exit
                    return;
                }
            }

            //display photo
            //create file form
            SelectFileForm fileForm = new SelectFileForm(
                photoFile, this.Status != RegisterStatus.Consulting);

            //display form as dialog and get result
            if (fileForm.ShowDialog(Manager.MainForm) != DialogResult.OK)
            {
                //user canceled operation
                //exit
                return;
            }

            //check if photo has changed
            if (photoFile.Equals(fileForm.SelectedFile))
            {
                //same file still
                //exit
                return;
            }

            //update photo file
            photoFile = fileForm.SelectedFile;

            //display thumbnail
            LoadFileThumbnail(photoFile, pbPhoto, null);
        }

        /// <summary>
        /// Assigment file click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbAssignment_Click(object sender, EventArgs e)
        {
            //check if user is consulting
            if (this.Status == RegisterStatus.Consulting)
            {
                //check if any student is selected
                if (lsItems.SelectedIndex == -1)
                {
                    //no student is selected
                    //no need to select file
                    //exit
                    return;
                }
            }

            //display assignment
            //create file form
            SelectFileForm fileForm = new SelectFileForm(
                assignmentFile, this.Status != RegisterStatus.Consulting);

            //display form as dialog and get result
            if (fileForm.ShowDialog(Manager.MainForm) != DialogResult.OK)
            {
                //user canceled operation
                //exit
                return;
            }

            //check if assignment has changed
            if (assignmentFile.Equals(fileForm.SelectedFile))
            {
                //same file still
                //exit
                return;
            }

            //update assignment file
            assignmentFile = fileForm.SelectedFile;

            //display thumbnail
            LoadFileThumbnail(assignmentFile, pbAssignment, null);
        }

        #endregion UI Event Handlers

    } //end of class RegisterStudentControl 

} //end of namespace PnT.SongClient.UI.Controls
