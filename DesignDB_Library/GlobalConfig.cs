using DesignDB_Library.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DesignDB_Library;

namespace DesignDB_Library
{
    public static class GlobalConfig
    {        
        public static IDataConnection Connection { get; private set; }

        public static string AttachmentPath = "";
        public static string ArchiveAttachmentPath = "";
        public static string ArchiveSandboxAttachmentPath = "";
        public static DatabaseType DatabaseMode = DatabaseType.Sandbox;
        public static void InitializeConnection(DatabaseType db)
        { 
            if (db == DatabaseType.Live)
            {
                SqlConnector.db = "Live";
                SqlConnector LiveConnection = new SqlConnector();
                Connection = LiveConnection;
            }
            else if (db == DatabaseType.Sandbox)
            {
                SqlConnector.db = "Sandbox";
                SqlConnector SandboxConnection = new SqlConnector();
                Connection = SandboxConnection;
            }
        }

        public static void SetDatabaseMode(DatabaseType mode)
        {
            if (mode == DatabaseType.Live)
            {
                GlobalConfig.InitializeConnection(DesignDB_Library.DatabaseType.Live);
                GlobalConfig.AttachmentPath = @"\\rdcpstntapfil01\ANS_Design\AttachmentsDesign";
                GlobalConfig.ArchiveAttachmentPath = @"\\arrisi.com\arrisdfs\us-west-shares\Customer_Quotations";
                DatabaseMode = DatabaseType.Live;
            }
            else
            { 
                GlobalConfig.InitializeConnection(DatabaseType.Sandbox);
                GlobalConfig.AttachmentPath = @"\\rdcpstntapfil01\ANS_Design\Sandbox\AttachmentsDesign";
                GlobalConfig.ArchiveSandboxAttachmentPath = @"\\arrisi.com\arrisdfs\us-west-shares\Customer_Quotations\Sandbox";
                DatabaseMode = DatabaseType.Sandbox;
            }
        }
        /// <summary>
        /// Retrieves connection string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String ConnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

       
    }
}
