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
    /// This control is used to manage institution registries in web service.
    /// Inherits from base register control.
    /// </summary>
    public partial class RegisterInstitutionControl : RegisterBaseControl
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

        #endregion Fields


        #region Constructors **********************************************************


        public RegisterInstitutionControl() : base("Institution", Manager.Settings.HideInactiveInstitutions)
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

            //institution cannot be deleted
            this.classHasDeletion = false;

            //set permissions
            this.allowAddItem = Manager.HasLogonPermission("Institution.Insert");
            this.allowEditItem = Manager.HasLogonPermission("Institution.Edit");
            this.allowDeleteItem = Manager.HasLogonPermission("Institution.Inactivate");

            //load combos
            //load statuses
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

            //list coordinators
            ListCoordinators();
        }

        #endregion Constructors


        #region Properties ************************************************************

        #endregion Properties


        #region Private Methods *******************************************************

        /// <summary>
        /// List coordinators into UI.
        /// </summary>
        private void ListCoordinators()
        {
            //set default empty list to UI
            mcbCoordinator.ValueMember = "Id";
            mcbCoordinator.DisplayMember = "Description";
            mcbCoordinator.DataSource = new List<IdDescriptionStatus>();

            //load users that are coordinators
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
                //get list of active coordinator users
                List<IdDescriptionStatus> users =
                    songChannel.ListCoordinatorsByStatus((int)ItemStatus.Active);

                //check result
                if (users[0].Result == (int)SelectResult.Success)
                {
                    //sort users by description
                    users.Sort((x, y) => x.Description.CompareTo(y.Description));

                    //set users to UI
                    mcbCoordinator.ValueMember = "Id";
                    mcbCoordinator.DisplayMember = "Description";
                    mcbCoordinator.DataSource = users;
                }
                else if (users[0].Result == (int)SelectResult.Empty)
                {
                    //no user is available
                    //exit
                    return;
                }
                else if (users[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting users
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_User, users[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_User,
                        users[0].ErrorMessage));

                    //could not get users
                    //exit
                    return;
                }
            }
            catch (Exception ex)
            {
                //database error while getting users
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_User), ex);

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
        /// Validate input data.
        /// </summary>
        /// <returns>
        /// True if data is valid.
        /// </returns>
        private bool ValidateData()
        {
            //validate project name
            if (!ValidateRequiredField(mtxtProjectName, mlblProjectName.Text, null, null) ||
                !ValidateDescriptionField(mtxtProjectName, mlblProjectName.Text, null, null))
            {
                //invalid field
                return false;
            }

            //validate entity name
            if (!ValidateRequiredField(mtxtEntityName, mlblEntityName.Text, null, null))
            {
                //invalid field
                return false;
            }

            //validate cnpj if set
            if (!mtxtTaxId.Text.Equals("  .   .   /    -") &&
                !ValidateCnpjField(mtxtTaxId, null, null))
            {
                //invalid field
                return false;
            }

            //validate address
            if (!ValidateRequiredField(mtxtAddress, mlblAddress.Text, null, null))
            {
                //invalid field
                return false;
            }

            //validate district
            if (!ValidateRequiredField(mtxtDistrict, mlblDistrict.Text, null, null))
            {
                //invalid field
                return false;
            }

            //validate city
            if (!ValidateRequiredField(mtxtCity, mlblCity.Text, null, null))
            {
                //invalid field
                return false;
            }

            //validate zip code
            if (!ValidateRequiredField(mtxtZipCode, mlblZipCode.Text, null, null) ||
                !ValidateZipField(mtxtZipCode, null, null))
            {
                //invalid field
                return false;
            }

            //validate phone
            if (!ValidateRequiredField(mtxtPhone, mlblPhone.Text, null, null) ||
                !ValidatePhoneField(mtxtPhone, null, null))
            {
                //invalid field
                return false;
            }

            //validate mobile
            if (!ValidateRequiredField(mtxtMobile, mlblMobile.Text, null, null) ||
                !ValidatePhoneField(mtxtMobile, null, null))
            {
                //invalid field
                return false;
            }

            //validate email
            if (!ValidateRequiredField(mtxtEmail, mlblEmail.Text, null, null) ||
                !ValidateEmailField(mtxtEmail, null, null))
            {
                //invalid field
                return false;
            }

            //validate site if set
            if(mtxtSite.Text.Length > 0 && !ValidateUriField(mtxtSite, null, null))
            {
                //invalid field
                return false;
            }

            //validate description
            if (!ValidateRequiredField(mtxtDescription, mlblDescription.Text, null, null))
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
            mtxtProjectName.Text = string.Empty;
            mtxtEntityName.Text = string.Empty;
            mtxtLocalInitiative.Text = string.Empty;
            mtxtTaxId.Text = string.Empty;
            mtxtAddress.Text = string.Empty;
            mtxtDistrict.Text = string.Empty;
            mtxtCity.Text = string.Empty;
            mtxtZipCode.Text = string.Empty;
            mtxtPhone.Text = string.Empty;
            mtxtMobile.Text = string.Empty;
            mtxtEmail.Text = string.Empty;
            mtxtSite.Text = string.Empty;
            mtxtDescription.Text = string.Empty;

            //select Rio de Janeiro
            mcbState.SelectedIndex = 17;

            //check number of coordinators
            if (mcbCoordinator.Items.Count > 0)
            {
                //select first coordinator
                mcbCoordinator.SelectedIndex = 0;
            }

            //set institutionalized option
            mcbInstitutionalized.Checked = false;
        }

        /// <summary>
        /// Dispose used resources from user control.
        /// </summary>
        public override void DisposeControl()
        {
            //update option to hide inactive items
            Manager.Settings.HideInactiveInstitutions = this.hideInactiveItems;
        }

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        /// <returns></returns>
        public override string SelectMenuOption()
        {
            //select institution option
            return "Institution";
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
            mtxtProjectName.Enabled = enable;
            mtxtEntityName.Enabled = enable;
            mtxtLocalInitiative.Enabled = enable;
            mtxtTaxId.Enabled = enable;
            mtxtAddress.Enabled = enable;
            mtxtDistrict.Enabled = enable;
            mtxtCity.Enabled = enable;
            mtxtZipCode.Enabled = enable;
            mtxtPhone.Enabled = enable;
            mtxtMobile.Enabled = enable;
            mtxtEmail.Enabled = enable;
            mtxtSite.Enabled = enable;
            mtxtDescription.Enabled = enable;

            //set state list
            mcbState.Enabled = enable;

            //set coordinator list
            mcbCoordinator.Enabled = enable;

            //set institutionalized option
            mcbInstitutionalized.Enabled = enable;
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
                //get selected institution from web service
                Institution institution = songChannel.FindInstitution(itemId);

                //check result
                if (institution.Result == (int)SelectResult.Empty)
                {
                    //should never happen
                    //could not load data
                    return false;
                }
                else if (institution.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting institution
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadData, 
                        itemTypeDescription, institution.ErrorMessage),
                        Properties.Resources.titleWebServiceError, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadData, 
                        itemTypeDescription, institution.ErrorMessage));

                    //could not load data
                    return false;
                }

                //set selected institution ID
                selectedId = institution.InstitutionId;

                //set status
                mcbStatus.SelectedValue = institution.InstitutionStatus;

                //set text fields
                mtxtProjectName.Text = institution.ProjectName;
                mtxtEntityName.Text = institution.EntityName;
                mtxtLocalInitiative.Text = institution.LocalInitiative;
                mtxtTaxId.Text = institution.TaxId;
                mtxtAddress.Text = institution.Address;
                mtxtDistrict.Text = institution.District;
                mtxtCity.Text = institution.City;
                mtxtZipCode.Text = institution.ZipCode;
                mtxtPhone.Text = institution.Phone;
                mtxtMobile.Text = institution.Mobile;
                mtxtEmail.Text = institution.Email;
                mtxtSite.Text = institution.Site;
                mtxtDescription.Text = institution.Description;

                //set state
                mcbState.SelectedValue = institution.State;

                //set inactivation fields
                inactivationReason = institution.InactivationReason;
                inactivationTime = institution.InactivationTime;

                //check selected state
                if (mcbState.SelectedIndex < 0)
                {
                    //should never happen
                    //select default state
                    mcbState.SelectedIndex = 17;
                }

                //set institutionalized option
                mcbInstitutionalized.Checked = institution.Institutionalized;

                //set coordinator
                mcbCoordinator.SelectedValue = institution.CoordinatorId;

                //check selected coordinator
                if (mcbCoordinator.SelectedIndex < 0)
                {
                    try
                    {
                        //coordinator is not available
                        //it might be inactive
                        //must load user from web service
                        User user = songChannel.FindUser(institution.CoordinatorId);

                        //check result
                        if (user.Result == (int)SelectResult.Success)
                        {
                            //add user to list of coordinators
                            List<IdDescriptionStatus> users =
                                (List<IdDescriptionStatus>)mcbCoordinator.DataSource;
                            users.Add(user.GetDescriptionWithName());

                            //update displayed list
                            mcbCoordinator.DataSource = null;
                            mcbCoordinator.ValueMember = "Id";
                            mcbCoordinator.DisplayMember = "Description";
                            mcbCoordinator.DataSource = users;

                            //set coordinator
                            mcbCoordinator.SelectedValue = institution.CoordinatorId;
                        }
                    }
                    catch
                    {
                        //do nothing
                    }
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

            //data were loaded
            return true;
        }

        /// <summary>
        /// Load institution list from database.
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
                //list of institutions
                List<IdDescriptionStatus> institutions = null;

                //check if logged on user has an assigned institution
                if (Manager.LogonUser != null &&
                    Manager.LogonUser.InstitutionId > 0)
                {
                    //create institution description
                    IdDescriptionStatus institution = songChannel.ListInstitution(
                        Manager.LogonUser.InstitutionId);

                    //create list of institutions
                    institutions = new List<IdDescriptionStatus>();

                    //add institution description
                    institutions.Add(institution);
                }
                else
                {
                    //get list of all institutions
                    institutions = songChannel.ListInstitutions();
                }

                //check result
                if (institutions[0].Result == (int)SelectResult.Success)
                {
                    //institutions were found
                    return institutions;
                }
                else if (institutions[0].Result == (int)SelectResult.Empty)
                {
                    //no institution is available
                    return null;
                }
                else if (institutions[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting institutions
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem, 
                        itemTypeDescription, institutions[0].ErrorMessage));

                    //could not get institutions
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

            //could not get institutions
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
                //inactivate selected institution and get result
                DeleteResult result = songChannel.InactivateInstitution(
                    SelectedItemId, reasonForm.InactivationReason);

                //check result
                if (result.Result == (int)SelectResult.Success)
                {
                    //item was inactivated
                    //check if there is a parent control
                    //and if it is an institution register control
                    if (parentControl != null && parentControl is ViewInstitutionControl)
                    {
                        //update institution to inactive in parent control
                        ((ViewInstitutionControl)parentControl).UpdateInstitution(
                            SelectedItemId, (int)ItemStatus.Inactive, 
                            DateTime.Now, reasonForm.InactivationReason);
                    }

                    //item was inactivated
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
            //focus project name field
            mtxtProjectName.Focus();
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

            //create an institution and set data
            Institution institution = new Institution();
            
            //set selected institution ID
            institution.InstitutionId = selectedId;

            //set text fields
            institution.ProjectName = mtxtProjectName.Text.Trim();
            institution.EntityName = mtxtEntityName.Text.Trim();
            institution.LocalInitiative = mtxtLocalInitiative.Text.Trim();
            institution.TaxId = mtxtTaxId.Text.Equals("  .   .   /    -") ? string.Empty : mtxtTaxId.Text.Trim();
            institution.Address = mtxtAddress.Text.Trim();
            institution.District = mtxtDistrict.Text.Trim();
            institution.City = mtxtCity.Text.Trim();
            institution.State = mcbState.SelectedValue.ToString();
            institution.ZipCode = mtxtZipCode.Text.Trim();
            institution.Phone = mtxtPhone.Text.Trim();
            institution.Mobile = mtxtMobile.Text.Trim();
            institution.Email = mtxtEmail.Text.Trim();
            institution.Site = mtxtSite.Text.Trim();
            institution.Description = mtxtDescription.Text.Trim();

            //check selected status
            if (mcbStatus.SelectedIndex >= 0)
            {
                //set status
                institution.InstitutionStatus = (int)mcbStatus.SelectedValue;

                //check if selected status is inactive
                if ((int)mcbStatus.SelectedValue == (int)ItemStatus.Inactive)
                {
                    //create inactivation reason form
                    InactivationReasonForm reasonForm = new InactivationReasonForm(
                        itemTypeDescription, (int)mcbStatus.SelectedValue, inactivationReason);

                    //let user input an inactivation reason
                    reasonForm.ShowDialog(this);

                    //set inactivation reason with result
                    institution.InactivationReason = reasonForm.InactivationReason;

                    //set inactivation time
                    institution.InactivationTime = (inactivationTime != DateTime.MinValue) ?
                        inactivationTime : DateTime.Now;
                }
                else
                {
                    //reset inactivation
                    institution.InactivationReason = string.Empty;
                    institution.InactivationTime = DateTime.MinValue;
                }
            }
            else
            {
                //should never happen
                //set default status
                institution.InstitutionStatus = (int)ItemStatus.Active;

                //reset inactivation
                institution.InactivationTime = DateTime.MinValue;
                institution.InactivationReason = string.Empty;
            }

            //set institutionalized option
            institution.Institutionalized = mcbInstitutionalized.Checked;

            //set coordinator
            institution.CoordinatorId = (int)mcbCoordinator.SelectedValue;

            //set coordinator name to properly display institution in datagridview
            institution.CoordinatorName = ((IdDescriptionStatus)mcbCoordinator.SelectedItem).Description;

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not save institution
                return null;
            }

            try
            {
                //save institution and get result
                SaveResult saveResult = songChannel.SaveInstitution(institution);

                //check result
                if (saveResult.Result == (int)SelectResult.FatalError)
                {
                    //institution was not saved
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

                    //could not save institution
                    return null;
                }

                //set saved ID to institution ID
                institution.InstitutionId = saveResult.SavedId;

                //set selected ID
                selectedId = saveResult.SavedId;

                //check logged on user has saved institution as assigned institution
                if (Manager.LogonUser != null &&
                    Manager.LogonUser.InstitutionId == institution.InstitutionId)
                {
                    //update institution name
                    Manager.LogonUser.InstitutionName = institution.ProjectName;
                }

                //check if there is a parent control
                //check if it is an institution register control
                    if (parentControl != null && parentControl is ViewInstitutionControl)
                {
                    //update institution in parent control
                    ((ViewInstitutionControl)parentControl).UpdateInstitution(institution);
                }
                //check if it is an institution register control
                else if (parentControl != null && parentControl is ViewPoleControl)
                {
                    //update institution in parent control
                    ((ViewPoleControl)parentControl).UpdateInstitution(institution);
                }
                //check if it is an user register control
                else if (parentControl != null && parentControl is ViewUserControl)
                {
                    //update institution in parent control
                    ((ViewUserControl)parentControl).UpdateInstitution(institution);
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
            
            //institution was saved
            //return updated description
            return institution.GetDescription();
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
        private void RegisterInstitution_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set font to UI controls
            mtxtTaxId.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtZipCode.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtPhone.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtMobile.Font = MetroFramework.MetroFonts.Default(13.0F);
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
        /// Tax id masked text box click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtTaxId_Click(object sender, EventArgs e)
        {
            //check text
            if (mtxtTaxId.Text.Equals("  .   .   /    -"))
            {
                //set cursor position
                mtxtTaxId.Select(0, 0);
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

        #endregion UI Event Handlers

    } //end of class RegisterInstitutionControl

} //end of namespace PnT.SongClient.UI.Controls
