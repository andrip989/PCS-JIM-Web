<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="setuproom.aspx.cs" Inherits="PCS_JIM_Web.Module.setuproom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        function roommodified(val) {
            //alert("The input value has changed. The new value is: " + val);

            document.getElementById('<%=sequencetrans.ClientID%>').value = val+"-YY-######";
        }        
    </script>   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Setup<i class="fa fa-angle-right"></i> Room Operation</li>
            </ol>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate> 
    <div class="grid-form1">
<div class="form-horizontal">
    
          <asp:Button Text="Room Information" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
              OnClick="Tab1_Click" />
          <asp:Button Text="Other Information" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
              OnClick="Tab2_Click" />
          <asp:Button Text="Setup Room" BorderStyle="None" ID="Tab3" CssClass="Initial" runat="server"
              OnClick="Tab3_Click" />
          <asp:MultiView ID="MainView" runat="server">
           <asp:View ID="View1" runat="server">
         <br />
         <br />
         <div class="form-group">
            <label for="noroom" class="col-sm-2 control-label hor-form">No. Room</label>
            <div class="col-sm-6">
                <asp:TextBox ID="noroom" runat="server" CssClass="form-control" onchange="roommodified(this.value)"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup='valGroup3' runat="server" ControlToValidate="noroom" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
          </div>
   
          <div class="form-group">
            <label for="description" class="col-sm-2 control-label hor-form">Description</label>
            <div class="col-sm-6">
                <asp:TextBox ID="description" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          </div>

         <div class="form-group">
            <label for="roomtypes" class="col-sm-2 control-label hor-form">Room Types</label>
    
            <div class="col-sm-6">
                 <asp:DropDownList ID="roomtypes" runat="server" CssClass="form-control">
                        <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                        </asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ValidationGroup='valGroup3' ID="RequiredFieldValidator4" runat="server" ControlToValidate="roomtypes" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
          </div>

        <div class="form-group">
            <label for="floortype" class="col-sm-2 control-label hor-form">Floor Types</label>
    
            <div class="col-sm-6">
                 <asp:DropDownList ID="floortype" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
          </div>

          <div class="form-group">
            <label for="phoneext" class="col-sm-2 control-label hor-form">Phone Extension</label>    
            <div class="col-sm-6">
                <asp:TextBox ID="phoneext" TextMode="Phone" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label for="dataext" class="col-sm-2 control-label hor-form">Data Extension</label>    
            <div class="col-sm-6">
                <asp:TextBox ID="dataext" TextMode="Phone" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label for="keycard" class="col-sm-2 control-label hor-form">Keycard Alias</label>    
            <div class="col-sm-6">
                <asp:TextBox ID="keycard" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          <div class="form-group">
            <label for="powercode" class="col-sm-2 control-label hor-form">Power Code</label>    
            <div class="col-sm-6">
                <asp:TextBox ID="powercode" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
          </asp:View>
          <asp:View ID="View2" runat="server">
             <br />
             <br />
            <div class="form-group">
                    <label for="active" class="col-sm-2 control-label hor-form">Lock Price</label>
                    <div class="col-sm-6">
                        <asp:CheckBox ID="lockprice" runat="server" />
                    </div>
             </div>
              <div class="form-group">
                  <label for="roomrate" class="col-sm-2 control-label hor-form">Fixed Rate</label>
                  <div class="col-sm-6">     
                    <asp:TextBox ID="roomrate" runat="server" CssClass="form-control" data-type="currency"></asp:TextBox>  
                  </div>
                  <div class="col-sm-2">
                        <asp:CheckBox ID="fixedrate" runat="server" />
                    </div>
              </div>
              <div class="form-group">
                <label for="allowsmoking" class="col-sm-2 control-label hor-form">Smoking Room</label>
                <div class="col-sm-6">
                    <asp:CheckBox ID="allowsmoking" runat="server" />
                </div>
              </div>
                <div class="form-group">
                <label for="roomamenities" class="col-sm-2 control-label hor-form">Room Amenities</label>
                <div class="col-sm-3">          
                    <asp:CheckBoxList ID="roomamenities" runat="server" RepeatDirection="Vertical" Datafield="description" DataValueField="roomamenities">
                    </asp:CheckBoxList>                
                </div>

                <label for="housekeepingday" class="col-sm-2 control-label hor-form">House Keeping</label>
                <div class="col-sm-3">
                    <asp:CheckBoxList ID="housekeepingday" runat="server" RepeatDirection="Vertical">
                    <asp:ListItem id="Sunday" Value="Sunday"></asp:ListItem>
                    <asp:ListItem id="Monday" Value="Monday"></asp:ListItem>
                    <asp:ListItem id="Tuesday" Value="Tuesday"></asp:ListItem>
                    <asp:ListItem id="Wednesday" Value="Wednesday"></asp:ListItem>
                    <asp:ListItem id="Thursday" Value="Thursday"></asp:ListItem>
                    <asp:ListItem id="Friday" Value="Friday"></asp:ListItem>
                    <asp:ListItem id="Saturday" Value="Saturday"></asp:ListItem>
                        </asp:CheckBoxList>
                </div>                         
              </div>

             <br />
             <div class="form-group">
                <label for="statushousekeeping" class="col-sm-2 control-label hor-form">Status Housekeeping</label>
                <div class="col-sm-6">
                    <asp:CheckBox ID="statushousekeeping" runat="server" Enabled="false"/>
                </div>
              </div>
              <div class="form-group">
                <label for="statusmaintenance" class="col-sm-2 control-label hor-form">Status Maintenance</label>
                <div class="col-sm-6">
                    <asp:CheckBox ID="statusmaintenance" runat="server"/>
                </div>
              </div>
              <div class="form-group">
                <label for="statusroom" class="col-sm-2 control-label hor-form">Status Room (On/Off)</label>
                <div class="col-sm-6">
                    <asp:CheckBox ID="statusroom" runat="server" Enabled="false" />
                </div>
              </div>
        </asp:View>
        <asp:View ID="View3" runat="server">
        <br />
        <br />
              <div class="form-group">
                <label for="listoutletno" class="col-sm-2 control-label hor-form">Outlet No</label>
                <div class="col-sm-6">
                    <asp:DropDownList ID="listoutletno" runat="server" CssClass="form-control" OnSelectedIndexChanged="listoutletno_SelectedIndexChanged" OnDataBinding="listoutletno_DataBinding" OnDataBound="listoutletno_DataBound">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ValidationGroup='valGroup3' ID="RequiredFieldValidator1" runat="server" ControlToValidate="listoutletno" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
              </div>

               <div class="form-group">
                <label for="nsequencetransoroom" class="col-sm-2 control-label hor-form">No. Sequence Transaksi</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="sequencetrans" runat="server" CssClass="form-control" ToolTip="NoOutlet-YY-######"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup='valGroup3' runat="server" ControlToValidate="sequencetrans" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
              </div>

              <div class="form-group">
                <label for="active" class="col-sm-2 control-label hor-form">Active</label>
                <div class="col-sm-6">
                    <asp:CheckBox ID="active" runat="server" />
                </div>
              </div>
           </asp:View>
  
        </asp:MultiView>
  

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

                                        <asp:BoundField DataField="noroom" HeaderText="No. Room" SortExpression="noroom" />
                                        <asp:BoundField DataField="description" HeaderText="Description" SortExpression="description" />
                                        <asp:BoundField DataField="roomtypes" HeaderText="Room Types" SortExpression="roomtypes" />
                                        <asp:BoundField DataField="floortype" HeaderText="Floor Types" SortExpression="floortype" />
                                        <asp:BoundField DataField="outletno" HeaderText="No. Outlet" SortExpression="outletno" />
                                        <asp:templatefield headertext="House Keeping">
                                            <itemtemplate>
	                                            <asp:checkbox id="chkstatushousekeeping" runat="server" Checked='<%# Convert.ToBoolean(Eval("statushousekeeping")) %>' Enabled ="false" />
                                            </itemtemplate>
                                        </asp:templatefield>
                                        
                                        <asp:templatefield headertext="Maintenance">
                                            <itemtemplate>
	                                            <asp:checkbox id="chkmaintenance" runat="server" Checked='<%# Convert.ToBoolean(Eval("statusmaintenance")) %>' Enabled ="false" />
                                            </itemtemplate>
                                        </asp:templatefield>

                                        <asp:BoundField DataField="statusroom" HeaderText="Status Room" SortExpression="statusroom" />
                                        
                                        <asp:BoundField DataField="createddatetime" HeaderText="Created Date" SortExpression="createddatetime" />
                                        <asp:BoundField DataField="createdby" HeaderText="Created By" SortExpression="createdby" />
                                        <asp:BoundField DataField="updateddatetime" HeaderText="Updated Date" SortExpression="updateddatetime" />
                                        <asp:BoundField DataField="updatedby" HeaderText="Updated By" SortExpression="updatedby" />

                                        <asp:templatefield headertext="Active">
                                            <itemtemplate>
	                                            <asp:checkbox id="chkactive" runat="server" Checked='<%# Convert.ToBoolean(Eval("active")) %>' Enabled ="false" />
                                            </itemtemplate>
                                        </asp:templatefield>

                                    </Columns>
                                </asp:GridView>
						    </div>
					</div>				
                </div>

		</ContentTemplate>
	</asp:UpdatePanel>

</asp:Content>
