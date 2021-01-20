using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDBLibrary.Models
{
    /// <summary>
    /// List of designers and reviewers
    /// </summary>
    public class DesignersReviewersModel
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Full name of designer
        /// </summary>
        public string Designer { get; set; }

        /// <summary>
        /// Level of access for designer
        /// </summary>
        public int Priviledge { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Indicates if individual is active designer
        /// </summary>
        public bool ActiveDesigner { get; set; }
    }
}
