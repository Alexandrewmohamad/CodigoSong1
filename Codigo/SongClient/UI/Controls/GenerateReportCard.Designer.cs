namespace PnT.SongClient.UI.Controls
{
    partial class GenerateReportCard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateReportCard));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mlblTitle = new MetroFramework.Controls.MetroLabel();
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.tlpFilters = new System.Windows.Forms.TableLayoutPanel();
            this.mcbSemester = new MetroFramework.Controls.MetroComboBox();
            this.mlblSemester = new MetroFramework.Controls.MetroLabel();
            this.mcbPole = new MetroFramework.Controls.MetroComboBox();
            this.mlblPole = new MetroFramework.Controls.MetroLabel();
            this.tlpPermissions = new System.Windows.Forms.TableLayoutPanel();
            this.mlblAvailableStudents = new MetroFramework.Controls.MetroLabel();
            this.mlblSelectedPermissions = new MetroFramework.Controls.MetroLabel();
            this.lbAvailableStudents = new System.Windows.Forms.ListBox();
            this.lbSelectedStudents = new System.Windows.Forms.ListBox();
            this.mbtnAddStudents = new MetroFramework.Controls.MetroButton();
            this.mbtnRemoveStudents = new MetroFramework.Controls.MetroButton();
            this.mbtnAddAllStudents = new MetroFramework.Controls.MetroButton();
            this.mbtnRemoveAllStudents = new MetroFramework.Controls.MetroButton();
            this.mtlReportCards = new MetroFramework.Controls.MetroTile();
            this.sfdReportCardsFile = new System.Windows.Forms.SaveFileDialog();
            this.tlpMain.SuspendLayout();
            this.tlpContent.SuspendLayout();
            this.tlpFilters.SuspendLayout();
            this.tlpPermissions.SuspendLayout();
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
            this.tlpContent.Controls.Add(this.tlpPermissions, 0, 2);
            this.tlpContent.Controls.Add(this.mtlReportCards, 0, 3);
            this.tlpContent.Name = "tlpContent";
            // 
            // tlpFilters
            // 
            resources.ApplyResources(this.tlpFilters, "tlpFilters");
            this.tlpFilters.Controls.Add(this.mcbSemester, 0, 0);
            this.tlpFilters.Controls.Add(this.mlblSemester, 0, 0);
            this.tlpFilters.Controls.Add(this.mcbPole, 4, 0);
            this.tlpFilters.Controls.Add(this.mlblPole, 3, 0);
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
            // mlblPole
            // 
            resources.ApplyResources(this.mlblPole, "mlblPole");
            this.mlblPole.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblPole.Name = "mlblPole";
            // 
            // tlpPermissions
            // 
            resources.ApplyResources(this.tlpPermissions, "tlpPermissions");
            this.tlpPermissions.Controls.Add(this.mlblAvailableStudents, 0, 0);
            this.tlpPermissions.Controls.Add(this.mlblSelectedPermissions, 2, 0);
            this.tlpPermissions.Controls.Add(this.lbAvailableStudents, 0, 1);
            this.tlpPermissions.Controls.Add(this.lbSelectedStudents, 2, 1);
            this.tlpPermissions.Controls.Add(this.mbtnAddStudents, 1, 1);
            this.tlpPermissions.Controls.Add(this.mbtnRemoveStudents, 1, 2);
            this.tlpPermissions.Controls.Add(this.mbtnAddAllStudents, 1, 3);
            this.tlpPermissions.Controls.Add(this.mbtnRemoveAllStudents, 1, 4);
            this.tlpPermissions.Name = "tlpPermissions";
            // 
            // mlblAvailableStudents
            // 
            resources.ApplyResources(this.mlblAvailableStudents, "mlblAvailableStudents");
            this.mlblAvailableStudents.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblAvailableStudents.Name = "mlblAvailableStudents";
            // 
            // mlblSelectedPermissions
            // 
            resources.ApplyResources(this.mlblSelectedPermissions, "mlblSelectedPermissions");
            this.mlblSelectedPermissions.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblSelectedPermissions.Name = "mlblSelectedPermissions";
            // 
            // lbAvailableStudents
            // 
            this.lbAvailableStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.lbAvailableStudents, "lbAvailableStudents");
            this.lbAvailableStudents.FormattingEnabled = true;
            this.lbAvailableStudents.Name = "lbAvailableStudents";
            this.tlpPermissions.SetRowSpan(this.lbAvailableStudents, 4);
            this.lbAvailableStudents.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAvailableStudents.SelectedIndexChanged += new System.EventHandler(this.lbAvailableStudents_SelectedIndexChanged);
            // 
            // lbSelectedStudents
            // 
            this.lbSelectedStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.lbSelectedStudents, "lbSelectedStudents");
            this.lbSelectedStudents.FormattingEnabled = true;
            this.lbSelectedStudents.Name = "lbSelectedStudents";
            this.tlpPermissions.SetRowSpan(this.lbSelectedStudents, 4);
            this.lbSelectedStudents.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbSelectedStudents.SelectedIndexChanged += new System.EventHandler(this.lbSelectedStudents_SelectedIndexChanged);
            // 
            // mbtnAddStudents
            // 
            resources.ApplyResources(this.mbtnAddStudents, "mbtnAddStudents");
            this.mbtnAddStudents.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveRightOne;
            this.mbtnAddStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnAddStudents.Name = "mbtnAddStudents";
            this.mbtnAddStudents.UseSelectable = true;
            this.mbtnAddStudents.Click += new System.EventHandler(this.mbtnAddStudents_Click);
            // 
            // mbtnRemoveStudents
            // 
            resources.ApplyResources(this.mbtnRemoveStudents, "mbtnRemoveStudents");
            this.mbtnRemoveStudents.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveLeftOne;
            this.mbtnRemoveStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnRemoveStudents.Name = "mbtnRemoveStudents";
            this.mbtnRemoveStudents.UseSelectable = true;
            this.mbtnRemoveStudents.Click += new System.EventHandler(this.mbtnRemoveStudents_Click);
            // 
            // mbtnAddAllStudents
            // 
            resources.ApplyResources(this.mbtnAddAllStudents, "mbtnAddAllStudents");
            this.mbtnAddAllStudents.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveRightAll;
            this.mbtnAddAllStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnAddAllStudents.Name = "mbtnAddAllStudents";
            this.mbtnAddAllStudents.UseSelectable = true;
            this.mbtnAddAllStudents.Click += new System.EventHandler(this.mbtnAddAllStudents_Click);
            // 
            // mbtnRemoveAllStudents
            // 
            resources.ApplyResources(this.mbtnRemoveAllStudents, "mbtnRemoveAllStudents");
            this.mbtnRemoveAllStudents.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveLeftAll;
            this.mbtnRemoveAllStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnRemoveAllStudents.Name = "mbtnRemoveAllStudents";
            this.mbtnRemoveAllStudents.UseSelectable = true;
            this.mbtnRemoveAllStudents.Click += new System.EventHandler(this.mbtnRemoveAllStudents_Click);
            // 
            // mtlReportCards
            // 
            this.mtlReportCards.ActiveControl = null;
            resources.ApplyResources(this.mtlReportCards, "mtlReportCards");
            this.mtlReportCards.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtlReportCards.Name = "mtlReportCards";
            this.mtlReportCards.Style = MetroFramework.MetroColorStyle.White;
            this.mtlReportCards.Tag = "Document";
            this.mtlReportCards.TileImage = global::PnT.SongClient.Properties.Resources.IconDocumentWhite;
            this.mtlReportCards.TileImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mtlReportCards.TileTextFontSize = MetroFramework.MetroTileTextSize.Small;
            this.mtlReportCards.UseCustomForeColor = true;
            this.mtlReportCards.UseSelectable = true;
            this.mtlReportCards.UseTileImage = true;
            this.mtlReportCards.Click += new System.EventHandler(this.mtlReportCards_Click);
            // 
            // sfdReportCardsFile
            // 
            this.sfdReportCardsFile.DefaultExt = "html";
            resources.ApplyResources(this.sfdReportCardsFile, "sfdReportCardsFile");
            // 
            // GenerateReportCard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "GenerateReportCard";
            this.Load += new System.EventHandler(this.GenerateReportCard_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tlpContent.ResumeLayout(false);
            this.tlpFilters.ResumeLayout(false);
            this.tlpFilters.PerformLayout();
            this.tlpPermissions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroLabel mlblTitle;
        private System.Windows.Forms.TableLayoutPanel tlpFilters;
        private MetroFramework.Controls.MetroComboBox mcbPole;
        private MetroFramework.Controls.MetroLabel mlblPole;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private System.Windows.Forms.TableLayoutPanel tlpPermissions;
        private MetroFramework.Controls.MetroLabel mlblAvailableStudents;
        private MetroFramework.Controls.MetroLabel mlblSelectedPermissions;
        private System.Windows.Forms.ListBox lbAvailableStudents;
        private System.Windows.Forms.ListBox lbSelectedStudents;
        private MetroFramework.Controls.MetroButton mbtnAddStudents;
        private MetroFramework.Controls.MetroButton mbtnRemoveStudents;
        private MetroFramework.Controls.MetroButton mbtnAddAllStudents;
        private MetroFramework.Controls.MetroButton mbtnRemoveAllStudents;
        private MetroFramework.Controls.MetroLabel mlblSemester;
        private MetroFramework.Controls.MetroComboBox mcbSemester;
        private MetroFramework.Controls.MetroTile mtlReportCards;
        private System.Windows.Forms.SaveFileDialog sfdReportCardsFile;
    }
}
