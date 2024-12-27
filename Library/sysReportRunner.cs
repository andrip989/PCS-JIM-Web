using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.UI;
using static System.Collections.Specialized.BitVector32;
using System.Drawing;

namespace PCS_JIM_Web.Library
{
    public class sysReportRunner
    {
        /* sysReportRunner.openReport(this.Page
                                    , "EZBill\\EZRegInfo.rpt"
                                    , "Report Register Info"
                                    , this.createReportCriteria()
                                    , "userid=" + userSession.UserId
                                    //, "EZPartnerInfo" + DateTime.Now.ToString(sysConfig.getValue("Format_REPORT_EXT")) + ".rpt"
                                    );
        */
        public static void openReport(Page page, string _ReportName, string _titleReport, string _rptCriteria, string _rptParameter)
        {
            try
            {
                /* Input to report runner */
                sysConnection connection = new sysConnection();
                SqlParameter[] param = new SqlParameter[4];
                Int64 reportId = 0;

                connection.openConnection();

                param[0] = new SqlParameter("@reportname", _ReportName);
                param[0].SqlDbType = SqlDbType.VarChar; param[0].Size = 50;

                param[1] = new SqlParameter("@reporttitle", _titleReport);
                param[1].SqlDbType = SqlDbType.VarChar; param[1].Size = 100;

                param[2] = new SqlParameter("@rptCriteria", _rptCriteria);
                param[2].SqlDbType = SqlDbType.VarChar; param[2].Size = 8000;

                param[3] = new SqlParameter("@rptParameter", _rptParameter);
                param[3].SqlDbType = SqlDbType.VarChar; param[3].Size = 8000;                

                reportId = Convert.ToInt64(connection.executeScalar(new sysSQLParam("select * from sp_addreportrunner(@reportname,@reporttitle,@rptCriteria,@rptParameter)", param)));

                connection.closeConnection();

                /* Encryption reportid */
                string encode = page.Server.HtmlEncode(Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(reportId))));

                string urlreport = page.ResolveUrl("~/Tools/ReportViewer.aspx?"); 
                /* Running report in windows */
                string strScript = "sysOpenWindow('" + urlreport + encode + "', 'IDF - " + _titleReport + "');";

                //string strScript = "alert('./Tools/ReportViewer.aspx?" + encode + " IDF - " + _titleReport + "');";
                ScriptManager.RegisterStartupScript(page, page.GetType(), DateTime.Now.ToString() + "-" + (new Random()).Next().ToString(), strScript, true);
                //ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "popupwindows", strScript, true);
            }
            catch
            {

            }

        }
        /*
        public static void GenerateReport(Page page, string _ReportName, string _titleReport, string _rptCriteria, string _rptParameter, string _filetempName)
        {
            try
            {
                string exportFileName = _ReportName;
                string exportPath = sysConfig.getValue("Download_Folder") + "\\" + _filetempName;
                string downloadPath = sysConfig.getValue("Download_FolderWeb") + "/" + _filetempName;

                ReportDocument crReportDocument = new ReportDocument();

                crReportDocument.Load(page.Request.PhysicalApplicationPath + sysConfig.getValue("REPORT_DIR") + "\\" + _ReportName);
                if (!_rptCriteria.Equals(""))
                    crReportDocument.RecordSelectionFormula = _rptCriteria;

                if (!_rptParameter.Equals(""))
                {
                    string[] value;
                    String[] reportParam = _rptParameter.Split(';');
                    foreach (string rptParam in reportParam)
                    {
                        value = rptParam.Split('=');
                        crReportDocument.SetParameterValue(value[0], value[1]);
                    }
                }

                //crReportDocument.DataSourceConnections[0].SetConnection(sysConfig.getValue("REPORT_Server"), sysConfig.getValue("REPORT_DB"), sysConfig.getValue("REPORT_User"), sysConfig.getValue("REPORT_Password"));
                crReportDocument.DataSourceConnections[0].SetConnection(sysConfig.getValue("REPORT_Server"), sysConfig.getValue("REPORT_DB"), true);

                ExportOptions crExportOptions;

                DiskFileDestinationOptions crDestOptions = new DiskFileDestinationOptions();

                crDestOptions.DiskFileName = exportPath;
                crExportOptions = crReportDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDestOptions;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //Specify the format in which you want to export (like .doc,.pdf etc)
                crExportOptions.ExportFormatType = ExportFormatType.CrystalReport;

                crReportDocument.Export();
                // Close Report Template 
                crReportDocument.Close();
                // Running report in windows 
                if (File.Exists(exportPath))
                {
                    string strScript = "downloadReport('" + downloadPath + "');";
                    ScriptManager.RegisterStartupScript(page, page.GetType(), DateTime.Now.ToString() + "-" + (new Random()).Next().ToString(), strScript, true);
                }
            }
            catch (Exception ex)
            {
                string strScript = "alert('" + ex.Message.Replace("\r", "").Replace("'", "") + "');";
                strScript = strScript.Replace("\n", "");
                ScriptManager.RegisterStartupScript(page, page.GetType(), DateTime.Now.ToString() + "-" + (new Random()).Next().ToString(), strScript, true);
            }
        }
        */
    }
}