using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetIntegrationKit;
using System.Data.SqlClient;
namespace IntegrationKIt4._5
{
    public partial class aponline2 : System.Web.UI.Page
    {
        private string SQLString = string.Empty;
        private string CnnString = string.Empty;
        private string RegNo = string.Empty;
        private DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {

          
            try
            {
                this.CnnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                if (this.IsPostBack)
                    //  return;
                    this.lblErr.Text = "";
               this.lblAuthNo.Text = ((object)this.Request.QueryString["X"]).ToString();
                this.orderNo.Text = ((object)this.Request.QueryString["orderNo"]).ToString();
                order_id.Value = ((object)this.Request.QueryString["orderNo"]).ToString();
                int RCount = Utils.getScalarCount("SELECT count(*) as Rcount from APOnlinePayment where OnlinePaymentStatus='Y' and HSRPRecord_AuthorizationNo='" + ((object)this.Request.QueryString["X"]).ToString() + "'", this.CnnString);
                if (RCount > 0)
                {
                    lblErr.Text = "Error: Order  is already processed for this Authrization number contact tech support.";
                    return;
                }
                int NetAmount = Utils.getScalarCount("SELECT top 1 RoundOff_NetAmount from APOnlinePayment where HSRPRecord_AuthorizationNo='" + ((object)this.Request.QueryString["X"]).ToString() + "' order by ID desc", this.CnnString);
                this.transAmt.Text = NetAmount.ToString();
                amount.Value = NetAmount.ToString();
                //Values for Axis
                PRN.Value = orderNo.Text;
                double AxisTDRAmount = (double)(NetAmount) * .02;
                double AxisTaxAmount = (double)(AxisTDRAmount) * (15 / 100);
                double AxisAmount = (double)(NetAmount) + AxisTDRAmount + AxisTaxAmount;
                AMT.Value = Math.Round(AxisAmount, 2).ToString();
                ITC.Value = lblAuthNo.Text;
                //VAlues for HDFC
                double ScAmount =  (double)(NetAmount) * .02;
                double ScAmountTax = (double)(ScAmount) * (15 / 100);
                ScAmountTax = ScAmount + ScAmountTax;
                 TxnAmount.Value = NetAmount.ToString();
                 TxnScAmount.Value = Math.Round(ScAmountTax, 2).ToString();
                MerchantRefNo.Value = orderNo.Text;
                Date.Value = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                //Valuesfor ICICI
                txtRequestType.Value = "T";
                txtMerchantCode.Value = "L58340";
                txtMerchantTxnRefNumber.Value =order_id.Value;
                txtITC.Value = "A2";
                txtAmount.Value = NetAmount.ToString();
                txtCurrencyCode.Value = "INR";
                txtUniqueCustomerId.Value = "NA";
                txtReturnURL.Value = "https://hsrpap.com/hsrponline/tpslresponse.aspx";
                txtS2SReturnURL.Value = "NA";
                txtTPSLTxnID.Value = "NA";
                txtShoppingCartDetails.Value = "Link_" + NetAmount.ToString()+ "_0.0";
               // txtShoppingCartDetails.Value = "Link_1.0_0.0";
                txttxnDate.Value = System.DateTime.Now.ToString("dd-MM-yyyy"); ;
                txtEmailIcici.Value = "NA";
                txtMobileNumber.Value = "NA";
                txtBankCode.Value = "720";
                txtCustomerName.Value = "NA";
                txtCardID.Value = "NA";
                txtKey.Value = "9865645077ADJSEO";
                txtIv.Value = "8566652421NCGISC"; 


                DataTable dataTable1 = Utils.GetDataTable("select top 1 rtoname, [RTOLocationID],[HSRPRecord_AuthorizationNo],[HSRPRecord_AuthorizationDate],[OwnerName],[Address1],[State],[City],[Pin],[MobileNo],[LandlineNo],[EmailID],[OrderNo],[OrderType],[OrderDate],[OrderStatus],[HSRPAuthorizationSlipDate],[VehicleClass],[VehicleType],[ManufacturerName],[ManufacturerModel],[TypeOfApplication],[ChassisNo],[EngineNo],[Manufacturer],[ManufacturingYear],[VehicleRegNo],[VehicleColor],[ISFrontPlateSize],[FrontPlateSize],[FrontplatePrize],[ISRearPlateSize],[RearPlateSize],[RearPlatePrize],[StickerMandatory],[StickerPrize],[CashReceiptNo],[InvoiceNo],[DeliveryChallanNo],[CashReceiptDateTime],[InvoiceDateTime],[DeliveryChallanDateTime],[ScrewPrize],[FixingCharge],[TotalAmount],[VAT_Percentage],[VATAmount],[ServiceTax_Percentage],[ServiceTax_Amount],[NetAmount],[TimeTakenToFill],[CreatedBy],[dealerid],[entrydate],[navaffixflag],[Affix_Id],[CounterNo],[exciseamt],[exciseper],[excisebasic],[cessamt],[shecessamt],[VAT_Amount],[NAVEMBID],[SaveMacAddress],[isVIP],[Reference],[vehicleref],[RoundOff_NetAmount],[remarks],[PlateAffixationDate],[OnlinePaymentStatus],[OnlinePaymentID] from APOnlinePayment where  [OnlinePaymentID]='" + ((object)this.orderNo.Text) + "'", this.CnnString);
                if (dataTable1.Rows.Count > 0)
                {
                    this.lblName.Text = dataTable1.Rows[0]["OwnerName"].ToString();
                    this.txtEmail.Text = dataTable1.Rows[0]["EmailID"].ToString();
                    this.txtMobileNo.Text = dataTable1.Rows[0]["MobileNo"].ToString();
                    billing_name.Value = dataTable1.Rows[0]["OwnerName"].ToString();
                    billing_email.Value = dataTable1.Rows[0]["EmailID"].ToString();
                    billing_tel.Value = dataTable1.Rows[0]["MobileNo"].ToString();
                    billing_zip.Value = dataTable1.Rows[0]["Pin"].ToString();
                    billing_state.Value = dataTable1.Rows[0]["State"].ToString();
                    billing_city.Value = dataTable1.Rows[0]["City"].ToString();
                    billing_address.Value = dataTable1.Rows[0]["Address1"].ToString();
                    merchant_param1.Value = dataTable1.Rows[0]["rtoname"].ToString();
                    merchant_param2.Value = dataTable1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                }


            }
            catch (Exception ex)
            {
                lblErr.Text = "Error in Processing :" + ex.Message.ToString();
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
        protected void payICICI_Click(object sender, EventArgs e)
        {
            string strQuery = "Update APOnlinePayment set BtnInitiatedFrom='ICICI',[PaymentMobile]='" + Request.Form["txtMobileNo"].ToString() + "',[PaymentEmail]='" + Request.Form["txtEmail"].ToString() + "' where [OnlinePaymentID]='" + Request.Form["order_id"].ToString() + "'";
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            ExecNonQuery(strQuery, ConnectionString);

            RequestURL objRequestURL = new RequestURL();
            String response = objRequestURL.SendRequest


                                          (


                                          txtRequestType.Value,
                                          txtMerchantCode.Value,
                                            txtMerchantTxnRefNumber.Value,
                                            txtITC.Value,
                                            txtAmount.Value,
                                            txtCurrencyCode.Value,
                                            txtUniqueCustomerId.Value,
                                            txtReturnURL.Value,
                                            txtS2SReturnURL.Value,
                                            txtTPSLTxnID.Value,
                                            txtShoppingCartDetails.Value,
                                            txttxnDate.Value,
                                            txtEmailIcici.Value,
                                            txtMobileNumber.Value,
                                            txtBankCode.Value,
                                            txtCustomerName.Value,
                                            txtCardID.Value,
                                            txtKey.Value,
                                            txtIv.Value

                                          );

            String strResponse = response.ToUpper();

            if (strResponse.StartsWith("ERROR"))
            {
                lblError.Text = response;
            }
            else
            {
                if (txtRequestType.Value.ToUpper() == "T")
                {

                    Response.Write("<form name='s1_2' id='s1_2' action='" + response + "' method='post'> ");

                    Response.Write("<script type='text/javascript' language='javascript' >document.getElementById('s1_2').submit();");

                    Response.Write("</script>");
                    Response.Write("<script language='javascript' >");
                    Response.Write("</script>");
                    Response.Write("</form> ");

                }

            }

        }
    }
}