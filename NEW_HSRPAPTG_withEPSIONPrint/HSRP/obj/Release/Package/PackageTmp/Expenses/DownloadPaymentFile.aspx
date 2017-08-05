
<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DownloadPaymentFile.aspx.cs" Inherits="HSRP.Expenses.DownloadPaymentFile" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <style type="text/css">
        .style4 {
            width: 428px;
        }

        .style5 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 103px;
            padding-left: 20px;
        }

        .style6 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 184px;
            padding-left: 20px;
        }

        .style7 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
        }
    </style>

    <script type="text/javascript">

        function Datefrom_OnDateChange(sender, eventArgs) {
            var fromDate = Datefrom.getSelectedDate();
            CalendarDatefrom.setSelectedDate(fromDate);

        }

        function Datefrom_OnChange(sender, eventArgs) {
            var fromDate = CalendarDatefrom.getSelectedDate();
            Datefrom.setSelectedDate(fromDate);

        }

        function Datefrom_OnClick() {
            if (CalendarDatefrom.get_popUpShowing()) {
                CalendarDatefrom.hide();
            }
            else {
                CalendarDatefrom.setSelectedDate(Datefrom.getSelectedDate());
                CalendarDatefrom.show();
            }
        }

        function Datefrom_OnMouseUp() {
            if (CalendarDatefrom.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function Dateto_OnDateChange(sender, eventArgs) {
            var toDate = Dateto.getSelectedDate();
            CalendarDateto.setSelectedDate(toDate);
        }

        function Dateto_OnChange(sender, eventArgs) {
            var toDate = CalendarDateto.getSelectedDate();
            Dateto.setSelectedDate(toDate);
        }

        function Dateto_OnClick() {
            if (CalendarDateto.get_popUpShowing()) {
                CalendarDateto.hide();
            }
            else {
                CalendarDateto.setSelectedDate(Dateto.getSelectedDate());
                CalendarDateto.show();
            }
        }

        function Dateto_OnMouseUp() {
            if (CalendarDateto.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

    </script>
     <div>
        <center>
         <table cellpadding="0" cellspacing="0" border="1" width="700px" height="80px">
             <tr>
                 <%--<td align="left">
                       <asp:Label ID="lbldownloadfiletg" runat="server" Text="TG Region Log File :" style="text-align:center"></asp:Label> 
                 </td>
                 &nbsp;&nbsp;
                 <td>
                      <asp:Button ID="btndownloadTG" runat="server" Text="Download TG Files" OnClick="btndownloadTG_Click"/>
                 </td>--%>
                

                 <td align="left">
                       <asp:Label ID="lbldownloadfileap" runat="server" Text="AP/TG File :" style="text-align:center"></asp:Label> 
                 </td>
                 &nbsp;&nbsp;
                 <td>
                      <asp:Button ID="btndownloadAP" runat="server" Text="Download AP/TG Files" OnClick="btndownloadAP_Click"/>
                 </td>
             </tr>

         </table>
       
 </center>
    </div>
</asp:Content>
