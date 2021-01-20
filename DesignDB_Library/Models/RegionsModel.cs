using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    /// <summary>
    /// Identifies global region of request
    /// </summary>
    public class RegionsModel
    {
        /// <summary>
        /// unique identifier
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Name of region
        /// </summary>
        public string Region { get; set; }
    }
}
