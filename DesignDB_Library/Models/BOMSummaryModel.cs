using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class BOMSummaryModel
    {
        int _pctLineMatches = 0;
        int _BOMLines = 0;
        int _LineMatches = 0;
        public string PID { get; set; }
        public int BOMLines
        {
            get
            {
                return _BOMLines;
            }
            set
            {
                _BOMLines = value;
                _pctLineMatches = getPct(_LineMatches, _BOMLines);
            }
        }
        public int LineMatches
        {
            get
            {
                return _LineMatches;
            }
            set
            {
                _LineMatches = value;
                _pctLineMatches = getPct(_LineMatches, _BOMLines);
            }
        }
        public int pctLineMatches
        {
            get
            {
                return _pctLineMatches;
            }
        }
        public int CityMatches { get; set; }
        public int StateMatches { get; set; }
        public int ValidSO_DateMatches { get; set; }

        private static int getPct(int count, int total)
        {
            int intPct = 0;
            if (total > 0)
            {
                double pct = Math.Round((double)(count * 100 / total));
                intPct = Convert.ToInt16(pct);
            }
            return intPct;
        }         
    }
}
