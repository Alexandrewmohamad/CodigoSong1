using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PnT.SongClient.UI.Controls
{

    /// <summary>
    /// Display monthly class statistics.
    /// </summary>
    public partial class StatisticsClassMonthControl : UserControl
    {

        /// <summary>
        /// The month of the displayed statistics.
        /// </summary>
        private DateTime month = DateTime.MinValue;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatisticsClassMonthControl()
        {
            //init ui components
            InitializeComponent();
        }

        /// <summary>
        /// Set displayed number of evasions.
        /// </summary>
        public int NumEvasions
        {
            set
            {
                //set number of evasions
                mlblNumEvasionValue.Text = value.ToString();
            }
        }

        /// <summary>
        /// Set displayed percentage of evasions.
        /// </summary>
        public double PercentageEvasion
        {
            set
            {
                //set percentage of evasions
                mlblPercentEvasionValue.Text = value != double.MinValue ?
                    value.ToString("0.00") + "%" : "-%";
            }
        }

        /// <summary>
        /// Get/set the month of the displayed statistics.
        /// </summary>
        public DateTime Month
        {
            get
            {
                return month;
            }

            set
            {
                //set month
                month = value;

                //display month name
                mlblMonth.Text = Properties.Resources.ResourceManager.GetString(
                    "Month_" + month.Month) + " " + month.Year.ToString();
            }
        }

    } //end of class StatisticsClassMonthControl

} //end of namespace PnT.SongClient.UI.Controls
