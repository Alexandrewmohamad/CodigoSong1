using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace PnT.SongClient.UI.Controls
{

    /// <summary>
    /// Event argument class.
    /// </summary>
    public class SelectedDateChangedEventArgs : EventArgs
    {
        private DateTime pSelectedDate;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dateSelected">Date</param>
        public SelectedDateChangedEventArgs(DateTime dateSelected)
        {
            pSelectedDate = dateSelected;
        }

        /// <summary>
        /// The selected date.
        /// </summary>
        public DateTime SelectedDate
        {
            get
            {
                return pSelectedDate;
            }
        }

    }

    /// <summary>
    /// Event handler for SelectedDate changed
    /// </summary>
    public delegate void SelectedDateChangedEventHandler(object sender, SelectedDateChangedEventArgs e);


	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public partial class Calendar : UserControl
    {
        /// <summary>
        /// The height of the header row.
        /// </summary>
        public const int HEADER_HEIGHT = 25;


        #region Private variables

        private int pWidth;
		private int pHeight;
		private int pPicWidth;
		private int pPicHeight;
		private int intX=0;
		private int intY=0;
		private int pintMonth=DateTime.Now.Month;
		private int pintYear=DateTime.Now.Year;
		private int pintDay=0;
		private int pintMonthPrev=0;
		private int pintMonthNext=0;
		private DateTime[] pBoldedDates=null;
		private DateTime p_SelectedDate=DateTime.Now;
		private bool p_ShowGrid = true;
		private bool p_ShowPrevNextButton = true;
		private bool p_AbbreviateWeekDayHeader=true;
		private bool p_ShowCurrentMonthInDay=false;
		private bool p_DisplayWeekendsDarker = true;
		private Color p_BackgroundColor=Color.LightSteelBlue;
		private Color p_GridColor=Color.Black;
		private Color p_HeaderColor=Color.LightSteelBlue;
		private Color p_BoldedDateFontColor=Color.Red;
		private Color p_ActiveMonthColor=Color.White;
		private Color p_InactiveMonthColor=Color.Silver;
		private Color p_SelectedDayColor=Color.LightSteelBlue;
		private Color p_SelectedDayFontColor=Color.White;
		private Color p_NonselectedDayFontColor=Color.Black;
		private Font p_ApptFont = new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold);
		private Font p_NoApptFont = new Font(FontFamily.GenericSansSerif,10,FontStyle.Regular);
		private Font p_HeaderFont = new Font(FontFamily.GenericSansSerif,9,FontStyle.Regular);
		private Rectangle[,] rects;
		private Rectangle[] rectDays;
		private string[] strDays = new string[7] {"Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"};
		private string[] strAbbrDays = new string[7] {"Sun","Mon","Tue","Wed","Thu","Fri","Sat"};
		private string[] strMonths = new string[12] {"January","February","March","April","May","June","July","August","September","October","November","December"};
		private string[] strAbbrMonths = new string[12] {"Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"};
		
		private bool bDesign = true;

		private Pen penGrid;

		private Color ActiveDarker;
		private Color InActiveDarker;
		private SolidBrush brushHeader;
		private SolidBrush brushActive;
		private SolidBrush brushActiveDarker;
		private SolidBrush brushInactive;
		private SolidBrush brushInactiveDarker;
        private SolidBrush brushBolded;
        private SolidBrush brushBoldedDarker;
        private SolidBrush brushSelectedDay;
		private SolidBrush brushSelectedDayFont;
		private SolidBrush brushNonselectedDayFont;
		private SolidBrush brushBoldedDateFont;


		private StringFormat sf;
		private StringFormat sfHeader;

		//new version

		private DateTime[,] arrDates;

		#endregion Private variables

		#region Custom events
		/// <summary>
		/// Event handler definition
		/// </summary>
		public event SelectedDateChangedEventHandler SelectedDateChanged;

		/// <summary>
		/// Raises event by invoking delegate
		/// </summary>
		/// <param name="eventArgs"></param>
		protected virtual void OnSelectedDateChanged(SelectedDateChangedEventArgs eventArgs)
		{
			if(SelectedDateChanged!=null)
			{
				SelectedDateChanged(this,eventArgs);
			}
		}
		#endregion Custom events

		#region Properties

		/// <summary>
		/// The year of the active month
		/// </summary>
		[Description("The year portion of the month, year to display.")]
		[Category("MPK_Calendar")]
		public int intYear
		{
			get
			{
				if(pintYear==0)
				{
					return DateTime.Now.Year;

				}
				else
				{
					return pintYear;
				}
			}
			set
			{
				pintYear=value;
			}
		}
		
		/// <summary>
		/// The number of the active month.
		/// </summary>
		[Description("The month portion of the month, year to display.")]
		[Category("MPK_Calendar")]
		public int intMonth
		{
			get
			{
				if(pintMonth==0)
				{
					return DateTime.Now.Month;

				}
				else
				{
					return pintMonth;
				}
			}
			set
			{
				if(value<1)
				{
					pintMonth=12;
					pintYear--;
				}
				else if(value>12)
				{
					pintMonth=1;
					pintYear++;
				}
				else
				{
					pintMonth=value;
				}

				if(pintMonth>1)
				{
					pintMonthPrev = pintMonth-1;
				}
				else
				{
					pintMonthPrev=12;
				}
				if(pintMonth<12)
				{
					pintMonthNext=pintMonth+1;
				}
				else
				{
					pintMonthNext=1;
				}


			}
		}
		
		/// <summary>
		/// Array of dates to be bolded.
		/// </summary>
		[Description("Bolded dates.")]
		[Category("MPK_Calendar")]
		public DateTime[] BoldedDates
		{
			get
			{

					return pBoldedDates;

			}
			set
			{
				pBoldedDates = value;
				//look at each date and see if it corresponds with a date in the grid
				if(pBoldedDates!=null)
				{
					Array.Sort(pBoldedDates);
				}

				
			}

		}
			
			/// <summary>
			/// The day number of the selected date.
			/// </summary>
			[Description("The day number.")]
			[Category("MPK_Calendar")]
			public int intDay
			{
				get
				{
					if(pintDay==0)
					{
						return DateTime.Now.Day;

					}
					else
					{
						return pintDay;
					}
				}
				set
				{
					pintDay = value;
				}

			}
		
		/// <summary>
		/// The selected date.
		/// </summary>
		[Description("The selected date.")]
		[Category("MPK_Calendar")]
		public DateTime SelectedDate
		{
			get
			{
				return p_SelectedDate;
			}
			set
			{
				p_SelectedDate = value;
				intMonth = p_SelectedDate.Month;
				intYear = p_SelectedDate.Year;

				SelectedDateChangedEventArgs eventArgs = new SelectedDateChangedEventArgs(SelectedDate);
				OnSelectedDateChanged(eventArgs);

			}

		}

		/// <summary>
		/// Property - Determines whether the Previous and Next buttons are visible.
		/// </summary>
		[Description("Determines whether the Previous and Next buttons are visible.")]
		[Category("MPK_Calendar")]
		public bool ShowPrevNextButton
		{
			get
			{
				return p_ShowPrevNextButton;
			}
			set
			{
				p_ShowPrevNextButton = value;
				this.btnNext.Visible=p_ShowPrevNextButton;
				this.btnPrev.Visible=p_ShowPrevNextButton;
			}
		}

		/// <summary>
		/// Property - Determines whether the weekends are displayed darker.
		/// </summary>
		[Description("Determines whether the weekends are displayed darker.")]
		[Category("MPK_Calendar")]
		public bool DisplayWeekendsDarker
		{
			get
			{
				return p_DisplayWeekendsDarker;
			}
			set
			{
				p_DisplayWeekendsDarker = value;
				picMPK_Cal.Invalidate();
			}
		}

		/// <summary>
		/// Property - Determines whether or not the grid is drawn.
		/// </summary>
		[Description("Determines whether or not the grid is drawn.")]
		[Category("MPK_Calendar")]
		public bool ShowGrid
		{
			get
			{
				return p_ShowGrid;
			}
			set
			{
				p_ShowGrid = value;
				picMPK_Cal.Invalidate();
			}
		}

		/// <summary>
		/// Property - Determines whether or not to abbreviate the weekday in the header.
		/// </summary>
		[Description("Determines whether or not to abbreviate the weekday in the header.")]
		[Category("MPK_Calendar")]
		public bool AbbreviateWeekDayHeader
		{
			get
			{
				return p_AbbreviateWeekDayHeader;
			}
			set
			{
				p_AbbreviateWeekDayHeader = value;
			}
		}

		/// <summary>
		/// Property - Determines whether or not to display the month name in each day of the current month.
		/// </summary>
		[Description("Determines whether or not to display the month name in each day of the current month.")]
		[Category("MPK_Calendar")]
		public bool ShowCurrentMonthInDay
		{
			get
			{
				return p_ShowCurrentMonthInDay;
			}
			set
			{
				p_ShowCurrentMonthInDay = value;
			}
		}

		/// <summary>
		/// Property - Grid color.
		/// </summary>
		[Description("Grid color.")]
		[Category("MPK_Calendar")]
		public Color GridColor
		{
			get
			{
				return p_GridColor;
			}
			set
			{
				p_GridColor=value;
				this.picMPK_Cal.Invalidate();
			}
		}

		/// <summary>
		/// Property - Header background color.
		/// </summary>
		[Description("Header background color.")]
		[Category("MPK_Calendar")]
		public Color HeaderColor
		{
			get
			{
				return p_HeaderColor;
			}
			set
			{
				p_HeaderColor=value;
				this.picMPK_Cal.Invalidate();
			}
		}

		/// <summary>
		/// Property - Control background color.
		/// </summary>
		[Description("Background color.")]
		[Category("MPK_Calendar")]
		public Color BackgroundColor
		{
			get
			{
				return p_BackgroundColor;
			}
			set
			{
				p_BackgroundColor=value;
				this.BackColor = p_BackgroundColor;
			}
		}

		/// <summary>
		/// Property - Color of the active month.
		/// </summary>
		[Description("Color of the active month.")]
		[Category("MPK_Calendar")]
		public Color ActiveMonthColor
		{
			get
			{
				return p_ActiveMonthColor;
			}
			set
			{
				p_ActiveMonthColor = value;
				this.picMPK_Cal.Invalidate();
			}
		}

		/// <summary>
		/// Property - Color of the inactive month.
		/// </summary>
		[Description("Color of the inactive month.")]
		[Category("MPK_Calendar")]
		public Color InactiveMonthColor
		{
			get
			{
				return p_InactiveMonthColor;
			}
			set
			{
				p_InactiveMonthColor = value;
				this.picMPK_Cal.Invalidate();
			}
		}

		/// <summary>
		/// Property - Color for bolded dates font.
		/// </summary>
		[Description("Color for bolded dates font.")]
		[Category("MPK_Calendar")]
		public Color BoldedDateFontColor
		{
			get
			{
				return p_BoldedDateFontColor;
			}
			set
			{
				p_BoldedDateFontColor = value;
			}
		}

		/// <summary>
		/// Property - Color of the background for selected day.
		/// </summary>
		[Description("Color of the background for selected day.")]
		[Category("MPK_Calendar")]
		public Color SelectedDayColor
		{
			get
			{
				return p_SelectedDayColor;
			}
			set
			{
				p_SelectedDayColor = value;
			}
		}

		/// <summary>
		/// Property - Color of the text for selected day.
		/// </summary>
		[Description("Color of the text for selected day.")]
		[Category("MPK_Calendar")]
		public Color SelectedDayFontColor
		{
			get
			{
				return p_SelectedDayFontColor;
			}
			set
			{
				p_SelectedDayFontColor = value;
			}
		}

		/// <summary>
		/// Property - Color of the text for non-selected days.
		/// </summary>
		[Description("Color of the text for non-selected days.")]
		[Category("MPK_Calendar")]
		public Color NonselectedDayFontColor
		{
			get
			{
				return p_NonselectedDayFontColor;
			}
			set
			{
				p_NonselectedDayFontColor = value;
			}
		}

		/// <summary>
		/// Property - Font for bolded days.
		/// </summary>
		[Description("Font for bolded days.")]
		[Category("MPK_Calendar")]
		public Font ApptFont
		{
			get
			{
				return p_ApptFont;
			}
			set
			{
				p_ApptFont = value;
			}
		}

		/// <summary>
		/// Property - Font for not bolded days.
		/// </summary>
		[Description("Font for not bolded days.")]
		[Category("MPK_Calendar")]
		public Font NoApptFont
		{
			get
			{
				return p_NoApptFont;
			}
			set
			{
				p_NoApptFont = value;
			}
		}

		/// <summary>
		/// Property - Header text font.
		/// </summary>
		[Description("Header text font.")]
		[Category("MPK_Calendar")]
		public Font HeaderFont
		{
			get
			{
				return p_HeaderFont;
			}
			set
			{
				p_HeaderFont = value;
			}
		}



		#endregion Properties
		
		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public Calendar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            //check selected UI culture
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Equals("pt-BR"))
            {
                //translate calendar words
                //strDays = new string[7] { "Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado" };
                strAbbrDays = new string[7] { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb" };
                strMonths = new string[12] { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
                strAbbrMonths = new string[12] { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" };
            }
        }

		#endregion Constructor

		#region Control events



		private void MPK_Calendar_SizeChanged(object sender, System.EventArgs e)
		{
			pWidth = this.Size.Width;
			pPicWidth=pWidth;
			pHeight = this.Size.Height;
			pPicHeight=pHeight-35;
			picMPK_Cal.Width=pPicWidth;
			picMPK_Cal.Height=pPicHeight;
			//keep the buttons in the same place
			btnPrev.Left = 5;
			btnNext.Left = this.Width - btnNext.Width - 5;
			lblMonth.Left = (this.Width - lblMonth.Width)/2;
			rects = CreateGrid(picMPK_Cal.Width,picMPK_Cal.Height);
			picMPK_Cal.Invalidate();
		}

		private void picMPK_Cal_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{

			intX = e.X;
			intY = e.Y;
			int inti = 0;
			int intj = 0;
			for(int j = 0;j<6;j++)
			{
				//for(int i = 0;i<7i++)
				for(int i = 0;i<7;i++)
				{
					if(rects[i,j].Contains(intX,intY))
					{
						inti = i;
						intj = j;

						//update
						this.SelectedDate = arrDates[i,j];;


						//update
						intMonth = arrDates[i,j].Month;
					}
				}

			}
			picMPK_Cal.Invalidate();
		}

		//this is not used when displaying appointment data
		public void btnPrev_Click(object sender, System.EventArgs e)
		{
			if(intMonth==1)
			{
				intMonth=12;
				intYear=intYear-1;
			}
			else
			{
				intMonth--;
			}
			//update
			this.SelectedDate = this.SelectedDate.AddMonths(-1);
			picMPK_Cal.Invalidate();

		}

		//this is not used when displaying appointment data
		public void btnNext_Click(object sender, System.EventArgs e)
		{
			if(intMonth==12)
			{
				intMonth=1;
				intYear++;
			}
			else
			{
				intMonth++;
			}
			//update
			this.SelectedDate = this.SelectedDate.AddMonths(1);
			picMPK_Cal.Invalidate();
		}

		private void picMPK_Cal_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			bool bActive = false;
			bool bSelected = false;
			bool bApptOnDate = false;
			if(!bDesign)
			{
				//the calendar is created as a 7 x 6 grid drawn onto the picture box
				//the data to be displayed in the calendar is stored in a 7 x 6 array of arrays
				//update
				FillDates2(this.SelectedDate);
				CreateGraphicObjects();

                //update
                //month label at top
                this.lblMonth.Text = strMonths[intMonth-1] + ", " + intYear.ToString();
                
				Rectangle rect = e.ClipRectangle;
                rect.Y += HEADER_HEIGHT;
                rect.Height -= HEADER_HEIGHT;
                
                //graphics object for paint event
                Graphics g = e.Graphics;
				g.FillRectangle(Brushes.White,rect);
				string str;
				
                //day of week header
				for(int i = 0;i<7;i++)
				{
					//draw weekday header rectangle
					g.FillRectangle(brushHeader,rectDays[i]);
					//draw rectnagle for grid if requested
					if(p_ShowGrid)
					{
						g.DrawRectangle(new Pen(Color.Black,2),rectDays[i]);
					}
					//draw weekday header
					if(p_AbbreviateWeekDayHeader)
					{
						g.DrawString( strAbbrDays[i],p_HeaderFont, Brushes.Black,rectDays[i],sfHeader);
					}
					else
					{
						g.DrawString( strDays[i], p_HeaderFont, Brushes.Black,rectDays[i],sfHeader);
					}

				}
				
                //actual calendar 
				for(int j = 0;j<6;j++)
				{
					for(int i = 0;i<7;i++)
					{
						//draw rectangle for grid if requested
						if(p_ShowGrid)
						{
							g.DrawRectangle(penGrid,rects[i,j]);
						}

						//rects for text
						Rectangle rectTopHalf = new Rectangle(
                            rects[i,j].X,rects[i,j].Y,rects[i,j].Width,(int)(rects[i,j].Height));

                        //check to see if day is in active month
                        //update
                        if (arrDates[i,j].Month==pintMonth)
						{
							bActive=true;
							//add month name to box if requested
							if(p_ShowCurrentMonthInDay)
							{
								//update
								str=strAbbrMonths[intMonth-1] + " " + arrDates[i,j].Day.ToString();
							}
							else
							{

								//update
								str=arrDates[i,j].Day.ToString();
							}

							//update
							DateTime dateTest = arrDates[i,j];
							//should box be filled as selected date

							if(dateTest.Date==this.SelectedDate.Date)
							{
								bSelected = true;
							}
							else
							{
								bSelected = false;
							}

							//check to see if date is in BoldedDates
							//first, are there any BoldedDates?
							if(pBoldedDates!=null)//yes, there are BoldedDates
							{
								//do any dates match the date for this rectangle?
								if(Array.IndexOf(pBoldedDates,dateTest)>-1)
								{
									bApptOnDate = true;
								}
								else//no match
								{
									bApptOnDate=false;
								}
							}
							else//no BoldedDates
							{
								bApptOnDate=false;
							}

						}
						else//not the active month
						{
							//update
							str=arrDates[i,j].Day.ToString();
							bActive=false;
							bSelected=false;
						}

						/////////////////////////////////////////////////////////////////////////////
						//finally, draw rectangle and text
						if(bSelected)//selected date
						{
							g.FillRectangle(brushSelectedDay,rects[i,j]);
							if(bApptOnDate)
							{
								g.DrawString( str, p_ApptFont, brushBoldedDateFont, rectTopHalf,sfHeader);

							}
							else
							{
								g.DrawString( str, p_NoApptFont, brushSelectedDayFont, rectTopHalf,sfHeader);

							}
						}
						else if(bActive)//not selected but active
						{
                            if (((i == 0) || (i == 6)) && p_DisplayWeekendsDarker)
                            {
                                //weekend
                                g.FillRectangle(bApptOnDate ? brushBoldedDarker : brushActiveDarker, rects[i, j]);
                            }
                            else
                            {
                                //weekday
                                g.FillRectangle(bApptOnDate ? brushBolded : brushActive, rects[i, j]);
                            }
							
							if(bApptOnDate)
							{
								g.DrawString( str, p_ApptFont, brushBoldedDateFont, rectTopHalf,sfHeader);

							}
							else
							{
								g.DrawString( str, p_NoApptFont, brushNonselectedDayFont, rectTopHalf,sfHeader);

							}
						}
						else//not selected or active
						{
							if(((i == 0) || (i==6)) && p_DisplayWeekendsDarker)//weekend
							{
								g.FillRectangle(brushInactiveDarker,rects[i,j]);
							}
							else//weekday
							{
								g.FillRectangle(brushInactive,rects[i,j]);
							}
							
							if(bApptOnDate)
							{
								g.DrawString( str, p_NoApptFont, brushNonselectedDayFont, rectTopHalf,sfHeader);

							}
							else
							{
								g.DrawString( str, p_NoApptFont, brushNonselectedDayFont, rectTopHalf,sfHeader);
							}
						}

                        //check if date is today
                        if (arrDates[i, j].Date == DateTime.Today)
                        {
                            //get rectangle
                            Rectangle r = rects[i, j];

                            //draw rectangle
                            g.DrawRectangle(new Pen(Color.Gray, 1.0F),
                                r.X, r.Y, r.Width - 1, r.Height - 1);
                        }
                    }
				}

			}

		}

		#endregion Control events

		#region Private functions

		private void CreateGraphicObjects()
		{
			//these are the objects that will be used in the paint event
			//pens
			penGrid = new Pen(p_GridColor,3);

			//brushes
			brushHeader = new SolidBrush(p_HeaderColor);

			brushActive = new SolidBrush(p_ActiveMonthColor);
			ActiveDarker = Color.FromArgb((int)(p_ActiveMonthColor.R*0.8),(int)(p_ActiveMonthColor.G*0.8),(int)(p_ActiveMonthColor.B*0.8));
			brushActiveDarker = new SolidBrush(ActiveDarker);

			brushInactive = new SolidBrush(p_InactiveMonthColor);
			InActiveDarker = Color.FromArgb((int)(p_InactiveMonthColor.R*0.8),(int)(p_InactiveMonthColor.G*0.8),(int)(p_InactiveMonthColor.B*0.8));
			brushInactiveDarker = new SolidBrush(InActiveDarker);
            
            brushBolded = new SolidBrush(Color.FromArgb(153, 217, 234));
            brushBoldedDarker = new SolidBrush(Color.FromArgb(
                (int)(brushBolded.Color.R * 0.8), 
                (int)(brushBolded.Color.G * 0.8), 
                (int)(brushBolded.Color.B * 0.8)));

            brushSelectedDay = new SolidBrush(p_SelectedDayColor);
			brushBoldedDateFont = new SolidBrush(p_BoldedDateFontColor);
			brushSelectedDayFont = new SolidBrush(p_SelectedDayFontColor);
			brushNonselectedDayFont = new SolidBrush(p_NonselectedDayFontColor);

			//stringformats for displaying text
			//this is used to display appointment text in each day box
			//the text is left aligned and trimmed to fit with ellipsis
			sf = new StringFormat();
			sf.Alignment = StringAlignment.Near;//left
			sf.LineAlignment = StringAlignment.Center;
			sf.Trimming = StringTrimming.EllipsisCharacter;
			//this is used for day header and day numbers
			//the text is centered vertically and horizontally
			sfHeader = new StringFormat();
			sfHeader.Alignment = StringAlignment.Center;
			sfHeader.LineAlignment = StringAlignment.Center;

		}

        private Rectangle[,] CreateGrid(int intW, int intH)
        {
            //Array of rectangles representing the calendar
            Rectangle[,] rectTemp = new Rectangle[7, 6];

            //header rectangles
            //
            rectDays = new Rectangle[7];

            int intXX = 0;
            int intXSize = (int)Math.Floor((double)intW / 7);
            int intYSize = (int)Math.Floor((double)(intH - HEADER_HEIGHT) / 6);
            int intYY = 0;

            int deltaX = intW - (7 * intXSize);


            intXX = 0;

            for (int i = 0; i < 7; i++)
            {
                Rectangle r1 = new Rectangle(
                    intXX, intYY, intXSize + ((i == 6) ? deltaX : 0), HEADER_HEIGHT);
                intXX += intXSize;
                rectDays[i] = r1;

            }
            intYY = HEADER_HEIGHT;
            for (int j = 0; j < 6; j++)
            {
                intXX = 0;
                for (int i = 0; i < 7; i++)
                {
                    Rectangle r1 = new Rectangle(
                        intXX, intYY, intXSize + ((i == 6) ? deltaX : 0), intYSize);
                    intXX += intXSize;
                    rectTemp[i, j] = r1;
                }
                intYY += intYSize;
            }
            return rectTemp;
        }



		public void FillDates2(DateTime datCurrent)
		{

			//grid column
			int intDayofWeek=0;
			//grid row
			int intWeek = 0;

			//total day counter
			int intTotalDays=-1;

			//this is where the first day of the month falls in the grid
			int intFirstDay=0;

			DateTime datPrevMonth = datCurrent.AddMonths(-1);
			DateTime datNextMonth = datCurrent.AddMonths(1);

			//number of days in active month
			int intCurrDays = DateTime.DaysInMonth(datCurrent.Year,datCurrent.Month);
			
			//number of days in active month
			int intPrevDays = DateTime.DaysInMonth(datPrevMonth.Year,datPrevMonth.Month);
			
			//number of days in active month
			int intNextDays = DateTime.DaysInMonth(datNextMonth.Year,datNextMonth.Month);
			

			DateTime[] datesCurr = new DateTime[intCurrDays];
			DateTime[] datesPrev = new DateTime[intPrevDays];
			DateTime[] datesNext = new DateTime[intNextDays];

			for(int i = 0;i<intCurrDays;i++)
			{
				datesCurr[i] = new DateTime(datCurrent.Year,datCurrent.Month,i+1);
			}

			for(int i = 0;i<intPrevDays;i++)
			{
				datesPrev[i] = new DateTime(datPrevMonth.Year,datPrevMonth.Month,i+1);
			}

			for(int i = 0;i<intNextDays;i++)
			{
				datesNext[i] = new DateTime(datNextMonth.Year,datNextMonth.Month,i+1);
			}

			//array to hold dates corresponding to grid
			arrDates  = new DateTime[7,6];//dates ahead of current date

			//where does the first day of the week land?
			intDayofWeek = Array.IndexOf(strDays,datesCurr[0].DayOfWeek.ToString());


			for(int intDay = 0;intDay<intCurrDays;intDay++)
			{
				//populate array of dates for active month, this is used to tell what day of the week each day is

				intDayofWeek = Array.IndexOf(strDays,datesCurr[intDay].DayOfWeek.ToString());


				//fill the array with the day numbers
				arrDates[intDayofWeek,intWeek] = datesCurr[intDay];
				if(intDayofWeek==6)
				{
					intWeek++;
				}

				//Back fill any days from the previous month
				//this is does here because I needed to know where the first day of the active month fell in the grid
				if(intDay==0)
				{
					intFirstDay=intDayofWeek;
					//Days in previous month
					int intDaysPrev = DateTime.DaysInMonth(datPrevMonth.Year,datPrevMonth.Month);

					//if the first day of the active month is not sunday, count backwards and fill in day number
					if(intDayofWeek>0)
					{
						for(int i = intDayofWeek-1;i>=0;i--)
						{
							arrDates[i,0] = datesPrev[intDaysPrev-1];
							intDaysPrev--;
							intTotalDays++;
						}
					}
				}
				intTotalDays++;
			}//for

			//fill in the remaining days of the grid with the beginning of the next month

			intTotalDays++;
			//what row did we leave off in for active month?
			int intRow = intTotalDays/7;

			int intCol;

			int intDayNext=0;

			for(int i = intRow;i<6;i++)
			{
				intCol = intTotalDays - (intRow*7);
				for(int j = intCol;j<7;j++)
				{
					arrDates[j,i] = datesNext[intDayNext];
					intDayNext++;
					intTotalDays++;
				}	
				intRow++;
			}

		



		}
		#endregion Private functions

		#region Form events

		private void MPK_Calendar_Load(object sender, System.EventArgs e)
		{
			pWidth = this.Size.Width;
			pPicWidth=pWidth;
			pHeight = this.Size.Height;
			pPicHeight=pHeight-35;
			picMPK_Cal.Width=pPicWidth;
			picMPK_Cal.Height=pPicHeight;
			rects = CreateGrid(picMPK_Cal.Width,picMPK_Cal.Height);
			//FillDates();
			btnPrev.Visible = p_ShowPrevNextButton;
			btnNext.Visible = p_ShowPrevNextButton;
			CreateGraphicObjects();
			bDesign = false;

		}

        #endregion Form events

    } //end of class Calendar

} //end of namespace PnT.SongClient.UI.Controls
