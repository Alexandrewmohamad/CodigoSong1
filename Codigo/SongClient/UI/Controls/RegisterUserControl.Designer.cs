namespace PnT.SongClient.UI.Controls
{
    partial class RegisterUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterUserControl));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnContent = new System.Windows.Forms.Panel();
            this.mcbRequestPasswordChange = new MetroFramework.Controls.MetroCheckBox();
            this.mcbInstitution = new MetroFramework.Controls.MetroComboBox();
            this.mlblInstitution = new MetroFramework.Controls.MetroLabel();
            this.mtxtConfirmPassword = new MetroFramework.Controls.MetroTextBox();
            this.mlblConfirmPassword = new MetroFramework.Controls.MetroLabel();
            this.mtxtLogin = new MetroFramework.Controls.MetroTextBox();
            this.mlblLogin = new MetroFramework.Controls.MetroLabel();
            this.mcbStatus = new MetroFramework.Controls.MetroComboBox();
            this.mlblStatus = new MetroFramework.Controls.MetroLabel();
            this.mtxtEmail = new MetroFramework.Controls.MetroTextBox();
            this.mlblEmail = new MetroFramework.Controls.MetroLabel();
            this.mtxtPassword = new MetroFramework.Controls.MetroTextBox();
            this.mlblPassword = new MetroFramework.Controls.MetroLabel();
            this.mcbRole = new MetroFramework.Controls.MetroComboBox();
            this.mlblRole = new MetroFramework.Controls.MetroLabel();
            this.mtxtName = new MetroFramework.Controls.MetroTextBox();
            this.mlblName = new MetroFramework.Controls.MetroLabel();
            this.tlpMain.SuspendLayout();
            this.pnContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.BackColor = System.Drawing.Color.White;
            this.tlpMain.Controls.Add(this.pnContent, 2, 0);
            this.tlpMain.Name = "tlpMain";
            // 
            // pnContent
            // 
            resources.ApplyResources(this.pnContent, "pnContent");
            this.pnContent.BackColor = System.Drawing.Color.White;
            this.pnContent.Controls.Add(this.mcbRequestPasswordChange);
            this.pnContent.Controls.Add(this.mcbInstitution);
            this.pnContent.Controls.Add(this.mlblInstitution);
            this.pnContent.Controls.Add(this.mtxtConfirmPassword);
            this.pnContent.Controls.Add(this.mlblConfirmPassword);
            this.pnContent.Controls.Add(this.mtxtLogin);
            this.pnContent.Controls.Add(this.mlblLogin);
            this.pnContent.Controls.Add(this.mcbStatus);
            this.pnContent.Controls.Add(this.mlblStatus);
            this.pnContent.Controls.Add(this.mtxtEmail);
            this.pnContent.Controls.Add(this.mlblEmail);
            this.pnContent.Controls.Add(this.mtxtPassword);
            this.pnContent.Controls.Add(this.mlblPassword);
            this.pnContent.Controls.Add(this.mcbRole);
            this.pnContent.Controls.Add(this.mlblRole);
            this.pnContent.Controls.Add(this.mtxtName);
            this.pnContent.Controls.Add(this.mlblName);
            this.pnContent.Name = "pnContent";
            // 
            // mcbRequestPasswordChange
            // 
            resources.ApplyResources(this.mcbRequestPasswordChange, "mcbRequestPasswordChange");
            this.mcbRequestPasswordChange.FontWeight = MetroFramework.MetroCheckBoxWeight.Light;
            this.mcbRequestPasswordChange.Name = "mcbRequestPasswordChange";
            this.mcbRequestPasswordChange.UseSelectable = true;
            // 
            // mcbInstitution
            // 
            resources.ApplyResources(this.mcbInstitution, "mcbInstitution");
            this.mcbInstitution.DisplayMember = "Description";
            this.mcbInstitution.DropDownWidth = 240;
            this.mcbInstitution.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbInstitution.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbInstitution.FormattingEnabled = true;
            this.mcbInstitution.Name = "mcbInstitution";
            this.mcbInstitution.UseSelectable = true;
            this.mcbInstitution.ValueMember = "Id";
            // 
            // mlblInstitution
            // 
            resources.ApplyResources(this.mlblInstitution, "mlblInstitution");
            this.mlblInstitution.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblInstitution.Name = "mlblInstitution";
            // 
            // mtxtConfirmPassword
            // 
            resources.ApplyResources(this.mtxtConfirmPassword, "mtxtConfirmPassword");
            // 
            // 
            // 
            this.mtxtConfirmPassword.CustomButton.AccessibleDescription = resources.GetString("resource.AccessibleDescription");
            this.mtxtConfirmPassword.CustomButton.AccessibleName = resources.GetString("resource.AccessibleName");
            this.mtxtConfirmPassword.CustomButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("resource.Anchor")));
            this.mtxtConfirmPassword.CustomButton.AutoSize = ((bool)(resources.GetObject("resource.AutoSize")));
            this.mtxtConfirmPassword.CustomButton.AutoSizeMode = ((System.Windows.Forms.AutoSizeMode)(resources.GetObject("resource.AutoSizeMode")));
            this.mtxtConfirmPassword.CustomButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resource.BackgroundImage")));
            this.mtxtConfirmPassword.CustomButton.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("resource.BackgroundImageLayout")));
            this.mtxtConfirmPassword.CustomButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("resource.Dock")));
            this.mtxtConfirmPassword.CustomButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("resource.FlatStyle")));
            this.mtxtConfirmPassword.CustomButton.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font")));
            this.mtxtConfirmPassword.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.mtxtConfirmPassword.CustomButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.ImageAlign")));
            this.mtxtConfirmPassword.CustomButton.ImageIndex = ((int)(resources.GetObject("resource.ImageIndex")));
            this.mtxtConfirmPassword.CustomButton.ImageKey = resources.GetString("resource.ImageKey");
            this.mtxtConfirmPassword.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode")));
            this.mtxtConfirmPassword.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location")));
            this.mtxtConfirmPassword.CustomButton.MaximumSize = ((System.Drawing.Size)(resources.GetObject("resource.MaximumSize")));
            this.mtxtConfirmPassword.CustomButton.Name = "";
            this.mtxtConfirmPassword.CustomButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("resource.RightToLeft")));
            this.mtxtConfirmPassword.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size")));
            this.mtxtConfirmPassword.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtConfirmPassword.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex")));
            this.mtxtConfirmPassword.CustomButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.TextAlign")));
            this.mtxtConfirmPassword.CustomButton.TextImageRelation = ((System.Windows.Forms.TextImageRelation)(resources.GetObject("resource.TextImageRelation")));
            this.mtxtConfirmPassword.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtConfirmPassword.CustomButton.UseSelectable = true;
            this.mtxtConfirmPassword.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible")));
            this.mtxtConfirmPassword.Lines = new string[0];
            this.mtxtConfirmPassword.MaxLength = 32767;
            this.mtxtConfirmPassword.Name = "mtxtConfirmPassword";
            this.mtxtConfirmPassword.PasswordChar = '●';
            this.mtxtConfirmPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtConfirmPassword.SelectedText = "";
            this.mtxtConfirmPassword.SelectionLength = 0;
            this.mtxtConfirmPassword.SelectionStart = 0;
            this.mtxtConfirmPassword.ShortcutsEnabled = true;
            this.mtxtConfirmPassword.UseSelectable = true;
            this.mtxtConfirmPassword.UseSystemPasswordChar = true;
            this.mtxtConfirmPassword.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtConfirmPassword.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblConfirmPassword
            // 
            resources.ApplyResources(this.mlblConfirmPassword, "mlblConfirmPassword");
            this.mlblConfirmPassword.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblConfirmPassword.Name = "mlblConfirmPassword";
            // 
            // mtxtLogin
            // 
            resources.ApplyResources(this.mtxtLogin, "mtxtLogin");
            // 
            // 
            // 
            this.mtxtLogin.CustomButton.AccessibleDescription = resources.GetString("resource.AccessibleDescription1");
            this.mtxtLogin.CustomButton.AccessibleName = resources.GetString("resource.AccessibleName1");
            this.mtxtLogin.CustomButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("resource.Anchor1")));
            this.mtxtLogin.CustomButton.AutoSize = ((bool)(resources.GetObject("resource.AutoSize1")));
            this.mtxtLogin.CustomButton.AutoSizeMode = ((System.Windows.Forms.AutoSizeMode)(resources.GetObject("resource.AutoSizeMode1")));
            this.mtxtLogin.CustomButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resource.BackgroundImage1")));
            this.mtxtLogin.CustomButton.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("resource.BackgroundImageLayout1")));
            this.mtxtLogin.CustomButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("resource.Dock1")));
            this.mtxtLogin.CustomButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("resource.FlatStyle1")));
            this.mtxtLogin.CustomButton.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font1")));
            this.mtxtLogin.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            this.mtxtLogin.CustomButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.ImageAlign1")));
            this.mtxtLogin.CustomButton.ImageIndex = ((int)(resources.GetObject("resource.ImageIndex1")));
            this.mtxtLogin.CustomButton.ImageKey = resources.GetString("resource.ImageKey1");
            this.mtxtLogin.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode1")));
            this.mtxtLogin.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location1")));
            this.mtxtLogin.CustomButton.MaximumSize = ((System.Drawing.Size)(resources.GetObject("resource.MaximumSize1")));
            this.mtxtLogin.CustomButton.Name = "";
            this.mtxtLogin.CustomButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("resource.RightToLeft1")));
            this.mtxtLogin.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size1")));
            this.mtxtLogin.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtLogin.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex1")));
            this.mtxtLogin.CustomButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.TextAlign1")));
            this.mtxtLogin.CustomButton.TextImageRelation = ((System.Windows.Forms.TextImageRelation)(resources.GetObject("resource.TextImageRelation1")));
            this.mtxtLogin.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtLogin.CustomButton.UseSelectable = true;
            this.mtxtLogin.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible1")));
            this.mtxtLogin.Lines = new string[0];
            this.mtxtLogin.MaxLength = 32767;
            this.mtxtLogin.Name = "mtxtLogin";
            this.mtxtLogin.PasswordChar = '\0';
            this.mtxtLogin.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtLogin.SelectedText = "";
            this.mtxtLogin.SelectionLength = 0;
            this.mtxtLogin.SelectionStart = 0;
            this.mtxtLogin.ShortcutsEnabled = true;
            this.mtxtLogin.UseSelectable = true;
            this.mtxtLogin.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtLogin.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblLogin
            // 
            resources.ApplyResources(this.mlblLogin, "mlblLogin");
            this.mlblLogin.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblLogin.Name = "mlblLogin";
            // 
            // mcbStatus
            // 
            resources.ApplyResources(this.mcbStatus, "mcbStatus");
            this.mcbStatus.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbStatus.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbStatus.FormattingEnabled = true;
            this.mcbStatus.Name = "mcbStatus";
            this.mcbStatus.UseSelectable = true;
            // 
            // mlblStatus
            // 
            resources.ApplyResources(this.mlblStatus, "mlblStatus");
            this.mlblStatus.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblStatus.Name = "mlblStatus";
            // 
            // mtxtEmail
            // 
            resources.ApplyResources(this.mtxtEmail, "mtxtEmail");
            // 
            // 
            // 
            this.mtxtEmail.CustomButton.AccessibleDescription = resources.GetString("resource.AccessibleDescription2");
            this.mtxtEmail.CustomButton.AccessibleName = resources.GetString("resource.AccessibleName2");
            this.mtxtEmail.CustomButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("resource.Anchor2")));
            this.mtxtEmail.CustomButton.AutoSize = ((bool)(resources.GetObject("resource.AutoSize2")));
            this.mtxtEmail.CustomButton.AutoSizeMode = ((System.Windows.Forms.AutoSizeMode)(resources.GetObject("resource.AutoSizeMode2")));
            this.mtxtEmail.CustomButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resource.BackgroundImage2")));
            this.mtxtEmail.CustomButton.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("resource.BackgroundImageLayout2")));
            this.mtxtEmail.CustomButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("resource.Dock2")));
            this.mtxtEmail.CustomButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("resource.FlatStyle2")));
            this.mtxtEmail.CustomButton.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font2")));
            this.mtxtEmail.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            this.mtxtEmail.CustomButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.ImageAlign2")));
            this.mtxtEmail.CustomButton.ImageIndex = ((int)(resources.GetObject("resource.ImageIndex2")));
            this.mtxtEmail.CustomButton.ImageKey = resources.GetString("resource.ImageKey2");
            this.mtxtEmail.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode2")));
            this.mtxtEmail.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location2")));
            this.mtxtEmail.CustomButton.MaximumSize = ((System.Drawing.Size)(resources.GetObject("resource.MaximumSize2")));
            this.mtxtEmail.CustomButton.Name = "";
            this.mtxtEmail.CustomButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("resource.RightToLeft2")));
            this.mtxtEmail.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size2")));
            this.mtxtEmail.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtEmail.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex2")));
            this.mtxtEmail.CustomButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.TextAlign2")));
            this.mtxtEmail.CustomButton.TextImageRelation = ((System.Windows.Forms.TextImageRelation)(resources.GetObject("resource.TextImageRelation2")));
            this.mtxtEmail.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtEmail.CustomButton.UseSelectable = true;
            this.mtxtEmail.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible2")));
            this.mtxtEmail.Lines = new string[0];
            this.mtxtEmail.MaxLength = 32767;
            this.mtxtEmail.Name = "mtxtEmail";
            this.mtxtEmail.PasswordChar = '\0';
            this.mtxtEmail.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtEmail.SelectedText = "";
            this.mtxtEmail.SelectionLength = 0;
            this.mtxtEmail.SelectionStart = 0;
            this.mtxtEmail.ShortcutsEnabled = true;
            this.mtxtEmail.UseSelectable = true;
            this.mtxtEmail.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtEmail.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblEmail
            // 
            resources.ApplyResources(this.mlblEmail, "mlblEmail");
            this.mlblEmail.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblEmail.Name = "mlblEmail";
            // 
            // mtxtPassword
            // 
            resources.ApplyResources(this.mtxtPassword, "mtxtPassword");
            // 
            // 
            // 
            this.mtxtPassword.CustomButton.AccessibleDescription = resources.GetString("resource.AccessibleDescription3");
            this.mtxtPassword.CustomButton.AccessibleName = resources.GetString("resource.AccessibleName3");
            this.mtxtPassword.CustomButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("resource.Anchor3")));
            this.mtxtPassword.CustomButton.AutoSize = ((bool)(resources.GetObject("resource.AutoSize3")));
            this.mtxtPassword.CustomButton.AutoSizeMode = ((System.Windows.Forms.AutoSizeMode)(resources.GetObject("resource.AutoSizeMode3")));
            this.mtxtPassword.CustomButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resource.BackgroundImage3")));
            this.mtxtPassword.CustomButton.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("resource.BackgroundImageLayout3")));
            this.mtxtPassword.CustomButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("resource.Dock3")));
            this.mtxtPassword.CustomButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("resource.FlatStyle3")));
            this.mtxtPassword.CustomButton.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font3")));
            this.mtxtPassword.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image3")));
            this.mtxtPassword.CustomButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.ImageAlign3")));
            this.mtxtPassword.CustomButton.ImageIndex = ((int)(resources.GetObject("resource.ImageIndex3")));
            this.mtxtPassword.CustomButton.ImageKey = resources.GetString("resource.ImageKey3");
            this.mtxtPassword.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode3")));
            this.mtxtPassword.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location3")));
            this.mtxtPassword.CustomButton.MaximumSize = ((System.Drawing.Size)(resources.GetObject("resource.MaximumSize3")));
            this.mtxtPassword.CustomButton.Name = "";
            this.mtxtPassword.CustomButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("resource.RightToLeft3")));
            this.mtxtPassword.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size3")));
            this.mtxtPassword.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtPassword.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex3")));
            this.mtxtPassword.CustomButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.TextAlign3")));
            this.mtxtPassword.CustomButton.TextImageRelation = ((System.Windows.Forms.TextImageRelation)(resources.GetObject("resource.TextImageRelation3")));
            this.mtxtPassword.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtPassword.CustomButton.UseSelectable = true;
            this.mtxtPassword.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible3")));
            this.mtxtPassword.Lines = new string[0];
            this.mtxtPassword.MaxLength = 32767;
            this.mtxtPassword.Name = "mtxtPassword";
            this.mtxtPassword.PasswordChar = '●';
            this.mtxtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtPassword.SelectedText = "";
            this.mtxtPassword.SelectionLength = 0;
            this.mtxtPassword.SelectionStart = 0;
            this.mtxtPassword.ShortcutsEnabled = true;
            this.mtxtPassword.UseSelectable = true;
            this.mtxtPassword.UseSystemPasswordChar = true;
            this.mtxtPassword.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtPassword.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblPassword
            // 
            resources.ApplyResources(this.mlblPassword, "mlblPassword");
            this.mlblPassword.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblPassword.Name = "mlblPassword";
            // 
            // mcbRole
            // 
            resources.ApplyResources(this.mcbRole, "mcbRole");
            this.mcbRole.DisplayMember = "Description";
            this.mcbRole.DropDownWidth = 240;
            this.mcbRole.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbRole.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbRole.FormattingEnabled = true;
            this.mcbRole.Name = "mcbRole";
            this.mcbRole.UseSelectable = true;
            this.mcbRole.ValueMember = "Id";
            // 
            // mlblRole
            // 
            resources.ApplyResources(this.mlblRole, "mlblRole");
            this.mlblRole.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblRole.Name = "mlblRole";
            // 
            // mtxtName
            // 
            resources.ApplyResources(this.mtxtName, "mtxtName");
            // 
            // 
            // 
            this.mtxtName.CustomButton.AccessibleDescription = resources.GetString("resource.AccessibleDescription4");
            this.mtxtName.CustomButton.AccessibleName = resources.GetString("resource.AccessibleName4");
            this.mtxtName.CustomButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("resource.Anchor4")));
            this.mtxtName.CustomButton.AutoSize = ((bool)(resources.GetObject("resource.AutoSize4")));
            this.mtxtName.CustomButton.AutoSizeMode = ((System.Windows.Forms.AutoSizeMode)(resources.GetObject("resource.AutoSizeMode4")));
            this.mtxtName.CustomButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resource.BackgroundImage4")));
            this.mtxtName.CustomButton.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("resource.BackgroundImageLayout4")));
            this.mtxtName.CustomButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("resource.Dock4")));
            this.mtxtName.CustomButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("resource.FlatStyle4")));
            this.mtxtName.CustomButton.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font4")));
            this.mtxtName.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image4")));
            this.mtxtName.CustomButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.ImageAlign4")));
            this.mtxtName.CustomButton.ImageIndex = ((int)(resources.GetObject("resource.ImageIndex4")));
            this.mtxtName.CustomButton.ImageKey = resources.GetString("resource.ImageKey4");
            this.mtxtName.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode4")));
            this.mtxtName.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location4")));
            this.mtxtName.CustomButton.MaximumSize = ((System.Drawing.Size)(resources.GetObject("resource.MaximumSize4")));
            this.mtxtName.CustomButton.Name = "";
            this.mtxtName.CustomButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("resource.RightToLeft4")));
            this.mtxtName.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size4")));
            this.mtxtName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtName.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex4")));
            this.mtxtName.CustomButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.TextAlign4")));
            this.mtxtName.CustomButton.TextImageRelation = ((System.Windows.Forms.TextImageRelation)(resources.GetObject("resource.TextImageRelation4")));
            this.mtxtName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtName.CustomButton.UseSelectable = true;
            this.mtxtName.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible4")));
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
            // RegisterUserControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "RegisterUserControl";
            this.Controls.SetChildIndex(this.tlpMain, 0);
            this.tlpMain.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnContent;
        private MetroFramework.Controls.MetroComboBox mcbStatus;
        private MetroFramework.Controls.MetroLabel mlblStatus;
        private MetroFramework.Controls.MetroTextBox mtxtEmail;
        private MetroFramework.Controls.MetroLabel mlblEmail;
        private MetroFramework.Controls.MetroTextBox mtxtPassword;
        private MetroFramework.Controls.MetroLabel mlblPassword;
        private MetroFramework.Controls.MetroComboBox mcbRole;
        private MetroFramework.Controls.MetroLabel mlblRole;
        private MetroFramework.Controls.MetroTextBox mtxtName;
        private MetroFramework.Controls.MetroLabel mlblName;
        private MetroFramework.Controls.MetroTextBox mtxtLogin;
        private MetroFramework.Controls.MetroLabel mlblLogin;
        private MetroFramework.Controls.MetroTextBox mtxtConfirmPassword;
        private MetroFramework.Controls.MetroLabel mlblConfirmPassword;
        private MetroFramework.Controls.MetroComboBox mcbInstitution;
        private MetroFramework.Controls.MetroLabel mlblInstitution;
        private MetroFramework.Controls.MetroCheckBox mcbRequestPasswordChange;
    }
}
