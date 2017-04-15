<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginMac.aspx.cs" Inherits="HSRP.LoginMac" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="shortcut icon" type="image/ico" href="../images/favicon.ico" />
    <link rel="shortcut icon" href="../images/logo.ico" type="image/x-icon" />
    <title>HSRP Application Ver1.0</title>
    <link href="./css/style1.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
   
    <script type="text/javascript">
        var totalCount = 8;

        function ChangeIt() {
            var num = Math.ceil(Math.random() * totalCount);
            document.body.background = 'bgimages/' + num + '.jpg';
            document.body.style.backgroundRepeat = "repeat";
        }
        function validateForm() {

            var username = document.getElementById("txtUserID").value;
            var pass = document.getElementById("txtUserPassword").value;

            if (username == '') {
                alert('Please Provide User Name.');
                document.getElementById("txtUserID").focus();
                return false;
            }
            if (pass == '') {
                alert('Please Provide Password.');
                document.getElementById("txtUserPassword").focus();
                return false;
            }
            return true;


        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="MainPage" style="height: 520px;">
        <div class="container">
            <div class="col-lg-2 col-md-2 col-sm-12 col-xs-12">
                <div class="info-box Green-bg">
                    <div>
                        <%--<img src="images/Member.png" alt="Member Pic" />--%>
                        <h1> Login</h1>
                    </div>
                </div>
                <!--/.info-box-->
            </div>
            <!--/.col-->
            <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                <div class="info-box purple-bg">
                    <div class="count">
                        <br />
                        User ID</div>
                    <div class="title">
                        <asp:TextBox ID="txtUserID" CssClass="textclass" TabIndex="1" MaxLength="20" runat="server"></asp:TextBox>
                        <%--<asp:TextBox ID="txtUserID" runat="server" MaxLength="20" CssClass="textclass"></asp:TextBox>--%>
                        <br />
                        <asp:RequiredFieldValidator ID="reqFieValUserID" runat="server" ErrorMessage="Please Enter Your User ID"
                            ControlToValidate="txtUserID" ValidationGroup="login" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <!--/.info-box-->
            </div>
            <!--/.col-->
            <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12">
                <div class="info-box LightPurple-bg">
                    <div class="count">
                        <br />
                        Password</div>
                    <div class="title">
                         <asp:TextBox ID="txtUserPassword" MaxLength="50" CssClass="textclass" TabIndex="2"  TextMode="Password"
                                    runat="server"></asp:TextBox>
                       <%-- <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" CssClass="textclass"
                            TextMode="Password"></asp:TextBox>--%>
                        <br />
                        <asp:RequiredFieldValidator ID="reqFievalPassword" runat="server" ErrorMessage="Please Enter Your Password"
                            ControlToValidate="txtUserPassword" ValidationGroup="login" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <!--/.info-box-->
            </div>
            <div>
                <asp:Label ID="lblMsgBlue" CssClass="bluelink" runat="server" Text="Label"></asp:Label>
                <asp:Label ID="lblMsgRed" CssClass="alert2" runat="server" ForeColor="Red" Text="Label"></asp:Label>

            </div>
            <!--/.col-->
            <div class="col-lg-2 col-md-2 col-sm-12 col-xs-12">
                <div class="info-box Blue-bg">
                    <br />
                    <br />
                    <br />
                    <div style="margin-left:-6px;">
                         <asp:ImageButton ID="btnLogin" ImageUrl="~/images/login.png" Width="75" Height="22" runat="server" ValidationGroup="login" OnClick="btnLogin_Click" OnClientClick="javascript: return validateForm();" />
                        <%--<asp:ImageButton ID="imgBtnLogin" runat="server" ImageUrl="~/Images/Loginbutton.png"
                        alt="Login" OnClick="imgBtnLogin_Click" ValidationGroup="login" /></div>--%>
                </div>
                <!--/.info-box-->
            </div>
            <!--/.col-->
        </div>
        <div class="container">
            <div class="col-lg-10 col-md-10 col-sm-12 col-xs-12">
                <div class="info-box1 Red-bg">
                    <div class="RememberMe">
                        <asp:CheckBox ID="chkRememberMe" runat="server" />
                        Remember Me
                    </div>
                    <div class="RememberMeRight">
                        <a href="#"><strong>Forgot Password?</strong></a><%--ForgetPassword.aspx--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

   <%-- <div>
        <script type="text/javascript">
            ChangeIt();
        </script>
       
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                
                <td valign="middle" align="center" style="border:solid 0px black; padding-top:180px;">
                   
                    <table width="30%" border="0" style="background-color:#99ff99; height:180px; vertical-align:middle;"  cellpadding="0" cellspacing="0" align="center" >
                     
                        <tr>
                            <td height="5" class="form_text">
                                <img src="images/trans.gif" width="2" height="5" />
                            </td>
                            <td align="left">
                                <span class="form_text">
                                    <img src="images/trans.gif" width="1" height="5" /></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                User Name:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtUserID" CssClass="text_box" TabIndex="1" Width="210px" Height="35px" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                Password:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtUserPassword" CssClass="text_box" TabIndex="2" Width="210px" Height="35px" TextMode="Password"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bluelink" align="left">
                               
                            </td>
                            <td align="left">
                                <asp:ImageButton ID="btnLogin" ImageUrl="~/images/login.png" Width="75" Height="22"
                                    runat="server" OnClick="btnLogin_Click" OnClientClick="javascript: return validateForm();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMsgBlue" CssClass="bluelink" runat="server" Text="Label"></asp:Label>
                                <asp:Label ID="lblMsgRed" CssClass="alert2" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                            </td>
                        </tr>
                         
                    </table>
                    
                </td>
               
            </tr>
           
        </table>
   
        
    </div>--%>
    </form>
</body>
</html>
