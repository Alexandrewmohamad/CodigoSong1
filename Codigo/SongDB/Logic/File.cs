using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for file data.
    /// </summary>
    [DataContract]
    public class File
    {

        #region Fields *****************************************************************

        /// <summary>
        /// The path of the file in the server.
        /// </summary>
        private string filePath = null;

        /// <summary>
        /// The data of the file. In bytes.
        /// </summary>
        private byte[] data = null;

        /// <summary>
        /// The database select result.
        /// </summary>
        private int result;

        /// <summary>
        /// The database select error message.
        /// </summary>
        private string errorMessage = null;

        #endregion Fields


        #region Constructors ***********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public File()
        {
        }

        #endregion Constructors


        #region Properties *************************************************************

        /// <summary>
        /// Get/set the path of the file in the server.
        /// </summary>
        [DataMember]
        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
            }
        }

        /// <summary>
        /// Get/set the data of the file. In bytes.
        /// </summary>
        [DataMember]
        public byte[] Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        /// <summary>
        /// Get/set the database select result.
        /// </summary>
        [DataMember]
        public int Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }

        /// <summary>
        /// Get/set the database select error message.
        /// </summary>
        [DataMember]
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }

            set
            {
                errorMessage = value;
            }
        }

        /// <summary>
        /// Get the extension of the file. 
        /// </summary>
        public string FileExtension
        {
            get
            {
                //get file name
                string fileName = FileName;

                //check file name
                if (fileName == null || fileName.Length == 0)
                {
                    //no name is set
                    //file has no extension
                    return string.Empty;
                }

                //check if there is a point
                if (FileName.IndexOf('.') == -1 || FileName.EndsWith("."))
                {
                    //file has no extension
                    return string.Empty;
                }

                //get and return extension
                return fileName.Substring(FileName.LastIndexOf('.') + 1);
            }
        }

        /// <summary>
        /// Get the name of the file.
        /// </summary>
        public string FileName
        {
            get
            {
                //check file path
                if (filePath == null || filePath.Length == 0)
                {
                    //no name is set
                    return string.Empty;
                }

                //get file name by splitting file path
                string[] words = filePath.Split(new char[] {
                    '\\', '/'});

                //check number of words
                if (words.Length == 0)
                {
                    //no word was found
                    //should never happen
                    return string.Empty;
                }

                //return last word
                return words[words.Length - 1];
            }
        }

        #endregion Properties

    } //end of class File

} //end of namespace PnT.SongDB.Logic
