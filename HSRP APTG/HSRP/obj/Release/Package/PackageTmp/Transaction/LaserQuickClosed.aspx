﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="LaserQuickClosed.aspx.cs" Inherits="HSRP.Master.LaserQuickClosed" %>

    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
       <script src="../Scripts/User.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    
    
     

   <script type="text/javascript">

       function removeSpaces(string) {
           return string.split(' ').join('');
       }

       function remove(string) {
           return trims(string);
       }



       function trims(s) {
           return rtrim(ltrim(s));
       }

       function ltrim(s) {
           var l = 0;
           while (l < s.length && s[l] == ' ')
           { l++; }
           return s.substring(l, s.length);
       }

       function rtrim(s) {
           var r = s.length - 1;
           while (r > 0 && s[r] == ' ')
           { r -= 1; }
           return s.substring(0, r + 1);
       }


        

        
    </script>
    
    <style>
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .9em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth
        {
            width: 150px !important;
        }
        #divwidth div
        {
            width: 150px !important;
        }
    
select
{
  font-family: Verdana; 
  font-size:11px;
} 
        .style4
        {
            width: 188px;
        }
    </style>


    <script language="javascript" type="text/javascript">

     
        function validate() {

            if (document.getElementById("<%=textBoxVehicleRegNo.ClientID%>").value == "") {
                alert("Please Enter Vehicle Registration No!!");
                document.getElementById("<%=textBoxVehicleRegNo.ClientID%>").focus();
                return false;
            }
        }
     

        function ValidationQuick() {

             if (document.getElementById("<%=txtClosedDate.ClientID%>").value == "") {
                 alert("Please Select Closed Date!!");
                document.getElementById("<%=txtClosedDate.ClientID%>").focus();
                return false;
            }
            
        }
     
     </script>



    <%--</ContentTemplate>
                             </asp:UpdatePanel>--%>
    <div align="left" style="margin-left: 10px; font-size: medium; color: Black">
                                <span>Quick Embossing / Close</span>
                            </div>
     <div id="DivBorder" runat="server" class="assignQuick" >
    <table style="background-color:White" width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
    <asp:UpdatePanel ID="updatepannel1" runat="server">
    <ContentTemplate>
                <tr>
                        <td colspan="5"> &nbsp;</td> 
                </tr>
                <tr>
                        <td class="style4"  style="padding-bottom: 20px">
                           <font color="black"> Vehicle Registration No. : </font>
                        </td>
                        <td>
                            <asp:TextBox ID="textBoxVehicleRegNo" runat="server" class="form_textbox11" onlosefocus=" return sameLaserNo();" TabIndex="1" Width="150"></asp:TextBox> &nbsp;&nbsp;
                            <asp:Button ID="btnGO" runat="server" class="button" onclick="btnGO_Click" OnClientClick=" return validate()" TabIndex="4" Text="GO" />
                        </td>
                        <td>
                           <td>
                                                                <div id="div1">
                                                                </div>
                                                                 
                                                               <asp:AutoCompleteExtender ServiceMethod="VehicleRegNoQuick" MinimumPrefixLength="1" ServicePath="~/WCFService/ServiceForSuggestion.svc"
                                                                    CompletionInterval="10" EnableCaching="false" TargetControlID="textBoxVehicleRegNo" UseContextKey = "true"
                                                                    ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionSetCount="12"
                                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" 
                                                                    CompletionListElementID="div1">
                                                                </asp:AutoCompleteExtender>
                                                             &nbsp;&nbsp;    
                                                                                      
                    <td>
                        <asp:Label ID="lblMessegforAssignNewLaser" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                                                                </td>
                          
                        </td>   
                    </tr>
         
                    <tr id="DivFirst" runat="server" visible="false">
                       
                        <td class="style4" style="padding-bottom: 10px" align="center"> <%--<font color="black">Front Plate Laser No :--%> 
                                <asp:Label ID="lbltestfront" runat="server" Text="Front Plate Laser No :"></asp:Label>
                        </td>
                        <td align="left"><font color="black"><asp:Label ID="lblFrontLaserPlate" CssClass="" runat="server"></asp:Label></font></td>
                          
                   
                        <td class="style4" style="padding-bottom: 10px" align="center"> 
                            <%--<font color="black">Rear Plate Laser No : --%>
                            <asp:Label ID="lbltestrear" runat="server" Text="Rear Plate Laser No :"></asp:Label>
                              </td>
                         <td align="left"><font color="black"><asp:Label ID="lblRearLaserPlate" CssClass="" runat="server"></asp:Label></font></td>
                           
                    </tr>
                <tr  id="DivSecond" runat="server" visible="false">
                        <td class="form_text_View" style="padding-bottom: 10px"> <font color="black">Front Plate Laser No : <span class="alert">* </span></font> 
                         
                        </td> 
                        <td>  
                                                               
                                                               <asp:TextBox class="form_textbox11" TabIndex="1" Width="150" ID="textBoxFrontPlate" EnableViewState="true" 
                                                                    runat="server" ></asp:TextBox>
                                                                     
                                                                <div id="divwidth">
                                                                </div>
                                                                 
                                                               <asp:AutoCompleteExtender ServiceMethod="LaserNoQuick" MinimumPrefixLength="1" ServicePath="~/WCFService/ServiceForSuggestion.svc"
                                                                    CompletionInterval="10" EnableCaching="false" TargetControlID="textBoxFrontPlate" UseContextKey = "true"
                                                                    ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionSetCount="12"
                                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" 
                                                                    CompletionListElementID="divwidth">
                                                                </asp:AutoCompleteExtender>
                                                            </td>  
                   <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px"> <font color="black">Rear Plate Laser No : <span class="alert">* </span></font>  </td> 
                        <td>
                                                                <asp:TextBox class="form_textbox11" TabIndex="1" Width="150" ID="textBoxRear" onlosefocus=" return sameLaserNo();"
                                                                    runat="server" ></asp:TextBox>
                                                                <div id="divRear">
                                                                </div>
                                                                 
                                                               <asp:AutoCompleteExtender ServiceMethod="LaserNoQuick" MinimumPrefixLength="1" ServicePath="~/WCFService/ServiceForSuggestion.svc"
                                                                    CompletionInterval="10" EnableCaching="false" TargetControlID="textBoxRear" UseContextKey = "true"
                                                                    ID="AutoCompleteExtender3" runat="server" FirstRowSelected="false" CompletionSetCount="12"
                                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" 
                                                                    CompletionListElementID="divRear">
                                                                </asp:AutoCompleteExtender>
                                                            </td>   
                    </tr>
                    <tr>
                   
                          </td>
                        <td class="style4"></td>

                        <td  > </td> 
                        
                          
                         
                    </tr>
                      

</ContentTemplate>
                             </asp:UpdatePanel>
                    <tr>
                        <td class="style4"> &nbsp;</td>
                        <td align="left">

                            
                        <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblErrMessvehicle" runat="server" Font-Size="12px" 
                        ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblErrMessvehicleFront" runat="server" Font-Size="12px" 
                        ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>

                            
                        </td>  
                        <td align="right" width="150" nowrap="nowrap" > 
                        <asp:Label ID="lblClosedDate" runat="server" CssClass="Label_user" 
                        Text="Order Close Date: " Visible="False"></asp:Label>
                        </td> 
                        <td align="right" > 
                        <asp:TextBox ID="txtClosedDate" runat="server" Height="24px" Visible="False" Width="132px"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtClosedDate" runat="server" ValidationGroup="Close" Visible="false" ErrorMessage="Please Enter Close Date"></asp:RequiredFieldValidator>
                            
                        <asp:CalendarExtender ID="txtClosedDate_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="txtClosedDate">
                        </asp:CalendarExtender>&nbsp;
                        <%-- <asp:RangeValidator ID="rangeDate" runat="server" ControlToValidate="txtClosedDate" 
                        ErrorMessage="Please Select Date Less Than Today"></asp:RangeValidator>--%>
                        </td> 
                        <td align="right" > 
                        <asp:Button ID="btnAssignLaser" runat="server" Visible="false"  class="button"  
                        Text="Embossing" TabIndex="4"   
                        onclick="btnAssignLaser_Click" />&nbsp;&nbsp;
                        <asp:Button ID="btnDownloadInvoice" runat="server"  class="button"  Visible="false"
                        Text="Download Invoice" TabIndex="4" 
                        OnClientClick=" return ValidationQuick()" 
                        onclick="btnDownloadInvoice_Click"   />&nbsp;&nbsp;
                        <asp:Button ID="btnSave" runat="server"   class="button"  Text="Order Closed" ValidationGroup="Close" TabIndex="4" OnClientClick="return ValidationQuick()"   
                        onclick="btnSave_Click" />&nbsp;&nbsp; 
                   
                        <%--<input type="reset" class="button" value="Reset" />--%>
                        </td> 
                    </tr>
                    <asp:HiddenField ID="hiddenfieldvaluetype" runat="server" />
                    <asp:HiddenField ID="hiddenfieldStateID" runat="server" />
                    <asp:HiddenField ID="hiddenfieldRTOLocationID" runat="server" />
                    <asp:HiddenField ID="hiddenfieldFrontFrontCode" runat="server" />
                    <asp:HiddenField ID="hiddenfieldRearCode" runat="server" />
                    <asp:HiddenField ID="hiddenfieldHSRPRecordID" runat="server" />
                    <asp:HiddenField ID="HiddenfieldHsrpOrderStatusNewOrder" runat="server" />
                </table>
     </div>
                 <%--</ContentTemplate>
                             </asp:UpdatePanel>--%>
    <br />
</asp:Content>
