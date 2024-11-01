namespace PnT.SongClient.UI
{
    partial class SelectFileForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectFileForm));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mbtnOK = new MetroFramework.Controls.MetroButton();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.mbtnFindFile = new MetroFramework.Controls.MetroButton();
            this.mbtnDeleteFile = new MetroFramework.Controls.MetroButton();
            this.mbtnCancel = new MetroFramework.Controls.MetroButton();
            this.sfdFile = new System.Windows.Forms.SaveFileDialog();
            this.ofdFile = new System.Windows.Forms.OpenFileDialog();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mbtnOK, 0, 2);
            this.tlpMain.Controls.Add(this.pbImage, 0, 0);
            this.tlpMain.Controls.Add(this.mbtnFindFile, 2, 0);
            this.tlpMain.Controls.Add(this.mbtnDeleteFile, 2, 1);
            this.tlpMain.Controls.Add(this.mbtnCancel, 1, 2);
            this.tlpMain.Name = "tlpMain";
            // 
            // mbtnOK
            // 
            resources.ApplyResources(this.mbtnOK, "mbtnOK");
            this.mbtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.mbtnOK.Name = "mbtnOK";
            this.mbtnOK.UseSelectable = true;
            // 
            // pbImage
            // 
            this.pbImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.tlpMain.SetColumnSpan(this.pbImage, 2);
            resources.ApplyResources(this.pbImage, "pbImage");
            this.pbImage.Name = "pbImage";
            this.tlpMain.SetRowSpan(this.pbImage, 2);
            this.pbImage.TabStop = false;
            this.pbImage.Click += new System.EventHandler(this.pbImage_Click);
            // 
            // mbtnFindFile
            // 
            resources.ApplyResources(this.mbtnFindFile, "mbtnFindFile");
            this.mbtnFindFile.Name = "mbtnFindFile";
            this.mbtnFindFile.UseSelectable = true;
            this.mbtnFindFile.Click += new System.EventHandler(this.mbtnFindFile_Click);
            // 
            // mbtnDeleteFile
            // 
            resources.ApplyResources(this.mbtnDeleteFile, "mbtnDeleteFile");
            this.mbtnDeleteFile.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconSubtract;
            this.mbtnDeleteFile.Name = "mbtnDeleteFile";
            this.mbtnDeleteFile.UseSelectable = true;
            // 
            // mbtnCancel
            // 
            resources.ApplyResources(this.mbtnCancel, "mbtnCancel");
            this.mbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mbtnCancel.Name = "mbtnCancel";
            this.mbtnCancel.UseSelectable = true;
            // 
            // ofdFile
            // 
            this.ofdFile.FileName = "openFileDialog1";
            resources.ApplyResources(this.ofdFile, "ofdFile");
            // 
            // SelectFileForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ControlBox = false;
            this.Controls.Add(this.tlpMain);
            this.DisplayHeader = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectFileForm";
            this.Resizable = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.SelectFileForm_Load);
            this.tlpMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.PictureBox pbImage;
        private MetroFramework.Controls.MetroButton mbtnFindFile;
        private MetroFramework.Controls.MetroButton mbtnDeleteFile;
        private MetroFramework.Controls.MetroButton mbtnOK;
        private MetroFramework.Controls.MetroButton mbtnCancel;
        private System.Windows.Forms.SaveFileDialog sfdFile;
        private System.Windows.Forms.OpenFileDialog ofdFile;
    }
}