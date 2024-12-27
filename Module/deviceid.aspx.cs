using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Npgsql;
using PCS_JIM_Web.Library;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace PCS_JIM_Web.Module
{
    public partial class deviceidpage : System.Web.UI.Page 
    {
        sysConnection dbcon;
        sysUserSession session;
        protected void Page_Load(object sender, EventArgs e)
        {
            //MasterPage masterpage = (MasterPage)this.Master;

            //masterpage.BodyTag.Attributes.Add("onFocus", "parent_disable();");
            //masterpage.BodyTag.Attributes.Add("onclick", "parent_disable();");

            if (HttpContext.Current.Session["sessionid"] == null)
            {
                HttpContext.Current.Session["sessionid"] = "";
            }

            sysSecurity.checkUserSession(ref session, this.Server, HttpContext.Current.Session["sessionid"].ToString());


            dbcon = new sysConnection();
            if (!this.IsPostBack)
            {
                sysfunction.setGridStyle(GridView1);

                //GridView1.RowCreated

                this.loadTable();
            }
        }

        private void loadTable()
        {
            deviceid.Text = "";
            description.Text = "";
            ipaddress.Text = "";
            ipport.Text = "";
            ipaddresswifi.Text = "";
            DataTable dt = GetDataFromDatabase("SELECT s.deviceid AS \"Nomor Device Id\"" +
                                            ",s.description AS keterangan " + 
                                            ",s.createdtime "+
                                            ",s.createdby "+
                                            ",s.ipaddresslan AS \"IP Address Lan\"" +
                                            ",s.ipaddresswifi AS \"IP Address Wifi\"" +
                                            ",s.tipeipaddress " +
                                            ",s.ipport " +
                                            ",s.updatedatetime "+
                                            ",s.updatedby "+
                                            ",s.recid "+ 
                                            " FROM setupdevice s");

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

            dbcon.closeConnection();
        }

        DataTable GetDataFromDatabase(string SQLSyntax)
        {
            return dbcon.getdataTable(SQLSyntax);
        }

        protected void SaveClick(object sender, EventArgs e)        
        {
            if (Page.IsValid && submit.Text == "Submit")
            {
                SqlParameter[] empparam = new SqlParameter[8];
                empparam[0] = new SqlParameter("@deviceid", deviceid.Text);

                empparam[1] = new SqlParameter("@description", description.Text);

                empparam[2] = new SqlParameter("@currentTimestamp", DateTime.Now);

                empparam[3] = new SqlParameter("@ipaddress", tipeipaddress.SelectedValue == "1" ? ipaddresswifi.Text : ipaddress.Text);

                empparam[4] = new SqlParameter("@ipport", ipport.Text);
                empparam[5] = new SqlParameter("@ipaddresslan", ipaddress.Text);
                empparam[6] = new SqlParameter("@ipaddresswifi", ipaddresswifi.Text);
                empparam[7] = new SqlParameter("@tipeipaddress", Convert.ToInt32(tipeipaddress.SelectedValue));

                string sql = "insert into setupdevice (deviceid,description,createddate,createdby,createdtime,ipaddress,ipport,ipaddresslan,ipaddresswifi,tipeipaddress) ";
                        sql += " values (@deviceid,@description,NOW(),'"+ session.UserId + "',@currentTimestamp,@ipaddress,@ipport,@ipaddresslan,@ipaddresswifi,@tipeipaddress) ";
                dbcon.executeNonQuery(new sysSQLParam(sql,empparam));

                this.loadTable();
                dbcon.closeConnection();
            }
            else if (Page.IsValid && submit.Text == "Update")
            {
                SqlParameter[] empparam = new SqlParameter[11];
                empparam[0] = new SqlParameter("@deviceid", deviceid.Text);

                empparam[1] = new SqlParameter("@description", description.Text);

                empparam[2] = new SqlParameter("@updateddatetime", DateTime.Now);

                empparam[3] = new SqlParameter("@ipaddress", tipeipaddress.SelectedValue == "1" ? ipaddresswifi.Text : ipaddress.Text);

                empparam[4] = new SqlParameter("@ipport", ipport.Text);

                empparam[5] = new SqlParameter("@currentTimestamp", DateTime.Now);

                empparam[6] = new SqlParameter("@recid", Convert.ToInt64(recidparam.Value));

                empparam[7] = new SqlParameter("@updatedby", session.UserId);

                empparam[8] = new SqlParameter("@ipaddresslan", ipaddress.Text);
                empparam[9] = new SqlParameter("@ipaddresswifi", ipaddresswifi.Text);
                empparam[10] = new SqlParameter("@tipeipaddress", Convert.ToInt32(tipeipaddress.SelectedValue));

                string sql = "update setupdevice set deviceid = @deviceid" +
                                                    ",description = @description" +
                                                    ",updatedatetime = @updateddatetime" +
                                                    ",updatedby = @updatedby" +
                                                    ",ipaddress = @ipaddress" +
                                                    ",ipport = @ipport " +
                                                    ",ipaddresslan = @ipaddresslan" +
                                                    ",ipaddresswifi = @ipaddresswifi" +
                                                    ",tipeipaddress = @tipeipaddress ";
                sql += " where recid = @recid ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));

                this.loadTable();
                dbcon.closeConnection();
            }
        }       

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            loadTable();
        }

        public int GetColumnIndexByDBName(GridView aGridView, String ColumnText)
        {
            System.Web.UI.WebControls.BoundField DataColumn;

            for (int Index = 0; Index < aGridView.Columns.Count; Index++)
            {
                DataColumn = aGridView.Columns[Index] as System.Web.UI.WebControls.BoundField;

                if (DataColumn != null)
                {
                    if (DataColumn.DataField == ColumnText)
                        return Index;
                }
            }
            return -1;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DataRowView dr = (DataRowView)e.Row.DataItem;
                //field1 = dr["createddate"].ToString();
                int index = -1; // karena ada checkbox
                /*
                index = sysfunction.GetColumnIndexByName(e.Row, "createddate");

                e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "createdtime");

                if(e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                  e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.TimeFormat());
                */
                index = sysfunction.GetColumnIndexByName(e.Row, "createdtime");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "updatedatetime");
                
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "tipeipaddress");

                if (e.Row.Cells[index].Text != "")
                {
                    e.Row.Cells[index].Text = e.Row.Cells[index].Text == "1" ? "Wifi":"Lan";
                }
            }
        }

        protected void LoadDevice_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //string test = "ayam";
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "openchild", "openWindowchild()", true);
                RestClient rClient = new RestClient();
                //rClient.endPoint = sysConfig.apisonofflink() + "zeroconf/deviceid";
                rClient.endPoint = "http://" + ipaddress.Text + ":" + ipport.Text + "/" + "zeroconf/deviceid";
                rClient.httpMethod = httpVerb.POST;

                rClient.postJSON = "{  \"data\": {}  }";

                string strResponse = rClient.makeRequest();
                if (strResponse.IndexOf("error") != -1)
                {
                    ResultsysSonOFF obj = JsonConvert.DeserializeObject<ResultsysSonOFF>(strResponse);
                    sysSonOFFdata objchild = obj.data;
                    if (objchild.deviceid != null)
                    {
                        dbcon.executeNonQuery("delete from setupdevice");

                        SqlParameter[] empparam = new SqlParameter[5];
                        empparam[0] = new SqlParameter("@deviceid", objchild.deviceid);

                        empparam[1] = new SqlParameter("@description", objchild.deviceid);

                        empparam[2] = new SqlParameter("@currentTimestamp", DateTime.Now);

                        empparam[3] = new SqlParameter("@ipaddress", ipaddress.Text);

                        empparam[4] = new SqlParameter("@ipport", ipport.Text);

                        string sql = "insert into setupdevice (deviceid,description,createddate,createdby,createdtime,ipaddress,ipport) ";
                        sql += " values (@deviceid,@description,NOW(),'"+ session.UserId + "',@currentTimestamp,@ipaddress,@ipport) ";
                        dbcon.executeNonQuery(new sysSQLParam(sql, empparam));

                        this.loadTable();
                        dbcon.closeConnection();
                    }
                }
                rClient = null;
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

                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from setupdevice where recid = " + recidparam.Value + " ", null));
                if (objreader.Read())
                {
                    deviceid.Text = objreader["deviceid"].ToString();
                    ipaddress.Text = objreader["ipaddresslan"].ToString();
                    ipaddresswifi.Text = objreader["ipaddresswifi"].ToString();
                    tipeipaddress.SelectedValue = objreader["tipeipaddress"].ToString();
                    description.Text = objreader["description"].ToString();
                    ipport.Text = objreader["ipport"].ToString();
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
    }
}