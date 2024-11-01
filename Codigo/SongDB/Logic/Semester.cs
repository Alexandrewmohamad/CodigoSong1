using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

using PnT.SongDB.Mapper;


namespace PnT.SongDB.Logic
{

    /// <summary>
    /// Summary description for Semester.
    /// </summary>
    [DataContract]
    public class Semester
    {

        #region Fields *****************************************************************

        private int semesterId;
        private DateTime referenceDate;
        private DateTime startDate;
        private DateTime endDate;

        /// <summary>
        /// Indicates if the semester was updated.
        /// </summary>
        private bool updated = false;

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
        public Semester()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="semesterId">The id of the Semester.</param>
        public Semester(int semesterId)
        {
            this.semesterId = semesterId;
        }

        #endregion Constructors


        #region Properties *************************************************************

        public int Id
        {
            get{ return this.semesterId;}
            set{ this.semesterId = value;}
        }

        [DataMember]
        public int SemesterId
        {
            get{ return this.semesterId;}
            set{ this.semesterId = value;}
        }

        [DataMember]
        public DateTime ReferenceDate
        {
            get{ return this.referenceDate;}
            set{ this.referenceDate = value;}
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

        /// <summary>
        /// Get/set if the attendance was updated.
        /// </summary>
        public bool Updated
        {
            get
            {
                return updated;
            }

            set
            {
                updated = value;
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
        /// Get the semester description.
        /// </summary>
        public string Description
        {
            get
            {
                return this.referenceDate.Year + "." + (this.referenceDate.Month == 1 ? "I" : "II");
            }
        }

        /// <summary>
        /// Get the month of the start date.
        /// </summary>
        public DateTime StartMonth
        {
            get
            {
                //check start date
                if (startDate == DateTime.MinValue)
                {
                    //no start date
                    return DateTime.MinValue;
                }

                //return start month
                return new DateTime(startDate.Year, startDate.Month, 1);
            }
        }

        /// <summary>
        /// Get the month of the end date.
        /// </summary>
        public DateTime EndMonth
        {
            get
            {
                //check end date
                if (endDate == DateTime.MinValue)
                {
                    //no end date
                    return DateTime.MinValue;
                }

                //return end month
                return new DateTime(endDate.Year, endDate.Month, 1);
            }
        }

        #endregion Properties


        #region Methods ****************************************************************

        /// <summary>
        /// Save Semester to database.
        /// </summary>
        /// <returns>The id of the saved Semester.</returns>
        public int Save()
        {
            semesterId = Mapper.SemesterMapper.Save(null, this);
            return semesterId;
        }

        /// <summary>
        /// Save Semester to database with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>The id of the saved Semester.</returns>
        public int Save(MySqlTransaction trans)
        {
            semesterId = Mapper.SemesterMapper.Save(trans, this);
            return semesterId;
        }

        /// <summary>
        /// Delete Semester by id.
        /// </summary>
        /// <param name="id">The id of the selected Semester.</param>
        /// <returns>
        /// True if selected Semester was deleted.
        /// False if selected Semester was not found.
        /// </returns>
        public static bool Delete(int id)
        {
            return Mapper.SemesterMapper.Delete(null, id);
        }

        /// <summary>
        /// Delete Semester by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Semester.</param>
        /// <returns>
        /// True if selected Semester was deleted.
        /// False if selected Semester was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Mapper.SemesterMapper.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Semester by id.
        /// </summary>
        /// <param name="id">The id of the selected Semester.</param>
        /// <returns>
        /// True if selected Semester was inactivated.
        /// False if selected Semester was not found.
        /// </returns>
        public static bool Inactivate(int id)
        {
            return Mapper.SemesterMapper.Inactivate(null, id);
        }

        /// <summary>
        /// Inactivate Semester by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Semester.</param>
        /// <returns>
        /// True if selected Semester was inactivated.
        /// False if selected Semester was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Mapper.SemesterMapper.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Semester.
        /// </summary>
        /// <returns>
        /// List of Semester objects.
        /// Null if no Semester was found.
        /// </returns>
        public static List<Semester> Find()
        {
            return Mapper.SemesterMapper.Find(null);
        }

        /// <summary>
        /// Find all Semester with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <returns>
        /// List of Semester objects.
        /// Null if no Semester was found.
        /// </returns>
        public static List<Semester> Find(MySqlTransaction trans)
        {
            return Mapper.SemesterMapper.Find(trans);
        }

        /// <summary>
        /// Find Semester by id.
        /// </summary>
        /// <param name="id">The id of the selected Semester</param>
        /// <returns>
        /// The selected Semester.
        /// Null if selected Semester was not found.
        /// </returns>
        public static Semester Find(int id)
        {
            return Mapper.SemesterMapper.Find(null, id);
        }

        /// <summary>
        /// Find Semester by id with transaction.
        /// </summary>
        /// <param name="trans">The transaction to be used.</param>
        /// <param name="id">The id of the selected Semester</param>
        /// <returns>
        /// The selected Semester.
        /// Null if selected Semester was not found.
        /// </returns>
        public static Semester Find(MySqlTransaction trans, int id)
        {
            return Mapper.SemesterMapper.Find(trans, id);
        }

        /// <summary>
        /// Get description for this semester.
        /// </summary>
        /// <returns>
        /// The created description.
        /// </returns>
        public IdDescriptionStatus GetDescription()
        {
            //create and return description.
            return new IdDescriptionStatus(
                this.semesterId, this.Description, (int)ItemStatus.Active);
        }

        #endregion Methods

    } //end of class Semester

} //end of namespace PnT.SongDB.Logic
