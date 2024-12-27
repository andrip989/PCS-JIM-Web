<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="deviceid.aspx.cs" Inherits="PCS_JIM_Web.Module.deviceidpage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script type="text/javascript" language="javascript">

    var newWindow = null;

    function openWindowchild()
    {
        if (newWindow && !newWindow.closed)
            newWindow.focus();
        else
        {
            var x = true;//window.confirm("Reload ulang Device?")
            if (x)
            {
                height = screen.height / 2;
                width = screen.width / 2;
                file = "";
                var left = (screen.width / 2) - (width / 2);
                var top = (screen.height / 2) - (height / 2);

                file = 'http://www.google.com';
                newWindow = window.open(file, "addnote", "width=" + width + ",height=" + height + ",top=" + top + ", left=" + left + ",toolbar=no,scrollbars=yes,directories=no,status=no,menubar=no,resizable=no,location=no,modal=yes");		
                newWindow.focus();
                return false;
            } else {
                return false;
            }

        }
    }

    function parent_disable()
    {
        if (newWindow && !newWindow.closed)
            newWindow.focus();
    }

   </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="default.aspx">Home</a><i class="fa fa-angle-right"></i>Setup<i class="fa fa-angle-right"></i> Device Id</li>
            </ol>
       <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
			<ContentTemplate>

    <div class="grid-form1">
<div class="form-horizontal">
    <div class="form-group">
    <label for="ipaddress" class="col-sm-2 control-label hor-form">IP Address Lan</label>
    <div class="col-sm-4">
        <asp:TextBox ID="ipaddress" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ValidationGroup='valGroup2' ID="RequiredFieldValidator1" runat="server" ControlToValidate="ipaddress" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="ipport" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ValidationGroup='valGroup2' ID="RequiredFieldValidator2" runat="server" ControlToValidate="ipport" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
    <div class="col-sm-2">
        <asp:Button id="loaddevicebtn" ValidationGroup='valGroup2' runat="server" Text="Load Device" onclick="LoadDevice_Click" class="btn btn-lg btn-primary btn-block"></asp:Button>
    </div>
  </div>

  <div class="form-group">
    <label for="ipaddresswifi" class="col-sm-2 control-label hor-form">IP Address Wifi</label>
    <div class="col-sm-10">
        <asp:TextBox ID="ipaddresswifi" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
  </div>

  <div class="form-group">
    <label for="deviceid" class="col-sm-2 control-label hor-form">Device Id</label>
    <div class="col-sm-10">
        <asp:TextBox ID="deviceid" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ValidationGroup='valGroup1' ID="deviceInput" runat="server" ControlToValidate="deviceid" BackColor="GreenYellow" ErrorMessage="Please enter a value" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
  </div>

  <div class="form-group">
    <label for="description" class="col-sm-2 control-label hor-form">Description</label>
    <div class="col-sm-10">
        <asp:TextBox ID="description" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:HiddenField ID="recidparam" runat="server" />
    </div>
  </div>

  <div class="form-group">
    <label for="tipeipaddress" class="col-sm-2 control-label hor-form">Tipe Lan / Wifi</label>
    <div class="col-sm-10">
                <asp:DropDownList ID="tipeipaddress" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Lan" Enabled="true" Selected="True" Value='0'>                        
                    </asp:ListItem>
                    <asp:ListItem Text="Wifi" Value='1'>
                    </asp:ListItem>
              </asp:DropDownList>
    </div>
  </div>

 


  <div class="form-group">
    <div class="col-sm-offset-2 col-sm-10">
      <asp:Button ID="submit" ValidationGroup='valGroup1' runat="server" Text="Submit" CssClass="btn-primary btn" OnClick="SaveClick"  />
        <button type="reset" class="btn-default btn">Reset</button>       
        <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn-warning btn" OnClick="btncancel_Click"  />            
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
                                            <ItemTemplate>
                                                <asp:CheckBox id="chk" runat="server" AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                                <asp:HiddenField ID="recchk" runat="server" Value='<%# Eval("recid") %>'  />
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
						    </div>
					</div>				
                </div>
	</ContentTemplate>
	</asp:UpdatePanel>

</asp:Content>
