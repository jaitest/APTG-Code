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
            string pathx = "C:\\OnlinePaymentLog\\AP-" + mode + "-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
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
                    string strIsKey = "9865645077ADJSEO";
                    string strIsIv = "8566652421NCGISC";
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
                            AddLog("***Payment Recieved***-" + orderNo.Text + "==" + System.DateTime.Now.ToString(), "ICICI");
                            AddLog(strDecryptedVal, "ICICI");
                            // lblValidate.Text = "Transaction Success  " + strPG_TxnStatus;
                            string CnnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();


                            string AuthNoSQL = "select distinct mobileno,HSRPRecord_AuthorizationNo,ChassisNo,DealerName,dealerid,DealerAddress,DealerContactNo,TotalAmount,ReferenceUrl,Netamount from APOnlinePayment where [OnlinePaymentID]='" + orderNo.Text + "'";
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
                                    hdnPhone.Value = dt.Rows[i]["mobileno"].ToString();
                                }
                            }
                            else
                            {
                                lblErrMess.Text = "No record found";

                            }

                            int returnvalX = Utils.ExecNonQuery("update APOnlinePayment set OnlinePaymentStatus='Y' ,PaymentGateway='ICICI', BankRefNo='" + lblBankRefNo.Text + "' where [OnlinePaymentID]='" + orderNo.Text + "'", CnnString);

                            //execute query to move record from APOnlinePayment to HSRPRecords
                            Utils.ExecNonQuery("Exec [GetAPOnlinePayment] '" + orderNo.Text + "'", CnnString);
                            AddLog("***Order Booked***-execute Response code" + returnvalX.ToString() + "-" + System.DateTime.Now.ToString(), "ICICI");

                            if (hdnPhone.Value.Length > 0)
                            {
                                string SMSText = " Amount Rs." + HSRPAmnt.Value + " Received against HSRP Auth No. " + AuthNo.Value + " Order No. " + orderNo.Text + " dated " + System.DateTime.Now.ToString("dd/MM/yyyy") + ",Including Fitting Charges-HSRP Team ";
                                string sendURL = "http://103.16.101.52:8080/bulksms/bulksms?username=sse-aphsrp&password=aphsrp&type=0&dlr=1&destination=" + hdnPhone.Value.ToString() + "&source=APHSRP&message=" + SMSText;
                                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                                myRequest.Method = "GET";
                                WebResponse myResponse = myRequest.GetResponse();
                                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                                string result = sr.ReadToEnd();
                                sr.Close();
                                myResponse.Close();
                                System.Threading.Thread.Sleep(350);

                                Utils.ExecNonQuery("insert into APSMSDetail(AuthorizationNumber,MobileNo,SentResponseCode,smstext) values('" + AuthNo.Value + "','" + hdnPhone.Value.ToString() + "','" + result + "','" + SMSText + "')", CnnString);

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


                            string AuthNoSQL = "select distinct HSRPRecord_AuthorizationNo,ChassisNo,DealerName,dealerid,DealerAddress,DealerContactNo,TotalAmount,ReferenceUrl,Netamount from APOnlinePayment where [OnlinePaymentID]='" + orderNo.Text + "'";
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
