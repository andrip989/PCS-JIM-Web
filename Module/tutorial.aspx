<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="tutorial.aspx.cs" Inherits="PCS_JIM_Web.Module.tutorial" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <iframe id="iframepdf" class="responsive-iframe" Width="100%" Height="800" src="<%= this.getUserid() %>"> </iframe>
</asp:Content>
