using DesignDB_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignDB_Utilities
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
#if (ACTIVE)
            GlobalConfig.AttachmentPath = "\\" + "\\sccacve1\\databases\\DatabaseBE\\AttachmentsDesign";
#else
            GlobalConfig.AttachmentPath = "\\" + "\\USCA5PDBATDGS01\\Databases\\Sandbox\\AttachmentsDesign";
#endif
            DesignDB_Library.GlobalConfig.InitializeConnection(DesignDB_Library.DatabaseType.Sandbox);
            Application.Run(new frmUtilityMenu());
        }
    }
}
