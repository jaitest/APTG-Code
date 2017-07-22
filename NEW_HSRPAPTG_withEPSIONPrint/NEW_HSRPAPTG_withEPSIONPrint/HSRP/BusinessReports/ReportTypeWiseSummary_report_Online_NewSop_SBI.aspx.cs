using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using iTextSharp.text;
using CarlosAg.ExcelXmlWriter;

namespace HSRP.BusinessReports
{
    public partial class ReportTypeWiseSummary_report_Online_NewSop_SBI : System.Web.UI.Page
    {

        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        string SQLString1 = string.Empty;

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

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
                    gridTD.Visible = false;
                    grd.Visible = false;
                    try
                    {
                        if (UserType1.Equals(0))
                        {
                            
                            //InitialSetting();
                            BindReportTypeddl();
                        }
                        else
                        {
                            
                           // InitialSetting();
                            BindReportTypeddl();
                        }
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

      

        public static DataTable dt11;

        private void BindReportTypeddl()
        {
            string sqlstring = "[Business_ReportTypewisesummary_report_AP_NewSOP_SBI] 'DB'";
            dt11 = new DataTable();
            dt11 = Utils.GetDataTable(sqlstring, CnnString1);
            DdlReportType.DataSource = dt11;
            DdlReportType.DataTextField = "ReportType";
            DdlReportType.DataValueField = "Code";
            DdlReportType.DataBind();
            DdlReportType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "--Select--"));
        }


        //private void InitialSetting()
        //{
        //    string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        //    string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

        //    Datefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    Datefrom.MaxDate = DateTime.Parse(MaxDate);
        //    CalendarDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    CalendarDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        //    Dateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    Dateto.MaxDate = DateTime.Parse(MaxDate);
        //    CalendarDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    CalendarDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //}
        
        protected void btn_Click(object sender, EventArgs e)
        {

            try
            {
                lblErrMess.Text = "";
                SqlConnection con = new SqlConnection(CnnString1);
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                //change sp Name
                cmd = new SqlCommand("Business_ReportTypewisesummary_report_AP_NewSOP_SBI", con);//Business_ReportTypewisesummary_report
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@reportdate", Convert.ToDateTime(Datefrom.SelectedDate.ToString("yyyy/MM/dd"))));
                //cmd.Parameters.Add(new SqlParameter("@reportto", Convert.ToDateTime(Dateto.SelectedDate.ToString("yyyy/MM/dd"))));
                cmd.Parameters.Add(new SqlParameter("@reporttype", DdlReportType.SelectedValue.ToString()));
               

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                #endregion
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gridTD.Visible = true;
                    grd.DataSource = ds.Tables[0];
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
        }

        protected void DropDownListStateName_TextChanged(object sender, EventArgs e)
        {
            //FilldropddlRTOLocation(DropDownListStateName.SelectedValue);
            gridTD.Visible = false;
            grd.DataSource = null;
            grd.Visible = false;
        }


        private void Export1(string strReportType, Workbook book, int iActiveSheet, string strProcName)
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
                book.Properties.Title = "AP New SOP Summary";
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

                //Worksheet sheet = book.Worksheets.Add("Summary");
                Worksheet sheet = book.Worksheets.Add(DdlReportType.SelectedItem.ToString().Trim());

                string strreptype = "E";
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand(strProcName, con);


                cmd.CommandType = CommandType.StoredProcedure;
                if (btndownload1 != true)
                {
                   // cmd.Parameters.Add(new SqlParameter("@reportdate", Convert.ToDateTime(Datefrom.SelectedDate.ToString("yyyy/MM/dd"))));
                    //cmd.Parameters.Add(new SqlParameter("@reportto", Convert.ToDateTime(Dateto.SelectedDate.ToString("yyyy/MM/dd"))));
                    cmd.Parameters.Add(new SqlParameter("@reporttype", DdlReportType.SelectedValue.ToString()));                  
                }
                else
                {
                   // cmd.Parameters.Add(new SqlParameter("@reportdate", Convert.ToDateTime(Datefrom.SelectedDate.ToString("yyyy/MM/dd"))));
                   // cmd.Parameters.Add(new SqlParameter("@reportto", Convert.ToDateTime(Dateto.SelectedDate.ToString("yyyy/MM/dd"))));
                    cmd.Parameters.Add(new SqlParameter("@reporttype", DdlReportType.SelectedValue.ToString()));       
                }

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //  dt = new DataTable();
                da.Fill(dt);
                #endregion

                if (dt.Rows.Count > 0)
                {
                    AddColumnToSheet(sheet, 100, dt.Columns.Count);
                    int iIndex = 1;
                    WorksheetRow row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("AP New Sop Data Summary");
                    cell.MergeAcross = 2; // Merge two cells together
                    cell.StyleID = "HeaderStyle2";
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;                    

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    row.Cells.Add(new WorksheetCell("AP New SOP Data Summary Dated:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                    AddNewCell(row, "", "HeaderStyle2", 2);
                    row = sheet.Table.Rows.Add();


                    row.Index = iIndex++;


                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                    }
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);
                        }
                        row = sheet.Table.Rows.Add();

                    }
                }
                else
                {
                    Label1.Visible = false;
                    Label1.Text = "Record not Found";
                    return;
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }


        int icount = 0;
        private void SaveAndDownloadFile_old()
        {
            Workbook book = new Workbook();
            string filename = "Summary report on Type Wise" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;
           
            Export1(strOrderType, book, 1, "Business_ReportTypewisesummary_report_AP_NewSOP_SBI");                      

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }
        private void SaveAndDownloadFile2()
        {
            Workbook book = new Workbook();
            string filename = "APTG Report Online Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

           // Export(strOrderType, book, 1, "Business_ReportTypewisesummary_report_AP_NewSOP_SBI");

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }


        private void SaveAndDownloadFile()
        {
            Workbook book = new Workbook();
            string filename = "Summary report on Type Wise" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

           // Export(strOrderType, book, 1, "Business_Report_APNewSOPReport");

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }


        //public void ExportToExcel()
        private void ExportToExcel(string strReportType, Workbook book, int iActiveSheet, string strProcName)
        {
            try
            {
                lblErrMess.Text = "";

              
                DataTable dt = new DataTable();


                string filename = "AP New SOP Report-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
              //  Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "AP New SOP Report";
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


                WorksheetStyle style7 = book.Styles.Add("HeaderStyle7");
                style7.Font.FontName = "Tahoma";
                style7.Font.Size = 10;
                style7.Font.Bold = true;
                style7.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style7.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style7.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style7.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style7.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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

                Worksheet sheet = book.Worksheets.Add("AP New SOP Report");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(195));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(90));

                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(140));
                sheet.Table.Columns.Add(new WorksheetColumn(162));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));



                WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                style9.Font.FontName = "Tahoma";
                style9.Font.Size = 10;
                style9.Font.Bold = true;
                style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style9.Interior.Color = "#FCF6AE";
                style9.Interior.Pattern = StyleInteriorPattern.Solid;


                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = 2;
                row.Cells.Add(new WorksheetCell("   Date :-" + System.DateTime.Now.ToString("dd/MM/yyyy"), "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("AP New SOP Report");


                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";
                row = sheet.Table.Rows.Add();
                // Skip one row, and add some text
                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());             

                SqlConnection con = new SqlConnection(CnnString1);



                string strreptype = "E";
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand(strProcName, con);


                cmd.CommandType = CommandType.StoredProcedure;
                if (btndownload1 != true)
                {
                  ////  cmd.Parameters.Add(new SqlParameter("@reportdate", Convert.ToDateTime(Datefrom.SelectedDate.ToString("yyyy/MM/dd"))));
                   // cmd.Parameters.Add(new SqlParameter("@reportto", Convert.ToDateTime(Dateto.SelectedDate.ToString("yyyy/MM/dd"))));
                    cmd.Parameters.Add(new SqlParameter("@reporttype", DdlReportType.SelectedValue.ToString()));
                }
                else
                {
                   // cmd.Parameters.Add(new SqlParameter("@reportdate", Convert.ToDateTime(Datefrom.SelectedDate.ToString("yyyy/MM/dd"))));
                   // cmd.Parameters.Add(new SqlParameter("@reportto", Convert.ToDateTime(Dateto.SelectedDate.ToString("yyyy/MM/dd"))));
                    cmd.Parameters.Add(new SqlParameter("@reporttype", DdlReportType.SelectedValue.ToString()));
                }

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //  dt = new DataTable();
                da.Fill(dt);


                if (dt.Rows.Count > 0)
                {
                    row.Index = 4;
                    row.Cells.Add(new WorksheetCell(" S.No:", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("AP New SOP Data Summary:" + dates.ToString("dd/MM/yyyy"), "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("No. of Vehicles", "HeaderStyle3"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 5;
                    row.Cells.Add(new WorksheetCell("1", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Total Transaction Received from SBI ", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["TotalTransaction"].ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 6;
                    row.Cells.Add(new WorksheetCell(" 2", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Duplicate Received", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["Duplicate"].ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 7;
                    row.Cells.Add(new WorksheetCell("3", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Total Unique Transaction Received from SBI ", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["UniqueTransaction"].ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 8;
                    row.Cells.Add(new WorksheetCell("4", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("TR Received", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["TRReceived"].ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 9;
                    row.Cells.Add(new WorksheetCell("5", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("PR Received", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["PRReceived"].ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 10;
                    row.Cells.Add(new WorksheetCell("6", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Order Booked With PR", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["BookedWithPR"].ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 11;
                    row.Cells.Add(new WorksheetCell("7", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Order Booked Without PR", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["BookedWithoutPR"].ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 12;
                    row.Cells.Add(new WorksheetCell("8", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Order Not Booked ", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["OrdernotBooked"].ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 13;
                    row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Reasons of Order Not Booking", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("  ", "HeaderStyle3"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 14;
                    row.Cells.Add(new WorksheetCell("1", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Wrong Payment Received", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["WrongPaymentReceived"].ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 15;
                    row.Cells.Add(new WorksheetCell("2", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Payment Not Received", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["PaymentNotReceived"].ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 16;
                    row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Data yet to receive", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("  ", "HeaderStyle3"));
                    row = sheet.Table.Rows.Add();


                    row.Index = 17;
                    row.Cells.Add(new WorksheetCell("1", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("TR & PR data yet to receive", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["TRPRYetToReceived"].ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 18;
                    row.Cells.Add(new WorksheetCell("2", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Only PR data yet to receive", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["OnlyPRYetToTeceive"].ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                }



                row = sheet.Table.Rows.Add();
                //HttpContext context = HttpContext.Current;
                //context.Response.Clear();
                //// Save the file and open it
                //book.Save(Response.OutputStream);

                ////context.Response.ContentType = "text/csv";
                //context.Response.ContentType = "application/vnd.ms-excel";

                //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                //context.Response.End();

            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message.ToString();
                return;
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

        protected void btn_download_Click(object sender, EventArgs e)
        {           
            //ExportToExcel();
            //this.SaveAndDownloadFile();
        }
        bool btndownload1 = false;
        protected void btndownloadDetail_Click(object sender, EventArgs e)
        {
            btndownload1 = true;
            this.SaveAndDownloadFile2();
        }

        protected void DdlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }


        private String MakeQueryOnTheBasisOfSelectedOption(String StringOrderDate, String StringAuthDate, String RtoCode)
        {

            string SQLString = "[Business_ReportTypewisesummary_report_AP_NewSOP_SBI] '" + StringOrderDate + "','" + StringAuthDate + "','" + RtoCode.ToString() + "'";
           
             return SQLString;
        }


                
        protected void btntypewise_Click(object sender, EventArgs e)
        {
            this.SaveAndDownloadFile_old();
          
        }

        

    }
}

                #endregion