using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using System.Globalization;
using System.Security.AccessControl;

namespace PnT.SongClient.Data
{
    public class XmlSettings
    {
        #region Constants *************************************************************

        /// <summary>
        /// Root node name in the setting file.
        /// </summary>
        public const string ROOT_NAME = "Settings";

        #endregion Constants


        #region Fields ****************************************************************

        /// <summary>
        /// The path of the XML setting file.
        /// </summary>
        string filePath;

        /// <summary>
        /// The threaded log manager to log messages.
        /// </summary>
        ThreadedLogManager threadedLogManager = null;

        /// <summary>
        /// The XML document to store the nodes.
        /// </summary>
        XmlDocument xmlDocument;

        #endregion Fields


        #region Constructor ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="filePath">
        /// The file path of the setting XML file.
        /// </param>
        public XmlSettings(string filePath, ThreadedLogManager threadedLogManager)
        {
            //set file path
            this.filePath = filePath;

            //set threaded log manager
            this.threadedLogManager = threadedLogManager;

            //settings were not loaded yet
            IsLoaded = false;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="filePath">The file path of the setting XML file.</param>
        public XmlSettings(string filePath)
        {
            //set file path
            this.filePath = filePath;

            //settings were not loaded yet
            IsLoaded = false;
        }

        #endregion Constructor


        #region Properties ************************************************************

        /// <summary>
        /// Get the XML file path.
        /// </summary>
        public string FilePath
        {
            get { return this.filePath; }
        }

        /// <summary>
        /// True is settings were loaded.
        /// </summary>
        public bool IsLoaded
        {
            get;
            private set;
        }

        #endregion Properties


        #region Public Methods ********************************************************

        /// <summary>
        /// If filename exists, loads configuration file. If it does not exist,
        /// configures _xmlDocument to create a new one when the config is saved.
        /// </summary>
        /// <returns>true if file does not exist or load is successful; false if load fails</returns>
        public bool LoadOrCreate()
        {
            try
            {
                //check file path
                if (filePath == null || filePath.Length == 0)
                {
                    //file path was not set correctly
                    return false;
                }

                //check if file exists
                if (File.Exists(filePath))
                {
                    //file exists
                    //load data
                    return Load();
                }

                //file does not exist
                FileInfo settingFile = new FileInfo(filePath);

                //check if directory exists
                if (!settingFile.Directory.Exists)
                {
                    //directory does not exist
                    //create directory
                    settingFile.Directory.Create();
                }

                //create a new document
                xmlDocument = new XmlDocument();

                //lock xmlDocument
                lock (xmlDocument)
                {

                    //create XML header
                    XmlNode xmlNode = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
                    xmlDocument.AppendChild(xmlNode);

                    //create root element
                    XmlElement xmlElement = xmlDocument.CreateElement(ROOT_NAME);
                    xmlDocument.AppendChild(xmlElement);

                    //save setting file and set IsLoaded option
                    IsLoaded = Save();

                    //return result
                    return IsLoaded;
                }
            }
            catch (Exception ex)
            {
                //unexpected error while loading setting file
                string errorMessage =
                    "Unexpected error while loading settings " +
                    " from file " + filePath + ": " + ex.Message;

                //check threaded log manager
                if (threadedLogManager != null)
                {
                    //log message
                    threadedLogManager.WriteError(errorMessage);
                }

                //settings were not loaded
                IsLoaded = false;

                //return negative result
                return false;
            }
        }


        /// <summary>
        /// Save setting data back to file on hard disk.
        /// </summary>
        /// <returns>
        /// True if XML setting file was saved.
        /// False otherwise.
        /// </returns>
        public bool Save()
        {
            try
            {
                //check if there is a xml document to be saved
                if (xmlDocument == null)
                {
                    //settings were not loaded
                    return false;
                }

                //lock xml document
                lock (xmlDocument)
                {
                    ////sort xmlDocument
                    //XmlNode rootNode = xmlDocument[ROOT_NAME];

                    ////check each child node
                    //for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                    //{
                    //    for (int j = i + 1; j < rootNode.ChildNodes.Count; j++)
                    //    {
                    //        if (rootNode.ChildNodes[i].Name.CompareTo(
                    //            rootNode.ChildNodes[j].Name) > 0)
                    //        {
                    //            //send child node to the back
                    //            XmlNode childNode = rootNode.ChildNodes[j];
                    //            rootNode.RemoveChild(childNode);
                    //            rootNode.InsertBefore(childNode, rootNode.ChildNodes[i]);
                    //        }
                    //    }
                    //}

                    //save settings file to temporary file
                    xmlDocument.Save(this.filePath + ".temp");
                }

                //check if current setting file exists
                if (File.Exists(this.filePath))
                {
                    //check if backup file exists
                    if (File.Exists(this.filePath + ".bkp"))
                    {
                        //delete backup
                        File.Delete(this.filePath + ".bkp");
                    }

                    //rename setting file to backup
                    File.Move(this.filePath, this.filePath + ".bkp");
                }

                //rename 
                File.Move(this.filePath + ".temp", this.filePath);

                //settings were saved
                return true;
            }
            catch (Exception ex)
            {
                //error while saving setting file
                string errorMessage =
                    "Unexpected error while saving settings " +
                    " to file " + filePath + ": " + ex.Message;

                //check threaded log manager
                if (threadedLogManager != null)
                {
                    //log message
                    threadedLogManager.WriteError(errorMessage);
                }

                //return negative result
                return false;
            }
        }


        /// <summary>
        /// Check if a setting is already in the setting file.
        /// </summary>
        /// <param name="path">The setting path.</param>
        /// <returns>True if setting was found. False otherwise.</returns>
        public bool HasSetting(string path)
        {
            //check is settings were loaded
            if (!IsLoaded || xmlDocument == null)
            {
                //settings were not loaded yet
                return false;
            }

            //get node from given path
            //return if node was found
            return (GetXmlNode(path) != null);
        }

        /// <summary>
        /// Remova a setting already in the setting file.
        /// </summary>
        /// <param name="path">The setting path.</param>
        /// <returns>True if setting was removed. False if setting was not found.</returns>
        public bool RemoveSetting(string path)
        {
            //check is settings were loaded
            if (!IsLoaded || xmlDocument == null)
            {
                //settings were not loaded yet
                return false;
            }

            //remove node from given path
            //return result
            return RemoveXmlNode(path);
        }

        #endregion Public Methods


        #region Setting Methods *******************************************************

        /// <summary>
        /// Get bool value for given setting or return default value.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value of the setting. 
        /// Default value if setting was not found.
        /// </returns>
        public bool GetBool(string path, bool defaultValue)
        {
            //get value
            string str = GetNodeValue(path);

            //check result
            if (str == null)
            {
                //node was not found
                //return default value
                return defaultValue;
            }

            //check value
            if (str.ToLower() == "true")
            {
                return true;
            }
            else if (str.ToLower() == "false")
            {
                return false;
            }

            //could not parse value
            PrintParseErrorMsg(path, str);

            //return default value
            return defaultValue;
        }

        /// <summary>
        /// Get Color value for given setting or return default value.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value of the setting. 
        /// Default value if setting was not found.
        /// </returns>
        public Color GetColor(string path, Color defaultValue)
        {
            //get value
            string str = GetNodeValue(path);

            //check result
            if (str == null)
            {
                //node was not found
                //return default value
                return defaultValue;
            }

            try
            {
                string prepStr = str;
                if (prepStr[0] == '#')
                {
                    prepStr = str.Substring(1);
                }

                int value = int.Parse(prepStr, NumberStyles.AllowHexSpecifier);

                return IntToColor(value);
            }
            catch
            {
                PrintParseErrorMsg(path, str);
                return defaultValue;
            }
        }


        /// <summary>
        /// Get CultureInfo value for given setting or return default value.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value of the setting. 
        /// Default value if setting was not found.
        /// </returns>
        public CultureInfo GetCultureInfo(string path, CultureInfo defaultValue)
        {
            //get text from node
            string str = GetNodeValue(path);

            //check result
            if (str == null)
            {
                //node was not found
                //return default value
                return defaultValue;
            }
            else
            {
                try
                {
                    //get culture info from text
                    return new CultureInfo(str);
                }
                catch
                {
                    //log parse error
                    PrintParseErrorMsg(path, str);

                    //return default value
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// Get DateTime value for given setting or return default value.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value of the setting. 
        /// Default value if setting was not found.
        /// </returns>
        public DateTime GetDateTime(string path, DateTime defaultValue)
        {
            //get node value
            string str = GetNodeValue(path);

            //check result
            if (str == null)
            {
                //node was not found
                //return default value
                return defaultValue;
            }
            else
            {
                DateTime result;

                if (DateTime.TryParse(str, out result))
                {
                    return result;
                }

                PrintParseErrorMsg(path, str);
                return defaultValue;
            }
        }

        /// <summary>
        /// Get double value for given setting or return default value.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value of the setting. 
        /// Default value if setting was not found.
        /// </returns>
        public double GetDouble(string path, double defaultValue)
        {
            //get text from node
            string str = GetNodeValue(path);

            if (str == null)
            {
                //node was not found
                //return default value
                return defaultValue;
            }
            else
            {
                //get double from text
                double result;

                //parse and check result
                if (double.TryParse(str, NumberStyles.Float,
                    CultureInfo.InvariantCulture, out result))
                {
                    //return pared double
                    return result;
                }

                //log parse error
                PrintParseErrorMsg(path, str);

                //return default value
                return defaultValue;
            }
        }

        /// <summary>
        /// Get int value for given setting or return default value.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value of the setting. 
        /// Default value if setting was not found.
        /// </returns>
        public int GetInt(string path, int defaultValue)
        {
            //get node value
            string str = GetNodeValue(path);

            if (str == null)
            {
                //node was not found
                //return default value
                return defaultValue;
            }
            else
            {
                //get integer from text
                int result;

                //parse and check result
                if (int.TryParse(str, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out result))
                {
                    //return parsed integer
                    return result;
                }

                //log parse error
                PrintParseErrorMsg(path, str);

                //return default value
                return defaultValue;
            }
        }

        /// <summary>
        /// Get string value for given setting or return default value.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value of the setting. 
        /// Default value if setting was not found.
        /// </returns>
        public string GetString(string path, string defaultValue)
        {
            //get node value
            string str = GetNodeValue(path);

            //check result
            if (str != null)
            {
                return str;
            }
            else
            {
                //node was not found
                //return default value
                return defaultValue;
            }
        }

        /// <summary>
        /// Set bool value for setting.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="value">The new value of the setting.</param>
        public void SetBool(string path, bool value)
        {
            //operation must be done all in one step to be thread safe
            //lock xml document
            lock (xmlDocument)
            {
                //get node
                XmlElement elem = GetOrCreateXmlElement(path);

                //set inner text value
                elem.InnerText = value.ToString();
            }
        }

        /// <summary>
        /// Set Color value for setting.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="value">The new value of the setting.</param>
        public void SetColor(string path, Color value)
        {
            //operation must be done all in one step to be thread safe
            //lock xml document
            lock (xmlDocument)
            {
                //get node
                XmlElement elem = GetOrCreateXmlElement(path);

                //set inner text value
                elem.InnerText = String.Format("#{0:X}", ColorToInt(value));
            }
        }

        public void SetCultureInfo(string path, CultureInfo value)
        {
            //operation must be done all in one step to be thread safe
            //lock xml document
            lock (xmlDocument)
            {
                //get node
                XmlElement elem = GetOrCreateXmlElement(path);

                //set inner text value
                elem.InnerText = value.ToString();
            }
        }

        /// <summary>
        /// Set DateTime value for setting.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="value">The new value of the setting.</param>
        public void SetDateTime(string path, DateTime value)
        {
            //operation must be done all in one step to be thread safe
            //lock xml document
            lock (xmlDocument)
            {
                //get node
                XmlElement elem = GetOrCreateXmlElement(path);

                //set inner text value
                elem.InnerText = value.ToString();
            }
        }

        /// <summary>
        /// Set double value for setting.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="value">The new value of the setting.</param>
        public void SetDouble(string path, double value)
        {
            //operation must be done all in one step to be thread safe
            //lock xml document
            lock (xmlDocument)
            {
                //get node
                XmlElement elem = GetOrCreateXmlElement(path);

                //set inner text value
                elem.InnerText = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Set int value for setting.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="value">The new value of the setting.</param>
        public void SetInt(string path, int value)
        {
            //operation must be done all in one step to be thread safe
            //lock xml document
            lock (xmlDocument)
            {
                //get node
                XmlElement elem = GetOrCreateXmlElement(path);

                //set inner text value
                elem.InnerText = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Set string value for setting.
        /// </summary>
        /// <param name="path">The path of the setting.</param>
        /// <param name="value">The new value of the setting.</param>
        public void SetString(string path, string value)
        {
            //operation must be done all in one step to be thread safe
            //lock xml document
            lock (xmlDocument)
            {
                //get node
                XmlElement elem = GetOrCreateXmlElement(path);

                //set inner text value
                elem.InnerText = value;
            }
        }

        #endregion Setting Methods


        #region Protected Methods *****************************************************

        protected XmlElement GetOrCreateXmlElement(string configPath)
        {
            //split path nodes
            string[] pathElems = configPath.Split(new char[] { '\\', '/' },
                StringSplitOptions.RemoveEmptyEntries);

            //lock xml document
            lock (xmlDocument)
            {
                //get root node
                XmlNode xmlNode = (XmlNode)xmlDocument[ROOT_NAME];

                //search for elements under root node
                foreach (string elem in pathElems)
                {
                    //try getting element by name
                    XmlElement xmlElem = xmlNode[elem];

                    //check result
                    if (xmlElem == null)
                    {
                        //element was not found
                        xmlElem = xmlDocument.CreateElement(elem);

                        //append child ordering by name
                        //check each child node
                        for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
                        {
                            //compare names
                            if (xmlElem.Name.CompareTo(xmlNode.ChildNodes[i].Name) < 0)
                            {
                                //insert new node before current child node
                                xmlNode.InsertBefore(xmlElem, xmlNode.ChildNodes[i]);

                                //exit loop
                                break;
                            }
                        }

                        //check if node was inserted
                        if (xmlNode[elem] == null)
                        {
                            //node was not inserted
                            //append it to the end
                            xmlNode.AppendChild(xmlElem);
                        }
                    }

                    //update current node
                    xmlNode = xmlElem;
                }

                //node was found or created
                //return last node
                return xmlNode as XmlElement;
            }
        }

        protected XmlNode GetXmlNode(string configPath)
        {
            //check if settings are loaded
            if (!IsLoaded || xmlDocument == null)
            {
                //settings are not loaded yet
                return null;
            }

            string[] pathElems = configPath.Split(new char[] { '\\', '/' },
                StringSplitOptions.RemoveEmptyEntries);

            //lock xml document
            lock (xmlDocument)
            {
                //get root node
                XmlNode xmlNode = (XmlNode)xmlDocument[ROOT_NAME];

                //search for elements under root node
                foreach (string elem in pathElems)
                {
                    XmlElement xmlElem = xmlNode[elem];
                    if (xmlElem == null)
                    {
                        return null;
                    }
                    else
                    {
                        xmlNode = xmlElem;
                    }
                }

                // Node was found, return it.
                return xmlNode;
            }
        }

        protected bool RemoveXmlNode(string configPath)
        {
            //check if settings are loaded
            if (!IsLoaded || xmlDocument == null)
            {
                //settings are not loaded yet
                return false;
            }

            string[] pathElems = configPath.Split(new char[] { '\\', '/' },
                StringSplitOptions.RemoveEmptyEntries);

            //lock xml document
            lock (xmlDocument)
            {
                //get root node
                XmlNode xmlNode = (XmlNode)xmlDocument[ROOT_NAME];

                //search for elements under root node
                foreach (string elem in pathElems)
                {
                    XmlElement xmlElem = xmlNode[elem];
                    if (xmlElem == null)
                    {
                        //node was not found
                        return false;
                    }
                    else
                    {
                        xmlNode = xmlElem;
                    }
                }

                //remove node
                xmlNode.ParentNode.RemoveChild(xmlNode);
            }

            //node was removed
            return false;
        }

        /// <summary>
        /// Loads the config file. Null value for filename is accepted, and will
        /// create a XmlSetting that always returns its default values.
        /// </summary>
        /// <returns></returns>
        protected bool Load()
        {
            //xml text reader to read input text
            XmlTextReader xmlTextReader = null;

            try
            {
                //open XML file to read
                xmlTextReader = new XmlTextReader(filePath);

                //remove white spaces
                xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;

                //create xml document
                xmlDocument = new XmlDocument();

                //lock xml document
                lock (xmlDocument)
                {
                    //load stream on xml document
                    xmlDocument.Load(xmlTextReader);

                    //settings were loaded
                    IsLoaded = true;

                    //return positive result
                    return true;
                }
            }
            catch (Exception ex)
            {
                //create error message
                string errorMessage = String.Format(
                    "Could not read from '{0}': {1}", filePath, ex.Message);

                //check threaded log manager
                if (threadedLogManager != null)
                {
                    //log error
                    threadedLogManager.WriteError(errorMessage);
                }

                //settings were not loaded
                IsLoaded = false;

                //return negative result
                return false;
            }
            finally
            {
                //close xml reader
                if (xmlTextReader != null)
                {
                    xmlTextReader.Close();
                }
            }
        }

        /// <summary>
        /// Get node inner text value.
        /// </summary>
        /// <param name="path">The node path.</param>
        /// <returns>
        /// The inner text value.
        /// Null if node was not found.
        /// </returns>
        protected string GetNodeValue(string path)
        {
            //get node from given path
            XmlNode xmlNode = GetXmlNode(path);

            //check result
            if (xmlNode == null)
            {
                //node was not found
                return null;
            }
            else
            {
                //node was found
                //return inner text
                return xmlNode.InnerText;
            }
        }

        protected void PrintParseErrorMsg(string path, string text)
        {
            string parseErrorMsg = String.Format(
                "Value '{2}' of node '{0}' in config file '{1}' " +
                "could not be parsed.", path, this.filePath, text);

            if (threadedLogManager != null)
            {
                threadedLogManager.WriteError(parseErrorMsg);
            }
        }

        protected int ColorToInt(Color color)
        {
            return (int)color.R | (int)color.G << 8 | (int)color.B << 16;
        }

        protected Color IntToColor(int color)
        {
            return Color.FromArgb(color & 0xFF, (color >> 8) & 0xFF, (color >> 16) & 0xFF);
        }

        #endregion Protected Methods

    } //end of public class XmlSetting   

} //end of namespace PnT.SongClient.Data
