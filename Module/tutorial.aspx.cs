using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCS_JIM_Web.Tools;

namespace PCS_JIM_Web.Module
{
    public partial class tutorial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string getUserid()
        {
            string url = ResolveUrl("~/Tools/showPDF.ashx?urlparam=");
            url =  url + Request.PhysicalApplicationPath + "ReportTemplate\\";
            url += "&userid=User Manual IFD Front Desk.pdf";
            return url;
        }

    }
}