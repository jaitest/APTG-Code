<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="MacBase.aspx.cs" Inherits="HSRP.Transaction.MacBase" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>

        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" style="background-image: url(../images/midtablebg.jpg)">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="26">
                                            <span class="headingmain">PC Status</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                                    <tr>
                                        <td height="26" bgcolor="#FFFFFF" class="maintext">
                                            <table width="98%" border="0" align="center" cellpadding="3" cellspacing="3">
                                                <tr>
                                                    <td>
                                                        <span id="lblMsg" class="header"></span>
                                                    </td>
                                                    <td style="width: 13%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="label1" runat="server" Text="User Name:"></asp:Label>
                                                    </td>
                                                    <td style="width: 13%">
                                                        <asp:TextBox class="form_textbox" MaxLength="30" ID="textBoxUserName"
                                                            runat="server"></asp:TextBox>
                                                        
                                                    </td>
                                                    &nbsp;
                                                    <td>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="buttonSave" runat="server" Text="Update" class="button" Visible="false" OnClick="buttonSave_Click" />
                                                    </td>
                                                </tr>                                               
                                               
                                                <tr>
                                                    <td style="padding-left: 45px" class="form_text" align="center" colspan="2">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        &nbsp;&nbsp;
                                                        &nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="alert">
                                                        * Fields are mandatory.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" class="FieldText">
                                                        <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                                        <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                                    </td>
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
    </div>
</asp:Content>
