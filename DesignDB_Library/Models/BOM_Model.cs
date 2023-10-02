using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    /// <summary>
    /// model to hold BOM info
    /// </summary>
    public class BOM_Model
    {
        public int ID { get; set; }
        public double Quantity { get; set; }
        public string ModelNumber { get; set; }
        public string Description { get; set; }
        public string Quote { get; set; }
        public string DisplayText { get; set; }

        public Dictionary<int, List<string>> pnMatchList { get; set; }
        public Dictionary<int, List<string>> cityStateMatches { get; set; }
        //public List<string> pnMatches { get; set; }
        //public List<string> cityStateMatches { get; set; }
        public BOM_Model()
	    {
            Dictionary<int, List<string>> pnMatches = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> cityStateMatches = new Dictionary<int, List<string>>();
            //       List<string> pnMatches = new List<string>();
            //       List<string> cityStateMatches = new List<string>();
        }
	
    }

}
