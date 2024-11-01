using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Enumerates the possible instrument types.
    /// </summary>
    public enum InstrumentsType
    {
        Acordeao = 0,
        Afoxe,
        Agogo,
        Atabaque,
        Baixo,
        Bandolim,
        Banjo,
        Bata,
        Bateria,
        BlocoSonoro,
        Bongo,
        Caixa,
        Carrilhao,
        Casaca,
        Castanhola,
        Cavaquinho,
        Caxixi,
        Ceramofone,
        Chimbau,
        Chocalho,
        Clarinete,
        Conga,
        Contrabaixo,
        Cowbell,
        Cuica,
        Darbuka,
        Drums,
        FlautaDoce,
        FlautaTransversa,
        Ganza,
        Guitarra,
        Marimba,
        Oboe,
        Orgao,
        Pandeiro,
        Percussao,
        Piano,
        Pratos,
        RecoReco,
        Repinique,
        Saxofone,
        Sino,
        SinosTubulares,
        Surdo,
        Tambor,
        Tamborim,
        Tanta,
        Teclado,
        Timpano,
        TomTom,
        Triangulo,
        Trombone,
        Trompa,
        Trompete,
        Tuba,
        Vibrafone,
        Viola,
        Violao,
        Violino,
        Violoncelo,
        Xequere,
        Xilofone,
        Zabumba,
        Harpa,
        Escaleta
    }

    /// <summary>
    /// Summary description for Instrument.
    /// </summary>
    [DataContract]
    public class Instrument
    {

        #region Fields *****************************************************************

        private int instrumentId;
        private int poleId;
        private string code;
        private string model;
        private int instrumentType;
        private string storageLocation;
        private string comments;
        private int instrumentStatus;
        private DateTime creationTime;
        private DateTime inactivationTime;
        private string inactivationReason;

        /// <summary>
        /// The name of the pole.
        /// </summary>
        private string poleName;

        /// <summary>
        /// The name of the student of the current active instrument loan.
        /// </summary>
        private string studentName;

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
        public Instrument()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="instrumentId">The id of the Instrument.</param>
        public Instrument(int instrumentId)
        {
            this.instrumentId = instrumentId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.instrumentId;}
            set{ this.instrumentId = value;}
        }

        [DataMember]
        public int InstrumentId
        {
            get{ return this.instrumentId;}
            set{ this.instrumentId = value;}
        }

        [DataMember]
        public int PoleId
        {
            get{ return this.poleId;}
            set{ this.poleId = value;}
        }

        [DataMember]
        public string Code
        {
            get{ return this.code;}
            set{ this.code = value;}
        }

        [DataMember]
        public string Model
        {
            get { return this.model; }
            set { this.model = value; }
        }

        [DataMember]
        public int InstrumentType
        {
            get{ return this.instrumentType;}
            set{ this.instrumentType = value;}
        }

        [DataMember]
        public string StorageLocation
        {
            get{ return this.storageLocation;}
            set{ this.storageLocation = value;}
        }

        [DataMember]
        public string Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        [DataMember]
        public int InstrumentStatus
        {
            get{ return this.instrumentStatus;}
            set{ this.instrumentStatus = value;}
        }

        [DataMember]
        public DateTime CreationTime
        {
            get{ return this.creationTime;}
            set{ this.creationTime = value;}
        }

        [DataMember]
        public DateTime InactivationTime
        {
            get{ return this.inactivationTime;}
            set{ this.inactivationTime = value;}
        }

        [DataMember]
        public string InactivationReason
        {
            get{ return this.inactivationReason;}
            set{ this.inactivationReason = value;}
        }

        /// <summary>
        /// Get/set the name of the pole.
        /// </summary>
        [DataMember]
        public string PoleName
        {
            get
            {
                return poleName;
            }

            set
            {
                poleName = value;
            }
        }

        /// <summary>
        /// Get/set the name of the student of the current active instrument loan.
        /// </summary>
        [DataMember]
        public string StudentName
        {
            get
            {
                return studentName;
            }

            set
            {
                studentName = value;
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

        #endregion Properties


        #region Methods ****************************************************************

        /// <summary>
        /// Save Instrument to database.
        /// </summary>
        /// <returns>The id of the saved Instrument.</returns>
        public int Save()
        {
            instrumentId = Mapper.InstrumentMapper.Save(null, this);
            return instrumentId;
        }

        /// <summary>
        /// Save Instrument to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Instrument.</returns>
        public int Save(MySqlTransaction trans)
        {
            instrumentId = Mapper.InstrumentMapper.Save(trans, this);
            return instrumentId;
        }

        /// <summary>
        /// Delete Instrument by id.
        /// </summary>
        /// <param name="id">The id of the selected Instrument.</param>
        /// <returns>
        /// True if selected Instrument was deleted.
        /// False if selected Instrument was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.InstrumentMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Instrument by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Instrument.</param>
        /// <returns>
        /// True if selected Instrument was deleted.
        /// False if selected Instrument was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.InstrumentMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Instrument by id.
        /// </summary>
        /// <param name="id">The id of the selected Instrument.</param>
        /// <param name="inactivationReason">
        /// The reason why the instrument is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Instrument was inactivated.
        /// False if selected Instrument was not found.
        /// </returns>
        public static bool Inactivate(int id, string inactivationReason)
        {
            return Mapper.InstrumentMapper.Inactivate(null, id, inactivationReason);
        }

        /// <summary>
        /// Inactivate Instrument by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Instrument.</param>
        /// <param name="inactivationReason">
        /// The reason why the instrument is being inactivated.
        /// </param>
        /// <returns>
        /// True if selected Instrument was inactivated.
        /// False if selected Instrument was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id, string inactivationReason)
        {
            return Mapper.InstrumentMapper.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all Instrument.
        /// </summary>
        /// <returns>
        /// List of Instrument objects.
        /// Null if no Instrument was found.
        /// </returns>
        public static List<Instrument> Find()
        {
            return Mapper.InstrumentMapper.Find(null);
        }

        /// <summary>
        /// Find all Instrument with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Instrument objects.
        /// Null if no Instrument was found.
        /// </returns>
        public static List<Instrument> Find(MySqlTransaction trans)
        {
            return Mapper.InstrumentMapper.Find(trans);
        }

        /// <summary>
        /// Count instruments by filter.
        /// </summary>
        /// <param name="filterInstrumentStatus">
        /// The instrument status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <returns>
        /// The number of instruments.
        /// </returns>
        public static int CountByFilter(
            int filterInstrumentStatus, int filterInstrumentType, int filterInstitution, int filterPole)
        {
            return Mapper.InstrumentMapper.CountByFilter(
                null, filterInstrumentStatus, filterInstrumentType, filterInstitution, filterPole);
        }

        /// <summary>
        /// Find instruments by filter.
        /// </summary>
        /// <param name="filterInstrumentStatus">
        /// The instrument status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <returns>
        /// List of Instrument objects.
        /// Null if no Instrument was found.
        /// </returns>
        public static List<Instrument> FindByFilter(
            int filterInstrumentStatus, int filterInstrumentType, int filterInstitution, int filterPole)
        {
            return Mapper.InstrumentMapper.FindByFilter(
                null, filterInstrumentStatus, filterInstrumentType, filterInstitution, filterPole);
        }

        /// <summary>
        /// Find instruments by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="filterInstrumentStatus">
        /// The instrument status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type filter.
        /// -1 to select all types.
        /// </param>
        /// <returns>
        /// List of Instrument objects.
        /// Null if no Instrument was found.
        /// </returns>
        public static List<Instrument> FindByFilter(
            MySqlTransaction trans, int filterInstrumentStatus, int filterInstrumentType, int filterPole)
        {
            return Mapper.InstrumentMapper.FindByFilter(
                trans, filterInstrumentStatus, filterInstrumentType, filterInstrumentType, filterPole);
        }

        /// <summary>
        /// Find all instruments that were loaned to selected student.
        /// </summary>
        /// <param name="studentId">The ID of the selected student.</param>
        /// <returns>
        /// List of Instrument objects.
        /// Null if no Instrument was found.
        /// </returns>
        public static List<Instrument> FindByStudent(int studentId)
        {
            return Mapper.InstrumentMapper.FindByStudent(null, studentId);
        }

        /// <summary>
        /// Find all instruments that were loaned to selected student with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="studentId">The ID of the selected student.</param>
        /// <returns>
        /// List of Instrument objects.
        /// Null if no Instrument was found.
        /// </returns>
        public static List<Instrument> FindByStudent(
            MySqlTransaction trans, int studentId)
        {
            return Mapper.InstrumentMapper.FindByStudent(trans, studentId);
        }

        /// <summary>
        /// Find Instrument by id.
        /// </summary>
        /// <param name="id">The id of the selected Instrument</param>
        /// <returns>
        /// The selected Instrument.
        /// Null if selected Instrument was not found.
        /// </returns>
        public static Instrument Find(int id)
        {
            return Mapper.InstrumentMapper.Find(null, id);
        }

        /// <summary>
        /// Find Instrument by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Instrument</param>
        /// <returns>
        /// The selected Instrument.
        /// Null if selected Instrument was not found.
        /// </returns>
        public static Instrument Find(MySqlTransaction trans, int id)
        {
            return Mapper.InstrumentMapper.Find(trans, id);
        }

        /// <summary>
        /// Find Instrument by code.
        /// </summary>
        /// <param name="code">The code of the selected Instrument</param>
        /// <returns>
        /// The selected Instrument.
        /// Null if selected Instrument was not found.
        /// </returns>
        public static Instrument Find(string code)
        {
            return Mapper.InstrumentMapper.Find(null, code);
        }

        /// <summary>
        /// Find Instrument by code with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="code">The code of the selected Instrument</param>
        /// <returns>
        /// The selected Instrument.
        /// Null if selected Instrument was not found.
        /// </returns>
        public static Instrument Find(MySqlTransaction trans, string code)
        {
            return Mapper.InstrumentMapper.Find(trans, code);
        }

        /// <summary>
        /// Get description for this instrument.
        /// </summary>
        /// <returns>
        /// The created description.
        /// </returns>
        public IdDescriptionStatus GetDescription()
        {
            //create and return description.
            return new IdDescriptionStatus(
                this.instrumentId, this.ToString(), this.instrumentStatus);
        }

        /// <summary>
        /// Returns a string that represents the current instrument.
        /// </summary>
        /// <returns>
        /// A string that represents the current instrument.
        /// </returns>
        public override string ToString()
        {
            //create and return string
            return ((InstrumentsType)this.InstrumentType).ToString() + "#" + this.code;
        }

        /// <summary>
        /// Get list of comments.
        /// </summary>
        /// <returns>
        /// The list of comments.
        /// </returns>
        public List<Comment> GetComments()
        {
            try
            {
                //check comments
                if (this.comments == null ||
                    this.comments.Length == 0)
                {
                    //no comment
                    //return empty list
                    return new List<Comment>();
                }

                //create list of comments
                List<Comment> parsedComments = new List<Comment>();

                //split comments
                string[] words = this.comments.Split(
                    new string[] { Comment.COMMENT_SEPARATOR },
                    StringSplitOptions.RemoveEmptyEntries);

                //parse each comment
                foreach (string word in words)
                {
                    //parse and add comment
                    parsedComments.Add(Comment.FromString(word));
                }

                //return list of parsed comments
                return parsedComments;
            }
            catch
            {
                //error while getting comments
                //return empty list
                return new List<Logic.Comment>();
            }
        }

        /// <summary>
        /// Set comments with list of comments.
        /// </summary>
        /// <param name="comments">
        /// The list of comments.
        /// </param>
        public void SetComments(List<Comment> comments)
        {
            //check comments
            if (comments == null || comments.Count == 0)
            {
                //set no comment
                this.comments = string.Empty;

                //exit
                return;
            }

            //create string with comments
            StringBuilder sbComments = new StringBuilder(32 * comments.Count);

            //cheack each comment
            foreach (Comment comment in comments)
            {
                //add comment
                sbComments.Append(comment.ToString());

                //add separator
                sbComments.Append(Comment.COMMENT_SEPARATOR);
            }

            //remove last separator
            sbComments.Length -= Comment.COMMENT_SEPARATOR.Length;

            //set comments with result
            this.comments = sbComments.ToString();
        }

        #endregion Methods

    } //end of class Instrument

} //end of namespace PnT.SongDB.Logic
