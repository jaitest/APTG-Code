using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataProvider;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;

namespace HSRP.Expenses
{
    public partial class AttendanceReport : System.Web.UI.Page
    {
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
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        SqlConnection con;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

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
                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
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
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            FilldropDowndistrictcenter();
                            //FillUsername();
                           
                      
                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            FilldropDowndistrictcenter();
                           // FillUsername();
                          
                        
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        //private void InitialSetting()
        //{

        //    string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        //    string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        //    HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
        //    CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    OrderDate.MaxDate = DateTime.Parse(MaxDate);
        //    CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        //}
        #region DropDown

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

        private void FilldropDownListClient()
        {
            if (UserType.Equals(0))
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                string SQLString11 = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and District='" + ddldistrictname.SelectedValue.ToString() + "' and ActiveStatus!='N'   Order by RTOLocationName asc";
                Utils.PopulateDropDownList(ddllocation, SQLString11.ToString(), CnnString, "--Select RTO Name--");
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                //string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
                string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM Employeemaster e INNER JOIN RTOLocation a ON e.RTOLocationID = a.RTOLocationID where a.Hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and a.District='" + ddldistrictname.SelectedValue.ToString() + "' and e.Activestatus='Y'  Order by RTOLocationName asc ";
                System.Data.DataTable dt1 = Utils.GetDataTable(SQLString11, CnnString);
                ddllocation.DataSource = dt1;
                ddllocation.DataBind();
                ddllocation.Items.Insert(0, new ListItem("--Select Location Name--"));
            }
        } 

        public void FillUsername()
        {
            //string SQLString = "Select Emp_id,Emp_Name from EmployeeMaster where hsrp_stateid='" + HSRP_StateID1 + "' and activestatus='Y' order by Emp_Name";
            //string SQLString = "Select distinct Emp_id,Emp_Name from EmployeeMaster e , AttendanceDetails a where e.Emp_id=a.EmpId and hsrp_stateid='" + HSRPStateID + "' and activestatus='Y'  order by Emp_Name";
            string SQLString = "Select distinct e.Emp_id,(e.Emp_id +'_'+ Emp_Name) as 'Emp_Name' from EmployeeMaster e , AttendanceLog a where e.Emp_id=a.Emp_Id and e.hsrp_stateid='" + HSRPStateID + "' and e.rtolocationid='" + ddllocation.SelectedValue + "' and activestatus='Y'  order by Emp_Name asc";
            System.Data.DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            ddlUserAccount.DataSource = dt;
            ddlUserAccount.DataTextField = "Emp_Name";
            ddlUserAccount.DataValueField = "Emp_id";
            ddlUserAccount.DataBind();
            ddlUserAccount.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select username--"));
            ddlUserAccount.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All"));
            ddlUserAccount.Items.Insert(2, new System.Web.UI.WebControls.ListItem("AllLocation"));
        }


        private void FilldropDowndistrictcenter()
        {
            if (UserType.Equals(0))
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                string SQLString11 = "select distinct district ,district from rtolocation where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' and EmbCenterName is not null group by district ,district Order by 1";
                //string SQLString11 = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName asc";
                Utils.PopulateDropDownList(ddldistrictname, SQLString11.ToString(), CnnString, "--Select District Name--");
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                //string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
                
                //string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM Employeemaster e INNER JOIN RTOLocation a ON e.RTOLocationID = a.RTOLocationID where a.Hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and EmbCenterName='" + ddldistrictname.SelectedValue.ToString() + "' and e.Activestatus='Y'  Order by RTOLocationName asc ";
                ////string SQLString11 = "select distinct EmbCenterName ,EmbCenterName from rtolocation where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N' and EmbCenterName is not null group by EmbCenterName,EmbCenterName Order by 1";
                string SQLString11 = "select distinct district ,district from rtolocation where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N' and EmbCenterName is not null group by district ,district Order by 1";
                System.Data.DataTable dt1 = Utils.GetDataTable(SQLString11, CnnString);
                ddldistrictname.DataSource = dt1;
                ddldistrictname.DataBind();
                ddldistrictname.Items.Insert(0, new ListItem("--Select District Name--"));
            }
        } 



        //private void FilldropDownListClient()
        //{
        //    if (UserType.Equals(0))
        //    {
        //        int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

        //        Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
        //        // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
        //    }
        //    else
        //    {
        //        // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
        //        SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' Order by RTOLocationName ";

        //        DataSet dss = Utils.getDataSet(SQLString, CnnString);
        //        dropDownListClient.DataSource = dss;
        //        dropDownListClient.DataBind();
        //    }
        //}

        #endregion


        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FilldropDownListClient();
        }
       // string strInvoiceNo = "54321";
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAndDownloadFile();
            }
            catch (Exception ex)
            {
                lblErrMess.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        string filename = string.Empty;

        private void SaveAndDownloadFile()
        {
            Workbook book = new Workbook();
            filename = "Attendance Sheet" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            //string strOrderType = string.Empty;

            Export(book, 1, "EmployeeMonthlyPerformanceReport");

          

        }

        int icount = 0;
   

        public System.Data.DataTable GetRecordsAttendanceLog( string month, string year,string employeeID)
        {
            try
            {
                string Query = "EmployeeMonthlyPerformanceReport";
                SqlParameter[] sqlParameter = {

               new SqlParameter("@month", month),
               new SqlParameter("@year", year),
               new SqlParameter("@empcode", employeeID)               
               // new SqlParameter("@EmployeeId",employeeID),
                //new SqlParameter("@MonthName",Convert.ToDateTime(Datefrom.ToShortDateString()))               
                };
                return objutil.GetRecords(Query, sqlParameter,con);
            }
            catch (Exception)
            {
                throw;
            }
        }

     

        private void Export(Workbook book, int iActiveSheet, string strProcName)
        {
            try
            {
                SqlConnection con = new SqlConnection(CnnString);


                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = iActiveSheet;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Collection Summary";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook

                #region Styles
               // if (icount <= 0)
               // {
                    icount++;
                    WorksheetStyle style = book.Styles.Add("HeaderStyle");

                    style.Font.FontName = "Tahoma";
                    style.Font.Size = 9;
                    style.Font.Bold = false;
                    style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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


                    WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                    style6.Font.FontName = "Tahoma";
                    style6.Font.Size = 10;
                    style6.Font.Bold = true;
                    style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                    WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                    style9.Font.FontName = "Tahoma";
                    style9.Font.Size = 10;
                    style9.Font.Bold = true;
                    style9.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style9.Interior.Color = "#FCF6AE";
                    style9.Interior.Pattern = StyleInteriorPattern.Solid;

              //  }
                #endregion

                Worksheet sheet = book.Worksheets.Add("Attendance Sheet");
                string strquery1 = string.Empty;
                DataTable selectedEmployees;
                #region Fetch Data

                if (ddlUserAccount.SelectedValue == "AllLocation")
                {
                    strquery1 = "select distinct al.Emp_ID as 'EmpID' ,em.Emp_Name as 'Name', r.RtolocationName as 'RTOLocationName' from AttendanceLog al,employeemaster em , rtolocation r where al.Emp_Id=em.Emp_ID and em.rtolocationid=r.rtolocationid and al.hsrp_stateid='" + HSRPStateID + "' and em.activestatus='Y' ";//and em.Emp_ID like '%LR%'  and ad.Emp_ID='" + ddlUserAccount.SelectedValue.ToString() + "'  --and  r.rtolocationid='" + ddllocation.SelectedValue.ToString() + "'
                    //string strquery = "select distinct al.Emp_ID from AttendanceLog al,employeemaster em where al.Emp_Id=em.Emp_ID and al.hsrp_stateid='" + HSRP_StateID1 + "' and em.activestatus='Y' and em.Emp_ID like '%LR%'";// and approvalhead='" + Session["UID"].ToString() + "'";                    
                }

                else if (ddlUserAccount.SelectedValue == "All")
                {
                    strquery1 = "select distinct al.Emp_ID as 'EmpID' ,em.Emp_Name as 'Name', r.RtolocationName as 'RTOLocationName' from AttendanceLog al,employeemaster em , rtolocation r where al.Emp_Id=em.Emp_ID and em.rtolocationid=r.rtolocationid and al.hsrp_stateid='" + HSRPStateID + "' and em.activestatus='Y'  and  r.rtolocationid='" + ddllocation.SelectedValue.ToString() + "'";//and em.Emp_ID like '%LR%'  and ad.Emp_ID='" + ddlUserAccount.SelectedValue.ToString() + "'
                    //string strquery = "select distinct al.Emp_ID from AttendanceLog al,employeemaster em where al.Emp_Id=em.Emp_ID and al.hsrp_stateid='" + HSRP_StateID1 + "' and em.activestatus='Y' and em.Emp_ID like '%LR%'";// and approvalhead='" + Session["UID"].ToString() + "'";                    
                }
                else
                {
                    strquery1 = "select Top 1 ad.Emp_ID as 'EmpID', em.Emp_Name as 'Name', r.RtolocationName as 'RTOLocationName' from AttendanceLog ad, employeemaster em , rtolocation r where ad.Emp_ID=em.Emp_id and em.rtolocationid=r.rtolocationid and ad.hsrp_stateid='" + HSRPStateID + "' and ad.Emp_ID='" + ddlUserAccount.SelectedValue.ToString() + "'";
                    //string strquery = "select distinct Emp_ID from AttendanceLog where hsrp_stateid='" + HSRP_StateID1 + "' and Emp_ID='" + userid + "'";
                }


               // string strquery1 = "select Top 1 ad.Emp_ID as 'EmpID', em.Emp_Name as 'Name', r.RtolocationName as 'RTOLocationName' from AttendanceLog ad, employeemaster em , rtolocation r where ad.Emp_ID=em.Emp_id and em.rtolocationid=r.rtolocationid and ad.hsrp_stateid='" + HSRPStateID + "' and ad.Emp_ID='" + ddlUserAccount.SelectedValue.ToString() + "'";
                //string strquery1 = "select Top 1 AD.EmpId,RE.Emp_Name as 'Name',RE.Designation as 'Designation',RE.Department as 'Department',(Select RTOLocationName from rtolocation where RTOLocationID=RE.rtolocationid) as 'Location',(Select District from rtolocation where RTOLocationID=RE.rtolocationid) as 'District',(Select ZonalManager from rtolocation where RTOLocationID=RE.rtolocationid) as 'Zone' from AttendanceDetails AD inner join Employeemaster RE on AD.EmpId=RE.Emp_ID where RE.hsrp_stateid='" + HSRPStateID + "' and ad.EmpID='" + ddlUserAccount.SelectedValue.ToString() + "'";
                // string strquery1 = "select Top 1 AD.EmpId,RE.Emp_Name as 'Name',RE.Designation as 'Designation',RE.Department as 'Department',(Select RTOLocationName from rtolocation where RTOLocationID=RE.rtolocation) as 'Location',(Select District from rtolocation where RTOLocationID=RE.rtolocation) as 'District',(Select ZonalManager from rtolocation where RTOLocationID=RE.rtolocation) as 'Zone' from AttendanceDetails AD inner join Employeemaster RE on AD.EmpId=RE.Emp_ID";
                selectedEmployees = Utils.GetDataTable(strquery1, CnnString);
                //if (selectedEmployees.Rows.Count > 0)
                //{
                //    for (int i = 0; i < selectedEmployees.Rows.Count; i++)
                //    { 
                //        //"@month", ddlMonth.SelectedValue,"@year", ddlYear.SelectedValue, "@empcode", ddlUserAccount.SelectedValue,
                //        GetRecordsAttendanceLog(selectedEmployees.Rows[0]["Month"].ToString().Trim(), selectedEmployees.Rows[0]["Year"].ToString().Trim(), selectedEmployees.Rows[0]["EmpID"].ToString().Trim());
                //        // GetSingleEmpRecords = GetRecordsAttendanceLog(s, Convert.ToDateTime(Datefrom.SelectedDate.ToShortDateString()));
                //    }
                //}

                if (selectedEmployees.Rows.Count > 0)
                {
                    WorksheetRow row = sheet.Table.Rows.Add();
                    int iIndex = 1;
                    DataTable dtempdata = Utils.GetDataTable(strquery1, CnnString);
                    for (int i1 = 0; i1 < selectedEmployees.Rows.Count; i1++)
                    {
                        string _empcode = selectedEmployees.Rows[i1]["empid"].ToString();
                        string _empname = selectedEmployees.Rows[i1]["Name"].ToString();
                        string _empnamelocation = selectedEmployees.Rows[i1]["RTOLocationName"].ToString();
                        
                        DataSet ds = new DataSet();
                        SqlCommand cmd = new SqlCommand();
                        string strParameter = string.Empty;
                        cmd = new SqlCommand(strProcName, con);
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(new SqlParameter("@month", ddlMonth.SelectedValue));
                        cmd.Parameters.Add(new SqlParameter("@year", ddlYear.SelectedValue));
                        cmd.Parameters.Add(new SqlParameter("@empcode", _empcode.Trim().ToString()));
                        // cmd.Parameters.Add(new SqlParameter("@dateto", Convert.ToDateTime(HSRPAuthDate.SelectedDate)));


                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        dt = new DataTable();
                        da.Fill(dt);
                #endregion

                        if (dt.Rows.Count > 0)
                        {

                            AddColumnToSheet(sheet, 100, dt.Columns.Count);
                            if (i1>0)                             
                                iIndex = iIndex+7;
                            

                            //int iIndex = 1;
//                            WorksheetRow row = sheet.Table.Rows.Add();
                            row.Index = iIndex++;
                            row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                            WorksheetCell cell = row.Cells.Add("Attendance Status");
                            cell.MergeAcross = 4; // Merge two cells together
                            cell.StyleID = "HeaderStyle2";

                            row = sheet.Table.Rows.Add();
                            row.Index = iIndex++;

                            AddNewCell(row, "State:", "HeaderStyle2", 1);
                            AddNewCell(row, DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2", 1);
                            row = sheet.Table.Rows.Add();
                            row.Index = iIndex++;

                            //row = sheet.Table.Rows.Add();
                            // row.Index = iIndex++;

                            AddNewCell(row, "Employee Name:", "HeaderStyle2", 1);
                            AddNewCell(row, _empname.ToString(), "HeaderStyle2", 1);

                            AddNewCell(row, "Employee Code:", "HeaderStyle2", 1);
                            AddNewCell(row, _empcode.ToString(), "HeaderStyle2", 1);

                            AddNewCell(row, "Location Name :", "HeaderStyle2", 1);
                            AddNewCell(row, _empnamelocation.ToString(), "HeaderStyle2", 1);

                            row = sheet.Table.Rows.Add();
                            row.Index = iIndex++;


                            DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                            AddNewCell(row, "Report Month :", "HeaderStyle2", 1);
                            AddNewCell(row, ddlMonth.SelectedItem.ToString(), "HeaderStyle2", 1);
                            row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                            row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                            AddNewCell(row, "", "HeaderStyle2", 2);
                            row = sheet.Table.Rows.Add();

                            row.Index = iIndex++;

                            //AddNewCell(row, "", "HeaderStyle6", 1);

                            //row = sheet.Table.Rows.Add();

                            //row.Index = iIndex++;
                            

                            for (int i = 0; i < dt.Columns.Count; i++)
                            {

                                AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                            }
                           
                            row = sheet.Table.Rows.Add();
                            row.Index = iIndex++;
                            

                            for ( int j = 0; j < dt.Rows.Count; j++)
                            {
                                for (int i = 0; i < dt.Columns.Count; i++)
                                {
                                    AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);
                                }
                                row = sheet.Table.Rows.Add();

                            }


                        }

                    }
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    // Save the file and open it
                    book.Save(Response.OutputStream);
                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();
                }
                    else
                    {
                        lblErrMess.Text = "Record not Found";
                        return;
                    }
              

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static void StyleForTheFirstTime(Workbook book)
        {
            // Specify which Sheet should be opened and the size of window by default
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.ExcelWorkbook.WindowTopX = 100;
            book.ExcelWorkbook.WindowTopY = 200;
            book.ExcelWorkbook.WindowHeight = 7000;
            book.ExcelWorkbook.WindowWidth = 8000;

            // Some optional properties of the Document
            book.Properties.Author = "HSRP";
            book.Properties.Title = "HSRP Affixation Report";
            book.Properties.Created = DateTime.Now;

            #region Style
            WorksheetStyle style = book.Styles.Add("HeaderStyle");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 9;
            style.Font.Bold = false;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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

            WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
            style6.Font.FontName = "Tahoma";
            style6.Font.Size = 10;
            style6.Font.Bold = true;
            style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

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

            WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
            style9.Font.FontName = "Tahoma";
            style9.Font.Size = 10;
            style9.Font.Bold = true;
            style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style9.Interior.Color = "#FCF6AE";
            style9.Interior.Pattern = StyleInteriorPattern.Solid;
            #endregion
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

        protected void ddllocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddllocation.SelectedItem.ToString() == "--Select Location Name--")
                {
                 
                }
                else{
                     FillUsername();

                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ddldistrictname_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                FilldropDownListClient();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}