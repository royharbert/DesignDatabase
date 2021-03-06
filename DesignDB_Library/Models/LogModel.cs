using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class LogModel
    {
        public DateTime TimeStamp { get; set; }
        public string User { get; set; }
        public string Action { get; set; }
        public string AffectedFields { get; set; }
        public string RequestID { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
    }
}
