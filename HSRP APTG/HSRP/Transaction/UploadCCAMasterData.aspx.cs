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
    public partial class UploadCCAMasterData : System.Web.UI.Page
    {
        string SaveLocation = string.Empty;
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        string getExcelSheetName = string.Empty;
        string Id = string.Empty;
        int UserType;
        int intHSRPStateID;
        string strUserID = string.Empty;
        string HSRP_StateID = string.Empty, RTOLocationID = string.Empty;
        bool FlagIsDirty = false;
        string ExcelSheetName = string.Empty;

        string CnnStringupload = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
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

                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                //ComputerIP = Request.UserHostAddress;

                Id = Session["UID"].ToString();
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                if (!IsPostBack)
                {
                    try
                    {
                        if (UserType.Equals(0))
                        {

                            FilldropDownListOrganization();

                        }
                        else
                        {

                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;

                            FilldropDownListOrganization();

                           
                        }


                    }
                    catch (Exception err)
                    {

                    }
                }
            }
        }



        private void FilldropDownListOrganization()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }

        }


        protected void Button1_Click(object sender, EventArgs e)
        {

            try
            {
                DataTable dtExcelRecords = new DataTable();
                if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
                {
                    InsertDataInstage();
                }
                else
                {

                    llbMSGError.Text = "Please select a file to upload.";
                    llbMSGSuccess.Text = "";
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
                ExcelSheetName = FileUpload1.PostedFile.FileName + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString();
                string filename = ExcelSheetName + "-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                string fileExtension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                string fileLocation = System.Configuration.ConfigurationManager.AppSettings["HSRPExcel"].ToString();
                fileLocation += filename.Replace("\\\\", "\\");
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Please UpLoad Excel File. ";
                    return;
                }
                IExcelDataReader excelReader;
                DataTable dtExcelRecords = new DataTable();
                FileUpload1.PostedFile.SaveAs(fileLocation);


                FileStream stream = File.Open(fileLocation, FileMode.Open, FileAccess.Read);
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

                if (fileExtension != ".xls")
                {
                    llbMSGError.Text = "The Excel File must be in .xls Format..Kindly Convert Your  File into .xls format";

                    return;

                }

                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();
                excelReader.Close();


                ///>>>> Validation Check On All Excel Sheets
                if (result.Tables[0].Rows.Count > 0 || result != null)
                {
                    //ValidationCheckOnRecords(result.Tables[0]);
                    if (FlagIsDirty)
                    {
                        return;
                    }
                    InsertionOfRecords(result.Tables[0]);
                }
                else
                {
                    llbMSGError.Text = "No Data IN Excel File";
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
        DateTime dojdate;
        DateTime dojdate1;
        DateTime dojdate2;
        DateTime dojdate3;
       
        
        string ArrVehicle = string.Empty;
        private void InsertionOfRecords(DataTable dt)
        {
            Double CCAvenueRef;	
            string	ordertype= string.Empty;	
            string	OrderNo= string.Empty;	
            //string	OrderDatetime= string.Empty;	
            //string	ShipDateTime= string.Empty;
            string order_type = string.Empty;   
            string	PaymentMode= string.Empty;	
            string	CardType= string.Empty;	
            string	CardName= string.Empty;	
            string	Currency= string.Empty;
            double GrossAmount = 0.0;
            double Discount = 0.0;
            double OrderAmount=0.0;
            double FeeValue=0.0;
            double Applicabletax=0.0;
            double Share1=0.0;
            double Share2=0.0;
            string	FeeFlat= string.Empty;
            double Tax = 0.0;
            double TDS = 0.0;	
            string	BillName= string.Empty;	
            string	BillAddress= string.Empty;	
            string	BillCity= string.Empty;	
            string	BillState= string.Empty;	
            string	BillZip= string.Empty;	
            string	BillCountry= string.Empty;	
            string	BillTel= string.Empty;	
            string	BillEmail= string.Empty;	
            string	shipName= string.Empty;	
            string	shipAddress= string.Empty;	
            string	shipCity= string.Empty;	
            string	shipState= string.Empty;	
            string	shipZip= string.Empty;	
            string	shipCountry= string.Empty;	
            string	shipTel= string.Empty;	
            string	Instructions= string.Empty;	
            string	FraudStatus= string.Empty;	
            string	WebsiteURL= string.Empty;	
            string	OrderBankResponse= string.Empty;	
            string	OrderStatus= string.Empty;	
            string	SubAccId= string.Empty;	
            string	OrderBinCountry= string.Empty;	
            string	OrderStlmtDate= string.Empty;
            int stateid;	
           // string	cerationdatetime= string.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                
                double dResult;
                CCAvenueRef = Convert.ToDouble(dr["CCAvenue Ref#"]);
                ordertype = dr["order type"].ToString().Trim();
                OrderNo = dr["Order No"].ToString().Trim();

                if (double.TryParse(dr["Order Datetime"].ToString(), out dResult))
                {
                    dojdate = DateTime.FromOADate(dResult);
                    //OrderDatetime = dr["Order Datetime"].ToString().Trim();
                }

                if (double.TryParse(dr["Monthly"].ToString(), out dResult))
                {
                    dojdate1 = DateTime.FromOADate(dResult);
                    //OrderDatetime = dr["Order Datetime"].ToString().Trim();
                }

                if (double.TryParse(dr["Ship Date Time"].ToString(), out dResult))
                {
                    dojdate2 = DateTime.FromOADate(dResult);
                    //OrderDatetime = dr["Order Datetime"].ToString().Trim();
                }
                order_type = dr["order_type"].ToString().Trim();
                PaymentMode = dr["Payment Mode"].ToString().Trim();
                CardType = dr["Card Type"].ToString().Trim();
                CardName = dr["Card Name"].ToString().Trim();
                Currency = dr["Currency"].ToString().Trim();               
                GrossAmount = Convert.ToDouble(dr["Gross Amount"]);
                Discount = Convert.ToDouble(dr["Discount"]);
                OrderAmount = Convert.ToDouble(dr["Order Amount"]);
                FeeValue = Convert.ToDouble(dr["Fee Value@2%"]);
                Applicabletax = Convert.ToDouble(dr["Applicable tax %"]);
                Share1 = Convert.ToDouble(dr["Share 1"]);
                Share2 = Convert.ToDouble(dr["Share 2"]);
                FeeFlat = dr["Fee Flat"].ToString().Trim();
                Tax = Convert.ToDouble(dr["Tax"]);
                TDS = Convert.ToDouble(dr["TDS"]);
                BillName = dr["Bill Name"].ToString().Trim();
                BillAddress = dr["Bill Address"].ToString().Trim();
                BillCity = dr["Bill City"].ToString().Trim();
                BillState = dr["Bill State"].ToString().Trim();
                BillZip = dr["Bill Zip"].ToString().Trim();
                BillCountry = dr["Bill Country"].ToString().Trim();
                BillTel = dr["Bill Tel"].ToString().Trim();
                BillEmail = dr["Bill Email"].ToString().Trim();
                shipName = dr["ship Name"].ToString().Trim();
                shipAddress = dr["Ship Address"].ToString().Trim();
                shipCity = dr["Ship City"].ToString().Trim();
                shipState = dr["Ship State"].ToString().Trim();
                shipZip = dr["Ship Zip"].ToString().Trim();
                shipCountry = dr["Ship Country"].ToString().Trim();
                shipTel = dr["Ship Tel"].ToString().Trim();
                Instructions = dr["Instructions"].ToString().Trim();
                FraudStatus = dr["Fraud Status"].ToString().Trim();
                WebsiteURL = dr["Website URL"].ToString().Trim();
                OrderBankResponse = dr["Order Bank Response"].ToString().Trim();
                OrderStatus = dr["Order Status"].ToString().Trim();
                SubAccId = dr["Sub Acc Id"].ToString().Trim();
                OrderBinCountry = dr["Order Bin Country"].ToString().Trim();
                OrderStlmtDate = dr["Order Stlmt Date"].ToString().Trim();
                if (double.TryParse(dr["Order Stlmt Date"].ToString(), out dResult))
                {
                    dojdate3 = DateTime.FromOADate(dResult);
                    //OrderDatetime = dr["Order Datetime"].ToString().Trim();
                }
                stateid = Convert.ToInt32(dr["stateid"]);


                string strquery = "select count(*) from APOnlineTesting.dbo.CCACalculation where  [Order No]='" + OrderNo + "' ";
                int Iresult = Utils.getScalarCount(strquery, CnnStringupload);
                if (Iresult > 0)
                {
                    ArrVehicle = ArrVehicle + "|" + OrderNo;
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Duplicate Records Found.";
                    countDuplicate = countDuplicate + 1;
                    txtduploicateempid.Visible = true;
                    txtduploicateempid.Text = ArrVehicle;


                }
                else
                {
                    sb.Append("Insert into APOnlineTesting.dbo.CCACalculation ([CCAvenue Ref#],[order type],[Order No],[Order Datetime],[Monthly],[Ship Date Time],[order_type],[Payment Mode],[Card Type],[Card Name],[Currency],[Gross Amount],[Discount],[Order Amount],[Fee Value@2%],[Applicable tax %],[Share 1],[Share 2],[Fee Flat],[Tax],[TDS],[Bill Name],[Bill Address],[Bill City],[Bill State],[Bill Zip],[Bill Country],[Bill Tel],[Bill Email],[ship Name],[Ship Address],[Ship City],[Ship State],[Ship Zip],[Ship Country],[Ship Tel],[Instructions],[Fraud Status],[Website URL],[Order Bank Response],[Order Status],[Sub Acc Id],[Order Bin Country],[Order Stlmt Date],[stateid])values ('" + CCAvenueRef + "','" + ordertype + "','" + OrderNo + "','" + dojdate + "','" + dojdate1 + "','" + dojdate2 + "','" + order_type + "','" + PaymentMode + "','" + CardType + "','" + CardName + "','" + Currency + "','" + GrossAmount + "','" + Discount + "','" + OrderAmount + "','" + FeeValue + "','" + Applicabletax + "','" + Share1 + "','" + Share2 + "','" + FeeFlat + "','" + Tax + "','" + TDS + "','" + BillName + "','" + BillAddress + "','" + BillCity + "','" + BillState + "','" + BillZip + "','" + BillCountry + "','" + BillTel + "','" + BillEmail + "','" + shipName + "','" + shipAddress + "','" + shipCity + "','" + shipState + "','" + shipZip + "','" + shipCountry + "','" + shipTel + "','" + Instructions + "','" + FraudStatus + "','" + WebsiteURL + "','" + OrderBankResponse + "','" + OrderStatus + "','" + SubAccId + "','" + OrderBinCountry + "','" + dojdate3 + "','" + stateid + "')");
                    countupload = countupload + 1;                  
                }

            }

            int i = 10;
            if (sb.ToString() != "")
            {
                i = Utils.ExecNonQuery(sb.ToString(), CnnStringupload);
            }

            if (countupload == i)
            {
                //llbMSGSuccess.Visible = true;
                //llbMSGSuccess.Text = "Record Save Sucessfully.";
                lbltotaluploadrecords.Text = countupload.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
            }

            else
            {
                llbMSGSuccess.Visible = true;
               // llbMSGSuccess.Text = "Record  Not Save .";
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
                lbltotaluploadrecords.Text = "0";

            }

        }

        private void ValidationCheckOnRecords(DataTable ExcelSheet)
        {

            for (int i = 0; i < ExcelSheet.Rows.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["TRANSACTIONDATE"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>TRANSACTIONDATE<b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["REFNO"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>REFNO</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (ExcelSheet.Rows[i]["CHNO"].ToString().Trim().Replace(" ", "").Length > 10)
                {

                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>CHNO</b> Field Empty At Row : " + i;
                    //i = i + 2;
                    //llbMSGError.Text = "</b> Field more than 10 characters At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["HSRPNO"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>HSRPNO</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

            }


        }

                
    }
}