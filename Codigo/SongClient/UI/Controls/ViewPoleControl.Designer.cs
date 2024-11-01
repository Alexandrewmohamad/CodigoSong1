namespace PnT.SongClient.UI.Controls
{
    partial class ViewPoleControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewPoleControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mtlView = new MetroFramework.Controls.MetroTile();
            this.mlblItemHeading = new MetroFramework.Controls.MetroLabel();
            this.dgvPoles = new System.Windows.Forms.DataGridView();
            this.PoleId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstitutionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PoleLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Phones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PoleStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InactivationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InactivationReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpFilters = new System.Windows.Forms.TableLayoutPanel();
            this.mcbStatus = new MetroFramework.Controls.MetroComboBox();
            this.mlblStatus = new MetroFramework.Controls.MetroLabel();
            this.mlblInstitution = new MetroFramework.Controls.MetroLabel();
            this.mcbInstitution = new MetroFramework.Controls.MetroComboBox();
            this.mlblItemCount = new MetroFramework.Controls.MetroLabel();
            this.mcmPole = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.mnuViewPole = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewInstitution = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDisplayColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoles)).BeginInit();
            this.tlpFilters.SuspendLayout();
            this.mcmPole.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mtlView, 1, 0);
            this.tlpMain.Controls.Add(this.mlblItemHeading, 0, 0);
            this.tlpMain.Controls.Add(this.dgvPoles, 0, 2);
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
            // dgvPoles
            // 
            this.dgvPoles.AllowUserToAddRows = false;
            this.dgvPoles.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.dgvPoles.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPoles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dgvPoles.BackgroundColor = System.Drawing.Color.White;
            this.dgvPoles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPoles.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvPoles.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvPoles.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPoles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPoles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PoleId,
            this.DisplayName,
            this.InstitutionName,
            this.PoleLocation,
            this.Phones,
            this.Email,
            this.PoleStatus,
            this.CreationTime,
            this.InactivationTime,
            this.InactivationReason});
            this.tlpMain.SetColumnSpan(this.dgvPoles, 2);
            this.dgvPoles.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle13.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPoles.DefaultCellStyle = dataGridViewCellStyle13;
            resources.ApplyResources(this.dgvPoles, "dgvPoles");
            this.dgvPoles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPoles.Name = "dgvPoles";
            this.dgvPoles.ReadOnly = true;
            this.dgvPoles.RowHeadersVisible = false;
            this.dgvPoles.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPoles.RowTemplate.Height = 44;
            this.dgvPoles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvPoles_MouseUp);
            // 
            // PoleId
            // 
            this.PoleId.DataPropertyName = "PoleId";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.PoleId.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.PoleId, "PoleId");
            this.PoleId.Name = "PoleId";
            this.PoleId.ReadOnly = true;
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
            // InstitutionName
            // 
            this.InstitutionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InstitutionName.DataPropertyName = "InstitutionName";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InstitutionName.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.InstitutionName, "InstitutionName");
            this.InstitutionName.Name = "InstitutionName";
            this.InstitutionName.ReadOnly = true;
            // 
            // PoleLocation
            // 
            this.PoleLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PoleLocation.DataPropertyName = "Location";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PoleLocation.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.PoleLocation, "PoleLocation");
            this.PoleLocation.Name = "PoleLocation";
            this.PoleLocation.ReadOnly = true;
            // 
            // Phones
            // 
            this.Phones.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Phones.DataPropertyName = "Phones";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Phones.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.Phones, "Phones");
            this.Phones.Name = "Phones";
            this.Phones.ReadOnly = true;
            // 
            // Email
            // 
            this.Email.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Email.DataPropertyName = "Email";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Email.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.Email, "Email");
            this.Email.Name = "Email";
            this.Email.ReadOnly = true;
            // 
            // PoleStatus
            // 
            this.PoleStatus.DataPropertyName = "PoleStatusName";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.PoleStatus.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.PoleStatus, "PoleStatus");
            this.PoleStatus.Name = "PoleStatus";
            this.PoleStatus.ReadOnly = true;
            // 
            // CreationTime
            // 
            this.CreationTime.DataPropertyName = "CreationTime";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle10.Format = "g";
            this.CreationTime.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.CreationTime, "CreationTime");
            this.CreationTime.Name = "CreationTime";
            this.CreationTime.ReadOnly = true;
            // 
            // InactivationTime
            // 
            this.InactivationTime.DataPropertyName = "InactivationTime";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle11.Format = "g";
            this.InactivationTime.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.InactivationTime, "InactivationTime");
            this.InactivationTime.Name = "InactivationTime";
            this.InactivationTime.ReadOnly = true;
            // 
            // InactivationReason
            // 
            this.InactivationReason.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InactivationReason.DataPropertyName = "InactivationReason";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InactivationReason.DefaultCellStyle = dataGridViewCellStyle12;
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
            // mlblItemCount
            // 
            resources.ApplyResources(this.mlblItemCount, "mlblItemCount");
            this.mlblItemCount.Name = "mlblItemCount";
            // 
            // mcmPole
            // 
            this.mcmPole.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewPole,
            this.mnuViewInstitution,
            this.tssSeparator,
            this.mnuCopy,
            this.tssSeparator2,
            this.mnuDisplayColumns});
            this.mcmPole.Name = "mcmPole";
            resources.ApplyResources(this.mcmPole, "mcmPole");
            // 
            // mnuViewPole
            // 
            this.mnuViewPole.Name = "mnuViewPole";
            resources.ApplyResources(this.mnuViewPole, "mnuViewPole");
            this.mnuViewPole.Click += new System.EventHandler(this.mnuViewPole_Click);
            // 
            // mnuViewInstitution
            // 
            this.mnuViewInstitution.Name = "mnuViewInstitution";
            resources.ApplyResources(this.mnuViewInstitution, "mnuViewInstitution");
            this.mnuViewInstitution.Click += new System.EventHandler(this.mnuViewInstitution_Click);
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
            // ViewPoleControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "ViewPoleControl";
            this.Load += new System.EventHandler(this.ViewPoleControl_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoles)).EndInit();
            this.tlpFilters.ResumeLayout(false);
            this.tlpFilters.PerformLayout();
            this.mcmPole.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroTile mtlView;
        private MetroFramework.Controls.MetroLabel mlblItemHeading;
        private System.Windows.Forms.DataGridView dgvPoles;
        private System.Windows.Forms.TableLayoutPanel tlpFilters;
        private MetroFramework.Controls.MetroComboBox mcbStatus;
        private MetroFramework.Controls.MetroLabel mlblStatus;
        private MetroFramework.Controls.MetroContextMenu mcmPole;
        private System.Windows.Forms.ToolStripMenuItem mnuViewPole;
        private MetroFramework.Controls.MetroLabel mlblInstitution;
        private MetroFramework.Controls.MetroComboBox mcbInstitution;
        private System.Windows.Forms.ToolStripMenuItem mnuViewInstitution;
        private MetroFramework.Controls.MetroLabel mlblItemCount;
        private System.Windows.Forms.ToolStripSeparator tssSeparator;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripSeparator tssSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuDisplayColumns;
        private System.Windows.Forms.DataGridViewTextBoxColumn PoleId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstitutionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PoleLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Phones;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn PoleStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn InactivationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn InactivationReason;
    }
}
