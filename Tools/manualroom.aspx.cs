using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;
using Newtonsoft.Json;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Tools
{
    public partial class manualroom : System.Web.UI.Page
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

            MasterPage masterpage = (MasterPage)this.Master;

            masterpage.BodyTag.Attributes.Add("onFocus", "parent_disable();");
            masterpage.BodyTag.Attributes.Add("onclick", "parent_disable();");


            dbcon = new sysConnection();

            if (!IsPostBack)
            {
                this.LoadOutlet();
            }
            else
            {
                string parameter = Request["__EVENTARGUMENT"]; // parameter
                string value = Request["__EVENTTARGET"]; // Request["__EVENTTARGET"]; // btnSave

                if (value.Contains("buttonlink") && parameter != "reloadoutlet" && parameter != null)
                {
                    value = value.Replace("buttonlink", "");
                    value = value.Split(new string[] { ":" }, StringSplitOptions.None)[0];
                    
                    if (session.Usergroupid != "staff")
                        this.UpdateSonOFF(value, parameter);
                }

                if (parameter.Contains("reloadoutlet"))
                {
                    this.LoadOutlet();
                }
            }
        }


        private void UpdateSonOFF(string recidparam, string switchsaklar)
        {
            string alasan = "";
            string valueparam_ = switchsaklar;
            switchsaklar = valueparam_.Split(new string[] { ":" }, StringSplitOptions.None)[0];

            string triggersaklar = "on";
            if (switchsaklar == "on")
            {
                triggersaklar = "off";
            }

            sysfunction.SetONOFFOutlet(recidparam, switchsaklar);

            if (valueparam_.Split(new string[] { ":" }, StringSplitOptions.None).Length > 1)
            {
                alasan = valueparam_.Split(new string[] { ":" }, StringSplitOptions.None)[1];

                string sqltax = " select noroom from setuproom t where recid = " + recidparam;

                string noRoom_ = Convert.ToString(dbcon.executeScalar(new sysSQLParam(sqltax, null)));
                dbcon.closeConnection();

                sysZeptoMail obj = new sysZeptoMail();
                obj.ToAddress = "andrip@jimmail.com,yanto@jimmail.com,iwanh@jimmail.com";
                obj.Subject = "Manual On / Off Room - (" + triggersaklar + ") oleh " + session.UserId + " - " + sysConfig.IDFTitle() + " "+sysConfig.IDFlokasi();

                string value = "<table border=0>";
                value += "<tr> <td>No. Room           </td><td> : </td> <td> " + noRoom_ + "</td></tr>";
                value += "<tr> <td>Tanggal             </td><td> : </td> <td> " + DateTime.Now.ToString(sysConfig.DateTimeFormat()) + "</td></tr>";
                value += "<tr> <td>Alasan              </td><td> : </td> <td> " + alasan + "</td></tr>";
                value += "</table>";

                obj.Body = value;

                obj.SendMail();
            }

            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@updateddate", DateTime.Now));
            list.Add(new SqlParameter("@updatedby", session.UserId));
            SqlParameter[] empparam = new SqlParameter[list.Count];
            empparam = list.ToArray();
            dbcon.executeNonQuery(new sysSQLParam("update setuproom set statusroom = '" + triggersaklar + "' " +
                                                          ",updateddatetime = @updateddate,updatedby = @updatedby " +
                                                          "where recid = "+ recidparam, empparam));
            dbcon.closeConnection();

            this.LoadOutlet();
        }

        public void getinformationroom(ref string reservationname, ref int reserved, ref string duedatename, ref int dueout, string noroom)
        {
            DateTime periodnow = DateTime.Now;
            periodnow = Convert.ToDateTime(periodnow.ToString(sysConfig.DateFormat()));

            dbcon = new sysConnection();
            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@period", periodnow));
            list.Add(new SqlParameter("@noroom", noroom));
            SqlParameter[] empparam = new SqlParameter[list.Count];
            empparam = list.ToArray();
            NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select s.*,s1.* " +
                                                               " from transaksiroom s" +
                                                               " LEFT JOIN setupguestlist s1 ON s1.custcode::text = s.custcode::text " +
                                                               " where noroom = @noroom and " +
                                                               " (coalesce(s.status,-1::integer) = 0 and s.departure::date = @period or" +
                                                               " coalesce(s.status,-1::integer) < 0 and s.arrival::date = @period)", empparam));

            if (objreader.Read())
            {
                reservationname = objreader["firstname"].ToString();
                reserved = 1;
            }
            objreader.Close();
            dbcon.closeConnection();

            string timenow = DateTime.Now.ToString("HH:mm");
            list = new List<SqlParameter>();
            list.Add(new SqlParameter("@periodtime", Convert.ToDateTime(DateTime.Now)));
            list.Add(new SqlParameter("@period", Convert.ToDateTime(periodnow)));
            list.Add(new SqlParameter("@noroom", noroom));
            empparam = new SqlParameter[list.Count];
            empparam = list.ToArray();

            objreader = dbcon.executeQuery(new sysSQLParam("select * " +
                                                               " from transaksiroom s" +
                                                               " LEFT JOIN setupguestlist s1 ON s1.custcode::text = s.custcode::text " +
                                                               " where noroom = @noroom and s.arrival::date = @period " +
                                                               "and s.departure < @periodtime and s.status < 1", empparam));

            if (objreader.Read())
            {
                duedatename = objreader["firstname"].ToString();
                dueout = 1;
            }
            objreader.Close();
            dbcon.closeConnection();

        }

        public void LoadOutlet()
        {
            dbcon = new sysConnection();
            DataTable ListOutlet = dbcon.getdataTable("select * from vwRoom order by floortype ,noroom ");
            dbcon.closeConnection();
            mainpagediv.Controls.Clear();
            string tampungfloor = "";
            foreach (DataRow row in ListOutlet.Rows)
            {
                string reservationname = "";
                int reserved = 0;
                string duedatename = "";
                int dueout = 0;

                this.getinformationroom(ref reservationname, ref reserved, ref duedatename, ref dueout, row["noroom"].ToString());

                var htmlDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                htmlDiv.Attributes.Add("class", "col-md-2 agileinfo-bnt");
                htmlDiv.ID = "divhome" + row["recid"].ToString();

                LinkButton objaddbutton = new LinkButton();
                objaddbutton.ID = "buttonlink" + row["recid"].ToString() + ":" + row["noroom"].ToString();
                var htmlDiv2 = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                htmlDiv2.ID = "divhome2" + row["recid"].ToString() + ":" + row["noroom"].ToString();

                int outletno = Convert.ToInt32(row["outletno"]);

                DateTime arrival = DateTime.Now;
                arrival = Convert.ToDateTime(arrival.ToString(sysConfig.DateFormat()));
                Boolean adaorang = false;
                if (Convert.IsDBNull(row["arrival"]) == false)
                {
                    if (arrival == Convert.ToDateTime(Convert.ToDateTime(row["arrival"].ToString()).ToString(sysConfig.DateFormat())) || reservationname != "")
                        adaorang = true;
                }

                if (Convert.ToInt32(row["active"]) == 0)
                {
                    htmlDiv2.Attributes.Add("class", "bg-outoforder pv20 text-white fw600 text-center");
                }
                else
                {
                    if (row["statusroom"].ToString().ToLower() == "on" && adaorang)
                    {
                        if (dueout != 0)
                        {
                            htmlDiv2.Attributes.Add("class", "bg-duedate pv20 text-black fw600 text-center");
                        }
                        else
                            htmlDiv2.Attributes.Add("class", "bg-occupied pv20 text-white fw600 text-center");
                    }
                    else
                    {
                        if (Convert.ToInt32(row["statushousekeeping"]) == 1 && Convert.ToInt32(row["statusmaintenance"]) == 0)
                        {
                            htmlDiv2.Attributes.Add("class", "bg-dirty pv20 text-white fw600 text-center");
                        }
                        else if (Convert.ToInt32(row["statusmaintenance"]) == 1)
                        {
                            htmlDiv2.Attributes.Add("class", "bg-outoforder pv20 text-black fw600 text-center");
                        }
                        else if (reserved != 0)
                        {
                            htmlDiv2.Attributes.Add("class", "bg-reserved pv20 text-white fw600 text-center");
                        }
                        else if (dueout != 0)
                        {
                            htmlDiv2.Attributes.Add("class", "bg-duedate pv20 text-white fw600 text-center");
                        }
                        else
                            htmlDiv2.Attributes.Add("class", "bg-vacant pv20 text-white fw600 text-center");
                    }
                }

                if (session.Usergroupid.ToLower() == "staff")
                {
                    objaddbutton.OnClientClick = "alert('no access security')";
                    objaddbutton.Attributes.Add("href", "#");
                }
                else
                    objaddbutton.OnClientClick = "clickoutletevent('" + objaddbutton.ID + "','" + row["statusroom"].ToString().ToLower() + "')";

                if (Convert.IsDBNull(row["firstname"]) == false && adaorang)
                {
                    htmlDiv2.InnerHtml = row["noroom"].ToString() + "(" + row["description"].ToString() + ")" + "<br>" + row["firstname"].ToString();
                }
                else if (reservationname != "")
                {
                    htmlDiv2.InnerHtml = row["noroom"].ToString() + "(" + row["description"].ToString() + ")" + "<br>" + reservationname;
                }
                else if (duedatename != "")
                {
                    htmlDiv2.InnerHtml = row["noroom"].ToString() + "(" + row["description"].ToString() + ")" + "<br>" + duedatename;
                }
                else
                {
                    htmlDiv2.InnerHtml = row["noroom"].ToString() + "(" + row["description"].ToString() + ")" + "<br>";
                }

                htmlDiv2.InnerHtml += "<br>";

                if (adaorang)
                {
                    htmlDiv2.InnerHtml += "<i class=\"fa fa-user\" aria-hidden=\"true\"></i>&nbsp;";
                }

                if (row["statusroom"].ToString().ToLower() == "on")
                {
                    htmlDiv2.InnerHtml += "<i class=\"fa  fa-lightbulb-o\" aria-hidden=\"true\"></i>&nbsp;";
                }

                if (Convert.ToInt32(row["statushousekeeping"]) == 1)
                {
                    //htmlDiv2.InnerHtml += "<i class=\"glyphicon glyphicon-bed\" aria-hidden=\"true\"></i>";
                    htmlDiv2.InnerHtml += "<img src=\"" + ResolveUrl("~/images/broom.png") + "\">";
                }
                else
                    htmlDiv2.InnerHtml += "&nbsp;&nbsp;&nbsp;";

                if (Convert.ToInt32(row["statusmaintenance"]) == 1)
                {
                    htmlDiv2.InnerHtml += "&nbsp;" + "<i class=\"fa fa-wrench\" aria-hidden=\"true\"></i>";
                }
                else
                    htmlDiv2.InnerHtml += "&nbsp;&nbsp;&nbsp;";

                if (Convert.ToInt32(row["allowsmoking"]) == 1)
                {
                    htmlDiv2.InnerHtml += "&nbsp;" + "<img src=\"" + ResolveUrl("~/images/smoking.png") + "\">";
                }

                objaddbutton.Controls.Add(htmlDiv2);

                htmlDiv.Controls.Add(objaddbutton);

                if (tampungfloor != row["floortype"].ToString())
                {
                    if (tampungfloor != "")
                    {
                        var htmldivclear = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                        htmldivclear.Attributes.Add("class", "clearfix");
                        mainpagediv.Controls.Add(htmldivclear);
                    }
                    var htmlDivJudul = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                    htmlDivJudul.Attributes.Add("class", "panel-title pn");
                    htmlDivJudul.InnerHtml = "<h3 class=\"mtn mb10 fw400\">" + row["floortype"].ToString() + "</h3>";
                    mainpagediv.Controls.Add(htmlDivJudul);
                }

                mainpagediv.Controls.Add(htmlDiv);

                tampungfloor = row["floortype"].ToString();

                mainpagediv.EnableViewState = true;
            }
            dbcon.closeConnection();

        }

        private void SynchStateALLDevice()
        {
            NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from vwsubdeviceid v order by keterangan,\"Nomor Device Id\",\"Nomor Sub Device Id\"  ", null));
            while (objreader.Read())
            {
                string deviceid = objreader["Nomor Device Id"].ToString();
                string subdeviceid = objreader["Nomor Sub Device Id"].ToString();
                string ipaddress = objreader["ipaddress"].ToString();
                string ipport = objreader["ipport"].ToString();

                sysfunction.setTimeDevice(subdeviceid, ipaddress, ipport, deviceid);

                RestClient rClient = new RestClient();
                rClient.endPoint = "http://" + ipaddress + ":" + ipport + "/" + "zeroconf/getState";

                rClient.httpMethod = httpVerb.POST;
                rClient.postJSON = "{\r\n    \"deviceid\":\"" + deviceid + "\",\r\n    \"data\": {  \"subDevId\": \"" + subdeviceid + "\"\r\n    }\r\n}";

                string strResponse = rClient.makeRequest();

                if (strResponse.IndexOf("error") != -1)
                {
                    ResultsysSonOFFAllState obj = JsonConvert.DeserializeObject<ResultsysSonOFFAllState>(strResponse);
                    sysSonOFFdataSub objchild = obj.data;

                    foreach (sysSonOFFswitches objoutlet in objchild.switches)
                    {
                        sysConnection dbcon2 = new sysConnection();

                        string recidoutlet = Convert.ToString(dbcon2.executeScalar(new sysSQLParam("select recid from vwoutlet v where deviceid = '" + deviceid + "' " +
                                                                 "and subdeviceid  = '" + subdeviceid + "' and outletno  = '" + objoutlet.outlet + "' limit 1 ", null)));
                        dbcon2.closeConnection();
                        if (recidoutlet != "" && recidoutlet != null)
                        {
                            NpgsqlDataReader objreader2 = dbcon2.executeQuery(new sysSQLParam("select * from vwroom s where recidoutlet = " + recidoutlet, null));
                            while (objreader2.Read())
                            {
                                string recidroom = objreader2["recid"].ToString();
                                string statusroom = objreader2["statusroom"].ToString();
                                string noroom = objreader2["noroom"].ToString();
                                int statushousekeeping = Convert.ToInt32(objreader2["statushousekeeping"]);
                                int statusmaintenance = Convert.ToInt32(objreader2["statusmaintenance"]);
                                if (statusroom != objoutlet.Switch)
                                {
                                    if (Convert.IsDBNull(objreader2["arrival"]) == false) // berarti ada yang pake nyalain kembali
                                    {
                                        sysfunction.SetONOFFOutlet(recidroom, objoutlet.Switch);
                                    }
                                    else if (statushousekeeping != 0)
                                    {
                                        sysConnection dbcon3 = new sysConnection();
                                        int jumlahdata = Convert.ToInt32(dbcon3.executeScalar(new sysSQLParam("select count(*) from housekeepingroom v where noroom = '" + noroom + "' " +
                                                                                                              "and status <= 0 ", null)));
                                        if (jumlahdata != 0)
                                        {
                                            sysfunction.SetONOFFOutlet(recidroom, objoutlet.Switch);
                                        }
                                        dbcon3.closeConnection();
                                    }
                                    else if (statusmaintenance != 0)
                                    {
                                        sysConnection dbcon3 = new sysConnection();
                                        int jumlahdata = Convert.ToInt32(dbcon3.executeScalar(new sysSQLParam("select count(*) from maitenanceroom v where noroom = '" + noroom + "' " +
                                                                                                              "and status <= 0 ", null)));
                                        if (jumlahdata != 0)
                                        {
                                            sysfunction.SetONOFFOutlet(recidroom, objoutlet.Switch);
                                        }
                                        dbcon3.closeConnection();
                                    }
                                    else
                                    {
                                        sysConnection dbcon3 = new sysConnection();
                                        dbcon3.executeNonQuery(new sysSQLParam("update setuproom set statusroom = '" + objoutlet.Switch + "' " +
                                                          "where recid = " + recidroom, null));
                                        dbcon3.closeConnection();
                                    }
                                }
                            }
                            objreader2.Close();
                            dbcon2.closeConnection();
                        }

                    }
                }

                rClient = null;
            }
            objreader.Close();
            dbcon.closeConnection();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "alert('synchronize state dan set time sukses');", true);
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            //check state all device
            this.SynchStateALLDevice();

            this.LoadOutlet();
        }
    }
}