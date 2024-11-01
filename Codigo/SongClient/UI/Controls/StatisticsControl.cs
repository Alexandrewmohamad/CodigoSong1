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
    /// Display and compare statistics for all application data.
    /// </summary>
    public partial class StatisticsControl : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The song child control.
        /// </summary>
        ISongControl childControl = null;

        /// <summary>
        /// Indicates if the control is being loaded.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// Indicates if the control is loading poles.
        /// </summary>
        private bool isLoadingPoles = false;

        /// <summary>
        /// Indicates if the control is loading teachers.
        /// </summary>
        private bool isLoadingTeachers = false;

        /// <summary>
        /// Indicates if the control is loading classes.
        /// </summary>
        private bool isLoadingClasses = false;

        /// <summary>
        /// Indicates if class statistics were loaded for set 1.
        /// </summary>
        private bool loadedClassesSet1 = false;

        /// <summary>
        /// Indicates if class statistics were loaded for set 2.
        /// </summary>
        private bool loadedClassesSet2 = false;

        /// <summary>
        /// Indicates if attendance statistics were loaded for set 1.
        /// </summary>
        private bool loadedAttendanceSet1 = false;

        /// <summary>
        /// Indicates if attendance statistics were loaded for set 2.
        /// </summary>
        private bool loadedAttendanceSet2 = false;

        /// <summary>
        /// Indicates if grade statistics were loaded for set 1.
        /// </summary>
        private bool loadedGradeSet1 = false;

        /// <summary>
        /// Indicates if grade statistics were loaded for set 2.
        /// </summary>
        private bool loadedGradeSet2 = false;

        /// <summary>
        /// The list of class month controls.
        /// </summary>
        private List<StatisticsClassMonthControl> classMonthControls = null;

        /// <summary>
        /// The list of attendance month controls.
        /// </summary>
        private List<StatisticsAttendanceMonthControl> attendanceMonthControls = null;

        /// <summary>
        /// The list of grade month controls.
        /// </summary>
        private List<StatisticsGradeMonthControl> gradeMonthControls = null;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatisticsControl()
        {
            //set loading flag
            isLoading = true;

            //init UI components
            InitializeComponent();

            //create list of controls
            //create list of class month controls
            classMonthControls = new List<StatisticsClassMonthControl>();
            classMonthControls.Add(scmA1);
            classMonthControls.Add(scmB1);
            classMonthControls.Add(scmC1);
            classMonthControls.Add(scmD1);
            classMonthControls.Add(scmE1);
            classMonthControls.Add(scmF1);
            classMonthControls.Add(scmA2);
            classMonthControls.Add(scmB2);
            classMonthControls.Add(scmC2);
            classMonthControls.Add(scmD2);
            classMonthControls.Add(scmE2);
            classMonthControls.Add(scmF2);

            attendanceMonthControls = new List<StatisticsAttendanceMonthControl>();
            attendanceMonthControls.Add(samA1);
            attendanceMonthControls.Add(samB1);
            attendanceMonthControls.Add(samC1);
            attendanceMonthControls.Add(samD1);
            attendanceMonthControls.Add(samE1);
            attendanceMonthControls.Add(samF1);
            attendanceMonthControls.Add(samA2);
            attendanceMonthControls.Add(samB2);
            attendanceMonthControls.Add(samC2);
            attendanceMonthControls.Add(samD2);
            attendanceMonthControls.Add(samE2);
            attendanceMonthControls.Add(samF2);

            gradeMonthControls = new List<StatisticsGradeMonthControl>();
            gradeMonthControls.Add(sgmA1);
            gradeMonthControls.Add(sgmB1);
            gradeMonthControls.Add(sgmC1);
            gradeMonthControls.Add(sgmD1);
            gradeMonthControls.Add(sgmE1);
            gradeMonthControls.Add(sgmF1);
            gradeMonthControls.Add(sgmA2);
            gradeMonthControls.Add(sgmB2);
            gradeMonthControls.Add(sgmC2);
            gradeMonthControls.Add(sgmD2);
            gradeMonthControls.Add(sgmE2);
            gradeMonthControls.Add(sgmF2);

            //load combos
            //list semesters
            ListSemesters();

            //list institutions
            ListInstitutions();

            //check if logged on user has an assigned institution
            if (Manager.LogonUser != null &&
                Manager.LogonUser.InstitutionId > 0)
            {
                //list assigned institution poles
                ListPoles(mcbPole1, Manager.LogonUser.InstitutionId);

                //list assigned institution teachers
                ListTeachers(mcbTeacher1, Manager.LogonUser.InstitutionId, -1);

                //list institution classes
                ListClasses(
                    mcbClass1, mcbSemester1.SelectedIndex > -1 ? (int)mcbSemester1.SelectedValue : -1,
                    Manager.LogonUser.InstitutionId, -1, -1);
            }
            else
            {
                //list all poles
                ListPoles(mcbPole1, -1);

                //list all teachers
                ListTeachers(mcbTeacher1, -1, -1);

                //list all classes
                ListClasses(
                    mcbClass1, mcbSemester1.SelectedIndex > -1 ? (int)mcbSemester1.SelectedValue : -1, 
                    -1, -1, -1);
            }

            //copy listed poles, teachers and classes from set 1 to set 2
            //list poles
            mcbPole2.ValueMember = "Id";
            mcbPole2.DisplayMember = "Description";
            mcbPole2.DataSource = new List<IdDescriptionStatus>(
                (List<IdDescriptionStatus>)mcbPole1.DataSource);

            //list teachers
            mcbTeacher2.ValueMember = "Id";
            mcbTeacher2.DisplayMember = "Description";
            mcbTeacher2.DataSource = new List<IdDescriptionStatus>(
                (List<IdDescriptionStatus>)mcbTeacher1.DataSource);

            //list classes
            mcbClass2.ValueMember = "ClassId";
            mcbClass2.DisplayMember = "Code";
            mcbClass2.DataSource = new List<Class>(
                (List<Class>)mcbClass1.DataSource);
        }

        #endregion Constructors


        #region Properties ************************************************************

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
            //select dashboard
            return "Statistic";
        }

        #endregion ISong Methods


        #region Private Methods *******************************************************

        /// <summary>
        /// Calculate mean grade and display result in the selected indicator.
        /// </summary>
        /// <param name="grades">
        /// The list of grades to calculate mean grade.
        /// </param>
        /// <param name="indicator">
        /// The selected indicator to display mean grade result.
        /// </param>
        private void CalculateAndDisplayMeanGrade(
            List<Grade> grades, IndicatorControl indicator, string caption)
        {
            //clear indicator
            indicator.ClearIndicators();

            //check number of grades
            if (grades == null || grades.Count == 0)
            {
                //no grade
                //set empty indicator
                indicator.AddIndicator("-", caption);

                //exit
                return;
            }

            //sum grade scores
            double sum = 0.0;

            //check each grade
            foreach (Grade grade in grades)
            {
                //add grade score
                sum += grade.Score;
            }

            //calculate mean score
            double mean = sum / (double)grades.Count;

            //set result
            indicator.AddIndicator(mean.ToString("0.0"), caption);
        }

        /// <summary>
        /// Display set 2 UI fields according to option.
        /// </summary>
        /// <param name="visible">
        /// True to show set 2 fields.
        /// False to hide set 2 fields.
        /// </param>
        private void DisplaySet2(bool visible)
        {
            //set filter controls
            tlpFilters2.Visible = visible;

            //set class statistic controls
            tlpClassIndicators2.Visible = visible;
            tlpClassData2.Visible = visible;

            //check each class month control
            for (int i = 6; i < classMonthControls.Count; i++)
            {
                //get next month control
                StatisticsClassMonthControl monthControl = classMonthControls[i];

                //display control only if month is not in the future
                monthControl.Visible = (monthControl.Month <= DateTime.Today) && visible;
            }

            //set attendance statistic controls
            tlpAttendanceIndicators2.Visible = visible;

            //check each attendance month control
            for (int i = 6; i < attendanceMonthControls.Count; i++)
            {
                //get next month control
                StatisticsAttendanceMonthControl monthControl = attendanceMonthControls[i];

                //display control only if month is not in the future
                monthControl.Visible = (monthControl.Month <= DateTime.Today) && visible;
            }

            //set grade statistic controls
            tlpGradeIndicators2.Visible = visible;

            //check each grade month control
            for (int i = 6; i < gradeMonthControls.Count; i++)
            {
                //get next month control
                StatisticsGradeMonthControl monthControl = gradeMonthControls[i];

                //display control only if month is not in the future
                monthControl.Visible = (monthControl.Month <= DateTime.Today) && visible;
            }
        }

        /// <summary>
        /// List classes into UI for selected institution
        /// </summary>
        /// <param name="mcbClass">
        /// The combo box to display listed classes.
        /// </param>
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
        /// <param name="teacherId">
        /// The ID of the selected teacher.
        /// -1 to select all teachers.
        /// </param>
        private void ListClasses(
            MetroComboBox mcbClass, int semesterId, int institutionId, int poleId, int teacherId)
        {
            //set default empty list to UI
            mcbClass.ValueMember = "ClassId";
            mcbClass.DisplayMember = "Code";
            mcbClass.DataSource = new List<Class>();

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
                List<Class> classes = songChannel.FindClassesByFilter(
                    false, false, true, (int)ItemStatus.Active, -1, -1, -1, 
                    semesterId, institutionId, poleId, teacherId);

                //check result
                if (classes[0].Result == (int)SelectResult.Success)
                {
                    //sort classes by description
                    classes.Sort((x, y) => x.Code.CompareTo(y.Code));

                    //check each class
                    foreach (Class classObj in classes)
                    {
                        //set description to code
                        classObj.Code = Manager.GetClassDescription(classObj, true);
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

                //create all option and add it to list
                Class allOption = new Class();
                allOption.ClassId = -1;
                allOption.Code = Properties.Resources.wordAllFeminine;
                allOption.ClassStatus = (int)ItemStatus.Active;
                classes.Insert(0, allOption);

                //set classes to UI
                mcbClass.ValueMember = "ClassId";
                mcbClass.DisplayMember = "Code";
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
        /// List institutions into UI.
        /// </summary>
        private void ListInstitutions()
        {
            //set default empty list to UI
            mcbInstitution1.ValueMember = "Id";
            mcbInstitution1.DisplayMember = "Description";
            mcbInstitution1.DataSource = new List<IdDescriptionStatus>();
            mcbInstitution2.ValueMember = "Id";
            mcbInstitution2.DisplayMember = "Description";
            mcbInstitution2.DataSource = new List<IdDescriptionStatus>();

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

                //check if a teacher is viewing its classes
                if (Manager.LogonTeacher != null &&
                    !Manager.HasLogonPermission("Class.View"))
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
                mcbInstitution1.ValueMember = "Id";
                mcbInstitution1.DisplayMember = "Description";
                mcbInstitution1.DataSource = new List<IdDescriptionStatus>(institutions);
                mcbInstitution2.ValueMember = "Id";
                mcbInstitution2.DisplayMember = "Description";
                mcbInstitution2.DataSource = new List<IdDescriptionStatus>(institutions);
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
        /// List poles into combo box for selected institution.
        /// </summary>
        /// <param name="mcbPole">
        /// The combo box to display listed poles.
        /// </param>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// -1 to select all institutions.
        /// </param>
        private void ListPoles(MetroComboBox mcbPole, int institutionId)
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
            mcbSemester1.ValueMember = "SemesterId";
            mcbSemester1.DisplayMember = "Description";
            mcbSemester1.DataSource = new List<Semester>();
            mcbSemester2.ValueMember = "SemesterId";
            mcbSemester2.DisplayMember = "Description";
            mcbSemester2.DataSource = new List<Semester>();

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
                List<Semester> semesters = songChannel.FindSemesters(false);
                
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

                //remove future semesters
                semesters = semesters.FindAll(s => s.StartDate <= DateTime.Today);

                //set semesters to UI
                mcbSemester1.ValueMember = "SemesterId";
                mcbSemester1.DisplayMember = "Description";
                mcbSemester1.DataSource = new List<Semester>(semesters);
                mcbSemester2.ValueMember = "SemesterId";
                mcbSemester2.DisplayMember = "Description";
                mcbSemester2.DataSource = new List<Semester>(semesters);

                //check if there is any semester to be selected 
                //other than all option
                if (semesters.Count >= 1)
                {
                    //check current semester
                    if (Manager.CurrentSemester.Result == (int)SelectResult.Success)
                    {
                        //select current semester
                        mcbSemester1.SelectedValue = Manager.CurrentSemester.SemesterId;
                        mcbSemester2.SelectedValue = Manager.CurrentSemester.SemesterId;
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
        /// List teachers into UI for selected institution
        /// </summary>
        /// <param name="mcbTeacher">
        /// The combo box to display listed teachers.
        /// </param>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// -1 to select all poles.
        /// </param>
        private void ListTeachers(MetroComboBox mcbTeacher, int institutionId, int poleId)
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

                //check if a teacher is viewing its classes
                if (Manager.LogonTeacher != null &&
                    !Manager.HasLogonPermission("Class.View"))
                {
                    //user is a teacher and has no class view permission
                    //can only view classes for logon teacher
                    //create list with only logon teacher
                    teachers = new List<IdDescriptionStatus>();
                    teachers.Add(Manager.LogonTeacher.GetDescription());

                    //does not create an all option
                    createAllOption = false;
                }
                //check selected pole
                else if (poleId > 0)
                {
                    //get list of pole active teachers
                    teachers = songChannel.ListTeachersByPole(
                        poleId, (int)ItemStatus.Active);
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
                    //user can view any teacher data
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
        /// Load and display filtered statistics for attendance tab.
        /// </summary>
        /// <param name="useSet1">
        /// True to reload data for set 1.
        /// False to reload data for set 2.
        /// </param>
        private void LoadAttendanceStatistics(bool useSet1)
        {
            //selected filters
            int semesterId, institutionId, poleId, teacherId, classId;

            //selected semester
            Semester semester = null;

            //check selected set
            if (useSet1)
            {
                //set filters
                semesterId = mcbSemester1.SelectedIndex >= 0 ? (int)mcbSemester1.SelectedValue : -1;
                institutionId = mcbInstitution1.SelectedIndex >= 0 ? (int)mcbInstitution1.SelectedValue : -1;
                poleId = mcbPole1.SelectedIndex >= 0 ? (int)mcbPole1.SelectedValue : -1;
                teacherId = mcbTeacher1.SelectedIndex >= 0 ? (int)mcbTeacher1.SelectedValue : -1;
                classId = mcbClass1.SelectedIndex >= 0 ? (int)mcbClass1.SelectedValue : -1;

                //set semester
                semester = mcbSemester1.SelectedIndex >= 0 ?
                    (Semester)mcbSemester1.SelectedItem : null;
            }
            else
            {
                //set filters
                semesterId = mcbSemester2.SelectedIndex >= 0 ? (int)mcbSemester2.SelectedValue : -1;
                institutionId = mcbInstitution2.SelectedIndex >= 0 ? (int)mcbInstitution2.SelectedValue : -1;
                poleId = mcbPole2.SelectedIndex >= 0 ? (int)mcbPole2.SelectedValue : -1;
                teacherId = mcbTeacher2.SelectedIndex >= 0 ? (int)mcbTeacher2.SelectedValue : -1;
                classId = mcbClass2.SelectedIndex >= 0 ? (int)mcbClass2.SelectedValue : -1;

                //set semester
                semester = mcbSemester2.SelectedIndex >= 0 ?
                    (Semester)mcbSemester2.SelectedItem : null;
            }

            //load indicator data
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
                //find attendances using class filters
                List<Attendance> attendances = songChannel.FindAttendancesByClassFilter(
                    false, false, semesterId, institutionId, poleId, teacherId, classId);

                //check result
                if (attendances[0].Result == (int)SelectResult.Empty)
                {
                    //no attendance was found
                    //clear list
                    attendances.Clear();
                }
                else if (attendances[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting attendances
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        Properties.Resources.item_Attendance, attendances[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        Properties.Resources.item_Attendance, attendances[0].ErrorMessage));

                    //could not load attendances
                    //exit
                    return;
                }

                //generate statistics
                //get number of presences
                List<Attendance> presences = attendances.FindAll(
                    a => a.RollCall == (int)RollCall.Present);

                //get number of absences
                List<Attendance> absences = attendances.FindAll(
                    a => a.RollCall == (int)RollCall.Absent);

                //calculate presence percentage
                double percentPresence = (presences.Count + absences.Count) > 0 ?
                    (double)presences.Count * 100.0 / (double)(presences.Count + absences.Count) : double.MinValue;

                //get indicator
                IndicatorControl icAttendance = useSet1 ? icAttendance1 : icAttendance2;

                //set data to indicator
                icAttendance.ClearIndicators();
                icAttendance.AddIndicator(percentPresence != double.MinValue ?
                    percentPresence.ToString("0.0") + "%" : "-%", 
                    Properties.Resources.item_Attendance);


                //get indicator
                IndicatorControl icCountPresence = useSet1 ? icCountPresence1 : icCountPresence2;

                //set data to indicator
                icCountPresence.ClearIndicators();
                icCountPresence.AddIndicator(presences.Count, Properties.Resources.caption_Presences);


                //get indicator
                IndicatorControl icCountAbsence = useSet1 ? icCountAbsence1 : icCountAbsence2;

                //set data to indicator
                icCountAbsence.ClearIndicators();
                icCountAbsence.AddIndicator(absences.Count, Properties.Resources.caption_Absences);


                //get indicator
                IndicatorControl icCounOtherRollCall = useSet1 ? icCounOtherRollCall1 : icCounOtherRollCall2;

                //set data to indicator
                icCounOtherRollCall.ClearIndicators();
                icCounOtherRollCall.AddIndicator(
                    attendances.Count - presences.Count - absences.Count, 
                    Properties.Resources.caption_Others);

                //set month statistics
                //check each month
                for (int i = 0; i < 6; i++)
                {
                    //get next month control
                    StatisticsAttendanceMonthControl monthControl = useSet1 ?
                        attendanceMonthControls[i] : attendanceMonthControls[i + 6];

                    //check semester
                    if (semester == null)
                    {
                        //no semester is selected
                        //should never happen
                        //hide month control
                        monthControl.Visible = false;

                        //go to next month control
                        continue;
                    }

                    //get month
                    DateTime month = semester.ReferenceDate.AddMonths(i);

                    //set month to control
                    monthControl.Month = month;

                    //check if month has not started yet
                    if (month > DateTime.Today)
                    {
                        //hide month control
                        monthControl.Visible = false;

                        //go to next month control
                        continue;
                    }

                    //show month control
                    monthControl.Visible = true;

                    //get month presences
                    presences = attendances.FindAll(a => a.RollCall == (int)RollCall.Present && 
                        a.Date.Year.Equals(month.Year) && a.Date.Month.Equals(month.Month));

                    //get month absences
                    absences = attendances.FindAll(a => a.RollCall == (int)RollCall.Absent &&
                        a.Date.Year.Equals(month.Year) && a.Date.Month.Equals(month.Month));

                    //calculate month presence percentage
                    percentPresence = (presences.Count + absences.Count) > 0 ?
                        (double)presences.Count * 100.0 / (double)(presences.Count + absences.Count) : double.MinValue;

                    //set indicators
                    monthControl.PercentagePresence = percentPresence;
                    monthControl.NumPresences = presences.Count;
                    monthControl.NumAbsences = absences.Count;
                }



                //class statistics were loaded
                if (useSet1)
                {
                    //set flag for set 1
                    loadedAttendanceSet1 = true;
                }
                else
                {
                    //set flag for set 2
                    loadedAttendanceSet2 = true;
                }
            }
            catch (Exception ex)
            {
                //database error while getting institutions
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Indicator), ex);

                //log exception
                Manager.Log.WriteException(ex);

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
        }

        /// <summary>
        /// Load and display filtered statistics for class tab.
        /// </summary>
        /// <param name="useSet1">
        /// True to reload data for set 1.
        /// False to reload data for set 2.
        /// </param>
        private void LoadClassStatistics(bool useSet1)
        {
            //selected filters
            int semesterId, institutionId, poleId, teacherId, classId;

            //selected semester
            Semester semester = null;

            //check selected set
            if (useSet1)
            {
                //set filters
                semesterId = mcbSemester1.SelectedIndex >= 0 ? (int)mcbSemester1.SelectedValue : -1;
                institutionId = mcbInstitution1.SelectedIndex >= 0 ? (int)mcbInstitution1.SelectedValue : -1;
                poleId = mcbPole1.SelectedIndex >= 0 ? (int)mcbPole1.SelectedValue : -1;
                teacherId = mcbTeacher1.SelectedIndex >= 0 ? (int)mcbTeacher1.SelectedValue : -1;
                classId = mcbClass1.SelectedIndex >= 0 ? (int)mcbClass1.SelectedValue : -1;

                //set semester
                semester = mcbSemester1.SelectedIndex >= 0 ?
                    (Semester)mcbSemester1.SelectedItem : null;
            }
            else
            {
                //set filters
                semesterId = mcbSemester2.SelectedIndex >= 0 ? (int)mcbSemester2.SelectedValue : -1;
                institutionId = mcbInstitution2.SelectedIndex >= 0 ? (int)mcbInstitution2.SelectedValue : -1;
                poleId = mcbPole2.SelectedIndex >= 0 ? (int)mcbPole2.SelectedValue : -1;
                teacherId = mcbTeacher2.SelectedIndex >= 0 ? (int)mcbTeacher2.SelectedValue : -1;
                classId = mcbClass2.SelectedIndex >= 0 ? (int)mcbClass2.SelectedValue : -1;

                //set semester
                semester = mcbSemester2.SelectedIndex >= 0 ?
                    (Semester)mcbSemester2.SelectedItem : null;
            }

            //load indicator data
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //exit
                return;
            }

            //local variables
            CountResult classCount = null;

            try
            {
                //check if there is a class selected
                if (classId > 0)
                {
                    //only one class
                    //set result without using service
                    classCount = new CountResult();
                    classCount.Count = 1;
                    classCount.Result = (int)SelectResult.Success;
                }
                else
                {
                    //get number of active classes
                    classCount = songChannel.CountClassesByFilter(
                        (int)ItemStatus.Active, -1, -1, -1,
                        semesterId, institutionId, poleId, teacherId);
                }

                //check result
                if (classCount.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(classCount, Properties.Resources.item_Class);

                    //could not load indicators
                    //exit
                    return;
                }

                //get classes indicator
                IndicatorControl icClasses = useSet1 ? icClasses1 : icClasses2;

                //set data to indicator
                icClasses.ClearIndicators();
                icClasses.AddIndicator(
                    classCount.Count, Properties.Resources.item_plural_Class);


                //get list of registrations
                List<Registration> registrations = songChannel.FindRegistrationsByFilter(
                    false, false, false, false, -1,
                    semesterId,
                    institutionId,
                    poleId,
                    teacherId,
                    classId);

                //check result
                if (registrations[0].Result == (int)SelectResult.Empty)
                {
                    //no registration was found
                    //clear list
                    registrations.Clear();
                }
                else if (registrations[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting registrations
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        Properties.Resources.item_Registration, registrations[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        Properties.Resources.item_Registration, registrations[0].ErrorMessage));

                    //could not load registrations
                    //exit
                    return;
                }

                //generate statistics
                HashSet<int> registeredStudentIds = new HashSet<int>();
                int evasionCount = 0;

                //check each registration
                foreach (Registration registration in registrations)
                {
                    //add student ID to list of student ids
                    registeredStudentIds.Add(registration.StudentId);

                    //check registration status
                    if (registration.RegistrationStatus == (int)ItemStatus.Evaded)
                    {
                        //increment number of evations
                        evasionCount++;
                    }
                }
                
                //get registered students indicator
                IndicatorControl icRegisteredStudents = useSet1 ? icRegisteredStudents1 : icRegisteredStudents2;

                //set data to indicator
                icRegisteredStudents.ClearIndicators();
                icRegisteredStudents.AddIndicator(
                    registeredStudentIds.Count, Properties.Resources.caption_RegisteredStudents);

                //get registrations indicator
                IndicatorControl icRegistrations = useSet1 ? icRegistrations1 : icRegistrations2;

                //set data to indicator
                icRegistrations.ClearIndicators();
                icRegistrations.AddIndicator(
                    registrations.Count, Properties.Resources.item_plural_Registration);

                //get evasions indicator
                IndicatorControl icEvasions = useSet1 ? icEvasions1 : icEvasions2;

                //set data to indicator
                icEvasions.ClearIndicators();
                icEvasions.AddIndicator(
                    evasionCount, Properties.Resources.item_plural_Evasion);

                //generate further statistics
                //calculate average registrion by class
                double avgRegistrationByClass = classCount.Count > 0 ?
                    (double)(registrations.Count) / (double)(classCount.Count) : double.MinValue;

                //get indicator
                MetroLabel mlblAvgRegistrationClassValue = useSet1 ?
                    mlblAvgRegistrationClassValue1 : mlblAvgRegistrationClassValue2;

                //set indicator
                mlblAvgRegistrationClassValue.Text = 
                    avgRegistrationByClass != double.MinValue ?
                    avgRegistrationByClass.ToString("0.00") : "-";


                //calculate average registrion by student
                double avgRegistrationByStudent = registeredStudentIds.Count > 0 ?
                    (double)(registrations.Count) / (double)(registeredStudentIds.Count) : double.MinValue;

                //get indicator
                MetroLabel mlblAvgRegistrationStudentValue = useSet1 ?
                    mlblAvgRegistrationStudentValue1 : mlblAvgRegistrationStudentValue2;

                //set indicator
                mlblAvgRegistrationStudentValue.Text =
                    avgRegistrationByStudent != double.MinValue ?
                    avgRegistrationByStudent.ToString("0.00") : "-";


                //calculate evasion percentage
                double percentEvasion = registrations.Count > 0 ?
                    (double)evasionCount * 100.0 / (double)registrations.Count : double.MinValue;

                //get indicator
                MetroLabel mlblPercentEvasionValue = useSet1 ?
                    mlblPercentEvasionValue1 : mlblPercentEvasionValue2;

                //set indicator
                mlblPercentEvasionValue.Text = percentEvasion != double.MinValue ? 
                    percentEvasion.ToString("0.00") + "%" : "-%";

                //set month statistics
                //check each month
                for (int i = 0; i < 6; i++)
                {
                    //get next month control
                    StatisticsClassMonthControl monthControl = useSet1 ?
                        classMonthControls[i] : classMonthControls[i + 6];
                    
                    //check semester
                    if (semester == null)
                    {
                        //no semester is selected
                        //should never happen
                        //hide month control
                        monthControl.Visible = false;

                        //go to next month control
                        continue;
                    }

                    //get month
                    DateTime month = semester.ReferenceDate.AddMonths(i);

                    //set month to control
                    monthControl.Month = month;

                    //check if month has not started yet
                    if (month > DateTime.Today)
                    {
                        //hide month control
                        monthControl.Visible = false;

                        //go to next month control
                        continue;
                    }

                    //show month control
                    monthControl.Visible = true;

                    //get month evasions
                    List<Registration> monthEvasions = registrations.FindAll(
                        r => r.InactivationTime >= month && r.InactivationTime < month.AddMonths(1));

                    //calculate month evasion percentage
                    double monthPercentEvasion = registrations.Count > 0 ?
                        (double)monthEvasions.Count * 100.0 / (double)registrations.Count : double.MinValue;

                    //set indicators
                    monthControl.NumEvasions = monthEvasions.Count;
                    monthControl.PercentageEvasion = monthPercentEvasion;
                }

                //class statistics were loaded
                if (useSet1)
                {
                    //set flag for set 1
                    loadedClassesSet1 = true;
                }
                else
                {
                    //set flag for set 2
                    loadedClassesSet2 = true;
                }
            }
            catch (Exception ex)
            {
                //database error while getting institutions
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Indicator), ex);

                //log exception
                Manager.Log.WriteException(ex);

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
        }

        /// <summary>
        /// Load and display filtered statistics for grade tab.
        /// </summary>
        /// <param name="useSet1">
        /// True to reload data for set 1.
        /// False to reload data for set 2.
        /// </param>
        private void LoadGradeStatistics(bool useSet1)
        {
            //selected filters
            int semesterId, institutionId, poleId, teacherId, classId;

            //selected semester
            Semester semester = null;

            //check selected set
            if (useSet1)
            {
                //set filters
                semesterId = mcbSemester1.SelectedIndex >= 0 ? (int)mcbSemester1.SelectedValue : -1;
                institutionId = mcbInstitution1.SelectedIndex >= 0 ? (int)mcbInstitution1.SelectedValue : -1;
                poleId = mcbPole1.SelectedIndex >= 0 ? (int)mcbPole1.SelectedValue : -1;
                teacherId = mcbTeacher1.SelectedIndex >= 0 ? (int)mcbTeacher1.SelectedValue : -1;
                classId = mcbClass1.SelectedIndex >= 0 ? (int)mcbClass1.SelectedValue : -1;

                //set semester
                semester = mcbSemester1.SelectedIndex >= 0 ?
                    (Semester)mcbSemester1.SelectedItem : null;
            }
            else
            {
                //set filters
                semesterId = mcbSemester2.SelectedIndex >= 0 ? (int)mcbSemester2.SelectedValue : -1;
                institutionId = mcbInstitution2.SelectedIndex >= 0 ? (int)mcbInstitution2.SelectedValue : -1;
                poleId = mcbPole2.SelectedIndex >= 0 ? (int)mcbPole2.SelectedValue : -1;
                teacherId = mcbTeacher2.SelectedIndex >= 0 ? (int)mcbTeacher2.SelectedValue : -1;
                classId = mcbClass2.SelectedIndex >= 0 ? (int)mcbClass2.SelectedValue : -1;

                //set semester
                semester = mcbSemester2.SelectedIndex >= 0 ?
                    (Semester)mcbSemester2.SelectedItem : null;
            }

            //load indicator data
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
                //find grades using class filters
                List<Grade> grades = songChannel.FindGradesByFilter(
                    false, false, false, false, false, false,
                    (int)GradeRapporteur.Teacher, -1, -1, -1,
                    semesterId, DateTime.MinValue, institutionId,
                    poleId, teacherId, -1, -1, classId);

                //check result
                if (grades[0].Result == (int)SelectResult.Empty)
                {
                    //no grade was found
                    //clear list
                    grades.Clear();
                }
                else if (grades[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting grades
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        Properties.Resources.item_Grade, grades[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        Properties.Resources.item_Grade, grades[0].ErrorMessage));

                    //could not load grades
                    //exit
                    return;
                }

                //remove special grades
                grades = grades.FindAll(g => g.Score >= 0);
                
                //display mean grade
                CalculateAndDisplayMeanGrade(
                    grades, 
                    useSet1 ? icMeanGrade1 : icMeanGrade2, 
                    Properties.Resources.wordMean);

                //display discipline mean grade
                CalculateAndDisplayMeanGrade(
                    grades.FindAll(g => g.GradeSubject == (int)GradeSubject.Discipline), 
                    useSet1 ? icDisciplineGrade1 : icDisciplineGrade2,       
                    Properties.Resources.GradeSubject_Discipline);

                //display performance mean grade
                CalculateAndDisplayMeanGrade(
                    grades.FindAll(g => g.GradeSubject == (int)GradeSubject.Performance),
                    useSet1 ? icPerformanceGrade1 : icPerformanceGrade2,
                    Properties.Resources.GradeSubject_Performance);

                //display dedication mean grade
                CalculateAndDisplayMeanGrade(
                    grades.FindAll(g => g.GradeSubject == (int)GradeSubject.Dedication),
                    useSet1 ? icDedicationGrade1 : icDedicationGrade2,
                    Properties.Resources.GradeSubject_Dedication);

                //set month statistics
                //check each month
                for (int i = 0; i < 6; i++)
                {
                    //get next month control
                    StatisticsGradeMonthControl monthControl = useSet1 ?
                        gradeMonthControls[i] : gradeMonthControls[i + 6];

                    //check semester
                    if (semester == null)
                    {
                        //no semester is selected
                        //should never happen
                        //hide month control
                        monthControl.Visible = false;

                        //go to next month control
                        continue;
                    }

                    //get month
                    DateTime month = semester.ReferenceDate.AddMonths(i);

                    //set month to control
                    monthControl.Month = month;

                    //check if month has not started yet
                    if (month > DateTime.Today)
                    {
                        //hide month control
                        monthControl.Visible = false;

                        //go to next month control
                        continue;
                    }

                    //show month control
                    monthControl.Visible = true;

                    //get month grades and set it to 
                    monthControl.SetGrades(grades.FindAll(g => g.ReferenceDate.Equals(month)));
                }



                //class statistics were loaded
                if (useSet1)
                {
                    //set flag for set 1
                    loadedGradeSet1 = true;
                }
                else
                {
                    //set flag for set 2
                    loadedGradeSet2 = true;
                }
            }
            catch (Exception ex)
            {
                //database error while getting institutions
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Indicator), ex);

                //log exception
                Manager.Log.WriteException(ex);

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
        }

        /// <summary>
        /// Reload and display filtered statistics for current selected tab.
        /// </summary>
        /// <param name="useSet1">
        /// True to reload data for set 1.
        /// False to reload data for set 2.
        /// </param>
        private void ReloadStatistics(bool useSet1)
        {
            //clear previous loaded statistics flags
            //check selected set
            if (useSet1)
            {
                //clear statistics flags
                loadedClassesSet1 = false;
                loadedAttendanceSet1 = false;
                loadedGradeSet1 = false;
            }
            else
            {
                //clear statistics flags
                loadedClassesSet2 = false;
                loadedAttendanceSet2 = false;
                loadedGradeSet2 = false;
            }

            //check selected tab
            if (mtbTabManager.SelectedTab.Equals(tbClasses))
            {
                //load class statistics
                LoadClassStatistics(useSet1);
            }
            else if (mtbTabManager.SelectedTab.Equals(tbAttendance))
            {
                //load attendance statistics
                LoadAttendanceStatistics(useSet1);
            }
            else if (mtbTabManager.SelectedTab.Equals(tbGrades))
            {
                //load grade statistics
                LoadGradeStatistics(useSet1);
            }
        }

        /// <summary>
        /// Process count error by displaying and logging the error.
        /// </summary>
        /// <param name="count">
        /// The count result.
        /// </param>
        /// <param name="itemTypeDescription">
        /// The type description of the count item.
        /// </param>
        private void ProcessCountError(CountResult count, string itemTypeDescription)
        {
            //display message
            MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.errorWebServiceCountItem,
                itemTypeDescription, count.ErrorMessage),
                Properties.Resources.titleWebServiceError,
                MessageBoxButtons.OK, MessageBoxIcon.Error);

            //log error
            Manager.Log.WriteError(string.Format(
                Properties.Resources.errorWebServiceCountItem,
                itemTypeDescription, count.ErrorMessage));
        }

        #endregion Private Methods


        #region UI Event Handlers *****************************************************

        /// <summary>
        /// Control load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatisticsControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //create colors
            Color colorIndicators1 = Color.FromArgb(235, 235, 235);
            Color colorIndicators2 = Color.FromArgb(
                colorIndicators1.R - 13, colorIndicators1.G - 13, colorIndicators1.B - 13);

            //set back color for indicators
            icClasses1.BackgroundColor = colorIndicators1;
            icClasses2.BackgroundColor = colorIndicators2;
            icRegisteredStudents1.BackgroundColor = colorIndicators1;
            icRegisteredStudents2.BackgroundColor = colorIndicators2;
            icRegistrations1.BackgroundColor = colorIndicators1;
            icRegistrations2.BackgroundColor = colorIndicators2;
            icEvasions1.BackgroundColor = colorIndicators1;
            icEvasions2.BackgroundColor = colorIndicators2;
            icAttendance1.BackgroundColor = colorIndicators1;
            icAttendance2.BackgroundColor = colorIndicators2;
            icCountPresence1.BackgroundColor = colorIndicators1;
            icCountPresence2.BackgroundColor = colorIndicators2;
            icCountAbsence1.BackgroundColor = colorIndicators1;
            icCountAbsence2.BackgroundColor = colorIndicators2;
            icCounOtherRollCall1.BackgroundColor = colorIndicators1;
            icCounOtherRollCall2.BackgroundColor = colorIndicators2;
            icMeanGrade1.BackgroundColor = colorIndicators1;
            icMeanGrade2.BackgroundColor = colorIndicators2;
            icDisciplineGrade1.BackgroundColor = colorIndicators1;
            icDisciplineGrade2.BackgroundColor = colorIndicators2;
            icPerformanceGrade1.BackgroundColor = colorIndicators1;
            icPerformanceGrade2.BackgroundColor = colorIndicators2;
            icDedicationGrade1.BackgroundColor = colorIndicators1;
            icDedicationGrade2.BackgroundColor = colorIndicators2;

            //set fore color for indicators
            icClasses1.ForegroundColor = Color.Black;
            icClasses2.ForegroundColor = Color.Black;
            icRegisteredStudents1.ForegroundColor = Color.Black;
            icRegisteredStudents2.ForegroundColor = Color.Black;
            icRegistrations1.ForegroundColor = Color.Black;
            icRegistrations2.ForegroundColor = Color.Black;
            icEvasions1.ForegroundColor = Color.Black;
            icEvasions2.ForegroundColor = Color.Black;
            icAttendance1.ForegroundColor = Color.Black;
            icAttendance2.ForegroundColor = Color.Black;
            icCountPresence1.ForegroundColor = Color.Black;
            icCountPresence2.ForegroundColor = Color.Black;
            icCountAbsence1.ForegroundColor = Color.Black;
            icCountAbsence2.ForegroundColor = Color.Black;
            icCounOtherRollCall1.ForegroundColor = Color.Black;
            icCounOtherRollCall2.ForegroundColor = Color.Black;
            icMeanGrade1.ForegroundColor = Color.Black;
            icMeanGrade2.ForegroundColor = Color.Black;
            icDisciplineGrade1.ForegroundColor = Color.Black;
            icDisciplineGrade2.ForegroundColor = Color.Black;
            icPerformanceGrade1.ForegroundColor = Color.Black;
            icPerformanceGrade2.ForegroundColor = Color.Black;
            icDedicationGrade1.ForegroundColor = Color.Black;
            icDedicationGrade2.ForegroundColor = Color.Black;

            //display first tab
            mtbTabManager.SelectedIndex = 0;

            //reset loading flag
            isLoading = false;

            //load first tab statistics for set1
            ReloadStatistics(true);

            //hide set 2 fields
            //simulate disable click
            mcbEnableSet2_CheckedChanged(mcbEnableSet2, new EventArgs());
        }

        /// <summary>
        /// Enable set 2 checked changed event handler. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbEnableSet2_CheckedChanged(object sender, EventArgs e)
        {
            //display set 2 fields according to option
            DisplaySet2(mcbEnableSet2.Checked);

            //check if set 2 is enabled
            if (mcbEnableSet2.Checked)
            {
                //load current tab statistics for set 2
                //simulate tab click event to make sure data is loaded
                mtbTabManager_SelectedIndexChanged(this, new EventArgs());
            }
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

            //get sender combo box
            MetroComboBox mcbSemester = (MetroComboBox)sender;

            //get set combo boxes
            MetroComboBox mcbInstitution = sender.Equals(mcbSemester1) ? mcbInstitution1 : mcbInstitution2;
            MetroComboBox mcbPole = sender.Equals(mcbSemester1) ? mcbPole1 : mcbPole2;
            MetroComboBox mcbTeacher = sender.Equals(mcbSemester1) ? mcbTeacher1 : mcbTeacher2;
            MetroComboBox mcbClass = sender.Equals(mcbSemester1) ? mcbClass1 : mcbClass2;

            //set flag is loading classes
            isLoadingClasses = true;

            //get id of current selected class
            int selectedClassId = mcbClass.SelectedIndex >= 0 ?
                (int)mcbClass.SelectedValue : int.MinValue;

            //reload class list
            ListClasses(mcbClass,
                mcbSemester.SelectedIndex >= 0 ? (int)mcbSemester.SelectedValue : -1,
                mcbInstitution.SelectedIndex >= 0 ? (int)mcbInstitution.SelectedValue : -1,
                mcbPole.SelectedIndex >= 0 ? (int)mcbPole.SelectedValue : -1,
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

            //reloade current tab statistics for selected set
            ReloadStatistics(sender.Equals(mcbSemester1));
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

            //get seleted set
            bool useSet1 = sender.Equals(mcbInstitution1);

            //get sender combo box
            MetroComboBox mcbInstitution = (MetroComboBox)sender;

            //get set combo boxes
            MetroComboBox mcbSemester = useSet1 ? mcbSemester1 : mcbSemester2;
            MetroComboBox mcbPole = useSet1 ? mcbPole1 : mcbPole2;
            MetroComboBox mcbTeacher = useSet1 ? mcbTeacher1 : mcbTeacher2;
            MetroComboBox mcbClass = useSet1 ? mcbClass1 : mcbClass2;

            //set flag is loading poles
            isLoadingPoles = true;

            //get id of current selected pole
            int selectedPoleId = mcbPole.SelectedIndex >= 0 ?
                (int)mcbPole.SelectedValue : int.MinValue;

            //reload pole list
            ListPoles(mcbPole, mcbInstitution.SelectedIndex >= 0 ? 
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
            

            //set flag is loading teachers
            isLoadingTeachers = true;

            //get id of current selected teacher
            int selectedTeacherId = mcbTeacher.SelectedIndex >= 0 ?
                (int)mcbTeacher.SelectedValue : int.MinValue;

            //reload teacher list
            ListTeachers(mcbTeacher,
                mcbInstitution.SelectedIndex >= 0 ? (int)mcbInstitution.SelectedValue : -1,
                mcbPole.SelectedIndex >= 0 ? (int)mcbPole.SelectedValue : -1);

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
            ListClasses(mcbClass,
                mcbSemester.SelectedIndex >= 0 ? (int)mcbSemester.SelectedValue : -1,
                mcbInstitution.SelectedIndex >= 0 ? (int)mcbInstitution.SelectedValue : -1,
                mcbPole.SelectedIndex >= 0 ? (int)mcbPole.SelectedValue : -1,
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

            //reloade current tab statistics for selected set
            ReloadStatistics(useSet1);
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

            //get seleted set
            bool useSet1 = sender.Equals(mcbPole1);

            //get sender combo box
            MetroComboBox mcbPole = (MetroComboBox)sender;

            //get set combo boxes
            MetroComboBox mcbSemester = useSet1 ? mcbSemester1 : mcbSemester2;
            MetroComboBox mcbInstitution = useSet1 ? mcbInstitution1 : mcbInstitution2;
            MetroComboBox mcbTeacher = useSet1 ? mcbTeacher1 : mcbTeacher2;
            MetroComboBox mcbClass = useSet1 ? mcbClass1 : mcbClass2;
            

            //set flag is loading teachers
            isLoadingTeachers = true;

            //get id of current selected teacher
            int selectedTeacherId = mcbTeacher.SelectedIndex >= 0 ?
                (int)mcbTeacher.SelectedValue : int.MinValue;

            //reload teacher list
            ListTeachers(mcbTeacher,
                mcbInstitution.SelectedIndex >= 0 ? (int)mcbInstitution.SelectedValue : -1,
                mcbPole.SelectedIndex >= 0 ? (int)mcbPole.SelectedValue : -1);

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
            ListClasses(mcbClass,
                mcbSemester.SelectedIndex >= 0 ? (int)mcbSemester.SelectedValue : -1,
                mcbInstitution.SelectedIndex >= 0 ? (int)mcbInstitution.SelectedValue : -1,
                mcbPole.SelectedIndex >= 0 ? (int)mcbPole.SelectedValue : -1,
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

            //reloade current tab statistics for selected set
            ReloadStatistics(useSet1);
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

            //get seleted set
            bool useSet1 = sender.Equals(mcbTeacher1);

            //get sender combo box
            MetroComboBox mcbTeacher = (MetroComboBox)sender;

            //get set combo boxes
            MetroComboBox mcbSemester = useSet1 ? mcbSemester1 : mcbSemester2;
            MetroComboBox mcbInstitution = useSet1 ? mcbInstitution1 : mcbInstitution2;
            MetroComboBox mcbPole = useSet1 ? mcbPole1 : mcbPole2;
            MetroComboBox mcbClass = useSet1 ? mcbClass1 : mcbClass2;

            
            //set flag is loading classes
            isLoadingClasses = true;

            //get id of current selected class
            int selectedClassId = mcbClass.SelectedIndex >= 0 ?
                (int)mcbClass.SelectedValue : int.MinValue;

            //reload class list
            ListClasses(mcbClass,
                mcbSemester.SelectedIndex >= 0 ? (int)mcbSemester.SelectedValue : -1,
                mcbInstitution.SelectedIndex >= 0 ? (int)mcbInstitution.SelectedValue : -1,
                mcbPole.SelectedIndex >= 0 ? (int)mcbPole.SelectedValue : -1,
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

            //reloade current tab statistics for selected set
            ReloadStatistics(useSet1);
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

            //reloade current tab statistics for selected set
            ReloadStatistics(sender.Equals(mcbClass1));
        }

        /// <summary>
        /// Tab manager selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtbTabManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading)
            {
                //no need to handle this event
                //exit
                return;
            }

            //check selected tab
            if (mtbTabManager.SelectedTab.Equals(tbClasses))
            {
                //check if statistics were not loaded for set 1
                if (!loadedClassesSet1)
                {
                    //reloade current tab statistics for set 1
                    LoadClassStatistics(true);
                }

                //check if set 2 is to be displayed
                if (mcbEnableSet2.Checked)
                {
                    //check if statistics were not loaded for set 2
                    if (!loadedClassesSet2)
                    {
                        //reloade current tab statistics for set 2
                        LoadClassStatistics(false);
                    }
                }
            }
            else if (mtbTabManager.SelectedTab.Equals(tbAttendance))
            {
                //check if statistics were not loaded for set 1
                if (!loadedAttendanceSet1)
                {
                    //reloade current tab statistics for set 1
                    LoadAttendanceStatistics(true);
                }

                //check if set 2 is to be displayed
                if (mcbEnableSet2.Checked)
                {
                    //check if statistics were not loaded for set 2
                    if (!loadedAttendanceSet2)
                    {
                        //reloade current tab statistics for set 2
                        LoadAttendanceStatistics(false);
                    }
                }
            }
            else if (mtbTabManager.SelectedTab.Equals(tbGrades))
            {
                //check if statistics were not loaded for set 1
                if (!loadedGradeSet1)
                {
                    //reloade current tab statistics for set 1
                    LoadGradeStatistics(true);
                }

                //check if set 2 is to be displayed
                if (mcbEnableSet2.Checked)
                {
                    //check if statistics were not loaded for set 2
                    if (!loadedGradeSet2)
                    {
                        //reloade current tab statistics for set 2
                        LoadGradeStatistics(false);
                    }
                }
            }
        }

        #endregion UI Event Handlers

    }

} //end of namespace PnT.SongClient.UI.Controls
