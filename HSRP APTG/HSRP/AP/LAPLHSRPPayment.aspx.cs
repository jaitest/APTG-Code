

using HSRP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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
        string ID;
        protected void Page_Load(object sender, EventArgs e)
        {

            CnnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!IsPostBack)
            {
                LblOrderNo.Text = Request.Form["order_id"].ToString();
                LblAmount.Text = Request.Form["amount"].ToString();
                ID = Request.Form["ID"].ToString();

                try
                {

                    AddLog("***Pay Now Initiated***- " + LblOrderNo.Text + "--" + System.DateTime.Now.ToString(), "PayByLAPL");

                    string AuthNoSQL = "select distinct HSRPRecord_AuthorizationNo,ChassisNo,DealerName,dealerid,DealerAddress,DealerContactNo,TotalAmount,ReferenceUrl,Netamount from hsrprecords where hsrprecordid='" + ID + "'";
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
                        }
                    }
                    //Utils.ExecNonQuery("update APOnlinePayment Set OnlinePaymentStatus='Y', ReSyncUserId='" + Session["UID"].ToString() + "',ReSyncDatetime=getdate(), ReSyncStatus='Y' where ID='" + ID + "'", CnnString);
                    string serverdate = System.DateTime.Now.ToString("dd/MM/yyyy");
                    //execute query to move record from APOnlinePayment to HSRPRecords
                    AddLog("***Order Booked***-" + System.DateTime.Now.ToString(), "PayByLAPL");
                    
                    PymntFlag.Value = "Yes";
                    PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                    HSRPAmnt.Value = HSRPAmnt.Value;
                    stingHSRPAmnt = HSRPAmnt.Value;
                    PymntErrorMsg.Value = string.Empty;
                    form1.Action = returnR.Value;//"https://www.hsrpap.com/hsrponlinetest/returnurl.aspx";//
                    lbldone.Text = "Transaction Complete Wait for control to transfer to Dealer App";
                    AddLog("***Control Sent To Govt Server ***-" + System.DateTime.Now.ToString(), "PayByLAPL");
                }
                catch (Exception ex)
                {
                    AddLog("***Pay Now Cancelled Exception***- " + ex.Message.ToString(), "PayByLAPL");

                }   
            }

        }
        static void AddLog(string logtext, string mode)
        {
            string path = @"C:\OnlinePaymentLog";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string pathx = "C:\\OnlinePaymentLog\\APHSRPReSync-" + mode + "-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
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
        




        protected void btnpaynow_Click(object sender, EventArgs e)
        {

            try
            {

                AddLog("***Pay Now Initiated***- " + LblOrderNo.Text + "--" + System.DateTime.Now.ToString(), "PayByLAPL");

                string AuthNoSQL = "select distinct HSRPRecord_AuthorizationNo,ChassisNo,DealerName,dealerid,DealerAddress,DealerContactNo,TotalAmount,ReferenceUrl,Netamount from APOnlinePayment where ID=''";
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


                    }
                }
                
                //update APOnlinePayment Set OnlinePaymentStatus='Y', ReSyncUserId='" + strUserID + "',ReSyncDatetime=getdate(), ReSyncStatus='Y' where ID='" + strId + "'
                //Utils.ExecNonQuery("update APOnlinePayment set CreatedBy='" + UserID.Value + "',OnlinePaymentStatus='Y' ,PaymentGateway='PayByLAPL', BankRefNo='" + LblOrderNo.Text + "' where [OnlinePaymentID]='" + LblOrderNo.Text + "'", CnnString);


                string serverdate = System.DateTime.Now.ToString("dd/MM/yyyy");

                //execute query to move record from APOnlinePayment to HSRPRecords
                AddLog("***Order Booked***-" + System.DateTime.Now.ToString(), "PayByLAPL");

                PymntFlag.Value = "Yes";
                PymntDt.Value = DateTime.Now.ToString("MM/dd/yyyy");
                HSRPAmnt.Value = HSRPAmnt.Value;
                stingHSRPAmnt = HSRPAmnt.Value;
                PymntErrorMsg.Value = string.Empty;
                form1.Action = returnR.Value;
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