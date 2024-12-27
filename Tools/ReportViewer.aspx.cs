using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PCS_JIM_Web.Library;
using Npgsql;
using CrystalDecisions.Web;

namespace PCS_JIM_Web.Tools
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        string ReportTitle = "";
        sysUserSession session;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["sessionid"] == null)
            {
                HttpContext.Current.Session["sessionid"] = "";
            }

            sysSecurity.checkUserSession(ref session, this.Server, HttpContext.Current.Session["sessionid"].ToString());

            this.generatereport();
        }

        public string getUserid()
        {
            return session.UserId;
        }
        public void generatereport()
        {
            ReportDocument rptDocument = new ReportDocument();

            string reportNameComplete = "";
            string ReportName = "", ReportCriteria = "", ReportParamCollection = "";
            Int64 reportId;

            /* reading report id */
            reportId = Convert.ToInt64(Encoding.UTF8.GetString(Convert.FromBase64String(Request.Url.Query.Remove(0, 1))));

            /* read report */
            try
            {
                sysConnection connection = new sysConnection();

                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@RecId", reportId);
                param[0].SqlDbType = SqlDbType.BigInt; param[0].Size = 8;

                connection.openConnection();

                NpgsqlDataReader reader = connection.executeQuery(new sysSQLParam("select * from sysReportRunner where RecId=@RecId", param));
                if (reader.Read())
                {
                    if (Convert.IsDBNull(reader["reportname"]) == false)
                    {
                        ReportName = Convert.ToString(reader["reportname"]);
                    }

                    if (Convert.IsDBNull(reader["reporttitle"]) == false)
                    {
                        ReportTitle = Convert.ToString(reader["reporttitle"]);
                    }

                    if (Convert.IsDBNull(reader["rptcriteria"]) == false)
                    {
                        ReportCriteria = Convert.ToString(reader["rptcriteria"]);
                    }

                    if (Convert.IsDBNull(reader["rptparameter"]) == false)
                    {
                        ReportParamCollection = Convert.ToString(reader["rptparameter"]);
                    }

                    /* Setting Report Name */
                    reportNameComplete = Request.PhysicalApplicationPath + "\\" + ReportName;
                    rptDocument.Load(reportNameComplete);

                    /* Setting DB Access Connection */
                    /*
                    rptDocument.SetDatabaseLogon("sa", "sa123", ".", "i4te");
                    foreach (CrystalDecisions.CrystalReports.Engine.Table table in rptDocument.Database.Tables)
                    {
                        TableLogOnInfo logonInfo = table.LogOnInfo;
                        logonInfo.ConnectionInfo.ServerName = ".";
                        logonInfo.ConnectionInfo.DatabaseName = "i4te";
                        logonInfo.ConnectionInfo.UserID = "sa";
                        logonInfo.ConnectionInfo.Password = "sa123";

                        table.ApplyLogOnInfo(logonInfo);
                    }
                    */
                    //rptDocument.SetDataSource(reportNameComplete);
                    CrystalDecisions.Shared.ConnectionInfo myConnectionInfo = new CrystalDecisions.Shared.ConnectionInfo();
                    myConnectionInfo.ServerName = sysConfig.ReportODBCDSN();
                    myConnectionInfo.DatabaseName = sysConfig.DatabaseDB();
                    myConnectionInfo.UserID = sysConfig.UsernameDB();
                    myConnectionInfo.Password = sysConfig.PasswordDB();
                    
                    foreach (CrystalDecisions.CrystalReports.Engine.Table item in rptDocument.Database.Tables)
                    {
                        TableLogOnInfo t = item.LogOnInfo;
                        t.ConnectionInfo = myConnectionInfo;
                        item.ApplyLogOnInfo(t);
                    }

                    /* Setting Report Criteria */
                    if (!ReportCriteria.Equals(""))
                        rptDocument.RecordSelectionFormula = ReportCriteria;

                    ParameterFields paramFields = new ParameterFields();
                    ParameterField paramField;
                    ParameterDiscreteValue paramDiscreteValue;

                    /* Setting Report Parameter */
                    if (!ReportParamCollection.Equals(""))
                    {
                        string[] value;
                        String[] reportParam = ReportParamCollection.Split(';');
                        foreach (string rptParam in reportParam)
                        {
                            value = rptParam.Split('=');
                            rptDocument.SetParameterValue(value[0], value[1]);

                            paramField = new ParameterField();
                            paramDiscreteValue = new ParameterDiscreteValue();
                            paramField.Name = value[0];
                            paramDiscreteValue.Value = value[1];
                            paramField.CurrentValues.Add(paramDiscreteValue);
                            paramFields.Add(paramField);
                        }
                    }
                    rptViewer.ParameterFieldInfo = paramFields;
                    /* Refresh */
                    rptViewer.ReportSource = rptDocument;
                    rptViewer.DisplayGroupTree = false;
                    rptViewer.DataBind();// RefreshReport();

                }

                reader.Dispose();
                reader.Close();

                connection.closeConnection();

                rptDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Page.Request.PhysicalApplicationPath + "Temporary\\Download\\" + session.UserId + "-reportview.pdf");
                //cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, Page.Request.PhysicalApplicationPath + "Temporary\\Download\\" + session.UserId + "-uangmakan.xls");
                rptViewer.Visible = false;
            }
            catch (Exception ex)
            {

            }
        }

        public string FormReportTitle()
        {
            return ReportTitle;
        }
    }
}