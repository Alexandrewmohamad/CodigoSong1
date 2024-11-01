using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetroFramework;
using MetroFramework.Forms;
using MetroFramework.Controls;

using PnT.SongClient.Data;
using PnT.SongClient.Logic;
using PnT.SongClient.UI.Controls;

namespace PnT.SongClient.UI
{

    #region Delegates *****************************************************************

    /// <summary>
    /// Delegate to invoke ShowStatusMessage method.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="time"></param>
    public delegate void ShowStatusMessageDelegate(string message, int time);

    /// <summary>
    /// Delegate to invoke a click event handler method.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ClickDelegate(object sender, EventArgs e);

    #endregion Delegates


    /// <summary>
    /// Application main form.
    /// </summary>
    public partial class MainForm : MetroForm
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The current selected menu tile.
        /// </summary>
        private MetroTile selectedMenuTile = null;

        /// <summary>
        /// The current displayed control.
        /// </summary>
        private UserControl displayedControl = null;

        /// <summary>
        /// List of added controls.
        /// </summary>
        private List<UserControl> addedControls = null;

        /// <summary>
        /// The list of menu options.
        /// Key is the menu item name.
        /// </summary>
        private Dictionary<string, MetroTile> menuOptions = null;

        /// <summary>
        /// True if it is the first logon of the local user.
        /// </summary>
        private bool isFirstLogon = true;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainForm()
        {
            //set thread culture
            Thread.CurrentThread.CurrentCulture = Properties.Settings.Default.Culture;

            //set thread ui culture
            Thread.CurrentThread.CurrentUICulture = Properties.Settings.Default.Culture;

            //init ui components
            InitializeComponent();

            //create list of added controls
            addedControls = new List<UserControl>();

            //create list of menu options
            menuOptions = new Dictionary<string, MetroTile>();
            menuOptions["Calendar"] = mtlCalendar;
            menuOptions["Class"] = mtlClasses;
            menuOptions["Document"] = mtlDocuments;
            menuOptions["Home"] = mtlHome;
            menuOptions["Institution"] = mtlInstitutions;
            menuOptions["Instrument"] = mtlInstruments;
            menuOptions["Pole"] = mtlPoles;
            menuOptions["Report"] = mtlReports;
            menuOptions["Registration"] = mtlRegistrations;
            menuOptions["Statistic"] = mtlStatistics;
            menuOptions["Student"] = mtlStudents;
            menuOptions["Teacher"] = mtlTeachers;
            menuOptions["Option"] = mtlOptions;
            menuOptions["User"] = mtlUsers;

            //reset selected menu tile
            selectedMenuTile = null;
        }

        #endregion Constructors


        #region Public Methods ********************************************************

        /// <summary>
        /// Add and display selected user control in the content panel.
        /// </summary>
        /// <param name="control">
        /// The selected user control.
        /// </param>
        public void AddAndDisplayControl(UserControl control)
        {
            //check if control is not added yet
            if (!this.addedControls.Contains(control))
            {
                //add control to the list of added controls
                this.addedControls.Add(control);

                //add control to content panel
                control.Dock = DockStyle.Fill;
                this.mpnContent.Controls.Add(control);
            }

            //display control by bringing it to front
            control.BringToFront();
            
            //update displayed control
            displayedControl = control;

            //check if a menu tile should be selected
            //get control selected menu option
            String menuOption = ((ISongControl)control).SelectMenuOption();

            //check result
            if (menuOption != null && menuOption.Length > 0)
            {
                //the selected menu option tile
                MetroTile menuOptionTile = null;

                //try to get menu option tile
                if (menuOptions.TryGetValue(menuOption, out menuOptionTile))
                {
                    //select menu option tile
                    SelectMenuTile(menuOptionTile);
                }
                else
                {
                    //clear menu tile selection
                    UnselectMenuTile();
                }
            }
            else
            {
                //clear menu tile selection
                UnselectMenuTile();
            }
        }

        /// <summary>
        /// Display impersonation confirmation and 
        /// let current user impersonate selected user.
        /// </summary>
        /// <param name="userId">
        /// The id of the selected user.
        /// </param>
        public void ConfirmAndImpersonateUser(int userId, string userName)
        {
            //display message
            if (MetroMessageBox.Show(this, string.Format(
                Properties.Resources.msgImpersonateUser, userName),
                Properties.Resources.titleImpersonateUser,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                //user canceled operation
                //exit
                return;
            }
            
            //the song channel
            SongServer.ISongService songChannel = null;

            try
            {
                //get song channel
                songChannel = Manager.WebServiceManager.GetSongChannel();

                //check result
                if (songChannel == null)
                {
                    //channel is not available at the moment
                    //could not load data
                    return;
                }

                //get selected user from web service
                SongDB.Logic.User impersonatedUser = songChannel.FindUser(userId);

                //check result
                if (impersonatedUser.Result == (int)SongDB.Logic.SelectResult.Empty)
                {
                    //should never happen
                    //could not load data
                    return;
                }
                else if (impersonatedUser.Result == (int)SongDB.Logic.SelectResult.FatalError)
                {
                    //database error while getting user
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        Properties.Resources.item_User, impersonatedUser.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        Properties.Resources.item_User, impersonatedUser.ErrorMessage));

                    //could not load data
                    return;
                }
                
                //get assigned permissions for assigned role
                List<SongDB.Logic.Permission> permissions = 
                    songChannel.FindPermissionsByRole(impersonatedUser.RoleId);

                //check result
                if (permissions[0].Result == (int)SongDB.Logic.SelectResult.Empty)
                {
                    //role has no permission
                    //clear list
                    permissions.Clear();
                }
                else if (permissions[0].Result == (int)SongDB.Logic.SelectResult.FatalError)
                {
                    //database error while getting permissions
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadPermissions, 
                        Properties.Resources.item_User, permissions[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadPermissions,
                        Properties.Resources.item_User, permissions[0].ErrorMessage));

                    //exit
                    return;
                }

                //get current logon user and its permissions
                SongDB.Logic.User impersonatingUser = Manager.LogonUser;
                List<SongDB.Logic.Permission> impersonatingPermissions = Manager.ListLogonPermissions;

                //reset logon user
                Manager.ResetLogonUser();

                //remove all added controls
                RemoveAndDisposeAllControls();

                //unload permissions
                UnloadLogonUserPermissions();

                //delete all cache files
                RegisterBaseControl.DeleteAllItemsFromDisk();

                //set logged on user with impersonation
                Manager.SetLogonUser(
                    impersonatedUser, permissions, 
                    impersonatingUser, impersonatingPermissions);

                //update displayed title
                UpdateTitle();

                //load logon user permissions into UI
                LoadLogonUserPermissions();

                //select home option
                mtlMenuOptions_Click(mtlHome, new EventArgs());
            }
            catch (Exception ex)
            {
                //show error message
                MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelLoadData, 
                    Properties.Resources.item_User, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log exception
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelLoadData, 
                    Properties.Resources.item_User, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load data
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
        }

        /// <summary>
        /// Display class control. Close other controls.
        /// </summary>
        public void DisplayClasses()
        {
            //check if user has permission to view classes
            if (!Manager.HasLogonPermission("Class.View"))
            {
                //user has no permission
                //cannot display class control
                //exit
                return;
            }

            //remove all added controls
            RemoveAndDisposeAllControls();

            //create class control
            ViewClassControl classControl = new ViewClassControl();

            //add and display class control
            AddAndDisplayControl(classControl);
        }

        /// <summary>
        /// Display report control. Close other controls.
        /// </summary>
        public void DisplayReports(int semesterId, int classId)
        {
            //check if user has permission to view reports
            if (!Manager.HasLogonPermission("Report.View"))
            {
                //user has no permission
                //cannot display report control
                //exit
                return;
            }

            //remove all added controls
            RemoveAndDisposeAllControls();

            //create report control with filters
            ViewReportControl reportControl = new ViewReportControl(semesterId, classId);

            //add and display report control
            AddAndDisplayControl(reportControl);
        }

        /// <summary>
        /// Remove all added controls from content panel. 
        /// </summary>
        public void RemoveAndDisposeAllControls()
        {
            //check number of added controls
            while (this.addedControls.Count > 0)
            {
                //remove last added control
                RemoveAndDisposeControl(this.addedControls[this.addedControls.Count - 1]);
            }

            //remove selected menu
            UnselectMenuTile();

            //reset displayed control
            displayedControl = null;
        }

        /// <summary>
        /// Remove selected user control from content panel 
        /// and dispose it.
        /// </summary>
        /// <param name="control">
        /// The selected user control.
        /// </param>
        public void RemoveAndDisposeControl(UserControl control)
        {
            //check if control is added
            if (this.addedControls.Contains(control))
            {
                //remove control from list of added controls
                this.addedControls.Remove(control);

                //remove control from content panel
                this.mpnContent.Controls.Remove(control);
                
                //dispose child song control
                ((ISongControl)control).DisposeControl();
            }

            //check if control is being displyaed
            if (displayedControl == control)
            {
                //remove selected menu
                UnselectMenuTile();

                //reset displayed control
                displayedControl = null;
            }
        }

        /// <summary>
        /// Show status message in the status bar for the selected amount of time.
        /// </summary>
        /// <param name="message">
        /// The message to be displayed.
        /// </param>
        /// <param name="time">
        /// The time period to display the message.
        /// 0 to display message until another message is displayed.
        /// </param>
        public void ShowStatusMessage(string message, int time)
        {
            //check if form is displayed
            if (!this.Created || this.Disposing || this.IsDisposed)
            {
                //form is not displayed
                //exit
                return;
            }

            //check if invoke is required
            if (this.InvokeRequired)
            {
                //invoke method
                this.Invoke(new ShowStatusMessageDelegate(ShowStatusMessage),
                    new object[] { message, time });

                //exir
                return;
            }

            //disable timer
            timMessage.Enabled = false;

            //check if label is being reset
            while (mlblMessage.Tag != null && (bool)mlblMessage.Tag)
            {
                //label is being reset
                //wait
                Thread.Sleep(1);
            }

            //lock label
            lock (mlblMessage)
            {
                //set message
                mlblMessage.Text = message;

                //check if time was set
                if (time > 0)
                {
                    //set time to timer
                    timMessage.Interval = time;

                    //start timer
                    timMessage.Enabled = true;
                }
            }
        }

        #endregion Public Methods


        #region Private Methods

        /// <summary>
        /// Display logon form and let user logon to system.
        /// </summary>
        private void DisplayLogon()
        {
            //create logon form
            LogonForm logonForm = new UI.LogonForm(isFirstLogon);

            //show logon form and check result
            if (logonForm.ShowDialog() == DialogResult.OK)
            {
                //check if client version is compatible
                if (Manager.WebServiceManager.ServerInfo != null &&
                    !Manager.WebServiceManager.ServerInfo.Version.Equals(Manager.Version))
                {
                    //incompatible version
                    //display message to client
                    MetroMessageBox.Show(this, string.Format(
                        Properties.Resources.errorIncompatibleVersion,
                        Manager.Version, Manager.WebServiceManager.ServerInfo.Version),
                        Properties.Resources.titleIncompatibleVersion,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //close main form
                    this.Close();

                    //exit
                    return;
                }

                //update idsplayed title
                UpdateTitle();

                //load logon user permissions into UI
                LoadLogonUserPermissions();

                //select home option
                mtlMenuOptions_Click(mtlHome, new EventArgs());

                //reset first logon option
                isFirstLogon = false;
            }
        }

        /// <summary>
        /// Load current logged on user permissions into UI options.
        /// </summary>
        private void LoadLogonUserPermissions()
        {
            //always visible options
            mtlHome.Visible = true;
            mtlOptions.Visible = true;

            //set menu options
            mtlCalendar.Visible = Manager.HasLogonPermission("Event.View");
            mtlClasses.Visible = Manager.HasLogonPermission("Class.View") || Manager.HasLogonPermission("Class.Teacher");
            mtlDocuments.Visible = false;
            mtlInstitutions.Visible = Manager.HasLogonPermission("Institution.View");
            mtlInstruments.Visible = Manager.HasLogonPermission("Instrument.View");
            mtlPoles.Visible = Manager.HasLogonPermission("Pole.View");
            mtlRegistrations.Visible = Manager.HasLogonPermission("Registration.View");
            mtlReports.Visible = Manager.HasLogonPermission("Report.View") || 
                Manager.HasLogonPermission("Institution.Coordinator") || Manager.HasLogonPermission("Class.Teacher");
            mtlStatistics.Visible = Manager.HasLogonPermission("Statistics.View");
            mtlStudents.Visible = Manager.HasLogonPermission("Student.View");
            mtlTeachers.Visible = Manager.HasLogonPermission("Teacher.View") || Manager.HasLogonPermission("Class.Teacher");
            mtlUsers.Visible = Manager.HasLogonPermission("User.View");

            //set edit role options
            mnuEditRoles.Visible = Manager.HasLogonPermission("Role.View");
            tssSeparator1.Visible = Manager.HasLogonPermission("Role.View");

            //set generate options
            mnuGenerateReportCards.Visible = Manager.HasLogonPermission("ReportCard.Generate");
            mnuImportClasses.Visible = Manager.HasLogonPermission("Class.Import");
            tssSeparator2.Visible = Manager.HasLogonPermission("Class.Import") ||
                Manager.HasLogonPermission("ReportCard.Generate");

        }

        /// <summary>
        /// Unkoad current logged on user permissions from UI options.
        /// </summary>
        private void UnloadLogonUserPermissions()
        {
            //options are always visible
            mtlOptions.Visible = true;

            //hide all options
            mtlHome.Visible = false;
            mtlCalendar.Visible = false;
            mtlClasses.Visible = false;
            mtlDocuments.Visible = false;
            mtlInstitutions.Visible = false;
            mtlInstruments.Visible = false;
            mtlPoles.Visible = false;
            mtlRegistrations.Visible = false;
            mtlReports.Visible = false;
            mtlStatistics.Visible = false;
            mtlStudents.Visible = false;
            mtlTeachers.Visible = false;
            mtlUsers.Visible = false;

            //hide menu options
            mnuEditRoles.Visible = false;
        }

        /// <summary>
        /// Update title according to current logged on user.
        /// </summary>
        private void UpdateTitle()
        {
            //check logged on user
            if (Manager.LogonUser != null)
            {
                //check if there is an impersonating user
                if (Manager.ImpersonatingUser != null)
                {
                    //set title with user login
                    this.Text = "Song  >  " + Manager.ImpersonatingUser.Login +
                        "  >  " + Manager.LogonUser.Login;
                }
                else
                {
                    //set title with user login
                    this.Text = "Song  >  " + Manager.LogonUser.Login;
                }

                //check if there is any selected institution
                if (Manager.LogonUser.InstitutionId > 0)
                {
                    //add institution
                    this.Text += " - " + Manager.LogonUser.InstitutionName;
                }
            }
            else
            {
                //set default title
                this.Text = "Song";
            }

#if (Homologation || Certification)
            //add designation to title
            this.Text = "[Homologação] " + this.Text;
#endif

            //redraw window
            this.Invalidate();
        }

        /// <summary>
        /// Select menu tile.
        /// </summary>
        private void SelectMenuTile(MetroTile menuTile)
        {
            //check if there is a selected menu tile
            if (selectedMenuTile != null)
            {
                //check if it is the same menu
                if (selectedMenuTile == menuTile)
                {
                    //no need to select menu
                    //exit
                    return;
                }
                else
                {
                    //unselect menu tile before continuing
                    UnselectMenuTile();
                }
            }

            //set reference to selected tile
            selectedMenuTile = menuTile;

            //set style
            selectedMenuTile.Style = MetroFramework.MetroColorStyle.Blue;

            //display default fore font color
            selectedMenuTile.UseCustomForeColor = false;

            //select blue image
            selectedMenuTile.TileImage = (Image)Properties.Resources.ResourceManager.GetObject(
                "Icon" + selectedMenuTile.Tag.ToString() + "Blue");

            //redraw tile
            selectedMenuTile.Refresh();
        }

        /// <summary>
        /// Set displayed web service status.
        /// </summary>
        /// <param name="status">The current web service status.</param>
        private void SetWebServiceStatus(WebServiceStatus status)
        {
            //display default forecolor if connected
            mlblWebServiceStatus.UseCustomForeColor = !(status == WebServiceStatus.Connected);

            //set text according to status
            mlblWebServiceStatus.Text = Properties.Resources.ResourceManager.GetString(
                    "WebServiceStatus_" + status.ToString());
        }

        /// <summary>
        /// Wait some time before displaying logon form.
        /// </summary>
        /// <param name="stateInfo"></param>
        private void WaitAndDisplayLogon(object stateInfo)
        {
            //wait some time before displaying logon
            Thread.Sleep(1000);

            //show logon on UI thread
            this.Invoke(
                new NoObjectDelegate(DisplayLogon),
                new object[] { });
        }

        /// <summary>
        /// Unselect current menu tile.
        /// </summary>
        private void UnselectMenuTile()
        {
            //check if there is a selected menu tile
            if (selectedMenuTile == null)
            {
                //exit
                return;
            }

            //set style
            selectedMenuTile.Style = MetroFramework.MetroColorStyle.White;

            //display custom fore font color
            selectedMenuTile.UseCustomForeColor = true;

            //select white image
            selectedMenuTile.TileImage = (Image)Properties.Resources.ResourceManager.GetObject(
                "Icon" + selectedMenuTile.Tag.ToString() + "White");

            //redraw tile
            selectedMenuTile.Refresh();

            //remove reference to selected tile
            selectedMenuTile = null;
        }

        #endregion Private Methods


        #region Application Event Handlers ********************************************

        /// <summary>
        /// Web service manager status changed event handler.
        /// </summary>
        /// <param name="status">The current status.</param>
        private void WebServiceManager_StatusChanged(Data.WebServiceStatus status)
        {
            try
            {
                //check if form is still on memory
                if (!this.Created || this.Disposing || this.IsDisposed)
                {
                    //not on memory
                    return;
                }

                //check if form is still on memory
                if (!this.Created || this.Disposing || this.IsDisposed)
                {
                    //not on memory
                    return;
                }

                //check if invoke is required
                if (this.InvokeRequired)
                {
                    //invoke AddError to change UI
                    //can't call method directly because might 
                    //be a different thread form the UI thread
                    this.Invoke(
                        new WebServiceStatusEventHandler(this.SetWebServiceStatus),
                        new object[] { status }
                        );
                }
                else
                {
                    //call method to add error
                    this.SetWebServiceStatus(status);
                }
            }
            catch (Exception ex)
            {
                //unexpected error adding error on main form
                Manager.Log.WriteException(
                    "Error while handling web service manager status changed event on main form.", ex);
            }
        }

        #endregion Application Event Handlers


        #region UI Event Handlers *****************************************************

        /// <summary>
        /// Main form load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //register to web service status changed event
            Manager.WebServiceManager.StatusChanged += WebServiceManager_StatusChanged;

            //display initial value
            SetWebServiceStatus(Manager.WebServiceManager.Status);
        }

        /// <summary>
        /// Main form show event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            //application was started
            Manager.Log.WriteInfo(Properties.Resources.msgAppStarted);

            //update title
            UpdateTitle();

            //show welcome message
            ShowStatusMessage(Properties.Resources.msgWelcome, 5000);

            //error message parameter
            string errorMessage = null;

            //start the web service manager
            if (!Manager.WebServiceManager.Start(ref errorMessage))
            {
                //should never happen
                //display message
                MetroMessageBox.Show(this, string.Format(
                    Properties.Resources.errorStartWebServiceManager, errorMessage),
                    Properties.Resources.titleFatalError);

                //exit application
                this.Close();
            }

            //activate windows
            this.Activate();

            //let user logon to system
            ThreadPool.QueueUserWorkItem(WaitAndDisplayLogon);
        }

        /// <summary>
        /// Main form closing event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //write message
            Manager.Log.WriteInfo(Properties.Resources.msgAppExiting);

            //check if there is a impersonating user
            if (Manager.ImpersonatingUser != null)
            {
                //delete all cache files
                RegisterBaseControl.DeleteAllItemsFromDisk();
            }

            //dispose application resources
            Manager.DisposeResources();
        }

        /// <summary>
        /// Menu option tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlMenuOptions_Click(object sender, EventArgs e)
        {
            //get clicked menu tile
            MetroTile clickedMenuTile = (MetroTile)sender;

            //check if clicked menu tile is the same selected menu tile
            if (clickedMenuTile.Equals(selectedMenuTile))
            {
                //no need to perform click
                //exit
                return;
            }

            //remove all added controls
            RemoveAndDisposeAllControls();

            //check selected menu tile and perform an action accordingly
            if (clickedMenuTile.Equals(mtlHome))
            {
                //create home control
                HomeControl homeControl = new HomeControl();

                //add and display home control
                AddAndDisplayControl(homeControl);
            }
            else if (clickedMenuTile.Equals(mtlCalendar))
            {
                //create event control
                ViewEventControl eventControl = new ViewEventControl();

                //add and display event control
                AddAndDisplayControl(eventControl);
            }
            else if (clickedMenuTile.Equals(mtlClasses))
            {
                //create class control
                ViewClassControl classControl = new ViewClassControl();

                //add and display class control
                AddAndDisplayControl(classControl);
            }
            else if (clickedMenuTile.Equals(mtlInstitutions))
            {
                //create institution control
                ViewInstitutionControl institutionControl = new ViewInstitutionControl();

                //add and display institution control
                AddAndDisplayControl(institutionControl);
            }
            else if (clickedMenuTile.Equals(mtlInstruments))
            {
                //create instrument control
                ViewInstrumentControl instrumentControl = new ViewInstrumentControl();

                //add and display instrument control
                AddAndDisplayControl(instrumentControl);
            }
            else if (clickedMenuTile.Equals(mtlPoles))
            {
                //create pole control
                ViewPoleControl poleControl = new ViewPoleControl();

                //add and display pole control
                AddAndDisplayControl(poleControl);
            }
            else if (clickedMenuTile.Equals(mtlRegistrations))
            {
                //create registration control
                ViewRegistrationControl registrationControl = new ViewRegistrationControl();

                //add and display registration control
                AddAndDisplayControl(registrationControl);
            }
            else if (clickedMenuTile.Equals(mtlReports))
            {
                //create report control
                ViewReportControl reportControl = new ViewReportControl();

                //add and display report control
                AddAndDisplayControl(reportControl);
            }
            else if (clickedMenuTile.Equals(mtlStatistics))
            {
                //create statistics control
                StatisticsControl statisticsControl = new StatisticsControl();

                //add and display statistics control
                AddAndDisplayControl(statisticsControl);
            }
            else if (clickedMenuTile.Equals(mtlStudents))
            {
                //create student control
                ViewStudentControl studentControl = new ViewStudentControl();

                //add and display student control
                AddAndDisplayControl(studentControl);
            }
            else if (clickedMenuTile.Equals(mtlTeachers))
            {
                //create teacher control
                ViewTeacherControl teacherControl = new ViewTeacherControl();

                //add and display teacher control
                AddAndDisplayControl(teacherControl);
            }
            else if (clickedMenuTile.Equals(mtlUsers))
            {
                //create user control
                ViewUserControl userControl = new ViewUserControl();

                //add and display user control
                AddAndDisplayControl(userControl);
            }
        }

        /// <summary>
        /// Menu tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlOptions_Click(object sender, EventArgs e)
        {
            //check if options context menu is already show
            if(mcmOptions.Visible)
            {
                //hide context menu
                mcmOptions.Visible = false;
            }
            else
            {
                //check if context menu was not closed just a few miliseconds ago
                if (mcmOptions.Tag == null ||
                    DateTime.Now.Subtract((DateTime)mcmOptions.Tag).TotalMilliseconds > 500)
                {
                    //show context menu
                    mcmOptions.Show(mtlOptions, new Point(0, mtlOptions.Height));

                    //remove reference time
                    mcmOptions.Tag = null;
                }
            }
        }

        /// <summary>
        /// Options context menu visible changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcmOptions_VisibleChanged(object sender, EventArgs e)
        {
            //set current time to context menu
            mcmOptions.Tag = DateTime.Now;
        }

        /// <summary>
        /// Options context menu opening event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcmOptions_Opening(object sender, CancelEventArgs e)
        {
            //get logon users
            SongDB.Logic.User logonUser = Manager.LogonUser;
            SongDB.Logic.User impersonatingUser = Manager.ImpersonatingUser;
            
            //hide desimpersonate option by default
            mnuFileDesimpersonate.Visible = false;

            //check if there is any logged on user
            if (logonUser != null)
            {
                //check if there is any impersonating user
                if (impersonatingUser != null)
                {
                    //display logoff option for impersonating user
                    mnuFileLogIn.Text = string.Format(
                        Properties.Resources.menuLogOff, impersonatingUser.Login);

                    //display desimpersonate option from impersonated user
                    mnuFileDesimpersonate.Text = string.Format(
                        Properties.Resources.menuDesimpersonate, logonUser.Login);
                    mnuFileDesimpersonate.Visible = true;
                }
                else
                {
                    //display logoff option for logon user
                    mnuFileLogIn.Text = string.Format(
                        Properties.Resources.menuLogOff, logonUser.Login);
                }
            }
            else
            {
                //display login option
                mnuFileLogIn.Text = Properties.Resources.menuLogIn;
            }
        }

        /// <summary>
        /// Desimpersonate user menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuFileDesimpersonate_Click(object sender, EventArgs e)
        {
            //get current impersonating user and its permissions
            SongDB.Logic.User user = Manager.ImpersonatingUser;
            List<SongDB.Logic.Permission> permissions = Manager.ListImpersonatingPermissions;

            //reset logon user
            Manager.ResetLogonUser();

            //remove all added controls
            RemoveAndDisposeAllControls();

            //unload permissions
            UnloadLogonUserPermissions();

            //delete all cache files
            RegisterBaseControl.DeleteAllItemsFromDisk();

            //set logged on user without impersonation
            Manager.SetLogonUser(user, permissions);

            //update displayed title
            UpdateTitle();

            //load logon user permissions into UI
            LoadLogonUserPermissions();

            //select home option
            mtlMenuOptions_Click(mtlHome, new EventArgs());
        }

        /// <summary>
        /// Log in / off menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuFileLogIn_Click(object sender, EventArgs e)
        {
            //get logon user
            SongDB.Logic.User logonUser = Manager.LogonUser;

            //check if there is any logged on user
            if (logonUser != null)
            {
                //logoff user
                //check if there is a impersonating user
                if (Manager.ImpersonatingUser != null)
                {
                    //delete all cache files
                    RegisterBaseControl.DeleteAllItemsFromDisk();
                }

                //reset logon user
                Manager.ResetLogonUser();

                //remove all added controls
                RemoveAndDisposeAllControls();

                //unload permissions
                UnloadLogonUserPermissions();

                //update displayed title
                UpdateTitle();

                //let user log in
                DisplayLogon();
            }
            else
            {
                //let user log in
                DisplayLogon();
            }
        }

        /// <summary>
        /// File exit menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            //close main form
            this.Close();
        }

        /// <summary>
        /// Edit roles menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuEditRoles_Click(object sender, EventArgs e)
        {
            //remove all added controls
            RemoveAndDisposeAllControls();

            //create role control
            RegisterRoleControl roleControl = new RegisterRoleControl();

            //add and display role control
            AddAndDisplayControl(roleControl);
        }

        /// <summary>
        /// Generate report cards menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuGenerateReportCards_Click(object sender, EventArgs e)
        {
            //remove all added controls
            RemoveAndDisposeAllControls();

            //create report card control
            GenerateReportCard reportCardControl = new GenerateReportCard();

            //add and display report card control
            AddAndDisplayControl(reportCardControl);
        }

        /// <summary>
        /// Import classes menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuImportClasses_Click(object sender, EventArgs e)
        {
            //remove all added controls
            RemoveAndDisposeAllControls();

            //create import class control
            ImportClassControl importClassControl = new ImportClassControl();

            //add and display report card control
            AddAndDisplayControl(importClassControl);
        }

        /// <summary>
        /// Edit options menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuEditOptions_Click(object sender, EventArgs e)
        {
            //show options form as a dialog
            OptionsForm optionsForm = new OptionsForm(null);
            optionsForm.ShowDialog(this);

            //check current connection status
            if (Manager.WebServiceManager.Status == WebServiceStatus.Connected)
            {
                //check if server settings were edited
                if (optionsForm.HasEditedServer)
                {
                    //must restart the web service manager
                    //stop the web service manager
                    Manager.WebServiceManager.Stop();

                    //error message parameter
                    string errorMessage = null;

                    //start the web service manager
                    if (!Manager.WebServiceManager.Start(ref errorMessage))
                    {
                        //should never happen
                        //display message
                        MetroMessageBox.Show(this, string.Format(
                            Properties.Resources.errorStartWebServiceManager, errorMessage),
                            Properties.Resources.titleFatalError);

                        //exit application
                        this.Close();
                    }

                    //check if there is a logged on user
                    if (Manager.LogonUser != null)
                    {
                        //log off user
                        //reset first logon flag
                        isFirstLogon = true;

                        //simulate log off menu click event
                        mnuFileLogIn_Click(this, new EventArgs());
                    }
                }
            }

            //dispose window
            optionsForm.Dispose();
            optionsForm = null;
        }

        /// <summary>
        /// View log file menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewLogFile_Click(object sender, EventArgs e)
        {
            //open file with default text editor for TXT extension
            FileInfo logFile = new FileInfo(Manager.Log.LogFilePath);

            try
            {
                //start process
                System.Diagnostics.Process.Start(logFile.FullName);
            }
            catch (Exception ex)
            {
                //could not start IE
                Manager.Log.WriteException(
                    "Could not open LOG file on default text editor.", ex);
            }
        }

        /// <summary>
        /// Help abou menu click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            //show about form as a dialog
            AboutMetroForm aboutForm = new AboutMetroForm();
            aboutForm.ShowDialog();

            //dispose window
            aboutForm.Dispose();
            aboutForm = null;
        }

        /// <summary>
        /// Message timer tick event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timMessage_Tick(object sender, EventArgs e)
        {
            //stop timer
            timMessage.Enabled = false;

            //indicate that message is being reset
            mlblMessage.Tag = true;

            //lock label
            lock (mlblMessage)
            {

                //clear message
                mlblMessage.Text = string.Empty;
                mlblMessage.Image = null;
            }

            //message was reset
            mlblMessage.Tag = false;
        }

        #endregion UI Event Handlers

    } //end of class MainForm

} //end of namespace PnT.SongClient.UI