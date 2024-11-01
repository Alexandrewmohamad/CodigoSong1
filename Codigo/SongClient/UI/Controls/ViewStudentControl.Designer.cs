namespace PnT.SongClient.UI.Controls
{
    partial class ViewStudentControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewStudentControl));
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
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.StudentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Birthdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HasDisability = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GuardianName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PoleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserLogin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StudentLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Phones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StudentStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InactivationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InactivationReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpFilters = new System.Windows.Forms.TableLayoutPanel();
            this.mcbStatus = new MetroFramework.Controls.MetroComboBox();
            this.mlblStatus = new MetroFramework.Controls.MetroLabel();
            this.mlblInstitution = new MetroFramework.Controls.MetroLabel();
            this.mlblPole = new MetroFramework.Controls.MetroLabel();
            this.mcbInstitution = new MetroFramework.Controls.MetroComboBox();
            this.mcbPole = new MetroFramework.Controls.MetroComboBox();
            this.mlblItemCount = new MetroFramework.Controls.MetroLabel();
            this.mcmStudent = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.mnuViewStudent = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewPole = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDisplayColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.tlpFilters.SuspendLayout();
            this.mcmStudent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mtlView, 1, 0);
            this.tlpMain.Controls.Add(this.mlblItemHeading, 0, 0);
            this.tlpMain.Controls.Add(this.dgvStudents, 0, 2);
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
            // dgvStudents
            // 
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.dgvStudents.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStudents.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dgvStudents.BackgroundColor = System.Drawing.Color.White;
            this.dgvStudents.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStudents.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvStudents.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvStudents.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStudents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StudentId,
            this.DisplayName,
            this.Birthdate,
            this.HasDisability,
            this.GuardianName,
            this.PoleName,
            this.UserLogin,
            this.StudentLocation,
            this.Phones,
            this.Email,
            this.StudentStatus,
            this.CreationTime,
            this.InactivationTime,
            this.InactivationReason});
            this.tlpMain.SetColumnSpan(this.dgvStudents, 2);
            this.dgvStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle16.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStudents.DefaultCellStyle = dataGridViewCellStyle16;
            resources.ApplyResources(this.dgvStudents, "dgvStudents");
            this.dgvStudents.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.RowHeadersVisible = false;
            this.dgvStudents.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvStudents.RowTemplate.Height = 44;
            this.dgvStudents.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvStudents_MouseUp);
            // 
            // StudentId
            // 
            this.StudentId.DataPropertyName = "StudentId";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.StudentId.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.StudentId, "StudentId");
            this.StudentId.Name = "StudentId";
            this.StudentId.ReadOnly = true;
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
            // HasDisability
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.HasDisability.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.HasDisability, "HasDisability");
            this.HasDisability.Name = "HasDisability";
            this.HasDisability.ReadOnly = true;
            // 
            // GuardianName
            // 
            this.GuardianName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GuardianName.DataPropertyName = "GuardianName";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GuardianName.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.GuardianName, "GuardianName");
            this.GuardianName.Name = "GuardianName";
            this.GuardianName.ReadOnly = true;
            // 
            // PoleName
            // 
            this.PoleName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PoleName.DataPropertyName = "PoleName";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PoleName.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.PoleName, "PoleName");
            this.PoleName.Name = "PoleName";
            this.PoleName.ReadOnly = true;
            // 
            // UserLogin
            // 
            this.UserLogin.DataPropertyName = "UserLogin";
            resources.ApplyResources(this.UserLogin, "UserLogin");
            this.UserLogin.Name = "UserLogin";
            this.UserLogin.ReadOnly = true;
            // 
            // StudentLocation
            // 
            this.StudentLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.StudentLocation.DataPropertyName = "Location";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.StudentLocation.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.StudentLocation, "StudentLocation");
            this.StudentLocation.Name = "StudentLocation";
            this.StudentLocation.ReadOnly = true;
            // 
            // Phones
            // 
            this.Phones.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Phones.DataPropertyName = "Phones";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Phones.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.Phones, "Phones");
            this.Phones.Name = "Phones";
            this.Phones.ReadOnly = true;
            // 
            // Email
            // 
            this.Email.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Email.DataPropertyName = "Email";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Email.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.Email, "Email");
            this.Email.Name = "Email";
            this.Email.ReadOnly = true;
            // 
            // StudentStatus
            // 
            this.StudentStatus.DataPropertyName = "StudentStatusName";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.StudentStatus.DefaultCellStyle = dataGridViewCellStyle12;
            resources.ApplyResources(this.StudentStatus, "StudentStatus");
            this.StudentStatus.Name = "StudentStatus";
            this.StudentStatus.ReadOnly = true;
            // 
            // CreationTime
            // 
            this.CreationTime.DataPropertyName = "CreationTime";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle13.Format = "g";
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
            this.tlpFilters.Controls.Add(this.mlblPole, 6, 0);
            this.tlpFilters.Controls.Add(this.mcbInstitution, 4, 0);
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
            // mlblPole
            // 
            resources.ApplyResources(this.mlblPole, "mlblPole");
            this.mlblPole.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblPole.Name = "mlblPole";
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
            // mcmStudent
            // 
            this.mcmStudent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewStudent,
            this.mnuViewPole,
            this.tssSeparator,
            this.mnuCopy,
            this.tssSeparator2,
            this.mnuDisplayColumns});
            this.mcmStudent.Name = "mcmStudent";
            resources.ApplyResources(this.mcmStudent, "mcmStudent");
            // 
            // mnuViewStudent
            // 
            this.mnuViewStudent.Name = "mnuViewStudent";
            resources.ApplyResources(this.mnuViewStudent, "mnuViewStudent");
            this.mnuViewStudent.Click += new System.EventHandler(this.mnuViewStudent_Click);
            // 
            // mnuViewPole
            // 
            this.mnuViewPole.Name = "mnuViewPole";
            resources.ApplyResources(this.mnuViewPole, "mnuViewPole");
            this.mnuViewPole.Click += new System.EventHandler(this.mnuViewPole_Click);
            // 
            // tssSeparator
            // 
            this.tssSeparator.Name = "tssSeparator";
            resources.ApplyResources(this.tssSeparator, "tssSeparator");
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
            // ViewStudentControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "ViewStudentControl";
            this.Load += new System.EventHandler(this.ViewStudentControl_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.tlpFilters.ResumeLayout(false);
            this.tlpFilters.PerformLayout();
            this.mcmStudent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroTile mtlView;
        private MetroFramework.Controls.MetroLabel mlblItemHeading;
        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.TableLayoutPanel tlpFilters;
        private MetroFramework.Controls.MetroComboBox mcbStatus;
        private MetroFramework.Controls.MetroLabel mlblStatus;
        private MetroFramework.Controls.MetroLabel mlblPole;
        private MetroFramework.Controls.MetroComboBox mcbPole;
        private MetroFramework.Controls.MetroLabel mlblItemCount;
        private MetroFramework.Controls.MetroContextMenu mcmStudent;
        private System.Windows.Forms.ToolStripMenuItem mnuViewStudent;
        private System.Windows.Forms.ToolStripMenuItem mnuViewPole;
        private System.Windows.Forms.ToolStripSeparator tssSeparator;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private MetroFramework.Controls.MetroLabel mlblInstitution;
        private MetroFramework.Controls.MetroComboBox mcbInstitution;
        private System.Windows.Forms.ToolStripSeparator tssSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuDisplayColumns;
        private System.Windows.Forms.DataGridViewTextBoxColumn StudentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Birthdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn HasDisability;
        private System.Windows.Forms.DataGridViewTextBoxColumn GuardianName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PoleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserLogin;
        private System.Windows.Forms.DataGridViewTextBoxColumn StudentLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Phones;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn StudentStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn InactivationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn InactivationReason;
    }
}
