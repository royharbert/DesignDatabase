using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Operations
{
    public static class RequestOps
    { 
        public static RequestModel CreateRevision(RequestModel request)
        {
            //save original with has revision status
            request.AwardStatus = "Has Revision";
            GlobalConfig.Connection.RequestUpdate(request);

            //increment rev letter 
            request.ProjectID = request.ProjectID.ToUpper();
            int loc = request.ProjectID.IndexOf("REV_");
            if (loc > -1)
            {
                request.OriginalQuote = request.ProjectID;
                char c = request.ProjectID[loc + 4];
                int asc = (int)c;
                char rev = (char)(asc + 1);
                char[] requestAsChars = request.ProjectID.ToCharArray();
                requestAsChars[loc + 4] = rev;
                request.ProjectID = new string(requestAsChars);
                if (request.ProjectID.Length > loc + 5)
                {
                    request.ProjectID = request.ProjectID.Remove(request.ProjectID.Length - 1, 1);
                }
            }
            else
            {
                //if original had no _REV then default to REV_B
                request.ProjectID += "_REV_B";
            }

            //Clear appropriate cells
            request.DateAssigned = DateTime.Now;
            request.DateAllInfoReceived = DateTime.Parse("1/1/1900");
            request.DateCompleted = DateTime.Parse("1/1/1900");
            request.DateDue = DateTime.Parse("1/1/1900");
            request.BOM_Value = 0;
            request.PercentageProjectCovered = 100;
            request.ReviewedBy = "";
            request.Pty = "";
            request.ArchitectureType = "";
            request.Category = "";
            request.AssistedBy = "";
            request.AwardStatus = "Pending";
            request.TotalHours = 0;
            request.Designer = "";
            //Save record
            InsertNewRequest(request);
            return request;
        }

        public static RequestModel Clone(RequestModel model)
        {
            //generate new PID            
            model.ProjectID = PID_Generator.MakePID(model.msoModel);

            //set model properties
            model.AssistedBy = "";
            model.Category = "";
            model.ArchitectureType = "";
            model.QuoteType = "";
            model.DateCompleted = DateTime.MinValue;
            model.ReviewedBy = "";
            model.BOM_Value = 0;
            model.PercentageProjectCovered = 0;
            model.AwardStatus = "Pending";
            model.TotalHours = 0;
            model.ArchitectureDetails = "";
            model.DateAllInfoReceived = DateTime.MinValue;
            model.DateAssigned = DateTime.Today;
            model.DateDue = new DateTime(1900,1,1);
            model.DateAllInfoReceived = new DateTime(1900, 1, 1);
            model.DateCompleted = new DateTime(1900, 1, 1);
            model.DateLastUpdate = new DateTime(1900, 1, 1);
            model.Pty = "";
            return model;
            
        }

        public static void DeleteRequest(RequestModel requestModel)
        {
            string db = GetConnectionString();
            List<RequestModel> requestModelList = new List<RequestModel>();
            requestModelList.Add(requestModel);
            DataTable dt = MakeRequestDataTable(requestModelList);
            using (SqlConnection con = new SqlConnection(db))
            {
                SqlCommand cmd = new SqlCommand("spRequest_DeleteByID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@RequestModel";
                param.Value = dt;
                cmd.Parameters.Add(param);
                
                param = new SqlParameter();
                param.ParameterName = "@ID";
                param.Value = requestModel.ID;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                System.Windows.Forms.MessageBox.Show("Request " + requestModel.ProjectID + " has been deleted.");
            }            
        }
        /// <summary>
        /// Make data table for requestmodel so we can use RequestTableModel in db data typed
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        public static DataTable MakeRequestDataTable(List<RequestModel> requests, bool includeID = false)
        {
            //Make table for update
            DataTable dt = new DataTable();
            RequestModel request = requests[0];
            string propName = "";
            
            Type T = request.GetType();
            PropertyInfo[] properties = T.GetProperties();            
    
            foreach (PropertyInfo property in properties)
            {
                propName = property.Name;               
                dt.Columns.Add(propName, property.PropertyType);
            }
            
            foreach (RequestModel req in requests)
            {
                DataRow row = dt.NewRow();
                for (int k = 0; k < properties.Length; k++)
                {
                    try
                    {
                        row[k] = properties[k].GetValue(req);
                    }
                    catch (Exception)
                    {
                        row[k] = DBNull.Value;
                    }
                }
                dt.Rows.Add(row);                
            }
            if (!includeID)
            {
                dt.Columns.Remove("ID");
            }
            dt.Columns.Remove("msoModel");
            return dt;
        }

        public static void RestoreRequest(RequestModel request)
        {
            bool saveSuccessful = true;
            string db = GetConnectionString();
            List<RequestModel> requests = new List<RequestModel>();
            requests.Add(request);
            DataTable dt = MakeRequestDataTable(requests);
         
            using (SqlConnection con = new SqlConnection(db))
            {
                //InsertNewRequest(requestModel);
                SqlCommand cmd = new SqlCommand("spRequest_UnDeleteByID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@RequestTableType";
                param.Value = dt;
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@ID";
                param.Value = request.ID    ;
                cmd.Parameters.Add(param);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    saveSuccessful = false;
                }
            }

            if (saveSuccessful)
            {
                System.Windows.Forms.MessageBox.Show("Request " + request.ProjectID + " has been restored.");
            }
            
        }
        public static void UpdateRequest(DataTable dt)
        {
            string db = GetConnectionString();
            using (SqlConnection con = new SqlConnection(db))
            {
                SqlCommand cmd = new SqlCommand("spRequest_Insert_TableTypeParam]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@RequestTableType";
                param.Value = dt;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }



        public static void InsertNewRequest(RequestModel request)
        {
            string db = GetConnectionString();
            List<RequestModel> requests = new List<RequestModel>();
            requests.Add(request);
            DataTable dt = MakeRequestDataTable(requests);
            using (SqlConnection con = new SqlConnection(db))
            {
                SqlCommand cmd = new SqlCommand("spRequest_Insert_TableTypeParam", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@RequestTableType";
                param.Value = dt;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //public static void InsertNewRequest(List<RequestModel> requests)
        //{
        //    string db = GetConnectionString();
        //    DataTable dt = MakeRequestDataTable(requests);
        //    using (SqlConnection con = new SqlConnection(db))
        //    {
        //        SqlCommand cmd = new SqlCommand("spRequest_Insert_TableTypeParam", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        SqlParameter param = new SqlParameter();
        //        param.ParameterName = "@RequestTableType";
        //        param.Value = dt;
        //        cmd.Parameters.Add(param);

        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        private static string GetConnectionString()
        {
            string db = "";
            if (GlobalConfig.DatabaseMode == DatabaseType.Live)
            {
                db = ConfigurationManager.ConnectionStrings["Live"].ConnectionString;
            }
            else
            {
                db = ConfigurationManager.ConnectionStrings["Sandbox"].ConnectionString;
            }
            return db;
        }
    }
}

