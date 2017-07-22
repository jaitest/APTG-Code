﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Net;


namespace HSRP.Transaction
{
    public partial class APReAssignAuthNo : System.Web.UI.Page
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string ProductivityID = string.Empty;
        string UserType = string.Empty;
        string UserName = string.Empty;
        string Sticker = string.Empty;
        string VIP = string.Empty;
        string USERID = string.Empty;
        DataTable dt = new DataTable();
        string macbase = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblSucMess.Visible = false;
                lblErrMess.Visible = false;
                if (Session["UserType"].ToString() == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    UserType = Session["UserType"].ToString();

                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserName = Session["UID"].ToString();
            USERID = Session["UID"].ToString();
            macbase = Session["MacAddress"].ToString();
            string SqlQuery = "exec GetTodayCollectionUserWise '" + HSRPStateID + "','" + RTOLocationID + "','" + USERID + "','" + System.DateTime.Today.ToString("yyyy-MM-dd") + "'";
            DataTable dtCount = Utils.GetDataTable(SqlQuery, ConnectionString);
            string strVehicle = dtCount.Rows[0]["TodayVehicleCount"].ToString();
            string strAmount = dtCount.Rows[0]["TodayCollection"].ToString();
            string strDepositAmount = dtCount.Rows[0]["DepositAmount"].ToString();

            //lblCount.Visible = true;
            //lblCount.Text = strVehicle;
            //lblCollection.Visible = true;

            if (dtCount.Rows[0]["DepositDate"].ToString() == "")
            {
                //lblLastDepositdate.Visible = true;
                //lblLastDepositdate.Text = "Never Deposit";
            }
            else
            {
              
                //lblLastDepositdate.Visible = true;
                //lblLastDepositdate.Text = dtCount.Rows[0]["DepositDate"].ToString();
            }
            
            if (strAmount == "")
            {
                //lblCollection.Text = "0";
            }
            else
            {
                //lblCollection.Text = strAmount;
            }

            
            if (strDepositAmount == "")
            {
                //lblLastAmount.Visible = true;
                //lblLastAmount.Text = "0";

            }
            else
            {
                //lblLastAmount.Visible = true;
                //lblLastAmount.Text = strDepositAmount;
            }
            if (!IsPostBack)
            {
               
                fillAffixationCenter();
            }
            else
            {
                fillAffixationCenter();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string strAuthno = string.Empty;
            string StrRtoLocationCode = string.Empty;
            string StrRtoName = string.Empty;
            string StrAuthorizationNo = string.Empty;
            string StrOwnerName = string.Empty;
            string StrOwnerAddress = string.Empty;
            string StrVehicleType = string.Empty;
            string StrTransactonType = string.Empty;
            string StrAuthdate = string.Empty;
            string StrMobileNo = string.Empty;
            string StrVehicleClassType = string.Empty;
            string StrManufacturarName = string.Empty;
            string StrModelName = string.Empty;
            string StrRegistrationNo = string.Empty;
            string StrEngineNo = string.Empty;
            string StrChasisNo = string.Empty;
            string strAuthNo = txtAuthNo.Text.Trim();
            string stremail = string.Empty;
            string SQLString = string.Empty;
            string RTOlocationName=string.Empty;
            string SQlQuery = string.Empty;
            string  SqlRto=string.Empty;

            //SQLString = "select Rtolocationname from rtolocation where rtolocationid='" + RTOLocationID + "'";
            RTOlocationName = Session["RTOLocationName"].ToString();// Utils.getScalarValue(SQLString, ConnectionString);

            try
            {
                if (string.IsNullOrEmpty(strAuthNo))
                {
                    string closescript1 = "<script>alert('Please Provide Vehicle Authorization No.')</script>";
                    Page.RegisterStartupScript("abc", closescript1);
                    return;
                    bln3rdSticker.Checked = false;
                }

              

                //HSRP.APWebrefrence.HSRPAuthorizationService objAPService = new HSRP.APWebrefrence.HSRPAuthorizationService();
                //string AuthData = objAPService.GetHSRPAuthorizationno(strAuthNo);

                SQlQuery = "select hsrprecord_authorizationno,convert(varchar(15),HSRPRecord_AuthorizationDate,103) as AuthorizationDate,VehicleRegNo ,ownername,address1,Emailid,vehicleclass,VehicleType,ManufacturerName,MobileNo,ChassisNo,EngineNo,OrderType,RoundOff_NetAmount As Amont from HSRPRecords where hsrp_stateid='" + HSRPStateID + "' and orderstatus='New Order' and hsrprecord_authorizationno = '" + strAuthNo + "'";
                DataTable dt = Utils.GetDataTable(SQlQuery, ConnectionString);
                if (dt.Rows.Count > 0)
                {

                    strAuthno = dt.Rows[0]["hsrprecord_authorizationno"].ToString();
                    stremail = dt.Rows[0]["Emailid"].ToString();
                    StrMobileNo = dt.Rows[0]["MobileNo"].ToString();
                    StrOwnerName = dt.Rows[0]["ownername"].ToString();
                    StrRegistrationNo = dt.Rows[0]["VehicleRegNo"].ToString();
                    StrOwnerAddress = dt.Rows[0]["address1"].ToString();
                    StrVehicleType = dt.Rows[0]["VehicleType"].ToString();
                    StrVehicleClassType = dt.Rows[0]["vehicleclass"].ToString();
                    StrAuthdate = dt.Rows[0]["AuthorizationDate"].ToString();
                    StrChasisNo = dt.Rows[0]["ChassisNo"].ToString();
                    StrEngineNo = dt.Rows[0]["EngineNo"].ToString();
                    StrManufacturarName = dt.Rows[0]["ManufacturerName"].ToString();
                    StrTransactonType=dt.Rows[0]["OrderType"].ToString();
                    //lblAmount.Text = dt.Rows[0]["Amont"].ToString();

                }
                else
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Authorization Number not found";
                    refresh();
                    return;
                }
       
                    lblAuthNo.Text = strAuthno;
                    lblRTOLocationCode.Text = StrRtoLocationCode;
                    lblRTOName.Text = RTOlocationName;
                    lblEmail.Text = stremail;
                    lblOwnerName.Text = StrOwnerName;
                    lblAddress.Text = StrOwnerAddress;
                    lblVehicleType.Text = StrVehicleType;
                    lblTransactionType.Text = StrTransactonType;
                    lblAuthDate.Text = StrAuthdate;
                    if (StrMobileNo.Trim() == "" || StrMobileNo.Trim() == null || StrMobileNo.Trim() == "NOMOBILENUMBER")
                    {
                        lblMobileNo.Text = "0";
                    }
                    else
                    {
                        lblMobileNo.Text = StrMobileNo;
                    }
                    lblVehicleClassType.Text = StrVehicleClassType;
                    lblMfgName.Text = StrManufacturarName;
                    lblModelName.Text = StrModelName;
                    if (StrRegistrationNo.ToString() == "NOREGNNUMBER")
                    {
                        StrRegistrationNo = "";
                    }

                    lblRegNo.Text = StrRegistrationNo;

                    lblEngineNo.Text = StrEngineNo;
                    lblChasisNo.Text = StrChasisNo;

                   
                

        
                string SQLString2 = string.Empty;
                SQLString2 = "select dbo.hsrpplateamt ('" + HSRPStateID + "','" + StrVehicleType + "','" + StrVehicleClassType + "','" + StrTransactonType + "') as Amount";
                DataTable dt1 = Utils.GetDataTable(SQLString2, ConnectionString);
                lblAmount.Text = dt1.Rows[0]["Amount"].ToString();

                if ((StrVehicleType == "MCV/HCV/TRAILERS") || (StrVehicleType == "LMV(CLASS)") || (StrVehicleType == "LMV") || (StrVehicleType == "THREE WHEELER"))
                {
                    bln3rdSticker.Checked = true;
                }
                else
                {
                    bln3rdSticker.Checked = false;
                }

                if (bln3rdSticker.Checked == true)
                {
                    Sticker = "Y";
                }
                else
                {
                    Sticker = "N";
                }

                if (blnVIP.Checked == true)
                {
                    VIP = "Y";
                }
                else
                {
                    VIP = "N";
                }
               
            }
            catch (Exception ex)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Server is not responding... Pls contact to System Administrator :" + ex.ToString();
            }
        }



        public void refresh()
        {
            lblAuthNo.Text = "";
            lblRTOLocationCode.Text = "";
            lblRTOName.Text = "";
            lblEmail.Text = "";
            lblOwnerName.Text = "";
            lblAddress.Text = "";
            lblVehicleType.Text = "";
            lblTransactionType.Text = "";
            lblAuthDate.Text = "";
            lblMobileNo.Text = "";
            lblVehicleClassType.Text = "";
            lblMfgName.Text = "";
            lblModelName.Text = "";
            lblRegNo.Text = "";
            lblEngineNo.Text = "";
            lblChasisNo.Text = "";
            lblAmount.Text = "";
        }

        
        
        string Query = string.Empty;
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ToDay = System.DateTime.Today.DayOfWeek.ToString();
                if (ToDay == "Sunday")
                {
                    lblErrMess.Visible =true;
                    lblErrMess.Text = "No Collection Allowed On Sunday";
                    
                    return;
                }
                if (ddlaffixation.Text == "--Select Affixation Center--")
                {
                    lblErrMess.Text = "Please select Affixation Center";
                    ddlaffixation.Focus();
                    return;
                }
             

                Query = "select count(*) from hsrprecords where HSRP_StateID =9 and vehicleregno='" + lblRegNo.Text + "'";
                int co = Utils.getScalarCount(Query, ConnectionString);
                
                if (co > 0)
                {
                    lblSucMess.Visible = false;
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Registration No. Already Exist Cannot Update Record.";
                }
                else
                {
                    string hsrprecordid = string.Empty;
                    string strQueryinsert = "insert into APReAssignedAuthNo(HSRPRecordID,OldAuthorizationNo,NewAuthorizationNo,Remarks,UserID,Recordgenrateddatetime) " +
                         "values('" + hsrprecordid + "','" + txtAuthNo + "','" + txtnewAuthNo + "','"+ remarks +"','"+ USERID +"','"+ System.DateTime.Now +"')";


                    int i = Utils.ExecNonQuery(strQueryinsert, ConnectionString);


                    if (i > 0)
                    {

                        lblSucMess.Visible = true;
                        lblErrMess.Visible = false;

                        lblSucMess.Text = "Record Insert Successfully";


                        // refresh();

                    }

                    string SqlRecordid = "select hsrprecordid from hsrprecord where hsrp_stateid=9 and HSRPRecord_AuthorizationNo='" + lblAuthNo + "' ";
                     hsrprecordid = Utils.getScalarValue(SqlRecordid, ConnectionString);
                   
                   
                   // Expected Affixation Date has been added on 8th Jan 2014
                    string strQuery = "update hsrprecords set HSRPRecord_AuthorizationNo='" + txtnewAuthNo + "' where hsrp_stateid=9 and hsrprecordid='" + hsrprecordid + "'"; 

                    int j = Utils.ExecNonQuery(strQuery, ConnectionString);
                
                    
                    //if (j > 0)
                    //{
                        
                    //    lblSucMess.Visible = true;
                    //    lblErrMess.Visible = false;
                      
                    //    lblSucMess.Text = "Record Update Successfully";
                     

                    //   // refresh();
                       
                    //}
                }
            }
            catch (Exception ex)
            {
                lblSucMess.Text = "Message : " + ex;
                return;
            }
        }

       
        public void fillAffixationCenter()
        {

            string sqlquery = "select Rto_Id ,AffixCenterDesc from AffixationCenters where State_Id='" + Session["UserHSRPStateID"].ToString() + "'";
            Utils.PopulateDropDownList(ddlaffixation, sqlquery, ConnectionString, "--Select Affixation Center--");
        }

        protected void btngo2_Click(object sender, EventArgs e)
        {
            string strAuthno = string.Empty;
            string StrRtoLocationCode = string.Empty;
            string StrRtoName = string.Empty;
            string StrAuthorizationNo = string.Empty;
            string StrOwnerName = string.Empty;
            string StrOwnerAddress = string.Empty;
            string StrVehicleType = string.Empty;
            string StrTransactonType = string.Empty;
            string StrAuthdate = string.Empty;
            string StrMobileNo = string.Empty;
            string StrVehicleClassType = string.Empty;
            string StrManufacturarName = string.Empty;
            string StrModelName = string.Empty;
            string StrRegistrationNo = string.Empty;
            string StrEngineNo = string.Empty;
            string StrChasisNo = string.Empty;
            string strAuthNo = txtAuthNo.Text.Trim();
            string stremail = string.Empty;
            string SQLString = string.Empty;
            string RTOlocationName = string.Empty;

            //SQLString = "select Rtolocationname from rtolocation where rtolocationid='" + RTOLocationID + "'";
            RTOlocationName = Session["RTOLocationName"].ToString();// Utils.getScalarValue(SQLString, ConnectionString);

            try
            {
                if (string.IsNullOrEmpty(strAuthNo))
                {
                    string closescript1 = "<script>alert('Please Provide Vehicle Authorization No.')</script>";
                    Page.RegisterStartupScript("abc", closescript1);
                    return;
                }

                bln3rdSticker.Checked = false;

                HSRP.APWebrefrence.HSRPAuthorizationService objAPService = new HSRP.APWebrefrence.HSRPAuthorizationService();
                string AuthData = objAPService.GetHSRPAuthorizationno(strAuthNo);
                //string TBH_ID = "";
                if (AuthData == "1")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Cash Not Collected";
                    refresh();

                    return;
                }
                else if (AuthData.Length > 1)
                {
                    using (StringReader stringReader = new StringReader(AuthData))
                    using (XmlTextReader reader = new XmlTextReader(stringReader))
                    {
                        while (reader.Read())
                        {

                            if (reader.Name.ToString() == "Rto_Code")
                            {
                                reader.Read();
                                StrRtoLocationCode = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Rto_Name")
                            {
                                reader.Read();
                                StrRtoName = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Authorization_Ref_no")
                            {
                                reader.Read();
                                strAuthno = reader.Value.ToString();
                                //StrAuthorizationNo = AuthArray[2].ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Owner_Name")
                            {
                                reader.Read();
                                StrOwnerName = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Owner_Address")
                            {
                                reader.Read();
                                StrOwnerAddress = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Vehicle_Type")
                            {
                                reader.Read();
                                string strquery = "select upper(ourValue) as ourValue from  [dbo].[Mapping_Vahan_HSRP_ap] where rtovalue ='" + reader.Value.ToString() + "'";
                                // Utils.getScalarValue( 
                                StrVehicleType = Utils.getDataSingleValue(strquery, ConnectionString, "ourValue");

                                // StrVehicleType = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Trans_Type")
                            {
                                reader.Read();
                                StrTransactonType = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Authorization_Date")
                            {
                                reader.Read();
                                StrAuthdate = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Mobile_No")
                            {
                                reader.Read();
                                StrMobileNo = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Owner_Email_Id")
                            {
                                reader.Read();
                                stremail = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Veh_Class_Type")
                            {
                                reader.Read();
                                if (reader.Value.ToString() == "N")
                                {
                                    StrVehicleClassType = "Non-Transport";
                                }
                                else if (reader.Value.ToString() == "T")
                                {
                                    StrVehicleClassType = "Transport";
                                }
                                reader.Read();

                            }
                            if (reader.Name.ToString() == "MFRS_Name")
                            {
                                reader.Read();
                                StrManufacturarName = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Model_Name")
                            {
                                reader.Read();
                                StrModelName = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Reg_no")
                            {
                                reader.Read();
                                lblErrMess.Text = "";
                                StrRegistrationNo = reader.Value.ToString().Trim();


                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Engine_No")
                            {
                                reader.Read();
                                StrEngineNo = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Chassis_No")
                            {
                                reader.Read();
                                StrChasisNo = reader.Value.ToString();
                                reader.Read();
                            }

                        }
                    }
                    lblAuthNo.Text = strAuthno;
                    lblRTOLocationCode.Text = StrRtoLocationCode;
                    lblRTOName.Text = StrRtoName;
                    lblEmail.Text = stremail;
                    lblOwnerName.Text = StrOwnerName;
                    lblAddress.Text = StrOwnerAddress;
                    lblVehicleType.Text = StrVehicleType;
                    lblTransactionType.Text = StrTransactonType;
                    lblAuthDate.Text = StrAuthdate;
                    if (StrMobileNo.Trim() == "" || StrMobileNo.Trim() == null || StrMobileNo.Trim() == "NOMOBILENUMBER")
                    {
                        lblMobileNo.Text = "0";
                    }
                    else
                    {
                        lblMobileNo.Text = StrMobileNo;
                    }
                    lblVehicleClassType.Text = StrVehicleClassType;
                    lblMfgName.Text = StrManufacturarName;
                    lblModelName.Text = StrModelName;
                    if (StrRegistrationNo.ToString() == "NOREGNNUMBER")
                    {
                        StrRegistrationNo = "";
                    }

                    lblRegNo.Text = StrRegistrationNo;

                    lblEngineNo.Text = StrEngineNo;
                    lblChasisNo.Text = StrChasisNo;


                }
                string SQLString2 = string.Empty;
                SQLString2 = "select dbo.hsrpplateamt ('" + HSRPStateID + "','" + StrVehicleType + "','" + StrVehicleClassType + "','" + StrTransactonType + "') as Amount";
                DataTable dt1 = Utils.GetDataTable(SQLString2, ConnectionString);
                lblAmount.Text = dt1.Rows[0]["Amount"].ToString();

                if ((StrVehicleType == "MCV/HCV/TRAILERS") || (StrVehicleType == "LMV(CLASS)") || (StrVehicleType == "LMV") || (StrVehicleType == "THREE WHEELER"))
                {
                    bln3rdSticker.Checked = true;
                }
                else
                {
                    bln3rdSticker.Checked = false;
                }

                if (bln3rdSticker.Checked == true)
                {
                    Sticker = "Y";
                }
                else
                {
                    Sticker = "N";
                }

                if (blnVIP.Checked == true)
                {
                    VIP = "Y";
                }
                else
                {
                    VIP = "N";
                }

            }
            catch (Exception ex)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "RTA Server is not responding... Pls contact to System Administrator :" + ex.ToString();
            }
        }

        

      
    }
}