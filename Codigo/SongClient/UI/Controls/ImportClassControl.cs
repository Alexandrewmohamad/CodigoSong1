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
    /// Import classes from selected semester to target semester.
    /// </summary>
    public partial class ImportClassControl : UserControl, ISongControl
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
        /// The list of selected classes.
        /// </summary>
        private List<IdDescriptionStatus> selectedClasses = null;

        /// <summary>
        /// The list of available classes.
        /// </summary>
        private List<IdDescriptionStatus> availableClasses = null;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ImportClassControl()
        {
            //set loading flag
            isLoading = true;

            //init UI components
            InitializeComponent();

            //list semesters
            ListSemesters();

            //check if logged on user has an assigned institution
            if (Manager.LogonUser != null &&
                Manager.LogonUser.InstitutionId > 0)
            {
                //list poles
                ListPoles(Manager.LogonUser.InstitutionId);
            }
            else
            {
                //list poles
                ListPoles(-1);
            }

            //list registration options
            List<KeyValuePair<int, string>> options = new List<KeyValuePair<int, string>>();
            options.Add(new KeyValuePair<int, string>(0, Properties.Resources.ImportOption_NoRegistrations));
            options.Add(new KeyValuePair<int, string>(1, Properties.Resources.ImportOption_AllRegistrations));
            options.Add(new KeyValuePair<int, string>(2, Properties.Resources.ImportOption_AutoRenewalRegistrations));
            mcbRegistrationOption.DataSource = options;
            mcbRegistrationOption.ValueMember = "Key";
            mcbRegistrationOption.DisplayMember = "Value";
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
            //select class
            return "Class";
        }

        #endregion ISong Methods


        #region Private Methods *******************************************************

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

                //check selected institution
                if (institutionId <= 0)
                {
                    //create all option and add it to list
                    IdDescriptionStatus allOption = new IdDescriptionStatus(
                        -1, Properties.Resources.wordAll, 0);
                        poles.Insert(0, allOption);
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

                //check result
                if (semesters.Count > 1)
                {
                    //remove last semester
                    //can't import classes to a further semester 
                    //because it is the last one
                    semesters.RemoveAt(semesters.Count - 1);
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

                        //check if semester has not started yet
                        if (Manager.CurrentSemester.StartDate > DateTime.Today &&
                            Manager.CurrentSemester.SemesterId > 1)
                        {
                            //select previous semester
                            mcbSemester.SelectedValue = Manager.CurrentSemester.SemesterId - 1;
                        }
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
                //get list of classes for selected semester and pole
                filteredClasses = songChannel.FindClassesByFilter(
                    false, false, true,
                    (int)ItemStatus.Active, -1, -1, -1,
                    (int)mcbSemester.SelectedValue, -1, 
                    (int)mcbPole.SelectedValue, -1);

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
                        Properties.Resources.item_Class, filteredClasses[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        Properties.Resources.item_Class, filteredClasses[0].ErrorMessage));

                    //clear list
                    filteredClasses.Clear();
                }
            }
            catch (Exception ex)
            {
                //show error message
                MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelFilterItem, Properties.Resources.item_Class, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //database error while getting classes
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, Properties.Resources.item_Class, ex.Message));
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

            //gather list of class descriptions
            List<IdDescriptionStatus> classes = new List<IdDescriptionStatus>();

            //check each filtered class
            foreach (Class filteredClass in filteredClasses)
            {
                //create class description
                IdDescriptionStatus classObj = new IdDescriptionStatus(
                    filteredClass.ClassId,
                    Manager.GetClassDescription(filteredClass, true), 
                    (int)ItemStatus.Active);

                //add student description to list
                classes.Add(classObj);
            }

            //sort classes by name
            classes.Sort((s1, s2) => s1.Description.CompareTo(s2.Description));

            //set available classes
            availableClasses = classes;

            //set empty selected classes
            selectedClasses = new List<IdDescriptionStatus>(); 

            //display lists
            RefreshClassLists();

            //classes were loaded
            return true;
        }

        /// <summary>
        /// List target semesters into UI.
        /// </summary>
        private void LoadTargetSemesters()
        {
            //set default empty list to UI
            mcbTargetSemester.ValueMember = "Id";
            mcbTargetSemester.DisplayMember = "Description";
            mcbTargetSemester.DataSource = new List<IdDescriptionStatus>();

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
                    //remove old semesters
                    semesters = semesters.FindAll(
                        s => s.Id >= Manager.CurrentSemester.SemesterId);
                }

                //check selected semester
                if (mcbSemester.SelectedIndex >= 0)
                {
                    //remove older semesters
                    semesters = semesters.FindAll(
                        s => s.Id > (int)mcbSemester.SelectedValue);
                }

                //set semesters to UI
                mcbTargetSemester.ValueMember = "Id";
                mcbTargetSemester.DisplayMember = "Description";
                mcbTargetSemester.DataSource = semesters;

                //check if there is any semester to be selected 
                //other than all option
                if (semesters.Count > 1)
                {
                    //select next semester
                    mcbTargetSemester.SelectedIndex = 0;
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
        /// Refresh displayed class lists and its items.
        /// </summary>
        private void RefreshClassLists()
        {
            //refresh displayed list of selected classes
            lbSelectedClasses.DataSource = null;
            lbSelectedClasses.DisplayMember = "Description";
            lbSelectedClasses.ValueMember = "Id";
            lbSelectedClasses.DataSource = selectedClasses;
            lbSelectedClasses.SelectedIndex = -1;
            lbSelectedClasses_SelectedIndexChanged(this, new EventArgs());

            //refresh displayed list of available classes
            lbAvailableClasses.DataSource = null;
            lbAvailableClasses.DisplayMember = "Description";
            lbAvailableClasses.ValueMember = "Id";
            lbAvailableClasses.DataSource = availableClasses;
            lbAvailableClasses.SelectedIndex = -1;
            lbAvailableClasses_SelectedIndexChanged(this, new EventArgs());
        }

        #endregion Private Methods


        #region UI Event Handlers *****************************************************

        /// <summary>
        /// Control load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportClassControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set font to UI controls
            lbAvailableClasses.Font = MetroFramework.MetroFonts.DefaultLight(12.0F);
            lbSelectedClasses.Font = MetroFramework.MetroFonts.DefaultLight(12.0F);

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

            //load target semesters
            LoadTargetSemesters();

            //load classes
            LoadClasses();
        }

        /// <summary>
        /// Available classes listbox selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbAvailableClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check the number of selected available classes
            if (lbAvailableClasses.SelectedIndex >= 0)
            {
                //enable button
                mbtnAddClasses.Enabled = true;
                mbtnAddClasses.BackgroundImage = Properties.Resources.IconMoveRightOne;
            }
            else
            {
                //disable button
                mbtnAddClasses.Enabled = false;
                mbtnAddClasses.BackgroundImage = Properties.Resources.IconMoveRightOneDisabled;
            }

            //check the number of available classes
            if (lbAvailableClasses.Items.Count > 0)
            {
                //enable button
                mbtnAddAllClasses.Enabled = true;
                mbtnAddAllClasses.BackgroundImage = Properties.Resources.IconMoveRightAll;
            }
            else
            {
                //disable button
                mbtnAddAllClasses.Enabled = false;
                mbtnAddAllClasses.BackgroundImage = Properties.Resources.IconMoveRightAllDisabled;
            }
        }

        /// <summary>
        /// Selected classes listbox selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbSelectedClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check number of selected selected classes
            if (lbSelectedClasses.SelectedIndex >= 0)
            {
                //enable button
                mbtnRemoveClasses.Enabled = true;
                mbtnRemoveClasses.BackgroundImage = Properties.Resources.IconMoveLeftOne;
            }
            else
            {
                //disable button
                mbtnRemoveClasses.Enabled = false;
                mbtnRemoveClasses.BackgroundImage = Properties.Resources.IconMoveLeftOneDisabled;
            }

            //check number of available classes
            if (lbSelectedClasses.Items.Count > 0)
            {
                //enable button
                mbtnRemoveAllClasses.Enabled = true;
                mbtnRemoveAllClasses.BackgroundImage = Properties.Resources.IconMoveLeftAll;
            }
            else
            {
                //disable button
                mbtnRemoveAllClasses.Enabled = false;
                mbtnRemoveAllClasses.BackgroundImage = Properties.Resources.IconMoveLeftAllDisabled;
            }

            //enable generate tile if any class is selected
            mtlImportClasses.Enabled = (lbSelectedClasses.Items.Count > 0);
        }

        /// <summary>
        /// Add classes button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddClasses_Click(object sender, EventArgs e)
        {
            //check if there is any selected available class
            if (lbAvailableClasses.SelectedItems.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbAvailableClasses_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected available class
            for (int i = 0; i < lbAvailableClasses.SelectedItems.Count; i++)
            {
                //get selected class
                IdDescriptionStatus classObj = (IdDescriptionStatus)lbAvailableClasses.SelectedItems[i];

                //remove class from available classes
                availableClasses.Remove(classObj);

                //add class to selected classes and sort list
                selectedClasses.Add(classObj);
            }

            //sort selected class list
            selectedClasses.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed class lists
            RefreshClassLists();
        }

        /// <summary>
        /// Add all classes button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddAllClasses_Click(object sender, EventArgs e)
        {
            //check if there is any available class
            if (lbAvailableClasses.Items.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbAvailableClasses_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each available class
            for (int i = 0; i < lbAvailableClasses.Items.Count; i++)
            {
                //get available class
                IdDescriptionStatus classObj = (IdDescriptionStatus)lbAvailableClasses.Items[i];

                //remove class from available classes
                availableClasses.Remove(classObj);

                //add class to selected classes and sort list
                selectedClasses.Add(classObj);
            }

            //sort selected class list
            selectedClasses.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed class lists
            RefreshClassLists();
        }

        /// <summary>
        /// Button remove classes click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnRemoveClasses_Click(object sender, EventArgs e)
        {
            //check if there is any selected selected class
            if (lbSelectedClasses.SelectedItems.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedClasses_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected selected class
            for (int i = 0; i < lbSelectedClasses.SelectedItems.Count; i++)
            {
                //get selected class
                IdDescriptionStatus classObj = (IdDescriptionStatus)lbSelectedClasses.SelectedItems[i];

                //remove class from selected classes
                selectedClasses.Remove(classObj);

                //add class to available classes and sort list
                availableClasses.Add(classObj);
            }

            //sort available class list
            availableClasses.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed class lists
            RefreshClassLists();
        }

        /// <summary>
        /// Button remove all classes click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnRemoveAllClasses_Click(object sender, EventArgs e)
        {
            //check if there is any selected class
            if (lbSelectedClasses.Items.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedClasses_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected class
            for (int i = 0; i < lbSelectedClasses.Items.Count; i++)
            {
                //get selected class
                IdDescriptionStatus classObj = (IdDescriptionStatus)lbSelectedClasses.Items[i];

                //remove class from selected classes
                selectedClasses.Remove(classObj);

                //add class to available classes and sort list
                availableClasses.Add(classObj);
            }

            //sort available class list
            availableClasses.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed class lists
            RefreshClassLists();
        }

        /// <summary>
        /// Import classes tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlImportClasses_Click(object sender, EventArgs e)
        {
            //check if there is no selected semester or no target semester
            if (mcbSemester.SelectedIndex < 0 ||
                mcbTargetSemester.SelectedIndex < 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedClasses_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check if there is no selected class
            if (lbSelectedClasses.Items.Count == 0 || selectedClasses.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedClasses_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }
            
            //get selected semester
            IdDescriptionStatus semester = (IdDescriptionStatus)mcbSemester.SelectedItem;
            
            //get selected target semester
            IdDescriptionStatus targetSemester = (IdDescriptionStatus)mcbTargetSemester.SelectedItem;

            //ask user to confirm
            if (MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgConfirmImportSemesterClasses,
                lbSelectedClasses.Items.Count, semester.Description, targetSemester.Description),
                Properties.Resources.titleImportClasses,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
            {
                //user cancelled operation
                //exit
                return;
            }

            //import selected classes
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
                //import classes and get result
                SaveResult saveResult = songChannel.ImportClasses(
                    semester, targetSemester, selectedClasses, 
                    (int)mcbRegistrationOption.SelectedValue);

                //check result
                if (saveResult.Result == (int)SelectResult.FatalError)
                {
                    //classes were not imported
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceSaveItem,
                        Properties.Resources.item_Class, saveResult.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceSaveItem,
                        Properties.Resources.item_Class, saveResult.ErrorMessage));
                }
                else
                {
                    //classes were imported
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.msgImportedSemesterClasses,
                        semester.Description, targetSemester.Description),
                        Properties.Resources.titleImportedClasses,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //log message
                    Manager.Log.WriteInfo(string.Format(
                        Properties.Resources.msgImportedSemesterClasses,
                        semester.Description, targetSemester.Description));

                    //open class control and display target semester classes
                    Manager.MainForm.DisplayClasses();
                }
            }
            catch (Exception ex)
            {
                //database error while getting users
                //write error
                Manager.Log.WriteException(Properties.Resources.errorChannelImportClasses, ex);

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

    } //end of class ImportClassControl

} //end of namespace PnT.SongClient.UI.Controls
