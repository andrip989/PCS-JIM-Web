using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class setuppaymenttype : System.Web.UI.Page
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

                this.loadTable();

            }
        }

        private string gettablename()
        {
            return "setuppaymenttype";
        }

        private void loadTable()
        {
            paymenttype.Text = "";
            description.Text = "";

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
                    if (Convert.IsDBNull(objreader["paymenttype"]) == false)
                        paymenttype.Text = objreader["paymenttype"].ToString();
                    if (Convert.IsDBNull(objreader["description"]) == false)
                        description.Text = objreader["description"].ToString();
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

                string sqlinsert = "INSERT INTO " + this.gettablename() + " (paymenttype" +
                                                                    ", description" +
                                                                    ", createddate" +
                                                                    ", createdby " +
                                                                    ", active)" + 
                                                                    "VALUES(@paymenttype" +
                                                                    ", @description" +
                                                                    ", @createddate" +
                                                                    ", @createdby" +
                                                                    ", @active)";
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@paymenttype", paymenttype.Text));
                list.Add(new SqlParameter("@description", description.Text));
                list.Add(new SqlParameter("@createddate", DateTime.Now));
                list.Add(new SqlParameter("@createdby", session.UserId));
                list.Add(new SqlParameter("@active", active.Checked ? 1 : 0));

                SqlParameter[] empparam = new SqlParameter[list.Count];

                empparam = list.ToArray();

                dbcon.executeNonQuery(new sysSQLParam(sqlinsert, empparam));
                dbcon.closeConnection();
                this.loadTable();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "alert(\"Save Success\");", true);
            }
            else if (Page.IsValid && submit.Text == "Update")
            {

                string sql = "UPDATE " + this.gettablename() + " SET paymenttype=@paymenttype" +
                                                           ", description=@description" +
                                                           ", updateddate=@updateddate" +
                                                           ", updatedby=@updatedby" +
                                                           ", active=@active";
                sql += " where recid = @recid ";

                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@paymenttype", paymenttype.Text));
                list.Add(new SqlParameter("@description", description.Text));
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