<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="APOnlineOrderBookingConfirmation.aspx.cs" Inherits="HSRP.AP.APOnlineOrderBookingConfirmation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .cal_Calendar .ajax__calendar_title,
        .cal_Calendar .ajax__calendar_next,
        .cal_Calendar .ajax__calendar_prev {
            color: #004080;
            padding-top: 3px;
        }

        .cal_Calendar .ajax_calendar_invalid .ajax_calendar_day,
        .ajax_calendar_invalid {
            color: red;
            text-decoration: strike;
            cursor: pointer;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .listMain {
            background-repeat: repeat-x;
            background-color: #FFFFFF;
            z-index: 1000;
            width: 302px !important;
            height: 250px !important;
            text-align: inherit;
            text-indent: -1;
            list-style: none;
            overflow-y: scroll;
            scrollbar-arrow-color: #B89020;
            scrollbar-base-color: #8E6E1C;
            scrollbar-face-color: #B6C5D4;
            scrollbar-3dlight-color: #8E6E1C;
            scrollbar-highlight-color: #EED47D;
            scrollbar-shadow-color: #959595;
            scrollbar-darkshadow-color: #00337E;
            margin-left: 0px;
            border-bottom: 1px solid #B5C6D4;
            border-left: 1px solid #B5C6D4;
            margin-top: 0px;
        }

        .wordWheel .itemsMain {
            background: none;
            border-collapse: collapse;
            color: #00337E;
            white-space: nowrap;
            text-align: inherit left;
        }

        .wordWheel .itemsSelected {
            background-repeat: repeat-x;
            background-color: #EED47D;
            color: #00337E;
            border-top: 1px solid #FFF8E8;
            border-left: 1px solid #FFF8E8;
            border-bottom: 1px solid #00337E;
            border-right: 1px solid #00337E;
        }
    </style>

    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">AP Online Order Booking Confirmation</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        &nbsp;<asp:Label Text="Auth No. :" runat="server" Visible="true" ID="lblAuthono" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAuthono" Width="300px" runat="server"></asp:TextBox>
                                        <asp:AutoCompleteExtender ServiceMethod="SearchByAuthno" MinimumPrefixLength="2" CompletionInterval="500" TargetControlID="txtAuthono" ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false"
                                            CompletionListCssClass="wordWheel listMain .box"
                                            CompletionListItemCssClass="wordWheel itemsMain"
                                            CompletionListHighlightedItemCssClass="wordWheel itemsSelected">
                                        </asp:AutoCompleteExtender>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btngo" Width="58px" Visible="true" runat="server"
                                            Text="GO" ToolTip="Please Click for Report"
                                            class="button" OnClientClick=" return validate()" OnClick="btngo_Click" />
                                    </td>
                                    <td align="left" valign="middle">
                                        
                                    </td>
                                    <td valign="left" class="form_text"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                        <asp:Label ID="lblSucMess" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="Blue"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="padding-top: 37px;">
                                        <asp:GridView ID="grdid" runat="server" CellPadding="4" ForeColor="#333333"
                                            GridLines="None" AutoGenerateColumns="false" OnRowCommand="grdid_RowCommand1"
                                            OnRowDataBound="grdid_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Hsrprecord ID" DataField="hsrprecordid" />
                                                <asp:BoundField HeaderText="Auth No" DataField="hsrprecord_Authorizationno" />
                                                <asp:BoundField HeaderText="Vehicle Reg No" DataField="vehicleregno" />
                                                <asp:BoundField HeaderText="Owner Name" DataField="OwnerName" />
                                                <asp:BoundField HeaderText="Dealer Name" DataField="DealerName" />
                                                <asp:BoundField HeaderText="Online Payment ID" DataField="OnlinePaymentID" />
                                                <asp:BoundField HeaderText="Payment Gateway" DataField="PaymentGateway" />
                                                <asp:BoundField HeaderText="Online Payment Tracking No" DataField="OnlinePaymentTrackingNo" />
                                                <asp:TemplateField HeaderText="Approval">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lnkApproval" ClientIDMode="Static" CommandArgument='<%#Eval("hsrprecordid") %>'
                                                            CommandName="Approval" ValidationGroup="link">Approved</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reject">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lnkReject" ClientIDMode="Static" CommandArgument='<%#Eval("hsrprecordid") %>'
                                                            CommandName="Reject" ValidationGroup="link">Reject</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#2461BF" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#EFF3FB" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                        </asp:GridView>
                                            </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
