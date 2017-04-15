﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpenseMaster.aspx.cs" Inherits="HSRP.Master.ExpenseMaster" %>

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
               alert("Please Provide Expense Name");
               document.getElementById("textboxBoxHSRPState").focus();
               return false;
           }
           else if (invalidChar(document.getElementById("textboxBoxHSRPState"))) {
               alert("Special Characters Not Allowed.");
               document.getElementById("textboxBoxHSRPState").focus();
               return false;
           }
           else {
               return true;
           }
       }
    
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 20px;" align="left">
        <fieldset>
            <legend style="width:170px;">
                <div style="font-size:medium;color:Black; width: 156px;text-align:center">
                    Expense Head</div>
            </legend>
            <br />
            <table style="height: 116px">
                <tr>
                    <td class ="lable_style">
                        Expense Name :</td>
                    <td>
                        <asp:TextBox ID="textboxBoxHSRPState" runat="server" TextMode="MultiLine" onkeyup="this.value=this.value.toUpperCase();" class="form_textbox"  Width="183px" Height="36px" ></asp:TextBox>
                        
                    </td>
                </tr>
                <tr class ="lable_style">
                    <td >
                        Active Status:
                    </td>
                    <td>
                        <asp:CheckBox ID="checkBoxActiveStatus" Checked="true" TextAlign="Left" runat="server" 
                             TabIndex="1" />
                    </td>
                </tr>
                <tr align="right" style="margin-right:10px">
                <td>
                         <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                          <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        </td>
                    <td colspan="2" nowrap="nowrap">
                        <br />
                        <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8"  OnClientClick=" return validate()" 
                                class="button" onclick="buttonUpdate_Click" />&nbsp;&nbsp;
                        <asp:Button ID="ButtonSave" runat="server" Text="Save" TabIndex="2" OnClientClick=" return validate()" 
                            class="button" onclick="ButtonSave_Click1" />&nbsp;&nbsp;
                             <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" /> &nbsp;&nbsp;
                             <%--<input type="reset" class="button" value="Reset" />--%>
                    </td>
                    
                </tr>
            </table>
        </fieldset>
    </div>
    </form>
</body>
</html>