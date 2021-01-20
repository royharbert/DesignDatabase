using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class ForecastModel
    {
        public int Quantity { get; set; }
        public string ModelNumber { get; set; }
        public string Description { get; set; }
        public List<string> Quotes { get; set; }

        public ForecastModel()
        {
            List<string> quotes = new List<string>();
            Quotes = quotes;
        }

        /// <summary>
        /// merges duplicate products
        /// </summary>
        /// <param name="product"></param>
        public void MergeLine(BOM_Model product)
        {
            Quantity = product.Quantity + Quantity;
            Quotes.Add(product.Quote);
        }
    } 
}
