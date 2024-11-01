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
    /// Welcome user to application and summarize indicators, events and others.
    /// </summary>
    public partial class HomeControl : UserControl, ISongControl
    {

        #region Constants *************************************************************

        /// <summary>
        /// The indicator refresh interval.
        /// Indicators are refreshed by displaying next data.
        /// </summary>
        public const int REFRESH_INTERVAL = 3000;

        #endregion Constants


        #region Fields ****************************************************************

        /// <summary>
        /// The song child control.
        /// </summary>
        ISongControl childControl = null;

        /// <summary>
        /// The list of indicators.
        /// </summary>
        List<IndicatorControl> indicators = null;

        /// <summary>
        /// The total elapsed time since last indicator refresh. In milliseconds.
        /// </summary>
        int elapsedTime = 0;

        /// <summary>
        /// DataTable for events.
        /// </summary>
        private DataTable dtEvents = null;

        /// <summary>
        /// The id of the right-clicked event. The event of the displayed context menu.
        /// </summary>
        private int clickedEventId = int.MinValue;

        /// <summary>
        /// The event ID column index in the datagridview.
        /// </summary>
        private int columnIndexEventId;

        /// <summary>
        /// List of notices shown on the control.
        /// </summary>
        private List<Notice> notices = null;

        /// <summary>
        /// Datatable for notices.
        /// </summary>
        private DataTable dtNotices = null;

        /// <summary>
        /// The id of the right-clicked notice report. 
        /// The notice report of the displayed context menu.
        /// </summary>
        private int clickedNoticeId = int.MinValue;

        /// <summary>
        /// The notice ID column index in the datagridview.
        /// </summary>
        private int columnIndexNoticeId;

        /// <summary>
        /// Option to allow any data grid view selection.
        /// Avoid data grid view default selection.
        /// </summary>
        private bool allowDataGridViewSelection = false;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public HomeControl()
        {
            //init UI components
            InitializeComponent();

            //create lists
            indicators = new List<UI.Controls.IndicatorControl>();
            notices = new List<Notice>();

            //create event data table
            CreateEventDataTable();

            //create notice data table
            CreateNoticeDataTable();

            //get event ID column index
            columnIndexEventId = dgvEvents.Columns[EventId.Name].Index;

            //get notice ID column index
            columnIndexNoticeId = dgvNotices.Columns[NoticeId.Name].Index;
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

            //stop timer
            timIndicators.Stop();
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
            //select dashboard
            return "Home";
        }

        #endregion ISong Methods


        #region Public Methods ********************************************************

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

            //set primary key column
            dtEvents.PrimaryKey = new DataColumn[] { dcEventId };
        }
        
        /// <summary>
        /// Create Notice data table.
        /// </summary>
        private void CreateNoticeDataTable()
        {
            //create data table
            dtNotices = new DataTable();

            //NoticeId
            DataColumn dcNoticeId = new DataColumn("NoticeId", typeof(int));
            dtNotices.Columns.Add(dcNoticeId);

            //NoticeText
            DataColumn dcNoticeText = new DataColumn("NoticeText", typeof(string));
            dtNotices.Columns.Add(dcNoticeText);

            //set primary key column
            dtNotices.PrimaryKey = new DataColumn[] { dcNoticeId };
        }

        /// <summary>
        /// Process average error by displaying and logging the error.
        /// </summary>
        /// <param name="average">
        /// The average result.
        /// </param>
        /// <param name="itemTypeDescription">
        /// The type description of the average item.
        /// </param>
        private void ProcessAverageError(AverageResult average, string itemTypeDescription)
        {
            //display message
            MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.errorWebServiceAverageItem,
                itemTypeDescription, average.ErrorMessage),
                Properties.Resources.titleWebServiceError,
                MessageBoxButtons.OK, MessageBoxIcon.Error);

            //log error
            Manager.Log.WriteError(string.Format(
                Properties.Resources.errorWebServiceAverageItem,
                itemTypeDescription, average.ErrorMessage));
        }

        /// <summary>
        /// Process count error by displaying and logging the error.
        /// </summary>
        /// <param name="count">
        /// The count result.
        /// </param>
        /// <param name="itemTypeDescription">
        /// The type description of the count item.
        /// </param>
        private void ProcessCountError(CountResult count, string itemTypeDescription)
        {
            //display message
            MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.errorWebServiceCountItem,
                itemTypeDescription, count.ErrorMessage),
                Properties.Resources.titleWebServiceError,
                MessageBoxButtons.OK, MessageBoxIcon.Error);

            //log error
            Manager.Log.WriteError(string.Format(
                Properties.Resources.errorWebServiceCountItem,
                itemTypeDescription, count.ErrorMessage));
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
        }

        /// <summary>
        /// Set data row with selected Notice data.
        /// </summary>
        /// <param name="dr">The data row to be set.</param>
        /// <param name="notice">The selected notice.</param>
        private void SetNoticeDataRow(DataRow dataRow, Notice notice)
        {
            //set data row
            dataRow["NoticeId"] = notice.NoticeId;
            dataRow["NoticeText"] = notice.Text;
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
            //lock data table of events
            lock (dtEvents)
            {
                //get displayed data row
                DataRow dr = dtEvents.Rows.Find(eventId);

                //check result
                if (dr != null)
                {
                    //remove displayed data row
                    dtEvents.Rows.Remove(dr);

                    //refresh grid
                    dgvEvents.Refresh();
                }
            }
        }


        /// <summary>
        /// Update a displayed notice report. 
        /// Add report if it is a new report.
        /// </summary>
        /// <param name="reportObj">
        /// The updated report.
        /// </param>
        public void UpdateNoticeReport(Report reportObj)
        {
            //check report
            if (reportObj.ReportStatus != (int)ReportStatus.Completed)
            {
                //report is not completed
                //exit
                return;
            }

            //find report notice
            Notice notice = notices.Find(
                n => n.Report != null && n.Report.ReportId == reportObj.ReportId);

            //check result
            if (notice == null)
            {
                ////notice was not found
                //should never happen
                //exit
                return;
            }

            //remove notice from list
            notices.Remove(notice);

            //lock data table of notices
            lock (dtNotices)
            {
                //get displayed data row
                DataRow dr = dtNotices.Rows.Find(notice.NoticeId);

                //check result
                if (dr != null)
                {
                    //remove displayed data row
                    dtNotices.Rows.Remove(dr);

                    //refresh grid
                    dgvNotices.Refresh();
                }
            }
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
            //check event should be displayed
            //today filter
            if (DateTime.Today > eventObj.StartTime.Date)
            {
                //event should not be displayed
                //remove event if it is being displayed
                RemoveEvent(eventObj.EventId);

                //exit
                return;
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

            //refresh grid
            dgvEvents.Refresh();
        }

        #endregion Event Handlers


        #region UI Event Handlers *****************************************************

        /// <summary>
        /// Control load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomeControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //avoid auto generated columns
            dgvEvents.AutoGenerateColumns = false;
            dgvNotices.AutoGenerateColumns = false;

            //set fonts
            dgvEvents.ColumnHeadersDefaultCellStyle.Font = MetroFramework.MetroFonts.Default(12.0F);
            dgvEvents.DefaultCellStyle.Font = MetroFramework.MetroFonts.DefaultLight(12.0F);
            dgvNotices.ColumnHeadersDefaultCellStyle.Font = MetroFramework.MetroFonts.Default(12.0F);
            dgvNotices.DefaultCellStyle.Font = MetroFramework.MetroFonts.DefaultLight(12.0F);

            //check if there is a logged on user
            if (Manager.LogonUser != null)
            {
                //set control welcome message
                mlblWelcome.Text = string.Format(
                    Properties.Resources.titleWelcomeUser, Manager.LogonUser.Name);
            }
            else
            {
                //should never happen
                //reset control welcome message
                mlblWelcome.Text = string.Empty;
            }

            //reset indicators data
            mlblIndicatorsData.Text = string.Empty;

            //set list of indicators
            indicators.Add(icIndicator1);
            indicators.Add(icIndicator2);
            indicators.Add(icIndicator3);
            indicators.Add(icIndicator4);
            indicators.Add(icIndicator5);
            indicators.Add(icIndicator6);
            indicators.Add(icIndicator7);
            indicators.Add(icIndicator8);
            indicators.Add(icIndicator9);
            indicators.Add(icIndicator10);

            //set color of statistics indicators
            icIndicator7.BackgroundColor = Color.FromArgb(66, 174, 216);
            icIndicator8.BackgroundColor = Color.FromArgb(66, 174, 216);
            icIndicator9.BackgroundColor = Color.FromArgb(66, 174, 216);
            icIndicator10.BackgroundColor = Color.FromArgb(66, 174, 216);

            //load indicator data
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
                //perform base server functions
                #region base functions

                //region generate reports
                CountResult countResult = songChannel.GenerateReports();

                //check result
                if (countResult.Result == (int)SelectResult.FatalError)
                {
                    //unexpected error while generating reports
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceGenerateReports,
                        countResult.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceGenerateReports,
                        countResult.ErrorMessage));
                }

                #endregion base functions

                //set indicator 1
                #region indicator 1 - institution and pole count

                //get number of active institutions
                CountResult count = songChannel.CountInstitutionsByFilter(
                    (int)ItemStatus.Active);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Institution);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator1.AddIndicator(
                    count.Count, Properties.Resources.item_plural_Institution);
                
                //get number of active poles
                count = songChannel.CountPolesByFilter(
                    (int)ItemStatus.Active, -1);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Pole);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator1.AddIndicator(
                    count.Count, Properties.Resources.item_plural_Pole);

                #endregion indicator 1 - institution and pole count

                //set indicator 2
                #region indicator 2 - student and registration count

                //get number of active students
                count = songChannel.CountStudentsByFilter(
                    (int)ItemStatus.Active, -1, -1);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Student);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator2.AddIndicator(
                    count.Count, Properties.Resources.item_plural_Student);

                //get number of active registrations
                count = songChannel.CountRegistrationsByFilter(
                    (int)ItemStatus.Active, Manager.CurrentSemester.Id, -1, -1, -1, -1);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Registration);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator2.AddIndicator(
                    count.Count, Properties.Resources.item_plural_Registration);

                #endregion indicator 2 - student and registration count

                //set indicator 3
                #region indicator 3 - class and teacher count

                //get number of active classes
                count = songChannel.CountClassesByFilter(
                    (int)ItemStatus.Active, -1, -1, -1, 
                    Manager.CurrentSemester.SemesterId, -1, -1, -1);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Class);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator3.AddIndicator(
                    count.Count, Properties.Resources.item_plural_Class);

                //get number of active teachers
                count = songChannel.CountTeachersByFilter(
                    (int)ItemStatus.Active, -1, -1);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Teacher);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator3.AddIndicator(
                    count.Count, Properties.Resources.item_plural_Teacher);

                #endregion indicator 3 - class and teacher count

                //set indicator 4
                #region indicator 4 - instrument and loan count

                //get number of active instruments
                count = songChannel.CountInstrumentsByFilter(
                    (int)ItemStatus.Active, -1, -1, -1);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Instrument);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator4.AddIndicator(
                    count.Count, Properties.Resources.item_plural_Instrument);

                //get number of active loans
                count = songChannel.CountLoansByFilter(
                    (int)ItemStatus.Active, -1, -1, DateTime.MinValue, DateTime.MinValue);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Loan);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator4.AddIndicator(
                    count.Count, Properties.Resources.item_plural_Loan);

                //get number of pending close loans
                count = songChannel.CountLoansByFilter(
                    (int)ItemStatus.Active, -1, -1, DateTime.MinValue, DateTime.Today);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Loan);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator4.AddIndicator(
                    count.Count, Properties.Resources.caption_LoanPendingClose);

                #endregion indicator 4 - instrument and loan count

                //set indicator 5
                #region indicator 5 - semester time

                //get current semester
                Semester semester = Manager.CurrentSemester;

                //get today
                DateTime today = DateTime.Today;

                //check if semester has not started yet
                if (today < semester.StartDate)
                {
                    //calculate number of weeks to start semester
                    //round number up
                    int numWeeks = (int)((semester.StartDate.Subtract(today).TotalDays / 7.0) + 0.99);

                    //add data to indicator
                    icIndicator5.AddIndicator(
                        numWeeks, (numWeeks > 1) ?
                        Properties.Resources.caption_WeeksToStartSemester :
                        Properties.Resources.caption_WeekToStartSemester);
                }
                //check if semester has not ended yet
                else if (today <= semester.EndDate)
                {
                    //calculate number of weeks to end semester
                    //round number up
                    int numWeeks = (int)((semester.EndDate.Subtract(today).TotalDays / 7.0) + 0.99);

                    //add data to indicator
                    icIndicator5.AddIndicator(
                        numWeeks, (numWeeks > 1) ? 
                        Properties.Resources.caption_WeeksToEndSemester :
                        Properties.Resources.caption_WeekToEndSemester);

                    //calculate number of weeks since start of semester
                    //no number round up
                    numWeeks = (int)(today.Subtract(semester.StartDate).TotalDays / 7.0);

                    //add data to indicator
                    icIndicator5.AddIndicator(
                        numWeeks, (numWeeks > 1) ?
                        Properties.Resources.caption_WeeksStartSemester :
                        Properties.Resources.caption_WeekStartSemester);
                }
                else
                {
                    //semester has ended
                    //get next semester
                    Semester nextSemester = songChannel.FindSemester(semester.Id + 1);

                    //check result
                    if (nextSemester.Result == (int)SelectResult.Success)
                    {
                        //calculate number of weeks to start next semester
                        //round number up
                        int numWeeks = (int)((nextSemester.StartDate.Subtract(today).TotalDays / 7.0) + 0.99);

                        //add data to indicator
                        icIndicator5.AddIndicator(
                            numWeeks, (numWeeks > 1) ?
                            Properties.Resources.caption_WeeksToStartSemester :
                            Properties.Resources.caption_WeekToStartSemester);
                    }
                    else
                    {
                        //should hardly never happen
                        //must create next semester
                        //add warning to indicator
                        icIndicator5.AddIndicator(
                            1, Properties.Resources.caption_CreateNextSemester);
                    }
                }

                #endregion indicator 5 - semester time

                //set indicator 6
                #region indicator 6 - event count

                //get current month
                DateTime currentMonth = new DateTime(
                    DateTime.Now.Year, DateTime.Now.Month, 1);
                
                //get number of month events
                count = songChannel.CountEventsByFilter(
                    -1, currentMonth, currentMonth.AddMonths(1).AddDays(-1));

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Event);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator6.AddIndicator(
                    count.Count, Properties.Resources.caption_MonthEvents);

                //get number of semester events
                count = songChannel.CountEventsByFilter(
                    -1, Manager.CurrentSemester.StartDate, Manager.CurrentSemester.EndDate);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Event);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator6.AddIndicator(
                    count.Count, Properties.Resources.caption_SemesterEvents);

                //get number of year events
                count = songChannel.CountEventsByFilter(
                    -1, new DateTime(DateTime.Now.Year, 1, 1), 
                    new DateTime(DateTime.Now.Year, 12, 31));

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Event);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator6.AddIndicator(
                    count.Count, Properties.Resources.caption_YearEvents);

                #endregion indicator 6 - event count

                //set indicator 7
                #region indicator 7 - student attendance percentage

                //get number of month present attendance
                count = songChannel.CountAttendancesByFilter(
                    -1, -1, -1, (int)RollCall.Present,
                    currentMonth, currentMonth.AddMonths(1).AddDays(-1));

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Attendance);

                    //could not load indicators
                    //exit
                    return;
                }

                //get number of month absent attendance
                CountResult secondCount = songChannel.CountAttendancesByFilter(
                    -1, -1, -1, (int)RollCall.Absent,
                    currentMonth, currentMonth.AddMonths(1).AddDays(-1));

                //check result
                if (secondCount.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(secondCount, Properties.Resources.item_Attendance);

                    //could not load indicators
                    //exit
                    return;
                }

                //calculate presence percentage
                double percentage = double.MinValue;

                //check counts
                if (count.Count + secondCount.Count > 0)
                {
                    percentage = (double)(count.Count) / (double)(count.Count + secondCount.Count);
                    percentage *= 100.0;
                }

                //add data to indicator
                icIndicator7.AddIndicator(percentage != double.MinValue ? 
                    percentage.ToString("0.0") + "%" : "--%", 
                    Properties.Resources.caption_MonthAttendance);
                
                //check if there is a previous month inside semester
                if (Manager.CurrentSemester.StartDate.Month < DateTime.Now.Month)
                {
                    //get last month
                    DateTime lastMonth = new DateTime(
                        DateTime.Now.Year, DateTime.Now.Month - 1, 1);

                    //get number of last month present attendance
                    count = songChannel.CountAttendancesByFilter(
                        -1, -1, -1, (int)RollCall.Present,
                        lastMonth, lastMonth.AddMonths(1).AddDays(-1));

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Attendance);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //get number of month absent attendance
                    secondCount = songChannel.CountAttendancesByFilter(
                        -1, -1, -1, (int)RollCall.Absent,
                        lastMonth, lastMonth.AddMonths(1).AddDays(-1));

                    //check result
                    if (secondCount.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(secondCount, Properties.Resources.item_Attendance);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //calculate presence percentage
                    percentage = double.MinValue;

                    //check counts
                    if (count.Count + secondCount.Count > 0)
                    {
                        percentage = (double)(count.Count) / (double)(count.Count + secondCount.Count);
                        percentage *= 100.0;
                    }

                    //add data to indicator
                    icIndicator7.AddIndicator(percentage != double.MinValue ?
                        percentage.ToString("0.0") + "%" : "--%",
                        Properties.Resources.caption_LastMonthAttendance);
                }
                
                //get number of semester present attendance
                count = songChannel.CountAttendancesByFilter(
                    -1, -1, -1, (int)RollCall.Present, 
                    Manager.CurrentSemester.StartDate,
                    Manager.CurrentSemester.EndDate);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Attendance);

                    //could not load indicators
                    //exit
                    return;
                }

                //get number of semester absent attendance
                secondCount = songChannel.CountAttendancesByFilter(
                    -1, -1, -1, (int)RollCall.Absent, 
                    Manager.CurrentSemester.StartDate,
                    Manager.CurrentSemester.EndDate);

                //check result
                if (secondCount.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(secondCount, Properties.Resources.item_Attendance);

                    //could not load indicators
                    //exit
                    return;
                }

                //calculate presence percentage
                percentage = double.MinValue;

                //check counts
                if (count.Count + secondCount.Count > 0)
                {
                    percentage = (double)(count.Count) / (double)(count.Count + secondCount.Count);
                    percentage *= 100.0;
                }

                //add data to indicator
                icIndicator7.AddIndicator(percentage != double.MinValue ?
                    percentage.ToString("0.0") + "%" : "--%",
                    Properties.Resources.caption_SemesterAttendance);

                #endregion indicator 7 - student attendance percentage

                //set indicator 8
                #region indicator 8 - student average grade

                //get grade average for present month
                AverageResult average = songChannel.AverageGradesByFilter(
                    (int)GradeRapporteur.Teacher, (int)GradeTarget.Student,
                    (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                    Manager.CurrentSemester.SemesterId : -1, currentMonth,
                    -1, -1, -1, -1, -1);

                //check result
                if (average.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting average
                    //process average error
                    ProcessAverageError(average, Properties.Resources.item_Grade);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator8.AddIndicator(average.Average != double.MinValue ?
                    average.Average.ToString("0.00") : "--",
                    Properties.Resources.caption_MonthGradeStudent);

                //check if there is a previous month inside semester
                if (Manager.CurrentSemester.StartDate.Month < DateTime.Now.Month)
                {
                    //get last month
                    DateTime lastMonth = new DateTime(
                        DateTime.Now.Year, DateTime.Now.Month - 1, 1);

                    //get grade average for last month
                    average = songChannel.AverageGradesByFilter(
                        (int)GradeRapporteur.Teacher, (int)GradeTarget.Student,
                        (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                        Manager.CurrentSemester.SemesterId : -1, lastMonth,
                        -1, -1, -1, -1, -1);

                    //check result
                    if (average.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting average
                        //process average error
                        ProcessAverageError(average, Properties.Resources.item_Grade);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator8.AddIndicator(average.Average != double.MinValue ?
                        average.Average.ToString("0.00") : "--",
                        Properties.Resources.caption_LastMonthGradeStudent);
                }

                //get grade average for present semester
                average = songChannel.AverageGradesByFilter(
                    (int)GradeRapporteur.Teacher, (int)GradeTarget.Student,
                    (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                    Manager.CurrentSemester.SemesterId : -1, DateTime.MinValue,
                    -1, -1, -1, -1, -1);

                //check result
                if (average.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting average
                    //process average error
                    ProcessAverageError(average, Properties.Resources.item_Grade);

                    //could not load indicators
                    //exit
                    return;
                }
                                
                //add data to indicator
                icIndicator8.AddIndicator(average.Average != double.MinValue ?
                    average.Average.ToString("0.00") : "--",
                    Properties.Resources.caption_SemesterGradeStudent);

                #endregion indicator 8 - student average grade

                //set indicator 9
                #region indicator 9 - evasion

                //get number of evations for present month
                count = songChannel.CountEvationsByFilter(
                    Manager.CurrentSemester.SemesterId, currentMonth, -1, -1);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Evasion);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator9.AddIndicator(
                    count.Count, Properties.Resources.caption_MonthEvation);

                //check if there is a previous month inside semester
                if (Manager.CurrentSemester.StartDate.Month < DateTime.Now.Month)
                {
                    //get last month
                    DateTime lastMonth = new DateTime(
                        DateTime.Now.Year, DateTime.Now.Month - 1, 1);

                    //get number of evations for last month
                    count = songChannel.CountEvationsByFilter(
                        Manager.CurrentSemester.SemesterId, lastMonth, -1, -1);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Evasion);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator9.AddIndicator(
                        count.Count, Properties.Resources.caption_LastMonthEvation);
                }

                //get number of evations for present semester
                count = songChannel.CountEvationsByFilter(
                Manager.CurrentSemester.SemesterId, DateTime.MinValue, -1, -1);

                //check result
                if (count.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting count
                    //process count error
                    ProcessCountError(count, Properties.Resources.item_Evasion);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator9.AddIndicator(
                    count.Count, Properties.Resources.caption_SemesterEvation);

                #endregion indicator 9 - evasion

                //set indicator 10
                #region indicator 10 - teacher average grade

                //get grade average for present month
                average = songChannel.AverageGradesByFilter(
                    (int)GradeRapporteur.Coordinator, (int)GradeTarget.Teacher,
                    (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                    Manager.CurrentSemester.SemesterId : -1, currentMonth,
                    -1, -1, -1, -1, -1);

                //check result
                if (average.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting average
                    //process average error
                    ProcessAverageError(average, Properties.Resources.item_Grade);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator10.AddIndicator(average.Average != double.MinValue ?
                    average.Average.ToString("0.00") : "--",
                    Properties.Resources.caption_MonthGradeTeacher);

                //check if there is a previous month inside semester
                if (Manager.CurrentSemester.StartDate.Month < DateTime.Now.Month)
                {
                    //get last month
                    DateTime lastMonth = new DateTime(
                        DateTime.Now.Year, DateTime.Now.Month - 1, 1);

                    //get grade average for last month
                    average = songChannel.AverageGradesByFilter(
                    (int)GradeRapporteur.Coordinator, (int)GradeTarget.Teacher,
                    (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                    Manager.CurrentSemester.SemesterId : -1, lastMonth,
                    -1, -1, -1, -1, -1);

                    //check result
                    if (average.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting average
                        //process average error
                        ProcessAverageError(average, Properties.Resources.item_Grade);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator10.AddIndicator(average.Average != double.MinValue ?
                        average.Average.ToString("0.00") : "--",
                        Properties.Resources.caption_LastMonthGradeTeacher);
                }

                //get grade average for present semester
                average = songChannel.AverageGradesByFilter(
                    (int)GradeRapporteur.Coordinator, (int)GradeTarget.Teacher,
                    (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                    Manager.CurrentSemester.SemesterId : -1, DateTime.MinValue,
                    -1, -1, -1, -1, -1);

                //check result
                if (average.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting average
                    //process average error
                    ProcessAverageError(average, Properties.Resources.item_Grade);

                    //could not load indicators
                    //exit
                    return;
                }

                //add data to indicator
                icIndicator10.AddIndicator(average.Average != double.MinValue ?
                    average.Average.ToString("0.00") : "--",
                    Properties.Resources.caption_SemesterGradeTeacher);

                #endregion indicator 10 - teacher average grade

                //check if logged on user is a teacher
                if (Manager.LogonTeacher != null)
                {
                    //add extra indicators for teacher
                    //get teacher id
                    int teacherId = Manager.LogonTeacher.TeacherId;

                    //set teacher name to indicators data
                    mlblIndicatorsData.Text = Properties.Resources.item_Teacher +
                        " " + Manager.LogonTeacher.Name;

                    //set indicator 1
                    #region indicator 1 - institution and pole count

                    //get number of poles for selected teacher
                    //add data to indicator
                    icIndicator1.AddIndicator(
                        Manager.LogonTeacher.PoleNames.Count, 
                        Properties.Resources.caption_TeacherPoles);

                    #endregion indicator 1 - institution and pole count

                    //set indicator 2
                    #region indicator 2 - student and registration count

                    //get number of active registrations for selected teacher
                    count = songChannel.CountRegistrationsByFilter(
                        (int)ItemStatus.Active, Manager.CurrentSemester.Id, -1, -1, teacherId, -1);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Registration);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator2.AddIndicator(
                        count.Count, Properties.Resources.caption_TeacherRegistrations);

                    #endregion indicator 2 - student and registration count

                    //set indicator 3
                    #region indicator 3 - class and teacher count

                    //get number of active classes for selected teacher
                    count = songChannel.CountClassesByFilter(
                        (int)ItemStatus.Active, -1, -1, -1,
                        Manager.CurrentSemester.SemesterId, -1, -1, teacherId);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Class);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator3.AddIndicator(
                        count.Count, Properties.Resources.caption_TeacherClasses);

                    #endregion indicator 3 - class and teacher count

                    //set indicator 8
                    #region indicator 8 - student average grade

                    //get grade average for present month and selected teacher
                    average = songChannel.AverageGradesByFilter(
                        (int)GradeRapporteur.Teacher, (int)GradeTarget.Student,
                        (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                        Manager.CurrentSemester.SemesterId : -1, currentMonth,
                        -1, teacherId, -1, -1, -1);

                    //check result
                    if (average.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting average
                        //process average error
                        ProcessAverageError(average, Properties.Resources.item_Grade);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator8.AddIndicator(average.Average != double.MinValue ?
                        average.Average.ToString("0.00") : "--",
                        Properties.Resources.caption_TeacherMonthGradeStudent);

                    //check if there is a previous month inside semester
                    if (Manager.CurrentSemester.StartDate.Month < DateTime.Now.Month)
                    {
                        //get last month
                        DateTime lastMonth = new DateTime(
                            DateTime.Now.Year, DateTime.Now.Month - 1, 1);

                        //get grade average for last month and selected institution
                        average = songChannel.AverageGradesByFilter(
                            (int)GradeRapporteur.Teacher, (int)GradeTarget.Student,
                            (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                            Manager.CurrentSemester.SemesterId : -1, lastMonth,
                            -1, teacherId, -1, -1, -1);

                        //check result
                        if (average.Result == (int)SelectResult.FatalError)
                        {
                            //database error while getting average
                            //process average error
                            ProcessAverageError(average, Properties.Resources.item_Grade);

                            //could not load indicators
                            //exit
                            return;
                        }

                        //add data to indicator
                        icIndicator8.AddIndicator(average.Average != double.MinValue ?
                            average.Average.ToString("0.00") : "--",
                            Properties.Resources.caption_TeacherLastMonthGradeStudent);
                    }

                    //get grade average for present semester
                    average = songChannel.AverageGradesByFilter(
                        (int)GradeRapporteur.Teacher, (int)GradeTarget.Student,
                        (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                        Manager.CurrentSemester.SemesterId : -1, DateTime.MinValue,
                        -1, teacherId, -1, -1, -1);

                    //check result
                    if (average.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting average
                        //process average error
                        ProcessAverageError(average, Properties.Resources.item_Grade);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator8.AddIndicator(average.Average != double.MinValue ?
                        average.Average.ToString("0.00") : "--",
                        Properties.Resources.caption_TeacherSemesterGradeStudent);

                    #endregion indicator 8 - student average grade

                    //set indicator 9
                    #region indicator 9 - evasion

                    //get number of evations for present month and selected teacher
                    count = songChannel.CountEvationsByFilter(
                        Manager.CurrentSemester.SemesterId, currentMonth, -1, teacherId);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Evasion);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator9.AddIndicator(
                        count.Count, Properties.Resources.caption_TeacherMonthEvation);

                    //check if there is a previous month inside semester
                    if (Manager.CurrentSemester.StartDate.Month < DateTime.Now.Month)
                    {
                        //get last month
                        DateTime lastMonth = new DateTime(
                            DateTime.Now.Year, DateTime.Now.Month - 1, 1);

                        //get number of evations for last month and selected teacher
                        count = songChannel.CountEvationsByFilter(
                            Manager.CurrentSemester.SemesterId, lastMonth, -1, teacherId);

                        //check result
                        if (count.Result == (int)SelectResult.FatalError)
                        {
                            //database error while getting count
                            //process count error
                            ProcessCountError(count, Properties.Resources.item_Evasion);

                            //could not load indicators
                            //exit
                            return;
                        }

                        //add data to indicator
                        icIndicator9.AddIndicator(
                            count.Count, Properties.Resources.caption_TeacherLastMonthEvation);
                    }

                    //get number of evations for present semester and selected teacher
                    count = songChannel.CountEvationsByFilter(
                        Manager.CurrentSemester.SemesterId, DateTime.MinValue, -1, teacherId);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Evasion);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator9.AddIndicator(
                        count.Count, Properties.Resources.caption_TeacherSemesterEvation);

                    #endregion indicator 9 - evasion
                }
                //check if logged on user has an assigned institution
                else if (Manager.LogonUser != null && Manager.LogonUser.InstitutionId > 0)
                {
                    //add extra indicators for institution
                    //get institution id
                    int institutionId = Manager.LogonUser.InstitutionId;

                    //set institution name to indicators data
                    mlblIndicatorsData.Text = Properties.Resources.item_Institution +
                        " " + Manager.LogonUser.InstitutionName;

                    //set indicator 1
                    #region indicator 1 - institution and pole count

                    //get number of active poles for selected institution
                    count = songChannel.CountPolesByFilter(
                        (int)ItemStatus.Active, institutionId);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Pole);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator1.AddIndicator(
                        count.Count, Properties.Resources.caption_InstitutionPoles);

                    #endregion indicator 1 - institution and pole count

                    //set indicator 2
                    #region indicator 2 - student and registration count

                    //get number of active students for selected institution
                    count = songChannel.CountStudentsByFilter(
                        (int)ItemStatus.Active, institutionId, -1);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Student);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator2.AddIndicator(
                        count.Count, Properties.Resources.caption_InstitutionStudents);

                    //get number of active registrations for selected institution
                    count = songChannel.CountRegistrationsByFilter(
                        (int)ItemStatus.Active, Manager.CurrentSemester.Id, institutionId, -1, -1, -1);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Registration);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator2.AddIndicator(
                        count.Count, Properties.Resources.caption_InstitutionRegistrations);

                    #endregion indicator 2 - student and registration count

                    //set indicator 3
                    #region indicator 3 - class and teacher count

                    //get number of active classes for selected institution
                    count = songChannel.CountClassesByFilter(
                        (int)ItemStatus.Active, -1, -1, -1,
                        Manager.CurrentSemester.SemesterId, institutionId, -1, -1);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Class);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator3.AddIndicator(
                        count.Count, Properties.Resources.caption_InstitutionClasses);

                    //get number of active teachers for selected institution
                    count = songChannel.CountTeachersByFilter(
                        (int)ItemStatus.Active, institutionId, -1);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Teacher);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator3.AddIndicator(
                        count.Count, Properties.Resources.caption_InstitutionTeachers);

                    #endregion indicator 3 - class and teacher count

                    //set indicator 4
                    #region indicator 4 - instrument and loan count

                    //get number of active instruments for selected institution
                    count = songChannel.CountInstrumentsByFilter(
                        (int)ItemStatus.Active, -1, institutionId, -1);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Instrument);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator4.AddIndicator(
                        count.Count, Properties.Resources.caption_InstitutionInstruments);

                    #endregion indicator 4 - instrument and loan count

                    //set indicator 8
                    #region indicator 8 - student average grade

                    //get grade average for present month and selected institution
                    average = songChannel.AverageGradesByFilter(
                        (int)GradeRapporteur.Teacher, (int)GradeTarget.Student,
                        (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                        Manager.CurrentSemester.SemesterId : -1, currentMonth,
                        institutionId, -1, -1, -1, -1);

                    //check result
                    if (average.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting average
                        //process average error
                        ProcessAverageError(average, Properties.Resources.item_Grade);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator8.AddIndicator(average.Average != double.MinValue ?
                        average.Average.ToString("0.00") : "--",
                        Properties.Resources.caption_InstitutionMonthGradeStudent);

                    //check if there is a previous month inside semester
                    if (Manager.CurrentSemester.StartDate.Month < DateTime.Now.Month)
                    {
                        //get last month
                        DateTime lastMonth = new DateTime(
                            DateTime.Now.Year, DateTime.Now.Month - 1, 1);

                        //get grade average for last month and selected institution
                        average = songChannel.AverageGradesByFilter(
                            (int)GradeRapporteur.Teacher, (int)GradeTarget.Student,
                            (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                            Manager.CurrentSemester.SemesterId : -1, lastMonth,
                            institutionId, -1, -1, -1, -1);

                        //check result
                        if (average.Result == (int)SelectResult.FatalError)
                        {
                            //database error while getting average
                            //process average error
                            ProcessAverageError(average, Properties.Resources.item_Grade);

                            //could not load indicators
                            //exit
                            return;
                        }

                        //add data to indicator
                        icIndicator8.AddIndicator(average.Average != double.MinValue ?
                            average.Average.ToString("0.00") : "--",
                            Properties.Resources.caption_InstitutionLastMonthGradeStudent);
                    }

                    //get grade average for present semester
                    average = songChannel.AverageGradesByFilter(
                        (int)GradeRapporteur.Teacher, (int)GradeTarget.Student,
                        (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                        Manager.CurrentSemester.SemesterId : -1, DateTime.MinValue,
                        institutionId, -1, -1, -1, -1);

                    //check result
                    if (average.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting average
                        //process average error
                        ProcessAverageError(average, Properties.Resources.item_Grade);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator8.AddIndicator(average.Average != double.MinValue ?
                        average.Average.ToString("0.00") : "--",
                        Properties.Resources.caption_InstitutionSemesterGradeStudent);

                    #endregion indicator 8 - student average grade

                    //set indicator 9
                    #region indicator 9 - evasion

                    //get number of evations for present month and selected institution
                    count = songChannel.CountEvationsByFilter(
                        Manager.CurrentSemester.SemesterId, currentMonth, institutionId, -1);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Evasion);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator9.AddIndicator(
                        count.Count, Properties.Resources.caption_InstitutionMonthEvation);

                    //check if there is a previous month inside semester
                    if (Manager.CurrentSemester.StartDate.Month < DateTime.Now.Month)
                    {
                        //get last month
                        DateTime lastMonth = new DateTime(
                            DateTime.Now.Year, DateTime.Now.Month - 1, 1);

                        //get number of evations for last month and selected institution
                        count = songChannel.CountEvationsByFilter(
                            Manager.CurrentSemester.SemesterId, lastMonth, institutionId, -1);

                        //check result
                        if (count.Result == (int)SelectResult.FatalError)
                        {
                            //database error while getting count
                            //process count error
                            ProcessCountError(count, Properties.Resources.item_Evasion);

                            //could not load indicators
                            //exit
                            return;
                        }

                        //add data to indicator
                        icIndicator9.AddIndicator(
                            count.Count, Properties.Resources.caption_InstitutionLastMonthEvation);
                    }

                    //get number of evations for present semester and selected institution
                    count = songChannel.CountEvationsByFilter(
                        Manager.CurrentSemester.SemesterId, DateTime.MinValue, institutionId, -1);

                    //check result
                    if (count.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting count
                        //process count error
                        ProcessCountError(count, Properties.Resources.item_Evasion);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator9.AddIndicator(
                        count.Count, Properties.Resources.caption_InstitutionSemesterEvation);

                    #endregion indicator 9 - evasion

                    //set indicator 10
                    #region indicator 10 - teacher average grade

                    //get grade average for present month and selected institution
                    average = songChannel.AverageGradesByFilter(
                        (int)GradeRapporteur.Coordinator, (int)GradeTarget.Teacher,
                        (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                        Manager.CurrentSemester.SemesterId : -1, currentMonth,
                        institutionId, -1, -1, -1, -1);

                    //check result
                    if (average.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting average
                        //process average error
                        ProcessAverageError(average, Properties.Resources.item_Grade);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator10.AddIndicator(average.Average != double.MinValue ?
                        average.Average.ToString("0.00") : "--",
                        Properties.Resources.caption_InstitutionMonthGradeTeacher);

                    //check if there is a previous month inside semester
                    if (Manager.CurrentSemester.StartDate.Month < DateTime.Now.Month)
                    {
                        //get last month
                        DateTime lastMonth = new DateTime(
                            DateTime.Now.Year, DateTime.Now.Month - 1, 1);

                        //get grade average for last month and selected institution
                        average = songChannel.AverageGradesByFilter(
                        (int)GradeRapporteur.Coordinator, (int)GradeTarget.Teacher,
                        (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                        Manager.CurrentSemester.SemesterId : -1, lastMonth,
                        institutionId, -1, -1, -1, -1);

                        //check result
                        if (average.Result == (int)SelectResult.FatalError)
                        {
                            //database error while getting average
                            //process average error
                            ProcessAverageError(average, Properties.Resources.item_Grade);

                            //could not load indicators
                            //exit
                            return;
                        }

                        //add data to indicator
                        icIndicator10.AddIndicator(average.Average != double.MinValue ?
                            average.Average.ToString("0.00") : "--",
                            Properties.Resources.caption_InstitutionLastMonthGradeTeacher);
                    }

                    //get grade average for present semester and selected institution
                    average = songChannel.AverageGradesByFilter(
                        (int)GradeRapporteur.Coordinator, (int)GradeTarget.Teacher,
                        (int)GradePeriodicity.Month, -1, Manager.CurrentSemester.SemesterId > 0 ?
                        Manager.CurrentSemester.SemesterId : -1, DateTime.MinValue,
                        institutionId, -1, -1, -1, -1);

                    //check result
                    if (average.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting average
                        //process average error
                        ProcessAverageError(average, Properties.Resources.item_Grade);

                        //could not load indicators
                        //exit
                        return;
                    }

                    //add data to indicator
                    icIndicator10.AddIndicator(average.Average != double.MinValue ?
                        average.Average.ToString("0.00") : "--",
                        Properties.Resources.caption_InstitutionSemesterGradeTeacher);

                    #endregion indicator 10 - teacher average grade
                }
            }
            catch (Exception ex)
            {
                //database error while getting institutions
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Indicator), ex);

                //log exception
                Manager.Log.WriteException(ex);

                //exit
                return;
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

            //load upcoming events
            //get song channel
            songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //exit
                return;
            }

            try
            {
                //get all upcoming events
                List<Event> upcomingEvents = songChannel.FindEventsByFilter(
                    false, -1, DateTime.Today, DateTime.MinValue);

                //check result
                if (upcomingEvents[0].Result == (int)SelectResult.Empty)
                {
                    //no event is available
                    //clear list
                    upcomingEvents.Clear();
                }
                else if (upcomingEvents[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting semesters
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Event, upcomingEvents[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Event,
                        upcomingEvents[0].ErrorMessage));

                    //clear list
                    upcomingEvents.Clear();
                }

                //sort events by start time
                upcomingEvents.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));

                //lock data table of events
                lock (dtEvents)
                {
                    //check each event in the list
                    foreach (Event eventObj in upcomingEvents)
                    {
                        //create, set and add event row
                        DataRow dr = dtEvents.NewRow();
                        SetEventDataRow(dr, eventObj);
                        dtEvents.Rows.Add(dr);
                    }
                }

                //set source to datagrid
                dgvEvents.DataSource = dtEvents;

                //sort events by name by default
                dgvEvents.Sort(EventDate, ListSortDirection.Ascending);
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

                //exit
                return;
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
            
            //generate notices
            //clear list of notices
            notices.Clear();

            //get song channel
            songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //exit
                return;
            }

            try
            {
                //list of pending reports
                List<Report> pendingReports = null;

                //check if logged on user is a teacher
                if (Manager.LogonTeacher != null)
                {
                    //get list of pending reports for logon teacher
                    //might be a coordinator
                    pendingReports = songChannel.FindReportsByFilter(
                        true, true, true, true, true,
                        (int)ReportStatus.Pending,
                        (int)ReportRapporteur.Teacher,
                        -1,
                        Manager.CurrentSemester.SemesterId,
                        DateTime.MinValue,
                        -1,
                        Manager.LogonTeacher.TeacherId,
                        -1);
                }
                else
                {
                    //get list of pending reports for logon user
                    //might be a coordinator
                    pendingReports = songChannel.FindReportsByFilter(
                        true, true, true, true, true,
                        (int)ReportStatus.Pending,
                        (int)ReportRapporteur.Coordinator,
                        -1,
                        Manager.CurrentSemester.SemesterId,
                        DateTime.MinValue,
                        -1,
                        -1,
                        -1);

                    //filter reports for logon user
                    pendingReports = pendingReports.FindAll(
                        r => r.CoordinatorId == Manager.LogonUser.UserId);
                }

                //check result
                if (pendingReports.Count == 0 ||
                    pendingReports[0].Result == (int)SelectResult.Empty)
                {
                    //no report
                    //do nothing
                }
                else if (pendingReports[0].Result == (int)SelectResult.Success)
                {
                    //create a notice for each pending report
                    foreach (Report report in pendingReports)
                    {
                        //create text
                        StringBuilder sbText = new StringBuilder(64);

                        //add title
                        sbText.Append(Properties.Resources.titleReportPending);
                        sbText.Append(": ");

                        //add period
                        sbText.Append(report.SemesterDescription);
                        sbText.Append(".");
                        sbText.Append(report.ReferenceDate.Month);
                        sbText.Append(" | ");

                        //check rapporteur
                        if (report.ReportRapporteur == (int)ReportRapporteur.Coordinator)
                        {
                            //add institution
                            sbText.Append(report.InstitutionName);
                        }
                        else if (report.ReportRapporteur == (int)ReportRapporteur.Teacher)
                        {
                            //add class description
                            sbText.Append(Manager.GetClassDescription(report.Class, false));
                        }

                        //create and set notice for pending report
                        Notice notice = new Notice();
                        notice.Report = report;
                        notice.Text = sbText.ToString();

                        //add created notice
                        notices.Add(notice);
                    }
                }
                else if (pendingReports[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting reports
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        Properties.Resources.item_Report, pendingReports[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceFilterItem,
                        Properties.Resources.item_Report, pendingReports[0].ErrorMessage));

                    //could not load reports
                    //exit
                    return;
                }
            }
            catch (Exception ex)
            {
                //show error message
                MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelFilterItem, 
                    Properties.Resources.item_Report, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //database error while getting reports
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelFilterItem, 
                    Properties.Resources.item_Report, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load reports
                //exit
                return;
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

            //check number of generated notices
            if (notices.Count > 0)
            {
                //lock data table of notices
                lock (dtNotices)
                {
                    //check each notice in the list
                    foreach (Notice notice in notices)
                    {
                        //create, set and add notice row
                        DataRow dr = dtNotices.NewRow();
                        SetNoticeDataRow(dr, notice);
                        dtNotices.Rows.Add(dr);
                    }
                }

                //set source to datagrid
                dgvNotices.DataSource = dtNotices;
            }


            //start indicators timer
            timIndicators.Start();
        }

        /// <summary>
        /// Indicators timer click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timIndicators_Tick(object sender, EventArgs e)
        {
            //allow user to select events
            allowDataGridViewSelection = true;

            //increment elapsed time
            elapsedTime += timIndicators.Interval;

            //check elapsed time
            if (elapsedTime < REFRESH_INTERVAL)
            {
                //exit
                return;
            }

            //display next indicator
            //calculate index of the indicator to be refreshed
            int index = (elapsedTime % REFRESH_INTERVAL) / timIndicators.Interval;

            //refresh indicator by displaying next data
            indicators[index].Next();

            //check index
            if (index == indicators.Count - 1)
            {
                //list has ended
                //reset elapsed time
                elapsedTime = 0;
            }
        }

        /// <summary>
        /// Events datagridview selection changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvEvents_SelectionChanged(object sender, EventArgs e)
        {
            //check if user is already allowed to select events
            if (allowDataGridViewSelection)
            {
                //exit
                return;
            }

            //clear default selection
            dgvEvents.ClearSelection();
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
            clickedEventId = int.MinValue;

            //check if there is a selected event
            if (dgvEvents.SelectedRows.Count > 0)
            {
                //there is one or more events selected
                //get first selected event
                for (int index = 0; index < dgvEvents.SelectedRows.Count; index++)
                {
                    //set event id
                    clickedEventId = (int)dgvEvents.SelectedRows[index].Cells[columnIndexEventId].Value;
                }

                //check result
                if (clickedEventId == int.MinValue)
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
            //check clicked event id
            if (clickedEventId == int.MinValue)
            {
                //should never happen
                //exit
                return;
            }

            //create control to display selected event
            RegisterEventControl registerControl =
                new UI.Controls.RegisterEventControl();
            registerControl.ParentControl = this;
            registerControl.FirstSelectedId = clickedEventId;

            //set child control
            childControl = registerControl;

            //display control
            Manager.MainForm.AddAndDisplayControl(registerControl);
        }

        /// <summary>
        /// Notices datagridview selection changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvNotices_SelectionChanged(object sender, EventArgs e)
        {
            //check if user is already allowed to select events
            if (allowDataGridViewSelection)
            {
                //exit
                return;
            }

            //clear default selection
            dgvNotices.ClearSelection();
        }

        /// <summary>
        /// Notices datagridview mouse up notice handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvNotices_MouseUp(object sender, MouseEventArgs e)
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
            DataGridView.HitTestInfo hitInfo = dgvNotices.HitTest(e.X, e.Y);

            //check selected row index
            if (hitInfo.RowIndex < 0)
            {
                //no row selected
                //exit
                return;
            }

            //check if there are selected rows and if notice clicked on them
            if (dgvNotices.SelectedRows.Count > 0 &&
                dgvNotices.Rows[hitInfo.RowIndex].Selected != true)
            {
                //notice did not click in the selected rows
                //clear selection
                dgvNotices.ClearSelection();

                //select clicked row
                dgvNotices.Rows[hitInfo.RowIndex].Selected = true;
            }
            //check if there are selected cells
            else if (dgvNotices.SelectedCells.Count > 0)
            {
                //get list of selected cell rows
                HashSet<int> selectedRowIndexes = new HashSet<int>();

                //check each selected cell
                foreach (DataGridViewCell cell in dgvNotices.SelectedCells)
                {
                    //add cell row index to the list
                    selectedRowIndexes.Add(cell.RowIndex);
                }

                //check if notice clicked on a row of a selected cell
                if (selectedRowIndexes.Contains(hitInfo.RowIndex))
                {
                    //notice clicked on a row of a selected cell
                    //select all rows
                    foreach (int selectedRowIndex in selectedRowIndexes)
                    {
                        //select row
                        dgvNotices.Rows[selectedRowIndex].Selected = true;
                    }
                }
                else
                {
                    //notice did not click on a row of a selected cell
                    //clear selected cells
                    dgvNotices.ClearSelection();

                    //select clicked row
                    dgvNotices.Rows[hitInfo.RowIndex].Selected = true;
                }
            }
            else
            {
                //select clicked row
                dgvNotices.Rows[hitInfo.RowIndex].Selected = true;
            }

            //get selected or clicked notice
            clickedNoticeId = int.MinValue;

            //check if there is a selected notice
            if (dgvNotices.SelectedRows.Count > 0)
            {
                //there is one or more notices selected
                //get first selected notice
                for (int index = 0; index < dgvNotices.SelectedRows.Count; index++)
                {
                    //set notice id
                    clickedNoticeId = (int)dgvNotices.SelectedRows[index].Cells[columnIndexNoticeId].Value;
                }

                //check result
                if (clickedNoticeId == int.MinValue)
                {
                    //no notice report was found
                    //do not display context menu
                    //exit
                    return;
                }
            }
            else
            {
                //there is no notice selected
                //should never happen
                //do not display context menu
                //exit
                return;
            }

            //show notice context menu on the clicked point
            mcmNotice.Show(this.dgvNotices, p);
        }
        
        /// <summary>
        /// View report menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewReport_Click(object sender, EventArgs e)
        {
            //check clicked notice id
            if (clickedNoticeId == int.MinValue)
            {
                //should never happen
                //exit
                return;
            }

            //get selected notice
            Notice notice = notices.Find(n => n.NoticeId == clickedNoticeId);

            //check result
            if (notice == null)
            {
                //should never happen
                //exit
                return;
            }

            //check notice report
            if (notice.Report == null)
            {
                //should never happen
                //exit
                return;
            }

            //get notice report
            Report report = notice.Report;

            //must create edit report control according to selected report
            ISongControl editControl = null;

            //check report rapporteur
            if (report.ReportRapporteur == (int)ReportRapporteur.Teacher)
            {
                //create teacher month control
                editControl = new EditReportTeacherMonth(report);

                //set parent control to current control
                ((EditReportTeacherMonth)editControl).ParentControl = this;
            }
            else if (report.ReportRapporteur == (int)ReportRapporteur.Coordinator)
            {
                //create coordinator month control
                editControl = new EditReportCoordinatorMonth(report);

                //set parent control to current control
                ((EditReportCoordinatorMonth)editControl).ParentControl = this;
            }
            else
            {
                //should never happen
                //exit
                return;
            }

            //set child control
            childControl = editControl;

            //display control
            Manager.MainForm.AddAndDisplayControl((UserControl)editControl);
        }

        #endregion UI Event Handlers

    } //end of class HomeControl

    /// <summary>
    /// Displayed home control notice.
    /// </summary>
    public class Notice
    {
        private static int noticeCount = 1;
        private int noticeId = noticeCount++;

        public int NoticeId { get { return noticeId; }}
        public Report Report { get; set; }
        public string Text { get; set; }
    }

} //end of namespace PnT.SongClient.UI.Controls
