using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_UI
{
  
    public  enum Mode
    {
        New,
        Edit,
        Revision,
        Clone,
        Delete,
        Export,
        Restore,
        DateRangeSearch,
        Forecast,
        Report_OpenRequests,
        Report_CatMSO,
        Report_Snapshot,
        Report_AvgCompletion,
        Report_ByPriority,
        Report_DesignerLoadReport,
        Report_Overdue,
        Report_Rollup,
        Search,
        SearchFields,
        Undo,
        Log_Search,
        Log_View,
        Add_Attachment,
        Delete_Attachment,
        MSO_Maintenance,
        ReviewerMaintenance,
        DesignerMaintenance,
        AddMSO,
        EditMSO,
        None
    }
    
}
