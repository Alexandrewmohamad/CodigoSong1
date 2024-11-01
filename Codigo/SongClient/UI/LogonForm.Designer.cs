namespace PnT.SongClient.UI
{
    partial class LogonForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogonForm));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mlblUser = new MetroFramework.Controls.MetroLabel();
            this.mtxtUser = new MetroFramework.Controls.MetroTextBox();
            this.mlblPassword = new MetroFramework.Controls.MetroLabel();
            this.mtxtPassword = new MetroFramework.Controls.MetroTextBox();
            this.mlblNewPassword = new MetroFramework.Controls.MetroLabel();
            this.mtxtNewPassword = new MetroFramework.Controls.MetroTextBox();
            this.mlblConfirmPassword = new MetroFramework.Controls.MetroLabel();
            this.mtxtConfirmPassword = new MetroFramework.Controls.MetroTextBox();
            this.pnButtons = new System.Windows.Forms.Panel();
            this.mlnkResetPassword = new MetroFramework.Controls.MetroLink();
            this.mbtnCancel = new MetroFramework.Controls.MetroButton();
            this.mbtnOK = new MetroFramework.Controls.MetroButton();
            this.tlpMain.SuspendLayout();
            this.pnButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mlblUser, 0, 0);
            this.tlpMain.Controls.Add(this.mtxtUser, 1, 0);
            this.tlpMain.Controls.Add(this.mlblPassword, 0, 1);
            this.tlpMain.Controls.Add(this.mtxtPassword, 1, 1);
            this.tlpMain.Controls.Add(this.mlblNewPassword, 0, 2);
            this.tlpMain.Controls.Add(this.mtxtNewPassword, 1, 2);
            this.tlpMain.Controls.Add(this.mlblConfirmPassword, 0, 3);
            this.tlpMain.Controls.Add(this.mtxtConfirmPassword, 1, 3);
            this.tlpMain.Controls.Add(this.pnButtons, 0, 4);
            this.tlpMain.Name = "tlpMain";
            // 
            // mlblUser
            // 
            resources.ApplyResources(this.mlblUser, "mlblUser");
            this.mlblUser.Name = "mlblUser";
            // 
            // mtxtUser
            // 
            resources.ApplyResources(this.mtxtUser, "mtxtUser");
            // 
            // 
            // 
            this.mtxtUser.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.mtxtUser.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode")));
            this.mtxtUser.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location")));
            this.mtxtUser.CustomButton.Name = "";
            this.mtxtUser.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size")));
            this.mtxtUser.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtUser.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex")));
            this.mtxtUser.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtUser.CustomButton.UseSelectable = true;
            this.mtxtUser.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible")));
            this.mtxtUser.Lines = new string[0];
            this.mtxtUser.MaxLength = 32767;
            this.mtxtUser.Name = "mtxtUser";
            this.mtxtUser.PasswordChar = '\0';
            this.mtxtUser.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtUser.SelectedText = "";
            this.mtxtUser.SelectionLength = 0;
            this.mtxtUser.SelectionStart = 0;
            this.mtxtUser.ShortcutsEnabled = true;
            this.mtxtUser.UseSelectable = true;
            this.mtxtUser.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtUser.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.mtxtUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxtUserData_KeyDown);
            this.mtxtUser.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mtxtUserData_KeyUp);
            // 
            // mlblPassword
            // 
            resources.ApplyResources(this.mlblPassword, "mlblPassword");
            this.mlblPassword.Name = "mlblPassword";
            // 
            // mtxtPassword
            // 
            resources.ApplyResources(this.mtxtPassword, "mtxtPassword");
            // 
            // 
            // 
            this.mtxtPassword.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            this.mtxtPassword.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode1")));
            this.mtxtPassword.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location1")));
            this.mtxtPassword.CustomButton.Name = "";
            this.mtxtPassword.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size1")));
            this.mtxtPassword.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtPassword.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex1")));
            this.mtxtPassword.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtPassword.CustomButton.UseSelectable = true;
            this.mtxtPassword.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible1")));
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
            this.mtxtPassword.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtPassword.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.mtxtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxtUserData_KeyDown);
            this.mtxtPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mtxtUserData_KeyUp);
            // 
            // mlblNewPassword
            // 
            resources.ApplyResources(this.mlblNewPassword, "mlblNewPassword");
            this.mlblNewPassword.Name = "mlblNewPassword";
            // 
            // mtxtNewPassword
            // 
            resources.ApplyResources(this.mtxtNewPassword, "mtxtNewPassword");
            // 
            // 
            // 
            this.mtxtNewPassword.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            this.mtxtNewPassword.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode2")));
            this.mtxtNewPassword.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location2")));
            this.mtxtNewPassword.CustomButton.Name = "";
            this.mtxtNewPassword.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size2")));
            this.mtxtNewPassword.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtNewPassword.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex2")));
            this.mtxtNewPassword.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtNewPassword.CustomButton.UseSelectable = true;
            this.mtxtNewPassword.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible2")));
            this.mtxtNewPassword.Lines = new string[0];
            this.mtxtNewPassword.MaxLength = 32767;
            this.mtxtNewPassword.Name = "mtxtNewPassword";
            this.mtxtNewPassword.PasswordChar = '●';
            this.mtxtNewPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtNewPassword.SelectedText = "";
            this.mtxtNewPassword.SelectionLength = 0;
            this.mtxtNewPassword.SelectionStart = 0;
            this.mtxtNewPassword.ShortcutsEnabled = true;
            this.mtxtNewPassword.UseSelectable = true;
            this.mtxtNewPassword.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtNewPassword.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.mtxtNewPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxtUserData_KeyDown);
            this.mtxtNewPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mtxtUserData_KeyUp);
            // 
            // mlblConfirmPassword
            // 
            resources.ApplyResources(this.mlblConfirmPassword, "mlblConfirmPassword");
            this.mlblConfirmPassword.Name = "mlblConfirmPassword";
            // 
            // mtxtConfirmPassword
            // 
            resources.ApplyResources(this.mtxtConfirmPassword, "mtxtConfirmPassword");
            // 
            // 
            // 
            this.mtxtConfirmPassword.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image3")));
            this.mtxtConfirmPassword.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode3")));
            this.mtxtConfirmPassword.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location3")));
            this.mtxtConfirmPassword.CustomButton.Name = "";
            this.mtxtConfirmPassword.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size3")));
            this.mtxtConfirmPassword.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtConfirmPassword.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex3")));
            this.mtxtConfirmPassword.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtConfirmPassword.CustomButton.UseSelectable = true;
            this.mtxtConfirmPassword.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible3")));
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
            this.mtxtConfirmPassword.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtConfirmPassword.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.mtxtConfirmPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxtUserData_KeyDown);
            this.mtxtConfirmPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mtxtUserData_KeyUp);
            // 
            // pnButtons
            // 
            this.tlpMain.SetColumnSpan(this.pnButtons, 2);
            this.pnButtons.Controls.Add(this.mlnkResetPassword);
            this.pnButtons.Controls.Add(this.mbtnCancel);
            this.pnButtons.Controls.Add(this.mbtnOK);
            resources.ApplyResources(this.pnButtons, "pnButtons");
            this.pnButtons.Name = "pnButtons";
            // 
            // mlnkResetPassword
            // 
            this.mlnkResetPassword.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            resources.ApplyResources(this.mlnkResetPassword, "mlnkResetPassword");
            this.mlnkResetPassword.Name = "mlnkResetPassword";
            this.mlnkResetPassword.UseSelectable = true;
            this.mlnkResetPassword.Click += new System.EventHandler(this.mlnkResetPassword_Click);
            // 
            // mbtnCancel
            // 
            this.mbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.mbtnCancel, "mbtnCancel");
            this.mbtnCancel.Name = "mbtnCancel";
            this.mbtnCancel.UseSelectable = true;
            // 
            // mbtnOK
            // 
            resources.ApplyResources(this.mbtnOK, "mbtnOK");
            this.mbtnOK.Name = "mbtnOK";
            this.mbtnOK.UseSelectable = true;
            this.mbtnOK.Click += new System.EventHandler(this.mbtnOK_Click);
            // 
            // LogonForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.Controls.Add(this.tlpMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogonForm";
            this.Resizable = false;
            this.TextPosition = new System.Drawing.Point(15, 5);
            this.Load += new System.EventHandler(this.LogonForm_Load);
            this.Shown += new System.EventHandler(this.LogonForm_Shown);
            this.tlpMain.ResumeLayout(false);
            this.pnButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroLabel mlblUser;
        private MetroFramework.Controls.MetroTextBox mtxtUser;
        private MetroFramework.Controls.MetroLabel mlblPassword;
        private MetroFramework.Controls.MetroTextBox mtxtPassword;
        private MetroFramework.Controls.MetroLabel mlblNewPassword;
        private MetroFramework.Controls.MetroTextBox mtxtNewPassword;
        private MetroFramework.Controls.MetroLabel mlblConfirmPassword;
        private MetroFramework.Controls.MetroTextBox mtxtConfirmPassword;
        private System.Windows.Forms.Panel pnButtons;
        private MetroFramework.Controls.MetroButton mbtnCancel;
        private MetroFramework.Controls.MetroButton mbtnOK;
        private MetroFramework.Controls.MetroLink mlnkResetPassword;
    }
}