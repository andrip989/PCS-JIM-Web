using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class setupseasontype : System.Web.UI.Page
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

                frommonth.DataSource = Enum.GetValues(typeof(Month));
                frommonth.AppendDataBoundItems = true;
                frommonth.DataBind();

                tomonth.DataSource = Enum.GetValues(typeof(Month));
                tomonth.AppendDataBoundItems = true;
                tomonth.DataBind();

                this.loadTable();

            }
        }

        private string gettablename()
        {
            return "setupseasontype";
        }

        private void loadTable()
        {
            seasontype.Text = "";
            description.Text = "";
            fromday.Text = "";
            frommonth.Text = "";
            today.Text = "";
            tomonth.Text = "";
            yearperiod.Text = "";
            active.Checked = false;

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
                    if (Convert.IsDBNull(objreader["seasontype"]) == false)
                        seasontype.Text = objreader["seasontype"].ToString();
                    if (Convert.IsDBNull(objreader["description"]) == false)
                        description.Text = objreader["description"].ToString();

                    if (Convert.IsDBNull(objreader["fromday"]) == false)
                        fromday.Text = objreader["fromday"].ToString();
                    if (Convert.IsDBNull(objreader["frommonth"]) == false)
                        frommonth.SelectedIndex = Convert.ToInt32(objreader["frommonth"]);
                    if (Convert.IsDBNull(objreader["today"]) == false)
                        today.Text = objreader["today"].ToString();
                    if (Convert.IsDBNull(objreader["tomonth"]) == false)
                        tomonth.SelectedIndex = Convert.ToInt32(objreader["tomonth"]);
                    if (Convert.IsDBNull(objreader["yearperiod"]) == false)
                        yearperiod.Text = objreader["yearperiod"].ToString();
                   
                    if (Convert.IsDBNull(objreader["active"]) == false)
                        active.Checked = Convert.ToInt16(objreader["active"]) == 1 ? true : false;
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

                string sqlinsert = "INSERT INTO " + this.gettablename() + " (seasontype" +
                                                                    ", description" +
                                                                    ", fromday" +
                                                                    ", frommonth" +
                                                                    ", today" +
                                                                    ", tomonth" +
                                                                    ", yearperiod" +
                                                                    ", active" +
                                                                    ", createddate" +
                                                                    ", createdby) " +
                                                                    "VALUES(@seasontype" +
                                                                    ", @description" +
                                                                    ", @fromday" +
                                                                    ", @frommonth" +
                                                                    ", @today" +
                                                                    ", @tomonth" +
                                                                    ", @yearperiod" +
                                                                    ", @active" +
                                                                    ", @createddate" +
                                                                    ", @createdby)";
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@seasontype", seasontype.Text));
                list.Add(new SqlParameter("@description", description.Text));
                list.Add(new SqlParameter("@fromday", fromday.Text == "" ? 0 : Convert.ToInt32(fromday.Text)));
                list.Add(new SqlParameter("@frommonth", frommonth.SelectedValue == "" ? 0 : Convert.ToInt32(frommonth.SelectedIndex)));
                list.Add(new SqlParameter("@today", today.Text == "" ? 0 : Convert.ToInt32(today.Text)));
                list.Add(new SqlParameter("@tomonth", tomonth.SelectedValue == "" ? 0 : Convert.ToInt32(tomonth.SelectedIndex)));
                list.Add(new SqlParameter("@yearperiod", yearperiod.Text == "" ? 0 : Convert.ToInt32(yearperiod.Text)));
                list.Add(new SqlParameter("@active", active.Checked ? 1 : 0));
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

                string sql = "UPDATE " + this.gettablename() + " SET seasontype=@seasontype" +
                                                           ", description=@description" +
                                                           ", fromday=@fromday" +
                                                           ", frommonth=@frommonth" +
                                                           ", today=@today" +
                                                           ", tomonth=@tomonth" +
                                                           ", yearperiod=@yearperiod" +
                                                           ", active=@active" +
                                                           ", updateddate=@updateddate" +
                                                           ", updatedby=@updatedby";
                sql += " where recid = @recid ";

                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@seasontype", seasontype.Text));
                list.Add(new SqlParameter("@description", description.Text));
                list.Add(new SqlParameter("@fromday", fromday.Text == "" ? 0 : Convert.ToInt32(fromday.Text)));
                list.Add(new SqlParameter("@frommonth", frommonth.SelectedValue == "" ? 0 : Convert.ToInt32(frommonth.SelectedIndex)));
                list.Add(new SqlParameter("@today", today.Text == "" ? 0 : Convert.ToInt32(today.Text)));
                list.Add(new SqlParameter("@tomonth", tomonth.SelectedValue == "" ? 0 : Convert.ToInt32(tomonth.SelectedIndex)));
                list.Add(new SqlParameter("@yearperiod", yearperiod.Text == "" ? 0 : Convert.ToInt32(yearperiod.Text)));
                list.Add(new SqlParameter("@active", active.Checked ? 1 : 0));
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

                index = sysfunction.GetColumnIndexByName(e.Row, "frommonth");

                e.Row.Cells[index].Text = Enum.Parse(typeof(Month), e.Row.Cells[index].Text).ToString();

                index = sysfunction.GetColumnIndexByName(e.Row, "tomonth");

                Month valuem;

                Enum.TryParse(e.Row.Cells[index].Text, out valuem);

                e.Row.Cells[index].Text = valuem.ToString();

                index = sysfunction.GetColumnIndexByName(e.Row, "createddate");

                e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "updateddate");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

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