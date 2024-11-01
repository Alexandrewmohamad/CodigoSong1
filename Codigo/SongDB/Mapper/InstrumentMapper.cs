using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for InstrumentMapper.
    /// </summary>
    public class InstrumentMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Instrument to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Instrument.</returns>
        public static int Save(MySqlTransaction trans, Instrument instrument)
        {
            return Access.InstrumentAccess.Save(trans, GetParameters(instrument));
        }

        /// <summary>
        /// Delete Instrument by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Instrument.</param>
        /// <returns>
        /// True if selected Instrument was deleted.
        /// False if selected Instrument was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.InstrumentAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Instrument by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
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
            return Access.InstrumentAccess.Inactivate(trans, id, inactivationReason);
        }

        /// <summary>
        /// Find all Instrument.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Instrument objects.
        /// Null if no Instrument was found.
        /// </returns>
        public static List<Instrument> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.InstrumentAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Instrument by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Instrument.
        /// Null if selected Instrument was not found.
        /// </returns>
        public static Instrument Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.InstrumentAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Instrument by code.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Instrument.
        /// Null if selected Instrument was not found.
        /// </returns>
        public static Instrument Find(MySqlTransaction trans, string code)
        {
            DataRow dr = Access.InstrumentAccess.Find(trans, code);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Count instruments by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterInstrumentStatus">
        /// The instrument status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <returns>
        /// The number of instruments.
        /// </returns>
        public static int CountByFilter(
            MySqlTransaction trans, int filterInstrumentStatus,
            int filterInstrumentType, int filterInstitution, int filterPole)
        {
            return Access.InstrumentAccess.CountByFilter(
                trans, filterInstrumentStatus, filterInstrumentType, filterInstitution, filterPole);
        }

        /// <summary>
        /// Find instruments by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterInstrumentStatus">
        /// The instrument status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <returns>
        /// List of Instrument objects.
        /// Null if no Instrument was found.
        /// </returns>
        public static List<Instrument> FindByFilter(
            MySqlTransaction trans, int filterInstrumentStatus, 
            int filterInstrumentType, int filterInstitution, int filterPole)
        {
            DataRow[] dr = Access.InstrumentAccess.FindByFilter(
                trans, filterInstrumentStatus, filterInstrumentType, filterInstitution, filterPole);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all instruments that were loaned to selected student.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="studentId">
        /// The ID of the selected student.
        /// </param>
        /// <returns>
        /// List of Instrument objects.
        /// Null if no Instrument was found.
        /// </returns>
        public static List<Instrument> FindByStudent(
            MySqlTransaction trans, int studentId)
        {
            DataRow[] dr = Access.InstrumentAccess.FindByStudent(trans, studentId);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Instrument objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Instrument objects.</returns>
        private static List<Instrument> Map(DataRow[] rows)
        {
            List<Instrument> instruments = new List<Instrument>();

            for (int i = 0; i < rows.Length; i++)
                instruments.Add(Map(rows[i]));

            return instruments;
        }

        /// <summary>
        /// Map database row to a Instrument object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Instrument</returns>
        private static Instrument Map(DataRow row)
        {
            Instrument instrument = new Instrument((int)(row["instrumentId"]));
            instrument.PoleId = (int)DataAccessCommon.HandleDBNull(row, "poleId", typeof(int));
            instrument.Code = (string)DataAccessCommon.HandleDBNull(row, "code", typeof(string));
            instrument.Model = (string)DataAccessCommon.HandleDBNull(row, "model", typeof(string));
            instrument.InstrumentType = (int)DataAccessCommon.HandleDBNull(row, "instrumentType", typeof(int));
            instrument.StorageLocation = (string)DataAccessCommon.HandleDBNull(row, "storageLocation", typeof(string));
            instrument.Comments = (string)DataAccessCommon.HandleDBNull(row, "comments", typeof(string));
            instrument.InstrumentStatus = (int)DataAccessCommon.HandleDBNull(row, "instrumentStatus", typeof(int));
            instrument.CreationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "creationTime", typeof(DateTime));
            instrument.InactivationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "inactivationTime", typeof(DateTime));
            instrument.InactivationReason = (string)DataAccessCommon.HandleDBNull(row, "inactivationReason", typeof(string));

            return instrument;
        }

        #endregion Mapper Methods

        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Instrument
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Instrument instrument)
        {
            MySqlParameter[] parameters = new MySqlParameter[11];
            parameters[0] = new MySqlParameter("instrumentId", instrument.InstrumentId);
            parameters[1] = new MySqlParameter("poleId", instrument.PoleId);
            parameters[2] = new MySqlParameter("code", instrument.Code);
            parameters[3] = new MySqlParameter("model", instrument.Model);
            parameters[4] = new MySqlParameter("instrumentType", instrument.InstrumentType);
            parameters[5] = new MySqlParameter("storageLocation", DataAccessCommon.HandleDBNull(instrument.StorageLocation));
            parameters[6] = new MySqlParameter("comments", DataAccessCommon.HandleDBNull(instrument.Comments));
            parameters[7] = new MySqlParameter("instrumentStatus", instrument.InstrumentStatus);
            parameters[8] = new MySqlParameter("creationTime", instrument.Id == -1 ? DateTime.Now : instrument.CreationTime);
            parameters[9] = new MySqlParameter("inactivationTime", DataAccessCommon.HandleDBNull(instrument.InactivationTime));
            parameters[10] = new MySqlParameter("inactivationReason", DataAccessCommon.HandleDBNull(instrument.InactivationReason));

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class InstrumentMapper

} //end of namespace PnT.SongDB.Mapper
