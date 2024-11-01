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
using MetroFramework.Controls;

using PnT.SongDB.Logic;
using PnT.SongServer;

using PnT.SongClient.Logic;

namespace PnT.SongClient.UI.Controls
{

    /// <summary>
    /// Report edit class for teacher monthly report
    /// </summary>
    public partial class EditReportTeacherMonth : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The parent control that opened this control. 
        /// </summary>
        private ISongControl parentControl = null;

        /// <summary>
        /// The selected report.
        /// </summary>
        private Report report = null;

        /// <summary>
        /// The report semester.
        /// </summary>
        private Semester reportSemester = null;

        /// <summary>
        /// Current control status.
        /// </summary>
        private RegisterStatus status = RegisterStatus.Consulting;

        /// <summary>
        /// The type name of the displayed items.
        /// </summary>
        private string itemTypeName = "Report";

        /// <summary>
        /// The item displayable name.
        /// </summary>
        private string itemTypeDescription = Properties.Resources.item_Report;

        /// <summary>
        /// Option to allow user to edit an item.
        /// True if user has permission to edit items.
        /// </summary>
        private bool allowEditItem = true;

        /// <summary>
        /// True if constructor was already performed.
        /// </summary>
        private bool isConstructed = false;

        /// <summary>
        /// The class student list.
        /// </summary>
        private List<IdDescriptionStatus> classStudents = null;

        /// <summary>
        /// The class registration list.
        /// </summary>
        private List<Registration> classRegistrations = null;

        /// <summary>
        /// The list of class days for the selected month.
        /// </summary>
        private List<DateTime> classDays = null;

        /// <summary>
        /// The index of the current displayed first class day.
        /// </summary>
        private int displayedFirstClassDayIndex = -1;

        /// <summary>
        /// The report attendace list.
        /// </summary>
        private List<Attendance> attendances = null;

        /// <summary>
        /// ID for the next created attendance.
        /// </summary>
        private int nextAttendanceId = -1;

        /// <summary>
        /// DataTable for attendances.
        /// </summary>
        private DataTable dtAttendances = null;

        /// <summary>
        /// The list of attendance id columns.
        /// </summary>
        private List<DataGridViewTextBoxColumn> attendanceIdColumns = null;

        /// <summary>
        /// The list of attendance value columns.
        /// </summary>
        private List<DataGridViewComboBoxColumn> attendanceValueColumns = null;

        /// <summary>
        /// The list of attendance text columns.
        /// </summary>
        private List<DataGridViewTextBoxColumn> attendanceTextColumns = null;

        /// <summary>
        /// The list of attendance date headers
        /// </summary>
        private List<MetroLabel> attendanceDateHeaders = null;

        /// <summary>
        /// The list of attendance date day headers
        /// </summary>
        private List<MetroLabel> attendanceDateDayHeaders = null;

        /// <summary>
        /// The report grade list.
        /// </summary>
        private List<Grade> grades = null;

        /// <summary>
        /// ID for the next created grade.
        /// </summary>
        private int nextGradeId = -1;

        /// <summary>
        /// DataTable for grades.
        /// </summary>
        private DataTable dtGrades = null;

        /// <summary>
        /// The list of grade id columns.
        /// </summary>
        private List<DataGridViewTextBoxColumn> gradeIdColumns = null;

        /// <summary>
        /// The list of grade value columns.
        /// </summary>
        private List<DataGridViewComboBoxColumn> gradeValueColumns = null;

        /// <summary>
        /// The list of grade text columns.
        /// </summary>
        private List<DataGridViewTextBoxColumn> gradeTextColumns = null;

        /// <summary>
        /// The list of all questions.
        /// </summary>
        private List<Question> allQuestions = null;

        /// <summary>
        /// The list of question answers.
        /// </summary>
        private List<Answer> answers = null;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="report">
        /// The report to be displayed.
        /// </param>
        public EditReportTeacherMonth(Report report)
        {
            //set report
            this.report = report;

            //init UI components
            InitializeComponent();

            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set permission
            this.allowEditItem = Manager.HasLogonPermission("Report.Correct");

            //check if a teacher is editing his report
            if (Manager.LogonTeacher != null && 
                Manager.LogonTeacher.TeacherId == report.TeacherId)
            {
                //allow teacher to edit report
                this.allowEditItem = true;
            }

            //set font to labels
            lblSummaryRollCall.Font = MetroFramework.MetroFonts.DefaultLight(14);
            lblSummaryRollCallStatus.Font = MetroFramework.MetroFonts.DefaultLight(14);
            lblSummaryGrades.Font = MetroFramework.MetroFonts.DefaultLight(14);
            lblSummaryGradesStatus.Font = MetroFramework.MetroFonts.DefaultLight(14);
            lblSummaryQuestions.Font = MetroFramework.MetroFonts.DefaultLight(14);
            lblSummaryQuestionsStatus.Font = MetroFramework.MetroFonts.DefaultLight(14);
            lblSummaryGeneralQuestions.Font = MetroFramework.MetroFonts.DefaultLight(14);
            lblSummaryGeneralQuestionsStatus.Font = MetroFramework.MetroFonts.DefaultLight(14);

            //set grade headers
            mlblGrade0.Text = Properties.Resources.GradeSubject_Discipline;
            mlblGrade1.Text = Properties.Resources.GradeSubject_Performance;
            mlblGrade2.Text = Properties.Resources.GradeSubject_Dedication;

            //create lists
            this.classDays = new List<DateTime>();
            this.classStudents = new List<IdDescriptionStatus>();
            this.classRegistrations = new List<Registration>();
            this.attendances = new List<Attendance>();
            this.grades = new List<Grade>();
            this.allQuestions = new List<Question>();
            this.answers = new List<Answer>();

            //create attendance data table
            CreateAttendanceDataTable();

            //create grade data table
            CreateGradeDataTable();

            //get attendance id column list
            attendanceIdColumns = new List<DataGridViewTextBoxColumn>();
            attendanceIdColumns.Add(AttendanceId0);
            attendanceIdColumns.Add(AttendanceId1);
            attendanceIdColumns.Add(AttendanceId2);
            attendanceIdColumns.Add(AttendanceId3);
            attendanceIdColumns.Add(AttendanceId4);

            //get attendance value column list
            attendanceValueColumns = new List<DataGridViewComboBoxColumn>();
            attendanceValueColumns.Add(AttendanceValue0);
            attendanceValueColumns.Add(AttendanceValue1);
            attendanceValueColumns.Add(AttendanceValue2);
            attendanceValueColumns.Add(AttendanceValue3);
            attendanceValueColumns.Add(AttendanceValue4);

            //get attendance text column list
            attendanceTextColumns = new List<DataGridViewTextBoxColumn>();
            attendanceTextColumns.Add(AttendanceText0);
            attendanceTextColumns.Add(AttendanceText1);
            attendanceTextColumns.Add(AttendanceText2);
            attendanceTextColumns.Add(AttendanceText3);
            attendanceTextColumns.Add(AttendanceText4);

            //get attendace date label list
            attendanceDateHeaders = new List<MetroLabel>();
            attendanceDateHeaders.Add(mlblDate0);
            attendanceDateHeaders.Add(mlblDate1);
            attendanceDateHeaders.Add(mlblDate2);
            attendanceDateHeaders.Add(mlblDate3);
            attendanceDateHeaders.Add(mlblDate4);

            //get attendace date day label list
            attendanceDateDayHeaders = new List<MetroLabel>();
            attendanceDateDayHeaders.Add(mlblDateDay0);
            attendanceDateDayHeaders.Add(mlblDateDay1);
            attendanceDateDayHeaders.Add(mlblDateDay2);
            attendanceDateDayHeaders.Add(mlblDateDay3);
            attendanceDateDayHeaders.Add(mlblDateDay4);

            //get grade id column list
            gradeIdColumns = new List<DataGridViewTextBoxColumn>();
            gradeIdColumns.Add(GradeId0);
            gradeIdColumns.Add(GradeId1);
            gradeIdColumns.Add(GradeId2);

            //get grade value column list
            gradeValueColumns = new List<DataGridViewComboBoxColumn>();
            gradeValueColumns.Add(GradeValue0);
            gradeValueColumns.Add(GradeValue1);
            gradeValueColumns.Add(GradeValue2);

            //get grade text column list
            gradeTextColumns = new List<DataGridViewTextBoxColumn>();
            gradeTextColumns.Add(GradeText0);
            gradeTextColumns.Add(GradeText1);
            gradeTextColumns.Add(GradeText2);

            //avoid auto generated columns
            dgvAttendances.AutoGenerateColumns = false;
            dgvGrades.AutoGenerateColumns = false;

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

            AttendanceValue0.ValueMember = "Key";
            AttendanceValue0.DisplayMember = "Value";
            AttendanceValue0.DataSource = rollCalls;

            AttendanceValue1.ValueMember = "Key";
            AttendanceValue1.DisplayMember = "Value";
            AttendanceValue1.DataSource = rollCalls;

            AttendanceValue2.ValueMember = "Key";
            AttendanceValue2.DisplayMember = "Value";
            AttendanceValue2.DataSource = rollCalls;

            AttendanceValue3.ValueMember = "Key";
            AttendanceValue3.DisplayMember = "Value";
            AttendanceValue3.DataSource = rollCalls;

            AttendanceValue4.ValueMember = "Key";
            AttendanceValue4.DisplayMember = "Value";
            AttendanceValue4.DataSource = rollCalls;


            //list roll call values
            List<KeyValuePair<int, string>> grades = new List<KeyValuePair<int, string>>();
            grades.Add(new KeyValuePair<int, string>((int)GradeSpecialScore.Empty, " "));
            grades.Add(new KeyValuePair<int, string>(10, "10"));
            grades.Add(new KeyValuePair<int, string>(9, "9"));
            grades.Add(new KeyValuePair<int, string>(8, "8"));
            grades.Add(new KeyValuePair<int, string>(7, "7"));
            grades.Add(new KeyValuePair<int, string>(6, "6"));
            grades.Add(new KeyValuePair<int, string>(5, "5"));
            grades.Add(new KeyValuePair<int, string>(4, "4"));
            grades.Add(new KeyValuePair<int, string>(3, "3"));
            grades.Add(new KeyValuePair<int, string>(2, "2"));
            grades.Add(new KeyValuePair<int, string>(1, "1"));
            grades.Add(new KeyValuePair<int, string>(0, "0"));
            grades.Add(new KeyValuePair<int, string>(
                (int)GradeSpecialScore.Ungraded, Properties.Resources.ResourceManager.GetString("GradeSpecialScore_Ungraded")));

            GradeValue0.ValueMember = "Key";
            GradeValue0.DisplayMember = "Value";
            GradeValue0.DataSource = grades;

            GradeValue1.ValueMember = "Key";
            GradeValue1.DisplayMember = "Value";
            GradeValue1.DataSource = grades;

            GradeValue2.ValueMember = "Key";
            GradeValue2.DisplayMember = "Value";
            GradeValue2.DataSource = grades;

            //constructor is done
            isConstructed = true;
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get/set option to allow user to edit an item.
        /// True if user has permission to edit items.
        /// </summary>
        protected bool AllowEditItem
        {
            get
            {
                return allowEditItem;
            }

            set
            {
                allowEditItem = value;
            }
        }

        /// <summary>
        /// Get/set the type name of the displayed items.
        /// </summary>
        public string ItemName
        {
            get
            {
                return itemTypeName;
            }

            set
            {
                itemTypeName = value;
            }
        }

        /// <summary>
        /// Get/set the current control status.
        /// </summary>
        protected RegisterStatus Status
        {
            get { return status; }
            set
            {
                //check if status has changed
                if (status != value)
                {
                    //set status
                    status = value;

                    //refresh buttons
                    RefreshButtons();
                }
            }
        }

        /// <summary>
        /// Get/set the parent control that opened this control.
        /// </summary>
        public ISongControl ParentControl
        {
            get
            {
                return parentControl;
            }

            set
            {
                parentControl = value;
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
        }

        /// <summary>
        /// Dispose child Song control.
        /// </summary>
        public void DisposeChildControl()
        {
        }

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        /// <returns></returns>
        public string SelectMenuOption()
        {
            //select teacher
            return "Report";
        }

        #endregion ISong Methods


        #region Register Methods ******************************************************

        /// <summary>
        /// Load data for selected item.
        /// </summary>
        /// <param name="itemId">the id of the selected item.</param>
        private bool LoadItemData()
        {
            //check report
            if (report == null)
            {
                //no report is selected
                //could not load data
                return false;
            }

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
                //load report semester
                reportSemester = report.SemesterId == Manager.CurrentSemester.SemesterId ?
                    Manager.CurrentSemester : songChannel.FindSemester(report.SemesterId);

                //check result
                if (reportSemester.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting semester
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadSemester, 
                        reportSemester.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadSemester, 
                        reportSemester.ErrorMessage));

                    //could not load data
                    return false;
                }

                //get month name
                string monthName = Properties.Resources.ResourceManager.GetString(
                    "Month_" + report.ReferenceDate.Month);

                //display report data
                mlblReportData.Text = report.SemesterDescription + "." + report.ReferenceDate.Month +
                    " " + monthName + " | " + Manager.GetClassDescription(report.Class, false);

                //display teacher
                mlblTeacher.Text = report.TeacherName;

                //display report status
                mlblReportStatus.Text = Properties.Resources.ResourceManager.GetString(
                    "ReportStatus_" + ((ReportStatus)report.ReportStatus).ToString());

                //load roll call data
                #region roll calls

                //get list of class days for selected month
                classDays = report.Class.GetClassDays(report.ReferenceDate);

                //check result
                if (classDays.Count == 0)
                {
                    //no day for selected class
                    //reset displayed day index
                    displayedFirstClassDayIndex = -1;
                }
                else
                {
                    //set first displayed day index
                    displayedFirstClassDayIndex = 0;
                }
                
                //load list of registrations for selected class
                classRegistrations = songChannel.FindRegistrationsByClass(report.ClassId, -1);

                //check result
                if (classRegistrations[0].Result == (int)SelectResult.Empty)
                {
                    //no registration for selected class
                    //clear list
                    classRegistrations.Clear();
                }
                else if (classRegistrations[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting registrations
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Registration,
                        classRegistrations[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Registration,
                        classRegistrations[0].ErrorMessage));

                    //could not load report
                    return false;
                }

                //load list of students for selected class
                classStudents = songChannel.ListStudentsByClass(report.ClassId, -1);

                //check result
                if (classStudents[0].Result == (int)SelectResult.Success)
                {
                    //get report references
                    List<int> references = report.GetReferences();

                    //check reference list
                    if (references.Count > 0)
                    {
                        //student list was saved before
                        //use same list of students
                        List<IdDescriptionStatus> referencedStudents = new List<IdDescriptionStatus>();

                        //check each reference
                        foreach (int reference in references)
                        {
                            //get referenced student
                            IdDescriptionStatus referencedStudent = classStudents.Find(s => s.Id == reference);

                            //check result
                            if (referencedStudent != null)
                            {
                                //add student to list
                                referencedStudents.Add(referencedStudent);
                            }
                            else
                            {
                                //might happen
                                //registration was deleted after start of semester
                                //go to next student
                                continue;
                            }
                        }

                        //update list of class students
                        classStudents = referencedStudents;
                    }
                    else
                    {
                        //remove waiting list students
                        while (classStudents.Count > report.Class.Capacity)
                        {
                            //remove last student
                            classStudents.RemoveAt(classStudents.Count - 1);
                        }
                    }

                    //sort students by name
                    classStudents.Sort((a, b) => a.Description.CompareTo(b.Description));
                }
                else if (classStudents[0].Result == (int)SelectResult.Empty)
                {
                    //no student for selected class
                    //clear list
                    classStudents.Clear();
                }
                else if (classStudents[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting students
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Student,
                        classStudents[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Student,
                        classStudents[0].ErrorMessage));

                    //could not load report
                    return false;
                }

                //load attendances for selected class
                attendances = songChannel.FindAttendancesByFilter(
                    false, false, report.ClassId, -1);

                //check result
                if (attendances[0].Result == (int)SelectResult.Success)
                {
                    //sort attendances by date
                    attendances.Sort((a, b) => a.Date.CompareTo(b.Date));

                    //get attendances for reference month
                    List<Attendance> monthAttendances = attendances.FindAll(
                        a => a.Date.Month == report.ReferenceDate.Month);

                    //check result
                    if (monthAttendances == null)
                    {
                        //set empty list
                        monthAttendances = new List<Attendance>();
                    }

                    //set month attendances as loaded attendances
                    attendances = monthAttendances;
                }
                else if (attendances[0].Result == (int)SelectResult.Empty)
                {
                    //no attendance for selected class
                    //clear list
                    attendances.Clear();
                }
                else if (attendances[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting attendances
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Attendance, attendances[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Attendance, attendances[0].ErrorMessage));
                    
                    //could not load report
                    return false;
                }

                //display attendances
                DisplayAttendances();

                //enable attendace fields
                EnableAttendanceFields();

                //check number of class days
                if (classDays.Count == 0)
                {
                    //display no class message
                    mlblNoClass.Text = Properties.Resources.msgNoClass;
                    mlblNoClass.Visible = true;
                }

                //get attendances that are not empty
                List<Attendance> setAttendances = attendances.FindAll(
                    a => a.RollCall != (int)RollCall.Empty);

                //check result
                if (setAttendances == null)
                {
                    //create empty list
                    setAttendances = new List<Attendance>();
                }

                //calculate complete percentage
                double percentage = (classStudents.Count * classDays.Count == 0) ? 100.0 :
                    ((double)setAttendances.Count)/((double)(classStudents.Count * classDays.Count)) * 100.0;

                //set percentage to attendance status
                lblSummaryRollCallStatus.Text = ((int)percentage).ToString() + "%";

                //set color to attendance panel according to percentage
                pnSummaryRollCall.BackColor = ((int)percentage) >= 100 ?
                    Color.FromArgb(0, 116, 162) : Color.FromArgb(150, 150, 150);

                #endregion roll calls

                //load grade data
                #region grades

                //load grades for selected class
                grades = songChannel.FindGradesByFilter(
                    false, false, false, false, false, false,
                    (int)GradeRapporteur.Teacher, (int)GradeTarget.Student, 
                    (int)GradePeriodicity.Month, -1, report.SemesterId, report.ReferenceDate,
                    -1, -1, report.TeacherId, -1, -1, report.ClassId);

                //check result
                if (grades[0].Result == (int)SelectResult.Empty)
                {
                    //no grade for selected class
                    //clear list
                    grades.Clear();
                }
                else if (grades[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting grades
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Grade, grades[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Grade, grades[0].ErrorMessage));

                    //could not load report
                    return false;
                }

                //display grades
                DisplayGrades();

                //enable grade fields
                EnableGradeFields();

                //get grades that are not empty
                List<Grade> setGrades = grades.FindAll(
                    a => a.Score != (int)GradeSpecialScore.Empty);

                //check result
                if (setGrades == null)
                {
                    //create empty list
                    setGrades = new List<Grade>();
                }

                //calculate complete percentage
                percentage = (classStudents.Count == 0) ? 100.0 :
                    ((double)setGrades.Count) / ((double)(classStudents.Count * gradeIdColumns.Count)) * 100.0;

                //set percentage to grade status
                lblSummaryGradesStatus.Text = ((int)percentage).ToString() + "%";

                //set color to grade panel according to percentage
                pnSummaryGrades.BackColor = ((int)percentage) >= 100 ?
                    Color.FromArgb(0, 116, 162) : Color.FromArgb(150, 150, 150);

                #endregion grades

                //load questions
                #region questions

                //get list of all questions from database
                allQuestions = songChannel.FindQuestions();

                //check result
                if (allQuestions[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting questions
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Question, allQuestions[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Question, allQuestions[0].ErrorMessage));

                    //could not load report
                    return false;
                }

                //get list of answers from database
                answers = songChannel.FindAnswersByFilter(
                    (int)QuestionRapporteur.Teacher, -1, (int)QuestionPeriodicity.Month, -1,
                    -1, report.SemesterId, report.ReferenceDate,
                    report.InstitutionId, report.TeacherId, -1, -1);

                //check result
                if (answers[0].Result == (int)SelectResult.Empty)
                {
                    //no answer for selected report
                    //clear list
                    answers.Clear();
                }
                else if (answers[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting answers
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Answer, answers[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Answer, answers[0].ErrorMessage));

                    //could not load report
                    return false;
                }
                
                //remove all answers that belong to another report
                answers.RemoveAll(a => a.ReportId > 0 && a.ReportId != report.ReportId);

                //calculate complete percentage before displaying questions
                //answers will be created
                //find answers for this report
                List <Answer> reportAnswers = answers.FindAll(a => a.ReportId == report.ReportId);

                //check result
                if (reportAnswers == null)
                {
                    //create empty list
                    reportAnswers = new List<Answer>();
                }

                //calculate complete percentage
                percentage = ((double)reportAnswers.Count) / 3.0 * 100.0;

                //set percentage to grade status
                lblSummaryQuestionsStatus.Text = ((int)percentage).ToString() + "%";

                //set color to question panel according to percentage
                pnSummaryQuestions.BackColor = ((int)percentage) >= 100 ?
                    Color.FromArgb(0, 116, 162) : Color.FromArgb(150, 150, 150);

                //find answers for general questions
                List<Answer> generalAnswers = answers.FindAll(
                    a => a.ReportId == int.MinValue && a.ClassId == int.MinValue);

                //check result
                if (generalAnswers == null)
                {
                    //create empty list
                    generalAnswers = new List<Answer>();
                }

                //calculate complete percentage
                percentage = ((double)generalAnswers.Count) / 5.0 * 100.0;

                //set percentage to grade status
                lblSummaryGeneralQuestionsStatus.Text = ((int)percentage).ToString() + "%";

                //set color to general question panel according to percentage
                pnSummaryGeneralQuestions.BackColor = ((int)percentage) >= 100 ?
                    Color.FromArgb(0, 116, 162) : Color.FromArgb(150, 150, 150);

                //display questions
                DisplayQuestions();

                //enable question fields
                EnableQuestionFields();

                #endregion questions
            }
            catch (Exception ex)
            {
                //show error message
                MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelLoadData, itemTypeDescription, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log exception
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelLoadData, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load data
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

            //data was loaded
            return true;
        }

        /// <summary>
        /// Start editing current selected item.
        /// </summary>
        public void EditItem()
        {
            //check if summary tab is selected
            if (mtbTabManager.SelectedTab.Equals(tbSummary))
            {
                //check which tab needs edition
                if (!lblSummaryRollCallStatus.Text.Equals("100%"))
                {
                    //select roll calls tab
                    mtbTabManager.SelectedTab = tbRollCall;
                }
                else if (!lblSummaryGradesStatus.Text.Equals("100%"))
                {
                    //select grades tab
                    mtbTabManager.SelectedTab = tbGrades;
                }
                else if (!lblSummaryQuestionsStatus.Text.Equals("100%"))
                {
                    //select questions tab
                    mtbTabManager.SelectedTab = tbQuestions;
                }
                else if (!lblSummaryGeneralQuestionsStatus.Text.Equals("100%"))
                {
                    //select general questions tab
                    mtbTabManager.SelectedTab = tbGeneralQuestions;
                }
            }
        }

        /// <summary>
        /// Save the data of the current edited item.
        /// </summary>
        /// <returns>
        /// The updated description of the saved item.
        /// Null if item could not be saved.
        /// </returns>
        public bool SaveItem()
        {
            //validate field controls and check result
            if (!this.ValidateData())
            {
                //data is not valid
                //cannot save institution
                return false;
            }

            //check if report is completed
            //get attendances that are not empty
            List<Attendance> setAttendances = attendances.FindAll(
                a => a.RollCall != (int)RollCall.Empty);

            //get grades that are not empty
            List<Grade> setGrades = grades.FindAll(
                a => a.Score != (int)GradeSpecialScore.Empty);

            //check number of attendances and grades
            if (setGrades.Count == classStudents.Count * gradeIdColumns.Count &&
                setAttendances.Count == classStudents.Count * classDays.Count)
            {
                //report is completed
                report.ReportStatus = (int)ReportStatus.Completed;

                //check if report has a reference list
                if (report.ReferenceList == null ||
                    report.ReferenceList.Length == 0)
                {
                    //create reference list with class student ids
                    List<int> referenceList = new List<int>();

                    //check each student in the list of students
                    foreach (IdDescriptionStatus student in classStudents)
                    {
                        //check if id is not in the list yet
                        if (!referenceList.Contains(student.Id))
                        {
                            //add student id
                            referenceList.Add(student.Id);
                        }
                    }

                    //set reference list to report
                    report.SetReferenceList(referenceList);
                }
            }
            else
            {
                //report is still pending
                report.ReportStatus = (int)ReportStatus.Pending;
            }

            //gather list of updated attendances for saving
            List<Attendance> saveAttendances = attendances.FindAll(a => a.Updated);
            
            //gather list of updated grades for saving
            List<Grade> saveGrades = grades.FindAll(a => a.Updated);

            //gather list of updated answers for saving
            List<Answer> saveAnswers = answers.FindAll(a => a.Updated);

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not save report
                return false;
            }

            try
            {
                //save report and get result
                SaveResult saveResult = songChannel.SaveReport(
                    report, saveAttendances, saveGrades, saveAnswers);

                //check result
                if (saveResult.Result == (int)SelectResult.FatalError)
                {
                    //report was not saved
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

                    //could not save report
                    return false;
                }

                //display summary tab
                mtbTabManager.SelectedTab = tbSummary;

                //check if there is a parent control
                //check if it is a view report control
                if (parentControl != null && parentControl is ViewReportControl)
                {
                    //update report in parent control
                    ((ViewReportControl)parentControl).UpdateReport(report);
                }
                else if (parentControl != null && parentControl is HomeControl)
                {
                    //update report in parent control
                    ((HomeControl)parentControl).UpdateNoticeReport(report);
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

            //report was saved
            return true;
        }

        /// <summary>
        /// Cancel changes from current edited item.
        /// </summary>
        public void CancelChanges()
        {
            //select summary tab
            mtbTabManager.SelectedTab = tbSummary;
        }

        /// <summary>
        /// Clear value for all UI fields.
        /// </summary>
        public void ClearFields()
        {
            //clear report Data
            mlblReportData.Text = string.Empty;

            //clear status
            mlblReportStatus.Text = string.Empty;

            //clear teacher
            mlblTeacher.Text = string.Empty;

            //clear summary statuses
            lblSummaryRollCallStatus.Text = string.Empty;
            lblSummaryGradesStatus.Text = string.Empty;
            lblSummaryQuestionsStatus.Text = string.Empty;
            lblSummaryGeneralQuestionsStatus.Text = string.Empty;

            //clear roll call data
            //clear class days
            classDays = new List<DateTime>();

            //reset displayed day index
            displayedFirstClassDayIndex = -1;
        }

        /// <summary>
        /// Enable all UI fields for edition.
        /// </summary>
        /// <param name="enable">True to enable fields. False to disable them.</param>
        public void EnableFields(bool enable)
        {
            //enable attendance fields
            EnableAttendanceFields();

            //enable grade fields
            EnableGradeFields();

            //enable question fields
            EnableQuestionFields();
        }

        #endregion Register Methods


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
        /// Clear grade data table.
        /// </summary>
        private void ClearGradeDataTable()
        {
            //lock datatable of grades
            lock (dtGrades)
            {
                //clear datatable
                dtGrades.Clear();
            }
        }

        /// <summary>
        /// Create attendance data table.
        /// </summary>
        private void CreateAttendanceDataTable()
        {
            //create data table
            dtAttendances = new DataTable();

            //StudentId
            DataColumn dcStudentId = new DataColumn("StudentId", typeof(int));
            dtAttendances.Columns.Add(dcStudentId);

            //StudentName
            DataColumn dcStudentName = new DataColumn("StudentName", typeof(string));
            dtAttendances.Columns.Add(dcStudentName);

            //RegistrationStatusValue
            DataColumn dcRegistrationStatusValue = new DataColumn("RegistrationStatusValue", typeof(int));
            dtAttendances.Columns.Add(dcRegistrationStatusValue);

            //AttendanceId0
            DataColumn dcAttendanceId0 = new DataColumn("AttendanceId0", typeof(int));
            dtAttendances.Columns.Add(dcAttendanceId0);

            //AttendanceId1
            DataColumn dcAttendanceId1 = new DataColumn("AttendanceId1", typeof(int));
            dtAttendances.Columns.Add(dcAttendanceId1);

            //AttendanceId2
            DataColumn dcAttendanceId2 = new DataColumn("AttendanceId2", typeof(int));
            dtAttendances.Columns.Add(dcAttendanceId2);

            //AttendanceId3
            DataColumn dcAttendanceId3 = new DataColumn("AttendanceId3", typeof(int));
            dtAttendances.Columns.Add(dcAttendanceId3);

            //AttendanceId4
            DataColumn dcAttendanceId4 = new DataColumn("AttendanceId4", typeof(int));
            dtAttendances.Columns.Add(dcAttendanceId4);

            //AttendanceValue0
            DataColumn dcAttendanceValue0 = new DataColumn("AttendanceValue0", typeof(int));
            dtAttendances.Columns.Add(dcAttendanceValue0);

            //AttendanceValue1
            DataColumn dcAttendanceValue1 = new DataColumn("AttendanceValue1", typeof(int));
            dtAttendances.Columns.Add(dcAttendanceValue1);

            //AttendanceValue2
            DataColumn dcAttendanceValue2 = new DataColumn("AttendanceValue2", typeof(int));
            dtAttendances.Columns.Add(dcAttendanceValue2);

            //AttendanceValue3
            DataColumn dcAttendanceValue3 = new DataColumn("AttendanceValue3", typeof(int));
            dtAttendances.Columns.Add(dcAttendanceValue3);

            //AttendanceValue4
            DataColumn dcAttendanceValue4 = new DataColumn("AttendanceValue4", typeof(int));
            dtAttendances.Columns.Add(dcAttendanceValue4);

            //AttendanceText0
            DataColumn dcAttendanceText0 = new DataColumn("AttendanceText0", typeof(string));
            dtAttendances.Columns.Add(dcAttendanceText0);

            //AttendanceText1
            DataColumn dcAttendanceText1 = new DataColumn("AttendanceText1", typeof(string));
            dtAttendances.Columns.Add(dcAttendanceText1);

            //AttendanceText2
            DataColumn dcAttendanceText2 = new DataColumn("AttendanceText2", typeof(string));
            dtAttendances.Columns.Add(dcAttendanceText2);

            //AttendanceText3
            DataColumn dcAttendanceText3 = new DataColumn("AttendanceText3", typeof(string));
            dtAttendances.Columns.Add(dcAttendanceText3);

            //AttendanceText4
            DataColumn dcAttendanceText4 = new DataColumn("AttendanceText4", typeof(string));
            dtAttendances.Columns.Add(dcAttendanceText4);
        }

        /// <summary>
        /// Create grade data table.
        /// </summary>
        private void CreateGradeDataTable()
        {
            //create data table
            dtGrades = new DataTable();

            //StudentId
            DataColumn dcStudentId = new DataColumn("StudentId", typeof(int));
            dtGrades.Columns.Add(dcStudentId);

            //StudentName
            DataColumn dcStudentName = new DataColumn("StudentName", typeof(string));
            dtGrades.Columns.Add(dcStudentName);

            //RegistrationStatusValue
            DataColumn dcRegistrationStatusValue = new DataColumn("RegistrationStatusValue", typeof(int));
            dtGrades.Columns.Add(dcRegistrationStatusValue);

            //GradeId0
            DataColumn dcGradeId0 = new DataColumn("GradeId0", typeof(int));
            dtGrades.Columns.Add(dcGradeId0);

            //GradeId1
            DataColumn dcGradeId1 = new DataColumn("GradeId1", typeof(int));
            dtGrades.Columns.Add(dcGradeId1);

            //GradeId2
            DataColumn dcGradeId2 = new DataColumn("GradeId2", typeof(int));
            dtGrades.Columns.Add(dcGradeId2);

            //GradeValue0
            DataColumn dcGradeValue0 = new DataColumn("GradeValue0", typeof(int));
            dtGrades.Columns.Add(dcGradeValue0);

            //GradeValue1
            DataColumn dcGradeValue1 = new DataColumn("GradeValue1", typeof(int));
            dtGrades.Columns.Add(dcGradeValue1);

            //GradeValue2
            DataColumn dcGradeValue2 = new DataColumn("GradeValue2", typeof(int));
            dtGrades.Columns.Add(dcGradeValue2);

            //GradeText0
            DataColumn dcGradeText0 = new DataColumn("GradeText0", typeof(string));
            dtGrades.Columns.Add(dcGradeText0);

            //GradeText1
            DataColumn dcGradeText1 = new DataColumn("GradeText1", typeof(string));
            dtGrades.Columns.Add(dcGradeText1);

            //GradeText2
            DataColumn dcGradeText2 = new DataColumn("GradeText2", typeof(string));
            dtGrades.Columns.Add(dcGradeText2);
        }

        /// <summary>
        /// Display attendances for current selected class days.
        /// </summary>
        private void DisplayAttendances()
        {
            //clear attendance datatable
            ClearAttendanceDataTable();

            //check each class student
            foreach (IdDescriptionStatus student in classStudents)
            {
                //must gather displayed day attendances for current student
                Attendance[] studentAttendances = null;

                //check if there is no date to be displayed
                if (displayedFirstClassDayIndex > -1)
                {
                    //create array
                    studentAttendances = new Attendance[5];

                    //check each displayed class day
                    for (int i = 0; i < 5; i++)
                    {
                        //get displayed class day
                        int displayedClassDayIndex = displayedFirstClassDayIndex + i;

                        //check if there is displayed day
                        if (displayedClassDayIndex >= classDays.Count)
                        {
                            //end of class days was reached
                            //no attendance for displayed day
                            studentAttendances[i] = null;

                            //go to next day
                            continue;
                        }

                        //find attendances for current displayed day and student
                        Attendance attendance = attendances.Find(
                            a => a.Date.Equals(classDays[displayedClassDayIndex]) && 
                                 a.StudentId == student.Id);

                        //check result
                        if (attendance == null)
                        {
                            //no attendance was set to student
                            //create and set attendance
                            attendance = new Attendance();
                            attendance.AttendanceId = nextAttendanceId--;
                            attendance.ClassId = report.ClassId;
                            attendance.ClassDay = -1;
                            attendance.Date = classDays[displayedClassDayIndex];
                            attendance.RollCall = (int)RollCall.Empty;
                            attendance.StudentId = student.Id;
                            attendance.StudentName = student.Description;
                            attendance.TeacherId = report.TeacherId;

                            //add attendance to main list
                            attendances.Add(attendance);
                        }
                        else
                        {
                            //set student name since it is not loaded from database
                            attendance.StudentName = student.Description;
                        }
                        
                        //check if attendance roll call is empty
                        if (attendance.RollCall == (int)RollCall.Empty)
                        {
                            //check if student is evaded from class
                            //get registration
                            Registration registration = classRegistrations.Find(
                                r => r.StudentId == student.Id);

                            //check result
                            if (registration != null &&
                                registration.RegistrationStatus == (int)ItemStatus.Evaded)
                            {
                                //set roll call to evaded
                                attendance.RollCall = (int)RollCall.Evaded;
                                attendance.Updated = true;
                            }
                        }

                        //set displayed day attendance
                        studentAttendances[i] = attendance;
                    }
                }

                //create, set and add loan row
                DataRow dr = dtAttendances.NewRow();                
                SetAttendanceDataRow(dr, student, studentAttendances);
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
        }

        /// <summary>
        /// Display grades for class students.
        /// </summary>
        private void DisplayGrades()
        {
            //clear grade datatable
            ClearGradeDataTable();

            //check each class student
            foreach (IdDescriptionStatus student in classStudents)
            {
                //must gather displayed grades for current student
                Grade[] studentGrades = new Grade[gradeIdColumns.Count];
                studentGrades[0] = GetOrCreateGrade(student, GradeSubject.Discipline);
                studentGrades[1] = GetOrCreateGrade(student, GradeSubject.Performance);
                studentGrades[2] = GetOrCreateGrade(student, GradeSubject.Dedication);

                //create, set and add loan row
                DataRow dr = dtGrades.NewRow();
                SetGradeDataRow(dr, student, studentGrades);
                dtGrades.Rows.Add(dr);
            }

            //check if datagrid has not a source yet
            if (dgvGrades.DataSource == null)
            {
                //set source to datagrid
                dgvGrades.DataSource = dtGrades;
            }

            //refresh grid
            dgvGrades.Refresh();

            //clear default selection
            dgvGrades.ClearSelection();
        }

        /// <summary>
        /// Display questions.
        /// </summary>
        private void DisplayQuestions()
        {
            //display questions
            DisplayQuestion(qcQuestion1, allQuestions.Find(q => q.QuestionId == 1));
            DisplayQuestion(qcQuestion2, allQuestions.Find(q => q.QuestionId == 2));
            DisplayQuestion(qcQuestion3, allQuestions.Find(q => q.QuestionId == 3));

            //display general questions
            DisplayQuestion(qcGeneralQuestion1, allQuestions.Find(q => q.QuestionId == 4));
            DisplayQuestion(qcGeneralQuestion2, allQuestions.Find(q => q.QuestionId == 5));
            DisplayQuestion(qcGeneralQuestion3, allQuestions.Find(q => q.QuestionId == 6));
            DisplayQuestion(qcGeneralQuestion4, allQuestions.Find(q => q.QuestionId == 7));
            DisplayQuestion(qcGeneralQuestion5, allQuestions.Find(q => q.QuestionId == 8));
        }

        /// <summary>
        /// Display a question and its answer in the selected question control.
        /// Create an answer if there was no previous answer to the question.
        /// </summary>
        /// <param name="questionControl">
        /// The selected question control.
        /// </param>
        /// <param name="question">
        /// The question to be displayed.
        /// </param>
        private void DisplayQuestion(Control questionControl, Question question)
        {
            //check question
            if (question == null)
            {
                //should never happen
                //exit
                return;
            }

            //get answer for selected question
            Answer answer = answers.Find(a => a.QuestionId == question.QuestionId);

            //check answer
            if (answer == null)
            {
                //must create an answer
                //create and set answer
                answer = new Answer();
                answer.AnswerId = -1;

                //set question data
                answer.QuestionId = question.QuestionId;
                answer.AnswerMetric = question.QuestionMetric;
                answer.AnswerPeriodicity = question.QuestionPeriodicity;
                answer.AnswerRapporteur = question.QuestionRapporteur;
                answer.AnswerTarget = question.QuestionTarget;

                //set report data
                answer.ReportId = report.ReportId;
                answer.SemesterId = report.SemesterId;
                answer.ClassId = report.ClassId;
                answer.CoordinatorId = report.CoordinatorId;
                answer.InstitutionId = report.InstitutionId;
                answer.TeacherId = report.TeacherId;
                answer.ReferenceDate = report.ReferenceDate;

                //set answer data
                answer.Comments = string.Empty;
                answer.Score = int.MinValue;

                //check if question is a general question
                if (question.QuestionTarget == (int)QuestionTarget.Institution ||
                    question.QuestionTarget == (int)QuestionTarget.Self)
                {
                    //general question
                    //set general answer
                    answer.ReportId = int.MinValue;
                    answer.ClassId = int.MinValue;
                }

                //add created answer to list of answers
                answers.Add(answer);
            }

            //get question number from question control tag
            int questionNumber = -1;

            //check question control
            if (questionControl.Tag != null)
            {
                try
                {
                    //parse question number
                    questionNumber = int.Parse(questionControl.Tag.ToString());
                }
                catch
                {
                    //do nothing
                }
            }

            //check question control type
            if (questionControl is QuestionControl)
            {
                //load question data
                ((QuestionControl)questionControl).LoadData(
                    question, questionNumber, answer);
            }
            else if (questionControl is QuestionCommentControl)
            {
                //load question data
                ((QuestionCommentControl)questionControl).LoadData(
                    question, questionNumber, answer);
            }
        }

        /// <summary>
        /// Enable attendance fields according to current context.
        /// </summary>
        private void EnableAttendanceFields()
        {
            //display day fields
            mtlPreviousDay.Visible = (classDays.Count > 0 && displayedFirstClassDayIndex > 0);
            mtlNextDay.Visible = (classDays.Count > 0 && displayedFirstClassDayIndex < classDays.Count - 5);

            //enable each attendance column
            for (int i = 0; i < 5; i++)
            {
                //get displayed class day
                int displayedClassDayIndex = (displayedFirstClassDayIndex > -1) ?
                    (displayedFirstClassDayIndex + i) : -1;

                //check displayed class day
                if (displayedFirstClassDayIndex == -1 ||
                    displayedClassDayIndex >= classDays.Count)
                {
                    //no day is being displayed
                    //hide columns
                    attendanceValueColumns[i].Visible = false;
                    attendanceTextColumns[i].Visible = false;
                    
                    //hide headers
                    attendanceDateHeaders[i].Visible = false;
                    attendanceDateDayHeaders[i].Visible = false;

                    //go to next column
                    continue;
                }

                //check if report is being consulted 
                //or if displayed class day is later than today
                if (this.Status == RegisterStatus.Consulting ||
                    classDays.Count == 0 || displayedClassDayIndex < 0 )//||
                    //classDays[displayedClassDayIndex] > DateTime.Today)
                {
                    //display roll call text column
                    attendanceValueColumns[i].Visible = false;
                    attendanceTextColumns[i].Visible = true;
                }
                else
                {
                    //display roll call value column
                    attendanceValueColumns[i].Visible = true;
                    attendanceTextColumns[i].Visible = false;
                }

                //show header
                attendanceDateHeaders[i].Visible = true;
                attendanceDateDayHeaders[i].Visible = true;

                //set header
                SetAttendanceHeader(classDays[displayedClassDayIndex],
                    attendanceDateHeaders[i], attendanceDateDayHeaders[i]);
            }

            //enable header scroll shift if number of students is higher than 8
            mlblDateScroll.Visible = classStudents.Count > 8;
            mlblDateDayScroll.Visible = classStudents.Count > 8;
        }

        /// <summary>
        /// Enable grade fields according to current context.
        /// </summary>
        private void EnableGradeFields()
        {
            //enable each grade column
            for (int i = 0; i < gradeValueColumns.Count; i++)
            {
                //switch roll call value and text columns
                gradeValueColumns[i].Visible = !(this.Status == RegisterStatus.Consulting);
                gradeTextColumns[i].Visible = (this.Status == RegisterStatus.Consulting);
            }

            //enable header scroll shift if number of students is higher than 8
            mlblGradeScroll.Visible = classStudents.Count > 8;
        }

        /// <summary>
        /// Enable question fields according to current context.
        /// </summary>
        private void EnableQuestionFields()
        {
            //enable questions
            qcQuestion1.EnableFields(!(this.Status == RegisterStatus.Consulting));
            qcQuestion2.EnableFields(!(this.Status == RegisterStatus.Consulting));
            qcQuestion3.EnableFields(!(this.Status == RegisterStatus.Consulting));

            //enable general questions
            qcGeneralQuestion1.EnableFields(!(this.Status == RegisterStatus.Consulting));
            qcGeneralQuestion2.EnableFields(!(this.Status == RegisterStatus.Consulting));
            qcGeneralQuestion3.EnableFields(!(this.Status == RegisterStatus.Consulting));
            qcGeneralQuestion4.EnableFields(!(this.Status == RegisterStatus.Consulting));
            qcGeneralQuestion5.EnableFields(!(this.Status == RegisterStatus.Consulting));
        }

        /// <summary>
        /// Get grade form list of grades for selected student and subject.
        /// Create and add grade if it is not in the list.
        /// </summary>
        /// <param name="student">
        /// The selected student.
        /// </param>
        /// <param name="subject">
        /// The selected grade subject.
        /// </param>
        /// <returns>
        /// The grade.
        /// </returns>
        private Grade GetOrCreateGrade(IdDescriptionStatus student, GradeSubject subject)
        {
            //get subject grade for selected student
            Grade grade = grades.Find(
                g => g.StudentId == student.Id && g.GradeSubject == (int)subject);

            //check result
            if (grade == null)
            {
                //no grade was set to student
                //create and set grade
                grade = new Grade();
                grade.GradeId = nextGradeId--;
                grade.ClassId = report.ClassId;
                grade.CoordinatorId = int.MinValue;
                grade.GradePeriodicity = (int)GradePeriodicity.Month;
                grade.GradeRapporteur = (int)GradeRapporteur.Teacher;
                grade.GradeSubject = (int)subject;
                grade.GradeTarget = (int)GradeTarget.Student;
                grade.InstitutionId = report.InstitutionId;
                grade.ReferenceDate = report.ReferenceDate;
                grade.Score = (int)GradeSpecialScore.Empty;
                grade.SemesterId = report.SemesterId;
                grade.StudentId = student.Id;
                grade.StudentName = student.Description;
                grade.TeacherId = report.TeacherId;

                //add grade to main list
                grades.Add(grade);
            }
            else
            {
                //set student name since it is not loaded from database
                grade.StudentName = student.Description;
            }

            //check if grade is empty
            if (grade.Score == (int)GradeSpecialScore.Empty)
            {
                //check if student is evaded from class
                //get registration
                Registration registration = classRegistrations.Find(
                    r => r.StudentId == student.Id);

                //check result
                if (registration != null &&
                    registration.RegistrationStatus == (int)ItemStatus.Evaded)
                {
                    //set grade to ungraded
                    grade.Score = (int)GradeSpecialScore.Ungraded;
                    grade.Updated = true;
                }
            }

            //return found or created grade
            return grade;
        }

        /// <summary>
        /// Perform a data load for the selected item.
        /// </summary>
        /// <param name="itemId">
        /// THe selected item ID.
        /// </param>
        private void PerformDataLoad()
        {

            try
            {
                //set cursor to wait
                this.Cursor = Cursors.WaitCursor;

                //clear current fields
                ClearFields();

                //load data for the selected item and check result
                if (!LoadItemData())
                {
                    //could not load data
                    //clear current fields
                    ClearFields();

                    //disable edition
                    allowEditItem = false;
                }
            }
            finally
            {
                //reset cursor
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Refresh buttons by settings their state according to 
        /// current control status.
        /// </summary>
        private void RefreshButtons()
        {
            //enable cancel button while editing
            btCancel.Enabled = ((Status == RegisterStatus.Editing) ||
                             (Status == RegisterStatus.Creating));

            //disable save button while consulting
            btSave.Enabled = ((Status == RegisterStatus.Editing) ||
                             (Status == RegisterStatus.Creating));
            
            //enable edit button
            btEdit.Enabled = (Status == RegisterStatus.Consulting) && allowEditItem;
        }

        /// <summary>
        /// Reload report from database.
        /// </summary>
        /// <returns></returns>
        private bool ReloadReportFromDatabase()
        {
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not reload report
                return false;
            }

            try
            {
                //reload report from databse
                Report reloadedReport = songChannel.FindReport(report.ReportId);

                //check result
                if (reloadedReport.Result == (int)SelectResult.FatalError)
                {
                    //database error while reloading report students
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        Properties.Resources.item_Report, classStudents[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        Properties.Resources.item_Report, classStudents[0].ErrorMessage));

                    //could not reload report
                    return false;
                }

                //set extra data to reloaded report
                reloadedReport.Class = report.Class;
                reloadedReport.CoordinatorName = report.CoordinatorName;
                reloadedReport.InstitutionName = report.InstitutionName;
                reloadedReport.SemesterDescription = report.SemesterDescription;
                reloadedReport.TeacherName = report.TeacherName;
            }
            catch (Exception ex)
            {
                //show error message
                MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelLoadData, itemTypeDescription, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log exception
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelLoadData, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not reload report
                return false;
            }

            //report was reloaded
            return true;
        }

        /// <summary>
        /// Set data row with selected student and attendances data.
        /// </summary>
        /// <param name="dataRow">The data row to be set.</param>
        /// <param name="student">The selected student.</param>
        /// <param name="attendances">The selected attendances.</param>
        private void SetAttendanceDataRow(
            DataRow dataRow, IdDescriptionStatus student, Attendance[] attendances)
        {
            //set base data
            dataRow["StudentId"] = student.Id;
            dataRow["StudentName"] = student.Description;

            //get student class registration
            Registration registration = classRegistrations.Find(
                r => r.StudentId == student.Id);

            //check each attendance in the array
            for (int i = 0; i < 5; i++)
            {
                //get attendance
                Attendance attendance = attendances != null ? attendances[i] : null;

                //check attendance
                if (attendance != null)
                {
                    //set attendance data
                    dataRow["AttendanceId" + i] = attendance.AttendanceId;
                    dataRow["AttendanceValue" + i] = attendance.RollCall;
                    dataRow["AttendanceText" + i] = attendance.RollCall == (int)RollCall.Empty ? " " :
                        Properties.Resources.ResourceManager.GetString(
                            "RollCall_" + ((RollCall)attendance.RollCall).ToString());
                }
                else
                {
                    //set empty data
                    dataRow["AttendanceId" + i] = int.MinValue;
                    dataRow["AttendanceValue" + i] = RollCall.Empty;
                    dataRow["AttendanceText" + i] = string.Empty;
                }

                //set registration status
                dataRow["RegistrationStatusValue"] = registration != null ?
                    registration.RegistrationStatus : (int)ItemStatus.Active;
            }
        }

        /// <summary>
        /// Set date to selected attendance header.
        /// </summary>
        /// <param name="date">
        /// The date to be displayed.
        /// </param>
        /// <param name="dateHeader">
        /// The selected attendance date header.
        /// </param>
        /// <param name="dateDayHeader">
        /// The selected attendance date day header.
        /// </param>
        private void SetAttendanceHeader(
            DateTime date, MetroLabel dateHeader, MetroLabel dateDayHeader)
        {
            //set date
            dateHeader.Text = date.ToShortDateString();

            //remove year
            dateHeader.Text = dateHeader.Text.Substring(0, dateHeader.Text.Length - 5);

            //set date day
            dateDayHeader.Text = Properties.Resources.ResourceManager.GetString(
                "day" + date.DayOfWeek.ToString());
        }

        /// <summary>
        /// Set data row with selected student and grades data.
        /// </summary>
        /// <param name="dataRow">The data row to be set.</param>
        /// <param name="student">The selected student.</param>
        /// <param name="grades">The selected grades.</param>
        private void SetGradeDataRow(
            DataRow dataRow, IdDescriptionStatus student, Grade[] grades)
        {
            //set base data
            dataRow["StudentId"] = student.Id;
            dataRow["StudentName"] = student.Description;

            //get student class registration
            Registration registration = classRegistrations.Find(
                r => r.StudentId == student.Id);

            //check each grade in the array
            for (int i = 0; i < gradeIdColumns.Count; i++)
            {
                //get grade
                Grade grade = grades != null ? grades[i] : null;

                //check grade
                if (grade != null)
                {
                    //set grade data
                    dataRow["GradeId" + i] = grade.GradeId;
                    dataRow["GradeValue" + i] = grade.Score;

                    //set score
                    if (grade.Score >= 0)
                    {
                        dataRow["GradeText" + i] = grade.Score.ToString();
                    }
                    else
                    {
                        dataRow["GradeText" + i] = grade.Score == (int)GradeSpecialScore.Empty ? " " :
                            Properties.Resources.ResourceManager.GetString(
                                "GradeSpecialScore_" + ((GradeSpecialScore)grade.Score).ToString());
                    }
                }
                else
                {
                    //set empty data
                    dataRow["GradeId" + i] = int.MinValue;
                    dataRow["GradeValue" + i] = (int)GradeSpecialScore.Empty;
                    dataRow["GradeText" + i] = string.Empty;
                }

                //set registration status
                dataRow["RegistrationStatusValue"] = registration != null ?
                    registration.RegistrationStatus : (int)ItemStatus.Active;
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
            //validate question answers
            //validate question 1 answer
            if (!ValidateAnswerScore(qcQuestion1, tbQuestions))
            {
                //invalid field
                return false;
            }

            //validate question 2 answer
            if (!ValidateAnswerScore(qcQuestion2, tbQuestions))
            {
                //invalid field
                return false;
            }

            //validate question 3 answer
            if (!ValidateAnswerComments(qcQuestion3, tbQuestions))
            {
                //invalid field
                return false;
            }

            //validate general question answers
            //validate general question 1 answer
            if (!ValidateAnswerScore(qcGeneralQuestion1, tbGeneralQuestions))
            {
                //invalid field
                return false;
            }

            //validate general question 2 answer
            if (!ValidateAnswerScore(qcGeneralQuestion2, tbGeneralQuestions))
            {
                //invalid field
                return false;
            }

            //validate general question 3 answer
            if (!ValidateAnswerScore(qcGeneralQuestion3, tbGeneralQuestions))
            {
                //invalid field
                return false;
            }

            //validate general question 4 answer
            if (!ValidateAnswerScore(qcGeneralQuestion4, tbGeneralQuestions))
            {
                //invalid field
                return false;
            }

            //validate general question 5 answer
            if (!ValidateAnswerComments(qcGeneralQuestion5, tbGeneralQuestions))
            {
                //invalid field
                return false;
            }

            //data is valid
            return true;
        }

        /// <summary>
        /// Validate the answer comments for selected question control.
        /// </summary>
        /// <param name="questionControl">
        /// The selected question control to be validated.
        /// </param>
        /// <param name="tab">
        /// The tab page where the field is located.
        /// </param>
        /// <returns>
        /// True if comments are set.
        /// False otherwise.
        /// </returns>
        protected bool ValidateAnswerComments(
            QuestionCommentControl questionControl, TabPage tab)
        {
            //check comments
            if (questionControl.Answer.Comments != null &&
                questionControl.Answer.Comments.Length > 0)
            {
                //comments are set
                return true;
            }

            //get question number from question control tag
            int questionNumber = -1;

            //check question control
            if (questionControl.Tag != null)
            {
                try
                {
                    //parse question number
                    questionNumber = int.Parse(questionControl.Tag.ToString());
                }
                catch
                {
                    //do nothing
                }
            }

            //focus control
            questionControl.Focus();

            //check tab
            if (tab != null)
            {
                //display tab
                mtbTabManager.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgRequiredAnswerComments, questionNumber),
                Properties.Resources.titleRequiredField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus control again
            questionControl.Focus();

            //field is empty
            return false;
        }

        /// <summary>
        /// Validate the answer score for selected question control.
        /// </summary>
        /// <param name="questionControl">
        /// The selected question control to be validated.
        /// </param>
        /// <param name="tab">
        /// The tab page where the field is located.
        /// </param>
        /// <returns>
        /// True if score is set.
        /// False otherwise.
        /// </returns>
        protected bool ValidateAnswerScore(QuestionControl questionControl, TabPage tab)
        {
            //check score
            if (questionControl.Answer.Score >= 0)
            {
                //score is set
                return true;
            }

            //get question number from question control tag
            int questionNumber = -1;

            //check question control
            if (questionControl.Tag != null)
            {
                try
                {
                    //parse question number
                    questionNumber = int.Parse(questionControl.Tag.ToString());
                }
                catch
                {
                    //do nothing
                }
            }

            //focus control
            questionControl.Focus();

            //check tab
            if (tab != null)
            {
                //display tab
                mtbTabManager.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgRequiredAnswerScore, questionNumber),
                Properties.Resources.titleRequiredField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus control again
            questionControl.Focus();

            //field is empty
            return false;
        }

        #endregion Private Methods


        #region UI Event handlers *****************************************************

        /// <summary>
        /// Control load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditReportTeacherMonth_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //check if constructor is already done
            if (!isConstructed)
            {
                //should never happen
                //metro library must be fixed
                throw new ApplicationException(
                    "Load event handler called before constructor is done at EditReportTeacherMonth.");
            }

            //set auto validate
            this.AutoValidate = AutoValidate.Disable;
            
            //set font to datagridviews
            dgvAttendances.ColumnHeadersDefaultCellStyle.Font = MetroFramework.MetroFonts.Default(12.0F);
            dgvAttendances.DefaultCellStyle.Font = MetroFramework.MetroFonts.DefaultLight(12.0F);
            dgvGrades.ColumnHeadersDefaultCellStyle.Font = MetroFramework.MetroFonts.Default(12.0F);
            dgvGrades.DefaultCellStyle.Font = MetroFramework.MetroFonts.DefaultLight(12.0F);

            //check if this control has a parent control to return to
            if (parentControl == null)
            {
                //hide return tile
                mtlReturn.Visible = false;
            }

            //set status to consulting
            Status = RegisterStatus.Consulting;

            //disable fields
            EnableFields(false);

            //refresh buttons
            RefreshButtons();

            //load the data for the selected report
            PerformDataLoad();

            //display first tab
            mtbTabManager.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles a click on the Edit button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEdit_Click(object sender, EventArgs e)
        {
            //calculate edition start date
            DateTime editionDate = report.ReferenceDate;

            //check number of class days
            if (classDays.Count > 0)
            {
                //user can edit report at last day of class
                editionDate = classDays[classDays.Count - 1];
            }
            else
            {
                //no class days
                //user can edit report one week before reference date
                editionDate = report.ReferenceDate.AddDays(-7);
            }

            //check if edition start date is in the last week of the semester
            if (editionDate > reportSemester.EndDate.AddDays(-7))
            {
                //let user edit report one week before end of semester
                editionDate = reportSemester.EndDate.AddDays(-7);
            }

            //check final edition start date
            if (editionDate > DateTime.Today)
            {
                //user cannot edit report just yet
                //display message
                MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.msgReportAvailableEdition,
                    editionDate.ToShortDateString()),
                    Properties.Resources.titleReportNotAvailableEdition,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                //exit
                return;
            }

            //change status to editing
            Status = RegisterStatus.Editing;

            //enable fields
            EnableFields(true);

            //edit report
            EditItem();
        }

        /// <summary>
        /// Handles a click on the Save button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                //set cursor to wait
                this.Cursor = Cursors.WaitCursor;

                //save report adn check result
                if (SaveItem())
                {
                    //change status to consulting
                    Status = RegisterStatus.Consulting;

                    //disable fields
                    EnableFields(false);

                    //refresh buttons
                    RefreshButtons();

                    //display message
                    Manager.MainForm.ShowStatusMessage(string.Format(
                        Properties.Resources.msgItemSaved, itemTypeDescription), 5000);

                    //reload report from database
                    if (!ReloadReportFromDatabase())
                    {
                        //could not load data
                        //should never happen
                        //disable edition
                        allowEditItem = false;

                        //refresh buttons
                        RefreshButtons();
                    }
                    else
                    {
                        //reload the data for the selected report
                        PerformDataLoad();
                    }
                }
                else
                {
                    //disable fields
                    EnableFields(false);

                    //disable edit button
                    btEdit.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //database error while saving item
                //show error message
                MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelSaveItem, itemTypeDescription, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log exception
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelSaveItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);
            }
            finally
            {
                //reset cursor
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Handles a click on the Cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCancel_Click(object sender, EventArgs e)
        {
            //cancel changes
            CancelChanges();

            //change status to consult
            Status = RegisterStatus.Consulting;

            //disable fields
            EnableFields(false);

            //reload the data for the selected report
            PerformDataLoad();

            //display message
            Manager.MainForm.ShowStatusMessage(
                Properties.Resources.msgEditionCanceled, 4000);
        }

        /// <summary>
        /// Return tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlReturn_Click(object sender, EventArgs e)
        {
            //check status if status is not consulting            
            if (Status != RegisterStatus.Consulting)
            {
                //user must confirm operation
                //show message box
                DialogResult dr = MetroMessageBox.Show(Manager.MainForm, string.Format(
                    PnT.SongClient.Properties.Resources.msgEditingItem, itemTypeDescription),
                    PnT.SongClient.Properties.Resources.titleCancelEdition,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                //check result
                if (dr == DialogResult.No)
                {
                    //user canceled operation
                    //exit
                    return;
                }
            }

            //ask parent control to dispose this child control
            parentControl.DisposeChildControl();
        }

        /// <summary>
        /// Previous day metro tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlPreviousDay_Click(object sender, EventArgs e)
        {
            //check current selected first class day
            if (displayedFirstClassDayIndex == 0)
            {
                //there is no other day in month
                //should never happen
                //hide tile
                mtlPreviousDay.Visible = false;

                //exit
                return;
            }

            //decrement displayed first day index
            displayedFirstClassDayIndex--;

            //display attendances for selected days
            DisplayAttendances();

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
            //check current selected first class day
            if (displayedFirstClassDayIndex == classDays.Count - 5)
            {
                //there is no other day in month
                //should never happen
                //hide tile
                mtlNextDay.Visible = false;

                //exit
                return;
            }

            //increment displayed first day index
            displayedFirstClassDayIndex++;

            //display attendances for selected days
            DisplayAttendances();

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
            //check status
            if (this.Status == RegisterStatus.Consulting)
            {
                //exit
                return;
            }

            //check row and column indexes
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                //exit
                return;
            }

            //get changed attendance data row
            DataRow attendanceDataRow = dtAttendances.Rows[e.RowIndex];
            
            //update its roll call text column
            attendanceDataRow[e.ColumnIndex + 5] = Properties.Resources.ResourceManager.GetString(
                "RollCall_" + ((RollCall)attendanceDataRow[e.ColumnIndex]).ToString());

            //get id of the changed attendance
            int attendanceId = (int)attendanceDataRow[e.ColumnIndex - 5];

            //get attendace using its id
            Attendance attendance = attendances.Find(a => a.AttendanceId == attendanceId);

            //update attendance
            attendance.RollCall = (int)attendanceDataRow[e.ColumnIndex];
            attendance.Updated = true;
        }

        /// <summary>
        /// Grades datagridview cell dirty state changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGrades_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //commit edit so cell value changed event is raised 
            dgvGrades.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        /// <summary>
        /// Grades datagridview cell value changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGrades_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //check status
            if (this.Status == RegisterStatus.Consulting)
            {
                //exit
                return;
            }

            //check row and column indexes
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                //exit
                return;
            }

            //get changed grade data row
            DataRow gradeDataRow = dtGrades.Rows[e.RowIndex];

            //get new score
            int newScore = (int)gradeDataRow[e.ColumnIndex];

            //update its grade text column
            gradeDataRow[e.ColumnIndex + gradeIdColumns.Count] = (newScore >= 0) ? newScore.ToString() : 
                Properties.Resources.ResourceManager.GetString(
                    "GradeSpecialScore_" + ((RollCall)newScore).ToString());

            //get id of the changed grade
            int gradeId = (int)gradeDataRow[e.ColumnIndex - gradeIdColumns.Count];

            //get attendace using its id
            Grade grade = grades.Find(a => a.GradeId == gradeId);

            //update grade
            grade.Score = newScore;
            grade.Updated = true;
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

            //check row registration status
            if ((int)dtAttendances.Rows[e.RowIndex]["RegistrationStatusValue"] == (int)ItemStatus.Evaded)
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
        /// Grades datagridview cell painting event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGrades_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //check row index
            if (e.RowIndex < 0)
            {
                //exit
                return;
            }

            //check row registration status
            if ((int)dtGrades.Rows[e.RowIndex]["RegistrationStatusValue"] == (int)ItemStatus.Evaded)
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

        #endregion UI Event handlers

    } //end of class EditReportTeacherMonth

} //end of namespace PnT.SongClient.UI.Controls
