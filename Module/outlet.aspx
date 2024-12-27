<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="outlet.aspx.cs" Inherits="PCS_JIM_Web.Module.outlet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Setup<i class="fa fa-angle-right"></i> Outlet</li>
            </ol>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate>
    <div class="grid-form1">
<div class="form-horizontal">
   <div class="form-group">
    <label for="deviceid" class="col-sm-2 control-label hor-form">Device Id</label>
    <div class="col-sm-10">
        <asp:DropDownList ID="deviceid" runat="server" AutoPostBack="true" 
            OnSelectedIndexChanged="deviceid_SelectedIndexChanged" 
            CssClass="form-control">
            <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
            </asp:ListItem>

        </asp:DropDownList>
    </div>
  </div>

  <div class="form-group">
    <label for="deviceid" class="col-sm-2 control-label hor-form">Sub Device Id</label>
    <div class="col-sm-10">
        <asp:DropDownList ID="subdeviceid" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="subdeviceid_SelectedIndexChanged">
            <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
            </asp:ListItem>
        </asp:DropDownList>
    </div>
  </div>  

   <div class="form-group">
    <label for="deviceid" class="col-sm-2 control-label hor-form">Outlet No</label>
    <div class="col-sm-10">
        <asp:TextBox ID="inputoutlet" TextMode="Number" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ID="outletnotext" ValidationGroup='valGroup1' runat="server" ControlToValidate="inputoutlet" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
  </div>
   
  <div class="form-group">
    <label for="description" class="col-sm-2 control-label hor-form">Description</label>
    <div class="col-sm-10">
        <asp:TextBox ID="description" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
  </div>

    <asp:HiddenField ID="recidparam" runat="server" />


  <div class="form-group">
    <div class="col-sm-offset-2 col-sm-10">
      <asp:Button ID="submit" ValidationGroup='valGroup1' runat="server" Text="Submit" CssClass="btn-primary btn" OnClick="SaveClick"  />
        <button type="reset" class="btn-default btn">Reset</button>
      <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn-warning btn" OnClick="btncancel_Click"  />
    </div>
  </div>



</div>
</div>


                <div class="agile-grids">	
					<div class="agile-tables">
					        <div class="w3l-table-info">
                                <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" >
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox id="chk" runat="server" AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
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
