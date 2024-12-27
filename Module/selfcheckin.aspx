<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="selfcheckin.aspx.cs" Inherits="PCS_JIM_Web.Module.selfcheckin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Self Check in</li>
            </ol>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate>
                <div class="grid-form1">
                <div class="form-horizontal">  

                <div class="form-group">
                <label for="noroom" class="col-sm-2 control-label hor-form">No. Room / Booking Code / Trans. No / Cust Code / First Name / Last Name</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="referenceparam" TextMode="MultiLine"  runat="server" CssClass="form-control"></asp:TextBox>

                </div>
                </div>   
                <br />
                <div class="form-group">
                <label for="noroom" class="col-sm-2 control-label hor-form">&nbsp;</label>
                <div class="col-sm-6">
                    <a id="housekeepbtn2" runat="server" onserverclick="checkinbtn_ServerClick"><div class="bg-info pv20 text-white fw600 text-center"><asp:Label runat="server" ID="labelbtn"></asp:Label></div></a> 

                </div>
                </div>   


                </div>
              </div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
