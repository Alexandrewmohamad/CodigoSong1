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
    /// This control is used to manage teacher registries in web service.
    /// Inherits from base register control.
    /// </summary>
    public partial class RegisterTeacherControl : RegisterBaseControl
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
        /// The list of all poles.
        /// </summary>
        private List<IdDescriptionStatus> allPoles = null;

        /// <summary>
        /// The list of role poles currently saved in database.
        /// </summary>
        private List<IdDescriptionStatus> databasePoles = null;

        /// <summary>
        /// The list of available poles.
        /// </summary>
        private List<IdDescriptionStatus> availablePoles = null;

        /// <summary>
        /// The list of role selected poles.
        /// </summary>
        private List<IdDescriptionStatus> selectedPoles = null;

        /// <summary>
        /// The current semester.
        /// </summary>
        private Semester currentSemester = null;

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
        /// The list of class students by class ID.
        /// </summary>
        private Dictionary<int, List<IdDescriptionStatus>> classStudents = null;

        /// <summary>
        /// The current displayed class student list.
        /// </summary>
        private List<IdDescriptionStatus> displayedClassStudents = null;

        /// <summary>
        /// The list of class attendances by class ID.
        /// </summary>
        private Dictionary<int, List<Attendance>> classAttendances = null;

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
        /// The id of the previous selected class.
        /// Keep track of selected class while reloading data.
        /// </summary>
        private int previousSelectedClassId = int.MinValue;

        /// <summary>
        /// The index of the previous displayed class day.
        /// Keep track of selected class day while reloading data.
        /// </summary>
        private int previousDisplayedClassDayIndex = int.MinValue;

        #endregion Fields


        #region Constructors **********************************************************


        public RegisterTeacherControl() : base("Teacher", Manager.Settings.HideInactiveTeachers)
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

            //teacher cannot be deleted
            this.classHasDeletion = false;

            //set permissions
            this.allowAddItem = Manager.HasLogonPermission("Teacher.Insert");
            this.allowEditItem = Manager.HasLogonPermission("Teacher.Edit");
            this.allowDeleteItem = Manager.HasLogonPermission("Teacher.Inactivate");

            //check if a teacher is viewing its register and classes
            if (Manager.LogonTeacher != null &&
                !Manager.HasLogonPermission("Teacher.View"))
            {
                //allow teacher to edit its register and classes
                this.allowEditItem = true;
            }

            //create lists
            classDays = new List<DateTime>();
            classStudents = new Dictionary<int, List<IdDescriptionStatus>>();
            classAttendances = new Dictionary<int, List<Attendance>>();

            //create attendance data table
            CreateAttendanceDataTable();

            //get attendance ID and attendance student ID column indexes
            columnIndexAttendanceId = dgvAttendances.Columns[AttendanceId.Name].Index;
            columnIndexRollCall = dgvAttendances.Columns[RollCallValue.Name].Index;

            //avoid auto generated columns
            dgvAttendances.AutoGenerateColumns = false;

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

            //load states
            mcbState.ValueMember = "Key";
            mcbState.DisplayMember = "Value";
            mcbState.DataSource = ListStates();

            //list users
            ListUsers();

            //load poles
            LoadPoles();

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

            //list semester months
            ListMonths();
        }

        #endregion Constructors


        #region Properties ************************************************************

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
            //reset displayed class
            displayedClass = null;

            //clear class days
            classDays = new List<DateTime>();

            //reset displayed class day index
            displayedClassDayIndex = -1;

            //reset displayed class students
            displayedClassStudents = new List<IdDescriptionStatus>();

            //reset displayed class attendances
            displayedClassAttendances = new List<Attendance>();

            //reset displayed class day attendances
            displayedClassDayAttendances = new List<Attendance>();

            //clear attendance datatable
            ClearAttendanceDataTable();

            //enable attendace ui
            EnableAttendanceFields();
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
                    attendance.TeacherId = selectedId;

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
        /// Enable attendance fields according to current context.
        /// </summary>
        private void EnableAttendanceFields()
        {
            //enable roll calls tile
            mtlRollCalls.Enabled = mcbAttendanceClass.Items.Count > 0 &&
                mcbAttendanceMonth.SelectedIndex > -1;

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
        }

        /// <summary>
        /// List semester months into UI.
        /// </summary>
        private void ListMonths()
        {
            //set default empty list to UI
            mcbAttendanceMonth.ValueMember = "Key";
            mcbAttendanceMonth.DisplayMember = "Value";
            mcbAttendanceMonth.DataSource = new List<KeyValuePair<DateTime,string>>();

            try
            {
                //set loading flag
                isLoadingAttendance = true;

                //get current semester
                currentSemester = Manager.CurrentSemester;

                //check result
                if (currentSemester.Result == (int)SelectResult.Success)
                {
                    //add months to list
                    //create list
                    List<KeyValuePair<DateTime, string>> months = 
                        new List<KeyValuePair<DateTime, string>>();

                    //get first month
                    DateTime currentMonth = currentSemester.StartMonth;

                    //keep adding months
                    while (currentMonth <= currentSemester.EndMonth)
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

                    //get today month
                    DateTime todayMonth = DateTime.Today;
                    todayMonth = new DateTime(todayMonth.Year, todayMonth.Month, 1);

                    //check if month is listed
                    if (months.FindIndex(m => m.Key.Equals(todayMonth)) > -1)
                    {
                        //select today month
                        mcbAttendanceMonth.SelectedValue = todayMonth;
                    }
                }
                else if (currentSemester.Result == (int)SelectResult.Empty)
                {
                    //current semester is not available
                    //reset semester
                    currentSemester = null;

                    //exit
                    return;
                }
            }
            finally
            {
                //reset loading flag
                isLoadingAttendance = false;
            }
        }

        /// <summary>
        /// List users into UI.
        /// </summary>
        private void ListUsers()
        {
            //set default empty list to UI
            mcbUser.ValueMember = "Id";
            mcbUser.DisplayMember = "Description";
            mcbUser.DataSource = new List<IdDescriptionStatus>();

            //load users
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
                //get list of active users
                List<IdDescriptionStatus> users =
                    songChannel.ListUsersByStatus((int)ItemStatus.Active);

                //check result
                if (users[0].Result == (int)SelectResult.Success)
                {
                    //sort users by description
                    users.Sort((x, y) => x.Description.CompareTo(y.Description));

                    //set users to UI
                    mcbUser.ValueMember = "Id";
                    mcbUser.DisplayMember = "Description";
                    mcbUser.DataSource = users;
                }
                else if (users[0].Result == (int)SelectResult.Empty)
                {
                    //no user is available
                    //exit
                    return;
                }
                else if (users[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting users
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_User, users[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_User,
                        users[0].ErrorMessage));

                    //could not get users
                    //exit
                    return;
                }
            }
            catch (Exception ex)
            {
                //database error while getting users
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_User), ex);

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
        /// Load attendances for selected class and month and display them.
        /// </summary>
        /// <param name="classObj">
        /// The selected class.
        /// </param>
        /// <param name="month">
        /// The selected month.
        /// </param>
        private void LoadAttendances(Class classObj, DateTime month)
        {
            //set displayed class
            displayedClass = classObj;

            //get list of class days for selected month
            classDays = classObj.GetClassDays(month);

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

            //reset previous selected class id and displayed day index
            previousSelectedClassId = int.MinValue;
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
                //get list of students for selected class
                //check if students were already loaded for selected class
                if (!classStudents.ContainsKey(classObj.ClassId))
                {
                    //load students for selected class
                    List<IdDescriptionStatus> students = 
                        songChannel.ListStudentsByClass(classObj.ClassId, -1);

                    //check result
                    if (students[0].Result == (int)SelectResult.Success)
                    {
                        //remove waiting list students
                        while (students.Count > displayedClass.Capacity)
                        {
                            //remove last student
                            students.RemoveAt(students.Count - 1);
                        }

                        //sort students by name
                        students.Sort((a, b) => a.Description.CompareTo(b.Description));

                        //add list to dictionary
                        classStudents[classObj.ClassId] = students;
                        
                        //load list of registrations for selected class
                        List<Registration> registrations = 
                            songChannel.FindRegistrationsByClass(classObj.ClassId, -1);

                        //check result
                        if (registrations[0].Result == (int)SelectResult.Empty)
                        {
                            //no registration for selected class
                            //clear list
                            registrations.Clear();
                        }
                        else if (registrations[0].Result == (int)SelectResult.FatalError)
                        {
                            //database error while getting registrations
                            //display message
                            MetroMessageBox.Show(Manager.MainForm, string.Format(
                                Properties.Resources.errorWebServiceListItem,
                                Properties.Resources.item_Registration,
                                registrations[0].ErrorMessage),
                                Properties.Resources.titleWebServiceError,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                            //write error
                            Manager.Log.WriteError(string.Format(
                                Properties.Resources.errorWebServiceListItem,
                                Properties.Resources.item_Registration,
                                registrations[0].ErrorMessage));

                            //exit
                            return;
                        }

                        //check each student
                        foreach (IdDescriptionStatus student in students)
                        {
                            //find registration for student
                            Registration registration = registrations.Find(
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
                    }
                    else if (students[0].Result == (int)SelectResult.Empty)
                    {
                        //no student for selected class
                        //clear list
                        students.Clear();

                        //add list to dictionary
                        classStudents[classObj.ClassId] = students;
                    }
                    else if (students[0].Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting students
                        //clear displayed attendances
                        ClearDisplayedAttendances();

                        //display message
                        MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.errorWebServiceListItem,
                            Properties.Resources.item_Student, students[0].ErrorMessage),
                            Properties.Resources.titleWebServiceError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //write error
                        Manager.Log.WriteError(string.Format(
                            Properties.Resources.errorWebServiceListItem,
                            Properties.Resources.item_Student, students[0].ErrorMessage));

                        //could not get students
                        //exit
                        return;
                    }
                }

                //get students to be displayed
                displayedClassStudents = classStudents[classObj.ClassId];

                //check result
                if (displayedClassStudents == null)
                {
                    //should never happen
                    //clear displayed attendances
                    ClearDisplayedAttendances();

                    //exit
                    return;
                }
                else if (displayedClassStudents.Count == 0)
                {
                    //no student for selected class
                    //clear displayed attendances
                    ClearDisplayedAttendances();

                    //get class code
                    string classCode = classObj.Code.IndexOf(" | ") > -1 ?
                        classObj.Code.Substring(0, classObj.Code.IndexOf(" | ")) : classObj.Code;

                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.msgClassNoStudent, classCode),
                        Properties.Resources.titleNoStudent,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //exit
                    return;
                }

                //get list of attendances for selected class
                //check if attendances were already loaded for selected class
                if (!classAttendances.ContainsKey(classObj.ClassId))
                {
                    //load attendances for selected class
                    List<Attendance> attendances = songChannel.FindAttendancesByFilter(
                        false, false, classObj.ClassId, -1);

                    //check result
                    if (attendances[0].Result == (int)SelectResult.Success)
                    {
                        //sort attendances by date
                        attendances.Sort((a, b) => a.Date.CompareTo(b.Date));

                        //add list to dictionary
                        classAttendances[classObj.ClassId] = attendances;
                    }
                    else if (attendances[0].Result == (int)SelectResult.Empty)
                    {
                        //no attendance for selected class
                        //clear list
                        attendances.Clear();

                        //add list to dictionary
                        classAttendances[classObj.ClassId] = attendances;
                    }
                    else if (attendances[0].Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting attendances
                        //clear displayed attendances
                        ClearDisplayedAttendances();

                        //display message
                        MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.errorWebServiceListItem,
                            Properties.Resources.item_Attendance, attendances[0].ErrorMessage),
                            Properties.Resources.titleWebServiceError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //write error
                        Manager.Log.WriteError(string.Format(
                            Properties.Resources.errorWebServiceListItem,
                            Properties.Resources.item_Pole,
                            attendances[0].ErrorMessage));

                        //could not get attendances
                        //exit
                        return;
                    }
                }

                //get attendances to be displayed
                displayedClassAttendances = classAttendances[classObj.ClassId];

                //check result
                if (displayedClassAttendances == null)
                {
                    //should never happen
                    //clear displayed attendances
                    ClearDisplayedAttendances();

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
        /// Load all poles.
        /// </summary>
        private void LoadPoles()
        {
            //set default empty list
            allPoles = new List<IdDescriptionStatus>();

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

                    //set poles
                    allPoles = poles;
                }
                else if (poles[0].Result == (int)SelectResult.Empty)
                {
                    //no pole is available
                    //should never happen
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
        /// Refresh displayed pole lists and its items.
        /// </summary>
        private void RefreshPoleLists()
        {
            //refresh displayed list of selected poles
            lbSelectedPoles.DataSource = null;
            lbSelectedPoles.DisplayMember = "Description";
            lbSelectedPoles.ValueMember = "Id";
            lbSelectedPoles.DataSource = selectedPoles;
            lbSelectedPoles.SelectedIndex = -1;

            //refresh displayed list of available poles
            lbAvailablePoles.DataSource = null;
            lbAvailablePoles.DisplayMember = "Description";
            lbAvailablePoles.ValueMember = "Id";
            lbAvailablePoles.DataSource = availablePoles;
            lbAvailablePoles.SelectedIndex = -1;
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
        /// Set selected class to display extra data.
        /// </summary>
        /// <param name="classObj">
        /// The selected class.
        /// </param>
        private void SetClass(Class classObj)
        {
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

            //validate identity
            if (!ValidateRequiredField(mtxtIdentity, mlblIdentity.Text, mtbTabManager, tbPersonalData))
            {
                //invalid field
                return false;
            }

            //validate identity agency
            if (!ValidateRequiredField(mtxtIdentityAgency, mlblIdentityAgency.Text, mtbTabManager, tbPersonalData))
            {
                //invalid field
                return false;
            }

            //validate tax id if set
            if (!mtxtTaxId.Text.Equals("   .   .   -") &&
                !ValidateCpfField(mtxtTaxId, null, null))
            {
                //invalid field
                return false;
            }

            //validate identity date if set
            if (!mtxtIdentityDate.Text.Equals("  /  /") &&
                !ValidateDateField(mtxtIdentityDate, mtbTabManager, tbPersonalData))
            {
                //invalid field
                return false;
            }

            //validate academic background
            if (!ValidateRequiredField(mtxtAcademicBackground, mlblAcademicBackground.Text, mtbTabManager, tbPersonalData))
            {
                //invalid field
                return false;
            }

            //validate work experience
            if (!ValidateRequiredField(mtxtWorkExperience, mlblWorkExperience.Text, mtbTabManager, tbPersonalData))
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

            //validate mobile
            if (!ValidateRequiredField(mtxtMobile, mlblMobile.Text, mtbTabManager, tbAddressContact) ||
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
            mtxtTaxId.Text = string.Empty;
            mtxtPisId.Text = string.Empty;
            mtxtIdentity.Text = string.Empty;
            mtxtIdentityAgency.Text = string.Empty;
            mtxtIdentityDate.Text = string.Empty;
            mtxtAcademicBackground.Text = string.Empty;
            mtxtWorkExperience.Text = string.Empty;

            mtxtAddress.Text = string.Empty;
            mtxtDistrict.Text = string.Empty;
            mtxtCity.Text = string.Empty;
            mtxtZipCode.Text = string.Empty;
            mtxtPhone.Text = string.Empty;
            mtxtMobile.Text = string.Empty;
            mtxtEmail.Text = string.Empty;

            //select Rio de Janeiro
            mcbState.SelectedIndex = 17;

            //check number of users
            if (mcbUser.Items.Count > 0)
            {
                //select first user
                mcbUser.SelectedIndex = 0;
            }

            //display poles
            availablePoles = new List<IdDescriptionStatus>(allPoles);
            selectedPoles = new List<IdDescriptionStatus>();
            RefreshPoleLists();

            try
            {
                //clear attendance classes
                //set loading flag
                isLoadingAttendance = true;

                //check if there is a previous selected class
                if (mcbAttendanceClass.SelectedIndex >= 0)
                {
                    //set previous selected class id and displayed day index
                    previousSelectedClassId = (int)mcbAttendanceClass.SelectedValue;
                    previousDisplayedClassDayIndex = displayedClassDayIndex;
                }

                //clear classes
                mcbAttendanceClass.DataSource = null;
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
            Manager.Settings.HideInactiveTeachers = this.hideInactiveItems;
        }

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        /// <returns></returns>
        public override string SelectMenuOption()
        {
            //select teacher option
            return "Teacher";
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
            mtxtTaxId.Enabled = enable;
            mtxtPisId.Enabled = enable;
            mtxtIdentity.Enabled = enable;
            mtxtIdentityAgency.Enabled = enable;
            mtxtIdentityDate.Enabled = enable;
            mtxtAcademicBackground.Enabled = enable;
            mtxtWorkExperience.Enabled = enable;

            mtxtAddress.Enabled = enable;
            mtxtDistrict.Enabled = enable;
            mtxtCity.Enabled = enable;
            mtxtZipCode.Enabled = enable;
            mtxtPhone.Enabled = enable;
            mtxtMobile.Enabled = enable;
            mtxtEmail.Enabled = enable;

            //set state list
            mcbState.Enabled = enable;

            //set user list
            mcbUser.Enabled = enable;

            //call list event handlers and so setting buttons
            lbAvailablePoles_SelectedIndexChanged(this, new EventArgs());
            lbSelectedPoles_SelectedIndexChanged(this, new EventArgs());

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
                //get selected teacher from web service
                Teacher teacher = songChannel.FindTeacher(itemId);

                //check result
                if (teacher.Result == (int)SelectResult.Empty)
                {
                    //should never happen
                    //could not load data
                    return false;
                }
                else if (teacher.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting teacher
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, teacher.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, teacher.ErrorMessage));

                    //could not load data
                    return false;
                }

                //set selected teacher ID
                selectedId = teacher.TeacherId;

                //select status
                mcbStatus.SelectedValue = teacher.TeacherStatus;

                //set inactivation fields
                inactivationReason = teacher.InactivationReason;
                inactivationTime = teacher.InactivationTime;

                //set text fields
                mtxtName.Text = teacher.Name;
                mtxtBirthdate.Text = teacher.Birthdate.ToShortDateString();
                mtxtTaxId.Text = teacher.TaxId;
                mtxtPisId.Text = teacher.PisId;
                mtxtIdentity.Text = teacher.Identity;
                mtxtIdentityAgency.Text = teacher.IdentityAgency;
                mtxtIdentityDate.Text = teacher.IdentityDate.ToShortDateString();
                mtxtAcademicBackground.Text = teacher.AcademicBackground;
                mtxtWorkExperience.Text = teacher.WorkExperience;

                mtxtAddress.Text = teacher.Address;
                mtxtDistrict.Text = teacher.District;
                mtxtCity.Text = teacher.City;
                mtxtZipCode.Text = teacher.ZipCode;
                mtxtPhone.Text = teacher.Phone;
                mtxtMobile.Text = teacher.Mobile;
                mtxtEmail.Text = teacher.Email;

                //set state
                mcbState.SelectedValue = teacher.State;

                //check selected state
                if (mcbState.SelectedIndex < 0)
                {
                    //should never happen
                    //select default state
                    mcbState.SelectedIndex = 17;
                }

                //set user
                mcbUser.SelectedValue = teacher.UserId;

                //check selected index
                if (mcbUser.SelectedIndex < 0)
                {
                    try
                    {
                        //user is not available
                        //it might be inactive
                        //must load user from web service
                        User user = songChannel.FindUser(teacher.UserId);

                        //check result
                        if (user.Result == (int)SelectResult.Success)
                        {
                            //add user to list of users
                            List<IdDescriptionStatus> users =
                                (List<IdDescriptionStatus>)mcbUser.DataSource;
                            users.Add(user.GetDescription());

                            //update displayed list
                            mcbUser.DataSource = null;
                            mcbUser.ValueMember = "Id";
                            mcbUser.DisplayMember = "Description";
                            mcbUser.DataSource = users;

                            //set user
                            mcbUser.SelectedValue = teacher.UserId;
                        }
                    }
                    catch
                    {
                        //do nothing
                    }
                }

                #region load poles

                //get assigned poles for selected teacher
                List<IdDescriptionStatus> poles = songChannel.ListPolesByTeacher(teacher.TeacherId);

                //check result
                if (poles[0].Result == (int)SelectResult.Empty)
                {
                    //teacher has no pole
                    //clear list
                    poles.Clear();
                }
                else if (poles[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting poles
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadPoles,
                        itemTypeDescription, poles[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadPoles,
                        itemTypeDescription, poles[0].ErrorMessage));

                    //could not load data
                    return false;
                }

                //sort poles by description
                poles.Sort((x, y) => x.Description.CompareTo(y.Description));

                //copy list and set database poles
                databasePoles = new List<IdDescriptionStatus>(poles);

                //set selected poles
                selectedPoles = poles;

                //gather list of available poles
                availablePoles = new List<IdDescriptionStatus>();

                //check each pole
                foreach (IdDescriptionStatus pole in allPoles)
                {
                    //check if pole is not selected
                    if (selectedPoles.Find(p => p.Id == pole.Id) == null)
                    {
                        //add pole
                        availablePoles.Add(pole);
                    }
                }

                //refresh displayed pole lists
                RefreshPoleLists();

                #endregion load poles

                #region load attendance data

                //clear attendance data
                classDays = new List<DateTime>();
                classStudents.Clear();
                classAttendances.Clear();

                //check if there is a current semester
                if (currentSemester != null)
                {
                    try
                    {
                        //set loading flag
                        isLoadingAttendance = true;

                        //get list of teacher classes to be displayed
                        List <Class> classes = songChannel.FindClassesByFilter(
                            true, true, false, (int)ItemStatus.Active, -1, -1, -1, 
                            currentSemester.SemesterId, -1, -1, itemId);

                        //check result
                        if (classes[0].Result == (int)SelectResult.Success)
                        {
                            //set teacher name foreach class
                            classes.ToList().ForEach(c => c.TeacherName = teacher.Name);

                            //sort classes by code
                            classes.Sort((x, y) => x.Code.CompareTo(y.Code));

                            //check each class
                            foreach (Class classObj in classes)
                            {
                                //set class to display extra data
                                SetClass(classObj);
                            }
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

                        //set classes to UI
                        mcbAttendanceClass.ValueMember = "ClassId";
                        mcbAttendanceClass.DisplayMember = "Code";
                        mcbAttendanceClass.DataSource = classes;

                        //check if there was a previous selected value
                        if (previousSelectedClassId != int.MinValue &&
                            classes.FindIndex(c => c.ClassId == previousSelectedClassId) > -1)
                        {
                            //select class
                            mcbAttendanceClass.SelectedValue = previousSelectedClassId;
                        }
                        else
                        {
                            //reset previous selected class id and displayed day index
                            previousSelectedClassId = int.MinValue;
                            previousDisplayedClassDayIndex = int.MinValue;
                        }
                    }
                    finally
                    {
                        //reset loading flag
                        isLoadingAttendance = false;
                    }

                    //perform a data load for selected class if any
                    mcbAttendance_SelectedIndexChanged(this, new EventArgs());
                }

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
        /// Load teacher list from database.
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
                //list of teachers
                List<IdDescriptionStatus> teachers = null;
                
                //check if a teacher is viewing its register and classes
                if (Manager.LogonTeacher != null &&
                    !Manager.HasLogonPermission("Teacher.View"))
                {
                    //get list with only logon teacher
                    teachers = new List<IdDescriptionStatus>();
                    teachers.Add(Manager.LogonTeacher.GetDescription());
                }
                //check if logged on user has an assigned institution
                else if (Manager.LogonUser != null &&
                    Manager.LogonUser.InstitutionId > 0)
                {
                    //get list of teachers for assigned institution
                    teachers = songChannel.ListTeachersByInstitution(
                        Manager.LogonUser.InstitutionId, -1);
                }
                else
                {
                    //get list of all teachers
                    teachers = songChannel.ListTeachers();
                }

                //check result
                if (teachers[0].Result == (int)SelectResult.Success)
                {
                    //teachers were found
                    return teachers;
                }
                else if (teachers[0].Result == (int)SelectResult.Empty)
                {
                    //no teacher is available
                    return null;
                }
                else if (teachers[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting teachers
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        itemTypeDescription, teachers[0].ErrorMessage));

                    //could not get teachers
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

            //could not get teachers
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
                //inactivate selected teacher and get result
                DeleteResult result = songChannel.InactivateTeacher(
                    SelectedItemId, reasonForm.InactivationReason);

                //check result
                if (result.Result == (int)SelectResult.Success)
                {
                    //register was inactivated
                    //check if there is a parent control
                    //and if it is an teacher register control
                    if (parentControl != null && parentControl is ViewTeacherControl)
                    {
                        //update teacher to inactive in parent control
                        ((ViewTeacherControl)parentControl).UpdateTeacher(
                            SelectedItemId, (int)ItemStatus.Inactive,
                            DateTime.Now, reasonForm.InactivationReason);
                    }

                    //item was inactivated
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

            //create an teacher and set data
            Teacher teacher = new Teacher();

            //set selected teacher ID
            teacher.TeacherId = selectedId;

            //check selected status
            if (mcbStatus.SelectedIndex >= 0)
            {
                //set status
                teacher.TeacherStatus = (int)mcbStatus.SelectedValue;

                //check if selected status is inactive
                if ((int)mcbStatus.SelectedValue == (int)ItemStatus.Inactive)
                {
                    //create inactivation reason form
                    InactivationReasonForm reasonForm = new InactivationReasonForm(
                        itemTypeDescription, (int)mcbStatus.SelectedValue, inactivationReason);

                    //let user input an inactivation reason
                    reasonForm.ShowDialog(this);

                    //set inactivation reason with result
                    teacher.InactivationReason = reasonForm.InactivationReason;

                    //set inactivation time
                    teacher.InactivationTime = (inactivationTime != DateTime.MinValue) ?
                        inactivationTime : DateTime.Now;
                }
                else
                {
                    //reset inactivation
                    teacher.InactivationReason = string.Empty;
                    teacher.InactivationTime = DateTime.MinValue;
                }
            }
            else
            {
                //should never happen
                //set default status
                teacher.TeacherStatus = (int)ItemStatus.Active;

                //reset inactivation
                teacher.InactivationTime = DateTime.MinValue;
                teacher.InactivationReason = string.Empty;
            }

            //set text fields
            teacher.Name = mtxtName.Text;
            teacher.Birthdate = DateTime.Parse(mtxtBirthdate.Text);
            teacher.TaxId = mtxtTaxId.Text.Equals("   .   .   -") ? 
                string.Empty : mtxtTaxId.Text;
            teacher.PisId = mtxtPisId.Text;
            teacher.Identity = mtxtIdentity.Text;
            teacher.IdentityAgency = mtxtIdentityAgency.Text;
            teacher.IdentityDate = mtxtIdentityDate.Text.Equals("  /  /") ? 
                DateTime.MinValue : DateTime.Parse(mtxtIdentityDate.Text);
            teacher.AcademicBackground = mtxtAcademicBackground.Text;
            teacher.WorkExperience = mtxtWorkExperience.Text;

            teacher.Address = mtxtAddress.Text;
            teacher.District = mtxtDistrict.Text;
            teacher.City = mtxtCity.Text;
            teacher.State = mcbState.SelectedValue.ToString();
            teacher.ZipCode = mtxtZipCode.Text;
            teacher.Phone = mtxtPhone.Text.Equals("(  )     -") ? 
                string.Empty : mtxtPhone.Text;
            teacher.Mobile = mtxtMobile.Text;
            teacher.Email = mtxtEmail.Text;

            //set user
            teacher.UserId = (int)mcbUser.SelectedValue;

            //set user login to properly display teacher in datagridview
            teacher.UserLogin = ((IdDescriptionStatus)mcbUser.SelectedItem).Description;

            //set empty pole names list
            teacher.PoleNames = new List<string>();

            //gather list of seleted pole ids
            List<int> savePoleIds = new List<int>();

            //check each selected pole
            foreach (IdDescriptionStatus pole in selectedPoles)
            {
                //add pole id
                savePoleIds.Add(pole.Id);

                //add pole name to properly display teacher in datagridview
                teacher.PoleNames.Add(pole.Description);
            }

            //sort pole names list
            teacher.PoleNames.Sort();

            //gather list of attendances
            List<Attendance> saveAttendances = new List<Attendance>();

            //check attendances for each class
            foreach (List<Attendance> attendances in classAttendances.Values)
            {
                //save attendances that were saved
                saveAttendances.AddRange(attendances.FindAll(a => a.Updated));
            }

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not save teacher
                return null;
            }

            try
            {
                //save teacher and get result
                SaveResult saveResult = songChannel.SaveTeacher(
                    teacher, savePoleIds, saveAttendances);

                //check result
                if (saveResult.Result == (int)SelectResult.FatalError)
                {
                    //teacher was not saved
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

                    //could not save teacher
                    return null;
                }

                //set saved ID to teacher ID
                teacher.TeacherId = saveResult.SavedId;

                //set selected ID
                selectedId = saveResult.SavedId;

                //check if there is a parent control
                //check if it is an teacher register control
                if (parentControl != null && parentControl is ViewTeacherControl)
                {
                    //update teacher in parent control
                    ((ViewTeacherControl)parentControl).UpdateTeacher(teacher, savePoleIds);
                }
                else if (parentControl != null && parentControl is ViewClassControl)
                {
                    //update class in parent control
                    ((ViewClassControl)parentControl).UpdateTeacher(teacher);
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

            //teacher was saved
            //return updated description
            return teacher.GetDescription();
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
        private void RegisterTeacher_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set font to UI controls
            mtxtBirthdate.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtIdentityDate.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtTaxId.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtZipCode.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtPhone.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtMobile.Font = MetroFramework.MetroFonts.Default(13.0F);

            //set font to datagridviews
            dgvAttendances.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvAttendances.DefaultCellStyle.Font =
                MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);

            //display first tab
            mtbTabManager.SelectedIndex = 0;
        }

        /// <summary>
        /// Available poles listbox selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbAvailablePoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if status is consulting and pole has selected an item
            if (this.Status == RegisterStatus.Consulting && lbAvailablePoles.SelectedIndex >= 0)
            {
                //cannot select while consulting
                //clear selection
                lbAvailablePoles.SelectedIndex = -1;

                //exit
                return;
            }

            //check if is not consulting and the number of selected available poles
            if (this.Status != RegisterStatus.Consulting && lbAvailablePoles.SelectedIndex >= 0)
            {
                //enable button
                mbtnAddPoles.Enabled = true;
                mbtnAddPoles.BackgroundImage = Properties.Resources.IconMoveRightOne;
            }
            else
            {
                //disable button
                mbtnAddPoles.Enabled = false;
                mbtnAddPoles.BackgroundImage = Properties.Resources.IconMoveRightOneDisabled;
            }
        }
        
        /// <summary>
        /// Selected poles listbox selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbSelectedPoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if status is consulting and pole has selected an item
            if (this.Status == RegisterStatus.Consulting && lbSelectedPoles.SelectedIndex >= 0)
            {
                //cannot select while consulting
                //clear selection
                lbSelectedPoles.SelectedIndex = -1;

                //exit
                return;
            }

            //check if is not consulting and the number of selected selected poles
            if (this.Status != RegisterStatus.Consulting && lbSelectedPoles.SelectedIndex > -1)
            {
                //enable button
                mbtnRemovePoles.Enabled = true;
                mbtnRemovePoles.BackgroundImage = Properties.Resources.IconMoveLeftOne;
            }
            else
            {
                //disable button
                mbtnRemovePoles.Enabled = false;
                mbtnRemovePoles.BackgroundImage = Properties.Resources.IconMoveLeftOneDisabled;
            }
        }

        /// <summary>
        /// General poles listbox draw item event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbPoles_DrawItem(object sender, DrawItemEventArgs e)
        {
            //check if it is on design mode
            if (this.DesignMode == true)
            {
                //exit
                return;
            }

            //get sender listbox
            ListBox senderList = (ListBox)sender;

            //check index and number of items
            if (e.Index < 0 || senderList.Items.Count <= e.Index)
            {
                //exit
                return;
            }

            //get pole item
            IdDescriptionStatus pole = (IdDescriptionStatus)senderList.Items[e.Index];

            //draw item
            e.DrawBackground();
            e.Graphics.DrawString(pole.Description, e.Font,
                databasePoles != null && databasePoles.Contains(pole) ? Brushes.Black : Brushes.DarkCyan,
                e.Bounds.Left, e.Bounds.Top);
            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Button add poles click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddPoles_Click(object sender, EventArgs e)
        {
            //check if there is any selected available pole
            if (lbAvailablePoles.SelectedItems.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbAvailablePoles_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected available pole
            for (int i = 0; i < lbAvailablePoles.SelectedItems.Count; i++)
            {
                //get selected pole
                IdDescriptionStatus pole = (IdDescriptionStatus)lbAvailablePoles.SelectedItems[i];

                //remove pole from available poles
                availablePoles.Remove(pole);

                //add pole to selected poles and sort list
                selectedPoles.Add(pole);
            }

            //sort selected pole list
            selectedPoles.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed pole lists
            RefreshPoleLists();
        }

        /// <summary>
        /// Button remove poles click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnRemovePoles_Click(object sender, EventArgs e)
        {
            //check if there is any selected selected pole
            if (lbSelectedPoles.SelectedItems.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedPoles_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected selected pole
            for (int i = 0; i < lbSelectedPoles.SelectedItems.Count; i++)
            {
                //get selected pole
                IdDescriptionStatus pole = (IdDescriptionStatus)lbSelectedPoles.SelectedItems[i];

                //remove pole from selected poles
                selectedPoles.Remove(pole);

                //add pole to available poles and sort list
                availablePoles.Add(pole);
            }

            //sort available pole list
            availablePoles.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed pole lists
            RefreshPoleLists();
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
        /// Tax id masked text box click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtTaxId_Click(object sender, EventArgs e)
        {
            //check text
            if (mtxtTaxId.Text.Equals("   .   .   -"))
            {
                //set cursor position
                mtxtTaxId.Select(0, 0);
            }
        }

        /// <summary>
        /// Roll calls metro tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlRollCalls_Click(object sender, EventArgs e)
        {
            //check if there is no available class
            if (mcbAttendanceClass.DataSource == null)
            {
                //no class is available
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

            //get selected month
            DateTime month = (DateTime)mcbAttendanceMonth.SelectedValue;

            //let user select file path
            //set file default name
            sfdRollCallsFile.FileName = 
                Properties.Resources.item_plural_RollCall + " - " + lsItems.SelectedItem.ToString() + 
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
                //get previously loaded teacher classes for current semester
                List<Class> classes = (List<Class>)mcbAttendanceClass.DataSource;

                //make sure that class students and attendances were loaded for each class
                //check each class
                foreach (Class classObj in classes)
                {
                    //get list of students for selected class
                    //check if students were already loaded for selected class
                    if (!classStudents.ContainsKey(classObj.ClassId))
                    {
                        //load students for selected class
                        List<IdDescriptionStatus> students =
                            songChannel.ListStudentsByClass(classObj.ClassId, -1);

                        //check result
                        if (students[0].Result == (int)SelectResult.Success)
                        {
                            //remove waiting list students
                            while (students.Count > displayedClass.Capacity)
                            {
                                //remove last student
                                students.RemoveAt(students.Count - 1);
                            }

                            //sort students by name
                            students.Sort((a, b) => a.Description.CompareTo(b.Description));

                            //add list to dictionary
                            classStudents[classObj.ClassId] = students;
                        }
                        else if (students[0].Result == (int)SelectResult.Empty)
                        {
                            //no student for selected class
                            //clear list
                            students.Clear();

                            //add list to dictionary
                            classStudents[classObj.ClassId] = students;
                        }
                        else if (students[0].Result == (int)SelectResult.FatalError)
                        {
                            //database error while getting students
                            //clear displayed attendances
                            ClearDisplayedAttendances();

                            //display message
                            MetroMessageBox.Show(Manager.MainForm, string.Format(
                                Properties.Resources.errorWebServiceListItem,
                                Properties.Resources.item_Student, students[0].ErrorMessage),
                                Properties.Resources.titleWebServiceError,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                            //write error
                            Manager.Log.WriteError(string.Format(
                                Properties.Resources.errorWebServiceListItem,
                                Properties.Resources.item_Student, students[0].ErrorMessage));

                            //could not get students
                            //exit
                            return;
                        }
                    }

                    //get list of attendances for selected class
                    //check if attendances were already loaded for selected class
                    if (!classAttendances.ContainsKey(classObj.ClassId))
                    {
                        //load attendances for selected class
                        List<Attendance> attendances = songChannel.FindAttendancesByFilter(
                            false, false, classObj.ClassId, -1);

                        //check result
                        if (attendances[0].Result == (int)SelectResult.Success)
                        {
                            //sort attendances by date
                            attendances.Sort((a, b) => a.Date.CompareTo(b.Date));

                            //add list to dictionary
                            classAttendances[classObj.ClassId] = attendances;
                        }
                        else if (attendances[0].Result == (int)SelectResult.Empty)
                        {
                            //no attendance for selected class
                            //clear list
                            attendances.Clear();

                            //add list to dictionary
                            classAttendances[classObj.ClassId] = attendances;
                        }
                        else if (attendances[0].Result == (int)SelectResult.FatalError)
                        {
                            //database error while getting attendances
                            //clear displayed attendances
                            ClearDisplayedAttendances();

                            //display message
                            MetroMessageBox.Show(Manager.MainForm, string.Format(
                                Properties.Resources.errorWebServiceListItem,
                                Properties.Resources.item_Attendance, attendances[0].ErrorMessage),
                                Properties.Resources.titleWebServiceError,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                            //write error
                            Manager.Log.WriteError(string.Format(
                                Properties.Resources.errorWebServiceListItem,
                                Properties.Resources.item_Pole,
                                attendances[0].ErrorMessage));

                            //could not get attendances
                            //exit
                            return;
                        }
                    }
                }

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

            //check selected month and class
            if (mcbAttendanceMonth.SelectedIndex == -1 ||
                mcbAttendanceClass.SelectedIndex == -1)
            {
                //no month or class is selected
                //clear displayed attendances
                ClearDisplayedAttendances();

                //exit
                return;
            }

            //load attendances
            LoadAttendances(
                (Class)mcbAttendanceClass.SelectedItem,
                (DateTime)mcbAttendanceMonth.SelectedValue);
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

        #endregion UI Event Handlers
        
    } //end of class RegisterTeacherContro

} //end of namespace PnT.SongClient.UI.Controls
