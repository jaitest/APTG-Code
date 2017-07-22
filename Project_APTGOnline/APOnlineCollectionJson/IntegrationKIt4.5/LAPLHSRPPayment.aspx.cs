

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IntegrationKIt4._5
{
    public partial class LAPLHSRPPayment : System.Web.UI.Page
    {
        string stingHSRPAmnt = string.Empty;
        string ReturnUrl = string.Empty;
        string orderStatus = string.Empty;
        Utils utils = new Utils();
        string CnnString = String.Empty;
        string sql = string.Empty;
        string sql1 = string.Empty;
        string DepositAmount = "0";
        string Dealeardetail = string.Empty;
        string totcoll = "0";
        int intDepositAmount = 0;
        int inttotcoll = 0;
        public string PostX = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            CnnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!IsPostBack)
            {
                string strQuery = "Update APOnlinePayment set BtnInitiatedFrom='LAPL',[PaymentMobile]='" + Request.Form["txtMobileNo"].ToString() + "',[PaymentEmail]='" + Request.Form["txtEmail"].ToString() + "' where [OnlinePaymentID]='" + Request.Form["order_id"].ToString() + "'";
                string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                ExecNonQuery(strQuery, ConnectionString);
                LblOrderNo.Text = Request.Form["order_id"].ToString();
                LblAmount.Text = Request.Form["amount"].ToString();
            }

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
        public static int ExecNonQuery(string SQLString, string CnnString)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(CnnString))
            {
                SqlCommand command = new SqlCommand(SQLString, connection);
                command.Connection.Open();
                count = command.ExecuteNonQuery();
                command.Connection.Close();
            }
            return count;
        }
        protected void Btnlogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserId.Text))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide User Id.";
                return;
            }
            if (string.IsNullOrEmpty(txtpassword.Text))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Password.";
                return;
            }
            string sqlquery = "Select * from users where userloginname='" + txtUserId.Text.Trim() + "' and Password='" + txtpassword.Text.Trim() + "'";
            DataTable dt1 = Utils.GetDataTable(sqlquery, CnnString);
            if (dt1.Rows.Count > 0)
            {
                UserID.Value = dt1.Rows[0]["UserID"].ToString();
                sql = "select isnull(sum(DepositAmount),0) as DepositAmount from [BankTransaction] where approvedstatus='Y' and userid='" + dt1.Rows[0]["UserID"].ToString() + "'";
                DepositAmount = Utils.getScalarStringValue(sql, CnnString);
                intDepositAmount = int.Parse(DepositAmount);

                sql = "select isnull(sum(roundoff_netamount),0) as amount from HSRPRecords where createdby='" + dt1.Rows[0]["UserID"].ToString() + "'";

                totcoll = Utils.getScalarStringValue(sql, CnnString);
                inttotcoll = int.Parse(totcoll);
                intDepositAmount = intDepositAmount - inttotcoll;
                lblCollectionAmount.Text = inttotcoll.ToString();

                if (intDepositAmount <= 0)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Insufficient Balance.";
                    return;
                }
                else
                {
                    int intAmount = int.Parse(LblAmount.Text); 
                    lblBalanceAmount.Text = intDepositAmount.ToString();
                    if (intDepositAmount < intAmount)
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Insufficient balance contact administrator.";
                        return;
                    }
                }
              
                sql = "Select dealername from dealerMaster where dealerid='" + dt1.Rows[0]["dealerid"].ToString() + "' ";
                lblDealername.Text = Utils.getScalarStringValue(sql, CnnString);

                string AuthNoSQL = "select distinct PaymentMobile,HSRPRecord_AuthorizationNo,ChassisNo,DealerName,dealerid,DealerAddress,DealerContactNo,TotalAmount,ReferenceUrl,Netamount from APOnlinePayment where [OnlinePaymentID]='" + LblOrderNo.Text + "'";
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
                        returnR.Value = dt.Rows[i]["ReferenceUrl"].ToString();
                        AuthNo.Value = dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString();
                        HSRPAmnt.Value = dt.Rows[i]["Netamount"].ToString();
                        PymntOrderNo.Value = LblOrderNo.Text;
                        hdnPhone.Value = dt.Rows[i]["PaymentMobile"].ToString();

                    }
                }
                else
                {
                    lblErrMess.Text = "Invalid Record Contact Administration";
                    return;
                }

                PanelPayNow.Visible = true;
                PanelLogin.Visible = false;

            }
            else
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Invalid UserId and Password.";
                return;
            }


        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }
        protected void btncancle_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "Your online transaction has been declined.";
            PymntFlag.Value = "No";
            PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
            stingHSRPAmnt = HSRPAmnt.Value;
            HSRPAmnt.Value = HSRPAmnt.Value;
            PymntErrorMsg.Value = "Your online transaction has been declined.";
            form1.Action = returnR.Value;
            btnpaynow.Enabled = false;
            btnPaynowcancle.Enabled = false;
        }

        protected void btnPaynowcancle_Click(object sender, EventArgs e)
        {
            try
            {

                AddLog("***Pay Now Cancelleed***- " + LblOrderNo.Text + "--" + System.DateTime.Now.ToString(), "PayByLAPL");

                lblErrMess.Text = "Your online transaction has been declined.";
                PymntFlag.Value = "No";
                PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                stingHSRPAmnt = HSRPAmnt.Value;
                HSRPAmnt.Value = HSRPAmnt.Value;
                PymntErrorMsg.Value = "Your online transaction has been declined.";
                form1.Action = returnR.Value;
                btnpaynow.Enabled = false;
                btnPaynowcancle.Enabled = false;
            }
            catch (Exception ex)
            {
                AddLog("***Pay Now Cancelled Exception***- " + ex.Message.ToString(), "PayByLAPL");

            }
        }

        protected void btnpaynow_Click(object sender, EventArgs e)
        {

            try
            {

                AddLog("***Pay Now Initiated***- " + LblOrderNo.Text + "--" + System.DateTime.Now.ToString(), "PayByLAPL");



                Utils.ExecNonQuery("update APOnlinePayment set CreatedBy='" + UserID.Value + "',OnlinePaymentStatus='Y' ,PaymentGateway='PayByLAPL', BankRefNo='" + LblOrderNo.Text + "' where [OnlinePaymentID]='" + LblOrderNo.Text + "'", CnnString);


                string serverdate = System.DateTime.Now.ToString("dd/MM/yyyy");

                //execute query to move record from APOnlinePayment to HSRPRecords
                Utils.ExecNonQuery("Exec [GetAPOnlinePayment] '" + LblOrderNo.Text + "'", CnnString);
                AddLog("***Order Booked***-" + System.DateTime.Now.ToString(), "PayByLAPL");
                if (hdnPhone.Value.Length >0)
                {
                    string SMSText = "Dear customer, Your account ID has been debited for Rs " + HSRPAmnt.Value + ". AuthNo -" + AuthNo.Value + " Transaction ID-" + LblOrderNo.Text + ". Thank You - LAPL";
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
                form1.Action = returnR.Value;
                btnpaynow.Enabled = false;
                btnPaynowcancle.Enabled = false;
                lbldone.Text = "Transaction Complete Wait for control to transfer to Dealer App";
                AddLog("***Control Sent To Govt Server ***-" + System.DateTime.Now.ToString(), "PayByLAPL");
            }
            catch (Exception ex)
            {
                AddLog("***Pay Now Cancelled Exception***- " + ex.Message.ToString(), "PayByLAPL");

            }    
        }

    }
}