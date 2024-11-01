namespace PnT.SongClient.UI
{
    partial class ResetPasswordForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResetPasswordForm));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnStudent = new System.Windows.Forms.Panel();
            this.mtxtUser = new MetroFramework.Controls.MetroTextBox();
            this.mlblUser = new MetroFramework.Controls.MetroLabel();
            this.mbtnCancel = new MetroFramework.Controls.MetroButton();
            this.mbtnOK = new MetroFramework.Controls.MetroButton();
            this.pnContent = new System.Windows.Forms.Panel();
            this.mlblMessage = new MetroFramework.Controls.MetroLabel();
            this.tlpMain.SuspendLayout();
            this.pnStudent.SuspendLayout();
            this.pnContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.pnStudent, 0, 1);
            this.tlpMain.Controls.Add(this.mbtnCancel, 1, 2);
            this.tlpMain.Controls.Add(this.mbtnOK, 0, 2);
            this.tlpMain.Controls.Add(this.pnContent, 0, 0);
            this.tlpMain.Name = "tlpMain";
            // 
            // pnStudent
            // 
            this.tlpMain.SetColumnSpan(this.pnStudent, 2);
            this.pnStudent.Controls.Add(this.mtxtUser);
            this.pnStudent.Controls.Add(this.mlblUser);
            resources.ApplyResources(this.pnStudent, "pnStudent");
            this.pnStudent.Name = "pnStudent";
            // 
            // mtxtUser
            // 
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
            resources.ApplyResources(this.mtxtUser, "mtxtUser");
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
            this.mtxtUser.TextChanged += new System.EventHandler(this.mtxtUser_TextChanged);
            // 
            // mlblUser
            // 
            this.mlblUser.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblUser, "mlblUser");
            this.mlblUser.Name = "mlblUser";
            // 
            // mbtnCancel
            // 
            resources.ApplyResources(this.mbtnCancel, "mbtnCancel");
            this.mbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
            // pnContent
            // 
            this.tlpMain.SetColumnSpan(this.pnContent, 2);
            this.pnContent.Controls.Add(this.mlblMessage);
            resources.ApplyResources(this.pnContent, "pnContent");
            this.pnContent.Name = "pnContent";
            // 
            // mlblMessage
            // 
            this.mlblMessage.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblMessage, "mlblMessage");
            this.mlblMessage.Name = "mlblMessage";
            // 
            // ResetPasswordForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ControlBox = false;
            this.Controls.Add(this.tlpMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResetPasswordForm";
            this.Resizable = false;
            this.ShowInTaskbar = false;
            this.TextPosition = new System.Drawing.Point(15, 5);
            this.Load += new System.EventHandler(this.ResetPasswordForm_Load);
            this.tlpMain.ResumeLayout(false);
            this.pnStudent.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnStudent;
        private MetroFramework.Controls.MetroLabel mlblUser;
        private MetroFramework.Controls.MetroButton mbtnCancel;
        private MetroFramework.Controls.MetroButton mbtnOK;
        private System.Windows.Forms.Panel pnContent;
        private MetroFramework.Controls.MetroLabel mlblMessage;
        private MetroFramework.Controls.MetroTextBox mtxtUser;
    }
}