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
    /// List and display students to user.
    /// </summary>
    public partial class ViewStudentControl : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The song child control.
        /// </summary>
        ISongControl childControl = null;

        /// <summary>
        /// List of students shown on the control.
        /// </summary>
        private Dictionary<long, Student> students = null;

        /// <summary>
        /// The last found student.
        /// Used to improve the find method.
        /// </summary>
        private Student lastFoundStudent = null;

        /// <summary>
        /// DataTable for students.
        /// </summary>
        private DataTable dtStudents = null;

        /// <summary>
        /// The item displayable name.
        /// </summary>
        private string itemTypeDescription = Properties.Resources.item_Student;

        /// <summary>
        /// The item displayable plural name.
        /// </summary>
        protected string itemTypeDescriptionPlural = Properties.Resources.item_plural_Student;

        /// <summary>
        /// Indicates if the control is being loaded.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// Indicates if the control is loading poles.
        /// </summary>
        private bool isLoadingPoles = false;

        /// <summary>
        /// Right-clicked student. The student of the displayed context menu.
        /// </summary>
        private Student clickedStudent = null;

        /// <summary>
        /// The student ID column index in the datagridview.
        /// </summary>
        private int columnIndexStudentId;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ViewStudentControl()
        {
            //set loading flag
            isLoading = true;

            //init ui components
            InitializeComponent();

            //create list of students
            students = new Dictionary<long, Student>();

            //create student data table
            CreateStudentDataTable();

            //get student ID column index
            columnIndexStudentId = dgvStudents.Columns[StudentId.Name].Index;

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
                (int)ItemStatus.Evaded, Properties.Resources.ResourceManager.GetString("ItemStatus_Evaded")));
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
        /// Get list of students.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public List<Student> ListStudents
        {
            get
            {
                //lock list of students
                lock (students)
                {
                    return new List<Student>(students.Values);
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
            //select student
            return "Student";
        }

        #endregion ISong Methods


        #region Public Methods ********************************************************

        /// <summary>
        /// Set columns visibility according to application settings.
        /// </summary>
        public void SetVisibleColumns()
        {
            //split visible column settings
            string[] columns = Manager.Settings.StudentGridDisplayedColumns.Split(',');

            //hide all columns
            foreach (DataGridViewColumn dgColumn in dgvStudents.Columns)
            {
                //hide column
                dgColumn.Visible = false;
            }

            //set invisible last index
            int lastIndex = dgvStudents.Columns.Count - 1;

            //check each column and set visibility
            foreach (DataGridViewColumn dgvColumn in dgvStudents.Columns)
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

                        //set column display index student
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
        /// Create Student data table.
        /// </summary>
        private void CreateStudentDataTable()
        {
            //create data table
            dtStudents = new DataTable();

            //StudentId
            DataColumn dcStudentId = new DataColumn("StudentId", typeof(int));
            dtStudents.Columns.Add(dcStudentId);

            //Name
            DataColumn dcName = new DataColumn("Name", typeof(string));
            dtStudents.Columns.Add(dcName);

            //Birthdate
            DataColumn dcBirthdate = new DataColumn("Birthdate", typeof(DateTime));
            dtStudents.Columns.Add(dcBirthdate);

            //HasDisability
            DataColumn dcHasDisability = new DataColumn("HasDisability", typeof(string));
            dtStudents.Columns.Add(dcHasDisability);

            //GuardianName
            DataColumn dcGuardianName = new DataColumn("GuardianName", typeof(string));
            dtStudents.Columns.Add(dcGuardianName);

            //PoleName
            DataColumn dcPoleName = new DataColumn("PoleName", typeof(string));
            dtStudents.Columns.Add(dcPoleName);

            //UserLogin
            DataColumn dcUserName = new DataColumn("UserLogin", typeof(string));
            dtStudents.Columns.Add(dcUserName);

            //Location
            DataColumn dcLocation = new DataColumn("Location", typeof(string));
            dtStudents.Columns.Add(dcLocation);

            //Phones
            DataColumn dcPhone = new DataColumn("Phones", typeof(string));
            dtStudents.Columns.Add(dcPhone);

            //Email
            DataColumn dcEmail = new DataColumn("Email", typeof(string));
            dtStudents.Columns.Add(dcEmail);

            //StudentStatusName
            DataColumn dcStudentStatus = new DataColumn("StudentStatusName", typeof(string));
            dtStudents.Columns.Add(dcStudentStatus);

            //CreationTime
            DataColumn dcCreationTime = new DataColumn("CreationTime", typeof(DateTime));
            dtStudents.Columns.Add(dcCreationTime);

            //InactivationTime
            DataColumn dcInactivationTime = new DataColumn("InactivationTime", typeof(DateTime));
            dtStudents.Columns.Add(dcInactivationTime);

            //InactivationReason
            DataColumn dcInactivationReason = new DataColumn("InactivationReason", typeof(string));
            dtStudents.Columns.Add(dcInactivationReason);

            //set primary key column
            dtStudents.PrimaryKey = new DataColumn[] { dcStudentId };
        }

        /// <summary>
        /// Display selected students.
        /// Clear currently displayed students before loading selected students.
        /// </summary>
        /// <param name="selectedStudents">
        /// The selected students to be loaded.
        /// </param>
        private void DisplayStudents(List<Student> selectedStudents)
        {
            //lock list of students
            lock (this.students)
            {
                //clear list
                this.students.Clear();

                //reset last found student
                lastFoundStudent = null;
            }

            //lock datatable of students
            lock (dtStudents)
            {
                //clear datatable
                dtStudents.Clear();
            }

            //check number of selected students
            if (selectedStudents != null && selectedStudents.Count > 0 &&
                selectedStudents[0].Result == (int)SelectResult.Success)
            {
                //lock list of students
                lock (students)
                {
                    //add selected students
                    foreach (Student student in selectedStudents)
                    {
                        //check if student is not in the list
                        if (!students.ContainsKey(student.StudentId))
                        {
                            //add student to the list
                            students.Add(student.StudentId, student);

                            //set last found student
                            lastFoundStudent = student;
                        }
                        else
                        {
                            //should never happen
                            //log message
                            Manager.Log.WriteError(
                                "Error while loading students. Two students with same StudentID " +
                                student.StudentId + ".");

                            //throw exception
                            throw new ApplicationException(
                                "Error while loading students. Two students with same StudentID " +
                                student.StudentId + ".");
                        }
                    }
                }

                //lock data table of students
                lock (dtStudents)
                {
                    //check each student in the list
                    foreach (Student student in ListStudents)
                    {
                        //create, set and add student row
                        DataRow dr = dtStudents.NewRow();
                        SetStudentDataRow(dr, student);
                        dtStudents.Rows.Add(dr);
                    }
                }
            }

            //refresh displayed UI
            RefreshUI(false);
        }

        /// <summary>
        /// Find student in the list of students.
        /// </summary>
        /// <param name="studentID">
        /// The ID of the selected student.
        /// </param>
        /// <returns>
        /// The student of the selected student ID.
        /// Null if student was not found.
        /// </returns>
        private Student FindStudent(long studentID)
        {
            //lock list of students
            lock (students)
            {
                //check last found student
                if (lastFoundStudent != null &&
                    lastFoundStudent.StudentId == studentID)
                {
                    //same student
                    return lastFoundStudent;
                }

                //try to find selected student
                students.TryGetValue(studentID, out lastFoundStudent);

                //return result
                return lastFoundStudent;
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
        /// Load and display filtered students.
        /// </summary>
        /// <returns>
        /// True if students were loaded.
        /// False otherwise.
        /// </returns>
        private bool LoadStudents()
        {
            //filter and load students
            List<Student> filteredStudents = null;

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
                //get list of students
                filteredStudents = songChannel.FindStudentsByFilter(
                    true, true, (int)mcbStatus.SelectedValue,
                    (int)mcbInstitution.SelectedValue, (int)mcbPole.SelectedValue);

                //check result
                if (filteredStudents[0].Result == (int)SelectResult.Empty)
                {
                    //no student was found
                    //clear list
                    filteredStudents.Clear();
                }
                else if (filteredStudents[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting students
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredStudents[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredStudents[0].ErrorMessage));

                    //could not load students
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

                //database error while getting students
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load students
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

            //sort students by name
            filteredStudents.Sort((x, y) => x.Name.CompareTo(y.Name));

            //display filtered students
            DisplayStudents(filteredStudents);

            //sort students by name by default
            dgvStudents.Sort(DisplayName, ListSortDirection.Ascending);

            //students were loaded
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
            //check if last row should be displayed
            //and if the is at least one row
            if (displayLastRow && dgvStudents.Rows.Count > 0)
            {
                //refresh grid by displaying last row
                dgvStudents.FirstDisplayedScrollingRowIndex = (dgvStudents.Rows.Count - 1);
            }

            //refresh grid
            dgvStudents.Refresh();

            //set number of students
            mlblItemCount.Text = students.Count + " " +
                (students.Count > 1 ? itemTypeDescriptionPlural : itemTypeDescription);
        }

        /// <summary>
        /// Set data row with selected Student data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="student">The selected student.</param>
        private void SetStudentDataRow(DataRow dataRow, Student student)
        {
            dataRow["StudentId"] = student.StudentId;
            dataRow["Name"] = student.Name;
            dataRow["Birthdate"] = student.Birthdate;
            dataRow["HasDisability"] = student.HasDisability ?
                Properties.Resources.wordYes : Properties.Resources.wordNo;
            dataRow["GuardianName"] = student.GuardianName;
            dataRow["PoleName"] = student.PoleName;
            dataRow["UserLogin"] = student.UserLogin;
            dataRow["Location"] = student.State + " - " + student.City + ", " + student.District;
            dataRow["Phones"] = GetStudentPhones(student);
            dataRow["Email"] = student.Email;
            dataRow["StudentStatusName"] = Properties.Resources.ResourceManager.GetString(
                "ItemStatus_" + ((ItemStatus)student.StudentStatus).ToString());
            dataRow["CreationTime"] = student.CreationTime;
            dataRow["InactivationReason"] = student.InactivationReason;

            //set inactivation time
            if (student.InactivationTime != DateTime.MinValue)
                dataRow["InactivationTime"] = student.InactivationTime;
            else
                dataRow["InactivationTime"] = DBNull.Value;
        }

        /// <summary>
        /// Get string that represents the selected student phone numbers.
        /// </summary>
        /// <param name="student">
        /// The selected student.
        /// </param>
        /// <returns>
        /// A string with phone numbers.
        /// </returns>
        private string GetStudentPhones(Student student)
        {
            //get student phones
            StringBuilder sbPhones = new StringBuilder(32);

            //check student phone
            if (student.Phone != null && student.Phone.Length > 0)
            {
                //add phone
                sbPhones.Append(student.Phone);
            }

            //check student mobile
            if (student.Mobile != null && student.Mobile.Length > 0)
            {
                //check if there is a previous number
                if (sbPhones.Length > 0)
                {
                    //add new line
                    sbPhones.AppendLine();
                }

                //add mobile
                sbPhones.Append(student.Mobile);
            }

            //return created text
            return sbPhones.ToString();
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Remove displayed student.
        /// </summary>
        /// <param name="studentId">
        /// The ID of the student to be removed.
        /// </param>
        public void RemoveStudent(int studentId)
        {
            //lock list of students
            lock (students)
            {
                //check if student is not in the list
                if (!students.ContainsKey(studentId))
                {
                    //no need to remove student
                    //exit
                    return;
                }

                //remove student
                students.Remove(studentId);
            }

            //lock data table of students
            lock (dtStudents)
            {
                //get displayed data row
                DataRow dr = dtStudents.Rows.Find(studentId);

                //remove displayed data row
                dtStudents.Rows.Remove(dr);
            }

            //refresh user interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update the status of a displayed student.
        /// </summary>
        /// <param name="studentId">
        /// The ID of the selected student.
        /// </param>
        /// <param name="studentStatus">
        /// The updated status of the student.
        /// </param>
        /// <param name="inactivationTime">
        /// The time the student was inactivated.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the student is being inactivated.
        /// </param>
        public void UpdateStudent(int studentId, int studentStatus,
            DateTime inactivationTime, string inactivationReason)
        {
            //the student to be updated
            Student student = null;

            //lock list of students
            lock (students)
            {
                //try to find student
                if (!students.TryGetValue(studentId, out student))
                {
                    //student was not found
                    //no need to update student
                    //exit
                    return;
                }
            }

            //update status
            student.StudentStatus = studentStatus;

            //update inactivation
            student.InactivationTime = inactivationTime;
            student.InactivationReason = inactivationReason;

            //update displayed student
            UpdateStudent(student);
        }

        /// <summary>
        /// Update a displayed student. 
        /// Add student if it is a new student.
        /// </summary>
        /// <param name="student">
        /// The updated student.
        /// </param>
        public void UpdateStudent(Student student)
        {
            //check student should be displayed
            //status filter
            if (mcbStatus.SelectedIndex > 0 &&
                (int)mcbStatus.SelectedValue != student.StudentStatus)
            {
                //student should not be displayed
                //remove student if it is being displayed
                RemoveStudent(student.StudentId);

                //exit
                return;
            }

            //pole filter
            if (mcbPole.SelectedIndex > 0 &&
                (int)mcbPole.SelectedValue != student.PoleId)
            {
                //student should not be displayed
                //remove student if it is being displayed
                RemoveStudent(student.StudentId);

                //exit
                return;
            }

            ////institution filter
            ////no pole should be selected
            //if (mcbPole.SelectedIndex == -1 && 
            //    mcbInstitution.SelectedIndex > 0 &&
            //    (int)mcbInstitution.SelectedValue != student.InstitutionId)
            //{
            //    //student should not be displayed
            //    //remove student if it is being displayed
            //    RemoveStudent(student.StudentId);

            //    //exit
            //    return;
            //}

            //lock list of students
            lock (students)
            {
                //set student
                students[student.StudentId] = student;
            }

            //lock data table of students
            lock (dtStudents)
            {
                //get displayed data row
                DataRow dr = dtStudents.Rows.Find(student.StudentId);

                //check if there was no data row yet
                if (dr == null)
                {
                    //create, set and add data row
                    dr = dtStudents.NewRow();
                    SetStudentDataRow(dr, student);
                    dtStudents.Rows.Add(dr);
                }
                else
                {
                    //set data
                    SetStudentDataRow(dr, student);
                }
            }

            //refresh user interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update displayed students with pole data.
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

            //gather list of updated students
            List<Student> updatedStudents = new List<Student>();

            //lock list
            lock (students)
            {
                //check all displayed students
                foreach (Student student in students.Values)
                {
                    //check student pole
                    if (student.PoleId == pole.PoleId &&
                        !student.PoleName.Equals(pole.Name))
                    {
                        //update student
                        student.PoleName = pole.Name;

                        //add student to he list of updated students
                        updatedStudents.Add(student);
                    }
                }
            }

            //check result
            if (updatedStudents.Count == 0)
            {
                //no student was updated
                //exit
                return;
            }

            //update data rows
            //lock data table of students
            lock (dtStudents)
            {
                //check each updated role
                foreach (Student student in updatedStudents)
                {
                    //get displayed data row
                    DataRow dr = dtStudents.Rows.Find(student.StudentId);

                    //check result
                    if (dr != null)
                    {
                        //set data
                        SetStudentDataRow(dr, student);
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
                dgvStudents.ColumnHeadersDefaultCellStyle.Font =
                    MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
                dgvStudents.DefaultCellStyle.Font =
                    MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            }
            else if (e.PropertyName.Equals("StudentGridDisplayedColumns"))
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
        private void ViewStudentControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //avoid auto generated columns
            dgvStudents.AutoGenerateColumns = false;

            //set fonts
            dgvStudents.ColumnHeadersDefaultCellStyle.Font =
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvStudents.DefaultCellStyle.Font =
                MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);

            //set visible columns
            SetVisibleColumns();

            //set source to datagrid
            dgvStudents.DataSource = dtStudents;

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

            //clear number of students
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

            //load students
            LoadStudents();
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

            //load students
            LoadStudents();
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

            //load students
            LoadStudents();
        }

        /// <summary>
        /// Edit tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlEdit_Click(object sender, EventArgs e)
        {
            //create control
            RegisterStudentControl registerControl =
                new UI.Controls.RegisterStudentControl();
            registerControl.ParentControl = this;

            //check if there is any selected student
            if (dgvStudents.SelectedCells.Count > 0)
            {
                //select first selected student in the register control
                registerControl.FirstSelectedId =
                    (int)dgvStudents.Rows[dgvStudents.SelectedCells[0].RowIndex].Cells[columnIndexStudentId].Value;
            }

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Students datagridview mouse up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvStudents_MouseUp(object sender, MouseEventArgs e)
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
            DataGridView.HitTestInfo hitInfo = dgvStudents.HitTest(e.X, e.Y);

            //check selected row index
            if (hitInfo.RowIndex < 0)
            {
                //no row selected
                //exit
                return;
            }

            //check if there are selected rows and if student clicked on them
            if (dgvStudents.SelectedRows.Count > 0 &&
                dgvStudents.Rows[hitInfo.RowIndex].Selected != true)
            {
                //student did not click in the selected rows
                //clear selection
                dgvStudents.ClearSelection();

                //select clicked row
                dgvStudents.Rows[hitInfo.RowIndex].Selected = true;
            }
            //check if there are selected cells
            else if (dgvStudents.SelectedCells.Count > 0)
            {
                //get list of selected cell rows
                HashSet<int> selectedRowIndexes = new HashSet<int>();

                //check each selected cell
                foreach (DataGridViewCell cell in dgvStudents.SelectedCells)
                {
                    //add cell row index to the list
                    selectedRowIndexes.Add(cell.RowIndex);
                }

                //check if student clicked on a row of a selected cell
                if (selectedRowIndexes.Contains(hitInfo.RowIndex))
                {
                    //student clicked on a row of a selected cell
                    //select all rows
                    foreach (int selectedRowIndex in selectedRowIndexes)
                    {
                        //select row
                        dgvStudents.Rows[selectedRowIndex].Selected = true;
                    }
                }
                else
                {
                    //student did not click on a row of a selected cell
                    //clear selected cells
                    dgvStudents.ClearSelection();

                    //select clicked row
                    dgvStudents.Rows[hitInfo.RowIndex].Selected = true;
                }
            }
            else
            {
                //select clicked row
                dgvStudents.Rows[hitInfo.RowIndex].Selected = true;
            }

            //get selected or clicked student
            clickedStudent = null;

            //check if there is a selected student
            if (dgvStudents.SelectedRows.Count > 0)
            {
                //there is one or more students selected
                //get first selected student
                for (int index = 0; index < dgvStudents.SelectedRows.Count; index++)
                {
                    //get student using its student id
                    int studentId = (int)dgvStudents.SelectedRows[index].Cells[columnIndexStudentId].Value;
                    Student student = FindStudent(studentId);

                    //check result
                    if (student != null)
                    {
                        //add student to list
                        clickedStudent = student;

                        //exit loop
                        break;
                    }
                }

                //check result
                if (clickedStudent == null)
                {
                    //no student was found
                    //should never happen
                    //do not display context menu
                    //exit
                    return;
                }
            }
            else
            {
                //there is no student selected
                //should never happen
                //do not display context menu
                //exit
                return;
            }

            //set context menu items visibility
            if (dgvStudents.SelectedRows.Count == 1)
            {
                //display view menu items according to permissions
                mnuViewStudent.Visible = true;
                mnuViewPole.Visible = Manager.HasLogonPermission("Pole.View");
                tssSeparator.Visible = true;
            }
            else
            {
                //hide view menu items
                mnuViewStudent.Visible = false;
                mnuViewPole.Visible = false;
                tssSeparator.Visible = false;
            }

            //show student context menu on the clicked point
            mcmStudent.Show(this.dgvStudents, p);
        }

        /// <summary>
        /// View student menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewStudent_Click(object sender, EventArgs e)
        {
            //check clicked student
            if (clickedStudent == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected student
            RegisterStudentControl registerControl =
                new UI.Controls.RegisterStudentControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedStudent.StudentId;

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
            //check clicked student
            if (clickedStudent == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected student pole
            RegisterPoleControl registerControl =
                new UI.Controls.RegisterPoleControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedStudent.PoleId;

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
            if (this.dgvStudents.GetCellCount(DataGridViewElementStates.Selected) <= 0)
            {
                //should never happen
                //exit
                return;
            }

            try
            {
                //include header text 
                dgvStudents.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                //copy selection to the clipboard
                Clipboard.SetDataObject(this.dgvStudents.GetClipboardContent());
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

    } //end of class ViewStudentControl

} //end of namespace PnT.SongClient.UI.Controls
