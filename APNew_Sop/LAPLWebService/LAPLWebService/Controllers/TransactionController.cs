using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LAPLWebService.Models;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace LAPLWebService.Controllers
{
    public class TransactionController : ApiController
    {
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        Response[] responcePositive = new Response[] 
        { 
            new Response { Status = 0, Message = "Save successfully"}
        };
        Response[] responceNegative = new Response[] 
        { 
            new Response { Status = 1, Message = "Wrong Security Key"}
        };
        Response[] responceNotSave = new Response[] 
        { 
            new Response { Status = 2, Message = "Record Not Save"}
        };
        Response[] responcetransactionNoBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "transactionNo cannot be blank"}
        };
        Response[] responcetransactionDateBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "transactionDate cannot be blank"}
        };
        Response[] responceamountBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "amount cannot be blank"}
        };
        Response[] responceauthorizationRefNoBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "authorizationRefNo cannot be blank"}
        };
        Response[] responceDuplicateTransaction = new Response[] 
        { 
            new Response { Status = 2, Message = "Transaction no already exist"}
        };
        [HttpPost]
        public IEnumerable<Response> updateTransactionDetails([FromBody] TransactionRootObject value)
        {
            try
            {
                AddLog("***Method Call***-" + System.DateTime.Now.ToString());
                if (value.SecurityKey == "M8@!a5q*F2e#^D0W")
                {
                    AddLog("***Security Key Metched***-" + System.DateTime.Now.ToString());
                    AddLog("transactionNo--" + value.Data[0].transactionNo + "|" + "transactionDate--" + value.Data[0].transactionDate + "|" + "amount--" + value.Data[0].amount + "|" + "authorizationRefNo--" + value.Data[0].authorizationRefNo + "|---" + System.DateTime.Now.ToString());

                    if (value.Data[0].transactionNo == "" || value.Data[0].transactionNo==null)
                    {
                        AddLog("*** transactionNo  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcetransactionNoBlank;
                    }
                    if (value.Data[0].transactionDate == "" || value.Data[0].transactionDate==null)
                    {
                        AddLog("*** transactionDate  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcetransactionDateBlank;
                    }
                    if (value.Data[0].amount == "" || value.Data[0].amount==null)
                    {
                        AddLog("*** amount  is  blank ***-" + System.DateTime.Now.ToString());
                        return responceamountBlank;
                    }
                    if (value.Data[0].authorizationRefNo == "" || value.Data[0].authorizationRefNo==null)
                    {
                        AddLog("*** authorizationRefNo  is  blank ***-" + System.DateTime.Now.ToString());
                        return responceauthorizationRefNoBlank;
                    }
                    int i;
                    string SqlQuery = "Select transactionNo from Transaction_Stagging where transactionNo='" + value.Data[0].transactionNo + "'";
                    DataTable dt = Utils.GetDataTable(SqlQuery, CnnString);
                    if (dt.Rows.Count > 0)
                    {
                        AddLog("***Transaction no Already Exist ***-" + System.DateTime.Now.ToString());
                        return responceDuplicateTransaction;
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(CnnString))
                        {
                            using (SqlCommand cmd = new SqlCommand("InsertOrUpdateLAPL"))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@MethodName", "updateTransactionDetails");
                                cmd.Parameters.AddWithValue("@transactionNo", value.Data[0].transactionNo);
                                String[] StringtransactionDate = value.Data[0].transactionDate.Split('/');
                                String transactionDate = StringtransactionDate[2] + "-" + StringtransactionDate[1] + "-" + StringtransactionDate[0];
                                cmd.Parameters.AddWithValue("@transactionDate", transactionDate);
                                cmd.Parameters.AddWithValue("@amount", value.Data[0].amount);
                                cmd.Parameters.AddWithValue("@authorizationRefNo", value.Data[0].authorizationRefNo);
                                cmd.Connection = con;
                                con.Open();
                                i = cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        if (i > 0)
                        {
                            AddLog("***Data Save***-" + System.DateTime.Now.ToString());
                            return responcePositive;
                        }
                        else
                        {
                            AddLog("***Data Not Save***-" + System.DateTime.Now.ToString());
                            return responceNotSave;
                        }

                    }
                    //string SQLQuery = "update hsrpData set transactionNo='" + value.Data[0].transactionNo.ToString() + "',transactionDate='" + value.Data[0].transactionDate.ToString() + "',amount='" + value.Data[0].amount.ToString() + "' where authorizationRefNo='" + value.Data[0].authorizationRefNo.ToString() + "'";
                    //int i = Utils.ExecNonQuery(SQLQuery, CnnString);
                    //if (i > 0)
                    //{
                    //    AddLog("***Data Save***-" + System.DateTime.Now.ToString());
                    //    return responcePositive;
                    //}
                    //else
                    //{
                    //    AddLog("***Data Not Save***-" + System.DateTime.Now.ToString());
                    //    return responceNotSave;
                    //}
                }
                else
                {
                    AddLog("***Security Key Not Metched***-" + System.DateTime.Now.ToString());
                    return responceNegative;
                }
                AddLog("***Method End***-" + System.DateTime.Now.ToString());
            }
            catch (Exception ex)
            {

                AddLog(ex.Message.ToString() + System.DateTime.Now.ToString());
                Response[] responceExp = new Response[] 
                { 
                 new Response { Status = 2, Message = ex.Message.ToString()}
                };
                return responceExp;
            }
        }

        private void AddLog(string logtext)
        {
            string filename = "updateTransactionDetails-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".txt";
            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["APPathX"].ToString();
            fileLocation += filename;
            using (StreamWriter sw = File.AppendText(fileLocation))
            {
                // sw.WriteLine("-------------------" + System.DateTime.Now.ToString() + "--------------------");
                sw.WriteLine(logtext);
                // sw.WriteLine("-----------------------------------------------------------------------------");
                sw.Close();
            }
        }
    }
}
