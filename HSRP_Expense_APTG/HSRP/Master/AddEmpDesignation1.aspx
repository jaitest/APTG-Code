﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEmpDesignation.aspx.cs" Inherits="HSRP.Master.AddEmpDesignation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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

</head>
<body>
    <form id="form1" runat="server">
        <div style="margin: 20px;" align="left">
            <fieldset>
                <legend>
                    <div style="margin-left: 10px; font-size: medium; color: Black">
                        Add Employee Designation
                    </div>
                </legend>
                <br />
                <table>
                    <tr>
                        <td class="lable_style">Designation Name :</td>
                        <td>
                            <asp:TextBox ID="textboxBoxHSRPState" runat="server" onkeyup="this.value=this.value.toUpperCase();" class="form_textbox" TabIndex="1"></asp:TextBox>
                        </td>
                    </tr>
                    <%--<tr class="lable_style">
                        <td>Active Status:
                        </td>
                        <td>
                        <asp:CheckBox ID="checkBoxActiveStatus" Checked="true" TextAlign="Left" Style="left: 8px; padding-left: 5px;" runat="server" TabIndex="2" />
                        </td>
                    </tr>--%>
                    <tr align="left" style="margin-right: 10px">
                        <td>
                            <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                            <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        </td>
                        <td colspan="2" nowrap="nowrap" style="padding-left: 9px;">
                            <br />
                        <asp:Button ID="ButtonSave" runat="server" Text="Save" TabIndex="3" OnClientClick=" return validate()" class="button" OnClick="ButtonSave_Click1" />&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div style="margin: 20px;" align="left">
            <fieldset>
                <legend>
                    <div style="margin-left: 10px; font-size: medium; color: Black">
                        Emplopee Designation List
                    </div>
                </legend>
                <br />
                <div style="text-align: center;">
                    <div class="GridviewDiv">
                        <asp:GridView runat="server" ID="gvEmpDesignation" DataKeyNames="ID" OnRowDataBound="gvEmpDesignation_RowDataBound" OnRowEditing="gvEmpDesignation_RowEditing" OnRowCancelingEdit="gvEmpDesignation_RowCancelingEdit" OnRowUpdating="gvEmpDesignation_RowUpdating" OnRowDeleting="gvEmpDesignation_RowDeleting" Width="100%" EmptyDataText="No records has been added." AutoGenerateEditButton="true" AutoGenerateDeleteButton="true" >
                            <HeaderStyle CssClass="headerstyle" />
                        </asp:GridView>
                    </div>
                </div>
            </fieldset>
        </div>
    </form>
</body>
</html>
