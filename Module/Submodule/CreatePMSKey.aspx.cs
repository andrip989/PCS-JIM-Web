using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module.Submodule
{
    public partial class CreatePMSKey : System.Web.UI.Page
    {
        sysConnection dbcon;
        public string usernameid = "";
        public sysUserSession session;

        public string UserName()
        {
            if (session.UserId != null)
                this.usernameid = session.UserId.ToString();
            return " - " + this.usernameid;
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
            if(this.IsPostBack)
            {
                string parameter = Request["__EVENTARGUMENT"]; // parameter
                string value = Request["__EVENTTARGET"]; // Request["__EVENTTARGET"]; // btnSave

                if (parameter.Contains("create"))
                {
                    //Thread.Sleep(5000);
                    this.btnCreate_Click(sender, e);
                }
            }
        }

        private void action(PMSType pMSType)
        {
            Uri myUri = new Uri(Request.Url.AbsoluteUri);

            string transid = HttpUtility.ParseQueryString(myUri.Query).Get("id");

            CardKeyPMS obj = new CardKeyPMS(transid);

            string tipePMS = HttpUtility.ParseQueryString(myUri.Query).Get("tipe");
            if (tipePMS != null && tipePMS != "")
            {
                switch (tipePMS)
                {
                    case "checkout":
                        obj.PMStype = PMSType.Checkout;
                        break;
                }
            }
            else
                obj.PMStype = pMSType;
            obj.Run();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            int tryCount = 3;

            if (tryCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(tryCount));

            while (true)
            {
                try
                {
                    action(PMSType.Create);
                    break; // success!
                }
                catch
                {
                    if (--tryCount == 0)
                        break;
                    Thread.Sleep(5000);
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);
        }
    }
}