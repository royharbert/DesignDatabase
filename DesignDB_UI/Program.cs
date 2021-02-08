//#define Live

using DesignDB_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignDB_UI
{
    static class Program
    {
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
        }
    }
}
