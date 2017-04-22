<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Complaints.aspx.cs" Inherits="HSRP.Complaints" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <%--css of hs--%>
    
    <link href="css/main.css" rel="stylesheet" type="text/css" />
    
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
<body >
    <form id="form1" runat="server">
    <div>
               <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" height="75px" style="background-image: url(images/ntopbgheader.png)">
  <tr>
    <td width="50%" class="topheaderboldtext">ROSMERTA HSRP VENTURES PVT. LTD. </td>
    <td width="50%" valign="middle">&nbsp;</td>
  </tr>
</table>

<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" >
  <tr>
    <td valign="middle" bgcolor="#3d3d3d"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="30">&nbsp;</td>
        <td width="40%">&nbsp;</td>
      </tr>
    </table>
    
    
    </td>
  </tr>
</table>
                    <br />
                    <table width="100%" style="height:400px;background-color:White" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top">
                                <%-- <asp:ContentPlaceHolder ID="ContentPlaceHolder1" runat="server">
                                </asp:ContentPlaceHolder>--%>

                               <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
            <div id="prin" >
                <div align="left">
                    <table style="width: 80%; height: 108px;">
                        <tr>
                            <td align="center">&nbsp;</td>
                            <td colspan="3" align="center"><span class="headingmain">Complaint</span>&nbsp;

                               
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td width="150">
                                &nbsp;</td>
                            <td width="200">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="Region"></asp:Label>
                            </td>
                            <td width="150">
                                <asp:DropDownList ID="ddlComplaintRegion" runat="server">
                                    <asp:ListItem Value="0">-Select Region-</asp:ListItem>
                                    <asp:ListItem Value="1">Issue In Affixiation</asp:ListItem>
                                    <asp:ListItem Value="2">Complaint</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="ddlComplaintRegion" ErrorMessage="Please Select Region" 
                                    InitialValue="-Select Region-"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td width="250">
                                &nbsp;</td>
                            <td width="100">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="Owner Name"></asp:Label>
                                <br />
                            </td>
                            <td class="style8">
                                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                    ControlToValidate="txtName" ErrorMessage="This Field can not be empty"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblRegno" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="Region No."></asp:Label>
                                <br />
                            </td>
                            <td class="style8">
                                <asp:TextBox ID="txtRegno" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                    ControlToValidate="txtRegno" ErrorMessage="This Field can not be empty"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style10">
                                &nbsp;</td>
                            <td class="style10">
                                <asp:Label ID="lblEngineNo" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="Engine No"></asp:Label>
                                <br />
                            </td>
                            <td class="style8">
                                <asp:TextBox ID="txtEngineno" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ControlToValidate="txtEngineno" ErrorMessage="This Field can not be empty"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblChassisNo" runat="server" Text="Chassis No" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                            <td class="style7">
                                <asp:TextBox ID="txtChasisNo" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                    ControlToValidate="txtChasisNo" ErrorMessage="This Field can not be empty"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblRemarks" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="Remarks"></asp:Label>
                            </td>
                            <td class="style7">
                                <asp:TextBox ID="txtRemarks" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style5">
                                &nbsp;</td>
                            <td class="style5">
                                &nbsp;</td>
                            <td class="style7">
                                <asp:Button ID="btnSave" runat="server" Text="Submit" onclick="btnSave_Click" />
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                                     </div>
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                  
                    <tr>
                   
                        <td>
                            <br />
                           
                                        </td>
                                    </tr>
                                </table>
                            </div>
                       
                <div style=" height:150px;"></div>
                     <table width="100%" style="height:60px" border="0" align="center" cellpadding="0" cellspacing="0"
                        class="marqueelinebg">
                        <tr>
                           <td style="color: #FFFFFF; font: normal 15px tahoma, arial, verdana;" valign="middle"
                                align="left">
                                ROSMERTA HSRP VENTURES PVT. LTD.
                                <br />
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
                    
                    <table width="100%">
                                                <tr>
                                                <td>
                                                    <asp:Button ID="Button1" runat="server" Text="Go Back" CssClass="button"   Font-Size="Medium"  onclick="Button1_Click"  />
                                                    <%--<asp:HyperLink ID="HyperLinkGoBack" NavigateUrl="~/OrderStatus.aspx" runat="server"><img src="img/Goback1.png" style=" width:50px; height:50px" /></asp:HyperLink>--%>
                                                    </td>
                                                <td align="right">
                                                    <%--<asp:HyperLink ID="HyperLink1" runat="server" Onclick="print()"><img src="img/print.png" /></asp:HyperLink>--%>
                                                   
                                                    </td>
                                                
                                                </tr>

                                                </table>
                 
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0"  class="footerbottoms">
                    <tr>
                    <td align="center" >
                   
                    </td>
                    </tr>

                        <tr>
                            <td valign="middle" >
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
