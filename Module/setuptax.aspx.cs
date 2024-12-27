using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class setuptax : System.Web.UI.Page
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

            if (session.Usergroupid != "admin")
            {
                submit.Visible = false;
                btncancel.Visible = false;
                btndelete.Visible = false;
            }

            dbcon = new sysConnection();

            if (!this.IsPostBack)
            {
                sysfunction.setGridStyle(GridView1);
                GridView1.AutoGenerateColumns = false;

                fromamount.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                fromamount.Attributes.Add("onblur", "formatCurrency(this,'blur');");
                toamount.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                toamount.Attributes.Add("onblur", "formatCurrency(this,'blur');");

                tax1.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                tax1.Attributes.Add("onblur", "formatCurrency(this,'blur');");
                tax2.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                tax2.Attributes.Add("onblur", "formatCurrency(this,'blur');");               

                this.loadTable();

            }
        }

        private string gettablename()
        {
            return "setuptax";
        }

        private void loadTable()
        {
            startdate.Text = "";
            fromamount.Text = "";
            toamount.Text = "";
            tax1.Text = "";
            tax2.Text = "";

            DataTable dt = dbcon.getdataTable("select * from " + this.gettablename() + " order by recid");
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

                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from " + this.gettablename() + " where recid = " + recidparam.Value + " ", null));
                if (objreader.Read())
                {                   
                    if (Convert.IsDBNull(objreader["startdate"]) == false)
                        startdate.Text = Convert.ToDateTime(objreader["startdate"]).ToString("yyyy-MM-dd");
                    if (Convert.IsDBNull(objreader["fromamount"]) == false)
                        fromamount.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["fromamount"].ToString()));
                    if (Convert.IsDBNull(objreader["toamount"]) == false)
                        toamount.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["toamount"].ToString()));
                    if (Convert.IsDBNull(objreader["tax1"]) == false)
                        tax1.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["tax1"].ToString()));
                    if (Convert.IsDBNull(objreader["tax2"]) == false)
                        tax2.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["tax2"].ToString()));
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

                string sqlinsert = "INSERT INTO "+this.gettablename()+" (startdate" +
                                                                    ", fromamount" +
                                                                    ", toamount" +
                                                                    ", tax1" +
                                                                    ", tax2" +
                                                                    ", createddate" +
                                                                    ", createdby) " +
                                                                    "VALUES(@startdate" +
                                                                    ", @fromamount" +
                                                                    ", @toamount" +
                                                                    ", @tax1" +
                                                                    ", @tax2" +
                                                                    ", @createddate" +
                                                                    ", @createdby)";
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@startdate", startdate.Text == "" ? new DateTime() : Convert.ToDateTime(startdate.Text)));
                list.Add(new SqlParameter("@fromamount", fromamount.Text == "" ? 0 : Convert.ToDecimal(fromamount.Text)));
                list.Add(new SqlParameter("@toamount", toamount.Text == "" ? 0 : Convert.ToDecimal(toamount.Text)));
                list.Add(new SqlParameter("@tax1", tax1.Text == "" ? 0 : Convert.ToDecimal(tax1.Text)));
                list.Add(new SqlParameter("@tax2", tax2.Text == "" ? 0 : Convert.ToDecimal(tax2.Text)));
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

                string sql = "UPDATE "+this.gettablename()+ " SET startdate=@startdate" +
                                                           ", fromamount=@fromamount" +
                                                           ", toamount=@toamount" +
                                                           ", tax1=@tax1" +
                                                           ", tax2=@tax2" +
                                                           ", updateddate=@updateddate" +
                                                           ", updatedby=@updatedby";
                sql += " where recid = @recid ";

                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@startdate", startdate.Text == "" ? new DateTime() : Convert.ToDateTime(startdate.Text)));
                list.Add(new SqlParameter("@fromamount", fromamount.Text == "" ? 0 : Convert.ToDecimal(fromamount.Text)));
                list.Add(new SqlParameter("@toamount", toamount.Text == "" ? 0 : Convert.ToDecimal(toamount.Text)));
                list.Add(new SqlParameter("@tax1", tax1.Text == "" ? 0 : Convert.ToDecimal(tax1.Text)));
                list.Add(new SqlParameter("@tax2", tax2.Text == "" ? 0 : Convert.ToDecimal(tax2.Text)));
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

                index = sysfunction.GetColumnIndexByName(e.Row, "startdate");

                e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "createddate");

                e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "updateddate");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "fromamount");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));

                index = sysfunction.GetColumnIndexByName(e.Row, "toamount");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));

                index = sysfunction.GetColumnIndexByName(e.Row, "tax1");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));

                index = sysfunction.GetColumnIndexByName(e.Row, "tax2");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));

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
                    dbcon.executeNonQuery(new sysSQLParam("delete from " + this.gettablename() + " where recid = " + columnvalue + " ", null));
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