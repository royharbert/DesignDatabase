using DesignDB_Library.Models;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace DesignDB_Library.Operations
{
    
    public static class ReportOps
    {
        public static void DoRollup(DateTime startDate, DateTime endDate, List<MSO_Model> msoModels = null)
        {
            startDate = startDate.Date;
            endDate = endDate.Date;
            List<Report_SalesProjectValuesModel> projectRollup = new List<Report_SalesProjectValuesModel>();
            List<ReportCategoryMSOModel> categoryReport = new List<ReportCategoryMSOModel>();
            List<OpenRequestsBySalesModel> openRequestsBySales = new List<OpenRequestsBySalesModel>();
            int curYear = startDate.Year;
            DateTime NewYearsDay = new DateTime(curYear,1,1);
            DateTime NewYearsEve = new DateTime(curYear, 12, 31);
            //Get all requests YTD
            List<RequestModel> requests = null;
            if (msoModels == null)
            {
                //requests = GlobalConfig.Connection.DateRangeSearch_Unfiltered(NewYearsDay, NewYearsEve, "DateAssigned", false, "");
                requests = GlobalConfig.Connection.DateRangeSearch_Unfiltered(NewYearsDay, endDate, "DateAssigned", false, "");

            }
            else
            {
                //requests = GlobalConfig.Connection.DateRangeSearch_MSOFiltered(NewYearsDay, NewYearsEve, "DateAssigned", msoModels[0].MSO, false);
                requests = GlobalConfig.Connection.DateRangeSearch_MSOFiltered(NewYearsDay, endDate, "DateAssigned", msoModels[0].MSO, false);

            }

            List<SalespersonModel> salespersons = GlobalConfig.Connection.GenericGetAll<SalespersonModel>("tblSalespersons");
            List<MSO_Model> msoList = GlobalConfig.Connection.GenericGetAll<MSO_Model>("tblMSO", "MSO");            
            
            Report_SalesProjectValuesModel accumulator = new Report_SalesProjectValuesModel();
            accumulator.SalesPerson = "Total";

            accumulator.CurrentYTD_Value = requests.Where(x => x.AwardStatus != "Has Revision" && x.AwardStatus != "Canceled").Sum(x => x.BOM_Value);
            //accumulator.CurrentYTD_Value = requests.Sum(x => x.BOM_Value);
            foreach (SalespersonModel salespersonModel in salespersons)
            {
                string name = salespersonModel.SalesPerson;
                List<RequestModel> salesRequests = requests;
                salesRequests = salesRequests.Where(x => x.DesignRequestor == name).ToList();
                if (salesRequests.Count > 0)
                {
                    Report_SalesProjectValuesModel model = new Report_SalesProjectValuesModel();
                    //model.CurrentYTD_Value = salesRequests.Where(x => x.AwardStatus != "Has Revision").Sum(x => x.BOM_Value);
                    model.CurrentYTD_Value = salesRequests.Where(x => x.AwardStatus != "Has Revisioon" && x.AwardStatus != "Canceled") .Sum(x => x.BOM_Value);
                    accumulator.CurrentYear_Count = requests.Count;
                    model.SalesPerson = name;
                    foreach (var request in salesRequests)
                    {
                        if (request.DateAssigned.Date >= (startDate) && request.DateAssigned.Date <= endDate)
                        {
                            model.Weekly++;
                            accumulator.Weekly++;
                        }
                        model.CurrentYear_Count=salesRequests.Count;
                        //accumulator.CurrentYear_Count++;
                        
                        model.AverageDollars = model.CurrentYTD_Value / model.CurrentYear_Count;
                        //model.PctTotalValue = model.CurrentYTD_Value / accumulator.CurrentYTD_Value;
                        int month = request.DateAssigned.Month;
                        List<RequestModel> monthlyRequests = salesRequests.Where(x => x.DateAssigned.Month == month).ToList();
                        switch (month)
                        {
                            case 1:
                                model.JanProjects++;
                                accumulator.JanProjects++;
                                break;
                            case 2:
                                model.FebProjects++;
                                accumulator.FebProjects++;
                                break;
                            case 3:
                                model.MarProjects++;
                                accumulator.MarProjects++;
                                break;
                            case 4:
                                model.AprProjects++;
                                accumulator.AprProjects++;
                                break;
                            case 5:
                                model.MayProjects++;
                                accumulator.MayProjects++;
                                break;
                            case 6:
                                model.JunProjects++;
                                accumulator.JunProjects++;
                                break;
                            case 7:
                                model.JulProjects++;
                                accumulator.JulProjects++;
                                break;
                            case 8:
                                model.AugProjects++;
                                accumulator.AugProjects++;
                                break;
                            case 9:
                                model.SepProjects++;
                                accumulator.SepProjects++;
                                break;
                            case 10:
                                model.OctProjects++;
                                accumulator.OctProjects++;
                                break;
                            case 11:
                                model.NovProjects++;
                                accumulator.NovProjects++;
                                break;
                            case 12:
                                model.DecProjects++;
                                accumulator.DecProjects++;
                                break;
                            default:
                                    break;
                        }
                        model.Total++;
                        accumulator.Total++;
                    }
                    model.PctTotalValue = model.CurrentYTD_Value / accumulator.CurrentYTD_Value;
                    //accumulator.PctTotalValue = accumulator.PctTotalValue + model.PctTotalValue;
                    accumulator.PctTotalValue = 1;
                    projectRollup.Add(model);
                }
            }
            accumulator.AverageDollars = accumulator.CurrentYTD_Value / accumulator.CurrentYear_Count;
            //accumulator.PctTotalValue = 1;
            projectRollup.Add(accumulator);





            ReportCategoryMSOModel categorySummary = new ReportCategoryMSOModel();
            //categorySummary.TotalDollars = requests.Sum(x => x.BOM_Value);
            categorySummary.TotalDollars = requests.Where(x => x.AwardStatus != "Has Revision").Sum(x => x.BOM_Value);
            //Category report
            foreach (var mso in msoList)
            {
                List<RequestModel> categoryRequests = requests;//.Where(x => x.AwardStatus == "Pending").ToList();
                categoryRequests = categoryRequests.Where(x => x.MSO == mso.MSO).ToList();
                if (categoryRequests.Count > 0)
                {
                    ReportCategoryMSOModel model = new ReportCategoryMSOModel();
                    model.MSO = mso.MSO;
                    model.TotalDollars = categoryRequests.Where(x => x.AwardStatus != "Has Revision").Sum(x => x.BOM_Value);
                    model.TotalRequests = categoryRequests.Count;
                    model.AverageDollarsPerRequest = model.TotalDollars / model.TotalRequests;

                    //may need to filter has revision
                    model.PctOfTotal = model.TotalDollars * 100/requests.Where(x => x.AwardStatus != "Has Revision").Sum(x => x.BOM_Value)/100;
                    foreach (var request in categoryRequests)
                    {
                        switch (request.Category)
                        {
                            case "HFC":
                                model.HFC++;
                                if (request.AwardStatus != "Has Revision" && request.AwardStatus != "Canceled")
                                {
                                    model.HFCDollars = model.HFCDollars + request.BOM_Value; 
                                }
                                break;
                            case "Node Split":
                                model.NodeSplit++;
                                if (request.AwardStatus != "Has Revision" && request.AwardStatus != "Canceled")
                                {
                                    model.NodeSplitDollars = model.NodeSplitDollars + request.BOM_Value;
                                }
                                break;
                            case "RFoG":
                                model.RFoG++;
                                if (request.AwardStatus != "Has Revision" && request.AwardStatus != "Canceled")
                                {
                                    model.RFoGDollars = model.RFoGDollars + request.BOM_Value;
                                }
                                break;
                            case "PON":
                                model.PON++;
                                if (request.AwardStatus != "Has Revision" && request.AwardStatus != "Canceled")
                                {
                                    model.PON_Dollars = model.PON_Dollars + request.BOM_Value;
                                }
                                break;
                            case "RFoG-PON":
                                model.RFoGPON++;
                                if (request.AwardStatus != "Has Revision" && request.AwardStatus != "Canceled")
                                {
                                    model.RFoGPON_Dollars = model.RFoGPON_Dollars + request.BOM_Value;
                                }
                                break;
                            case "Fiber Deep":
                                model.FiberDeep++;
                                if (request.AwardStatus != "Has Revision" && request.AwardStatus != "Canceled")
                                {
                                    model.FiberDeepDollars = model.FiberDeepDollars + request.BOM_Value;
                                }
                                break;
                            case "Data Transport":
                                model.DataTrans++;
                                if (request.AwardStatus != "Has Revision" && request.AwardStatus != "Canceled")
                                {
                                    model.DataTransportDollars = model.DataTransportDollars + request.BOM_Value;
                                }
                                break;
                            case "Other":
                                model.Other++;
                                if (request.AwardStatus != "Has Revision" && request.AwardStatus != "Canceled")
                                {
                                    model.OtherDollars = model.OtherDollars + request.BOM_Value;
                                }
                                break;
                            case "PEG":
                                model.PEG++;
                                if (request.AwardStatus != "Has Revision" && request.AwardStatus != "Canceled")
                                {
                                    model.PEG_Dollars = model.PEG_Dollars + request.BOM_Value;
                                }
                                break;
                            case "Commercial":
                                model.Commercial++;
                                if (request.AwardStatus != "Has Revision" && request.AwardStatus != "Canceled")
                                {
                                    model.CommercialDollars = model.CommercialDollars + request.BOM_Value;
                                }
                                break;
                            case "Unassigned":
                            case "":
                                model.Unassigned++;
                                if (request.AwardStatus != "Has Revision" && request.AwardStatus != "Canceled")
                                {
                                    model.UnassignedDollars = model.UnassignedDollars + request.BOM_Value;
                                }
                                break;
                            default:
                                
                                break;
                        }
                    }
                    categoryReport.Add(model);
                }
            }
            categorySummary.MSO = "TOTAL";
            //List<RequestModel> totalRequests = requests.Where(x => x.AwardStatus != "Has Revision").ToList();
            categorySummary.TotalRequests = requests.Count;
            categorySummary.AverageDollarsPerRequest=categorySummary.TotalDollars/categorySummary.TotalRequests;
            categorySummary.PctOfTotal = categoryReport.Sum(x => x.PctOfTotal/100); 
            categorySummary.HFC= categoryReport.Sum(x => x.HFC);
            categorySummary.NodeSplit= categoryReport.Sum(x => x.NodeSplit);
            categorySummary.RFoG = categoryReport.Sum(x => x.RFoG);
            categorySummary.PON= categoryReport.Sum(x => x.PON);
            categorySummary.RFoGPON = categoryReport.Sum(x => x.RFoGPON);
            categorySummary.FiberDeep= categoryReport.Sum(x => x.FiberDeep);
            categorySummary.DataTrans = categoryReport.Sum(x => x.DataTrans);
            categorySummary.Other = categoryReport.Sum(x => x.Other);
            categorySummary.PEG = categoryReport.Sum(x => x.PEG);
            categorySummary.Commercial = categoryReport.Sum(x => x.Commercial);
            categorySummary.Unassigned = categoryReport.Sum(x => x.Unassigned);
            categorySummary.HFCDollars = categoryReport.Sum(x => x.HFCDollars);
            categorySummary.NodeSplitDollars = categoryReport.Sum(x => x.NodeSplitDollars);
            categorySummary.RFoGDollars = categoryReport.Sum(x => x.RFoGDollars);
            categorySummary.PON_Dollars = categoryReport.Sum(x => x.PON_Dollars);
            categorySummary.RFoGPON_Dollars = categoryReport.Sum(x => x.RFoGPON_Dollars);
            categorySummary.FiberDeepDollars = categoryReport.Sum(x => x.FiberDeepDollars);
            categorySummary.DataTransportDollars = categoryReport.Sum(x => x.DataTransportDollars);
            categorySummary.OtherDollars = categoryReport.Sum(x => x.OtherDollars);
            categorySummary.PEG_Dollars = categoryReport.Sum(x => x.OtherDollars);
            categorySummary.CommercialDollars = categoryReport.Sum(x => x.CommercialDollars);
            categorySummary.UnassignedDollars = categoryReport.Sum(x => x.UnassignedDollars);

            categoryReport.Add(categorySummary);

            //Open Requests by salesperson
            List<RequestModel> openDesignBySales = GlobalConfig.Connection.GetOpenRequests();
            if (msoModels != null)
            {
                openDesignBySales = openDesignBySales.Where(x => x.MSO == msoModels[0].MSO).ToList();
            }
            OpenRequestsBySalesModel accumulatorModel = new OpenRequestsBySalesModel();
            accumulatorModel.Salesperson = "Total";
            foreach (SalespersonModel salesperson in salespersons)
            {                
                List<RequestModel> openRequests = openDesignBySales;
                string person = salesperson.SalesPerson;
                OpenRequestsBySalesModel openModel = new OpenRequestsBySalesModel();
                openModel.Salesperson = person;
                openRequests = (List<RequestModel>)openRequests.Where(x => x.DesignRequestor == person).ToList();
                if (openRequests.Count > 0)
                {
                    foreach (var request in openRequests)
                    {
                        int mAssigned = request.DateAssigned.Month;
                        switch (mAssigned)
                        {
                            case 1:
                                openModel.Jan++;
                                accumulatorModel.Jan++;
                                break;
                            case 2:
                                openModel.Feb++;
                                accumulatorModel.Feb++;
                                break;
                            case 3:
                                openModel.Mar++;
                                accumulatorModel.Mar++;
                                break;
                            case 4:
                                openModel.Apr++;
                                accumulatorModel.Apr++;
                                break;
                            case 5:
                                openModel.May++;
                                accumulatorModel.May++;
                                break;
                            case 6:
                                openModel.Jun++;
                                accumulatorModel.Jun++;
                                break;
                            case 7:
                                openModel.Jul++;
                                accumulatorModel.Jul++;
                                break;
                            case 8:
                                openModel.Aug++;
                                accumulatorModel.Aug++;
                                break;
                            case 9:
                                openModel.Sep++;
                                accumulatorModel.Sep++;
                                break;
                            case 10:
                                openModel.Oct++;
                                accumulatorModel.Oct++;
                                break;
                            case 11:
                                openModel.Nov++;
                                accumulatorModel.Nov++;
                                break;
                            case 12:
                                openModel.Dec++;
                                accumulatorModel.Dec++;
                                break;
                            default:
                                break;
                        }
                        openModel.Count++;
                        accumulatorModel.Count++;
                    }                        
                    openRequestsBySales.Add(openModel);
                }
            }
            openRequestsBySales.Add(accumulatorModel);
            List<ReportSalesPriorityModel> priorityReport = ReportBySalesPriority(requests, salespersons);
            List<Report_SalesProjectValuesModel> msoSummary = MonthlyMSO_Summary(msoList, requests, startDate, endDate);
            ExcelOps.PlaceRollupInExcel(startDate, endDate, openRequestsBySales, categoryReport, projectRollup, priorityReport, accumulator.CurrentYTD_Value, msoSummary);
        }

        public static List<ReportSalesPriorityModel> ReportBySalesPriority(List<RequestModel> requests, List<SalespersonModel> salesPeople)
        {
            List<ReportSalesPriorityModel> priorityModels = new List<ReportSalesPriorityModel>();
            ReportSalesPriorityModel companyTotal = new ReportSalesPriorityModel();
            companyTotal.SalesPerson = "Company Total";
            ReportSalesPriorityModel percentModel = new ReportSalesPriorityModel();
            percentModel.SalesPerson = "Percent of Total";
            foreach (var person in salesPeople)
            {
                string name = person.SalesPerson;
                List<RequestModel> FilteredRequests = requests;
                FilteredRequests = FilteredRequests.Where(x => x.DesignRequestor == name).ToList();
                if (FilteredRequests.Count > 0)
                {
                    ReportSalesPriorityModel model = new ReportSalesPriorityModel();
                    model.SalesPerson = name;
                    foreach (var filteredRequest in FilteredRequests)
                    {
                        switch (filteredRequest.Pty)
                        {
                            case "P1":
                                model.P1Count++;
                                model.P1Dollars = model.P1Dollars + filteredRequest.BOM_Value;
                                model.TotalCount++;
                                companyTotal.P1Count++;
                                companyTotal.P1Dollars=companyTotal.P1Dollars = filteredRequest.BOM_Value;
                                break;
                            case "P2":
                                model.P2Count++;
                                model.P2Dollars = model.P2Dollars + filteredRequest.BOM_Value;
                                model.TotalCount++;
                                companyTotal.P2Count++;
                                companyTotal.P2Dollars = companyTotal.P2Dollars = filteredRequest.BOM_Value;
                                break;
                            case "P3":
                                model.P3Count++;
                                model.P3Dollars = model.P3Dollars + filteredRequest.BOM_Value;
                                model.TotalCount++;
                                companyTotal.P3Count++;
                                companyTotal.P3Dollars = companyTotal.P3Dollars = filteredRequest.BOM_Value;
                                break;
                            default:
                                break;
                        }
                        companyTotal.TotalCount++;
                    }
                    priorityModels.Add(model);
                }
            }
            priorityModels.Add(companyTotal);
            percentModel.TotalCount = companyTotal.TotalCount / companyTotal.TotalCount;
            percentModel.P1Count = companyTotal.P1Count / companyTotal.TotalCount;
            percentModel.P2Count = companyTotal.P2Count / companyTotal.TotalCount;
            percentModel.P3Count = companyTotal.P3Count / companyTotal.TotalCount;
            priorityModels.Add(percentModel);

            return priorityModels;
        }

        private static List<Report_SalesProjectValuesModel> MonthlyMSO_Summary(List<MSO_Model> msoList, List<RequestModel> AllRequests, DateTime startDate,
            DateTime endDate)
        {
            List<Report_SalesProjectValuesModel> result = new List<Report_SalesProjectValuesModel>();
            Report_SalesProjectValuesModel accumulatorModel = new Report_SalesProjectValuesModel();
            accumulatorModel.SalesPerson = "Total";
            accumulatorModel.CurrentYTD_Value = AllRequests.Where(x => x.AwardStatus != "Has Revision").Sum(x => x.BOM_Value);
            //accumulatorModel.CurrentYTD_Value = AllRequests.Sum(x => x.BOM_Value);
            accumulatorModel.CurrentYear_Count = AllRequests.Count;
            foreach (MSO_Model mso in msoList)
            {
                Report_SalesProjectValuesModel openModel = new Report_SalesProjectValuesModel();
                openModel.SalesPerson = mso.MSO;
                List<RequestModel> filteredRequests = AllRequests.Where(x => x.MSO == mso.MSO).ToList();
                if (filteredRequests.Count > 0)
                   
                {
                    openModel.CurrentYTD_Value = filteredRequests.Where(x => x.AwardStatus != "Has Revisioon" && x.AwardStatus != "Canceled").Sum (x => x.BOM_Value);
                    openModel.CurrentYear_Count = filteredRequests.Count;
                    foreach (RequestModel request in filteredRequests)
                    {
                        if (request.DateAssigned.Date >= (startDate) && request.DateAssigned.Date <= endDate)
                        {
                            openModel.Weekly++;
                            accumulatorModel.Weekly++;
                        }
                        openModel.CurrentYear_Count = filteredRequests.Count;
                        openModel.AverageDollars = openModel.CurrentYTD_Value / openModel.CurrentYear_Count;
                        //openModel.PctTotalValue = openModel.CurrentYTD_Value / accumulatorModel.CurrentYTD_Value;
                        int mAssigned = request.DateAssigned.Month;
                        switch (mAssigned)
                        {
                            case 1:
                                openModel.JanProjects++;
                                accumulatorModel.JanProjects++;
                                break;
                            case 2:
                                openModel.FebProjects++;
                                accumulatorModel.FebProjects++;
                                break;
                            case 3:
                                openModel.MarProjects++;
                                accumulatorModel.MarProjects++;
                                break;
                            case 4:
                                openModel.AprProjects++;
                                accumulatorModel.AprProjects++;
                                break;
                            case 5:
                                openModel.MayProjects++;
                                accumulatorModel.MayProjects++;
                                break;
                            case 6:
                                openModel.JunProjects++;
                                accumulatorModel.JunProjects++;
                                break;
                            case 7:
                                openModel.JulProjects++;
                                accumulatorModel.JulProjects++;
                                break;
                            case 8:
                                openModel.AugProjects++;
                                accumulatorModel.AugProjects++;
                                break;
                            case 9:
                                openModel.SepProjects++;
                                accumulatorModel.SepProjects++;
                                break;
                            case 10:
                                openModel.OctProjects++;
                                accumulatorModel.OctProjects++;
                                break;
                            case 11:
                                openModel.NovProjects++;
                                accumulatorModel.NovProjects++;
                                break;
                            case 12:
                                openModel.DecProjects++;
                                accumulatorModel.DecProjects++;
                                break;
                            default:
                                break;
                        }
                        openModel.Total++;
                        accumulatorModel.Total++;
                    }
                    openModel.PctTotalValue = openModel.CurrentYTD_Value/accumulatorModel.CurrentYTD_Value;
                    //accumulatorModel.PctTotalValue = accumulatorModel.PctTotalValue + openModel.PctTotalValue;
                    accumulatorModel.PctTotalValue = 1;
                    result.Add(openModel);
                }
            }
            accumulatorModel.AverageDollars = accumulatorModel.CurrentYTD_Value / accumulatorModel.CurrentYear_Count;
            result.Add(accumulatorModel);
            return result;
        }
        public static List<List<(string Field, bool Active)>> CollectDropDownLists(TableLayoutPanel BoxForm)
        {
            List<List<(string, bool)>> BoxData = new List<List<(string, bool)>>();

            List<string> items = new List<string>();
            foreach (Control ctl in BoxForm.Controls)
            {
                if (ctl is ComboBox)
                {
                    ComboBox cbo = (ComboBox)ctl;

                    //parse tag info
                    string cTag = ctl.Tag.ToString();
                    string[] tagArray = cTag.Split('|');
                    string field = tagArray[3];
                    //create list for drop-down items
                    List<(string Field, bool Active)> ddList = new List<(string, bool)>();
                    //place list description at head of list
                    ddList.Add((tagArray[4], true));
                    if (tagArray[2] == "")
                    {
                        //Source is internal list
                        foreach (var item in cbo.Items)
                        {
                            (string, bool) tup = (item.ToString(), true);
                            ddList.Add(tup);
                        }
                        BoxData.Add(ddList);
                    }
                    else
                    { 
                        if (tagArray[4] != "City")
                        {
                            //source is from database
                            switch (tagArray[4])
                            {
                                case "MSO":
                                    BoxData.Add(MakeTupleList<MSO_Model>("tblMSO", "MSO", "Active", "MSO"));
                                    break;

                                case "Salesperson":
                                    BoxData.Add(MakeTupleList<SalespersonModel>("tblSalespersons", "SalesPerson", "Active", "SalesPersons"));
                                    break;

                                case "Designer":
                                    BoxData.Add(MakeTupleList<DesignersReviewersModel>("tblReviewers", "Designer", "ActiveDesigner", "Designer"));
                                    break;

                                case "Assisted By":
                                    BoxData.Add(MakeTupleList<DesignersReviewersModel>("tblReviewers", "Designer", "ActiveDesigner", "Assisted By"));
                                    break;

                                case "Reviewed By":
                                    BoxData.Add(MakeTupleList<DesignersReviewersModel>("tblReviewers", "Designer", "ActiveReviewer", "Reviewers"));
                                    break;

                                case "State":
                                    BoxData.Add(MakeTupleList<StateModel>("tblStates", "State", "States"));
                                    break;

                                case "Country":
                                    BoxData.Add(MakeTupleList<CountriesModel>("tblCountries", "Country", "Countries"));
                                        break;

                                default:
                                    //BoxData.Add(MakeTupleList<MSO_Model>("tblDesigners", "Designer"));
                                    break;
                            }
                        }
                    }
                }                
            }
                
            return BoxData;
        }

        private static List<(string Field, bool Active)> MakeTupleList<T>(string tableName, string FieldName, string cName)
        {
            (string Field, bool Active) tup = (cName, true);
            List<(string Field, bool Active)> ddList = new List<(string Field, bool Active)>();
            ddList.Add(tup);
            //Type model = GetType(T);
            List<T> dataList = GlobalConfig.Connection.GenericGetAll<T>(tableName);
            foreach (T item in dataList)
            {
                Type modelType = item.GetType();
                //PropertyInfo[] props = modelType.GetProperties();
                PropertyInfo fieldInfo = modelType.GetProperty(FieldName);
                tup.Field = fieldInfo.GetValue(item, null).ToString();
                tup.Active = true;
                ddList.Add(tup);
            }
            return ddList;
        }

            private static List<(string Field, bool Active)> MakeTupleList<T>(string tableName,  string FieldName, string activeField, string cName)
        {
            (string Field, bool Active) tup = (cName, true);
            List<(string Field,bool Active)> ddList = new List<(string Field,bool Active)> ();
            ddList.Add(tup);
            //Type model = GetType(T);
            List <T> dataList = GlobalConfig.Connection.GenericGetAll<T>(tableName);
            foreach (T item in dataList)
            {
                Type modelType = item.GetType();
                //PropertyInfo[] props = modelType.GetProperties();
                PropertyInfo fieldInfo = modelType.GetProperty(FieldName);
                tup.Field = fieldInfo.GetValue(item, null).ToString();
                PropertyInfo modelActive = modelType.GetProperty(activeField);
                tup.Active = (bool)modelActive.GetValue(item, null);
                ddList.Add(tup);
            }
            return ddList;
        }
        public static List<CompletionTimeModel> GenerateCompletionTimeSummary
            (DateTime startDate, DateTime endDate, List<MSO_Model> msoList)
        {
            List<CompletionTimeModel> report = new List<CompletionTimeModel>();
            //get completed requests from the time frame
            foreach (MSO_Model mso in msoList)
            {
                List<RequestModel> requestList =
                    GlobalConfig.Connection.DateRangeSearch_MSOFiltered(startDate, endDate, "DateAssigned", 
                    mso.MSO, false);
                if (requestList.Count > 0)
                {
                    CompletionTimeModel model = new CompletionTimeModel();
                    model.MSO = mso.MSO;
                    List<RequestModel> completedList = requestList.Where
                        (x => x.DateCompleted != DateTime.MinValue).ToList();
                    List<RequestModel> openList = requestList.Where
                        (x => x.DateCompleted == DateTime.MinValue).ToList();
                    int totalOpenDays = 0;
                    float averageOpenDays = 0;
                    float averageDaysToComplete = 0;
                    int invalidCompletedRequests = 0;

                    //Accumulate days open
                    int totalDaysToComplete = 0;

                    //sort completed designs               
                    foreach (RequestModel completedRequest in completedList)
                    {
                        //do completed designs
                        int designDays = (completedRequest.DateCompleted - completedRequest.DateAssigned).Days;
                        if (designDays < 0)
                        {
                            invalidCompletedRequests++;
                        }
                        else
                        {
                            totalDaysToComplete = totalDaysToComplete + designDays;
                        }
                    }
                    if (completedList.Count > 0)
                    {
                        averageDaysToComplete = totalDaysToComplete / completedList.Count;
                    }
                    model.CompletedDesigns = completedList.Count;
                    model.TotalDaysToComplete = totalDaysToComplete;
                    model.AvgDaysToComplete = (averageDaysToComplete - invalidCompletedRequests).ToString("0.00");

                    //sort open designs
                    foreach (RequestModel openRequest in openList)
                    {
                        int openDays = (DateTime.Now - openRequest.DateAssigned).Days;
                        totalOpenDays = totalOpenDays + openDays;
                    }
                    if (openList.Count > 0)
                    {
                        averageOpenDays = totalOpenDays / openList.Count;
                    }

                    model.CompletedDesigns = completedList.Count;
                    model.TotalDaysToComplete = totalDaysToComplete;
                    model.AvgDaysToComplete = averageDaysToComplete.ToString("0.00");

                    model.OpenDesigns = openList.Count;
                    model.OpenDays = totalOpenDays;
                    model.AverageOpenDays = averageOpenDays.ToString("#0.00");

                    model.OpenDesigns = openList.Count;
                    model.OpenDays = totalOpenDays;
                    model.AverageOpenDays = averageOpenDays.ToString("#0.00");
                    List<RequestModel> canceledDesigns = requestList.Where(x => x.AwardStatus == "Canceled").ToList();
                    model.CanceledDesigns = canceledDesigns.Count;

                    report.Add(model);
                    model = null;
                }

            }
            return report;
        }
        public static List<ReportSalesPriorityModel> GenerateSalesSummary(DateTime startDate, DateTime endDate)
        {
            List<SalespersonModel> sales = GlobalConfig.Connection.SalesGetActive();
            List<ReportSalesPriorityModel> report = new List<ReportSalesPriorityModel>();

            //get list of requests in timeframe
            List<RequestModel> requests = GlobalConfig.Connection.DateRangeSearch_Unfiltered(startDate, endDate, 
                "DateCompleted", false, "");

            //loop thru sales team
            List<RequestModel> requestsP1 = null;
            List<RequestModel> requestsP2 = null;
            List<RequestModel> requestsP3 = null;
            List<RequestModel> sRequests = null;
            foreach (SalespersonModel salesperson in sales)
            {
                ReportSalesPriorityModel model = new ReportSalesPriorityModel();
                model.SalesPerson = salesperson.SalesPerson;

                //use linq to extract data
                sRequests = requests.Where(x => x.DesignRequestor == salesperson.SalesPerson).ToList();
                model.TotalCount = sRequests.Count;
                requestsP1 = sRequests.Where(x => x.Pty == "P1").ToList();
                requestsP2 = sRequests.Where(x => x.Pty == "P2").ToList();
                requestsP3 = sRequests.Where(x => x.Pty == "P3").ToList();

                model.P1Count = requestsP1.Count;
                model.P2Count = requestsP2.Count;
                model.P3Count = requestsP3.Count;

                model.P1Dollars = AccumulateDollars(requestsP1);
                model.P2Dollars = AccumulateDollars(requestsP2);
                model.P3Dollars = AccumulateDollars(requestsP3);

                model.TotalDollars = model.P1Dollars + model.P2Dollars + model.P3Dollars;
                //add line to report
                report.Add(model);
                model = null;
                sRequests = null;
            }

            return report;
        }

        public static void formatPriorityDGV(DataGridView dgv)
        {
            string[] headers = { "Salesperson", "P1 Count", "P2 Count", "P3 Count", "P1 Dollars",
                "P2 Dollars", "P3 Dollars", "Total Count","Total Dollars" };
            setDGV_HeaderText(dgv, headers);

            int[] widths = { 120, 80, 80, 80, 80, 80, 80, 80 };
            SetDGV_ColumnWidths(dgv, widths);
        }

        private static decimal AccumulateDollars(List<RequestModel> requests)
        {
            decimal sum = 0;
            foreach (RequestModel request in requests)
            {
                sum = sum + request.BOM_Value;
            }
            return sum;
        }

        public static List<SnapshotModel> GenerateSnapshotReport(List<MSO_Model> msoList, DateTime startDate, DateTime endDate)
        {
            List<SnapshotModel> snapshots = new List<SnapshotModel>();
            foreach (MSO_Model mso in msoList)
            {
                SnapshotModel snapshotModel = ReportOps.SnapshotLine
                    (mso.MSO, startDate, endDate);
                if (snapshotModel != null)
                {
                    snapshots.Add(snapshotModel);
                }
            }
            return snapshots;
        }

        private static SnapshotModel SnapshotLine(string mso, DateTime startDate, DateTime endDate)
        {
            SnapshotModel snap = new SnapshotModel();
            int year = startDate.Year;
            int month = startDate.Month;
            DateTime yearStart = DateTime.Parse("1/1/" + year.ToString());
            DateTime yearEnd = DateTime.Parse("12/31/" + year.ToString());
            List<RequestModelReport> snapshot = GlobalConfig.Connection.GetSnapshotData(mso, yearStart, endDate);
            snap.MSO = mso;

            snap.RequestsThisYear = snapshot.Count;
            if (snap.RequestsThisYear > 0)
            {
                List<RequestModelReport> CompletedDesignList = snapshot.Where(x => x.DateCompleted != DateTime.Parse("1/1/0001")).ToList();
                snap.TotalCompletedDesigns = CompletedDesignList.Count;

                //loop thru completed design list and sum the days to complete
                int totalDaysToComplete = 0;
                int requestDaysToComplete = 0;
                foreach (RequestModelReport request in CompletedDesignList)
                {
                    if (request.DateCompleted > DateTime.Parse("1/1/2000") && request.AwardStatus == "Pending")
                    {
                        TimeSpan timeSpan = request.DateCompleted - request.DateAssigned;
                        requestDaysToComplete = timeSpan.Days;
                        totalDaysToComplete += requestDaysToComplete;
                    }
                }
                snap.TotalDaysToComplete = totalDaysToComplete;

                if (snap.TotalCompletedDesigns > 0)
                {
                    snap.AverageCompletionTime = (totalDaysToComplete / snap.TotalCompletedDesigns).ToString("#.#");
                }
                else
                {
                    snap.AverageCompletionTime = "";
                }
                List<RequestModelReport> snapList = snapshot.Where(x => x.DateCompleted <= DateTime.Parse("1/1/2000")).ToList();
                snap.TotalOpenRequests = snapList.Where(x => x.AwardStatus == "Pending").ToList().Count;
                snap.TotalCanceledDesigns = snapshot.Where(x => x.AwardStatus == "Canceled").ToList().Count;

                snap.RequestsThisMonth = snapshot.Where(x => month == x.DateAssigned.Month).ToList().Count;
                snap.RequestsThisWeek = snapshot.Where(x => x.DateAssigned.Date >= startDate.Date && x.DateAssigned.Date <= startDate.AddDays(7)).ToList().Count;
                snap.TotalValue = snapshot.Sum(x => x.BOM_Value);
            }
            else
            {
                snap = null;
            }
            return snap;
        }

        public static void FormatSnapshotDGV(DataGridView dgv)
        {
            string[] headers = { "MSO", "Requests This Year", "Requests This Month", "Requests This Week", "Total Days to Complete",
            "Total Completed Designs", "Total Open Requests", "Total Canceled Designs", "Total Value", "Avg. Days to Complete"};
            setDGV_HeaderText(dgv, headers);
            

            int[] widths = { 140, 140, 140, 140, 140, 140, 140, 140, 140, 140 };
            SetDGV_ColumnWidths(dgv, widths);
        }

        public static void FormatLogViewDGV(DataGridView dgv)
        {
            string[] headers = { "Time Stamp", "Request ID", "User", "Action", "Affected Fields" };
            setDGV_HeaderText(dgv, headers);


            int[] widths = { 140, 200, 140, 140, 140 };
            SetDGV_ColumnWidths(dgv, widths);

            dgv.Columns[3].Visible = true;
            dgv.Columns[4].Visible = false;
        }

        public static void FormatDesignerLoadDGV(DataGridView dgv)
        {
            string[] headers = { "Designer", "Priority", "Date Due", "Project ID", "Award Status" };
            setDGV_HeaderText(dgv, headers);

            int[] widths = { 160, 60, 110, 180, 70 };
            SetDGV_ColumnWidths(dgv, widths);            
        }
        public static void FormatDesignerLoadExport(Excel.Worksheet wks)
        {
            string[] headers = { "Designer", "Priority", "Date Due", "Project ID", "Award Status" };
            placeHeaderTextInExport(wks, headers);
            formatExcelHeaderRow(wks);            

            int[] widths = { 25, 10, 15, 25,15 };
            setExcelExportColumnWidths(wks, widths);

            CenterAllExcelColumns(wks, widths);
        }

        public static void FormatSnapshotExport(Excel.Worksheet wks)
        {
            string[] headers = { "MSO", "Requests This Year", "Requests This Month", "Requests This Week", "Total Days to Complete",
            "Total Completed Designs", "Total Open Requests", "Total Canceled Designs", "Total Value", "Avg. Days to Complete"};
            placeHeaderTextInExport(wks, headers);
            formatExcelHeaderRow(wks);

            int[] widths = { 20, 12, 12, 12, 12, 12, 12, 12, 15, 12 };
            setExcelExportColumnWidths(wks, widths);

            string[] currencyCols = { "I" };
            FormatExcelColumnsAsCurrency(wks, currencyCols);

            int[] cols = { 2, 3, 4, 5, 6, 7, 8,  10 };
            CenterSpecificExcelColumns(wks, cols);
        }

        public static List<ReportCategoryMSOModel> reportCategoryMSOs(List<MSO_Model> msoList, DateTime startDate, DateTime endDate)
        {
            List<ReportCategoryMSOModel> reportList = new List<ReportCategoryMSOModel>();
            ReportCategoryMSOModel reportLine = null;
            foreach (MSO_Model mso in msoList)
            {
                List<RequestModel> requests = GlobalConfig.Connection.DateRangeSearch_MSOFiltered(startDate, endDate,
                    "DateAssigned", mso.MSO,false).ToList();

                if (requests.Count > 0)
                {
                    reportLine = new ReportCategoryMSOModel();
                    reportLine.TotalRequests = requests.Count;
                    foreach (RequestModel request in requests)
                    {
                        reportLine.MSO = request.MSO;
                        switch (request.Category)
                        {
                            case "HFC":
                                reportLine.HFC++;
                                reportLine.HFCDollars = reportLine.HFCDollars + request.BOM_Value;
                                break;
                            case "Node Split":
                                reportLine.NodeSplit++;
                                reportLine.NodeSplitDollars = reportLine.HFCDollars + request.BOM_Value;
                                break;
                            case "RFOG":
                                reportLine.RFoG++;
                                reportLine.RFoGDollars = reportLine.RFoGDollars + request.BOM_Value;
                                break;                            
                            case "PON":
                                reportLine.PON++;
                                reportLine.PON_Dollars = reportLine.PON_Dollars + request.BOM_Value;
                                break;
                            case "Fiber Deep":
                                reportLine.FiberDeep++;
                                reportLine.FiberDeepDollars = reportLine.FiberDeepDollars + request.BOM_Value;
                                break;
                            case "Data Transport":
                                reportLine.DataTrans++;
                                reportLine.DataTransportDollars = reportLine.DataTransportDollars + request.BOM_Value;
                                break;
                            case "PEG":
                                reportLine.PEG++;
                                reportLine.PEG_Dollars = reportLine.PEG_Dollars + request.BOM_Value;
                                break;
                            case "Commercial":
                                reportLine.Commercial++;
                                reportLine.CommercialDollars = reportLine.CommercialDollars + request.BOM_Value;
                                break;
                            case "Other":
                                reportLine.Other++;
                                reportLine.OtherDollars = reportLine.OtherDollars + request.BOM_Value;
                                break;
                            default:
                                reportLine.Unassigned++;
                                reportLine.UnassignedDollars = reportLine.UnassignedDollars + request.BOM_Value;
                                break;
                        }
                        reportLine.TotalDollars = reportLine.TotalDollars + request.BOM_Value;
                    }
                    if (reportLine.TotalRequests > 0)
                    {
                        reportLine.AverageDollarsPerRequest = reportLine.TotalDollars / reportLine.TotalRequests;
                    }
                    reportList.Add(reportLine);
                }
                else
                {
                    reportLine = null;
                }
            }

            return reportList;
        }

        public static void formatCompletionTimeExport(Excel.Worksheet wks)
        {
            //column widths
            int[] widths = { 20, 12, 12, 12, 12, 12, 12, 12 };
            setExcelExportColumnWidths(wks, widths);

            //Header Texts
            string[] headers = { "MSO", "Completed Designs", "Total Days to Complete", "Average Days to Complete",
                "Open Designs", "Total Days Open", "Average Days Open", "Canceled Designs" };
            placeHeaderTextInExport(wks, headers);

            formatExcelHeaderRow(wks);
        }
        public static void FormatCompletionTimeDGV(DataGridView dgv)
        {
            string[] headers = { "MSO", "Completed Designs","Total Days to Complete", "Average Days to Complete", "Open Designs",
            "Open Days", "Average Open Days", "Canceled Designs"};
            setDGV_HeaderText(dgv, headers);
        }
        public static void FormatRequestPriorityExport(Excel.Worksheet wks)
        {
            string[] headers = { "Sales Person", "P1 Count", "P1 Dollars", "P2 Count", "P2 Dollars", "P3 Count", "P3 Dollars",
                "Total Count", "Total Dollars" };
            placeHeaderTextInExport(wks, headers);
            formatExcelHeaderRow(wks);

            int[] widths = { 20, 12, 12, 12, 12, 12, 12, 12 };
            setExcelExportColumnWidths(wks, widths);

            string[] currencyRange = { "C", "E", "G", "I" };
            FormatExcelColumnsAsCurrency(wks, currencyRange);

            int[] cols = { 2, 4, 6, 8 };
            CenterSpecificExcelColumns(wks, cols);

        }

        private static void FormatExcelColumnsAsCurrency(Excel.Worksheet wks, string[] cols)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                string rangeString = cols[i] + ":" + cols[i];
                Excel.Range formatRange;
                formatRange = wks.get_Range(rangeString);
                formatRange.NumberFormat = "$###,###,###.00";
            }
        }

        private static void setExcelExportColumnWidths(Excel.Worksheet wks, int[] widths)
        {
            for (int i = 0; i < widths.Length; i++)
            {
                wks.Columns[i + 1].ColumnWidth = widths[i];
            }
        }

        public static void ReportFormatMultiResultExport(Excel.Worksheet wks)
        {
            string[] headers = new string[]
            {
                 "Project ID", "Quote Type", "Original Quote", "Priority", "Award Status", "Design Requestor",
                "BOM Value", "% Project Covered", "MSO", "Region", "City", "Date Assigned", "Date All Info Received", "Date Due",
                "Date Completed", "Date Last Update", "Designer", "Assisted By", "Reviewed By", "Category",
                "Architecture Details", "Total Hours", "Architecture Type", "End Customer", "State","Country", "Project Name"
            };
        {
                //"Project ID", "MSO", "End Customer", "City", "State","Country", "Region", "Design Requestor", "Quote Type",
                //"Priority", "Designer", "Project Name", "Original Quote", "Assisted By", "Category", "Architecture Type",
                //"Date Assigned", "Date All Info Received", "Date Due", "Award Status", "Date Last Update", "Reviewed By",
                //"Date Completed", "Total Hours", "BOM Value", "% Project Covered", "Architecture Details", "Comments"
        };
            placeHeaderTextInExport(wks, headers);
            formatExcelHeaderRow(wks);

            int[] widths = new int[]
            {
            //  A  B  C  D  E  F  G  H  I  J  K  L  M  N  O  P  Q  R  S  T  U  V  W  X  Y  Z  AA AB  
                26,15,26,15,17,26,17,12,22,15,25,15,15,15,15,15,22,22,22,12,22,15,22,15,20,20,50,26
            };

            string[] currencyCols = { "G" };
            FormatExcelColumnsAsCurrency(wks,currencyCols);
            setExcelExportColumnWidths(wks, widths);
            wks.get_Range("R:R").WrapText = true;
            wks.get_Range("Z:Z").WrapText = true;
            wks.get_Range("AA:AA").WrapText = false;
            wks.get_Range("L:L").WrapText = true;

            int[] cols = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 };
            CenterSpecificExcelColumns(wks, cols);
        }

        public static void FormatMultiResultExport(Excel.Worksheet wks)
        {
            string[] headers = new string[]
            {
                // "Project ID", "Quote Type", "Original Quote", "Priority", "Award Status", "Design Requestor",
                //"BOM Value", "% Project Covered", "MSO", "Region", "City", "Date Assigned", "Date All Info Received", "Date Due",
                //"Date Completed", "Date Last Update", "Designer", "Assisted By", "Reviewed By", "Category",
                //"Architecture Details", "Total Hours", "Architecture Type", "End Customer", "State","Country", "Project Name"
            
                "Project ID", "MSO", "End Customer", "City", "State","Country", "Region", "Design Requestor", "Quote Type",
                "Priority", "Designer", "Project Name", "Original Quote", "Assisted By", "Category", "Architecture Type",
                "Date Assigned", "Date All Info Received", "Date Due", "Award Status", "Date Last Update", "Reviewed By",
                "Date Completed", "Total Hours", "BOM Value", "% Project Covered", "Architecture Details", "Comments"
            };
            placeHeaderTextInExport(wks, headers);
            formatExcelHeaderRow(wks);

            int[] widths = new int[]
            {
            //  A  B  C  D  E  F  G  H  I  J  K  L  M  N  O  P  Q  R  S  T  U  V  W  X  Y  Z  AA AB  
                26,26,26,22,17,26,17,22,18,15,25,22,26,22,15,22,18,18,18,18,18,22,18,12,12,12,26,26
            };

            string[] currencyCols = { "Y" };
            FormatExcelColumnsAsCurrency(wks, currencyCols);
            setExcelExportColumnWidths(wks, widths);
            wks.get_Range("R:R").WrapText = true;
            wks.get_Range("Z:Z").WrapText = true;
            wks.get_Range("AA:AA").WrapText = false;
            wks.get_Range("L:L").WrapText = true;

            int[] cols = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 };
            CenterSpecificExcelColumns(wks, cols);
        }

        private static void placeHeaderTextInExport(Excel.Worksheet wks, string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                wks.Cells[1, i + 1].Value = headers[i];
            }
        }

        private static void formatExcelHeaderRow(Excel.Worksheet wks)
        {
            //Center text in header/Turn word wrap on/Bold Font
            wks.Range["1:1"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            wks.Range["1:1"].WrapText = true;
            wks.Range["1:1"].Font.Bold = true;
        }

        private static void CenterAllExcelColumns(Excel.Worksheet wks, int[] widths)
        {
            for (int i = 1; i < widths.Length +1; i++)
            {
                wks.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            }
        }

        private static void CenterSpecificExcelColumns(Excel.Worksheet wks, int[] cols)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                wks.Columns[cols[i]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            }
        }

        //format datagridview controls
        private static void SetDGV_ColumnWidths(DataGridView dgv, int[] widths)
        {
            for (int i = 0; i < widths.Length; i++)
            {
                dgv.Columns[i].Width = widths[i];
            }
        }
        public static void FormatMultiResultDGV(DataGridView dgv)
        {
            string[] headers = new string[]
            {
                "ID","Project ID", "MSO", "End Customer", "City", "State","Country", "Region", "Design Requestor", "Quote Type",
                "Priority", "Designer", "Project Name", "Original Quote", "Assisted By", "Category", "Architecture Type",
                "Date Assigned", "Date All Info Received", "Date Due", "Award Status", "Date Last Update", "Reviewed By",
                "Date Completed", "Total Hours", "BOM Value", "% Project Covered", "Architecture Details", "Comments"            
            };

            setDGV_HeaderText(dgv, headers);

            int[] widths = { 10,220,140,140,130,130,130,100,150,90,70,140,180,200,
                140,140,100,120,100,100,120,100,100,140,100,100,150,100 };
            SetDGV_ColumnWidths(dgv, widths);
            dgv.Columns[0].Visible = false;
            dgv.Columns[29].Visible = false;

            string[] currencyCols = { "BOM_Value" };
            setDGV_CellFormatToCurrency(dgv, currencyCols);
        }

        public static void ReportFormatMultiResultDGV(DataGridView dgv)
        {
            string[] headers = new string[]

            {
                "ID", "Project ID", "Quote Type", "Original Quote", "Priority", "Award Status", "Design Requestor",
                "BOM Value", "% Project Covered", "MSO", "Region", "City", "Date Assigned", "Date All Info Received", "Date Due",
                "Date Completed", "Date Last Update", "Designer", "Assisted By", "Reviewed By", "Category", 
                "Architecture Details", "Comments", "Total Hours", "Architecture Type", "End Customer", "State","Country", "Project Name"
            };
            
            setDGV_HeaderText(dgv, headers);

            int[] widths = { 10,220,140,140,130,130,130,100,150,90,70,140,180,200,
                140,140,100,120,100,100,120,100,100,140,100,100,150,100 };
            SetDGV_ColumnWidths(dgv, widths);
            dgv.Columns[0].Visible = false;
            dgv.Columns[29].Visible = false;

            string[] currencyCols = { "BOM_Value" };
            setDGV_CellFormatToCurrency(dgv, currencyCols);
        }

        private static void setDGV_CellFormatToCurrency(DataGridView dgv, string[] cols)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                dgv.Columns[i].DefaultCellStyle.Format = "c";
            }
        }
   
        private static void setDGV_HeaderText(DataGridView dgv, string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                dgv.Columns[i].HeaderText = headers[i];
                DataGridViewCellStyle style = dgv.ColumnHeadersDefaultCellStyle;
                style.Font = new System.Drawing.Font(dgv.Font, System.Drawing.FontStyle.Bold);
                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
            }
      
        }
        public static void FormatCatMSO_DGV(DataGridView dgv)
        {
            string[] headers = { "MSO", "Total Dollars", "Average $/ Request","Total Requests", "HFC", "Node Split", "RFoG", "PON", "RFoG/ PON",
            "Fiber Deep", "Data Trans.", "Other", "PEG", "Commercial","Unassigned", "HFC Dollars","Node Split Dollars", "RFoG Dollars", "PON Dollars",
            "RFoG/ Pon Dollars","Fiber Deep Dollars", "Data Trans. Dollars","Other Dollars", "PEG Dollars","Commercial Dollars",
            "Unassigned Dollars" };

            int[] widths = { 150, 100, 100, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 75, 75, 160,
             160,160,160,160,160,160,160,160};

            SetDGV_ColumnWidths(dgv, widths);
            setDGV_HeaderText(dgv, headers);

            int[] currencyCols = { 1, 2, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
            setDGV_CurrencyCols(dgv, currencyCols);
        }

        private static void setDGV_CurrencyCols(DataGridView dgv, int[] cols)
        {
            foreach (int col in cols)
            {
                dgv.Columns[col].DefaultCellStyle.Format = "c2";
            }
        }
        public static void FormatCatMSO_Export(Excel.Worksheet wks)
        {
            string[] headers = { "MSO", "Total Dollars", "Average $/ Request","Total Requests", "HFC", "Node Split", "RFoG", "PON", "RFoG/ PON",
            "Fiber Deep", "Data Trans.", "Other", "PEG", "Commercial","Unassigned", "HFC Dollars", "Node Split Dollars", "RFoG Dollars", "PON Dollars",
            "RFoG/ Pon Dollars","Fiber Deep Dollars", "Data Trans. Dollars","Other Dollars", "PEG Dollars","Commercial Dollars",
            "Unassigned Dollars" };
            placeHeaderTextInExport(wks, headers);
            formatExcelHeaderRow(wks);

            int[] widths = { 25, 15, 20, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 20,
             20,20,20,20,20,20,20,20,20,20};
            setExcelExportColumnWidths(wks, widths);

            string[] currencyCols = { "B", "C", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            FormatExcelColumnsAsCurrency(wks, currencyCols);

            int[] cols = { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15 };
            CenterSpecificExcelColumns(wks, cols);
        }

        public static List<Report_SalesProjectValuesModel> Report_SalesProjectValues()
        {
            List<Report_SalesProjectValuesModel> ValueReport = new List<Report_SalesProjectValuesModel>();
            //List<SalespersonModel> ActiveSales = GlobalConfig.Connection.


            return ValueReport;
        }
    }
}
