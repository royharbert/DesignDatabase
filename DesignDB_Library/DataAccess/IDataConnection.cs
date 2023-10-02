using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.DataAccess
{
    public interface IDataConnection
    {
        /// <summary>
        /// Returns List<BOMLineModel> that includes PID, BOM file name and DateCompleted
        /// </summary>
        /// <param name="PIDs"></param>
        /// <returns></returns>
        List<BOMLineModel> getBOMList(string PIDs);
        List<RollupRequestModel> GetRollupRequests(DateTime startDate, DateTime endDate);
        List<MSO_Model> MSO_GetByTier(int tier);
        void MSO_Update(MSO_Model model);
        List<RequestModel> DateRangeSearch_Unfiltered(DateTime StartDate, DateTime EndDate);
        List<RequestModel> GetRequestsDeleted();
        List<RequestModelReport> ReportDateRangeSearch_Unfiltered_Pending_HasRevision(DateTime StartDate, DateTime EndDate,
            string SearchTerm, string mso, string designer = null);
        List<T> GenericConditionalGetAll<T>(string tableName, string conditionColumn, string condition, string orderByField = "");
        void FE_CRUD(FE_Model model, char action);
        List<T> GetItemByColumn<T>(string tableName, string columnName, string stringValue, int intValue = -1);
        void MSO_Add(string MSO_Name, string TLA, bool Active, int tier);
        void ToggleActiveStatus(string tableName, string activeColumnName, int Idx, string idxName);
        bool GetCurrentActivityStatus(string tableName, string activeColumnName, int Idx, string idxName);
        List<T> GenericGetAll<T>(string tableName, string OorderByField = "");
        List<T> GenericGetAllByField<T>(string tableName, string fieldName);
        List<LogModel> ActivityLog_GetAll();
        List<LogModel> LogList(string searchTerm, string searchValue);
        void LogEntry_Add(string User, string Action, string AffectedFields, string RequestID);
        List<DesignersReviewersModel> Reviewers_GetActive();
        List<DesignersReviewersModel> Reviewers_GetAll();
        List<SalespersonModel> SalespersonsGetAll();
        void SalespersonAdd(SalespersonModel model);
        void SalespersonDelete(SalespersonModel model);
        void SalespersonUpdate(SalespersonModel model);
        List<RequestModel> SearchMultipleFields(string whereClause);
        List<RequestModel> GetOverdueRequests(DateTime dueDate);
        List<RequestModel> GetOpenRequests();
        List<RequestModel> GetDeletedRecordByPID(string PID);
        void ClearTable(string tableName);
        void UpdateSnapshotMSO_s(string mso);
        List<string> GetSnapshotMSO_s();
        List<RequestModelReport> GetSnapshotData(string MSO, DateTime start, DateTime end);
        List<DesignersReviewersModel> GetDesigner(string designerName);
        List<RequestModel> DateRangeSearch_MSOFiltered(DateTime StartDate, DateTime EndDate, string SearchTerm, 
            string mso, bool pendingOnly, string designer, string requestor);
        List<RequestModel> DateRangeSearch_Unfiltered(DateTime StartDate, DateTime EndDate, string SearchTerm, 
            bool pendingOnly, string mso = null, string designer =null, string requestor = null);
        List<RequestModel> ReportDateRangeSearch_MSOFiltered(DateTime StartDate, DateTime EndDate, 
            string SearchTerm, string mso, bool pendingOnly, string designer = null, string requestor = null); 
        List<RequestModelReport> ReportDateRangeSearch_Unfiltered(DateTime StartDate, DateTime EndDate, 
            string SearchTerm, string mso, string designer = null);
        List<DesignerLoadModel> DoLoadReport();
        void DeleteAttachment(AttachmentModel model);
        List<AttachmentModel> GetAttachments(string PID);
        void InsertInto_tblAttachments(AttachmentModel model);
        List<RequestModel> GetRequestsForDesignerUpdate(string designer);
        int GetSequence();
        List<RequestModel> GetRequestByPID(string PID);
        int RequestUpdate(RequestModel model);
        int RequestInsert(RequestModel model);
        void SetSequence(int seq);
        List<DesignersReviewersModel> DesignersGetActive();
        List<SalespersonModel> SalesGetActive();
        List<RegionsModel> GetAllRegions();
        List<StateModel> GetAllStates();
        void AddUser(UserModel NewUser);
        void DeleteUser(int OldUser);
        void UpdateUser(UserModel ThisUser);        
        UserModel GetUser(string userName);
        List<UserModel> GetUsers_All();
        List<CompanyHolidaysModel> GetAllHolidays();
        List<CountriesModel> GetAllCountries();
        void AddCountry(String ctry);
        void DeleteCountry(int idy);
        void UpdateCountry(int idx, string designer);
        void AddDesigner(DesignersReviewersModel designer, string tableName);
        void DeleteDesigner(DesignersReviewersModel designer);
        void UpdateDesigner(DesignersReviewersModel designer, string tableName);
        List<DesignersReviewersModel> GetAllDesigners();
        List<MSO_Model> GetAllMSO();
        List<MSO_Model> GetAllActiveMSO();
        List<CityModel> GetAllCities();
    }
}
