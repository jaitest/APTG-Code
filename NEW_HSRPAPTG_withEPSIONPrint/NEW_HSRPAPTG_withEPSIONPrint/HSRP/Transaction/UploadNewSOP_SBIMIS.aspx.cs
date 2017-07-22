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



namespace HSRP.Transaction
{
    public partial class UploadNewSOP_SBIMIS : System.Web.UI.Page
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
        string orderno = string.Empty;
        
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
                         SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='9' and ActiveStatus='Y' Order by HSRPStateName";
                         DataSet dts = Utils.getDataSet(SQLString, CnnString);
                         DropDownListStateName.DataSource = dts;
                         DropDownListStateName.DataBind();        
                      }
          
        }

           protected void btnuploadhrexceldata_Click(object sender, EventArgs e)
             {
                llbMSGError.Text = string.Empty;
                DataTable dtExcelRecords = new DataTable();
                if ((FileUpload2.PostedFile != null) && (FileUpload2.PostedFile.ContentLength > 0))
                {               
                    InsertDataInstage();
                }
                else
                {
                    llbMSGError.Text = "Please select a file to upload.";
                    llbMSGSuccess.Text = "";
                }
            }


         string fileLocation = string.Empty;

         private void InsertDataInstage()
         {
             try
             {
                 ExcelSheetName = FileUpload2.PostedFile.FileName + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString();
                 string filename = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                 string fileExtension = System.IO.Path.GetExtension(FileUpload2.PostedFile.FileName);
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
                 FileUpload2.PostedFile.SaveAs(fileLocation);


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
                    // ValidationCheckOnRecords(result.Tables[0]);
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

        int countDuplicate = 0, countupload = 0,errorinexcel=0;

        DateTime dojdate;
        int orderno1;
        private void InsertionOfRecords( DataTable dtExcelRecords)
        {
            string ArrVehicle = string.Empty;
            string TRANSACTIONDATE=string.Empty;
            string REFNO = string.Empty;
            string CHNO = string.Empty;
            string HSRPNO = string.Empty;
            int AMOUNT=0;
            string Userid = string.Empty;
            string CReatedate=string.Empty;
            string UPLOADFILENAME = string.Empty;
            string ISPAYMENTRECIEVED = string.Empty;
            int  strnocode = 0;
            string strno = "select distinct orderNo from apnewsop_sbimis order by Orderno desc";
            DataTable dtno = Utils.GetDataTable(strno, CnnString);
            
            if (dtno.Rows.Count<=0)
            {
                strnocode = 1;
            }              
            else
            {
                strnocode = Convert.ToInt32(dtno.Rows[0]["OrderNo"]);
                strnocode = strnocode+1;              
            }


            StringBuilder sb = new StringBuilder();
            Userid = Session["UID"].ToString();

            foreach (DataRow dr in dtExcelRecords.Rows)
            {
                double dResult;
                
                if (double.TryParse(dr["TRANSACTIONDATE"].ToString(), out dResult))
                {
                    dojdate = DateTime.FromOADate(dResult);
                }            
                REFNO = dr["REFNO"].ToString().Trim();            
                CHNO = dr["CHNO"].ToString().Trim();
                HSRPNO = dr["HSRPNO"].ToString().Trim();               
                AMOUNT = Convert.ToInt32(dr["AMOUNT"].ToString().Trim());
        
                string SQLString1 = "select count(*) from dbo.APNewSOP_SBIMIS where HSRPNO='" + HSRPNO.ToString() + "'";
                int Iresult = Utils.getScalarCount(SQLString1, CnnStringupload);
                
                if (Iresult > 0)
                {
                    ArrVehicle = ArrVehicle + "|" + HSRPNO.ToString();
                    llbMSGError.Visible = true;                   
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "Duplicate Records Found.";
                    countDuplicate = countDuplicate + 1;
                    txtduploicateempid.Visible = true; 
                    txtduploicateempid.Text = ArrVehicle;
                }
                else
                {
                    sb.Append("Insert into APNewSOP_SBIMIS([TRANSACTIONDATE],[REFNO],[CHNO],[HSRPNO],[AMOUNT],[USERID],[UPLOADFILENAME],[OrderNo]) values ('" + dojdate + "','" + REFNO + "','" + CHNO + "','" + HSRPNO + "','" + AMOUNT + "','" + Userid + "','" + ExcelSheetName + "','" + strnocode + "')");                  
                    countupload = countupload + 1;                   
                }
               
         }

            int i = 0;
            if (sb.ToString() != "")
            {
                i = Utils.ExecNonQuery(sb.ToString(), CnnStringupload);
            }

            if (countupload == i)
            {
                llbMSGSuccess.Visible = true;
                llbMSGSuccess.Text = "Record save successfully";
                lbltotaluploadrecords.Text = countupload.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
            }
          
        }
        int j;
        private void ValidationCheckOnRecords(DataTable ExcelSheet)
        {
            for (int i = 1; i < ExcelSheet.Rows.Count; i++)
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


                if (string.IsNullOrEmpty(ExcelSheet.Rows[i]["AMOUNT"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>AMOUNT</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

            }

           
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FilldropDownListClient();
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
        }

       
  
    }
    }
