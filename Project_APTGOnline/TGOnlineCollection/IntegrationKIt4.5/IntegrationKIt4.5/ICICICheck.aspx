<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ICICICheck.aspx.cs" Inherits="IntegrationKIt4._5.ICICICheck" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    
    <form id="form1" runat="server">
    <div>
        <h2>Check ICICI Status against Transaction ID</h2>
       
         OrderNo :<input type="Text" id="OrderNo" name="OrderNo" runat="server" /></br>
        Transaction date in dd-mm-yyyy format :<input type="Text" id="xdate" name="xdate" runat="server" /></br>
           <input type="hidden" id="txtRequestType" name="txtRequestType" runat="server" />
                    <input type="hidden" id="txtMerchantCode" name="txtMerchantCode" runat="server" />
                    <input type="hidden" id="txtMerchantTxnRefNumber" name="txtMerchantTxnRefNumber" runat="server" />
                    <input type="hidden" id="txtITC" name="txtITC" runat="server" />
                    <input type="hidden" id="txtAmount" name="txtAmount" runat="server" />
                    <input type="hidden" id="txtCurrencyCode" name="txtCurrencyCode" runat="server" />
                    <input type="hidden" id="txtUniqueCustomerId" name="txtUniqueCustomerId" runat="server" />
                    <input type="hidden" id="txtReturnURL" name="txtReturnURL" runat="server" />
                    <input type="hidden" id="txtS2SReturnURL" name="txtS2SReturnURL" runat="server" />
                    <input type="hidden" id="txtTPSLTxnID" name="txtTPSLTxnID" runat="server" />
                    <input type="hidden" id="txtShoppingCartDetails" name="txtShoppingCartDetails" runat="server" />
                    <input type="hidden" id="txttxnDate" name="txttxnDate" runat="server" />
                    <input type="hidden" id="txtEmailIcici" name="txtEmailIcici" runat="server" />
                    <input type="hidden" id="txtMobileNumber" name="txtMobileNumber" runat="server" />
                    <input type="hidden" id="txtBankCode" name="txtBankCode" runat="server" />
                    <input type="hidden" id="txtCustomerName" name="txtCustomerName" runat="server" />
                    <input type="hidden" id="txtCardID" name="txtCardID" runat="server" />
                    <input type="hidden" id="txtKey" name="txtKey" runat="server" />
                    <input type="hidden" id="txtIv" name="txtIv" runat="server" />
                    <input type="hidden" id="txtLAPLReturnurl" name="txtLAPLReturnurl" value="https://hsrpts.com/hsrponline/LAPLResponseHandler.aspx" runat="server" />

    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />

         <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
         <asp:Label ID="lblSuccess"  runat="server" Text=""></asp:Label>
        <table>
             
            <tr><td colspan="2"> <asp:Label ID="RawMessage"  runat="server" Text=""></asp:Label></td></tr>
         
        </table>
    </div>
    </form>
</body>
</html>
