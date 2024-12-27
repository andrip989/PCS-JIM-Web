<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testpage.aspx.cs" Inherits="PCS_JIM_Web.testpage" %>

<%@ Register Assembly="CrystalDecisions.Web" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title></title>     
    <!--
    <script type="text/javascript" src="js/jquery-2.1.4.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
    <link href="css/jquery-ui.css" rel="Stylesheet" type="text/css"/>
    <link href="css/bootstrap.min.css" rel="Stylesheet" type="text/css"/>
    -->

    <script type="text/javascript" src="<%= ResolveUrl("~/js/global.js") %>"></script>    
	<script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-2.1.4.min.js") %>"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-ui.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/Webcam.js") %>"></script>

   <link href="<%= ResolveUrl("~/aspnet_client/system_web/4_0_30319/crystalreportviewers13/css/default.css") %>" rel="stylesheet" type="text/css" />

   <script src="<%= ResolveUrl("~/aspnet_client/system_web/4_0_30319/crystalreportviewers13/js/crviewer/crv.js") %>" type="text/javascript"></script>
 

	<link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/css/jquery-ui.css") %>" />
    <link rel="stylesheet" type='text/css' href="<%= ResolveUrl("~/css/icon-font.min.css") %>" />

    <link rel='stylesheet' type='text/css' href="<%= ResolveUrl("~/css/bootstrap.min.css") %>" />
    <link rel='stylesheet' type='text/css' href="<%= ResolveUrl("~/css/style.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/css/morris.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/css/font-awesome.css") %>" >
    <style type="text/css">
        body { font-family: Arial; font-size: 10pt; }
        table { border: 1px solid #ccc; border-collapse: collapse; }
        table th { background-color: #F7F7F7; color: #333; font-weight: bold; }
        table th, table td { padding: 5px; width: 300px; border: 1px solid #ccc; }
    </style>

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
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/webcamjs/1.0.26/webcam.js"></script>
  

    <script type="text/javascript" language="javascript">
        
        //document.addEventListener('DOMContentLoaded', function () {
        //    document.getElementById("<%= housekeepbtn2.ClientID %>").click();
        //});
        
    </script>


</head>    
<body>    

     <div class="page-container">
   <!--/content-inner-->
   <div class="mother-grid-inner">
              <!--header start here-->
    <form id="form1" runat="server">    


     <div class="grid-form1">
       <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server"></asp:ScriptManager>

          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>

                <div class="w3-agile-chat">
				<div class="charts">
					<div class="col-md-12 w3layouts-char" runat="server" id="hide1">
						<div class="charts-grids widget">
                             
                             <div class="form-group1">
                                <a id="housekeepbtn2" runat="server" onserverclick="btnCreate_Click"><div class="bg-info pv20 text-white fw600 text-center">Create</div></a> 
                                <a id="A1" runat="server" onserverclick="btnCheckout_Click"><div class="bg-danger pv20 text-white fw600 text-center">Checkout</div></a> 
                                <a id="A2" runat="server" onserverclick="btnClear_Click"><div class="bg-dark pv20 text-white fw600 text-center">Clear</div></a> 
                                <a id="A3" runat="server" onserverclick="btnCheck_Click"><div class="bg-dirty pv20 text-white fw600 text-center">Check</div></a> 
                                <a id="A4" runat="server" onserverclick="btnCheckout_Click"><div class="bg-light pv20 text-white fw600 text-center">Duplicate</div></a> 
                                <a id="btnclose" onclick="window.close();" runat="server"><div class="bg-alert light pv20 text-white fw600 text-center">Cancel</div></a>
                             </div>

                        </div>
                    </div>
                </div>
                </div>


                <div class="w3-agile-chat">
				<div class="charts">
					<div class="col-md-12 w3layouts-char" runat="server" id="Div1">
						<div class="charts-grids widget">
                             
                             <div class="form-group1">

                             </div>

                        </div>
                    </div>
                </div>
                </div>
                </ContentTemplate>
             </asp:UpdatePanel>

         <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                
        <div class="modal" >
                <asp:Image id="image" class="modal-content" runat="server" ImageUrl="~/images/createkey.gif" />
        </div>

                



            </ProgressTemplate>
        </asp:UpdateProgress>

    </div>
    </form>    
    </div>

  </div>   
</body>    
</html> 
