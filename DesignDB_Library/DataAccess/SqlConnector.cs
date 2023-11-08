using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignDB_Library.Models;
using System.Data.SqlClient;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Diagnostics.Eventing.Reader;

namespace DesignDB_Library.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        public bool FE_Update(FE_Model fe)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@ID", fe.ID, DbType.Int32);
                p.Add("@FirstName", fe.FirstName, DbType.String);
                p.Add("@LastName", fe.LastName, DbType.String);
                p.Add("@ManagerID", fe.ManagerID, DbType.String);
                p.Add("@Region", fe.Region, DbType.String);
                p.Add("@Phone", fe.Phone, DbType.String);
                p.Add("@EMail", fe.EMail, DbType.String);
                p.Add("@Active", fe.Active, DbType.Boolean);
                object success = null;
                bool rtn;
                try
                {
                    success = connection.ExecuteScalar<string>("dbo.spFE_Update", p, commandType: CommandType.StoredProcedure);
                    rtn = true;
                }
                catch (Exception)
                {
                    rtn = false;   
                }

                return rtn;
            }
        }
        public string GetStateAbbreviation(string stateName)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@State", stateName, DbType.String);


                string output = connection.ExecuteScalar<string>("dbo.spStateAbbreviationGet", p,
                    commandType: CommandType.StoredProcedure);
                string abbreviation = output;
                return abbreviation;
            }
        }

        /// <summary>
        /// Returns List<BOMLineModel> that includes PID, BOM file name and DateCompleted
        /// </BOMLineModel>
        /// </summary>
        /// <param name="PIDs"></param>
        /// <returns></returns>
        public List<BOMLineModel> getBOMList(string PIDs)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@PIDs", PIDs, DbType.String);


                List<BOMLineModel> output = connection.Query<BOMLineModel>("dbo.spBOM_GetFileName", p,
                    commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }
        public List<RollupRequestModel> GetRollupRequests(DateTime startDate, DateTime endDate)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@StartDate", startDate, DbType.Date);
                p.Add("@EndDate", endDate, DbType.Date);


                List<RollupRequestModel> output = connection.Query<RollupRequestModel>("dbo.spRollupSelectQuery", p,
                    commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }
        public List<MSO_Model> MSO_GetByTier(int tier)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@Tier", tier, DbType.Int16);

                List<MSO_Model> output = connection.Query<MSO_Model>("dbo.spMSO_GetByTier", p,
                    commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }
        public bool MSO_Update(MSO_Model model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@MSO", model.MSO, DbType.String);
                p.Add("@TLA", model.Abbreviation, DbType.String);
                p.Add("@Tier", model.Tier, DbType.Int16);
                p.Add("@Active", model.Active, DbType.Boolean);
                p.Add("@ID", model.ID, DbType.Int32);
                bool rtn = true;
                object success = new object();
                try
                {
                    //throw new Exception();
                    success = connection.ExecuteScalar("dbo.spMSO_Update", p,
                                commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    if(success.ToString() != "True")
                    {
                        rtn = false;
                    }
                }
                return rtn;
            }
        }
        public List<RequestModel>GetRequestsDeleted()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<RequestModel> output = connection.Query<RequestModel>("spRequests_GetOpen",
                    commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }
        public List<T> GenericConditionalGetAll<T>(string tableName, string conditionColumn, string condition, string orderByField = null)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@TableName", tableName, DbType.String);
                p.Add("@ConditionColumn", conditionColumn, DbType.String);
                p.Add("@Condition", condition, DbType.String);
                p.Add("@OrderByField", orderByField, DbType.String);

                List<T> output = connection.Query<T>("dbo.spGenericConditionalGetAll", p,
                    commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }
        public int FE_Add(FE_Model model)
        {
            object success = new object();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@ID", model.ID, DbType.Int32, ParameterDirection.Output);
                p.Add("@FirstName", model.FirstName, DbType.String);
                p.Add("@LastName", model.LastName, DbType.String);
                p.Add("@ManagerID", model.ManagerID, DbType.String);
                p.Add("@Region", model.Region, DbType.String);
                p.Add("@Phone", model.Phone, DbType.String);
                p.Add("@EMail", model.EMail, DbType.String);
                p.Add("@Active", model.Active, DbType.Boolean);

                int rtn = -1;
                try
                {
                    success = connection.ExecuteScalar("dbo.spFE_Add", p,
                                commandType: CommandType.StoredProcedure);
                    rtn = Convert.ToInt32(success);
                }
                catch (Exception)
                {
                    rtn = -1;
                }
                return rtn;
            }
        }
        public List<T> GetItemByColumn<T>(string tableName, string columnName, string stringValue, int intValue = -1)
        {
            List<T> list = new List<T>();
            string iVal = intValue.ToString();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@TableName", tableName, DbType.String, ParameterDirection.Input);
                p.Add("@ColumnName", columnName, DbType.String, ParameterDirection.Input);
                p.Add("@IntValue", iVal, DbType.String, ParameterDirection.Input);
                p.Add("@StringValue", stringValue, DbType.String, ParameterDirection.Input);

                list = connection.Query<T>("dbo.spGetItemByColumn", p,
                    commandType: CommandType.StoredProcedure).ToList();
                return list;
            }
        }
        public int MSO_Add(string MSO_Name, string TLA, bool Active, int Tier)
        {
            object id = new object();
            int dbID = -1;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@MSO_Name", MSO_Name, DbType.String);
                p.Add("@TLA", TLA, DbType.String);
                p.Add("@Active", Active, DbType.Boolean);
                p.Add("@Tier", Tier, DbType.Int16);
                try
                {
                    id = connection.ExecuteScalar("dbo.spMSO_Add", p, commandType: CommandType.StoredProcedure);
                    dbID = Convert.ToInt32(id);
                }
                catch (Exception)
                {
                    dbID = -1;
                }

                return dbID;
            }
        }
        public List<T> GenericGetAll<T>(string tableName, string orderByField = "")
        {
            var p = new DynamicParameters();
            p.Add("@TableName", tableName, DbType.String);
            if ( orderByField != "")
            {
                p.Add("@OrderField", orderByField, DbType.String);
            }
            List<T> list = new List<T>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                list = connection.Query<T>("dbo.spGenericOrderedGetAll", p,
                    commandType: CommandType.StoredProcedure).ToList();
                return list;
            }
        }
        public static string db { get; set; }
        public List<T> GenericGetAllByField<T>(string tableName, string fieldName)
        {
            var p = new DynamicParameters();
            p.Add("@TableName", tableName, DbType.String);
            p.Add("@FieldName", fieldName, DbType.String);
            List<T> list = new List<T>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                list = connection.Query<T>("dbo.spGenericGetAllByField", p,
                    commandType: CommandType.StoredProcedure).ToList();
                return list;
            }
        }

        public void AddUser(UserModel NewUser)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
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
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@Ident", OldUser);
                connection.Execute("dbo.spUser_Delete", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<UserModel> GetUsers_All()
        {
            List<UserModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                output = connection.Query<UserModel>("dbo.spUsers_GetAll").ToList();

                foreach (UserModel User in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@UserName", User.UserName);
                    p.Add("@PW", User.PW);
                    p.Add("@Priviledge", User.Priviledge);
                    p.Add("@ActiveDesigner", User.ActiveDesigner);
                    p.Add("@ID", User.ID, DbType.Int32);

                }
            }

            return output;

        }


        public void UpdateUser(UserModel ThisUser)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
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

            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
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

            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
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
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<CompanyHolidaysModel> output = connection.Query<CompanyHolidaysModel>("dbo.spHolidays_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public void AddCountry(string ctry)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@cAdd", ctry, DbType.String);
                connection.Execute("dbo.spCountries_Add", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteCountry(int idx)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@Idx", idx, DbType.Int32);
                connection.Execute("dbo.spCountry_Delete", p, commandType: CommandType.StoredProcedure);
            }

        }

        public void UpdateCountry(int idx, string country)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@Idx", idx, DbType.Int32);
                p.Add("@country", country, DbType.String);
                connection.Execute("dbo.spCountry_Update", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<CountriesModel> GetAllCountries()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<CountriesModel> output = connection.Query<CountriesModel>("dbo.spCountries_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public void AddDesigner(DesignersReviewersModel designer,string tableName)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@TableName", tableName, DbType.String);
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
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
                {
                    var p = new DynamicParameters();
                    p.Add("@Ident", designer.ID);
                    connection.Execute("dbo.spDesigner_Delete", p, commandType: CommandType.StoredProcedure);
                }
            }
        }

        public void UpdateDesigner(DesignersReviewersModel designer, string tableName)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {

                var p = new DynamicParameters();
                p.Add("@TableName", tableName, DbType.String);
                p.Add("@UName", designer.Designer);
                p.Add("@PWord", designer.Pwd);
                p.Add("@Priv", designer.Priviledge);
                p.Add("@ActiveDesigner", designer.ActiveDesigner);
                p.Add("@ActiveReviewer", designer.ActiveReviewer);
                p.Add("@Ident", designer.ID);


                connection.Execute("dbo.spDesigner_Update", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<DesignersReviewersModel> GetAllDesigners()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<DesignersReviewersModel> output = connection.Query<DesignersReviewersModel>("dbo.spDesigners_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<DesignersReviewersModel> Reviewers_GetActive()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<DesignersReviewersModel> output = connection.Query<DesignersReviewersModel>("dbo.spReviewers_GetActive", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<DesignersReviewersModel> Reviewers_GetAll()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<DesignersReviewersModel> output = connection.Query<DesignersReviewersModel>("dbo.spReviewers_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<MSO_Model> GetAllMSO()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<MSO_Model> output = connection.Query<MSO_Model>("dbo.spMSO_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }


        public List<MSO_Model> GetAllActiveMSO()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<MSO_Model> output = connection.Query<MSO_Model>("dbo.spMSO_GetAllActive", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<CityModel> GetAllCities()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<CityModel> output = connection.Query<CityModel>("dbo.spCities_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<StateModel> GetAllStates()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<StateModel> output = connection.Query<StateModel>("dbo.spStates_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<RegionsModel> GetAllRegions()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<RegionsModel> output = connection.Query<RegionsModel>("dbo.spRegions_GetAll", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<SalespersonModel> SalesGetActive()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<SalespersonModel> output = connection.Query<SalespersonModel>("dbo.spSalespersons_GetActive", commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<DesignersReviewersModel> DesignersGetActive()
        {
            List<DesignersReviewersModel> output = new List<DesignersReviewersModel>();
            try
            {
                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
                {
                    output = connection.Query<DesignersReviewersModel>("dbo.spDesigners_GetActive", commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                System.Windows.Forms.MessageBox.Show("Cannot connect to server. Please check network and VPN status and try again");
            }
            return output;
        }

        public int GetSequence()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<SequenceModel> output = connection.Query<SequenceModel>("dbo.spSequence_Get", commandType: CommandType.StoredProcedure).ToList();
                return output[0].Sequence;
            }
        }

        public void SetSequence(int seq)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@Seq", seq, DbType.Int32);
                connection.Execute("dbo.spSequence_Update", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<RequestModel> GetRequestByPID(string PID)
        {
            RequestModel myUser = new RequestModel();

            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@PID", PID, DbType.String, ParameterDirection.Input);
                List<RequestModel> output = connection.Query<RequestModel>("dbo.spRequest_GetByPID", p, commandType: CommandType.StoredProcedure).ToList();

                return output;
            }
        }

        public bool RequestUpdate(RequestModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                DynamicParameters p = makParams(model);
                object rtnObj = new object();
                bool updated = false;
                try
                {
                    rtnObj = connection.ExecuteScalar("[dbo].[spRequest_UpdateByPID]", p, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    updated = false;
                }
                string rtnString = rtnObj.ToString();
                if (rtnString == "True")
                {
                    updated = true;
                }
                return updated;
            }
        }

        public int RequestInsert(RequestModel model)
        {
            int id = -1;
            object rtnObj = new object();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                DynamicParameters p = makParams(model);
                try
                {
                    rtnObj = connection.ExecuteScalar("[dbo].[spRequest_Insert]", p, commandType: CommandType.StoredProcedure);
                    id = Convert.ToInt32(rtnObj);
                }
                catch (Exception)
                {
                    id = -1;
                }
            }
            return id;
        }

        public List<RequestModel> GetRequestsForDesignerUpdate(string designer)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
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
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
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
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
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
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
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

                using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
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

        public List<RequestModel> DateRangeSearch_Unfiltered(DateTime StartDate, 
            DateTime EndDate, string SearchTerm, bool pendingOnly, string mso = null, string designer = null, string requestor = null)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@StartDate", StartDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@EndDate", EndDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@PendingOnly", pendingOnly, DbType.Boolean, ParameterDirection.Input);
                p.Add("@MSO", mso, DbType.String, ParameterDirection.Input);
                p.Add("@SearchTerm", SearchTerm, DbType.String, ParameterDirection.Input);
                p.Add("@Designer", designer, DbType.String, ParameterDirection.Input);
                p.Add("@Requestor", requestor, DbType.String, ParameterDirection.Input);
                List<RequestModel> output = null;


                output = connection.Query<RequestModel>("spRequests_DateRangeSearch_Dynamic",
                    p, commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<RequestModel> DateRangeSearch_Unfiltered(DateTime StartDate, DateTime EndDate)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@StartDate", StartDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@EndDate", EndDate, DbType.DateTime, ParameterDirection.Input);
                List<RequestModel> output = null;


                output = connection.Query<RequestModel>("spRequests_DateRangeSearch_Unfiltered_DateAssigned",
                    p, commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<RequestModelReport> ReportDateRangeSearch_Unfiltered_Pending_HasRevision(DateTime StartDate, DateTime EndDate,
            string SearchTerm, string mso, string designer = null)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@StartDate", StartDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@EndDate", EndDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("SearchTerm", SearchTerm, DbType.String, ParameterDirection.Input);
                p.Add(@"PendingOnly",false,DbType.Boolean)
;               p.Add("@MSO", mso, DbType.String, ParameterDirection.Input);
                p.Add("Designer", designer, DbType.String, ParameterDirection.Input);
                List<RequestModelReport> output = null;


                output = connection.Query<RequestModelReport>("spRequests_DateRangeSearch_Dynamic",
                    p, commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }
        public List<RequestModelReport> ReportDateRangeSearch_Unfiltered(DateTime StartDate, DateTime EndDate, 
            string SearchTerm, string mso, string designer = null)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@StartDate", StartDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@EndDate", EndDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@SearchTerm", SearchTerm, DbType.String, ParameterDirection.Input);
                p.Add("@PendingOnly", false, DbType.Boolean, ParameterDirection.Input);
                p.Add("@MSO", mso, DbType.String, ParameterDirection.Input);
                p.Add("Designer", designer, DbType.String, ParameterDirection.Input);
                List<RequestModelReport> output = null;


                output = connection.Query<RequestModelReport>("spRequests_DateRangeSearch_Dynamic",
                    p, commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<RequestModel> DateRangeSearch_MSOFiltered(DateTime StartDate, DateTime EndDate, 
            string SearchTerm, string mso, bool pendingOnly, string designer, string requestor)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@StartDate", StartDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@EndDate", EndDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@SearchTerm", SearchTerm, DbType.String, ParameterDirection.Input);
                p.Add("@PendingOnly", pendingOnly, DbType.Boolean, ParameterDirection.Input);
                p.Add("@MSO", mso, DbType.String, ParameterDirection.Input);
                p.Add("@Designer", designer, DbType.String, ParameterDirection.Input);
                p.Add("@Requestor", requestor, DbType.String, ParameterDirection.Input);
                List<RequestModel> output = null;

                output = connection.Query<RequestModel>("spRequests_DateRangeSearch_Dynamic",
                    p, commandType: CommandType.StoredProcedure).ToList();
        
                return output;
            }
        }


        public List<RequestModel> ReportDateRangeSearch_MSOFiltered(DateTime StartDate, DateTime EndDate, 
            string SearchTerm, string mso, bool pendingOnly, string designer, string requestor)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@StartDate", StartDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@EndDate", EndDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@SearchTerm", SearchTerm, DbType.String, ParameterDirection.Input);
                p.Add("@PendingOnly", pendingOnly, DbType.Boolean, ParameterDirection.Input);
                p.Add("@MSO", mso, DbType.String, ParameterDirection.Input);
                p.Add("@Designer", designer, DbType.String, ParameterDirection.Input);
                p.Add("@Requestor", requestor, DbType.String, ParameterDirection.Input);
                List<RequestModel> output = null;

                output = connection.Query<RequestModel>("spRequests_DateRangeSearch_Dynamic",
                            p, commandType: CommandType.StoredProcedure).ToList();
        
                return output;
            }
        }

        List<DesignersReviewersModel> IDataConnection.GetDesigner(string designerName)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@Designer", designerName, DbType.String, ParameterDirection.Input);

                List<DesignersReviewersModel> output = connection.Query<DesignersReviewersModel>("spDesigner_GetByName",
                    p, commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public List<RequestModelReport> GetSnapshotData(string MSO, DateTime start, DateTime end)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();


                p.Add("@MSO", MSO, DbType.String, ParameterDirection.Input);
                p.Add("@StartDate", start, DbType.DateTime, ParameterDirection.Input);
                p.Add("@EndDate", end, DbType.DateTime, ParameterDirection.Input);
                List<RequestModelReport> output = connection.Query<RequestModelReport>("spRequests_DateRangeSearch_MSOFiltered_DateAssigned",
                    p, commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        public void UpdateSnapshotMSO_s(string mso)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnString(db)))
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
                p.Add("@ID", model.Id, DbType.Int32, ParameterDirection.Output);
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

        public void LogEntry_Add(string User, string Action, string AffectedFields, string RequestID)

        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {

                var p = new DynamicParameters();
                p.Add("@User", User, DbType.String);
                p.Add("@Action", Action, DbType.String);
                p.Add("@AffectedFields", AffectedFields, DbType.String);
                p.Add("@RequestID", RequestID, DbType.String);


                connection.Execute("dbo.spActivityLog_Add", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<LogModel> LogList(string searchTerm, string searchValue)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@Searchterm", searchTerm, DbType.String, ParameterDirection.Input);
                p.Add("@SearchValue", searchValue, DbType.String, ParameterDirection.Input);
                List<LogModel> output =
                    connection.Query<LogModel>("dbo.spActivityLog_Search", p, commandType: CommandType.StoredProcedure).ToList();

                return output;
            }
        }

        public List<LogModel> ActivityLog_GetAll()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                List<LogModel> output =
                    connection.Query<LogModel>("dbo.spActivityLog_GetAll", commandType: CommandType.StoredProcedure).ToList();

                return output;
            }
        }

        public bool GetCurrentActivityStatus(string tableName, string activeColumnName, int Idx, string idxName)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                bool active = false;
                var p = new DynamicParameters();

                p.Add("@TableName", tableName, DbType.String, ParameterDirection.Input);
                p.Add("@ActiveColumnName", activeColumnName, DbType.String, ParameterDirection.Input);
                p.Add("@Index", Idx, DbType.Int32, ParameterDirection.Input);
                p.Add("@IndexName", idxName, DbType.String, ParameterDirection.Input);
                List<bool> output =
                    connection.Query<bool>("dbo.spGetActiveStatus",p , commandType: CommandType.StoredProcedure).ToList();
                active = output[0];
                return active;
            }
        }

        public void ToggleActiveStatus(string tableName, string activeColumnName, int Idx, string idxName)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnString(db)))
            {
                //bool active = false;
                var p = new DynamicParameters();

                p.Add("@TableName", tableName, DbType.String, ParameterDirection.Input);
                p.Add("@ActiveColumnName", activeColumnName, DbType.String, ParameterDirection.Input);
                p.Add("@ID", Idx, DbType.Int32, ParameterDirection.Input);
                p.Add("@ID_ColName", idxName, DbType.String, ParameterDirection.Input);

                //List<bool> output =
                connection.Execute("dbo.spToggleActiveStatus", p, commandType: CommandType.StoredProcedure);
                //active = output[0];
                //return active;
            }
        }
    }
}
