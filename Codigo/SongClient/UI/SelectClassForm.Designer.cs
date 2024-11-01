namespace PnT.SongClient.UI
{
    partial class SelectClassForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectClassForm));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnStudent = new System.Windows.Forms.Panel();
            this.mcbClass = new MetroFramework.Controls.MetroComboBox();
            this.mlblClass = new MetroFramework.Controls.MetroLabel();
            this.mbtnCancel = new MetroFramework.Controls.MetroButton();
            this.mbtnOK = new MetroFramework.Controls.MetroButton();
            this.pnContent = new System.Windows.Forms.Panel();
            this.mcbPole = new MetroFramework.Controls.MetroComboBox();
            this.mlblPole = new MetroFramework.Controls.MetroLabel();
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
            this.pnStudent.Controls.Add(this.mcbClass);
            this.pnStudent.Controls.Add(this.mlblClass);
            resources.ApplyResources(this.pnStudent, "pnStudent");
            this.pnStudent.Name = "pnStudent";
            // 
            // mcbClass
            // 
            this.mcbClass.DisplayMember = "Description";
            this.mcbClass.DropDownWidth = 631;
            this.mcbClass.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbClass.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbClass.FormattingEnabled = true;
            resources.ApplyResources(this.mcbClass, "mcbClass");
            this.mcbClass.Name = "mcbClass";
            this.mcbClass.UseSelectable = true;
            this.mcbClass.ValueMember = "Id";
            this.mcbClass.SelectedIndexChanged += new System.EventHandler(this.mcbClass_SelectedIndexChanged);
            // 
            // mlblClass
            // 
            this.mlblClass.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblClass, "mlblClass");
            this.mlblClass.Name = "mlblClass";
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
            this.pnContent.Controls.Add(this.mcbPole);
            this.pnContent.Controls.Add(this.mlblPole);
            resources.ApplyResources(this.pnContent, "pnContent");
            this.pnContent.Name = "pnContent";
            // 
            // mcbPole
            // 
            this.mcbPole.DisplayMember = "Description";
            this.mcbPole.DropDownWidth = 150;
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
            // SelectClassForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ControlBox = false;
            this.Controls.Add(this.tlpMain);
            this.DisplayHeader = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectClassForm";
            this.Resizable = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.SelectClassForm_Load);
            this.tlpMain.ResumeLayout(false);
            this.pnStudent.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroButton mbtnCancel;
        private MetroFramework.Controls.MetroButton mbtnOK;
        private System.Windows.Forms.Panel pnContent;
        private MetroFramework.Controls.MetroLabel mlblPole;
        private MetroFramework.Controls.MetroComboBox mcbPole;
        private System.Windows.Forms.Panel pnStudent;
        private MetroFramework.Controls.MetroComboBox mcbClass;
        private MetroFramework.Controls.MetroLabel mlblClass;
    }
}