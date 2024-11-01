using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetroFramework.Controls;

using PnT.SongDB.Logic;


namespace PnT.SongClient.UI.Controls
{

    /// <summary>
    /// Display grade statistics for selected month.
    /// </summary>
    public partial class StatisticsGradeMonthControl : UserControl
    {

        /// <summary>
        /// The month of the displayed statistics.
        /// </summary>
        private DateTime month = DateTime.MinValue;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatisticsGradeMonthControl()
        {
            //init UI components
            InitializeComponent();
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

        /// <summary>
        /// Set list of displayed grades. 
        /// Calculate all values according to list of grades.
        /// </summary>
        /// <param name="grades">
        /// The selected list of grades.
        /// </param>
        public void SetGrades(List<Grade> grades)
        {
            //calculate and display mean grade
            CalculateAndDisplayMeanGrade(grades, mlblMeanGradeValue);

            //calculate and display mean discipline grade
            CalculateAndDisplayMeanGrade(
                grades.FindAll(g => g.GradeSubject == (int)GradeSubject.Discipline), 
                mlblDisciplineGradeValue);

            //calculate and display mean performance grade
            CalculateAndDisplayMeanGrade(
                grades.FindAll(g => g.GradeSubject == (int)GradeSubject.Performance),
                mlblPerformanceGradeValue);

            //calculate and display mean dedication grade
            CalculateAndDisplayMeanGrade(
                grades.FindAll(g => g.GradeSubject == (int)GradeSubject.Dedication),
                mlblDedicationGradeValue);
        }

        /// <summary>
        /// Calculate mean grade and display result in the selected indicator.
        /// </summary>
        /// <param name="grades">
        /// The list of grades to calculate mean grade.
        /// </param>
        /// <param name="indicator">
        /// The selected indicator label to display mean grade result.
        /// </param>
        private void CalculateAndDisplayMeanGrade(List<Grade> grades, MetroLabel indicator)
        {
            //check number of grades
            if (grades == null || grades.Count == 0)
            {
                //no grade
                //set empty indicator
                indicator.Text = "-";

                //exit
                return;
            }

            //sum grade scores
            double sum = 0.0;

            //check each grade
            foreach (Grade grade in grades)
            {
                //add grade score
                sum += grade.Score;
            }

            //calculate mean score
            double mean = sum / (double)grades.Count;

            //set result
            indicator.Text = mean.ToString("0.0");
        }

    } //end of class StatisticsGradeMonthControl

} //end of namespace PnT.SongClient.UI.Controls
