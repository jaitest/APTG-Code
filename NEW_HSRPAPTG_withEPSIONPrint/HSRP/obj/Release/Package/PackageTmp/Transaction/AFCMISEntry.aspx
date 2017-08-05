<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="AFCMISEntry.aspx.cs" Inherits="HSRP.Transaction.AFCMISEntry" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#ddlErpProductCode").change(function () {
                //alert("Selected");
                $('#lblProcessing').show();
            });
        });
    </script>
    <script type="text/javascript">
        function isNumberKey(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>

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
    </script>
    <script language="javascript" type="text/javascript">
        
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
                                        <asp:Label ID="Label4" class="headingmain" runat="server"> AFC MIS Entry</asp:Label>
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
                                        <asp:Label Text="2W Photo Recived:" Visible="true" runat="server" ID="Label1" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txt2WPhotoRecived" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt2WPhotoRecived" ErrorMessage="Please Enter 2W Photo Recived In Number"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>

                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Other Than 2W Photo Recieved:" Visible="true" runat="server" ID="lblOtherThan2WPhotoRecieved" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtOtherThan2WPhotoRecieved" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOtherThan2WPhotoRecieved" ErrorMessage="Please Enter Other Than 2W Photo Recieved"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                 <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="3rd Sticker Photo Recieved:" Visible="true" runat="server" ID="Label2" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txt3rdStickerPhotoRecieved" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt3rdStickerPhotoRecieved" ErrorMessage="Please Enter 3rd Sticker Photo Recieved"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>

                                <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Order Closed 2W:" Visible="true" runat="server" ID="Label3" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtOrderClosed2W" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtOrderClosed2W" ErrorMessage="Please Enter Order Closed 2W"></asp:RequiredFieldValidator>
                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                 <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Other Than 2W Order Closed:" Visible="true" runat="server" ID="Label5" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtOtherThan2WOrderClosed" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtOtherThan2WOrderClosed" ErrorMessage="Please Enter Other Than 2W Order Closed"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                 <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Snap Lock Recieved 19mm:" Visible="true" runat="server" ID="Label6" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtSnapLockRecieved19mm" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtSnapLockRecieved19mm" ErrorMessage="Please Enter Snap Lock Recieved 19mm"></asp:RequiredFieldValidator>
                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                 <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Snap Lock Recieved 25mm:" Visible="true" runat="server" ID="Label7" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtSnapLockRecieved25mm" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtSnapLockRecieved25mm" ErrorMessage="Please Enter Snap Lock Recieved 25mm"></asp:RequiredFieldValidator>
                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                 <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                                          <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Snap Lock Consumption 19mm:" Visible="true" runat="server" ID="Label9" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtSnapLockConsumption19mm" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtSnapLockConsumption19mm" ErrorMessage="Please Enter Snap Lock Consumption 19mm"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                 <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Snap Lock Consumption 25mm:" Visible="true" runat="server" ID="Label15" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtSnapLockConsumption25mm" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtSnapLockConsumption25mm" ErrorMessage="Please Enter Snap Lock Consumption 25mm"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Missing Plate Font:" Visible="true" runat="server" ID="Label8" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtMissingPlateFont" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtMissingPlateFont" ErrorMessage="Please Enter Missing Plate Font"></asp:RequiredFieldValidator>
                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                 <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
      
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Missing Plate Rear:" Visible="true" runat="server" ID="Label10" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtMissingPlateRear" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtMissingPlateRear" ErrorMessage="Please Enter Missing Plate Rear"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                 <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Missing Plate Both:" Visible="true" runat="server" ID="Label11" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtMissingPlateBoth" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtMissingPlateBoth" ErrorMessage="Please Enter Missing Plate Both"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                 <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Rejected Plate Front:" Visible="true" runat="server" ID="Label12" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtRejectedPlateFront" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtRejectedPlateFront" ErrorMessage="Please Enter Rejected Plate Front"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                 <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Rejected Plate Rear:" Visible="true" runat="server" ID="Label13" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtRejectedPlateRear" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtRejectedPlateRear" ErrorMessage="Please Enter Rejected Plate Rear"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                 <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Rejected Plate Both:" Visible="true" runat="server" ID="Label14" />
                                        <span style="color: red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtRejectedPlateBoth" runat="server" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtRejectedPlateBoth" ErrorMessage="Please Enter Rejected Plate Both"></asp:RequiredFieldValidator>

                                    </td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                 <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                

                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label ID="lblErrMsg" runat="server" Text="" />
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button_go" OnClick="btnSave_Click" />
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text"></td>
                                    <td valign="middle" class="form_text">
                                    </td>
                                    <td valign="middle" class="form_text">
                                        &nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text"></td>
                                    <td valign="middle" class="form_text"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>
