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
    public partial class UploadAllEmployeeSalarySheet : System.Web.UI.Page
    {
        string SaveLocation = string.Empty;
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        string getExcelSheetName = string.Empty;
        bool FlagIsDirty = false;      
        int UserType;
        string strUserID = string.Empty;
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
                if (!IsPostBack)
                {                
                    try
                    {
                      FilldropDownListCompany();
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }


        public void FilldropDownListCompany()
        {
             CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            SQLString = "select hsrp_stateid,   HSRPStateName from hsrpstate where  ActiveStatus='Y' Order by HSRPStateName";
            Utils.PopulateDropDownList(DDLState, SQLString.ToString(), CnnString, "--Select State Name--");
           
        


        }       

        protected void Button1_Click(object sender, EventArgs e)
        {
           
            llbMSGError.Text = "";
            llbMSGSuccess.Text = "";

            if (DDLState.SelectedItem.Text.ToString().Equals("--Select State Name--"))
            {
                llbMSGError.Text = "Please Select State Name..";
                return;
            }

            if (DDlCompany_Name.SelectedItem.Text.ToString().Equals("--Select Company Name--"))
            {

                llbMSGError.Text = "Please  Company Name..";
                return;
            }

            if (DDLMonth.SelectedItem.Text.ToString().Equals("--Select Month--"))
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "";
                llbMSGError.Text = "Please Select  Month Name...";
                return;
            }

             if (ddlyear.SelectedItem.Text.ToString().Equals("--Select Year--"))
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "";
                llbMSGError.Text = "Please Select  Year";
                return;
            }
           
           
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
                ExcelSheetName = FileUpload1.PostedFile.FileName;
                string filename = "SalarySheet-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                string fileExtension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                string fileLocation = System.Configuration.ConfigurationManager.AppSettings["DealerFolder"].ToString();
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
               
                if (fileExtension == ".xlsx")
                {
                    llbMSGError.Text = "The Excel File must be in .xls Format..Kindly Convert Your .xlsx File into .xls format";

                    excelReader.Close();
                    return;
                  
                }

                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();                
                excelReader.Close();
               if (result.Tables[0].Rows.Count > 0 || result != null)
                {
                    ValidationCheckOnRecords(result.Tables[0]);
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
        double dResult;
        DateTime exelMonthyear;
       
        private void InsertionOfRecords(DataTable dt)
        {

    string EmpCode = string.Empty; 
    string EmployeeNameinBank = string.Empty;   
    long Net_Salary = 0;
	string Pay_Mode = string.Empty;
    string ArialSalary = string.Empty;
    string Status = string.Empty;
    string Remarks = string.Empty;

    string ExcelSheetMonth = string.Empty;
    string ExcelOrderNo = string.Empty;

  //Status,ExcelOrderNo, SalryArial, Remarks

	int UploadedBy  =0;
    string CompanyName= string.Empty;
    int  Month_Name  =0;
	int Year =0;
    string ACountNo = string.Empty;
    int Hsrp_stateid = 0;
    

            string ArrVehicle = string.Empty;
            StringBuilder sb = new StringBuilder();
           // DateTime result;


            ExcelOrderNo = "Sal" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString();
            foreach (DataRow dr in dt.Rows)
            {	DataTable dt1; 	
                EmpCode= dr["EmpCode"].ToString().Trim();
                EmployeeNameinBank = dr["Name"].ToString().Trim();
                Net_Salary = Convert.ToInt64(dr["NetSalary"].ToString().Trim());
                Pay_Mode = dr["PayMode"].ToString().Trim();
                ArialSalary = dr["ArialSalary"].ToString().Trim();
                Status = dr["Status"].ToString().Trim();
                Remarks = dr["Remarks"].ToString().Trim();
                ExcelSheetMonth = dr["Month"].ToString().Trim();

                if ((double.TryParse(ExcelSheetMonth, out dResult)))
                {
                     exelMonthyear = DateTime.FromOADate(dResult);
                }



                UploadedBy = Convert.ToInt32(strUserID.ToString().Trim());
                CompanyName = DDlCompany_Name.SelectedValue.ToString();
                Hsrp_stateid = Convert.ToInt32(DDLState.SelectedValue.ToString());

                string query = " Select  BankAccountNo from employeemaster where emp_id = '" + EmpCode + "' and hsrp_stateid  = "+DDLState.SelectedValue.ToString()+"";
                 dt1= Utils.GetDataTable(query, CnnStringupload);
                 if ( dt1.Rows.Count >0 )
                 {                   
                     ACountNo = dt1.Rows[0]["BankAccountNo"].ToString();
                   }
                              

                 Month_Name =Convert.ToInt32( DDLMonth.SelectedValue.ToString());
                 Year = Convert.ToInt32(ddlyear.SelectedItem.Text.ToString());
              
                string strquery = "select count(*) from salarysheet where  LTRIM(RTRIM(Empcode))= LTRIM(RTRIM('" + EmpCode + "'))and LTRIM(RTRIM(Month_Name))= LTRIM(RTRIM(" + Convert.ToInt32(DDLMonth.SelectedValue.ToString()) + ")) and LTRIM(RTRIM(Year))= LTRIM(RTRIM(" + Convert.ToInt32(ddlyear.SelectedValue.ToString()) + ")) and LTRIM(RTRIM(CompanyName))= LTRIM(RTRIM('" + DDlCompany_Name.SelectedValue.ToString() + "')) and Hsrp_stateid = " + DDLState.SelectedValue.ToString() + " ";
                 int Iresult = Utils.getScalarCount(strquery, CnnStringupload);
                if (Iresult > 0)
                {
                    ArrVehicle = ArrVehicle + "|" + EmpCode;
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Duplicate Records Found.";                   
                    countDuplicate = countDuplicate + 1;
                    txtduploicateempid.Visible = true;                   
                    txtduploicateempid.Text = ArrVehicle;                    
                   
                }
                else
                {
                    sb.Append("Insert into dbo.SalarySheet (EmpCode,Name_INBank ,AccountNo ,Net_Salary,Pay_Mode,UploadedDatatime,UploadedBy,CompanyName,Month_Name ,Year,ExcelSheetName, Hsrp_stateid , Status,ExcelOrderNo, SalryArial, Remarks,ExcelSheetMonth) values('" + EmpCode + "','" + EmployeeNameinBank + "','" + ACountNo + "'," + Net_Salary + ",'" + Pay_Mode + "', getdate()," + UploadedBy + ",'" + CompanyName + "','" + Month_Name + "'," + Year + " ,'" + ExcelSheetName + "', " + DDLState.SelectedValue.ToString() + " , '" + Status + "', '" + ExcelOrderNo + "',  '" + ArialSalary + "', '" + Remarks + "' ,'" + exelMonthyear + "');"); 
                   countupload = countupload + 1;
                  
               }
                
            }

            int i = 9000;
            if (sb.ToString() != "")
            {
              i= Utils.ExecNonQuery(sb.ToString(), CnnStringupload);
            }

            if (countupload==i)
            {
                llbMSGSuccess.Visible = true;
                llbMSGSuccess.Text = "Record Save Sucessfully.";
                lbltotaluploadrecords.Text = countupload.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
            }

            else
            {
                llbMSGSuccess.Visible = true;
                llbMSGSuccess.Text = "Record  Not Save .";
                lbltotladuplicaterecords.Text = countDuplicate.ToString();

            }
            

        }               

        private void ValidationCheckOnRecords(DataTable ExcelSheet)
        {
            try
            {
           
            for (int i = 0; i < ExcelSheet.Rows.Count; i++)
            {
                string s = ExcelSheet.Rows[i]["EmpCode"].ToString().Trim().ToUpper();

               if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["EmpCode"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>EmpCode</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                DataTable dt1 = new DataTable();
                string query = " Select   hsrp_stateid , BankAccountNo from employeemaster where emp_id = '" + ExcelSheet.Rows[i]["EmpCode"].ToString().Trim() + "' and hsrp_stateid= " + DDLState.SelectedValue.ToString() + "";
                dt1= Utils.GetDataTable(query, CnnStringupload);
                if ( dt1.Rows.Count ==0 )
                 {               
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>EmpCode =" + ExcelSheet.Rows[i]["EmpCode"].ToString().Trim() + " Not Available in Employee Records. </b> Field At Row : " + i;
                    FlagIsDirty = true;
                    return;
                 }
               
                              
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Name"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Name</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;

                }               
             	
               
                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["NetSalary"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>NetSalary</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }


                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["PayMode"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>PayMode</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }

                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ArialSalary"].ToString().Trim()))
                 {
                     i = i + 2;
                     llbMSGError.Text = "Excel Sheet : Has <b>ArialSalary</b> Field Empty At Row : " + i;
                     FlagIsDirty = true;
                     return;
                 }

                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Remarks"].ToString().Trim()))
                 {
                     i = i + 2;
                     llbMSGError.Text = "Excel Sheet : Has <b>Remarks</b> Field Empty At Row : " + i;
                     FlagIsDirty = true;
                     return;

                 }

                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Status"].ToString().Trim()))
                 {
                     i = i + 2;
                     llbMSGError.Text = "Excel Sheet : Has <b>Status</b> Field Empty At Row : " + i;
                     FlagIsDirty = true;
                     return;

                 }
                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Month"].ToString().Trim()))
                 {
                     i = i + 2;
                     llbMSGError.Text = "Excel Sheet : Has <b>Month</b> Field Empty At Row : " + i;
                     FlagIsDirty = true;
                     return;

                 }          



            }
            for (int i = 0; i < ExcelSheet.Rows.Count; i++)
            {
              
            
                for (int j = 0; j < ExcelSheet.Rows.Count; j++)
                {
                   
                    if (i != j)
                    {
                        if (ExcelSheet.Rows[i]["EmpCode"].ToString().Trim().ToUpper() == ExcelSheet.Rows[j]["EmpCode"].ToString().Trim().ToUpper())
                        {
                            i = i + 1;
                            llbMSGError.Text = "Excel Sheet : Has <b>EmpCode</b> Field Duplicte At Row : " + i;
                            FlagIsDirty = true;
                            return;

                        }
                    }
                   
   
                }                

            }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        protected void DDLState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLState.SelectedItem.Text.ToString() != "--Select State Name--")
            {
                lblCompanyName.Visible = true;
                DDlCompany_Name.Visible = true;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                SQLString = " select  CompanyName from hsrpstate where  hsrp_stateid =" + Convert.ToInt32(DDLState.SelectedValue.ToString()) + " and ActiveStatus='Y' ";
                Utils.PopulateDropDownList(DDlCompany_Name, SQLString.ToString(), CnnString, "--Select Company Name--");
     
            }           

        }        

        
       
       
    }
}