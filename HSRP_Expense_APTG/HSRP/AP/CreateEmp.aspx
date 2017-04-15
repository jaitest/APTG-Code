<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CreateEmp.aspx.cs" Inherits="HSRP.AP.CreateEmp" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />--%>
    
    <script language="javascript" type="text/javascript">

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
            else
            {
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

        function validate() {
            if (document.getElementById("<%=ddlstate.ClientID%>").value == "--Select State--") {
                alert("Please select State");
                document.getElementById("<%=ddlstate.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlrtolocation.ClientID%>").value == "--Select RTO--") {
                alert("Please select Rto Location ");
                document.getElementById("<%=ddlrtolocation.ClientID%>").focus();
                return false;
            }
            <%--if (document.getElementById("<%=ddlUserAccount.ClientID%>").value == "--Select username--") {
                alert("Please select username ");
                document.getElementById("<%=ddlUserAccount.ClientID%>").focus();
                return false;
            }--%>

            if (document.getElementById("<%=txtempname.ClientID%>").value == "") {
                alert("Please provide employee name ");
                document.getElementById("<%=txtempname.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtEmail.ClientID%>").value == "") {
                alert("Please provide email-id ");
                document.getElementById("<%=txtEmail.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtmobileno.ClientID%>").value == "") {
                alert("Please provide mobile no ");
                document.getElementById("<%=txtmobileno.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtempcode.ClientID%>").value == "") {
                alert("Please provide employee code ");
                document.getElementById("<%=txtempcode.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDesignation.ClientID%>").value == "") {
                alert("Please provide designation");
                document.getElementById("<%=txtDesignation.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlhead.ClientID%>").value == "--Select State Head--") {
                alert("Please select State Head ");
                document.getElementById("<%=ddlhead.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlResponsibility.ClientID%>").value == "--Select Role--") {
                alert("Please select Role ");
                document.getElementById("<%=ddlResponsibility.ClientID%>").focus();
                return false;
            }

        }
    </script>
    <style type="text/css">
        .style4 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 210px;
            padding-left: 20px;
        }
    </style>

    <script type="text/javascript">
       <!--
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode > 31
          && (charCode < 48 || charCode > 57))
            return false;

        return true;
    }
    //-->
    </script>

    <div style="width: 100%;">
        <fieldset>
            <legend style="width:142px;">
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Employee Master
                </div>
            </legend>
            <br />
            <br />
            <div style="width: 100%; margin: 0px auto 0px auto">

                <table border="0" align="right" style="height: 348px; width: 85%;">
                    <tr>
                        <td class="style4">State Name<span class="alert">* </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstate" runat="server" Style="margin-left: 8px" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" TabIndex="1" Width="165px" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Rto Location Name<span class="alert">* </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlrtolocation" runat="server" Style="margin-left: 8px" TabIndex="2" Width="165px" OnSelectedIndexChanged="ddlrtolocation_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">User Name <%--<span class="alert">* </span>--%>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUserAccount" runat="server" Style="margin-left: 8px" TabIndex="3" Width="165px" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Employee Name <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtempname" class="form_textbox12" runat="server" MaxLength="30" TabIndex="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Email-Id <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" class="form_textbox12" TabIndex="5" MaxLength="50"></asp:TextBox>
                            &nbsp;
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtEmail" runat="server" ErrorMessage="Invalid Email-id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="save"></asp:RegularExpressionValidator>
                        </td>
                    </tr>

                    <tr>
                        <td class="style4">Mobile No<span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtmobileno" runat="server" class="form_textbox12" MaxLength="10" TabIndex="6" onkeypress="return isNumberKey(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Employee Code<span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtempcode" runat="server" class="form_textbox12" MaxLength="7" TabIndex="7"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Designation <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDesignation" runat="server" class="form_textbox12" MaxLength="30" TabIndex="8"></asp:TextBox>
                        </td>
                    </tr>

                     <tr>
                        <td class="style4">Department <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdepartment" runat="server" class="form_textbox12" MaxLength="30" TabIndex="8"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">HOD<span class="alert">* </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlhead" runat="server" Style="margin-left: 8px" TabIndex="9" Width="165px" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Joining Date <span class="alert">* </span>
                        </td>
                        <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left">
                                                                &nbsp;
                                                                <componentart:calendar ID="OrderDate" Width="150px" runat="server" 
                                                                    PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker" Height="22px">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </componentart:calendar>
                                                                &nbsp;
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                </td>
                    </tr>
                    <tr>
                        <td class="style4">Role <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlResponsibility" runat="server" Style="margin-left: 8px" TabIndex="10" Width="165px" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label><br />
                            <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>

                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="button1" runat="server" TabIndex="11" class="button_go" Text="Save" OnClientClick=" return validate()" OnClick="ButtonSubmit_Click" ValidationGroup="save" />
                            &nbsp;&nbsp;&nbsp;<asp:Button ID="btnReset" runat="server" class="button_go" Text="Reset" TabIndex="12" OnClick="btnReset_Click" />
                             <componentart:calendar runat="server" ID="CalendarOrderDate" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" 
                                        DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" 
                                        DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" 
                                        CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" 
                                        SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" 
                                        NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnChange" />
                                                        </ClientEvents>
                                                    </componentart:calendar>
                        </td>

                    </tr>

                </table>
                <div style="clear: both"></div>

            </div>


        </fieldset>
    </div>

</asp:Content>
