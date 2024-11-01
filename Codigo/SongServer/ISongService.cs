using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using PnT.SongDB.Logic;


namespace PnT.SongServer
{
    /// <summary>
    /// Song service interface. Publishes all Song application methods.
    /// </summary>
    [ServiceContract]
    public interface ISongService
    {

        #region Answer ****************************************************************

        /// <summary>
        /// Find answers by filter.
        /// </summary>
        /// <param name="filterAnswerRapporteur">
        /// The answer rapporteur filter.
        /// -1 to select all rapporteurs.
        /// </param>
        /// <param name="filterAnswerTarget">
        /// The answer target filter.
        /// -1 to select all targets.
        /// </param>
        /// <param name="filterAnswerPeriodicity">
        /// The answer periodicity filter.
        /// -1 to select all periodicities.
        /// </param>
        /// <param name="filterAnswerMetric">
        /// The answer metric filter.
        /// -1 to select all metrics.
        /// </param>
        /// <param name="filterReport">
        /// The report filter.
        /// -1 to select all reports.
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
        /// <param name="filterCoordinator">
        /// The coordinator filter.
        /// -1 to select all coordinators.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// The list of answers.
        /// </returns>
        [OperationContract]
        List<Answer> FindAnswersByFilter(
            int filterAnswerRapporteur, int filterAnswerTarget, int filterAnswerPeriodicity,
            int filterAnswerMetric, int filterReport, int filterSemester, DateTime filterReferenceDate,
            int filterInstitution, int filterTeacher, int filterCoordinator, int filterClass);

        #endregion Answer


        #region Attendance ************************************************************

        /// <summary>
        /// Count attendances by filter.
        /// </summary>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterRollCall">
        /// The roll call filter.
        /// -1 to selct all roll calls.
        /// </param>
        /// <param name="filterStartDate">
        /// The start date filter.
        /// DateTime.MinValue to select all dates.
        /// </param>
        /// <param name="filterEndDate">
        /// The end date filter.
        /// DateTime.MinValue to select all dates.
        /// </param>
        /// <returns>
        /// The number of attendances.
        /// </returns>
        [OperationContract]
        CountResult CountAttendancesByFilter(
            int filterClass, int filterStudent, int filterTeacher,
            int filterRollCall, DateTime filterStartDate, DateTime filterEndDate);

        /// <summary>
        /// Find attendances by filter.
        /// </summary>
        /// <param name="loadClass">
        /// True to load class data for each found attendance.
        /// </param>
        /// <param name="loadStudent">
        /// True to load student data for each found attendance.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <returns>
        /// The list of attendances.
        /// </returns>
        [OperationContract]
        List<Attendance> FindAttendancesByFilter(bool loadClass, bool loadStudent, int filterClass, int filterStudent);

        /// <summary>
        /// Find attendances by class related filter.
        /// </summary>
        /// <param name="loadClass">
        /// True to load class data for each found attendance.
        /// </param>
        /// <param name="loadStudent">
        /// True to load student data for each found attendance.
        /// </param>
        /// <param name="filterSemester">
        /// The class semester filter.
        /// -1 to select all semesters.
        /// </param>
        /// <param name="filterInstitution">
        /// The class institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterPole">
        /// The class pole filter.
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterTeacher">
        /// The class teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// The list of attendances.
        /// </returns>
        [OperationContract]
        List<Attendance> FindAttendancesByClassFilter(
            bool loadClass, bool loadStudent, int filterSemester, 
            int filterInstitution, int filterPole, int filterTeacher, int filterClass);

        #endregion Attendance


        #region Class *****************************************************************

        /// <summary>
        /// Count classes by filter.
        /// </summary>
        /// <param name="filterClassStatus">
        /// The class status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterClassType">
        /// The class type filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type.
        /// -1 to select all instrument types.
        /// </param>
        /// <param name="filterClassLevel">
        /// The class level filter.
        /// -1 to select all levels.
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
        /// <returns>
        /// The number of classes.
        /// </returns>
        [OperationContract]
        CountResult CountClassesByFilter(
            int filterClassStatus, int filterClassType, int filterInstrumentType,
            int filterClassLevel, int filterSemester, int filterInstitution,
            int filterPole, int filterTeacher);
        
        /// <summary>
        /// Find class by ID.
        /// </summary>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <param name="loadTeacher">
        /// True to loade teacher data for selected class.
        /// </param>
        /// <param name="loadSemester">
        /// True to load semester data for selected class.
        /// </param>
        /// <returns>
        /// The selected class.
        /// </returns>
        [OperationContract]
        Class FindClass(int classId, bool loadTeacher, bool loadSemester);

        /// <summary>
        /// Find classes by filter.
        /// </summary>
        /// <param name="loadSemester">
        /// True to load semester data for each found class.
        /// </param>
        /// <param name="loadPole">
        /// True to load pole data for each found class.
        /// </param>
        /// <param name="loadTeacher">
        /// True to load teacher data for each found class.
        /// </param>
        /// <param name="filterClassStatus">
        /// The class status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterClassType">
        /// The class type filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type.
        /// -1 to select all instrument types.
        /// </param>
        /// <param name="filterClassLevel">
        /// The class level filter.
        /// -1 to select all levels.
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
        /// <returns>
        /// The list of classes.
        /// </returns>
        [OperationContract]
        List<Class> FindClassesByFilter(
            bool loadSemester, bool loadPole, bool loadTeacher, 
            int filterClassStatus, int filterClassType, int filterInstrumentType, 
            int filterClassLevel, int filterSemester, int filterInstitution, 
            int filterPole, int filterTeacher);

        /// <summary>
        /// Find next available subject code to be used when creating a new class.
        /// </summary>
        /// <returns>
        /// The next available subject code.
        /// </returns>
        [OperationContract]
        CountResult FindNextAvailableSubjectCode();

        /// <summary>
        /// Import selected classes into target semester and save new classes in database.
        /// </summary>
        /// <param name="semester">The source semester of the selected classes.</param>
        /// <param name="targetSemester">The target semester into which the classes will be imported.</param>
        /// <param name="classes">The list of selected classes.</param>
        /// <param name="registrationOption">
        /// 0 - to import no registration from classes.
        /// 1 - to import all registrations from classes.
        /// 2 - to import all auto renewal registrations from classes.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult ImportClasses(
            IdDescriptionStatus semester, IdDescriptionStatus targetSemester, 
            List<IdDescriptionStatus> classes, int registrationOption);

        /// <summary>
        /// Inactivate class by ID.
        /// </summary>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the class is being inactivated.
        /// </param>
        /// <returns>
        /// The delete operation result.
        /// </returns>
        [OperationContract]
        DeleteResult InactivateClass(int classId, string inactivationReason);

        /// <summary>
        /// Get list of class descriptions.
        /// </summary>
        /// <returns>
        /// The list of class descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListClasses();

        /// <summary>
        /// Get list of class descriptions by filter.
        /// </summary>
        /// <param name="filterClassStatus">
        /// The class status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterClassType">
        /// The class type filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterInstrumentType">
        /// The instrument type.
        /// -1 to select all instrument types.
        /// </param>
        /// <param name="filterClassLevel">
        /// The class level filter.
        /// -1 to select all levels.
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
        /// <returns>
        /// The list of class descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListClassesByFilter(
            int filterClassStatus, int filterClassType, int filterInstrumentType,
            int filterClassLevel, int filterSemester, int filterInstitution,
            int filterPole, int filterTeacher);

        /// <summary>
        /// Get list of class descriptions that belong to selected institution
        /// and match selected status.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <param name="status">
        /// The status of the returned classs.
        /// -1 to return all classs.
        /// </param>
        /// <returns>
        /// The list of class descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListClassesByInstitution(int institutionId, int status);

        /// <summary>
        /// Save class to database.
        /// The classId should be -1 if it is a new class.
        /// </summary>
        /// <param name="classObj">
        /// The class to be saved.
        /// </param>
        /// <param name="registrations">
        /// The list of registrations assigned to the class.
        /// All assigned registrations should be included in the list.
        /// Any other previously assigned registration will be removed.
        /// </param>
        /// <param name="attendances">
        /// The list of set attendances of the class.
        /// Updated and new attendances should be included in the list.
        /// Null if no attendance should be saved.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult SaveClass(
            Class classObj, List<Registration> registrations, List<Attendance> attendances);

        #endregion Class


        #region Email *****************************************************************

        #endregion Email


        #region Event *****************************************************************

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
        [OperationContract]
        CountResult CountEventsByFilter(
            int filterInstitution, DateTime filterStartDate, DateTime filterEndDate);

        /// <summary>
        /// Delete event by ID.
        /// </summary>
        /// <param name="eventId">
        /// The ID of the selected event.
        /// </param>
        /// <returns>
        /// The delete operation result.
        /// </returns>
        [OperationContract]
        DeleteResult DeleteEvent(int eventId);

        /// <summary>
        /// Find event by ID.
        /// </summary>
        /// <param name="eventId">
        /// The ID of the selected event.
        /// </param>
        /// <returns>
        /// The selected event.
        /// </returns>
        [OperationContract]
        Event FindEvent(int eventId);

        /// <summary>
        /// Find events by filter.
        /// </summary>
        /// <param name="loadInstitution">
        /// True to load institution data for each found event.
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
        /// The list of events.
        /// </returns>
        [OperationContract]
        List<Event> FindEventsByFilter(bool loadInstitution, 
            int filterInstitution, DateTime filterStartDate, DateTime filterEndDate);

        /// <summary>
        /// Get list of event descriptions.
        /// </summary>
        /// <returns>
        /// The list of event descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListEvents();

        /// <summary>
        /// Get list of event descriptions that belong to selected institution.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <returns>
        /// The list of event descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListEventsByInstitution(int institutionId);

        /// <summary>
        /// Save event to database.
        /// The eventId should be -1 if it is a new event.
        /// </summary>
        /// <param name="eventObj">
        /// The event to be saved.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult SaveEvent(Event eventObj);

        #endregion Event


        #region File ******************************************************************

        /// <summary>
        /// Get selected file.
        /// </summary>
        /// <param name="file">
        /// The file path.
        /// </param>
        /// <returns>
        /// The file.
        /// </returns>
        [OperationContract]
        File GetFile(string file);

        /// <summary>
        /// Get thumbnail file for selected file.
        /// </summary>
        /// <param name="file">
        /// The selected file.
        /// </param>
        /// <returns>
        /// The thumbnail file.
        /// </returns>
        [OperationContract]
        File GetFileThumbnail(string file);

        /// <summary>
        /// Save selected file to server disk.
        /// </summary>
        /// <param name="file">
        /// The file to be saved.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult SaveFile(File file);

        #endregion File


        #region Grade *****************************************************************

        /// <summary>
        /// Average grades score by filter.
        /// </summary>
        /// <param name="filterGradeRapporteur">
        /// The grade rapporteur filter.
        /// -1 to select all rapporteurs.
        /// </param>
        /// <param name="filterGradeTarget">
        /// The grade target filter.
        /// -1 to select all targets.
        /// </param>
        /// <param name="filterGradePeriodicity">
        /// The grade periodicity filter.
        /// -1 to select all periodicities.
        /// </param>
        /// <param name="filterGradeSubject">
        /// The grade subject filter.
        /// -1 to select all subjects.
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
        /// <param name="filterCoordinator">
        /// The coordinator filter.
        /// -1 to select all coordinators.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// The average score of filtered grades.
        /// </returns>
        [OperationContract]
        AverageResult AverageGradesByFilter(
            int filterGradeRapporteur, int filterGradeTarget, int filterGradePeriodicity,
            int filterGradeSubject, int filterSemester, DateTime filterReferenceDate,
            int filterInstitution, int filterTeacher, int filterCoordinator,
            int filterStudent, int filterClass);

        /// <summary>
        /// Find grades by filter.
        /// </summary>
        /// <param name="loadSemester">
        /// True to load semester data for each found grade.
        /// </param>
        /// <param name="loadInstitution">
        /// True to load institution data for each found grade.
        /// </param>
        /// <param name="loadTeacher">
        /// True to load teacher data for each found grade.
        /// </param>
        /// <param name="loadCoordinator">
        /// True to load coordinator data for each found grade.
        /// </param>
        /// <param name="loadStudent">
        /// True to load student data for each found grade.
        /// </param>
        /// <param name="loadClass">
        /// True to load class data for each found grade.
        /// </param>
        /// <param name="filterGradeRapporteur">
        /// The grade rapporteur filter.
        /// -1 to select all rapporteurs.
        /// </param>
        /// <param name="filterGradeTarget">
        /// The grade target filter.
        /// -1 to select all targets.
        /// </param>
        /// <param name="filterGradePeriodicity">
        /// The grade periodicity filter.
        /// -1 to select all periodicities.
        /// </param>
        /// <param name="filterGradeSubject">
        /// The grade subject filter.
        /// -1 to select all subjects.
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
        /// <param name="filterPole">
        /// The pole filter
        /// -1 to select all poles.
        /// </param>
        /// <param name="filterTeacher">
        /// The teacher filter.
        /// -1 to select all teachers.
        /// </param>
        /// <param name="filterCoordinator">
        /// The coordinator filter.
        /// -1 to select all coordinators.
        /// </param>
        /// <param name="filterStudent">
        /// The student filter.
        /// -1 to select all students.
        /// </param>
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// The list of grades.
        /// </returns>
        [OperationContract]
        List<Grade> FindGradesByFilter(
            bool loadSemester, bool loadInstitution, bool loadTeacher, 
            bool loadCoordinator, bool loadStudent, bool loadClass,
            int filterGradeRapporteur, int filterGradeTarget, int filterGradePeriodicity,
            int filterGradeSubject, int filterSemester, DateTime filterReferenceDate, 
            int filterInstitution, int filterPole, int filterTeacher, 
            int filterCoordinator, int filterStudent, int filterClass);

        #endregion Grade


        #region Institution ***********************************************************

        /// <summary>
        /// Count institutions by filter.
        /// </summary>
        /// <param name="filterInstitutionStatus">
        /// The institution status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <returns>
        /// The number of institutions.
        /// </returns>
        [OperationContract]
        CountResult CountInstitutionsByFilter(int filterInstitutionStatus);

        /// <summary>
        /// Find institution by ID.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <returns>
        /// The selected institution.
        /// </returns>
        [OperationContract]
        Institution FindInstitution(int institutionId);

        /// <summary>
        /// Find next available instrument without a loan for selected pole.
        /// </summary>
        /// <param name="instrumentId">
        /// The ID of the selected pole.
        /// </param>
        /// <returns>
        /// The next available instrument.
        /// </returns>
        [OperationContract]
        Instrument FindNextInstrumentWithoutLoan(int poleId);

        /// <summary>
        /// Find institutions by filter.
        /// </summary>
        /// <param name="loadCoordinator">
        /// True to load coordinator data for each found institution.
        /// </param>
        /// <param name="filterInstitutionStatus">
        /// The institution status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <returns>
        /// The list of institutions.
        /// </returns>
        [OperationContract]
        List<Institution> FindInstitutionsByFilter(bool loadCoordinator, int filterInstitutionStatus);

        /// <summary>
        /// Inactivate institution by ID.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the institution is being inactivated.
        /// </param>
        /// <returns>
        /// The delete operation result.
        /// </returns>
        [OperationContract]
        DeleteResult InactivateInstitution(int institutionId, string inactivationReason);

        /// <summary>
        /// Get institution description for selected institution.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <returns>
        /// The institution description.
        /// </returns>
        [OperationContract]
        IdDescriptionStatus ListInstitution(int institutionId);

        /// <summary>
        /// Get list of institution descriptions.
        /// </summary>
        /// <returns>
        /// The list of institution descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListInstitutions();

        /// <summary>
        /// Get list of institution descriptions that match 
        /// selected institution status.
        /// </summary>
        /// <param name="status">
        /// The selected institution status.
        /// </param>
        /// <returns>
        /// The list of institution descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListInstitutionsByStatus(int status);

        /// <summary>
        /// Save institution to database.
        /// The institutionId should be -1 if it is a new institution.
        /// </summary>
        /// <param name="institution">
        /// The institution to be saved.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult SaveInstitution(Institution institution);

        #endregion Institution


        #region Instrument ************************************************************

        /// <summary>
        /// Copy instrument by ID.
        /// </summary>
        /// <param name="instrumentId">
        /// The ID of the selected instrument.
        /// </param>
        /// <returns>
        /// Tthe instrument copy.
        /// </returns>
        [OperationContract]
        Instrument CopyInstrument(int instrumentId);

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
        [OperationContract]
        CountResult CountInstrumentsByFilter(
            int filterInstrumentStatus, int filterInstrumentType, 
            int filterInstitution, int filterPole);

        /// <summary>
        /// Find instrument by ID.
        /// </summary>
        /// <param name="instrumentId">
        /// The ID of the selected instrument.
        /// </param>
        /// <returns>
        /// The selected instrument.
        /// </returns>
        [OperationContract]
        Instrument FindInstrument(int instrumentId);

        /// <summary>
        /// Find instruments by filter.
        /// </summary>
        /// <param name="loadPole">
        /// True to load pole data for each found instrument.
        /// </param>
        /// <param name="loadPole">
        /// True to load loan data for each found instrument.
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
        /// The list of instruments.
        /// </returns>
        [OperationContract]
        List<Instrument> FindInstrumentsByFilter(
            bool loadPole, bool loadLoan, int filterInstrumentStatus, 
            int filterInstrumentType, int filterInstitution, int filterPole);

        /// <summary>
        /// Inactivate instrument by ID.
        /// </summary>
        /// <param name="instrumentId">
        /// The ID of the selected instrument.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the instrument is being inactivated.
        /// </param>
        /// <returns>
        /// The delete operation result.
        /// </returns>
        [OperationContract]
        DeleteResult InactivateInstrument(int instrumentId, string inactivationReason);

        /// <summary>
        /// Get list of instrument descriptions.
        /// </summary>
        /// <returns>
        /// The list of instrument descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListInstruments();

        /// <summary>
        /// Get list of instrument descriptions for selected institution.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <param name="status">
        /// The status of the returned instruments.
        /// -1 to return all instruments.
        /// </param>
        /// <returns>
        /// The list of instrument descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListInstrumentsByInstitution(int institutionId, int status);

        /// <summary>
        /// Get list of instrument descriptions for selected pole.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// </param>
        /// <param name="status">
        /// The status of the returned instruments.
        /// -1 to return all instruments.
        /// </param>
        /// <returns>
        /// The list of instrument descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListInstrumentsByPole(int poleId, int status);

        /// <summary>
        /// Get list of instrument descriptions that match 
        /// selected instrument status.
        /// </summary>
        /// <param name="status">
        /// The selected instrument status.
        /// </param>
        /// <returns>
        /// The list of instrument descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListInstrumentsByStatus(int status);

        /// <summary>
        /// Save instrument to database.
        /// The instrumentId should be -1 if it is a new instrument.
        /// </summary>
        /// <param name="instrument">
        /// The instrument to be saved.
        /// </param>
        /// <param name="loans">
        /// The list of loans assigned to the student.
        /// All assigned loans should be included in the list.
        /// Any other previously assigned loan will be removed.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult SaveInstrument(Instrument instrument, List<Loan> loans);

        #endregion Instrument


        #region Loan ******************************************************************

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
        [OperationContract]
        CountResult CountLoansByFilter(
            int filterLoanStatus, int filterInstrument, int filterStudent,
            DateTime filterStartDate, DateTime filterEndDate);

        /// <summary>
        /// Find all loans for selected instrument.
        /// </summary>
        /// <param name="instrumentId">
        /// The ID of the selected instrument.
        /// </param>
        /// <param name="status">
        /// The status of the returned loans.
        /// -1 to return all loans.
        /// </param>
        /// <returns>
        /// The list of loans.
        /// </returns>
        [OperationContract]
        List<Loan> FindLoansByInstrument(int instrumentId, int status);

        /// <summary>
        /// Find all loans for selected pole.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// </param>
        /// <param name="status">
        /// The status of the returned loans.
        /// -1 to return all loans.
        /// </param>
        /// <returns>
        /// The list of loans.
        /// </returns>
        [OperationContract]
        List<Loan> FindLoansByPole(int poleId, int status);

        /// <summary>
        /// Find all loans for selected student.
        /// </summary>
        /// <param name="studentId">
        /// The ID of the selected student.
        /// </param>
        /// <param name="status">
        /// The status of the returned loans.
        /// -1 to return all loans.
        /// </param>
        /// <returns>
        /// The list of loans.
        /// </returns>
        [OperationContract]
        List<Loan> FindLoansByStudent(int studentId, int status);

        #endregion Loan


        #region Permission ************************************************************

        /// <summary>
        /// Find permission by ID.
        /// </summary>
        /// <param name="permissionId">
        /// The ID of the selected permission.
        /// </param>
        /// <returns>
        /// The selected permission.
        /// </returns>
        [OperationContract]
        Permission FindPermission(int permissionId);

        /// <summary>
        /// Find all permissions.
        /// </summary>
        /// <returns>
        /// The list of permissions.
        /// </returns>
        [OperationContract]
        List<Permission> FindPermissions();

        /// <summary>
        /// Find all assigned permissions for selected role.
        /// </summary>
        /// <param name="roleId">
        /// The ID of the selected role.
        /// </param>
        /// <returns>
        /// The list of permissions.
        /// </returns>
        [OperationContract]
        List<Permission> FindPermissionsByRole(int roleId);

        #endregion Permission


        #region Pole ******************************************************************

        /// <summary>
        /// Count poles by filter.
        /// </summary>
        /// <param name="filterPoleStatus">
        /// The pole status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <returns>
        /// The number of poles.
        /// </returns>
        [OperationContract]
        CountResult CountPolesByFilter(int filterPoleStatus, int filterInstitution);

        /// <summary>
        /// Find pole by ID.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// </param>
        /// <returns>
        /// The selected pole.
        /// </returns>
        [OperationContract]
        Pole FindPole(int poleId);

        /// <summary>
        /// Find poles by filter.
        /// </summary>
        /// <param name="loadInstitution">
        /// True to load institution data for each found pole.
        /// </param>
        /// <param name="filterPoleStatus">
        /// The pole status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <returns>
        /// The list of poles.
        /// </returns>
        [OperationContract]
        List<Pole> FindPolesByFilter(bool loadInstitution, int filterPoleStatus, int filterInstitution);

        /// <summary>
        /// Inactivate pole by ID.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the pole is being inactivated.
        /// </param>
        /// <returns>
        /// The delete operation result.
        /// </returns>
        [OperationContract]
        DeleteResult InactivatePole(int poleId, string inactivationReason);

        /// <summary>
        /// Get list of pole descriptions.
        /// </summary>
        /// <returns>
        /// The list of pole descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListPoles();
        
        /// <summary>
        /// Get list of pole descriptions that belong to selected institution
        /// and match selected status.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <param name="status">
        /// The status of the returned poles.
        /// -1 to return all poles.
        /// </param>
        /// <returns>
        /// The list of pole descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListPolesByInstitution(int institutionId, int status);

        /// <summary>
        /// Get list of pole descriptions that match 
        /// selected pole status.
        /// </summary>
        /// <param name="status">
        /// The selected pole status.
        /// </param>
        /// <returns>
        /// The list of pole descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListPolesByStatus(int status);

        /// <summary>
        /// Find all assigned poles for selected teacher.
        /// </summary>
        /// <param name="teacherId">
        /// The ID of the selected teacher.
        /// </param>
        /// <returns>
        /// The list of pole descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListPolesByTeacher(int teacherId);

        /// <summary>
        /// Save pole to database.
        /// The poleId should be -1 if it is a new pole.
        /// </summary>
        /// <param name="pole">
        /// The pole to be saved.
        /// </param>
        /// <param name="teacherIds">
        /// The ID list of teachers assigned to the teacher.
        /// All assigned teachers should be included in the list.
        /// Any other previously assigned teacher will be removed.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult SavePole(Pole pole, List<int> teacherIds);

        #endregion Pole


        #region Questions *************************************************************

        /// <summary>
        /// Find all questions.
        /// </summary>
        /// <returns>
        /// The list of questions.
        /// </returns>
        [OperationContract]
        List<Question> FindQuestions();

        #endregion Questions


        #region Registration **********************************************************

        /// <summary>
        /// Count registration evations by filter.
        /// </summary>
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
        /// The number of evations.
        /// </returns>
        [OperationContract]
        CountResult CountEvationsByFilter(
            int filterSemester, DateTime filterReferenceDate, int filterInstitution, int filterTeacher);

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
        /// <returns>
        /// The number of registrations.
        /// </returns>
        [OperationContract]
        CountResult CountRegistrationsByClass(int classId, int status);

        /// <summary>
        /// Count registrations by filter.
        /// </summary>
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
        [OperationContract]
        CountResult CountRegistrationsByFilter(
            int filterRegistrationStatus, int filterSemester, int filterInstitution,
            int filterPole, int filterTeacher, int filterClass);

        /// <summary>
        /// Find all registrations for selected class.
        /// </summary>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <param name="status">
        /// The status of the returned registrations.
        /// -1 to return all registrations.
        /// </param>
        /// <returns>
        /// The list of registrations.
        /// </returns>
        [OperationContract]
        List<Registration> FindRegistrationsByClass(int classId, int status);

        /// <summary>
        /// Find registrations by filter.
        /// </summary>
        /// <param name="loadClass">
        /// True to load class data for each found registration.
        /// </param>
        /// <param name="loadStudent">
        /// True to load student data for each found registration.
        /// </param>
        /// <param name="loadSemester">
        /// True to load semester data for each found registration.
        /// </param>
        /// <param name="loadPole">
        /// True to load pole data for each found registration.
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
        /// The list of registrations.
        /// </returns>
        [OperationContract]
        List<Registration> FindRegistrationsByFilter(
            bool loadClass, bool loadStudent, bool loadSemester, bool loadPole,
            int filterRegistrationStatus, int filterSemester, int filterInstitution,
            int filterPole, int filterTeacher, int filterClass);

        /// <summary>
        /// Find all registrations for selected student.
        /// </summary>
        /// <param name="loadTeacher">
        /// True to load teacher data for each found registration.
        /// </param>
        /// <param name="loadSemester">
        /// True to load semester data for each found registration.
        /// </param>
        /// <param name="studentId">
        /// The ID of the selected student.
        /// </param>
        /// <param name="status">
        /// The status of the returned registrations.
        /// -1 to return all registrations.
        /// </param>
        /// <returns>
        /// The list of registrations.
        /// </returns>
        [OperationContract]
        List<Registration> FindRegistrationsByStudent(
            bool loadTeacher, bool loadSemester, int studentId, int status);

        #endregion Registration


        #region Report ****************************************************************

        /// <summary>
        /// Check generated reports for selected class.
        /// </summary>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <returns>
        /// A list of missing report months for the selected class.
        /// </returns>
        [OperationContract]
        List<DateTimeResult> CheckReports(int classId);

        /// <summary>
        /// Find report by ID.
        /// </summary>
        /// <param name="reportId">
        /// The ID of the selected report.
        /// </param>
        /// <returns>
        /// The selected report.
        /// </returns>
        [OperationContract]
        Report FindReport(int reportId);

        /// <summary>
        /// Find reports by filter.
        /// </summary>
        /// <param name="loadSemester">
        /// True to load semester data for each found report.
        /// </param>
        /// <param name="loadInstitution">
        /// True to load institution data for each found report.
        /// </param>
        /// <param name="loadTeacher">
        /// True to load teacher data for each found report.
        /// </param>
        /// <param name="loadCoordinator">
        /// True to load coordinator data for each found report.
        /// </param>
        /// <param name="loadClass">
        /// True to load class data for each found report.
        /// </param>
        /// <param name="filterReportStatus">
        /// The report status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterReportRapporteur">
        /// The report rapporteur filter.
        /// -1 to select all types.
        /// </param>
        /// <param name="filterReportPeriodicity">
        /// The report periodicity filter.
        /// -1 to select all instrument types.
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
        /// <param name="filterClass">
        /// The class filter.
        /// -1 to select all classes.
        /// </param>
        /// <returns>
        /// The list of reports.
        /// </returns>
        [OperationContract]
        List<Report> FindReportsByFilter(
            bool loadSemester, bool loadInstitution, 
            bool loadTeacher, bool loadCoordinator, bool loadClass,
            int filterReportStatus, int filterReportRapporteur, int filterReportPeriodicity,
            int filterSemester, DateTime filterReferenceDate, int filterInstitution, 
            int filterTeacher, int filterClass);

        /// <summary>
        /// Generate report for selected class and reference date.
        /// </summary>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <param name="referenceDate">
        /// The selected reference date.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult GenerateReport(int classId, DateTime period);

        /// <summary>
        /// Generate reports for current semester month.
        /// </summary>
        /// <returns>
        /// The number of generated reports.
        /// </returns>
        [OperationContract]
        CountResult GenerateReports();

        /// <summary>
        /// Save report to database.
        /// The reportId should be -1 if it is a new report.
        /// </summary>
        /// <param name="report">
        /// The report to be saved.
        /// </param>
        /// <param name="attendances">
        /// The list of set attendances for class target by the report.
        /// Updated and new attendances should be included in the list.
        /// Null if no attendance should be saved.
        /// </param>
        /// <param name="grades">
        /// The list of set grades for students target by the report.
        /// Updated and new grades should be included in the list.
        /// Null if no grade should be saved.
        /// </param>
        /// <param name="answers">
        /// The list of set answers for students target by the report.
        /// Updated and new answers should be included in the list.
        /// Null if no answer should be saved.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult SaveReport(
            Report report, List<Attendance> attendances, List<Grade> grades, List<Answer> answers);

        #endregion Report


        #region Role ******************************************************************

        /// <summary>
        /// Copy role by ID.
        /// </summary>
        /// <param name="roleId">
        /// The ID of the selected role.
        /// </param>
        /// <returns>
        /// Tthe role copy.
        /// </returns>
        [OperationContract]
        Role CopyRole(int roleId);

        /// <summary>
        /// Delete role by ID.
        /// </summary>
        /// <param name="roleId">
        /// The ID of the selected role.
        /// </param>
        /// <returns>
        /// The delete operation result.
        /// </returns>
        [OperationContract]
        DeleteResult DeleteRole(int roleId);

        /// <summary>
        /// Find role by ID.
        /// </summary>
        /// <param name="roleId">
        /// The ID of the selected role.
        /// </param>
        /// <returns>
        /// The selected role.
        /// </returns>
        [OperationContract]
        Role FindRole(int roleId);

        /// <summary>
        /// Get list of role descriptions.
        /// </summary>
        /// <returns>
        /// The list of role descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListRoles();

        /// <summary>
        /// Save role to database.
        /// The roleId should be -1 if it is a new role.
        /// </summary>
        /// <param name="role">
        /// The role to be saved.
        /// </param>
        /// <param name="permissionIds">
        /// The ID list of permissions assigned to the role.
        /// All assigned permissions should be included in the list.
        /// Any other previously assigned permission will be removed.
        /// </param>
        /// <param name="addedUserIds">
        /// The ID list of users to whom the role was assigned.
        /// Only the newly assigned users should be included in the list.
        /// Any other previously assigned user will remain in the role.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult SaveRole(Role role, List<int> permissionIds, List<int> addedUserIds);

        #endregion Role


        #region Semester **************************************************************

        /// <summary>
        /// Find current ongoing semester.
        /// </summary>
        /// <returns>
        /// The current ongoing semester.
        /// </returns>
        [OperationContract]
        Semester FindCurrentSemester();

        /// <summary>
        /// Find semester by ID.
        /// </summary>
        /// <param name="semesterId">
        /// The ID of the selected semester.
        /// </param>
        /// <returns>
        /// The selected semester.
        /// </returns>
        [OperationContract]
        Semester FindSemester(int semesterId);

        /// <summary>
        /// Find all semesters.
        /// </summary>
        /// <param name="excludePastSemesters">
        /// True to exclude past semesters and return only semesters from today on.
        /// False to return all semesters.
        /// </param>
        /// <returns>
        /// The list of semesters.
        /// </returns>
        [OperationContract]
        List<Semester> FindSemesters(bool excludePastSemesters);

        /// <summary>
        /// Get list of semester descriptions.
        /// </summary>
        /// <returns>
        /// The list of semester descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListSemesters();

        /// <summary>
        /// Update semester to database.
        /// </summary>
        /// <param name="semester">
        /// The semester to be updated.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult UpdateSemester(Semester semester);

        #endregion Semester


        #region Student ***************************************************************

        /// <summary>
        /// Copy student by ID.
        /// </summary>
        /// <param name="studentId">
        /// The ID of the selected student.
        /// </param>
        /// <returns>
        /// Tthe student copy.
        /// </returns>
        [OperationContract]
        Student CopyStudent(int studentId);

        /// <summary>
        /// Count students by filter.
        /// </summary>
        /// <param name="filterStudentStatus">
        /// The student status filter.
        /// -1 to select all statuses.
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
        /// The number of students.
        /// </returns>
        [OperationContract]
        CountResult CountStudentsByFilter(
            int filterStudentStatus, int filterInstitution, int filterPole);

        /// <summary>
        /// Find student by ID.
        /// </summary>
        /// <param name="studentId">
        /// The ID of the selected student.
        /// </param>
        /// <returns>
        /// The selected student.
        /// </returns>
        [OperationContract]
        Student FindStudent(int studentId);

        /// <summary>
        /// Find next available student without a loan for selected pole.
        /// </summary>
        /// <param name="studentId">
        /// The ID of the selected pole.
        /// </param>
        /// <returns>
        /// The next available student.
        /// </returns>
        [OperationContract]
        Student FindNextStudentWithoutLoan(int poleId);

        /// <summary>
        /// Find students by filter.
        /// </summary>
        /// <param name="loadUser">
        /// True to load user data for each found student.
        /// </param>
        /// <param name="loadPole">
        /// True to load pole data for each found student.
        /// </param>
        /// <param name="filterStudentStatus">
        /// The student status filter.
        /// -1 to select all statuses.
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
        /// The list of students.
        /// </returns>
        [OperationContract]
        List<Student> FindStudentsByFilter(bool loadUser, bool loadPole, 
            int filterStudentStatus, int filterInstitution, int filterPole);

        /// <summary>
        /// Inactivate student by ID.
        /// </summary>
        /// <param name="studentId">
        /// The ID of the selected student.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the student is being inactivated.
        /// </param>
        /// <returns>
        /// The delete operation result.
        /// </returns>
        [OperationContract]
        DeleteResult InactivateStudent(int studentId, string inactivationReason);

        /// <summary>
        /// Get list of student descriptions.
        /// </summary>
        /// <returns>
        /// The list of student descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListStudents();

        /// <summary>
        /// Get list of student descriptions for selected class.
        /// </summary>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <param name="status">
        /// The status of the returned students.
        /// -1 to return all students.
        /// </param>
        /// <returns>
        /// The list of student descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListStudentsByClass(int classId, int status);

        /// <summary>
        /// Get list of student descriptions for selected institution.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <param name="status">
        /// The status of the returned students.
        /// -1 to return all students.
        /// </param>
        /// <returns>
        /// The list of student descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListStudentsByInstitution(int institutionId, int status);

        /// <summary>
        /// Get list of student descriptions that match 
        /// selected student status.
        /// </summary>
        /// <param name="status">
        /// The selected student status.
        /// </param>
        /// <returns>
        /// The list of student descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListStudentsByStatus(int status);

        /// <summary>
        /// Get list of student descriptions for selected pole.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// </param>
        /// <param name="status">
        /// The status of the returned students.
        /// -1 to return all students.
        /// </param>
        /// <returns>
        /// The list of student descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListStudentsByPole(int poleId, int status);

        /// <summary>
        /// Save student to database.
        /// The studentId should be -1 if it is a new student.
        /// </summary>
        /// <param name="student">
        /// The student to be saved.
        /// </param>
        /// <param name="registrations">
        /// The list of registrations assigned to the student.
        /// All assigned registrations should be included in the list.
        /// Any other previously assigned registration will be removed.
        /// </param>
        /// <param name="loans">
        /// The list of loans assigned to the student.
        /// All assigned loans should be included in the list.
        /// Any other previously assigned loan will be removed.
        /// </param>
        /// <param name="photo">
        /// The new photo file assigned to the student.
        /// Any previous photo file will be replaced.
        /// Null if no new photo file is being sent.
        /// </param>
        /// <param name="assignment">
        /// The new assignment file assigned to the student.
        /// Any previous assignment file will be replaced.
        /// Null if no new assignment file is being sent.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult SaveStudent(
            Student student, List<Registration> registrations, 
            List<Loan> loans, File photo, File assignment);

        #endregion Student


        #region Teacher ***************************************************************

        /// <summary>
        /// Count teachers by filter.
        /// </summary>
        /// <param name="filterTeacherStatus">
        /// The teacher status filter.
        /// -1 to select all statuses.
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
        /// The number of teachers.
        /// </returns>
        [OperationContract]
        CountResult CountTeachersByFilter(
            int filterTeacherStatus, int filterInstitution, int filterPole);

        /// <summary>
        /// Find teacher by ID.
        /// </summary>
        /// <param name="teacherId">
        /// The ID of the selected teacher.
        /// </param>
        /// <returns>
        /// The selected teacher.
        /// </returns>
        [OperationContract]
        Teacher FindTeacher(int teacherId);

        /// <summary>
        /// Find teacher by user ID.
        /// </summary>
        /// <param name="userId">
        /// The ID of the selected user.
        /// </param>
        /// <param name="loadUser">
        /// True to load user data for found teacher.
        /// </param>
        /// <param name="loadPoles">
        /// True to load assigned poles data for found teacher.
        /// </param>
        /// <returns>
        /// The teacher of the selected user.
        /// Null if user has no teacher.
        /// </returns>
        [OperationContract]
        Teacher FindTeacherByUser(int userId, bool loadUser, bool loadPoles);

        /// <summary>
        /// Find teachers by filter.
        /// </summary>
        /// <param name="loadUser">
        /// True to load user data for each found teacher.
        /// </param>
        /// <param name="loadPoles">
        /// True to load assigned poles data for each found teacher.
        /// </param>
        /// <param name="filterTeacherStatus">
        /// The teacher status filter.
        /// -1 to select all statuses.
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
        /// The list of teachers.
        /// </returns>
        [OperationContract]
        List<Teacher> FindTeachersByFilter(bool loadUser, bool loadPoles, 
            int filterTeacherStatus, int filterInstitution, int filterPole);

        /// <summary>
        /// Inactivate teacher by ID.
        /// </summary>
        /// <param name="teacherId">
        /// The ID of the selected teacher.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the teacher is being inactivated.
        /// </param>
        /// <returns>
        /// The delete operation result.
        /// </returns>
        [OperationContract]
        DeleteResult InactivateTeacher(int teacherId, string inactivationReason);

        /// <summary>
        /// Get list of teacher descriptions.
        /// </summary>
        /// <returns>
        /// The list of teacher descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListTeachers();

        /// <summary>
        /// Get list of teacher descriptions for selected institution.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <param name="status">
        /// The status of the returned teachers.
        /// -1 to return all teachers.
        /// </param>
        /// <returns>
        /// The list of teacher descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListTeachersByInstitution(int institutionId, int status);

        /// <summary>
        /// Get list of teacher descriptions for selected pole.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// </param>
        /// <param name="status">
        /// The status of the returned teachers.
        /// -1 to return all teachers.
        /// </param>
        /// <returns>
        /// The list of teacher descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListTeachersByPole(int poleId, int status);

        /// <summary>
        /// Get list of teacher descriptions that match 
        /// selected teacher status.
        /// </summary>
        /// <param name="status">
        /// The selected teacher status.
        /// </param>
        /// <returns>
        /// The list of teacher descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListTeachersByStatus(int status);

        /// <summary>
        /// Save teacher to database.
        /// The teacherId should be -1 if it is a new teacher.
        /// </summary>
        /// <param name="teacher">
        /// The teacher to be saved.
        /// </param>
        /// <param name="poleIds">
        /// The ID list of poles assigned to the teacher.
        /// All assigned poles should be included in the list.
        /// Any other previously assigned pole will be removed.
        /// </param>
        /// <param name="attendances">
        /// The list of set attendances for all classes assigned to the teacher.
        /// Updated and new attendances should be included in the list.
        /// Null if no attendance should be saved.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult SaveTeacher(
            Teacher teacher, List<int> poleIds, List<Attendance> attendances);

        #endregion Teacher


        #region User ******************************************************************

        /// <summary>
        /// Find user by ID.
        /// </summary>
        /// <param name="userId">
        /// The ID of the selected user.
        /// </param>
        /// <returns>
        /// The selected user.
        /// </returns>
        [OperationContract]
        User FindUser(int userId);

        /// <summary>
        /// Find user by login.
        /// </summary>
        /// <param name="login">
        /// The login of the selected user.
        /// </param>
        /// <returns>
        /// The selected user.
        /// </returns>
        [OperationContract]
        User FindUserByLogin(string login);

        /// <summary>
        /// Find users by filter.
        /// </summary>
        /// <param name="loadInstitution">
        /// True to load institution data for each found user.
        /// </param>
        /// <param name="loadRole">
        /// True to load assigned role data for each found user.
        /// </param>
        /// <param name="filterUserStatus">
        /// The user status filter.
        /// -1 to select all statuses.
        /// </param>
        /// <param name="filterInstitution">
        /// The institution filter.
        /// -1 to select all institutions.
        /// </param>
        /// <param name="filterRole">
        /// The role filter.
        /// -1 to select all roles.
        /// </param>
        /// <returns>
        /// The list of users.
        /// </returns>
        [OperationContract]
        List<User> FindUsersByFilter(bool loadInstitution, bool loadRole,
            int filterUserStatus, int filterInstitution, int filterRole);

        /// <summary>
        /// Inactivate user by ID.
        /// </summary>
        /// <param name="userId">
        /// The ID of the selected user.
        /// </param>
        /// <param name="inactivationReason">
        /// The reason why the user is being inactivated.
        /// </param>
        /// <returns>
        /// The delete operation result.
        /// </returns>
        [OperationContract]
        DeleteResult InactivateUser(int userId, string inactivationReason);

        /// <summary>
        /// Get list of user descriptions.
        /// </summary>
        /// <returns>
        /// The list of user descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListUsers();

        /// <summary>
        /// Get list of user descriptions for selected institution.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <param name="status">
        /// The status of the returned teachers.
        /// -1 to return all teachers.
        /// </param>
        /// <returns>
        /// The list of user descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListUsersByInstitution(int institutionId, int status);

        /// <summary>
        /// Get list of user descriptions that are assigned to the selected role.
        /// </summary>
        /// <param name="roleId">
        /// The ID of the selected role.
        /// </param>
        /// <returns>
        /// The list of user descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListUsersByRole(int roleId);

        /// <summary>
        /// Get list of user descriptions that match 
        /// selected user status.
        /// </summary>
        /// <param name="status">
        /// The selected user status.
        /// </param>
        /// <returns>
        /// The list of user descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListUsersByStatus(int status);

        /// <summary>
        /// Get list of coordinator user descriptions that match 
        /// selected user status.
        /// </summary>
        /// <param name="status">
        /// The selected user status.
        /// </param>
        /// <returns>
        /// The list of coordinator user descriptions.
        /// </returns>
        [OperationContract]
        List<IdDescriptionStatus> ListCoordinatorsByStatus(int status);

        /// <summary>
        /// Logon selected user into system.
        /// </summary>
        /// <param name="login">
        /// The login of the selected user.
        /// </param>
        /// <param name="password">
        /// The password of the selected user.
        /// </param>
        /// <returns>
        /// The logged on user.
        /// </returns>
        [OperationContract]
        User LogonUser(string login, byte[] password);

        /// <summary>
        /// Change password for selected user.
        /// </summary>
        /// <param name="login">
        /// The login of the selected user.
        /// </param>
        /// <param name="oldPassword">
        /// The old password.
        /// </param>
        /// <param name="newPassword">
        /// The new password.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult ChangeUserPassword(string login, byte[] oldPassword, byte[] newPassword);

        /// <summary>
        /// Save user to database.
        /// The userId should be -1 if it is a new user.
        /// </summary>
        /// <param name="user">
        /// The user to be saved.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        [OperationContract]
        SaveResult SaveUser(User user);
        
        /// <summary>
        /// Generate recovery password for selected user. 
        /// Send generated code by e-mail to user.
        /// </summary>
        /// <param name="userId">
        /// The ID of the selected user.
        /// </param>
        /// <returns>
        /// The send operation result.
        /// </returns>
        [OperationContract]
        SendResult SendRecoveryPasswordToUser(int userId);

        #endregion User


        #region Server ****************************************************************

        /// <summary>
        /// Get heartbeat response from Song Server.
        /// </summary>
        /// <returns>
        /// The current time of the server.
        /// </returns>
        [OperationContract]
        DateTime GetHeartbeat();

        /// <summary>
        /// Get server related information of the Song Server.
        /// </summary>
        /// <returns>
        /// The server related information.
        /// </returns>
        [OperationContract]
        ServerInfo GetServerInfo();

        #endregion Server

    } //end of interface ISongService

} //end of namespace PnT.SongServer
