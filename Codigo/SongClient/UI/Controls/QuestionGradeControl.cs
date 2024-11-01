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
    public partial class QuestionGradeControl : UserControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The displayed previous answer if any.
        /// </summary>
        private Answer answer = null;

        /// <summary>
        /// The list of score radio buttons.
        /// </summary>
        private List<RadioButton> scoreButtons = null;

        /// <summary>
        /// Indicates if data is being loaded.
        /// </summary>
        private bool isLoading = false;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public QuestionGradeControl()
        {            
            //init UI components
            InitializeComponent();

            //create list of score radio buttons
            scoreButtons = new List<RadioButton>();
            scoreButtons.Add(rbScore0);
            scoreButtons.Add(rbScore1);
            scoreButtons.Add(rbScore2);
            scoreButtons.Add(rbScore3);
            scoreButtons.Add(rbScore4);
            scoreButtons.Add(rbScore5);
            scoreButtons.Add(rbScore6);
            scoreButtons.Add(rbScore7);
            scoreButtons.Add(rbScore8);
            scoreButtons.Add(rbScore9);
            scoreButtons.Add(rbScore10);
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get the answer.
        /// </summary>
        public Answer Answer
        {
            get
            {
                return answer;
            }
        }

        #endregion Properties


        #region Public Methods ********************************************************

        /// <summary>
        /// Display/hide question number.
        /// </summary>
        /// <param name="show">
        /// True to display question number.
        /// False to hider.
        /// </param>
        public void DisplayQuestionNumber(bool display)
        {
            //display or hide question number
            mlblQuestionNumber.Visible = display;
        }

        /// <summary>
        /// Enable editable fields according to selected option.
        /// </summary>
        /// <param name="enable">
        /// True to enable editable fields.
        /// False otherwise.
        /// </param>
        public void EnableFields(bool enable)
        {
            //enable fields
            tlpScore.Enabled = enable;
        }

        /// <summary>
        /// Load and display data.
        /// </summary>
        /// <param name="question">
        /// The displayed question.
        /// </param>
        /// <param name="questionNumber">
        /// The number of the displayed question.
        /// </param>
        /// <param name="answer">
        /// The displayed previous answer if any.
        /// Null if there is no previous answer.
        /// </param>
        public void LoadData(Question question, int questionNumber, Answer answer)
        {
            //check question
            if (question == null)
            {
                //cannot load data
                //exit
                return;
            }

            //check answer
            if (answer == null)
            {
                //cannot load data
                //exit
                return;
            }

            try
            {
                //set loading flag
                isLoading = true;

                //load question
                mlblQuestionNumber.Text = questionNumber.ToString();
                lblQuestion.Text = question.Text;
                mlblMinusLabel.Text = question.MinusLabel;
                mlblPlusLabel.Text = question.PlusLabel;

                //load answer
                //load score
                if (answer.Score == int.MinValue)
                {
                    //no score is selected
                    //uncheck any button
                    foreach (RadioButton button in scoreButtons)
                    {
                        //check if button is checked
                        button.Checked = false;
                    }
                }
                else
                {
                    //select score
                    //find corresponding button
                    RadioButton button = scoreButtons.Find(
                        b => b.Tag.ToString().Equals(answer.Score.ToString()));

                    //check result
                    if (button != null)
                    {
                        //check score button
                        button.Checked = true;
                    }
                }
            }
            finally
            {
                //reset loading flag
                isLoading = false;

                //set fields
                this.answer = answer;
            }
        }

        #endregion Public Methods
        

        #region Private Methods *******************************************************

        #endregion Private Methods


        #region UI Event Handlers *****************************************************

        /// <summary>
        /// Control load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuestionGradeControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //set font
            lblQuestion.Font = MetroFramework.MetroFonts.DefaultLight(14);
        }

        /// <summary>
        /// Score radio button checked changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbScore_CheckedChanged(object sender, EventArgs e)
        {
            //check if is loading data
            if (isLoading)
            {
                //skip event
                //exit
                return;
            }

            //check answer
            if (answer == null)
            {
                //cannot load data
                //exit
                return;
            }

            //check if sender radio button is not checked
            if (!((RadioButton)sender).Checked)
            {
                //no need to handle event twice
                //exit
                return;
            }

            //find checked radio button
            RadioButton checkedButton = scoreButtons.Find(b => b.Checked);

            //check result
            if (checkedButton == null)
            {
                //no button is checked
                //reset score
                answer.Score = int.MinValue;
            }
            else
            {
                //update score
                answer.Score = int.Parse(checkedButton.Tag.ToString());
            }

            //answer was updated
            answer.Updated = true;
        }

        /// <summary>
        /// Score label click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mlblScore_Click(object sender, EventArgs e)
        {
            //check if is loading data
            if (isLoading)
            {
                //skip event
                //exit
                return;
            }

            //check answer
            if (answer == null)
            {
                //skip event
                //exit
                return;
            }

            //get clicked label
            MetroFramework.Controls.MetroLabel label = (MetroFramework.Controls.MetroLabel)sender;

            //find corresponding button
            RadioButton button = scoreButtons.Find(
                b => b.Tag.ToString().Equals(label.Tag.ToString()));

            //check result
            if (button == null)
            {
                //should never happen
                //exit
                return;
            }

            //check button 
            button.Checked = true;
        }

        #endregion UI Event Handlers

    } //end of class QuestionGradeControl

} //end of namespace PnT.SongClient.UI.Controls
