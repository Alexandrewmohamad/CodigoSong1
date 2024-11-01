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
    public partial class StatisticsAttendanceMonthControl : UserControl
    {

        /// <summary>
        /// The month of the displayed statistics.
        /// </summary>
        private DateTime month = DateTime.MinValue;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatisticsAttendanceMonthControl()
        {
            //init ui components
            InitializeComponent();
        }

        /// <summary>
        /// Set displayed number of absences.
        /// </summary>
        public int NumAbsences
        {
            set
            {
                //set number of absences
                mlblNumAbsencesValue.Text = value.ToString();
            }
        }

        /// <summary>
        /// Set displayed number of presences.
        /// </summary>
        public int NumPresences
        {
            set
            {
                //set number of presences
                mlblNumPresencesValue.Text = value.ToString();
            }
        }

        /// <summary>
        /// Set displayed percentage of presences.
        /// </summary>
        public double PercentagePresence
        {
            set
            {
                //set percentage of presences
                mlblPercentPresenceValue.Text = value != double.MinValue ?
                    value.ToString("0.0") + "%" : "-%";
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

    } //end of StatisticsAttendanceMonthControl

} //end of namespace PnT.SongClient.UI.Controls
