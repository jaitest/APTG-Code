<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AttendanceReport.aspx.cs" Inherits="HSRP.Expenses.AttendanceReport" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
<script type="text/javascript">
 
    function OrderDate_OnDateChange(sender, eventArgs) {
        var fromDate = OrderDate.getSelectedDate();
        CalendarOrderDate.setSelectedDate(fromDate);

    }

    function OrderDate_OnChange(sender, eventArgs) {
        var fromDate = CalendarOrderDate.getSelectedDate();
        OrderDate.setSelectedDate(fromDate);

    }

    function OrderDate_OnClick() {
        if (CalendarOrderDate.get_popUpShowing()) {
            CalendarOrderDate.hide();
        }
        else {
            CalendarOrderDate.setSelectedDate(OrderDate.getSelectedDate());
            CalendarOrderDate.show();
        }
    }

    function OrderDate_OnMouseUp() {
        if (CalendarOrderDate.get_popUpShowing()) {
            event.cancelBubble = true;
            event.returnValue = false;
            return false;
        }
        else {
            return true;
        }
    }

    ////>>>>>> Pollution Due Date

    function HSRPAuthDate_OnDateChange(sender, eventArgs) {
        var fromDate = HSRPAuthDate.getSelectedDate();
        CalendarHSRPAuthDate.setSelectedDate(fromDate);

    }

    function CalendarHSRPAuthDate_OnChange(sender, eventArgs) {
        var fromDate = CalendarHSRPAuthDate.getSelectedDate();
        HSRPAuthDate.setSelectedDate(fromDate);

    }

    function HSRPAuthDate_OnClick() {
        if (CalendarHSRPAuthDate.get_popUpShowing()) {
            CalendarHSRPAuthDate.hide();
        }
        else {
            CalendarHSRPAuthDate.setSelectedDate(HSRPAuthDate.getSelectedDate());
            CalendarHSRPAuthDate.show();
        }
    }

    function HSRPAuthDate_OnMouseUp() {
        if (CalendarHSRPAuthDate.get_popUpShowing()) {
            event.cancelBubble = true;
            event.returnValue = false;
            return false;
        }
        else {
            return true;
        }
    }
    </script>
    <script language="javascript" type="text/javascript">

         

        function validate() {

           <%-- if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "Select Location--") {
                alert("Select Location");
                document.getElementById("<%=dropDownListClient.ClientID%>").focus();
                return false;
            }--%>

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
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
                                        <span class="headingmain">Attendance Report</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap="nowrap">
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
                               
                                 <td  class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                 </td>

                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" Width="102px"  >
                                        </asp:DropDownList>
                                    </td>
                                    </tr>
                                <tr>

                                    <td class="form_text" nowrap="nowrap">
                                        <asp:Label Text="District Name:" runat="server" ID="Label3" Visible="true"/>
                                    </td>
                                    <td style="width:95px">                                         
                                           <asp:DropDownList ID="ddldistrictname" runat="server" Style="margin-left: 5px" TabIndex="3" Width="171px" DataTextField="district" DataValueField="district" AutoPostBack="true" OnSelectedIndexChanged="ddldistrictname_SelectedIndexChanged" >                                             
                                          </asp:DropDownList>
                                    </td>
                                    </tr>


                                    <td class="form_text" nowrap="nowrap">
                                            <asp:Label Text="Location Name:" runat="server" ID="lbllocation" Visible="true"/>
                                        </td>
                                         <td style="width:95px">
                                           <asp:DropDownList ID="ddllocation" runat="server" Style="margin-left: 5px" TabIndex="3" Width="171px" DataTextField="RTOLocationName" 
                                                    DataValueField="RTOLocationID" AutoPostBack="true" OnSelectedIndexChanged="ddllocation_SelectedIndexChanged">
                                             
                                          </asp:DropDownList>
                                     <td class="form_text" nowrap="nowrap">
                                        <asp:Label Text="EmpName:" Visible="true" runat="server" ID="labelemp" />&nbsp;
                                    </td>
                                      
                                         <td style="width:95px">
                                           <asp:DropDownList ID="ddlUserAccount" runat="server" Style="margin-left: 5px" TabIndex="3" Width="171px" AutoPostBack="true">                                             
                                          </asp:DropDownList>
                                         </td>
                                             
                                    </td>

                                     <td valign="middle" class="form_text" nowrap="nowrap" align="right">
                                                   
                                                    <asp:Label Text="Month :" Visible="true" runat="server" ID="label2" />
                                                    &nbsp;&nbsp;
                                                </td>
                                   
                                        <td valign="middle"  align="left">
                                                    <asp:DropDownList ID="ddlMonth" runat="server">
                                                        <asp:ListItem Value="01">January</asp:ListItem>
                                                        <asp:ListItem Value="02">February</asp:ListItem>
                                                        <asp:ListItem Value="03">March</asp:ListItem>
                                                        <asp:ListItem Value="04">April</asp:ListItem>
                                                        <asp:ListItem Value="05">May</asp:ListItem>
                                                        <asp:ListItem Value="06">June</asp:ListItem>
                                                        <asp:ListItem Value="07">July</asp:ListItem>
                                                        <asp:ListItem Value="08">August</asp:ListItem>
                                                        <asp:ListItem Value="09">September</asp:ListItem>
                                                        <asp:ListItem Value="10">October</asp:ListItem>
                                                        <asp:ListItem Value="11">November</asp:ListItem>
                                                        <asp:ListItem Value="12">December</asp:ListItem>
                                                    </asp:DropDownList>

                                       </td>
                                     <td valign="middle" class="form_text" nowrap="nowrap" align="right">
                                                    <asp:Label Text="Year:" Visible="true" runat="server" ID="label1" />
                                                   &nbsp;&nbsp;
                                                </td>
                                                <td valign="middle"  align="left">                                                    
                                                    <asp:DropDownList ID="ddlYear" runat="server">
                                                       <%-- <asp:ListItem>2012</asp:ListItem>
                                                        <asp:ListItem>2013</asp:ListItem>
                                                        <asp:ListItem>2014</asp:ListItem>
                                                        <asp:ListItem>2015</asp:ListItem>--%>
                                                        <asp:ListItem>2016</asp:ListItem>
                                                        <asp:ListItem>2017</asp:ListItem>
                                                        <asp:ListItem>2018</asp:ListItem>
                                                        <asp:ListItem>2019</asp:ListItem>
                                                        <asp:ListItem>2020</asp:ListItem>
                                                        <asp:ListItem>2021</asp:ListItem>
                                                        <asp:ListItem>2022</asp:ListItem>
                                                    </asp:DropDownList>
                                                  
                                                </td>
                                 
                                    <%-- <td nowrap="nowrap">&nbsp; <asp:Label Text="From:" Visible="true" ForeColor="Black" runat="server" ID="labelDate" /> &nbsp;&nbsp;</td>
                                    <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left">
                                                                <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                            </td>
                                                           
                                                            <td valign="top" align="left">
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>
                                   <td nowrap="nowrap"> &nbsp;<asp:Label Text="To:" Visible="true" ForeColor="Black" runat="server" ID="labelTO" /> &nbsp;&nbsp;</td>
                                   <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()" align="left">
                                                                <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                            </td>
                                                      
                                                            <td valign="top" align="left">
                                                                <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                                    class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>--%>

                                    
                                    <%--<td style="Position:relative; right:0px; top: 0px;"> --%>
                                <td valign="middle" class="form_text" nowrap="nowrap" align="right">
                                        
                                        <asp:Button ID="btnExportToExcel" runat="server" Text="Attendance Report" ToolTip="Please Click for Report"
                                            class="button"  
                                            onclick="btnExportToExcel_Click"    />
                                    </td>
                                   <%-- <td>
                                                                                      <asp:Button ID="btnAllLocationPdf" runat="server"
                                                        Text="For All Location" ToolTip="Please Click for Report"
                                                        class="button" onclick="btnAllLocationPdf_Click" Visible="false"/>
                                                </td>--%>
                                    <tr>
                                    <td colspan="9">
                                    <asp:Label ID="lblErrMess" runat="server" Font-Names="Tahoma" ForeColor="Red"></asp:Label>
                                    </td>
                                    </tr>

                                   <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
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
                               
                                 
                            </table>
                        </td>
                    </tr>
                    
                               <%-- <tr>
                                                <td colspan="6">
                                                    <ComponentArt:Calendar runat="server" ID="CalendarOrderDate" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <ComponentArt:Calendar runat="server" ID="CalendarHSRPAuthDate" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="ImgPollution" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="CalendarHSRPAuthDate_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                            </tr> --%>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
