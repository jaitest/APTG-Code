﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EmbossingStationtoRegisteringAuthority.aspx.cs" Inherits="HSRP.Report.EmbossingStationtoRegisteringAuthority" %>
<%@ Register assembly="ComponentArt.Web.UI" namespace="ComponentArt.Web.UI" tagprefix="ComponentArt" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
     <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />


    <script type="text/javascript">

        function OrderDatefrom_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDatefrom.getSelectedDate();
            CalendarOrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDatefrom.getSelectedDate();
            OrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnClick() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                CalendarOrderDatefrom.hide();
            }
            else {
                CalendarOrderDatefrom.setSelectedDate(OrderDatefrom.getSelectedDate());
                CalendarOrderDatefrom.show();
            }
        }

        function OrderDatefrom_OnMouseUp() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function OrderDateto_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDateto.getSelectedDate();
            CalendarOrderDateto.setSelectedDate(fromDate);

        }

        function OrderDateto_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDateto.getSelectedDate();
            OrderDateto.setSelectedDate(fromDate);

        }


        function OrderDateto_OnClick() {
            if (CalendarOrderDateto.get_popUpShowing()) {
                CalendarOrderDateto.hide();
            }
            else {
                CalendarOrderDateto.setSelectedDate(OrderDateto.getSelectedDate());
                CalendarOrderDateto.show();
            }
        }



        function OrderDateto_OnMouseUp() {
            if (CalendarOrderDateto.get_popUpShowing()) {
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
                                        <span class="headingmain">Embossing Station To Registering Authority</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                            </td>
                            </tr>
                            </table>
                            </td>
                            </tr>
                            </table>

   
    <table style="width: 100%; height: 51px;">
        <tr>
            <td style="height: 25px;" width="20">
                &nbsp;</td>
            <td style="height: 25px; width: 80px;" valign="bottom">
                
                        <asp:Label ID="labelOrganization1" runat="server" Font-Bold="True" 
                            ForeColor="Black" Text="HSRP State:" Width="80px" />
                            </td>
            <td style="height: 25px; width: 266px;" valign="bottom">
                
                        <asp:DropDownList ID="ddlState" runat="server" 
                            CausesValidation="false" DataTextField="HSRPStateName" 
                            DataValueField="HSRP_StateID" Height="22px" 
                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" 
                            Width="125px" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="ddlState" ErrorMessage="Select State" 
                            InitialValue="--Select State--" Width="70px"></asp:RequiredFieldValidator>
                            </td>
                              <td style="height: 25px;" valign="middle" width="90">
                
                        <asp:Label ID="labelOrganization2" runat="server" Font-Bold="True" 
                            ForeColor="Black" Text="RTO Location" Width="90px" />
                        </td>
            <td style="height: 25px;" valign="middle" width="195">
                
                        <asp:DropDownList ID="ddlRtoLocation" runat="server" AutoPostBack="True" 
                            DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="ddlRtoLocation" ErrorMessage="Select Location" 
                            InitialValue="--Select RTO Name--" Width="90px"></asp:RequiredFieldValidator>
            </td>
                        <td width="30">
                    <asp:Label ID="labelDate" runat="server" Font-Bold="True" ForeColor="Black" 
                        Text="Date" Width="30px" />
                            </td>
                        <td width="100">
                            <ComponentArt:Calendar ID="OrderDatefrom" runat="server" ControlType="Picker" 
                                PickerCssClass="picker" PickerCustomFormat="dd/MM/yyyy" 
                                PickerFormat="Custom">
                                <ClientEvents>
                                    <SelectionChanged EventHandler="OrderDatefrom_OnDateChange" />
                                </ClientEvents>
                            </ComponentArt:Calendar>
                    </td>
           
                        <td width="30">
                    <img id="calendar_from_button"  alt="" onclick="OrderDatefrom_OnClick()"
                                                                    onmouseup="OrderDatefrom_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" /></td>
           
                        
                                                <td style="height: 25px" width="40">
                                                    
                        <asp:Button ID="btnexport" runat="server" Font-Bold="True" ForeColor="#3333FF" 
                            onclick="btnexport_Click" Text="Export" Width="70px" />
                   
            </td>
        </tr>
        <td width="20">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td colspan="2">
            <asp:Label ID="Label1" runat="server" ForeColor="#CC3300"></asp:Label>
        </td>
        <tr>
             <td width="20">
                                                    &nbsp;</td>
             <td>
                                                    &nbsp;</td>
             <td colspan="5">
                                                    &nbsp;</td>
             <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td width="20">
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                <td colspan="2">
                                                    <ComponentArt:Calendar ID="CalendarOrderDatefrom" runat="server" 
                                                        AllowMonthSelection="false" AllowMultipleSelection="false" 
                                                        AllowWeekSelection="false" CalendarCssClass="calendar" 
                                                        CalendarTitleCssClass="title" ControlType="Calendar" DayCssClass="day" 
                                                        DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover" 
                                                        DayNameFormat="FirstTwoLetters" DisabledDayCssClass="disabledday" 
                                                        DisabledDayHoverCssClass="disabledday" ImagesBaseUrl="../images" 
                                                        MonthCssClass="month" NextImageUrl="cal_nextMonth.gif" 
                                                        NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday" 
                                                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" 
                                                        PrevImageUrl="cal_prevMonth.gif" SelectedDayCssClass="selectedday" 
                                                        SwapDuration="300" SwapSlide="Linear">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDatefrom_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
        </tr>
                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                            
                                            
                                        </table>
    
                                            
</asp:Content>

