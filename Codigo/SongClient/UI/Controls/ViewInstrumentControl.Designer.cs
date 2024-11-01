namespace PnT.SongClient.UI.Controls
{
    partial class ViewInstrumentControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewInstrumentControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mtlView = new MetroFramework.Controls.MetroTile();
            this.mlblItemHeading = new MetroFramework.Controls.MetroLabel();
            this.dgvInstruments = new System.Windows.Forms.DataGridView();
            this.InstrumentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstrumentTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PoleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstrumentLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstrumentStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InactivationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InactivationReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpFilters = new System.Windows.Forms.TableLayoutPanel();
            this.mcbStatus = new MetroFramework.Controls.MetroComboBox();
            this.mlblStatus = new MetroFramework.Controls.MetroLabel();
            this.mlblInstrumentType = new MetroFramework.Controls.MetroLabel();
            this.mcbInstrumentType = new MetroFramework.Controls.MetroComboBox();
            this.mlblPole = new MetroFramework.Controls.MetroLabel();
            this.mcbPole = new MetroFramework.Controls.MetroComboBox();
            this.mlblInstitution = new MetroFramework.Controls.MetroLabel();
            this.mcbInstitution = new MetroFramework.Controls.MetroComboBox();
            this.mlblItemCount = new MetroFramework.Controls.MetroLabel();
            this.mcmInstrument = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.mnuViewInstrument = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewPole = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDisplayColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstruments)).BeginInit();
            this.tlpFilters.SuspendLayout();
            this.mcmInstrument.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mtlView, 1, 0);
            this.tlpMain.Controls.Add(this.mlblItemHeading, 0, 0);
            this.tlpMain.Controls.Add(this.dgvInstruments, 0, 2);
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
            // dgvInstruments
            // 
            this.dgvInstruments.AllowUserToAddRows = false;
            this.dgvInstruments.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.dgvInstruments.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvInstruments.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dgvInstruments.BackgroundColor = System.Drawing.Color.White;
            this.dgvInstruments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvInstruments.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvInstruments.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvInstruments.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInstruments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvInstruments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInstruments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.InstrumentId,
            this.Code,
            this.InstrumentTypeName,
            this.PoleName,
            this.InstrumentLocation,
            this.InstrumentStatus,
            this.CreationTime,
            this.InactivationTime,
            this.InactivationReason});
            this.tlpMain.SetColumnSpan(this.dgvInstruments, 2);
            this.dgvInstruments.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInstruments.DefaultCellStyle = dataGridViewCellStyle12;
            resources.ApplyResources(this.dgvInstruments, "dgvInstruments");
            this.dgvInstruments.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvInstruments.Name = "dgvInstruments";
            this.dgvInstruments.ReadOnly = true;
            this.dgvInstruments.RowHeadersVisible = false;
            this.dgvInstruments.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvInstruments.RowTemplate.Height = 44;
            this.dgvInstruments.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvInstruments_MouseUp);
            // 
            // InstrumentId
            // 
            this.InstrumentId.DataPropertyName = "InstrumentId";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.InstrumentId.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.InstrumentId, "InstrumentId");
            this.InstrumentId.Name = "InstrumentId";
            this.InstrumentId.ReadOnly = true;
            // 
            // Code
            // 
            this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Code.DataPropertyName = "Code";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Code.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.Code, "Code");
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            // 
            // InstrumentTypeName
            // 
            this.InstrumentTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InstrumentTypeName.DataPropertyName = "InstrumentTypeName";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InstrumentTypeName.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.InstrumentTypeName, "InstrumentTypeName");
            this.InstrumentTypeName.Name = "InstrumentTypeName";
            this.InstrumentTypeName.ReadOnly = true;
            // 
            // PoleName
            // 
            this.PoleName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PoleName.DataPropertyName = "PoleName";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PoleName.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.PoleName, "PoleName");
            this.PoleName.Name = "PoleName";
            this.PoleName.ReadOnly = true;
            // 
            // InstrumentLocation
            // 
            this.InstrumentLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InstrumentLocation.DataPropertyName = "Location";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InstrumentLocation.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.InstrumentLocation, "InstrumentLocation");
            this.InstrumentLocation.Name = "InstrumentLocation";
            this.InstrumentLocation.ReadOnly = true;
            // 
            // InstrumentStatus
            // 
            this.InstrumentStatus.DataPropertyName = "InstrumentStatusName";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.InstrumentStatus.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.InstrumentStatus, "InstrumentStatus");
            this.InstrumentStatus.Name = "InstrumentStatus";
            this.InstrumentStatus.ReadOnly = true;
            // 
            // CreationTime
            // 
            this.CreationTime.DataPropertyName = "CreationTime";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle9.Format = "g";
            dataGridViewCellStyle9.NullValue = null;
            this.CreationTime.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.CreationTime, "CreationTime");
            this.CreationTime.Name = "CreationTime";
            this.CreationTime.ReadOnly = true;
            // 
            // InactivationTime
            // 
            this.InactivationTime.DataPropertyName = "InactivationTime";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle10.Format = "g";
            dataGridViewCellStyle10.NullValue = null;
            this.InactivationTime.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.InactivationTime, "InactivationTime");
            this.InactivationTime.Name = "InactivationTime";
            this.InactivationTime.ReadOnly = true;
            // 
            // InactivationReason
            // 
            this.InactivationReason.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InactivationReason.DataPropertyName = "InactivationReason";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InactivationReason.DefaultCellStyle = dataGridViewCellStyle11;
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
            this.tlpFilters.Controls.Add(this.mlblInstrumentType, 3, 0);
            this.tlpFilters.Controls.Add(this.mcbInstrumentType, 4, 0);
            this.tlpFilters.Controls.Add(this.mlblPole, 9, 0);
            this.tlpFilters.Controls.Add(this.mcbPole, 10, 0);
            this.tlpFilters.Controls.Add(this.mlblInstitution, 6, 0);
            this.tlpFilters.Controls.Add(this.mcbInstitution, 7, 0);
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
            // mlblItemCount
            // 
            resources.ApplyResources(this.mlblItemCount, "mlblItemCount");
            this.mlblItemCount.Name = "mlblItemCount";
            // 
            // mcmInstrument
            // 
            this.mcmInstrument.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewInstrument,
            this.mnuViewPole,
            this.tssSeparator,
            this.mnuCopy,
            this.tssSeparator2,
            this.mnuDisplayColumns});
            this.mcmInstrument.Name = "mcmInstrument";
            resources.ApplyResources(this.mcmInstrument, "mcmInstrument");
            // 
            // mnuViewInstrument
            // 
            this.mnuViewInstrument.Name = "mnuViewInstrument";
            resources.ApplyResources(this.mnuViewInstrument, "mnuViewInstrument");
            this.mnuViewInstrument.Click += new System.EventHandler(this.mnuViewInstrument_Click);
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
            // ViewInstrumentControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "ViewInstrumentControl";
            this.Load += new System.EventHandler(this.ViewInstrumentControl_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstruments)).EndInit();
            this.tlpFilters.ResumeLayout(false);
            this.tlpFilters.PerformLayout();
            this.mcmInstrument.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroTile mtlView;
        private MetroFramework.Controls.MetroLabel mlblItemHeading;
        private System.Windows.Forms.DataGridView dgvInstruments;
        private System.Windows.Forms.TableLayoutPanel tlpFilters;
        private MetroFramework.Controls.MetroComboBox mcbStatus;
        private MetroFramework.Controls.MetroLabel mlblStatus;
        private MetroFramework.Controls.MetroLabel mlblPole;
        private MetroFramework.Controls.MetroComboBox mcbPole;
        private MetroFramework.Controls.MetroLabel mlblInstrumentType;
        private MetroFramework.Controls.MetroComboBox mcbInstrumentType;
        private MetroFramework.Controls.MetroLabel mlblItemCount;
        private MetroFramework.Controls.MetroContextMenu mcmInstrument;
        private System.Windows.Forms.ToolStripMenuItem mnuViewInstrument;
        private System.Windows.Forms.ToolStripMenuItem mnuViewPole;
        private System.Windows.Forms.ToolStripSeparator tssSeparator;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private MetroFramework.Controls.MetroLabel mlblInstitution;
        private MetroFramework.Controls.MetroComboBox mcbInstitution;
        private System.Windows.Forms.ToolStripSeparator tssSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuDisplayColumns;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstrumentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstrumentTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PoleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstrumentLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstrumentStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn InactivationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn InactivationReason;
    }
}
