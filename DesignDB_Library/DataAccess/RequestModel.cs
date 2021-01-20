using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDBLibrary.Models
{
    /// <summary>
    /// Model of design request
    /// </summary>
    public class RequestModel        
    {
        public int Id { get; set; }
        public string RequestID { get; set; }
        public string EndCustomer { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string DesignRequestor { get; set; }
        public string QuoteType { get; set; }
        public string Priority { get; set; }
        public string Designer { get; set; }
        public string ProjectName { get; set; }
        public string OriginalQuote { get; set; }
        public string AssistedBy { get; set; }
        public string Category { get; set; }
        public string ArchitectureType { get; set; }
        public DateTime DateAssigned { get; set; }
        public DateTime DateAllInfoRX  { get; set; }
        public DateTime DateDue { get; set; }
        public string AwardStatus { get; set; }
        public DateTime DateLastUpdate { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime DateCompleted { get; set; }
        public int TotalHours { get; set; }
        public string Revision { get; set; }
        public string FileName { get; set; }
        public decimal BOM_Value { get; set; }
        public float PctProjectCovered { get; set; }
        public string ArchitectureDetails { get; set; }
        public string Comments { get; set; }
        public int CustomerLocation { get; set; }
    }
}
