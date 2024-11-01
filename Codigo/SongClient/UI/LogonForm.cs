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
    /// Logon form. Let user log on into system.
    /// </summary>
    public partial class LogonForm : MetroForm
    {

        #region Constants *************************************************************

        /// <summary>
        /// The maximum amount of time to wait while trying to log on. In seconds.
        /// </summary>
        public const int LOGON_TIMEOUT = 20;

        #endregion Constants


        #region Fields ****************************************************************

        /// <summary>
        /// Option that indicates if this is the first logon of the local user.
        /// </summary>
        private bool isFirstLogon = true;

        /// <summary>
        /// Indicates if form is waiting for connection to log on to server.
        /// </summary>
        private bool isWaitingToLogOn = false;

        /// <summary>
        /// The entered user already validated.
        /// </summary>
        private User validatedUser = null;

        #endregion Fields


        #region Constructor ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="isFirstLogon">
        /// True if this is the first logon of the local user.
        /// </param>
        public LogonForm(bool isFirstLogon)
        {
            //set fields
            this.isFirstLogon = isFirstLogon;

            //init ui components
            InitializeComponent();
        }

        #endregion Constructor


        #region Private Methods *******************************************************

        /// <summary>
        /// Wait application to connect before performing logon.
        /// </summary>
        /// <param name="state"></param>
        private void WaitToLogOn(object state)
        {
            //get current time
            DateTime startTime = DateTime.Now;

            //keep checking connection while wait has not timed out
            while (Manager.WebServiceManager.Status != Data.WebServiceStatus.Connected &&
                DateTime.Now.Subtract(startTime).TotalSeconds < LOGON_TIMEOUT)
            {
                //wait some more time
                Thread.Sleep(100);
            }

            //invoke ok button click event handler method
            this.Invoke(new ClickDelegate(mbtnOK_Click),
                new object[] { this, new EventArgs() });
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Form load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogonForm_Load(object sender, EventArgs e)
        {
            //resize form to smaller size
            this.Size = new Size(this.Size.Width, this.Size.Height - 70);

            //set last user login to field
            mtxtUser.Text = Manager.Settings.LastUserLogin;
        }

        /// <summary>
        /// Form shown event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogonForm_Shown(object sender, EventArgs e)
        {
            //check if there is no previous user
            if (mtxtUser.Text.Length == 0)
            {
                //focus user field
                mtxtUser.Focus();
            }
            else
            {
                //check if this is the first logon
                if (isFirstLogon)
                {
                    //focus password field
                    mtxtPassword.Focus();

#if DEBUG
                    //prform login to save development time
                    //set password
                    mtxtPassword.Text = "1234";

                    //simulate OK button click
                    mbtnOK_Click(this, new EventArgs());
#endif
                }
                else
                {
                    //focus user field and select its text
                    mtxtUser.Focus();
                    mtxtUser.SelectAll();
                }
            }
        }

        /// <summary>
        /// OK button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnOK_Click(object sender, EventArgs e)
        {
            //check input data
            if (mtxtUser.Text.Length == 0 || mtxtPassword.Text.Length == 0)
            {
                //should never happen
                //disable button
                mbtnOK.Enabled = false;

                //exit
                return;
            }

            //check if there is a validated user
            if (validatedUser != null)
            {
                //check input data
                if (mtxtNewPassword.Text.Length == 0 || mtxtConfirmPassword.Text.Length == 0)
                {
                    //should never happen
                    //disable button
                    mbtnOK.Enabled = false;

                    //exit
                    return;
                }

                //check new password length
                if (mtxtNewPassword.Text.Length < 6)
                {
                    //display message
                    MetroMessageBox.Show(Manager.MainForm,
                        Properties.Resources.msgInvalidShortPassword,
                        Properties.Resources.titleInvalidNewPassword,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //focus new password field
                    mtxtNewPassword.Focus();

                    //select text
                    mtxtNewPassword.SelectAll();

                    //clear confirm password field
                    mtxtConfirmPassword.Text = string.Empty;

                    //exit
                    return;
                }

                //compare new password
                if (mtxtNewPassword.Text.Equals(mtxtPassword.Text))
                {
                    //display message
                    MetroMessageBox.Show(Manager.MainForm,
                        Properties.Resources.msgInvalidSamePassword,
                        Properties.Resources.titleInvalidNewPassword,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //focus new password field
                    mtxtNewPassword.Focus();

                    //select text
                    mtxtNewPassword.SelectAll();

                    //clear confirm password field
                    mtxtConfirmPassword.Text = string.Empty;

                    //exit
                    return;
                }

                //confirm new password
                if (!mtxtNewPassword.Text.Equals(mtxtConfirmPassword.Text))
                {
                    //display message
                    MetroMessageBox.Show(Manager.MainForm,
                        Properties.Resources.msgInvalidConfirmedPassword,
                        Properties.Resources.titleInvalidNewPassword,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //focus confirm password field
                    mtxtConfirmPassword.Focus();

                    //select text
                    mtxtConfirmPassword.SelectAll();

                    //exit
                    return;
                }
            }

            //check if was waiting to log on
            if (isWaitingToLogOn)
            {
                //set cursor back to normal
                this.Cursor = Cursors.Default;

                //enable all ui
                tlpMain.Enabled = true;
            }
            
            //logon
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //check if was not waiting to log on
                if (!isWaitingToLogOn)
                {
                    //channel is not available at the moment
                    //wait to log on using another thread
                    isWaitingToLogOn = true;

                    //disable all ui
                    tlpMain.Enabled = false;

                    //set cursor
                    this.Cursor = Cursors.WaitCursor;

                    ThreadPool.QueueUserWorkItem(WaitToLogOn);

                    //exit
                    return;
                }
                else
                {
                    //was already waiting
                    //display message
                    MetroMessageBox.Show(Manager.MainForm,
                        Properties.Resources.msgLogonTimeout,
                        Properties.Resources.titleNoConnection);

                    //reset waiting flag
                    isWaitingToLogOn = false;

                    //exit
                    return;
                }
            }
            else
            {
                //channel is available
                //no need to wait
                //reset waiting flag
                isWaitingToLogOn = false;
            }

            try
            {
                //the logon user
                User user = null;

                //check if there is no validated user yet
                if (validatedUser == null)
                {
                    //logon user into system
                    user = songChannel.LogonUser(
                        mtxtUser.Text, Criptography.Encrypt(mtxtPassword.Text));

                    //check result
                    if (user.Result == (int)SelectResult.Empty)
                    {
                        //invalid user and password
                        //display message
                        MetroMessageBox.Show(Manager.MainForm,
                            Properties.Resources.msgInvalidUser,
                            Properties.Resources.titleFailedLogIn,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        //set focus to password textbox
                        mtxtPassword.Focus();

                        //exit
                        return;
                    }
                    else if (user.Result == (int)SelectResult.FatalError)
                    {
                        //database error while getting roles
                        //display message
                        MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.errorWebServiceLogonUser, user.ErrorMessage),
                            Properties.Resources.titleWebServiceError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //write error
                        Manager.Log.WriteError(string.Format(
                            Properties.Resources.errorWebServiceLogonUser, user.ErrorMessage));

                        //exit
                        return;
                    }

                    //check if user must change password
                    if (user.RequestPasswordChange)
                    {
                        //request user to change password
                        //store already validated user
                        validatedUser = user;

                        //display message
                        MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.msgEnterNewPassword, user.Login),
                            Properties.Resources.titleNewPassword,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //display new password fields
                        mlblNewPassword.Visible = true;
                        mtxtNewPassword.Visible = true;
                        mlblConfirmPassword.Visible = true;
                        mtxtConfirmPassword.Visible = true;

                        //clear new password fields
                        mtxtNewPassword.Text = string.Empty;
                        mtxtConfirmPassword.Text = string.Empty;

                        //disable login fields
                        mtxtUser.Enabled = false;
                        mtxtPassword.Enabled = false;

                        //disable OK button
                        mbtnOK.Enabled = false;

                        //resize form to normal size
                        this.Size = new Size(this.Size.Width, this.Size.Height + 70);

                        //focus new password field
                        mtxtNewPassword.Focus();

                        //exit
                        return;
                    }
                }
                else
                {
                    //no need to logon again
                    //set validated user
                    user = validatedUser;

                    //change user password and get result
                    SaveResult result = songChannel.ChangeUserPassword(
                        mtxtUser.Text, Criptography.Encrypt(mtxtPassword.Text),
                        Criptography.Encrypt(mtxtNewPassword.Text));

                    //check result
                    if (result.Result == (int)SelectResult.FatalError)
                    {
                        //web service could not change password
                        //should never happen
                        //display message
                        MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.errorWebServiceChangeUserPassword, result.ErrorMessage),
                            Properties.Resources.titleSaveError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //write error
                        Manager.Log.WriteError(string.Format(
                            Properties.Resources.errorWebServiceLogonUser, result.ErrorMessage));

                        //exit
                        return;
                    }

                    //password was changed
                    //display message
                    MetroMessageBox.Show(Manager.MainForm,
                        Properties.Resources.msgNewPasswordSaved,
                        Properties.Resources.titleNewPassword,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //get assigned permissions for assigned role
                List<Permission> permissions = songChannel.FindPermissionsByRole(user.RoleId);

                //check result
                if (permissions[0].Result == (int)SelectResult.Empty)
                {
                    //role has no permission
                    //clear list
                    permissions.Clear();
                }
                else if (permissions[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting permissions
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceLogonUser, permissions[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceLogonUser, permissions[0].ErrorMessage));

                    //exit
                    return;
                }

                //set logged on user
                Manager.SetLogonUser(user, permissions);

                //update last user login
                Manager.Settings.LastUserLogin = mtxtUser.Text;

                //set dialog result and exit
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                //database error while getting roles
                //write error
                Manager.Log.WriteException(
                    Properties.Resources.errorChannelLogonUser, ex);

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

        /// <summary>
        /// User data textbox key down event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtUserData_KeyDown(object sender, KeyEventArgs e)
        {
            //check pressed button
            if (e.KeyCode == Keys.Enter)
            {
                //simulate an OK button press
                mbtnOK_Click(sender, new EventArgs());

                //avoid annoying ding
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// User data textbox key up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtUserData_KeyUp(object sender, KeyEventArgs e)
        {
            //check if there is no validated user yet
            if (validatedUser == null)
            {
                //enable OK button if user and password are entered
                mbtnOK.Enabled = mtxtUser.Text.Length > 0 && mtxtPassword.Text.Length > 0;
            }
            else
            {
                //enable OK button if new password is entered and confirmed
                mbtnOK.Enabled = mtxtNewPassword.Text.Length > 0 && mtxtConfirmPassword.Text.Length > 0;
            }
        }

        /// <summary>
        /// Reset password link click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mlnkResetPassword_Click(object sender, EventArgs e)
        {
            //create reset password form
            ResetPasswordForm resetForm = new ResetPasswordForm(mtxtUser.Text);

            //display form as a dialog and get result
            if (resetForm.ShowDialog(this) == DialogResult.OK)
            {
                //set selected user login to user field
                mtxtUser.Text = resetForm.UserLogin;
            }

            //check if user is set
            if (mtxtUser.Text.Length > 0)
            {
                //focus password field
                mtxtPassword.Focus();
            }
            else
            {
                //focus user field
                mtxtUser.Focus();
            }
        }

        #endregion Event Handlers

    } //end of class LogonForm

} //end of namespace PnT.SongClient.UI
