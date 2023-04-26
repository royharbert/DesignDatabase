using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DesignDB_Library;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace DesignDB_Library.Operations
{
    public static class AttachmentOps
    {
        public static List<AttachmentModel> DeleteAttachment(AttachmentModel model)
        {
            bool success = true;

            //delete the file
            string fullFilePath = GlobalConfig.AttachmentPath + "\\" + model.PID + "\\" + model.DisplayText;
            List<AttachmentModel> modelList= new List<AttachmentModel>();

            try
            {
                File.Delete(fullFilePath);
            }
            catch (Exception ex)
            {
                string exDescription = ex.Message;
                success = false;
            }

            //delete the record
            if (success)
            {
                GlobalConfig.Connection.DeleteAttachment(model);
                modelList = GlobalConfig.Connection.GetAttachments(model.PID);
                MessageBox.Show("Attachment " + model.DisplayText + " has been deleted");
            }

            return modelList;
        }

        public static void ImportAttachmentsFromExcel(Excel.Application xlApp, TextBox txtBox)
        {
            AttachmentModel model = new AttachmentModel();
            Excel.Worksheet sheet = (Excel.Worksheet)xlApp.ActiveSheet;
            xlApp.Visible = true;

            //data begins in line 2
            int row = 2;
            string indexCell = sheet.Cells[row, 2].Value.ToString();
            while (indexCell != "")
            {
                model.PID = sheet.Cells[row, 2].Value.ToString();
                model.DisplayText = sheet.Cells[row, 3].Value.ToString();
                model.ItemType = sheet.Cells[row, 4].Value.ToString();
                
                GlobalConfig.Connection.InsertInto_tblAttachments(model);
                row++;
                if (sheet.Cells[row, 2].Value != null)
                {
                    indexCell = sheet.Cells[row, 2].Value.ToString();
                    txtBox.Text = row.ToString();
                }
            }
            

        }
    }
}
