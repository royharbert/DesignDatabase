﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DesignDB_Library.Models;
using DesignDB_Library;
using System.Configuration;

namespace DesignDB_Library
{
    public class Holiday_Update
    {
        public static void UpdateHolidays(DataTable dt)
        {
#if (ACTIVE)
            string db = ConfigurationManager.ConnectionStrings["Live"].ConnectionString;
#else
            string db = ConfigurationManager.ConnectionStrings["Sandbox"].ConnectionString;
#endif
            using (SqlConnection con = new SqlConnection(db))
            {
                SqlCommand cmd = new SqlCommand("spHolidays_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@HolidayTable";
                param.Value = dt;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }
    }
}
