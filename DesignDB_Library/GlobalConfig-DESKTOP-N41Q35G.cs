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

        public static String ConnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static string AppKeyLookup(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

       
    }
}
