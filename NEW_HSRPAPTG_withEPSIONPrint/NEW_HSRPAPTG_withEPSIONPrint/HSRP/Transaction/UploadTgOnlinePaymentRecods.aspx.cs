using System;
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

namespace HSRP.Transaction
{
    public partial class UploadTgOnlinePaymentRecods : System.Web.UI.Page
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
                string filename = "TGOnlinePaymentRecord-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
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
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
                if (File.Exists(fileLocation))
                {
                    File.Delete(fileLocation);
                }
                return;
            }


        }
        int countDuplicate = 0, countupload = 0, errorinexcel = 0;
        string tt = string.Empty;

        private void InsertionOfRecords(DataTable dt)
        {
            try
            {
                string PaymentGateway = string.Empty;
                string PaymentOrderId = string.Empty;
                string OnlinePaymentID = string.Empty;
                string OrderDate = string.Empty;
                string ShippingDate = string.Empty;
                string AuthorizationNo = string.Empty;
                string OwnerName = string.Empty;
                string RTOLocation = string.Empty;
                string DearlerName = string.Empty;
                string BankTransactionId = string.Empty;
                decimal TotalAmount = 0;
                decimal Charges = 0;
                decimal ServiceTax = 0;
                decimal NetAmount = 0;
                string PaymentDate = string.Empty;
                string PaymentStatus = string.Empty;


                orderno = "TG" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
                // string  = Convert.ToString(System.DateTime.Now);
                string UploadedDateTime = System.DateTime.Now.Day.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Year.ToString();

                StringBuilder sb = new StringBuilder();
                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    string str= dt.Columns[i].ColumnName.ToString();
                //}


                if (dt.Columns[0].ColumnName.ToString() != "PaymentGateway")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[0].ColumnName.ToString() + " is not valid please Enter valid column name <b>PaymentGateway</b>.";
                    return;
                }
                if (dt.Columns[1].ColumnName.ToString() != "PaymentOrderId")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[1].ColumnName.ToString() + " is not valid please Enter valid column name <b>PaymentOrderId</b>.";
                    return;

                }
                if (dt.Columns[2].ColumnName.ToString() != "OnlinePaymentID")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[2].ColumnName.ToString() + " is not valid please Enter valid column name <b>OnlinePaymentID</b>.";
                    return;
                }

                if (dt.Columns[3].ColumnName.ToString() != "OrderDate")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[3].ColumnName.ToString() + " is not valid please Enter valid column name <b>OrderDate</b>.";
                    return;
                }


                if (dt.Columns[4].ColumnName.ToString() != "ShippingDate")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[4].ColumnName.ToString() + " is not valid please Enter valid column name <b>ShippingDate</b>.";
                    return;
                }


                if (dt.Columns[5].ColumnName.ToString() != "AuthorizationNo")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[5].ColumnName.ToString() + " is not valid please Enter valid column name <b>AuthorizationNo</b>.";
                    return;
                }


                if (dt.Columns[6].ColumnName.ToString() != "OwnerName")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[6].ColumnName.ToString() + " is not valid please Enter valid column name <b>OwnerName</b>.";
                    return;
                }

                if (dt.Columns[7].ColumnName.ToString() != "RTOLocation")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[7].ColumnName.ToString() + " is not valid please Enter valid column name <b>RTOLocation</b>.";
                    return;
                }

                if (dt.Columns[8].ColumnName.ToString() != "DearlerName")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[8].ColumnName.ToString() + " is not valid please Enter valid column name <b>DearlerName</b>.";
                    return;
                }


                if (dt.Columns[9].ColumnName.ToString() != "BankTransactionId")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[9].ColumnName.ToString() + " is not valid please Enter valid column name <b>BankTransactionId</b>.";
                    return;
                }


                if (dt.Columns[10].ColumnName.ToString() != "TotalAmount")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[10].ColumnName.ToString() + " is not valid please Enter valid column name <b>TotalAmount</b>.";
                    return;
                }

                if (dt.Columns[11].ColumnName.ToString() != "Charges")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[11].ColumnName.ToString() + " is not valid please Enter valid column name <b>Charges</b>.";
                    return;
                }

                if (dt.Columns[12].ColumnName.ToString() != "ServiceTax")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[12].ColumnName.ToString() + " is not valid please Enter valid column name <b>ServiceTax</b>.";
                    return;
                }
                if (dt.Columns[13].ColumnName.ToString() != "NetAmount")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[13].ColumnName.ToString() + " is not valid please Enter valid column name <b>NetAmount</b>.";
                    return;
                }
                if (dt.Columns[14].ColumnName.ToString() != "PaymentDate")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[14].ColumnName.ToString() + " is not valid please Enter valid column name <b>PaymentDate</b>.";
                    return;
                }
                if (dt.Columns[15].ColumnName.ToString() != "PaymentStatus")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    FlagIsDirty = true;
                    llbMSGError.Text = dt.Columns[15].ColumnName.ToString() + " is not valid please Enter valid column name <b>PaymentStatus</b>.";
                    return;
                }
                ValidationCheckOnRecords(dt);
                if (FlagIsDirty == false)
                {
                    return;
                }
                CreateDataTable();
                DataTable Appdt = (DataTable)Session["TgOnlinePaymentReco1"];
                foreach (DataRow dr in dt.Rows)
                {
                    PaymentGateway = dr["PaymentGateway"].ToString().Trim();
                    PaymentOrderId = dr["PaymentOrderId"].ToString().Trim();
                    OnlinePaymentID = dr["OnlinePaymentID"].ToString().Trim();
                    OrderDate = dr["OrderDate"].ToString().Trim();
                    ShippingDate = dr["ShippingDate"].ToString().Trim();
                    AuthorizationNo = dr["AuthorizationNo"].ToString().Trim();
                    OwnerName = dr["OwnerName"].ToString().Trim();
                    RTOLocation = dr["RTOLocation"].ToString().Trim();
                    DearlerName = dr["DearlerName"].ToString().Trim();
                    BankTransactionId = dr["BankTransactionId"].ToString().Trim();

                    if (dr["TotalAmount"].ToString().Trim() != "")
                    {
                        TotalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                    }
                    if (dr["Charges"].ToString().Trim() != "")
                    {
                        Charges = Convert.ToDecimal(dr["Charges"]);
                    }
                    if (dr["ServiceTax"].ToString().Trim() != "")
                    {
                        ServiceTax = Convert.ToDecimal(dr["ServiceTax"]);
                    }
                    if (dr["NetAmount"].ToString().Trim() != "")
                    {
                        NetAmount = Convert.ToDecimal(dr["NetAmount"]);
                    }
                    PaymentDate = dr["PaymentDate"].ToString().Trim();
                    PaymentStatus = dr["PaymentStatus"].ToString().Trim();

                    DataRow dr1 = Appdt.NewRow();
                    dr1["PaymentGateway"] = PaymentGateway;
                    dr1["PaymentOrderId"] = PaymentOrderId;
                    dr1["OnlinePaymentID"] = OnlinePaymentID;
                    dr1["OrderDate"] = OrderDate;
                    if (ShippingDate != "")
                    {
                        dr1["ShippingDate"] = ShippingDate;
                    }
                    dr1["AuthorizationNo"] = AuthorizationNo;
                    dr1["OwnerName"] = OwnerName;
                    dr1["RTOLocation"] = RTOLocation;
                    dr1["DearlerName"] = DearlerName;
                    dr1["BankTransactionId"] = BankTransactionId;
                    dr1["TotalAmount"] = TotalAmount;
                    dr1["Charges"] = Charges;
                    dr1["ServiceTax"] = ServiceTax;
                    dr1["NetAmount"] = NetAmount;
                    if (PaymentDate != "")
                    {
                        dr1["PaymentDate"] = PaymentDate;
                    }
                    if (PaymentStatus != "")
                    {
                        dr1["PaymentStatus"] = PaymentStatus;
                    }
                    dr1["CreatedBy"] = Convert.ToInt32(strUserID);
                    Appdt.Rows.Add(dr1);
                    dr1.AcceptChanges();
                    //sb.Append("insert into TgOnlinePaymentReco1 (PaymentGateway,PaymentOrderId,OnlinePaymentID,OrderDate,ShippingDate,AuthorizationNo,OwnerName,RTOLocation,DearlerName,BankTransactionId,TotalAmount,Charges,ServiceTax,NetAmount,PaymentDate,CreatedBy,CreationDate) values('" + PaymentGateway + "','" + PaymentOrderId + "','" + OnlinePaymentID + "','" + OrderDate + "','" + ShippingDate + "','" + AuthorizationNo + "','" + OwnerName + "','" + RTOLocation + "','" + DearlerName + "','" + BankTransactionId + "'," + TotalAmount + "," + Charges + "," + ServiceTax + "," + NetAmount + ",'" + PaymentDate + "'," + Convert.ToInt32(strUserID) + ",getdate());");

                }
                DataSet ds2 = new DataSet("emp");
                ds2.Tables.Add(Appdt);
                string strData = ds2.GetXml();
                DataTable dt1 = Utils.GetDataTable("InsertIntoTgOnlinePaymentReco '" + strData + "','" + Convert.ToInt32(strUserID) + "'", CnnStringupload);
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
                dtforgv.Columns.Add("PaymentGateway", typeof(string));
                dtforgv.Columns.Add("PaymentOrderId", typeof(string));
                dtforgv.Columns.Add("OnlinePaymentID", typeof(string));
                dtforgv.Columns.Add("OrderDate", typeof(DateTime));
                dtforgv.Columns.Add("ShippingDate", typeof(DateTime));
                dtforgv.Columns.Add("AuthorizationNo", typeof(string));
                dtforgv.Columns.Add("OwnerName", typeof(string));
                dtforgv.Columns.Add("RTOLocation", typeof(string));
                dtforgv.Columns.Add("DearlerName", typeof(string));
                dtforgv.Columns.Add("BankTransactionId", typeof(string));
                dtforgv.Columns.Add("TotalAmount", typeof(decimal));
                dtforgv.Columns.Add("Charges", typeof(decimal));
                dtforgv.Columns.Add("ServiceTax", typeof(decimal));
                dtforgv.Columns.Add("NetAmount", typeof(decimal));
                dtforgv.Columns.Add("PaymentDate", typeof(DateTime));
                dtforgv.Columns.Add("PaymentStatus", typeof(string));
                dtforgv.Columns.Add("CreatedBy", typeof(int));
                Session["TgOnlinePaymentReco1"] = dtforgv;
            }
            catch (Exception ex)
            {
                llbMSGError.Text = ex.Message.ToString();
            }
        }

    }
}