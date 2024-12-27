using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class settledeposit : System.Web.UI.Page
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
                status.Items.Insert(4, new ListItem("Cancel", "2"));
                status.Items.Insert(5, new ListItem("No-Show", "3"));

                status.SelectedIndex = 3;

                statussettle.AppendDataBoundItems = true;
                statussettle.Items.Insert(1, new ListItem("Open", "0"));
                statussettle.Items.Insert(2, new ListItem("Closed", "1"));
                
                statussettle.SelectedIndex = 1;

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
            //else
            //{
            //    sqlwhere += "coalesce(t.status,-1::integer) <= 0";
            //}
            if(statussettle.SelectedValue != "")
            { 
                string operator_ = statussettle.SelectedValue == "0" ? "=" : "!="; 
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "coalesce(t.closedate,'1900-01-01'::date) "+ operator_ + " '1900-01-01'::date ";
            }

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
                /*
                index = sysfunction.GetColumnIndexByName(e.Row, "amountpaid");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));

                index = sysfunction.GetColumnIndexByName(e.Row, "deposit");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
                */
                index = sysfunction.GetColumnIndexByName(e.Row, "balance");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));

                index = sysfunction.GetColumnIndexByName(e.Row, "arrival");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "departure");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "closedate");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());
                string closedate_ = e.Row.Cells[index].Text;

                index = sysfunction.GetColumnIndexByName(e.Row, "settlepaydeposit");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));

                index = sysfunction.GetColumnIndexByName(e.Row, "closebalance");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));


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
                string uriparam = "";
                string transaksiid = "";
                LinkButton myLink;
                //if (status_ == "0" || status_ == "&nbsp;")
                {
                    index = sysfunction.GetColumnIndexByName(e.Row, "transaksiid");
                    transaksiid = e.Row.Cells[index].Text;
                    uriparam = ResolveUrl("~/Module/inputtrans.aspx");
                    uriparam += "?tipe=reservation&noroom=" + noroom + "&transid=" + e.Row.Cells[index].Text;

                    myLink = new LinkButton();
                    myLink.ID = "RoomBtn" + e.Row.Cells[index].Text;
                    myLink.Text = e.Row.Cells[index].Text;
                    myLink.OnClientClick = "openWindowchild('" + uriparam + "');";
                    myLink.Attributes.Add("href", "#");
                    e.Row.Cells[index].Controls.Add(myLink);
                    myLink = null;
                }

                index = sysfunction.GetColumnIndexByName(e.Row, "custcode");

                uriparam = ResolveUrl("~/Module/Submodule/custcreate.aspx");
                uriparam += "?custcodeparam=" + e.Row.Cells[index].Text;

                myLink = new LinkButton();
                myLink.ID = "RoomBtn" + e.Row.Cells[index].Text;
                myLink.Text = e.Row.Cells[index].Text;
                myLink.OnClientClick = "openWindowchild('" + uriparam + "');";
                myLink.Attributes.Add("href", "#");
                e.Row.Cells[index].Controls.Add(myLink);
                myLink = null;

                ImageButton cbchild = (ImageButton)e.Row.FindControl("btnchk");
                if (cbchild != null)
                {
                    if (status_ == "1")
                    {
                        cbchild.OnClientClick = closedate_ == "" || closedate_ == "&nbsp;" ? this.redirecttrans(transaksiid) :
                            "alert('transaksi ini sudah settle payment');";
                    }
                    else
                    {
                        cbchild.OnClientClick = "alert('transaksi ini belum checkout');";
                    }
                }
            }
        }

        public string redirecttrans(string transid)
        {
            string uriparam = ResolveUrl("~/Module/Submodule/settledeposittrans.aspx");
            uriparam += "?transid="+ transid;

            return "openWindowchild('" + uriparam + "');";
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
                var transid = clickedRow.Cells[0].Text;

            }

        }
    }
}