namespace PnT.SongClient.UI.Controls
{
    partial class RegisterEventControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterEventControl));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnContent = new System.Windows.Forms.Panel();
            this.mtxtDescription = new MetroFramework.Controls.MetroTextBox();
            this.mlblDescription = new MetroFramework.Controls.MetroLabel();
            this.nudDuration = new PnT.SongClient.UI.Controls.UnitNumericUpDown();
            this.mtxtStartTime = new System.Windows.Forms.MaskedTextBox();
            this.mlblStartTime = new MetroFramework.Controls.MetroLabel();
            this.mlblDuration = new MetroFramework.Controls.MetroLabel();
            this.mcbSendOption = new MetroFramework.Controls.MetroComboBox();
            this.mlblSendOption = new MetroFramework.Controls.MetroLabel();
            this.mtxtDate = new System.Windows.Forms.MaskedTextBox();
            this.mlblDate = new MetroFramework.Controls.MetroLabel();
            this.mcbInstitution = new MetroFramework.Controls.MetroComboBox();
            this.mlblInstitution = new MetroFramework.Controls.MetroLabel();
            this.mtxtLocation = new MetroFramework.Controls.MetroTextBox();
            this.mlblLocation = new MetroFramework.Controls.MetroLabel();
            this.mtxtName = new MetroFramework.Controls.MetroTextBox();
            this.mlblName = new MetroFramework.Controls.MetroLabel();
            this.tlpMain.SuspendLayout();
            this.pnContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.pnContent, 2, 0);
            this.tlpMain.Name = "tlpMain";
            // 
            // pnContent
            // 
            this.pnContent.BackColor = System.Drawing.Color.White;
            this.pnContent.Controls.Add(this.mtxtDescription);
            this.pnContent.Controls.Add(this.mlblDescription);
            this.pnContent.Controls.Add(this.nudDuration);
            this.pnContent.Controls.Add(this.mtxtStartTime);
            this.pnContent.Controls.Add(this.mlblStartTime);
            this.pnContent.Controls.Add(this.mlblDuration);
            this.pnContent.Controls.Add(this.mcbSendOption);
            this.pnContent.Controls.Add(this.mlblSendOption);
            this.pnContent.Controls.Add(this.mtxtDate);
            this.pnContent.Controls.Add(this.mlblDate);
            this.pnContent.Controls.Add(this.mcbInstitution);
            this.pnContent.Controls.Add(this.mlblInstitution);
            this.pnContent.Controls.Add(this.mtxtLocation);
            this.pnContent.Controls.Add(this.mlblLocation);
            this.pnContent.Controls.Add(this.mtxtName);
            this.pnContent.Controls.Add(this.mlblName);
            resources.ApplyResources(this.pnContent, "pnContent");
            this.pnContent.Name = "pnContent";
            // 
            // mtxtDescription
            // 
            // 
            // 
            // 
            this.mtxtDescription.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.mtxtDescription.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode")));
            this.mtxtDescription.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location")));
            this.mtxtDescription.CustomButton.Name = "";
            this.mtxtDescription.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size")));
            this.mtxtDescription.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtDescription.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex")));
            this.mtxtDescription.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtDescription.CustomButton.UseSelectable = true;
            this.mtxtDescription.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible")));
            this.mtxtDescription.Lines = new string[0];
            resources.ApplyResources(this.mtxtDescription, "mtxtDescription");
            this.mtxtDescription.MaxLength = 32767;
            this.mtxtDescription.Multiline = true;
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
            this.mlblDescription.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblDescription, "mlblDescription");
            this.mlblDescription.Name = "mlblDescription";
            // 
            // nudDuration
            // 
            this.nudDuration.BackColor = System.Drawing.Color.White;
            this.nudDuration.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            resources.ApplyResources(this.nudDuration, "nudDuration");
            this.nudDuration.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudDuration.Name = "nudDuration";
            this.nudDuration.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // mtxtStartTime
            // 
            this.mtxtStartTime.BackColor = System.Drawing.Color.White;
            this.mtxtStartTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.mtxtStartTime, "mtxtStartTime");
            this.mtxtStartTime.Name = "mtxtStartTime";
            this.mtxtStartTime.ValidatingType = typeof(System.DateTime);
            this.mtxtStartTime.Click += new System.EventHandler(this.Time_Click);
            // 
            // mlblStartTime
            // 
            this.mlblStartTime.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblStartTime, "mlblStartTime");
            this.mlblStartTime.Name = "mlblStartTime";
            // 
            // mlblDuration
            // 
            this.mlblDuration.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblDuration, "mlblDuration");
            this.mlblDuration.Name = "mlblDuration";
            // 
            // mcbSendOption
            // 
            this.mcbSendOption.DisplayMember = "Description";
            this.mcbSendOption.DropDownWidth = 240;
            this.mcbSendOption.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbSendOption.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbSendOption.FormattingEnabled = true;
            resources.ApplyResources(this.mcbSendOption, "mcbSendOption");
            this.mcbSendOption.Name = "mcbSendOption";
            this.mcbSendOption.UseSelectable = true;
            this.mcbSendOption.ValueMember = "Id";
            // 
            // mlblSendOption
            // 
            this.mlblSendOption.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblSendOption, "mlblSendOption");
            this.mlblSendOption.Name = "mlblSendOption";
            // 
            // mtxtDate
            // 
            this.mtxtDate.BackColor = System.Drawing.Color.White;
            this.mtxtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.mtxtDate, "mtxtDate");
            this.mtxtDate.Name = "mtxtDate";
            this.mtxtDate.ValidatingType = typeof(System.DateTime);
            this.mtxtDate.Click += new System.EventHandler(this.Date_Click);
            // 
            // mlblDate
            // 
            this.mlblDate.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblDate, "mlblDate");
            this.mlblDate.Name = "mlblDate";
            // 
            // mcbInstitution
            // 
            this.mcbInstitution.DisplayMember = "Description";
            this.mcbInstitution.DropDownWidth = 240;
            this.mcbInstitution.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbInstitution.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbInstitution.FormattingEnabled = true;
            resources.ApplyResources(this.mcbInstitution, "mcbInstitution");
            this.mcbInstitution.Name = "mcbInstitution";
            this.mcbInstitution.UseSelectable = true;
            this.mcbInstitution.ValueMember = "Id";
            // 
            // mlblInstitution
            // 
            this.mlblInstitution.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblInstitution, "mlblInstitution");
            this.mlblInstitution.Name = "mlblInstitution";
            // 
            // mtxtLocation
            // 
            // 
            // 
            // 
            this.mtxtLocation.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            this.mtxtLocation.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode1")));
            this.mtxtLocation.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location1")));
            this.mtxtLocation.CustomButton.Name = "";
            this.mtxtLocation.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size1")));
            this.mtxtLocation.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtLocation.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex1")));
            this.mtxtLocation.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtLocation.CustomButton.UseSelectable = true;
            this.mtxtLocation.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible1")));
            this.mtxtLocation.Lines = new string[0];
            resources.ApplyResources(this.mtxtLocation, "mtxtLocation");
            this.mtxtLocation.MaxLength = 32767;
            this.mtxtLocation.Name = "mtxtLocation";
            this.mtxtLocation.PasswordChar = '\0';
            this.mtxtLocation.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtLocation.SelectedText = "";
            this.mtxtLocation.SelectionLength = 0;
            this.mtxtLocation.SelectionStart = 0;
            this.mtxtLocation.ShortcutsEnabled = true;
            this.mtxtLocation.UseSelectable = true;
            this.mtxtLocation.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtLocation.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblLocation
            // 
            this.mlblLocation.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblLocation, "mlblLocation");
            this.mlblLocation.Name = "mlblLocation";
            // 
            // mtxtName
            // 
            // 
            // 
            // 
            this.mtxtName.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            this.mtxtName.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode2")));
            this.mtxtName.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location2")));
            this.mtxtName.CustomButton.Name = "";
            this.mtxtName.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size2")));
            this.mtxtName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtName.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex2")));
            this.mtxtName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtName.CustomButton.UseSelectable = true;
            this.mtxtName.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible2")));
            this.mtxtName.Lines = new string[0];
            resources.ApplyResources(this.mtxtName, "mtxtName");
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
            this.mlblName.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblName, "mlblName");
            this.mlblName.Name = "mlblName";
            // 
            // RegisterEventControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "RegisterEventControl";
            this.Load += new System.EventHandler(this.RegisterEvent_Load);
            this.Controls.SetChildIndex(this.tlpMain, 0);
            this.tlpMain.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.pnContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnContent;
        private MetroFramework.Controls.MetroTextBox mtxtLocation;
        private MetroFramework.Controls.MetroLabel mlblLocation;
        private MetroFramework.Controls.MetroTextBox mtxtName;
        private MetroFramework.Controls.MetroLabel mlblName;
        private MetroFramework.Controls.MetroComboBox mcbInstitution;
        private MetroFramework.Controls.MetroLabel mlblInstitution;
        private System.Windows.Forms.MaskedTextBox mtxtDate;
        private MetroFramework.Controls.MetroLabel mlblDate;
        private MetroFramework.Controls.MetroComboBox mcbSendOption;
        private MetroFramework.Controls.MetroLabel mlblSendOption;
        private UnitNumericUpDown nudDuration;
        private System.Windows.Forms.MaskedTextBox mtxtStartTime;
        private MetroFramework.Controls.MetroLabel mlblStartTime;
        private MetroFramework.Controls.MetroLabel mlblDuration;
        private MetroFramework.Controls.MetroTextBox mtxtDescription;
        private MetroFramework.Controls.MetroLabel mlblDescription;
    }
}
