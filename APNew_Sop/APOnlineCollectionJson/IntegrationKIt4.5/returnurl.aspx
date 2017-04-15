<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" EnableViewStateMac="false" CodeBehind="returnurl.aspx.cs" Inherits="IntegrationKIt4._5.returnurl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="tableizer-table">

                <tr class="tableizer-firstrow">
                    <th></th>
                    <th>&nbsp;</th>
                </tr>
                <tr>
                    <td>Authorization Number</td>
                    <td>
                        <asp:Label ID="AuthNo" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>Chassis Number</td>
                    <td>
                        <asp:Label ID="ChassisNo" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>Dealer Name</td>
                    <td>
                        <asp:Label ID="DealerName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>DealerId</td>
                    <td>
                        <asp:Label ID="DealerId" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>Dealer Address</td>
                    <td>
                        <asp:Label ID="DealerAddress" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Dealer Contact No</td>
                    <td>
                        <asp:Label ID="DealerContactNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>HSRP Amount</td>
                    <td>
                        <asp:Label ID="HSRPAmnt" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Payment Date</td>
                    <td>
                        <asp:Label ID="PymntDt" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>payment Status</td>
                    <td>
                        <asp:Label ID="PymntFlag" runat="server"></asp:Label>
                    </td>
                </tr>
                 <tr>
                    <td>payment Error Message(Only is case of Payment Status 'No')</td>
                    <td>
                        <asp:Label ID="PymntErrorMsg" runat="server"></asp:Label>
                    </td>
                </tr>
                  <tr>
                    <td>payment OrderID</td>
                    <td>
                        <asp:Label ID="PymntOrderNo" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
