using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetroFramework;
using MetroFramework.Controls;

using PnT.SongDB.Logic;
using PnT.SongServer;
using PnT.SongClient.Logic;

namespace PnT.SongClient.UI.Controls
{

    #region Delegates *****************************************************************

    /// <summary>
    /// Delegate to call a void method with no parameter.
    /// </summary>
    public delegate void NoObjectDelegate();

    #endregion Delegates


    #region Enums *********************************************************************

    /// <summary>
    /// Base register status. Indicates the contorl status.
    /// </summary>
    public enum RegisterStatus
    {
        Consulting,
        Editing,
        Creating
    };

    #endregion Enums


    /// <summary>
    /// Base class for creating data register controls.
    /// </summary>
    public partial class RegisterBaseControl : UserControl, ISongControl
    {

        #region Fields ****************************************************************

        /// <summary>
        /// The parent control that opened this control. 
        /// </summary>
        protected ISongControl parentControl = null;

        /// <summary>
        /// Last selected item ID.
        /// </summary>
        protected int selectedId = -1;

        /// <summary>
        /// The type name of the displayed items.
        /// </summary>
        protected string itemTypeName = "Item";

        /// <summary>
        /// The item displayable name.
        /// </summary>
        protected string itemTypeDescription = "Item";

        /// <summary>
        /// The loaded items to be displayed.
        /// </summary>
        private List<IdDescriptionStatus> loadedItems = null;

        /// <summary>
        /// The displayed items in the list box.
        /// </summary>
        private List<IdDescriptionStatus> displayedItems;

        /// <summary>
        /// Current control status.
        /// </summary>
        private RegisterStatus status = RegisterStatus.Consulting;

        /// <summary>
        /// The last selected item id from the listbox.
        /// </summary>
        private int lastSelectedId = int.MinValue;

        /// <summary>
        /// Flag that indicates whether the checkbox for 
        /// hiding not active items will be displayed.
        /// </summary>
        protected bool classHasStatus = true;

        /// <summary>
        /// Flag that indicates if an item can deleted 
        /// or if it should be inactivated instead.
        /// </summary>
        protected bool classHasDeletion = true;

        /// <summary>
        /// Flag that indicates whether inactive items be hidden from listbox.
        /// </summary>
        protected bool hideInactiveItems = true;

        /// <summary>
        /// Flag that indicates if inactive items should be displayed even when
        /// inactive items are to be hidden.
        /// </summary>
        protected bool overrideHideInactiveItems = false;

        /// <summary>
        /// Flag that indicates whether the new button will be displayed.
        /// </summary>
        protected bool displayNewButton = true;

        /// <summary>
        /// Flag that indicates whether the copy button will be displayed.
        /// </summary>
        protected bool displayCopyButton = false;

        /// <summary>
        /// Option to allow user to add a new item.
        /// True if user has permission to add new items.
        /// </summary>
        protected bool allowAddItem = true;

        /// <summary>
        /// Option to allow user to edit an item.
        /// True if user has permission to edit items.
        /// </summary>
        protected bool allowEditItem = true;

        /// <summary>
        /// Option to allow user to delete an item.
        /// True if user has permission to delete items.
        /// </summary>
        protected bool allowDeleteItem = true;

        /// <summary>
        /// The ID of the item to be displayed when control is displayed for the first time.
        /// Item is selected only once.
        /// </summary>
        private int firstSelectedId = int.MinValue;

        /// <summary>
        /// Indicates if this control is being initialized.
        /// </summary>
        private bool isInitializing = false;

        /// <summary>
        /// Option to skip next data load.
        /// </summary>
        protected bool skipDataLoad = false;

        /// <summary>
        /// The name of the UI culture
        /// </summary>
        private string uiCultureName = "";

        /// <summary>
        /// True if constructor was already performed.
        /// </summary>
        private bool isConstructed = false;

        /// <summary>
        /// The list of items found in the last search.
        /// </summary>
        private List<IdDescriptionStatus> searchedItems = null;

        /// <summary>
        /// The index of the last selected item found in the last search.
        /// </summary>
        private int selectedSearchedItemIndex = int.MinValue;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RegisterBaseControl() : this("Item", false)
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="itemTypeName">The name of the item type.</param>
        /// <param name="hideInactiveItems">True to hide inactive items by default.</param>
        public RegisterBaseControl(string itemTypeName, bool hideInactiveItems)
        {
            //set item type name
            this.itemTypeName = itemTypeName;

            //set to hide inactive items option
            this.hideInactiveItems = hideInactiveItems;

            //get item type description
            this.itemTypeDescription = Properties.Resources.ResourceManager.GetString(
                "item_" + itemTypeName);

            //get UI culture
            uiCultureName = System.Globalization.CultureInfo.CurrentUICulture.Name.Length > 0 ?
                System.Globalization.CultureInfo.CurrentUICulture.Name : "international";

            //init UI components
            InitializeComponent();

            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //create list of searched items
            searchedItems = new List<IdDescriptionStatus>();

            //load item descriptions
            LoadItems();

            //constructor is done
            isConstructed = true;
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get/set option to allow user to add a new item.
        /// True if user has permission to add new items.
        /// </summary>
        protected bool AllowAddItem
        {
            get
            {
                return allowAddItem;
            }

            set
            {
                allowAddItem = value;
            }
        }

        /// <summary>
        /// Get/set option to allow user to edit an item.
        /// True if user has permission to edit items.
        /// </summary>
        protected bool AllowEditItem
        {
            get
            {
                return allowEditItem;
            }

            set
            {
                allowEditItem = value;
            }
        }

        /// <summary>
        /// Get/set option to allow user to delete an item.
        /// True if user has permission to delete items.
        /// </summary>
        protected bool AllowDeleteItem
        {
            get
            {
                return allowDeleteItem;
            }

            set
            {
                allowDeleteItem = value;
            }
        }

        /// <summary>
        /// Get/set the ID of the item to be displayed when control is displayed 
        /// for the first time. Item is selected only once.
        /// </summary>
        public int FirstSelectedId
        {
            get
            {
                return firstSelectedId;
            }

            set
            {
                firstSelectedId = value;
            }
        }

        /// <summary>
        /// Get/set the type name of the displayed items.
        /// </summary>
        public string ItemName
        {
            get
            {
                return itemTypeName;
            }

            set
            {
                itemTypeName = value;
            }
        }

        /// <summary>
        /// Get list of loaded items.
        /// The returned list is a copy. No need to lock it.
        /// </summary>
        public List<IdDescriptionStatus> ListLoadedItems
        {
            get
            {
                //copy and return loaded items
                return new List<IdDescriptionStatus>(loadedItems);
            }
        }

        /// <summary>
        /// Get/set the id of the selected item in the displayed list.
        /// </summary>
        public int SelectedItemId
        {
            get
            {
                //check selected item
                if (this.lsItems.SelectedValue != null)
                {
                    //return the ID of the selected item
                    return (int)this.lsItems.SelectedValue;
                }
                else
                {
                    //no item is selected.
                    return int.MinValue;
                }
            }
            set
            {
                //check if form is still on memory
                if (!this.Created || this.Disposing || this.IsDisposed)
                {
                    //not on memory
                    return;
                }

                try
                {
                    //get selected id
                    this.lsItems.SelectedValue = value;
                }
                catch
                {
                    //do nothing
                }
            }
        }

        /// <summary>
        /// Get/set the current control status.
        /// </summary>
        protected RegisterStatus Status
        {
            get { return status; }
            set
            {
                //check if status has changed
                if (status != value)
                {
                    //set status
                    status = value;

                    //refresh buttons
                    RefreshButtons();
                }
            }
        }

        /// <summary>
        /// Get/set the parent control that opened this control.
        /// </summary>
        public ISongControl ParentControl
        {
            get
            {
                return parentControl;
            }

            set
            {
                parentControl = value;
            }
        }

        #endregion Properties


        #region Virtual Methods *******************************************************

        /// <summary>
        /// Dispose used resources from user control.
        /// </summary>
        public virtual void DisposeControl()
        {
        }

        /// <summary>
        /// Dispose child Song control.
        /// </summary>
        public virtual void DisposeChildControl()
        {
        }

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        /// <returns></returns>
        public virtual string SelectMenuOption()
        {
            //select no option
            return "";
        }

        /// <summary>
        /// Load data for selected item.
        /// </summary>
        /// <param name="itemId">the id of the selected item.</param>
        public virtual bool LoadItemData(int itemId)
        {
            //sub class must load data
            return false;
        }

        /// <summary>
        /// Start creating a new item from scratch.
        /// </summary>
        public virtual void CreateItem()
        {
        }

        /// <summary>
        /// Start editing current selected item.
        /// </summary>
        public virtual void EditItem()
        {
        }

        /// <summary>
        /// Save the data of the current edited item.
        /// </summary>
        /// <returns>
        /// The updated description of the saved item.
        /// Null if item could not be saved.
        /// </returns>
        public virtual IdDescriptionStatus SaveItem()
        {
            return null;
        }

        /// <summary>
        /// Delete current selected item.
        /// </summary>
        public virtual bool DeleteItem()
        {
            return true;
        }

        /// <summary>
        /// Copy current selected item.
        /// </summary>
        /// <returns>
        /// The description of the copied item.
        /// Null if item could not be copied.
        /// </returns>
        public virtual IdDescriptionStatus CopyItem()
        {
            return null;
        }

        /// <summary>
        /// Cancel changes from current edited item.
        /// </summary>
        public virtual void CancelChanges()
        {
        }

        /// <summary>
        /// Clear value for all UI fields.
        /// </summary>
        public virtual void ClearFields()
        {
        }

        /// <summary>
        /// Enable all UI fields for edition.
        /// </summary>
        /// <param name="enable">True to enable fields. False to disable them.</param>
        public virtual void EnableFields(bool enable)
        {
        }

        #endregion Virtual Methods


        #region Cache Methods *********************************************************

        /// <summary>
        /// Compare selected list to displayed items.
        /// </summary>
        /// <param name="list">
        /// The selected list.
        /// </param>
        /// <returns>
        /// True if the lists are the same with the same objects.
        /// False otherwise.
        /// </returns>
        protected bool CompareToDisplayedItems(List<IdDescriptionStatus> list)
        {
            //check list
            if (this.displayedItems == null || list == null)
            {
                //cannot compare lists
                return false;
            }

            //compare sizes
            if (this.displayedItems.Count != list.Count)
            {
                //cannot be the same list
                return false;
            }

            //compare each item
            for (int i = 0; i < list.Count; i++)
            {
                //compare item
                if (!list[i].Equals(this.displayedItems[i]))
                {
                    //not the same item
                    return false;
                }
            }

            //lists are the same
            return true;
        }

        /// <summary>
        /// Load item description list from disk or database.
        /// </summary>
        /// <returns>
        /// The list of item descriptions.
        /// </returns>
        private void LoadItems()
        {
            //get item descriptions from disk
            List<IdDescriptionStatus> items = LoadItemsFromDisk();

            //check result
            if (items != null)
            {
                //list was loaded from disk
                loadedItems = items;

                //exit
                return;
            }

            try
            {
                //get item descriptions from database
                loadedItems = LoadItemsFromDatabase();            
            }
            catch (Exception ex)
            {
                //database error while getting institutions
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorChannelListItem, itemTypeDescription), ex);

                //reset loaded items
                loadedItems = null;
            }

            //check result
            if (loadedItems != null)
            {
                //save list to disk
                SaveItemsToDisk(loadedItems);
            }
            else
            {
                //set empty list
                loadedItems = new List<IdDescriptionStatus>();
            }
        }

        /// <summary>
        /// Load item list from database.
        /// </summary>
        /// <returns>
        /// The list of registered items.
        /// </returns>
        protected virtual List<IdDescriptionStatus> LoadItemsFromDatabase()
        {
            //return list of items
            return null;
        }

        /// <summary>
        /// Load item list from disk cache.
        /// </summary>
        /// <returns>
        /// The list of registered items.
        /// Null if no list could be loaded.
        /// </returns>
        private List<IdDescriptionStatus> LoadItemsFromDisk()
        {
            //check if list of items is cached
            if (!System.IO.File.Exists(Manager.CACHE_DIR_PATH + "/" + itemTypeName + "Descriptions." + uiCultureName + ".bin"))
            {
                //list not available
                return null;
            }

            //read file
            FileStream stream = null;

            try
            {
                //open file for reading
                stream = new FileStream(
                    Manager.CACHE_DIR_PATH + "/" + itemTypeName + "Descriptions." + uiCultureName + ".bin", FileMode.Open);

                //create list of item descriptions
                List<IdDescriptionStatus> items = new List<IdDescriptionStatus>();

                //create binary reader
                BinaryReader reader = new BinaryReader(stream);

                //get number of descriptions
                int count = reader.ReadInt32();

                //read each item description
                for (int i = 0; i < count; i++)
                {
                    //create item description
                    IdDescriptionStatus item = new IdDescriptionStatus();

                    //set data
                    item.Id = reader.ReadInt32();
                    item.Status = reader.ReadInt32();
                    item.Description = reader.ReadString();

                    //add item descripton to result
                    items.Add(item);
                }

                //return result
                return items;
            }
            catch
            {
                //error while loading list
                return null;
            }
            finally
            {
                //check stream
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Save item list to disk cache.
        /// </summary>
        /// <param name="items">
        /// The item list to be saved.
        /// </param>
        private void SaveItemsToDisk(List<IdDescriptionStatus> items)
        {
            //check list of items
            if (items == null)
            {
                //exit
                return;
            }

            //save file to disk
            FileStream stream = null;

            try
            {
                //check if directory exists
                DirectoryInfo cacheDir = new DirectoryInfo(Manager.CACHE_DIR_PATH);

                //check if directory exists
                if (!cacheDir.Exists)
                {
                    try
                    {
                        //create directory
                        cacheDir.Create();
                    }
                    catch
                    {
                        //do nothing
                        //exit
                        return;
                    }
                }

                //save list to disk
                stream = new FileStream(
                    Manager.CACHE_DIR_PATH + "/" + itemTypeName + "Descriptions." + uiCultureName + ".bin", FileMode.Create);

                //use binary writer
                BinaryWriter writer = new BinaryWriter(stream);

                //write number of item decriptions
                writer.Write(items.Count);

                //write each item description
                foreach (IdDescriptionStatus item in items)
                {
                    //write data
                    writer.Write(item.Id);
                    writer.Write(item.Status);
                    writer.Write(item.Description);
                }

                //flush data
                writer.Flush();
            }
            catch (Exception ex)
            {
                //log exception
                Manager.Log.WriteException(
                    "Unexpected exception while saving registered " + itemTypeName + "s to cache file.", ex);
            }
            finally
            {
                //check stream
                if (stream != null)
                {
                    //close stream
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Reload cache from database.
        /// </summary>
        /// <param name="stateInfo"></param>
        private void ReloadDataCache(object stateInfo)
        {
            try
            {
                //wait a second before beginning
                Thread.Sleep(2000);

                //set culture to thread
                System.Threading.Thread.CurrentThread.CurrentCulture = Manager.Settings.Culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = Manager.Settings.Culture;

                //the reloaded item list
                List<IdDescriptionStatus> reloadedItems = null;
                try
                {
                    //reload list of items from database
                    reloadedItems = LoadItemsFromDatabase();

                }
                catch (Exception ex)
                {
                    //database error while getting institutions
                    Manager.Log.WriteException(string.Format(
                        Properties.Resources.errorChannelListItem, itemTypeDescription), ex);

                    //reset reloaded items
                    reloadedItems = null;
                }

                //check result
                if (reloadedItems == null)
                {
                    //exit
                    return;
                }

                //compare sizes
                if (reloadedItems.Count == loadedItems.Count)
                {
                    //check if data were updated
                    bool updated = false;

                    //check each item
                    for (int i = 0; i < reloadedItems.Count; i++)
                    {
                        //get reloaded item and its description
                        IdDescriptionStatus reloadedItem = reloadedItems[i];
                        IdDescriptionStatus loadedItem = loadedItems[i];

                        //compare id, status and description
                        if (reloadedItem.Id != loadedItem.Id ||
                            reloadedItem.Status != loadedItem.Status ||
                            !reloadedItem.Description.Equals(loadedItem.Description))
                        {
                            //data were updated
                            updated = true;

                            //exit loop
                            break;
                        }
                    }

                    //check result
                    if (!updated)
                    {
                        //data were not updated
                        //remove reference to list
                        reloadedItems = null;
                    }
                }

                //check if reloaded items should be saved to disk
                if (reloadedItems != null)
                {
                    //save items to disk
                    SaveItemsToDisk(reloadedItems);

                    //set reloaded items as the current loaded items
                    loadedItems = reloadedItems;

                    //display result on UI thread
                    this.Invoke(
                        new NoObjectDelegate(DisplayReloadedItemDescriptions),
                        new object[] { });
                }
            }
            catch (Exception ex)
            {
                //unexpected error while reloading order filter cache
                Manager.Log.WriteException(
                    "Unexpected error while reloading item description cache from database.", ex);
            }
        }

        /// <summary>
        /// Display reloaded item descriptions.
        /// </summary>
        private void DisplayReloadedItemDescriptions()
        {
            try
            {
                //check if item is not just consulting items
                if (this.Status != RegisterStatus.Consulting)
                {
                    //cannot update displayed item descriptions now
                    //exit
                    return;
                }

                //refresh displayed item list
                RefreshDisplayedItemList();
            }
            catch (Exception ex)
            {
                //unexpected error while displaying order filter cache
                Manager.Log.WriteException(
                    "Unexpected error while applyint item description cache to UI.", ex);
            }
        }

        /// <summary>
        /// Remove cached item description.
        /// </summary>
        /// <param name="itemId">
        /// The ID of the selected item.
        /// </param>
        private void RemoveDisplayedItem(int itemId)
        {
            //try to find loaded item
            IdDescriptionStatus loadedItem =
                loadedItems.Find(a => a.Id == itemId);

            //check result
            if (loadedItem == null)
            {
                //no need to delete item
                //exit
                return;
            }

            //remove item
            loadedItems.Remove(loadedItem);

            //save descriptions to file
            SaveItemsToDisk(loadedItems);
        }

        /// <summary>
        /// Update cached item description.
        /// </summary>
        /// <param name="item">The updatem item.</param>
        /// <returns>
        /// True if item description was updated.
        /// False otherwise.
        /// </returns>
        private bool UpdateDisplayedItem(IdDescriptionStatus item)
        {
            //try to find loaded item
            IdDescriptionStatus loadedItem =
                loadedItems.Find(a => a.Id == item.Id);

            //check result
            if (loadedItem == null)
            {
                //add item to the list
                loadedItems.Add(item);
            }
            else
            {
                //check if status and description are still the same
                if (loadedItem.Status == item.Status &&
                    loadedItem.Description.Equals(item.Description))
                {
                    //no need to update item
                    return false;
                }

                //update status and description
                loadedItem.Status = item.Status;
                loadedItem.Description = item.Description;
            }
            
            //save descriptions to file
            SaveItemsToDisk(loadedItems);

            //description was updated
            return true;
        }

        /// <summary>
        /// Update cached item description.
        /// </summary>
        /// <param name="itemId">
        /// The ID of the selected item.
        /// </param>
        /// <param name="status">
        /// The new status value.
        /// </param>
        /// <returns>
        /// True if item description was updated.
        /// False otherwise.
        /// </returns>
        private bool UpdateDisplayedItem(int itemId, int status)
        {
            //try to find loaded item
            IdDescriptionStatus loadedItem =
                loadedItems.Find(a => a.Id == itemId);

            //check result
            if (loadedItem == null)
            {
                //no need to update item
                return false;
            }
            else
            {
                //check if status is still the same
                if (loadedItem.Status == status)
                {
                    //no need to update description
                    return false;
                }

                //update status
                loadedItem.Status = status;
            }

            //save descriptions to file
            SaveItemsToDisk(loadedItems);

            //description was updated
            return true;
        }

        #endregion Cache Private Methods


        #region UI Event handlers *****************************************************

        /// <summary>
        /// Control loaded event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterBaseControl_Load(object sender, EventArgs e)
        {
            //check if control is in design mode
            if (this.DesignMode)
            {
                //exit
                return;
            }

            //check if constructor is already done
            if (!isConstructed)
            {
                //should never happen
                //metro library must be fixed
                throw new ApplicationException(
                    "Load event handler called before constructor is done at RegisterBaseControl.");
            }

            //set auto validate
            this.AutoValidate = AutoValidate.Disable;

            //set initializing flag
            isInitializing = true;

            //set hide not active checkbox
            ckHideNotActive.Checked = hideInactiveItems;

            //set font to UI controls
            lsItems.Font = MetroFramework.MetroFonts.DefaultLight(12.0F);

            //set control item heading
            mlblItemHeading.Text = itemTypeDescription;

            //reset loading flag
            isInitializing = false;

            //check if displayed class does not have a status field
            if (classHasStatus == false)
            {
                //cannot hide inactive items
                hideInactiveItems = false;

                //does not display inactive status checkbox
                //suspend layout
                this.tlpLeft.SuspendLayout();

                //remove checkbox
                tlpLeft.Controls.Remove(ckHideNotActive);

                //enlarge list
                tlpLeft.SetRowSpan(lsItems, 2);

                //resume layout
                this.tlpLeft.ResumeLayout(false);

                //set increase list bottom margin
                this.lsItems.Margin = new Padding(
                    this.lsItems.Margin.Left, this.lsItems.Margin.Top, 
                    this.lsItems.Margin.Right, this.lsItems.Margin.Bottom + 2);
            }

            //check if displayed class does cannot be deleted
            if (classHasDeletion == false)
            {
                //change delete button text
                btDelete.Text = Properties.Resources.buttonInactivate;
            }

            //check if new button should not be displayed
            if (!displayNewButton)
            {
                //suspend layout
                this.tlpBottom.SuspendLayout();

                //get column of the new button
                int column = tlpBottom.GetColumn(btNew);

                //remove comlun style
                tlpBottom.ColumnStyles.RemoveAt(column);

                //remove button
                tlpBottom.Controls.Remove(btNew);

                foreach (Control control in tlpBottom.Controls)
                {
                    //get control column
                    int controlColumn = tlpBottom.GetColumn(control);

                    //check if control is displayed after new button
                    if (controlColumn > column)
                    {
                        //move control one column to the left
                        tlpBottom.SetColumn(control, controlColumn - 1);
                    }
                }

                //decrement column count
                tlpBottom.ColumnCount -= 1;

                //resume layout
                this.tlpBottom.ResumeLayout(false);
            }

            //check if copy button should not be displayed
            if (!displayCopyButton)
            {
                //suspend layout
                this.tlpBottom.SuspendLayout();

                //get column of the copy button
                int column = tlpBottom.GetColumn(btCopy);

                //remove comlun style
                tlpBottom.ColumnStyles.RemoveAt(column);

                //remove button
                tlpBottom.Controls.Remove(btCopy);

                foreach (Control control in tlpBottom.Controls)
                {
                    //get control column
                    int controlColumn = tlpBottom.GetColumn(control);

                    //check if control is displayed after copy button
                    if (controlColumn > column)
                    {
                        //move control one column to the left
                        tlpBottom.SetColumn(control, controlColumn - 1);
                    }
                }

                //decrement column count
                tlpBottom.ColumnCount -= 1;

                //resume layout
                this.tlpBottom.ResumeLayout(false);
            }

            //check if this control has a parent control to return to
            if (parentControl == null)
            {
                //hide return tile
                mtlReturn.Visible = false;
            }

            //set status to consulting
            Status = RegisterStatus.Consulting;

            //refresh displayed item list
            RefreshDisplayedItemList();

            //disable fields
            EnableFields(false);
            
            //refresh buttons
            RefreshButtons();

            //check if there is an item to be selected
            if (firstSelectedId > 0)
            {
                //select first id
                lsItems.SelectedValue = firstSelectedId;

                //check if item was not selected
                //and if option to hide inactive items is checked
                if (lsItems.SelectedIndex < 0 && ckHideNotActive.Checked)
                {
                    //uncheck hide inactive items
                    ckHideNotActive.Checked = false;

                    //select first id again
                    lsItems.SelectedValue = firstSelectedId;
                }

                //check if item was selected
                if (lsItems.SelectedIndex >= 0)
                {
                    //display item at the top
                    lsItems.TopIndex = Math.Max(lsItems.SelectedIndex - 10, 0);
                }

                //reset first selected id
                firstSelectedId = int.MinValue;
            }

            //reload items from database using another thread
            ThreadPool.QueueUserWorkItem(ReloadDataCache);
        }

        /// <summary>
        /// Handles event for typed string on search text box 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txSearchString_KeyUp(object sender, KeyEventArgs e)
        {
            //check if text has length zero
            if (txSearchString.Text.Length == 0)
            {
                //unselect any user
                lsItems.SelectedItems.Clear();

                //exit method
                return;
            }

            //update the search string typed in the search textbox
            string searchString = txSearchString.Text;

            //Checks if there are items to filter
            if (lsItems.Items.Count > 0)
            {
                //search the list
                IdDescriptionStatus listBoxItem = FindListItemByWords(searchString);

                //check result
                if (listBoxItem != null)
                {
                    //Check if found item changed
                    if (lsItems.SelectedValue == null ||
                        (int)lsItems.SelectedValue != listBoxItem.Id)
                    {
                        //Selects different item found in the listbox.
                        lsItems.SelectedValue = listBoxItem.Id;
                        //change button state
                        Status = RegisterStatus.Consulting;
                        //disable fields
                        EnableFields(false);
                    }
                }
                else
                {
                    //no item was found
                    //clear selection
                    lsItems.SelectedItems.Clear();
                }
            }
        }

        /// <summary>
        /// Next search result button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnNextSearchResult_Click(object sender, EventArgs e)
        {
            //check searched items
            if (searchedItems == null || searchedItems.Count == 0)
            {
                //should never happen
                //disable button
                mbtnNextSearchResult.Enabled = false;

                //exit
                return;
            }

            //check selected searched item index
            if (selectedSearchedItemIndex < 0 || 
                selectedSearchedItemIndex >= searchedItems.Count)
            {
                //index is out of range
                //should never happen
                //disable button
                mbtnNextSearchResult.Enabled = false;

                //exit
                return;
            }

            //check if list has one item only
            if (searchedItems.Count == 1)
            {
                //exit
                return;
            }

            //check if there is any selected item
            if (this.lsItems.SelectedValue != null)
            {
                //check if selected searched item is the current selected item
                if (searchedItems[selectedSearchedItemIndex].Id ==
                    (int)this.lsItems.SelectedValue)
                {
                    //it is
                    //increment index
                    selectedSearchedItemIndex++;

                    //check if list has ended
                    if (selectedSearchedItemIndex == searchedItems.Count)
                    {
                        //go to start again
                        selectedSearchedItemIndex = 0;
                    }
                }
            }

            //select next serached item
            this.lsItems.SelectedValue = searchedItems[selectedSearchedItemIndex].Id;
        }

        /// <summary>
        /// Handles a selected index change on object list box. Loads the database data for the 
        /// selected object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if data load should be skipped
            if (skipDataLoad)
            {
                //skip data load
                //reset flag
                skipDataLoad = false;

                //exit
                return;
            }

            //check status if status is not consulting            
            if (Status != RegisterStatus.Consulting)
            {
                //user is editing register
                //check if th
                if (lsItems.SelectedIndex == -1 ||
                    lastSelectedId == lsItems.SelectedIndex)
                {
                    //exit
                    return;
                }
                else
                {
                    //user must confirm operation
                    //show message box
                    DialogResult dr = MetroMessageBox.Show(Manager.MainForm, string.Format(
                        PnT.SongClient.Properties.Resources.msgEditingItem, itemTypeDescription),
                        PnT.SongClient.Properties.Resources.titleCancelEdition,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2);

                    //check result
                    if (dr == DialogResult.No)
                    {
                        //let user continue editing current register
                        lsItems.SelectedIndex = lastSelectedId;

                        //exit
                        return;
                    }
                    else if (dr == DialogResult.Yes)
                    {
                        //cancel changes by clicking at cancel button
                        btCancel_Click(this, null);
                    }
                }
            }

            //change lastSelectedItem
            lastSelectedId = lsItems.SelectedIndex;

            //check if there is a selected object
            if (lsItems.SelectedValue != null)
            {
                //reload the data for the selected object
                PerformDataLoad((int)lsItems.SelectedValue);
            }
            else
            {
                //clear data
                ClearFields();
            }

            //refresh buttons
            RefreshButtons();
        }

        /// <summary>
        /// Handles a click on the new button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNew_Click(object sender, EventArgs e)
        {
            //set status
            Status = RegisterStatus.Creating;

            //clear fields
            ClearFields();

            //enable fields
            EnableFields(true);

            //de-select listbox item
            lsItems.SelectedItems.Clear();

            //clear flags
            selectedId = -1;

            //inform new register
            CreateItem();
        }

        /// <summary>
        /// Handles a click on the Edit button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEdit_Click(object sender, EventArgs e)
        {
            //change status to editing
            Status = RegisterStatus.Editing;

            //enable fields
            EnableFields(true);

            //edire register
            EditItem();
        }

        /// <summary>
        /// Handles a click on the Copy button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCopy_Click(object sender, EventArgs e)
        {
            //check if there is a selected item
            if (lsItems.SelectedIndex < 0)
            {
                //no item is selected
                //should never happen
                //disable copy button
                RefreshButtons();

                //exit
                return;
            }

            //confirm that user wants to copy register
            DialogResult dr = MetroMessageBox.Show(Manager.MainForm, string.Format(
                PnT.SongClient.Properties.Resources.msgCopyItem, this.itemTypeDescription),
                PnT.SongClient.Properties.Resources.cptConfirm,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            //check result
            if (dr == DialogResult.Cancel)
            {
                //user canceled copy operation
                //exit
                return;
            }

            try
            {
                //set cursor to wait
                this.Cursor = Cursors.WaitCursor;

                //copy and get copied item
                IdDescriptionStatus copiedItem = CopyItem();
                
                //check copied item
                if (copiedItem != null)
                {
                    //change status to consulting
                    Status = RegisterStatus.Consulting;

                    //disable fields
                    EnableFields(false);

                    //check if item was updated and item displayed list must be refreshed
                    if (UpdateDisplayedItem(copiedItem))
                    {
                        //refresh displayed item list
                        RefreshDisplayedItemList();
                    }

                    //check if there are displayed items
                    if (displayedItems != null)
                    {
                        //copied item must be selected in the list
                        //check if displayed items contains updated item
                        if (displayedItems.Find(x => x.Id == copiedItem.Id) != null)
                        {
                            //skip one data load
                            //data will be loaded in the next step bellow
                            skipDataLoad = true;

                            //reselect item without loading data
                            SelectedItemId = copiedItem.Id;

                            //reset flag
                            skipDataLoad = false;
                        }
                    }

                    //reload item data from web service
                    //avoid data diference between displayed and saved
                    //check if there is a selected object
                    if (lsItems.SelectedValue != null)
                    {
                        //reload the data for the selected object
                        PerformDataLoad((int)lsItems.SelectedValue);
                    }

                    //refresh buttons
                    RefreshButtons();

                    //set focus to list of items
                    lsItems.Focus();

                    //display message
                    Manager.MainForm.ShowStatusMessage(string.Format(
                        Properties.Resources.msgItemCopied, itemTypeDescription), 5000);
                }
            }
            catch (Exception ex)
            {
                //database error while saving item
                //show error message
                MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelCopyItem, itemTypeDescription, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log exception
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelCopyItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);
            }
            finally
            {
                //reset cursor
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Handles a click on the Save button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                //set cursor to wait
                this.Cursor = Cursors.WaitCursor;

                //save register and get updated item
                IdDescriptionStatus updatedItem = SaveItem();

                //check updated item
                if (updatedItem != null)
                {
                    //change status to consulting
                    Status = RegisterStatus.Consulting;

                    //disable fields
                    EnableFields(false);

                    //check if item was updated and item displayed list must be refreshed
                    if (UpdateDisplayedItem(updatedItem))
                    {
                        //refresh displayed item list
                        RefreshDisplayedItemList();
                    }

                    //check if there is no selected selected item
                    //even though updated item is displayed
                    if (lsItems.SelectedIndex < 0 && displayedItems != null)
                    {
                        //new items must be selected in the list
                        //check if displayed items contains updated item
                        if (displayedItems.Find(x => x.Id == updatedItem.Id) != null)
                        {
                            //skip one data load
                            //data will be loaded in the next step bellow
                            skipDataLoad = true;

                            //reselect item without loading data
                            SelectedItemId = updatedItem.Id;

                            //reset flag
                            skipDataLoad = false;
                        }
                    }

                    //reload item data from web service
                    //avoid data diference between displayed and saved
                    //check if there is a selected object
                    if (lsItems.SelectedValue != null)
                    {
                        //reload the data for the selected object
                        PerformDataLoad((int)lsItems.SelectedValue);
                    }
                    else
                    {
                        //selected item is not being displayed anymore
                        //might happen if user click to hide inative items
                        //while editing an inactive item
                        //just clear fields
                        ClearFields();
                    }

                    //refresh buttons
                    RefreshButtons();

                    //set focus to list of items
                    lsItems.Focus();

                    //display message
                    Manager.MainForm.ShowStatusMessage(string.Format(
                        Properties.Resources.msgItemSaved, itemTypeDescription), 5000);
                }
            }
            catch (Exception ex)
            {
                //database error while saving item
                //show error message
                MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelSaveItem, itemTypeDescription, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log exception
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelSaveItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);
            }
            finally
            {
                //reset cursor
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Handles a click on the Delete button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDelete_Click(object sender, EventArgs e)
        {
            //check if there is a selected item
            if (SelectedItemId <= 0)
            {
                //no item is selected
                //exit
                return;
            }

            //user must confirm operation
            DialogResult result;

            //check if class has deletion
            if (classHasDeletion)
            {
                //confirm deletion operation
                result = MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.msgDeleteRegister, this.itemTypeDescription),
                    Properties.Resources.cptConfirm,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            else
            {
                //confirm inactivation operation
                result = MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.msgInactivateRegister, this.itemTypeDescription),
                    Properties.Resources.cptConfirm,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

            //check result
            if (result == DialogResult.Cancel)
            {
                //user canceled operation
                //exit
                return;
            }
            
            try
            {
                //set cursor to wait
                this.Cursor = Cursors.WaitCursor;

                //delete register and check result
                if (DeleteItem())
                {
                    //set state to consulting
                    Status = RegisterStatus.Consulting;

                    //disable fields
                    EnableFields(false);

                    //check if class has deletion
                    if (classHasDeletion)
                    {
                        //remove displayed item
                        RemoveDisplayedItem(SelectedItemId);

                        //refresh displayed item list
                        RefreshDisplayedItemList();
                    }
                    else
                    {
                        //check if item was updated and displayed list must be refreshed
                        if (UpdateDisplayedItem(SelectedItemId, (int)ItemStatus.Inactive))
                        {
                            //refresh displayed item list
                            RefreshDisplayedItemList();
                        }
                    }

                    //reload item data from web service
                    //avoid data diference between displayed and saved
                    //check if there is a selected object
                    if (lsItems.SelectedValue != null)
                    {
                        //reload the data for the selected object
                        PerformDataLoad((int)lsItems.SelectedValue);
                    }

                    //refresh buttons
                    RefreshButtons();

                    //set focus to list of items
                    lsItems.Focus();
                }
            }
            catch (Exception ex)
            {
                //database error while deleting item
                //show error message
                MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelDeleteItem, itemTypeDescription, ex.Message),
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log exception
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelDeleteItem, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);
            }
            finally
            {
                //reset cursor
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Handles a click on the Cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCancel_Click(object sender, EventArgs e)
        {
            //cancel changes
            CancelChanges();

            //store current status
            RegisterStatus previousStatus = Status;

            //change status to consult
            Status = RegisterStatus.Consulting;

            //disable fields
            EnableFields(false);

            //check if user was editing or creating
            if (previousStatus == RegisterStatus.Editing)
            {
                //was editing
                if (lsItems.Items.Count > 0 && lsItems.SelectedValue != null)
                {
                    //reload the data for the selected object
                    PerformDataLoad((int)lsItems.SelectedValue);
                }
                else
                {
                    //selected item is not being displayed anymore
                    //might happen if user click to hide inative items
                    //while editing an inactive item
                    //just clear fields
                    ClearFields();
                }
            }
            else if (previousStatus == RegisterStatus.Creating)
            {
                //was creating
                //just clear fields
                ClearFields();
            }

            //set focus to list of items
            lsItems.Focus();

            //display message
            Manager.MainForm.ShowStatusMessage(
                Properties.Resources.msgEditionCanceled, 4000);
        }

        /// <summary>
        /// Items listbox draw item event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsItems_DrawItem(object sender, DrawItemEventArgs e)
        {
            //check if it is on design mode
            if (this.DesignMode == true)
            {
                //exit
                return;
            }

            //check index and number of items
            if (e.Index < 0 ||
                lsItems.Items.Count <= e.Index)
            {
                //exit
                return;
            }

            //get item
            IdDescriptionStatus item = (IdDescriptionStatus)lsItems.Items[e.Index];

            //select the font color
            Brush color = Brushes.Black;

            //check if item has status
            if (classHasStatus)
            {
                //check status
                if (item.Status == (int)ItemStatus.Inactive)
                {
                    //set font color to red
                    color = Brushes.Red;
                }
                else if (item.Status == (int)ItemStatus.Blocked ||
                    item.Status == (int)ItemStatus.Lost ||
                    item.Status == (int)ItemStatus.Evaded)
                {
                    //set font color to orange
                    color = Brushes.Orange;
                }
                else if (item.Status == (int)ItemStatus.Maintenance)
                {
                    //set font color to light green
                    color = Brushes.LightSeaGreen;
                }
            }

            //draw item
            e.DrawBackground();
            e.Graphics.DrawString(item.Description, e.Font, color, e.Bounds.Left, e.Bounds.Top);
            e.DrawFocusRectangle();
        }

        private void ckHideNotActive_CheckedChanged(object sender, EventArgs e)
        {
            //check if control is being initialized
            if (isInitializing)
            {
                //ignore event
                //exit
                return;
            }

            //set field
            hideInactiveItems = ckHideNotActive.Checked;

            //refresh displayed item list
            RefreshDisplayedItemList();
        }

        /// <summary>
        /// Return tile click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mtlReturn_Click(object sender, EventArgs e)
        {
            //check status if status is not consulting            
            if (Status != RegisterStatus.Consulting)
            {
                //user must confirm operation
                //show message box
                DialogResult dr = MetroMessageBox.Show(Manager.MainForm, string.Format(
                    PnT.SongClient.Properties.Resources.msgEditingItem, itemTypeDescription),
                    PnT.SongClient.Properties.Resources.titleCancelEdition,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                //check result
                if (dr == DialogResult.No)
                {
                    //user canceled operation
                    //exit
                    return;
                }
            }

            //ask parent control to dispose this child control
            parentControl.DisposeChildControl();
        }

        #endregion UI Event handlers


        #region Public Static Methods *************************************************
        
        /// <summary>
        /// Delete all item caches from disk.
        /// </summary>
        public static void DeleteAllItemsFromDisk()
        {
            try
            {
                //get cache directory
                System.IO.DirectoryInfo cacheDirectory = new DirectoryInfo(Manager.CACHE_DIR_PATH);

                //check each file in cache directory
                foreach (FileInfo file in cacheDirectory.GetFiles())
                {
                    //delete file
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                //log exception
                Manager.Log.WriteException(
                    "Unexpected exception while deleting all cache files.", ex);
            }
        }

        #endregion Public Static Methods


        #region Protected Methods *****************************************************

        /// <summary>
        /// List all Brazilian states.
        /// </summary>
        /// <returns>
        /// The list of Brazilian states to be displayed.
        /// </returns>
        protected List<KeyValuePair<string, string>> ListStates()
        {
            //create list
            List<KeyValuePair<string, string>> states = new List<KeyValuePair<string, string>>();

            //add states
            states.Add(new KeyValuePair<string, string>("Alagoas", "Alagoas"));
            states.Add(new KeyValuePair<string, string>("Amapá", "Amapá"));
            states.Add(new KeyValuePair<string, string>("Amazonas", "Amazonas"));
            states.Add(new KeyValuePair<string, string>("Bahia", "Bahia"));
            states.Add(new KeyValuePair<string, string>("Ceará", "Ceará"));
            states.Add(new KeyValuePair<string, string>("Distrito Federal", "Distrito Federal"));
            states.Add(new KeyValuePair<string, string>("Espírito Santo", "Espírito Santo"));
            states.Add(new KeyValuePair<string, string>("Goiás", "Goiás"));
            states.Add(new KeyValuePair<string, string>("Maranhão", "Maranhão"));
            states.Add(new KeyValuePair<string, string>("Mato Grosso", "Mato Grosso"));
            states.Add(new KeyValuePair<string, string>("Mato Grosso do Sul", "Mato Grosso do Sul"));
            states.Add(new KeyValuePair<string, string>("Minas Gerais", "Minas Gerais"));
            states.Add(new KeyValuePair<string, string>("Pará ", "Pará "));
            states.Add(new KeyValuePair<string, string>("Paraíba", "Paraíba"));
            states.Add(new KeyValuePair<string, string>("Paraná", "Paraná"));
            states.Add(new KeyValuePair<string, string>("Pernambuco", "Pernambuco"));
            states.Add(new KeyValuePair<string, string>("Piauí", "Piauí"));
            states.Add(new KeyValuePair<string, string>("Rio de Janeiro", "Rio de Janeiro"));
            states.Add(new KeyValuePair<string, string>("Rio Grande do Norte", "Rio Grande do Norte"));
            states.Add(new KeyValuePair<string, string>("Rio Grande do Sul", "Rio Grande do Sul"));
            states.Add(new KeyValuePair<string, string>("Rondônia", "Rondônia"));
            states.Add(new KeyValuePair<string, string>("Roraima", "Roraima"));
            states.Add(new KeyValuePair<string, string>("Santa Catarina", "Santa Catarina"));
            states.Add(new KeyValuePair<string, string>("São Paulo", "São Paulo"));
            states.Add(new KeyValuePair<string, string>("Sergipe", "Sergipe"));
            states.Add(new KeyValuePair<string, string>("Tocantins", "Tocantins"));

            //return created list
            return states;
        }

        /// <summary>
        /// Validate cnpj field.
        /// </summary>
        /// <param name="field">
        /// The cnpj field.
        /// </param>
        /// <param name="tabControl">
        /// The tab control that contains the tab page of the cnpj field.
        /// Null if cnpj field is not in a tab page.
        /// </param>
        /// <param name="tab">
        /// The tab page where the cnpj field is located.
        /// Null if cnpj field is not in a tab page.
        /// </param>
        /// <returns>
        /// True if cnpj field is valid.
        /// False if cnpj field is invalid.
        /// </returns>
        protected bool ValidateCnpjField(
            MaskedTextBox field, MetroTabControl tabControl, TabPage tab)
        {
            //check field
            if (field.Text.Length == 18)
            {
                //cnpj is valid
                return true;
            }

            //focus field
            field.Focus();

            //check tab
            if (tabControl != null && tab != null)
            {
                //display tab
                tabControl.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgInvalidFieldCnpj, field.Text),
                Properties.Resources.titleInvalidField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus field again
            field.Focus();

            //field is empty
            return false;
        }

        /// <summary>
        /// Validate cpf field.
        /// </summary>
        /// <param name="field">
        /// The cpf field.
        /// </param>
        /// <param name="tabControl">
        /// The tab control that contains the tab page of the cpf field.
        /// Null if cpf field is not in a tab page.
        /// </param>
        /// <param name="tab">
        /// The tab page where the cpf field is located.
        /// Null if cpf field is not in a tab page.
        /// </param>
        /// <returns>
        /// True if cpf field is valid.
        /// False if cpf field is invalid.
        /// </returns>
        protected bool ValidateCpfField(
            MaskedTextBox field, MetroTabControl tabControl, TabPage tab)
        {
            //check field
            if (field.Text.Length == 14)
            {
                //cpf is valid
                return true;
            }

            //focus field
            field.Focus();

            //check tab
            if (tabControl != null && tab != null)
            {
                //display tab
                tabControl.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgInvalidFieldCpf, field.Text),
                Properties.Resources.titleInvalidField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus field again
            field.Focus();

            //field is empty
            return false;
        }

        /// <summary>
        /// Validate date field.
        /// </summary>
        /// <param name="field">
        /// The date field.
        /// </param>
        /// <param name="tabControl">
        /// The tab control that contains the tab page of the date field.
        /// Null if date field is not in a tab page.
        /// </param>
        /// <param name="tab">
        /// The tab page where the date field is located.
        /// Null if date field is not in a tab page.
        /// </param>
        /// <returns>
        /// True if date field is valid.
        /// False if date field is invalid.
        /// </returns>
        protected bool ValidateDateField(
            MaskedTextBox field, MetroTabControl tabControl, TabPage tab)
        {
            //parse date
            DateTime time;
            if (DateTime.TryParse(field.Text, out time))
            {
                //date is valid
                return true;
            }

            //focus field
            field.Focus();

            //check tab
            if (tabControl != null && tab != null)
            {
                //display tab
                tabControl.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgInvalidFieldDate, field.Text),
                Properties.Resources.titleInvalidField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus field again
            field.Focus();

            //field is empty
            return false;
        }

        /// <summary>
        /// Validate description field against all listed items.
        /// </summary>
        /// <param name="field">
        /// The selected description field.
        /// </param>
        /// <param name="fieldName">
        /// The name of the description field.
        /// </param>
        /// <param name="tabControl">
        /// The tab control that contains the tab page of the field.
        /// Null if field is not in a tab page.
        /// </param>
        /// <param name="tab">
        /// The tab page where the field is located.
        /// Null if field is not in a tab page.
        /// </param>
        /// <returns>
        /// True if description is valid.
        /// False if description is repeated.
        /// </returns>
        protected bool ValidateDescriptionField(
            MetroTextBox field, string fieldName, MetroTabControl tabControl, TabPage tab)
        {
            //check loaded items
            if (loadedItems == null || loadedItems.Count == 0)
            {
                //there is no other description
                //description is valid
                return true;
            }

            //get and format description
            string description = FormatDescription(field.Text);

            //try to find another item if the same description
            IdDescriptionStatus sameItem = null;

            //check each loaded item
            foreach (IdDescriptionStatus item in loadedItems)
            {
                //check if item is selected
                if (item.Id == selectedId)
                {
                    //skip item
                    continue;
                }

                //format item description and compare
                if (FormatDescription(item.Description).Equals(description))
                {
                    //same description
                    sameItem = item;

                    //exit loop
                    break;
                }
            }

            //check result
            if (sameItem == null)
            {
                //description is valid
                return true;
            }

            //focus field
            field.Focus();

            //check tab
            if (tabControl != null && tab != null)
            {
                //display tab
                tabControl.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgInvalidFieldDescription, 
                itemTypeDescription, fieldName, sameItem.Description),
                Properties.Resources.titleInvalidField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus field again
            field.Focus();

            //field is empty
            return false;
        }

        /// <summary>
        /// Validate e-mail field.
        /// </summary>
        /// <param name="field">
        /// The e-mail field.
        /// </param>
        /// <param name="tabControl">
        /// The tab control that contains the tab page of the e-mail field.
        /// Null if e-mail field is not in a tab page.
        /// </param>
        /// <param name="tab">
        /// The tab page where the e-mail field is located.
        /// Null if e-mail field is not in a tab page.
        /// </param>
        /// <returns>
        /// True if e-mail field is valid.
        /// False if e-mail field is invalid.
        /// </returns>
        protected bool ValidateEmailField(
            MetroTextBox field, MetroTabControl tabControl, TabPage tab)
        {
            //check field
            if (field.Text.Length > 0 && (new EmailAddressAttribute()).IsValid(field.Text))
            {
                //email is valid
                return true;
            }

            //focus field
            field.Focus();

            //check tab
            if (tabControl != null && tab != null)
            {
                //display tab
                tabControl.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgInvalidFieldEmail, field.Text),
                Properties.Resources.titleInvalidField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus field again
            field.Focus();

            //field is empty
            return false;
        }

        /// <summary>
        /// Validate password fields by comparing them.
        /// </summary>
        /// <param name="field">
        /// The password field.
        /// </param>
        /// <param name="confirmField">
        /// The confirm password field.
        /// </param>
        /// <param name="tabControl">
        /// The tab control that contains the tab page of the phone field.
        /// Null if phone field is not in a tab page.
        /// </param>
        /// <param name="tab">
        /// The tab page where the phone field is located.
        /// Null if phone field is not in a tab page.
        /// </param>
        /// <returns>
        /// True if phone field is valid.
        /// False if phone field is invalid.
        /// </returns>
        protected bool ValidatePasswordField(
            MetroTextBox field, MetroTextBox confirmField, MetroTabControl tabControl, TabPage tab)
        {
            //compare fields
            if (field.Text.Equals(confirmField.Text))
            {
                //password is valid
                return true;
            }

            //focus confirm field
            confirmField.Focus();

            //check tab
            if (tabControl != null && tab != null)
            {
                //display tab
                tabControl.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm,
                Properties.Resources.msgInvalidFieldPassword,
                Properties.Resources.titleInvalidField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus confirm field again
            confirmField.Focus();

            //select confirm field text
            confirmField.SelectAll();

            //field is empty
            return false;
        }

        /// <summary>
        /// Validate phone field.
        /// </summary>
        /// <param name="field">
        /// The phone field.
        /// </param>
        /// <param name="tabControl">
        /// The tab control that contains the tab page of the phone field.
        /// Null if phone field is not in a tab page.
        /// </param>
        /// <param name="tab">
        /// The tab page where the phone field is located.
        /// Null if phone field is not in a tab page.
        /// </param>
        /// <returns>
        /// True if phone field is valid.
        /// False if phone field is invalid.
        /// </returns>
        protected bool ValidatePhoneField(
            MaskedTextBox field, MetroTabControl tabControl, TabPage tab)
        {
            //check field
            if (field.Text.Length == 14 || field.Text.Length == 15)
            {
                //phone is valid
                return true;
            }

            //focus field
            field.Focus();

            //check tab
            if (tabControl != null && tab != null)
            {
                //display tab
                tabControl.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgInvalidFieldPhone, field.Text),
                Properties.Resources.titleInvalidField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus field again
            field.Focus();

            //field is empty
            return false;
        }

        /// <summary>
        /// Validate required field.
        /// </summary>
        /// <param name="field">
        /// The required field.
        /// </param>
        /// <param name="fieldName">
        /// The name of the field.
        /// </param>
        /// <param name="tabControl">
        /// The tab control that contains the tab page of the field.
        /// Null if field is not in a tab page.
        /// </param>
        /// <param name="tab">
        /// The tab page where the field is located.
        /// Null if field is not in a tab page.
        /// </param>
        /// <returns>
        /// True if field is valid.
        /// False if field is empty.
        /// </returns>
        protected bool ValidateRequiredField(
            MetroTextBox field, string fieldName, MetroTabControl tabControl, TabPage tab)
        {
            //check field
            if (field.Text.Length > 0)
            {
                //field is set
                return true;
            }

            //focus field
            field.Focus();

            //check tab
            if (tabControl != null && tab != null)
            {
                //display tab
                tabControl.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgRequiredFieldName, fieldName),
                Properties.Resources.titleRequiredField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus field again
            field.Focus();

            //field is empty
            return false;
        }

        /// <summary>
        /// Validate required field.
        /// </summary>
        /// <param name="field">
        /// The required field.
        /// </param>
        /// <param name="fieldName">
        /// The name of the field.
        /// </param>
        /// <param name="tabControl">
        /// The tab control that contains the tab page of the field.
        /// Null if field is not in a tab page.
        /// </param>
        /// <param name="tab">
        /// The tab page where the field is located.
        /// Null if field is not in a tab page.
        /// </param>
        /// <returns>
        /// True if field is valid.
        /// False if field is empty.
        /// </returns>
        protected bool ValidateRequiredField(
            MaskedTextBox field, string fieldName, MetroTabControl tabControl, TabPage tab)
        {
            //get current mask format
            MaskFormat currentFormat = field.TextMaskFormat;

            try
            {
                //set format to exclude prompt and literals
                field.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;

                //check field
                if (field.Text.Length > 0)
                {
                    //field is set
                    return true;
                }
            }
            finally
            {
                //set mask format back
                field.TextMaskFormat = currentFormat;
            }

            //focus field
            field.Focus();

            //check tab
            if (tabControl != null && tab != null)
            {
                //display tab
                tabControl.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgRequiredFieldName, fieldName),
                Properties.Resources.titleRequiredField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus field again
            field.Focus();

            //field is empty
            return false;
        }

        /// <summary>
        /// Validate short time field.
        /// </summary>
        /// <param name="field">
        /// The time field.
        /// </param>
        /// <param name="tabControl">
        /// The tab control that contains the tab page of the time field.
        /// Null if time field is not in a tab page.
        /// </param>
        /// <param name="tab">
        /// The tab page where the time field is located.
        /// Null if time field is not in a tab page.
        /// </param>
        /// <returns>
        /// True if time field is valid.
        /// False if time field is invalid.
        /// </returns>
        protected bool ValidateShortTimeField(
            MaskedTextBox field, MetroTabControl tabControl, TabPage tab)
        {
            try
            {
                //parse time
                DateTime.ParseExact(field.Text + ":00", "H:m:s", null);

                //date is valid
                return true;
            }
            catch
            {
                //do nothing
            }

            //focus field
            field.Focus();

            //check tab
            if (tabControl != null && tab != null)
            {
                //display tab
                tabControl.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgInvalidFieldTime, field.Text),
                Properties.Resources.titleInvalidField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus field again
            field.Focus();

            //field is empty
            return false;
        }

        /// <summary>
        /// Validate uri field.
        /// </summary>
        /// <param name="field">
        /// The uri field.
        /// </param>
        /// <param name="tabControl">
        /// The tab control that contains the tab page of the uri field.
        /// Null if uri field is not in a tab page.
        /// </param>
        /// <param name="tab">
        /// The tab page where the uri field is located.
        /// Null if uri field is not in a tab page.
        /// </param>
        /// <returns>
        /// True if uri field is valid.
        /// False if uri field is invalid.
        /// </returns>
        protected bool ValidateUriField(
            MetroTextBox field, MetroTabControl tabControl, TabPage tab)
        {
            //check field
            if (field.Text.Length > 0)
            {
                //create regular expression to valiate uri
                Regex reg = new Regex(
                     @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$", 
                     RegexOptions.Compiled | RegexOptions.IgnoreCase);

                //validate uri
                if (reg.IsMatch(field.Text))
                {
                    //uri is valid
                    return true;
                }
            }

            //focus field
            field.Focus();

            //check tab
            if (tabControl != null && tab != null)
            {
                //display tab
                tabControl.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgInvalidFieldUri, field.Text),
                Properties.Resources.titleInvalidField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus field again
            field.Focus();

            //field is empty
            return false;
        }

        /// <summary>
        /// Validate zip code field.
        /// </summary>
        /// <param name="field">
        /// The zip field.
        /// </param>
        /// <param name="tabControl">
        /// The tab control that contains the tab page of the zip field.
        /// Null if zip field is not in a tab page.
        /// </param>
        /// <param name="tab">
        /// The tab page where the zip field is located.
        /// Null if zip field is not in a tab page.
        /// </param>
        /// <returns>
        /// True if zip field is valid.
        /// False if zip field is invalid.
        /// </returns>
        protected bool ValidateZipField(
            MaskedTextBox field, MetroTabControl tabControl, TabPage tab)
        {
            //check field
            if (field.Text.Length == 9)
            {
                //zip is valid
                return true;
            }

            //focus field
            field.Focus();

            //check tab
            if (tabControl != null && tab != null)
            {
                //display tab
                tabControl.SelectedTab = tab;
            }

            //display message
            MetroMessageBox.Show(Manager.MainForm, string.Format(
                Properties.Resources.msgInvalidFieldZip, field.Text),
                Properties.Resources.titleInvalidField,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //focus field again
            field.Focus();

            //field is empty
            return false;
        }

        #endregion Protected Methods


        #region Private Methods *******************************************************

        /// <summary>
        /// Format description text.
        /// </summary>
        /// <param name="description">
        /// The description to be formatted.
        /// </param>
        /// <returns>
        /// The formatted descirption.
        /// </returns>
        private string FormatDescription(string description)
        {
            //format description
            description = description.Trim().ToLower();
            description = description.Replace(" ", string.Empty);
            description = description.Replace(":", string.Empty);
            description = description.Replace(".", string.Empty);
            description = description.Replace(",", string.Empty);
            description = description.Replace(";", string.Empty);
            description = description.Replace("(", string.Empty);
            description = description.Replace(")", string.Empty);
            description = description.Replace("-", string.Empty);
            description = description.Replace("_", string.Empty);
            description = description.Replace('á', 'a');
            description = description.Replace('é', 'e');
            description = description.Replace('í', 'i');
            description = description.Replace('ó', 'o');
            description = description.Replace('ú', 'u');
            description = description.Replace('â', 'a');
            description = description.Replace('ê', 'e');
            description = description.Replace('î', 'i');
            description = description.Replace('ô', 'o');
            description = description.Replace('û', 'u');
            description = description.Replace('à', 'a');
            description = description.Replace('è', 'e');
            description = description.Replace('ì', 'i');
            description = description.Replace('ò', 'o');
            description = description.Replace('ù', 'u');
            description = description.Replace('ä', 'a');
            description = description.Replace('ë', 'e');
            description = description.Replace('ï', 'i');
            description = description.Replace('ö', 'o');
            description = description.Replace('ü', 'u');
            description = description.Replace('ç', 'c');
            description = description.Replace('ã', 'a');
            description = description.Replace('õ', 'o');

            //return formatted description
            return description;
        }

        /// <summary>
        /// Format description texts.
        /// </summary>
        /// <param name="descriptions">
        /// The descriptions to be formatted.
        /// </param>
        private void FormatDescriptions(string[] descriptions)
        {
            //check array
            if (descriptions == null || descriptions.Length == 0)
            {
                //exit
                return;
            }

            //check each descrition
            for (int i = 0; i < descriptions.Length; i++)
            {
                //get current description
                string description = descriptions[i];

                //check description
                if (description == null || description.Length == 0)
                {
                    //go to next description
                    continue;
                }

                //format description and update array
                descriptions[i] = FormatDescription(description);
            }
        }

        /// <summary>
        /// Perform a data load for the selected item.
        /// </summary>
        /// <param name="itemId">
        /// THe selected item ID.
        /// </param>
        private void PerformDataLoad(int itemId)
        {
            try
            {
                //set cursor to wait
                this.Cursor = Cursors.WaitCursor;

                //clear current fields
                ClearFields();

                //load data for the selected item and check result
                if (!LoadItemData(itemId))
                {
                    //could not load data
                    //clear selection
                    lsItems.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                //show error message
                MetroMessageBox.Show(Manager.MainForm, string.Format(
                    Properties.Resources.errorChannelLoadData, itemTypeDescription,  ex.Message), 
                    Properties.Resources.titleChannelError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log exception
                Manager.Log.WriteError(string.Format(
                    Properties.Resources.errorChannelLoadData, itemTypeDescription, ex.Message));
                Manager.Log.WriteException(ex);

                //could not load data
                //clear selection
                lsItems.SelectedIndex = -1;
            }
            finally
            {
                //reset cursor
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Refresh buttons by settings their state according to current control status.
        /// </summary>
        private void RefreshButtons()
        {
            //enable cancel button while editing
            btCancel.Enabled = ((Status == RegisterStatus.Editing) ||
                             (Status == RegisterStatus.Creating));

            //disable save button while consulting
            btSave.Enabled = ((Status == RegisterStatus.Editing) ||
                             (Status == RegisterStatus.Creating));

            //enable new button while consulting
            btNew.Enabled = Status == RegisterStatus.Consulting && allowAddItem;

            //enable copy button
            btCopy.Enabled =
                (lsItems.SelectedIndex > -1) &&
                (Status == RegisterStatus.Consulting) &&
                allowAddItem && allowEditItem;

            //enable edit button
            btEdit.Enabled =
                (lsItems.SelectedIndex > -1) &&
                (Status == RegisterStatus.Consulting) &&
                allowEditItem;

            //enable delete button
            btDelete.Enabled =
                (lsItems.SelectedIndex > -1) &&
                (Status == RegisterStatus.Consulting) &&
                allowDeleteItem;

            //disable delete button if item is inactive
            if (lsItems.SelectedItem != null &&
                ((IdDescriptionStatus)lsItems.SelectedItem).Status == (int)ItemStatus.Inactive)
            {
                btDelete.Enabled = false;
            }
        }

        /// <summary>
        /// Set the displayed item list.
        /// </summary>
        /// <param name="items"></param>
        private void SetDisplayedItemList(List<IdDescriptionStatus> items)
        {
            //get current selected item
            int currentItemId = SelectedItemId;

            //check if there is any item
            if (items.Count > 0)
            {
                //when a new list is databinded the first item is automatically selected
                //skip one data load
                skipDataLoad = true;
            }

            //update displayed item list
            this.displayedItems = items;

            //set list to be displayed
            lsItems.DisplayMember = "Description";
            lsItems.ValueMember = "Id";
            lsItems.DataSource = this.displayedItems;

            //check number of displayed items
            if (this.displayedItems.Count == 0)
            {
                //no item to be displayed
                //item selected index changed will not be called since list is empty
                //simulate an item selected index changed event
                lsItems_SelectedIndexChanged(this, new EventArgs());
            }

            //check selected item id
            if (currentItemId > 0)
            {
                //check if loaded items contains selected item id
                if (items.Find(x => x.Id == currentItemId) != null)
                {
                    //must reselect item
                    //check if selected item is not the item to be reselected
                    if (SelectedItemId != currentItemId)
                    {
                        //skip one data load
                        skipDataLoad = true;

                        //reselect item without loading data
                        SelectedItemId = currentItemId;

                        //reset flag
                        skipDataLoad = false;
                    }
                }
                else
                {
                    //clear automatic selection
                    lsItems.ClearSelected();
                }
            }
            else
            {
                //clear automatic selection
                lsItems.ClearSelected();
            }
        }

        /// <summary>
        /// Method to populate the list box with the items currently in the database.
        /// It must be implemented by the especific user control to load the proper data.
        /// </summary>
        /// <returns></returns>
        private void RefreshDisplayedItemList()
        {
            //check if any item was loaded
            if (loadedItems == null && loadedItems.Count == 0)
            {
                //no item was loaded
                //display empty list
                SetDisplayedItemList(new List<IdDescriptionStatus>());

                //exit
                return;
            }

            //create list of filtered items
            List<IdDescriptionStatus> filteredItems = new List<IdDescriptionStatus>(loadedItems.Count);

            //check each loaded item
            foreach (IdDescriptionStatus item in loadedItems)
            {
                //check if should filter by status
                if (this.classHasStatus && this.hideInactiveItems)
                {
                    //check item status
                    if (item.Status == (int)ItemStatus.Inactive)
                    {
                        //item is not active
                        //skip it
                        continue;
                    }
                }

                //add item
                filteredItems.Add((IdDescriptionStatus)item);
            }

            //sort items by description
            filteredItems.Sort((x, y) => x.Description.CompareTo(y.Description));

            //compare list of descriptions with current list
            if (!CompareToDisplayedItems(filteredItems))
            {
                //display filtered loaded items
                SetDisplayedItemList(filteredItems);
            }
        }

        /// <summary>
        /// Find item that matches all search words.
        /// </summary>
        /// <param name="searchString">
        /// The input seatch text with words.
        /// </param>
        /// <returns>
        /// The first item that contains all input words.
        /// </returns>
        private IdDescriptionStatus FindListItemByWords(string searchString)
        {
            //reset last search result
            searchedItems = new List<IdDescriptionStatus>();
            selectedSearchedItemIndex = int.MinValue;

            //disable next item button
            mbtnNextSearchResult.Enabled = false;

            //split words using upper case
            string[] words = searchString.ToUpper().Split(
                new char[] { ' ', '.', ',', ':', ';', '-', '(', ')' });

            //format words
            FormatDescriptions(words);

            //check each item
            foreach (IdDescriptionStatus item in displayedItems)
            {
                //get upper description
                string[] descriptions = item.Description.ToUpper().Split(
                    new char[] { ' ', '.', ',', ':', ';', '-', '(', ')' });

                //format item descriptions
                FormatDescriptions(descriptions);

                //check if description contains each word
                bool foundWords = true;

                //check each word against description
                foreach (string word in words)
                {
                    //check current word
                    if (words == null || word.Length == 0)
                    {
                        //go to next word
                        continue;
                    }

                    //current word is a valid word
                    //must match a description
                    bool foundMatch = false;

                    //check each description 
                    foreach (string description in descriptions)
                    {
                        //check current description 
                        if (description == null || description.Length == 0)
                        {
                            //go to next description
                            continue;
                        }

                        //check if current description starts with current word
                        if (description.StartsWith(word))
                        {
                            //found match for word
                            foundMatch = true;

                            //exit loop
                            break;
                        }
                    }

                    //check a description matches current word
                    if (!foundMatch)
                    {
                        //no descripton matches current word
                        //not all words were found
                        foundWords = false;

                        //exit loop
                        break;
                    }
                }

                //check final result
                if (foundWords)
                {
                    //all words were found
                    //current item does match
                    //add current item to search result list
                    searchedItems.Add(item);
                }
            }

            //check number of found items
            if (searchedItems.Count > 0)
            {
                //enable next item button
                mbtnNextSearchResult.Enabled = true;

                //select first item
                selectedSearchedItemIndex = 0;

                //return first item
                return searchedItems[0];
            }

            //no matching item            
            return null;
        }

        #endregion Private Methods

    } //end of class RegisterBaseControl

} //end of namespace PnT.SongClient.UI.Controls
