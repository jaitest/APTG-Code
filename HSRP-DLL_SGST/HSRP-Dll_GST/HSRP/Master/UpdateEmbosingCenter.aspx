<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UpdateEmbosingCenter.aspx.cs" 
    Inherits="HSRP.Master.UpdateEmbosingCenter" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div style="background-color:White">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2" height="27" style="background-image: url(../images/midtablebg.jpg)">
                    <span class="headingmain">Update Embossing Center..</span>
                </td>
            </tr>
            <tr>
                <td colspan="2" height="19" valign="top" nowrap="nowrap" bgcolor="#d2e0f1" class="heading1">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td valign="top" class="form_text">
                    <asp:Label ID="labelOrganization" Font-Size="Medium" ForeColor="Black" runat="server"
                        Text="Select Dealer :"></asp:Label>
                    <span class="alert">* </span>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="dropDownDealer"  TabIndex="1"  DataTextField="dealername" DataValueField="sno"   runat="server"></asp:DropDownList>

                    <%--<asp:DropDownList Font-Size="Small" Width="200px" Enabled="false" TabIndex="1"   ID="dropDownDealer" runat="server" DataTextField="dealername" DataValueField="sno" >
                    </asp:DropDownList>--%>
                 
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td  valign="top" class="form_text" >
                    <asp:Label Text="Select Embossing Center:" runat="server" Font-Size="Medium" ForeColor="Black" /><span
                        class="alert">* </span>
                </td>
                <td valign="top">

                    <asp:DropDownList ID="dropDownListEmbCenter" TabIndex="2"   DataTextField="EmbCenterName"   DataValueField="Emb_Center_Id"   runat="server"></asp:DropDownList>

                   <%-- <asp:UpdatePanel ID="UpdatePanelUser" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" Font-Size="Small" Width="200px" Enabled="false"
                                TabIndex="2"   ID="dropDownListEmbCenter" DataTextField="EmbCenterName"
                                DataValueField="Emb_Center_Id">
                            </asp:DropDownList>

                       </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="dropDownListEmbCenter" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>--%>

                </td>
            </tr>
            <tr>
            <td></td>
           
                </tr>
             <tr>
                 <td></td> <td></td>
            </tr>
               
            <tr>
                
                <td colspan="2" align="center">
                    <asp:Button ID="buttonSave" runat="server" Text="Update Embossing Certer Center" TabIndex="3" class="button"
                        OnClick="buttonSave_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="alert">
                    * Fields are Mandatory
                </td>
            </tr>
            <tr>
                <td colspan="2" class="FieldText">
                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                    <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>
