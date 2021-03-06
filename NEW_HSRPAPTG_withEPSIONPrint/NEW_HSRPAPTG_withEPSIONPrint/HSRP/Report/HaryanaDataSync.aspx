﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HaryanaDataSync.aspx.cs" Inherits="HSRP.Report.HaryanaDataSync" %>
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

        if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
            alert("Select State");
            document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
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
                                        <span class="headingmain"> Data Sync Report</span>
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
                   
                        <asp:DropDownList ID="DropDownListStateName" runat="server" AutoPostBack="True" CausesValidation="false" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
 Height="22px" Width="120px" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                        </asp:DropDownList>                    
                
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="DropDownListStateName" ErrorMessage="Select State" 
                            InitialValue="--Select State--"></asp:RequiredFieldValidator>
                
            </td>
            
                                                <td style="height: 40px" width="30">
                                                    <asp:Button ID="btnexport" runat="server" onclick="btnexport_Click" Text="Export" 
                                                        Font-Bold="True" ForeColor="#3333FF"/>
                                                </td>

               <td style="height: 40px"  width="30">
                                                    &nbsp;</td>
             <td style="height: 40px" width="30">
                   <br />
                   </td>
            <td>&nbsp;</td>
            <td style="height: 40px" width="30">
                   &nbsp;</td>
                                                
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
            <td style="height: 20px" width="10px">
               
          </td>


        </tr>
        <td colspan="6">
            <asp:Label ID="Label1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
        </td>

        <tr>
             <td>
                                                    &nbsp;</td>
             <td colspan="6">
                                                    &nbsp;</td>
                                            </tr>
                                            
                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                            
                                            
                                        </table>
    
                                            
</asp:Content>

