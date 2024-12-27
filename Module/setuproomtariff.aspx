<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="setuproomtariff.aspx.cs" Inherits="PCS_JIM_Web.Module.setuproomtariff" %>
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

    <script type="text/javascript">
        $(function () {
            $("[id$=txtSearch]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Module/setuproomtariff.aspx/GetCustomers") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    val: item.split('-')[0],
                                    label: item.split('-')[1]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $("[id$=custcode]").val(i.item.val);                    
                },
                minLength: 2
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                var div = $("<li>")
                    .append("<a class='dvDetails'><span class='labelText'>" + item.label +
                        "</span><span class='valueText'>(" + item.val + ")</span></a>")
                    .appendTo(ul);
                return div;
            };;
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Setup<i class="fa fa-angle-right"></i> Room Tariff</li>
            </ol>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate>
    <div class="grid-form1">
<div class="form-horizontal"> 

  <div class="form-group">
    <label for="startdate" class="col-sm-2 control-label hor-form">Start Date</label>
    <div class="col-sm-4">
        <asp:TextBox ID="startdate" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup='valGroup3' runat="server" ControlToValidate="startdate" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
  </div>

         <div class="form-group">
            <label for="floortype" class="col-sm-2 control-label hor-form">Floor Types</label>
    
            <div class="col-sm-6">
                 <asp:DropDownList ID="floortype" runat="server" CssClass="form-control">
                        <asp:ListItem Text="--Select Floor--" Enabled="true" Selected="True" Value=''>
                        </asp:ListItem>
                </asp:DropDownList>
            </div>
          </div>

        <div class="form-group">
            <label for="roomtypes" class="col-sm-2 control-label hor-form">Room Types</label>
    
            <div class="col-sm-6">
                 <asp:DropDownList ID="roomtypes" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="roomtypes_TextChanged">
                        <asp:ListItem Text="--Select Room Type--" Enabled="true" Selected="True" Value=''>
                        </asp:ListItem>
                </asp:DropDownList>
            </div>
          </div>

        <div class="form-group">
            <label for="noroom" class="col-sm-2 control-label hor-form">Room No.</label>
    
            <div class="col-sm-6">
                 <asp:DropDownList ID="noroom" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
          </div>

         <div class="form-group">
            <label for="ratetypes" class="col-sm-2 control-label hor-form">Rate Type</label>
    
            <div class="col-sm-6">
                 <asp:DropDownList ID="ratetypes" runat="server" CssClass="form-control">
                        <asp:ListItem Text="--Select Rate Type--" Enabled="true" Selected="True" Value=''>
                        </asp:ListItem>
                </asp:DropDownList>

                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup='valGroup3' runat="server" ControlToValidate="ratetypes" BackColor="Red" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
          </div>

         <div class="form-group">
            <label for="seasontype" class="col-sm-2 control-label hor-form">Season Type</label>
    
            <div class="col-sm-6">
                 <asp:DropDownList ID="seasontype" runat="server" CssClass="form-control">
                        <asp:ListItem Text="--Select Season--" Enabled="true" Selected="True" Value=''>
                        </asp:ListItem>
                </asp:DropDownList>
            </div>
          </div>
        
        <div class="form-group">
            <label for="bussinesssource" class="col-sm-2 control-label hor-form">Bussiness Source</label>
    
            <div class="col-sm-6">
                 <asp:DropDownList ID="bussinesssource" runat="server" CssClass="form-control">
                        <asp:ListItem Text="--Select Bussiness Source--" Enabled="true" Selected="True" Value=''>
                        </asp:ListItem>
                </asp:DropDownList>
            </div>
          </div>

        <div class="form-group">
            <label for="txtSearch" class="col-sm-2 control-label hor-form">Cust. Code</label>
    
            <div class="col-sm-2">
                 <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" />
                 <asp:HiddenField ID="custcode" runat="server" />

            </div>
          </div>
        
        <div class="form-group">
                <label for="usetax" class="col-sm-2 control-label hor-form">Use Tax</label>
                <div class="col-sm-6">
                    <asp:CheckBox ID="usetax" runat="server" />
                </div>
         </div>

        <div class="form-group">
                <label for="tariff" class="col-sm-2 control-label hor-form">Tariff</label>
                <div class="col-sm-2">
                    <asp:TextBox ID="tariff" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
         </div>

        <div class="form-group">
            <label for="extraadult" class="col-sm-2 control-label hor-form">Extra Adult</label>
            <div class="col-sm-2">
                <asp:TextBox ID="extraadult" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <label for="extrachild" class="col-sm-1 control-label hor-form">Extra Child</label>
            <div class="col-sm-2">
                <asp:TextBox ID="extrachild" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>


        <div class="form-group">
                <label for="active" class="col-sm-2 control-label hor-form">Active</label>
                <div class="col-sm-6">
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
                                        
                                        <asp:BoundField DataField="startdate" HeaderText="Start Date" SortExpression="startdate" />

                                        <asp:BoundField DataField="floortype" HeaderText="Floor Type" SortExpression="floortype" />
                                        <asp:BoundField DataField="roomtypes" HeaderText="Room Types" SortExpression="roomtypes" />
                                        <asp:BoundField DataField="noroom" HeaderText="No Room" SortExpression="noroom" />
                                        <asp:BoundField DataField="ratetypes" HeaderText="Rate Types" SortExpression="ratetypes" />
                                        <asp:BoundField DataField="seasontype" HeaderText="Season Type" SortExpression="seasontype" />
                                        <asp:BoundField DataField="bussinesssource" HeaderText="Bussiness Source" SortExpression="bussinesssource" />
                                        <asp:BoundField DataField="custcode" HeaderText="Cust. Code" SortExpression="custcode" />                                                                               
                                        
                                        <asp:templatefield headertext="Tax">
                                            <itemtemplate>
	                                            <asp:checkbox id="chkusetax" runat="server" Checked='<%# Convert.ToBoolean(Eval("usetax")) %>' Enabled ="false" />
                                            </itemtemplate>
                                        </asp:templatefield>

                                        <asp:BoundField DataField="tariff" HeaderText="Tariff" SortExpression="tariff" />
                                        <asp:BoundField DataField="extraadult" HeaderText="Extra Adult" SortExpression="extraadult" />
                                        <asp:BoundField DataField="extrachild" HeaderText="Extra Child" SortExpression="extrachild" />                                                                               

                                        <asp:templatefield headertext="Active">
                                            <itemtemplate>
	                                            <asp:checkbox id="chkactive" runat="server" Checked='<%# Convert.ToBoolean(Eval("active")) %>' Enabled ="false" />
                                            </itemtemplate>
                                        </asp:templatefield>

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
