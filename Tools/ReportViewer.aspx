<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="PCS_JIM_Web.Tools.ReportViewer" %>

<%@ Register Assembly="CrystalDecisions.Web" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=this.FormReportTitle() %></title>
    
   <link href="<%= ResolveUrl("~/aspnet_client/system_web/4_0_30319/crystalreportviewers13/css/default.css") %>" rel="stylesheet" type="text/css" />

   <script src="<%= ResolveUrl("~/aspnet_client/system_web/4_0_30319/crystalreportviewers13/js/crviewer/crv.js") %>" type="text/javascript"></script>
 
</head>
<body>
<form runat="server" method="post" >
         <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server"></asp:ScriptManager>

        
        <CR:CrystalReportViewer ID="rptViewer" runat="server" Width="100%" Height="100%" AutoDataBind="true" />
        <iframe id="iframepdf" class="responsive-iframe" Width="100%" Height="800" src="showPDF.ashx?userid=<%= this.getUserid() %>-reportview.pdf"> </iframe>
</form>
</body>
</html>
