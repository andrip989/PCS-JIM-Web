using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using PCS_JIM_Web.Library;
using static System.Collections.Specialized.BitVector32;

namespace PCS_JIM_Web
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        sysConnection dbcon;
        public string usernameid = "";
        public sysUserSession session;
        public string UserName()
        {
            if (session.UserId != null)
                this.usernameid = session.UserId.ToString();
            return this.usernameid;
        }

        public string UserFullName()
        {
            string val = "";
            if (session.Fullname != null)
                val = string.Format("{0}",session.Fullname.ToString(), session.Usergroupid);
            else
                val = string.Format("{1}", session.Fullname.ToString(), session.Usergroupid);
            return val;
        }

        public string ApplicationTitle()
        {
            return sysConfig.IDFTitle();
        }

        public string ApplicationLokasi()
        {
            return sysConfig.IDFlokasi();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["sessionid"] == null)
            {
                HttpContext.Current.Session["sessionid"] = "";
            }

            sysSecurity.checkUserSession(ref session, this.Server, HttpContext.Current.Session["sessionid"].ToString());
            if (session.UserId != "")
            {
                dbcon = new sysConnection();
                object imageData = dbcon.executeScalar(new sysSQLParam("SELECT picprofile FROM sysuser WHERE username ='" + session.UserId + "'", null));
                dbcon.closeConnection();
                if (Convert.IsDBNull(imageData) == false)
                {
                    byte[] bytes = (byte[])imageData;

                    imageprofile.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(bytes, 0, bytes.Length);
                }
                else
                    imageprofile.ImageUrl = ResolveUrl("images/in4.jpg");
            }

            if(session.Statuscashier == 0)
            { 
                string uriparam = ResolveUrl("~/Module/Submodule/shiftopen.aspx");
                //uriparam += "?recidparam=" + recidparam;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "popupwindows", "openWindowcashier('" + uriparam + "');", true);
            }
        }

        public string getopenURL(string tipetrans)
        {
            string uriparam = "";
            if (tipetrans != "" && tipetrans != null)
                uriparam = ResolveUrl("~/Module/inputtrans.aspx?tipe="+ tipetrans);
            else
                uriparam = ResolveUrl("~/Module/inputtrans.aspx");
            return "openWindowchild('" + uriparam + "');";
        }

        public string ClosingShiftURL(string tipetrans)
        {
            string uriparam = ResolveUrl("~/Module/Submodule/shiftopen.aspx");
            uriparam += "?tipe=closing";

            return "openWindowcashier('" + uriparam + "');";
        }

        public HtmlGenericControl BodyTag
        {
            get
            {
                return this.bodymaster;
            }
        }

        public HtmlForm FormTag
        {
            get
            {
                return this.formBody;
            }
        }
    }
}