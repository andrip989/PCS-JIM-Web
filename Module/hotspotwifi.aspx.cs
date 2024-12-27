using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class hotspotwifi : System.Web.UI.Page
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

                status.AppendDataBoundItems = true;
                status.Items.Insert(1, new ListItem("Active", "0"));
                status.Items.Insert(2, new ListItem("Clear", "1"));

                status.SelectedIndex = 1;
                loadTable();
            }
            else
            {
                string parameter = Request["__EVENTARGUMENT"]; // parameter
                string value = Request["__EVENTTARGET"]; // Request["__EVENTTARGET"]; // btnSave

                if (parameter.Contains("refreshdata"))
                {
                    this.loadTable();
                }
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
            
            if (status.SelectedValue != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "coalesce(t.status,-1::integer) = " + status.SelectedValue;
            }          

            if (sqlwhere != "")
                sqlwhere = " where " + sqlwhere;

            string sql = "select t.*,s2.* from transaksidetailhotspot t " +
                                        "left join setuproom s2 on s2.noroom = t.noroom " +
                                        " " + sqlwhere + " order by t.checkin desc,t.NoRoom ";


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
                index = sysfunction.GetColumnIndexByName(e.Row, "checkin");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "checkout");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "noroom");

                string noroom = e.Row.Cells[index].Text;

                index = sysfunction.GetColumnIndexByName(e.Row, "status");

                string status_ = e.Row.Cells[index].Text;

                switch (status_)
                {
                    case "0":
                        e.Row.Cells[index].Text = "Active";
                        break;
                    case "1":
                        e.Row.Cells[index].Text = "Clear";
                        break;
                    default:
                        e.Row.Cells[index].Text = " ";
                        break;
                }

                ImageButton cbchild = (ImageButton)e.Row.FindControl("btnchk");
                cbchild.ImageUrl = status_ == "0" ? ResolveUrl("~/images/hotspoton.png") : ResolveUrl("~/images/hotspotoff.png");
                if (cbchild != null)
                    cbchild.OnClientClick = this.redirecttrans(noroom , status_ == "0" ? "Clear" : "Active");
            }
        }

        public string redirecttrans(string noroom_,string tipetrans_)
        {
            return "if( validationhotspot('"+ noroom_ + "','"+ tipetrans_ + "') ) { return true; } return false;";
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                // Retrieve the row index stored in the 
                // CommandArgument property.
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve the row that contains the button 
                // from the Rows collection.
                GridViewRow row = GridView1.Rows[index];

                // Add code here 
                var clickedButton = e.CommandSource as ImageButton;
                //find the row of the button
                var clickedRow = clickedButton.NamingContainer as GridViewRow;
                //now as the UserName is in the BoundField, access it using the cell index.
                var noroom = clickedRow.Cells[0].Text;
                var status = clickedRow.Cells[6].Text;
                var username_ = clickedRow.Cells[2].Text;
                var password_ = clickedRow.Cells[3].Text;
                var transaksiid = clickedRow.Cells[7].Text;
                if (status == "Active")
                {
                    try
                    {
                        sysCreateHotspot objh = new sysCreateHotspot();
                        objh.Username = noroom;
                        objh.HapusUser(noroom);
                    }
                    catch { }

                    SqlParameter[] empparam = new SqlParameter[2];

                    empparam[0] = new SqlParameter("@updatedby", session.UserId);

                    empparam[1] = new SqlParameter("@createddatetime", DateTime.Now);

                    dbcon.executeNonQuery(new sysSQLParam("update transaksidetailhotspot set status = 1" +
                                                      ",updateddate = @createddatetime,updatedby = @updatedby " +
                                                      " where noroom = '" + noroom + "' and coalesce(status, -1::integer) <= 0 and reftransid = '"+ transaksiid + "' ", empparam));
                    dbcon.closeConnection();
                }
                else
                {
                    try
                    {
                        sysCreateHotspot objh = new sysCreateHotspot();
                        objh.Username = noroom;
                        objh.Password = password_;
                        objh.Create();
                    }
                    catch { }

                    SqlParameter[] empparam = new SqlParameter[2];

                    empparam[0] = new SqlParameter("@updatedby", session.UserId);

                    empparam[1] = new SqlParameter("@createddatetime", DateTime.Now);

                    dbcon.executeNonQuery(new sysSQLParam("update transaksidetailhotspot set status = 0" +
                                                      ",updateddate = @createddatetime,updatedby = @updatedby " +
                                                      " where noroom = '" + noroom + "' and coalesce(status, -1::integer) = 1 and reftransid = '"+ transaksiid + "' ", empparam));
                    dbcon.closeConnection();
                }
                this.loadTable();
            }

        }
    }
}