namespace PnT.SongClient.UI.Controls
{
    partial class ImportClassControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportClassControl));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mlblTitle = new MetroFramework.Controls.MetroLabel();
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.tlpFilters = new System.Windows.Forms.TableLayoutPanel();
            this.mcbSemester = new MetroFramework.Controls.MetroComboBox();
            this.mlblSemester = new MetroFramework.Controls.MetroLabel();
            this.mcbTargetSemester = new MetroFramework.Controls.MetroComboBox();
            this.mlblTargetSemester = new MetroFramework.Controls.MetroLabel();
            this.mlblPole = new MetroFramework.Controls.MetroLabel();
            this.mcbPole = new MetroFramework.Controls.MetroComboBox();
            this.mlblRegistrationOption = new MetroFramework.Controls.MetroLabel();
            this.mcbRegistrationOption = new MetroFramework.Controls.MetroComboBox();
            this.tlpClasses = new System.Windows.Forms.TableLayoutPanel();
            this.mlblAvailableClasses = new MetroFramework.Controls.MetroLabel();
            this.mlblSelectedClasses = new MetroFramework.Controls.MetroLabel();
            this.lbAvailableClasses = new System.Windows.Forms.ListBox();
            this.lbSelectedClasses = new System.Windows.Forms.ListBox();
            this.mbtnAddClasses = new MetroFramework.Controls.MetroButton();
            this.mbtnRemoveClasses = new MetroFramework.Controls.MetroButton();
            this.mbtnAddAllClasses = new MetroFramework.Controls.MetroButton();
            this.mbtnRemoveAllClasses = new MetroFramework.Controls.MetroButton();
            this.mtlImportClasses = new MetroFramework.Controls.MetroTile();
            this.tlpMain.SuspendLayout();
            this.tlpContent.SuspendLayout();
            this.tlpFilters.SuspendLayout();
            this.tlpClasses.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mlblTitle, 0, 0);
            this.tlpMain.Controls.Add(this.tlpContent, 0, 1);
            this.tlpMain.Name = "tlpMain";
            // 
            // mlblTitle
            // 
            resources.ApplyResources(this.mlblTitle, "mlblTitle");
            this.mlblTitle.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.mlblTitle.Name = "mlblTitle";
            // 
            // tlpContent
            // 
            resources.ApplyResources(this.tlpContent, "tlpContent");
            this.tlpMain.SetColumnSpan(this.tlpContent, 2);
            this.tlpContent.Controls.Add(this.tlpFilters, 0, 0);
            this.tlpContent.Controls.Add(this.tlpClasses, 0, 2);
            this.tlpContent.Controls.Add(this.mtlImportClasses, 0, 3);
            this.tlpContent.Name = "tlpContent";
            // 
            // tlpFilters
            // 
            resources.ApplyResources(this.tlpFilters, "tlpFilters");
            this.tlpFilters.Controls.Add(this.mcbSemester, 0, 0);
            this.tlpFilters.Controls.Add(this.mlblSemester, 0, 0);
            this.tlpFilters.Controls.Add(this.mcbTargetSemester, 4, 0);
            this.tlpFilters.Controls.Add(this.mlblTargetSemester, 3, 0);
            this.tlpFilters.Controls.Add(this.mlblPole, 0, 1);
            this.tlpFilters.Controls.Add(this.mcbPole, 1, 1);
            this.tlpFilters.Controls.Add(this.mlblRegistrationOption, 3, 1);
            this.tlpFilters.Controls.Add(this.mcbRegistrationOption, 4, 1);
            this.tlpFilters.Name = "tlpFilters";
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
            // mlblSemester
            // 
            resources.ApplyResources(this.mlblSemester, "mlblSemester");
            this.mlblSemester.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblSemester.Name = "mlblSemester";
            // 
            // mcbTargetSemester
            // 
            resources.ApplyResources(this.mcbTargetSemester, "mcbTargetSemester");
            this.mcbTargetSemester.DropDownWidth = 240;
            this.mcbTargetSemester.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbTargetSemester.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbTargetSemester.FormattingEnabled = true;
            this.mcbTargetSemester.Name = "mcbTargetSemester";
            this.mcbTargetSemester.UseSelectable = true;
            // 
            // mlblTargetSemester
            // 
            resources.ApplyResources(this.mlblTargetSemester, "mlblTargetSemester");
            this.mlblTargetSemester.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblTargetSemester.Name = "mlblTargetSemester";
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
            this.mcbPole.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbPole.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbPole.FormattingEnabled = true;
            this.mcbPole.Name = "mcbPole";
            this.mcbPole.UseSelectable = true;
            this.mcbPole.SelectedIndexChanged += new System.EventHandler(this.mcbSemester_SelectedIndexChanged);
            // 
            // mlblRegistrationOption
            // 
            resources.ApplyResources(this.mlblRegistrationOption, "mlblRegistrationOption");
            this.mlblRegistrationOption.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblRegistrationOption.Name = "mlblRegistrationOption";
            // 
            // mcbRegistrationOption
            // 
            resources.ApplyResources(this.mcbRegistrationOption, "mcbRegistrationOption");
            this.mcbRegistrationOption.DropDownWidth = 240;
            this.mcbRegistrationOption.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbRegistrationOption.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbRegistrationOption.FormattingEnabled = true;
            this.mcbRegistrationOption.Name = "mcbRegistrationOption";
            this.mcbRegistrationOption.UseSelectable = true;
            // 
            // tlpClasses
            // 
            resources.ApplyResources(this.tlpClasses, "tlpClasses");
            this.tlpClasses.Controls.Add(this.mlblAvailableClasses, 0, 0);
            this.tlpClasses.Controls.Add(this.mlblSelectedClasses, 2, 0);
            this.tlpClasses.Controls.Add(this.lbAvailableClasses, 0, 1);
            this.tlpClasses.Controls.Add(this.lbSelectedClasses, 2, 1);
            this.tlpClasses.Controls.Add(this.mbtnAddClasses, 1, 1);
            this.tlpClasses.Controls.Add(this.mbtnRemoveClasses, 1, 2);
            this.tlpClasses.Controls.Add(this.mbtnAddAllClasses, 1, 3);
            this.tlpClasses.Controls.Add(this.mbtnRemoveAllClasses, 1, 4);
            this.tlpClasses.Name = "tlpClasses";
            // 
            // mlblAvailableClasses
            // 
            resources.ApplyResources(this.mlblAvailableClasses, "mlblAvailableClasses");
            this.mlblAvailableClasses.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblAvailableClasses.Name = "mlblAvailableClasses";
            // 
            // mlblSelectedClasses
            // 
            resources.ApplyResources(this.mlblSelectedClasses, "mlblSelectedClasses");
            this.mlblSelectedClasses.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblSelectedClasses.Name = "mlblSelectedClasses";
            // 
            // lbAvailableClasses
            // 
            resources.ApplyResources(this.lbAvailableClasses, "lbAvailableClasses");
            this.lbAvailableClasses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbAvailableClasses.FormattingEnabled = true;
            this.lbAvailableClasses.Name = "lbAvailableClasses";
            this.tlpClasses.SetRowSpan(this.lbAvailableClasses, 4);
            this.lbAvailableClasses.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAvailableClasses.SelectedIndexChanged += new System.EventHandler(this.lbAvailableClasses_SelectedIndexChanged);
            // 
            // lbSelectedClasses
            // 
            resources.ApplyResources(this.lbSelectedClasses, "lbSelectedClasses");
            this.lbSelectedClasses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbSelectedClasses.FormattingEnabled = true;
            this.lbSelectedClasses.Name = "lbSelectedClasses";
            this.tlpClasses.SetRowSpan(this.lbSelectedClasses, 4);
            this.lbSelectedClasses.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbSelectedClasses.SelectedIndexChanged += new System.EventHandler(this.lbSelectedClasses_SelectedIndexChanged);
            // 
            // mbtnAddClasses
            // 
            resources.ApplyResources(this.mbtnAddClasses, "mbtnAddClasses");
            this.mbtnAddClasses.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveRightOne;
            this.mbtnAddClasses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnAddClasses.Name = "mbtnAddClasses";
            this.mbtnAddClasses.UseSelectable = true;
            this.mbtnAddClasses.Click += new System.EventHandler(this.mbtnAddClasses_Click);
            // 
            // mbtnRemoveClasses
            // 
            resources.ApplyResources(this.mbtnRemoveClasses, "mbtnRemoveClasses");
            this.mbtnRemoveClasses.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveLeftOne;
            this.mbtnRemoveClasses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnRemoveClasses.Name = "mbtnRemoveClasses";
            this.mbtnRemoveClasses.UseSelectable = true;
            this.mbtnRemoveClasses.Click += new System.EventHandler(this.mbtnRemoveClasses_Click);
            // 
            // mbtnAddAllClasses
            // 
            resources.ApplyResources(this.mbtnAddAllClasses, "mbtnAddAllClasses");
            this.mbtnAddAllClasses.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveRightAll;
            this.mbtnAddAllClasses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnAddAllClasses.Name = "mbtnAddAllClasses";
            this.mbtnAddAllClasses.UseSelectable = true;
            this.mbtnAddAllClasses.Click += new System.EventHandler(this.mbtnAddAllClasses_Click);
            // 
            // mbtnRemoveAllClasses
            // 
            resources.ApplyResources(this.mbtnRemoveAllClasses, "mbtnRemoveAllClasses");
            this.mbtnRemoveAllClasses.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveLeftAll;
            this.mbtnRemoveAllClasses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnRemoveAllClasses.Name = "mbtnRemoveAllClasses";
            this.mbtnRemoveAllClasses.UseSelectable = true;
            this.mbtnRemoveAllClasses.Click += new System.EventHandler(this.mbtnRemoveAllClasses_Click);
            // 
            // mtlImportClasses
            // 
            resources.ApplyResources(this.mtlImportClasses, "mtlImportClasses");
            this.mtlImportClasses.ActiveControl = null;
            this.mtlImportClasses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlImportClasses.Name = "mtlImportClasses";
            this.mtlImportClasses.Style = MetroFramework.MetroColorStyle.White;
            this.mtlImportClasses.Tag = "Document";
            this.mtlImportClasses.TileImage = global::PnT.SongClient.Properties.Resources.IconClassWhite;
            this.mtlImportClasses.TileImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mtlImportClasses.TileTextFontSize = MetroFramework.MetroTileTextSize.Small;
            this.mtlImportClasses.UseCustomForeColor = true;
            this.mtlImportClasses.UseSelectable = true;
            this.mtlImportClasses.UseTileImage = true;
            this.mtlImportClasses.Click += new System.EventHandler(this.mtlImportClasses_Click);
            // 
            // ImportClassControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "ImportClassControl";
            this.Load += new System.EventHandler(this.ImportClassControl_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tlpContent.ResumeLayout(false);
            this.tlpFilters.ResumeLayout(false);
            this.tlpFilters.PerformLayout();
            this.tlpClasses.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroLabel mlblTitle;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private System.Windows.Forms.TableLayoutPanel tlpFilters;
        private MetroFramework.Controls.MetroComboBox mcbSemester;
        private MetroFramework.Controls.MetroLabel mlblSemester;
        private MetroFramework.Controls.MetroComboBox mcbTargetSemester;
        private MetroFramework.Controls.MetroLabel mlblTargetSemester;
        private System.Windows.Forms.TableLayoutPanel tlpClasses;
        private MetroFramework.Controls.MetroLabel mlblAvailableClasses;
        private MetroFramework.Controls.MetroLabel mlblSelectedClasses;
        private System.Windows.Forms.ListBox lbAvailableClasses;
        private System.Windows.Forms.ListBox lbSelectedClasses;
        private MetroFramework.Controls.MetroButton mbtnAddClasses;
        private MetroFramework.Controls.MetroButton mbtnRemoveClasses;
        private MetroFramework.Controls.MetroButton mbtnAddAllClasses;
        private MetroFramework.Controls.MetroButton mbtnRemoveAllClasses;
        private MetroFramework.Controls.MetroTile mtlImportClasses;
        private MetroFramework.Controls.MetroLabel mlblPole;
        private MetroFramework.Controls.MetroComboBox mcbPole;
        private MetroFramework.Controls.MetroLabel mlblRegistrationOption;
        private MetroFramework.Controls.MetroComboBox mcbRegistrationOption;
    }
}
