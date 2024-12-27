using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module.Submodule
{
    public partial class repairtrans : cleantrans
    {        

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

                objreader = dbcon.executeQuery(new sysSQLParam("select * from maintenancetype where active = 1", null));
                int rowidx = 0;
                while (objreader.Read())
                {
                    maintenancetype.Items.Insert(rowidx, new ListItem(objreader["type"].ToString() + " - " + objreader["description"].ToString()
                        + " (" + objreader["period"].ToString() + " minutes) " 
                        , objreader["type"].ToString()));
                    rowidx++;
                }
                objreader.Close();
                dbcon.closeConnection();
                //maintenancetype.DataSource = dbcon.getdataTable("select * from maintenancetype where active = 1");
                //maintenancetype.DataTextField = "description";
                //maintenancetype.DataValueField = "type";
                //maintenancetype.AppendDataBoundItems = true;
                //maintenancetype.DataBind();

                if (!this.isupdate)
                {
                    typetransaksi.Text = "Maintenance - Repair Room";
                    transactionid.Text = "Create";

                    datetimestart.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
                    DateTime strdate = Convert.ToDateTime(datetimestart.Text);

                    //datetimeend.Text = strdate.AddDays(1).ToString("yyyy-MM-ddTHH:mm");

                    datetimestart.ReadOnly = true;
                    datetimeend.ReadOnly = true;
                    maintenancetype_SelectedIndexChanged(sender, e);
                    //this.datetimeend_TextChanged1(sender, e);
                }
            }
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

                objreader = dbcon.executeQuery(new sysSQLParam("select T1.* from " + this.gettabletrans() + " T1 " +
                                                                "where T1.transid = '" + transid + "' and coalesce(T1.status,0::integer) <= 0 ", null));
            }
            else
            {
                this.loadroom(param1);

                objreader = dbcon.executeQuery(new sysSQLParam("select T1.* from " + this.gettabletrans() + " T1 " +
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

                maintenancetype.SelectedValue = objreader["controltype"].ToString();
                maintenancetype.Enabled = false;
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


        public override void housekeepbtn_Click(object sender, EventArgs e)
        {
            if (transactionid.Text == "Create")
            {
                string transaksiidv = this.getNoTransID();
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@noroom", noroom.SelectedValue));
                //list.Add(new SqlParameter("@arrival", DateTime.Now));
                //list.Add(new SqlParameter("@departure", DateTime.Now.AddHours(1)));                
                list.Add(new SqlParameter("@arrival", DateTime.Now));//Convert.ToDateTime(datetimestart.Text)));
                list.Add(new SqlParameter("@departure", DateTime.Now.AddMinutes(Convert.ToInt32(minutesparam.Value))));//Convert.ToDateTime(datetimeend.Text)));
                list.Add(new SqlParameter("@createddatetime", DateTime.Now));
                list.Add(new SqlParameter("@createdby", session.UserId));
                list.Add(new SqlParameter("@transid", transaksiidv));
                list.Add(new SqlParameter("@keterangan", remark.Text));
                list.Add(new SqlParameter("@status", -1));
                list.Add(new SqlParameter("@controltype", maintenancetype.SelectedValue));
                

                SqlParameter[] empparam = new SqlParameter[list.Count];
                empparam = list.ToArray();

                string sql = "insert into " + this.gettabletrans() + " (noroom,arrival,transid,status" +
                                                        ",controltype,departure,createddate,createdby" +
                                                        ",keterangan) ";
                sql += " values (@noroom,@arrival,@transid,@status" +
                                                        ",@controltype,@departure,@createddatetime,@createdby" +
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

                string sql = "update " + this.gettabletrans() + " set keterangan = @keterangan ";
                sql += " ,updateddate = @createddatetime";
                sql += " ,updatedby = @createdby";
                sql += " where transid = @transid";

                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "window.close();", true);
        }

        public override string gettabletrans()
        {
            return "maitenanceroom";
        }

        public override string typetrans()
        {
            return "Maintenance - ";
        }

        public override string getNoTransID()
        {
            return sysfunction.getNextSequenceMaintenance(noroom.SelectedValue);
        }

        public override string updateRoomQuery()
        {
            return "update setuproom set statusroom = 'off'" +
                                              ",updateddatetime = @updateddate,updatedby = @updatedby " +
                                              ",statusmaintenance = 0 where statusmaintenance = 1 and noroom = '" + noroom.SelectedValue + "' ";
        }

        protected void maintenancetype_SelectedIndexChanged(object sender, EventArgs e)
        {
            minutesparam.Value = Convert.ToString(dbcon.executeScalar(new sysSQLParam("select period from maintenancetype where type = '"+maintenancetype.SelectedValue+"'", null)));
            dbcon.closeConnection();

            datetimestart.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            DateTime strdate = Convert.ToDateTime(datetimestart.Text);

            datetimeend.Text = strdate.AddMinutes(Convert.ToInt32(minutesparam.Value)).ToString("yyyy-MM-ddTHH:mm");

            DateTime enddate = Convert.ToDateTime(datetimeend.Text);

            TimeSpan diff = enddate - strdate;
            jumlahhari.Text = diff.Days.ToString() + " day " + diff.Hours.ToString() + " hour " + diff.Minutes.ToString() + " minute";
        }

    }
}