//#define Live

using DesignDB_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace DesignDB_UI
{
    static class Program
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Properties.Settings.Default.DatabaseLive)
            {
                GlobalConfig.SetDatabaseMode(DatabaseType.Live);
            }
            else
            {
                GlobalConfig.SetDatabaseMode(DatabaseType.Sandbox);
            }
            Application.Run(new frmLogin());                      
            //Application.Run(new aTest());
            //Application.Run(new frmDemo());
        }
    }
}
