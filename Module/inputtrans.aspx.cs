using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Npgsql;
using PCS_JIM_Web.Library;
using static System.Net.Mime.MediaTypeNames;

namespace PCS_JIM_Web.Module
{
    public partial class inputtrans : System.Web.UI.Page
    {
        public sysConnection dbcon;
        public sysUserSession session;
        public NpgsqlDataReader objreader;
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["sessionid"] == null)
            {
                HttpContext.Current.Session["sessionid"] = "";
            }

            sysSecurity.checkUserSession(ref session, this.Server, HttpContext.Current.Session["sessionid"].ToString());

            Uri myUri = new Uri(Request.Url.AbsoluteUri);

            string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("recidparam");

            string tipetrans = HttpUtility.ParseQueryString(myUri.Query).Get("tipe");

            if (tipetrans != "" && tipetrans != null)
            {
                typetransaksi.Text = "Reservation";
                amountpaid.Enabled = false;
                if (this.GetType().Name.Contains("inputtrans"))
                    tipetransparam.Value = "reservation";
            }
            else
            {
                typetransaksi.Text = "Walk In";
                if (this.GetType().Name.Contains("inputtrans"))
                {
                    refbookingcode.Text = "Walk in";
                    tipetransparam.Value = "walkin";
                }
            }

            dbcon = new sysConnection();

            if (!this.IsPostBack)
            {
                labelbtncreated.Text = "Create";                

                sysfunction.setCurrencyStyle(totalcharges);
                sysfunction.setCurrencyStyle(discount);
                sysfunction.setCurrencyStyle(totaltax);
                sysfunction.setCurrencyStyle(totalrate);
                sysfunction.setCurrencyStyle(extracharges);
                sysfunction.setCurrencyStyle(total);
                sysfunction.setCurrencyStyle(amountpaid);
                sysfunction.setCurrencyStyle(flatdiscount);
                sysfunction.setCurrencyStyle(deposit);
                sysfunction.setCurrencyStyle(roundoff);
                sysfunction.setCurrencyStyle(balance);

                objreader = dbcon.executeQuery(new sysSQLParam(this.getQueryRoom(), null));
                int rowidx = 0;
                while (objreader.Read())
                {
                    if (rowidx == 0)
                    {
                        noroom.Items.Insert(rowidx, new ListItem("--Select No.Room--", ""));
                        rowidx++;
                    }

                    noroom.Items.Insert(rowidx, new ListItem(objreader["noroom"].ToString() + " - " + objreader["description"].ToString()
                        + " (" + objreader["roomtypes"].ToString() + ") - " + objreader["floortype"].ToString()
                        , objreader["noroom"].ToString()));
                    rowidx++;
                }
                objreader.Close();
                dbcon.closeConnection();

                //noroom.Enabled = (param1 != "" && param1 != null) ? false : true;

                roomamenities.DataSource = dbcon.getdataTable("select * from setuproomamenities");
                dbcon.closeConnection();
                roomamenities.DataTextField = "description";
                roomamenities.DataValueField = "roomamenities";
                roomamenities.AppendDataBoundItems = true;
                roomamenities.DataBind();

                paymenttype.DataSource = dbcon.getdataTable("select * from setuppaymenttype where active = 1");//Enum.GetValues(typeof(PaymentType));
                dbcon.closeConnection();
                paymenttype.DataTextField = "description";
                paymenttype.DataValueField = "paymenttype";
                paymenttype.AppendDataBoundItems = true;
                paymenttype.DataBind();

                businesscode.DataSource = dbcon.getdataTable("select * from setupbusinesssources");
                dbcon.closeConnection();
                businesscode.DataTextField = "companyname";
                businesscode.DataValueField = "alias";
                businesscode.AppendDataBoundItems = true;
                businesscode.DataBind();

                ratetype.DataSource = dbcon.getdataTable("select * from setupratetype");
                dbcon.closeConnection();
                ratetype.DataTextField = "keterangan";
                ratetype.DataValueField = "ratetype";
                ratetype.DataBind();

                floortype.DataSource = dbcon.getdataTable("select * from setupfloor");
                dbcon.closeConnection();
                floortype.DataTextField = "description";
                floortype.DataValueField = "floortype";
                floortype.DataBind();

                roomtype.DataSource = dbcon.getdataTable("select * from setuproomtypes");
                dbcon.closeConnection();
                roomtype.DataTextField = "keterangan";
                roomtype.DataValueField = "roomtypes";
                roomtype.DataBind();

                //floortype.Enabled = noroom.Enabled;
                //roomtype.Enabled = noroom.Enabled;

                this.loadcurrent();
            }
            else
            {
                string parameter = Request["__EVENTARGUMENT"]; // parameter
                string value = Request["__EVENTTARGET"]; // Request["__EVENTTARGET"]; // btnSave
                //this.loadroom(param1);
            }
        }
        public string ApplicationTitle()
        {
            return sysConfig.IDFTitle();
        }

        public string ApplicationLokasi()
        {
            return sysConfig.IDFlokasi();
        }

        public virtual string getQueryRoom()
        {
            return "select * from setuproom where active = 1 order by noroom ";
        }

        public virtual void loadcurrent() 
        {
            Boolean isupdate = false;
            Uri myUri = new Uri(Request.Url.AbsoluteUri);

            string transid = HttpUtility.ParseQueryString(myUri.Query).Get("transid");
            string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("recidparam");

            string tanggalparam = HttpUtility.ParseQueryString(myUri.Query).Get("period");


            if (transid != null && transid != "")
            {
                string paramroom = HttpUtility.ParseQueryString(myUri.Query).Get("noroom");

                this.loadroom(paramroom);

                objreader = dbcon.executeQuery(new sysSQLParam("select T1.*,S.* from transaksiroom T1 " +
                                                                "LEFT JOIN setupguestlist s ON T1.custcode::text = s.custcode::text " +
                                                                "where T1.transaksiid = '" + transid + "' " +
                                                                "order by T1.arrival ", null));//and coalesce(T1.status,0::integer) <= 0 ", null));
            }
            else
            {
                this.loadroom(param1);

                if (tanggalparam != null && tanggalparam != "")
                {
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@period", Convert.ToDateTime(tanggalparam)));
                    list.Add(new SqlParameter("@noroom", noroom.SelectedValue));
                    SqlParameter[] empparam = new SqlParameter[list.Count];
                    empparam = list.ToArray();

                    objreader = dbcon.executeQuery(new sysSQLParam("select T1.*,S.* from transaksiroom T1 " +
                                                                "LEFT JOIN setupguestlist s ON T1.custcode::text = s.custcode::text " +
                                                                "where T1.noroom = @noroom " +
                                                                "and @period between T1.arrival::date and T1.departure ::date " +
                                                                "and coalesce(T1.status,-1::integer) <= 0 " +
                                                                "order by T1.arrival ", empparam));
                }
                else
                {
                    objreader = dbcon.executeQuery(new sysSQLParam("select T1.*,S.* from transaksiroom T1 " +
                                                                "LEFT JOIN setupguestlist s ON T1.custcode::text = s.custcode::text " +
                                                                "where T1.noroom = '" + noroom.SelectedValue + "' and coalesce(T1.status,-1::integer) <= 0 " +
                                                                "order by T1.arrival ", null));
                }
            }

            if (objreader.Read())
            {
                if (noroom.SelectedValue == "")
                {
                    noroom.SelectedValue = objreader["noroom"].ToString();
                    noroom.Enabled = false;
                }

                isupdate = true;
                transactionid.Text = objreader["transaksiid"].ToString();
                transactionid.Visible = true;
                noroom.Enabled = false;
                if (Convert.IsDBNull(objreader["tipetrans"]) == false)
                    typetransaksi.Text = objreader["tipetrans"].ToString()  + " - ";
                else
                    typetransaksi.Text = "Reservation - ";

                DateTime strdate = Convert.ToDateTime(objreader["arrival"]);
                DateTime enddate = Convert.ToDateTime(objreader["departure"]);

                TimeSpan diff = enddate - strdate;

                datetimestart.Text = strdate.ToString("yyyy-MM-ddTHH:mm");
                datetimeend.Text = enddate.ToString("yyyy-MM-dd");
                datetimeend2.Text = enddate.ToString("HH:mm tt");

                datetimestart.ReadOnly = true;
                datetimeend.ReadOnly = true;

                if (Convert.IsDBNull(objreader["totalcharges"]) == false)
                    totalcharges.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["totalcharges"].ToString()));
                if (Convert.IsDBNull(objreader["discount"]) == false)
                    discount.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["discount"].ToString()));
                if (Convert.IsDBNull(objreader["totaltax"]) == false)
                    totaltax.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["totaltax"].ToString()));
                if (Convert.IsDBNull(objreader["total"]) == false)
                    total.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["total"].ToString()));
                if (Convert.IsDBNull(objreader["flatdiscount"]) == false)
                    flatdiscount.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["flatdiscount"].ToString()));
                if (Convert.IsDBNull(objreader["amountpaid"]) == false)
                    amountpaid.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["amountpaid"].ToString()));
                if (Convert.IsDBNull(objreader["deposit"]) == false)
                    deposit.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["deposit"].ToString()));
                if (Convert.IsDBNull(objreader["roundoff"]) == false)
                    roundoff.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["roundoff"].ToString()));
                if (Convert.IsDBNull(objreader["balance"]) == false)
                    balance.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["balance"].ToString()));
                if (Convert.IsDBNull(objreader["extracharges"]) == false)
                    extracharges.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["extracharges"].ToString()));
                if (Convert.IsDBNull(objreader["totalrate"]) == false)
                    totalrate.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["totalrate"].ToString()));

                if (Convert.IsDBNull(objreader["discountcode"]) == false)
                    discpct.Text = "(" + objreader["discountcode"].ToString() + ")" + string.Format("{0:N2}", Convert.ToDecimal(objreader["discpct"].ToString()));

                jumlahadult.Text = objreader["jumlahadult"].ToString();
                jumlahanak.Text = objreader["jumlahchild"].ToString();
                keterangantambahan.Text = objreader["keterangantambahan"].ToString();
                refbookingcode.Text = objreader["refbookingcode"].ToString();

                //jumlahhari.Text = diff.Days.ToString() + " day " + diff.Hours.ToString() + " hour " + diff.Minutes.ToString() + " minute";
                jumlahhari.Text = (enddate.Day - strdate.Day).ToString() + " day ";
                submit.Text = "Update";
                submit.CssClass = "btn-success btn";
                submit.Enabled = true;
                //load customer
                custcode.Text = objreader["custcode"].ToString();

                if (this.GetType().Name.Contains("inputtrans"))
                {
                    tipetransparam.Value = objreader["tipetrans"].ToString();

                    if (Convert.IsDBNull(objreader["bookingsource"]) == false)
                        businesscode.SelectedValue = objreader["bookingsource"].ToString();
                    else
                        businesscode.SelectedValue = objreader["businesscode"].ToString();
                }

                labelbtncreated.Text = "Update";
                txtSearch.Text = objreader["firstname"].ToString();
                firstname.Text = objreader["firstname"].ToString();
                lastname.Text = objreader["lastname"].ToString();
                paymenttype.SelectedValue = objreader["paymenttype"].ToString();
                
                creditcardno.Text = objreader["creditcardno"].ToString();

                ratetype.Enabled = false;
                usetax.Enabled = false;
                if (Convert.IsDBNull(objreader["usetax1"]) == false)
                    usetax.Items[0].Selected = objreader["usetax1"].ToString() == "1" ? true : false;
                if (Convert.IsDBNull(objreader["usetax2"]) == false)
                    usetax.Items[1].Selected = objreader["usetax2"].ToString() == "1" ? true : false;
                //if (Convert.IsDBNull(objreader["usetax3"]) == false)
                //    usetax.Items[2].Selected = objreader["usetax3"].ToString() == "1" ? true : false;
                //if (Convert.IsDBNull(objreader["usetax4"]) == false)
                //    usetax.Items[3].Selected = objreader["usetax4"].ToString() == "1" ? true : false;

                if (Convert.IsDBNull(objreader["status"]) == false)
                {
                    if (Convert.ToInt32(objreader["status"]) >= 1)
                    {
                        btncancel.Text = "Close";
                        submit.Visible = false;
                        btncheckout.Visible = false;
                        btncancelcheckin.Visible = false;
                        btncheckin.Visible = false;
                        btnnoshow.Visible = false;
                    }
                    else if (Convert.ToInt32(objreader["status"]) == 0)
                    {
                        btncheckout.Visible = true;
                        btncancelcheckin.Visible = session.Usergroupid != "STAFF";
                        btnnoshow.Visible = false;
                        btncheckin.Visible = false;
                    }
                    else if (Convert.ToInt32(objreader["status"]) == -1 && Convert.ToInt32(objreader["manualcheckin"]) == 0)
                    {
                        btncheckout.Visible = false;
                        btncancelcheckin.Visible = session.Usergroupid != "STAFF";
                        btncancelcheckin.Text = "Cancel";
                        btncheckin.Visible = false;
                        btnnoshow.Visible = session.Usergroupid != "STAFF";
                    }
                    else
                    {
                        btncheckout.Visible = false;
                        btncancelcheckin.Visible = false;
                        btncheckin.Visible = true;
                        btnnoshow.Visible = session.Usergroupid != "STAFF";
                    }
                }
                else
                {
                    btncheckout.Visible = false;
                    btncancelcheckin.Visible = false;
                    btncheckin.Visible = true;
                    btnnoshow.Visible = session.Usergroupid != "STAFF";
                }

                if (btncheckout.Visible || (typetransaksi.Text.ToLower().Contains("reservation")))
                {
                    amountpaid.Enabled = false;
                }
            }
            objreader.Close();
            dbcon.closeConnection();

            if (!isupdate)
            {
                keterangantambahan.Text = "";

                if (tanggalparam != null && tanggalparam != "")
                {
                    datetimestart.Text = Convert.ToDateTime(tanggalparam).ToString("yyyy-MM-ddT14:00");
                    //datetimeend.Text = "yyyy - MM - ddT12:00";//DateTime.Now.AddDays(1).ToString("yyyy-MM-ddT12:00");
                    datetimeend2.Text = "12:00:00 PM";
                }
                else
                {
                    datetimestart.Text = DateTime.Now.ToString("yyyy-MM-ddT14:00");
                    //datetimeend.Text = "yyyy - MM - ddT12:00";//DateTime.Now.AddDays(1).ToString("yyyy-MM-ddT12:00");
                    datetimeend2.Text = "12:00:00 PM";
                }
            }
        }

        public void loadroom(string param1)
        {
            decimal roomratedefault = 0;
            roomamenities.ClearSelection();

            if (param1 != "" && param1 != null)
            {
                string classname = this.GetType().Name;
                Uri myUri = new Uri(Request.Url.AbsoluteUri);

                string param12 = HttpUtility.ParseQueryString(myUri.Query).Get("recidparam");

                if (param12 != "" && param12 != null)
                { 
                    objreader = dbcon.executeQuery(new sysSQLParam("select T1.*,s.baseadult,s.basechild,s.adultmax ,s.childmax  from setuproom T1 LEFT JOIN setuproomtypes s ON T1.roomtypes::text = s.roomtypes::text where T1.recid = " + param1 + " ", null));
                    noroom.Enabled = false;
                }
                else
                {                    
                    objreader = dbcon.executeQuery(new sysSQLParam("select T1.*,s.baseadult,s.basechild,s.adultmax ,s.childmax  from setuproom T1 LEFT JOIN setuproomtypes s ON T1.roomtypes::text = s.roomtypes::text where T1.noroom = '" + param1 + "' ", null));
                }

                if (objreader.Read())
                {
                    noroom.SelectedValue = objreader["noroom"].ToString();
                    //noroom.Enabled = false;
                    description.Text = objreader["description"].ToString();
                    recidroom.Value = objreader["recid"].ToString();
                    if (Convert.IsDBNull(objreader["roomrate"]) == false)
                        roomratedefault = Convert.ToDecimal(objreader["roomrate"].ToString());
                    Boolean lockprice = true;
                    if (Convert.IsDBNull(objreader["lockprice"]) == false)
                        lockprice = Convert.ToInt16(objreader["lockprice"]) == 1 ? true : false;
                    if (Convert.IsDBNull(objreader["floortype"]) == false)
                        floortype.SelectedValue = objreader["floortype"].ToString();
                    if (Convert.IsDBNull(objreader["roomtypes"]) == false)
                        roomtype.SelectedValue = objreader["roomtypes"].ToString();

                    if (Convert.IsDBNull(objreader["roomamenities"]) == false)
                    {
                        string[] ids = objreader["roomamenities"].ToString().Split(new string[] { "," }, StringSplitOptions.None);
                        foreach (ListItem item in roomamenities.Items)
                        {
                            if (ids.Contains(item.Value))
                            {
                                item.Selected = true;
                            }
                        }
                    }
                    
                    if(classname.Contains("inputtrans"))
                    { 
                        if (Convert.IsDBNull(objreader["adultmax"]) == false)
                            maxadult.Text = objreader["adultmax"].ToString();
                        if (Convert.IsDBNull(objreader["childmax"]) == false)
                            maxchild.Text = objreader["childmax"].ToString();

                        if (Convert.IsDBNull(objreader["baseadult"]) == false)
                            jumlahadult.Text = objreader["baseadult"].ToString();
                        if (Convert.IsDBNull(objreader["basechild"]) == false)
                            jumlahanak.Text = objreader["basechild"].ToString();

                        if (Convert.IsDBNull(objreader["statushousekeeping"]) == false)
                            statushousekeeping.Value = objreader["statushousekeeping"].ToString();
                        if (Convert.IsDBNull(objreader["statusmaintenance"]) == false)
                            statusmaintenance.Value = objreader["statusmaintenance"].ToString();
                    }
                    if (this.GetType().Name.Contains("inputtrans"))
                    {
                        if (lockprice && tipetransparam.Value == "walkin")
                        {
                            totalcharges.Enabled = false;
                            discount.Enabled = false;
                            extracharges.Enabled = false;
                            flatdiscount.Enabled = false;

                            //deposit.Enabled = false;
                        }
                    }
                    //string tipetrans = HttpUtility.ParseQueryString(myUri.Query).Get("tipe");
                    //if (tipetrans != "" && tipetrans != null)
                    

                    if (classname.Contains("inputtrans"))
                    {
                        servicecharge.Enabled = false;
                        pb1charge.Enabled = false;
                    }
                        totaltax.Enabled = false;
                        totalrate.Enabled = false;
                        total.Enabled = false;
                        roundoff.Enabled = false;
                        balance.Enabled = false;
                    
                }
                objreader.Close();
                dbcon.closeConnection();
                //set nol harga
                totalcharges.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                discount.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                totaltax.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                totalrate.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                extracharges.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                total.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                flatdiscount.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                amountpaid.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                deposit.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                roundoff.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                balance.Text = string.Format(sysConfig.CurrencyFormat(), 0);

                if (classname.Contains("inputtrans"))
                {
                    servicecharge.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                    pb1charge.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                    scpct.Value = string.Format(sysConfig.CurrencyFormat(), 0);
                    scamount.Value = string.Format(sysConfig.CurrencyFormat(), 0);
                    pb1pct.Value = string.Format(sysConfig.CurrencyFormat(), 0);
                    pb1amount.Value = string.Format(sysConfig.CurrencyFormat(), 0);
                    datetimeend2.Text = "12:00:00 PM";
                }
                datetimeend.Text = "";
                

                string sqltax = " select s.usetax  from setuproomtariff s " +
                                " where (s.roomtypes = '" + roomtype.SelectedValue + "' or s.roomtypes = '') " +
                                " and (s.floortype = '" + floortype.SelectedValue +"' or s.floortype = '') " +
                                " and (s.bussinesssource = '" + businesscode.SelectedValue + "' or s.bussinesssource = '') " +
                                " and (s.custcode = '" + custcode.Text + "' or s.custcode = '') " +
                                " and (s.noroom = '" + noroom.SelectedValue + "' or s.noroom = '') " +
                                " and s.active = 1 " +
                                " order by roomtypes , floortype, ratetypes ,bussinesssource ,seasontype ,custcode ,noroom " +
                                " limit 1 ";

                int usetaxdefault = Convert.ToInt32(dbcon.executeScalar(new sysSQLParam(sqltax,null)));
                usetax.Items[0].Selected = usetaxdefault > 0 ? true : false;
                usetax.Items[1].Selected = usetaxdefault > 0 ? true : false;
                dbcon.closeConnection();

            }


        }
        protected void listoutletno_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadroom(noroom.SelectedValue);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SetAutoComplete2", "SetAutoComplete();", true); // untuk refresh ajax
        }

        public string getopenURL()
        {
            string uriparam = ResolveUrl("Submodule/custcreate.aspx");
            
            if (custcode.Text != "")
            { 
                uriparam += "?custcodeparam=" + custcode.Text;
            }

            return "openWindowchildnew('" + uriparam + "');";
        }

        [WebMethod]
        public static string[] GetCustomers(string prefix)
        {
            List<string> Emp = new List<string>();

            sysConnection dbcon;
            dbcon = new sysConnection();

            NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from setupguestlist " +
                                                                            "where lower(firstname) like '%" + prefix.ToLower() + "%' " +
                                                                            "or lower(lastname) like '%" + prefix.ToLower() + "%' " +
                                                                            "or lower(email) like '%" + prefix.ToLower() + "%' " +
                                                                            "or lower(identificationid) like '%" + prefix.ToLower() + "%' " +
                                                                            "", null));
            while (objreader.Read())
            {
                Emp.Add(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|({6}) {7}|{8}"
                    , objreader["custcode"]
                    , objreader["firstname"]
                    , objreader["lastname"]
                    , objreader["paymenttype"]
                    , objreader["businesscode"]
                    , objreader["creditcardno"]
                    , objreader["discountcode"] 
                    , objreader["discpct"]
                    , objreader["identificationid"]
                    ));
            }

            //Emp.Add("AAA");
            //Emp.Add("BBB");
            objreader.Close();
            dbcon.closeConnection();

            return Emp.ToArray();
        }

        [WebMethod]
        public static string CheckAvailable(string custcodeparam,string noroomparam,string startdateparam,string enddateparam)
        {
            int a = 0;

            //startdateparam = startdateparam.Replace("T", " ");
            //enddateparam = enddateparam.Replace("T", " ");

            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@custcode", custcodeparam));
            list.Add(new SqlParameter("@noroom", noroomparam));
            list.Add(new SqlParameter("@arrival", Convert.ToDateTime(startdateparam)));
            list.Add(new SqlParameter("@departure", Convert.ToDateTime(enddateparam)));
            SqlParameter[] empparam = new SqlParameter[list.Count];
            empparam = list.ToArray();

            sysConnection dbcon;
            dbcon = new sysConnection();

            string retval = "not available";
            NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from sp_checkavailableroom(@arrival,@departure,@noroom,@custcode)", empparam));
            if (objreader.Read())
            {
                if (Convert.IsDBNull(objreader[0]) == false)
                    retval = objreader[0].ToString();
            }
            objreader.Close();
            dbcon.closeConnection();

            return retval;
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            if (custcode.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validasicust", "alert('Customer wajib diisi..!');", true);
            }
            else
            {
                if (Page.IsValid && submit.Text == "Update")
                {
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@noroom", noroom.SelectedValue));
                    list.Add(new SqlParameter("@refrecidroom", Convert.ToInt64(recidroom.Value)));
                    list.Add(new SqlParameter("@arrival", Convert.ToDateTime(datetimestart.Text)));
                    list.Add(new SqlParameter("@jumlahadult", Convert.ToInt32(jumlahadult.Text)));
                    list.Add(new SqlParameter("@jumlahchild", Convert.ToInt32(jumlahanak.Text)));
                    list.Add(new SqlParameter("@departure", Convert.ToDateTime(datetimeend.Text + "T" + datetimeend2.Text.Substring(0,5))));//"T12:00")));
                    list.Add(new SqlParameter("@createddatetime", DateTime.Now));
                    list.Add(new SqlParameter("@createdby", session.UserId));
                    list.Add(new SqlParameter("@transaksiid", transactionid.Text));
                    list.Add(new SqlParameter("@keterangantambahan", keterangantambahan.Text));
                    list.Add(new SqlParameter("@custcode", custcode.Text));
                    list.Add(new SqlParameter("@refbookingcode", refbookingcode.Text));
                    list.Add(new SqlParameter("@seasontype", ""));
                    list.Add(new SqlParameter("@ratetype", ratetype.SelectedValue));
                    list.Add(new SqlParameter("@usetax1", usetax.Items[0].Selected ? 1 : 0));
                    list.Add(new SqlParameter("@usetax2", usetax.Items[1].Selected ? 1 : 0));
                    list.Add(new SqlParameter("@usetax3", Convert.ToInt32(0)));
                    list.Add(new SqlParameter("@usetax4", Convert.ToInt32(0)));
                    list.Add(new SqlParameter("@totalcharges", Convert.ToDecimal(totalcharges.Text)));
                    list.Add(new SqlParameter("@discount", Convert.ToDecimal(discount.Text)));
                    list.Add(new SqlParameter("@totaltax", Convert.ToDecimal(totaltax.Text)));
                    list.Add(new SqlParameter("@total", Convert.ToDecimal(total.Text)));
                    list.Add(new SqlParameter("@flatdiscount", Convert.ToDecimal(flatdiscount.Text)));
                    list.Add(new SqlParameter("@amountpaid", Convert.ToDecimal(amountpaid.Text)));
                    list.Add(new SqlParameter("@deposit", Convert.ToDecimal(deposit.Text)));
                    list.Add(new SqlParameter("@roundoff", Convert.ToDecimal(roundoff.Text)));
                    list.Add(new SqlParameter("@balance", Convert.ToDecimal(balance.Text)));
                    list.Add(new SqlParameter("@extracharges", Convert.ToDecimal(extracharges.Text)));
                    list.Add(new SqlParameter("@totalrate", Convert.ToDecimal(totalrate.Text)));
                    list.Add(new SqlParameter("@paymenttype", paymenttype.SelectedValue));
                    list.Add(new SqlParameter("@bookingsource", businesscode.SelectedValue));

                    SqlParameter[] empparam = new SqlParameter[list.Count];
                    empparam = list.ToArray();


                    string sql = "update transaksiroom set noroom = @noroom" +
                                                        ",refrecidroom = @refrecidroom" +
                                                        ",arrival = @arrival" +
                                                        ",paymenttype = @paymenttype" +
                                                        ",jumlahadult = @jumlahadult" +
                                                        ",jumlahchild = @jumlahchild" +
                                                        ",departure = @departure" +
                                                        ",keterangantambahan = @keterangantambahan" +
                                                        ",custcode = @custcode" +
                                                        ",refbookingcode = @refbookingcode" +
                                                        ",seasontype = @seasontype" +
                                                        ",ratetype = @ratetype" +
                                                        ",usetax1 = @usetax1" +
                                                        ",usetax2 = @usetax2" +
                                                        ",usetax3 = @usetax3" +
                                                        ",usetax4 = @usetax4" +
                                                        ",totalcharges = @totalcharges" +
                                                        ",discount = @discount" +
                                                        ",totaltax = @totaltax" +
                                                        ",total = @total" +
                                                        ",flatdiscount = @flatdiscount" +
                                                        ",amountpaid = @amountpaid" +
                                                        ",deposit = @deposit" +
                                                        ",roundoff = @roundoff" +
                                                        ",balance = @balance" +
                                                        ",bookingsource = @bookingsource" + 
                                                        ",extracharges = @extracharges" +
                                                        ",totalrate = @totalrate" +
                                                        ",updatedatetime = @createddatetime" +
                                                        ",updatedby = @createdby ";
                    sql += " where transaksiid = @transaksiid ";

                    dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                    dbcon.closeConnection();


                    //update telexa
                    sysConnection conWork2 = new sysConnection("DBConnectionBatch");

                    list = new List<SqlParameter>();

                    list.Add(new SqlParameter("@updateddate", DateTime.Now));
                    list.Add(new SqlParameter("@updatedby", session.UserId));
                    list.Add(new SqlParameter("@departure", Convert.ToDateTime(datetimeend.Text + "T" + datetimeend2.Text.Substring(0, 5))));//"T12:00")));

                    empparam = new SqlParameter[list.Count];
                    empparam = list.ToArray();

                    conWork2.executeNonQuery(new sysSQLParam("update pcstelexa set departure = @departure " +
                                                             ",updateddate = @updateddate,updatedby = @updatedby " +
                                                             "where refnum='" + transactionid.Text + "'", empparam));
                    conWork2.closeConnection();

                    this.calculateHarga(transactionid.Text);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "checkouta", "alert('Reservasi berhasil diupdate');", true);
                }
                else
                {
                    string transaksiidv = sysfunction.getNextSequenceRoom(noroom.SelectedValue);
                    var list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@noroom", noroom.SelectedValue));
                    list.Add(new SqlParameter("@refrecidroom", Convert.ToInt64(recidroom.Value)));
                    list.Add(new SqlParameter("@arrival", Convert.ToDateTime(datetimestart.Text)));
                    list.Add(new SqlParameter("@jumlahadult", Convert.ToInt32(jumlahadult.Text)));
                    list.Add(new SqlParameter("@jumlahchild", Convert.ToInt32(jumlahanak.Text)));
                    list.Add(new SqlParameter("@departure", Convert.ToDateTime(datetimeend.Text + "T" + datetimeend2.Text.Substring(0, 5))));//+ "T12:00")));
                    list.Add(new SqlParameter("@createddatetime", DateTime.Now));
                    list.Add(new SqlParameter("@createdby", session.UserId));
                    list.Add(new SqlParameter("@transaksiid", transaksiidv));
                    list.Add(new SqlParameter("@keterangantambahan", keterangantambahan.Text));
                    list.Add(new SqlParameter("@custcode", custcode.Text));
                    list.Add(new SqlParameter("@refbookingcode", refbookingcode.Text));
                    list.Add(new SqlParameter("@seasontype", ""));
                    list.Add(new SqlParameter("@ratetype", ratetype.SelectedValue));
                    list.Add(new SqlParameter("@usetax1", usetax.Items[0].Selected ? 1 : 0));
                    list.Add(new SqlParameter("@usetax2", usetax.Items[1].Selected ? 1 : 0));
                    list.Add(new SqlParameter("@usetax3", Convert.ToInt32(0)));
                    list.Add(new SqlParameter("@usetax4", Convert.ToInt32(0)));
                    list.Add(new SqlParameter("@totalcharges", Convert.ToDecimal(totalcharges.Text)));
                    list.Add(new SqlParameter("@discount", Convert.ToDecimal(discount.Text)));
                    list.Add(new SqlParameter("@totaltax", Convert.ToDecimal(totaltax.Text)));
                    list.Add(new SqlParameter("@total", Convert.ToDecimal(total.Text)));
                    list.Add(new SqlParameter("@flatdiscount", Convert.ToDecimal(flatdiscount.Text)));
                    list.Add(new SqlParameter("@amountpaid", Convert.ToDecimal(amountpaid.Text)));
                    list.Add(new SqlParameter("@deposit", Convert.ToDecimal(deposit.Text)));
                    list.Add(new SqlParameter("@roundoff", Convert.ToDecimal(roundoff.Text)));
                    list.Add(new SqlParameter("@balance", Convert.ToDecimal(balance.Text)));
                    list.Add(new SqlParameter("@extracharges", Convert.ToDecimal(extracharges.Text)));
                    list.Add(new SqlParameter("@totalrate", Convert.ToDecimal(totalrate.Text)));
                    list.Add(new SqlParameter("@paymenttype", paymenttype.SelectedValue));
                    list.Add(new SqlParameter("@manualcheckin", Convert.ToInt16(1)));
                    list.Add(new SqlParameter("@bookingsource", businesscode.SelectedValue));
                    

                    Uri myUri = new Uri(Request.Url.AbsoluteUri);

                    string tipetrans = HttpUtility.ParseQueryString(myUri.Query).Get("tipe");

                    if (tipetrans != "" && tipetrans != null)
                    {
                        list.Add(new SqlParameter("@tipetrans", "reservation"));
                    }
                    else
                        list.Add(new SqlParameter("@tipetrans", "walkin"));

                    SqlParameter[] empparam = new SqlParameter[list.Count];
                    empparam = list.ToArray();

                    string sql = "insert into transaksiroom (noroom,refrecidroom,arrival,jumlahadult,jumlahchild" +
                                                            ",departure,createddatetime,createdby,transaksiid" +
                                                            ",keterangantambahan,custcode,refbookingcode,seasontype,ratetype" +
                                                            ",usetax1,usetax2,usetax3,usetax4" +
                                                            ",totalcharges,discount,totaltax,total" +
                                                            ",flatdiscount,amountpaid,deposit,roundoff,balance,extracharges,totalrate,paymenttype,manualcheckin,tipetrans,bookingsource) ";
                    sql += " values (@noroom,@refrecidroom,@arrival,@jumlahadult,@jumlahchild" +
                                                            ",@departure,@createddatetime,@createdby,@transaksiid" +
                                                            ",@keterangantambahan,@custcode,@refbookingcode,@seasontype,@ratetype" +
                                                            ",@usetax1,@usetax2,@usetax3,@usetax4" +
                                                            ",@totalcharges,@discount,@totaltax,@total" +
                                                            ",@flatdiscount,@amountpaid,@deposit,@roundoff,@balance,@extracharges,@totalrate,@paymenttype,@manualcheckin,@tipetrans,@bookingsource) ";
                    dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                    dbcon.closeConnection();

                    this.calculateHarga(transaksiidv);
                    //this.SetONOFFOutlet(recidroom.Value);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "checkina", "alert('Reservasi berhasil');", true);
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);
            }
        }

        protected void btncheckin_Click(object sender, EventArgs e)
        {
            if (tipetransparam.Value.ToLower().Contains("walkin") && Convert.ToDecimal(amountpaid.Text) == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validasiwalkin", "alert('Amount Paid wajib diisi !');", true);
            }
            else if(statusmaintenance.Value == "1")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "roomismaintenance", "alert('Room sedang maintenance !');", true);
            }
            else
            {
                var list = new List<SqlParameter>();

                list.Add(new SqlParameter("@updatedby", session.UserId));
                list.Add(new SqlParameter("@createddatetime", DateTime.Now));
                list.Add(new SqlParameter("@transaksiid", transactionid.Text));
                string sql = "";
                if (tipetransparam.Value.ToLower().Contains("walkin"))
                {
                    list.Add(new SqlParameter("@amountpaid", Convert.ToDecimal(amountpaid.Text)));
                    list.Add(new SqlParameter("@deposit", Convert.ToDecimal(deposit.Text)));
                    list.Add(new SqlParameter("@roundoff", Convert.ToDecimal(roundoff.Text)));
                    list.Add(new SqlParameter("@balance", Convert.ToDecimal(balance.Text)));
                    
                    sql = "update transaksiroom set status = -1 " +
                                                    ",manualcheckin = 0 " +
                                                    ",arrival = @createddatetime" +
                                                    ",amountpaid = @amountpaid" +
                                                    ",deposit = @deposit" +
                                                    ",roundoff = @roundoff" +
                                                    ",balance = @balance" +
                                                    ",updatedatetime = @createddatetime" +
                                                    ",updatedby = @updatedby ";
                    sql += " where transaksiid = @transaksiid ";
                }
                else
                {
                    sql = "update transaksiroom set status = -1 " +
                                                        ",manualcheckin = 0 " +
                                                        ",arrival = @createddatetime" +
                                                        ",updatedatetime = @createddatetime" +
                                                        ",updatedby = @updatedby ";
                    sql += " where transaksiid = @transaksiid ";
                }

                SqlParameter[] empparam = new SqlParameter[list.Count];
                empparam = list.ToArray();

                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();

                /* -- checkoutnya otomatis
                dbcon.executeNonQuery(new sysSQLParam("update setuproom set statusroom = 'on'" +
                                                      ",updateddatetime = @createddatetime,updatedby = @updatedby " +
                                                      " where noroom = '" + noroom.SelectedValue + "' ", empparam));
                dbcon.closeConnection();

                sysfunction.SetONOFFOutlet(recidroom.Value, "on");
                */

                string sqltax = " select s.email from setupguestlist s " +
                                    " where s.custcode = '" + custcode.Text + "' " +
                                    " limit 1 ";

                string emailcustomer = Convert.ToString(dbcon.executeScalar(new sysSQLParam(sqltax, null)));
                dbcon.closeConnection();

                sysZeptoMail obj = new sysZeptoMail();
                /*
                if (emailcustomer != "")
                {
                    obj.ToAddress = emailcustomer;
                    obj.BCCAddress = "andrip@jimmail.com,iwanh@jimmail.com,yanto@jimmail.com";
                }
                else email ke IT aja checkin buat monitoring
                */
                {
                    obj.ToAddress = "andrip@jimmail.com,iwanh@jimmail.com,yanto@jimmail.com";
                    //obj.ToAddress = "andrip@jimmail.com";
                    //obj.BCCAddress = "iwanh@jimmail.com, yanto@jimmail.com";
                }
                obj.Subject = "Check-in activity - " + this.ApplicationLokasi() + " : " + transactionid.Text;
                obj.Body = this.generateBodyMail();
                obj.SendMail();

                DateTime strdate = Convert.ToDateTime(datetimestart.Text);
                DateTime enddate = Convert.ToDateTime(datetimeend.Text + "T" + datetimeend2.Text.Substring(0, 5));//+ "T12:00");

                TimeSpan diff = enddate - strdate;
                sysCreateHotspot objh = new sysCreateHotspot();
                objh.Limituptime = diff.Days;
                objh.Username = noroom.SelectedValue;
                objh.Devicecount = Convert.ToInt32(jumlahadult.Text);
                if (objh.Devicecount < 2) objh.Devicecount = 4;
                if (objh.Devicecount > 8) objh.Devicecount = 8;
                if (emailcustomer != "")
                {
                    objh.Emailhotspot = emailcustomer;
                }
                else
                    objh.Emailhotspot = "andrip@jimmail.com,iwanh@jimmail.com";

                objh.Create();

                list = new List<SqlParameter>();
                list.Add(new SqlParameter("@noroom", noroom.SelectedValue));
                list.Add(new SqlParameter("@createddatetime", DateTime.Now));
                list.Add(new SqlParameter("@createdby", session.UserId));
                list.Add(new SqlParameter("@transaksiid", transactionid.Text));
                list.Add(new SqlParameter("@ssid", sysConfig.routerssidhotspot()));
                list.Add(new SqlParameter("@username", objh.Username));
                list.Add(new SqlParameter("@password", objh.Password));
                list.Add(new SqlParameter("@checkout", Convert.ToDateTime(datetimeend.Text + "T" + datetimeend2.Text.Substring(0, 5))));//+ "T12:00")));

                empparam = new SqlParameter[list.Count];
                empparam = list.ToArray();

                sql = "insert into transaksidetailhotspot (noroom, ssid, username, password, checkin, checkout, createddate, createdby, reftransid) ";
                sql += " values (@noroom,@ssid,@username,@password,@createddatetime" +
                                                        ",@checkout,@createddatetime,@createdby" +
                                                        ",@transaksiid) ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();

                if (statushousekeeping.Value == "1")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "checkina", "alert('Harap di bersihkan terlebih dahulu ,menggunakan House Keeping !');", true);
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "checkina", "alert('Checkin room berhasil');", true);
                
                string uriparam = ResolveUrl("~/Module/Submodule/CreatePMSKey.aspx");
                uriparam += "?id=" + transactionid.Text;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "popupwindows", "openWindowchildnew('" + uriparam + "');", true);
                
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);                
                
            }
        }

        public string generateBodyMail()
        {
            string value = "<table border=0>";
            value += "<tr> <td>No. Room           </td><td> : </td> <td> " + noroom.SelectedValue + "</td></tr>";
            value += "<tr> <td>Check - in         </td><td> : </td> <td> " + DateTime.Now.ToString(sysConfig.DateTimeFormat()) + "</td></tr>";
            value += "<tr> <td>Check - out        </td><td> : </td> <td> " + Convert.ToDateTime(datetimeend.Text + "T" + datetimeend2.Text.Substring(0, 5)).ToString(sysConfig.DateTimeFormat()) + "</td></tr>";
            value += "<tr> <td>Total Charges      </td><td> : </td> <td> <b>" + totalcharges.Text + "</b></td></tr>";
            value += "<tr> <td>Deposit            </td><td> : </td> <td> <b>" + deposit.Text + "</b></td></tr>";
            value += "<tr> <td>Guest Name         </td><td> : </td> <td> " + firstname.Text + " " + lastname.Text + "</td></tr>";
            value += "<tr> <td>Guest Remark       </td><td> : </td> <td> " + keterangantambahan.Text + "</td></tr>";
            value += "<tr> <td>Guest Payment      </td><td> : </td> <td> " + paymenttype.SelectedValue + "</td></tr>";
            value += "<tr> <td>Ref. Booking Code  </td><td> : </td> <td> " + refbookingcode.Text + "</td></tr>";
            value += "</table>";
            return value;
        }

        protected void btncheckout_Click(object sender, EventArgs e)
        {
            SqlParameter[] empparam = new SqlParameter[3];

            empparam[0] = new SqlParameter("@updatedby", session.UserId);

            empparam[1] = new SqlParameter("@createddatetime", DateTime.Now);

            empparam[2] = new SqlParameter("@transaksiid", transactionid.Text);

            string sql = "update transaksiroom set status = 1 " +
                                                ",updatedatetime = @createddatetime" +
                                                ",actualcheckout = @createddatetime" +
                                                ",updatedby = @updatedby ";
            sql += " where transaksiid = @transaksiid ";

            dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
            dbcon.closeConnection();

            sysConnection conWork2 = new sysConnection("DBConnectionBatch");

            var list = new List<SqlParameter>();

            list.Add(new SqlParameter("@updateddate", DateTime.Now));
            list.Add(new SqlParameter("@updatedby", session.UserId));
            empparam = new SqlParameter[list.Count];
            empparam = list.ToArray();

            conWork2.executeNonQuery(new sysSQLParam("update pcstelexa set status = 1 " +
                                                     ",updateddate = @updateddate,updatedby = @updatedby " +
                                                     "where refnum='" + transactionid.Text + "'", empparam));
            conWork2.closeConnection();

            dbcon.executeNonQuery(new sysSQLParam("update setuproom set statusroom = 'off'" +
                                                  ",updateddatetime = @updateddate,updatedby = @updatedby " +
                                                  ",statushousekeeping = 1 where noroom = '" + noroom.SelectedValue + "' ", empparam));
            dbcon.closeConnection();

            sysfunction.SetONOFFOutlet(recidroom.Value, "on");

            sysZeptoMail obj = new sysZeptoMail();
            //obj.ToAddress = "andrip@jimmail.com,yanto@jimmail.com,iwanh@jimmail.com";
            obj.ToAddress = "andrip@jimmail.com";
            obj.BCCAddress = "iwanh@jimmail.com";
            obj.Subject = "Check-out activity - " + this.ApplicationLokasi() + " : " + transactionid.Text;

            obj.Body = this.generateBodyMail();

            obj.SendMail();

            string sqltax = " select count(*) from transaksidetailhotspot s " +
                                " where s.noroom = '" + noroom.SelectedValue + "' and coalesce(s.status, -1::integer) <= 0 ";

            int countdata = Convert.ToInt32(dbcon.executeScalar(new sysSQLParam(sqltax, null)));
            dbcon.closeConnection();

            if (countdata > 0)
            {
                //clear dulu ssid hotspot lama
                sysCreateHotspot objh = new sysCreateHotspot();
                objh.Username = noroom.SelectedValue;
                objh.HapusUser(noroom.SelectedValue);

                empparam = new SqlParameter[2];

                empparam[0] = new SqlParameter("@updatedby", session.UserId);

                empparam[1] = new SqlParameter("@createddatetime", DateTime.Now);

                dbcon.executeNonQuery(new sysSQLParam("update transaksidetailhotspot set status = 1" +
                                                  ",updateddate = @createddatetime,updatedby = @updatedby " +
                                                  " where noroom = '" + noroom.SelectedValue + "' and coalesce(status, -1::integer) <= 0 ", empparam));
                dbcon.closeConnection();
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "checkout", "alert('checkout room berhasil');", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);

        }

        protected void datetimestart_TextChanged(object sender, EventArgs e)
        {
            if(datetimestart.Text != "")
            { 
                DateTime strdate = Convert.ToDateTime(datetimestart.Text);
                if(datetimeend.Text != "")
                {
                    DateTime enddate = Convert.ToDateTime(datetimeend.Text +"T" + datetimeend2.Text.Substring(0, 5));

                    if (strdate < enddate)
                    { 
                        setSeasonandRatetype();
                        this.calculateHarga();
                    }
                    else {
                        datetimestart.Text = datetimeend.Text;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertprice", "alert('Arrival invalid !');", true);
                        submit.Enabled = false;
                    }
                }
            }
        }

        protected void datetimeend_TextChanged(object sender, EventArgs e)
        {
            if(datetimeend.Text != "")
            { 
                DateTime enddate = Convert.ToDateTime(datetimeend.Text + "T" + datetimeend2.Text.Substring(0, 5));
                if(datetimestart.Text != "")
                {
                    DateTime strdate = Convert.ToDateTime(datetimestart.Text);
                    if (strdate < enddate)
                    {
                        setSeasonandRatetype();
                        this.calculateHarga();
                    }
                    else
                    {
                        datetimeend.Text = datetimestart.Text;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertprice", "alert('Departure invalid !');", true);
                        submit.Enabled = false;
                    }
                }
            }
        }

        private void calculateHarga()
        {
            this.calculateHarga("");
        }

        private void calculateHarga(string transaksiidv)
        {
            if (datetimestart.Text != "" && datetimeend.Text != "" && noroom.SelectedValue != "")
            { 
                DateTime strdate = Convert.ToDateTime(datetimestart.Text);
                DateTime enddate = Convert.ToDateTime(datetimeend.Text + "T" + datetimeend2.Text.Substring(0, 5));

                TimeSpan diff = enddate - strdate;

                int ratetypehour = Convert.ToInt32(dbcon.executeScalar(new sysSQLParam("select hour from setupratetype where ratetype = '"+ ratetype.SelectedValue + "'", null)));
                dbcon.closeConnection();

                if (ratetypehour == 0)
                {
                    jumlahhari.Text = (enddate.Day - strdate.Day).ToString() + " day ";//+ diff.Hours.ToString() + " hour " + diff.Minutes.ToString() + " minute";
                }
                else
                    jumlahhari.Text = diff.Days.ToString() + " day " + diff.Hours.ToString() + " hour " + diff.Minutes.ToString() + " minute";

                labelbtncreated.Text = custcode.Text != "" ? "Update" : "Create";

                string _paymenttype = paymenttype.SelectedValue;
                string _businesscode = businesscode.SelectedValue;

                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@startdate", Convert.ToDateTime(datetimestart.Text)));
                list.Add(new SqlParameter("@enddate", Convert.ToDateTime(datetimeend.Text + "T" + datetimeend2.Text.Substring(0, 5))));
                list.Add(new SqlParameter("@noroom", noroom.SelectedValue));
                list.Add(new SqlParameter("@custcode", custcode.Text));
                list.Add(new SqlParameter("@bussinesssor", businesscode.SelectedValue));
                list.Add(new SqlParameter("@seasontype", ""));
                list.Add(new SqlParameter("@ratetype", ratetype.SelectedValue));
                list.Add(new SqlParameter("@adult", Convert.ToInt32(jumlahadult.Text)));
                list.Add(new SqlParameter("@child", Convert.ToInt32(jumlahanak.Text)));
                list.Add(new SqlParameter("@tax1", usetax.Items[0].Selected ? 1 : 0 ));
                list.Add(new SqlParameter("@tax2", usetax.Items[1].Selected ? 1 : 0 ));
                list.Add(new SqlParameter("@tax3", Convert.ToInt32(0) ));
                list.Add(new SqlParameter("@tax4", Convert.ToInt32(0) ));
                list.Add(new SqlParameter("@transaksiid", transaksiidv));

                SqlParameter[] empparam = new SqlParameter[list.Count];
                empparam = list.ToArray();
                //dbcon = new sysConnection();

                objreader = dbcon.executeQuery(new sysSQLParam("select * from sp_calculateharga(@startdate,@enddate,@noroom" +
                                                                ",@custcode,@bussinesssor,@seasontype,@ratetype" +
                                                                ",@adult,@child,@tax1,@tax2,@tax3,@tax4,@transaksiid) ", empparam));
                string result = "";
                if (objreader.Read())
                {
                    if (Convert.IsDBNull(objreader["result"]) == false)
                        result = objreader["result"].ToString();

                    if (Convert.IsDBNull(objreader["totalcharges"]) == false)
                        totalcharges.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["totalcharges"].ToString()));
                    if (Convert.IsDBNull(objreader["discount"]) == false)
                        discount.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["discount"].ToString()));
                    if (Convert.IsDBNull(objreader["totaltax"]) == false)
                        totaltax.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["totaltax"].ToString()));
                    if (Convert.IsDBNull(objreader["totalrate"]) == false)
                        totalrate.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["totalrate"].ToString()));
                    if (Convert.IsDBNull(objreader["extracharges"]) == false)
                        extracharges.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["extracharges"].ToString()));
                    if (Convert.IsDBNull(objreader["total"]) == false)
                        total.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["total"].ToString()));
                    if (Convert.IsDBNull(objreader["flatdiscount"]) == false)
                        flatdiscount.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["flatdiscount"].ToString()));
                    if (Convert.IsDBNull(objreader["amoutpaid"]) == false)
                        amountpaid.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["amoutpaid"].ToString()));
                    if (Convert.IsDBNull(objreader["deposit"]) == false)
                        deposit.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["deposit"].ToString()));
                    if (Convert.IsDBNull(objreader["roundoff"]) == false)
                        roundoff.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["roundoff"].ToString()));
                    if (Convert.IsDBNull(objreader["balance"]) == false)
                        balance.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["balance"].ToString()));

                }

                Uri myUri = new Uri(Request.Url.AbsoluteUri);

                string tipetrans = HttpUtility.ParseQueryString(myUri.Query).Get("tipe");

                if (tipetrans != "" && tipetrans != null)
                {
                    balance.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                }
                else
                {
                    amountpaid.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                }

                objreader.Close();
                dbcon.closeConnection();

                if ((totalcharges.Text == "" || totalcharges.Text == "0.00") && transaksiidv == "") //&& totalcharges.Enabled == false)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertprice", "alert('" + result + "');", true);
                    submit.Enabled = false;
                }
                else if (this.GetType().Name.Contains("inputtrans"))
                {
                    
                    objreader = dbcon.executeQuery(new sysSQLParam("select s.tax1,s.tax2,s.tax1amount,s.tax2amount "+
                                                                   "from setuptax s " +
                                                                   "where s.fromamount < "+ Convert.ToDecimal(totalcharges.Text) + "::money " +
                                                                   "and s.toamount >= " + Convert.ToDecimal(totalcharges.Text) + "::money limit 1", null));

                    decimal tax1 = 0, tax1amount = 0, tax2 = 0, tax2amount = 0;

                    servicecharge.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                    pb1charge.Text = string.Format(sysConfig.CurrencyFormat(), 0);
                    scpct.Value = string.Format(sysConfig.CurrencyFormat(), tax1);
                    scamount.Value = string.Format(sysConfig.CurrencyFormat(), tax1amount);
                    pb1pct.Value = string.Format(sysConfig.CurrencyFormat(), tax2);
                    pb1amount.Value = string.Format(sysConfig.CurrencyFormat(), tax2amount);

                    if (objreader.Read() && (usetax.Items[0].Selected || usetax.Items[1].Selected))
                    {
                        tax1 = Convert.ToDecimal(objreader["tax1"].ToString());
                        tax1amount = Convert.ToDecimal(objreader["tax1amount"].ToString());
                        tax2 = Convert.ToDecimal(objreader["tax2"].ToString());
                        tax2amount = Convert.ToDecimal(objreader["tax2amount"].ToString());

                        scpct.Value = string.Format(sysConfig.CurrencyFormat(), tax1);
                        scamount.Value = string.Format(sysConfig.CurrencyFormat(), tax1amount);
                        pb1pct.Value = string.Format(sysConfig.CurrencyFormat(), tax2);
                        pb1amount.Value = string.Format(sysConfig.CurrencyFormat(), tax2amount);

                        decimal sc = 0;

                        if (scpct.Value != "" && usetax.Items[0].Selected)
                        {
                            sc = Convert.ToDecimal(totalcharges.Text) - Convert.ToDecimal(discount.Text) + Convert.ToDecimal(extracharges.Text);
                            sc = sc * Convert.ToDecimal(scpct.Value) / 100;

                            servicecharge.Text = string.Format(sysConfig.CurrencyFormat(), sc);
                        }

                        if (pb1pct.Value != "" && usetax.Items[1].Selected)
                        {
                            sc = Convert.ToDecimal(totalcharges.Text) - Convert.ToDecimal(discount.Text) + Convert.ToDecimal(extracharges.Text) + sc;
                            sc = sc * Convert.ToDecimal(pb1pct.Value) / 100;
                            pb1charge.Text = string.Format(sysConfig.CurrencyFormat(), sc);  
                        }

                    }

                    objreader.Close();
                    dbcon.closeConnection();
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SetAutoComplete", "SetAutoComplete();", true); // untuk refresh ajax

                //paymenttype.SelectedValue = _paymenttype;
                //businesscode.SelectedValue = _businesscode;
            }
        }

        private void setSeasonandRatetype()
        {
            try
            {
                DateTime strdate = Convert.ToDateTime(datetimestart.Text);
                DateTime enddate = Convert.ToDateTime(datetimeend.Text + "T" + datetimeend2.Text.Substring(0, 5));

                TimeSpan diff = enddate - strdate;
                //jumlahhari.Text = diff.Days.ToString() + " day " + diff.Hours.ToString() + " hour " + diff.Minutes.ToString() + " minute";

                int day = diff.Days;
                if (diff.Hours >= 20)
                    day++;
                //if (diff.Days != 0)
                    ratetype.DataSource = dbcon.getdataTable("select * from setupratetype where hour = 0 and day <= " + day.ToString());
                //else
                //    ratetype.DataSource = dbcon.getdataTable("select * from setupratetype where day = 0 and hour <= " + diff.Hours.ToString());

                dbcon.closeConnection();
                ratetype.DataTextField = "keterangan";
                ratetype.DataValueField = "ratetype";
                ratetype.DataBind();
            }
            catch { }
        }

        protected void jumlahadult_TextChanged(object sender, EventArgs e)
        {
            this.calculateHarga();
        }

        protected void jumlahanak_TextChanged(object sender, EventArgs e)
        {
            this.calculateHarga();
        }

        protected void ratetype_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.calculateHarga();
        }

        protected void usetax_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.calculateHarga();
        }

        protected void btnnoshow_Click(object sender, EventArgs e)
        {
            SqlParameter[] empparam = new SqlParameter[3];

            empparam[0] = new SqlParameter("@updatedby", session.UserId);

            empparam[1] = new SqlParameter("@createddatetime", DateTime.Now);

            empparam[2] = new SqlParameter("@transaksiid", transactionid.Text);

            string sql = "update transaksiroom set status = 3 " +
                                                ",updatedatetime = @createddatetime" +
                                                ",actualcheckin = @createddatetime" +
                                                ",updatedby = @updatedby ";
            sql += " where transaksiid = @transaksiid ";

            dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
            dbcon.closeConnection();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "noshow", "alert('No Show room berhasil');", true);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            SqlParameter[] empparam = new SqlParameter[3];

            empparam[0] = new SqlParameter("@updatedby", session.UserId);

            empparam[1] = new SqlParameter("@createddatetime", DateTime.Now);

            empparam[2] = new SqlParameter("@transaksiid", transactionid.Text);

            string sql = "update transaksiroom set status = 2 " +
                                                ",updatedatetime = @createddatetime" +
                                                ",updatedby = @updatedby ";
            sql += " where transaksiid = @transaksiid ";

            dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
            dbcon.closeConnection();

            if (btncheckout.Visible)
            {
                sysConnection conWork2 = new sysConnection("DBConnectionBatch");

                var list = new List<SqlParameter>();

                list.Add(new SqlParameter("@updateddate", DateTime.Now));
                list.Add(new SqlParameter("@updatedby", session.UserId));
                empparam = new SqlParameter[list.Count];
                empparam = list.ToArray();

                conWork2.executeNonQuery(new sysSQLParam("update pcstelexa set status = 1 " +
                                                         ",updateddate = @updateddate,updatedby = @updatedby " +
                                                         "where refnum='" + transactionid.Text + "'", empparam));
                conWork2.closeConnection();

                dbcon.executeNonQuery(new sysSQLParam("update setuproom set statusroom = 'off'" +
                                                      ",updateddatetime = @updateddate,updatedby = @updatedby " +
                                                      " where noroom = '" + noroom.SelectedValue + "' ", empparam));
                dbcon.closeConnection();

                sysfunction.SetONOFFOutlet(recidroom.Value, "on");
            }

            sysZeptoMail obj = new sysZeptoMail();
            //obj.ToAddress = "andrip@jimmail.com,yanto@jimmail.com,iwanh@jimmail.com";
            obj.ToAddress = "andrip@jimmail.com";
            obj.BCCAddress = "iwanh@jimmail.com";
            obj.Subject = "Cancel Room activity - " + this.ApplicationLokasi() + " : " + transactionid.Text;

            obj.Body = this.generateBodyMail();

            obj.SendMail();

            string sqltax = " select count(*) from transaksidetailhotspot s " +
                                " where s.noroom = '" + noroom.SelectedValue + "' and coalesce(s.status, -1::integer) <= 0 ";

            int countdata = Convert.ToInt32(dbcon.executeScalar(new sysSQLParam(sqltax, null)));
            dbcon.closeConnection();

            if (countdata > 0)
            {
                //clear dulu ssid hotspot lama
                sysCreateHotspot objh = new sysCreateHotspot();
                objh.Username = noroom.SelectedValue;
                objh.HapusUser(noroom.SelectedValue);

                empparam = new SqlParameter[2];

                empparam[0] = new SqlParameter("@updatedby", session.UserId);

                empparam[1] = new SqlParameter("@createddatetime", DateTime.Now);

                dbcon.executeNonQuery(new sysSQLParam("update transaksidetailhotspot set status = 1" +
                                                  ",updateddate = @createddatetime,updatedby = @updatedby " +
                                                  " where noroom = '" + noroom.SelectedValue + "' and coalesce(status, -1::integer) <= 0 ", empparam));
                dbcon.closeConnection();
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "checkout", "alert('cancel room berhasil');", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);
        }

        protected void latecheckout_CheckedChanged(object sender, EventArgs e)
        {
            datetimeend2.ReadOnly = !latecheckout.Checked;
            datetimeend.ReadOnly = !latecheckout.Checked;
        }
    }
}