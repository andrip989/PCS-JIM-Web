using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class cleanroom : System.Web.UI.Page
    {
        public sysConnection dbcon;
        public sysUserSession session;
        public virtual void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["sessionid"] == null)
            {
                HttpContext.Current.Session["sessionid"] = "";
            }

            sysSecurity.checkUserSession(ref session, this.Server, HttpContext.Current.Session["sessionid"].ToString());

            MasterPage masterpage = (MasterPage)this.Master;

            masterpage.BodyTag.Attributes.Add("onFocus", "parent_disable();");
            masterpage.BodyTag.Attributes.Add("onclick", "parent_disable();");            

            if (!IsPostBack)
            {
                this.LoadinformationRoom();
                this.LoadOutlet();
            }
            else
            {
                string parameter = Request["__EVENTARGUMENT"]; // parameter
                string value = Request["__EVENTTARGET"]; // Request["__EVENTTARGET"]; // btnSave

                if (value.Contains("buttonlink"))
                {
                    value = value.Replace("buttonlink", "");
                    value = value.Split(new string[] { ":" }, StringSplitOptions.None)[0];
                    this.RedirecttoTrans(value, parameter);
                }

                if (parameter.Contains("reloadoutlet"))
                {
                    this.LoadinformationRoom();
                    this.LoadOutlet();
                }
            }
        }

        public virtual void RedirecttoTrans(string recidparam, string switchsaklar)
        {
            Boolean isexec = true;

            isexec = false;
            string uriparam = ResolveUrl("~/Module/Submodule/cleantrans.aspx");
            uriparam += "?recidparam=" + recidparam;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "popupwindows", "openWindowchild('" + uriparam + "');", true);

            if (isexec)
                this.LoadOutlet();
        }

        public virtual string getquery()
        {
            return "select * from vwRoom " +
                   //" where noroom not in (select noroom from transaksiroom t where coalesce(t.status, -1::integer) < 1) " +
                   "order by floortype ,noroom ";
        }

        public void Timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            int timeMsSinceMidnight = (int)dateTime.TimeOfDay.TotalSeconds;

            //timernow.Text = dateTime.ToString(sysConfig.DateTimeFormat());

            if (timeMsSinceMidnight % 10 == 0)
            {
                this.LoadinformationRoom();
                //this.LoadOutlet();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "reloadoutlet", "clickoutletevent('', 'reloadoutlet');", true);                
            }

            this.LoadOutlet();
        }

        public void LoadinformationRoom()
        {
            dbcon = new sysConnection();
            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@period", Convert.ToDateTime(DateTime.Now)));
            SqlParameter[] empparam = new SqlParameter[list.Count];
            empparam = list.ToArray();

            NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from public.loadinformationroom(@period) ", empparam));
            if (objreader.Read())
            {
                vacanttext.Text = objreader["vacant_"].ToString();
                occupiedtext.Text = objreader["occupied_"].ToString();
                reservedtext.Text = objreader["reserved_"].ToString();
                outofordertext.Text = objreader["outoforder_"].ToString();
                dueouttext.Text = objreader["duedate_"].ToString();
                dirtytext.Text = objreader["dirty_"].ToString();
            }
            objreader.Close();
            dbcon.closeConnection();

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
                                                               " coalesce(s.status, -1::integer) <= 0 and @period between s.arrival::date and s.departure ::date ", empparam));

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
                                                               "and s.departure < @periodtime and s.status < 0", empparam));

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

                    objaddbutton.OnClientClick = "alert('room is no active')";
                    objaddbutton.Attributes.Add("href", "#");
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

                    objaddbutton.OnClientClick = "clickoutletevent('" + objaddbutton.ID + "','" + row["statusroom"].ToString().ToLower() + "')";
                }                

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

        public void LoadOutletold()
        {
            dbcon = new sysConnection();
            DataTable ListOutlet = dbcon.getdataTable(this.getquery());
            dbcon.closeConnection();
            mainpagediv.Controls.Clear();
            string tampungfloor = "";
            foreach (DataRow row in ListOutlet.Rows)
            {
                var htmlDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                htmlDiv.Attributes.Add("class", "col-md-2 agileinfo-bnt");
                htmlDiv.ID = "divhome" + row["recid"].ToString();

                LinkButton objaddbutton = new LinkButton();
                objaddbutton.ID = "buttonlink" + row["recid"].ToString() + ":" + row["noroom"].ToString();
                var htmlDiv2 = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                htmlDiv2.ID = "divhome2" + row["recid"].ToString() + ":" + row["noroom"].ToString();


                    int outletno = Convert.ToInt32(row["outletno"]);
                    /*
                    if (Convert.ToInt32(row["active"]) == 0)
                    {
                        htmlDiv2.Attributes.Add("class", "bg-dark pv20 text-white fw600 text-center");
                    }
                    else
                    */
                    {
                        if (row["statusroom"].ToString().ToLower() == "on")
                            htmlDiv2.Attributes.Add("class", "bg-occupied light pv20 text-white fw600 text-center");
                        else
                            htmlDiv2.Attributes.Add("class", "bg-vacant dark pv20 text-white fw600 text-center");
                    }
                    objaddbutton.OnClientClick = "clickoutletevent('" + objaddbutton.ID + "','" + row["statusroom"].ToString().ToLower() + "')";

                    if (Convert.IsDBNull(row["firstname"]) == false)    
                    {
                        htmlDiv2.InnerHtml = row["noroom"].ToString() + "(" + row["description"].ToString() + ")" + "<br>" + row["firstname"].ToString();
                    }
                    else
                    {
                        htmlDiv2.InnerHtml = row["noroom"].ToString() + "(" + row["description"].ToString() + ")" + "<br>";
                    }

                    htmlDiv2.InnerHtml += "<br>";

                    if (row["statusroom"].ToString().ToLower() == "on" && Convert.ToInt32(row["statushousekeeping"]) == 0 && Convert.ToInt32(row["statusmaintenance"]) == 0)
                    {
                        htmlDiv2.InnerHtml += "<i class=\"fa fa-user\" aria-hidden=\"true\"></i>&nbsp;";
                    }
                    else if (row["statusroom"].ToString().ToLower() == "on")
                    {
                        htmlDiv2.InnerHtml += "<i class=\"fa  fa-lightbulb-o\" aria-hidden=\"true\"></i>&nbsp;";
                    }

                    if (Convert.ToInt32(row["statushousekeeping"]) == 1)
                    {
                        htmlDiv2.InnerHtml += "<i class=\"glyphicon glyphicon-bed\" aria-hidden=\"true\"></i>";
                        if (!this.Page.ToString().Contains("cleanroom"))
                        {
                            objaddbutton.OnClientClick = "alert('room is dirty')";
                            objaddbutton.Attributes.Add("href", "#");
                        }
                    }
                    else
                        htmlDiv2.InnerHtml += "&nbsp;&nbsp;&nbsp;";

                    if (Convert.ToInt32(row["statusmaintenance"]) == 1)
                    {
                        htmlDiv2.InnerHtml += "&nbsp;" + "<i class=\"fa fa-wrench\" aria-hidden=\"true\"></i>";
                        if (!this.Page.ToString().Contains("repairroom"))
                        {
                            objaddbutton.OnClientClick = "alert('room is maintenance')";
                            objaddbutton.Attributes.Add("href", "#");
                        }
                    }
                    else
                        htmlDiv2.InnerHtml += "&nbsp;&nbsp;&nbsp;";

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
    }
}