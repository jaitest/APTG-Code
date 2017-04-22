using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LAPLWebService.Models;
using Newtonsoft.Json;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace LAPLWebService.Controllers
{
    public class HSRPRecordController : ApiController
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
        Response[] responceTransactionnoBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "Transaction no cannot be blank"}
        };
        Response[] responceTransactionDateBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "Transaction Date cannot be blank"}
        };
        Response[] responceauthorizationRefNoBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "authorizationRefNo cannot be blank"}
        };
        Response[] responceengineNoBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "engineNo cannot be blank"}
        };
        Response[] responcechassisNoBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "chassisNo cannot be blank"}
        };
        Response[] responcevehicleTypeBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "vehicleType cannot be blank"}
        };
        Response[] responcetransTypeBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "transType cannot be blank"}
        };
        Response[] responcevehicleClassTypeBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "vehicleClassType cannot be blank"}
        };
        Response[] responcehsrpFeeBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "hsrpFee cannot be blank"}
        };
        Response[] responcertoCodeBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "rtoCode cannot be blank"}
        };
        Response[] responcertoNameBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "rtoName cannot be blank"}
        };
        Response[] responceownerNameBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "ownerName cannot be blank"}
        };
        Response[] responceownerAddressBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "responceownerAddressBlank cannot be blank"}
        };
        Response[] responcemobileNoBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "mobileNo cannot be blank"}
        };
        Response[] responcedealerNameBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "dealerName cannot be blank"}
        };
        Response[] responcedealerRtoCodeBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "dealerRtoCode cannot be blank"}
        };
        Response[] responceaffixationCenterCodeBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "affixationCenterCode cannot be blank"}
        };
        Response[] responceauthorizationDateBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "authorizationDate cannot be blank"}
        };
        Response[] responceownerPinCodeBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "ownerPinCode cannot be blank"}
        };
        Response[] responcemfrsNameBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "mfrsName cannot be blank"}
        };
        Response[] responcemodelNameBlank = new Response[] 
        { 
            new Response { Status = 2, Message = "modelName cannot be blank"}
        };
        Response[] responceDuplicateAuthNo = new Response[] 
        { 
            new Response { Status = 2, Message = "authorization Ref No already exist"}
        };

        [HttpPost]
        public IEnumerable<Response> postHsrpRecords([FromBody] RootObject value)
        {
            try
            {
                AddLog("***Method Call***-" + System.DateTime.Now.ToString());

                if (value.SecurityKey == "M8@!a5q*F2e#^D0W")
                {
                    AddLog("***Security Key Metched***-" + System.DateTime.Now.ToString());
                    AddLog("Rto_Code" + "-" + value.Data[0].rtoCode + "|" + "Rto_Name" + "-" + value.Data[0].rtoName + "|" + "Affixation_Center_Code" + "-" + value.Data[0].affixationCenterCode + "|" + "transactionNo" + "-" + value.Data[0].transactionNo + "|" + "transactionDate" + "-" + value.Data[0].transactionDate + "|" + "authorizationRefNo" + "-" + value.Data[0].authorizationRefNo + "|" + "authorizationDate" + "-" + value.Data[0].authorizationDate + "|" + "engineNo" + "-" + value.Data[0].engineNo + "|" + "chassisNo" + "-" + value.Data[0].chassisNo + "|" + "prNumber" + "-" + value.Data[0].prNumber + "|" + "ownerName" + "-" + value.Data[0].ownerName + "|" + "ownerAddress" + "-" + value.Data[0].ownerAddress + "|" + "ownerEmailId" + "-" + value.Data[0].ownerEmailId + "|" + "ownerPinCode" + "-" + value.Data[0].ownerPinCode + "|" + "mobileNo" + "-" + value.Data[0].mobileNo + "|" + "vehicleType" + "-" + value.Data[0].vehicleType + "|" + "transType" + "-" + value.Data[0].transType + "|" + "vehicleClassType" + "-" + value.Data[0].vehicleClassType + "|" + "mfrsName" + "-" + value.Data[0].mfrsName + "|" + "modelName" + "-" + value.Data[0].modelName + "|" + "hsrpFee" + "-" + value.Data[0].hsrpFee + "|" + "oldNewFlag" + "-" + value.Data[0].oldNewFlag + "|" + "govtVehicleTag" + "-" + value.Data[0].govtVehicleTag + "|" + "timeStamp" + "-" + value.Data[0].timeStamp + "|" + "trNumber" + "-" + value.Data[0].trNumber + "|" + "dealerName" + "-" + value.Data[0].dealerName + "|" + "dealerMail" + "-" + value.Data[0].dealerMail + "|" + "dealerRtoCode" + "-" + value.Data[0].dealerRtoCode + "---" + System.DateTime.Now.ToString() + "---");

                    if (value.Data[0].transactionNo == "" || value.Data[0].transactionNo==null)
                    {
                        AddLog("***Transaction no is  blank***-" + System.DateTime.Now.ToString());
                        return responceTransactionnoBlank;
                    }
                    if (value.Data[0].transactionDate == "" || value.Data[0].transactionDate==null)
                    {
                        AddLog("*** Transaction Date is  blank ***-" + System.DateTime.Now.ToString());
                        return responceTransactionDateBlank;
                    }
                    if (value.Data[0].authorizationRefNo == "" || value.Data[0].authorizationRefNo==null)
                    {
                        AddLog("*** authorizationRefNo  is  blank ***-" + System.DateTime.Now.ToString());
                        return responceauthorizationRefNoBlank;
                    }
                    if (value.Data[0].engineNo == "" || value.Data[0].engineNo==null)
                    {
                        AddLog("*** engineNo  is  blank ***-" + System.DateTime.Now.ToString());
                        return responceengineNoBlank;
                    }
                    if (value.Data[0].chassisNo == "" || value.Data[0].chassisNo==null)
                    {
                        AddLog("*** chassisNo  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcechassisNoBlank;
                    }
                    if (value.Data[0].vehicleType == "" || value.Data[0].vehicleType==null)
                    {
                        AddLog("*** vehicleType  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcevehicleTypeBlank;
                    }
                    if (value.Data[0].transType == "" || value.Data[0].transType==null)
                    {
                        AddLog("*** transType  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcetransTypeBlank;
                    }
                    if (value.Data[0].vehicleClassType == "" || value.Data[0].vehicleClassType==null)
                    {
                        AddLog("*** vehicleClassType  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcevehicleClassTypeBlank;
                    }
                    if (value.Data[0].hsrpFee == "" || value.Data[0].hsrpFee==null)
                    {
                        AddLog("*** hsrpFee  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcehsrpFeeBlank;
                    }
                    if (value.Data[0].rtoCode == "" || value.Data[0].rtoCode == null)
                    {
                        AddLog("*** rtoCode  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcertoCodeBlank;
                    }
                    if (value.Data[0].rtoName == "" || value.Data[0].rtoName == null)
                    {
                        AddLog("*** rtoName  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcertoNameBlank;
                    }
                    if (value.Data[0].affixationCenterCode == "" || value.Data[0].affixationCenterCode == null)
                    {
                        AddLog("*** affixationCenterCode  is  blank ***-" + System.DateTime.Now.ToString());
                        return responceaffixationCenterCodeBlank;
                    }
                    if (value.Data[0].ownerName == "" || value.Data[0].ownerName == null)
                    {
                        AddLog("*** ownerName  is  blank ***-" + System.DateTime.Now.ToString());
                        return responceownerNameBlank;
                    }
                    if (value.Data[0].ownerAddress == "" || value.Data[0].ownerAddress == null)
                    {
                        AddLog("*** ownerAddress  is  blank ***-" + System.DateTime.Now.ToString());
                        return responceownerAddressBlank;
                    }
                    if (value.Data[0].mobileNo == "" || value.Data[0].mobileNo == null)
                    {
                        AddLog("*** mobileNo  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcemobileNoBlank;
                    }
                    if (value.Data[0].dealerName == "" || value.Data[0].dealerName == null)
                    {
                        AddLog("*** dealerName  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcedealerNameBlank;
                    }
                    if (value.Data[0].authorizationDate == "" || value.Data[0].authorizationDate == null)
                    {
                        AddLog("*** authorizationDate  is  blank ***-" + System.DateTime.Now.ToString());
                        return responceauthorizationDateBlank;
                    }
                    if (value.Data[0].ownerPinCode == "" || value.Data[0].ownerPinCode == null)
                    {
                        AddLog("*** ownerPinCode  is  blank ***-" + System.DateTime.Now.ToString());
                        return responceownerPinCodeBlank;
                    }
                    if (value.Data[0].mfrsName == "" || value.Data[0].mfrsName == null)
                    {
                        AddLog("*** mfrsName  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcemfrsNameBlank;
                    }
                    if (value.Data[0].modelName == "" || value.Data[0].modelName == null)
                    {
                        AddLog("*** modelName  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcemodelNameBlank;
                    }
                    if (value.Data[0].dealerRtoCode == "" || value.Data[0].dealerRtoCode == null)
                    {
                        AddLog("*** dealerRtoCode  is  blank ***-" + System.DateTime.Now.ToString());
                        return responcedealerRtoCodeBlank;
                    }
                    int i;
                    string SqlQuery = "Select authorizationRefNo from HsrpData where authorizationRefNo='" + value.Data[0].authorizationRefNo + "'";
                    DataTable dt = Utils.GetDataTable(SqlQuery, CnnString);
                    if (dt.Rows.Count > 0)
                    {
                        AddLog("***authorizationRefNo Already Exist ***-" + System.DateTime.Now.ToString());
                        return responceDuplicateAuthNo;
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(CnnString))
                        {
                            using (SqlCommand cmd = new SqlCommand("InsertOrUpdateLAPL"))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@MethodName", "postHsrpRecords");
                                cmd.Parameters.AddWithValue("@rtoCode", value.Data[0].rtoCode);
                                cmd.Parameters.AddWithValue("@rtoName", value.Data[0].rtoName);
                                cmd.Parameters.AddWithValue("@affixationCenterCode", value.Data[0].affixationCenterCode);
                                cmd.Parameters.AddWithValue("@transactionNo", value.Data[0].transactionNo);
                                String[] StringDate = value.Data[0].transactionDate.Split('/');
                                String transactionDate = StringDate[2] + "-" + StringDate[1] + "-" + StringDate[0];
                                cmd.Parameters.AddWithValue("@transactionDate", transactionDate);
                                cmd.Parameters.AddWithValue("@authorizationRefNo", value.Data[0].authorizationRefNo);
                                String[] StringauthorizationDate = value.Data[0].transactionDate.Split('/');
                                String authorizationDate = StringauthorizationDate[2] + "-" + StringauthorizationDate[1] + "-" + StringauthorizationDate[0];
                                cmd.Parameters.AddWithValue("@authorizationDate", authorizationDate);
                                cmd.Parameters.AddWithValue("@engineNo", value.Data[0].engineNo);
                                cmd.Parameters.AddWithValue("@chassisNo", value.Data[0].chassisNo);
                                cmd.Parameters.AddWithValue("@prNumber", value.Data[0].prNumber);
                                cmd.Parameters.AddWithValue("@ownerName", value.Data[0].ownerName);
                                cmd.Parameters.AddWithValue("@ownerAddress", value.Data[0].ownerAddress);
                                cmd.Parameters.AddWithValue("@ownerEmailId", value.Data[0].ownerEmailId);
                                cmd.Parameters.AddWithValue("@ownerPinCode", value.Data[0].ownerPinCode);
                                cmd.Parameters.AddWithValue("@mobileNo", value.Data[0].mobileNo);
                                cmd.Parameters.AddWithValue("@vehicleType", value.Data[0].vehicleType);
                                cmd.Parameters.AddWithValue("@transType", value.Data[0].transType);
                                cmd.Parameters.AddWithValue("@vehicleClassType", value.Data[0].vehicleClassType);
                                cmd.Parameters.AddWithValue("@mfrsName", value.Data[0].mfrsName);
                                cmd.Parameters.AddWithValue("@modelName", value.Data[0].modelName);
                                cmd.Parameters.AddWithValue("@hsrpFee", value.Data[0].hsrpFee);
                                cmd.Parameters.AddWithValue("@oldNewFlag", value.Data[0].oldNewFlag);
                                cmd.Parameters.AddWithValue("@govtVehicleTag", value.Data[0].govtVehicleTag);
                                String[] StringtimeStamp = value.Data[0].transactionDate.Split('/');
                                String timeStamp = StringtimeStamp[2] + "-" + StringtimeStamp[1] + "-" + StringtimeStamp[0];
                                cmd.Parameters.AddWithValue("@timeStamp", timeStamp);//value.Data[0].timeStamp.ToString());
                                cmd.Parameters.AddWithValue("@trNumber", value.Data[0].trNumber);
                                cmd.Parameters.AddWithValue("@dealerName", value.Data[0].dealerName);
                                cmd.Parameters.AddWithValue("@dealerMail", value.Data[0].dealerMail);
                                cmd.Parameters.AddWithValue("@dealerRtoCode", value.Data[0].dealerRtoCode);
                                cmd.Connection = con;
                                con.Open();
                                i = cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        //string SQLQuery = "Insert into hsrpData(rtoCode,rtoName,affixationCenterCode,transactionNo,transactionDate,authorizationRefNo,authorizationDate,engineNo,chassisNo,prNumber,ownerName,ownerAddress,ownerEmailId,ownerPinCode,mobileNo,vehicleType,transType,vehicleClassType,mfrsName,modelName,hsrpFee,oldNewFlag,govtVehicleTag,timeStamp,trNumber,dealerName,dealerMail,dealerRtoCode) values('" + value.Data[0].rtoCode.ToString() + "',	'" + value.Data[0].rtoName.ToString() + "',	'" + value.Data[0].affixationCenterCode.ToString() + "',	'" + value.Data[0].transactionNo.ToString() + "',	'" + value.Data[0].transactionDate.ToString() + "',	'" + value.Data[0].authorizationRefNo.ToString() + "',	'" + value.Data[0].authorizationDate.ToString() + "',	'" + value.Data[0].engineNo.ToString() + "',	'" + value.Data[0].chassisNo.ToString() + "',	'" + value.Data[0].prNumber.ToString() + "',	'" + value.Data[0].ownerName.ToString() + "',	'" + value.Data[0].ownerAddress.ToString() + "',	'" + value.Data[0].ownerEmailId.ToString() + "',	'" + value.Data[0].ownerPinCode.ToString() + "',	'" + value.Data[0].mobileNo.ToString() + "',	'" + value.Data[0].vehicleType.ToString() + "',	'" + value.Data[0].transType.ToString() + "',	'" + value.Data[0].vehicleClassType.ToString() + "',	'" + value.Data[0].mfrsName.ToString() + "',	'" + value.Data[0].modelName.ToString() + "',	'" + value.Data[0].hsrpFee.ToString() + "',	'" + value.Data[0].oldNewFlag.ToString() + "',	'" + value.Data[0].govtVehicleTag.ToString() + "',	'" + value.Data[0].timeStamp.ToString() + "',	'" + value.Data[0].trNumber.ToString() + "',	'" + value.Data[0].dealerName.ToString() + "',	'" + value.Data[0].dealerMail.ToString() + "',	'" + value.Data[0].dealerRtoCode.ToString() + "')";
                        //int i = Utils.ExecNonQuery(SQLQuery, CnnString);
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
            string filename = "postHsrpRecords-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".txt";
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
