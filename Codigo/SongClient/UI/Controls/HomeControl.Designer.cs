namespace PnT.SongClient.UI.Controls
{
    partial class HomeControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.dgvEvents = new System.Windows.Forms.DataGridView();
            this.EventId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mlblWelcome = new MetroFramework.Controls.MetroLabel();
            this.mlblIndicators = new MetroFramework.Controls.MetroLabel();
            this.tlpIndicators = new System.Windows.Forms.TableLayoutPanel();
            this.icIndicator3 = new PnT.SongClient.UI.Controls.IndicatorControl();
            this.icIndicator4 = new PnT.SongClient.UI.Controls.IndicatorControl();
            this.icIndicator6 = new PnT.SongClient.UI.Controls.IndicatorControl();
            this.icIndicator1 = new PnT.SongClient.UI.Controls.IndicatorControl();
            this.icIndicator2 = new PnT.SongClient.UI.Controls.IndicatorControl();
            this.icIndicator5 = new PnT.SongClient.UI.Controls.IndicatorControl();
            this.icIndicator7 = new PnT.SongClient.UI.Controls.IndicatorControl();
            this.icIndicator8 = new PnT.SongClient.UI.Controls.IndicatorControl();
            this.icIndicator9 = new PnT.SongClient.UI.Controls.IndicatorControl();
            this.icIndicator10 = new PnT.SongClient.UI.Controls.IndicatorControl();
            this.mlblEvents = new MetroFramework.Controls.MetroLabel();
            this.mlblNotices = new MetroFramework.Controls.MetroLabel();
            this.dgvNotices = new System.Windows.Forms.DataGridView();
            this.NoticeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoticeText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mlblIndicatorsData = new MetroFramework.Controls.MetroLabel();
            this.timIndicators = new System.Windows.Forms.Timer(this.components);
            this.mcmEvent = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.mnuViewEvent = new System.Windows.Forms.ToolStripMenuItem();
            this.mcmNotice = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.mnuViewReport = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).BeginInit();
            this.tlpIndicators.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotices)).BeginInit();
            this.mcmEvent.SuspendLayout();
            this.mcmNotice.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.dgvEvents, 0, 5);
            this.tlpMain.Controls.Add(this.mlblWelcome, 0, 0);
            this.tlpMain.Controls.Add(this.mlblIndicators, 0, 1);
            this.tlpMain.Controls.Add(this.tlpIndicators, 0, 2);
            this.tlpMain.Controls.Add(this.mlblEvents, 0, 4);
            this.tlpMain.Controls.Add(this.mlblNotices, 1, 4);
            this.tlpMain.Controls.Add(this.dgvNotices, 1, 5);
            this.tlpMain.Controls.Add(this.mlblIndicatorsData, 1, 1);
            this.tlpMain.Name = "tlpMain";
            // 
            // dgvEvents
            // 
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
            this.dgvEvents.ColumnHeadersVisible = false;
            this.dgvEvents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EventId,
            this.EventDate,
            this.EventTime,
            this.EventName});
            this.dgvEvents.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvEvents.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.dgvEvents, "dgvEvents");
            this.dgvEvents.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvEvents.MultiSelect = false;
            this.dgvEvents.Name = "dgvEvents";
            this.dgvEvents.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvEvents.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvEvents.RowHeadersVisible = false;
            this.dgvEvents.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvEvents.RowTemplate.Height = 44;
            this.dgvEvents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEvents.SelectionChanged += new System.EventHandler(this.dgvEvents_SelectionChanged);
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
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
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
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
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
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.EventName.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.EventName, "EventName");
            this.EventName.Name = "EventName";
            this.EventName.ReadOnly = true;
            // 
            // mlblWelcome
            // 
            resources.ApplyResources(this.mlblWelcome, "mlblWelcome");
            this.mlblWelcome.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.mlblWelcome.Name = "mlblWelcome";
            // 
            // mlblIndicators
            // 
            resources.ApplyResources(this.mlblIndicators, "mlblIndicators");
            this.mlblIndicators.Name = "mlblIndicators";
            // 
            // tlpIndicators
            // 
            resources.ApplyResources(this.tlpIndicators, "tlpIndicators");
            this.tlpMain.SetColumnSpan(this.tlpIndicators, 2);
            this.tlpIndicators.Controls.Add(this.icIndicator3, 1, 0);
            this.tlpIndicators.Controls.Add(this.icIndicator4, 1, 1);
            this.tlpIndicators.Controls.Add(this.icIndicator6, 2, 1);
            this.tlpIndicators.Controls.Add(this.icIndicator1, 0, 0);
            this.tlpIndicators.Controls.Add(this.icIndicator2, 0, 1);
            this.tlpIndicators.Controls.Add(this.icIndicator5, 2, 0);
            this.tlpIndicators.Controls.Add(this.icIndicator7, 3, 0);
            this.tlpIndicators.Controls.Add(this.icIndicator8, 3, 1);
            this.tlpIndicators.Controls.Add(this.icIndicator9, 4, 0);
            this.tlpIndicators.Controls.Add(this.icIndicator10, 4, 1);
            this.tlpIndicators.Name = "tlpIndicators";
            // 
            // icIndicator3
            // 
            this.icIndicator3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            resources.ApplyResources(this.icIndicator3, "icIndicator3");
            this.icIndicator3.Name = "icIndicator3";
            // 
            // icIndicator4
            // 
            this.icIndicator4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            resources.ApplyResources(this.icIndicator4, "icIndicator4");
            this.icIndicator4.Name = "icIndicator4";
            // 
            // icIndicator6
            // 
            this.icIndicator6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            resources.ApplyResources(this.icIndicator6, "icIndicator6");
            this.icIndicator6.Name = "icIndicator6";
            // 
            // icIndicator1
            // 
            this.icIndicator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            resources.ApplyResources(this.icIndicator1, "icIndicator1");
            this.icIndicator1.Name = "icIndicator1";
            // 
            // icIndicator2
            // 
            this.icIndicator2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            resources.ApplyResources(this.icIndicator2, "icIndicator2");
            this.icIndicator2.Name = "icIndicator2";
            // 
            // icIndicator5
            // 
            this.icIndicator5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            resources.ApplyResources(this.icIndicator5, "icIndicator5");
            this.icIndicator5.Name = "icIndicator5";
            // 
            // icIndicator7
            // 
            this.icIndicator7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            resources.ApplyResources(this.icIndicator7, "icIndicator7");
            this.icIndicator7.Name = "icIndicator7";
            // 
            // icIndicator8
            // 
            this.icIndicator8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            resources.ApplyResources(this.icIndicator8, "icIndicator8");
            this.icIndicator8.Name = "icIndicator8";
            // 
            // icIndicator9
            // 
            this.icIndicator9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            resources.ApplyResources(this.icIndicator9, "icIndicator9");
            this.icIndicator9.Name = "icIndicator9";
            // 
            // icIndicator10
            // 
            this.icIndicator10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            resources.ApplyResources(this.icIndicator10, "icIndicator10");
            this.icIndicator10.Name = "icIndicator10";
            // 
            // mlblEvents
            // 
            resources.ApplyResources(this.mlblEvents, "mlblEvents");
            this.mlblEvents.Name = "mlblEvents";
            // 
            // mlblNotices
            // 
            resources.ApplyResources(this.mlblNotices, "mlblNotices");
            this.mlblNotices.Name = "mlblNotices";
            // 
            // dgvNotices
            // 
            this.dgvNotices.AllowUserToAddRows = false;
            this.dgvNotices.AllowUserToDeleteRows = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.dgvNotices.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvNotices.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dgvNotices.BackgroundColor = System.Drawing.Color.White;
            this.dgvNotices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvNotices.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvNotices.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvNotices.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNotices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvNotices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNotices.ColumnHeadersVisible = false;
            this.dgvNotices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NoticeId,
            this.NoticeText});
            this.dgvNotices.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle13.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNotices.DefaultCellStyle = dataGridViewCellStyle13;
            resources.ApplyResources(this.dgvNotices, "dgvNotices");
            this.dgvNotices.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvNotices.MultiSelect = false;
            this.dgvNotices.Name = "dgvNotices";
            this.dgvNotices.ReadOnly = true;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNotices.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvNotices.RowHeadersVisible = false;
            this.dgvNotices.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvNotices.RowTemplate.Height = 44;
            this.dgvNotices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNotices.SelectionChanged += new System.EventHandler(this.dgvNotices_SelectionChanged);
            this.dgvNotices.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvNotices_MouseUp);
            // 
            // NoticeId
            // 
            this.NoticeId.DataPropertyName = "NoticeId";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.NoticeId.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.NoticeId, "NoticeId");
            this.NoticeId.Name = "NoticeId";
            this.NoticeId.ReadOnly = true;
            // 
            // NoticeText
            // 
            this.NoticeText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NoticeText.DataPropertyName = "NoticeText";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.NoticeText.DefaultCellStyle = dataGridViewCellStyle12;
            resources.ApplyResources(this.NoticeText, "NoticeText");
            this.NoticeText.Name = "NoticeText";
            this.NoticeText.ReadOnly = true;
            // 
            // mlblIndicatorsData
            // 
            resources.ApplyResources(this.mlblIndicatorsData, "mlblIndicatorsData");
            this.mlblIndicatorsData.Name = "mlblIndicatorsData";
            // 
            // timIndicators
            // 
            this.timIndicators.Tick += new System.EventHandler(this.timIndicators_Tick);
            // 
            // mcmEvent
            // 
            this.mcmEvent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewEvent});
            this.mcmEvent.Name = "mcmEvent";
            resources.ApplyResources(this.mcmEvent, "mcmEvent");
            // 
            // mnuViewEvent
            // 
            this.mnuViewEvent.Name = "mnuViewEvent";
            resources.ApplyResources(this.mnuViewEvent, "mnuViewEvent");
            this.mnuViewEvent.Click += new System.EventHandler(this.mnuViewEvent_Click);
            // 
            // mcmNotice
            // 
            this.mcmNotice.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewReport});
            this.mcmNotice.Name = "mcmNotice";
            resources.ApplyResources(this.mcmNotice, "mcmNotice");
            // 
            // mnuViewReport
            // 
            this.mnuViewReport.Name = "mnuViewReport";
            resources.ApplyResources(this.mnuViewReport, "mnuViewReport");
            this.mnuViewReport.Click += new System.EventHandler(this.mnuViewReport_Click);
            // 
            // HomeControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "HomeControl";
            this.Load += new System.EventHandler(this.HomeControl_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).EndInit();
            this.tlpIndicators.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotices)).EndInit();
            this.mcmEvent.ResumeLayout(false);
            this.mcmNotice.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroLabel mlblWelcome;
        private MetroFramework.Controls.MetroLabel mlblIndicators;
        private System.Windows.Forms.TableLayoutPanel tlpIndicators;
        private IndicatorControl icIndicator3;
        private IndicatorControl icIndicator4;
        private IndicatorControl icIndicator6;
        private IndicatorControl icIndicator1;
        private IndicatorControl icIndicator2;
        private IndicatorControl icIndicator5;
        private IndicatorControl icIndicator7;
        private IndicatorControl icIndicator8;
        private System.Windows.Forms.Timer timIndicators;
        private IndicatorControl icIndicator9;
        private IndicatorControl icIndicator10;
        private MetroFramework.Controls.MetroLabel mlblEvents;
        private System.Windows.Forms.DataGridView dgvEvents;
        private MetroFramework.Controls.MetroContextMenu mcmEvent;
        private System.Windows.Forms.ToolStripMenuItem mnuViewEvent;
        private MetroFramework.Controls.MetroLabel mlblNotices;
        private System.Windows.Forms.DataGridView dgvNotices;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventId;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventName;
        private MetroFramework.Controls.MetroContextMenu mcmNotice;
        private System.Windows.Forms.ToolStripMenuItem mnuViewReport;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoticeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoticeText;
        private MetroFramework.Controls.MetroLabel mlblIndicatorsData;
    }
}
