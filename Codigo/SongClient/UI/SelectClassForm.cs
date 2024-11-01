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
    /// Select class form.
    /// </summary>
    public partial class SelectClassForm : MetroForm
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
        private bool displayPoles = false;

        /// <summary>
        /// Option to let user select classes that are in progress.
        /// </summary>
        private bool displayInProgressClasses = false;

        /// <summary>
        /// The selected class.
        /// </summary>
        private Class selectedClass = null;

        /// <summary>
        /// The list of all loaded class lists.
        /// Keep lists for better performance.
        /// </summary>
        private Dictionary<int, List<Class>> classLists = null;

        #endregion Fields


        #region Constructor ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the default pole.
        /// </param>
        /// <param name="displayPoles">
        /// True to let user select another pole from the list of poles.
        /// False otherwise.
        /// </param>
        /// <param name="displayInProgressClasses">
        /// Option to let user select classes that are in progress.
        /// False to display only classes in registration status.
        /// </param>
        public SelectClassForm(int poleId, bool displayPoles, bool displayInProgressClasses)
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
            this.displayPoles = displayPoles;
            this.displayInProgressClasses = displayInProgressClasses;

            //create list of loaded classes
            classLists = new Dictionary<int, List<Class>>();

            //list poles
            ListPoles();
        }

        #endregion Constructor


        #region Properties ************************************************************

        /// <summary>
        /// Get the selected class description.
        /// </summary>
        public Class SelectedClass
        {
            get
            {
                return selectedClass;
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
        /// List classes into UI for selected pole.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// -1 to select all poles.
        /// </param>
        private void ListClasses(int poleId)
        {
            //set default empty list to UI
            mcbClass.ValueMember = "ClassId";
            mcbClass.DisplayMember = "Code";
            mcbClass.DataSource = null;

            //check if there is a list of classes is for selected pole
            if (classLists.ContainsKey(poleId))
            {
                //set stored classes to UI
                mcbClass.ValueMember = "ClassId";
                mcbClass.DisplayMember = "Code";
                mcbClass.DataSource = classLists[poleId];

                //exit
                return;
            }

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
                //list of classses to be displayed
                List<Class> classes = null;

                //check selected pole
                if (poleId <= 0)
                {
                    //get list of all active classes
                    classes = songChannel.FindClassesByFilter(
                        true, false, true, (int)ItemStatus.Active, -1, -1, -1, -1, -1, -1, -1);
                }
                else
                {
                    //get list of pole active classes
                    classes = songChannel.FindClassesByFilter(
                        true, false, true, (int)ItemStatus.Active, -1, -1, -1, -1, -1, poleId, -1);
                }

                //check result
                if (classes[0].Result == (int)SelectResult.Success)
                {
                    //check if classes in progress should be displayed too
                    if (displayInProgressClasses)
                    {
                        //filter classes
                        classes = classes.FindAll(
                            c => (int)c.ClassProgress == (int)ClassProgress.Registration ||
                                 (int)c.ClassProgress == (int)ClassProgress.InProgress);
                    }
                    else
                    {
                        //filter classes
                        classes = classes.FindAll(
                            c => (int)c.ClassProgress == (int)ClassProgress.Registration);
                    }

                    //sort classes by code
                    classes.Sort((x, y) => x.Code.CompareTo(y.Code));

                    //describe classes further
                    //use class code to store description
                    //check each class
                    foreach (Class classObj in classes)
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

                        //add class teacher
                        sbDescription.Append(classObj.TeacherName);
                        sbDescription.Append(" | ");

                        //add class level
                        sbDescription.Append(Properties.Resources.ResourceManager.GetString(
                            "ClassLevel_" + ((ClassLevel)classObj.ClassLevel).ToString()));
                        sbDescription.Append(" | ");

                        //add class days and start time
                        sbDescription.Append(sbDays.ToString());
                        sbDescription.Append(" ");
                        sbDescription.Append(classObj.StartTime.ToString("HH:mm"));

                        //set description to class code
                        classObj.Code += sbDescription.ToString();
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
                mcbClass.ValueMember = "ClassId";
                mcbClass.DisplayMember = "Code";
                mcbClass.DataSource = classes;

                //store list for faster performance
                classLists[poleId] = classes;
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

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Form load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectClassForm_Load(object sender, EventArgs e)
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
            mcbPole.Enabled = displayPoles;

            //reset flag
            isLoading = false;

            //load classes for selected pole
            mcbPole_SelectedIndexChanged(this, new EventArgs());
        }

        /// <summary>
        /// OK button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnOK_Click(object sender, EventArgs e)
        {
            //check selected class
            if (mcbClass.SelectedIndex >= 0)
            {
                //set selected class
                selectedClass = (Class)mcbClass.SelectedItem;

                //reset class code
                //check class code
                if (selectedClass.Code.IndexOf(" | ") >= 0)
                {
                    //remove added data
                    selectedClass.Code = selectedClass.Code.Substring(
                        0, selectedClass.Code.IndexOf(" | "));
                }
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

            //list classes for selected pole
            ListClasses((int)mcbPole.SelectedValue);
        }

        /// <summary>
        /// Selected class index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            //enable OK button if any class is selected
            mbtnOK.Enabled = (mcbClass.SelectedIndex >= 0);
        }

        #endregion Event Handlers

    } //end of class SelectClassForm

} //end of namespace PnT.SongClient.UI
