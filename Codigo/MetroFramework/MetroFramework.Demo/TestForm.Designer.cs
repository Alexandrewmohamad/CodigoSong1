namespace MetroFramework.Demo
{
    partial class TestForm
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnControls = new System.Windows.Forms.Panel();
            this.metroTile1 = new MetroFramework.Controls.MetroTile();
            this.mtTeachers = new MetroFramework.Controls.MetroTile();
            this.pnContent = new System.Windows.Forms.Panel();
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.tlpMain.SuspendLayout();
            this.pnControls.SuspendLayout();
            this.pnContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.pnControls, 0, 0);
            this.tlpMain.Controls.Add(this.pnContent, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(20, 45);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(760, 536);
            this.tlpMain.TabIndex = 0;
            // 
            // pnControls
            // 
            this.pnControls.Controls.Add(this.metroTile1);
            this.pnControls.Controls.Add(this.mtTeachers);
            this.pnControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnControls.Location = new System.Drawing.Point(0, 0);
            this.pnControls.Margin = new System.Windows.Forms.Padding(0);
            this.pnControls.Name = "pnControls";
            this.pnControls.Size = new System.Drawing.Size(150, 536);
            this.pnControls.TabIndex = 1;
            // 
            // metroTile1
            // 
            this.metroTile1.ActiveControl = null;
            this.metroTile1.DisplayFocusBorder = false;
            this.metroTile1.Location = new System.Drawing.Point(0, 39);
            this.metroTile1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.metroTile1.Name = "metroTile1";
            this.metroTile1.Size = new System.Drawing.Size(150, 39);
            this.metroTile1.Style = MetroFramework.MetroColorStyle.Silver;
            this.metroTile1.TabIndex = 2;
            this.metroTile1.Text = "Students";
            this.metroTile1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTile1.UseSelectable = true;
            // 
            // mtTeachers
            // 
            this.mtTeachers.ActiveControl = null;
            this.mtTeachers.BackColor = System.Drawing.Color.DarkBlue;
            this.mtTeachers.DisplayFocusBorder = false;
            this.mtTeachers.Location = new System.Drawing.Point(0, 0);
            this.mtTeachers.Margin = new System.Windows.Forms.Padding(0);
            this.mtTeachers.Name = "mtTeachers";
            this.mtTeachers.Size = new System.Drawing.Size(150, 39);
            this.mtTeachers.Style = MetroFramework.MetroColorStyle.Blue;
            this.mtTeachers.TabIndex = 0;
            this.mtTeachers.Text = "Teacher";
            this.mtTeachers.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mtTeachers.UseSelectable = true;
            // 
            // pnContent
            // 
            this.pnContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.pnContent.Controls.Add(this.tlpContent);
            this.pnContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnContent.Location = new System.Drawing.Point(150, 0);
            this.pnContent.Margin = new System.Windows.Forms.Padding(0);
            this.pnContent.Name = "pnContent";
            this.pnContent.Padding = new System.Windows.Forms.Padding(1);
            this.pnContent.Size = new System.Drawing.Size(610, 536);
            this.pnContent.TabIndex = 2;
            // 
            // tlpContent
            // 
            this.tlpContent.BackColor = System.Drawing.Color.White;
            this.tlpContent.ColumnCount = 2;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.Location = new System.Drawing.Point(1, 1);
            this.tlpContent.Margin = new System.Windows.Forms.Padding(5);
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.RowCount = 2;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContent.Size = new System.Drawing.Size(608, 534);
            this.tlpContent.TabIndex = 0;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(800, 601);
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MinimumSize = new System.Drawing.Size(800, 601);
            this.Name = "TestForm";
            this.Text = "Song";
            this.TextPosition = new System.Drawing.Point(14, 5);
            this.tlpMain.ResumeLayout(false);
            this.pnControls.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnControls;
        private Controls.MetroTile mtTeachers;
        private Controls.MetroTile metroTile1;
        private System.Windows.Forms.Panel pnContent;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
    }
}