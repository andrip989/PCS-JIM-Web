<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="setuproomtype.aspx.cs" Inherits="PCS_JIM_Web.Module.setuproomtype" %>
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
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Setup<i class="fa fa-angle-right"></i> Room Types</li>
            </ol>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate>
    <div class="grid-form1">
<div class="form-horizontal"> 

   <div class="form-group">
    <label for="noroom" class="col-sm-2 control-label hor-form">Room Types</label>
    <div class="col-sm-4">
        <asp:TextBox ID="roomtypes" runat="server" CssClass="form-control" ></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup='valGroup3' runat="server" ControlToValidate="roomtypes" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
  </div>
   
  <div class="form-group">
    <label for="keterangan" class="col-sm-2 control-label hor-form">Keterangan</label>
    <div class="col-sm-4">
        <asp:TextBox ID="keterangan" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup='valGroup3' runat="server" ControlToValidate="keterangan" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
  </div>

    <hr />
  <div class="form-group">
      <label for="defaultrate" class="col-sm-2 control-label hor-form">Default Rate</label>
      <div class="col-sm-3">     
        <asp:TextBox ID="defaultrate" runat="server" CssClass="form-control" data-type="currency"></asp:TextBox> 
      </div>
  </div>

  <div class="form-group">
      <label for="defaultadultrate" class="col-sm-2 control-label hor-form">Adult Rate</label>
      <div class="col-sm-3">     
        <asp:TextBox ID="defaultadultrate" runat="server" CssClass="form-control" data-type="currency"></asp:TextBox> 
      </div>
      <label for="defaultchildrate" class="col-sm-2 control-label hor-form">Child Rate</label>
      <div class="col-sm-3">     
        <asp:TextBox ID="defaultchildrate" runat="server" CssClass="form-control" data-type="currency"></asp:TextBox>  
      </div>
  </div>
     <hr />
 <div class="form-group">
      <label for="roomrate" class="col-sm-2 control-label hor-form">Over Booking %</label>
      <div class="col-sm-3">     
        <asp:TextBox ID="overbooking" runat="server" CssClass="form-control" data-type="currency"></asp:TextBox> 
      </div>
  </div>

  <div class="form-group">
    <label for="baseadult" class="col-sm-2 control-label hor-form">Base adult</label>
    <div class="col-sm-3">    
      <asp:TextBox ID="baseadult" TextMode="Number" runat="server" CssClass="form-control"></asp:TextBox>
     </div>

    <label for="jumlahadult" class="col-sm-2 control-label hor-form">Max adult</label>
    <div class="col-sm-3">    
      <asp:TextBox ID="jumlahadult" TextMode="Number" runat="server" CssClass="form-control"></asp:TextBox>
     </div>
  </div>

  <div class="form-group">
      <label for="baseanak" class="col-sm-2 control-label hor-form">Base child</label>
      <div class="col-sm-3">    
      <asp:TextBox ID="baseanak" TextMode="Number" runat="server" CssClass="form-control"></asp:TextBox>
      </div>  

      <label for="jumlahanak" class="col-sm-2 control-label hor-form">Max child</label>
      <div class="col-sm-3">    
      <asp:TextBox ID="jumlahanak" TextMode="Number" runat="server" CssClass="form-control"></asp:TextBox>
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
                                <asp:GridView ID="GridView1" runat="server" OnPageIndexChanging="GridView1_PageIndexChanging" >
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

                                        <asp:BoundField DataField="roomtypes" HeaderText="Room Types" SortExpression="roomtypes" />
                                        <asp:BoundField DataField="keterangan" HeaderText="Keterangan" SortExpression="keterangan" />
                                        <asp:BoundField DataField="baseadult" HeaderText="Base Adult" SortExpression="baseadult" />
                                        <asp:BoundField DataField="adultmax" HeaderText="Max. Adult" SortExpression="adultmax" />
                                        <asp:BoundField DataField="basechild" HeaderText="Base Child" SortExpression="basechild" />
                                        <asp:BoundField DataField="childmax" HeaderText="Max. Child" SortExpression="childmax" />

                                        <asp:BoundField DataField="defaultrate" HeaderText="Rate" SortExpression="defaultrate" />
                                        <asp:BoundField DataField="defaultadultrate" HeaderText="Adult Rate" SortExpression="defaultadultrate" />
                                        <asp:BoundField DataField="defaultchildrate" HeaderText="Child Rate" SortExpression="defaultchildrate" />
                                        <asp:BoundField DataField="overbooking" HeaderText="Overbooking %" SortExpression="overbooking" />

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
