namespace PnT.SongClient.UI.Controls
{
    partial class RegisterInstrumentControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterInstrumentControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mtbTabManager = new MetroFramework.Controls.MetroTabControl();
            this.tbGeneralData = new System.Windows.Forms.TabPage();
            this.tlpGeneralData = new System.Windows.Forms.TableLayoutPanel();
            this.dgvComments = new System.Windows.Forms.DataGridView();
            this.CommentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CommentText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnComment = new System.Windows.Forms.Panel();
            this.mbtnAddComment = new MetroFramework.Controls.MetroButton();
            this.mtxtComment = new MetroFramework.Controls.MetroTextBox();
            this.mlblComment = new MetroFramework.Controls.MetroLabel();
            this.pnContent = new System.Windows.Forms.Panel();
            this.mcbInstrumentType = new MetroFramework.Controls.MetroComboBox();
            this.mlblInstrumentType = new MetroFramework.Controls.MetroLabel();
            this.mcbStatus = new MetroFramework.Controls.MetroComboBox();
            this.mlblStatus = new MetroFramework.Controls.MetroLabel();
            this.mtxtStorageLocation = new MetroFramework.Controls.MetroTextBox();
            this.mlblStorageLocation = new MetroFramework.Controls.MetroLabel();
            this.mtxtCode = new MetroFramework.Controls.MetroTextBox();
            this.mlblCode = new MetroFramework.Controls.MetroLabel();
            this.mcbPole = new MetroFramework.Controls.MetroComboBox();
            this.mlblPole = new MetroFramework.Controls.MetroLabel();
            this.mtxtModel = new MetroFramework.Controls.MetroTextBox();
            this.mlblModel = new MetroFramework.Controls.MetroLabel();
            this.tbLoans = new System.Windows.Forms.TabPage();
            this.tlpLoan = new System.Windows.Forms.TableLayoutPanel();
            this.pnNewLoan = new System.Windows.Forms.Panel();
            this.mbtnAddLoan = new MetroFramework.Controls.MetroButton();
            this.mlblAddLoan = new MetroFramework.Controls.MetroLabel();
            this.dgvLoans = new System.Windows.Forms.DataGridView();
            this.LoanId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loanStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loanStudent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loanStartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loanEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnLoan = new System.Windows.Forms.Panel();
            this.mtxtLoanEndDate = new System.Windows.Forms.MaskedTextBox();
            this.mlblLoanEndDate = new MetroFramework.Controls.MetroLabel();
            this.mtxtLoanStartDate = new System.Windows.Forms.MaskedTextBox();
            this.mlblLoanStartDate = new MetroFramework.Controls.MetroLabel();
            this.mcbLoanStudent = new MetroFramework.Controls.MetroComboBox();
            this.mlblLoanStudent = new MetroFramework.Controls.MetroLabel();
            this.mcbLoanStatus = new MetroFramework.Controls.MetroComboBox();
            this.mlblLoanStatus = new MetroFramework.Controls.MetroLabel();
            this.mcmComment = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.mnuEditComment = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteComment = new System.Windows.Forms.ToolStripMenuItem();
            this.mbtnDeleteLoan = new MetroFramework.Controls.MetroButton();
            this.tlpMain.SuspendLayout();
            this.mtbTabManager.SuspendLayout();
            this.tbGeneralData.SuspendLayout();
            this.tlpGeneralData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComments)).BeginInit();
            this.pnComment.SuspendLayout();
            this.pnContent.SuspendLayout();
            this.tbLoans.SuspendLayout();
            this.tlpLoan.SuspendLayout();
            this.pnNewLoan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoans)).BeginInit();
            this.pnLoan.SuspendLayout();
            this.mcmComment.SuspendLayout();
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
            this.mtbTabManager.Controls.Add(this.tbLoans);
            resources.ApplyResources(this.mtbTabManager, "mtbTabManager");
            this.mtbTabManager.Name = "mtbTabManager";
            this.mtbTabManager.SelectedIndex = 1;
            this.mtbTabManager.UseSelectable = true;
            // 
            // tbGeneralData
            // 
            this.tbGeneralData.BackColor = System.Drawing.Color.White;
            this.tbGeneralData.Controls.Add(this.tlpGeneralData);
            resources.ApplyResources(this.tbGeneralData, "tbGeneralData");
            this.tbGeneralData.Name = "tbGeneralData";
            // 
            // tlpGeneralData
            // 
            resources.ApplyResources(this.tlpGeneralData, "tlpGeneralData");
            this.tlpGeneralData.Controls.Add(this.dgvComments, 0, 2);
            this.tlpGeneralData.Controls.Add(this.pnComment, 0, 1);
            this.tlpGeneralData.Controls.Add(this.pnContent, 0, 0);
            this.tlpGeneralData.Name = "tlpGeneralData";
            // 
            // dgvComments
            // 
            this.dgvComments.AllowUserToAddRows = false;
            this.dgvComments.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.dgvComments.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvComments.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgvComments.BackgroundColor = System.Drawing.Color.White;
            this.dgvComments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvComments.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvComments.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvComments.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvComments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvComments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComments.ColumnHeadersVisible = false;
            this.dgvComments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CommentDate,
            this.CommentText});
            this.tlpGeneralData.SetColumnSpan(this.dgvComments, 2);
            this.dgvComments.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvComments.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.dgvComments, "dgvComments");
            this.dgvComments.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvComments.MultiSelect = false;
            this.dgvComments.Name = "dgvComments";
            this.dgvComments.ReadOnly = true;
            this.dgvComments.RowHeadersVisible = false;
            this.dgvComments.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvComments.RowTemplate.Height = 44;
            this.dgvComments.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvComments_KeyUp);
            this.dgvComments.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvComments_MouseUp);
            // 
            // CommentDate
            // 
            this.CommentDate.DataPropertyName = "Date";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle3.Format = "d";
            dataGridViewCellStyle3.NullValue = null;
            this.CommentDate.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.CommentDate, "CommentDate");
            this.CommentDate.Name = "CommentDate";
            this.CommentDate.ReadOnly = true;
            // 
            // CommentText
            // 
            this.CommentText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CommentText.DataPropertyName = "Text";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CommentText.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.CommentText, "CommentText");
            this.CommentText.Name = "CommentText";
            this.CommentText.ReadOnly = true;
            // 
            // pnComment
            // 
            this.pnComment.Controls.Add(this.mbtnAddComment);
            this.pnComment.Controls.Add(this.mtxtComment);
            this.pnComment.Controls.Add(this.mlblComment);
            resources.ApplyResources(this.pnComment, "pnComment");
            this.pnComment.Name = "pnComment";
            // 
            // mbtnAddComment
            // 
            resources.ApplyResources(this.mbtnAddComment, "mbtnAddComment");
            this.mbtnAddComment.Name = "mbtnAddComment";
            this.mbtnAddComment.UseSelectable = true;
            this.mbtnAddComment.Click += new System.EventHandler(this.mbtnAddComment_Click);
            // 
            // mtxtComment
            // 
            // 
            // 
            // 
            this.mtxtComment.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.mtxtComment.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode")));
            this.mtxtComment.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location")));
            this.mtxtComment.CustomButton.Name = "";
            this.mtxtComment.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size")));
            this.mtxtComment.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtComment.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex")));
            this.mtxtComment.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtComment.CustomButton.UseSelectable = true;
            this.mtxtComment.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible")));
            this.mtxtComment.Lines = new string[0];
            resources.ApplyResources(this.mtxtComment, "mtxtComment");
            this.mtxtComment.MaxLength = 32767;
            this.mtxtComment.Name = "mtxtComment";
            this.mtxtComment.PasswordChar = '\0';
            this.mtxtComment.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtComment.SelectedText = "";
            this.mtxtComment.SelectionLength = 0;
            this.mtxtComment.SelectionStart = 0;
            this.mtxtComment.ShortcutsEnabled = true;
            this.mtxtComment.UseSelectable = true;
            this.mtxtComment.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtComment.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.mtxtComment.TextChanged += new System.EventHandler(this.mtxtComment_TextChanged);
            // 
            // mlblComment
            // 
            this.mlblComment.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblComment, "mlblComment");
            this.mlblComment.Name = "mlblComment";
            // 
            // pnContent
            // 
            this.pnContent.BackColor = System.Drawing.Color.White;
            this.pnContent.Controls.Add(this.mcbInstrumentType);
            this.pnContent.Controls.Add(this.mlblInstrumentType);
            this.pnContent.Controls.Add(this.mcbStatus);
            this.pnContent.Controls.Add(this.mlblStatus);
            this.pnContent.Controls.Add(this.mtxtStorageLocation);
            this.pnContent.Controls.Add(this.mlblStorageLocation);
            this.pnContent.Controls.Add(this.mtxtCode);
            this.pnContent.Controls.Add(this.mlblCode);
            this.pnContent.Controls.Add(this.mcbPole);
            this.pnContent.Controls.Add(this.mlblPole);
            this.pnContent.Controls.Add(this.mtxtModel);
            this.pnContent.Controls.Add(this.mlblModel);
            resources.ApplyResources(this.pnContent, "pnContent");
            this.pnContent.Name = "pnContent";
            // 
            // mcbInstrumentType
            // 
            this.mcbInstrumentType.DisplayMember = "Description";
            this.mcbInstrumentType.DropDownWidth = 150;
            this.mcbInstrumentType.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbInstrumentType.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbInstrumentType.FormattingEnabled = true;
            resources.ApplyResources(this.mcbInstrumentType, "mcbInstrumentType");
            this.mcbInstrumentType.Name = "mcbInstrumentType";
            this.mcbInstrumentType.UseSelectable = true;
            this.mcbInstrumentType.ValueMember = "Id";
            // 
            // mlblInstrumentType
            // 
            this.mlblInstrumentType.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblInstrumentType, "mlblInstrumentType");
            this.mlblInstrumentType.Name = "mlblInstrumentType";
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
            // mtxtStorageLocation
            // 
            // 
            // 
            // 
            this.mtxtStorageLocation.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            this.mtxtStorageLocation.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode1")));
            this.mtxtStorageLocation.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location1")));
            this.mtxtStorageLocation.CustomButton.Name = "";
            this.mtxtStorageLocation.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size1")));
            this.mtxtStorageLocation.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtStorageLocation.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex1")));
            this.mtxtStorageLocation.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtStorageLocation.CustomButton.UseSelectable = true;
            this.mtxtStorageLocation.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible1")));
            this.mtxtStorageLocation.Lines = new string[0];
            resources.ApplyResources(this.mtxtStorageLocation, "mtxtStorageLocation");
            this.mtxtStorageLocation.MaxLength = 32767;
            this.mtxtStorageLocation.Name = "mtxtStorageLocation";
            this.mtxtStorageLocation.PasswordChar = '\0';
            this.mtxtStorageLocation.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtStorageLocation.SelectedText = "";
            this.mtxtStorageLocation.SelectionLength = 0;
            this.mtxtStorageLocation.SelectionStart = 0;
            this.mtxtStorageLocation.ShortcutsEnabled = true;
            this.mtxtStorageLocation.UseSelectable = true;
            this.mtxtStorageLocation.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtStorageLocation.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblStorageLocation
            // 
            this.mlblStorageLocation.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblStorageLocation, "mlblStorageLocation");
            this.mlblStorageLocation.Name = "mlblStorageLocation";
            // 
            // mtxtCode
            // 
            // 
            // 
            // 
            this.mtxtCode.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            this.mtxtCode.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode2")));
            this.mtxtCode.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location2")));
            this.mtxtCode.CustomButton.Name = "";
            this.mtxtCode.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size2")));
            this.mtxtCode.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtCode.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex2")));
            this.mtxtCode.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtCode.CustomButton.UseSelectable = true;
            this.mtxtCode.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible2")));
            this.mtxtCode.Lines = new string[0];
            resources.ApplyResources(this.mtxtCode, "mtxtCode");
            this.mtxtCode.MaxLength = 32767;
            this.mtxtCode.Name = "mtxtCode";
            this.mtxtCode.PasswordChar = '\0';
            this.mtxtCode.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtCode.SelectedText = "";
            this.mtxtCode.SelectionLength = 0;
            this.mtxtCode.SelectionStart = 0;
            this.mtxtCode.ShortcutsEnabled = true;
            this.mtxtCode.UseSelectable = true;
            this.mtxtCode.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtCode.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblCode
            // 
            this.mlblCode.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblCode, "mlblCode");
            this.mlblCode.Name = "mlblCode";
            // 
            // mcbPole
            // 
            this.mcbPole.DisplayMember = "Description";
            this.mcbPole.DropDownWidth = 240;
            this.mcbPole.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbPole.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbPole.FormattingEnabled = true;
            resources.ApplyResources(this.mcbPole, "mcbPole");
            this.mcbPole.Name = "mcbPole";
            this.mcbPole.UseSelectable = true;
            this.mcbPole.ValueMember = "Id";
            this.mcbPole.SelectedIndexChanged += new System.EventHandler(this.mcbPole_SelectedIndexChanged);
            // 
            // mlblPole
            // 
            this.mlblPole.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblPole, "mlblPole");
            this.mlblPole.Name = "mlblPole";
            // 
            // mtxtModel
            // 
            // 
            // 
            // 
            this.mtxtModel.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image3")));
            this.mtxtModel.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode3")));
            this.mtxtModel.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location3")));
            this.mtxtModel.CustomButton.Name = "";
            this.mtxtModel.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size3")));
            this.mtxtModel.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtModel.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex3")));
            this.mtxtModel.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtModel.CustomButton.UseSelectable = true;
            this.mtxtModel.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible3")));
            this.mtxtModel.Lines = new string[0];
            resources.ApplyResources(this.mtxtModel, "mtxtModel");
            this.mtxtModel.MaxLength = 32767;
            this.mtxtModel.Name = "mtxtModel";
            this.mtxtModel.PasswordChar = '\0';
            this.mtxtModel.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtModel.SelectedText = "";
            this.mtxtModel.SelectionLength = 0;
            this.mtxtModel.SelectionStart = 0;
            this.mtxtModel.ShortcutsEnabled = true;
            this.mtxtModel.UseSelectable = true;
            this.mtxtModel.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtModel.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mlblModel
            // 
            this.mlblModel.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblModel, "mlblModel");
            this.mlblModel.Name = "mlblModel";
            // 
            // tbLoans
            // 
            this.tbLoans.BackColor = System.Drawing.Color.White;
            this.tbLoans.Controls.Add(this.tlpLoan);
            resources.ApplyResources(this.tbLoans, "tbLoans");
            this.tbLoans.Name = "tbLoans";
            // 
            // tlpLoan
            // 
            resources.ApplyResources(this.tlpLoan, "tlpLoan");
            this.tlpLoan.Controls.Add(this.mbtnDeleteLoan, 1, 1);
            this.tlpLoan.Controls.Add(this.pnNewLoan, 0, 0);
            this.tlpLoan.Controls.Add(this.dgvLoans, 0, 1);
            this.tlpLoan.Controls.Add(this.pnLoan, 0, 2);
            this.tlpLoan.Name = "tlpLoan";
            // 
            // pnNewLoan
            // 
            this.pnNewLoan.Controls.Add(this.mbtnAddLoan);
            this.pnNewLoan.Controls.Add(this.mlblAddLoan);
            resources.ApplyResources(this.pnNewLoan, "pnNewLoan");
            this.pnNewLoan.Name = "pnNewLoan";
            // 
            // mbtnAddLoan
            // 
            resources.ApplyResources(this.mbtnAddLoan, "mbtnAddLoan");
            this.mbtnAddLoan.Name = "mbtnAddLoan";
            this.mbtnAddLoan.UseSelectable = true;
            this.mbtnAddLoan.Click += new System.EventHandler(this.mbtnAddLoan_Click);
            // 
            // mlblAddLoan
            // 
            this.mlblAddLoan.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblAddLoan, "mlblAddLoan");
            this.mlblAddLoan.Name = "mlblAddLoan";
            // 
            // dgvLoans
            // 
            this.dgvLoans.AllowUserToAddRows = false;
            this.dgvLoans.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.dgvLoans.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvLoans.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgvLoans.BackgroundColor = System.Drawing.Color.White;
            this.dgvLoans.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLoans.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvLoans.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvLoans.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLoans.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvLoans.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLoans.ColumnHeadersVisible = false;
            this.dgvLoans.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LoanId,
            this.loanStatus,
            this.loanStudent,
            this.loanStartDate,
            this.loanEndDate});
            this.dgvLoans.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLoans.DefaultCellStyle = dataGridViewCellStyle12;
            resources.ApplyResources(this.dgvLoans, "dgvLoans");
            this.dgvLoans.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLoans.MultiSelect = false;
            this.dgvLoans.Name = "dgvLoans";
            this.dgvLoans.ReadOnly = true;
            this.dgvLoans.RowHeadersVisible = false;
            this.dgvLoans.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvLoans.RowTemplate.Height = 44;
            this.dgvLoans.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLoans.SelectionChanged += new System.EventHandler(this.dgvLoans_SelectionChanged);
            // 
            // LoanId
            // 
            this.LoanId.DataPropertyName = "LoanId";
            resources.ApplyResources(this.LoanId, "LoanId");
            this.LoanId.Name = "LoanId";
            this.LoanId.ReadOnly = true;
            // 
            // loanStatus
            // 
            this.loanStatus.DataPropertyName = "LoanStatusName";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle8.NullValue = null;
            this.loanStatus.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.loanStatus, "loanStatus");
            this.loanStatus.Name = "loanStatus";
            this.loanStatus.ReadOnly = true;
            // 
            // loanStudent
            // 
            this.loanStudent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.loanStudent.DataPropertyName = "StudentName";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.loanStudent.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.loanStudent, "loanStudent");
            this.loanStudent.Name = "loanStudent";
            this.loanStudent.ReadOnly = true;
            // 
            // loanStartDate
            // 
            this.loanStartDate.DataPropertyName = "StartDate";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle10.Format = "d";
            dataGridViewCellStyle10.NullValue = null;
            this.loanStartDate.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.loanStartDate, "loanStartDate");
            this.loanStartDate.Name = "loanStartDate";
            this.loanStartDate.ReadOnly = true;
            // 
            // loanEndDate
            // 
            this.loanEndDate.DataPropertyName = "EndDate";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle11.Format = "d";
            dataGridViewCellStyle11.NullValue = "-";
            this.loanEndDate.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.loanEndDate, "loanEndDate");
            this.loanEndDate.Name = "loanEndDate";
            this.loanEndDate.ReadOnly = true;
            // 
            // pnLoan
            // 
            this.tlpLoan.SetColumnSpan(this.pnLoan, 2);
            this.pnLoan.Controls.Add(this.mtxtLoanEndDate);
            this.pnLoan.Controls.Add(this.mlblLoanEndDate);
            this.pnLoan.Controls.Add(this.mtxtLoanStartDate);
            this.pnLoan.Controls.Add(this.mlblLoanStartDate);
            this.pnLoan.Controls.Add(this.mcbLoanStudent);
            this.pnLoan.Controls.Add(this.mlblLoanStudent);
            this.pnLoan.Controls.Add(this.mcbLoanStatus);
            this.pnLoan.Controls.Add(this.mlblLoanStatus);
            resources.ApplyResources(this.pnLoan, "pnLoan");
            this.pnLoan.Name = "pnLoan";
            // 
            // mtxtLoanEndDate
            // 
            this.mtxtLoanEndDate.BackColor = System.Drawing.Color.White;
            this.mtxtLoanEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.mtxtLoanEndDate, "mtxtLoanEndDate");
            this.mtxtLoanEndDate.Name = "mtxtLoanEndDate";
            this.mtxtLoanEndDate.ValidatingType = typeof(System.DateTime);
            this.mtxtLoanEndDate.Click += new System.EventHandler(this.Date_Click);
            this.mtxtLoanEndDate.TextChanged += new System.EventHandler(this.mtxtLoanEndDate_TextChanged);
            // 
            // mlblLoanEndDate
            // 
            this.mlblLoanEndDate.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblLoanEndDate, "mlblLoanEndDate");
            this.mlblLoanEndDate.Name = "mlblLoanEndDate";
            // 
            // mtxtLoanStartDate
            // 
            this.mtxtLoanStartDate.BackColor = System.Drawing.Color.White;
            this.mtxtLoanStartDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.mtxtLoanStartDate, "mtxtLoanStartDate");
            this.mtxtLoanStartDate.Name = "mtxtLoanStartDate";
            this.mtxtLoanStartDate.ValidatingType = typeof(System.DateTime);
            this.mtxtLoanStartDate.Click += new System.EventHandler(this.Date_Click);
            this.mtxtLoanStartDate.TextChanged += new System.EventHandler(this.mtxtLoanStartDate_TextChanged);
            // 
            // mlblLoanStartDate
            // 
            this.mlblLoanStartDate.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblLoanStartDate, "mlblLoanStartDate");
            this.mlblLoanStartDate.Name = "mlblLoanStartDate";
            // 
            // mcbLoanStudent
            // 
            this.mcbLoanStudent.DisplayMember = "Description";
            this.mcbLoanStudent.DropDownWidth = 150;
            this.mcbLoanStudent.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbLoanStudent.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbLoanStudent.FormattingEnabled = true;
            resources.ApplyResources(this.mcbLoanStudent, "mcbLoanStudent");
            this.mcbLoanStudent.Name = "mcbLoanStudent";
            this.mcbLoanStudent.UseSelectable = true;
            this.mcbLoanStudent.ValueMember = "Id";
            this.mcbLoanStudent.SelectedIndexChanged += new System.EventHandler(this.mcbLoanStudent_SelectedIndexChanged);
            // 
            // mlblLoanStudent
            // 
            this.mlblLoanStudent.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblLoanStudent, "mlblLoanStudent");
            this.mlblLoanStudent.Name = "mlblLoanStudent";
            // 
            // mcbLoanStatus
            // 
            this.mcbLoanStatus.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbLoanStatus.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbLoanStatus.FormattingEnabled = true;
            resources.ApplyResources(this.mcbLoanStatus, "mcbLoanStatus");
            this.mcbLoanStatus.Name = "mcbLoanStatus";
            this.mcbLoanStatus.UseSelectable = true;
            this.mcbLoanStatus.SelectedIndexChanged += new System.EventHandler(this.mcbLoanStatus_SelectedIndexChanged);
            // 
            // mlblLoanStatus
            // 
            this.mlblLoanStatus.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblLoanStatus, "mlblLoanStatus");
            this.mlblLoanStatus.Name = "mlblLoanStatus";
            // 
            // mcmComment
            // 
            this.mcmComment.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEditComment,
            this.mnuDeleteComment});
            this.mcmComment.Name = "mcmComment";
            resources.ApplyResources(this.mcmComment, "mcmComment");
            // 
            // mnuEditComment
            // 
            this.mnuEditComment.Name = "mnuEditComment";
            resources.ApplyResources(this.mnuEditComment, "mnuEditComment");
            this.mnuEditComment.Click += new System.EventHandler(this.mnuEditComment_Click);
            // 
            // mnuDeleteComment
            // 
            this.mnuDeleteComment.Name = "mnuDeleteComment";
            resources.ApplyResources(this.mnuDeleteComment, "mnuDeleteComment");
            this.mnuDeleteComment.Click += new System.EventHandler(this.mnuDeleteComment_Click);
            // 
            // mbtnDeleteLoan
            // 
            resources.ApplyResources(this.mbtnDeleteLoan, "mbtnDeleteLoan");
            this.mbtnDeleteLoan.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconSubtract;
            this.mbtnDeleteLoan.Name = "mbtnDeleteLoan";
            this.mbtnDeleteLoan.UseSelectable = true;
            this.mbtnDeleteLoan.Click += new System.EventHandler(this.mbtnDeleteLoan_Click);
            // 
            // RegisterInstrumentControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "RegisterInstrumentControl";
            this.Load += new System.EventHandler(this.RegisterInstrument_Load);
            this.Controls.SetChildIndex(this.tlpMain, 0);
            this.tlpMain.ResumeLayout(false);
            this.mtbTabManager.ResumeLayout(false);
            this.tbGeneralData.ResumeLayout(false);
            this.tlpGeneralData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComments)).EndInit();
            this.pnComment.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.tbLoans.ResumeLayout(false);
            this.tlpLoan.ResumeLayout(false);
            this.pnNewLoan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoans)).EndInit();
            this.pnLoan.ResumeLayout(false);
            this.pnLoan.PerformLayout();
            this.mcmComment.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroTabControl mtbTabManager;
        private System.Windows.Forms.TabPage tbGeneralData;
        private System.Windows.Forms.TabPage tbLoans;
        private System.Windows.Forms.Panel pnContent;
        private MetroFramework.Controls.MetroComboBox mcbInstrumentType;
        private MetroFramework.Controls.MetroLabel mlblInstrumentType;
        private MetroFramework.Controls.MetroComboBox mcbStatus;
        private MetroFramework.Controls.MetroLabel mlblStatus;
        private MetroFramework.Controls.MetroTextBox mtxtStorageLocation;
        private MetroFramework.Controls.MetroLabel mlblStorageLocation;
        private MetroFramework.Controls.MetroTextBox mtxtCode;
        private MetroFramework.Controls.MetroLabel mlblCode;
        private MetroFramework.Controls.MetroComboBox mcbPole;
        private MetroFramework.Controls.MetroLabel mlblPole;
        private MetroFramework.Controls.MetroTextBox mtxtModel;
        private MetroFramework.Controls.MetroLabel mlblModel;
        private System.Windows.Forms.TableLayoutPanel tlpLoan;
        private System.Windows.Forms.Panel pnNewLoan;
        private MetroFramework.Controls.MetroButton mbtnAddLoan;
        private MetroFramework.Controls.MetroLabel mlblAddLoan;
        private System.Windows.Forms.TableLayoutPanel tlpGeneralData;
        private System.Windows.Forms.DataGridView dgvComments;
        private System.Windows.Forms.Panel pnComment;
        private MetroFramework.Controls.MetroButton mbtnAddComment;
        private MetroFramework.Controls.MetroTextBox mtxtComment;
        private MetroFramework.Controls.MetroLabel mlblComment;
        private System.Windows.Forms.DataGridView dgvLoans;
        private System.Windows.Forms.Panel pnLoan;
        private MetroFramework.Controls.MetroComboBox mcbLoanStatus;
        private MetroFramework.Controls.MetroLabel mlblLoanStatus;
        private MetroFramework.Controls.MetroComboBox mcbLoanStudent;
        private MetroFramework.Controls.MetroLabel mlblLoanStudent;
        private System.Windows.Forms.MaskedTextBox mtxtLoanEndDate;
        private MetroFramework.Controls.MetroLabel mlblLoanEndDate;
        private System.Windows.Forms.MaskedTextBox mtxtLoanStartDate;
        private MetroFramework.Controls.MetroLabel mlblLoanStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn LoanId;
        private System.Windows.Forms.DataGridViewTextBoxColumn loanStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn loanStudent;
        private System.Windows.Forms.DataGridViewTextBoxColumn loanStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn loanEndDate;
        private MetroFramework.Controls.MetroContextMenu mcmComment;
        private System.Windows.Forms.ToolStripMenuItem mnuEditComment;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn CommentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn CommentText;
        private MetroFramework.Controls.MetroButton mbtnDeleteLoan;
    }
}
