using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignDB_Library.Operations
{
    public static class DesignerUpdate
    {
        public static void ChangePassword(string designer, string oldPW, string newPW)
        {
            List<DesignersReviewersModel> oldModelList = GlobalConfig.Connection.GetDesigner(designer);

            //TODO -- add code to handle list count > 1

            DesignersReviewersModel oldModel = oldModelList[0];
            DesignersReviewersModel newModel = oldModel;
            newModel.Pwd = newPW;

            if (oldModel.Pwd == newPW)
            {
                GlobalConfig.Connection.UpdateDesigner(newModel, "tblReviewers");
                MessageBox.Show("Password successfully changed");
            }
            else
            {
                MessageBox.Show("Incorrect password. Please retry");
            }
        }

        public static void UpdateDesigner(DesignersReviewersModel model)
        {
            GlobalConfig.Connection.UpdateDesigner(model, "tblReviewer");
        }
    }
}
