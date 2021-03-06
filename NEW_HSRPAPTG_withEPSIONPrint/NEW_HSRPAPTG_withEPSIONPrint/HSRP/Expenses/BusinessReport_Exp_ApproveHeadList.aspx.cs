﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using iTextSharp.text;
using CarlosAg.ExcelXmlWriter;

namespace HSRP.Expenses
{
    public partial class BusinessReport_Exp_ApproveHeadList : System.Web.UI.Page
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
                            Datefrom.SelectedDate = System.DateTime.Now;
                            Dateto.SelectedDate = System.DateTime.Now;
                            InitialSetting();
                            BindReportTypeddl();
                        }
                        else
                        {
                            Datefrom.SelectedDate = System.DateTime.Now;
                            Dateto.SelectedDate = System.DateTime.Now;
                            InitialSetting();
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
            string sqlstring = "[BusinessReport_Expense_ApproveHead_list] '" + Dateto.SelectedDate + "','" + Dateto.SelectedDate + "','','Report'," + HSRP_StateID1 + " ," + strUserID1 + " ";
            dt11 = new DataTable();
            dt11 = Utils.GetDataTable(sqlstring, CnnString1);
            DdlReportType.DataSource = dt11;
            DdlReportType.DataTextField = "ReportType";
            DdlReportType.DataValueField = "Code";
            DdlReportType.DataBind();
            DdlReportType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "--Select--"));
        }


        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            Datefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            Datefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            Dateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            Dateto.MaxDate = DateTime.Parse(MaxDate);
            CalendarDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

      

        protected void btn_Click(object sender, EventArgs e)
        {

            if (DdlReportType.SelectedValue.ToString() == "--Select--")
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Select Report Type.";
                return;
            }
          
            try
            {
                lblErrMess.Text = "";
                SqlConnection con = new SqlConnection(CnnString1);
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                //change sp Name
                cmd = new SqlCommand("BusinessReport_Expense_ApproveHead_list", con);
                cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Parameters.Add(new SqlParameter("@reportdate", Convert.ToDateTime(Datefrom.SelectedDate)));
                 cmd.Parameters.Add(new SqlParameter("@reportto", Convert.ToDateTime(Dateto.SelectedDate)));
                 cmd.Parameters.Add(new SqlParameter("@searchdata", txtSearch.Text.ToString()));
                cmd.Parameters.Add(new SqlParameter("@reporttype", DdlReportType.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@stateid", Convert.ToInt32(Session["UserHSRPStateID"])));
                cmd.Parameters.Add(new SqlParameter("@userid", Convert.ToInt32(Session["UID"])));
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
                    lblErrMess.Text = "Record Not Found";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void DropDownListStateName_TextChanged(object sender, EventArgs e)
        {
            
            gridTD.Visible = false;
            grd.DataSource = null;
            grd.Visible = false;
        }
        int icount = 0;
        string filename = string.Empty;
        private void SaveAndDownloadFile()
        {
            Workbook book = new Workbook();
            filename = "Employee Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            Export(strOrderType, book, 1, "BusinessReport_Expense_ApproveHead_list");

           

        }
        
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
                book.Properties.Title = "User Master Report";
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
                    style5.Font.Bold = true;
                    style5.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                    WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                    style6.Font.FontName = "Tahoma";
                    style6.Font.Size = 10;
                    style6.Font.Bold = false;
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
                if (btndownload1 != true)
                {
                     cmd.Parameters.Add(new SqlParameter("@reportdate", Convert.ToDateTime(Datefrom.SelectedDate)));
                     cmd.Parameters.Add(new SqlParameter("@reportto", Convert.ToDateTime(Dateto.SelectedDate)));
                     cmd.Parameters.Add(new SqlParameter("@searchdata",txtSearch.Text.ToString()));
                     cmd.Parameters.Add(new SqlParameter("@reporttype", DdlReportType.SelectedValue));
                     cmd.Parameters.Add(new SqlParameter("@stateid", Convert.ToInt32(Session["UserHSRPStateID"])));
                     cmd.Parameters.Add(new SqlParameter("@userid", Convert.ToInt32(Session["UID"])));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@reportdate", Convert.ToDateTime(Datefrom.SelectedDate)));
                    cmd.Parameters.Add(new SqlParameter("@reportto", Convert.ToDateTime(Dateto.SelectedDate)));
                    cmd.Parameters.Add(new SqlParameter("@searchdata", txtSearch.Text.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@reporttype", DdlReportType.SelectedValue));
                    cmd.Parameters.Add(new SqlParameter("@stateid", Convert.ToInt32(Session["UserHSRPStateID"])));
                    cmd.Parameters.Add(new SqlParameter("@userid", Convert.ToInt32(Session["UID"])));
                }

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //  dt = new DataTable();
                da.Fill(dt);
                #endregion

                if (dt.Rows.Count > 0)
                {
                    AddColumnToSheet(sheet, 100, dt.Columns.Count);

                    int iIndex = 3;
                    WorksheetRow row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("Heads Report");
                    cell.MergeAcross = 4; // Merge two cells together
                    cell.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;                
                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    AddNewCell(row, "Report Date from:", "HeaderStyle2", 1);
                    AddNewCell(row, Datefrom.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);

                    AddNewCell(row, "Report Date To:", "HeaderStyle2", 1);
                    AddNewCell(row, Dateto.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);

                    row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                    AddNewCell(row, "", "HeaderStyle2", 2);
                    row = sheet.Table.Rows.Add();

                    row.Index = iIndex++;

                    AddNewCell(row, "", "HeaderStyle2", 1);
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle5", 1);

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
            this.SaveAndDownloadFile();
        }
        bool btndownload1 = false;
        protected void btndownloadDetail_Click(object sender, EventArgs e)
        {
           // btndownload1 = true;
           // this.SaveAndDownloadFile2();
        }

        protected void DdlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlReportType.SelectedItem.ToString() == "Search User")
            {
                txtSearch.Visible = true;
                label2.Visible = true;
            }
            else
            {
                txtSearch.Visible = false;
                label2.Visible = false;
            }
           
        }


        

    }
}
