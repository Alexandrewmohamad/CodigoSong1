namespace PnT.SongClient.UI.Controls
{
    partial class ViewEventControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewEventControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.mlblItemCount = new MetroFramework.Controls.MetroLabel();
            this.mtlView = new MetroFramework.Controls.MetroTile();
            this.mlblItemHeading = new MetroFramework.Controls.MetroLabel();
            this.dgvEvents = new System.Windows.Forms.DataGridView();
            this.EventId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstitutionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventSendOptionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpCalendar = new System.Windows.Forms.TableLayoutPanel();
            this.mcCalendar = new PnT.SongClient.UI.Controls.Calendar();
            this.mtlNextMonth = new MetroFramework.Controls.MetroTile();
            this.mtlPreviousMonth = new MetroFramework.Controls.MetroTile();
            this.mcmEvent = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.mnuViewEvent = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDisplayColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).BeginInit();
            this.tlpCalendar.SuspendLayout();
            this.mcmEvent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mlblItemCount, 0, 3);
            this.tlpMain.Controls.Add(this.mtlView, 1, 0);
            this.tlpMain.Controls.Add(this.mlblItemHeading, 0, 0);
            this.tlpMain.Controls.Add(this.dgvEvents, 0, 2);
            this.tlpMain.Controls.Add(this.tlpCalendar, 0, 1);
            this.tlpMain.Name = "tlpMain";
            // 
            // mlblItemCount
            // 
            resources.ApplyResources(this.mlblItemCount, "mlblItemCount");
            this.mlblItemCount.Name = "mlblItemCount";
            // 
            // mtlView
            // 
            resources.ApplyResources(this.mtlView, "mtlView");
            this.mtlView.ActiveControl = null;
            this.mtlView.Cursor = System.Windows.Forms.Cursors.Hand;
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
            // dgvEvents
            // 
            resources.ApplyResources(this.dgvEvents, "dgvEvents");
            this.dgvEvents.AllowUserToAddRows = false;
            this.dgvEvents.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.dgvEvents.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvEvents.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dgvEvents.BackgroundColor = System.Drawing.Color.White;
            this.dgvEvents.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvEvents.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvEvents.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvEvents.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvEvents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEvents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EventId,
            this.EventDate,
            this.EventTime,
            this.EventName,
            this.InstitutionName,
            this.EventLocation,
            this.Duration,
            this.Description,
            this.EventSendOptionName,
            this.CreationTime});
            this.tlpMain.SetColumnSpan(this.dgvEvents, 2);
            this.dgvEvents.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle13.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvEvents.DefaultCellStyle = dataGridViewCellStyle13;
            this.dgvEvents.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvEvents.Name = "dgvEvents";
            this.dgvEvents.ReadOnly = true;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvEvents.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvEvents.RowHeadersVisible = false;
            this.dgvEvents.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvEvents.RowTemplate.Height = 44;
            this.dgvEvents.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvEvents_MouseUp);
            // 
            // EventId
            // 
            this.EventId.DataPropertyName = "EventId";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.EventId.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.EventId, "EventId");
            this.EventId.Name = "EventId";
            this.EventId.ReadOnly = true;
            // 
            // EventDate
            // 
            this.EventDate.DataPropertyName = "EventDate";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle4.Format = "d";
            dataGridViewCellStyle4.NullValue = null;
            this.EventDate.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.EventDate, "EventDate");
            this.EventDate.Name = "EventDate";
            this.EventDate.ReadOnly = true;
            // 
            // EventTime
            // 
            this.EventTime.DataPropertyName = "EventTime";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle5.Format = "t";
            dataGridViewCellStyle5.NullValue = null;
            this.EventTime.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.EventTime, "EventTime");
            this.EventTime.Name = "EventTime";
            this.EventTime.ReadOnly = true;
            // 
            // EventName
            // 
            this.EventName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EventName.DataPropertyName = "EventName";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.EventName.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.EventName, "EventName");
            this.EventName.Name = "EventName";
            this.EventName.ReadOnly = true;
            // 
            // InstitutionName
            // 
            this.InstitutionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InstitutionName.DataPropertyName = "InstitutionName";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InstitutionName.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.InstitutionName, "InstitutionName");
            this.InstitutionName.Name = "InstitutionName";
            this.InstitutionName.ReadOnly = true;
            // 
            // EventLocation
            // 
            this.EventLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EventLocation.DataPropertyName = "EventLocation";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.EventLocation.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.EventLocation, "EventLocation");
            this.EventLocation.Name = "EventLocation";
            this.EventLocation.ReadOnly = true;
            // 
            // Duration
            // 
            this.Duration.DataPropertyName = "Duration";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.Duration.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.Duration, "Duration");
            this.Duration.Name = "Duration";
            this.Duration.ReadOnly = true;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.DataPropertyName = "Description";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Description.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.Description, "Description");
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // EventSendOptionName
            // 
            this.EventSendOptionName.DataPropertyName = "EventSendOptionName";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.EventSendOptionName.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.EventSendOptionName, "EventSendOptionName");
            this.EventSendOptionName.Name = "EventSendOptionName";
            this.EventSendOptionName.ReadOnly = true;
            // 
            // CreationTime
            // 
            this.CreationTime.DataPropertyName = "CreationTime";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle12.Format = "g";
            this.CreationTime.DefaultCellStyle = dataGridViewCellStyle12;
            resources.ApplyResources(this.CreationTime, "CreationTime");
            this.CreationTime.Name = "CreationTime";
            this.CreationTime.ReadOnly = true;
            // 
            // tlpCalendar
            // 
            resources.ApplyResources(this.tlpCalendar, "tlpCalendar");
            this.tlpMain.SetColumnSpan(this.tlpCalendar, 2);
            this.tlpCalendar.Controls.Add(this.mcCalendar, 2, 0);
            this.tlpCalendar.Controls.Add(this.mtlNextMonth, 3, 0);
            this.tlpCalendar.Controls.Add(this.mtlPreviousMonth, 1, 0);
            this.tlpCalendar.Name = "tlpCalendar";
            // 
            // mcCalendar
            // 
            this.mcCalendar.AbbreviateWeekDayHeader = true;
            resources.ApplyResources(this.mcCalendar, "mcCalendar");
            this.mcCalendar.ActiveMonthColor = System.Drawing.Color.White;
            this.mcCalendar.ApptFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.mcCalendar.BackColor = System.Drawing.Color.White;
            this.mcCalendar.BackgroundColor = System.Drawing.Color.White;
            this.mcCalendar.BoldedDateFontColor = System.Drawing.Color.Black;
            this.mcCalendar.BoldedDates = null;
            this.mcCalendar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mcCalendar.DisplayWeekendsDarker = false;
            this.mcCalendar.ForeColor = System.Drawing.Color.Black;
            this.mcCalendar.GridColor = System.Drawing.Color.Gray;
            this.mcCalendar.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(174)))), ((int)(((byte)(216)))));
            this.mcCalendar.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.mcCalendar.InactiveMonthColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(214)))));
            this.mcCalendar.intDay = 8;
            this.mcCalendar.intMonth = 8;
            this.mcCalendar.intYear = 2018;
            this.mcCalendar.Name = "mcCalendar";
            this.mcCalendar.NoApptFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.mcCalendar.NonselectedDayFontColor = System.Drawing.Color.DimGray;
            this.mcCalendar.SelectedDate = new System.DateTime(2018, 8, 8, 0, 0, 0, 0);
            this.mcCalendar.SelectedDayColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(174)))), ((int)(((byte)(216)))));
            this.mcCalendar.SelectedDayFontColor = System.Drawing.Color.White;
            this.mcCalendar.ShowCurrentMonthInDay = false;
            this.mcCalendar.ShowGrid = false;
            this.mcCalendar.ShowPrevNextButton = false;
            this.mcCalendar.SelectedDateChanged += new PnT.SongClient.UI.Controls.SelectedDateChangedEventHandler(this.mcCalendar_SelectedDateChanged);
            // 
            // mtlNextMonth
            // 
            resources.ApplyResources(this.mtlNextMonth, "mtlNextMonth");
            this.mtlNextMonth.ActiveControl = null;
            this.mtlNextMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlNextMonth.Name = "mtlNextMonth";
            this.mtlNextMonth.TileImage = global::PnT.SongClient.Properties.Resources.IconSendDRightOne;
            this.mtlNextMonth.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mtlNextMonth.TileTextFontSize = MetroFramework.MetroTileTextSize.Small;
            this.mtlNextMonth.UseCustomBackColor = true;
            this.mtlNextMonth.UseCustomForeColor = true;
            this.mtlNextMonth.UseSelectable = true;
            this.mtlNextMonth.UseTileImage = true;
            this.mtlNextMonth.Click += new System.EventHandler(this.mtlNextMonth_Click);
            // 
            // mtlPreviousMonth
            // 
            resources.ApplyResources(this.mtlPreviousMonth, "mtlPreviousMonth");
            this.mtlPreviousMonth.ActiveControl = null;
            this.mtlPreviousMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlPreviousMonth.Name = "mtlPreviousMonth";
            this.mtlPreviousMonth.TileImage = global::PnT.SongClient.Properties.Resources.IconSendDLeftOne;
            this.mtlPreviousMonth.TileImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mtlPreviousMonth.TileTextFontSize = MetroFramework.MetroTileTextSize.Small;
            this.mtlPreviousMonth.UseCustomBackColor = true;
            this.mtlPreviousMonth.UseCustomForeColor = true;
            this.mtlPreviousMonth.UseSelectable = true;
            this.mtlPreviousMonth.UseTileImage = true;
            this.mtlPreviousMonth.Click += new System.EventHandler(this.mtlPreviousMonth_Click);
            // 
            // mcmEvent
            // 
            resources.ApplyResources(this.mcmEvent, "mcmEvent");
            this.mcmEvent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewEvent,
            this.tssSeparator,
            this.mnuCopy,
            this.tssSeparator2,
            this.mnuDisplayColumns});
            this.mcmEvent.Name = "mcmInstitution";
            // 
            // mnuViewEvent
            // 
            resources.ApplyResources(this.mnuViewEvent, "mnuViewEvent");
            this.mnuViewEvent.Name = "mnuViewEvent";
            this.mnuViewEvent.Click += new System.EventHandler(this.mnuViewEvent_Click);
            // 
            // tssSeparator
            // 
            resources.ApplyResources(this.tssSeparator, "tssSeparator");
            this.tssSeparator.Name = "tssSeparator";
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
            // ViewEventControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "ViewEventControl";
            this.Load += new System.EventHandler(this.ViewEventControl_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).EndInit();
            this.tlpCalendar.ResumeLayout(false);
            this.mcmEvent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroLabel mlblItemHeading;
        private System.Windows.Forms.DataGridView dgvEvents;
        private MetroFramework.Controls.MetroContextMenu mcmEvent;
        private System.Windows.Forms.ToolStripMenuItem mnuViewEvent;
        private MetroFramework.Controls.MetroLabel mlblItemCount;
        private System.Windows.Forms.ToolStripSeparator tssSeparator;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripSeparator tssSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuDisplayColumns;
        private MetroFramework.Controls.MetroTile mtlView;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventId;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventName;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstitutionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Duration;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventSendOptionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationTime;
        private Calendar mcCalendar;
        private System.Windows.Forms.TableLayoutPanel tlpCalendar;
        private MetroFramework.Controls.MetroTile mtlNextMonth;
        private MetroFramework.Controls.MetroTile mtlPreviousMonth;
    }
}
