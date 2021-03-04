using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignDB_Library.Models;

namespace DesignDB_Library.Operations
{
    public static class Logger
    {
        public static void WriteToLog(LogModel logEntry)
        {
            GlobalConfig.Connection.LogEntry_Add(logEntry.User, logEntry.Action, logEntry.AffectedFields , logEntry.RequestID);
        }
    }
}
