<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="cleanroomlist.aspx.cs" Inherits="PCS_JIM_Web.Module.cleanroomlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>House Keeping<i class="fa fa-angle-right"></i>Clean Room List</li>
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
                                <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging" >
                                    <Columns>
                                        <asp:BoundField DataField="transid" HeaderText="Transaksi Id" SortExpression="transid" />
                                        <asp:BoundField DataField="noroom" HeaderText="No. Room" SortExpression="noroom" />
                                        <asp:BoundField DataField="arrival" HeaderText="Start Time" SortExpression="arrival" />
                                        <asp:BoundField DataField="departure" HeaderText="End Time" SortExpression="departure" />
                                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                                        <asp:BoundField DataField="createddate" HeaderText="Created Date" SortExpression="createddate" />
                                        <asp:BoundField DataField="createdby" HeaderText="Created By" SortExpression="createdby" />
                                        <asp:BoundField DataField="updateddate" HeaderText="Updated Date" SortExpression="updateddate" />
                                        <asp:BoundField DataField="updatedby" HeaderText="Updated By" SortExpression="updatedby" />
                                    </Columns>
                                </asp:GridView>
						    </div>
					</div>				
                </div>
              

		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
