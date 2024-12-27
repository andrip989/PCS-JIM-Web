using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using static System.Collections.Specialized.BitVector32;

namespace PCS_JIM_Web.Module.Submodule
{
    public partial class cleantrans : inputtrans
    {
        public Boolean Isupdate;

        public Boolean isupdate
        {
            get
            {
                return this.Isupdate;
            }
            set
            {
                this.Isupdate = value;
            }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            
            if (!this.IsPostBack) 
            { 
                hide1.Visible = false;
                hide2.Visible = false;
                hide3.Visible = false;
                jumlahadult.Visible = false;
                jumlahanak.Visible = false;

                if (!this.isupdate)
                { 
                    typetransaksi.Text = "House Keeping - Clean Room";
                    transactionid.Text = "Create";

                    datetimestart.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
                    DateTime strdate = Convert.ToDateTime(datetimestart.Text);
                    //datetimeend.Text = strdate.AddHours(1).ToString("yyyy-MM-ddTHH:mm");
                    datetimeend.Text = strdate.AddMinutes(Convert.ToInt32(sysConfig.HouseKeepingMinutes())).ToString("yyyy-MM-ddTHH:mm");
                    datetimestart.ReadOnly = true;
                    datetimeend.ReadOnly = true;
                    this.datetimeend_TextChanged1(sender, e);
                }
            }
        }
        
        public override string getQueryRoom()
        {
            return "select * from setuproom order by noroom ";
        }

        public virtual string gettabletrans()
        {
            return "housekeepingroom";
        }

        public virtual string typetrans()
        {
            return "House Keeping - ";
        }

        public override void loadcurrent()
        {            
            Uri myUri = new Uri(Request.Url.AbsoluteUri);

            string transid = HttpUtility.ParseQueryString(myUri.Query).Get("transid");
            string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("recidparam");

            if (transid != null && transid != "")
            {
                string paramroom = HttpUtility.ParseQueryString(myUri.Query).Get("noroom");

                this.loadroom(paramroom);

                objreader = dbcon.executeQuery(new sysSQLParam("select T1.* from " +this.gettabletrans()+ " T1 " +
                                                                "where T1.transid = '" + transid + "' and coalesce(T1.status,0::integer) <= 0 ", null));
            }
            else
            {
                this.loadroom(param1);

                objreader = dbcon.executeQuery(new sysSQLParam("select T1.* from " +this.gettabletrans()+ " T1 " +
                                                                "where T1.noroom = '" + noroom.SelectedValue + "' and coalesce(T1.status,-1::integer) = 0 ", null));
            }

            if (objreader.Read())
            {
                if (noroom.SelectedValue == "")
                {
                    noroom.SelectedValue = objreader["noroom"].ToString();
                    
                }
                noroom.Enabled = false;
                this.isupdate = true;
                transactionid.Text = objreader["transid"].ToString();
                transactionid.Visible = true;
                btncheckout.Visible = true;
                typetransaksi.Text = this.typetrans();

                DateTime strdate = Convert.ToDateTime(objreader["arrival"]);
                DateTime enddate = Convert.ToDateTime(objreader["departure"]);

                TimeSpan diff = enddate - strdate;

                datetimestart.Text = strdate.ToString("yyyy-MM-ddTHH:mm");
                datetimeend.Text = enddate.ToString("yyyy-MM-ddTHH:mm");

                jumlahhari.Text = diff.Days.ToString() + " day " + diff.Hours.ToString() + " hour " + diff.Minutes.ToString() + " minute";

                datetimestart.ReadOnly = true;
                datetimeend.ReadOnly = true;
                
                remark.Text = objreader["keterangan"].ToString();

                housekeepbtn2.InnerHtml = "<div class=\"bg-success pv20 text-white fw600 text-center\">Update</div>";
                housekeepbtn2.Attributes.Add("onclick", "#");

                btnclose.InnerHtml = "<div class=\"bg-alert light pv20 text-white fw600 text-center\">Cancel</div>";
                housekeepbtnclose.Visible = true;
            }
            objreader.Close();
            dbcon.closeConnection();
        }

        public virtual string getNoTransID()
        { 
            return sysfunction.getNextSequenceHouseKeeping(noroom.SelectedValue);
        }

        public virtual void housekeepbtn_Click(object sender, EventArgs e)
        {
            if (transactionid.Text == "Create")
            {
                string transaksiidv = this.getNoTransID();
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@noroom", noroom.SelectedValue));
                //list.Add(new SqlParameter("@arrival", DateTime.Now));
                //list.Add(new SqlParameter("@departure", DateTime.Now.AddHours(1)));                
                list.Add(new SqlParameter("@arrival", DateTime.Now));//Convert.ToDateTime(datetimestart.Text)));
                list.Add(new SqlParameter("@departure", DateTime.Now.AddMinutes(Convert.ToInt32(sysConfig.HouseKeepingMinutes()))));//Convert.ToDateTime(datetimeend.Text)));
                list.Add(new SqlParameter("@createddatetime", DateTime.Now));
                list.Add(new SqlParameter("@createdby", session.UserId));
                list.Add(new SqlParameter("@transid", transaksiidv));
                list.Add(new SqlParameter("@keterangan", remark.Text));
                list.Add(new SqlParameter("@status", -1));

                SqlParameter[] empparam = new SqlParameter[list.Count];
                empparam = list.ToArray();

                string sql = "insert into " +this.gettabletrans()+ " (noroom,arrival,transid,status" +
                                                        ",departure,createddate,createdby" +
                                                        ",keterangan) ";
                sql += " values (@noroom,@arrival,@transid,@status" +
                                                        ",@departure,@createddatetime,@createdby" +
                                                        ",@keterangan) ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
            }
            else
            {
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@keterangan", remark.Text));
                list.Add(new SqlParameter("@createddatetime", DateTime.Now));
                list.Add(new SqlParameter("@createdby", session.UserId));
                list.Add(new SqlParameter("@transid", transactionid.Text));
                SqlParameter[] empparam = new SqlParameter[list.Count];
                empparam = list.ToArray();

                string sql = "update " +this.gettabletrans()+ " set keterangan = @keterangan ";
                      sql += " ,updateddate = @createddatetime";
                      sql += " ,updatedby = @createdby";
                      sql += " where transid = @transid";

                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);
        }

        public void datetimeend_TextChanged1(object sender, EventArgs e)
        {
            DateTime strdate = Convert.ToDateTime(datetimestart.Text);
            DateTime enddate = Convert.ToDateTime(datetimeend.Text);

            TimeSpan diff = enddate - strdate;
            jumlahhari.Text = diff.Days.ToString() + " day " + diff.Hours.ToString() + " hour " + diff.Minutes.ToString() + " minute";
        }

        public void housekeepbtnclose_ServerClick(object sender, EventArgs e)
        {
            var list = new List<SqlParameter>();
            list.Add(new SqlParameter("@keterangan", remark.Text));
            list.Add(new SqlParameter("@createddatetime", DateTime.Now));
            list.Add(new SqlParameter("@createdby", session.UserId));
            list.Add(new SqlParameter("@transid", transactionid.Text));
            SqlParameter[] empparam = new SqlParameter[list.Count];
            empparam = list.ToArray();

            string sql = "update " +this.gettabletrans()+ " set keterangan = @keterangan,status = 1 ";
            sql += " ,updateddate = @createddatetime";
            sql += " ,updatedby = @createdby";
            sql += " where transid = @transid";

            dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
            dbcon.closeConnection();

            sysConnection conWork2 = new sysConnection("DBConnectionBatch");

            list = new List<SqlParameter>();

            list.Add(new SqlParameter("@updateddate", DateTime.Now));
            list.Add(new SqlParameter("@updatedby", session.UserId));
            empparam = new SqlParameter[list.Count];
            empparam = list.ToArray();

            conWork2.executeNonQuery(new sysSQLParam("update pcstelexa set status = 1 " +
                                                     ",updateddate = @updateddate,updatedby = @updatedby " +
                                                     "where refnum='" + transactionid.Text + "'", empparam));
            conWork2.closeConnection();

            dbcon.executeNonQuery(new sysSQLParam(this.updateRoomQuery(), empparam));
            dbcon.closeConnection();

            sysfunction.SetONOFFOutlet(noroom.SelectedValue, "on");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);
        }

        public virtual string updateRoomQuery()
        {
            return "update setuproom set statusroom = 'off'" +
                                              ",updateddatetime = @updateddate,updatedby = @updatedby " +
                                              ",statushousekeeping = 0 where statushousekeeping = 1 and noroom = '" + noroom.SelectedValue + "' ";
        }

    }
    
}