namespace PnT.SongClient.UI.Controls
{
    partial class ViewReportControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewReportControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mtlView = new MetroFramework.Controls.MetroTile();
            this.mlblItemHeading = new MetroFramework.Controls.MetroLabel();
            this.dgvReports = new System.Windows.Forms.DataGridView();
            this.tlpFilters = new System.Windows.Forms.TableLayoutPanel();
            this.mlblSemester = new MetroFramework.Controls.MetroLabel();
            this.mcbStatus = new MetroFramework.Controls.MetroComboBox();
            this.mlblStatus = new MetroFramework.Controls.MetroLabel();
            this.mlblReportType = new MetroFramework.Controls.MetroLabel();
            this.mcbReportType = new MetroFramework.Controls.MetroComboBox();
            this.mlblInstitution = new MetroFramework.Controls.MetroLabel();
            this.mcbInstitution = new MetroFramework.Controls.MetroComboBox();
            this.mcbSemester = new MetroFramework.Controls.MetroComboBox();
            this.mlblPeriodicity = new MetroFramework.Controls.MetroLabel();
            this.mcbPeriodicity = new MetroFramework.Controls.MetroComboBox();
            this.mlblTeacher = new MetroFramework.Controls.MetroLabel();
            this.mcbTeacher = new MetroFramework.Controls.MetroComboBox();
            this.mcbClass = new MetroFramework.Controls.MetroComboBox();
            this.mlblClass = new MetroFramework.Controls.MetroLabel();
            this.tlpFooter = new System.Windows.Forms.TableLayoutPanel();
            this.mlblItemCount = new MetroFramework.Controls.MetroLabel();
            this.mlblCompletedCount = new MetroFramework.Controls.MetroLabel();
            this.mlblPendingCount = new MetroFramework.Controls.MetroLabel();
            this.mcmReport = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.mnuViewReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewInstitution = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewTeacher = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuImpersonateCoordinator = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImpersonateTeacher = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparatorImpersonate = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDisplayColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.ReportId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SemesterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PeriodName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AuthorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstitutionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClassData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReportStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).BeginInit();
            this.tlpFilters.SuspendLayout();
            this.tlpFooter.SuspendLayout();
            this.mcmReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mtlView, 1, 0);
            this.tlpMain.Controls.Add(this.mlblItemHeading, 0, 0);
            this.tlpMain.Controls.Add(this.dgvReports, 0, 2);
            this.tlpMain.Controls.Add(this.tlpFilters, 0, 1);
            this.tlpMain.Controls.Add(this.tlpFooter, 0, 3);
            this.tlpMain.Name = "tlpMain";
            // 
            // mtlView
            // 
            this.mtlView.ActiveControl = null;
            this.mtlView.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.mtlView, "mtlView");
            this.mtlView.Name = "mtlView";
            this.mtlView.Style = MetroFramework.MetroColorStyle.White;
            this.mtlView.Tag = "";
            this.mtlView.TileImage = global::PnT.SongClient.Properties.Resources.IconEditWhite;
            this.mtlView.TileImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mtlView.UseCustomForeColor = true;
            this.mtlView.UseSelectable = true;
            this.mtlView.UseTileImage = true;
            this.mtlView.Click += new System.EventHandler(this.mtlEdit_Click);
            // 
            // mlblItemHeading
            // 
            resources.ApplyResources(this.mlblItemHeading, "mlblItemHeading");
            this.mlblItemHeading.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.mlblItemHeading.Name = "mlblItemHeading";
            // 
            // dgvReports
            // 
            this.dgvReports.AllowUserToAddRows = false;
            this.dgvReports.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.dgvReports.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReports.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgvReports.BackgroundColor = System.Drawing.Color.White;
            this.dgvReports.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvReports.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvReports.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvReports.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReports.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReports.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReportId,
            this.SemesterName,
            this.PeriodName,
            this.TypeName,
            this.AuthorName,
            this.InstitutionName,
            this.ClassData,
            this.ReportStatus});
            this.tlpMain.SetColumnSpan(this.dgvReports, 2);
            this.dgvReports.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReports.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.dgvReports, "dgvReports");
            this.dgvReports.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvReports.Name = "dgvReports";
            this.dgvReports.ReadOnly = true;
            this.dgvReports.RowHeadersVisible = false;
            this.dgvReports.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvReports.RowTemplate.Height = 44;
            this.dgvReports.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvReports_MouseUp);
            // 
            // tlpFilters
            // 
            resources.ApplyResources(this.tlpFilters, "tlpFilters");
            this.tlpMain.SetColumnSpan(this.tlpFilters, 2);
            this.tlpFilters.Controls.Add(this.mlblSemester, 0, 1);
            this.tlpFilters.Controls.Add(this.mcbStatus, 1, 0);
            this.tlpFilters.Controls.Add(this.mlblStatus, 0, 0);
            this.tlpFilters.Controls.Add(this.mlblReportType, 3, 0);
            this.tlpFilters.Controls.Add(this.mcbReportType, 4, 0);
            this.tlpFilters.Controls.Add(this.mlblInstitution, 6, 0);
            this.tlpFilters.Controls.Add(this.mcbInstitution, 7, 0);
            this.tlpFilters.Controls.Add(this.mcbSemester, 1, 1);
            this.tlpFilters.Controls.Add(this.mlblPeriodicity, 3, 1);
            this.tlpFilters.Controls.Add(this.mcbPeriodicity, 4, 1);
            this.tlpFilters.Controls.Add(this.mlblTeacher, 6, 1);
            this.tlpFilters.Controls.Add(this.mcbTeacher, 7, 1);
            this.tlpFilters.Controls.Add(this.mcbClass, 10, 1);
            this.tlpFilters.Controls.Add(this.mlblClass, 9, 1);
            this.tlpFilters.Name = "tlpFilters";
            // 
            // mlblSemester
            // 
            resources.ApplyResources(this.mlblSemester, "mlblSemester");
            this.mlblSemester.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblSemester.Name = "mlblSemester";
            // 
            // mcbStatus
            // 
            resources.ApplyResources(this.mcbStatus, "mcbStatus");
            this.mcbStatus.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbStatus.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbStatus.FormattingEnabled = true;
            this.mcbStatus.Name = "mcbStatus";
            this.mcbStatus.UseSelectable = true;
            this.mcbStatus.SelectedIndexChanged += new System.EventHandler(this.mcbStatus_SelectedIndexChanged);
            // 
            // mlblStatus
            // 
            resources.ApplyResources(this.mlblStatus, "mlblStatus");
            this.mlblStatus.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblStatus.Name = "mlblStatus";
            // 
            // mlblReportType
            // 
            resources.ApplyResources(this.mlblReportType, "mlblReportType");
            this.mlblReportType.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblReportType.Name = "mlblReportType";
            // 
            // mcbReportType
            // 
            resources.ApplyResources(this.mcbReportType, "mcbReportType");
            this.mcbReportType.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbReportType.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbReportType.FormattingEnabled = true;
            this.mcbReportType.Name = "mcbReportType";
            this.mcbReportType.UseSelectable = true;
            this.mcbReportType.SelectedIndexChanged += new System.EventHandler(this.mcbReportType_SelectedIndexChanged);
            // 
            // mlblInstitution
            // 
            resources.ApplyResources(this.mlblInstitution, "mlblInstitution");
            this.mlblInstitution.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblInstitution.Name = "mlblInstitution";
            // 
            // mcbInstitution
            // 
            resources.ApplyResources(this.mcbInstitution, "mcbInstitution");
            this.mcbInstitution.DropDownWidth = 240;
            this.mcbInstitution.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbInstitution.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbInstitution.FormattingEnabled = true;
            this.mcbInstitution.Name = "mcbInstitution";
            this.mcbInstitution.UseSelectable = true;
            this.mcbInstitution.SelectedIndexChanged += new System.EventHandler(this.mcbInstitution_SelectedIndexChanged);
            // 
            // mcbSemester
            // 
            resources.ApplyResources(this.mcbSemester, "mcbSemester");
            this.mcbSemester.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbSemester.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbSemester.FormattingEnabled = true;
            this.mcbSemester.Name = "mcbSemester";
            this.mcbSemester.UseSelectable = true;
            this.mcbSemester.SelectedIndexChanged += new System.EventHandler(this.mcbSemester_SelectedIndexChanged);
            // 
            // mlblPeriodicity
            // 
            resources.ApplyResources(this.mlblPeriodicity, "mlblPeriodicity");
            this.mlblPeriodicity.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblPeriodicity.Name = "mlblPeriodicity";
            // 
            // mcbPeriodicity
            // 
            resources.ApplyResources(this.mcbPeriodicity, "mcbPeriodicity");
            this.mcbPeriodicity.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbPeriodicity.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbPeriodicity.FormattingEnabled = true;
            this.mcbPeriodicity.Name = "mcbPeriodicity";
            this.mcbPeriodicity.UseSelectable = true;
            this.mcbPeriodicity.SelectedIndexChanged += new System.EventHandler(this.mcbReportPeriodicity_SelectedIndexChanged);
            // 
            // mlblTeacher
            // 
            resources.ApplyResources(this.mlblTeacher, "mlblTeacher");
            this.mlblTeacher.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblTeacher.Name = "mlblTeacher";
            // 
            // mcbTeacher
            // 
            resources.ApplyResources(this.mcbTeacher, "mcbTeacher");
            this.mcbTeacher.DropDownWidth = 240;
            this.mcbTeacher.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbTeacher.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbTeacher.FormattingEnabled = true;
            this.mcbTeacher.Name = "mcbTeacher";
            this.mcbTeacher.UseSelectable = true;
            this.mcbTeacher.SelectedIndexChanged += new System.EventHandler(this.mcbTeacher_SelectedIndexChanged);
            // 
            // mcbClass
            // 
            resources.ApplyResources(this.mcbClass, "mcbClass");
            this.mcbClass.DropDownWidth = 240;
            this.mcbClass.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbClass.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbClass.FormattingEnabled = true;
            this.mcbClass.Name = "mcbClass";
            this.mcbClass.UseSelectable = true;
            this.mcbClass.SelectedIndexChanged += new System.EventHandler(this.mcbClass_SelectedIndexChanged);
            // 
            // mlblClass
            // 
            resources.ApplyResources(this.mlblClass, "mlblClass");
            this.mlblClass.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblClass.Name = "mlblClass";
            // 
            // tlpFooter
            // 
            resources.ApplyResources(this.tlpFooter, "tlpFooter");
            this.tlpMain.SetColumnSpan(this.tlpFooter, 2);
            this.tlpFooter.Controls.Add(this.mlblItemCount, 0, 0);
            this.tlpFooter.Controls.Add(this.mlblCompletedCount, 1, 0);
            this.tlpFooter.Controls.Add(this.mlblPendingCount, 2, 0);
            this.tlpFooter.Name = "tlpFooter";
            // 
            // mlblItemCount
            // 
            resources.ApplyResources(this.mlblItemCount, "mlblItemCount");
            this.mlblItemCount.Name = "mlblItemCount";
            // 
            // mlblCompletedCount
            // 
            resources.ApplyResources(this.mlblCompletedCount, "mlblCompletedCount");
            this.mlblCompletedCount.Name = "mlblCompletedCount";
            // 
            // mlblPendingCount
            // 
            resources.ApplyResources(this.mlblPendingCount, "mlblPendingCount");
            this.mlblPendingCount.Name = "mlblPendingCount";
            // 
            // mcmReport
            // 
            this.mcmReport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewReport,
            this.mnuViewInstitution,
            this.mnuViewTeacher,
            this.tssSeparator,
            this.mnuImpersonateCoordinator,
            this.mnuImpersonateTeacher,
            this.tssSeparatorImpersonate,
            this.mnuCopy,
            this.tssSeparator2,
            this.mnuDisplayColumns});
            this.mcmReport.Name = "mcmReport";
            resources.ApplyResources(this.mcmReport, "mcmReport");
            // 
            // mnuViewReport
            // 
            this.mnuViewReport.Name = "mnuViewReport";
            resources.ApplyResources(this.mnuViewReport, "mnuViewReport");
            this.mnuViewReport.Click += new System.EventHandler(this.mnuViewReport_Click);
            // 
            // mnuViewInstitution
            // 
            this.mnuViewInstitution.Name = "mnuViewInstitution";
            resources.ApplyResources(this.mnuViewInstitution, "mnuViewInstitution");
            this.mnuViewInstitution.Click += new System.EventHandler(this.mnuViewInstitution_Click);
            // 
            // mnuViewTeacher
            // 
            this.mnuViewTeacher.Name = "mnuViewTeacher";
            resources.ApplyResources(this.mnuViewTeacher, "mnuViewTeacher");
            this.mnuViewTeacher.Click += new System.EventHandler(this.mnuViewTeacher_Click);
            // 
            // tssSeparator
            // 
            this.tssSeparator.Name = "tssSeparator";
            resources.ApplyResources(this.tssSeparator, "tssSeparator");
            // 
            // mnuImpersonateCoordinator
            // 
            this.mnuImpersonateCoordinator.Name = "mnuImpersonateCoordinator";
            resources.ApplyResources(this.mnuImpersonateCoordinator, "mnuImpersonateCoordinator");
            this.mnuImpersonateCoordinator.Click += new System.EventHandler(this.mnuImpersonateCoordinator_Click);
            // 
            // mnuImpersonateTeacher
            // 
            this.mnuImpersonateTeacher.Name = "mnuImpersonateTeacher";
            resources.ApplyResources(this.mnuImpersonateTeacher, "mnuImpersonateTeacher");
            this.mnuImpersonateTeacher.Click += new System.EventHandler(this.mnuImpersonateTeacher_Click);
            // 
            // tssSeparatorImpersonate
            // 
            this.tssSeparatorImpersonate.Name = "tssSeparatorImpersonate";
            resources.ApplyResources(this.tssSeparatorImpersonate, "tssSeparatorImpersonate");
            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            resources.ApplyResources(this.mnuCopy, "mnuCopy");
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // tssSeparator2
            // 
            this.tssSeparator2.Name = "tssSeparator2";
            resources.ApplyResources(this.tssSeparator2, "tssSeparator2");
            // 
            // mnuDisplayColumns
            // 
            this.mnuDisplayColumns.Name = "mnuDisplayColumns";
            resources.ApplyResources(this.mnuDisplayColumns, "mnuDisplayColumns");
            this.mnuDisplayColumns.Click += new System.EventHandler(this.mnuDisplayColumns_Click);
            // 
            // ReportId
            // 
            this.ReportId.DataPropertyName = "ReportId";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.ReportId.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.ReportId, "ReportId");
            this.ReportId.Name = "ReportId";
            this.ReportId.ReadOnly = true;
            // 
            // SemesterName
            // 
            this.SemesterName.DataPropertyName = "SemesterName";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.SemesterName.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.SemesterName, "SemesterName");
            this.SemesterName.Name = "SemesterName";
            this.SemesterName.ReadOnly = true;
            // 
            // PeriodName
            // 
            this.PeriodName.DataPropertyName = "PeriodName";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PeriodName.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.PeriodName, "PeriodName");
            this.PeriodName.Name = "PeriodName";
            this.PeriodName.ReadOnly = true;
            // 
            // TypeName
            // 
            this.TypeName.DataPropertyName = "TypeName";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.TypeName.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.TypeName, "TypeName");
            this.TypeName.Name = "TypeName";
            this.TypeName.ReadOnly = true;
            // 
            // AuthorName
            // 
            this.AuthorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AuthorName.DataPropertyName = "AuthorName";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AuthorName.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.AuthorName, "AuthorName");
            this.AuthorName.Name = "AuthorName";
            this.AuthorName.ReadOnly = true;
            // 
            // InstitutionName
            // 
            this.InstitutionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InstitutionName.DataPropertyName = "InstitutionName";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InstitutionName.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.InstitutionName, "InstitutionName");
            this.InstitutionName.Name = "InstitutionName";
            this.InstitutionName.ReadOnly = true;
            // 
            // ClassData
            // 
            this.ClassData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClassData.DataPropertyName = "ClassData";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ClassData.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.ClassData, "ClassData");
            this.ClassData.Name = "ClassData";
            this.ClassData.ReadOnly = true;
            // 
            // ReportStatus
            // 
            this.ReportStatus.DataPropertyName = "ReportStatusName";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.ReportStatus.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.ReportStatus, "ReportStatus");
            this.ReportStatus.Name = "ReportStatus";
            this.ReportStatus.ReadOnly = true;
            // 
            // ViewReportControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "ViewReportControl";
            this.Load += new System.EventHandler(this.ViewReportControl_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).EndInit();
            this.tlpFilters.ResumeLayout(false);
            this.tlpFilters.PerformLayout();
            this.tlpFooter.ResumeLayout(false);
            this.tlpFooter.PerformLayout();
            this.mcmReport.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroTile mtlView;
        private MetroFramework.Controls.MetroLabel mlblItemHeading;
        private System.Windows.Forms.DataGridView dgvReports;
        private System.Windows.Forms.TableLayoutPanel tlpFilters;
        private MetroFramework.Controls.MetroLabel mlblSemester;
        private MetroFramework.Controls.MetroComboBox mcbStatus;
        private MetroFramework.Controls.MetroLabel mlblStatus;
        private MetroFramework.Controls.MetroLabel mlblReportType;
        private MetroFramework.Controls.MetroComboBox mcbReportType;
        private MetroFramework.Controls.MetroLabel mlblInstitution;
        private MetroFramework.Controls.MetroComboBox mcbInstitution;
        private MetroFramework.Controls.MetroComboBox mcbSemester;
        private MetroFramework.Controls.MetroLabel mlblPeriodicity;
        private MetroFramework.Controls.MetroComboBox mcbPeriodicity;
        private MetroFramework.Controls.MetroLabel mlblClass;
        private MetroFramework.Controls.MetroComboBox mcbClass;
        private MetroFramework.Controls.MetroLabel mlblTeacher;
        private MetroFramework.Controls.MetroComboBox mcbTeacher;
        private MetroFramework.Controls.MetroLabel mlblItemCount;
        private MetroFramework.Controls.MetroContextMenu mcmReport;
        private System.Windows.Forms.ToolStripMenuItem mnuViewReport;
        private System.Windows.Forms.ToolStripSeparator tssSeparator;
        private System.Windows.Forms.ToolStripMenuItem mnuViewInstitution;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripSeparator tssSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuDisplayColumns;
        private System.Windows.Forms.ToolStripMenuItem mnuViewTeacher;
        private System.Windows.Forms.ToolStripMenuItem mnuImpersonateTeacher;
        private System.Windows.Forms.ToolStripSeparator tssSeparatorImpersonate;
        private System.Windows.Forms.ToolStripMenuItem mnuImpersonateCoordinator;
        private System.Windows.Forms.TableLayoutPanel tlpFooter;
        private MetroFramework.Controls.MetroLabel mlblCompletedCount;
        private MetroFramework.Controls.MetroLabel mlblPendingCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReportId;
        private System.Windows.Forms.DataGridViewTextBoxColumn SemesterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PeriodName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AuthorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstitutionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClassData;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReportStatus;
    }
}
