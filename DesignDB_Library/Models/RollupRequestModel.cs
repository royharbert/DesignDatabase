using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class RollupRequestModel
    {
        public int ID { get; set; }
        public string MSO { get; set; }
        public string DesignRequestor  { get; set; }
        public  decimal BOM_Value { get; set; }
        public DateTime DateAssigned { get; set; }
        public DateTime DateAllInfoReceived { get; set; }
        public DateTime DateCompleted { get; set; }
        public string Pty { get; set; }
        public string AwardStatus { get; set; }
        public string Category { get; set; }
        public string Region { get; set; }
    }
}
