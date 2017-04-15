using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using HSRP.TGWebrefrence;
using System.Net;
using CarlosAg.ExcelXmlWriter;

namespace HSRP.Transaction
{
    public partial class TGOnlineRecho : System.Web.UI.Page
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ToString();
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string ProductivityID = string.Empty;
        string UserType = string.Empty;
        string UserName = string.Empty;
        string Sticker = string.Empty;
        string VIP = string.Empty;
        string USERID = string.Empty;
        string macbase = string.Empty;
        DataTable dt;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserType"].ToString() == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    UserType = Session["UserType"].ToString();

                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserName = Session["UID"].ToString();
            USERID = Session["UID"].ToString();
            macbase = Session["MacAddress"].ToString();
            if (!IsPostBack)
            {
               
                
            }
            else
            {

            }
        }

        
        protected void btnStepOne_Click(object sender, EventArgs e)
        {
            lblErrMsg.Text = "";
            try
            {
                dt = new DataTable();
                dt = Utils.GetDataTable("Business_ReportOnlineAnalysis_report 'S1','" + USERID + "'", ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    Label1.Text = dt.Rows[0]["result"].ToString();
                }
            }
            catch (Exception Ex)
            {

                lblErrMsg.Text = Ex.Message.ToString();
            }
        }

        int icount = 0;

        private void SaveAndDownloadFile(DataTable dt,string Fname,string Title)
        {
            Workbook book = new Workbook();
            string filename = Fname + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            Export(dt, book, Title);

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }

        private void Export(DataTable dt, Workbook book,string Title)
        {
            try
            {
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = Title;
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
                if (dt.Rows.Count > 0)
                {
                    AddColumnToSheet(sheet, 100, dt.Columns.Count);
                    int iIndex = 3;
                    WorksheetRow row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add(Title);
                    cell.MergeAcross = 4; // Merge two cells together
                    cell.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    //AddNewCell(row, "State:", "HeaderStyle2", 1);
                    //AddNewCell(row, DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2", 1);
                    //AddNewCell(row, "Report Type:", "HeaderStyle2", 1);
                    //AddNewCell(row, DropDownListReportType.SelectedItem.ToString(), "HeaderStyle2", 1);
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());


                    AddNewCell(row, "Report Date :", "HeaderStyle2", 1);
                    AddNewCell(row, System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);

                    AddNewCell(row, "", "HeaderStyle2", 2);
                    row = sheet.Table.Rows.Add();

                    row.Index = iIndex++;
                    AddNewCell(row, "", "HeaderStyle6", 1);
                    row = sheet.Table.Rows.Add();

                    row.Index = iIndex++;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                    }
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    row.Index = iIndex++;
                    AddNewCell(row, "", "HeaderStyle", 1);
                    row = sheet.Table.Rows.Add();

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle", 1);
                        }
                        row = sheet.Table.Rows.Add();
                    }
                }
                else
                {
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

        protected void BtnGetdupliateInUploaaded_Click(object sender, EventArgs e)
        {
            try
            {
                dt = new DataTable();
                dt = Utils.GetDataTable("Business_ReportOnlineAnalysis_report 'U1','" + USERID + "'", ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    SaveAndDownloadFile(dt, "RocoDupliateRecords", "TG Dupliate Reords");
                }
            }
            catch (Exception Ex)
            {

                lblErrMsg.Text = Ex.Message.ToString();
            }
        }

        protected void BtnGetduplicateInpayment_Click(object sender, EventArgs e)
        {
            try
            {
                dt = new DataTable();
                dt = Utils.GetDataTable("Business_ReportOnlineAnalysis_report 'P1','" + USERID + "'", ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    SaveAndDownloadFile(dt, "RocoDupliateRecords", "TG Dupliate Reords");
                }
            }
            catch (Exception Ex)
            {

                lblErrMsg.Text = Ex.Message.ToString();
            }
        }

        protected void BtnGetduplicateInOrder_Click(object sender, EventArgs e)
        {
            try
            {
                dt = new DataTable();
                dt = Utils.GetDataTable("Business_ReportOnlineAnalysis_report 'O1','" + USERID + "'", ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    SaveAndDownloadFile(dt, "RocoDupliateRecords", "TG Dupliate Reords");
                }
            }
            catch (Exception Ex)
            {

                lblErrMsg.Text = Ex.Message.ToString();
            }
        }
    
    }
}