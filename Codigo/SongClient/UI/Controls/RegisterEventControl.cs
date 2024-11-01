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
    /// This control is used to manage event registries in web service.
    /// Inherits from base register control.
    /// </summary>
    public partial class RegisterEventControl : RegisterBaseControl
    {

        #region Fields ****************************************************************

        #endregion Fields


        #region Constructors **********************************************************
        
        public RegisterEventControl() : base("Event", false)
        {
            //init UI components
            InitializeComponent();

            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set units
            nudDuration.Unit = Properties.Resources.unitMinutes.ToLower();

            //hide copy button
            this.displayCopyButton = false;

            //event can be deleted
            this.classHasDeletion = true;

            //event does not have status
            this.classHasStatus = false;

            //set permissions
            this.allowAddItem = Manager.HasLogonPermission("Event.Insert");
            this.allowEditItem = Manager.HasLogonPermission("Event.Edit");
            this.allowDeleteItem = Manager.HasLogonPermission("Event.Delete");

            //load combos
            //list send options
            ListSendOptions();

            //list institutions
            ListInstitutions();
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
                //list of institutions
                List<IdDescriptionStatus> institutions = null;

                //option to create an all option
                bool createAllOption = true;

                //check if logged on event has an assigned institution
                if (Manager.LogonUser != null &&
                    Manager.LogonUser.InstitutionId > 0)
                {
                    //create institution description
                    IdDescriptionStatus institution = new IdDescriptionStatus(
                        Manager.LogonUser.InstitutionId,
                        Manager.LogonUser.InstitutionName,
                        (int)ItemStatus.Active);
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

                    //could not get institutions
                    //exit
                    return;
                }

                //check if should create an all option
                if (createAllOption)
                {
                    //create and add an all option
                    IdDescriptionStatus allOption = new IdDescriptionStatus(
                        int.MinValue, Properties.Resources.wordAll, 0);
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
        /// List send options into UI.
        /// </summary>
        private void ListSendOptions()
        {
            //create list of send options
            List<KeyValuePair<int, string>> sendOptions = new List<KeyValuePair<int, string>>();

            //check each send option
            foreach (EventSendOption sendOption in Enum.GetValues(typeof(EventSendOption)))
            {
                //add converted send option
                sendOptions.Add(new KeyValuePair<int, string>(
                    (int)sendOption, Properties.Resources.ResourceManager.GetString(
                        "EventSendOption_" + sendOption.ToString())
                    ));
            }

            //set send options to UI
            mcbSendOption.ValueMember = "Key";
            mcbSendOption.DisplayMember = "Value";
            mcbSendOption.DataSource = sendOptions;
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
            if (!ValidateRequiredField(mtxtName, mlblName.Text, null, null))
            {
                //invalid field
                return false;
            }

            //validate date
            if (!ValidateRequiredField(mtxtDate, mlblDate.Text, null, null) ||
                !ValidateDateField(mtxtDate, null, null))
            {
                //invalid field
                return false;
            }

            //validate start time
            if (!ValidateRequiredField(mtxtStartTime, mlblStartTime.Text, null, null) ||
                !ValidateShortTimeField(mtxtStartTime, null, null))
            {
                //invalid field
                return false;
            }

            //validate location
            if (!ValidateRequiredField(mtxtLocation, mlblLocation.Text, null, null))
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
            //clear text fields
            mtxtName.Text = string.Empty;
            mtxtDate.Text = string.Empty;
            mtxtLocation.Text = string.Empty;
            mtxtDescription.Text = string.Empty;

            //check number of institutions
            if (mcbInstitution.Items.Count > 0)
            {
                //select first institution
                mcbInstitution.SelectedIndex = 0;
            }

            //select first send option
            mcbSendOption.SelectedIndex = 0;

            //select first send otpion
            mcbSendOption.SelectedIndex = 0;

            //reset start time
            mtxtStartTime.Text = string.Empty;

            //reset duration
            nudDuration.Value = 60;
        }

        /// <summary>
        /// Dispose used resources from event control.
        /// </summary>
        public override void DisposeControl()
        {
        }

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        /// <returns></returns>
        public override string SelectMenuOption()
        {
            //select event option
            return "Event";
        }

        /// <summary>
        /// Enable all UI fields for edition.
        /// </summary>
        /// <param name="enable">True to enable fields. False to disable them.</param>
        public override void EnableFields(bool enable)
        {
            //set text fields
            mtxtName.Enabled = enable;
            mtxtDate.Enabled = enable;
            mtxtLocation.Enabled = enable;
            mtxtDescription.Enabled = enable;

            //set institution list
            mcbInstitution.Enabled = enable;

            //set send option list
            mcbSendOption.Enabled = enable;

            //set send option
            mcbSendOption.Enabled = enable;

            //set start time field
            mtxtStartTime.Enabled = enable;

            //set duration field
            nudDuration.Enabled = enable;
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
                //get selected event from web service
                Event eventObj = songChannel.FindEvent(itemId);

                //check result
                if (eventObj.Result == (int)SelectResult.Empty)
                {
                    //should never happen
                    //could not load data
                    return false;
                }
                else if (eventObj.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting event
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, eventObj.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        itemTypeDescription, eventObj.ErrorMessage));

                    //could not load data
                    return false;
                }

                //set selected event ID
                selectedId = eventObj.EventId;

                //set text fields
                mtxtName.Text = eventObj.Name;
                mtxtLocation.Text = eventObj.Location;
                mtxtDescription.Text = eventObj.Description;

                //set start time fields
                mtxtDate.Text = eventObj.StartTime.ToShortDateString();
                mtxtStartTime.Text = eventObj.StartTime.ToString("HH:mm");

                //set duration
                nudDuration.Value = eventObj.Duration;

                //set send option
                mcbSendOption.SelectedValue = eventObj.EventSendOption;

                //check if event has a designated institution
                if (eventObj.InstitutionId > 0)
                {
                    //set institution
                    mcbInstitution.SelectedValue = eventObj.InstitutionId;

                    //check selected index
                    if (mcbInstitution.SelectedIndex < 0)
                    {
                        try
                        {
                            //institution is not available
                            //it might be inactive
                            //must load institution from web service
                            Institution institution = songChannel.FindInstitution(eventObj.InstitutionId);

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
                                mcbInstitution.SelectedValue = eventObj.InstitutionId;
                            }
                        }
                        catch
                        {
                            //do nothing
                        }
                    }
                }
                else
                {
                    //select all option
                    mcbInstitution.SelectedIndex = 0;
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
        /// Load event list from database.
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
                //list of events
                List<IdDescriptionStatus> events = null;

                //check if logged on event has an assigned institution
                if (Manager.LogonUser != null &&
                    Manager.LogonUser.InstitutionId > 0)
                {
                    //get list of events for assigned institution
                    events = songChannel.ListEventsByInstitution(
                        Manager.LogonUser.InstitutionId);
                }
                else
                {
                    //get list of all events
                    events = songChannel.ListEvents();
                }

                //check result
                if (events[0].Result == (int)SelectResult.Success)
                {
                    //events were found
                    return events;
                }
                else if (events[0].Result == (int)SelectResult.Empty)
                {
                    //no event is available
                    return null;
                }
                else if (events[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting events
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        itemTypeDescription, events[0].ErrorMessage));

                    //could not get events
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

            //could not get events
            return null;
        }

        /// <summary>
        /// Delete current selected item.
        /// </summary>
        public override bool DeleteItem()
        {
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
                //delete selected event and get result
                DeleteResult result = songChannel.DeleteEvent(selectedId);

                //check result
                if (result.Result == (int)SelectResult.Success)
                {
                    //item was deleted
                    //check if there is a parent control
                    //check if it is an event register control
                    if (parentControl != null && parentControl is ViewEventControl)
                    {
                        //delete event in parent control
                        ((ViewEventControl)parentControl).DeleteEvent(selectedId);
                    }
                    else if (parentControl != null && parentControl is HomeControl)
                    {
                        //delete event in parent control
                        ((HomeControl)parentControl).DeleteEvent(selectedId);
                    }

                    //register was deleted
                    return true;
                }
                else if (result.Result == (int)SelectResult.FatalError)
                {
                    //error while deleting item
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

            //could not delete item
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

            //create an event and set data
            Event eventObj = new Event();

            //set selected event ID
            eventObj.EventId = selectedId;

            //set text fields
            eventObj.Name = mtxtName.Text.Trim();
            eventObj.Location = mtxtLocation.Text.Trim();
            eventObj.Description = mtxtDescription.Text.Trim();

            //set start time
            eventObj.StartTime = DateTime.Parse(mtxtDate.Text);
            DateTime time = DateTime.ParseExact(mtxtStartTime.Text + ":00", "H:m:s", null);
            eventObj.StartTime = eventObj.StartTime.AddHours(time.Hour);
            eventObj.StartTime = eventObj.StartTime.AddMinutes(time.Minute);

            //set duration
            eventObj.Duration = (int)nudDuration.Value;

            //set institution
            eventObj.InstitutionId = (int)mcbInstitution.SelectedValue;

            //set institution name to properly display event in datagridview
            eventObj.InstitutionName = (mcbInstitution.SelectedIndex >= 1) ?
                ((IdDescriptionStatus)mcbInstitution.SelectedItem).Description : string.Empty;

            //set send option
            eventObj.EventSendOption = (int)mcbSendOption.SelectedValue;

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not save event
                return null;
            }

            try
            {
                //save event and get result
                SaveResult saveResult = songChannel.SaveEvent(eventObj);

                //check result
                if (saveResult.Result == (int)SelectResult.FatalError)
                {
                    //event was not saved
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

                    //could not save event
                    return null;
                }

                //set saved ID to event ID
                eventObj.EventId = saveResult.SavedId;

                //set selected ID
                selectedId = saveResult.SavedId;

                //check if there is a parent control
                //check if it is an event register control
                if (parentControl != null && parentControl is ViewEventControl)
                {
                    //update event in parent control
                    ((ViewEventControl)parentControl).UpdateEvent(eventObj);
                }
                else if (parentControl != null && parentControl is HomeControl)
                {
                    //update event in parent control
                    ((HomeControl)parentControl).UpdateEvent(eventObj);
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

            //event was saved
            //return updated description
            return eventObj.GetDescription();
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
        private void RegisterEvent_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set font to UI controls
            mtxtDate.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtStartTime.Font = MetroFramework.MetroFonts.Default(13.0F);
            nudDuration.Font = MetroFramework.MetroFonts.Default(13.0F);
        }

        /// <summary>
        /// Date masked text box click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Date_Click(object sender, EventArgs e)
        {
            //get sender textbox
            MaskedTextBox mtxtDate = (MaskedTextBox)sender;

            //check text
            if (mtxtDate.Text.Equals("  /  /"))
            {
                //set cursor position
                mtxtDate.Select(0, 0);
            }
        }

        /// <summary>
        /// Time masked text box click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Time_Click(object sender, EventArgs e)
        {
            //get sender textbox
            MaskedTextBox mtxtTime = (MaskedTextBox)sender;

            //check text
            if (mtxtTime.Text.Equals("  :"))
            {
                //set cursor position
                mtxtTime.Select(0, 0);
            }
        }

        #endregion UI Event Handlers

    } //end of class RegisterEventControl 

} //end of namespace PnT.SongClient.UI.Controls
