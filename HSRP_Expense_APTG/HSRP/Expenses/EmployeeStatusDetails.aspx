<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EmployeeStatusDetails.aspx.cs" Inherits="HSRP.Expenses.EmployeeStatusDetails" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />


    <script type="text/javascript">



        function getQueryStrings() {
            var assoc = {};
            var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
            var queryString = location.search.substring(1);
            var keyValues = queryString.split('&');

            for (var i in keyValues) {
                var key = keyValues[i].split('=');
                if (key.length > 1) {
                    assoc[decode(key[0])] = decode(key[1]);
                }
            }

            return assoc;
        }



        function edit(i)
        {
           
           
            googlewin = dhtmlwindow.open("googlebox", "iframe", "UpdateEmpDetails.aspx?Mode=Edit&Emp_id=" + i, "Update Bank Emp_id", "width=650px,height=520px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = 'EmployeeStatusDetails.aspx';
                //var qs = getQueryStrings();
                //var myParam = qs["PageIndex"];                
                    //?PageIndex=' + myParam;

                return true;

            }
        }

      
        //function Voids(i) {

        //    googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransactionVoid.aspx?Mode=Voids&TransactionID=" + i, "  Void Bank Transaction  ", "width=450px,height=220px,resize=1,scrolling=1,center=1", "recal")
        //    googlewin.onclose = function () {

        //        //var qs = getQueryStrings();
        //      //  var myParam = qs["PageIndex"];
        //        window.location.href = 'ViewBankTransactionUpdate.aspx';
        //        //?PageIndex=' + myParam;

        //        return true;

        //    }
        //}


        function AddNewPop() {
                       
            googlewin = dhtmlwindow.open("googlebox", "iframe", "UpdateEmpDetails.aspx?Mode=New", "Add New Bank Emp_id", "width=700px,height=480px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location = 'EmployeeStatusDetails.aspx';
                return true;
            }
        }


        //function PrintChalan(i, S) {

        //    googlewin = dhtmlwindow.open("googlebox", "iframe", "ViewPrintInvoice.aspx?Mode=PrintChalan&HSRPRecordID=" + i + "&Status=" + S + "", "Print DELIVERY CHALLAN / AP", "width=400px,height=75px,resize=1,scrolling=1,center=2", "recal")
        //    googlewin.onclose = function () {
        //         return true;
        //    }
        //}


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


    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>

            <td>

                <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0"
                    class="topheader">
                </table>
            </td>

        </tr>
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">Employee Status Details</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap></td>
                                </tr>



                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                <tr>                                
                    <td  class="form_text" nowrap="nowrap">
                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                    </td>
                    <td valign="middle">
                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                            Width="102px"  >
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
                        <asp:DropDownList ID="ddllocation" runat="server" Style="margin-left: 5px" TabIndex="3" Width="171px" DataTextField="RTOLocationName"  DataValueField="RTOLocationID" AutoPostBack="true" >                                             
                        </asp:DropDownList>
                        
                    </td>

                   

                    <td>&nbsp;&nbsp;</td>

                   <td valign="middle" class="form_text" nowrap="nowrap" align="right">
                        
<%--   <asp:Button ID="btnstatus" runat="server" Text="EmpStatus" OnClick="btnstatus_Click" />    
                       <br /> <br />--%>
                        <asp:Button ID="btngo" runat="server" Text="GO" OnClick="btngo_Click" />                     

                    </td>

                  

                </tr>
            </table>

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
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="left" class="form_text"></td>
                                    <td align="left" valign="middle"></td>
                                    <td valign="left" class="form_text"></td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>


                                <tr>

                                    <td>
                                        <asp:GridView ID="Grid1" runat="server" OnRowDataBound="Grid1_RowDataBound" AllowPaging="true" PageSize="15" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                                            AutoGenerateColumns="false" OnPageIndexChanging="Grid1_PageIndexChanging" ShowHeader="true" Width="100%" BackColor="White" BorderColor="#FFCC99" BorderStyle="Solid"
                                            BorderWidth="1px">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                  <%--<asp:BoundField DataField="S No" HeaderText="SNo" ControlStyle-Width="30px"/>--%>
                                                  <asp:BoundField DataField="Emp_id" HeaderText="EmployeeID" ControlStyle-Width="30px"/>
                                                  <asp:BoundField DataField="Emp_Name" HeaderText="EmployeeName" ControlStyle-Width="30px"/>
                                                  <asp:BoundField DataField="Email" HeaderText="Location Name" ControlStyle-Width="30px"/>
                                                  <asp:BoundField DataField="Designation" HeaderText="Designation" ControlStyle-Width="30px"/>
                                                  <asp:BoundField DataField="MobileNo" HeaderText="MobileNo" ControlStyle-Width="30px"/>
                                                <asp:BoundField DataField="Department" HeaderText="Department" ControlStyle-Width="30px"/>
                                                <asp:BoundField DataField="EntryDate" HeaderText="JoiningDate" ControlStyle-Width="30px"/>
                                                <asp:BoundField DataField="Activestatus" HeaderText="Activestatus" ControlStyle-Width="30px"/> 
                                                  <%--<asp:BoundField DataField="UserId" HeaderText="UserId" ControlStyle-Width="30px"/>
                                                <asp:BoundField DataField="Role" HeaderText="Role" ControlStyle-Width="30px"/>
                                                <asp:BoundField DataField="hsrp_stateid" HeaderText="hsrp_stateid" ControlStyle-Width="30px"/>
                                                <asp:BoundField DataField="RtoLocationid" HeaderText="RtoLocationid" ControlStyle-Width="30px"/>--%>
                                                <asp:BoundField DataField="EmpJoiningDate" HeaderText="EmpJoiningDate" ControlStyle-Width="30px"/>
                                                  

                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>

                                                        <a href="javascript:edit(<%#Eval("Emp_id")%>)"> <asp:Label runat="server" ID="lbledit" Text="Edit"></asp:Label></a>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                               <%-- <asp:TemplateField HeaderText="Void">
                                                    <ItemTemplate>
                                                    <a href="javascript:Voids(<%#Eval("TransactionID")%>);"> <asp:Label runat="server" ID="lblvoid" Text='<%#Bind("voidstatus")%>'></asp:Label></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

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


                            </table>
                        </td>

                    </tr>
                </table>
            </td>
        </tr>
    </table>


</asp:Content>
