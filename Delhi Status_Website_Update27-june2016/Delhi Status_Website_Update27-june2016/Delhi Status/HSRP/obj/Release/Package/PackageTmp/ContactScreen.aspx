<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactScreen.aspx.cs" Inherits="HSRP.ContactScreen" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Delhi HSRP Status</title>

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
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" height="75px"
                        style="background-image: url(images/ntopbgheader.png)">
                        <tr>
                            <td width="50%" class="topheaderboldtext">
                                 ROSMERTA HSRP VENTURES PVT. LTD.
                            </td>
                            <td width="50%" valign="middle">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="middle" bgcolor="#3d3d3d">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td height="30">
                                            &nbsp;
                                        </td>
                                        <td width="40%">
                                            &nbsp;
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
                                                        <td>
                                                            &nbsp;
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
                                                                                                <span class="headingmain">Contact Details</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <span class="headingmain">   &nbsp;   
                                                                                                    </span>
                                                                                            </td>
                                                                                             <td>
                                                                                                 <span class="headingmain">   &nbsp;   
                                                                                                    </span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <br />
                                                                                    
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;
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
                                                    <tr style="height: 80px">
                                                        <td style="color: Black; font: normal 14px tahoma, arial, verdana;" valign="bottom"
                                                            align="center">
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
                                align="center">
                                &nbsp;</td>
                        </tr>
                    </table>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="footerbottoms">
                        <tr>
                            <td align="center">
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">
                                ROSMERTA HSRP VENTURES PVT. LTD.
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div>
    <asp:Button ID="btnHidden" runat="server" Text="Button" Enabled="false" CssClass="btnhide" Visible="false" />
                                                <asp:Button ID="Button2" runat="server" Text="Button" style="visibility:hidden"/>
                                                <asp:Panel ID="PnlModal" runat="server" Width="650px" CssClass="modalPopup" Font-Names="Arial" Font-Size="Small" ForeColor="Black" Height="400px" HorizontalAlign="Center" Visible="false">
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
                                                                <td width="200">
                                                                    &nbsp;</td>
                                                                <td width="150">
                                                                    &nbsp;</td>
                                                                <td width="200">
                                                                    &nbsp;</td>
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
                                                                <td width="100">
                                                                    <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Mobile Number"></asp:Label>
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
                                                                <td class="style5">
                                                                    &nbsp;</td>
                                                                <td>
                                                                    <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="Submit" 
                                                                        ValidationGroup="v" />
                                                                    &nbsp;
                                                                    <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
                                                                        Text="Cancel" CausesValidation="False" />
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
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
