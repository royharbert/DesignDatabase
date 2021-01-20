using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    /// <summary>
    /// Identifies urgency and drives date due calculation
    /// </summary>
    public class PriorityModel
    {
        /// <summary>
        /// unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Shows priority P1 is urgent
        /// </summary>
        public string Priority { get; set; }
    }
}
