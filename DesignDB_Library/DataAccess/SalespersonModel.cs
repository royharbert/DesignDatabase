using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDBLibrary.Models
{
    public class SalespersonModel
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Salesperson full name
        /// </summary>
        public string SalesPerson { get; set; }

        /// <summary>
        /// indicates whether salesperson is currently active
        /// </summary>
        public bool Active { get; set; }
    }
}
