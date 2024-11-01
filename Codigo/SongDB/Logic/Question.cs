using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Enumerates the possible question rapporteurs.
    /// </summary>
    public enum QuestionRapporteur
    {
        Teacher = 0,
        Coordinator
    }

    /// <summary>
    /// Enumerates the possible question targets.
    /// </summary>
    public enum QuestionTarget
    {
        Class = 0,
        Institution,
        Self
    }

    /// <summary>
    /// Enumerates the possible report periodicities.
    /// </summary>
    public enum QuestionPeriodicity
    {
        Month = 0,
        Semester
    }

    /// <summary>
    /// Enumerates the possible question metrics.
    /// </summary>
    public enum QuestionMetric
    {
        Score0To10 = 0,
        CommentOnly
    }


    /// <summary>
    /// Summary description for Question.
    /// </summary>
    [DataContract]
    public class Question
    {

        #region Fields *****************************************************************

        private int questionId;
        private int questionRapporteur;
        private int questionTarget;
        private int questionPeriodicity;
        private int questionMetric;
        private bool commentsRequired;
        private string text;
        private string plusLabel;
        private string minusLabel;

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
        public Question()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="questionId">The id of the Question.</param>
        public Question(int questionId)
        {
            this.questionId = questionId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get { return this.questionId; }
            set { this.questionId = value; }
        }

        [DataMember]
        public int QuestionId
        {
            get { return this.questionId; }
            set { this.questionId = value; }
        }

        [DataMember]
        public int QuestionRapporteur
        {
            get { return this.questionRapporteur; }
            set { this.questionRapporteur = value; }
        }

        [DataMember]
        public int QuestionTarget
        {
            get { return this.questionTarget; }
            set { this.questionTarget = value; }
        }

        [DataMember]
        public int QuestionPeriodicity
        {
            get { return this.questionPeriodicity; }
            set { this.questionPeriodicity = value; }
        }

        [DataMember]
        public int QuestionMetric
        {
            get { return this.questionMetric; }
            set { this.questionMetric = value; }
        }

        [DataMember]
        public bool CommentsRequired
        {
            get { return this.commentsRequired; }
            set { this.commentsRequired = value; }
        }

        [DataMember]
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        [DataMember]
        public string PlusLabel
        {
            get { return this.plusLabel; }
            set { this.plusLabel = value; }
        }

        [DataMember]
        public string MinusLabel
        {
            get { return this.minusLabel; }
            set { this.minusLabel = value; }
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

        #endregion Properties


        #region Methods ****************************************************************

        /// <summary>
        /// Save Question to database.
        /// </summary>
        /// <returns>The id of the saved Question.</returns>
        public int Save()
        {
            questionId = Mapper.QuestionMapper.Save(null, this);
            return questionId;
        }

        /// <summary>
        /// Save Question to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Question.</returns>
        public int Save(MySqlTransaction trans)
        {
            questionId = Mapper.QuestionMapper.Save(trans, this);
            return questionId;
        }

        /// <summary>
        /// Delete Question by id.
        /// </summary>
        /// <param name="id">The id of the selected Question.</param>
        /// <returns>
        /// True if selected Question was deleted.
        /// False if selected Question was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.QuestionMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Question by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Question.</param>
        /// <returns>
        /// True if selected Question was deleted.
        /// False if selected Question was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.QuestionMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Question by id.
        /// </summary>
        /// <param name="id">The id of the selected Question.</param>
        /// <returns>
        /// True if selected Question was inactivated.
        /// False if selected Question was not found.
        /// </returns>
        public static bool Inactivate(int id)
        {
            return Mapper.QuestionMapper.Inactivate(null, id);
        }

        /// <summary>
        /// Inactivate Question by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Question.</param>
        /// <returns>
        /// True if selected Question was inactivated.
        /// False if selected Question was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Mapper.QuestionMapper.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Question.
        /// </summary>
        /// <returns>
        /// List of Question objects.
        /// Null if no Question was found.
        /// </returns>
        public static List<Question> Find()
        {
            return Mapper.QuestionMapper.Find(null);
        }

        /// <summary>
        /// Find all Question with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Question objects.
        /// Null if no Question was found.
        /// </returns>
        public static List<Question> Find(MySqlTransaction trans)
        {
            return Mapper.QuestionMapper.Find(trans);
        }

        /// <summary>
        /// Find Question by id.
        /// </summary>
        /// <param name="id">The id of the selected Question</param>
        /// <returns>
        /// The selected Question.
        /// Null if selected Question was not found.
        /// </returns>
        public static Question Find(int id)
        {
            return Mapper.QuestionMapper.Find(null, id);
        }

        /// <summary>
        /// Find Question by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Question</param>
        /// <returns>
        /// The selected Question.
        /// Null if selected Question was not found.
        /// </returns>
        public static Question Find(MySqlTransaction trans, int id)
        {
            return Mapper.QuestionMapper.Find(trans, id);
        }

        #endregion Methods

    } //end of class Question

} //end of namespace PnT.SongDB.Logic
