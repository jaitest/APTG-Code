using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetIntegrationKit;
using System.Data.SqlClient;
namespace IntegrationKIt4._5
{
    public partial class ICICICheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string oderid = OrderNo.Value;
            string Amt = "";
            txtRequestType.Value = "S";
            txtMerchantCode.Value = "L59451";
            txtMerchantTxnRefNumber.Value = oderid;
            txtITC.Value = "A2";
            txtAmount.Value = Amt.ToString();
            txtCurrencyCode.Value = "INR";
            txtUniqueCustomerId.Value = "NA";
            txtReturnURL.Value = "https://hsrpts.com/hsrponline/tpslresponse.aspx";
            txtS2SReturnURL.Value = "NA";
            txtTPSLTxnID.Value = "NA";
            txtShoppingCartDetails.Value = "Link_" + Amt.ToString() + "_0.0";
            // txtShoppingCartDetails.Value = "Link_1.0_0.0";
            txttxnDate.Value = xdate.Value;
            txtEmailIcici.Value = "NA";
            txtMobileNumber.Value = "NA";
            txtBankCode.Value = "720";
            txtCustomerName.Value = "NA";
            txtCardID.Value = "NA";
            txtKey.Value = "3480176807VSWRXM";
            txtIv.Value = "2490077168CANYWH";

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
                return;
            }
            else
            {
                if (txtRequestType.Value.ToUpper() == "S")
                {
                    
                    string[] words = strResponse.Split('|');
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (string word in words)
                    {
                        sb.Append(word + "<br/>");
                       
                    }
                    RawMessage.Text = sb.ToString();
               
                }

            }
        }
    }
}