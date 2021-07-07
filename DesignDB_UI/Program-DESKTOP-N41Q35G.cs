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
#if (Live)
            DesignDB_Library.GlobalConfig.InitializeConnection(DesignDB_Library.DatabaseType.Live);
            GlobalConfig.AttachmentPath =  "\\" + "\\USCA5PDBATDGS01\\Databases\\AttachmentsDesign";
#else
            DesignDB_Library.GlobalConfig.InitializeConnection(DesignDB_Library.DatabaseType.Sandbox);
            GlobalConfig.AttachmentPath = "\\" + "\\USCA5PDBATDGS01\\Databases\\Sandbox\\AttachmentsDesign";
#endif 
            Application.Run(new frmLogin());                      
        }
    }
}
