<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderStatus.aspx.cs" Inherits="HSRP.WebForm1" %>

<%--<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AP HSRP Status</title>

        <style type="text/css">
        .modalPopup
        {
            background-color: #FFFFFF;
            filter: alpha(opacity=40);
            opacity: 0.7;
            xindex: -1;
        }
    </style>
     <style type="text/css">
      
        .ModalPopupBG
        {
            background-color: #333333;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        .modalPopup
        {
           
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 250px;
        }
    </style>


    <%--css of hs--%><%--<link href="css/Style.css" rel="stylesheet" type="text/css" />
    <link href="css/baseStyle.css" rel="stylesheet" type="text/css" />
    <link href="css/gridStyle.css" rel="stylesheet" type="text/css" />
    <link href="css/legend.css" rel="stylesheet" type="text/css" />--%><%-- <link href="css/menuStyle.css" rel="stylesheet" type="text/css" />
    <link href="css/calendarStyle.css" rel="stylesheet" type="text/css" />--%>
    <link href="css/main.css" rel="stylesheet" type="text/css" />
    <script src="javascript/common.js" type="text/javascript"></script>
    <%--css of hs--%>
    <script language="javascript" type="text/javascript">

        function validateSearch() {

            if (document.getElementById("<%=txtUserName.ClientID%>").value == "") {
                alert("Please Provide User Name");
                document.getElementById("<%=txtUserName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtPassword.ClientID%>").value == "") {
                alert("Please Provide Password");
                document.getElementById("<%=txtPassword.ClientID%>").focus();
                return false;
            }
        }


        function validate() {

            if (document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").value == "") {
                alert("Please Provide Vehicle Registration Number.");
                document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>"))) {
                alert("Please Provide Valid Vehicle Registration Number.");
                document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").value != "") {
                var VehicalRegNo = document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").value.toUpperCase();
                if (VehicalRegNo.length < 4) {
                    alert("Please Provide Valid Vehicle Registration Number.");
                    document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").focus();
                    return false;
                }


                // alert(VehicalRegNo.substring(0, 2));
                if (VehicalRegNo.substring(0, 2) != 'HR') {
                    alert("Please Provide Valid Vehicle Registration Number.");
                    document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").focus();
                    return false;
                }

            }




        }

         
        
    
    </script>
    <script type="text/javascript">

        function printSelection(node) {

            var content = node.innerHTML
            var pwin = window.open('', 'print_content', 'width=1500,height=1000');

            pwin.document.open();
            pwin.document.write('<html><body onload="window.print()">' + content + '</body></html>');
            pwin.document.close();

            setTimeout(function () { pwin.close(); }, 1000);

        }
    </script>
    <style type="text/css">

        .style8
        {
            width: 89px;
        }
        .style10
        {
            width: 77px;
        }
        .style7
        {
            height: 29px;
            width: 89px;
        }
        .style5
        {
            height: 29px;
            width: 77px;
        }
        </style>
</head>
<body style="background-color: #F0F0F0">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <%--<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" height="75px"
                        style="background-image: url(images/ntopbgheader.png)">
                        <tr>
                            <td width="50%" class="topheaderboldtext">
                                 LINK UTSAV AUTO SYSTEMS PVT. LTD.</td>
                            <td width="50%" valign="middle">
                                &nbsp;
                            </td>
                        </tr>
                    </table>--%>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="middle" bgcolor="#576e8c">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td height="30">&nbsp;
                                            
                                        </td>
                                        <td width="40%">&nbsp;
                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="middle">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" style="background-image: url(http://203.122.58.242/hsrp/images/menu_bg.gif);">
                                    <tr>
                                        <td>
                                            <div>
                                                <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>&nbsp;
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td height="27" background="images/midboxtopbg.jpg">
                                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td height="26">
                                                                                                <span class="headingmain">CHECK STATUS OF HIGH SECURITY REGISTRATION PLATES</span>
                                                                                            </td>
                                                                                            <td>
                                                                                             <span class="headingmain">   <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                                                                                    onclick="LinkButton1_Click" 
                                                                                                    Font-Bold="False" 
                                                                                                    >Transport Login</asp:LinkButton>
                                                                                                    &nbsp;   
                                                                                                <asp:LinkButton ID="lnkComplaint" runat="server" CausesValidation="False" 
                                                                                                    onclick="lnkComplaint_Click" 
                                                                                                    Font-Bold="False" Enabled="False" Visible="False" 
                                                                                                    >Complaint</asp:LinkButton>
                                                                                                    </span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <br />
                                                                                    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td valign="top">
                                                                                                <table width="1024" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
                                                                                                    <tr>
                                                                                                        <td height="324" background="images/homebgl.gif" >
                                                                                                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td valign="top">
                                                                                                                        <table width="510" border="0" align="right" cellpadding="3" cellspacing="3">
                                                                                                                            
                                                                                                                            <tr>
                                                                                                                              <td nowrap="nowrap" class="form_text">&nbsp;</td>
                                                                                                                              <td nowrap="nowrap" class="headingmain">&nbsp;</td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td nowrap="nowrap" class="form_text">&nbsp;
                                                                                                                                    
                                                                                                                                </td>
                                                                                                                                <td nowrap="nowrap" class="headingmain">
                                                                                                                                    <label for="test">
                                                                                                                                    </label>
                                                                                                                                   <div id="t1" runat="server">
                                                                                                                                    <span  id="Span2">Vehicle Registration No :</span><span class="header"><span
                                                                                                                                        class="form_text"> <span class="alert">* </span></span></span>
                                                                                                                                &nbsp;&nbsp;<br />
                                                                                                                                    
                                                                                                                                    <input id="TextBoxVehicalRegNo" runat="server" style="text-transform: uppercase"
                                                                                                                                        maxlength="12" name="test" autocomplete="off" type="text" class="form_textboxn" />
                                                                                                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                                                                                                           ControlToValidate="TextBoxVehicalRegNo" ErrorMessage="Enter Vehicle  No" 
                                                                                                                                           SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                                                    <br />
                                                                                                                                       Ex : AP16DA4821
                                                                                                                                    
                                                                                                                                </div>
                                                                                                                                    <div id="t2" runat="server">
                                                                                                                                    
                                                                                                                                  User Name : <asp:TextBox ID="txtUserName" runat="server" Width="111px"></asp:TextBox>
                                                                                                                                       <br />
                                                                                                                                     Password :&nbsp;&nbsp;  <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="110px"></asp:TextBox>
                                                                                                                                    
                                                                                                                                </div>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td class="form_text">&nbsp;
                                                                                                                                    
                                                                                                                                </td>
                                                                                                                               <td align="center" colspan="3">
                                                                    <asp:Label ID="lblmsg1" runat="server" Font-Bold="True" ForeColor="#000099"></asp:Label>
                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td class="form_text">&nbsp;
                                                                                                                                    
                                                                                                                                </td>
                                                                                                                                <td>&nbsp;
                                                                                                                                    
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td >
                                                                                                                                   &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="ButtonGo" runat="server" Text="Search" OnClick="ButtonGo_Click" 
                                                                                                                                        CssClass="button_h" />
                                                                                                                                </td>
                                                                                                                                <td>&nbsp;
                                                                                                                                    
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td class="form_text">&nbsp;
                                                                                                                                    </td>
                                                                                                                                <td>&nbsp;
                                                                                                                                    
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>&nbsp;
                                                                                    
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <div id="vehshow" runat="server" style="text-align: center; width: inherit">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" style="height: 60px" border="0" align="center" cellpadding="0"
                        cellspacing="0" class="marqueelinebg">
                        <tr>
                            <td style="color: Black; font: normal 15px tahoma, arial, verdana;" valign="middle"
                                align="center">&nbsp;
                                </td>
                        </tr>
                    </table>
                   <%-- <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="footerbottoms">
                        <tr>
                            <td align="center">
                            </td>
                        </tr>
                        </table>--%>
                </td>
            </tr>
        </table>
    </div>
    <div align="center">
    <asp:Button ID="btnHidden" runat="server" Text="Button" Enabled="false" 
            CssClass="btnhide" Visible="False" />
                                            <%--<cc1:modalpopupextender ID="mpeSave" runat="server" 
                                                    TargetControlID="lnkComplaint" PopupControlID="PnlModal" 
                                                    BackgroundCssClass="ModalPopupBG">
                                                </cc1:modalpopupextender>--%>
                                                <asp:Button ID="Button2" runat="server" Text="Button" style="visibility:hidden"/>
                                                <asp:Panel ID="PnlModal" runat="server" 
            Width="650px" 
                                                   Font-Names="Arial" 
            Font-Size="Small" ForeColor="Black" Height="400px" 
            HorizontalAlign="Center" Visible="False">
                                                   <br />
                                                    <div align="left">
                                                        <table style="width: 100%; height: 108px;">
                                                            <tr>
                                                                <td align="center" colspan="3">
                                                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="#000099"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="3">
                                                                    <span class="headingmain">Complaint</span>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="200">&nbsp;
                                                                    </td>
                                                                <td width="150">&nbsp;
                                                                    </td>
                                                                <td width="200">&nbsp;
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Reason"></asp:Label>
                                                                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                                                </td>
                                                                <td width="190">
                                                                    <asp:DropDownList ID="ddlComplaintRegion" runat="server" ValidationGroup="v">
                                                                        <asp:ListItem Value="0">-Select Reason-</asp:ListItem>
                                                                        <asp:ListItem Value="1">Issue In Affixiation</asp:ListItem>
                                                                        <asp:ListItem Value="2">Complaint</asp:ListItem>
                                                                        <asp:ListItem>Query</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                                                        ControlToValidate="ddlComplaintRegion" ErrorMessage="Please Select Region" 
                                                                        InitialValue="-Select Region-" ValidationGroup="v"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="125">
                                                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Owner Name"></asp:Label>
                                                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                                                    <br />
                                                                </td>
                                                                <td class="style8" style="color: #FF0000" width="190">
                                                                    <asp:TextBox ID="txtName" runat="server" ValidationGroup="v"></asp:TextBox>
                                                                    </td>
                                                                <td>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                                        ControlToValidate="txtName" ErrorMessage="This Field can not be empty" 
                                                                        ValidationGroup="v" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                                                        ControlToValidate="txtName" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v"></asp:RegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="120">
                                                                    <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Mobile Number" Width="100px"></asp:Label>
                                                                    <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="Red" 
                                                                        Text="*"></asp:Label>
                                                                </td>
                                                                <td class="style8" style="color: #FF0000" width="190">
                                                                    <asp:TextBox ID="txtMobile" runat="server" ValidationGroup="v" MaxLength="10"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                                                                        ControlToValidate="txtMobile" ErrorMessage="This Field can not be empty" 
                                                                        ValidationGroup="v" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                                                        ControlToValidate="txtMobile" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="^[0-9]+$" ValidationGroup="v"></asp:RegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100">
                                                                    <asp:Label ID="Label11" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="E-Mail"></asp:Label>
                                                                </td>
                                                                <td class="style8" style="color: #FF0000" width="190">
                                                                    <asp:TextBox ID="txtMail" runat="server" ValidationGroup="v"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                                        ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                                        ControlToValidate="txtMail" Display="Dynamic" ValidationGroup="v"></asp:RegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="175">
                                                                    <asp:Label ID="lblRegno" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Vehicle Registration No."></asp:Label>
                                                                    <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                                                    <br />
                                                                </td>
                                                                <td class="style8">
                                                                    <asp:TextBox ID="txtRegno" runat="server" ValidationGroup="v" MaxLength="10"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                                        ControlToValidate="txtRegno" ErrorMessage="This Field can not be empty" 
                                                                        ValidationGroup="v" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                                                                        ControlToValidate="txtRegno" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v"></asp:RegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style10">
                                                                    <asp:Label ID="lblEngineNo" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Engine No"></asp:Label>
                                                                    <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                                                    <br />
                                                                </td>
                                                                <td class="style8">
                                                                    <asp:TextBox ID="txtEngineno" runat="server" ValidationGroup="v"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                                                        ControlToValidate="txtEngineno" ErrorMessage="This Field can not be empty" 
                                                                        ValidationGroup="v" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" 
                                                                        ControlToValidate="txtEngineno" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v"></asp:RegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblChassisNo" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Chassis No"></asp:Label>
                                                                    <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                                                </td>
                                                                <td class="style7">
                                                                    <asp:TextBox ID="txtChasisNo" runat="server" ValidationGroup="v"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                                                        ControlToValidate="txtChasisNo" ErrorMessage="This Field can not be empty" 
                                                                        ValidationGroup="v" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" 
                                                                        ControlToValidate="txtChasisNo" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v"></asp:RegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblRemarks" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Remarks"></asp:Label>
                                                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                                                </td>
                                                                <td class="style7">
                                                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="40px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                                                        ControlToValidate="txtRemarks" ErrorMessage="This Field can not be empty" 
                                                                        ValidationGroup="v" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" 
                                                                        ControlToValidate="txtRemarks" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v"></asp:RegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style5">&nbsp;
                                                                    </td>
                                                                <td>
                                                                    <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="Submit" 
                                                                        ValidationGroup="v" />
                                                                    &nbsp;
                                                                    <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
                                                                        Text="Cancel" CausesValidation="False" />
                                                                </td>
                                                                <td>&nbsp;
                                                                    </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <br />
                                                    <br />  
                                                      
                                                </asp:Panel> 
    </div>
    </form>
</body>
</html>
