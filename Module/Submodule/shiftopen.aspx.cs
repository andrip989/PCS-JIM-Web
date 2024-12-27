using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module.Submodule
{
    public partial class shiftopen : System.Web.UI.Page
    {
        sysConnection dbcon;
        public string usernameid = "";
        public sysUserSession session;
        
        public string UserName()
        {
            if (session.UserId != null)
                this.usernameid = session.UserId.ToString();
            return " - "+this.usernameid;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(HttpContext.Current.Session["sessionid"] == null)
            {
                HttpContext.Current.Session["sessionid"] = "";
            }

            sysSecurity.checkUserSession(ref session, this.Server, HttpContext.Current.Session["sessionid"].ToString());

            dbcon = new sysConnection();

            if (!this.IsPostBack) 
            {
                sysfunction.setCurrencyStyle(saldoawal);
                sysfunction.setCurrencyStyle(saldoakhir);
                //sysfunction.setCurrencyStyle(totalbalance);

                transactionid.Text = "Cashier" + this.UserName();
                transactionid.Visible = true;
                Uri myUri = new Uri(Request.Url.AbsoluteUri);

                string tipetrans = HttpUtility.ParseQueryString(myUri.Query).Get("tipe");
                DateTime tanggalopen = DateTime.Now;

                if (tipetrans != null)
                {
                    typetransaksi.Text = "Close";

                    NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from cashiertrans where status = 1 and tanggalclose  is not null order by tanggalclose desc limit 1", null));
                    if (objreader.Read())
                    {
                        if (Convert.IsDBNull(objreader["saldoakhir"]) == false)
                            saldoawal.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["saldoakhir"].ToString()));

                        tanggalopen = Convert.ToDateTime(objreader["tanggalopen"]);
                    }
                    dbcon.closeConnection();
                    /*
                    string sqltax = " select sum(closebalance) as closebalance,sum(balance) as balance from transaksiroom t where " +
                        "(t.departure >= '" + tanggalopen.ToString("yyyy-MM-dd HH:mm:ss") + "' or " + 
                        "t.closedate >= '" + tanggalopen.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    objreader = dbcon.executeQuery(new sysSQLParam(sqltax, null));
                    Decimal SaldoBalance = 0;
                    if (objreader.Read())
                    {
                        if (Convert.IsDBNull(objreader["closebalance"]) == false)
                            SaldoBalance = Convert.ToDecimal(objreader["closebalance"].ToString());
                        
                        if (Convert.IsDBNull(objreader["balance"]) == false)
                            SaldoBalance -= Convert.ToDecimal(objreader["balance"].ToString());
                    }

                    dbcon.closeConnection();
                    totalbalance.Text = string.Format("{0:N2}", SaldoBalance);
                    */
                    saldoakhir.ReadOnly = false;
                    saldoawal.ReadOnly = true;
                }
                else
                {
                    typetransaksi.Text = "Open";
                    saldoakhir.ReadOnly = true;
                    saldoawal.ReadOnly = false;
                }
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

        protected void cashierbtn_ServerClick(object sender, EventArgs e)
        {
            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@createdby", session.UserId));
            list.Add(new SqlParameter("@saldoawal", Convert.ToDecimal(saldoakhir.Text)));

            SqlParameter[] empparam = new SqlParameter[list.Count];
            empparam = list.ToArray();

            dbcon.executeNonQuery(new sysSQLParam("select * from sp_cashiertrans(@createdby,@saldoawal)", empparam));
            dbcon.closeConnection();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);

            Uri myUri = new Uri(Request.Url.AbsoluteUri);

            string tipetrans = HttpUtility.ParseQueryString(myUri.Query).Get("tipe");
            if (tipetrans != null)
            {
                NpgsqlConnection.ClearPool(dbcon.Connection);

                Session.Clear();
            }
        }

    }
}