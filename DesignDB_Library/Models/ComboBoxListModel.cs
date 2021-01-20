using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignDB_Library.Models
{
    public delegate void ListQuery();
    public class ComboBoxListModel
    {
        public ListQuery query { get; set; }
        public ComboBox comboBox { get; set; }
        public string DisplayMember { get; set; }
        public int SelectedIndex { get; set; }
    }
}
