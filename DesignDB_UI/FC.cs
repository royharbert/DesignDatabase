using DesignDB_Library;
using DesignDB_Library.Models;
using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignDB_UI
{
    public class FC
    {
        public static void SetFormPosition(Form frm, int altLocationX = -1, int altLocationY = -1, 
            bool useDefaultLocation = true)
        {
            Point adjustedPoint = new Point(altLocationX, altLocationY);
            frm.StartPosition = FormStartPosition.Manual;
            if (useDefaultLocation)
            {
                GV.ScreenList = DesignDB_Library.Operations.Screens.GetScreenInfo();
                int Monitor = GV.ActiveScreen;
                int numScreens = GV.ScreenList.Count;
                if (Monitor > numScreens)
                {
                    Monitor = 1;
                    GV.ActiveScreen = 1;
                }
                ScreenModel model = GV.ScreenList[Monitor - 1];
                frm.Location = new Point(model.Xpos, model.Ypos);
            }
            else
            {
                frm.Location = new Point(altLocationX, altLocationY);
            }
        }

        public static void setDBMode(Form callingForm, bool isLive)
        {
            if (isLive)
            {
                GlobalConfig.SetDatabaseMode(DatabaseType.Live);
                GlobalConfig.AttachmentPath = @"\\rdcpstntapfil01\ANS_Design\\AttachmentsDesign";
                Properties.Settings.Default.DatabaseLive = true;
                Properties.Settings.Default.Save();
                callingForm.BackColor = Color.Silver;                
            }
            else
            {
                GlobalConfig.SetDatabaseMode(DatabaseType.Sandbox);
                GlobalConfig.AttachmentPath = @"\\rdcpstntapfil01\ANS_Design\\Sandbox\\AttachmentsDesign";
                Properties.Settings.Default.DatabaseLive = false;
                Properties.Settings.Default.Save();
                callingForm.BackColor = Color.IndianRed;                
            }
        }
        public static bool isFormOpen(string formName)
        {
            FormCollection fc = Application.OpenForms;
            bool isOpen = false;

            foreach (Form frm in fc)
            {                 
                if (frm.Name == formName)
                {
                    isOpen = true;
                    break;
                }
                else
                {
                    isOpen = false;
                }
            }
            return isOpen;
        }

        public static void clearTextinControl(System.Windows.Forms.Control.ControlCollection controls, Type type)
        {
            foreach (Control control in controls)
            {
                if (control.GetType() == type)
                {
                    control.Text = string.Empty;
                }
            }
        }

        public static frmRequests DisplayRequestForm(RequestModel request = null)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (request != null)
            {
                GV.REQFORM.Request = request; 
            }
            GV.REQFORM.setDateRangeControls();
            GV.REQFORM.Show();
            Point altLocation = GV.REQFORM.FormLocation;
            FC.SetFormPosition(GV.REQFORM, altLocation.X, altLocation.Y, GV.REQFORM.UseDefaultLocation);            
            GV.REQFORM.BringToFront();
            Cursor.Current = Cursors.Default;
            return GV.REQFORM;
        }
    }
}

