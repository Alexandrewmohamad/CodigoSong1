using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetroFramework;
using MetroFramework.Forms;
using PnT.SongDB.Logic;
using PnT.SongServer;
using PnT.SongClient.Logic;

namespace PnT.SongClient.UI
{
    /// <summary>
    /// Select file form.
    /// </summary>
    public partial class SelectFileForm : MetroForm
    {

        #region Fields ****************************************************************

        /// <summary>
        /// True if user can select file.
        /// False only if previous selected file is displayed.
        /// </summary>
        bool enabled = false;

        /// <summary>
        /// The path of the selected file.
        /// </summary>
        string selectedFile = string.Empty;

        #endregion Fields


        #region Constructor ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="selectedFile">
        /// The path of the initially selected file.
        /// </param>
        /// <param name="enabled">
        /// True if user can select another file.
        /// False otherwise.
        /// </param>
        public SelectFileForm(string selectedFile, bool enabled)
        {
            //check selected file path
            if (selectedFile != null)
            {
                //set selected file path field
                this.selectedFile = selectedFile;
            }

            //set enabled
            this.enabled = enabled;

            //init ui components
            InitializeComponent();
        }

        #endregion Constructor


        #region Properties ************************************************************

        /// <summary>
        /// Get the path of the selected file.
        /// </summary>
        public string SelectedFile
        {
            get
            {
                return selectedFile;
            }
        }

        #endregion Properties


        #region Private Methods *******************************************************

        /// <summary>
        /// Display image in the selected picture box.
        /// Resample image according to picture box size.
        /// </summary>
        /// <param name="image">
        /// The image to be displayed.
        /// </param>
        private void DisplayImage(Bitmap image)
        {
            //get image ration
            double imgRation = (double)image.Width / (double)image.Height;

            //get picture box ratio
            double pbRatio = (double)pbImage.Width / (double)pbImage.Height;

            //must calculate ration
            double ratio;

            //compare rations
            if (imgRation >= pbRatio)
            {
                //image is wider
                ratio = (double)pbImage.Width / (double)image.Width;
            }
            else
            {
                //image is taller
                ratio = (double)pbImage.Height / (double)image.Height;
            }

            //create a resampled image
            System.Drawing.Bitmap resampledImage = new System.Drawing.Bitmap(
                (int)(image.Width * ratio), (int)(image.Height * ratio));

            //paint on the resampled image
            using (System.Drawing.Graphics G =
                System.Drawing.Graphics.FromImage(resampledImage))
            {
                //draw original image with maximum quality
                G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                G.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                G.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                G.DrawImage(image, 0, 0, resampledImage.Width, resampledImage.Height);
            }

            //display resampled image
            pbImage.Image = resampledImage;
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Form load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFileForm_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //enable find button according to option
            mbtnFindFile.Enabled = enabled;

            //check file path
            if (selectedFile.Length == 0)
            {
                //no file is selected yet
                //disable picture box
                pbImage.Enabled = false;
                pbImage.Cursor = Cursors.Default;

                //exit
                return;
            }

            //check if file was not download yet
            if (!Manager.FileManager.HasFile(selectedFile))
            {
                //get song channel
                ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

                //check result
                if (songChannel == null)
                {
                    //channel is not available at the moment
                    //could not load image
                    return;
                }

                try
                {
                    //download file
                    SongDB.Logic.File file = songChannel.GetFile(selectedFile);

                    //check result
                    if (file.Result == (int)SelectResult.Success)
                    {
                        //save file to disk
                        if (!Manager.FileManager.SaveFile(file))
                        {
                            //error while saving file to disk
                            //display message
                            MetroMessageBox.Show(Manager.MainForm, string.Format(
                                Properties.Resources.errorWriteSongFile, file.FilePath),
                                Properties.Resources.wordError,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (file.Result == (int)SelectResult.Empty)
                    {
                        //file was not found
                        //should never happen
                        //exit
                        return;
                    }
                    else if (file.Result == (int)SelectResult.FatalError)
                    {
                        //file error while getting file
                        //display message
                        MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.errorWebServiceLoadFile,
                            selectedFile, file.ErrorMessage),
                            Properties.Resources.titleWebServiceError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //log error
                        Manager.Log.WriteError(string.Format(
                            Properties.Resources.errorWebServiceLoadFile,
                            selectedFile, file.ErrorMessage));

                        //set dialog result to cancel
                        this.DialogResult = DialogResult.Cancel;

                        //exit
                        return;
                    }
                }
                catch (Exception ex)
                {
                    //database error while getting file
                    //show error message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorChannelLoadFile,
                        selectedFile, ex.Message),
                        Properties.Resources.titleChannelError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //log exception
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorChannelLoadFile,
                        selectedFile, ex.Message));
                    Manager.Log.WriteException(ex);

                    //set dialog result to cancel
                    this.DialogResult = DialogResult.Cancel;

                    //exit
                    return;
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

            //display file
            //set picture box cursor
            pbImage.Cursor = Cursors.Hand;

            //enable delete button according to option
            mbtnDeleteFile.Enabled = enabled;

            //check format
            if (selectedFile.ToLower().EndsWith(".pdf"))
            {
                //display pdf thumbnail
                DisplayImage(Properties.Resources.IconPDF);

                //exit
                return;
            }

            //only image formats remain
            //try to get file image
            Bitmap image = Manager.FileManager.GetImage(selectedFile);

            //check result
            if (image != null)
            {
                //display image
                DisplayImage(image);
            }
            else
            {
                //should never happen
                //disable picture box
                pbImage.Enabled = false;
                pbImage.Cursor = Cursors.Default;

                //disable delete button
                mbtnDeleteFile.Enabled = false;

                //exit
                return;
            }
        }

        /// <summary>
        /// Image click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbImage_Click(object sender, EventArgs e)
        {
            //check if there is no file to be downloaded
            if (pbImage.Image == null || selectedFile.Length == 0)
            {
                //no file is selected yet
                //disable picture box
                pbImage.Enabled = false;
                pbImage.Cursor = Cursors.Default;

                //exit
                return;
            }

            //get file info for selected file
            FileInfo fileInfo = Manager.FileManager.GetFileInfo(selectedFile);

            //get extension
            string extension = fileInfo.Extension.ToLower();

            //check extension
            if (extension != null && extension.Length > 0)
            {
                //check if extension starts with '.'
                if (extension[0].Equals('.'))
                {
                    //remove '.'
                    extension = extension.Substring(1);
                }

                //set save file dialog filter according to extension
                sfdFile.Filter = Properties.Resources.ResourceManager.GetString(
                    "filter_" + extension);

                //set save file dialog default extension
                sfdFile.DefaultExt = extension;
            }

            //set file default name
            sfdFile.FileName = fileInfo.Name;

            //display save file dialog and check result
            if (sfdFile.ShowDialog() != DialogResult.OK)
            {
                //user canceled operation
                //exit
                return;
            }

            try
            {
                //copy file to selected path
                fileInfo.CopyTo(sfdFile.FileName, true);
            }
            catch (Exception ex)
            {
                //unexpected error while copying file
                //display message
                MetroMessageBox.Show(this, string.Format(
                    Properties.Resources.errorSaveFile,
                    sfdFile.FileName, ex.Message),
                    Properties.Resources.titleSaveError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log exception
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorSaveFile,
                    sfdFile.FileName, ex.Message), ex);
            }
        }

        /// <summary>
        /// Find file click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnFindFile_Click(object sender, EventArgs e)
        {
            //show open file dialog and check result
            if (ofdFile.ShowDialog() != DialogResult.OK)
            {
                //user canceled operation
                //exit
                return;
            }

            try
            {
                //get selected file info 
                FileInfo fileInfo = new FileInfo(ofdFile.FileName);

                //create file path where to save temporary file
                string filePath = string.Empty;

                //avoid existing file name
                int index = 1;

                do
                {
                    //set file path
                    filePath = Manager.FILE_DIR_PATH + "\\Temp\\tempFile" + (index++) + 
                        fileInfo.Extension.ToLower();
                }
                while (System.IO.File.Exists(filePath));

                //create directories if needed
                new FileInfo(filePath).Directory.Create();
                
                //check extension
                if (fileInfo.Extension.ToLower().Equals(".pdf"))
                {
                    //pdf file
                    #region process pdf file

                    //just copy the pdf file
                    fileInfo.CopyTo(filePath, true);

                    //display pdf thumbnail
                    DisplayImage(Properties.Resources.IconPDF);

                    #endregion process pdf file
                }
                else
                {
                    //image file
                    #region process image file

                    //get image from selected file
                    Bitmap image = null;

                    //open image file
                    using (Stream stream = System.IO.File.Open(
                        fileInfo.FullName, System.IO.FileMode.Open))
                    {
                        //read image
                        image = new Bitmap(Image.FromStream(stream));
                    }

                    //calculate new width and height when resizing image
                    int width, height;

                    //check if image should be resized
                    if (image.Width > 1400 || image.Height > 1400)
                    {

                        //check if largest side
                        if (image.Width > image.Height)
                        {
                            //set new sizes keeping aspect ratio
                            width = 1400;
                            height = (int)(image.Height * (1400.0 / ((double)image.Width)));
                        }
                        else
                        {
                            //set new sizes keeping aspect ratio
                            width = (int)(image.Width * (1400.0 / ((double)image.Height)));
                            height = 1400;
                        }

                        //create a resampled image
                        System.Drawing.Bitmap resampledImage =
                            new System.Drawing.Bitmap(width, height);

                        //paint on the resampled image
                        using (System.Drawing.Graphics G =
                            System.Drawing.Graphics.FromImage(resampledImage))
                        {
                            //draw original image with maximum quality
                            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            G.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            G.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                            G.DrawImage(image, 0, 0, width, height);
                        }

                        //set resampled image as the image
                        image.Dispose();
                        image = resampledImage;
                    }

                    //save image file as jpg file always
                    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //create thumbnail
                    //check largest side
                    if (image.Width > image.Height)
                    {
                        //set new sizes keeping aspect ratio
                        width = 200;
                        height = (int)(image.Height * (200.0 / ((double)image.Width)));
                    }
                    else
                    {
                        //set new sizes keeping aspect ratio
                        width = (int)(image.Width * (200.0 / ((double)image.Height)));
                        height = 200;
                    }

                    //create a resampled image
                    System.Drawing.Bitmap thumbnailImage =
                        new System.Drawing.Bitmap(width, height);

                    //paint on the resampled image
                    using (System.Drawing.Graphics G =
                        System.Drawing.Graphics.FromImage(thumbnailImage))
                    {
                        //draw original image with maximum quality
                        G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        G.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        G.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        G.DrawImage(image, 0, 0, width, height);
                    }

                    //save thumbnail file as jpg file
                    thumbnailImage.Save(filePath.Replace(".jpg", ".thumbnail.jpg"),
                        System.Drawing.Imaging.ImageFormat.Jpeg);

                    //display loaded image
                    DisplayImage(image);

                    #endregion process image file
                }

                //update selected file
                //remove base file directory path
                selectedFile = filePath.Replace(Manager.FILE_DIR_PATH + "\\", "");

                //enable ok button
                mbtnOK.Enabled = true;
            }
            catch (Exception ex)
            {
                //unexpected error while processing selected file
                //display message
                MetroMessageBox.Show(this, string.Format(
                    Properties.Resources.errorProcessFile, ex.Message),
                    Properties.Resources.titleProcessError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log exception
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorProcessFile, ex.Message), ex);
            }
        }

        #endregion Event Handlers

    } //end of class SelectFileForm

} //end of namespace PnT.SongClient.UI
