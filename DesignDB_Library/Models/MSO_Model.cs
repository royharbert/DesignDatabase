using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    /// <summary>
    /// Represents MSO 
    /// </summary>
    public class MSO_Model
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Full MSO name
        /// </summary>
        public string MSO { get; set; }

        /// <summary>
        /// 3 letter abbreviation
        /// </summary>
        public string Abbreviation { get; set; }

    } 
}
