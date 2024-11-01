namespace PnT.SongClient.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnMenu = new System.Windows.Forms.Panel();
            this.mtlOptions = new MetroFramework.Controls.MetroTile();
            this.tlpMenu = new System.Windows.Forms.TableLayoutPanel();
            this.mtlRegistrations = new MetroFramework.Controls.MetroTile();
            this.mtlClasses = new MetroFramework.Controls.MetroTile();
            this.mtlInstruments = new MetroFramework.Controls.MetroTile();
            this.mtlStudents = new MetroFramework.Controls.MetroTile();
            this.mtlTeachers = new MetroFramework.Controls.MetroTile();
            this.mtlPoles = new MetroFramework.Controls.MetroTile();
            this.mtlInstitutions = new MetroFramework.Controls.MetroTile();
            this.mtlUsers = new MetroFramework.Controls.MetroTile();
            this.mtlCalendar = new MetroFramework.Controls.MetroTile();
            this.mtlDocuments = new MetroFramework.Controls.MetroTile();
            this.mtlStatistics = new MetroFramework.Controls.MetroTile();
            this.mtlHome = new MetroFramework.Controls.MetroTile();
            this.mtlReports = new MetroFramework.Controls.MetroTile();
            this.pnContent = new System.Windows.Forms.Panel();
            this.mpnContent = new MetroFramework.Controls.MetroPanel();
            this.mlblMessage = new MetroFramework.Controls.MetroLabel();
            this.mlblWebServiceStatus = new MetroFramework.Controls.MetroLabel();
            this.mcmOptions = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.mnuEditRoles = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuGenerateReportCards = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImportClasses = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEditOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewLogFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileDesimpersonate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileLogIn = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.timMessage = new System.Windows.Forms.Timer(this.components);
            this.tlpMain.SuspendLayout();
            this.pnMenu.SuspendLayout();
            this.tlpMenu.SuspendLayout();
            this.pnContent.SuspendLayout();
            this.mcmOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.BackColor = System.Drawing.Color.White;
            this.tlpMain.Controls.Add(this.pnMenu, 0, 0);
            this.tlpMain.Controls.Add(this.pnContent, 1, 0);
            this.tlpMain.Controls.Add(this.mlblMessage, 1, 1);
            this.tlpMain.Controls.Add(this.mlblWebServiceStatus, 2, 1);
            this.tlpMain.Name = "tlpMain";
            // 
            // pnMenu
            // 
            resources.ApplyResources(this.pnMenu, "pnMenu");
            this.pnMenu.Controls.Add(this.mtlOptions);
            this.pnMenu.Controls.Add(this.tlpMenu);
            this.pnMenu.Name = "pnMenu";
            // 
            // mtlOptions
            // 
            resources.ApplyResources(this.mtlOptions, "mtlOptions");
            this.mtlOptions.ActiveControl = null;
            this.mtlOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlOptions.Name = "mtlOptions";
            this.mtlOptions.Style = MetroFramework.MetroColorStyle.White;
            this.mtlOptions.Tag = "Option";
            this.mtlOptions.TileImage = global::PnT.SongClient.Properties.Resources.IconOptionWhite;
            this.mtlOptions.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlOptions.UseCustomForeColor = true;
            this.mtlOptions.UseSelectable = true;
            this.mtlOptions.UseTileImage = true;
            this.mtlOptions.Click += new System.EventHandler(this.mtlOptions_Click);
            // 
            // tlpMenu
            // 
            resources.ApplyResources(this.tlpMenu, "tlpMenu");
            this.tlpMenu.Controls.Add(this.mtlRegistrations, 0, 11);
            this.tlpMenu.Controls.Add(this.mtlClasses, 0, 10);
            this.tlpMenu.Controls.Add(this.mtlInstruments, 0, 9);
            this.tlpMenu.Controls.Add(this.mtlStudents, 0, 8);
            this.tlpMenu.Controls.Add(this.mtlTeachers, 0, 7);
            this.tlpMenu.Controls.Add(this.mtlPoles, 0, 6);
            this.tlpMenu.Controls.Add(this.mtlInstitutions, 0, 5);
            this.tlpMenu.Controls.Add(this.mtlUsers, 0, 4);
            this.tlpMenu.Controls.Add(this.mtlCalendar, 0, 3);
            this.tlpMenu.Controls.Add(this.mtlDocuments, 0, 2);
            this.tlpMenu.Controls.Add(this.mtlStatistics, 0, 1);
            this.tlpMenu.Controls.Add(this.mtlHome, 0, 0);
            this.tlpMenu.Controls.Add(this.mtlReports, 0, 12);
            this.tlpMenu.Name = "tlpMenu";
            // 
            // mtlRegistrations
            // 
            resources.ApplyResources(this.mtlRegistrations, "mtlRegistrations");
            this.mtlRegistrations.ActiveControl = null;
            this.mtlRegistrations.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlRegistrations.Name = "mtlRegistrations";
            this.mtlRegistrations.Style = MetroFramework.MetroColorStyle.White;
            this.mtlRegistrations.Tag = "Registration";
            this.mtlRegistrations.TileImage = global::PnT.SongClient.Properties.Resources.IconRegistrationWhite;
            this.mtlRegistrations.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlRegistrations.UseCustomForeColor = true;
            this.mtlRegistrations.UseSelectable = true;
            this.mtlRegistrations.UseTileImage = true;
            this.mtlRegistrations.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // mtlClasses
            // 
            resources.ApplyResources(this.mtlClasses, "mtlClasses");
            this.mtlClasses.ActiveControl = null;
            this.mtlClasses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlClasses.Name = "mtlClasses";
            this.mtlClasses.Style = MetroFramework.MetroColorStyle.White;
            this.mtlClasses.Tag = "Class";
            this.mtlClasses.TileImage = global::PnT.SongClient.Properties.Resources.IconClassWhite;
            this.mtlClasses.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlClasses.UseCustomForeColor = true;
            this.mtlClasses.UseSelectable = true;
            this.mtlClasses.UseTileImage = true;
            this.mtlClasses.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // mtlInstruments
            // 
            resources.ApplyResources(this.mtlInstruments, "mtlInstruments");
            this.mtlInstruments.ActiveControl = null;
            this.mtlInstruments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlInstruments.Name = "mtlInstruments";
            this.mtlInstruments.Style = MetroFramework.MetroColorStyle.White;
            this.mtlInstruments.Tag = "Instrument";
            this.mtlInstruments.TileImage = global::PnT.SongClient.Properties.Resources.IconInstrumentWhite;
            this.mtlInstruments.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlInstruments.UseCustomForeColor = true;
            this.mtlInstruments.UseSelectable = true;
            this.mtlInstruments.UseTileImage = true;
            this.mtlInstruments.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // mtlStudents
            // 
            resources.ApplyResources(this.mtlStudents, "mtlStudents");
            this.mtlStudents.ActiveControl = null;
            this.mtlStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlStudents.Name = "mtlStudents";
            this.mtlStudents.Style = MetroFramework.MetroColorStyle.White;
            this.mtlStudents.Tag = "Student";
            this.mtlStudents.TileImage = global::PnT.SongClient.Properties.Resources.IconStudentWhite;
            this.mtlStudents.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlStudents.UseCustomForeColor = true;
            this.mtlStudents.UseSelectable = true;
            this.mtlStudents.UseTileImage = true;
            this.mtlStudents.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // mtlTeachers
            // 
            resources.ApplyResources(this.mtlTeachers, "mtlTeachers");
            this.mtlTeachers.ActiveControl = null;
            this.mtlTeachers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlTeachers.Name = "mtlTeachers";
            this.mtlTeachers.Style = MetroFramework.MetroColorStyle.White;
            this.mtlTeachers.Tag = "Teacher";
            this.mtlTeachers.TileImage = global::PnT.SongClient.Properties.Resources.IconTeacherWhite;
            this.mtlTeachers.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlTeachers.UseCustomForeColor = true;
            this.mtlTeachers.UseSelectable = true;
            this.mtlTeachers.UseTileImage = true;
            this.mtlTeachers.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // mtlPoles
            // 
            resources.ApplyResources(this.mtlPoles, "mtlPoles");
            this.mtlPoles.ActiveControl = null;
            this.mtlPoles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlPoles.Name = "mtlPoles";
            this.mtlPoles.Style = MetroFramework.MetroColorStyle.White;
            this.mtlPoles.Tag = "Pole";
            this.mtlPoles.TileImage = global::PnT.SongClient.Properties.Resources.IconPoleWhite;
            this.mtlPoles.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlPoles.UseCustomForeColor = true;
            this.mtlPoles.UseSelectable = true;
            this.mtlPoles.UseTileImage = true;
            this.mtlPoles.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // mtlInstitutions
            // 
            resources.ApplyResources(this.mtlInstitutions, "mtlInstitutions");
            this.mtlInstitutions.ActiveControl = null;
            this.mtlInstitutions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlInstitutions.Name = "mtlInstitutions";
            this.mtlInstitutions.Style = MetroFramework.MetroColorStyle.White;
            this.mtlInstitutions.Tag = "Project";
            this.mtlInstitutions.TileImage = global::PnT.SongClient.Properties.Resources.IconProjectWhite;
            this.mtlInstitutions.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlInstitutions.UseCustomForeColor = true;
            this.mtlInstitutions.UseSelectable = true;
            this.mtlInstitutions.UseTileImage = true;
            this.mtlInstitutions.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // mtlUsers
            // 
            resources.ApplyResources(this.mtlUsers, "mtlUsers");
            this.mtlUsers.ActiveControl = null;
            this.mtlUsers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlUsers.Name = "mtlUsers";
            this.mtlUsers.Style = MetroFramework.MetroColorStyle.White;
            this.mtlUsers.Tag = "User";
            this.mtlUsers.TileImage = global::PnT.SongClient.Properties.Resources.IconUserWhite;
            this.mtlUsers.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlUsers.UseCustomForeColor = true;
            this.mtlUsers.UseSelectable = true;
            this.mtlUsers.UseTileImage = true;
            this.mtlUsers.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // mtlCalendar
            // 
            resources.ApplyResources(this.mtlCalendar, "mtlCalendar");
            this.mtlCalendar.ActiveControl = null;
            this.mtlCalendar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlCalendar.Name = "mtlCalendar";
            this.mtlCalendar.Style = MetroFramework.MetroColorStyle.White;
            this.mtlCalendar.Tag = "Calendar";
            this.mtlCalendar.TileImage = global::PnT.SongClient.Properties.Resources.IconCalendarWhite;
            this.mtlCalendar.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlCalendar.UseCustomForeColor = true;
            this.mtlCalendar.UseSelectable = true;
            this.mtlCalendar.UseTileImage = true;
            this.mtlCalendar.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // mtlDocuments
            // 
            resources.ApplyResources(this.mtlDocuments, "mtlDocuments");
            this.mtlDocuments.ActiveControl = null;
            this.mtlDocuments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlDocuments.Name = "mtlDocuments";
            this.mtlDocuments.Style = MetroFramework.MetroColorStyle.White;
            this.mtlDocuments.Tag = "Document";
            this.mtlDocuments.TileImage = global::PnT.SongClient.Properties.Resources.IconDocumentWhite;
            this.mtlDocuments.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlDocuments.UseCustomForeColor = true;
            this.mtlDocuments.UseSelectable = true;
            this.mtlDocuments.UseTileImage = true;
            this.mtlDocuments.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // mtlStatistics
            // 
            resources.ApplyResources(this.mtlStatistics, "mtlStatistics");
            this.mtlStatistics.ActiveControl = null;
            this.mtlStatistics.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlStatistics.Name = "mtlStatistics";
            this.mtlStatistics.Style = MetroFramework.MetroColorStyle.White;
            this.mtlStatistics.Tag = "Statistics";
            this.mtlStatistics.TileImage = global::PnT.SongClient.Properties.Resources.IconStatisticsWhite;
            this.mtlStatistics.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlStatistics.UseCustomForeColor = true;
            this.mtlStatistics.UseSelectable = true;
            this.mtlStatistics.UseTileImage = true;
            this.mtlStatistics.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // mtlHome
            // 
            resources.ApplyResources(this.mtlHome, "mtlHome");
            this.mtlHome.ActiveControl = null;
            this.mtlHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlHome.Name = "mtlHome";
            this.mtlHome.Style = MetroFramework.MetroColorStyle.White;
            this.mtlHome.Tag = "Dash";
            this.mtlHome.TileImage = global::PnT.SongClient.Properties.Resources.IconDashWhite;
            this.mtlHome.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlHome.UseCustomForeColor = true;
            this.mtlHome.UseSelectable = true;
            this.mtlHome.UseTileImage = true;
            this.mtlHome.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // mtlReports
            // 
            resources.ApplyResources(this.mtlReports, "mtlReports");
            this.mtlReports.ActiveControl = null;
            this.mtlReports.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlReports.Name = "mtlReports";
            this.mtlReports.Style = MetroFramework.MetroColorStyle.White;
            this.mtlReports.Tag = "Report";
            this.mtlReports.TileImage = global::PnT.SongClient.Properties.Resources.IconReportWhite;
            this.mtlReports.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlReports.UseCustomForeColor = true;
            this.mtlReports.UseSelectable = true;
            this.mtlReports.UseTileImage = true;
            this.mtlReports.Click += new System.EventHandler(this.mtlMenuOptions_Click);
            // 
            // pnContent
            // 
            resources.ApplyResources(this.pnContent, "pnContent");
            this.pnContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.tlpMain.SetColumnSpan(this.pnContent, 2);
            this.pnContent.Controls.Add(this.mpnContent);
            this.pnContent.Name = "pnContent";
            // 
            // mpnContent
            // 
            resources.ApplyResources(this.mpnContent, "mpnContent");
            this.mpnContent.HorizontalScrollbarBarColor = true;
            this.mpnContent.HorizontalScrollbarHighlightOnWheel = false;
            this.mpnContent.HorizontalScrollbarSize = 10;
            this.mpnContent.Name = "mpnContent";
            this.mpnContent.VerticalScrollbarBarColor = true;
            this.mpnContent.VerticalScrollbarHighlightOnWheel = false;
            this.mpnContent.VerticalScrollbarSize = 10;
            // 
            // mlblMessage
            // 
            resources.ApplyResources(this.mlblMessage, "mlblMessage");
            this.mlblMessage.Name = "mlblMessage";
            // 
            // mlblWebServiceStatus
            // 
            resources.ApplyResources(this.mlblWebServiceStatus, "mlblWebServiceStatus");
            this.mlblWebServiceStatus.ForeColor = System.Drawing.Color.Red;
            this.mlblWebServiceStatus.Name = "mlblWebServiceStatus";
            this.mlblWebServiceStatus.UseCustomForeColor = true;
            // 
            // mcmOptions
            // 
            resources.ApplyResources(this.mcmOptions, "mcmOptions");
            this.mcmOptions.BackColor = System.Drawing.Color.White;
            this.mcmOptions.DropShadowEnabled = false;
            this.mcmOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEditRoles,
            this.tssSeparator1,
            this.mnuGenerateReportCards,
            this.mnuImportClasses,
            this.tssSeparator2,
            this.mnuEditOptions,
            this.mnuViewLogFile,
            this.mnuHelpAbout,
            this.tssSeparator3,
            this.mnuFileDesimpersonate,
            this.mnuFileLogIn,
            this.mnuFileExit});
            this.mcmOptions.Name = "mcmOptions";
            this.mcmOptions.Style = MetroFramework.MetroColorStyle.White;
            this.mcmOptions.Opening += new System.ComponentModel.CancelEventHandler(this.mcmOptions_Opening);
            this.mcmOptions.VisibleChanged += new System.EventHandler(this.mcmOptions_VisibleChanged);
            // 
            // mnuEditRoles
            // 
            resources.ApplyResources(this.mnuEditRoles, "mnuEditRoles");
            this.mnuEditRoles.Name = "mnuEditRoles";
            this.mnuEditRoles.Click += new System.EventHandler(this.mnuEditRoles_Click);
            // 
            // tssSeparator1
            // 
            resources.ApplyResources(this.tssSeparator1, "tssSeparator1");
            this.tssSeparator1.Name = "tssSeparator1";
            // 
            // mnuGenerateReportCards
            // 
            resources.ApplyResources(this.mnuGenerateReportCards, "mnuGenerateReportCards");
            this.mnuGenerateReportCards.Name = "mnuGenerateReportCards";
            this.mnuGenerateReportCards.Click += new System.EventHandler(this.mnuGenerateReportCards_Click);
            // 
            // mnuImportClasses
            // 
            resources.ApplyResources(this.mnuImportClasses, "mnuImportClasses");
            this.mnuImportClasses.Name = "mnuImportClasses";
            this.mnuImportClasses.Click += new System.EventHandler(this.mnuImportClasses_Click);
            // 
            // tssSeparator2
            // 
            resources.ApplyResources(this.tssSeparator2, "tssSeparator2");
            this.tssSeparator2.Name = "tssSeparator2";
            // 
            // mnuEditOptions
            // 
            resources.ApplyResources(this.mnuEditOptions, "mnuEditOptions");
            this.mnuEditOptions.Name = "mnuEditOptions";
            this.mnuEditOptions.Click += new System.EventHandler(this.mnuEditOptions_Click);
            // 
            // mnuViewLogFile
            // 
            resources.ApplyResources(this.mnuViewLogFile, "mnuViewLogFile");
            this.mnuViewLogFile.Name = "mnuViewLogFile";
            this.mnuViewLogFile.Click += new System.EventHandler(this.mnuViewLogFile_Click);
            // 
            // mnuHelpAbout
            // 
            resources.ApplyResources(this.mnuHelpAbout, "mnuHelpAbout");
            this.mnuHelpAbout.Name = "mnuHelpAbout";
            this.mnuHelpAbout.Click += new System.EventHandler(this.mnuHelpAbout_Click);
            // 
            // tssSeparator3
            // 
            resources.ApplyResources(this.tssSeparator3, "tssSeparator3");
            this.tssSeparator3.Name = "tssSeparator3";
            // 
            // mnuFileDesimpersonate
            // 
            resources.ApplyResources(this.mnuFileDesimpersonate, "mnuFileDesimpersonate");
            this.mnuFileDesimpersonate.Name = "mnuFileDesimpersonate";
            this.mnuFileDesimpersonate.Click += new System.EventHandler(this.mnuFileDesimpersonate_Click);
            // 
            // mnuFileLogIn
            // 
            resources.ApplyResources(this.mnuFileLogIn, "mnuFileLogIn");
            this.mnuFileLogIn.Name = "mnuFileLogIn";
            this.mnuFileLogIn.Click += new System.EventHandler(this.mnuFileLogIn_Click);
            // 
            // mnuFileExit
            // 
            resources.ApplyResources(this.mnuFileExit, "mnuFileExit");
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // timMessage
            // 
            this.timMessage.Interval = 1000;
            this.timMessage.Tick += new System.EventHandler(this.timMessage_Tick);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackImage = global::PnT.SongClient.Properties.Resources.PNTSong;
            this.BackImagePadding = new System.Windows.Forms.Padding(6, 8, 0, 0);
            this.BackMaxSize = 30;
            this.Controls.Add(this.tlpMain);
            this.Name = "MainForm";
            this.Tag = "";
            this.TextPosition = new System.Drawing.Point(32, 7);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.pnMenu.ResumeLayout(false);
            this.tlpMenu.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.mcmOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroTile mtlHome;
        private System.Windows.Forms.Panel pnMenu;
        private System.Windows.Forms.Panel pnContent;
        private MetroFramework.Controls.MetroPanel mpnContent;
        private System.Windows.Forms.TableLayoutPanel tlpMenu;
        private MetroFramework.Controls.MetroTile mtlClasses;
        private MetroFramework.Controls.MetroTile mtlInstruments;
        private MetroFramework.Controls.MetroTile mtlStudents;
        private MetroFramework.Controls.MetroTile mtlTeachers;
        private MetroFramework.Controls.MetroTile mtlPoles;
        private MetroFramework.Controls.MetroTile mtlInstitutions;
        private MetroFramework.Controls.MetroTile mtlUsers;
        private MetroFramework.Controls.MetroTile mtlCalendar;
        private MetroFramework.Controls.MetroTile mtlDocuments;
        private MetroFramework.Controls.MetroTile mtlStatistics;
        private MetroFramework.Controls.MetroTile mtlReports;
        private MetroFramework.Controls.MetroTile mtlOptions;
        private MetroFramework.Controls.MetroContextMenu mcmOptions;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripSeparator tssSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnuEditOptions;
        private System.Windows.Forms.ToolStripMenuItem mnuViewLogFile;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuHelpAbout;
        private MetroFramework.Controls.MetroLabel mlblWebServiceStatus;
        private System.Windows.Forms.ToolStripMenuItem mnuEditRoles;
        private System.Windows.Forms.Timer timMessage;
        private MetroFramework.Controls.MetroLabel mlblMessage;
        private System.Windows.Forms.ToolStripMenuItem mnuFileLogIn;
        private MetroFramework.Controls.MetroTile mtlRegistrations;
        private System.Windows.Forms.ToolStripMenuItem mnuFileDesimpersonate;
        private System.Windows.Forms.ToolStripMenuItem mnuGenerateReportCards;
        private System.Windows.Forms.ToolStripMenuItem mnuImportClasses;
        private System.Windows.Forms.ToolStripSeparator tssSeparator2;
    }
}

