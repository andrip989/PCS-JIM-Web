using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using Npgsql;
using System.Web.UI.WebControls;
using PCS_JIM_Web.Library;
using System.Data.SqlClient;
using System.EnterpriseServices;
using Newtonsoft.Json;

namespace PCS_JIM_Web.Tools
{
    public partial class batchroom : System.Web.UI.Page
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

                ((BoundField)GridView1.Columns[1]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";
                ((BoundField)GridView1.Columns[2]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";

                ((BoundField)GridView1.Columns[4]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";
                ((BoundField)GridView1.Columns[5]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";

                sysfunction.setGridStyle(GridView2);
                GridView2.AutoGenerateColumns = false;

                ((BoundField)GridView2.Columns[1]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";
                ((BoundField)GridView2.Columns[2]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";
                ((BoundField)GridView2.Columns[4]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";

                sysfunction.setGridStyle(GridView3);
                GridView3.AutoGenerateColumns = false;

                ((BoundField)GridView3.Columns[1]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";
                ((BoundField)GridView3.Columns[2]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";
                ((BoundField)GridView3.Columns[4]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";

                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from setuproom order by noroom ", null));
                int rowidx = 0;
                while (objreader.Read())
                {
                    if (rowidx == 0)
                    {
                        noroom.Items.Insert(rowidx, new ListItem("--Select No.Room--", ""));
                        rowidx++;
                    }

                    noroom.Items.Insert(rowidx, new ListItem(objreader["noroom"].ToString() + " - " + objreader["description"].ToString()
                        + " (" + objreader["roomtypes"].ToString() + ") - "+ objreader["floortype"].ToString()
                        , objreader["noroom"].ToString()));
                    rowidx++;
                }
                objreader.Close();
                dbcon.closeConnection();

                this.loadTable();

            }
        }

        private void loadTable()
        {
            DataTable dt = dbcon.getdataTable("SELECT T1.* from transaksiroom T1 where coalesce(T1.status,0::integer) <= 0 order by arrival,noroom,departure");
            dbcon.closeConnection();
            GridView1.DataSource = dt;
            GridView1.DataBind();

            dt = dbcon.getdataTable("SELECT T1.* from housekeepingroom T1 where coalesce(T1.status,0::integer) <= 0 order by arrival,noroom,departure");
            dbcon.closeConnection();
            GridView2.DataSource = dt;
            GridView2.DataBind();

            dt = dbcon.getdataTable("SELECT T1.* from maitenanceroom T1 where coalesce(T1.status,0::integer) <= 0 order by arrival,noroom,departure");
            dbcon.closeConnection();
            GridView3.DataSource = dt;
            GridView3.DataBind();
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@noroom", noroom.SelectedValue));
            list.Add(new SqlParameter("@period", DateTime.Now));
            list.Add(new SqlParameter("@userid", session.UserId));

            SqlParameter[] empparam = new SqlParameter[list.Count];
            empparam = list.ToArray();
            
            dbcon.executeNonQuery(new sysSQLParam("select * from sp_prosesbatchroom(@noroom,@period,@userid)", empparam));
            dbcon.closeConnection();

            this.CheckinRoom();
            this.CheckOutRoom();
            
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertwarning", "alert('Sukses Proses');", true);

            this.loadTable();
        }

        private void CheckinRoom()
        {
            Boolean isexec = true;
            Int64 RecId = 0;
            sysConnection conWork = new sysConnection("DBConnectionBatch");
            sysConnection conWork2 = new sysConnection("DBConnectionBatch");
            NpgsqlDataReader objreader = conWork.executeQuery(new sysSQLParam("select T1.* from pcstelexa T1 where coalesce(T1.status, -1::integer) < 0 order by t1.arrival asc", null));
            while (objreader.Read())
            {
                RecId = Convert.ToInt64(objreader["recid"]);
                string outletnoparam = objreader["outletno"].ToString();
                string deviceidparam = objreader["deviceid"].ToString();
                string subdeviceidparam = objreader["subdeviceid"].ToString();
                string refnum = objreader["refnum"].ToString();
                string noroomv = objreader["noroom"].ToString();

                DateTime strdate = Convert.ToDateTime(objreader["arrival"]);
                DateTime enddate = Convert.ToDateTime(objreader["departure"]);

                string ipaddress = objreader["ipaddress"].ToString();
                string ipport = objreader["ipport"].ToString();
                if (strdate <= DateTime.Now)
                    isexec = true;
                else
                    isexec = false;

                RestClient rClient = new RestClient();
                rClient.endPoint = "http://" + ipaddress + ":" + ipport + "/" + "zeroconf/switches";

                rClient.httpMethod = httpVerb.POST;
                string triggersaklar = objreader["switch"].ToString();

                if (isexec)
                {
                    sysfunction.setTimeDevice(noroomv, ipaddress, ipport , deviceidparam);

                    rClient.postJSON = "{\r\n  \"deviceid\": \"" + deviceidparam + "\",\r\n  \"data\": {\r\n    \"subDevId\": \"" + subdeviceidparam + "\",\r\n    \"switches\": [\r\n      {\r\n        \"switch\": \"" + triggersaklar + "\",\r\n        \"outlet\": " + outletnoparam + "\r\n      }\r\n    ]\r\n  }\r\n}";

                    string strResponse = rClient.makeRequest();

                    var list = new List<SqlParameter>();

                    list.Add(new SqlParameter("@urlexec", rClient.endPoint));
                    list.Add(new SqlParameter("@json", rClient.postJSON));
                    list.Add(new SqlParameter("@updateddate", DateTime.Now));
                    list.Add(new SqlParameter("@updatedby", session.UserId));
                    SqlParameter[] empparam = new SqlParameter[list.Count];
                    empparam = list.ToArray();
                    rClient = null;
                    conWork2.executeNonQuery(new sysSQLParam("update pcstelexa set status = 0,urlexec = @urlexec" +
                                                            ",json = @json,updateddate = @updateddate,updatedby = @updatedby " +
                                                             "where RecId=" + Convert.ToString(RecId), empparam));
                    conWork2.closeConnection();

                    //set status jadi on / off
                    dbcon.executeNonQuery(new sysSQLParam("update setuproom set statusroom = '"+ triggersaklar + "' " +
                                                          ",updateddatetime = @updateddate,updatedby = @updatedby " +
                                                          "where noroom = '" + noroomv + "' and outletno='" + Convert.ToString(outletnoparam) + "'", empparam));
                    dbcon.closeConnection();
                }
            }
            objreader.Close();
            conWork.closeConnection();
        }

        private void CheckOutRoom()
        {
            Boolean isexec = true;
            Int64 RecId = 0;
            sysConnection conWork = new sysConnection("DBConnectionBatch");
            sysConnection conWork2 = new sysConnection("DBConnectionBatch");
            NpgsqlDataReader objreader = conWork.executeQuery(new sysSQLParam("select T1.* from pcstelexa T1 where coalesce(T1.status, -1::integer) = 0 order by t1.arrival asc", null));
            while (objreader.Read())
            {
                RecId = Convert.ToInt64(objreader["recid"]);
                string outletnoparam = objreader["outletno"].ToString();
                string deviceidparam = objreader["deviceid"].ToString();
                string subdeviceidparam = objreader["subdeviceid"].ToString();
                string refnum = objreader["refnum"].ToString();
                string noroomv = objreader["noroom"].ToString();
                string typetrans = objreader["typetrans"].ToString();

                DateTime strdate = Convert.ToDateTime(objreader["arrival"]);
                DateTime enddate = Convert.ToDateTime(objreader["departure"]);

                if (enddate <= DateTime.Now)
                    isexec = true;
                else
                    isexec = false;

                RestClient rClient = new RestClient();
                rClient.endPoint = "http://" + objreader["ipaddress"].ToString() + ":" + objreader["ipport"].ToString() + "/" + "zeroconf/switches";

                rClient.httpMethod = httpVerb.POST;
                string triggersaklar = "off";

                if (isexec)
                {
                    rClient.postJSON = "{\r\n  \"deviceid\": \"" + deviceidparam + "\",\r\n  \"data\": {\r\n    \"subDevId\": \"" + subdeviceidparam + "\",\r\n    \"switches\": [\r\n      {\r\n        \"switch\": \"" + triggersaklar + "\",\r\n        \"outlet\": " + outletnoparam + "\r\n      }\r\n    ]\r\n  }\r\n}";

                    string strResponse = rClient.makeRequest();
                    rClient = null;

                    var list = new List<SqlParameter>();

                    list.Add(new SqlParameter("@updateddate", DateTime.Now));
                    list.Add(new SqlParameter("@updatedby", session.UserId));
                    SqlParameter[] empparam = new SqlParameter[list.Count];
                    empparam = list.ToArray();

                    conWork2.executeNonQuery(new sysSQLParam("update pcstelexa set status = 1 " +
                                                             ",updateddate = @updateddate,updatedby = @updatedby " +
                                                             "where RecId=" + Convert.ToString(RecId), empparam));
                    conWork2.closeConnection();

                    if (typetrans == "housekeeping")
                    {
                        dbcon.executeNonQuery(new sysSQLParam("update setuproom set statusroom = '" + triggersaklar + "'" +
                                                          ",updateddatetime = @updateddate,updatedby = @updatedby " +
                                                          ",statushousekeeping = 0 where statushousekeeping = 1 and noroom = '" + noroomv + "' and outletno='" + Convert.ToString(outletnoparam) + "'", empparam));
                    }
                    else if (typetrans == "maintenance")
                    {
                        dbcon.executeNonQuery(new sysSQLParam("update setuproom set statusroom = '" + triggersaklar + "'" +
                                                          ",updateddatetime = @updateddate,updatedby = @updatedby " +
                                                          ",statusmaintenance = 0 where statusmaintenance = 1 and noroom = '" + noroomv + "' and outletno='" + Convert.ToString(outletnoparam) + "'", empparam));
                    }
                    else
                    { 
                        dbcon.executeNonQuery(new sysSQLParam("update setuproom set statusroom = '" + triggersaklar + "'" +
                                                          ",updateddatetime = @updateddate,updatedby = @updatedby " +
                                                          ",statushousekeeping = 1 where noroom = '" + noroomv + "' and outletno='" + Convert.ToString(outletnoparam) + "'", empparam));
                    }
                    dbcon.closeConnection();

                }
            }
            objreader.Close();
            conWork.closeConnection();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            loadTable();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int index = -1;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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
                    default:
                        e.Row.Cells[index].Text = "Open";
                        break;
                }
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            loadTable();
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int index = -1;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                index = sysfunction.GetColumnIndexByName(e.Row, "status");

                string status_ = e.Row.Cells[index].Text;

                switch (status_)
                {
                    case "0":
                        e.Row.Cells[index].Text = "Cleaning";
                        break;
                    case "1":
                        e.Row.Cells[index].Text = "Finish";
                        break;
                    default:
                        e.Row.Cells[index].Text = "Open";
                        break;
                }
            }
        }

        protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView3.PageIndex = e.NewPageIndex;
            loadTable();
        }

        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int index = -1;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                index = sysfunction.GetColumnIndexByName(e.Row, "status");

                string status_ = e.Row.Cells[index].Text;

                switch (status_)
                {
                    case "0":
                        e.Row.Cells[index].Text = "Cleaning";
                        break;
                    case "1":
                        e.Row.Cells[index].Text = "Finish";
                        break;
                    default:
                        e.Row.Cells[index].Text = "Open";
                        break;
                }
            }
        }
    }
}