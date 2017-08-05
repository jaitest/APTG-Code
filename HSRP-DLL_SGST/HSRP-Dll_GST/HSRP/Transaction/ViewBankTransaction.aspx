﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewBankTransaction.aspx.cs" Inherits="HSRP.Transaction.ViewBankTransaction" %>


<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript">


        function edit(i) { // Define This function of Send Assign Laser ID 
            //alert("AssignLaser" + i);
            //            var usertype = document.getElementById('username').value;
            //            alert(usertype);

            googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransaction.aspx?Mode=Edit&TransactionID=" + i, "Update BankTransaction.aspx", "width=700px,height=480px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
               window.location.href = "ViewBankTransaction.aspx";
                return true;
            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransaction.aspx?Mode=New", "Add New Bank Transaction", "width=700px,height=480px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
               window.location = 'ViewBankTransaction.aspx';
                return true;
            }
        }


        function PrintChalan(i, S) {
            //Define arbitrary function to run desired DHTML Window widget codes
            //            alert("Hello");
            googlewin = dhtmlwindow.open("googlebox", "iframe", "ViewPrintInvoice.aspx?Mode=PrintChalan&HSRPRecordID=" + i + "&Status=" + S + "", "Print DELIVERY CHALLAN / AP", "width=400px,height=75px,resize=1,scrolling=1,center=2", "recal")
            googlewin.onclose = function () {
                // window.location.href = "ViewSearch.aspx";
                return true;
            }
        }


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
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">View Bank Transaction</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="left" class="form_text" >
                                       
                                    </td>
                                    <td align="left" valign="middle">
                                        
                                    </td>
                                    <td valign="left" class="form_text">
                                        
                                    </td>
                                    <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Bank Transaction" class="button">Add New Bank Transaction</a>
                                    </td>

                                    

                                   <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                         
                                        <ComponentArt:Grid ID="Grid1" RTOLocationIDMode="AutoID" runat="server" ImagesBaseUrl="~/images"
                                            Width="100%" GroupingNotificationText="Drag a column to this area to group by it."
                                            LoadingPanelPosition="MiddleCenter" LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                            GroupBySortImageHeight="10" GroupBySortImageWidth="10" GroupBySortDescendingImageUrl="group_desc.gif"
                                            GroupBySortAscendingImageUrl="group_asc.gif" GroupingNotificationTextCssClass="GridHeaderText"
                                            IndentCellWidth="22" TreeLineImageHeight="19" TreeLineImageWidth="22" TreeLineImagesFolderUrl="~/images/lines/"
                                            PagerImagesFolderUrl="~/images/pager/" PreExpandOnGroup="true" GroupingPageSize="5"
                                            SliderPopupClientTemplateId="SliderTemplate" SliderPopupOffsetX="20" SliderGripWidth="9"
                                            SliderWidth="150" SliderHeight="20" PagerButtonHeight="22" PagerButtonWidth="41"
                                            PagerTextCssClass="GridFooterText" PageSize="25" GroupByTextCssClass="GroupByText"
                                            GroupByCssClass="GroupByCell" FooterCssClass="GridFooter" HeaderCssClass="GridHeader"
                                            SearchOnKeyPress="true" SearchTextCssClass="GridHeaderText" ShowSearchBox="true"
                                            ShowHeader="true" CssClass="Grid" RunningMode="Callback" SearchBoxPosition="TopLeft"
                                            GroupingNotificationPosition="TopRight" FillContainer="true" Height="300px">
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="TransactionID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                                         <ComponentArt:GridColumn DataField="TransactionID" Visible="False" />
                                                        <ComponentArt:GridColumn DataField="Deposit Date" HeadingText="Deposit Date" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                             <ComponentArt:GridColumn DataField="BankSlipNo" HeadingText="Bank Slip No"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                       
                                                        <ComponentArt:GridColumn DataField="BankName" HeadingText="Bank Name"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                        <ComponentArt:GridColumn DataField="BranchName" Visible="true" HeadingText="Branch Name"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="DepositAmount" HeadingText="Deposit Amount" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="DepositBy" HeadingText="Deposit By" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                            <ComponentArt:GridColumn DataField="DepositLocation" HeadingText="Deposit Location" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                            <ComponentArt:GridColumn DataCellClientTemplateId="DeliveryChallan" HeadingText="Status"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />

                                                       <ComponentArt:GridColumn DataField="StateID" HeadingText="State ID" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />


                                                            <ComponentArt:GridColumn DataField="RTOLocation" HeadingText="RTOLocation" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                        <ComponentArt:GridColumn DataField="UserID" HeadingText="User ID" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />
                                                            <ComponentArt:GridColumn DataField="CurrentDate" HeadingText="Current Date" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />
                                                        <%--<ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="edit"
                                                            HeadingText="Approved/Status" SortedDataCellCssClass="SortedDataCell" Width="50" />--%>
                                                        

                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>

                                            
                                          <%--  <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="Invoice">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonInvoice" 
                                                            runat="server" Text="Invoice" CommandName="Invoice"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates> --%>


                                      <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="Invoice">
                                                    <Template>
                                            <asp:LinkButton runat="server">LinkButton</asp:LinkButton>
                                                   </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates> 
                                            <ClientTemplates>
                                               

                                               <%-- <ComponentArt:ClientTemplate ID="edit">
                                                    <a style="color: Red" onclick="javascript:edit(## DataItem.GetMember('TransactionID').Value ##);">
                                                        Approval</a></ComponentArt:ClientTemplate>--%>


                                                <ComponentArt:ClientTemplate ID="LoadingFeedbackTemplate" runat="server">
                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td style="font-size: 10px;">
                                                                Loading...&nbsp;
                                                            </td>
                                                            <td>
                                                                <img alt="loading" src="/Images/spinner.gif" width="16" height="16" border="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="SliderTemplate" runat="server">
                                                    <table class="SliderPopup" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td valign="top" style="padding: 5px;">
                                                                <table width="741" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td width="25" align="center" valign="top" style="padding-top: 3px;">
                                                                        </td>
                                                                        <td>
                                                                            <table cellspacing="0" cellpadding="2" border="0" style="width: 255px;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="overflow: hidden; width: 115px;">
                                                                                            <b>Code :</b><nobr>## DataItem.GetMember(&#39;OrgID&#39;).Value ##</nobr></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="overflow: hidden; width: 135px;">
                                                                                            <b>Name :</b><nobr>## DataItem.GetMember(&#39;OrgName&#39;).Value ##</nobr></div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="center">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table width="741" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            Page <b>## DataItem.PageIndex + 1 ##</b> of <b>## Grid1.PageCount ##</b>
                                                                        </td>
                                                                        <td align="right">
                                                                            Record <b>## DataItem.Index + 1 ##</b> of <b>## Grid1.RecordCount ##</b>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ComponentArt:ClientTemplate>
                                            </ClientTemplates>
                                        </ComponentArt:Grid>
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
