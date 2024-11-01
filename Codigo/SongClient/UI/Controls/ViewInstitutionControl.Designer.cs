namespace PnT.SongClient.UI.Controls
{
    partial class ViewInstitutionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewInstitutionControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mlblItemCount = new MetroFramework.Controls.MetroLabel();
            this.mtlView = new MetroFramework.Controls.MetroTile();
            this.mlblItemHeading = new MetroFramework.Controls.MetroLabel();
            this.dgvInstitutions = new System.Windows.Forms.DataGridView();
            this.InstitutionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EntityName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalInitiative = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Institutionalized = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CoordinatorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstitutionLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Phones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstitutionSite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstitutionStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InactivationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InactivationReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpFilters = new System.Windows.Forms.TableLayoutPanel();
            this.mcbStatus = new MetroFramework.Controls.MetroComboBox();
            this.mlblStatus = new MetroFramework.Controls.MetroLabel();
            this.mcmInstitution = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.mnuViewInstitution = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewCoordinator = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparatorImpersonate = new System.Windows.Forms.ToolStripSeparator();
            this.mnuImpersonateCoordinator = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDisplayColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstitutions)).BeginInit();
            this.tlpFilters.SuspendLayout();
            this.mcmInstitution.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mlblItemCount, 0, 3);
            this.tlpMain.Controls.Add(this.mtlView, 1, 0);
            this.tlpMain.Controls.Add(this.mlblItemHeading, 0, 0);
            this.tlpMain.Controls.Add(this.dgvInstitutions, 0, 2);
            this.tlpMain.Controls.Add(this.tlpFilters, 0, 1);
            this.tlpMain.Name = "tlpMain";
            // 
            // mlblItemCount
            // 
            resources.ApplyResources(this.mlblItemCount, "mlblItemCount");
            this.mlblItemCount.Name = "mlblItemCount";
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
            // dgvInstitutions
            // 
            this.dgvInstitutions.AllowUserToAddRows = false;
            this.dgvInstitutions.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.dgvInstitutions.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvInstitutions.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dgvInstitutions.BackgroundColor = System.Drawing.Color.White;
            this.dgvInstitutions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvInstitutions.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvInstitutions.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvInstitutions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInstitutions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvInstitutions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInstitutions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.InstitutionId,
            this.DisplayName,
            this.EntityName,
            this.LocalInitiative,
            this.Institutionalized,
            this.TaxId,
            this.CoordinatorName,
            this.InstitutionLocation,
            this.Phones,
            this.InstitutionSite,
            this.Email,
            this.InstitutionStatus,
            this.CreationTime,
            this.InactivationTime,
            this.InactivationReason});
            this.tlpMain.SetColumnSpan(this.dgvInstitutions, 2);
            this.dgvInstitutions.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle18.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInstitutions.DefaultCellStyle = dataGridViewCellStyle18;
            resources.ApplyResources(this.dgvInstitutions, "dgvInstitutions");
            this.dgvInstitutions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvInstitutions.Name = "dgvInstitutions";
            this.dgvInstitutions.ReadOnly = true;
            this.dgvInstitutions.RowHeadersVisible = false;
            this.dgvInstitutions.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvInstitutions.RowTemplate.Height = 44;
            this.dgvInstitutions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvInstitutions_MouseUp);
            // 
            // InstitutionId
            // 
            this.InstitutionId.DataPropertyName = "InstitutionId";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.InstitutionId.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.InstitutionId, "InstitutionId");
            this.InstitutionId.Name = "InstitutionId";
            this.InstitutionId.ReadOnly = true;
            // 
            // DisplayName
            // 
            this.DisplayName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DisplayName.DataPropertyName = "ProjectName";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DisplayName.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.DisplayName, "DisplayName");
            this.DisplayName.Name = "DisplayName";
            this.DisplayName.ReadOnly = true;
            // 
            // EntityName
            // 
            this.EntityName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EntityName.DataPropertyName = "EntityName";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.EntityName.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.EntityName, "EntityName");
            this.EntityName.Name = "EntityName";
            this.EntityName.ReadOnly = true;
            // 
            // LocalInitiative
            // 
            this.LocalInitiative.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LocalInitiative.DataPropertyName = "LocalInitiative";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LocalInitiative.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.LocalInitiative, "LocalInitiative");
            this.LocalInitiative.Name = "LocalInitiative";
            this.LocalInitiative.ReadOnly = true;
            // 
            // Institutionalized
            // 
            this.Institutionalized.DataPropertyName = "Institutionalized";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.Institutionalized.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.Institutionalized, "Institutionalized");
            this.Institutionalized.Name = "Institutionalized";
            this.Institutionalized.ReadOnly = true;
            // 
            // TaxId
            // 
            this.TaxId.DataPropertyName = "TaxId";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.TaxId.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.TaxId, "TaxId");
            this.TaxId.Name = "TaxId";
            this.TaxId.ReadOnly = true;
            // 
            // CoordinatorName
            // 
            this.CoordinatorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CoordinatorName.DataPropertyName = "CoordinatorName";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CoordinatorName.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.CoordinatorName, "CoordinatorName");
            this.CoordinatorName.Name = "CoordinatorName";
            this.CoordinatorName.ReadOnly = true;
            // 
            // InstitutionLocation
            // 
            this.InstitutionLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InstitutionLocation.DataPropertyName = "Location";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InstitutionLocation.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.InstitutionLocation, "InstitutionLocation");
            this.InstitutionLocation.Name = "InstitutionLocation";
            this.InstitutionLocation.ReadOnly = true;
            // 
            // Phones
            // 
            this.Phones.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Phones.DataPropertyName = "Phones";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Phones.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.Phones, "Phones");
            this.Phones.Name = "Phones";
            this.Phones.ReadOnly = true;
            // 
            // InstitutionSite
            // 
            this.InstitutionSite.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InstitutionSite.DataPropertyName = "Site";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.InstitutionSite.DefaultCellStyle = dataGridViewCellStyle12;
            resources.ApplyResources(this.InstitutionSite, "InstitutionSite");
            this.InstitutionSite.Name = "InstitutionSite";
            this.InstitutionSite.ReadOnly = true;
            // 
            // Email
            // 
            this.Email.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Email.DataPropertyName = "Email";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Email.DefaultCellStyle = dataGridViewCellStyle13;
            resources.ApplyResources(this.Email, "Email");
            this.Email.Name = "Email";
            this.Email.ReadOnly = true;
            // 
            // InstitutionStatus
            // 
            this.InstitutionStatus.DataPropertyName = "InstitutionStatusName";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.InstitutionStatus.DefaultCellStyle = dataGridViewCellStyle14;
            resources.ApplyResources(this.InstitutionStatus, "InstitutionStatus");
            this.InstitutionStatus.Name = "InstitutionStatus";
            this.InstitutionStatus.ReadOnly = true;
            // 
            // CreationTime
            // 
            this.CreationTime.DataPropertyName = "CreationTime";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle15.Format = "g";
            this.CreationTime.DefaultCellStyle = dataGridViewCellStyle15;
            resources.ApplyResources(this.CreationTime, "CreationTime");
            this.CreationTime.Name = "CreationTime";
            this.CreationTime.ReadOnly = true;
            // 
            // InactivationTime
            // 
            this.InactivationTime.DataPropertyName = "InactivationTime";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle16.Format = "g";
            dataGridViewCellStyle16.NullValue = null;
            this.InactivationTime.DefaultCellStyle = dataGridViewCellStyle16;
            resources.ApplyResources(this.InactivationTime, "InactivationTime");
            this.InactivationTime.Name = "InactivationTime";
            this.InactivationTime.ReadOnly = true;
            // 
            // InactivationReason
            // 
            this.InactivationReason.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InactivationReason.DataPropertyName = "InactivationReason";
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InactivationReason.DefaultCellStyle = dataGridViewCellStyle17;
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
            // mcmInstitution
            // 
            this.mcmInstitution.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewInstitution,
            this.mnuViewCoordinator,
            this.tssSeparatorImpersonate,
            this.mnuImpersonateCoordinator,
            this.tssSeparator,
            this.mnuCopy,
            this.tssSeparator2,
            this.mnuDisplayColumns});
            this.mcmInstitution.Name = "mcmInstitution";
            resources.ApplyResources(this.mcmInstitution, "mcmInstitution");
            // 
            // mnuViewInstitution
            // 
            this.mnuViewInstitution.Name = "mnuViewInstitution";
            resources.ApplyResources(this.mnuViewInstitution, "mnuViewInstitution");
            this.mnuViewInstitution.Click += new System.EventHandler(this.mnuViewInstitution_Click);
            // 
            // mnuViewCoordinator
            // 
            this.mnuViewCoordinator.Name = "mnuViewCoordinator";
            resources.ApplyResources(this.mnuViewCoordinator, "mnuViewCoordinator");
            this.mnuViewCoordinator.Click += new System.EventHandler(this.mnuViewCoordinator_Click);
            // 
            // tssSeparatorImpersonate
            // 
            this.tssSeparatorImpersonate.Name = "tssSeparatorImpersonate";
            resources.ApplyResources(this.tssSeparatorImpersonate, "tssSeparatorImpersonate");
            // 
            // mnuImpersonateCoordinator
            // 
            this.mnuImpersonateCoordinator.Name = "mnuImpersonateCoordinator";
            resources.ApplyResources(this.mnuImpersonateCoordinator, "mnuImpersonateCoordinator");
            this.mnuImpersonateCoordinator.Click += new System.EventHandler(this.mnuImpersonateCoordinator_Click);
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
            // ViewInstitutionControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "ViewInstitutionControl";
            this.Load += new System.EventHandler(this.ViewInstitutionControl_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstitutions)).EndInit();
            this.tlpFilters.ResumeLayout(false);
            this.tlpFilters.PerformLayout();
            this.mcmInstitution.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroLabel mlblItemHeading;
        private MetroFramework.Controls.MetroTile mtlView;
        private MetroFramework.Controls.MetroComboBox mcbStatus;
        private MetroFramework.Controls.MetroLabel mlblStatus;
        private System.Windows.Forms.DataGridView dgvInstitutions;
        private System.Windows.Forms.TableLayoutPanel tlpFilters;
        private MetroFramework.Controls.MetroContextMenu mcmInstitution;
        private System.Windows.Forms.ToolStripMenuItem mnuViewInstitution;
        private System.Windows.Forms.ToolStripMenuItem mnuViewCoordinator;
        private MetroFramework.Controls.MetroLabel mlblItemCount;
        private System.Windows.Forms.ToolStripSeparator tssSeparator;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripSeparator tssSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuDisplayColumns;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstitutionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn EntityName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalInitiative;
        private System.Windows.Forms.DataGridViewTextBoxColumn Institutionalized;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxId;
        private System.Windows.Forms.DataGridViewTextBoxColumn CoordinatorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstitutionLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Phones;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstitutionSite;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstitutionStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn InactivationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn InactivationReason;
        private System.Windows.Forms.ToolStripMenuItem mnuImpersonateCoordinator;
        private System.Windows.Forms.ToolStripSeparator tssSeparatorImpersonate;
    }
}
