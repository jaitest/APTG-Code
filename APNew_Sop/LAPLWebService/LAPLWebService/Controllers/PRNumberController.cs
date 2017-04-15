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
    public class PRNumberController : ApiController
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
        Response[] responceprNumberBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "prNumber cannot be blank"}
        };
        Response[] responceregDateBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "regDate cannot be blank"}
        };
        Response[] responceauthorizationRefNoBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "authorizationRefNo cannot be blank"}
        };
        Response[] responceDuplicatePRNumber = new Response[] 
        { 
            new Response { Status = 2, Message = "PR No already exist"}
        };
        Response[] responcePRNumberLenght = new Response[] 
        { 
            new Response { Status = 2, Message = "PR No not exceed"}
        };
        [HttpPost]
        public IEnumerable<Response> updatePRNumber([FromBody] PRRootObject value)
        {
            try
            {
                
                AddLog("***Method Call***-" + System.DateTime.Now.ToString());
                if (value.SecurityKey == "M8@!a5q*F2e#^D0W")
                {
                    AddLog("***Security Key Metched***-" + System.DateTime.Now.ToString());
                    AddLog("prNumber--" + value.Data[0].prNumber + "|" + "regDate--" + value.Data[0].regDate + "|" + "authorizationRefNo--" + value.Data[0].authorizationRefNo + "---" + System.DateTime.Now.ToString());

                    if (value.Data[0].prNumber == "" || value.Data[0].prNumber==null)
                    {
                        AddLog("*** prNumber  is  blank ***-" + System.DateTime.Now.ToString());
                        return responceprNumberBlank;
                    }
                    if (value.Data[0].authorizationRefNo == "" || value.Data[0].authorizationRefNo==null)
                    {
                        AddLog("*** authorizationRefNo  is  blank ***-" + System.DateTime.Now.ToString());
                        return responceauthorizationRefNoBlank;
                    }
                    if (value.Data[0].regDate == "" || value.Data[0].regDate==null)
                    {
                        AddLog("*** regDate  is  blank ***-" + System.DateTime.Now.ToString());
                        return responceregDateBlank;
                    }
                    if (value.Data[0].prNumber.Length > 10)
                    {
                        AddLog("*** PR No less than 11 digit ***-" + System.DateTime.Now.ToString());
                        return responcePRNumberLenght;
                    }
                    int i;
                    string SqlQuery = "Select prNumber from PRNumber_Stagging where prNumber='" + value.Data[0].prNumber + "'";
                    DataTable dt = Utils.GetDataTable(SqlQuery, CnnString);
                    if (dt.Rows.Count > 0)
                    {
                        AddLog("***PR Number Already Exist ***-" + System.DateTime.Now.ToString());
                        return responceDuplicatePRNumber;
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(CnnString))
                        {
                            using (SqlCommand cmd = new SqlCommand("InsertOrUpdateLAPL"))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@MethodName", "updatePRNumber");
                                cmd.Parameters.AddWithValue("@authorizationRefNo", value.Data[0].authorizationRefNo);
                                cmd.Parameters.AddWithValue("@prNumber", value.Data[0].prNumber);
                                String[] StringregDate = value.Data[0].regDate.Split('/');
                                String regDate = StringregDate[2] + "-" + StringregDate[1] + "-" + StringregDate[0];
                                cmd.Parameters.AddWithValue("@regDate", regDate);
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

                    //string SQLQuery = "update hsrpData set prNumber='" + value.Data[0].prNumber.ToString() + "',regDate='" + value.Data[0].regDate.ToString() + "' where authorizationRefNo='" + value.Data[0].authorizationRefNo.ToString() + "'";
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
            string filename = "updatePRNumber-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".txt";
            //string pathx = "E:\\GetDataLog\\TGGETREGNO-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
            //using (StreamWriter sw = File.AppendText(pathx))

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
