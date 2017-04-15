<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BankSalaryReport.aspx.cs" Inherits="HSRP.Transaction.BankSalaryReport" %>

<%--<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>--%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />    
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />


    <div style="width:800px; height: 500px;">
       <%-- <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td valign="top">
                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" background="../images/midtablebg.jpg">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <span class="headingmain">Bank Report</span>
                                        </td>
                                        <td width="300px" height="26" align="left" nowrap></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>--%>

        <table width="100%">
            <tr>
                                   <td>
                                         <span class="headingmain">Bank Report</span>
                                        </td>
                                        <td width="300px" height="26" align="left" nowrap></td>
                                    </tr>
            <tr>
                <td colspan="8">
                    <table style="width: 90%">
                        <tr>
                            <td colspan="10" align="left">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>

                                          <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="State Name:"  runat="server" ID="lblStateName" ForeColor="Black" />
                                    </td>
                                   <td valign="middle" class="form_text" nowrap="nowrap">                                    
                                     
                                    <asp:DropDownList  ID="DDlState_Name"  AutoPostBack="true" runat="server" DataTextField="HSRPStateName" DataValueField="hsrp_stateid" OnSelectedIndexChanged="DDlState_Name_SelectedIndexChanged" >
                                    </asp:DropDownList>                                                                 

                                    </td>
                                        <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Company Name:"  runat="server" ID="lblCompanyName" ForeColor="Black" />
                                    </td>
                                   <td valign="middle" class="form_text" nowrap="nowrap">                                    
                                     
                                    <asp:DropDownList  ID="DDlCompany_Name"  runat="server" DataTextField="CompanyName" DataValueField="CompanyName" >
                                    </asp:DropDownList>&nbsp;&nbsp;                                                                   

                                    </td>
                                       
                                                              
                                                                               
                                        


                                    </tr>
                                       <td valign="middle" class="form_text" nowrap="nowrap"> 
                        <asp:Label Text="Year:"  runat="server" ID="lblyear" 
                            ForeColor="Black" />&nbsp;&nbsp;
                    </td>

                                       <td valign="middle" class="form_text" nowrap="nowrap">
                                                   
                                                      <asp:DropDownList ID="ddlyear"                                                    
                                                                runat="server" >
                                                          <asp:ListItem>--Select Year--</asp:ListItem>
                                                          <asp:ListItem>2016</asp:ListItem>
                                                          <asp:ListItem>2017</asp:ListItem>
                                                          <asp:ListItem>2018</asp:ListItem>
                                                          <asp:ListItem>2019</asp:ListItem>
                                                          <asp:ListItem>2020</asp:ListItem>
                                                          <asp:ListItem>2021</asp:ListItem>
                                                          <asp:ListItem>2022</asp:ListItem>
                                                          <asp:ListItem>2023</asp:ListItem>
                                                          <asp:ListItem>2024</asp:ListItem>
                                                          <asp:ListItem>2025</asp:ListItem>                                                         
                                                            </asp:DropDownList>
                                                         </td>       

                                      <td valign="middle" class="form_text" nowrap="nowrap">
                                           <asp:Label Text="Month Name :"  runat="server" ID="lblMonth" ForeColor="Black" />

                                      </td>

                                         <td valign="middle" class="form_text" nowrap="nowrap">
                                                   
                                                     <asp:DropDownList ID="DDLMonth" runat="server"   >
                                                         <asp:ListItem Text="--Select Month--" Value="--Select Month--" />
                                                         <asp:ListItem Text="January" Value="1" />
                                                         <asp:ListItem Text="February" Value="2" />
                                                          <asp:ListItem Text="March" Value="3" />
                                                          <asp:ListItem Text="April" Value="4" />
                                                          <asp:ListItem Text="May" Value="5" />
                                                          <asp:ListItem Text="June" Value="6" />
                                                          <asp:ListItem Text="July" Value="7" />
                                                          <asp:ListItem Text="August" Value="8" />
                                                          <asp:ListItem Text="September" Value="9" />
                                                          <asp:ListItem Text="October" Value="10" />
                                                          <asp:ListItem Text="November" Value="11" />
                                                          <asp:ListItem Text="December" Value="12" />                                                 

                                                       
                                                            </asp:DropDownList>&nbsp;&nbsp;
                                                         </td>


                                    <tr>
                                        <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Min Amount :"  runat="server" ID="Label2" ForeColor="Black" />

                                        </td>
                                        <td valign="middle" class="form_text" nowrap="nowrap">
                                            <asp:TextBox ID="txtMinAmount" runat="server"></asp:TextBox>

                                      

                                    </td>
                                        <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Max Amount :"  runat="server" ID="Label1" ForeColor="Black" />

                                        </td>
                                        <td valign="middle" class="form_text" nowrap="nowrap">
                                            <asp:TextBox ID="txtMaxAmount" runat="server"></asp:TextBox> 
                                            </td>

                                       
                                        </tr>

                                    <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                             <asp:Button ID="btn_download" runat="server" Text="Export Details For ICICI Bank A/C "  Font-Bold="True" ForeColor="#3333FF" OnClick="btn_download_Click" />
                                            &nbsp;&nbsp;&nbsp;
                                        </td>

                                          <td valign="middle" class="form_text" nowrap="nowrap">
                                             <asp:Button ID="Button1" runat="server" Text=" Export Details For Non- ICICI Bank A/C"      Font-Bold="True" ForeColor="#3333FF" OnClick="btn_detail_NonICIClick" Width="287px" />
                                        </td>
                                         <td valign="middle" class="form_text" nowrap="nowrap">
                                             <asp:Button ID="btncheque" runat="server" Text="Export Details For Cheque"      Font-Bold="True" ForeColor="#3333FF" OnClick="btncheque_Click"  />
                                        </td>
                                        <td valign="middle" class="form_text" nowrap="nowrap">
                                             <asp:Button ID="btnhold" runat="server" Text="Export Hold Details"      Font-Bold="True" ForeColor="#3333FF" OnClick="btnhold_Click"   />
                                        </td>
                                    </tr>





                                         <tr>
                                        

                                       

                                        
                                        
                                       <td valign="middle" class="form_text" nowrap="nowrap">
                                           &nbsp;&nbsp;
                                    </td>
                                      
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        &nbsp;&nbsp;

                                    </td>
                                       
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                            
                        </tr>
                        <tr>
                            <td width="80" colspan="4">&nbsp;</td>
                            <td colspan="3" align="left">
                                &nbsp;</td>
                            <td colspan="3" align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="10" align="left">
                                <asp:Label ID="lblerror" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        
                       
                       
                    </table>
                </td>
            </tr>
         
        </table>
    </div>
</asp:Content>


