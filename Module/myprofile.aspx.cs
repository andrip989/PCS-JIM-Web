using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCS_JIM_Web.Library;
using System.Web.Services.Description;

namespace PCS_JIM_Web.Module
{
    public partial class myprofile : System.Web.UI.Page
    {
        sysConnection dbcon;
        sysUserSession session;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["sessionid"] == null)
            {
                HttpContext.Current.Session["sessionid"] = "";
            }

            sysSecurity.checkUserSession(ref session, this.Server, HttpContext.Current.Session["sessionid"].ToString());

            dbcon = new sysConnection();

            if (!this.IsPostBack)
            {
                username.Text = session.UserId;
                password.Text = session.Password;
                fullname.Text = session.Fullname;
                email.Text = session.Email;

                FileUpload1.Attributes["onchange"] = "UploadFile(this)";
                this.reloadPicture();
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            string strQuery = "";
            strQuery = "update sysuser " +
                             " set password = @password " +
                             " , fullname = @fullname " +
                             " , email = @email " +
                             " where username = @username ";

            SqlParameter[] empparam = new SqlParameter[4];
            empparam[0] = new SqlParameter("@password", password.Text);
            empparam[1] = new SqlParameter("@fullname", fullname.Text);
            empparam[2] = new SqlParameter("@email", email.Text);
            empparam[3] = new SqlParameter("@username", session.UserId);

            dbcon.executeNonQuery(new sysSQLParam(strQuery, empparam));
            dbcon.closeConnection();
        }

        private void reloadPicture()
        {            
            object imageData = dbcon.executeScalar(new sysSQLParam("SELECT picprofile FROM sysuser WHERE username ='" + session.UserId + "'",null));
            dbcon.closeConnection();
            if (Convert.IsDBNull(imageData) == false)
            {
                byte[] bytes = (byte[])imageData;

                imagemyprofile.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(bytes, 0, bytes.Length);
            }
            else
                imagemyprofile.ImageUrl = ResolveUrl("images/in4.jpg");
        }

        protected void checkpass_CheckedChanged(object sender, EventArgs e)
        {            
            //if(ShowPassword.Checked)
            //    password.TextMode = TextBoxMode.SingleLine;
            //else
            //    password.TextMode = TextBoxMode.Password;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Stream fs = FileUpload1.PostedFile.InputStream;
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                string strQuery = "";
                strQuery = "update sysuser " +
                                 " set picprofile = @picprofile " +                                 
                                 " where username = @username ";

                SqlParameter[] empparam = new SqlParameter[2];
                empparam[0] = new SqlParameter("@picprofile", bytes);

                empparam[1] = new SqlParameter("@username", session.UserId);

                dbcon.executeNonQuery(new sysSQLParam(strQuery, empparam));
                dbcon.closeConnection();
                this.reloadPicture();
                
            }
            catch
            {}
        }
    }
}