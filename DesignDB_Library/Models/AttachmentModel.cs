using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class AttachmentModel
    {
        /// <summary>
        /// Unique ID for record
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Project ID
        /// </summary>
        public string PID { get; set; }
        /// <summary>
        /// Text to be displayed in gridview
        /// </summary>
        public string DisplayText { get; set; }
        /// <summary>
        /// Type of attachment (BOM, Drawing ...)
        /// </summary>
        public string ItemType { get; set; }  
        /// <summary>
        /// Filename of file to save
        /// </summary>
        public string FileToSave { get; set; }
        /// <summary>
        /// archive MSO
        /// </summary>
        public string MSO { get; set; }
        /// <summary>
        /// archive year
        /// </summary>
        public string Year { get; set; }
        /// <summary>
        /// SQL full file path
        /// </summary>
        public string FullSavePath { get; set; }
        /// <summary>
        /// Full path to archive file
        /// </summary>
        public string ArchiveFullSavePath { get; set; }


        public AttachmentModel()
        {

        }

        //public AttachmentModel(string pID, string displayText, string itemType, string serverPath, string fileToSave)
        //{
        //    PID = pID;
        //    DisplayText = displayText;
        //    ItemType = itemType;            
        //}
    }
}
