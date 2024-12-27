using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class cleanroomlist : System.Web.UI.Page
    {
        public sysConnection dbcon;
        public sysUserSession session;

        protected virtual void initcombobox()
        {
            status.AppendDataBoundItems = true;
            status.Items.Insert(1, new ListItem("Open", "-1"));
            status.Items.Insert(2, new ListItem("Cleaning", "0"));
            status.Items.Insert(3, new ListItem("Finish", "1"));
        }

        protected virtual void Page_Load(object sender, EventArgs e)
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
               

                this.initcombobox();
                loadTable();
            }
            //else
              //  loadTable();
        }

        public void loadTable()
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

                sqlwhere += "s2.floortype = '" + floortype.SelectedValue + "'  ";
            }            
            if (arrival.Text != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";

                sqlwhere += "t.arrival::Date >= '" + Convert.ToDateTime(arrival.Text).ToString("yyyy-MM-dd") + "'::Date  ";
            }
            if (departure.Text != "")            
            {
                if (sqlwhere != "") sqlwhere += " and ";

                sqlwhere += "t.departure::Date >= '" + Convert.ToDateTime(departure.Text).ToString("yyyy-MM-dd") + "'::Date  ";
            }
            if (status.SelectedValue != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";

                sqlwhere += "coalesce(t.status,-1::integer) = " + status.SelectedValue;
            }
            //else
            //{
            //    sqlwhere += "coalesce(t.status,-1::integer) <= 0";
            //}

            if (sqlwhere != "")
                sqlwhere = " where " + sqlwhere;

            string sql = "select t.*,s2.* from "+this.getTableQuery()+ " t " +
                                        "left join setuproom s2 on s2.noroom = t.noroom " +
                                        " " + sqlwhere + " order by t.departure desc,t.NoRoom,t.transid ";


            dt = dbcon.getdataTable(sql);

            dbcon.closeConnection();
            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

        public virtual string getTableQuery()
        {
            return "housekeepingroom";
        }

        public void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            loadTable();
        }

        public void submit_Click(object sender, EventArgs e)
        {
            loadTable();
        }

        public virtual string getstatusdesc(string status_)
        {
            string value = "";

            switch (status_)
            {
                case "0":
                    value = "Cleaning";
                    break;
                case "1":
                    value = "Finish";
                    break;
                case "2":
                    value = "Cancel";
                    break;
                default:
                    value = "Open";
                    break;
            }

            return value;
        }

        public virtual string getURLtrans()
        { 
            return ResolveUrl("~/Module/Submodule/cleantrans.aspx"); 
        }

        public void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int index = -1;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                index = sysfunction.GetColumnIndexByName(e.Row, "createddate");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "arrival");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "departure");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "updateddate");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "noroom");

                string noroom = e.Row.Cells[index].Text;

                index = sysfunction.GetColumnIndexByName(e.Row, "status");

                string status_ = e.Row.Cells[index].Text;

                e.Row.Cells[index].Text = this.getstatusdesc(status_);

                string uriparam = "";
                LinkButton myLink;
                if (status_ == "0" || status_ == "&nbsp;")
                {
                    index = sysfunction.GetColumnIndexByName(e.Row, "transid");

                    uriparam = this.getURLtrans();
                    uriparam += "?tipe=reservation&noroom=" + noroom + "&transid=" + e.Row.Cells[index].Text;

                    myLink = new LinkButton();
                    myLink.ID = "RoomBtn" + e.Row.Cells[index].Text;
                    myLink.Text = e.Row.Cells[index].Text;
                    myLink.OnClientClick = "openWindowchild('" + uriparam + "');";
                    myLink.Attributes.Add("href", "#");
                    e.Row.Cells[index].Controls.Add(myLink);
                    myLink = null;
                }                
            }
        }
    }
}