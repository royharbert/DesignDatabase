using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignDB_Library.Models;
using System.Data.SqlClient;

namespace DesignDB_Library.DataAccess
{
    public class SqlConnector : IDataConnection
    {        
        public static string db { get; set; }

        public void AddUser(UserModel NewUser)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@UserName", NewUser.UserName);
                p.Add("@PW", NewUser.PW);
                p.Add("@Priviledge", NewUser.Priviledge);
                p.Add("ActiveDesigner", NewUser.ActiveDesigner);
                p.Add("ID", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);


                //TODO - Add try catch to handle duplicate users
                connection.Execute("dbo.spUser_Add", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteUser(int OldUser)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@Ident", OldUser);
                connection.Execute("dbo.spUser_Delete", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<UserModel> GetUsers_All()
        {
            List<UserModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                output = connection.Query<UserModel>("dbo.spUsers_GetAll").ToList();

                foreach (UserModel User in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@UserName", User.UserName);
                    p.Add("@PW", User.PW);
                    p.Add("@Priviledge", User.Priviledge);
                    p.Add("@ActiveDesigner", User.ActiveDesigner);
                    p.Add("@ID", User.ID,DbType.Int32);

                }
            }

            return output;

        }


        public void UpdateUser(UserModel ThisUser)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {

                var p = new DynamicParameters();
                p.Add("@UName", ThisUser.UserName);
                p.Add("@PWord", ThisUser.PW);
                p.Add("@Priv", ThisUser.Priviledge);
                p.Add("@Active", ThisUser.ActiveDesigner);
                p.Add("@Ident", ThisUser.ID);


                connection.Execute("dbo.spUser_Update", p, commandType: CommandType.StoredProcedure);
            }
        }


        public UserModel GetUser(string userName)
        {

            UserModel myUser = new UserModel(userName, "", "", "", 0);

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@uName", myUser.UserName, DbType.String, ParameterDirection.Input);
                List<UserModel> output = connection.Query<UserModel>("dbo.spUser_GetByName", p, commandType: CommandType.StoredProcedure).ToList();
                if (output.Count == 0)
                {
                    return null;
                }
                else
                {
                    return output[0];
                }

            }

        }

        public UserModel GetUser(int userID)
        {

            UserModel myUser = new UserModel("", "", "", "", 0);

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@Ident", userID, DbType.Int32, ParameterDirection.Input);
                List<UserModel> output = connection.Query<UserModel>("dbo.spUser_Get", p, commandType: CommandType.StoredProcedure).ToList();
                if (output.Count == 0)
                {
                    return null;
                }
                else
                {
                    return output[0];
                }

            }

        }

        public List<CompanyHolidaysModel> GetAllHolidays()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<CompanyHolidaysModel> output = connection.Query<CompanyHolidaysModel>("dbo.spHolidays_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public void AddCountry(string ctry)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@cAdd", ctry, DbType.String);
                connection.Execute("dbo.spCountries_Add", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteCountry(int idx)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@Idx", idx, DbType.Int32);
                connection.Execute("dbo.spCountry_Delete", p, commandType: CommandType.StoredProcedure);
            }

        }

        public void UpdateCountry(int idx, string country)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@Idx", idx, DbType.Int32);
                p.Add("@country", country, DbType.String);
                connection.Execute("dbo.spCountry_Update", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<CountriesModel> GetAllCountries()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<CountriesModel> output = connection.Query<CountriesModel>("dbo.spCountries_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public void AddDesigner(DesignersReviewersModel designer)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@UserName", designer.Designer, DbType.String);
                p.Add("@PW", designer.Pwd, DbType.String);
                p.Add("@Priviledge", designer.Priviledge, DbType.Int32);
                p.Add("@ActiveDesigner", designer.ActiveDesigner, DbType.Boolean);
                p.Add("@ID", designer.ID, DbType.Int32);
                connection.Execute("dbo.spDesigner_Add", p, commandType: CommandType.StoredProcedure);
            };
        }

        public void DeleteDesigner(DesignersReviewersModel designer)
        {
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
                {
                    var p = new DynamicParameters();
                    p.Add("@Ident", designer.ID);
                    connection.Execute("dbo.spDesigner_Delete", p, commandType: CommandType.StoredProcedure);
                }
            }
        }

        public void UpdateDesigner(DesignersReviewersModel designer)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {

                var p = new DynamicParameters();
                p.Add("@UName", designer.Designer);
                p.Add("@PWord", designer.Pwd);
                p.Add("@Priv", designer.Priviledge);
                p.Add("@Active", designer.ActiveDesigner);
                p.Add("@Ident", designer.ID);


                connection.Execute("dbo.spDesigner_Update", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<DesignersReviewersModel> GetAllDesigners()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<DesignersReviewersModel> output = connection.Query<DesignersReviewersModel>("dbo.spDesigners_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<DesignersReviewersModel> Reviewers_GetActive()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<DesignersReviewersModel> output = connection.Query<DesignersReviewersModel>("dbo.spReviewers_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<DesignersReviewersModel> Reviewers_GetAll()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<DesignersReviewersModel> output = connection.Query<DesignersReviewersModel>("dbo.spReviewers_GetActive", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<MSO_Model> GetAllMSO()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<MSO_Model> output = connection.Query<MSO_Model>("dbo.spMSO_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<CityModel> GetAllCities()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<CityModel> output = connection.Query<CityModel>("dbo.spCities_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<StateModel> GetAllStates()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<StateModel> output = connection.Query<StateModel>("dbo.spStates_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<RegionsModel> GetAllRegions()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<RegionsModel> output = connection.Query<RegionsModel>("dbo.spRegions_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<SalespersonModel> SalesGetActive()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<SalespersonModel> output = connection.Query<SalespersonModel>("dbo.spSalespersons_GetActive", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<DesignersReviewersModel> DesignersGetActive()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<DesignersReviewersModel> output = connection.Query<DesignersReviewersModel>("dbo.spDesigners_GetActive", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public int GetSequence()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<SequenceModel> output = connection.Query<SequenceModel>("dbo.spSequence_Get", commandType: CommandType.StoredProcedure).ToList();
                return output[0].Sequence;
            }
        }

        public void SetSequence(int seq)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@Seq", seq, DbType.Int32);
                connection.Execute("dbo.spSequence_Update", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<RequestModel> GetRequestByPID(string PID)
        {
            RequestModel myUser = new RequestModel();

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@PID", PID, DbType.String, ParameterDirection.Input);
                List<RequestModel> output = connection.Query<RequestModel>("dbo.spRequest_GetByPID", p, commandType: CommandType.StoredProcedure).ToList();

                return output;
            }
        }  

        public void RequestUpdate(RequestModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {               
                DynamicParameters p = makParams(model);
                connection.Execute("[dbo].[spRequest_UpdateByPID]", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void RequestInsert(RequestModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                DynamicParameters p = makParams(model);
                connection.Execute("[dbo].[spRequest_Insert]", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<RequestModel> GetRequestsForDesignerUpdate(string designer)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("Designer", designer, DbType.String, ParameterDirection.Input);
                List<RequestModel> output = connection.Query<RequestModel>("dbo.spRequests_GetForDesignerUpdate", p, commandType: CommandType.StoredProcedure).ToList();

                return output;
            }
        }

        private DynamicParameters makParams(RequestModel request)
        {
            var p = new DynamicParameters();
            p.Add("ID", request.ID, DbType.Int32);
            p.Add("ProjectID", request.ProjectID, DbType.String);
            p.Add("MSO", request.MSO, DbType.String);
            p.Add("Cust", request.Cust, DbType.String);
            p.Add("City", request.City, DbType.String);
            p.Add("State", request.ST, DbType.String);
            p.Add("Country", request.Country, DbType.String);
            p.Add("Region", request.Region, DbType.String);
            p.Add("DesignRequestor", request.DesignRequestor, DbType.String);
            p.Add("QuoteType", request.QuoteType, DbType.String);
            p.Add("Priority", request.Pty, DbType.String);
            p.Add("Designer", request.Designer, DbType.String);
            p.Add("ProjectName", request.ProjectName, DbType.String);
            p.Add("OriginalQuote", request.OriginalQuote, DbType.String);
            p.Add("AssistedBy", request.AssistedBy, DbType.String);
            p.Add("Category", request.Category, DbType.String);
            p.Add("ArchitectureType", request.ArchitectureType, DbType.String);
            p.Add("DateAssigned", request.DateAssigned, DbType.Date);
            p.Add("DateAllInfoReceived", request.DateAllInfoReceived, DbType.Date);
            p.Add("DateDue", request.DateDue, DbType.Date);
            p.Add("AwardStatus", request.AwardStatus, DbType.String);
            p.Add("DateLastUpdate", request.DateLastUpdate, DbType.Date);
            p.Add("ReviewedBy", request.ReviewedBy, DbType.String);
            p.Add("DateCompleted", request.DateCompleted, DbType.Date);
            p.Add("TotalHours", request.TotalHours, DbType.Int32);
            p.Add("BOM_Value", request.BOM_Value, DbType.Decimal);
            p.Add("PercentageProjectCovered", request.PercentageProjectCovered, DbType.Int32);
            p.Add("ArchitectureDetails", request.ArchitectureDetails, DbType.String);
            p.Add("Comments", request.Comments, DbType.String);     

            return p;
        }

        public void InsertInto_tblAttachments(AttachmentModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                DynamicParameters p = new DynamicParameters();
                //Unique ID for record
                p.Add("@ID", model.ID, DbType.String, ParameterDirection.Input);
                //Text to display in grid. Equals File Name no Path
                p.Add("@DisplayText", model.DisplayText, DbType.String);
                //Path to server
                p.Add("@ItemType", model.ItemType, DbType.String);
                //Project ID
                p.Add("@PID", model.PID, DbType.String);
                connection.Execute("spAttachment_Insert", p, commandType: CommandType.StoredProcedure);                
            }
            //System.Windows.Forms.MessageBox.Show("Operation Complete");
        }

        public void DeleteAttachment(AttachmentModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                DynamicParameters p = new DynamicParameters();
      
                //Project ID
                p.Add("@PID", model.PID, DbType.String);
                p.Add("@DisplayText", model.DisplayText, DbType.String);
                connection.Execute("spAttachment_Delete", p, commandType: CommandType.StoredProcedure);
            }
        }
        public List<AttachmentModel> GetAttachments(string PID)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@PID", PID, DbType.String, ParameterDirection.Input);
                List<AttachmentModel> output = connection.Query<AttachmentModel>("spAttachments_GetByPID", p, commandType: CommandType.StoredProcedure).ToList();

                return output;
            }
        }

        public List<DesignerLoadModel> DoLoadReport()
        {
            //get designers list
            List<DesignersReviewersModel> designers = GlobalConfig.Connection.DesignersGetActive();

            //make new loadList
            List<DesignerLoadModel> loadList = new List<DesignerLoadModel>();

            //foreach designer, get pending incomplete
            foreach (DesignersReviewersModel designerModel in designers)
            {
                string designer = designerModel.Designer;

                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
                {
                    var p = new DynamicParameters();

                    p.Add("Designer", designer, DbType.String, ParameterDirection.Input);
                    List<DesignerLoadModel> output = connection.
                        Query<DesignerLoadModel>("spDesigner_LoadReport", p, commandType: CommandType.StoredProcedure).ToList();

                    //add to loadList
                    foreach (DesignerLoadModel project in output)
                    {
                        loadList.Add(project);
                    }
                }
            }

            return loadList;
        }

        public List<RequestModel> DateRangeSearch_Unfiltered(DateTime StartDate, DateTime EndDate, string SearchTerm)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@StartDate", StartDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@EndDate", EndDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@SearchField", SearchTerm, DbType.String, ParameterDirection.Input);
                List<RequestModel> output = null;


                output = connection.Query<RequestModel>("spRequests_DateRangeSearch_Unfiltered_DateAssigned_PendingOnly", 
                    p, commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<RequestModel> DateRangeSearch_MSOFiltered(DateTime StartDate, DateTime EndDate, string SearchTerm, string mso)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@StartDate", StartDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@EndDate", EndDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@MSO", mso, DbType.String, ParameterDirection.Input);
                List<RequestModel> output = null;

                switch (SearchTerm)
                {
                    case "DateAssigned":
                        output = connection.Query<RequestModel>("spRequests_DateRangeSearch_MSOFiltered_DateAssigned",
                            p, commandType: CommandType.StoredProcedure).ToList();
                        break;

                    case "DateDue":
                        output = connection.Query<RequestModel>("spRequests_DateRangeSearch_MSOFiltered_DateDue",
                            p, commandType: CommandType.StoredProcedure).ToList();
                        break;

                    case "DateCompleted":
                        output = connection.Query<RequestModel>("spRequests_DateRangeSearch_MSOFiltered_DateCompleted",
                            p, commandType: CommandType.StoredProcedure).ToList();
                        break;

                    default:
                        break;
                }
                return output;
            }
        }

        List<DesignersReviewersModel> IDataConnection.GetDesigner(string designerName)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@Designer", designerName, DbType.String, ParameterDirection.Input);
                
                List<DesignersReviewersModel>  output = connection.Query<DesignersReviewersModel>("spDesigner_GetByName",
                    p, commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<RequestModel> GetSnapshotData(string MSO, DateTime start, DateTime end)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

  
                p.Add("@MSO", MSO, DbType.String, ParameterDirection.Input);
                p.Add("@StartDate", start, DbType.DateTime, ParameterDirection.Input);
                p.Add("@EndDate", end, DbType.DateTime, ParameterDirection.Input);
                List<RequestModel> output = connection.Query<RequestModel>("spRequests_DateRangeSearch_MSOFiltered_DateAssigned",
                    p, commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public void UpdateSnapshotMSO_s(string mso)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                DynamicParameters p = new DynamicParameters();

                //Project ID
                p.Add("@MSO", mso, DbType.String);               
                connection.Execute("spSnapshotMSO_S_InsertInto", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<string> GetSnapshotMSO_s()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {  
                List<string> output = connection.Query<string>("spSnapshotMSO_S_GetAll",
                    commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public void ClearTable(string tableName)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                DynamicParameters p = new DynamicParameters();

                //Project ID
                p.Add("@TableName", tableName, DbType.String);
                connection.Execute("spRecords_DeleteAllFromTable", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<RequestModel> GetDeletedRecordByPID(string PID)
        {
            RequestModel request = new RequestModel();

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@PID", PID, DbType.String, ParameterDirection.Input);
                List<RequestModel> output = 
                    connection.Query<RequestModel>("dbo.spDeletedRecords_GetByPID", p, commandType: CommandType.StoredProcedure).ToList();

                return output;
            }
        }

        public List<RequestModel> GetOpenRequests()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<RequestModel> output = connection.Query<RequestModel>("dbo.[spRequests_GetOpen]", commandType: CommandType.StoredProcedure).ToList();

                return output;
            }
        }

        public List<RequestModel> GetOverdueRequests(DateTime dueDate)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@DueDate", dueDate, DbType.DateTime, ParameterDirection.Input);
                List<RequestModel> output = connection.Query<RequestModel>("dbo.spRequests_Overdue", p, commandType: CommandType.StoredProcedure).ToList();

                return output;
            }
        }

        public List<RequestModel> SearchMultipleFields(string whereClause)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                DynamicParameters p = new DynamicParameters();

                //Project ID
                p.Add("@WhereClause", whereClause, DbType.String);
                List<RequestModel> output = connection.Query<RequestModel>("dbo.spRequests_SearchVariableFields", p, commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public void SalespersonAdd(SalespersonModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@UserName", model.SalesPerson, DbType.String);      
                p.Add("@ActiveDesigner", model.Active, DbType.Boolean);
                p.Add("@ID", model.Id, DbType.Int32,ParameterDirection.Output);
                connection.Execute("dbo.spSalesperson_Add", p, commandType: CommandType.StoredProcedure);
            };
        }

        public void SalespersonDelete(SalespersonModel model)
        {
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
                {
                    var p = new DynamicParameters();
                    p.Add("@Ident", model.Id);
                    connection.Execute("dbo.spSalesperson_Delete", p, commandType: CommandType.StoredProcedure);
                }
            }
        }

        public void SalespersonUpdate(SalespersonModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {

                var p = new DynamicParameters();
                p.Add("@UName", model.SalesPerson);      
                p.Add("@Active", model.Active);
                p.Add("@Ident", model.Id);

                connection.Execute("dbo.spSalesperson_Update", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<SalespersonModel> SalespersonsGetAll()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {

                List<SalespersonModel> output = connection.Query<SalespersonModel>("dbo.spSalespersons_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }
    }
}
