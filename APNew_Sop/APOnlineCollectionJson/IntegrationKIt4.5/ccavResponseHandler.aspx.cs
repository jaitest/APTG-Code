using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using CCA.Util;
using System.Configuration;
using System.Net;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

public partial class ResponseHandler : System.Web.UI.Page
{
    //string ChassisNo = string.Empty;
    //string DealerName = string.Empty;
    //string DealerId = string.Empty;
    //string DealerAddress = string.Empty;
    //string DealerContactNo = string.Empty;
    string stingHSRPAmnt = string.Empty;

    //string PymentFlag = string.Empty;
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
        try
        {
            string CnnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            string workingKey = "A32AD88CFF2FB6B00CD025B97679F12F";
            string orderStatus = "";
            CCACrypto ccaCrypto = new CCACrypto();
            string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"], workingKey);
            NameValueCollection Params = new NameValueCollection();
            string[] segments = encResponse.Split('&');
            foreach (string seg in segments)
            {
                string[] parts = seg.Split('=');
                if (parts.Length > 0)
                {
                    if (parts[0].Trim() == "order_status")
                    {
                        orderStatus = parts[1].Trim();
                    }
                    string Key = parts[0].Trim();
                    string Value = parts[1].Trim();
                    Params.Add(Key, Value);
                }
            }

            //for (int i = 0; i < Params.Count; i++)
            //{
            //     Response.Write(Params.Keys[i] + " = " + Params[i] + "<br>");
            //}
            // Response.End();

            // insert all CCAvenue Response Records

            orderNo.Text = Params[0].ToString();
            PymntOrderNo.Value = Params[0].ToString();
            AddLog("***Order No***-" + orderNo.Text + "==" + System.DateTime.Now.ToString(), "CCAvenue");

            //string AuthNoSQL = "select distinct HSRPRecord_AuthorizationNo,ChassisNo,DealerName,dealerid,DealerAddress,DealerContactNo,TotalAmount,ReferenceUrl from TGOnlinePayment 
            string AuthNoSQL = "select distinct HSRPRecord_AuthorizationNo,ChassisNo,DealerName,dealerid,DealerAddress,DealerContactNo,TotalAmount,ReferenceUrl,Netamount from APOnlinePayment where [OnlinePaymentID]='" + Params[0].ToString() + "'";
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

            lblOrderStatus.Text = orderStatus;
            lblOrderStatusMsg.Text = Params[3].ToString();
            lbltrackingid.Text = Params[1].ToString();
            // lblAmount.Text = Params[10].ToString(); //Charged Amount
            lblAmount.Text = Params[35].ToString();
            lblOwnerName.Text = Params[11].ToString();
            lblAddress.Text = Params[12].ToString();
            lblCity.Text = Params[13].ToString();
            lblPin.Text = Params[15].ToString();
            lblState.Text = Params[14].ToString();
            lblBankRefNo.Text = Params[2].ToString();
            lblAffixation.Text = Params[26].ToString();
            lblTelephone.Text = Params[17].ToString();
            lblEmailID.Text = Params[18].ToString();
            //        0order_id = TS20151130170955065
            //1tracking_id = 104020651696
            //2bank_ref_no = 025263
            //3order_status = Success
            //4failure_message = 
            //5payment_mode = Debit Card
            //6card_name = Visa Debit Card
            //7status_code = 0
            //8status_message = Transaction Successful
            //9currency = INR
            //10amount = 250.61
            //11billing_name = RAVINDAR MERAVATH
            //12billing_address = H NO104 NAYAKUNI THANDA, NALGONDA
            //13billing_city = NALGONDA
            //14billing_state = TS
            //15billing_zip = 201301
            //16billing_country = India
            //17billing_tel = 9810509118
            //18billing_email = amitbhargavain@gmail.com
            if (orderStatus == "Success")
            {

                Utils.ExecNonQuery("update APOnlinePayment set OnlinePaymentStatus='Y',PaymentGateway='CCAvenue', BankRefNo='" + lblBankRefNo.Text + "',OnlinePaymentTrackingNo='" + lbltrackingid.Text + "' where [OnlinePaymentID]='" + Params[0].ToString() + "'", CnnString);

                //execute query to move record from APOnlinePayment to HSRPRecords
                Utils.ExecNonQuery("Exec [GetAPOnlinePayment] '" + Params[0].ToString() + "'", CnnString);
                AddLog("***Order Booked***-" + System.DateTime.Now.ToString(), "CCAvenue");
                if (lblTelephone.Text.Length > 0)//will change 
                {

                    string SMSText = " Amount Rs." + HSRPAmnt.Value + " Received against HSRP Auth No. " + AuthNo.Value + " Order No. " + Params[0].ToString() + "dated  " + System.DateTime.Now.ToString("dd/MM/yyyy") + ",HSRP Team ";
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
                AddLog("***Synced To Govt***-" + System.DateTime.Now.ToString(), "CCAvenue");

            }


            else if (orderStatus == "Failure")
            {
                AddLog("***Order Status ***-" + orderStatus, "CCAvenue");

                lblErrMess.Text = "Your online transaction has been declined.";
                PymntFlag.Value = "No";
                PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                stingHSRPAmnt = HSRPAmnt.Value;
                HSRPAmnt.Value = HSRPAmnt.Value;
                PymntErrorMsg.Value = "Your online transaction has been declined.";
                form1.Action = ReturnUrl;

            }
            else if (orderStatus == "Aborted")
            {
                AddLog("***Order Status ***-" + orderStatus, "CCAvenue");

                lblErrMess.Text = " Your Order Was Aborted by Payment Gateway Server.";
                PymntFlag.Value = "No";
                PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                stingHSRPAmnt = HSRPAmnt.Value;
                HSRPAmnt.Value = HSRPAmnt.Value;
                PymntErrorMsg.Value = "Your Order Was Aborted by Payment Gateway Server.";
                form1.Action = ReturnUrl;
            }
        }
        catch (Exception ex)
        {
            AddLog("***Error ***-" + ex.Message.ToString(), "CCAvenue");

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
