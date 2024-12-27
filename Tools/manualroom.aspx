<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="manualroom.aspx.cs" Inherits="PCS_JIM_Web.Tools.manualroom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript" language="javascript">

		function clickoutletevent(event,param)
        {
            if (param == 'reloadoutlet') {
                __doPostBack(event, param);
            }
            else
            {
                var skalar = param == 'on' ? 'off' : 'on';

                //var x = window.confirm("Are you sure turn " + skalar + " electricity ?")

                let person = prompt("Turn " + skalar + " electricity ? , Alasan : ", "");

                if (person != null && person != "")
                {
                    __doPostBack(event, param + ":" + person);

                    return true;
                } else {
                    alert("Alasan wajib diisi");
                    __doPostBack(event, 'reloadoutlet');
                    return false;
                }
                
            }
		}
		
     </script>

	<script type="text/javascript" language="javascript">

        var timer = setInterval(checkChild, 500);

        function checkChild() {
            if (newWindow.closed) {
                //alert("Child window closed");
				clearInterval(timer);
                //window.location.reload();
				clickoutletevent('','reloadoutlet');
            }
		}

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Tools<i class="fa fa-angle-right"></i>Manual ON/OFF</li>
            </ol>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate>
                <div class="panel1 variations-panel">	
                    <div class="form-group">
                        <label for="submit" class="col-sm-2 control-label hor-form">Synchronize room :</label>
                        <div class="col-sm-2">
					  <asp:Button ID="submit" runat="server" Text="Synch" CssClass="btn btn-hover btn-dark btn-block" OnClick="submit_Click"  />
                         </div>
                     </div>
                    <div class="clearfix"> </div>
						 <hr />

                    	<div class="panel-body mtn">
							<div class="col-adjust-10">							
							
							<asp:placeholder runat="server" ID="mainpagediv">
							</asp:placeholder>

								<div class="clearfix"> </div>
							</div>
						</div>
				</div>
            </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
