<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="VerifyExpenseByAcc.aspx.cs" Inherits="HSRP.Transaction.VerifyExpenseByAcc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <script type="text/javascript">
        function VerifyAmount(i) {
            googlewin = dhtmlwindow.open("googlebox", "iframe", "ExpAmountApprovebyAcc.aspx?Mode=Edit&ExpenseID=" + i, "Update Expense Details", "width=950px,height=450px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function ()
            {
                window.location = "VerifyExpenseByAcc.aspx";
                return true;
            }
        }
    </script>
    <%--<script language="javascript" type="text/javascript">
        function validate() {
            <%--if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State Name");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
            return false;
        }
        if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select RTO Name--") {
                alert("Select RTO Name");
                document.getElementById("<%=dropDownListClient.ClientID%>").focus();
            return false;
        }
            //        if (document.getElementById("<%=dropDownListorderStatus.ClientID%>").value == "-- Select Status --") {
            //            alert("Select Status");
            //            document.getElementById("<%=dropDownListorderStatus.ClientID%>").focus();
            //            return false;
            //        }
        }

    </script>--%>

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
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                                            OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" />
                                    </td>
                                    <td valign="middle" class="form_text">
                                        <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList AutoPostBack="false" Visible="true" ID="dropDownListClient" CausesValidation="false" Width="140px"
                                                    runat="server" DataTextField="RTOLocationName"
                                                    DataValueField="RTOLocationID">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListClient" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>

                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                        <asp:Label Text="From:" Visible="true" runat="server" ID="labelDate" />
                                        &nbsp;&nbsp;</td>
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
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;<asp:Label Text="To:" Visible="true" runat="server" ID="labelTO" />
                                        &nbsp;&nbsp;</td>
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
                                    </td>


                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Expense Status" Visible="true" runat="server" ID="labelOrderStatus" />&nbsp;&nbsp; </td>
                                    <td>
                                        <asp:DropDownList AutoPostBack="false" Visible="true"
                                            ID="dropDownListorderStatus" CausesValidation="false" runat="server">
                                            <asp:ListItem>-- Select Status --</asp:ListItem>
                                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                            <asp:ListItem Value="Hold">Hold</asp:ListItem>
                                            <asp:ListItem Value="Approve">Approve</asp:ListItem>
                                        </asp:DropDownList>&nbsp; </td>

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:LinkButton ID="LinkbuttonSearch" runat="server" Text="GO" class="button" OnClientClick=" return validate()" OnClick="ButtonGo_Click" />
                                        &nbsp;  
                                            &nbsp;  

                                            &nbsp;
                                    </td>

                                    <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>
                                </tr>
                            </table>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                     <tr>
                        <td>&nbsp;</td>
                    </tr>
                     <tr>
                        <td>&nbsp;</td>
                    </tr>
                     <tr>
                        <td>&nbsp;</td>
                    </tr>

                    <tr>
                        <td style="padding-top: 1px;">
                            <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" AllowPaging="true" PageSize="15" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                                AutoGenerateColumns="false" ShowHeader="true" Width="100%" BackColor="White" BorderColor="#FFCC99" BorderStyle="Solid" BorderWidth="1px" OnRowCommand="GridView1_RowCommand">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField HeaderText="ExpenseSaveID" DataField="ExpenseSaveID" />
                                    <asp:BoundField HeaderText="ExpenseName" DataField="ExpenseName" /> 
                                    <asp:BoundField HeaderText="claimedby" DataField="claimedby" />
                                    <asp:BoundField HeaderText="BillNo" DataField="BillNo" />
                                    <asp:BoundField HeaderText="BillDate" DataField="BillDate" />
                                    <asp:BoundField HeaderText="BillAmount" DataField="BillAmount" />
                                    <asp:BoundField HeaderText="VerifiedAmount" DataField="VerifiedAmount" />
                                    <asp:BoundField HeaderText="VerifiedRemarks" DataField="VerifiedRemarks" />
                                    <asp:BoundField HeaderText="VerifiedDate" DataField="VerifiedDate" />
                                    <asp:BoundField HeaderText="VerifiedBy" DataField="VerifiedBy" />
                                    <asp:BoundField HeaderText="Verified By CEO" DataField="VerifiedByCEO" />
                                    <asp:BoundField HeaderText="Status" DataField="CEOExpenseStatus" />
                                    <asp:TemplateField HeaderText="Approve">
                                        <ItemTemplate>
                                            <a href="javascript:VerifyAmount(<%#Eval("ExpenseSaveID")%>)">
                                                <asp:Label runat="server" ID="lblApprove" Text="Approve"></asp:Label></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reject">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkReject" runat="server" CommandArgument='<%#Eval("ExpenseSaveID")%>' CommandName="Rejected" ValidationGroup="Reject">Reject</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transfer Expense For CEO Approval">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkTransfer" runat="server" CommandArgument='<%#Eval("ExpenseSaveID")%>' CommandName="Transfer">Transfer</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#CCCC99" />
                                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                <RowStyle BackColor="#F7F7DE" />
                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                <SortedAscendingHeaderStyle BackColor="#848384" />
                                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                <SortedDescendingHeaderStyle BackColor="#575357" />
                            </asp:GridView>
                        </td>

                        <td>
                            <asp:Panel ID="pnlpopup2" runat="server" BackColor="White" Height="269px" Width="400px" Style="display: none">
                                            <table width="100%" style="border: Solid 3px #507CD1; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
                                                <tr style="background-color: #507CD1">
                                                    <td colspan="2" style="height: 10%; color: White; font-weight: bold; font-size: larger" align="center">Reject Transaction</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="width: 45%">Expense ID:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTransactionId" runat="server"></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td align="right">Rejected Summary
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" /><br />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtRemarks" ValidationGroup="reject" runat="server" ErrorMessage="Please Enter Rejected Summary"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="Button1" CommandName="Update" runat="server" Text="Rejected" OnClick="btnReject" ValidationGroup="reject" />
                                                        <asp:Button ID="Button2" runat="server" Text="Cancel" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSucMess" runat="server" ForeColor="Green" Text=""></asp:Label></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Button ID="Button3" runat="server" Style="display: none" />

                                        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="Button3" PopupControlID="pnlpopup2"
                                            CancelControlID="Button2" BackgroundCssClass="modalBackground">
                                        </asp:ModalPopupExtender>
                        </td>

                        <td>

                            <%--<ComponentArt:Grid ID="Grid1" RTOLocationIDMode="AutoID" runat="server" ImagesBaseUrl="~/images"
                                Width="100%" GroupingNotificationText="Drag a column to this area to group by it."
                                LoadingPanelPosition="MiddleCenter" LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                GroupBySortImageHeight="10" GroupBySortImageWidth="10" GroupBySortDescendingImageUrl="group_desc.gif"
                                GroupBySortAscendingImageUrl="group_asc.gif" GroupingNotificationTextCssClass="GridHeaderText"
                                IndentCellWidth="22" TreeLineImageHeight="19" TreeLineImageWidth="22" TreeLineImagesFolderUrl="~/images/lines/"
                                PagerImagesFolderUrl="~/images/pager/" PreExpandOnGroup="true" GroupingPageSize="5"
                                SliderPopupClientTemplateId="SliderTemplate" SliderPopupOffsetX="20" SliderGripWidth="9"
                                SliderWidth="150" SliderHeight="20" PagerButtonHeight="22" PagerButtonWidth="41"
                                PagerTextCssClass="GridFooterText" PageSize="25" GroupByTextCssClass="GroupByText"
                                GroupByCssClass="GroupByCell" FooterCssClass="GridFooter" HeaderCssClass="GridHeader"
                                SearchOnKeyPress="true" SearchTextCssClass="GridHeaderText" ShowSearchBox="true"
                                ShowHeader="true" CssClass="Grid" RunningMode="Callback" SearchBoxPosition="TopLeft"
                                GroupingNotificationPosition="TopRight" FillContainer="true"
                                Height="300px">
                                <Levels>
                                    <ComponentArt:GridLevel DataKeyField="ExpenseSaveID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                        DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                        HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                        HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                        SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                        SortImageHeight="19">
                                        <Columns>
                                            <ComponentArt:GridColumn DataField="ExpenseSaveID" Visible="False" />
                                            <ComponentArt:GridColumn DataField="ExpenseName" HeadingText="Expense Name" SortedDataCellCssClass="SortedDataCell"
                                                Width="120" />

                                            <ComponentArt:GridColumn DataField="BillNo" HeadingText="Bill No." SortedDataCellCssClass="SortedDataCell"
                                                Width="120" />

                                            <ComponentArt:GridColumn DataField="ExpenseStatus" FormatString="" HeadingText="Status" SortedDataCellCssClass="SortedDataCell" Width="80" />

                                            <ComponentArt:GridColumn DataField="BillDate" HeadingText="Bill Date" SortedDataCellCssClass="SortedDataCell"
                                                Width="80" />

                                            <ComponentArt:GridColumn DataField="BillAmount" Align="Right" HeadingText="Bill Amount" SortedDataCellCssClass="SortedDataCell"
                                                Width="70" />

                                            <ComponentArt:GridColumn DataField="VerifiedAmount" Align="Right" Visible="true" HeadingText="Verify Amount"
                                                Width="70" />
                                            <ComponentArt:GridColumn DataField="Balance" Align="Right" HeadingText="Hold Amount" SortedDataCellCssClass="SortedDataCell"
                                                Width="70" />
                                            <ComponentArt:GridColumn DataField="VerifiedRemarks" HeadingText="Verify Remarks" SortedDataCellCssClass="SortedDataCell"
                                                Width="180" />
                                            <ComponentArt:GridColumn DataField="VerifiedDate" HeadingText="Verify Date" SortedDataCellCssClass="SortedDataCell"
                                                Width="80" />
                                            <ComponentArt:GridColumn DataField="VerifiedBy" HeadingText="Verified By" SortedDataCellCssClass="SortedDataCell"
                                                Width="130" />


                                            <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="Assign" HeadingText="Verify" SortedDataCellCssClass="SortedDataCell" Width="80" />

                                        </Columns>
                                    </ComponentArt:GridLevel>
                                </Levels>
                                <ClientTemplates>

                                    <ComponentArt:ClientTemplate ID="Assign">
                                        <a style="color: Blue" onclick="javascript:VerifyAmount(## DataItem.GetMember('ExpenseSaveID').Value ##);">Verify</a>
                                    </ComponentArt:ClientTemplate>

                                    <ComponentArt:ClientTemplate ID="ClientTemplate2" runat="server">
                                        <table cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td style="font-size: 10px;">Loading...&nbsp;
                                                </td>
                                                <td>
                                                    <img alt="loading" src="/Images/spinner.gif" width="16" height="16" border="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ComponentArt:ClientTemplate>
                                    <ComponentArt:ClientTemplate ID="ClientTemplate3" runat="server">
                                        <table class="SliderPopup" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td valign="top" style="padding: 5px;">
                                                    <table width="741" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td width="25" align="center" valign="top" style="padding-top: 3px;"></td>
                                                            <td>
                                                                <table cellspacing="0" cellpadding="2" border="0" style="width: 255px;">
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table width="741" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td>Page <b>## DataItem.PageIndex + 1 ##</b> of <b>## Grid1.PageCount ##</b>
                                                            </td>
                                                            <td align="right">Record <b>## DataItem.Index + 1 ##</b> of <b>## Grid1.RecordCount ##</b>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ComponentArt:ClientTemplate>
                                </ClientTemplates>
                                <ServerTemplates>
                                    <ComponentArt:GridServerTemplate ID="Sticker">
                                        <Template>
                                            <asp:LinkButton ID="LinkButtonSticker" runat="server" Text="Sticker" CommandName="Sticker"></asp:LinkButton>
                                        </Template>
                                    </ComponentArt:GridServerTemplate>
                                </ServerTemplates>
                                <ServerTemplates>
                                    <ComponentArt:GridServerTemplate ID="TVSSticker">
                                        <Template>
                                            <asp:LinkButton ID="LinkButtonStickerTVS" runat="server" Text="Sticker" CommandName="TVSSticker"></asp:LinkButton>
                                        </Template>
                                    </ComponentArt:GridServerTemplate>
                                </ServerTemplates>
                            </ComponentArt:Grid>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
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
                        <td colspan="2">
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

</asp:Content>
