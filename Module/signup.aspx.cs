using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class signup : System.Web.UI.Page
    {
        sysConnection dbcon;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            else
            {
                if (Request.RequestType == "POST")
                {
                    string username = Request.Form["name"];
                    string email = Request.Form["email"];
                    string password = Request.Form["password"];

                    this.insertProsesSignup(username, email, password);
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

        void insertProsesSignup(string username,string email,string password)
        {
            string[] checkpassword = password.Split(new Char[] { ',' });
            Boolean valid = true;
            if (!checkpassword[0].Equals(checkpassword[1]))
            {
                valid = false;

                var htmlDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                htmlDiv.Attributes.Add("class", "alert alert-danger");
                htmlDiv.Attributes.Add("role", "alert");
                htmlDiv.InnerHtml = "password not match";
                errmsg.Controls.Add(htmlDiv);
            }

            if (!this.IsValidEmailAddress(email))
            {
                var htmlDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                htmlDiv.Attributes.Add("class", "alert alert-danger");
                htmlDiv.Attributes.Add("role", "alert");
                htmlDiv.InnerHtml = "email invalid";
                errmsg.Controls.Add(htmlDiv);

                valid = false;
            }

            if (this.isUsernameDuplicate(username))
            {
                var htmlDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                htmlDiv.Attributes.Add("class", "alert alert-danger");
                htmlDiv.Attributes.Add("role", "alert");
                htmlDiv.InnerHtml = "username not available";
                errmsg.Controls.Add(htmlDiv);

                valid = false;
            }

            if (valid)
            {
                SqlParameter[] empparam = new SqlParameter[3];
                empparam[0] = new SqlParameter("@username", username);

                empparam[1] = new SqlParameter("@password", checkpassword[0]);

                empparam[2] = new SqlParameter("@email", email);

                string sql = "insert into sysuser (username,email,password,createddatetime,usergroupid) ";
                sql += " values (@username,@email,@password,NOW(),'USER') ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                Response.Redirect("signin.aspx");
            }
        }

        public bool IsValidEmailAddress(string email)
        {
            try
            {
                var emailChecked = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool isUsernameDuplicate(string username)
        {
            try
            {
                dbcon = new sysConnection();
                int countdata = Convert.ToInt32(dbcon.executeScalar(new sysSQLParam("select count(*) from sysuser where username = '"+username+"'",null)));
                dbcon.closeConnection();
                if (countdata != 0)
                    return true;
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