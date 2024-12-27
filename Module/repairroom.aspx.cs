using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class repairroom : cleanroom
    {
        public override void RedirecttoTrans(string recidparam, string switchsaklar)
        {
            Boolean isexec = true;

            isexec = false;
            string uriparam = ResolveUrl("~/Module/Submodule/repairtrans.aspx");
            uriparam += "?recidparam=" + recidparam;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "popupwindows", "openWindowchild('" + uriparam + "');", true);

            if (isexec)
                this.LoadOutlet();
        }

        public override string getquery()
        {
            return "select * from vwRoom " +
                    //" where noroom not in (select noroom from transaksiroom t where coalesce(t.status, -1::integer) < 1) " +
                    "order by floortype, noroom ";
        }
    }
}