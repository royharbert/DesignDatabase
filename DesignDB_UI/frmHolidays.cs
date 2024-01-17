using DesignDB_Library.Models;
using DesignDB_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignDB_UI
{
    public partial class frmHolidays : Form
    {
        List<StringHolidaysModel> sHolidayList = new List<StringHolidaysModel>();
        List<CompanyHolidaysModel> HolidayList = new List<CompanyHolidaysModel>();

        public frmHolidays()
        {
            InitializeComponent();
            
            HolidayList = getHolidays();
            sHolidayList = make_hList(HolidayList);
            DataTable dtbl= makeTable(sHolidayList);
            dgvHolidays.DataSource = dtbl;
            dgvHolidays.Refresh();
            dgvHolidays.AutoGenerateColumns = false;

        }

        private List<CompanyHolidaysModel> getHolidays()
        {
            HolidayList = GlobalConfig.Connection.GenericGetAll<CompanyHolidaysModel>
                ("tblHolidaysList", "HolidayDate");
            return HolidayList;
        }    

        private List<StringHolidaysModel> make_hList(List<CompanyHolidaysModel> cHoliday)
        {            
            foreach (CompanyHolidaysModel holiday in cHoliday)
            {                
                StringHolidaysModel sHol = new StringHolidaysModel();
                sHol.Holiday = holiday.Holiday;
                sHol.HolidayDate = holiday.HolidayDate.ToShortDateString();            
                sHolidayList.Add(sHol);                
                }
            return sHolidayList;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Make table for update
            DataTable dt = new DataTable();

            dt.Columns.Add("Holiday");
            dt.Columns.Add("HolidayDate");
            int i = dgvHolidays.Rows.Count;
            
            for (int k = 0; k <i-1; k++ )
            {
                dt.Rows.Add(dgvHolidays.Rows[k].Cells[0].Value.ToString(), dgvHolidays.Rows[k].Cells[1].Value.ToString());
            }
            DesignDB_Library.Holiday_Update.UpdateHolidays(dt);
        }

        private static DataTable makeTable(List<StringHolidaysModel> hList)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Holiday");
            dt.Columns.Add("HolidayDate");

            foreach (StringHolidaysModel hDay in hList)
            {            
                 dt.Rows.Add( hDay.Holiday, hDay.HolidayDate);             
            }
            return dt;
           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    } 
}
