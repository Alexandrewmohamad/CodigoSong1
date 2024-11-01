namespace PnT.SongClient.UI.Controls
{
    partial class QuestionCommentControl
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mlblQuestionNumber = new MetroFramework.Controls.MetroLabel();
            this.lblQuestion = new System.Windows.Forms.Label();
            this.tlpComments = new System.Windows.Forms.TableLayoutPanel();
            this.mtxtComments = new MetroFramework.Controls.MetroTextBox();
            this.tlpMain.SuspendLayout();
            this.tlpComments.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.mlblQuestionNumber, 0, 0);
            this.tlpMain.Controls.Add(this.lblQuestion, 1, 0);
            this.tlpMain.Controls.Add(this.tlpComments, 1, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(700, 150);
            this.tlpMain.TabIndex = 1;
            // 
            // mlblQuestionNumber
            // 
            this.mlblQuestionNumber.AutoSize = true;
            this.mlblQuestionNumber.Location = new System.Drawing.Point(0, 0);
            this.mlblQuestionNumber.Margin = new System.Windows.Forms.Padding(0);
            this.mlblQuestionNumber.Name = "mlblQuestionNumber";
            this.mlblQuestionNumber.Size = new System.Drawing.Size(23, 19);
            this.mlblQuestionNumber.TabIndex = 0;
            this.mlblQuestionNumber.Text = "10";
            // 
            // lblQuestion
            // 
            this.lblQuestion.AutoSize = true;
            this.lblQuestion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblQuestion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblQuestion.Location = new System.Drawing.Point(26, 0);
            this.lblQuestion.Margin = new System.Windows.Forms.Padding(0);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(674, 19);
            this.lblQuestion.TabIndex = 2;
            this.lblQuestion.Text = "Ask the user something here.";
            // 
            // tlpComments
            // 
            this.tlpComments.ColumnCount = 2;
            this.tlpComments.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpComments.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpComments.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpComments.Controls.Add(this.mtxtComments, 1, 0);
            this.tlpComments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpComments.Location = new System.Drawing.Point(26, 19);
            this.tlpComments.Margin = new System.Windows.Forms.Padding(0);
            this.tlpComments.Name = "tlpComments";
            this.tlpComments.RowCount = 1;
            this.tlpComments.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpComments.Size = new System.Drawing.Size(674, 131);
            this.tlpComments.TabIndex = 4;
            // 
            // mtxtComments
            // 
            // 
            // 
            // 
            this.mtxtComments.CustomButton.Image = null;
            this.mtxtComments.CustomButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.mtxtComments.CustomButton.Location = new System.Drawing.Point(531, 1);
            this.mtxtComments.CustomButton.Name = "";
            this.mtxtComments.CustomButton.Size = new System.Drawing.Size(129, 129);
            this.mtxtComments.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtxtComments.CustomButton.TabIndex = 1;
            this.mtxtComments.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtxtComments.CustomButton.UseSelectable = true;
            this.mtxtComments.CustomButton.Visible = false;
            this.mtxtComments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtxtComments.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            this.mtxtComments.Lines = new string[0];
            this.mtxtComments.Location = new System.Drawing.Point(3, 7);
            this.mtxtComments.Margin = new System.Windows.Forms.Padding(3, 7, 11, 0);
            this.mtxtComments.MaxLength = 32767;
            this.mtxtComments.Multiline = true;
            this.mtxtComments.Name = "mtxtComments";
            this.mtxtComments.PasswordChar = '\0';
            this.mtxtComments.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtxtComments.SelectedText = "";
            this.mtxtComments.SelectionLength = 0;
            this.mtxtComments.SelectionStart = 0;
            this.mtxtComments.ShortcutsEnabled = true;
            this.mtxtComments.Size = new System.Drawing.Size(660, 124);
            this.mtxtComments.TabIndex = 22;
            this.mtxtComments.UseSelectable = true;
            this.mtxtComments.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtxtComments.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.mtxtComments.TextChanged += new System.EventHandler(this.mtxtComments_TextChanged);
            // 
            // QuestionCommentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "QuestionCommentControl";
            this.Size = new System.Drawing.Size(700, 150);
            this.Load += new System.EventHandler(this.QuestionCommentControl_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tlpComments.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private MetroFramework.Controls.MetroLabel mlblQuestionNumber;
        private System.Windows.Forms.Label lblQuestion;
        private System.Windows.Forms.TableLayoutPanel tlpComments;
        private MetroFramework.Controls.MetroTextBox mtxtComments;
    }
}
