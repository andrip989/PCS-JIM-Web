using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class selfcheckin : System.Web.UI.Page
    {
        public sysConnection dbcon;
        public sysUserSession session;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(HttpContext.Current.Session["sessionid"] == null)
            {
                HttpContext.Current.Session["sessionid"] = "";
            }

            sysSecurity.checkUserSession(ref session, this.Server, HttpContext.Current.Session["sessionid"].ToString());

            MasterPage masterpage = (MasterPage)this.Master;

            dbcon = new sysConnection();

            if (!IsPostBack)
            {
                labelbtn.Text = "Check - in";
            }
        }     

        protected void checkinbtn_ServerClick(object sender, EventArgs e)
        {
            int status = 0;
        }
    }
}