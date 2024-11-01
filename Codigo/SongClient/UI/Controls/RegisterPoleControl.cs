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
    /// This control is used to manage pole registries in web service.
    /// Inherits from base register control.
    /// </summary>
    public partial class RegisterPoleControl : RegisterBaseControl
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
        /// The list of all teachers.
        /// </summary>
        private List<IdDescriptionStatus> allTeachers = null;

        /// <summary>
        /// The list of role teachers currently saved in database.
        /// </summary>
        private List<IdDescriptionStatus> databaseTeachers = null;

        /// <summary>
        /// The list of available teachers.
        /// </summary>
        private List<IdDescriptionStatus> availableTeachers = null;

        /// <summary>
        /// The list of role selected teachers.
        /// </summary>
        private List<IdDescriptionStatus> selectedTeachers = null;

        #endregion Fields


        #region Constructors **********************************************************


        public RegisterPoleControl() : base("Pole", Manager.Settings.HideInactivePoles)
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

            //pole cannot be deleted
            this.classHasDeletion = false;

            //set permissions
            this.allowAddItem = Manager.HasLogonPermission("Pole.Insert");
            this.allowEditItem = Manager.HasLogonPermission("Pole.Edit");
            this.allowDeleteItem = Manager.HasLogonPermission("Pole.Inactivate");

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

            //list institutions
            ListInstitutions();

            //load teachers
            LoadTeachers();
        }

        #endregion Constructors


        #region Properties ************************************************************

        #endregion Properties


        #region Private Methods *******************************************************

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
                //list of active institutions
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

                    //create list
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

                    //set institutions to UI
                    mcbInstitution.ValueMember = "Id";
                    mcbInstitution.DisplayMember = "Description";
                    mcbInstitution.DataSource = institutions;
                }
                else if (institutions[0].Result == (int)SelectResult.Empty)
                {
                    //no institution is available
                    //exit
                    return;
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

                    //could not get institutions
                    //exit
                    return;
                }
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
        /// Load all teachers.
        /// </summary>
        private void LoadTeachers()
        {
            //set default empty list
            allTeachers = new List<IdDescriptionStatus>();

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
                //get list of all active teachers
                List<IdDescriptionStatus> teachers = 
                    songChannel.ListTeachersByStatus((int)ItemStatus.Active);
                
                //check result
                if (teachers[0].Result == (int)SelectResult.Success)
                {
                    //sort teachers by description
                    teachers.Sort((x, y) => x.Description.CompareTo(y.Description));

                    //set teachers
                    allTeachers = teachers;
                }
                else if (teachers[0].Result == (int)SelectResult.Empty)
                {
                    //no teacher is available
                    //should never happen
                    //exit
                    return;
                }
                else if (teachers[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting teacherspole
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

                    //could not get teachers
                    //exit
                    return;
                }
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
        /// Refresh displayed teacher lists and its items.
        /// </summary>
        private void RefreshTeacherLists()
        {
            //refresh displayed list of selected teachers
            lbSelectedTeachers.DataSource = null;
            lbSelectedTeachers.DisplayMember = "Description";
            lbSelectedTeachers.ValueMember = "Id";
            lbSelectedTeachers.DataSource = selectedTeachers;
            lbSelectedTeachers.SelectedIndex = -1;

            //refresh displayed list of available teachers
            lbAvailableTeachers.DataSource = null;
            lbAvailableTeachers.DisplayMember = "Description";
            lbAvailableTeachers.ValueMember = "Id";
            lbAvailableTeachers.DataSource = availableTeachers;
            lbAvailableTeachers.SelectedIndex = -1;
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
            if (!ValidateRequiredField(mtxtName, mlblName.Text, mtbTabManager, tbGeneralData) ||
                !ValidateDescriptionField(mtxtName, mlblName.Text, mtbTabManager, tbGeneralData))
            {
                //invalid field
                return false;
            }

            //validate address
            if (!ValidateRequiredField(mtxtAddress, mlblAddress.Text, mtbTabManager, tbGeneralData))
            {
                //invalid field
                return false;
            }

            //validate district
            if (!ValidateRequiredField(mtxtDistrict, mlblDistrict.Text, mtbTabManager, tbGeneralData))
            {
                //invalid field
                return false;
            }

            //validate city
            if (!ValidateRequiredField(mtxtCity, mlblCity.Text, mtbTabManager, tbGeneralData))
            {
                //invalid field
                return false;
            }

            //validate zip code
            if (!ValidateRequiredField(mtxtZipCode, mlblZipCode.Text, mtbTabManager, tbGeneralData) ||
                !ValidateZipField(mtxtZipCode, mtbTabManager, tbGeneralData))
            {
                //invalid field
                return false;
            }

            //validate phone
            if (!ValidateRequiredField(mtxtPhone, mlblPhone.Text, mtbTabManager, tbGeneralData) ||
                !ValidatePhoneField(mtxtPhone, mtbTabManager, tbGeneralData))
            {
                //invalid field
                return false;
            }

            //validate mobile if set
            if (!mtxtMobile.Text.Equals("(  )     -") &&
                !ValidatePhoneField(mtxtMobile, mtbTabManager, tbGeneralData))
            {
                //invalid field
                return false;
            }

            //validate email if set
            if (mtxtEmail.Text.Length > 0 && !ValidateEmailField(mtxtEmail, mtbTabManager, tbGeneralData))
            {
                //invalid field
                return false;
            }

            //validate description
            if (!ValidateRequiredField(mtxtDescription, mlblDescription.Text, mtbTabManager, tbGeneralData))
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
            mtxtAddress.Text = string.Empty;
            mtxtDistrict.Text = string.Empty;
            mtxtCity.Text = string.Empty;
            mtxtZipCode.Text = string.Empty;
            mtxtPhone.Text = string.Empty;
            mtxtMobile.Text = string.Empty;
            mtxtEmail.Text = string.Empty;
            mtxtDescription.Text = string.Empty;

            //select Rio de Janeiro
            mcbState.SelectedIndex = 17;

            //check number of institutions
            if (mcbInstitution.Items.Count > 0)
            {
                //select first institution
                mcbInstitution.SelectedIndex = 0;
            }

            //display teachers
            availableTeachers = new List<IdDescriptionStatus>(allTeachers);
            selectedTeachers = new List<IdDescriptionStatus>();
            RefreshTeacherLists();
        }

        /// <summary>
        /// Dispose used resources from user control.
        /// </summary>
        public override void DisposeControl()
        {
            //update option to hide inactive items
            Manager.Settings.HideInactivePoles = this.hideInactiveItems;
        }

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        /// <returns></returns>
        public override string SelectMenuOption()
        {
            //select pole option
            return "Pole";
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
            mtxtAddress.Enabled = enable;
            mtxtDistrict.Enabled = enable;
            mtxtCity.Enabled = enable;
            mtxtZipCode.Enabled = enable;
            mtxtPhone.Enabled = enable;
            mtxtMobile.Enabled = enable;
            mtxtEmail.Enabled = enable;
            mtxtDescription.Enabled = enable;

            //set state list
            mcbState.Enabled = enable;

            //set institution list
            mcbInstitution.Enabled = enable;

            //call list event handlers and so setting buttons
            lbAvailableTeachers_SelectedIndexChanged(this, new EventArgs());
            lbSelectedTeachers_SelectedIndexChanged(this, new EventArgs());
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
                //get selected pole from web service
                Pole pole = songChannel.FindPole(itemId);

                //check result
                if (pole.Result == (int)SelectResult.Empty)
                {
                    //should never happen
                    //could not load data
                    return false;
                }
                else if (pole.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting pole
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, pole.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, pole.ErrorMessage));

                    //could not load data
                    return false;
                }

                //set selected pole ID
                selectedId = pole.PoleId;

                //select status
                mcbStatus.SelectedValue = pole.PoleStatus;

                //set inactivation fields
                inactivationReason = pole.InactivationReason;
                inactivationTime = pole.InactivationTime;

                //set text fields
                mtxtName.Text = pole.Name;
                mtxtAddress.Text = pole.Address;
                mtxtDistrict.Text = pole.District;
                mtxtCity.Text = pole.City;
                mtxtZipCode.Text = pole.ZipCode;
                mtxtPhone.Text = pole.Phone;
                mtxtMobile.Text = pole.Mobile;
                mtxtEmail.Text = pole.Email;
                mtxtDescription.Text = pole.Description;

                //set state
                mcbState.SelectedValue = pole.State;

                //check selected state
                if (mcbState.SelectedIndex < 0)
                {
                    //should never happen
                    //select default state
                    mcbState.SelectedIndex = 17;
                }

                //set institution
                mcbInstitution.SelectedValue = pole.InstitutionId;

                //check selected index
                if (mcbInstitution.SelectedIndex < 0)
                {
                    try
                    {
                        //institution is not available
                        //it might be inactive
                        //must load institution from web service
                        Institution institution = songChannel.FindInstitution(pole.InstitutionId);

                        //check result
                        if (institution.Result == (int)SelectResult.Success)
                        {
                            //add institution to list of institutions
                            List<IdDescriptionStatus> institutions = 
                                (List<IdDescriptionStatus>)mcbInstitution.DataSource;
                            institutions.Add(institution.GetDescription());

                            //update displayed list
                            mcbInstitution.DataSource = null;
                            mcbInstitution.ValueMember = "Id";
                            mcbInstitution.DisplayMember = "Description";
                            mcbInstitution.DataSource = institutions;

                            //set institution
                            mcbInstitution.SelectedValue = pole.InstitutionId;
                        }
                    }
                    catch
                    {
                        //do nothing
                    }
                }

                #region load teachers

                //get assigned teachers for selected pole
                List<IdDescriptionStatus> teachers = songChannel.ListTeachersByPole(pole.PoleId, -1);

                //check result
                if (teachers[0].Result == (int)SelectResult.Empty)
                {
                    //pole has no teacher
                    //clear list
                    teachers.Clear();
                }
                else if (teachers[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting teachers
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadTeachers,
                        itemTypeDescription, teachers[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadTeachers,
                        itemTypeDescription, teachers[0].ErrorMessage));

                    //could not load data
                    return false;
                }

                //sort teachers by description
                teachers.Sort((x, y) => x.Description.CompareTo(y.Description));

                //copy list and set database teachers
                databaseTeachers = new List<IdDescriptionStatus>(teachers);

                //set selected teachers
                selectedTeachers = teachers;

                //gather list of available teachers
                availableTeachers = new List<IdDescriptionStatus>();

                //check each teacher
                foreach (IdDescriptionStatus teacher in allTeachers)
                {
                    //check if teacher is not selected
                    if (selectedTeachers.Find(p => p.Id == teacher.Id) == null)
                    {
                        //add teacher
                        availableTeachers.Add(teacher);
                    }
                }

                //refresh displayed teacher lists
                RefreshTeacherLists();

                #endregion load teachers
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
        /// Load pole list from database.
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
                //list of poles
                List<IdDescriptionStatus> poles = null;

                //check if logged on user has an assigned institution
                if (Manager.LogonUser != null &&
                    Manager.LogonUser.InstitutionId > 0)
                {
                    //get list of poles for assigned institution
                    poles = songChannel.ListPolesByInstitution(
                        Manager.LogonUser.InstitutionId, -1);
                }
                else
                {
                    //get list of all poles
                    poles = songChannel.ListPoles();
                }
                
                //check result
                if (poles[0].Result == (int)SelectResult.Success)
                {
                    //poles were found
                    return poles;
                }
                else if (poles[0].Result == (int)SelectResult.Empty)
                {
                    //no pole is available
                    return null;
                }
                else if (poles[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting poles
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        itemTypeDescription, poles[0].ErrorMessage));

                    //could not get poles
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

            //could not get poles
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
                //inactivate selected pole and get result
                DeleteResult result = songChannel.InactivatePole(
                    SelectedItemId, reasonForm.InactivationReason);

                //check result
                if (result.Result == (int)SelectResult.Success)
                {
                    //item was inactivated
                    //check if there is a parent control
                    //and if it is an pole register control
                    if (parentControl != null && parentControl is ViewPoleControl)
                    {
                        //update pole to inactive in parent control
                        ((ViewPoleControl)parentControl).UpdatePole(
                            SelectedItemId, (int)ItemStatus.Inactive,
                            DateTime.Now, reasonForm.InactivationReason);
                    }

                    //register was inactivated
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

            //create an pole and set data
            Pole pole = new Pole();

            //set selected pole ID
            pole.PoleId = selectedId;

            //check selected status
            if (mcbStatus.SelectedIndex >= 0)
            {
                //set status
                pole.PoleStatus = (int)mcbStatus.SelectedValue;

                //check if selected status is inactive
                if ((int)mcbStatus.SelectedValue == (int)ItemStatus.Inactive)
                {
                    //create inactivation reason form
                    InactivationReasonForm reasonForm = new InactivationReasonForm(
                        itemTypeDescription, (int)mcbStatus.SelectedValue, inactivationReason);

                    //let user input an inactivation reason
                    reasonForm.ShowDialog(this);

                    //set inactivation reason with result
                    pole.InactivationReason = reasonForm.InactivationReason;

                    //set inactivation time
                    pole.InactivationTime = (inactivationTime != DateTime.MinValue) ?
                        inactivationTime : DateTime.Now;
                }
                else
                {
                    //reset inactivation
                    pole.InactivationReason = string.Empty;
                    pole.InactivationTime = DateTime.MinValue;
                }
            }
            else
            {
                //should never happen
                //set default status
                pole.PoleStatus = (int)ItemStatus.Active;

                //reset inactivation
                pole.InactivationTime = DateTime.MinValue;
                pole.InactivationReason = string.Empty;
            }

            //set text fields
            pole.Name = mtxtName.Text.Trim();
            pole.Address = mtxtAddress.Text.Trim();
            pole.District = mtxtDistrict.Text.Trim();
            pole.City = mtxtCity.Text.Trim();
            pole.State = mcbState.SelectedValue.ToString();
            pole.ZipCode = mtxtZipCode.Text.Trim();
            pole.Phone = mtxtPhone.Text.Trim();
            pole.Mobile = mtxtMobile.Text.Equals("(  )     -") ? string.Empty : mtxtMobile.Text.Trim();
            pole.Email = mtxtEmail.Text.Trim();
            pole.Description = mtxtDescription.Text.Trim();

            //set institution
            pole.InstitutionId = (int)mcbInstitution.SelectedValue;

            //set institution name to properly display pole in datagridview
            pole.InstitutionName = ((IdDescriptionStatus)mcbInstitution.SelectedItem).Description;

            //gather list of seleted teacher ids
            List<int> selectedTeacherIds = new List<int>();

            //check each selected teacher
            foreach (IdDescriptionStatus teacher in selectedTeachers)
            {
                //add teacher id
                selectedTeacherIds.Add(teacher.Id);
            }

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not save pole
                return null;
            }

            try
            {
                //save pole and get result
                SaveResult saveResult = songChannel.SavePole(pole, selectedTeacherIds);

                //check result
                if (saveResult.Result == (int)SelectResult.FatalError)
                {
                    //pole was not saved
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

                    //could not save pole
                    return null;
                }

                //set saved ID to pole ID
                pole.PoleId = saveResult.SavedId;

                //set selected ID
                selectedId = saveResult.SavedId;

                //check if there is a parent control and its type
                if (parentControl != null && parentControl is ViewPoleControl)
                {
                    //update pole in parent control
                    ((ViewPoleControl)parentControl).UpdatePole(pole);
                }
                else if (parentControl != null && parentControl is ViewClassControl)
                {
                    //update pole in parent control
                    ((ViewClassControl)parentControl).UpdatePole(pole);
                }
                else if (parentControl != null && parentControl is ViewInstrumentControl)
                {
                    //update pole in parent control
                    ((ViewInstrumentControl)parentControl).UpdatePole(pole);
                }
                else if (parentControl != null && parentControl is ViewRegistrationControl)
                {
                    //update pole in parent control
                    ((ViewRegistrationControl)parentControl).UpdatePole(pole);
                }
                else if (parentControl != null && parentControl is ViewStudentControl)
                {
                    //update pole in parent control
                    ((ViewStudentControl)parentControl).UpdatePole(pole);
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

            //pole was saved
            //return updated description
            return pole.GetDescription();
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
        private void RegisterPole_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set font to UI controls
            mtxtZipCode.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtPhone.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtMobile.Font = MetroFramework.MetroFonts.Default(13.0F);

            //display first tab
            mtbTabManager.SelectedIndex = 0;
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
        /// Available teachers listbox selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbAvailableTeachers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if status is consulting and teacher has selected an item
            if (this.Status == RegisterStatus.Consulting && lbAvailableTeachers.SelectedIndex >= 0)
            {
                //cannot select while consulting
                //clear selection
                lbAvailableTeachers.SelectedIndex = -1;

                //exit
                return;
            }

            //check if is not consulting and the number of selected available teachers
            if (this.Status != RegisterStatus.Consulting && lbAvailableTeachers.SelectedIndex >= 0)
            {
                //enable button
                mbtnAddTeachers.Enabled = true;
                mbtnAddTeachers.BackgroundImage = Properties.Resources.IconMoveRightOne;
            }
            else
            {
                //disable button
                mbtnAddTeachers.Enabled = false;
                mbtnAddTeachers.BackgroundImage = Properties.Resources.IconMoveRightOneDisabled;
            }
        }

        /// <summary>
        /// Selected teachers listbox selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbSelectedTeachers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if status is consulting and teacher has selected an item
            if (this.Status == RegisterStatus.Consulting && lbSelectedTeachers.SelectedIndex >= 0)
            {
                //cannot select while consulting
                //clear selection
                lbSelectedTeachers.SelectedIndex = -1;

                //exit
                return;
            }

            //check if is not consulting and the number of selected selected teachers
            if (this.Status != RegisterStatus.Consulting && lbSelectedTeachers.SelectedIndex > -1)
            {
                //enable button
                mbtnRemoveTeachers.Enabled = true;
                mbtnRemoveTeachers.BackgroundImage = Properties.Resources.IconMoveLeftOne;
            }
            else
            {
                //disable button
                mbtnRemoveTeachers.Enabled = false;
                mbtnRemoveTeachers.BackgroundImage = Properties.Resources.IconMoveLeftOneDisabled;
            }
        }

        /// <summary>
        /// General teachers listbox draw item event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbTeachers_DrawItem(object sender, DrawItemEventArgs e)
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

            //get teacher item
            IdDescriptionStatus teacher = (IdDescriptionStatus)senderList.Items[e.Index];

            //draw item
            e.DrawBackground();
            e.Graphics.DrawString(teacher.Description, e.Font,
                databaseTeachers != null && databaseTeachers.Contains(teacher) ? Brushes.Black : Brushes.DarkCyan,
                e.Bounds.Left, e.Bounds.Top);
            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Button add teachers click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddTeachers_Click(object sender, EventArgs e)
        {
            //check if there is any selected available teacher
            if (lbAvailableTeachers.SelectedItems.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbAvailableTeachers_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected available teacher
            for (int i = 0; i < lbAvailableTeachers.SelectedItems.Count; i++)
            {
                //get selected teacher
                IdDescriptionStatus teacher = (IdDescriptionStatus)lbAvailableTeachers.SelectedItems[i];

                //remove teacher from available teachers
                availableTeachers.Remove(teacher);

                //add teacher to selected teachers and sort list
                selectedTeachers.Add(teacher);
            }

            //sort selected teacher list
            selectedTeachers.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed teacher lists
            RefreshTeacherLists();
        }

        /// <summary>
        /// Button remove teachers click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnRemoveTeachers_Click(object sender, EventArgs e)
        {
            //check if there is any selected selected teacher
            if (lbSelectedTeachers.SelectedItems.Count == 0)
            {
                //should never happen
                //refresh buttons
                lbSelectedTeachers_SelectedIndexChanged(this, new EventArgs());

                //exit
                return;
            }

            //check each selected selected teacher
            for (int i = 0; i < lbSelectedTeachers.SelectedItems.Count; i++)
            {
                //get selected teacher
                IdDescriptionStatus teacher = (IdDescriptionStatus)lbSelectedTeachers.SelectedItems[i];

                //remove teacher from selected teachers
                selectedTeachers.Remove(teacher);

                //add teacher to available teachers and sort list
                availableTeachers.Add(teacher);
            }

            //sort available teacher list
            availableTeachers.Sort((x, y) => x.Description.CompareTo(y.Description));

            //refresh displayed teacher lists
            RefreshTeacherLists();
        }

        #endregion UI Event Handlers

    } //end of class RegisterPoleControl

} //end of namespace PnT.SongClient.UI.Controls
