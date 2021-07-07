
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    /// <summary>
    /// List of observed holidays. Impacts date due function
    /// </summary>
    public class CompanyHolidaysModel
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Holiday name
        /// </summary>
        public string Holiday { get; set; }

        /// <summary>
        /// Date of holiday
        /// </summary>
        public DateTime HolidayDate { get; set; }

    }
}
