<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Complaint.aspx.cs" Inherits="HSRP.Complaint" %>

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



        

    </script>
    <style type="text/css">
        .style4
        {
            height: 29px;
            width: 242px;
        }
        .style5
        {
            height: 29px;
            width: 77px;
        }
        .style7
        {
            height: 29px;
            width: 89px;
        }
        .style8
        {
            width: 89px;
        }
        .style9
        {
            width: 242px;
        }
        .style10
        {
            width: 77px;
        }
    </style>
</head>
<body style="background-color: #F0F0F0">
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


                    <br />
                    <table width="100%" style="height:400px;background-color:White" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top">
                                <%-- <asp:ContentPlaceHolder ID="ContentPlaceHolder1" runat="server">
                                </asp:ContentPlaceHolder>--%>

                               
                            </div>
                       
                <div style=" height:150px;">
                    <table style="width: 100%; height: 108px;">
                        <tr>
                            <td colspan="3">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblComplaint" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="Complaint" Font-Size="Medium"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td class="style5">
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="Region"></asp:Label>
                            </td>
                            <td class="style7">
                                <asp:DropDownList ID="ddlComplaintRegion" runat="server">
                                    <asp:ListItem Value="0">-Select Region--</asp:ListItem>
                                    <asp:ListItem Value="1">Issue In Affixiation</asp:ListItem>
                                    <asp:ListItem Value="2">Complaint</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="style4">
                            </td>
                        </tr>
                        <tr>
                            <td class="style10">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="Owner Name"></asp:Label>
                                <br />
                            </td>
                            <td class="style8">
                                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                            </td>
                            <td class="style9">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style10">
                                <asp:Label ID="lblRegno" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="RegNo."></asp:Label>
                                <br />
                            </td>
                            <td class="style8">
                                <asp:TextBox ID="txtRegno" runat="server"></asp:TextBox>
                            </td>
                            <td class="style9">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style10">
                                <asp:Label ID="lblEngineNo" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="EngineNo"></asp:Label>
                                <br />
                            </td>
                            <td class="style8">
                                <asp:TextBox ID="txtEngineno" runat="server"></asp:TextBox>
                            </td>
                            <td class="style9">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style5">
                                <asp:Label ID="lblChassisNo" runat="server" Text="ChassisNo" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                            <td class="style7">
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </td>
                            <td class="style4">
                            </td>
                        </tr>
                        <tr>
                            <td class="style5">
                                <asp:Label ID="lblRemarks" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="Remarks"></asp:Label>
                            </td>
                            <td class="style7">
                                <asp:TextBox ID="txtRemarks" runat="server"></asp:TextBox>
                            </td>
                            <td class="style4">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style5">
                                &nbsp;</td>
                            <td class="style7">
                                <asp:Button ID="btnSave" runat="server" Text="Submit" />
                            </td>
                            <td class="style4">
                                &nbsp;</td>
                        </tr>
                    </table>
                                </div>
      
                    
                    
                 
                    
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
