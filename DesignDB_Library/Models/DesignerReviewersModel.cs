using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    /// <summary>
    /// List of designers and reviewers
    /// </summary>
    public class DesignersReviewersModel
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public int ID { get; set; }

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
        public string Pwd { get; set; }

        /// <summary>
        /// Indicates if individual is active designer
        /// </summary>
        public bool ActiveDesigner { get; set; }

        public bool ActiveReviewer { get; set; }

        public DesignersReviewersModel()
        {

        }

        public DesignersReviewersModel(string designerName, string pW, string priviledge, string activeDesigner, string id, string activeReviewer)
        {
            Designer = designerName;
            Pwd = pW;

            int pValue = 0;
            int.TryParse(priviledge, out pValue);
            Priviledge = pValue;

            bool pActive = true;
            bool.TryParse(activeDesigner, out pActive);
            ActiveDesigner = pActive;

            bool pActiveReviewer = true;
            bool.TryParse(activeReviewer, out pActiveReviewer);
            ActiveReviewer = pActive;

            int IDval = 0;
            int.TryParse(id, out IDval);
            ID = IDval;
        }
    }
}
