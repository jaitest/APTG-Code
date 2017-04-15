﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpAmountApprovebyAcc.aspx.cs" Inherits="HSRP.Transaction.ExpAmountApprovebyAcc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/AutoNumeric.js" type="text/javascript"></script>


    <script type="text/javascript">
        function validate() {

            if (document.getElementById("txtVerifyAmt").value == "") {
                alert("Fill Amount");
                document.getElementById("txtVerifyAmt").focus();
                return false;
            }
            if ((parseFloat(document.getElementById("txtVerifyAmt").value)) > (parseFloat(document.getElementById("LabelBillAmount").value))) {
                alert("Amount should not exceed the bil amount");
                document.getElementById("txtVerifyAmt").focus();
                return false;
            }
        }
    </script>
    <script type="text/javascript" language="javascript">

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57 || charCode == 110) && charCode != 46) {

                return false;
            }
            else {

                //               var len = document.getElementById("ctl00_ContentPlaceHolder1_txtAmount").value.length;
                //               var index = document.getElementById("ctl00_ContentPlaceHolder1_txtAmount").value.indexOf('.');

                //               if (index > 0 && charCode == 46) {
                //                   return false;
                //               }
                //               if (index > 0) {
                //                   var CharAfterdot = (len + 1) - index;
                //                   if (CharAfterdot > 6) {
                //                       return false;
                //                   }
                //               }
                //               if (len < 8 && charCode == 46) {
                //                   return false;
                //               }

                //               var text = document.getElementById("ctl00_ContentPlaceHolder1_txtAmount").value;

                //               if (charCode != 46 && len == 8) {
                //                   return false;
                //               }

            }
            return true;
        }

        // example uses the selector "input" with the class "auto" & no options passed
        jQuery(function ($) {
            $('#txtVerifyAmt').autoNumeric();
            $('#txtOthers').autoNumeric();

        });

        // example uses the selector "input" with the class "auto" & with options passed
        // See details below on allowed options
        jQuery(function ($) {
            $('#txtVerifyAmt').autoNumeric({ aSep: '.', aDec: '' });
            $('#txtOthers').autoNumeric({ aSep: '.', aDec: '' });

        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="margin: 20px;" align="left">
            <fieldset>
                <legend>
                    <div style="margin-left: 10px; font-size: medium; color: Black">
                        Verify Expense 
                    </div>
                </legend>
                <div style="width: 880px; margin: 0px auto 0px auto">
                    <table style="background-color: White" width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
                        <tr>
                            <td colspan="7">
                                <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
                                </asp:ScriptManager>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text_View" style="padding-bottom: 10px">State Name :
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="LabelStateID" runat="server" class="form_text_View"></asp:Label>
                            </td>
                            <td></td>
                            <td class="form_text_View" style="padding-bottom: 10px">Location Name :
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="LabelRTOLocationID" runat="server" class="form_text_View"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text_View" style="padding-bottom: 10px">Expense Name :
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="LabelExpenseName" runat="server" class="form_text_View"></asp:Label>
                            </td>
                            <td></td>
                            <td class="form_text_View" style="padding-bottom: 10px">Bill No :
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="LabelBillNo" runat="server" class="form_text_View"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text_View" style="padding-bottom: 10px">Bill Date :
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="LabelBillDate" runat="server" class="form_text_View"></asp:Label>
                            </td>
                            <td></td>
                            <td class="form_text_View" style="padding-bottom: 10px">Bill Amount :
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="LabelBillAmount" runat="server" class="form_text_View"></asp:Label>
                            </td>
                        </tr>

                        <%--<tr>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            VAT\CST :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="lblVat" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Service Tax :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="lblService" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    </tr>--%>

                        <%--<tr>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Excise Duty + Cess :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="lblExcise" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Others :
                        </td>
                        <td></td>
                        <td>
                            <asp:TextBox class="form_textbox11" TabIndex="1" Width="150" ID="txtOthers" 
                                                                    runat="server" MaxLength="7"></asp:TextBox>
                        </td>   
                    </tr>--%>

                        <tr>
                            <td class="form_text_View" style="padding-bottom: 10px">Remarks :
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="Remarks" runat="server" class="form_text_View"></asp:Label>

                            </td>
                            <td></td>
                            <td class="form_text_View" style="padding-bottom: 10px">Vendor/Supplier Name :
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblVendor" runat="server" class="form_text_View"></asp:Label>
                            </td>
                            <td></td>
                        </tr>

                        <tr>

                            <td class="form_text_View" style="padding-bottom: 10px">Claimed By :
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblClaimedBy" runat="server" class="form_text_View"></asp:Label>

                            </td>
                            <td></td>
                            <td class="form_text_View" style="padding-bottom: 10px">
                                <asp:Label ID="lblApprovedAmountByHOD" runat="server" class="form_text_View" Text="Approved Amount By HOD: "></asp:Label>
                            </td>
                            <td></td>
                            <td>

                                <asp:Label ID="lblApprovedByHODAmount" runat="server" class="form_text_View" Text=""></asp:Label>


                            </td>
                        </tr>
                        <tr>

                            <td class="form_text_View" style="padding-bottom: 10px">Approved By HOD Name :
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblHODName" runat="server" class="form_text_View"></asp:Label>

                            </td>
                            <td></td>
                            <td class="form_text_View" style="padding-bottom: 10px">
                                <asp:Label ID="lblApprovelRemarks" runat="server" class="form_text_View" Text=" Approvel Remarks: "></asp:Label>
                            </td>
                            <td></td>
                            <td>

                                <asp:Label ID="lblApprovelRemarksData" runat="server" class="form_text_View" Text=""></asp:Label>


                            </td>
                        </tr>
                        <tr>

                            <td class="form_text_View" style="padding-bottom: 10px">Approved Date:
                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblApprovedDateData" runat="server" class="form_text_View"></asp:Label>

                            </td>
                            <td></td>
                            <td class="form_text_View" style="padding-bottom: 10px">
                                <asp:Label ID="lblVerifyAmount" runat="server" class="form_text_View" Text="Verify Amount"></asp:Label>
                                <span class="alert">* </span>

                            </td>
                            <td></td>
                            <td>

                                <asp:TextBox ID="txtVerifyAmount" runat="server"></asp:TextBox>


                            </td>
                        </tr>

                        <tr>
                            <td class="form_text_View" style="padding-bottom: 10px; vertical-align: top">Verify Remarks :</td>
                            <td></td>
                            <td colspan="7">
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="4" TabIndex="2" Columns="40"></asp:TextBox>
                            </td>

                        </tr>


                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                <asp:HiddenField ID="billAmt" runat="server" />
                                <asp:HiddenField ID="RemainAmt" runat="server" />
                                <asp:HiddenField ID="OtherAmount" runat="server" />

                            </td>




                            <td colspan="3" align="right">


                                <asp:Button ID="btnSave" runat="server" class="button" Text="Verify" TabIndex="3" OnClientClick=" return validate()"
                                    OnClick="btnSave_Click" />&nbsp;&nbsp;
                     <input type="button" onclick="javascript: parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" />

                            </td>
                        </tr>

                    </table>
                </div>
            </fieldset>
        </div>

    </form>
</body>
</html>