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
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using System.Globalization;


namespace HSRP.Report
{
    public partial class AttendanceReport_Old : System.Web.UI.Page
    {
        Utils objutil = new Utils();
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        int intHSRPStateID1;
        int intRTOLocationID1;
        string SQLString1 = string.Empty;
        string OrderType;
        string recordtype = string.Empty;
        //DateTime OrderDate1;
        string userid = string.Empty;
        string dbUserName = string.Empty;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

        int Month, Year, Index, Days = 0;
        int EmpMonth;
        int EmpYear;
        DateTime EmpFromDate;
        DateTime EmpToDate;
        int i;

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
                strUserID1 = Session["UID"].ToString();
                // DealerID = Session["dealerid"].ToString();
                HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
                RTOLocationID1 = Session["UserRTOLocationID"].ToString();
                //UserName = Session["UID"].ToString();
                if (!IsPostBack)
                {
                    try
                    {
                        InitialSetting();
                        if (UserType1.Equals(0))
                        {
                            FillUsername();
                            FilldropDownListOrganization();
                        }
                        else
                        {
                            FillUsername();
                            FilldropDownListOrganization();
                           
                        }
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }

          


        }


        private void FilldropDownListOrganization()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString1.ToString(), CnnString1, "--Select State--");
            }
            else
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID1 + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }


        private void InitialSetting()
        {            
            string indate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();//System.DateTime.Now.ToString();
            string outdate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();//System.DateTime.Now.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(indate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(outdate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(indate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(indate)).AddDays(0.00);

            OrderDate.SelectedDate = (DateTime.Parse(indate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(outdate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(indate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(indate)).AddDays(0.00);


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


        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        protected void DropDownListyearName_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        public System.Data.DataTable GetRecordsAttendanceLog(string employeeID, DateTime Datefrom)
        {
            try
            {
                string Query = "GetAttendanceRecord_Details";
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
            //string SQLString = "Select Emp_id,Emp_Name from EmployeeMaster where hsrp_stateid='" + HSRP_StateID1 + "' and activestatus='Y' order by Emp_Name";
            //string SQLString = "Select distinct Emp_id,Emp_Name from EmployeeMaster e , AttendanceDetails a where e.Emp_id=a.EmpId and hsrp_stateid='" + HSRP_StateID1 + "' and activestatus='Y'  order by Emp_Name";
            //string SQLString = "Select Emp_id,Emp_Name from EmployeeMaster where hsrp_stateid='" + HSRP_StateID1 + "' and UserId='" + strUserID1 + "' and activestatus='Y'";
            string SQLString = "select distinct Emp_id,Emp_Name  from Employeemaster e , AttendanceDetails a where a.EmpId=e.Emp_id and  hsrp_stateid='" + HSRP_StateID1 + "' and Emp_id='LPA1057' and activestatus='Y'";

            System.Data.DataTable dt = Utils.GetDataTable(SQLString, CnnString1);
            ddlUserAccount.DataSource = dt;
            ddlUserAccount.DataTextField = "Emp_Name";
            ddlUserAccount.DataValueField = "Emp_id";
            ddlUserAccount.DataBind();
            ddlUserAccount.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select username--"));
            ddlUserAccount.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All"));
        }


        protected void btnexport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(OrderDate.SelectedDate.ToShortDateString()).Month == Convert.ToDateTime(HSRPAuthDate.SelectedDate.ToShortDateString()).Month && Convert.ToDateTime(OrderDate.SelectedDate.ToShortDateString()).Year == Convert.ToDateTime(HSRPAuthDate.SelectedDate.ToShortDateString()).Year)
                {
                    TimeSpan spanDiff = Convert.ToDateTime(HSRPAuthDate.SelectedDate.ToShortDateString()) - Convert.ToDateTime(OrderDate.SelectedDate.ToShortDateString());
                    if (spanDiff.TotalDays >= 0)
                    {
                        string strquery = "select distinct EmpID from AttendanceDetails a , Employeemaster e where a.Empid=e.Emp_ID and  e.hsrp_stateid='" + HSRP_StateID1 + "'";
                        System.Data.DataTable dtemp = Utils.GetDataTable(strquery, CnnString1);

                        if (dtemp.Rows.Count > 0)
                        {
                            DisplayInExcel(GetRecordsAttendanceLog(dtemp.Rows[0]["EmpID"].ToString().Trim(), Convert.ToDateTime(OrderDate.SelectedDate.ToShortDateString())));
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

            //Export(DropDownListStateName.SelectedValue, DropDownListStateName.SelectedItem.Text);
        }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

      
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
        string sk = string.Empty;
        StringBuilder str = new StringBuilder();
        public void DisplayInExcel(System.Data.DataTable selectedEmployees)
        {
            string strquery = string.Empty;

            try
            {
                userid = ddlUserAccount.SelectedValue.ToString();
                dbUserName = ddlUserAccount.SelectedItem.Text.ToString();
                if (ddlUserAccount.SelectedValue == "All")
                {

                    //strquery = "select distinct al.Emp_ID from AttendanceLog al,employeemaster em where al.Emp_Id=em.Emp_ID and al.hsrp_stateid='" + HSRP_StateID1 + "' and em.RtoLocationid ='" + RTOLocationID.ToString() + "'";    
                    strquery = "select distinct al.EmpID from attendancedetails al,employeemaster em where al.EmpId=em.Emp_ID and al.hsrp_stateid='" + HSRP_StateID1 + "' and approvalhead='" + Session["UID"].ToString() + "'";
                    //strquery = "select distinct al.EmpID from AttendanceDetails al,employeemaster em where al.EmpId=em.Emp_ID and em.hsrp_stateid='" + HSRP_StateID1 + "' and approvalhead='" + Session["UID"].ToString() + "' ";
                }
                else
                {
                    strquery = "select distinct EmpID from attendancedetails al,employeemaster em where al.EmpId=em.Emp_ID and em.hsrp_stateid='" + HSRP_StateID1 + "' and EmpID='" + userid + "'";
                }

                //System.Data.DataTable dtemp = Utils.GetDataTable(strquery, CnnString1);
                selectedEmployees = Utils.GetDataTable(strquery, CnnString1);

                if (selectedEmployees.Rows.Count > 0)
                {
                    for (int i = 0; i < selectedEmployees.Rows.Count; i++)
                    {
                        GetRecordsAttendanceLog(selectedEmployees.Rows[0]["EmpID"].ToString().Trim(), Convert.ToDateTime(HSRPAuthDate.SelectedDate.ToShortDateString()));
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
                        GetRecordsAttendanceLog(s,Convert.ToDateTime(OrderDate.SelectedDate.ToShortDateString()));
                        //Fatech Data in Employee---------------
                        string strquery1 = "select Top 1 ad.EmpID as 'EMPID', em.Emp_Name as 'Name', r.RtolocationName as 'Location' from attendanceDetails ad, employeemaster em , rtolocation r where ad.EmpID=em.Emp_id and em.rtolocationid=r.rtolocationid and em.hsrp_stateid='" + HSRP_StateID1 + "' and ad.EmpID='" + s + "'";
                        System.Data.DataTable dtempdata = Utils.GetDataTable(strquery1, CnnString1);
                        EmpFromDate = Convert.ToDateTime(OrderDate.SelectedDate.ToShortDateString());
                        EmpToDate = Convert.ToDateTime(OrderDate.SelectedDate.ToShortDateString());

                        str.Append("<!DOCTYPE><html><body>");
                        str.Append("<table style='width:100%'>");

                        str.Append("<tr>");
                        str.Append("<td style=min-width:50px>");
                        str.Append("<nobr>Employee Name :</nobr>");
                        str.Append("</td>");
                        str.Append("<td style=min-width:50px>");
                        str.Append("<nobr>" + dtempdata.Rows[0]["Name"] + "</nobr>");
                        str.Append("</td>");

                        str.Append("<td style=min-width:50px>");
                        str.Append("<nobr>Employee Code :</nobr>");
                        str.Append("</td>");
                        str.Append("<td style=min-width:50px>");
                        str.Append("<nobr>" + dtempdata.Rows[0]["EMPID"] + "</nobr>");
                        str.Append("</td>");

                        str.Append("<td style=min-width:50px>");
                        str.Append("<nobr>Location :</nobr>");
                        str.Append("</td>");
                        str.Append("<td style=min-width:50px>");
                        str.Append("<nobr>" + dtempdata.Rows[0]["Location"] + "</nobr>");
                        str.Append("</td>");
                        str.Append("</tr>");
                        str.Append("</tr>");

                        str.Append("<tr>");
                        str.Append("<td style=min-width:50px>");
                        str.Append("<nobr>From:</nobr>");
                        str.Append("</td>");
                        str.Append("<td style=min-width:50px>");
                        str.Append("<nobr>" + EmpFromDate.ToString("dd-MMM-yyyy") + "</nobr>");
                        str.Append("</td>");

                        str.Append("<td style=min-width:50px>");
                        str.Append("<nobr>To :</nobr>");
                        str.Append("</td>");
                        str.Append("<td style=min-width:50px>");
                        str.Append("<nobr>" + EmpToDate.ToString("dd-MMM-yyyy") + "</nobr>");
                        str.Append("</td>");
                        str.Append("</tr>");

                        str.Append("<tr>");
                        str.Append("<td style=min-width:50px>");


                        str.Append("</td>");
                        str.Append("</tr>");

                        int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                        string employeeId = userid;

                        StringBuilder StrDate = new StringBuilder();
                        StringBuilder InTime = new StringBuilder();
                        StringBuilder OutTime = new StringBuilder();
                        StringBuilder PersentAndAbsent = new StringBuilder();
                        StringBuilder DoubleAttach = new StringBuilder();

                        StringBuilder strintime = new StringBuilder();
                        StringBuilder strouttime = new StringBuilder();
                        StringBuilder stringresult = new StringBuilder();

                        OutTime.Append("<tr>");


                        InTime.Append("<tr>");
                        StrDate.Append("<tr>");
                        PersentAndAbsent.Append("<tr>");
                        StrDate.Append("<td style=min-width:10px>Days</td>");
                        InTime.Append("<td style=min-width:10px>In Time</td>");
                        OutTime.Append("<td style=min-width:10px>Out Time</td>");
                        PersentAndAbsent.Append("<td style=min-width:10px>Persent</td>");
                        for (int i = 1; i <= days; i++)
                        {
                            StrDate.Append("<td style=min-width:10px>" + i.ToString() + "</td>");
                            bool Values = false;
                            for (int j = 0; j < GetSingleEmpRecords.Rows.Count; j++)
                            {
                                if (employeeId == GetSingleEmpRecords.Rows[j]["EmpID"].ToString().Trim())
                                {
                                    if (i.ToString() == GetSingleEmpRecords.Rows[j]["INOUTDate"].ToString().Trim())
                                    {
                                        Values = true;                                       

                                       strintime = InTime.Append("<td>" + GetSingleEmpRecords.Rows[j]["INOUTTime"].ToString().Trim() + "</td>");
                                        
                                        if (GetSingleEmpRecords.Rows.Count - 1 > j)
                                        {
                                            if (employeeId == GetSingleEmpRecords.Rows[j + 1]["EmpID"].ToString().Trim())
                                            {
                                                strouttime=OutTime.Append("<td>" + GetSingleEmpRecords.Rows[j + 1]["INOUTTime"].ToString().Trim() + "</td>");
                                                j += 1;

                                                //stringresult = Convert.ToString(strintime) - Convert.ToString(strouttime);
                                                //if (stringresult >= 8)
                                                //{
                                                //    PersentAndAbsent.Append("<td style=min-width:20px>" + "P" + "</td>");
                                                //}
                                                //else if ((stringresult >= 4) && (stringresult < 7))
                                                //{
                                                //    PersentAndAbsent.Append("<td style=min-width:20px>" + "HLF" + "</td>");
                                                //}
                                            }                                            
                                        }
                                        

                                    }

                                    else
                                    {
                                        InTime.Append("<td style=min-width:10px></td>");
                                        OutTime.Append("<td style=min-width:10px></td>");
                                        PersentAndAbsent.Append("<td style=min-width:20px>" + "A" + "</td>");
                                    }
                                }
                            }

                            //if (Values)
                            //{
                            //    PersentAndAbsent.Append("<td style=min-width:20px>" + "P" + "</td>");
                            //}
                            //else if (Values)
                            //{
                            //    PersentAndAbsent.Append("<td style=min-width:20px>" + "HLF" + "</td>");
                            //}
                            //else
                            //{
                            //    InTime.Append("<td style=min-width:10px></td>");
                            //    OutTime.Append("<td style=min-width:10px></td>");
                            //    PersentAndAbsent.Append("<td style=min-width:20px>" + "A" + "</td>");
                            //}
                        }
                        StrDate.Append("</tr>");
                        InTime.Append("</tr>");
                        OutTime.Append("</tr>");

                        PersentAndAbsent.Append("</tr>");

                        str.Append(StrDate.ToString());
                        str.Append(InTime.ToString());
                        str.Append(OutTime.ToString());
                        str.Append(DoubleAttach.ToString());
                        str.Append(PersentAndAbsent.ToString());
                        str.Append("</table>");
                        str.Append("</body></html>");
                        sk = str.ToString();
                    }
                }

                string strFile = "Text_Excel.xls";
                Response.ContentType = "application/x-msexcel";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                Response.ContentEncoding = Encoding.UTF8;
                sk = str.ToString();
                Response.Write(str.ToString());
                Response.End();

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
       
    }
}