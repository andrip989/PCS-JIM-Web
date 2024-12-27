using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class setuproomtype : System.Web.UI.Page
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

            if (session.Usergroupid != "admin")
            {
                submit.Visible = false;
                btncancel.Visible = false;
                btndelete.Visible = false;
            }

            dbcon = new sysConnection();
            if (!this.IsPostBack)
            {
                sysfunction.setGridStyle(GridView1);
                 GridView1.AutoGenerateColumns = false;

                ((BoundField)GridView1.Columns[7]).DataFormatString = sysConfig.CurrencyFormat();
                ((BoundField)GridView1.Columns[8]).DataFormatString = sysConfig.CurrencyFormat();
                ((BoundField)GridView1.Columns[9]).DataFormatString = sysConfig.CurrencyFormat();
                ((BoundField)GridView1.Columns[10]).DataFormatString = sysConfig.CurrencyFormat();

                ((BoundField)GridView1.Columns[11]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";
                ((BoundField)GridView1.Columns[13]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";

                defaultrate.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                defaultrate.Attributes.Add("onblur", "formatCurrency(this,'blur');");
                defaultadultrate.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                defaultadultrate.Attributes.Add("onblur", "formatCurrency(this,'blur');");
                defaultchildrate.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                defaultchildrate.Attributes.Add("onblur", "formatCurrency(this,'blur');");
                overbooking.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                overbooking.Attributes.Add("onblur", "formatCurrency(this,'blur');");

                this.loadTable();

            }
        }

        private void loadTable()
        {
            keterangan.Text = "";
            roomtypes.Text = "";
            jumlahadult.Text = "";
            jumlahanak.Text = "";
            
            defaultrate.Text = "";
            defaultadultrate.Text = "";
            defaultchildrate.Text = "";

            baseadult.Text = "";
            baseanak.Text = "";
            overbooking.Text = "";

            DataTable dt = dbcon.getdataTable("select * from setuproomtypes order by defaultrate desc");
            dbcon.closeConnection();
            GridView1.DataSource = dt;
            GridView1.DataBind();

            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                CheckBox cb = (CheckBox)GridView1.Rows[i].FindControl("chk");
                cb.Enabled = true;
                if (cb.Checked == true)
                    cb.Checked = false;
            }

            submit.Text = "Submit";
            submit.CssClass = "btn-primary btn";
            btndelete.Enabled = false;
        }

        protected void chkheader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)GridView1.HeaderRow.FindControl("chkheader");
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                CheckBox cbchild = (CheckBox)GridView1.Rows[i].FindControl("chk");
                cbchild.Checked = cb.Checked;
            }
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            int rowind = ((GridViewRow)(sender as Control).NamingContainer).RowIndex;

            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                if (rowind != i)
                {
                    CheckBox cba = (CheckBox)GridView1.Rows[i].FindControl("chk");
                    cba.Enabled = true;
                    if (cba.Checked == true)
                        cba.Checked = false;
                }
            }

            string columnvalue = ((HiddenField)GridView1.Rows[rowind].FindControl("recchk")).Value;

            CheckBox cb = (CheckBox)GridView1.Rows[rowind].FindControl("chk");
            if (cb.Checked)
            {
                recidparam.Value = columnvalue;
                submit.Text = "Update";
                submit.CssClass = "btn-success btn";
                btndelete.Enabled = true;

                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from setuproomtypes where recid = " + recidparam.Value + " ", null));
                if (objreader.Read())
                {
                    roomtypes.Text = objreader["roomtypes"].ToString();
                    keterangan.Text = objreader["keterangan"].ToString();
                    if (Convert.IsDBNull(objreader["defaultrate"]) == false)
                        defaultrate.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["defaultrate"].ToString()));
                    if (Convert.IsDBNull(objreader["defaultadultrate"]) == false)
                        defaultadultrate.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["defaultadultrate"].ToString()));
                    if (Convert.IsDBNull(objreader["defaultchildrate"]) == false)
                        defaultchildrate.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["defaultchildrate"].ToString()));
                    if (Convert.IsDBNull(objreader["overbooking"]) == false)
                        overbooking.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["overbooking"].ToString()));

                    if (Convert.IsDBNull(objreader["adultmax"]) == false)
                        jumlahadult.Text = Convert.ToDecimal(objreader["adultmax"]).ToString();
                    if (Convert.IsDBNull(objreader["childmax"]) == false)
                        jumlahanak.Text = Convert.ToDecimal(objreader["childmax"]).ToString();
                    if (Convert.IsDBNull(objreader["baseadult"]) == false)
                        baseadult.Text = Convert.ToDecimal(objreader["baseadult"]).ToString();
                    if (Convert.IsDBNull(objreader["basechild"]) == false)
                        baseanak.Text = Convert.ToDecimal(objreader["basechild"]).ToString();

                }
                objreader.Close();
                dbcon.closeConnection();
            }
            cb.Enabled = false;
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            this.loadTable();
        }

        protected void SaveClick(object sender, EventArgs e)
        {
            if (Page.IsValid && submit.Text == "Submit")
            {
                SqlParameter[] empparam = new SqlParameter[11];
                empparam[0] = new SqlParameter("@roomtypes", roomtypes.Text);

                empparam[1] = new SqlParameter("@keterangan", keterangan.Text);

                empparam[2] = new SqlParameter("@defaultrate", Convert.ToDecimal(defaultrate.Text == "" ? "0" : defaultrate.Text));

                empparam[3] = new SqlParameter("@defaultadultrate", Convert.ToDecimal(defaultadultrate.Text == "" ? "0" : defaultadultrate.Text));
                
                empparam[4] = new SqlParameter("@defaultchildrate", Convert.ToDecimal(defaultchildrate.Text == "" ? "0" : defaultchildrate.Text));

                empparam[5] = new SqlParameter("@createddate", DateTime.Now);

                empparam[6] = new SqlParameter("@adultmax", Convert.ToDecimal(jumlahadult.Text == "" ? "0" : jumlahadult.Text));
                empparam[7] = new SqlParameter("@childmax", Convert.ToDecimal(jumlahanak.Text == "" ? "0" : jumlahanak.Text));

                empparam[8] = new SqlParameter("@baseadult", Convert.ToDecimal(baseadult.Text == "" ? "0" : baseadult.Text));
                empparam[9] = new SqlParameter("@baseanak", Convert.ToDecimal(baseanak.Text == "" ? "0" : baseanak.Text));

                empparam[10] = new SqlParameter("@overbooking", Convert.ToDecimal(overbooking.Text == "" ? "0" : overbooking.Text));

                string sql = "insert into setuproomtypes (roomtypes,keterangan,defaultrate,defaultadultrate" +
                                                        ",defaultchildrate,createdby,createddate,adultmax,childmax" +
                                                        ",baseadult,basechild,overbooking) ";
                sql += " values (@roomtypes,@keterangan,@defaultrate,@defaultadultrate,@defaultchildrate" +
                                                        ",'" + session.UserId + "',@createddate,@adultmax,@childmax" +
                                                        ",@baseadult,@baseanak,@overbooking) ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                this.loadTable();
            }
            else if (Page.IsValid && submit.Text == "Update")
            {
                SqlParameter[] empparam = new SqlParameter[13];
                empparam[0] = new SqlParameter("@roomtypes", roomtypes.Text);

                empparam[1] = new SqlParameter("@keterangan", keterangan.Text);

                empparam[2] = new SqlParameter("@defaultrate", Convert.ToDecimal(defaultrate.Text == "" ? "0" : defaultrate.Text));

                empparam[3] = new SqlParameter("@defaultadultrate", Convert.ToDecimal(defaultadultrate.Text == "" ? "0" : defaultadultrate.Text));

                empparam[4] = new SqlParameter("@defaultchildrate", Convert.ToDecimal(defaultchildrate.Text == "" ? "0" : defaultchildrate.Text));

                empparam[5] = new SqlParameter("@createddate", DateTime.Now);

                empparam[6] = new SqlParameter("@recid", Convert.ToInt64(recidparam.Value));

                empparam[7] = new SqlParameter("@updatedby", session.UserId);

                empparam[8] = new SqlParameter("@adultmax", Convert.ToDecimal(jumlahadult.Text == "" ? "0" : jumlahadult.Text));
                empparam[9] = new SqlParameter("@childmax", Convert.ToDecimal(jumlahanak.Text == "" ? "0" : jumlahanak.Text));

                empparam[10] = new SqlParameter("@baseadult", Convert.ToDecimal(baseadult.Text == "" ? "0" : baseadult.Text));
                empparam[11] = new SqlParameter("@baseanak", Convert.ToDecimal(baseanak.Text == "" ? "0" : baseanak.Text));

                empparam[12] = new SqlParameter("@overbooking", Convert.ToDecimal(overbooking.Text == "" ? "0" : overbooking.Text));

                string sql = "update setuproomtypes set roomtypes = @roomtypes" +
                                                    ",keterangan = @keterangan" +
                                                    ",defaultrate = @defaultrate" +
                                                    ",defaultadultrate = @defaultadultrate" +
                                                    ",defaultchildrate = @defaultchildrate" +
                                                    ",updateddate = @createddate" +
                                                    ",updatedby = @updatedby" +
                                                    ",adultmax = @adultmax" +
                                                    ",childmax = @childmax" +
                                                    ",baseadult = @baseadult" +
                                                    ",basechild = @baseanak" +
                                                    ",overbooking = @overbooking";
                sql += " where recid = @recid ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                this.loadTable();
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            loadTable();
        }     
      
        protected void btndelete_Click(object sender, EventArgs e)
        {
            Boolean isexec = false;
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                CheckBox cb = (CheckBox)GridView1.Rows[i].FindControl("chk");
                string columnvalue = ((HiddenField)GridView1.Rows[i].FindControl("recchk")).Value;

                if (cb.Checked == true)
                {
                    isexec = true;
                    dbcon.executeNonQuery(new sysSQLParam("delete from setuproomtypes where recid = " + columnvalue + " ", null));
                    dbcon.closeConnection();
                }
            }
            if (isexec)
                this.loadTable();
        }
    }
}