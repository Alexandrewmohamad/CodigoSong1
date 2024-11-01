using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using MetroFramework;
using MetroFramework.Forms;

using PnT.SongServer;
using PnT.SongDB.Logic;
using PnT.SongDB.Mapper;

using PnT.SongClient.Logic;

namespace PnT.SongClient.UI
{
    /// <summary>
    /// Let user reset password for selected user.
    /// </summary>
    public partial class ResetPasswordForm : MetroForm
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The selected user login.
        /// </summary>
        string userLogin = string.Empty;

        #endregion Fields


        #region Constructor ***********************************************************

        /// <summary>
        ///Default constructor.
        /// </summary>
        public ResetPasswordForm(string userLogin)
        {
            //init UI components
            InitializeComponent();

            //set fields
            this.userLogin = userLogin;
        }

        #endregion Constructor


        #region Properties ************************************************************

        /// <summary>
        /// Get the selected user login.
        /// </summary>
        public string UserLogin
        {
            get
            {
                return userLogin;
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
        private void ResetPasswordForm_Load(object sender, System.EventArgs e)
        {
            //set user login to UI
            mtxtUser.Text = this.userLogin;
        }

        /// <summary>
        /// User textbox text changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtUser_TextChanged(object sender, System.EventArgs e)
        {
            //enable ok button if any user was typed
            mbtnOK.Enabled = mtxtUser.Text.Length > 0;
        }

        /// <summary>
        /// OK button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnOK_Click(object sender, System.EventArgs e)
        {
            //check input user
            if (mtxtUser.Text.Length == 0)
            {
                //should never happen
                //disable OK button
                mbtnOK.Enabled = false;

                //exit
                return;
            }

            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //was already waiting
                //display message
                MetroMessageBox.Show(Manager.MainForm,
                    Properties.Resources.msgWaitConnection,
                    Properties.Resources.titleNoConnection,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //exit
                return;
            }

            try
            {
                //get selected user by login
                User user = songChannel.FindUserByLogin(mtxtUser.Text);

                //check result
                if (user.Result == (int)SelectResult.Empty)
                {
                    //invalid user
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.msgUserNotFoundResetPassword, mtxtUser.Text),
                        Properties.Resources.titleInvalidUser,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //set focus to user textbox
                    mtxtUser.Focus();
                    mtxtUser.SelectAll();

                    //exit
                    return;
                }
                else if (user.Result == (int)SelectResult.FatalError)
                {
                    //database error while getting user
                    //display message
                    MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        Properties.Resources.item_User, user.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLoadData,
                        Properties.Resources.item_User, user.ErrorMessage));

                    //exit
                    return;
                }

                //reset and send password for selected user
                SendResult sendResult = songChannel.SendRecoveryPasswordToUser(user.UserId);

                //check result
                if (sendResult.Result == (int)SelectResult.FatalError)
                {
                    //database error while sending recovery password to user
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceSendRecoveryPassword, 
                        sendResult.ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceSendRecoveryPassword, 
                        sendResult.ErrorMessage));

                    //exit
                    return;
                }

                //email was sent
                //display message
                MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.msgRecoveryPasswordSent, user.Login),
                    Properties.Resources.titleEmailSent,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);


                //set selected user login
                this.userLogin = user.Login;

                //set dialog result and exit
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                //database error while resetting password
                //write error
                Manager.Log.WriteException(
                    Properties.Resources.errorChannelResetPassword, ex);

                //log exception
                Manager.Log.WriteException(ex);
            }
            finally
            {
                //check channel
                if (songChannel != null)
                {
                    //close channel
                    ((System.ServiceModel.IClientChannel)songChannel).Close();
                }
            }
        }

        #endregion Event Handlers

    } //end of class ResetPasswordForm

} //end of namespace PnT.SongClient.UI
