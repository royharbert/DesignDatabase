using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDBLibrary.Models
{
    /// <summary>
    /// Represents list of world countries
    /// </summary>
    public class CountriesModel
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Full name of country
        /// </summary>
        public string CountryName { get; set; }
    }
}
