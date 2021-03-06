﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="GST_APChalan_New_Change.aspx.cs" Inherits="HSRP.EmbossingData.GST_APChalan_New_Change" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>

    <style type="text/css">
        .button-success {
            background: rgb(28, 184, 65); /* this is a green */
        }
    </style>


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

        function validatevalue() {
            var temp = true;
            if ($('#txtTransporter').val() == '') {
                alert('Please Enter Transporter value');
                temp = false;
                return temp;
            }
            if ($('#txtLorryNo').val() == '') {
                alert('Please enter Lorry No');
                temp = false;
                return temp;

            }
            var tempvalues = checkcheckbox();
            if (tempvalues == true) {
                alert('please check at least one');
                temp = false;
            }
            return temp;
        }

        function checkcheckbox() {
            var ischeck = true;
            $('.testing input:checkbox').each(function () {

                if ($(this).prop("checked") == true) {
                    ischeck = false;
                    return false;

                }
                else {
                    ischeck = true;


                }

            })
            return ischeck;
        }

        $(document).ready(function () {

        })
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>
    <script language="javascript" type="text/javascript">
        function validateStatus() {


        }

    </script>

    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select RTO Name--") {
                alert("Please Select RTO Name");
                document.getElementById("<%=dropDownListClient.ClientID%>").focus();
                return false;
            }

        }
        function HideShow() {
            $('#btnChalan').hide();
        }
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
                                        <asp:Label ID="Label4" class="headingmain" runat="server">Challan/Invoice for Embossed Plates</asp:Label>

                                    </td>
                                </tr>

                                <tr id="TRRTOHide" runat="server">
                                    <td>
                                      
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
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" nowrap="nowrap" class="form_text">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                                            OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" />
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                       
                                        <asp:DropDownList Visible="true" ID="dropDownListClient" CausesValidation="false" Width="140px"
                                            runat="server" DataTextField="RTOLocationName" AutoPostBack="false"
                                            DataValueField="RTOLocationID">
                                        </asp:DropDownList>
                                      
                                    </td>

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="From:" runat="server" ID="labelDate" />
                                        &nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text" nowrap="nowrap" onmouseup="OrderDate_OnMouseUp()">
                                        <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                        <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                            onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" /></td>


                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="To:" runat="server" ID="labelTO" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap" onmouseup="HSRPAuthDate_OnMouseUp()">
                                        <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                        <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                            class="calendar_button" src="../images/btn_calendar.gif" />
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">

                                        <asp:Button ID="btnGO" Width="58px" runat="server"
                                            Text="GO" ToolTip="Please Click for Report" BackColor="Orange" ForeColor="#000000"
                                            class="button" OnClientClick=" return validate()"
                                            OnClick="btnGO_Click" />
                                        &nbsp;&nbsp;&nbsp;  

                                             &nbsp;&nbsp;&nbsp;  
                                         
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="bottom" class="form_text" nowrap="nowrap">&nbsp;</td>
                                    <td valign="bottom">&nbsp;</td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;</td>
                                    <td valign="middle">&nbsp;</td>
                                    <td dir="ltr" valign="middle" class="form_text">&nbsp;</td>
                                    <td align="left" valign="middle">&nbsp;</td>
                                    <td colspan="2">&nbsp;</td>
                                    <td align="left" valign="middle">&nbsp;</td>
                                    <td align="left" valign="middle">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp; <asp:HiddenField ID="hdnfldinvoiceno" runat="server" /></td>
                                    <td valign="top">&nbsp; </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap" align="left">Transporter:<span style="color: Red">*</span>
                                        <br />
                                        <span>&nbsp;</span></td>
                                    <td valign="middle" class="form_text" nowrap="nowrap" align="left">&nbsp; 
                                    <asp:TextBox ClientIDMode="Static" ID="txtTransporter" runat="server"></asp:TextBox>
                                        <br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtTransporter" ErrorMessage="Enter transporter name" ValidationGroup="challan"></asp:RequiredFieldValidator>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap" align="left">Lorry No:<span style="color: Red">*</span>
                                        <br />
                                        <span style="color: Red">&nbsp;</span></td>
                                    <td valign="middle" class="form_text" nowrap="nowrap" align="left">
                                        <asp:TextBox ClientIDMode="Static" ID="txtLorryNo" runat="server"></asp:TextBox><br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLorryNo" ErrorMessage="Enter lorry no." ValidationGroup="challan"></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="left" valign="middle">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>


            </td>
            <td dir="ltr" valign="middle" class="form_text"></td>
            
        </tr>
        
        <tr>
            <td colspan="4">
                <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="red" />
                <asp:Label ID="LblMessage" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="blue" />
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">

                    <tr>
                        <td align="center" style="padding: 10px">
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" AutoGenerateColumns="false"
                                PageSize="25" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3"
                                DataKeyNames="hsrprecordid">
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <PagerSettings Visible="False" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Select
                                           <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" OnCheckedChanged="CHKSelect1_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox CssClass="testing" ClientIDMode="Static" ID="CHKSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            S.No
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="SrNo" runat="server" Text='<%#Eval("SNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Authorization No
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="AuthorizationNo" runat="server" Text='<%#Eval("HSRPRecord_AuthorizationNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Vehicle Reg No
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVehicleRegNo" runat="server" Text='<%#Eval("VehicleRegNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            F Laser Code
                                        </HeaderTemplate>
                                        <ItemTemplate>

                                            <asp:TextBox ID="txtFLaserCode" runat="server" Text='<%#Eval("hsrp_front_lasercode") %>' Enabled="false"></asp:TextBox>
                                          
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            R Laser Code
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRlaserCode" runat="server" Text='<%#Eval("hsrp_rear_lasercode") %>' Enabled="false"></asp:TextBox>
                                          
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="id" runat="server" Text='<%#Eval("hsrprecordid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Order Status
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderStatus" runat="server" Text='<%#Eval("OrderStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="padding-top: 10px">
                            <asp:Button ID="btnChalan" ClientIDMode="Static" runat="server" Text="1. Generate and Download Challan/Invoice" BackColor="Orange" ForeColor="#000000" autoposback="true" Visible="false" OnClientClick="return HideShow();" OnClick="btnChalan_Click" ValidationGroup="challan" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         
                        &nbsp;&nbsp;
                        <asp:Button ID="btnrecordinpdf" runat="server" BackColor="Orange" ForeColor="#000000" OnClick="Button1_Click" Text="2. Download Challan/Invoice Summary" Visible="false" />
                            &nbsp;&nbsp;
                        </td>
                        <td align="center" style="padding-top: 10px"></td>
                    </tr>
                    <tr>
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
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>
