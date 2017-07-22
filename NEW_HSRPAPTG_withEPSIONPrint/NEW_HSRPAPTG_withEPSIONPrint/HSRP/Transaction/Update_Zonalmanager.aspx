<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Update_Zonalmanager.aspx.cs" Inherits="HSRP.Transaction.Update_Zonalmanager" %>
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

    
        <div style="margin: 20px;" align="left">
            <fieldset>
                <legend style="width: 144px;">
                    <div style="margin-left: 10px; font-size: medium; color: Black">
                       Zonal Manager
                    </div>
                </legend>
                <br />
               

 <asp:GridView ID="gvEmpDesignation" runat="server" Width="100%" AutoGenerateColumns="false" ShowFooter="true" ForeColor="Black" Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px" onrowupdating="gvEmpDesignation_RowUpdating" onrowcancelingedit="gvEmpDesignation_RowCancelingEdit"  onrowediting="gvEmpDesignation_RowEditing">
                <Columns>           
                    <asp:TemplateField HeaderText="Location ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "RtoLocationID") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:Label ID="lblEditid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "RtoLocationID") %>'></asp:Label>           
                        </EditItemTemplate>
                      
                    </asp:TemplateField>
 
                    <asp:TemplateField HeaderText="Location Name">
                        <ItemTemplate>
                            <asp:Label ID="lbllocationlName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Location Name") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:TextBox ID="txtEditlocationName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Location Name") %>'></asp:TextBox>           
                        </EditItemTemplate>
                       
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Zonal Manager">
                        <ItemTemplate>
                            <asp:Label ID="lblZonalName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ZonalManager") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:TextBox ID="txtEditZonalName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ZonalManager") %>'></asp:TextBox>           
                        </EditItemTemplate>
                       
                    </asp:TemplateField>
 
              
 
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                       
                            <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit"  Text="Edit"  />
                            <%--<asp:Button ID="ButtonDelete" runat="server" CommandName="Delete"  Text="Delete"  />--%>
                           
                        </ItemTemplate>
                        <EditItemTemplate>
                         
                             <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update"  Text="Update"  />
                              <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel"  Text="Cancel" />
                        </EditItemTemplate>
                       
                    </asp:TemplateField>                    
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
                </div>
                <div>
                   
                            <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                            <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        
                </div>
            </fieldset>
        </div>
</asp:Content>
