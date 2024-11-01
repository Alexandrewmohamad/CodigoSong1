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
    /// Generate printable report cards for selected semester and pole.
    /// </summary>
    public partial class GenerateReportCard : UserControl, ISongControl
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
        /// The list of selected students.
        /// </summary>
        private List<IdDescriptionStatus> selectedStudents = null;

        /// <summary>
        /// The list of available students.
        /// </summary>
        private List<IdDescriptionStatus> availableStudents = null;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenerateReportCard()
        {
            //set loading flag
            isLoading = true;

            //init UI components
            InitializeComponent();
            
            //list semesters
            ListSemesters();

            //list poles
            ListPoles();
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
            //select nothing
            return "";
        }

        #endregion ISong Methods


        #region Private Methods *******************************************************

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
                //get list of all active poles
                List<IdDescriptionStatus> poles = songChannel.ListPolesByStatus((int)ItemStatus.Active);

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

                //check current semester
                if (Manager.CurrentSemester.Result == (int)SelectResult.Success)
                {
                    //remove new semesters by selecting old ones
                    semesters = semesters.FindAll(
                        s => s.Id <= Manager.CurrentSemester.SemesterId);
                }

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
        /// Load and display filtered students.
        /// </summary>
        /// <returns>
        /// True if students were loaded.
        /// False otherwise.
        /// </returns>
        private bool LoadStudents()
        {
            //filter and load registrations
            List<Registration> filteredRegistrations = null;

            //only consider students from selected pole
            List<IdDescriptionStatus> poleStudents = null;

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
                //get list of registrations for selected semester and pole
                filteredRegistrations = songChannel.FindRegistrationsByFilter(
                    false, true, false, false, 
                    (int)ItemStatus.Active, 
                    (int)mcbSemester.SelectedValue,
                    -1, 
                    (int)mcbPole.SelectedValue, 
                    -1, 
                    -1);

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
                        Properties.Resources.item_Registration, filteredRegistrations[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        Properties.Resources.item_Registration, filteredRegistrations[0].ErrorMessage));

                    //clear list
                    filteredRegistrations.Clear();
                }
                
                //get list of all pole students
                poleStudents = songChannel.ListStudentsByPole((int)mcbPole.SelectedValue, -1);

                //check result
                if (poleStudents[0].Result == (int)SelectResult.Empty)
                {
                    //no student is available
                    //clear list
                    poleStudents.Clear();
                }
                else if (poleStudents[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting students
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Student, poleStudents[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Student,
                        poleStudents[0].ErrorMessage));

                    //clear list
                    poleStudents.Clear();
                }
            }
            catch (Exception ex)
            {
                //show error message
                MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelFilterItem, 
                    Properties.Resources.item_ReportCard, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //database error while getting registrations
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, 
                    Properties.Resources.item_ReportCard, ex.Message));
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

            //gather list of student descriptions
            List<IdDescriptionStatus> students = new List<IdDescriptionStatus>();

            //check each filtered registration
            foreach (Registration registration in filteredRegistrations)
            {
                //check if registration student is not from selected pole
                if (poleStudents.Find(s => s.Id == registration.StudentId) == null)
                {
                    //student is not from selected pole
                    //skip student
                    continue;
                }

                //check if registration student is not in the list yet
                if (students.Find(s => s.Id == registration.StudentId) == null)
                {
                    //create student description
                    IdDescriptionStatus student = new IdDescriptionStatus(
                        registration.StudentId, registration.StudentName, (int)ItemStatus.Active);

                    //add student description to list
                    students.Add(student);
                }
            }

            //sort students by name
            students.Sort((s1, s2) => s1.Description.CompareTo(s2.Description));

            //set selected students
            selectedStudents = students;

            //set empty available students
            availableStudents = new List<IdDescriptionStatus>();

            //display lists
            RefreshStudentLists();

            //registrations were loaded
            return true;
        }

        /// <summary>
        /// Refresh displayed student lists and its items.
        /// </summary>
        private void RefreshStudentLists()
        {
            //refresh displayed list of selected students
            lbSelectedStudents.DataSource = null;
            lbSelectedStudents.DisplayMember = "Description";
            lbSelectedStudents.ValueMember = "Id";
            lbSelectedStudents.DataSource = selectedStudents;
            lbSelectedStudents.SelectedIndex = -1;
            lbSelectedStudents_SelectedIndexChanged(this, new EventArgs());

            //refresh displayed list of available students
            lbAvailableStudents.DataSource = null;
            lbAvailableStudents.DisplayMember = "Description";
            lbAvailableStudents.ValueMember = "Id";
            lbAvailableStudents.DataSource = availableStudents;
            lbAvailableStudents.SelectedIndex = -1;
            lbAvailableStudents_SelectedIndexChanged(this, new EventArgs());
        }

        #endregion Private Methods


        #region UI Event Handlers *****************************************************

        /// <summary>
        /// Control load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateReportCard_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set font to UI controls
            lbAvailableStudents.Font = MetroFramework.MetroFonts.DefaultLight(12.0F);
            lbSelectedStudents.Font = MetroFramework.MetroFonts.DefaultLight(12.0F);

            //reset loading flag
            isLoading = false;

            //load data for the first time by simulating semester selection
            mcbSemester_SelectedIndexChanged(this, new EventArgs());
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
        /// Available students listbox selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbAvailableStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check the number of selected available students
            if (lbAvailableStudents.SelectedIndex >= 0)
            {
                //enable button
                mbtnAddStudents.Enabled = true;
                mbtnAddStudents.BackgroundImage = Properties.Resources.IconMoveRightOne;
            }
            else
            {
                //disable button
                mbtnAddStudents.Enabled = false;
                mbtnAddStudents.BackgroundImage = Properties.Resources.IconMoveRightOneDisabled;
            }

            //check the number of available students
            if (lbAvailableStudents.Items.Count > 0)
            {
                //enable button
                mbtnAddAllStudents.Enabled = true;
                mbtnAddAllStudents.BackgroundImage = Properties.Resources.IconMoveRightAll;
            }
            else
            {
                //disable button
                mbtnAddAllStudents.Enabled = false;
                mbtnAddAllStudents.BackgroundImage = Properties.Resources.IconMoveRightAllDisabled;
            }
        }

        /// <summary>
        /// Selected students listbox selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbSelectedStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check number of selected selected students
            if (lbSelectedStudents.SelectedIndex >= 0)
            {
                //enable button
                mbtnRemoveStudents.Enabled = true;
                mbtnRemoveStudents.BackgroundImage = Properties.Resources.IconMoveLeftOne;
            }
            else
            {
                //disable button
                mbtnRemoveStudents.Enabled = false;
                mbtnRemoveStudents.BackgroundImage = Properties.Resources.IconMoveLeftOneDisabled;
            }

            //check number of available students
            if (lbSelectedStudents.Items.Count > 0)
            {
                //enable button
                mbtnRemoveAllStudents.Enabled = true;
                mbtnRemoveAllStudents.BackgroundImage = Properties.Resources.IconMoveLeftAll;
            }
            else
            {
                //disable button
                mbtnRemoveAllStudents.Enabled = false;
                mbtnRemoveAllStudents.BackgroundImage = Properties.Resources.IconMoveLeftAllDisabled;
            }

            //enable generate tile if any student is selected
            mtlReportCards.Enabled = (lbSelectedStudents.Items.Count > 0);
        }

        /// <summary>
        /// Add students button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddStudents_Click(object sender, EventArgs e)
        {
            //check if there is any selected available student
            if (lbAvailableStudents.SelectedItems.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbAvailableStudents_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected available student
            for (int i = 0; i < lbAvailableStudents.SelectedItems.Count; i++)
            {
                //get selected student
                IdDescriptionStatus student = (IdDescriptionStatus)lbAvailableStudents.SelectedItems[i];

                //remove student from available students
                availableStudents.Remove(student);

                //add student to selected students and sort list
                selectedStudents.Add(student);
            }

            //sort selected student list
            selectedStudents.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed student lists
            RefreshStudentLists();
        }

        /// <summary>
        /// Add all students button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddAllStudents_Click(object sender, EventArgs e)
        {
            //check if there is any available student
            if (lbAvailableStudents.Items.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbAvailableStudents_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each available student
            for (int i = 0; i < lbAvailableStudents.Items.Count; i++)
            {
                //get available student
                IdDescriptionStatus student = (IdDescriptionStatus)lbAvailableStudents.Items[i];

                //remove student from available students
                availableStudents.Remove(student);

                //add student to selected students and sort list
                selectedStudents.Add(student);
            }

            //sort selected student list
            selectedStudents.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed student lists
            RefreshStudentLists();
        }

        /// <summary>
        /// Button remove students click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnRemoveStudents_Click(object sender, EventArgs e)
        {
            //check if there is any selected selected student
            if (lbSelectedStudents.SelectedItems.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedStudents_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected selected student
            for (int i = 0; i < lbSelectedStudents.SelectedItems.Count; i++)
            {
                //get selected student
                IdDescriptionStatus student = (IdDescriptionStatus)lbSelectedStudents.SelectedItems[i];

                //remove student from selected students
                selectedStudents.Remove(student);

                //add student to available students and sort list
                availableStudents.Add(student);
            }

            //sort available student list
            availableStudents.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed student lists
            RefreshStudentLists();
        }

        /// <summary>
        /// Button remove all students click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnRemoveAllStudents_Click(object sender, EventArgs e)
        {
            //check if there is any selected student
            if (lbSelectedStudents.Items.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedStudents_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected student
            for (int i = 0; i < lbSelectedStudents.Items.Count; i++)
            {
                //get selected student
                IdDescriptionStatus student = (IdDescriptionStatus)lbSelectedStudents.Items[i];

                //remove student from selected students
                selectedStudents.Remove(student);

                //add student to available students and sort list
                availableStudents.Add(student);
            }

            //sort available student list
            availableStudents.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed student lists
            RefreshStudentLists();
        }

        /// <summary>
        /// Generate report cards tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlReportCards_Click(object sender, EventArgs e)
        {
            //check if there is no selected pole or no semester
            if (mcbPole.SelectedIndex < 0 ||
                mcbSemester.SelectedIndex < 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedStudents_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check if there is no selected student
            if (lbSelectedStudents.Items.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedStudents_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //get selected semester
            IdDescriptionStatus semester = (IdDescriptionStatus)mcbSemester.SelectedItem;

            //get selected pole
            IdDescriptionStatus pole = (IdDescriptionStatus)mcbPole.SelectedItem;

            //let user select file path
            //check number of selected students
            if (lbSelectedStudents.Items.Count > 1)
            {
                //set file default name for selected pole
                sfdReportCardsFile.FileName =
                    Properties.Resources.item_plural_ReportCard + 
                    " - " + semester.Description + " - " + pole.Description;
            }
            else
            {
                //only one student selected
                //get selected student
                IdDescriptionStatus student = (IdDescriptionStatus)lbSelectedStudents.Items[0];

                //set file default name for selected student
                sfdReportCardsFile.FileName =
                    Properties.Resources.item_plural_ReportCard +
                    " - " + semester.Description + " - " + student.Description;
            }

            //display file dialog
            if (sfdReportCardsFile.ShowDialog() != DialogResult.OK)
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
                //copy list of selected students
                List<IdDescriptionStatus> students = 
                    new List<IdDescriptionStatus>(selectedStudents);

                //gather registrations
                List <Registration> studentRegistrations = new List<Registration>();

                //gather attendances
                List<Attendance> studentAttendances = new List<Attendance>();

                //gather grades
                List<Grade> studentGrades = new List<Grade>();

                //check each selected student
                foreach (IdDescriptionStatus student in students)
                {
                    //get registrations for student
                    List<Registration> registrations = songChannel.FindRegistrationsByStudent(
                        false, false, student.Id, (int)ItemStatus.Active);
                    
                    //check result
                    if (registrations[0].Result == (int)SelectResult.Empty)
                    {
                        //student has no registration
                        //should never happen
                        //go to next student
                        continue;
                    }
                    else if (registrations[0].Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting registrations
                        //display message
                        MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.errorWebServiceLoadRegistrations,
                            Properties.Resources.item_Student, registrations[0].ErrorMessage),
                            Properties.Resources.titleWebServiceError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //log error
                        Manager.Log.WriteError(string.Format(
                            Properties.Resources.errorWebServiceLoadRegistrations,
                            Properties.Resources.item_Student, registrations[0].ErrorMessage));

                        //could not load data
                        //exit
                        return;
                    }

                    //filter registrations by selected semester
                    List<Registration> semesterRegistrations = registrations.FindAll(
                        r => r.Class.SemesterId == semester.Id);

                    //check result
                    if (semesterRegistrations == null || semesterRegistrations.Count == 0)
                    {
                        //student has no registration for current semester
                        //should never happen
                        //go to next student
                        continue;
                    }

                    //add student registrations to main list
                    studentRegistrations.AddRange(semesterRegistrations);

                    //check each registration
                    foreach (Registration registration in semesterRegistrations)
                    {
                        //find attendance for selected class
                        List<Attendance> attendances = songChannel.FindAttendancesByFilter(
                            false, false, registration.ClassId, student.Id);

                        //check result
                        if (attendances[0].Result == (int)SelectResult.Empty)
                        {
                            //no attendance for selected class
                            //go to next registration
                            continue;
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

                            //could not load data
                            return;
                        }

                        //add student class attendances to main list
                        studentAttendances.AddRange(attendances);
                    }

                    //get student grades for selected semester
                    List<Grade> grades = songChannel.FindGradesByFilter(
                        false, false, false, false, false, false,
                        -1, (int)GradeTarget.Student, -1, -1, semester.Id, 
                        DateTime.MinValue, -1, -1, -1, -1, student.Id, -1);

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

                        //could not load data
                        return;
                    }

                    //add student grades to main list
                    studentGrades.AddRange(grades);
                }

                //must generate roll call file and save it to file
                string errorMessage = string.Empty;

                //generate roll call file with data
                if (Manager.FileManager.GenerateReportCardFile(
                    sfdReportCardsFile.FileName, semester.Copy(), 
                    pole, students, studentRegistrations, 
                    studentAttendances, studentGrades, ref errorMessage))
                {
                    try
                    {
                        //file was created
                        //open file
                        System.Diagnostics.Process.Start(sfdReportCardsFile.FileName);
                    }
                    catch (Exception ex)
                    {
                        //could not start browser
                        Manager.Log.WriteException(
                            "Could not open report card file on default browser.", ex);
                    }
                }
                else
                {
                    //error while saving file
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorGenerateReportCardFile, errorMessage),
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
                    Properties.Resources.item_ReportCard), ex);

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

        #endregion UI Event Handlers

    } //end of class GenerateReportCard

} //end of namespace PnT.SongClient.UI.Controls
