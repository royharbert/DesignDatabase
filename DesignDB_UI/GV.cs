using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 

namespace DesignDB_UI
{
    public class GV
    {
        public static bool Exiting { get; set; }
        public static frmLogView LogViewer { get; set; }
        public static int ActiveScreen { get; set; }
        public static List<ScreenModel> ScreenList { get; set; }
        public static frmDateMSO_Picker PickerForm { get; set; }
        public static frmInput InputForm { get; set; }
        public static frmRequests REQFORM { get; set; }
        public static DesignersReviewersModel USERNAME { get; set; }
        public static Form LOGIN { get; set; }
        public static Form MAINMENU { get; set; }
        public static frmMultiResult MultiResult { get; set; }
        public static Mode MODE
        {
            get
            {
                return mode;
            }
            set
            {
                if (mode != PreviousMode)
                {
                    PreviousMode = mode; 
                }
                mode = value;                
            }
        }
        public static Mode PreviousMode { get; set; }

        private static Mode mode = Mode.None;
    }
}
