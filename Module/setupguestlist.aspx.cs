using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using PCS_JIM_Web.Library;

namespace PCS_JIM_Web.Module
{
    public partial class setupguestlist : System.Web.UI.Page
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
                GridView1.AutoGenerateColumns = false;

                ((BoundField)GridView1.Columns[9]).DataFormatString = "{0:" + sysConfig.DateTimeFormat() + "}";

                this.loadTable();

            }
            else
            {
                string parameter = Request["__EVENTARGUMENT"]; // parameter
                string value = Request["__EVENTTARGET"]; // Request["__EVENTTARGET"]; // btnSave

                if (parameter.Contains("refreshdata"))
                {
                    this.loadTable();
                }
            }
        }

        private void loadTable()
        {
            DataTable dt;
            if (txtSearch.Text != "")
            {
                string[] splittext = txtSearch.Text.Split(new string[] { " " }, StringSplitOptions.None);
                if (splittext.Length == 2)
                {
                    dt = dbcon.getdataTable("select * from setupguestlist " +
                                            "where lower(firstname) like '%" + splittext[0].ToLower() + "%' " +
                                            "or lower(lastname) like '%" + splittext[1].ToLower() + "%' " +
                                            "order by createddate");
                }
                else
                {
                    dt = dbcon.getdataTable("select * from setupguestlist " +
                                                "where lower(firstname) like '%" + txtSearch.Text.ToLower() + "%' " +
                                                "or lower(lastname) like '%" + txtSearch.Text.ToLower() + "%' " +
                                                "or lower(email) like '%" + txtSearch.Text.ToLower() + "%' " +
                                                "order by createddate");
                }
            }
            else
                dt = dbcon.getdataTable("select * from setupguestlist order by createddate");


            dbcon.closeConnection();
            GridView1.DataSource = dt;
            GridView1.DataBind();
            /*
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                CheckBox cb = (CheckBox)GridView1.Rows[i].FindControl("chk");
                cb.Enabled = true;
                if (cb.Checked == true)
                    cb.Checked = false;
            }
            */
            labelbtncreated.Text = "Create";
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
                //Emp.Add(objreader["noroom"].ToString());
                Emp.Add(string.Format("{0}-{1} {2}-{3}", objreader["custcode"], objreader["firstname"], objreader["lastname"], objreader["identificationid"]));
            }

            //Emp.Add("AAA");
            //Emp.Add("BBB");
            objreader.Close();
            dbcon.closeConnection();

            return Emp.ToArray();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            loadTable();
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            loadTable();
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
                hfCustomerId.Value = columnvalue;
                labelbtncreated.Text = "Update";
            }
            cb.Enabled = false;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int index = -1; // karena ada checkbox            

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                /*
                index = sysfunction.GetColumnIndexByName(e.Row, "custcode");

                string uriparam = ResolveUrl("~/Module/Submodule/custcreate.aspx");
                uriparam += "?custcodeparam=" + e.Row.Cells[index].Text;

                LinkButton myLink = new LinkButton();
                myLink.ID = "RoomBtn" + e.Row.Cells[index].Text;
                myLink.Text = e.Row.Cells[index].Text;
                //myLink.OnClientClick = "openWindowchild('" + uriparam + "');";                
                myLink.OnClientClick = "alert('" + uriparam + "');";
                
                e.Row.Cells[index].Controls.Add(myLink);
                myLink = null;
                */
            }
        }

        public string getopenURL()
        {
            string uriparam = ResolveUrl("Submodule/custcreate.aspx");
            return "openWindowchild('" + uriparam + "');";
        }

        protected void btncreated_Click(object sender, EventArgs e)
        {
            //int status = 0;

            //isexec = false;
            string uriparam = ResolveUrl("Submodule/custcreate.aspx");
            uriparam += "?custcodeparam=" + hfCustomerId.Value;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "popupwindows", "openWindowchild('" + uriparam + "');", true);

        }
    }
}