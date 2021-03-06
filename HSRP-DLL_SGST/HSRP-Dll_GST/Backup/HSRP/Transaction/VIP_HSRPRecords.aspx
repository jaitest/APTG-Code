﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VIP_HSRPRecords.aspx.cs" Inherits="HSRP.Transaction.VIP_HSRPRecords" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/themes/base/jquery-ui.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script>
  <%--  <script type="text/javascript">
        $(function () {
        <%=Session["lang"]%>
            var session = '<%=Session["UserHSRPStateID"]%>'
            var session1 = '<%=Session["UserRTOLocationID"]%>'
//            alert(session);
//            alert(session1);
            $(".tb").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "EmployeeList.asmx/FetchEmailList",
                        data: "{ 'mail': '" + request.term + "'}",
                        //data: "{ 'mail': '" + request.term + "',stateid:67,rtolocationid:90}",
                        //data: "{ 'mail': '" + request.term + "',stateid:"+session+",rtolocationid:"+session1+"}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.Email
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                },
                minLength: 1
            });
        });
    </script>--%>
    <script type="text/javascript">
        function OnSelectedIndexChangeVehicleModel() {
            if (document.getElementById("<%=DropDownListVehicleClass.ClientID%>").value == "--Select Vehicle Class--") {
                alert("Select correct Vehicle Class.");
                //document.getElementById("<%=DropDownListVehicleClass.ClientID%>").options[0].selected = true;
                //document.getElementById("<%=DropDownListVehicleClass.ClientID%>").value == "--Select Vehicle Class--";
                document.getElementById("<%=DropDownListVehicleModel.ClientID%>").selectedIndex = 0;
                document.getElementById("<%=DropDownListVehicleClass.ClientID%>").focus();
                return false;
            }
        }

        function OnSelectedIndexChangeVehicleMaker() {
            //            if (document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").value == "--Select Vehicle Maker--") {
            //                alert("Select correct Vehicle Maker.");
            //                //document.getElementById("<%=DropDownListVehicleClass.ClientID%>").options[0].selected = true;
            //                //document.getElementById("<%=DropDownListVehicleClass.ClientID%>").value == "--Select Vehicle Class--";
            //                document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").selectedIndex = 0;
            //                document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").focus();
            //                return false;
            //            }
        }


        function OnSelectedIndexChangeModel() {
            //            if (document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").value == "--Select Vehicle Maker--") {
            //                alert("Select correct Vehicle Maker.");
            //                document.getElementById("<%=DropDownListModel.ClientID%>").selectedIndex = 0;
            //                document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").focus();
            //                return false;
            //            }
        }



        function OnSelectedIndexChangeVehicleOrder() {
            if (document.getElementById("<%=DropDownListVehicleClass.ClientID%>").value == "--Select Vehicle Class--") {
                alert("Select correct Vehicle Class.");
                document.getElementById("<%=DropDownListVehicleClass.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=DropDownListVehicleModel.ClientID%>").value == "--Select Vehicle Model--") {
                alert("Select correct Vehicle Type.");
                document.getElementById("<%=DropDownListVehicleModel.ClientID%>").focus();
                return false;
            }
            //            if (document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").value == "--Select Vehicle Maker--") {
            //                alert("Select correct Vehicle Maker.");
            //                document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").focus();
            //                return false;
            //            }
            //            if (document.getElementById("<%=DropDownListModel.ClientID%>").value == "--Select Vehicle Model--") {
            //                alert("Select correct Vehicle Model.");
            //                document.getElementById("<%=DropDownListModel.ClientID%>").focus();
            //                return false;
            //            }
        }
    </script>
    <script type="text/javascript">
        1.
        function clearForm(form) {
            alert(form.toString());
            $(':input', form).each(function () {
                var type = this.type;
                var tag = this.tagName.toLowerCase(); // normalize case
                if (type == 'text' || type == 'password' || tag == 'textarea')
                    this.value = "";
                else if (type == 'checkbox' || type == 'radio')
                    this.checked = false;
                else if (tag == 'select')
                    this.selectedIndex = -1;
            });
        };


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
    <script type="text/javascript">

        function removeSpaces(string) {
            return string.split(' ').join('');
        }

        function CheckAuthNo() {
            var textBoxAuthrizationNo = document.getElementById("textBoxAuthorizationNo").value;
            if (textBoxAuthrizationNo == "") {
                alert("Please Provide Authorization No.");
                document.getElementById("textBoxAuthorizationNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxAuthorizationNo"))) {
                alert("Please Provide Valid Authorization No.");
                document.getElementById("textBoxAuthorizationNo").focus();
                return false;
            }

            // ValidRegNo(document.getElementById("textBoxAuthorizationNo").value, document.getElementById("<%=LabelCreatedID.ClientID%>").innerText);

        }

        function CheckVehicleRegNo() {
            var TextBoxVehicleRegNo = document.getElementById("TextBoxVehicleRegNo").value;
            // alert(TextBoxVehicleRegNo);
            if (TextBoxVehicleRegNo == "") {
                alert("Please Provide Vehicle Registration No.");
                document.getElementById("TextBoxVehicleRegNo").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxVehicleRegNo"))) {
                alert("Please Provide Valid Registration No.");
                document.getElementById("TextBoxVehicleRegNo").focus();
                return false;
            }

            if (!ValidRegNo(document.getElementById("TextBoxVehicleRegNo").value, document.getElementById("<%=LabelCreatedID.ClientID%>").innerText)) {
                alert("Please Provide Valid Registration No.");
                document.getElementById("TextBoxVehicleRegNo").focus();
                return false;
            }
        }

        function CheckCashReceiptNo() {
            var TextBoxCashReceiptNo = document.getElementById("TextBoxCashReceiptNo").value;
            if (TextBoxCashReceiptNo == "") {
                alert("Please Provide Cash Receipt No.");
                document.getElementById("TextBoxCashReceiptNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("TextBoxCashReceiptNo"))) {
                alert("Please Provide Valid Cash Receipt No.");
                document.getElementById("TextBoxCashReceiptNo").focus();
                return false;
            }
        }

        
    </script>
    <script type="text/javascript">
        function ValidateForm() {
            var textBoxAuthrizationNo = document.getElementById("textBoxAuthorizationNo").value;
            if (textBoxAuthrizationNo == "") {
                alert("Please Provide Authorization No.");
                document.getElementById("textBoxAuthorizationNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxAuthorizationNo"))) {
                alert("Please Provide Valid Authorization No.");
                document.getElementById("textBoxAuthorizationNo").focus();
                return false;
            }

            //            var HSRPDate = OrderDate_picker.value;
            //            if (HSRPDate == "") {
            //                alert("Please Provide HSRP Authentication Date.");
            //                document.getElementById("OrderDate_picker").focus();
            //                return false;
            //            }

            //            var MobileNo = document.getElementById("textBoxMobileNo").value;

            //            if (MobileNo != "") {
            //                if (MobileNo.length != 10) {
            //                    alert("Please Provide Correct Mobile No.");
            //                    document.getElementById("textBoxMobileNo").focus();
            //                    return false;
            //                }
            //            }


            var CustomerName = document.getElementById("textBoxCustomerName").value;

            if (CustomerName == "") {
                alert("Please Provide Customer Name.");
                document.getElementById("textBoxCustomerName").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxCustomerName"))) {
                alert("Your can't enter special characters.");
                document.getElementById("textBoxCustomerName").focus();
                return false;
            }

            var Address1 = document.getElementById("textBoxAddress1").value;
            if (Address1 == "") {
                alert("Please Provide address.");
                document.getElementById("textBoxAddress1").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxAddress1"))) {
                alert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.");
                document.getElementById("textBoxAddress1").focus();
                return false;
            }


            var Landline = document.getElementById("textBoxLandline").value;

            if (Landline != "") {
                if (Landline.length < 6) {
                    alert("Please Provide Correct Landline No.");
                    document.getElementById("textBoxLandline").focus();
                    return false;
                }
            }

            var emailID = document.getElementById("textBoxEmailId").value;
            if (emailID != "") {
                if (emailcheck(emailID) == false) {
                    document.getElementById("textBoxEmailId").value = "";
                    document.getElementById("textBoxEmailId").focus();
                    return false;
                }
            }
            //            if (document.getElementById("DropDownListVehicleClass").selectedIndex == 0) {
            //                alert("Please Select Correct Vehicle Type");
            //                document.getElementById("DropDownListVehicleClass").focus();
            //                return false;
            //            }

            //            var VehicleMake = document.getElementById("textBoxVehicleMake").value;

            //            if (VehicleMake == "") {
            //                alert("Please Provide Vehicle Maker.");
            //                document.getElementById("textBoxVehicleMake").focus();
            //                return false;
            //            }

            if (document.getElementById("<%=DropDownListVehicleClass.ClientID%>").value == "--Select Vehicle Class--") {
                alert("Select correct Vehicle Class.");
                // document.getElementById("<%=DropDownListVehicleClass.ClientID%>").selectedIndex = 0;
                document.getElementById("<%=DropDownListVehicleClass.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=DropDownListVehicleModel.ClientID%>").value == "--Select Vehicle Model--") {
                alert("Select correct Vehicle Type.");
                //document.getElementById("<%=DropDownListVehicleModel.ClientID%>").selectedIndex = 0;
                document.getElementById("<%=DropDownListVehicleModel.ClientID%>").focus();
                return false;
            }
            //            if (document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").value == "--Select Vehicle Maker--") {
            //                alert("Select correct Vehicle Maker.");
            //                document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").focus();
            //                return false;
            //            }
            //            if (document.getElementById("<%=DropDownListModel.ClientID%>").value == "--Select Vehicle Model--") {
            //                alert("Select correct Vehicle Model.");
            //                //document.getElementById("<%=DropDownListModel.ClientID%>").selectedIndex = 0;
            //                document.getElementById("<%=DropDownListModel.ClientID%>").focus();
            //                return false;
            //            }
            //            var VehicleMake = document.getElementById("textBoxVehicleMake").value;

            //            if (VehicleMake == "") {
            //                alert("Please Provide Vehicle Maker.");
            //                document.getElementById("textBoxVehicleMake").focus();
            //                return false;
            //            }

            //            if (invalidChar(document.getElementById("textBoxVehicleMake"))) {
            //                alert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.");
            //                document.getElementById("textBoxVehicleMake").focus();
            //                return false;
            //            }

            if (document.getElementById("DropDownListVehicleModel").selectedIndex == 0) {
                alert("Please Select Correct Vehicle Model");
                document.getElementById("DropDownListVehicleModel").focus();
                return false;
            }
            var VehicleRegNo = document.getElementById("TextBoxVehicleRegNo").value;

            if (VehicleRegNo == "") {
                alert("Please Provide Vehicle Registration Number.");
                document.getElementById("TextBoxVehicleRegNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("TextBoxVehicleRegNo"))) {
                alert("Please Provide Valid Registration No.");
                document.getElementById("TextBoxVehicleRegNo").focus();
                return false;
            }

            if (!ValidRegNo(document.getElementById("TextBoxVehicleRegNo").value, document.getElementById("<%=LabelCreatedID.ClientID%>").innerText)) {
                alert("Please Provide Valid Registration No.");
                document.getElementById("TextBoxVehicleRegNo").focus();
                return false;
            }


            var textboxEngineNo = document.getElementById("textboxEngineNo").value;

            if (textboxEngineNo == "") {
                alert("Please Provide Vehicle Engine No.");
                document.getElementById("textboxEngineNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textboxEngineNo"))) {
                alert("Please Provide Valid Vehicle Engine No.");
                document.getElementById("textboxEngineNo").focus();
                return false;
            }

            var textBoxChassisNo = document.getElementById("textBoxChassisNo").value;
            if (textBoxChassisNo == "") {
                alert("Please Provide Vehicle Chassis No.");
                document.getElementById("textBoxChassisNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxChassisNo"))) {
                alert("Please Provide Valid Vehicle Chassis No.");
                document.getElementById("textBoxChassisNo").focus();
                return false;
            }

            if (document.getElementById("DropDownListOrderType").selectedIndex == 0) {
                alert("Please Select Correct order Type.");
                document.getElementById("DropDownListOrderType").focus();
                return false;
            }

            //            if (document.getElementById("DropDownListFrontPlateSize").selectedIndex == 0) {
            //                alert("Please Select Correct Front Plate Size.");
            //                document.getElementById("DropDownListFrontPlateSize").focus();
            //                return false;
            //            }
            //            if (document.getElementById("DropDownListRearPlateSize").selectedIndex == 0) {
            //                alert("Please Select Correct Rear Plate Size.");
            //                document.getElementById("DropDownListRearPlateSize").focus();
            //                return false;
            //            }

        }
        
    </script>
    <style>
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .9em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth
        {
            width: 140px !important;
        }
        #divwidth div
        {
            width: 140px !important;
        }
    </style>
    <style>
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .9em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth2
        {
            width: 120px !important;
        }
        #divwidth2 div
        {
            width: 120px !important;
        }
    </style>
    <style>
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .9em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth3
        {
            width: 150px !important;
        }
        #divwidth3 div
        {
            width: 150px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <table width="100%" border="0" align="left">
        <tr>
            <td>
                <div style="margin: 20px;" align="left">
                    <fieldset>
                        <legend>
                            <div align="left" style="margin-left: 10px; font-size: medium; color: Black">
                                <span>Add HSRP Records</span>
                            </div>
                        </legend>
                        <div>
                            <table style="background-color: #FFFFFF" border="0" align="left" width="100%" cellpadding="3"
                                cellspacing="1">
                                <tr>
                                    <td>
                                        <table style="background-color: #FFFFFF" width="100%" border="0" align="left" cellpadding="3"
                                            cellspacing="1">
                                            <tr>
                                                <td colspan="6">
                                                    <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                                                      <%--  <tr valign="top">
                                                            <td colspan="6">
                                                                <asp:Label ID="Label1" Text="Allowed RTO's :- " ForeColor="OrangeRed" Font-Size="Medium" runat="server" />
                                                                <asp:Label Font-Size="medium" ID="LabelCreatedID" ForeColor="OrangeRed" runat="server" />
                                                            </td>
                                                        </tr>--%>
                                                        <tr valign="top">
                                                            <td colspan="2" align="left" style="margin-left: 50px" width="258px" class="form_text">
                                                                <b>AUTHORIZATION INFORMATION</b>
                                                            </td>
                                                            <td class="form_text" nowrap="nowrap" align="left" width="150px">
                                                                Record Created By:
                                                            </td>
                                                            <td width="170px">
                                                                <asp:Label ID="LabelUSER" ForeColor="Blue" runat="server" />
                                                            </td>
                                                            <td class="form_text" nowrap="nowrap" align="left" width="140px">
                                                                Record Date:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="LabelCreatedDateTime" ForeColor="Blue" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" nowrap="nowrap" align="left" style="width: 12.8%">
                                                    HSRP Auth. No :<span class="alert">* </span>
                                                </td>
                                                <td nowrap="nowrap" style="width: 170px" align="left">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanelAuthNo" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox class="form_textbox11" onblur="this.value=removeSpaces(this.value);"
                                                                            Style="text-transform: uppercase" TabIndex="1" Width="160" ID="textBoxAuthorizationNo"
                                                                            runat="server"></asp:TextBox>
                                                                        <%--<div class="demo">
                                                                            <div class="ui-widget">
                                                                                <asp:TextBox ID="tbAuto" class="tb" runat="server">
                                                                                </asp:TextBox>
                                                                            </div>
                                                                        </div>--%>
                                                                        <div id="divwidth">
                                                                        </div>
                                                                        <asp:AutoCompleteExtender UseContextKey="true" ServiceMethod="GetCustomers1" MinimumPrefixLength="1" ServicePath="~/WCFService/ServiceForSuggestion.svc"
                                                                            CompletionInterval="10" EnableCaching="false" TargetControlID="textBoxAuthorizationNo"
                                                                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionSetCount="12"
                                                                            CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListElementID="divwidth">
                                                                        </asp:AutoCompleteExtender>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanelGo" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:LinkButton ID="buttonGo" runat="server" CausesValidation="false" OnClientClick="javascript:return CheckAuthNo();"
                                                                            Text="Go" class="button" TabIndex="2" OnClick="buttonGo_Click"></asp:LinkButton>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <%--<asp:Button ID="buttonGo" runat="server" OnClientClick="javascript:return CheckAuthNo();"
                                                        Text="Go" class="button" TabIndex="2" OnClick="buttonGo_Click" />--%>
                                                </td>
                                                <td class="form_text" nowrap="nowrap" align="left">
                                                    HSRP Auth. Date :<span class="alert">* </span>
                                                </td>
                                                <td align="left">
                                                    <table id="Table1" runat="server" style="margin-left: 8px;" align="left" cellspacing="0"
                                                        cellpadding="0" border="0">
                                                        <tr>
                                                            <td valign="top" onmouseup="OrderDate_OnMouseUp()">
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanelOrderDate" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <ComponentArt:Calendar TabIndex="3" ID="OrderDate" runat="server" PickerFormat="Custom"
                                                                            PickerCustomFormat="dd/MM/yyyy" ControlType="Picker" PickerCssClass="picker">
                                                                            <ClientEvents>
                                                                                <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                            </ClientEvents>
                                                                        </ComponentArt:Calendar>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td style="font-size: 10px;">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="form_text" align="left">
                                                    Mobile No :
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelMobile" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxMobileNo" runat="server" Style="text-transform: uppercase"
                                                                MaxLength="10" TabIndex="4" class="form_textbox11"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" align="left" nowrap="nowrap">
                                                    Customer Name :<span class="alert">* </span>
                                                </td>
                                                <td align="left" width="170px">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelCustomerName" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxCustomerName" Style="text-transform: uppercase" runat="server"
                                                                Width="160px" class="form_textbox11" TabIndex="5"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left" nowrap="nowrap">
                                                    Order Status :
                                                </td>
                                                <td align="left" style="margin-left: 8px">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelOrderStatus" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxOrderStatus" class="form_textbox11" Width="126px" TabIndex="6"
                                                                Enabled="false" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left">
                                                    Landline :
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelLandline" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxLandline" Style="text-transform: uppercase" MaxLength="12"
                                                                class="form_textbox11" runat="server" TabIndex="7"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" align="left">
                                                    Address1 :<span class="alert">* </span>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelAddress1" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxAddress1" Style="text-transform: uppercase" Width="480px"
                                                                Columns="400" Rows="2" class="form_textbox11" runat="server" TabIndex="8"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" align="left" width="141px">
                                                    Email Id :
                                                </td>
                                                <td align="left" width="180px" colspan="3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelEmail" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxEmailId" Width="480px" class="form_textbox11" runat="server"
                                                                TabIndex="9"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" nowrap="nowrap" align="left">
                                                    Cash Receipt No :
                                                </td>
                                                <td nowrap="nowrap" style="width: 170px" align="left">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanelCashReceiptNo" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="TextBoxCashReceiptNo" Style="text-transform: uppercase" onblur="this.value=removeSpaces(this.value);"
                                                                            MaxLength="10" class="form_textbox11" runat="server" TabIndex="10"></asp:TextBox>
                                                                        <div id="divwidth3">
                                                                        </div>
                                                                        <asp:AutoCompleteExtender ServiceMethod="GetCashReceiptNos" MinimumPrefixLength="1"
                                                                            ServicePath="~/WCFService/ServiceForSuggestion.svc" CompletionInterval="10" EnableCaching="false"
                                                                            TargetControlID="TextBoxCashReceiptNo" ID="AutoCompleteExtender3" runat="server"
                                                                            FirstRowSelected="false" CompletionSetCount="12" CompletionListCssClass="AutoExtender"
                                                                            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                            CompletionListElementID="divwidth3">
                                                                        </asp:AutoCompleteExtender>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:LinkButton Enabled="false" ID="LinkButtonGo3" runat="server" CausesValidation="false"
                                                                            OnClientClick="javascript:return CheckCashReceiptNo();" Text="Go" class="button"
                                                                            TabIndex="11" OnClick="LinkButtonGo3_Click"></asp:LinkButton>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                                            <tr valign="top">
                                                <td colspan="6" style="margin-left: 50px;" align="left" class="form_text">
                                                    <b>VEHICLE INFO</b>
                                                    <asp:TextBox Visible="false" Width="1px" ID="textBoxAddress2" runat="server" class="form_textbox11"
                                                        TabIndex="9"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" align="left" style="width: 12.8%">
                                                    Vehicle Class :<span class="alert">* </span>
                                                </td>
                                                <td align="left" width="200px">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleClass" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" AutoPostBack="true"
                                                                Width="170px" ID="DropDownListVehicleClass" runat="server" TabIndex="12" OnSelectedIndexChanged="DropDownListVehicleClass_SelectedIndexChanged">
                                                                <asp:ListItem Value="--Select Vehicle Class--" Text="--Select Vehicle Class--"></asp:ListItem>
                                                                <asp:ListItem Value="Transport" Text="Transport"></asp:ListItem>
                                                                <asp:ListItem Value="Non-Transport" Text="Non-Transport"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left" nowrap="nowrap" width="120px">
                                                    Vehicle Type :<span class="alert">* </span>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleModel" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList Style="margin-left: 8px" AutoPostBack="true" Font-Size="Small"
                                                                CausesValidation="false" ID="DropDownListVehicleModel" Width="170px" runat="server"
                                                                TabIndex="13" OnSelectedIndexChanged="DropDownListVehicleModel_SelectedIndexChanged">
                                                                <asp:ListItem Value="--Select Vehicle Model--" Text="--Select Vehicle Model--"></asp:ListItem>
                                                                <asp:ListItem Value="SCOOTER" Text="SCOOTER"></asp:ListItem>
                                                                <asp:ListItem Value="MOTOR CYCLE" Text="MOTOR CYCLE"></asp:ListItem>
                                                                <asp:ListItem Value="TRACTOR" Text="TRACTOR"></asp:ListItem>
                                                                <asp:ListItem Value="THREE WHEELER" Text="THREE WHEELER"></asp:ListItem>
                                                                <asp:ListItem Value="LMV" Text="LMV"></asp:ListItem>
                                                                <asp:ListItem Value="LMV(CLASS)" Text="LMV(CLASS)"></asp:ListItem>
                                                                <asp:ListItem Value="MCV/HCV/TRAILERS" Text="MCV/HCV/TRAILERS"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" align="left" nowrap="nowrap" width="141px">
                                                    Vehicle Maker :<%--<span class="alert">* </span>--%>
                                                </td>
                                                <td align="left" style="width: 180px">
                                                    <%-- <asp:DropDownList runat="server" Style="margin-left: 8px" DataTextField="VehicleMakerDescription"
                                                        DataValueField="VehicleMakerID" TabIndex="15" EnableViewState="true" Font-Size="Small"
                                                        AutoPostBack="true" ID="DropDownList44" 
                                                        onselectedindexchanged="DropDownList44_SelectedIndexChanged"></asp:DropDownList>--%>
                                                    <%-- <asp:DropDownList runat="server" Style="margin-left: 8px" DataTextField="VehicleMakerDescription"
                                                        DataValueField="VehicleMakerID" TabIndex="15" EnableViewState="true" Font-Size="Small"
                                                        AutoPostBack="true" ID="DropDownListVehicleMaker" 
                                                        onselectedindexchanged="DropDownList44_SelectedIndexChanged"></asp:DropDownList>--%>
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleMake" UpdateMode="Conditional"
                                                        ChildrenAsTriggers="true">
                                                        <ContentTemplate>
                                                            <asp:DropDownList Style="margin-left: 8px" EnableViewState="true" Font-Size="Small"
                                                                AutoPostBack="true" ID="DropDownListVehicleMaker1" Width="170px" DataTextField="VehicleMakerDescription"
                                                                DataValueField="VehicleMakerID" runat="server" TabIndex="14" OnSelectedIndexChanged="DropDownList44_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left" nowrap="nowrap" width="141px">
                                                    Vehicle Model :<%--<span class="alert">* </span>--%>
                                                </td>
                                                <td colspan="3" align="left" style="width: 180px">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleModel1" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <%-- <asp:TextBox ID="textBoxVehicleMake" class="form_textbox11" runat="server" TabIndex="12"></asp:TextBox>--%>
                                                            <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" CausesValidation="false"
                                                                ID="DropDownListModel" Width="170px" DataTextField="VehicleModelDescription"
                                                                DataValueField="VehicleModelID" runat="server" TabIndex="15">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" nowrap="nowrap" align="left">
                                                    Vehicle Reg No :<span class="alert">* </span>
                                                </td>
                                                <td nowrap="nowrap" style="width: 170px" align="left">
                                                    <table>
                                                        <tr>
                                                            <td nowrap="nowrap">
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleRegNo" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="TextBoxVehicleRegNo" onblur="this.value=removeSpaces(this.value);"
                                                                            Style="text-transform: uppercase" MaxLength="12" class="form_textbox11" runat="server"
                                                                            TabIndex="16"></asp:TextBox>
                                                                        <div id="divwidth2">
                                                                        </div>
                                                                        <asp:AutoCompleteExtender ServiceMethod="GetVehicleRegs" UseContextKey="true" MinimumPrefixLength="1"
                                                                            ServicePath="~/WCFService/ServiceForSuggestion.svc" CompletionInterval="10" EnableCaching="false"
                                                                            TargetControlID="TextBoxVehicleRegNo" ID="AutoCompleteExtender2" runat="server"
                                                                            FirstRowSelected="false" CompletionSetCount="12" CompletionListCssClass="AutoExtender"
                                                                            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                            CompletionListElementID="divwidth2">
                                                                        </asp:AutoCompleteExtender>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:LinkButton ID="LinkButtonGo2" runat="server" CausesValidation="false" OnClientClick="javascript:return CheckVehicleRegNo();"
                                                                            Text="Go" class="button" TabIndex="17" OnClick="LinkButtonGo2_Click"></asp:LinkButton>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="form_text" align="left">
                                                    Engine No :<span class="alert">* </span>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelEngineNo" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textboxEngineNo" Style="text-transform: uppercase" TabIndex="18"
                                                                Enabled="true" class="form_textbox11" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left">
                                                    Chassis No :<span class="alert">* </span>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelChassisNo" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxChassisNo" TabIndex="19" Style="text-transform: uppercase"
                                                                Enabled="true" class="form_textbox11" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                                            <tr valign="top">
                                                <td colspan="7" style="margin-left: 50px" align="left" class="form_text">
                                                    <b>NUMBER PLATE INFO</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" nowrap="nowrap" align="left" style="width: 13.4%">
                                                    Order Type :
                                                </td>
                                                <td align="left" width="193px">
                                                    <%--<asp:DropDownList Style="margin-left: 3px" AutoPostBack="true" CausesValidation="false"
                                                                ID="DropDownList1" runat="server" TabIndex="18" 
                                                        Width="170px" onselectedindexchanged="DropDownList1_SelectedIndexChanged1" 
                                                                 >
                                                                <asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                                                <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                                                <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                                                <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>
                                                            </asp:DropDownList>--%>
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelOrderType" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <%--  <asp:DropDownList Style="margin-left: 3px" AutoPostBack="true" CausesValidation="false"
                                                                ID="DropDownListOrderType" runat="server" TabIndex="18" Width="170px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                                                <asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                                                <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                                                <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                                                <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>
                                                            </asp:DropDownList>--%>
                                                            <asp:DropDownList Style="margin-left: 3px" AutoPostBack="true" CausesValidation="false"
                                                                ID="DropDownListOrderType" runat="server" TabIndex="20" Width="170px" OnSelectedIndexChanged="DropDownListOrderType_SelectedIndexChanged">
                                                                <asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                                                <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                                                <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                                                <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <%-- <asp:DropDownList Style="margin-left: 3px" AutoPostBack="true" CausesValidation="false"
                                                                ID="DropDownList1" runat="server" TabIndex="18" 
                                                        Width="170px" onselectedindexchanged="DropDownList1_SelectedIndexChanged" >
                                                                <asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                                                <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                                                <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                                                <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>
                                                            </asp:DropDownList>--%>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:UpdatePanel ID="UpdatePanelDropDownListFrontPlate" UpdateMode="Conditional"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <%-- <asp:DropDownList Style="margin-left: 3px" CausesValidation="false" AutoPostBack="true"
                                                                ID="DropDownListFrontPlateSize" Width="170px" DataTextField="ProductCode" DataValueField="ProductID"
                                                                runat="server" TabIndex="16" OnSelectedIndexChanged="DropDownListFrontPlateSize_SelectedIndexChanged">
                                                            </asp:DropDownList>--%>
                                                            <asp:Label ID="labelFrontPlateSize" Font-Size="Medium" Visible="false" ForeColor="Black" runat="server"></asp:Label>
                                                            <asp:DropDownList Style="margin-left: 3px" CausesValidation="false" 
                                                                ID="DropDownListFrontPlateSize" Width="170px" DataTextField="ProductCode" DataValueField="ProductID"
                                                                runat="server" TabIndex="21" >
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" align="left">
                                                    <asp:UpdatePanel ID="UpdatePanelSnap" ChildrenAsTriggers="true" UpdateMode="Conditional"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox Text="Snap Lock" Checked="true" Enabled="false" class="form_text" TextAlign="Right"
                                                                ID="checkBoxSnapLock" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanelThirdSticker" ChildrenAsTriggers="true" UpdateMode="Conditional"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox Text="Third Sticker" Enabled="false" class="form_text" TextAlign="Right"
                                                                ID="checkBoxThirdSticker" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:UpdatePanel ID="UpdatePanelDropDownListRearPlate" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>
                                                            <%--   <asp:DropDownList Style="margin-left: 3px" AutoPostBack="true" DataTextField="ProductCode"
                                                                DataValueField="ProductID" ID="DropDownListRearPlateSize" Width="170px" CausesValidation="false"
                                                                runat="server" TabIndex="17" OnSelectedIndexChanged="DropDownListRearPlateSize_SelectedIndexChanged">
                                                            </asp:DropDownList>--%>
                                                            <asp:Label ID="labelRearPlateSize" Font-Size="Medium" Visible="false" ForeColor="Black" runat="server"></asp:Label>
                                                            <asp:DropDownList Style="margin-left: 3px" DataTextField="ProductCode" 
                                                                DataValueField="ProductID" ID="DropDownListRearPlateSize" Width="170px" CausesValidation="false"
                                                                runat="server" TabIndex="22">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                                            <tr valign="top">
                                                <td colspan="10" align="left" class="form_text" style="margin-left: 50px">
                                                    <b>FINANCIAL INFO</b>
                                                </td>
                                            </tr>
                                            <%--  <tr>
                                                <td class="form_text" align="left" width="11%">
                                                    Invoice No :
                                                </td>
                                                <td align="left" width="196px">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelInvoiceNo" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox class="form_textbox11" ID="textBoxInvoiceNo" Enabled="false" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left" nowrap="nowrap">
                                                    Cash Receipt No :
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelCashReceiptNo" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxCashReceiptNo" class="form_textbox11" Enabled="false" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td class="form_text" align="left">
                                                    Total Amount :
                                                </td>
                                                <td align="left" width="193px">
                                                    <asp:UpdatePanel ID="UpdatePanelTotalAmount" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxTotalAmount" Width="50px" Enabled="false" class="form_textbox11"
                                                                runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" nowrap="nowrap" align="left">
                                                    VAT (%) :
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="UpdatePanelVat" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxVat" runat="server" Width="50px" Enabled="false" class="form_textbox11"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left" nowrap="nowrap" width="120px">
                                                    <span id="Span12">VAT Amount : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="UpdatePanelVatAmount" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxVatAmount" runat="server" Width="50px" Enabled="false" class="form_textbox11"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left" nowrap="nowrap">
                                                    Net Total :
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="UpdatePanelNetTotal" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxNetTotal" Width="50px" runat="server" Enabled="false" class="form_textbox11"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="alert" nowrap="nowrap" align="left">
                                                    <%--Service Tax Amount :--%>* Fields are mandatory.
                                                </td>
                                                <td colspan="4" align="left">
                                                    <asp:UpdatePanel ID="UpdatePanelMessage" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                                            <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td nowrap="nowrap" align="right" colspan="" style="margin-right: 10px; margin-left: 100px">
                                                    <asp:Button ID="buttonUpdate" runat="server" TabIndex="23" class="button" Visible="false"
                                                        Text="Update" />
                                                </td>
                                                <td align="right" nowrap="nowrap">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="buttonSave" runat="server" TabIndex="24" OnClientClick="javascript:return ValidateForm();"
                                                                class="button" Text="Save" Visible="false" OnClick="buttonSave_Click" />&nbsp;
                                                            <asp:Button Text="reset" ID="Button1" class="button" runat="server" TabIndex="25"
                                                                OnClientClick="this.form.reset();" OnClick="Button1_Click" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <input type="button" onclick="javascript:parent.googlewin.close();" tabindex="26"
                                                        name="buttonClose" id="buttonClose" value="Close" class="button" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" nowrap="nowrap" align="left">
                                                    <%-- Service Tax (%) :--%>
                                                    <asp:UpdatePanel ID="UpdatePanelServiceTaxAmount" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textboxServiceTaxAmount" Width="1px" Visible="false" Enabled="false"
                                                                class="form_textbox11" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="UpdatePanelServiceTax" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxServiceTax" Width="1px" Visible="false" Enabled="false" class="form_textbox11"
                                                                runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td colspan="6">
                                                    <asp:TextBox ID="HiddenFieldFrontPlate" Width="10px" Visible="false" Text="0" runat="server" />
                                                    <asp:TextBox ID="HiddenFieldRearPlate" Width="10px" runat="server" Text="0" Visible="false" />
                                                    <asp:TextBox ID="HiddenFieldSticker" runat="server" Width="10px" Text="0" Visible="false" />
                                                    <asp:TextBox ID="HiddenFieldScrew" runat="server" Width="10px" Text="0" Visible="false" />
                                                    <asp:TextBox ID="HiddenFieldFixing" runat="server" Width="10px" Text="0" Visible="false" />
                                                    <asp:TextBox ID="HiddenFieldFrontPlateCode" runat="server" Width="10px" Text="0"
                                                        Visible="false" />
                                                    <asp:TextBox ID="HiddenFieldRearPlateCode" runat="server" Width="10px" Text="0" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="8">
                                                    <ComponentArt:Calendar runat="server" ID="CalendarOrderDate" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <ComponentArt:Calendar runat="server" ID="CalendarHSRPAuthDate" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="ImgPollution" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="CalendarHSRPAuthDate_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
