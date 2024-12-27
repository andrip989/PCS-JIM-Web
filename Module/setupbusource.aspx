<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="setupbusource.aspx.cs" Inherits="PCS_JIM_Web.Module.setupbusource" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Setup<i class="fa fa-angle-right"></i> Booking Source</li>
            </ol>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate> 
    <div class="grid-form1">
<div class="form-horizontal">

   <div class="form-group">
    <label for="alias" class="col-sm-2 control-label hor-form">Alias</label>
    <div class="col-sm-5">
        <asp:TextBox ID="alias" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup='valGroup3' runat="server" 
            ControlToValidate="alias" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
  </div>

  <div class="form-group">
    <label for="companyname" class="col-sm-2 control-label hor-form">Company Name</label>
    <div class="col-sm-5">
        <asp:TextBox ID="companyname" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup='valGroup3' runat="server" 
            ControlToValidate="companyname" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
  </div>

          <div class="form-group">
          <label for="firstname" class="col-sm-2 control-label hor-form">First Name</label>
          <div class="col-sm-2">
              <asp:TextBox ID="firstname" runat="server" CssClass="form-control"></asp:TextBox>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup='valGroup3' 
                runat="server" ControlToValidate="firstname" 
                BackColor="GreenYellow" 
                ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
          </div>
          <label for="lastname" class="col-sm-1 control-label hor-form">Last Name</label>
           <div class="col-sm-2">
              <asp:TextBox ID="lastname" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          </div>
         
   
      <div class="form-group">
        <label for="description" class="col-sm-2 control-label hor-form">Description</label>
        <div class="col-sm-5">
            <asp:TextBox ID="description" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
      </div>

         <div class="form-group">
              <label for="marketplace" class="col-sm-2 control-label hor-form">Market Place</label>
        <div class="col-sm-5">
              <asp:DropDownList ID="marketplace" runat="server" CssClass="form-control">
                   <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                    </asp:ListItem>
              </asp:DropDownList>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup='valGroup3' runat="server" 
            ControlToValidate="marketplace" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>

        </div>            
        </div>
    <hr />
    <div class="form-group">
    <div class="col-sm-1">
        &nbsp;
    </div>
     <asp:Button Text="Address Information" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
              OnClick="Tab1_Click" />
          <asp:Button Text="Contact Information" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
              OnClick="Tab2_Click" />
          <asp:Button Text="Credit Card Info" BorderStyle="None" ID="Tab3" CssClass="Initial" runat="server"
              OnClick="Tab3_Click" />
           <asp:Button Text="Other Information" BorderStyle="None" ID="Tab4" CssClass="Initial" runat="server"
              OnClick="Tab4_Click" />
           <asp:Button Text="Other Option" BorderStyle="None" ID="Tab5" CssClass="Initial" runat="server"
              OnClick="Tab5_Click" />
    
     </div>
     <asp:MultiView ID="MainView" runat="server">
     <asp:View ID="View1" runat="server">
         <br />
               <div class="form-group">
                <label for="address" class="col-sm-2 control-label hor-form">Address</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="address" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>

               <div class="form-group">
                <label for="address2" class="col-sm-2 control-label hor-form">Address 2</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="address2" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>

               <div class="form-group">
                <label for="city" class="col-sm-2 control-label hor-form">City</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="city" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>

               <div class="form-group">
                <label for="postalcode" class="col-sm-2 control-label hor-form">Postal Code</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="postalcode" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>

               <div class="form-group">
                <label for="state" class="col-sm-2 control-label hor-form">State</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="state" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>

                <div class="form-group">
                <label for="country" class="col-sm-2 control-label hor-form">Country</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="country" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>
   </asp:View>
   <asp:View ID="View2" runat="server">
       <br />
              <div class="form-group">
                <label for="phone" class="col-sm-2 control-label hor-form">Phone</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="phone" TextMode="Phone" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>
                
             <div class="form-group">
                <label for="fax" class="col-sm-2 control-label hor-form">Fax</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="fax" TextMode="Phone" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>

            <div class="form-group">
                <label for="email" class="col-sm-2 control-label hor-form">Email</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="email" TextMode="email" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>

            <div class="form-group">
                <label for="website" class="col-sm-2 control-label hor-form">Website</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="website" TextMode="Url" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>
    </asp:View>

   <asp:View ID="View3" runat="server">
       <br />
            <div class="form-group">
                <label for="creditcardtype" class="col-sm-2 control-label hor-form">Credit Card Type</label>
                <div class="col-sm-5">
                    <asp:DropDownList ID="creditcardtype" runat="server" CssClass="form-control">
                        <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                    </asp:ListItem>
                    </asp:DropDownList>
                </div>
              </div>

            <div class="form-group">
                <label for="cardholder" class="col-sm-2 control-label hor-form">Card Holder</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="cardholder" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>

            <div class="form-group">
                <label for="creditcardno" class="col-sm-2 control-label hor-form">Card No.</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="creditcardno" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>

            <div class="form-group">
                <label for="ccexpdate" class="col-sm-2 control-label hor-form">Exp. Date</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="ccexpdate" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>
   </asp:View>       
   <asp:View ID="View4" runat="server">
       <br />
        <div class="form-group">
                <label for="iatano" class="col-sm-2 control-label hor-form">IATA No.</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="iatano" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>
        <div class="form-group">
                <label for="regno" class="col-sm-2 control-label hor-form">Reg No.</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="regno" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>
        <div class="form-group">
                <label for="regno1" class="col-sm-2 control-label hor-form">Reg No. 1</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="regno1" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>
        <div class="form-group">
                <label for="regno2" class="col-sm-2 control-label hor-form">Reg No. 2</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="regno2" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>
       <div class="form-group">
                <label for="acnumber" class="col-sm-2 control-label hor-form">Ac. Number</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="acnumber" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
              </div>
    </asp:View>
    <asp:View ID="View5" runat="server">
        <br />
            <div class="form-group">
                <label for="definespecialseason" class="col-sm-2 control-label hor-form">Define Special Season</label>
                <div class="col-sm-6">
                    <asp:CheckBox ID="definespecialseason" runat="server" />
                </div>
              </div>
            <div class="form-group">
                <label for="definespecialrate" class="col-sm-2 control-label hor-form">Define Special Rate</label>
                <div class="col-sm-6">
                    <asp:CheckBox ID="definespecialrate" runat="server" />
                </div>
              </div>
            <div class="form-group">
                <label for="plantype" class="col-sm-2 control-label hor-form">Plan</label>
                <div class="col-sm-5">
                    <asp:DropDownList ID="plantype" runat="server" CssClass="form-control">
                        <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                    </asp:ListItem>
                    </asp:DropDownList>
                </div>
              </div>
          <div class="form-group">
          <label for="valueplan" class="col-sm-2 control-label hor-form">Value %</label>
          <div class="col-sm-2">
              <asp:TextBox ID="valueplan" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
           <label for="term" class="col-sm-1 control-label hor-form">Term</label>

           <div class="col-sm-2">
              <asp:TextBox ID="term" TextMode="Number" runat="server" CssClass="form-control" ></asp:TextBox>
          </div>
          </div>
        
    </asp:View>
    </asp:MultiView>
   <asp:HiddenField ID="recidparam" runat="server" />

    <hr />

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
                                        <asp:BoundField DataField="alias" HeaderText="Alias" SortExpression="alias" />
                                        <asp:BoundField DataField="companyname" HeaderText="Company Name" SortExpression="companyname" />
                                        <asp:BoundField DataField="firstname" HeaderText="First Name" SortExpression="firstname" />
                                        <asp:BoundField DataField="lastname" HeaderText="Last Name" SortExpression="lastname" />
                                        <asp:BoundField DataField="marketplace" HeaderText="Market Place" SortExpression="marketplace" />
                                        <asp:templatefield headertext="Special. Season">
                                            <itemtemplate>
	                                            <asp:checkbox id="chkdefinespecialseason" runat="server" Checked='<%# Convert.ToBoolean(Eval("definespecialseason")) %>' Enabled ="false" />
                                            </itemtemplate>
                                        </asp:templatefield>
                                        
                                        <asp:templatefield headertext="Special. Rate">
                                            <itemtemplate>
	                                            <asp:checkbox id="chkdefinespecialrate" runat="server" Checked='<%# Convert.ToBoolean(Eval("definespecialrate")) %>' Enabled ="false" />
                                            </itemtemplate>
                                        </asp:templatefield>
                                        <asp:BoundField DataField="plantype" HeaderText="Plan" SortExpression="plantype" />
                                        <asp:BoundField DataField="valueplan" HeaderText="Value %" SortExpression="valueplan" />
                                        <asp:BoundField DataField="term" HeaderText="Term" SortExpression="term" />
                                        <asp:BoundField DataField="createddate" HeaderText="Created Date" SortExpression="createddate" />
                                        <asp:BoundField DataField="createdby" HeaderText="Created By" SortExpression="createdby" />
                            </Columns>
                        </asp:GridView>
					</div>
			</div>				
        </div>

		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
