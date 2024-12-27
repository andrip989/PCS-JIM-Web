<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="hotspotwifi.aspx.cs" Inherits="PCS_JIM_Web.Module.hotspotwifi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">

        function clickoutletevent(event, param) {
            __doPostBack(event, param);
        }

    </script>

	<script type="text/javascript" language="javascript">

        var timer = setInterval(checkChild, 500);

        function checkChild() {
            if (newWindow.closed) {
                //alert("Child window closed");
                clearInterval(timer);
                //window.location.reload();
                clickoutletevent('', 'refreshdata');
            }
        }

        function validationhotspot(vroom,paramtipe) {
            var x = window.confirm("Are you sure want to " + paramtipe +" hotspot " + vroom + " ?")
            if (x) {
                return true;
            } else {
                return false;
            }
        }  

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Tools<i class="fa fa-angle-right"></i> Hotspot Wifi</li>
            </ol>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate>
    <div class="grid-form1">
<div class="form-horizontal">  

   <div class="form-group">
    <label for="noroom" class="col-sm-1 control-label hor-form">No. Room</label>
    <div class="col-sm-4">
        <asp:DropDownList ID="noroom" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <label for="roomtypes" class="col-sm-1 control-label hor-form">Room Type</label>
    <div class="col-sm-4">
        <asp:DropDownList ID="roomtypes" runat="server" CssClass="form-control">
            <asp:ListItem Text="All" Enabled="true" Selected="True" Value=''>
                </asp:ListItem>
        </asp:DropDownList>
    </div>
  </div>   

   <div class="form-group">
    <label for="floortype" class="col-sm-1 control-label hor-form">Floor Types</label>
    
    <div class="col-sm-4">
            <asp:DropDownList ID="floortype" runat="server" CssClass="form-control">
                <asp:ListItem Text="All" Enabled="true" Selected="True" Value=''>
                </asp:ListItem>
        </asp:DropDownList>
    </div>
    </div>

  
<div class="form-group">
    <label for="status" class="col-sm-1 control-label hor-form">Status</label>
    <div class="col-sm-4">
        <asp:DropDownList ID="status" runat="server" CssClass="form-control">
                <asp:ListItem Text="All" Enabled="true" Selected="True" Value=''>
                </asp:ListItem>
        </asp:DropDownList>
    </div>
    
  </div>  

  <div class="form-group">
    <div class="col-sm-offset-1 col-sm-10">
        <asp:Button ID="submit" runat="server" Text="Search" CssClass="btn-system btn" OnClick="submit_Click"  />
    </div>
  </div>


</div>
</div>               

                <div class="agile-grids">	
					<div class="agile-tables">
					        <div class="w3l-table-info">
                                <asp:GridView ID="GridView1" runat="server" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging" >
                                    <Columns>
                                        <asp:BoundField DataField="noroom" HeaderText="No. Room" SortExpression="noroom" />
                                        <asp:BoundField DataField="ssid" HeaderText="SSID" SortExpression="ssid" />
                                        <asp:BoundField DataField="username" HeaderText="Username" SortExpression="username" />
                                        <asp:BoundField DataField="password" HeaderText="Password" SortExpression="password" />
                                        
                                        <asp:BoundField DataField="checkin" HeaderText="Check in" SortExpression="checkin" />
                                        <asp:BoundField DataField="checkout" HeaderText="Check out" SortExpression="checkout" />
                                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                                        <asp:BoundField DataField="reftransid" HeaderText="Ref. Id" SortExpression="reftransid" />
                                        
                                        <asp:TemplateField>
                                            <ItemTemplate>                                                
                                                <asp:ImageButton id="btnchk" runat="server" CommandName="edit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/images/tick1.png"  />
                                                <asp:HiddenField ID="recchk" runat="server" Value='<%# Eval("recid") %>'  />
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
						    </div>
					</div>				
                </div>
              

		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
