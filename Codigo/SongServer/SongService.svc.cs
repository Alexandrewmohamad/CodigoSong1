using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using MySql.Data.MySqlClient;

using PnT.SongDB.Logic;
using PnT.SongDB;

namespace PnT.SongServer
{

    /// <summary>
    /// Implements the song service interface.
    /// </summary>
    public class SongService : ISongService
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
        public List<Answer> FindAnswersByFilter(
            int filterAnswerRapporteur, int filterAnswerTarget, int filterAnswerPeriodicity,
            int filterAnswerMetric, int filterReport, int filterSemester, DateTime filterReferenceDate,
            int filterInstitution, int filterTeacher, int filterCoordinator, int filterClass)
        {
            //create result list
            List<Answer> answers = new List<Answer>();

            try
            {
                //get list of answers from database using filters
                List<Answer> dbAnswers = Answer.FindByFilter(
                    filterAnswerRapporteur, filterAnswerTarget, filterAnswerPeriodicity,
                    filterAnswerMetric, filterReport, filterSemester, filterReferenceDate,
                    filterInstitution, filterTeacher, filterCoordinator, filterClass);

                //check result
                if (dbAnswers == null || dbAnswers.Count == 0)
                {
                    //no answer was found
                    //create result and add it to the list
                    Answer resultAnswer = new Answer();
                    resultAnswer.Result = (int)SelectResult.Empty;
                    answers.Add(resultAnswer);
                }
                else
                {
                    //check each answer
                    foreach (Answer dbAnswer in dbAnswers)
                    {
                        //set and add answer
                        dbAnswer.Result = (int)SelectResult.Success;
                        answers.Add(dbAnswer);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading answers
                //create result and add it to the list
                Answer resultAnswer = new Answer();
                resultAnswer.Result = (int)SelectResult.FatalError;
                resultAnswer.ErrorMessage = ex.Message;
                answers.Add(resultAnswer);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultAnswer.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return answers;
        }

        #endregion Answer


        #region Constants *************************************************************

        /// <summary>
        /// Output file directory path.
        /// </summary>
        public const string FILE_DIR_PATH = "Files";


        #endregion Constants


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
        public CountResult CountAttendancesByFilter(
            int filterClass, int filterStudent, int filterTeacher,
            int filterRollCall, DateTime filterStartDate, DateTime filterEndDate)
        {
            //create result
            CountResult countResult = new CountResult();

            try
            {
                //count attendances in database and set result
                countResult.Count = Attendance.CountByFilter(
                    filterClass, filterStudent, filterTeacher, 
                    filterRollCall, filterStartDate, filterEndDate);
            }
            catch (Exception ex)
            {
                //error while loading attendances
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return countResult;
        }

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
        public List<Attendance> FindAttendancesByFilter(
            bool loadClass, bool loadStudent, int filterClass, int filterStudent)
        {
            //create result list
            List<Attendance> attendances = new List<Attendance>();

            try
            {
                //get list of attendances from database using filters
                List<Attendance> dbAttendances = Attendance.FindByFilter(filterClass, filterStudent);

                //check result
                if (dbAttendances == null || dbAttendances.Count == 0)
                {
                    //no attendance was found
                    //create result and add it to the list
                    Attendance resultAttendance = new Attendance();
                    resultAttendance.Result = (int)SelectResult.Empty;
                    attendances.Add(resultAttendance);
                }
                else
                {
                    //check each attendance
                    foreach (Attendance dbAttendance in dbAttendances)
                    {
                        //set and add attendance
                        dbAttendance.Result = (int)SelectResult.Success;
                        attendances.Add(dbAttendance);
                    }

                    //check if class should be loaded
                    if (loadClass)
                    {
                        //check filter class
                        if (filterClass > -1)
                        {
                            //get class
                            Class classObj = Class.Find(filterClass);

                            //check result
                            if (classObj != null)
                            {
                                //check each attendance
                                foreach (Attendance attendance in attendances)
                                {
                                    //set class code
                                    attendance.ClassCode = classObj.Code;
                                }
                            }
                        }
                        else
                        {
                            //get list of classes
                            List<Class> classes = Class.Find();

                            //check result
                            if (classes != null)
                            {
                                //check each attendance
                                foreach (Attendance attendance in attendances)
                                {
                                    //find class
                                    Class classObj = classes.Find(
                                        i => i.ClassId == attendance.ClassId);

                                    //check result
                                    if (classObj != null)
                                    {
                                        //set class code
                                        attendance.ClassCode = classObj.Code;
                                    }
                                }
                            }
                        }
                    }

                    //check if class should be loaded
                    if (loadStudent)
                    {
                        //check filter student
                        if (filterStudent > -1)
                        {
                            //get student
                            Student student = Student.Find(filterStudent);

                            //check result
                            if (student != null)
                            {
                                //check each attendance
                                foreach (Attendance attendance in attendances)
                                {
                                    //set student name
                                    attendance.StudentName = student.Name;
                                }
                            }
                        }
                        else
                        {
                            //get list of students
                            List<Student> students = Student.Find();

                            //check result
                            if (students != null)
                            {
                                //check each attendance
                                foreach (Attendance attendance in attendances)
                                {
                                    //find student
                                    Student student = students.Find(
                                        i => i.StudentId == attendance.StudentId);

                                    //check result
                                    if (student != null)
                                    {
                                        //set student name
                                        attendance.StudentName = student.Name;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading attendances
                //create result and add it to the list
                Attendance resultAttendance = new Attendance();
                resultAttendance.Result = (int)SelectResult.FatalError;
                resultAttendance.ErrorMessage = ex.Message;
                attendances.Add(resultAttendance);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultAttendance.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return attendances;
        }
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
        public List<Attendance> FindAttendancesByClassFilter(
            bool loadClass, bool loadStudent, int filterSemester,
            int filterInstitution, int filterPole, int filterTeacher, int filterClass)
        {
            //create result list
            List<Attendance> attendances = new List<Attendance>();

            try
            {
                //get list of attendances from database using class filters
                List<Attendance> dbAttendances = Attendance.FindByClassFilter(
                    filterSemester, filterInstitution, filterPole, filterTeacher, filterClass);

                //check result
                if (dbAttendances == null || dbAttendances.Count == 0)
                {
                    //no attendance was found
                    //create result and add it to the list
                    Attendance resultAttendance = new Attendance();
                    resultAttendance.Result = (int)SelectResult.Empty;
                    attendances.Add(resultAttendance);
                }
                else
                {
                    //check each attendance
                    foreach (Attendance dbAttendance in dbAttendances)
                    {
                        //set and add attendance
                        dbAttendance.Result = (int)SelectResult.Success;
                        attendances.Add(dbAttendance);
                    }

                    //check if class should be loaded
                    if (loadClass)
                    {
                        //check filter class
                        if (filterClass > -1)
                        {
                            //get class
                            Class classObj = Class.Find(filterClass);

                            //check result
                            if (classObj != null)
                            {
                                //check each attendance
                                foreach (Attendance attendance in attendances)
                                {
                                    //set class code
                                    attendance.ClassCode = classObj.Code;
                                }
                            }
                        }
                        else
                        {
                            //get list of classes
                            List<Class> classes = Class.Find();

                            //check result
                            if (classes != null)
                            {
                                //check each attendance
                                foreach (Attendance attendance in attendances)
                                {
                                    //find class
                                    Class classObj = classes.Find(
                                        i => i.ClassId == attendance.ClassId);

                                    //check result
                                    if (classObj != null)
                                    {
                                        //set class code
                                        attendance.ClassCode = classObj.Code;
                                    }
                                }
                            }
                        }
                    }

                    //check if class should be loaded
                    if (loadStudent)
                    {
                        //get list of students
                        List<Student> students = Student.Find();

                        //check result
                        if (students != null)
                        {
                            //check each attendance
                            foreach (Attendance attendance in attendances)
                            {
                                //find student
                                Student student = students.Find(
                                    i => i.StudentId == attendance.StudentId);

                                //check result
                                if (student != null)
                                {
                                    //set student name
                                    attendance.StudentName = student.Name;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading attendances
                //create result and add it to the list
                Attendance resultAttendance = new Attendance();
                resultAttendance.Result = (int)SelectResult.FatalError;
                resultAttendance.ErrorMessage = ex.Message;
                attendances.Add(resultAttendance);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultAttendance.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return attendances;
        }

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
        public CountResult CountClassesByFilter(
            int filterClassStatus, int filterClassType, int filterInstrumentType,
            int filterClassLevel, int filterSemester, int filterInstitution,
            int filterPole, int filterTeacher)
        {
            //create result
            CountResult countResult = new CountResult();

            try
            {
                //count classs in database and set result
                countResult.Count = Class.CountByFilter(
                    filterClassStatus, filterClassType, filterInstrumentType,
                    filterClassLevel, filterSemester, filterInstitution, filterPole, filterTeacher);
            }
            catch (Exception ex)
            {
                //error while loading classs
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return countResult;
        }

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
        public Class FindClass(int classId, bool loadTeacher, bool loadSemester)
        {
            //the target class
            Class resultClass = null;

            try
            {
                //find class in database
                resultClass = Class.Find(classId);

                //check result
                if (resultClass != null)
                {
                    //class was found
                    resultClass.Result = (int)SelectResult.Success;

                    //check if teacher should be loaded
                    if (loadTeacher)
                    {
                        //find teacher
                        Teacher teacher = Teacher.Find(resultClass.TeacherId);

                        //check result
                        if (teacher != null)
                        {
                            //set name and id
                            resultClass.TeacherName = teacher.Name;
                            resultClass.TeacherUserId = teacher.UserId;
                        }
                    }

                    //check if semester should be loaded
                    if (loadSemester)
                    {
                        //find semester
                        resultClass.Semester = Semester.Find(resultClass.SemesterId);
                    }
                }
                else
                {
                    //class was not found
                    //create result and set it
                    resultClass = new Class();
                    resultClass.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding class
                //create result and set it
                resultClass = new Class();
                resultClass.Result = (int)SelectResult.FatalError;
                resultClass.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultClass.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultClass;
        }

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
        public List<Class> FindClassesByFilter(
            bool loadSemester, bool loadPole, bool loadTeacher,
            int filterClassStatus, int filterClassType, int filterInstrumentType, 
            int filterClassLevel, int filterSemester, int filterInstitution,
            int filterPole, int filterTeacher)
        {
            //create result list
            List<Class> classes = new List<Class>();

            try
            {
                //get list of classes from database using filters
                List<Class> dbClasses = Class.FindByFilter(
                    filterClassStatus, filterClassType, filterInstrumentType, 
                    filterClassLevel, filterSemester, filterInstitution, filterPole, filterTeacher);

                //check result
                if (dbClasses == null || dbClasses.Count == 0)
                {
                    //no class was found
                    //create result and add it to the list
                    Class resultClass = new Class();
                    resultClass.Result = (int)SelectResult.Empty;
                    classes.Add(resultClass);
                }
                else
                {
                    //check each class
                    foreach (Class dbClass in dbClasses)
                    {
                        //set and add class
                        dbClass.Result = (int)SelectResult.Success;
                        classes.Add(dbClass);
                    }

                    //check if class should be loaded
                    if (loadSemester)
                    {
                        //check filter semester
                        if (filterSemester > -1)
                        {
                            //get semester
                            Semester semester = Semester.Find(filterSemester);

                            //check result
                            if (semester != null)
                            {
                                //check each class
                                foreach (Class classObj in classes)
                                {
                                    //set semester
                                    classObj.Semester = semester;
                                }
                            }
                        }
                        else
                        {

                            //get list of semesters
                            List<Semester> semesters = Semester.Find();

                            //check result
                            if (semesters != null)
                            {
                                //check each class
                                foreach (Class classObj in classes)
                                {
                                    //find semester
                                    Semester semester = semesters.Find(
                                        i => i.SemesterId == classObj.SemesterId);

                                    //check result
                                    if (semester != null)
                                    {
                                        //set semester
                                        classObj.Semester = semester;
                                    }
                                }
                            }
                        }
                    }

                    //check if class should be loaded
                    if (loadPole)
                    {
                        //check filter pole
                        if (filterPole > -1)
                        {
                            //get pole
                            Pole pole = Pole.Find(filterPole);

                            //check result
                            if (pole != null)
                            {
                                //check each class
                                foreach (Class classObj in classes)
                                {
                                    //set pole name
                                    classObj.PoleName = pole.Name;
                                }
                            }
                        }
                        else
                        {

                            //get list of poles
                            List<Pole> poles = Pole.FindByFilter(-1, filterInstitution);

                            //check result
                            if (poles != null)
                            {
                                //check each class
                                foreach (Class classObj in classes)
                                {
                                    //find pole
                                    Pole pole = poles.Find(
                                        i => i.PoleId == classObj.PoleId);

                                    //check result
                                    if (pole != null)
                                    {
                                        //set pole name
                                        classObj.PoleName = pole.Name;
                                    }
                                }
                            }
                        }
                    }

                    //check if class should be loaded
                    if (loadTeacher)
                    {
                        //check filter teacher
                        if (filterTeacher > -1)
                        {
                            //get teacher
                            Teacher teacher = Teacher.Find(filterTeacher);

                            //check result
                            if (teacher != null)
                            {
                                //check each class
                                foreach (Class classObj in classes)
                                {
                                    //set teacher name and id
                                    classObj.TeacherName = teacher.Name;
                                    classObj.TeacherUserId = teacher.UserId;
                                }
                            }
                        }
                        else
                        {

                            //get list of teachers using some filters
                            List<Teacher> teachers = Teacher.FindByFilter(
                                -1, filterInstitution, filterPole);

                            //check result
                            if (teachers != null)
                            {
                                //check each class
                                foreach (Class classObj in classes)
                                {
                                    //find teacher
                                    Teacher teacher = teachers.Find(
                                        i => i.TeacherId == classObj.TeacherId);

                                    //check result
                                    if (teacher == null)
                                    {
                                        //teacher was not found
                                        //teacher might not belong to pole anymore
                                        //find teacher directly
                                        teacher = Teacher.Find(classObj.TeacherId);

                                        //check result
                                        if (teacher != null)
                                        {
                                            //add teacher to list of teachers
                                            teachers.Add(teacher);
                                        }
                                    }

                                    //check final result
                                    if (teacher != null)
                                    {
                                        //set teacher name and ID
                                        classObj.TeacherName = teacher.Name;
                                        classObj.TeacherUserId = teacher.UserId;
                                    }
                                    else
                                    {
                                        //should never happen
                                        //set teacher ID as teacher name
                                        classObj.TeacherName = classObj.TeacherId.ToString();

                                        //reset teacher user ID
                                        classObj.TeacherUserId = int.MinValue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading classes
                //create result and add it to the list
                Class resultClass = new Class();
                resultClass.Result = (int)SelectResult.FatalError;
                resultClass.ErrorMessage = ex.Message;
                classes.Add(resultClass);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultClass.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return classes;
        }

        /// <summary>
        /// Find next available subject code to be used when creating a new class.
        /// </summary>
        /// <returns>
        /// The next available subject code.
        /// </returns>
        public CountResult FindNextAvailableSubjectCode()
        {
            //create result
            CountResult nextResult = new CountResult();

            try
            {
                //cinf next available subect code in database and set result
                nextResult.Count = Class.FindNextAvailableSubjectCode();
            }
            catch (Exception ex)
            {
                //error while loading classs
                //set result
                nextResult.Count = int.MinValue;
                nextResult.Result = (int)SelectResult.FatalError;
                nextResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    nextResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return nextResult;
        }

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
        public SaveResult ImportClasses(
            IdDescriptionStatus semester, IdDescriptionStatus targetSemester, 
            List<IdDescriptionStatus> classes, int registrationOption)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();
            
            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //get list of classes from source semester from database using filters
                List<Class> dbSourceClasses = Class.FindByFilter(
                    -1, -1, -1, -1, semester.Id, -1, -1, -1);

                //check result
                if (dbSourceClasses == null || dbSourceClasses.Count == 0)
                {
                    //no class was found
                    //should never happen
                    //set result
                    saveResult.Result = (int)SelectResult.FatalError;
                    saveResult.ErrorMessage = "No class in the source semester.";

                    //return result
                    return saveResult;
                }

                //get list of classes from target semester from database using filters
                List<Class> dbTargetClasses = Class.FindByFilter(
                    -1, -1, -1, -1, targetSemester.Id, -1, -1, -1);

                //check result
                if (dbTargetClasses == null)
                {
                    //no class was found
                    //set empty list
                    dbTargetClasses = new List<Class>();
                }

                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //check each selected class
                foreach (IdDescriptionStatus classObj in classes)
                {
                    //find source class
                    Class dbSourceClass = dbSourceClasses.Find(c => c.ClassId == classObj.Id);

                    //check result
                    if (dbSourceClass == null)
                    {
                        //source class was not found
                        //should never happen
                        //go to next class
                        continue;
                    }

                    //check if there is already a target class
                    //find target class by subject code
                    Class dbTargetClass = dbTargetClasses.Find(
                        c => c.SubjectCode == dbSourceClass.SubjectCode);

                    //check result
                    if (dbTargetClass != null)
                    {
                        //there is already a target class
                        //class was already imported
                        //go to next class
                        continue;
                    }

                    //copy source class into target class
                    dbTargetClass = dbSourceClass.Copy();

                    //set target class
                    dbTargetClass.ClassId = -1;
                    dbTargetClass.ClassStatus = (int)ItemStatus.Active;
                    dbTargetClass.Code = targetSemester.Description +
                        "." + dbTargetClass.SubjectCode.ToString("00000");
                    dbTargetClass.CreationTime = DateTime.Now;
                    dbTargetClass.InactivationReason = string.Empty;
                    dbTargetClass.InactivationTime = DateTime.MinValue;
                    dbTargetClass.SemesterId = targetSemester.Id;

                    //save class with transaction
                    dbTargetClass.Save(transaction);

                    //check registration option
                    if (registrationOption == 0)
                    {
                        //no need to import registrations
                        //go to next class
                        continue;
                    }

                    //get source class registrations
                    List<Registration> dbSourceRegistrations = 
                        Registration.FindByClass(dbSourceClass.ClassId, -1);

                    //check result
                    if (dbSourceRegistrations == null)
                    {
                        //class has no registration
                        //go to next class
                        continue;
                    }

                    //keep track of next registration position
                    int nextPosition = 0;

                    //check each registration
                    foreach (Registration dbSourceRegistration in dbSourceRegistrations)
                    {
                        //check if registration should not be renewed
                        if (registrationOption == 2 && !dbSourceRegistration.AutoRenewal)
                        {
                            //registration is not marked for auto renewel
                            //go to next registration
                            continue;
                        }

                        //check registration status
                        if (dbSourceRegistration.RegistrationStatus != (int)ItemStatus.Active)
                        {
                            //registration is not active anymore
                            //go to next registration
                            continue;
                        }

                        //check if registration should not be renewed
                        if (!dbSourceRegistration.AutoRenewal)
                        {
                            //registration should not be renewed
                            //go to next registration
                            continue;
                        }

                        //copy source registration into target registration
                        Registration dbTargetRegistration = dbSourceRegistration.Copy();

                        //set target registration
                        dbTargetRegistration.RegistrationId = -1;
                        dbTargetRegistration.ClassId = dbTargetClass.ClassId;
                        dbTargetRegistration.CreationTime = DateTime.Now;
                        dbTargetRegistration.InactivationReason = string.Empty;
                        dbTargetRegistration.InactivationTime = DateTime.MinValue;
                        dbTargetRegistration.Position = nextPosition++;

                        //save registration with transaction
                        dbTargetRegistration.Save(transaction);
                    }
                }

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving classes
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return saveResult;
        }

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
        public DeleteResult InactivateClass(int classId, string inactivationReason)
        {
            //the inactivate result to be returned
            DeleteResult inactivateResult = new DeleteResult();

            try
            {
                //inactivate selected class
                if (Class.Inactivate(classId, inactivationReason))
                {
                    //class was inactivated
                    //set result
                    inactivateResult.Result = (int)SelectResult.Success;
                }
                else
                {
                    //class was not found
                    //set result
                    inactivateResult.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while inactivating class
                //set result
                inactivateResult.Result = (int)SelectResult.FatalError;
                inactivateResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    inactivateResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return inactivateResult;
        }

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
        public List<IdDescriptionStatus> ListClassesByFilter(
            int filterClassStatus, int filterClassType, int filterInstrumentType,
            int filterClassLevel, int filterSemester, int filterInstitution,
            int filterPole, int filterTeacher)
        {
            //create result list
            List<IdDescriptionStatus> classes = new List<IdDescriptionStatus>();

            try
            {
                //get list of classes from database using filters
                List<Class> dbClasses = Class.FindByFilter(
                    filterClassStatus, filterClassType, filterInstrumentType,
                    filterClassLevel, filterSemester, filterInstitution, filterPole, filterTeacher);

                //check result
                if (dbClasses == null || dbClasses.Count == 0)
                {
                    //no class was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    classes.Add(resultItem);
                }
                else
                {
                    //check each class
                    foreach (Class dbClass in dbClasses)
                    {
                        //get description
                        IdDescriptionStatus item = dbClass.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        classes.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading classes
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                classes.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return classes;
        }

        /// <summary>
        /// Get list of class descriptions.
        /// </summary>
        /// <returns>
        /// The list of class descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListClasses()
        {
            //create result list
            List<IdDescriptionStatus> classes = new List<IdDescriptionStatus>();

            try
            {
                //get list of all classes from database
                List<Class> dbClasses = Class.Find();

                //check result
                if (dbClasses == null || dbClasses.Count == 0)
                {
                    //no class was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    classes.Add(resultItem);
                }
                else
                {
                    //check each class
                    foreach (Class dbClass in dbClasses)
                    {
                        //get description
                        IdDescriptionStatus item = dbClass.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        classes.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading classes
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                classes.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return classes;
        }

        /// <summary>
        /// Get list of class descriptions that belong to selected institution
        /// and match selected status.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <param name="status">
        /// The status of the returned classes.
        /// -1 to return all classes.
        /// </param>
        /// <returns>
        /// The list of class descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListClassesByInstitution(int institutionId, int status)
        {
            //create result list
            List<IdDescriptionStatus> classes = new List<IdDescriptionStatus>();

            try
            {
                //get list of classes from database
                List<Class> dbClasss = Class.FindByFilter(
                    status, -1, -1, -1, -1, institutionId, -1, -1);

                //check result
                if (dbClasss == null || dbClasss.Count == 0)
                {
                    //no class was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    classes.Add(resultItem);
                }
                else
                {
                    //check each class
                    foreach (Class dbClass in dbClasss)
                    {
                        //get description
                        IdDescriptionStatus item = dbClass.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        classes.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading classes
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                classes.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return classes;
        }

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
        public SaveResult SaveClass(
            Class classObj, List<Registration> registrations, List<Attendance> attendances)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //save class and get row ID
                saveResult.SavedId = classObj.Save(transaction);

                //check registrations
                if (registrations == null)
                {
                    //create empty list
                    registrations = new List<Registration>();
                }

                //check each registration
                foreach (Registration registration in registrations)
                {
                    //set class ID to registration
                    registration.ClassId = saveResult.SavedId;
                }

                //get list of registrations for saved class
                List<Registration> dbRegistrations = classObj.ClassId > 0 ? 
                    Registration.FindByClass(transaction, classObj.ClassId, -1) : null;

                //check result
                if (dbRegistrations == null)
                {
                    //no registration in database
                    //set empty list
                    dbRegistrations = new List<Registration>();
                }

                //check each assigned registration
                foreach (Registration registration in registrations)
                {
                    //check if registration has id
                    if (registration.RegistrationId > 0)
                    {
                        //find database registration
                        Registration dbRegistration = dbRegistrations.Find(
                            r => r.RegistrationId == registration.RegistrationId);

                        //check result
                        if (dbRegistration != null &&
                            dbRegistration.AutoRenewal == registration.AutoRenewal &&
                            dbRegistration.Position == registration.Position &&
                            dbRegistration.RegistrationStatus == registration.RegistrationStatus &&
                            dbRegistration.InactivationReason == registration.InactivationReason &&
                            dbRegistration.InactivationTime == registration.InactivationTime)
                        {
                            //registration data has not changed
                            //go to next registration
                            continue;
                        }
                    }

                    //save registration in database
                    registration.Save(transaction);
                }

                //get current semester
                Semester currentSemester = FindCurrentSemester();

                //check result
                if (currentSemester.Result != (int)SelectResult.Success)
                {
                    //error while getting current semester
                    //throw an exception
                    throw new ApplicationException(currentSemester.ErrorMessage);
                }

                //must remove old registrations
                //check each previous registrations
                foreach (Registration dbRegistration in dbRegistrations)
                {
                    //check if registration is not in the new list
                    if (registrations.Find(r => r.RegistrationId == dbRegistration.RegistrationId) == null)
                    {
                        //safety procedure
                        //check if class and registration is for a past semester
                        if (classObj.SemesterId < currentSemester.SemesterId)
                        {
                            //cannot delete registration
                            continue;
                        }

                        //delete any attendance for registration class student
                        Attendance.DeleteForClassStudent(
                            transaction, dbRegistration.ClassId, dbRegistration.StudentId);

                        //delete any grade for registration class student
                        Grade.DeleteForClassStudent(
                            transaction, dbRegistration.ClassId, dbRegistration.StudentId);

                        //delete registration from database
                        Registration.Delete(transaction, dbRegistration.RegistrationId);
                    }
                }

                //check attendances
                if (attendances != null)
                {
                    //check each attendance
                    foreach (Attendance attendance in attendances)
                    {
                        //check if attendance student is still registered
                        if (registrations.Find(r => r.StudentId == attendance.StudentId) == null)
                        {
                            //student is no longer registered
                            //might happen if new attendance is being registered
                            //while deleting registration
                            //go to next attendance
                            continue;
                        }

                        //set class ID to attendance
                        attendance.ClassId = saveResult.SavedId;

                        //save attendance in database
                        attendance.Save(transaction);
                    }
                }

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving class
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return saveResult;
        }

        #endregion Class


        #region Email *****************************************************************

        /// <summary>
        /// Indicates if server is currently processing email events.
        /// </summary>
        private static bool isProcessingEmailEvents = false;

        /// <summary>
        /// List of events sent to be processed and sent by email.
        /// </summary>
        private static Queue<Event> emailEvents = new Queue<Event>();

        /// <summary>
        /// Add a new event to be processed and sent by email.
        /// </summary>
        /// <param name="eventObj">
        /// The new event to be added.
        /// </param>
        public static void AddEmailEvent(Event eventObj)
        {
            //lock list of email events
            lock (emailEvents)
            {
                //add event to list
                emailEvents.Enqueue(eventObj);

                //check if there is no thread processing events right now
                if (!isProcessingEmailEvents)
                {
                    try
                    {
                        //update processing flag
                        isProcessingEmailEvents = true;

                        //process events on a thread of the pool
                        System.Threading.ThreadPool.QueueUserWorkItem(ProcessEmailEvents);
                    }
                    catch
                    {
                        //could not queue work item
                        //reset processing flag
                        isProcessingEmailEvents = false;

                        //rethrow exception up
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Process events on the list of event emails
        /// and send them by email.
        /// </summary>
        private static void ProcessEmailEvents(Object stateInfo)
        {
            //process email events on the list
            Event emailEvent = null;

            //keep processing
            while (true)
            {
                try
                {
                    //lock list of email events
                    lock (emailEvents)
                    {
                        //check if there is an event on the list
                        if (emailEvents.Count == 0)
                        {
                            //there is no event
                            //reset processing flag
                            isProcessingEmailEvents = false;

                            //exit
                            return;
                        }

                        //get next email event and remove it from list
                        emailEvent = emailEvents.Dequeue();
                    }

                    //send event by email
                    SendEventEmail(emailEvent);
                }
                catch
                {
                    //unexpected error
                    //lock list of email events
                    lock (emailEvents)
                    {
                        //clear email events
                        emailEvents.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Send an email to selected destinations.
        /// </summary>
        /// <param name="to">
        /// The list of recipients of the sent email.
        /// </param>
        /// <param name="subject">
        /// The subject of the e-mail.
        /// </param>
        /// <param name="body">
        /// The body of the e-mail. In HTML format.
        /// </param>
        /// <param name="priority">
        /// The priority of the e-mail.
        /// </param>
        /// <returns>
        /// The send result.
        /// </returns>
        private static SendResult SendEmail(
            List<string> to, string subject, string body, MailPriority priority)
        {
            //create result
            SendResult sendResult = new SongDB.Logic.SendResult();

            //check sent subject
            if (subject == null || subject.Length == 0)
            {
                //empty subject
                //set result
                sendResult.Result = (int)SelectResult.FatalError;

                //set error message
                sendResult.ErrorMessage = "No subject is set.";
                
                //return result
                return sendResult;
            }

            //check sent body
            if (body == null || body.Length == 0)
            {
                //empty body
                //set result
                sendResult.Result = (int)SelectResult.FatalError;

                //set error message
                sendResult.ErrorMessage = "Empty e-mail.";

                //return result
                return sendResult;
            }

            //check email settings before creating email
            if (ApplicationSettings.SmtpAddress == null ||
                ApplicationSettings.SmtpAddress.Length == 0 ||
                ApplicationSettings.SmtpFrom == null ||
                ApplicationSettings.SmtpFrom.Length == 0 ||
                ApplicationSettings.SmtpUser == null ||
                ApplicationSettings.SmtpUser.Length == 0 ||
                ApplicationSettings.SmtpPassword == null ||
                ApplicationSettings.SmtpPassword.Length == 0)
            {
                //smtp server is not set
                //set result
                sendResult.Result = (int)SelectResult.FatalError;

                //set error message
                sendResult.ErrorMessage = "SMTP server is not set correctly.";

                //return result
                return sendResult;
            }

            try
            {
                //create a new email message
                MailMessage email = new MailMessage();

                //set from field
                email.From = new MailAddress(
                    (ApplicationSettings.SmtpFrom != string.Empty) ?
                    ApplicationSettings.SmtpFrom : ApplicationSettings.SmtpUser,
                    ApplicationSettings.SmtpDisplayName);

                //add recipients
                foreach (string recipient in to)
                {
                    //add recipient
                    email.To.Add(recipient);
                }

                //set HTML
                email.IsBodyHtml = true;

                //set email subject
                email.Subject = subject;

                //set email body and add a footer
                email.Body = body;

                //set priority
                email.Priority = priority;

                //create and set SMTP client
                SmtpClient smtp = new SmtpClient(
                    ApplicationSettings.SmtpAddress, ApplicationSettings.SmtpPort);

                //set SSL option
                smtp.EnableSsl = ApplicationSettings.SmtpSSL;

                //check credentials
                if (ApplicationSettings.SmtpUser.Length > 0 &&
                    ApplicationSettings.SmtpPassword.Length > 0)
                {
                    //set user credentials
                    smtp.Credentials = new System.Net.NetworkCredential(
                        ApplicationSettings.SmtpUser, ApplicationSettings.SmtpPassword);
                }

                //send email
                smtp.Send(email);

                //email was sent
                sendResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //unexpected error while sending email
                //set result
                sendResult.Result = (int)SelectResult.FatalError;

                //set error message
                sendResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    sendResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return sendResult;
        }

        /// <summary>
        /// Send an event email to users according to selected send option.
        /// </summary>
        /// <param name="eventObj">The announced event.</param>
        /// <param name="isUpdate">
        /// True if event was updated and email is being resent.
        /// False if event is new and email is being sent for the first time.
        /// </param>
        /// <returns>
        /// The send result.
        /// </returns>
        private static SendResult SendEventEmail(Event eventObj)
        {
            //create result
            SendResult sendResult = new SongDB.Logic.SendResult();

            try
            {
                //list of users to send email to
                List<User> users = null;

                //get target name
                string targetName = "";

                //check send option
                if (eventObj.EventSendOption == (int)EventSendOption.ToNoOne)
                {
                    //no need to send email
                    //set and return result
                    sendResult.Result = (int)SelectResult.Success;
                    return sendResult;
                }
                else if (eventObj.EventSendOption == (int)EventSendOption.ToEveryone)
                {
                    //get all active users
                    users = User.FindByFilter((int)ItemStatus.Active, -1, -1);

                    //check result
                    if (users == null)
                    {
                        //create empty list
                        users = new List<User>();
                    }
                }
                else if (eventObj.EventSendOption == (int)EventSendOption.ToInstitution)
                {
                    //get all active users for now
                    users = User.FindByFilter((int)ItemStatus.Active, -1, -1);

                    //check result
                    if (users == null)
                    {
                        //create empty list
                        users = new List<User>();
                    }

                    //check if there is a selected institution
                    if (eventObj.InstitutionId > 0)
                    {
                        //must find all institution teacher users
                        List<User> teacherUsers = new List<User>();

                        //find teachers
                        List<Teacher> teachers = Teacher.FindByFilter(
                            -1, eventObj.InstitutionId, -1);

                        //check result
                        if (teachers != null)
                        {
                            //check each teacher
                            foreach (Teacher teacher in teachers)
                            {
                                //get teacher user
                                User teacherUser = users.Find(
                                    u => u.UserId == teacher.UserId);

                                //check result
                                if (teacherUser != null)
                                {
                                    //add it to the list of teacher users
                                    teacherUsers.Add(teacherUser);
                                }
                            } 
                        }

                        //filter users that are from selected institution
                        users = users.FindAll(u => u.InstitutionId == eventObj.InstitutionId);

                        //add teacher users to list
                        //doesn't matter if same user is twice in the list
                        users.AddRange(teacherUsers);
                    }
                }
                else if (eventObj.EventSendOption == (int)EventSendOption.ToCoordinators)
                {
                    //set target name
                    targetName = "Coordenador";

                    //get all assigned coordinator users
                    users = User.FindAssignedCoordinators();

                    //check result
                    if (users == null)
                    {
                        //create empty list
                        users = new List<User>();
                    }
                }
                else if (eventObj.EventSendOption == (int)EventSendOption.ToTeachers)
                {
                    //set target name
                    targetName = "Professor";

                    //get all assigned teacher users
                    users = User.FindTeacher();

                    //check result
                    if (users == null)
                    {
                        //create empty list
                        users = new List<User>();
                    }
                }
                else
                {
                    //should never happen
                    //create empty list
                    users = new List<User>();
                }

                //filter not active users out
                users = users.FindAll(u => u.UserStatus == (int)ItemStatus.Active);

                //check result
                if (users == null || users.Count == 0)
                {
                    //no user is selected
                    //no need to send email
                    //set and return result
                    sendResult.Result = (int)SelectResult.Success;
                    return sendResult;
                }

                //create list of emails
                List<string> emails = new List<string>();

                //add each user email
                foreach (User user in users)
                {
                    //check email
                    if (user.Email == null ||
                        user.Email.Length == 0)
                    {
                        //go to next user
                        continue;
                    }

                    //check if email is already in the list
                    if (emails.Contains(user.Email))
                    {
                        //go to next user
                        continue;
                    }

                    //add email
                    emails.Add(user.Email);
                }

                //sort emails
                emails.Sort();

#if DEBUG
                emails.Clear();
                emails.Add("gmalizia@gmail.com");
#endif

                //create title
                string title = (eventObj.MarkEmailAsNew ? "[Novo] " : "[Atualizado] ") + "Evento - " + eventObj.Name;

                //create email body
                StringBuilder sbBody = new StringBuilder(1024);

                //write html document
                sbBody.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional //EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                sbBody.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">");

                //write header
                WriteEmailHeader(title, sbBody);

                //write body
                sbBody.Append("<body style=\"margin:0; padding:0;\" bgcolor=\"#F0F0F0\" leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\">");

                //write main mesage
                sbBody.Append("<table border=\"0\" width=\"100%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"#F0F0F0\">");
                sbBody.Append("  <tr>");
                sbBody.Append("    <td align=\"center\" valign=\"top\" bgcolor=\"#F0F0F0\" style=\"background-color: #F0F0F0;\">");
                sbBody.Append("      <br />");
                sbBody.Append("      <table border=\"0\" width=\"600\" cellpadding=\"0\" cellspacing=\"0\" class=\"container\" style=\"width:600px;max-width:600px\">");
                sbBody.Append("        <tr>");
                sbBody.Append("          <td class=\"container-padding content\" align=\"left\" style=\"padding-left:24px;padding-right:24px;padding-top:12px;padding-bottom:12px;background-color:#ffffff\">");
                sbBody.Append("            <br />");
                sbBody.Append("            <div class=\"body-text\"  style=\"font-family:Arial, sans-serif;font-size:14px;line-height:20px;text-align:left;color:#333333\">");
                sbBody.Append("                Olá" + (targetName.Length > 0 ? " " + targetName : "") + ", ");
                sbBody.Append("                <br /><br />");
                sbBody.Append("                Você foi convidado a participar do seguinte evento a ser realizado em breve.");
                sbBody.Append("                <br /><br />");
                sbBody.Append("                <strong>Nome: </strong> " + eventObj.Name + " <br />");
                sbBody.Append("                <strong>Projeto: </strong> " + (eventObj.InstitutionId > 0 ? eventObj.InstitutionName : "Todos") + " <br />");
                sbBody.Append("                <strong>Data e Hora: </strong> " + eventObj.StartTime.ToString("dd/MM/yyyy, HH:mm") + " - " + (eventObj.StartTime.AddMinutes(eventObj.Duration)).ToString("HH:mm") + " <br />");
                sbBody.Append("                <strong>Local: </strong> " + eventObj.Location + " <br />");
                sbBody.Append("                <strong>Descrição: </strong> " + (eventObj.Description != null && eventObj.Description.Length > 0 ? eventObj.Description : "-"));
                sbBody.Append("                <br /><br />");
                sbBody.Append("                Você pode consultar este e outros eventos no aplicativo do Song. ");
                sbBody.Append("                Nunca divulgue os seus dados de acesso para outra pessoa.");
                sbBody.Append("                <br /><br />");
                sbBody.Append("                Atenciosamente, ");
                sbBody.Append("                <br />");
                sbBody.Append("                Agência do Bem");
                sbBody.Append("            </div>");
                sbBody.Append("          </td>");
                sbBody.Append("        </tr>");

                //write footer message
                sbBody.Append("        <tr>");
                sbBody.Append("            <td class=\"container-padding footer-text\" align=\"left\"  style=\"font-family:Arial, sans-serif;font-size:12px;line-height:16px;color:#aaaaaa;padding-left:24px;padding-right:24px\">");
                sbBody.Append("                <br />");
                sbBody.Append("                <strong>Importante: </strong> esta é uma mensagem automática e não deve ser respondida. <br />");
                sbBody.Append("                Para enviar a sua sugestão ou solicitação para Agência do Bem, por favor, envie um e-mail para <a href=\"contato@agenciadobem.org.br\" style=\"color:#aaaaaa\">contato</a>.");
                sbBody.Append("                <br /><br />");
                sbBody.Append("            </td>");
                sbBody.Append("        </tr>");
                sbBody.Append("      </table>");
                sbBody.Append("    </td>");
                sbBody.Append("  </tr>");
                sbBody.Append("</table>");

                //end body
                sbBody.Append("</body>");

                //end html document
                sbBody.Append("</html> ");

                //send email to user and get result
                sendResult = SendEmail(
                    emails, title, sbBody.ToString(), MailPriority.Low);
            }
            catch (Exception ex)
            {
                //unexpected error while sending email
                //set result
                sendResult.Result = (int)SelectResult.FatalError;

                //set error message
                sendResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    sendResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return sendResult;
        }

        /// <summary>
        /// Send an email to selected user with generated recovery password.
        /// </summary>
        /// <param name="userId">The ID of the selected user.</param>
        /// <param name="recoveryPassword">The generated recovery password.</param>
        /// <returns>
        /// The send result.
        /// </returns>
        private SendResult SendRecoveryPasswordEmail(int userId, string recoveryPassword)
        {
            //create result
            SendResult sendResult = new SongDB.Logic.SendResult();

            try
            {
                //get user
                User user = User.Find(userId);

                //check result
                if (user == null)
                {
                    //user was not found
                    //should never happen as code was already generated
                    //set result
                    sendResult.Result = (int)SelectResult.FatalError;
                    sendResult.ErrorMessage = "User was not found in database.";

                    //return result
                    return sendResult;
                }

                //create email body
                StringBuilder sbBody = new StringBuilder(1024);

                //write html document
                sbBody.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional //EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                sbBody.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">");
                
                //write header
                WriteEmailHeader("Recuperação de Senha", sbBody);
                
                //write body
                sbBody.Append("<body style=\"margin:0; padding:0;\" bgcolor=\"#F0F0F0\" leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\">");

                //write main mesage
                sbBody.Append("<table border=\"0\" width=\"100%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"#F0F0F0\">");
                sbBody.Append("  <tr>");
                sbBody.Append("    <td align=\"center\" valign=\"top\" bgcolor=\"#F0F0F0\" style=\"background-color: #F0F0F0;\">");
                sbBody.Append("      <br />");
                sbBody.Append("      <table border=\"0\" width=\"600\" cellpadding=\"0\" cellspacing=\"0\" class=\"container\" style=\"width:600px;max-width:600px\">");
                sbBody.Append("        <tr>");
                sbBody.Append("          <td class=\"container-padding content\" align=\"left\" style=\"padding-left:24px;padding-right:24px;padding-top:12px;padding-bottom:12px;background-color:#ffffff\">");
                sbBody.Append("            <br />");
                sbBody.Append("            <div class=\"body-text\"  style=\"font-family:Arial, sans-serif;font-size:14px;line-height:20px;text-align:left;color:#333333\">");
                sbBody.Append("                Olá " + user.Name + ", ");
                sbBody.Append("                <br /><br />");
                sbBody.Append("                Você requisitou a recuperação de sua senha no Song. Os seus dados de acesso ao sistema são:");
                sbBody.Append("                <br /><br />");
                sbBody.Append("                <strong>Login: </strong> " + user.Login + " <br />");
                sbBody.Append("                <strong>Senha: </strong> " + recoveryPassword);
                sbBody.Append("                <br /><br />");
                sbBody.Append("                Após o seu próximo login, você será requisitado a escolher uma nova senha. ");
                sbBody.Append("                Nunca divulgue os seus dados de acesso para outra pessoa.");
                sbBody.Append("                <br /><br />");
                sbBody.Append("                Atenciosamente, ");
                sbBody.Append("                <br />");
                sbBody.Append("                Agência do Bem");
                sbBody.Append("            </div>");
                sbBody.Append("          </td>");
                sbBody.Append("        </tr>");

                //write footer message
                sbBody.Append("        <tr>");
                sbBody.Append("            <td class=\"container-padding footer-text\" align=\"left\"  style=\"font-family:Arial, sans-serif;font-size:12px;line-height:16px;color:#aaaaaa;padding-left:24px;padding-right:24px\">");
                sbBody.Append("                <br />");
                sbBody.Append("                Você está recebendo este e-mail por ter requisitado a recuperação de sua senha no Song.<br />");
                sbBody.Append("                Caso você não o tenha feito, por favor comunique o recebimento deste e-mail à Agência do Bem o quanto antes.");
                sbBody.Append("                <br /><br />");
                sbBody.Append("                <strong>Importante: </strong> esta é uma mensagem automática e não deve ser respondida. <br />");
                sbBody.Append("                Para enviar a sua sugestão ou solicitação para Agência do Bem, por favor, envie um e-mail para <a href=\"contato@agenciadobem.org.br\" style=\"color:#aaaaaa\">contato</a>.");
                sbBody.Append("                <br /><br />");
                sbBody.Append("            </td>");
                sbBody.Append("        </tr>");
                sbBody.Append("      </table>");
                sbBody.Append("    </td>");
                sbBody.Append("  </tr>");
                sbBody.Append("</table>");

                //end body
                sbBody.Append("</body>");

                //end html document
                sbBody.Append("</html> ");

                //send email to user and get result
                sendResult =  SendEmail(
                    new List<string>() { user.Email }, "Recuperação de Senha", 
                    sbBody.ToString(), MailPriority.High);
            }
            catch (Exception ex)
            {
                //unexpected error while sending email
                //set result
                sendResult.Result = (int)SelectResult.FatalError;

                //set error message
                sendResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    sendResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return sendResult;
        }

        /// <summary>
        /// Write HTML header to selected email being built.
        /// </summary>
        /// <param name="title">The title of the email header.</param>
        /// <param name="email">
        /// The selected email builder.
        /// </param>
        private static void WriteEmailHeader(string title, StringBuilder email)
        {
            //write header
            email.Append("<head>");

            //write style
            email.Append("    <style type=\"text/css\">");
            email.Append("    body { margin: 0; padding: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; }");
            email.Append("    table { border-spacing: 0; }");
            email.Append("    table td { border-collapse: collapse; }");
            email.Append("    .ExternalClass { width: 100%; }");
            email.Append("    .ExternalClass,");
            email.Append("    .ExternalClass p,");
            email.Append("    .ExternalClass span,");
            email.Append("    .ExternalClass font,");
            email.Append("    .ExternalClass td,");
            email.Append("    .ExternalClass div { line-height: 100%; }");
            email.Append("    .ReadMsgBody { width: 100%; background-color: #ebebeb; }");
            email.Append("    table { mso-table-lspace: 0pt; mso-table-rspace: 0pt; }");
            email.Append("    img { -ms-interpolation-mode: bicubic; }");
            email.Append("    .yshortcuts a { border-bottom: none !important; }");
            email.Append("    @media screen and (max-width: 599px) {");
            email.Append("    	.force-row, .container { width: 100% !important; max-width: 100% !important; } }");
            email.Append("    @media screen and (max-width: 400px) {");
            email.Append("        .container-padding { padding-left: 12px !important; padding-right: 12px !important; } }");
            email.Append("    .ios-footer a { color: #aaaaaa !important; text-decoration: underline; }");
            email.Append("    </style>");

            //write title
            email.Append("    <title>" + title + "</title>");

            //end header
            email.Append("</head>");
        }

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
        public CountResult CountEventsByFilter(
            int filterInstitution, DateTime filterStartDate, DateTime filterEndDate)
        {
            //create result
            CountResult countResult = new CountResult();

            try
            {
                //count events in database and set result
                countResult.Count = Event.CountByFilter(
                    filterInstitution, filterStartDate, filterEndDate);
            }
            catch (Exception ex)
            {
                //error while couting registrations
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return countResult;
        }

        /// <summary>
        /// Delete event by ID.
        /// </summary>
        /// <param name="eventId">
        /// The ID of the selected event.
        /// </param>
        /// <returns>
        /// The delete operation result.
        /// </returns>
        public DeleteResult DeleteEvent(int eventId)
        {
            //the delete result to be returned
            DeleteResult deleteResult = new DeleteResult();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //delete selected event
                if (Event.Delete(transaction, eventId))
                {
                    //event was inactivated
                    //set result
                    deleteResult.Result = (int)SelectResult.Success;
                }
                else
                {
                    //event was not found
                    //set result
                    deleteResult.Result = (int)SelectResult.Empty;
                }

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                //error while deleting event
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                deleteResult.Result = (int)SelectResult.FatalError;
                deleteResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    deleteResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return deleteResult;
        }

        /// <summary>
        /// Find event by ID.
        /// </summary>
        /// <param name="eventId">
        /// The ID of the selected event.
        /// </param>
        /// <returns>
        /// The selected event.
        /// </returns>
        public Event FindEvent(int eventId)
        {
            //the target event
            Event resultEvent = null;

            try
            {
                //find event in database
                resultEvent = Event.Find(eventId);

                //check result
                if (resultEvent != null)
                {
                    //event was found
                    resultEvent.Result = (int)SelectResult.Success;
                }
                else
                {
                    //event was not found
                    //create result and set it
                    resultEvent = new Event();
                    resultEvent.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding event
                //create result and set it
                resultEvent = new Event();
                resultEvent.Result = (int)SelectResult.FatalError;
                resultEvent.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultEvent.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultEvent;
        }

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
        public List<Event> FindEventsByFilter(bool loadInstitution,
            int filterInstitution, DateTime filterStartDate, DateTime filterEndDate)
        {
            //create result list
            List<Event> events = new List<Event>();

            try
            {
                //get list of events from database using filters
                List<Event> dbEvents = Event.FindByFilter(
                    filterInstitution, filterStartDate, filterEndDate);

                //check result
                if (dbEvents == null || dbEvents.Count == 0)
                {
                    //no event was found
                    //create result and add it to the list
                    Event resultEvent = new Event();
                    resultEvent.Result = (int)SelectResult.Empty;
                    events.Add(resultEvent);
                }
                else
                {
                    //check each event
                    foreach (Event dbEvent in dbEvents)
                    {
                        //set and add event
                        dbEvent.Result = (int)SelectResult.Success;
                        events.Add(dbEvent);
                    }

                    //check if institution should be loaded
                    if (loadInstitution)
                    {
                        //get list of institutions
                        List<Institution> institutions = Institution.Find();

                        //check result
                        if (institutions != null)
                        {
                            //check each event
                            foreach (Event eventObj in events)
                            {
                                //find institution
                                Institution institution = institutions.Find(
                                    i => i.InstitutionId == eventObj.InstitutionId);

                                //check result
                                if (institution != null)
                                {
                                    //set institution name
                                    eventObj.InstitutionName = institution.ProjectName;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading events
                //create result and add it to the list
                Event resultEvent = new Event();
                resultEvent.Result = (int)SelectResult.FatalError;
                resultEvent.ErrorMessage = ex.Message;
                events.Add(resultEvent);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultEvent.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return events;
        }

        /// <summary>
        /// Get list of event descriptions.
        /// </summary>
        /// <returns>
        /// The list of event descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListEvents()
        {
            //create result list
            List<IdDescriptionStatus> events = new List<IdDescriptionStatus>();

            try
            {
                //get list of all events from database
                List<Event> dbEvents = Event.Find();

                //check result
                if (dbEvents == null || dbEvents.Count == 0)
                {
                    //no event was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    events.Add(resultItem);
                }
                else
                {
                    //check each event
                    foreach (Event dbEvent in dbEvents)
                    {
                        //get description
                        IdDescriptionStatus item = dbEvent.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        events.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading events
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                events.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return events;
        }

        /// <summary>
        /// Get list of event descriptions that belong to selected institution.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <returns>
        /// The list of event descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListEventsByInstitution(int institutionId)
        {
            //create result list
            List<IdDescriptionStatus> events = new List<IdDescriptionStatus>();

            try
            {
                //get list of events from database
                List<Event> dbEvents = Event.FindByFilter(
                    institutionId, DateTime.MinValue, DateTime.MinValue);

                //check result
                if (dbEvents == null || dbEvents.Count == 0)
                {
                    //no event was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    events.Add(resultItem);
                }
                else
                {
                    //check each event
                    foreach (Event dbEvent in dbEvents)
                    {
                        //get description
                        IdDescriptionStatus item = dbEvent.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        events.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading events
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                events.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return events;
        }

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
        public SaveResult SaveEvent(Event eventObj)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //get if event is new or being updated
                bool isNew = (eventObj.EventId <= 0);

                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //save event and get row ID
                saveResult.SavedId = eventObj.Save(transaction);

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                saveResult.Result = (int)SelectResult.Success;

                //set email option
                eventObj.MarkEmailAsNew = isNew;

                //send event email using another thread
                AddEmailEvent(eventObj);
            }
            catch (Exception ex)
            {
                //error while saving event
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return saveResult;
        }

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
        public File GetFile(string file)
        {
            //the target file
            File resultFile = new File();

            //set file path
            resultFile.FilePath = file;

            try
            {
                //update file path
                file = AppDomain.CurrentDomain.BaseDirectory + FILE_DIR_PATH + "\\" + file;

                //check if file exists
                if (System.IO.File.Exists(file))
                {
                    //read file data
                    resultFile.Data = System.IO.File.ReadAllBytes(file);

                    //set result
                    resultFile.Result = (int)SelectResult.Success;
                }
                else
                {
                    //file was not found
                    //set result
                    resultFile.Result = (int)SelectResult.Empty;
                    resultFile.ErrorMessage = "File not found.";
                }
            }
            catch (Exception ex)
            {
                //error while loading thumbnail file
                //set result
                resultFile.Result = (int)SelectResult.FatalError;
                resultFile.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultFile.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultFile;
        }

        /// <summary>
        /// Get thumbnail file for selected file.
        /// </summary>
        /// <param name="file">
        /// The selected file.
        /// </param>
        /// <returns>
        /// The thumbnail file.
        /// </returns>
        public File GetFileThumbnail(string file)
        {
            //the target thumbnail file
            File resultThumbnail = new File();

            try
            {
                //get thumbnail file path
                string thumbnailFile = file.Substring(
                    0, file.LastIndexOf('.')) + ".thumbnail.jpg";

                //set file path
                resultThumbnail.FilePath = thumbnailFile;

                //check if file is an image file
                if (!file.EndsWith(".jpg") && 
                    !file.EndsWith(".jpeg") &&
                    !file.EndsWith(".png") &&
                    !file.EndsWith(".tif") &&
                    !file.EndsWith(".tiff"))
                {
                    //file is not an image file
                    //set result
                    resultThumbnail.ErrorMessage = "File is not an image file.";
                    resultThumbnail.Result = (int)SelectResult.FatalError;

                    //return result
                    return resultThumbnail;
                }

                //update file and thumbnail file paths
                file = AppDomain.CurrentDomain.BaseDirectory + FILE_DIR_PATH + "\\" + file;
                thumbnailFile = AppDomain.CurrentDomain.BaseDirectory + FILE_DIR_PATH + "\\" + thumbnailFile;

                //check if file exists
                if (!System.IO.File.Exists(file))
                {
                    //file was not found
                    //set result
                    resultThumbnail.ErrorMessage = "Original file not found.";
                    resultThumbnail.Result = (int)SelectResult.Empty;

                    //return result
                    return resultThumbnail;
                }

                //check if thumbnail file exists
                if (!System.IO.File.Exists(thumbnailFile))
                {
                    //must create thumbnail for image
                    //the original image
                    System.Drawing.Bitmap image = null;
                    
                    //open image file
                    using (System.IO.Stream stream = System.IO.File.Open(file, System.IO.FileMode.Open))
                    {
                        //read image
                        image =  new System.Drawing.Bitmap(System.Drawing.Image.FromStream(stream));
                    }

                    //calculate new width and height
                    int width, height;

                    //check if largest side
                    if (image.Width > image.Height)
                    {
                        //set new sizes keeping aspect ratio
                        width = 200;
                        height = (int)(image.Height * (200.0/((double)image.Width)));
                    }
                    else
                    {
                        //set new sizes keeping aspect ratio
                        width = (int)(image.Width * (200.0 / ((double)image.Height)));
                        height = 200;
                    }

                    //create a resampled image
                    System.Drawing.Bitmap resampledImage = 
                        new System.Drawing.Bitmap(width, height);

                    //paint on the resampled image
                    using (System.Drawing.Graphics G = 
                        System.Drawing.Graphics.FromImage(resampledImage))
                    {
                        //draw original image with maximum quality
                        G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        G.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        G.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        G.DrawImage(image, 0, 0, width, height);
                    }

                    ////get enconder
                    //var encoder = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().First(
                    //    c => c.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid);
                    //var encParams = new System.Drawing.Imaging.EncoderParameters() {
                    //    Param = new[] { new System.Drawing.Imaging.EncoderParameter(
                    //        System.Drawing.Imaging.Encoder.Quality, 90L) } };

                    //save image
                    resampledImage.Save(thumbnailFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                //read file data
                resultThumbnail.Data = System.IO.File.ReadAllBytes(thumbnailFile);

                //set result
                resultThumbnail.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while loading thumbnail file
                //set result
                resultThumbnail.Result = (int)SelectResult.FatalError;
                resultThumbnail.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultThumbnail.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultThumbnail;
        }

        /// <summary>
        /// Save selected file to server disk.
        /// </summary>
        /// <param name="file">
        /// The file to be saved.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        public SaveResult SaveFile(File file)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            try
            {
                //create temporary directory if needed
                new System.IO.FileInfo(
                    AppDomain.CurrentDomain.BaseDirectory + 
                    FILE_DIR_PATH + "\\Temp\\" + file.FileName).Directory.Create();

                //write file to local disk
                System.IO.File.WriteAllBytes(
                    AppDomain.CurrentDomain.BaseDirectory +
                    FILE_DIR_PATH + "\\Temp\\" + file.FileName, file.Data);
                
                //create selected file directories if needed
                new System.IO.FileInfo(
                    AppDomain.CurrentDomain.BaseDirectory +
                    FILE_DIR_PATH + "\\" + file.FilePath).Directory.Create();

                //move file to place
                System.IO.File.Move(
                    AppDomain.CurrentDomain.BaseDirectory +
                    FILE_DIR_PATH + "\\Temp\\" + file.FileName,
                    AppDomain.CurrentDomain.BaseDirectory +
                    FILE_DIR_PATH + "\\" + file.FilePath);
                
                //check if file is an image file
                if (file.FilePath.EndsWith(".jpg") ||
                    file.FilePath.EndsWith(".jpeg") ||
                    file.FilePath.EndsWith(".png") ||
                    file.FilePath.EndsWith(".tif") ||
                    file.FilePath.EndsWith(".tiff"))
                {
                    //delete previous thumbnail if any
                    //set thumbnail file path
                    file.FilePath = file.FilePath.Substring(
                        0, file.FilePath.LastIndexOf('.')) + ".thumbnail.jpg";

                    //check if file exists
                    if (System.IO.File.Exists(file.FilePath))
                    {
                        //delete file
                        System.IO.File.Delete(file.FilePath);
                    }
                }

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving file
                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return saveResult;
        }

        #endregion File


        #region Grade *****************************************************************


        /// <summary>
        /// Average attendance score by filter.
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
        public AverageResult AverageGradesByFilter(
            int filterGradeRapporteur, int filterGradeTarget, int filterGradePeriodicity,
            int filterGradeSubject, int filterSemester, DateTime filterReferenceDate,
            int filterInstitution, int filterTeacher, int filterCoordinator,
            int filterStudent, int filterClass)
        {
            //create result
            AverageResult averageResult = new AverageResult();

            try
            {
                //average grades in database and set result
                averageResult.Average = Grade.AverageByFilter(
                    filterGradeRapporteur, filterGradeTarget, filterGradePeriodicity,
                    filterGradeSubject, filterSemester, filterReferenceDate,
                    filterInstitution, filterTeacher, filterCoordinator,
                    filterStudent, filterClass);
            }
            catch (Exception ex)
            {
                //error while loading grades
                //set result
                averageResult.Average = int.MinValue;
                averageResult.Result = (int)SelectResult.FatalError;
                averageResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    averageResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return averageResult;
        }

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
        public List<Grade> FindGradesByFilter(
            bool loadSemester, bool loadInstitution, bool loadTeacher,
            bool loadCoordinator, bool loadStudent, bool loadClass,
            int filterGradeRapporteur, int filterGradeTarget, int filterGradePeriodicity,
            int filterGradeSubject, int filterSemester, DateTime filterReferenceDate,
            int filterInstitution, int filterPole, int filterTeacher, 
            int filterCoordinator, int filterStudent, int filterClass)
        {
            //create result list
            List<Grade> grades = new List<Grade>();

            try
            {
                //get list of grades from database using filters
                List<Grade> dbGrades = Grade.FindByFilter(
                    filterGradeRapporteur, filterGradeTarget, filterGradePeriodicity,
                    filterGradeSubject, filterSemester, filterReferenceDate,
                    filterInstitution, filterPole, filterTeacher, filterCoordinator,
                    filterStudent, filterClass);

                //check result
                if (dbGrades == null || dbGrades.Count == 0)
                {
                    //no grade was found
                    //create result and add it to the list
                    Grade resultGrade = new Grade();
                    resultGrade.Result = (int)SelectResult.Empty;
                    grades.Add(resultGrade);
                }
                else
                {
                    //check each grade
                    foreach (Grade dbGrade in dbGrades)
                    {
                        //set and add grade
                        dbGrade.Result = (int)SelectResult.Success;
                        grades.Add(dbGrade);
                    }

                    //check if institution should be loaded
                    if (loadInstitution)
                    {
                        //check filter institution
                        if (filterInstitution > -1)
                        {
                            //get institution
                            Institution institution = Institution.Find(filterInstitution);

                            //check result
                            if (institution != null)
                            {
                                //check each grade
                                foreach (Grade gradeObj in grades)
                                {
                                    //set institution name
                                    gradeObj.InstitutionName = institution.ProjectName;
                                }
                            }
                        }
                        else
                        {

                            //get list of institutions
                            List<Institution> institutions = Institution.FindByFilter(-1);

                            //check result
                            if (institutions != null)
                            {
                                //check each grade
                                foreach (Grade gradeObj in grades)
                                {
                                    //find institution
                                    Institution institution = institutions.Find(
                                        i => i.InstitutionId == gradeObj.InstitutionId);

                                    //check result
                                    if (institution != null)
                                    {
                                        //set institution name
                                        gradeObj.InstitutionName = institution.ProjectName;
                                    }
                                }
                            }
                        }
                    }

                    //check if teacher should be loaded
                    if (loadTeacher)
                    {
                        //check filter teacher
                        if (filterTeacher > -1)
                        {
                            //get teacher
                            Teacher teacher = Teacher.Find(filterTeacher);

                            //check result
                            if (teacher != null)
                            {
                                //check each grade
                                foreach (Grade gradeObj in grades)
                                {
                                    //set teacher name
                                    gradeObj.TeacherName = teacher.Name;

                                    //set teacher user id
                                    gradeObj.TeacherUserId = teacher.UserId;
                                }
                            }
                        }
                        else
                        {

                            //get list of teachers using filters
                            List<Teacher> teachers = Teacher.FindByFilter(
                                -1, filterInstitution, -1);

                            //check result
                            if (teachers != null)
                            {
                                //check each grade
                                foreach (Grade gradeObj in grades)
                                {
                                    //check if grade has no teacher
                                    if (gradeObj.TeacherId == int.MinValue)
                                    {
                                        //go to next grade
                                        continue;
                                    }

                                    //find teacher
                                    Teacher teacher = teachers.Find(
                                        i => i.TeacherId == gradeObj.TeacherId);

                                    //check result
                                    if (teacher == null)
                                    {
                                        //teacher was not found
                                        //teacher might not belong to pole anymore
                                        //find teacher directly
                                        teacher = Teacher.Find(gradeObj.TeacherId);

                                        //check result
                                        if (teacher != null)
                                        {
                                            //add teacher to list of teachers
                                            teachers.Add(teacher);
                                        }
                                    }

                                    //check final result
                                    if (teacher != null)
                                    {
                                        //set teacher name
                                        gradeObj.TeacherName = teacher.Name;

                                        //set teacher user id
                                        gradeObj.TeacherUserId = teacher.UserId;
                                    }
                                    else
                                    {
                                        //should never happen
                                        //set teacher id as teacher name
                                        gradeObj.TeacherName = gradeObj.TeacherId.ToString();

                                        //reset teacher user id
                                        gradeObj.TeacherUserId = int.MinValue;
                                    }
                                }
                            }
                        }
                    }

                    //check if coordinator should be loaded
                    if (loadCoordinator)
                    {
                        //get list of coordinators using filters
                        List<User> users = User.FindAssignedCoordinators();

                        //check result
                        if (users != null)
                        {
                            //check each grade
                            foreach (Grade gradeObj in grades)
                            {
                                //check if grade has no coordinator
                                if (gradeObj.CoordinatorId == int.MinValue)
                                {
                                    //go to next grade
                                    continue;
                                }

                                //find coordinator
                                User coordinator = users.Find(
                                    i => i.UserId == gradeObj.CoordinatorId);

                                //check result
                                if (coordinator == null)
                                {
                                    //coordinator was not found
                                    //coordinator might not belong to institution anymore
                                    //find coordinator directly
                                    coordinator = User.Find(gradeObj.CoordinatorId);

                                    //check result
                                    if (coordinator != null)
                                    {
                                        //add coordinator to list of coordinators
                                        users.Add(coordinator);
                                    }
                                }

                                //check final result
                                if (coordinator != null)
                                {
                                    //set coordinator name
                                    gradeObj.CoordinatorName = coordinator.Name;
                                }
                                else
                                {
                                    //should never happen
                                    //set coordinator ID as coordinator name
                                    gradeObj.CoordinatorName = gradeObj.CoordinatorId.ToString();
                                }
                            }
                        }
                    }

                    //check if student should be loaded
                    if (loadStudent)
                    {
                        //check filter student
                        if (filterStudent > -1)
                        {
                            //get student
                            Student student = Student.Find(filterStudent);

                            //check result
                            if (student != null)
                            {
                                //check each grade
                                foreach (Grade gradeObj in grades)
                                {
                                    //set student name
                                    gradeObj.StudentName = student.Name;
                                }
                            }
                        }
                        else
                        {

                            //get list of students using filters
                            List<Student> students = Student.FindByFilter(
                                -1, filterInstitution, -1);

                            //check result
                            if (students != null)
                            {
                                //check each grade
                                foreach (Grade gradeObj in grades)
                                {
                                    //check if grade has no student
                                    if (gradeObj.StudentId == int.MinValue)
                                    {
                                        //go to next grade
                                        continue;
                                    }

                                    //find student
                                    Student student = students.Find(
                                        i => i.StudentId == gradeObj.StudentId);

                                    //check result
                                    if (student == null)
                                    {
                                        //student was not found
                                        //student might not belong to pole anymore
                                        //find student directly
                                        student = Student.Find(gradeObj.StudentId);

                                        //check result
                                        if (student != null)
                                        {
                                            //add student to list of students
                                            students.Add(student);
                                        }
                                    }

                                    //check final result
                                    if (student != null)
                                    {
                                        //set student name
                                        gradeObj.StudentName = student.Name;
                                    }
                                    else
                                    {
                                        //should never happen
                                        //set student id as student name
                                        gradeObj.StudentName = gradeObj.StudentId.ToString();
                                    }
                                }
                            }
                        }
                    }

                    //check if class should be loaded
                    if (loadClass)
                    {
                        //check filter class
                        if (filterClass > -1)
                        {
                            //get class
                            Class classObj = Class.Find(filterClass);

                            //check result
                            if (classObj != null)
                            {
                                //check each grade
                                foreach (Grade gradeObj in grades)
                                {
                                    //set class
                                    gradeObj.Class = classObj;
                                }
                            }
                        }
                        else
                        {
                            //get list of classes using filters
                            List<Class> classes = Class.FindByFilter(
                                -1, -1, -1, -1, filterSemester, filterInstitution, -1, filterTeacher);

                            //check result
                            if (classes != null)
                            {
                                //check each grade
                                foreach (Grade gradeObj in grades)
                                {
                                    //check if grade has no class
                                    if (gradeObj.ClassId == int.MinValue)
                                    {
                                        //go to next grade
                                        continue;
                                    }

                                    //find class
                                    Class classObj = classes.Find(
                                        i => i.ClassId == gradeObj.ClassId);

                                    //check result
                                    if (classObj == null)
                                    {
                                        //class was not found
                                        //class might not belong to pole anymore
                                        //find class directly
                                        classObj = Class.Find(gradeObj.ClassId);

                                        //check result
                                        if (classObj != null)
                                        {
                                            //add class to list of classes
                                            classes.Add(classObj);
                                        }
                                    }

                                    //check final result
                                    if (classObj != null)
                                    {
                                        //set class
                                        gradeObj.Class = classObj;

                                        //check teacher name
                                        if (gradeObj.TeacherName != null &&
                                            gradeObj.TeacherName.Length > 0)
                                        {
                                            //set teacher name to class
                                            gradeObj.Class.TeacherName = gradeObj.TeacherName;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //check if grade should be loaded
                    if (loadSemester)
                    {
                        //check filter semester
                        if (filterSemester > -1)
                        {
                            //get semester
                            Semester semester = Semester.Find(filterSemester);

                            //check result
                            if (semester != null)
                            {
                                //check each grade
                                foreach (Grade gradeObj in grades)
                                {
                                    //set semester description
                                    gradeObj.SemesterDescription = semester.Description;

                                    //check grade class
                                    if (gradeObj.Class != null)
                                    {
                                        //set semester to class
                                        gradeObj.Class.Semester = semester;
                                    }
                                }
                            }
                        }
                        else
                        {

                            //get list of semesters
                            List<Semester> semesters = Semester.Find();

                            //check result
                            if (semesters != null)
                            {
                                //check each grade
                                foreach (Grade gradeObj in grades)
                                {
                                    //find semester
                                    Semester semester = semesters.Find(
                                        i => i.SemesterId == gradeObj.SemesterId);

                                    //check result
                                    if (semester != null)
                                    {
                                        //set semester description
                                        gradeObj.SemesterDescription = semester.Description;

                                        //check grade class
                                        if (gradeObj.Class != null)
                                        {
                                            //set semester to class
                                            gradeObj.Class.Semester = semester;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading grades
                //create result and add it to the list
                Grade resultGrade = new Grade();
                resultGrade.Result = (int)SelectResult.FatalError;
                resultGrade.ErrorMessage = ex.Message;
                grades.Add(resultGrade);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultGrade.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return grades;
        }

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
        public CountResult CountInstitutionsByFilter(int filterInstitutionStatus)
        {
            //create result
            CountResult countResult = new CountResult();

            try
            {
                //count institutions in database and set result
                countResult.Count = Institution.CountByFilter(filterInstitutionStatus);
            }
            catch (Exception ex)
            {
                //error while loading institutions
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return countResult;
        }

        /// <summary>
        /// Find institution by ID.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <returns>
        /// The selected institution.
        /// </returns>
        public Institution FindInstitution(int institutionId)
        {
            //the target institution
            Institution resultInstitution = null;

            try
            {
                //find institution in database
                resultInstitution = Institution.Find(institutionId);

                //check result
                if (resultInstitution != null)
                {
                    //institution was found
                    resultInstitution.Result = (int)SelectResult.Success;
                }
                else
                {
                    //institution was not found
                    //create result and set it
                    resultInstitution = new Institution();
                    resultInstitution.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding institution
                //create result and set it
                resultInstitution = new Institution();
                resultInstitution.Result = (int)SelectResult.FatalError;
                resultInstitution.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultInstitution.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultInstitution;
        }

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
        public List<Institution> FindInstitutionsByFilter(bool loadCoordinator, int filterInstitutionStatus)
        {
            //create result list
            List<Institution> institutions = new List<Institution>();

            try
            {
                //get list of institutions from database using filters
                List<Institution> dbInstitutions = 
                    Institution.FindByFilter(filterInstitutionStatus);

                //check result
                if (dbInstitutions == null || dbInstitutions.Count == 0)
                {
                    //no institution was found
                    //create result and add it to the list
                    Institution resultInstitution = new Institution();
                    resultInstitution.Result = (int)SelectResult.Empty;
                    institutions.Add(resultInstitution);
                }
                else
                {
                    //check each institution
                    foreach (Institution dbInstitution in dbInstitutions)
                    {
                        //set and add institution
                        dbInstitution.Result = (int)SelectResult.Success;
                        institutions.Add(dbInstitution);
                    }

                    //check if coordinator should be loaded
                    if (loadCoordinator)
                    {
                        //get list of coordinators
                        List<User> coordinators = User.FindAssignedCoordinators();

                        //check result
                        if (coordinators != null)
                        {
                            //check each institution
                            foreach (Institution institution in institutions)
                            {
                                //find coordinator
                                User coordinator = coordinators.Find(
                                    c => c.UserId == institution.CoordinatorId);

                                //check result
                                if (coordinator != null)
                                {
                                    //set coordinator name
                                    institution.CoordinatorName = coordinator.Name;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading institutions
                //create result and add it to the list
                Institution resultInstitution = new Institution();
                resultInstitution.Result = (int)SelectResult.FatalError;
                resultInstitution.ErrorMessage = ex.Message;
                institutions.Add(resultInstitution);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultInstitution.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return institutions;
        }

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
        public DeleteResult InactivateInstitution(int institutionId, string inactivationReason)
        {
            //the inactivate result to be returned
            DeleteResult inactivateResult = new DeleteResult();

            try
            {
                //inactivate selected institution
                if (Institution.Inactivate(institutionId, inactivationReason))
                {
                    //institution was inactivated
                    //set result
                    inactivateResult.Result = (int)SelectResult.Success;
                }
                else
                {
                    //institution was not found
                    //set result
                    inactivateResult.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while inactivating institution
                //set result
                inactivateResult.Result = (int)SelectResult.FatalError;
                inactivateResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    inactivateResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return inactivateResult;
        }


        /// <summary>
        /// Get institution description.
        /// </summary>
        /// <param name="institutionId">
        /// The ID of the selected institution.
        /// </param>
        /// <returns>
        /// The institution description.
        /// </returns>
        public IdDescriptionStatus ListInstitution(int institutionId)
        {
            //the target institution
            IdDescriptionStatus resultInstitution = null;

            try
            {
                //find institution in database
                Institution dbInstitution = Institution.Find(institutionId);

                //check result
                if (dbInstitution != null)
                {
                    //institution was found
                    resultInstitution = dbInstitution.GetDescription();
                    resultInstitution.Result = (int)SelectResult.Success;
                }
                else
                {
                    //institution was not found
                    //create result and set it
                    resultInstitution = new IdDescriptionStatus();
                    resultInstitution.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding institution
                //create result and set it
                resultInstitution = new IdDescriptionStatus();
                resultInstitution.Result = (int)SelectResult.FatalError;
                resultInstitution.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultInstitution.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultInstitution;
        }

        /// <summary>
        /// Get list of institution descriptions.
        /// </summary>
        /// <returns>
        /// The list of institution descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListInstitutions()
        {
            //create result list
            List<IdDescriptionStatus> institutions = new List<IdDescriptionStatus>();

            try
            {
                //get list of all institutions from database
                List<Institution> dbInstitutions = Institution.Find();

                //check result
                if (dbInstitutions == null || dbInstitutions.Count == 0)
                {
                    //no institution was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    institutions.Add(resultItem);
                }
                else
                {
                    //check each institution
                    foreach (Institution dbInstitution in dbInstitutions)
                    {
                        //get description
                        IdDescriptionStatus item = dbInstitution.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        institutions.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading institutions
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                institutions.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return institutions;
        }

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
        public List<IdDescriptionStatus> ListInstitutionsByStatus(int status)
        {
            //create result list
            List<IdDescriptionStatus> institutions = new List<IdDescriptionStatus>();

            try
            {
                //get list of institutions from database
                List<Institution> dbInstitutions = Institution.FindByFilter(status);

                //check result
                if (dbInstitutions == null || dbInstitutions.Count == 0)
                {
                    //no institution was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    institutions.Add(resultItem);
                }
                else
                {
                    //check each institution
                    foreach (Institution dbInstitution in dbInstitutions)
                    {
                        //get description
                        IdDescriptionStatus item = dbInstitution.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        institutions.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading institutions
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                institutions.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return institutions;
        }

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
        public SaveResult SaveInstitution(Institution institution)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            try
            {
                //save institution and get row ID
                saveResult.SavedId = institution.Save();

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving institution
                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return saveResult;
        }

        #endregion Institution


        #region Instrument ************************************************************

        /// <summary>
        /// Copy instrument by ID.
        /// </summary>
        /// <param name="instrumentId">
        /// The ID of the selected instrument.
        /// </param>
        /// <returns>
        /// The instrument copy.
        /// </returns>
        public Instrument CopyInstrument(int instrumentId)
        {
            //the copy result to be returned
            //set empty instrument
            Instrument copiedInstrument = new Instrument();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //get instrument
                Instrument dbInstrument = Instrument.Find(transaction, instrumentId);

                //check result
                if (dbInstrument == null)
                {
                    //instrument was not found
                    //shold never happen
                    //set result
                    copiedInstrument.Result = (int)SelectResult.FatalError;
                    copiedInstrument.ErrorMessage = "Instrument " + instrumentId + " not found.";

                    //return result
                    return copiedInstrument;
                }

                //alter instrument before saving it as a copy
                dbInstrument.InstrumentId = -1;
                dbInstrument.CreationTime = DateTime.Now;
                dbInstrument.InactivationReason = string.Empty;
                dbInstrument.InactivationTime = DateTime.MinValue;
                dbInstrument.InstrumentStatus = (int)ItemStatus.Active;

                //reset comments
                dbInstrument.Comments = string.Empty;

                //get instrument code
                string code = dbInstrument.Code;

                //check if code ends with a number
                if (code.Length > 0 && char.IsDigit(code[code.Length - 1]))
                {
                    try
                    {
                        //find first digit
                        int firstDigitIndex = code.Length - 1;

                        //keep looking for first digit
                        while (firstDigitIndex - 1 >= 0 &&
                            char.IsDigit(code[firstDigitIndex - 1]))
                        {
                            //descrement first digit index
                            firstDigitIndex--;
                        }

                        //get code number
                        int codeNumber = int.Parse(code.Substring(firstDigitIndex));

                        //increment number
                        codeNumber++;

                        //set new code by adding new code number to prefix
                        //add zeros if necessarily
                        code = code.Substring(0, firstDigitIndex) + 
                            codeNumber.ToString().PadLeft(code.Length - firstDigitIndex, '0');

                        //check if there is another instrument already with generated code
                        if (Instrument.Find(transaction, code) != null)
                        {
                            //there is another instrument already
                            //reset generated code
                            code = dbInstrument.Code;
                        }
                    }
                    catch
                    {
                        //do nothing for now
                    }
                }

                //check final code
                if (!code.Equals(dbInstrument.Code))
                {
                    //set code to copy
                    dbInstrument.Code = code;
                }
                else
                {
                    //just set a sufix to code
                    dbInstrument.Code += " - Copy";
                }

                //save altered instrument and get row ID
                int savedId = dbInstrument.Save(transaction);

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                copiedInstrument = dbInstrument;
                copiedInstrument.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving instrument
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                copiedInstrument.Result = (int)SelectResult.FatalError;
                copiedInstrument.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    copiedInstrument.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return copiedInstrument;
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
        public CountResult CountInstrumentsByFilter(
            int filterInstrumentStatus, int filterInstrumentType,
            int filterInstitution, int filterPole)
        {
            //create result
            CountResult countResult = new CountResult();

            try
            {
                //count instruments in database and set result
                countResult.Count = Instrument.CountByFilter(
                    filterInstrumentStatus, filterInstrumentType, filterInstitution, filterPole);
            }
            catch (Exception ex)
            {
                //error while loading instruments
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return countResult;
        }

        /// <summary>
        /// Find instrument by ID.
        /// </summary>
        /// <param name="instrumentId">
        /// The ID of the selected instrument.
        /// </param>
        /// <returns>
        /// The selected instrument.
        /// </returns>
        public Instrument FindInstrument(int instrumentId)
        {
            //the target instrument
            Instrument resultInstrument = null;

            try
            {
                //find instrument in database
                resultInstrument = Instrument.Find(instrumentId);

                //check result
                if (resultInstrument != null)
                {
                    //instrument was found
                    resultInstrument.Result = (int)SelectResult.Success;
                }
                else
                {
                    //instrument was not found
                    //create result and set it
                    resultInstrument = new Instrument();
                    resultInstrument.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding instrument
                //create result and set it
                resultInstrument = new Instrument();
                resultInstrument.Result = (int)SelectResult.FatalError;
                resultInstrument.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultInstrument.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultInstrument;
        }

        /// <summary>
        /// Find next available instrument without a loan for selected pole.
        /// </summary>
        /// <param name="instrumentId">
        /// The ID of the selected pole.
        /// </param>
        /// <returns>
        /// The next available instrument.
        /// </returns>
        public Instrument FindNextInstrumentWithoutLoan(int poleId)
        {
            //the target instrument
            Instrument resultInstrument = null;

            try
            {
                //get list of active instruments from database for selected pole
                List<Instrument> dbInstruments = Instrument.FindByFilter(
                    (int)ItemStatus.Active, -1, -1, poleId);

                //get list of active loans from database for selected pole
                List<Loan> dbLoans = Loan.FindByPole(poleId, (int)ItemStatus.Active);

                //check each instrument
                foreach (Instrument instrument in dbInstruments)
                {
                    //check if instrument has no loan
                    if (dbLoans.Find(l => l.InstrumentId == instrument.InstrumentId) == null)
                    {
                        //found next available instrument
                        resultInstrument = instrument;

                        //set result
                        resultInstrument.Result = (int)SelectResult.Success;

                        //exit loop
                        break;
                    }
                }

                //check final result
                if (resultInstrument == null)
                {
                    //no instrument is available
                    //create result and set it
                    resultInstrument = new Instrument();
                    resultInstrument.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding instrument
                //create result and set it
                resultInstrument = new Instrument();
                resultInstrument.Result = (int)SelectResult.FatalError;
                resultInstrument.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultInstrument.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultInstrument;
        }

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
        public List<Instrument> FindInstrumentsByFilter(
            bool loadPole, bool loadLoan, int filterInstrumentStatus,
            int filterInstrumentType, int filterInstitution, int filterPole)
        {
            //create result list
            List<Instrument> instruments = new List<Instrument>();

            try
            {
                //get list of instruments from database using filters
                List<Instrument> dbInstruments = Instrument.FindByFilter(
                    filterInstrumentStatus, filterInstrumentType, filterInstitution, filterPole);

                //check result
                if (dbInstruments == null || dbInstruments.Count == 0)
                {
                    //no instrument was found
                    //create result and add it to the list
                    Instrument resultInstrument = new Instrument();
                    resultInstrument.Result = (int)SelectResult.Empty;
                    instruments.Add(resultInstrument);
                }
                else
                {
                    //check each instrument
                    foreach (Instrument dbInstrument in dbInstruments)
                    {
                        //set and add instrument
                        dbInstrument.Result = (int)SelectResult.Success;
                        instruments.Add(dbInstrument);
                    }

                    //check if instrument should be loaded
                    if (loadPole)
                    {
                        //get list of all poles
                        List<Pole> poles = Pole.Find();

                        //check result
                        if (poles != null)
                        {
                            //check each instrument
                            foreach (Instrument instrument in instruments)
                            {
                                //find pole
                                Pole pole = poles.Find(
                                    p => p.PoleId == instrument.PoleId);

                                //check result
                                if (pole != null)
                                {
                                    //set pole name
                                    instrument.PoleName = pole.Name;
                                }
                            }
                        }
                    }

                    //check if loan data should be loaded
                    if (loadLoan)
                    {
                        //get list of loans by filter
                        List<Loan> loans = null;

                        //check pole filter
                        if (filterPole > -1)
                        {
                            //get list of loans by pole
                            loans = Loan.FindByPole(filterPole, (int)ItemStatus.Active);
                        }
                        else
                        {
                            //get list of loans
                            loans = Loan.FindByStatus((int)ItemStatus.Active);
                        }

                        //check result
                        if (loans != null)
                        {
                            //get students with loan
                            List<Student> students = Student.FindWithLoan();

                            //check result
                            if (students != null)
                            {
                                //check each instrument
                                foreach (Instrument instrument in instruments)
                                {
                                    //find loan for current instrument
                                    Loan loan = loans.Find(
                                        l => l.InstrumentId == instrument.InstrumentId);

                                    //check result
                                    if (loan == null)
                                    {
                                        //go to next instrument
                                        continue;
                                    }

                                    //find student
                                    Student student = students.Find(
                                        s => s.StudentId == loan.StudentId);

                                    //check result
                                    if (student == null)
                                    {
                                        //go to next instrument
                                        continue;
                                    }

                                    //set tudent name
                                    instrument.StudentName = student.Name;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading instruments
                //create result and add it to the list
                Instrument resultInstrument = new Instrument();
                resultInstrument.Result = (int)SelectResult.FatalError;
                resultInstrument.ErrorMessage = ex.Message;
                instruments.Clear();
                instruments.Add(resultInstrument);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultInstrument.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return instruments;
        }

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
        public DeleteResult InactivateInstrument(int instrumentId, string inactivationReason)
        {
            //the inactivate result to be returned
            DeleteResult inactivateResult = new DeleteResult();

            try
            {
                //inactivate selected instrument
                if (Instrument.Inactivate(instrumentId, inactivationReason))
                {
                    //instrument was inactivated
                    //set result
                    inactivateResult.Result = (int)SelectResult.Success;
                }
                else
                {
                    //instrument was not found
                    //set result
                    inactivateResult.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while inactivating instrument
                //set result
                inactivateResult.Result = (int)SelectResult.FatalError;
                inactivateResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    inactivateResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return inactivateResult;
        }

        /// <summary>
        /// Get list of instrument descriptions.
        /// </summary>
        /// <returns>
        /// The list of instrument descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListInstruments()
        {
            //create result list
            List<IdDescriptionStatus> instruments = new List<IdDescriptionStatus>();

            try
            {
                //get list of all instruments from database
                List<Instrument> dbInstruments = Instrument.Find();

                //check result
                if (dbInstruments == null || dbInstruments.Count == 0)
                {
                    //no instrument was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    instruments.Add(resultItem);
                }
                else
                {
                    //check each instrument
                    foreach (Instrument dbInstrument in dbInstruments)
                    {
                        //get description
                        IdDescriptionStatus item = dbInstrument.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        instruments.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading instruments
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                instruments.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return instruments;
        }

        /// <summary>
        /// Get list of instrument descriptions.
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
        public List<IdDescriptionStatus> ListInstrumentsByInstitution(int institutionId, int status)
        {
            //create result list
            List<IdDescriptionStatus> instruments = new List<IdDescriptionStatus>();

            try
            {
                //get list of instruments from database
                List<Instrument> dbInstruments = Instrument.FindByFilter(
                    status, -1, institutionId, -1);

                //check result
                if (dbInstruments == null || dbInstruments.Count == 0)
                {
                    //no instrument was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    instruments.Add(resultItem);
                }
                else
                {
                    //check each instrument
                    foreach (Instrument dbInstrument in dbInstruments)
                    {
                        //get description
                        IdDescriptionStatus item = dbInstrument.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        instruments.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading instruments
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                instruments.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return instruments;
        }

        /// <summary>
        /// Get list of instrument descriptions.
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
        public List<IdDescriptionStatus> ListInstrumentsByPole(int poleId, int status)
        {
            //create result list
            List<IdDescriptionStatus> instruments = new List<IdDescriptionStatus>();

            try
            {
                //get list of instruments from database
                List<Instrument> dbInstruments = Instrument.FindByFilter(
                    status, -1, -1, poleId);

                //check result
                if (dbInstruments == null || dbInstruments.Count == 0)
                {
                    //no instrument was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    instruments.Add(resultItem);
                }
                else
                {
                    //check each instrument
                    foreach (Instrument dbInstrument in dbInstruments)
                    {
                        //get description
                        IdDescriptionStatus item = dbInstrument.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        instruments.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading instruments
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                instruments.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return instruments;
        }

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
        public List<IdDescriptionStatus> ListInstrumentsByStatus(int status)
        {
            //create result list
            List<IdDescriptionStatus> instruments = new List<IdDescriptionStatus>();

            try
            {
                //get list of instruments from database
                List<Instrument> dbInstruments = Instrument.FindByFilter(
                    status, -1, -1, -1);

                //check result
                if (dbInstruments == null || dbInstruments.Count == 0)
                {
                    //no instrument was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    instruments.Add(resultItem);
                }
                else
                {
                    //check each instrument
                    foreach (Instrument dbInstrument in dbInstruments)
                    {
                        //get description
                        IdDescriptionStatus item = dbInstrument.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        instruments.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading instruments
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                instruments.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return instruments;
        }

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
        public SaveResult SaveInstrument(Instrument instrument, List<Loan> loans)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //save instrument and get row ID
                saveResult.SavedId = instrument.Save(transaction);
                

                //check loans
                if (loans == null)
                {
                    //create empty list
                    loans = new List<Loan>();
                }

                //check each loan
                foreach (Loan loan in loans)
                {
                    //set instrument ID to loan
                    loan.InstrumentId = saveResult.SavedId;
                }

                //get list of loans for saved instrument
                List<Loan> dbLoans = instrument.InstrumentId > 0 ?
                    FindLoansByInstrument(instrument.InstrumentId, -1) :
                    new List<Loan>();

                //check result
                if (dbLoans[0].Result == (int)SelectResult.Empty)
                {
                    //no loan
                    //clear list
                    dbLoans.Clear();
                }
                else if (dbLoans[0].Result == (int)SelectResult.FatalError)
                {
                    //error while getting loans
                    //throw an exception
                    throw new ApplicationException(
                        "Unexpected error while getting instrument loans. " +
                        dbLoans[0].ErrorMessage);
                }

                //check each assigned loan
                foreach (Loan loan in loans)
                {
                    //check if loan has id
                    if (loan.LoanId > 0)
                    {
                        //find database loan
                        Loan dbLoan = dbLoans.Find(r => r.LoanId == loan.LoanId);

                        //check result
                        if (dbLoan != null &&
                            dbLoan.Comments == loan.Comments &&
                            dbLoan.EndDate == loan.EndDate &&
                            dbLoan.InstrumentId == loan.InstrumentId &&
                            dbLoan.LoanStatus == loan.LoanStatus &&
                            dbLoan.StartDate == loan.StartDate &&
                            dbLoan.InstrumentId == loan.InstrumentId)
                        {
                            //loan data has not changed
                            //go to next loan
                            continue;
                        }
                    }

                    //save loan in database
                    loan.Save(transaction);
                }

                //must remove old loans
                //check each previous loans
                foreach (Loan dbLoan in dbLoans)
                {
                    //check if loan is not in the new list
                    if (loans.Find(r => r.LoanId == dbLoan.LoanId) == null)
                    {
                        //safety procedure
                        //check if loan was created more than edition threshold
                        if (dbLoan.CreationTime.Date <
                            DateTime.Today.AddDays(-Loan.EDITION_THRESHOLD))
                        {
                            //cannot delete loan
                            continue;
                        }

                        //delete loan from database
                        Loan.Delete(transaction, dbLoan.LoanId);
                    }
                }
                

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving instrument
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return saveResult;
        }

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
        public CountResult CountLoansByFilter(
            int filterLoanStatus, int filterInstrument, int filterStudent,
            DateTime filterStartDate, DateTime filterEndDate)
        {
            //create result
            CountResult countResult = new CountResult();

            try
            {
                //count loans in database and set result
                countResult.Count = Loan.CountByFilter(
                    filterLoanStatus, filterInstrument, filterStudent,
                    filterStartDate, filterEndDate);
            }
            catch (Exception ex)
            {
                //error while loading loans
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return countResult;
        }

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
        public List<Loan> FindLoansByInstrument(int instrumentId, int status)
        {
            //create result list
            List<Loan> loans = new List<Loan>();

            try
            {
                //get list of all instrument loans from database
                List<Loan> dbLoans = Loan.FindByInstrument(instrumentId, status);

                //check result
                if (dbLoans == null || dbLoans.Count == 0)
                {
                    //no loan was found
                    //create result and add it to the list
                    Loan resultLoan = new Loan();
                    resultLoan.Result = (int)SelectResult.Empty;
                    loans.Add(resultLoan);
                }
                else
                {
                    //find students for selected loans
                    List<Student> dbStudents = Student.FindByInstrument(instrumentId);

                    //check each loan
                    foreach (Loan dbLoan in dbLoans)
                    {
                        //set loan result
                        dbLoan.Result = (int)SelectResult.Success;

                        //check students
                        if (dbStudents != null)
                        {
                            //find loan student
                            Student student = dbStudents.Find(s => s.StudentId == dbLoan.StudentId);

                            //check result
                            if (student != null)
                            {
                                //set student name
                                dbLoan.StudentName = student.Name;
                            }
                        }

                        //add loan
                        loans.Add(dbLoan);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading loans
                //create result and add it to the list
                Loan resultLoan = new Loan();
                resultLoan.Result = (int)SelectResult.FatalError;
                resultLoan.ErrorMessage = ex.Message;
                loans.Add(resultLoan);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultLoan.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return loans;
        }

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
        public List<Loan> FindLoansByPole(int poleId, int status)
        {
            //create result list
            List<Loan> loans = new List<Loan>();

            try
            {
                //get list of all pole loans from database
                List<Loan> dbLoans = Loan.FindByPole(poleId, status);

                //check result
                if (dbLoans == null || dbLoans.Count == 0)
                {
                    //no loan was found
                    //create result and add it to the list
                    Loan resultLoan = new Loan();
                    resultLoan.Result = (int)SelectResult.Empty;
                    loans.Add(resultLoan);
                }
                else
                {
                    //check each loan
                    foreach (Loan dbLoan in dbLoans)
                    {
                        //set loan result
                        dbLoan.Result = (int)SelectResult.Success;

                        //add loan
                        loans.Add(dbLoan);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading loans
                //create result and add it to the list
                Loan resultLoan = new Loan();
                resultLoan.Result = (int)SelectResult.FatalError;
                resultLoan.ErrorMessage = ex.Message;
                loans.Add(resultLoan);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultLoan.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return loans;
        }

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
        public List<Loan> FindLoansByStudent(int studentId, int status)
        {
            //create result list
            List<Loan> loans = new List<Loan>();

            try
            {
                //get list of all student loans from database
                List<Loan> dbLoans = Loan.FindByStudent(studentId, status);

                //check result
                if (dbLoans == null || dbLoans.Count == 0)
                {
                    //no loan was found
                    //create result and add it to the list
                    Loan resultLoan = new Loan();
                    resultLoan.Result = (int)SelectResult.Empty;
                    loans.Add(resultLoan);
                }
                else
                {
                    //find instruments for selected loans
                    List<Instrument> dbInstruments = Instrument.FindByStudent(studentId);

                    //check each loan
                    foreach (Loan dbLoan in dbLoans)
                    {
                        //set loan result
                        dbLoan.Result = (int)SelectResult.Success;

                        //check instruments
                        if (dbInstruments != null)
                        {
                            //find loan instrument
                            Instrument instrument = dbInstruments.Find(s => s.InstrumentId == dbLoan.InstrumentId);

                            //check result
                            if (instrument != null)
                            {
                                //set instrument name
                                dbLoan.InstrumentCode = instrument.Code;
                            }
                        }

                        //add loan
                        loans.Add(dbLoan);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading loans
                //create result and add it to the list
                Loan resultLoan = new Loan();
                resultLoan.Result = (int)SelectResult.FatalError;
                resultLoan.ErrorMessage = ex.Message;
                loans.Add(resultLoan);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultLoan.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return loans;
        }

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
        public Permission FindPermission(int permissionId)
        {
            //the target permission
            Permission resultPermission = null;

            try
            {
                //find permission in database
                resultPermission = Permission.Find(permissionId);

                //check result
                if (resultPermission != null)
                {
                    //permission was found
                    resultPermission.Result = (int)SelectResult.Success;
                }
                else
                {
                    //permission was not found
                    //create result and set it
                    resultPermission = new Permission();
                    resultPermission.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding permission
                //create result and set it
                resultPermission = new Permission();
                resultPermission.Result = (int)SelectResult.FatalError;
                resultPermission.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultPermission.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultPermission;
        }

        /// <summary>
        /// Find all permissions.
        /// </summary>
        /// <returns>
        /// The list of permissions.
        /// </returns>
        public List<Permission> FindPermissions()
        {
            //create result list
            List<Permission> permissions = new List<Permission>();

            try
            {
                //get list of all permission from database
                List<Permission> dbPermissions = Permission.Find();

                //check result
                if (dbPermissions == null || dbPermissions.Count == 0)
                {
                    //no permission was found
                    //create result and add it to the list
                    Permission resultPermission = new Permission();
                    resultPermission.Result = (int)SelectResult.Empty;
                    permissions.Add(resultPermission);
                }
                else
                {
                    //check each permission
                    foreach (Permission dbPermission in dbPermissions)
                    {
                        //set and add permission
                        dbPermission.Result = (int)SelectResult.Success;
                        permissions.Add(dbPermission);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading permissions
                //create result and add it to the list
                Permission resultPermission = new Permission();
                resultPermission.Result = (int)SelectResult.FatalError;
                resultPermission.ErrorMessage = ex.Message;
                permissions.Add(resultPermission);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultPermission.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return permissions;
        }

        /// <summary>
        /// Find all assigned permissions for selected role.
        /// </summary>
        /// <param name="roleId">
        /// The ID of the selected role.
        /// </param>
        /// <returns>
        /// The list of permissions.
        /// </returns>
        public List<Permission> FindPermissionsByRole(int roleId)
        {
            //create result list
            List<Permission> permissions = new List<Permission>();

            try
            {
                //get list of all role permission from database
                List<Permission> dbPermissions = Permission.FindByRole(roleId);

                //check result
                if (dbPermissions == null || dbPermissions.Count == 0)
                {
                    //no permission was found
                    //create result and add it to the list
                    Permission resultPermission = new Permission();
                    resultPermission.Result = (int)SelectResult.Empty;
                    permissions.Add(resultPermission);
                }
                else
                {
                    //check each permission
                    foreach (Permission dbPermission in dbPermissions)
                    {
                        //set and add permission
                        dbPermission.Result = (int)SelectResult.Success;
                        permissions.Add(dbPermission);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading permissions
                //create result and add it to the list
                Permission resultPermission = new Permission();
                resultPermission.Result = (int)SelectResult.FatalError;
                resultPermission.ErrorMessage = ex.Message;
                permissions.Add(resultPermission);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultPermission.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return permissions;
        }

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
        public CountResult CountPolesByFilter(int filterPoleStatus, int filterInstitution)
        {
            //create result
            CountResult countResult = new CountResult();

            try
            {
                //count poles in database and set result
                countResult.Count = Pole.CountByFilter(filterPoleStatus, filterInstitution);
            }
            catch (Exception ex)
            {
                //error while loading poles
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return countResult;
        }

        /// <summary>
        /// Find pole by ID.
        /// </summary>
        /// <param name="poleId">
        /// The ID of the selected pole.
        /// </param>
        /// <returns>
        /// The selected pole.
        /// </returns>
        public Pole FindPole(int poleId)
        {
            //the target pole
            Pole resultPole = null;

            try
            {
                //find pole in database
                resultPole = Pole.Find(poleId);

                //check result
                if (resultPole != null)
                {
                    //pole was found
                    resultPole.Result = (int)SelectResult.Success;
                }
                else
                {
                    //pole was not found
                    //create result and set it
                    resultPole = new Pole();
                    resultPole.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding pole
                //create result and set it
                resultPole = new Pole();
                resultPole.Result = (int)SelectResult.FatalError;
                resultPole.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultPole.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultPole;
        }

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
        public List<Pole> FindPolesByFilter(bool loadInstitution, int filterPoleStatus, int filterInstitution)
        {
            //create result list
            List<Pole> poles = new List<Pole>();

            try
            {
                //get list of poles from database using filters
                List<Pole> dbPoles = Pole.FindByFilter(filterPoleStatus, filterInstitution);

                //check result
                if (dbPoles == null || dbPoles.Count == 0)
                {
                    //no pole was found
                    //create result and add it to the list
                    Pole resultPole = new Pole();
                    resultPole.Result = (int)SelectResult.Empty;
                    poles.Add(resultPole);
                }
                else
                {
                    //check each pole
                    foreach (Pole dbPole in dbPoles)
                    {
                        //set and add pole
                        dbPole.Result = (int)SelectResult.Success;
                        poles.Add(dbPole);
                    }

                    //check if institution should be loaded
                    if (loadInstitution)
                    {
                        //get list of institutions
                        List<Institution> institutions = Institution.Find();

                        //check result
                        if (institutions != null)
                        {
                            //check each pole
                            foreach (Pole pole in poles)
                            {
                                //find institution
                                Institution institution = institutions.Find(
                                    i => i.InstitutionId == pole.InstitutionId);

                                //check result
                                if (institution != null)
                                {
                                    //set institution name
                                    pole.InstitutionName = institution.ProjectName;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading poles
                //create result and add it to the list
                Pole resultPole = new Pole();
                resultPole.Result = (int)SelectResult.FatalError;
                resultPole.ErrorMessage = ex.Message;
                poles.Add(resultPole);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultPole.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return poles;
        }

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
        public DeleteResult InactivatePole(int poleId, string inactivationReason)
        {
            //the inactivate result to be returned
            DeleteResult inactivateResult = new DeleteResult();

            try
            {
                //inactivate selected pole
                if (Pole.Inactivate(poleId, inactivationReason))
                {
                    //pole was inactivated
                    //set result
                    inactivateResult.Result = (int)SelectResult.Success;
                }
                else
                {
                    //pole was not found
                    //set result
                    inactivateResult.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while inactivating pole
                //set result
                inactivateResult.Result = (int)SelectResult.FatalError;
                inactivateResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    inactivateResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return inactivateResult;
        }

        /// <summary>
        /// Get list of pole descriptions.
        /// </summary>
        /// <returns>
        /// The list of pole descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListPoles()
        {
            //create result list
            List<IdDescriptionStatus> poles = new List<IdDescriptionStatus>();

            try
            {
                //get list of all poles from database
                List<Pole> dbPoles = Pole.Find();

                //check result
                if (dbPoles == null || dbPoles.Count == 0)
                {
                    //no pole was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    poles.Add(resultItem);
                }
                else
                {
                    //check each pole
                    foreach (Pole dbPole in dbPoles)
                    {
                        //get description
                        IdDescriptionStatus item = dbPole.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        poles.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading poles
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                poles.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return poles;
        }

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
        public List<IdDescriptionStatus> ListPolesByInstitution(int institutionId, int status)
        {
            //create result list
            List<IdDescriptionStatus> poles = new List<IdDescriptionStatus>();

            try
            {
                //get list of poles from database
                List<Pole> dbPoles = Pole.FindByFilter(status, institutionId);

                //check result
                if (dbPoles == null || dbPoles.Count == 0)
                {
                    //no pole was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    poles.Add(resultItem);
                }
                else
                {
                    //check each pole
                    foreach (Pole dbPole in dbPoles)
                    {
                        //get description
                        IdDescriptionStatus item = dbPole.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        poles.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading poles
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                poles.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return poles;
        }

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
        public List<IdDescriptionStatus> ListPolesByStatus(int status)
        {
            //create result list
            List<IdDescriptionStatus> poles = new List<IdDescriptionStatus>();

            try
            {
                //get list of poles from database
                List<Pole> dbPoles = Pole.FindByFilter(status, -1);

                //check result
                if (dbPoles == null || dbPoles.Count == 0)
                {
                    //no pole was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    poles.Add(resultItem);
                }
                else
                {
                    //check each pole
                    foreach (Pole dbPole in dbPoles)
                    {
                        //get description
                        IdDescriptionStatus item = dbPole.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        poles.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading poles
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                poles.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return poles;
        }

        /// <summary>
        /// Find all assigned poles for selected teacher.
        /// </summary>
        /// <param name="teacherId">
        /// The ID of the selected teacher.
        /// </param>
        /// <returns>
        /// The list of pole descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListPolesByTeacher(int teacherId)
        {
            //create result list
            List<IdDescriptionStatus> poles = new List<IdDescriptionStatus>();

            try
            {
                //get list of poles from database
                List<Pole> dbPoles = Pole.FindByTeacher(teacherId);

                //check result
                if (dbPoles == null || dbPoles.Count == 0)
                {
                    //no pole was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    poles.Add(resultItem);
                }
                else
                {
                    //check each pole
                    foreach (Pole dbPole in dbPoles)
                    {
                        //get description
                        IdDescriptionStatus item = dbPole.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        poles.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading poles
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                poles.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return poles;
        }

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
        public SaveResult SavePole(Pole pole, List<int> teacherIds)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            //check teacher ids
            if (teacherIds == null)
            {
                //no permission set to role
                //set empty list
                teacherIds = new List<int>();
            }

            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //save pole and get row ID
                saveResult.SavedId = pole.Save(transaction);

                //get list of teacherpoles for saved pole
                List<Teacherpole> dbTeacherPoles =
                    Teacherpole.FindByPole(transaction, saveResult.SavedId);

                //check result
                if (dbTeacherPoles == null)
                {
                    //no teacherpole in database
                    //set empty list
                    dbTeacherPoles = new List<Teacherpole>();
                }

                //check each assigned teacher
                foreach (int teacherId in teacherIds)
                {
                    //check if assigned teacher is not in database yet
                    if (dbTeacherPoles.Find(rp => rp.TeacherId == teacherId) == null)
                    {
                        //must add new pole teacher relation
                        //create and save new pole teacher
                        Teacherpole poleTeacher = new Teacherpole();
                        poleTeacher.TeacherPoleId = -1;
                        poleTeacher.PoleId = saveResult.SavedId;
                        poleTeacher.TeacherId = teacherId;
                        poleTeacher.Save(transaction);
                    }
                }

                //must removed old pole teacher relations
                //check each previous relations
                foreach (Teacherpole poleTeacher in dbTeacherPoles)
                {
                    //check if relation is not in the new list
                    if (!teacherIds.Contains(poleTeacher.TeacherId))
                    {
                        //delete relation from database
                        Teacherpole.Delete(transaction, poleTeacher.TeacherPoleId);
                    }
                }

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving pole
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return saveResult;
        }

        #endregion Pole


        #region Questions *************************************************************
        
        /// <summary>
        /// Find all questions.
        /// </summary>
        /// <returns>
        /// The list of questions.
        /// </returns>
        public List<Question> FindQuestions()
        {
            //create result list
            List<Question> questions = new List<Question>();

            try
            {
                //get list of all question from database
                List<Question> dbQuestions = Question.Find();

                //check result
                if (dbQuestions == null || dbQuestions.Count == 0)
                {
                    //no question was found
                    //create result and add it to the list
                    Question resultQuestion = new Question();
                    resultQuestion.Result = (int)SelectResult.Empty;
                    questions.Add(resultQuestion);
                }
                else
                {
                    //check each question
                    foreach (Question dbQuestion in dbQuestions)
                    {
                        //set and add question
                        dbQuestion.Result = (int)SelectResult.Success;
                        questions.Add(dbQuestion);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading questions
                //create result and add it to the list
                Question resultQuestion = new Question();
                resultQuestion.Result = (int)SelectResult.FatalError;
                resultQuestion.ErrorMessage = ex.Message;
                questions.Add(resultQuestion);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultQuestion.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return questions;
        }

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
        public CountResult CountEvationsByFilter(
            int filterSemester, DateTime filterReferenceDate, int filterInstitution, int filterTeacher)
        {
            //create result
            CountResult countResult = new CountResult();

            try
            {
                //count registration evations in database and set result
                countResult.Count = Registration.CountEvationsByFilter(
                    filterSemester, filterReferenceDate, filterInstitution, filterTeacher);
            }
            catch (Exception ex)
            {
                //error while couting registrations
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return countResult;
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
        /// <returns>
        /// The number of registrations.
        /// </returns>
        public CountResult CountRegistrationsByClass(int classId, int status)
        {
            //create result
            CountResult countResult = new CountResult();

            try
            {
                //count class registrations in database and set result
                countResult.Count = Registration.CountByClass(classId, status, -1);                
            }
            catch (Exception ex)
            {
                //error while couting registrations
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return countResult;
        }

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
        public CountResult CountRegistrationsByFilter(
            int filterRegistrationStatus, int filterSemester, int filterInstitution, 
            int filterPole, int filterTeacher, int filterClass)
        {
            //create result
            CountResult countResult = new CountResult();

            try
            {
                //count registrations in database and set result
                countResult.Count = Registration.CountByFilter(
                    filterRegistrationStatus, filterSemester, 
                    filterInstitution, filterPole, filterTeacher, filterClass);
            }
            catch (Exception ex)
            {
                //error while loading registrations
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return countResult;
        }

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
        public List<Registration> FindRegistrationsByClass(int classId, int status)
        {
            //create result list
            List<Registration> registrations = new List<Registration>();

            try
            {
                //get list of all class registrations from database
                List<Registration> dbRegistrations = Registration.FindByClass(classId, status);

                //check result
                if (dbRegistrations == null || dbRegistrations.Count == 0)
                {
                    //no registration was found
                    //create result and add it to the list
                    Registration resultRegistration = new Registration();
                    resultRegistration.Result = (int)SelectResult.Empty;
                    registrations.Add(resultRegistration);
                }
                else
                {
                    //find students for selected registrations
                    List<Student> dbStudents = Student.FindByClass(classId);

                    //check each registration
                    foreach (Registration dbRegistration in dbRegistrations)
                    {
                        //set registration result
                        dbRegistration.Result = (int)SelectResult.Success;

                        //check students
                        if (dbStudents != null)
                        {
                            //find registration student
                            Student student = dbStudents.Find(s => s.StudentId == dbRegistration.StudentId);

                            //check result
                            if (student != null)
                            {
                                //set student name
                                dbRegistration.StudentName = student.Name;
                            }
                        }

                        //add registration
                        registrations.Add(dbRegistration);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading registrations
                //create result and add it to the list
                Registration resultRegistration = new Registration();
                resultRegistration.Result = (int)SelectResult.FatalError;
                resultRegistration.ErrorMessage = ex.Message;
                registrations.Add(resultRegistration);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultRegistration.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return registrations;
        }

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
        public List<Registration> FindRegistrationsByFilter(
            bool loadClass, bool loadStudent, bool loadSemester, bool loadPole,
            int filterRegistrationStatus, int filterSemester, int filterInstitution,
            int filterPole, int filterTeacher, int filterClass)
        {
            //create result list
            List<Registration> registrations = new List<Registration>();

            try
            {
                //get list of registrations from database using filters
                List<Registration> dbRegistrations = Registration.FindByFilter(
                    filterRegistrationStatus, filterSemester, 
                    filterInstitution, filterPole, filterTeacher, filterClass);

                //check result
                if (dbRegistrations == null || dbRegistrations.Count == 0)
                {
                    //no registration was found
                    //create result and add it to the list
                    Registration resultRegistration = new Registration();
                    resultRegistration.Result = (int)SelectResult.Empty;
                    registrations.Add(resultRegistration);
                }
                else
                {
                    //check each registration
                    foreach (Registration dbRegistration in dbRegistrations)
                    {
                        //set and add registration
                        dbRegistration.Result = (int)SelectResult.Success;
                        registrations.Add(dbRegistration);
                    }

                    //list of classes
                    List<Class> classes = null;

                    //list of semesters
                    List<Semester> semesters = null;

                    //list of poles
                    List<Pole> poles = null;

                    //check if class, semester or pole should be loaded
                    //semester and pole are class dependent
                    if (loadClass || loadSemester || loadPole)
                    {
                        //check filter class
                        if (filterClass > -1)
                        {
                            //create list of classes
                            classes = new List<Class>();

                            //get class
                            Class classObj = Class.Find(filterClass);

                            //check result
                            if (classObj != null)
                            {
                                //add class to list
                                classes.Add(classObj);
                            }
                        }
                        else
                        {

                            //get list of classes using filters
                            classes = Class.FindByFilter(
                                -1, -1, -1, -1, filterSemester, filterInstitution, filterPole, -1);

                            //check result
                            if (classes == null)
                            {
                                //no class is available
                                //create empty list
                                classes = new List<Class>();
                            }
                        }
                        
                        //check each registration
                        foreach (Registration registration in registrations)
                        {
                            //find class for current registration
                            Class classObj = null;

                            //find class
                            classObj = classes.Find(
                                i => i.ClassId == registration.ClassId);

                            //check result
                            if (classObj == null)
                            {
                                //class was not found
                                //class might not belong to pole anymore
                                //get class directly
                                classObj = Class.Find(registration.ClassId);

                                //check result
                                if (classObj != null)
                                {
                                    //add class to list of classes
                                    classes.Add(classObj);
                                }
                            }

                            //check if class should be loaded
                            if (loadClass)
                            {
                                //check class
                                if (classObj != null)
                                {
                                    //set class
                                    registration.Class = classObj;
                                }
                            }

                            //check if registration should be loaded
                            if (loadSemester)
                            {
                                //check filter semester
                                if (filterSemester > -1)
                                {
                                    //get semester
                                    Semester semester = Semester.Find(filterSemester);

                                    //check result
                                    if (semester != null)
                                    {
                                        //set semester
                                        registration.Semester = semester;
                                    }
                                }
                                //check class
                                else if(classObj != null)
                                {
                                    //check semesters
                                    if (semesters == null)
                                    {
                                        //get list of semesters
                                        semesters = Semester.Find();

                                        //check result
                                        if (semesters == null)
                                        {
                                            //no semester was found
                                            //create empty list
                                            semesters = new List<Semester>();
                                        }
                                    }

                                    //find semester
                                    Semester semester = semesters.Find(
                                        i => i.SemesterId == classObj.SemesterId);

                                    //check result
                                    if (semester != null)
                                    {
                                        //set semester
                                        registration.Semester = semester;
                                    }
                                }
                            }

                            //check if registration should be loaded
                            if (loadPole)
                            {
                                //check filter pole
                                if (filterPole > -1)
                                {
                                    //get pole
                                    Pole pole = Pole.Find(filterPole);

                                    //check result
                                    if (pole != null)
                                    {
                                        //set pole
                                        registration.PoleId = pole.PoleId;
                                        registration.PoleName = pole.Name;
                                    }
                                }
                                else
                                {
                                    //check list of poles
                                    if (poles == null)
                                    {
                                        //get list of poles
                                        poles = Pole.FindByFilter(-1, filterInstitution);

                                        //check result
                                        if (poles == null)
                                        {
                                            //create empty list
                                            poles = new List<Pole>();
                                        }
                                    }

                                    //find pole
                                    Pole pole = poles.Find(
                                        i => i.PoleId == classObj.PoleId);

                                    //check result
                                    if (pole != null)
                                    {
                                        //set pole
                                        registration.PoleId = pole.PoleId;
                                        registration.PoleName = pole.Name;
                                    }
                                }
                            }
                        }
                    }
                    
                    //check if student should be loaded
                    if (loadStudent)
                    {
                        //get list of students
                        List<Student> students = filterClass > -1 ?
                            Student.FindByClass(filterClass) : 
                            Student.FindByFilter(-1, filterInstitution, filterPole);

                        //check result
                        if (students != null)
                        {
                            //check each registration
                            foreach (Registration registration in registrations)
                            {
                                //find student
                                Student student = students.Find(
                                    i => i.StudentId == registration.StudentId);

                                //check result
                                if (student != null)
                                {
                                    //set student name
                                    registration.StudentName = student.Name;
                                }
                                else
                                {
                                    //student from a different pole or institution 
                                    //might have been added to class
                                    //find student directly
                                    student = Student.Find(registration.StudentId);

                                    //check result
                                    if (student != null)
                                    {
                                        //set student name
                                        registration.StudentName = student.Name;

                                        //add student to list
                                        students.Add(student);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading registrations
                //create result and add it to the list
                Registration resultRegistration = new Registration();
                resultRegistration.Result = (int)SelectResult.FatalError;
                resultRegistration.ErrorMessage = ex.Message;
                registrations.Add(resultRegistration);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultRegistration.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return registrations;
        }

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
        public List<Registration> FindRegistrationsByStudent(
            bool loadTeacher, bool loadSemester, int studentId, int status)
        {
            //create result list
            List<Registration> registrations = new List<Registration>();

            try
            {
                //get list of all student registrations from database
                List<Registration> dbRegistrations = Registration.FindByStudent(
                    studentId, status);

                //check result
                if (dbRegistrations == null || dbRegistrations.Count == 0)
                {
                    //no registration was found
                    //create result and add it to the list
                    Registration resultRegistration = new Registration();
                    resultRegistration.Result = (int)SelectResult.Empty;
                    registrations.Add(resultRegistration);
                }
                else
                {
                    //find classes for selected registrations
                    List<Class> dbClasses = Class.FindByStudent(studentId);

                    //list of teachers
                    List<Teacher> teachers = null;

                    //list of semesters
                    List<Semester> semesters = null;

                    //check each registration
                    foreach (Registration dbRegistration in dbRegistrations)
                    {
                        //set registration result
                        dbRegistration.Result = (int)SelectResult.Success;
                        
                        //add registration
                        registrations.Add(dbRegistration);

                        //check students
                        if (dbClasses == null)
                        {
                            //go to next registration
                            continue;
                        }

                        //find registration class
                        Class classObj = dbClasses.Find(c => c.ClassId == dbRegistration.ClassId);

                        //check result
                        if (classObj == null)
                        {
                            //should never happen
                            //go to next registration
                            continue;
                        }

                        //set class
                        dbRegistration.Class = classObj;

                        //check if teacher should be loaded
                        if (loadTeacher)
                        {
                            //check teachers
                            if (teachers == null)
                            {
                                //get list of teachers
                                teachers = Teacher.Find();

                                //check result
                                if (teachers == null)
                                {
                                    //no teacher was found
                                    //create empty list
                                    teachers = new List<Teacher>();
                                }
                            }

                            //find teacher
                            Teacher teacher = teachers.Find(
                                i => i.TeacherId == classObj.TeacherId);

                            //check result
                            if (teacher != null)
                            {
                                //set teacher name to registration
                                dbRegistration.TeacherName = teacher.Name;
                                dbRegistration.Class.TeacherName = teacher.Name;
                                dbRegistration.Class.TeacherUserId = teacher.UserId;
                            }
                        }

                        //check if semester should be loaded
                        if (loadSemester)
                        {
                            //check semesters
                            if (semesters == null)
                            {
                                //get list of semesters
                                semesters = Semester.Find();

                                //check result
                                if (semesters == null)
                                {
                                    //no semester was found
                                    //create empty list
                                    semesters = new List<Semester>();
                                }
                            }

                            //find semester
                            Semester semester = semesters.Find(
                                i => i.SemesterId == classObj.SemesterId);

                            //check result
                            if (semester != null)
                            {
                                //set semester to registration
                                dbRegistration.Semester = semester;
                                dbRegistration.Class.Semester = semester;
                            }
                        }

                        //count number of registrations with position lower 
                        //than current registration position
                        //update registration position
                        //check if position is not the first position
                        if (dbRegistration.Position > 0)
                        {
                            //get next available position in zero base index
                            dbRegistration.Position = Registration.CountByClass(
                            dbRegistration.ClassId, -1, dbRegistration.Position - 1) + 1 - 1;
                        }
                        else
                        {
                            //no need to do anything
                            //it is the first position already
                            //no difference could exist
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                //error while loading registrations
                //create result and add it to the list
                Registration resultRegistration = new Registration();
                resultRegistration.Result = (int)SelectResult.FatalError;
                resultRegistration.ErrorMessage = ex.Message;
                registrations.Add(resultRegistration);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultRegistration.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return registrations;
        }

        #endregion Registration


        #region Report ****************************************************************

        /// <summary>
        /// Find report by ID.
        /// </summary>
        /// <param name="reportId">
        /// The ID of the selected report.
        /// </param>
        /// <returns>
        /// The selected report.
        /// </returns>
        public Report FindReport(int reportId)
        {
            //the target report
            Report resultReport = null;

            try
            {
                //find report in database
                resultReport = Report.Find(reportId);

                //check result
                if (resultReport != null)
                {
                    //report was found
                    resultReport.Result = (int)SelectResult.Success;
                }
                else
                {
                    //report was not found
                    //create result and set it
                    resultReport = new Report();
                    resultReport.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding report
                //create result and set it
                resultReport = new Report();
                resultReport.Result = (int)SelectResult.FatalError;
                resultReport.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultReport.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultReport;
        }

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
        public List<Report> FindReportsByFilter(
            bool loadSemester, bool loadInstitution, 
            bool loadTeacher, bool loadCoordinator, bool loadClass,
            int filterReportStatus, int filterReportRapporteur, int filterReportPeriodicity,
            int filterSemester, DateTime filterReferenceDate, int filterInstitution, 
            int filterTeacher, int filterClass)
        {
            //create result list
            List<Report> reports = new List<Report>();

            try
            {
                //get list of reports from database using filters
                List<Report> dbReports = Report.FindByFilter(
                    filterReportStatus, filterReportRapporteur, filterReportPeriodicity,
                    filterSemester, filterReferenceDate, filterInstitution, filterTeacher, filterClass);

                //check result
                if (dbReports == null || dbReports.Count == 0)
                {
                    //no report was found
                    //create result and add it to the list
                    Report resultReport = new Report();
                    resultReport.Result = (int)SelectResult.Empty;
                    reports.Add(resultReport);
                }
                else
                {
                    //check each report
                    foreach (Report dbReport in dbReports)
                    {
                        //set and add report
                        dbReport.Result = (int)SelectResult.Success;
                        reports.Add(dbReport);
                    }

                    //check if institution should be loaded
                    if (loadInstitution)
                    {
                        //check filter institution
                        if (filterInstitution > -1)
                        {
                            //get institution
                            Institution institution = Institution.Find(filterInstitution);

                            //check result
                            if (institution != null)
                            {
                                //check each report
                                foreach (Report reportObj in reports)
                                {
                                    //set institution name
                                    reportObj.InstitutionName = institution.ProjectName;
                                }
                            }
                        }
                        else
                        {

                            //get list of institutions
                            List<Institution> institutions = Institution.FindByFilter(-1);

                            //check result
                            if (institutions != null)
                            {
                                //check each report
                                foreach (Report reportObj in reports)
                                {
                                    //find institution
                                    Institution institution = institutions.Find(
                                        i => i.InstitutionId == reportObj.InstitutionId);

                                    //check result
                                    if (institution != null)
                                    {
                                        //set institution name
                                        reportObj.InstitutionName = institution.ProjectName;
                                    }
                                }
                            }
                        }
                    }

                    //check if teacher should be loaded
                    if (loadTeacher)
                    {
                        //check filter teacher
                        if (filterTeacher > -1)
                        {
                            //get teacher
                            Teacher teacher = Teacher.Find(filterTeacher);

                            //check result
                            if (teacher != null)
                            {
                                //check each report
                                foreach (Report reportObj in reports)
                                {
                                    //set teacher name
                                    reportObj.TeacherName = teacher.Name;

                                    //set teacher user id
                                    reportObj.TeacherUserId = teacher.UserId;
                                }
                            }
                        }
                        else
                        {

                            //get list of teachers using filters
                            List<Teacher> teachers = Teacher.FindByFilter(
                                -1, filterInstitution, -1);

                            //check result
                            if (teachers != null)
                            {
                                //check each report
                                foreach (Report reportObj in reports)
                                {
                                    //check if report has no teacher
                                    if (reportObj.TeacherId == int.MinValue)
                                    {
                                        //go to next report
                                        continue;
                                    }

                                    //find teacher
                                    Teacher teacher = teachers.Find(
                                        i => i.TeacherId == reportObj.TeacherId);

                                    //check result
                                    if (teacher == null)
                                    {
                                        //teacher was not found
                                        //teacher might not belong to pole anymore
                                        //find teacher directly
                                        teacher = Teacher.Find(reportObj.TeacherId);

                                        //check result
                                        if (teacher != null)
                                        {
                                            //add teacher to list of teachers
                                            teachers.Add(teacher);
                                        }
                                    }

                                    //check final result
                                    if (teacher != null)
                                    {
                                        //set teacher name
                                        reportObj.TeacherName = teacher.Name;

                                        //set teacher user id
                                        reportObj.TeacherUserId = teacher.UserId;
                                    }
                                    else
                                    {
                                        //should never happen
                                        //set teacher id as teacher name
                                        reportObj.TeacherName = reportObj.TeacherId.ToString();

                                        //reset teacher user id
                                        reportObj.TeacherUserId = int.MinValue;
                                    }
                                }
                            }
                        }
                    }

                    //check if coordinator should be loaded
                    if (loadCoordinator)
                    {
                        //get list of coordinators using filters
                        List<User> users = User.FindAssignedCoordinators();

                        //check result
                        if (users != null)
                        {
                            //check each report
                            foreach (Report reportObj in reports)
                            {
                                //check if report has no coordinator
                                if (reportObj.CoordinatorId == int.MinValue)
                                {
                                    //go to next report
                                    continue;
                                }

                                //find coordinator
                                User coordinator = users.Find(
                                    i => i.UserId == reportObj.CoordinatorId);

                                //check result
                                if (coordinator == null)
                                {
                                    //coordinator was not found
                                    //coordinator might not belong to institution anymore
                                    //find coordinator directly
                                    coordinator = User.Find(reportObj.CoordinatorId);

                                    //check result
                                    if (coordinator != null)
                                    {
                                        //add coordinator to list of coordinators
                                        users.Add(coordinator);
                                    }
                                }

                                //check final result
                                if (coordinator != null)
                                {
                                    //set coordinator name
                                    reportObj.CoordinatorName = coordinator.Name;

                                    //set coordinator user id
                                    reportObj.CoordinatorUserId = coordinator.UserId;
                                }
                                else
                                {
                                    //should never happen
                                    //set coordinator ID as coordinator name
                                    reportObj.CoordinatorName = reportObj.CoordinatorId.ToString();

                                    //reset coordinator user id
                                    reportObj.CoordinatorUserId = int.MinValue;
                                }
                            }
                        }
                    }

                    //check if class should be loaded
                    if (loadClass)
                    {
                        //check filter class
                        if (filterClass > -1)
                        {
                            //get class
                            Class classObj = Class.Find(filterClass);

                            //check result
                            if (classObj != null)
                            {
                                //check each report
                                foreach (Report reportObj in reports)
                                {
                                    //set class
                                    reportObj.Class = classObj;
                                }
                            }
                        }
                        else
                        {
                            //get list of classes using filters
                            List<Class> classes = Class.FindByFilter(
                                -1, -1, -1, -1, filterSemester, filterInstitution, -1, filterTeacher);

                            //check result
                            if (classes != null)
                            {
                                //check each report
                                foreach (Report reportObj in reports)
                                {
                                    //check if report has no class
                                    if (reportObj.ClassId == int.MinValue)
                                    {
                                        //go to next report
                                        continue;
                                    }

                                    //find class
                                    Class classObj = classes.Find(
                                        i => i.ClassId == reportObj.ClassId);

                                    //check result
                                    if (classObj == null)
                                    {
                                        //class was not found
                                        //class might not belong to pole anymore
                                        //find class directly
                                        classObj = Class.Find(reportObj.ClassId);

                                        //check result
                                        if (classObj != null)
                                        {
                                            //add class to list of classes
                                            classes.Add(classObj);
                                        }
                                    }

                                    //check final result
                                    if (classObj != null)
                                    {
                                        //set class
                                        reportObj.Class = classObj;

                                        //check teacher name
                                        if (reportObj.TeacherName != null &&
                                            reportObj.TeacherName.Length > 0)
                                        {
                                            //set teacher name to class
                                            reportObj.Class.TeacherName = reportObj.TeacherName;

                                            //set teacher user id to class
                                            reportObj.Class.TeacherUserId = reportObj.TeacherUserId;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //check if report should be loaded
                    if (loadSemester)
                    {
                        //check filter semester
                        if (filterSemester > -1)
                        {
                            //get semester
                            Semester semester = Semester.Find(filterSemester);

                            //check result
                            if (semester != null)
                            {
                                //check each report
                                foreach (Report reportObj in reports)
                                {
                                    //set semester description
                                    reportObj.SemesterDescription = semester.Description;

                                    //check report class
                                    if (reportObj.Class != null)
                                    {
                                        //set semester to class
                                        reportObj.Class.Semester = semester;
                                    }
                                }
                            }
                        }
                        else
                        {

                            //get list of semesters
                            List<Semester> semesters = Semester.Find();

                            //check result
                            if (semesters != null)
                            {
                                //check each report
                                foreach (Report reportObj in reports)
                                {
                                    //find semester
                                    Semester semester = semesters.Find(
                                        i => i.SemesterId == reportObj.SemesterId);

                                    //check result
                                    if (semester != null)
                                    {
                                        //set semester description
                                        reportObj.SemesterDescription = semester.Description;

                                        //check report class
                                        if (reportObj.Class != null)
                                        {
                                            //set semester to class
                                            reportObj.Class.Semester = semester;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading reports
                //create result and add it to the list
                Report resultReport = new Report();
                resultReport.Result = (int)SelectResult.FatalError;
                resultReport.ErrorMessage = ex.Message;
                reports.Add(resultReport);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultReport.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return reports;
        }

        /// <summary>
        /// Lock user to generate reports.
        /// </summary>
        private static object reportLock = new object();

        /// <summary>
        /// Check generated reports for selected class.
        /// </summary>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <returns>
        /// A list of missing report months for the selected class.
        /// </returns>
        public List<DateTimeResult> CheckReports(int classId)
        {
            //create month result list
            List<DateTimeResult> months = new List<DateTimeResult>();

            try
            {
                //get selected class
                Class classObj = FindClass(classId, false, true);

                //check result
                if (classObj.Result == (int)SelectResult.Empty)
                {
                    //class could not be found
                    DateTimeResult checkResult = new DateTimeResult();
                    checkResult.Result = (int)SelectResult.Empty;
                    months.Insert(0, checkResult);

                    //return result list
                    return months;
                }
                else if (classObj.Result == (int)SelectResult.FatalError)
                {
                    //error while loading class
                    DateTimeResult checkResult = new DateTimeResult();
                    checkResult.Result = (int)SelectResult.FatalError;
                    checkResult.ErrorMessage = classObj.ErrorMessage;
                    months.Insert(0, checkResult);

                    //return result list
                    return months;
                }

                //check if class is in progress
                if (classObj.ClassProgress != (int)ClassProgress.InProgress)
                {
                    //class is not in progress
                    //no need to generate reports
                    DateTimeResult checkResult = new DateTimeResult();
                    checkResult.Result = (int)SelectResult.Empty;
                    months.Insert(0, checkResult);

                    //return result list
                    return months;
                }

                //get class generated reports
                List<Report> reports = FindReportsByFilter(
                    false, false, false, false, false,
                    -1, (int)ReportRapporteur.Teacher, (int)ReportPeriodicity.Month, 
                    -1, DateTime.MinValue, -1, -1, classId);

                //check result
                if (reports[0].Result == (int)SelectResult.Empty)
                {
                    //no report is available
                    //clear list
                    reports.Clear();
                }
                else if (reports[0].Result == (int)SelectResult.FatalError)
                {
                    //error while loading reports
                    DateTimeResult checkResult = new DateTimeResult();
                    checkResult.Result = (int)SelectResult.FatalError;
                    checkResult.ErrorMessage = reports[0].ErrorMessage;
                    months.Insert(0, checkResult);

                    //return result list
                    return months;
                }

                //get class semester
                Semester currentSemester = classObj.Semester;

                //set current month as semester start month
                DateTime referenceDate = new DateTime(
                    classObj.Semester.StartDate.Year,
                    classObj.Semester.StartDate.Month, 1);

                //check each semester month
                while (true)
                {
                    //check if month report should be generated
                    bool generateMonthReports = true;
                        
                    //check if month has enough days
                    //check if it is the starting month
                    if (referenceDate.Month == currentSemester.StartDate.Month)
                    {
                        //calculate number of days until end of first month
                        double days = referenceDate.AddMonths(1).AddDays(-1).Subtract(currentSemester.StartDate).TotalDays + 1.0;

                        //check result
                        if (days < 14.0)
                        {
                            //no enough days
                            //no need to generate reports
                            generateMonthReports = false;
                        }
                    }
                    //check if it is the ending month
                    else if (referenceDate.Month == currentSemester.EndDate.Month)
                    {
                        //calculate number of days until end semester
                        double days = currentSemester.EndDate.Subtract(referenceDate).TotalDays + 1.0;

                        //check result
                        if (days < 14.0)
                        {
                            //no enough days
                            //no need to generate reports
                            generateMonthReports = false;
                        }
                    }

                    //check partial result 
                    if (generateMonthReports)
                    {
                        //calculate the date the report should be generated in the month
                        //set to a week before start of the next month
                        DateTime generationDate = referenceDate.AddMonths(1).AddDays(-7);

                        //check if it is the ending month
                        if (referenceDate.Month == currentSemester.EndDate.Month)
                        {
                            //set to a week before the next day after the end of the semester
                            generationDate = currentSemester.EndDate.AddDays(1).AddDays(-7);
                        }

                        //check generation date 
                        if (DateTime.Today < generationDate)
                        {
                            //still early to generate reports
                            generateMonthReports = false;
                        }
                    }

                    //check result
                    if (generateMonthReports)
                    {
                        //month should have a report for selected class
                        //check if report is already created
                        Report monthReport = reports.Find(
                            r => r.ReferenceDate.Equals(referenceDate));

                        //check result
                        if (monthReport == null)
                        {
                            //must generate report
                            //add month to result
                            DateTimeResult month = new DateTimeResult();
                            month.Result = (int)SelectResult.Success;
                            month.Time = referenceDate;
                            months.Add(month);
                        }
                    }
                    else
                    {
                        //end of month report check
                        //exit loop
                        break;
                    }

                    //increment current month
                    referenceDate = referenceDate.AddMonths(1);
                }

                //check final result
                if (months.Count == 0)
                {
                    //no month report is missing
                    //create result and add it to the list
                    DateTimeResult checkResult = new DateTimeResult();
                    checkResult.Result = (int)SelectResult.Empty;
                    months.Insert(0, checkResult);
                }
            }
            catch (Exception ex)
            {
                //error while checking reports
                //create result and add it to the list
                DateTimeResult checkResult = new DateTimeResult();
                checkResult.Result = (int)SelectResult.FatalError;
                checkResult.ErrorMessage = ex.Message;
                months.Insert(0, checkResult);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    checkResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return months;
        }

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
        public SaveResult GenerateReport(int classId, DateTime referenceDate)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            try
            {
                //get selected class
                Class classObj = Class.Find(classId);

                //check result
                if (classObj == null)
                {
                    //selected class was not found
                    //set result
                    saveResult.Result = (int)SelectResult.FatalError;
                    saveResult.ErrorMessage = "Selected class was not found in database.";

                    //return result
                    return saveResult;
                }

                //get selected class pole
                Pole pole = Pole.Find(classObj.PoleId);

                //check result
                if (pole == null)
                {
                    //pole was not found
                    //should never happen
                    //set result
                    saveResult.Result = (int)SelectResult.FatalError;
                    saveResult.ErrorMessage = "Pole of selected class was not found in database.";

                    //return result
                    return saveResult;
                }
                
                //check if report exists
                List<Report> reports = Report.FindByFilter(
                    -1, (int)ReportRapporteur.Teacher, (int)ReportPeriodicity.Month,
                    -1, referenceDate, -1, -1, classId);

                //check result
                if (reports != null)
                {
                    //set result
                    saveResult.Result = (int)SelectResult.FatalError;
                    saveResult.ErrorMessage = "Report already exists.";

                    //return result
                    return saveResult;
                }

                //create and set report
                Report report = new Report();
                report.ReportId = -1;
                report.ClassId = classObj.ClassId;
                report.CoordinatorId = int.MinValue;
                report.InstitutionId = pole.InstitutionId;
                report.ReferenceDate = referenceDate;
                report.ReportPeriodicity = (int)ReportPeriodicity.Month;
                report.ReportRapporteur = (int)ReportRapporteur.Teacher;
                report.ReportStatus = (int)ReportStatus.Pending;
                report.ReportTarget = (int)ReportTarget.Class;
                report.SemesterId = classObj.SemesterId;
                report.TeacherId = classObj.TeacherId;

                //save report
                report.Save();

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving report
                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return saveResult;
        }

        /// <summary>
        /// Generate reports for current semester month.
        /// </summary>
        /// <returns>
        /// The number of generated reports.
        /// </returns>
        public CountResult GenerateReports()
        {
            //create result and set empty result
            CountResult countResult = new CountResult();
            countResult.Result = (int)SelectResult.Empty;
            countResult.Count = 0;

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //get current semester
                Semester currentSemester = FindCurrentSemester();

                //check result
                if (currentSemester.Result == (int)SelectResult.Empty)
                {
                    //no current semester
                    //set result
                    countResult.Result = (int)SelectResult.FatalError;
                    countResult.ErrorMessage = "No current semester.";

                    //return result
                    return countResult;
                }
                else if (currentSemester.Result == (int)SelectResult.FatalError)
                {
                    //no current semester
                    //set result
                    countResult.Result = (int)SelectResult.FatalError;
                    countResult.ErrorMessage = "Unexpected error while reading semester.";

                    //return result
                    return countResult;
                }

                //check if semester has not started yet
                if (DateTime.Today < currentSemester.StartDate)
                {
                    //semester has not started yet
                    //return result
                    return countResult;
                }

                //check if semester has ended
                if (DateTime.Today > currentSemester.EndDate)
                {
                    //semester has ended
                    return countResult;
                }

                //list of semester classes
                List<Class> classes = null;

                //list of coordinators
                List<User> coordinators = null;

                //list of institutions
                List<Institution> institutions = null;

                //list of poles
                List<Pole> poles = null;

                //set reference date to current month
                DateTime referenceDate = new DateTime(
                    DateTime.Now.Year, DateTime.Now.Month, 1);
                
                //check if month report should be generated
                bool generateMonthReports = true;

                //check if month has enough days
                //check if it is the starting month
                if (referenceDate.Month == currentSemester.StartDate.Month)
                {
                    //calculate number of days until end of first month
                    double days = referenceDate.AddMonths(1).AddDays(-1).Subtract(currentSemester.StartDate).TotalDays + 1.0;

                    //check result
                    if (days < 14.0)
                    {
                        //no enough days
                        //no need to generate reports
                        generateMonthReports = false;
                    }
                }
                //check if it is the ending month
                else if (referenceDate.Month == currentSemester.EndDate.Month)
                {
                    //calculate number of days until end semester
                    double days = currentSemester.EndDate.Subtract(referenceDate).TotalDays + 1.0;

                    //check result
                    if (days < 14.0)
                    {
                        //no enough days
                        //no need to generate reports
                        generateMonthReports = false;
                    }
                }

                //check partial result 
                if (generateMonthReports)
                {
                    //calculate the date the report should be generated in the month
                    //set to a week before start of the next month
                    DateTime generationDate = referenceDate.AddMonths(1).AddDays(-7);
                        
                    //check if it is the ending month
                    if (referenceDate.Month == currentSemester.EndDate.Month)
                    {
                        //set to a week before the next day after the end of the semester
                        generationDate = currentSemester.EndDate.AddDays(1).AddDays(-7);
                    }

                    //check generation date 
                    if (DateTime.Today < generationDate)
                    {
                        //still early to generate reports
                        generateMonthReports = false;
                    }
                }

                //check result
                if (generateMonthReports)
                {
                    //get reports for current month
                    List<Report> reports = Report.FindByFilter(
                        -1, -1, (int)ReportPeriodicity.Month,
                        currentSemester.SemesterId, referenceDate, -1, -1, -1);

                    //check result
                    if (reports == null)
                    {
                        //use lock
                        lock (reportLock)
                        {
                            //check and generate reports for current month
                            //open connection and begin a transaction
                            connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                            connection.Open();
                            transaction = connection.BeginTransaction();

                            //generate teacher month reports for classes
                            #region generate teacher month reports

                            //get list of all poles
                            poles = Pole.Find();

                            //get all active classes for current semester
                            classes = Class.FindByFilter(
                                (int)ItemStatus.Active, -1, -1, -1,
                                currentSemester.SemesterId, -1, -1, -1);

                            //check result
                            if (classes == null)
                            {
                                //no class
                                //set empty list
                                classes = new List<Class>();
                            }

                            //create a report for each class
                            foreach (Class classObj in classes)
                            {
                                //create and set report
                                Report report = new Report();
                                report.ReportId = -1;
                                report.ClassId = classObj.ClassId;
                                report.CoordinatorId = int.MinValue;
                                report.InstitutionId = (poles.Find(
                                    p => p.PoleId == classObj.PoleId)).InstitutionId;
                                report.ReferenceDate = referenceDate;
                                report.ReportPeriodicity = (int)ReportPeriodicity.Month;
                                report.ReportRapporteur = (int)ReportRapporteur.Teacher;
                                report.ReportStatus = (int)ReportStatus.Pending;
                                report.ReportTarget = (int)ReportTarget.Class;
                                report.SemesterId = currentSemester.SemesterId;
                                report.TeacherId = classObj.TeacherId;

                                //save report
                                report.Save(transaction);

                                //increment number of reports
                                countResult.Count++;
                            }

                            #endregion generate teacher month reports

                            //generate coordinator month reports for each institution
                            #region generate coordinator month reports

                            //get list of all institutions
                            institutions = Institution.Find();

                            //get list of all coordinators
                            coordinators = User.FindAssignedCoordinators();

                            //create a report for each innstitution
                            foreach (Institution institution in institutions)
                            {
                                //institution must have one class at least
                                bool hasClass = false;

                                //find institution pole
                                List<Pole> institutionPoles = poles.FindAll(
                                    p => p.InstitutionId == institution.InstitutionId);

                                //check result
                                if (institutionPoles == null)
                                {
                                    //create empty list
                                    institutionPoles = new List<Pole>();
                                }

                                //check each pole
                                foreach (Pole pole in institutionPoles)
                                {
                                    //get pole classes
                                    List<Class> poleClasses = classes.FindAll(
                                        p => p.PoleId == pole.PoleId);

                                    //check result
                                    if (poleClasses != null && poleClasses.Count > 0)
                                    {
                                        //found class
                                        hasClass = true;

                                        //exit loop
                                        break;
                                    }
                                }

                                //check final class result
                                if (!hasClass)
                                {
                                    //go to next institution
                                    continue;
                                }

                                //create and set report
                                Report report = new Report();
                                report.ReportId = -1;
                                report.ClassId = int.MinValue;
                                report.CoordinatorId = (coordinators.Find(
                                    c => c.UserId == institution.CoordinatorId)).UserId;
                                report.InstitutionId = institution.InstitutionId;
                                report.ReferenceDate = referenceDate;
                                report.ReportPeriodicity = (int)ReportPeriodicity.Month;
                                report.ReportRapporteur = (int)ReportRapporteur.Coordinator;
                                report.ReportStatus = (int)ReportStatus.Pending;
                                report.ReportTarget = (int)ReportTarget.Institution;
                                report.SemesterId = currentSemester.SemesterId;
                                report.TeacherId = int.MinValue;

                                //save report
                                report.Save(transaction);

                                //increment number of reports
                                countResult.Count++;
                            }

                            #endregion generate coordinator month reports
                        }
                    }
                }

                //check if semester report should already be generated
                if (DateTime.Today >= currentSemester.EndDate.AddDays(1).AddMonths(-1))
                {
                    //get reports for current semester
                    List<Report> reports = Report.FindByFilter(
                        -1, -1, (int)ReportPeriodicity.Semester,
                        currentSemester.SemesterId, DateTime.MinValue, -1, -1, -1);

                    //check result
                    if (reports == null)
                    {
                        //use lock
                        lock (reportLock)
                        {
                            //check and generate reports for current semester
                            //check transaction
                            if (transaction == null)
                            {
                                //open connection and begin a transaction
                                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                                connection.Open();
                                transaction = connection.BeginTransaction();
                            }

                            //generate teacher semester reports for classes
                            #region generate teacher semester reports

                            //check poles
                            if (poles == null)
                            {
                                //get list of all poles
                                poles = Pole.Find();
                            }

                            //check classes
                            if (classes == null)
                            {
                                //get all active classes for current semester
                                classes = Class.FindByFilter(
                                    (int)ItemStatus.Active, -1, -1, -1,
                                    currentSemester.SemesterId, -1, -1, -1);

                                //check result
                                if (classes == null)
                                {
                                    //no class
                                    //set empty list
                                    classes = new List<Class>();
                                }
                            }

                            //create a report for each class
                            foreach (Class classObj in classes)
                            {
                                //create and set report
                                Report report = new Report();
                                report.ReportId = -1;
                                report.ClassId = classObj.ClassId;
                                report.CoordinatorId = int.MinValue;
                                report.InstitutionId = (poles.Find(
                                    p => p.PoleId == classObj.PoleId)).InstitutionId;
                                report.ReferenceDate = currentSemester.ReferenceDate;
                                report.ReportPeriodicity = (int)ReportPeriodicity.Semester;
                                report.ReportRapporteur = (int)ReportRapporteur.Teacher;
                                report.ReportStatus = (int)ReportStatus.Pending;
                                report.ReportTarget = (int)ReportTarget.Class;
                                report.SemesterId = currentSemester.SemesterId;
                                report.TeacherId = classObj.TeacherId;

                                //save report
                                report.Save(transaction);

                                //increment number of reports
                                countResult.Count++;
                            }

                            #endregion generate teacher semester reports

                            //generate coordinator semester reports for each institution
                            #region generate coordinator semester reports

                            //check institutions
                            if (institutions == null)
                            {
                                //get list of all institutions
                                institutions = Institution.Find();
                            }

                            //check coordinators
                            if (coordinators == null)
                            {
                                //get list of all coordinators
                                coordinators = User.FindAssignedCoordinators();
                            }

                            //create a report for each innstitution
                            foreach (Institution institution in institutions)
                            {
                                //institution must have one class at least
                                bool hasClass = false;

                                //find institution pole
                                List<Pole> institutionPoles = poles.FindAll(
                                    p => p.InstitutionId == institution.InstitutionId);

                                //check result
                                if (institutionPoles == null)
                                {
                                    //create empty list
                                    institutionPoles = new List<Pole>();
                                }

                                //check each pole
                                foreach (Pole pole in institutionPoles)
                                {
                                    //get pole classes
                                    List<Class> poleClasses = classes.FindAll(
                                        p => p.PoleId == pole.PoleId);

                                    //check result
                                    if (poleClasses != null && poleClasses.Count > 0)
                                    {
                                        //found class
                                        hasClass = true;

                                        //exit loop
                                        break;
                                    }
                                }

                                //check final class result
                                if (!hasClass)
                                {
                                    //go to next institution
                                    continue;
                                }

                                //create and set report
                                Report report = new Report();
                                report.ReportId = -1;
                                report.ClassId = int.MinValue;
                                report.CoordinatorId = (coordinators.Find(
                                    c => c.UserId == institution.CoordinatorId)).UserId;
                                report.InstitutionId = institution.InstitutionId;
                                report.ReferenceDate = currentSemester.ReferenceDate;
                                report.ReportPeriodicity = (int)ReportPeriodicity.Semester;
                                report.ReportRapporteur = (int)ReportRapporteur.Coordinator;
                                report.ReportStatus = (int)ReportStatus.Pending;
                                report.ReportTarget = (int)ReportTarget.Institution;
                                report.SemesterId = currentSemester.SemesterId;
                                report.TeacherId = int.MinValue;

                                //save report
                                report.Save(transaction);

                                //increment number of reports
                                countResult.Count++;
                            }

                            #endregion generate coordinator semester reports
                        }
                    }
                }

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                //error while saving role
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //error while loading students
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //check final result
            if (countResult.Count > 0)
            {
                //set result
                countResult.Result = (int)SelectResult.Success;
            }

            //return result
            return countResult;
        }

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
        public SaveResult SaveReport(
            Report report, List<Attendance> attendances, List<Grade> grades, List<Answer> answers)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //save report and get row ID
                saveResult.SavedId = report.Save(transaction);

                //check attendances
                if (attendances != null)
                {
                    //check each attendance
                    foreach (Attendance attendance in attendances)
                    {
                        //save attendance in database
                        attendance.Save(transaction);
                    }
                }

                //check grades
                if (grades != null)
                {
                    //check each grade
                    foreach (Grade grade in grades)
                    {
                        //save grade in database
                        grade.Save(transaction);
                    }
                }

                //check answers
                if (answers != null)
                {
                    //check each answer
                    foreach (Answer answer in answers)
                    {
                        //save answer in database
                        answer.Save(transaction);
                    }
                }

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving report
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return saveResult;
        }

        #endregion Report


        #region Role ******************************************************************

        /// <summary>
        /// Copy role by ID.
        /// </summary>
        /// <param name="roleId">
        /// The ID of the selected role.
        /// </param>
        /// <returns>
        /// The role copy.
        /// </returns>
        public Role CopyRole(int roleId)
        {
            //the copy result to be returned
            //set empty role
            Role copiedRole = new Role();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //get role
                Role dbRole = Role.Find(transaction, roleId);

                //check result
                if (dbRole == null)
                {
                    //role was not found
                    //shold never happen
                    //set result
                    copiedRole.Result = (int)SelectResult.FatalError;
                    copiedRole.ErrorMessage = "Role " + roleId + " not found.";

                    //return result
                    return copiedRole;
                }

                //alter role before saving it as a copy
                dbRole.RoleId = -1;
                dbRole.Name += " - Copy";
                dbRole.CreationTime = DateTime.Now;

                //save altered role and get row ID
                int savedId = dbRole.Save(transaction);
                
                //get list of rolepermissions for original role
                List<Rolepermission> dbRolePermissions =
                    Rolepermission.FindByRole(transaction, roleId);

                //check result
                if (dbRolePermissions != null)
                {
                    //check each rolepermission
                    foreach (Rolepermission dbRolePermission in dbRolePermissions)
                    {
                        //reset id
                        dbRolePermission.RolePermissionId = -1;

                        //set copied role to rolepermission
                        dbRolePermission.RoleId = savedId;

                        //save role permission
                        dbRolePermission.Save(transaction);
                    }
                }

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                copiedRole = dbRole;
                copiedRole.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving role
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                copiedRole.Result = (int)SelectResult.FatalError;
                copiedRole.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    copiedRole.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return copiedRole;
        }

        /// <summary>
        /// Delete role by ID.
        /// </summary>
        /// <param name="roleId">
        /// The ID of the selected role.
        /// </param>
        /// <returns>
        /// The delete operation result.
        /// </returns>
        public DeleteResult DeleteRole(int roleId)
        {
            //the delete result to be returned
            DeleteResult deleteResult = new DeleteResult();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();
                
                //get list of rolepermissions for role
                List<Rolepermission> dbRolePermissions =
                    Rolepermission.FindByRole(transaction, roleId);

                //check result
                if (dbRolePermissions != null)
                {
                    //check each rolepermission
                    foreach (Rolepermission dbRolePermission in dbRolePermissions)
                    {
                        //delete rolepermission
                        Rolepermission.Delete(transaction, dbRolePermission.RolePermissionId);
                    }
                }

                //delete selected role
                if (Role.Delete(transaction, roleId))
                {
                    //role was inactivated
                    //set result
                    deleteResult.Result = (int)SelectResult.Success;
                }
                else
                {
                    //role was not found
                    //set result
                    deleteResult.Result = (int)SelectResult.Empty;
                }

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                //error while deleting role
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                deleteResult.Result = (int)SelectResult.FatalError;
                deleteResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    deleteResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return deleteResult;
        }

        /// <summary>
        /// Find role by ID.
        /// </summary>
        /// <param name="roleId">
        /// The ID of the selected role.
        /// </param>
        /// <returns>
        /// The selected role.
        /// </returns>
        public Role FindRole(int roleId)
        {
            //the target role
            Role resultRole = null;

            try
            {
                //find role in database
                resultRole = Role.Find(roleId);

                //check result
                if (resultRole != null)
                {
                    //role was found
                    resultRole.Result = (int)SelectResult.Success;
                }
                else
                {
                    //role was not found
                    //create result and set it
                    resultRole = new Role();
                    resultRole.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding role
                //create result and set it
                resultRole = new Role();
                resultRole.Result = (int)SelectResult.FatalError;
                resultRole.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultRole.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultRole;
        }

        /// <summary>
        /// Get list of role descriptions.
        /// </summary>
        /// <returns>
        /// The list of role descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListRoles()
        {
            //create result list
            List<IdDescriptionStatus> roles = new List<IdDescriptionStatus>();

            try
            {
                //get list of all roles from database
                List<Role> dbRoles = Role.Find();

                //check result
                if (dbRoles == null || dbRoles.Count == 0)
                {
                    //no role was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    roles.Add(resultItem);
                }
                else
                {
                    //check each role
                    foreach (Role dbRole in dbRoles)
                    {
                        //get description
                        IdDescriptionStatus item = dbRole.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        roles.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading roles
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                roles.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return roles;
        }

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
        public SaveResult SaveRole(Role role, List<int> permissionIds, List<int> addedUserIds)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            //check permission ids
            if (permissionIds == null)
            {
                //no permission set to role
                //set empty list
                permissionIds = new List<int>();
            }

            //check users id
            if (addedUserIds == null)
            {
                //no newly added users
                //set empty list
                addedUserIds = new List<int>();
            }

            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //save role and get row ID
                saveResult.SavedId = role.Save(transaction);
                
                //get list of rolepermissions for saved role
                List<Rolepermission> dbRolePermissions = 
                    Rolepermission.FindByRole(transaction, saveResult.SavedId);

                //check result
                if (dbRolePermissions == null)
                {
                    //no rolepermission in database
                    //set empty list
                    dbRolePermissions = new List<Rolepermission>();
                }

                //check each assigned permission
                foreach (int permissionId in permissionIds)
                {
                    //check if assigned permission is not in database yet
                    if (dbRolePermissions.Find(rp => rp.PermissionId == permissionId) == null)
                    {
                        //must add new role permission relation
                        //create and save new role permission
                        Rolepermission rolePermission = new Rolepermission();
                        rolePermission.RolePermissionId = -1;
                        rolePermission.RoleId = saveResult.SavedId;
                        rolePermission.PermissionId = permissionId;
                        rolePermission.Save(transaction);
                    }
                }

                //must removed old role permission relations
                //check each previously relations
                foreach (Rolepermission rolePermission in dbRolePermissions)
                {
                    //check if relation is not in the new list
                    if (!permissionIds.Contains(rolePermission.PermissionId))
                    {
                        //delete relation from database
                        Rolepermission.Delete(transaction, rolePermission.RolePermissionId);
                    }
                }

                //save each user to whom the role was set
                //check each added user id
                foreach (int userId in addedUserIds)
                {
                    //set role to user
                    User.SetRole(transaction, userId, saveResult.SavedId);
                }

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving role
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return saveResult;
        }

        #endregion Role


        #region Semester **************************************************************

        /// <summary>
        /// Find current ongoing semester.
        /// </summary>
        /// <returns>
        /// The current ongoing semester.
        /// </returns>
        public Semester FindCurrentSemester()
        {
            //the target semester
            Semester resultSemester = null;

            try
            {
                //get list of all semester from database
                List<Semester> dbSemesters = Semester.Find();

                //check result
                if (dbSemesters != null && dbSemesters.Count > 0)
                {
                    //get today
                    DateTime today = DateTime.Today;

                    //get reference date
                    DateTime referenceDate = today.Month > 6 ?
                        new DateTime(today.Year, 7, 1) : new DateTime(today.Year, 1, 1);

                    //check each semester
                    foreach (Semester dbSemester in dbSemesters)
                    {
                        //check end date
                        if (dbSemester.EndDate >= today)
                        {
                            //semester is still going on
                            //found current semester
                            resultSemester = dbSemester;
                            resultSemester.Result = (int)SelectResult.Success;

                            //exit loop
                            break;
                        }

                        //compare reference dates
                        if (dbSemester.ReferenceDate.Equals(referenceDate))
                        {
                            //found current semester
                            resultSemester = dbSemester;
                            resultSemester.Result = (int)SelectResult.Success;

                            //exit loop
                            break;
                        }
                    }

                    //check result
                    if (resultSemester == null)
                    {
                        //create current semester
                        Semester currentSemester = new Semester();
                        currentSemester.SemesterId = -1;
                        currentSemester.ReferenceDate = new DateTime(
                            DateTime.Now.Year, DateTime.Now.Month > 6 ? 7 : 1, 1);
                        currentSemester.StartDate = currentSemester.ReferenceDate.AddMonths(1);
                        currentSemester.EndDate = currentSemester.ReferenceDate.AddMonths(5);

                        //save semester
                        int savedId = currentSemester.Save();

                        //set saved semester
                        currentSemester.SemesterId = savedId;

                        //set result
                        resultSemester = currentSemester;
                        resultSemester.Result = (int)SelectResult.Success;
                    }
                }

                //check result
                if (resultSemester == null)
                {
                    //create result and set it
                    resultSemester = new Semester();
                    resultSemester.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding semester
                //create result and set it
                resultSemester = new Semester();
                resultSemester.Result = (int)SelectResult.FatalError;
                resultSemester.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultSemester.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultSemester;
        }

        /// <summary>
        /// Find semester by ID.
        /// </summary>
        /// <param name="semesterId">
        /// The ID of the selected semester.
        /// </param>
        /// <returns>
        /// The selected semester.
        /// </returns>
        public Semester FindSemester(int semesterId)
        {
            //the target semester
            Semester resultSemester = null;

            try
            {
                //find semester in database
                resultSemester = Semester.Find(semesterId);

                //check result
                if (resultSemester != null)
                {
                    //semester was found
                    resultSemester.Result = (int)SelectResult.Success;
                }
                else
                {
                    //semester was not found
                    //create result and set it
                    resultSemester = new Semester();
                    resultSemester.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding semester
                //create result and set it
                resultSemester = new Semester();
                resultSemester.Result = (int)SelectResult.FatalError;
                resultSemester.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultSemester.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultSemester;
        }

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
        public List<Semester> FindSemesters(bool excludePastSemesters)
        {
            //create result list
            List<Semester> semesters = new List<Semester>();

            try
            {
                //get list of all semester from database
                List<Semester> dbSemesters = Semester.Find();

                //check result
                if (dbSemesters != null && dbSemesters.Count > 0)
                {
                    //check each semester
                    foreach (Semester dbSemester in dbSemesters)
                    {
                        //check if semester should be removed
                        if (excludePastSemesters &&
                            dbSemester.EndDate < DateTime.Today &&
                            dbSemester.ReferenceDate.AddMonths(6) < DateTime.Today)
                        {
                            //past semester
                            //go to next semester
                            continue;
                        }

                        //set and add semester
                        dbSemester.Result = (int)SelectResult.Success;
                        semesters.Add(dbSemester);
                    }
                }

                //check result
                if (semesters.Count == 0)
                {
                    //no semester was found
                    //create result and add it to the list
                    Semester resultSemester = new Semester();
                    resultSemester.Result = (int)SelectResult.Empty;
                    semesters.Add(resultSemester);
                }
            }
            catch (Exception ex)
            {
                //error while loading semesters
                //create result and add it to the list
                Semester resultSemester = new Semester();
                resultSemester.Result = (int)SelectResult.FatalError;
                resultSemester.ErrorMessage = ex.Message;
                semesters.Add(resultSemester);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultSemester.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return semesters;
        }

        /// <summary>
        /// Get list of semester descriptions.
        /// </summary>
        /// <returns>
        /// The list of semester descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListSemesters()
        {
            //create result list
            List<IdDescriptionStatus> semesters = new List<IdDescriptionStatus>();

            try
            {
                //get list of all semesters from database
                List<Semester> dbSemesters = Semester.Find();

                //check result
                if (dbSemesters == null || dbSemesters.Count == 0)
                {
                    //no semester was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    semesters.Add(resultItem);
                }
                else
                {
                    //check each semester
                    foreach (Semester dbSemester in dbSemesters)
                    {
                        //get description
                        IdDescriptionStatus item = dbSemester.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        semesters.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading semesters
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                semesters.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return semesters;
        }

        /// <summary>
        /// Update semester to database.
        /// </summary>
        /// <param name="semester">
        /// The semester to be updated.
        /// </param>
        /// <returns>
        /// The save operation result.
        /// </returns>
        public SaveResult UpdateSemester(Semester semester)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            //check semester
            if (semester.SemesterId <= 0)
            {
                //semester can only be updated
                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = "Invalid ID for updated semester.";

                //return result
                return saveResult;
            }

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //save semester and get row ID
                saveResult.SavedId = semester.Save(transaction);

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving semester
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return saveResult;
        }

        #endregion Semester


        #region Student ***************************************************************

        /// <summary>
        /// Copy student by ID.
        /// </summary>
        /// <param name="studentId">
        /// The ID of the selected student.
        /// </param>
        /// <returns>
        /// The student copy.
        /// </returns>
        public Student CopyStudent(int studentId)
        {
            //the copy result to be returned
            //set empty student
            Student copiedStudent = new Student();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //get student
                Student dbStudent = Student.Find(transaction, studentId);

                //check result
                if (dbStudent == null)
                {
                    //student was not found
                    //shold never happen
                    //set result
                    copiedStudent.Result = (int)SelectResult.FatalError;
                    copiedStudent.ErrorMessage = "Student " + studentId + " not found.";

                    //return result
                    return copiedStudent;
                }

                //alter student before saving it as a copy
                dbStudent.StudentId = -1;
                dbStudent.CreationTime = DateTime.Now;
                dbStudent.InactivationReason = string.Empty;
                dbStudent.InactivationTime = DateTime.MinValue;
                dbStudent.StudentStatus = (int)ItemStatus.Active;

                // set a sufix to name
                dbStudent.Name += " - Copy";

                //save altered student and get row ID
                int savedId = dbStudent.Save(transaction);

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                copiedStudent = dbStudent;
                copiedStudent.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving student
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                copiedStudent.Result = (int)SelectResult.FatalError;
                copiedStudent.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    copiedStudent.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return copiedStudent;
        }

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
        public CountResult CountStudentsByFilter(
            int filterStudentStatus, int filterInstitution, int filterPole)
        {
            //create result
            CountResult countResult = new CountResult();

            try
            {
                //count students in database and set result
                countResult.Count = Student.CountByFilter(
                    filterStudentStatus, filterInstitution, filterPole);
            }
            catch (Exception ex)
            {
                //error while loading students
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return countResult;
        }

        /// <summary>
        /// Find student by ID.
        /// </summary>
        /// <param name="studentId">
        /// The ID of the selected student.
        /// </param>
        /// <returns>
        /// The selected student.
        /// </returns>
        public Student FindStudent(int studentId)
        {
            //the target student
            Student resultStudent = null;

            try
            {
                //find student in database
                resultStudent = Student.Find(studentId);

                //check result
                if (resultStudent != null)
                {
                    //student was found
                    resultStudent.Result = (int)SelectResult.Success;
                }
                else
                {
                    //student was not found
                    //create result and set it
                    resultStudent = new Student();
                    resultStudent.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding student
                //create result and set it
                resultStudent = new Student();
                resultStudent.Result = (int)SelectResult.FatalError;
                resultStudent.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultStudent.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultStudent;
        }

        /// <summary>
        /// Find next available student without a loan for selected pole.
        /// </summary>
        /// <param name="studentId">
        /// The ID of the selected pole.
        /// </param>
        /// <returns>
        /// The next available student.
        /// </returns>
        public Student FindNextStudentWithoutLoan(int poleId)
        {
            //the target student
            Student resultStudent = null;

            try
            {
                //get list of active students from database for selected pole
                List<Student> dbStudents = Student.FindByFilter((int)ItemStatus.Active, -1, poleId);

                //get list of active loans from database for selected pole
                List<Loan> dbLoans = Loan.FindByPole(poleId, (int)ItemStatus.Active);

                //check each student
                foreach (Student student in dbStudents)
                {
                    //check if student has no loan
                    if (dbLoans.Find(l => l.StudentId == student.StudentId) == null)
                    {
                        //found next available student
                        resultStudent = student;

                        //set result
                        resultStudent.Result = (int)SelectResult.Success;

                        //exit loop
                        break;
                    }
                }

                //check final result
                if (resultStudent == null)
                {
                    //no student is available
                    //create result and set it
                    resultStudent = new Student();
                    resultStudent.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding student
                //create result and set it
                resultStudent = new Student();
                resultStudent.Result = (int)SelectResult.FatalError;
                resultStudent.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultStudent.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultStudent;
        }

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
        public List<Student> FindStudentsByFilter(bool loadUser, bool loadPole, 
            int filterStudentStatus, int filterInstitution, int filterPole)
        {
            //create result list
            List<Student> students = new List<Student>();

            try
            {
                //get list of students from database using filters
                List<Student> dbStudents = Student.FindByFilter(
                    filterStudentStatus, filterInstitution, filterPole);

                //check result
                if (dbStudents == null || dbStudents.Count == 0)
                {
                    //no student was found
                    //create result and add it to the list
                    Student resultStudent = new Student();
                    resultStudent.Result = (int)SelectResult.Empty;
                    students.Add(resultStudent);
                }
                else
                {
                    //check each student
                    foreach (Student dbStudent in dbStudents)
                    {
                        //set and add student
                        dbStudent.Result = (int)SelectResult.Success;
                        students.Add(dbStudent);
                    }

                    //check if user should be loaded
                    if (loadUser)
                    {
                        //get list of student users
                        List<User> users = User.FindStudent();

                        //check result
                        if (users != null)
                        {
                            //check each student
                            foreach (Student student in students)
                            {
                                //find user
                                User user = users.Find(
                                    i => i.UserId == student.UserId);

                                //check result
                                if (user != null)
                                {
                                    //set user login
                                    student.UserLogin = user.Login;
                                }
                            }
                        }
                    }

                    //check if pole should be loaded
                    if (loadPole)
                    {
                        //get list of all poles
                        List<Pole> poles = Pole.Find();

                        //check result
                        if (poles != null)
                        {
                            //check each student
                            foreach (Student student in students)
                            {
                                //find pole
                                Pole pole = poles.Find(
                                    p => p.PoleId == student.PoleId);

                                //check result
                                if (pole != null)
                                {
                                    //set pole name
                                    student.PoleName = pole.Name;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading students
                //create result and add it to the list
                Student resultStudent = new Student();
                resultStudent.Result = (int)SelectResult.FatalError;
                resultStudent.ErrorMessage = ex.Message;
                students.Add(resultStudent);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultStudent.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return students;
        }

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
        public DeleteResult InactivateStudent(int studentId, string inactivationReason)
        {
            //the inactivate result to be returned
            DeleteResult inactivateResult = new DeleteResult();

            try
            {
                //inactivate selected student
                if (Student.Inactivate(studentId, inactivationReason))
                {
                    //student was inactivated
                    //set result
                    inactivateResult.Result = (int)SelectResult.Success;
                }
                else
                {
                    //student was not found
                    //set result
                    inactivateResult.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while inactivating student
                //set result
                inactivateResult.Result = (int)SelectResult.FatalError;
                inactivateResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    inactivateResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return inactivateResult;
        }

        /// <summary>
        /// Get list of student descriptions.
        /// </summary>
        /// <returns>
        /// The list of student descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListStudents()
        {
            //create result list
            List<IdDescriptionStatus> students = new List<IdDescriptionStatus>();

            try
            {
                //get list of all students from database
                List<Student> dbStudents = Student.Find();

                //check result
                if (dbStudents == null || dbStudents.Count == 0)
                {
                    //no student was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    students.Add(resultItem);
                }
                else
                {
                    //check each student
                    foreach (Student dbStudent in dbStudents)
                    {
                        //get description
                        IdDescriptionStatus item = dbStudent.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        students.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading students
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                students.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return students;
        }

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
        public List<IdDescriptionStatus> ListStudentsByClass(int classId, int status)
        {
            //create result list
            List<IdDescriptionStatus> students = new List<IdDescriptionStatus>();

            try
            {
                //get list of students from database
                List<Student> dbStudents = Student.FindByClass(classId);

                //check result
                if (dbStudents != null)
                {
                    //check each student
                    foreach (Student dbStudent in dbStudents)
                    {
                        //check status filter
                        if (status != -1 && 
                            dbStudent.StudentStatus != status)
                        {
                            //go to next student
                            continue;
                        }

                        //get description
                        IdDescriptionStatus item = dbStudent.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        students.Add(item);
                    }
                }

                //check final result
                if (students.Count == 0)
                {
                    //no student was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    students.Add(resultItem);
                }
            }
            catch (Exception ex)
            {
                //error while loading students
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                students.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return students;
        }

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
        public List<IdDescriptionStatus> ListStudentsByInstitution(int institutionId, int status)
        {
            //create result list
            List<IdDescriptionStatus> students = new List<IdDescriptionStatus>();

            try
            {
                //get list of students from database
                List<Student> dbStudents = Student.FindByFilter(status, institutionId, -1);

                //check result
                if (dbStudents == null || dbStudents.Count == 0)
                {
                    //no student was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    students.Add(resultItem);
                }
                else
                {
                    //check each student
                    foreach (Student dbStudent in dbStudents)
                    {
                        //get description
                        IdDescriptionStatus item = dbStudent.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        students.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading students
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                students.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return students;
        }

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
        public List<IdDescriptionStatus> ListStudentsByStatus(int status)
        {
            //create result list
            List<IdDescriptionStatus> students = new List<IdDescriptionStatus>();

            try
            {
                //get list of students from database
                List<Student> dbStudents = Student.FindByFilter(status, -1, -1);

                //check result
                if (dbStudents == null || dbStudents.Count == 0)
                {
                    //no student was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    students.Add(resultItem);
                }
                else
                {
                    //check each student
                    foreach (Student dbStudent in dbStudents)
                    {
                        //get description
                        IdDescriptionStatus item = dbStudent.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        students.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading students
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                students.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return students;
        }

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
        public List<IdDescriptionStatus> ListStudentsByPole(int poleId, int status)
        {
            //create result list
            List<IdDescriptionStatus> students = new List<IdDescriptionStatus>();

            try
            {
                //get list of students from database
                List<Student> dbStudents = Student.FindByFilter(status, -1, poleId);

                //check result
                if (dbStudents == null || dbStudents.Count == 0)
                {
                    //no student was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    students.Add(resultItem);
                }
                else
                {
                    //check each student
                    foreach (Student dbStudent in dbStudents)
                    {
                        //get description
                        IdDescriptionStatus item = dbStudent.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        students.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading students
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                students.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return students;
        }

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
        public SaveResult SaveStudent(
            Student student, List<Registration> registrations, 
            List<Loan> loans, File photo, File assignment)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;
               
            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //save student and get row ID
                saveResult.SavedId = student.Save(transaction);

                //check registrations
                if (registrations == null)
                {
                    //create empty list
                    registrations = new List<Registration>();
                }

                //check each registration
                foreach (Registration registration in registrations)
                {
                    //set student ID to registration
                    registration.StudentId = saveResult.SavedId;
                }

                //get list of registrations for saved student
                List<Registration> dbRegistrations = student.StudentId > 0 ?
                    FindRegistrationsByStudent(false, true, student.StudentId, -1) : 
                    new List<Registration>();

                //check result
                if (dbRegistrations[0].Result == (int)SelectResult.Empty)
                {
                    //no registration
                    //clear list
                    dbRegistrations.Clear();
                }
                else if (dbRegistrations[0].Result == (int)SelectResult.FatalError)
                {
                    //error while getting registrations
                    //throw an exception
                    throw new ApplicationException(
                        "Unexpected error while getting student registrations. " +
                        dbRegistrations[0].ErrorMessage);
                }
                
                //check each assigned registration
                foreach (Registration registration in registrations)
                {
                    //check if registration has id
                    if (registration.RegistrationId > 0)
                    {
                        //find database registration
                        Registration dbRegistration = dbRegistrations.Find(
                            r => r.RegistrationId == registration.RegistrationId);

                        //check result
                        if (dbRegistration != null &&
                            dbRegistration.AutoRenewal == registration.AutoRenewal &&
                            dbRegistration.Position == registration.Position &&
                            dbRegistration.RegistrationStatus == registration.RegistrationStatus)
                        {
                            //registration data has not changed
                            //go to next registration
                            continue;
                        }
                    }

                    //save registration in database
                    registration.Save(transaction);
                }

                //must remove old registrations
                //check each previous registrations
                foreach (Registration dbRegistration in dbRegistrations)
                {
                    //check if registration is not in the new list
                    if (registrations.Find(r => r.RegistrationId == dbRegistration.RegistrationId) == null)
                    {
                        //safety procedure
                        //check if registration is for a past semester
                        if (dbRegistration.Semester == null ||
                            dbRegistration.Semester.EndDate <= DateTime.Today)
                        {
                            //cannot delete registration
                            continue;
                        }

                        //delete any attendance for registration class student
                        Attendance.DeleteForClassStudent(
                            transaction, dbRegistration.ClassId, dbRegistration.StudentId);

                        //delete any grade for registration class student
                        Grade.DeleteForClassStudent(
                            transaction, dbRegistration.ClassId, dbRegistration.StudentId);

                        //delete registration from database
                        Registration.Delete(transaction, dbRegistration.RegistrationId);
                    }
                }
                
                //check loans
                if (loans == null)
                {
                    //create empty list
                    loans = new List<Loan>();
                }

                //check each loan
                foreach (Loan loan in loans)
                {
                    //set student ID to loan
                    loan.StudentId = saveResult.SavedId;
                }

                //get list of loans for saved student
                List<Loan> dbLoans = student.StudentId > 0 ?
                    FindLoansByStudent(student.StudentId, -1) :
                    new List<Loan>();

                //check result
                if (dbLoans[0].Result == (int)SelectResult.Empty)
                {
                    //no loan
                    //clear list
                    dbLoans.Clear();
                }
                else if (dbLoans[0].Result == (int)SelectResult.FatalError)
                {
                    //error while getting loans
                    //throw an exception
                    throw new ApplicationException(
                        "Unexpected error while getting student loans. " +
                        dbLoans[0].ErrorMessage);
                }

                //check each assigned loan
                foreach (Loan loan in loans)
                {
                    //check if loan has id
                    if (loan.LoanId > 0)
                    {
                        //find database loan
                        Loan dbLoan = dbLoans.Find(r => r.LoanId == loan.LoanId);

                        //check result
                        if (dbLoan != null &&
                            dbLoan.Comments == loan.Comments &&
                            dbLoan.EndDate == loan.EndDate &&
                            dbLoan.InstrumentId == loan.InstrumentId &&
                            dbLoan.LoanStatus == loan.LoanStatus &&
                            dbLoan.StartDate == loan.StartDate &&
                            dbLoan.StudentId == loan.StudentId)
                        {
                            //loan data has not changed
                            //go to next loan
                            continue;
                        }
                    }

                    //save loan in database
                    loan.Save(transaction);
                }

                //must remove old loans
                //check each previous loans
                foreach (Loan dbLoan in dbLoans)
                {
                    //check if loan is not in the new list
                    if (loans.Find(r => r.LoanId == dbLoan.LoanId) == null)
                    {
                        //safety procedure
                        //check if loan was created more than edition threshold
                        if (dbLoan.CreationTime.Date < 
                            DateTime.Today.AddDays(-Loan.EDITION_THRESHOLD))
                        {
                            //cannot delete loan
                            continue;
                        }

                        //delete loan from database
                        Loan.Delete(transaction, dbLoan.LoanId);
                    }
                }



                //check if there is a new photo file
                if (photo != null)
                {
                    //save photo
                    //get next photo file name
                    //avoid existing photo file name
                    int index = 1;
                    string filePath = string.Empty;

                    do
                    {
                        //set file path
                        filePath = "Students\\Student_" + saveResult.SavedId + 
                            "_Photo_" + (index++) + "." + photo.FileExtension;
                    }
                    while (System.IO.File.Exists(
                        AppDomain.CurrentDomain.BaseDirectory + FILE_DIR_PATH + "\\" + filePath));

                    //set file path to photo file
                    photo.FilePath = filePath;
                    
                    //save photo file
                    SaveResult saveFileResult = SaveFile(photo);

                    //check result
                    if (saveFileResult.Result != (int)SelectResult.Success)
                    {
                        //unexpected error while saving file
                        //should hardly ever happen
                        //throw exception
                        throw new ApplicationException(
                            "Unexpected error while saving photo file to disk. " +
                            saveFileResult.ErrorMessage);
                    }

                    //set file path to student
                    //student will be saved later
                    student.PhotoFile = filePath;
                }

                //check if there is a new assignment file
                if (assignment != null)
                {
                    //save assignment
                    //get next assignment file name
                    //avoid existing assignment file name
                    int index = 1;
                    string filePath = string.Empty;

                    do
                    {
                        //set file path
                        filePath = "Students\\Student_" + saveResult.SavedId +
                            "_Assignment_" + (index++) + "." + assignment.FileExtension;
                    }
                    while (System.IO.File.Exists(
                        AppDomain.CurrentDomain.BaseDirectory + FILE_DIR_PATH + "\\" + filePath));

                    //set file path to assignment file
                    assignment.FilePath = filePath;

                    //save assignment file
                    SaveResult saveFileResult = SaveFile(assignment);

                    //check result
                    if (saveFileResult.Result != (int)SelectResult.Success)
                    {
                        //unexpected error while saving file
                        //should hardly ever happen
                        //throw exception
                        throw new ApplicationException(
                            "Unexpected error while saving assignment file to disk. " +
                            saveFileResult.ErrorMessage);
                    }

                    //set file path to student
                    //student will be saved later
                    student.AssignmentFile = filePath;
                }

                //check if any file was saved
                if (photo != null || assignment != null)
                {
                    //must save student again to save file path
                    //set ID to student
                    //it might be a new student
                    student.StudentId = saveResult.SavedId;

                    //save student
                    student.Save(transaction);
                }

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving student
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return saveResult;
        }

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
        public CountResult CountTeachersByFilter(
            int filterTeacherStatus, int filterInstitution, int filterPole)
        {
            //create result
            CountResult countResult = new CountResult();

            try
            {
                //count teachers in database and set result
                countResult.Count = Teacher.CountByFilter(
                    filterTeacherStatus, filterInstitution, filterPole);
            }
            catch (Exception ex)
            {
                //error while loading teachers
                //set result
                countResult.Count = int.MinValue;
                countResult.Result = (int)SelectResult.FatalError;
                countResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    countResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return countResult;
        }

        /// <summary>
        /// Find teacher by ID.
        /// </summary>
        /// <param name="teacherId">
        /// The ID of the selected teacher.
        /// </param>
        /// <returns>
        /// The selected teacher.
        /// </returns>
        public Teacher FindTeacher(int teacherId)
        {
            //the target teacher
            Teacher resultTeacher = null;

            try
            {
                //find teacher in database
                resultTeacher = Teacher.Find(teacherId);

                //check result
                if (resultTeacher != null)
                {
                    //teacher was found
                    resultTeacher.Result = (int)SelectResult.Success;
                }
                else
                {
                    //teacher was not found
                    //create result and set it
                    resultTeacher = new Teacher();
                    resultTeacher.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding teacher
                //create result and set it
                resultTeacher = new Teacher();
                resultTeacher.Result = (int)SelectResult.FatalError;
                resultTeacher.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultTeacher.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultTeacher;
        }

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
        public Teacher FindTeacherByUser(int userId, bool loadUser, bool loadPoles)
        {
            //the target teacher
            Teacher resultTeacher = null;

            try
            {
                //find teacher in database
                resultTeacher = Teacher.FindByUser(userId);

                //check result
                if (resultTeacher != null)
                {
                    //teacher was found
                    resultTeacher.Result = (int)SelectResult.Success;

                    //check if user should be loaded
                    if (loadUser)
                    {
                        //get teacher user
                        User user = User.Find(resultTeacher.UserId);

                        //check result
                        if (user != null)
                        {
                            //set user login
                            resultTeacher.UserLogin = user.Login;
                        }
                    }

                    //check if poles should be loaded
                    if (loadPoles)
                    {
                        //create list of pole names
                        resultTeacher.PoleNames = new List<string>();

                        //get list of teacher pole from database
                        List<Pole> dbPoles = Pole.FindByTeacher(resultTeacher.TeacherId);

                        //check result
                        if (dbPoles != null && dbPoles.Count > 0)
                        {
                            //check each pole
                            foreach (Pole dbPole in dbPoles)
                            {
                                //add pole name
                                resultTeacher.PoleNames.Add(dbPole.Name);
                            }

                            //sort list
                            resultTeacher.PoleNames.Sort();
                        }
                    }
                }
                else
                {
                    //teacher was not found
                    //create result and set it
                    resultTeacher = new Teacher();
                    resultTeacher.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding teacher
                //create result and set it
                resultTeacher = new Teacher();
                resultTeacher.Result = (int)SelectResult.FatalError;
                resultTeacher.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultTeacher.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultTeacher;
        }

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
        public List<Teacher> FindTeachersByFilter(bool loadUser, bool loadPoles,
            int filterTeacherStatus, int filterInstitution, int filterPole)
        {
            //create result list
            List<Teacher> teachers = new List<Teacher>();

            try
            {
                //get list of teachers from database using filters
                List<Teacher> dbTeachers = Teacher.FindByFilter(
                    filterTeacherStatus, filterInstitution, filterPole);

                //check result
                if (dbTeachers == null || dbTeachers.Count == 0)
                {
                    //no teacher was found
                    //create result and add it to the list
                    Teacher resultTeacher = new Teacher();
                    resultTeacher.Result = (int)SelectResult.Empty;
                    teachers.Add(resultTeacher);
                }
                else
                {
                    //check each teacher
                    foreach (Teacher dbTeacher in dbTeachers)
                    {
                        //set and add teacher
                        dbTeacher.Result = (int)SelectResult.Success;
                        teachers.Add(dbTeacher);
                    }

                    //check if user should be loaded
                    if (loadUser)
                    {
                        //get list of teacher users
                        List<User> users = User.FindTeacher();

                        //check result
                        if (users != null)
                        {
                            //check each teacher
                            foreach (Teacher teacher in teachers)
                            {
                                //find user
                                User user = users.Find(
                                    i => i.UserId == teacher.UserId);

                                //check result
                                if (user != null)
                                {
                                    //set user login
                                    teacher.UserLogin = user.Login;
                                }
                            }
                        }
                    }

                    //check if poles should be loaded
                    if (loadPoles)
                    {
                        //check each teacher
                        foreach (Teacher teacher in teachers)
                        {
                            //create list of pole names
                            teacher.PoleNames = new List<string>();

                            //get list of teacher pole from database
                            List<Pole> dbPoles = Pole.FindByTeacher(teacher.TeacherId);

                            //check result
                            if (dbPoles != null && dbPoles.Count > 0)
                            {
                                //check each pole
                                foreach (Pole dbPole in dbPoles)
                                {
                                    //add pole name
                                    teacher.PoleNames.Add(dbPole.Name);
                                }

                                //sort list
                                teacher.PoleNames.Sort();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading teachers
                //create result and add it to the list
                Teacher resultTeacher = new Teacher();
                resultTeacher.Result = (int)SelectResult.FatalError;
                resultTeacher.ErrorMessage = ex.Message;
                teachers.Add(resultTeacher);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultTeacher.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return teachers;
        }

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
        public DeleteResult InactivateTeacher(int teacherId, string inactivationReason)
        {
            //the inactivate result to be returned
            DeleteResult inactivateResult = new DeleteResult();

            try
            {
                //inactivate selected teacher
                if (Teacher.Inactivate(teacherId, inactivationReason))
                {
                    //teacher was inactivated
                    //set result
                    inactivateResult.Result = (int)SelectResult.Success;
                }
                else
                {
                    //teacher was not found
                    //set result
                    inactivateResult.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while inactivating teacher
                //set result
                inactivateResult.Result = (int)SelectResult.FatalError;
                inactivateResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    inactivateResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return inactivateResult;
        }

        /// <summary>
        /// Get list of teacher descriptions.
        /// </summary>
        /// <returns>
        /// The list of teacher descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListTeachers()
        {
            //create result list
            List<IdDescriptionStatus> teachers = new List<IdDescriptionStatus>();

            try
            {
                //get list of all teachers from database
                List<Teacher> dbTeachers = Teacher.Find();

                //check result
                if (dbTeachers == null || dbTeachers.Count == 0)
                {
                    //no teacher was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    teachers.Add(resultItem);
                }
                else
                {
                    //check each teacher
                    foreach (Teacher dbTeacher in dbTeachers)
                    {
                        //get description
                        IdDescriptionStatus item = dbTeacher.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        teachers.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading teachers
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                teachers.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return teachers;
        }

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
        public List<IdDescriptionStatus> ListTeachersByInstitution(int institutionId, int status)
        {
            //create result list
            List<IdDescriptionStatus> teachers = new List<IdDescriptionStatus>();

            try
            {
                //get list of teachers from database
                List<Teacher> dbTeachers = Teacher.FindByFilter(status, institutionId, -1);

                //check result
                if (dbTeachers == null || dbTeachers.Count == 0)
                {
                    //no teacher was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    teachers.Add(resultItem);
                }
                else
                {
                    //check each teacher
                    foreach (Teacher dbTeacher in dbTeachers)
                    {
                        //get description
                        IdDescriptionStatus item = dbTeacher.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        teachers.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading teachers
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                teachers.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return teachers;
        }

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
        public List<IdDescriptionStatus> ListTeachersByPole(int poleId, int status)
        {
            //create result list
            List<IdDescriptionStatus> teachers = new List<IdDescriptionStatus>();

            try
            {
                //get list of teachers from database
                List<Teacher> dbTeachers = Teacher.FindByFilter(status, -1, poleId);

                //check result
                if (dbTeachers == null || dbTeachers.Count == 0)
                {
                    //no teacher was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    teachers.Add(resultItem);
                }
                else
                {
                    //check each teacher
                    foreach (Teacher dbTeacher in dbTeachers)
                    {
                        //get description
                        IdDescriptionStatus item = dbTeacher.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        teachers.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading teachers
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                teachers.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return teachers;
        }

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
        public List<IdDescriptionStatus> ListTeachersByStatus(int status)
        {
            //create result list
            List<IdDescriptionStatus> teachers = new List<IdDescriptionStatus>();

            try
            {
                //get list of teachers from database
                List<Teacher> dbTeachers = Teacher.FindByFilter(status, -1, -1);

                //check result
                if (dbTeachers == null || dbTeachers.Count == 0)
                {
                    //no teacher was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    teachers.Add(resultItem);
                }
                else
                {
                    //check each teacher
                    foreach (Teacher dbTeacher in dbTeachers)
                    {
                        //get description
                        IdDescriptionStatus item = dbTeacher.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        teachers.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading teachers
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                teachers.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return teachers;
        }

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
        public SaveResult SaveTeacher(
            Teacher teacher, List<int> poleIds, List<Attendance> attendances)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            //use a transaction for all operations
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                //open connection and begin a transaction
                connection = new MySqlConnection(PnT.SongDB.ConnectionSettings.SongDBConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                //save teacher and get row ID
                saveResult.SavedId = teacher.Save(transaction);

                //get list of teacherpoles for saved teacher
                List<Teacherpole> dbTeacherPoles =
                    Teacherpole.FindByTeacher(transaction, saveResult.SavedId);

                //check result
                if (dbTeacherPoles == null)
                {
                    //no teacherpole in database
                    //set empty list
                    dbTeacherPoles = new List<Teacherpole>();
                }

                //check pole ids
                if (poleIds == null)
                {
                    //no permission set to role
                    //set empty list
                    poleIds = new List<int>();
                }

                //check each assigned pole
                foreach (int poleId in poleIds)
                {
                    //check if assigned pole is not in database yet
                    if (dbTeacherPoles.Find(rp => rp.PoleId == poleId) == null)
                    {
                        //must add new teacher pole relation
                        //create and save new teacher pole
                        Teacherpole teacherPole = new Teacherpole();
                        teacherPole.TeacherPoleId = -1;
                        teacherPole.TeacherId = saveResult.SavedId;
                        teacherPole.PoleId = poleId;
                        teacherPole.Save(transaction);
                    }
                }

                //must removed old teacher pole relations
                //check each previously relations
                foreach (Teacherpole teacherPole in dbTeacherPoles)
                {
                    //check if relation is not in the new list
                    if (!poleIds.Contains(teacherPole.PoleId))
                    {
                        //delete relation from database
                        Teacherpole.Delete(transaction, teacherPole.TeacherPoleId);
                    }
                }

                //check attendances
                if (attendances != null)
                {
                    //check each attendance
                    foreach (Attendance attendance in attendances)
                    {
                        //save attendance in database
                        attendance.Save(transaction);
                    }
                }

                //check transaction
                if (transaction != null)
                {
                    //commit transaction
                    transaction.Commit();
                }

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving teacher
                //check transaction
                if (transaction != null)
                {
                    //rollback transaction
                    transaction.Rollback();
                }

                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }
            finally
            {
                //check connection and its state
                if (connection != null &&
                    connection.State == System.Data.ConnectionState.Open)
                {
                    //close conenction
                    connection.Close();
                }
            }

            //return result
            return saveResult;
        }

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
        public User FindUser(int userId)
        {
            //the target user
            User resultUser = null;

            try
            {
                //find user in database
                resultUser = User.Find(userId);

                //check result
                if (resultUser != null)
                {
                    //user was found
                    resultUser.Result = (int)SelectResult.Success;

                    //check if user has an assigned institution
                    if (resultUser.InstitutionId > 0)
                    {
                        //get institution
                        Institution institution = Institution.Find(resultUser.InstitutionId);

                        //check result
                        if (institution != null)
                        {
                            //set institution name
                            resultUser.InstitutionName = institution.ProjectName;
                        }
                    }
                }
                else
                {
                    //user was not found
                    //create result and set it
                    resultUser = new User();
                    resultUser.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding user
                //create result and set it
                resultUser = new User();
                resultUser.Result = (int)SelectResult.FatalError;
                resultUser.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultUser.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultUser;
        }

        /// <summary>
        /// Find user by login.
        /// </summary>
        /// <param name="login">
        /// The login of the selected user.
        /// </param>
        /// <returns>
        /// The selected user.
        /// </returns>
        public User FindUserByLogin(string login)
        {
            //the target user
            User resultUser = null;

            try
            {
                //find user in database
                resultUser = User.Find(login);

                //check result
                if (resultUser != null)
                {
                    //user was found
                    resultUser.Result = (int)SelectResult.Success;
                }
                else
                {
                    //user was not found
                    //create result and set it
                    resultUser = new User();
                    resultUser.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding user
                //create result and set it
                resultUser = new User();
                resultUser.Result = (int)SelectResult.FatalError;
                resultUser.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultUser.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultUser;
        }

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
        public List<User> FindUsersByFilter(bool loadInstitution, bool loadRole,
            int filterUserStatus, int filterInstitution, int filterRole)
        {
            //create result list
            List<User> users = new List<User>();

            try
            {
                //get list of users from database using filters
                List<User> dbUsers = User.FindByFilter(
                    filterUserStatus, filterInstitution, filterRole);

                //check result
                if (dbUsers == null || dbUsers.Count == 0)
                {
                    //no user was found
                    //create result and add it to the list
                    User resultUser = new User();
                    resultUser.Result = (int)SelectResult.Empty;
                    users.Add(resultUser);
                }
                else
                {
                    //check each user
                    foreach (User dbUser in dbUsers)
                    {
                        //set and add user
                        dbUser.Result = (int)SelectResult.Success;
                        users.Add(dbUser);
                    }

                    //check if user should be loaded
                    if (loadInstitution)
                    {
                        //get list of all institutions
                        List<Institution> institutions = Institution.Find();

                        //check result
                        if (institutions != null)
                        {
                            //check each user
                            foreach (User user in users)
                            {
                                //check if user has no assigned institution
                                if (user.InstitutionId <= 0)
                                {
                                    //go to next user
                                    continue;
                                }

                                //find institution
                                Institution institution = institutions.Find(
                                    i => i.InstitutionId == user.InstitutionId);

                                //check result
                                if (institution != null)
                                {
                                    //set institution name
                                    user.InstitutionName = institution.ProjectName;
                                }
                            }
                        }
                    }

                    //check if user should be loaded
                    if (loadRole)
                    {
                        //get list of all roles
                        List<Role> roles = Role.Find();

                        //check result
                        if (roles != null)
                        {
                            //check each user
                            foreach (User user in users)
                            {
                                //find role
                                Role role = roles.Find(
                                    i => i.RoleId == user.RoleId);

                                //check result
                                if (role != null)
                                {
                                    //set role name
                                    user.RoleName = role.Name;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading users
                //create result and add it to the list
                User resultUser = new User();
                resultUser.Result = (int)SelectResult.FatalError;
                resultUser.ErrorMessage = ex.Message;
                users.Add(resultUser);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultUser.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return users;
        }

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
        public DeleteResult InactivateUser(int userId, string inactivationReason)
        {
            //the inactivate result to be returned
            DeleteResult inactivateResult = new DeleteResult();

            try
            {
                //inactivate selected user
                if (User.Inactivate(userId, inactivationReason))
                {
                    //user was inactivated
                    //set result
                    inactivateResult.Result = (int)SelectResult.Success;
                }
                else
                {
                    //user was not found
                    //set result
                    inactivateResult.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while inactivating user
                //set result
                inactivateResult.Result = (int)SelectResult.FatalError;
                inactivateResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    inactivateResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return inactivateResult;
        }

        /// <summary>
        /// Get list of user descriptions.
        /// </summary>
        /// <returns>
        /// The list of user descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListUsers()
        {
            //create result list
            List<IdDescriptionStatus> users = new List<IdDescriptionStatus>();

            try
            {
                //get list of all users from database
                List<User> dbUsers = User.Find();

                //check result
                if (dbUsers == null || dbUsers.Count == 0)
                {
                    //no user was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    users.Add(resultItem);
                }
                else
                {
                    //check each user
                    foreach (User dbUser in dbUsers)
                    {
                        //get description
                        IdDescriptionStatus item = dbUser.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        users.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading users
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                users.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return users;
        }

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
        public List<IdDescriptionStatus> ListUsersByInstitution(int institutionId, int status)
        {
            //create result list
            List<IdDescriptionStatus> users = new List<IdDescriptionStatus>();

            try
            {
                //get list of users from database
                List<User> dbUsers = User.FindByFilter(status, institutionId, -1);

                //check result
                if (dbUsers == null || dbUsers.Count == 0)
                {
                    //no user was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    users.Add(resultItem);
                }
                else
                {
                    //check each user
                    foreach (User dbUser in dbUsers)
                    {
                        //get description
                        IdDescriptionStatus item = dbUser.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        users.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading users
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                users.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return users;
        }

        /// <summary>
        /// Get list of user descriptions that are assigned to the selected role.
        /// </summary>
        /// <param name="roleId">
        /// The ID of the selected role.
        /// </param>
        /// <returns>
        /// The list of user descriptions.
        /// </returns>
        public List<IdDescriptionStatus> ListUsersByRole(int roleId)
        {
            //create result list
            List<IdDescriptionStatus> users = new List<IdDescriptionStatus>();

            try
            {
                //get list of users from database
                List<User> dbUsers = User.FindByFilter(-1, -1, roleId);

                //check result
                if (dbUsers == null || dbUsers.Count == 0)
                {
                    //no user was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    users.Add(resultItem);
                }
                else
                {
                    //check each user
                    foreach (User dbUser in dbUsers)
                    {
                        //get description
                        IdDescriptionStatus item = dbUser.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        users.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading users
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                users.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return users;
        }

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
        public List<IdDescriptionStatus> ListUsersByStatus(int status)
        {
            //create result list
            List<IdDescriptionStatus> users = new List<IdDescriptionStatus>();

            try
            {
                //get list of users from database
                List<User> dbUsers = User.FindByFilter(status, -1, -1);

                //check result
                if (dbUsers == null || dbUsers.Count == 0)
                {
                    //no user was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    users.Add(resultItem);
                }
                else
                {
                    //check each user
                    foreach (User dbUser in dbUsers)
                    {
                        //get description
                        IdDescriptionStatus item = dbUser.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        users.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading users
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                users.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return users;
        }

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
        public List<IdDescriptionStatus> ListCoordinatorsByStatus(int status)
        {
            //create result list
            List<IdDescriptionStatus> users = new List<IdDescriptionStatus>();

            try
            {
                //get list of all coordinator users from database
                List<User> dbUsers = User.FindCoordinator();

                //check result
                if (dbUsers == null || dbUsers.Count == 0)
                {
                    //no user was found
                    //create result and add it to the list
                    IdDescriptionStatus resultItem = new IdDescriptionStatus();
                    resultItem.Result = (int)SelectResult.Empty;
                    users.Add(resultItem);
                }
                else
                {
                    //check each user
                    foreach (User dbUser in dbUsers)
                    {
                        //get description
                        IdDescriptionStatus item = dbUser.GetDescription();
                        item.Result = (int)SelectResult.Success;
                        users.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //error while loading users
                //create result and add it to the list
                IdDescriptionStatus resultItem = new IdDescriptionStatus();
                resultItem.Result = (int)SelectResult.FatalError;
                resultItem.ErrorMessage = ex.Message;
                users.Add(resultItem);

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultItem.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result list
            return users;
        }

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
        public User LogonUser(string login, byte[] password)
        {
            //the target user
            User resultUser = null;

            try
            {
                //find user in database
                resultUser = User.Find(login, password);

                //check result
                if (resultUser == null)
                {
                    //user was not found
                    //find user in database by recovery password
                    resultUser = User.FindByRecoveryPassword(login, password);

                    //check result
                    if (resultUser != null)
                    {
                        //user must change password when after logon
                        resultUser.RequestPasswordChange = true;
                    }
                }

                //check result
                if (resultUser != null)
                {
                    //user was found
                    resultUser.Result = (int)SelectResult.Success;

                    //check if user has an assigned institution
                    if (resultUser.InstitutionId > 0)
                    {
                        //find institution
                        Institution institution = Institution.Find(resultUser.InstitutionId);

                        //check result
                        if (institution != null)
                        {
                            //set institution name
                            resultUser.InstitutionName = institution.ProjectName;
                        }
                        else
                        {
                            //should never happen
                            resultUser.InstitutionName = "Assigned Institution";
                        }
                    }
                }
                else
                {
                    //user was not found
                    //create result and set it
                    resultUser = new User();
                    resultUser.Result = (int)SelectResult.Empty;
                }
            }
            catch (Exception ex)
            {
                //error while finding user
                //create result and set it
                resultUser = new User();
                resultUser.Result = (int)SelectResult.FatalError;
                resultUser.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    resultUser.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return resultUser;
        }

        /// <summary>
        /// Change password for selected user.
        /// </summary>
        /// <param name="userId">
        /// The ID of the selected user.
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
        public SaveResult ChangeUserPassword(string login, byte[] oldPassword, byte[] newPassword)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            try
            {
                //find user in database
                User user = User.Find(login, oldPassword);

                //check result
                if (user == null)
                {
                    //find user in database by recovery password
                    user = User.FindByRecoveryPassword(login, oldPassword);
                }

                //check result
                if (user != null)
                {
                    //set new password to user and check result
                    if (User.SetPassword(user.UserId, newPassword))
                    {
                        //password was changed
                        //set result
                        saveResult.Result = (int)SelectResult.Success;
                    }
                    else
                    {
                        //user was not found
                        //should never happen
                        //set result
                        saveResult.Result = (int)SelectResult.FatalError;
                        saveResult.ErrorMessage = "User was not found while saving password.";
                    }
                }
                else
                {
                    //user was not found
                    //cannot change password
                    //set result
                    saveResult.Result = (int)SelectResult.FatalError;
                    saveResult.ErrorMessage = "Invalid user and password.";
                }
            }
            catch (Exception ex)
            {
                //error while saving password
                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return saveResult;
        }

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
        public SaveResult SaveUser(User user)
        {
            //the save result to be returned
            SaveResult saveResult = new SaveResult();

            try
            {
                //save user and get row ID
                saveResult.SavedId = user.Save();

                //set result
                saveResult.Result = (int)SelectResult.Success;
            }
            catch (Exception ex)
            {
                //error while saving user
                //set result
                saveResult.Result = (int)SelectResult.FatalError;
                saveResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    saveResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return saveResult;
        }

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
        public SendResult SendRecoveryPasswordToUser(int userId)
        {
            //the send result to be returned
            SendResult sendResult = new SendResult();

            try
            {
                //generate recovery password for selected user
                string recoveryPassword = System.Web.Security.Membership.GeneratePassword(8, 0);

                //set recovery password to user and check result
                if (User.SetRecoveryPassword(
                    userId, SongDB.Mapper.Criptography.Encrypt(recoveryPassword)))
                {
                    //recovery password was changed
                    //send email to user and get result
                    sendResult = SendRecoveryPasswordEmail(userId, recoveryPassword);
                }
                else
                {
                    //user was not found
                    //set result
                    sendResult.Result = (int)SelectResult.FatalError;
                    sendResult.ErrorMessage = "User was not found while saving generated recovery password.";
                }
            }
            catch (Exception ex)
            {
                //error while inactivating user
                //set result
                sendResult.Result = (int)SelectResult.FatalError;
                sendResult.ErrorMessage = ex.Message;

                //check inner exception
                if (ex.InnerException != null)
                {
                    //add inner exception message
                    sendResult.ErrorMessage += " " + ex.InnerException.Message;
                }
            }

            //return result
            return sendResult;
        }

        #endregion User


        #region Server ****************************************************************

        /// <summary>
        /// Get server related information of the Song Server.
        /// </summary>
        /// <returns>
        /// The server related information.
        /// </returns>
        public ServerInfo GetServerInfo()
        {
            //apply application settings
            ApplicationSettings.ApplySettings();

            //create server info
            ServerInfo serverInfo = new ServerInfo();

            //set server info
            //set database connection string
            serverInfo.DbConnectionString = ConnectionSettings.SongDBConnectionString;

            //get assembly version
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            //set version
            serverInfo.Version = version.Major + "." + version.Minor + "." + version.Build;

            //return server info
            return serverInfo;
        }

        /// <summary>
        /// Get heartbeat response from Song Server.
        /// </summary>
        /// <returns>
        /// The current time of the server.
        /// </returns>
        public DateTime GetHeartbeat()
        {
            //apply database settings
            ApplicationSettings.ApplySettings();

            //return current time
            return DateTime.Now;
        }

        #endregion Server

    } //end of class SongService

} //end of namespace PnT.SongServer
