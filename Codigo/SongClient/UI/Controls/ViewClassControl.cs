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
    /// List and display classes to user.
    /// </summary>
    public partial class ViewClassControl : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The song child control.
        /// </summary>
        ISongControl childControl = null;

        /// <summary>
        /// List of classes shown on the control.
        /// </summary>
        private Dictionary<long, Class> classes = null;

        /// <summary>
        /// The last found class.
        /// Used to improve the find method.
        /// </summary>
        private Class lastFoundClass = null;

        /// <summary>
        /// DataTable for classes.
        /// </summary>
        private DataTable dtClasses = null;

        /// <summary>
        /// The item displayable name.
        /// </summary>
        private string itemTypeDescription = Properties.Resources.item_Class;

        /// <summary>
        /// The item displayable plural name.
        /// </summary>
        protected string itemTypeDescriptionPlural = Properties.Resources.item_plural_Class;

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
        /// Indicates if the control is loading teachers.
        /// </summary>
        private bool isLoadingTeachers = false;

        /// <summary>
        /// Right-clicked class. The class of the displayed context menu.
        /// </summary>
        private Class clickedClass = null;

        /// <summary>
        /// The class ID column index in the datagridview.
        /// </summary>
        private int columnIndexClassId;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ViewClassControl()
        {
            //set loading flag
            isLoading = true;

            //init ui components
            InitializeComponent();

            //create list of classes
            classes = new Dictionary<long, Class>();

            //create class data table
            CreateClassDataTable();

            //get class ID column index
            columnIndexClassId = dgvClasses.Columns[ClassId.Name].Index;

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
            mcbStatus.ValueMember = "Key";
            mcbStatus.DisplayMember = "Value";
            mcbStatus.DataSource = statuses;

            //list semesters
            ListSemesters();

            //list class types
            ListClassTypes();

            //list instrument types
            ListInstrumentTypes();

            //list class levels
            ListClassLevels();

            //list institutions
            ListInstitutions();
            
            //check if a teacher is viewing its classes
            if (Manager.LogonTeacher != null &&
                !Manager.HasLogonPermission("Class.View"))
            {
                //list all poles
                ListPoles(-1);

                //list all teachers
                ListTeachers(-1, -1);
            }
            //check if logged on user has an assigned institution
            else if (Manager.LogonUser != null &&
                Manager.LogonUser.InstitutionId > 0)
            {
                //list assigned institution poles
                ListPoles(Manager.LogonUser.InstitutionId);

                //list assigned institution teachers
                ListTeachers(Manager.LogonUser.InstitutionId, -1);
            }
            else
            {
                //list all poles
                ListPoles(-1);

                //list all teachers
                ListTeachers(-1, -1);
            }

            //subscribe settings property changed event
            Manager.Settings.PropertyChanged += Settings_PropertyChanged;
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get list of classes.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public List<Class> ListClasses
        {
            get
            {
                //lock list of classes
                lock (classes)
                {
                    return new List<Class>(classes.Values);
                }
            }
        }

        #endregion Properties


        #region ISong Methods *********************************************************

        /// <summary>
        /// Dispose used resources from class control.
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
            //select class
            return "Class";
        }

        #endregion ISong Methods


        #region Public Methods ********************************************************

        /// <summary>
        /// Set columns visibility according to application settings.
        /// </summary>
        public void SetVisibleColumns()
        {
            //split visible column settings
            string[] columns = Manager.Settings.ClassGridDisplayedColumns.Split(',');

            //hide all columns
            foreach (DataGridViewColumn dgColumn in dgvClasses.Columns)
            {
                //hide column
                dgColumn.Visible = false;
            }

            //set invisible last index
            int lastIndex = dgvClasses.Columns.Count - 1;

            //check each column and set visibility
            foreach (DataGridViewColumn dgvColumn in dgvClasses.Columns)
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

                        //set column display index class
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
        /// Create Class data table.
        /// </summary>
        private void CreateClassDataTable()
        {
            //create data table
            dtClasses = new DataTable();

            //ClassId
            DataColumn dcClassId = new DataColumn("ClassId", typeof(int));
            dtClasses.Columns.Add(dcClassId);

            //Semester
            DataColumn dcSemester = new DataColumn("SemesterName", typeof(string));
            dtClasses.Columns.Add(dcSemester);

            //SubjectCode
            DataColumn dcSubjectCode = new DataColumn("SubjectCode", typeof(string));
            dtClasses.Columns.Add(dcSubjectCode);

            //Code
            DataColumn dcCode = new DataColumn("Code", typeof(string));
            dtClasses.Columns.Add(dcCode);

            //Pole
            DataColumn dcPole = new DataColumn("PoleName", typeof(string));
            dtClasses.Columns.Add(dcPole);

            //Teacher
            DataColumn dcTeacher = new DataColumn("TeacherName", typeof(string));
            dtClasses.Columns.Add(dcTeacher);

            //AggregatedType
            DataColumn dcAggregatedTypeName = new DataColumn("AggregatedTypeName", typeof(string));
            dtClasses.Columns.Add(dcAggregatedTypeName);

            //ClassLevelName
            DataColumn dcClassLevel = new DataColumn("ClassLevelName", typeof(string));
            dtClasses.Columns.Add(dcClassLevel);

            //Capacity
            DataColumn dcCapacity = new DataColumn("Capacity", typeof(int));
            dtClasses.Columns.Add(dcCapacity);

            //WeekDays
            DataColumn dcWeekDays = new DataColumn("WeekDays", typeof(string));
            dtClasses.Columns.Add(dcWeekDays);

            //StartTime
            DataColumn dcStartTime = new DataColumn("StartTime", typeof(DateTime));
            dtClasses.Columns.Add(dcStartTime);

            //Duration
            DataColumn dcDuration = new DataColumn("Duration", typeof(int));
            dtClasses.Columns.Add(dcDuration);

            //ClassStatusName
            DataColumn dcClassStatus = new DataColumn("ClassStatusName", typeof(string));
            dtClasses.Columns.Add(dcClassStatus);

            //CreationTime
            DataColumn dcCreationTime = new DataColumn("CreationTime", typeof(DateTime));
            dtClasses.Columns.Add(dcCreationTime);

            //InactivationTime
            DataColumn dcInactivationTime = new DataColumn("InactivationTime", typeof(DateTime));
            dtClasses.Columns.Add(dcInactivationTime);

            //InactivationReason
            DataColumn dcInactivationReason = new DataColumn("InactivationReason", typeof(string));
            dtClasses.Columns.Add(dcInactivationReason);

            //set primary key column
            dtClasses.PrimaryKey = new DataColumn[] { dcClassId };
        }

        /// <summary>
        /// Display selected classes.
        /// Clear currently displayed classes before loading selected classes.
        /// </summary>
        /// <param name="selectedClasses">
        /// The selected classes to be loaded.
        /// </param>
        private void DisplayClasses(List<Class> selectedClasses)
        {
            //lock list of classes
            lock (this.classes)
            {
                //clear list
                this.classes.Clear();

                //reset last found class
                lastFoundClass = null;
            }

            //lock datatable of classes
            lock (dtClasses)
            {
                //clear datatable
                dtClasses.Clear();
            }

            //check number of selected classes
            if (selectedClasses != null && selectedClasses.Count > 0 &&
                selectedClasses[0].Result == (int)SelectResult.Success)
            {
                //lock list of classes
                lock (classes)
                {
                    //add selected classes
                    foreach (Class classObj in selectedClasses)
                    {
                        //check if class is not in the list
                        if (!classes.ContainsKey(classObj.ClassId))
                        {
                            //add class to the list
                            classes.Add(classObj.ClassId, classObj);

                            //set last found class
                            lastFoundClass = classObj;
                        }
                        else
                        {
                            //should never happen
                            //log message
                            Manager.Log.WriteError(
                                "Error while loading classes. Two classes with same ClassID " +
                                classObj.ClassId + ".");

                            //throw exception
                            throw new ApplicationException(
                                "Error while loading classes. Two classes with same ClassID " +
                                classObj.ClassId + ".");
                        }
                    }
                }

                //lock data table of classes
                lock (dtClasses)
                {
                    //check each class in the list
                    foreach (Class classObj in ListClasses)
                    {
                        //create, set and add class row
                        DataRow dr = dtClasses.NewRow();
                        SetClassDataRow(dr, classObj);
                        dtClasses.Rows.Add(dr);
                    }
                }
            }

            //refresh displayed UI
            RefreshUI(false);
        }

        /// <summary>
        /// Find class in the list of classes.
        /// </summary>
        /// <param name="classID">
        /// The ID of the selected class.
        /// </param>
        /// <returns>
        /// The class of the selected class ID.
        /// Null if class was not found.
        /// </returns>
        private Class FindClass(long classID)
        {
            //lock list of classes
            lock (classes)
            {
                //check last found class
                if (lastFoundClass != null &&
                    lastFoundClass.ClassId == classID)
                {
                    //same class
                    return lastFoundClass;
                }

                //try to find selected class
                classes.TryGetValue(classID, out lastFoundClass);

                //return result
                return lastFoundClass;
            }
        }

        /// <summary>
        /// List classes into UI.
        /// </summary>
        private void ListClassLevels()
        {
            //create list of class levels
            List<KeyValuePair<int, string>> classLevels = new List<KeyValuePair<int, string>>();

            //check each class level
            foreach (ClassLevel classLevel in Enum.GetValues(typeof(ClassLevel)))
            {
                //add converted class level
                classLevels.Add(new KeyValuePair<int, string>(
                    (int)classLevel, Properties.Resources.ResourceManager.GetString(
                        "ClassLevel_" + classLevel.ToString())
                    ));
            }

            //create all option and add it to list
            classLevels.Insert(0, new KeyValuePair<int, string>(
                -1, Properties.Resources.wordAll));

            //set class levels to UI
            mcbClassLevel.ValueMember = "Key";
            mcbClassLevel.DisplayMember = "Value";
            mcbClassLevel.DataSource = classLevels;
        }

        /// <summary>
        /// List classes into UI.
        /// </summary>
        private void ListClassTypes()
        {
            //create list of class types
            List<KeyValuePair<int, string>> classTypes = new List<KeyValuePair<int, string>>();

            //check each class type
            foreach (ClassType classType in Enum.GetValues(typeof(ClassType)))
            {
                //add converted class type
                classTypes.Add(new KeyValuePair<int, string>(
                    (int)classType, Properties.Resources.ResourceManager.GetString(
                        "ClassType_" + classType.ToString())
                    ));
            }

            //create all option and add it to list
            classTypes.Insert(0, new KeyValuePair<int, string>(
                -1, Properties.Resources.wordAll));

            //set class types to UI
            mcbClassType.ValueMember = "Key";
            mcbClassType.DisplayMember = "Value";
            mcbClassType.DataSource = classTypes;
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
        /// List teachers into UI for selected institution
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// -1 to select all poles.
        /// </param>
        private void ListTeachers(int institutionId, int poleId)
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
        /// Load and display filtered classes.
        /// </summary>
        /// <returns>
        /// True if classes were loaded.
        /// False otherwise.
        /// </returns>
        private bool LoadClasses()
        {
            //filter and load classes
            List<Class> filteredClasses = null;

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
                //get list of classes
                filteredClasses = songChannel.FindClassesByFilter(
                    true, true, true, 
                    (int)mcbStatus.SelectedValue,
                    (int)mcbClassType.SelectedValue,
                    (int)mcbInstrumentType.SelectedValue,
                    (int)mcbClassLevel.SelectedValue,
                    (int)mcbSemester.SelectedValue,
                    (int)mcbInstitution.SelectedValue,
                    (int)mcbPole.SelectedValue,
                    (int)mcbTeacher.SelectedValue);

                //check result
                if (filteredClasses[0].Result == (int)SelectResult.Empty)
                {
                    //no class was found
                    //clear list
                    filteredClasses.Clear();
                }
                else if (filteredClasses[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting classes
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredClasses[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredClasses[0].ErrorMessage));

                    //could not load classes
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

                //database error while getting classes
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load classes
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

            //sort classes by code
            filteredClasses.Sort((x, y) => x.Code.CompareTo(y.Code));

            //display filtered classes
            DisplayClasses(filteredClasses);

            //sort classes by code by default
            dgvClasses.Sort(Code, ListSortDirection.Ascending);

            //classes were loaded
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
            if (dgvClasses.DataSource == null)
            {
                //set source to datagrid
                dgvClasses.DataSource = dtClasses;
            }

            //check if last row should be displayed
            //and if the is at least one row
            if (displayLastRow && dgvClasses.Rows.Count > 0)
            {
                //refresh grid by displaying last row
                dgvClasses.FirstDisplayedScrollingRowIndex = (dgvClasses.Rows.Count - 1);
            }

            //refresh grid
            dgvClasses.Refresh();

            //set number of classes
            mlblItemCount.Text = classes.Count + " " +
                (classes.Count > 1 ? itemTypeDescriptionPlural : itemTypeDescription);
        }

        /// <summary>
        /// Set data row with selected Class data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="class">The selected class.</param>
        private void SetClassDataRow(DataRow dataRow, Class classObj)
        {
            dataRow["ClassId"] = classObj.ClassId;
            dataRow["SemesterName"] = classObj.Semester != null ?
                classObj.Semester.Description : "-";
            dataRow["SubjectCode"] = classObj.SubjectCode.ToString("00000");
            dataRow["Code"] = classObj.Code;
            dataRow["PoleName"] = classObj.PoleName;
            dataRow["TeacherName"] = classObj.TeacherName;
            dataRow["AggregatedTypeName"] = classObj.ClassType == (int)ClassType.Instrument ?
                Properties.Resources.ResourceManager.GetString(
                    "InstrumentsType_" + ((InstrumentsType)classObj.InstrumentType).ToString()) :
                Properties.Resources.ResourceManager.GetString(
                    "ClassType_" + ((ClassType)classObj.ClassType).ToString());
            dataRow["ClassLevelName"] = Properties.Resources.ResourceManager.GetString(
                "ClassLevel_" + ((ClassLevel)classObj.ClassLevel).ToString());
            dataRow["Capacity"] = classObj.Capacity;
            dataRow["StartTime"] = classObj.StartTime;
            dataRow["Duration"] = classObj.Duration;
            dataRow["ClassStatusName"] = (classObj.ClassStatus == (int)ItemStatus.Active) ?
                Properties.Resources.ResourceManager.GetString(
                    "ClassProgress_" + ((ClassProgress)classObj.ClassProgress).ToString()) :
                Properties.Resources.ResourceManager.GetString(
                    "ItemStatus_" + ((ItemStatus)classObj.ClassStatus).ToString());
            dataRow["CreationTime"] = classObj.CreationTime;
            dataRow["InactivationReason"] = classObj.InactivationReason;

            //set inactivation time
            if (classObj.InactivationTime != DateTime.MinValue)
                dataRow["InactivationTime"] = classObj.InactivationTime;
            else
                dataRow["InactivationTime"] = DBNull.Value;

            //gather week days
            StringBuilder sbDays = new StringBuilder();
            if (classObj.WeekMonday)
            {
                sbDays.Append(Properties.Resources.dayMonday);
                sbDays.Append(", ");
            }
            if (classObj.WeekTuesday)
            {
                sbDays.Append(Properties.Resources.dayTuesday);
                sbDays.Append(", ");
            }
            if (classObj.WeekWednesday)
            {
                sbDays.Append(Properties.Resources.dayWednesday);
                sbDays.Append(", ");
            }
            if (classObj.WeekThursday)
            {
                sbDays.Append(Properties.Resources.dayThursday);
                sbDays.Append(", ");
            }
            if (classObj.WeekFriday)
            {
                sbDays.Append(Properties.Resources.dayFriday);
                sbDays.Append(", ");
            }
            if (classObj.WeekSaturday)
            {
                sbDays.Append(Properties.Resources.daySaturday);
                sbDays.Append(", ");
            }
            if (classObj.WeekSunday)
            {
                sbDays.Append(Properties.Resources.daySunday);
                sbDays.Append(", ");
            }

            //check result
            if (sbDays.Length > 2)
            {
                //remove last ", "
                sbDays.Length -= 2;

                //WeekDays
                dataRow["WeekDays"] = sbDays.ToString();
            }
            else
            {
                //WeekDays
                dataRow["WeekDays"] = "-";
            }
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Remove displayed class.
        /// </summary>
        /// <param name="classId">
        /// The ID of the class to be removed.
        /// </param>
        public void RemoveClass(int classId)
        {
            //lock list of classes
            lock (classes)
            {
                //check if class is not in the list
                if (!classes.ContainsKey(classId))
                {
                    //no need to remove class
                    //exit
                    return;
                }

                //remove class
                classes.Remove(classId);
            }

            //lock data table of classes
            lock (dtClasses)
            {
                //get displayed data row
                DataRow dr = dtClasses.Rows.Find(classId);

                //remove displayed data row
                dtClasses.Rows.Remove(dr);
            }

            //refresh class interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update the status of a displayed class.
        /// </summary>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <param name="classStatus">
        /// The updated status of the class.
        /// </param>
        /// <param name="inactivationTime">
        /// The time the class was inactivated.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the class is being inactivated.
        /// </param>
        public void UpdateClass(int classId, int classStatus,
            DateTime inactivationTime, string inactivationReason)
        {
            //the class to be updated
            Class classObj = null;

            //lock list of classes
            lock (classes)
            {
                //try to find class
                if (!classes.TryGetValue(classId, out classObj))
                {
                    //class was not found
                    //no need to update class
                    //exit
                    return;
                }
            }

            //update status
            classObj.ClassStatus = classStatus;

            //update inactivation
            classObj.InactivationTime = inactivationTime;
            classObj.InactivationReason = inactivationReason;

            //update displayed class
            UpdateClass(classObj);
        }

        /// <summary>
        /// Update a displayed class. 
        /// Add class if it is a new class.
        /// </summary>
        /// <param name="classObj">
        /// The updated class.
        /// </param>
        public void UpdateClass(Class classObj)
        {
            //check class should be displayed
            //status filter
            if (mcbStatus.SelectedIndex > 0 &&
                (int)mcbStatus.SelectedValue != classObj.ClassStatus)
            {
                //class should not be displayed
                //remove class if it is being displayed
                RemoveClass(classObj.ClassId);

                //exit
                return;
            }

            //class type filter
            if (mcbClassType.SelectedIndex > 0 &&
                (int)mcbClassType.SelectedValue != classObj.ClassType)
            {
                //class should not be displayed
                //remove class if it is being displayed
                RemoveClass(classObj.ClassId);

                //exit
                return;
            }

            //check if class type is instrument
            if (classObj.ClassType == (int)ClassType.Instrument)
            {
                //instrument type filter
                if (mcbInstrumentType.SelectedIndex > 0 &&
                    (int)mcbInstrumentType.SelectedValue != classObj.InstrumentType)
                {
                    //class should not be displayed
                    //remove class if it is being displayed
                    RemoveClass(classObj.ClassId);

                    //exit
                    return;
                }
            }

            //class level filter
            if (mcbClassLevel.SelectedIndex > 0 &&
                (int)mcbClassLevel.SelectedValue != classObj.ClassLevel)
            {
                //class should not be displayed
                //remove class if it is being displayed
                RemoveClass(classObj.ClassId);

                //exit
                return;
            }

            //semester filter
            if (mcbSemester.SelectedIndex > 0 &&
                (int)mcbSemester.SelectedValue != classObj.SemesterId)
            {
                //class should not be displayed
                //remove class if it is being displayed
                RemoveClass(classObj.ClassId);

                //exit
                return;
            }

            //pole filter
            if (mcbPole.SelectedIndex > 0 &&
                (int)mcbPole.SelectedValue != classObj.PoleId)
            {
                //class should not be displayed
                //remove class if it is being displayed
                RemoveClass(classObj.ClassId);

                //exit
                return;
            }

            ////institution filter
            ////no pole should be selected
            //if (mcbPole.SelectedIndex == -1 && 
            //    mcbInstitution.SelectedIndex > 0 &&
            //    (int)mcbInstitution.SelectedValue != class.InstitutionId)
            //{
            //    //class should not be displayed
            //    //remove class if it is being displayed
            //    RemoveClass(class.ClassId);

            //    //exit
            //    return;
            //}

            //teacher filter
            if (mcbTeacher.SelectedIndex > 0 &&
                (int)mcbTeacher.SelectedValue != classObj.TeacherId)
            {
                //class should not be displayed
                //remove class if it is being displayed
                RemoveClass(classObj.ClassId);

                //exit
                return;
            }

            //lock list of classes
            lock (classes)
            {
                //set class
                classes[classObj.ClassId] = classObj;
            }

            //lock data table of classes
            lock (dtClasses)
            {
                //get displayed data row
                DataRow dr = dtClasses.Rows.Find(classObj.ClassId);

                //check if there was no data row yet
                if (dr == null)
                {
                    //create, set and add data row
                    dr = dtClasses.NewRow();
                    SetClassDataRow(dr, classObj);
                    dtClasses.Rows.Add(dr);
                }
                else
                {
                    //set data
                    SetClassDataRow(dr, classObj);
                }
            }

            //refresh class interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed classes with pole data.
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

            //gather list of updated classes
            List<Class> updatedClasses = new List<Class>();

            //lock list
            lock (classes)
            {
                //check all displayed classes
                foreach (Class classObj in classes.Values)
                {
                    //check class pole
                    if (classObj.PoleId == pole.PoleId &&
                        !classObj.PoleName.Equals(pole.Name))
                    {
                        //update class
                        classObj.PoleName = pole.Name;

                        //add class to the list of updated classes
                        updatedClasses.Add(classObj);
                    }
                }
            }

            //check result
            if (updatedClasses.Count == 0)
            {
                //no class was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of classes
            lock (dtClasses)
            {
                //check each updated role
                foreach (Class classObj in updatedClasses)
                {
                    //get displayed data row
                    DataRow dr = dtClasses.Rows.Find(classObj.ClassId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetClassDataRow(dr, classObj);
                    }
                }
            }

            //refresh pole interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed classes with teacher data.
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

            //gather list of updated classes
            List<Class> updatedClasses = new List<Class>();

            //lock list
            lock (classes)
            {
                //check all displayed classes
                foreach (Class classObj in classes.Values)
                {
                    //check class teacher
                    if (classObj.TeacherId == teacher.TeacherId &&
                        !classObj.TeacherName.Equals(teacher.Name))
                    {
                        //update class
                        classObj.TeacherName = teacher.Name;

                        //add class to the list of updated classes
                        updatedClasses.Add(classObj);
                    }
                }
            }

            //check result
            if (updatedClasses.Count == 0)
            {
                //no class was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of classes
            lock (dtClasses)
            {
                //check each updated role
                foreach (Class classObj in updatedClasses)
                {
                    //get displayed data row
                    DataRow dr = dtClasses.Rows.Find(classObj.ClassId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetClassDataRow(dr, classObj);
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
                dgvClasses.ColumnHeadersDefaultCellStyle.Font =
                    MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
                dgvClasses.DefaultCellStyle.Font =
                    MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            }
            else if (e.PropertyName.Equals("ClassGridDisplayedColumns"))
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
        private void ViewClassControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //avoid auto generated columns
            dgvClasses.AutoGenerateColumns = false;

            //set fonts
            dgvClasses.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvClasses.DefaultCellStyle.Font =
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

            //clear number of classes
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

            //load classes
            LoadClasses();
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

            //load classes
            LoadClasses();
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

            //set flag is loading teachers
            isLoadingTeachers = true;

            //get id of current selected teacher
            int selectedTeacherId = mcbTeacher.SelectedIndex >= 0 ?
                (int)mcbTeacher.SelectedValue : int.MinValue;

            //reload teacher list
            ListTeachers(
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

            //load classes
            LoadClasses();
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

            //set flag is loading teachers
            isLoadingTeachers = true;

            //get id of current selected teacher
            int selectedTeacherId = mcbTeacher.SelectedIndex >= 0 ?
                (int)mcbTeacher.SelectedValue : int.MinValue;

            //reload teacher list
            ListTeachers(
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

            //load classes
            LoadClasses();
        }

        /// <summary>
        /// Class type combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbClassType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading)
            {
                //no need to handle this event
                //exit
                return;
            }

            //set flag is loading instrument types
            isLoadingInstrumentTypes = true;

            //check if user selected all class types or instrument type
            if ((int)mcbClassType.SelectedValue == -1 ||
                (int)mcbClassType.SelectedValue == (int)ClassType.Instrument)
            {
                //enable instrument type combo
                mcbInstrumentType.Enabled = true;
            }
            else
            {
                //disable instrument type combo
                mcbInstrumentType.Enabled = false;

                //reset selected instrument type
                mcbInstrumentType.SelectedValue = -1;
            }

            //reset flag is loading instrument types
            isLoadingInstrumentTypes = false;

            //load classes
            LoadClasses();
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

            //load classes
            LoadClasses();
        }

        /// <summary>
        /// Class level combo box selected index changed event handler. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is loading
            if (isLoading)
            {
                //no need to handle this event
                //exit
                return;
            }

            //load classes
            LoadClasses();
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

            //load classes
            LoadClasses();
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

            //check if there is any selected class
            if (dgvClasses.SelectedCells.Count > 0)
            {
                //select first selected class in the register control
                registerControl.FirstSelectedId = 
                    (int)dgvClasses.Rows[dgvClasses.SelectedCells[0].RowIndex].Cells[columnIndexClassId].Value;
            }

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Classes datagridview mouse up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvClasses_MouseUp(object sender, MouseEventArgs e)
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
            DataGridView.HitTestInfo hitInfo = dgvClasses.HitTest(e.X, e.Y);

            //check selected row index
            if (hitInfo.RowIndex < 0)
            {
                //no row selected
                //exit
                return;
            }

            //check if there are selected rows and if class clicked on them
            if (dgvClasses.SelectedRows.Count > 0 &&
                dgvClasses.Rows[hitInfo.RowIndex].Selected != true)
            {
                //class did not click in the selected rows
                //clear selection
                dgvClasses.ClearSelection();

                //select clicked row
                dgvClasses.Rows[hitInfo.RowIndex].Selected = true;
            }
            //check if there are selected cells
            else if (dgvClasses.SelectedCells.Count > 0)
            {
                //get list of selected cell rows
                HashSet<int> selectedRowIndexes = new HashSet<int>();

                //check each selected cell
                foreach (DataGridViewCell cell in dgvClasses.SelectedCells)
                {
                    //add cell row index to the list
                    selectedRowIndexes.Add(cell.RowIndex);
                }

                //check if class clicked on a row of a selected cell
                if (selectedRowIndexes.Contains(hitInfo.RowIndex))
                {
                    //class clicked on a row of a selected cell
                    //select all rows
                    foreach (int selectedRowIndex in selectedRowIndexes)
                    {
                        //select row
                        dgvClasses.Rows[selectedRowIndex].Selected = true;
                    }
                }
                else
                {
                    //class did not click on a row of a selected cell
                    //clear selected cells
                    dgvClasses.ClearSelection();

                    //select clicked row
                    dgvClasses.Rows[hitInfo.RowIndex].Selected = true;
                }
            }
            else
            {
                //select clicked row
                dgvClasses.Rows[hitInfo.RowIndex].Selected = true;
            }

            //get selected or clicked class
            clickedClass = null;

            //check if there is a selected class
            if (dgvClasses.SelectedRows.Count > 0)
            {
                //there is one or more classes selected
                //get first selected class
                for (int index = 0; index < dgvClasses.SelectedRows.Count; index++)
                {
                    //get class using its class id
                    int classId = (int)dgvClasses.SelectedRows[index].Cells[columnIndexClassId].Value;
                    Class classObj = FindClass(classId);

                    //check result
                    if (classObj != null)
                    {
                        //add class to list
                        clickedClass = classObj;

                        //exit loop
                        break;
                    }
                }

                //check result
                if (clickedClass == null)
                {
                    //no class was found
                    //should never happen
                    //do not display context menu
                    //exit
                    return;
                }
            }
            else
            {
                //there is no class selected
                //should never happen
                //do not display context menu
                //exit
                return;
            }

            //set context menu items visibility
            if (dgvClasses.SelectedRows.Count == 1)
            {
                //display view menu items according to permissions
                mnuViewClass.Visible = true;
                mnuViewPole.Visible = Manager.HasLogonPermission("Pole.View");
                tssSeparator.Visible = true;

                //display generate reports and impersonate options
                mnuGenerateReports.Visible = 
                    clickedClass.ClassProgress == (int)ClassProgress.InProgress && Manager.HasLogonPermission("Report.Generate");
                mnuImpersonateTeacher.Visible = 
                    Manager.ImpersonatingUser == null && Manager.HasLogonPermission("User.Impersonate");
                tssSeparatorSpecial.Visible = 
                    (clickedClass.ClassProgress == (int)ClassProgress.InProgress && Manager.HasLogonPermission("Report.Generate")) || 
                    (Manager.ImpersonatingUser == null && Manager.HasLogonPermission("User.Impersonate"));
            }
            else
            {
                //hide view menu items
                mnuViewClass.Visible = false;
                mnuViewPole.Visible = false;
                tssSeparator.Visible = false;

                //hide generate reports and impersonate options
                mnuGenerateReports.Visible = false;
                mnuImpersonateTeacher.Visible = false;
                tssSeparatorSpecial.Visible = false;
            }

            //show class context menu on the clicked point
            mcmClass.Show(this.dgvClasses, p);
        }

        /// <summary>
        /// View class menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewClass_Click(object sender, EventArgs e)
        {
            //check clicked class
            if (clickedClass == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected class
            RegisterClassControl registerControl =
                new UI.Controls.RegisterClassControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedClass.ClassId;

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
            if (clickedClass == null)
            {
                //should never happen
                //exit
                return;
            }

            //check class pole
            if (clickedClass.PoleId <= 0)
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
            registerControl.FirstSelectedId = clickedClass.PoleId;

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
            //check clicked class
            if (clickedClass == null)
            {
                //should never happen
                //exit
                return;
            }

            //check class teacher
            if (clickedClass.TeacherId <= 0)
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
            registerControl.FirstSelectedId = clickedClass.TeacherId;

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Generate missing reports menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuGenerateReports_Click(object sender, EventArgs e)
        {
            //check clicked class
            if (clickedClass == null)
            {
                //should never happen
                //exit
                return;
            }

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
                //get missing report months for selected class
                List<DateTimeResult> missingMonths = songChannel.CheckReports(clickedClass.ClassId);

                //check result
                if (missingMonths[0].Result == (int)SelectResult.Empty)
                {
                    //no report is missing
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.msgNoMissingReport, clickedClass.Code),
                        Properties.Resources.titleMissingReports,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //exit
                    return;
                }
                else if (missingMonths[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting missing reports
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        Properties.Resources.item_plural_Report, missingMonths[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        Properties.Resources.item_plural_Report, missingMonths[0].ErrorMessage));

                    //exit
                    return;
                }

                //there are missing reports
                //gather minssing report periods
                StringBuilder missingReports = new StringBuilder(32);

                //check each date
                foreach (DateTimeResult month in missingMonths)
                {
                    //append month report
                    missingReports.Append(
                        clickedClass.Semester.Description + "." + month.Time.Month + " " + 
                        Properties.Resources.ResourceManager.GetString("Month_" + month.Time.Month));

                    //add separator
                    missingReports.Append("\n");
                }

                //remove last separator
                missingReports.Length -= 1;

                //display missing reports and ask user to confirm operation
                DialogResult result = MetroFramework.MetroMessageBox.Show(Manager.MainForm, 
                    string.Format(Properties.Resources.msgMissingReports, 
                    clickedClass.Code, missingReports.ToString()),
                    Properties.Resources.titleMissingReports,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                //check result
                if (result != DialogResult.OK)
                {
                    //user canceled operation
                    //exit
                    return;
                }

                //generate each missing report
                foreach (DateTimeResult month in missingMonths)
                {
                    //generate report and get result
                    SaveResult saveResult = songChannel.GenerateReport(clickedClass.ClassId, month.Time);

                    //check result
                    if (saveResult.Result == (int)SelectResult.FatalError)
                    {
                        //unexpected error while generating report
                        //display message
                        MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.errorWebServiceSaveItem,
                            Properties.Resources.item_plural_Report, saveResult.ErrorMessage),
                            Properties.Resources.titleWebServiceError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //log error
                        Manager.Log.WriteError(string.Format(
                            Properties.Resources.errorWebServiceSaveItem,
                            Properties.Resources.item_plural_Report, saveResult.ErrorMessage));

                        //could not generate report
                        return;
                    }
                }

                //missing reports were generated
                //display result and ask user if they should be displayed
                result = MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.msgMissingReportsGenerated, clickedClass.Code),
                    Properties.Resources.titleMissingReportsGenerated,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                //check result
                if (result == DialogResult.OK)
                {
                    //display class reports
                    Manager.MainForm.DisplayReports(
                        clickedClass.SemesterId, clickedClass.ClassId);
                }
            }
            catch (Exception ex)
            {
                //show error message
                MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelSaveItem, 
                    Properties.Resources.item_plural_Report, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //database error while generating reports
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelSaveItem, 
                    Properties.Resources.item_plural_Report, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load classes
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
        /// Impersonate teacher menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuImpersonateTeacher_Click(object sender, EventArgs e)
        {
            //check clicked class
            if (clickedClass == null)
            {
                //should never happen
                //exit
                return;
            }

            //check class teacher user id
            if (clickedClass.TeacherUserId <= 0)
            {
                //no teacher user
                //should never happen
                //exit
                return;
            }

            //let user impersonate teacher user
            //display impersonation confirmation
            Manager.MainForm.ConfirmAndImpersonateUser(
                clickedClass.TeacherUserId, clickedClass.TeacherName);
        }

        /// <summary>
        /// Copy menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCopy_Click(object sender, EventArgs e)
        {
            //check if any cell is selected
            if (this.dgvClasses.GetCellCount(DataGridViewElementStates.Selected) <= 0)
            {
                //should never happen
                //exit
                return;
            }

            try
            {
                //include header text 
                dgvClasses.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                //copy selection to the clipboard
                Clipboard.SetDataObject(this.dgvClasses.GetClipboardContent());
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

    } //end of class ViewClassControl 

} //end of namespace PnT.SongClient.UI.Controls
