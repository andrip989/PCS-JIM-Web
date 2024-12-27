﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="setuproomamenities.aspx.cs" Inherits="PCS_JIM_Web.Module.setuproomamenities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Setup<i class="fa fa-angle-right"></i> Room Amenities</li>
            </ol>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate> 
    <div class="grid-form1">
<div class="form-horizontal">

   <div class="form-group">
    <label for="roomamenities" class="col-sm-2 control-label hor-form">Name</label>
    <div class="col-sm-4">
        <asp:TextBox ID="roomamenities" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup='valGroup3' runat="server" ControlToValidate="roomamenities" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
  </div>
   
  <div class="form-group">
    <label for="description" class="col-sm-2 control-label hor-form">Description</label>
    <div class="col-sm-4">
        <asp:TextBox ID="description" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
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
                                        <asp:BoundField DataField="roomamenities" HeaderText="Room Amenities" SortExpression="roomamenities" />
                                        <asp:BoundField DataField="description" HeaderText="Description" SortExpression="description" />
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