using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;
using System.Net;

namespace IntegrationKIt4._5
{
    public partial class axisresponse : System.Web.UI.Page
    {
        
        string stingHSRPAmnt = string.Empty;
        string ReturnUrl = string.Empty;

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }
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
            string orderStatus = string.Empty;
            try
            {
                string CnnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                orderStatus = Request.QueryString["PAID"].ToString();
                orderNo.Text = Request.Form["PRN"].ToString();
                PymntOrderNo.Value = Request.Form["PRN"].ToString();

                string AuthNoSQL = "select distinct Mobileno,HSRPRecord_AuthorizationNo,ChassisNo,DealerName,dealerid,DealerAddress,DealerContactNo,TotalAmount,ReferenceUrl,Netamount from APOnlinePayment where [OnlinePaymentID]='" + Request.Form["PRN"].ToString() + "'";
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
                        lblTelephone.Text = dt.Rows[i]["Mobileno"].ToString();
                    }
                }
                else
                {
                    lblErrMess.Text = "No record found";

                }

                lblOrderStatus.Text = Request.QueryString["PAID"].ToString();
                lblOrderStatusMsg.Text = Request.QueryString["PAID"].ToString();
                lbltrackingid.Text = Request.Form["BID"].ToString();
                lblAmount.Text = Request.Form["AMT"].ToString();
                lblBankRefNo.Text = Request.Form["BID"].ToString();
                //foreach (string s in Request.Form.Keys)
                //{


                //    Response.Write((s.ToString() + ":" + Request.Form[s] + ""));


                //}
                //Response.End();
                if (orderStatus == "Y")
                {
                    AddLog("***Order No***-" + Request.Form["PRN"].ToString() + "==" + System.DateTime.Now.ToString(), "AXIS");

                    Utils.ExecNonQuery("update APOnlinePayment set OnlinePaymentStatus='Y' ,PaymentGateway='AxisBank', BankRefNo='" + lblBankRefNo.Text + "' where [OnlinePaymentID]='" + Request.Form["PRN"].ToString() + "'", CnnString);

                    //execute query to move record from APOnlinePayment to HSRPRecords
                    Utils.ExecNonQuery("Exec [GetAPOnlinePayment] '" + Request.Form["PRN"].ToString() + "'", CnnString);
                    AddLog("***Order Booked***-" + System.DateTime.Now.ToString(), "AXIS");
                    if (lblTelephone.Text.Length > 0)//will change 
                    {

                        string SMSText = " Amount Rs." + HSRPAmnt.Value + " Received against HSRP Auth No. " + AuthNo.Value + " Order No. " + orderNo.Text + " dated " + System.DateTime.Now.ToString("dd/MM/yyyy") + ",Including Fitting Charges-HSRP Team ";
                        string sendURL = "http://103.16.101.52:8080/bulksms/bulksms?username=sse-aphsrp&password=aphsrp&type=0&dlr=1&destination=" + lblTelephone.Text.ToString() + "&source=APHSRP&message=" + SMSText;
                        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                        myRequest.Method = "GET";
                        WebResponse myResponse = myRequest.GetResponse();
                        StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                        string result = sr.ReadToEnd();
                        sr.Close();
                        myResponse.Close();
                        System.Threading.Thread.Sleep(350);

                        Utils.ExecNonQuery("insert into APSMSDetail(AuthorizationNumber,MobileNo,SentResponseCode,smstext) values('" + AuthNo.Value + "'," + lblTelephone.Text.ToString() + ",'" + result + "','" + SMSText + "')", CnnString);
                    }

                    AddLog("***SMSSent***-" + System.DateTime.Now.ToString(), "AXIS");
                    PymntFlag.Value = "Yes";
                    PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                    HSRPAmnt.Value = HSRPAmnt.Value;
                    stingHSRPAmnt = HSRPAmnt.Value;
                    PymntErrorMsg.Value = string.Empty;
                    form1.Action = ReturnUrl;
                    AddLog("***Control Sent To Govt Server ***-" + System.DateTime.Now.ToString(), "AXIS");
               

                }
                else if (orderStatus == "N")
                {
                    AddLog("***False Response From Axis Server ***-" + System.DateTime.Now.ToString(), "AXIS");

                    lblErrMess.Text = "Your online transaction has been declined.";
                    PymntFlag.Value = "No";
                    PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                    stingHSRPAmnt = HSRPAmnt.Value;
                    HSRPAmnt.Value = HSRPAmnt.Value;
                    PymntErrorMsg.Value = "Your online transaction has been declined.";
                    form1.Action = ReturnUrl;
                }
            }
            catch (Exception ex)
            {
                AddLog("***Error Exception ***-" + ex.Message.ToString(), "AXIS");
                
                lblErrMess.Text = "Error While Connecting RTA Server Contact administrator:" + ex.Message.ToString();
                PymntFlag.Value = "No";
                PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                stingHSRPAmnt = HSRPAmnt.Value;
                HSRPAmnt.Value = HSRPAmnt.Value;
                PymntErrorMsg.Value = "Error While Connecting RTA Server Contact administrator:" + ex.Message.ToString();
                form1.Action = ReturnUrl;
            }
         }
        public void failureSMS(string connectionString, string mobileNo, string paymentId)
        {
             Utils.ExecNonQuery("update APOnlinePayment set OnlinePaymentStatus='N' where [OnlinePaymentID]='" + paymentId + "'", connectionString);
        }
    }
}