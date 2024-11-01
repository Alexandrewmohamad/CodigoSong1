namespace PnT.SongClient.UI.Controls
{
    partial class RegisterRoleControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterRoleControl));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnContent = new System.Windows.Forms.Panel();
            this.mtxtDescription = new MetroFramework.Controls.MetroTextBox();
            this.mlblDescription = new MetroFramework.Controls.MetroLabel();
            this.mtxtName = new MetroFramework.Controls.MetroTextBox();
            this.mlblName = new MetroFramework.Controls.MetroLabel();
            this.tlpUsers = new System.Windows.Forms.TableLayoutPanel();
            this.mlblAvailableUsers = new MetroFramework.Controls.MetroLabel();
            this.mlblSelectedUsers = new MetroFramework.Controls.MetroLabel();
            this.lbAvailableUsers = new System.Windows.Forms.ListBox();
            this.lbSelectedUsers = new System.Windows.Forms.ListBox();
            this.mbtnAddUsers = new MetroFramework.Controls.MetroButton();
            this.mbtnRemoveUsers = new MetroFramework.Controls.MetroButton();
            this.tlpPermissions = new System.Windows.Forms.TableLayoutPanel();
            this.mlblAvailablePermissions = new MetroFramework.Controls.MetroLabel();
            this.mlblSelectedPermissions = new MetroFramework.Controls.MetroLabel();
            this.lbAvailablePermissions = new System.Windows.Forms.ListBox();
            this.lbSelectedPermissions = new System.Windows.Forms.ListBox();
            this.mbtnAddPermissions = new MetroFramework.Controls.MetroButton();
            this.mbtnRemovePermissions = new MetroFramework.Controls.MetroButton();
            this.mbtnAddAllPermissions = new MetroFramework.Controls.MetroButton();
            this.mbtnRemoveAllPermissions = new MetroFramework.Controls.MetroButton();
            this.tlpMain.SuspendLayout();
            this.pnContent.SuspendLayout();
            this.tlpUsers.SuspendLayout();
            this.tlpPermissions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.BackColor = System.Drawing.Color.White;
            this.tlpMain.Controls.Add(this.pnContent, 2, 0);
            this.tlpMain.Controls.Add(this.tlpUsers, 2, 2);
            this.tlpMain.Controls.Add(this.tlpPermissions, 2, 1);
            this.tlpMain.Name = "tlpMain";
            // 
            // pnContent
            // 
            resources.ApplyResources(this.pnContent, "pnContent");
            this.pnContent.BackColor = System.Drawing.Color.White;
            this.pnContent.Controls.Add(this.mtxtDescription);
            this.pnContent.Controls.Add(this.mlblDescription);
            this.pnContent.Controls.Add(this.mtxtName);
            this.pnContent.Controls.Add(this.mlblName);
            this.pnContent.Name = "pnContent";
            // 
            // mtxtDescription
            // 
            resources.ApplyResources(this.mtxtDescription, "mtxtDescription");
            // 
            // 
            // 
            this.mtxtDescription.CustomButton.AccessibleDescription = resources.GetString("resource.AccessibleDescription");
            this.mtxtDescription.CustomButton.AccessibleName = resources.GetString("resource.AccessibleName");
            this.mtxtDescription.CustomButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("resource.Anchor")));
            this.mtxtDescription.CustomButton.AutoSize = ((bool)(resources.GetObject("resource.AutoSize")));
            this.mtxtDescription.CustomButton.AutoSizeMode = ((System.Windows.Forms.AutoSizeMode)(resources.GetObject("resource.AutoSizeMode")));
            this.mtxtDescription.CustomButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resource.BackgroundImage")));
            this.mtxtDescription.CustomButton.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("resource.BackgroundImageLayout")));
            this.mtxtDescription.CustomButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("resource.Dock")));
            this.mtxtDescription.CustomButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("resource.FlatStyle")));
            this.mtxtDescription.CustomButton.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font")));
            this.mtxtDescription.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.mtxtDescription.CustomButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.ImageAlign")));
            this.mtxtDescription.CustomButton.ImageIndex = ((int)(resources.GetObject("resource.ImageIndex")));
            this.mtxtDescription.CustomButton.ImageKey = resources.GetString("resource.ImageKey");
            this.mtxtDescription.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode")));
            this.mtxtDescription.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location")));
            this.mtxtDescription.CustomButton.MaximumSize = ((System.Drawing.Size)(resources.GetObject("resource.MaximumSize")));
            this.mtxtDescription.CustomButton.Name = "";
            this.mtxtDescription.CustomButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("resource.RightToLeft")));
            this.mtxtDescription.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size")));
            this.mtxtDescription.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtDescription.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex")));
            this.mtxtDescription.CustomButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.TextAlign")));
            this.mtxtDescription.CustomButton.TextImageRelation = ((System.Windows.Forms.TextImageRelation)(resources.GetObject("resource.TextImageRelation")));
            this.mtxtDescription.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtDescription.CustomButton.UseSelectable = true;
            this.mtxtDescription.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible")));
            this.mtxtDescription.Lines = new string[0];
            this.mtxtDescription.MaxLength = 32767;
            this.mtxtDescription.Name = "mtxtDescription";
            this.mtxtDescription.PasswordChar = '\0';
            this.mtxtDescription.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtDescription.SelectedText = "";
            this.mtxtDescription.SelectionLength = 0;
            this.mtxtDescription.SelectionStart = 0;
            this.mtxtDescription.ShortcutsEnabled = true;
            this.mtxtDescription.UseSelectable = true;
            this.mtxtDescription.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtDescription.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblDescription
            // 
            resources.ApplyResources(this.mlblDescription, "mlblDescription");
            this.mlblDescription.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblDescription.Name = "mlblDescription";
            // 
            // mtxtName
            // 
            resources.ApplyResources(this.mtxtName, "mtxtName");
            // 
            // 
            // 
            this.mtxtName.CustomButton.AccessibleDescription = resources.GetString("resource.AccessibleDescription1");
            this.mtxtName.CustomButton.AccessibleName = resources.GetString("resource.AccessibleName1");
            this.mtxtName.CustomButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("resource.Anchor1")));
            this.mtxtName.CustomButton.AutoSize = ((bool)(resources.GetObject("resource.AutoSize1")));
            this.mtxtName.CustomButton.AutoSizeMode = ((System.Windows.Forms.AutoSizeMode)(resources.GetObject("resource.AutoSizeMode1")));
            this.mtxtName.CustomButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resource.BackgroundImage1")));
            this.mtxtName.CustomButton.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("resource.BackgroundImageLayout1")));
            this.mtxtName.CustomButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("resource.Dock1")));
            this.mtxtName.CustomButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("resource.FlatStyle1")));
            this.mtxtName.CustomButton.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font1")));
            this.mtxtName.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            this.mtxtName.CustomButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.ImageAlign1")));
            this.mtxtName.CustomButton.ImageIndex = ((int)(resources.GetObject("resource.ImageIndex1")));
            this.mtxtName.CustomButton.ImageKey = resources.GetString("resource.ImageKey1");
            this.mtxtName.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode1")));
            this.mtxtName.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location1")));
            this.mtxtName.CustomButton.MaximumSize = ((System.Drawing.Size)(resources.GetObject("resource.MaximumSize1")));
            this.mtxtName.CustomButton.Name = "";
            this.mtxtName.CustomButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("resource.RightToLeft1")));
            this.mtxtName.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size1")));
            this.mtxtName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtName.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex1")));
            this.mtxtName.CustomButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.TextAlign1")));
            this.mtxtName.CustomButton.TextImageRelation = ((System.Windows.Forms.TextImageRelation)(resources.GetObject("resource.TextImageRelation1")));
            this.mtxtName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtName.CustomButton.UseSelectable = true;
            this.mtxtName.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible1")));
            this.mtxtName.Lines = new string[0];
            this.mtxtName.MaxLength = 32767;
            this.mtxtName.Name = "mtxtName";
            this.mtxtName.PasswordChar = '\0';
            this.mtxtName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtName.SelectedText = "";
            this.mtxtName.SelectionLength = 0;
            this.mtxtName.SelectionStart = 0;
            this.mtxtName.ShortcutsEnabled = true;
            this.mtxtName.UseSelectable = true;
            this.mtxtName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblName
            // 
            resources.ApplyResources(this.mlblName, "mlblName");
            this.mlblName.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblName.Name = "mlblName";
            // 
            // tlpUsers
            // 
            resources.ApplyResources(this.tlpUsers, "tlpUsers");
            this.tlpUsers.Controls.Add(this.mlblAvailableUsers, 0, 0);
            this.tlpUsers.Controls.Add(this.mlblSelectedUsers, 2, 0);
            this.tlpUsers.Controls.Add(this.lbAvailableUsers, 0, 1);
            this.tlpUsers.Controls.Add(this.lbSelectedUsers, 2, 1);
            this.tlpUsers.Controls.Add(this.mbtnAddUsers, 1, 1);
            this.tlpUsers.Controls.Add(this.mbtnRemoveUsers, 1, 2);
            this.tlpUsers.Name = "tlpUsers";
            // 
            // mlblAvailableUsers
            // 
            resources.ApplyResources(this.mlblAvailableUsers, "mlblAvailableUsers");
            this.mlblAvailableUsers.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblAvailableUsers.Name = "mlblAvailableUsers";
            // 
            // mlblSelectedUsers
            // 
            resources.ApplyResources(this.mlblSelectedUsers, "mlblSelectedUsers");
            this.mlblSelectedUsers.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblSelectedUsers.Name = "mlblSelectedUsers";
            // 
            // lbAvailableUsers
            // 
            resources.ApplyResources(this.lbAvailableUsers, "lbAvailableUsers");
            this.lbAvailableUsers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbAvailableUsers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbAvailableUsers.FormattingEnabled = true;
            this.lbAvailableUsers.Name = "lbAvailableUsers";
            this.tlpUsers.SetRowSpan(this.lbAvailableUsers, 4);
            this.lbAvailableUsers.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbUsers_DrawItem);
            this.lbAvailableUsers.SelectedIndexChanged += new System.EventHandler(this.lbAvailableUsers_SelectedIndexChanged);
            // 
            // lbSelectedUsers
            // 
            resources.ApplyResources(this.lbSelectedUsers, "lbSelectedUsers");
            this.lbSelectedUsers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbSelectedUsers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbSelectedUsers.FormattingEnabled = true;
            this.lbSelectedUsers.Name = "lbSelectedUsers";
            this.tlpUsers.SetRowSpan(this.lbSelectedUsers, 4);
            this.lbSelectedUsers.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbUsers_DrawItem);
            this.lbSelectedUsers.SelectedIndexChanged += new System.EventHandler(this.lbSelectedUsers_SelectedIndexChanged);
            // 
            // mbtnAddUsers
            // 
            resources.ApplyResources(this.mbtnAddUsers, "mbtnAddUsers");
            this.mbtnAddUsers.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveRightOne;
            this.mbtnAddUsers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnAddUsers.Name = "mbtnAddUsers";
            this.mbtnAddUsers.UseSelectable = true;
            this.mbtnAddUsers.Click += new System.EventHandler(this.mbtnAddUsers_Click);
            // 
            // mbtnRemoveUsers
            // 
            resources.ApplyResources(this.mbtnRemoveUsers, "mbtnRemoveUsers");
            this.mbtnRemoveUsers.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveLeftOne;
            this.mbtnRemoveUsers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnRemoveUsers.Name = "mbtnRemoveUsers";
            this.mbtnRemoveUsers.UseSelectable = true;
            this.mbtnRemoveUsers.Click += new System.EventHandler(this.mbtnRemoveUsers_Click);
            // 
            // tlpPermissions
            // 
            resources.ApplyResources(this.tlpPermissions, "tlpPermissions");
            this.tlpPermissions.Controls.Add(this.mlblAvailablePermissions, 0, 0);
            this.tlpPermissions.Controls.Add(this.mlblSelectedPermissions, 2, 0);
            this.tlpPermissions.Controls.Add(this.lbAvailablePermissions, 0, 1);
            this.tlpPermissions.Controls.Add(this.lbSelectedPermissions, 2, 1);
            this.tlpPermissions.Controls.Add(this.mbtnAddPermissions, 1, 1);
            this.tlpPermissions.Controls.Add(this.mbtnRemovePermissions, 1, 2);
            this.tlpPermissions.Controls.Add(this.mbtnAddAllPermissions, 1, 3);
            this.tlpPermissions.Controls.Add(this.mbtnRemoveAllPermissions, 1, 4);
            this.tlpPermissions.Name = "tlpPermissions";
            // 
            // mlblAvailablePermissions
            // 
            resources.ApplyResources(this.mlblAvailablePermissions, "mlblAvailablePermissions");
            this.mlblAvailablePermissions.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblAvailablePermissions.Name = "mlblAvailablePermissions";
            // 
            // mlblSelectedPermissions
            // 
            resources.ApplyResources(this.mlblSelectedPermissions, "mlblSelectedPermissions");
            this.mlblSelectedPermissions.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblSelectedPermissions.Name = "mlblSelectedPermissions";
            // 
            // lbAvailablePermissions
            // 
            resources.ApplyResources(this.lbAvailablePermissions, "lbAvailablePermissions");
            this.lbAvailablePermissions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbAvailablePermissions.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbAvailablePermissions.FormattingEnabled = true;
            this.lbAvailablePermissions.Name = "lbAvailablePermissions";
            this.tlpPermissions.SetRowSpan(this.lbAvailablePermissions, 4);
            this.lbAvailablePermissions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAvailablePermissions.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbPermissions_DrawItem);
            this.lbAvailablePermissions.SelectedIndexChanged += new System.EventHandler(this.lbAvailablePermissions_SelectedIndexChanged);
            // 
            // lbSelectedPermissions
            // 
            resources.ApplyResources(this.lbSelectedPermissions, "lbSelectedPermissions");
            this.lbSelectedPermissions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbSelectedPermissions.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbSelectedPermissions.FormattingEnabled = true;
            this.lbSelectedPermissions.Name = "lbSelectedPermissions";
            this.tlpPermissions.SetRowSpan(this.lbSelectedPermissions, 4);
            this.lbSelectedPermissions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbSelectedPermissions.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbPermissions_DrawItem);
            this.lbSelectedPermissions.SelectedIndexChanged += new System.EventHandler(this.lbSelectedPermissions_SelectedIndexChanged);
            // 
            // mbtnAddPermissions
            // 
            resources.ApplyResources(this.mbtnAddPermissions, "mbtnAddPermissions");
            this.mbtnAddPermissions.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveRightOne;
            this.mbtnAddPermissions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnAddPermissions.Name = "mbtnAddPermissions";
            this.mbtnAddPermissions.UseSelectable = true;
            this.mbtnAddPermissions.Click += new System.EventHandler(this.mbtnAddPermissions_Click);
            // 
            // mbtnRemovePermissions
            // 
            resources.ApplyResources(this.mbtnRemovePermissions, "mbtnRemovePermissions");
            this.mbtnRemovePermissions.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveLeftOne;
            this.mbtnRemovePermissions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnRemovePermissions.Name = "mbtnRemovePermissions";
            this.mbtnRemovePermissions.UseSelectable = true;
            this.mbtnRemovePermissions.Click += new System.EventHandler(this.mbtnRemovePermissions_Click);
            // 
            // mbtnAddAllPermissions
            // 
            resources.ApplyResources(this.mbtnAddAllPermissions, "mbtnAddAllPermissions");
            this.mbtnAddAllPermissions.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveRightAll;
            this.mbtnAddAllPermissions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnAddAllPermissions.Name = "mbtnAddAllPermissions";
            this.mbtnAddAllPermissions.UseSelectable = true;
            this.mbtnAddAllPermissions.Click += new System.EventHandler(this.mbtnAddAllPermissions_Click);
            // 
            // mbtnRemoveAllPermissions
            // 
            resources.ApplyResources(this.mbtnRemoveAllPermissions, "mbtnRemoveAllPermissions");
            this.mbtnRemoveAllPermissions.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveLeftAll;
            this.mbtnRemoveAllPermissions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnRemoveAllPermissions.Name = "mbtnRemoveAllPermissions";
            this.mbtnRemoveAllPermissions.UseSelectable = true;
            this.mbtnRemoveAllPermissions.Click += new System.EventHandler(this.mbtnRemoveAllPermissions_Click);
            // 
            // RegisterRoleControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "RegisterRoleControl";
            this.Load += new System.EventHandler(this.RegisterRole_Load);
            this.Controls.SetChildIndex(this.tlpMain, 0);
            this.tlpMain.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.tlpUsers.ResumeLayout(false);
            this.tlpPermissions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnContent;
        private MetroFramework.Controls.MetroLabel mlblAvailableUsers;
        private MetroFramework.Controls.MetroTextBox mtxtDescription;
        private MetroFramework.Controls.MetroLabel mlblDescription;
        private MetroFramework.Controls.MetroLabel mlblSelectedUsers;
        private MetroFramework.Controls.MetroTextBox mtxtName;
        private MetroFramework.Controls.MetroLabel mlblName;
        private System.Windows.Forms.TableLayoutPanel tlpUsers;
        private System.Windows.Forms.ListBox lbAvailableUsers;
        private System.Windows.Forms.ListBox lbSelectedUsers;
        private MetroFramework.Controls.MetroButton mbtnAddUsers;
        private MetroFramework.Controls.MetroButton mbtnRemoveUsers;
        private System.Windows.Forms.TableLayoutPanel tlpPermissions;
        private MetroFramework.Controls.MetroLabel mlblAvailablePermissions;
        private MetroFramework.Controls.MetroLabel mlblSelectedPermissions;
        private System.Windows.Forms.ListBox lbAvailablePermissions;
        private System.Windows.Forms.ListBox lbSelectedPermissions;
        private MetroFramework.Controls.MetroButton mbtnAddPermissions;
        private MetroFramework.Controls.MetroButton mbtnRemovePermissions;
        private MetroFramework.Controls.MetroButton mbtnAddAllPermissions;
        private MetroFramework.Controls.MetroButton mbtnRemoveAllPermissions;
    }
}
