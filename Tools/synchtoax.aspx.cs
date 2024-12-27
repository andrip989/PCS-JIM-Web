using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Tools
{
    public partial class synchtoax : System.Web.UI.Page
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

                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from setuproom order by noroom ", null));
                int rowidx = 0;
                while (objreader.Read())
                {
                    if (rowidx == 0)
                    {
                        noroom.Items.Insert(rowidx, new ListItem("All", ""));
                        rowidx++;
                    }

                    noroom.Items.Insert(rowidx, new ListItem(objreader["noroom"].ToString() + " - " + objreader["description"].ToString()
                        + " (" + objreader["roomtypes"].ToString() + ") - " + objreader["floortype"].ToString()
                        , objreader["noroom"].ToString()));
                    rowidx++;
                }
                objreader.Close();
                dbcon.closeConnection();

                roomtypes.DataSource = dbcon.getdataTable("select * from setuproomtypes");
                dbcon.closeConnection();
                roomtypes.DataTextField = "keterangan";
                roomtypes.DataValueField = "roomtypes";
                roomtypes.AppendDataBoundItems = true;
                roomtypes.DataBind();

                floortype.DataSource = dbcon.getdataTable("select * from setupfloor");
                dbcon.closeConnection();
                floortype.DataTextField = "description";
                floortype.DataValueField = "floortype";
                floortype.AppendDataBoundItems = true;
                floortype.DataBind();

                businesscode.DataSource = dbcon.getdataTable("select * from setupbusinesssources");
                dbcon.closeConnection();
                businesscode.DataTextField = "companyname";
                businesscode.DataValueField = "alias";
                businesscode.AppendDataBoundItems = true;
                businesscode.DataBind();

                status.AppendDataBoundItems = true;
                status.Items.Insert(1, new ListItem("Open", "-1"));
                status.Items.Insert(2, new ListItem("Check - in", "0"));
                status.Items.Insert(3, new ListItem("Check - out", "1"));
                status.Items.Insert(3, new ListItem("Cancel", "2"));
                status.Items.Insert(3, new ListItem("No-Show", "3"));

                exportstatus.AppendDataBoundItems = true;
                exportstatus.Items.Insert(1, new ListItem("Not Export", "0"));
                exportstatus.Items.Insert(2, new ListItem("Export", "1"));
                loadTable();
            }
        }

        private void loadTable()
        {
            DataTable dt;

            string sqlwhere = "";

            if (noroom.SelectedValue != "")
            {
                sqlwhere += "t.noroom = '" + noroom.SelectedValue + "' ";
            }
            if (roomtypes.SelectedValue != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "s2.roomtypes = '" + roomtypes.SelectedValue + "' ";
            }
            if (floortype.SelectedValue != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "s2.floortype = '" + floortype.SelectedValue + "' ";
            }
            if (custcode.Text != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "t.custcode like '%" + custcode.Text + "%' ";
            }
            if (ktpid.Text != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "s.identificationid like '%" + ktpid.Text + "%' ";
            }
            if (transaksiid.Text != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "s.transaksiid like '%" + transaksiid.Text + "%' ";
            }
            if (firstname.Text != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "s.firstname like '%" + firstname.Text + "%' ";
            }
            if (lastname.Text != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "s.lastname like '%" + lastname.Text + "%' ";
            }
            if (businesscode.SelectedValue != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "s.businesscode = '" + businesscode.SelectedValue + "' ";
            }
            if (arrival.Text != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "t.arrival::Date >= '" + Convert.ToDateTime(arrival.Text).ToString("yyyy-MM-dd") + "'::Date ";
            }
            if (departure.Text != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "t.departure::Date >= '" + Convert.ToDateTime(departure.Text).ToString("yyyy-MM-dd") + "'::Date ";
            }
            if (status.SelectedValue != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "coalesce(t.status,-1::integer) = " + status.SelectedValue;
            }
            if (exportstatus.SelectedValue != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "coalesce(t.exportstatus,-1::integer) = " + exportstatus.SelectedValue;
            }
            //else
            //{
            //    sqlwhere += "coalesce(t.status,-1::integer) <= 0";
            //}

            if (sqlwhere != "")
                sqlwhere = " where " + sqlwhere;

            string sql = "select t.*,s.*,s2.* from transaksiroom t " +
                                        "left join setupguestlist s on s.custcode = t.custcode " +
                                        "left join setuproom s2 on s2.noroom = t.noroom " +
                                        " " + sqlwhere + " order by t.arrival desc,t.NoRoom,t.transaksiId ";


            dt = dbcon.getdataTable(sql);

            dbcon.closeConnection();
            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            loadTable();
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            loadTable();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int index = -1;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                index = sysfunction.GetColumnIndexByName(e.Row, "noroom");

                string noroom = e.Row.Cells[index].Text;

                index = sysfunction.GetColumnIndexByName(e.Row, "status");

                string status_ = e.Row.Cells[index].Text;

                switch (status_)
                {
                    case "0":
                        e.Row.Cells[index].Text = "Check - in";
                        break;
                    case "1":
                        e.Row.Cells[index].Text = "Check - out";
                        break;
                    case "2":
                        e.Row.Cells[index].Text = "Cancel";
                        break;
                    case "3":
                        e.Row.Cells[index].Text = "No-Show";
                        break;
                    default:
                        e.Row.Cells[index].Text = "Open";
                        break;
                }

                index = sysfunction.GetColumnIndexByName(e.Row, "exportstatus");

                status_ = e.Row.Cells[index].Text;

                switch (status_)
                {
                    case "0":
                        e.Row.Cells[index].Text = "Not Export";
                        break;
                    case "1":
                        e.Row.Cells[index].Text = "Export";
                        break;
                }

               
            }
        }

        protected void chkheader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)GridView1.HeaderRow.FindControl("chkheader");
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                CheckBox cbchild = (CheckBox)GridView1.Rows[i].FindControl("chk");
                string paramexport = ((HiddenField)GridView1.Rows[i].FindControl("paramexport")).Value;
                string paramstatus = ((HiddenField)GridView1.Rows[i].FindControl("paramstatus")).Value;
                if (paramexport == "0" && paramstatus == "1")
                {
                    cbchild.Checked = cb.Checked;
                }
                else
                    cbchild.Checked = false;
            }            
        }



        protected void btnexport_Click(object sender, EventArgs e)
        {
            Boolean isexec = false;
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                CheckBox cb = (CheckBox)GridView1.Rows[i].FindControl("chk");
                string columnvalue = ((HiddenField)GridView1.Rows[i].FindControl("recchk")).Value;

                if (cb.Checked == true)
                {
                    isexec = true;
                    string transid_ = GridView1.Rows[i].Cells[1].Text;
                    exportAX objAX = new exportAX(transid_,session);
                    objAX.autoposting = true;
                    objAX.CreateJournalAX();

                }
            }
            if (isexec)
                this.loadTable();
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            int rowind = ((GridViewRow)(sender as Control).NamingContainer).RowIndex;
            string columnvalue = ((HiddenField)GridView1.Rows[rowind].FindControl("recchk")).Value;

            CheckBox cb = (CheckBox)GridView1.Rows[rowind].FindControl("chk");
            if (cb.Checked)
            {
                string paramexport = ((HiddenField)GridView1.Rows[rowind].FindControl("paramexport")).Value;
                string paramstatus = ((HiddenField)GridView1.Rows[rowind].FindControl("paramstatus")).Value;
                if (paramexport == "0" && paramstatus == "1")
                {
                    cb.Checked = true;
                }
                else if (paramstatus != "1")
                {
                    cb.Checked = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "checkina", "alert('transaksi ini belum checkout!!');", true);
                }
                else
                {
                    cb.Checked = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "checkina", "alert('transaksi ini sudah di export!!');", true);
                }
            }
        }
    }
}