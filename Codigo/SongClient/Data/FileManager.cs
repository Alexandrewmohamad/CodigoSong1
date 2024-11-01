using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PnT.SongClient.Logic;
using PnT.SongDB.Logic;


namespace PnT.SongClient.Data
{

    /// <summary>
    /// Manages the downloaded file storage.
    /// </summary>
    public class FileManager
    {

        #region Fields ****************************************************************

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FileManager()
        {
            //log manager must be created first because other resources might
            //check if log directory exists
            DirectoryInfo logDir = new DirectoryInfo(Manager.FILE_DIR_PATH);
            if (!logDir.Exists)
            {
                try
                {
                    //create directory
                    logDir.Create();
                }
                catch
                {
                    //do nothing
                }
            }
        }

        #endregion Constructors


        #region Properties ************************************************************

        #endregion Properties


        #region Public Data Methods ***************************************************

        /// <summary>
        /// Generate report card file for selected students.
        /// </summary>
        /// <param name="filePath">
        /// The output file path.
        /// </param>
        /// <param name="semester">
        /// The selected semester of the report cards.
        /// </param>
        /// <param name="pole">
        /// The selected pole of the report cards.
        /// </param>
        /// <param name="students">
        /// The selected students to whom report cards will be generated.
        /// </param>
        /// <param name="registrations">
        /// The list of semester registrations for all selected students.
        /// </param>
        /// <param name="attendances">
        /// The list of semester attendance for all selected students.
        /// </param>
        /// <param name="grades">
        /// The list of semester grades for all selected students.
        /// </param>
        /// <param name="errorMessage">
        /// The error message if file could not be generated.
        /// </param>
        /// <returns>
        /// True if file was generated.
        /// False otherwise.
        /// </returns>
        public bool GenerateReportCardFile(
            string filePath, IdDescriptionStatus semester,
            IdDescriptionStatus pole, List<IdDescriptionStatus> students,
            List<Registration> registrations, List<Attendance> attendances,
            List<Grade> grades, ref string errorMessage)
        {
            //check students
            if (students == null || students.Count == 0)
            {
                //no student selected
                //should never happen
                errorMessage = "No student selected.";

                //file was not generated
                return false;
            }

            //set semester description
            if (semester.Description.EndsWith(".I"))
            {
                //replace description
                semester.Description = "1º " + Properties.Resources.item_Semester +
                    " " + semester.Description.Replace(".I", "");
            }
            else if (semester.Description.EndsWith(".II"))
            {
                //replace description
                semester.Description = "2º " + Properties.Resources.item_Semester +
                    " " + semester.Description.Replace(".II", "");
            }

            //use writer to create roll call file
            StreamWriter writer = null;

            try
            {
                //check if file exists
                if (System.IO.File.Exists(filePath))
                {
                    //delete file
                    System.IO.File.Delete(filePath);
                }

                //create image file path based on file path
                string imageFilePath = filePath.Replace(
                    new FileInfo(filePath).Extension, ".png");

                //check if image file exists
                if (System.IO.File.Exists(imageFilePath))
                {
                    //delete image file
                    System.IO.File.Delete(imageFilePath);
                }

                //copy image file to output directory
                System.IO.File.Copy(
                    Manager.IMAGE_DIR_PATH + "\\LogoEMC.png", imageFilePath);

                //create image file info for later use
                FileInfo imageFile = new FileInfo(imageFilePath);

                //open file to write
                writer = new StreamWriter(new FileStream(
                    filePath, FileMode.OpenOrCreate, FileAccess.Write), Encoding.UTF8);

                //write html document
                writer.WriteLine("<!DOCTYPE html>");
                writer.WriteLine("<html>");

                //write header
                writer.WriteLine("<head>");

                //write css style
                writer.WriteLine("<style type=\"text/css\">");
                writer.WriteLine(".a01 { padding: 0 0 0 20px; overflow: hidden; }");
                writer.WriteLine(".a01 .a01-1,");
                writer.WriteLine(".a01 .a01-2 { position: relative; float: left; width: 50%; }");
                writer.WriteLine(".a01 .a01-1 { left: -20px; }");
                writer.WriteLine(".a01 .a01-2 {  }");
                writer.WriteLine();

                writer.WriteLine(".certificateWrapper { float: left; width: 830px; margin: 0; padding: 0; }");
                writer.WriteLine(".certificateWrapper:nth-child(even) { margin-left: 10px; }");
                writer.WriteLine(".certificate { padding: 10px; line-height: 1.4; overflow: hidden; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; box-sizing: border-box; }");
                writer.WriteLine(".certificateWrapper:nth-child(odd) .certificate { border-left: 0; }");
                writer.WriteLine(".certificateWrapper .certificate { border-bottom: 0; }");
                writer.WriteLine(".wrapcertificate { border: 5px solid #000; border-radius: 20px; }");
                writer.WriteLine(".wrapcertificate .head { padding: 15px; }");
                writer.WriteLine(".wrapcertificate .head .a01-1 { font-size: 45px; font-weight: bold; margin-top: -15px; }");
                writer.WriteLine(".wrapcertificate .head .a01-2 img { float: right; height: 60px; }");
                writer.WriteLine(".wrapcertificate .msg01 { font-size: 13px; padding: 0 10px 5px 10px; }");
                writer.WriteLine(".wrapcertificate .studentinfo { padding: 5px; text-transform: uppercase; background: #000; }");
                writer.WriteLine(".wrapcertificate .studentinfo .boxSI { font-size: 13px; margin: 0 0 5px; padding: 3px 5px; background: #fff; border-radius: 5px; }");
                writer.WriteLine(".wrapcertificate .studentinfo .boxSI:last-child { margin: 0; }");
                writer.WriteLine(".wrapcertificate .studentinfo .boxSI .si { display: inline; font-weight: bold; }");
                writer.WriteLine(".wrapcertificate .wraptable { /*padding: 5px;*/ line-height: 1.2; }");
                writer.WriteLine(".wrapcertificate .wraptable table div { margin: 0 auto; }");
                writer.WriteLine(".wrapcertificate .wraptable table .col1 { width: 140px; }");
                writer.WriteLine(".wrapcertificate .wraptable table .col2 { width: 80px; }");
                writer.WriteLine(".wrapcertificate .wraptable table .col3 { width: 80px; }");
                writer.WriteLine(".wrapcertificate .wraptable table .col4 { width: 80px; }");
                writer.WriteLine(".wrapcertificate .wraptable table .col5 { width: 110px; }");
                writer.WriteLine(".wrapcertificate .wraptable table .col6 { width: 80px; }");
                writer.WriteLine(".wrapcertificate .wraptable table .col7 { width: 90px; }");
                writer.WriteLine(".wrapcertificate .wraptable table td,");
                writer.WriteLine(".wrapcertificate .wraptable table th { padding: 2px; height: 25px; }");
                writer.WriteLine(".wrapcertificate .wraptable table td,");
                writer.WriteLine(".wrapcertificate .wraptable table th,");
                writer.WriteLine(".wrapcertificate .wraptable table { border: 5px solid #fff; border-collapse: collapse; }");
                writer.WriteLine(".wrapcertificate .wraptable table { width: 100%; empty-cells: show; }");
                writer.WriteLine(".wrapcertificate .wraptable table .thead td,");
                writer.WriteLine(".wrapcertificate .wraptable table .thead th { color: #fff; font-weight: bold; font-size: 13px; background: #000; }");
                writer.WriteLine(".wrapcertificate .wraptable table .tbody td,");
                writer.WriteLine(".wrapcertificate .wraptable table .tbody th { background: #ccc; }");
                writer.WriteLine(".wrapcertificate .wraptable table .tbody th { text-align: left; font-weight: normal; font-size: 13px; }");
                writer.WriteLine(".wrapcertificate .wraptable table .tbody td { text-align: center; font-size: 13px; }");
                writer.WriteLine(".wrapcertificate .wraptable table .tavg td,");
                writer.WriteLine(".wrapcertificate .wraptable table .tavg th { background: #ccc; }");
                writer.WriteLine(".wrapcertificate .wraptable table .tavg th { text-align: center; font-weight: bold; font-size: 13px; }");
                writer.WriteLine(".wrapcertificate .wraptable table .tavg td { text-align: center; font-weight: bold; font-size: 13px; }");
                writer.WriteLine(".wrapcertificate .msg02 { font-size: 18px; font-weight: bold; text-align: center; color: #fff; background: #000; }");
                writer.WriteLine(".wrapcertificate .wrapmsgs { padding: 10px; }");
                writer.WriteLine(".wrapcertificate .msg03 { font-size: 9px; text-align: justify; }");
                writer.WriteLine();

                //write css style colors                
                writer.WriteLine(".wrapcertificate { color: #92b84a; border-color: #92b84a; }");
                writer.WriteLine(".wrapcertificate .msg02,");
                writer.WriteLine(".wrapcertificate .wraptable table .thead td,");
                writer.WriteLine(".wrapcertificate .wraptable table .thead th,");
                writer.WriteLine(".wrapcertificate .studentinfo { background: #92b84a; }");
                writer.WriteLine(".wrapcertificate .wraptable table .tbody td,");
                writer.WriteLine(".wrapcertificate .wraptable table .tbody th { background: #e7e8ea; }");
                writer.WriteLine(".wrapcertificate .wraptable table .tavg td,");
                writer.WriteLine(".wrapcertificate .wraptable table .tavg th { background: #d2d3d5; }");
                writer.WriteLine(".wrapcertificate .wraptable table tr td.blank,");
                writer.WriteLine(".wrapcertificate .wraptable table tr th.blank { background: #fff !important; }");
                writer.WriteLine();

                //print each report card in one page
                writer.WriteLine(".pagebreak { page-break-before: always; }");
                writer.WriteLine();

                //print page in landscape
                writer.WriteLine("@page { size: landscape; }");

                //end css style
                writer.WriteLine("</style>");

                //write title
                writer.Write("<title>");
                writer.Write(students.Count > 1 ?
                    Properties.Resources.item_plural_ReportCard : 
                    Properties.Resources.item_ReportCard);
                writer.WriteLine("</title>");

                //end header
                writer.WriteLine("</head>");

                //write body
                writer.WriteLine("<body>");

                //check each student
                foreach (IdDescriptionStatus student in students)
                {
                    //get registrations for current student
                    List<Registration> studentRegistrations = registrations.FindAll(
                        r => r.StudentId == student.Id);

                    //order registrations by class code
                    studentRegistrations.Sort((r1, r2) => r1.Class.Code.CompareTo(r2.Class.Code));

                    //calculate month grades for each class
                    Dictionary<int, double> classDisciplines = new Dictionary<int, double>();
                    Dictionary<int, double> classPerformances = new Dictionary<int, double>();
                    Dictionary<int, double> classDedications = new Dictionary<int, double>();

                    //calculate audition grade for each class
                    Dictionary<int, double> classAuditions = new Dictionary<int, double>();

                    //calculate attendance percentage for each class
                    Dictionary<int, int> classAttendances = new Dictionary<int, int>();

                    //check each registration and its class
                    foreach (Registration registration in studentRegistrations)
                    {
                        //calculate and store mean discipline grade for class
                        CalculateMeanSubjectGradeAndStore(
                            grades, student.Id, registration.ClassId,
                            (int)GradeSubject.Discipline, classDisciplines);

                        //calculate and store mean performance grade for class
                        CalculateMeanSubjectGradeAndStore(
                            grades, student.Id, registration.ClassId,
                            (int)GradeSubject.Performance, classPerformances);

                        //calculate and store mean dedication grade for class
                        CalculateMeanSubjectGradeAndStore(
                            grades, student.Id, registration.ClassId,
                            (int)GradeSubject.Dedication, classDedications);

                        //calculate and store mean audition grade for class
                        CalculateMeanAuditionGradeAndStore(
                            grades, student.Id, registration.ClassId, classAuditions);

                        //calculate and store attendance percentage for class
                        CalculateAttendanceAndStore(
                            attendances, student.Id, 
                            registration.ClassId, classAttendances);
                    }

                    //calculate mean month grades of all classes
                    double meanDiscipline = CalculateMeanGrade(classDisciplines);
                    double meanPerformance = CalculateMeanGrade(classPerformances);
                    double meanDedication = CalculateMeanGrade(classDedications);

                    //calculate mean audition grade of all classes
                    double meanAudition = CalculateMeanGrade(classAuditions);

                    //calculate mean mean grade
                    double meanMeanGrade = double.MinValue;

                    //check mean month grades
                    if (meanDiscipline != double.MinValue &&
                        meanPerformance != double.MinValue &&
                        meanDedication != double.MinValue)
                    {
                        //calculate mean mean grade
                        meanMeanGrade = (meanDiscipline + meanPerformance + meanDedication) / 3.0;
                    }

                    //calculate final grade
                    double finalGrade = double.MinValue;

                    //check mean mean grade and mean audition grade
                    if (meanMeanGrade != double.MinValue &&
                        meanAudition != double.MinValue)
                    {
                        //calculate final grade
                        finalGrade = (meanMeanGrade + meanAudition) / 2.0;
                    }

                    //calculate mean attendance percentage
                    int meanAttendance = CalculateMeanAttendance(classAttendances);

                    //write report card for current student
                    writer.WriteLine("<div class=\"certificateWrapper\" style=\"font-family: 'Segoe UI'; background-color: #FFFFFF;\">");
                    writer.WriteLine("<div class=\"certificate\">");
                    writer.WriteLine("<div class=\"wrapcertificate\">");

                    //write report card header with image
                    writer.WriteLine("<div class=\"head\">");
                    writer.WriteLine(" <div class=\"a01\">");
                    writer.WriteLine("  <div class=\"a01-1\">Boletim</div>");
                    writer.WriteLine("  <div class=\"a01-2\"><img src=\"" + imageFile.Name + "\" alt=\"EMC\"/></div>");
                    writer.WriteLine(" </div>");
                    writer.WriteLine("</div>");

                    //write report card certificate text                
                    writer.WriteLine("<div class=\"msg01\">" +
                        "Certificamos que o(a) aluno(a) abaixo identificado(a) participou das atividades do " +
                        "projeto de educação musical no referido período.");
                    writer.WriteLine("</div>");

                    //write report card info                
                    writer.WriteLine("<div class=\"studentinfo\">");
                    writer.WriteLine(" <div class=\"boxSI\">Aluno: <span class=\"si\">" + student.Description + "</span></div>");
                    writer.WriteLine(" <div class=\"boxSI\">Polo: <span class=\"si\">" + pole.Description + "</span></div>");
                    writer.WriteLine(" <div class=\"boxSI\">Período: <span class=\"si\">" + semester.Description + "</span></div>");
                    writer.WriteLine("</div>");

                    //write report card grades and attendance by class
                    //write data table header
                    writer.WriteLine("<div class=\"wraptable\">");
                    writer.WriteLine(" <table cellpadding=\"0\" cellspacing=\"0\">");
                    writer.WriteLine("  <tbody>");
                    writer.WriteLine("   <tr class=\"thead\">");
                    writer.WriteLine("    <th rowspan=\"2\">");
                    writer.WriteLine("     <div class=\"col1\">Aula</div>");
                    writer.WriteLine("    </th>");
                    writer.WriteLine("    <th rowspan=\"2\">");
                    writer.WriteLine("     <div class=\"col5\">Frequência</div>");
                    writer.WriteLine("    </th>");
                    writer.WriteLine("    <th colspan=\"3\">");
                    writer.WriteLine("     <div class=\"col1\">Avaliações Mensais</div>");
                    writer.WriteLine("    </th>");
                    writer.WriteLine("    <th rowspan=\"2\">");
                    writer.WriteLine("     <div class=\"col6\">Avaliação<br />Semestral</div>");
                    writer.WriteLine("    </th>");
                    writer.WriteLine("    <th rowspan=\"2\">");
                    writer.WriteLine("     <div class=\"col7\">Média Final</div>");
                    writer.WriteLine("    </th>");
                    writer.WriteLine("   </tr>");
                    writer.WriteLine("   <tr class=\"thead\">");
                    writer.WriteLine("    <th>");
                    writer.WriteLine("     <div class=\"col2\">Disciplina</div>");
                    writer.WriteLine("    </th>");
                    writer.WriteLine("    <th>");
                    writer.WriteLine("     <div class=\"col3\">Desempenho</div>");
                    writer.WriteLine("    </th>");
                    writer.WriteLine("    <th>");
                    writer.WriteLine("     <div class=\"col4\">Dedicação</div>");
                    writer.WriteLine("    </th>");
                    writer.WriteLine("   </tr>");

                    //write data table lines
                    //check each registration
                    for (int i = 0; i < studentRegistrations.Count; i++)
                    {
                        //get current registration
                        Registration registration = studentRegistrations[i];
                        Class classObj = registration.Class;

                        //get class grades to be displayed
                        string discipline = GetClassGradeText(classDisciplines, classObj.ClassId);
                        string performance = GetClassGradeText(classPerformances, classObj.ClassId);
                        string dedication = GetClassGradeText(classDedications, classObj.ClassId);
                        string audition = GetClassGradeText(classAuditions, classObj.ClassId);

                        //get class attendance to be displayed
                        string attendance = GetClassAttendanceText(classAttendances, classObj.ClassId);

                        //generate code for registration class
                        string classCode = "&nbsp;" + classObj.SubjectCode.ToString("00000") + " - ";
                        classCode += classObj.ClassType == (int)ClassType.Instrument ?
                            Properties.Resources.ResourceManager.GetString(
                                "InstrumentsType_" + ((InstrumentsType)classObj.InstrumentType).ToString()) :
                            Properties.Resources.ResourceManager.GetString(
                                "ClassType_" + ((ClassType)classObj.ClassType).ToString());

                        //write class data
                        writer.WriteLine("   <tr class=\"tbody\">");
                        writer.WriteLine("    <th>" + classCode + "</th>");
                        writer.WriteLine("    <td>" + attendance + "</td>");
                        writer.WriteLine("    <td>" + discipline + "</td>");
                        writer.WriteLine("    <td>" + performance + "</td>");
                        writer.WriteLine("    <td>" + dedication + "</td>");
                        writer.WriteLine("    <td>" + audition + "</td>");

                        //check if final grade average should be written
                        if (i == 0)
                        {
                            //get final grade to be displayed
                            string final = GetGradeText(finalGrade);

                            //calculate rowspan based on the number of registrations
                            int rowspan = Math.Max(studentRegistrations.Count, 3) + 1;

                            //write final grade
                            writer.WriteLine("    <td rowspan=\"" + rowspan + "\" style=\"background-color: #D2D3D5; font-size: 18px; font-weight: bold;\">" + final + "</td>");
                        }

                        //close class data
                        writer.WriteLine("   </tr>");
                    }

                    //complete with empty class lines if necessary
                    for (int i = studentRegistrations.Count; i < 3; i++)
                    {
                        //write empty class line
                        writer.WriteLine("   <tr class=\"tbody\">");
                        writer.WriteLine("    <td> - </td>");
                        writer.WriteLine("    <td> - </td>");
                        writer.WriteLine("    <td> - </td>");
                        writer.WriteLine("    <td> - </td>");
                        writer.WriteLine("    <td> - </td>");
                        writer.WriteLine("    <td> - </td>");
                        writer.WriteLine("   </tr>");
                    }

                    //write mean data line
                    writer.WriteLine("<tr class=\"tavg\">");
                    writer.WriteLine("<th>");
                    writer.WriteLine("<div class=\"col1\">Média</div>");
                    writer.WriteLine("</th>");
                    writer.WriteLine("<td>");
                    writer.WriteLine("<div class=\"col5\">" + GetAttendanceText(meanAttendance) + "</div>");
                    writer.WriteLine("</td>");
                    writer.WriteLine("<td>");
                    writer.WriteLine("<div class=\"col2\">" + GetGradeText(meanDiscipline) + "</div>");
                    writer.WriteLine("</td>");
                    writer.WriteLine("<td>");
                    writer.WriteLine("<div class=\"col3\">" + GetGradeText(meanPerformance) + "</div>");
                    writer.WriteLine("</td>");
                    writer.WriteLine("<td>");
                    writer.WriteLine("<div class=\"col4\">" + GetGradeText(meanDedication) + "</div>");
                    writer.WriteLine("</td>");
                    writer.WriteLine("<td rowspan=\"2\">");
                    writer.WriteLine("<div class=\"col6\">" + GetGradeText(meanAudition) + "</div>");
                    writer.WriteLine("</td>");
                    writer.WriteLine("</tr>");

                    //close datatable
                    writer.WriteLine("</tbody>");
                    writer.WriteLine("</table>");
                    writer.WriteLine("</div>");

                    //write report orientations                    
                    writer.WriteLine("<div class=\"msg02\">Orientações para Leitura</div>");
                    writer.WriteLine("<div class=\"wrapmsgs\">");
                    writer.WriteLine(" <div class=\"msg03\">" + 
                        "O objetivo deste boletim é sinalizar para alunos e responsáveis " +
                        "o desenvolvimento do seu aprendizado no projeto.</div>");
                    writer.WriteLine(" <div class=\"msg03\">" + 
                        "Este boletim é preenchido e distribuído ao final do semestre " + 
                        "das aulas de música pelos professores de cada modalidade de ensino. " + 
                        "Os alunos estão matriculados em diferentes turmas e modalidades " + 
                        "de instrumento. Em alguns polos não há aulas de Teoria Musical " +
                        "e Oficina Criativa. Nestes casos não constam no boletim tais modalidades.</div>");
                    writer.WriteLine(" <div class=\"msg03\">" + 
                        "<b>Média Final</b> - É a nota final do aluno calculada a partir " +
                        "de todas as avaliações do professor no semestre.</div>");
                    writer.WriteLine(" <div class=\"msg03\">" + 
                        "<b>Avaliação Semestral</b> - É a nota do aluno na avaliação prática " +
                        "ou teórica (Oficina Criativa e Teoria Musical) ao final do semestre.</div>");
                    writer.WriteLine(" <div class=\"msg03\">" +
                        "<b>Avaliações Mensais</b> - É a nota média do aluno nas avaliações feitas " +
                        "mensalmente ao longo do semestre para cada um dos três critérios avaliados.</div>");
                    writer.WriteLine(" <div class=\"msg03\">" +
                        "<b>Frequência</b> - É a porcentagem de presença do aluno nas aulas " +
                        "ao longo do semestre. O valor mínimo esperado é de 75%.</div>");
                    writer.WriteLine("</div>");

                    //end report card
                    writer.WriteLine("</div>");
                    writer.WriteLine("</div>");
                    writer.WriteLine("</div>");

                    //check if current student is not the last student
                    if (student != students[students.Count - 1])
                    {
                        //add print page break
                        writer.WriteLine("<br/><br/>");
                        writer.WriteLine("<div class=\"pagebreak\"> </div>");
                    }
                }

                //end body
                writer.WriteLine("</body>");

                //end document
                writer.WriteLine("</html>");

                //flush document
                writer.Flush();
            }
            catch (Exception ex)
            {
                //unexpected error while generating report card file
                //log exception
                Manager.Log.WriteException(
                    "Unexpected error while generating report card file.", ex);

                //set error message
                errorMessage = ex.Message;

                //file was not generated
                return false;
            }
            finally
            {
                //check writer
                if (writer != null)
                {
                    //close writer
                    writer.Close();
                }
            }

            //file was generated
            return true;
        }

        /// <summary>
        /// Generate roll call file for selected classes.
        /// </summary>
        /// <param name="filePath">
        /// The output file path.
        /// </param>
        /// <param name="month">
        /// The selected month of the roll calls.
        /// </param>
        /// <param name="classes">
        /// The list of selected classes.
        /// </param>
        /// <param name="students">
        /// The list of student descriptions for each class.
        /// </param>
        /// <param name="attendances">
        /// The list of registered month attendances for each class.
        /// </param>
        /// <param name="errorMessage">
        /// The error message if file could not be generated.
        /// </param>
        /// <returns>
        /// True if file was generated.
        /// False otherwise.
        /// </returns>
        public bool GenerateRollCallFile(
            string filePath, DateTime month, List<Class> classes,
            Dictionary<int, List<IdDescriptionStatus>> students,
            Dictionary<int, List<Attendance>> attendances, ref string errorMessage)
        {
            //check classes
            if (classes == null || classes.Count == 0)
            {
                //no class selected
                //should never happen
                errorMessage = "No class selected.";

                //file was not generated
                return false;
            }

            //use writer to create roll call file
            StreamWriter writer = null;

            try
            {
                //check if file exists
                if (System.IO.File.Exists(filePath))
                {
                    //delete file
                    System.IO.File.Delete(filePath);
                }

                //open file to write
                writer = new StreamWriter(new FileStream(
                    filePath, FileMode.OpenOrCreate, FileAccess.Write), Encoding.UTF8);
                
                //write html document
                writer.WriteLine("<!DOCTYPE html>");
                writer.WriteLine("<html>");

                //write header
                writer.WriteLine("<head>");

                //write css style
                writer.WriteLine("<style type=\"text/css\">");
                writer.WriteLine(".tg  {border-collapse:collapse;border-spacing:0;border-color:#ccc;margin:0px auto;}");
                writer.WriteLine(".tg td{font-family:Arial, sans-serif;font-size:11px;padding:8px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:#ccc;color:#333;background-color:#fff;}");
                writer.WriteLine(".tg th{font-family:Arial, sans-serif;font-size:12px;font-weight:normal;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:#ccc;color:#333;background-color:#e9e9e9;}");
                writer.WriteLine(".tg .tg-jo0b{background-color:#e9e9e9;font-weight:bold;vertical-align:top}");
                writer.WriteLine(".tg .tg-baqh{text-align:center;vertical-align:top}");
                writer.WriteLine(".tg .tg-0vih{background-color:#e9e9e9;font-weight:bold;text-align:center;vertical-align:top}");
                writer.WriteLine(".tg .tg-9hbo{font-weight:bold;vertical-align:top;text-align:left}");
                writer.WriteLine(".tg .tg-yw4l{vertical-align:top}");
                writer.WriteLine(".tg .tg-b7b8{background-color:#f6f6f6;vertical-align:top}");
                writer.WriteLine(".tg .tg-dzk6{background-color:#f6f6f6;text-align:center;vertical-align:top}");
                writer.WriteLine(".pagebreak { page-break-before: always; }");
                writer.WriteLine("</style>");

                //write title
                writer.Write("<title>");
                writer.Write(classes.Count > 1 ? 
                    Properties.Resources.item_plural_RollCall : Properties.Resources.item_RollCall);
                writer.Write(" - ");
                writer.Write(Properties.Resources.item_Teacher);
                writer.Write(" ");
                writer.Write(classes[0].TeacherName);
                writer.WriteLine("</title>");
                writer.WriteLine("</head>");

                //write body
                writer.WriteLine("<body>");

                //write print button
                writer.WriteLine("<table align=\"center\" style=\"border-style: none; width: 90%; border-width:0px; \">");
                writer.WriteLine("  <tr>");
                writer.WriteLine("    <th style=\"border-style: none; border-width: 0px;\">");
                writer.WriteLine("      <div style=\"border-width: 0px; width: 100%; height: 40px;\" align=\"right\">");
                writer.WriteLine("        <button onclick=\"window.print();\">" + Properties.Resources.wordPrint + "</button>");
                writer.WriteLine("      </div>");
                writer.WriteLine("    </th>");
                writer.WriteLine("  </tr>");
                writer.WriteLine("</table>");

                //write roll call for each class
                for (int i = 0; i < classes.Count; i++)
                {
                    //get current class
                    Class classObj = classes[i];

                    //get class registrations
                    List<IdDescriptionStatus> classStudents = students[classObj.ClassId];

                    //check class students
                    if (classStudents == null)
                    {
                        //create empty list
                        classStudents = new List<IdDescriptionStatus>();
                    }

                    //get class attendances
                    List<Attendance> classAttendances = attendances[classObj.ClassId];

                    //calculate class days
                    List <DateTime> classDays = classObj.GetClassDays(month);
                    
                    //gather week days
                    StringBuilder sbDays = new StringBuilder(8);
                    if (classObj.WeekMonday)
                    {
                        sbDays.Append(Properties.Resources.dayShortMondays);
                        sbDays.Append(", ");
                    }
                    if (classObj.WeekTuesday)
                    {
                        sbDays.Append(Properties.Resources.dayShortTuesdays);
                        sbDays.Append(", ");
                    }
                    if (classObj.WeekWednesday)
                    {
                        sbDays.Append(Properties.Resources.dayShortWednesdays);
                        sbDays.Append(", ");
                    }
                    if (classObj.WeekThursday)
                    {
                        sbDays.Append(Properties.Resources.dayShortThursdays);
                        sbDays.Append(", ");
                    }
                    if (classObj.WeekFriday)
                    {
                        sbDays.Append(Properties.Resources.dayShortFridays);
                        sbDays.Append(", ");
                    }
                    if (classObj.WeekSaturday)
                    {
                        sbDays.Append(Properties.Resources.dayShortSaturdays);
                        sbDays.Append(", ");
                    }
                    if (classObj.WeekSunday)
                    {
                        sbDays.Append(Properties.Resources.dayShortSundays);
                        sbDays.Append(", ");
                    }

                    //check result
                    if (sbDays.Length > 2)
                    {
                        //remove last ", "
                        sbDays.Length -= 2;
                    }

                    //write roll call table
                    writer.WriteLine("<table class=\"tg\" align=\"center\" style=\"width: 90%\">");

                    //write roll call title
                    writer.WriteLine("  <tr>");
                    writer.Write("    <th class=\"tg-9hbo\" colspan=\"");
                    writer.Write((classDays.Count + 1));
                    writer.Write("\">");
                    writer.Write(Properties.Resources.item_Class);
                    writer.Write(" ");
                    writer.Write(classObj.Code);
                    writer.Write(" | ");
                    writer.WriteLine(classObj.ClassType == (int)ClassType.Instrument ?
                        Properties.Resources.ResourceManager.GetString(
                            "InstrumentsType_" + ((InstrumentsType)classObj.InstrumentType).ToString()) :
                        Properties.Resources.ResourceManager.GetString(
                            "ClassType_" + ((ClassType)classObj.ClassType).ToString()));
                    writer.Write(" | ");
                    writer.WriteLine(classObj.TeacherName);
                    writer.Write(" | ");
                    writer.Write(Properties.Resources.ResourceManager.GetString(
                        "ClassLevel_" + ((ClassLevel)classObj.ClassLevel).ToString()));
                    writer.Write(" | ");
                    writer.Write(sbDays.ToString());
                    writer.Write(" ");
                    writer.Write(classObj.StartTime.ToString("HH:mm"));
                    writer.Write(" | ");
                    writer.Write(classObj.PoleName);
                    writer.WriteLine("</th>");
                    writer.WriteLine("  </tr>");

                    //write roll call header with class dates
                    writer.WriteLine("  <tr>");
                    writer.Write("    <td class=\"tg-jo0b\">");
                    writer.Write(Properties.Resources.item_Student);
                    writer.WriteLine("</td>");

                    //check each class day
                    foreach (DateTime classDay in classDays)
                    {
                        //get date text and remove year
                        string dateText = classDay.ToShortDateString();
                        dateText = dateText.Substring(0, dateText.LastIndexOf('/'));

                        //write line
                        writer.Write("    <td class=\"tg-0vih\">");
                        writer.Write(dateText);
                        writer.WriteLine("</td>");
                    }

                    //close roll call header
                    writer.WriteLine("  </tr>");

                    //write one line for each student registration
                    foreach (IdDescriptionStatus student in classStudents)
                    {
                        //get attendances for current student
                        List<Attendance> studentAttendances = classAttendances != null ?
                            classAttendances.FindAll(a => a.StudentId == student.Id) : null;

                        //write line
                        writer.WriteLine("  <tr>");
                        writer.Write("    <td class=\"tg-yw4l\">");
                        writer.Write(student.Description);
                        writer.WriteLine("</td>");

                        //add cell for each class day
                        foreach (DateTime classDay in classDays)
                        {
                            //write cell
                            writer.Write("    <td class=\"tg-baqh\">");

                            //find day attendance
                            Attendance dayAttendance = (studentAttendances != null) ?
                                studentAttendances.Find(a => a.Date.Equals(classDay)) : null;

                            //check result
                            if (dayAttendance != null)
                            {
                                //write attendance short value
                                writer.Write(Properties.Resources.ResourceManager.GetString(
                                    "RollCall_Short_" + ((RollCall)dayAttendance.RollCall).ToString()));
                            }

                            //close cell
                            writer.WriteLine("</td>");
                        }

                        //close line
                        writer.WriteLine("  </tr>");
                    }

                    //close table
                    writer.WriteLine("</table>");
                    writer.WriteLine("<br/><br/>");

                    //check if this is not the last class
                    if (i < classes.Count - 1)
                    {
                        //add print page break
                        writer.WriteLine("<div class=\"pagebreak\"> </div>");
                    }
                }

                //end document
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");

                //flush document
                writer.Flush();
            }
            catch (Exception ex)
            {
                //unexpected error while generating roll call file
                //log exception
                Manager.Log.WriteException(
                    "Unexpected error while generating roll call file.", ex);

                //set error message
                errorMessage = ex.Message;

                //file was not generated
                return false;
            }
            finally
            {
                //check writer
                if (writer != null)
                {
                    //close writer
                    writer.Close();
                }
            }

            //file was generated
            return true;
        }

        /// <summary>
        /// Get selected file.
        /// </summary>
        /// <param name="file">
        /// The selected file.
        /// </param>
        /// <returns>
        /// The file.
        /// Null if file is not available.
        /// </returns>
        public SongDB.Logic.File GetFile(string file)
        {
            try
            {
                //check if file exists
                if (!HasFile(file))
                {
                    //file does not exist
                    return null;
                }

                //the selected file
                SongDB.Logic.File selectedFile = new SongDB.Logic.File();

                //set file path
                selectedFile.FilePath = file;

                //read file data
                selectedFile.Data = System.IO.File.ReadAllBytes(
                    Manager.FILE_DIR_PATH + "\\" + file);

                //set result
                selectedFile.Result = (int)SelectResult.Success;

                //return selected file
                return selectedFile;
            }
            catch (Exception ex)
            {
                //error while loading file info
                //should never happen
                //log error
                Manager.Log.WriteException(
                    "Unexpected error while loading file from " +
                    file + ".", ex);

                //could not load file info
                return null;
            }
        }

        /// <summary>
        /// Get file info of the selected file.
        /// </summary>
        /// <param name="file">
        /// The selected file.
        /// </param>
        /// <returns>
        /// The file info.
        /// Null if file is not available.
        /// </returns>
        public FileInfo GetFileInfo(string file)
        {
            try
            {
                //check if file exists
                if (!HasFile(file))
                {
                    //file does not exist
                    return null;
                }

                //create and return file info
                return new FileInfo(Manager.FILE_DIR_PATH + "\\" + file);
            }
            catch (Exception ex)
            {
                //error while loading file info
                //should never happen
                //log error
                Manager.Log.WriteException(
                    "Unexpected error while loading file info from " +
                    file + ".", ex);

                //could not load file info
                return null;
            }
        }

        /// <summary>
        /// Get image file.
        /// </summary>
        /// <param name="file">
        /// The selected image file.
        /// </param>
        /// <returns>
        /// The image.
        /// Null if file is not available.
        /// </returns>
        public Bitmap GetImage(string file)
        {
            try
            {
                //check if file exists
                if (!HasFile(file))
                {
                    //file does not exist
                    return null;
                }

                //open thumbnail file
                using (Stream stream = System.IO.File.Open(
                    Manager.FILE_DIR_PATH + "\\" + file, System.IO.FileMode.Open))
                {
                    //read image and return a bitmap
                    return new Bitmap(Image.FromStream(stream));
                }
            }
            catch (Exception ex)
            {
                //error while loading image
                //should never happen
                //log error
                Manager.Log.WriteException(
                    "Unexpected error while loading image from " +
                    file + ".", ex);

                //could not load image
                return null;
            }
        }

        /// <summary>
        /// Get thumbnail image for the selected file.
        /// </summary>
        /// <param name="file">
        /// The selected file.
        /// </param>
        /// <returns>
        /// The thumbnail.
        /// Null if thumbnail is not available.
        /// </returns>
        public Bitmap GetThumbnail(string file)
        {
            //check file path
            if (file == null || file.Length == 0)
            {
                //invalid path
                //cannot find thumbnail
                return null;
            }

            //get thumb nail path
            string thumbnailPath = GetThumbnailFilePath(file);

            //check result
            if (thumbnailPath == null)
            {
                //invalid path
                //cannot find thumbnail
                return null;
            }

            try
            {
                //check if file exists
                if (!System.IO.File.Exists(thumbnailPath))
                {
                    //file does not exist
                    return null;
                }

                //open thumbnail file
                using (Stream stream = System.IO.File.Open(thumbnailPath, System.IO.FileMode.Open))
                {
                    //read image and return a bitmap
                    return new Bitmap(Image.FromStream(stream));
                }
            }
            catch (Exception ex)
            {
                //error while loading thumbnail
                //should never happen
                //log error
                Manager.Log.WriteException(
                    "Unexpected error while loading thumbnail image from " + 
                    thumbnailPath + ".", ex);

                //could not load thumbnail
                return null;
            }
        }

        /// <summary>
        /// Check if selected file exists.
        /// </summary>
        /// <param name="file">
        /// The selected file.
        /// </param>
        /// <returns>
        /// True if file exists.
        /// False otherwise.
        /// </returns>
        public bool HasFile(string file)
        {
            //check file path
            if (file == null || file.Length == 0)
            {
                //invalid path
                //cannot find file
                return false;
            }

            try
            {
                //return if file exists
                return System.IO.File.Exists(Manager.FILE_DIR_PATH + "\\" + file);
            }
            catch (Exception ex)
            {
                //error while loading image
                //should never happen
                //log error
                Manager.Log.WriteException(
                    "Unexpected error while checking file " +
                    file + ".", ex);

                //could not check file
                return false;
            }
        }

        /// <summary>
        /// Save file to disk.
        /// </summary>
        /// <param name="file">
        /// The file to be saved.
        /// </param>
        public bool SaveFile(SongDB.Logic.File file)
        {
            try
            {
                //create directories if needed
                new FileInfo(Manager.FILE_DIR_PATH + "\\" + file.FilePath).Directory.Create();

                //write file to local disk
                System.IO.File.WriteAllBytes(
                    Manager.FILE_DIR_PATH + "\\" + file.FilePath, file.Data);

                //file was written
                return true;
            }
            catch (Exception ex)
            {
                //log exception
                Manager.Log.WriteException(string.Format(
                    Properties.Resources.errorWriteSongFile, file.FilePath), ex);

                //could not write file
                return false;
            }
        }

        #endregion Public Data Methods


        #region Private Methods *******************************************************

        /// <summary>
        /// Calculate class attendance percentage and store result.
        /// </summary>
        /// <param name="attendances">
        /// The input list of all attendances.
        /// </param>
        /// <param name="studentId">
        /// The ID of the student to filter attendances.
        /// </param>
        /// <param name="classId">
        /// The ID of the class to filter attendances.
        /// </param>
        /// <param name="classAuditionAttendances">
        /// The list where to store the calculated attendance percentage.
        /// </param>
        private void CalculateAttendanceAndStore(
            List<Attendance> attendances, int studentId, int classId,
            Dictionary<int, int> classAuditionAttendances)
        {
            //calculate attendance percentage
            double percentage = double.MinValue;

            //count presences for class and student
            int presences = attendances.Count(
                a => a.StudentId == studentId &&
                    a.ClassId == classId &&
                    a.RollCall == (int)RollCall.Present);

            //count absences for class and student
            int absences = attendances.Count(
                a => a.StudentId == studentId &&
                    a.ClassId == classId &&
                    a.RollCall == (int)RollCall.Absent);

            //check result
            if (presences + absences > 0)
            {
                //calculate percentage
                percentage = (double)(presences) / (double)(presences + absences);
                percentage *= 100.0;

                //add attendance percentage
                classAuditionAttendances[classId] = (int)percentage;
            }
        }

        /// <summary>
        /// Calculate mean attendance for all class attendances in the selected list.
        /// </summary>
        /// <param name="classAttendaces">
        /// The selected class attendances.
        /// </param>
        /// <returns>
        /// The mean attendance.
        /// int.MinValue if selected list is empty.
        /// </returns>
        private int CalculateMeanAttendance(Dictionary<int, int> classAttendaces)
        {
            //check input attendances
            if (classAttendaces.Count == 0)
            {
                //no attendance
                return int.MinValue;
            }

            //calculate and return mean attendance
            return (int)(classAttendaces.Values.Average());
        }

        /// <summary>
        /// Calculate mean grade for all class grades in the selected list.
        /// </summary>
        /// <param name="classSubjectGrades">
        /// The selected class grades.
        /// </param>
        /// <returns>
        /// The mean grade.
        /// int.MinValue if selected list is empty.
        /// </returns>
        private double CalculateMeanGrade(Dictionary<int, double> classSubjectGrades)
        {
            //check input grades
            if (classSubjectGrades.Count == 0)
            {
                //no grade
                return double.MinValue;
            }

            //calculate and return mean grade
            return classSubjectGrades.Values.Average();
        }

        /// <summary>
        /// Calculate class mean grade for selected subject and store result.
        /// </summary>
        /// <param name="grades">
        /// The input list of all grades.
        /// </param>
        /// <param name="studentId">
        /// The ID of the student to filter grades.
        /// </param>
        /// <param name="classId">
        /// The ID of the class to filter grades.
        /// </param>
        /// <param name="gradeSubject">
        /// The selected grade subject.
        /// </param>
        /// <param name="classSubjectGrades">
        /// The list where to store the calculated mean grade.
        /// </param>
        private void CalculateMeanSubjectGradeAndStore(
            List<Grade> grades, int studentId, int classId, 
            int gradeSubject, Dictionary<int, double> classSubjectGrades)
        {
            //find subject grades for class and student
            List<Grade> subjectGrades = grades.FindAll(
                g => g.StudentId == studentId &&
                    g.ClassId == classId &&
                    g.GradeSubject == gradeSubject &&
                    g.Score >= 0);

            //check result
            if (subjectGrades.Count > 0)
            {
                //calculate and store subject average grade
                classSubjectGrades[classId] = subjectGrades.Average(
                    g => (double)(g.Score));
            }
        }

        /// <summary>
        /// Calculate class mean audition grade and store result.
        /// </summary>
        /// <param name="grades">
        /// The input list of all grades.
        /// </param>
        /// <param name="studentId">
        /// The ID of the student to filter grades.
        /// </param>
        /// <param name="classId">
        /// The ID of the class to filter grades.
        /// </param>
        /// <param name="classAuditionGrades">
        /// The list where to store the calculated mean grade.
        /// </param>
        private void CalculateMeanAuditionGradeAndStore(
            List<Grade> grades, int studentId, int classId,
            Dictionary<int, double> classAuditionGrades)
        {
            //find discipline grades for registration class
            List<Grade> auditionGrades = grades.FindAll(
                g => g.StudentId == studentId &&
                    g.ClassId == classId &&
                    g.GradePeriodicity == (int)GradePeriodicity.Semester &&
                    g.GradeSubject != (int)GradeSubject.ObjectiveReached &&
                    g.Score >= 0);

            //check result
            if (auditionGrades.Count > 0)
            {
                //calculate and store audition average grade
                classAuditionGrades[classId] = auditionGrades.Average(
                    g => (double)(g.Score));
            }
        }

        /// <summary>
        /// Get text that represents selected attendance.
        /// </summary>
        /// <param name="attendance">
        /// The selected attendance.
        /// </param>
        /// <returns>
        /// A representative text for the attendance.
        /// </returns>
        private string GetAttendanceText(int attendance)
        {
            //check attendance
            if (attendance == int.MinValue)
            {
                //empty attendance
                return "-";
            }

            //evaluate attendance percentage
            string evaluation = string.Empty;

            //check value
            if (attendance >= 90)
            {
                //high attendance percentage
                evaluation = "Alta";
            }
            else if (attendance >= 75)
            {
                //good attendance percentage
                evaluation = "Adequada";
            }
            else
            {
                //low attendance percentage
                evaluation = "Baixa";
            }

            //return attendance with evaluation
            return attendance.ToString() + "% - " + evaluation;
        }

        /// <summary>
        /// Find attendance for selected class and get text that represents found attendance.
        /// </summary>
        /// <param name="classAttendances">
        /// The list of class attendances.
        /// </param>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <returns>
        /// A representative text for the attendance.
        /// </returns>
        private string GetClassAttendanceText(
            Dictionary<int, int> classAttendances, int classId)
        {
            //attendance for selected class
            int attendance = int.MinValue;

            //try to get attendance from list
            if (classAttendances.TryGetValue(classId, out attendance))
            {
                //return attendance text
                return GetAttendanceText(attendance);
            }
            else
            {
                //no attendance
                return "-";
            }
        }

        /// <summary>
        /// Get text that represents selected grade.
        /// </summary>
        /// <param name="grade">
        /// The selected grade.
        /// </param>
        /// <returns>
        /// A representative text for the grade.
        /// </returns>
        private string GetGradeText(double grade)
        {
            //check grade
            if (grade == double.MinValue)
            {
                //empty grade
                return "-";
            }

            //check if grade is a ten
            if (grade == 10.0)
            {
                //no need for decimal
                return "10";
            }

            //return grade with one decimal
            return grade.ToString("0.0");
        }

        /// <summary>
        /// Find grade for selected class and get text that represents found grade.
        /// </summary>
        /// <param name="classGrades">
        /// The list of class grades.
        /// </param>
        /// <param name="classId">
        /// The ID of the selected class.
        /// </param>
        /// <returns>
        /// A representative text for the grade.
        /// </returns>
        private string GetClassGradeText(
            Dictionary<int, double> classGrades, int classId)
        {
            //grade for selected class
            double grade = double.MinValue;

            //try to get grade from list
            if (classGrades.TryGetValue(classId, out grade))
            {
                //return grade text
                return GetGradeText(grade);
            }
            else
            {
                //no grade
                return "-";
            }
        }

        /// <summary>
        /// Get thumbnail file path for the selected file.
        /// </summary>
        /// <param name="filePath">
        /// The selected file path.
        /// </param>
        /// <returns></returns>
        private string GetThumbnailFilePath(string filePath)
        {
            //check file path
            if (filePath == null || filePath.Length == 0 || filePath.IndexOf('.') < 0)
            {
                //invalid path
                //cannot find thumbnail
                return null;
            }

            //create and return thumbnail file path
            return Manager.FILE_DIR_PATH + "\\" + filePath.Substring(
                0, filePath.LastIndexOf('.')) + ".thumbnail.jpg";
        }

        #endregion Private Methods

    } //end of class FileManager

} //end of namespace PnT.SongClient.Data
