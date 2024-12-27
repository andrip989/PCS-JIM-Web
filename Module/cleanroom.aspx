<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="cleanroom.aspx.cs" Inherits="PCS_JIM_Web.Module.cleanroom" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">

		function clickoutletevent(event,param)
		{
            __doPostBack(event, param);
		}
		
    </script>

	<script type="text/javascript" language="javascript">
		
        var timer = setInterval(checkChild, 1000);

        function checkChild() {
			//clickoutletevent('','reloadoutlet');
		}
		
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>House Keeping<i class="fa fa-angle-right"></i>Clean Room</li>
            </ol>

                <div class="panel1 variations-panel">	
					
                    	<div class="panel-body mtn">
							<div class="col-adjust-10">							
							 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
			                    <ContentTemplate>

							<asp:Timer ID="Timer" runat="server" OnTick="Timer1_Tick" Interval="5000" />

							<asp:placeholder runat="server" ID="mainpagediv">
							</asp:placeholder>
                                 </ContentTemplate>
                            </asp:UpdatePanel>

								<div class="clearfix"> </div>
							</div>
						</div>
				</div>


         <asp:UpdatePanel ID="UpdatePanel2" runat="server">		
		 <ContentTemplate>	
	<div class="agile3-grids">	
				<!-- grids -->
				<div class="grids">
				
					<div class="panel1 panel-widget top-grids">
						<div class="chute chute-center text-center">							
							<div class="row mb40">
								<div class="col-md-2">
									<div class="demo-grid">
										<h4><asp:Label ID="vacanttext" runat="server"></asp:Label></h4>
									</div>
								</div>
								<div class="col-md-2">
									<div class="demo-grid-occupied">
										<h4><asp:Label ID="occupiedtext" runat="server"></asp:Label></h4>
									</div>
								</div>
								<div class="col-md-2">
									<div class="demo-grid-reserved">
										<h4><asp:Label ID="reservedtext" runat="server"></asp:Label></h4>
									</div>
								</div>
								<div class="col-md-2">
									<div class="demo-grid-outoforder">
										<h4><asp:Label ID="outofordertext" runat="server"></asp:Label></h4>
									</div>
								</div>
								<div class="col-md-2">
									<div class="demo-grid-duedate">
										<h4><asp:Label ID="dueouttext" runat="server"></asp:Label></h4>
									</div>
								</div>
								<div class="col-md-2">
									<div class="demo-grid-dirty">
										<h4><asp:Label ID="dirtytext" runat="server"></asp:Label></h4>
									</div>
								</div>
							</div>
						</div>
					</div>
			</div>
	</div>
	</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
