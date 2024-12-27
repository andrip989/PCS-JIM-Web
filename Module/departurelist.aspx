<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="departurelist.aspx.cs" Inherits="PCS_JIM_Web.Module.departurelist" %>
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Departure<i class="fa fa-angle-right"></i>List</li>
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
      <label for="arrivalto" class="col-sm-1 control-label hor-form">s/d</label>
    <div class="col-sm-4">
        <asp:TextBox ID="arrivalto" runat="server" CssClass="form-control" type="date"></asp:TextBox>
    </div>
  </div> 

<div class="form-group">
    <label for="arrival" class="col-sm-1 control-label hor-form">Departure</label>
    <div class="col-sm-4">
        <asp:TextBox ID="departure" runat="server" CssClass="form-control" type="date"></asp:TextBox>
    </div>
      <label for="departure" class="col-sm-1 control-label hor-form">s/d</label>
    <div class="col-sm-4">
        <asp:TextBox ID="departureto" runat="server" CssClass="form-control" type="date"></asp:TextBox>
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
                                        <asp:BoundField DataField="transaksiid" HeaderText="Transaksi Id" SortExpression="transaksiid" />
                                        <asp:BoundField DataField="noroom" HeaderText="No. Room" SortExpression="noroom" />
                                        <asp:BoundField DataField="arrival" HeaderText="Arrival" SortExpression="arrival" />
                                        <asp:BoundField DataField="departure" HeaderText="Departure" SortExpression="departure" />
                                        <asp:BoundField DataField="refbookingcode" HeaderText="Ref. Booking Code" SortExpression="refbookingcode" />
                                        <asp:BoundField DataField="custcode" HeaderText="Cust Code" SortExpression="custcode" />
                                        <asp:BoundField DataField="firstname" HeaderText="First Name" SortExpression="firstname" />
                                        <asp:BoundField DataField="tipetrans" HeaderText="Tipe Trans" SortExpression="tipetrans" />
                                        <asp:BoundField DataField="paymenttype" HeaderText="Payment Type" SortExpression="paymenttype" />
                                        <asp:BoundField DataField="guesttype" HeaderText="Guest Type" SortExpression="guesttype" />
                                        <asp:BoundField DataField="bookingsource" HeaderText="Booking Source" SortExpression="bookingsource" />

                                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                                        <asp:BoundField DataField="totalcharges" HeaderText="Total Charges" SortExpression="totalcharges" />
                                        <asp:BoundField DataField="deposit" HeaderText="Deposit" SortExpression="deposit" />
                                        <asp:BoundField DataField="createddatetime" HeaderText="Created Date" SortExpression="createddatetime" />
                                        <asp:BoundField DataField="createdby" HeaderText="Created By" SortExpression="createdby" />
                                    </Columns>
                                </asp:GridView>
						    </div>
					</div>				
                </div>
              

		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
