<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LAPLHSRPPayment.aspx.cs" Inherits="IntegrationKIt4._5.LAPLHSRPPayment" %>

<!DOCTYPE html>
<html>
<head runat="server">
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
        .auto-style1 {
            font-family: Verdana, Geneva, sans-serif, "Times New Roman", Times, serif Verdana, Geneva, sans-serif;
            font-size: 10pt;
            font-weight: normal;
            color: #000000;
            margin-left: 20px;
            padding-left: 40px;
            margin-right: 20px;
            padding-right: 20px;
            text-align: justify;
            line-height: 20px;
            padding-top: 10px;
        }
    </style>
 
    <script type="text/javascript" lang="javascript">
      
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
                    if (document.getElementById("PymntFlag").value == 'Yes')
                    {
                        document.getElementById("form1").submit();
                    }
                    if (document.getElementById("PymntFlag").value == 'No') {
                        document.getElementById("form1").submit();
                    }

                });
            }
        }
    </script>
    <title>Telangana HSRP</title>
    <script language="javascript" src="js/header.js"></script>
</head>
<body>
   <script language="javascript">header();</script>
    
    <form id="form1" method="post" runat="server">
      
                 
               <table width="1100" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2" class="maininnertextheading">
                    <h2>Pay Directly by LAPL</h2>
                </td>
            </tr>

             <tr>
                <td colspan="2" class="auto-style1">                    
                   
                    <em >(If you dont have this service enabled please contact: online@hsrpts.com/9100026696,9396864878)</em></td>
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
                    <asp:Label ID="Label5" runat="server" Text="Amount"></asp:Label>            
               </td>
                 <td class="maininnertextheading" > 
                     <asp:Label ID="LblAmount" runat="server"></asp:Label>
                    <input type="hidden" runat="server" id="hdnPhone" name="hdnPhone"/>            
               </td>
             </tr>
                   <asp:Panel ID="PanelLogin" runat="server">
              <tr>
                <td class="maininnertextheading" > 
                    <asp:Label ID="Label7" runat="server" Text="User Id"></asp:Label><span style="color:red">*</span>            
               </td>
                 <td class="maininnertextheading" > 
                     <asp:TextBox ID="txtUserId" runat="server" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="login" ForeColor="Red" ControlToValidate="txtUserId" ErrorMessage="Please Enter User Id"></asp:RequiredFieldValidator>
                     
                                
               </td>


             </tr>

              <tr>
                <td class="maininnertextheading" > 
                    <asp:Label ID="Label9" runat="server" Text="Passwod"></asp:Label><span style="color:red">*</span>            
               </td>
                 <td class="maininnertextheading" > 
                     <asp:TextBox ID="txtpassword" runat="server" TextMode="Password" onkeypress="return blockSpecialChar(event)"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red" ValidationGroup="login" ControlToValidate="txtpassword" ErrorMessage="Please Enter Password"></asp:RequiredFieldValidator>
                     
                                
               </td>
             </tr>
                                                 <tr>
              
               <td class="maininnertextheading">
                    
               </td>
               <td class="maininnertextheading">
                   <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Text=""></asp:Label>
                   
                   
               </td>
              
              
           </tr>
           <tr>
               <td class="maininnertextheading"></td>
               <td class="maininnertextheading">
                   <asp:Button ID="Btnlogin" runat="server" Text="Login to LAPL" OnClick="Btnlogin_Click" ValidationGroup="login" class="btnLAPL" />
                   &nbsp;&nbsp;
                   <asp:Button ID="btnLogincancle" runat="server" Text="Cancel" OnClick="btncancle_Click" class="btnLAPL" />

               </td>
           </tr>
                       </asp:Panel>
                   <asp:Panel ID="PanelPayNow" runat="server" Visible="false">
                   <tr>
              
               <td class="maininnertextheading">
                    <asp:Label ID="Label2" runat="server"  Text="Account Balance:"></asp:Label>
               </td>
               <td class="maininnertextheading">                   
                   <asp:Label ID="lblBalanceAmount" runat="server"  Text=""></asp:Label>                   
               </td>
              
              
           </tr>
                   <tr>
              
               <td class="maininnertextheading">
                    <asp:Label ID="Label4" runat="server" Text="Utilized Amount:"></asp:Label>
               </td>
               <td class="maininnertextheading">                   
                   <asp:Label ID="lblCollectionAmount" runat="server"  Text=""></asp:Label>
               </td>
              
              
           </tr>
                                          <tr>
              
               <td class="maininnertextheading">
                    <asp:Label ID="Label3" runat="server" Text="Dealer Name:"></asp:Label>
               </td>
               <td class="maininnertextheading">                   
                   <asp:Label ID="lblDealername" runat="server"  Text=""></asp:Label>
               </td>
              
              
           </tr>
                   <tr>
              
               <td class="maininnertextheading">
                    
               </td>
               <td class="maininnertextheading">                   
                   <asp:Button ID="btnpaynow" runat="server" Text="Pay Now" OnClick="btnpaynow_Click" ValidationGroup="login" class="btnLAPL" />&nbsp;&nbsp;
                   
                   <asp:Button ID="btnPaynowcancle" runat="server" Text="Cancel" OnClick="btnPaynowcancle_Click" class="btnLAPL" />
               </td>
              
              
           </tr>
                       </asp:Panel>
                    <tr>
              
               <td class="maininnertextheading">
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
                     <asp:HiddenField ID="UserID" runat="server" />
                   <asp:HiddenField ID="returnR" runat="server" />
           
               </td>
               <td class="maininnertextheading">                   
                   <asp:Label ID="lbldone" runat="server" Text="" ForeColor="Blue"></asp:Label>
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
