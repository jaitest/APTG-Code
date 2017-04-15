<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="aponline.aspx.cs" Inherits="IntegrationKIt4._5.aponline" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
.btn {
  background: #fc7805;
  background-image: -webkit-linear-gradient(top, #fc7805, #fc7805);
  background-image: -moz-linear-gradient(top, #fc7805, #fc7805);
  background-image: -ms-linear-gradient(top, #fc7805, #fc7805);
  background-image: -o-linear-gradient(top, #fc7805, #fc7805);
  background-image: linear-gradient(to bottom, #fc7805, #fc7805);
  -webkit-border-radius: 11;
  -moz-border-radius: 11;
  border-radius: 11px;
  -webkit-box-shadow: 2px 3px 4px #666666;
  -moz-box-shadow: 2px 3px 4px #666666;
  box-shadow: 2px 3px 4px #666666;
  font-family: Arial;
  color: #ffffff;
  font-size: 20px;
  padding: 6px 21px 6px 21px;
  text-decoration: none;
}

.btn:hover {
  background: #f25811;
  background-image: -webkit-linear-gradient(top, #f25811, #f25811);
  background-image: -moz-linear-gradient(top, #f25811, #f25811);
  background-image: -ms-linear-gradient(top, #f25811, #f25811);
  background-image: -o-linear-gradient(top, #f25811, #f25811);
  background-image: linear-gradient(to bottom, #f25811, #f25811);
  text-decoration: none;
}
    </style>

  
    <title>Andhra Pradesh HSRP</title>
    <script lang="javascript" src="js/header.js"></script>
</head>
<body>
    <script lang="javascript">header();</script>
    <div id="loader" style="display:none;">

        <img src="images/loader.gif" />
    </div>
    <form id="TT"  runat="server">
        
        <table width="1100" border="0" align="center" cellpadding="0" cellspacing="0">
             
            <tr>
                <td class="maininnertextheading">Authorization No: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblAuthNo" runat="server" Text=""></asp:Label></td>
                <td class="maininnertextheading" nowrap>Authorization Date: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblAuthDate" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="maininnertextheading">Registration No: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblRegNo" runat="server" Text=""></asp:Label></td>
                <td class="maininnertextheading">RTO Code/Name: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblRTOLocationCode" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblRTOName" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="maininnertextheading">Owner Name: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblOwnerName" runat="server" Text=""></asp:Label></td>
                <td class="maininnertextheading">Owner Email: </td>
                <td class="maininnertextheading">
                    <asp:TextBox ID="txtEmail" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="maininnertextheading">Address: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label></td>
                 <td class="maininnertextheading">City: <asp:Label ID="lblCity" runat="server" Text=""></asp:Label><asp:Label ID="lblPin" runat="server" Text=""></asp:Label></td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblstate" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="maininnertextheading">Vehicle Type: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblVehicleType" runat="server" Text=""></asp:Label></td>
                <td class="maininnertextheading">Transaction Type: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblTransactionType" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="maininnertextheading">Vehicle Class: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblVehicleClassType" runat="server" Text=""></asp:Label></td>
                <td class="maininnertextheading">Mobile No: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblMobileNo" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="maininnertextheading" nowrap>Manufacturer Name: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblMfgName" runat="server" Text=""></asp:Label></td>
                <td class="maininnertextheading">Model Name: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblModelName" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="maininnertextheading">Engine No: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblEngineNo" runat="server" Text=""></asp:Label></td>
                <td class="maininnertextheading">Chassis No: </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblChasisNo" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="maininnertextheading">Amount :</td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblAmount" runat="server" Text=""></asp:Label></td>
               
                <td class="maininnertextheading">Affixation Center Address  :<span style="color: Red">*</span> </td>
                <td class="maininnertextheading"> <asp:Label ID="lblAffixName" runat="server" Text=""></asp:Label>
                  </td>
            </tr>
             
            <asp:HiddenField ID="hdnDlrName" runat="server"  />
            <asp:HiddenField ID="hdnDlrAddress" runat="server"  />
            <asp:HiddenField ID="hdnDlrContactNO" runat="server"  />
             <asp:HiddenField ID="hdnReferenceURL" runat="server"  />
            <asp:HiddenField ID="hdnDealerId" runat="server" />
            
        
            <tr>
                <td colspan="2" class="maininnertextheading">

                    <asp:Button ID="btnSave" runat="server" Text="Confirm Data &amp; Submit" OnClick="btnSave_Click"
                        CssClass="btn" Width="245px" /></td>
               
                <td colspan="2" class="maininnertextheading">
                    <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                    <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="4" class="maininnertextheading">
                    <h3>Please Note :</h3>
                    <b>1.</b>Orders with valid authorization Numbers from Transport Department only can be booked.<br/>
                    <b>2.</b>Your order will be processed only upon receipt of payments.<br/>
                    <b>3. Ensure to provide the contact number &amp; mail id of the vehicle owner.</b><br />
                    <b>4. Payment Support Mail ID :</b><b style="color: #0000FF">online@hsrpap.com</b><br />
                    <b>5. Customer Care Number :</b><b style="color: #0000FF">9100026696,9100026691,7306139243,7306698827,040-69999712/040-69999713/040-69999772/040-69999773</b> <br />
                </td>

            </tr>

        </table>
        
        </form>
    <script language="javascript">footer();</script>
</body>
</html>
