namespace PnT.SongClient.UI
{
    partial class EditCommentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditCommentForm));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mbtnCancel = new MetroFramework.Controls.MetroButton();
            this.mbtnOK = new MetroFramework.Controls.MetroButton();
            this.pnContent = new System.Windows.Forms.Panel();
            this.mtxtComment = new MetroFramework.Controls.MetroTextBox();
            this.mlblComment = new MetroFramework.Controls.MetroLabel();
            this.tlpMain.SuspendLayout();
            this.pnContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.mbtnCancel, 1, 1);
            this.tlpMain.Controls.Add(this.mbtnOK, 0, 1);
            this.tlpMain.Controls.Add(this.pnContent, 0, 0);
            this.tlpMain.Name = "tlpMain";
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
            resources.ApplyResources(this.pnContent, "pnContent");
            this.tlpMain.SetColumnSpan(this.pnContent, 2);
            this.pnContent.Controls.Add(this.mtxtComment);
            this.pnContent.Controls.Add(this.mlblComment);
            this.pnContent.Name = "pnContent";
            // 
            // mtxtComment
            // 
            resources.ApplyResources(this.mtxtComment, "mtxtComment");
            // 
            // 
            // 
            this.mtxtComment.CustomButton.AccessibleDescription = resources.GetString("resource.AccessibleDescription");
            this.mtxtComment.CustomButton.AccessibleName = resources.GetString("resource.AccessibleName");
            this.mtxtComment.CustomButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("resource.Anchor")));
            this.mtxtComment.CustomButton.AutoSize = ((bool)(resources.GetObject("resource.AutoSize")));
            this.mtxtComment.CustomButton.AutoSizeMode = ((System.Windows.Forms.AutoSizeMode)(resources.GetObject("resource.AutoSizeMode")));
            this.mtxtComment.CustomButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resource.BackgroundImage")));
            this.mtxtComment.CustomButton.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("resource.BackgroundImageLayout")));
            this.mtxtComment.CustomButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("resource.Dock")));
            this.mtxtComment.CustomButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("resource.FlatStyle")));
            this.mtxtComment.CustomButton.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font")));
            this.mtxtComment.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.mtxtComment.CustomButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.ImageAlign")));
            this.mtxtComment.CustomButton.ImageIndex = ((int)(resources.GetObject("resource.ImageIndex")));
            this.mtxtComment.CustomButton.ImageKey = resources.GetString("resource.ImageKey");
            this.mtxtComment.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode")));
            this.mtxtComment.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location")));
            this.mtxtComment.CustomButton.MaximumSize = ((System.Drawing.Size)(resources.GetObject("resource.MaximumSize")));
            this.mtxtComment.CustomButton.Name = "";
            this.mtxtComment.CustomButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("resource.RightToLeft")));
            this.mtxtComment.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size")));
            this.mtxtComment.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtComment.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex")));
            this.mtxtComment.CustomButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("resource.TextAlign")));
            this.mtxtComment.CustomButton.TextImageRelation = ((System.Windows.Forms.TextImageRelation)(resources.GetObject("resource.TextImageRelation")));
            this.mtxtComment.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtComment.CustomButton.UseSelectable = true;
            this.mtxtComment.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible")));
            this.mtxtComment.Lines = new string[0];
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
            resources.ApplyResources(this.mlblComment, "mlblComment");
            this.mlblComment.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblComment.Name = "mlblComment";
            // 
            // EditCommentForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ControlBox = false;
            this.Controls.Add(this.tlpMain);
            this.DisplayHeader = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditCommentForm";
            this.Resizable = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.EditCommentForm_Load);
            this.tlpMain.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnContent;
        private MetroFramework.Controls.MetroTextBox mtxtComment;
        private MetroFramework.Controls.MetroLabel mlblComment;
        private MetroFramework.Controls.MetroButton mbtnOK;
        private MetroFramework.Controls.MetroButton mbtnCancel;
    }
}