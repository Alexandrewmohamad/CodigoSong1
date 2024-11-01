using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetroFramework.Forms;
using MetroFramework;

using PnT.SongDB.Logic;
using PnT.SongServer;
using PnT.SongClient.Logic;


namespace PnT.SongClient.UI
{

    /// <summary>
    /// Display application setting options.
    /// </summary>
    public partial class OptionsForm : MetroForm
    {

        #region Fields ****************************************************************

        /// <summary>
        /// Option to select and display options from a grid when loading.
        /// </summary>
        private string displayGridOptions = string.Empty;

        /// <summary>
        /// Indicates if user has edited web service server settings.
        /// </summary>
        private bool hasEditedServer = false;

        /// <summary>
        /// List of semesters.
        /// </summary>
        private List<Semester> semesters = null;

        /// <summary>
        /// The current selected semester.
        /// </summary>
        private Semester selectedSemester = null;

        /// <summary>
        /// Current List of available columns.
        /// </summary>
        BindingList<NamedColumn> availableColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// Current list of selected columns.
        /// </summary>
        BindingList<NamedColumn> selectedColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of class available columns.
        /// </summary>
        BindingList<NamedColumn> classAvailableColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of class selected columns.
        /// </summary>
        BindingList<NamedColumn> classSelectedColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of event available columns.
        /// </summary>
        BindingList<NamedColumn> eventAvailableColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of event selected columns.
        /// </summary>
        BindingList<NamedColumn> eventSelectedColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of institution available columns.
        /// </summary>
        BindingList<NamedColumn> institutionAvailableColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of institution selected columns.
        /// </summary>
        BindingList<NamedColumn> institutionSelectedColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of instrument available columns.
        /// </summary>
        BindingList<NamedColumn> instrumentAvailableColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of instrument selected columns.
        /// </summary>
        BindingList<NamedColumn> instrumentSelectedColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of pole available columns.
        /// </summary>
        BindingList<NamedColumn> poleAvailableColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of pole selected columns.
        /// </summary>
        BindingList<NamedColumn> poleSelectedColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of registration available columns.
        /// </summary>
        BindingList<NamedColumn> registrationAvailableColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of registration selected columns.
        /// </summary>
        BindingList<NamedColumn> registrationSelectedColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of report available columns.
        /// </summary>
        BindingList<NamedColumn> reportAvailableColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of report selected columns.
        /// </summary>
        BindingList<NamedColumn> reportSelectedColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of student available columns.
        /// </summary>
        BindingList<NamedColumn> studentAvailableColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of student selected columns.
        /// </summary>
        BindingList<NamedColumn> studentSelectedColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of teacher available columns.
        /// </summary>
        BindingList<NamedColumn> teacherAvailableColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of teacher selected columns.
        /// </summary>
        BindingList<NamedColumn> teacherSelectedColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of user available columns.
        /// </summary>
        BindingList<NamedColumn> userAvailableColumns = new BindingList<NamedColumn>();

        /// <summary>
        /// List of user selected columns.
        /// </summary>
        BindingList<NamedColumn> userSelectedColumns = new BindingList<NamedColumn>();

        #endregion Fields


        #region Constructor ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="displayGridOptions">
        /// Display options for a selected grid.
        /// String.Empty not to display options for any specific grid.
        /// </param>
        public OptionsForm(string displayGridOptions)
        {
            //set fields
            this.displayGridOptions = displayGridOptions;

            //init ui components
            InitializeComponent();
        }

        #endregion Constructor


        #region Properties ************************************************************

        /// <summary>
        /// True if user has edited web service server settings.
        /// </summary>
        public bool HasEditedServer
        {
            get
            {
                return hasEditedServer;
            }
        }

        /// <summary>
        /// Get current selected grid value.
        /// </summary>
        private string SelectedGrid
        {
            get
            {
                //return selected grid value
                return mcbGrid.SelectedIndex == -1 ?
                    string.Empty : mcbGrid.SelectedItem.ToString();
            }
            set
            {
                //check each item
                foreach (Object item in mcbGrid.Items)
                {
                    //check item
                    if (item.ToString().Equals(value))
                    {
                        //select item
                        mcbGrid.SelectedItem = item;
                    }
                }
            }
        }

        #endregion


        #region Private Methods *******************************************************

        /// <summary>
        /// Get all accepted cultures and put it on the language combobox.
        /// </summary>
        private void ListAcceptedCultures()
        {
            //default invariant culture
            NamedCulture defaultLanguage = new NamedCulture();
            defaultLanguage.name = "International (English)";
            defaultLanguage.culture = CultureInfo.InvariantCulture;
            mcbLanguages.Items.Add(defaultLanguage);
            
            //pt-BR culture
            NamedCulture ptBrLanguage = new NamedCulture();
            ptBrLanguage.name = "Português (Brasil)";
            ptBrLanguage.culture = new CultureInfo("pt-BR");
            mcbLanguages.Items.Add(ptBrLanguage);
        }

        /// <summary>
        /// Load semester data from server.
        /// </summary>
        private void LoadSemesters()
        {
            //load semesters
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //exit
                return;
            }

            try
            {
                //get list of all semesters
                semesters = songChannel.FindSemesters(false);

                //check result
                if (semesters[0].Result == (int)SelectResult.Empty)
                {
                    //no semester is available
                    //clear list
                    semesters.Clear();

                    //remove semester tab
                    mtcOptions.TabPages.Remove(tpSemester);

                    //exit
                    return;
                }
                else if (semesters[0].Result == (int)SelectResult.FatalError)
                {
                    //database error while getting semesters
                    //display message
                    MetroMessageBox.Show(Manager.MainForm, string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Semester, semesters[0].ErrorMessage),
                        Properties.Resources.titleWebServiceError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //write error
                    Manager.Log.WriteError(string.Format(
                        Properties.Resources.errorWebServiceListItem,
                        Properties.Resources.item_Semester,
                        semesters[0].ErrorMessage));

                    //could not get semesters
                    //clear list
                    semesters.Clear();

                    //remove semester tab
                    mtcOptions.TabPages.Remove(tpSemester);

                    //exit
                    return;
                }

                //display semesters
                mcbSemester.ValueMember = "SemesterId";
                mcbSemester.DisplayMember = "Description";
                mcbSemester.DataSource = semesters;

                //check current semester
                if (Manager.CurrentSemester.SemesterId > 0)
                {
                    //select current semester
                    mcbSemester.SelectedValue = Manager.CurrentSemester.SemesterId;
                }
            }
            catch (Exception ex)
            {
                //database error while getting semesters
                //write error
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem,
                    Properties.Resources.item_Semester), ex);

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
        /// Load settings from settings manager.
        /// </summary>
        private void LoadSettings()
        {
            //select restart culture on list of items
            foreach (NamedCulture language in mcbLanguages.Items)
            {
                if (language.culture.DisplayName ==
                    Manager.Settings.RestartCulture.DisplayName)
                {
                    //select current language
                    mcbLanguages.SelectedItem = language;

                    break;
                }
            }

            //load grid data
            //set grid font size
            mcbGridFontSize.SelectedValue = Manager.Settings.GridFontSize;

            //load column settings
            LoadColumnSettings();

            //load server configs
            //set server IP
            mtxtServerIP.Text = Manager.HardSettings.ServerIP;

            //set server port
            nudServerPort.Value = (decimal)Manager.HardSettings.ServerPort;
        }

        /// <summary>
        /// Load column related settings from settings manager.
        /// </summary>
        private void LoadColumnSettings()
        {
            //load class grid columns
            LoadColumns(
                Manager.Settings.ClassGridColumns,
                Manager.Settings.ClassGridDisplayedColumns,
                classAvailableColumns, classSelectedColumns);

            //load event grid columns
            LoadColumns(
                Manager.Settings.EventGridColumns,
                Manager.Settings.EventGridDisplayedColumns,
                eventAvailableColumns, eventSelectedColumns);

            //load institution grid columns
            LoadColumns(
                Manager.Settings.InstitutionGridColumns,
                Manager.Settings.InstitutionGridDisplayedColumns,
                institutionAvailableColumns, institutionSelectedColumns);

            //load instrument grid columns
            LoadColumns(
                Manager.Settings.InstrumentGridColumns,
                Manager.Settings.InstrumentGridDisplayedColumns,
                instrumentAvailableColumns, instrumentSelectedColumns);

            //load pole grid columns
            LoadColumns(
                Manager.Settings.PoleGridColumns,
                Manager.Settings.PoleGridDisplayedColumns,
                poleAvailableColumns, poleSelectedColumns);

            //load registration grid columns
            LoadColumns(
                Manager.Settings.RegistrationGridColumns,
                Manager.Settings.RegistrationGridDisplayedColumns,
                registrationAvailableColumns, registrationSelectedColumns);

            //load report grid columns
            LoadColumns(
                Manager.Settings.ReportGridColumns,
                Manager.Settings.ReportGridDisplayedColumns,
                reportAvailableColumns, reportSelectedColumns);

            //load student grid columns
            LoadColumns(
                Manager.Settings.StudentGridColumns,
                Manager.Settings.StudentGridDisplayedColumns,
                studentAvailableColumns, studentSelectedColumns);

            //load teacher grid columns
            LoadColumns(
                Manager.Settings.TeacherGridColumns,
                Manager.Settings.TeacherGridDisplayedColumns,
                teacherAvailableColumns, teacherSelectedColumns);

            //load user grid columns
            LoadColumns(
                Manager.Settings.UserGridColumns,
                Manager.Settings.UserGridDisplayedColumns,
                userAvailableColumns, userSelectedColumns);

            //create list of column grids
            mcbGrid.Items.Add(Properties.Resources.item_plural_Class);
            mcbGrid.Items.Add(Properties.Resources.item_plural_Event);
            mcbGrid.Items.Add(Properties.Resources.item_plural_Institution);
            mcbGrid.Items.Add(Properties.Resources.item_plural_Instrument);
            mcbGrid.Items.Add(Properties.Resources.item_plural_Pole);
            mcbGrid.Items.Add(Properties.Resources.item_plural_Registration);
            mcbGrid.Items.Add(Properties.Resources.item_plural_Report);
            mcbGrid.Items.Add(Properties.Resources.item_plural_Student);
            mcbGrid.Items.Add(Properties.Resources.item_plural_Teacher);
            mcbGrid.Items.Add(Properties.Resources.item_plural_User);

            //select firt column grid option
            mcbGrid.SelectedIndex = 0;
        }

        /// <summary>
        /// Load columns into lists.
        /// </summary>
        /// <param name="gridColumns">
        /// The list of all grid column names.
        /// </param>
        /// <param name="gridDisplayedColumns">
        /// The list of displayed column names.
        /// </param>
        /// <param name="availableColumns">
        /// The resulting list of available columns.
        /// </param>
        /// <param name="selectedColumns">
        /// The resulting list of selected columns.
        /// </param>
        private void LoadColumns(
            string gridColumns, string gridDisplayedColumns,
            BindingList<NamedColumn> availableColumns,
            BindingList<NamedColumn> selectedColumns)
        {
            //check all columns
            foreach (string column in gridColumns.Split(','))
            {
                //check value
                if (column == null || column.Length == 0)
                {
                    //go to next column
                    continue;
                }

                //add column to available list
                availableColumns.Add(new NamedColumn(column));
            }

            //check all selected columns
            foreach (string selectedColumn in gridDisplayedColumns.Split(','))
            {
                //check value
                if (selectedColumn == null || selectedColumn.Length == 0)
                {
                    //go to next column
                    continue;
                }

                //add column to selected list
                selectedColumns.Add(new NamedColumn(selectedColumn));

                //remove column from available list
                for (int i = availableColumns.Count - 1; i >= 0; i--)
                {
                    //compare columns
                    if (availableColumns[i].ColumnValue.Equals(selectedColumn))
                    {
                        //remove column
                        availableColumns.RemoveAt(i);

                        //exit loop
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Save semesters back to settings manager.
        /// </summary>
        private void SaveSemesters()
        {
            //get song channel
            ISongService songChannel = Manager.WebServiceManager.GetSongChannel();

            //check result
            if (songChannel == null)
            {
                //channel is not available at the moment
                //could not save semesters
                //exit
                return;
            }

            try
            {
                //set cursor to wait
                this.Cursor = Cursors.WaitCursor;

                //check each loaded semester
                foreach (Semester semester in semesters)
                {
                    //check if semester was not updated
                    if (!semester.Updated)
                    {
                        //go to next semester
                        continue;
                    }

                    //update semester and get result
                    SaveResult saveResult = songChannel.UpdateSemester(semester);

                    //check result
                    if (saveResult.Result == (int)SelectResult.FatalError)
                    {
                        //semester was not saved
                        //display message
                        MetroFramework.MetroMessageBox.Show(Manager.MainForm, string.Format(
                            Properties.Resources.errorWebServiceSaveItem,
                            Properties.Resources.item_Semester, saveResult.ErrorMessage),
                            Properties.Resources.titleWebServiceError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //log error
                        Manager.Log.WriteError(string.Format(
                            Properties.Resources.errorWebServiceSaveItem,
                            Properties.Resources.item_Semester, saveResult.ErrorMessage));

                        //could not save semester
                        //exit
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //database error while saving item
                //show error message
                MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelSaveItem, 
                    Properties.Resources.item_Semester, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log exception
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelSaveItem, 
                    Properties.Resources.item_Semester, ex.Message));
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

                //reset cursor
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Save settings back to settings manager.
        /// </summary>
        private void SaveSettings()
        {
            //set restart culture
            Manager.Settings.RestartCulture =
                ((NamedCulture)mcbLanguages.SelectedItem).culture;

            //check if restart culture is different from current culture
            if (Manager.Settings.RestartCulture.DisplayName !=
                Manager.Settings.Culture.DisplayName)
            {
                //show message to user
                MetroMessageBox.Show(this,
                    Properties.Resources.msgRestartCulture,
                    Properties.Resources.msgCultureChanged,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //save grid settings
            //check selected grid font size
            if (mcbGridFontSize.SelectedIndex > -1)
            {
                //set grid font size
                Manager.Settings.GridFontSize = (decimal)mcbGridFontSize.SelectedValue;
            }

            //save columns
            SaveColumnSettings();

            //check if server options have been edited
            if (Manager.HardSettings.ServerIP != mtxtServerIP.Text ||
                Manager.HardSettings.ServerPort != (int)nudServerPort.Value)
            {
                //user has edited server options
                hasEditedServer = true;
            }

            //save server options
            //set server IP
            Manager.HardSettings.ServerIP = mtxtServerIP.Text;

            //set server port
            Manager.HardSettings.ServerPort = (int)nudServerPort.Value;

            //save hard settings and check result
            if (!Manager.HardSettings.Save())
            {
                //could not save settings
                //display message
                MetroMessageBox.Show(this,
                    Properties.Resources.errorSaveSettings,
                    Properties.Resources.wordError,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //exit
                return;
            }

            //save settings
            Manager.Settings.Save();
        }

        /// <summary>
        /// Save column related settings to settings manager.
        /// </summary>
        private void SaveColumnSettings()
        {
            //save class displayed columns
            Manager.Settings.ClassGridDisplayedColumns =
                GatherColumnValues(classSelectedColumns);

            //save event displayed columns
            Manager.Settings.EventGridDisplayedColumns =
                GatherColumnValues(eventSelectedColumns);

            //save institution displayed columns
            Manager.Settings.InstitutionGridDisplayedColumns =
                GatherColumnValues(institutionSelectedColumns);

            //save instrument displayed columns
            Manager.Settings.InstrumentGridDisplayedColumns =
                GatherColumnValues(instrumentSelectedColumns);

            //save pole displayed columns
            Manager.Settings.PoleGridDisplayedColumns =
                GatherColumnValues(poleSelectedColumns);

            //save registration displayed columns
            Manager.Settings.RegistrationGridDisplayedColumns =
                GatherColumnValues(registrationSelectedColumns);

            //save report displayed columns
            Manager.Settings.ReportGridDisplayedColumns =
                GatherColumnValues(reportSelectedColumns);

            //save student displayed columns
            Manager.Settings.StudentGridDisplayedColumns =
                GatherColumnValues(studentSelectedColumns);

            //save teacher displayed columns
            Manager.Settings.TeacherGridDisplayedColumns =
                GatherColumnValues(teacherSelectedColumns);

            //save user displayed columns
            Manager.Settings.UserGridDisplayedColumns =
                GatherColumnValues(userSelectedColumns);
        }

        /// <summary>
        /// Gather selected column values.
        /// </summary>
        /// <param name="orderSelectedColumns">
        /// The list of selected columns.
        /// </param>
        /// <returns>
        /// The selected column values text.
        /// </returns>
        private string GatherColumnValues(BindingList<NamedColumn> selectedColumns)
        {
            //create string builder to hold the result
            StringBuilder displayedColumns = new StringBuilder(16 * selectedColumns.Count);

            //check each selected column
            foreach (NamedColumn selectedColumn in selectedColumns)
            {
                //add column value
                displayedColumns.Append(selectedColumn.ColumnValue);
                displayedColumns.Append(",");
            }

            //if there is any displayed colummn
            if (displayedColumns.Length > 0)
            {
                //remove last comma
                displayedColumns.Length -= 1;
            }

            //return result
            return displayedColumns.ToString();
        }

        /// <summary>
        /// Validate input setting options.
        /// </summary>
        /// <returns>True if all settings are OK.</returns>
        private bool ValidateSettings()
        {
            try
            {
                //check if selected IP is actually an IP
                if (mtxtServerIP.Text.Length > 0 && char.IsDigit(mtxtServerIP.Text[0]))
                {
                    //validate server IP
                    System.Net.IPAddress.Parse(mtxtServerIP.Text);
                }
            }
            catch
            {
                //invalid IP address
                //show server tab
                mtcOptions.SelectedTab = tpServer;

                //set focus
                mtxtServerIP.Focus();                

                //show message
                MetroMessageBox.Show(this,
                    Properties.Resources.msgIPFormat,
                    Properties.Resources.titleInvalidIP,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                //select value
                mtxtServerIP.SelectAll();

                //return negative result
                return false;
            }

            //fields are valid
            return true;
        }

        #endregion Private Methods


        #region Event Handlers ********************************************************

        /// <summary>
        /// Form load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OptionsForm_Load(object sender, EventArgs e)
        {
            //display first tab always
            mtcOptions.SelectedIndex = 0;

            //set font to UI controls
            nudServerPort.Font = MetroFramework.MetroFonts.DefaultLight(14.0F);
            gbExamples.Font = MetroFramework.MetroFonts.DefaultLight(14.0F);
            gbColumns.Font = MetroFramework.MetroFonts.DefaultLight(14.0F);
            gbSemester.Font = MetroFramework.MetroFonts.DefaultLight(14.0F);
            mtxtSemesterStartDate.Font = MetroFramework.MetroFonts.Default(13.0F);
            mtxtSemesterEndDate.Font = MetroFramework.MetroFonts.Default(13.0F);

            //set port number limits
            nudServerPort.Maximum = IPEndPoint.MaxPort;
            nudServerPort.Minimum = IPEndPoint.MinPort;

            //list font sizes
            List<KeyValuePair<string, decimal>> sizes = new List<KeyValuePair<string, decimal>> ();
            sizes.Add(new KeyValuePair<string, decimal>("18 pts", 18));
            sizes.Add(new KeyValuePair<string, decimal>("14 pts", 14));
            sizes.Add(new KeyValuePair<string, decimal>("12 pts", 12));
            mcbGridFontSize.DisplayMember = "Key";
            mcbGridFontSize.ValueMember = "Value";
            mcbGridFontSize.DataSource = sizes;

            //set accepted cultures
            ListAcceptedCultures();

            //load data into form
            LoadSettings();

            //check if there is a grid to be displayed 
            if (displayGridOptions != null && displayGridOptions.Length > 0)
            {
                //display grid options
                SelectedGrid = displayGridOptions;

                //focus grid combo
                mcbGrid.Focus();

                //remove server options
                mtcOptions.TabPages.Remove(tpServer);
            }

            //check if user has semester edit permission
            if (Manager.HasLogonPermission("Semester.Edit"))
            {
                //load semester data into form
                LoadSemesters();
            }
            else
            {
                //set mepty list of semesters
                semesters = new List<Semester>();

                //remove semester tab
                mtcOptions.TabPages.Remove(tpSemester);
            }
        }

        /// <summary>
        /// OK button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnOK_Click(object sender, EventArgs e)
        {
            //validate fields
            if (this.ValidateSettings())
            {
                //fields are valid
                //save settings
                SaveSettings();

                //check if user has semester edit permission
                if (Manager.HasLogonPermission("Semester.Edit"))
                {
                    //save semesters
                    SaveSemesters();
                }

                //close dialog and return OK as dialog result
                DialogResult = DialogResult.OK;
            }

        }

        /// <summary>
        /// Language selected index changed evnet handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            //save current thread culture and change it
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture =
                ((NamedCulture)mcbLanguages.SelectedItem).culture;

            //write a number
            double dblExample = 1550300.00;
            mtxtNumber.Text = dblExample.ToString("N");

            //write date
            mtxtDate.Text = DateTime.Now.ToShortDateString();

            //write hour
            mtxtHour.Text = DateTime.Now.ToShortTimeString();

            //reset ti previous culture
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        /// <summary>
        /// Grid combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check selected index
            if (SelectedGrid.Equals(string.Empty))
            {
                //reset current selected columns list
                selectedColumns = null;

                //reset current available columns list
                availableColumns = null;
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Class))
            {
                //set class selected columns list as the selected columns list
                selectedColumns = classSelectedColumns;

                //set class available columns list as the available columns list
                availableColumns = classAvailableColumns;
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Event))
            {
                //set event selected columns list as the selected columns list
                selectedColumns = eventSelectedColumns;

                //set event available columns list as the available columns list
                availableColumns = eventAvailableColumns;
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Institution))
            {
                //set institution selected columns list as the selected columns list
                selectedColumns = institutionSelectedColumns;

                //set institution available columns list as the available columns list
                availableColumns = institutionAvailableColumns;
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Instrument))
            {
                //set instrument selected columns list as the selected columns list
                selectedColumns = instrumentSelectedColumns;

                //set instrument available columns list as the available columns list
                availableColumns = instrumentAvailableColumns;
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Pole))
            {
                //set pole selected columns list as the selected columns list
                selectedColumns = poleSelectedColumns;

                //set pole available columns list as the available columns list
                availableColumns = poleAvailableColumns;
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Registration))
            {
                //set registration selected columns list as the selected columns list
                selectedColumns = registrationSelectedColumns;

                //set registration available columns list as the available columns list
                availableColumns = registrationAvailableColumns;
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Report))
            {
                //set report selected columns list as the selected columns list
                selectedColumns = reportSelectedColumns;

                //set report available columns list as the available columns list
                availableColumns = reportAvailableColumns;
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Student))
            {
                //set student selected columns list as the selected columns list
                selectedColumns = studentSelectedColumns;

                //set student available columns list as the available columns list
                availableColumns = studentAvailableColumns;
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Teacher))
            {
                //set teacher selected columns list as the selected columns list
                selectedColumns = teacherSelectedColumns;

                //set teacher available columns list as the available columns list
                availableColumns = teacherAvailableColumns;
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_User))
            {
                //set user selected columns list as the selected columns list
                selectedColumns = userSelectedColumns;

                //set user available columns list as the available columns list
                availableColumns = userAvailableColumns;
            }

            //set current selected columns to selected columns list box
            lsSelectedColumns.DataSource = selectedColumns;

            //set current available columns to available columns list box
            lsAvailableColumns.DataSource = availableColumns;

            //clear selection
            lsSelectedColumns.ClearSelected();
            lsAvailableColumns.ClearSelected();
        }

        /// <summary>
        /// Add column button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnAddColumn_Click(object sender, EventArgs e)
        {
            //check number of selected available columns
            if (lsAvailableColumns.SelectedItems.Count == 0)
            {
                //should never happen
                //exit
                return;
            }

            //keep track of removed columns
            List<NamedColumn> removeColumns = new List<NamedColumn>();

            //add column to list of selected columns
            foreach (NamedColumn o in lsAvailableColumns.SelectedItems)
            {
                //add column
                selectedColumns.Add(o);

                //add to removed column list
                removeColumns.Add(o);
            }

            //check each removed column
            foreach (NamedColumn column in removeColumns)
            {
                //remove it from the available column list
                availableColumns.Remove(column);
            }

            //clear selected available columns
            lsAvailableColumns.ClearSelected();

            //select newly added selected columns
            lsSelectedColumns.ClearSelected();
            foreach (NamedColumn column in removeColumns)
            {
                lsSelectedColumns.SelectedItems.Add(column);
            }
        }

        /// <summary>
        /// Remove column button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnRemoveColumn_Click(object sender, EventArgs e)
        {
            //check number of selected selected columns
            if (lsSelectedColumns.SelectedItems.Count == 0)
            {
                //should never happen
                //exit
                return;
            }

            //keep track of removed columns
            List<NamedColumn> removeColumns = new List<NamedColumn>();

            //add column to list of available columns
            foreach (NamedColumn o in lsSelectedColumns.SelectedItems)
            {
                //add column
                availableColumns.Add(o);

                //add to removed column list
                removeColumns.Add(o);
            }

            //check each removed column
            foreach (NamedColumn column in removeColumns)
            {
                //remove it from the selected column list
                selectedColumns.Remove(column);
            }

            //clear selected selected columns
            lsSelectedColumns.ClearSelected();

            //select newly added available columns
            lsAvailableColumns.ClearSelected();
            foreach (NamedColumn column in removeColumns)
            {
                lsAvailableColumns.SelectedItems.Add(column);
            }
        }

        /// <summary>
        /// Decrease column position button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnDecreaseColumn_Click(object sender, EventArgs e)
        {
            //check if only one column is selected
            if (lsSelectedColumns.SelectedIndices.Count != 1)
            {
                //exit
                return;
            }

            //get selected index
            int selectedIndex = lsSelectedColumns.SelectedIndex;

            //check if selected item is not the first one
            if (selectedIndex == 0)
            {
                //exit
                return;
            }

            //get selected item
            NamedColumn item = selectedColumns[selectedIndex];

            //reinsert item
            selectedColumns.Insert(selectedIndex - 1, item);

            //remove item
            selectedColumns.RemoveAt(selectedIndex + 1);

            //select item
            lsSelectedColumns.ClearSelected();
            lsSelectedColumns.SelectedIndex = selectedIndex - 1;
        }

        /// <summary>
        /// Increase column position button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnIncreaseColumn_Click(object sender, EventArgs e)
        {
            //check if only one column is selected
            if (lsSelectedColumns.SelectedIndices.Count != 1)
            {
                //exit
                return;
            }

            //get selected index
            int selectedIndex = lsSelectedColumns.SelectedIndex;

            //check if selected item is not the last one
            if (selectedIndex == lsSelectedColumns.Items.Count - 1)
            {
                //exit
                return;
            }

            //get selected item
            NamedColumn item = selectedColumns[selectedIndex];

            //remove item 
            selectedColumns.RemoveAt(selectedIndex);

            //reinsert item
            selectedColumns.Insert(selectedIndex + 1, item);

            //select item
            lsSelectedColumns.ClearSelected();
            lsSelectedColumns.SelectedIndex = selectedIndex + 1;
        }

        /// <summary>
        /// Reset columns button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnResetColumns_Click(object sender, EventArgs e)
        {
            //reset current selected columns list
            selectedColumns = null;

            //reset current available columns list
            availableColumns = null;

            //check selected grid element
            if (SelectedGrid.Equals(string.Empty))
            {
                //should never happen
                //exit
                return;
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Class))
            {
                //clear class columns
                classAvailableColumns.Clear();
                classSelectedColumns.Clear();

                //load default class grid columns
                LoadColumns(
                    Manager.Settings.ClassGridColumns,
                    Manager.Settings.ClassGridDefaultColumns,
                    classAvailableColumns, classSelectedColumns);
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Event))
            {
                //clear event columns
                eventAvailableColumns.Clear();
                eventSelectedColumns.Clear();

                //load default event grid columns
                LoadColumns(
                    Manager.Settings.EventGridColumns,
                    Manager.Settings.EventGridDefaultColumns,
                    eventAvailableColumns, eventSelectedColumns);
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Institution))
            {
                //clear institution columns
                institutionAvailableColumns.Clear();
                institutionSelectedColumns.Clear();

                //load default institution grid columns
                LoadColumns(
                    Manager.Settings.InstitutionGridColumns,
                    Manager.Settings.InstitutionGridDefaultColumns,
                    institutionAvailableColumns, institutionSelectedColumns);
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Instrument))
            {
                //clear instrument columns
                instrumentAvailableColumns.Clear();
                instrumentSelectedColumns.Clear();

                //load default instrument grid columns
                LoadColumns(
                    Manager.Settings.InstrumentGridColumns,
                    Manager.Settings.InstrumentGridDefaultColumns,
                    instrumentAvailableColumns, instrumentSelectedColumns);
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Pole))
            {
                //clear pole columns
                poleAvailableColumns.Clear();
                poleSelectedColumns.Clear();

                //load default pole grid columns
                LoadColumns(
                    Manager.Settings.PoleGridColumns,
                    Manager.Settings.PoleGridDefaultColumns,
                    poleAvailableColumns, poleSelectedColumns);
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Registration))
            {
                //clear registration columns
                registrationAvailableColumns.Clear();
                registrationSelectedColumns.Clear();

                //load default registration grid columns
                LoadColumns(
                    Manager.Settings.RegistrationGridColumns,
                    Manager.Settings.RegistrationGridDefaultColumns,
                    registrationAvailableColumns, registrationSelectedColumns);
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Report))
            {
                //clear report columns
                reportAvailableColumns.Clear();
                reportSelectedColumns.Clear();

                //load default report grid columns
                LoadColumns(
                    Manager.Settings.ReportGridColumns,
                    Manager.Settings.ReportGridDefaultColumns,
                    reportAvailableColumns, reportSelectedColumns);
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Student))
            {
                //clear student columns
                studentAvailableColumns.Clear();
                studentSelectedColumns.Clear();

                //load default student grid columns
                LoadColumns(
                    Manager.Settings.StudentGridColumns,
                    Manager.Settings.StudentGridDefaultColumns,
                    studentAvailableColumns, studentSelectedColumns);
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_Teacher))
            {
                //clear teacher columns
                teacherAvailableColumns.Clear();
                teacherSelectedColumns.Clear();

                //load default teacher grid columns
                LoadColumns(
                    Manager.Settings.TeacherGridColumns,
                    Manager.Settings.TeacherGridDefaultColumns,
                    teacherAvailableColumns, teacherSelectedColumns);
            }
            else if (SelectedGrid.Equals(Properties.Resources.item_plural_User))
            {
                //clear user columns
                userAvailableColumns.Clear();
                userSelectedColumns.Clear();

                //load default user grid columns
                LoadColumns(
                    Manager.Settings.UserGridColumns,
                    Manager.Settings.UserGridDefaultColumns,
                    userAvailableColumns, userSelectedColumns);
            }

            //set loaded lists
            //simulate a grid combox selected index changed event
            mcbGrid_SelectedIndexChanged(this, new EventArgs());
        }

        /// <summary>
        /// Available columns list selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsAvailableColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            //enable add button if there is one or more selected items
            mbtnAddColumn.Enabled = (lsAvailableColumns.SelectedIndex >= 0);
            mbtnAddColumn.BackgroundImage = mbtnAddColumn.Enabled ?
                Properties.Resources.IconMoveRightOne :
                Properties.Resources.IconMoveRightOneDisabled;
        }

        /// <summary>
        /// Selected columns list selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsSelectedColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            //enable remove button if there is one or more selected items
            mbtnRemoveColumn.Enabled = (lsSelectedColumns.SelectedIndex >= 0);
            mbtnRemoveColumn.BackgroundImage = mbtnRemoveColumn.Enabled ?
                Properties.Resources.IconMoveLeftOne :
                Properties.Resources.IconMoveLeftOneDisabled;

            //enable increase column and decrease column buttons if there is
            //one and only one item selected
            mbtnDecreaseColumn.Enabled =
                (lsSelectedColumns.SelectedIndices.Count == 1);
            mbtnDecreaseColumn.BackgroundImage = mbtnDecreaseColumn.Enabled ?
                Properties.Resources.IconMoveUpOne :
                Properties.Resources.IconMoveUpOneDisabled;
            mbtnIncreaseColumn.Enabled = mbtnDecreaseColumn.Enabled;
            mbtnIncreaseColumn.BackgroundImage = mbtnIncreaseColumn.Enabled ?
                Properties.Resources.IconMoveDownOne :
                Properties.Resources.IconMoveDownOneDisabled;
        }

        /// <summary>
        /// Semester combo box selected index changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check selected semester
            if (mcbSemester.SelectedIndex >= 0)
            {
                //get selected semester and display it
                selectedSemester = semesters.Find(
                    s => s.SemesterId == (int)mcbSemester.SelectedValue);

                //load data
                mtxtSemesterStartDate.Text = selectedSemester.StartDate.ToShortDateString();
                mtxtSemesterEndDate.Text = selectedSemester.EndDate.ToShortDateString();

                //check current semester
                if (Manager.CurrentSemester.SemesterId > 0)
                {
                    //compare selected semester with current semester
                    if (selectedSemester.SemesterId < Manager.CurrentSemester.SemesterId)
                    {
                        //it is a past semester
                        //disable fields
                        mtxtSemesterStartDate.Enabled = false;
                        mtxtSemesterEndDate.Enabled = false;
                    }
                    else if (selectedSemester.SemesterId > Manager.CurrentSemester.SemesterId)
                    {
                        //it is a future semester
                        //enable fields
                        mtxtSemesterStartDate.Enabled = true;
                        mtxtSemesterEndDate.Enabled = true;
                    }
                    else
                    {
                        //it is current semester
                        //enable fields accordingly
                        mtxtSemesterStartDate.Enabled = selectedSemester.StartDate > DateTime.Today;
                        mtxtSemesterEndDate.Enabled = selectedSemester.EndDate > DateTime.Today;
                    }
                }
                else
                {
                    //semester was not loaded
                    //disable fields
                    mtxtSemesterStartDate.Enabled = false;
                    mtxtSemesterEndDate.Enabled = false;
                }
            }
            else
            {
                //no selected semester
                selectedSemester = null;

                //clear data
                mtxtSemesterStartDate.Text = string.Empty;
                mtxtSemesterEndDate.Text = string.Empty;

                //disable fields
                mtxtSemesterStartDate.Enabled = false;
                mtxtSemesterEndDate.Enabled = false;
            }
        }

        /// <summary>
        /// Date masked text box click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Date_Click(object sender, EventArgs e)
        {
            //get sender textbox
            MaskedTextBox mtxtDate = (MaskedTextBox)sender;

            //check text
            if (mtxtDate.Text.Equals("  /  /"))
            {
                //set cursor position
                mtxtDate.Select(0, 0);
            }
        }

        /// <summary>
        /// Semester start date textbox key up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtSemesterStartDate_KeyUp(object sender, KeyEventArgs e)
        {
            //check selected semester
            if (selectedSemester == null)
            {
                //should never happen
                //exit
                return;
            }

            //check entered text size
            if (mtxtSemesterStartDate.Text.Length != 10)
            {
                //cannot be a valid date
                //exit
                return;
            }

            //parse entered date
            DateTime date;
            if (!DateTime.TryParse(mtxtSemesterStartDate.Text, out date))
            {
                //date is invalid
                //reset start date to its original

                //set start date
                mtxtSemesterStartDate.Text = selectedSemester.StartDate.ToShortDateString();

                //exit
                return;
            }

            //compare entered date with reference date
            if (date < selectedSemester.ReferenceDate)
            {
                //start date is sooner than reference date
                //set date to reference date
                date = selectedSemester.ReferenceDate;

                //set start date
                mtxtSemesterStartDate.Text = date.ToShortDateString();
            }
            else if (date >= selectedSemester.ReferenceDate.AddMonths(3))
            {
                //start date is later than reference date plus 3 months
                //set date to reference date plus 3 months minus 1 day
                date = selectedSemester.ReferenceDate.AddMonths(3).AddDays(-1);

                //set start date
                mtxtSemesterStartDate.Text = date.ToShortDateString();
            }

            //compare entered date with end date
            if (date >= selectedSemester.EndDate)
            {
                //start date is higher than end date
                //set date to maximum value
                date = selectedSemester.EndDate.AddDays(-1);

                //set start date
                mtxtSemesterStartDate.Text = date.ToShortDateString();
            }

            //compare entered date with today
            if (date <= DateTime.Today)
            {
                //start date is sooner than today
                //set date to tomorrow
                date = DateTime.Today.AddDays(1);

                //set start date
                mtxtSemesterStartDate.Text = date.ToShortDateString();
            } 

            //date is valid
            //check if date is the same
            if (date == selectedSemester.StartDate)
            {
                //same date
                //exit
                return;
            }

            //update selected semester start date
            selectedSemester.StartDate = date;
            selectedSemester.Updated = true;
        }

        /// <summary>
        /// Semester end date textbox key up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtxtSemesterEndDate_KeyUp(object sender, KeyEventArgs e)
        {
            //check selected semester
            if (selectedSemester == null)
            {
                //should never happen
                //exit
                return;
            }

            //check entered text size
            if (mtxtSemesterEndDate.Text.Length != 10)
            {
                //cannot be a valid date
                //exit
                return;
            }

            //parse entered date
            DateTime date;
            if (!DateTime.TryParse(mtxtSemesterEndDate.Text, out date))
            {
                //date is invalid
                //reset end date to its original

                //set end date
                mtxtSemesterEndDate.Text = selectedSemester.EndDate.ToShortDateString();

                //exit
                return;
            }

            //compare entered date with reference date plus 4 months
            if (date < selectedSemester.ReferenceDate.AddMonths(4))
            {
                //end date is sooner than reference date plus 4 months
                //set date to reference date plus 4 months
                date = selectedSemester.ReferenceDate.AddMonths(4);

                //set end date
                mtxtSemesterEndDate.Text = date.ToShortDateString();
            }
            else if (date >= selectedSemester.ReferenceDate.AddMonths(7))
            {
                //end date is later than reference date plus 7 months
                //set date to reference date plus 7 months minus 1 day
                date = selectedSemester.ReferenceDate.AddMonths(7).AddDays(-1);

                //set end date
                mtxtSemesterEndDate.Text = date.ToShortDateString();
            }

            //compare entered date with start date
            if (date <= selectedSemester.StartDate)
            {
                //end date is lower than start date
                //set date to minimum value
                date = selectedSemester.StartDate.AddDays(1);

                //set end date
                mtxtSemesterEndDate.Text = date.ToShortDateString();
            }

            //compare entered date with today
            if (date <= DateTime.Today)
            {
                //end date is sooner than today
                //set date to tomorrow
                date = DateTime.Today.AddDays(1);

                //set end date
                mtxtSemesterEndDate.Text = date.ToShortDateString();
            }

            //date is valid
            //check if date is the same
            if (date == selectedSemester.EndDate)
            {
                //same date
                //exit
                return;
            }

            //update selected semester end date
            selectedSemester.EndDate = date;
            selectedSemester.Updated = true;
        }

        #endregion Event Handlers

    } //end of class OptionsForm

    /// <summary>
    /// This class set a customized name for a given culture.
    /// Works as a structure.
    /// </summary>
    class NamedCulture
    {
        public string name;
        public CultureInfo culture;

        /// <summary>
        /// Return its name.
        /// </summary>
        /// <returns>The name of this language.</returns>
        public override string ToString()
        {
            return name;
        }

    } //end of class NamedCulture


    /// <summary>
    /// This class set a customized name for a given column.
    /// Works as a structure.
    /// </summary>
    class NamedColumn
    {
        private string columnName = "";
        private string columnValue = "";

        /// <summary>
        /// Create a column with a value.
        /// </summary>
        /// <param name="columnValue"></param>
        public NamedColumn(string columnValue)
        {
            //set columnValue
            this.columnValue = columnValue;

            //get name
            string name = Properties.Resources.ResourceManager.GetString(
                "grid" + columnValue);

            //set columnName
            if (name == null || name.Length == 0)
            {
                //translation not found
                this.columnName = columnValue;
            }
            else
            {
                //set translated name
                this.columnName = name;
            }
        }

        /// <summary>
        /// Return its name.
        /// </summary>
        /// <returns>The name of this column.</returns>
        public override string ToString()
        {
            return columnName;
        }

        /// <summary>
        /// Get column name.
        /// </summary>
        public string ColumnName
        {
            get { return columnName; }
        }

        /// <summary>
        /// Get column value.
        /// </summary>
        public string ColumnValue
        {
            get { return columnValue; }
        }

    } //end of class NamedColumn

} //end of namespace PnT.SongClient.UI
