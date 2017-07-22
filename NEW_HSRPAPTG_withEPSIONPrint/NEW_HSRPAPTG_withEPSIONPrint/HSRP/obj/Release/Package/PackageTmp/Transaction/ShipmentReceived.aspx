<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ShipmentReceived.aspx.cs" Inherits="HSRP.Master.ShipmentReceived" %>
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
        function HideLabel()
        {
            document.getElementById("<%=lblProcessing.ClientID %>").style.display = "none";            
        };
        function isNumber(evt) {
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
                                        <asp:Label ID="Label4" class="headingmain" runat="server">Shipment Received</asp:Label>
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
                                        <asp:Label Text="Transfar Order Number:" Visible="true" runat="server" ID="lblTransfarOrderNumber" />
                                        <span style="color:red;">*</span>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txtTransfarOrderNumber" runat="server" MaxLength="20"></asp:TextBox>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:RequiredFieldValidator  ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTransfarOrderNumber" ErrorMessage="Please Enter Transfar Order Number"></asp:RequiredFieldValidator></td>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <td valign="middle" class="form_text" nowrap="nowrap">                                      
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                    </td>
                                   
                                </tr><br />
                                <tr>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                    <td valign="middle" class="form_text">&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnSave" runat="server" Text="Order Received" OnClick="btnSave_Click" />
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                       

                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        </td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text"></td>
                                    <td valign="middle" class="form_text"><asp:Label Text="" runat="server" ID="lblErrMsg" /></td>
                                    <td valign="middle" class="form_text">
                                        <asp:Label ID="lblProcessing" ClientIDMode="Static" Visible="true" runat="server" Style="display: none;" Text="Processing..."></asp:Label></td>
                                    </td>
                                    <td valign="middle" class="form_text"></td>
                                    <td valign="middle" class="form_text"></td>
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
    <br />
    <asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>
