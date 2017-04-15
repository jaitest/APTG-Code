<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="APSyncWithGovt.aspx.cs" Inherits="HSRP.Transaction.APSyncWithGovt" %>

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
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
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
                                        <asp:Label ID="Label4" class="headingmain" runat="server">AP Sync</asp:Label>

                                    </td>
                                </tr>

                                <tr id="TRRTOHide" runat="server">
                                    <td>
                                        <!--     <asp:Label ID="dataLabellbl" class="headingmain" runat="server"  >Allowed RTO's :</asp:Label> 
                                          <asp:Label ID="lblRTOCode" class="form_Label_Repeter"  runat="server">Allowed RTO's : </asp:Label> 
                                    -->
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
                                        <asp:Label Text="Auth No.:" Visible="true" runat="server" ID="lblAuthno" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtAuthNo" runat="server"></asp:TextBox>
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
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("hsrprecordid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Payment Initial DateTime
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHSRPRecord_CreationDate" runat="server" Text='<%#Eval("HSRPRecord_CreationDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Online Payment ID
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOnlinePaymentID" runat="server" Text='<%#Eval("OnlinePaymentID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Amount
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRoundoff_netAmount" runat="server" Text='<%#Eval("Roundoff_netAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Owner Name
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblownername" runat="server" Text='<%#Eval("ownername") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Dealer Id
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldealerid" runat="server" Text='<%#Eval("dealerid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Vehicle Class
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblvehicleclass" runat="server" Text='<%#Eval("vehicleclass") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Vehicle Type
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblvehicletype" runat="server" Text='<%#Eval("vehicletype") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Online Payment Status
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOnlinePaymentStatus" runat="server" Text='<%#Eval("OnlinePaymentStatus") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Online Payment Tracking No
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOnlinePaymentTrackingNo" runat="server" Text='<%#Eval("OnlinePaymentTrackingNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Btn Initiated From
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBtnInitiatedFrom" runat="server" Text='<%#Eval("BtnInitiatedFrom") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Payment Gateway
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblpaymentGateway" runat="server" Text='<%#Eval("paymentGateway") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                    <td align="center" style="padding-top: 10px"></td>
                                </tr>
                                <tr>
                                    <%--<asp:Panel ID="Panel1" runat="server" Visible="false">
                                    <td align="center" style="padding-top: 10px">
                                        <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                            <tr>
                                                <td align="center" style="padding-top: 10px">
                                                    <asp:Label ID="lblGatewayName" runat="server" Text="Payment Gateway" Font-Bold="True" Font-Size="Medium" ForeColor="Black"></asp:Label>
                                                    <span style="color:red;">*</span>
                                                </td>
                                                <td align="center" style="padding-top: 10px">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlGateway" runat="server" style="height:26px;width:215px;padding-left:2px;position: relative;top: 0px;left: 8px;" >
                                                        <asp:ListItem>--Select--</asp:ListItem>
                                                        <asp:ListItem>ICICI</asp:ListItem>
                                                        <asp:ListItem>AxisBank</asp:ListItem>
                                                        <asp:ListItem>HDFC</asp:ListItem>
                                                        <asp:ListItem>PayByLAPL</asp:ListItem>
                                                        <asp:ListItem>CCAvenue</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="sync" ControlToValidate="ddlGateway" InitialValue="--Select--" ErrorMessage="Please Select Gateway Name"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="padding-top: 10px">
                                                    <asp:Label ID="Label2" runat="server" Text="Tracking No" Font-Bold="True" Font-Size="Medium" ForeColor="Black"></asp:Label>
                                                    <span style="color:red;">*</span>
                                                </td>
                                                <td align="center" style="padding-top: 10px">
                                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="form_textbox" onkeypress="return isNumber(event)"></asp:TextBox>
                                                    &nbsp;&nbsp;
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="sync" ControlToValidate="TextBox1" ErrorMessage="Please Enter Tracking No"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="padding-top: 10px;"> </td>
                                                <td align="center" style="padding-top: 10px;padding-right: 89px;">
                                                    <asp:Button ID="btnSyncWithGovt" ClientIDMode="Static" runat="server" Text="Sync With Govt" ValidationGroup="sync" BackColor="Orange" ForeColor="#000000" autoposback="true" OnClientClick="validatevalue();" Visible="false" OnClick="btnChalan_Click" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnVoid" ClientIDMode="Static" runat="server" Text="Void" BackColor="Orange" ForeColor="#000000" autoposback="true" Visible="false" OnClick="btnVoid_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                        </asp:Panel>--%>
                                    <td align="center" style="padding-top: 10px"></td>
                                </tr>
                                <tr>
                                    <td align="center" style="padding-top: 10px">
                                        <asp:Button ID="btnSyncWithGovt" ClientIDMode="Static" runat="server" Text="Sync With Govt" ValidationGroup="sync" BackColor="Orange" ForeColor="#000000" autoposback="true" OnClientClick="validatevalue();" Visible="false" OnClick="btnChalan_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                     <%--<asp:Button ID="btnVoid" ClientIDMode="Static" runat="server" Text="Void" BackColor="Orange" ForeColor="#000000" autoposback="true" Visible="false" OnClick="btnVoid_Click" />--%>
                                    </td>
                                    <td align="center" style="padding-top: 10px"></td>
                                </tr>


                            </table>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
    <br />
    <asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>
