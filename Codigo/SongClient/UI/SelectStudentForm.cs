using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetroFramework;
using MetroFramework.Forms;
using PnT.SongDB.Logic;
using PnT.SongServer;
using PnT.SongClient.Logic;

namespace PnT.SongClient.UI
{
    /// <summary>
    /// Select student form.
    /// </summary>
    public partial class SelectStudentForm : MetroForm
    {

        #region Fields ****************************************************************

        /// <summary>
        /// Indicates if control is being loaded.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// The ID of the default pole.
        /// </summary>
        private int poleId = int.MinValue;

        /// <summary>
        /// Option to let user select a different pole.
        /// </summary>
        private bool poleSelectable = false;

        /// <summary>
        /// The selected student description.
        /// </summary>
        private IdDescriptionStatus selectedStudent = null;

        /// <summary>
        /// The list of all loaded student lists.
        /// Keep lists for better performance.
        /// </summary>
        private Dictionary<int, List<IdDescriptionStatus>> studentLists = null;

        #endregion Fields


        #region Constructor ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the default pole.
        /// </param>
        /// <param name="poleSelectable">
        /// True to let user select another pole.
        /// False otherwise.
        /// </param>
        public SelectStudentForm(int poleId, bool poleSelectable)
        {
            //set flag
            isLoading = true;

            //init ui components
            InitializeComponent();

            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set fields
            this.poleId = poleId;
            this.poleSelectable = poleSelectable;

            //create list of loaded students
            studentLists = new Dictionary<int, List<IdDescriptionStatus>>();

            //list poles
            ListPoles();
        }

        #endregion Constructor


        #region Properties ************************************************************

        /// <summary>
        /// Get the selected student description.
        /// </summary>
        public IdDescriptionStatus SelectedStudent
        {
            get
            {
                return selectedStudent;
            }
        }

        #endregion Properties


        #region Private Methods *******************************************************

        /// <summary>
        /// List poles into UI.
        /// </summary>
        private void ListPoles()
        {
            //set default empty list to UI
            mcbPole.ValueMember = "Id";
            mcbPole.DisplayMember = "Description";
            mcbPole.DataSource = null;

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
        /// List students into UI for selected pole.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// -1 to select all poles.
        /// </param>
        private void ListStudents(int poleId)
        {
            //set default empty list to UI
            mcbStudent.ValueMember = "Id";
            mcbStudent.DisplayMember = "Description";
            mcbStudent.DataSource = null;

            //check if there is a list of students is for selected pole
            if (studentLists.ContainsKey(poleId))
            {
                //set stored students to UI
                mcbStudent.ValueMember = "Id";
                mcbStudent.DisplayMember = "Description";
                mcbStudent.DataSource = studentLists[poleId];

                //exit
                return;
            }

            //load students
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
                //list of students to be displayed
                List<IdDescriptionStatus> students = null;

                //check selected pole
                if (poleId <= 0)
                {
                    //get list of all active students
                    students = songChannel.ListStudentsByStatus((int)ItemStatus.Active);
                }
                else
                {
                    //get list of pole active students
                    students = songChannel.ListStudentsByPole(
                        poleId, (int)ItemStatus.Active);
                }

                //check result
                if (students[0].Result == (int)SelectResult.Success)
                {
                    //sort students by description
                    students.Sort((x, y) => x.Description.CompareTo(y.Description));
                }
                else if (students[0].Result == (int)SelectResult.Empty)
                {
                    //no student is available
                    //clear list
                    students.Clear();
                }
                else if (students[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting students
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Student, students[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Student,
                        students[0].ErrorMessage));

                    //clear list
                    students.Clear();
                }

                //set students to UI
                mcbStudent.ValueMember = "Id";
                mcbStudent.DisplayMember = "Description";
                mcbStudent.DataSource = students;

                //store list for faster performance
                studentLists[poleId] = students;
            }
            catch (Exception ex)
            {
                //database error while getting students
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Student), ex);

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

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Form load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectStudentForm_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //check number of poles
            if (mcbPole.Items.Count == 0)
            {
                //no pole was found
                //should never happen
                //disable OK button
                mbtnOK.Enabled = false;

                //exit
                return;
            }

            //check default pole id
            if (poleId > 0)
            {
                //select default pole
                mcbPole.SelectedValue = poleId;
            }

            //check if there is no selected pole
            if (mcbPole.SelectedIndex < 0)
            {
                //check if there is a pole
                if (mcbPole.Items.Count > 0)
                {
                    //select first pole
                    mcbPole.SelectedIndex = 0;
                }
            }

            //enable pole control if user can select another pole
            mcbPole.Enabled = poleSelectable;

            //reset flag
            isLoading = false;

            //load students for selected pole
            mcbPole_SelectedIndexChanged(this, new EventArgs());
        }

        /// <summary>
        /// OK button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnOK_Click(object sender, EventArgs e)
        {
            //check selected student
            if (mcbStudent.SelectedIndex >= 0)
            {
                //set selected student
                selectedStudent = (IdDescriptionStatus)mcbStudent.SelectedItem;
            }

            //set dialog result to OK
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Selected pole index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbPole_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if control is being loaded
            if (isLoading)
            {
                //exit
                return;
            }

            //check selected pole
            if (mcbPole.SelectedIndex < 0)
            {
                //no pole is selected
                //exit
                return;
            }

            //list students for selected pole
            ListStudents((int)mcbPole.SelectedValue);
        }

        /// <summary>
        /// Selected student index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            //enable OK button if any student is selected
            mbtnOK.Enabled = (mcbStudent.SelectedIndex >= 0);
        }

        #endregion Event Handlers

    } //end of class SelectStudentForm

} //end of namespace PnT.SongClient.UI
