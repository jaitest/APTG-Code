<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewBankTransactionUpdate.aspx.cs" Inherits="HSRP.Transaction.ViewBankTransactionUpdate" %>


<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />


    <script type="text/javascript">
        function openPopup(Transactionid) {
            window.open('BankTransactionUpdate.aspx?TransactionID=' + Transactionid, "Popup", "width=500,height=500");
        }
     </script>

    <script type="text/javascript">


        function edit(i) { 

            googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransactionUpdate.aspx?Mode=Edit&TransactionID=" + i, "Update BankTransactionUpdate.aspx", "width=700px,height=620px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = "ViewBankTransactionUpdate.aspx";
                return true;
            }
        }


        function Voids(i) { 
                 
            googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransactionVoid.aspx?Mode=Voids&TransactionID=" + i, "Update BankTransactionVoid.aspx", "width=450px,height=220px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = "ViewBankTransactionUpdate.aspx";
                return true;
            }
        }


        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransactionUpdate.aspx?Mode=New", "Add New Bank Transaction", "width=700px,height=480px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
               window.location = 'ViewBankTransactionUpdate.aspx';
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

         <td>

              <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0"
                                class="topheader">
                                <tr>
                                 
                                    <td>
                                     
                                        <asp:DropDownList Visible="false" ID="ddlBothDealerHHT"  CausesValidation="false" Width="140px"
                                         DataTextField="dealername" DataValueField="dealerid"  
                                            AutoPostBack="false" runat="server">  
                                        </asp:DropDownList>


                                        

                                    </td>
                               
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnGO" Width="58px" runat="server" Visible="false" Text="GO" ToolTip="Please Click for Report"
                                            class="button" OnClick="btnGO_Click1"  />                                        
                                        </td>                                  

                                </tr>
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
                                        <span class="headingmain">View Bank Transaction Update</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="left" class="form_text" >
                                       
                                    </td>
                                    <td align="left" valign="middle">
                                        
                                    </td>
                                    <td valign="left" class="form_text">
                                        
                                    </td>
                             
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
                              <%--  <tr>
                                   
                                    <td>
                                       <asp:Panel runat="server" ID="Panel1" ScrollBars="Horizontal" Height="550px" Width="1500px"  Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px">
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
                                                    SortImageHeight="19" >
                                                    <Columns>
                                                         <ComponentArt:GridColumn DataField="TransactionID" Visible="true" HeadingText="TransactionID"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100"/>
                                                        <ComponentArt:GridColumn DataField="Deposit Date" HeadingText="Deposit Date" SortedDataCellCssClass="SortedDataCell"
                                                            Width="120" />
                                                             <ComponentArt:GridColumn DataField="BankSlipNo" HeadingText="Bank Slip No"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                       
                                                        <ComponentArt:GridColumn DataField="BankName" HeadingText="Bank Name"
                                                            SortedDataCellCssClass="SortedDataCell" Width="160" />
                                                        <ComponentArt:GridColumn DataField="BranchName" Visible="true" HeadingText="Branch Name"
                                                            Width="160" />
                                                        <ComponentArt:GridColumn DataField="DepositAmount" HeadingText="Deposit Amount" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="DepositBy" HeadingText="Deposit By" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="150" />
                                                            <ComponentArt:GridColumn DataField="DepositLocation" HeadingText="Deposit Location" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="125" />
                                                            <ComponentArt:GridColumn DataCellClientTemplateId="DeliveryChallan" HeadingText="Status"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />

                                                       <ComponentArt:GridColumn DataField="StateID" HeadingText="State ID" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />


                                                            <ComponentArt:GridColumn DataField="RTOLocation" HeadingText="RTOLocation" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />
                                                        <ComponentArt:GridColumn DataField="UserID" HeadingText="User ID" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />
                                                            <ComponentArt:GridColumn DataField="CurrentDate" HeadingText="Current Date" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="150" />

                                                          <ComponentArt:GridColumn DataField="ETAProcess" HeadingText="ETA Process" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                        <ComponentArt:GridColumn DataField="EntryType" HeadingText="Entry Type" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                          <ComponentArt:GridColumn DataField="oldTransactionID" HeadingText="Old TransactionID" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                          <ComponentArt:GridColumn DataField="EntryDate" HeadingText="Entry Date" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="150" />
                                                          <ComponentArt:GridColumn DataField="DealerID" HeadingText="DealerID" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />

                                                         <ComponentArt:GridColumn DataField="chq_no" HeadingText="Cheque No" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />
                                                      
                                                         <ComponentArt:GridColumn DataField="ApprovedStatus" HeadingText="ApprovedStatus" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />

                                                        <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="edit"
                                                            HeadingText="Approved/Status" SortedDataCellCssClass="SortedDataCell" Width="120" />

                                                         <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="Voids"
                                                            HeadingText="Void/Status" SortedDataCellCssClass="SortedDataCell1" Width="120" />
                                                        

                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>    

                                      <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="Invoice">
                                                    <Template>
                                            <asp:LinkButton runat="server">LinkButton</asp:LinkButton>
                                                   </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates> 
                                            <ClientTemplates>
                                               
                                             
                                                <ComponentArt:ClientTemplate ID="edit">
                                                    <a style="color: Red" onclick="javascript:edit(## DataItem.GetMember('TransactionID').Value ##);">
                                                       Edit</a></ComponentArt:ClientTemplate>


                                                 <ComponentArt:ClientTemplate ID="Voids">
                                                    <a style="color: Red" onclick="javascript:Voids(## DataItem.GetMember('TransactionID').Value ##);">
                                                      
                                                       Void</a></ComponentArt:ClientTemplate>


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
                                                                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td width="25" align="center" valign="top" style="padding-top: 3px;">
                                                                        </td>
                                                                        <td>
                                                                            <table cellspacing="0" cellpadding="2" border="0" style="width:100%;">
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
                                                                <table width="100%" cellspacing="0" cellpadding="0" border="0">
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
                                            </asp:Panel>
                                    </td>
                                   
                                </tr>--%>

                                <tr>

                                    <td>
                                          <asp:GridView ID="Grid1" runat="server" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="false"
                            ShowHeader="true" Width="100%" BackColor="White" BorderColor="#FFCC99" BorderStyle="Solid" BorderWidth="1px">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>  
                                       <%-- <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="HiddenField1" ClientIDMode="Static" runat="server" Value='<%# Eval("TransactionID") %>' />
                                             </ItemTemplate>
                                          </asp:TemplateField>--%>
                             
                                  <asp:BoundField HeaderText="TransactionID" DataField="TransactionID" />
                                  <asp:BoundField HeaderText="Deposit Date" DataField="Deposit Date" />
                                  <asp:BoundField HeaderText="BankName" DataField="BankName" />
                                  <asp:BoundField HeaderText="BranchName" DataField="BranchName" />
                                  <asp:BoundField HeaderText="DepositAmount" DataField="DepositAmount" />
                                  <asp:BoundField HeaderText="DepositBy" DataField="DepositBy" />
                                  <asp:BoundField HeaderText="StateID" DataField="StateID" />
                                  <asp:BoundField HeaderText="RTOLocation" DataField="RTOLocation" />
                                  <asp:BoundField HeaderText="UserID" DataField="UserID" />
                                  <asp:BoundField HeaderText="CurrentDate" DataField="CurrentDate" />
                                  <asp:BoundField HeaderText="BankSlipNo" DataField="BankSlipNo" />
                                  <asp:BoundField HeaderText="Remarks" DataField="Remarks"/>
                                  <asp:BoundField HeaderText="AccountNo" DataField="AccountNo" />
                                 <%-- <asp:BoundField HeaderText="ETAProcess" DataField="ETAProcess" />
                                                    <asp:BoundField HeaderText="depositelocationid" DataField="depositelocationid" />
                                                    <asp:BoundField HeaderText="EntryType" DataField="EntryType" />
                                                     <asp:BoundField HeaderText="oldTransactionID" DataField="oldTransactionID" />
                                                
                                <asp:BoundField HeaderText="EntryDate" DataField="EntryDate" />
                                <asp:BoundField HeaderText="DealerID" DataField="DealerID" />
                                <asp:BoundField HeaderText="chq_no" DataField="chq_no" />
                                <asp:BoundField HeaderText="chq_date" DataField="chq_date" />
                                <asp:BoundField HeaderText="ApprovedStatus" DataField="ApprovedStatus" />
                                <asp:BoundField HeaderText="VoidStatus" DataField="VoidStatus" />--%>

                                
                                 <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                  <asp:Button ID="Edit" runat="server" Text="Edit" PostBackUrl='<%# Eval("TransactionID", "~/BankTransactionUpdate.aspx?TransactionID={0}") %>' />
                                </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField>
                                <ItemTemplate>
                                <a href="#" class="gridViewToolTip" onclick='openPopup("<%# Eval("TransactionID")%>")'>Edit</a>
                                </ItemTemplate>
                                </asp:TemplateField>


                                  <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lnkEdit" ClientIDMode="Static" CommandArgument='<%#Eval("TransactionID") %>'
                                                            CommandName="Edit" ValidationGroup="link">Edit</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Void">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lnkVoid" ClientIDMode="Static" CommandArgument='<%#Eval("TransactionID") %>'
                                                            CommandName="Void">Void</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                             <%--   <asp:TemplateField HeaderText="Rejected">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lnkRejected" ClientIDMode="Static" CommandArgument='<%#Eval("TransactionID") %>' CommandName="Rejected" ValidationGroup="Reject">Rejected</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>                                             
                                            <asp:LinkButton runat="server" ID="edit" OnClientClick="edit(239693)" CommandName="Edit">ViewEdit</asp:LinkButton>
                                             </ItemTemplate>
                                          </asp:TemplateField>--%>

                              <%--   <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <a href="javascript:openPopup('Info.aspx?id=<%# Eval("ID") %>')">
                                           <img src="pics/info.gif" border=0px width=13px/></a>
                                    </ItemTemplate>
                                    </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="Voids" OnClientClick="javascript:Voids(## DataItem.GetMember('TransactionID').Value ##);" CommandName="Void">Void</asp:LinkButton>
                                             </ItemTemplate>
                                          </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                    <a href ='<%#"BankTransactionUpdate.aspx?TransactionID="+DataBinder.Eval(Container.DataItem,"TransactionID") %>'> <%#Eval("TransactionID") %>  </a>
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
