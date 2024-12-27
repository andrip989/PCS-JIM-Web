<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreatePMSKey.aspx.cs" Inherits="PCS_JIM_Web.Module.Submodule.CreatePMSKey" %>

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


 <style>
        .modal {
          display: block; /* Hidden by default */
          position: fixed; /* Stay in place */
          z-index: 1; /* Sit on top */
          padding-top: 100px; /* Location of the box */
          left: 0;
          top: 0;
          width: 100%; /* Full width */
          height: 100%; /* Full height */
          overflow: auto; /* Enable scroll if needed */
          background-color: rgb(0,0,0); /* Fallback color */
          background-color: rgba(0,0,0,0.9); /* Black w/ opacity */
        }

        /* Modal Content (image) */
        .modal-content {
          margin: auto;
          display: block;
          width: 80%;
          max-width: 700px;
        }

        .modal-content, #caption {  
          -webkit-animation-name: zoom;
          -webkit-animation-duration: 0.6s;
          animation-name: zoom;
          animation-duration: 0.6s;
        }

        @-webkit-keyframes zoom {
          from {-webkit-transform:scale(0)} 
          to {-webkit-transform:scale(1)}
        }

        @keyframes zoom {
          from {transform:scale(0)} 
          to {transform:scale(1)}
        }
        
        /* 100% Image Width on Smaller Screens */
        @media only screen and (max-width: 700px){
          .modal-content {
            width: 100%;
          }
        }

    </style>
  

    <script type="text/javascript" language="javascript">

        document.addEventListener('DOMContentLoaded', function () {
            // Kode JavaScript Anda di sini 
            document.getElementById("<%= housekeepbtn2.ClientID %>").click();
        });

    </script>
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
                <li class="breadcrumb-item"><a href="<%= ResolveUrl("default.aspx") %>">Home</a><i class="fa fa-angle-right"></i>Create PMS Key</li>
            </ol>
        </div>
        <div class="grid-form1">
		<!--grid-->
        </div>

<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server"></asp:ScriptManager>
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>


        <div class="w3-agile-chat">
				<div class="charts">
					<div class="col-md-12 w3layouts-char" runat="server" id="hide1">
						<div class="charts-grids widget">
                             
                             <div class="form-group1">
                                <a id="housekeepbtn2" runat="server" onserverclick="btnCreate_Click"><div class="bg-info pv20 text-white fw600 text-center">Create</div></a> 
                                <a id="btnclose" onclick="window.close();" runat="server"><div class="bg-alert light pv20 text-white fw600 text-center">Cancel</div></a>
                             </div>

                        </div>
                    </div>
                </div>
                </div>




    </contenttemplate>
  </asp:UpdatePanel>

             <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                
        <div class="modal" >
                <asp:Image id="image" class="modal-content" runat="server" ImageUrl="~/images/createkey.gif" />
        </div>

                



            </ProgressTemplate>
        </asp:UpdateProgress>
</form>
</div>

  </div>
 	
</body>
</html>