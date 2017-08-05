<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="APTG_Monthly_Sales_Report.aspx.cs" Inherits="HSRP.Report.APTG_Monthly_Sales_Report" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script language="javascript" type="text/javascript">



        function validate() {


            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State Name");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
        } 
    </script>

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
   
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">Andhra Pradesh: Monthly Report form Embossing Stations to Registering Authority</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td>
                                        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                            <tr>
                                                <td>
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 100px">
                                                    <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization" />
                                                </td>
                                                <td valign="middle" style="width: 200px">
                                                    <asp:DropDownList CausesValidation="false" ID="DropDownListStateName"
                                                        runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID">
                                                    </asp:DropDownList>
                                                </td>
                                       
                                                 <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:Label Text="From:" runat="server" ID="labelDate" Font-Bold="True" ForeColor="Black" Width="48px" />
                                        </td>
                                         <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <ComponentArt:Calendar ID="Datefrom" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="Datefrom_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                    <img id="calendar_from_button" alt="" onclick="Datefrom_OnClick()"
                                                        onmouseup="Datefrom_OnMouseUp()" class="calendar_button"
                                                        src="../images/btn_calendar.gif" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                                <td valign="top" align="left">
                                                     </td>
                                                <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:Label Text="To:" runat="server" ID="label3" Font-Bold="True"  ForeColor="Black" />
                                        </td>
                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <ComponentArt:Calendar ID="Dateto" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="Dateto_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                    <img id="calendar_to_button" alt="" onclick="Dateto_OnClick()"
                                                        onmouseup="Dateto_OnMouseUp()" class="calendar_button"
                                                        src="../images/btn_calendar.gif" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                                <td valign="top" align="left">
                                           
                                                </td>
                                                <td style="position: relative; right: 0px; top: 0px;">
                                                    <asp:Button ID="btnExportToExcel" runat="server" Text="Report In Excel" ToolTip="Please Click for Report"
                                                        class="button"  OnClick="btnExportToExcel_Click" />
                                                </td>
                                                <tr>
                                                    <td colspan="9">
                                                        <asp:Label ID="LabelError" runat="server" Font-Names="Tahoma" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                        
                                            </tr>
                                        </table>
                                    </td>

                                </tr>

                            </table>
                        </td>

                    </tr>


                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                 <tr>
                            <td width="80" colspan="4">&nbsp;</td>
                            <td colspan="3" align="left">
                                <ComponentArt:Calendar runat="server" ID="CalendarDatefrom" AllowMultipleSelection="false"
                                    AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                    PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                    DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                    OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                    SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                    MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                    ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif"
                                    NextImageUrl="cal_nextMonth.gif" Height="172px" Width="200px">
                                    <ClientEvents>
                                        <SelectionChanged EventHandler="Datefrom_OnChange" />
                                    </ClientEvents>
                                </ComponentArt:Calendar>
                            </td>
                            <td colspan="3" align="left">
                                <ComponentArt:Calendar runat="server" ID="CalendarDateto" AllowMultipleSelection="false"
                                    AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                    PopUp="Custom" PopUpExpandControlId="calendar_to_button" CalendarTitleCssClass="title"
                                    DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                    OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                    SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                    MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                    ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif"
                                    NextImageUrl="cal_nextMonth.gif" Height="172px" Width="200px">
                                    <ClientEvents>
                                        <SelectionChanged EventHandler="Dateto_OnChange" />
                                    </ClientEvents>
                                </ComponentArt:Calendar>
                            </td>
                        </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                          
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                           
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
