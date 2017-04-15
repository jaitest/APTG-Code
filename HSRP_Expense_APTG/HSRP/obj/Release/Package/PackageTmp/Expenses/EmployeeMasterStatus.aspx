<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EmployeeMasterStatus.aspx.cs" Inherits="HSRP.Expenses.EmployeeMasterStatus" %>
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

                    <td class="form_text" nowrap="nowrap">
                    <asp:Label Text="Location Name:" runat="server" ID="lbllocation" Visible="true"/>
                   </td>
                    <td valign="middle">
                        <asp:DropDownList ID="ddllocation" runat="server" Style="margin-left: 5px" TabIndex="3" Width="171px" DataTextField="RTOLocationName"  DataValueField="RTOLocationID" AutoPostBack="true" OnSelectedIndexChanged="ddllocation_SelectedIndexChanged">                                             
                        </asp:DropDownList>
                        
                    </td>

                    <%--  <td class="form_text" nowrap="nowrap">
                    <asp:Label Text="Status:" runat="server" ID="Label1" Visible="true"/>
                   </td>
                   <td valign="middle">
                        <asp:DropDownList ID="ddlstatus" runat="server" Style="margin-left: 5px" TabIndex="3" Width="151px" DataTextField="sactivetatus"  DataValueField="sactivetatus" AutoPostBack="true">                                             
                           
                      <asp:ListItem>--Select Status Type--</asp:ListItem>
                      <asp:ListItem>Active</asp:ListItem>
                      <asp:ListItem>InActive</asp:ListItem> 
                        </asp:DropDownList>
                        
                    </td>--%>

                    <td>&nbsp;&nbsp;</td>

                   <td valign="middle" class="form_text" nowrap="nowrap" align="right">
                        
<%--                        <asp:Button ID="btnstatus" runat="server" Text="EmpStatus" OnClick="btnstatus_Click" />    
                       <br /> <br />--%>
                        <asp:Button ID="btngo" runat="server" Text="GO" OnClick="btngo_Click" />                     

                    </td>

                  

                </tr>
            </table>
        </div>
        <div>
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                       <tr>                
                    <td  class="form_text" nowrap="nowrap" >
                    <asp:Label Text="Emp ID:" runat="server" ID="lblempid" Visible="false"/>
                        </td>
                        <td valign="middle">
                    <asp:TextBox ID="txtempid" runat="server" Visible="false"></asp:TextBox>&nbsp; 
                            </td>

                            <td  class="form_text" nowrap="nowrap">
                                <asp:Label Text="Emp Name:" runat="server" ID="lblempname" Visible="false"/>
                            </td>

                  
                           <td valign="middle">
                    <asp:TextBox ID="txtempname" runat="server" Visible="false"></asp:TextBox>
                               </td>
                                         
                   
                                        
                
                <td width="300px" height="26" align="center" nowrap>
                    <asp:Button ID="BtnSearch" runat="server" Text="Search" OnClick="BtnSearch_Click" Visible="false"/></td>
            </tr>

            </table>

        </div>

      


        <div style="margin: 20px;" align="left">
          

               

 <asp:GridView ID="gvEmpDesignation" runat="server" Width="100%" AutoGenerateColumns="false" ShowFooter="true" ForeColor="Black" Font-Bold="True" AllowPaging="true" PageSize="2" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px" onrowupdating="gvEmpDesignation_RowUpdating" onrowcancelingedit="gvEmpDesignation_RowCancelingEdit"  onrowediting="gvEmpDesignation_RowEditing" OnPageIndexChanging="gvEmpDesignation_PageIndexChanging">
                <Columns>      
                    
                    <asp:TemplateField HeaderText="Hsrp StateID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblstateid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Hsrp_stateid") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:Label ID="lblEditstateid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Hsrp_stateid") %>'></asp:Label>           
                        </EditItemTemplate>
                      
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Location ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lbllocationid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "RtoLocationID") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:Label ID="lblEditlocationid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "RtoLocationID") %>'></asp:Label>           
                        </EditItemTemplate>
                      
                    </asp:TemplateField>
                         
                    <asp:TemplateField HeaderText="Employee ID" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EmployeeID") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:Label ID="lblEditid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EmployeeID") %>'></asp:Label>           
                        </EditItemTemplate>
                      
                    </asp:TemplateField>
 
                    <asp:TemplateField HeaderText="Employee Name">
                        <ItemTemplate>
                            <asp:Label ID="lblempName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EmployeeName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:TextBox ID="txtEditempName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EmployeeName") %>'></asp:TextBox>           
                        </EditItemTemplate>
                       
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Designation">
                        <ItemTemplate>
                            <asp:Label ID="lbldesignation" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Designation") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:TextBox ID="txtEditdesignation" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Designation") %>'></asp:TextBox>           
                        </EditItemTemplate>
                       
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Mobile No">
                        <ItemTemplate>
                            <asp:Label ID="lblmobno" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "MobileNo") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:TextBox ID="txtEditmobno" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "MobileNo") %>'></asp:TextBox>           
                        </EditItemTemplate>
                       
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Emp Joining Date">
                        <ItemTemplate>
                            <asp:Label ID="lblempdate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "JoiningDate") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:TextBox ID="txtEditempdate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "JoiningDate") %>'></asp:TextBox>           
                        </EditItemTemplate>
                       
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Department">
                        <ItemTemplate>
                            <asp:Label ID="lbldepartment" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Department") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:TextBox ID="txtEditdepartment" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Department") %>'></asp:TextBox>           
                        </EditItemTemplate>
                       
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CompanyName">
                        <ItemTemplate>
                            <asp:Label ID="lblcompanyname" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CompanyName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:TextBox ID="txtEditcompanyname" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CompanyName") %>'></asp:TextBox>           
                        </EditItemTemplate>
                       
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Active Status">
                        <ItemTemplate>
                            <asp:Label ID="lblactive" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Activestatus") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:TextBox ID="txtEditactive" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Activestatus") %>'></asp:TextBox>           
                        </EditItemTemplate>
                       
                    </asp:TemplateField>

                   <asp:CommandField ShowEditButton="true" />
                   <%-- <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                       
                            <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit"  Text="Edit"  />
                           
                           
                        </ItemTemplate>
                        <EditItemTemplate>
                         
                             <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update"  Text="Update"  />
                              <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel"  Text="Cancel" />
                        </EditItemTemplate>
                       
                    </asp:TemplateField>  --%>
                                      
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
