<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="custcreate.aspx.cs" Inherits="PCS_JIM_Web.Module.Submodule.custcreate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=this.ApplicationTitle() %> - <%=this.ApplicationLokasi() %></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="Pooled Responsive web template, Bootstrap Web Templates, Flat Web Templates, Android Compatible web template, 
    Smartphone Compatible web template, free webdesigns for Nokia, Samsung, LG, SonyEricsson, Motorola web design" />
    <script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); } </script>

    <script type="text/javascript" src="<%= ResolveUrl("~/js/global.js") %>"></script>    
	<script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-2.1.4.min.js") %>"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-ui.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/Webcam.js") %>"></script>

	<link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/css/jquery-ui.css") %>" />
    <link rel="stylesheet" type='text/css' href="<%= ResolveUrl("~/css/icon-font.min.css") %>" />

    <link rel='stylesheet' type='text/css' href="<%= ResolveUrl("~/css/bootstrap.min.css") %>" />
    <link rel='stylesheet' type='text/css' href="<%= ResolveUrl("~/css/style.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/css/morris.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/css/font-awesome.css") %>" >
    
    <!-- //jQuery -->
    <link href='//fonts.googleapis.com/css?family=Roboto:700,500,300,100italic,100,400' rel='stylesheet' type='text/css'/>
    <link href='//fonts.googleapis.com/css?family=Montserrat:400,700' rel='stylesheet' type='text/css' />
    <!-- lined-icons -->
     <style type="text/css">
        .Initial
	    {
		    display: block;
		    padding: 4px 18px 4px 18px;
		    float: left;
		    background-color: mistyrose;
            background-image: url('../../images/tab3.png');
            background-repeat: no-repeat;
            background-size: 100% 100%;
            width: 180px;
            height: 40px;
		    color: Black;
		    font-weight: bold;
	    }
	    .Initial:hover
	    {
		    color: darkblue;
		    background-color: powderblue;
            background-image: url('../../images/tab2.png');
            background-repeat: no-repeat;
            background-size: 100% 100%;
            width: 180px;
            height: 40px;
	    }
	    .Clicked
	    {
            background-image: url('../../images/tab1.png');
            background-repeat: no-repeat;
            background-size: 100% 100%;
            width: 180px;
            height: 40px;
		    float: left;
		    display: block;
		    background-color: #1b93e1;
		    padding: 4px 18px 4px 18px;
		    color: Black;
		    font-weight: bold;
	    }
    </style>

    <script type="text/javascript">
        //let queryString = new URLSearchParams(window.location.search);

        if (true)//!queryString.has('custcodeparam'))
        {

            $(function () {
                 
                var objimage = $("#imagemyprofile").attr("src");
                //alert(objimage);
                if (objimage == null) {
                    let el = document.getElementById('webcam');

                    if (el != null) {
                        Webcam.set({
                            width: 400,
                            height: 300,
                            image_format: 'jpeg',
                            jpeg_quality: 95,
                            constraints: { facingMode: 'environment' }
                        });
                        Webcam.attach('#webcam');
                    }
                }                

                $("#snapbtn").click(function () {
                    Webcam.snap(function (data_uri) {
                        $.ajax({
                            type: "POST",
                            url: '<%=ResolveUrl("~/Module/Submodule/custcreate.aspx/SaveCapturedImage") %>',
                            data: "{data: '" + data_uri + "' , tipe : '0'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (r) {
                                if (r.hasOwnProperty("d")) {                                    

                                    document.getElementById("<%=btnUpload.ClientID %>").click();
                                    //$("#imagemyprofile").attr("src", r.d);
                                    //$('#imagemyprofile').removeAttr('display');
                                    //$("#imagemyprofile").css('display', 'block');
                                }

                            }
                        });
                    });
                });

                var objimage2 = $("#ktpprofile").attr("src");
                //alert(objimage);
                if (objimage2 == null) {
                    let el2 = document.getElementById('webcam2');

                    if (el2 != null) {
                        Webcam.set({
                            width: 400,
                            height: 300,
                            image_format: 'jpeg',
                            jpeg_quality: 95,
                            constraints: { facingMode: 'environment' }
                        });
                        Webcam.attach('#webcam2');
                    }
                }     

                $("#snapbtn2").click(function () {
                    Webcam.snap(function (data_uri) {
                        $.ajax({
                            type: "POST",
                            url: '<%=ResolveUrl("~/Module/Submodule/custcreate.aspx/SaveCapturedImage") %>',
                            data: "{data: '" + data_uri + "', tipe : '1'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (r) {
                                if (r.hasOwnProperty("d")) {
                                    //alert(r.d);

                                    document.getElementById("<%=btnuploadktp.ClientID %>").click();
                                    //$("#imagemyprofile").attr("src", r.d);
                                    //$('#imagemyprofile').removeAttr('display');
                                    //$("#imagemyprofile").css('display', 'block');
                                }

                            }
                        });
                    });
                });

             });
            

            function webCameraoff() {
                Webcam.reset();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        function validationSave() {
            var roomno = document.getElementById('<%=custcodeparam.ClientID%>').value;

            if (roomno == "") {
                var x = window.confirm("Are you sure want to Create new guest list ?")
                if (x) {
                    return true;
                } else {
                    return false;
                }
            }
            else {
                var x = window.confirm("Are you sure want to update cust. code " + roomno + "?")
                if (x) {
                    return true;
                } else {
                    return false;
                }
            }
        }

    </script>


    <style>
			.image-upload > input
			{
			display: none;
			}

			.image-upload img
			{
			width: 80px;
			cursor: pointer;
			}

            .FormatRadioButtonList label
            {
                margin-right: 15px;
                margin-left: 5px;
                font-weight : normal;
            }
		</style>

    <script language="javascript">

        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=btnUpload.ClientID %>").click();
            }
        }

        function UploadFile2(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=btnuploadktp.ClientID %>").click();
            }
        }

    </script>
</head>
<body onBlur="self.focus();">
   <!--/content-inner-->
              <!--header start here-->
				
<!--heder end here-->
	<ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="<%= ResolveUrl("default.aspx") %>">Home</a><i class="fa fa-angle-right"></i>Forms <i class="fa fa-angle-right"></i> Create New Guest</li>
            </ol>
		<!--grid-->

 	
 		<form id="form1" runat="server">
 		<div class="grid-form1">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server"></asp:ScriptManager>
          <asp:Button Text="Overview" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
              OnClick="Tab1_Click" />
          <asp:Button Text="Personal" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
              OnClick="Tab2_Click" />
          <asp:Button Text="Address Information" BorderStyle="None" ID="Tab3" CssClass="Initial" runat="server"
              OnClick="Tab3_Click" />
          <asp:Button Text="Other Information" BorderStyle="None" ID="Tab4" CssClass="Initial" runat="server"
              OnClick="Tab4_Click" />
           <asp:Button Text="Other Option" BorderStyle="None" ID="Tab5" CssClass="Initial" runat="server"
              OnClick="Tab5_Click" />
          
            
  <asp:MultiView ID="MainView" runat="server" >
     <asp:View ID="View1" runat="server">
         <br />
         <br />
           <asp:HiddenField ID="custcodeparam" runat="server" />
         <div class="title margin-top">
			<h3 class="page-header icon-subheading">Guest Information</h3>
		</div>
		<div class="box_content">
          <div class="col-md-4 form-group1">
            <label for="noroom">Cust. Code</label>
            <asp:TextBox ID="custcode" runat="server" Enabled="false"></asp:TextBox> 
              
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup='valGroup4' 
                runat="server" ControlToValidate="custcode" 
                BackColor="Red" 
                ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
              <asp:Button id="generatecodebtn" runat="server" Text="Generate Cust.Code" onclick="generatecode_Click" class="btn btn-lg btn-primary btn-block"></asp:Button>

          </div>
          <div class="col-md-4 form-group1">
              <div class="image-upload">
		            <label for="imageprofile[]"> Profile
			            <asp:Image ID="imagemyprofile" runat="server" Width="400" Height="300" />
                        <asp:FileUpload ID="FileUpload1" runat="server" accept="image/*;capture=camera" /> 
                        <!--<div id="webcam" runat="server" visible="false"></div>-->
                                            
		            </label>
		            <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="btnUpload_Click" Style="display: none" />                  
	            </div>
                  <!--
                <asp:ImageButton id="snapbtn" runat="server" ImageUrl="~/images/captureres.png" Visible="false" />
                <asp:ImageButton id="gambarprofile" runat="server" ImageUrl="~/images/retakeres.png" OnClick="retakecapture_Click" /> -->
          </div>
            <div class="col-md-4 form-group1">
              <div class="image-upload">
		            <label for="ktpprofile[]"> KTP
			            <asp:Image ID="ktpprofile" runat="server" Width="400" Height="300"/>
                        <asp:FileUpload ID="FileUpload2" runat="server" accept="image/*;capture=camera" /> 
                        <!--<div id="webcam2" runat="server" visible="false"></div>   -->                 
		            </label>
		            <asp:Button ID="btnuploadktp" Text="Upload" runat="server" OnClick="btnUpload_Click" Style="display: none" />
	            </div>
                  <!--
                <asp:ImageButton id="snapbtn2" runat="server" ImageUrl="~/images/captureres.png" Visible="false" />
                <asp:ImageButton id="gambarktp" runat="server" ImageUrl="~/images/retakeres.png" OnClick="retakecapture_Click"  />

                <asp:CustomValidator ID="CustomValidator1" ValidationGroup="valGroup3" runat="server" ErrorMessage="CustomValidator" OnServerValidate="CustomValidator1_ServerValidate">                    
                </asp:CustomValidator>
                   -->
          </div>

            <div class="clearfix"> </div>
         
          <div class="vali-form">
          <div class="col-md-4 form-group1">
              <label for="firstname">First Name</label>
              <asp:TextBox ID="firstname" runat="server"></asp:TextBox>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup='valGroup4' 
                runat="server" ControlToValidate="firstname" 
                BackColor="Red" 
                ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
          </div>

           <div class="col-md-4 form-group1">
              <label for="lastname">Last Name</label>
              <asp:TextBox ID="lastname" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          </div>
          <div class="clearfix"> </div>

          <div class="col-md-4 form-group1">
            <label for="email">Email</label>
            <asp:TextBox ID="email" TextMode="Email" runat="server" CssClass="form-control"></asp:TextBox>  
          </div>
          <div class="clearfix"> </div>

         <div class="col-md-4 form-group1">
            <label for="identificationtype">Tipe Identitas</label>
             <asp:DropDownList ID="identificationtype" runat="server" CssClass="form-control">
                        <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                        </asp:ListItem>
                </asp:DropDownList>
             <!--
              <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup='valGroup3' 
                runat="server" ControlToValidate="identificationtype" 
                BackColor="Red" 
                ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
             -->
          </div>
          <div class="clearfix"> </div>

          <div class="vali-form">
          <div class="col-md-4 form-group1">
            <label for="identificationid">No Identitas</label>
            <asp:TextBox ID="identificationid" runat="server" CssClass="form-control"></asp:TextBox>   
              <!--
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup='valGroup3' 
                runat="server" ControlToValidate="identificationid" 
                BackColor="Red" 
                ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
              -->
          </div>
           <div class="col-md-4 form-group1">
              <label for="npwp">Fiscal Code</label>
              <asp:TextBox ID="npwp" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          </div>
          <div class="clearfix"> </div>

         <div class="col-md-4 form-group1">
            <label for="identificationexpdate">Exp.Date</label>
             <asp:TextBox ID="identificationexpdate" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          <div class="clearfix"> </div>

          <div class="vali-form">
          <div class="col-md-4 form-group1">
            <label for="phone">Handphone</label>
            <asp:TextBox ID="phone" TextMode="Phone" runat="server" CssClass="form-control"></asp:TextBox>  
          </div>
        <div class="col-md-4 form-group1">
              <label for="telephone">Telephone</label>
              <asp:TextBox ID="telephone" TextMode="Phone" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
          </div>
          <div class="clearfix"> </div>
        </div>
        <div class="title margin-top">
			<h3 class="page-header icon-subheading">Other Information</h3>
		</div>
		<div class="box_content">
         <div class="col-md-4 form-group1">
            <label for="guesttype">Guest Type</label>
              <asp:DropDownList ID="guesttype" runat="server" CssClass="form-control">
                  <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                  </asp:ListItem>
             </asp:DropDownList>
          </div>
          <div class="clearfix"> </div>

         <div class="col-md-4 form-group1">
            <label for="birthdate">Birth Date</label>
              <asp:TextBox ID="birthdate" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          <div class="clearfix"> </div>

          <div class="col-md-4 form-group1">
            <label for="gender">Gender</label>
              <asp:DropDownList ID="gender" runat="server" CssClass="form-control">
             </asp:DropDownList>
          </div>
          <div class="clearfix"> </div>

         <div class="col-md-4 form-group1">
            <label for="regno">Reg No</label>
              <asp:TextBox ID="regno" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          <div class="clearfix"> </div>
        </div>
    </asp:View>

    <asp:View ID="View2" runat="server">
         <br />
         <br />
        <div class="title margin-top">
			<h3 class="page-header icon-subheading">Marital Information</h3>
		</div>
		<div class="box_content">
        <div class="col-md-4 form-group1">
            <label for="maritalstatus">Marital Status</label>
              <asp:DropDownList ID="maritalstatus" runat="server" CssClass="form-control">
                  <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                  </asp:ListItem>
             </asp:DropDownList>
          </div>
          <div class="clearfix"> </div>
          <div class="col-md-4 form-group1">
            <label for="anniversarydate">Anniversary Date</label>
              <asp:TextBox ID="anniversarydate" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          <div class="clearfix"> </div>
        <div class="col-md-4 form-group1">
              <label for="children">Children</label>
              <asp:TextBox ID="children" TextMode="Number" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          <div class="clearfix"> </div>
         <div class="col-md-4 form-group1">
            <label for="spousebirthdate">Spouse Birth Date</label>
              <asp:TextBox ID="spousebirthdate" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          <div class="clearfix"> </div>
         </div>
        <div class="title margin-top">
			<h3 class="page-header icon-subheading">Profesional Information</h3>
		</div>
		<div class="box_content">
              <div class="col-md-4 form-group1">
                <label for="education">Education</label>
                  <asp:TextBox ID="education" runat="server" CssClass="form-control"></asp:TextBox>
              </div>
              <div class="clearfix"> </div>
              <div class="col-md-4 form-group1">
                <label for="occupation">Occupation</label>
                  <asp:TextBox ID="occupation" runat="server" CssClass="form-control"></asp:TextBox>
              </div>
              <div class="clearfix"> </div>
              <div class="col-md-4 form-group1">
                <label for="income">Income</label>
                  <asp:TextBox ID="income" runat="server" CssClass="form-control"></asp:TextBox>
              </div>
              <div class="clearfix"> </div>
        </div>

        <div class="title margin-top">
			<h3 class="page-header icon-subheading">Regional Information</h3>
		</div>
		<div class="box_content">
         <div class="col-md-8 form-group1">
              <label for="nationality">Nationality</label>
              <asp:TextBox ID="nationality" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="clearfix"> </div>
         <div class="vali-form">
          <div class="col-md-4 form-group1">
            <label for="language1">Language 1</label>
            <asp:TextBox ID="language1" runat="server" CssClass="form-control"></asp:TextBox>   
          </div>
           <div class="col-md-4 form-group1">
              <label for="language2">Language 2</label>
              <asp:TextBox ID="language2" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          </div>
           <div class="clearfix"> </div>
        </div>

        <div class="title margin-top">
			<h3 class="page-header icon-subheading">Health Information</h3>
		</div>
		<div class="box_content">
         <div class="col-md-8 form-group1">
              <label for="allergies">Allergies</label>
              <asp:TextBox ID="allergies" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="clearfix"> </div>
         <div class="col-md-4 form-group1">
            <label for="bloodtype">Blood Type</label>
              <asp:DropDownList ID="bloodtype" runat="server" CssClass="form-control">
                  <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                  </asp:ListItem>
             </asp:DropDownList>
          </div>

           <div class="clearfix"> </div>
        </div>

    </asp:View>


    <asp:View ID="View3" runat="server">
         <br />
         <br />
        <div class="box_content">
        <div class="col-md-8 form-group1">
              <label for="address">Address</label>
              <asp:TextBox ID="address" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
          <div class="clearfix"> </div>
        <div class="col-md-8 form-group1">
              <label for="street">Street</label>
              <asp:TextBox ID="street" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
          <div class="clearfix"> </div>
        <div class="vali-form">
          <div class="col-md-4 form-group1">
            <label for="city">City</label>
            <asp:TextBox ID="city" runat="server" CssClass="form-control"></asp:TextBox>   
          </div>
           <div class="col-md-4 form-group1">
              <label for="state">State</label>
              <asp:TextBox ID="state" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          </div>
        <div class="col-md-8 form-group1">
              <label for="zipcode">Zip Code</label>
              <asp:TextBox ID="zipcode" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
          <div class="clearfix"> </div>
        <div class="col-md-8 form-group1">
              <label for="country">Country</label>
              <asp:TextBox ID="country" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
          <div class="clearfix"> </div>
        </div>
    </asp:View>

    <asp:View ID="View4" runat="server">
          <br />
          <br />
        <div class="title margin-top">
			<h3 class="page-header icon-subheading">Vehicle Information</h3>
		</div>
		<div class="box_content">
          <div class="col-md-4 form-group1">
              <label for="vehiclelicenseplate">Lic. Plate</label>
              <asp:TextBox ID="vehiclelicenseplate" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          <div class="clearfix"> </div>
          
          <div class="vali-form">
              <div class="col-md-4 form-group1">
                  <label for="vehiclecompany">Company Car</label>
                  <asp:TextBox ID="vehiclecompany" runat="server" CssClass="form-control"></asp:TextBox>
              </div>
              <div class="col-md-4 form-group1">
                  <label for="vehiclemodel">Model Car</label>
                  <asp:TextBox ID="vehiclemodel" runat="server" CssClass="form-control"></asp:TextBox>
              </div>
           </div>
          <div class="clearfix"> </div>

          <div class="vali-form">
              <div class="col-md-4 form-group1">
                  <label for="vehiclecolor">Color Car</label>
                  <asp:TextBox ID="vehiclecolor" runat="server" CssClass="form-control"></asp:TextBox>
              </div>          
              <div class="col-md-4 form-group1">
                  <label for="vehicleyear">Year Car</label>
                  <asp:TextBox ID="vehicleyear" TextMode="Number" runat="server" CssClass="form-control"></asp:TextBox>
              </div>
          </div>
          <div class="clearfix"> </div>
        </div>
        
        <div class="title margin-top">
			<h3 class="page-header icon-subheading">Communication Channel</h3>
		</div>
		<div class="box_content">
            <div class="col-md-8 form-group1">
              <label for="officeadd">Office add</label>
              <asp:TextBox ID="officeadd" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
              <div class="clearfix"> </div>
              <div class="col-md-8 form-group1">
              <label for="office">Office #</label>
              <asp:TextBox ID="office" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                  <div class="clearfix"> </div>
            <div class="col-md-8 form-group1">
              <label for="residential">Residential #</label>
              <asp:TextBox ID="residential" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                  <div class="clearfix"> </div>

          <div class="vali-form">
          <div class="col-md-4 form-group1">
              <label for="fax">Fax</label>
              <asp:TextBox ID="fax" runat="server" CssClass="form-control"></asp:TextBox>
          </div>

           <div class="col-md-4 form-group1">
              <label for="website">Website</label>
              <asp:TextBox ID="website" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          </div>
          <div class="clearfix"> </div>

           <div class="vali-form">
          <div class="col-md-4 form-group1">
              <label for="followup">Follow Up</label>
             <asp:DropDownList ID="followup" runat="server" CssClass="form-control">
                  <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                  </asp:ListItem>
             </asp:DropDownList>
          </div>

           <div class="col-md-4 form-group1">
              <label for="heardfrom">Heard From</label>
                 <asp:DropDownList ID="heardfrom" runat="server" CssClass="form-control">
                  <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                  </asp:ListItem>
             </asp:DropDownList>
          </div>
          </div>
          <div class="clearfix"> </div>

            <div class="col-md-4 form-group1">
              <label for="denial">Denial</label>
               <asp:DropDownList ID="denial" runat="server" CssClass="form-control">
                  <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                  </asp:ListItem>
             </asp:DropDownList>
          </div>
          </div>
          <div class="clearfix"> </div>

        
    </asp:View>

    <asp:View ID="View5" runat="server">
         <br />
         <br />
        <div class="title margin-top">
			<h3 class="page-header icon-subheading">Credit Limit</h3>
		</div>
		<div class="box_content">
          <div class="col-md-4 form-group1">
              <label for="statusblock">Status</label>
              <asp:DropDownList ID="statusblock" runat="server" CssClass="form-control"></asp:DropDownList>
          </div>
          <div class="clearfix"> </div>

            <div class="col-md-4 form-group1">
              <label for="creditlimit">Credit Limit</label>
               <asp:TextBox ID="creditlimit" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
          <div class="clearfix"> </div>


          <div class="vali-form">
          <div class="col-md-4 form-group1">
              <label for="createddate">Created Date</label>
              <asp:TextBox ID="createddate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
          </div>

           <div class="col-md-4 form-group1">
              <label for="createdby">Created By</label>
              <asp:TextBox ID="createdby" runat="server" CssClass="form-control"  ReadOnly="true"></asp:TextBox>
          </div>
          </div>
          <div class="clearfix"> </div>

          <div class="vali-form">
          <div class="col-md-4 form-group1">
              <label for="updateddate">Updated Date</label>
              <asp:TextBox ID="updateddate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
          </div>

           <div class="col-md-4 form-group1">
              <label for="updatedby">Updated By</label>
              <asp:TextBox ID="updatedby" runat="server" CssClass="form-control"  ReadOnly="true"></asp:TextBox>
          </div>
          </div>
          <div class="clearfix"> </div>
        
        <div class="col-md-8 form-group1">
              <label for="paymenttype">Payment Type</label>
             <asp:DropDownList ID="paymenttype" runat="server" CssClass="form-control">
                <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                    </asp:ListItem>
            </asp:DropDownList>


          </div>
          <div class="clearfix"> </div>

         <div class="col-md-8 form-group1">
              <label for="businesscode">Business Code</label>
             <asp:DropDownList ID="businesscode" runat="server" CssClass="form-control">
                  <asp:ListItem Text="" Enabled="true" Selected="True" Value=''>
                  </asp:ListItem>
             </asp:DropDownList>
        </div>
          <div class="clearfix"> </div>


          <div class="vali-form">
          <div class="col-md-4 form-group1">
              <label for="discountcode">Disc. Code</label>
              <asp:TextBox ID="discountcode" runat="server" CssClass="form-control"></asp:TextBox>
          </div>

           <div class="col-md-4 form-group1">
              <label for="discpct">Disc. %</label>
              <asp:TextBox ID="discpct" runat="server" CssClass="form-control" ></asp:TextBox>
          </div>
          </div>
          <div class="clearfix"> </div>

         <div class="col-md-8 form-group1">
              <label for="creditcardno">Credit Card No</label>
              <asp:TextBox ID="creditcardno" runat="server" CssClass="form-control"></asp:TextBox>
          </div>
        <div class="clearfix"> </div>
        </div>
    </asp:View>



  </asp:MultiView>

    

<hr />
<div class="form-horizontal">  
    <div class="form-group">
        <div class="col-sm-offset-0 col-sm-10">

        <asp:Button ID="submit" ValidationGroup='valGroup4' runat="server" Text="Submit" CssClass="btn-primary btn" OnClick="submit_Click"  />
        <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn-warning btn" OnClientClick="window.close();"  /> 

        <asp:Button ID="btndelete" runat="server" Text="Delete" Visible="false" CssClass="btn-dark btn" OnClick="btndelete_Click" OnClientClick="if( validationDelete() ) { return true; } return false;"  />

        </div>
    </div>
</div>

</div>
</form>

  


  
</body>
</html>
