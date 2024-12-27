using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class signin : System.Web.UI.Page
    {
        sysConnection dbcon;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
                {
                    txtUserName.Value = Request.Cookies["UserName"].Value;
                    txtPassword.Value = Request.Cookies["Password"].Value;
                    chkRememberMe.Checked = true;
                }
            }
            else
            {
                if (Request.RequestType == "POST")
                {
                    string username = txtUserName.Value;
                    string password = txtPassword.Value;

                    this.LoginManeng(username, password);
                }
            }
        }

        public string ApplicationTitle()
        {
            return sysConfig.IDFTitle();
        }

        public string ApplicationLokasi()
        {
            return sysConfig.IDFlokasi();
        }

        void LoginManeng(string username, string password)
        {
            Boolean valid = true;
            if (!this.logincheck(username, password))
            {
                valid = false;

                var htmlDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                htmlDiv.Attributes.Add("class", "alert alert-danger");
                htmlDiv.Attributes.Add("role", "alert");
                htmlDiv.InnerHtml = "username or password not match";
                errmsg.Controls.Add(htmlDiv);
            }

            if(valid)
            {
                /*
                sysMail obj = new sysMail();
                obj.ToAddress = "andrip@jimmail.com,yanto@jimmail.com,iwanh@jimmail.com";
                obj.Subject = "Login activity : " + this.ApplicationLokasi() + " by "+ username;
                obj.Body = DateTime.Now.ToString(sysConfig.DateTimeFormat());
                obj.SendMail();
                */
                Session["userid"] = username;
                Response.Redirect("~/Default.aspx");
            }
        }
        public bool logincheck(string username,string password)
        {
            try
            {
                dbcon = new sysConnection();
                int countdata = Convert.ToInt32(dbcon.executeScalar(new sysSQLParam("select count(*) from sysuser where username = '" + username + "' and password = '"+ password + "'", null)));
                dbcon.closeConnection();
                if (countdata != 0)
                {
                    string sessionid = Convert.ToString(dbcon.executeScalar(new sysSQLParam("SELECT setUserSession ('" + username + "') ", null)));
                    dbcon.closeConnection();
                    Session["sessionid"] = sessionid;

                    if (chkRememberMe.Checked)
                    {
                        Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                        Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);

                        Response.Cookies["UserName"].Value = username;
                        Response.Cookies["Password"].Value = password;
                    }
                    else
                    {
                        Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
                    }

                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}