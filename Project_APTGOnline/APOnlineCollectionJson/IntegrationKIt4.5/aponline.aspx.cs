
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace IntegrationKIt4._5
{

    public partial class aponline : System.Web.UI.Page
    {
        HSRPPayment hsrp = new HSRPPayment();

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
            HSRPStateID = "9";
            if (!IsPostBack)
            {


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
                string strAuthNo = lblAuthNo.Text;
                string stremail = string.Empty;
                string SQLString = string.Empty;
                string RTOlocationName = string.Empty;
                string StrDealerAddress = string.Empty;
                string StrDlrContactNo = string.Empty;
                string StrRetunrUrl = string.Empty;
                string StrDlearName=string.Empty;
                string ReferenceURL = string.Empty;
                string strdealerID = string.Empty;




                SQLString = "select Rtolocationname from rtolocation where rtolocationid='" + RTOLocationID + "'";
                RTOlocationName = Utils.getScalarValue(SQLString, ConnectionString);
                try
                {
                    const string SecurityKey = "2F3bc029F944bDFbe10d777C224D538e";

                    strAuthNo =  Request.Form["AuthNo"];
                    string Skey =  Request.Form["authkey"];
                    StrRetunrUrl = Request.Form["ReturnURL"];
                    StrDlearName =  Request.Form["DealerName"];
                    StrDealerAddress = Request.Form["DealerAddress"];
                    StrDlrContactNo =  Request.Form["DealerContactNo"];
                    ReferenceURL =  Request.Form["ReturnURL"];
                    strdealerID =  Request.Form["DealerId"];

                    if (strAuthNo.Trim() == "")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please provide Valid Info.";
                    }
                
                    if (!Skey.Equals(SecurityKey))
                    {
                        Response.Write("<h1 style='color:red'>Not a authorized call of page.</h1>");
                        Response.End();
                    }
                  

                    var request = (HttpWebRequest)WebRequest.Create("http://104.211.231.161/aptransprod/hsrp_payment.php");
                    var postData = "AuthNo=" +strAuthNo;
                    postData += "&authkey=" +Skey;
                    var data = Encoding.ASCII.GetBytes(postData);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    APOnline ObjAPOnline = JsonConvert.DeserializeObject<APOnline>(responseString);
                   

                    //Request.Form["txtDealerId"];
                    //Request.Form["txtDealerAddress"];
                    //Request.Form["txtDealerContactNo"];
                    //Request.Form["txtChassisNo"];




                    string strDate = System.DateTime.Now.ToString("hh:mm tt");
                    string strnine = "09:00 AM";
                    string strsix = "06:00 PM";
                    //if (DateTime.Parse(strDate) < DateTime.Parse(strnine) || DateTime.Parse(strDate) > DateTime.Parse(strsix))
                    //{
                    //    lblErrMess.Visible = true;
                    //    lblErrMess.Text = "";
                    //    lblErrMess.Text = "Cash Collection Timing is  Between 9 AM TO 6 PM ";
                    //    btnSave.Visible = false;
                    //    return;
                    //}
                   
                    //ServiceReference1.HSRPAuthorizationServiceSoapClient objAPService = new ServiceReference1.HSRPAuthorizationServiceSoapClient();
                  //  HSRP.HSRPAuthorizationServiceSoapClient objTgService = new HSRP.HSRPAuthorizationServiceSoapClient();
                    //string AuthData = objAPService.GetHSRPAuthorizationno(strAuthNo);
                    if (ObjAPOnline.ChassisNo == null)
                    {
                    //if (AuthData == "1")
                    //{
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Record not Found";
                        refresh();
                        return;
                    }
                    #region
                    //else if (AuthData.Length > 1)
                    //{
                        //using (StringReader stringReader = new StringReader(AuthData))
                        //using (XmlTextReader reader = new XmlTextReader(stringReader))
                        //{
                        //    while (reader.Read())
                        //    {

                        //        if (reader.Name.ToString() == "Rto_Code")
                        //        {
                        //            reader.Read();
                        //            StrRtoLocationCode = reader.Value.ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Rto_Name")
                        //        {
                        //            reader.Read();
                        //            StrRtoName = reader.Value.ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Authorization_Ref_no")
                        //        {
                        //            reader.Read();
                        //            strAuthno = reader.Value.ToString();
                        //            //StrAuthorizationNo = AuthArray[2].ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Owner_Name")
                        //        {
                        //            reader.Read();
                        //            StrOwnerName = reader.Value.ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Owner_Address")
                        //        {
                        //            reader.Read();
                        //            StrOwnerAddress = reader.Value.ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Vehicle_Type")
                        //        {
                        //            reader.Read();
                        //            string strquery = "select upper(ourValue) as ourValue from  [dbo].[Mapping_Vahan_HSRP_ap] where rtovalue ='" + reader.Value.ToString() + "'";
                        //            // Utils.getScalarValue( 
                        //            StrVehicleType = Utils.getDataSingleValue(strquery, ConnectionString, "ourValue");

                        //            // StrVehicleType = reader.Value.ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Trans_Type")
                        //        {
                        //            reader.Read();
                        //            StrTransactonType = reader.Value.ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Authorization_Date")
                        //        {
                        //            reader.Read();
                        //            StrAuthdate = reader.Value.ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Mobile_No")
                        //        {
                        //            reader.Read();
                        //            StrMobileNo = reader.Value.ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Owner_Email_Id")
                        //        {
                        //            reader.Read();
                        //            stremail = reader.Value.ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Veh_Class_Type")
                        //        {
                        //            reader.Read();
                        //            if (reader.Value.ToString() == "N")
                        //            {
                        //                StrVehicleClassType = "Non-Transport";
                        //            }
                        //            else if (reader.Value.ToString() == "T")
                        //            {
                        //                StrVehicleClassType = "Transport";
                        //            }
                        //            reader.Read();

                        //        }
                        //        if (reader.Name.ToString() == "MFRS_Name")
                        //        {
                        //            reader.Read();
                        //            StrManufacturarName = reader.Value.ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Model_Name")
                        //        {
                        //            reader.Read();
                        //            StrModelName = reader.Value.ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Reg_no")
                        //        {
                        //            reader.Read();
                        //            lblErrMess.Text = "";
                        //            StrRegistrationNo = reader.Value.ToString().Trim();
                        //            //if (StrRegistrationNo.Length > 10)
                        //            //{
                        //            //    lblErrMess.Visible = true;
                        //            //    lblErrMess.Text = "Record Not Found. Please contact your RTO.";
                        //            //    return;
                        //            //}

                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Engine_No")
                        //        {
                        //            reader.Read();
                        //            StrEngineNo = reader.Value.ToString();
                        //            reader.Read();
                        //        }
                        //        if (reader.Name.ToString() == "Chassis_No")
                        //        {
                        //            reader.Read();
                        //            StrChasisNo = reader.Value.ToString();
                        //            reader.Read();
                        //        }

                        //    }
                    //}
                    #endregion
                    lblAuthNo.Text = strAuthNo;
                        lblRTOLocationCode.Text = StrRtoLocationCode;
                        lblRTOName.Text = ObjAPOnline.RTOCodeName;// StrRtoName;
                        lblAffixName.Text = ObjAPOnline.AffixationCenterAddress;// StrRtoName;
                        txtEmail.Text = ObjAPOnline.OwnerEmail.Replace("'", "");
                        lblOwnerName.Text = ObjAPOnline.OwnerName.Replace(",", " ").Replace(".", " ").Replace("'", " ").Replace(":", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("!", " ").Replace("&", " ").Replace("/", " ").Replace("#", " ").ToString();// StrOwnerName;
                        lblAddress.Text = ObjAPOnline.Address.Replace(",", " ").Replace(".", " ").Replace("'", " ").Replace(":", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("!", " ").Replace("&", " ").Replace("/", " ").Replace("#", " ").ToString();// StrOwnerAddress;
                        lblCity.Text = ObjAPOnline.City;
                        lblPin.Text = ObjAPOnline.Pin;
                        lblstate.Text = ObjAPOnline.State;
                        lblVehicleType.Text = ObjAPOnline.VehicleType;// StrVehicleType;
                        lblTransactionType.Text = ObjAPOnline.TransactionType;// StrTransactonType;
                        lblAuthDate.Text = ObjAPOnline.AuthorizationDate.ToString(); //; StrAuthdate;
                        hdnDlrName.Value = StrDlearName;
                        hdnDlrContactNO.Value = StrDlrContactNo;
                        hdnReferenceURL.Value = StrRetunrUrl;
                        hdnDlrAddress.Value = StrDealerAddress.Replace(",", " ").Replace(".", " ").Replace("'", " ").Replace(":", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("!", " ").Replace("&", " ").Replace("/", " ").Replace("#", " ").ToString();
                        hdnDealerId.Value = strdealerID;


                        if (ObjAPOnline.MobileNo.ToString().Trim() == "" || ObjAPOnline.MobileNo.ToString().Trim() == null || ObjAPOnline.MobileNo.ToString().Trim() == "NOMOBILENUMBER")
                        {
                            lblMobileNo.Text = "0";
                        }
                        else
                        {
                            lblMobileNo.Text = ObjAPOnline.MobileNo.ToString().Trim();
                        }
                        if (ObjAPOnline.RegistrationNo == "NOREGNNUMBER")
                        {
                            ObjAPOnline.RegistrationNo = "";
                        }
                        lblVehicleClassType.Text = ObjAPOnline.VehicleClass;// StrVehicleClassType;
                        lblMfgName.Text = ObjAPOnline.ManufacturerName;// StrManufacturarName;
                        lblModelName.Text = ObjAPOnline.ModelName;// StrModelName;
                        lblRegNo.Text = ObjAPOnline.RegistrationNo;// StrRegistrationNo;

                        lblEngineNo.Text = ObjAPOnline.EngineNo;// StrEngineNo;
                        lblChasisNo.Text = ObjAPOnline.ChassisNo;// StrChasisNo;

                    
                    string SQLString2 = string.Empty;
                    SQLString2 = "select dbo.hsrpplateamt ('9','" +  ObjAPOnline.VehicleType + "','" + ObjAPOnline.VehicleClass + "','" + ObjAPOnline.TransactionType + "') as Amount";
                    DataTable dt1 = Utils.GetDataTable(SQLString2, ConnectionString);
                    lblAmount.Text = ObjAPOnline.Amount.ToString();

                    if ((ObjAPOnline.VehicleType == "MCV/HCV/TRAILERS") || (ObjAPOnline.VehicleType == "LMV(CLASS)") || (ObjAPOnline.VehicleType == "LMV") || (ObjAPOnline.VehicleType == "THREE WHEELER"))
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
                    lblErrMess.Text = "Valid Info Missing. Check data and Re-post.";
                }


            }
            else
            {

            }
        }



        public void refresh()
        {
            lblAuthNo.Text = "";
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
            lblAffixName.Text = "";
            lblCity.Text = "";
            lblPin.Text = "";
            lblstate.Text = "";
        }


        string Query = string.Empty;
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                //string ToDay = System.DateTime.Today.DayOfWeek.ToString();
                //if (ToDay == "Sunday")
                //{
                //    lblErrMess.Visible = true;
                //    lblErrMess.Text = "No Collection Allowed On Sunday";

                //    return;
                //}

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
                string strquery = "select upper(ourValue) as ourValue from  [dbo].[Mapping_Vahan_HSRP_ap] where rtovalue ='" + lblVehicleType.Text + "'";
                // Utils.getScalarValue( 
               string StrVehicleType = Utils.getDataSingleValue(strquery, ConnectionString, "ourValue");
               string sticker1 = string.Empty;
               if ((StrVehicleType == "MCV/HCV/TRAILERS") || (StrVehicleType == "LMV(CLASS)") || (StrVehicleType == "LMV") || (StrVehicleType == "THREE WHEELER"))
               {
                   sticker1 = "Y";
               }
               else
               {
                   sticker1 = "N";
               }

                VIP = "N";
               

                string sql = "exec [getPlatesData] '9','" + StrVehicleType + "','" + lblVehicleClassType.Text + "', '" + lblTransactionType.Text + "'";
                DataTable dt = Utils.GetDataTable(sql, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    lblSucMess.Visible = false;
                }
                string AuthNo = lblAuthNo.Text.Trim();

                Query = "select count(*) from hsrprecords where HSRP_StateID=9   and HSRPRecord_AuthorizationNo ='" + AuthNo + "'";
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

                  //  string OrderNo = "AP" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string OrderNo = AuthNo.Replace("/", "").ToString() + DateTime.Now.ToString("ddHHmmss"); 
                   
                    //string strQuery = "insert into APOnlinePayment (RTOName,ISFrontPlateSize,ISRearPlateSize, address1,City,State,Pin,manufacturername," +
                    //    " HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo," +
                    //    "VehicleClass,OrderType,NetAmount,VehicleType,OrderStatus,CashReceiptNo,OwnerFatherName,EmailID,ChassisNo, EngineNo," +
                    //    "frontplatesize, rearplatesize,CreatedBy,ManufacturerModel,StickerPrize,ScrewPrize," +
                    //    "RoundOff_NetAmount,VAT_Percentage,affix_id,PlateAffixationDate,OnlinePaymentStatus,OnlinePaymentID,ReferenceUrl,DealerContactNo,DealerAddress,DealerName,dealerid,AffixationCenterAddress,DeliveryChallanNo,invoiceno,isVIP,RearPlatePrize,remarks,SaveMacAddress,StickerMandatory,TotalAmount,VAT_Amount,vehicleref,FrontplatePrize) " +
                    //    "values('" + lblRTOName.Text + "','" + dt.Rows[0]["frontplateflag"].ToString() + "','" + dt.Rows[0]["rearplateflag"].ToString() +
                    //    "','" + lblAddress.Text + "','" + lblCity.Text + "','" + lblstate.Text + "','" + lblPin.Text + "','" + lblMfgName.Text + "', GetDate(),'" + HSRPStateID + "','" + RTOLocationID + "','" + AuthNo + "','" +
                    //    authdate + "','" + lblRegNo.Text.Trim().ToUpper() + "','" + lblOwnerName.Text + "','" + lblMobileNo.Text + "','" + lblVehicleClassType.Text +
                    //    "','" + lblTransactionType.Text + "','" + lblAmount.Text + "','" + StrVehicleType + "','New Order','" + OrderNo +
                    //    "',    '','" + strEmail + "', '" + lblChasisNo.Text.Trim() + "', '" + lblEngineNo.Text.Trim() + "', '" +
                    //    dt.Rows[0]["frontplateID"].ToString() + "', '" + dt.Rows[0]["RearPlateID"].ToString() + "','" + USERID + "','" + lblModelName.Text +
                    //    "','" + dt.Rows[0]["stickercost"].ToString() +
                    //    "','" + dt.Rows[0]["snaplockcost"].ToString() + "','" + Math.Round(decimal.Parse(lblAmount.Text), 0) + "','" + dt.Rows[0]["vatper"].ToString() + "','1','" + date1 + "','N','" + OrderNo + "','" + hdnReferenceURL.Value + "','" + hdnDlrContactNO.Value + "','" + hdnDlrAddress.Value + "','" + hdnDlrName.Value + "','" + hdnDealerId.Value + "','" + lblAffixName.Text + "','','','" + VIP + "','" + dt.Rows[0]["rearplatecost"].ToString() + "','','DEALERMACADD','" + sticker1 + "','" + dt.Rows[0]["totalamount"].ToString() + "','" + dt.Rows[0]["vatamount"].ToString() + "','','" + dt.Rows[0]["FrontPlateCost"].ToString() + "')";
                    //int i = Utils.ExecNonQuery(strQuery, ConnectionString);
                    //if (i > 0)
                    //{

                    //    lblSucMess.Visible = true;
                    //    lblErrMess.Visible = false;
                    //    Response.Redirect("aponline2.aspx?X=" + HttpContext.Current.Server.HtmlEncode(AuthNo.ToString()) + "&orderNo=" + HttpContext.Current.Server.HtmlEncode(OrderNo));

                    //}

                    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
                    SqlCommand com = new SqlCommand("Insert_APOnlinePayment", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@RTOName", lblRTOName.Text);
                    com.Parameters.AddWithValue("@SaveMacAddress", macbase);
                    com.Parameters.AddWithValue("@ISFrontPlateSize", dt.Rows[0]["frontplateflag"].ToString());
                    com.Parameters.AddWithValue("@ISRearPlateSize", dt.Rows[0]["rearplateflag"].ToString());
                    com.Parameters.AddWithValue("@address1", lblAddress.Text);
                    com.Parameters.AddWithValue("@manufacturername", lblMfgName.Text);
                    com.Parameters.AddWithValue("@HSRP_StateID", HSRPStateID);
                    com.Parameters.AddWithValue("@RTOLocationID", RTOLocationID);
                    com.Parameters.AddWithValue("@HSRPRecord_AuthorizationNo", AuthNo);
                    com.Parameters.AddWithValue("@HSRPRecord_AuthorizationDate", authdate);
                    com.Parameters.AddWithValue("@VehicleRegNo", lblRegNo.Text.Trim().ToUpper());
                    com.Parameters.AddWithValue("@OwnerName", lblOwnerName.Text);
                    com.Parameters.AddWithValue("@MobileNo", lblMobileNo.Text);
                    com.Parameters.AddWithValue("@VehicleClass", lblVehicleClassType.Text);
                    com.Parameters.AddWithValue("@OrderType", "NB");
                    com.Parameters.AddWithValue("@StickerMandatory", sticker1);
                    com.Parameters.AddWithValue("@isVIP", VIP);
                    com.Parameters.AddWithValue("@NetAmount", lblAmount.Text);
                    com.Parameters.AddWithValue("@VehicleType", StrVehicleType);
                    com.Parameters.AddWithValue("@OrderStatus", "New Order");
                    com.Parameters.AddWithValue("@CashReceiptNo", "");
                    com.Parameters.AddWithValue("@EmailID", strEmail);
                    com.Parameters.AddWithValue("@ChassisNo", lblChasisNo.Text.Trim());
                    com.Parameters.AddWithValue("@EngineNo", lblEngineNo.Text.Trim());
                    com.Parameters.AddWithValue("@frontplatesize", dt.Rows[0]["frontplateID"].ToString());
                    com.Parameters.AddWithValue("@rearplatesize", dt.Rows[0]["RearPlateID"].ToString());
                    com.Parameters.AddWithValue("@CreatedBy", USERID);
                    com.Parameters.AddWithValue("@ManufacturerModel", lblModelName.Text);
                    com.Parameters.AddWithValue("@vehicleref", "New");
                    com.Parameters.AddWithValue("@FrontplatePrize", dt.Rows[0]["FrontPlateCost"].ToString());
                    com.Parameters.AddWithValue("@RearPlatePrize", dt.Rows[0]["rearplatecost"].ToString());
                    com.Parameters.AddWithValue("@StickerPrize", dt.Rows[0]["stickercost"].ToString());
                    com.Parameters.AddWithValue("@ScrewPrize", dt.Rows[0]["snaplockcost"].ToString());
                    com.Parameters.AddWithValue("@TotalAmount", dt.Rows[0]["totalamount"].ToString());
                    com.Parameters.AddWithValue("@VAT_Amount", dt.Rows[0]["vatamount"].ToString());
                    com.Parameters.AddWithValue("@RoundOff_NetAmount", Math.Round(decimal.Parse(lblAmount.Text), 0));
                    com.Parameters.AddWithValue("@VAT_Percentage", dt.Rows[0]["vatper"].ToString());
                    com.Parameters.AddWithValue("@affix_id", Convert.ToInt32(1));
                    com.Parameters.AddWithValue("@PlateAffixationDate", date1);
                    com.Parameters.AddWithValue("@OnlinePaymentStatus", "N");
                    com.Parameters.AddWithValue("@OnlinePaymentID", OrderNo);
                    //com.Parameters.AddWithValue("@OnlinePaymentID", "HSRPTS003521812016HS07125309");                    
                    com.Parameters.AddWithValue("@ReferenceUrl", hdnReferenceURL.Value);
                    com.Parameters.AddWithValue("@DealerContactNo", hdnDlrContactNO.Value);
                    com.Parameters.AddWithValue("@DealerAddress", hdnDlrAddress.Value);
                    com.Parameters.AddWithValue("@DealerName", hdnDlrName.Value);
                    com.Parameters.AddWithValue("@dealerid", hdnDealerId.Value);
                    com.Parameters.AddWithValue("@AffixationCenterCode", lblRTOLocationCode.Text);
                    com.Parameters.AddWithValue("@AffixationCenterAddress", lblAffixName.Text);
                    SqlParameter objSQLParm = new SqlParameter("@Result", SqlDbType.VarChar, 100);
                    objSQLParm.Direction = ParameterDirection.Output;
                    com.Parameters.Add(objSQLParm);
                    con.Open();
                    int i = com.ExecuteNonQuery();
                    con.Close();
                    if (i == 2)
                    {
                        lblSucMess.Visible = false;
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please retry.";
                        return;
                    }
                    else if (i == 1)
                    {
                        lblSucMess.Visible = true;
                        lblErrMess.Visible = false;
                        Response.Redirect("aponline2.aspx?X=" + HttpContext.Current.Server.UrlEncode(AuthNo.ToString()) + "&orderNo=" + HttpContext.Current.Server.UrlEncode(OrderNo));
                    }
                    else
                    {
                        lblSucMess.Visible = false;
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please retry.";

                    }

                }
            }
            catch (Exception ex)
            {
                lblErrMess.Text = "Message : " + ex.Message.ToString();
                return;
            }
        }







    }
}