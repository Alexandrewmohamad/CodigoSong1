using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for EventMapper.
    /// </summary>
    public class EventMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Event to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Event.</returns>
        public static int Save(MySqlTransaction trans, Event eventObj)
        {
            return Access.EventAccess.Save(trans, GetParameters(eventObj));
        }

        /// <summary>
        /// Delete Event by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Event.</param>
        /// <returns>
        /// True if selected Event was deleted.
        /// False if selected Event was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.EventAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Event by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Event.</param>
        /// <returns>
        /// True if selected Event was inactivated.
        /// False if selected Event was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Access.EventAccess.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Event.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Event objects.
        /// Null if no Event was found.
        /// </returns>
        public static List<Event> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.EventAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Event by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Event.
        /// Null if selected Event was not found.
        /// </returns>
        public static Event Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.EventAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Count events by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterStartDate">
        /// The start date filter. The start time must be later than selected date.
        /// DateTime.MinValue to set no start date.
        /// </param>
        /// <param name="filterEndDate">
        /// The end date filter. The start time must be sooner than selected date.
        /// DateTime.MinValue to set no end date.
        /// </param>
        /// <returns>
        /// The number of events.
        /// </returns>
        public static int CountByFilter(
            MySqlTransaction trans, int filterInstitution,
            DateTime filterStartDate, DateTime filterEndDate)
        {
            return Access.EventAccess.CountByFilter(
                trans, filterInstitution, filterStartDate, filterEndDate);
        }

        /// <summary>
        /// Find events by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterStartDate">
        /// The start date filter. The start time must be later than selected date.
        /// DateTime.MinValue to set no start date.
        /// </param>
        /// <param name="filterEndDate">
        /// The end date filter. The start time must be sooner than selected date.
        /// DateTime.MinValue to set no end date.
        /// </param>
        /// <returns>
        /// List of Event objects.
        /// Null if no Event was found.
        /// </returns>
        public static List<Event> FindByFilter(
            MySqlTransaction trans, int filterInstitution, 
            DateTime filterStartDate, DateTime filterEndDate)
        {
            DataRow[] dr = Access.EventAccess.FindByFilter(
                trans, filterInstitution, filterStartDate, filterEndDate);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Event objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Event objects.</returns>
        private static List<Event> Map(DataRow[] rows)
        {
            List<Event> events = new List<Event>();

            for (int i = 0; i < rows.Length; i++)
                events.Add(Map(rows[i]));

            return events;
        }

        /// <summary>
        /// Map database row to a Event object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Event</returns>
        private static Event Map(DataRow row)
        {
            Event eventObj = new Event((int)(row["eventId"]));
            eventObj.InstitutionId = (int)DataAccessCommon.HandleDBNull(row,"institutionId", typeof(int));
            eventObj.StartTime = (DateTime)DataAccessCommon.HandleDBNull(row,"startTime", typeof(DateTime));
            eventObj.Duration = (int)DataAccessCommon.HandleDBNull(row,"duration", typeof(int));
            eventObj.Location = (string)DataAccessCommon.HandleDBNull(row,"location", typeof(string));
            eventObj.EventSendOption = (int)DataAccessCommon.HandleDBNull(row,"eventSendOption", typeof(int));
            eventObj.Name = (string)DataAccessCommon.HandleDBNull(row,"name", typeof(string));
            eventObj.Description = (string)DataAccessCommon.HandleDBNull(row,"description", typeof(string));
            eventObj.CreationTime = (DateTime)DataAccessCommon.HandleDBNull(row,"creationTime", typeof(DateTime));

            return eventObj;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Event
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Event eventObj)
        {
            MySqlParameter[] parameters = new MySqlParameter[9];
            parameters[0] = new MySqlParameter("eventId", eventObj.Id);
            parameters[1] = new MySqlParameter("institutionId", DataAccessCommon.HandleDBNull(eventObj.InstitutionId));
            parameters[2] = new MySqlParameter("startTime", eventObj.StartTime);
            parameters[3] = new MySqlParameter("duration", eventObj.Duration);
            parameters[4] = new MySqlParameter("location", DataAccessCommon.HandleDBNull(eventObj.Location));
            parameters[5] = new MySqlParameter("eventSendOption", eventObj.EventSendOption);
            parameters[6] = new MySqlParameter("name", eventObj.Name);
            parameters[7] = new MySqlParameter("description", DataAccessCommon.HandleDBNull(eventObj.Description));
            parameters[8] = new MySqlParameter("creationTime", eventObj.Id == -1 ? DateTime.Now : eventObj.CreationTime);

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class EventMapper

} //end of namespace PnT.SongDB.Mapper
