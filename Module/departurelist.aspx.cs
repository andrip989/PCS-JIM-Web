using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PCS_JIM_Web.Module
{
    public partial class departurelist : reservationlist
    {
        protected override void initcombobox()
        {
            status.SelectedIndex = 2;
            status.Enabled = false;
        
        }

        public override string getQuery(string sqlwhere)
        {
            return "select t.*,s.*,s2.* from transaksiroom t " +
                                        "left join setupguestlist s on s.custcode = t.custcode " +
                                        "left join setuproom s2 on s2.noroom = t.noroom " +
                                        " " + sqlwhere + " order by t.departure asc,t.NoRoom,t.transaksiId ";
        }

    }
}