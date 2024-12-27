using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class signout : System.Web.UI.Page
    {
        public string ApplicationTitle()
        {
            return sysConfig.IDFTitle();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sysConnection dbcon;

            dbcon = new sysConnection();

            dbcon.closeConnection();

            NpgsqlConnection.ClearPool(dbcon.Connection);

            Session.Clear();

            Response.Redirect("signin.aspx");
        }
    }
}