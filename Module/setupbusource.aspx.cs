using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Win32;
using Npgsql;
using PCS_JIM_Web.Library;
using static System.Collections.Specialized.BitVector32;
using System.Net;
using System.Drawing.Drawing2D;

namespace PCS_JIM_Web.Module
{
    public partial class setupbusource : System.Web.UI.Page
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
                sysfunction.setGridStyle(GridView1);
                GridView1.AutoGenerateColumns = false;

                valueplan.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                valueplan.Attributes.Add("onblur", "formatCurrency(this,'blur');");

                marketplace.DataSource = Enum.GetValues(typeof(MarketPlace));
                marketplace.AppendDataBoundItems = true; 
                marketplace.DataBind();
                
                creditcardtype.DataSource = Enum.GetValues(typeof(CreditCardType));
                creditcardtype.AppendDataBoundItems = true; 
                creditcardtype.DataBind();

                plantype.DataSource = sysfunction.PlanType(plantype);
                plantype.AppendDataBoundItems = true; 
                plantype.DataBind();

                this.loadTable();

                Tab1.CssClass = "Clicked";
                MainView.ActiveViewIndex = 0;
            }
        }

        protected void Tab1_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Clicked";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Initial";
            Tab4.CssClass = "Initial";
            Tab5.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;
        }

        protected void Tab2_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Clicked";
            Tab3.CssClass = "Initial";
            Tab4.CssClass = "Initial";
            Tab5.CssClass = "Initial";
            MainView.ActiveViewIndex = 1;
        }

        protected void Tab3_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Clicked";
            Tab4.CssClass = "Initial";
            Tab5.CssClass = "Initial";
            MainView.ActiveViewIndex = 2;
        }
        protected void Tab4_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Initial";
            Tab4.CssClass = "Clicked";
            Tab5.CssClass = "Initial";
            MainView.ActiveViewIndex = 3;
        }
        protected void Tab5_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Initial";
            Tab4.CssClass = "Initial";
            Tab5.CssClass = "Clicked";
            MainView.ActiveViewIndex = 4;
        }

        private string gettablename()
        {
            return "setupbusinesssources";
        }

        private void loadTable()
        {
            alias.Text = "";
            companyname.Text = "";
            firstname.Text = "";
            lastname.Text = "";
            description.Text = "";

            marketplace.SelectedValue = "";
            address.Text = "";
            address2.Text = "";
            city.Text = "";
            postalcode.Text = "";
            state.Text = "";
            country.Text = "";

            phone.Text = "";
            fax.Text = "";
            email.Text = "";
            website.Text = "";

            creditcardtype.SelectedValue = "";
            cardholder.Text = "";
            creditcardno.Text = "";
            ccexpdate.Text = "";
            iatano.Text = "";
            regno.Text = "";
            regno1.Text = "";
            regno2.Text = "";
            acnumber.Text = "";
            definespecialseason.Checked = false;
            definespecialrate.Checked = false;
            plantype.SelectedValue = "";
            valueplan.Text = "";
            term.Text = "";

            DataTable dt = dbcon.getdataTable("select * from "+ this.gettablename() + " order by recid");
            dbcon.closeConnection();
            GridView1.DataSource = dt;
            GridView1.DataBind();

            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                CheckBox cb = (CheckBox)GridView1.Rows[i].FindControl("chk");
                cb.Enabled = true;
                if (cb.Checked == true)
                    cb.Checked = false;
            }

            submit.Text = "Submit";
            submit.CssClass = "btn-primary btn";
            btndelete.Enabled = false;
        }

        protected void chkheader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)GridView1.HeaderRow.FindControl("chkheader");
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                CheckBox cbchild = (CheckBox)GridView1.Rows[i].FindControl("chk");
                cbchild.Checked = cb.Checked;
            }

            btndelete.Enabled = true;
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            int rowind = ((GridViewRow)(sender as Control).NamingContainer).RowIndex;

            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                if (rowind != i)
                {
                    CheckBox cba = (CheckBox)GridView1.Rows[i].FindControl("chk");
                    cba.Enabled = true;
                    if (cba.Checked == true)
                        cba.Checked = false;
                }
            }

            string columnvalue = ((HiddenField)GridView1.Rows[rowind].FindControl("recchk")).Value;

            CheckBox cb = (CheckBox)GridView1.Rows[rowind].FindControl("chk");
            if (cb.Checked)
            {
                recidparam.Value = columnvalue;
                submit.Text = "Update";
                submit.CssClass = "btn-success btn";
                btndelete.Enabled = true;
                
                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from "+ this.gettablename() + " where recid = " + recidparam.Value + " ", null));
                if (objreader.Read())
                {
                    if (Convert.IsDBNull(objreader["alias"]) == false)
                        alias.Text = objreader["alias"].ToString();
                    if (Convert.IsDBNull(objreader["companyname"]) == false)
                        companyname.Text = objreader["companyname"].ToString();
                    if (Convert.IsDBNull(objreader["firstname"]) == false) 
                        firstname.Text = objreader["firstname"].ToString();
                    if (Convert.IsDBNull(objreader["lastname"]) == false)
                        lastname.Text = objreader["lastname"].ToString();
                    
                    if (Convert.IsDBNull(objreader["description"]) == false)
                        description.Text = objreader["description"].ToString();
                    if (Convert.IsDBNull(objreader["marketplace"]) == false)
                        marketplace.SelectedValue = objreader["marketplace"].ToString();
                    if (Convert.IsDBNull(objreader["address"]) == false)
                        address.Text = objreader["address"].ToString();
                    if (Convert.IsDBNull(objreader["address2"]) == false)
                        address2.Text = objreader["address2"].ToString();

                    if (Convert.IsDBNull(objreader["city"]) == false)
                        city.Text = objreader["city"].ToString();
                    if (Convert.IsDBNull(objreader["postalcode"]) == false)
                        postalcode.Text = objreader["postalcode"].ToString();
                    if (Convert.IsDBNull(objreader["state"]) == false)
                        state.Text = objreader["state"].ToString();
                    if (Convert.IsDBNull(objreader["country"]) == false)
                        country.Text = objreader["country"].ToString();

                    if (Convert.IsDBNull(objreader["phone"]) == false)
                        phone.Text = objreader["phone"].ToString();
                    if (Convert.IsDBNull(objreader["fax"]) == false)
                        fax.Text = objreader["fax"].ToString();
                    if (Convert.IsDBNull(objreader["email"]) == false)
                        email.Text = objreader["email"].ToString();
                    if (Convert.IsDBNull(objreader["website"]) == false)
                        website.Text = objreader["website"].ToString();

                    if (Convert.IsDBNull(objreader["creditcardtype"]) == false)
                        creditcardtype.SelectedValue = objreader["creditcardtype"].ToString();
                    if (Convert.IsDBNull(objreader["cardholder"]) == false)
                        cardholder.Text = objreader["cardholder"].ToString();
                    if (Convert.IsDBNull(objreader["creditcardno"]) == false)
                        creditcardno.Text = objreader["creditcardno"].ToString();
                    if (Convert.IsDBNull(objreader["ccexpdate"]) == false)
                        ccexpdate.Text = Convert.ToDateTime(objreader["ccexpdate"]).ToString("yyyy-MM-dd");
                    if (Convert.IsDBNull(objreader["iatano"]) == false)
                        iatano.Text = objreader["iatano"].ToString();
                    if (Convert.IsDBNull(objreader["regno"]) == false)
                        regno.Text = objreader["regno"].ToString();
                    if (Convert.IsDBNull(objreader["regno1"]) == false)
                        regno1.Text = objreader["regno1"].ToString();
                    if (Convert.IsDBNull(objreader["regno2"]) == false)
                        regno2.Text = objreader["regno2"].ToString();

                    if (Convert.IsDBNull(objreader["acnumber"]) == false)
                        acnumber.Text = objreader["acnumber"].ToString();
                    if (Convert.IsDBNull(objreader["definespecialseason"]) == false)
                        definespecialseason.Checked = objreader["definespecialseason"].ToString() == "1" ? true : false;
                    if (Convert.IsDBNull(objreader["definespecialrate"]) == false)
                        definespecialrate.Checked = objreader["definespecialrate"].ToString() == "1" ? true : false;
                    if (Convert.IsDBNull(objreader["plantype"]) == false)
                        plantype.SelectedValue = objreader["plantype"].ToString();
                    if (Convert.IsDBNull(objreader["valueplan"]) == false)
                        valueplan.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["valueplan"].ToString()));
                    if (Convert.IsDBNull(objreader["term"]) == false)
                        term.Text = objreader["term"].ToString();

                }
                objreader.Close();
                dbcon.closeConnection();
                
            }
            cb.Enabled = false;
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            this.loadTable();
        }

        protected void SaveClick(object sender, EventArgs e)
        {
            if (Page.IsValid && submit.Text == "Submit")
            {

                string sqlinsert = "INSERT INTO setupbusinesssources (alias" +
                                                                    ", companyname" +
                                                                    ", firstname" +
                                                                    ", lastname" +
                                                                    ", description" +
                                                                    ", address" +
                                                                    ", address2" +
                                                                    ", city" +
                                                                    ", postalcode" +
                                                                    ", state" +
                                                                    ", country" +
                                                                    ", phone" +
                                                                    ", fax" +
                                                                    ", email" +
                                                                    ", website" +
                                                                    ", creditcardtype" +
                                                                    ", cardholder" +
                                                                    ", creditcardno" +
                                                                    ", ccexpdate" +
                                                                    ", iatano" +
                                                                    ", regno" +
                                                                    ", regno1" +
                                                                    ", regno2" +
                                                                    ", acnumber" +
                                                                    ", definespecialseason" +
                                                                    ", definespecialrate" +
                                                                    ", plantype" +
                                                                    ", valueplan" +
                                                                    ", term" +
                                                                    ", createddate, createdby" +
                                                                    ", marketplace) " +
                                                                    "VALUES(@alias" +
                                                                    ", @companyname" +
                                                                    ", @firstname" +
                                                                    ", @lastname" +
                                                                    ", @description" +
                                                                    ", @address" +
                                                                    ", @address2" +
                                                                    ", @city" +
                                                                    ", @postalcode" +
                                                                    ", @state" +
                                                                    ", @country" +
                                                                    ", @phone" +
                                                                    ", @fax" +
                                                                    ", @email" +
                                                                    ", @website" +
                                                                    ", @creditcardtype" +
                                                                    ", @cardholder" +
                                                                    ", @creditcardno" +
                                                                    ", @ccexpdate" +
                                                                    ", @iatano" +
                                                                    ", @regno" +
                                                                    ", @regno1" +
                                                                    ", @regno2" +
                                                                    ", @acnumber" +
                                                                    ", @definespecialseason" +
                                                                    ", @definespecialrate" +
                                                                    ", @plantype" +
                                                                    ", @valueplan" +
                                                                    ", @term" +
                                                                    ", @createddate, @createdby" +
                                                                    ", @marketplace)";
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@alias", alias.Text));
                list.Add(new SqlParameter("@companyname", companyname.Text));
                list.Add(new SqlParameter("@firstname", firstname.Text));
                list.Add(new SqlParameter("@lastname", lastname.Text));
                list.Add(new SqlParameter("@description", description.Text));
                list.Add(new SqlParameter("@address", address.Text));
                list.Add(new SqlParameter("@address2", address2.Text));
                list.Add(new SqlParameter("@city", city.Text));
                list.Add(new SqlParameter("@postalcode", postalcode.Text));
                list.Add(new SqlParameter("@state", state.Text));
                list.Add(new SqlParameter("@country", country.Text));
                list.Add(new SqlParameter("@phone", phone.Text));
                list.Add(new SqlParameter("@fax", fax.Text));
                list.Add(new SqlParameter("@email", email.Text));
                list.Add(new SqlParameter("@website", website.Text));
                list.Add(new SqlParameter("@creditcardtype", creditcardtype.SelectedValue));
                list.Add(new SqlParameter("@cardholder", cardholder.Text));
                list.Add(new SqlParameter("@creditcardno", creditcardno.Text));
                list.Add(new SqlParameter("@ccexpdate", ccexpdate.Text == "" ? new DateTime() : Convert.ToDateTime(ccexpdate.Text)));
                list.Add(new SqlParameter("@iatano", iatano.Text));
                list.Add(new SqlParameter("@regno", regno.Text));
                list.Add(new SqlParameter("@regno1", regno1.Text));
                list.Add(new SqlParameter("@regno2", regno2.Text));
                list.Add(new SqlParameter("@acnumber", acnumber.Text));
                list.Add(new SqlParameter("@definespecialseason", definespecialseason.Checked ? 1 : 0));
                list.Add(new SqlParameter("@definespecialrate", definespecialrate.Checked ? 1 : 0));
                list.Add(new SqlParameter("@plantype", plantype.SelectedValue));
                list.Add(new SqlParameter("@valueplan", valueplan.Text == "" ? 0 : Convert.ToDecimal(valueplan.Text)));
                list.Add(new SqlParameter("@term", term.Text == "" ? 0 : Convert.ToInt16(term.Text)));
                list.Add(new SqlParameter("@marketplace", marketplace.SelectedValue));
                list.Add(new SqlParameter("@createddate", DateTime.Now));
                list.Add(new SqlParameter("@createdby", session.UserId));

                SqlParameter[] empparam = new SqlParameter[list.Count];

                empparam = list.ToArray();
               
                dbcon.executeNonQuery(new sysSQLParam(sqlinsert, empparam));
                dbcon.closeConnection();
                this.loadTable();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "alert(\"Save Success\");", true);
            }
            else if (Page.IsValid && submit.Text == "Update")
            {

                string sql = "UPDATE setupbusinesssources SET alias=@alias" +
                                                           ", companyname=@companyname" +
                                                           ", firstname=@firstname" +
                                                           ", lastname=@lastname" +
                                                           ", description=@description" +
                                                           ", address=@address" +
                                                           ", address2=@address2" +
                                                           ", city=@city" +
                                                           ", postalcode=@postalcode" +
                                                           ", state=@state" +
                                                           ", country=@country" +
                                                           ", phone=@phone" +
                                                           ", fax=@fax" +
                                                           ", email=@email" +
                                                           ", website=@website" +
                                                           ", creditcardtype=@creditcardtype" +
                                                           ", cardholder=@cardholder" +
                                                           ", creditcardno=@creditcardno" +
                                                           ", ccexpdate=@ccexpdate" +
                                                           ", iatano=@iatano" +
                                                           ", regno=@regno" +
                                                           ", regno1=@regno1" +
                                                           ", regno2=@regno2" +
                                                           ", acnumber=@acnumber" +
                                                           ", definespecialseason=@definespecialseason" +
                                                           ", definespecialrate=@definespecialrate" +
                                                           ", plantype=@plantype" +
                                                           ", valueplan=@valueplan" +
                                                           ", term=@term" +
                                                           ", updateddate=@updateddate" +
                                                           ", updatedby=@updatedby" +
                                                           ", marketplace=@marketplace ";
                sql += " where recid = @recid ";

                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@alias", alias.Text));
                list.Add(new SqlParameter("@companyname", companyname.Text));
                list.Add(new SqlParameter("@firstname", firstname.Text));
                list.Add(new SqlParameter("@lastname", lastname.Text));
                list.Add(new SqlParameter("@description", description.Text));
                list.Add(new SqlParameter("@address", address.Text));
                list.Add(new SqlParameter("@address2", address2.Text));
                list.Add(new SqlParameter("@city", city.Text));
                list.Add(new SqlParameter("@postalcode", postalcode.Text));
                list.Add(new SqlParameter("@state", state.Text));
                list.Add(new SqlParameter("@country", country.Text));
                list.Add(new SqlParameter("@phone", phone.Text));
                list.Add(new SqlParameter("@fax", fax.Text));
                list.Add(new SqlParameter("@email", email.Text));
                list.Add(new SqlParameter("@website", website.Text));
                list.Add(new SqlParameter("@creditcardtype", creditcardtype.SelectedValue));
                list.Add(new SqlParameter("@cardholder", cardholder.Text));
                list.Add(new SqlParameter("@creditcardno", creditcardno.Text));
                list.Add(new SqlParameter("@ccexpdate", ccexpdate.Text == "" ? new DateTime() : Convert.ToDateTime(ccexpdate.Text)));
                list.Add(new SqlParameter("@iatano", iatano.Text));
                list.Add(new SqlParameter("@regno", regno.Text));
                list.Add(new SqlParameter("@regno1", regno1.Text));
                list.Add(new SqlParameter("@regno2", regno2.Text));
                list.Add(new SqlParameter("@acnumber", acnumber.Text));
                list.Add(new SqlParameter("@definespecialseason", definespecialseason.Checked ? 1 : 0));
                list.Add(new SqlParameter("@definespecialrate", definespecialrate.Checked ? 1 : 0));
                list.Add(new SqlParameter("@plantype", plantype.SelectedValue));
                list.Add(new SqlParameter("@valueplan", valueplan.Text == "" ? 0 : Convert.ToDecimal(valueplan.Text)));
                list.Add(new SqlParameter("@term", term.Text == "" ? 0 : Convert.ToInt16(term.Text)));
                list.Add(new SqlParameter("@marketplace", marketplace.SelectedValue));
                list.Add(new SqlParameter("@updateddate", DateTime.Now));
                list.Add(new SqlParameter("@updatedby", session.UserId));
                list.Add(new SqlParameter("@recid", Convert.ToInt64(recidparam.Value)));

                SqlParameter[] empparam = new SqlParameter[list.Count];

                empparam = list.ToArray();
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                this.loadTable();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "alert(\"Update Success\");", true);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            loadTable();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = -1; // karena ada checkbox

                index = sysfunction.GetColumnIndexByName(e.Row, "createddate");

                e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                //index = sysfunction.GetColumnIndexByName(e.Row, "updateddate");

                //if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                //    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "valueplan");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));

                index = sysfunction.GetColumnIndexByName(e.Row, "plantype");
                string status_ = e.Row.Cells[index].Text;

                switch (status_)
                {
                    case "1":
                        e.Row.Cells[index].Text = "of all night %";
                        break;
                    case "2":
                        e.Row.Cells[index].Text = "of first night %";
                        break;
                    case "3":
                        e.Row.Cells[index].Text = "fixed amount per night";
                        break;
                    case "4":
                        e.Row.Cells[index].Text = "fixed amount per stay";
                        break;
                    default:
                        e.Row.Cells[index].Text = " - ";
                        break;
                }

            }
        }

        protected void btndelete_Click(object sender, EventArgs e)
        {
            Boolean isexec = false;
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                CheckBox cb = (CheckBox)GridView1.Rows[i].FindControl("chk");
                string columnvalue = ((HiddenField)GridView1.Rows[i].FindControl("recchk")).Value;

                if (cb.Checked == true)
                {
                    isexec = true;
                    dbcon.executeNonQuery(new sysSQLParam("delete from "+ this.gettablename() +" where recid = " + columnvalue + " ", null));
                    dbcon.closeConnection();
                }
            }
            if (isexec)
            {
                this.loadTable();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "alert(\"Delete Success\");", true);
            }
        }
    }
}