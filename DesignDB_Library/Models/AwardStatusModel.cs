using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class AwardStatusModel
    {
        public int TotalCount { get; set; }
        public string MSO { get; set; }
        public int PendingCount { get; set; }
        public decimal PendingDollars { get; set; }
        public int HasRevisionCount { get; set; }
        public decimal HasRevisionDollars { get; set; }
        public int CanceledCount { get; set; }
        public decimal CanceledDollars { get; set; }
        public int InactiveCount { get; set; }
        public decimal InactiveDollars { get; set; }
        public int YesCount { get; set; }
        public decimal YesDollars { get; set; }
        public int NoCount { get; set; }
        public decimal NoDollars { get; set; }
    }
}
