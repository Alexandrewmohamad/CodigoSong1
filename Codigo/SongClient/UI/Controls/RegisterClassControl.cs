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
    /// This control is used to manage class registries in web service.
    /// Inherits from base register control.
    /// </summary>
    public partial class RegisterClassControl : RegisterBaseControl
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
        /// The subject code of the displayed class.
        /// </summary>
        private int subjectCode = int.MinValue;

        /// <summary>
        /// The current progress of the displayed class.
        /// </summary>
        private ClassProgress classProgress = ClassProgress.Unknown;

        /// <summary>
        /// The list of all semesters.
        /// </summary>
        private List<Semester> semesters = null;

        /// <summary>
        /// The semester to be used when creating a new class.
        /// The semester must have not started classes yet.
        /// </summary>
        private Semester nextSemester = null;

        /// <summary>
        /// The list of all loaded teacher lists.
        /// Keep lists for better performance.
        /// </summary>
        private Dictionary<int, List<IdDescriptionStatus>> teacherLists = null;

        /// <summary>
        /// The list of registrations for the selected class.
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
        /// ID for the next create attendance.
        /// </summary>
        private int nextAttendanceId = -1;

        /// <summary>
        /// DataTable for attendances.
        /// </summary>
        private DataTable dtAttendances = null;

        /// <summary>
        /// The attendance ID column index in the datagridview.
        /// </summary>
        private int columnIndexAttendanceId;

        /// <summary>
        /// The roll call column index in the datagridview.
        /// </summary>
        private int columnIndexRollCall;

        /// <summary>
        /// True if attendance data are being loaded into UI.
        /// </summary>
        private bool isLoadingAttendance = false;

        /// <summary>
        /// The current displayed class.
        /// </summary>
        private Class displayedClass = null;

        /// <summary>
        /// The current displayed class student list.
        /// </summary>
        private List<IdDescriptionStatus> displayedClassStudents = null;

        /// <summary>
        /// The current displayed class attendace list.
        /// </summary>
        private List<Attendance> displayedClassAttendances = null;

        /// <summary>
        /// The current displayed class attendace list for selected day.
        /// </summary>
        private List<Attendance> displayedClassDayAttendances = null;

        /// <summary>
        /// The list of class days for the selected month.
        /// </summary>
        private List<DateTime> classDays = null;

        /// <summary>
        /// The index of the current displayed class day.
        /// </summary>
        private int displayedClassDayIndex = -1;

        /// <summary>
        /// The previous selected month.
        /// Keep track of selected month while reloading data.
        /// </summary>
        private DateTime previousSelectedMonth = DateTime.MinValue;

        /// <summary>
        /// The index of the previous displayed class day.
        /// Keep track of selected class day while reloading data.
        /// </summary>
        private int previousDisplayedClassDayIndex = int.MinValue;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RegisterClassControl() : base("Class", Manager.Settings.HideInactiveClasses)
        {
            //init UI components
            InitializeComponent();

            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set units
            nudCapacity.Unit = Properties.Resources.item_plural_Student.ToLower();
            nudDuration.Unit = Properties.Resources.unitMinutes.ToLower();

            //hide copy button
            this.displayCopyButton = false;

            //class cannot be deleted
            this.classHasDeletion = false;

            //set permissions
            this.allowAddItem = Manager.HasLogonPermission("Class.Insert");
            this.allowEditItem = Manager.HasLogonPermission("Class.Edit");
            this.allowDeleteItem = Manager.HasLogonPermission("Class.Inactivate");

            //check if a teacher is viewing its classes
            if (Manager.LogonTeacher != null &&
                !Manager.HasLogonPermission("Class.View"))
            {
                //allow teacher to edit its classes
                this.allowEditItem = true;
            }

            //create list of registrations
            registrations = new Dictionary<int, Registration>();

            //create registration data table
            CreateRegistrationDataTable();

            //get registration ID column index
            columnIndexRegistrationId = dgvRegistrations.Columns[RegistrationId.Name].Index;

            //avoid auto generated columns
            dgvRegistrations.AutoGenerateColumns = false;

            //create lists of attendances
            classDays = new List<DateTime>();

            //create attendance data table
            CreateAttendanceDataTable();

            //get attendance ID and attendance student ID column indexes
            columnIndexAttendanceId = dgvAttendances.Columns[AttendanceId.Name].Index;
            columnIndexRollCall = dgvAttendances.Columns[RollCallValue.Name].Index;

            //avoid auto generated columns
            dgvAttendances.AutoGenerateColumns = false;

            //create list of loaded teachers
            teacherLists = new Dictionary<int, List<IdDescriptionStatus>>();

            //load combos
            //list statuses
            List<KeyValuePair<int, string>> statuses = new List<KeyValuePair<int, string>>();
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Active, Properties.Resources.ResourceManager.GetString("ItemStatus_Active")));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Inactive, Properties.Resources.ResourceManager.GetString("ItemStatus_Inactive")));
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

            //list semesters
            ListSemesters();

            //list class types
            ListClassTypes();

            //list instrument types
            ListInstrumentTypes();

            //list class levels
            ListClassLevels();

            //set empty list of teachers before listing poles
            mcbTeacher.DataSource = new List<IdDescriptionStatus>();

            //list poles
            ListPoles();

            //list roll call values
            List<KeyValuePair<int, string>> rollCalls = new List<KeyValuePair<int, string>>();
            rollCalls.Add(new KeyValuePair<int, string>(
                (int)RollCall.Empty, " "));
            rollCalls.Add(new KeyValuePair<int, string>(
                (int)RollCall.Absent, Properties.Resources.ResourceManager.GetString("RollCall_Absent")));
            rollCalls.Add(new KeyValuePair<int, string>(
                (int)RollCall.Present, Properties.Resources.ResourceManager.GetString("RollCall_Present")));
            rollCalls.Add(new KeyValuePair<int, string>(
                (int)RollCall.Sick, Properties.Resources.ResourceManager.GetString("RollCall_Sick")));
            rollCalls.Add(new KeyValuePair<int, string>(
                (int)RollCall.Justified, Properties.Resources.ResourceManager.GetString("RollCall_Justified")));
            rollCalls.Add(new KeyValuePair<int, string>(
                (int)RollCall.Evaded, Properties.Resources.ResourceManager.GetString("RollCall_Evaded")));
            rollCalls.Add(new KeyValuePair<int, string>(
                (int)RollCall.NoClass, Properties.Resources.ResourceManager.GetString("RollCall_NoClass")));
            rollCalls.Add(new KeyValuePair<int, string>(
                (int)RollCall.TeacherAbsent, Properties.Resources.ResourceManager.GetString("RollCall_TeacherAbsent")));
            rollCalls.Add(new KeyValuePair<int, string>(
                (int)RollCall.NotRegistered, Properties.Resources.ResourceManager.GetString("RollCall_NotRegistered")));
            RollCallValue.ValueMember = "Key";
            RollCallValue.DisplayMember = "Value";
            RollCallValue.DataSource = rollCalls;
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


        #region Private Methods *******************************************************

        /// <summary>
        /// Clear attendance data table.
        /// </summary>
        private void ClearAttendanceDataTable()
        {
            //lock datatable of attendances
            lock (dtAttendances)
            {
                //clear datatable
                dtAttendances.Clear();
            }
        }

        /// <summary>
        /// Clear displayed attendances and refresh UI.
        /// </summary>
        private void ClearDisplayedAttendances()
        {
            //clear class days
            classDays = new List<DateTime>();

            //reset displayed class day index
            displayedClassDayIndex = -1;

            //reset displayed class students
            displayedClassStudents = null;

            //reset displayed class attendances
            displayedClassAttendances = null;

            //reset displayed class day attendances
            displayedClassDayAttendances = null;

            //clear attendance datatable
            ClearAttendanceDataTable();

            //enable attendace ui
            EnableAttendanceFields();
        }

        /// <summary>
        /// Clear registration fields.
        /// </summary>
        private void ClearRegistrationFields()
        {
            //clear student 
            mtxtRegistrationStudent.Text = string.Empty;

            //clear status selection
            mcbRegistrationStatus.SelectedIndex = -1;

            //clear auto renewal option
            mcbRegistrationAutoRenewal.Checked = false;
        }

        /// <summary>
        /// Create attendance data table.
        /// </summary>
        private void CreateAttendanceDataTable()
        {
            //create data table
            dtAttendances = new DataTable();

            //AttendanceId
            DataColumn dcAttendanceId = new DataColumn("AttendanceId", typeof(int));
            dtAttendances.Columns.Add(dcAttendanceId);

            //StudentId
            DataColumn dcStudentId = new DataColumn("StudentId", typeof(int));
            dtAttendances.Columns.Add(dcStudentId);

            //StudentName
            DataColumn dcStudentName = new DataColumn("StudentName", typeof(string));
            dtAttendances.Columns.Add(dcStudentName);

            //StudentStatusValue
            DataColumn dcStudentStatusValue = new DataColumn("StudentStatusValue", typeof(int));
            dtAttendances.Columns.Add(dcStudentStatusValue);

            //RollCallValue
            DataColumn dcRollCallValue = new DataColumn("RollCallValue", typeof(int));
            dtAttendances.Columns.Add(dcRollCallValue);

            //RollCallText
            DataColumn dcRollCallText = new DataColumn("RollCallText", typeof(string));
            dtAttendances.Columns.Add(dcRollCallText);

            //set primary key column
            dtAttendances.PrimaryKey = new DataColumn[] { dcAttendanceId };
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

            //Student
            DataColumn dcStudent = new DataColumn("StudentName", typeof(string));
            dtRegistrations.Columns.Add(dcStudent);

            //Position
            DataColumn dcPosition = new DataColumn("Position", typeof(int));
            dtRegistrations.Columns.Add(dcPosition);

            //AutoRenewal
            DataColumn dcAutoRenewal = new DataColumn("AutoRenewal", typeof(string));
            dtRegistrations.Columns.Add(dcAutoRenewal);

            //RegistrationStatusValue
            DataColumn dcRegistrationStatusValue = new DataColumn("RegistrationStatusValue", typeof(int));
            dtRegistrations.Columns.Add(dcRegistrationStatusValue);

            //RegistrationStatusName
            DataColumn dcRegistrationStatusName = new DataColumn("RegistrationStatusName", typeof(string));
            dtRegistrations.Columns.Add(dcRegistrationStatusName);

            //set primary key column
            dtRegistrations.PrimaryKey = new DataColumn[] { dcRegistrationId };
        }

        /// <summary>
        /// Display attendances for current selected class day.
        /// </summary>
        private void DisplayClassDayAttendances()
        {
            //find attendances for current displayed day
            displayedClassDayAttendances = displayedClassAttendances.FindAll(
                p => p.Date.Equals(classDays[displayedClassDayIndex]));

            //check result
            if (displayedClassDayAttendances == null)
            {
                //no attendance for displayed day
                //create empty list
                displayedClassDayAttendances = new List<Attendance>();
            }

            //clear attendance datatable
            ClearAttendanceDataTable();

            //check each student
            foreach (IdDescriptionStatus student in displayedClassStudents)
            {
                //get attendance for student for the selected day
                Attendance attendance = displayedClassDayAttendances.Find(
                    a => a.StudentId == student.Id);

                //check result
                if (attendance == null)
                {
                    //no attendance was set to student
                    //create and set attendance
                    attendance = new Attendance();
                    attendance.AttendanceId = nextAttendanceId--;
                    attendance.ClassId = displayedClass.ClassId;
                    attendance.ClassDay = -1;
                    attendance.Date = classDays[displayedClassDayIndex];
                    attendance.RollCall = (int)RollCall.Empty;
                    attendance.StudentId = student.Id;
                    attendance.StudentName = student.Description;
                    attendance.TeacherId = displayedClass.TeacherId;

                    //add attendance to lists
                    displayedClassAttendances.Add(attendance);
                    displayedClassDayAttendances.Add(attendance);
                }
                else
                {
                    //set student name since it is not loaded from database
                    attendance.StudentName = student.Description;
                }

                //check if displayed day is not in the future
                //and if attendance roll call is empty
                //check if student is evaded from class
                if (classDays[displayedClassDayIndex] <= DateTime.Today &&
                    attendance.RollCall == (int)RollCall.Empty &&
                    student.Status == (int)ItemStatus.Evaded)
                {
                    //set roll call to evaded
                    attendance.RollCall = (int)RollCall.Evaded;
                    attendance.Updated = true;
                }

                //create, set and add loan row
                DataRow dr = dtAttendances.NewRow();
                SetAttendanceDataRow(dr, attendance);
                dtAttendances.Rows.Add(dr);
            }

            //check if datagrid has not a source yet
            if (dgvAttendances.DataSource == null)
            {
                //set source to datagrid
                dgvAttendances.DataSource = dtAttendances;
            }

            //refresh grid
            dgvAttendances.Refresh();

            //clear default selection
            dgvAttendances.ClearSelection();

            //display selected day
            SetDate(classDays[displayedClassDayIndex], mlblDay);
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
        /// Enable attendance fields according to current context.
        /// </summary>
        private void EnableAttendanceFields()
        {
            //enable roll calls tile
            mtlRollCalls.Enabled = mcbAttendanceMonth.SelectedIndex > -1 &&
                dtAttendances.Rows.Count > 0;

            //enable month combo if not creating
            mcbAttendanceMonth.Enabled = this.Status != RegisterStatus.Creating;

            //display day fields
            mlblDay.Visible = (classDays.Count > 0 && displayedClassDayIndex >= 0);
            mtlPreviousDay.Visible = (classDays.Count > 1 && displayedClassDayIndex > 0);
            mtlNextDay.Visible = (classDays.Count > 1 && displayedClassDayIndex < classDays.Count - 1);

            //check if class is being edited 
            //and if displayed class day is not later than today
            if (this.Status == RegisterStatus.Editing && (
                classDays.Count < 0 || displayedClassDayIndex < 0 ||
                classDays[displayedClassDayIndex] <= DateTime.Today))
            {
                //display roll call value column
                RollCallValue.Visible = true;
                RollCallText.Visible = false;
            }
            else
            {
                //display roll call value column
                RollCallValue.Visible = false;
                RollCallText.Visible = true;
            }

            //hide attendance ID column
            AttendanceId.Visible = false;
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

            //student is read only
            mtxtRegistrationStudent.Enabled = false;

            //set status combo box
            mcbRegistrationStatus.Enabled = (Status != RegisterStatus.Consulting) &&
                (classProgress == ClassProgress.InProgress) && registration != null;

            //set auto renewal
            mcbRegistrationAutoRenewal.Enabled = (Status != RegisterStatus.Consulting) && registration != null;

            //calculate permission to delete registration after start of semester
            bool deleteRegistration = (classProgress == ClassProgress.InProgress) &&
                Manager.HasLogonPermission("Registration.Delete");

            //set registration buttons
            mbtnDecreaseRegistration.Enabled = (Status != RegisterStatus.Consulting) &&
                (classProgress == ClassProgress.Registration || classProgress == ClassProgress.InProgress) &&
                (dgvRegistrations.SelectedRows.Count > 0) && (dgvRegistrations.SelectedRows[0].Index > 0);
            mbtnIncreaseRegistration.Enabled = (Status != RegisterStatus.Consulting) &&
                (classProgress == ClassProgress.Registration || classProgress == ClassProgress.InProgress) &&
                (dgvRegistrations.SelectedRows.Count > 0) && (dgvRegistrations.SelectedRows[0].Index < dgvRegistrations.RowCount - 1);
            mbtnDeleteRegistration.Enabled = (Status != RegisterStatus.Consulting) &&
                (classProgress == ClassProgress.Registration || deleteRegistration) &&
                (dgvRegistrations.SelectedRows.Count > 0);

            //hide registration ID column
            RegistrationId.Visible = false;
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
        /// List class levels into UI.
        /// </summary>
        private void ListClassLevels()
        {
            //create list of class levels
            List<KeyValuePair<int, string>> classObjLevels = new List<KeyValuePair<int, string>>();

            //check each class level
            foreach (ClassLevel classObjLevel in Enum.GetValues(typeof(ClassLevel)))
            {
                //add converted class level
                classObjLevels.Add(new KeyValuePair<int, string>(
                    (int)classObjLevel, Properties.Resources.ResourceManager.GetString(
                        "ClassLevel_" + classObjLevel.ToString())
                    ));
            }

            //set classObj levels to UI
            mcbClassLevel.ValueMember = "Key";
            mcbClassLevel.DisplayMember = "Value";
            mcbClassLevel.DataSource = classObjLevels;
        }

        /// <summary>
        /// List class types into UI.
        /// </summary>
        private void ListClassTypes()
        {
            //create list of class types
            List<KeyValuePair<int, string>> classObjTypes = new List<KeyValuePair<int, string>>();

            //check each class type
            foreach (ClassType classObjType in Enum.GetValues(typeof(ClassType)))
            {
                //add converted class type
                classObjTypes.Add(new KeyValuePair<int, string>(
                    (int)classObjType, Properties.Resources.ResourceManager.GetString(
                        "ClassType_" + classObjType.ToString())
                    ));
            }

            //set classObj types to UI
            mcbClassType.ValueMember = "Key";
            mcbClassType.DisplayMember = "Value";
            mcbClassType.DataSource = classObjTypes;
        }

        /// <summary>
        /// List instrument types into UI.
        /// </summary>
        private void ListInstrumentTypes()
        {
            //create list of instrument types
            List<KeyValuePair<int, string>> instrumentObjTypes = new List<KeyValuePair<int, string>>();

            //check each instrument type
            foreach (InstrumentsType instrumentObjType in Enum.GetValues(typeof(InstrumentsType)))
            {
                //add converted instrument type
                instrumentObjTypes.Add(new KeyValuePair<int, string>(
                    (int)instrumentObjType, Properties.Resources.ResourceManager.GetString(
                        "InstrumentsType_" + instrumentObjType.ToString())
                    ));
            }

            //add empty option
            instrumentObjTypes.Insert(0, new KeyValuePair<int, string>(int.MinValue, "-"));

            //set instrument types to UI
            mcbInstrumentType.ValueMember = "Key";
            mcbInstrumentType.DisplayMember = "Value";
            mcbInstrumentType.DataSource = instrumentObjTypes;
        }

        /// <summary>
        /// List semester months into UI.
        /// </summary>
        private void ListMonths(Semester semester)
        {
            try
            {
                //set loading flag
                isLoadingAttendance = true;

                //add months to list
                //create list
                List<KeyValuePair<DateTime, string>> months =
                    new List<KeyValuePair<DateTime, string>>();

                //get first month
                DateTime currentMonth = semester.StartMonth;

                //keep adding months
                while (currentMonth <= semester.EndMonth)
                {
                    //create month
                    KeyValuePair<DateTime, string> month = new KeyValuePair<DateTime, string>(
                        currentMonth, Properties.Resources.ResourceManager.GetString(
                            "Month_" + currentMonth.Month) + " " + currentMonth.Year);

                    //add month
                    months.Add(month);

                    //increment current month
                    currentMonth = new DateTime(
                        currentMonth.Month < 12 ? currentMonth.Year : currentMonth.Year + 1,
                        currentMonth.Month < 12 ? currentMonth.Month + 1 : 1,
                        1);
                }

                //set months to UI
                mcbAttendanceMonth.ValueMember = "Key";
                mcbAttendanceMonth.DisplayMember = "Value";
                mcbAttendanceMonth.DataSource = months;

                //check if there was a previous selected value
                if (previousSelectedMonth != DateTime.MinValue &&
                    months.FindIndex(m => m.Key.Equals(previousSelectedMonth)) > -1)
                {
                    //select month
                    mcbAttendanceMonth.SelectedValue = previousSelectedMonth;
                }
                else
                {
                    //reset previous selected month and displayed class day index
                    previousSelectedMonth = DateTime.MinValue;
                    previousDisplayedClassDayIndex = int.MinValue;

                    //get today month
                    DateTime todayMonth = DateTime.Today;
                    todayMonth = new DateTime(todayMonth.Year, todayMonth.Month, 1);

                    //check if month is listed
                    if (months.FindIndex(m => m.Key.Equals(todayMonth)) > -1)
                    {
                        //select today month
                        mcbAttendanceMonth.SelectedValue = todayMonth;
                    }
                    //check if today month is after all months
                    else if (months.Count > 0 && months[0].Key < todayMonth)
                    {
                        //select last month in the list
                        mcbAttendanceMonth.SelectedValue = months[months.Count - 1].Key;
                    }
                }
            }
            finally
            {
                //reset loading flag
                isLoadingAttendance = false;
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
        /// List semesters into UI.
        /// </summary>
        private void ListSemesters()
        {
            //set default empty list to UI
            mcbSemester.ValueMember = "SemesterId";
            mcbSemester.DisplayMember = "Description";
            mcbSemester.DataSource = new List<Semester>();

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
                semesters = songChannel.FindSemesters(false);

                //check result
                if (semesters[0].Result == (int)SelectResult.Success)
                {
                    //create list of semesters to be displayed
                    List<Semester> displayedSemesters = new List<Semester>();

                    //check each semester
                    foreach (Semester semester in semesters)
                    {
                        //check end and reference date
                        if (semester.EndDate < DateTime.Today &&
                            semester.ReferenceDate.AddMonths(6) < DateTime.Today)
                        {
                            //past semester
                            //go to next semester
                            continue;
                        }

                        //add semester to displayed semesters
                        displayedSemesters.Add(semester);

                        //check if next semester was not set yet
                        if (nextSemester == null)
                        {
                            //check if current semester has not started 
                            //or started no more than one month ago
                            if (semester.StartDate >= DateTime.Today.AddMonths(-1))
                            {
                                //set next semester
                                nextSemester = semester;
                            }
                        }
                    }

                    //display semesters
                    mcbSemester.ValueMember = "SemesterId";
                    mcbSemester.DisplayMember = "Description";
                    mcbSemester.DataSource = displayedSemesters;
                }
                else if (semesters[0].Result == (int)SelectResult.Empty)
                {
                    //no semester is available
                    //clear list
                    semesters.Clear();

                    //exit
                    return;
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

                    //could not get semesters
                    //clear list
                    semesters.Clear();

                    //exit
                    return;
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
        /// List teachers into UI for selected pole.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// -1 to select all poles.
        /// </param>
        private void ListTeachers(int poleId)
        {
            //set default empty list to UI
            mcbTeacher.ValueMember = "Id";
            mcbTeacher.DisplayMember = "Description";
            mcbTeacher.DataSource = new List<IdDescriptionStatus>();

            //check if there is a list of teachers is for selected pole
            if (teacherLists.ContainsKey(poleId))
            {
                //set stored teachers to UI
                //copy list because an assigned teacher might be added
                mcbTeacher.ValueMember = "Id";
                mcbTeacher.DisplayMember = "Description";
                mcbTeacher.DataSource = new List<IdDescriptionStatus>(teacherLists[poleId]);

                //exit
                return;
            }

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

                //check selected pole
                if (poleId <= 0)
                {
                    //get list of all active teachers
                    teachers = songChannel.ListTeachersByStatus((int)ItemStatus.Active);
                }
                else
                {
                    //get list of pole active teachers
                    teachers = songChannel.ListTeachersByPole(
                        poleId, (int)ItemStatus.Active);
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

                //set teachers to UI
                mcbTeacher.ValueMember = "Id";
                mcbTeacher.DisplayMember = "Description";
                mcbTeacher.DataSource = teachers;

                //store list for faster performance
                //copy list because an assigned teacher might be added
                teacherLists[poleId] = new List<IdDescriptionStatus>(teachers);
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
        /// Load attendances for selected month and display them.
        /// </summary>
        /// <param name="month">
        /// The selected month.
        /// </param>
        private void LoadAttendances(DateTime month)
        {
            //check displayed class
            if (displayedClass == null)
            {
                //should never happen
                //exit
                return;
            }

            //get list of class days for selected month
            classDays = displayedClass.GetClassDays(month);

            //check result
            if (classDays.Count == 0)
            {
                //no day for selected class
                //reset displayed day index
                displayedClassDayIndex = -1;

                //clear displayed attendances
                ClearDisplayedAttendances();

                //display message
                mlblDay.Text = Properties.Resources.msgNoClass;
                mlblDay.Visible = true;

                //exit
                return;
            }

            //display first class day if there was no previous displayed class day
            displayedClassDayIndex = previousDisplayedClassDayIndex > -1 ?
                previousDisplayedClassDayIndex : 0;

            //reset previous selected month and displayed day index
            previousSelectedMonth = DateTime.MinValue;
            previousDisplayedClassDayIndex = int.MinValue;

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //clear displayed attendances
                ClearDisplayedAttendances();

                //could not load attendances
                return;
            }

            try
            {
                //load list of students for selected class
                displayedClassStudents = songChannel.ListStudentsByClass(
                    displayedClass.ClassId, -1);

                //check result
                if (displayedClassStudents[0].Result == (int)SelectResult.Success)
                {
                    //remove waiting list students
                    while (displayedClassStudents.Count > displayedClass.Capacity)
                    {
                        //remove last student
                        displayedClassStudents.RemoveAt(displayedClassStudents.Count - 1);
                    }

                    //sort students by name
                    displayedClassStudents.Sort((a, b) => a.Description.CompareTo(b.Description));
                }
                else if (displayedClassStudents[0].Result == (int)SelectResult.Empty)
                {
                    //no student for selected class
                    //clear list
                    displayedClassStudents.Clear();
                }
                else if (displayedClassStudents[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting students
                    //clear displayed attendances
                    ClearDisplayedAttendances();

                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Student,
                        displayedClassStudents[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Student,
                        displayedClassStudents[0].ErrorMessage));

                    //could not get students
                    //exit
                    return;
                }

                //update student status with registration status
                //if student registration status is evaded
                //and before registrations are edited
                //get list of class registrations
                List<Registration> classRegistrations = ListRegistrations;

                //check each class student
                foreach (IdDescriptionStatus student in displayedClassStudents)
                {
                    //find registration for student
                    Registration registration = classRegistrations.Find(
                        r => r.StudentId == student.Id);

                    //check result
                    if (registration != null &&
                        registration.RegistrationStatus == (int)ItemStatus.Evaded)
                    {
                        //student is evaded from class
                        //update student status to evaded
                        student.Status = (int)ItemStatus.Evaded;
                    }            
                }

                //check result
                if (displayedClassStudents.Count == 0)
                {
                    //no student for selected class
                    //clear displayed attendances
                    ClearDisplayedAttendances();

                    ////display message
                    //MetroMessageBox.Show(Manager.MainForm, string.Format(
                    //    Properties.Resources.msgClassNoStudent, displayedClass.Code),
                    //    Properties.Resources.titleNoStudent,
                    //    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //exit
                    return;
                }

                //load attendances for selected class
                displayedClassAttendances = songChannel.FindAttendancesByFilter(
                    false, false, displayedClass.ClassId, -1);

                //check result
                if (displayedClassAttendances[0].Result == (int)SelectResult.Success)
                {
                    //sort attendances by date
                    displayedClassAttendances.Sort((a, b) => a.Date.CompareTo(b.Date));
                }
                else if (displayedClassAttendances[0].Result == (int)SelectResult.Empty)
                {
                    //no attendance for selected class
                    //clear list
                    displayedClassAttendances.Clear();
                }
                else if (displayedClassAttendances[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting attendances
                    //clear displayed attendances
                    ClearDisplayedAttendances();

                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Attendance,
                        displayedClassAttendances[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Pole,
                        displayedClassAttendances[0].ErrorMessage));

                    //could not get attendances
                    //exit
                    return;
                }

                //display attendances for current class day
                DisplayClassDayAttendances();
            }
            catch (Exception ex)
            {
                //database error while getting attendances
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Attendance), ex);

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

            //enable attendace ui
            EnableAttendanceFields();
        }

        /// <summary>
        /// Refresh displayed registration datagrid.
        /// </summary>
        /// <param name="selectedRow">
        /// The index of the row to be selected.
        /// -1 if no row should be selected.
        /// </param>
        /// <param name="displayLastRow">
        /// True if last row must be displayed.
        /// False if no specific row must be displayed.
        /// </param>
        private void RefreshRegistrations(int selectedRow, bool displayLastRow)
        {
            //check if datagrid has not a source yet
            if (dgvRegistrations.DataSource == null)
            {
                //set source to datagrid
                dgvRegistrations.DataSource = dtRegistrations;
            }

            //check if first row should be displayed
            //and if the is at least one row
            if (displayLastRow && dgvRegistrations.Rows.Count > 0)
            {
                //refresh grid by displaying last row
                dgvRegistrations.FirstDisplayedScrollingRowIndex = (dgvRegistrations.Rows.Count - 1);
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
        /// Set data row with selected Attendance data.
        /// </summary>
        /// <param name="dataRow">The data row to be set.</param>
        /// <param name="attendance">The selected attendance.</param>
        private void SetAttendanceDataRow(DataRow dataRow, Attendance attendance)
        {
            dataRow["AttendanceId"] = attendance.AttendanceId;
            dataRow["StudentId"] = attendance.StudentId;
            dataRow["StudentName"] = attendance.StudentName;
            dataRow["RollCallValue"] = attendance.RollCall;
            dataRow["RollCallText"] = attendance.RollCall == (int)RollCall.Empty ? " " :
                Properties.Resources.ResourceManager.GetString(
                    "RollCall_" + ((RollCall)attendance.RollCall).ToString());

            //get student for attendance
            IdDescriptionStatus student = displayedClassStudents.Find(
                s => s.Id == attendance.StudentId);

            //set student status
            dataRow["StudentStatusValue"] = student != null ? 
                student.Status : (int)ItemStatus.Active;
        }

        /// <summary>
        /// Set date to selected control.
        /// </summary>
        /// <param name="date">
        /// The date to be displayed.
        /// </param>
        /// <param name="control">
        /// The selected control.
        /// </param>
        private void SetDate(DateTime date, Control control)
        {
            //set date to tile
            StringBuilder sbDate = new StringBuilder();

            //add week day
            sbDate.Append(Properties.Resources.ResourceManager.GetString(
                "day" + date.DayOfWeek.ToString()));

            //add comma
            sbDate.Append(", ");

            //add date
            sbDate.Append(date.ToShortDateString());

            //remove year
            sbDate.Length -= 5;

            //set date to control
            control.Text = sbDate.ToString();
        }

        /// <summary>
        /// Set data row with selected Registration data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="registration">The selected registration.</param>
        private void SetRegistrationDataRow(DataRow dataRow, Registration registration)
        {
            dataRow["RegistrationId"] = registration.RegistrationId;
            dataRow["StudentName"] = registration.StudentName;
            dataRow["Position"] = registration.Position + 1;
            dataRow["AutoRenewal"] = registration.AutoRenewal ? "Auto" : string.Empty;
            dataRow["RegistrationStatusValue"] = registration.RegistrationStatus;
            dataRow["RegistrationStatusName"] = Properties.Resources.ResourceManager.GetString(
                "ItemStatus_" + ((ItemStatus)registration.RegistrationStatus).ToString());
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

            //validate start time
            if (!ValidateRequiredField(mtxtStartTime, mlblStartTime.Text, mtbTabManager, tbGeneralData) ||
                !ValidateShortTimeField(mtxtStartTime, mtbTabManager, tbGeneralData))
            {
                //invalid field
                return false;
            }

            //validate days of the week
            if (!mcbMonday.Checked && !mcbTuesday.Checked && !mcbWednesday.Checked &&
                !mcbThursday.Checked && !mcbFriday.Checked && !mcbSaturday.Checked && !mcbSunday.Checked)
            {
                //at least one day of the week must be selected
                //focus monday field
                mcbMonday.Focus();

                //display tab
                mtbTabManager.SelectedTab = tbGeneralData;

                //display message
                MetroMessageBox.Show(Manager.MainForm,
                    Properties.Resources.msgRequiredOneWeekDay,
                    Properties.Resources.titleRequiredField,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //focus monday field again
                mcbMonday.Focus();

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
            //clear progress field
            mtxtProgress.Text = string.Empty;

            //select first status
            mcbStatus.SelectedIndex = 0;

            //clear inactivation fields
            inactivationReason = string.Empty;
            inactivationTime = DateTime.MinValue;

            //check number of semesters
            if (mcbSemester.Items.Count > 0)
            {
                //set semester
                mcbSemester.SelectedIndex = 0;
            }

            //clear code field
            mtxtCode.Text = string.Empty;

            //select first class type
            mcbClassType.SelectedIndex = 0;
            
            //select default instrument
            mcbInstrumentType.SelectedValue = (int)InstrumentsType.Violino;

            //select first level
            mcbClassLevel.SelectedIndex = 0;

            //check number of poles
            if (mcbPole.Items.Count > 0)
            {
                //select first pole
                mcbPole.SelectedIndex = 0;
            }

            //check number of teachers
            if (mcbTeacher.Items.Count > 0)
            {
                //select first teacher
                mcbTeacher.SelectedIndex = 0;
            }

            //reset capacity
            nudCapacity.Value = 10;

            //reset start time
            mtxtStartTime.Text = string.Empty;

            //reset duration
            nudDuration.Value = 60;

            //clear week days
            mcbMonday.Checked = false;
            mcbTuesday.Checked = false;
            mcbWednesday.Checked = false;
            mcbThursday.Checked = false;
            mcbFriday.Checked = false;
            mcbSaturday.Checked = false;
            mcbSunday.Checked = false;

            //clear registrations
            //lock datatable of registrations
            lock (dtRegistrations)
            {
                //clear datatable
                dtRegistrations.Clear();
            }

            try
            {
                //clear attendance months
                //set loading flag
                isLoadingAttendance = true;

                //check if there is a previous selected month
                if (mcbAttendanceMonth.SelectedIndex >= 0)
                {
                    //set previous selected month
                    previousSelectedMonth = (DateTime)mcbAttendanceMonth.SelectedValue;
                }

                //clear months
                mcbAttendanceMonth.DataSource = null;
            }
            finally
            {
                //reset loading flag
                isLoadingAttendance = false;
            }

            //clear attendances
            ClearDisplayedAttendances();
        }

        /// <summary>
        /// Dispose used resources from user control.
        /// </summary>
        public override void DisposeControl()
        {
            //update option to hide inactive items
            Manager.Settings.HideInactiveClasses = this.hideInactiveItems;
        }

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        /// <returns></returns>
        public override string SelectMenuOption()
        {
            //select classObj option
            return "Class";
        }

        /// <summary>
        /// Enable all UI fields for edition.
        /// </summary>
        /// <param name="enable">True to enable fields. False to disable them.</param>
        public override void EnableFields(bool enable)
        {
            //create local class conditions
            bool enableWhileRegistering = enable && classProgress == ClassProgress.Registration;
            bool enableWhileInProgress = enable && classProgress == ClassProgress.InProgress;

            //progress is generated
            mtxtProgress.Enabled = false;

            //code is generated
            mtxtCode.Enabled = false;

            //set status list
            mcbStatus.Enabled = enableWhileRegistering || enableWhileInProgress;

            //set teacher list
            mcbTeacher.Enabled = enableWhileRegistering || enableWhileInProgress;

            //set class type list
            mcbClassType.Enabled = enableWhileRegistering || enableWhileInProgress;

            //set instrument type list if class type is instrument
            mcbInstrumentType.Enabled = 
                (enableWhileRegistering || enableWhileInProgress) && (mcbClassType.SelectedIndex == 0);

            //set class level list
            mcbClassLevel.Enabled = enableWhileRegistering || enableWhileInProgress;

            //set start time field
            mtxtStartTime.Enabled = enableWhileRegistering || enableWhileInProgress;

            //set duration field
            nudDuration.Enabled = enableWhileRegistering || enableWhileInProgress;

            //set capacity field
            nudCapacity.Enabled = enableWhileRegistering || enableWhileInProgress;

            //set semester
            mcbSemester.Enabled = enableWhileRegistering;

            //set pole list
            mcbPole.Enabled = enableWhileRegistering;

            //calculate permission to edit week days after start of semester
            //can only edit until one month after semester has started
            bool editWeekDays = enable && Manager.HasLogonPermission("Class.Edit.WeekDays")
                && DateTime.Today <= Manager.CurrentSemester.StartDate.AddMonths(1);

            //clear week days
            mcbMonday.Enabled = enableWhileRegistering || editWeekDays;
            mcbTuesday.Enabled = enableWhileRegistering || editWeekDays;
            mcbWednesday.Enabled = enableWhileRegistering || editWeekDays;
            mcbThursday.Enabled = enableWhileRegistering || editWeekDays;
            mcbFriday.Enabled = enableWhileRegistering || editWeekDays;
            mcbSaturday.Enabled = enableWhileRegistering || editWeekDays;
            mcbSunday.Enabled = enableWhileRegistering || editWeekDays;

            //set registration fields
            mbtnAddRegistration.Enabled = enableWhileRegistering || enableWhileInProgress;
            EnableRegistrationFields();

            //set attendance fields
            EnableAttendanceFields();
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
                //get selected class from web service
                //load teacher and semester data to use them with roll calls
                Class classObj = songChannel.FindClass(itemId, true, true);

                //check result
                if (classObj.Result == (int)SelectResult.Empty)
                {
                    //should never happen
                    //could not load data
                    return false;
                }
                else if (classObj.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting classObj
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, classObj.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, classObj.ErrorMessage));

                    //could not load data
                    return false;
                }

                //set selected classObj ID
                selectedId = classObj.ClassId;

                //set class progress
                classProgress = (ClassProgress)classObj.ClassProgress;
                mtxtProgress.Text = Properties.Resources.ResourceManager.GetString(
                    "ClassProgress_" + ((ClassProgress)classObj.ClassProgress).ToString());

                //select status
                mcbStatus.SelectedValue = classObj.ClassStatus;

                //set inactivation fields
                inactivationReason = classObj.InactivationReason;
                inactivationTime = classObj.InactivationTime;

                //set suject code
                subjectCode = classObj.SubjectCode;

                //set code field
                mtxtCode.Text = classObj.Code;

                //set semester
                mcbSemester.SelectedValue = classObj.SemesterId;

                //check selected index
                if (mcbSemester.SelectedIndex < 0)
                {
                    //semester is not available
                    //find semester from loaded semesters
                    Semester semester = semesters.Find(s => s.SemesterId == classObj.SemesterId);

                    //check result
                    if (semester != null)
                    {
                        //add semester to list of semesters
                        List<Semester> displayedSemesters = 
                            (List<Semester>)mcbSemester.DataSource;
                        displayedSemesters.Add(semester);

                        //order list of semesters
                        displayedSemesters.Sort((x, y) => x.ReferenceDate.CompareTo(y.ReferenceDate));

                        //display semesters
                        mcbSemester.DataSource = null;
                        mcbSemester.ValueMember = "SemesterId";
                        mcbSemester.DisplayMember = "Description";
                        mcbSemester.DataSource = displayedSemesters;

                        //set semester
                        mcbSemester.SelectedValue = classObj.SemesterId;
                    }
                }

                //set class type
                mcbClassType.SelectedValue = classObj.ClassType;

                //set instrument type
                mcbInstrumentType.SelectedValue = classObj.InstrumentType;

                //set class level
                mcbClassLevel.SelectedValue = classObj.ClassLevel;

                //set pole
                mcbPole.SelectedValue = classObj.PoleId;

                //check selected index
                if (mcbPole.SelectedIndex < 0)
                {
                    try
                    {
                        //pole is not available
                        //it might be inactive
                        //must load pole from web service
                        Pole pole = songChannel.FindPole(classObj.PoleId);

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
                            mcbPole.SelectedValue = classObj.PoleId;
                        }
                    }
                    catch
                    {
                        //do nothing
                    }
                }
                
                //set teacher
                mcbTeacher.SelectedValue = classObj.TeacherId;

                //check selected index
                if (mcbTeacher.SelectedIndex < 0)
                {
                    try
                    {
                        //teacher is not available
                        //it might be inactive
                        //must load teacher from web service
                        Teacher teacher = songChannel.FindTeacher(classObj.TeacherId);

                        //check result
                        if (teacher.Result == (int)SelectResult.Success)
                        {
                            //add teacher to list of teachers
                            List<IdDescriptionStatus> teachers =
                                (List<IdDescriptionStatus>)mcbTeacher.DataSource;
                            teachers.Add(teacher.GetDescription());

                            //update displayed list
                            mcbTeacher.DataSource = null;
                            mcbTeacher.ValueMember = "Id";
                            mcbTeacher.DisplayMember = "Description";
                            mcbTeacher.DataSource = teachers;

                            //set teacher
                            mcbTeacher.SelectedValue = classObj.TeacherId;
                        }
                    }
                    catch
                    {
                        //do nothing
                    }
                }

                //set capacity
                nudCapacity.Value = classObj.Capacity;

                //set start time
                mtxtStartTime.Text = classObj.StartTime.ToString("HH:mm");

                //set duration
                nudDuration.Value = classObj.Duration;
                
                //set week days
                mcbMonday.Checked = classObj.WeekMonday;
                mcbTuesday.Checked = classObj.WeekTuesday;
                mcbWednesday.Checked = classObj.WeekWednesday;
                mcbThursday.Checked = classObj.WeekThursday;
                mcbFriday.Checked = classObj.WeekFriday;
                mcbSaturday.Checked = classObj.WeekSaturday;
                mcbSunday.Checked = classObj.WeekSunday;

                #region load registrations

                //get registrations for selected instrument
                List<Registration> registrations = songChannel.FindRegistrationsByClass(
                    classObj.ClassId, -1);

                //check result
                if (registrations[0].Result == (int)SelectResult.Empty)
                {
                    //class has no registration
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

                //sort registrations by Position
                registrations.Sort((x, y) => x.Position.CompareTo(y.Position));

                //a registration might have been deleted
                //must update registration position before displaying registrations
                //the registration order is kept the same
                //check each registration
                for (int i = 0; i < registrations.Count; i++)
                {
                    //update position
                    registrations[i].Position = i;
                }
                
                //display registrations
                DisplayRegistrations(registrations);

                #endregion load registrations

                #region load attendance data

                //clear class days
                classDays = new List<DateTime>();

                //set displayed class
                displayedClass = classObj;
                
                //list class semester months
                ListMonths(displayedClass.Semester);

                //perform a data load for selected month if any
                mcbAttendance_SelectedIndexChanged(this, new EventArgs());

                #endregion load attendance data
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
        /// Load classObj list from database.
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
                //list of classes
                List<IdDescriptionStatus> classObjs = null;

                //check if a teacher is viewing its classes
                if (Manager.LogonTeacher != null &&
                    !Manager.HasLogonPermission("Class.View"))
                {
                    //get list of classes for assigned teacher
                    classObjs = songChannel.ListClassesByFilter(
                        -1, -1, -1, -1, -1, -1, -1, Manager.LogonTeacher.TeacherId);
                }
                //check if logged on user has an assigned institution
                else if (Manager.LogonUser != null && Manager.LogonUser.InstitutionId > 0)
                {
                    //get list of classes for assigned institution
                    classObjs = songChannel.ListClassesByInstitution(
                        Manager.LogonUser.InstitutionId, -1);
                }
                else
                {
                    //get list of all classes
                    classObjs = songChannel.ListClasses();
                }

                //check result
                if (classObjs[0].Result == (int)SelectResult.Success)
                {
                    //classes were found
                    //return classes
                    return classObjs;
                }
                else if (classObjs[0].Result == (int)SelectResult.Empty)
                {
                    //no class is available
                    return null;
                }
                else if (classObjs[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting classes
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        itemTypeDescription, classObjs[0].ErrorMessage));

                    //could not get classes
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

            //could not get classes
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
                //inactivate selected classObj and get result
                DeleteResult result = songChannel.InactivateClass(
                    SelectedItemId, reasonForm.InactivationReason);

                //check result
                if (result.Result == (int)SelectResult.Success)
                {
                    //item was inactivated
                    //check if there is a parent control
                    //and if it is an classObj register control
                    if (parentControl != null && parentControl is ViewClassControl)
                    {
                        //update classObj to inactive in parent control
                        ((ViewClassControl)parentControl).UpdateClass(
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
            //remove past and in progress semesters from list of semesters
            //check if semester can still be selected
            if (classProgress != ClassProgress.Registration)
            {
                //no need to remove semesters
                //user won't be able to selected semester
                //exit
                return;
            }

            //check current selected semester
            if (mcbSemester.SelectedItem == null)
            {
                //should never happen
                //exit
                return;
            }

            //get current selected semester
            Semester selectedSemester = (Semester)mcbSemester.SelectedItem;

            //remove past and in progress semesters from list of semesters
            List<Semester> displayedSemesters = (List<Semester>)mcbSemester.DataSource;

            //check each semester
            for (int i = displayedSemesters.Count - 1; i >= 0; i--)
            {
                //check if semester has started no more than a month ago
                if (displayedSemesters[i].StartDate < DateTime.Today.AddMonths(-1))
                {
                    //semester has started
                    //check if semester is current semester
                    //and if user has permission to select current semester
                    if (displayedSemesters[i].SemesterId == Manager.CurrentSemester.SemesterId &&
                        Manager.HasLogonPermission("Class.Edit.Semester.SetCurrent"))
                    {
                        //keep current semester
                        continue;
                    }

                    //remove semester
                    displayedSemesters.RemoveAt(i);
                }
            }

            //display semesters and reselect semester
            mcbSemester.DataSource = null;
            mcbSemester.ValueMember = "SemesterId";
            mcbSemester.DisplayMember = "Description";
            mcbSemester.DataSource = displayedSemesters;
            mcbSemester.SelectedItem = selectedSemester;
        }

        /// <summary>
        /// Start creating a new item from scratch.
        /// </summary>
        public override void CreateItem()
        {
            //must get next available subject code
            int nextSubjectCode = int.MinValue;

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel != null)
            {
                try
                {
                    //get next available subject code
                    CountResult nextCount = songChannel.FindNextAvailableSubjectCode();

                    //check result
                    if (nextCount.Result != (int)SelectResult.FatalError)
                    {
                        //set next available subject code
                        nextSubjectCode = nextCount.Count;
                    }
                    else
                    {
                        //display message
                        MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.errorWebServiceCountItem,
                            "Subject Code", nextCount.ErrorMessage),
                            Properties.Resources.titleWebServiceError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //log error
                        Manager.Log.WriteError(string.Format(
                            Properties.Resources.errorWebServiceCountItem,
                            "Subject Code", nextCount.ErrorMessage));
                    }
                }
                catch (Exception ex)
                {
                    //database error while getting subject code
                    Manager.Log.WriteException(string.Format(
                        Properties.Resources.errorChannelListItem, "Subject Code"), ex);
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

            //check final result
            if (nextSubjectCode == int.MinValue)
            {
                //could not load next subject code
                //cancel new item
                btCancel_Click(this, new EventArgs());

                //exit
                return;
            }

            //set subject code with next available number
            subjectCode = nextSubjectCode;
            
            //remove past and in progress semesters from list of semesters
            List<Semester> displayedSemesters = (List<Semester>)mcbSemester.DataSource;

            //check each semester
            for (int i = displayedSemesters.Count - 1; i >= 0; i--)
            {
                //check if semester has started no more than a month ago
                if (displayedSemesters[i].StartDate < DateTime.Today.AddMonths(-1))
                {
                    //semester has started
                    //check if semester is current semester
                    //and if user has permission to select current semester
                    if (displayedSemesters[i].SemesterId == Manager.CurrentSemester.SemesterId &&
                        Manager.HasLogonPermission("Class.Edit.Semester.SetCurrent"))
                    {
                        //keep current semester
                        continue;
                    }

                    //remove semester
                    displayedSemesters.RemoveAt(i);
                }
            }

            //display semesters
            mcbSemester.DataSource = null;
            mcbSemester.ValueMember = "SemesterId";
            mcbSemester.DisplayMember = "Description";
            mcbSemester.DataSource = displayedSemesters;

            //check next semester
            if (nextSemester != null)
            {
                //select next semester as default
                mcbSemester.SelectedValue = nextSemester.SemesterId;
            }
            else
            {
                //should never happen
                //check if there is any semester
                if (displayedSemesters.Count > 0)
                {
                    //select first available semester
                    mcbSemester.SelectedIndex = 0;
                }
            }

            ////set code by ensuring that semester index changed event is called
            //mcbSemester_SelectedIndexChanged(this, new EventArgs());

            //set class progress to registration
            //semester has not started yet
            classProgress = ClassProgress.Registration;

            //clear registrations by displaying empty list
            DisplayRegistrations(new List<Registration>());

            //clear displayed attendances
            mcbAttendanceMonth.DataSource = null;
            ClearDisplayedAttendances();

            //enable fields again after setting class progress
            EnableFields(true);

            //select first tab
            mtbTabManager.SelectedIndex = 0;

            //focus classObj type field
            mcbClassType.Focus();
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

            //create an classObj and set data
            Class classObj = new Class();

            //set selected classObj ID
            classObj.ClassId = selectedId;

            //check selected status
            if (mcbStatus.SelectedIndex >= 0)
            {
                //set status
                classObj.ClassStatus = (int)mcbStatus.SelectedValue;

                //check if selected status is inactive
                if ((int)mcbStatus.SelectedValue == (int)ItemStatus.Inactive)
                {
                    //create inactivation reason form
                    InactivationReasonForm reasonForm = new InactivationReasonForm(
                        itemTypeDescription, (int)mcbStatus.SelectedValue, inactivationReason);

                    //let user input an inactivation reason
                    reasonForm.ShowDialog(this);

                    //set inactivation reason with result
                    classObj.InactivationReason = reasonForm.InactivationReason;

                    //set inactivation time
                    classObj.InactivationTime = (inactivationTime != DateTime.MinValue) ?
                        inactivationTime : DateTime.Now;
                }
                else
                {
                    //reset inactivation
                    classObj.InactivationReason = string.Empty;
                    classObj.InactivationTime = DateTime.MinValue;
                }
            }
            else
            {
                //should never happen
                //set default status
                classObj.ClassStatus = (int)ItemStatus.Active;

                //reset inactivation
                classObj.InactivationTime = DateTime.MinValue;
                classObj.InactivationReason = string.Empty;
            }

            //set subject code
            classObj.SubjectCode = subjectCode;

            //set code
            classObj.Code = mtxtCode.Text.Trim();

            //set semester
            classObj.SemesterId = (int)mcbSemester.SelectedValue;

            //set semester
            classObj.Semester = (Semester)mcbSemester.SelectedItem;

            //set class type
            classObj.ClassType = (int)mcbClassType.SelectedValue;

            //set instrument type
            classObj.InstrumentType = (int)mcbInstrumentType.SelectedValue;

            //set class level
            classObj.ClassLevel = (int)mcbClassLevel.SelectedValue;

            //set pole
            classObj.PoleId = (int)mcbPole.SelectedValue;

            //set pole name to properly display classObj in datagridview
            classObj.PoleName = ((IdDescriptionStatus)mcbPole.SelectedItem).Description;

            //set teacher
            classObj.TeacherId = (int)mcbTeacher.SelectedValue;

            //set teacher name and id to properly display classObj in datagridview
            classObj.TeacherName = ((IdDescriptionStatus)mcbTeacher.SelectedItem).Description;
            classObj.TeacherId = ((IdDescriptionStatus)mcbTeacher.SelectedItem).Id;

            //set capacity
            classObj.Capacity = (int)nudCapacity.Value;

            //set start time
            classObj.StartTime = DateTime.ParseExact(
                mtxtStartTime.Text + ":00", "H:m:s", null);

            //set duration
            classObj.Duration = (int)nudDuration.Value;

            //set week days
            classObj.WeekMonday = mcbMonday.Checked;
            classObj.WeekTuesday = mcbTuesday.Checked;
            classObj.WeekWednesday = mcbWednesday.Checked;
            classObj.WeekThursday = mcbThursday.Checked;
            classObj.WeekFriday = mcbFriday.Checked;
            classObj.WeekSaturday = mcbSaturday.Checked;
            classObj.WeekSunday = mcbSunday.Checked;

            //gather list of registrations to be saved
            List<Registration> saveRegistrations = new List<Registration>();

            //check each registration row
            for (int i = 0; i < dtRegistrations.Rows.Count; i++)
            {
                //get registration for current row
                Registration registration = FindRegistration(
                    (int)dtRegistrations.Rows[i][columnIndexRegistrationId]);

                //set position to avoid errors
                registration.Position = i;

                //add registration to be saved
                saveRegistrations.Add(registration);
            }

            //gather list of attendances
            List<Attendance> saveAttendances = new List<Attendance>();

            //check displayed class attendances
            if (displayedClassAttendances != null)
            {
                //save attendances that were saved
                saveAttendances.AddRange(
                    displayedClassAttendances.FindAll(a => a.Updated));
            }            

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not save classObj
                return null;
            }

            try
            {
                //save classObj and get result
                SaveResult saveResult = songChannel.SaveClass(
                    classObj, saveRegistrations, saveAttendances);

                //check result
                if (saveResult.Result == (int)SelectResult.FatalError)
                {
                    //classObj was not saved
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

                    //could not save classObj
                    return null;
                }

                //set saved ID to classObj ID
                classObj.ClassId = saveResult.SavedId;

                //set selected ID
                selectedId = saveResult.SavedId;

                //set previous displayed day index
                previousDisplayedClassDayIndex = displayedClassDayIndex;

                //check if there is a parent control and its type
                if (parentControl != null && parentControl is ViewClassControl)
                {
                    //update class in parent control
                    ((ViewClassControl)parentControl).UpdateClass(classObj);
                }
                else if (parentControl != null && parentControl is ViewRegistrationControl)
                {
                    //update class in parent control
                    ((ViewRegistrationControl)parentControl).UpdateClass(classObj);

                    //update registrations in parent control
                    ((ViewRegistrationControl)parentControl).UpdateRegistrations(classObj.ClassId);
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

            //class was saved
            //get description
            IdDescriptionStatus classDescription = classObj.GetDescription();

            //return updated description
            return classDescription;
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
        private void RegisterClass_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set font to UI controls
            mtxtStartTime.Font = MetroFramework.MetroFonts.Default(13.0F);
            nudCapacity.Font = MetroFramework.MetroFonts.Default(13.0F);
            nudDuration.Font = MetroFramework.MetroFonts.Default(13.0F);

            //set font to datagridviews
            dgvAttendances.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvAttendances.DefaultCellStyle.Font =
                MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            dgvRegistrations.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.DefaultLight(12);
            dgvRegistrations.DefaultCellStyle.Font =
                MetroFramework.MetroFonts.DefaultLight(12);

            //display first tab
            mtbTabManager.SelectedIndex = 0;
        }

        /// <summary>
        /// Semester combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check selected value
            if (mcbSemester.SelectedIndex < 0)
            {
                //no value is selected
                //exit
                return;
            }

            //check subject code
            if (subjectCode < 0)
            {
                //should never happen
                //exit
                return;
            }

            //generate new code and set it to UI
            mtxtCode.Text = ((Semester)mcbSemester.SelectedItem).Description + 
                "." + subjectCode.ToString("00000");
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
                //set empty list of teachers
                mcbTeacher.DataSource = new List<IdDescriptionStatus>();

                //exit
                return;
            }

            //list teachers for selected pole
            ListTeachers((int)mcbPole.SelectedValue);
        }

        /// <summary>
        /// Class type selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbClassType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check selected index
            if (mcbClassType.SelectedIndex == -1)
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

            //check if instrument is selected
            if ((int)mcbClassType.SelectedValue == (int)ClassType.Instrument)
            {
                //enable instrument combo
                mcbInstrumentType.Enabled = true;

                //check selected instrument type
                if (mcbInstrumentType.SelectedIndex <= 0)
                {
                    //selet default instrument
                    mcbInstrumentType.SelectedValue = (int)InstrumentsType.Violino;
                }
            }
            else
            {
                //disable instrument combo
                mcbInstrumentType.Enabled = false;

                //select no specific instrument type
                mcbInstrumentType.SelectedIndex = 0;
            }
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

            //let user select student
            SelectStudentForm studentForm = new SelectStudentForm(
                (int)mcbPole.SelectedValue, Manager.HasLogonPermission("Registration.InsertAnyStudent"));

            //display form as a dialog
            if (studentForm.ShowDialog(Manager.MainForm) == DialogResult.Cancel)
            {
                //user canceled operation
                //exit
                return;
            }

            //get selected student description
            IdDescriptionStatus selectedStudent = studentForm.SelectedStudent;

            //lock list of registrations
            lock (registrations)
            {
                //check if selected student is already registered to class
                if (new List<Registration>(registrations.Values).Find(
                    r => r.StudentId == selectedStudent.Id) != null)
                {
                    //selected student already registered to class
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.msgStudentAlreadyRegistered,
                        selectedStudent.Description, mtxtCode.Text),
                        Properties.Resources.item_Registration,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        
                    //find selected student previous registration
                    //lock data table of registrations
                    lock (dtRegistrations)
                    {
                        //check each registration row
                        for (int i = 0; i < dtRegistrations.Rows.Count; i++)
                        {
                            //get next row registration
                            Registration previousRegistration = FindRegistration(
                                (int)dtRegistrations.Rows[i][columnIndexRegistrationId]);

                            //compare students
                            if (previousRegistration.StudentId == selectedStudent.Id)
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
            registration.StudentId = selectedStudent.Id;
            registration.StudentName = selectedStudent.Description;
            registration.ClassId = selectedId;
            registration.AutoRenewal = true;
            registration.CreationTime = DateTime.Now;
            registration.InactivationReason = string.Empty;
            registration.InactivationTime = DateTime.MinValue;

            //create and set registration class
            registration.Class = new Class(selectedId);
            registration.Class.Code = mtxtCode.Text;

            //lock list of registrations
            lock (registrations)
            {
                //set position according to number of previous registrations
                registration.Position = registrations.Count;

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
                dtRegistrations.Rows.Add(dr);
            }

            //refresh displayed registrations
            //select and display last row
            RefreshRegistrations(dtRegistrations.Rows.Count - 1, true);
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

            //check if class has already started
            if (classProgress == ClassProgress.InProgress)
            {
                //semester has started
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
                //semester has not started yet
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

            //update each subsequent datarow
            for (int i = selectedIndex; i < dtRegistrations.Rows.Count; i++)
            {
                //get current row
                DataRow currentRow = dtRegistrations.Rows[i];

                //get current row registration
                Registration currentRegistration = FindRegistration((int)currentRow[columnIndexRegistrationId]);

                //decrement position for current row registration
                currentRegistration.Position--;

                //set current row
                SetRegistrationDataRow(currentRow, currentRegistration);
            }

            //clear selection
            dgvRegistrations.ClearSelection();

            //refresh grid
            dgvRegistrations.Refresh();
        }

        /// <summary>
        /// Decrease position button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnDecreaseRegistration_Click(object sender, EventArgs e)
        {
            //check if there is a selected registration
            if (dgvRegistrations.SelectedRows.Count == 0)
            {
                //no selected registration
                //exit
                return;
            }

            //check selected row index
            if (dgvRegistrations.SelectedRows[0].Index == 0)
            {
                //cannot decrease position
                //should never happen
                //exit
                return;
            }

            //get selected index
            int selectedIndex = dgvRegistrations.SelectedRows[0].Index;

            //clear selection
            dgvRegistrations.ClearSelection();

            //get operation rows
            DataRow lowerRow = dtRegistrations.Rows[selectedIndex - 1];
            DataRow higherRow = dtRegistrations.Rows[selectedIndex];

            //get operation registrations
            Registration lowerRegistration = FindRegistration((int)lowerRow[columnIndexRegistrationId]);
            Registration higherRegistration = FindRegistration((int)higherRow[columnIndexRegistrationId]);

            //update positions
            lowerRegistration.Position++;
            higherRegistration.Position--;

            //switch rows data
            higherRow["RegistrationId"] = int.MinValue;
            SetRegistrationDataRow(lowerRow, higherRegistration);
            SetRegistrationDataRow(higherRow, lowerRegistration);

            //update selected index to its new position
            selectedIndex--;

            //refresh grid
            dgvRegistrations.Refresh();

            //refresh grid by displaying selected row
            dgvRegistrations.FirstDisplayedScrollingRowIndex =
                selectedIndex > 3 ? (selectedIndex - 4) : 0;

            //select row
            dgvRegistrations.Rows[selectedIndex].Selected = true;
        }

        /// <summary>
        /// Increase position button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnIncreaseRegistration_Click(object sender, EventArgs e)
        {
            //check if there is a selected registration
            if (dgvRegistrations.SelectedRows.Count == 0)
            {
                //no selected registration
                //exit
                return;
            }

            //check selected row index
            if (dgvRegistrations.SelectedRows[0].Index == dgvRegistrations.RowCount - 1)
            {
                //cannot increase position
                //should never happen
                //exit
                return;
            }

            //get selected index
            int selectedIndex = dgvRegistrations.SelectedRows[0].Index;

            //clear selection
            dgvRegistrations.ClearSelection();

            //get operation rows
            DataRow lowerRow = dtRegistrations.Rows[selectedIndex];
            DataRow higherRow = dtRegistrations.Rows[selectedIndex + 1];

            //get operation registrations
            Registration lowerRegistration = FindRegistration((int)lowerRow[columnIndexRegistrationId]);
            Registration higherRegistration = FindRegistration((int)higherRow[columnIndexRegistrationId]);

            //update positions
            lowerRegistration.Position++;
            higherRegistration.Position--;

            //switch rows data
            higherRow["RegistrationId"] = int.MinValue;
            SetRegistrationDataRow(lowerRow, higherRegistration);
            SetRegistrationDataRow(higherRow, lowerRegistration);

            //update selected index to its new position
            selectedIndex++;

            //refresh grid
            dgvRegistrations.Refresh();

            //refresh grid by displaying selected row
            dgvRegistrations.FirstDisplayedScrollingRowIndex =
                selectedIndex > 3 ? (selectedIndex - 4) : 0;

            //select row
            dgvRegistrations.Rows[selectedIndex].Selected = true;
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
                        //set student name
                        mtxtRegistrationStudent.Text = registration.StudentName;

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

            //check row registration status
            if ((int)dtRegistrations.Rows[e.RowIndex]["RegistrationStatusValue"] == (int)ItemStatus.Evaded)
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
            //compare row index to capacity
            else if (e.RowIndex >= nudCapacity.Value)
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
        /// Roll calls metro tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlRollCalls_Click(object sender, EventArgs e)
        {
            //check if there is no displayed class or selected month
            if (displayedClass == null || mcbAttendanceMonth.SelectedIndex == -1)
            {
                //should never happen
                //disable tile
                mtlRollCalls.Enabled = false;

                //exit
                return;
            }

            //generate file for teacher month roll calls
            //check if there is a selected month
            if (mcbAttendanceMonth.SelectedIndex < 0)
            {
                //no month selected
                //should never happen
                //disable tile
                mtlRollCalls.Enabled = false;

                //exit
                return;
            }

            //check selected teacher
            if (mcbTeacher.SelectedIndex > -1)
            {
                //set teacher name
                displayedClass.TeacherName = mcbTeacher.Text;
            }

            //get selected month
            DateTime month = (DateTime)mcbAttendanceMonth.SelectedValue;

            //let user select file path
            //set file default name
            sfdRollCallsFile.FileName =
                Properties.Resources.item_RollCall + " - " + displayedClass.Code +
                " - " + Properties.Resources.ResourceManager.GetString("Month_" + month.Month) + " " +
                month.Year;

            //display file dialog
            if (sfdRollCallsFile.ShowDialog() != DialogResult.OK)
            {
                //user cancelled operation
                //exit
                return;
            }

            //gather data for file
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
                //create list of classes and add displayed class
                List<Class> classes = new List<Class>();
                classes.Add(displayedClass);

                //create list of class students and add displayed students
                Dictionary<int, List<IdDescriptionStatus>> classStudents = 
                    new Dictionary<int, List<IdDescriptionStatus>>();
                classStudents[displayedClass.ClassId] = displayedClassStudents;

                //create list of class attendances and add displayed attendances
                Dictionary<int, List<Attendance>> classAttendances =
                    new Dictionary<int, List<Attendance>>();
                classAttendances[displayedClass.ClassId] = displayedClassAttendances;

                //must generate roll call file and save it to file
                string errorMessage = string.Empty;

                //generate roll call file with data
                if (Manager.FileManager.GenerateRollCallFile(
                    sfdRollCallsFile.FileName, month, classes,
                    classStudents, classAttendances, ref errorMessage))
                {
                    try
                    {
                        //file was created
                        //open file
                        System.Diagnostics.Process.Start(sfdRollCallsFile.FileName);
                    }
                    catch (Exception ex)
                    {
                        //could not start IE
                        Manager.Log.WriteException(
                            "Could not open roll call file on default browser.", ex);
                    }
                }
                else
                {
                    //error while saving file
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorGenerateRollCallFile, errorMessage),
                        Properties.Resources.titleError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch (Exception ex)
            {
                //database error while getting users
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_RollCall), ex);

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
        /// Attendace combo selected index changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbAttendance_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if is loading attendance data 
            if (isLoadingAttendance)
            {
                //is loading
                //exit
                return;
            }

            //check selected month
            if (mcbAttendanceMonth.SelectedIndex == -1)
            {
                //no month or class is selected
                //clear displayed attendances
                ClearDisplayedAttendances();

                //exit
                return;
            }

            //load attendances
            LoadAttendances((DateTime)mcbAttendanceMonth.SelectedValue);
        }

        /// <summary>
        /// Previous day metro tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlPreviousDay_Click(object sender, EventArgs e)
        {
            //check current selected class day
            if (displayedClassDayIndex == 0)
            {
                //there is no other day in month
                //should never happen
                //hide tile
                mtlPreviousDay.Visible = false;

                //exit
                return;
            }

            //decrement displayed day index
            displayedClassDayIndex--;

            //display attendances for current class day
            DisplayClassDayAttendances();

            //enable attendace ui
            EnableAttendanceFields();
        }

        /// <summary>
        /// Next day metro tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlNextDay_Click(object sender, EventArgs e)
        {
            //check current selected class day
            if (displayedClassDayIndex == classDays.Count - 1)
            {
                //there is no other day in month
                //should never happen
                //hide tile
                mtlNextDay.Visible = false;

                //exit
                return;
            }

            //increment displayed day index
            displayedClassDayIndex++;

            //display attendances for current class day
            DisplayClassDayAttendances();

            //enable attendace ui
            EnableAttendanceFields();
        }

        /// <summary>
        /// Attendances datagridview cell dirty state changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAttendances_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //commit edit so cell value changed event is raised 
            dgvAttendances.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        
        /// <summary>
        /// Attendances datagridview cell painting event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAttendances_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //check row index
            if (e.RowIndex < 0)
            {
                //exit
                return;
            }

            //check row attendance status
            if ((int)dtAttendances.Rows[e.RowIndex]["StudentStatusValue"] == (int)ItemStatus.Evaded)
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
        }

        /// <summary>
        /// Attendances datagridview cell value changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAttendances_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //check row index
            if (e.RowIndex < 0)
            {
                //exit
                return;
            }

            //get changed attendance data row
            DataRow attendanceDataRow = dtAttendances.Rows[e.RowIndex];

            //update its roll call text column
            attendanceDataRow["RollCallText"] = Properties.Resources.ResourceManager.GetString(
                "RollCall_" + ((RollCall)attendanceDataRow[columnIndexRollCall]).ToString());

            //get id of the changed attendance
            int attendanceId = (int)attendanceDataRow[columnIndexAttendanceId];

            //get attendace using its id
            Attendance attendance = displayedClassDayAttendances.Find(
                a => a.AttendanceId == attendanceId);

            //update attendance
            attendance.RollCall = (int)attendanceDataRow[columnIndexRollCall];
            attendance.Updated = true;
        }

        /// <summary>
        /// Time masked text box click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Time_Click(object sender, EventArgs e)
        {
            //get sender textbox
            MaskedTextBox mtxtTime = (MaskedTextBox)sender;

            //check text
            if (mtxtTime.Text.Equals("  :"))
            {
                //set cursor position
                mtxtTime.Select(0, 0);
            }
        }

        #endregion UI Event Handlers

    } //end of class RegisterClassControl

} //end of namespace PnT.SongClient.UI.Controls
