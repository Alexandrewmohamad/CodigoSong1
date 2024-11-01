namespace PnT.SongClient.UI
{
    partial class InactivationReasonForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InactivationReasonForm));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mlblMessage = new MetroFramework.Controls.MetroLabel();
            this.mbtnOK = new MetroFramework.Controls.MetroButton();
            this.pnContent = new System.Windows.Forms.Panel();
            this.mcbReason = new MetroFramework.Controls.MetroComboBox();
            this.mlblReason = new MetroFramework.Controls.MetroLabel();
            this.tlpMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.panel1, 0, 0);
            this.tlpMain.Controls.Add(this.mbtnOK, 0, 2);
            this.tlpMain.Controls.Add(this.pnContent, 0, 1);
            this.tlpMain.Name = "tlpMain";
            // 
            // panel1
            // 
            this.tlpMain.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.mlblMessage);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // mlblMessage
            // 
            this.mlblMessage.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblMessage, "mlblMessage");
            this.mlblMessage.Name = "mlblMessage";
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
            this.pnContent.Controls.Add(this.mcbReason);
            this.pnContent.Controls.Add(this.mlblReason);
            resources.ApplyResources(this.pnContent, "pnContent");
            this.pnContent.Name = "pnContent";
            // 
            // mcbReason
            // 
            this.mcbReason.DisplayMember = "Description";
            this.mcbReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.mcbReason.DropDownWidth = 150;
            this.mcbReason.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.mcbReason.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.mcbReason.FormattingEnabled = true;
            resources.ApplyResources(this.mcbReason, "mcbReason");
            this.mcbReason.Name = "mcbReason";
            this.mcbReason.UseSelectable = true;
            this.mcbReason.ValueMember = "Id";
            this.mcbReason.TextChanged += new System.EventHandler(this.mcbReason_TextChanged);
            // 
            // mlblReason
            // 
            this.mlblReason.FontSize = MetroFramework.MetroLabelSize.Small;
            resources.ApplyResources(this.mlblReason, "mlblReason");
            this.mlblReason.Name = "mlblReason";
            // 
            // InactivationReasonForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ControlBox = false;
            this.Controls.Add(this.tlpMain);
            this.DisplayHeader = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InactivationReasonForm";
            this.Resizable = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.InactivationReasonForm_Load);
            this.tlpMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroButton mbtnOK;
        private System.Windows.Forms.Panel pnContent;
        private MetroFramework.Controls.MetroComboBox mcbReason;
        private MetroFramework.Controls.MetroLabel mlblReason;
        private System.Windows.Forms.Panel panel1;
        private MetroFramework.Controls.MetroLabel mlblMessage;
    }
}