using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class ShipmentLineModel
    {
        public DateTime SODate { get; set; }
        public string SOCust { get; set; }
        public string PartNumber { get; set; }
        public string   Desc { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public double Quantity { get; set; }
        public string SONumber { get; set; }
        public int ExcelRow { get; set; }
        public string QuoteCity { get; set; }
        public string QuoteState { get; set; }
        public string QuoteDateCompleted { get; set; }
        public string BOM_Quantity { get; set; }
    }
}
