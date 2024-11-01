using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetroFramework;

using PnT.SongDB.Logic;
using PnT.SongServer;

using PnT.SongClient.Logic;


namespace PnT.SongClient.UI.Controls
{

    /// <summary>
    /// Displays an indicator data.
    /// </summary>
    public partial class IndicatorControl : UserControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The list of indicators to be displayed.
        /// </summary>
        List<KeyValuePair<string, string>> indicators = null;

        /// <summary>
        /// The index of the displayed indicator in the list of indicators.
        /// </summary>
        private int displayedIndicatorIndex = -1;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IndicatorControl()
        {
            //create lists
            indicators = new List<KeyValuePair<string, string>>();

            //init UI components
            InitializeComponent();

            //set back color to blue
            this.BackColor = Color.FromArgb(0, 174, 219);
        }


        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Set background color.
        /// </summary>
        public Color BackgroundColor
        {
            set
            {
                //update back color
                tlpMain.BackColor = value;
            }
        }

        /// <summary>
        /// Set foreground color.
        /// </summary>
        public Color ForegroundColor
        {
            set
            {
                //update fore color
                lblCaption.ForeColor = value;
                lblNumber.ForeColor = value;
            }
        }

        #endregion Properties


        #region Private Methods *******************************************************

        /// <summary>
        /// Clear list of indicators.
        /// </summary>
        public void ClearIndicators()
        {
            //clear list of indicators
            indicators.Clear();

            //clear displayed indicator
            //by displaying next indicator
            Next();
        }

        /// <summary>
        /// Add new indicator to list of indicators.
        /// </summary>
        /// <param name="number">
        /// The indicator number.
        /// </param>
        /// <param name="caption">
        /// The indicator caption.
        /// </param>
        public void AddIndicator(int number, string caption)
        {
            //add indicator
            AddIndicator(number.ToString(), caption);
        }

        /// <summary>
        /// Add new indicator to list of indicators.
        /// </summary>
        /// <param name="number">
        /// The indicator number.
        /// </param>
        /// <param name="caption">
        /// The indicator caption.
        /// </param>
        public void AddIndicator(string number, string caption)
        {
            //add indicator to list
            indicators.Add(new KeyValuePair<string, string>(number, caption));

            //check if added indicator is the first one
            if (indicators.Count == 1)
            {
                //first added indicator
                //display indicator
                Next();
            }
        }

        /// <summary>
        /// Display next indicator in the list of indicators.
        /// </summary>
        public void Next()
        {
            //check number of indicators
            if (indicators.Count == 0)
            {
                //clear number and caption
                lblNumber.Text = string.Empty;
                lblCaption.Text = string.Empty;

                //exit
                return;
            }

            //increment displayed indicator index
            displayedIndicatorIndex = (displayedIndicatorIndex == indicators.Count - 1) ?
                0 : displayedIndicatorIndex + 1;

            //get indicator
            KeyValuePair<string, string> indicator = indicators[displayedIndicatorIndex];

            //display indicator
            lblNumber.Text = indicator.Key;
            lblCaption.Text = indicator.Value;
        }

        #endregion Private Methods


        #region UI Event Handlers *****************************************************

        /// <summary>
        /// Control load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IndicatorControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set font
            lblNumber.Font = MetroFramework.MetroFonts.Default(24);
            lblCaption.Font = MetroFramework.MetroFonts.DefaultLight(10);

            //clear mockup indicator
            Next();
        }

        #endregion UI Event Handlers

    } //end of class IndicatorControl

} //end of namespace PnT.SongClient.UI.Controls
