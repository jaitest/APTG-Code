using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace IntegrationKIt4._5
{
    public partial class hsrponlinepayment : System.Web.UI.Page
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string ProductivityID = string.Empty;
        string UserType = string.Empty;
        string UserName = string.Empty;
        string Sticker = string.Empty;
        string VIP = string.Empty;
        string USERID = string.Empty;
        DataTable dt = new DataTable();
        string macbase = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
           
        }



        public void refresh()
        {
            txtAuthNo.Text = "";
            lblRTOLocationCode.Text = "";
            lblRTOName.Text = "";
            txtEmail.Text = "";
            lblOwnerName.Text = "";
            lblAddress.Text = "";
            lblVehicleType.Text = "";
            lblTransactionType.Text = "";
            lblAuthDate.Text = "";
            lblMobileNo.Text = "";
            lblVehicleClassType.Text = "";
            lblMfgName.Text = "";
            lblModelName.Text = "";
            lblRegNo.Text = "";
            lblEngineNo.Text = "";
            lblChasisNo.Text = "";
            lblAmount.Text = "";
        }


        string Query = string.Empty;
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                string ToDay = System.DateTime.Today.DayOfWeek.ToString();
                if (ToDay == "Sunday")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "No Collection Allowed On Sunday";

                    return;
                }

                if (lblOwnerName.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Record invalid to proceed. Check authorization number. ";
                    return;
                }
                string strEmail = string.Empty;

                if (txtEmail.Text.Trim() == "")
                {
                    strEmail = "";
                }
                else
                {
                    strEmail = txtEmail.Text.Trim();
                    try
                    {
                        MailAddress m = new MailAddress(strEmail);


                    }
                    catch (FormatException)
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "";
                        lblErrMess.Text = "Email ID provided is not valid.";
                        return;
                    }
                }


                VIP = "N";
                string sticker1 = Sticker;

                string sql = "exec [getPlatesData] '9','" + lblVehicleType.Text + "','" + lblVehicleClassType.Text + "', '" + lblTransactionType.Text + "'";
                DataTable dt = Utils.GetDataTable(sql, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    lblSucMess.Visible = false;
                }
                string AuthNo = txtAuthNo.Text.Trim();

                Query = "select count(*) from TGOnlinePayment where HSRP_StateID=11 and OnlinePaymentStatus='Y' and HSRPRecord_AuthorizationNo ='" + AuthNo + "'";
                int co = Utils.getScalarCount(Query, ConnectionString);
                if (co > 0)
                {
                    lblSucMess.Visible = false;
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Authorization No. Already Exist With Status HSRP Fee Paid.";
                    return;
                }
                else
                {
                    DataTable dt5 = new DataTable();
                    string cashrc = string.Empty;
                    string authdate = string.Empty;

                    string[] arrauthdate = lblAuthDate.Text.Replace("T", " ").Split('.');
                    authdate = arrauthdate[0].ToString();
                    string[] arrauthdate1 = arrauthdate[0].ToString().Split('+');
                    authdate = arrauthdate1[0].ToString();

                    string Invoice = string.Empty;
                    string DC = string.Empty;
                    cashrc = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                    cashrc = Utils.getScalarValue(cashrc, ConnectionString);
                    //cashrc = dt5.Rows[0]["Receiptno"].ToString();
                    Invoice = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Invoice No'";
                    Invoice = Utils.getScalarValue(Invoice, ConnectionString);
                    DC = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Delivery Challan No'";
                    DC = Utils.getScalarValue(DC, ConnectionString);
                    lblAddress.Text = lblAddress.Text.Replace("'", "");
                    lblErrMess.Text = "";

                    String strquery1 = "select [dbo].GetAffxDate_insert_new('" + HSRPStateID + "') as Date";
                    string date1 = Utils.getDataSingleValue(strquery1, ConnectionString, "Date");

                    string OrderNo = "TS" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    //Expected Affixation Date has been added on 8th Jan 2014
                    string strQuery = "insert into TGOnlinePayment (SaveMacAddress,DeliveryChallanNo,ISFrontPlateSize,ISRearPlateSize,invoiceno, address1,manufacturername," +
                        " HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo," +
                        "VehicleClass,OrderType,StickerMandatory,isVIP,NetAmount,VehicleType,OrderStatus,CashReceiptNo,OwnerFatherName,EmailID,ChassisNo, EngineNo," +
                        "frontplatesize, rearplatesize,CreatedBy,ManufacturerModel,vehicleref,FrontplatePrize,RearPlatePrize,StickerPrize,ScrewPrize,TotalAmount," +
                        "VAT_Amount,RoundOff_NetAmount,VAT_Percentage,affix_id,PlateAffixationDate,OnlinePaymentStatus,OnlinePaymentID) " +
                        "values('" + macbase + "','" + DC + "', '" + dt.Rows[0]["frontplateflag"].ToString() + "','" + dt.Rows[0]["rearplateflag"].ToString() + "', '" + Invoice +
                        "','" + lblAddress.Text + "','" + lblMfgName.Text + "', GetDate(),         '" + HSRPStateID + "','" + RTOLocationID + "','" + AuthNo + "','" +
                        authdate + "','" + lblRegNo.Text.Trim().ToUpper() + "','" + lblOwnerName.Text + "','" + lblMobileNo.Text + "','" + lblVehicleClassType.Text +
                        "','" + lblTransactionType.Text + "','" + sticker1 + "','" + VIP + "','" + lblAmount.Text + "','" + lblVehicleType.Text + "','New Order','" + cashrc +
                        "',    '','" + strEmail + "', '" + lblChasisNo.Text.Trim() + "', '" + lblEngineNo.Text.Trim() + "', '" +
                        dt.Rows[0]["frontplateID"].ToString() + "', '" + dt.Rows[0]["RearPlateID"].ToString() + "','" + USERID + "','" + lblModelName.Text +
                        "','New','" + dt.Rows[0]["FrontPlateCost"].ToString() + "','" + dt.Rows[0]["rearplatecost"].ToString() + "','" + dt.Rows[0]["stickercost"].ToString() +
                        "','" + dt.Rows[0]["snaplockcost"].ToString() + "','" + dt.Rows[0]["totalamount"].ToString() + "','" + dt.Rows[0]["vatamount"].ToString() + "','" +
                        Math.Round(decimal.Parse(lblAmount.Text), 0) + "','" + dt.Rows[0]["vatper"].ToString() + "','1','" + date1 + "','N','" + OrderNo + "')";
                    int i = Utils.ExecNonQuery(strQuery, ConnectionString);
                    if (i > 0)
                    {

                        lblSucMess.Visible = true;
                        lblErrMess.Visible = false;
                        Response.Redirect("tgonline2.aspx?X=" + HttpContext.Current.Server.HtmlEncode(AuthNo.ToString()) + "&orderNo=" + HttpContext.Current.Server.HtmlEncode(OrderNo));

                    }

                }
            }
            catch (Exception ex)
            {
                lblErrMess.Text = "Message : " + ex.Message.ToString();
                return;
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            HSRPStateID = "11";
          

                string strAuthno = string.Empty;
                string StrRtoLocationCode = string.Empty;
                string StrRtoName = string.Empty;
                string StrAuthorizationNo = string.Empty;
                string StrOwnerName = string.Empty;
                string StrOwnerAddress = string.Empty;
                string StrVehicleType = string.Empty;
                string StrTransactonType = string.Empty;
                string StrAuthdate = string.Empty;
                string StrMobileNo = string.Empty;
                string StrVehicleClassType = string.Empty;
                string StrManufacturarName = string.Empty;
                string StrModelName = string.Empty;
                string StrRegistrationNo = string.Empty;
                string StrEngineNo = string.Empty;
                string StrChasisNo = string.Empty;
                string strAuthNo = txtAuthNo.Text;
                string stremail = string.Empty;
                string SQLString = string.Empty;
                string RTOlocationName = string.Empty;
            
                try
                {
                    

                    string strDate = System.DateTime.Now.ToString("hh:mm tt");
                    string strnine = "09:00 AM";
                    string strsix = "06:00 PM";
                    if (DateTime.Parse(strDate) < DateTime.Parse(strnine) || DateTime.Parse(strDate) > DateTime.Parse(strsix))
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "";
                        lblErrMess.Text = "Cash Collection Timing is  Between 9 AM TO 6 PM ";
                        return;
                    }

                    if (txtAuthNo.Text.Trim()=="")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "";
                        lblErrMess.Text = "Please Provide Valid Authorization Number. ";
                        return;
                    }

                    strAuthNo = txtAuthNo.Text;

                    HSRP.HSRPAuthorizationServiceSoapClient objTgService = new HSRP.HSRPAuthorizationServiceSoapClient();
                    string AuthData = objTgService.GetHSRPAuthorizationno(strAuthNo);


                    if (AuthData == "1")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Record not Found";
                        refresh();
                        return;
                    }
                    else if (AuthData.Length > 1)
                    {
                        using (StringReader stringReader = new StringReader(AuthData))
                        using (XmlTextReader reader = new XmlTextReader(stringReader))
                        {
                            while (reader.Read())
                            {

                                if (reader.Name.ToString() == "Rto_Code")
                                {
                                    reader.Read();
                                    StrRtoLocationCode = reader.Value.ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Rto_Name")
                                {
                                    reader.Read();
                                    StrRtoName = reader.Value.ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Authorization_Ref_no")
                                {
                                    reader.Read();
                                    strAuthno = reader.Value.ToString();
                                    //StrAuthorizationNo = AuthArray[2].ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Owner_Name")
                                {
                                    reader.Read();
                                    StrOwnerName = reader.Value.ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Owner_Address")
                                {
                                    reader.Read();
                                    StrOwnerAddress = reader.Value.ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Vehicle_Type")
                                {
                                    reader.Read();
                                    string strquery = "select upper(ourValue) as ourValue from  [dbo].[Mapping_Vahan_HSRP_ap] where rtovalue ='" + reader.Value.ToString() + "'";
                                    // Utils.getScalarValue( 
                                    StrVehicleType = Utils.getDataSingleValue(strquery, ConnectionString, "ourValue");

                                    // StrVehicleType = reader.Value.ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Trans_Type")
                                {
                                    reader.Read();
                                    StrTransactonType = reader.Value.ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Authorization_Date")
                                {
                                    reader.Read();
                                    StrAuthdate = reader.Value.ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Mobile_No")
                                {
                                    reader.Read();
                                    StrMobileNo = reader.Value.ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Owner_Email_Id")
                                {
                                    reader.Read();
                                    stremail = reader.Value.ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Veh_Class_Type")
                                {
                                    reader.Read();
                                    if (reader.Value.ToString() == "N")
                                    {
                                        StrVehicleClassType = "Non-Transport";
                                    }
                                    else if (reader.Value.ToString() == "T")
                                    {
                                        StrVehicleClassType = "Transport";
                                    }
                                    reader.Read();

                                }
                                if (reader.Name.ToString() == "MFRS_Name")
                                {
                                    reader.Read();
                                    StrManufacturarName = reader.Value.ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Model_Name")
                                {
                                    reader.Read();
                                    StrModelName = reader.Value.ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Reg_no")
                                {
                                    reader.Read();
                                    lblErrMess.Text = "";
                                    StrRegistrationNo = reader.Value.ToString().Trim();
                                    //if (StrRegistrationNo.Length > 10)
                                    //{
                                    //    lblErrMess.Visible = true;
                                    //    lblErrMess.Text = "Record Not Found. Please contact your RTO.";
                                    //    return;
                                    //}

                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Engine_No")
                                {
                                    reader.Read();
                                    StrEngineNo = reader.Value.ToString();
                                    reader.Read();
                                }
                                if (reader.Name.ToString() == "Chassis_No")
                                {
                                    reader.Read();
                                    StrChasisNo = reader.Value.ToString();
                                    reader.Read();
                                }

                            }
                        }
                        lblAffix.Text = StrRtoName;
                        txtAuthNo.Text = strAuthno;
                        lblRTOLocationCode.Text = StrRtoLocationCode;
                        lblRTOName.Text = StrRtoName;
                        txtEmail.Text = stremail.Replace("'", "");
                        lblOwnerName.Text = StrOwnerName;
                        lblAddress.Text = StrOwnerAddress;
                        lblVehicleType.Text = StrVehicleType;
                        lblTransactionType.Text = StrTransactonType;
                        lblAuthDate.Text = StrAuthdate;
                        if (StrMobileNo.Trim() == "" || StrMobileNo.Trim() == null || StrMobileNo.Trim() == "NOMOBILENUMBER")
                        {
                            lblMobileNo.Text = "0";
                        }
                        else
                        {
                            lblMobileNo.Text = StrMobileNo;
                        }
                        if (StrRegistrationNo.ToString() == "NOREGNNUMBER")
                        {
                            StrRegistrationNo = "";
                        }
                        lblVehicleClassType.Text = StrVehicleClassType;
                        lblMfgName.Text = StrManufacturarName;
                        lblModelName.Text = StrModelName;
                        lblRegNo.Text = StrRegistrationNo;

                        lblEngineNo.Text = StrEngineNo;
                        lblChasisNo.Text = StrChasisNo;

                    }
                    string SQLString2 = string.Empty;
                    SQLString2 = "select dbo.hsrpplateamt ('9','" + StrVehicleType + "','" + StrVehicleClassType + "','" + StrTransactonType + "') as Amount";
                    DataTable dt1 = Utils.GetDataTable(SQLString2, ConnectionString);
                    lblAmount.Text = dt1.Rows[0]["Amount"].ToString();

                    if ((StrVehicleType == "MCV/HCV/TRAILERS") || (StrVehicleType == "LMV(CLASS)") || (StrVehicleType == "LMV") || (StrVehicleType == "THREE WHEELER"))
                    {
                        Sticker = "Y";
                    }
                    else
                    {
                        Sticker = "N";
                    }




                    VIP = "N";

                }
                catch (Exception ex)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "RTA Server is not responding... Pls contact to System Administrator  " + ex.ToString();
                }


           
        }

    }
}