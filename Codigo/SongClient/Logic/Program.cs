using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using PnT.SongClient.UI;


namespace PnT.SongClient.Logic
{

    /// <summary>
    /// This class is used as the entry point for the application.
    /// </summary>
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">
        /// An array of arguments received from command line.
        /// </param>
        [STAThread]
        static void Main(string[] args)
        {
            //set windows visual and rendering options
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //write version info into log
            Manager.Log.WriteInfo("Song Client " +
                Assembly.GetExecutingAssembly().GetName().Version.ToString());

            //initiating application
            Manager.Log.WriteInfo(Properties.Resources.msgAppStarting);

            //create a main form, set it to manager and run it
            MainForm mainForm = new MainForm();
            Manager.MainForm = mainForm;
            Application.Run(mainForm);
        }

    } //end of class Program

} //end of namespace PnT.SongClient.Logic