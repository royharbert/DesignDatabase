using DesignDB_Library;
using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Operations
{
    public static class PID_Generator
    {      
        public static string MakePID(MSO_Model MSO)
        {            
            string seq = getSequence(true);

            return  MSO.Abbreviation + "_" + makeDateString() + "_" + seq + "_REV_A";
        }

        private static string makeDateString()
        {
            string today = DateTime.Now.ToShortDateString();
            string[] dte = today.Split('/');
            string mo = dte[0].PadLeft(2, '0');
            string dy = dte[1].PadLeft(2, '0');
            string yr = dte[2].Substring(dte[2].Length - 2);
            return yr + mo + dy;
        }

        private static string  getSequence(bool increment)
        {
            int seq =  GlobalConfig.Connection.GetSequence();
            if (increment)
            {
                GlobalConfig.Connection.SetSequence(seq + 1);
            }

            return seq.ToString().PadLeft(4,'0');
        }
    }

  
}
