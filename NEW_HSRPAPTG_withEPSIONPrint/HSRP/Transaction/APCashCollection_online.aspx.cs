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
    public partial class APCashCollection_online : System.Web.UI.Page
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
        DataTable dataSetFillHSRPDeliveryChallan = new DataTable();
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

            lblCount.Visible = true;
            lblCount.Text = strVehicle;
            lblCollection.Visible = true;

            if (dtCount.Rows[0]["DepositDate"].ToString() == "")
            {
                lblLastDepositdate.Visible = true;
                lblLastDepositdate.Text = "Never Deposit";
            }
            else
            {

                lblLastDepositdate.Visible = true;
                lblLastDepositdate.Text = dtCount.Rows[0]["DepositDate"].ToString();
            }

            if (strAmount == "")
            {
                lblCollection.Text = "0";
            }
            else
            {
                lblCollection.Text = strAmount;
            }


            if (strDepositAmount == "")
            {
                lblLastAmount.Visible = true;
                lblLastAmount.Text = "0";

            }
            else
            {
                lblLastAmount.Visible = true;
                lblLastAmount.Text = strDepositAmount;
            }

            //string receipetno = dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString();

            if (strDepositAmount == "")
            {
                lblLastAmount.Visible = true;
                lblLastAmount.Text = "0";

            }
            else
            {
                lblLastAmount.Visible = true;
                lblLastAmount.Text = strDepositAmount;
            }
            if (!IsPostBack)
            {
                
                fillAffixationCenter();
            }
            else
            {

            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            Button3.Visible = false;
            btnDownload.Visible = false;
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

                string strDate = System.DateTime.Now.ToString("hh:mm tt");
                string strnine = "09:00 AM";
                string strsix = "06:00 PM";
                if (DateTime.Parse(strDate) < DateTime.Parse(strnine) || DateTime.Parse(strDate) > DateTime.Parse(strsix))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Cash Collection Timing is  Between 9 AM TO 6 PM ";
                    return;
                }   
                bln3rdSticker.Checked = false;

                HSRP.APWebrefrence.HSRPAuthorizationService objAPService = new HSRP.APWebrefrence.HSRPAuthorizationService();
                string AuthData = objAPService.GetHSRPAuthorizationno(strAuthNo);
                //string TBH_ID = "";
                if (AuthData == "1")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Data not recieved from Govt. or Authorization Number is wrong. Please Check.";
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
                            if (reader.Name.ToString() == "OldNew_Flag")
                            {
                                reader.Read();
                                hdnoldnewValue.Value= reader.Value.ToString();
                                reader.Read();
                            }

                        }
                    }
                    lblAuthNo.Text = strAuthno;
                    lblRTOLocationCode.Text = StrRtoLocationCode;
                    lblRTOName.Text = StrRtoName;
                    lblEmail.Text = stremail;                   
                    lblOwnerName.Text = StrOwnerName.Replace(@"'", string.Empty);
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


        protected void btnDownloadReceipt_Click(object sender, EventArgs e)
        {

            DataTable GetAddress;
            string Address;
            decimal sgstno;
            decimal cgstno;

            DataTable rtoaddr = new DataTable();
            rtoaddr = Utils.GetDataTable("select r.RTOLocationAddress from users as u inner join rtolocation as r on u.RTOLocationID=r.RTOLocationID where userid='" + Session["UID"].ToString() + "'", ConnectionString);

            string sqlquery = "select Address1 from AffixationCenters where Rto_Id='" + RTOLocationID + "'";
            string AffAddress = Utils.getDataSingleValue(sqlquery, ConnectionString, "Address1");


            GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRPStateID + "'", ConnectionString);

            if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
            {
                Address = " - " + GetAddress.Rows[0]["pincode"];

            }
            else
            {
                Address = "";
            }
            string getTinNo = GetAddress.Rows[0]["GSTIN"].ToString();

            sgstno = Convert.ToDecimal(GetAddress.Rows[0]["SGSTPer"].ToString());
            cgstno = Convert.ToDecimal(GetAddress.Rows[0]["CGSTPer"].ToString());

            string SQLString = "select hsrprecordID, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,CashReceiptNo,SGSTPer,SGSTAmount,CGSTPer,CGSTAmount,GSTBasicAmount from hsrprecords where HSRP_StateID =9 and  HSRPRecord_AuthorizationNo ='" + lblAuthNo.Text + "'";
            DataTable dataSetFillHSRPDeliveryChallan = Utils.GetDataTable(SQLString, ConnectionString);


            if (dataSetFillHSRPDeliveryChallan.Rows.Count > 0)
            {


                string filename = "CASHRECEIPT_No-" + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString().Replace("/", "-") + ".pdf";


                String StringField = String.Empty;
                String StringAlert = String.Empty;


                //Creates an instance of the iTextSharp.text.Document-object:
                Document document = new Document();

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:csh
                document.Open();

                //Adds content to the document:
                // document.Add(new Paragraph("Ignition Log Report"));
                PdfPTable table = new PdfPTable(4);
                //actual width of table in points
                table.TotalWidth = 585f;

                PdfPTable table1 = new PdfPTable(4);
                //actual width of table in points
                table1.TotalWidth = 585f;

                PdfPTable table2 = new PdfPTable(4);
                //actual width of table in points
                table2.TotalWidth = 585f;

                PdfPTable table3 = new PdfPTable(4);
                //actual width of table in points
                table3.TotalWidth = 585f;

                PdfPTable table4 = new PdfPTable(4);
                //actual width of table in points
                table4.TotalWidth = 585f;

                PdfPTable table5 = new PdfPTable(4);
                //actual width of table in points
                table5.TotalWidth = 585f;

                //fix the absolute width of the table


                PdfPCell cell312 = new PdfPCell(new Phrase("Original", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell312.Colspan = 4;
                cell312.BorderColor = BaseColor.WHITE;
                cell312.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell312);

                PdfPCell cell312a = new PdfPCell(new Phrase("Duplicate", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell312a.Colspan = 4;
                cell312a.BorderColor = BaseColor.WHITE;
                cell312a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cell312a);

                PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 4;
                cell12.BorderColor = BaseColor.WHITE;
                cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);

                PdfPCell cell1203 = new PdfPCell(new Phrase("" + AffAddress, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1203.Colspan = 4;
                cell1203.BorderWidthLeft = 0f;
                cell1203.BorderWidthRight = 0f;
                cell1203.BorderWidthTop = 0f;
                cell1203.BorderWidthBottom = 0f;
                cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1203);

                PdfPCell cell0 = new PdfPCell(new Phrase("HSRP CASH RECEIPT", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK)));
                cell0.Colspan = 4;
                cell0.BorderWidthLeft = 0f;
                cell0.BorderWidthRight = 0f;
                cell0.BorderWidthTop = 0f;
                cell0.BorderWidthBottom = 0f;

                cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell0);


                PdfPCell cell1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1.Colspan = 4;
                cell1.BorderWidthLeft = 0f;
                cell1.BorderWidthRight = 0f;
                cell1.BorderWidthTop = 0f;
                cell1.BorderWidthBottom = 0f;

                cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1);

                PdfPCell cellInv2 = new PdfPCell(new Phrase("RECEIPT No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellInv2.Colspan = 1;

                cellInv2.BorderWidthLeft = 0f;
                cellInv2.BorderWidthRight = 0f;
                cellInv2.BorderWidthTop = 0f;
                cellInv2.BorderWidthBottom = 0f;
                cellInv2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellInv2);

                PdfPCell cellInv22111 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellInv22111.Colspan = 1;
                cellInv22111.BorderWidthLeft = 0f;
                cellInv22111.BorderWidthRight = 0f;
                cellInv22111.BorderWidthTop = 0f;
                cellInv22111.BorderWidthBottom = 0f;
                cellInv22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellInv22111);

                PdfPCell cell21 = new PdfPCell(new Phrase("DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell21.Colspan = 1;

                cell21.BorderWidthLeft = 0f;
                cell21.BorderWidthRight = 0f;
                cell21.BorderWidthTop = 0f;
                cell21.BorderWidthBottom = 0f;
                cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell21);
                string CashReceiptDateTime = string.Empty;

                if (dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"].ToString() == "")
                {
                    CashReceiptDateTime = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    CashReceiptDateTime = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("dd/MM/yyyy");
                }
                PdfPCell cell212 = new PdfPCell(new Phrase(": " + CashReceiptDateTime, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell212.Colspan = 1;

                cell212.BorderWidthLeft = 0f;
                cell212.BorderWidthRight = 0f;
                cell212.BorderWidthTop = 0f;
                cell212.BorderWidthBottom = 0f;
                cell212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell212);



                PdfPCell cell2 = new PdfPCell(new Phrase("GSTIN", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2.Colspan = 1;

                cell2.BorderWidthLeft = 0f;
                cell2.BorderWidthRight = 0f;
                cell2.BorderWidthTop = 0f;
                cell2.BorderWidthBottom = 0f;
                cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2);

              

                PdfPCell cell22111 = new PdfPCell(new Phrase(": " + getTinNo.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //PdfPCell cell22111 = new PdfPCell(new Phrase(": ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell22111.Colspan = 1;
                cell22111.BorderWidthLeft = 0f;
                cell22111.BorderWidthRight = 0f;
                cell22111.BorderWidthTop = 0f;
                cell22111.BorderWidthBottom = 0f;
                cell22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell22111);




                PdfPCell cell22 = new PdfPCell(new Phrase("TIME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell22.Colspan = 1;

                cell22.BorderWidthLeft = 0f;
                cell22.BorderWidthRight = 0f;
                cell22.BorderWidthTop = 0f;
                cell22.BorderWidthBottom = 0f;
                cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell22);

                PdfPCell cell222 = new PdfPCell(new Phrase(": " + Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("hh:mm:ss tt"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell222.Colspan = 1;

                cell222.BorderWidthLeft = 0f;
                cell222.BorderWidthRight = 0f;
                cell222.BorderWidthTop = 0f;
                cell222.BorderWidthBottom = 0f;
                cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell222);

                PdfPCell cell15 = new PdfPCell(new Phrase("________________________________________________________________________", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell15.Colspan = 4;
                cell15.BorderWidthLeft = 0f;
                cell15.BorderWidthRight = 0f;
                cell15.BorderWidthTop = 0f;
                cell15.BorderWidthBottom = 0f;
                table.AddCell(cell15);


                PdfPCell cell5 = new PdfPCell(new Phrase("AUTH. NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell5.Colspan = 1;

                cell5.BorderWidthLeft = 0f;
                cell5.BorderWidthRight = 0f;
                cell5.BorderWidthTop = 0f;
                cell5.BorderWidthBottom = 0f;
                cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell5);

                PdfPCell cell55 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell55.Colspan = 1;

                cell55.BorderWidthLeft = 0f;
                cell55.BorderWidthRight = 0f;
                cell55.BorderWidthTop = 0f;
                cell55.BorderWidthBottom = 0f;
                cell55.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell55);

                PdfPCell cell25 = new PdfPCell(new Phrase("AUTH. DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell25.Colspan = 1;

                cell25.BorderWidthLeft = 0f;
                cell25.BorderWidthRight = 0f;
                cell25.BorderWidthTop = 0f;
                cell25.BorderWidthBottom = 0f;
                cell25.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell25);

                DateTime AuthDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
                PdfPCell cell255 = new PdfPCell(new Phrase(": " + AuthDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell255.Colspan = 1;

                cell255.BorderWidthLeft = 0f;
                cell255.BorderWidthRight = 0f;
                cell255.BorderWidthTop = 0f;
                cell255.BorderWidthBottom = 0f;
                cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell255);


                DateTime Orddate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"].ToString());

                PdfPCell cell7 = new PdfPCell(new Phrase("OWNER NAME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell7.Colspan = 1;
                cell7.BorderWidthLeft = 0f;
                cell7.BorderWidthRight = 0f;
                cell7.BorderWidthTop = 0f;
                cell7.BorderWidthBottom = 0f;
                cell7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell7);

                PdfPCell cell75 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell75.Colspan = 1;
                cell75.BorderWidthLeft = 0f;
                cell75.BorderWidthRight = 0f;
                cell75.BorderWidthTop = 0f;
                cell75.BorderWidthBottom = 0f;
                cell75.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell75);

                PdfPCell cell29 = new PdfPCell(new Phrase("OWNER CONTACT NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell29.Colspan = 1;
                cell29.BorderWidthLeft = 0f;
                cell29.BorderWidthRight = 0f;
                cell29.BorderWidthTop = 0f;
                cell29.BorderWidthBottom = 0f;
                cell29.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell29);

                PdfPCell cell295 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell295.Colspan = 1;
                cell295.BorderWidthLeft = 0f;
                cell295.BorderWidthRight = 0f;
                cell295.BorderWidthTop = 0f;
                cell295.BorderWidthBottom = 0f;
                cell295.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell295);



                PdfPCell cell8 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell8.Colspan = 1;
                cell8.BorderWidthLeft = 0f;
                cell8.BorderWidthRight = 0f;
                cell8.BorderWidthTop = 0f;
                cell8.BorderWidthBottom = 0f;
                cell8.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell8);

                PdfPCell cell85 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell85.Colspan = 3;
                cell85.BorderWidthLeft = 0f;
                cell85.BorderWidthRight = 0f;
                cell85.BorderWidthTop = 0f;
                cell85.BorderWidthBottom = 0f;
                cell85.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell85);

                PdfPCell cell9 = new PdfPCell(new Phrase("VEHICLE REG. NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell9.Colspan = 1;
                cell9.BorderWidthLeft = 0f;
                cell9.BorderWidthRight = 0f;
                cell9.BorderWidthTop = 0f;
                cell9.BorderWidthBottom = 0f;
                cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell9);

                PdfPCell cell95 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell95.Colspan = 1;
                cell95.BorderWidthLeft = 0f;
                cell95.BorderWidthRight = 0f;
                cell95.BorderWidthTop = 0f;
                cell95.BorderWidthBottom = 0f;
                cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell95);

                PdfPCell cell10 = new PdfPCell(new Phrase("VEHICLE MODEL", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell10.Colspan = 1;
                cell10.BorderWidthLeft = 0f;
                cell10.BorderWidthRight = 0f;
                cell10.BorderWidthTop = 0f;
                cell10.BorderWidthBottom = 0f;
                cell10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell10);

                PdfPCell cell105 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell105.Colspan = 1;
                cell105.BorderWidthLeft = 0f;
                cell105.BorderWidthRight = 0f;
                cell105.BorderWidthTop = 0f;
                cell105.BorderWidthBottom = 0f;
                cell105.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell105);

                PdfPCell cell11 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell11.Colspan = 1;
                cell11.BorderWidthLeft = 0f;
                cell11.BorderWidthRight = 0f;
                cell11.BorderWidthTop = 0f;
                cell11.BorderWidthBottom = 0f;
                cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell11);

                PdfPCell cell115 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell115.Colspan = 1;
                cell115.BorderWidthLeft = 0f;
                cell115.BorderWidthRight = 0f;
                cell115.BorderWidthTop = 0f;
                cell115.BorderWidthBottom = 0f;
                cell115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell115);

                PdfPCell cell1113 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1113.Colspan = 1;
                cell1113.BorderWidthLeft = 0f;
                cell1113.BorderWidthRight = 0f;
                cell1113.BorderWidthTop = 0f;
                cell1113.BorderWidthBottom = 0f;
                cell1113.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1113);

                PdfPCell cell11135 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell11135.Colspan = 1;
                cell11135.BorderWidthLeft = 0f;
                cell11135.BorderWidthRight = 0f;
                cell11135.BorderWidthTop = 0f;
                cell11135.BorderWidthBottom = 0f;
                cell11135.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell11135);

                PdfPCell cell1112 = new PdfPCell(new Phrase("________________________________________________________________________", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1112.Colspan = 4;
                cell1112.BorderWidthLeft = 0f;
                cell1112.BorderWidthRight = 0f;
                cell1112.BorderWidthTop = 0f;
                cell1112.BorderWidthBottom = 0f;
                cell1112.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1112);


                PdfPCell cellspa12 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellspa12.Colspan = 1;
                cellspa12.BorderWidthLeft = 0f;
                cellspa12.BorderWidthRight = 0f;
                cellspa12.BorderWidthTop = 0f;
                cellspa12.BorderWidthBottom = 0f;
                cellspa12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellspa12);


                PdfPCell cell112 = new PdfPCell(new Phrase("DESCRIPTION", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell112.Colspan = 1;
                cell112.BorderWidthLeft = 0f;
                cell112.BorderWidthRight = 0f;
                cell112.BorderWidthTop = 0f;
                cell112.BorderWidthBottom = 0f;
                cell112.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell112);

                PdfPCell cellspa1s2 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellspa1s2.Colspan = 1;
                cellspa1s2.BorderWidthLeft = 0f;
                cellspa1s2.BorderWidthRight = 0f;
                cellspa1s2.BorderWidthTop = 0f;
                cellspa1s2.BorderWidthBottom = 0f;
                cellspa1s2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellspa1s2);

                PdfPCell cell119 = new PdfPCell(new Phrase("AMOUNT(RS)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell119.Colspan = 1;
                cell119.BorderWidthLeft = 0f;
                cell119.BorderWidthRight = 0f;
                cell119.BorderWidthTop = 0f;
                cell119.BorderWidthBottom = 0f;
                cell119.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell119);

                PdfPCell cellDesc = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellDesc.Colspan = 1;
                cellDesc.BorderWidthLeft = 0f;
                cellDesc.BorderWidthRight = 0f;
                cellDesc.BorderWidthTop = 0f;
                cellDesc.BorderWidthBottom = 0f;
                cellDesc.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellDesc);

                PdfPCell cellDescSet = new PdfPCell(new Phrase("SET OF HSRP PLATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellDescSet.Colspan = 1;
                cellDescSet.BorderWidthLeft = 0f;
                cellDescSet.BorderWidthRight = 0f;
                cellDescSet.BorderWidthTop = 0f;
                cellDescSet.BorderWidthBottom = 0f;
                cellDescSet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellDescSet);

                PdfPCell cellDesc1Sp = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellDesc1Sp.Colspan = 1;
                cellDesc1Sp.BorderWidthLeft = 0f;
                cellDesc1Sp.BorderWidthRight = 0f;
                cellDesc1Sp.BorderWidthTop = 0f;
                cellDesc1Sp.BorderWidthBottom = 0f;
                cellDesc1Sp.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellDesc1Sp);

                //decimal i = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString());
                //decimal one = i / 128;
                //decimal amt = one * 100;
                //decimal tax = amt / 100 * sgstno;
                //decimal tax1 = amt / 100 * cgstno;
                //amt = decimal.Round(amt, 2, MidpointRounding.AwayFromZero);  
                //tax = decimal.Round(tax, 2, MidpointRounding.AwayFromZero); 
                decimal amt = decimal.Round(Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["GSTBasicAmount"].ToString()), 2);
                decimal tax = decimal.Round(Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["SGSTAmount"].ToString()), 2);
                decimal tax1 = decimal.Round(Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["CGSTAmount"].ToString()), 2);

                //decimal roundoffvalue = decimal.Round(Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["Roundoff_Value"].ToString()), 2);

               // amt = amt + (-roundoffvalue);

                PdfPCell cellDescSp = new PdfPCell(new Phrase(" " + amt + "*", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellDescSp.Colspan = 1;
                cellDescSp.BorderWidthLeft = 0f;
                cellDescSp.BorderWidthRight = 0f;
                cellDescSp.BorderWidthTop = 0f;
                cellDescSp.BorderWidthBottom = 0f;
                cellDescSp.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellDescSp);


                PdfPCell cell1195 = new PdfPCell(new Phrase("________________________________________________________________________", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1195.Colspan = 4;
                cell1195.BorderWidthLeft = 0f;
                cell1195.BorderWidthRight = 0f;
                cell1195.BorderWidthTop = 0f;
                cell1195.BorderWidthBottom = 0f;
                cell1195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1195);
                //changes for gst
                PdfPCell cell1201 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1201.Colspan = 4;
                cell1201.BorderWidthLeft = 0f;
                cell1201.BorderWidthRight = 0f;
                cell1201.BorderWidthTop = 0f;
                cell1201.BorderWidthBottom = 0f;
                cell1201.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1201);

                PdfPCell cell1202 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1202.Colspan = 4;
                cell1202.BorderWidthLeft = 0f;
                cell1202.BorderWidthRight = 0f;
                cell1202.BorderWidthTop = 0f;
                cell1202.BorderWidthBottom = 0f;
                cell1202.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1202);


                PdfPCell celldupRouCash401 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash401.Colspan = 1;
                celldupRouCash401.BorderWidthLeft = 0f;
                celldupRouCash401.BorderWidthRight = 0f;
                celldupRouCash401.BorderWidthTop = 0f;
                celldupRouCash401.BorderWidthBottom = 0f;
                celldupRouCash401.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash401);

                PdfPCell celldupCash401 = new PdfPCell(new Phrase("SGST @14 % ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupCash401.Colspan = 1;
                celldupCash401.BorderWidthLeft = 0f;
                celldupCash401.BorderWidthRight = 0f;
                celldupCash401.BorderWidthTop = 0f;
                celldupCash401.BorderWidthBottom = 0f;
                celldupCash401.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash401);

                PdfPCell celldupRouCash402 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash402.Colspan = 1;
                celldupRouCash402.BorderWidthLeft = 0f;
                celldupRouCash402.BorderWidthRight = 0f;
                celldupRouCash402.BorderWidthTop = 0f;
                celldupRouCash402.BorderWidthBottom = 0f;
                celldupRouCash402.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash402);



                PdfPCell celldupCash402 = new PdfPCell(new Phrase(" " + tax, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupCash402.Colspan = 1;
                celldupCash402.BorderWidthLeft = 0f;
                celldupCash402.BorderWidthRight = 0f;
                celldupCash402.BorderWidthTop = 0f;
                celldupCash402.BorderWidthBottom = 0f;
                celldupCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash402);
                ///

                //cell1195.Colspan = 4;
                //cell1195.BorderWidthLeft = 0f;
                //cell1195.BorderWidthRight = 0f;
                //cell1195.BorderWidthTop = 0f;
                //cell1195.BorderWidthBottom = 0f;
                //cell1195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1195);

                //changes for gst
                PdfPCell cell1201SGST = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1201SGST.Colspan = 4;
                cell1201SGST.BorderWidthLeft = 0f;
                cell1201SGST.BorderWidthRight = 0f;
                cell1201SGST.BorderWidthTop = 0f;
                cell1201SGST.BorderWidthBottom = 0f;
                cell1201SGST.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1201SGST);

                PdfPCell cell1202SGST = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1202SGST.Colspan = 4;
                cell1202SGST.BorderWidthLeft = 0f;
                cell1202SGST.BorderWidthRight = 0f;
                cell1202SGST.BorderWidthTop = 0f;
                cell1202SGST.BorderWidthBottom = 0f;
                cell1202SGST.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1202SGST);


                PdfPCell celldupRouCash401sgst = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash401sgst.Colspan = 1;
                celldupRouCash401sgst.BorderWidthLeft = 0f;
                celldupRouCash401sgst.BorderWidthRight = 0f;
                celldupRouCash401sgst.BorderWidthTop = 0f;
                celldupRouCash401sgst.BorderWidthBottom = 0f;
                celldupRouCash401sgst.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash401sgst);

                PdfPCell celldupCash401sgst = new PdfPCell(new Phrase("CGST @14 % ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupCash401sgst.Colspan = 1;
                celldupCash401sgst.BorderWidthLeft = 0f;
                celldupCash401sgst.BorderWidthRight = 0f;
                celldupCash401sgst.BorderWidthTop = 0f;
                celldupCash401sgst.BorderWidthBottom = 0f;
                celldupCash401sgst.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash401sgst);

                PdfPCell celldupRouCash402sgst = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash402sgst.Colspan = 1;
                celldupRouCash402sgst.BorderWidthLeft = 0f;
                celldupRouCash402sgst.BorderWidthRight = 0f;
                celldupRouCash402sgst.BorderWidthTop = 0f;
                celldupRouCash402sgst.BorderWidthBottom = 0f;
                celldupRouCash402sgst.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash402sgst);



                PdfPCell celldupCash402sgst = new PdfPCell(new Phrase(" " + tax1, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupCash402sgst.Colspan = 1;
                celldupCash402sgst.BorderWidthLeft = 0f;
                celldupCash402sgst.BorderWidthRight = 0f;
                celldupCash402sgst.BorderWidthTop = 0f;
                celldupCash402sgst.BorderWidthBottom = 0f;
                celldupCash402sgst.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash402sgst);

                /////////////////////////////////////////////////////////

                PdfPCell cell12011 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12011.Colspan = 4;
                cell12011.BorderWidthLeft = 0f;
                cell12011.BorderWidthRight = 0f;
                cell12011.BorderWidthTop = 0f;
                cell12011.BorderWidthBottom = 0f;
                cell12011.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12011);

                PdfPCell cell12022 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12022.Colspan = 4;
                cell12022.BorderWidthLeft = 0f;
                cell12022.BorderWidthRight = 0f;
                cell12022.BorderWidthTop = 0f;
                cell12022.BorderWidthBottom = 0f;
                cell12022.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12022);


                PdfPCell celldupRouCash4011 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash4011.Colspan = 1;
                celldupRouCash4011.BorderWidthLeft = 0f;
                celldupRouCash4011.BorderWidthRight = 0f;
                celldupRouCash4011.BorderWidthTop = 0f;
                celldupRouCash4011.BorderWidthBottom = 0f;
                celldupRouCash4011.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash4011);

                PdfPCell celldupCash4001 = new PdfPCell(new Phrase("ROUND OF AMOUNT ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupCash4001.Colspan = 1;
                celldupCash4001.BorderWidthLeft = 0f;
                celldupCash4001.BorderWidthRight = 0f;
                celldupCash4001.BorderWidthTop = 0f;
                celldupCash4001.BorderWidthBottom = 0f;
                celldupCash4001.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash4001);

                PdfPCell celldupRouCash4402 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash4402.Colspan = 1;
                celldupRouCash4402.BorderWidthLeft = 0f;
                celldupRouCash4402.BorderWidthRight = 0f;
                celldupRouCash4402.BorderWidthTop = 0f;
                celldupRouCash4402.BorderWidthBottom = 0f;
                celldupRouCash4402.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash4402);

                decimal roundAmt = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString());
                roundAmt = Math.Round(roundAmt, 0);

                PdfPCell celldupCash4002 = new PdfPCell(new Phrase(" " + roundAmt, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupCash4002.Colspan = 1;
                celldupCash4002.BorderWidthLeft = 0f;
                celldupCash4002.BorderWidthRight = 0f;
                celldupCash4002.BorderWidthTop = 0f;
                celldupCash4002.BorderWidthBottom = 0f;
                celldupCash4002.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash4002);

                //-----------------------------


                //-----------------------------

                if (lblRegNo.Text.Trim() == "")
                {

                    string Messagenew = "\u2022" + " Payment against your authorization has been recieved. HSRP will be processed once the vehicle registration no.  will be provided by the department.";

                    PdfPCell cellnew = new PdfPCell(new Phrase(Messagenew, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellnew.Colspan = 4;
                    cellnew.BorderWidthLeft = 0f;
                    cellnew.BorderWidthRight = 0f;
                    cellnew.BorderWidthTop = 0f;
                    cellnew.BorderWidthBottom = 0f;
                    cellnew.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellnew);
                }


                string Messageff = "\u2022" + " Fitment Charges Included.";

                PdfPCell cell64ff = new PdfPCell(new Phrase(Messageff, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell64ff.Colspan = 4;
                cell64ff.BorderWidthLeft = 0f;
                cell64ff.BorderWidthRight = 0f;
                cell64ff.BorderWidthTop = 0f;
                cell64ff.BorderWidthBottom = 0f;
                cell64ff.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell64ff);


                string Message = "\u2022" + " Please verify the details on Cash Receipt, before leaving the counter.";

                PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell64.Colspan = 4;
                cell64.BorderWidthLeft = 0f;
                cell64.BorderWidthRight = 0f;
                cell64.BorderWidthTop = 0f;
                cell64.BorderWidthBottom = 0f;
                cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell64);

                //string MessageSec11 = "\u2022" + " The prices are inclusive of Taxes.";

                //PdfPCell cell631 = new PdfPCell(new Phrase(MessageSec11, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell631.Colspan = 4;
                //cell631.BorderWidthLeft = 0f;
                //cell631.BorderWidthRight = 0f;
                //cell631.BorderWidthTop = 0f;
                //cell631.BorderWidthBottom = 0f;
                //cell631.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell631);


                string MessageSec = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP, on the fourth working day, a confirmation SMS will be sent to the registered mobile number provided by the customer.";

                PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell65a.Colspan = 4;
                cell65a.BorderWidthLeft = 0f;
                cell65a.BorderWidthRight = 0f;
                cell65a.BorderWidthTop = 0f;
                cell65a.BorderWidthBottom = 0f;
                cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell65a);

                PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell63.Colspan = 4;
                cell63.BorderWidthLeft = 0f;
                cell63.BorderWidthRight = 0f;
                cell63.BorderWidthTop = 0f;
                cell63.BorderWidthBottom = 0f;
                cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell63);

                //if (lblRegNo.Text.Trim() != "")
                //{

                //    PdfPCell cell163 = new PdfPCell(new Phrase("--Affixation--", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //    cell163.Colspan = 4;
                //    cell163.BorderWidthLeft = 0f;
                //    cell163.BorderWidthRight = 0f;
                //    cell163.BorderWidthTop = 0f;
                //    cell163.BorderWidthBottom = 0f;
                //    cell163.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //    table.AddCell(cell163);

                //    // String strquery1 = "select [dbo].GetAffxDate('" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "','" + dataSetFillHSRPDeliveryChallan.Rows[0]["hsrp_stateId"].ToString() + "') as Date";

                //    //Expected Date through Query
                //    string strquery1 = "select convert(varchar(15),PlateAffixationDate,103) as Date from hsrprecords where HSRPRecord_AuthorizationNo='" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "'";
                //    string date1 = Utils.getDataSingleValue(strquery1, ConnectionString, "Date");
                //    //DateTime date;

                //    //if (DateTime.TryParse(date1, out date))
                //    //{
                //    //}
                //    //else
                //    //{
                //    //    if (DateTime.Now.AddDays(2).DayOfWeek == DayOfWeek.Sunday)
                //    //    {
                //    //        date = DateTime.Now.AddDays(3);
                //    //    }
                //    //    else
                //    //    {
                //    //        date = System.DateTime.Now.AddDays(2);
                //    //    }
                //    //}

                //    PdfPCell cell13 = new PdfPCell(new Phrase("Date : " + date1 + " dd/mm/yyyy", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //    cell13.Colspan = 4;
                //    cell13.BorderWidthLeft = 0f;
                //    cell13.BorderWidthRight = 0f;
                //    cell13.BorderWidthTop = 0f;
                //    cell13.BorderWidthBottom = 0f;
                //    cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //    table.AddCell(cell13);
                //    PdfPCell cell1213 = new PdfPCell(new Phrase("Time : 2:00 PM - 6:00 PM", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //    cell1213.Colspan = 4;
                //    cell1213.BorderWidthLeft = 0f;
                //    cell1213.BorderWidthRight = 0f;
                //    cell1213.BorderWidthTop = 0f;
                //    cell1213.BorderWidthBottom = 0f;
                //    cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //    table.AddCell(cell1213);
                //    PdfPCell cell123 = new PdfPCell(new Phrase("Place : " + AffAddress, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //    cell123.Colspan = 4;
                //    cell123.BorderWidthLeft = 0f;
                //    cell123.BorderWidthRight = 0f;
                //    cell123.BorderWidthTop = 0f;
                //    cell123.BorderWidthBottom = 0f;
                //    cell123.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //    table.AddCell(cell123);
                //}


                PdfPCell cellsp5 = new PdfPCell(new Phrase("Customer Copy", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp5.Colspan = 4;
                cellsp5.BorderWidthLeft = 0f;
                cellsp5.BorderWidthRight = 0f;
                cellsp5.BorderWidthTop = 0f;
                cellsp5.BorderWidthBottom = 0f;
                cellsp5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table3.AddCell(cellsp5);

                PdfPCell cellsp5a = new PdfPCell(new Phrase("Office Copy", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp5a.Colspan = 4;
                cellsp5a.BorderWidthLeft = 0f;
                cellsp5a.BorderWidthRight = 0f;
                cellsp5a.BorderWidthTop = 0f;
                cellsp5a.BorderWidthBottom = 0f;
                cellsp5a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table4.AddCell(cellsp5a);

                PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell62.Colspan = 4;
                cell62.BorderWidthLeft = 0f;
                cell62.BorderWidthRight = 0f;
                cell62.BorderWidthTop = 0f;
                cell62.BorderWidthBottom = 0f;
                cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table5.AddCell(cell62);

                //////////////////////////////////////Duplicate CashRecipt/////////////////////////////////////////////////////////////////////////////////////////////


                PdfPCell cell2195 = new PdfPCell(new Phrase("----------------------------------------------------------------------------------------------------------------------------", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2195.Colspan = 4;
                cell2195.BorderWidthLeft = 0f;
                cell2195.BorderWidthRight = 0f;
                cell2195.BorderWidthTop = 0f;
                cell2195.BorderWidthBottom = 0f;
                cell2195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table5.AddCell(cell2195);

                document.Add(table1);
                document.Add(table);
                document.Add(table3);
                document.Add(table5);


                document.Add(table2);
                document.Add(table);
                document.Add(table4);
                document.Add(table5);



                document.Close();
                btnDownload.Visible = false;
                HttpContext context = HttpContext.Current;
                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }

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
                //if (ddlaffixation.Text == "--Select Affixation Center--")
                //{
                //    lblErrMess.Text = "Please select Affixation Center";
                //    ddlaffixation.Focus();
                //    return;
                //}
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

                string sticker1 = Sticker;

                string sql = "exec [getPlatesData] '" + HSRPStateID + "','" + lblVehicleType.Text + "','" + lblVehicleClassType.Text + "', '" + lblTransactionType.Text + "'";
                DataTable dt = Utils.GetDataTable(sql, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    lblSucMess.Visible = false;
                }

                Query = "select count(*) from hsrprecords where HSRP_StateID =9 and HSRPRecord_AuthorizationNo ='" + lblAuthNo.Text + "'";
                int co = Utils.getScalarCount(Query, ConnectionString);
                
                if (co > 0)//will change
                {
                    lblSucMess.Visible = false;
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Authorization No. Already Exist";
                }
                else
                {
                   
                    string cashrc = string.Empty;
                    string authdate = string.Empty;
                    
                    string[] arrauthdate = lblAuthDate.Text.Replace("T", " ").Split('.');
                    authdate = arrauthdate[0].ToString();
                    string[] arrauthdate1 = arrauthdate[0].ToString().Split('+');
                    authdate = arrauthdate1[0].ToString();

                    string Invoice = string.Empty;
                    string DC = string.Empty;
                    cashrc = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='"+ HSRPStateID +"' and rtolocationid ='"+ RTOLocationID +"' and prefixfor='Cash Receipt No'";
                    cashrc = Utils.getScalarValue(cashrc, ConnectionString);
                   
                   // Invoice = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Invoice No'";
                   // Invoice = Utils.getScalarValue(Invoice, ConnectionString);

                    DC = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Delivery Challan No'";
                    DC = Utils.getScalarValue(DC, ConnectionString);
                    lblAddress.Text =lblAddress.Text.Replace("'","");  
                    string AffixationDate = Utils.getDataSingleValue("select [dbo].GetAffxDate_insert_new('" + HSRPStateID + "') as Date", ConnectionString, "Date");
                   
                   // Expected Affixation Date has been added on 8th Jan 2014

                    //--------------Start GST Code--------------------------------
                    decimal SGSTPer;
                    decimal CGSTPer;
                    decimal CGSTPeramt;
                    decimal SGSTPeanmt;
                    decimal GSTBasicPeanmt;
                    decimal roundoffvalue;

                    //string strtax = " select  SGSTPer,SGSTAmount,CGSTPer,CGSTAmount,Roundoff_NetAmount,GSTBasicAmount from tax where OrderType = '" + lblTransactionType.Text.ToString() + "' and VehicleType ='" + lblVehicleType.Text.ToString() + "' and isnull(GSTBasicAmount,0)!=0  and   HSRP_StateID=9";
                    string strtax = " select  SGSTPer,SGSTAmount,CGSTPer,CGSTAmount,Roundoff_NetAmount,GSTBasicAmount,roundoff_value from tax where OrderType = '" + lblTransactionType.Text.ToString() + "' and VehicleType ='" + lblVehicleType.Text.ToString() + "' and isnull(GSTBasicAmount,0)!=0  and   HSRP_StateID=9";

                    DataTable dtgsttax = Utils.GetDataTable(strtax, ConnectionString);
                    
                    if(dtgsttax.Rows.Count>0)
                    {                        
                        SGSTPer = decimal.Round(Convert.ToDecimal(dtgsttax.Rows[0]["SGSTPer"].ToString()), 2);                        
                        SGSTPeanmt = decimal.Round(Convert.ToDecimal(dtgsttax.Rows[0]["SGSTAmount"].ToString()), 2);                       
                        CGSTPer = decimal.Round(Convert.ToDecimal(dtgsttax.Rows[0]["CGSTPer"].ToString()), 2);                        
                        CGSTPeramt = decimal.Round(Convert.ToDecimal(dtgsttax.Rows[0]["CGSTAmount"].ToString()), 2);
                        GSTBasicPeanmt = decimal.Round(Convert.ToDecimal(dtgsttax.Rows[0]["GSTBasicAmount"].ToString()), 2);                        
                        roundoffvalue = decimal.Round(Convert.ToDecimal(dtgsttax.Rows[0]["roundoff_value"].ToString()), 2);
                   
                 

                    //--------End GST Code


                    string strQuery = "insert into hsrprecords (SaveMacAddress,DeliveryChallanNo,ISFrontPlateSize,ISRearPlateSize, address1,manufacturername," +
                           " HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo," +
                           "VehicleClass,OrderType,StickerMandatory,isVIP,NetAmount,VehicleType,OrderStatus,CashReceiptNo,OwnerFatherName,EmailID,ChassisNo, EngineNo," +
                           "frontplatesize, rearplatesize,CreatedBy,Reference,ManufacturerModel,vehicleref,FrontplatePrize,RearPlatePrize,StickerPrize,ScrewPrize,TotalAmount," +
                           "VAT_Amount,RoundOff_NetAmount,VAT_Percentage,remarks,affix_id,PlateAffixationDate,OldNew_Flag,SGSTPer,SGSTAmount,CGSTPer,CGSTAmount,GSTBasicAmount,roundoff_value) " +
                           "values('" + macbase + "','" + DC + "', '" + dt.Rows[0]["frontplateflag"].ToString() + "','" + dt.Rows[0]["rearplateflag"].ToString() + "', '" +
                           lblAddress.Text + "','" + lblMfgName.Text + "', GetDate(),'" + HSRPStateID + "','" + RTOLocationID + "','" + lblAuthNo.Text + "','" +      
                           authdate + "','" + lblRegNo.Text.Trim().ToUpper() + "','" + lblOwnerName.Text + "','" + lblMobileNo.Text + "',  '" + lblVehicleClassType.Text    +
                           "','" + lblTransactionType.Text + "','" + sticker1 + "','" + VIP + "','" + lblAmount.Text + "','" + lblVehicleType.Text + "','New Order','" + cashrc +
                           "',    '','" + lblEmail.Text.Trim() + "', '" + lblChasisNo.Text.Trim() + "', '" + lblEngineNo.Text.Trim() + "', '" +
                           dt.Rows[0]["frontplateID"].ToString() + "', '" + dt.Rows[0]["RearPlateID"].ToString() + "','" + USERID + "','" + remarks.Text + "','" + lblModelName.Text.Replace("'"," ").ToString() +
                           "','New','" + dt.Rows[0]["FrontPlateCost"].ToString() + "','" + dt.Rows[0]["rearplatecost"].ToString() + "','" + dt.Rows[0]["stickercost"].ToString() +
                           "','" + dt.Rows[0]["snaplockcost"].ToString() + "','" + dt.Rows[0]["totalamount"].ToString() + "','" + dt.Rows[0]["vatamount"].ToString() + "','" +
                           Math.Round(decimal.Parse(lblAmount.Text), 0) + "','" + dt.Rows[0]["vatper"].ToString() + "','" + remarks.Text + "','" +
                           RTOLocationID + "','" + AffixationDate + "','" + hdnoldnewValue.Value.ToString() + "','" + SGSTPer + "','" + SGSTPeanmt + "','" + CGSTPer + "','" + CGSTPeramt + "','" + GSTBasicPeanmt + "','" + roundoffvalue + "')";


                    int i = Utils.ExecNonQuery(strQuery, ConnectionString);
                // Disable Button 
                    btnDownload.Visible = false;
                    Button3.Visible = true;
                    if (i > 0)
                    {
                        
                        lblSucMess.Visible = true;
                        lblErrMess.Visible = false;
                        btnDownload.Visible = false;
                        Button3.Visible = true;
                        lblSucMess.Text = "Record Saved Successfully";
                        string serverdate = System.DateTime.Now.ToString("dd/MM/yyyy");
                        Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                        int  u = Utils.ExecNonQuery(Query, ConnectionString);
                       // Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Invoice No'";
                       // u = Utils.ExecNonQuery(Query, ConnectionString);
                        Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Delivery Challan No'";
                        u = Utils.ExecNonQuery(Query, ConnectionString);

                        HSRP.APWebrefrence.HSRPAuthorizationService objAPService = new HSRP.APWebrefrence.HSRPAuthorizationService();
                        
                       string AuthData = objAPService.UpdateHSRPCharges(txtAuthNo.Text, Math.Round (Convert.ToDecimal(lblAmount.Text),0), serverdate);
                        //string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                       Query = "update hsrprecords set APwebserviceresp = '" + AuthData + "', APwebservicerespdate=getdate() where  HSRP_StateID='9' and HSRPRecord_AuthorizationNo ='" + lblAuthNo.Text + "'";
                        u = Utils.ExecNonQuery(Query, ConnectionString);
                      //==== ========SMS============== //

                        if (lblMobileNo.Text.Length > 0)//will change 
                        {
                            string SMSText = " Cash Rs." + Math.Round(decimal.Parse(lblAmount.Text), 0) + " Received against HSRP Auth No. " + lblAuthNo.Text + " RECEIPT No. " + cashrc + "dated  " + System.DateTime.Now.ToString("dd/MM/yyyy") + ",Including Fitting Charges-HSRP Team ";
                            string sendURL = "http://103.16.101.52:8080/bulksms/bulksms?username=sse-aphsrp&password=aphsrp&type=0&dlr=1&destination=" + lblMobileNo.Text.ToString() + "&source=APHSRP&message=" + SMSText;
                            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                            myRequest.Method = "GET";
                            WebResponse myResponse = myRequest.GetResponse();
                            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                            string result = sr.ReadToEnd();
                            sr.Close();
                            myResponse.Close();
                            System.Threading.Thread.Sleep(350);

                            Utils.ExecNonQuery("insert into APSMSDetail(RtoLocationID,AuthorizationNumber,VehicleRegNo,MobileNo,SentResponseCode,smstext) values('" + RTOLocationID + "','" + lblAuthNo.Text + "','" + lblRegNo.Text.Trim().ToUpper() + "'," + lblMobileNo.Text.ToString() + ",'" + result + "','" + SMSText + "')", ConnectionString);
                        }
                        //==============
                        lblSucMess.Text = "Record Saved Successfully..";

                       // refresh();
                        btnDownload.Visible = false;
                        Button3.Visible = true;
                    }

                    }
                    else
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please  Contact to Administor.";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblSucMess.Text = "Message : " + ex;
                return;
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            EpsionPrint();
            //Button3.Visible = false;
           
        }
        public void fillAffixationCenter()
        {

            //string sqlquery = "select Rto_Id ,AffixCenterDesc from AffixationCenters where State_Id='" + Session["UserHSRPStateID"].ToString() + "'";
            //Utils.PopulateDropDownList(ddlaffixation, sqlquery, ConnectionString, "--Select Affixation Center--");
        }

        public void EpsionPrint()
        {
            String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();


            DataTable GetAddress;
            string Address;
            string AffAddress = string.Empty;
            string sqlquery = string.Empty;
            decimal sgstno;
            decimal cgstno;
            GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + Session["UserHSRPStateID"].ToString() + "'", ConnectionString);

            if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
            {
                Address = " - " + GetAddress.Rows[0]["pincode"];
            }
            else
            {
                Address = "";
            }
            string getTinNo = GetAddress.Rows[0]["GSTIN"].ToString();
            string getExciseNo = GetAddress.Rows[0]["ExciseNo"].ToString();

            sgstno = Convert.ToDecimal(GetAddress.Rows[0]["SGSTPer"].ToString());
            cgstno = Convert.ToDecimal(GetAddress.Rows[0]["CGSTPer"].ToString());

            sqlquery = "select * from AffixationCenters where State_Id='" + Session["UserHSRPStateID"].ToString() + "' and Rto_Id='" + Session["UserRTOLocationID"].ToString() + "'";
            sqlquery = "select Address1 from AffixationCenters where Rto_Id='" + RTOLocationID + "'";
            AffAddress = Utils.getDataSingleValue(sqlquery, ConnectionString, "Address1");
            //string SQLString = "select convert(varchar(15),PlateAffixationDate,103) as PlateAffixationDate,hsrprecordID,CashReceiptNo, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,convert(varchar(12),OwnerName)as OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,,SGSTPer,SGSTAmount,CGSTPer,CGSTAmount,GSTBasicAmount  from hsrprecords where  hsrp_stateid=9 and HSRPRecord_AuthorizationNo ='" + txtAuthNo.Text.Replace(" ", "") + "'";
            string SQLString = "select convert(varchar(15),PlateAffixationDate,103) as PlateAffixationDate,hsrprecordID,CashReceiptNo, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,convert(varchar(12),OwnerName)as OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,SGSTPer,SGSTAmount,CGSTPer,CGSTAmount,GSTBasicAmount from hsrprecords where  hsrp_stateid=9 and HSRPRecord_AuthorizationNo ='" + txtAuthNo.Text.Replace(" ", "") + "'";
            dataSetFillHSRPDeliveryChallan = Utils.GetDataTable(SQLString, ConnectionString);

            DataProvider.BAL obj = new DataProvider.BAL();
            if (dataSetFillHSRPDeliveryChallan.Rows.Count > 0)
            {
                string strquery1 = "select convert(varchar(15),PlateAffixationDate,103) as Date from hsrprecords where HSRPRecord_AuthorizationNo='" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "'";
                string date1 = Utils.getDataSingleValue(strquery1, ConnectionString, "Date");

                //string filename = "CASH RECEIPT-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                string filename = "CASHRECEIPT_No-" + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString().Replace("/", "-") + ".pdf";
               // string strvehno = dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString();

                String StringField = String.Empty;
                String StringAlert = String.Empty;


                //Creates an instance of the iTextSharp.text.Document-object:
                Document document = new Document();
                float imageWidth = 221;
                float imageHeight = 422;
                document.SetMargins(0, 0, 5, 0);
                document.SetPageSize(new iTextSharp.text.Rectangle(imageWidth, imageHeight));

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                //Adds content to the document:
                // document.Add(new Paragraph("Ignition Log Report"));
                PdfPTable table = new PdfPTable(2);
                PdfPTable table1 = new PdfPTable(2);
                PdfPTable table2 = new PdfPTable(2);
                //actual width of table in points
                //table.TotalWidth = 100f;
                string auths = string.Empty;
                //fix the absolute width of the table

                PdfPCell cell312 = new PdfPCell(new Phrase("Original", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell312.Colspan = 2;
                cell312.BorderColor = BaseColor.WHITE;
                cell312.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell312);

                PdfPCell cell312a = new PdfPCell(new Phrase("Duplicate", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell312a.Colspan = 2;
                cell312a.BorderColor = BaseColor.WHITE;
                cell312a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cell312a);

                PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 2;
                cell12.BorderColor = BaseColor.WHITE;
                cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);

                PdfPCell cell1203 = new PdfPCell(new Phrase(AffAddress, new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1203.Colspan = 2;
                cell1203.BorderWidthLeft = 0f;
                cell1203.BorderWidthRight = 0f;
                cell1203.BorderWidthTop = 0f;
                cell1203.BorderWidthBottom = 0f;
                cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1203);



                PdfPCell cell0 = new PdfPCell(new Phrase("HSRP CASH RECEIPT", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK)));
                cell0.Colspan = 2;
                cell0.BorderWidthLeft = 0f;
                cell0.BorderWidthRight = 0f;
                cell0.BorderWidthTop = 0f;
                cell0.BorderWidthBottom = 0f;

                cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell0);


                PdfPCell cell1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1.Colspan = 2;
                cell1.BorderWidthLeft = 0f;
                cell1.BorderWidthRight = 0f;
                cell1.BorderWidthTop = 0f;
                cell1.BorderWidthBottom = 0f;
                cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1);




                PdfPCell cellInv2 = new PdfPCell(new Phrase("RECEIPT No", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellInv2.Colspan = 0;

                cellInv2.BorderWidthLeft = 0f;
                cellInv2.BorderWidthRight = 0f;
                cellInv2.BorderWidthTop = 0f;
                cellInv2.BorderWidthBottom = 0f;
                cellInv2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellInv2);
                

                PdfPCell cellInv22111 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellInv22111.Colspan = 0;
                cellInv22111.BorderWidthLeft = 0f;
                cellInv22111.BorderWidthRight = 0f;
                cellInv22111.BorderWidthTop = 0f;
                cellInv22111.BorderWidthBottom = 0f;
                cellInv22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellInv22111);


                PdfPCell cell21 = new PdfPCell(new Phrase("DATE", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell21.Colspan = 0;

                cell21.BorderWidthLeft = 0f;
                cell21.BorderWidthRight = 0f;
                cell21.BorderWidthTop = 0f;
                cell21.BorderWidthBottom = 0f;
                cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell21);
                string CashReceiptDateTime = string.Empty;

                if (dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"].ToString() == "")
                {
                    CashReceiptDateTime = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    CashReceiptDateTime = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("dd/MM/yyyy");
                }
                PdfPCell cell212 = new PdfPCell(new Phrase(": " + CashReceiptDateTime, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell212.Colspan = 0;

                cell212.BorderWidthLeft = 0f;
                cell212.BorderWidthRight = 0f;
                cell212.BorderWidthTop = 0f;
                cell212.BorderWidthBottom = 0f;
                cell212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell212);



                // PdfPCell cell2 = new PdfPCell(new Phrase("TIN NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                PdfPCell cell2 = new PdfPCell(new Phrase("GSTIN", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2.Colspan = 0;

                cell2.BorderWidthLeft = 0f;
                cell2.BorderWidthRight = 0f;
                cell2.BorderWidthTop = 0f;
                cell2.BorderWidthBottom = 0f;
                cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2);


                 PdfPCell cell22111 = new PdfPCell(new Phrase(": " + getTinNo.ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //PdfPCell cell22111 = new PdfPCell(new Phrase(": ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell22111.Colspan = 0;
                cell22111.BorderWidthLeft = 0f;
                cell22111.BorderWidthRight = 0f;
                cell22111.BorderWidthTop = 0f;
                cell22111.BorderWidthBottom = 0f;
                cell22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell22111);

                PdfPCell cell22 = new PdfPCell(new Phrase("TIME", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell22.Colspan = 0;

                cell22.BorderWidthLeft = 0f;
                cell22.BorderWidthRight = 0f;
                cell22.BorderWidthTop = 0f;
                cell22.BorderWidthBottom = 0f;
                cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell22);

                PdfPCell cell222 = new PdfPCell(new Phrase(": " + Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("hh:mm:ss tt"), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell222.Colspan = 0;

                cell222.BorderWidthLeft = 0f;
                cell222.BorderWidthRight = 0f;
                cell222.BorderWidthTop = 0f;
                cell222.BorderWidthBottom = 0f;
                cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell222);

                PdfPCell cell5 = new PdfPCell(new Phrase("AUTH. NO.", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell5.Colspan = 0;

                cell5.BorderWidthLeft = 0f;
                cell5.BorderWidthRight = 0f;
                cell5.BorderWidthTop = 0f;
                cell5.BorderWidthBottom = 0f;
                cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell5);

                PdfPCell cell55 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell55.Colspan = 0;

                cell55.BorderWidthLeft = 0f;
                cell55.BorderWidthRight = 0f;
                cell55.BorderWidthTop = 0f;
                cell55.BorderWidthBottom = 0f;
                cell55.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell55);

                PdfPCell cell25 = new PdfPCell(new Phrase("AUTH. DATE", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell25.Colspan = 0;

                cell25.BorderWidthLeft = 0f;
                cell25.BorderWidthRight = 0f;
                cell25.BorderWidthTop = 0f;
                cell25.BorderWidthBottom = 0f;
                cell25.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell25);

                if (auths == "")
                {
                    PdfPCell cell255 = new PdfPCell(new Phrase(": " + auths, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell255.Colspan = 0;

                    cell255.BorderWidthLeft = 0f;
                    cell255.BorderWidthRight = 0f;
                    cell255.BorderWidthTop = 0f;
                    cell255.BorderWidthBottom = 0f;
                    cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell255);

                }
                else
                {
                    DateTime AuthDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
                    PdfPCell cell255 = new PdfPCell(new Phrase(": " + AuthDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell255.Colspan = 0;

                    cell255.BorderWidthLeft = 0f;
                    cell255.BorderWidthRight = 0f;
                    cell255.BorderWidthTop = 0f;
                    cell255.BorderWidthBottom = 0f;
                    cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell255);

                }
                PdfPCell cell7 = new PdfPCell(new Phrase("OWNER NAME", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell7.Colspan = 0;
                cell7.BorderWidthLeft = 0f;
                cell7.BorderWidthRight = 0f;
                cell7.BorderWidthTop = 0f;
                cell7.BorderWidthBottom = 0f;
                cell7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell7);

                PdfPCell cell75 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell75.Colspan = 0;
                cell75.BorderWidthLeft = 0f;
                cell75.BorderWidthRight = 0f;
                cell75.BorderWidthTop = 0f;
                cell75.BorderWidthBottom = 0f;
                cell75.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell75);

                PdfPCell cell29 = new PdfPCell(new Phrase("CONTACT NO.", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell29.Colspan = 0;
                cell29.BorderWidthLeft = 0f;
                cell29.BorderWidthRight = 0f;
                cell29.BorderWidthTop = 0f;
                cell29.BorderWidthBottom = 0f;
                cell29.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell29);

                PdfPCell cell295 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell295.Colspan = 0;
                cell295.BorderWidthLeft = 0f;
                cell295.BorderWidthRight = 0f;
                cell295.BorderWidthTop = 0f;
                cell295.BorderWidthBottom = 0f;
                cell295.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell295);



                PdfPCell cell9 = new PdfPCell(new Phrase("VEHICLE REG.", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell9.Colspan = 0;
                cell9.BorderWidthLeft = 0f;
                cell9.BorderWidthRight = 0f;
                cell9.BorderWidthTop = 0f;
                cell9.BorderWidthBottom = 0f;
                cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell9);

                PdfPCell cell95 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell95.Colspan = 0;
                cell95.BorderWidthLeft = 0f;
                cell95.BorderWidthRight = 0f;
                cell95.BorderWidthTop = 0f;
                cell95.BorderWidthBottom = 0f;
                cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell95);

                PdfPCell cell10 = new PdfPCell(new Phrase("VEHICLE MODEL", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell10.Colspan = 0;
                cell10.BorderWidthLeft = 0f;
                cell10.BorderWidthRight = 0f;
                cell10.BorderWidthTop = 0f;
                cell10.BorderWidthBottom = 0f;
                cell10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell10);

                PdfPCell cell105 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell105.Colspan = 0;
                cell105.BorderWidthLeft = 0f;
                cell105.BorderWidthRight = 0f;
                cell105.BorderWidthTop = 0f;
                cell105.BorderWidthBottom = 0f;
                cell105.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell105);

                PdfPCell cell11 = new PdfPCell(new Phrase(" VEHICLE CLASS ", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell11.Colspan = 0;
                cell11.BorderWidthLeft = 0f;
                cell11.BorderWidthRight = 0f;
                cell11.BorderWidthTop = 0f;
                cell11.BorderWidthBottom = 0f;
                cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell11);

                PdfPCell cell115 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell115.Colspan = 0;
                cell115.BorderWidthLeft = 0f;
                cell115.BorderWidthRight = 0f;
                cell115.BorderWidthTop = 0f;
                cell115.BorderWidthBottom = 0f;
                cell115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell115);

                // Change GST Amount 

                PdfPCell cell11goods = new PdfPCell(new Phrase("DESCRIPTION OF GOODS ", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell11goods.Colspan = 0;
                cell11goods.BorderWidthLeft = 0f;
                cell11goods.BorderWidthRight = 0f;
                cell11goods.BorderWidthTop = 0f;
                cell11goods.BorderWidthBottom = 0f;
                cell11goods.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell11goods);

                PdfPCell cell11577 = new PdfPCell(new Phrase(": HSRP (HSN8310)", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell11577.Colspan = 0;
                cell11577.BorderWidthLeft = 0f;
                cell11577.BorderWidthRight = 0f;
                cell11577.BorderWidthTop = 0f;
                cell11577.BorderWidthBottom = 0f;
                cell11577.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell11577);

                PdfPCell cellNet120 = new PdfPCell(new Phrase("HSRP FEES (Rs.)", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellNet120.Colspan = 0;
                cellNet120.BorderWidthLeft = 0f;
                cellNet120.BorderWidthRight = 0f;
                cellNet120.BorderWidthTop = 0f;
                cellNet120.BorderWidthBottom = 0f;
                cellNet120.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellNet120);

                decimal amt = decimal.Round(Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["GSTBasicAmount"].ToString()), 2);
                decimal tax = decimal.Round(Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["SGSTAmount"].ToString()), 2);
                decimal tax1 = decimal.Round(Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["CGSTAmount"].ToString()), 2);

                //decimal roundvalue = decimal.Round(Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["roundoff_value"].ToString()), 2);
                //amt = amt + (-roundvalue);


                PdfPCell cell1205 = new PdfPCell(new Phrase(": " + amt.ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1205.Colspan = 0;
                cell1205.BorderWidthLeft = 0f;
                cell1205.BorderWidthRight = 0f;
                cell1205.BorderWidthTop = 0f;
                cell1205.BorderWidthBottom = 0f;
                cell1205.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1205);

                PdfPCell cellNet120SGST = new PdfPCell(new Phrase("CGST @14% ", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellNet120SGST.Colspan = 0;
                cellNet120SGST.BorderWidthLeft = 0f;
                cellNet120SGST.BorderWidthRight = 0f;
                cellNet120SGST.BorderWidthTop = 0f;
                cellNet120SGST.BorderWidthBottom = 0f;
                cellNet120SGST.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellNet120SGST);

                PdfPCell cell125 = new PdfPCell(new Phrase(": " + tax1, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell125.Colspan = 0;
                cell125.BorderWidthLeft = 0f;
                cell125.BorderWidthRight = 0f;
                cell125.BorderWidthTop = 0f;
                cell125.BorderWidthBottom = 0f;
                cell125.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell125);

                PdfPCell cellNet120CGST = new PdfPCell(new Phrase("SGST @14%", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellNet120CGST.Colspan = 0;
                cellNet120CGST.BorderWidthLeft = 0f;
                cellNet120CGST.BorderWidthRight = 0f;
                cellNet120CGST.BorderWidthTop = 0f;
                cellNet120CGST.BorderWidthBottom = 0f;
                cellNet120CGST.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellNet120CGST);



                PdfPCell cell205 = new PdfPCell(new Phrase(": " + tax, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell205.Colspan = 0;
                cell205.BorderWidthLeft = 0f;
                cell205.BorderWidthRight = 0f;
                cell205.BorderWidthTop = 0f;
                cell205.BorderWidthBottom = 0f;
                cell205.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell205);

                //End of GST

                PdfPCell celldupCash401 = new PdfPCell(new Phrase("TOTAL AMOUNT", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                celldupCash401.Colspan = 0;
                celldupCash401.BorderWidthLeft = 0f;
                celldupCash401.BorderWidthRight = 0f;
                celldupCash401.BorderWidthTop = 0f;
                celldupCash401.BorderWidthBottom = 0f;
                celldupCash401.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash401);


                decimal roundAmt = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString());
                //roundAmt = Math.Round(roundAmt + roundvalue, 0);
                roundAmt = Math.Round(roundAmt, 0);

                PdfPCell celldupCash402 = new PdfPCell(new Phrase(": " + roundAmt, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                celldupCash402.Colspan = 0;
                celldupCash402.BorderWidthLeft = 0f;
                celldupCash402.BorderWidthRight = 0f;
                celldupCash402.BorderWidthTop = 0f;
                celldupCash402.BorderWidthBottom = 0f;
                celldupCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash402);
                //PdfPCell celldupCash40z1 = new PdfPCell(new Phrase("(Inclusive of All Tax)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                PdfPCell cellkLk1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellkLk1.Colspan = 2;
                cellkLk1.BorderWidthLeft = 0f;
                cellkLk1.BorderWidthRight = 0f;
                cellkLk1.BorderWidthTop = 0f;
                cellkLk1.BorderWidthBottom = 0f;
                cellkLk1.HorizontalAlignment = 0;
                table.AddCell(cellkLk1);

                PdfPCell celldupRouCash402 = new PdfPCell(new Phrase("Disclaimer :", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash402.Colspan = 2;
                celldupRouCash402.BorderWidthLeft = 0f;
                celldupRouCash402.BorderWidthRight = 0f;
                celldupRouCash402.BorderWidthTop = 0f;
                celldupRouCash402.BorderWidthBottom = 0f;
                celldupRouCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash402);

                PdfPCell cellkk10 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellkk10.Colspan = 2;
                cellkk10.BorderWidthLeft = 0f;
                cellkk10.BorderWidthRight = 0f;
                cellkk10.BorderWidthTop = 0f;
                cellkk10.BorderWidthBottom = 0f;
                cellkk10.HorizontalAlignment = 0;
                table.AddCell(cellkk10);



                //if (strvehno.ToString().Trim() == "")
                if (lblRegNo.Text.Trim() == "")
                {
                    PdfPCell cellk1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellk1.Colspan = 2;
                    cellk1.BorderWidthLeft = 0f;
                    cellk1.BorderWidthRight = 0f;
                    cellk1.BorderWidthTop = 0f;
                    cellk1.BorderWidthBottom = 0f;

                    cellk1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellk1);

                    PdfPCell cellkk1 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellkk1.Colspan = 2;
                    cellkk1.BorderWidthLeft = 0f;
                    cellkk1.BorderWidthRight = 0f;
                    cellkk1.BorderWidthTop = 0f;
                    cellkk1.BorderWidthBottom = 0f;
                    cellkk1.HorizontalAlignment = 0;
                    table.AddCell(cellkk1);

                   

                    string Messagenew = "\u2022" + " Payment against your authorization has been received. HSRP will be processed once the vehicle registration no.  will be provided by the department.";

                    PdfPCell cellnew = new PdfPCell(new Phrase(Messagenew, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellnew.Colspan = 2;
                    cellnew.BorderWidthLeft = 0f;
                    cellnew.BorderWidthRight = 0f;
                    cellnew.BorderWidthTop = 0f;
                    cellnew.BorderWidthBottom = 0f;
                    cellnew.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellnew);

                    PdfPCell cellkkk1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellkkk1.Colspan = 2;
                    cellkkk1.BorderWidthLeft = 0f;
                    cellkkk1.BorderWidthRight = 0f;
                    cellkkk1.BorderWidthTop = 0f;
                    cellkkk1.BorderWidthBottom = 0f;
                    cellkkk1.HorizontalAlignment = 0;
                    table.AddCell(cellkkk1);

                    PdfPCell cellkkk11 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellkkk11.Colspan = 2;
                    cellkkk11.BorderWidthLeft = 0f;
                    cellkkk11.BorderWidthRight = 0f;
                    cellkkk11.BorderWidthTop = 0f;
                    cellkkk11.BorderWidthBottom = 0f;
                    cellkkk11.HorizontalAlignment = 0;
                    table.AddCell(cellkkk11);

                }
                PdfPCell cellkLk11 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellkLk11.Colspan = 2;
                cellkLk11.BorderWidthLeft = 0f;
                cellkLk11.BorderWidthRight = 0f;
                cellkLk11.BorderWidthTop = 0f;
                cellkLk11.BorderWidthBottom = 0f;
                cellkLk11.HorizontalAlignment = 0;
                table.AddCell(cellkLk11);

                string Messageff = "\u2022" + " Fitment Charges Included.";
                //string Messageff = "\u2022" + " Vehicle Owner is requested to please check the Correctness of the cash slip.";
                PdfPCell cell64ff = new PdfPCell(new Phrase(Messageff, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell64ff.Colspan = 2;
                cell64ff.BorderWidthLeft = 0f;
                cell64ff.BorderWidthRight = 0f;
                cell64ff.BorderWidthTop = 0f;
                cell64ff.BorderWidthBottom = 0f;
                cell64ff.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell64ff);


                string Message = "\u2022" + " Vehicle Owner is requested to please check the Correctness of the cash slip.";
                //string Message = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP, on the fourth working day from the date of  issuance of cash receipt.";
                PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell64.Colspan = 2;
                cell64.BorderWidthLeft = 0f;
                cell64.BorderWidthRight = 0f;
                cell64.BorderWidthTop = 0f;
                cell64.BorderWidthBottom = 0f;
                cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell64);


                string MessageSec = "\u2022" + " Before leaving the counter verify your data on cash receipt. The company shall not be responsible for any clerical mistake what so ever.";

                PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell65a.Colspan = 4;
                cell65a.BorderWidthLeft = 0f;
                cell65a.BorderWidthRight = 0f;
                cell65a.BorderWidthTop = 0f;
                cell65a.BorderWidthBottom = 0f;
                cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell65a);

                PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell63.Colspan = 2;
                cell63.BorderWidthLeft = 0f;
                cell63.BorderWidthRight = 0f;
                cell63.BorderWidthTop = 0f;
                cell63.BorderWidthBottom = 0f;
                cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell63);

                if (lblRegNo.Text.Trim() != "")
                //if (strvehno.ToString().Trim() != "")
                {
                    PdfPCell cellkLk21 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellkLk21.Colspan = 2;
                    cellkLk21.BorderWidthLeft = 0f;
                    cellkLk21.BorderWidthRight = 0f;
                    cellkLk21.BorderWidthTop = 0f;
                    cellkLk21.BorderWidthBottom = 0f;
                    cellkLk21.HorizontalAlignment = 0;
                    table.AddCell(cellkLk21);

                    PdfPCell cellkLk201 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellkLk201.Colspan = 2;
                    cellkLk201.BorderWidthLeft = 0f;
                    cellkLk201.BorderWidthRight = 0f;
                    cellkLk201.BorderWidthTop = 0f;
                    cellkLk201.BorderWidthBottom = 0f;
                    cellkLk201.HorizontalAlignment = 0;
                    table.AddCell(cellkLk201);
                    PdfPCell cell163 = new PdfPCell(new Phrase("--Affixation--", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell163.Colspan = 4;
                    cell163.BorderWidthLeft = 0f;
                    cell163.BorderWidthRight = 0f;
                    cell163.BorderWidthTop = 0f;
                    cell163.BorderWidthBottom = 0f;
                    cell163.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell163);

                    PdfPCell cellkLk0211 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellkLk0211.Colspan = 2;
                    cellkLk0211.BorderWidthLeft = 0f;
                    cellkLk0211.BorderWidthRight = 0f;
                    cellkLk0211.BorderWidthTop = 0f;
                    cellkLk0211.BorderWidthBottom = 0f;
                    cellkLk0211.HorizontalAlignment = 0;
                    table.AddCell(cellkLk0211);
                    PdfPCell cellkLk1201 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellkLk1201.Colspan = 2;
                    cellkLk1201.BorderWidthLeft = 0f;
                    cellkLk1201.BorderWidthRight = 0f;
                    cellkLk1201.BorderWidthTop = 0f;
                    cellkLk1201.BorderWidthBottom = 0f;
                    cellkLk1201.HorizontalAlignment = 0;
                    table.AddCell(cellkLk1201);

                    PdfPCell cell13 = new PdfPCell(new Phrase("Date : " + date1 + " dd/mm/yyyy", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell13.Colspan = 4;
                    cell13.BorderWidthLeft = 0f;
                    cell13.BorderWidthRight = 0f;
                    cell13.BorderWidthTop = 0f;
                    cell13.BorderWidthBottom = 0f;
                    cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell13);
                    PdfPCell cell1213 = new PdfPCell(new Phrase("Time : 2:00 PM - 6:00 PM", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1213.Colspan = 4;
                    cell1213.BorderWidthLeft = 0f;
                    cell1213.BorderWidthRight = 0f;
                    cell1213.BorderWidthTop = 0f;
                    cell1213.BorderWidthBottom = 0f;
                    cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1213);
                    PdfPCell cell123 = new PdfPCell(new Phrase("Place : " + AffAddress, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell123.Colspan = 4;
                    cell123.BorderWidthLeft = 0f;
                    cell123.BorderWidthRight = 0f;
                    cell123.BorderWidthTop = 0f;
                    cell123.BorderWidthBottom = 0f;
                    cell123.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell123);
                }
                PdfPCell cellkLk111 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellkLk111.Colspan = 2;
                cellkLk111.BorderWidthLeft = 0f;
                cellkLk111.BorderWidthRight = 0f;
                cellkLk111.BorderWidthTop = 0f;
                cellkLk111.BorderWidthBottom = 0f;
                cellkLk111.HorizontalAlignment = 0;
                table.AddCell(cellkLk111);

                PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell62.Colspan = 2;
                cell62.BorderWidthLeft = 0f;
                cell62.BorderWidthRight = 0f;
                cell62.BorderWidthTop = 0f;
                cell62.BorderWidthBottom = 0f;
                cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell62);

                //////////////////////////////////////Duplicate CashRecipt/////////////////////////////////////////////////////////////////////////////////////////////


                document.Add(table1);
                document.Add(table);

                document.Add(table2);
                document.Add(table);

                document.Close();
                HttpContext context = HttpContext.Current;
                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();

               
            }
        }
                      
    }
}