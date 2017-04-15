using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetIntegrationKit;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
namespace IntegrationKIt4._5
{
    public partial class tpslresponse : System.Web.UI.Page
    {
        #region Variable Declaration
        string stingHSRPAmnt = string.Empty;
        string ReturnUrl = string.Empty;
        string orderStatus = string.Empty;

        string strHEX, strPGActualReponseWithChecksum, strPGActualReponseEncrypted, strPGActualReponseDecrypted, strPGresponseChecksum, strPGTxnStatusCode;
        string[] strPGChecksum, strPGTxnString;
        bool isDecryptable = false;
        string strPG_TxnStatus = string.Empty,
        strPG_ClintTxnRefNo = string.Empty,
        strPG_TPSLTxnBankCode = string.Empty,
        strPG_TPSLTxnID = string.Empty,
        strPG_TxnAmount = string.Empty,
        strPG_TxnDateTime = string.Empty,
        strPG_TxnDate = string.Empty,
        strPG_TxnTime = string.Empty,
        strPG_MerchantCode = string.Empty
        ;
        string strPGResponse;
        string[] strSplitDecryptedResponse;
        string[] strArrPG_TxnDateTime;
        #endregion
        static void AddLog(string logtext, string mode)
        {
            string pathx = "C:\\OnlinePaymentLog\\TG-" + mode + "-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
            using (StreamWriter sw = File.AppendText(pathx))
            {
                // sw.WriteLine("-------------------" + System.DateTime.Now.ToString() + "--------------------");
                sw.WriteLine(logtext);
                // sw.WriteLine("-----------------------------------------------------------------------------");
                sw.Close();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                strPGResponse = Convert.ToString(HttpContext.Current.Request["msg"]);
                strPG_MerchantCode = Convert.ToString(HttpContext.Current.Request["tpsl_mrct_cd"]);

                if (strPGResponse != "" || strPGResponse != null)
                {
                    LBL_DisplayResult.Text = "Response :: " + strPGResponse;
                    //Creating Object of Class DotNetIntegration_1_1.RequestURL
                    RequestURL objRequestURL = new RequestURL();


                    //Decrypting the PG response
                    string strIsKey = "3480176807VSWRXM";
                    string strIsIv = "2490077168CANYWH";
                    //Verify Response using Key and Iv
                    string strDecryptedVal = objRequestURL.VerifyPGResponse(strPGResponse, strIsKey, strIsIv);

                    // lblResponseDecrypted.Text=strDecryptedVal;
                    if (strDecryptedVal.StartsWith("ERROR"))
                    {
                        lblValidate.Text = strDecryptedVal;
                    }
                    else
                    {
                        strSplitDecryptedResponse = strDecryptedVal.Split('|');
                        GetPGRespnseData(strSplitDecryptedResponse);

                        if (strPG_TxnStatus == "0300")
                        {
                            AddLog("***Payment Recieved***-"+orderNo.Text + "==" + System.DateTime.Now.ToString(), "ICICI");
                            AddLog(strDecryptedVal, "ICICI");
                            // lblValidate.Text = "Transaction Success  " + strPG_TxnStatus;
                            string CnnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();


                            string AuthNoSQL = "select distinct paymentmobile,mobileno,HSRPRecord_AuthorizationNo,ChassisNo,DealerName,dealerid,DealerAddress,DealerContactNo,TotalAmount,ReferenceUrl,Netamount from TGOnlinePayment where [OnlinePaymentID]='" + orderNo.Text + "'";
                            DataTable dt = new DataTable();
                            dt = Utils.GetDataTable(AuthNoSQL, CnnString);
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {

                                    ChassisNo.Value = dt.Rows[i]["ChassisNo"].ToString();
                                    DealerName.Value = dt.Rows[i]["DealerName"].ToString();
                                    DealerId.Value = dt.Rows[i]["dealerid"].ToString();
                                    DealerAddress.Value = dt.Rows[i]["DealerAddress"].ToString();
                                    DealerContactNo.Value = dt.Rows[i]["DealerContactNo"].ToString();
                                    ReturnUrl = dt.Rows[i]["ReferenceUrl"].ToString();
                                    AuthNo.Value = dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString();
                                    HSRPAmnt.Value = dt.Rows[i]["Netamount"].ToString();
                                    lblTelephone.Text = dt.Rows[i]["paymentmobile"].ToString();
                                }
                            }
                            else
                            {
                                lblErrMess.Text = "No record found";

                            }

                            int returnvalX = Utils.ExecNonQuery("update TGOnlinePayment set OnlinePaymentStatus='Y' ,PaymentGateway='ICICI',OnlinePaymentTrackingNo='" + lbltrackingid.Text + "', BankRefNo='" + lblBankRefNo.Text + "' where [OnlinePaymentID]='" + orderNo.Text + "'", CnnString);
                           
                            //execute query to move record from TGOnlinePayment to HSRPRecords
                           returnvalX= Utils.ExecNonQuery("Exec [GetTGOnlinePayment] '" + orderNo.Text + "'", CnnString);
                           AddLog("***Order Booked***-execute Response code" + returnvalX.ToString() + "-" + System.DateTime.Now.ToString(), "ICICI");

                            IntegrationKIt4._5.HSRP.HSRPAuthorizationServiceSoapClient objTgService = new IntegrationKIt4._5.HSRP.HSRPAuthorizationServiceSoapClient();

                            string serverdate = System.DateTime.Now.ToString("dd/MM/yyyy");
                            string AuthData = objTgService.UpdateHSRPCharges(AuthNo.Value, Math.Round(Convert.ToDecimal(HSRPAmnt.Value), 0), serverdate);
                            AddLog("***Synced To Govt***-" + System.DateTime.Now.ToString(), "ICICI");
                            AddLog("***Before SMS Text***-" + System.DateTime.Now.ToString(), "ICICI");
                            string SMSText = " Online Payment Rs." + HSRPAmnt.Value + " received against HSRP Authorization No. " + AuthNo.Value + " on " + System.DateTime.Now.ToString("dd/MM/yyyy") + " Online order number " + orderNo.Text + ". HSRP Team.";
                            AddLog("***After SMS Text***-" + SMSText.ToString() + System.DateTime.Now.ToString(), "ICICI");
                            if (lblTelephone.Text.Length == 10)
                            {

                                string sendURL = "http://quick.smseasy.in:8080/bulksms/bulksms?username=sse-tlhsrp1&password=tlhsrp1&type=0&dlr=1&destination=" + lblTelephone.Text + "&source=TSHSRP&message=" + SMSText.ToString();
                                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                                myRequest.Method = "GET";
                                WebResponse myResponse = myRequest.GetResponse();
                                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                                string result = sr.ReadToEnd();
                                sr.Close();
                                myResponse.Close();
                                System.Threading.Thread.Sleep(350);

                                AddLog("***SMS Sent ***-" + System.DateTime.Now.ToString(), "AXIS");
                                Utils.ExecNonQuery("insert into TSSMSDetail(AuthorizationNumber,MobileNo,SentResponseCode,smstext) values('" + AuthNo.Value + "','" + lblTelephone.Text.ToString() + "','" + result + "','" + SMSText.ToString() + "')", CnnString);
                            }
                            else
                            {
                                AddLog("***SMS Not Sent ***-" + System.DateTime.Now.ToString(), "AXIS");
                                Utils.ExecNonQuery("insert into TSSMSDetail(AuthorizationNumber,MobileNo,SentResponseCode,smstext) values('" + AuthNo.Value + "','" + lblTelephone.Text.ToString() + "','Not Sent','" + SMSText.ToString() + "')", CnnString);
                           
                            }

                            
                            PymntFlag.Value = "Yes";
                            PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                            HSRPAmnt.Value = HSRPAmnt.Value;
                            stingHSRPAmnt = HSRPAmnt.Value;
                            PymntErrorMsg.Value = string.Empty;
                            form1.Action = ReturnUrl;
                            AddLog("***Control Sent To Govt Server ***-" + System.DateTime.Now.ToString(), "ICICI");
                
                        }
                        else
                        {
                            
                            string CnnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                            AddLog("***False Response From Server TxStatus :" + orderNo.Text + "==" + strPG_TxnStatus + " ***-" + System.DateTime.Now.ToString(), "ICICI");

                            string AuthNoSQL = "select distinct HSRPRecord_AuthorizationNo,ChassisNo,DealerName,dealerid,DealerAddress,DealerContactNo,TotalAmount,ReferenceUrl,Netamount from TGOnlinePayment where [OnlinePaymentID]='" + orderNo.Text + "'";
                            DataTable dt = new DataTable();
                            dt = Utils.GetDataTable(AuthNoSQL, CnnString);
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {

                                    ChassisNo.Value = dt.Rows[i]["ChassisNo"].ToString();
                                    DealerName.Value = dt.Rows[i]["DealerName"].ToString();
                                    DealerId.Value = dt.Rows[i]["dealerid"].ToString();
                                    DealerAddress.Value = dt.Rows[i]["DealerAddress"].ToString();
                                    DealerContactNo.Value = dt.Rows[i]["DealerContactNo"].ToString();
                                    ReturnUrl = dt.Rows[i]["ReferenceUrl"].ToString();
                                    AuthNo.Value = dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString();
                                    HSRPAmnt.Value = dt.Rows[i]["Netamount"].ToString();
                                }
                            }
                            else
                            {
                                lblErrMess.Text = "No record found";

                            }
                            lblErrMess.Text = "Your online transaction has been declined.";
                            PymntFlag.Value = "No";
                            PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                            stingHSRPAmnt = HSRPAmnt.Value;
                            HSRPAmnt.Value = HSRPAmnt.Value;
                            PymntErrorMsg.Value = "Your online transaction has been declined.";
                            form1.Action = ReturnUrl;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                AddLog("***ICICI Exception Error***-" + System.DateTime.Now.ToString(), "ICICI");
                AddLog(ex.Message.ToString(), "ICICI");
                AddLog("==================================", "ICICI");

            }
        }
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }
        public void GetPGRespnseData(string[] parameters)
        {
            string[] strGetMerchantParamForCompare;
            for (int i = 0; i < parameters.Length; i++)
            {
                strGetMerchantParamForCompare = parameters[i].ToString().Split('=');
                if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_STATUS")
                {
                    strPG_TxnStatus = Convert.ToString(strGetMerchantParamForCompare[1]);
                    lblOrderStatus.Text = strPG_TxnStatus;
                    lblOrderStatusMsg.Text = strPG_TxnStatus;
                }
                else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_TXN_REF")
                {
                    strPG_ClintTxnRefNo = Convert.ToString(strGetMerchantParamForCompare[1]);
                    orderNo.Text = strPG_ClintTxnRefNo;
                    PymntOrderNo.Value = strPG_ClintTxnRefNo;
                   
                }
                else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_BANK_CD")
                {
                    strPG_TPSLTxnBankCode = Convert.ToString(strGetMerchantParamForCompare[1]);
                }
                else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_ID")
                {
                    strPG_TPSLTxnID = Convert.ToString(strGetMerchantParamForCompare[1]);
                    lbltrackingid.Text = strPG_TPSLTxnID;
                    lblBankRefNo.Text = strPG_TPSLTxnID;
                }
                else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_AMT")
                {
                    strPG_TxnAmount = Convert.ToString(strGetMerchantParamForCompare[1]);
                    lblAmount.Text = strPG_TxnAmount;
                }
                else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_TIME")
                {
                    strPG_TxnDateTime = Convert.ToString(strGetMerchantParamForCompare[1]);
                    strArrPG_TxnDateTime = strPG_TxnDateTime.Split(' ');
                    strPG_TxnDate = Convert.ToString(strArrPG_TxnDateTime[0]);
                    strPG_TxnTime = Convert.ToString(strArrPG_TxnDateTime[1]);
                }
            }
        }
    }
}