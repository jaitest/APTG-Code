<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AxisReconcile.aspx.cs" Inherits="IntegrationKIt4._5.AxisReconcile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>Order No : <asp:TextBox ID="OrderNo" runat="server"></asp:TextBox><br/>
        Auth No : <asp:TextBox ID="AuthNo" text="A2" runat="server"></asp:TextBox><br/>
        Order Date(yyyy-mm-dd)  : <asp:TextBox ID="OrderDate" runat="server"></asp:TextBox><br/>
              Amt : <asp:TextBox ID="OrderAmt" runat="server"></asp:TextBox><br/>
        <asp:Label ID="ResponseText" runat="server" Text="Label"></asp:Label>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        
    </div>
    </form>
</body>
</html>
