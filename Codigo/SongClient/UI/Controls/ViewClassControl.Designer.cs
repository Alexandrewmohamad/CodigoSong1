namespace PnT.SongClient.UI.Controls
{
    partial class ViewClassControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewClassControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mtlView = new MetroFramework.Controls.MetroTile();
            this.mlblItemHeading = new MetroFramework.Controls.MetroLabel();
            this.dgvClasses = new System.Windows.Forms.DataGridView();
            this.ClassId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SemesterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubjectCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PoleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeacherName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AggregatedTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClassLevelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Capacity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WeekDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClassStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InactivationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InactivationReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpFilters = new System.Windows.Forms.TableLayoutPanel();
            this.mlblSemester = new MetroFramework.Controls.MetroLabel();
            this.mcbStatus = new MetroFramework.Controls.MetroComboBox();
            this.mlblStatus = new MetroFramework.Controls.MetroLabel();
            this.mlblClassType = new MetroFramework.Controls.MetroLabel();
            this.mcbClassType = new MetroFramework.Controls.MetroComboBox();
            this.mlblPole = new MetroFramework.Controls.MetroLabel();
            this.mcbPole = new MetroFramework.Controls.MetroComboBox();
            this.mlblInstitution = new MetroFramework.Controls.MetroLabel();
            this.mcbInstitution = new MetroFramework.Controls.MetroComboBox();
            this.mcbSemester = new MetroFramework.Controls.MetroComboBox();
            this.mlblInstrumentType = new MetroFramework.Controls.MetroLabel();
            this.mcbInstrumentType = new MetroFramework.Controls.MetroComboBox();
            this.mlblClassLevel = new MetroFramework.Controls.MetroLabel();
            this.mcbClassLevel = new MetroFramework.Controls.MetroComboBox();
            this.mlblTeacher = new MetroFramework.Controls.MetroLabel();
            this.mcbTeacher = new MetroFramework.Controls.MetroComboBox();
            this.mlblItemCount = new MetroFramework.Controls.MetroLabel();
            this.mcmClass = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.mnuViewClass = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewPole = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewTeacher = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuGenerateReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImpersonateTeacher = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparatorSpecial = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDisplayColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClasses)).BeginInit();
            this.tlpFilters.SuspendLayout();
            this.mcmClass.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mtlView, 1, 0);
            this.tlpMain.Controls.Add(this.mlblItemHeading, 0, 0);
            this.tlpMain.Controls.Add(this.dgvClasses, 0, 2);
            this.tlpMain.Controls.Add(this.tlpFilters, 0, 1);
            this.tlpMain.Controls.Add(this.mlblItemCount, 0, 3);
            this.tlpMain.Name = "tlpMain";
            // 
            // mtlView
            // 
            resources.ApplyResources(this.mtlView, "mtlView");
            this.mtlView.ActiveControl = null;
            this.mtlView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlView.Name = "mtlView";
            this.mtlView.Style = MetroFramework.MetroColorStyle.White;
            this.mtlView.Tag = "Edit";
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
            // dgvClasses
            // 
            resources.ApplyResources(this.dgvClasses, "dgvClasses");
            this.dgvClasses.AllowUserToAddRows = false;
            this.dgvClasses.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.dgvClasses.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvClasses.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgvClasses.BackgroundColor = System.Drawing.Color.White;
            this.dgvClasses.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvClasses.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvClasses.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvClasses.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvClasses.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvClasses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClasses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClassId,
            this.SemesterName,
            this.SubjectCode,
            this.Code,
            this.PoleName,
            this.TeacherName,
            this.AggregatedTypeName,
            this.ClassLevelName,
            this.Capacity,
            this.WeekDays,
            this.StartTime,
            this.Duration,
            this.ClassStatus,
            this.CreationTime,
            this.InactivationTime,
            this.InactivationReason});
            this.tlpMain.SetColumnSpan(this.dgvClasses, 2);
            this.dgvClasses.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle19.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvClasses.DefaultCellStyle = dataGridViewCellStyle19;
            this.dgvClasses.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvClasses.Name = "dgvClasses";
            this.dgvClasses.ReadOnly = true;
            this.dgvClasses.RowHeadersVisible = false;
            this.dgvClasses.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvClasses.RowTemplate.Height = 44;
            this.dgvClasses.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvClasses_MouseUp);
            // 
            // ClassId
            // 
            this.ClassId.DataPropertyName = "ClassId";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.ClassId.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.ClassId, "ClassId");
            this.ClassId.Name = "ClassId";
            this.ClassId.ReadOnly = true;
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
            // SubjectCode
            // 
            this.SubjectCode.DataPropertyName = "SubjectCode";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.SubjectCode.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.SubjectCode, "SubjectCode");
            this.SubjectCode.Name = "SubjectCode";
            this.SubjectCode.ReadOnly = true;
            // 
            // Code
            // 
            this.Code.DataPropertyName = "Code";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Code.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.Code, "Code");
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            // 
            // PoleName
            // 
            this.PoleName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PoleName.DataPropertyName = "PoleName";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PoleName.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.PoleName, "PoleName");
            this.PoleName.Name = "PoleName";
            this.PoleName.ReadOnly = true;
            // 
            // TeacherName
            // 
            this.TeacherName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TeacherName.DataPropertyName = "TeacherName";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TeacherName.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.TeacherName, "TeacherName");
            this.TeacherName.Name = "TeacherName";
            this.TeacherName.ReadOnly = true;
            // 
            // AggregatedTypeName
            // 
            this.AggregatedTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AggregatedTypeName.DataPropertyName = "AggregatedTypeName";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AggregatedTypeName.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.AggregatedTypeName, "AggregatedTypeName");
            this.AggregatedTypeName.Name = "AggregatedTypeName";
            this.AggregatedTypeName.ReadOnly = true;
            // 
            // ClassLevelName
            // 
            this.ClassLevelName.DataPropertyName = "ClassLevelName";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.ClassLevelName.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.ClassLevelName, "ClassLevelName");
            this.ClassLevelName.Name = "ClassLevelName";
            this.ClassLevelName.ReadOnly = true;
            // 
            // Capacity
            // 
            this.Capacity.DataPropertyName = "Capacity";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.Capacity.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.Capacity, "Capacity");
            this.Capacity.Name = "Capacity";
            this.Capacity.ReadOnly = true;
            // 
            // WeekDays
            // 
            this.WeekDays.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WeekDays.DataPropertyName = "WeekDays";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.WeekDays.DefaultCellStyle = dataGridViewCellStyle12;
            resources.ApplyResources(this.WeekDays, "WeekDays");
            this.WeekDays.Name = "WeekDays";
            this.WeekDays.ReadOnly = true;
            // 
            // StartTime
            // 
            this.StartTime.DataPropertyName = "StartTime";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle13.Format = "t";
            dataGridViewCellStyle13.NullValue = null;
            this.StartTime.DefaultCellStyle = dataGridViewCellStyle13;
            resources.ApplyResources(this.StartTime, "StartTime");
            this.StartTime.Name = "StartTime";
            this.StartTime.ReadOnly = true;
            // 
            // Duration
            // 
            this.Duration.DataPropertyName = "Duration";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.Duration.DefaultCellStyle = dataGridViewCellStyle14;
            resources.ApplyResources(this.Duration, "Duration");
            this.Duration.Name = "Duration";
            this.Duration.ReadOnly = true;
            // 
            // ClassStatus
            // 
            this.ClassStatus.DataPropertyName = "ClassStatusName";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.ClassStatus.DefaultCellStyle = dataGridViewCellStyle15;
            resources.ApplyResources(this.ClassStatus, "ClassStatus");
            this.ClassStatus.Name = "ClassStatus";
            this.ClassStatus.ReadOnly = true;
            // 
            // CreationTime
            // 
            this.CreationTime.DataPropertyName = "CreationTime";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle16.Format = "g";
            dataGridViewCellStyle16.NullValue = null;
            this.CreationTime.DefaultCellStyle = dataGridViewCellStyle16;
            resources.ApplyResources(this.CreationTime, "CreationTime");
            this.CreationTime.Name = "CreationTime";
            this.CreationTime.ReadOnly = true;
            // 
            // InactivationTime
            // 
            this.InactivationTime.DataPropertyName = "InactivationTime";
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle17.Format = "g";
            dataGridViewCellStyle17.NullValue = null;
            this.InactivationTime.DefaultCellStyle = dataGridViewCellStyle17;
            resources.ApplyResources(this.InactivationTime, "InactivationTime");
            this.InactivationTime.Name = "InactivationTime";
            this.InactivationTime.ReadOnly = true;
            // 
            // InactivationReason
            // 
            this.InactivationReason.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InactivationReason.DataPropertyName = "InactivationReason";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InactivationReason.DefaultCellStyle = dataGridViewCellStyle18;
            resources.ApplyResources(this.InactivationReason, "InactivationReason");
            this.InactivationReason.Name = "InactivationReason";
            this.InactivationReason.ReadOnly = true;
            // 
            // tlpFilters
            // 
            resources.ApplyResources(this.tlpFilters, "tlpFilters");
            this.tlpMain.SetColumnSpan(this.tlpFilters, 2);
            this.tlpFilters.Controls.Add(this.mlblSemester, 0, 1);
            this.tlpFilters.Controls.Add(this.mcbStatus, 1, 0);
            this.tlpFilters.Controls.Add(this.mlblStatus, 0, 0);
            this.tlpFilters.Controls.Add(this.mlblClassType, 3, 0);
            this.tlpFilters.Controls.Add(this.mcbClassType, 4, 0);
            this.tlpFilters.Controls.Add(this.mlblPole, 9, 0);
            this.tlpFilters.Controls.Add(this.mcbPole, 10, 0);
            this.tlpFilters.Controls.Add(this.mlblInstitution, 6, 0);
            this.tlpFilters.Controls.Add(this.mcbInstitution, 7, 0);
            this.tlpFilters.Controls.Add(this.mcbSemester, 1, 1);
            this.tlpFilters.Controls.Add(this.mlblInstrumentType, 3, 1);
            this.tlpFilters.Controls.Add(this.mcbInstrumentType, 4, 1);
            this.tlpFilters.Controls.Add(this.mlblClassLevel, 6, 1);
            this.tlpFilters.Controls.Add(this.mcbClassLevel, 7, 1);
            this.tlpFilters.Controls.Add(this.mlblTeacher, 9, 1);
            this.tlpFilters.Controls.Add(this.mcbTeacher, 10, 1);
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
            // mlblClassType
            // 
            resources.ApplyResources(this.mlblClassType, "mlblClassType");
            this.mlblClassType.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblClassType.Name = "mlblClassType";
            // 
            // mcbClassType
            // 
            resources.ApplyResources(this.mcbClassType, "mcbClassType");
            this.mcbClassType.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbClassType.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbClassType.FormattingEnabled = true;
            this.mcbClassType.Name = "mcbClassType";
            this.mcbClassType.UseSelectable = true;
            this.mcbClassType.SelectedIndexChanged += new System.EventHandler(this.mcbClassType_SelectedIndexChanged);
            // 
            // mlblPole
            // 
            resources.ApplyResources(this.mlblPole, "mlblPole");
            this.mlblPole.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblPole.Name = "mlblPole";
            // 
            // mcbPole
            // 
            resources.ApplyResources(this.mcbPole, "mcbPole");
            this.mcbPole.DropDownWidth = 240;
            this.mcbPole.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbPole.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbPole.FormattingEnabled = true;
            this.mcbPole.Name = "mcbPole";
            this.mcbPole.UseSelectable = true;
            this.mcbPole.SelectedIndexChanged += new System.EventHandler(this.mcbPole_SelectedIndexChanged);
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
            // mlblInstrumentType
            // 
            resources.ApplyResources(this.mlblInstrumentType, "mlblInstrumentType");
            this.mlblInstrumentType.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblInstrumentType.Name = "mlblInstrumentType";
            // 
            // mcbInstrumentType
            // 
            resources.ApplyResources(this.mcbInstrumentType, "mcbInstrumentType");
            this.mcbInstrumentType.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbInstrumentType.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbInstrumentType.FormattingEnabled = true;
            this.mcbInstrumentType.Name = "mcbInstrumentType";
            this.mcbInstrumentType.UseSelectable = true;
            this.mcbInstrumentType.SelectedIndexChanged += new System.EventHandler(this.mcbInstrumentType_SelectedIndexChanged);
            // 
            // mlblClassLevel
            // 
            resources.ApplyResources(this.mlblClassLevel, "mlblClassLevel");
            this.mlblClassLevel.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblClassLevel.Name = "mlblClassLevel";
            // 
            // mcbClassLevel
            // 
            resources.ApplyResources(this.mcbClassLevel, "mcbClassLevel");
            this.mcbClassLevel.DropDownWidth = 150;
            this.mcbClassLevel.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbClassLevel.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbClassLevel.FormattingEnabled = true;
            this.mcbClassLevel.Name = "mcbClassLevel";
            this.mcbClassLevel.UseSelectable = true;
            this.mcbClassLevel.SelectedIndexChanged += new System.EventHandler(this.mcbLevel_SelectedIndexChanged);
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
            // mlblItemCount
            // 
            resources.ApplyResources(this.mlblItemCount, "mlblItemCount");
            this.mlblItemCount.Name = "mlblItemCount";
            // 
            // mcmClass
            // 
            resources.ApplyResources(this.mcmClass, "mcmClass");
            this.mcmClass.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewClass,
            this.mnuViewPole,
            this.mnuViewTeacher,
            this.tssSeparator,
            this.mnuGenerateReports,
            this.mnuImpersonateTeacher,
            this.tssSeparatorSpecial,
            this.mnuCopy,
            this.tssSeparator2,
            this.mnuDisplayColumns});
            this.mcmClass.Name = "mcmClass";
            // 
            // mnuViewClass
            // 
            resources.ApplyResources(this.mnuViewClass, "mnuViewClass");
            this.mnuViewClass.Name = "mnuViewClass";
            this.mnuViewClass.Click += new System.EventHandler(this.mnuViewClass_Click);
            // 
            // mnuViewPole
            // 
            resources.ApplyResources(this.mnuViewPole, "mnuViewPole");
            this.mnuViewPole.Name = "mnuViewPole";
            this.mnuViewPole.Click += new System.EventHandler(this.mnuViewPole_Click);
            // 
            // mnuViewTeacher
            // 
            resources.ApplyResources(this.mnuViewTeacher, "mnuViewTeacher");
            this.mnuViewTeacher.Name = "mnuViewTeacher";
            this.mnuViewTeacher.Click += new System.EventHandler(this.mnuViewTeacher_Click);
            // 
            // tssSeparator
            // 
            resources.ApplyResources(this.tssSeparator, "tssSeparator");
            this.tssSeparator.Name = "tssSeparator";
            // 
            // mnuGenerateReports
            // 
            resources.ApplyResources(this.mnuGenerateReports, "mnuGenerateReports");
            this.mnuGenerateReports.Name = "mnuGenerateReports";
            this.mnuGenerateReports.Click += new System.EventHandler(this.mnuGenerateReports_Click);
            // 
            // mnuImpersonateTeacher
            // 
            resources.ApplyResources(this.mnuImpersonateTeacher, "mnuImpersonateTeacher");
            this.mnuImpersonateTeacher.Name = "mnuImpersonateTeacher";
            this.mnuImpersonateTeacher.Click += new System.EventHandler(this.mnuImpersonateTeacher_Click);
            // 
            // tssSeparatorSpecial
            // 
            resources.ApplyResources(this.tssSeparatorSpecial, "tssSeparatorSpecial");
            this.tssSeparatorSpecial.Name = "tssSeparatorSpecial";
            // 
            // mnuCopy
            // 
            resources.ApplyResources(this.mnuCopy, "mnuCopy");
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // tssSeparator2
            // 
            resources.ApplyResources(this.tssSeparator2, "tssSeparator2");
            this.tssSeparator2.Name = "tssSeparator2";
            // 
            // mnuDisplayColumns
            // 
            resources.ApplyResources(this.mnuDisplayColumns, "mnuDisplayColumns");
            this.mnuDisplayColumns.Name = "mnuDisplayColumns";
            this.mnuDisplayColumns.Click += new System.EventHandler(this.mnuDisplayColumns_Click);
            // 
            // ViewClassControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "ViewClassControl";
            this.Load += new System.EventHandler(this.ViewClassControl_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClasses)).EndInit();
            this.tlpFilters.ResumeLayout(false);
            this.tlpFilters.PerformLayout();
            this.mcmClass.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroTile mtlView;
        private MetroFramework.Controls.MetroLabel mlblItemHeading;
        private System.Windows.Forms.DataGridView dgvClasses;
        private System.Windows.Forms.TableLayoutPanel tlpFilters;
        private MetroFramework.Controls.MetroComboBox mcbStatus;
        private MetroFramework.Controls.MetroLabel mlblStatus;
        private MetroFramework.Controls.MetroLabel mlblClassType;
        private MetroFramework.Controls.MetroComboBox mcbClassType;
        private MetroFramework.Controls.MetroLabel mlblPole;
        private MetroFramework.Controls.MetroComboBox mcbPole;
        private MetroFramework.Controls.MetroLabel mlblInstitution;
        private MetroFramework.Controls.MetroComboBox mcbInstitution;
        private MetroFramework.Controls.MetroLabel mlblItemCount;
        private MetroFramework.Controls.MetroContextMenu mcmClass;
        private System.Windows.Forms.ToolStripMenuItem mnuViewClass;
        private System.Windows.Forms.ToolStripMenuItem mnuViewPole;
        private System.Windows.Forms.ToolStripMenuItem mnuViewTeacher;
        private System.Windows.Forms.ToolStripSeparator tssSeparator;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripSeparator tssSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuDisplayColumns;
        private MetroFramework.Controls.MetroLabel mlblSemester;
        private MetroFramework.Controls.MetroComboBox mcbSemester;
        private MetroFramework.Controls.MetroLabel mlblInstrumentType;
        private MetroFramework.Controls.MetroComboBox mcbInstrumentType;
        private MetroFramework.Controls.MetroLabel mlblClassLevel;
        private MetroFramework.Controls.MetroComboBox mcbClassLevel;
        private MetroFramework.Controls.MetroLabel mlblTeacher;
        private MetroFramework.Controls.MetroComboBox mcbTeacher;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClassId;
        private System.Windows.Forms.DataGridViewTextBoxColumn SemesterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubjectCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn PoleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TeacherName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AggregatedTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClassLevelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Capacity;
        private System.Windows.Forms.DataGridViewTextBoxColumn WeekDays;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Duration;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClassStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn InactivationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn InactivationReason;
        private System.Windows.Forms.ToolStripMenuItem mnuImpersonateTeacher;
        private System.Windows.Forms.ToolStripSeparator tssSeparatorSpecial;
        private System.Windows.Forms.ToolStripMenuItem mnuGenerateReports;
    }
}
