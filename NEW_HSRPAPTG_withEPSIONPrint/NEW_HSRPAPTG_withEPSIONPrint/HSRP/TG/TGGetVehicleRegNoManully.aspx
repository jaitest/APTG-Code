<%@ Page Title="TGGetVehicleRegNoManully" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TGGetVehicleRegNoManully.aspx.cs" Inherits="HSRP.TG.TGGetVehicleRegNoManully" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">Fatch & Update Vehicle Reg No</span>
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
                                        &nbsp;
                                        <asp:Label Text="Auth No. :" runat="server" Visible="true" ID="lblAuthono" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAuthono" Width="200px" runat="server"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btngo" Width="58px" Visible="true" runat="server" Text="GO" ToolTip="Please Click for Get VehicleRegNo" class="button" OnClick="btngo_Click" />
                                        &nbsp;&nbsp;<asp:Label ID="Label1" runat="server" ForeColor="Blue" Text=""></asp:Label>
                                    </td>
                                    <td align="left" valign="middle">
                                        <asp:HiddenField ID="hdnvehicleregno" runat="server" />
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
                                        <div style="padding-top: 37px; padding-left: 313px;">
                                        <asp:GridView ID="grdid" runat="server" CellPadding="4" ForeColor="#333333"
                                            GridLines="None" AutoGenerateColumns="false" OnRowCommand="grdid_RowCommand1"
                                            OnRowDataBound="grdid_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Hsrprecord ID" DataField="hsrprecordid" />
                                                <asp:BoundField HeaderText="Auth No" DataField="hsrprecord_Authorizationno" />
                                                <asp:BoundField HeaderText="Vehicle Reg No" DataField="vehicleregno" />
                                                <asp:BoundField HeaderText="Owner Name" DataField="OwnerName" />
                                                <asp:BoundField HeaderText="ChassisNo" DataField="ChassisNo" />
                                                <asp:BoundField HeaderText="EngineNo" DataField="EngineNo" />
                                                <asp:TemplateField HeaderText="Update">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lnkUpdate" ClientIDMode="Static" CommandArgument='<%#Eval("hsrprecordid") %>'
                                                            CommandName="Update" >Update</asp:LinkButton>
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
