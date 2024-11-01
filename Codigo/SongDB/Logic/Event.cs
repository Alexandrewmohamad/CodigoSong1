using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Enumerates the possible event send options.
    /// </summary>
    public enum EventSendOption
    {
        ToNoOne = 0,
        ToEveryone,
        ToInstitution,
        ToCoordinators,
        ToTeachers
    }


    /// <summary>
    /// Summary description for Event.
    /// </summary>
    [DataContract]
    public class Event
    {

        #region Fields *****************************************************************

        private int eventId;
        private int institutionId;
        private DateTime startTime;
        private int duration;
        private string location;
        private int eventSendOption;
        private string name;
        private string description;
        private DateTime creationTime;

        /// <summary>
        /// The name of the institution.
        /// </summary>
        private string institutionName;

        /// <summary>
        /// True to mark sent event emails as new.
        /// False to mark as updated.
        /// </summary>
        private bool markEmailAsNew;

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
        public Event()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="eventId">The id of the Event.</param>
        public Event(int eventId)
        {
            this.eventId = eventId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get { return this.eventId; }
            set { this.eventId = value; }
        }

        [DataMember]
        public int EventId
        {
            get { return this.eventId; }
            set { this.eventId = value; }
        }

        [DataMember]
        public int InstitutionId
        {
            get { return this.institutionId; }
            set { this.institutionId = value; }
        }

        [DataMember]
        public DateTime StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }

        [DataMember]
        public int Duration
        {
            get { return this.duration; }
            set { this.duration = value; }
        }

        [DataMember]
        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        [DataMember]
        public int EventSendOption
        {
            get { return this.eventSendOption; }
            set { this.eventSendOption = value; }
        }

        [DataMember]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [DataMember]
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        [DataMember]
        public DateTime CreationTime
        {
            get { return this.creationTime; }
            set { this.creationTime = value; }
        }

        /// <summary>
        /// Get/set the name of the institution.
        /// </summary>
        [DataMember]
        public string InstitutionName
        {
            get
            {
                return institutionName;
            }

            set
            {
                institutionName = value;
            }
        }

        /// <summary>
        /// Get/set option to mark sent event emails as new.
        /// True to mark sent event emails as new.
        /// False to mark as updated.
        /// </summary>
        public bool MarkEmailAsNew
        {
            get
            {
                return markEmailAsNew;
            }

            set
            {
                markEmailAsNew = value;
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
        /// Save Event to database.
        /// </summary>
        /// <returns>The id of the saved Event.</returns>
        public int Save()
        {
            eventId = Mapper.EventMapper.Save(null, this);
            return eventId;
        }

        /// <summary>
        /// Save Event to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Event.</returns>
        public int Save(MySqlTransaction trans)
        {
            eventId = Mapper.EventMapper.Save(trans, this);
            return eventId;
        }

        /// <summary>
        /// Delete Event by id.
        /// </summary>
        /// <param name="id">The id of the selected Event.</param>
        /// <returns>
        /// True if selected Event was deleted.
        /// False if selected Event was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.EventMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Event by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Event.</param>
        /// <returns>
        /// True if selected Event was deleted.
        /// False if selected Event was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.EventMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Event by id.
        /// </summary>
        /// <param name="id">The id of the selected Event.</param>
        /// <returns>
        /// True if selected Event was inactivated.
        /// False if selected Event was not found.
        /// </returns>
        public static bool Inactivate(int id)
        {
            return Mapper.EventMapper.Inactivate(null, id);
        }

        /// <summary>
        /// Inactivate Event by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Event.</param>
        /// <returns>
        /// True if selected Event was inactivated.
        /// False if selected Event was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Mapper.EventMapper.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Event.
        /// </summary>
        /// <returns>
        /// List of Event objects.
        /// Null if no Event was found.
        /// </returns>
        public static List<Event> Find()
        {
            return Mapper.EventMapper.Find(null);
        }

        /// <summary>
        /// Find all Event with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Event objects.
        /// Null if no Event was found.
        /// </returns>
        public static List<Event> Find(MySqlTransaction trans)
        {
            return Mapper.EventMapper.Find(trans);
        }

        /// <summary>
        /// Count events by filter.
        /// </summary>
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
            int filterInstitution, DateTime filterStartDate, DateTime filterEndDate)
        {
            return Mapper.EventMapper.CountByFilter(
                null, filterInstitution, filterStartDate, filterEndDate);
        }

        /// <summary>
        /// Find events by filter.
        /// </summary>
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
            int filterInstitution, DateTime filterStartDate, DateTime filterEndDate)
        {
            return Mapper.EventMapper.FindByFilter(
                null, filterInstitution, filterStartDate, filterEndDate);
        }

        /// <summary>
        /// Find events by filter with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
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
            return Mapper.EventMapper.FindByFilter(
                trans, filterInstitution, filterStartDate, filterEndDate);
        }

        /// <summary>
        /// Find Event by id.
        /// </summary>
        /// <param name="id">The id of the selected Event</param>
        /// <returns>
        /// The selected Event.
        /// Null if selected Event was not found.
        /// </returns>
        public static Event Find(int id)
        {
            return Mapper.EventMapper.Find(null, id);
        }

        /// <summary>
        /// Find Event by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Event</param>
        /// <returns>
        /// The selected Event.
        /// Null if selected Event was not found.
        /// </returns>
        public static Event Find(MySqlTransaction trans, int id)
        {
            return Mapper.EventMapper.Find(trans, id);
        }

        /// <summary>
        /// Get description for this institution.
        /// </summary>
        /// <returns>
        /// The created description.
        /// </returns>
        public IdDescriptionStatus GetDescription()
        {
            //create and return description
            return new IdDescriptionStatus(
                this.eventId, 
                this.startTime.ToString("yyyy.MM.dd ") + this.Name, 
                (int)ItemStatus.Active);
        }

        #endregion Methods

    } //end of class Event

} //end of namespace PnT.SongDB.Logic
