using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Report
{
    public partial class logdaily : System.Web.UI.Page
    {
        sysConnection dbcon;
        sysUserSession session;
        int kolomvoltage = 0;
        decimal totalvoltage = 0;
        int kolomrupiah = 0;
        decimal totalrupiah = 0;
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

                sysfunction.setGridStyle(GridView2);
                GridView2.AutoGenerateColumns = true;
                GridView2.ShowFooter = true;

                datetimestart.Text = DateTime.UtcNow.ToShortDateString();

                /*
                noroom.DataSource = dbcon.getdataTable("select * from vwRoom order by noroom");
                dbcon.closeConnection();
                noroom.DataTextField = "noroom";
                noroom.DataValueField = "noroom";
                noroom.DataBind();
                */

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


                datetimestart.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddT00:00");
                datetimeend.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddT23:59");

                sysfunction.setGridStyle(GridView1);

            }
        }

        private void loadTable()
        {
            
                string dates = datetimestart.Text;
                string datef = datetimeend.Text;

                dates = dates.Replace("T", " ");
                datef = datef.Replace("T", " ");
                
                string nomoroutlet = "";
                string deviceid = "";
                string subdeviceid = "";
                
                if (noroom.SelectedValue != "")
                { 
                    NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from vwroom where noroom = '" + noroom.SelectedValue + "' limit 1", null));
                    if (objreader.Read())
                    {
                        nomoroutlet = objreader["outletno"].ToString();
                        deviceid = objreader["deviceid"].ToString();
                        subdeviceid = objreader["subdeviceid"].ToString();
                    }
                    objreader.Close();
                    dbcon.closeConnection();
                }
                DataTable dt;

                if (nomoroutlet != "")
                    dt = dbcon.getdataTable("select * from vwlogdailydetail where trim(outletno) = '" + nomoroutlet + "' " +
                                                  " and datetime between '" + dates + "' and '" + datef + "'" +
                                                  " and deviceid = '" + deviceid + "' and subdeviceid = '" + subdeviceid + "'");
                else
                    dt = dbcon.getdataTable("select * from vwlogdailydetail where datetime between '" + dates + "' and '" + datef + "'");

                dbcon.closeConnection();
                GridView1.DataSource = dt;
                GridView1.DataBind();

                dates = Convert.ToDateTime(datetimestart.Text).ToString("yyyy-MM-dd");
                datef = Convert.ToDateTime(datetimeend.Text).ToString("yyyy-MM-dd");

                if (noroom.SelectedValue != "")
                {
                    dt = dbcon.getdataTable("select * from vwlogdaily v where \"Log Date\" between '" + dates + "' and '" + datef + "' " +
                                            "and noroom = '" + noroom.SelectedValue + "' order by noroom ");
                }
                else
                    dt = dbcon.getdataTable("select * from vwlogdaily v where \"Log Date\" between '" + dates + "' and '" + datef + "' " +
                                            "order by noroom ");
                dbcon.closeConnection();
                GridView2.DataSource = dt;
                GridView2.DataBind();
        }


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = -1; // karena ada checkbox

                index = sysfunction.GetColumnIndexByName(e.Row, "datetime");

                e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "current");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
                index = sysfunction.GetColumnIndexByName(e.Row, "voltage");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
                index = sysfunction.GetColumnIndexByName(e.Row, "realpower");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
                index = sysfunction.GetColumnIndexByName(e.Row, "reactivepower");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
                index = sysfunction.GetColumnIndexByName(e.Row, "apparentpower");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            loadTable();
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = -1; // karena ada checkbox

                index = sysfunction.GetColumnIndexByName(e.Row, "Log Date");

                e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "Total Current (A)");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
                index = sysfunction.GetColumnIndexByName(e.Row, "AVG voltage (V)");
                
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                { 
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
                }
                index = sysfunction.GetColumnIndexByName(e.Row, "Total Real Power (W)");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
                index = sysfunction.GetColumnIndexByName(e.Row, "Total Reactivepower (W)");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
                index = sysfunction.GetColumnIndexByName(e.Row, "Total Apparentpower (W)");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
                index = sysfunction.GetColumnIndexByName(e.Row, "Kilowatt hour (kWh)");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                {
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
                    totalvoltage += Convert.ToDecimal(e.Row.Cells[index].Text);
                }
                kolomvoltage = index;

                index = sysfunction.GetColumnIndexByName(e.Row, "Rupiah");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                {
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));
                    totalrupiah += Convert.ToDecimal(e.Row.Cells[index].Text);
                }
                kolomrupiah = index;
            }            
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = "Total";
                e.Row.Cells[kolomvoltage].Text = "<b>" +string.Format(sysConfig.CurrencyFormat(), totalvoltage) + "</b>";
                e.Row.Cells[kolomrupiah].Text = "<b>" + string.Format(sysConfig.CurrencyFormat(), totalrupiah) + "</b>";
            }
            
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            loadTable();
        }

        protected void listoutletno_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void InjetLogDaily()
        {
            string nomoroutlet = "";
            string deviceid = "";
            string subdeviceid = "";
            string ipaddress = "";
            string ipport = "";
            NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select outletno,deviceid,subdeviceid,ipaddress,ipport " +
                                                                                 "from vwroom s group by outletno,deviceid,subdeviceid,ipaddress,ipport ", null));
            while (objreader.Read())
            {
                nomoroutlet = objreader["outletno"].ToString();
                deviceid = objreader["deviceid"].ToString();
                subdeviceid = objreader["subdeviceid"].ToString();
                ipaddress = objreader["ipaddress"].ToString();
                ipport = objreader["ipport"].ToString();

                sysfunction.setTimeDevice(nomoroutlet, ipaddress, ipport, deviceid);
                RestClient rClient = new RestClient();
                //rClient.endPoint = sysConfig.apisonofflink() + "zeroconf/historicalData";
                rClient.endPoint = "http://" + ipaddress + ":" + ipport + "/zeroconf/historicalData";

                rClient.httpMethod = httpVerb.POST;
                /*
                DateTime period = DateTime.Now;
                string dates = period.ToString("yyyy-MM-dd");
                string datef = period.ToString("yyyy-MM-dd HH:mm");

                dates += " 00:00";
                */
                string dates = datetimestart.Text;
                string datef = datetimeend.Text;

                dates = dates.Replace("T", " ");
                datef = datef.Replace("T", " ");


                rClient.postJSON = "{\r\n  \"deviceid\": \"" + deviceid + "\",\r\n  \"data\": {\r\n    \"subDevId\": \"" + subdeviceid + "\",\r\n" +
                    "    \"outlet\": " + nomoroutlet + ",\r\n    \"dateStart\": \"" + dates + "\",\r\n    \"dateEnd\": \"" + datef + "\"\r\n  }\r\n}";

                string strResponse = rClient.makeRequest();

                string[] arrItemsPlanner = strResponse.Split(new string[] { "\n" }, StringSplitOptions.None);
                int i = 0;
                string sql = "";
                string[] columndata = { };
                foreach (string row in arrItemsPlanner)
                {
                    i++;
                    if (i > 1 && i <= (arrItemsPlanner.Length - 1))
                    {
                        string val = row;
                        if (row.Substring(0, 4) == "\0\0\0\0")
                        {
                            val = row.Replace("\0", "");
                        }

                        columndata = val.Split(new string[] { "," }, StringSplitOptions.None);

                        sysConnection conAppWork2 = new sysConnection();

                        sql = "select count(*) from logdaily " +
                                                                                            " where " +
                                                                                            " datetime = '" + columndata[0].TrimStart().TrimEnd() + " " + columndata[1].TrimStart().TrimEnd() + "' " +
                                                                                            " and trim(outletno) = '" + columndata[2].TrimStart().TrimEnd() + "' " +
                                                                                            " and deviceid = '" + deviceid + "' " +
                                                                                            " and subdeviceid = '" + subdeviceid + "'";

                        int countdata = Convert.ToInt32(conAppWork2.executeScalar(new sysSQLParam(sql, null)));
                        conAppWork2.closeConnection();
                        if (countdata == 0)
                        {
                            sql = "insert into logdaily (datetime,outletno,current,voltage,realpower,reactivepower,apparentpower,\"5minpowerconsumption\",deviceid,subdeviceid,createdby) ";
                            sql += " values ('" + columndata[0].TrimStart().TrimEnd() + " " + columndata[1].TrimStart().TrimEnd() + "'" +
                                                ",'" + columndata[2].TrimStart().TrimEnd() + "'" +
                                                "," + columndata[3].TrimStart().TrimEnd() + "" +
                                                "," + columndata[4].TrimStart().TrimEnd() + "" +
                                                "," + columndata[5].TrimStart().TrimEnd() + "" +
                                                "," + columndata[6].TrimStart().TrimEnd() + "" +
                                                "," + columndata[7].TrimStart().TrimEnd() + "" +
                                                "," + columndata[8].TrimStart().TrimEnd() + "" +
                                                ",'" + deviceid + "'" +
                                                ",'" + subdeviceid + "'" +
                                                ",'batch')";
                            conAppWork2.executeNonQuery(new sysSQLParam(sql, null));
                            conAppWork2.closeConnection();
                        }
                    }
                }



            }
            objreader.Close();
            dbcon.closeConnection();
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            if(injectapi.Checked)
                this.InjetLogDaily();

            loadTable();

        }
    }
}