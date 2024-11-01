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
    /// List and display reports to user.
    /// </summary>
    public partial class ViewReportControl : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The song child control.
        /// </summary>
        ISongControl childControl = null;

        /// <summary>
        /// List of reports shown on the control.
        /// </summary>
        private Dictionary<long, Report> reports = null;

        /// <summary>
        /// The last found report.
        /// Used to improve the find method.
        /// </summary>
        private Report lastFoundReport = null;

        /// <summary>
        /// DataTable for reports.
        /// </summary>
        private DataTable dtReports = null;

        /// <summary>
        /// The item displayable name.
        /// </summary>
        private string itemTypeDescription = Properties.Resources.item_Report;

        /// <summary>
        /// The item displayable plural name.
        /// </summary>
        protected string itemTypeDescriptionPlural = Properties.Resources.item_plural_Report;

        /// <summary>
        /// Indicates if the control is being loaded.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// Indicates if the control is loading classes.
        /// </summary>
        private bool isLoadingClasses = false;

        /// <summary>
        /// Indicates if the control is loading teachers.
        /// </summary>
        private bool isLoadingTeachers = false;

        /// <summary>
        /// Right-clicked report. The report of the displayed context menu.
        /// </summary>
        private Report clickedReport = null;

        /// <summary>
        /// The report ID column index in the datagridview.
        /// </summary>
        private int columnIndexReportId;

        /// <summary>
        /// The pre selected semester ID filter.
        /// </summary>
        private int filterSemesterId = -1;

        /// <summary>
        /// The pre selected class ID filter.
        /// </summary>
        private int filterClassId = -1;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ViewReportControl()
        {
            //set loading flag
            isLoading = true;

            //init ui components
            InitializeComponent();

            //create list of reports
            reports = new Dictionary<long, Report>();

            //create report data table
            CreateReportDataTable();

            //get report ID column index
            columnIndexReportId = dgvReports.Columns[ReportId.Name].Index;

            //load combos
            //list reoprt statuses
            List<KeyValuePair<int, string>> statuses = new List<KeyValuePair<int, string>>();
            statuses.Add(new KeyValuePair<int, string>(
                -1, Properties.Resources.wordAll));
            statuses.Add(new KeyValuePair<int, string>((int)PnT.SongDB.Logic.ReportStatus.Completed,
                Properties.Resources.ResourceManager.GetString("ReportStatus_Completed")));
            statuses.Add(new KeyValuePair<int, string>((int)PnT.SongDB.Logic.ReportStatus.Pending, 
                Properties.Resources.ResourceManager.GetString("ReportStatus_Pending")));
            mcbStatus.ValueMember = "Key";
            mcbStatus.DisplayMember = "Value";
            mcbStatus.DataSource = statuses;

            //list semesters
            ListSemesters();

            //list report types
            ListReportTypes();

            //list report periodicity
            ListReportPeriodicity();

            //list institutions
            ListInstitutions();

            //check if logged on user has an assigned institution
            if (Manager.LogonUser != null &&
                Manager.LogonUser.InstitutionId > 0)
            {
                //list assigned institution teachers
                ListTeachers(Manager.LogonUser.InstitutionId);

                //list institution classes
                ListClasses(
                    mcbSemester.SelectedIndex > -1 ? (int)mcbSemester.SelectedValue : -1,
                    Manager.LogonUser.InstitutionId, -1);
            }
            else
            {
                //list all teachers
                ListTeachers(-1);

                //list all classes
                ListClasses(mcbSemester.SelectedIndex > -1 ? 
                    (int)mcbSemester.SelectedValue : -1, -1, -1);
            }

            //subscribe settings property changed event
            Manager.Settings.PropertyChanged += Settings_PropertyChanged;
        }

        /// <summary>
        /// Overloaded constructor.
        /// Set selected filters when displaying control for the first time.
        /// </summary>
        /// <param name="filterSemesterId">
        /// Selected semester ID filter.
        /// -1 to select default semester.
        /// </param>
        /// <param name="filterClassId">
        /// Selected class ID filter.
        /// -1 to select all classes.
        /// </param>
        public ViewReportControl(int filterSemesterId, int filterClassId) : this()
        {
            //set pre filters
            this.filterSemesterId = filterSemesterId;
            this.filterClassId = filterClassId;
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get list of reports.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public List<Report> ListReports
        {
            get
            {
                //lock list of reports
                lock (reports)
                {
                    return new List<Report>(reports.Values);
                }
            }
        }

        #endregion Properties


        #region ISong Methods *********************************************************

        /// <summary>
        /// Dispose used resources from report control.
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
            //select report
            return "Report";
        }

        #endregion ISong Methods


        #region Public Methods ********************************************************

        /// <summary>
        /// Set columns visibility according to application settings.
        /// </summary>
        public void SetVisibleColumns()
        {
            //split visible column settings
            string[] columns = Manager.Settings.ReportGridDisplayedColumns.Split(',');

            //hide all columns
            foreach (DataGridViewColumn dgColumn in dgvReports.Columns)
            {
                //hide column
                dgColumn.Visible = false;
            }

            //set invisible last index
            int lastIndex = dgvReports.Columns.Count - 1;

            //check each column and set visibility
            foreach (DataGridViewColumn dgvColumn in dgvReports.Columns)
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

                        //set column display index report
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
        /// Create Report data table.
        /// </summary>
        private void CreateReportDataTable()
        {
            //create data table
            dtReports = new DataTable();

            //ReportId
            DataColumn dcReportId = new DataColumn("ReportId", typeof(int));
            dtReports.Columns.Add(dcReportId);

            //Semester
            DataColumn dcSemester = new DataColumn("SemesterName", typeof(string));
            dtReports.Columns.Add(dcSemester);

            //PeriodName
            DataColumn dcPeriodName = new DataColumn("PeriodName", typeof(string));
            dtReports.Columns.Add(dcPeriodName);

            //TypeName
            DataColumn dcTypeName = new DataColumn("TypeName", typeof(string));
            dtReports.Columns.Add(dcTypeName);

            //AuthorName
            DataColumn dcAuthorName = new DataColumn("AuthorName", typeof(string));
            dtReports.Columns.Add(dcAuthorName);

            //InstitutionName
            DataColumn dcInstitutionName = new DataColumn("InstitutionName", typeof(string));
            dtReports.Columns.Add(dcInstitutionName);

            //ClassData
            DataColumn dcClassData = new DataColumn("ClassData", typeof(string));
            dtReports.Columns.Add(dcClassData);

            //ReportStatusName
            DataColumn dcReportStatus = new DataColumn("ReportStatusName", typeof(string));
            dtReports.Columns.Add(dcReportStatus);

            //set primary key column
            dtReports.PrimaryKey = new DataColumn[] { dcReportId };
        }

        /// <summary>
        /// Display selected reports.
        /// Clear currently displayed reports before loading selected reports.
        /// </summary>
        /// <param name="selectedReports">
        /// The selected reports to be loaded.
        /// </param>
        private void DisplayReports(List<Report> selectedReports)
        {
            //lock list of reports
            lock (this.reports)
            {
                //clear list
                this.reports.Clear();

                //reset last found report
                lastFoundReport = null;
            }

            //lock datatable of reports
            lock (dtReports)
            {
                //clear datatable
                dtReports.Clear();
            }

            //check number of selected reports
            if (selectedReports != null && selectedReports.Count > 0 &&
                selectedReports[0].Result == (int)SelectResult.Success)
            {
                //lock list of reports
                lock (reports)
                {
                    //add selected reports
                    foreach (Report reportObj in selectedReports)
                    {
                        //check if report is not in the list
                        if (!reports.ContainsKey(reportObj.ReportId))
                        {
                            //add report to the list
                            reports.Add(reportObj.ReportId, reportObj);

                            //set last found report
                            lastFoundReport = reportObj;
                        }
                        else
                        {
                            //should never happen
                            //log message
                            Manager.Log.WriteError(
                                "Error while loading reports. Two reports with same ReportID " +
                                reportObj.ReportId + ".");

                            //throw exception
                            throw new ApplicationException(
                                "Error while loading reports. Two reports with same ReportID " +
                                reportObj.ReportId + ".");
                        }
                    }
                }

                //lock data table of reports
                lock (dtReports)
                {
                    //check each report in the list
                    foreach (Report reportObj in ListReports)
                    {
                        //create, set and add report row
                        DataRow dr = dtReports.NewRow();
                        SetReportDataRow(dr, reportObj);
                        dtReports.Rows.Add(dr);
                    }
                }
            }

            //refresh displayed UI
            RefreshUI(false);
        }

        /// <summary>
        /// Find report in the list of reports.
        /// </summary>
        /// <param name="reportID">
        /// The ID of the selected report.
        /// </param>
        /// <returns>
        /// The report of the selected report ID.
        /// Null if report was not found.
        /// </returns>
        private Report FindReport(long reportID)
        {
            //lock list of reports
            lock (reports)
            {
                //check last found report
                if (lastFoundReport != null &&
                    lastFoundReport.ReportId == reportID)
                {
                    //same report
                    return lastFoundReport;
                }

                //try to find selected report
                reports.TryGetValue(reportID, out lastFoundReport);

                //return result
                return lastFoundReport;
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
        /// <param name="teacherId">
        /// The ID of the selected teacher.
        /// -1 to select all teachers.
        /// </param>
        private void ListClasses(int semesterId, int institutionId, int teacherId)
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
                    (int)ItemStatus.Active, -1, -1, -1, semesterId, institutionId, -1, teacherId);

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
        /// List report types into UI.
        /// </summary>
        private void ListReportTypes()
        {
            //create list of report types
            List<KeyValuePair<int, string>> reportTypes = new List<KeyValuePair<int, string>>();
            
            //check if a teacher is viewing its reports
            if (Manager.LogonTeacher != null &&
                !Manager.HasLogonPermission("Report.View"))
            {
                //user is a teacher and has no report view permission
                //can only view reports for logon teacher
                //add teacher report type
                reportTypes.Add(new KeyValuePair<int, string>(
                    (int)ReportRapporteur.Teacher, 
                    Properties.Resources.ResourceManager.GetString("ReportRapporteur_Teacher")
                    ));
            }
            else
            {
                //check each report type
                foreach (ReportRapporteur reportRapporteur in Enum.GetValues(typeof(ReportRapporteur)))
                {
                    //add converted report type
                    reportTypes.Add(new KeyValuePair<int, string>(
                        (int)reportRapporteur, Properties.Resources.ResourceManager.GetString(
                            "ReportRapporteur_" + reportRapporteur.ToString())
                        ));
                }

                //create all option and add it to list
                reportTypes.Insert(0, new KeyValuePair<int, string>(
                    -1, Properties.Resources.wordAll));
            }

            //set report types to UI
            mcbReportType.ValueMember = "Key";
            mcbReportType.DisplayMember = "Value";
            mcbReportType.DataSource = reportTypes;
        }

        /// <summary>
        /// List report periodicities into UI.
        /// </summary>
        private void ListReportPeriodicity()
        {
            //create list of instrument types
            List<KeyValuePair<int, string>> instrumentTypes = new List<KeyValuePair<int, string>>();

            //check each instrument type
            foreach (ReportPeriodicity periodicity in Enum.GetValues(typeof(ReportPeriodicity)))
            {
                //add converted instrument type
                instrumentTypes.Add(new KeyValuePair<int, string>(
                    (int)periodicity, Properties.Resources.ResourceManager.GetString(
                        "ReportPeriodicity_" + periodicity.ToString())
                    ));
            }

            //create all option and add it to list
            instrumentTypes.Insert(0, new KeyValuePair<int, string>(
                -1, Properties.Resources.wordAllFeminine));

            //set instrument types to UI
            mcbPeriodicity.ValueMember = "Key";
            mcbPeriodicity.DisplayMember = "Value";
            mcbPeriodicity.DataSource = instrumentTypes;
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

                //option to create an all option
                bool createAllOption = true;

                //check if a teacher is viewing its reports
                if (Manager.LogonTeacher != null &&
                    !Manager.HasLogonPermission("Report.View"))
                {
                    //get list of active institutions
                    institutions = songChannel.ListInstitutionsByStatus((int)ItemStatus.Active);
                }
                //check if logged on user has an assigned institution
                else if (Manager.LogonUser != null &&
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

                    //does not create an all option
                    createAllOption = false;
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

                //check if should create an all option
                if (createAllOption)
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

                ////create all option and add it to list
                //IdDescriptionStatus allOption = new IdDescriptionStatus(
                //    -1, Properties.Resources.wordAll, 0);
                //semesters.Insert(0, allOption);

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
        /// List teachers into UI for selected institution.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// -1 to select all institutions.
        /// </param>
        private void ListTeachers(int institutionId)
        {
            //set default empty list to UI
            mcbTeacher.ValueMember = "Id";
            mcbTeacher.DisplayMember = "Description";
            mcbTeacher.DataSource = new List<IdDescriptionStatus>();

            //load teachers
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
                //list of teachers to be displayed
                List<IdDescriptionStatus> teachers = null;

                //option to create an all option
                bool createAllOption = true;

                //check if a teacher is viewing its reports
                if (Manager.LogonTeacher != null &&
                    !Manager.HasLogonPermission("Report.View"))
                {
                    //user is a teacher and has no report view permission
                    //can only view reports for logon teacher
                    //create list with only logon teacher
                    teachers = new List<IdDescriptionStatus>();
                    teachers.Add(Manager.LogonTeacher.GetDescription());

                    //does not create an all option
                    createAllOption = false;
                }
                //check selected institution
                else if (institutionId > 0)
                {
                    //get list of institution active teachers
                    teachers = songChannel.ListTeachersByInstitution(
                        institutionId, (int)ItemStatus.Active);
                }
                else
                {
                    //get list of all active teachers
                    teachers = songChannel.ListTeachersByStatus((int)ItemStatus.Active);
                }

                //check result
                if (teachers[0].Result == (int)SelectResult.Success)
                {
                    //sort teachers by description
                    teachers.Sort((x, y) => x.Description.CompareTo(y.Description));
                }
                else if (teachers[0].Result == (int)SelectResult.Empty)
                {
                    //no teacher is available
                    //clear list
                    teachers.Clear();
                }
                else if (teachers[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting teachers
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Teacher, teachers[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Teacher,
                        teachers[0].ErrorMessage));

                    //clear list
                    teachers.Clear();
                }

                //check if should create an all option
                if (createAllOption)
                {
                    //create all option and add it to list
                    IdDescriptionStatus allOption = new IdDescriptionStatus(
                        -1, Properties.Resources.wordAll, 0);
                        teachers.Insert(0, allOption);
                }

                //set teachers to UI
                mcbTeacher.ValueMember = "Id";
                mcbTeacher.DisplayMember = "Description";
                mcbTeacher.DataSource = teachers;
            }
            catch (Exception ex)
            {
                //database error while getting teachers
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Teacher), ex);

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
        /// Load and display filtered reports.
        /// </summary>
        /// <returns>
        /// True if reports were loaded.
        /// False otherwise.
        /// </returns>
        private bool LoadReports()
        {
            //filter and load reports
            List<Report> filteredReports = null;

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
                //get list of reports
                filteredReports = songChannel.FindReportsByFilter(
                    true, true, true, true, true,
                    (int)mcbStatus.SelectedValue,
                    (int)mcbReportType.SelectedValue,
                    (int)mcbPeriodicity.SelectedValue,
                    (int)mcbSemester.SelectedValue,
                    DateTime.MinValue,
                    (int)mcbInstitution.SelectedValue,
                    (int)mcbTeacher.SelectedValue,
                    (int)mcbClass.SelectedValue);

                //check result
                if (filteredReports[0].Result == (int)SelectResult.Empty)
                {
                    //no report was found
                    //clear list
                    filteredReports.Clear();
                }
                else if (filteredReports[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting reports
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredReports[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredReports[0].ErrorMessage));

                    //could not load reports
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

                //database error while getting reports
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load reports
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

            //sort reports by code
            filteredReports.Sort((x, y) => x.ReportId.CompareTo(y.ReportId));

            //display filtered reports
            DisplayReports(filteredReports);

            //sort reports by semester by default
            dgvReports.Sort(PeriodName, ListSortDirection.Ascending);

            //reports were loaded
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
            if (dgvReports.DataSource == null)
            {
                //set source to datagrid
                dgvReports.DataSource = dtReports;
            }

            //check if last row should be displayed
            //and if the is at least one row
            if (displayLastRow && dgvReports.Rows.Count > 0)
            {
                //refresh grid by displaying last row
                dgvReports.FirstDisplayedScrollingRowIndex = (dgvReports.Rows.Count - 1);
            }

            //refresh grid
            dgvReports.Refresh();

            //set number of reports
            mlblItemCount.Text = reports.Count + " " +
                (reports.Count > 1 ? itemTypeDescriptionPlural : itemTypeDescription);

            //set number of completed reports
            mlblCompletedCount.Text = ListReports.FindAll(
                r => r.ReportStatus == (int)PnT.SongDB.Logic.ReportStatus.Completed).Count +
                " " + Properties.Resources.wordCompletedPlural;

            //set number of pending reports
            mlblPendingCount.Text = ListReports.FindAll(
                r => r.ReportStatus == (int)PnT.SongDB.Logic.ReportStatus.Pending).Count +
                " " + Properties.Resources.wordPendingPlural;
        }

        /// <summary>
        /// Set data row with selected Report data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="report">The selected report.</param>
        private void SetReportDataRow(DataRow dataRow, Report report)
        {
            dataRow["ReportId"] = report.ReportId;
            dataRow["SemesterName"] = report.SemesterDescription;
            dataRow["PeriodName"] = report.ReportPeriodicity == (int)ReportPeriodicity.Semester ?
                report.SemesterDescription + ".Final"  : report.SemesterDescription + "." + report.ReferenceDate.Month +
                " " + Properties.Resources.ResourceManager.GetString("Month_" + report.ReferenceDate.Month);
            dataRow["TypeName"] = Properties.Resources.ResourceManager.GetString(
                    "ReportRapporteur_" + ((ReportRapporteur)report.ReportRapporteur).ToString());
            dataRow["AuthorName"] = report.ReportRapporteur == (int)ReportRapporteur.Teacher ? 
                report.TeacherName : report.CoordinatorName;
            dataRow["InstitutionName"] = report.InstitutionName;
            dataRow["ClassData"] = report.Class != null ? 
                Manager.GetClassDescription(report.Class, false) : "-";
            dataRow["ReportStatusName"] = Properties.Resources.ResourceManager.GetString(
                "ReportStatus_" + ((ReportStatus)report.ReportStatus).ToString());
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Remove displayed report.
        /// </summary>
        /// <param name="reportId">
        /// The ID of the report to be removed.
        /// </param>
        public void RemoveReport(int reportId)
        {
            //lock list of reports
            lock (reports)
            {
                //check if report is not in the list
                if (!reports.ContainsKey(reportId))
                {
                    //no need to remove report
                    //exit
                    return;
                }

                //remove report
                reports.Remove(reportId);
            }

            //lock data table of reports
            lock (dtReports)
            {
                //get displayed data row
                DataRow dr = dtReports.Rows.Find(reportId);

                //remove displayed data row
                dtReports.Rows.Remove(dr);
            }

            //refresh report interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update the status of a displayed report.
        /// </summary>
        /// <param name="reportId">
        /// The ID of the selected report.
        /// </param>
        /// <param name="reportStatus">
        /// The updated status of the report.
        /// </param>
        public void UpdateReport(int reportId, int reportStatus)
        {
            //the report to be updated
            Report reportObj = null;

            //lock list of reports
            lock (reports)
            {
                //try to find report
                if (!reports.TryGetValue(reportId, out reportObj))
                {
                    //report was not found
                    //no need to update report
                    //exit
                    return;
                }
            }

            //update status
            reportObj.ReportStatus = reportStatus;

            //update displayed report
            UpdateReport(reportObj);
        }

        /// <summary>
        /// Update a displayed report. 
        /// Add report if it is a new report.
        /// </summary>
        /// <param name="reportObj">
        /// The updated report.
        /// </param>
        public void UpdateReport(Report reportObj)
        {
            //check report should be displayed
            //status filter
            if (mcbStatus.SelectedIndex > 0 &&
                (int)mcbStatus.SelectedValue != reportObj.ReportStatus)
            {
                //report should not be displayed
                //remove report if it is being displayed
                RemoveReport(reportObj.ReportId);

                //exit
                return;
            }

            //report type filter
            if (mcbReportType.SelectedIndex > 0 &&
                (int)mcbReportType.SelectedValue != reportObj.ReportRapporteur)
            {
                //report should not be displayed
                //remove report if it is being displayed
                RemoveReport(reportObj.ReportId);

                //exit
                return;
            }

            //report periodicity filter
            if (mcbPeriodicity.SelectedIndex > 0 &&
                (int)mcbPeriodicity.SelectedValue != reportObj.ReportPeriodicity)
            {
                //report should not be displayed
                //remove report if it is being displayed
                RemoveReport(reportObj.ReportId);

                //exit
                return;
            }

            //semester filter
            if (mcbSemester.SelectedIndex > 0 &&
                (int)mcbSemester.SelectedValue != reportObj.SemesterId)
            {
                //report should not be displayed
                //remove report if it is being displayed
                RemoveReport(reportObj.ReportId);

                //exit
                return;
            }

            //institution filter
            if (mcbInstitution.SelectedIndex > 0 &&
                (int)mcbInstitution.SelectedValue != reportObj.InstitutionId)
            {
                //report should not be displayed
                //remove report if it is being displayed
                RemoveReport(reportObj.ReportId);

                //exit
                return;
            }

            //teacher filter
            if (mcbTeacher.SelectedIndex > 0 &&
                (int)mcbTeacher.SelectedValue != reportObj.TeacherId)
            {
                //report should not be displayed
                //remove report if it is being displayed
                RemoveReport(reportObj.ReportId);

                //exit
                return;
            }

            //class filter
            if (mcbClass.SelectedIndex > 0 &&
                (int)mcbClass.SelectedValue != reportObj.ClassId)
            {
                //report should not be displayed
                //remove report if it is being displayed
                RemoveReport(reportObj.ReportId);

                //exit
                return;
            }

            //lock list of reports
            lock (reports)
            {
                //set report
                reports[reportObj.ReportId] = reportObj;
            }

            //lock data table of reports
            lock (dtReports)
            {
                //get displayed data row
                DataRow dr = dtReports.Rows.Find(reportObj.ReportId);

                //check if there was no data row yet
                if (dr == null)
                {
                    //create, set and add data row
                    dr = dtReports.NewRow();
                    SetReportDataRow(dr, reportObj);
                    dtReports.Rows.Add(dr);
                }
                else
                {
                    //set data
                    SetReportDataRow(dr, reportObj);
                }
            }

            //refresh report interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed reports with institution data.
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

            //gather list of updated reports
            List<Report> updatedReports = new List<Report>();

            //lock list
            lock (reports)
            {
                //check all displayed reports
                foreach (Report reportObj in reports.Values)
                {
                    //check report institution
                    if (reportObj.InstitutionId == institution.InstitutionId &&
                        !reportObj.InstitutionName.Equals(institution.ProjectName))
                    {
                        //update report
                        reportObj.InstitutionName = institution.ProjectName;

                        //add report to the list of updated reports
                        updatedReports.Add(reportObj);
                    }
                }
            }

            //check result
            if (updatedReports.Count == 0)
            {
                //no report was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of reports
            lock (dtReports)
            {
                //check each updated role
                foreach (Report reportObj in updatedReports)
                {
                    //get displayed data row
                    DataRow dr = dtReports.Rows.Find(reportObj.ReportId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetReportDataRow(dr, reportObj);
                    }
                }
            }

            //refresh institution interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed reports with teacher data.
        /// </summary>
        /// <param name="teacher">
        /// The updated teacher.
        /// </param>
        public void UpdateTeacher(Teacher teacher)
        {
            //check each teacher item
            foreach (IdDescriptionStatus item in mcbTeacher.Items)
            {
                //compare data
                if (item.Id == teacher.TeacherId &&
                    !item.Description.Equals(teacher.Name))
                {
                    //update item
                    item.Description = teacher.Name;

                    //check if item is selected
                    if (mcbTeacher.SelectedItem == item)
                    {
                        //set loading flag
                        isLoading = true;

                        //clear selection and reselect item
                        mcbTeacher.SelectedIndex = -1;
                        mcbTeacher.SelectedItem = item;

                        //reset loading flag
                        isLoading = false;
                    }

                    //exit loop
                    break;
                }
            }

            //gather list of updated reports
            List<Report> updatedReports = new List<Report>();

            //lock list
            lock (reports)
            {
                //check all displayed reports
                foreach (Report reportObj in reports.Values)
                {
                    //check report teacher
                    if (reportObj.TeacherId == teacher.TeacherId &&
                        !reportObj.TeacherName.Equals(teacher.Name))
                    {
                        //update report
                        reportObj.TeacherName = teacher.Name;

                        //add report to the list of updated reports
                        updatedReports.Add(reportObj);
                    }
                }
            }

            //check result
            if (updatedReports.Count == 0)
            {
                //no report was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of reports
            lock (dtReports)
            {
                //check each updated role
                foreach (Report reportObj in updatedReports)
                {
                    //get displayed data row
                    DataRow dr = dtReports.Rows.Find(reportObj.ReportId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetReportDataRow(dr, reportObj);
                    }
                }
            }

            //refresh teacher interface
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
                dgvReports.ColumnHeadersDefaultCellStyle.Font =
                    MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
                dgvReports.DefaultCellStyle.Font =
                    MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            }
            else if (e.PropertyName.Equals("ReportGridDisplayedColumns"))
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
        private void ViewReportControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //avoid auto generated columns
            dgvReports.AutoGenerateColumns = false;

            //set fonts
            dgvReports.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvReports.DefaultCellStyle.Font =
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

            //clear number of reports
            mlblItemCount.Text = string.Empty;
            mlblCompletedCount.Text = string.Empty;
            mlblPendingCount.Text = string.Empty;

            //check if a specific semester should be selected
            if (filterSemesterId > 0)
            {
                //select semester
                mcbSemester.SelectedValue = filterSemesterId;
            }

            //check if a specific class should be selected
            if (filterClassId > 0)
            {
                //select class
                mcbClass.SelectedValue = filterClassId;
            }

            //reset loading flag
            isLoading = false;

            //load data for the first time by simulating status selection
            //select all status filter
            mcbStatus_SelectedIndexChanged(this, new EventArgs());
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

            //load reports
            LoadReports();
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
                mcbTeacher.SelectedIndex >= 0 ? (int)mcbTeacher.SelectedValue : -1);

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

            //load reports
            LoadReports();
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

            //set flag is loading teachers
            isLoadingTeachers = true;

            //get id of current selected teacher
            int selectedTeacherId = mcbTeacher.SelectedIndex >= 0 ?
                (int)mcbTeacher.SelectedValue : int.MinValue;

            //reload teacher list
            ListTeachers(mcbInstitution.SelectedIndex >= 0 ? 
                (int)mcbInstitution.SelectedValue : -1);

            //try to reselet previous selected teacher
            mcbTeacher.SelectedValue = selectedTeacherId;

            //check result
            if (mcbTeacher.SelectedIndex < 0)
            {
                //select all option
                mcbTeacher.SelectedIndex = 0;
            }

            //reset flag is loading teachers
            isLoadingTeachers = false;

            //set flag is loading classes
            isLoadingClasses = true;

            //get id of current selected class
            int selectedClassId = mcbClass.SelectedIndex >= 0 ?
                (int)mcbClass.SelectedValue : int.MinValue;

            //reload class list
            ListClasses(
                mcbSemester.SelectedIndex >= 0 ? (int)mcbSemester.SelectedValue : -1,
                mcbInstitution.SelectedIndex >= 0 ? (int)mcbInstitution.SelectedValue : -1,
                mcbTeacher.SelectedIndex >= 0 ? (int)mcbTeacher.SelectedValue : -1);

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

            //load reports
            LoadReports();
        }

        /// <summary>
        /// Report type combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading)
            {
                //no need to handle this event
                //exit
                return;
            }

            //load reports
            LoadReports();
        }

        /// <summary>
        /// Report periodicity combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbReportPeriodicity_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading)
            {
                //no need to handle this event
                //exit
                return;
            }

            //load reports
            LoadReports();
        }

        /// <summary>
        /// Teacher combo box selected index changed event handler. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading || isLoadingTeachers)
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
                mcbTeacher.SelectedIndex >= 0 ? (int)mcbTeacher.SelectedValue : -1);

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

            //load reports
            LoadReports();
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

            //load reports
            LoadReports();
        }

        /// <summary>
        /// Edit tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlEdit_Click(object sender, EventArgs e)
        {
            //check if there is any selected report
            if (dgvReports.SelectedCells.Count == 0)
            {
                //no report is selected
                //ask user to select report
                MetroMessageBox.Show(Manager.MainForm,
                    Properties.Resources.msgSelectReport,
                    Properties.Resources.titleViewReport,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                //exit
                return;
            }

            //get selected report
            Report report = FindReport(
                 (int)dgvReports.Rows[dgvReports.SelectedCells[0].RowIndex].Cells[columnIndexReportId].Value);

            //check result
            if (report == null)
            {
                //should never happen
                //exit
                return;
            }

            //must create edit report control according to selected report
            ISongControl editControl = null;

            //check report rapporteur
            if (report.ReportRapporteur == (int)ReportRapporteur.Teacher)
            {
                //check report periodicity
                if (report.ReportPeriodicity == (int)ReportPeriodicity.Month)
                {
                    //create teacher month control
                    editControl = new EditReportTeacherMonth(report);

                    //set parent control to current control
                    ((EditReportTeacherMonth)editControl).ParentControl = this;
                }
                else if (report.ReportPeriodicity == (int)ReportPeriodicity.Semester)
                {
                    //create teacher semester control
                    editControl = new EditReportTeacherSemester(report);

                    //set parent control to current control
                    ((EditReportTeacherSemester)editControl).ParentControl = this;
                }
            }
            else if (report.ReportRapporteur == (int)ReportRapporteur.Coordinator)
            {
                //check report periodicity
                if (report.ReportPeriodicity == (int)ReportPeriodicity.Month)
                {
                    //create coordinator month control
                    editControl = new EditReportCoordinatorMonth(report);

                    //set parent control to current control
                    ((EditReportCoordinatorMonth)editControl).ParentControl = this;
                }
                else if (report.ReportPeriodicity == (int)ReportPeriodicity.Semester)
                {
                    //create coordinator semester control
                    editControl = new EditReportCoordinatorSemester(report);

                    //set parent control to current control
                    ((EditReportCoordinatorSemester)editControl).ParentControl = this;
                }
            }

            //check result
            if (editControl == null)
            {
                //unhandled report
                //should never happen
                //exit
                return;
            }

            //set child control
            childControl = editControl;

            //display control
            Manager.MainForm.AddAndDisplayControl((UserControl)editControl);
        }

        /// <summary>
        /// Reports datagridview mouse up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReports_MouseUp(object sender, MouseEventArgs e)
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
            DataGridView.HitTestInfo hitInfo = dgvReports.HitTest(e.X, e.Y);

            //check selected row index
            if (hitInfo.RowIndex < 0)
            {
                //no row selected
                //exit
                return;
            }

            //check if there are selected rows and if report clicked on them
            if (dgvReports.SelectedRows.Count > 0 &&
                dgvReports.Rows[hitInfo.RowIndex].Selected != true)
            {
                //report did not click in the selected rows
                //clear selection
                dgvReports.ClearSelection();

                //select clicked row
                dgvReports.Rows[hitInfo.RowIndex].Selected = true;
            }
            //check if there are selected cells
            else if (dgvReports.SelectedCells.Count > 0)
            {
                //get list of selected cell rows
                HashSet<int> selectedRowIndexes = new HashSet<int>();

                //check each selected cell
                foreach (DataGridViewCell cell in dgvReports.SelectedCells)
                {
                    //add cell row index to the list
                    selectedRowIndexes.Add(cell.RowIndex);
                }

                //check if report clicked on a row of a selected cell
                if (selectedRowIndexes.Contains(hitInfo.RowIndex))
                {
                    //report clicked on a row of a selected cell
                    //select all rows
                    foreach (int selectedRowIndex in selectedRowIndexes)
                    {
                        //select row
                        dgvReports.Rows[selectedRowIndex].Selected = true;
                    }
                }
                else
                {
                    //report did not click on a row of a selected cell
                    //clear selected cells
                    dgvReports.ClearSelection();

                    //select clicked row
                    dgvReports.Rows[hitInfo.RowIndex].Selected = true;
                }
            }
            else
            {
                //select clicked row
                dgvReports.Rows[hitInfo.RowIndex].Selected = true;
            }

            //get selected or clicked report
            clickedReport = null;

            //check if there is a selected report
            if (dgvReports.SelectedRows.Count > 0)
            {
                //there is one or more reports selected
                //get first selected report
                for (int index = 0; index < dgvReports.SelectedRows.Count; index++)
                {
                    //get report using its report id
                    int reportId = (int)dgvReports.SelectedRows[index].Cells[columnIndexReportId].Value;
                    Report report = FindReport(reportId);

                    //check result
                    if (report != null)
                    {
                        //add report to list
                        clickedReport = report;

                        //exit loop
                        break;
                    }
                }

                //check result
                if (clickedReport == null)
                {
                    //no report was found
                    //should never happen
                    //do not display context menu
                    //exit
                    return;
                }
            }
            else
            {
                //there is no report selected
                //should never happen
                //do not display context menu
                //exit
                return;
            }

            //set context menu items visibility
            if (dgvReports.SelectedRows.Count == 1)
            {
                //display view menu items according to permissions
                mnuViewReport.Visible = true;
                mnuViewInstitution.Visible = Manager.HasLogonPermission("Institution.View");
                mnuViewTeacher.Visible = Manager.HasLogonPermission("Teacher.View") && clickedReport.TeacherId > 0;
                tssSeparator.Visible = true;

                //display impersonate options
                mnuImpersonateCoordinator.Visible = Manager.ImpersonatingUser == null &&
                    Manager.HasLogonPermission("User.Impersonate") && clickedReport.CoordinatorUserId > 0;
                mnuImpersonateTeacher.Visible = Manager.ImpersonatingUser == null &&
                    Manager.HasLogonPermission("User.Impersonate") && clickedReport.TeacherUserId > 0;
                tssSeparatorImpersonate.Visible = Manager.ImpersonatingUser == null &&
                    Manager.HasLogonPermission("User.Impersonate") && 
                    (clickedReport.TeacherUserId > 0 || clickedReport.CoordinatorUserId > 0);
            }
            else
            {
                //hide view menu items
                mnuViewReport.Visible = false;
                mnuViewInstitution.Visible = false;
                mnuViewTeacher.Visible = false;
                tssSeparator.Visible = false;

                //hide impersonate options
                mnuImpersonateCoordinator.Visible = false;
                mnuImpersonateTeacher.Visible = false;
                tssSeparatorImpersonate.Visible = false;
            }

            //show report context menu on the clicked point
            mcmReport.Show(this.dgvReports, p);
        }

        /// <summary>
        /// View report menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewReport_Click(object sender, EventArgs e)
        {
            //check clicked report
            if (clickedReport == null)
            {
                //should never happen
                //exit
                return;
            }

            //simulate an edit tile click
            mtlEdit_Click(this, new EventArgs());
        }

        /// <summary>
        /// View institution menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewInstitution_Click(object sender, EventArgs e)
        {
            //check clicked report
            if (clickedReport == null)
            {
                //should never happen
                //exit
                return;
            }

            //check report institution
            if (clickedReport.InstitutionId <= 0)
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
            registerControl.FirstSelectedId = clickedReport.InstitutionId;

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// View teacher menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewTeacher_Click(object sender, EventArgs e)
        {
            //check clicked report
            if (clickedReport == null)
            {
                //should never happen
                //exit
                return;
            }

            //check report teacher
            if (clickedReport.TeacherId <= 0)
            {
                //no teacher
                //should never happen
                //exit
                return;
            }

            //create control to display selected teacher
            RegisterTeacherControl registerControl =
                new UI.Controls.RegisterTeacherControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedReport.TeacherId;

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
            //check clicked report
            if (clickedReport == null)
            {
                //should never happen
                //exit
                return;
            }

            //check report coordinator user
            if (clickedReport.CoordinatorUserId <= 0)
            {
                //no coordinator user
                //should never happen
                //exit
                return;
            }

            //let user impersonate coordinator user
            //display impersonation confirmation
            Manager.MainForm.ConfirmAndImpersonateUser(
                clickedReport.CoordinatorUserId, clickedReport.CoordinatorName);
        }

        /// <summary>
        /// Impersonate teacher menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuImpersonateTeacher_Click(object sender, EventArgs e)
        {
            //check clicked report
            if (clickedReport == null)
            {
                //should never happen
                //exit
                return;
            }

            //check report teacher user
            if (clickedReport.TeacherUserId <= 0)
            {
                //no teacher user
                //should never happen
                //exit
                return;
            }

            //let user impersonate teacher user
            //display impersonation confirmation
            Manager.MainForm.ConfirmAndImpersonateUser(
                clickedReport.TeacherUserId, clickedReport.TeacherName);
        }

        /// <summary>
        /// Copy menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCopy_Click(object sender, EventArgs e)
        {
            //check if any cell is selected
            if (this.dgvReports.GetCellCount(DataGridViewElementStates.Selected) <= 0)
            {
                //should never happen
                //exit
                return;
            }

            try
            {
                //include header text 
                dgvReports.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                //copy selection to the clipboard
                Clipboard.SetDataObject(this.dgvReports.GetClipboardContent());
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

    }

} //end of namespace PnT.SongClient.UI.Controls
