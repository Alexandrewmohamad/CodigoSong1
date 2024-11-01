using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using PnT.SongClient.Logic;


namespace PnT.SongClient.Data
{
    /// <summary>
    /// Manage application settings.
    /// </summary>
    public class SettingManager
    {

        #region Fields ****************************************************************

        /// <summary>
        /// XML settings manager. Store settings on a XML file.
        /// </summary>
        XmlSettings xmlSettings = null;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SettingManager()
        {
            //create xml setting manager
            xmlSettings = new XmlSettings(Manager.SETTING_FILE_PATH, Manager.Log);
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SettingManager(string filePath)
        {
            //create xml setting manager
            xmlSettings = new XmlSettings(filePath);
        }

        #endregion Constructors


        #region Properties ************************************************************

        /// <summary>
        /// Get/set song server IP.
        /// </summary>
        public string ServerIP
        {
            get
            {
                //get stored value or return default value
                return xmlSettings.GetString("Router\\ServerIP", "18.231.79.85");
            }
            set
            {
                //set value
                xmlSettings.SetString("Router\\ServerIP", value);
            }
        }


        /// <summary>
        /// Get/set OMS Router Port.
        /// </summary>
        public int ServerPort
        {
            get
            {
                //get stored value or return default value
                return xmlSettings.GetInt("Router\\ServerPort", 80);
            }
            set
            {
                //set value
                xmlSettings.SetInt("Router\\ServerPort", value);
            }
        }

        #endregion Properties


        #region Administrative Properties *********************************************

        /// <summary>
        /// True if settings were loaded.
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                //check xml settings
                if (xmlSettings == null)
                {
                    //could not possibly have loaded settings
                    return false;
                }

                //return if settings are loaded
                return xmlSettings.IsLoaded;
            }
        }

        #endregion Administrative Properties


        #region Private Methods *******************************************************

        /// <summary>
        /// Load settings from file.
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            //load settings and return result
            return xmlSettings.LoadOrCreate();
        }

        /// <summary>
        /// Save settings to file.
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            //save settings and return result
            return xmlSettings.Save();
        }

        #endregion Private Methods


    } //end of class SettingManager

} //end of namespace PnT.SongClient.Data
