<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="myprofile.aspx.cs" Inherits="PCS_JIM_Web.Module.myprofile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
		</style>

    <script language="javascript">

        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=btnUpload.ClientID %>").click();
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Setup<i class="fa fa-angle-right"></i> My Profile</li>
            </ol>
    
    <div class="grid-form1">
<div class="form-horizontal">

     <div class="image-upload">
		<label for="imageprofile[]">
			<asp:Image ID="imagemyprofile" runat="server" />
            <asp:FileUpload ID="FileUpload1" runat="server" /> 
                                            
		</label>
		<asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="btnUpload_Click" Style="display: none" />
	</div>

	<hr/>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate>   

  <div class="form-group">
    <label for="username" class="col-sm-2 control-label hor-form">Username</label>
    <div class="col-sm-10">
        <asp:TextBox ID="username" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        <asp:RequiredFieldValidator ValidationGroup='valGroup1' ID="deviceInput" runat="server" ControlToValidate="username" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
  </div>

  <div class="form-group">
    <label for="fullname" class="col-sm-2 control-label hor-form">Password</label>
    <div class="col-sm-3">
        <asp:TextBox ID="password" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ValidationGroup='valGroup1' ID="RequiredFieldValidator1" runat="server" ControlToValidate="password" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
      
  </div>

  <div class="form-group">
    <label for="fullname" class="col-sm-2 control-label hor-form">Full name</label>
    <div class="col-sm-10">
        <asp:TextBox ID="fullname" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
  </div>

    <div class="form-group">
    <label for="email" class="col-sm-2 control-label hor-form">Email</label>
    <div class="col-sm-10">
        <asp:TextBox ID="email" TextMode="Email" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
  </div>

  <div class="form-group">
    <div class="col-sm-offset-2 col-sm-10">
      <asp:Button ID="submit" runat="server" ValidationGroup='valGroup1' Text="Update" CssClass="btn-success btn" OnClick="submit_Click"  />
    </div>
  </div>
      </ContentTemplate>
	</asp:UpdatePanel>


</div>
</div>

  
</asp:Content>
