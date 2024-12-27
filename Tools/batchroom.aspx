<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="batchroom.aspx.cs" Inherits="PCS_JIM_Web.Tools.batchroom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Tools<i class="fa fa-angle-right"></i> Batch Room</li>
            </ol>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate>
    <div class="grid-form1">
<div class="form-horizontal"> 

   <div class="form-group">
    <label for="noroom" class="col-sm-2 control-label hor-form">No. Room</label>
    <div class="col-sm-4">
        <asp:DropDownList ID="noroom" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
  </div>   


  <div class="form-group">
    <div class="col-sm-offset-2 col-sm-4">
      <asp:Button ID="submit" runat="server" Text="Process" CssClass="btn btn-hover btn-dark btn-block btn" OnClick="submit_Click"  />
    </div>
  </div>


</div>
</div>


                <div class="agile-grids">	
					<div class="agile-tables">
                    <h4 class="title">Reservation - List</h4>
					        <div class="w3l-table-info">
                                <asp:GridView ID="GridView1" runat="server" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" >
                                    <Columns>
                                        <asp:BoundField DataField="noroom" HeaderText="No. Room" SortExpression="noroom" />
                                        <asp:BoundField DataField="arrival" HeaderText="Arrival" SortExpression="arrival" />
                                        <asp:BoundField DataField="departure" HeaderText="Departure" SortExpression="departure" />
                                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                                        <asp:BoundField DataField="actualcheckin" HeaderText="Actual Checkin" SortExpression="actualcheckin" />
                                        <asp:BoundField DataField="actualcheckout" HeaderText="Actual Checkout" SortExpression="actualcheckout" />
                                    </Columns>
                                </asp:GridView>
						    </div>
					</div>				
                </div>


                 <div class="agile-grids">	
					<div class="agile-tables">
                     <h4 class="title">House Keeping - List</h4>
					        <div class="w3l-table-info">
                                <asp:GridView ID="GridView2" runat="server" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDataBound="GridView2_RowDataBound" >
                                    <Columns>
                                        <asp:BoundField DataField="noroom" HeaderText="No. Room" SortExpression="noroom" />
                                        <asp:BoundField DataField="arrival" HeaderText="Arrival" SortExpression="arrival" />
                                        <asp:BoundField DataField="departure" HeaderText="Departure" SortExpression="departure" />
                                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                                        <asp:BoundField DataField="createddate" HeaderText="Create Date" SortExpression="createddate" />
                                        <asp:BoundField DataField="createdby" HeaderText="Create By" SortExpression="createdby" />
                                    </Columns>
                                </asp:GridView>
						    </div>
					</div>				
                </div>

                <div class="agile-grids">	
					<div class="agile-tables">
                     <h4 class="title">Maintenance - List</h4>
					        <div class="w3l-table-info">
                                <asp:GridView ID="GridView3" runat="server" OnPageIndexChanging="GridView3_PageIndexChanging" OnRowDataBound="GridView3_RowDataBound" >
                                    <Columns>
                                        <asp:BoundField DataField="noroom" HeaderText="No. Room" SortExpression="noroom" />
                                        <asp:BoundField DataField="arrival" HeaderText="Arrival" SortExpression="arrival" />
                                        <asp:BoundField DataField="departure" HeaderText="Departure" SortExpression="departure" />
                                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                                        <asp:BoundField DataField="createddate" HeaderText="Create Date" SortExpression="createddate" />
                                        <asp:BoundField DataField="createdby" HeaderText="Create By" SortExpression="createdby" />
                                    </Columns>
                                </asp:GridView>
						    </div>
					</div>				
                </div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
