<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="shiftopen.aspx.cs" Inherits="PCS_JIM_Web.Module.Submodule.shiftopen" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title><%=this.ApplicationTitle() %> - <%=this.ApplicationLokasi() %></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="Pooled Responsive web template, Bootstrap Web Templates, Flat Web Templates, Android Compatible web template, 
    Smartphone Compatible web template, free webdesigns for Nokia, Samsung, LG, SonyEricsson, Motorola web design" />
    <script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); } </script>

    <link href='//fonts.googleapis.com/css?family=Roboto:700,500,300,100italic,100,400' rel='stylesheet' type='text/css'/>
    <link href='//fonts.googleapis.com/css?family=Montserrat:400,700' rel='stylesheet' type='text/css'>

    <link href="<%= ResolveUrl("~/css/bootstrap.min.css") %>" rel='stylesheet' type='text/css' />
    <link href="<%= ResolveUrl("~/css/style.css") %>" rel='stylesheet' type='text/css' />
    <link href="<%= ResolveUrl("~/css/morris.css") %>" rel="stylesheet" type="text/css"/>
    <link href="<%= ResolveUrl("~/css/font-awesome.css") %>" rel="stylesheet"> 
    <link href="<%= ResolveUrl("~/css/icon-font.min.css") %>" rel="stylesheet" type='text/css' />
	<script src="<%= ResolveUrl("~/js/jquery-2.1.4.min.js") %>"></script>
	<script src="<%= ResolveUrl("~/js/jquery-ui.js") %>"></script>

	<link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/css/jquery-ui.css") %>" />

	<link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/css/table-style.css") %>" />
	<link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/css/basictable.css") %>" />
	<script type="text/javascript" src="<%= ResolveUrl("~/js/jquery.basictable.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/js/global.js") %>"></script>   
</head>
<body>
     <div class="page-container">
   <!--/content-inner-->
   <div class="mother-grid-inner">
              <!--header start here-->
				
<!--heder end here-->
 		<form id="form1" runat="server">
       <div class="grid-form1">
	        <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="<%= ResolveUrl("default.aspx") %>">Home</a><i class="fa fa-angle-right"></i>Cashier</li>
            </ol>
        </div>
        <div class="grid-form1">
		<!--grid-->
 		<h3 id="forms-example"><asp:label runat="server" ID="typetransaksi"></asp:label> <asp:TextBox ID="transactionid" runat="server" BorderStyle="None" ReadOnly="true" Visible="false"></asp:TextBox> </h3>
        </div>

<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server"></asp:ScriptManager>
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>

  <div class="w3-agile-chat">
				<div class="charts">
					<div class="col-md-8 w3layouts-char" runat="server" id="hide1">
						<div class="charts-grids widget">
                         <div class="form-group1">
                              <label for="totalcharges">Saldo Awal</label>
                              <asp:TextBox ID="saldoawal" runat="server" CssClass="form-control"></asp:TextBox>
                          </div>

                            <div class="form-group1">
                              <label for="totalcharges">Saldo Akhir</label>
                              <asp:TextBox ID="saldoakhir" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                          </div>


                             <div class="form-group1">
                                <asp:Button ID="submit" runat="server" Text="Submit" CssClass="btn-primary btn" OnClick="cashierbtn_ServerClick" />
                             </div>

						</div>
					</div>				                  
					
           </div>
    </div>

  </contenttemplate>
  </asp:UpdatePanel>
</form>
</div>

  </div>
 	
</body>
</html>