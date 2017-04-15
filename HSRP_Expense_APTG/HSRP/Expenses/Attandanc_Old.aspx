<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Attandanc_Old.aspx.cs" Inherits="HSRP.Expenses.Attandanc" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />--%>
    
    <%--<script language="javascript" type="text/javascript">

        function OrderDate_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDate.getSelectedDate();
            CalendarOrderDate.setSelectedDate(fromDate);
        }A

        function OrderDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDate.getSelectedDate();
            OrderDate.setSelectedDate(fromDate);
        }

        function OrderDate_OnClick() {
            if (CalendarOrderDate.get_popUpShowing()) {
                CalendarOrderDate.hide();
            }
            else
            {
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
        
    </script>--%>
    <style type="text/css">
        .style4 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 210px;
            padding-left: 20px;
        }
    </style>
    
    <div style="width: 75%; margin-left:100px; height:350px;">
        
          <table border="0" align="center" style="height: 348px; width:100%">
                 
                    <tr>
                  
                   
                         <td valign="middle" class="form_text" nowrap="nowrap" >
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> &nbsp;&nbsp;
                                    </td>
                         <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                           >
                                        </asp:DropDownList>
                                    </td>
                         <td valign="middle" class="form_text" nowrap="nowrap">
                                    <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" /> 
                                    </td>
                         <td valign="middle" class="form_text">
                                       <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate> 
                                             <asp:DropDownList AutoPostBack="true" Visible="true" ID="dropDownListClient" CausesValidation="false" Width="140px"
                                                    runat="server" DataTextField="RTOLocationName" 
                                                    DataValueField="RTOLocationID" OnSelectedIndexChanged="dropDownListClient_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListClient" /> 
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>

                         <%--<td class="style4" style="width:160px" >Employee Name 

                        </td>--%>
                         <td style="width:110px">
                            <asp:DropDownList ID="ddlUserAccount" runat="server" Style="margin-left: 5px" TabIndex="3" Width="165px" AutoPostBack="true" Visible="false">
                            </asp:DropDownList>
                        </td>
                         <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="button1" runat="server" TabIndex="11" class="button_go" Text="Save" OnClientClick=" return validate()" OnClick="ButtonSubmit_Click" ValidationGroup="save" Visible="false" />
                            &nbsp;
                            <asp:Button ID="btnintime" runat="server" class="button_go" Text="InTime/OutTime" TabIndex="12" OnClick="btnintime_Click" Visible="true"/>
                             <%-- &nbsp;
                            <asp:Button ID="btnouttime" runat="server" class="button_go" Text="Out" TabIndex="12" OnClick="btnouttime_Click" />--%>
                         
                        </td>
                        
                      
                       
                    </tr>
                 

                
                        <tr>
                        <td colspan="3">
                            <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label><br />
                            <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                        </td>   
                     </tr> 
              <tr>
                                    <td colspan="8" align="center" id="gridTD" runat="server">
                    <asp:Panel runat="server" ID="Panel1" ScrollBars="Vertical" Height="570px" Width="400px" Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px">
                              <asp:GridView ID="grd" runat="server" BackColor="White" AutoGenerateColumns="false" 
                                PageSize="25" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3"  DataKeyNames="Emp_ID"  >
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
                                            S.No</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="SrNo" runat="server" Text='<%#Eval("SNo") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                           EMP ID
                                            </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Emp_ID" runat="server" Text='<%#Eval("Emp_ID") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            EMP Name</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Emp_Name" runat="server" Text='<%#Eval("Emp_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                          <%--  Select--%>
                                           <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" OnCheckedChanged="CHKSelect1_CheckedChanged" Visible="false" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox CssClass="testing" ClientIDMode="Static" ID="CHKSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            F Laser Code</HeaderTemplate>
                                        <ItemTemplate>
                                        
                                            <asp:TextBox ID="txtFLaserCode" runat="server" Text='<%#Eval("hsrp_front_lasercode") %>' Enabled="false"></asp:TextBox>
                                       
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            R Laser Code</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRlaserCode" runat="server" Text='<%#Eval("hsrp_rear_lasercode") %>' Enabled="false"></asp:TextBox>
                                          
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
                                        Order Status
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderStatus" runat="server" Text='<%#Eval("OrderStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    
                                </Columns>
                            </asp:GridView>
                          </asp:Panel>
                </td>
                                    <caption>
                                        &nbsp;</td>
                                    </caption>
                                </tr>
             <%-- <tr>
                <td colspan="8" align="center" id="gridTD" runat="server">
                    <asp:Panel runat="server" ID="Panel1" ScrollBars="Vertical" Height="250px" Width="855px" Font-Bold="True" BorderColor="#3FAFBC" BorderStyle="Groove" BorderWidth="1px">
                        <asp:GridView ID="grd" runat="server" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                            ShowHeader="true" Width="100%" BackColor="White" BorderColor="#FFCC99" BorderStyle="Solid" BorderWidth="1px">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>                                       
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
                    </asp:Panel>
                </td>
            </tr>            --%>      
                </table>
   
    </div>

</asp:Content>
