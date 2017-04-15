<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="EmbCenterconsumption.aspx.cs" Inherits="HSRP.Transaction.EmbCenterconsumption" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#ddlErpProductCode").change(function () {
                //alert("Selected");
                $('#lblProcessing').show();
            });
        });
    </script>
    <script type="text/javascript">
        function HideLabel() {
            document.getElementById("<%=lblProcessing.ClientID %>").style.display = "none";
        };
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>

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
    <script type="text/javascript" language="javascript">
        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change Secure Devices status?")) {

                return true;
            }
            else {
                return false;
            }

        }
    </script>
    <script language="javascript" type="text/javascript">
        
    </script>
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr id="TR1" runat="server">
                                    <td>
                                        <asp:Label ID="Label4" class="headingmain" runat="server">Shipment Received</asp:Label>
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0" class="topheader">

                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Date:" Visible="true" runat="server" ID="Label2" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="bottom" onmouseup="OrderDate_OnMouseUp()" align="left" class="span3" style="width: 106px">
                                        <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker" Height="26px"
                                            PopUpExpandDirection="BelowRight">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </td>
                                    <td valign="middle" align="left" class="span3" style="width: 36px">
                                        <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                            onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>

                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Item:" Visible="true" runat="server" ID="Label1" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList ID="DropDownList1" runat="server">
                                            <asp:ListItem>--Select Item--</asp:ListItem>
                                            <asp:ListItem>RM0014</asp:ListItem>
                                            <asp:ListItem>RM0015</asp:ListItem>
                                            <asp:ListItem>RM0016</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DropDownList1" ErrorMessage="Please Select Item" InitialValue="--Select Item--"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>

                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Quantity:" Visible="true" runat="server" ID="lblQuantity" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtQuantity" runat="server" MaxLength="20"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Please Enter Quantity"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>

                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label ID="lblErrMsg" runat="server" Text="" />
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text"></td>
                                    <td valign="middle" class="form_text">
                                        

                                    </td>
                                    <td valign="middle" class="form_text">
                                        <asp:Label ID="lblProcessing" ClientIDMode="Static" Visible="true" runat="server" Style="display: none;" Text="Processing..."></asp:Label>

                                    </td>
                                    <td valign="middle" class="form_text"></td>
                                    <td valign="middle" class="form_text"></td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text"></td>
                                    <td valign="middle" class="form_text"></td>
                                                <td>
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
                                    <td valign="middle" class="form_text"></td>
                                    <td valign="middle" class="form_text"></td>
                                            </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>
