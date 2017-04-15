using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IntegrationKIt4._5
{
    public partial class hdfcresponse : System.Web.UI.Page
    {
        string stingHSRPAmnt = string.Empty;
        string ReturnUrl = string.Empty;

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string orderStatus = string.Empty;
            try
            {
                string CnnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                string BankAckCode = Request.QueryString["BankAckCode"].ToString();
                if (BankAckCode == "102")
                {
                    orderStatus = "Y";
                    lblOrderStatus.Text = "Yes";
                    lblOrderStatusMsg.Text = "Yes";
                    lbltrackingid.Text = Request.QueryString["BankRefNo"].ToString();
                    lblAmount.Text = Request.QueryString["TxnAmount"].ToString();
                    lblBankRefNo.Text = Request.QueryString["BankRefNo"].ToString();

                }
                else
                {
                    orderStatus = "N";
                }
                //BackAckMsg

                orderNo.Text = Request.QueryString["MerchantRefNo"].ToString();
                PymntOrderNo.Value = Request.QueryString["MerchantRefNo"].ToString();

                string AuthNoSQL = "select distinct HSRPRecord_AuthorizationNo,ChassisNo,DealerName,dealerid,DealerAddress,DealerContactNo,TotalAmount,ReferenceUrl,Netamount from TGOnlinePayment where [OnlinePaymentID]='" + Request.QueryString["MerchantRefNo"].ToString() + "'";
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



                //foreach (string s in Request.Form.Keys)
                //{


                //    Response.Write((s.ToString() + ":" + Request.Form[s] + ""));


                //}
                //Response.End();
                if (orderStatus == "Y")
                {

                    Utils.ExecNonQuery("update TGOnlinePayment set OnlinePaymentStatus='Y' ,PaymentGateway='HDFC', BankRefNo='" + lblBankRefNo.Text + "' where [OnlinePaymentID]='" + Request.QueryString["MerchantRefNo"].ToString() + "'", CnnString);

                    //execute query to move record from APOnlinePayment to HSRPRecords
                     Utils.ExecNonQuery("Exec [GetTGOnlinePayment] '" + Request.QueryString["MerchantRefNo"].ToString() + "'", CnnString);

                    PymntFlag.Value = "Yes";
                    PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                    HSRPAmnt.Value = HSRPAmnt.Value;
                    stingHSRPAmnt = HSRPAmnt.Value;
                    PymntErrorMsg.Value = string.Empty;
                    form1.Action = ReturnUrl;


                }
                else if (orderStatus == "N")
                {
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
            Utils.ExecNonQuery("update TGOnlinePayment set OnlinePaymentStatus='N' where [OnlinePaymentID]='" + paymentId + "'", connectionString);
        }
    }
}