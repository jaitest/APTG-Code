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
            string CnnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            string workingKey = "6679138283CF1C1021711F6B1B364E9C";
            string orderStatus = "";
            string NetAmount = string.Empty;
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
            //  //  Response.Write(Params.Keys[i] + " = " + Params[i] + "<br>");
            //}
            orderNo.Text = Params[0].ToString();
            AddLog("***Order No***-" + orderNo.Text +"=="+ System.DateTime.Now.ToString(), "CCAvenue");

            string AuthNoSQL = "select distinct HSRPRecord_AuthorizationNo,ChassisNo,DealerName,dealerid,DealerAddress,DealerContactNo,TotalAmount,ReferenceUrl,Netamount,paymentmobile from TGOnlinePayment where [OnlinePaymentID]='" + Params[0].ToString() + "'";
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
                    NetAmount = dt.Rows[i]["Netamount"].ToString();
                    lblTelephone.Text = dt.Rows[i]["paymentmobile"].ToString();

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

                Utils.ExecNonQuery("update TGOnlinePayment set OnlinePaymentStatus='Y' ,PaymentGateway='CCAvenue', BankRefNo='" + lblBankRefNo.Text + "',OnlinePaymentTrackingNo='" + lbltrackingid.Text + "'  where [OnlinePaymentID]='" + Params[0].ToString() + "'", CnnString);
                //execute query to move record from APOnlinePayment to HSRPRecords
                Utils.ExecNonQuery("Exec [GetTGOnlinePayment] '" + Params[0].ToString() + "'", CnnString);
                AddLog("***Order Booked***-" + System.DateTime.Now.ToString(), "CCAvenue");


                IntegrationKIt4._5.HSRP.HSRPAuthorizationServiceSoapClient objTgService = new IntegrationKIt4._5.HSRP.HSRPAuthorizationServiceSoapClient();

                string serverdate = System.DateTime.Now.ToString("dd/MM/yyyy");
                string AuthData = objTgService.UpdateHSRPCharges(AuthNo.Value, Math.Round(Convert.ToDecimal(lblAmount.Text), 0), serverdate);
                AddLog("***Synced To Govt***-" + System.DateTime.Now.ToString(), "CCAvenue");
                
                //string Query = "update [TGOnlinePayment] set APwebserviceresp = '" + AuthData + "', APwebservicerespdate=getdate(),OnlinePaymentStatus='Y' where HSRP_StateID=11 and HSRPRecord_AuthorizationNo ='" + lblAuthNo.Text + "'";
                //Utils.ExecNonQuery(Query, CnnString);
                string SMSText = " Online Payment Rs." + HSRPAmnt.Value + " received against HSRP Authorization No. " + AuthNo.Value + " on " + System.DateTime.Now.ToString("dd/MM/yyyy") + " Online order number " + orderNo.Text + ". HSRP Team.";
                    
                if (lblTelephone.Text.Length == 10 )
                {

                   string sendURL = "http://quick.smseasy.in:8080/bulksms/bulksms?username=sse-tlhsrp1&password=tlhsrp1&type=0&dlr=1&destination=" + lblTelephone.Text + "&source=TSHSRP&message=" + SMSText;
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                    myRequest.Method = "GET";
                    WebResponse myResponse = myRequest.GetResponse();
                    StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                    string result = sr.ReadToEnd();
                    sr.Close();
                    myResponse.Close();
                    System.Threading.Thread.Sleep(350);
                    AddLog("***SMS  Sent***-" + System.DateTime.Now.ToString(), "CCAvenue");
                    Utils.ExecNonQuery("insert into TSSMSDetail(AuthorizationNumber,MobileNo,SentResponseCode,smstext,onlinepaymentid) values('" + AuthNo.Value + "','" + lblTelephone.Text.ToString() + "','" + result + "','" + SMSText + "','" + orderNo.Text + "')", CnnString);
                }
                else
                {
                    AddLog("***SMS Not Sent***-" + System.DateTime.Now.ToString(), "CCAvenue");
                    Utils.ExecNonQuery("insert into TSSMSDetail(AuthorizationNumber,MobileNo,SentResponseCode,smstext,onlinepaymentid) values('" + AuthNo.Value + "'," + lblTelephone.Text.ToString() + ",'Not Sent','" + SMSText + "','" + orderNo.Text + "')", CnnString);

                }
                // update TGOnlinePayment
                //load data to text box
                //add action to form for return URL
                //

               
                PymntFlag.Value = "Yes";
                PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                HSRPAmnt.Value = HSRPAmnt.Value;
                stingHSRPAmnt = HSRPAmnt.Value;
                PymntErrorMsg.Value = string.Empty;
                form1.Action = ReturnUrl;
            }
            else if (orderStatus == "Failure")
            {
                AddLog("***Order Status ***-" + orderStatus, "CCAvenue");
               
                lblErrMess.Text = "Your online transaction has been declined.";
                PymntFlag.Value = "No";
                PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                stingHSRPAmnt = HSRPAmnt.Value;
                HSRPAmnt.Value = HSRPAmnt.Value;
                form1.Action = ReturnUrl;

            }
            else if (orderStatus == "Invalid")
            {

                AddLog("***Order Status ***-" + orderStatus, "CCAvenue");
               
                lblErrMess.Text = "Your online transaction has been marked invalid By Payment Gateway.";
                PymntFlag.Value = "No";
                PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                stingHSRPAmnt = HSRPAmnt.Value;
                HSRPAmnt.Value = HSRPAmnt.Value;
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
                form1.Action = ReturnUrl;
            }
            else 
            {
                AddLog("***Order Status ***-" + orderStatus, "CCAvenue");
               
                lblErrMess.Text = " Your Order Was marked :" + orderStatus + " by Payment Gateway Server.";
                PymntFlag.Value = "No";
                PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                stingHSRPAmnt = HSRPAmnt.Value;
                HSRPAmnt.Value = HSRPAmnt.Value;
                form1.Action = ReturnUrl;
            }
        }
        catch (Exception ex)
        {
            AddLog("***Error***", "CCAvenue");
            AddLog(ex.Message.ToString(), "CCAvenue");
            // failureSMS(CnnString, lblTelephone.Text, Params[0].ToString());
            lblErrMess.Text = "Error While Connecting RTA Server Contact administrator:" + ex.Message.ToString();
            PymntFlag.Value = "No";
            PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
            stingHSRPAmnt = HSRPAmnt.Value;
            HSRPAmnt.Value = HSRPAmnt.Value;
            PymntErrorMsg.Value = "Error While Connecting RTA Server Contact administrator:" + ex.Message.ToString();
            form1.Action = ReturnUrl;
        }
    }
}
