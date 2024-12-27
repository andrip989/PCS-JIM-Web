using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;
using static log4net.Appender.RollingFileAppender;

namespace PCS_JIM_Web.Module
{
    public partial class reservationlist : System.Web.UI.Page
    {
        sysConnection dbcon;
        sysUserSession session;

        protected virtual void initcombobox()
        {
            
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

                this.initcombobox();
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

        private string getSQLWhere() 
        {
            string sqlwhere = "";

            string datearrs = arrival.Text;
            string datearrf = arrivalto.Text;

            datearrs = datearrs + " 00:00";
            datearrf = datearrf + " 23:59";

            string datedeparts = departure.Text;
            string datedepartf = departureto.Text;

            datedeparts = datedeparts + " 00:00";
            datedepartf = datedepartf + " 23:59";

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
                sqlwhere += "t.arrival >= '" + datearrs + "' ";
            }
            if (arrivalto.Text != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "t.arrival <= '" + datearrf + "' ";
            }
            if (departure.Text != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "t.departure >= '" + datedeparts + "' ";
            }
            if (departureto.Text != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "t.departure <= '" + datedepartf + "' ";
            }
            if (status.SelectedValue != "")
            {
                if (sqlwhere != "") sqlwhere += " and ";
                sqlwhere += "coalesce(t.status,-1::integer) = " + status.SelectedValue;
            }
            return sqlwhere;
        }
        private void loadTable()
        {
            DataTable dt;

            string sqlwhere = this.getSQLWhere();           

            if (sqlwhere != "")
                sqlwhere = " where " + sqlwhere;

            string sql = this.getQuery(sqlwhere);
            
            dt = dbcon.getdataTable(sql);

            dbcon.closeConnection();
            GridView1.DataSource = dt;
            GridView1.DataBind();
           
        }

        public virtual string getQuery(string sqlwhere)
        { 
            return "select t.*,s.*,s2.* from transaksiroom t " +
                                        "left join setupguestlist s on s.custcode = t.custcode " +
                                        "left join setuproom s2 on s2.noroom = t.noroom " +
                                        " " + sqlwhere + " order by t.arrival desc,t.NoRoom,t.transaksiId ";
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

                index = sysfunction.GetColumnIndexByName(e.Row, "createddatetime");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "arrival");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "departure");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "totalcharges");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));

                index = sysfunction.GetColumnIndexByName(e.Row, "deposit");
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
                LinkButton myLink;
                //if (status_ == "0" || status_ == "&nbsp;")
                {
                    index = sysfunction.GetColumnIndexByName(e.Row, "transaksiid");

                    uriparam = ResolveUrl("~/Module/inputtrans.aspx");
                    uriparam += "?tipe=reservation&noroom="+ noroom + "&transid=" + e.Row.Cells[index].Text;

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
            }
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

                string strScript = "alert('"+transid+"');";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), DateTime.Now.ToString() + "-" + (new Random()).Next().ToString(), strScript, true);

                string message = "This is test message";
                string jqueryCodeString = @"<script type='text/javascript'>ShowPopup('" + message + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(string), "Confirm1", jqueryCodeString, false);


                //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Call my function", "alert('aaa');", true);

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "popupwindows", strScript, true);
                /*
                sysReportRunner.openReport(this.Page
                                   , "ReportTemplate\\billinginvoice.rpt"
                                   , "Report Billing"
                                   , ""
                                   , "userid="+session.UserId+";transid="+ transid
                                   );
                */

            }
        }

        protected void btnchk_Click(object sender, ImageClickEventArgs e)
        {
            int rowind = ((GridViewRow)(sender as Control).NamingContainer).RowIndex;

            string columnvalue = ((HiddenField)GridView1.Rows[rowind].FindControl("paramtransid")).Value;

            //string strScript = "alert('" + columnvalue + "');";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), DateTime.Now.ToString() + "-" + (new Random()).Next().ToString(), strScript, true);
            sysReportRunner.openReport(this.Page
                                   , "ReportTemplate\\billinginvoice.rpt"
                                   , "Report Billing"
                                   , ""
                                   , "userid=" + session.UserId + ";transid=" + columnvalue
                                   );

        }

        protected void print_Click(object sender, EventArgs e)
        {

            string datearrs = arrival.Text;
            string datearrf = arrivalto.Text;

            string datedeparts = departure.Text;
            string datedepartf = departureto.Text;

            if (datearrs == "") datearrs = DateTime.MinValue.ToString("yyyy-MM-dd");
            if (datearrf == "") datearrf = DateTime.MaxValue.ToString("yyyy-MM-dd");

            if (datedeparts == "") datedeparts = DateTime.MinValue.ToString("yyyy-MM-dd");
            if (datedepartf == "") datedepartf = DateTime.MaxValue.ToString("yyyy-MM-dd");

            datearrs = datearrs + " 00:00";
            datearrf = datearrf + " 23:59";

            datedeparts = datedeparts + " 00:00";
            datedepartf = datedepartf + " 23:59";

            //string reportkriteria = "{ transaksiroom.arrival } >= '"+dates +"'";



            sysReportRunner.openReport(this.Page
                                   , "ReportTemplate\\laporanrekap.rpt"
                                   , "Report Laporan Penjualan Harian"
                                   , ""
                                   , "userid=" + session.UserId +";arrival="+ datearrs + ";arrivalto=" + datearrf + ";departure=" + datedeparts + ";departureto=" + datedepartf
                                   );
        }
    }
}