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
    /// List and display registrations to user.
    /// </summary>
    public partial class ViewRegistrationControl : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The song child control.
        /// </summary>
        ISongControl childControl = null;

        /// <summary>
        /// List of registrations shown on the control.
        /// </summary>
        private Dictionary<long, Registration> registrations = null;

        /// <summary>
        /// The last found registration.
        /// Used to improve the find method.
        /// </summary>
        private Registration lastFoundRegistration = null;

        /// <summary>
        /// DataTable for registrations.
        /// </summary>
        private DataTable dtRegistrations = null;

        /// <summary>
        /// The item displayable name.
        /// </summary>
        private string itemTypeDescription = Properties.Resources.item_Registration;

        /// <summary>
        /// The item displayable plural name.
        /// </summary>
        protected string itemTypeDescriptionPlural = Properties.Resources.item_plural_Registration;

        /// <summary>
        /// Indicates if the control is being loaded.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// Indicates if the control is loading instrument types.
        /// </summary>
        private bool isLoadingInstrumentTypes = false;

        /// <summary>
        /// Indicates if the control is loading poles.
        /// </summary>
        private bool isLoadingPoles = false;

        /// <summary>
        /// Indicates if the control is loading classes.
        /// </summary>
        private bool isLoadingClasses = false;

        /// <summary>
        /// Right-clicked registration. The registration of the displayed context menu.
        /// </summary>
        private Registration clickedRegistration = null;

        /// <summary>
        /// The registration ID column index in the datagridview.
        /// </summary>
        private int columnIndexRegistrationId;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ViewRegistrationControl()
        {
            //set loading flag
            isLoading = true;

            //init ui components
            InitializeComponent();

            //create list of registrations
            registrations = new Dictionary<long, Registration>();

            //create registration data table
            CreateRegistrationDataTable();

            //get registration ID column index
            columnIndexRegistrationId = dgvRegistrations.Columns[RegistrationId.Name].Index;

            //load combos
            //list statuses
            List<KeyValuePair<int, string>> statuses = new List<KeyValuePair<int, string>>();
            statuses.Add(new KeyValuePair<int, string>(
                -1, Properties.Resources.wordAll));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Active, Properties.Resources.ResourceManager.GetString("ItemStatus_Active")));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Evaded, Properties.Resources.ResourceManager.GetString("ItemStatus_Evaded")));
            mcbStatus.ValueMember = "Key";
            mcbStatus.DisplayMember = "Value";
            mcbStatus.DataSource = statuses;

            //list semesters
            ListSemesters();

            //list institutions
            ListInstitutions();

            //check if logged on user has an assigned institution
            if (Manager.LogonUser != null &&
                Manager.LogonUser.InstitutionId > 0)
            {
                //list assigned institution poles
                ListPoles(Manager.LogonUser.InstitutionId);

                //list institution classes
                ListClasses(
                    mcbSemester.SelectedIndex >-1 ? (int)mcbSemester.SelectedValue : -1, 
                    Manager.LogonUser.InstitutionId, -1);
            }
            else
            {
                //list all poles
                ListPoles(-1);

                //list all classes
                ListClasses(
                    mcbSemester.SelectedIndex > -1 ? (int)mcbSemester.SelectedValue : -1, -1, -1);
            }

            //subscribe settings property changed event
            Manager.Settings.PropertyChanged += Settings_PropertyChanged;
        }

        #endregion Constructors


        #region Properties ************************************************************

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


        #region ISong Methods *********************************************************

        /// <summary>
        /// Dispose used resources from registration control.
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
            //select registration
            return "Registration";
        }

        #endregion ISong Methods


        #region Public Methods ********************************************************

        /// <summary>
        /// Set columns visibility according to application settings.
        /// </summary>
        public void SetVisibleColumns()
        {
            //split visible column settings
            string[] columns = Manager.Settings.RegistrationGridDisplayedColumns.Split(',');

            //hide all columns
            foreach (DataGridViewColumn dgColumn in dgvRegistrations.Columns)
            {
                //hide column
                dgColumn.Visible = false;
            }

            //set invisible last index
            int lastIndex = dgvRegistrations.Columns.Count - 1;

            //check each column and set visibility
            foreach (DataGridViewColumn dgvColumn in dgvRegistrations.Columns)
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

                        //set column display index registration
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
        /// Create Registration data table.
        /// </summary>
        private void CreateRegistrationDataTable()
        {
            //create data table
            dtRegistrations = new DataTable();

            //RegistrationId
            DataColumn dcRegistrationId = new DataColumn("RegistrationId", typeof(int));
            dtRegistrations.Columns.Add(dcRegistrationId);

            //Semester
            DataColumn dcSemester = new DataColumn("SemesterName", typeof(string));
            dtRegistrations.Columns.Add(dcSemester);

            //ClassCode
            DataColumn dcClassCode = new DataColumn("ClassCode", typeof(string));
            dtRegistrations.Columns.Add(dcClassCode);

            //PoleName
            DataColumn dcPoleName = new DataColumn("PoleName", typeof(string));
            dtRegistrations.Columns.Add(dcPoleName);

            //StudentName
            DataColumn dcStudentName = new DataColumn("StudentName", typeof(string));
            dtRegistrations.Columns.Add(dcStudentName);

            //AutoRenewal
            DataColumn dcAutoRenewal = new DataColumn("AutoRenewal", typeof(string));
            dtRegistrations.Columns.Add(dcAutoRenewal);

            //RegistrationStatusName
            DataColumn dcRegistrationStatus = new DataColumn("RegistrationStatusName", typeof(string));
            dtRegistrations.Columns.Add(dcRegistrationStatus);

            //CreationTime
            DataColumn dcCreationTime = new DataColumn("CreationTime", typeof(DateTime));
            dtRegistrations.Columns.Add(dcCreationTime);

            //InactivationTime
            DataColumn dcInactivationTime = new DataColumn("InactivationTime", typeof(DateTime));
            dtRegistrations.Columns.Add(dcInactivationTime);

            //InactivationReason
            DataColumn dcInactivationReason = new DataColumn("InactivationReason", typeof(string));
            dtRegistrations.Columns.Add(dcInactivationReason);

            //set primary key column
            dtRegistrations.PrimaryKey = new DataColumn[] { dcRegistrationId };
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
                    foreach (Registration registrationObj in selectedRegistrations)
                    {
                        //check if registration is not in the list
                        if (!registrations.ContainsKey(registrationObj.RegistrationId))
                        {
                            //add registration to the list
                            registrations.Add(registrationObj.RegistrationId, registrationObj);

                            //set last found registration
                            lastFoundRegistration = registrationObj;
                        }
                        else
                        {
                            //should never happen
                            //log message
                            Manager.Log.WriteError(
                                "Error while loading registrations. Two registrations with same RegistrationID " +
                                registrationObj.RegistrationId + ".");

                            //throw exception
                            throw new ApplicationException(
                                "Error while loading registrations. Two registrations with same RegistrationID " +
                                registrationObj.RegistrationId + ".");
                        }
                    }
                }

                //lock data table of registrations
                lock (dtRegistrations)
                {
                    //check each registration in the list
                    foreach (Registration registrationObj in ListRegistrations)
                    {
                        //create, set and add registration row
                        DataRow dr = dtRegistrations.NewRow();
                        SetRegistrationDataRow(dr, registrationObj);
                        dtRegistrations.Rows.Add(dr);
                    }
                }
            }

            //refresh displayed UI
            RefreshUI(false);
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
        private Registration FindRegistration(long registrationID)
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
        /// List semesters into UI.
        /// </summary>
        private void ListSemesters()
        {
            //set default empty list to UI
            mcbSemester.ValueMember = "Id";
            mcbSemester.DisplayMember = "Description";
            mcbSemester.DataSource = new List<IdDescriptionStatus>();

            //load semesters
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
                //get list of all semesters
                List<IdDescriptionStatus> semesters = songChannel.ListSemesters();

                //check result
                if (semesters[0].Result == (int)SelectResult.Empty)
                {
                    //no semester is available
                    //clear list
                    semesters.Clear();
                }
                else if (semesters[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting semesters
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Semester, semesters[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Semester,
                        semesters[0].ErrorMessage));

                    //clear list
                    semesters.Clear();
                }

                //create all option and add it to list
                IdDescriptionStatus allOption = new IdDescriptionStatus(
                    -1, Properties.Resources.wordAll, 0);
                semesters.Insert(0, allOption);

                //set semesters to UI
                mcbSemester.ValueMember = "Id";
                mcbSemester.DisplayMember = "Description";
                mcbSemester.DataSource = semesters;

                //check if there is any semester to be selected 
                //other than all option
                if (semesters.Count > 1)
                {
                    //check current semester
                    if (Manager.CurrentSemester.Result == (int)SelectResult.Success)
                    {
                        //select current semester
                        mcbSemester.SelectedValue = Manager.CurrentSemester.SemesterId;
                    }
                }
            }
            catch (Exception ex)
            {
                //database error while getting semesters
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Semester), ex);

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
        /// List classes into UI for selected institution
        /// </summary>
        /// <param name="semesterId">
        /// The ID of the selected semester.
        /// -1 to select all semesters.
        /// </param>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// -1 to select all poles.
        /// </param>
        private void ListClasses(int semesterId, int institutionId, int poleId)
        {
            //set default empty list to UI
            mcbClass.ValueMember = "Id";
            mcbClass.DisplayMember = "Description";
            mcbClass.DataSource = new List<IdDescriptionStatus>();

            //load classes
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
                //get list of classes to be displayed
                List<IdDescriptionStatus> classes = songChannel.ListClassesByFilter(
                    (int)ItemStatus.Active, -1, -1, -1, semesterId, institutionId, poleId, -1);

                //check result
                if (classes[0].Result == (int)SelectResult.Success)
                {
                    //sort classes by description
                    classes.Sort((x, y) => x.Description.CompareTo(y.Description));
                }
                else if (classes[0].Result == (int)SelectResult.Empty)
                {
                    //no class is available
                    //clear list
                    classes.Clear();
                }
                else if (classes[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting classes
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Class, classes[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Class,
                        classes[0].ErrorMessage));

                    //clear list
                    classes.Clear();
                }

                //create all option and add it to list
                IdDescriptionStatus allOption = new IdDescriptionStatus(
                    -1, Properties.Resources.wordAllFeminine, 0);
                classes.Insert(0, allOption);

                //set classes to UI
                mcbClass.ValueMember = "Id";
                mcbClass.DisplayMember = "Description";
                mcbClass.DataSource = classes;
            }
            catch (Exception ex)
            {
                //database error while getting classes
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Class), ex);

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
        /// Load and display filtered registrations.
        /// </summary>
        /// <returns>
        /// True if registrations were loaded.
        /// False otherwise.
        /// </returns>
        private bool LoadRegistrations()
        {
            //filter and load registrations
            List<Registration> filteredRegistrations = null;

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
                //get list of registrations
                filteredRegistrations = songChannel.FindRegistrationsByFilter(
                    true, true, true, true,
                    (int)mcbStatus.SelectedValue,
                    (int)mcbSemester.SelectedValue,
                    (int)mcbInstitution.SelectedValue,
                    (int)mcbPole.SelectedValue,
                    -1,
                    (int)mcbClass.SelectedValue);

                //check result
                if (filteredRegistrations[0].Result == (int)SelectResult.Empty)
                {
                    //no registration was found
                    //clear list
                    filteredRegistrations.Clear();
                }
                else if (filteredRegistrations[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting registrations
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredRegistrations[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredRegistrations[0].ErrorMessage));

                    //could not load registrations
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

                //database error while getting registrations
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load registrations
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

            //sort registrations by class code
            filteredRegistrations.Sort((x, y) => x.Class.Code.CompareTo(y.Class.Code));

            //display filtered registrations
            DisplayRegistrations(filteredRegistrations);

            //sort registrations by class code by default
            dgvRegistrations.Sort(ClassCode, ListSortDirection.Ascending);

            //registrations were loaded
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
            if (dgvRegistrations.DataSource == null)
            {
                //set source to datagrid
                dgvRegistrations.DataSource = dtRegistrations;
            }

            //check if last row should be displayed
            //and if the is at least one row
            if (displayLastRow && dgvRegistrations.Rows.Count > 0)
            {
                //refresh grid by displaying last row
                dgvRegistrations.FirstDisplayedScrollingRowIndex = (dgvRegistrations.Rows.Count - 1);
            }

            //refresh grid
            dgvRegistrations.Refresh();

            //set number of registrations
            mlblItemCount.Text = registrations.Count + " " +
                (registrations.Count > 1 ? itemTypeDescriptionPlural : itemTypeDescription);
        }

        /// <summary>
        /// Set data row with selected Registration data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="registration">The selected registration.</param>
        private void SetRegistrationDataRow(DataRow dataRow, Registration registration)
        {
            dataRow["RegistrationId"] = registration.RegistrationId;
            dataRow["SemesterName"] = registration.Semester != null ?
                registration.Semester.Description : string.Empty;
            dataRow["ClassCode"] = registration.Class.Code;
            dataRow["PoleName"] = registration.PoleName;
            dataRow["StudentName"] = registration.StudentName;
            dataRow["AutoRenewal"] = registration.AutoRenewal ? "Auto" : string.Empty;
            dataRow["RegistrationStatusName"] = Properties.Resources.ResourceManager.GetString(
                "ItemStatus_" + ((ItemStatus)registration.RegistrationStatus).ToString());
            dataRow["CreationTime"] = registration.CreationTime;
            dataRow["InactivationReason"] = registration.InactivationReason;

            //set inactivation time
            if (registration.InactivationTime != DateTime.MinValue)
                dataRow["InactivationTime"] = registration.InactivationTime;
            else
                dataRow["InactivationTime"] = DBNull.Value;
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Remove displayed registration.
        /// </summary>
        /// <param name="registrationId">
        /// The ID of the registration to be removed.
        /// </param>
        public void RemoveRegistration(int registrationId)
        {
            //lock list of registrations
            lock (registrations)
            {
                //check if registration is not in the list
                if (!registrations.ContainsKey(registrationId))
                {
                    //no need to remove registration
                    //exit
                    return;
                }

                //remove registration
                registrations.Remove(registrationId);
            }

            //lock data table of registrations
            lock (dtRegistrations)
            {
                //get displayed data row
                DataRow dr = dtRegistrations.Rows.Find(registrationId);

                //remove displayed data row
                dtRegistrations.Rows.Remove(dr);
            }

            //refresh registration interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update a displayed registration. 
        /// Add registration if it is a new registration.
        /// </summary>
        /// <param name="registration">
        /// The updated registration.
        /// </param>
        public void UpdateRegistration(Registration registration)
        {
            //check registration should be displayed
            //status filter
            if (mcbStatus.SelectedIndex > 0 &&
                (int)mcbStatus.SelectedValue != registration.RegistrationStatus)
            {
                //registration should not be displayed
                //remove registration if it is being displayed
                RemoveRegistration(registration.RegistrationId);

                //exit
                return;
            }

            //semester filter
            if (mcbSemester.SelectedIndex > 0 &&
                (int)mcbSemester.SelectedValue != registration.Semester.SemesterId)
            {
                //registration should not be displayed
                //remove registration if it is being displayed
                RemoveRegistration(registration.RegistrationId);

                //exit
                return;
            }

            //pole filter
            if (mcbPole.SelectedIndex > 0 &&
                (int)mcbPole.SelectedValue != registration.PoleId)
            {
                //registration should not be displayed
                //remove registration if it is being displayed
                RemoveRegistration(registration.RegistrationId);

                //exit
                return;
            }

            ////institution filter
            ////no pole should be selected
            //if (mcbPole.SelectedIndex == -1 && 
            //    mcbInstitution.SelectedIndex > 0 &&
            //    (int)mcbInstitution.SelectedValue != registration.InstitutionId)
            //{
            //    //registration should not be displayed
            //    //remove registration if it is being displayed
            //    RemoveRegistration(registration.RegistrationId);

            //    //exit
            //    return;
            //}

            //class filter
            if (mcbClass.SelectedIndex > 0 &&
                (int)mcbClass.SelectedValue != registration.ClassId)
            {
                //registration should not be displayed
                //remove registration if it is being displayed
                RemoveRegistration(registration.RegistrationId);

                //exit
                return;
            }

            //lock list of registrations
            lock (registrations)
            {
                //set registration
                registrations[registration.RegistrationId] = registration;
            }

            //lock data table of registrations
            lock (dtRegistrations)
            {
                //get displayed data row
                DataRow dr = dtRegistrations.Rows.Find(registration.RegistrationId);

                //check if there was no data row yet
                if (dr == null)
                {
                    //create, set and add data row
                    dr = dtRegistrations.NewRow();
                    SetRegistrationDataRow(dr, registration);
                    dtRegistrations.Rows.Add(dr);
                }
                else
                {
                    //set data
                    SetRegistrationDataRow(dr, registration);
                }
            }

            //refresh registration interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed registrations for selected class.
        /// </summary>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        public void UpdateRegistrations(int classId)
        {
            //filter and load registrations for selected class
            List<Registration> updatedRegistrations = null;

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
                //get list of registrations for selected class
                //keep other filters on
                updatedRegistrations = songChannel.FindRegistrationsByFilter(
                    true, true, true, true,
                    (int)mcbStatus.SelectedValue,
                    (int)mcbSemester.SelectedValue,
                    (int)mcbInstitution.SelectedValue,
                    (int)mcbPole.SelectedValue,
                    -1,
                    classId);

                //check result
                if (updatedRegistrations[0].Result == (int)SelectResult.Empty)
                {
                    //no registration was found
                    //clear list
                    updatedRegistrations.Clear();
                }
                else if (updatedRegistrations[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting registrations
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, updatedRegistrations[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, updatedRegistrations[0].ErrorMessage));

                    //could not load registrations
                    //exit
                    return;
                }
            }
            catch (Exception ex)
            {
                //show error message
                MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //database error while getting registrations
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load registrations
                //exit
                return;
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

            //check each updated registration
            foreach (Registration registration in updatedRegistrations)
            {
                //lock list of registrations
                lock (registrations)
                {
                    //set registration
                    registrations[registration.RegistrationId] = registration;
                }

                //lock data table of registrations
                lock (dtRegistrations)
                {
                    //get displayed data row
                    DataRow dr = dtRegistrations.Rows.Find(registration.RegistrationId);

                    //check if there was no data row yet
                    if (dr == null)
                    {
                        //create, set and add data row
                        dr = dtRegistrations.NewRow();
                        SetRegistrationDataRow(dr, registration);
                        dtRegistrations.Rows.Add(dr);
                    }
                    else
                    {
                        //set data
                        SetRegistrationDataRow(dr, registration);
                    }
                }
            }

            //list of current registrations
            List<Registration> currentRegistrations = null;

            //lock list of registrations
            lock (registrations)
            {
                //get list of current registrations
                currentRegistrations = new List<Registration>(registrations.Values);
            }

            //check each current registration
            foreach (Registration currentRegistration in currentRegistrations)
            {
                //check if registration is for selected class
                if (currentRegistration.ClassId != classId)
                {
                    //not the same class
                    //go to next registration
                    continue;
                }

                //check if registration is not in the updated list
                if (updatedRegistrations.Find(
                    r => r.RegistrationId == currentRegistration.RegistrationId) == null)
                {
                    //must remove registration
                    //lock list of registrations
                    lock (registrations)
                    {
                        //remove registration
                        registrations.Remove(currentRegistration.RegistrationId);
                    }

                    //lock data table of registrations
                    lock (dtRegistrations)
                    {
                        //get displayed data row
                        DataRow dr = dtRegistrations.Rows.Find(currentRegistration.RegistrationId);

                        //remove displayed data row
                        dtRegistrations.Rows.Remove(dr);
                    }
                }
            }

            //refresh registration interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed registrations with class data.
        /// </summary>
        /// <param name="classObj">
        /// The updated class.
        /// </param>
        public void UpdateClass(Class classObj)
        {
            //check each class item
            foreach (IdDescriptionStatus item in mcbClass.Items)
            {
                //compare data
                if (item.Id == classObj.ClassId &&
                    !item.Description.Equals(classObj.Code))
                {
                    //update item
                    item.Description = classObj.Code;

                    //check if item is selected
                    if (mcbClass.SelectedItem == item)
                    {
                        //set loading flag
                        isLoading = true;

                        //clear selection and reselect item
                        mcbClass.SelectedIndex = -1;
                        mcbClass.SelectedItem = item;

                        //reset loading flag
                        isLoading = false;
                    }

                    //exit loop
                    break;
                }
            }

            //gather list of updated registrations
            List<Registration> updatedRegistrations = new List<Registration>();

            //lock list
            lock (registrations)
            {
                //check all displayed registrations
                foreach (Registration registrationObj in registrations.Values)
                {
                    //check registration class
                    if (registrationObj.ClassId == classObj.ClassId &&
                        !registrationObj.Class.Code.Equals(classObj.Code))
                    {
                        //update registration
                        registrationObj.Class.Code = classObj.Code;

                        //add registration to the list of updated registrations
                        updatedRegistrations.Add(registrationObj);
                    }
                }
            }

            //check result
            if (updatedRegistrations.Count == 0)
            {
                //no registration was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of registrations
            lock (dtRegistrations)
            {
                //check each updated role
                foreach (Registration registrationObj in updatedRegistrations)
                {
                    //get displayed data row
                    DataRow dr = dtRegistrations.Rows.Find(registrationObj.RegistrationId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetRegistrationDataRow(dr, registrationObj);
                    }
                }
            }

            //refresh class interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed registrations with pole data.
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

            //gather list of updated registrations
            List<Registration> updatedRegistrations = new List<Registration>();

            //lock list
            lock (registrations)
            {
                //check all displayed registrations
                foreach (Registration registration in registrations.Values)
                {
                    //check registration pole
                    if (registration.PoleId == pole.PoleId &&
                        !registration.PoleName.Equals(pole.Name))
                    {
                        //update registration
                        registration.PoleName = pole.Name;

                        //add registration to the list of updated registrations
                        updatedRegistrations.Add(registration);
                    }
                }
            }

            //check result
            if (updatedRegistrations.Count == 0)
            {
                //no registration was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of registrations
            lock (dtRegistrations)
            {
                //check each updated role
                foreach (Registration registrationObj in updatedRegistrations)
                {
                    //get displayed data row
                    DataRow dr = dtRegistrations.Rows.Find(registrationObj.RegistrationId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetRegistrationDataRow(dr, registrationObj);
                    }
                }
            }

            //refresh pole interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed registrations with student data.
        /// </summary>
        /// <param name="student">
        /// The updated student.
        /// </param>
        public void UpdateStudent(Student student)
        {
            //gather list of updated registrations
            List<Registration> updatedRegistrations = new List<Registration>();

            //lock list
            lock (registrations)
            {
                //check all displayed registrations
                foreach (Registration registration in registrations.Values)
                {
                    //check registration student
                    if (registration.StudentId == student.StudentId &&
                        !registration.StudentName.Equals(student.Name))
                    {
                        //update registration
                        registration.StudentName = student.Name;

                        //add registration to the list of updated registrations
                        updatedRegistrations.Add(registration);
                    }
                }
            }

            //check result
            if (updatedRegistrations.Count == 0)
            {
                //no registration was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of registrations
            lock (dtRegistrations)
            {
                //check each updated role
                foreach (Registration registrationObj in updatedRegistrations)
                {
                    //get displayed data row
                    DataRow dr = dtRegistrations.Rows.Find(registrationObj.RegistrationId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetRegistrationDataRow(dr, registrationObj);
                    }
                }
            }

            //refresh student interface
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
                dgvRegistrations.ColumnHeadersDefaultCellStyle.Font =
                    MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
                dgvRegistrations.DefaultCellStyle.Font =
                    MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            }
            else if (e.PropertyName.Equals("RegistrationGridDisplayedColumns"))
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
        private void ViewRegistrationControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //avoid auto generated columns
            dgvRegistrations.AutoGenerateColumns = false;

            //set fonts
            dgvRegistrations.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvRegistrations.DefaultCellStyle.Font =
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

            //clear number of registrations
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

            //load registrations
            LoadRegistrations();
        }

        /// <summary>
        /// Semester combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading)
            {
                //no need to handle this event
                //exit
                return;
            }

            //set flag is loading classes
            isLoadingClasses = true;

            //get id of current selected class
            int selectedClassId = mcbClass.SelectedIndex >= 0 ?
                (int)mcbClass.SelectedValue : int.MinValue;

            //reload class list
            ListClasses(
                mcbSemester.SelectedIndex >= 0 ? (int)mcbSemester.SelectedValue : -1,
                mcbInstitution.SelectedIndex >= 0 ? (int)mcbInstitution.SelectedValue : -1,
                mcbPole.SelectedIndex >= 0 ? (int)mcbPole.SelectedValue : -1);

            //try to reselet previous selected class
            mcbClass.SelectedValue = selectedClassId;

            //check result
            if (mcbClass.SelectedIndex < 0)
            {
                //select all option
                mcbClass.SelectedIndex = 0;
            }

            //reset flag is loading classes
            isLoadingClasses = false;

            //load registrations
            LoadRegistrations();
        }

        /// <summary>
        /// Institution combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading || isLoadingInstrumentTypes)
            {
                //no need to handle this event
                //exit
                return;
            }

            //set flag is loading poles
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

            //reset flag is loading poles
            isLoadingPoles = false;

            //set flag is loading classes
            isLoadingClasses = true;

            //get id of current selected class
            int selectedClassId = mcbClass.SelectedIndex >= 0 ?
                (int)mcbClass.SelectedValue : int.MinValue;

            //reload class list
            ListClasses(
                mcbSemester.SelectedIndex >= 0 ? (int)mcbSemester.SelectedValue : -1,
                mcbInstitution.SelectedIndex >= 0 ? (int)mcbInstitution.SelectedValue : -1,
                mcbPole.SelectedIndex >= 0 ? (int)mcbPole.SelectedValue : -1);

            //try to reselet previous selected class
            mcbClass.SelectedValue = selectedClassId;

            //check result
            if (mcbClass.SelectedIndex < 0)
            {
                //select all option
                mcbClass.SelectedIndex = 0;
            }

            //reset flag is loading classes
            isLoadingClasses = false;

            //load registrations
            LoadRegistrations();
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

            //set flag is loading classes
            isLoadingClasses = true;

            //get id of current selected class
            int selectedClassId = mcbClass.SelectedIndex >= 0 ?
                (int)mcbClass.SelectedValue : int.MinValue;

            //reload class list
            ListClasses(
                mcbSemester.SelectedIndex >= 0 ? (int)mcbSemester.SelectedValue : -1,
                mcbInstitution.SelectedIndex >= 0 ? (int)mcbInstitution.SelectedValue : -1,
                mcbPole.SelectedIndex >= 0 ? (int)mcbPole.SelectedValue : -1);

            //try to reselet previous selected class
            mcbClass.SelectedValue = selectedClassId;

            //check result
            if (mcbClass.SelectedIndex < 0)
            {
                //select all option
                mcbClass.SelectedIndex = 0;
            }

            //reset flag is loading classes
            isLoadingClasses = false;

            //load registrations
            LoadRegistrations();
        }

        /// <summary>
        /// Class combo box selected index changed event handler. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading || isLoadingClasses)
            {
                //no need to handle this event
                //exit
                return;
            }

            //load registrations
            LoadRegistrations();
        }

        /// <summary>
        /// Edit tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlEdit_Click(object sender, EventArgs e)
        {
            //create control
            RegisterClassControl registerControl =
                new UI.Controls.RegisterClassControl();
            registerControl.ParentControl = this;

            //check if there is any selected registration
            if (dgvRegistrations.SelectedCells.Count > 0)
            {
                //get id of selected registration
                int registrationId = (int)dgvRegistrations.Rows[dgvRegistrations.SelectedCells[0].RowIndex].Cells[columnIndexRegistrationId].Value;

                //get registration
                Registration registration = FindRegistration(registrationId);

                //check result
                if (registration != null)
                {
                    //select first selected registration class in the register control
                    registerControl.FirstSelectedId = registration.ClassId;
                }
            }

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Registrations datagridview mouse up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRegistrations_MouseUp(object sender, MouseEventArgs e)
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
            DataGridView.HitTestInfo hitInfo = dgvRegistrations.HitTest(e.X, e.Y);

            //check selected row index
            if (hitInfo.RowIndex < 0)
            {
                //no row selected
                //exit
                return;
            }

            //check if there are selected rows and if registration clicked on them
            if (dgvRegistrations.SelectedRows.Count > 0 &&
                dgvRegistrations.Rows[hitInfo.RowIndex].Selected != true)
            {
                //registration did not click in the selected rows
                //clear selection
                dgvRegistrations.ClearSelection();

                //select clicked row
                dgvRegistrations.Rows[hitInfo.RowIndex].Selected = true;
            }
            //check if there are selected cells
            else if (dgvRegistrations.SelectedCells.Count > 0)
            {
                //get list of selected cell rows
                HashSet<int> selectedRowIndexes = new HashSet<int>();

                //check each selected cell
                foreach (DataGridViewCell cell in dgvRegistrations.SelectedCells)
                {
                    //add cell row index to the list
                    selectedRowIndexes.Add(cell.RowIndex);
                }

                //check if registration clicked on a row of a selected cell
                if (selectedRowIndexes.Contains(hitInfo.RowIndex))
                {
                    //registration clicked on a row of a selected cell
                    //select all rows
                    foreach (int selectedRowIndex in selectedRowIndexes)
                    {
                        //select row
                        dgvRegistrations.Rows[selectedRowIndex].Selected = true;
                    }
                }
                else
                {
                    //registration did not click on a row of a selected cell
                    //clear selected cells
                    dgvRegistrations.ClearSelection();

                    //select clicked row
                    dgvRegistrations.Rows[hitInfo.RowIndex].Selected = true;
                }
            }
            else
            {
                //select clicked row
                dgvRegistrations.Rows[hitInfo.RowIndex].Selected = true;
            }

            //get selected or clicked registration
            clickedRegistration = null;

            //check if there is a selected registration
            if (dgvRegistrations.SelectedRows.Count > 0)
            {
                //there is one or more registrations selected
                //get first selected registration
                for (int index = 0; index < dgvRegistrations.SelectedRows.Count; index++)
                {
                    //get registration using its registration id
                    int registrationId = (int)dgvRegistrations.SelectedRows[index].Cells[columnIndexRegistrationId].Value;
                    Registration registrationObj = FindRegistration(registrationId);

                    //check result
                    if (registrationObj != null)
                    {
                        //add registration to list
                        clickedRegistration = registrationObj;

                        //exit loop
                        break;
                    }
                }

                //check result
                if (clickedRegistration == null)
                {
                    //no registration was found
                    //should never happen
                    //do not display context menu
                    //exit
                    return;
                }
            }
            else
            {
                //there is no registration selected
                //should never happen
                //do not display context menu
                //exit
                return;
            }

            //set context menu items visibility
            if (dgvRegistrations.SelectedRows.Count == 1)
            {
                //display view menu items according to permissions
                mnuViewClass.Visible = Manager.HasLogonPermission("Class.View");
                mnuViewPole.Visible = Manager.HasLogonPermission("Pole.View");
                tssSeparator.Visible = Manager.HasLogonPermission("Class.View") ||
                    Manager.HasLogonPermission("Pole.View");
            }
            else
            {
                //hide view menu items
                mnuViewClass.Visible = false;
                mnuViewPole.Visible = false;
                tssSeparator.Visible = false;
            }

            //show registration context menu on the clicked point
            mcmRegistration.Show(this.dgvRegistrations, p);
        }

        /// <summary>
        /// View class menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewClass_Click(object sender, EventArgs e)
        {
            //check clicked registration
            if (clickedRegistration == null)
            {
                //should never happen
                //exit
                return;
            }

            //check registration class
            if (clickedRegistration.ClassId <= 0)
            {
                //no class
                //should never happen
                //exit
                return;
            }

            //create control to display selected class
            RegisterClassControl registerControl =
                new UI.Controls.RegisterClassControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedRegistration.ClassId;

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
            //check clicked class
            if (clickedRegistration == null)
            {
                //should never happen
                //exit
                return;
            }

            //check class pole
            if (clickedRegistration.PoleId <= 0)
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
            registerControl.FirstSelectedId = clickedRegistration.PoleId;

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
            if (this.dgvRegistrations.GetCellCount(DataGridViewElementStates.Selected) <= 0)
            {
                //should never happen
                //exit
                return;
            }

            try
            {
                //include header text 
                dgvRegistrations.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                //copy selection to the clipboard
                Clipboard.SetDataObject(this.dgvRegistrations.GetClipboardContent());
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

    } //end of class ViewRegistrationControl

} //end of namespace PnT.SongClient.UI.Controls
