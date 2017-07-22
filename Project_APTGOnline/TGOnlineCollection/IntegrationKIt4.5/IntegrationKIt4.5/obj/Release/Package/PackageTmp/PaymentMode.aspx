<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentMode.aspx.cs" Inherits="WebApplication2.PaymentMode" %>

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
        }.btnICICI {
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
            checkEmail();
            document.getElementById('billing_tel').value = document.getElementById('txtMobileNo').value

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

    <form id="form1" runat="server">

               <table width="1100" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2" class="maininnertextheading">
                    <h2>Pay Directly by LAPL</h2>
                </td>
            </tr>

             <tr>
                <td colspan="2" class="maininnertextheading">                    
                   
                </td>
            </tr>
            <tr>
                <td class="maininnertextheading" >
                     <asp:Label ID="Label1" runat="server" Text="Order No"></asp:Label>
                                
               </td>
                 <td class="maininnertextheading"> 
                    <asp:Label ID="LblOrderNo" runat="server" ></asp:Label>            
               </td>
             </tr>


            <tr>
                <td class="maininnertextheading" > 
                     <asp:Label ID="Label3" runat="server" Text="Auth No"></asp:Label>           
               </td>
                 <td class="maininnertextheading" > 
                      <asp:Label ID="LblAuthNo" runat="server"></asp:Label>          
               </td>
             </tr>


            <tr>
                <td class="maininnertextheading" > 
                    <asp:Label ID="Label5" runat="server" Text="Amount"></asp:Label>            
               </td>
                 <td class="maininnertextheading" > 
                     <asp:Label ID="LblAmount" runat="server"></asp:Label>
                                
               </td>
             </tr>

              
           
           <tr>
              
               <td class="maininnertextheading">
                  <asp:Button ID="BtnpayMode" runat="server" Text="Payment Mode" />
               </td>
                <td class="maininnertextheading">
                    <asp:Button ID="Btncalcel" runat="server" Text="Cancel" />
               </td>
              
              
           </tr>
                                               <tr>
                <td colspan="2" class="maininnertextheading">
                    <h3>Please Note :</h3>
                    <b>1.</b>Orders with valid authorization Numbers from Transport Department only can be booked.<br />
                    <b>2.</b>Your order will be processed only upon receipt of payments.<br />
                    <b>3. Ensure to provide the contact number &amp; mail id of the vehicle owner.</b><br />
                    <b>4. Payment Support Mail ID :</b><b style="color: #0000FF">online@hsrpts.com</b><br />
                    <b>5. Customer Care Number :</b><b style="color: #0000FF">9100026696,9100026691,040-69999712/040-69999772/040-69999773</b>
                    <br />
                </td>

            </tr>         
        </table>
    </form>
    <script language="javascript">footer();</script>
</body>
</html>
