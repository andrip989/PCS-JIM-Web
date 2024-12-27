﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signin.aspx.cs" Inherits="PCS_JIM_Web.Module.signin" %>

<!DOCTYPE html>
<html>
<head>
<title><%=this.ApplicationTitle() %> - <%=this.ApplicationLokasi() %></title>
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="keywords" content="Pooled Responsive web template, Bootstrap Web Templates, Flat Web Templates, Android Compatible web template, 
Smartphone Compatible web template, free webdesigns for Nokia, Samsung, LG, SonyEricsson, Motorola web design" />
<script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); } </script>
<!-- Bootstrap Core CSS -->
<link rel="shortcut icon" href="../images/logoIFD.gif" type="image/x-icon "/>

<link href="../css/bootstrap.min.css" rel='stylesheet' type='text/css' />
<!-- Custom CSS -->
<link href="../css/style.css" rel='stylesheet' type='text/css' />
<link rel="stylesheet" href="../css/morris.css" type="text/css"/>
<!-- Graph CSS -->
<link href="../css/font-awesome.css" rel="stylesheet">
<link rel="stylesheet" href="../css/jquery-ui.css"> 
<!-- jQuery -->
<script src="~/js/jquery-2.1.4.min.js"></script>
<!-- //jQuery -->
<link href='//fonts.googleapis.com/css?family=Roboto:700,500,300,100italic,100,400' rel='stylesheet' type='text/css'/>
<link href='//fonts.googleapis.com/css?family=Montserrat:400,700' rel='stylesheet' type='text/css'>
<!-- lined-icons -->
<link rel="stylesheet" href="../css/icon-font.min.css" type='text/css' />
<!-- //lined-icons -->
</head> 
<body>
	<div class="main-wthree">
	<div class="container">		
	<div class="sin-w3-agile">
		<h2><b><%=this.ApplicationTitle() %></b><br />
			Sign In</h2>
		<form id="form1" runat="server">
			<div class="username">
				<span class="username">Username:</span>
				<input type="text" name="name" runat="server" id="txtUserName" class="name" placeholder="" required="">
				<div class="clearfix"></div>
			</div>
			<div class="password-agileits">
				<span class="username">Password:</span>
				<input type="password" name="password" runat="server" id="txtPassword" class="password" placeholder="" required="">
				<div class="clearfix"></div>
			</div>
			<div class="rem-for-agile">
				<input type="checkbox" name="remember" runat="server" id="chkRememberMe" class="remember">Remember me<br>
			</div>
			<div class="login-w3">
					<input type="submit" class="login" value="Sign In">
			</div>
			<div class="clearfix"></div>
			<div class="grid_3 grid_5 w3ls">
				<asp:PlaceHolder runat="server" ID="errmsg"></asp:PlaceHolder>
			</div>
		</form>
				
				<div class="footer">
					 <p>© 2024 <%=this.ApplicationTitle() %></p>
				</div>
	</div>
	</div>
	</div>
</body>
</html>
