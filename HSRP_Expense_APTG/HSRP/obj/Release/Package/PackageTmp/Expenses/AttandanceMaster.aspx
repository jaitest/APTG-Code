<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AttandanceMaster.aspx.cs" Inherits="HSRP.Expenses.AttandanceMaster" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
<%--<body>
    <form id="form1" runat="server">--%>
    <div style="text-align:center;">
    <div style="font-size:medium; color:Blue; text-align:center;">Today’s Date : <%=strCurrntMonthYear %></div>
        <center>
    <asp:GridView ID="gvCalander"  Font-Size="Smaller" Font-Names="verdana, arial" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" RowStyle-BackColor="gray" AlternatingRowStyle-BackColor="Aqua" CellPadding="5"  CellSpacing="5" runat="server" ShowHeader="false" AutoGenerateColumns="false" OnRowDataBound="gvCalander_RowDataBound">
    <Columns>
    <asp:BoundField DataField="AutoID" HeaderText="Days" />
    <asp:BoundField DataField="DaysName" HeaderText="Name" />
    <asp:BoundField DataField="Date" HeaderText="Name" />
    <asp:TemplateField>
    <ItemTemplate>
    <asp:TextBox ID="txtRemarks" runat="server" Text="Remarks" Font-Size="8" onfocus="if(this.value=='Remarks'){this.value=''}" onblur="if(this.value==''){this.value='Remarks'}" ></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField>
    <ItemTemplate>
    <asp:CheckBox ID="chkMark" runat="server" />
    </ItemTemplate>
    </asp:TemplateField>
    </Columns>
        <RowStyle BackColor="Gray" HorizontalAlign="Center" />
        <HeaderStyle HorizontalAlign="Center" />
        <AlternatingRowStyle BackColor="Aqua" />
 </asp:GridView>
            </center>
    </div>
    <div style="text-align:center;">
    <asp:Button ID="btnAddAttendence" runat="server" Text="Add" OnClick="btnAddAttendence_Click"/> 
        <asp:Button ID="btnReset" runat="server" Text="Reset" />
    </div>
   <%-- </form>
</body>--%>
</asp:Content>
