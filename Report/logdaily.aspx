<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="logdaily.aspx.cs" Inherits="PCS_JIM_Web.Report.logdaily" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Report<i class="fa fa-angle-right"></i> Log Daily</li>
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
  </div>   

    <div class="form-group">
    <label for="datetimestart" class="col-sm-1 control-label hor-form">Log Date</label>
    <div class="col-sm-10">
        <asp:TextBox ID="datetimestart" type="datetime-local" runat="server"></asp:TextBox>
        &nbsp;<asp:TextBox ID="datetimeend" runat="server" type="datetime-local"></asp:TextBox>
    </div>
  </div>   

      <div class="form-group">
    <label for="injectapi" class="col-sm-1 control-label hor-form">Get from SonSoff</label>
    <div class="col-sm-10">
        <asp:CheckBox ID="injectapi" runat="server" />
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
                        <h4 class="title">Summary</h4>
					        <div class="w3l-table-info">
                                <asp:GridView ID="GridView2" runat="server" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" >
                                    <Columns>
                                    </Columns>
                                </asp:GridView>
						    </div>
					</div>				
                </div>

                <div class="agile-grids">	
					<div class="agile-tables">
                        <h4 class="title">Detail</h4>
					        <div class="w3l-table-info">
                                <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging" >
                                    <Columns>
                                         <asp:BoundField DataField="noroom" HeaderText="No. Room" SortExpression="noroom" />
                                         <asp:BoundField DataField="description" HeaderText="Room Description" SortExpression="description" />
                                         <asp:BoundField DataField="datetime" HeaderText="Log Date" SortExpression="datetime" />
                                         <asp:BoundField DataField="current" HeaderText="Current (A)" SortExpression="current" />
                                         <asp:BoundField DataField="voltage" HeaderText="Voltage (V)" SortExpression="voltage" />
                                         <asp:BoundField DataField="realpower" HeaderText="Real Power (W)" SortExpression="realpower" />
                                         <asp:BoundField DataField="reactivepower" HeaderText="Reactive Power(W)" SortExpression="reactivepower" />
                                         <asp:BoundField DataField="apparentpower" HeaderText="Apparent Power(W)" SortExpression="apparentpower" />
                                         <asp:BoundField DataField="createdby" HeaderText="Created By" SortExpression="createdby" />

                                    </Columns>
                                    
                                </asp:GridView>
						    </div>
					</div>				
                </div>
               
                 

		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
