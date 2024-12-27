<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="synchtoax.aspx.cs" Inherits="PCS_JIM_Web.Tools.synchtoax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">

        function validationExport() {
            var x = window.confirm("Are you sure want to export to axapta ?")
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
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Tools<i class="fa fa-angle-right"></i>Export to Axapta</li>
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

    <label for="transaksiid" class="col-sm-1 control-label hor-form">Transaksi Id</label>
    <div class="col-sm-4">
        <asp:TextBox ID="transaksiid" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    </div>

    <div class="form-group">
    <label for="custcode" class="col-sm-1 control-label hor-form">Cust. Code</label>
    <div class="col-sm-4">
        <asp:TextBox ID="custcode" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <label for="ktpid" class="col-sm-1 control-label hor-form">Identification Id</label>
    <div class="col-sm-4">
        <asp:TextBox ID="ktpid" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
  </div>   
    
  <div class="form-group">
    <label for="firstname" class="col-sm-1 control-label hor-form">First Name</label>
    <div class="col-sm-4">
        <asp:TextBox ID="firstname" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
      <label for="lastname" class="col-sm-1 control-label hor-form">Last Name</label>
    <div class="col-sm-4">
        <asp:TextBox ID="lastname" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
  </div> 
    
  <div class="form-group">
    <label for="businesscode" class="col-sm-1 control-label hor-form">Business Code.</label>
    <div class="col-sm-4">
        <asp:DropDownList ID="businesscode" runat="server" CssClass="form-control">
                <asp:ListItem Text="All" Enabled="true" Selected="True" Value=''>
                </asp:ListItem>
        </asp:DropDownList>
    </div>
  </div>   
    
<div class="form-group">
    <label for="arrival" class="col-sm-1 control-label hor-form">Arrival</label>
    <div class="col-sm-4">
        <asp:TextBox ID="arrival" runat="server" CssClass="form-control" type="date"></asp:TextBox>
    </div>
      <label for="departure" class="col-sm-1 control-label hor-form">Departure</label>
    <div class="col-sm-4">
        <asp:TextBox ID="departure" runat="server" CssClass="form-control" type="date"></asp:TextBox>
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
     <label for="exportstatus" class="col-sm-1 control-label hor-form">Export Status</label>
    <div class="col-sm-4">
        <asp:DropDownList ID="exportstatus" runat="server" CssClass="form-control">
                <asp:ListItem Text="All" Enabled="true" Selected="True" Value=''>
                </asp:ListItem>
        </asp:DropDownList>
    </div>
  </div>  

  <div class="form-group">
    <div class="col-sm-offset-1 col-sm-10">
        <asp:Button ID="submit" runat="server" Text="Search" CssClass="btn-system btn" OnClick="submit_Click"  />
         <asp:Button ID="btnexport" runat="server" Text="Export" CssClass="btn-dark btn" OnClick="btnexport_Click" OnClientClick="if( validationExport() ) { return true; } return false;"  />
    </div>
  </div>


</div>
</div>               

                <div class="agile-grids">	
					<div class="agile-tables">
					        <div class="w3l-table-info">
                                <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging" >
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox id="chkheader" runat="server" AutoPostBack="true" OnCheckedChanged="chkheader_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox id="chk" runat="server" AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                                <asp:HiddenField ID="recchk" runat="server" Value='<%# Eval("recid") %>'  />
                                                <asp:HiddenField ID="paramexport" runat="server" Value='<%# Eval("exportstatus") %>'  />
                                                <asp:HiddenField ID="paramstatus" runat="server" Value='<%# Eval("status") %>'  />
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:BoundField DataField="transaksiid" HeaderText="Transaksi Id" SortExpression="transaksiid" />
                                        <asp:BoundField DataField="noroom" HeaderText="No. Room" SortExpression="noroom" />
                                        <asp:BoundField DataField="arrival" HeaderText="Check - in" SortExpression="arrival" />
                                        <asp:BoundField DataField="departure" HeaderText="Check - out" SortExpression="departure" />
                                        <asp:BoundField DataField="refbookingcode" HeaderText="Ref. Booking Code" SortExpression="refbookingcode" />
                                        <asp:BoundField DataField="custcode" HeaderText="Cust Code" SortExpression="custcode" />
                                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                                        <asp:BoundField DataField="exportstatus" HeaderText="Export Status" SortExpression="exportstatus" />
                                        <asp:BoundField DataField="journalidax" HeaderText="No. Journal Ax" SortExpression="journalidax" />
                                    </Columns>
                                </asp:GridView>
						    </div>
					</div>				
                </div>
              

		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
