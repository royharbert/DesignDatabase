using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignDB_Library.Operations
{
    public class FilePicker
    {
        public string GetFile(string initialDir = "")
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                string fileName = "";
                if (initialDir != "")
                {
                    ofd.InitialDirectory = initialDir;
                    ofd.RestoreDirectory = true;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        fileName = ofd.FileName;
                    }
                }

                return fileName;

            }
        }

        public FilePicker()
        {

        }
    }
}
