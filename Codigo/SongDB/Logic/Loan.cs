using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for Loan.
    /// </summary>
    [DataContract]
    public class Loan
    {

        #region Constants **************************************************************

        /// <summary>
        /// The time the user has to edit or delete a loan. In days.
        /// </summary>
        public const int EDITION_THRESHOLD = 90;

        #endregion Constants


        #region Fields *****************************************************************

        private int loanId;
        private int instrumentId;
        private int studentId;
        private DateTime startDate;
        private DateTime endDate;
        private string comments;
        private int loanStatus;
        private DateTime creationTime;

        /// <summary>
        /// The code of the instrument.
        /// </summary>
        private string instrumentCode;

        /// <summary>
        /// The name of the student.
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
        public Loan()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="loanId">The id of the Loan.</param>
        public Loan(int loanId)
        {
            this.loanId = loanId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.loanId;}
            set{ this.loanId = value;}
        }

        [DataMember]
        public int LoanId
        {
            get{ return this.loanId;}
            set{ this.loanId = value;}
        }

        [DataMember]
        public int InstrumentId
        {
            get{ return this.instrumentId;}
            set{ this.instrumentId = value;}
        }

        [DataMember]
        public int StudentId
        {
            get{ return this.studentId;}
            set{ this.studentId = value;}
        }

        [DataMember]
        public DateTime StartDate
        {
            get{ return this.startDate;}
            set{ this.startDate = value;}
        }

        [DataMember]
        public DateTime EndDate
        {
            get{ return this.endDate;}
            set{ this.endDate = value;}
        }

        [DataMember]
        public string Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        [DataMember]
        public int LoanStatus
        {
            get{ return this.loanStatus;}
            set{ this.loanStatus = value;}
        }

        [DataMember]
        public DateTime CreationTime
        {
            get { return this.creationTime; }
            set { this.creationTime = value; }
        }

        /// <summary>
        /// Get/set the code of the instrument.
        /// </summary>
        [DataMember]
        public string InstrumentCode
        {
            get
            {
                return instrumentCode;
            }

            set
            {
                instrumentCode = value;
            }
        }

        /// <summary>
        /// Get/set the name of the student.
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
        /// Save Loan to database.
        /// </summary>
        /// <returns>The id of the saved Loan.</returns>
        public int Save()
        {
            loanId = Mapper.LoanMapper.Save(null, this);
            return loanId;
        }

        /// <summary>
        /// Save Loan to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Loan.</returns>
        public int Save(MySqlTransaction trans)
        {
            loanId = Mapper.LoanMapper.Save(trans, this);
            return loanId;
        }

        /// <summary>
        /// Delete Loan by id.
        /// </summary>
        /// <param name="id">The id of the selected Loan.</param>
        /// <returns>
        /// True if selected Loan was deleted.
        /// False if selected Loan was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.LoanMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Loan by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Loan.</param>
        /// <returns>
        /// True if selected Loan was deleted.
        /// False if selected Loan was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.LoanMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Loan by id.
        /// </summary>
        /// <param name="id">The id of the selected Loan.</param>
        /// <returns>
        /// True if selected Loan was inactivated.
        /// False if selected Loan was not found.
        /// </returns>
        public static bool Inactivate(int id)
        {
            return Mapper.LoanMapper.Inactivate(null, id);
        }

        /// <summary>
        /// Inactivate Loan by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Loan.</param>
        /// <returns>
        /// True if selected Loan was inactivated.
        /// False if selected Loan was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Mapper.LoanMapper.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Loan.
        /// </summary>
        /// <returns>
        /// List of Loan objects.
        /// Null if no Loan was found.
        /// </returns>
        public static List<Loan> Find()
        {
            return Mapper.LoanMapper.Find(null);
        }

        /// <summary>
        /// Find all Loan with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Loan objects.
        /// Null if no Loan was found.
        /// </returns>
        public static List<Loan> Find(MySqlTransaction trans)
        {
            return Mapper.LoanMapper.Find(trans);
        }

        /// <summary>
        /// Count loans by filter.
        /// </summary>
        /// <param name="filterLoanStatus">
        /// The loan status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstrument">
        /// The instrument filter.
        /// -1 to select all instruments.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <param name="filterStartDate">
        /// The start date filter.
        /// DateTime.MinValue to select all start dates.
        /// </param>
        /// <param name="filterEndDate">
        /// The end date filter.
        /// DateTime.MinValue to select all end dates.
        /// </param>
        /// <returns>
        /// The number of loans.
        /// </returns>
        public static int CountByFilter(
            int filterLoanStatus, int filterInstrument, int filterStudent,
            DateTime filterStartDate, DateTime filterEndDate)
        {
            return Mapper.LoanMapper.CountByFilter(
                null, filterLoanStatus, filterInstrument, 
                filterStudent, filterStartDate, filterEndDate);
        }

        /// <summary>
        /// Find all Loan for selected Instrument.
        /// </summary>
        /// <param name="instrumentId">The id of the selected instrument.</param>
        /// <param name="status">
        /// The status of the returned loans.
        /// -1 to return all loans.
        /// </param>
        /// <returns>
        /// List of Loan objects.
        /// Null if no Loan was found.
        /// </returns>
        public static List<Loan> FindByInstrument(int instrumentId, int status)
        {
            return Mapper.LoanMapper.FindByInstrument(null, instrumentId, status);
        }

        /// <summary>
        /// Find all Loan for selected Instrument with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="instrumentId">The id of the selected instrument.</param>
        /// <param name="status">
        /// The status of the returned loans.
        /// -1 to return all loans.
        /// </param>
        /// <returns>
        /// List of Loan objects.
        /// Null if no Loan was found.
        /// </returns>
        public static List<Loan> FindByInstrument(MySqlTransaction trans, int instrumentId, int status)
        {
            return Mapper.LoanMapper.FindByInstrument(trans, instrumentId, status);
        }

        /// <summary>
        /// Find all Loan for selected Pole.
        /// </summary>
        /// <param name="poleId">The id of the selected pole.</param>
        /// <param name="status">
        /// The status of the returned loans.
        /// -1 to return all loans.
        /// </param>
        /// <returns>
        /// List of Loan objects.
        /// Null if no Loan was found.
        /// </returns>
        public static List<Loan> FindByPole(int poleId, int status)
        {
            return Mapper.LoanMapper.FindByPole(null, poleId, status);
        }

        /// <summary>
        /// Find all Loan for selected Pole with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="poleId">The id of the selected pole.</param>
        /// <param name="status">
        /// The status of the returned loans.
        /// -1 to return all loans.
        /// </param>
        /// <returns>
        /// List of Loan objects.
        /// Null if no Loan was found.
        /// </returns>
        public static List<Loan> FindByPole(MySqlTransaction trans, int poleId, int status)
        {
            return Mapper.LoanMapper.FindByPole(trans, poleId, status);
        }

        /// <summary>
        /// Find all Loan for selected status.
        /// </summary>
        /// <param name="status">
        /// The selected loan status.
        /// </param>
        /// <returns>
        /// List of Loan objects.
        /// Null if no Loan was found.
        /// </returns>
        public static List<Loan> FindByStatus(int status)
        {
            return Mapper.LoanMapper.FindByStatus(null, status);
        }

        /// <summary>
        /// Find all Loan for selected status with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="status">
        /// The status of the returned loans.
        /// </param>
        /// <returns>
        /// List of Loan objects.
        /// Null if no Loan was found.
        /// </returns>
        public static List<Loan> FindByStatus(MySqlTransaction trans, int status)
        {
            return Mapper.LoanMapper.FindByStatus(trans, status);
        }

        /// <summary>
        /// Find all Loan for selected Student.
        /// </summary>
        /// <param name="studentId">The id of the selected student.</param>
        /// <param name="status">
        /// The status of the returned loans.
        /// -1 to return all loans.
        /// </param>
        /// <returns>
        /// List of Loan objects.
        /// Null if no Loan was found.
        /// </returns>
        public static List<Loan> FindByStudent(int studentId, int status)
        {
            return Mapper.LoanMapper.FindByStudent(null, studentId, status);
        }

        /// <summary>
        /// Find all Loan for selected Student with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="studentId">The id of the selected student.</param>
        /// <param name="status">
        /// The status of the returned loans.
        /// -1 to return all loans.
        /// </param>
        /// <returns>
        /// List of Loan objects.
        /// Null if no Loan was found.
        /// </returns>
        public static List<Loan> FindByStudent(MySqlTransaction trans, int studentId, int status)
        {
            return Mapper.LoanMapper.FindByStudent(trans, studentId, status);
        }

        /// <summary>
        /// Find Loan by id.
        /// </summary>
        /// <param name="id">The id of the selected Loan.</param>
        /// <returns>
        /// The selected Loan.
        /// Null if selected Loan was not found.
        /// </returns>
        public static Loan Find(int id)
        {
            return Mapper.LoanMapper.Find(null, id);
        }

        /// <summary>
        /// Find Loan by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Loan.</param>
        /// <returns>
        /// The selected Loan.
        /// Null if selected Loan was not found.
        /// </returns>
        public static Loan Find(MySqlTransaction trans, int id)
        {
            return Mapper.LoanMapper.Find(trans, id);
        }

        #endregion Methods

    } //end of class Loan

} //end of namespace PnT.SongDB.Logic
