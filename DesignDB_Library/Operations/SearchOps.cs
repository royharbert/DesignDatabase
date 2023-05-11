using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Operations
{
    public class SearchOps
    {
        public static List<RequestModel> FieldSearch(List<FieldSearchModel> searchTerms, bool isAnd)
        {
            StringBuilder whereClause = new StringBuilder();
            string op = "OR";
            if (isAnd)
            {
                op = "AND";
            }           
            
            foreach (FieldSearchModel item in searchTerms)
            {
                string delim = "'";

                //if (item.FieldName == "TotalHours" || item.FieldName == "BOM_Value" || item.FieldName == "PercentageProjectCovered")
                Type t = item.FieldValue.GetType();
                if (t.Name == "int")
                {
                    delim = "";
                }
                whereClause.Append(item.FieldName + " LIKE " + delim +"%" + item.FieldValue + "%" + delim + " " + op + " ");
            }
            string where = whereClause.ToString();
            int opLength = op.Length + 1;
            where = where.Substring(0, where.Length - opLength);
            List<RequestModel> requests = GlobalConfig.Connection.SearchMultipleFields(where);
            return requests;        
        }

        public static List<RequestModel> FieldSearch(List<FieldSearchModel> searchTerms, bool isAnd, DateTime startDate, DateTime endDate)
        {
            StringBuilder whereClause = new StringBuilder();
            string op = "OR";
            if (isAnd)
            {
                op = "AND";
            }

            foreach (FieldSearchModel item in searchTerms)
            {
                string delim = "'";

                //if (item.FieldName == "TotalHours" || item.FieldName == "BOM_Value" || item.FieldName == "PercentageProjectCovered")
                Type t = item.FieldValue.GetType();
                if (t.Name == "int")
                {
                    delim = "";
                }
                whereClause.Append(item.FieldName + " LIKE " + delim + "%" + item.FieldValue + "%" + delim + " " + op + " ");
            }
            string where = whereClause.ToString();
            int opLength = op.Length + 1;
            where = where.Substring(0, where.Length - opLength);
            where = where + " and DateAssigned between '" + startDate + "' and '" + endDate + "'";
            List<RequestModel> requests = GlobalConfig.Connection.SearchMultipleFields(where);
            return requests;
        }

    
    }
}

