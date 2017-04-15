<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="APTGCallCenterDetails.aspx.cs" Inherits="HSRP.Report.APTGCallCenterDetails" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="15">
                                        <span class="headingmain">AP TG Call Center Details</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td valign="middle" class="form_text" nowrap="nowrap">
                                                    <asp:Label ID="lblvehicleno" runat="server" Text="Enter Vehicle No."></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtvehicleno" runat="server" CssClass="form_textbox" MaxLength="10"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td valign="middle" class="form_text" nowrap="nowrap"><asp:Label ID="Label1" runat="server" Text="Enter Vehicle No."></asp:Label></td>
                                                <td class="form_text">
                                                    &nbsp;&nbsp;
                                                    <asp:DropDownList ID="DropDownList1" runat="server" Height="19px" Width="219px"></asp:DropDownList></td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td valign="middle" class="form_text" nowrap="nowrap"><asp:Label ID="Label2" runat="server" Text="Enter Vehicle No."></asp:Label></td>
                                                <td class="form_text">
                                                    &nbsp;&nbsp;
                                                    <asp:DropDownList ID="DropDownList2" runat="server" Height="16px" Width="218px"></asp:DropDownList></td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td valign="middle" class="form_text" nowrap="nowrap"><asp:Label ID="Label3" runat="server" Text="Enter Vehicle No."></asp:Label></td>
                                                <td>
                                                    &nbsp;&nbsp;
                                                    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Height="45px" Width="215px"></asp:TextBox></td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    &nbsp;&nbsp;
                                                    <asp:Button ID="Button1" runat="server" Text="Save" CssClass="button_go" /></td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
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
