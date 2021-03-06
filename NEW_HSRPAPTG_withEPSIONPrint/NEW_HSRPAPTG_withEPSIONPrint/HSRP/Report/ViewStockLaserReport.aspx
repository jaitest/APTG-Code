﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewStockLaserReport.aspx.cs" Inherits="HSRP.Report.ViewStockLaserReport" %>
<%@ Register assembly="ComponentArt.Web.UI" namespace="ComponentArt.Web.UI" tagprefix="ComponentArt" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
     <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />



<script type="text/javascript">
    function validate() {

        if (document.getElementById("<%=ddlStateName.ClientID%>").value == "--Select State--") {
            alert("Select State");
            document.getElementById("<%=ddlStateName.ClientID%>").focus();
            return false;
        }


    }
    </script>

    <script type="text/javascript">

        function OrderDatefrom_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDatefrom.getSelectedDate();
            CalendarOrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDatefrom.getSelectedDate();
            OrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnClick() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                CalendarOrderDatefrom.hide();
            }
            else {
                CalendarOrderDatefrom.setSelectedDate(OrderDatefrom.getSelectedDate());
                CalendarOrderDatefrom.show();
            }
        }

        function OrderDatefrom_OnMouseUp() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function OrderDateto_OnDateChange(sender, eventArgs) {   
            var fromDate = OrderDateto.getSelectedDate();
            CalendarOrderDateto.setSelectedDate(fromDate);

        } 

        function OrderDateto_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDateto.getSelectedDate();
            OrderDateto.setSelectedDate(fromDate);

        } 


        function OrderDateto_OnClick() {
            if (CalendarOrderDateto.get_popUpShowing()) {
                CalendarOrderDateto.hide();
            }
            else {
                CalendarOrderDateto.setSelectedDate(OrderDateto.getSelectedDate());
                CalendarOrderDateto.show();
            }
        }

        

        function OrderDateto_OnMouseUp() {
            if (CalendarOrderDateto.get_popUpShowing()) {
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
                                <tr>
                                    <td>
                                        <span class="headingmain">Stock Details</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                            </td>
                            </tr>
                            </table>
                            </td>
                            </tr>
                            </table>

   
    <table style="height: 51px;" width="80%">
        <tr>
            <td style="height: 40px;" width="20">
                </td>
            <td style="height: 40px;" valign="middle" width="80">
                <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization1" 
                                            ForeColor="Black" Font-Bold="True" Width="100px" />
            </td>
            <td style="width: 150px; height: 40px;" valign="middle">               
                   
                        <asp:DropDownList ID="ddlStateName" runat="server"
                             AutoPostBack="True" Height="22px" Width="120px" OnSelectedIndexChanged="ddlStateName_SelectedIndexChanged">
                        </asp:DropDownList>                    
                
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="DropDownListStateName" ErrorMessage="Select State" 
                            InitialValue="--Select State--"></asp:RequiredFieldValidator>--%>
                
            </td>
            <td style="width: 50px; height: 40px;">               
                   
                    <asp:Label ID="label2" runat="server" Font-Bold="True" ForeColor="Black" 
                        Text="Embossing Centers:" Width="100px" />
                
            </td>
            <td style="height: 40px;" width="80">       
                   
                    <asp:DropDownList ID="ddlEmbossingCenters" runat="server" 
                         Height="22px" Width="170px">
                        
                    </asp:DropDownList>
                
            </td>
         
                                                <td style="height: 40px" width="20"> <asp:Label Text="From:" runat="server" 
                                                        ID="labelDate" Font-Bold="True" 
                                                        ForeColor="Black" Width="60px" /> </td>
                                                <td style="height: 40px; width: 188px;">
                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                    <ContentTemplate>
                                                                        <ComponentArt:Calendar ID="OrderDatefrom" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    
    ControlType="Picker" PickerCssClass="picker" Height="22px" Width="79px">
                                                                            <ClientEvents>
                                                                                <SelectionChanged EventHandler="OrderDatefrom_OnDateChange" />
                                                                            </ClientEvents>
                                                                        </ComponentArt:Calendar>
                                                                        <img id="calendar_from_button"  alt="" onclick="OrderDatefrom_OnClick()" onmouseup="OrderDatefrom_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                <td style="height: 40px" width="30">
                                                    <asp:Button ID="btnexport" runat="server" onclick="btnexport_Click" Text="PhysicalVSERP" 
                                                        Font-Bold="True" BackColor="Orange"  ForeColor="#3333FF" Height="21px" Width="224px"/>
                                                </td>
                                                 <%--<td style="height: 40px">
                                                    <asp:Button ID="btnDetail" runat="server" Text="Detail" 
                                                        Font-Bold="True" ForeColor="#3333FF" Height="18px" Width="61px" OnClick="btnDetail_Click" />
                                                </td>--%>
                                           
                                               <%--<td style="height: 40px" width="30">
                                                    <asp:Button ID="btnproctiondispatch" runat="server"  Text="PIPVSReceived(Detail)"  Visible="True"
                                                        Font-Bold="True" ForeColor="#3333FF" Height="21px" Width="224px" OnClick="btnproctiondispatch_Click"/>
                                                </td>--%>
                                              
                                                 <%--<td style="height: 40px" width="50">
                                                    <asp:Button ID="btnnDetails" runat="server"  Text="Detail"  Visible="True"
                                                        Font-Bold="True" ForeColor="#3333FF" Height="21px" Width="224px" OnClick="btnnDetails_Click"/>
                                                </td>--%>
                                               
                                                

              <%-- <td style="height: 40px" width="30">
                                                    <asp:Button ID="btnassign" runat="server"  Text="ASSIGNMENT VS PRODUCTION" 
                                                        Font-Bold="True" ForeColor="#3333FF" OnClick="btnassign_Click"/>
                   </td>--%>
             <%--<td style="height: 40px" width="30">
                   <asp:Button ID="Button1" runat="server"  Text="Detail" 
                                                        Font-Bold="True" ForeColor="#3333FF" OnClick="Button1_Click"/>
                   </td>--%>
                                                
        </tr>
      
        
        <tr  align="center">
                                                <td colspan="4">
                                                    &nbsp;</td>
                                                <td>
                                                    <%--<asp:GridView ID="grdpending" runat="server" CellPadding="5" ForeColor="#333333"  AutoGenerateColumns="false"
                                        GridLines="None" Width="350px" Visible="true">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                                 
                                                    <asp:BoundField HeaderText="Embossing Center" DataField="Embossing Center" />
                                                    <asp:BoundField HeaderText="AssignedLaserCode" DataField="Assigned LaserCode" />
                                                    <asp:BoundField HeaderText="ToDayProd" DataField="Today Prod." />
                                                    <asp:BoundField HeaderText="Day1" DataField="Day1" />
                                                    <asp:BoundField HeaderText="Day2" DataField="Day2" />
                                                    <asp:BoundField HeaderText="Day3" DataField="Day3" />
                                                     <asp:BoundField HeaderText="Day4" DataField="Day4" />
                                                    <asp:BoundField HeaderText="Day5orMore" DataField="Day5 or More" />
                                                </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" /> 
                                    
                                    </asp:GridView>--%></td>
            


            
            <td colspan="2">&nbsp;</td>
        </tr>
        <td colspan="6">
            <asp:Label ID="Label1" runat="server" ForeColor="#CC3300"></asp:Label>
        </td>

        <tr>
             <td>
                                                    &nbsp;</td>
             <td colspan="6">
                                                    <ComponentArt:Calendar runat="server" ID="CalendarOrderDatefrom" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" 
                                                        NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDatefrom_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                            </tr>
                                            
                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                            
                                            
                                        </table>
   
                                            
</asp:Content>

