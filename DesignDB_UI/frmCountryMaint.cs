using DesignDB_Library;
using DesignDB_Library.Models;
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
    public partial class frmCountryMaint : Form
    {
        public frmCountryMaint()
        {
            InitializeComponent();
            refreshLBox();
        }

        private void refreshLBox()
        {
            List<CountriesModel> cList = GlobalConfig.Connection.GetAllCountries();
            lbxCountries.DataSource = null;
            lbxCountries.DataSource = cList;
            lbxCountries.DisplayMember = "Country";            
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            GlobalConfig.Connection.AddCountry(txtCountry.Text);
            MessageBox.Show(txtCountry.Text + " Added");
            txtCountry.Clear();
            refreshLBox();            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            CountriesModel dCtry = (CountriesModel)lbxCountries.SelectedItem;
            string country = dCtry.Country;
            int id = dCtry.ID;
            country = txtCountry.Text;
            GlobalConfig.Connection.UpdateCountry(id, country);
            MessageBox.Show(country + " Updated");
            refreshLBox();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbxCountries.SelectedItems.Count != 0)
            {
                CountriesModel dCtry = (CountriesModel)lbxCountries.SelectedItem;
                string country = dCtry.Country;
                int id = dCtry.ID;
                GlobalConfig.Connection.DeleteCountry(id);
                MessageBox.Show(country + " removed");
                refreshLBox();  
            }
            else
            {
                MessageBox.Show("Please select country from list");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCountryMaint_Load(object sender, EventArgs e)
        {

        }

        private void lbxCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxCountries.SelectedItems.Count != 0)
            {
                CountriesModel cm = (CountriesModel)lbxCountries.SelectedItem;
                txtCountry.Text = cm.Country;
            }
        }
    }
}
