using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.PerformanceData;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;
using Npgsql;
using PCS_JIM_Web.Library;
using System.Web.Services;
using System.Runtime.CompilerServices;

namespace PCS_JIM_Web.Module.Submodule
{
    public partial class custcreate : System.Web.UI.Page
    {
        sysConnection dbcon;
        sysUserSession session;
        int maximagesize = 2000;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["sessionid"] == null)
            {
                HttpContext.Current.Session["sessionid"] = "";
            }

            sysSecurity.checkUserSession(ref session, this.Server, HttpContext.Current.Session["sessionid"].ToString());

            Uri myUri = new Uri(Request.Url.AbsoluteUri);

            string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("custcodeparam");
            
            dbcon = new sysConnection();

            if (!this.IsPostBack)
            {
                creditlimit.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                discpct.Attributes.Add("onblur", "formatCurrency(this,'blur');");
                income.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                income.Attributes.Add("onblur", "formatCurrency(this,'blur');");

                statusblock.DataSource = Enum.GetValues(typeof(StatusBlock));
                statusblock.DataBind();

                paymenttype.DataSource = dbcon.getdataTable("select * from setuppaymenttype where active = 1");//Enum.GetValues(typeof(PaymentType));
                dbcon.closeConnection();
                paymenttype.DataTextField = "description";
                paymenttype.DataValueField = "paymenttype";
                paymenttype.AppendDataBoundItems = true;
                paymenttype.DataBind();

                identificationtype.DataSource = sysfunction.IdentificationType(identificationtype);
                identificationtype.AppendDataBoundItems = true;
                identificationtype.DataBind();

                guesttype.DataSource = sysfunction.GuestType(guesttype); 
                guesttype.AppendDataBoundItems = true;
                guesttype.DataBind();

                gender.DataSource = Enum.GetValues(typeof(GenderType));
                gender.DataBind();

                maritalstatus.DataSource = Enum.GetValues(typeof(MaritalStatus));
                maritalstatus.AppendDataBoundItems = true;
                maritalstatus.DataBind();

                bloodtype.DataSource = sysfunction.BloodType(bloodtype);
                bloodtype.AppendDataBoundItems = true;
                bloodtype.DataBind();


                heardfrom.DataSource = Enum.GetValues(typeof(HeardFrom));
                heardfrom.AppendDataBoundItems = true;
                heardfrom.DataBind();

                denial.DataSource = Enum.GetValues(typeof(HeardFrom));
                denial.AppendDataBoundItems = true;
                denial.DataBind();

                followup.DataSource = Enum.GetValues(typeof(Followup));
                followup.AppendDataBoundItems = true;
                followup.DataBind();

                businesscode.DataSource = dbcon.getdataTable("select * from setupbusinesssources");
                dbcon.closeConnection();
                businesscode.DataTextField = "companyname";
                businesscode.DataValueField = "alias";
                businesscode.AppendDataBoundItems = true;
                businesscode.DataBind();

                if (param1 != "" && param1 != null)
                {
                    custcodeparam.Value = param1;
                    FileUpload1.Attributes["onchange"] = "UploadFile(this)";
                    FileUpload2.Attributes["onchange"] = "UploadFile2(this)";
                    generatecodebtn.Visible = false;
                    this.loadcustcodeinfo();
                }
                else
                {
                    custcode.Text = lastNumberCode();

                    //FileUpload1.Style.Add("display", "none") ;
                    imagemyprofile.Visible = false;
                    webcam.Visible = true;
                    snapbtn.Visible = true;

                    ktpprofile.Visible = false;
                    webcam2.Visible = true;
                    snapbtn2.Visible = true;
                }
                Tab1.CssClass = "Clicked";
                MainView.ActiveViewIndex = 0;
            }
            else
            {
                if (Session["FileUpload1"] != null && (!FileUpload1.HasFile))
                {
                    FileUpload1 = (FileUpload)Session["FileUpload1"];
                }

                if (Session["FileUpload2"] != null && (!FileUpload2.HasFile))
                {
                    FileUpload2 = (FileUpload)Session["FileUpload2"];
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

        public string lastNumberCode()
        {
            try
            {
                dbcon = new sysConnection();
                string lastcustcode = Convert.ToString(dbcon.executeScalar(new sysSQLParam("select max(custcode) from setupguestlist ", null)));
                dbcon.closeConnection();
                Regex re = new Regex(@"\d+");
                Match m = re.Match(lastcustcode);
                string tampungstr = "";
                if (m.Success)
                {
                    tampungstr = lastcustcode.Replace(m.Value, "");

                    //int nomorformat = Regex.Matches(m.Value, Regex.Escape("0")).Count;

                    int nextseq = int.Parse(m.Value) + 1;

                    tampungstr  += string.Concat(Enumerable.Repeat("0", m.Value.Length - Convert.ToString(nextseq).Length)) + Convert.ToString(nextseq);

                }
                else
                {
                    tampungstr = lastcustcode;
                    tampungstr += string.Concat(Enumerable.Repeat("0", 5)) + "1";
                }

                return tampungstr;
            }
            catch
            {
                return "";
            }
        }

        [WebMethod(enableSession: true)]
        public static string SaveCapturedImage(string data,string tipe)
        {
            string fileName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");

            //Convert Base64 Encoded string to Byte Array.
            byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);
            /*
            if (imageBytes.Length > 2000)
            {
                imageBytes = sysfunction.ResizeImageFile(imageBytes, 2000);
            }
            */
            string ImageURL = Convert.ToBase64String(imageBytes, 0, imageBytes.Length);


            if (tipe == "0")
                HttpContext.Current.Session["FileUpload1data"] = "data:image/png;base64," + ImageURL;//Convert.ToBase64String(imageBytes, 0, imageBytes.Length);
            else
                HttpContext.Current.Session["FileUpload2data"] = "data:image/png;base64," + ImageURL;//Convert.ToBase64String(imageBytes, 0, imageBytes.Length);

            //Save the Byte Array as Image File.
            //string filePath = HttpContext.Current.Server.MapPath(string.Format("~/images/{0}.jpg", fileName));
            //File.WriteAllBytes(filePath, imageBytes);

                //access page control
            if (HttpContext.Current != null)
            {
                //Page page = (Page)HttpContext.Current.Handler;
                //System.Web.UI.WebControls.Image objimage = (System.Web.UI.WebControls.Image)page.FindControl("imagemyprofile");
                //objimage.ImageUrl = (string)HttpContext.Current.Session["FileUpload1data"];
                //objimage.Visible = true;
            }

            return "data:image/png;base64," + ImageURL;//Convert.ToBase64String(imageBytes, 0, imageBytes.Length);
        }

        protected void Tab1_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Clicked";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Initial";
            Tab4.CssClass = "Initial";
            Tab5.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;

            if (Session["FileUpload1"] == null && FileUpload1.HasFile)
            {
                Session["FileUpload1"] = FileUpload1;
            }

            if (Session["FileUpload2"] == null && FileUpload2.HasFile)
            {
                Session["FileUpload2"] = FileUpload2;
            }

        }

        protected void Tab2_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Clicked";
            Tab3.CssClass = "Initial";
            Tab4.CssClass = "Initial";
            Tab5.CssClass = "Initial";
            MainView.ActiveViewIndex = 1;

            if (Session["FileUpload1"] == null && FileUpload1.HasFile)
            {
                Session["FileUpload1"] = FileUpload1;
            }

            if (Session["FileUpload2"] == null && FileUpload2.HasFile)
            {
                Session["FileUpload2"] = FileUpload2;
            }
        }

        protected void Tab3_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Clicked";
            Tab4.CssClass = "Initial";
            Tab5.CssClass = "Initial";
            MainView.ActiveViewIndex = 2;

            if (Session["FileUpload1"] == null && FileUpload1.HasFile)
            {
                Session["FileUpload1"] = FileUpload1;
            }

            if (Session["FileUpload2"] == null && FileUpload2.HasFile)
            {
                Session["FileUpload2"] = FileUpload2;
            }
        }
        protected void Tab4_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Initial";
            Tab4.CssClass = "Clicked";
            Tab5.CssClass = "Initial";
            MainView.ActiveViewIndex = 3;

            if (Session["FileUpload1"] == null && FileUpload1.HasFile)
            {
                Session["FileUpload1"] = FileUpload1;
            }

            if (Session["FileUpload2"] == null && FileUpload2.HasFile)
            {
                Session["FileUpload2"] = FileUpload2;
            }
        }

        protected void Tab5_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Initial";
            Tab4.CssClass = "Initial";
            Tab5.CssClass = "Clicked";
            MainView.ActiveViewIndex = 4;

            if (Session["FileUpload1"] == null && FileUpload1.HasFile)
            {
                Session["FileUpload1"] = FileUpload1;
            }

            if (Session["FileUpload2"] == null && FileUpload2.HasFile)
            {
                Session["FileUpload2"] = FileUpload2;
            }
        }

        private void loadcustcodeinfo()
        {
            NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from setupguestlist where custcode = '" + custcodeparam.Value + "' limit 1 ", null));
            if (objreader.Read())
            {
                custcode.Text = objreader["custcode"].ToString();
                custcode.Enabled = false;
                firstname.Text = objreader["firstname"].ToString();
                lastname.Text = objreader["lastname"].ToString();
                identificationid.Text = objreader["identificationid"].ToString();
                phone.Text = objreader["phone"].ToString();
                email.Text = objreader["email"].ToString();
                discountcode.Text = objreader["discountcode"].ToString();
                if (Convert.IsDBNull(objreader["discpct"]) == false)
                    discpct.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["discpct"].ToString()));
                if (Convert.IsDBNull(objreader["creditlimit"]) == false)
                    creditlimit.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["creditlimit"].ToString()));
                npwp.Text = objreader["npwp"].ToString();
                telephone.Text = objreader["telephone"].ToString();
                createddate.Text = Convert.ToDateTime(objreader["createddate"]).ToString(sysConfig.DateTimeFormat());
                createdby.Text = objreader["createdby"].ToString();
                if (Convert.IsDBNull(objreader["updateddate"]) == false)
                {
                    updateddate.Text = Convert.ToDateTime(objreader["updateddate"]).ToString(sysConfig.DateTimeFormat());
                    updatedby.Text = objreader["updatedby"].ToString();
                }
                businesscode.SelectedValue = objreader["businesscode"].ToString();
                paymenttype.SelectedValue = objreader["paymenttype"].ToString();
                creditcardno.Text = objreader["creditcardno"].ToString();
                statusblock.SelectedValue = objreader["statusblock"].ToString();
                address.Text = objreader["address"].ToString();

                if (Convert.IsDBNull(objreader["picprofile"]) == false)
                {
                    //object imageData = objreader["picprofile"];
                    //byte[] bytes = (byte[])imageData;

                    imagemyprofile.ImageUrl = "data:image/png;base64," + objreader["picprofile"].ToString();//Convert.ToBase64String(bytes, 0, bytes.Length);
                }

                if (Convert.IsDBNull(objreader["ktpprofile"]) == false)
                {
                    //object imageData = objreader["ktpprofile"];
                    //byte[] bytes = (byte[])imageData;

                    ktpprofile.ImageUrl = "data:image/png;base64," + objreader["ktpprofile"].ToString();//Convert.ToBase64String(bytes, 0, bytes.Length);
                }

                gender.SelectedValue = objreader["gender"].ToString();
                vehiclelicenseplate.Text = objreader["vehiclelicenseplate"].ToString();
                vehiclecompany.Text = objreader["vehiclecompany"].ToString();
                vehiclemodel.Text = objreader["vehiclemodel"].ToString();
                vehiclecolor.Text = objreader["vehiclecolor"].ToString();
                vehicleyear.Text = objreader["vehicleyear"].ToString();

                street.Text = objreader["street"].ToString();
                city.Text = objreader["city"].ToString();
                state.Text = objreader["state"].ToString();
                zipcode.Text = objreader["zipcode"].ToString();
                country.Text = objreader["country"].ToString();

                identificationtype.SelectedValue = objreader["identificationtype"].ToString();
                if (Convert.IsDBNull(objreader["identificationexpdate"]) == false)
                    identificationexpdate.Text = Convert.ToDateTime(objreader["identificationexpdate"]).ToString("yyyy-MM-dd");
                guesttype.SelectedValue = objreader["guesttype"].ToString();
                if (Convert.IsDBNull(objreader["birthdate"]) == false)
                    birthdate.Text = Convert.ToDateTime(objreader["birthdate"]).ToString("yyyy-MM-dd");
                regno.Text = objreader["regno"].ToString();

                education.Text = objreader["education"].ToString();
                occupation.Text = objreader["occupation"].ToString();
                if (Convert.IsDBNull(objreader["income"]) == false)
                    income.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["income"].ToString()));

                bloodtype.SelectedValue = objreader["bloodtype"].ToString();
                allergies.Text = objreader["allergies"].ToString();

                nationality.Text = objreader["nationality"].ToString();
                language1.Text = objreader["language1"].ToString();
                language2.Text = objreader["language2"].ToString();

                officeadd.Text = objreader["officeadd"].ToString();
                office.Text = objreader["office"].ToString();
                residential.Text = objreader["residential"].ToString();
                fax.Text = objreader["fax"].ToString();
                website.Text = objreader["website"].ToString();
                followup.SelectedValue = objreader["followup"].ToString();
                heardfrom.SelectedValue = objreader["heardfrom"].ToString();
                denial.SelectedValue = objreader["denial"].ToString();

                maritalstatus.SelectedValue = objreader["maritalstatus"].ToString();
                if (Convert.IsDBNull(objreader["anniversarydate"]) == false)
                    anniversarydate.Text = Convert.ToDateTime(objreader["anniversarydate"]).ToString();
                children.Text = objreader["children"].ToString();
                if (Convert.IsDBNull(objreader["spousebirthdate"]) == false)
                    spousebirthdate.Text = Convert.ToDateTime(objreader["spousebirthdate"]).ToString();
            }

            objreader.Close();
            dbcon.closeConnection();

            submit.Text = "Update";

            if (session.Usergroupid == "admin")
            {
                btndelete.Visible = true;
            }            
            
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Stream fs;

                if (HttpContext.Current.Session["FileUpload1data"] != null)
                {
                    imagemyprofile.ImageUrl = (string)HttpContext.Current.Session["FileUpload1data"];
                    imagemyprofile.Visible = true;
                    webcam.Visible = false;
                    snapbtn.Visible = false;
                    HttpContext.Current.Session["FileUpload1data"] = null;
                }
                else if (HttpContext.Current.Session["FileUpload2data"] != null)
                {
                    ktpprofile.ImageUrl = (string)HttpContext.Current.Session["FileUpload2data"];
                    ktpprofile.Visible = true;
                    webcam2.Visible = false;
                    snapbtn2.Visible = false;
                    HttpContext.Current.Session["FileUpload2data"] = null;
                }

                if (FileUpload1.HasFile)
                {
                    fs = FileUpload1.PostedFile.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                
                    if(fs.Length > maximagesize)
                    { 
                        bytes = sysfunction.ResizeImageFile(bytes, maximagesize);
                    }
                    //buat debug
                    //File.WriteAllBytes(Server.MapPath("~/images/temp.jpg"), bytes);

                    string ImageURL = Convert.ToBase64String(bytes, 0, bytes.Length);
                    string strQuery = "";
                    strQuery = "update setupguestlist " +
                                     " set picprofile = @picprofile " +
                                     " where custcode = @custcode ";

                    SqlParameter[] empparam = new SqlParameter[2];
                    empparam[0] = new SqlParameter("@picprofile", ImageURL);

                    empparam[1] = new SqlParameter("@custcode", custcode.Text);

                    dbcon.executeNonQuery(new sysSQLParam(strQuery, empparam));
                    dbcon.closeConnection();

                    imagemyprofile.ImageUrl = "data:image/png;base64," + ImageURL;

                    Session["FileUpload1"] = null;
                }

               
                if (FileUpload2.HasFile)
                {
                    fs = FileUpload2.PostedFile.InputStream;

                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                    if (fs.Length > maximagesize)
                    {
                        bytes = sysfunction.ResizeImageFile(bytes, maximagesize);
                    }
                    //buat debug
                    //File.WriteAllBytes(Server.MapPath("~/images/temp.jpg"), bytes);
                    string ImageURL = Convert.ToBase64String(bytes, 0, bytes.Length);
                    string strQuery = "";
                    strQuery = "update setupguestlist " +
                                     " set ktpprofile = @picprofile " +
                                     " where custcode = @custcode ";

                    SqlParameter[] empparam = new SqlParameter[2];
                    empparam[0] = new SqlParameter("@picprofile", ImageURL);

                    empparam[1] = new SqlParameter("@custcode", custcode.Text);

                    dbcon.executeNonQuery(new sysSQLParam(strQuery, empparam));
                    dbcon.closeConnection();

                    ktpprofile.ImageUrl = "data:image/png;base64," + ImageURL;

                    Session["FileUpload2"] = null;
                }

            }
            catch
            { }
        }

        public bool isCustCodeDuplicate(string username)
        {
            try
            {
                dbcon = new sysConnection();
                int countdata = Convert.ToInt32(dbcon.executeScalar(new sysSQLParam("select count(*) from setupguestlist where custcode = '" + username + "'", null)));
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

        public bool isEmailDuplicate(string email,string custcodetext)
        {
            try
            {
                dbcon = new sysConnection();
                int countdata = Convert.ToInt32(dbcon.executeScalar(new sysSQLParam("select count(*) from setupguestlist where custcode != '"+ custcodetext + "' and email = '" + email + "'", null)));
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

        public bool isidentificationidDuplicate(string identificationid, string custcodetext)
        {
            try
            {
                dbcon = new sysConnection();
                int countdata = Convert.ToInt32(dbcon.executeScalar(new sysSQLParam("select count(*) from setupguestlist where custcode != '"+ custcodetext + "' and identificationid = '" + identificationid + "'", null)));
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
        

        protected void submit_Click(object sender, EventArgs e)
        {
            Boolean valid = false;
            if (custcode.Text == "" || firstname.Text == "")
            {
                valid = false;
                this.Tab1_Click(null, null);
                Page.Validate("valGroup3");
            }

            if (this.isCustCodeDuplicate(custcode.Text) && Page.IsValid && submit.Text != "Update")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "alert('Cust Code is not available !')", true);
                valid = false;
            }
            else if (this.isEmailDuplicate(email.Text,custcode.Text) && Page.IsValid && email.Text !="")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "alert('Email text already available !')", true);
                valid = false;
            }
            else if (this.isidentificationidDuplicate(identificationid.Text,custcode.Text) && Page.IsValid && identificationid.Text != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "alert('No Identitas already available !')", true);
                valid = false;
            }
            else if (Page.IsValid && submit.Text == "Update")
            {
                var list = new List<SqlParameter>();

                list.Add(new SqlParameter("@custcode", custcode.Text));
                list.Add(new SqlParameter("@firstname", firstname.Text));
                list.Add(new SqlParameter("@lastname", lastname.Text));
                list.Add(new SqlParameter("@identificationid", identificationid.Text));
                list.Add(new SqlParameter("@phone", phone.Text));
                list.Add(new SqlParameter("@email", email.Text));
                list.Add(new SqlParameter("@discountcode", discountcode.Text));
                list.Add(new SqlParameter("@discpct", discpct.Text == "" ? 0 : Convert.ToDecimal(discpct.Text)));
                list.Add(new SqlParameter("@npwp", npwp.Text));
                list.Add(new SqlParameter("@telephone", telephone.Text));
                list.Add(new SqlParameter("@updateddate", DateTime.Now));
                list.Add(new SqlParameter("@updatedby", session.UserId));
                list.Add(new SqlParameter("@businesscode", businesscode.SelectedValue));
                list.Add(new SqlParameter("@paymenttype", paymenttype.SelectedValue));
                list.Add(new SqlParameter("@creditcardno", creditcardno.Text));
                list.Add(new SqlParameter("@creditlimit", creditlimit.Text == "" ? 0 : Convert.ToDecimal(creditlimit.Text)));
                list.Add(new SqlParameter("@statusblock", statusblock.SelectedValue));
                list.Add(new SqlParameter("@address", address.Text));
                list.Add(new SqlParameter("@gender", gender.SelectedValue));

                list.Add(new SqlParameter("@vehiclelicenseplate", vehiclelicenseplate.Text));
                list.Add(new SqlParameter("@vehiclecompany", vehiclecompany.Text));
                list.Add(new SqlParameter("@vehiclemodel", vehiclemodel.Text));
                list.Add(new SqlParameter("@vehiclecolor", vehiclecolor.Text));
                list.Add(new SqlParameter("@vehicleyear", vehicleyear.Text == "" ? 0 : Convert.ToInt32(vehicleyear.Text)));

                list.Add(new SqlParameter("@street", street.Text));
                list.Add(new SqlParameter("@city", city.Text));
                list.Add(new SqlParameter("@state", state.Text));
                list.Add(new SqlParameter("@zipcode", zipcode.Text));
                list.Add(new SqlParameter("@country", country.Text));

                list.Add(new SqlParameter("@identificationtype", identificationtype.SelectedValue));
                list.Add(new SqlParameter("@identificationexpdate", identificationexpdate.Text == "" ? new DateTime() : Convert.ToDateTime(identificationexpdate.Text)));
                list.Add(new SqlParameter("@guesttype", guesttype.SelectedValue));
                list.Add(new SqlParameter("@birthdate", birthdate.Text == "" ? new DateTime() : Convert.ToDateTime(birthdate.Text)));
                list.Add(new SqlParameter("@regno", regno.Text));

                list.Add(new SqlParameter("@education", education.Text));
                list.Add(new SqlParameter("@occupation", occupation.Text));
                list.Add(new SqlParameter("@income", income.Text == "" ? 0 : Convert.ToDecimal(income.Text)));

                list.Add(new SqlParameter("@bloodtype", bloodtype.SelectedValue));
                list.Add(new SqlParameter("@allergies", allergies.Text));

                list.Add(new SqlParameter("@nationality", nationality.Text));
                list.Add(new SqlParameter("@language1", language1.Text));
                list.Add(new SqlParameter("@language2", language2.Text));

                if (imagemyprofile.ImageUrl != "")
                {
                    /*
                    Byte[] bytes = Convert.FromBase64String(imagemyprofile.ImageUrl.Split(',')[1]);

                    if (bytes.Length > 1000)
                    {
                        bytes = sysfunction.ResizeImageFile(bytes, 1000);
                    }
                    */
                    list.Add(new SqlParameter("@picprofile", imagemyprofile.ImageUrl.Split(',')[1]));//bytes));
                }
                else
                    list.Add(new SqlParameter("@picprofile", ""));//new byte[0]));

                if (ktpprofile.ImageUrl != "")
                {
                    /*
                    Byte[] bytes = Convert.FromBase64String(ktpprofile.ImageUrl.Split(',')[1]);

                    if (bytes.Length > 1000)
                    {
                        bytes = sysfunction.ResizeImageFile(bytes, 1000);
                    }
                    */
                    list.Add(new SqlParameter("@ktpprofile", ktpprofile.ImageUrl.Split(',')[1]));//bytes));
                }
                else
                    list.Add(new SqlParameter("@ktpprofile", ""));//new byte[0]));

                list.Add(new SqlParameter("@officeadd", officeadd.Text));
                list.Add(new SqlParameter("@office", office.Text));
                list.Add(new SqlParameter("@residential", residential.Text));
                list.Add(new SqlParameter("@fax", fax.Text));
                list.Add(new SqlParameter("@website", website.Text));
                list.Add(new SqlParameter("@followup", followup.SelectedValue));
                list.Add(new SqlParameter("@heardfrom", heardfrom.SelectedValue));
                list.Add(new SqlParameter("@denial", denial.SelectedValue));

                list.Add(new SqlParameter("@maritalstatus", maritalstatus.SelectedValue));
                list.Add(new SqlParameter("@anniversarydate", anniversarydate.Text == "" ? new DateTime() : Convert.ToDateTime(anniversarydate.Text)));
                list.Add(new SqlParameter("@children", children.Text == "" ? 0 : Convert.ToInt32(children.Text)));
                list.Add(new SqlParameter("@spousebirthdate", spousebirthdate.Text == "" ? new DateTime() : Convert.ToDateTime(spousebirthdate.Text)));

                SqlParameter[] empparam = new SqlParameter[list.Count];
                empparam = list.ToArray();

                string sql = " Update setupguestlist set firstname = @firstname, lastname = @lastname " +
                              "                 , identificationid = @identificationid, phone = @phone, email = @email " +
                              "                 , discountcode = @discountcode, discpct = @discpct " +
                              "                 , npwp = @npwp , telephone = @telephone " +
                              "                 , updateddate = @updateddate, updatedby = @updatedby " +
                              "                 , businesscode = @businesscode, paymenttype = @paymenttype, creditcardno = @creditcardno, creditlimit = @creditlimit  " +
                              "                 , statusblock = @statusblock, address = @address  " +
                              "                 , identificationtype = @identificationtype, identificationexpdate = @identificationexpdate  " +
                              "                 , guesttype = @guesttype, birthdate = @birthdate ,gender = @gender  " +
                              "                 , picprofile = @picprofile, ktpprofile = @ktpprofile  " +
                              "                 , street = @street, city = @city  " +
                              "                 , state = @state, zipcode = @zipcode  " +
                              "                 , regno = @regno, education = @education  " +
                              "                 , occupation = @occupation, income = @income  " +
                              "                 , bloodtype = @bloodtype, allergies = @allergies  " +
                              "                 , nationality = @nationality, language1 = @language1  " +
                              "                 , language2 = @language2, officeadd = @officeadd  " +
                              "                 , office = @office, residential = @residential  " +
                              "                 , fax = @fax, website = @website  " +
                              "                 , followup = @followup, heardfrom = @heardfrom  " +
                              "                 , denial = @denial, maritalstatus = @maritalstatus  " +
                              "                 , anniversarydate = @anniversarydate, children = @children  " +
                              "                 , spousebirthdate = @spousebirthdate, country = @country " +
                              "  where custcode = @custcode ";

                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                valid = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "checkina", "alert('Update GuestList berhasil');", true);
            }
            else if(Page.IsValid)
            {                
                //else 
                {
                    if (Session["FileUpload1"] != null && (!FileUpload1.HasFile))
                    {
                        FileUpload1 = (FileUpload)Session["FileUpload1"];
                    }

                    var list = new List<SqlParameter>();

                    list.Add(new SqlParameter("@custcode", custcode.Text));
                    list.Add(new SqlParameter("@firstname", firstname.Text));
                    list.Add(new SqlParameter("@lastname", lastname.Text));
                    list.Add(new SqlParameter("@identificationid", identificationid.Text));
                    list.Add(new SqlParameter("@phone", phone.Text));
                    list.Add(new SqlParameter("@email", email.Text));
                    list.Add(new SqlParameter("@discountcode", discountcode.Text));
                    list.Add(new SqlParameter("@discpct", discpct.Text == "" ? 0 : Convert.ToDecimal(discpct.Text)));
                    list.Add(new SqlParameter("@npwp", npwp.Text));
                    list.Add(new SqlParameter("@telephone", telephone.Text));
                    list.Add(new SqlParameter("@createddate", DateTime.Now));
                    list.Add(new SqlParameter("@createdby", session.UserId));
                    list.Add(new SqlParameter("@businesscode", businesscode.SelectedValue));
                    list.Add(new SqlParameter("@paymenttype", paymenttype.SelectedValue));
                    list.Add(new SqlParameter("@creditcardno", creditcardno.Text));
                    list.Add(new SqlParameter("@creditlimit", creditlimit.Text == "" ? 0 : Convert.ToDecimal(creditlimit.Text)));
                    list.Add(new SqlParameter("@statusblock", statusblock.SelectedValue));
                    list.Add(new SqlParameter("@address", address.Text));
                    if (FileUpload1.HasFile)
                    {
                        Stream fs = FileUpload1.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                        if (fs.Length > maximagesize)
                        {
                            bytes = sysfunction.ResizeImageFile(bytes, maximagesize);
                        }

                        string ImageURL = Convert.ToBase64String(bytes, 0, bytes.Length);

                        list.Add(new SqlParameter("@picprofile", ImageURL));//bytes));
                    }
                    else if (imagemyprofile.ImageUrl != "")
                    {
                        /*
                        Byte[] bytes = Convert.FromBase64String(imagemyprofile.ImageUrl.Split(',')[1]);

                        if (bytes.Length > 1000)
                        {
                            bytes = sysfunction.ResizeImageFile(bytes, 1000);
                        }
                        */
                        list.Add(new SqlParameter("@picprofile", imagemyprofile.ImageUrl.Split(',')[1]));//bytes));
                    }
                    else
                        list.Add(new SqlParameter("@picprofile", ""));//new byte[0]));

                    if (FileUpload2.HasFile)
                    {
                        Stream fs = FileUpload2.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                        if (fs.Length > maximagesize)
                        {
                            bytes = sysfunction.ResizeImageFile(bytes, maximagesize);
                        }

                        string ImageURL = Convert.ToBase64String(bytes, 0, bytes.Length);

                        list.Add(new SqlParameter("@ktpprofile", ImageURL));//bytes));
                    }
                    else if (ktpprofile.ImageUrl != "")
                    {
                        /*
                        Byte[] bytes = Convert.FromBase64String(ktpprofile.ImageUrl.Split(',')[1]);

                        if (bytes.Length > 1000)
                        {
                            bytes = sysfunction.ResizeImageFile(bytes, 1000);
                        }
                        */
                        list.Add(new SqlParameter("@ktpprofile", ktpprofile.ImageUrl.Split(',')[1]));//bytes));
                    }
                    else
                        list.Add(new SqlParameter("@ktpprofile", ""));//new byte[0]));

                    list.Add(new SqlParameter("@vehiclelicenseplate", vehiclelicenseplate.Text));
                    list.Add(new SqlParameter("@vehiclecompany", vehiclecompany.Text));
                    list.Add(new SqlParameter("@vehiclemodel", vehiclemodel.Text));
                    list.Add(new SqlParameter("@vehiclecolor", vehiclecolor.Text));
                    list.Add(new SqlParameter("@vehicleyear", vehicleyear.Text == "" ? 0 : Convert.ToInt32(vehicleyear.Text)));
                    list.Add(new SqlParameter("@gender", gender.SelectedValue));
                    list.Add(new SqlParameter("@street", street.Text));
                    list.Add(new SqlParameter("@city", city.Text));
                    list.Add(new SqlParameter("@state", state.Text));
                    list.Add(new SqlParameter("@zipcode", zipcode.Text));
                    list.Add(new SqlParameter("@country", country.Text));

                    list.Add(new SqlParameter("@identificationtype", identificationtype.SelectedValue));
                    list.Add(new SqlParameter("@identificationexpdate", identificationexpdate.Text == "" ? new DateTime() : Convert.ToDateTime(identificationexpdate.Text)));
                    list.Add(new SqlParameter("@guesttype", guesttype.SelectedValue));
                    list.Add(new SqlParameter("@birthdate", birthdate.Text == "" ? new DateTime() : Convert.ToDateTime(birthdate.Text)));
                    list.Add(new SqlParameter("@regno", regno.Text));

                    list.Add(new SqlParameter("@education", education.Text));
                    list.Add(new SqlParameter("@occupation", occupation.Text));
                    list.Add(new SqlParameter("@income", income.Text == "" ? 0 : Convert.ToDecimal(income.Text)));

                    list.Add(new SqlParameter("@bloodtype", bloodtype.SelectedValue));
                    list.Add(new SqlParameter("@allergies", allergies.Text));

                    list.Add(new SqlParameter("@nationality", nationality.Text));
                    list.Add(new SqlParameter("@language1", language1.Text));
                    list.Add(new SqlParameter("@language2", language2.Text));

                    list.Add(new SqlParameter("@officeadd", officeadd.Text));
                    list.Add(new SqlParameter("@office", office.Text));
                    list.Add(new SqlParameter("@residential", residential.Text));
                    list.Add(new SqlParameter("@fax", fax.Text));
                    list.Add(new SqlParameter("@website", website.Text));
                    list.Add(new SqlParameter("@followup", followup.SelectedValue));
                    list.Add(new SqlParameter("@heardfrom", heardfrom.SelectedValue));
                    list.Add(new SqlParameter("@denial", denial.SelectedValue));

                    list.Add(new SqlParameter("@maritalstatus", maritalstatus.SelectedValue));
                    list.Add(new SqlParameter("@anniversarydate", anniversarydate.Text == "" ? new DateTime() : Convert.ToDateTime(anniversarydate.Text)));
                    list.Add(new SqlParameter("@children", children.Text == "" ? 0 : Convert.ToInt32(children.Text)));
                    list.Add(new SqlParameter("@spousebirthdate", spousebirthdate.Text == "" ? new DateTime() : Convert.ToDateTime(spousebirthdate.Text)));

                    SqlParameter[] empparam = new SqlParameter[list.Count];
                    empparam = list.ToArray();

                    string sql =  " INSERT INTO setupguestlist(custcode, firstname, lastname " +
                                  "                 , identificationid, phone, email " +
                                  "                 , discountcode, discpct " +
                                  "                 , npwp, telephone " +
                                  "                 , createddate, createdby " +
                                  "                 , businesscode, paymenttype, creditcardno, creditlimit "+
                                  "                 , statusblock, address, picprofile" +
                                  "                 , vehiclelicenseplate, vehiclecompany, vehiclemodel" +
                                  "                 , vehiclecolor, vehicleyear, street" +
                                  "                 , city, state, zipcode" +
                                  "                 , country, identificationtype, identificationexpdate" +
                                  "                 , guesttype,birthdate,gender, regno" +
                                  "                 , education, occupation, income" +
                                  "                 , bloodtype, allergies, nationality" +
                                  "                 , language1, language2, officeadd" +
                                  "                 , office, residential, fax" +
                                  "                 , website, followup, heardfrom" +
                                  "                 , denial, maritalstatus, anniversarydate" +
                                  "                 , children, spousebirthdate , ktpprofile" +
                                  ") "+
                                  "  values " +
                                  "  (@custcode, @firstname, @lastname " +
                                  "  , @identificationid, @phone, @email " +
                                  "  , @discountcode, @discpct, @npwp " +
                                  "  , @telephone, @createddate, @createdby" +
                                  "  , @businesscode, @paymenttype,@creditcardno,@creditlimit " +
                                  "  , @statusblock, @address, @picprofile" +
                                  "  , @vehiclelicenseplate, @vehiclecompany, @vehiclemodel" +
                                  "  , @vehiclecolor, @vehicleyear, @street" +
                                  "  , @city, @state, @zipcode" +
                                  "  , @country, @identificationtype, @identificationexpdate" +
                                  "  , @guesttype, @birthdate, @gender, @regno" +
                                  "  , @education, @occupation, @income" +
                                  "  , @bloodtype, @allergies, @nationality" +
                                  "  , @language1, @language2, @officeadd" +
                                  "  , @office, @residential, @fax" +
                                  "  , @website, @followup, @heardfrom" +
                                  "  , @denial, @maritalstatus, @anniversarydate" +
                                  "  , @children, @spousebirthdate , @ktpprofile" +
                                  ") ";

                   dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                   dbcon.closeConnection();

                    valid = true;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "checkina", "alert('Insert GuestList berhasil');", true);
                }
            }
            if(valid)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);
        }

        protected void generatecode_Click(object sender, EventArgs e)
        {
            custcode.Text = lastNumberCode();
        }

        protected void retakecapture_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = sender as ImageButton;
            var namabutton = button.ClientID;
            if (namabutton == "gambarprofile")
            {
                imagemyprofile.ImageUrl = "";
                imagemyprofile.Visible = false;
                webcam.Visible = true;
                snapbtn.Visible = true;

            }
            else if (namabutton == "gambarktp")
            {
                ktpprofile.ImageUrl = "";
                ktpprofile.Visible = false;
                webcam2.Visible = true;
                snapbtn2.Visible = true;
            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;

            if (!FileUpload2.HasFile && ktpprofile.ImageUrl == "")
            {
                CustomValidator1.ErrorMessage = "Upload KTP terlebih dahulu !";
                CustomValidator1.BackColor = Color.Red;                
                args.IsValid = false;
            }
        }

        protected void btndelete_Click(object sender, EventArgs e)
        {
            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@custcode", custcode.Text));

            SqlParameter[] empparam = new SqlParameter[list.Count];
            empparam = list.ToArray();

            string sql = " delete from setupguestlist " +
                          "  where custcode = @custcode ";

            dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
            dbcon.closeConnection();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "checkina", "alert('Delete GuestList berhasil');", true);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);
        }
    }
}