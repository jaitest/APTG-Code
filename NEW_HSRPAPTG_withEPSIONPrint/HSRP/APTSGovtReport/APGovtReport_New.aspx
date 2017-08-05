<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="APGovtReport_New.aspx.cs" Inherits="HSRP.APTSGovtReport.APGovtReport" CompilationMode="Auto" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        <%--function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
        }--%>
    
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
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                
                                
                                <tr id="TR1" runat="server">
                                    <td > 
                                          <asp:Label ID="Label4" class="headingmain" runat="server"  > Report:Download AP Govt Data </asp:Label> 
                                          
                                    </td> 
                                </tr>

                               
                            </table>
                        </td>
                    </tr>
                   
                       <tr id="TR11" runat="server">
                        <td colspan="2"  class="headingmain" style="height: 87px">
                           
                            1. Details of Vehicles With Registration Numbers Not Isueed By Transport Department&nbsp;&nbsp;<br/>
                            <i>Direct Call To Govt Server Method :GetAuthorizationnotRegistered </i><br /><br />
                      </td>
                         
                            <td valign="middle" class="form_text" nowrap="nowrap" style="height: 87px">
                               <asp:Button ID="APGetAuthno"  Width="158px"  runat="server" 
                                            Text="DownloadAPGetAuthNo" ToolTip="APGetAuthNo" Visible="true"
                                            class="button"  OnClientClick=" return validate()" OnClick="APGetAuthno_Click"/>
                      </td>
                          
                    </tr>
                      <tr>
                     <td colspan="2"  class="headingmain">
                            2. Details of Vehicles With Registration Numbers Not Recieved from Govt Other Then Not Isueed By Transport Department<br/>
                           
                      </td>
                           <td>
                               
                                <asp:Button ID="BtnApreport"  Width="157px"  runat="server" 
                                            Text="DowonloadAPGetReport" ToolTip="DowonloadAPGetReport"
                                            class="button"  OnClientClick=" return validate()" OnClick="BtnApreport_Click"/>
                               
                               <asp:Button ID="btnOfflineAPReport" Width="182px" runat="server" Text="DownloadAPGetOfflineReport" OnClick="btnOfflineAPReport_Click"/>

                      </td>

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
                                    <td align="center" style=" padding:10px">
                              <%--<asp:GridView ID="GridView1" runat="server" BackColor="White" AutoGenerateColumns="false"
                                PageSize="25" AllowPaging="true" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" >
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                                <Columns>
                                   
                                    <asp:TemplateField HeaderText="S.No">
                                      <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                      </ItemTemplate>
                                      </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            RTO Name</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblrtolocation" runat="server" Text='<%#Eval("RTOName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                          Performance</HeaderTemplate>
                                        <ItemTemplate>
                                        
                                            <asp:Label ID="lblPerformance" runat="server" Text='<%#Eval("Performance") %>'></asp:Label>
                                           
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                  
                                   
                                    
                                </Columns>
                            </asp:GridView>--%>
                                    </td>
                                </tr>
                                 
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
    <br /><asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>
