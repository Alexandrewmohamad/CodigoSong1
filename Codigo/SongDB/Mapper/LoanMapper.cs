using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for LoanMapper.
    /// </summary>
    public class LoanMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Loan to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Loan.</returns>
        public static int Save(MySqlTransaction trans, Loan loan)
        {
            return Access.LoanAccess.Save(trans, GetParameters(loan));
        }

        /// <summary>
        /// Delete Loan by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Loan.</param>
        /// <returns>
        /// True if selected Loan was deleted.
        /// False if selected Loan was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.LoanAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Loan by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Loan.</param>
        /// <returns>
        /// True if selected Loan was inactivated.
        /// False if selected Loan was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Access.LoanAccess.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Loan.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Loan objects.
        /// Null if no Loan was found.
        /// </returns>
        public static List<Loan> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.LoanAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Count loans by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
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
            MySqlTransaction trans, int filterLoanStatus, int filterInstrument, 
            int filterStudent, DateTime filterStartDate, DateTime filterEndDate)
        {
            return Access.LoanAccess.CountByFilter(
                trans, filterLoanStatus, filterInstrument,
                filterStudent, filterStartDate, filterEndDate);
        }

        /// <summary>
        /// Find all Loan for selected Instrument.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
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
            DataRow[] dr = Access.LoanAccess.FindByInstrument(trans, instrumentId, status);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all Loan for selected Pole.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
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
            DataRow[] dr = Access.LoanAccess.FindByPole(trans, poleId, status);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all Loan for selected status.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="status">
        /// The selected loan status.
        /// </param>
        /// <returns>
        /// List of Loan objects.
        /// Null if no Loan was found.
        /// </returns>
        public static List<Loan> FindByStatus(MySqlTransaction trans, int status)
        {
            DataRow[] dr = Access.LoanAccess.FindByStatus(trans, status);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all Loan for selected Student.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
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
            DataRow[] dr = Access.LoanAccess.FindByStudent(trans, studentId, status);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Loan by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Loan.
        /// Null if selected Loan was not found.
        /// </returns>
        public static Loan Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.LoanAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Loan objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Loan objects.</returns>
        private static List<Loan> Map(DataRow[] rows)
        {
            List<Loan> loans = new List<Loan>();

            for (int i = 0; i < rows.Length; i++)
                loans.Add(Map(rows[i]));

            return loans;
        }

        /// <summary>
        /// Map database row to a Loan object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Loan</returns>
        private static Loan Map(DataRow row)
        {
            Loan loan = new Loan((int)(row["loanId"]));
            loan.InstrumentId = (int)DataAccessCommon.HandleDBNull(row, "instrumentId", typeof(int));
            loan.StudentId = (int)DataAccessCommon.HandleDBNull(row, "studentId", typeof(int));
            loan.StartDate = (DateTime)DataAccessCommon.HandleDBNull(row, "startDate", typeof(DateTime));
            loan.EndDate = (DateTime)DataAccessCommon.HandleDBNull(row, "endDate", typeof(DateTime));
            loan.Comments = (string)DataAccessCommon.HandleDBNull(row, "comments", typeof(string));
            loan.LoanStatus = (int)DataAccessCommon.HandleDBNull(row, "loanStatus", typeof(int));
            loan.CreationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "creationTime", typeof(DateTime));

            return loan;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Loan
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Loan loan)
        {
            MySqlParameter[] parameters = new MySqlParameter[8];
            parameters[0] = new MySqlParameter("loanId", loan.Id);
            parameters[1] = new MySqlParameter("instrumentId", loan.InstrumentId);
            parameters[2] = new MySqlParameter("studentId", loan.StudentId);
            parameters[3] = new MySqlParameter("startDate", loan.StartDate);
            parameters[4] = new MySqlParameter("endDate", DataAccessCommon.HandleDBNull(loan.EndDate));
            parameters[5] = new MySqlParameter("comments", DataAccessCommon.HandleDBNull(loan.Comments));
            parameters[6] = new MySqlParameter("loanStatus", loan.LoanStatus);
            parameters[7] = new MySqlParameter("creationTime", loan.Id <= -1 ? DateTime.Now : loan.CreationTime);

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class LoanMapper

} //end of namespace PnT.SongDB.Mapper
