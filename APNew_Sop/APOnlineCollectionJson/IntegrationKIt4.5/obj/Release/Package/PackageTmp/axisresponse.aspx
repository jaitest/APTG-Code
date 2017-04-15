<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="axisresponse.aspx.cs" Inherits="IntegrationKIt4._5.axisresponse" %>

<!DOCTYPE html>
<html>
<head>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     #overlay {
        position: fixed;
        background-color:white;
        width: 100%;
        height: 100%;
        top: 0;
        left: 0;
    }
    .hide {
        display: none;
    }
    </style>
    <script type="text/javascript" lang="javascript">
        function ValidateForm() {



            if (document.getElementById('txtEmail').value == '') {
                alert('Enter your Email-ID..');
                document.getElementById('txtEmail').focus();
                return false;
            }

            if (document.getElementById('txtMobileNo').value == '') {
                alert('Enter Your Mobile No.');
                document.getElementById('txtMobileNo').focus();
                return false;
            }

            document.getElementById('billing_email').value = document.getElementById('txtEmail').value;
            document.getElementById('billing_tel').value = document.getElementById('txtMobileNo').value

            if (confirm("Confirm to Continue?")) {
                return true;
            }
            else {
                return false;
            }
        }

        function isNumberKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 46 || charCode > 57))
                return false;

            return true;
        }
        $(window).load(function () {

        });

        window.onload = function () {

            // Onload event of Javascript
            // Initializing timer variable
            var x = 20;
            // Display count down for 20s
            setInterval(function () {
                if (x <= 21 && x >= 1) {
                    x--;

                    if (x == 1) {
                        x = 21;
                    }
                }
            }, 1000);
            // Form Submitting after 20s
            var auto_refresh = setInterval(function () {
                submitform();
            }, 5000);
            // Form submit function
            function submitform() {
                $(document).ready(function () {
                    document.getElementById("form1").submit();


                });
            }
        }
    </script>
    <title>Andhra Pradesh HSRP</title>
    <script language="javascript" src="js/header.js"></script>
</head>
<body>
    <script language="javascript">header();</script>
    
    <form id="form1" method="post" runat="server">
        <div>
      <div id="overlay">
          <img style="align-content: center;
    height: 250px;
    width: 350px;
    margin-left: 35%;
    margin-top: 15%;" src="images/loading-x.gif" />
         <div style="font-size:large; align-content: center; margin-left: 340px; color:blue;">
        You are being Redirected to Dealer Application
            

         </div>
          <span style="font-size:large; align-content: center; margin-left: 250px;">
              Do Not "Close the Windows" or Press "Referesh" Or "Browser back/forward button"
          </span>

      </div>
            <table width="1100" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2" class="maininnertextheading">
                        <h2>HSRP Fee Online Payment Confirmation</h2>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="maininnertextheading">
                        <h3>Your Payment Order Status is :
                            <asp:Label ID="lblOrderStatus" runat="server"></asp:Label>,
                            <asp:Label ID="lblOrderStatusMsg" runat="server"></asp:Label>
                        </h3>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="maininnertextheading">
                        <u>Order Details </u>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">HSRP Online Order Number :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="orderNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Authorization Number :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="lblAuthNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Bank Ref. Number :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="lblBankRefNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Online Payment Tracking ID :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="lbltrackingid" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Amount Paid :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="lblAmount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Owner Name :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="lblOwnerName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Owner Address :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">City :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="lblCity" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Pin :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="lblPin" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">State :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="lblState" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Telephone :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="lblTelephone" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Email ID :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="lblEmailID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Affixation Office Address :                       
                    </td>
                    <td class="maininnertextheading">
                        <asp:Label ID="lblAffixation" runat="server"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:HiddenField ID="PymntOrderNo" runat="server" />
                        <asp:HiddenField ID="AuthNo" runat="server" />
                        <asp:HiddenField ID="ChassisNo" runat="server" />
                        <asp:HiddenField ID="DealerName" runat="server" />
                        <asp:HiddenField ID="DealerId" runat="server" />
                        <asp:HiddenField ID="DealerAddress" runat="server" />
                        <asp:HiddenField ID="DealerContactNo" runat="server" />
                        <asp:HiddenField ID="HSRPAmnt" runat="server" />
                        <asp:HiddenField ID="PymntDt" runat="server" />
                        <asp:HiddenField ID="PymntFlag" runat="server" />
                        <asp:HiddenField ID="PymntErrorMsg" runat="server" />
                        
                        <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                        <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label></td>

                </tr>
                <tr>
                    <td class="auto-style1">
                        <input type="button" id="btnPrint" class="btn" value="Print" onclick="javascript: window.print();" />
                        &nbsp; </td>
                </tr>
            </table>
        </div>
    </form>
    <script language="javascript">footer();</script>
    <div class="modal"><!-- Place at bottom of page --></div>

</body>
</html>
