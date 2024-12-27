<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="setupguestlist.aspx.cs" Inherits="PCS_JIM_Web.Module.setupguestlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <script type="text/javascript">
        $(function () {
            $("[id$=txtSearch]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Module/setupguestlist.aspx/GetCustomers") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    val: item.split('-')[0],
                                    label: item.split('-')[1] + ' , ' + item.split('-')[2]
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
                    $("[id$=hfCustomerId]").val(i.item.val);

                    document.getElementById('<%=labelbtncreated.ClientID%>').innerHTML = "Update";

                    document.getElementById('createlinkbtn').onclick = function () {
                        var custcodeparam = document.getElementById('<%=hfCustomerId.ClientID%>').value;
                        var url = '<%=ResolveUrl("~/Module/Submodule/custcreate.aspx") %>';
                        if (custcodeparam != "")
                        {
                            url = url + "?custcodeparam=" + custcodeparam;
                        }
                        //alert(url);
                        openWindowchild(url);

                        return false;
                    };
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
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Setup<i class="fa fa-angle-right"></i> Guest List</li>
            </ol>
    <div class="grid-form1">
<div class="form-horizontal">
   <div class="form-group">
    <label for="custcodelist" class="col-sm-2 control-label hor-form">Search Guest</label>
    <div class="col-sm-8">
       <asp:TextBox ID="txtSearch" runat="server" />
        <asp:HiddenField ID="hfCustomerId" runat="server" />
        
    </div>
  </div>

  <div class="form-group">
    <div class="col-sm-offset-2 col-sm-10">
      <asp:Button ID="submit" runat="server" Text="Search" CssClass="btn-system btn" OnClick="submit_Click"  />
        <button type="reset" class="btn-default btn">Reset</button>
        <a href="#" id="createlinkbtn" onclick="<%= this.getopenURL() %>" class ="hvr-icon-float-away col-15 btn"><asp:Label runat="server" Text="Create" ID="labelbtncreated"></asp:Label></a>
    </div>
  </div>



</div>
</div>


                <div class="agile-grids">	
					<div class="agile-tables">
					        <div class="w3l-table-info">
                                <asp:GridView ID="GridView1" runat="server" OnPageIndexChanging="GridView1_PageIndexChanging" >
                                    <Columns>
                                      
                                        <asp:templatefield headertext="Cust. Code">
                                            <itemtemplate>
                                                <a href="#" onclick='<%# "openWindowchild(\""+ResolveUrl("~/Module/Submodule/custcreate.aspx") + "?custcodeparam=" + Eval("custcode") + "\");" %>'><%# Eval("custcode") %></a>
                                            </itemtemplate>
                                        </asp:templatefield>
                                      
                                            <asp:BoundField DataField="firstname" HeaderText="First Name" SortExpression="firstname" />
                                            <asp:BoundField DataField="lastname" HeaderText="Last Name" SortExpression="lastname" />
                                            <asp:BoundField DataField="phone" HeaderText="Phone Number" SortExpression="phone" />
                                            <asp:BoundField DataField="telephone" HeaderText="Phone Number 2" SortExpression="telephone" />
                                            <asp:BoundField DataField="email" HeaderText="Email" SortExpression="email" />
                                            <asp:BoundField DataField="identificationid" HeaderText="No. Identitas" SortExpression="identificationid" />
                                            <asp:BoundField DataField="paymenttype" HeaderText="Payment" SortExpression="paymenttype" />
                                            <asp:BoundField DataField="createddate" HeaderText="Created Date" SortExpression="createddate" />
                                             <asp:BoundField DataField="createdby" HeaderText="Created By" SortExpression="createdby" />
                                    </Columns>
                                </asp:GridView>
						    </div>
					</div>				
                </div>
		
</asp:Content>
