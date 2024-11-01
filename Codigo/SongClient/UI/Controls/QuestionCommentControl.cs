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
    public partial class QuestionCommentControl : UserControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The displayed previous answer if any.
        /// </summary>
        private Answer answer = null;

        /// <summary>
        /// Indicates if data is being loaded.
        /// </summary>
        private bool isLoading = false;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public QuestionCommentControl()
        {
            //init UI components
            InitializeComponent();
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
        /// Enable editable fields according to selected option.
        /// </summary>
        /// <param name="enable">
        /// True to enable editable fields.
        /// False otherwise.
        /// </param>
        public void EnableFields(bool enable)
        {
            //enable fields
            mtxtComments.Enabled = enable;
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

                //load answer
                //load comments
                mtxtComments.Text = answer.Comments;
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
        private void QuestionCommentControl_Load(object sender, EventArgs e)
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
        /// Comments text box text changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtComments_TextChanged(object sender, EventArgs e)
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

            //update comments
            answer.Comments = mtxtComments.Text;

            //answer was updated
            answer.Updated = true;
        }

        #endregion UI Event Handlers
    }
}
