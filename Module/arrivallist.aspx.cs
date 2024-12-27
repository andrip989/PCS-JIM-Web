using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class arrivallist : reservationlist
    {
        protected override void initcombobox()
        {
            status.SelectedIndex = 1;
            status.Enabled = false;
        }

        public override string getQuery(string sqlwhere)
        {
            return "select t.*,s.*,s2.* from transaksiroom t " +
                                        "left join setupguestlist s on s.custcode = t.custcode " +
                                        "left join setuproom s2 on s2.noroom = t.noroom " +
                                        " " + sqlwhere + " order by t.arrival asc,t.NoRoom,t.transaksiId ";
        }
    }
}