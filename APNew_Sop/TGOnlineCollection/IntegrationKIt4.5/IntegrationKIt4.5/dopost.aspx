<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dopost.aspx.cs" Inherits="IntegrationKIt4._5.dopost" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       <style type="text/css">
	table.tableizer-table {
	border: 1px solid #CCC; font-family: Arial, Helvetica, sans-serif;
	font-size: 12px;
} 
.tableizer-table td {
	padding: 4px;
	margin: 3px;
	border: 1px solid #ccc;
}
.tableizer-table th {
	background-color: #104E8B; 
	color: #FFF;
	font-weight: bold;
}
.tb4 {
	background-image: url(images/bg.png);
	border: 1px solid #6297BC;
	width: 230px;
}
div#page_loader {
  position: absolute;
  top: 0;
  bottom: 0%;
  left: 0;
  right: 0%;
  background-color: white;
  z-index: 99;
}
</style>
  <script>
      function ShowBussy()
      {
          $("#loader").show();
      }


  </script>
</head>
<body>
    <form id="form1" method="post" action="tgonline.aspx" runat="server">
           <div id="loader" style="display:none;">

        <img src="images/loader.gif" />
    </div>
    <div>
        <table class="tableizer-table">
     
<tr class="tableizer-firstrow"><th></th><th>&nbsp;</th></tr>
 <tr><td>Authorization Number</td><td>  <asp:TextBox ID="AuthNo" runat="server"></asp:TextBox></td></tr>
 <tr><td>Chassis Number</td><td> <asp:TextBox ID="ChassisNo"  runat="server"></asp:TextBox></td></tr>
 <tr><td>Dealer Name</td><td>  <asp:TextBox ID="DealerName" Text="TextName" runat="server"></asp:TextBox></td></tr>
 <tr><td>DealerId</td><td>  <asp:TextBox ID="DealerId" Text="11" runat="server"></asp:TextBox></td></tr>
 <tr><td>Dealer Address</td><td>        <asp:TextBox ID="DealerAddress" Text="Test Address" runat="server"></asp:TextBox>      </td></tr>
 <tr><td>Dealer Contact Number</td><td> <asp:TextBox ID="DealerContactNo" Text="9810509118" runat="server"></asp:TextBox></td></tr>
 <tr><td>Return URL</td><td><asp:TextBox ID="ReturnURL" Text="https://www.hsrpts.com/hsrponline/returnurl.aspx" runat="server"></asp:TextBox></td></tr>
     <tr><td>Security Key</td><td> <asp:TextBox ID="SecurityKey" Text="!@FHY^#@2fl86hb3$a1v9s3h5*A" ReadOnly="true" runat="server"></asp:TextBox></td></tr>
         <tr><td></td><td> <asp:button ID="BtnSubmit" runat="server" Text="Submit"></asp:button></td></tr>
</table>
    </div>
     
    </form>
</body>
</html>
