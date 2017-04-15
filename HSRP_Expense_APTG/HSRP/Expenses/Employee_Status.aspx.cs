using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataProvider;
using System.Drawing;
using CarlosAg.ExcelXmlWriter;
using System.Text;


namespace HSRP.Expenses
{
    public partial class Employee_Status : System.Web.UI.Page
    {       
        string UserID = string.Empty;        
        Utils objutil = new Utils();
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;
        string OrderStatus = string.Empty;
             
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

       // string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                if (!IsPostBack)
                {

                    // InitialSetting();
                    try
                    {
                        // InitialSetting();
                        if (UserType.Equals(0))
                        {                            
                            FilldropDownListOrganization();
                            FilldropDowndistrictcenter();
                            //FilldropDownListClient();                           
                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDowndistrictcenter();
                           // FilldropDownListClient();
                            //BindGrid();
                         
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            
        }

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }

       

        private void FilldropDowndistrictcenter()
        {
            if (UserType.Equals(0))
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                string SQLString11 = "select distinct district,district from rtolocation where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' and EmbCenterName is not null group by district,district Order by 1";
                //string SQLString11 = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName asc";
                Utils.PopulateDropDownList(ddldistrictname, SQLString11.ToString(), CnnString, "--Select District Name--");
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                //string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM Employeemaster e INNER JOIN RTOLocation a ON e.RTOLocationID = a.RTOLocationID where a.Hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and EmbCenterName='" + ddldistrictname.SelectedValue.ToString() + "' and e.Activestatus='Y'  Order by RTOLocationName asc ";
                string SQLString11 = "select distinct district ,district from rtolocation where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N' and EmbCenterName is not null group by district,district Order by 1";
                System.Data.DataTable dt1 = Utils.GetDataTable(SQLString11, CnnString);
                ddldistrictname.DataSource = dt1;
                ddldistrictname.DataBind();
                ddldistrictname.Items.Insert(0, new ListItem("--Select District Name--"));
            }
        } 



      

       

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FilldropDownListClient();
        }

        protected void ddllocation_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

      

        protected void ddldistrictname_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    FilldropDownListClient();
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        public void BindActivestatus_Y()
        {
            string strquery = "SELECT row_number() over (order by Emp_ID) as 'S No',Emp_ID as 'EmployeeID',Emp_Name as 'EmployeeName',r.RtolocationName,e.Designation,e.MobileNo,Convert(varchar(12),EmpJoiningDate,103) as 'JoiningDate' , e.Department,CompanyName,e.Activestatus FROM EmployeeMaster E, rtolocation r  where E.rtolocationid=r.rtolocationid and e.activestatus='Y' and e.Hsrp_stateID='" + HSRPStateID + "' and r.embcentername='" + ddldistrictname.SelectedItem.ToString() + "' and e.rtolocationid is not null order by 1";            
            
            DataTable dt = Utils.GetDataTable(strquery, CnnString);        
            if (dt.Rows.Count > 0)
            {
                lblErrMess.Text = "";
                
                gvid.Visible = true;
                gvid.DataSource = dt;
                gvid.DataBind();
            }
           
        }

        public void BindActivestatus_N()
        {
            //string strquery = "select * from Employeemaster where hsrp_stateid='" + HSRPStateID + "' and activestatus='N'";
            //string strquery = "SELECT Hsrp_stateid,RtoLocationID,Emp_ID as 'EmployeeID',Emp_Name as 'EmployeeName',Designation,MobileNo,Convert(varchar(12),EmpJoiningDate,103) as 'JoiningDate' , Department,CompanyName,Activestatus FROM EmployeeMaster where activestatus='N' and Hsrp_stateID='" + HSRPStateID + "' order by Emp_name asc";
            string strquery = "SELECT row_number() over (order by Emp_ID) as 'S No',Emp_ID as 'EmployeeID',Emp_Name as 'EmployeeName',r.RtolocationName,e.Designation,e.MobileNo,Convert(varchar(12),EmpJoiningDate,103) as 'JoiningDate' , e.Department,CompanyName,e.Activestatus FROM EmployeeMaster E, rtolocation r  where E.rtolocationid=r.rtolocationid and e.activestatus='N' and e.Hsrp_stateID='" + HSRPStateID + "' and r.embcentername='" + ddldistrictname.SelectedItem.ToString() + "' and e.rtolocationid is not null order by 1";
            DataTable dt = Utils.GetDataTable(strquery, CnnString);
            //  DataSet ds = Utils.getDataSet("EmployeeMasterStatus_Details '" + HSRPStateID + "','" + ddllocation.SelectedValue.ToString() + "','0','0','0','0','0','0','" + ddlstatus.SelectedValue.ToString() + "','SELECT'", CnnString);
            if (dt.Rows.Count > 0)
            {
                lblErrMess.Text = "";               
                gvid.Visible = true;
                gvid.DataSource = dt;
                gvid.DataBind();
            }
            else
            {
                gvid.Visible = false;
                lblErrMess.Text = "Record Not Found";

            }

        }

        protected void btnstatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddldistrictname.SelectedItem.ToString() == "--Select District Name--")
                {
                    lblErrMess.Text = "Please Select District Name";
                    return;
                }

                if (ddlstatus.SelectedItem.ToString() == "--Select Status Type--")
                {
                    lblErrMess.Text = "Please Select Status";
                    return;
                }
               
                if (ddlstatus.SelectedValue.ToString() == "Active")
                {
                    BindActivestatus_Y();
                }
                else if(ddlstatus.SelectedValue.ToString() == "InActive")
                {
                    BindActivestatus_N();
                }
                 
               else  if (ddlstatus.SelectedItem.ToString() == "All")
                {
                    StringBuilder strSQL = new StringBuilder();
                    strSQL.Append("SELECT row_number() over (order by Emp_ID) as 'S No',Emp_ID as 'EmployeeID',Emp_Name as 'EmployeeName',r.RtolocationName,e.Designation,e.MobileNo,Convert(varchar(12),EmpJoiningDate,103) as 'JoiningDate' , e.Department,CompanyName,e.Activestatus FROM EmployeeMaster E, rtolocation r  where E.rtolocationid=r.rtolocationid and e.activestatus in ('Y','N') and e.Hsrp_stateID='" + HSRPStateID + "'  and e.rtolocationid is not null order by 1");
                    DataTable dt = Utils.GetDataTable(strSQL.ToString(), CnnString);
                    if (dt.Rows.Count > 0)
                    {
                        lblErrMess.Text = "";
                        gvid.Visible = true;
                        gvid.DataSource = dt;
                        gvid.DataBind();
                    }
                    else
                    {
                        gvid.Visible = false;
                        lblErrMess.Text = "Record Not Found";

                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SaveAndDownloadFile()
        {
             StringBuilder strSQL = new StringBuilder();
            if (ddlstatus.SelectedItem.ToString()=="Active")
                {                    
                   strSQL.Append("SELECT row_number() over (order by Emp_ID) as 'S No',Emp_ID as 'EmployeeID',Emp_Name as 'EmployeeName',r.RtolocationName,e.Designation,e.MobileNo,Convert(varchar(12),EmpJoiningDate,103) as 'JoiningDate' , e.Department,CompanyName,e.Activestatus FROM EmployeeMaster E, rtolocation r  where E.rtolocationid=r.rtolocationid and e.activestatus='Y' and e.Hsrp_stateID='" + HSRPStateID + "' and r.embcentername='" + ddldistrictname.SelectedItem.ToString() + "' and e.rtolocationid is not null order by 1");
                   
                }
             if (ddlstatus.SelectedItem.ToString()=="InActive")
                {                    
                   strSQL.Append("SELECT row_number() over (order by Emp_ID) as 'S No',Emp_ID as 'EmployeeID',Emp_Name as 'EmployeeName',r.RtolocationName,e.Designation,e.MobileNo,Convert(varchar(12),EmpJoiningDate,103) as 'JoiningDate' , e.Department,CompanyName,e.Activestatus FROM EmployeeMaster E, rtolocation r  where E.rtolocationid=r.rtolocationid and e.activestatus='N' and e.Hsrp_stateID='" + HSRPStateID + "' and r.embcentername='" + ddldistrictname.SelectedItem.ToString() + "' and e.rtolocationid is not null order by 1");
                   
                }

            string strVehicleType = string.Empty;

            DataTable dt = Utils.GetDataTable(strSQL.ToString(), CnnString);
            if (dt.Rows.Count > 0)
            {
                string filename = "Export_Data" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Export Data";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                style8.Font.FontName = "Tahoma";
                style8.Font.Size = 10;
                style8.Font.Bold = true;
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style8.Interior.Color = "#D4CDCD";
                style8.Interior.Pattern = StyleInteriorPattern.Solid;

                WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                style5.Font.FontName = "Tahoma";
                style5.Font.Size = 10;
                style5.Font.Bold = false;
                style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
                style2.Font.FontName = "Tahoma";
                style2.Font.Size = 10;
                style2.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
                style3.Font.FontName = "Tahoma";
                style3.Font.Size = 12;
                style3.Font.Bold = true;
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                Worksheet sheet = book.Worksheets.Add("Employee Status Report");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(205));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(130));

                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(112));
                sheet.Table.Columns.Add(new WorksheetColumn(109));
                sheet.Table.Columns.Add(new WorksheetColumn(105));
                sheet.Table.Columns.Add(new WorksheetColumn(160));


                WorksheetRow row = sheet.Table.Rows.Add();

                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Employee Status Report");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row.Index = 4;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                // row.Index = 5;

                //DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell(OrderDatefrom.SelectedDate.ToString("dd/MM/yyyy"), "HeaderStyle2"));
                ////row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row = sheet.Table.Rows.Add();
                row.Index = 6;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "HeaderStyle2"));
                // row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();



                row.Index = 7;
                row = sheet.Table.Rows.Add();
                row.Cells.Add(new WorksheetCell("S.No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("EmpID", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Emp Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Location Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Designation", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Mobile No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Joining Date ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Department", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Company Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Activestatus", "HeaderStyle"));
            
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                //row.Index = 9;


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {

                        //sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                      
                        row.Cells.Add(new WorksheetCell(dtrows["S No"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["EmployeeID"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["EmployeeName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RtolocationName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Designation"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["JoiningDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Department"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["CompanyName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Activestatus"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["NewPdfRunningNo"].ToString(), DataType.String, "HeaderStyle1"));
                       
                    }


                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);

                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();
                }
            }

        }

       

        private static void AddNewCell(WorksheetRow row, string strText, string strStyle, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                row.Cells.Add(new WorksheetCell(strText, strStyle));
        }

        private static void AddColumnToSheet(Worksheet sheet, int iWidth, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                sheet.Table.Columns.Add(new WorksheetColumn(iWidth));
        }

        protected void btnexceltoexport_Click(object sender, EventArgs e)
        {
            SaveAndDownloadFile();
        }

        private void SaveAndDownloadAll()
        {
            StringBuilder strSQL = new StringBuilder();
            if (ddlstatus.SelectedItem.ToString() == "All")
            {
                strSQL.Append("SELECT row_number() over (order by Emp_ID) as 'S No',Emp_ID as 'EmployeeID',Emp_Name as 'EmployeeName',r.RtolocationName,e.Designation,e.MobileNo,Convert(varchar(12),EmpJoiningDate,103) as 'JoiningDate' , e.Department,CompanyName,e.Activestatus FROM EmployeeMaster E, rtolocation r  where E.rtolocationid=r.rtolocationid and e.activestatus in ('Y','N') and e.Hsrp_stateID='" + HSRPStateID + "'  and e.rtolocationid is not null order by 1");

            }
            //if (ddlstatus.SelectedItem.ToString() == "InActive")
            //{
            //    strSQL.Append("SELECT row_number() over (order by Emp_ID) as 'S No',Emp_ID as 'EmployeeID',Emp_Name as 'EmployeeName',r.RtolocationName,e.Designation,e.MobileNo,Convert(varchar(12),EmpJoiningDate,103) as 'JoiningDate' , e.Department,CompanyName,e.Activestatus FROM EmployeeMaster E, rtolocation r  where E.rtolocationid=r.rtolocationid and e.activestatus='N' and e.Hsrp_stateID='" + HSRPStateID + "' and e.rtolocationid is not null order by 1");

            //}

            string strVehicleType = string.Empty;

            DataTable dt = Utils.GetDataTable(strSQL.ToString(), CnnString);
            if (dt.Rows.Count > 0)
            {
                string filename = "Export_Data" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Export Data";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                style8.Font.FontName = "Tahoma";
                style8.Font.Size = 10;
                style8.Font.Bold = true;
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style8.Interior.Color = "#D4CDCD";
                style8.Interior.Pattern = StyleInteriorPattern.Solid;

                WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                style5.Font.FontName = "Tahoma";
                style5.Font.Size = 10;
                style5.Font.Bold = false;
                style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
                style2.Font.FontName = "Tahoma";
                style2.Font.Size = 10;
                style2.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
                style3.Font.FontName = "Tahoma";
                style3.Font.Size = 12;
                style3.Font.Bold = true;
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                Worksheet sheet = book.Worksheets.Add("Employee Status Report");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(205));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(130));

                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(112));
                sheet.Table.Columns.Add(new WorksheetColumn(109));
                sheet.Table.Columns.Add(new WorksheetColumn(105));
                sheet.Table.Columns.Add(new WorksheetColumn(160));


                WorksheetRow row = sheet.Table.Rows.Add();

                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Employee Status Report");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row.Index = 4;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                // row.Index = 5;

                //DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell(OrderDatefrom.SelectedDate.ToString("dd/MM/yyyy"), "HeaderStyle2"));
                ////row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row = sheet.Table.Rows.Add();
                row.Index = 6;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "HeaderStyle2"));
                // row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();



                row.Index = 7;
                row = sheet.Table.Rows.Add();
                row.Cells.Add(new WorksheetCell("S.No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("EmpID", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Emp Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Location Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Designation", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Mobile No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Joining Date ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Department", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Company Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Activestatus", "HeaderStyle"));

                String StringField = String.Empty;
                String StringAlert = String.Empty;

                //row.Index = 9;


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {

                        //sno = sno + 1;
                        row = sheet.Table.Rows.Add();

                        row.Cells.Add(new WorksheetCell(dtrows["S No"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["EmployeeID"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["EmployeeName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RtolocationName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Designation"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["JoiningDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Department"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["CompanyName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Activestatus"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["NewPdfRunningNo"].ToString(), DataType.String, "HeaderStyle1"));

                    }


                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);

                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();
                }
            }

        }

        protected void btnallstatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddldistrictname.SelectedItem.ToString() == "--Select District Name--")
                {
                    lblErrMess.Text = "Please Select District Name";
                    return;
                }

                if (ddlstatus.SelectedItem.ToString() == "--Select Status Type--")
                {
                    lblErrMess.Text = "Please Select Status";
                    return;
                }

                if (ddlstatus.SelectedItem.ToString() == "Active" || ddlstatus.SelectedItem.ToString() == "InActive")
                {
                    lblErrMess.Text = "Please Select All Type";
                    return;
                }

                SaveAndDownloadAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }


       
        
    }
}