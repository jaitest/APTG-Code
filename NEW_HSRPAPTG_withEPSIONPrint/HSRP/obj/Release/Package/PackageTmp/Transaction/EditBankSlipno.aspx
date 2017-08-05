<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EditBankSlipno.aspx.cs" Inherits="HSRP.Master.EditBankSlipno" %>
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
    <%-- <asp:TextBox ID="txtRlaserCode" runat="server"></asp:TextBox>--%>
    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=dropDownRtoLocation.ClientID%>").value == "--Select RTO Name--") {
                alert("Please Select RTO Name");
                document.getElementById("<%=dropDownRtoLocation.ClientID%>").focus();
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
                                    <td>
                                        <asp:Label ID="Label4" class="headingmain" runat="server">HSRP Without Mac </asp:Label>
                                    </td>
                                </tr>
                               <%-- <tr id="TRRTOHide" runat="server">
                                    <td>
                                        <asp:Label ID="dataLabellbl" class="headingmain" runat="server">Allowed RTO's :</asp:Label>
                                        <asp:Label ID="lblRTOCode" class="form_Label_Repeter" runat="server">Allowed RTO's : </asp:Label>
                                    </td>
                                </tr>--%>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0"
                                class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Dealer Name:" Visible="true" runat="server" ID="labelClient" />
                                    </td>
                                    <td valign="middle" class="form_text">
                                        <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList Visible="true" ID="dropDownRtoLocation" CausesValidation="false"
                                                    Width="140px" runat="server" DataTextField="name" AutoPostBack="false"
                                                    DataValueField="DealerID" OnSelectedIndexChanged="dropDownRtoLocation_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownRtoLocation" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnGO" Width="58px" runat="server" Text="GO" ToolTip="Please Click for Report"
                                            class="button" OnClientClick=" return validate()" OnClick="btnGO_Click" />
                                        &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                    </td>
                                   
                                    <td valign="middle" visible="false" class="form_text" nowrap="nowrap">
                                        &nbsp;&nbsp;
                                        <asp:Label Text="From:" runat="server" ID="labelDate" Visible="False" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left">
                                        <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker" Visible="False">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </td>
                                    <td valign="top" align="left">
                                        <%--<img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                            onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />--%>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="To:" runat="server" ID="labelTO" Visible="False" />
                                        
                                    </td>
                                    <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()" align="left">
                                        <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker" Visible="False">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </td>
                                    <td valign="top" align="left" visible="false">
                                        <%--<img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                            class="calendar_button" src="../images/btn_calendar.gif" />--%>
                                    </td>
                                    
                                </tr>
                                </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                
                                    <td align="left">
                                        <asp:Label ID="LblMessage" runat="server" Text=""></asp:Label>
                                        &nbsp;&nbsp;
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#83f442" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="padding: 10px">
                                       <asp:GridView ID="gvEmpDesignation" runat="server" Width="100%" AutoGenerateColumns="false" ShowFooter="true" ForeColor="Black" Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px" onrowupdating="gvEmpDesignation_RowUpdating" onrowcancelingedit="gvEmpDesignation_RowCancelingEdit"  onrowediting="gvEmpDesignation_RowEditing">
                                                          
                                           
                                           <Columns>

                                                <asp:TemplateField HeaderText="Transaction ID" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransactionid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TransactionID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>           
                                                    <asp:Label ID="lblTransactionEditid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TransactionID") %>'></asp:Label>           
                                                </EditItemTemplate>                      
                                            </asp:TemplateField>

                                               <asp:TemplateField HeaderText="Deposit Date" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDepositDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Deposit Date") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>           
                                                    <asp:Label ID="lblDepositDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Deposit Date") %>'></asp:Label>           
                                                </EditItemTemplate>                      
                                            </asp:TemplateField>
 
               
                                             

                                                <%--<asp:BoundField HeaderText="Transaction ID" DataField="TransactionID" />
                                                <asp:BoundField HeaderText="Deposit Date" DataField="Deposit Date" />--%>

                                               <asp:TemplateField HeaderText="Bank Slip No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBankSlipNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "BankSlipNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate> 
                                                              
                                                    <asp:TextBox ID="txtEditBankSlipNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "BankSlipNo") %>'></asp:TextBox>           
                                                </EditItemTemplate>                       
                                            </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="Bank Slip No" DataField="BankSlipNo" />
                                                <asp:BoundField HeaderText="Branch Name" DataField="BranchName" />--%>
                                               <asp:TemplateField HeaderText="Branch Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBranchName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "BranchName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate> 
                                                    <asp:Label ID="lblBranchName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "BranchName") %>'></asp:Label>          
                                                           
                                                </EditItemTemplate>                       
                                            </asp:TemplateField>
                                               <asp:TemplateField HeaderText="Deposit Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDepositAmount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DepositAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblDepositAmount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DepositAmount") %>'></asp:Label>           
                                                           
                                                </EditItemTemplate>                       
                                            </asp:TemplateField>

                                               <asp:TemplateField HeaderText="Deposit By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDepositBy" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DepositBy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate> 
                                                    <asp:Label ID="lblDepositBy" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DepositBy") %>'></asp:Label>          
                                                    
                                                </EditItemTemplate>                       
                                            </asp:TemplateField>
                                               <asp:TemplateField HeaderText="DealerName And ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDealer" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Dealer") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>           
                                                    <asp:Label ID="lblDealer" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Dealer") %>'></asp:Label>           
                                                </EditItemTemplate>                       
                                            </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Deposit Location">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDepositLocation" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DepositLocation") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>           
                                                    <asp:Label ID="lblDepositLocation" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DepositLocation") %>'></asp:Label>
                                                </EditItemTemplate>                       
                                            </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Amount Clearance Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmtClearDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AmtClearDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>  
                                                    <asp:Label ID="lblAmtClearDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AmtClearDate") %>'></asp:Label>         
                                                    
                                                </EditItemTemplate>                       
                                            </asp:TemplateField>
                                              <%--  <asp:BoundField HeaderText="Deposit Amount" DataField="DepositAmount" />
                                                <asp:BoundField HeaderText="Deposit By" DataField="DepositBy" />
                                                <asp:BoundField HeaderText="DealerName And ID" DataField="Dealer" />
                                                <asp:BoundField HeaderText="Deposit Location" DataField="DepositLocation" />
                                                <asp:BoundField HeaderText="Amount Clearance Date" DataField="AmtClearDate" />
                                                <asp:BoundField HeaderText="Approved By" DataField="ApprovedBy" />
                                                <asp:BoundField HeaderText="Approved Date" DataField="ApprovedDate" />
                                                <asp:BoundField HeaderText="Cheque Approved By" DataField="ChqrecBy" />
                                                <asp:BoundField HeaderText="Cheque Approved Date" DataField="ChqrecDateTime" />
                                                <asp:BoundField HeaderText="Rejected By" DataField="RejectedBy" />
                                                <asp:BoundField HeaderText="Reject Date" DataField="RejectDate" />--%>

                                              <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>                       
                                                    <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit"  Text="Edit"  />                                                      
                                                </ItemTemplate>
                                                <EditItemTemplate>
                         
                                                     <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update"  Text="Update"  />
                                                      <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel"  Text="Cancel" />
                                                </EditItemTemplate>
                       
                                            </asp:TemplateField>     
                                            </Columns>   
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                            <SortedAscendingHeaderStyle BackColor="#848384" />
                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                            <SortedDescendingHeaderStyle BackColor="#575357" />   
            </asp:GridView>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td align="center" style="padding-top: 10px">
                                        <asp:Button ID="btnUpdate" runat="server" Text="Save" CssClass="button"
                                            OnClick="btnUpdate_Click" Visible="False" Height="21px" BackColor="Orange" ForeColor="#000000"/>
                                        <asp:Button ID="btnSave" runat="server" Text="Sticker" CssClass="button" Visible="false" BackColor="Orange" ForeColor="#000000"
                                            OnClick="btnSticker_Click" />
                             <asp:Button ID="Button1" runat="server" Text="Mirror Sticker" CssClass="button" Visible="false" BackColor="Orange" ForeColor="#000000"
                                             onclick="Button1_Click"/>
                                    
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    
                                    </td>
                                    <td align="center" style="padding-top: 10px">
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td colspan="6">
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
            </td>
        </tr>
    </table>
    <br />
    <asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>

