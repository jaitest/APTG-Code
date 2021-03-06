﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Batch.aspx.cs" Inherits="HSRP.Master.Batch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
     
        function validateState() {

      
            if (document.getElementById("<%=DropDownListPrefix.ClientID%>").value == "-- Select Prefix --") {
                alert("Please Provide Prefix Lode");
                document.getElementById("<%=DropDownListPrefix.ClientID%>").focus();
                return false;
            }
        }

        function validate() {
          
            if (document.getElementById("<%=DropDownListProductName.ClientID%>").value == "-- Select Product --") {
                alert("Select Product");
                document.getElementById("<%=DropDownListProductName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=textboxBatchCode.ClientID%>").value == "") {
                alert("Please Provide Batch Code");
                document.getElementById("<%=textboxBatchCode.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=DropDownListPrefix.ClientID%>").value == "-- Select Prefix --") {
                alert("Please Provide Prefix Lode");
                document.getElementById("<%=DropDownListPrefix.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textboxWeight.ClientID%>").value == "") {
                alert("Please Provide Batch Weight");
                document.getElementById("<%=textboxWeight.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").value == "") {
                alert("Please Provide Laser Code");
                document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textboxLaserCodeTo.ClientID%>").value == "") {
                alert("Please Provide Laser Code");
                document.getElementById("<%=textboxLaserCodeTo.ClientID%>").focus();
                return false;
            }


            var LaserCodeFrom = document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").value - 1;
            var LaserCodeTo = document.getElementById("<%=textboxLaserCodeTo.ClientID%>").value;
            var totalPlate = LaserCodeTo - LaserCodeFrom;


            if (invalidChar(document.getElementById("textboxBatchCode"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxBatchCode").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textboxLaserCodeFrom"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxLaserCodeFrom").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textboxLaserCodeTo"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxLaserCodeTo").focus();
                return false;
            }


            if (invalidChar(document.getElementById("textboxbatchRelesedDate"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxbatchRelesedDate").focus();
                return false;
            }
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

        }
    
    </script>
    <script type="text/javascript">
        function uppercharacter() {
            var char = document.getElementById("textboxPrefixFrom").value;
            alert(document.write(char.toUpperCase()));
        }
    
    </script>
    <script type="text/javascript">
        function OrderDate_OnDateChange(sender, eventArgs) {

            var fromDate = OrderDate.getSelectedDate();
            alert(fromDate);
            CalendarOrderDate.setSelectedDate(fromDate);

        }

        function OrderDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDate.getSelectedDate();
            //alert(fromDate);
            var toDate = new Date()
            // alert(currentTime);
            OrderDate.setSelectedDate(fromDate);
            if (fromDate > toDate) {
                alert("Check Selected Date");
                OrderDate.setSelectedDate(toDate);
                return false;
            }

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
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 20px; background-color: #FFFFFF;" align="left">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    <asp:Label ID="LabelFormName" runat="server" Text="Label"></asp:Label>
                </div>
            </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                        <tr>
                            <td class="form_text" style="padding-bottom: 10px">
                                Product Name : <span class="alert">* </span>
                            </td>
                            &nbsp; &nbsp;
                            <td>
                                <asp:DropDownList ID="DropDownListProductName" Width="180" runat="server" DataTextField="ProductCode"
                                    DataValueField="ProductID" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td class="form_text" style="padding-bottom: 10px">
                                Batch Code : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="textboxBatchCode" class="form_textbox" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text" style="padding-bottom: 10px">
                                Prefix Laser Code From : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownListPrefix" Width="140" runat="server" DataTextField="Prefix"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td class="form_text" style="padding-bottom: 10px">
                                Laser Code From : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="textboxLaserCodeFrom" class="form_textbox" MaxLength="15" onkeypress="return isNumberKey(event)"
                                    runat="server" TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text" style="padding-bottom: 10px">
                                Weight : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="textboxWeight" class="form_textbox" onkeypress="return isNumberKey(event)"
                                    runat="server" TabIndex="5"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td class="form_text" style="padding-bottom: 10px">
                                Laser Code To : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="textboxLaserCodeTo" class="form_textbox" MaxLength="15" onkeypress="return isNumberKey(event)"
                                    runat="server" TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text" style="padding-bottom: 10px">
                                Date of Manufacturing : <span class="alert">* </span>
                            </td>
                            <td valign="top" onmouseup="OrderDate_OnMouseUp()">
                                &nbsp;&nbsp;
                                <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                    ControlType="Picker" PickerCssClass="picker">
                                    <ClientEvents>
                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                    </ClientEvents>
                                </ComponentArt:Calendar>
                                &nbsp; &nbsp;
                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                            </td>
                            <td>
                            </td>
                            <td class="form_text" style="padding-bottom: 10px">
                                <span class="alert">&nbsp;</span>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text" style="padding-bottom: 10px">
                                Remarks :
                            </td>
                            <td colspan="4" valign="top" onmouseup="HSRPAuthDate_OnMouseUp()">
                                <asp:TextBox ID="textboxRemarks" class="form_textbox" runat="server" Columns="25"
                                    Rows="15" Height="85px" Width="600px" TabIndex="6" TextMode="MultiLine"></asp:TextBox>
                                <br />
                                <%--<ComponentArt:Calendar ID="HSRPAuthDate" runat="server" Visible="false" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>--%>
                                &nbsp;&nbsp;
                                <img id="ImgPollution" alt="" class="calendar_button" style="visibility: hidden"
                                    onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()" src="../images/btn_calendar.gif"
                                    tabindex="4" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelUpdated" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                <asp:Label ID="lblTotalRecord" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                            </td>
                        </tr>
                        <td align="left" colspan="3" align="right">
                            <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                            <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        </td>
                        <td colspan="2" align="right">
                            <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8" OnClientClick=" return validateState()()"
                                class="button" OnClick="buttonUpdate_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" class="button" OnClientClick=" return validate()()"
                                OnClick="btnSave_Click" />&nbsp;&nbsp;
                            <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                id="buttonClose" value="Close" class="button" />
                            &nbsp;&nbsp;
                            <input type="reset" class="button" value="Reset" />
                        </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblExist" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                <asp:TextBox ID="TextBoxLaserNoError" runat="server" Rows="10" Columns="17" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
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
                            <td colspan="5">
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
                        </tr>
                    </table>
                </div>
            </div>
        </fieldset>
    </div>
    </form>
</body>
</html>
