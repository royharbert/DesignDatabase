
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PW { get; set; }
        public int Priviledge { get; set; }
        public bool ActiveDesigner { get; set; }

        public UserModel()
        {

        }

        public UserModel (string userName, string pW, string priviledge, string activeDesigner, string ID)
        {
            UserName = userName;
            PW = pW;

            int pValue = 0;
            int.TryParse(priviledge, out pValue);
            Priviledge = pValue;

            bool pActive = true;
            bool.TryParse(activeDesigner, out pActive);
            ActiveDesigner = pActive;

            int IDval = 0;
            int.TryParse(ID, out IDval);
            Id = IDval;
        }
    }


}
