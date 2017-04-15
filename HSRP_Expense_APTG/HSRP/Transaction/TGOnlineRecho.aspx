<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TGOnlineRecho.aspx.cs" Inherits="HSRP.Transaction.TGOnlineRecho" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/table.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />    
      <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                  Online Recho.  </div>
            </legend>
      </fieldset>
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">


                                <tr id="TR1" runat="server">
                                    <td>
                                        <asp:Label ID="Label4" class="headingmain" runat="server">AP Sync</asp:Label>

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
                                        <asp:Button ID="btnStepOne" runat="server" Text="Step 1" OnClick="btnStepOne_Click" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:Label ID="Label1" runat="server" Text="Msg"></asp:Label>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="BtnGetdupliateInUploaaded" runat="server" Text="Get Uploaded Data" ToolTip="Get duplicate in uploaded data" BackColor="Orange" ForeColor="#000000" class="button" OnClick="BtnGetdupliateInUploaaded_Click"/>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="BtnGetduplicateInpayment" runat="server" Text="Get Payment Data" ToolTip="Get Duplicate in Payment Data" BackColor="Orange" ForeColor="#000000" class="button" OnClick="BtnGetduplicateInpayment_Click"/>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="BtnGetduplicateInOrder" runat="server" Text="Get Order Data" ToolTip="Get Duplicate in Order Data" BackColor="Orange" ForeColor="#000000" class="button" OnClick="BtnGetduplicateInOrder_Click"/>
                                    </td>

                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnStep2" runat="server" Text="Step 2" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:Label ID="Label2" runat="server" Text="Msg"></asp:Label>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnStepTwoGetdata" runat="server" Text="Get Data" ToolTip="Get Data" BackColor="Orange" ForeColor="#000000" class="button" />
                                    </td>

                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnStepThree" runat="server" Text="Step 3" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:Label ID="Label3" runat="server" Text="Msg"></asp:Label>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnStepthreeGetdata" runat="server" Text="Get Data" ToolTip="Get Data" BackColor="Orange" ForeColor="#000000" class="button" />
                                    </td>

                                </tr>
                                <tr>
                                    <td valign="bottom" class="form_text" nowrap="nowrap">&nbsp;</td>
                                    <td valign="bottom">&nbsp;</td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;</td>



                                </tr>
                                <tr>
                                    <td valign="bottom" class="form_text" nowrap="nowrap">&nbsp;</td>
                                    <td valign="bottom">&nbsp;</td>
                                    <td colspan="4">
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="red" />
                                        <asp:Label ID="LblMessage" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="blue" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</asp:Content>
