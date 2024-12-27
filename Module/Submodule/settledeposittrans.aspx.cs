using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module.Submodule
{
    public partial class settledeposittrans : System.Web.UI.Page
    {
        sysConnection dbcon;
        public string usernameid = "";
        public sysUserSession session;
        public string UserName()
        {
            if (session.UserId != null)
                this.usernameid = session.UserId.ToString();
            return " - " + this.usernameid;
        }

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
                sysfunction.setCurrencyStyle(deposit);
                sysfunction.setCurrencyStyle(balance);
                sysfunction.setCurrencyStyle(settledeposit);
                sysfunction.setCurrencyStyle(newbalance);
                sysfunction.setCurrencyStyle(amountpaid);
                sysfunction.setCurrencyStyle(total);

                Uri myUri = new Uri(Request.Url.AbsoluteUri);

                string transid = HttpUtility.ParseQueryString(myUri.Query).Get("transid");
                if (transid != null)
                {
                    typetransaksi.Text = "Settle : ";

                    NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from transaksiroom where transaksiid = '" + transid + "' ", null));
                    if (objreader.Read())
                    {
                        transactionid.Text = objreader["tipetrans"].ToString() + " - " + objreader["transaksiid"].ToString();
                        transactionid.Visible = true;
                        if (Convert.IsDBNull(objreader["total"]) == false)
                            total.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["total"].ToString()));
                        if (Convert.IsDBNull(objreader["flatdiscount"]) == false)
                            flatdiscount.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["flatdiscount"].ToString()));
                        if (Convert.IsDBNull(objreader["amountpaid"]) == false)
                            amountpaid.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["amountpaid"].ToString()));                        
                        if (Convert.IsDBNull(objreader["deposit"]) == false)
                            deposit.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["deposit"].ToString()));
                        if (Convert.IsDBNull(objreader["balance"]) == false)
                            balance.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["balance"].ToString()));
                    }
                    dbcon.closeConnection();

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
            Uri myUri = new Uri(Request.Url.AbsoluteUri);

            string transid = HttpUtility.ParseQueryString(myUri.Query).Get("transid");

            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@transid", transid));
            list.Add(new SqlParameter("@settlepaydeposit", Convert.ToDecimal(settledeposit.Text)));
            list.Add(new SqlParameter("@closebalance", Convert.ToDecimal(newbalance.Text)));
            list.Add(new SqlParameter("@updatetime", DateTime.Now));
            list.Add(new SqlParameter("@updateby", session.UserId));
            list.Add(new SqlParameter("@settletxt", settletxt.Text));

            SqlParameter[] empparam = new SqlParameter[list.Count];
            empparam = list.ToArray();

            dbcon.executeNonQuery(new sysSQLParam("update transaksiroom set settlepaydeposit = @settlepaydeposit " +
                                                  ",closebalance = @closebalance " +
                                                  ",closedate = @updatetime " +
                                                  ",settletxt = @settletxt " +
                                                  ",closeby = @updateby " +
                                                  "where transaksiid = @transid", empparam));
            dbcon.closeConnection();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);

           
        }

        protected void settledeposit_TextChanged(object sender, EventArgs e)
        {
            Decimal total_ = Convert.ToDecimal(total.Text);
            Decimal amountpaid_ = Convert.ToDecimal(amountpaid.Text);
            Decimal deposit_ = Convert.ToDecimal(deposit.Text);
            Decimal balance_ = Convert.ToDecimal(balance.Text);

            if (settledeposit.Text == "") settledeposit.Text = "0";

            Decimal settledeposit_ = Convert.ToDecimal(settledeposit.Text);
            Decimal newbalance_ = 0;
            if (transactionid.Text.Contains("walkin"))
            {
                newbalance_ = amountpaid_ + (settledeposit_ - balance_);
            }
            else
            {
                newbalance_ = returndeposit.Checked ? settledeposit_ : settledeposit_ - balance_;
            }
            newbalance.Text = string.Format("{0:N2}", newbalance_);
        }

        protected void returndeposit_CheckedChanged(object sender, EventArgs e)
        {
            //if (returndeposit.Checked)
            {
                Decimal deposit_ = Convert.ToDecimal(deposit.Text);
                if (deposit_ != 0)
                {
                    Decimal settledeposit_ = Convert.ToDecimal(deposit.Text) * -1;
                    //settledeposit.ReadOnly = true;
                    settledeposit.Text = returndeposit.Checked ? string.Format("{0:N2}", settledeposit_) : string.Format("{0:N2}", 0);
                    /*
                    if (transactionid.Text.Contains("walkin"))
                    {
                        newbalance.Text = returndeposit.Checked ? string.Format("{0:N2}", Convert.ToDecimal(amountpaid.Text)) : string.Format("{0:N2}", Convert.ToDecimal(balance.Text));
                    }
                    else
                    {
                        newbalance.Text = string.Format("{0:N2}", 0);
                    }
                    */
                    settledeposit_TextChanged(sender, e);
                }
                else
                {
                    returndeposit.Checked = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningalert", "alert('tidak ada deposit !');", true);
                }
            }
            /*
            else
            {
                settledeposit.ReadOnly = false;
                settledeposit.Text = string.Format("{0:N2}", 0);
                newbalance.Text = string.Format("{0:N2}", 0);
            }
            */
        }
    }
}