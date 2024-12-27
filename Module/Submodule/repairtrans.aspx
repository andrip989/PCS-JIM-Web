<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="repairtrans.aspx.cs" Inherits="PCS_JIM_Web.Module.Submodule.repairtrans" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title><%=this.ApplicationTitle() %> - <%=this.ApplicationLokasi() %></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="Pooled Responsive web template, Bootstrap Web Templates, Flat Web Templates, Android Compatible web template, 
    Smartphone Compatible web template, free webdesigns for Nokia, Samsung, LG, SonyEricsson, Motorola web design" />
    <script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); } </script>

    <link href='//fonts.googleapis.com/css?family=Roboto:700,500,300,100italic,100,400' rel='stylesheet' type='text/css'/>
    <link href='//fonts.googleapis.com/css?family=Montserrat:400,700' rel='stylesheet' type='text/css'>

    <link href="<%= ResolveUrl("~/css/bootstrap.min.css") %>" rel='stylesheet' type='text/css' />
    <link href="<%= ResolveUrl("~/css/style.css") %>" rel='stylesheet' type='text/css' />
    <link href="<%= ResolveUrl("~/css/morris.css") %>" rel="stylesheet" type="text/css"/>
    <link href="<%= ResolveUrl("~/css/font-awesome.css") %>" rel="stylesheet"> 
    <link href="<%= ResolveUrl("~/css/icon-font.min.css") %>" rel="stylesheet" type='text/css' />
	<script src="<%= ResolveUrl("~/js/jquery-2.1.4.min.js") %>"></script>
	<script src="<%= ResolveUrl("~/js/jquery-ui.js") %>"></script>

	<link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/css/jquery-ui.css") %>" />

	<link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/css/table-style.css") %>" />
	<link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/css/basictable.css") %>" />
	<script type="text/javascript" src="<%= ResolveUrl("~/js/jquery.basictable.min.js") %>"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/js/global.js") %>"></script>
    <script type="text/javascript" language="javascript">

        function validationSave()
        {
            var roomno = document.getElementById('<%=noroom.ClientID%>').value;

            var x = window.confirm("Are you sure want to control room " + roomno + " ?")
            if (x) {
                return true;
            } else {
                return false;
            }
        }

        function validationCheckout() {
            var roomno = document.getElementById('<%=noroom.ClientID%>').value;

            var x = window.confirm("finish control room = " + roomno + " ?")
            if (x) {
                return true;
            } else {
                return false;
            }
        }

        function execReset() {
            document.getElementById('<%=labelbtncreated.ClientID%>').innerText = "Create";

            document.getElementById('createlinkbtn').onclick = function () {
                var custcodeparam = document.getElementById('<%=custcode.ClientID%>').value;
                        var url = '<%=ResolveUrl("~/Module/Submodule/custcreate.aspx") %>';
                        if (custcodeparam != "") {
                            url = url + "?custcodeparam=" + custcodeparam;
                        }
                        //alert(url);
                        openWindowchildnew(url);

                        return false;
                    };

        }

    </script>
   
     <script type="text/javascript">
         $(function () {
             SetAutoComplete();
         });

         //On UpdatePanel Refresh.
         var prm = Sys.WebForms.PageRequestManager.getInstance();
         if (prm != null)
         {
             prm.add_endRequest(function (sender, e) {
                 if (sender._postBackSettings.panelsToUpdate != null) {
                     SetAutoComplete();
                 }
             });
         }

         function SetAutoComplete() {

             $("[id$=txtSearch]").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/Module/inputtrans.aspx/GetCustomers") %>',
                         data: "{ 'prefix': '" + request.term + "'}",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (data) {
                             response($.map(data.d, function (item) {
                                 //alert(item);

                                 return {
                                     val: item.split('|')[0],
                                     label: item.split('|')[1],
                                     firstname: item.split('|')[1],
                                     lastname: item.split('|')[2],
                                     paymenttype: item.split('|')[3],
                                     businesscode: item.split('|')[4],
                                     creditcardno: item.split('|')[5],
                                     discountcode: item.split('|')[6]
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
                      document.getElementById('<%=labelbtncreated.ClientID%>').innerText = "Update";

                     document.getElementById('<%=firstname.ClientID%>').value = i.item.firstname;
                     document.getElementById('<%=lastname.ClientID%>').value = i.item.lastname;

                     var paymentt = document.getElementById('<%=paymenttype.ClientID%>');
                     paymentt.value = i.item.paymenttype;

                      //alert(i.item.paymenttype);

                     document.getElementById('<%=businesscode.ClientID%>').value = i.item.businesscode;

                     document.getElementById('<%=creditcardno.ClientID%>').value = i.item.creditcardno;
                      
                     document.getElementById('<%=discpct.ClientID%>').value = i.item.discountcode;

                     document.getElementById('createlinkbtn').onclick = function () {
                         var custcodeparam = document.getElementById('<%=custcode.ClientID%>').value;
                         var url = '<%=ResolveUrl("~/Module/Submodule/custcreate.aspx") %>';
                         if (custcodeparam != "") {
                             url = url + "?custcodeparam=" + custcodeparam;
                         }
                         //alert(url);
                         openWindowchildnew(url);

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
         }


         function checkavailableroom() {

             //var transactionidv = document.getElementById('<%=transactionid.ClientID%>').value;
             //alert(transactionidv);
             if (Page_ClientValidate('valGroup3')) {
                 var custcodeparam = document.getElementById('<%=custcode.ClientID%>').value;
                 var noroomparam = document.getElementById('<%=noroom.ClientID%>').value;
                 var startdateparam = document.getElementById('<%=datetimestart.ClientID%>').value;
                 var enddateparam = document.getElementById('<%=datetimeend.ClientID%>').value;

                 var btn = document.getElementById("<%=submit.ClientID%>");
                 btn.disabled = true;

                 $.ajax({
                     url: '<%=ResolveUrl("~/Module/inputtrans.aspx/CheckAvailable") %>',
                     type: "POST",
                     data: "{ 'custcodeparam': '" + custcodeparam + "' "+ 
                             ",'noroomparam': '" + noroomparam + "' "+ 
                             ",'startdateparam': '" + startdateparam + "' " + 
                             ",'enddateparam': '" + enddateparam + "' " + 
                            "}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (data) {
                         if (data.d == "ok") {
                             alert("room available");

                             btn.disabled = false;
                         
                         }
                         else {
                             alert(data.d);
                             btn.disabled = true;
                         }

                     },
                     error: function (response) {
                         alert(response.responseText);
                     },
                     failure: function (response) {
                         alert(response.responseText);
                     }
                 });

             }
         }

     </script>

    <script type="text/javascript" language="javascript">
        function updatepricing()
        {
            var _totalcharges = document.getElementById('<%=totalcharges.ClientID%>').value;
            var _discount = document.getElementById('<%=discount.ClientID%>').value;
            var _totaltax = document.getElementById('<%=totaltax.ClientID%>').value;
            var _totalrate = document.getElementById('<%=totalrate.ClientID%>').value;
            var _extracharges = document.getElementById('<%=extracharges.ClientID%>').value;
            var _total = document.getElementById('<%=total.ClientID%>').value;
            var _flatdiscount = document.getElementById('<%=flatdiscount.ClientID%>').value;
            var _amountpaid = document.getElementById('<%=amountpaid.ClientID%>').value;
            var _deposit = document.getElementById('<%=deposit.ClientID%>').value;
            var _roundoff = document.getElementById('<%=roundoff.ClientID%>').value;
            var _balance = document.getElementById('<%=balance.ClientID%>').value;

            _totalcharges = parseFloat(convertnumseparator(_totalcharges, "."));
            _discount = parseFloat(convertnumseparator(_discount, "."));
            _totaltax = parseFloat(convertnumseparator(_totaltax, "."));
            _totalrate = parseFloat(convertnumseparator(_totalrate, "."));
            _extracharges = parseFloat(convertnumseparator(_extracharges, "."));
            _total = parseFloat(convertnumseparator(_total, "."));
            _flatdiscount = parseFloat(convertnumseparator(_flatdiscount, "."));
            _amountpaid = parseFloat(convertnumseparator(_amountpaid, "."));
            _deposit = parseFloat(convertnumseparator(_deposit, "."));
            _roundoff = parseFloat(convertnumseparator(_roundoff, "."));
            _balance = parseFloat(convertnumseparator(_balance, "."));

            _totalrate = _totalcharges - _discount + _totaltax;
            document.getElementById('<%=totalrate.ClientID%>').value = convertnum(_totalrate, decimalpoint, decimaldigit, thousandsseparator);
            
            _total = _totalrate + _extracharges ;

            document.getElementById('<%=total.ClientID%>').value = convertnum(_total, decimalpoint, decimaldigit, thousandsseparator);

            var totalbalance = 0;
            totalbalance = _total - _flatdiscount - _amountpaid - _deposit - _roundoff;

            _balance = totalbalance;

            _balance = Math.floor(_balance / 100) * 100;

            _roundoff = totalbalance - _balance;

            document.getElementById('<%=roundoff.ClientID%>').value = convertnum(_roundoff, decimalpoint, decimaldigit, thousandsseparator);

            document.getElementById('<%=balance.ClientID%>').value = convertnum(_balance, decimalpoint, decimaldigit, thousandsseparator);
        }        
    </script>   
</head>
<body>
     <div class="page-container">
   <!--/content-inner-->
   <div class="mother-grid-inner">
              <!--header start here-->
				
<!--heder end here-->
 		<form id="form1" runat="server">
       <div class="grid-form1">
	        <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="<%= ResolveUrl("default.aspx") %>">Home</a><i class="fa fa-angle-right"></i>Maintenance <i class="fa fa-angle-right"></i> Control Room Input</li>
            </ol>
        </div>
        <div class="grid-form1">
		<!--grid-->
 		<h3 id="forms-example"><asp:label runat="server" ID="typetransaksi"></asp:label> <asp:TextBox ID="transactionid" runat="server" BorderStyle="None" ReadOnly="true" Visible="false"></asp:TextBox> </h3>
        </div>

<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server"></asp:ScriptManager>
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>

  <div class="w3-agile-chat">
				<div class="charts">
					<div class="col-md-4 w3layouts-char" runat="server" id="hide1">
						<div class="charts-grids widget">
							<h4 class="title">Guest Information</h4>
                            
                            <div class="form-group1">
                            <label for="txtSearch">Search Guest</label>
                               <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" />
                             </div>
                             <a href="#" id="createlinkbtn" style="color: #000" onclick="<%= this.getopenURL() %>" class ="hvr-icon-float-away col-15 btn"><asp:Label runat="server" ID="labelbtncreated"></asp:Label></a>
                              <button type="reset" onclick="execReset()" class="btn-default btn">Reset</button>
                            
                            <div class="form-group1">
                            <label for="custcode">Cust. Code</label>
                               <asp:TextBox ID="custcode" runat="server" CssClass="form-control" Enabled="false" />
                            </div>
                            <div class="form-group1">
                            <label for="firstname">First Name</label>
                               <asp:TextBox ID="firstname" runat="server" CssClass="form-control" Enabled="false" />
                            </div>
                            <div class="form-group1">
                            <label for="lastname">Last Name</label>
                               <asp:TextBox ID="lastname" runat="server" CssClass="form-control" Enabled="false" />
                            </div>

                            <div class="form-group1">
                              <label for="discpct">Disc. %</label>
                              <asp:TextBox ID="discpct" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>

                            <div class="form-group1">
                            <label for="paymenttype">Payment Type</label>
                               <asp:DropDownList ID="paymenttype" runat="server" CssClass="form-control">
                               </asp:DropDownList>
                            </div>
                            
                             <div class="form-group1">
                                  <label for="businesscode">Business Code</label>
                                 <asp:DropDownList ID="businesscode" runat="server" CssClass="form-control">
                                      <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                                      </asp:ListItem>
                                 </asp:DropDownList>
                            </div>
                            <div class="form-group1">
                                  <label for="creditcardno">Credit Card No</label>
                                  <asp:TextBox ID="creditcardno" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                              </div>
                             <div class="form-group1">
                                      <label for="description">Guest Remark</label>
                                      <asp:TextBox ID="keterangantambahan" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            <div class="form-group1">
                                      <label for="refbookingcode">Ref. Booking Code</label>
                                      <asp:TextBox ID="refbookingcode" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
						</div>
					</div>
					<div class="col-md-6 w3-char">
						<div class="charts-grids widget states-mdl">
							<h4 class="title">Stay Information</h4>
                            
                              <div class="form-group1">
                                <label for="noroom">No. Room</label>
                                 <asp:DropDownList ID="noroom" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="listoutletno_SelectedIndexChanged">
                                 </asp:DropDownList>
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup='valGroup3' runat="server" ControlToValidate="noroom" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
                              </div>

                                <asp:HiddenField ID="recidroom" runat="server" />
                                
                                  <div class="form-group1">
                                      <label for="description">Room Description</label>
                                      <asp:TextBox ID="description" TextMode="MultiLine" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                  </div>

                              <div class="form-group1">
                                  <label for="floortype">Floor</label>
                                 <asp:DropDownList ID="floortype" runat="server" CssClass="form-control" Enabled="false">
                                 </asp:DropDownList>
                            </div>
                             <div class="form-group1">
                                  <label for="roomtype">Room Type</label>
                                 <asp:DropDownList ID="roomtype" runat="server" CssClass="form-control" Enabled="false" >
                                 </asp:DropDownList>
                            </div>

                             <div class="form-group1">
                                  <label for="roomtype">Control Type</label>
                                 <asp:DropDownList ID="maintenancetype" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="maintenancetype_SelectedIndexChanged" >
                                 </asp:DropDownList>

                                 <asp:HiddenField ID="minutesparam" runat="server" />
                            </div>

                                 <div class="form-group1">
                                    <label for="datetimestart">Start Time</label>
                                    <asp:TextBox ID="datetimestart" placeholder="Date - Checkin" CssClass="form-control" runat="server" type="datetime-local"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup='valGroup3' runat="server" ControlToValidate="datetimestart" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
                                  </div>  
                            
                                <div class="form-group1">
                                    <label for="datetimeend">End Time</label>
                                    <asp:TextBox ID="datetimeend" placeholder="Date - Checkout" runat="server" AutoPostBack="true" OnTextChanged="datetimeend_TextChanged1" CssClass="form-control" type="datetime-local"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup='valGroup3' runat="server" ControlToValidate="datetimeend" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
                                  </div>
                                    <% if(transactionid.Text == "") {  %>
                                  <a id="checkavail" class ="hvr-icon-spin col-23 btn" style="color: #fff" onclick="checkavailableroom(); return false;">Check</a>
                                    <% } %>
                                  <div class="form-group1">
                                      <label for="jumlahhari">Total Time</label>
                                      <asp:TextBox ID="jumlahhari" ValidationGroup='valGroup3' runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                  </div>
                            
                            <div class="form-group1">
                                  <div class="col-sm-6">
                                  <asp:TextBox ID="jumlahadult" TextMode="Number" OnTextChanged="jumlahadult_TextChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:TextBox>
                                  </div>
                                 
                                     <div class="col-sm-4">
                                        <asp:TextBox ID="maxadult" runat="server" CssClass="form-control" Visible="false" ReadOnly="true"></asp:TextBox>  
                                    </div>
                              </div>
                            <div class="clearfix"> </div>
                            
                              <div class="form-group1">
                                  <div class="col-sm-6">  
                                  <asp:TextBox ID="jumlahanak" TextMode="Number" OnTextChanged="jumlahanak_TextChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:TextBox>
                                  </div>
                                   <div class="col-sm-4">
                                        <asp:TextBox ID="maxchild" runat="server" CssClass="form-control" Visible="false" ReadOnly="true"></asp:TextBox>  
                                    </div>
                             </div>
                            <div class="clearfix"> </div>
                             <div class="form-group1">
                                <label for="allowsmoking">Smoking Room</label>
                                <br />
                                    <asp:CheckBox ID="allowsmoking" runat="server" Enabled="false" />
                              </div>
                                  
						</div>
					</div>
                    <div class="col-md-5 w3l-char">
                        <div class="charts-grids widget">
                            <h4 class="title">Confirmation</h4>

                                <div class="form-group1">
                                      <label for="description">Remark</label>
                                      <asp:TextBox ID="remark" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                             <div class="form-group1">
                                <a id="housekeepbtn2" runat="server" onserverclick="housekeepbtn_Click" onclick="if( validationSave() ) { return true; } return false;"><div class="bg-info pv20 text-white fw600 text-center">Submit</div></a> 
                                <a id="btnclose" onclick="window.close();" runat="server"><div class="bg-alert light pv20 text-white fw600 text-center">Cancel</div></a>
                                <a id="housekeepbtnclose" runat="server" visible="false" onserverclick="housekeepbtnclose_ServerClick" onclick="if( validationCheckout() ) { return true; } return false;"><div class="bg-dark light pv20 text-white fw600 text-center">Finish</div></a> 
                             </div>
                         </div>
                    </div>
					<div class="col-md-4 w3l-char" runat="server" id="hide2">
						<div class="charts-grids widget">
							<h4 class="title">Rate Information</h4>
                            
                          <div class="form-group1">
                              <label for="totalcharges">Total Charges</label>
                              <asp:TextBox ID="totalcharges" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>

                          <div class="form-group1">
                              <label for="discount">Discount</label>
                              <asp:TextBox ID="discount" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>

                          <div class="form-group1">
                              <label for="totaltax">Total Tax</label>
                              <asp:TextBox ID="totaltax" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>
                          <div class="form-group1">
                              <label for="totalrate">Total Rate</label>
                              <asp:TextBox ID="totalrate" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>

                          <div class="form-group1">
                              <label for="extracharges">Extra Charges</label>
                              <asp:TextBox ID="extracharges" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>

                          <div class="form-group1">
                              <label for="total">Total</label>
                              <asp:TextBox ID="total" BackColor="LightGreen" ForeColor="Black" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                          </div>
                           
                          <div class="form-group1">
                              <label for="flatdiscount">Flat Discount</label>
                              <asp:TextBox ID="flatdiscount" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>

                          <div class="form-group1">
                              <label for="amountpaid">Amount Paid</label>
                              <asp:TextBox ID="amountpaid" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>

                          <div class="form-group1">
                              <label for="deposit">Deposit</label>
                              <asp:TextBox ID="deposit" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>

                          <div class="form-group1">
                              <label for="roundoff">RoundOff</label>
                              <asp:TextBox ID="roundoff" runat="server" onchange="updatepricing()" CssClass="form-control" Enabled="false"></asp:TextBox>
                          </div>

                           <div class="form-group1">
                              <label for="balance">Balance</label>
                              <asp:TextBox ID="balance" BackColor="LimeGreen" ForeColor="Black" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                          </div>

						</div>
					</div>
					<div class="clearfix"> </div>
           </div>
    </div>

 	<div class="grid-form" runat="server" id="hide3">
 		<div class="grid-form1">
            
        <div class="vali-form">
            <div class="col-md-4 form-group1">
             <h4 class="title">Season & Rate</h4>
            <label for="ratetype">Rate Type</label>
            <asp:DropDownList ID="ratetype" runat="server" CssClass="form-control" OnSelectedIndexChanged="ratetype_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>

            </div>
            <div class="col-md-4 form-group1">
                 <h4 class="title">Use Tax</h4>
                <label for="usetax">Use Tax</label>
                    <asp:CheckBoxList ID="usetax" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Tax1" Value="Tax1">Tax 1</asp:ListItem>
                        <asp:ListItem Text="Tax2" Value="Tax2">Tax 2</asp:ListItem>
                        <asp:ListItem Text="Tax3" Value="Tax3">Tax 3</asp:ListItem>
                        <asp:ListItem Text="Tax4" Value="Tax4">Tax 4</asp:ListItem>
                    </asp:CheckBoxList>        
            </div>
            <div class="col-md-4 form-group1">
                 <h4 class="title">Amenities Information</h4>
                <label for="roomamenities">Room Amenities</label>
                    <asp:CheckBoxList ID="roomamenities" runat="server" RepeatDirection="Vertical" Enabled="false" Datafield="description" DataValueField="roomamenities">
                    </asp:CheckBoxList>                
             </div>
        </div>
    
    <div class="clearfix"> </div>

    

  <hr />

<div class="col-md-4 form-group1">
    <asp:Button ID="submit" ValidationGroup='valGroup3' runat="server" Text="Submit" CssClass="btn-primary btn" OnClientClick="if( validationSave() ) { return true; } return false;" OnClick="submit_Click" Enabled="false"  />

    <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn-warning btn" OnClientClick="window.close();"  /> 


     <asp:Button ID="btncheckout" Visible="false" runat="server" Text="Check Out" CssClass="btn-toolbar btn" OnClientClick="if( validationCheckout() ) { return true; } return false;" OnClick="btncheckout_Click"  />
</div>
    <div class="clearfix"> </div>
</div>
</div>
  </contenttemplate>
  </asp:UpdatePanel>
</form>
</div>

  </div>
 	
</body>
</html>