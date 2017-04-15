<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tgonline2.aspx.cs" Inherits="IntegrationKIt4._5.tgonline2" %>

<html>
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

        .btnICICI {
            background: #c81b5d;
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

            .btnICICI:hover {
                background: #f25811;
                background-image: -webkit-linear-gradient(top, #f25811, #f25811);
                background-image: -moz-linear-gradient(top, #f25811, #f25811);
                background-image: -ms-linear-gradient(top, #f25811, #f25811);
                background-image: -o-linear-gradient(top, #f25811, #f25811);
                background-image: linear-gradient(to bottom, #f25811, #f25811);
                text-decoration: none;
            }

        .btnHDFC {
            background: #005593;
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

            .btnHDFC:hover {
                background: #005593;
                background-image: -webkit-linear-gradient(top, #f25811, #f25811);
                background-image: -moz-linear-gradient(top, #f25811, #f25811);
                background-image: -ms-linear-gradient(top, #f25811, #f25811);
                background-image: -o-linear-gradient(top, #f25811, #f25811);
                background-image: linear-gradient(to bottom, #f25811, #f25811);
                text-decoration: none;
            }

        .btnLAPL {
            background: #3498db;
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

            .btnLAPL:hover {
                background: #3cb0fd;
                background-image: -webkit-linear-gradient(top, #f25811, #f25811);
                background-image: -moz-linear-gradient(top, #f25811, #f25811);
                background-image: -ms-linear-gradient(top, #f25811, #f25811);
                background-image: -o-linear-gradient(top, #f25811, #f25811);
                background-image: linear-gradient(to bottom, #f25811, #f25811);
                text-decoration: none;
            }

        .btnAxis {
            background: #c81b5d;
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

            .btnAxis:hover {
                background: #f25811;
                background-image: -webkit-linear-gradient(top, #f25811, #f25811);
                background-image: -moz-linear-gradient(top, #f25811, #f25811);
                background-image: -ms-linear-gradient(top, #f25811, #f25811);
                background-image: -o-linear-gradient(top, #f25811, #f25811);
                background-image: linear-gradient(to bottom, #f25811, #f25811);
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

        .auto-style1 {
            font-family: Verdana, Geneva, sans-serif, "Times New Roman", Times, serif Verdana, Geneva, sans-serif;
            font-size: 10pt;
            font-weight: bold;
            color: #000000;
            margin-left: 20px;
            padding-left: 40px;
            margin-right: 20px;
            padding-right: 20px;
            text-align: justify;
            line-height: 20px;
            padding-top: 10px;
            width: 250px;
        }

        .auto-style2 {
            width: 250px;
        }

        .text_box {
        }
    </style>
    <script type="text/javascript" lang="javascript">
        function ValidateLAPLForm() {

            if (document.getElementById('TxnAmount').value == '') {
                alert('Invalid payment. Check if already paid ..');
                document.getElementById('txtEmail').focus();
                return false;
            }
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
            checkEmail();
            document.getElementById('billing_tel').value = document.getElementById('txtMobileNo').value;
            if (confirm("Confirm to Continue?")) {
                document.getElementById('customerData').action = 'LAPLHSRPPayment.aspx';
                document.getElementById('customerData').submit();
                return true;
            }
            else {
                return false;
            }
        }
        function ValidateFormAxis() {

            if (document.getElementById('TxnAmount').value == '') {
                alert('Invalid payment. Check if already paid ..');
                document.getElementById('txtEmail').focus();
                return false;
            }

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
            checkEmail();
            document.getElementById('billing_tel').value = document.getElementById('txtMobileNo').value;

            if (confirm("Confirm to Continue?")) {
             
                return true;
            }
            else {
                return false;
            }
        }

        function ValidateForm() {

            if (document.getElementById('TxnAmount').value == '') {
                alert('Invalid payment. Check if already paid ..');
                document.getElementById('txtEmail').focus();
                return false;
            }

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
            checkEmail();
            document.getElementById('billing_tel').value = document.getElementById('txtMobileNo').value;

            if (confirm("Confirm to Continue?")) {
                document.getElementById('customerData').action = 'ccavRequestHandler.aspx';
                document.getElementById('customerData').submit();
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
        function checkEmail() {

            var email = document.getElementById('txtEmail');
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

            if (!filter.test(email.value)) {
                alert('Please provide a valid email address');
                email.focus;
                return false;
            }
        }
    </script>
    <title>Telangana HSRP</title>
    <script lang="javascript" src="js/header.js"></script>
</head>
<body>
    <script language="javascript">header();</script>
    <form method="post" name="customerData" id="customerData" runat="server">
        <table width="1100" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2" class="maininnertextheading">
                    <h2>Step-2:Confirm Alert Information </h2>
                </td>
            </tr>
             <tr>
                <td colspan="2" >
                    <font size="3" color="blue"><marquee behavior="scroll" direction="right" onmouseover="this.stop();" onmouseout="this.start();"> <i>'PAY DIRECT-LAPL'</i> IS AVAILABLE * HASSLE FREE WAY TO PAY HSRP FEE  *  <i>'PAY DIRECT-LAPL'</i> IS AVAILABLE </marquee> </font>
                </td>
            </tr>

            <tr>
                <td colspan="2" class="maininnertextheading">
                    <asp:Label ID="lblError" ForeColor="Red" runat="server"></asp:Label>

                </td>
            </tr>
            <tr>
                <td class="maininnertextheading">HSRP Online Order Number :
                       
                </td>


                <td class="maininnertextheading">
                    <asp:Label ID="orderNo" runat="server"></asp:Label>
                    <input type="hidden" name="merchant_id" id="merchant_id" value="81612" />
                </td>
            </tr>
            <tr>
                <td class="maininnertextheading">Authorization Number :
                       
                </td>

                <td class="maininnertextheading">
                    <asp:Label ID="lblAuthNo" runat="server"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="maininnertextheading">Transaction Amount :
                </td>

                <td class="maininnertextheading">
                    <asp:Label ID="transAmt" runat="server"></asp:Label>
                    /=
                </td>

            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label1" runat="server" Text="Vehicle Owner Name:"></asp:Label>

                </td>
                <td class="maininnertextheading">
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label3" runat="server" Text="Email ID of Vehicle Owner:"></asp:Label>
                    <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Red"
                        Text="*"></asp:Label>
                </td>
                <td class="maininnertextheading">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="text_box" MaxLength="50" size="50"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label5" runat="server" Text="Mobile No. Of Vehicle Owner:"></asp:Label>
                    <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Red"
                        Text="*"></asp:Label>
                </td>
                <td class="maininnertextheading">
                    <asp:TextBox ID="txtMobileNo" runat="server" CssClass="text_box" onkeypress="return isNumberKey(event)" MaxLength="10" size="50"></asp:TextBox>(Example:9911739727)
                </td>
            </tr>
            <tr>
                <td colspan="2" class="maininnertextheading">

                    <asp:Label ID="lblErr" runat="server" Font-Bold="true" ForeColor="red"></asp:Label>
                    <input type="hidden" name="order_id" id="order_id" runat="server" />
                    <input type="hidden" name="amount" id="amount" runat="server" />
                    <input type="hidden" name="currency" value="INR" />
                    <input type="hidden" name="redirect_url" value="https://hsrpts.com/hsrponline/ccavResponseHandler.aspx" />
                    <input type="hidden" name="cancel_url" value="https://hsrpts.com/hsrponline/PaymentTransactionFaliure.aspx" />
                    <input type="hidden" name="language" value="EN" />
                    <input type="Hidden" name="billing_name" id="billing_name" runat="server" />
                    <input type="Hidden" name="billing_address" id="billing_address" runat="server" />
                    <input type="Hidden" name="billing_city" id="billing_city" runat="server" />
                    <input type="Hidden" name="billing_state" id="billing_state" runat="server" value="TS" />
                    <input type="Hidden" name="billing_zip" id="billing_zip" runat="server" />
                    <input type="Hidden" name="billing_country" id="billing_country" value="India" />
                    <input type="Hidden" name="billing_tel" id="billing_tel" runat="server" />
                    <input type="Hidden" name="billing_email" id="billing_email" runat="server" />
                    <input type="Hidden" name="delivery_name" value="" />
                    <input type="Hidden" name="delivery_address" value="" />
                    <input type="Hidden" name="delivery_city" value="" />
                    <input type="Hidden" name="delivery_state" value="" />
                    <input type="Hidden" name="delivery_zip" value="" />
                    <input type="Hidden" name="delivery_country" value="" />
                    <input type="Hidden" name="delivery_tel" value="" />
                    <input type="Hidden" name="merchant_param1" id="merchant_param1" runat="server" />
                    <input type="Hidden" name="merchant_param2" id="merchant_param2"  runat="server" />
                    <input type="Hidden" name="merchant_param3" value="" />
                    <input type="Hidden" name="merchant_param4" value="" />
                    <input type="Hidden" name="merchant_param5" value="" />
                    <input type="Hidden" name="promo_code" />
                    <input type="Hidden" name="customer_identifier" />
                </td>
            </tr>

            <tr>
                <td class="auto-style2"></td>
                <td>
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



                    <img src="../images/new.gif"/><input type="button" value="Pay Direct-LAPL" class="btnLAPL" onclick="javascript: return ValidateLAPLForm();" /><br />
                    <br />
                    <input type="button" value="Pay Now Through-All Banks" class="Btn" onclick="javascript: return ValidateForm();" /><br />
                    <br />
                    <asp:Button ID="payICICI" runat="server" Text="ICICI Corporate Banking" class="btnICICI" OnClick="payICICI_Click" />

                </td>
            </tr>
        </table>
    </form>
    <table width="1100" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="auto-style2"></td>
            <td>
                <form action="https://www.iconnect.co.in/BankAwayRetail/sgonHttpHandler.aspx?Action.ShoppingMall.Login.Init=Y&BankId=211&USER_LANG_ID=001&UserType=2&AppType=corporate" method="post" style="margin-left: 51px">

                    <input type="hidden" name="AMT" id="AMT" runat="server">
                    <input type="hidden" name="MD" id="MD" value="P">
                    <input type="hidden" name="CRN" id="CRN" value="INR">
                    <input type="hidden" name="PID" id="PID" value="000000004659">
                    <input type="hidden" name="ITC" id="ITC" runat="server">
                    <input type="hidden" name="PRN" id="PRN" runat="server">
                    <input type="hidden" name="RU" value="https://www.hsrpts.com/hsrponline/axisresponse.aspx">
                    <input type="hidden" name="CG" value="N">
                    <input type="hidden" name="RESPONSE" value="AUTO">
                    <input type="HIDDEN" name="STATFLG" value="H">
                    <input name="pay" type="submit" value="Axis i-Connect" class="btnAxis" onclick="javascript: return ValidateFormAxis();">
                </form>
            </td>
        </tr>
        <tr>
            <td class="auto-style2"></td>
            <td>
                <form action="https://corporate.hdfcbank.com/CTPRoute.html" method="get" style="margin-left: 51px">
                    <input type="Hidden" name="ClientCode" id="ClientCode" value="HSRPTS" runat="server" />
                    <input type="Hidden" name="MerchantCode" id="MerchantCode" value="HSRPTS" runat="server" />
                    <input type="Hidden" name="TxnCurrency" id="TxnCurrency" value="INR" runat="server" />
                    <input type="Hidden" name="TxnAmount" id="TxnAmount" runat="server" />
                    <input type="Hidden" name="TxnScAmount" id="TxnScAmount" runat="server" />
                    <input type="Hidden" name="MerchantRefNo" id="MerchantRefNo" runat="server" />
                    <input type="Hidden" name="Ack" id="Ack" value="Y" />
                    <input type="Hidden" name="Response" id="Response" value="N" />
                    <input type="Hidden" name="FailureStaticFlag" id="FailureStaticFlag" value="N" runat="server" />
                    <input type="Hidden" name="SuccessStaticFlag" id="SuccessStaticFlag" value="N" runat="server" />
                    <input type="Hidden" name="Date" id="Date" runat="server" />
                    <input type="Hidden" name="Ref1" id="Ref1" value="" runat="server" />
                    <input type="Hidden" name="Ref2" id="Ref2" value="" runat="server" />
                    <input type="Hidden" name="Ref3" id="Ref3" value="" runat="server" />
                    <input type="Hidden" name="Ref4" id="Ref4" value="" runat="server" />
                    <input type="Hidden" name="Ref5" id="Ref5" value="" runat="server" />
                    <input type="Hidden" name="Ref6" id="Ref6" value="" runat="server" />
                    <input type="Hidden" name="Ref7" id="Ref7" value="" runat="server" />
                    <input type="Hidden" name="Ref8" id="Ref8" value="" runat="server" />
                    <input type="Hidden" name="Ref9" id="Ref9" value="" runat="server" />
                    <input type="Hidden" name="Ref10" id="Ref10" value="" runat="server" />
                    <input type="Hidden" name="Ref11" id="Ref11" value="" runat="server" />
                    <input type="Hidden" name="Date1" id="Date1" value="" runat="server" />
                    <input type="Hidden" name="Date2" id="Date2" value="" runat="server" />

                  <!--  <input name="pay" type="submit" value="HDFC Corporate Banking" class="btnHDFC"><br />-->
                    <br />


                </form>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="maininnertextheading">
                <h3>Please Note :</h3>
                <b>1.</b>Orders with valid authorization Numbers from Transport Department only can be booked.<br />
                <b>2.</b>Your order will be processed only upon receipt of payments.<br />
                <b>3. Ensure to provide the contact number &amp; mail id of the vehicle owner.</b><br />
                <b>4. Payment Support Mail ID :</b><b style="color: #0000FF">online@hsrpts.com</b><br />
                <b>5. Customer Care Number :</b><b style="color: #0000FF">9100026696,9100026691,7306139243,7306698827,040-69999712/040-69999713/040-69999772/040-69999773</b>
                <br />
            </td>

        </tr>
    </table>

    <script language="javascript">footer();</script>
</body>
</html>
