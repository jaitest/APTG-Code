using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using iTextSharp.text;
using CarlosAg.ExcelXmlWriter;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Net;
using System.IO;
using System.Text;
using System.Text;
using System.Globalization;
//using System.Runtime.InteropServices.COMException;
namespace HSRP.Expenses
{


    public partial class AttandancReport_1 : System.Web.UI.Page
    {
        Utils objutil = new Utils();
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        string SQLString1 = string.Empty;
        string CnnString = String.Empty;
        SqlConnection con;
        //DataTable dt = new DataTable();
        //DataTable dt1 = new DataTable();

        string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty, DealerID = string.Empty;
        int Month, Year, Index, Days = 0;
        int EmpMonth;
        int EmpYear;
        DateTime EmpFromDate;
        DateTime EmpToDate;
        string FileName = string.Empty;
        string Employee = string.Empty;
        string EmployeeID = string.Empty;
        string EmpLocation = string.Empty;
        string userid = string.Empty;
        string dbUserName = string.Empty;
        System.Data.DataTable GetEmployeeRecords = null;
        int intHSRPStateID;

        string SQLString11 = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType1 = Convert.ToInt32(Session["UserType"]);
                CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
                strUserID1 = Session["UID"].ToString();

                HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
                RTOLocationID1 = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                // HSRPStateID = Session["UserHSRPStateID"].ToString();
                //HSRP_StateID = Convert.ToInt32(HSRPStateID);
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserName = Session["UID"].ToString();
                DealerID = Session["dealerid"].ToString();
                if (!IsPostBack)
                {
                    gridTD.Visible = false;
                    grd.Visible = false;
                    try
                    {
                        if (UserType1.Equals(0))
                        {
                            DropDownListStateName.Visible = true;
                            FilldropDownListStateName();
                            //Datefrom.SelectedDate = System.DateTime.Now;
                            //Dateto.SelectedDate = System.DateTime.Now;
                            InitialSetting();
                            FilldropDownListClient();
                        }
                        else
                        {
                            DropDownListStateName.Visible = true;
                            FilldropDownListStateName();
                            FillUsername();
                            FilldropDownListClient();
                            //Datefrom.SelectedDate = System.DateTime.Now;
                            //Dateto.SelectedDate = System.DateTime.Now;
                            //FilldropddlRTOLocation(HSRP_StateID1);
                            InitialSetting();
                        }
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                string SQLString11 = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                Utils.PopulateDropDownList(ddllocation, SQLString11.ToString(), CnnString1, "--Select RTO Name--");
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                //string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
                string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM Employeemaster e INNER JOIN RTOLocation a ON e.RTOLocationID = a.RTOLocationID where a.Hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and e.Activestatus='Y'  ";
                GetEmployeeRecords = Utils.GetDataTable(SQLString11, CnnString1);
                ddllocation.DataSource = GetEmployeeRecords;
                ddllocation.DataBind();
                ddllocation.Items.Insert(0, new ListItem("--Select Location Name--"));               
            }
        } 

        private void FilldropDownListStateName()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString1.ToString(), CnnString1, "--Select State--");
            }
            else
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState where HSRP_StateID=" + HSRP_StateID1 + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }


        private void InitialSetting()
        {
            // string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            // string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string indate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();//System.DateTime.Now.ToString();
            string outdate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();//System.DateTime.Now.ToString();
            Datefrom.SelectedDate = (DateTime.Parse(indate)).AddDays(0.00);
            Datefrom.MaxDate = DateTime.Parse(outdate);
            CalendarDatefrom.SelectedDate = (DateTime.Parse(indate)).AddDays(0.00);
            CalendarDatefrom.VisibleDate = (DateTime.Parse(indate)).AddDays(0.00);

            Dateto.SelectedDate = (DateTime.Parse(indate)).AddDays(0.00);
            Dateto.MaxDate = DateTime.Parse(outdate);
            CalendarDateto.SelectedDate = (DateTime.Parse(indate)).AddDays(0.00);
            CalendarDateto.VisibleDate = (DateTime.Parse(indate)).AddDays(0.00);


        }

        private Boolean validate1()
        {
            Boolean blnvalid = true;
            String getvalue = string.Empty;
            getvalue = DropDownListStateName.SelectedItem.Text;
            if (getvalue == "--Select State--")
            {
                blnvalid = false;
                Label1.Text = "Please select State Name";
            }
            return blnvalid;
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(CnnString1);
                #region Fetch Data
                System.Data.DataTable dt = new System.Data.DataTable();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                //change sp Name
                string Query = "GetRecordsAttendanceLog1";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@EmployeeId", selectedEmployees.Rows[0]["Emp_ID"]));
                cmd.Parameters.Add(new SqlParameter("@MonthName", (Datefrom.SelectedDate.ToShortDateString())));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                #endregion
                if (dt.Rows.Count > 0)
                {
                    gridTD.Visible = true;
                    grd.DataSource = dt;
                    grd.DataBind();
                    grd.Visible = true;
                }
                else
                {
                    gridTD.Visible = false;
                    grd.DataSource = null;
                    grd.Visible = false;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // DisplayInExcel(System.Data.DataTable selectedEmployees);

        }

        protected void DropDownListStateName_TextChanged(object sender, EventArgs e)
        {
            gridTD.Visible = false;
            grd.DataSource = null;
            grd.Visible = false;
        }


        public System.Data.DataTable GetRecordsAttendanceLog(string employeeID, DateTime Datefrom)
        {
            try
            {
                string Query = "GetRecordsAttendanceLog1";
                SqlParameter[] sqlParameter = {
                new SqlParameter("@EmployeeId",employeeID),
                new SqlParameter("@MonthName",Convert.ToDateTime(Datefrom.ToShortDateString()))               
                };
                return objutil.GetRecords(Query, sqlParameter, con);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void FillUsername()
        {
            //string SQLString = "Select Emp_id,Emp_Name from EmployeeMaster where hsrp_stateid='" + HSRP_StateID1 + "' and UserId='" + UserName + "' and rtolocationid='" + ddllocation.SelectedValue + "' and activestatus='Y' order by Emp_Name";
            string SQLString = "Select Emp_id,Emp_Name from EmployeeMaster where hsrp_stateid='" + HSRP_StateID1 + "' and rtolocationid='" + ddllocation.SelectedValue + "' and activestatus='Y' order by Emp_Name";
            System.Data.DataTable dt = Utils.GetDataTable(SQLString, CnnString1);
            ddlUserAccount.DataSource = dt;
            ddlUserAccount.DataTextField = "Emp_Name";
            ddlUserAccount.DataValueField = "Emp_id";
            ddlUserAccount.DataBind();
            ddlUserAccount.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select username--"));
            ddlUserAccount.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All"));
        }
        string strquery1 = string.Empty;
        public void DisplayInExcel(System.Data.DataTable selectedEmployees)
        {
            string strquery = string.Empty;
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workBook;
            Microsoft.Office.Interop.Excel.Worksheet workSheet;
            try
            {
                workBook = excelApp.Workbooks.Add();
                workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;
                userid = ddlUserAccount.SelectedValue.ToString();
                dbUserName = ddlUserAccount.SelectedItem.Text.ToString();
                if (ddlUserAccount.SelectedValue == "All")
                {
                    strquery1 = "select Distinct ad.Emp_ID as 'EmpID', em.Emp_Name as 'Name', r.RtolocationName as 'RTOLocationName' from AttendanceLog ad, employeemaster em , rtolocation r where ad.Emp_ID=em.Emp_id and em.rtolocationid=r.rtolocationid and ad.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and em.activestatus='Y'  and r.rtolocationid='" + ddllocation.SelectedValue.ToString() + "'";
                    //strquery1 = "select distinct al.Emp_ID as 'EmpID' ,em.Emp_Name as 'Name', r.RtolocationName as 'RTOLocationName' from AttendanceLog al,employeemaster em , rtolocation r where al.Emp_Id=em.Emp_ID and em.rtolocationid=r.rtolocationid and al.hsrp_stateid='" + HSRPStateID + "' and em.activestatus='Y' and em.Emp_ID like '%LR%' and  r.rtolocationid='" + ddllocation.SelectedValue.ToString() + "'";
                    //string strquery = "select distinct al.Emp_ID from AttendanceLog al,employeemaster em where al.Emp_Id=em.Emp_ID and al.hsrp_stateid='" + HSRP_StateID1 + "' and em.activestatus='Y' and em.Emp_ID like '%LR%'";// and approvalhead='" + Session["UID"].ToString() + "'";

                }
                else
                {
                    strquery1 = "select Top 1 ad.Emp_ID as 'EmpID', em.Emp_Name as 'Name', r.RtolocationName as 'RTOLocationName' from AttendanceLog ad, employeemaster em , rtolocation r where ad.Emp_ID=em.Emp_id and em.rtolocationid=r.rtolocationid and ad.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "'  and ad.Emp_ID='" + ddlUserAccount.SelectedValue.ToString() + "'";
                    //string strquery = "select distinct Emp_ID from AttendanceLog where hsrp_stateid='" + HSRP_StateID1 + "' and Emp_ID='" + userid + "'";
                }
                //if (ddlUserAccount.SelectedValue == "All")
                //{
                //    strquery = "select distinct al.Emp_ID from AttendanceLog al,employeemaster em where al.Emp_Id=em.Emp_ID and al.hsrp_stateid='" + HSRP_StateID1 + "' and em.activestatus='Y' and em.Emp_ID like '%LR%'";// and approvalhead='" + Session["UID"].ToString() + "'";                   
                //}
                //else
                //{
                //    strquery = "select distinct Emp_ID from AttendanceLog where hsrp_stateid='" + HSRP_StateID1 + "' and Emp_ID='" + userid + "'";
                //}

                //System.Data.DataTable dtemp = Utils.GetDataTable(strquery, CnnString1);
                selectedEmployees = Utils.GetDataTable(strquery1, CnnString1);

                if (selectedEmployees.Rows.Count > 0)
                {
                    for (int i = 0; i < selectedEmployees.Rows.Count; i++)
                    {
                        GetRecordsAttendanceLog(selectedEmployees.Rows[0]["EmpID"].ToString().Trim(), Convert.ToDateTime(Datefrom.SelectedDate.ToShortDateString()));
                        // GetSingleEmpRecords = GetRecordsAttendanceLog(s, Convert.ToDateTime(Datefrom.SelectedDate.ToShortDateString()));
                    }
                }
                if (selectedEmployees.Rows.Count > 0)
                {
                    int startEmpIndex = 2;
                    for (int n = 0; n < selectedEmployees.Rows.Count; n++)
                    {
                        System.Data.DataTable GetSingleEmpRecords = new System.Data.DataTable();
                        string s = selectedEmployees.Rows[n]["EmpID"].ToString().Trim();
                        GetSingleEmpRecords = GetRecordsAttendanceLog(s, Convert.ToDateTime(Datefrom.SelectedDate.ToShortDateString()));
                        //Fatech Data in Employee---------------
                        strquery1 = "select Top 1 ad.Emp_ID as 'EmpID', em.Emp_Name, r.RtolocationName from AttendanceLog ad, employeemaster em , rtolocation r where ad.Emp_ID=em.Emp_id and em.rtolocationid=r.rtolocationid and ad.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and ad.Emp_ID='" + s + "'";
                        System.Data.DataTable dtempdata = Utils.GetDataTable(strquery1, CnnString1);

                        //Fatech Data in Employee--------------- 

                        EmpMonth = Convert.ToDateTime(Datefrom.SelectedDate.ToShortDateString()).Month;
                        EmpYear = Convert.ToDateTime(Datefrom.SelectedDate.ToShortDateString()).Year;
                        EmpFromDate = Convert.ToDateTime(Datefrom.SelectedDate.ToShortDateString());
                        EmpToDate = Convert.ToDateTime(Dateto.SelectedDate.ToShortDateString());

                        int daysCount = DateTime.DaysInMonth(EmpYear, EmpMonth);
                        DateTime datetime = new DateTime(EmpYear, EmpMonth, 1);

                        int DayStartRowIndex = 4;
                        //int DayStartASCII = 65;
                        int maxPunch = 0;
                        double[] workingHours = new double[daysCount];
                        int[] punchCount = new int[daysCount];

                        int woCount = 0;
                        int powCount = 0;
                        int aCount = 0;
                        int pCount = 0;
                        int hlfCount = 0;
                        for (int i = EmpFromDate.Day; i <= EmpToDate.Day; i++)
                        {
                            int temp = 0;
                            int tempPunchCount = 0;
                            DateTime tempFromDateTime = default(DateTime);
                            DateTime tempToDateTime;
                            TimeSpan span;
                            for (int j = 0; j < GetSingleEmpRecords.Rows.Count; j++)
                            {
                                if (Convert.ToInt32(Convert.ToDateTime(GetSingleEmpRecords.Rows[j]["INOUTDateTime"]).Day) == i && tempPunchCount <= punchCount[i - 1])
                                {
                                    tempPunchCount++;
                                    temp++;
                                    workSheet.Cells[DayStartRowIndex + startEmpIndex + tempPunchCount, 1 + i] = Convert.ToDateTime(GetSingleEmpRecords.Rows[j]["INOUTDateTime"]).Hour + ":" + Convert.ToDateTime(GetSingleEmpRecords.Rows[j]["INOUTDateTime"]).Minute;
                                    if (temp % 2 == 0 && temp > 1)
                                    {
                                        tempToDateTime = Convert.ToDateTime(GetSingleEmpRecords.Rows[j]["INOUTDateTime"]);
                                        span = tempToDateTime - tempFromDateTime;
                                        workingHours[i - 1] += span.Hours;
                                    }
                                    else
                                        tempFromDateTime = Convert.ToDateTime(GetSingleEmpRecords.Rows[j]["INOUTDateTime"]);
                                    punchCount[i - 1]++;
                                }
                            }
                            if (temp > maxPunch)
                                maxPunch = temp;
                        }

                        workSheet.Range["B" + startEmpIndex + ":AF" + startEmpIndex].Font.Size = 11;
                        workSheet.Range["B" + startEmpIndex + ":AF" + startEmpIndex].Font.FontStyle = "Segoe UI";
                        workSheet.Range["B" + startEmpIndex + ":AF" + startEmpIndex].Font.Bold = true;
                        workSheet.Range["B" + startEmpIndex + ":AF" + startEmpIndex].MergeCells = true;
                        workSheet.Range["B" + startEmpIndex + ":AF" + startEmpIndex].Font.Name = "Arial Rounded MT Bold";
                        workSheet.Range["B" + (startEmpIndex + 1) + ":AF" + (startEmpIndex + 1)].Font.Size = 11;
                        workSheet.Range["B" + (startEmpIndex + 1) + ":AF" + (startEmpIndex + 1)].Font.FontStyle = "Segoe UI";
                        workSheet.Range["B" + (startEmpIndex + 1) + ":AF" + (startEmpIndex + 1)].Font.Bold = true;
                        workSheet.Range["B" + (startEmpIndex + 1) + ":AF" + (startEmpIndex + 1)].MergeCells = true;
                        workSheet.Range["B" + (startEmpIndex + 1) + ":AF" + (startEmpIndex + 1)].Font.Name = "Arial Rounded MT Bold";
                        workSheet.Range["B" + (startEmpIndex + 2) + ":AF" + (startEmpIndex + 2)].Font.Size = 11;
                        workSheet.Range["B" + (startEmpIndex + 2) + ":AF" + (startEmpIndex + 2)].Font.FontStyle = "Segoe UI";
                        workSheet.Range["B" + (startEmpIndex + 2) + ":AF" + (startEmpIndex + 2)].Font.Bold = true;
                        workSheet.Range["B" + (startEmpIndex + 2) + ":AF" + (startEmpIndex + 2)].MergeCells = true;
                        workSheet.Range["B" + (startEmpIndex + 2) + ":AF" + (startEmpIndex + 2)].Font.Name = "Arial Rounded MT Bold";
                        workSheet.Cells[startEmpIndex, "B"] = "Employee : - " + dtempdata.Rows[0]["Emp_Name"] + "  " + "Employee Code : - " + dtempdata.Rows[0]["EmpID"];
                        workSheet.Cells[(startEmpIndex + 1), "B"] = "Location : - " + dtempdata.Rows[0]["RtoLocationName"];
                        workSheet.Cells[(startEmpIndex + 2), "B"] = "From : - " + EmpFromDate.ToString("dd-MMM-yyyy") + "  " + "To : - " + EmpToDate.ToString("dd-MMM-yyyy");


                        workSheet.Cells[DayStartRowIndex + startEmpIndex, "A"] = "DAY";

                        int oddPunch = 0;
                        if (maxPunch % 2 == 1)
                            oddPunch = 1;
                        for (int i = 1; i <= maxPunch + oddPunch; i++)
                        {
                            if (i == 1 || i % 2 == 1)
                                workSheet.Cells[DayStartRowIndex + startEmpIndex + i, "A"] = "IN TIME";
                            else
                                workSheet.Cells[DayStartRowIndex + startEmpIndex + i, "A"] = "OUT TIME";
                        }
                        workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 1, "A"] = "STATUS";
                        workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 2, "A"] = "WORK HRS";
                        workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 3, "A"] = "REPORTS:";

                        workSheet.Range["A" + DayStartRowIndex + startEmpIndex + ":A" + DayStartRowIndex + startEmpIndex + maxPunch + oddPunch].Font.Size = 10;
                        workSheet.Range["A" + DayStartRowIndex + startEmpIndex + ":A" + DayStartRowIndex + startEmpIndex + maxPunch + oddPunch].Font.FontStyle = "Segoe UI";
                        workSheet.Range["A" + DayStartRowIndex + startEmpIndex + ":A" + DayStartRowIndex + startEmpIndex + maxPunch + oddPunch].Font.Bold = true;

                        for (int i = 1; i <= daysCount; i++)
                        {
                            int tempPunchCount = 0;
                            //Number of Day of the selected Month
                            workSheet.Cells[DayStartRowIndex + startEmpIndex, 1 + i] = "" + i;
                            for (int j = 0; j < GetSingleEmpRecords.Rows.Count; j++)
                            {
                                if (Convert.ToDateTime(GetSingleEmpRecords.Rows[j]["INOUTDateTime"]).Day == i && tempPunchCount <= punchCount[i - 1]
                                    && Convert.ToDateTime(GetSingleEmpRecords.Rows[j]["INOUTDateTime"]).Day >= Convert.ToDateTime(Datefrom.SelectedDate.ToShortDateString()).Day
                                    && Convert.ToDateTime(GetSingleEmpRecords.Rows[j]["INOUTDateTime"]).Day <= Convert.ToDateTime(Dateto.SelectedDate.ToShortDateString()).Day)
                                {
                                    tempPunchCount++;
                                    punchCount[i - 1]++;
                                    //Total Working Hours on ith Day
                                    workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 2, 1 + i] = workingHours[i - 1];
                                }
                            }
                            if (datetime.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                            {
                                if (workingHours[i - 1] < 8 && workingHours[i - 1] >= 4)
                                {
                                    workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 1, 1 + i] = "POW";
                                    powCount++;
                                }
                                else
                                {
                                    workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 1, 1 + i] = "WO";
                                    woCount++;
                                }
                                workSheet.Range[workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 1, 1 + i], workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 1, 1 + i]].Font.Color = XlRgbColor.rgbMediumPurple;
                                workSheet.Range[workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 1, 1 + i], workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 1, 1 + i]].Font.Bold = true;
                            }
                            else
                            {
                                if (workingHours[i - 1] >= 8)
                                {
                                    workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 1, 1 + i] = "P";
                                    pCount++;
                                }
                                else if (workingHours[i - 1] < 8 && workingHours[i - 1] >= 4)
                                {
                                    workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 1, 1 + i] = "HLF";
                                    hlfCount++;
                                }
                                else
                                {
                                    workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 1, 1 + i] = "A";
                                    aCount++;
                                }
                            }
                        }

                        workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 3, "B"] = "Weekends Off : " + woCount + "   Prensent On Weekends : " + pCount + "   Presents : " + pCount + "   Half Days : " + hlfCount + "   Absent : " + aCount;
                        workSheet.Range[workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 3, "B"], workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 3, daysCount + 1]].Merge();

                        workSheet.UsedRange.EntireRow.AutoFit();
                        workSheet.Range[workSheet.Cells[DayStartRowIndex + startEmpIndex, 1 + 1], workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 3, 1 + daysCount]].ColumnWidth = 6;
                        workSheet.Range[workSheet.Cells[DayStartRowIndex + startEmpIndex, 1 + 1], workSheet.Cells[DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 3, 1 + daysCount]].HorizontalAlignment =
                        Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        int gap = 2;
                        startEmpIndex = DayStartRowIndex + startEmpIndex + maxPunch + oddPunch + 3 + gap;
                    }
                    workSheet.Name = "Attendance Log";
                    workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, DateTime.DaysInMonth(EmpYear, EmpMonth) + 1]].Merge();

                    workSheet.Cells[1, 1] = "ATTENDANCE LOG";

                    workSheet.Cells[1, 1].Font.Size = 15;
                    workSheet.Cells[1, 1].Font.Bold = true;
                    workSheet.Cells[1, 1].Font.FontStyle = "Segoe UI";
                    workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 1]].HorizontalAlignment =
                    Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                }
                //excelApp.Visible = true;
                workSheet.UsedRange.AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic2);
                // Put the spreadsheet contents on the clipboard.
                workSheet.UsedRange.Copy();
                string filename = "Attendance" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xlsx";
                string workbookpath = Server.MapPath("~\\AttendanceSheet\\" + filename);
                // Response.Write(workbookpath);
                //string workbookpath = @"C:\AttendanceSheet\" + filename;
                workBook.SaveAs(workbookpath);
                string s2 = workbookpath.ToString();
                workBook.Close();
                excelApp.Quit();

                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                Response.WriteFile(s2);
            }
            catch (Exception ex)
            {
                throw ex;
                //Utils.MessageBoxError(ex.Message);
            }
            finally
            {
                //workSheet = null;
                //workBook = null;
                //excelApp = null;
            }
        }

        
        public string monthOrDayCheck(string monthOrDay)
        {
            try
            {
                if (monthOrDay.Length == 1)
                    monthOrDay = "0" + monthOrDay;
                return monthOrDay;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        System.Data.DataTable selectedEmployees = new System.Data.DataTable();
        protected void btn_download_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(Datefrom.SelectedDate.ToShortDateString()).Month == Convert.ToDateTime(Dateto.SelectedDate.ToShortDateString()).Month && Convert.ToDateTime(Datefrom.SelectedDate.ToShortDateString()).Year == Convert.ToDateTime(Dateto.SelectedDate.ToShortDateString()).Year)
                {
                    TimeSpan spanDiff = Convert.ToDateTime(Dateto.SelectedDate.ToShortDateString()) - Convert.ToDateTime(Datefrom.SelectedDate.ToShortDateString());
                    if (spanDiff.TotalDays >= 0)
                    {
                        string strquery = "select distinct Emp_ID from AttendanceLog where hsrp_stateid='" + HSRP_StateID1 + "'";
                        System.Data.DataTable dtemp = Utils.GetDataTable(strquery, CnnString1);

                        if (dtemp.Rows.Count > 0)
                        {
                            DisplayInExcel(GetRecordsAttendanceLog(dtemp.Rows[0]["Emp_ID"].ToString().Trim(), Convert.ToDateTime(Datefrom.SelectedDate.ToShortDateString())));
                        }
                        else
                        {

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        protected void ddllocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillUsername();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}

