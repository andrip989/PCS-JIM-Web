using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCS_JIM_Web.Library;
using Npgsql;
using static System.Collections.Specialized.BitVector32;
using System.Reflection;

namespace PCS_JIM_Web.Module
{
    public partial class outlet : System.Web.UI.Page
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

                deviceid.DataSource = GetDataFromDatabase("select * from setupdevice");
                deviceid.DataTextField = "description";
                deviceid.DataValueField = "deviceid";
                deviceid.AppendDataBoundItems = true;
                deviceid.DataBind();               
            }
            
        }

        private void loadTable()
        {
            description.Text = "";
            inputoutlet.Text = "";
            DataTable dt;
            //if (subdeviceid.SelectedValue != "")
            //    dt = GetDataFromDatabase("select * from vwoutlet ");// where deviceid = '" + deviceid.SelectedValue + "' and subdeviceid = '" + subdeviceid.SelectedValue + "' order by outletno ");
            //else
                dt = GetDataFromDatabase("select * from vwoutlet where deviceid = '" + deviceid.SelectedValue + "' order by outletno ");

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
        }

        DataTable GetDataFromDatabase(string SQLSyntax)
        {
            return dbcon.getdataTable(SQLSyntax);
        }

        protected void SaveClick(object sender, EventArgs e)
        {
            if (Page.IsValid && submit.Text == "Submit")
            {

                SqlParameter[] empparam = new SqlParameter[5];
                empparam[0] = new SqlParameter("@subdeviceid", subdeviceid.Text);

                empparam[1] = new SqlParameter("@description", description.Text);

                empparam[2] = new SqlParameter("@currentTimestamp", DateTime.Now);

                empparam[3] = new SqlParameter("@deviceid", deviceid.SelectedValue);

                empparam[4] = new SqlParameter("@outlet", inputoutlet.Text);

                string sql = "insert into setupoutlet (outletno,subdeviceid,description,createddate,createdby,createdtime,deviceid) ";
                sql += " values (@outlet,@subdeviceid,@description,NOW(),'admin',@currentTimestamp,@deviceid) ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                this.loadTable();
            }
            else if (Page.IsValid && submit.Text == "Update")
            {
                SqlParameter[] empparam = new SqlParameter[6];
                empparam[0] = new SqlParameter("@subdeviceid", subdeviceid.Text);

                empparam[1] = new SqlParameter("@description", description.Text);

                empparam[2] = new SqlParameter("@currentTimestamp", DateTime.Now);

                empparam[3] = new SqlParameter("@deviceid", deviceid.SelectedValue);

                empparam[4] = new SqlParameter("@outlet", inputoutlet.Text);

                empparam[5] = new SqlParameter("@recid", Convert.ToInt64(recidparam.Value));

                string sql = "update setupoutlet set outletno = @outlet" +
                                                    ",subdeviceid = @subdeviceid" +
                                                    ",description = @description" +
                                                    ",updatedatetime = NOW()" +
                                                    ",deviceid = @deviceid ";
                sql += " where recid = @recid ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                this.loadTable();
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int index = -1; // karena ada checkbox
            if (e.Row.RowType == DataControlRowType.Header)
            {
                index = sysfunction.GetColumnIndexByName(e.Row, "createddate");
                e.Row.Cells[index].Visible = false;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {                
                index = sysfunction.GetColumnIndexByName(e.Row, "createddate");

                e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateFormat());
                e.Row.Cells[index].Visible = false;

                index = sysfunction.GetColumnIndexByName(e.Row, "createdtime");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "updatedatetime");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());
            }
        }

        protected void deviceid_SelectedIndexChanged(object sender, EventArgs e)
        {
            subdeviceid.DataSource = GetDataFromDatabase("select * from setupsubdevice where deviceid = '" + deviceid.SelectedValue + "'");
            subdeviceid.DataTextField = "description";
            subdeviceid.DataValueField = "subdeviceid";
            subdeviceid.AppendDataBoundItems = true;
            subdeviceid.DataBind();

            this.loadTable();
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

                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from vwoutlet where recid = " + recidparam.Value + " ", null));
                if (objreader.Read())
                {
                    deviceid.SelectedValue = objreader["deviceid"].ToString();
                    subdeviceid.SelectedValue = objreader["subdeviceid"].ToString();
                    inputoutlet.Text = objreader["outletno"].ToString();
                    description.Text = objreader["description"].ToString();                    
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

        protected void subdeviceid_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadTable();
        }
    }
}