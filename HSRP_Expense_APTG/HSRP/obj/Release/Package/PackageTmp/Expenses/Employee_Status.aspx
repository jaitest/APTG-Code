<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Employee_Status.aspx.cs" Inherits="HSRP.Expenses.Employee_Status" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("textboxBoxHSRPState").value == "") {
                alert("Please Provide Designation Name");
                document.getElementById("textboxBoxHSRPState").focus();
                return false;
            }
            else {
                return true;
            }
        }

    </script>
    <style type="text/css">
        .GridviewDiv {
            font-size: 100%;
            font-family: 'Lucida Grande', 'Lucida Sans Unicode', Verdana, Arial, Helevetica, sans-serif;
            color: #303933;
            text-align:center;
        }

        .headerstyle {
            color: #303933;
            border-right-color: #abb079;
            border-bottom-color: #abb079;
            background-color: #d6b600;
            padding: 0.5em 0.5em 0.5em 0.5em;
            text-align: center;
        }
    </style>

    <div>

        <div style="margin:20px;" align="left">
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                <tr>                                
                    <td  class="form_text" nowrap="nowrap">
                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                    </td>
                    <td valign="middle">
                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" Width="102px"  >
                        </asp:DropDownList>
                    </td>

                    <td class="form_text" nowrap="nowrap">
                                        <asp:Label Text="District Name:" runat="server" ID="Label3" Visible="true"/>
                                    </td>
                                    <td valign="middle">                                         
                                           <asp:DropDownList ID="ddldistrictname" runat="server" Style="margin-left: 5px" TabIndex="3" Width="171px" DataTextField="district" DataValueField="district" AutoPostBack="true" OnSelectedIndexChanged="ddldistrictname_SelectedIndexChanged" >                                             
                                          </asp:DropDownList>
                                    </td>

                   <%-- <td class="form_text" nowrap="nowrap">
                    <asp:Label Text="Location Name:" runat="server" ID="lbllocation" Visible="true"/>
                   </td>
                    <td valign="middle">
                        <asp:DropDownList ID="ddllocation" runat="server" Style="margin-left: 5px" TabIndex="3" Width="171px" DataTextField="RTOLocationName"  DataValueField="RTOLocationID" AutoPostBack="true" OnSelectedIndexChanged="ddllocation_SelectedIndexChanged">                                             
                        </asp:DropDownList>
                        
                    </td>--%>

                     <td class="form_text" nowrap="nowrap">
                    <asp:Label Text="Status:" runat="server" ID="Label1" Visible="true"/>
                   </td>
                    <td valign="middle">
                        <asp:DropDownList ID="ddlstatus" runat="server" Style="margin-left: 5px" TabIndex="3" Width="151px" DataTextField="sactivetatus"  DataValueField="sactivetatus" AutoPostBack="true">                                             
                           
                      <asp:ListItem>--Select Status Type--</asp:ListItem>
                       <asp:ListItem>All</asp:ListItem>
                      <asp:ListItem>Active</asp:ListItem>
                      <asp:ListItem>InActive</asp:ListItem> 
                        </asp:DropDownList>
                        
                    </td>

                    <td>&nbsp;&nbsp;</td>

                   <td valign="middle" class="form_text" nowrap="nowrap" align="right">                        
                        <asp:Button ID="btnstatus" runat="server" Text="Preview" OnClick="btnstatus_Click" />  &nbsp;&nbsp; <asp:Button ID="btnexceltoexport" runat="server" Text="ExcelToExport" OnClick="btnexceltoexport_Click" />  
                       <asp:Button ID="btnallstatus" runat="server" Text="ExportToAll" OnClick="btnallstatus_Click"/> 
                    </td>

                </tr>
              
            </table>
        </div>


        <div style="margin: 20px;" align="left">
           
              <%--  <legend style="width: 144px;">
                    <div style="margin-left: 10px; font-size: medium; color: Black">
                       Employee Status
                    </div>
                </legend>
                <br />--%>

           

            <asp:GridView ID="gvid" runat="server" Width="100%" AutoGenerateColumns="false" Visible="false" ShowFooter="true" ForeColor="Black" Font-Bold="True" PageSize="20" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px" >
                <Columns> 
                    <asp:BoundField DataField="S No" HeaderText="SNo" ControlStyle-Width="30px"/>
                    <asp:BoundField DataField="EmployeeID" HeaderText="EmployeeID" ControlStyle-Width="30px"/>
                    <asp:BoundField DataField="EmployeeName" HeaderText="EmployeeName" ControlStyle-Width="30px"/>
                    <asp:BoundField DataField="RtolocationName" HeaderText="Location Name" ControlStyle-Width="30px"/>
                    <asp:BoundField DataField="Designation" HeaderText="Designation" ControlStyle-Width="30px"/>
                    <asp:BoundField DataField="MobileNo" HeaderText="MobileNo" ControlStyle-Width="30px"/>
                    <asp:BoundField DataField="JoiningDate" HeaderText="JoiningDate" ControlStyle-Width="30px"/>
                    <asp:BoundField DataField="Department" HeaderText="Department" ControlStyle-Width="30px"/>
                    <asp:BoundField DataField="CompanyName" HeaderText="CompanyName" ControlStyle-Width="30px"/>
                    <asp:BoundField DataField="Activestatus" HeaderText="Activestatus" ControlStyle-Width="30px"/> 
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
               

  

           </div>
              
                <div>
                   
                            <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                            <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        
                </div>
           
     </div>
</asp:Content>
