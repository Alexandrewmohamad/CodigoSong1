namespace PnT.SongClient.UI
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mtcOptions = new MetroFramework.Controls.MetroTabControl();
            this.tpGrid = new System.Windows.Forms.TabPage();
            this.pnGrid = new System.Windows.Forms.Panel();
            this.gbColumns = new System.Windows.Forms.GroupBox();
            this.mbtnResetColumns = new MetroFramework.Controls.MetroButton();
            this.mbtnIncreaseColumn = new MetroFramework.Controls.MetroButton();
            this.mbtnDecreaseColumn = new MetroFramework.Controls.MetroButton();
            this.mbtnRemoveColumn = new MetroFramework.Controls.MetroButton();
            this.mbtnAddColumn = new MetroFramework.Controls.MetroButton();
            this.mlblSelectedColumns = new MetroFramework.Controls.MetroLabel();
            this.mlblAvailableColumns = new MetroFramework.Controls.MetroLabel();
            this.lsSelectedColumns = new System.Windows.Forms.ListBox();
            this.lsAvailableColumns = new System.Windows.Forms.ListBox();
            this.mcbGrid = new MetroFramework.Controls.MetroComboBox();
            this.mcbGridFontSize = new MetroFramework.Controls.MetroComboBox();
            this.mlblGridFontSize = new MetroFramework.Controls.MetroLabel();
            this.tpServer = new System.Windows.Forms.TabPage();
            this.pnServer = new System.Windows.Forms.Panel();
            this.nudServerPort = new System.Windows.Forms.NumericUpDown();
            this.mlblServerPort = new MetroFramework.Controls.MetroLabel();
            this.mtxtServerIP = new MetroFramework.Controls.MetroTextBox();
            this.mlblServerIP = new MetroFramework.Controls.MetroLabel();
            this.tpSemester = new System.Windows.Forms.TabPage();
            this.pnSemester = new System.Windows.Forms.Panel();
            this.gbSemester = new System.Windows.Forms.GroupBox();
            this.mtxtSemesterEndDate = new System.Windows.Forms.MaskedTextBox();
            this.mtxtSemesterStartDate = new System.Windows.Forms.MaskedTextBox();
            this.mlblSemesterEndDate = new MetroFramework.Controls.MetroLabel();
            this.mlblSemesterStartDate = new MetroFramework.Controls.MetroLabel();
            this.mcbSemester = new MetroFramework.Controls.MetroComboBox();
            this.mlblSemester = new MetroFramework.Controls.MetroLabel();
            this.tpRegion = new System.Windows.Forms.TabPage();
            this.gbExamples = new System.Windows.Forms.GroupBox();
            this.mtxtDate = new MetroFramework.Controls.MetroTextBox();
            this.mtxtHour = new MetroFramework.Controls.MetroTextBox();
            this.mtxtNumber = new MetroFramework.Controls.MetroTextBox();
            this.mlblDate = new MetroFramework.Controls.MetroLabel();
            this.mlblHour = new MetroFramework.Controls.MetroLabel();
            this.mlblNumber = new MetroFramework.Controls.MetroLabel();
            this.mcbLanguages = new MetroFramework.Controls.MetroComboBox();
            this.mlblLanguage = new MetroFramework.Controls.MetroLabel();
            this.mbtnCancel = new MetroFramework.Controls.MetroButton();
            this.mbtnOK = new MetroFramework.Controls.MetroButton();
            this.tlpMain.SuspendLayout();
            this.mtcOptions.SuspendLayout();
            this.tpGrid.SuspendLayout();
            this.pnGrid.SuspendLayout();
            this.gbColumns.SuspendLayout();
            this.tpServer.SuspendLayout();
            this.pnServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudServerPort)).BeginInit();
            this.tpSemester.SuspendLayout();
            this.pnSemester.SuspendLayout();
            this.gbSemester.SuspendLayout();
            this.tpRegion.SuspendLayout();
            this.gbExamples.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mtcOptions, 0, 0);
            this.tlpMain.Controls.Add(this.mbtnCancel, 1, 1);
            this.tlpMain.Controls.Add(this.mbtnOK, 0, 1);
            this.tlpMain.Name = "tlpMain";
            // 
            // mtcOptions
            // 
            this.tlpMain.SetColumnSpan(this.mtcOptions, 2);
            this.mtcOptions.Controls.Add(this.tpGrid);
            this.mtcOptions.Controls.Add(this.tpServer);
            this.mtcOptions.Controls.Add(this.tpSemester);
            this.mtcOptions.Controls.Add(this.tpRegion);
            resources.ApplyResources(this.mtcOptions, "mtcOptions");
            this.mtcOptions.Name = "mtcOptions";
            this.mtcOptions.SelectedIndex = 2;
            this.mtcOptions.UseSelectable = true;
            // 
            // tpGrid
            // 
            this.tpGrid.BackColor = System.Drawing.Color.White;
            this.tpGrid.Controls.Add(this.pnGrid);
            resources.ApplyResources(this.tpGrid, "tpGrid");
            this.tpGrid.Name = "tpGrid";
            // 
            // pnGrid
            // 
            this.pnGrid.Controls.Add(this.gbColumns);
            this.pnGrid.Controls.Add(this.mcbGridFontSize);
            this.pnGrid.Controls.Add(this.mlblGridFontSize);
            resources.ApplyResources(this.pnGrid, "pnGrid");
            this.pnGrid.Name = "pnGrid";
            // 
            // gbColumns
            // 
            this.gbColumns.Controls.Add(this.mbtnResetColumns);
            this.gbColumns.Controls.Add(this.mbtnIncreaseColumn);
            this.gbColumns.Controls.Add(this.mbtnDecreaseColumn);
            this.gbColumns.Controls.Add(this.mbtnRemoveColumn);
            this.gbColumns.Controls.Add(this.mbtnAddColumn);
            this.gbColumns.Controls.Add(this.mlblSelectedColumns);
            this.gbColumns.Controls.Add(this.mlblAvailableColumns);
            this.gbColumns.Controls.Add(this.lsSelectedColumns);
            this.gbColumns.Controls.Add(this.lsAvailableColumns);
            this.gbColumns.Controls.Add(this.mcbGrid);
            resources.ApplyResources(this.gbColumns, "gbColumns");
            this.gbColumns.Name = "gbColumns";
            this.gbColumns.TabStop = false;
            // 
            // mbtnResetColumns
            // 
            this.mbtnResetColumns.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconTrashGreen;
            resources.ApplyResources(this.mbtnResetColumns, "mbtnResetColumns");
            this.mbtnResetColumns.Name = "mbtnResetColumns";
            this.mbtnResetColumns.UseSelectable = true;
            this.mbtnResetColumns.Click += new System.EventHandler(this.mbtnResetColumns_Click);
            // 
            // mbtnIncreaseColumn
            // 
            this.mbtnIncreaseColumn.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveDownOne;
            resources.ApplyResources(this.mbtnIncreaseColumn, "mbtnIncreaseColumn");
            this.mbtnIncreaseColumn.Name = "mbtnIncreaseColumn";
            this.mbtnIncreaseColumn.UseSelectable = true;
            this.mbtnIncreaseColumn.Click += new System.EventHandler(this.mbtnIncreaseColumn_Click);
            // 
            // mbtnDecreaseColumn
            // 
            this.mbtnDecreaseColumn.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveUpOne;
            resources.ApplyResources(this.mbtnDecreaseColumn, "mbtnDecreaseColumn");
            this.mbtnDecreaseColumn.Name = "mbtnDecreaseColumn";
            this.mbtnDecreaseColumn.UseSelectable = true;
            this.mbtnDecreaseColumn.Click += new System.EventHandler(this.mbtnDecreaseColumn_Click);
            // 
            // mbtnRemoveColumn
            // 
            this.mbtnRemoveColumn.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveLeftOne;
            resources.ApplyResources(this.mbtnRemoveColumn, "mbtnRemoveColumn");
            this.mbtnRemoveColumn.Name = "mbtnRemoveColumn";
            this.mbtnRemoveColumn.UseSelectable = true;
            this.mbtnRemoveColumn.Click += new System.EventHandler(this.mbtnRemoveColumn_Click);
            // 
            // mbtnAddColumn
            // 
            this.mbtnAddColumn.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveRightOne;
            resources.ApplyResources(this.mbtnAddColumn, "mbtnAddColumn");
            this.mbtnAddColumn.Name = "mbtnAddColumn";
            this.mbtnAddColumn.UseSelectable = true;
            this.mbtnAddColumn.Click += new System.EventHandler(this.mbtnAddColumn_Click);
            // 
            // mlblSelectedColumns
            // 
            this.mlblSelectedColumns.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblSelectedColumns, "mlblSelectedColumns");
            this.mlblSelectedColumns.Name = "mlblSelectedColumns";
            // 
            // mlblAvailableColumns
            // 
            this.mlblAvailableColumns.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblAvailableColumns, "mlblAvailableColumns");
            this.mlblAvailableColumns.Name = "mlblAvailableColumns";
            // 
            // lsSelectedColumns
            // 
            this.lsSelectedColumns.FormattingEnabled = true;
            resources.ApplyResources(this.lsSelectedColumns, "lsSelectedColumns");
            this.lsSelectedColumns.Name = "lsSelectedColumns";
            this.lsSelectedColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lsSelectedColumns.SelectedIndexChanged += new System.EventHandler(this.lsSelectedColumns_SelectedIndexChanged);
            // 
            // lsAvailableColumns
            // 
            this.lsAvailableColumns.FormattingEnabled = true;
            resources.ApplyResources(this.lsAvailableColumns, "lsAvailableColumns");
            this.lsAvailableColumns.Name = "lsAvailableColumns";
            this.lsAvailableColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lsAvailableColumns.SelectedIndexChanged += new System.EventHandler(this.lsAvailableColumns_SelectedIndexChanged);
            // 
            // mcbGrid
            // 
            this.mcbGrid.FormattingEnabled = true;
            resources.ApplyResources(this.mcbGrid, "mcbGrid");
            this.mcbGrid.Name = "mcbGrid";
            this.mcbGrid.UseSelectable = true;
            this.mcbGrid.SelectedIndexChanged += new System.EventHandler(this.mcbGrid_SelectedIndexChanged);
            // 
            // mcbGridFontSize
            // 
            this.mcbGridFontSize.FormattingEnabled = true;
            resources.ApplyResources(this.mcbGridFontSize, "mcbGridFontSize");
            this.mcbGridFontSize.Name = "mcbGridFontSize";
            this.mcbGridFontSize.UseSelectable = true;
            // 
            // mlblGridFontSize
            // 
            resources.ApplyResources(this.mlblGridFontSize, "mlblGridFontSize");
            this.mlblGridFontSize.Name = "mlblGridFontSize";
            // 
            // tpServer
            // 
            this.tpServer.BackColor = System.Drawing.Color.White;
            this.tpServer.Controls.Add(this.pnServer);
            resources.ApplyResources(this.tpServer, "tpServer");
            this.tpServer.Name = "tpServer";
            // 
            // pnServer
            // 
            this.pnServer.Controls.Add(this.nudServerPort);
            this.pnServer.Controls.Add(this.mlblServerPort);
            this.pnServer.Controls.Add(this.mtxtServerIP);
            this.pnServer.Controls.Add(this.mlblServerIP);
            resources.ApplyResources(this.pnServer, "pnServer");
            this.pnServer.Name = "pnServer";
            // 
            // nudServerPort
            // 
            resources.ApplyResources(this.nudServerPort, "nudServerPort");
            this.nudServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudServerPort.Name = "nudServerPort";
            this.nudServerPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // mlblServerPort
            // 
            resources.ApplyResources(this.mlblServerPort, "mlblServerPort");
            this.mlblServerPort.Name = "mlblServerPort";
            // 
            // mtxtServerIP
            // 
            // 
            // 
            // 
            this.mtxtServerIP.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.mtxtServerIP.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode")));
            this.mtxtServerIP.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location")));
            this.mtxtServerIP.CustomButton.Name = "";
            this.mtxtServerIP.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size")));
            this.mtxtServerIP.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtServerIP.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex")));
            this.mtxtServerIP.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtServerIP.CustomButton.UseSelectable = true;
            this.mtxtServerIP.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible")));
            this.mtxtServerIP.Lines = new string[0];
            resources.ApplyResources(this.mtxtServerIP, "mtxtServerIP");
            this.mtxtServerIP.MaxLength = 32767;
            this.mtxtServerIP.Name = "mtxtServerIP";
            this.mtxtServerIP.PasswordChar = '\0';
            this.mtxtServerIP.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtServerIP.SelectedText = "";
            this.mtxtServerIP.SelectionLength = 0;
            this.mtxtServerIP.SelectionStart = 0;
            this.mtxtServerIP.ShortcutsEnabled = true;
            this.mtxtServerIP.UseSelectable = true;
            this.mtxtServerIP.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtServerIP.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblServerIP
            // 
            resources.ApplyResources(this.mlblServerIP, "mlblServerIP");
            this.mlblServerIP.Name = "mlblServerIP";
            // 
            // tpSemester
            // 
            this.tpSemester.BackColor = System.Drawing.Color.White;
            this.tpSemester.Controls.Add(this.pnSemester);
            resources.ApplyResources(this.tpSemester, "tpSemester");
            this.tpSemester.Name = "tpSemester";
            // 
            // pnSemester
            // 
            this.pnSemester.Controls.Add(this.gbSemester);
            this.pnSemester.Controls.Add(this.mcbSemester);
            this.pnSemester.Controls.Add(this.mlblSemester);
            resources.ApplyResources(this.pnSemester, "pnSemester");
            this.pnSemester.Name = "pnSemester";
            // 
            // gbSemester
            // 
            this.gbSemester.Controls.Add(this.mtxtSemesterEndDate);
            this.gbSemester.Controls.Add(this.mtxtSemesterStartDate);
            this.gbSemester.Controls.Add(this.mlblSemesterEndDate);
            this.gbSemester.Controls.Add(this.mlblSemesterStartDate);
            resources.ApplyResources(this.gbSemester, "gbSemester");
            this.gbSemester.Name = "gbSemester";
            this.gbSemester.TabStop = false;
            // 
            // mtxtSemesterEndDate
            // 
            this.mtxtSemesterEndDate.BackColor = System.Drawing.Color.White;
            this.mtxtSemesterEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.mtxtSemesterEndDate, "mtxtSemesterEndDate");
            this.mtxtSemesterEndDate.Name = "mtxtSemesterEndDate";
            this.mtxtSemesterEndDate.ValidatingType = typeof(System.DateTime);
            this.mtxtSemesterEndDate.Click += new System.EventHandler(this.Date_Click);
            this.mtxtSemesterEndDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mtxtSemesterEndDate_KeyUp);
            // 
            // mtxtSemesterStartDate
            // 
            this.mtxtSemesterStartDate.BackColor = System.Drawing.Color.White;
            this.mtxtSemesterStartDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.mtxtSemesterStartDate, "mtxtSemesterStartDate");
            this.mtxtSemesterStartDate.Name = "mtxtSemesterStartDate";
            this.mtxtSemesterStartDate.ValidatingType = typeof(System.DateTime);
            this.mtxtSemesterStartDate.Click += new System.EventHandler(this.Date_Click);
            this.mtxtSemesterStartDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mtxtSemesterStartDate_KeyUp);
            // 
            // mlblSemesterEndDate
            // 
            resources.ApplyResources(this.mlblSemesterEndDate, "mlblSemesterEndDate");
            this.mlblSemesterEndDate.Name = "mlblSemesterEndDate";
            // 
            // mlblSemesterStartDate
            // 
            resources.ApplyResources(this.mlblSemesterStartDate, "mlblSemesterStartDate");
            this.mlblSemesterStartDate.Name = "mlblSemesterStartDate";
            // 
            // mcbSemester
            // 
            this.mcbSemester.FormattingEnabled = true;
            resources.ApplyResources(this.mcbSemester, "mcbSemester");
            this.mcbSemester.Name = "mcbSemester";
            this.mcbSemester.UseSelectable = true;
            this.mcbSemester.SelectedIndexChanged += new System.EventHandler(this.mcbSemester_SelectedIndexChanged);
            // 
            // mlblSemester
            // 
            resources.ApplyResources(this.mlblSemester, "mlblSemester");
            this.mlblSemester.Name = "mlblSemester";
            // 
            // tpRegion
            // 
            this.tpRegion.BackColor = System.Drawing.Color.White;
            this.tpRegion.Controls.Add(this.gbExamples);
            this.tpRegion.Controls.Add(this.mcbLanguages);
            this.tpRegion.Controls.Add(this.mlblLanguage);
            resources.ApplyResources(this.tpRegion, "tpRegion");
            this.tpRegion.Name = "tpRegion";
            // 
            // gbExamples
            // 
            this.gbExamples.Controls.Add(this.mtxtDate);
            this.gbExamples.Controls.Add(this.mtxtHour);
            this.gbExamples.Controls.Add(this.mtxtNumber);
            this.gbExamples.Controls.Add(this.mlblDate);
            this.gbExamples.Controls.Add(this.mlblHour);
            this.gbExamples.Controls.Add(this.mlblNumber);
            resources.ApplyResources(this.gbExamples, "gbExamples");
            this.gbExamples.Name = "gbExamples";
            this.gbExamples.TabStop = false;
            // 
            // mtxtDate
            // 
            // 
            // 
            // 
            this.mtxtDate.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            this.mtxtDate.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode1")));
            this.mtxtDate.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location1")));
            this.mtxtDate.CustomButton.Name = "";
            this.mtxtDate.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size1")));
            this.mtxtDate.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtDate.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex1")));
            this.mtxtDate.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtDate.CustomButton.UseSelectable = true;
            this.mtxtDate.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible1")));
            this.mtxtDate.Lines = new string[0];
            resources.ApplyResources(this.mtxtDate, "mtxtDate");
            this.mtxtDate.MaxLength = 32767;
            this.mtxtDate.Name = "mtxtDate";
            this.mtxtDate.PasswordChar = '\0';
            this.mtxtDate.ReadOnly = true;
            this.mtxtDate.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtDate.SelectedText = "";
            this.mtxtDate.SelectionLength = 0;
            this.mtxtDate.SelectionStart = 0;
            this.mtxtDate.ShortcutsEnabled = true;
            this.mtxtDate.UseSelectable = true;
            this.mtxtDate.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtDate.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mtxtHour
            // 
            // 
            // 
            // 
            this.mtxtHour.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            this.mtxtHour.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode2")));
            this.mtxtHour.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location2")));
            this.mtxtHour.CustomButton.Name = "";
            this.mtxtHour.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size2")));
            this.mtxtHour.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtHour.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex2")));
            this.mtxtHour.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtHour.CustomButton.UseSelectable = true;
            this.mtxtHour.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible2")));
            this.mtxtHour.Lines = new string[0];
            resources.ApplyResources(this.mtxtHour, "mtxtHour");
            this.mtxtHour.MaxLength = 32767;
            this.mtxtHour.Name = "mtxtHour";
            this.mtxtHour.PasswordChar = '\0';
            this.mtxtHour.ReadOnly = true;
            this.mtxtHour.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtHour.SelectedText = "";
            this.mtxtHour.SelectionLength = 0;
            this.mtxtHour.SelectionStart = 0;
            this.mtxtHour.ShortcutsEnabled = true;
            this.mtxtHour.UseSelectable = true;
            this.mtxtHour.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtHour.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mtxtNumber
            // 
            // 
            // 
            // 
            this.mtxtNumber.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image3")));
            this.mtxtNumber.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode3")));
            this.mtxtNumber.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location3")));
            this.mtxtNumber.CustomButton.Name = "";
            this.mtxtNumber.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size3")));
            this.mtxtNumber.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtNumber.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex3")));
            this.mtxtNumber.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtNumber.CustomButton.UseSelectable = true;
            this.mtxtNumber.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible3")));
            this.mtxtNumber.Lines = new string[0];
            resources.ApplyResources(this.mtxtNumber, "mtxtNumber");
            this.mtxtNumber.MaxLength = 32767;
            this.mtxtNumber.Name = "mtxtNumber";
            this.mtxtNumber.PasswordChar = '\0';
            this.mtxtNumber.ReadOnly = true;
            this.mtxtNumber.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtNumber.SelectedText = "";
            this.mtxtNumber.SelectionLength = 0;
            this.mtxtNumber.SelectionStart = 0;
            this.mtxtNumber.ShortcutsEnabled = true;
            this.mtxtNumber.UseSelectable = true;
            this.mtxtNumber.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtNumber.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblDate
            // 
            resources.ApplyResources(this.mlblDate, "mlblDate");
            this.mlblDate.Name = "mlblDate";
            // 
            // mlblHour
            // 
            resources.ApplyResources(this.mlblHour, "mlblHour");
            this.mlblHour.Name = "mlblHour";
            // 
            // mlblNumber
            // 
            resources.ApplyResources(this.mlblNumber, "mlblNumber");
            this.mlblNumber.Name = "mlblNumber";
            // 
            // mcbLanguages
            // 
            this.mcbLanguages.FormattingEnabled = true;
            resources.ApplyResources(this.mcbLanguages, "mcbLanguages");
            this.mcbLanguages.Name = "mcbLanguages";
            this.mcbLanguages.UseSelectable = true;
            this.mcbLanguages.SelectedIndexChanged += new System.EventHandler(this.mcbLanguages_SelectedIndexChanged);
            // 
            // mlblLanguage
            // 
            resources.ApplyResources(this.mlblLanguage, "mlblLanguage");
            this.mlblLanguage.Name = "mlblLanguage";
            // 
            // mbtnCancel
            // 
            resources.ApplyResources(this.mbtnCancel, "mbtnCancel");
            this.mbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.mbtnCancel.Name = "mbtnCancel";
            this.mbtnCancel.UseSelectable = true;
            // 
            // mbtnOK
            // 
            resources.ApplyResources(this.mbtnOK, "mbtnOK");
            this.mbtnOK.Name = "mbtnOK";
            this.mbtnOK.UseSelectable = true;
            this.mbtnOK.Click += new System.EventHandler(this.mbtnOK_Click);
            // 
            // OptionsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.Controls.Add(this.tlpMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Resizable = false;
            this.TextPosition = new System.Drawing.Point(15, 5);
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.tlpMain.ResumeLayout(false);
            this.mtcOptions.ResumeLayout(false);
            this.tpGrid.ResumeLayout(false);
            this.pnGrid.ResumeLayout(false);
            this.gbColumns.ResumeLayout(false);
            this.tpServer.ResumeLayout(false);
            this.pnServer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudServerPort)).EndInit();
            this.tpSemester.ResumeLayout(false);
            this.pnSemester.ResumeLayout(false);
            this.gbSemester.ResumeLayout(false);
            this.gbSemester.PerformLayout();
            this.tpRegion.ResumeLayout(false);
            this.gbExamples.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroTabControl mtcOptions;
        private MetroFramework.Controls.MetroButton mbtnCancel;
        private MetroFramework.Controls.MetroButton mbtnOK;
        private System.Windows.Forms.TabPage tpServer;
        private System.Windows.Forms.TabPage tpRegion;
        private System.Windows.Forms.Panel pnServer;
        private MetroFramework.Controls.MetroTextBox mtxtServerIP;
        private MetroFramework.Controls.MetroLabel mlblServerIP;
        private System.Windows.Forms.NumericUpDown nudServerPort;
        private MetroFramework.Controls.MetroLabel mlblServerPort;
        private MetroFramework.Controls.MetroLabel mlblLanguage;
        private MetroFramework.Controls.MetroComboBox mcbLanguages;
        private System.Windows.Forms.GroupBox gbExamples;
        private MetroFramework.Controls.MetroTextBox mtxtDate;
        private MetroFramework.Controls.MetroTextBox mtxtHour;
        private MetroFramework.Controls.MetroTextBox mtxtNumber;
        private MetroFramework.Controls.MetroLabel mlblDate;
        private MetroFramework.Controls.MetroLabel mlblHour;
        private MetroFramework.Controls.MetroLabel mlblNumber;
        private System.Windows.Forms.TabPage tpGrid;
        private System.Windows.Forms.Panel pnGrid;
        private MetroFramework.Controls.MetroComboBox mcbGridFontSize;
        private MetroFramework.Controls.MetroLabel mlblGridFontSize;
        private System.Windows.Forms.GroupBox gbColumns;
        private MetroFramework.Controls.MetroLabel mlblSelectedColumns;
        private MetroFramework.Controls.MetroLabel mlblAvailableColumns;
        private System.Windows.Forms.ListBox lsSelectedColumns;
        private System.Windows.Forms.ListBox lsAvailableColumns;
        private MetroFramework.Controls.MetroComboBox mcbGrid;
        private MetroFramework.Controls.MetroButton mbtnAddColumn;
        private MetroFramework.Controls.MetroButton mbtnResetColumns;
        private MetroFramework.Controls.MetroButton mbtnIncreaseColumn;
        private MetroFramework.Controls.MetroButton mbtnDecreaseColumn;
        private MetroFramework.Controls.MetroButton mbtnRemoveColumn;
        private System.Windows.Forms.TabPage tpSemester;
        private System.Windows.Forms.Panel pnSemester;
        private System.Windows.Forms.GroupBox gbSemester;
        private MetroFramework.Controls.MetroLabel mlblSemesterEndDate;
        private MetroFramework.Controls.MetroLabel mlblSemesterStartDate;
        private MetroFramework.Controls.MetroComboBox mcbSemester;
        private MetroFramework.Controls.MetroLabel mlblSemester;
        private System.Windows.Forms.MaskedTextBox mtxtSemesterEndDate;
        private System.Windows.Forms.MaskedTextBox mtxtSemesterStartDate;
    }
}