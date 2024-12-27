<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="setuptax.aspx.cs" Inherits="PCS_JIM_Web.Module.setuptax" %>
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
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Setup<i class="fa fa-angle-right"></i> Tax</li>
            </ol>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate>
    <div class="grid-form1">
<div class="form-horizontal"> 

   <div class="form-group">
    <label for="startdate" class="col-sm-2 control-label hor-form">Start Date</label>
    <div class="col-sm-4">
        <asp:TextBox ID="startdate" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup='valGroup3' runat="server" ControlToValidate="startdate" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
  </div>
   
    <div class="form-group">
    <label for="fromamount" class="col-sm-2 control-label hor-form">From Amount</label>
    <div class="col-sm-2">
        <asp:TextBox ID="fromamount" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <label for="toamount" class="col-sm-1 control-label hor-form">s/d</label>
    <div class="col-sm-2">
        <asp:TextBox ID="toamount" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    </div>

    <div class="form-group">
    <label for="tax1" class="col-sm-2 control-label hor-form">Service Charge %</label>
    <div class="col-sm-2">
        <asp:TextBox ID="tax1" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    </div>

    <div class="form-group">
    <label for="tax2" class="col-sm-2 control-label hor-form">PB 1 %</label>
    <div class="col-sm-2">
        <asp:TextBox ID="tax2" runat="server" CssClass="form-control"></asp:TextBox>
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

                                        <asp:BoundField DataField="startdate" HeaderText="Start Date" SortExpression="startdate" />
                                        <asp:BoundField DataField="fromamount" HeaderText="From Amount" SortExpression="fromamount" />
                                        <asp:BoundField DataField="toamount" HeaderText="To Amount" SortExpression="toamount" />
                                        <asp:BoundField DataField="tax1" HeaderText="Service Charges %" SortExpression="tax1" />
                                        <asp:BoundField DataField="tax2" HeaderText="PB 1%" SortExpression="tax2" />
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
