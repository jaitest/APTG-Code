﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CCA.Util;
using System.Data.SqlClient;


    public partial class SubmitData : System.Web.UI.Page
    {
        CCACrypto ccaCrypto = new CCACrypto();
        //string workingKey = "244F0A3B9D18807CE88B7A06B9244409";//put in the 32bit alpha numeric key in the quotes provided here 	
        string workingKey = "6679138283CF1C1021711F6B1B364E9C";
        string ccaRequest = "";
        public string strEncRequest="";


        //public string strAccessCode = "AVTZ07CJ05CI08ZTIC";// put the access key in the quotes provided here.
        public string strAccessCode = "AVZF63CL32CJ94FZJC";
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
         protected void Page_Load(object sender, EventArgs e)
        {
             if (!IsPostBack)
            {
                string strQuery = "Update TGOnlinePayment set BtnInitiatedFrom='CCavenue',[PaymentMobile]='" + Request.Form["txtMobileNo"].ToString() + "',[PaymentEmail]='" + Request.Form["txtEmail"].ToString() + "' where [OnlinePaymentID]='" + Request.Form["order_id"].ToString() + "'";
                 string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                 ExecNonQuery(strQuery, ConnectionString);
                 foreach (string name in Request.Form)
                {
                    if (name != null)
                    {
                        if (!name.StartsWith("_"))
                        {
                            ccaRequest = ccaRequest + name + "=" + Request.Form[name] + "&";
                          /* Response.Write(name + "=" + Request.Form[name]);
                            Response.Write("</br>");*/
                        }
                    }
                }
                strEncRequest = ccaCrypto.Encrypt(ccaRequest, workingKey);
            }
        }
    }

