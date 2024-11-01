namespace PnT.SongClient.UI.Controls
{
    partial class RegisterPoleControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterPoleControl));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mtbTabManager = new MetroFramework.Controls.MetroTabControl();
            this.tbGeneralData = new System.Windows.Forms.TabPage();
            this.pnGeneralData = new System.Windows.Forms.Panel();
            this.pnContent = new System.Windows.Forms.Panel();
            this.mcbState = new MetroFramework.Controls.MetroComboBox();
            this.mtxtMobile = new System.Windows.Forms.MaskedTextBox();
            this.mtxtPhone = new System.Windows.Forms.MaskedTextBox();
            this.mtxtZipCode = new System.Windows.Forms.MaskedTextBox();
            this.mcbStatus = new MetroFramework.Controls.MetroComboBox();
            this.mlblStatus = new MetroFramework.Controls.MetroLabel();
            this.mtxtDescription = new MetroFramework.Controls.MetroTextBox();
            this.mlblDescription = new MetroFramework.Controls.MetroLabel();
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
            this.mcbInstitution = new MetroFramework.Controls.MetroComboBox();
            this.mlblInstitution = new MetroFramework.Controls.MetroLabel();
            this.mtxtName = new MetroFramework.Controls.MetroTextBox();
            this.mlblName = new MetroFramework.Controls.MetroLabel();
            this.tbTeachers = new System.Windows.Forms.TabPage();
            this.tlpTeachers = new System.Windows.Forms.TableLayoutPanel();
            this.mlblAvailableTeachers = new MetroFramework.Controls.MetroLabel();
            this.mlblSelectedTeachers = new MetroFramework.Controls.MetroLabel();
            this.lbAvailableTeachers = new System.Windows.Forms.ListBox();
            this.lbSelectedTeachers = new System.Windows.Forms.ListBox();
            this.mbtnAddTeachers = new MetroFramework.Controls.MetroButton();
            this.mbtnRemoveTeachers = new MetroFramework.Controls.MetroButton();
            this.tlpMain.SuspendLayout();
            this.mtbTabManager.SuspendLayout();
            this.tbGeneralData.SuspendLayout();
            this.pnGeneralData.SuspendLayout();
            this.pnContent.SuspendLayout();
            this.tbTeachers.SuspendLayout();
            this.tlpTeachers.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mtbTabManager, 2, 0);
            this.tlpMain.Name = "tlpMain";
            // 
            // mtbTabManager
            // 
            this.mtbTabManager.Controls.Add(this.tbGeneralData);
            this.mtbTabManager.Controls.Add(this.tbTeachers);
            resources.ApplyResources(this.mtbTabManager, "mtbTabManager");
            this.mtbTabManager.Name = "mtbTabManager";
            this.mtbTabManager.SelectedIndex = 1;
            this.mtbTabManager.UseSelectable = true;
            // 
            // tbGeneralData
            // 
            this.tbGeneralData.BackColor = System.Drawing.Color.White;
            this.tbGeneralData.Controls.Add(this.pnGeneralData);
            resources.ApplyResources(this.tbGeneralData, "tbGeneralData");
            this.tbGeneralData.Name = "tbGeneralData";
            // 
            // pnGeneralData
            // 
            this.pnGeneralData.Controls.Add(this.pnContent);
            resources.ApplyResources(this.pnGeneralData, "pnGeneralData");
            this.pnGeneralData.Name = "pnGeneralData";
            // 
            // pnContent
            // 
            this.pnContent.BackColor = System.Drawing.Color.White;
            this.pnContent.Controls.Add(this.mcbState);
            this.pnContent.Controls.Add(this.mtxtMobile);
            this.pnContent.Controls.Add(this.mtxtPhone);
            this.pnContent.Controls.Add(this.mtxtZipCode);
            this.pnContent.Controls.Add(this.mcbStatus);
            this.pnContent.Controls.Add(this.mlblStatus);
            this.pnContent.Controls.Add(this.mtxtDescription);
            this.pnContent.Controls.Add(this.mlblDescription);
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
            this.pnContent.Controls.Add(this.mcbInstitution);
            this.pnContent.Controls.Add(this.mlblInstitution);
            this.pnContent.Controls.Add(this.mtxtName);
            this.pnContent.Controls.Add(this.mlblName);
            resources.ApplyResources(this.pnContent, "pnContent");
            this.pnContent.Name = "pnContent";
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
            // mtxtZipCode
            // 
            this.mtxtZipCode.BackColor = System.Drawing.Color.White;
            this.mtxtZipCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.mtxtZipCode, "mtxtZipCode");
            this.mtxtZipCode.Name = "mtxtZipCode";
            this.mtxtZipCode.ValidatingType = typeof(System.DateTime);
            this.mtxtZipCode.Click += new System.EventHandler(this.mtxtZipCode_Click);
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
            // mtxtEmail
            // 
            // 
            // 
            // 
            this.mtxtEmail.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            this.mtxtEmail.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode1")));
            this.mtxtEmail.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location1")));
            this.mtxtEmail.CustomButton.Name = "";
            this.mtxtEmail.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size1")));
            this.mtxtEmail.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtEmail.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex1")));
            this.mtxtEmail.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtEmail.CustomButton.UseSelectable = true;
            this.mtxtEmail.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible1")));
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
            this.mtxtCity.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            this.mtxtCity.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode2")));
            this.mtxtCity.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location2")));
            this.mtxtCity.CustomButton.Name = "";
            this.mtxtCity.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size2")));
            this.mtxtCity.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtCity.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex2")));
            this.mtxtCity.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtCity.CustomButton.UseSelectable = true;
            this.mtxtCity.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible2")));
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
            this.mtxtDistrict.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image3")));
            this.mtxtDistrict.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode3")));
            this.mtxtDistrict.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location3")));
            this.mtxtDistrict.CustomButton.Name = "";
            this.mtxtDistrict.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size3")));
            this.mtxtDistrict.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtDistrict.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex3")));
            this.mtxtDistrict.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtDistrict.CustomButton.UseSelectable = true;
            this.mtxtDistrict.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible3")));
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
            this.mtxtAddress.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image4")));
            this.mtxtAddress.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode4")));
            this.mtxtAddress.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location4")));
            this.mtxtAddress.CustomButton.Name = "";
            this.mtxtAddress.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size4")));
            this.mtxtAddress.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtAddress.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex4")));
            this.mtxtAddress.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtAddress.CustomButton.UseSelectable = true;
            this.mtxtAddress.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible4")));
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
            // mtxtName
            // 
            // 
            // 
            // 
            this.mtxtName.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image5")));
            this.mtxtName.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode5")));
            this.mtxtName.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location5")));
            this.mtxtName.CustomButton.Name = "";
            this.mtxtName.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size5")));
            this.mtxtName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtName.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex5")));
            this.mtxtName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtName.CustomButton.UseSelectable = true;
            this.mtxtName.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible5")));
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
            // tbTeachers
            // 
            this.tbTeachers.BackColor = System.Drawing.Color.White;
            this.tbTeachers.Controls.Add(this.tlpTeachers);
            resources.ApplyResources(this.tbTeachers, "tbTeachers");
            this.tbTeachers.Name = "tbTeachers";
            // 
            // tlpTeachers
            // 
            resources.ApplyResources(this.tlpTeachers, "tlpTeachers");
            this.tlpTeachers.Controls.Add(this.mlblAvailableTeachers, 0, 1);
            this.tlpTeachers.Controls.Add(this.mlblSelectedTeachers, 2, 1);
            this.tlpTeachers.Controls.Add(this.lbAvailableTeachers, 0, 2);
            this.tlpTeachers.Controls.Add(this.lbSelectedTeachers, 2, 2);
            this.tlpTeachers.Controls.Add(this.mbtnAddTeachers, 1, 2);
            this.tlpTeachers.Controls.Add(this.mbtnRemoveTeachers, 1, 3);
            this.tlpTeachers.Name = "tlpTeachers";
            // 
            // mlblAvailableTeachers
            // 
            resources.ApplyResources(this.mlblAvailableTeachers, "mlblAvailableTeachers");
            this.mlblAvailableTeachers.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblAvailableTeachers.Name = "mlblAvailableTeachers";
            // 
            // mlblSelectedTeachers
            // 
            resources.ApplyResources(this.mlblSelectedTeachers, "mlblSelectedTeachers");
            this.mlblSelectedTeachers.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblSelectedTeachers.Name = "mlblSelectedTeachers";
            // 
            // lbAvailableTeachers
            // 
            this.lbAvailableTeachers.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.lbAvailableTeachers, "lbAvailableTeachers");
            this.lbAvailableTeachers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbAvailableTeachers.FormattingEnabled = true;
            this.lbAvailableTeachers.Name = "lbAvailableTeachers";
            this.tlpTeachers.SetRowSpan(this.lbAvailableTeachers, 4);
            this.lbAvailableTeachers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbAvailableTeachers.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbTeachers_DrawItem);
            this.lbAvailableTeachers.SelectedIndexChanged += new System.EventHandler(this.lbAvailableTeachers_SelectedIndexChanged);
            // 
            // lbSelectedTeachers
            // 
            this.lbSelectedTeachers.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.lbSelectedTeachers, "lbSelectedTeachers");
            this.lbSelectedTeachers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbSelectedTeachers.FormattingEnabled = true;
            this.lbSelectedTeachers.Name = "lbSelectedTeachers";
            this.tlpTeachers.SetRowSpan(this.lbSelectedTeachers, 4);
            this.lbSelectedTeachers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbSelectedTeachers.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbTeachers_DrawItem);
            this.lbSelectedTeachers.SelectedIndexChanged += new System.EventHandler(this.lbSelectedTeachers_SelectedIndexChanged);
            // 
            // mbtnAddTeachers
            // 
            resources.ApplyResources(this.mbtnAddTeachers, "mbtnAddTeachers");
            this.mbtnAddTeachers.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveRightOne;
            this.mbtnAddTeachers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnAddTeachers.Name = "mbtnAddTeachers";
            this.mbtnAddTeachers.UseSelectable = true;
            this.mbtnAddTeachers.Click += new System.EventHandler(this.mbtnAddTeachers_Click);
            // 
            // mbtnRemoveTeachers
            // 
            resources.ApplyResources(this.mbtnRemoveTeachers, "mbtnRemoveTeachers");
            this.mbtnRemoveTeachers.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconMoveLeftOne;
            this.mbtnRemoveTeachers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mbtnRemoveTeachers.Name = "mbtnRemoveTeachers";
            this.mbtnRemoveTeachers.UseSelectable = true;
            this.mbtnRemoveTeachers.Click += new System.EventHandler(this.mbtnRemoveTeachers_Click);
            // 
            // RegisterPoleControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "RegisterPoleControl";
            this.Load += new System.EventHandler(this.RegisterPole_Load);
            this.Controls.SetChildIndex(this.tlpMain, 0);
            this.tlpMain.ResumeLayout(false);
            this.mtbTabManager.ResumeLayout(false);
            this.tbGeneralData.ResumeLayout(false);
            this.pnGeneralData.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.pnContent.PerformLayout();
            this.tbTeachers.ResumeLayout(false);
            this.tlpTeachers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnContent;
        private MetroFramework.Controls.MetroComboBox mcbStatus;
        private MetroFramework.Controls.MetroLabel mlblStatus;
        private MetroFramework.Controls.MetroTextBox mtxtDescription;
        private MetroFramework.Controls.MetroLabel mlblDescription;
        private MetroFramework.Controls.MetroTextBox mtxtEmail;
        private MetroFramework.Controls.MetroLabel mlblEmail;
        private MetroFramework.Controls.MetroLabel mlblMobile;
        private MetroFramework.Controls.MetroLabel mlblPhone;
        private MetroFramework.Controls.MetroLabel mlblZipCode;
        private MetroFramework.Controls.MetroLabel mlblState;
        private MetroFramework.Controls.MetroTextBox mtxtCity;
        private MetroFramework.Controls.MetroLabel mlblCity;
        private MetroFramework.Controls.MetroTextBox mtxtDistrict;
        private MetroFramework.Controls.MetroLabel mlblDistrict;
        private MetroFramework.Controls.MetroTextBox mtxtAddress;
        private MetroFramework.Controls.MetroLabel mlblAddress;
        private MetroFramework.Controls.MetroComboBox mcbInstitution;
        private MetroFramework.Controls.MetroLabel mlblInstitution;
        private MetroFramework.Controls.MetroTextBox mtxtName;
        private MetroFramework.Controls.MetroLabel mlblName;
        private System.Windows.Forms.MaskedTextBox mtxtZipCode;
        private System.Windows.Forms.MaskedTextBox mtxtMobile;
        private System.Windows.Forms.MaskedTextBox mtxtPhone;
        private MetroFramework.Controls.MetroComboBox mcbState;
        private MetroFramework.Controls.MetroTabControl mtbTabManager;
        private System.Windows.Forms.TabPage tbGeneralData;
        private System.Windows.Forms.Panel pnGeneralData;
        private System.Windows.Forms.TabPage tbTeachers;
        private System.Windows.Forms.TableLayoutPanel tlpTeachers;
        private MetroFramework.Controls.MetroLabel mlblAvailableTeachers;
        private MetroFramework.Controls.MetroLabel mlblSelectedTeachers;
        private System.Windows.Forms.ListBox lbAvailableTeachers;
        private System.Windows.Forms.ListBox lbSelectedTeachers;
        private MetroFramework.Controls.MetroButton mbtnAddTeachers;
        private MetroFramework.Controls.MetroButton mbtnRemoveTeachers;
    }
}
