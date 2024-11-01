using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;


namespace PnT.SongDB.Mapper
{

    /// <summary>
    /// Summary description for RegistrationMapper.
    /// </summary>
    public class RegistrationMapper
    {

        #region Methods ****************************************************************

        /// <summary>
        /// Save Registration to database.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>The id of the saved Registration.</returns>
        public static int Save(MySqlTransaction trans, Registration registration)
        {
            return Access.RegistrationAccess.Save(trans, GetParameters(registration));
        }

        /// <summary>
        /// Delete Registration by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Registration.</param>
        /// <returns>
        /// True if selected Registration was deleted.
        /// False if selected Registration was not found.
        /// </returns>
        public static bool Delete(MySqlTransaction trans, int id)
        {
            return Access.RegistrationAccess.Delete(trans, id);
        }

        /// <summary>
        /// Inactivate Registration by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="id">The id of the selected Registration.</param>
        /// <returns>
        /// True if selected Registration was inactivated.
        /// False if selected Registration was not found.
        /// </returns>
        public static bool Inactivate(MySqlTransaction trans, int id)
        {
            return Access.RegistrationAccess.Inactivate(trans, id);
        }

        /// <summary>
        /// Find all Registration.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// List of Registration objects.
        /// Null if no Registration was found.
        /// </returns>
        public static List<Registration> Find(MySqlTransaction trans)
        {
            DataRow[] dr = Access.RegistrationAccess.Find(trans);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Count registrations for selected class.
        /// </summary>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <param name="status">
        /// The status of the selected registrations.
        /// -1 to count all registrations.
        /// </param>
        /// <param name="maxPosition">
        /// The maximum registration position to consider.
        /// -1 to count all registrations.
        /// </param>
        /// <returns>
        /// The number of registrations.
        /// </returns>
        public static int CountByClass(
            MySqlTransaction trans, int classId, int status, int maxPosition)
        {
            return Access.RegistrationAccess.CountByClass(trans, classId, status, maxPosition);
        }

        /// <summary>
        /// Find all Registration for selected Class.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="classId">The id of the selected class.</param>
        /// <param name="status">
        /// The status of the returned registrations.
        /// -1 to return all registrations.
        /// </param>
        /// <returns>
        /// List of Registration objects.
        /// Null if no Registration was found.
        /// </returns>
        public static List<Registration> FindByClass(MySqlTransaction trans, int classId, int status)
        {
            DataRow[] dr = Access.RegistrationAccess.FindByClass(trans, classId, status);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Count registrations by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterSemester">
        /// The semester filter.
        /// -1 to select all semesters.
        /// </param>
        /// <param name="filterReferenceDate">
        /// The reference date filter.
        /// DateTime.MinValue to selct all dates.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <returns>
        /// The number of registrations.
        /// </returns>
        public static int CountEvationsByFilter(
            MySqlTransaction trans, int filterSemester, 
            DateTime filterReferenceDate, int filterInstitution, int filterTeacher)
        {
            return Access.RegistrationAccess.CountEvationsByFilter(
                trans, filterSemester, filterReferenceDate, filterInstitution, filterTeacher);
        }

        /// <summary>
        /// Count registrations by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterRegistrationStatus">
        /// The registration status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterSemester">
        /// The semester filter.
        /// -1 to select all semesters.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// The number of registrations.
        /// </returns>
        public static int CountByFilter(
            MySqlTransaction trans, int filterRegistrationStatus, int filterSemester,
            int filterInstitution, int filterPole, int filterTeacher, int filterClass)
        {
            return Access.RegistrationAccess.CountByFilter(
                trans, filterRegistrationStatus, filterSemester,
                filterInstitution, filterPole, filterTeacher, filterClass);
        }

        /// <summary>
        /// Find registrations by filter.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="filterRegistrationStatus">
        /// The registration status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterSemester">
        /// The semester filter.
        /// -1 to select all semesters.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// List of Registration objects.
        /// Null if no Registration was found.
        /// </returns>
        public static List<Registration> FindByFilter(
            MySqlTransaction trans, int filterRegistrationStatus, int filterSemester,
            int filterInstitution, int filterPole, int filterTeacher, int filterClass)
        {
            DataRow[] dr = Access.RegistrationAccess.FindByFilter(
                trans, filterRegistrationStatus, filterSemester,
                filterInstitution, filterPole, filterTeacher, filterClass);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find all Registration for selected Student.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <param name="studentId">The id of the selected student.</param>
        /// <param name="status">
        /// The status of the returned registrations.
        /// -1 to return all registrations.
        /// </param>
        /// <returns>
        /// List of Registration objects.
        /// Null if no Registration was found.
        /// </returns>
        public static List<Registration> FindByStudent(MySqlTransaction trans, int studentId, int status)
        {
            DataRow[] dr = Access.RegistrationAccess.FindByStudent(trans, studentId, status);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        /// <summary>
        /// Find Registration by id.
        /// </summary>
        /// <param name="trans">
        /// The transaction to be used.
        /// Null if there is no specific transaction.
        /// </param>
        /// <returns>
        /// The selected Registration.
        /// Null if selected Registration was not found.
        /// </returns>
        public static Registration Find(MySqlTransaction trans, int id)
        {
            DataRow dr = Access.RegistrationAccess.Find(trans, id);

            if (dr != null)
                return Map(dr);
            else
                return null;
        }

        #endregion Methods


        #region Mapper Methods *********************************************************

        /// <summary>
        /// Map database rows to a list of Registration objects.
        /// </summary>
        /// <param name="rows">Database selected rows.</param>
        /// <returns>A list of Registration objects.</returns>
        private static List<Registration> Map(DataRow[] rows)
        {
            List<Registration> registrations = new List<Registration>();

            for (int i = 0; i < rows.Length; i++)
                registrations.Add(Map(rows[i]));

            return registrations;
        }

        /// <summary>
        /// Map database row to a Registration object.
        /// </summary>
        /// <param name="row">Database selected row.</param>
        /// <returns>Registration</returns>
        private static Registration Map(DataRow row)
        {
            Registration registration = new Registration((int)(row["registrationId"]));
            registration.StudentId = (int)DataAccessCommon.HandleDBNull(row, "studentId", typeof(int));
            registration.ClassId = (int)DataAccessCommon.HandleDBNull(row, "classId", typeof(int));
            registration.Position = (int)DataAccessCommon.HandleDBNull(row, "position", typeof(int));
            registration.AutoRenewal = (bool)DataAccessCommon.HandleDBNull(row, "autoRenewal", typeof(bool));
            registration.RegistrationStatus = (int)DataAccessCommon.HandleDBNull(row, "registrationStatus", typeof(int));
            registration.CreationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "creationTime", typeof(DateTime));
            registration.InactivationTime = (DateTime)DataAccessCommon.HandleDBNull(row, "inactivationTime", typeof(DateTime));
            registration.InactivationReason = (string)DataAccessCommon.HandleDBNull(row, "inactivationReason", typeof(string));

            return registration;
        }

        #endregion Mapper Methods


        #region Parameter Methods ******************************************************

        /// <summary>
        /// Get Parameters form insert/update of Registration
        /// </summary>
        /// <returns>Array of database parameters.</returns>
        private static MySqlParameter[] GetParameters(Registration registration)
        {
            //check if registration status is evaded
            if (registration.RegistrationStatus == (int)ItemStatus.Evaded)
            {
                //check if inactivation time is not set
                if (registration.InactivationTime == DateTime.MinValue)
                {
                    //should never happen anymore
                    //set inactivation time to now
                    registration.InactivationTime = DateTime.Now;
                }

                //check if inactivation reason is not set
                if (registration.InactivationReason == string.Empty)
                {
                    //should never happen anymore
                    //set generic reason
                    registration.InactivationReason = "Nenhuma razão específica.";
                }
            }

            MySqlParameter[] parameters = new MySqlParameter[9];
            parameters[0] = new MySqlParameter("registrationId", registration.Id);
            parameters[1] = new MySqlParameter("studentId", registration.StudentId);
            parameters[2] = new MySqlParameter("classId", registration.ClassId);
            parameters[3] = new MySqlParameter("position", registration.Position);
            parameters[4] = new MySqlParameter("autoRenewal", registration.AutoRenewal);
            parameters[5] = new MySqlParameter("registrationStatus", registration.RegistrationStatus);
            parameters[6] = new MySqlParameter("creationTime", registration.Id <= -1 ? DateTime.Now : registration.CreationTime);
            parameters[7] = new MySqlParameter("inactivationTime", DataAccessCommon.HandleDBNull(registration.InactivationTime));
            parameters[8] = new MySqlParameter("inactivationReason", DataAccessCommon.HandleDBNull(registration.InactivationReason));

            return parameters;
        }

        #endregion Parameter Methods

    } //end of class RegistrationMapper

} //end of namespace PnT.SongDB.Mapper
