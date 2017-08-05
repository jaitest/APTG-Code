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
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace HSRP.Report
{
    public partial class PIPVSReceived : System.Web.UI.Page
    {
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
        string day1date1 = string.Empty;
        string day1date = string.Empty;
        string day2date = string.Empty;
        string day3date = string.Empty;
        string day4date = string.Empty;
        string day5date = string.Empty;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dtsession = new DataTable();

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

                HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
                RTOLocationID1 = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {
                   
                    try
                    {
                        if (UserType1.Equals(0))
                        {
                            DropDownListStateName.Visible = true;
                            FilldropDownListOrganization();
                            OrderDatefrom.SelectedDate = System.DateTime.Now;
                        }
                        else
                        {

                            DropDownListStateName.Visible = true;
                            FilldropDownListOrganization();

                            OrderDatefrom.SelectedDate = System.DateTime.Now;
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
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

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

        protected void btnexport_Click(object sender, EventArgs e)
        {
            SaveAndDownloadFile();
        }

        private void SaveAndDownloadFile()
        {
            Workbook book = new Workbook();
            string filename = "PIP vs Received Summary" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            Export(strOrderType, book, 1, "Business_report_PIPVSReceivedSummary");

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }

        int icount = 0;

        private void Export(string strReportType, Workbook book, int iActiveSheet, string strProcName)
        {
            try
            {
                SqlConnection con = new SqlConnection(CnnString1);
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
                if (icount <= 0)
                {
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

                }
                #endregion

                Worksheet sheet = book.Worksheets.Add("Report");

                string strreptype="E";
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand(strProcName, con);

               
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
                cmd.Parameters.Add(new SqlParameter("@reptype", strreptype));

                

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //  dt = new DataTable();
                da.Fill(dt);
                #endregion

                AddColumnToSheet(sheet, 100, dt.Columns.Count);

                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                WorksheetCell cell = row.Cells.Add("PIP vs Received");
                cell.MergeAcross = 4; // Merge two cells together
                cell.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                AddNewCell(row, "State:", "HeaderStyle2", 1);
                AddNewCell(row, DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                AddNewCell(row, "Report Date:", "HeaderStyle2", 1);
                AddNewCell(row, OrderDatefrom.SelectedDate.ToString("dd/MM/yyyy"), "HeaderStyle2", 1);
                row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "HeaderStyle2"));  
                AddNewCell(row, "", "HeaderStyle2", 2);
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

               // AddNewCell(row, "", "HeaderStyle6", 1);
                // WorksheetCell cell1 = row.Cells.Add("Today's Collection");
                //cell1.MergeAcross = 3; // Merge cells together
                // cell1.StyleID = "HeaderStyle6";
                //WorksheetCell cell2 = row.Cells.Add("Collection and Deposit MTD");
                //cell2.MergeAcross = 3; // Merge cells together
                //cell2.StyleID = "HeaderStyle6";
                //WorksheetCell cell3 = row.Cells.Add("Collection and Deposit Fy YTD");
                //cell3.MergeAcross = 3; // Merge cells together
                //cell3.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //if (dt.Columns[i].ColumnName.ToString().EndsWith("1") || dt.Columns[i].ColumnName.ToString().EndsWith("2"))
                    //{
                    //   // string strCol = dt.Columns[i].ColumnName.ToString();
                    //   // AddNewCell(row, strCol.Remove(strCol.Length - 1), "HeaderStyle6", 1);
                    //}
                    //else
                    //{
                    AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                    //}
                }
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                        //if (dt.Rows[j]["Location"].ToString().Equals("ZZZZZ") && i.Equals(0))
                        //{
                        //    AddNewCell(row, "Total", "HeaderStyle6", 1);
                        //}
                        //else
                        //{
                    if (dt.Rows[j]["Zone"].ToString().Trim().Equals("ZZ") && i.Equals(0))
                    {
                        AddNewCell(row, " ", "HeaderStyle6", 1);
                    }
                    else 
                    { 
                        AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);

                    }
                    }
                    row = sheet.Table.Rows.Add();

                }

            }

            catch (Exception ex)
            {
                throw ex;
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




        private void SaveAndDownloadFileAssignment()
        {
            Workbook book = new Workbook();
            string filename = "ASSIGNMENT VS PRODUCTION" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            ExportAssignment(strOrderType, book, 1, "report_AssignmentVSProduction_Summary");



            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);

            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();



        }

        int icount1 = 0;

        private void ExportAssignment(string strReportType, Workbook book, int iActiveSheet, string strProcName)
        {
            try
            {
                SqlConnection con = new SqlConnection(CnnString1);


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
                if (icount1 <= 0)
                {
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

                }
                #endregion

                Worksheet sheet = book.Worksheets.Add("Report");

                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand(strProcName, con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
                // cmd.Parameters.Add(new SqlParameter("@flag", DropDownListOrderType.SelectedValue));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();


                da.Fill(dt);



                #endregion

                AddColumnToSheet(sheet, 100, dt.Columns.Count);



                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                WorksheetCell cell = row.Cells.Add("ASSIGNMENT VS PRODUCTION");
                cell.MergeAcross = 4; // Merge two cells together
                cell.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                AddNewCell(row, "State:", "HeaderStyle2", 1);
                AddNewCell(row, DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                AddNewCell(row, "Report Date:", "HeaderStyle2", 1);
                AddNewCell(row, OrderDatefrom.SelectedDate.ToString("dd/MM/yyyy"), "HeaderStyle2", 1);
                row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "HeaderStyle2"));  
                AddNewCell(row, "", "HeaderStyle2", 2);
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

               // AddNewCell(row, "", "HeaderStyle6", 1);
                // WorksheetCell cell1 = row.Cells.Add("Today's Collection");
                //cell1.MergeAcross = 3; // Merge cells together
                // cell1.StyleID = "HeaderStyle6";
                //WorksheetCell cell2 = row.Cells.Add("Collection and Deposit MTD");
                //cell2.MergeAcross = 3; // Merge cells together
                //cell2.StyleID = "HeaderStyle6";
                //WorksheetCell cell3 = row.Cells.Add("Collection and Deposit Fy YTD");
                //cell3.MergeAcross = 3; // Merge cells together
                //cell3.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //if (dt.Columns[i].ColumnName.ToString().EndsWith("1") || dt.Columns[i].ColumnName.ToString().EndsWith("2"))
                    //{
                    //   // string strCol = dt.Columns[i].ColumnName.ToString();
                    //   // AddNewCell(row, strCol.Remove(strCol.Length - 1), "HeaderStyle6", 1);
                    //}
                    //else
                    //{
                    AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                    //}
                }
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                        //if (dt.Rows[j]["Location"].ToString().Equals("ZZZZZ") && i.Equals(0))
                        //{
                        //    AddNewCell(row, "Total", "HeaderStyle6", 1);
                        //}
                        //else
                        //{
                        AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);

                        //}
                    }
                    row = sheet.Table.Rows.Add();

                }


            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void btnassign_Click(object sender, EventArgs e)
        {
            SaveAndDownloadFileAssignment();
        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    SqlConnection con = new SqlConnection(CnnString1);
        //    #region Fetch Data
        //    DataSet ds = new DataSet();

        //    SqlCommand cmd = new SqlCommand();
        //    string strParameter = string.Empty;
        //    cmd = new SqlCommand("Business_report_DispatchvsReceived_Summary", con);


        //    cmd.CommandType = CommandType.StoredProcedure;

        //    cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
        //    cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
        //    // cmd.Parameters.Add(new SqlParameter("@flag", DropDownListOrderType.SelectedValue));

        //    cmd.CommandTimeout = 0;
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    // dt = new DataTable();
        //    da.Fill(dt);


        //    #endregion
        //    if (dt.Rows.Count > 0)
        //    {
        //        grdid.DataSource = dt;
        //        grdid.DataBind();
        //        grdid.Visible = true;
        //    }
        //}

        DateTime d1=new DateTime();
        protected void btnDetail_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(CnnString1);
            #region Fetch Data
            DataSet ds = new DataSet();
            string strreptype = "E";
            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            cmd = new SqlCommand("Business_report_PIPVSReceivedSummary", con);


            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
            cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
            cmd.Parameters.Add(new SqlParameter("@reptype", strreptype));

            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // dt = new DataTable();
            da.Fill(dt);


            #endregion
            if (dt.Rows.Count > 0)
            {
                //DataTable dtTest = new DataTable();
                Session["dt"] = dt; 
                grdid.DataSource = dt;
                grdid.DataBind();
                grdid.Visible = true;
                day1date = dt.Rows[0]["day1date"].ToString();
                day2date = dt.Rows[0]["day2date"].ToString();
                day3date = dt.Rows[0]["day3date"].ToString();
                day4date = dt.Rows[0]["day4date"].ToString();
                day5date = dt.Rows[0]["day5date"].ToString();

              
            }

           
        }

       

        protected void grdid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
            string embcentername = string.Empty;

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            StringBuilder strSQL = new StringBuilder();
            DateTime ReportDate2 = OrderDatefrom.SelectedDate;
            string ReportDate3 = Convert.ToString(ReportDate2);
          
            string state = DropDownListStateName.SelectedValue;
            string BusinessReportStartDate = string.Empty;

            //State wise Business Report From date.
            //Field Added on State Table 
            string strBusinessStartSQL = "select businessreportstartdate from HSRPState where HSRP_StateID='" + state + "'";
            DataTable dtBusiness = Utils.GetDataTable(strBusinessStartSQL, CnnString1);

            BusinessReportStartDate = dtBusiness.Rows[0]["BusinessReportStartDate"].ToString();

            if (state == "9" || state == "11")
            {
                if (e.CommandName == "Day5orMore")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                    embcentername = embcenter.Text.ToString();
                    Label Day1 = (Label)grdid.Rows[RowIndex].FindControl("lblDay1Date");
                    day1date = Day1.Text.ToString();
                    Label Day2 = (Label)grdid.Rows[RowIndex].FindControl("lblDay2Date");
                    day2date = Day2.Text.ToString();
                    Label Day3 = (Label)grdid.Rows[RowIndex].FindControl("lblDay3Date");
                    day3date = Day3.Text.ToString();
                    Label Day4 = (Label)grdid.Rows[RowIndex].FindControl("lblDay4Date");
                    day4date = Day4.Text.ToString();

                    //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,OrderEmbossingDate,101)<=dateadd(day,-4,'" + ReportDate2 + "') and orderembossingdate>'01-jan-2015' and (convert(date,ChallanDate)>'" + ReportDate2 + "' or isnull(ChallanDate,'')='')");

                    strSQL.Append("select convert(varchar(15),erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,Convert(Varchar(15),challandate,103) as challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and isnull(VehicleRegNo,'')!='' and convert(date,aptgvehrecdate,101)<=dateadd(day,-1,'" + day4date + "') and HSRPRecord_CreationDate>'" + BusinessReportStartDate + "'  and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='') and hsrprecord_creationdate>'01-jan-2015'");

                   //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,isnull(aptgvehrecdate,hsrprecord_creationdate),101)<=dateadd(day,-1,'" + day4date + "') and HSRPRecord_CreationDate>'" + BusinessReportStartDate + "' and isnull(VehicleRegNo,'')!='' and  isnull(erpassigndate,'') !='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");

                }


                if (e.CommandName == "Day1")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                    embcentername = embcenter.Text.ToString();
                    Label Day1 = (Label)grdid.Rows[RowIndex].FindControl("lblDay1Date");
                    day1date = Day1.Text.ToString();
                    //APTG//
                    //APTG//  
                    strSQL.Append("select convert(varchar(15),erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,Convert(Varchar(15),challandate,103) as challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,aptgvehrecdate,101) between  dateadd(day,0,'" + day1date + "' ) and '" + ReportDate2 + "' and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='') and hsrprecord_creationdate>'01-jan-2015'");

                    //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,hsrprecord_creationdate,101) between  dateadd(day,0,'" + day1date + "' ) and '" + ReportDate2 + "' and isnull(VehicleRegNo,'')!='' and  isnull(erpassigndate,'') !='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
                    //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcenter--------------name,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,isnull(aptgvehrecdate,hsrprecord_creationdate),101) between  dateadd(day,0,'" + day1date + "' ) and dateadd(day,1,'" + ReportDate2 + "' ) and isnull(VehicleRegNo,'')!='' and  isnull(erpassigndate,'') !='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
                    //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "'  and convert(date,isnull(aptgvehrecdate,hsrprecord_creationdate),101) between '" + day1date + "' and '" + ReportDate2 + "' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
                }
                if (e.CommandName == "Day2")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                    embcentername = embcenter.Text.ToString();
                    Label Day1 = (Label)grdid.Rows[RowIndex].FindControl("lblDay1Date");
                    day1date = Day1.Text.ToString();
                    Label Day2 = (Label)grdid.Rows[RowIndex].FindControl("lblDay2Date");
                    day2date = Day2.Text.ToString();
                    //For APTG//
                    strSQL.Append("select convert(varchar(15),erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,Convert(Varchar(15),challandate,103) as challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,aptgvehrecdate,101) between  '" + day2date + "' and dateadd(day,-1,'" + day1date + "') and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='') and hsrprecord_creationdate>'01-jan-2015'");
                    //HR
                    //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,hsrprecord_creationdate,101) between  '" + day2date + "' and dateadd(day,-1,'" + day1date + "') and isnull(VehicleRegNo,'')!='' and  isnull(erpassigndate,'') !='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
                }
                if (e.CommandName == "Day3")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                    embcentername = embcenter.Text.ToString();
                    Label Day1 = (Label)grdid.Rows[RowIndex].FindControl("lblDay1Date");
                    day1date = Day1.Text.ToString();
                    Label Day2 = (Label)grdid.Rows[RowIndex].FindControl("lblDay2Date");
                    day2date = Day2.Text.ToString();
                    Label Day3 = (Label)grdid.Rows[RowIndex].FindControl("lblDay3Date");
                    day3date = Day3.Text.ToString();

                    //aptg
                    strSQL.Append("select convert(varchar(15),erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,Convert(Varchar(15),challandate,103) as challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,aptgvehrecdate,101) between  '" + day3date + "' and  dateadd(day,-1,'" + day2date + "') and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='') and hsrprecord_creationdate>'01-jan-2015'");
                    //hr
                    //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,hsrprecord_creationdate,101) between  '" + day3date + "' and  dateadd(day,-1,'" + day2date + "') and isnull(VehicleRegNo,'')!='' and  isnull(erpassigndate,'') !='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
                }
                if (e.CommandName == "Day4")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                    embcentername = embcenter.Text.ToString();
                    Label Day1 = (Label)grdid.Rows[RowIndex].FindControl("lblDay1Date");
                    day1date = Day1.Text.ToString();
                    Label Day2 = (Label)grdid.Rows[RowIndex].FindControl("lblDay2Date");
                    day2date = Day2.Text.ToString();
                    Label Day3 = (Label)grdid.Rows[RowIndex].FindControl("lblDay3Date");
                    day3date = Day3.Text.ToString();
                    Label Day4 = (Label)grdid.Rows[RowIndex].FindControl("lblDay4Date");
                    day4date = Day4.Text.ToString();
                    //aptg
                    strSQL.Append("select convert(varchar(15),erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,Convert(Varchar(15),challandate,103) as challandate ,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,aptgvehrecdate,101) between  '" + day4date + "' and dateadd(day,-1,'" + day3date + "') and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='') and hsrprecord_creationdate>'01-jan-2015'");
                    //hr
                   // strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,hsrprecord_creationdate,101) between  '" + day4date + "' and dateadd(day,-1,'" + day3date + "') and isnull(VehicleRegNo,'')!='' and  isnull(erpassigndate,'') !=''  and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");


                }
            }
            else
            {
                if (e.CommandName == "Day5orMore")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                    embcentername = embcenter.Text.ToString();

                    Label Day1 = (Label)grdid.Rows[RowIndex].FindControl("lblDay1Date");
                    day1date = Day1.Text.ToString();
                    Label Day2 = (Label)grdid.Rows[RowIndex].FindControl("lblDay2Date");
                    day2date = Day2.Text.ToString();
                    Label Day3 = (Label)grdid.Rows[RowIndex].FindControl("lblDay3Date");
                    day3date = Day3.Text.ToString();
                    Label Day4 = (Label)grdid.Rows[RowIndex].FindControl("lblDay4Date");
                    day4date = Day4.Text.ToString();

                    //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,OrderEmbossingDate,101)<=dateadd(day,-4,'" + ReportDate2 + "') and orderembossingdate>'01-jan-2015' and (convert(date,ChallanDate)>'" + ReportDate2 + "' or isnull(ChallanDate,'')='')");

                    strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and isnull(VehicleRegNo,'')!=''  and convert(date,hsrprecord_creationdate,101)<=dateadd(day,-1,'" + day4date + "') and HSRPRecord_CreationDate>'" + BusinessReportStartDate + "'  and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");

                    //aptg//strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,isnull(aptgvehrecdate,hsrprecord_creationdate),101)<=dateadd(day,-1,'" + day4date + "') and HSRPRecord_CreationDate>'" + BusinessReportStartDate + "' and isnull(VehicleRegNo,'')!='' and  isnull(erpassigndate,'') !='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");

                }


                if (e.CommandName == "Day1")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                    embcentername = embcenter.Text.ToString();
                    Label Day1 = (Label)grdid.Rows[RowIndex].FindControl("lblDay1Date");
                    day1date = Day1.Text.ToString();
                   
                    //APTG//
                    //APTG//strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,aptgvehrecdate,101) between  dateadd(day,0,'" + day1date + "' ) and '" + ReportDate2 + "' and isnull(VehicleRegNo,'')!='' and  isnull(erpassigndate,'') !='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");

                    strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,hsrprecord_creationdate,101) between  dateadd(day,0,'" + day1date + "' ) and '" + ReportDate2 + "' and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");

                    //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,isnull(aptgvehrecdate,hsrprecord_creationdate),101) between  dateadd(day,0,'" + day1date + "' ) and dateadd(day,1,'" + ReportDate2 + "' ) and isnull(VehicleRegNo,'')!='' and  isnull(erpassigndate,'') !='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
                    //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "'  and convert(date,isnull(aptgvehrecdate,hsrprecord_creationdate),101) between '" + day1date + "' and '" + ReportDate2 + "' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
                }
                if (e.CommandName == "Day2")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                    embcentername = embcenter.Text.ToString();
                    Label Day1 = (Label)grdid.Rows[RowIndex].FindControl("lblDay1Date");
                    day1date = Day1.Text.ToString();
                    Label Day2 = (Label)grdid.Rows[RowIndex].FindControl("lblDay2Date");
                    day2date = Day2.Text.ToString();


                    //For APTG//
                    //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,aptgvehrecdate,101) between  '" + day2date + "' and dateadd(day,-1,'" + day1date + "') and isnull(VehicleRegNo,'')!='' and  isnull(erpassigndate,'') !='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
                    //HR
                    strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,hsrprecord_creationdate,101) between  '" + day2date + "' and dateadd(day,-1,'" + day1date + "') and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2+ "' or isnull(RecievedAtAffixationDateTime,'')='')");
                }
                if (e.CommandName == "Day3")
                {
                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                    embcentername = embcenter.Text.ToString();
                    Label Day1 = (Label)grdid.Rows[RowIndex].FindControl("lblDay1Date");
                    day1date = Day1.Text.ToString();
                    Label Day2 = (Label)grdid.Rows[RowIndex].FindControl("lblDay2Date");
                    day2date = Day2.Text.ToString();
                    Label Day3 = (Label)grdid.Rows[RowIndex].FindControl("lblDay3Date");
                    day3date = Day3.Text.ToString();

                    //aptg
                    //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,aptgvehrecdate,101) between  '" + day3date + "' and  dateadd(day,-1,'" + day2date + "') and isnull(VehicleRegNo,'')!='' and  isnull(erpassigndate,'') !='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
                    //hr
                    strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,hsrprecord_creationdate,101) between  '" + day3date + "' and  dateadd(day,-1,'" + day2date + "') and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
                }
                if (e.CommandName == "Day4")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                    embcentername = embcenter.Text.ToString();
                    Label Day1 = (Label)grdid.Rows[RowIndex].FindControl("lblDay1Date");
                    day1date = Day1.Text.ToString();
                    Label Day2 = (Label)grdid.Rows[RowIndex].FindControl("lblDay2Date");
                    day2date = Day2.Text.ToString();
                    Label Day3 = (Label)grdid.Rows[RowIndex].FindControl("lblDay3Date");
                    day3date = Day3.Text.ToString();
                    Label Day4 = (Label)grdid.Rows[RowIndex].FindControl("lblDay4Date");
                    day4date = Day4.Text.ToString();
                    //aptg
                    //strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,aptgvehrecdate,101) between  '" + day4date + "' and dateadd(day,-1,'" + day3date + "') and isnull(VehicleRegNo,'')!='' and  isnull(erpassigndate,'') !=''  and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
                    //hr
                    strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,hsrprecord_creationdate,101) between  '" + day4date + "' and dateadd(day,-1,'" + day3date + "') and isnull(VehicleRegNo,'')!=''  and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");


                }



            }
                string strVehicleType = string.Empty;
                
                
                DataTable dt = Utils.GetDataTable(strSQL.ToString(), CnnString1);
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



                    //Worksheet sheet11 = book.Worksheets.Add("Report");
                    //sheet11.Table.Columns.Add(new WorksheetColumn(60));
                    //sheet11.Table.Columns.Add(new WorksheetColumn(205));
                    //sheet11.Table.Columns.Add(new WorksheetColumn(100));
                    //sheet11.Table.Columns.Add(new WorksheetColumn(130));

                    //sheet11.Table.Columns.Add(new WorksheetColumn(100));
                    //sheet11.Table.Columns.Add(new WorksheetColumn(120));
                    //sheet11.Table.Columns.Add(new WorksheetColumn(112));
                    //sheet11.Table.Columns.Add(new WorksheetColumn(109));
                    //sheet11.Table.Columns.Add(new WorksheetColumn(105));
                    //sheet11.Table.Columns.Add(new WorksheetColumn(160));


                    Worksheet sheet = book.Worksheets.Add("Export Report");
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


                    //row.Index = 2;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    //row.Cells.Add(new WorksheetCell("Rport", "HeaderStyle3"));

                    //row = sheet.Table.Rows.Add();

                    row.Index = 2;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                    WorksheetCell cell = row.Cells.Add("Export Report Details");
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
                    row.Index = 5;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(OrderDatefrom.SelectedDate.ToString("dd/MM/yyyy"), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 6;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "HeaderStyle2"));
                    //row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();



                    row.Index = 7;
                    row.Cells.Add(new WorksheetCell("S.No", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("erpassigndate", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("embcentername", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("rtolocationname", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("vehicleregno", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("hsrp_front_lasercode", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("hsrp_rear_lasercode ", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Order Status", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Challan No", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Challan Date", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("RejectFlag ", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Prod. Sheet No.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Prod Sheet Date", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Vehicle reg.rec date", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("RecievedAtAffixationDateTime", "HeaderStyle"));

                    row = sheet.Table.Rows.Add();
                    String StringField = String.Empty;
                    String StringAlert = String.Empty;

                    //row.Index = 9;


                    string RTOColName = string.Empty;
                    int sno = 0;
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                        {


                            sno = sno + 1;
                            row = sheet.Table.Rows.Add();
                            row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));


                            row.Cells.Add(new WorksheetCell(dtrows["erpassigndate"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["embcentername"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["rtolocationname"].ToString(), DataType.String, "HeaderStyle1"));
                            // row.Cells.Add(new WorksheetCell(dtrows["ManufacturerName"].ToString(), DataType.String, "HeaderStyle1"));

                            row.Cells.Add(new WorksheetCell(dtrows["vehicleregno"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["challanno"].ToString(), DataType.String, "HeaderStyle1"));

                            row.Cells.Add(new WorksheetCell(dtrows["challandate"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["RejectFlag"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["NewPdfRunningNo"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["PdfDownloadDate"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["aptgvehrecdate"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["RecievedAtAffixationDateTime"].ToString(), DataType.String, "HeaderStyle1"));


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
   


               // }
            
        }

        protected void grdid_SelectedIndexChanged(object sender, EventArgs e)
        {
            string embcenter11 = grdid.SelectedRow.RowIndex.ToString();
        }

        private void SaveAndDownloadFile12()
        {
            Workbook book = new Workbook();
            string filename = "PIP VS Received" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            Export12(strOrderType, book, 1, "Business_report_PIPVSReceivedSummary");

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }

       

        private void Export12(string strReportType, Workbook book, int iActiveSheet, string strProcName)
        {
            try
            {
                SqlConnection con = new SqlConnection(CnnString1);


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
                if (icount <= 0)
                {
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

                }
                #endregion

                Worksheet sheet = book.Worksheets.Add("Report");

                string strreptype = "A";
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand(strProcName, con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
                cmd.Parameters.Add(new SqlParameter("@reptype", strreptype));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //  dt = new DataTable();
                da.Fill(dt);
                #endregion

                AddColumnToSheet(sheet, 100, dt.Columns.Count);



                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                WorksheetCell cell = row.Cells.Add("PIP VS Received");
                cell.MergeAcross = 4; // Merge two cells together
                cell.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                AddNewCell(row, "State:", "HeaderStyle2", 1);
                AddNewCell(row, DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                AddNewCell(row, "Report Date:", "HeaderStyle2", 1);
                AddNewCell(row, OrderDatefrom.SelectedDate.ToString("dd/MM/yyyy"), "HeaderStyle2", 1);
                row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "HeaderStyle2"));  
                AddNewCell(row, "", "HeaderStyle2", 2);
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                //AddNewCell(row, "", "HeaderStyle6", 1);
                // WorksheetCell cell1 = row.Cells.Add("Today's Collection");
                //cell1.MergeAcross = 3; // Merge cells together
                // cell1.StyleID = "HeaderStyle6";
                //WorksheetCell cell2 = row.Cells.Add("Collection and Deposit MTD");
                //cell2.MergeAcross = 3; // Merge cells together
                //cell2.StyleID = "HeaderStyle6";
                //WorksheetCell cell3 = row.Cells.Add("Collection and Deposit Fy YTD");
                //cell3.MergeAcross = 3; // Merge cells together
                //cell3.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //if (dt.Columns[i].ColumnName.ToString().EndsWith("1") || dt.Columns[i].ColumnName.ToString().EndsWith("2"))
                    //{
                    //   // string strCol = dt.Columns[i].ColumnName.ToString();
                    //   // AddNewCell(row, strCol.Remove(strCol.Length - 1), "HeaderStyle6", 1);
                    //}
                    //else
                    //{
                    AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                    //}
                }
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                        //if (dt.Rows[j]["Location"].ToString().Equals("ZZZZZ") && i.Equals(0))
                        //{
                        //    AddNewCell(row, "Total", "HeaderStyle6", 1);
                        //}
                        //else
                        //{
                        if (dt.Rows[j]["Zone"].ToString().Trim().ToUpper().Equals("ZZ") && i.Equals(0))
                        {
                            AddNewCell(row, " ", "HeaderStyle6", 1);
                        }
                        else
                        {
                            AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);
                        }

                        //}
                    }
                    row = sheet.Table.Rows.Add();

                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void btnproctiondispatch_Click(object sender, EventArgs e)
        {
            SaveAndDownloadFile12();
        }

        protected void btnnDetails_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(CnnString1);
            #region Fetch Data
            DataSet ds = new DataSet();
            string strreptype = "A";
            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            cmd = new SqlCommand("Business_report_PIPVSReceivedSummary", con);


            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
            cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
            cmd.Parameters.Add(new SqlParameter("@reptype", strreptype));

            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // dt = new DataTable();
            da.Fill(dt);


            #endregion
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string embcenter11 = GridView1.SelectedRow.RowIndex.ToString();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string rtolocationname = string.Empty;
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            StringBuilder strSQL = new StringBuilder();
            DateTime ReportDate2 = Convert.ToDateTime(OrderDatefrom.SelectedDate.ToShortDateString());
            string ReportDate3 = Convert.ToString(ReportDate2);
            string state = DropDownListStateName.SelectedValue;
            string BusinessReportStartDate = string.Empty;
            //State wise Business Report From date.
            //Field Added on State Table 
            string strBusinessStartSQL = "select businessreportstartdate from HSRPState where HSRP_StateID='" + state + "'";
            DataTable dtBusiness = Utils.GetDataTable(strBusinessStartSQL, CnnString1);
            BusinessReportStartDate = dtBusiness.Rows[0]["BusinessReportStartDate"].ToString();

            if (state == "9" || state == "11")
            {
                if (e.CommandName == "Day5orMore")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
                    rtolocationname = rtolocation.Text.ToString();
                    Label lblDay5date = (Label)GridView1.Rows[RowIndex].FindControl("lblDay5date");
                    DateTime ReportDate = Convert.ToDateTime(Convert.ToDateTime(lblDay5date.Text).ToShortDateString());
                    strSQL.Append("select convert(varchar(15),erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,Convert(Varchar(15),PdfDownloadDate,103) as 'PdfDownloadDate',isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,aptgvehrecdate,101)<= convert(date,'" + ReportDate + "') and hsrprecord_creationdate>'" + BusinessReportStartDate + "' and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>convert(date,'" + ReportDate2 + "') or isnull(RecievedAtAffixationDateTime,'')='')");
                   
                }
                if (e.CommandName == "Day1")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
                    rtolocationname = rtolocation.Text.ToString();
                    Label lblDay1Date = (Label)GridView1.Rows[RowIndex].FindControl("lblDay1Date");
                    DateTime ReportDate = Convert.ToDateTime(Convert.ToDateTime(lblDay1Date.Text).ToShortDateString());
                    //Label lblDay2date = (Label)GridView1.Rows[RowIndex].FindControl("lblDay2date");
                    //DateTime Day2date = Convert.ToDateTime(Convert.ToDateTime(lblDay2date.Text).ToShortDateString());
                    strSQL.Append("select convert(varchar(15),erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,Convert(Varchar(15),PdfDownloadDate,103) as 'PdfDownloadDate',isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,aptgvehrecdate,101)=convert(date,'" + ReportDate + "') and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)> convert(date,'" + ReportDate2 + "') or isnull(RecievedAtAffixationDateTime,'')='')");
                }
                if (e.CommandName == "Day2")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
                    rtolocationname = rtolocation.Text.ToString();
                    Label lblDay2date = (Label)GridView1.Rows[RowIndex].FindControl("lblDay2date");
                    DateTime ReportDate = Convert.ToDateTime(Convert.ToDateTime(lblDay2date.Text).ToShortDateString());
                    Label lblDay1Date = (Label)GridView1.Rows[RowIndex].FindControl("lblDay1Date");
                    DateTime Day1Date = Convert.ToDateTime(Convert.ToDateTime(lblDay1Date.Text).ToShortDateString());
                    strSQL.Append("select convert(varchar(15),erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,Convert(Varchar(15),PdfDownloadDate,103) as 'PdfDownloadDate',isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,aptgvehrecdate,101) between convert(date,'" + ReportDate + "') and dateadd(day,-1,convert(date,'" + Day1Date + "')) and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)> convert(date,'" + ReportDate2 + "') or isnull(RecievedAtAffixationDateTime,'')='')");
                }
                if (e.CommandName == "Day3")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
                    rtolocationname = rtolocation.Text.ToString();
                    Label lblDay3Date = (Label)GridView1.Rows[RowIndex].FindControl("lblDay3Date");
                    DateTime ReportDate = Convert.ToDateTime(Convert.ToDateTime(lblDay3Date.Text).ToShortDateString());
                    Label lblDay2date = (Label)GridView1.Rows[RowIndex].FindControl("lblDay2date");
                    DateTime Day2date = Convert.ToDateTime(Convert.ToDateTime(lblDay2date.Text).ToShortDateString());
                    strSQL.Append("select convert(varchar(15),erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,Convert(Varchar(15),PdfDownloadDate,103) as 'PdfDownloadDate',isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,aptgvehrecdate,101) between convert(date,'" + ReportDate + "') and dateadd(day,-1,convert(date,'" + Day2date + "')) and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>convert(date,'" + ReportDate2 + "') or isnull(RecievedAtAffixationDateTime,'')='')");
                }
                if (e.CommandName == "Day4")
                {

                    GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    int RowIndex = oItem.RowIndex;
                    Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
                    rtolocationname = rtolocation.Text.ToString();
                    Label lblDay4Date = (Label)GridView1.Rows[RowIndex].FindControl("lblDay4Date");
                    DateTime ReportDate = Convert.ToDateTime(Convert.ToDateTime(lblDay4Date.Text).ToShortDateString());
                    Label lblDay3Date = (Label)GridView1.Rows[RowIndex].FindControl("lblDay3Date");
                    DateTime Day3Date = Convert.ToDateTime(Convert.ToDateTime(lblDay3Date.Text).ToShortDateString());
                    strSQL.Append("select convert(varchar(15),erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,Convert(Varchar(15),PdfDownloadDate,103) as 'PdfDownloadDate',isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,aptgvehrecdate,101) between convert(date,'" + ReportDate + "') and dateadd(day,-1,convert(date,'" + Day3Date + "')) and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>convert(date,'" + ReportDate2 + "') or isnull(RecievedAtAffixationDateTime,'')='')");
                }
            }
            //else 
            //{
            //    if (e.CommandName == "Day5orMore")
            //    {

            //        GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            //        int RowIndex = oItem.RowIndex;
            //        Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
            //        rtolocationname = rtolocation.Text.ToString();
            //        Label lblDay5date = (Label)GridView1.Rows[RowIndex].FindControl("lblDay5date");
            //        ReportDate2 = Convert.ToDateTime(Convert.ToDateTime(lblDay5date.Text).ToShortDateString());
            //        strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,Convert(Varchar(15),PdfDownloadDate,103) as 'PdfDownloadDate',isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,hsrprecord_creationdate,101)<='" + ReportDate2 + "' and hsrprecord_creationdate>'" + BusinessReportStartDate + "' and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");                   

            //    }
            //    if (e.CommandName == "Day1")
            //    {

            //        GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            //        int RowIndex = oItem.RowIndex;
            //        Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
            //        rtolocationname = rtolocation.Text.ToString();
            //        strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,Convert(Varchar(15),PdfDownloadDate,103) as 'PdfDownloadDate',isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,hsrprecord_creationdate,101)=dateadd(day,0,'" + ReportDate2 + "') and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
            //    }
            //    if (e.CommandName == "Day2")
            //    {

            //        GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            //        int RowIndex = oItem.RowIndex;
            //        Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
            //        rtolocationname = rtolocation.Text.ToString();
            //        strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,Convert(Varchar(15),PdfDownloadDate,103) as 'PdfDownloadDate',isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,hsrprecord_creationdate,101)=dateadd(day,-1,'" + ReportDate2 + "') and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
            //    }
            //    if (e.CommandName == "Day3")
            //    {

            //        GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            //        int RowIndex = oItem.RowIndex;
            //        Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
            //        rtolocationname = rtolocation.Text.ToString();
            //        strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,Convert(Varchar(15),PdfDownloadDate,103) as 'PdfDownloadDate',isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,hsrprecord_creationdate,101)=dateadd(day,-2,'" + ReportDate2 + "') and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
            //    }
            //    if (e.CommandName == "Day4")
            //    {
            //        GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            //        int RowIndex = oItem.RowIndex;
            //        Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
            //        rtolocationname = rtolocation.Text.ToString();
            //        strSQL.Append("select convert(date,erpassigndate,101) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,Convert(Varchar(15),PdfDownloadDate,103) as 'PdfDownloadDate',isnull(convert(varchar(15),aptgvehrecdate,101),convert(varchar(15),OrderDate,101)) aptgvehrecdate,RecievedAtAffixationDateTime from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,hsrprecord_creationdate,101)=dateadd(day,-3,'" + ReportDate2 + "') and isnull(VehicleRegNo,'')!='' and (convert(date,RecievedAtAffixationDateTime)>'" + ReportDate2 + "' or isnull(RecievedAtAffixationDateTime,'')='')");
            //    }
            //}

            string strVehicleType = string.Empty;

            DataTable dt = Utils.GetDataTable(strSQL.ToString(), CnnString1);
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



                //Worksheet sheet11 = book.Worksheets.Add("Report");
                //sheet11.Table.Columns.Add(new WorksheetColumn(60));
                //sheet11.Table.Columns.Add(new WorksheetColumn(205));
                //sheet11.Table.Columns.Add(new WorksheetColumn(100));
                //sheet11.Table.Columns.Add(new WorksheetColumn(130));

                //sheet11.Table.Columns.Add(new WorksheetColumn(100));
                //sheet11.Table.Columns.Add(new WorksheetColumn(120));
                //sheet11.Table.Columns.Add(new WorksheetColumn(112));
                //sheet11.Table.Columns.Add(new WorksheetColumn(109));
                //sheet11.Table.Columns.Add(new WorksheetColumn(105));
                //sheet11.Table.Columns.Add(new WorksheetColumn(160));


                Worksheet sheet = book.Worksheets.Add("Export Report");
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


                //row.Index = 2;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                //row.Cells.Add(new WorksheetCell("Rport", "HeaderStyle3"));

                //row = sheet.Table.Rows.Add();

                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Export Report");
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
                row.Index = 5;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(OrderDatefrom.SelectedDate.ToString("dd/MM/yyyy"), "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
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
                row.Cells.Add(new WorksheetCell("erpassigndate", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("embcentername", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("rtolocationname", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("vehicleregno", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("hsrp_front_lasercode", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("hsrp_rear_lasercode ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Order Status", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Challan No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Challan Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("RejectFlag ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Prod. Sheet No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Prod Sheet Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Vehicle reg.rec date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("RecievedAtAffixationDateTime", "HeaderStyle")); 

               
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                //row.Index = 9;


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {


                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));


                        row.Cells.Add(new WorksheetCell(dtrows["erpassigndate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["embcentername"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["rtolocationname"].ToString(), DataType.String, "HeaderStyle1"));
                        // row.Cells.Add(new WorksheetCell(dtrows["ManufacturerName"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["vehicleregno"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["challanno"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["challandate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RejectFlag"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["NewPdfRunningNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["PdfDownloadDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["aptgvehrecdate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RecievedAtAffixationDateTime"].ToString(), DataType.String, "HeaderStyle1"));

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

        
    }
}
            
       