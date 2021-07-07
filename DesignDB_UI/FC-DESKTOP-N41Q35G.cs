using DesignDB_Library.Models;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignDB_UI
{
    public class FC
    {      
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

        public static frmDateMSO_Picker DisplayPicker()
        {            
            frmDateMSO_Picker returnForm = null;
            if (GV.PickerForm == null)
            {
                returnForm = new frmDateMSO_Picker();                
                GV.PickerForm = returnForm;
                GV.PickerForm.ShowDialog();
            }
            else
            {
                GV.PickerForm.Visible = true;
                returnForm = GV.PickerForm;
            }           
            return returnForm;
        }

        public static frmRequests DisplayRequestForm(RequestModel request = null)
        {
            if (GV.REQFORM == null)
            {
                frmRequests frmRequests = new frmRequests();
                frmRequests.Show();                
                GV.REQFORM = frmRequests;
                if (request != null)
                {
                    GV.REQFORM.Request = request;
                }
            }
            else
            {
                GV.REQFORM.Visible = true;
                if (request != null)
                {
                    GV.REQFORM.Request = request;
                }
            }

            GV.REQFORM.BringToFront();

            return GV.REQFORM;
        }
    }
}

