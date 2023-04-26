using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DesignDB_Library.Models;
using System.Windows.Forms;

namespace DesignDB_Library.Operations
{
    public static class FileOps
    {
        public static bool SaveAttFile(AttachmentModel model)
        {
            string fullSavePath = GlobalConfig.AttachmentPath + "\\" + model.PID;
            bool fileSaved = false;

            try
            {
                if (!Directory.Exists(fullSavePath))
                {
                    Directory.CreateDirectory(fullSavePath);
                }
                string fullFileName = fullSavePath + "\\" + model.DisplayText;
                File.Copy(model.FileToSave, fullFileName, true);
                MessageBox.Show("1 file copied");
                fileSaved = true;
            
    }
            catch (Exception e)
            {
                if(e.Message.Contains("network path was not found"))
                {
                    MessageBox.Show("Attachment server unreachable. Check VPN and network status" );
                    fileSaved = false;
                }                
            }
            return fileSaved;
            
        }
    }
}
