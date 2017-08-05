<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UserMapping.aspx.cs" Inherits="HSRP.Expenses.UserMapping" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <%--<script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>--%>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

<%--    <script type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
        }
    </script>--%>

    <div style="width: 1107px; height: 100px;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td valign="top">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" background="../images/midtablebg.jpg">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <span class="headingmain"> User Mapping...</span>
                                        </td>
                                        <td width="300px" height="26" align="center" nowrap></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <table width="100%">
            <tr>
                <td colspan="8">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="5" align="center">
                                <table cellpadding="15" cellspacing="0">

                                   
                                    <tr>
                                        <td>  <asp:Label Text="Employee Name:" runat="server"
                                                ID="labelDate" Font-Bold="True"
                                                ForeColor="Black" Width="100px" />
                                        </td>
                                        <td >
                                            
                                            <asp:DropDownList ID="DDlEmploeeName" runat="server" AutoPostBack="True" DataTextField="Emp_name" DataValueField="Emp_Id"
                                                Height="23px" Width="180px" >
                                            </asp:DropDownList>

                                        </td>  
                                        
                                        <td >
                                            <asp:Label Text="User Name:" runat="server" ID="labelOrganization1"
                                                ForeColor="Black" Font-Bold="True" Width="100px" />
                                        </td>
                                        <td >                                         
                                                
                                            <asp:DropDownList ID="DDlUserName" runat="server" AutoPostBack="True" DataTextField="Username" DataValueField="Userid"
                                                CausesValidation="false" Height="23px" Width="180px" >
                                            </asp:DropDownList>
                                          
                                        </td>                                       

                                    </tr>


                                    <tr>
                                           <td>  <asp:Label Text="Approval Head:" runat="server"
                                                ID="label2" Font-Bold="True"
                                                ForeColor="Black" Width="100px" />
                                        </td>
                                          <td >                    

                                                
                                            <asp:DropDownList ID="DDLApprovalHead" runat="server" AutoPostBack="True" DataTextField="Username" DataValueField="userid"
                                                CausesValidation="false" Height="23px" Width="180px">
                                            </asp:DropDownList>                                          
                                                  
                                        </td>  
                                        
                        </tr>
                                    <tr>
                                          <td colspan="4" align="right">                                          
                                          <asp:Button ID="btn" runat="server" Text="Save" Font-Bold="True" ForeColor="#3333FF" OnClick="btn_Click" />                                                                                   
                                        </td>   

                                    </tr>
                                   
                                   
                                </table>
                            </td>
                            
                        </tr>                    


                        
                        <tr>
                            <td colspan="10" align="center">
                                <asp:Label ID="Label1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
        </table>
    </div>
</asp:Content>
