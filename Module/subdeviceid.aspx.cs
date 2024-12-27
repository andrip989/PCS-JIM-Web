using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCS_JIM_Web.Library;
using Newtonsoft.Json;
using System.Threading;
using Npgsql;

namespace PCS_JIM_Web.Module
{
    public partial class subdeviceidpage : System.Web.UI.Page
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

            dbcon = new sysConnection();
            if (!this.IsPostBack)
            {
                sysfunction.setGridStyle(GridView1);

                this.loadTable();

                deviceid.DataSource = GetDataFromDatabase("select * from setupdevice");
                dbcon.closeConnection();
                deviceid.DataTextField = "deviceid";
                deviceid.DataValueField = "deviceid";
                deviceid.AppendDataBoundItems = true;
                deviceid.DataBind();
            }
        }

        private void loadTable()
        {
            subdeviceid.Text = "";
            description.Text = "";
            //if(deviceid.Text != "")
            {
                DataTable dt = GetDataFromDatabase("select * from vwSubDeviceId ");// where \"Nomor Device Id\" = '" + deviceid.SelectedValue + "'");
                dbcon.closeConnection();
                if(dt.Rows.Count != 0) { 
                    ipaddress.Value = dt.Rows[0]["ipaddress"].ToString(); //ambil row pertama aja
                    ipport.Value = dt.Rows[0]["ipport"].ToString();
                }
                GridView1.DataSource = dt;
                //Thread.Sleep(10000);
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
            }
        }

        DataTable GetDataFromDatabase(string SQLSyntax)
        {
            return dbcon.getdataTable(SQLSyntax);
        }

        protected void LoadDevice_Click(object sender, EventArgs e)
        {
            if(ipaddress.Value != "" && ipport.Value != "")
            { 
                RestClient rClient = new RestClient();
                //rClient.endPoint = sysConfig.apisonofflink() + "zeroconf/subDevList";
                rClient.endPoint = "http://" + ipaddress.Value + ":" + ipport.Value + "/" + "zeroconf/subDevList";
                rClient.httpMethod = httpVerb.POST;

                sysSonOFF objchild = new sysSonOFF();
                objchild.deviceid = deviceid.SelectedValue;
                rClient.postJSON = JsonConvert.SerializeObject(objchild);

                string strResponse = rClient.makeRequest();
                if (strResponse.IndexOf("error") != -1)
                {
                    ResultsysSonOFFSub obj = JsonConvert.DeserializeObject<ResultsysSonOFFSub>(strResponse);

                    DataSonOFF dataaa = obj.data;
                    Boolean first = true;
                    foreach (sysSonOFFdataSub devicelist in dataaa.subDevList)
                    {
                        if (first)
                        {
                            dbcon.executeNonQuery("delete from setupsubdevice where deviceid = '"+ objchild.deviceid + "'");
                            first = false;
                            dbcon.closeConnection();
                        }

                        SqlParameter[] empparam = new SqlParameter[4];
                        empparam[0] = new SqlParameter("@subdeviceid", devicelist.subDevId);

                        empparam[1] = new SqlParameter("@description", devicelist.subDevId);

                        empparam[2] = new SqlParameter("@currentTimestamp", DateTime.Now);

                        empparam[3] = new SqlParameter("@deviceid", objchild.deviceid);

                        string sql = "insert into setupsubdevice (subdeviceid,description,createddate,createdby,createdtime,deviceid) ";
                        sql += " values (@subdeviceid,@description,NOW(),'admin',@currentTimestamp,@deviceid) ";
                        dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                        dbcon.closeConnection();
                    }
                
                    if (objchild.deviceid != null)
                    {
                        this.loadTable();
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

                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from setupsubdevice where recid = " + recidparam.Value + " ", null));
                if (objreader.Read())
                {
                    deviceid.SelectedValue = objreader["deviceid"].ToString();
                    subdeviceid.Text = objreader["subdeviceid"].ToString();
                    description.Text = objreader["description"].ToString();
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
                SqlParameter[] empparam = new SqlParameter[4];
                empparam[0] = new SqlParameter("@subdeviceid", subdeviceid.Text);

                empparam[1] = new SqlParameter("@description", description.Text);

                empparam[2] = new SqlParameter("@currentTimestamp", DateTime.Now);

                empparam[3] = new SqlParameter("@deviceid", deviceid.SelectedValue);

                string sql = "insert into setupsubdevice (subdeviceid,description,createddate,createdby,createdtime,deviceid) ";
                sql += " values (@subdeviceid,@description,NOW(),'admin',@currentTimestamp,@deviceid) ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                this.loadTable();
            }
            else if (Page.IsValid && submit.Text == "Update")
            {
                SqlParameter[] empparam = new SqlParameter[6];
                empparam[0] = new SqlParameter("@deviceid", deviceid.SelectedValue);

                empparam[1] = new SqlParameter("@description", description.Text);

                empparam[2] = new SqlParameter("@updateddatetime", DateTime.Now);

                empparam[3] = new SqlParameter("@subdeviceid", subdeviceid.Text);

                empparam[4] = new SqlParameter("@recid", Convert.ToInt64(recidparam.Value));

                empparam[5] = new SqlParameter("@updatedby", session.UserId);

                string sql = "update setupsubdevice set deviceid = @deviceid" +
                                                    ",description = @description" +
                                                    ",subdeviceid = @subdeviceid" +
                                                    ",updatedatetime = @updateddatetime" +
                                                    ",updatedby = @updatedby";
                sql += " where recid = @recid ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                this.loadTable();
            }
        }    

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = -1; // karena ada checkbox

                index = sysfunction.GetColumnIndexByName(e.Row, "createddate");

                e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "createdtime");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.TimeFormat());
                
                index = sysfunction.GetColumnIndexByName(e.Row, "updatedatetime");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());
                
            }
        }

        protected void deviceid_SelectedIndexChanged(object sender, EventArgs e)
        {
            loaddevicebtn.Enabled = deviceid.Text != "";
            this.loadTable();
        }
    }
}