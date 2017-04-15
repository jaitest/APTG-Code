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
    public partial class UploadEmployeeDetail : System.Web.UI.Page
    {
        string SaveLocation = string.Empty;
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        string getExcelSheetName = string.Empty;
        bool FlagIsDirty = false;
        int UserType;
        string strUserID = string.Empty;
        string ExcelSheetName = string.Empty;
        string fileLocation = string.Empty;
                
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
                        FilldropDownListState();
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        public void FilldropDownListState()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            SQLString = "select hsrp_stateid,   HSRPStateName from hsrpstate where ActiveStatus='Y' Order by HSRPStateName";
            Utils.PopulateDropDownList(DDLState, SQLString.ToString(), CnnString, "--Select State Name--");




        }       

        protected void Button1_Click(object sender, EventArgs e)
        {

            llbMSGError.Text = "";
            llbMSGSuccess.Text = "";

            if (DDLState.SelectedItem.Text.ToString().Equals("--Select State Name--"))
            {
                llbMSGError.Text = "Please Select  Company Name..";
                return;
            }

            if (DDlCompany_Name.SelectedItem.Text.ToString().Equals("--Select Company Name--"))
            {
                llbMSGError.Text = "Please  Select Company Name..";
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
        DateTime dojdate;
        private void InsertionOfRecords(DataTable dt)
        {
            string EmployeeNameInBank = string.Empty;
            string FatherName = string.Empty;
            string EC_AC = string.Empty;
            string EmpCode = string.Empty;
            string ESICNo = string.Empty;
            string EPFNo = string.Empty;
            string UANNo = string.Empty;
            string BankName = string.Empty;
            string ACountNo = string.Empty;
            string BranchName = string.Empty;
            string IFSCode = string.Empty;
            string Name = string.Empty;
            string DOJ = string.Empty;
            string Department = string.Empty;
            string Designation = string.Empty;
            string State = string.Empty;
            string Location = string.Empty;
            int state_id = 0;
             string district= string.Empty;
          
            int UploadedBy = 0;
            string CompanyName = string.Empty;
            

            string ArrVehicle = string.Empty;
            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {

              
                EmpCode = dr["EmpCode"].ToString().Trim();
                Name = dr["EmployeeName"].ToString().Trim();
                EmployeeNameInBank = dr["EmployeeNameInBank"].ToString().Trim();
                FatherName = dr["FatherName"].ToString().Trim();
                EC_AC = dr["EC/AC"].ToString().Trim();
                Location = dr["Location"].ToString().Trim();
                State = dr["State"].ToString().Trim();
                Designation = dr["Designation"].ToString().Trim();
                EPFNo = dr["EPFNo"].ToString().Trim();               

                ESICNo = dr["ESICNo"].ToString().Trim();
                IFSCode = dr["IFSCCode"].ToString().Trim();
                 ACountNo = dr["ACcountNo"].ToString().Trim();            
      
                BankName = dr["BankName"].ToString().Trim();
               
                BranchName = dr["BranchName"].ToString().Trim();
                district= dr["District"].ToString().Trim();
               
              
               
                    state_id = Convert.ToInt32(DDLState.SelectedValue.ToString());
               
               
                UploadedBy = Convert.ToInt32(strUserID.ToString().Trim());
                CompanyName = DDlCompany_Name.SelectedValue.ToString();


                string strquery = "select count(*) from employeemaster where  LTRIM(RTRIM(Emp_id))= LTRIM(RTRIM('" + EmpCode + "')) ";
                int Iresult = Utils.getScalarCount(strquery, CnnStringupload);
                if (Iresult > 0)
                {
                    ArrVehicle = ArrVehicle + "|" + EmpCode;


                    sb.Append(" Update employeemaster set EmployeeNameInBank='" + EmployeeNameInBank + "'  , FatherName='" + FatherName + "', EC_AC  = '" + EC_AC + "', ESINumber = '" + ESICNo + "', 	PFNumber =  '" + EPFNo + "',BankName  =  '" + BankName + "', BankAccountNo = '" + ACountNo + "',  BranchName ='" + BranchName + "' , IFSCCode ='" + IFSCode + "',	Emp_Name= '" + Name + "', 	 Designation = '" + Designation + "', Location = '" + Location + "',CompanyName = '" + CompanyName + "', EntryDate = getdate(), UploadedBy= " + UploadedBy + " ,  ActiveStatus = 'Y' , hsrp_stateid= " + state_id + " , district='"+district+"' where Emp_id = '" + EmpCode + "';");              
                    countDuplicate = countDuplicate + 1;
                    txtduploicateempid.Visible = true;
                    txtduploicateempid.Text = ArrVehicle;

                }
                else
                {
                    sb.Append("Insert into dbo.employeemaster (Emp_id, ESINumber,	PFNumber ,	BankName ,BankAccountNo  , BranchName  , IFSCCode ,Emp_Name ,	Designation ,Location  ,CompanyName, EntryDate, UploadedBy ,ActiveStatus,hsrp_stateid, EmployeeNameInBank  , FatherName, EC_AC , district  ) values('" + EmpCode + "','" + ESICNo + "','" + EPFNo + "','" + BankName + "','" + ACountNo + "','" + BranchName + "','" + IFSCode + "','" + Name + "','" + Designation + "','" + Location + "','" + CompanyName + "',getdate(),  " + UploadedBy + " ,  'Y', " + state_id + " ,  '" + EmployeeNameInBank + "' ,'" + FatherName + "', '" + EC_AC + "' , '" + district + "');");
                    countupload = countupload + 1;
                  
                }

            }

            int i = 90000;
            if (sb.ToString() != "")
            {
                i = Utils.ExecNonQuery(sb.ToString(), CnnStringupload);
            }

            if (countupload == i)
            {
               
                lbltotaluploadrecords.Text = countupload.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
            }

            else
            {
                llbMSGSuccess.Visible = true;
             
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
                lbltotaluploadrecords.Text = "0";

            }


        }

        private void ValidationCheckOnRecords(DataTable ExcelSheet)
        {
            try
            {

               
			

                for (int i = 0; i < ExcelSheet.Rows.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["EmpCode"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>EmpCode</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;
                    }
                       if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["EmployeeName"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>EmployeeName</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;


                    }


                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["EmployeeNameInBank"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>EmployeeNameInBank</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;


                    }

                     if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["FatherName"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>FatherName</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;


                    }
                     if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["EC/AC"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>EC/AC</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;


                    }
                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ESICNo"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>ESICNo</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;
                    }
                    	
                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["EPFNo"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>EPFNo</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;
                    }

                  

                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["BankName"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>BankName</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ACcountNo"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>ACcountNo</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["BranchName"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>BranchName</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;

                    }

                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["IFSCCode"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>IFSC Code</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;
                    }
                

                 

                    

                   
                  

                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Designation"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>Designation</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["State"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>State</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;

                    }                   

                    
                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Location"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>Location</b> Field Empty At Row : " + i;
                        FlagIsDirty = true;
                        return;


                    }

                    if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["District"].ToString().Trim()))
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>District</b> Field Empty At Row : " + i;
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
                                i = i + 2;
                                llbMSGError.Text = "Excel Sheet : Has <b>EmpCode</b> Field Duplicte At Row : " + i;
                                FlagIsDirty = true;
                                return;

                            }

                            if (ExcelSheet.Rows[i]["ACcountNo"].ToString().Trim().ToUpper() !="0")
                            { 
                            if (ExcelSheet.Rows[i]["ACcountNo"].ToString().Trim().ToUpper() == ExcelSheet.Rows[j]["ACcountNo"].ToString().Trim().ToUpper())
                            {
                                i = i + 2;
                                llbMSGError.Text = "Excel Sheet : Has <b>ACcountNo</b> Field Duplicte At Row : " + i;
                                FlagIsDirty = true;
                                return;

                            }
                            }

                            if (ExcelSheet.Rows[i]["EPFNo"].ToString().Trim().ToUpper() !="NULL")
                            {
                            if (ExcelSheet.Rows[i]["EPFNo"].ToString().Trim().ToUpper() == ExcelSheet.Rows[j]["EPFNo"].ToString().Trim().ToUpper())
                            {
                                i = i + 2;
                                llbMSGError.Text = "Excel Sheet : Has <b>EPFNo</b> Field Duplicte At Row : " + i;
                                FlagIsDirty = true;
                                return;

                            }
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