using ScheduleDatabaseClassLibrary.GeneralOps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class FE_Model : IHaveID
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ManagerID { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string Region { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }
        public bool Active { get; set; } 

        public FE_Model()
        {
            this.Active = true;

        }

        public FE_Model(int ID, string FirstName, string LastName, string ManagerID, string Region, string Phone, string EMail, bool Active = true)
        {
            this.ID = ID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.ManagerID = ManagerID;
            this.Region = Region;
            this.Phone = Phone;
            this.EMail = EMail;
            this.Active = Active;
        }

        public override string ToString()
        {
            return this.FullName;
        }

    }
}
