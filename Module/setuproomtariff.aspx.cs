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
using System.Web.Services;

namespace PCS_JIM_Web.Module
{
    public partial class setuproomtariff : System.Web.UI.Page
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

                tariff.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                tariff.Attributes.Add("onblur", "formatCurrency(this,'blur');");
                extraadult.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                extraadult.Attributes.Add("onblur", "formatCurrency(this,'blur');");
                extrachild.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                extrachild.Attributes.Add("onblur", "formatCurrency(this,'blur');");

                floortype.DataSource = dbcon.getdataTable("select * from setupfloor");
                dbcon.closeConnection();
                floortype.DataTextField = "description";
                floortype.DataValueField = "floortype";
                floortype.AppendDataBoundItems = true;
                floortype.DataBind();

                roomtypes.DataSource = dbcon.getdataTable("select * from setuproomtypes");
                dbcon.closeConnection();
                roomtypes.DataTextField = "keterangan";
                roomtypes.DataValueField = "roomtypes";
                roomtypes.AppendDataBoundItems = true;
                roomtypes.DataBind();

                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from setuproom order by noroom ", null));
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

                ratetypes.DataSource = dbcon.getdataTable("select * from setupratetype");
                dbcon.closeConnection();
                ratetypes.DataTextField = "keterangan";
                ratetypes.DataValueField = "ratetype";
                ratetypes.AppendDataBoundItems = true;
                ratetypes.DataBind();

                seasontype.DataSource = dbcon.getdataTable("select * from setupseasontype");
                dbcon.closeConnection();
                seasontype.DataTextField = "description";
                seasontype.DataValueField = "seasontype";
                seasontype.AppendDataBoundItems = true;
                seasontype.DataBind();

                bussinesssource.DataSource = dbcon.getdataTable("select * from setupbusinesssources");
                dbcon.closeConnection();
                bussinesssource.DataTextField = "companyname";
                bussinesssource.DataValueField = "alias";
                bussinesssource.AppendDataBoundItems = true;
                bussinesssource.DataBind();

                //custcode.SelectedValue = "";

                this.loadTable();

            }
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
                                                                            "", null));
            while (objreader.Read())
            {
                //Emp.Add(objreader["noroom"].ToString());
                Emp.Add(string.Format("{0}-{1} {2}", objreader["custcode"], objreader["firstname"], objreader["lastname"]));
            }

            //Emp.Add("AAA");
            //Emp.Add("BBB");
            objreader.Close();
            dbcon.closeConnection();

            return Emp.ToArray();
        }

        private string gettablename()
        {
            return "setuproomtariff";
        }

        private void loadTable()
        {
            startdate.Text = "";
            tariff.Text = "";
            extraadult.Text = "";
            extrachild.Text = "";
            floortype.SelectedValue = "";
            roomtypes.SelectedValue = "";
            noroom.SelectedValue = "";
            ratetypes.SelectedValue = "";
            seasontype.SelectedValue = "";
            bussinesssource.SelectedValue = "";
            custcode.Value = "";
            usetax.Checked = false;
            active.Checked = false;

            DataTable dt = dbcon.getdataTable("select * from " + this.gettablename() + " order by recid");
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

            btndelete.Enabled = true;
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

                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from " + this.gettablename() + " where recid = " + recidparam.Value + " ", null));
                if (objreader.Read())
                {
                    if (Convert.IsDBNull(objreader["startdate"]) == false)
                        startdate.Text = Convert.ToDateTime(objreader["startdate"]).ToString("yyyy-MM-dd");
                    if (Convert.IsDBNull(objreader["tariff"]) == false)
                        tariff.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["tariff"].ToString()));
                    if (Convert.IsDBNull(objreader["extraadult"]) == false)
                        extraadult.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["extraadult"].ToString()));
                    if (Convert.IsDBNull(objreader["extrachild"]) == false)
                        extrachild.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["extrachild"].ToString()));
                    if (Convert.IsDBNull(objreader["usetax"]) == false)
                        usetax.Checked = Convert.ToInt16(objreader["usetax"]) == 1 ? true : false;
                    if (Convert.IsDBNull(objreader["active"]) == false)
                        active.Checked = Convert.ToInt16(objreader["active"]) == 1 ? true : false;

                    if (Convert.IsDBNull(objreader["floortype"]) == false)
                        floortype.SelectedValue = objreader["floortype"].ToString();
                    if (Convert.IsDBNull(objreader["roomtypes"]) == false)
                        roomtypes.SelectedValue = objreader["roomtypes"].ToString();
                    if (Convert.IsDBNull(objreader["noroom"]) == false)
                        noroom.SelectedValue = objreader["noroom"].ToString();
                    if (Convert.IsDBNull(objreader["ratetypes"]) == false)
                        ratetypes.SelectedValue = objreader["ratetypes"].ToString();
                    if (Convert.IsDBNull(objreader["seasontype"]) == false)
                        seasontype.SelectedValue = objreader["seasontype"].ToString();
                    if (Convert.IsDBNull(objreader["bussinesssource"]) == false)
                        bussinesssource.SelectedValue = objreader["bussinesssource"].ToString();
                    if (Convert.IsDBNull(objreader["custcode"]) == false)
                        custcode.Value = objreader["custcode"].ToString();
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

                string sqlinsert = "INSERT INTO " + this.gettablename() + " (startdate" +
                                                                    ", tariff" +
                                                                    ", extraadult" +
                                                                    ", extrachild" +
                                                                    ", usetax" +
                                                                    ", active" +
                                                                    ", floortype" +
                                                                    ", roomtypes" +
                                                                    ", noroom" +
                                                                    ", ratetypes" +
                                                                    ", seasontype" +
                                                                    ", bussinesssource" +
                                                                    ", custcode " +
                                                                    ", createddate" +
                                                                    ", createdby) " +
                                                                    "VALUES(@startdate" +
                                                                    ", @tariff" +
                                                                    ", @extraadult" +
                                                                    ", @extrachild" +
                                                                    ", @usetax" +
                                                                    ", @active" +
                                                                    ", @floortype" +
                                                                    ", @roomtypes" +
                                                                    ", @noroom" +
                                                                    ", @ratetypes" +
                                                                    ", @seasontype" +
                                                                    ", @bussinesssource" +
                                                                    ", @custcode" +
                                                                    ", @createddate" +
                                                                    ", @createdby)";
                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@startdate", startdate.Text == "" ? new DateTime() : Convert.ToDateTime(startdate.Text)));
                list.Add(new SqlParameter("@tariff", tariff.Text == "" ? 0 : Convert.ToDecimal(tariff.Text)));
                list.Add(new SqlParameter("@extrachild", extrachild.Text == "" ? 0 : Convert.ToDecimal(extrachild.Text)));
                list.Add(new SqlParameter("@extraadult", extraadult.Text == "" ? 0 : Convert.ToDecimal(extraadult.Text)));
                list.Add(new SqlParameter("@usetax", usetax.Checked ? 1 : 0));
                list.Add(new SqlParameter("@active", active.Checked ? 1 : 0));

                list.Add(new SqlParameter("@floortype", floortype.SelectedValue));
                list.Add(new SqlParameter("@roomtypes", roomtypes.SelectedValue));
                list.Add(new SqlParameter("@noroom", noroom.SelectedValue));
                list.Add(new SqlParameter("@ratetypes", ratetypes.SelectedValue));
                list.Add(new SqlParameter("@seasontype", seasontype.SelectedValue));
                list.Add(new SqlParameter("@bussinesssource", bussinesssource.SelectedValue));
                list.Add(new SqlParameter("@custcode", custcode.Value));

                list.Add(new SqlParameter("@createddate", DateTime.Now));
                list.Add(new SqlParameter("@createdby", session.UserId));

                SqlParameter[] empparam = new SqlParameter[list.Count];

                empparam = list.ToArray();

                dbcon.executeNonQuery(new sysSQLParam(sqlinsert, empparam));
                dbcon.closeConnection();
                this.loadTable();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "alert(\"Save Success\");", true);
            }
            else if (Page.IsValid && submit.Text == "Update")
            {

                string sql = "UPDATE " + this.gettablename() + " SET startdate=@startdate" +
                                                           ", tariff=@tariff" +
                                                           ", extrachild=@extrachild" +
                                                           ", extraadult=@extraadult" +
                                                           ", usetax=@usetax" +
                                                           ", active=@active" +
                                                           ", floortype=@floortype" +
                                                           ", roomtypes=@roomtypes" +
                                                           ", noroom=@noroom" +
                                                           ", ratetypes=@ratetypes" +
                                                           ", seasontype=@seasontype" +
                                                           ", bussinesssource=@bussinesssource" +
                                                           ", custcode=@custcode" +                                                           
                                                           ", updateddate=@updateddate" +
                                                           ", updatedby=@updatedby";
                sql += " where recid = @recid ";

                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@startdate", startdate.Text == "" ? new DateTime() : Convert.ToDateTime(startdate.Text)));
                list.Add(new SqlParameter("@tariff", tariff.Text == "" ? 0 : Convert.ToDecimal(tariff.Text)));
                list.Add(new SqlParameter("@extrachild", extrachild.Text == "" ? 0 : Convert.ToDecimal(extrachild.Text)));
                list.Add(new SqlParameter("@extraadult", extraadult.Text == "" ? 0 : Convert.ToDecimal(extraadult.Text)));
                list.Add(new SqlParameter("@usetax", usetax.Checked ? 1 : 0));
                list.Add(new SqlParameter("@active", active.Checked ? 1 : 0));

                list.Add(new SqlParameter("@floortype", floortype.SelectedValue));
                list.Add(new SqlParameter("@roomtypes", roomtypes.SelectedValue));
                list.Add(new SqlParameter("@noroom", noroom.SelectedValue));
                list.Add(new SqlParameter("@ratetypes", ratetypes.SelectedValue));
                list.Add(new SqlParameter("@seasontype", seasontype.SelectedValue));
                list.Add(new SqlParameter("@bussinesssource", bussinesssource.SelectedValue));
                list.Add(new SqlParameter("@custcode", custcode.Value));
                list.Add(new SqlParameter("@recid", Convert.ToInt64(recidparam.Value)));
                list.Add(new SqlParameter("@updateddate", DateTime.Now));
                list.Add(new SqlParameter("@updatedby", session.UserId));

                SqlParameter[] empparam = new SqlParameter[list.Count];

                empparam = list.ToArray();
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                this.loadTable();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "alert(\"Update Success\");", true);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            loadTable();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = -1; // karena ada checkbox

                index = sysfunction.GetColumnIndexByName(e.Row, "startdate");

                e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "createddate");

                e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

               /* index = sysfunction.GetColumnIndexByName(e.Row, "updateddate");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());
               */
                index = sysfunction.GetColumnIndexByName(e.Row, "tariff");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));

                index = sysfunction.GetColumnIndexByName(e.Row, "extrachild");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));

                index = sysfunction.GetColumnIndexByName(e.Row, "extraadult");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(e.Row.Cells[index].Text));

                
            }
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
                    dbcon.executeNonQuery(new sysSQLParam("delete from " + this.gettablename() + " where recid = " + columnvalue + " ", null));
                    dbcon.closeConnection();
                }
            }
            if (isexec)
            {
                this.loadTable();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closewindows", "alert(\"Delete Success\");", true);
            }
        }

        protected void roomtypes_TextChanged(object sender, EventArgs e)
        {
            NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from setuproomtypes where " +
                                                                "roomtypes = '"+roomtypes.SelectedValue+"' ", null));
            if (objreader.Read())
            {
                if (Convert.IsDBNull(objreader["defaultrate"]) == false)
                    tariff.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["defaultrate"].ToString()));
                if (Convert.IsDBNull(objreader["defaultadultrate"]) == false)
                    extraadult.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["defaultadultrate"].ToString()));
                if (Convert.IsDBNull(objreader["defaultchildrate"]) == false)
                    extrachild.Text = string.Format(sysConfig.CurrencyFormat(), Convert.ToDecimal(objreader["defaultchildrate"].ToString()));
                                
            }

            objreader.Close();
            dbcon.closeConnection();
        }
    }
}