using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DataProvider
{
    public class BAL
    {

        public BAL()
        {

        }

        public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        public bool FillHSRPRecordDetail(string HSRPAuthNo, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[FillHSRPRecordDetail]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPAuthNo", HSRPAuthNo));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(ds);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPRecords(string HSRPRecord_AuthorizationNo, DateTime HSRPRecord_AuthorizationDate, string OrderNo,
         DateTime OrderDate, String OwnerName, String OrderStatus, string Address1, string Address2, string MobileNo, string LandlineNo,
         string EmailID, string VehicleClass, string ManufacturerName, string VehicleType, string VehicleRegNo, string EngineNo,
         string ChassisNo, string OrderType, string ISFrontPlateSize, string FrontPlateSize, string ISRearPlateSize,
         string RearPlateSize, string StickerMandatory, string InvoiceNo, string CashReceiptNo, string VAT_Percentage,
         string VAT_Amount, string ServiceTax_Percentage, string ServiceTax_Amount, string TotalAmount, string NetAmount)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_HSRPRecordInsert]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_AuthorizationNo", HSRPRecord_AuthorizationNo));
                cmd.Parameters.Add(new SqlParameter("@HSRPRecord_AuthorizationDate", HSRPRecord_AuthorizationDate));
                cmd.Parameters.Add(new SqlParameter("@OrderNo", OrderNo));
                cmd.Parameters.Add(new SqlParameter("@OrderDate", OrderDate));
                cmd.Parameters.Add(new SqlParameter("@OwnerName", OwnerName));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@Address1", Address1));
                cmd.Parameters.Add(new SqlParameter("@Address2", Address2));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", MobileNo));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", LandlineNo));
                cmd.Parameters.Add(new SqlParameter("@EmailID", EmailID));
                cmd.Parameters.Add(new SqlParameter("@VehicleClass", VehicleClass));
                cmd.Parameters.Add(new SqlParameter("@ManufacturerName", ManufacturerName));
                cmd.Parameters.Add(new SqlParameter("@VehicleType", VehicleType));
                cmd.Parameters.Add(new SqlParameter("@VehicleRegNo", VehicleRegNo));
                cmd.Parameters.Add(new SqlParameter("@EngineNo", EngineNo));
                cmd.Parameters.Add(new SqlParameter("@ChassisNo", ChassisNo));
                cmd.Parameters.Add(new SqlParameter("@OrderType", OrderType));
                cmd.Parameters.Add(new SqlParameter("@ISFrontPlateSize", ISFrontPlateSize));
                cmd.Parameters.Add(new SqlParameter("@FrontPlateSize", FrontPlateSize));
                cmd.Parameters.Add(new SqlParameter("@ISRearPlateSize", ISRearPlateSize));
                cmd.Parameters.Add(new SqlParameter("@RearPlateSize", RearPlateSize));
                cmd.Parameters.Add(new SqlParameter("@StickerMandatory", StickerMandatory));
                cmd.Parameters.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
                cmd.Parameters.Add(new SqlParameter("@CashReceiptNo", CashReceiptNo));
                cmd.Parameters.Add(new SqlParameter("@VAT_Percentage", VAT_Percentage));
                cmd.Parameters.Add(new SqlParameter("@VAT_Amount", VAT_Amount));
                cmd.Parameters.Add(new SqlParameter("@ServiceTax_Percentage", ServiceTax_Percentage));
                cmd.Parameters.Add(new SqlParameter("@ServiceTax_Amount", ServiceTax_Amount));
                cmd.Parameters.Add(new SqlParameter("@TotalAmount", TotalAmount));
                cmd.Parameters.Add(new SqlParameter("@NetAmount", NetAmount));

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }


        public bool InsertHSRPState(string StateName, string ActiveStatus, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_HSRPStateInsert]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPStateName", StateName));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Update_Master_HSRPState(int HSRP_StateID, string StateName, string ActiveStatus, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_HSRPStateUpdate]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@HSRPStateName", StateName));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Insert_Master_HSRP_RTOLocation(int HSRP_StateID, string RTOLocationName, string RTOLocationCode, string RTOLocationAddress, string ContactPersonName, string MobileNo, string LandLineNo, string EmailID, string ActiveStatus)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_InsertRTOLocation]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationName", RTOLocationName));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationCode", RTOLocationCode));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationAddress", RTOLocationAddress));
                cmd.Parameters.Add(new SqlParameter("@ContactPersonName", ContactPersonName));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", MobileNo));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", LandLineNo));
                cmd.Parameters.Add(new SqlParameter("@EmailID", EmailID));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Update_Master_HSRP_RTOLocation(int RTOLocationID, int HSRP_StateID, string RTOLocationName, string RTOLocationCode, string RTOLocationAddress, string ContactPersonName, string MobileNo, string LandLineNo, string EmailID, string ActiveStatus, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_UpdateRTOLocation]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationName", RTOLocationName));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationCode", RTOLocationCode));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationAddress", RTOLocationAddress));
                cmd.Parameters.Add(new SqlParameter("@ContactPersonName", ContactPersonName));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", MobileNo));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", LandLineNo));
                cmd.Parameters.Add(new SqlParameter("@EmailID", EmailID));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPPruduct(string ProductCode, string ProductColor, string ActiveStatus, string ProductDimension, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_HSRPProductInsert]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));
                cmd.Parameters.Add(new SqlParameter("@ProductColor", ProductColor));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));
                cmd.Parameters.Add(new SqlParameter("@ProductDimension", ProductDimension));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateHSRPPruduct(int ProductID, string ProductCode, string ProductColor, string ActiveStatus, string ProductDimension, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_HSRPProductUpdate]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                cmd.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));
                cmd.Parameters.Add(new SqlParameter("@ProductColor", ProductColor));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));
                cmd.Parameters.Add(new SqlParameter("@ProductDimension", ProductDimension));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPPrefix(int HSRP_StateID, int RTOLocationID, string PrefixFor, string PrefixText, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_InsertPrefix]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@PrefixFor", PrefixFor));
                cmd.Parameters.Add(new SqlParameter("@PrefixText", PrefixText));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateHSRPPrefix(int SerialPrefixID, int HSRP_StateID, int RTOLocationID, string PrefixFor, string PrefixText, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_UpdatePrefix]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@SerialPrefixID", SerialPrefixID));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@PrefixFor", PrefixFor));
                cmd.Parameters.Add(new SqlParameter("@PrefixText", PrefixText));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateHSRPMessageTicker(int TickerID, string MessageText, string MessageTextURL, int HSRP_StateID, int RTOLocationID, DateTime UpdateDate, string ActiveStatus, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_UpdateMessageTicker]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MessageID", TickerID));
                cmd.Parameters.Add(new SqlParameter("@MessageText", MessageText));
                cmd.Parameters.Add(new SqlParameter("@MessageTextURL", MessageTextURL));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));

                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@UpdateDateTime", UpdateDate));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPMessageTicker(string MessageText, string MessageTextURL, int HSRP_StateID, int RTOLocationID, DateTime CreatedDateTime, string ActiveStatus, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Master_InsertMessageTicker]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MessageText", MessageText));
                cmd.Parameters.Add(new SqlParameter("@MessageTextURL", MessageTextURL));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", HSRP_StateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@CreatedDateTime", CreatedDateTime));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", ActiveStatus));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateHSRPSecurityQuestion(int QuestionID, int userID, string QuestionText, DateTime UpdateDate, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_UpdateSecurityQuestion]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@QuestionID", QuestionID));
                cmd.Parameters.Add(new SqlParameter("@userID", userID));
                cmd.Parameters.Add(new SqlParameter("@QuestionText", QuestionText));
                cmd.Parameters.Add(new SqlParameter("@UpdateDate", UpdateDate));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPSecurityQuestion(int userID, string QuestionText, DateTime CreateDate, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //                
                SqlCommand cmd = new SqlCommand("[Master_InsertSecurityQuestion]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@userID", userID));
                cmd.Parameters.Add(new SqlParameter("@QuestionText", QuestionText));
                cmd.Parameters.Add(new SqlParameter("@CreateDate", CreateDate));
                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateLaserAssigned(int StateID, int RTOLocationID, string OrderStatus, string FrondLaserCode, string RearLaserCode, int HSRPRecordID, string HSRP_Sticker_LaserCode, ref int ISExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //string sqlSticker, string sqlFrontLaser, string sqlRearPlate,string HSRP_Sticker_LaserCode, 
                SqlCommand cmd = new SqlCommand("[Transaction_UpdateAssignedLaser]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@HSRP_StateID", StateID));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationID", RTOLocationID));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Front_LaserCode", FrondLaserCode));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Rear_LaserCode", RearLaserCode));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Sticker_LaserCode", HSRP_Sticker_LaserCode));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Transaction_UpdateAssignedLaser_MakeFree(string FrontLaserCode, int HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Transaction_UpdateAssignedLaser_MakeFree]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Front_LaserCode", FrontLaserCode));
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //  ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public bool Transaction_UpdateAssignedLaserRear_MakeFree(string RearLaserCode, int HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Transaction_UpdateAssignedLaserRear_MakeFree]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Rear_LaserCode", RearLaserCode));
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //  ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateEmbossing(string OrderStatus, string FrontLaserCode, string Sticker, string RearLaserCode, int HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("[Transaction_UpdateEmbossing]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Front_LaserCode", FrontLaserCode));
                cmd.Parameters.Add(new SqlParameter("@Sticker", Sticker));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Rear_LaserCode", RearLaserCode));

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //  ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool PlateFrontRejectEmbossing(string OrderStatus, string FrondLaserCode, int HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_PlateFrontRejectEmbossing]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Front_LaserCode", FrondLaserCode));

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //  ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool PlateRearRejectEmbossing(string OrderStatus, string RearLaserCode, int HSRPRecordID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_RearFrontRejectEmbossing]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HSRPRecordID", HSRPRecordID));
                cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                cmd.Parameters.Add(new SqlParameter("@HSRP_Rear_LaserCode", RearLaserCode));
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //  ISExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InsertHSRPPBatch(int ProductID, int BatchCode, string LaserCodeFrom, string LaserCodeTo, DateTime DateofManufacturing, decimal CurrentCost, decimal Weight, int TotalBoxUnits, int NoOfPlateInBoxUnit, DateTime BatchReleasedDate, DateTime CreateDateTime, ref int IsExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_InsertBatch]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                cmd.Parameters.Add(new SqlParameter("@BatchCode", BatchCode));
                cmd.Parameters.Add(new SqlParameter("@LaserCodeFrom", LaserCodeFrom));
                cmd.Parameters.Add(new SqlParameter("@LaserCodeTo", LaserCodeTo));
                cmd.Parameters.Add(new SqlParameter("@DateofManufacturing", DateofManufacturing));
                cmd.Parameters.Add(new SqlParameter("@CurrentCost", CurrentCost));
                cmd.Parameters.Add(new SqlParameter("@Weight", Weight));
                cmd.Parameters.Add(new SqlParameter("@TotalBoxUnits", TotalBoxUnits));
                cmd.Parameters.Add(new SqlParameter("@NoOfPlateInBoxUnit", NoOfPlateInBoxUnit));
                cmd.Parameters.Add(new SqlParameter("@BatchReleasedDate", BatchReleasedDate));
                cmd.Parameters.Add(new SqlParameter("@CreateDateTime", CreateDateTime));

                SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                isexists.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    IsExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdateHSRPPBatch(int BatchID, int ProductID, int BatchCode, string LaserCodeFrom, string LaserCodeTo, DateTime DateofManufacturing, decimal CurrentCost, decimal Weight, int TotalBoxUnits, int NoOfPlateInBoxUnit, DateTime BatchReleasedDate, DateTime CreateDateTime, ref int IsExists)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_UpdateBatch]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@BatchID", BatchID));
                cmd.Parameters.Add(new SqlParameter("@ProductID", ProductID));
                cmd.Parameters.Add(new SqlParameter("@BatchCode", BatchCode));
                cmd.Parameters.Add(new SqlParameter("@LaserCodeFrom", LaserCodeFrom));
                cmd.Parameters.Add(new SqlParameter("@LaserCodeTo", LaserCodeTo));
                cmd.Parameters.Add(new SqlParameter("@DateofManufacturing", DateofManufacturing));
                cmd.Parameters.Add(new SqlParameter("@CurrentCost", CurrentCost));
                cmd.Parameters.Add(new SqlParameter("@Weight", Weight));
                cmd.Parameters.Add(new SqlParameter("@TotalBoxUnits", TotalBoxUnits));
                cmd.Parameters.Add(new SqlParameter("@NoOfPlateInBoxUnit", NoOfPlateInBoxUnit));
                cmd.Parameters.Add(new SqlParameter("@BatchReleasedDate", BatchReleasedDate));
                cmd.Parameters.Add(new SqlParameter("@CreateDateTime", CreateDateTime));

                //SqlParameter isexists = new SqlParameter("@ISExists", SqlDbType.Int);
                //isexists.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(isexists);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //IsExists = (int)isexists.Value;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }



        public int SaveBankTranction(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_SaveBankTransaction]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@DepositDate", DateTime.Parse(lst[0])));
                cmd.Parameters.Add(new SqlParameter("@BankName", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@BranchName", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@DepositAmount", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@DepositBy", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@StateID", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@RTOLocation", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@UserID", lst[7]));
               
                cmd.Parameters.Add(new SqlParameter("@BankSlipNo", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@Remarks", lst[9]));


                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }

        }

        public int SavePlantDetail(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_SavePlant]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@PlantAddress", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@PlantCity", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@PlantState", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@PlantZip", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@ContactPersonName", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@EmailID", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", lst[8]));

                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }

        public int InsertMachineOperatorProductivity(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_SaveMOProductivity]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MachineID", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@OperatorName", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@ProductID", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@Quantity", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@ScrapQuantity", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@ScrapWeight", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@Remarks", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@StateID", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@RTOLocation", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@UserID", lst[9]));
                cmd.Parameters.Add(new SqlParameter("@ProductivityDate", DateTime.Parse(lst[10])));


                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }

        public int UpdateBankTranction(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_UpdateBankTransaction]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@DepositDate", DateTime.Parse(lst[0])));
                cmd.Parameters.Add(new SqlParameter("@BankName", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@BranchName", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@DepositAmount", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@DepositBy", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@StateID", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@RTOLocation", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@UserID", lst[7]));
                
                cmd.Parameters.Add(new SqlParameter("@BankSlipNo", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@Remarks", lst[9]));
                cmd.Parameters.Add(new SqlParameter("@TransactionID", lst[10]));


                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }


        public int UpdatePlantDetail(List<string> lst)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Transaction_UpdatePlant]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@PlantAddress", lst[0]));
                cmd.Parameters.Add(new SqlParameter("@PlantCity", lst[1]));
                cmd.Parameters.Add(new SqlParameter("@PlantState", lst[2]));
                cmd.Parameters.Add(new SqlParameter("@PlantZip", lst[3]));
                cmd.Parameters.Add(new SqlParameter("@ContactPersonName", lst[4]));
                cmd.Parameters.Add(new SqlParameter("@MobileNo", lst[5]));
                cmd.Parameters.Add(new SqlParameter("@LandlineNo", lst[6]));
                cmd.Parameters.Add(new SqlParameter("@EmailID", lst[7]));
                cmd.Parameters.Add(new SqlParameter("@ActiveStatus", lst[8]));
                cmd.Parameters.Add(new SqlParameter("@PlantID", lst[9]));

                try
                {
                    con.Open();
                    i = cmd.ExecuteNonQuery();
                    return i;

                }
                catch (Exception)
                {
                    return i;
                }

            }
        }
    }
}
