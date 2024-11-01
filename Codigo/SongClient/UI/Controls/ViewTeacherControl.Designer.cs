namespace PnT.SongClient.UI.Controls
{
    partial class ViewTeacherControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewTeacherControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mtlView = new MetroFramework.Controls.MetroTile();
            this.mlblItemHeading = new MetroFramework.Controls.MetroLabel();
            this.dgvTeachers = new System.Windows.Forms.DataGridView();
            this.TeacherId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Birthdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserLogin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Poles = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeacherLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Phones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AcademicBackground = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeacherStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InactivationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InactivationReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpFilters = new System.Windows.Forms.TableLayoutPanel();
            this.mcbStatus = new MetroFramework.Controls.MetroComboBox();
            this.mlblStatus = new MetroFramework.Controls.MetroLabel();
            this.mlblInstitution = new MetroFramework.Controls.MetroLabel();
            this.mcbInstitution = new MetroFramework.Controls.MetroComboBox();
            this.mlblPole = new MetroFramework.Controls.MetroLabel();
            this.mcbPole = new MetroFramework.Controls.MetroComboBox();
            this.mlblItemCount = new MetroFramework.Controls.MetroLabel();
            this.mcmTeacher = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.mnuViewTeacher = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewUser = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuImpersonateTeacher = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparatorImpersonate = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDisplayColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).BeginInit();
            this.tlpFilters.SuspendLayout();
            this.mcmTeacher.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mtlView, 1, 0);
            this.tlpMain.Controls.Add(this.mlblItemHeading, 0, 0);
            this.tlpMain.Controls.Add(this.dgvTeachers, 0, 2);
            this.tlpMain.Controls.Add(this.tlpFilters, 0, 1);
            this.tlpMain.Controls.Add(this.mlblItemCount, 0, 3);
            this.tlpMain.Name = "tlpMain";
            // 
            // mtlView
            // 
            this.mtlView.ActiveControl = null;
            this.mtlView.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.mtlView, "mtlView");
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
            // dgvTeachers
            // 
            this.dgvTeachers.AllowUserToAddRows = false;
            this.dgvTeachers.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.dgvTeachers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTeachers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dgvTeachers.BackgroundColor = System.Drawing.Color.White;
            this.dgvTeachers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTeachers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvTeachers.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvTeachers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTeachers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTeachers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTeachers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TeacherId,
            this.DisplayName,
            this.Birthdate,
            this.UserLogin,
            this.Poles,
            this.TeacherLocation,
            this.Phones,
            this.Email,
            this.AcademicBackground,
            this.TeacherStatus,
            this.CreationTime,
            this.InactivationTime,
            this.InactivationReason});
            this.tlpMain.SetColumnSpan(this.dgvTeachers, 2);
            this.dgvTeachers.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle16.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTeachers.DefaultCellStyle = dataGridViewCellStyle16;
            resources.ApplyResources(this.dgvTeachers, "dgvTeachers");
            this.dgvTeachers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvTeachers.Name = "dgvTeachers";
            this.dgvTeachers.ReadOnly = true;
            this.dgvTeachers.RowHeadersVisible = false;
            this.dgvTeachers.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTeachers.RowTemplate.Height = 44;
            this.dgvTeachers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvTeachers_MouseUp);
            // 
            // TeacherId
            // 
            this.TeacherId.DataPropertyName = "TeacherId";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.TeacherId.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.TeacherId, "TeacherId");
            this.TeacherId.Name = "TeacherId";
            this.TeacherId.ReadOnly = true;
            // 
            // DisplayName
            // 
            this.DisplayName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DisplayName.DataPropertyName = "Name";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DisplayName.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.DisplayName, "DisplayName");
            this.DisplayName.Name = "DisplayName";
            this.DisplayName.ReadOnly = true;
            // 
            // Birthdate
            // 
            this.Birthdate.DataPropertyName = "Birthdate";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle5.Format = "d";
            dataGridViewCellStyle5.NullValue = null;
            this.Birthdate.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.Birthdate, "Birthdate");
            this.Birthdate.Name = "Birthdate";
            this.Birthdate.ReadOnly = true;
            // 
            // UserLogin
            // 
            this.UserLogin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UserLogin.DataPropertyName = "UserLogin";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.UserLogin.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.UserLogin, "UserLogin");
            this.UserLogin.Name = "UserLogin";
            this.UserLogin.ReadOnly = true;
            // 
            // Poles
            // 
            this.Poles.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Poles.DataPropertyName = "Poles";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Poles.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.Poles, "Poles");
            this.Poles.Name = "Poles";
            this.Poles.ReadOnly = true;
            // 
            // TeacherLocation
            // 
            this.TeacherLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TeacherLocation.DataPropertyName = "Location";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TeacherLocation.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.TeacherLocation, "TeacherLocation");
            this.TeacherLocation.Name = "TeacherLocation";
            this.TeacherLocation.ReadOnly = true;
            // 
            // Phones
            // 
            this.Phones.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Phones.DataPropertyName = "Phones";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Phones.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.Phones, "Phones");
            this.Phones.Name = "Phones";
            this.Phones.ReadOnly = true;
            // 
            // Email
            // 
            this.Email.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Email.DataPropertyName = "Email";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Email.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.Email, "Email");
            this.Email.Name = "Email";
            this.Email.ReadOnly = true;
            // 
            // AcademicBackground
            // 
            this.AcademicBackground.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AcademicBackground.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.AcademicBackground, "AcademicBackground");
            this.AcademicBackground.Name = "AcademicBackground";
            this.AcademicBackground.ReadOnly = true;
            // 
            // TeacherStatus
            // 
            this.TeacherStatus.DataPropertyName = "TeacherStatusName";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.TeacherStatus.DefaultCellStyle = dataGridViewCellStyle12;
            resources.ApplyResources(this.TeacherStatus, "TeacherStatus");
            this.TeacherStatus.Name = "TeacherStatus";
            this.TeacherStatus.ReadOnly = true;
            // 
            // CreationTime
            // 
            this.CreationTime.DataPropertyName = "CreationTime";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle13.Format = "g";
            dataGridViewCellStyle13.NullValue = null;
            this.CreationTime.DefaultCellStyle = dataGridViewCellStyle13;
            resources.ApplyResources(this.CreationTime, "CreationTime");
            this.CreationTime.Name = "CreationTime";
            this.CreationTime.ReadOnly = true;
            // 
            // InactivationTime
            // 
            this.InactivationTime.DataPropertyName = "InactivationTime";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle14.Format = "g";
            dataGridViewCellStyle14.NullValue = null;
            this.InactivationTime.DefaultCellStyle = dataGridViewCellStyle14;
            resources.ApplyResources(this.InactivationTime, "InactivationTime");
            this.InactivationTime.Name = "InactivationTime";
            this.InactivationTime.ReadOnly = true;
            // 
            // InactivationReason
            // 
            this.InactivationReason.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InactivationReason.DataPropertyName = "InactivationReason";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InactivationReason.DefaultCellStyle = dataGridViewCellStyle15;
            resources.ApplyResources(this.InactivationReason, "InactivationReason");
            this.InactivationReason.Name = "InactivationReason";
            this.InactivationReason.ReadOnly = true;
            // 
            // tlpFilters
            // 
            resources.ApplyResources(this.tlpFilters, "tlpFilters");
            this.tlpMain.SetColumnSpan(this.tlpFilters, 2);
            this.tlpFilters.Controls.Add(this.mcbStatus, 1, 0);
            this.tlpFilters.Controls.Add(this.mlblStatus, 0, 0);
            this.tlpFilters.Controls.Add(this.mlblInstitution, 3, 0);
            this.tlpFilters.Controls.Add(this.mcbInstitution, 4, 0);
            this.tlpFilters.Controls.Add(this.mlblPole, 6, 0);
            this.tlpFilters.Controls.Add(this.mcbPole, 7, 0);
            this.tlpFilters.Name = "tlpFilters";
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
            // mlblItemCount
            // 
            resources.ApplyResources(this.mlblItemCount, "mlblItemCount");
            this.mlblItemCount.Name = "mlblItemCount";
            // 
            // mcmTeacher
            // 
            this.mcmTeacher.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewTeacher,
            this.mnuViewUser,
            this.tssSeparator,
            this.mnuImpersonateTeacher,
            this.tssSeparatorImpersonate,
            this.mnuCopy,
            this.tssSeparator2,
            this.mnuDisplayColumns});
            this.mcmTeacher.Name = "mcmTeacher";
            resources.ApplyResources(this.mcmTeacher, "mcmTeacher");
            // 
            // mnuViewTeacher
            // 
            this.mnuViewTeacher.Name = "mnuViewTeacher";
            resources.ApplyResources(this.mnuViewTeacher, "mnuViewTeacher");
            this.mnuViewTeacher.Click += new System.EventHandler(this.mnuViewTeacher_Click);
            // 
            // mnuViewUser
            // 
            this.mnuViewUser.Name = "mnuViewUser";
            resources.ApplyResources(this.mnuViewUser, "mnuViewUser");
            this.mnuViewUser.Click += new System.EventHandler(this.mnuViewUser_Click);
            // 
            // tssSeparator
            // 
            this.tssSeparator.Name = "tssSeparator";
            resources.ApplyResources(this.tssSeparator, "tssSeparator");
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
            // ViewTeacherControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "ViewTeacherControl";
            this.Load += new System.EventHandler(this.ViewTeacherControl_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).EndInit();
            this.tlpFilters.ResumeLayout(false);
            this.tlpFilters.PerformLayout();
            this.mcmTeacher.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroTile mtlView;
        private MetroFramework.Controls.MetroLabel mlblItemHeading;
        private System.Windows.Forms.DataGridView dgvTeachers;
        private System.Windows.Forms.TableLayoutPanel tlpFilters;
        private MetroFramework.Controls.MetroComboBox mcbStatus;
        private MetroFramework.Controls.MetroLabel mlblStatus;
        private MetroFramework.Controls.MetroLabel mlblInstitution;
        private MetroFramework.Controls.MetroComboBox mcbInstitution;
        private MetroFramework.Controls.MetroLabel mlblItemCount;
        private MetroFramework.Controls.MetroContextMenu mcmTeacher;
        private System.Windows.Forms.ToolStripMenuItem mnuViewTeacher;
        private MetroFramework.Controls.MetroLabel mlblPole;
        private MetroFramework.Controls.MetroComboBox mcbPole;
        private System.Windows.Forms.ToolStripMenuItem mnuViewUser;
        private System.Windows.Forms.ToolStripSeparator tssSeparator;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripSeparator tssSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuDisplayColumns;
        private System.Windows.Forms.DataGridViewTextBoxColumn TeacherId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Birthdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserLogin;
        private System.Windows.Forms.DataGridViewTextBoxColumn Poles;
        private System.Windows.Forms.DataGridViewTextBoxColumn TeacherLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Phones;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn AcademicBackground;
        private System.Windows.Forms.DataGridViewTextBoxColumn TeacherStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn InactivationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn InactivationReason;
        private System.Windows.Forms.ToolStripMenuItem mnuImpersonateTeacher;
        private System.Windows.Forms.ToolStripSeparator tssSeparatorImpersonate;
    }
}
