using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace APCollectionWeb
{
    public partial class HPCollection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HPservice();
        }
        string cnnLocal = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringLocal"].ToString();

        protected void HPservice()
        {
            try
            {
                double exciseamt = 0.0;
                double exciscess = 0.0;
                double excisshecess = 0.0;
                double excisebasic = 0.0;

                DataTable dt = new DataTable();
                WebReference_HP.SalesRequestWS_Service service = new WebReference_HP.SalesRequestWS_Service();
                service.UseDefaultCredentials = false;
                service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");

                label1.Text = "Start...";
                WebReference_HP.SalesRequestWS Cust;
                int count = 0;
                string query = "select SaveMacAddress,HSRPRecordID AS [HSRPRecordID],HSRP_StateID AS [StateCode],CashReceiptNo AS [OrderBookingNo.],convert(varchar(15),HSRPRecord_CreationDate,103) AS [OrderBookingDate],HSRPRecord_AuthorizationNo AS [AuthorizationNo.],convert(varchar(15),HSRPRecord_Authorizationdate,103) AS [AuthorizationDate],OwnerName AS [OwnerName],OwnerFatherName AS [OwnerFatherName],Address1 AS [OwnerAddress],PIN AS [OwnerPincode],MobileNo AS [MobileNo.],LandlineNo AS [LandLineNo.],EmailID AS [E-mailID],VehicleRegNo AS [VehicleRegistrationNo.],case addrecordby when  'Dealer' then 2 else 1 end  AS [RTO/DealerLocationType],case addrecordby when 'Dealer' then (select erpclientcode from dealermaster where erpclientcode is not null and erpclientcode !='HOLD' and dealerid=dealerid) else 'CUST/001' end as [Customer/DealerCode], UPPER([VehicleClass]) AS VehicleClass, upper([VehicleType]) AS VehicleType,  upper([ORDERType]) AS ORDERType, [ManufacturerName], [ManufacturerModel], ManufacturingYear AS [ManufacturingYear], Vehiclecolor AS [VehicleColor], upper(ChassisNo) AS [ChassisNo.], upper(EngineNo) AS [EngineNo.], IsFrontPlateSize AS [IsFrontPlateSize], (SELECT PRODUCTCODE FROM Product WHERE ProductID=FrontPlateSize) AS [FrontPlateSize], IsRearPlateSize AS [IsRearPlateSize],(SELECT PRODUCTCODE FROM Product WHERE ProductID=RearPlateSize) AS [RearPlateSize], StickerMandatory AS [StickerMandatory], ScrewPrize AS [ScrewPrice], TotalAmount AS [TotalAmount], VAT_Percentage AS [Vat], VAT_Amount AS [VatAmount], NetAmount AS [NetAmount], RoundOff_NetAmount AS [NetAmountRoundoff], '1753-01-01 00:00:00.000' AS [TimeTakentoFill], [CreatedBy], (select NAVCentralWarehouseCode from rtolocation where rtolocationid =a.RTOLocationID) AS [CentralWarehouseCode],   (select distrelation from rtolocation where rtolocationid =a.RTOLocationID) AS [RTO/CollectionCenterCode], (select NAVusercode from RTOLocation where  RTOLocationID=a.RTOLocationID)  AS [CollectionAgentCode],  (select Navzonecode from rtolocation where rtolocationid =a.RTOLocationID) AS [ZoneCode], CashReceiptNo AS [CashReceiptNo.], convert(varchar(15),HSRPRecord_CreationDate,103) AS [CashReceiptDate],OperatorID AS [OperatorID], (select NAVEMBID from rtolocation where rtolocationid =a.RTOLocationID) AS [EmbosssingCenterCode], ('AFX'+convert(varchar(15),rtolocationid)) AS [AffixationCenterCode],Remarks AS [Remarks], UserRTOLocationID AS [UserRTOLocationID], (case when isvip='Y' then 1 else 0 end) AS [IsVIP],OldRegPlate AS [No.ofOldReg.Plate], CashReceiptNo_HHT AS [CashReceiptNo_HHT], MPDataUploaded AS [MPDataUploaded], 0 AS [IsReplacementRequest], 'HP' AS [ResponsibilityCenter], 0 AS [RejectionRequestID], (select NAVID from vehicletype where vehicletype=a.vehicletype)as vehicleSubType,addrecordby,convert(varchar(15),cashreceiptdatetime_hht,103) as CashReceiptdate_HHT from hsrprecords a WITH (NOLOCK)  where  hsrp_stateid =3 and HSRPRecord_CreationDate >= '2015/01/19 00:00:00' and RoundOff_NetAmount > 0 and ScrewPrize >0 and (vehicleregno is not null and vehicleregno !='' ) and len(vehicleregno)<=10 and sendtoerp is null and orderstatus='New Order' ORDER BY HSRPRECORDID";
                
                dt = utils.GetDataTable(query, cnnLocal);
                count = dt.Rows.Count;
                label1.Text = Convert.ToString(count);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Cust = new WebReference_HP.SalesRequestWS();
                        Cust.AddRecordBy = dt.Rows[i]["RTO/DealerLocationType"].ToString();

                        if (dt.Rows[i]["vehicleSubType"].ToString() == "1")
                            Cust.Vehicle_Sub_Type = WebReference_HP.Vehicle_Sub_Type.Two_Wheeler;
                        else if (dt.Rows[i]["vehicleSubType"].ToString() == "2")
                            Cust.Vehicle_Sub_Type = WebReference_HP.Vehicle_Sub_Type.Three_Wheeler;
                        else if (dt.Rows[i]["vehicleSubType"].ToString() == "3")
                            Cust.Vehicle_Sub_Type = WebReference_HP.Vehicle_Sub_Type.Four_Wheeler;
                        else if (dt.Rows[i]["vehicleSubType"].ToString() == "4")
                            Cust.Vehicle_Sub_Type = WebReference_HP.Vehicle_Sub_Type.Tractor;

                        Cust.Web_Sync = true;
                        Cust.Web_SyncSpecified = true;

                        Cust.Vehicle_Sub_TypeSpecified = true;
                        Cust.Order_Booking_Date = DateTime.ParseExact(dt.Rows[i]["OrderBookingDate"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        Cust.Order_Booking_DateSpecified = true;
                        Cust.Order_Booking_No = dt.Rows[i]["OrderBookingNo."].ToString();
                        Cust.Authorization_Date = DateTime.ParseExact(dt.Rows[i]["AuthorizationDate"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        Cust.Authorization_DateSpecified = true;
                        Cust.Authorization_No = dt.Rows[i]["AuthorizationNo."].ToString();
                        Cust.Cash_Receipt_Date = DateTime.ParseExact(dt.Rows[i]["OrderBookingDate"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        Cust.Cash_Receipt_DateSpecified = true;
                        Cust.Cash_Receipt_No = dt.Rows[i]["CashReceiptNo."].ToString();
                        Cust.CashReceiptNo_HHT = dt.Rows[i]["CashReceiptNo_HHT"].ToString();
                        Cust.Central_Warehouse_Code = dt.Rows[i]["CentralWarehouseCode"].ToString();
                        Cust.Chassis_No = dt.Rows[i]["ChassisNo."].ToString();
                        Cust.Collection_Agent_Code = dt.Rows[i]["CollectionAgentCode"].ToString(); ;
                        string userid = dt.Rows[i]["CreatedBy"].ToString();
                        string rectype = dt.Rows[i]["addrecordby"].ToString();

                        if (userid == "" && rectype == "Dealer")
                            Cust.Created_By = 2063; 
                        else if (userid == "" && rectype == "Agent")
                            Cust.Created_By = 47;
                        else
                            Cust.Created_By = Convert.ToInt32(dt.Rows[i]["CreatedBy"].ToString());


                        Cust.Created_BySpecified = true;

                        Cust.E_mail_ID = dt.Rows[i]["E-mailID"].ToString();
                        Cust.Engine_No = dt.Rows[i]["EngineNo."].ToString();

                        Cust.FrontPlateSize = dt.Rows[i]["FrontPlateSize"].ToString();
                        Cust.HSRP_Record_ID = Convert.ToInt32(dt.Rows[i]["HSRPRecordID"].ToString());
                        Cust.HSRP_Record_IDSpecified = true;

                        if (dt.Rows[i]["IsReplacementRequest"].ToString() == "1")
                            Cust.Is_Replacement_Request = true;
                        else
                            Cust.Is_Replacement_Request = false;

                        Cust.Is_Replacement_RequestSpecified = true;
                        Cust.IsFrontPlateSize = dt.Rows[i]["IsFrontPlateSize"].ToString();
                        Cust.IsRearPlateSize = dt.Rows[i]["IsRearPlateSize"].ToString();

                        if (dt.Rows[i]["ISVIP"].ToString() == "0")
                            Cust.IsVIP = false;
                        else if (dt.Rows[i]["ISVIP"].ToString() == "1")
                            Cust.IsVIP = true;

                        Cust.IsVIPSpecified = true;
                        Cust.Land_Line_No = dt.Rows[i]["LandLineNo."].ToString();
                        Cust.Manufacturer_Model = dt.Rows[i]["ManufacturerModel"].ToString();
                        Cust.Manufacturer_Name = dt.Rows[i]["ManufacturerName"].ToString();
                        Cust.Manufacturing_Year = dt.Rows[i]["ManufacturingYear"].ToString();
                        Cust.Mobile_No = dt.Rows[i]["MobileNo."].ToString();
                        Cust.Net_Amount = Convert.ToDecimal(dt.Rows[i]["NetAmount"].ToString());
                        Cust.Net_AmountSpecified = true;
                        Cust.Net_Amount_Round_off = Convert.ToInt32(dt.Rows[i]["NetAmountRoundoff"].ToString());
                        Cust.Net_Amount_Round_offSpecified = true;

                        Cust.No_of_Old_Reg_Plate = dt.Rows[i]["No.ofOldReg.Plate"].ToString();
                        Cust.Operator_ID = dt.Rows[i]["operatorid"].ToString();
                        if (dt.Rows[i]["ORDERType"].ToString() == "DB")
                            Cust.Order_Type = WebReference_HP.Order_Type.DB;
                        else if (dt.Rows[i]["ORDERType"].ToString() == "OB")
                            Cust.Order_Type = WebReference_HP.Order_Type.OB;
                        else if (dt.Rows[i]["ORDERType"].ToString() == "NB")
                            Cust.Order_Type = WebReference_HP.Order_Type.NB;
                        else if (dt.Rows[i]["ORDERType"].ToString() == "OS")
                            Cust.Order_Type = WebReference_HP.Order_Type.OS;
                        else if (dt.Rows[i]["ORDERType"].ToString() == "DR")
                            Cust.Order_Type = WebReference_HP.Order_Type.DR;
                        else if (dt.Rows[i]["ORDERType"].ToString() == "DF")
                            Cust.Order_Type = WebReference_HP.Order_Type.DF;

                        Cust.Order_TypeSpecified = true;
                        Cust.Owner_Address = dt.Rows[i]["OwnerAddress"].ToString();
                        Cust.Owner_Father_Name = dt.Rows[i]["OwnerFatherName"].ToString();
                        string oname = string.Empty;
                        oname = dt.Rows[i]["OwnerName"].ToString().Trim();
                        if (oname.Length > 49)
                        {
                            oname = oname.Substring(0, 49);
                            Cust.Owner_Name = oname;
                        }
                        else
                        {
                            Cust.Owner_Name = oname;
                        }


                        excisebasic = Math.Round(Convert.ToDouble(dt.Rows[i]["TotalAmount"].ToString()) / 1.125, 2);
                        exciseamt = Math.Round(Convert.ToDouble(dt.Rows[i]["TotalAmount"].ToString()) - excisebasic, 2);

                        Cust.Excise_Amount = Convert.ToDecimal(exciseamt);
                        Cust.Excise_AmountSpecified = true;
                        Cust.Base_Price = Convert.ToDecimal(excisebasic);
                        Cust.Base_PriceSpecified = true;


                        Cust.Owner_Pincode = dt.Rows[i]["OwnerPincode"].ToString();
                        Cust.Rear_Plate_Size = dt.Rows[i]["RearPlateSize"].ToString();
                        Cust.Remarks = dt.Rows[i]["remarks"].ToString();
                        Cust.Responsibility_Center = "HP";
                        Cust.RTO_Collection_Center_Code = dt.Rows[i]["RTO/CollectionCenterCode"].ToString();

                        string dealername = dt.Rows[i]["RTO/DealerLocationType"].ToString();
                        //CUST/001


                        if (dealername == "2")
                        {
                            Cust.RTO_Dealer_Location_Type = WebReference_HP.RTO_Dealer_Location_Type.Dealer;
                            string dd = dt.Rows[i]["Customer/DealerCode"].ToString();
                            if (dd == "")
                                Cust.Customer_Dealer_Code = "CUST/327";
                            else
                                Cust.Customer_Dealer_Code = dt.Rows[i]["Customer/DealerCode"].ToString();


                            //"CUST/114
                            dealername = "";
                            string dealerEMb = "select code from dealercenter a inner join dealermaster as b on b.dealerid=a.dealerid where centertype ='EMB' and b.erpclientcode ='" + dd + "'";
                            dealername = utils.getDataSingleValue(dealerEMb, cnnLocal, "code");
                            dealerEMb = "select code from dealercenter a inner join dealermaster as b on b.dealerid=a.dealerid where centertype ='AFX' and b.erpclientcode ='" + dd + "'";
                            string dealername1 = utils.getDataSingleValue(dealerEMb, cnnLocal, "code");
                            if (dealername == "0" && dealername1 == "0")
                            {

                                Cust.Embosssing_Center_Code = dt.Rows[i]["EmbosssingCenterCode"].ToString();
                                Cust.Affixation_Center_Code = dt.Rows[i]["AffixationCenterCode"].ToString();
                            }
                            else if (dealername == "0" && dealername1 != "0")
                            {
                                Cust.Embosssing_Center_Code = dt.Rows[i]["EmbosssingCenterCode"].ToString();
                                Cust.Affixation_Center_Code = dealername1;
                            }
                            else if (dealername1 == "0" && dealername != "0")
                            {
                                Cust.Embosssing_Center_Code = dealername1;
                                Cust.Affixation_Center_Code = dt.Rows[i]["AffixationCenterCode"].ToString();
                            }
                            else
                            {
                                Cust.Embosssing_Center_Code = dealername;
                                Cust.Affixation_Center_Code = dealername1;
                            }
                        }
                        else
                        {
                            Cust.RTO_Dealer_Location_Type = WebReference_HP.RTO_Dealer_Location_Type.RTO;
                            Cust.Embosssing_Center_Code = dt.Rows[i]["EmbosssingCenterCode"].ToString();
                            Cust.Affixation_Center_Code = dt.Rows[i]["AffixationCenterCode"].ToString();
                            Cust.Customer_Dealer_Code = "CUST/001";
                        }



                        Cust.RTO_Dealer_Location_TypeSpecified = true;
                        Cust.SaveMacAddress = dt.Rows[i]["SaveMacAddress"].ToString();
                        Cust.Screw_Price = Convert.ToDecimal(dt.Rows[i]["ScrewPrice"].ToString());
                        Cust.Screw_PriceSpecified = true;

                        Cust.State_Code = Convert.ToInt32(dt.Rows[i]["StateCode"].ToString());
                        Cust.State_CodeSpecified = true;

                        Cust.StickerMandatory = dt.Rows[i]["StickerMandatory"].ToString();

                        Cust.Total_Amount = Convert.ToDecimal(dt.Rows[i]["TotalAmount"].ToString());
                        Cust.Total_AmountSpecified = true;
                        Cust.UserRTOLocationID = Convert.ToInt32(dt.Rows[i]["userrtolocationid"].ToString());
                        Cust.UserRTOLocationIDSpecified = true;
                        Cust.Vat_Amount = Convert.ToDecimal(dt.Rows[i]["VatAmount"].ToString());
                        Cust.Vat_AmountSpecified = true;
                        Cust.Vat_Percent = Convert.ToDecimal(dt.Rows[i]["Vat"].ToString());
                        Cust.Vat_PercentSpecified = true;

                        string vclass = dt.Rows[i]["VehicleClass"].ToString();
                        if (dt.Rows[i]["VehicleClass"].ToString() == "TRANSPORT")
                            Cust.Vehicle_Class = WebReference_HP.Vehicle_Class.TRANSPORT;
                        else
                            Cust.Vehicle_Class = WebReference_HP.Vehicle_Class.NON_TRANSPORT;

                        Cust.Vehicle_ClassSpecified = true;
                        Cust.Vehicle_Color = dt.Rows[i]["VehicleColor"].ToString();
                        Cust.Vehicle_Registration_No = dt.Rows[i]["VehicleRegistrationNo."].ToString();


                        if (dt.Rows[i]["VehicleType"].ToString() == "LMV")
                            Cust.Vehicle_Type = WebReference_HP.Vehicle_Type.LMV;
                        else if (dt.Rows[i]["VehicleType"].ToString() == "LMV(CLASS)")
                            Cust.Vehicle_Type = WebReference_HP.Vehicle_Type.LMV_CLASS;
                        else if (dt.Rows[i]["VehicleType"].ToString() == "MCV/HCV/TRAILERS")
                            Cust.Vehicle_Type = WebReference_HP.Vehicle_Type.MCV_HCV_TRAILERS;
                        else if (dt.Rows[i]["VehicleType"].ToString() == "MOTOR CYCLE")
                            Cust.Vehicle_Type = WebReference_HP.Vehicle_Type.MOTOR_CYCLE;
                        else if (dt.Rows[i]["VehicleType"].ToString() == "SCOOTER")
                            Cust.Vehicle_Type = WebReference_HP.Vehicle_Type.SCOOTER;
                        else if (dt.Rows[i]["VehicleType"].ToString() == "THREE WHEELER")
                            Cust.Vehicle_Type = WebReference_HP.Vehicle_Type.THREE_WHEELER;
                        else if (dt.Rows[i]["VehicleType"].ToString() == "TRACTOR")
                            Cust.Vehicle_Type = WebReference_HP.Vehicle_Type.TRACTOR;

                        Cust.Vehicle_TypeSpecified = true;
                        Cust.Zone_Code = dt.Rows[i]["ZoneCode"].ToString();

                        excisebasic = Math.Round(Convert.ToDouble(dt.Rows[i]["TotalAmount"].ToString()) / 1.125, 2);
                        //exciseamt = Math.Round((excisebasic * 12) / 100, 2);
                        exciseamt = Math.Round(Convert.ToDouble(dt.Rows[i]["TotalAmount"].ToString()) - excisebasic, 2);
                        exciscess = Math.Round(exciseamt * 2 / 100, 2);
                        excisshecess = Math.Round(exciseamt / 100, 2);

                        utils.ExecNonQuery("update hsrprecords set sendtoerp =2,cessamt='" + exciscess + "', shecessamt='" + excisshecess + "', EXCISEAMT='" + exciseamt + "' ,EXCISEBASIC='" + excisebasic + "' where  hsrprecordid ='" + dt.Rows[i]["hsrprecordid"].ToString() + "' ", cnnLocal);
                        try
                        {

                            service.Create(ref Cust);
                        }
                        catch (Exception ex)
                        {
                            utils.ExecNonQuery("update hsrprecords set sendtoerp =1 where  hsrprecordid ='" + dt.Rows[i]["hsrprecordid"].ToString() + "' ", cnnLocal);
                            //utils.ExecNonQuery("update hsrprecords set sendtoerp =NULL where  hsrprecordid ='" + dt.Rows[i]["hsrprecordid"].ToString() + "' and isnull(sendtoerp,'')=1 and hsrp_stateid=3", cnnLocal);
                            continue;
                        }
                        utils.ExecNonQuery("update hsrprecords set sendtoerp =1,EXCISEAMT='" + exciseamt + "' ,EXCISEBASIC='" + excisebasic + "' where  hsrprecordid ='" + dt.Rows[i]["hsrprecordid"].ToString() + "' ", cnnLocal);
                    }
                }
                else
                {
                    label1.Text = "There is no record.......";
                }
            }
            catch (Exception ex)
            {

                AddLog(ex.Message.ToString());
            }

        }
        static void AddLog(string logText)
        {
            string filename = "HPBankProcess-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["TGpathx"].ToString();
            fileLocation += filename;
            using (StreamWriter sw = File.AppendText(fileLocation))
            {
                sw.WriteLine("-------------------" + System.DateTime.Now.ToString() + "--------------------");
                sw.WriteLine(logText);
                sw.WriteLine("-----------------------------------------------------------------------------");
                sw.Close();
            }

            //string pathx = "D:\\ERPHSRPLog\\HPCollection-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
            //using (StreamWriter sw = File.AppendText(pathx))
            //{
            //    sw.WriteLine("-------------------" + System.DateTime.Now.ToString() + "--------------------");
            //    sw.WriteLine(logText);
            //    sw.WriteLine("-----------------------------------------------------------------------------");
            //    sw.Close();
            //}
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            HPservice();
        }
    }
}