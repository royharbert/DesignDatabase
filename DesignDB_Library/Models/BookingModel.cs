using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class BookingModel
    {
        public string CustAcctName { get; set; }
        public string ProductFamily { get; set; }
        public string ProductLine { get; set; }
        public string CustPO { get; set; }
        public string OrderLineStatus { get; set; }
        public string SalesOrderNumber{ get; set; }
        public float OrderDetailLineNo { get; set; }
        public string CustItemNo { get; set; }
        public string ProductNumber { get; set; }
        public string InventoryItemStatusCode { get; set; }
        public string ProductName { get; set; }
        public int OrderedQty { get; set; }
        public float OrderedLineAmount { get; set; }
        public DateTime OrderedDate { get; set; }
        public DateTime CRDDate { get; set; }
        public DateTime SSDDate { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }

    }
}
