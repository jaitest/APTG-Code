﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewOwnExpenses.aspx.cs" Inherits="HSRP.Transaction.ViewOwnExpenses" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>

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
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="true" PageSize="15" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                                AutoGenerateColumns="false" ShowHeader="true" Width="100%" BackColor="White" BorderColor="#FFCC99" BorderStyle="Solid" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField HeaderText="Expense Save ID" DataField="ExpenseSaveID" />
                                    <asp:BoundField HeaderText="Expense Name" DataField="ExpenseName" />
                                    <asp:BoundField HeaderText="Bill No" DataField="BillNo" />
                                    <asp:BoundField HeaderText="Bill Date" DataField="BillDate" />
                                    <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" />
                                    <asp:BoundField HeaderText="HOD Status" DataField="ExpenseStatus" />
                                    <asp:BoundField HeaderText="HOD Remarks" DataField="VerifiedRemarks" />
                                    <asp:BoundField HeaderText="HOD Date" DataField="VerifiedDate" />
                                    <asp:BoundField HeaderText="HOD Name" DataField="VerifiedBy" />
                                    <asp:BoundField HeaderText="HOD Amount" DataField="VerifiedAmount" />
                                    <asp:BoundField HeaderText="Acc Status" DataField="AccExpenseStatus" />
                                    <asp:BoundField HeaderText="Acc Remarks" DataField="VerifiedRemarksByAcc" />
                                    <asp:BoundField HeaderText="Acc Date" DataField="VerifiedByAccDate" />
                                    <asp:BoundField HeaderText="Acc Name" DataField="VerifiedByAcc" />
                                    <asp:BoundField HeaderText="Acc Amount" DataField="VerifiedAmountByAcc" />
                                    <asp:BoundField HeaderText="CEO Status" DataField="CEOExpenseStatus" />
                                    <asp:BoundField HeaderText="CEO Remarks" DataField="VerifiedRemarksByCEO" />
                                    <asp:BoundField HeaderText="CEO Date" DataField="VerifiedByDateCEO" />
                                    <asp:BoundField HeaderText="CEO Amount" DataField="VerifiedAmountByCEO" />
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
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>
