using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class SummaryModel
    {
        public int YTDassigned { get; set; }
        public decimal YTDvalue { get; set; }
        public int RequestsInPeriod { get; set; }
        public int RequestsCompleted { get; set; }
        public int Backlog { get; set; }
    }
}
