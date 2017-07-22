<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentTransactionFaliure.aspx.cs" Inherits="IntegrationKIt4._5.PaymentTransactionFaliure" %>



<html >

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

    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_btnSave").hide();

        });

        $(document).on("click", ".alert1", function (e) {

            var text = $("#txtAuthNo").val()
            var regno = $("#lblRegNo").val()
            debugger;
            if (text == "" && regno == "") {

                bootbox.alert("Please Enter Authorization No. and Click On Go Button !", function () {

                });
            }
            else {
                var ddlaffix = $("#ddlaffixation").val()
                if (ddlaffix == "--Select Affixation Center--") {
                    bootbox.alert("Please Select Affixation Center !", function () {

                    });
                }
                else {

                    bootbox.confirm("<B>Please Confirm !</b> <BR><i>Please Ensure Customer Data On Screen Matches With Authorization Slip Before Save.</i>", function (result) {

                        if (result) {
                            $("#save1").hide();
                            $("#btnSave").show();
                        }
                    });
                }
            }
        });

    </script>
    <title>Telangana HSRP</title>
    <script lang="javascript" src="js/header.js"></script>
</head>
<body>
    <script lang="javascript">header();</script>
    <form id="TT" runat="server">
        
        <table width="1100" border="0" align="center" cellpadding="0" cellspacing="0">
             <tr>
                 <td><h2>Your Online HSRP transaction was unsuccessfull. Please try again. </h2> </td>
             </tr>
    
            <tr>
                <td colspan="4" class="maininnertextheading">
                    <h3>Please Note :</h3>
                    <b>1.</b>Orders with valid authorization Numbers from Transport Department only can be booked.<br/>
                    <b>2.</b>Your order will be processed only upon receipt of payments.<br/>
                    <b>3. Ensure to provide the contact number &amp; mail id of the vehicle owner.</b><br />
                    <b>4. Payment Support Mail ID :</b><b style="color: #0000FF">online@hsrpts.com</b><br />
                    <b>5. Customer Care Number :</b><b style="color: #0000FF">040-69999712/040-69999772/040-69999773</b> <br />
                </td>

            </tr>

        </table>
        
        </form>
    <script language="javascript">footer();</script>
</body>
</html>

