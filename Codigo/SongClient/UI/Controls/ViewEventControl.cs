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
    /// List and display events to user.
    /// </summary>
    public partial class ViewEventControl : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The song child control.
        /// </summary>
        ISongControl childControl = null;

        /// <summary>
        /// The list of all events.
        /// Used to gather all event dates.
        /// </summary>
        private List<Event> allEvents = null;

        /// <summary>
        /// List of displayed events on the control.
        /// </summary>
        private Dictionary<long, Event> events = null;

        /// <summary>
        /// The last found event.
        /// Used to improve the find method.
        /// </summary>
        private Event lastFoundEvent = null;

        /// <summary>
        /// DataTable for events.
        /// </summary>
        private DataTable dtEvents = null;

        /// <summary>
        /// The item displayable name.
        /// </summary>
        private string itemTypeDescription = Properties.Resources.item_Event;

        /// <summary>
        /// The item displayable plural name.
        /// </summary>
        protected string itemTypeDescriptionPlural = Properties.Resources.item_plural_Event;

        /// <summary>
        /// Indicates if the control is being loaded.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// Right-clicked event. The event of the displayed context menu.
        /// </summary>
        private Event clickedEvent = null;

        /// <summary>
        /// The event ID column index in the datagridview.
        /// </summary>
        private int columnIndexEventId;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ViewEventControl()
        {
            //set loading flag
            isLoading = true;

            //init ui components
            InitializeComponent();

            //create list of events
            events = new Dictionary<long, Event>();

            //create event data table
            CreateEventDataTable();

            //get event ID column index
            columnIndexEventId = dgvEvents.Columns[EventId.Name].Index;

            //list event dates
            ListEventDates();

            //subscribe settings property changed event
            Manager.Settings.PropertyChanged += Settings_PropertyChanged;
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get list of events.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public List<Event> ListEvents
        {
            get
            {
                //lock list of events
                lock (events)
                {
                    return new List<Event>(events.Values);
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
            //select calendar
            return "Calendar";
        }

        #endregion ISong Methods


        #region Public Methods ********************************************************

        /// <summary>
        /// Set columns visibility according to application settings.
        /// </summary>
        public void SetVisibleColumns()
        {
            //split visible column settings
            string[] columns = Manager.Settings.EventGridDisplayedColumns.Split(',');

            //hide all columns
            foreach (DataGridViewColumn dgColumn in dgvEvents.Columns)
            {
                //hide column
                dgColumn.Visible = false;
            }

            //set invisible last index
            int lastIndex = dgvEvents.Columns.Count - 1;

            //check each column and set visibility
            foreach (DataGridViewColumn dgvColumn in dgvEvents.Columns)
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

                        //set column display index event
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
        /// Create Event data table.
        /// </summary>
        private void CreateEventDataTable()
        {
            //create data table
            dtEvents = new DataTable();

            //EventId
            DataColumn dcEventId = new DataColumn("EventId", typeof(int));
            dtEvents.Columns.Add(dcEventId);

            //EventDate
            DataColumn dcEventDate = new DataColumn("EventDate", typeof(DateTime));
            dtEvents.Columns.Add(dcEventDate);

            //EventTime
            DataColumn dcEventTime = new DataColumn("EventTime", typeof(DateTime));
            dtEvents.Columns.Add(dcEventTime);

            //EventName
            DataColumn dcEventName = new DataColumn("EventName", typeof(string));
            dtEvents.Columns.Add(dcEventName);

            //InstitutionName
            DataColumn dcInstitutionName = new DataColumn("InstitutionName", typeof(string));
            dtEvents.Columns.Add(dcInstitutionName);

            //EventLocation
            DataColumn dcEventLocation = new DataColumn("EventLocation", typeof(string));
            dtEvents.Columns.Add(dcEventLocation);

            //Duration
            DataColumn dcDuration = new DataColumn("Duration", typeof(string));
            dtEvents.Columns.Add(dcDuration);

            //Description
            DataColumn dcDescription = new DataColumn("Description", typeof(string));
            dtEvents.Columns.Add(dcDescription);

            //EventSendOptionName
            DataColumn dcEventSendOptionName = new DataColumn("EventSendOptionName", typeof(string));
            dtEvents.Columns.Add(dcEventSendOptionName);            

            //CreationTime
            DataColumn dcCreationTime = new DataColumn("CreationTime", typeof(DateTime));
            dtEvents.Columns.Add(dcCreationTime);

            //set primary key column
            dtEvents.PrimaryKey = new DataColumn[] { dcEventId };
        }

        /// <summary>
        /// Display selected events.
        /// Clear currently displayed events before loading selected events.
        /// </summary>
        /// <param name="selectedEvents">
        /// The selected events to be loaded.
        /// </param>
        private void DisplayEvents(List<Event> selectedEvents)
        {
            //lock list of events
            lock (this.events)
            {
                //clear list
                this.events.Clear();

                //reset last found event
                lastFoundEvent = null;
            }

            //lock datatable of events
            lock (dtEvents)
            {
                //clear datatable
                dtEvents.Clear();
            }

            //check number of selected events
            if (selectedEvents != null && selectedEvents.Count > 0 &&
                selectedEvents[0].Result == (int)SelectResult.Success)
            {
                //lock list of events
                lock (events)
                {
                    //add selected events
                    foreach (Event eventObj in selectedEvents)
                    {
                        //check if event is not in the list
                        if (!events.ContainsKey(eventObj.EventId))
                        {
                            //add event to the list
                            events.Add(eventObj.EventId, eventObj);

                            //set last found event
                            lastFoundEvent = eventObj;
                        }
                        else
                        {
                            //should never happen
                            //log message
                            Manager.Log.WriteError(
                                "Error while loading events. Two events with same EventID " +
                                eventObj.EventId + ".");

                            //throw exception
                            throw new ApplicationException(
                                "Error while loading events. Two events with same EventID " +
                                eventObj.EventId + ".");
                        }
                    }
                }

                //lock data table of events
                lock (dtEvents)
                {
                    //check each event in the list
                    foreach (Event eventObj in ListEvents)
                    {
                        //create, set and add event row
                        DataRow dr = dtEvents.NewRow();
                        SetEventDataRow(dr, eventObj);
                        dtEvents.Rows.Add(dr);
                    }
                }
            }

            //refresh displayed UI
            RefreshUI(false);
        }

        /// <summary>
        /// Find event in the list of events.
        /// </summary>
        /// <param name="eventID">
        /// The ID of the selected event.
        /// </param>
        /// <returns>
        /// The event of the selected event ID.
        /// Null if event was not found.
        /// </returns>
        private Event FindEvent(long eventID)
        {
            //lock list of events
            lock (events)
            {
                //check last found event
                if (lastFoundEvent != null &&
                    lastFoundEvent.EventId == eventID)
                {
                    //same event
                    return lastFoundEvent;
                }

                //try to find selected event
                events.TryGetValue(eventID, out lastFoundEvent);

                //return result
                return lastFoundEvent;
            }
        }

        /// <summary>
        /// List event dates into UI.
        /// </summary>
        private void ListEventDates()
        {
            //check list of all events
            if (allEvents == null)
            {
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
                    //get all events
                    allEvents = songChannel.FindEventsByFilter(
                        false, -1, DateTime.MinValue, DateTime.MinValue);

                    //check result
                    if (allEvents[0].Result == (int)SelectResult.Empty)
                    {
                        //no event is available
                        //clear list
                        allEvents.Clear();
                    }
                    else if (allEvents[0].Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting semesters
                        //display message
                        MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.errorWebServiceListItem,
                            Properties.Resources.item_Event, allEvents[0].ErrorMessage),
                            Properties.Resources.titleWebServiceError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //write error
                        Manager.Log.WriteError(string.Format(
                            Properties.Resources.errorWebServiceListItem,
                            Properties.Resources.item_Event,
                            allEvents[0].ErrorMessage));

                        //clear list
                        allEvents.Clear();
                    }
                }
                catch (Exception ex)
                {
                    //database error while getting events
                    //write error
                    Manager.Log.WriteException(string.Format(
                        Properties.Resources.errorChannelListItem,
                        Properties.Resources.item_Event), ex);

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

            //create list of all event dates
            HashSet<DateTime> allEventDates = new HashSet<DateTime>();

            //check each event
            foreach (Event eventObj in allEvents)
            {
                //add event date to list
                allEventDates.Add(eventObj.StartTime.Date);
            }

            //set list of all event dates to calendar
            mcCalendar.BoldedDates = allEventDates.Count > 0 ?
                allEventDates.ToArray() : null;
        }

        /// <summary>
        /// Load and display filtered events.
        /// </summary>
        /// <returns>
        /// True if events were loaded.
        /// False otherwise.
        /// </returns>
        private bool LoadEvents()
        {
            //filter and load events
            List<Event> filteredEvents = null;

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
                //get list of events for selected date and later
                filteredEvents = songChannel.FindEventsByFilter(
                    true, -1, mcCalendar.SelectedDate, DateTime.MinValue);
                
                //check result
                if (filteredEvents[0].Result == (int)SelectResult.Empty)
                {
                    //no event was found
                    //clear list
                    filteredEvents.Clear();
                }
                else if (filteredEvents[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting events
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredEvents[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        itemTypeDescription, filteredEvents[0].ErrorMessage));

                    //could not load events
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

                //database error while getting events
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load events
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

            //sort events by start time
            filteredEvents.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));

            //display filtered events
            DisplayEvents(filteredEvents);

            //sort events by name by default
            dgvEvents.Sort(EventDate, ListSortDirection.Ascending);

            //events were loaded
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
            if (dgvEvents.DataSource == null)
            {
                //set source to datagrid
                dgvEvents.DataSource = dtEvents;
            }

            //check if last row should be displayed
            //and if the is at least one row
            if (displayLastRow && dgvEvents.Rows.Count > 0)
            {
                //refresh grid by displaying last row
                dgvEvents.FirstDisplayedScrollingRowIndex = (dgvEvents.Rows.Count - 1);
            }

            //refresh grid
            dgvEvents.Refresh();

            //set number of events
            mlblItemCount.Text = events.Count + " " +
                (events.Count > 1 ? itemTypeDescriptionPlural : itemTypeDescription);
        }

        /// <summary>
        /// Set data row with selected Event data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="eventObj">The selected event.</param>
        private void SetEventDataRow(DataRow dataRow, Event eventObj)
        {
            //set data row
            dataRow["EventId"] = eventObj.EventId;
            dataRow["EventDate"] = eventObj.StartTime;
            dataRow["EventTime"] = new DateTime(
                2000, 01, 01, eventObj.StartTime.Hour, eventObj.StartTime.Minute, 0);
            dataRow["EventName"] = eventObj.Name;
            dataRow["InstitutionName"] = eventObj.InstitutionId > 0 ? 
                eventObj.InstitutionName : Properties.Resources.wordAll;
            dataRow["EventLocation"] = eventObj.Location;
            dataRow["Duration"] = eventObj.Duration + " " + Properties.Resources.unitMinutes;
            dataRow["EventSendOptionName"] = eventObj.EventSendOption == (int)EventSendOption.ToNoOne ?
                "-" : Properties.Resources.ResourceManager.GetString(
                    "EventSendOption_" + ((EventSendOption)eventObj.EventSendOption).ToString());
            dataRow["CreationTime"] = eventObj.CreationTime;

            //set description 
            if (eventObj.Description == null || eventObj.Description.Length == 0)
            {
                dataRow["Description"] = "-";
            }
            else if (eventObj.Description.Length <= 100)
            {
                dataRow["Description"] = eventObj.Description;
            }
            else
            {
                //get next white space index
                int indexSpace = eventObj.Description.IndexOf(" ", 100);

                //clip description
                dataRow["Description"] = eventObj.Description.Substring(0, indexSpace) + "...";
            }
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Delete event from UI.
        /// </summary>
        /// <param name="eventId">
        /// The ID of the deleted event.
        /// </param>
        public void DeleteEvent(int eventId)
        {
            //remove event from list of all events
            if (allEvents.RemoveAll(e => e.EventId == eventId) > 0)
            {
                //update list of event dates
                ListEventDates();
            }

            //remove displayed event
            RemoveEvent(eventId);
        }

        /// <summary>
        /// Remove displayed event.
        /// </summary>
        /// <param name="eventId">
        /// The ID of the event to be removed.
        /// </param>
        private void RemoveEvent(int eventId)
        {
            //lock list of events
            lock (events)
            {
                //check if event is not in the list
                if (!events.ContainsKey(eventId))
                {
                    //no need to remove event
                    //exit
                    return;
                }

                //remove event
                events.Remove(eventId);
            }

            //lock data table of events
            lock (dtEvents)
            {
                //get displayed data row
                DataRow dr = dtEvents.Rows.Find(eventId);

                //remove displayed data row
                dtEvents.Rows.Remove(dr);
            }

            //refresh user interface
            RefreshUI(false);
        }

        /// <summary>
        /// Update a displayed event. 
        /// Add event if it is a new event.
        /// </summary>
        /// <param name="event">
        /// The updated event.
        /// </param>
        public void UpdateEvent(Event eventObj)
        {
            //remove outdated event from list of all events
            allEvents.RemoveAll(e => e.EventId == eventObj.EventId);

            //add event to list of all events
            allEvents.Add(eventObj);

            //list event dates
            ListEventDates();

            //check event should be displayed
            //start date filter
            if (mcCalendar.SelectedDate > eventObj.StartTime.Date)
            {
                //event should not be displayed
                //remove event if it is being displayed
                RemoveEvent(eventObj.EventId);

                //exit
                return;
            }

            //lock list of events
            lock (events)
            {
                //set event
                events[eventObj.EventId] = eventObj;
            }

            //lock data table of events
            lock (dtEvents)
            {
                //get displayed data row
                DataRow dr = dtEvents.Rows.Find(eventObj.EventId);

                //check if there was no data row yet
                if (dr == null)
                {
                    //create, set and add data row
                    dr = dtEvents.NewRow();
                    SetEventDataRow(dr, eventObj);
                    dtEvents.Rows.Add(dr);
                }
                else
                {
                    //set data
                    SetEventDataRow(dr, eventObj);
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
                dgvEvents.ColumnHeadersDefaultCellStyle.Font =
                    MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
                dgvEvents.DefaultCellStyle.Font =
                    MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            }
            else if (e.PropertyName.Equals("EventGridDisplayedColumns"))
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
        private void ViewEventControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //avoid auto generated columns
            dgvEvents.AutoGenerateColumns = false;

            //set fonts
            dgvEvents.ColumnHeadersDefaultCellStyle.Font = 
                MetroFramework.MetroFonts.Default((float)Manager.Settings.GridFontSize);
            dgvEvents.DefaultCellStyle.Font = 
                MetroFramework.MetroFonts.DefaultLight((float)Manager.Settings.GridFontSize);
            mcCalendar.ApptFont = MetroFramework.MetroFonts.Default(14.0F);
            mcCalendar.NoApptFont = MetroFramework.MetroFonts.DefaultLight(14.0F);
            mcCalendar.HeaderFont = MetroFramework.MetroFonts.DefaultLight(14.0F);

            //set visible columns
            SetVisibleColumns();

            //set control item heading
            mlblItemHeading.Text = itemTypeDescriptionPlural;

            //clear number of events
            mlblItemCount.Text = string.Empty;

            //reset loading flag
            isLoading = false;

            //select today to load events
            mcCalendar.SelectedDate = DateTime.Today;
        }

        /// <summary>
        /// Edit tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlEdit_Click(object sender, EventArgs e)
        {
            //create control
            RegisterEventControl registerControl =
                new UI.Controls.RegisterEventControl();
            registerControl.ParentControl = this;

            //check if there is any selected event
            if (dgvEvents.SelectedCells.Count > 0)
            {
                //select first selected event in the register control
                registerControl.FirstSelectedId =
                    (int)dgvEvents.Rows[dgvEvents.SelectedCells[0].RowIndex].Cells[columnIndexEventId].Value;
            }

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Events datagridview mouse up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvEvents_MouseUp(object sender, MouseEventArgs e)
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
            DataGridView.HitTestInfo hitInfo = dgvEvents.HitTest(e.X, e.Y);

            //check selected row index
            if (hitInfo.RowIndex < 0)
            {
                //no row selected
                //exit
                return;
            }

            //check if there are selected rows and if event clicked on them
            if (dgvEvents.SelectedRows.Count > 0 &&
                dgvEvents.Rows[hitInfo.RowIndex].Selected != true)
            {
                //event did not click in the selected rows
                //clear selection
                dgvEvents.ClearSelection();

                //select clicked row
                dgvEvents.Rows[hitInfo.RowIndex].Selected = true;
            }
            //check if there are selected cells
            else if (dgvEvents.SelectedCells.Count > 0)
            {
                //get list of selected cell rows
                HashSet<int> selectedRowIndexes = new HashSet<int>();

                //check each selected cell
                foreach (DataGridViewCell cell in dgvEvents.SelectedCells)
                {
                    //add cell row index to the list
                    selectedRowIndexes.Add(cell.RowIndex);
                }

                //check if event clicked on a row of a selected cell
                if (selectedRowIndexes.Contains(hitInfo.RowIndex))
                {
                    //event clicked on a row of a selected cell
                    //select all rows
                    foreach (int selectedRowIndex in selectedRowIndexes)
                    {
                        //select row
                        dgvEvents.Rows[selectedRowIndex].Selected = true;
                    }
                }
                else
                {
                    //event did not click on a row of a selected cell
                    //clear selected cells
                    dgvEvents.ClearSelection();

                    //select clicked row
                    dgvEvents.Rows[hitInfo.RowIndex].Selected = true;
                }
            }
            else
            {
                //select clicked row
                dgvEvents.Rows[hitInfo.RowIndex].Selected = true;
            }

            //get selected or clicked event
            clickedEvent = null;

            //check if there is a selected event
            if (dgvEvents.SelectedRows.Count > 0)
            {
                //there is one or more events selected
                //get first selected event
                for (int index = 0; index < dgvEvents.SelectedRows.Count; index++)
                {
                    //get event using its event id
                    int eventId = (int)dgvEvents.SelectedRows[index].Cells[columnIndexEventId].Value;
                    Event eventObj = FindEvent(eventId);

                    //check result
                    if (eventObj != null)
                    {
                        //add event to list
                        clickedEvent = eventObj;

                        //exit loop
                        break;
                    }
                }

                //check result
                if (clickedEvent == null)
                {
                    //no event was found
                    //should never happen
                    //do not display context menu
                    //exit
                    return;
                }
            }
            else
            {
                //there is no event selected
                //should never happen
                //do not display context menu
                //exit
                return;
            }

            //set context menu items visibility
            if (dgvEvents.SelectedRows.Count == 1)
            {
                //display view menu items according to permissions
                mnuViewEvent.Visible = true;
                tssSeparator.Visible = true;
            }
            else
            {
                //hide view menu items
                mnuViewEvent.Visible = false;
                tssSeparator.Visible = false;
            }

            //show event context menu on the clicked point
            mcmEvent.Show(this.dgvEvents, p);
        }

        /// <summary>
        /// View event menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewEvent_Click(object sender, EventArgs e)
        {
            //check clicked event
            if (clickedEvent == null)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected event
            RegisterEventControl registerControl =
                new UI.Controls.RegisterEventControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedEvent.EventId;

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
            if (this.dgvEvents.GetCellCount(DataGridViewElementStates.Selected) <= 0)
            {
                //should never happen
                //exit
                return;
            }

            try
            {
                //include header text 
                dgvEvents.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                //copy selection to the clipboard
                Clipboard.SetDataObject(this.dgvEvents.GetClipboardContent());
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

        /// <summary>
        /// Calendar selected date changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcCalendar_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            //check if control is loading
            if (isLoading)
            {
                //no need to handle this event
                //exit
                return;
            }

            //load event
            LoadEvents();
        }

        /// <summary>
        /// Next month tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlNextMonth_Click(object sender, EventArgs e)
        {
            //call calendar next button click event handler
            mcCalendar.btnNext_Click(this, new EventArgs());
        }

        /// <summary>
        /// Previous month tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlPreviousMonth_Click(object sender, EventArgs e)
        {
            //call calendar previous button click event handler
            mcCalendar.btnPrev_Click(this, new EventArgs());
        }

        #endregion UI Event Handlers

    } //end of class ViewEventControl

} //end of namespace PnT.SongClient.UI.Controls
