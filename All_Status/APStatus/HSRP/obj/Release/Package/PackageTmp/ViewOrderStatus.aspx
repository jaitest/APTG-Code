<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewOrderStatus.aspx.cs" Inherits="HSRP.ViewOrderStatus" %>

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



        

    
</head>
<body style="background-color: #F0F0F0">
    <form id="form1" runat="server">
    <div>
           <%--    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" height="75px" style="background-image: url(images/ntopbgheader.png)">
  <tr>
    <td width="50%" class="topheaderboldtext">LINK UTSAV AUTO SYSTEMS PVT. LTD. </td>
    <td width="50%" valign="middle">&nbsp;</td>
  </tr>
</table>--%>

<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" >
  <tr>
    <td valign="middle" bgcolor="#576e8c"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="30">&nbsp;</td>
        <td width="40%">&nbsp;</td>
      </tr>
    </table>
    
    
    </td>
  </tr>
</table>
                    <br />
                    <table width="100%" style="height:400px; border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top">
                               <%-- <asp:ContentPlaceHolder ID="ContentPlaceHolder1" runat="server">
                                </asp:ContentPlaceHolder>--%>

                               <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
            <div id="prin" >
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                             </td>
                    </tr>
                  
                    <tr>
                   
                        <td>
                            <br />
                           
                            <div align="center">
                                <table id="show" runat="server">
                                    <tr>
                                        <td>
                                        
                                            <table>
                                            <tr><td>
                                                <div id="test" >





 
  <tr>
    <td><table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
      <tr>
        <td valign="top"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td height="27" background="images/midboxtopbg.jpg"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <%--<td height="26"><span class="headingmain">HSRP Status Report--%>
                <td height="26"><span class="headingmain">HSRP Status Report</span></td>
              </tr>
            </table></td>
          </tr>
          <tr>
          <td align="left" valign="middle" nowrap="nowrap" >
          <asp:Label style="padding-left:20px" ID="Label1" Visible="true" runat="server" Text="Order Status" Font-Bold="true" ForeColor="Black" Font-Size="Medium"></asp:Label> 
                        <asp:Label style="padding-left:20px" ID="yy" Visible="true" runat="server" Text="" Font-Bold="true" ForeColor="Black" Font-Size="Medium"></asp:Label> 
          
          </td>
          </tr>
          <tr>
          
            <td id="TDR" runat="server">
            <br />
              <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                  <td valign="top" bgcolor="#c0c0c0"><table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                    <tr>
                      <td height="30" valign="top" nowrap="nowrap" bgcolor="#94979c" class="midboxtop">Registration Number</td>
                      <td valign="top" bgcolor="#94979c" nowrap="nowrap" class="midboxtop" >RTO Center/RTO Code</td>
                      <td valign="top" nowrap="nowrap" bgcolor="#94979c" class="midboxtop" >Supervisor Name</td>
                      <td valign="top" nowrap="nowrap" bgcolor="#94979c" class="midboxtop" >RTO/Affixation Address</td>
                      <td valign="top" nowrap="nowrap" bgcolor="#94979c" class="midboxtop" >Mobile No</td>
                      <td valign="top" nowrap="nowrap" bgcolor="#94979c" class="midboxtop" >Landline No</td>
                      <td valign="top" nowrap="nowrap" bgcolor="#94979c" class="midboxtop" >Email id</td>
                    </tr>
                    <tr>
                      <td height="26" bgcolor="#e4e5e5" class="maintext" style="text-transform:uppercase"> <asp:Label ID="LabelVehicleNo" Font-Size="Small" runat="server" Text=""  class="maintext"></asp:Label> </td>
                                                                             <%--   <asp:Label  ID="LabelStatus" Font-Size="Small" runat="server" Text="" bgcolor="#e4e5e5" class="maintext"></asp:Label>--%>
 </td>
                      <td bgcolor="#e4e5e5" class="maintext">   <asp:Label Font-Size="Small" ID="LabelRTOCenter" runat="server" Text="" bgcolor="#e4e5e5" class="maintext"></asp:Label>
                                                                / <asp:Label Font-Size="Small" ID="LabelRTOCode" runat="server" Text="" bgcolor="#F9F9F9" class="maintext"></asp:Label> </td>
                      <td bgcolor="#e4e5e5" class="maintext">                                                             <asp:Label Font-Size="Small" ID="LabelContactPersonName" runat="server" Text="" bgcolor="#FFFFFF" class="maintext"></asp:Label>
 </td>
                      <td bgcolor="#e4e5e5" class="maintext">                                                            <asp:Label Font-Size="Small" ID="LabelAddress" runat="server" Text="" bgcolor="#FFFFFF" class="maintext"></asp:Label>
 </td>
                      <td bgcolor="#e4e5e5" class="maintext">                                                             <asp:Label Font-Size="Small" ID="LabelContactNo" runat="server" Text="" bgcolor="#FFFFFF" class="maintext"></asp:Label>
</td>
                      <td bgcolor="#e4e5e5" class="maintext">                                                             <asp:Label Font-Size="Small" ID="LabelLandlineno" runat="server" Text="" bgcolor="#FFFFFF" class="maintext"></asp:Label>
 </td>
                      <td bgcolor="#e4e5e5" class="maintext">                                                             <asp:Label Font-Size="Small" ID="LabelEmail" runat="server" Text="" bgcolor="#FFFFFF" class="maintext"></asp:Label>
 </td>
                    </tr>
                  </table></td>
                </tr>
              </table></td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
   </table>
                                                
                                                </td></tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                       
                     <table width="100%" style="height:60px" border="0" align="center" cellpadding="0" cellspacing="0"
                        class="marqueelinebg">
                        <tr>
                           <td style="color: Black; font: normal 15px tahoma, arial, verdana;" valign="middle"
                                align="center">
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
                    
                   <%-- <table width="100%">
                                                <tr>
                                                <td>
                                                    <asp:Button ID="Button1" runat="server" Text="Go Back" CssClass="button"   Font-Size="Medium"  onclick="Button1_Click"  />
                                                    </td>
                                                <td align="right">
                                                  <asp:Button ID="Button2" runat="server" Text="Print" CssClass="button" OnClientClick="printSelection(document.getElementById('prin'));return false" Font-Size="Medium" />
                                                   
                                                    </td>
                                                
                                                </tr>

                                                </table>--%>
                 
                    <%--<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0"  class="footerbottoms">
                    <tr>
                    
                    </tr>

                        </table>--%>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
