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
    /// List and display teachers to user.
    /// </summary>
    public partial class ViewTeacherControl : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The song child control.
        /// </summary>
        ISongControl childControl = null;

        /// <summary>
        /// List of teachers shown on the control.
        /// </summary>
        private Dictionary<long, Teacher> teachers = null;

        /// <summary>
        /// The last found teacher.
        /// Used to improve the find method.
        /// </summary>
        private Teacher lastFoundTeacher = null;

        /// <summary>
        /// DataTable for teachers.
        /// </summary>
        private DataTable dtTeachers = null;

        /// <summary>
        /// The item displayable name.
        /// </summary>
        private string itemTypeDescription = Properties.Resources.item_Teacher;

        /// <summary>
        /// The item displayable plural name.
        /// </summary>
        protected string itemTypeDescriptionPlural = Properties.Resources.item_plural_Teacher;

        /// <summary>
        /// Indicates if the control is being loaded.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// Indicates if the control is loading poles.
        /// </summary>
        private bool isLoadingPoles = false;

        /// <summary>
        /// Right-clicked teacher. The teacher of the displayed context menu.
        /// </summary>
        private Teacher clickedTeacher = null;

        /// <summary>
        /// The teacher ID column index in the datagridview.
        /// </summary>
        private int columnIndexTeacherId;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ViewTeacherControl()
        {
            //set loading flag
            isLoading = true;

            //init ui components
            InitializeComponent();

            //create list of teachers
            teachers = new Dictionary<long, Teacher>();

            //create teacher data table
            CreateTeacherDataTable();

            //get teacher ID column index
            columnIndexTeacherId = dgvTeachers.Columns[TeacherId.Name].Index;

            //load combos
            //list statuses
            List<KeyValuePair<int, string>> statuses = new List<KeyValuePair<int, string>>();
            statuses.Add(new KeyValuePair<int, string>(
                -1, Properties.Resources.wordAll));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Active, Properties.Resources.ResourceManager.GetString("ItemStatus_Active")));
            statuses.Add(new KeyValuePair<int, string>(
                (int)ItemStatus.Inactive, Properties.Resources.ResourceManager.GetString("ItemStatus_Inactive")));
            mcbStatus.ValueMember = "Key";
            mcbStatus.DisplayMember = "Value";
            mcbStatus.DataSource = statuses;

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
        /// Get list of teachers.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public List<Teacher> ListTeachers
        {
            get
            {
                //lock list of teachers
                lock (teachers)
                {
                    return new List<Teacher>(teachers.Values);
                }
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
            //select teacher
            return "Teacher";
        }

        #endregion ISong Methods


        #region Public Methods ********************************************************

        /// <summary>
        /// Set columns visibility according to application settings.
        /// </summary>
        public void SetVisibleColumns()
        {
            //split visible column settings
            string[] columns = Manager.Settings.TeacherGridDisplayedColumns.Split(',');

            //hide all columns
            foreach (DataGridViewColumn dgColumn in dgvTeachers.Columns)
            {
                //hide column
                dgColumn.Visible = false;
            }

            //set invisible last index
            int lastIndex = dgvTeachers.Columns.Count - 1;

            //check each column and set visibility
            foreach (DataGridViewColumn dgvColumn in dgvTeachers.Columns)
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

                        //set column display index teacher
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
        /// Create Teacher data table.
        /// </summary>
        private void CreateTeacherDataTable()
        {
            //create data table
            dtTeachers = new DataTable();

            //TeacherId
            DataColumn dcTeacherId = new DataColumn("TeacherId", typeof(int));
            dtTeachers.Columns.Add(dcTeacherId);

            //Name
            DataColumn dcName = new DataColumn("Name", typeof(string));
            dtTeachers.Columns.Add(dcName);

            //Birthdate
            DataColumn dcBirthdate = new DataColumn("Birthdate", typeof(DateTime));
            dtTeachers.Columns.Add(dcBirthdate);

            //UserLogin
            DataColumn dcUserId = new DataColumn("UserLogin", typeof(string));
            dtTeachers.Columns.Add(dcUserId);

            //Poles
            DataColumn dcPoles = new DataColumn("Poles", typeof(string));
            dtTeachers.Columns.Add(dcPoles);

            //Location
            DataColumn dcLocation = new DataColumn("Location", typeof(string));
            dtTeachers.Columns.Add(dcLocation);

            //Phones
            DataColumn dcPhones = new DataColumn("Phones", typeof(string));
            dtTeachers.Columns.Add(dcPhones);
            
            //Email
            DataColumn dcEmail = new DataColumn("Email", typeof(string));
            dtTeachers.Columns.Add(dcEmail);

            //AcademicBackground
            DataColumn dcAcademicBackground = new DataColumn("AcademicBackground", typeof(string));
            dtTeachers.Columns.Add(dcAcademicBackground);

            //TeacherStatusName
            DataColumn dcTeacherStatus = new DataColumn("TeacherStatusName", typeof(string));
            dtTeachers.Columns.Add(dcTeacherStatus);

            //CreationTime
            DataColumn dcCreationTime = new DataColumn("CreationTime", typeof(DateTime));
            dtTeachers.Columns.Add(dcCreationTime);

            //InactivationTime
            DataColumn dcInactivationTime = new DataColumn("InactivationTime", typeof(DateTime));
            dtTeachers.Columns.Add(dcInactivationTime);

            //InactivationReason
            DataColumn dcInactivationReason = new DataColumn("InactivationReason", typeof(string));
            dtTeachers.Columns.Add(dcInactivationReason);

            //set primary key column
            dtTeachers.PrimaryKey = new DataColumn[] { dcTeacherId };
        }

        /// <summary>
        /// Display selected teachers.
        /// Clear currently displayed teachers before loading selected teachers.
        /// </summary>
        /// <param name="selectedTeachers">
        /// The selected teachers to be loaded.
        /// </param>
        private void DisplayTeachers(List<Teacher> selectedTeachers)
        {
            //lock list of teachers
            lock (this.teachers)
            {
                //clear list
                this.teachers.Clear();

                //reset last found teacher
                lastFoundTeacher = null;
            }

            //lock datatable of teachers
            lock (dtTeachers)
            {
                //clear datatable
                dtTeachers.Clear();
            }

            //check number of selected teachers
            if (selectedTeachers != null && selectedTeachers.Count > 0 &&
                selectedTeachers[0].Result == (int)SelectResult.Success)
            {
                //lock list of teachers
                lock (teachers)
                {
                    //add selected teachers
                    foreach (Teacher teacher in selectedTeachers)
                    {
                        //check if teacher is not in the list
                        if (!teachers.ContainsKey(teacher.TeacherId))
                        {
                            //add teacher to the list
                            teachers.Add(teacher.TeacherId, teacher);

                            //set last found teacher
                            lastFoundTeacher = teacher;
                        }
                        else
                        {
                            //should never happen
                            //log message
                            Manager.Log.WriteError(
                                "Error while loading teachers. Two teachers with same TeacherID " +
                                teacher.TeacherId + ".");

                            //throw exception
                            throw new ApplicationException(
                                "Error while loading teachers. Two teachers with same TeacherID " +
                                teacher.TeacherId + ".");
                        }
                    }
                }

                //lock data table of teachers
                lock (dtTeachers)
                {
                    //check each teacher in the list
                    foreach (Teacher teacher in ListTeachers)
                    {
                        //create, set and add teacher row
                        DataRow dr = dtTeachers.NewRow();
                        SetTeacherDataRow(dr, teacher);
                        dtTeachers.Rows.Add(dr);
                    }
                }
            }

            //refresh displayed UI
            RefreshUI(false);
        }

        /// <summary>
        /// Find teacher in the list of teachers.
        /// </summary>
        /// <param name="teacherID">
        /// The ID of the selected teacher.
        /// </param>
        /// <returns>
        /// The teacher of the selected teacher ID.
        /// Null if teacher was not found.
        /// </returns>
        private Teacher FindTeacher(long teacherID)
        {
            //lock list of teachers
            lock (teachers)
            {
                //check last found teacher
                if (lastFoundTeacher != null &&
                    lastFoundTeacher.TeacherId == teacherID)
                {
                    //same teacher
                    return lastFoundTeacher;
                }

                //try to find selected teacher
                teachers.TryGetValue(teacherID, out lastFoundTeacher);

                //return result
                return lastFoundTeacher;
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

                //option to create an all option
                bool createAllOption = true;

                //check if a teacher is viewing its register and classes
                if (Manager.LogonTeacher != null &&
                    !Manager.HasLogonPermission("Teacher.View"))
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
        /// Load and display filtered teachers.
        /// </summary>
        /// <returns>
        /// True if teachers were loaded.
        /// False otherwise.
        /// </returns>
        private bool LoadTeachers()
        {
            //filter and load teachers
            List<Teacher> filteredTeachers = null;

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
                //check if a teacher is viewing its register and classes
                if (Manager.LogonTeacher != null &&
                    !Manager.HasLogonPermission("Teacher.View"))
                {
                    //create list with only logon teacher
                    filteredTeachers = new List<Teacher>();
                    filteredTeachers.Add(Manager.LogonTeacher);
                }
                else
                {
                    //get list of teachers
                    filteredTeachers = songChannel.FindTeachersByFilter(
                    true, true, (int)mcbStatus.SelectedValue,
                    (int)mcbInstitution.SelectedValue, (int)mcbPole.SelectedValue);
                }

                //check result
                if (filteredTeachers[0].Result == (int)SelectResult.Empty)
                {
                    //no teacher was found
                    //clear list
                    filteredTeachers.Clear();
                }
                else if (filteredTeachers[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting teachers
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredTeachers[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredTeachers[0].ErrorMessage));

                    //could not load teachers
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

                //database error while getting teachers
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load teachers
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

            //sort teachers by name
            filteredTeachers.Sort((x, y) => x.Name.CompareTo(y.Name));

            //display filtered teachers
            DisplayTeachers(filteredTeachers);

            //sort teachers by name by default
            dgvTeachers.Sort(DisplayName, ListSortDirection.Ascending);

            //teachers were loaded
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
            if (dgvTeachers.DataSource == null)
            {
                //set source to datagrid
                dgvTeachers.DataSource = dtTeachers;
            }

            //check if last row should be displayed
            //and if the is at least one row
            if (displayLastRow && dgvTeachers.Rows.Count > 0)
            {
                //refresh grid by displaying last row
                dgvTeachers.FirstDisplayedScrollingRowIndex = (dgvTeachers.Rows.Count - 1);
            }

            //refresh grid
            dgvTeachers.Refresh();

            //set number of teachers
            mlblItemCount.Text = teachers.Count + " " +
                (teachers.Count > 1 ? itemTypeDescriptionPlural : itemTypeDescription);
        }

        /// <summary>
        /// Set data row with selected Teacher data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="teacher">The selected teacher.</param>
        private void SetTeacherDataRow(DataRow dataRow, Teacher teacher)
        {
            dataRow["TeacherId"] = teacher.TeacherId;
            dataRow["Name"] = teacher.Name;
            dataRow["Birthdate"] = teacher.Birthdate;
            dataRow["UserLogin"] = teacher.UserLogin;
            dataRow["Poles"] = GetTeacherPoles(teacher);
            dataRow["Location"] = teacher.State + " - " + teacher.City + ", " + teacher.District;
            dataRow["Phones"] = GetTeacherPhones(teacher);
            dataRow["Email"] = teacher.Email;
            dataRow["AcademicBackground"] = teacher.AcademicBackground;
            dataRow["TeacherStatusName"] = Properties.Resources.ResourceManager.GetString(
                "ItemStatus_" + ((ItemStatus)teacher.TeacherStatus).ToString());
            dataRow["CreationTime"] = teacher.CreationTime;
            dataRow["InactivationReason"] = teacher.InactivationReason;

            //set inactivation time
            if (teacher.InactivationTime != DateTime.MinValue)
                dataRow["InactivationTime"] = teacher.InactivationTime;
            else
                dataRow["InactivationTime"] = DBNull.Value;
        }

        /// <summary>
        /// Get string that represents the selected teacher phone numbers.
        /// </summary>
        /// <param name="teacher">
        /// The selected teacher.
        /// </param>
        /// <returns>
        /// A string with phone numbers.
        /// </returns>
        private string GetTeacherPhones(Teacher teacher)
        {
            //get teacher phones
            StringBuilder sbPhones = new StringBuilder(32);

            //check teacher phone
            if (teacher.Phone != null && teacher.Phone.Length > 0)
            {
                //add phone
                sbPhones.Append(teacher.Phone);
            }

            //check teacher mobile
            if (teacher.Mobile != null && teacher.Mobile.Length > 0)
            {
                //check if there is a previous number
                if (sbPhones.Length > 0)
                {
                    //add new line
                    sbPhones.AppendLine();
                }

                //add mobile
                sbPhones.Append(teacher.Mobile);
            }

            //return created text
            return sbPhones.ToString();
        }

        /// <summary>
        /// Get string that represents the selected teacher poles.
        /// </summary>
        /// <param name="teacher">
        /// The selected teacher.
        /// </param>
        /// <returns>
        /// A string with pole names.
        /// </returns>
        private string GetTeacherPoles(Teacher teacher)
        {
            //gather pole names
            StringBuilder poles = new StringBuilder(64);

            //check assigned pole names
            if (teacher.PoleNames != null && teacher.PoleNames.Count > 0)
            {
                //add each pole name
                foreach (string poleName in teacher.PoleNames)
                {
                    //add pole name
                    poles.Append(poleName);
                    poles.Append(", ");
                }

                //remove last ", "
                poles.Length -= 2;
            }

            //return created text
            return poles.ToString();
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Remove displayed teacher.
        /// </summary>
        /// <param name="teacherId">
        /// The ID of the teacher to be removed.
        /// </param>
        public void RemoveTeacher(int teacherId)
        {
            //lock list of teachers
            lock (teachers)
            {
                //check if teacher is not in the list
                if (!teachers.ContainsKey(teacherId))
                {
                    //no need to remove teacher
                    //exit
                    return;
                }

                //remove teacher
                teachers.Remove(teacherId);
            }

            //lock data table of teachers
            lock (dtTeachers)
            {
                //get displayed data row
                DataRow dr = dtTeachers.Rows.Find(teacherId);

                //remove displayed data row
                dtTeachers.Rows.Remove(dr);
            }

            //refresh user interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update the status of a displayed teacher.
        /// </summary>
        /// <param name="teacherId">
        /// The ID of the selected teacher.
        /// </param>
        /// <param name="teacherStatus">
        /// The updated status of the teacher.
        /// </param>
        /// <param name="inactivationTime">
        /// The time the teacher was inactivated.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the teacher is being inactivated.
        /// </param>
        public void UpdateTeacher(int teacherId, int teacherStatus,
            DateTime inactivationTime, string inactivationReason)
        {
            //the teacher to be updated
            Teacher teacher = null;

            //lock list of teachers
            lock (teachers)
            {
                //try to find teacher
                if (!teachers.TryGetValue(teacherId, out teacher))
                {
                    //teacher was not found
                    //no need to update teacher
                    //exit
                    return;
                }
            }

            //update status
            teacher.TeacherStatus = teacherStatus;

            //update inactivation
            teacher.InactivationTime = inactivationTime;
            teacher.InactivationReason = inactivationReason;

            //update displayed teacher
            UpdateTeacher(teacher, null);
        }

        /// <summary>
        /// Update a displayed teacher. 
        /// Add teacher if it is a new teacher.
        /// </summary>
        /// <param name="teacher">
        /// The updated teacher.
        /// </param>
        /// <param name="poleIds">
        /// The ID list of assigned poles.
        /// </param>
        public void UpdateTeacher(Teacher teacher, List<int> poleIds)
        {
            //check teacher should be displayed
            //status filter
            if (mcbStatus.SelectedIndex > 0 &&
                (int)mcbStatus.SelectedValue != teacher.TeacherStatus)
            {
                //teacher should not be displayed
                //remove teacher if it is being displayed
                RemoveTeacher(teacher.TeacherId);

                //exit
                return;
            }

            //pole filter
            if (mcbPole.SelectedIndex > 0 && poleIds != null)
            {
                //check if list contains selected pole
                if (!poleIds.Contains((int)mcbPole.SelectedValue))
                {
                    //teacher should not be displayed
                    //remove teacher if it is being displayed
                    RemoveTeacher(teacher.TeacherId);

                    //exit
                    return;
                }
            }

            ////institution filter
            ////no pole should be selected
            //if (mcbPole.SelectedIndex == -1 && 
            //    mcbInstitution.SelectedIndex > 0 &&
            //    (int)mcbInstitution.SelectedValue != teacher.InstitutionId)
            //{
            //    //teacher should not be displayed
            //    //remove teacher if it is being displayed
            //    RemoveTeacher(teacher.TeacherId);

            //    //exit
            //    return;
            //}

            //lock list of teachers
            lock (teachers)
            {
                //set teacher
                teachers[teacher.TeacherId] = teacher;
            }

            //lock data table of teachers
            lock (dtTeachers)
            {
                //get displayed data row
                DataRow dr = dtTeachers.Rows.Find(teacher.TeacherId);

                //check if there was no data row yet
                if (dr == null)
                {
                    //create, set and add data row
                    dr = dtTeachers.NewRow();
                    SetTeacherDataRow(dr, teacher);
                    dtTeachers.Rows.Add(dr);
                }
                else
                {
                    //set data
                    SetTeacherDataRow(dr, teacher);
                }
            }

            //refresh user interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed teachers with user data.
        /// </summary>
        /// <param name="user">
        /// The updated user.
        /// </param>
        public void UpdateUser(User user)
        {
            //gather list of updated teachers
            List<Teacher> updatedTeachers = new List<Teacher>();

            //lock list
            lock (teachers)
            {
                //check all displayed teachers
                foreach (Teacher teacher in teachers.Values)
                {
                    //check teacher user
                    if (teacher.UserId == user.UserId &&
                        !teacher.UserLogin.Equals(user.Login))
                    {
                        //update teacher
                        teacher.UserLogin = user.Login;

                        //add teacher to the list of updated teachers
                        updatedTeachers.Add(teacher);
                    }
                }
            }

            //check result
            if (updatedTeachers.Count == 0)
            {
                //no teacher was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of teachers
            lock (dtTeachers)
            {
                //check each updated role
                foreach (Teacher teacher in updatedTeachers)
                {
                    //get displayed data row
                    DataRow dr = dtTeachers.Rows.Find(teacher.TeacherId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetTeacherDataRow(dr, teacher);
                    }
                }
            }

            //refresh user interface
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
                dgvTeachers.ColumnHeadersDefaultCellStyle.Font =
                    MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
                dgvTeachers.DefaultCellStyle.Font =
                    MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            }
            else if (e.PropertyName.Equals("TeacherGridDisplayedColumns"))
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
        private void ViewTeacherControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //avoid auto generated columns
            dgvTeachers.AutoGenerateColumns = false;

            //set fonts
            dgvTeachers.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvTeachers.DefaultCellStyle.Font =
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

            //clear number of teachers
            mlblItemCount.Text = string.Empty;

            //reset loading flag
            isLoading = false;

            //load data for the first time by selecting status
            //select active for status filter
            mcbStatus.SelectedIndex = 1;

            //check if a teacher is viewing its register and classes
            if (Manager.LogonTeacher != null &&
                !Manager.HasLogonPermission("Teacher.View"))
            {
                //select all for status filter
                mcbStatus.SelectedIndex = 0;

                //disable status filter
                mcbStatus.Enabled = false;

                //hide other filters
                mlblInstitution.Visible = false;
                mcbInstitution.Visible = false;
                mlblPole.Visible = false;
                mcbPole.Visible = false;
            }
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

            //load teachers
            LoadTeachers();
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

            //load teachers
            LoadTeachers();
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

            //load teachers
            LoadTeachers();
        }

        /// <summary>
        /// Edit tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlEdit_Click(object sender, EventArgs e)
        {
            //create control
            RegisterTeacherControl registerControl =
                new UI.Controls.RegisterTeacherControl();
            registerControl.ParentControl = this;

            //check if there is any selected teacher
            if (dgvTeachers.SelectedCells.Count > 0)
            {
                //select first selected teacher in the register control
                registerControl.FirstSelectedId =
                    (int)dgvTeachers.Rows[dgvTeachers.SelectedCells[0].RowIndex].Cells[columnIndexTeacherId].Value;
            }

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Teachers datagridview mouse up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTeachers_MouseUp(object sender, MouseEventArgs e)
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
            DataGridView.HitTestInfo hitInfo = dgvTeachers.HitTest(e.X, e.Y);

            //check selected row index
            if (hitInfo.RowIndex < 0)
            {
                //no row selected
                //exit
                return;
            }

            //check if there are selected rows and if teacher clicked on them
            if (dgvTeachers.SelectedRows.Count > 0 &&
                dgvTeachers.Rows[hitInfo.RowIndex].Selected != true)
            {
                //teacher did not click in the selected rows
                //clear selection
                dgvTeachers.ClearSelection();

                //select clicked row
                dgvTeachers.Rows[hitInfo.RowIndex].Selected = true;
            }
            //check if there are selected cells
            else if (dgvTeachers.SelectedCells.Count > 0)
            {
                //get list of selected cell rows
                HashSet<int> selectedRowIndexes = new HashSet<int>();

                //check each selected cell
                foreach (DataGridViewCell cell in dgvTeachers.SelectedCells)
                {
                    //add cell row index to the list
                    selectedRowIndexes.Add(cell.RowIndex);
                }

                //check if teacher clicked on a row of a selected cell
                if (selectedRowIndexes.Contains(hitInfo.RowIndex))
                {
                    //teacher clicked on a row of a selected cell
                    //select all rows
                    foreach (int selectedRowIndex in selectedRowIndexes)
                    {
                        //select row
                        dgvTeachers.Rows[selectedRowIndex].Selected = true;
                    }
                }
                else
                {
                    //teacher did not click on a row of a selected cell
                    //clear selected cells
                    dgvTeachers.ClearSelection();

                    //select clicked row
                    dgvTeachers.Rows[hitInfo.RowIndex].Selected = true;
                }
            }
            else
            {
                //select clicked row
                dgvTeachers.Rows[hitInfo.RowIndex].Selected = true;
            }

            //get selected or clicked teacher
            clickedTeacher = null;

            //check if there is a selected teacher
            if (dgvTeachers.SelectedRows.Count > 0)
            {
                //there is one or more teachers selected
                //get first selected teacher
                for (int index = 0; index < dgvTeachers.SelectedRows.Count; index++)
                {
                    //get teacher using its teacher id
                    int teacherId = (int)dgvTeachers.SelectedRows[index].Cells[columnIndexTeacherId].Value;
                    Teacher teacher = FindTeacher(teacherId);

                    //check result
                    if (teacher != null)
                    {
                        //add teacher to list
                        clickedTeacher = teacher;

                        //exit loop
                        break;
                    }
                }

                //check result
                if (clickedTeacher == null)
                {
                    //no teacher was found
                    //should never happen
                    //do not display context menu
                    //exit
                    return;
                }
            }
            else
            {
                //there is no teacher selected
                //should never happen
                //do not display context menu
                //exit
                return;
            }

            //set context menu items visibility
            if (dgvTeachers.SelectedRows.Count == 1)
            {
                //display view menu items according to permissions
                mnuViewTeacher.Visible = true;
                mnuViewUser.Visible = Manager.HasLogonPermission("User.View");
                tssSeparator.Visible = true;

                //display impersonate option
                mnuImpersonateTeacher.Visible = Manager.ImpersonatingUser == null &&
                    Manager.HasLogonPermission("User.Impersonate");
                tssSeparatorImpersonate.Visible = Manager.ImpersonatingUser == null &&
                    Manager.HasLogonPermission("User.Impersonate");
            }
            else
            {
                //hide view menu items
                mnuViewTeacher.Visible = false;
                mnuViewUser.Visible = false;
                tssSeparator.Visible = false;

                //hide impersonate option
                mnuImpersonateTeacher.Visible = false;
                tssSeparatorImpersonate.Visible = false;
            }

            //show teacher context menu on the clicked point
            mcmTeacher.Show(this.dgvTeachers, p);
        }

        /// <summary>
        /// Impersonate teacher menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuImpersonateTeacher_Click(object sender, EventArgs e)
        {
            //check clicked teacher
            if (clickedTeacher == null)
            {
                //should never happen
                //exit
                return;
            }

            //let user impersonate teacher user
            //display impersonation confirmation
            Manager.MainForm.ConfirmAndImpersonateUser(
                clickedTeacher.UserId, clickedTeacher.Name);
        }

        /// <summary>
        /// View teacher menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewTeacher_Click(object sender, EventArgs e)
        {
            //check clicked teacher
            if (clickedTeacher == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected teacher
            RegisterTeacherControl registerControl =
                new UI.Controls.RegisterTeacherControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedTeacher.TeacherId;

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// View user menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewUser_Click(object sender, EventArgs e)
        {
            //check clicked user
            if (clickedTeacher == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected user
            RegisterUserControl registerControl =
                new UI.Controls.RegisterUserControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedTeacher.UserId;

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
            if (this.dgvTeachers.GetCellCount(DataGridViewElementStates.Selected) <= 0)
            {
                //should never happen
                //exit
                return;
            }

            try
            {
                //include header text 
                dgvTeachers.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                //copy selection to the clipboard
                Clipboard.SetDataObject(this.dgvTeachers.GetClipboardContent());
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

    } //end of class ViewTeacherControl

} //end of namespace PnT.SongClient.UI.Controls
