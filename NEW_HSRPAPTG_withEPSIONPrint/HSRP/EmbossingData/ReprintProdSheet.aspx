﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ReprintProdSheet.aspx.cs" Inherits="HSRP.EmbossingData.ReprintProdSheet" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">


        function editAssignLaser(i) {

            googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserAssignCode.aspx?Mode=Edit&HSRPRecordID=" + i, "Update Laser Details", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                //window.location.href = "AssignLaserCode.aspx";
                return true;
            }
        }

        function editMakeLaserFree(i) { // Define This function of Send Assign Laser ID 
            // alert("MakeLaserFree" + i);

            googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserCodeEmbossing.aspx?Mode=Embossing&HSRPRecordID=" + i, "Update Laser Details", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                //window.location.href = "AssignLaserCode.aspx";
                return true;
            }
        }

        function editEmbossing(i) { // Define This function of Send Assign Laser ID 
            //  alert("Embossing" + i);

            googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserCodeMakeFree.aspx?Mode=Embossing&HSRPRecordID=" + i, "Update Laser Details", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = "AssignLaserCode.aspx";
                return true;
            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserAssignCode.aspx?Mode=New", "Add Secure Devices User", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                // window.location = 'AssignLaserCode.aspx';
                return true;
            }
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
        function validateStatus() {


        }
    
    </script>
    
    <%-- </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListClient"   /> 
                                            </Triggers>
                                        </asp:UpdatePanel>--%>
    
    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select RTO Name--") {
                alert("Please Select RTO Name");
                document.getElementById("<%=dropDownListClient.ClientID%>").focus();
                return false;
            }
           
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
                                    <td > 
                                          <asp:Label ID="Label4" class="headingmain" runat="server"  >Challan/Embossing</asp:Label> 
                                          
                                    </td> 
                                </tr>

                                <tr id="TRRTOHide" runat="server">
                                    <td > 
                                          <asp:Label ID="dataLabellbl" class="headingmain" runat="server"  >Allowed RTO's :</asp:Label> 
                                          <asp:Label ID="lblRTOCode" class="form_Label_Repeter"  runat="server">Allowed RTO's : </asp:Label> 
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
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                    <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" /> 
                                    </td>
                                    <td valign="middle" align="center">
                                        <%--<asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" OnCheckedChanged="CHKSelect1_CheckedChanged" />--%>
                                             <asp:DropDownList Visible="true" ID="dropDownListClient" CausesValidation="false" Width="140px"
                                                    runat="server" DataTextField="RTOLocationName" AutoPostBack="false"
                                                    DataValueField="RTOLocationID" 
                                                       >
                                                </asp:DropDownList>
                                        <%-- <asp:TextBox ID="txtFLaserCode" runat="server"></asp:TextBox>--%>
                                    </td>
                                    
                                    <td valign="middle" Visible="false" class="form_text" nowrap="nowrap" 
                                        align="left"> <asp:Label Text="From:"  runat="server" ID="labelDate" /> &nbsp;&nbsp;</td>
                                    <td valign="top"   onmouseup="OrderDate_OnMouseUp()" align="left">
                                                                <ComponentArt:Calendar ID="OrderDate" runat="server"  PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"  
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                     onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" /></td>
                                                           
                                                            <td valign="top" align="left">
                                                                &nbsp;</td>
                                   <td valign="middle" class="form_text" nowrap="nowrap"> &nbsp;&nbsp;<asp:Label Text="To:"   runat="server" ID="labelTO" /> &nbsp;&nbsp;</td>
                                   <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()"   align="left">
                                                                <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy" 
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                            </td>
                                                      
                                                            <td valign="top" align="left"  Visible="false">
                                                                <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                                    class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>


                               

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                       
                                        &nbsp;&nbsp;&nbsp;  

                                             <asp:Button ID="btnGO"  Width="58px"  runat="server" 
                                            Text="GO" ToolTip="Please Click for Report" BackColor="Orange" ForeColor="#000000"
                                            class="button"  OnClientClick=" return validate()" 
                                            onclick="btnGO_Click"  /> &nbsp;&nbsp;&nbsp;  
                                         
                                    </td>
                                     
                                </tr>
                                <tr>    <td valign="bottom" class="form_text" nowrap="nowrap" style="height: 18px"> 
                                    </td>
                                    <td valign="bottom" style="height: 18px" > </td>
                                                 <td valign="middle" class="form_text" nowrap="nowrap" 
                                        style="height: 18px"> 
                                                     </td>
                                    <td valign="middle" style="height: 18px"> 
                                        </td>
                                    <td dir="ltr" valign="middle" class="form_text" style="height: 18px">
                                        </td>
                                    <td align="left" valign="middle" style="height: 18px">
                                        </td>
                                    <td colspan="2" style="height: 18px">
                                        </td>
                                    <td align="left" valign="middle" style="height: 18px">
                                        </td>
                                </tr>
                                <tr>    <td valign="middle" class="form_text" nowrap="nowrap"> 
                                   <asp:Label Text="Production Sheet No:" Visible="False" runat="server" 
                                        ID="labelOrderStatus" />&nbsp;&nbsp; </td>
                                    <td valign="top" colspan="2" > 
                                        <asp:DropDownList   DataTextField ="newpdfrunningno" 
                                            DataValueField="newpdfrunningno" Visible="false"
                                            ID="dropdownDuplicateFIle" CausesValidation="false" runat="server" 
                                            onselectedindexchanged="dropdownDuplicateFIle_SelectedIndexChanged" 
                                            AutoPostBack="True" Width="150px"  > 
                                                </asp:DropDownList>&nbsp; </td>
                                    <td valign="middle" align="left"> 
                                        &nbsp; 
                                    </td>
                                    <td dir="ltr" valign="middle" class="form_text">
                                   <asp:Label Text="Challan No:" Visible="False" runat="server" 
                                        ID="labelOrderStatus0" />
                                    </td>
                                    <td align="left" valign="middle">
                                    <asp:TextBox ID="txtInvoiceNo" runat="server" Visible="False"></asp:TextBox>
                                    </td>
                                   
                                        <td dir="ltr" valign="middle" class="form_text"> </td>
                                    <td dir="ltr" valign="middle" class="form_text">
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td >
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>
                                   <tr>
                        <td align="center" style="padding-top: 10px">
                              <asp:GridView ID="gvRecords" runat="server" BackColor="White" Visible="false" 
                                AutoGenerateColumns="False" OnPageIndexChanging="gvRecords_PageIndexChanging"
                                PageSize="25" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" 
                                            DataKeyNames="hsrprecordid" >
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E"  />
                                <Columns>
                                    <asp:TemplateField Visible="False">
                                        <HeaderTemplate>
                                            Select
                                           <%--<asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" OnCheckedChanged="CHKSelect1_CheckedChanged" />--%></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CHKSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Vehicle Reg No</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVehicleRegNo" runat="server" Text='<%#Eval("VehicleRegNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            F Laser Code</HeaderTemplate>
                                        <ItemTemplate>
                                        
                                            <asp:TextBox ID="txtFLaserCode" runat="server" Text='<%#Eval("hsrp_front_lasercode") %>' Enabled="false"></asp:TextBox>
                                            <%-- <asp:TextBox ID="txtFLaserCode" runat="server"></asp:TextBox>--%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            R Laser Code</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRlaserCode" runat="server" Text='<%#Eval("hsrp_rear_lasercode") %>' Enabled="false"></asp:TextBox>
                                            <%-- <asp:TextBox ID="txtRlaserCode" runat="server"></asp:TextBox>--%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="id" runat="server" Text='<%#Eval("hsrprecordid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField >
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderStatus" runat="server" Text='<%#Eval("OrderStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td align="center" style="padding-top: 10px">
                          
                            &nbsp;</td>
                    </tr>
                                   <tr>
                        <td align="center" style="padding-top: 10px">
                            <asp:Label ID="LblMessage" runat="server" Text=""></asp:Label>
                            <asp:Button ID="btnChalan" runat="server" Text="Download Production Sheet" 
                                CssClass="button" BackColor="Orange" ForeColor="#000000"
                               OnClick="btnChalan_Click" Width="173px" Visible="False"  />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button1" runat="server" CssClass="button" BackColor="Orange" ForeColor="#000000"
                                onclick="Button1_Click" Text="Records In PDF" Visible="False" />
                        &nbsp;&nbsp;
                            <asp:Button ID="Button2" runat="server" CssClass="button" BackColor="Orange" ForeColor="#000000"
                                onclick="Button2_Click" Text="Download Invoice" Visible="False" />
                        </td>
                        <td align="center" style="padding-top: 10px">
                          
                        </td>
                    </tr>
                                <tr>
                                                <td colspan="2">
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
                                                <td colspan="2">
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
            </td>
        </tr>
    </table>
    <br /><asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>
