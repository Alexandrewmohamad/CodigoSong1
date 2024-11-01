using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetroFramework.Forms;


namespace PnT.SongClient.UI
{
    public partial class AboutMetroForm : MetroForm
    {
        public const string COPYRIGHT =
            "This computer program is protected by copyright laws and " +
            "international treaties. Unauthorized reproduction or distribution " +
            "of this program, or any portion of it, may result in severe civil " +
            "and criminal penalties, and will prosecuted to the maximum extent " +
            "possible under the law.";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AboutMetroForm()
        {
            InitializeComponent();
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyInformationalVersion
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyInformationalVersionAttribute assemblyInformationalVersionAttribute = (AssemblyInformationalVersionAttribute)attributes[0];
                    if (assemblyInformationalVersionAttribute.InformationalVersion != "")
                    {
                        return assemblyInformationalVersionAttribute.InformationalVersion;
                    }
                }
                return AssemblyVersion;
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0 ||
                    ((AssemblyDescriptionAttribute)attributes[0]).Description.Length == 0)
                {
                    return COPYRIGHT;
                }
                else
                {
                    return ((AssemblyDescriptionAttribute)attributes[0]).Description +
                        "\n " + COPYRIGHT;
                }
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        /// <summary>
        /// Form load event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutMetroForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format(
                Properties.Resources.lblAbout + " {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format(
                Properties.Resources.lblVersion + " {0}", AssemblyInformationalVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text =
                Properties.Resources.lblDeveloped + " " + AssemblyCompany;
            this.mtxtDescription.Text = AssemblyDescription;
        }

    } //end of class AboutMetroForm

} //end of namespace PnT.SongClient.UI
