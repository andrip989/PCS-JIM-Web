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
using System.Reflection;

namespace PCS_JIM_Web.Module
{
    public partial class setupratestype : System.Web.UI.Page
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

                //int index = sysfunction.GetColumnIndexByName(GridView1.Columns., "createddatetime");
                ((BoundField)GridView1.Columns[5]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";
                ((BoundField)GridView1.Columns[7]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";

                this.loadTable();

            }
        }       

        private void loadTable()
        {
            keterangan.Text = "";
            ratetypes.Text = "";
            hour.Text = "";
            daily.Text = "";

            DataTable dt = dbcon.getdataTable("select * from setupratetype order by createddate");
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

                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from setupratetype where recid = " + recidparam.Value + " ", null));
                if (objreader.Read())
                {
                    ratetypes.Text = objreader["ratetype"].ToString();
                    keterangan.Text = objreader["keterangan"].ToString();
                    hour.Text = objreader["hour"].ToString();
                    daily.Text = objreader["day"].ToString();
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
                SqlParameter[] empparam = new SqlParameter[5];
                empparam[0] = new SqlParameter("@ratetype", ratetypes.Text);

                empparam[1] = new SqlParameter("@keterangan", keterangan.Text);

                empparam[2] = new SqlParameter("@hour", Convert.ToInt32(hour.Text == "" ? "0" : hour.Text));
                empparam[3] = new SqlParameter("@day", Convert.ToInt32(daily.Text == "" ? "0" : daily.Text));

                empparam[4] = new SqlParameter("@createddate", DateTime.Now);

                string sql = "insert into setupratetype (ratetype,keterangan,hour,day,createdby,createddate) ";
                sql += " values (@ratetype,@keterangan,@hour,@day,'" + session.UserId + "',@createddate) ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                this.loadTable();
            }
            else if (Page.IsValid && submit.Text == "Update")
            {
                SqlParameter[] empparam = new SqlParameter[7];
                empparam[0] = new SqlParameter("@ratetype", ratetypes.Text);

                empparam[1] = new SqlParameter("@keterangan", keterangan.Text);

                empparam[2] = new SqlParameter("@hour", Convert.ToInt32(hour.Text == "" ? "0" : hour.Text));
                empparam[3] = new SqlParameter("@day", Convert.ToInt32(daily.Text == "" ? "0" : daily.Text));

                empparam[4] = new SqlParameter("@createddate", DateTime.Now);
                empparam[5] = new SqlParameter("@recid", Convert.ToInt64(recidparam.Value));
                empparam[6] = new SqlParameter("@updatedby", session.UserId);

                string sql = "update setupratetype set ratetype = @ratetype" +
                                                    ",keterangan = @keterangan" +
                                                    ",hour = @hour" +
                                                    ",day = @day" +
                                                    ",updateddate = @createddate" +
                                                    ",updatedby = @updatedby";                                                   
                sql += " where recid = @recid ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                this.loadTable();
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            loadTable();
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
                    dbcon.executeNonQuery(new sysSQLParam("delete from setupratetype where recid = " + columnvalue + " ", null));
                    dbcon.closeConnection();
                }
            }
            if (isexec)
                this.loadTable();
        }
    }
}