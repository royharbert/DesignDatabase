using DesignDB_Library.Models;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using Mono.Cecil.Cil;
using System.Diagnostics;
using StyleCop;
using NuGet;

namespace DesignDB_Library.Operations
{

    public class NewMessageEventArgs
    {
        public string MyMessage { get; set; }
    }
    public static class ReportOps
    {
        
        public static event EventHandler<NewMessageEventArgs> NewMessageEvent;
        

        //create list of salespersons in this report
        static List<SalespersonModel> includedSalesPersons = new List<SalespersonModel>();

        public static void RollupReport(DateTime startDate, DateTime endDate, List<MSO_Model> msoModels, List<string> regionQuery, 
            bool CustomFormat = false)
        {
            includedSalesPersons = new List<SalespersonModel>();
            NewMessageEventArgs args = new NewMessageEventArgs();
            startDate = startDate.Date;
            endDate = endDate.Date;            
            int curYear = startDate.Year;
            DateTime NewYearsDay = new DateTime(curYear, 1, 1);
            DateTime NewYearsEve = new DateTime(curYear, 12, 31);

            //Collect all salespersons            
            sendMessage("Getting Salespersons");
            List<SalespersonModel> allSalesPersons = GlobalConfig.Connection.GenericGetAll<SalespersonModel>("tblSalespersons", "SalesPerson");
            //Collect YTD requests
            sendMessage("Collecting YTD Requests");
            List<RollupRequestModel> allRequests = GlobalConfig.Connection.GetRollupRequests(NewYearsDay, NewYearsEve);
            sendMessage("Retrieved " + allRequests.Count + " Records");
            //sendMessage(args, "Filtering out Canceled, sorting by MSO");
            //Filter out Canceled
            List<RollupRequestModel> allNonCanceledlRequests = allRequests.Where(x => x.AwardStatus != "Canceled").ToList();
            //Filter to selected MSO's
            //Filter nonCanceled requests by MSO
            List<RollupRequestModel> msoFilteredRequests = new List<RollupRequestModel>();
            foreach (var mso in msoModels)
            {
                msoFilteredRequests.AddRange(allNonCanceledlRequests.Where(x => x.MSO == mso.MSO).ToList());
            }
            allNonCanceledlRequests = msoFilteredRequests;

            if (regionQuery.Count > 0)
            {
                List<RollupRequestModel> regionRequest = new List<RollupRequestModel>();
                foreach(string region in regionQuery)
                {
                    regionRequest.AddRange(allNonCanceledlRequests.Where(x => x.Region == region));
                }
               
                allNonCanceledlRequests = regionRequest;
            }

            //Section 1
            sendMessage("Creating Monthly MSO Summary");
            List<Report_SalesProjectValuesModel> monthlyMSO_Summary = MonthlyMSO_Summary(msoModels, allNonCanceledlRequests, startDate, 
                endDate);
            if (monthlyMSO_Summary.Count != 0)
            {
                //Section 2
                sendMessage("Creating MSO Requests by Category Summary");
                List<ReportCategoryMSOModel> mso_RequestsByCategory = MSO_RequestsByCategory(allNonCanceledlRequests, msoModels);

                //Section 4
                sendMessage("Award Status Summary");
                List<AwardStatusModel> awardStatusSummary = AwardStatusSummary(allNonCanceledlRequests, msoModels);

                //Section 7
                sendMessage("Creating Requests by Salesperson Summary");
                List<Report_SalesProjectValuesModel> salesProjects = DesignRequestsBySalespersonPerMonth(allNonCanceledlRequests,
                    allSalesPersons, msoModels, startDate, endDate);

                //Section 5
                sendMessage("Creating Open Requests by Salesperson Summary");
                List<OpenRequestsBySalesModel> openRequestsBySales = new List<OpenRequestsBySalesModel>();
                openRequestsBySales = OpenRequestsBySales(includedSalesPersons, msoModels);

                //Section 6
                //*
                sendMessage("Creating Priority by Salaeperson Summary");
                List<ReportSalesPriorityModel> priorityModelSummary = PriorityModelSummaryNoMS0(allNonCanceledlRequests, includedSalesPersons, msoModels);
                /*/
                List<ReportSalesPriorityModel> priorityModelSummary = PriorityModelSummary(allNonCanceledlRequests, includedSalesPersons, msoModels);
                //*/

                //Section 3
                sendMessage("Creating Completion Time Summary");
                List<RollupCompletionTimeModel> CompletionTime = RollupCompletionTimeSummary(msoModels, allRequests);

                sendMessage("Placing Data in Excel");
                ExcelOps.PlaceRollupInExcel(startDate, endDate, openRequestsBySales, mso_RequestsByCategory, salesProjects,
                    priorityModelSummary, allNonCanceledlRequests.Where(x => x.AwardStatus != "Has Revision").Sum(x => x.BOM_Value),
                    monthlyMSO_Summary, msoModels, awardStatusSummary, CompletionTime, CustomFormat);

                sendMessage("");
            }
            else 
            {
                //MessageBox.Show("No requests found");
                if (msoModels.Count > 0)
                {
                    ExcelOps.CreateNoRecordsFoundRollupReport(msoModels[0].MSO, startDate, endDate); 
                }
                else
                {
                    MessageBox.Show("No MSO selected");
                }
            }
        }        

        public static void sendMessage(string msg)
        {
            NewMessageEventArgs args = new NewMessageEventArgs();
            args.MyMessage = msg;
            NewMessageEvent?.Invoke("ReportOps", args);
            System.Windows.Forms.Application.DoEvents();
        }

        public static List<Report_SalesProjectValuesModel> DesignRequestsBySalespersonPerMonth(List<RollupRequestModel> requestList, 
            List<SalespersonModel> allSalesPersons, List<MSO_Model> msoList, DateTime startDate, DateTime endDate)
        { 
            //List to collect individual details
            List<Report_SalesProjectValuesModel> collectionIndividualRequests = new List<Report_SalesProjectValuesModel>();

            //Model to accumulate company totals
            Report_SalesProjectValuesModel accumulatedRequestData = new Report_SalesProjectValuesModel();

            //place totals in accumulatedRequestValues
            accumulatedRequestData.SalesPerson = "Total";
            //List<RequestModel> filteredRequests = FilterRequestListForMSO(allNonCanceledRequests, msoList);
            
            accumulatedRequestData.CurrentYTD_Value = requestList.Where(x => x.AwardStatus != "Has Revision" && x.AwardStatus != "Canceled").Sum(x => x.BOM_Value);
            accumulatedRequestData.CurrentYear_Count = requestList.Count;
            accumulatedRequestData.PctTotalValue = 1;

            //Get BOM value for all requests
            //Cycle through list of MSO's'
            List<RequestModel> msoRequests = new List<RequestModel>();
            
            if (accumulatedRequestData.CurrentYear_Count > 0)
            {
                List<RollupRequestModel> noRevisionRequests = requestList.Where(x => x.AwardStatus != "Has Revision").ToList();
                accumulatedRequestData.AverageDollars = accumulatedRequestData.CurrentYTD_Value / noRevisionRequests.Count;
            }
            else
            {
                accumulatedRequestData.AverageDollars = 0;
            }

            //foreach (var mso in msoList)
            //{
            //msoRequests.Clear();
            //msoRequests = requestList.Where(x => x.MSO == mso.MSO).ToList();
            foreach (var salesPerson in allSalesPersons)
            {
                //Model to collect single salesperson's requests
                Report_SalesProjectValuesModel individualSalesValue = new Report_SalesProjectValuesModel();
                //List<RequestModel> personRequestList = msoRequests.Where(x => x.DesignRequestor == salesPerson.SalesPerson).ToList();
                List<RollupRequestModel> personRequestList = requestList.Where(x => x.DesignRequestor == salesPerson.SalesPerson).ToList();
                if (personRequestList.Count > 0)
                {
                        individualSalesValue = new Report_SalesProjectValuesModel();
                        //Loop through salesPerson's requests and accumulate by DateAssigned
                        individualSalesValue.SalesPerson = salesPerson.SalesPerson;
                        //individualSalesValue.MSO = mso.MSO;
                        includedSalesPersons.Add(salesPerson);
                        foreach (var request in personRequestList)
                        {
                        if (request.DateAssigned >= startDate && request.DateAssigned <= endDate)
                        {
                            individualSalesValue.Weekly++;
                            accumulatedRequestData.Weekly++;
                        }
                        int month = request.DateAssigned.Month;
                        switch (month)
                        {
                            case 1:
                                individualSalesValue.JanProjects++;
                                accumulatedRequestData.JanProjects++;
                                break;
                            case 2:
                                individualSalesValue.FebProjects++;
                                accumulatedRequestData.FebProjects++;
                                break;
                            case 3:
                                individualSalesValue.MarProjects++;
                                accumulatedRequestData.MarProjects++;
                                break;
                            case 4:
                                individualSalesValue.AprProjects++;
                                accumulatedRequestData.AprProjects++;
                                break;
                            case 5:
                                individualSalesValue.MayProjects++;
                                accumulatedRequestData.MayProjects++;
                                break;
                            case 6:
                                individualSalesValue.JunProjects++;
                                accumulatedRequestData.JunProjects++;
                                break;
                            case 7:
                                individualSalesValue.JulProjects++;
                                accumulatedRequestData.JulProjects++;
                                break;
                            case 8:
                                individualSalesValue.AugProjects++;
                                accumulatedRequestData.AugProjects++;
                                break;
                            case 9:
                                individualSalesValue.SepProjects++;
                                accumulatedRequestData.SepProjects++;
                                break;
                            case 10:
                                individualSalesValue.OctProjects++;
                                accumulatedRequestData.OctProjects++;
                                break;
                            case 11:
                                individualSalesValue.NovProjects++;
                                accumulatedRequestData.NovProjects++;
                                break;
                            case 12:
                                individualSalesValue.DecProjects++;
                                accumulatedRequestData.DecProjects++;
                                break;
                            default:
                                break;
                        }
                        individualSalesValue.CurrentYear_Count++;
                        individualSalesValue.CurrentYTD_Value = personRequestList.Where(x => x.AwardStatus != "Has Revision").Sum(x => x.BOM_Value);
                        //individualSalesValue.AverageDollars = individualSalesValue.CurrentYTD_Value / individualSalesValue.CurrentYear_Count;
                        individualSalesValue.AverageDollars = AvoidDivideByZero(individualSalesValue.CurrentYTD_Value, individualSalesValue.CurrentYear_Count);
                        //individualSalesValue.PctTotalValue = individualSalesValue.CurrentYTD_Value / accumulatedRequestData.CurrentYTD_Value;
                        individualSalesValue.PctTotalValue = AvoidDivideByZero(individualSalesValue.CurrentYTD_Value, accumulatedRequestData.CurrentYTD_Value);
                    }
                }
                if (individualSalesValue.CurrentYear_Count > 0)
                {
                    collectionIndividualRequests.Add(individualSalesValue);
                    individualSalesValue = new Report_SalesProjectValuesModel();
                }
            }
            
            
            collectionIndividualRequests = collectionIndividualRequests.OrderByDescending(x => x.CurrentYTD_Value).ToList();
            collectionIndividualRequests.Add(accumulatedRequestData);

            return collectionIndividualRequests;
        }

        private static List<Report_SalesProjectValuesModel> MonthlyMSO_Summary(List<MSO_Model> msoList, List<RollupRequestModel> requestList, DateTime startDate,
            DateTime endDate)
        {
            Report_SalesProjectValuesModel accumulatedValues = new Report_SalesProjectValuesModel();
            accumulatedValues.SalesPerson = "Total";
            accumulatedValues.CurrentYTD_Value = requestList.Where(x => x.AwardStatus != "Has Revision").Sum(x => x.BOM_Value);
            accumulatedValues.CurrentYear_Count = requestList.Count;
            List<Report_SalesProjectValuesModel> monthlyMSO_Summary = new List<Report_SalesProjectValuesModel>();
            if(requestList.Count == 0)
            {
                return monthlyMSO_Summary;
            }
            accumulatedValues.AverageDollars = AvoidDivideByZero(accumulatedValues.CurrentYTD_Value, 
                requestList.Where(x => x.AwardStatus != "Has Revision").ToList().Count);
            //accumulatedValues.AverageDollars = accumulatedValues.AverageDollars = accumulatedValues.CurrentYTD_Value / requestList.Where(x => x.AwardStatus != "Has Revision").ToList().Count; 
            
            accumulatedValues.PctTotalValue = 1;
            
            foreach (var mso in msoList)
            {
                Report_SalesProjectValuesModel model = new Report_SalesProjectValuesModel();

                model.SalesPerson = mso.MSO;
                List<RollupRequestModel> msoRequests = requestList.Where(x => x.MSO == mso.MSO).ToList();
                if (msoRequests.Count > 0)
                {
                    foreach (var request in msoRequests)
                    {
                        List<RollupRequestModel> monthlyRequests = new List<RollupRequestModel>();
                        if (request.DateAssigned >= startDate && request.DateAssigned <= endDate)
                        {
                            model.Weekly++;
                            accumulatedValues.Weekly++;
                        }
                        model.CurrentYear_Count = msoRequests.Count;
                        model.CurrentYTD_Value = msoRequests.Where(x => x.AwardStatus != "Has Revision").Sum(x => x.BOM_Value);
                        if (model.CurrentYear_Count != 0)
                        {
                            model.AverageDollars = model.CurrentYTD_Value / model.CurrentYear_Count; 
                        }
                        if (model.CurrentYTD_Value != 0)
                        {
                            model.PctTotalValue = model.CurrentYTD_Value / accumulatedValues.CurrentYTD_Value; 
                        }

                        monthlyRequests = msoRequests.Where(x => x.DateAssigned.Month == 1).ToList();
                        int month = request.DateAssigned.Month;
                        switch (month)
                        {
                            case 1:
                                model.JanProjects++;
                                accumulatedValues.JanProjects++;
                                break;

                            case 2:
                                model.FebProjects++;
                                accumulatedValues.FebProjects++;
                                break;

                            case 3:
                                model.MarProjects++;
                                accumulatedValues.MarProjects++;
                                break;

                            case 4:
                                model.AprProjects++;
                                accumulatedValues.AprProjects++;
                                break;

                            case 5:
                                model.MayProjects++;
                                accumulatedValues.MayProjects++;
                                break;

                            case 6:
                                model.JunProjects++;
                                accumulatedValues.JunProjects++;
                                break;

                            case 7:
                                model.JulProjects++;
                                accumulatedValues.JulProjects++;
                                break;

                            case 8:
                                model.AugProjects++;
                                accumulatedValues.AugProjects++;
                                break;

                            case 9:
                                model.SepProjects++;
                                accumulatedValues.SepProjects++;
                                break;

                            case 10:
                                model.OctProjects++;
                                accumulatedValues.OctProjects++;
                                break;

                            case 11:
                                model.NovProjects++;
                                accumulatedValues.NovProjects++;
                                break;

                            case 12:
                                model.DecProjects++;
                                accumulatedValues.DecProjects++;
                                break;
                        }
                    }
                    monthlyMSO_Summary.Add(model); 
                }
            }
            monthlyMSO_Summary = monthlyMSO_Summary.OrderByDescending(x => x.CurrentYTD_Value).ToList();
            monthlyMSO_Summary.Add(accumulatedValues); 
            

            return monthlyMSO_Summary;
        }

        private static decimal AvoidDivideByZero(decimal dividend, decimal divisor, bool returnAsPercent = false)
        {
            decimal result = 0;
            if(divisor != 0)
            {
                result = dividend / divisor;
            }
            if(returnAsPercent)
            {
                return result * 100;
            }
            return result;
        }


        private static List<ReportCategoryMSOModel> MSO_RequestsByCategory(List<RollupRequestModel> requestList, List<MSO_Model> msoList)
        {
            //Create list for company totals
            List<ReportCategoryMSOModel> companySummary  = new List<ReportCategoryMSOModel>();

            //Create model to summarize totals
            ReportCategoryMSOModel accumulatedCategorySummary = new ReportCategoryMSOModel();
            accumulatedCategorySummary.MSO = "Total";

            List<RollupRequestModel> noRevisionRequests = requestList.Where(x => x.AwardStatus != "Has Revision").ToList();
            accumulatedCategorySummary.TotalDollars = noRevisionRequests.Sum(x => x.BOM_Value);
            accumulatedCategorySummary.TotalRequests = requestList.Count;

            accumulatedCategorySummary.AverageDollarsPerRequest = AvoidDivideByZero(accumulatedCategorySummary.TotalDollars, noRevisionRequests.Count);
            //accumulatedCategorySummary.AverageDollarsPerRequest = accumulatedCategorySummary.TotalDollars / noRevisionRequests.Count;
            accumulatedCategorySummary.PctOfTotal = 1;
            foreach (var mso in msoList)
            {
                List<RollupRequestModel> msoRequests = requestList.Where(x => x.MSO == mso.MSO).ToList();
                ReportCategoryMSOModel individualLine = new ReportCategoryMSOModel();
                individualLine.MSO = mso.MSO;
                if (msoRequests.Count > 0)
                {
                    individualLine.MSO = mso.MSO;
                    individualLine.TotalRequests = msoRequests.Count;
                    individualLine.TotalDollars = msoRequests.Where(x => x.AwardStatus != "Has Revision").Sum(x => x.BOM_Value);
                    individualLine.AverageDollarsPerRequest = individualLine.TotalDollars/individualLine.TotalRequests;
                    //individualLine.PctOfTotal = individualLine.TotalDollars / accumulatedCategorySummary.TotalDollars;
                    individualLine.PctOfTotal = AvoidDivideByZero(individualLine.TotalDollars, accumulatedCategorySummary.TotalDollars);
                    foreach (var request in msoRequests)
                    {
                        switch (request.Category)
                        {
                            case "HFC":
                                individualLine.HFC++;
                                individualLine.HFCDollars = individualLine.HFCDollars + request.BOM_Value;
                                accumulatedCategorySummary.HFC++;
                                accumulatedCategorySummary.HFCDollars = accumulatedCategorySummary.HFCDollars + request.BOM_Value;
                                break;

                            case "Node Split":
                                individualLine.NodeSplit++;
                                individualLine.NodeSplitDollars = individualLine.NodeSplitDollars + request.BOM_Value;
                                accumulatedCategorySummary.NodeSplit++;
                                accumulatedCategorySummary.NodeSplitDollars = accumulatedCategorySummary.NodeSplitDollars + request.BOM_Value;
                                break;

                            case "RFoG":
                                individualLine.RFoG++;
                                individualLine.RFoGDollars = individualLine.RFoGDollars + request.BOM_Value;
                                accumulatedCategorySummary.RFoG++;
                                accumulatedCategorySummary.RFoGDollars = accumulatedCategorySummary.RFoGDollars + request.BOM_Value;
                                break;

                            case "PON":
                                individualLine.PON++;
                                individualLine.PON_Dollars = individualLine.PON_Dollars + request.BOM_Value;
                                accumulatedCategorySummary.PON++;
                                accumulatedCategorySummary.PON_Dollars = accumulatedCategorySummary.PON_Dollars + request.BOM_Value;
                                break;

                            case "RFoG-POM":
                                individualLine.RFoGPON++;
                                individualLine.RFoGPON_Dollars = individualLine.RFoGPON_Dollars + request.BOM_Value;
                                accumulatedCategorySummary.RFoGPON++;
                                accumulatedCategorySummary.RFoGPON_Dollars = accumulatedCategorySummary.RFoGPON_Dollars + request.BOM_Value;
                                break;

                            case "Fiber Deep":
                                individualLine.FiberDeep++;
                                individualLine.FiberDeepDollars = individualLine.FiberDeepDollars + request.BOM_Value;
                                accumulatedCategorySummary.FiberDeep++;
                                accumulatedCategorySummary.FiberDeepDollars = accumulatedCategorySummary.FiberDeepDollars + request.BOM_Value;
                                break;

                            case "Data Transport":
                                individualLine.DataTrans++;
                                individualLine.DataTransportDollars = individualLine.DataTransportDollars + request.BOM_Value;
                                accumulatedCategorySummary.DataTrans++;
                                accumulatedCategorySummary.DataTransportDollars = accumulatedCategorySummary.DataTransportDollars + request.BOM_Value;
                                break;

                            case "Other":
                                individualLine.Other++;
                                individualLine.OtherDollars = individualLine.OtherDollars + request.BOM_Value;
                                accumulatedCategorySummary.Other++;
                                accumulatedCategorySummary.OtherDollars = accumulatedCategorySummary.OtherDollars + request.BOM_Value;
                                break;

                            case "Unassigned":
                            
                                individualLine.Unassigned++;
                                individualLine.UnassignedDollars = individualLine.UnassignedDollars + request.BOM_Value;
                                accumulatedCategorySummary.Unassigned++;
                                accumulatedCategorySummary.UnassignedDollars = accumulatedCategorySummary.UnassignedDollars + request.BOM_Value;
                                break;

                             default:
                                individualLine.Unassigned++;
                                individualLine.UnassignedDollars = individualLine.UnassignedDollars + request.BOM_Value;
                                accumulatedCategorySummary.Unassigned++;
                                accumulatedCategorySummary.UnassignedDollars = accumulatedCategorySummary.UnassignedDollars + request.BOM_Value;
                                break;
                        }
                    } 
                companySummary.Add(individualLine);
                }
            }
            companySummary = companySummary.OrderByDescending(x => x.TotalDollars).ToList();
            companySummary.Add(accumulatedCategorySummary);
            return companySummary;
        }

        private static List<AwardStatusModel> AwardStatusSummary(List<RollupRequestModel> requestList, List<MSO_Model> msoList)
        {
            List<AwardStatusModel> awardModels= new List<AwardStatusModel>();
            AwardStatusModel summaryLine = new AwardStatusModel();
            List<MSO_Model> activeMSOs = new List<MSO_Model>();
            summaryLine.MSO = "Total";
            foreach (var mso in msoList)
            {
                List<RollupRequestModel> requests = requestList.Where(x => x.MSO == mso.MSO).ToList();
                if (requests.Count > 0)
                {
                    activeMSOs.Add(mso);
                    AwardStatusModel msoAwardStatus = new AwardStatusModel();
                    msoAwardStatus.MSO = mso.MSO;
                    List<RollupRequestModel> msoRequests = requestList.Where(x => x.MSO == mso.MSO).ToList();
                    msoAwardStatus.PendingCount = msoRequests.Where(x => x.AwardStatus == "Pending").ToList().Count;
                    msoAwardStatus.PendingDollars = msoRequests.Where(x => x.AwardStatus == "Pending").ToList().Sum(x => x.BOM_Value);
                    msoAwardStatus.TotalCount = msoAwardStatus.TotalCount + msoAwardStatus.PendingCount;

                    msoAwardStatus.HasRevisionCount = msoRequests.Where(x => x.AwardStatus == "Has Revision").ToList().Count;
                    msoAwardStatus.HasRevisionDollars = msoRequests.Where(x => x.AwardStatus == "Has Revision").ToList().Sum(x => x.BOM_Value);
                    msoAwardStatus.TotalCount = msoAwardStatus.TotalCount + msoAwardStatus.HasRevisionCount;

                    msoAwardStatus.CanceledCount = msoRequests.Where(x => x.AwardStatus == "Canceled").ToList().Count;
                    msoAwardStatus.CanceledDollars = msoRequests.Where(x => x.AwardStatus == "Canceled").ToList().Sum(x => x.BOM_Value);
                    msoAwardStatus.TotalCount = msoAwardStatus.TotalCount + msoAwardStatus.CanceledCount;

                    msoAwardStatus.InactiveCount = msoRequests.Where(x => x.AwardStatus == "Inactive").ToList().Count;
                    msoAwardStatus.HasRevisionDollars = msoRequests.Where(x => x.AwardStatus == "Inactive").ToList().Sum(x => x.BOM_Value);
                    msoAwardStatus.TotalCount = msoAwardStatus.TotalCount + msoAwardStatus.InactiveCount;

                    msoAwardStatus.WonCount = msoRequests.Where(x => x.AwardStatus == "Won").ToList().Count;
                    msoAwardStatus.WonDollars = msoRequests.Where(x => x.AwardStatus == "Won").ToList().Sum(x => x.BOM_Value);
                    msoAwardStatus.TotalCount = msoAwardStatus.TotalCount + msoAwardStatus.WonCount;

                    msoAwardStatus.LostCount = msoRequests.Where(x => x.AwardStatus == "Lost").ToList().Count;
                    msoAwardStatus.LostDollars = msoRequests.Where(x => x.AwardStatus == "Lost").ToList().Sum(x => x.BOM_Value);
                    msoAwardStatus.TotalCount = msoAwardStatus.TotalCount + msoAwardStatus.LostCount;

                    summaryLine.PendingCount = summaryLine.PendingCount + msoAwardStatus.PendingCount;
                    summaryLine.PendingDollars = summaryLine.PendingDollars + msoAwardStatus.PendingDollars;
                    summaryLine.HasRevisionCount = summaryLine.HasRevisionCount + msoAwardStatus.HasRevisionCount;
                    summaryLine.HasRevisionDollars = summaryLine.HasRevisionDollars + msoAwardStatus.HasRevisionDollars;
                    summaryLine.CanceledCount = summaryLine.CanceledCount + msoAwardStatus.CanceledCount;
                    summaryLine.CanceledDollars = summaryLine.CanceledDollars + msoAwardStatus.CanceledDollars;
                    summaryLine.InactiveCount = summaryLine.InactiveCount + msoAwardStatus.InactiveCount;
                    summaryLine.InactiveDollars = summaryLine.InactiveDollars + msoAwardStatus.InactiveDollars;
                    summaryLine.WonCount = summaryLine.WonCount + msoAwardStatus.WonCount;
                    summaryLine.WonDollars = summaryLine.WonDollars + msoAwardStatus.WonDollars;
                    summaryLine.LostCount = summaryLine.LostCount + msoAwardStatus.LostCount;
                    summaryLine.LostDollars = summaryLine.LostDollars + msoAwardStatus.LostDollars;

                    awardModels.Add(msoAwardStatus);
                } 
            }
            awardModels = awardModels.OrderByDescending(x => x.PendingCount).ToList();
            awardModels.Add(summaryLine);

            return awardModels;
        }
        private static List<OpenRequestsBySalesModel> OpenRequestsBySales(List<SalespersonModel> salesPersons, List<MSO_Model> msoList)
        {
            //Create list to hold final values
            List<OpenRequestsBySalesModel> accumulatedOpenSummary = new List<OpenRequestsBySalesModel>();

            //Create model to accumulate Totals
            OpenRequestsBySalesModel companySummary = new OpenRequestsBySalesModel();

            //Assign 'Total' SalesPerson attribute
            companySummary.Salesperson = "Total";

            //Retrieve all open Requests
            List<RequestModel> openRequests = GlobalConfig.Connection.GetOpenRequests();
            //remove canceled
            List<RequestModel> filteredRequests = openRequests.Where(x => x.AwardStatus != "Canceled" && x.AwardStatus != "Has Revision").ToList();
           
            //remove duplicate salespersons
            salesPersons = salesPersons.Distinct().ToList();
            //Cycle through list of salespersons provided in input parameters
            OpenRequestsBySalesModel lineEntry = new OpenRequestsBySalesModel();
            foreach (var salesperson in salesPersons)
            {
                //Create list of open requests from current salesperson
                List<RequestModel> personRequests = openRequests.Where(x => x.DesignRequestor == salesperson.SalesPerson).ToList();

                if (personRequests.Count > 0)
                {
                    lineEntry = new OpenRequestsBySalesModel();
                    
                    lineEntry.Salesperson = salesperson.SalesPerson;
                    foreach (var request in personRequests)
                    {
                        //Place total count for salesperson/MSO pair in model
                        lineEntry.Count = personRequests.Count;
                        companySummary.Count ++;
                        //Increment counts by month
                        if (request.DesignRequestor == lineEntry.Salesperson)
                        {
                            switch (request.DateAssigned.Month)
                            {
                                case 1:
                                    lineEntry.Jan++;
                                    companySummary.Jan++;
                                    break;

                                case 2:
                                    lineEntry.Feb++;
                                    companySummary.Feb++;
                                    break;

                                case 3:
                                    lineEntry.Mar++;
                                    companySummary.Mar++;
                                    break;

                                case 4:
                                    lineEntry.Apr++;
                                    companySummary.Apr++;
                                    break;

                                case 5:
                                    lineEntry.May++;
                                    companySummary.May++;
                                    break;

                                case 6:
                                    lineEntry.Jun++;
                                    companySummary.Jun++;
                                    break;

                                case 7:
                                    lineEntry.Jul++;
                                    companySummary.Jul++;
                                    break;

                                case 8:
                                    lineEntry.Aug++;
                                    companySummary.Aug++;
                                    break;

                                case 9:
                                    lineEntry.Sep++;
                                    companySummary.Sep++;
                                    break;

                                case 10:
                                    lineEntry.Oct++;
                                    companySummary.Oct++;
                                    break;

                                case 11:
                                    lineEntry.Nov++;
                                    companySummary.Nov++;
                                    break;

                                case 12:
                                    lineEntry.Dec++;
                                    companySummary.Dec++;
                                    break;
                                default:
                                    break;
                            } 
                        }
                    }
                    accumulatedOpenSummary.Add(lineEntry); 
                }
            }
            accumulatedOpenSummary = accumulatedOpenSummary.OrderByDescending(x => x.Count).ToList();
            accumulatedOpenSummary.Add(companySummary);

            return accumulatedOpenSummary;
        }

        /*
        private static List<ReportSalesPriorityModel> PriorityModelSummary(List<RequestModel> requests, List<SalespersonModel> salesPersons,
            List<MSO_Model> msoList)
        {
            //Eliminate dups in salespersons list
            salesPersons = salesPersons.Distinct().ToList();

            //Create list of priority models to hold data
            List<ReportSalesPriorityModel> priorityModels = new List<ReportSalesPriorityModel>();

            //Create priority model to hold accumulated totals and place total count in model
            ReportSalesPriorityModel companyTotal = new ReportSalesPriorityModel();
            companyTotal.SalesPerson = "Total";
            companyTotal.TotalCount = requests.Count;

            //Create model in appropriate scope
            ReportSalesPriorityModel model = new ReportSalesPriorityModel();

            //List<RequestModel> msoRequests = new List<RequestModel>();
            List<RequestModel> filteredRequests = new List<RequestModel>();
            foreach (var person in salesPersons)
            {
                //msoRequests.Clear();
                foreach (var mso in msoList)
                {
                    filteredRequests = requests.Where(x => x.DesignRequestor == person.SalesPerson && x.MSO == mso.MSO).ToList();
                    if (filteredRequests.Count > 0)
                    {
                        model = new ReportSalesPriorityModel();
                        model.SalesPerson = person.SalesPerson;
                        model.MSO = mso.MSO;
                        foreach (var filteredRequest in filteredRequests)
                        {
                            if (filteredRequest.DesignRequestor == model.SalesPerson && filteredRequest.MSO == model.MSO)
                            {
                                switch (filteredRequest.Pty)
                                {
                                    case "P1":
                                        model.P1Count++;
                                        model.P1Dollars = model.P1Dollars + filteredRequest.BOM_Value;
                                        model.TotalCount++;
                                        companyTotal.P1Count++;
                                        companyTotal.P1Dollars = companyTotal.P1Dollars = filteredRequest.BOM_Value;
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
                            }
                        }
                        priorityModels.Add(model);
                    }
                }

            }
            priorityModels = priorityModels.OrderByDescending(x => x.TotalCount).ToList();
            priorityModels.Add(companyTotal);

            //Insert company total %


            return priorityModels;
        }
        /*/

        //Switch to NoMSO version in ExcelOps
        private static List<ReportSalesPriorityModel> PriorityModelSummaryNoMS0(List<RollupRequestModel> requests, List<SalespersonModel> salesPersons,
            List<MSO_Model> msoList)
        {
            //Eliminate dups in salespersons list
            salesPersons = salesPersons.Distinct().ToList();

            //Create list of priority models to hold data
            List<ReportSalesPriorityModel> priorityModels = new List<ReportSalesPriorityModel>();

            //Create priority model to hold accumulated totals and place total count in model
            ReportSalesPriorityModel companyTotal = new ReportSalesPriorityModel();
            companyTotal.SalesPerson = "Total";
            companyTotal.TotalCount = requests.Count;

            //Create model in appropriate scope
            ReportSalesPriorityModel model = new ReportSalesPriorityModel();

            //List<RollupRequestModel> msoRequests = new List<RollupRequestModel>();
            List<RollupRequestModel> filteredRequests = new List<RollupRequestModel>();
            foreach (var person in salesPersons)
            {

                filteredRequests = requests.Where(x => x.DesignRequestor == person.SalesPerson).ToList();
                if (filteredRequests.Count > 0)
                {
                    model = new ReportSalesPriorityModel();
                    model.SalesPerson = person.SalesPerson;
                    foreach (var filteredRequest in filteredRequests)
                    {
                        
                            switch (filteredRequest.Pty)
                            {
                                case "P1":
                                    model.P1Count++;
                                    model.P1Dollars = model.P1Dollars + filteredRequest.BOM_Value;
                                    model.TotalCount++;
                                    companyTotal.P1Count++;
                                    companyTotal.P1Dollars = companyTotal.P1Dollars = filteredRequest.BOM_Value;
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
                        
                    }
                    priorityModels.Add(model);
                }
                

            }
            priorityModels = priorityModels.OrderByDescending(x => x.TotalCount).ToList();
            priorityModels.Add(companyTotal);

            //Insert company total %


            return priorityModels;

        }
        //*/
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
                    
                    if (tagArray[2] == "")
                    {
                        //Source is internal list
                        foreach (var item in cbo.Items)
                        {
                            (string Field, bool Active) tup = (item.ToString(), true);
                            if (tup.Field != "\t" && tup.Field != "")
                            {
                                ddList.Add(tup); 
                            }
                        }
                        ddList = ddList.OrderBy(x => x.Field).ToList();
                        //place list description at head of list
                        (string Field, bool Active) listHead = (tagArray[4], true);
                        ddList.Insert(0, listHead);
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
            (string Field, bool Active) listHead = (cName, true);
            (string Field, bool Active) tup = (cName, true);
            List<(string Field, bool Active)> ddList = new List<(string Field, bool Active)>();
            
            //Type model = GetType(T);
            List<T> dataList = GlobalConfig.Connection.GenericGetAll<T>(tableName);
            foreach (T item in dataList)
            {
                Type modelType = item.GetType();
                //PropertyInfo[] props = modelType.GetProperties();
                PropertyInfo fieldInfo = modelType.GetProperty(FieldName);
                tup.Field = fieldInfo.GetValue(item, null).ToString();
                tup.Active = true;
                if (tup.Field != "")
                {
                    ddList.Add(tup); 
                }
            }
            ddList = ddList.OrderBy(x => x.Field).ToList();
            ddList.Insert(0, listHead);
            return ddList;
        }

        private static List<(string Field, bool Active)> MakeTupleList<T>(string tableName,  string FieldName, string activeField, string cName)
        {
        (string Field, bool Active) tup = (cName, true);
        (string Field, bool Active) listHead = (cName, true);
        List<(string Field,bool Active)> ddList = new List<(string Field,bool Active)> ();
        
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
                if (tup.Field != "")
                {
                    ddList.Add(tup); 
                }
        }
        ddList = ddList.OrderBy(x => x.Field).ToList();
        ddList.Insert(0, listHead);
        return ddList;
    }

        public static int NumberOfWorkDays(DateTime startDate, DateTime endDate)
        {
            List<CompanyHolidaysModel> holidaysList = GlobalConfig.Connection.GetAllHolidays();

            int workingDays = 0;
            while(startDate <= endDate)
            {
                if(startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }
                startDate = startDate.AddDays(1);
            }

            foreach (var holiday in holidaysList)
            {
                if ((holiday.HolidayDate.Date - endDate.Date).TotalDays <= 0)
                {
                    workingDays = workingDays - 1;
                }
                else
                {
                    break;
                }
            }

            return workingDays;
        }

        public static List<CompletionTimeModel>DoCompletionTimeSummary(DateTime startDate, DateTime endDate, List<MSO_Model> msoList)
        {
            DateTime emptyDate = new DateTime(1900, 1, 1);
            List<CompletionTimeModel> report = new List<CompletionTimeModel>();
            List<RequestModel> allRequests = GlobalConfig.Connection.DateRangeSearch_Unfiltered(startDate, endDate);
            List<RequestModel> canceledDesigns = allRequests.Where(x => x.AwardStatus == "Canceled").ToList();
            List<RequestModel> activeDesigns = allRequests.Where(x => x.AwardStatus != "Canceled").ToList();
            List<RequestModel> completedDesigns = activeDesigns.Where(x => x.DateCompleted != DateTime.MinValue && x.DateCompleted != emptyDate 
                && x.AwardStatus != "Canceled").ToList();
            List<RequestModel> openDesigns = activeDesigns.Where(x => x.DateCompleted == emptyDate || x.DateCompleted == DateTime.MinValue
                && x.AwardStatus != "Canceled").ToList();

            //loop thru MSO's with requests
            foreach (var mso in msoList)
            {
                string MSO = mso.MSO;
                List<RequestModel> msoRequests = completedDesigns.Where(x => x.MSO == MSO).ToList();
                if (msoRequests.Count > 0)
                {
                    CompletionTimeModel cModel = new CompletionTimeModel();
                    cModel.MSO = MSO;
                    cModel.CompletedDesigns = msoRequests.Count;
                    cModel.TotalDaysToComplete = (int)msoRequests.Sum(x => (x.DateCompleted - x.DateAssigned).TotalDays);
                    float avgDaysToComplete = (cModel.TotalDaysToComplete / cModel.CompletedDesigns);
                    cModel.AvgDaysToComplete = avgDaysToComplete.ToString("#.00");
                    cModel.CanceledDesigns = msoRequests.Where(x => x.AwardStatus == "Canceled").ToList().Count;
                    cModel.TotalDaysFromAllInfo = (int)msoRequests.Sum(x => (x.DateCompleted - x.DateAllInfoReceived).TotalDays);
                    float avgDaysFromAllInfo = (cModel.TotalDaysFromAllInfo / cModel.CompletedDesigns);
                    cModel.AvgDaysFromAllInfo = avgDaysFromAllInfo.ToString("#.00");

                    List<RequestModel> P1Jobs = msoRequests.Where(x => x.Pty == "P1").ToList();
                    int P1Count = P1Jobs.Count;
                    float P1Days = (int)P1Jobs.Sum(x => (x.DateCompleted - x.DateAllInfoReceived).TotalDays);
                    //cModel.TotalDaysFromAllInfo = P1Days;
                    if (P1Count > 0)
                    {
                        float P1Avg = P1Days / (float)P1Count; 
                        cModel.P1Average = P1Avg.ToString("#.00");
                    }
                    else
                    {
                        cModel.P1Average = "N/A";
                    }

                    List<RequestModel> P2Jobs = msoRequests.Where(x => x.Pty == "P2").ToList();
                    int P2Count = P2Jobs.Count;
                    float P2Days = (int)P2Jobs.Sum(x => (x.DateCompleted - x.DateAllInfoReceived).TotalDays);
                    if (P2Count > 0)
                    {
                        float P2Avg = P2Days / (float)P2Count;
                        cModel.P2Average = P2Avg.ToString("#.00");
                    }
                    else
                    {
                        cModel.P2Average = "N/A";
                    }

                    List<RequestModel> P3Jobs = msoRequests.Where(x => x.Pty == "P3").ToList();
                    int P3Count = P3Jobs.Count;
                    float P3Days = (int)P3Jobs.Sum(x => (x.DateCompleted - x.DateAllInfoReceived).TotalDays);
                    //cModel.TotalDaysFromAllInfo = P1Days;
                    if (P3Count > 0)
                    {
                        float P3Avg = P3Days / (float)P3Count;
                        cModel.P3Average = P3Avg.ToString("#.00");
                    }
                    else
                    {
                        cModel.P3Average = "N/A";
                    }

                    report.Add(cModel);
                }
            }
            return report;
        }

        public static List<RollupCompletionTimeModel> RollupCompletionTimeSummary(List<MSO_Model> msoList, 
            List<RollupRequestModel> allRequests)
        {
            DateTime emptyDate = new DateTime(1900,1,1);
            List<RollupCompletionTimeModel> report = new List<RollupCompletionTimeModel>();
            List<RollupRequestModel> completedDesigns = allRequests.Where(x => x.DateCompleted != null && 
                x.DateCompleted != emptyDate && x.AwardStatus != "Canceled").ToList();
            RollupCompletionTimeModel summaryLine = new RollupCompletionTimeModel();

            foreach (var mso in msoList)
            {
                List<RollupRequestModel> msoCompletedDesigns = completedDesigns.Where(x => x.MSO == mso.MSO).ToList();
                if (msoCompletedDesigns.Count > 0)
                {
                    RollupCompletionTimeModel lineEntry = new RollupCompletionTimeModel();
                    lineEntry.MSO = mso.MSO;
                    lineEntry.CompletedDesigns = msoCompletedDesigns.Count;
                    double daysToComplete = 0;
                    double daysFromAllInfo = 0;

                    //Accumulate days from requests
                    foreach (var request in msoCompletedDesigns)
                    {
                        double dtc = (request.DateCompleted - request.DateAssigned).TotalDays;
                        double dai = (request.DateCompleted - request.DateAssigned).TotalDays;
                        if (dtc >= 0 && dai > 0)
                        {
                            daysToComplete = daysToComplete + (request.DateCompleted - request.DateAssigned).TotalDays;
                            daysFromAllInfo = daysFromAllInfo + (request.DateAllInfoReceived - request.DateAssigned).TotalDays;

                        }
                    }
                    //Populate line entry model
                    lineEntry.AvgDaysToComplete = (float)daysToComplete / lineEntry.CompletedDesigns;
                    lineEntry.AvgDaysFromAllInfo = (float)daysFromAllInfo / lineEntry.CompletedDesigns;
                    report.Add(lineEntry);
                }
            }
            report = report.OrderByDescending(x => x.CompletedDesigns).ToList();
            
            return report;
        }

        public static List<ReportSalesPriorityModel> GenerateSalesSummary(DateTime startDate, DateTime endDate)
        {
            List<SalespersonModel> sales = GlobalConfig.Connection.GenericGetAll<SalespersonModel>("tblSalespersons", "SalesPerson");
            List<ReportSalesPriorityModel> report = new List<ReportSalesPriorityModel>();

            //get list of requests in timeframe
            List<RequestModel> requests = GlobalConfig.Connection.DateRangeSearch_Unfiltered(startDate, endDate, 
                "DateCompleted", false, "");

            //loop thru sales team
            List<RequestModel> requestsP1 = requests.Where(x => x.Pty == "P1").ToList();
            List<RequestModel> requestsP2 = requests.Where(x => x.Pty == "P2").ToList();
            List<RequestModel> requestsP3 = requests.Where(x => x.Pty == "P3").ToList();
            List<RequestModel> sRequests = new List<RequestModel>();
            foreach (SalespersonModel salesperson in sales)
            {

                //use linq to extract data
                sRequests = requests.Where(x => x.DesignRequestor == salesperson.SalesPerson).ToList();
                if (sRequests.Count > 0)
                {
                    ReportSalesPriorityModel model = new ReportSalesPriorityModel();
                    model.SalesPerson = salesperson.SalesPerson;
                    model.TotalCount = sRequests.Count;

                    model.P1Count = sRequests.Where(x => x.Pty == "P1").ToList().Count();
                    model.P2Count = sRequests.Where(x => x.Pty == "P2").ToList().Count();
                    model.P3Count = sRequests.Where(x => x.Pty == "P3").ToList().Count();


                    model.P1Dollars = sRequests.Where(x => x.Pty == "P1").Sum(x => x.BOM_Value);
                    model.P2Dollars = sRequests.Where(x => x.Pty == "P2").Sum(x => x.BOM_Value);
                    model.P3Dollars = sRequests.Where(x => x.Pty == "P3").Sum(x => x.BOM_Value);

                    model.TotalDollars = model.P1Dollars + model.P2Dollars + model.P3Dollars;
                    //add line to report
                    report.Add(model);
                }
                sRequests = null;
            }

            return report;
        }

        public static void formatPriorityDGV(DataGridView dgv)
        {
            string[] headers = { "Salesperson", "P1 Count", "P2 Count", "P3 Count", "P1 Dollars",
                "P2 Dollars", "P3 Dollars", "Total Count","Total Dollars", "P1 %", "P2 %", "P3 %" };
            setDGV_HeaderText(dgv, headers);

            int[] widths = { 130, 70, 70, 70, 80, 80, 80, 80 };
            //SetDGV_ColumnWidths(dgv, widths);
            dgv.Columns[9].DefaultCellStyle.Format = "##0%";
            dgv.Columns[10].DefaultCellStyle.Format = "##0%";
            dgv.Columns[11].DefaultCellStyle.Format = "##0%";
            dgv.Columns[4].DefaultCellStyle.Format = "c";
            dgv.Columns[5].DefaultCellStyle.Format = "c";
            dgv.Columns[6].DefaultCellStyle.Format = "c";
            dgv.Columns[8].DefaultCellStyle.Format = "c";
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
            DateTime emptyDate = new DateTime(1900, 1, 1);
            int year = DateTime.Now.Year;
            DateTime newYearsDay = new DateTime(year, 1, 1);
            int month = DateTime.Now.Month;
            List<RequestModel> ytdRequests = GlobalConfig.Connection.DateRangeSearch_Unfiltered(newYearsDay, endDate);
            
            ytdRequests = ytdRequests.Where(x => x.DateCompleted != null && x.DateCompleted != emptyDate).ToList();
            List<SnapshotModel> snapshots = new List<SnapshotModel>();
            foreach (MSO_Model mso in msoList)
            {
                List<RequestModel> msoRequests = ytdRequests.Where(x => x.MSO == mso.MSO).ToList();
                if (msoRequests.Count > 0)
                {
                    SnapshotModel snapLine = new SnapshotModel();
                    snapLine.TotalCanceledDesigns = msoRequests.Where(x => x.AwardStatus == "Canceled").Count();
                    List<RequestModel> nonCanceledMSORequests = msoRequests.Where(x => x.AwardStatus != "Canceled").ToList();
                    snapLine.MSO = mso.MSO;
                    snapLine.TotalCompletedDesigns = nonCanceledMSORequests.Count;
                    snapLine.RequestsThisYear = nonCanceledMSORequests.Where(x => x.AwardStatus != "Canceled").Count();
                    snapLine.RequestsThisMonth = nonCanceledMSORequests.Where(x => x.DateAssigned.Month == month).Count();
                    snapLine.RequestsThisWeek = nonCanceledMSORequests.Where(x => x.DateAssigned >=  startDate && x.DateAssigned < 
                        startDate.AddDays(7)).Count();
                    snapLine.TotalValue = nonCanceledMSORequests.Sum(x => x.BOM_Value);
                    
                    foreach (var msoRequest in msoRequests)
                    {
                        snapLine.TotalDaysToComplete = snapLine.TotalDaysToComplete + (msoRequest.DateCompleted - msoRequest.DateAssigned).Days;
                    }
                    snapLine.AverageCompletionTime = snapLine.TotalDaysToComplete / snapLine.TotalCompletedDesigns;
                    snapshots.Add(snapLine);
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

                //if (snap.TotalCompletedDesigns > 0)
                //{
                //    snap.AverageCompletionTime = (totalDaysToComplete / snap.TotalCompletedDesigns).ToString("#.#");
                //}
                //else
                //{
                //    snap.AverageCompletionTime = "";
                //}
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

            dgv.Columns[8].DefaultCellStyle.Format = "c";
            //int[] widths = { 140, 140, 140, 140, 140, 140, 140, 140, 140, 140 };
            //SetDGV_ColumnWidths(dgv, widths);
        }

        public static void FormatLogViewDGV(DataGridView dgv)
        {
            string[] headers = { "Time Stamp", "Request ID", "User", "Action", "Affected Fields" };
            setDGV_HeaderText(dgv, headers);


            int[] widths = { 140, 200, 140, 140, 140 };
            //SetDGV_ColumnWidths(dgv, widths);

            dgv.Columns[3].Visible = true;
            dgv.Columns[4].Visible = false;
        }

        public static void FormatDesignerLoadDGV(DataGridView dgv)
        {
            string[] headers = { "Designer", "Priority", "Date Due", "Project ID", "Award Status" };
            setDGV_HeaderText(dgv, headers);        
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
                    "DateAssigned", mso.MSO,false, "", "").ToList();

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
            int[] widths = { 25, 12, 12, 12, 12, 12, 12, 12 };
            setExcelExportColumnWidths(wks, widths);

            //Header Texts
            string[] headers = { "MSO", "Completed Designs", "Total Days to Complete", "Average Days to Complete",
                "Total Days From All Info Received", "Average Days From All Info Received", "Average Days P1", "Aveerage Days P2", "Average Days P3", "Canceled Designs" };
            placeHeaderTextInExport(wks, headers);

            formatExcelHeaderRow(wks);
        }
        public static void FormatCompletionTimeDGV(DataGridView dgv)
        {
            string[] headers = { "MSO", "Completed Designs","Total Days to Complete", "Average Days to Complete", "Total Days From All Info Received",
            "Average Days From All Info Received", "Average Days P1", "Average days P2", "Average Days P3", "Canceled Designs"};
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
            for (int i = 0; i < widths.Length - 1; i++)
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

            //int[] widths = { 10,220,140,140,130,130,130,100,150,90,70,140,180,200,
            //    140,140,100,120,100,100,120,100,100,140,100,100,150,100 };
            dgv.Columns[25].DefaultCellStyle.Format = "c";
            //SetDGV_ColumnWidths(dgv, widths);
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

            //int[] widths = { 10,220,140,140,130,130,130,100,150,90,70,140,180,200,
            //    140,140,100,120,100,100,120,100,100,140,100,100,150,100 };
            //SetDGV_ColumnWidths(dgv, widths);
            dgv.Columns[0].Visible = false;
            dgv.Columns[29].Visible = false;
            dgv.Columns["BOM_Value"].DefaultCellStyle.Format = "c";

            //string[] currencyCols = { "BOM_Value" };
            //setDGV_CellFormatToCurrency(dgv, currencyCols);
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
            "RFoG/ Pon Dollars","Fiber Deep Dollars", "Data Trans. Dollars","Other Dollars", "PEG Dollars","Commercial Dollars", "Unassigned Dollars" };

            //int[] widths = { 150, 100, 100, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 75, 75, 160,
            // 160,160,160,160,160,160,160,160};

            //SetDGV_ColumnWidths(dgv, widths);
            setDGV_HeaderText(dgv, headers);

            int[] currencyCols = { 1, 2, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 };
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

        public static SummaryModel DoWeeklySummary(DateTime start, DateTime end)
        {
            SummaryModel model = new SummaryModel();
            DateTime emptyDate = new DateTime(1900,1,1);
            DateTime startDate = start.Date;
            DateTime endDate = end.Date;
            int currentYear = DateTime.Now.Year;
            DateTime newYearsDay = new DateTime(currentYear, 1, 1);

            List<RequestModel> allRequests = GlobalConfig.Connection.DateRangeSearch_Unfiltered(newYearsDay, end);
            model.YTDassigned = allRequests.Count;
            model.YTDvalue = allRequests.Sum(x => x.BOM_Value);
            model.RequestsCompleted = allRequests.Where(x => x.DateCompleted >= start.Date 
                && x.DateCompleted <= endDate.Date).ToList().Count;  
            model.RequestsInPeriod = allRequests.Where(x => x.DateAssigned >= start.Date 
                && x.DateAssigned <= endDate.Date).ToList().Count;
            List<DesignerLoadModel> load = GlobalConfig.Connection.DoLoadReport();
            model.Backlog = load.Count;
            //model.Backlog*/ List<RequestModel> backlog  = allRequests.Where(x => x.DateCompleted.Date == emptyDate.Date && x.AwardStatus == "Pending" /*|| x.AwardStatus == "Won")*/).ToList()/*.Count*/;

            return model;
        }
    }
}
