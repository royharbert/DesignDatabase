﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    /// <summary>
    /// model to hold line of BOM info
    /// </summary>
    public class BOM_Model
    {
        public int ID { get; set; }
        public double Quantity { get; set; }
        public string ModelNumber { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Quote ID
        /// </summary>
        public string Quote { get; set; }
        /// <summary>
        /// BOM file name
        /// </summary>
        public string DisplayText { get; set; }
    }

}
