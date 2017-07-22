<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Downloadlogfile.aspx.cs" Inherits="APCollectionWeb.Downloadlogfile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
         <table cellpadding="0" cellspacing="0" border="1" width="700px" height="80px">
             <tr>
                 <td align="left">
                       <asp:Label ID="lbldownloadfiletg" runat="server" Text="TG Region Log File :" style="text-align:center"></asp:Label> 
                 </td>
                 &nbsp;&nbsp;
                 <td>
                      <asp:Button ID="btndownloadTG" runat="server" Text="Download TG Files" OnClick="btndownloadTG_Click"/>
                 </td>
                 &nbsp;&nbsp;&nbsp;&nbsp;

                 <td align="left">
                       <asp:Label ID="lbldownloadfileap" runat="server" Text="AP Region Log File :" style="text-align:center"></asp:Label> 
                 </td>
                 &nbsp;&nbsp;
                 <td>
                      <asp:Button ID="btndownloadAP" runat="server" Text="Download AP Files" OnClick="btndownloadAP_Click"/>
                 </td>
             </tr>

         </table>
       
 </center>
    </div>
    </form>
</body>
</html>
