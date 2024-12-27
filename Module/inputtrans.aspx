<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="inputtrans.aspx.cs" Inherits="PCS_JIM_Web.Module.inputtrans" %>

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

            var transactionid_ = document.getElementById('<%=transactionid.ClientID%>').value; 

            if (transactionid_)
            {
                return true;
            }
            else
            {
                var x = window.confirm("Are you sure want to reservation " + roomno + " ?")
                if (x) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        function validationCheckout() {
            var roomno = document.getElementById('<%=noroom.ClientID%>').value;

            var x = window.confirm("Are you sure want to check-out " + roomno + " ?")
            if (x) {
                return true;
            } else {
                return false;
            }
        }

        function validationCancel() {
            var roomno = document.getElementById('<%=noroom.ClientID%>').value;

            var x = window.confirm("Are you sure want to cancel " + roomno + " ?")
            if (x) {
                return true;
            } else {
                return false;
            }
        }

        function validationCheckin() {
            var roomno = document.getElementById('<%=noroom.ClientID%>').value;

            var x = window.confirm("Are you sure want to check-in " + roomno + " ?")
            if (x) {
                return true;
            } else {
                return false;
            }
        }

        function validationNoShow() {
            var roomno = document.getElementById('<%=noroom.ClientID%>').value;

            var x = window.confirm("Are you sure want to no show " + roomno + " ?")
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
                                     label: item.split('|')[1] + ' , ' + item.split('|')[7],
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
                         else if (data.d == "room is dirty") {
                             var x = window.confirm("room is dirty Are you sure want to continue ?")
                             if (x) {
                                 btn.disabled = false;
                             } else {
                                 btn.disabled = true;
                             }
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

            if (_flatdiscount == "") _flatdiscount = "0";

            var _servicecharge = document.getElementById('<%=servicecharge.ClientID%>').value;
            var _pb1charge = document.getElementById('<%=pb1charge.ClientID%>').value;

            var _scpct = document.getElementById('<%=scpct.ClientID%>').value;
            var _scamount = document.getElementById('<%=scamount.ClientID%>').value;
            var _pb1pct = document.getElementById('<%=pb1pct.ClientID%>').value;
            var _pb1amount = document.getElementById('<%=pb1amount.ClientID%>').value;

            _totalcharges = parseFloat(convertnumseparator(_totalcharges, "."));
            _discount = parseFloat(convertnumseparator(_discount, "."));
            _totaltax = parseFloat(convertnumseparator(_totaltax, "."));
            _totalrate = parseFloat(convertnumseparator(_totalrate, "."));
            _extracharges = parseFloat(convertnumseparator(_extracharges, "."));
            _total = parseFloat(convertnumseparator(_total, "."));
            _flatdiscount = parseFloat(convertnumseparator(_flatdiscount, "."));
            _amountpaid = parseFloat(convertnumseparator(_totalcharges, "."));
            _deposit = parseFloat(convertnumseparator(_deposit, "."));
            _roundoff = parseFloat(convertnumseparator(_roundoff, "."));
            _balance = parseFloat(convertnumseparator(_balance, "."));


            _servicecharge = _totalcharges - _discount + _extracharges;
            _servicecharge = _servicecharge * _scpct / 100;

            _pb1charge = _totalcharges - _discount + _extracharges + _servicecharge;
            _pb1charge = _pb1charge * _pb1pct / 100;

            document.getElementById('<%=servicecharge.ClientID%>').value = convertnum(_servicecharge, decimalpoint, decimaldigit, thousandsseparator);
            document.getElementById('<%=pb1charge.ClientID%>').value = convertnum(_pb1charge, decimalpoint, decimaldigit, thousandsseparator);

            _totaltax = _servicecharge + _pb1charge;

            _totalrate = _totalcharges - _discount + _extracharges;

            document.getElementById('<%=totalrate.ClientID%>').value = convertnum(_totalrate, decimalpoint, decimaldigit, thousandsseparator);
            
            _total = _totalrate + _totaltax;

            document.getElementById('<%=total.ClientID%>').value = convertnum(_total, decimalpoint, decimaldigit, thousandsseparator);

            var totalbalance = 0;

            _amountpaid = _total - _flatdiscount;
            document.getElementById('<%=amountpaid.ClientID%>').value = convertnum(_amountpaid, decimalpoint, decimaldigit, thousandsseparator);

            totalbalance = _amountpaid + _flatdiscount - _total - _deposit;

            _balance = totalbalance;

            //_balance = Math.floor(_balance / 100) * 100;

            //_roundoff = totalbalance - _balance;

            //document.getElementById('<%=roundoff.ClientID%>').value = convertnum(_roundoff, decimalpoint, decimaldigit, thousandsseparator);

            //_balance = _amountpaid - _deposit - _roundoff;
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
                <li class="breadcrumb-item"><a href="<%= ResolveUrl("default.aspx") %>">Home</a><i class="fa fa-angle-right"></i>Forms <i class="fa fa-angle-right"></i> Input</li>
            </ol>
        </div>
        <div class="grid-form1">
		<!--grid-->
 		<h3 id="forms-example"><asp:label runat="server" ID="typetransaksi"></asp:label> <asp:TextBox ID="transactionid" runat="server" BorderStyle="None" ReadOnly="true" Visible="false"></asp:TextBox> </h3>
        </div>

<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server"></asp:ScriptManager>
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>
                <asp:HiddenField ID="tipetransparam" runat="server" />
  <div class="w3-agile-chat">
				<div class="charts">
					<div class="col-md-4 w3layouts-char">
						<div class="charts-grids widget">
                            <div class="form-group1">
                                      <b>Ref. Booking Code</b>
                                      <asp:TextBox ID="refbookingcode" runat="server" CssClass="form-control"></asp:TextBox>
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup='valGroup3' runat="server" ControlToValidate="refbookingcode" BackColor="Red" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>

							<h4 class="title">Guest Information</h4>
                            
                            <div class="form-group1">
                            <b>Search Guest</b>
                               <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" />
                             </div>
                             <a href="#" id="createlinkbtn" style="color: #000" onclick="<%= this.getopenURL() %>" class ="hvr-icon-float-away col-15 btn"><asp:Label runat="server" ID="labelbtncreated"></asp:Label></a>
                              <button type="reset" onclick="execReset()" class="btn-default btn">Reset</button>
                            <br />
                            <div class="form-group1">
                            <b>Cust. Code</b>
                               <asp:TextBox ID="custcode" runat="server" CssClass="form-control" Enabled="false" />
                            </div>
                             <br />
                            <div class="form-group1">
                            <b>First Name</b>
                               <asp:TextBox ID="firstname" runat="server" CssClass="form-control" Enabled="false" />
                            </div>
                            <br />
                            <div class="form-group1">
                            <b>Last Name</b>
                               <asp:TextBox ID="lastname" runat="server" CssClass="form-control" Enabled="false" />
                            </div>
                            <br />
                            <div class="form-group1">
                              <b>Disc. %</b>
                              <asp:TextBox ID="discpct" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <br />
                            <div class="form-group1">
                            <b>Payment Type</b>
                               <asp:DropDownList ID="paymenttype" runat="server" CssClass="form-control">
                                   <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                                      </asp:ListItem>
                               </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup='valGroup3' runat="server" ControlToValidate="paymenttype" BackColor="Red" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                            <br />
                             <div class="form-group1">
                                  <b>Booking Source</b>
                                 <asp:DropDownList ID="businesscode" runat="server" CssClass="form-control">
                                      <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                                      </asp:ListItem>
                                 </asp:DropDownList>
                            </div>
                            <br />
                            <div class="form-group1">
                                  <b>Credit Card No</b>
                                  <asp:TextBox ID="creditcardno" runat="server" CssClass="form-control"></asp:TextBox>
                              </div>
                            <br />
                             <div class="form-group1">
                                      <b>Guest Remark</b>
                                      <asp:TextBox ID="keterangantambahan" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            <br />
                                
						</div>
					</div>
					<div class="col-md-4 w3-char">
						<div class="charts-grids widget states-mdl">
							<h4 class="title">Stay Information</h4>
                            
                              <div class="form-group1">
                                <b>No. Room</b>
                                 <asp:DropDownList ID="noroom" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="listoutletno_SelectedIndexChanged">
                                 </asp:DropDownList>
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup='valGroup3' runat="server" ControlToValidate="noroom" BackColor="Red" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
                              </div>

                                <asp:HiddenField ID="recidroom" runat="server" />
                                <asp:HiddenField ID="statushousekeeping" runat="server" />
                                <asp:HiddenField ID="statusmaintenance" runat="server" />
                                <br />
                                  <div class="form-group1">
                                      <b>Room Description</b>
                                      <asp:TextBox ID="description" TextMode="MultiLine" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                  </div>
                                <br />
                              <div class="form-group1">
                                  <b>Floor</b>
                                 <asp:DropDownList ID="floortype" runat="server" CssClass="form-control" Enabled="false">
                                 </asp:DropDownList>
                            </div>
                            <br />
                             <div class="form-group1">
                                  <b>Room Type</b>
                                 <asp:DropDownList ID="roomtype" runat="server" CssClass="form-control" Enabled="false" >
                                 </asp:DropDownList>
                            </div>
                            <br />
                             <div class="form-group1">
                                <b>Smoking Room</b>
                                <br />
                                    <asp:CheckBox ID="allowsmoking" runat="server" Enabled="false" />
                              </div>
                                <br />
                                 <div class="form-group1">
                                    <b>Check - in</b>
                                    <asp:TextBox ID="datetimestart" placeholder="Date - Checkin" AutoPostBack="true" OnTextChanged="datetimestart_TextChanged" CssClass="form-control" runat="server" type="datetime-local"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup='valGroup3' runat="server" ControlToValidate="datetimestart" BackColor="Red" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
                                  </div>  
                               <br />
                                <b>Check - out</b>
                                <div class="form-group1">
                                    
                                    <div class="col-sm-6">
                                    <asp:TextBox ID="datetimeend" placeholder="Date - Checkout" runat="server" AutoPostBack="true" CssClass="form-control" OnTextChanged="datetimeend_TextChanged" TextMode="Date"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup='valGroup3' runat="server" ControlToValidate="datetimeend" BackColor="Red" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
                                     </div>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="datetimeend2" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>  
                                        <asp:CheckBox ID="latecheckout" runat="server" Text="Late Checkout / Extends" AutoPostBack="true" OnCheckedChanged="latecheckout_CheckedChanged" />
                                    </div>
                                </div>
                                <br />
                                    <% if(transactionid.Text == "") {  %>
                                  <div class="form-group1">
                                     <a id="checkavail" class ="hvr-icon-spin col-23 btn" style="color: #fff" CssClass="form-control" onclick="checkavailableroom(); return false;">Check</a>
                                 </div>
                                        <% } %>
                                  <div class="form-group1">
                                      <label for="jumlahhari">Jumlah Hari</label>
                                      <asp:TextBox ID="jumlahhari" ValidationGroup='valGroup3' runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                  </div>
                             <br />
                            <div class="form-group1">
                                  <div class="col-sm-6">
                                   <b>Adult</b>   
                                  <asp:TextBox ID="jumlahadult" TextMode="Number" OnTextChanged="jumlahadult_TextChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:TextBox>
                                  </div>
                                 
                                     <div class="col-sm-4">
                                         <b>Max</b>
                                        <asp:TextBox ID="maxadult" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>  
                                    </div>
                              </div>
                            <br />
                            
                              <div class="form-group1">
                                  <div class="col-sm-6">  
                                       <b>Child</b>
                                  <asp:TextBox ID="jumlahanak" TextMode="Number" OnTextChanged="jumlahanak_TextChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:TextBox>
                                  </div>
                                   <div class="col-sm-4">
                                       <b>Max</b>
                                        <asp:TextBox ID="maxchild" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>  
                                    </div>
                             </div>
                            <div class="clearfix"> </div>
                            
						</div>
					</div>
					<div class="col-md-4 w3l-char">
						<div class="charts-grids widget">
							<h4 class="title">Rate Information</h4>
                            
                          <div class="form-group1">
                              <b>Total Charges</b>
                              <asp:TextBox ID="totalcharges" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>
                            <br />
                          <div class="form-group1">
                              <b>Discount</b>
                              <asp:TextBox ID="discount" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>
                            <br />
                          <div class="form-group1">
                              <b>Extra Charges</b>
                              <asp:TextBox ID="extracharges" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>
                            <br />
                          <div class="form-group1">
                              <b>Total Rate</b>
                              <asp:TextBox ID="totalrate" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>
                            <br />
                          <div class="form-group1">
                              <b>Service Charge</b>
                              <asp:TextBox ID="servicecharge" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                              <asp:HiddenField id="scpct" runat="server" />
                              <asp:HiddenField id="scamount" runat="server" />
                          </div>
                             <br />
                          <div class="form-group1">
                              <b>PB 1</b>
                              <asp:TextBox ID="pb1charge" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                              <asp:HiddenField id="pb1pct" runat="server" />
                              <asp:HiddenField id="pb1amount" runat="server" />
                          </div>
                             
                          <div class="form-group1" style="display:none">
                              <b>Total Tax</b>
                              <asp:TextBox ID="totaltax" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>
                           <br />                             
                          <div class="form-group1">
                              <b>Total</b>
                              <asp:TextBox ID="total" BackColor="LightGreen" ForeColor="Black" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                          </div>
                            <br />
                          <div class="form-group1">
                              <b>Flat Discount</b>
                              <asp:TextBox ID="flatdiscount" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>
                             <br />
                          <div class="form-group1">
                              <b>Amount Paid</b>
                              <asp:TextBox ID="amountpaid" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>
                            <br />
                          <div class="form-group1">
                              <b>Deposit</b>
                              <asp:TextBox ID="deposit" runat="server" onchange="updatepricing()" CssClass="form-control"></asp:TextBox>
                          </div>
                          <div class="form-group1" style="display:none">
                              <b>RoundOff</b>
                              <asp:TextBox ID="roundoff" runat="server" onchange="updatepricing()" CssClass="form-control" Enabled="false"></asp:TextBox>
                          </div>
                            <br />
                           <div class="form-group1">
                              <b>Balance</b>
                              <asp:TextBox ID="balance" BackColor="LimeGreen" ForeColor="Black" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                          </div>

						</div>
					</div>
					<div class="clearfix"> </div>
                    </div>
    </div>

 	<div class="grid-form">
 		<div class="grid-form1">
            
        <div class="vali-form">
            <div class="col-md-4 form-group1">
             <h4 class="title">Season & Rate</h4>
            <label for="ratetype">Rate Type</label>
            <asp:DropDownList ID="ratetype" runat="server" CssClass="form-control" OnSelectedIndexChanged="ratetype_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>

            </div>
            <div class="col-md-4 form-group1">
                <label for="usetax">Use Tax</label>
                    <asp:CheckBoxList ID="usetax" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="usetax_SelectedIndexChanged">
                        <asp:ListItem Text="tax1" Value="Tax1">Service Charges %</asp:ListItem>
                        <asp:ListItem Text="tax2" Value="Tax2">PB 1 %</asp:ListItem>
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

    <asp:Button ID="btncancel" runat="server" Text="Close" CssClass="btn-warning btn" OnClientClick="window.close();"  /> 

     <asp:Button ID="btncheckin" Visible="false" runat="server" Text="Check - In" CssClass="btn-system btn" OnClientClick="if( validationCheckin() ) { return true; } return false;" OnClick="btncheckin_Click"  />
     <asp:Button ID="btncancelcheckin" Visible="false" runat="server" Text="Cancel Check - in" CssClass="btn-info btn" OnClientClick="if( validationCancel() ) { return true; } return false;" OnClick="btncancel_Click"  />
     <asp:Button ID="btncheckout" Visible="false" runat="server" Text="Check - Out" CssClass="btn-toolbar btn" OnClientClick="if( validationCheckout() ) { return true; } return false;" OnClick="btncheckout_Click"  />
     <asp:Button ID="btnnoshow" Visible="false" runat="server" Text="No Show" CssClass="btn-dark btn" OnClientClick="if( validationNoShow() ) { return true; } return false;" OnClick="btnnoshow_Click"  />
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
