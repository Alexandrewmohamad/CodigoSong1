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

using PnT.SongDB.Logic;

namespace PnT.SongClient.UI
{

    /// <summaryinactivation reason form.
    /// Select student form.
    /// </summary>
    public partial class InactivationReasonForm : MetroForm
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The inactivation reason.
        /// </summary>
        private string inactivationReason = null;

        #endregion Fields


        #region Constructor ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="itemTypeName">
        /// The name of the type of the edited item.
        /// </param>
        /// <param name="itemStatus">
        /// The current item status.
        /// </param>
        /// <param name="inactivationReason">
        /// The initial inactivation reason.
        /// Null if there is no reason yet.
        /// </param>
        public InactivationReasonForm(
            string itemTypeName, int itemStatus, string inactivationReason)
        {
            //init ui components
            InitializeComponent();

            //set fields
            this.inactivationReason = inactivationReason;

            //set item type name and item status name to ui message
            mlblMessage.Text = string.Format(
                mlblMessage.Text, itemTypeName.ToLower(),
                Properties.Resources.ResourceManager.GetString(
                    "ItemStatus_" + ((ItemStatus)itemStatus).ToString()).ToLower());

            //create list of reasons
            mcbReason.Items.Add(Properties.Resources.inactivationReason_EvadedStudent);
            mcbReason.Items.Add(Properties.Resources.inactivationReason_NoInterest);
            mcbReason.Items.Add(Properties.Resources.inactivationReason_NoRegistrationRenewal);
            mcbReason.Items.Add(Properties.Resources.inactivationReason_DuplicateRegister);
            mcbReason.Items.Add(Properties.Resources.inactivationReason_SchedulingConflict);
            mcbReason.Items.Add(Properties.Resources.inactivationReason_ChangeOfAddress);
            mcbReason.Items.Add(Properties.Resources.inactivationReason_ContractTermination);
        }

        #endregion Constructor


        #region Properties ************************************************************

        /// <summary>
        /// Get the inactivation reason.
        /// </summary>
        public string InactivationReason
        {
            get
            {
                return inactivationReason;
            }
        }



        #endregion Properties

        #region Private Methods *******************************************************

        #endregion Private Methods

        #region Event Handlers ********************************************************

        /// <summary>
        /// Inactivation reason form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InactivationReasonForm_Load(object sender, EventArgs e)
        {
            //check if there is a initial reason
            if (this.inactivationReason != null &&
                this.inactivationReason.Length > 0)
            {
                //set initial reason text
                mcbReason.Text = this.inactivationReason;
            }
        }

        /// <summary>
        /// Reaason combobox text changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbReason_TextChanged(object sender, EventArgs e)
        {
            //enable OK button if any reason was set
            mbtnOK.Enabled = mcbReason.Text.Length > 0;
        }

        /// <summary>
        /// OK button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnOK_Click(object sender, EventArgs e)
        {
            //check if any reason was set
            if (mcbReason.Text.Length == 0)
            {
                //no reason was set yet
                //should never happen
                //disable OK button
                mbtnOK.Enabled = false;

                //exit
                return;
            }

            //set inactivation reason
            this.inactivationReason = mcbReason.Text;

            //set dialog result to OK
            this.DialogResult = DialogResult.OK;
        }

        #endregion Event Handlers

    } //end of class InactivationReasonForm

} //end of namespace PnT.SongClient.UI
