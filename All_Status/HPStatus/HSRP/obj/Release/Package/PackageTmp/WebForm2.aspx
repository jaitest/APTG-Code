<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="HSRP.WebForm2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

.headingmain{
	font: normal 13pt tahoma, arial, verdana;
    color: #000000;
	font-weight:normal;
	padding-left:10px;
	line-height: 20px;
}
        .style4
        {
            width: 100%;
        }
        .style8
        {
            width: 89px;
        }
        .style9
        {
            width: 242px;
        }
        .style10
        {
            width: 77px;
        }
        .style7
        {
            height: 29px;
            width: 89px;
        }
        .style5
        {
            height: 29px;
            width: 77px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
                    <table style="width: 80%; height: 108px;">
                        <tr>
                            <td colspan="3" align="center"><span class="headingmain">Complaint</span>&nbsp;

                               
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td width="150">
                                &nbsp;</td>
                            <td class="style4">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="Region"></asp:Label>
                            </td>
                            <td width="150">
                                <asp:DropDownList ID="ddlComplaintRegion" runat="server">
                                    <asp:ListItem Value="0">-Select Region--</asp:ListItem>
                                    <asp:ListItem Value="1">Issue In Affixiation</asp:ListItem>
                                    <asp:ListItem Value="2">Complaint</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="style4">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="ddlComplaintRegion" ErrorMessage="Please Select Region" 
                                    InitialValue="-Select Region-"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td width="250">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="Owner Name"></asp:Label>
                                <br />
                            </td>
                            <td class="style8">
                                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                            </td>
                            <td class="style9">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                    ControlToValidate="txtName" ErrorMessage="This Field can not be empty"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblRegno" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="RegNo."></asp:Label>
                                <br />
                            </td>
                            <td class="style8">
                                <asp:TextBox ID="txtRegno" runat="server"></asp:TextBox>
                            </td>
                            <td class="style9">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                    ControlToValidate="txtRegno" ErrorMessage="This Field can not be empty"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style10">
                                <asp:Label ID="lblEngineNo" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="EngineNo"></asp:Label>
                                <br />
                            </td>
                            <td class="style8">
                                <asp:TextBox ID="txtEngineno" runat="server"></asp:TextBox>
                            </td>
                            <td class="style9">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ControlToValidate="txtEngineno" ErrorMessage="This Field can not be empty"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblChassisNo" runat="server" Text="ChassisNo" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                            <td class="style7">
                                <asp:TextBox ID="txtChasisNo" runat="server"></asp:TextBox>
                            </td>
                            <td class="style4">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                    ControlToValidate="txtChasisNo" ErrorMessage="This Field can not be empty"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblRemarks" runat="server" Font-Bold="True" ForeColor="Black" 
                                    Text="Remarks"></asp:Label>
                            </td>
                            <td class="style7">
                                <asp:TextBox ID="txtRemarks" runat="server"></asp:TextBox>
                            </td>
                            <td class="style4">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style5">
                                &nbsp;</td>
                            <td class="style7">
                                <asp:Button ID="btnSave" runat="server" Text="Submit" onclick="btnSave_Click" />
                            </td>
                            <td class="style4">
                                &nbsp;</td>
                        </tr>
                    </table>
    
    </div>
    </form>
</body>
</html>
