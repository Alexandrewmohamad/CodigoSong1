namespace PnT.SongClient.UI.Controls
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public partial class Calendar
	{
		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picMPK_Cal = new System.Windows.Forms.PictureBox();
			this.btnPrev = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.lblMonth = new System.Windows.Forms.Label();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// picMPK_Cal
			// 
			this.picMPK_Cal.Location = new System.Drawing.Point(0, 35);
			this.picMPK_Cal.Name = "picMPK_Cal";
			this.picMPK_Cal.Size = new System.Drawing.Size(640, 445);
			this.picMPK_Cal.TabIndex = 7;
			this.picMPK_Cal.TabStop = false;
			this.picMPK_Cal.Paint += new System.Windows.Forms.PaintEventHandler(this.picMPK_Cal_Paint);
			this.picMPK_Cal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picMPK_Cal_MouseDown);
			// 
			// btnPrev
			// 
			this.btnPrev.BackColor = System.Drawing.SystemColors.Control;
			this.btnPrev.Location = new System.Drawing.Point(4, 4);
			this.btnPrev.Name = "btnPrev";
			this.btnPrev.Size = new System.Drawing.Size(32, 23);
			this.btnPrev.TabIndex = 9;
			this.btnPrev.Text = "<<";
			this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
			// 
			// btnNext
			// 
			this.btnNext.BackColor = System.Drawing.SystemColors.Control;
			this.btnNext.Location = new System.Drawing.Point(604, 4);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(32, 23);
			this.btnNext.TabIndex = 10;
			this.btnNext.Text = ">>";
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// lblMonth
			// 
			this.lblMonth.Font = MetroFramework.MetroFonts.DefaultLight(14.0F);
            this.lblMonth.Location = new System.Drawing.Point(76, 1);
			this.lblMonth.Name = "lblMonth";
			this.lblMonth.Size = new System.Drawing.Size(136, 23);
			this.lblMonth.TabIndex = 11;
			this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem1,
																						 this.menuItem2,
																						 this.menuItem4,
																						 this.menuItem3,
																						 this.menuItem5});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "6:00 AM - 7:00 AM";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "JUST A TEST";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "-";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 3;
			this.menuItem3.Text = "10:00 AM - 11:00 AM";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 4;
			this.menuItem5.Text = "ANOTHER TEST";
			// 
			// MPK_Calendar
			// 
			this.BackColor = System.Drawing.Color.LightSkyBlue;
			this.Controls.Add(this.lblMonth);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.btnPrev);
			this.Controls.Add(this.picMPK_Cal);
			this.Name = "MPK_Calendar";
			this.Size = new System.Drawing.Size(640, 480);
			this.Load += new System.EventHandler(this.MPK_Calendar_Load);
			this.SizeChanged += new System.EventHandler(this.MPK_Calendar_SizeChanged);
			this.ResumeLayout(false);

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#endregion

        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.PictureBox picMPK_Cal;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;


    }

}
