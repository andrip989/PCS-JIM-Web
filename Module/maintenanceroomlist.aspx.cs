using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PCS_JIM_Web.Module
{
    public partial class maintenanceroomlist : cleanroomlist
    {
        protected override void initcombobox()
        {
            status.AppendDataBoundItems = true;
            status.Items.Insert(1, new ListItem("Open", "-1"));
            status.Items.Insert(2, new ListItem("Repair", "0"));
            status.Items.Insert(3, new ListItem("Finish", "1"));
        }
        public override string getTableQuery()
        {
            return "maitenanceroom";
        }
        public override string getURLtrans()
        {
            return ResolveUrl("~/Module/Submodule/repairtrans.aspx");
        }

        public override string getstatusdesc(string status_)
        {
            string value = "";

            switch (status_)
            {
                case "0":
                    value = "Repair";
                    break;
                case "1":
                    value = "Finish";
                    break;
                case "2":
                    value = "Cancel";
                    break;
                default:
                    value = "Open";
                    break;
            }

            return value;
        }
    }
}