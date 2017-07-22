using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookAPOnlineHSRPRecords
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            try
            {            
            string StrVehicleClassType = string.Empty;
            string RTOLocationID = string.Empty;
            string macbase = string.Empty;
            string USERID = "0";
            string cashrc = string.Empty;
            string sticker1 = string.Empty;
            string VIP = string.Empty;
            string StrVehicleType = string.Empty;
            string AffixationDate = string.Empty;
            string HsrpRecordid = string.Empty;
            int i;
            string strProvider = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();


            //string sqlquery = "Select affixationcentercode,(select Rtolocationname from rtolocation where affixationcentercode=AffixationRTOCode)as 'rtoName',affixationCenterCode,H.transactionNo,H.transactionDate,H.authorizationRefNo,authorizationDate,engineNo,chassisNo,prNumber,ownerName,ownerAddress,ownerEmailId,ownerPinCode,mobileNo,vehicleType,transType,vehicleClassType,mfrsName,modelName,hsrpFee,oldNewFlag,govtVehicleTag,timeStamp,trNumber,dealerName,dealerMail,dealerRtoCode,regDate from HsrpData H,Transaction_Stagging T where H.transactionNo=T.transactionNo and H.transactionDate=T.transactionDate and H.hsrpFee=T.amount and H.authorizationRefNo=T.authorizationRefNo and affixationCenterCode=affixationcentercode and isnull(prNumber,'')!='' and isnull(T.authorizationRefNo,'')!='' and isnull(T.transactionNo,'')!='' and  H.authorizationRefNo not in (Select hsrprecord_Authorizationno from hsrprecords where hsrp_stateid=9)"; --
            string sqlquery = "select distinct affixationcentercode,(select Rtolocationname from rtolocation where affixationcentercode=AffixationRTOCode)as 'rtoName',affixationCenterCode,H.transactionNo,H.transactionDate,H.authorizationRefNo,authorizationDate,engineNo,chassisNo,h.prNumber,ownerName,ownerAddress,ownerEmailId,ownerPinCode,mobileNo,Ltrim(Rtrim(vehicleType)) as 'vehicleType',transType,vehicleClassType,mfrsName,modelName,hsrpFee,oldNewFlag,govtVehicleTag,timeStamp,trNumber,dealerName,dealerMail,dealerRtoCode,h.regDate from hsrpdata h , APNewSOP_SBIMIS sb where h.transactionNo=sb.HSRPNO and H.hsrpFee=sb.amount and affixationCenterCode=affixationcentercode and sb.ISPAYMENTRECIEVED='Y'  and isnull(H.authorizationRefNo,'')!='' and isnull(H.transactionNo,'')!='' and isnull(sb.HSRPNO,'')!='' and isnull(h.prNumber,'')!='' and isnull(h.paymentstatus,'')!='W' and H.authorizationRefNo not in (Select hsrprecord_Authorizationno from hsrprecords where hsrp_stateid=9 and paymentgateway like '%newsop%')";

            DataTable dtRecords = Utils.GetDataTable(sqlquery, strProvider);
            if (dtRecords.Rows.Count > 0)
            {
                for (int k = 0; k < dtRecords.Rows.Count; k++)
                {
                    if (dtRecords.Rows[k]["affixationcentercode"].ToString().Trim() == "AP905")
                    {
                        try
                        {
                            string updatesqlquery = "select distinct top 1 RTOCode from hsrpdata h , APNewSOP_SBIMIS sb where h.transactionNo=sb.HSRPNO and H.hsrpFee=sb.amount and affixationCenterCode=affixationcentercode and sb.ISPAYMENTRECIEVED='Y' and isnull(H.authorizationRefNo,'')!='' and isnull(H.transactionNo,'')!='' and isnull(sb.HSRPNO,'')!='' and isnull(h.prNumber,'')!='' and affixationCentercode='AP905' and H.authorizationRefNo not in (Select hsrprecord_Authorizationno from hsrprecords where hsrp_stateid=9)";
                            DataTable dtrtocode = Utils.GetDataTable(updatesqlquery, strProvider);
                            if (dtrtocode.Rows.Count > 0)
                            {
                                RTOLocationID = "select top 1 isnull(RTOLocationID,114) as rtoCode from RTOLocation where HSRP_StateID=9 and ActiveStatus='Y' and Affixationrtocode='" + dtrtocode.Rows[0]["RTOCode"].ToString().Trim() + "'";
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                    }
                    else
                    {
                        RTOLocationID = "select top 1 isnull(RTOLocationID,114) as rtoCode from RTOLocation where HSRP_StateID=9 and ActiveStatus='Y' and Affixationrtocode='" + dtRecords.Rows[k]["affixationcentercode"].ToString() + "'";
                    }

                    RTOLocationID = Utils.getScalarValue(RTOLocationID, strProvider);
                    if (dtRecords.Rows[k]["vehicleClassType"].ToString().ToUpper() == "N")
                    {
                        StrVehicleClassType = "Non-Transport";
                    }
                    else if (dtRecords.Rows[k]["vehicleClassType"].ToString().ToUpper() == "T")
                    {
                        StrVehicleClassType = "Transport";
                    }
                    AffixationDate = Utils.getDataSingleValue("select [dbo].GetAffxDate_insert_new('9') as Date", strProvider, "Date");
                    string strquery = "select upper(ourValue) as ourValue from  [dbo].[Mapping_Vahan_HSRP_ap] where rtovalue ='" + dtRecords.Rows[k]["vehicleType"].ToString() + "'";

                    StrVehicleType = Utils.getDataSingleValue(strquery, strProvider, "ourValue");

                    string SQLString2 = "select dbo.hsrpplateamt ('9','" + StrVehicleType + "','" + StrVehicleClassType + "','" + dtRecords.Rows[k]["transType"].ToString() + "') as Amount";
                    DataTable dt1 = Utils.GetDataTable(SQLString2, strProvider);                 
                    ///----------------Check Wrong Payment Data ------------- 
                    string Amount = dt1.Rows[0]["Amount"].ToString().Trim();
                    
                    string strhsrpfee = dtRecords.Rows[k]["Hsrpfee"].ToString().Trim() +".00";

                    //if (decimal.Parse(strhsrpfee) >=  decimal.Parse(Amount))
                    if (Amount == strhsrpfee)
                    { 
                        if ((StrVehicleType == "MCV/HCV/TRAILERS") || (StrVehicleType == "LMV(CLASS)") || (StrVehicleType == "LMV") || (StrVehicleType == "THREE WHEELER"))
                        {
                            sticker1 = "Y";
                        }
                        else
                        {
                            sticker1 = "N";
                        }
                        VIP = "N";
                        cashrc = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='9' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                        cashrc = Utils.getScalarValue(cashrc, strProvider);
                        string sql = "exec [getPlatesData] '9','" + StrVehicleType + "','" + StrVehicleClassType + "', '" + dtRecords.Rows[k]["transType"].ToString() + "'";
                        DataTable dt = Utils.GetDataTable(sql, strProvider);
                        if (dt.Rows.Count > 0)
                        {
                            label1.Visible = false;
                        }                   
                        //string strtype = dtRecords.Rows[k]["vehicleType"].ToString();

                        //--------------Start GST Code--------------------------------

                        //string sqlq = " select  SGSTPer,	CGSTPer    from Tax where hsrp_stateid = 9";
                        string sqlq = "select  SGSTPer,SGSTAmount,CGSTPer,CGSTAmount,GSTBasicAmount,Roundoff_value  from tax where HSRP_StateID=9 and OrderType = '" + dtRecords.Rows[k]["transType"].ToString() + "' and VehicleType ='" + StrVehicleType.ToString() + "' and isnull(GSTBasicAmount,0)!=0";
                        DataTable dtgstper = Utils.GetDataTable(sqlq, strProvider);

                        decimal SGSTPer;
                        decimal SGSTPeanmt;
                        decimal CGSTPer;
                        decimal CGSTPeramt;
                        decimal GSTBasicPeanmt;
                        decimal Roundoffvalue;
                        if (dtgstper.Rows.Count > 0)
                        {
                            SGSTPer = decimal.Round(Convert.ToDecimal(dtgstper.Rows[0]["SGSTPer"].ToString()),2);
                            SGSTPeanmt = decimal.Round(Convert.ToDecimal(dtgstper.Rows[0]["SGSTAmount"].ToString()),2);
                            CGSTPer = decimal.Round(Convert.ToDecimal(dtgstper.Rows[0]["CGSTPer"].ToString()),2);
                            CGSTPeramt = decimal.Round(Convert.ToDecimal(dtgstper.Rows[0]["CGSTAmount"].ToString()),2);
                            GSTBasicPeanmt = decimal.Round(Convert.ToDecimal(dtgstper.Rows[0]["GSTBasicAmount"].ToString()),2);
                            Roundoffvalue = decimal.Round(Convert.ToDecimal(dtgstper.Rows[0]["Roundoff_value"].ToString()),2);
                        }
                        else
                        {
                            label1.Text = "Please  Contact to Administor.";                            
                            return;
                        }

                        //--------End GST Code

                        using (SqlConnection con = new SqlConnection(strProvider))
                        {
                            using (SqlCommand cmd = new SqlCommand("InsertAPOnlineRcords"))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@HSRP_StateID", 9);
                                cmd.Parameters.AddWithValue("@RTOLocationID", RTOLocationID);
                                cmd.Parameters.AddWithValue("@HSRPRecord_AuthorizationNo", dtRecords.Rows[k]["authorizationRefNo"].ToString());
                                cmd.Parameters.AddWithValue("@HSRPRecord_AuthorizationDate", dtRecords.Rows[k]["authorizationDate"].ToString());
                                cmd.Parameters.AddWithValue("@OwnerName", dtRecords.Rows[k]["ownerName"].ToString());
                                cmd.Parameters.AddWithValue("@Address1", dtRecords.Rows[k]["ownerAddress"].ToString());
                                cmd.Parameters.AddWithValue("@MobileNo", dtRecords.Rows[k]["mobileNo"].ToString());
                                cmd.Parameters.AddWithValue("@EmailID", dtRecords.Rows[k]["ownerEmailId"].ToString());
                                cmd.Parameters.AddWithValue("@Pin", dtRecords.Rows[k]["ownerPinCode"].ToString());
                                cmd.Parameters.AddWithValue("@OrderType", dtRecords.Rows[k]["transType"].ToString());
                                cmd.Parameters.AddWithValue("@VehicleClass", StrVehicleClassType);                            
                                cmd.Parameters.AddWithValue("@VehicleType", StrVehicleType);
                                cmd.Parameters.AddWithValue("@ManufacturerName", dtRecords.Rows[k]["mfrsName"].ToString());
                                cmd.Parameters.AddWithValue("@ManufacturerModel", dtRecords.Rows[k]["modelName"].ToString());
                                cmd.Parameters.AddWithValue("@ChassisNo", dtRecords.Rows[k]["chassisNo"].ToString());
                                cmd.Parameters.AddWithValue("@EngineNo", dtRecords.Rows[k]["engineNo"].ToString());
                                cmd.Parameters.AddWithValue("@VehicleRegNo", dtRecords.Rows[k]["prNumber"].ToString());
                                cmd.Parameters.AddWithValue("@ISFrontPlateSize", dt.Rows[0]["frontplateflag"].ToString());
                                cmd.Parameters.AddWithValue("@FrontPlateSize", dt.Rows[0]["frontplateID"].ToString());
                                cmd.Parameters.AddWithValue("@FrontplatePrize", dt.Rows[0]["FrontPlateCost"].ToString());
                                cmd.Parameters.AddWithValue("@ISRearPlateSize", dt.Rows[0]["rearplateflag"].ToString());
                                cmd.Parameters.AddWithValue("@RearPlateSize", dt.Rows[0]["RearPlateID"].ToString());
                                cmd.Parameters.AddWithValue("@RearPlatePrize", dt.Rows[0]["rearplatecost"].ToString());
                                cmd.Parameters.AddWithValue("@StickerPrize", dt.Rows[0]["stickercost"].ToString());
                                cmd.Parameters.AddWithValue("@ScrewPrize", dt.Rows[0]["snaplockcost"].ToString());
                                cmd.Parameters.AddWithValue("@TotalAmount", dt.Rows[0]["totalamount"].ToString());
                                cmd.Parameters.AddWithValue("@VAT_Percentage", dt.Rows[0]["vatper"].ToString());
                                cmd.Parameters.AddWithValue("@NetAmount", Amount);
                                cmd.Parameters.AddWithValue("@CreatedBy", USERID);
                                cmd.Parameters.AddWithValue("@dealerid", dtRecords.Rows[k]["dealerRtoCode"].ToString());
                                cmd.Parameters.AddWithValue("@VAT_Amount", dt.Rows[0]["vatamount"].ToString());
                                cmd.Parameters.AddWithValue("@SaveMacAddress", macbase);
                                cmd.Parameters.AddWithValue("@isVIP", VIP);
                                cmd.Parameters.AddWithValue("@RoundOff_NetAmount", Convert.ToInt32(dtRecords.Rows[k]["hsrpfee"]));
                                cmd.Parameters.AddWithValue("@OnlinePaymentID", dtRecords.Rows[k]["transactionNo"].ToString());
                                cmd.Parameters.AddWithValue("@RTOName", dtRecords.Rows[k]["rtoName"].ToString());
                                cmd.Parameters.AddWithValue("@DealerName", dtRecords.Rows[k]["dealerName"].ToString());
                                cmd.Parameters.AddWithValue("@DealerMail", dtRecords.Rows[k]["dealerMail"].ToString());
                                cmd.Parameters.AddWithValue("@AffixationCenterAddress", dtRecords.Rows[k]["rtoName"].ToString());
                                cmd.Parameters.AddWithValue("@CashReceiptNo", cashrc);
                                cmd.Parameters.AddWithValue("@StickerMandatory", sticker1);
                                cmd.Parameters.AddWithValue("@Affix_Id", RTOLocationID);
                                cmd.Parameters.AddWithValue("@UserRTOLocationID", RTOLocationID);
                                cmd.Parameters.AddWithValue("@PlateAffixationDate", AffixationDate);
                                //add SGST
                                cmd.Parameters.AddWithValue("@SGSTPer", SGSTPer);
                                cmd.Parameters.AddWithValue("@SGSTAmount", SGSTPeanmt);
                                cmd.Parameters.AddWithValue("@CGSTPer", CGSTPer);
                                cmd.Parameters.AddWithValue("@CGSTAmount", CGSTPeramt);
                                cmd.Parameters.AddWithValue("@GSTBasicAmount", GSTBasicPeanmt);
                                cmd.Parameters.AddWithValue("@Roundoff_value", Roundoffvalue);                               

                                SqlParameter objSQLParm = new SqlParameter("@Result", SqlDbType.VarChar, 100);
                                objSQLParm.Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(objSQLParm);

                                cmd.Connection = con;
                                con.Open();

                                i = cmd.ExecuteNonQuery();
                                if (i != -1)
                                {
                                    HsrpRecordid = (string)objSQLParm.Value;
                                    con.Close();
                                    if (i > 0)
                                    {
                                        try
                                        {
                                            string Query = string.Empty;
                                            string Json = "{\"transactionNo\":\"" + dtRecords.Rows[k]["transactionNo"].ToString() + "\",\"transactionStatus\":\"PR\",\"amount\":\"" + dtRecords.Rows[k]["hsrpFee"].ToString() + "\",\"paymentReceivedDate\":\"" + System.DateTime.Now.ToString("dd/MM/yyyy") + "\",\"orderNo\":\"" + HsrpRecordid + "\",\"orderDate\":\"" + System.DateTime.Now.ToString("dd/MM/yyyy") + "\",\"authRefNo\":\"" + dtRecords.Rows[k]["authorizationRefNo"].ToString() + "\"}";
                                            List<Response> result = ConfirmPayment(Json, Login().ToString());
                                            Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                                            int u = Utils.ExecNonQuery(Query, strProvider);
                                            Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Invoice No'";
                                            u = Utils.ExecNonQuery(Query, strProvider);
                                            Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Delivery Challan No'";
                                            u = Utils.ExecNonQuery(Query, strProvider);
                                            string status = result[0].status.ToString();
                                            string message = result[0].message.ToString();
                                            Query = "update hsrprecords set APwebservicerespMsg='" + message + "', APNewSOPwebserviceresp = '" + status + "', APwebservicerespdate=getdate() where  HSRP_StateID='9' and hsrprecordid ='" + HsrpRecordid + "'";
                                            u = Utils.ExecNonQuery(Query, strProvider);
                                        }
                                        catch (Exception ex)
                                        {
                                            AddLog("***Records Not Update Successfully***- '" + dtRecords.Rows[k]["transactionNo"].ToString() + "' " + System.DateTime.Now.ToString());
                                            throw ex;
                                        }
                                        
                                    }
                                }                           
                            }
                        }
                    }
                    else
                    {
                        string sqle = "Update hsrpdata set paymentStatus='W' where transactionNo='" + dtRecords.Rows[k]["transactionNo"].ToString() + "' and hsrpfee='" + dtRecords.Rows[k]["HsrpFee"].ToString() + "' and authorizationRefNo='" + dtRecords.Rows[k]["authorizationRefNo"].ToString() + "'";
                        Utils.ExecNonQuery(sqle, strProvider);

                    }
                }
            }
            label1.Text = "Record Successfully '" + dtRecords.Rows.Count + "' ";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddLog(string logtext)
        {
            string filename = "BookPRTRNO-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".txt";
            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["TGpathx"].ToString();
            fileLocation += filename;
            using (StreamWriter sw = File.AppendText(fileLocation))
            {
                sw.WriteLine(logtext);
                sw.Close();
            }
        }

        public string Login()
        {
            //JSON :   {"username":"LAPLFORHSRP","password":"wE4FtNBoXwrmH1kr8wW/Vg=="}
            //string locationJSON = "{\"username\":\"LAPLFORHSRP\",\"password\":\"wE4FtNBoXwrmH1kr8wW/Vg==\"}";

            string locationJSON = "{\"username\":\"LAPLFORHSRP\",\"password\":\"admin\"}";

            var client = new RestClient("http://productionapp.trafficmanager.net:8080/registration/login");
            //var client = new RestClient("http://59.162.46.199:8181/registration/login");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Application/Json", locationJSON, ParameterType.RequestBody);
            var response2 = client.Execute<Login>(request);
            if (response2.StatusCode.ToString() == "OK")
            {
                var Token = response2.Data.token;
                return Token;
            }
            else
            {
                label1.Text = "Unauthorized";
                label1.ForeColor = Color.Red;
                return "Unauthorized";
            }
        }

        public List<Response> ConfirmPayment(string locationJSON, string token)
        {
        
            //var client = new RestClient("http://59.162.46.199:8080/registration/hsrp/notify/payment/");
            var client = new RestClient("http://productionapp.trafficmanager.net:8080/registration/hsrp/notify/payment/");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", token);
            request.AddParameter("Application/Json", locationJSON, ParameterType.RequestBody);
            var response2 = client.Execute<Response>(request);
            List<Response> list = new List<Response>();
            list.Add(new Response() { status = response2.Data.status.ToString(), message = response2.Data.message.ToString() });
            return list;
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            string strDate = DateTime.Now.ToString("hh:mm:ss tt");
            #region  In case of Local
            string strTime1 = "11:10:12 AM";

            if (DateTime.Parse(strDate) == DateTime.Parse(strTime1))
            {
            #endregion
                button1_Click(button1, new EventArgs());
            }
        }

        public void timer2_Tick(object sender, EventArgs e)
        {
            string strDate = DateTime.Now.ToString("hh:mm:ss tt");
            #region  In case of Local
            string strTime1 = "07:01:12 PM";

            if (DateTime.Parse(strDate) == DateTime.Parse(strTime1))
            {
            #endregion
                button1_Click(button1, new EventArgs());
            }
        }

        public void timer3_Tick(object sender, EventArgs e)
        {
            string strDate = DateTime.Now.ToString("hh:mm:ss tt");
            #region  In case of Local
            string strTime1 = "09:01:12 PM";

            if (DateTime.Parse(strDate) == DateTime.Parse(strTime1))
            {
            #endregion
                button1_Click(button1, new EventArgs());
            }
        }
    }
}
 