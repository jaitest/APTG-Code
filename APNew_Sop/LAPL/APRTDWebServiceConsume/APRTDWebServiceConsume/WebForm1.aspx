<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="APRTDWebServiceConsume.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" /><br />
        <asp:Button ID="btnConfirmPayment" runat="server" Text="Confirm payment" OnClick="btnConfirmPayment_Click" /><br />
        <asp:Button ID="btnUpdateLaserCodes" runat="server" Text="Update HSRP laser codes" OnClick="btnUpdateLaserCodes_Click" /><br />
        <asp:Button ID="btnHSRPAvailability" runat="server" Text="Notify HSRP availability" OnClick="btnHSRPAvailability_Click" /><br />
        <asp:Button ID="btnConfirmAffixation" runat="server" Text="Confirm affixation" OnClick="btnConfirmAffixation_Click" /><br />
    </div>
    </form>
</body>
</html>
