<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EmployeeMasterDetail.aspx.cs" Inherits="HSRP.Transaction.EmployeeMasterDetail" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />    
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />


    <div style="width: 800px; height: 500px;">
           
        <table width="100%">
              <tr>
                <td>
               <span class="headingmain">Employee Report</span>
              </td>
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
                                    <tr>
                                         <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="District Name:"  runat="server" ID="lbldistrict" ForeColor="Black" />
                                    </td>
                                         
                                     <td valign="middle" class="form_text" nowrap="nowrap">                                      
                                       <asp:TextBox ID="txtdistinct" runat="server"></asp:TextBox>
                                                                                
                                    </td>                                       
                                                           
                                  <td valign="middle" class="form_text" nowrap="nowrap">
                                   <asp:Button ID="btn_download" runat="server" Text="Export Details"  Font-Bold="True" ForeColor="#3333FF" OnClick="btn_download_Click" />
                                    
                                  </td>
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
