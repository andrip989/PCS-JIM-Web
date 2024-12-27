<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="setupseasontype.aspx.cs" Inherits="PCS_JIM_Web.Module.setupseasontype" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">

        function validationDelete() {
            var x = window.confirm("Are you sure want to delete ?")
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
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Setup<i class="fa fa-angle-right"></i> Season Type</li>
            </ol>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate>
    <div class="grid-form1">
<div class="form-horizontal"> 

   <div class="form-group">
    <label for="seasontype" class="col-sm-2 control-label hor-form">Season Type</label>
    <div class="col-sm-4">
        <asp:TextBox ID="seasontype" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
  </div>
   
  <div class="form-group">
    <label for="description" class="col-sm-2 control-label hor-form">Description</label>
    <div class="col-sm-4">
        <asp:TextBox ID="description" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
  </div>
   

    <div class="form-group">
    <label for="fromday" class="col-sm-2 control-label hor-form">From Day</label>
    <div class="col-sm-2">
        <asp:TextBox ID="fromday" TextMode="Number" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <label for="frommonth" class="col-sm-1 control-label hor-form">From Month</label>
    <div class="col-sm-2">
                    <asp:DropDownList ID="frommonth" runat="server" CssClass="form-control">
                        <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                    </asp:ListItem>
                    </asp:DropDownList>
    </div>
    </div>

    <div class="form-group">
    <label for="today" class="col-sm-2 control-label hor-form">To Day</label>
    <div class="col-sm-2">
        <asp:TextBox ID="today" TextMode="Number" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <label for="tomonth" class="col-sm-1 control-label hor-form">To Month</label>
    <div class="col-sm-2">
            <asp:DropDownList ID="tomonth" runat="server" CssClass="form-control">
                <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
            </asp:ListItem>
            </asp:DropDownList>
    </div>
    </div>

    <div class="form-group">
    <label for="yearperiod" class="col-sm-2 control-label hor-form">Year</label>
    <div class="col-sm-4">
        <asp:TextBox ID="yearperiod" TextMode="Number" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    </div>

    <div class="form-group">
    <label for="active" class="col-sm-2 control-label hor-form">Active</label>
    <div class="col-sm-4">
        <asp:CheckBox ID="active" runat="server" />
    </div>
   </div>

    <asp:HiddenField ID="recidparam" runat="server" />


  <div class="form-group">
    <div class="col-sm-offset-2 col-sm-10">
      <asp:Button ID="submit" ValidationGroup='valGroup3' runat="server" Text="Submit" CssClass="btn-primary btn" OnClick="SaveClick"  />
        <button type="reset" class="btn-default btn">Reset</button>
      <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn-warning btn" OnClick="btncancel_Click"  />
       <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="btn-dark btn" OnClick="btndelete_Click" OnClientClick="if( validationDelete() ) { return true; } return false;"  />
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
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:BoundField DataField="seasontype" HeaderText="Season Type" SortExpression="seasontype" />
                                        <asp:BoundField DataField="description" HeaderText="Description" SortExpression="description" />
                                        <asp:BoundField DataField="fromday" HeaderText="From Day" SortExpression="fromday" />
                                        <asp:BoundField DataField="frommonth" HeaderText="From Month" SortExpression="frommonth" />
                                        <asp:BoundField DataField="today" HeaderText="To Day" SortExpression="today" />
                                        <asp:BoundField DataField="tomonth" HeaderText="To Month" SortExpression="tomonth" />
                                        <asp:BoundField DataField="yearperiod" HeaderText="Year" SortExpression="yearperiod" />
                                         <asp:templatefield headertext="Active">
                                            <itemtemplate>
	                                            <asp:checkbox id="chkactive" runat="server" Checked='<%# Convert.ToBoolean(Eval("active")) %>' Enabled ="false" />
                                            </itemtemplate>
                                        </asp:templatefield>
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
