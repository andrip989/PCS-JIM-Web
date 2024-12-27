using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Win32;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class setuproom : System.Web.UI.Page
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

                roomrate.Attributes.Add("onkeyup", "formatCurrency(this,'');");
                roomrate.Attributes.Add("onblur", "formatCurrency(this,'blur');");

                this.loadTable();

                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from vwoutlet order by deviceid ,subdeviceid ,outletno ", null));
                int rowidx = 0;
                string tampungkey = "";
                while (objreader.Read())
                {
                    if(tampungkey != objreader["deviceid"].ToString() + objreader["subdeviceid"].ToString())
                    { 
                        listoutletno.Items.Insert(rowidx, new ListItem("--Select Outlet : "+ objreader["descriptionsubdevice"].ToString() + "--", ""));
                        rowidx++;
                    }

                    listoutletno.Items.Insert(rowidx, new ListItem(objreader["outletno"].ToString() +" - " + objreader["description"].ToString(), objreader["outletno"].ToString() + ":" + objreader["recid"].ToString()));
                    rowidx++;

                    tampungkey = objreader["deviceid"].ToString() + objreader["subdeviceid"].ToString();
                }
                objreader.Close();
                dbcon.closeConnection();

                roomtypes.DataSource = dbcon.getdataTable("select * from setuproomtypes");
                dbcon.closeConnection();
                roomtypes.DataTextField = "keterangan";
                roomtypes.DataValueField = "roomtypes";
                roomtypes.AppendDataBoundItems = true;
                roomtypes.DataBind();

                roomamenities.DataSource = dbcon.getdataTable("select * from setuproomamenities");
                dbcon.closeConnection();
                roomamenities.DataTextField = "description";
                roomamenities.DataValueField = "roomamenities";
                roomamenities.AppendDataBoundItems = true;
                roomamenities.DataBind();


                floortype.DataSource = dbcon.getdataTable("select * from setupfloor");
                dbcon.closeConnection();
                floortype.DataTextField = "description";
                floortype.DataValueField = "floortype";
                floortype.DataBind();

                Tab1.CssClass = "Clicked";
                MainView.ActiveViewIndex = 0;
            }
        }

        protected void Tab1_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Clicked";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;
        }

        protected void Tab2_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Clicked";
            Tab3.CssClass = "Initial";
            MainView.ActiveViewIndex = 1;
        }

        protected void Tab3_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Clicked";
            MainView.ActiveViewIndex = 2;
            if(noroom.Text != "")
            sequencetrans.Text = noroom.Text + "-YY-######";
        }

        private string gettablename()
        {
            return "setuproom";
        }

        private void loadTable()
        {
            listoutletno.SelectedValue = "";
            description.Text = "";
            noroom.Text = "";
            sequencetrans.Text = "";
            roomrate.Text = "0";
            roomamenities.ClearSelection();
            housekeepingday.ClearSelection();
            roomtypes.SelectedValue = "";
            phoneext.Text = "";
            dataext.Text = "";
            keycard.Text = "";
            powercode.Text = "";
            allowsmoking.Checked = false;
            active.Checked = false;
            statushousekeeping.Text = "";
            statusmaintenance.Text = "";
            statusroom.Text = "";
            lockprice.Checked = false;

            DataTable dt = dbcon.getdataTable("select * from setuproom order by noroom");
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
                if(rowind != i)
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

                NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from setuproom where recid = " + recidparam.Value + " ", null));
                if (objreader.Read())
                {
                    if (Convert.IsDBNull(objreader["refrecidoutlet"]) == false)
                        listoutletno.SelectedValue = objreader["outletno"].ToString() + ":" + objreader["refrecidoutlet"].ToString();

                    if (Convert.IsDBNull(objreader["floortype"]) == false)
                        floortype.SelectedValue = objreader["floortype"].ToString();

                    noroom.Text = objreader["noroom"].ToString();
                    description.Text = objreader["description"].ToString();
                    if (Convert.IsDBNull(objreader["roomrate"]) == false)
                        roomrate.Text = string.Format("{0:N2}", Convert.ToDecimal(objreader["roomrate"].ToString()));
                    if (Convert.IsDBNull(objreader["numbersequencetrans"]) == false)
                        sequencetrans.Text = objreader["numbersequencetrans"].ToString();
                    if (Convert.IsDBNull(objreader["roomtypes"]) == false)
                        roomtypes.SelectedValue = objreader["roomtypes"].ToString();
                    if (Convert.IsDBNull(objreader["phoneext"]) == false)
                        phoneext.Text = objreader["phoneext"].ToString();
                    if (Convert.IsDBNull(objreader["dataext"]) == false)
                        dataext.Text = objreader["dataext"].ToString();
                    if (Convert.IsDBNull(objreader["keycard"]) == false)
                        keycard.Text = objreader["keycard"].ToString();
                    if (Convert.IsDBNull(objreader["powercode"]) == false)
                        powercode.Text = objreader["powercode"].ToString();
                    if (Convert.IsDBNull(objreader["allowsmoking"]) == false)
                        allowsmoking.Checked = Convert.ToInt16(objreader["allowsmoking"]) == 1 ? true : false;
                    if (Convert.IsDBNull(objreader["active"]) == false)
                        active.Checked = Convert.ToInt16(objreader["active"]) == 1 ? true : false;
                    if (Convert.IsDBNull(objreader["statushousekeeping"]) == false)
                        statushousekeeping.Checked = Convert.ToInt16(objreader["statushousekeeping"]) == 1 ? true : false;
                    if (Convert.IsDBNull(objreader["statusmaintenance"]) == false)
                        statusmaintenance.Checked = Convert.ToInt16(objreader["statusmaintenance"]) == 1 ? true : false;
                    if (Convert.IsDBNull(objreader["statusroom"]) == false)
                        statusroom.Checked = objreader["statusroom"].ToString() == "on" ? true : false;

                    if (Convert.IsDBNull(objreader["lockprice"]) == false)
                        lockprice.Checked = Convert.ToInt16(objreader["lockprice"]) == 1 ? true : false;
                    if (Convert.IsDBNull(objreader["fixedrate"]) == false)
                        fixedrate.Checked = Convert.ToInt16(objreader["fixedrate"]) == 1 ? true : false;

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

                    if (Convert.IsDBNull(objreader["housekeepingday"]) == false)
                    {
                        string[] ids = objreader["housekeepingday"].ToString().Split(new string[] { "," }, StringSplitOptions.None);
                        foreach (ListItem item in housekeepingday.Items)
                        {
                            if (ids.Contains(item.Value))
                            {
                                item.Selected = true;
                            }
                        }
                    }
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
            if (noroom.Text == "" || roomtypes.SelectedValue == "")
            {
                this.Tab1_Click(null, null);
                Page.Validate("valGroup3");
            }
            else if (listoutletno.SelectedValue == "" || sequencetrans.Text == "")
            {
                this.Tab3_Click(null, null);
                Page.Validate("valGroup3");
            }

            if (Page.IsValid && submit.Text == "Submit")
            {
                List<ListItem> selectedamenities = roomamenities.Items.Cast<ListItem>()
                .Where(li => li.Selected)
                .ToList();

                List<ListItem> selectedhousekeeping = housekeepingday.Items.Cast<ListItem>()
                .Where(li => li.Selected)
                .ToList();

                string roomamenitiesval = String.Join(",",
                                          roomamenities.Items.OfType<ListItem>().Where(r => r.Selected)
                                         .Select(r => r.Value));
                string housekeepingval = String.Join(",",
                                          housekeepingday.Items.OfType<ListItem>().Where(r => r.Selected)
                                         .Select(r => r.Value));

                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@outletno", listoutletno.SelectedValue.Split(new string[] { ":" }, StringSplitOptions.None)[0]));
                list.Add(new SqlParameter("@description", description.Text));
                list.Add(new SqlParameter("@currentTimestamp", DateTime.Now));
                list.Add(new SqlParameter("@noroom", noroom.Text));
                list.Add(new SqlParameter("@refrecidoutlet", Convert.ToInt64(listoutletno.SelectedValue.Split(new string[] { ":" }, StringSplitOptions.None)[1])));
                list.Add(new SqlParameter("@sequencetrans", sequencetrans.Text));
                list.Add(new SqlParameter("@roomrate", roomrate.Text == "" ? 0 : Convert.ToDecimal(roomrate.Text)));
                list.Add(new SqlParameter("@roomtypes", roomtypes.SelectedValue));
                list.Add(new SqlParameter("@floortype" , floortype.SelectedValue));
                list.Add(new SqlParameter("@phoneext", phoneext.Text));
                list.Add(new SqlParameter("@dataext", dataext.Text));
                list.Add(new SqlParameter("@keycard", keycard.Text));
                list.Add(new SqlParameter("@powercode", powercode.Text));
                list.Add(new SqlParameter("@allowsmoking", allowsmoking.Checked ? 1 : 0));
                list.Add(new SqlParameter("@active", active.Checked ? 1 : 0));
                list.Add(new SqlParameter("@roomamenities", roomamenitiesval));
                list.Add(new SqlParameter("@housekeepingday", housekeepingval));
                list.Add(new SqlParameter("@lockprice", lockprice.Checked ? 1 : 0));
                list.Add(new SqlParameter("@fixedrate", fixedrate.Checked ? 1 : 0));

                
                SqlParameter[] empparam = new SqlParameter[list.Count];
                empparam = list.ToArray();

                string sql = "insert into setuproom (roomrate,outletno,description,noroom" +
                                                    ",createdby,createddatetime,numbersequencetrans,refrecidoutlet" +
                                                    ",roomtypes,floortype,phoneext,dataext,keycard" +
                                                    ",powercode,allowsmoking,active" +
                                                    ",roomamenities,housekeepingday,lockprice,fixedrate) ";
                sql += " values (@roomrate,@outletno,@description,@noroom" +
                                ",'" + session.UserId + "',@currentTimestamp,@sequencetrans , @refrecidoutlet" +
                                ",@roomtypes,@floortype,@phoneext,@dataext,@keycard" +
                                ",@powercode,@allowsmoking,@active" +
                                ",@roomamenities,@housekeepingday,@lockprice,@fixedrate) ";
                dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                dbcon.closeConnection();
                this.loadTable();
            }
            else if (Page.IsValid && submit.Text == "Update")
            {
                List<ListItem> selectedamenities = roomamenities.Items.Cast<ListItem>()
                .Where(li => li.Selected)
                .ToList();

                List<ListItem> selectedhousekeeping = housekeepingday.Items.Cast<ListItem>()
                .Where(li => li.Selected)
                .ToList();

                string roomamenitiesval = String.Join(",",
                                          roomamenities.Items.OfType<ListItem>().Where(r => r.Selected)
                                         .Select(r => r.Value));
                string housekeepingval = String.Join(",",
                                          housekeepingday.Items.OfType<ListItem>().Where(r => r.Selected)
                                         .Select(r => r.Value));

                var list = new List<SqlParameter>();
                list.Add(new SqlParameter("@outletno", listoutletno.SelectedValue.Split(new string[] { ":" }, StringSplitOptions.None)[0]));
                list.Add(new SqlParameter("@description", description.Text));
                list.Add(new SqlParameter("@updateddatetime", DateTime.Now));
                list.Add(new SqlParameter("@noroom", noroom.Text));
                list.Add(new SqlParameter("@recid", Convert.ToInt64(recidparam.Value)));
                list.Add(new SqlParameter("@updatedby", session.UserId));
                list.Add(new SqlParameter("@refrecidoutlet", Convert.ToInt64(listoutletno.SelectedValue.Split(new string[] { ":" }, StringSplitOptions.None)[1])));
                list.Add(new SqlParameter("@sequencetrans", sequencetrans.Text));
                list.Add(new SqlParameter("@roomrate", roomrate.Text == "" ? 0 : Convert.ToDecimal(roomrate.Text)));
                list.Add(new SqlParameter("@roomtypes", roomtypes.SelectedValue));
                list.Add(new SqlParameter("@floortype", floortype.SelectedValue));
                list.Add(new SqlParameter("@phoneext", phoneext.Text));
                list.Add(new SqlParameter("@dataext", dataext.Text));
                list.Add(new SqlParameter("@keycard", keycard.Text));
                list.Add(new SqlParameter("@powercode", powercode.Text));
                list.Add(new SqlParameter("@allowsmoking", allowsmoking.Checked ? 1 : 0));
                list.Add(new SqlParameter("@active", active.Checked ? 1 : 0));
                list.Add(new SqlParameter("@roomamenities", roomamenitiesval));
                list.Add(new SqlParameter("@housekeepingday", housekeepingval));
                list.Add(new SqlParameter("@lockprice", lockprice.Checked ? 1 : 0));
                list.Add(new SqlParameter("@fixedrate", fixedrate.Checked ? 1 : 0));
                list.Add(new SqlParameter("@statusmaintenance", statusmaintenance.Checked ? 1 : 0));

                SqlParameter[] empparam = new SqlParameter[list.Count];
                empparam = list.ToArray();

                string sql = "update setuproom set outletno = @outletno" +
                                                    ",roomrate = @roomrate" +
                                                    ",description = @description" +
                                                    ",noroom = @noroom" +
                                                    ",numbersequencetrans = @sequencetrans" +
                                                    ",refrecidoutlet = @refrecidoutlet" +
                                                    ",updateddatetime = @updateddatetime" +
                                                    ",updatedby = @updatedby" +
                                                    ",roomtypes = @roomtypes" +
                                                    ",floortype = @floortype" +
                                                    ",phoneext = @phoneext" +
                                                    ",dataext = @dataext" +
                                                    ",keycard = @keycard" +
                                                    ",powercode = @powercode" +
                                                    ",allowsmoking = @allowsmoking" +
                                                    ",active = @active" +
                                                    ",roomamenities = @roomamenities" +
                                                    ",housekeepingday = @housekeepingday" +
                                                    ",fixedrate = @fixedrate" +
                                                    ",statusmaintenance = @statusmaintenance" + 
                                                    ",lockprice = @lockprice";
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = -1; // karena ada checkbox

                index = sysfunction.GetColumnIndexByName(e.Row, "createddatetime");

                e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());

                index = sysfunction.GetColumnIndexByName(e.Row, "updateddatetime");

                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = Convert.ToDateTime(e.Row.Cells[index].Text).ToString(sysConfig.DateTimeFormat());
                /*
                index = sysfunction.GetColumnIndexByName(e.Row, "roomrate");
                if (e.Row.Cells[index].Text != "" && e.Row.Cells[index].Text != "&nbsp;")
                    e.Row.Cells[index].Text = string.Format("{0:N2}", Convert.ToDecimal(e.Row.Cells[index].Text));
                */
            }
        }

        protected void listoutletno_DataBound(object sender, EventArgs e)
        {

        }

        protected void listoutletno_DataBinding(object sender, EventArgs e)
        {

        }

        protected void listoutletno_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                    dbcon.executeNonQuery(new sysSQLParam("delete from setuproom where recid = " + columnvalue + " ", null));
                    dbcon.closeConnection();                    
                }
            }
            if(isexec)
            this.loadTable();
        }
    }
}