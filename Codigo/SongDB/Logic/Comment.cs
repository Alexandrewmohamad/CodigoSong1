using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Represents a dated text comment.
    /// It is saved as a structured text in database.
    /// </summary>
    public class Comment
    {

        #region Constants **************************************************************

        /// <summary>
        /// Fields separator.
        /// </summary>
        public const string SEPARATOR = "-@-";

        /// <summary>
        /// Comments separator.
        /// </summary>
        public const string COMMENT_SEPARATOR = "||";

        #endregion Constants


        #region Fields *****************************************************************

        /// <summary>
        /// The text of the comment.
        /// </summary>
        string text = string.Empty;

        /// <summary>
        /// The date of the comment.
        /// </summary>
        DateTime date = DateTime.MinValue;

        #endregion Fields


        #region Constructors ***********************************************************

        #endregion Constructors


        #region Fields ****************************************************************

        /// <summary>
        /// Get/set the date of the comment.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
            }
        }

        /// <summary>
        /// Get/set the text of the comment.
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        #endregion Fields


        #region Methods ****************************************************************

        /// <summary>
        /// Returns a string that represents the current comment.
        /// </summary>
        /// <returns>
        /// A string that represents the current instrument.
        /// </returns>
        public override string ToString()
        {
            //create and return string
            return this.text + SEPARATOR + this.date.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Parse comment from text.
        /// </summary>
        /// <param name="text">
        /// The input text.
        /// </param>
        /// <returns>
        /// The parsed comment.
        /// Null if text is invalid.
        /// </returns>
        public static Comment FromString(string text)
        {
            try
            {
                //split text
                string[] words = text.Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

                //check result
                if (words.Length != 2)
                {
                    //invalid text
                    return null;
                }
                //create comment
                Comment comment = new Logic.Comment();

                //set comment
                comment.text = words[0];
                comment.date = DateTime.ParseExact(
                    words[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //return result
                return comment;
            }
            catch
            {
                //invalid text
                return null;
            }
        }

        #endregion Methods

    } //end of class Comment

} //end of namespace PnT.SongDB.Logic
