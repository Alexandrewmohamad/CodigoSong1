namespace PnT.SongClient.UI.Controls
{
    partial class RegisterInstitutionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterInstitutionControl));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnContent = new System.Windows.Forms.Panel();
            this.mtxtMobile = new System.Windows.Forms.MaskedTextBox();
            this.mtxtPhone = new System.Windows.Forms.MaskedTextBox();
            this.mtxtTaxId = new System.Windows.Forms.MaskedTextBox();
            this.mtxtZipCode = new System.Windows.Forms.MaskedTextBox();
            this.mcbState = new MetroFramework.Controls.MetroComboBox();
            this.mcbStatus = new MetroFramework.Controls.MetroComboBox();
            this.mlblStatus = new MetroFramework.Controls.MetroLabel();
            this.mtxtDescription = new MetroFramework.Controls.MetroTextBox();
            this.mlblDescription = new MetroFramework.Controls.MetroLabel();
            this.mtxtSite = new MetroFramework.Controls.MetroTextBox();
            this.mlblSite = new MetroFramework.Controls.MetroLabel();
            this.mtxtEmail = new MetroFramework.Controls.MetroTextBox();
            this.mlblEmail = new MetroFramework.Controls.MetroLabel();
            this.mlblMobile = new MetroFramework.Controls.MetroLabel();
            this.mlblPhone = new MetroFramework.Controls.MetroLabel();
            this.mlblZipCode = new MetroFramework.Controls.MetroLabel();
            this.mlblState = new MetroFramework.Controls.MetroLabel();
            this.mtxtCity = new MetroFramework.Controls.MetroTextBox();
            this.mlblCity = new MetroFramework.Controls.MetroLabel();
            this.mtxtDistrict = new MetroFramework.Controls.MetroTextBox();
            this.mlblDistrict = new MetroFramework.Controls.MetroLabel();
            this.mtxtAddress = new MetroFramework.Controls.MetroTextBox();
            this.mlblAddress = new MetroFramework.Controls.MetroLabel();
            this.mlblTaxId = new MetroFramework.Controls.MetroLabel();
            this.mcbInstitutionalized = new MetroFramework.Controls.MetroCheckBox();
            this.mcbCoordinator = new MetroFramework.Controls.MetroComboBox();
            this.mlblCoordinator = new MetroFramework.Controls.MetroLabel();
            this.mtxtLocalInitiative = new MetroFramework.Controls.MetroTextBox();
            this.mlblLocalInitiative = new MetroFramework.Controls.MetroLabel();
            this.mtxtEntityName = new MetroFramework.Controls.MetroTextBox();
            this.mlblEntityName = new MetroFramework.Controls.MetroLabel();
            this.mtxtProjectName = new MetroFramework.Controls.MetroTextBox();
            this.mlblProjectName = new MetroFramework.Controls.MetroLabel();
            this.tlpMain.SuspendLayout();
            this.pnContent.SuspendLayout();
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
            this.pnContent.Controls.Add(this.mtxtMobile);
            this.pnContent.Controls.Add(this.mtxtPhone);
            this.pnContent.Controls.Add(this.mtxtTaxId);
            this.pnContent.Controls.Add(this.mtxtZipCode);
            this.pnContent.Controls.Add(this.mcbState);
            this.pnContent.Controls.Add(this.mcbStatus);
            this.pnContent.Controls.Add(this.mlblStatus);
            this.pnContent.Controls.Add(this.mtxtDescription);
            this.pnContent.Controls.Add(this.mlblDescription);
            this.pnContent.Controls.Add(this.mtxtSite);
            this.pnContent.Controls.Add(this.mlblSite);
            this.pnContent.Controls.Add(this.mtxtEmail);
            this.pnContent.Controls.Add(this.mlblEmail);
            this.pnContent.Controls.Add(this.mlblMobile);
            this.pnContent.Controls.Add(this.mlblPhone);
            this.pnContent.Controls.Add(this.mlblZipCode);
            this.pnContent.Controls.Add(this.mlblState);
            this.pnContent.Controls.Add(this.mtxtCity);
            this.pnContent.Controls.Add(this.mlblCity);
            this.pnContent.Controls.Add(this.mtxtDistrict);
            this.pnContent.Controls.Add(this.mlblDistrict);
            this.pnContent.Controls.Add(this.mtxtAddress);
            this.pnContent.Controls.Add(this.mlblAddress);
            this.pnContent.Controls.Add(this.mlblTaxId);
            this.pnContent.Controls.Add(this.mcbInstitutionalized);
            this.pnContent.Controls.Add(this.mcbCoordinator);
            this.pnContent.Controls.Add(this.mlblCoordinator);
            this.pnContent.Controls.Add(this.mtxtLocalInitiative);
            this.pnContent.Controls.Add(this.mlblLocalInitiative);
            this.pnContent.Controls.Add(this.mtxtEntityName);
            this.pnContent.Controls.Add(this.mlblEntityName);
            this.pnContent.Controls.Add(this.mtxtProjectName);
            this.pnContent.Controls.Add(this.mlblProjectName);
            resources.ApplyResources(this.pnContent, "pnContent");
            this.pnContent.Name = "pnContent";
            // 
            // mtxtMobile
            // 
            this.mtxtMobile.BackColor = System.Drawing.Color.White;
            this.mtxtMobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.mtxtMobile, "mtxtMobile");
            this.mtxtMobile.Name = "mtxtMobile";
            this.mtxtMobile.ValidatingType = typeof(System.DateTime);
            this.mtxtMobile.Click += new System.EventHandler(this.Phone_Click);
            // 
            // mtxtPhone
            // 
            this.mtxtPhone.BackColor = System.Drawing.Color.White;
            this.mtxtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.mtxtPhone, "mtxtPhone");
            this.mtxtPhone.Name = "mtxtPhone";
            this.mtxtPhone.ValidatingType = typeof(System.DateTime);
            this.mtxtPhone.Click += new System.EventHandler(this.Phone_Click);
            // 
            // mtxtTaxId
            // 
            this.mtxtTaxId.BackColor = System.Drawing.Color.White;
            this.mtxtTaxId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.mtxtTaxId, "mtxtTaxId");
            this.mtxtTaxId.Name = "mtxtTaxId";
            this.mtxtTaxId.ValidatingType = typeof(System.DateTime);
            this.mtxtTaxId.Click += new System.EventHandler(this.mtxtTaxId_Click);
            // 
            // mtxtZipCode
            // 
            this.mtxtZipCode.BackColor = System.Drawing.Color.White;
            this.mtxtZipCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.mtxtZipCode, "mtxtZipCode");
            this.mtxtZipCode.Name = "mtxtZipCode";
            this.mtxtZipCode.ValidatingType = typeof(System.DateTime);
            this.mtxtZipCode.Click += new System.EventHandler(this.mtxtZipCode_Click);
            // 
            // mcbState
            // 
            this.mcbState.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbState.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbState.FormattingEnabled = true;
            resources.ApplyResources(this.mcbState, "mcbState");
            this.mcbState.Name = "mcbState";
            this.mcbState.UseSelectable = true;
            // 
            // mcbStatus
            // 
            this.mcbStatus.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbStatus.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbStatus.FormattingEnabled = true;
            resources.ApplyResources(this.mcbStatus, "mcbStatus");
            this.mcbStatus.Name = "mcbStatus";
            this.mcbStatus.UseSelectable = true;
            // 
            // mlblStatus
            // 
            this.mlblStatus.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblStatus, "mlblStatus");
            this.mlblStatus.Name = "mlblStatus";
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
            // mtxtSite
            // 
            // 
            // 
            // 
            this.mtxtSite.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            this.mtxtSite.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode1")));
            this.mtxtSite.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location1")));
            this.mtxtSite.CustomButton.Name = "";
            this.mtxtSite.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size1")));
            this.mtxtSite.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtSite.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex1")));
            this.mtxtSite.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtSite.CustomButton.UseSelectable = true;
            this.mtxtSite.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible1")));
            this.mtxtSite.Lines = new string[0];
            resources.ApplyResources(this.mtxtSite, "mtxtSite");
            this.mtxtSite.MaxLength = 32767;
            this.mtxtSite.Name = "mtxtSite";
            this.mtxtSite.PasswordChar = '\0';
            this.mtxtSite.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtSite.SelectedText = "";
            this.mtxtSite.SelectionLength = 0;
            this.mtxtSite.SelectionStart = 0;
            this.mtxtSite.ShortcutsEnabled = true;
            this.mtxtSite.UseSelectable = true;
            this.mtxtSite.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtSite.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblSite
            // 
            this.mlblSite.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblSite, "mlblSite");
            this.mlblSite.Name = "mlblSite";
            // 
            // mtxtEmail
            // 
            // 
            // 
            // 
            this.mtxtEmail.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            this.mtxtEmail.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode2")));
            this.mtxtEmail.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location2")));
            this.mtxtEmail.CustomButton.Name = "";
            this.mtxtEmail.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size2")));
            this.mtxtEmail.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtEmail.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex2")));
            this.mtxtEmail.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtEmail.CustomButton.UseSelectable = true;
            this.mtxtEmail.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible2")));
            this.mtxtEmail.Lines = new string[0];
            resources.ApplyResources(this.mtxtEmail, "mtxtEmail");
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
            this.mlblEmail.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblEmail, "mlblEmail");
            this.mlblEmail.Name = "mlblEmail";
            // 
            // mlblMobile
            // 
            this.mlblMobile.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblMobile, "mlblMobile");
            this.mlblMobile.Name = "mlblMobile";
            // 
            // mlblPhone
            // 
            this.mlblPhone.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblPhone, "mlblPhone");
            this.mlblPhone.Name = "mlblPhone";
            // 
            // mlblZipCode
            // 
            this.mlblZipCode.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblZipCode, "mlblZipCode");
            this.mlblZipCode.Name = "mlblZipCode";
            // 
            // mlblState
            // 
            this.mlblState.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblState, "mlblState");
            this.mlblState.Name = "mlblState";
            // 
            // mtxtCity
            // 
            // 
            // 
            // 
            this.mtxtCity.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image3")));
            this.mtxtCity.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode3")));
            this.mtxtCity.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location3")));
            this.mtxtCity.CustomButton.Name = "";
            this.mtxtCity.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size3")));
            this.mtxtCity.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtCity.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex3")));
            this.mtxtCity.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtCity.CustomButton.UseSelectable = true;
            this.mtxtCity.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible3")));
            this.mtxtCity.Lines = new string[0];
            resources.ApplyResources(this.mtxtCity, "mtxtCity");
            this.mtxtCity.MaxLength = 32767;
            this.mtxtCity.Name = "mtxtCity";
            this.mtxtCity.PasswordChar = '\0';
            this.mtxtCity.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtCity.SelectedText = "";
            this.mtxtCity.SelectionLength = 0;
            this.mtxtCity.SelectionStart = 0;
            this.mtxtCity.ShortcutsEnabled = true;
            this.mtxtCity.UseSelectable = true;
            this.mtxtCity.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtCity.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblCity
            // 
            this.mlblCity.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblCity, "mlblCity");
            this.mlblCity.Name = "mlblCity";
            // 
            // mtxtDistrict
            // 
            // 
            // 
            // 
            this.mtxtDistrict.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image4")));
            this.mtxtDistrict.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode4")));
            this.mtxtDistrict.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location4")));
            this.mtxtDistrict.CustomButton.Name = "";
            this.mtxtDistrict.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size4")));
            this.mtxtDistrict.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtDistrict.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex4")));
            this.mtxtDistrict.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtDistrict.CustomButton.UseSelectable = true;
            this.mtxtDistrict.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible4")));
            this.mtxtDistrict.Lines = new string[0];
            resources.ApplyResources(this.mtxtDistrict, "mtxtDistrict");
            this.mtxtDistrict.MaxLength = 32767;
            this.mtxtDistrict.Name = "mtxtDistrict";
            this.mtxtDistrict.PasswordChar = '\0';
            this.mtxtDistrict.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtDistrict.SelectedText = "";
            this.mtxtDistrict.SelectionLength = 0;
            this.mtxtDistrict.SelectionStart = 0;
            this.mtxtDistrict.ShortcutsEnabled = true;
            this.mtxtDistrict.UseSelectable = true;
            this.mtxtDistrict.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtDistrict.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblDistrict
            // 
            this.mlblDistrict.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblDistrict, "mlblDistrict");
            this.mlblDistrict.Name = "mlblDistrict";
            // 
            // mtxtAddress
            // 
            // 
            // 
            // 
            this.mtxtAddress.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image5")));
            this.mtxtAddress.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode5")));
            this.mtxtAddress.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location5")));
            this.mtxtAddress.CustomButton.Name = "";
            this.mtxtAddress.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size5")));
            this.mtxtAddress.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtAddress.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex5")));
            this.mtxtAddress.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtAddress.CustomButton.UseSelectable = true;
            this.mtxtAddress.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible5")));
            this.mtxtAddress.Lines = new string[0];
            resources.ApplyResources(this.mtxtAddress, "mtxtAddress");
            this.mtxtAddress.MaxLength = 32767;
            this.mtxtAddress.Name = "mtxtAddress";
            this.mtxtAddress.PasswordChar = '\0';
            this.mtxtAddress.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtAddress.SelectedText = "";
            this.mtxtAddress.SelectionLength = 0;
            this.mtxtAddress.SelectionStart = 0;
            this.mtxtAddress.ShortcutsEnabled = true;
            this.mtxtAddress.UseSelectable = true;
            this.mtxtAddress.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtAddress.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblAddress
            // 
            this.mlblAddress.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblAddress, "mlblAddress");
            this.mlblAddress.Name = "mlblAddress";
            // 
            // mlblTaxId
            // 
            this.mlblTaxId.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblTaxId, "mlblTaxId");
            this.mlblTaxId.Name = "mlblTaxId";
            // 
            // mcbInstitutionalized
            // 
            resources.ApplyResources(this.mcbInstitutionalized, "mcbInstitutionalized");
            this.mcbInstitutionalized.FontWeight = MetroFramework.MetroCheckBoxWeight.Light;
            this.mcbInstitutionalized.Name = "mcbInstitutionalized";
            this.mcbInstitutionalized.UseSelectable = true;
            // 
            // mcbCoordinator
            // 
            this.mcbCoordinator.DropDownWidth = 240;
            this.mcbCoordinator.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbCoordinator.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbCoordinator.FormattingEnabled = true;
            resources.ApplyResources(this.mcbCoordinator, "mcbCoordinator");
            this.mcbCoordinator.Name = "mcbCoordinator";
            this.mcbCoordinator.UseSelectable = true;
            // 
            // mlblCoordinator
            // 
            this.mlblCoordinator.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblCoordinator, "mlblCoordinator");
            this.mlblCoordinator.Name = "mlblCoordinator";
            // 
            // mtxtLocalInitiative
            // 
            // 
            // 
            // 
            this.mtxtLocalInitiative.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image6")));
            this.mtxtLocalInitiative.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode6")));
            this.mtxtLocalInitiative.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location6")));
            this.mtxtLocalInitiative.CustomButton.Name = "";
            this.mtxtLocalInitiative.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size6")));
            this.mtxtLocalInitiative.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtLocalInitiative.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex6")));
            this.mtxtLocalInitiative.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtLocalInitiative.CustomButton.UseSelectable = true;
            this.mtxtLocalInitiative.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible6")));
            this.mtxtLocalInitiative.Lines = new string[0];
            resources.ApplyResources(this.mtxtLocalInitiative, "mtxtLocalInitiative");
            this.mtxtLocalInitiative.MaxLength = 32767;
            this.mtxtLocalInitiative.Name = "mtxtLocalInitiative";
            this.mtxtLocalInitiative.PasswordChar = '\0';
            this.mtxtLocalInitiative.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtLocalInitiative.SelectedText = "";
            this.mtxtLocalInitiative.SelectionLength = 0;
            this.mtxtLocalInitiative.SelectionStart = 0;
            this.mtxtLocalInitiative.ShortcutsEnabled = true;
            this.mtxtLocalInitiative.UseSelectable = true;
            this.mtxtLocalInitiative.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtLocalInitiative.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblLocalInitiative
            // 
            this.mlblLocalInitiative.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblLocalInitiative, "mlblLocalInitiative");
            this.mlblLocalInitiative.Name = "mlblLocalInitiative";
            // 
            // mtxtEntityName
            // 
            // 
            // 
            // 
            this.mtxtEntityName.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image7")));
            this.mtxtEntityName.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode7")));
            this.mtxtEntityName.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location7")));
            this.mtxtEntityName.CustomButton.Name = "";
            this.mtxtEntityName.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size7")));
            this.mtxtEntityName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtEntityName.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex7")));
            this.mtxtEntityName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtEntityName.CustomButton.UseSelectable = true;
            this.mtxtEntityName.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible7")));
            this.mtxtEntityName.Lines = new string[0];
            resources.ApplyResources(this.mtxtEntityName, "mtxtEntityName");
            this.mtxtEntityName.MaxLength = 32767;
            this.mtxtEntityName.Name = "mtxtEntityName";
            this.mtxtEntityName.PasswordChar = '\0';
            this.mtxtEntityName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtEntityName.SelectedText = "";
            this.mtxtEntityName.SelectionLength = 0;
            this.mtxtEntityName.SelectionStart = 0;
            this.mtxtEntityName.ShortcutsEnabled = true;
            this.mtxtEntityName.UseSelectable = true;
            this.mtxtEntityName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtEntityName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblEntityName
            // 
            this.mlblEntityName.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblEntityName, "mlblEntityName");
            this.mlblEntityName.Name = "mlblEntityName";
            // 
            // mtxtProjectName
            // 
            // 
            // 
            // 
            this.mtxtProjectName.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image8")));
            this.mtxtProjectName.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode8")));
            this.mtxtProjectName.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location8")));
            this.mtxtProjectName.CustomButton.Name = "";
            this.mtxtProjectName.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size8")));
            this.mtxtProjectName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtProjectName.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex8")));
            this.mtxtProjectName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtProjectName.CustomButton.UseSelectable = true;
            this.mtxtProjectName.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible8")));
            this.mtxtProjectName.Lines = new string[0];
            resources.ApplyResources(this.mtxtProjectName, "mtxtProjectName");
            this.mtxtProjectName.MaxLength = 32767;
            this.mtxtProjectName.Name = "mtxtProjectName";
            this.mtxtProjectName.PasswordChar = '\0';
            this.mtxtProjectName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtProjectName.SelectedText = "";
            this.mtxtProjectName.SelectionLength = 0;
            this.mtxtProjectName.SelectionStart = 0;
            this.mtxtProjectName.ShortcutsEnabled = true;
            this.mtxtProjectName.UseSelectable = true;
            this.mtxtProjectName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtProjectName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblProjectName
            // 
            this.mlblProjectName.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblProjectName, "mlblProjectName");
            this.mlblProjectName.Name = "mlblProjectName";
            // 
            // RegisterInstitutionControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "RegisterInstitutionControl";
            this.Load += new System.EventHandler(this.RegisterInstitution_Load);
            this.Controls.SetChildIndex(this.tlpMain, 0);
            this.tlpMain.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.pnContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnContent;
        private MetroFramework.Controls.MetroTextBox mtxtLocalInitiative;
        private MetroFramework.Controls.MetroLabel mlblLocalInitiative;
        private MetroFramework.Controls.MetroTextBox mtxtEntityName;
        private MetroFramework.Controls.MetroLabel mlblEntityName;
        private MetroFramework.Controls.MetroTextBox mtxtProjectName;
        private MetroFramework.Controls.MetroLabel mlblProjectName;
        private MetroFramework.Controls.MetroLabel mlblTaxId;
        private MetroFramework.Controls.MetroCheckBox mcbInstitutionalized;
        private MetroFramework.Controls.MetroComboBox mcbCoordinator;
        private MetroFramework.Controls.MetroLabel mlblCoordinator;
        private MetroFramework.Controls.MetroLabel mlblZipCode;
        private MetroFramework.Controls.MetroLabel mlblState;
        private MetroFramework.Controls.MetroTextBox mtxtCity;
        private MetroFramework.Controls.MetroLabel mlblCity;
        private MetroFramework.Controls.MetroTextBox mtxtDistrict;
        private MetroFramework.Controls.MetroLabel mlblDistrict;
        private MetroFramework.Controls.MetroTextBox mtxtAddress;
        private MetroFramework.Controls.MetroLabel mlblAddress;
        private MetroFramework.Controls.MetroLabel mlblPhone;
        private MetroFramework.Controls.MetroLabel mlblMobile;
        private MetroFramework.Controls.MetroTextBox mtxtSite;
        private MetroFramework.Controls.MetroLabel mlblSite;
        private MetroFramework.Controls.MetroTextBox mtxtEmail;
        private MetroFramework.Controls.MetroLabel mlblEmail;
        private MetroFramework.Controls.MetroTextBox mtxtDescription;
        private MetroFramework.Controls.MetroLabel mlblDescription;
        private MetroFramework.Controls.MetroComboBox mcbStatus;
        private MetroFramework.Controls.MetroLabel mlblStatus;
        private MetroFramework.Controls.MetroComboBox mcbState;
        private System.Windows.Forms.MaskedTextBox mtxtZipCode;
        private System.Windows.Forms.MaskedTextBox mtxtTaxId;
        private System.Windows.Forms.MaskedTextBox mtxtPhone;
        private System.Windows.Forms.MaskedTextBox mtxtMobile;
    }
}
