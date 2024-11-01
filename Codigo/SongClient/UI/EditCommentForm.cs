using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetroFramework;
using MetroFramework.Forms;


namespace PnT.SongClient.UI
{

    /// <summary>
    /// Edit comment form. Let user edit a comment.
    /// </summary>
    public partial class EditCommentForm : MetroForm
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The comment to be edited.
        /// </summary>
        string comment = string.Empty;

        #endregion Fields


        #region Constructor ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EditCommentForm(string comment)
        {
            //set field
            this.comment = comment;

            //init ui components
            InitializeComponent();
        }

        #endregion Constructor


        #region Properties ************************************************************

        /// <summary>
        /// Get edited comment.
        /// </summary>
        public string Comment
        {
            get
            {
                return comment;
            }
        }

        #endregion Properties


        #region Private Methods *******************************************************

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Form load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditCommentForm_Load(object sender, EventArgs e)
        {
            //display comment
            mtxtComment.Text = this.comment;
        }

        /// <summary>
        /// OK button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnOK_Click(object sender, EventArgs e)
        {
            //check entered comment
            if (mtxtComment.Text.Length == 0)
            {
                //should never happen
                //cancel edition
                DialogResult = DialogResult.Cancel;

                //exit
                return;
            }

            //set comment
            this.comment = mtxtComment.Text;

            //set final result
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Comment textbox text changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtComment_TextChanged(object sender, EventArgs e)
        {
            //enable OK button if any text
            mbtnOK.Enabled = mtxtComment.Text.Length > 0;
        }

        #endregion Event Handlers

    } //end of class EditCommentForm 

} //end of namespace PnT.SongClient.UI
