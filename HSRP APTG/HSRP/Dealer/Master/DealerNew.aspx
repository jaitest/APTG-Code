﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DealerNew.aspx.cs" Inherits="HSRP.Dealer.Master.DealerNew" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/User.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../javascript/common.js" type="text/javascript"></script>
    <%--<script src="http://code.jquery.com/jquery-1.4.3.min.js" type="text/javascript"></script>--%>
 
    <script type="text/javascript">
        function invalidChar11(_objData1) {
            // debugger;
            var iChars = '!$%^*+=[]{}|"<>?';

            for (var i = 0; i < _objData1.value.length; i++) {
                if (iChars.indexOf(_objData1.value.charAt(i)) != -1) {
                    return true;
                }
            }
        }
    </script>
    <script type="text/javascript" language="javascript">

        function vali() {

            if (document.getElementById("<%=textBoxDealerName.ClientID%>").value == "") {
                alert("Please Provide Dealer Name.");
                document.getElementById("<%=textBoxDealerName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=DropdownRTOName.ClientID%>").value == "--Select RTO Location--") {
                alert("Select Rto Location");
                document.getElementById("<%=DropdownRTOName.ClientID%>").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textBoxDealerName"))) {
                alert("Please provide valid dealer name.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("textBoxDealerName").focus();
                return false;
            }


            if (document.getElementById("<%=textBoxPersonName.ClientID%>").value == "") {
                alert("Please Provide Person Name.");
                document.getElementById("<%=textBoxPersonName.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxPersonName"))) {
                alert("Please provide valid person name.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("textBoxPersonName").focus();
                return false;
            }
            if (document.getElementById("<%=txtERPClientCode.ClientID%>").value == "") {
                alert("Please Provide ERP Client Code.");
                document.getElementById("<%=txtERPClientCode.ClientID%>").focus();
                return false;
            }
            if (invalidChar11(document.getElementById("txtERPClientCode"))) {
                alert("Please provide valid ERP Client Code.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtERPClientCode").focus();
                return false;
            }
            if (document.getElementById("<%=txtERPEmbossingCode.ClientID%>").value == "") {
                alert("Please Provide ERP Embossing Code.");
                document.getElementById("<%=txtERPEmbossingCode.ClientID%>").focus();
                return false;
            }
            if (invalidChar11(document.getElementById("txtERPEmbossingCode"))) {
                alert("Please provide valid ERP Embossing Code.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtERPEmbossingCode").focus();
                return false;
            }

            if (document.getElementById("<%=textBoxMobileNo.ClientID%>").value == "") {
                alert("Please Provide Mobile No.");
                document.getElementById("<%=textBoxMobileNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=textBoxAddress.ClientID%>").value == "") {
                alert("Please Provide Address.");
                document.getElementById("<%=textBoxAddress.ClientID%>").focus();
                return false;
            }
            if (invalidChar11(document.getElementById("textBoxAddress"))) {
                alert("Please provide valid address.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("textBoxAddress").focus();
                return false;
            }
            if (document.getElementById("<%=textBoxCity.ClientID%>").value == "") {
                alert("Please Provide City.");
                document.getElementById("<%=textBoxCity.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxCity"))) {
                alert("Please provide valid city name.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("textBoxCity").focus();
                return false;
            }

            //            var emailID = document.getElementById("textBoxEmail").value;
            //            if (emailID != "") {
            //                if (emailcheck(emailID) == false) {
            //                    document.getElementById("textBoxEmail").value = "";
            //                    document.getElementById("textBoxEmail").focus();
            //                    return false;
            //                }
            //            }


          
          


            if (document.getElementById("<%=textBoxDealerArea.ClientID%>").value == "") {
                alert("Please Provide Dealer Area.");
                document.getElementById("<%=textBoxDealerArea.ClientID%>").focus();
                return false;
            }


            if (invalidChar(document.getElementById("textBoxDealerArea"))) {
                alert("Please provide valid dealer area.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("textBoxDealerArea").focus();
                return false;
            }



        }

        function isNumberKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <style type="text/css">
        .style4
        {
            width: 428px;
        }
        .style5
        {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 103px;
            padding-left: 20px;
        }
        .style6
        {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 184px;
            padding-left: 20px;
        }
        .style7
        {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 174px;
            padding-left: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
               <div style="margin-left: 10px; font-size: medium; background:yellow; height:22px; width:119px; color: Black">
                    Dealer Profile</div>
            </legend>
            <table style="background-color: #FFFFFF" width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td>
                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table border="0" align="center" cellpadding="3" cellspacing="1" style="height: 385px;
                                        width: 100%">
                                        <tr>
                                            <td style="margin-left: 10px; font-size: medium; color: Black">
                                                <table border="0" align="center" cellpadding="3" cellspacing="3" style="height: 348px;
                                                    width: 100%">

                                                    
                                                    <tr>
                                                      
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px; ">
                                                        
                                                            State : <span class="alert">* </span> 
                                                        </td>
                                                        <td class="style4">
                                                            <%-------- Ambrish Coding ?? -------%>
                                                             <asp:DropDownList ID="ddlState" Height="20px" Width="175px" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" >
                                                            </asp:DropDownList>
                                                            <%-- <asp:DropDownList class="form_textbox12" runat="server" ID="ddlState" Height="20px" Width="175px">
                                                                <asp:ListItem Text="--Select State--" Value="--Select State--" />
                                                                <asp:ListItem Text="Bihar" Value="Bihar" />
                                                                <asp:ListItem Text="Delhi" Value="Delhi" />
                                                                <asp:ListItem Text="haryana" Value="Haryana" />
                                                                <asp:ListItem Text="HP" Value="HP" />
                                                                <asp:ListItem Text="MP" Value="MP" />
                                                                <asp:ListItem Text="Uttrakhand" Value="Uttrakhand" />
                                                            </asp:DropDownList>--%>

                                                            <br />
                                                        </td>
                                                        <td style="width:12px">
                                                            &nbsp;
                                                        </td>
                                                        <td class="style5" nowrap="nowrap" style="padding-bottom: 10px; ">
                                                              
                                                            Dealer Name : <span class="alert">* </span> 
                                                            <br />
                                                        </td>
                                                        <td class="style6">
                                                            <asp:TextBox class="form_textbox12" TabIndex="1" MaxLength="50" ID="textBoxDealerName"
                                                                runat="server" Text=""></asp:TextBox>
                                                            <br /> 
                                                        </td>
                                                    </tr>


                                                    <%--<tr>
                                                        <br />
                                                        <br />
                                                        <td nowrap="nowrap" class="Label_user" style="padding-bottom: 10px; width:28px">
                                                        
                                                        </td>
                                                        <td class="style4">
                                                            
                                                        </td>
                                                        <td style="width:12px">
                                                            &nbsp;
                                                        </td>
                                                        <td class="Label_user" nowrap="nowrap" style="padding-bottom: 10px; width:28px">
                                                            Dealer Code: <span class="alert">* </span>
                                                            <br />
                                                        </td>
                                                        <td class="style6">
                                                            <asp:TextBox class="form_textbox12" MaxLength="50" ID="textBoxDealerCode" TabIndex="2"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>--%>

                                                    
                                                    <tr>
                                                      
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px; ">
                                                        
                                                            <asp:Label ID="lblemb" runat="server" Text="Embossing Center: *" 
                                                                Visible="False"></asp:Label>
&nbsp;<span class="alert"> </span> </td>
                                                        <td class="style4">
                                                            <%--<asp:DropDownList ID="ddlembossing" runat="server" Height="20px" Width="187px" 
                                                                Visible="False">
                                                                <asp:ListItem>--Select Embossing Center-- </asp:ListItem>
                                                                <asp:ListItem>Gurgaon</asp:ListItem>
                                                                <asp:ListItem>Faridabad</asp:ListItem>
                                                                <asp:ListItem>Sonepat</asp:ListItem>
                                                                <asp:ListItem>Karnal</asp:ListItem>
                                                                <asp:ListItem>Hissar</asp:ListItem>
                                                                <asp:ListItem>Rohtak</asp:ListItem>
                                                            </asp:DropDownList>--%>
                                              
                                                <asp:DropDownList ID="dropDownListClient" Height="20px" Width="175px" 
                                                    runat="server" DataTextField="EmbCenterName" AutoPostBack="false"
                                                    DataValueField="NAVEMBID" >
                                                </asp:DropDownList>
                                        
                         
                                                             </td>
                                                        <td style="width:12px">
                                                            &nbsp;</td>
                                                        <td class="style5" nowrap="nowrap" style="padding-bottom: 10px; ">
                                                              ERP Client Code <span class="alert">* </span>
                                                            &nbsp;</td>
                                                        <td class="style6">
                                                            <asp:TextBox ID="txtERPClientCode" MaxLength="50" TabIndex="3" class="form_textbox12" runat="server"></asp:TextBox>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                      
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px; ">
                                                        RTO Location:<span class="alert">*</span>
                                                             </td>
                                                        <td class="style4">
                                                            <asp:DropDownList ID="DropdownRTOName" DataTextField="Rtolocationname" DataValueField="Rtolocationid" Height="20px" Width="175px"
                                                runat="server" TabIndex="1" AutoPostBack="True">
                                                <asp:ListItem>--Select RTO Location--</asp:ListItem>
                                            </asp:DropDownList>
                                                             </td>
                                                        <td style="width:12px">
                                                            &nbsp;</td>
                                                        <td class="style5" nowrap="nowrap" style="padding-bottom: 10px; ">
                                                            ERP Embossing Code  <span class="alert">* </span>
                                                            &nbsp;</td>
                                                        <td class="style6">
                                                            <asp:TextBox ID="txtERPEmbossingCode" MaxLength="50" TabIndex="3" class="form_textbox12" runat="server"></asp:TextBox>
                                                            &nbsp;</td>
                                                    </tr>


                                                    <tr>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            Contact Person Name: <span class="alert">* </span>
                                                        </td>
                                                        <td class="style4">
                                                            <asp:TextBox class="form_textbox12" ID="textBoxPersonName" MaxLength="50" TabIndex="3"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="style5" nowrap="nowrap">
                                                            Contact Person Mobile: <span class="alert">* </span>
                                                            <br />
                                                        </td>
                                                        <td class="style6">
                                                            <asp:TextBox class="form_textbox12" MaxLength="10" ID="textBoxMobileNo" TabIndex="4"
                                                                runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style7">
                                                            Address : <span class="alert">* </span>
                                                        </td>
                                                        <td class="style4">
                                                            <asp:TextBox class="form_textbox12" ID="textBoxAddress" TabIndex="5" runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="style5">
                                                            City <span class="alert">* </span>
                                                        </td>
                                                        <td  class="style6">
                                                            <asp:TextBox class="form_textbox12" MaxLength="30" ID="textBoxCity" TabIndex="6"
                                                                runat="server"></asp:TextBox><br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style7">
                                                            Sales Person :<span class="alert">* </span>
                                                        </td>
                                                        <td class="style4">
                                                            <%--                                                            <asp:TextBox class="form_textbox12" MaxLength="30" ID="textBoxState" TabIndex="7"
                                                                runat="server"></asp:TextBox>--%>
                                                           <asp:DropDownList class="form_textbox12" runat="server" DataTextField ="ContactPersionName" DataValueField ="contactPersionID" ID="ddlSalesPerson" Height="20px" Width="175px">
                                                                <asp:ListItem Text="--Select State--" Value="--Select State--" />
                                                                <asp:ListItem Text="TGDealer" Value="TGDealer" />
                                                                <asp:ListItem Text="TS" Value="TS" />
                                                            </asp:DropDownList> 
                                                            <br />
                                                        </td>
                                                        <td class="style8">
                                                            &nbsp;
                                                        </td>
                                                        <td class="style5" nowrap="nowrap">
                                                            Area Of Dealer: <span class="alert">* </span>
                                                        </td>
                                                        <td  class="style6">
                                                            <asp:TextBox class="form_textbox12" MaxLength="100" ID="textBoxDealerArea" runat="server"
                                                                TabIndex="8"></asp:TextBox><br />
                                                        </td>

                                                         
                                                    </tr>
                                                  
                                                    <tr>
                                                        <td class="style7">
                                                            Type of Dealer :<span class="alert">* </span>
                                                        </td>
                                                        <td class="style4">
                                                            <%--                                                            <asp:TextBox class="form_textbox12" MaxLength="30" ID="textBoxState" TabIndex="7"
                                                                runat="server"></asp:TextBox>--%>
                                                           <asp:DropDownList class="form_textbox12" runat="server" ID="ddltypeofdear" Height="20px" Width="175px">
                                                               <asp:ListItem Text="--Select State--" Value="--Select State--" />
                                                                <asp:ListItem Text="2W" Value="2W" />
                                                                <asp:ListItem Text="3W" Value="3W" />
                                                               <asp:ListItem Text="4W" Value="4W" />
                                                            </asp:DropDownList> 
                                                            <br />
                                                        </td>
                                                        <td class="style8">
                                                            &nbsp;
                                                        </td>
                                                        <td class="style5" nowrap="nowrap">
                                                            Name of OEM: <span class="alert">* </span>
                                                        </td>
                                                        <td  class="style6">
                                                            <asp:TextBox class="form_textbox12" MaxLength="100" ID="txtoem" runat="server"
                                                                TabIndex="9"></asp:TextBox><br />
                                                        </td>

                                                         
                                                    </tr>
                                                    <tr>
                                                        <td class="style7">
                                                            Type of Business Entity:<span class="alert">* </span>
                                                        </td>
                                                        <td class="style4">
                                                            <%--                                                            <asp:TextBox class="form_textbox12" MaxLength="30" ID="textBoxState" TabIndex="7"
                                                                runat="server"></asp:TextBox>--%>
                                                           <asp:DropDownList class="form_textbox12" runat="server" ID="ddltypeofbusinessentity" Height="20px" Width="175px">
                                                               <asp:ListItem Text="--Select State--" Value="--Select State--" />
                                                                <asp:ListItem Text="Sole Properietorship Private Ltd.Co." Value="Soleproperietorshippvtltd" />
                                                                <asp:ListItem Text="Partnership Public Ltd. Co." Value="Partnershippublic" />
                                                               <asp:ListItem Text="Other(Please Specify)" Value="other" />
                                                            </asp:DropDownList> 
                                                            <br />
                                                        </td>
                                                        <td class="style8">
                                                            &nbsp;
                                                        </td>
                                                        <td class="style5" nowrap="nowrap">
                                                            Income Tax Pan No.: <span class="alert">* </span>
                                                        </td>
                                                        <td  class="style6">
                                                            <asp:TextBox class="form_textbox12" MaxLength="100" ID="txtincometxtpanno" runat="server"
                                                                TabIndex="10"></asp:TextBox><br />
                                                        </td>

                                                         
                                                    </tr>

                                                     <tr>
                                                        <td class="style7">
                                                           CST No : <span class="alert">* </span>
                                                        </td>
                                                        <td class="style4">
                                                            <asp:TextBox class="form_textbox12" ID="txtcstno" TabIndex="11" runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="style5">
                                                            VAT TIN NO : <span class="alert">* </span>
                                                        </td>
                                                        <td  class="style6">
                                                            <asp:TextBox class="form_textbox12" MaxLength="30" ID="txtvatno" TabIndex="12"
                                                                runat="server"></asp:TextBox><br />
                                                        </td>
                                                    </tr>



                                                    <tr>
                                                        <td colspan="5" class="Label_user">
                                                            <asp:CheckBox ID="checkBoxStatus" runat="server" Text="Active Status : " 
                                                                TabIndex="14" Visible="false"
                                                                TextAlign="Left" Checked="True" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" nowrap="nowrap" align="right" style="margin-right: 200px">
                                                            <asp:Button ID="buttonUpdate" runat="server" class="button" TabIndex="15" Visible="false"
                                                                OnClientClick=" javascript:return vali();" Text="Update" OnClick="buttonUpdate_Click" />
                                                            &nbsp;
                                                            <asp:Button ID="buttonSave" runat="server" TabIndex="16" class="button" Text="Save"
                                                                OnClientClick=" javascript:return vali();" Visible="false" OnClick="buttonSave_Click" />&nbsp;
                                                            <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                                                id="buttonClose" value="Close" class="button" />
                                                            &nbsp;
                                                            <input type="reset" class="button" value="Reset" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="alert">
                                    * Fields are mandatory.
                                </td>
                            </tr>
                            <tr>
                                <td class="FieldText">
                                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                    <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
        </fieldset>
    </div>
    </form>
</body>
</html>

