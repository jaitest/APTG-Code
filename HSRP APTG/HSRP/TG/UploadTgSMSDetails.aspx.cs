﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using Excel;
using ICSharpCode;
using System.Globalization;

namespace HSRP.TG
{
    public partial class UploadTgSMSDetails : System.Web.UI.Page
    {
        string SaveLocation = string.Empty;
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        string getExcelSheetName = string.Empty;
        string Id = string.Empty;
        string HSRP_StateID = string.Empty, RTOLocationID = string.Empty;
        int RTOLocation_ID;
        bool FlagIsDirty = true;
        int UserType;
        string strUserID = string.Empty;

        string strEmbID = string.Empty;
        string userdealerid = string.Empty;
        string orderno = string.Empty;
        string CnnStringupload = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Utils.GZipEncodePage();
                lbltotaluploadrecords.Text = "";
                lbltotladuplicaterecords.Text = "";
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }
                else
                {
                    RTOLocation_ID = Convert.ToInt32(Session["UserRTOLocationID"]);
                    UserType = Convert.ToInt32(Session["UserType"]);
                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    strUserID = Session["UID"].ToString();
                    //  userdealerid = Session["userdealerid"].ToString();
                    Id = Session["UID"].ToString();
                    HSRP_StateID = Session["UserHSRPStateID"].ToString();
                    if (!IsPostBack)
                    {



                    }
                }
            }
            catch (Exception ex)
            {

                llbMSGError.Text = ex.Message.ToString();
            }
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            llbMSGError.Text = string.Empty;
            try
            {
                DataTable dtExcelRecords = new DataTable();
                if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
                {
                    InsertDataInstage();
                }
                else
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Please Select a file to Upload.";
                    return;
                }
            }
            catch (Exception ex)
            {
                llbMSGError.Text = "Error in Upload File :- " + ex.Message.ToString();
            }
        }

        string fileLocation = string.Empty;

        private void InsertDataInstage()
        {
            try
            {
                string filename = "TGOnlineSMSDetail-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                string fileExtension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                //string fileLocation = @"C:\\\\DealerFolder\\\\";
                string fileLocation = System.Configuration.ConfigurationManager.AppSettings["DealerFolder"].ToString();
                if (!Directory.Exists(fileLocation))
                {
                    Directory.CreateDirectory(fileLocation);
                }
                fileLocation += filename.Replace("\\\\", "\\");
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Please Upload Excel File. ";
                    return;
                }
                IExcelDataReader excelReader;
                DataTable dtExcelRecords = new DataTable();
                FileUpload1.PostedFile.SaveAs(fileLocation);

                FileStream stream = File.Open(fileLocation, FileMode.Open, FileAccess.Read);


                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                if (fileExtension != ".xls")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    llbMSGError.Text = "The Excel File must be in .xls Format..Kindly Convert Your  File into .xls format";

                    return;
                }


                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();

                if (result.Tables[0].Rows.Count == 0)
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Records are not available in file.";
                    return;
                }

                excelReader.Close();
                if (result.Tables[0].Rows.Count > 0)
                {
                    //   ValidationCheckOnRecords(result.Tables[0]);
                    //if (FlagIsDirty)
                    //{
                    //    return;
                    //}
                    InsertionOfRecords(result.Tables[0]);
                    if (FlagIsDirty)
                    {
                        return;
                    }
                }
                else
                {
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Recrd Not Exist in excel file";
                }
                if (File.Exists(fileLocation))
                {

                    File.Delete(fileLocation);

                }


            }
            catch (Exception ee)
            {
                llbMSGError.Text = ee.Message.ToString();
                //lbltotladuplicaterecords.Text = countDuplicate.ToString();
                if (File.Exists(fileLocation))
                {
                    File.Delete(fileLocation);
                }
                llbMSGError.Text = ee.Message.ToString();
                
            }


        }
        int countDuplicate = 0, countupload = 0, errorinexcel = 0;
        string tt = string.Empty;

        private void InsertionOfRecords(DataTable dt)
        {
            try
            {
                string MessageId = string.Empty;
                string MobileNo = string.Empty;
                string Message = string.Empty;
                string Message_Type = string.Empty;
                string Campaign_Name = string.Empty;
                string Length1 = string.Empty;
                string Sender = string.Empty;
                string Country = string.Empty;
                string Operator = string.Empty;
                string CreditsDeducted = string.Empty;
                string DLRStatus = string.Empty;
                string SentDate = string.Empty;
                string DoneDate = string.Empty;
                string DlrErrExp = string.Empty;
                string vehicleregno = string.Empty;
                string authno = string.Empty;
                


                string UploadedDateTime = System.DateTime.Now.Day.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Year.ToString();

                StringBuilder sb = new StringBuilder();

                if (dt.Columns[0].ColumnName.ToString() != "MessageId")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[0].ColumnName.ToString() + " is not valid please Enter valid column name <b>MessageId</b>.";
                    return;
                }
                if (dt.Columns[1].ColumnName.ToString() != "MobileNo")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[1].ColumnName.ToString() + " is not valid please Enter valid column name <b>MobileNo</b>.";
                    return;

                }
                if (dt.Columns[2].ColumnName.ToString() != "Message")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[2].ColumnName.ToString() + " is not valid please Enter valid column name <b>Message</b>.";
                    return;
                }

                if (dt.Columns[3].ColumnName.ToString() != "Type")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[3].ColumnName.ToString() + " is not valid please Enter valid column name <b>Type</b>.";
                    return;
                }


                if (dt.Columns[4].ColumnName.ToString() != "Campaign Name")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[4].ColumnName.ToString() + " is not valid please Enter valid column name <b>Campaign Name</b>.";
                    return;
                }


                if (dt.Columns[5].ColumnName.ToString() != "Length")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[5].ColumnName.ToString() + " is not valid please Enter valid column name <b>Length</b>.";
                    return;
                }


                if (dt.Columns[6].ColumnName.ToString() != "Sender")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[6].ColumnName.ToString() + " is not valid please Enter valid column name <b>Sender</b>.";
                    return;
                }

                if (dt.Columns[7].ColumnName.ToString() != "Country")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[7].ColumnName.ToString() + " is not valid please Enter valid column name <b>Country</b>.";
                    return;
                }

                if (dt.Columns[8].ColumnName.ToString() != "Operator")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[8].ColumnName.ToString() + " is not valid please Enter valid column name <b>Operator</b>.";
                    return;
                }


                if (dt.Columns[9].ColumnName.ToString() != "CreditsDeducted")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[9].ColumnName.ToString() + " is not valid please Enter valid column name <b>CreditsDeducted</b>.";
                    return;
                }


                if (dt.Columns[10].ColumnName.ToString() != "DLRStatus")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[10].ColumnName.ToString() + " is not valid please Enter valid column name <b>DLRStatus</b>.";
                    return;
                }

                if (dt.Columns[11].ColumnName.ToString() != "SentDate")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[11].ColumnName.ToString() + " is not valid please Enter valid column name <b>SentDate</b>.";
                    return;
                }

                if (dt.Columns[12].ColumnName.ToString() != "DoneDate")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[12].ColumnName.ToString() + " is not valid please Enter valid column name <b>DoneDate</b>.";
                    return;
                }
                if (dt.Columns[13].ColumnName.ToString() != "DlrErrExp")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[13].ColumnName.ToString() + " is not valid please Enter valid column name <b>DlrErrExp</b>.";
                    return;
                }
                if (dt.Columns[14].ColumnName.ToString() != "vehicleregno")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[14].ColumnName.ToString() + " is not valid please Enter valid column name <b>vehicleregno</b>.";
                    return;
                }
                if (dt.Columns[15].ColumnName.ToString() != "authno")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[15].ColumnName.ToString() + " is not valid please Enter valid column name <b>authno</b>.";
                    return;
                }
                //ValidationCheckOnRecords(dt);
                if (FlagIsDirty == false)
                {
                    return;
                }
                CreateDataTable();
                DataTable Appdt = (DataTable)Session["TgOnlineSMS"];
                foreach (DataRow dr in dt.Rows)
                {
                    MessageId = dr["MessageId"].ToString().Trim();
                    MobileNo = dr["MobileNo"].ToString().Trim();
                    Message = dr["Message"].ToString().Trim();
                    Message_Type = dr["Type"].ToString().Trim();
                    Campaign_Name = dr["Campaign Name"].ToString().Trim();
                    Length1 = dr["Length"].ToString().Trim();
                    Sender = dr["Sender"].ToString().Trim();
                    Country = dr["Country"].ToString().Trim();
                    Operator = dr["Operator"].ToString().Trim();
                    CreditsDeducted = dr["CreditsDeducted"].ToString().Trim();
                    DLRStatus = dr["DLRStatus"].ToString().Trim();

                    if (dr["SentDate"].ToString().Trim() != "")
                    {
                        SentDate = dr["SentDate"].ToString();
                    }
                    if (dr["DoneDate"].ToString().Trim() != "")
                    {
                        DoneDate = dr["DoneDate"].ToString();
                    }
                    if (dr["DlrErrExp"].ToString().Trim() != "")
                    {
                        DlrErrExp = dr["DlrErrExp"].ToString();
                    }
                    if (dr["vehicleregno"].ToString().Trim() != "")
                    {
                        vehicleregno = dr["vehicleregno"].ToString();
                    }
                    if (dr["authno"].ToString().Trim() != "")
                    {
                        authno = dr["authno"].ToString();
                    }
                    DataRow dr1 = Appdt.NewRow();
                    dr1["MessageId"] = MessageId;
                    dr1["MobileNo"] = MobileNo; ;
                    dr1["Message1"] = Message;
                    dr1["Message_Type"] = Message_Type;
                    if (Campaign_Name != "")
                    {
                        dr1["Campaign_Name"] = Campaign_Name;
                    }
                    dr1["Length1"] = Length1;
                    dr1["Sender"] = Sender;
                    dr1["Country"] = Country;
                    dr1["Operator"] = Operator;
                    dr1["CreditsDeducted"] = CreditsDeducted;
                    dr1["DLRStatus"] = DLRStatus;
                    dr1["SentDate"] = SentDate;
                    dr1["DoneDate"] = DoneDate;
                    dr1["DlrErrExp"] = DlrErrExp;
                    dr1["vehicleregno"] = vehicleregno;
                    dr1["authno"] = authno;
                    Appdt.Rows.Add(dr1);
                    dr1.AcceptChanges();
                    //sb.Append("insert into TgOnlinePaymentReco1 (PaymentGateway,PaymentOrderId,OnlinePaymentID,OrderDate,ShippingDate,AuthorizationNo,OwnerName,RTOLocation,DearlerName,BankTransactionId,TotalAmount,Charges,ServiceTax,NetAmount,PaymentDate,CreatedBy,CreationDate) values('" + PaymentGateway + "','" + PaymentOrderId + "','" + OnlinePaymentID + "','" + OrderDate + "','" + ShippingDate + "','" + AuthorizationNo + "','" + OwnerName + "','" + RTOLocation + "','" + DearlerName + "','" + BankTransactionId + "'," + TotalAmount + "," + Charges + "," + ServiceTax + "," + NetAmount + ",'" + PaymentDate + "'," + Convert.ToInt32(strUserID) + ",getdate());");

                }
                DataSet ds2 = new DataSet("emp");
                ds2.Tables.Add(Appdt);
                string strData = ds2.GetXml();
                DataTable dt1 = Utils.GetDataTable("InsertIntoTgOnlineSMS '" + strData + "'", CnnStringupload);
                if (dt1.Rows.Count > 0)
                {
                    if (dt1.Rows[0]["status"].ToString().Trim() != "0")
                    {
                        llbMSGSuccess.Text = "";
                        llbMSGError.Text = "";
                        llbMSGSuccess.Text = dt1.Rows[0]["msg"].ToString().Trim();
                    }
                    else
                    {
                        llbMSGError.Text = "";
                        llbMSGSuccess.Text = "";
                        llbMSGError.Text = dt1.Rows[0]["msg"].ToString().Trim();
                    }
                }
                //if (sb.ToString() != "")
                //{
                //    int i = Utils.ExecNonQuery(sb.ToString(), CnnStringupload);
                //    if (i > 0)
                //    {
                //        llbMSGSuccess.Text = "";
                //        llbMSGError.Text = "";
                //        llbMSGSuccess.Text = "Record Save Sucessfully. Your Order No : " + orderno;
                //        lbltotaluploadrecords.Text = countupload.ToString();
                //        lbltotladuplicaterecords.Text = countDuplicate.ToString();
                //    }
                //    else
                //    {
                //        llbMSGError.Text = "";
                //        llbMSGError.Text = "Record not save";
                //    }
                //}
            }
            catch (Exception ex)
            {
                llbMSGError.Text = ex.Message.ToString();
            }
        }

        private void ValidationCheckOnRecords(DataTable ExcelSheet)
        {
            try
            {
                for (int i = 0; i < ExcelSheet.Rows.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["PaymentGateway"].ToString().Trim()))
                    {
                        int a = i + 2;
                        llbMSGError.Text = "";
                        llbMSGError.Text = "Excel Sheet : Has <b>PaymentGateway</b> Field Empty At Row " + a + " Position : ";
                        FlagIsDirty = false;
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["PaymentOrderId"].ToString().Trim()))
                    {
                        int a = i + 2;
                        llbMSGError.Text = "";
                        llbMSGError.Text = "Excel Sheet : Has <b>PaymentOrderId</b> Field Empty At Row " + a + " Position : ";
                        FlagIsDirty = false;
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["OnlinePaymentID"].ToString().Trim()))
                    {
                        int a = i + 2;
                        llbMSGError.Text = "";
                        llbMSGError.Text = "Excel Sheet : Has <b>OnlinePaymentID</b> Field Empty At Row " + a + " Position : ";
                        FlagIsDirty = false;
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["OrderDate"].ToString().Trim()))
                    {
                        int a = i + 2;
                        llbMSGError.Text = "";
                        llbMSGError.Text = "Excel Sheet : Has <b>OrderDate</b> Field Empty At Row " + a + " Position : ";
                        FlagIsDirty = false;
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["TotalAmount"].ToString().Trim()))
                    {
                        int a = i + 2;
                        llbMSGError.Text = "";
                        llbMSGError.Text = "Excel Sheet : Has <b>TotalAmount</b> Field Empty At Row " + a + " Position : ";
                        FlagIsDirty = false;
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["NetAmount"].ToString().Trim()))
                    {
                        int a = i + 2;
                        llbMSGError.Text = "";
                        llbMSGError.Text = "Excel Sheet : Has <b>NetAmount</b> Field Empty At Row " + a + " Position : ";
                        FlagIsDirty = false;
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["PaymentStatus"].ToString().Trim()))
                    {
                        int a = i + 2;
                        llbMSGError.Text = "";
                        llbMSGError.Text = "Excel Sheet : Has <b>PaymentStatus</b> Field Empty At Row " + a + " Position : ";
                        FlagIsDirty = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

                llbMSGError.Text = ex.Message.ToString();
            }

        }

        private void CreateDataTable()
        {
            try
            {
                DataTable dtforgv = new DataTable();
                dtforgv.Columns.Add("MessageId", typeof(string));
                dtforgv.Columns.Add("MobileNo", typeof(string));
                dtforgv.Columns.Add("Message1", typeof(string));
                dtforgv.Columns.Add("Message_Type", typeof(string));
                dtforgv.Columns.Add("Campaign_Name", typeof(string));
                dtforgv.Columns.Add("Length1", typeof(string));
                dtforgv.Columns.Add("Sender", typeof(string));
                dtforgv.Columns.Add("Country", typeof(string));
                dtforgv.Columns.Add("Operator", typeof(string));
                dtforgv.Columns.Add("CreditsDeducted", typeof(string));
                dtforgv.Columns.Add("DLRStatus", typeof(string));
                dtforgv.Columns.Add("SentDate", typeof(string));
                dtforgv.Columns.Add("DoneDate", typeof(string));
                dtforgv.Columns.Add("DlrErrExp", typeof(string));
                dtforgv.Columns.Add("vehicleregno", typeof(string));
                dtforgv.Columns.Add("authno", typeof(string));
                Session["TgOnlineSMS"] = dtforgv;
            }
            catch (Exception ex)
            {
                llbMSGError.Text = ex.Message.ToString();
            }
        }

    }
}