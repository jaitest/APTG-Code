using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using CarlosAg.ExcelXmlWriter;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;
using SendMail;

namespace TenderReports
{
    public partial class frmTenderReports : Form
    {

        public frmTenderReports()
        {
            InitializeComponent();
           // Form1 m = new Form1();
            //  m.Show();
        }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["hsrpAPConfiguration"].ConnectionString);
        string strPath = string.Empty;
        string strMonth = string.Empty;
        string strYear = string.Empty;

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;

        private void button1_Click(object sender, EventArgs e)
        {
            //Form1 m = new Form1();
          //  m.Show();
            strMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dtpFrmDate.Value.Month);
            strYear = dtpFrmDate.Value.Year.ToString();
            string strStateId = cmbState.SelectedValue.ToString();
            switch (strStateId)
            {
                //case "5":
                //    GenerateMPCashCollection();
                //    GenerateSchedule_CE_MP();
                //    break;GenerateDLAuthorityReport()

                case "1":
                    GenerateBRAuthorityReport(); 
                    break;

                case "6":
                //    //GenerateUKCashCollection();
                   GenerateUKAuthorityReport();
                    break;

             //   case "9":
                  //  GenerateRTOWiseReport();
                   // GenerateCollectionReport();
                  //  GenerateAPEmbossingReport();
                //   GenerateAPEmbossingReportNew();
                //    break;
               // case "2":

                    //GenerateTaxRoyaltyReport();
                  //  GenerateAuthorityReport();
                   // GenerateCollectionReport();
                   // GenerateAffixationReport();
               //     break;
                case "4":

                    GenerateHRAuthorityReport();
                    // GenerateHREmbossingReport();
                    // GenerateHRMonthlyReport();
                      //GenerateRTOWiseReport();
                    break;
                case "3":
                    GenerateHPAuthorityReport();
                //    // GenerateCollectionReport();
                // //    GenerateAffixationReport();
                    break;
            }         
        }

        

        #region Initial Setting

        private void GetState()
        {
            string strState = "SELECT distinct [HSRP_StateID],[HSRPStateName] from  [dbo].[HSRPState] order by [HSRPStateName]";
           // string strState = "SELECT distinct [HSRP_StateID],[HSRPStateName] from  [dbo].[HSRPState] where HSRP_StateID='5' ";
            DataTable dt = GetDataInDT(strState);
            cmbState.DisplayMember = "HSRPStateName";
            cmbState.ValueMember = "HSRP_StateID";
            cmbState.DataSource = dt;
        }

        #region Pdf Function
        private static void PDFRows(BaseFont bfTimes, PdfPTable table1, int iColSpan, String strText, int iFontsize, int ialignMent, string strRowType, int iBorderWidthLeft, int iBorderWidthRight, int iBorderWidthTop, int iBorderWidthBottom, int optionalHeight = 0, int optionalWidth = 0)
        {
            PdfPCell cell;
            if (strRowType == "B")
            {
                cell = new PdfPCell(new iTextSharp.text.Phrase(strText, new iTextSharp.text.Font(bfTimes, iFontsize, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else
            {
                cell = new PdfPCell(new iTextSharp.text.Phrase(strText, new iTextSharp.text.Font(bfTimes, iFontsize, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            cell.Colspan = iColSpan;
            cell.BorderColor = iTextSharp.text.BaseColor.BLACK;
            cell.BorderWidthLeft = iBorderWidthLeft;
            cell.BorderWidthRight = iBorderWidthRight;
            cell.BorderWidthTop = iBorderWidthTop;
            cell.BorderWidthBottom = iBorderWidthBottom;
            cell.NoWrap = false;
            cell.HorizontalAlignment = ialignMent; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);
        }
        #endregion

        private void GenerateRTOWiseReport()
        {
            DataTable dt = GetRtoLocation();
            for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
            {
                string strRTOId = dt.Rows[iCnt]["rtolocationid"].ToString();
                string strRTOName = dt.Rows[iCnt]["rtolocationname"].ToString();
                switch (cmbState.SelectedValue.ToString())
                {
                    case "4":
                        //GenerateHRSheduleEReport(strRTOId, strRTOName);
                        GenerateHRSheduleEReport();
                        break;
                    case "9":
                        GenerateAPAffixingReport(strRTOId, strRTOName);
                      //  GenerateAPAffixingReportNew(strRTOId, strRTOName);
                        GenerateRegisteringAuthorityReport(strRTOId, strRTOName);
                      //  GenerateAPEmbossingReport(strRTOId, strRTOName);
                      //  GenerateAPStockReport(strRTOId, strRTOName);
                        break;
                }
            }
        }

        private DataTable GetRtoLocation()
        {
            DataTable dtrto = GetDataInDT("select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid='" + cmbState.SelectedValue + "') and RTOLocationID not in (148,331)");
            return dtrto;
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

        private DataTable GetDataInDT(string strSqlString)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter();
            //   con.Open();
            adp.SelectCommand = new SqlCommand(strSqlString, con);
            adp.SelectCommand.CommandTimeout = 0;
            adp.Fill(dt);
            //  con.Close();
            return dt;
        }

        #region Timer Set 

         //frmTenderReports.ActiveForm.Refresh();
        private void frmTenderReports_Load(object sender, EventArgs e)
        {
           
            timer1.Start();
            timer1.Interval = 1000;
            GetState();

           



        }
        #endregion

        private string  SetFolder(string strRTO, string strState, string strFile)
        {
            string DateFolder = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString();
            strPath = "D:\\TenderReports10";
            if (!Directory.Exists(strPath))
            {
                CreateFolder(DateFolder, strState, strPath);
                Directory.CreateDirectory(strPath + "\\" + strState);
                Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);
            }
            else
            {
                if (!Directory.Exists(strPath + "\\" + strState))
                {

                    Directory.CreateDirectory(strPath + "\\" + strState);
                    Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);

                }
                else
                {
                    if (!Directory.Exists(strPath + "\\" + strState + "\\" + DateFolder))
                    {

                        Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);

                    }
                    else
                    {
                        var files = Directory.GetFiles(strPath + "\\" + strState + "\\" + DateFolder, "*.*", SearchOption.AllDirectories);



                        //foreach (string file in files)
                        //{
                        //    if (file.StartsWith(strPath + "\\" + strState + "\\" + DateFolder + "\\" + strFile))
                        //    {
                        //        File.Delete(file);
                        //    }
                        //}
                    }
                }


            }
            return strPath = strPath + "\\" + strState + "\\" + DateFolder;
        }

        private static void CreateFolder(string strRTO, string strState, string strRTOLocFolderPath)
        {
            Directory.CreateDirectory(strRTOLocFolderPath);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState + "\\" + strRTO);
        }

        public DateTime FirstDayOfMonthFromDateTime()
        {
            return new DateTime(dtpFrmDate.Value.Year, dtpFrmDate.Value.Month, 1);
        }

        public DateTime LastDayOfMonthFromDateTime()
        {
            DateTime firstDayOfTheMonth = new DateTime(dtpFrmDate.Value.Year, dtpFrmDate.Value.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }
        #endregion

        #region Delhi
        private void GenerateTaxRoyaltyReport()
        {
            string RTOName = string.Empty;
            // string RTOName=string.Empty;

            try
            {

                DataTable dtrto = GetRtoLocation();
                for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
                {

                    RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                    string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                    SetFolder(RTOName, cmbState.Text, "Royalty and Tax-");
                    string filename = "Royalty and Tax-" + RTOName + "-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();

                    #region Excel
                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "HSRP Daily Cash Collection";
                    book.Properties.Created = DateTime.Now;


                    // Add some styles to the Workbook
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

                    Worksheet sheet = book.Worksheets.Add("HSRP Daily Cash Collection And Royalty");
                    sheet.Table.Columns.Add(new WorksheetColumn(60));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));

                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(92));
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
                    #endregion

                    WorksheetRow row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                    WorksheetCell cell = row.Cells.Add("HSRP Daily Cash Collection With Tax and Royalty");
                    cell.MergeAcross = 3; // Merge two cells together
                    cell.StyleID = "HeaderStyle3";

                    row = sheet.Table.Rows.Add();
                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(cmbState.Text, "HeaderStyle2"));

                    //row = sheet.Table.Rows.Add();
                    //row.Index = 4;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(RTOName, "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Vehicle Reference:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Both", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Vehicle Type:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("All", "HeaderStyle2"));

                    row = sheet.Table.Rows.Add();
                    row.Index = 4;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Duration:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dtpFrmDate.Value.ToString("dd/MMM/yyyy") + "-" + dtpToDate.Value.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();


                    row.Index = 5;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                    //  row.Cells.Add(new WorksheetCell("Authorisation No", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Cash Receipt No", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Authorisation Date", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle6"));
                    //row.Cells.Add(new WorksheetCell("Close Date", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Vehicle Reg. No", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Owner's Name", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Vechicle Type", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle6"));
                    if (cmbState.SelectedItem.ToString() == "BIHAR")
                    {
                        row.Cells.Add(new WorksheetCell("EngineNo", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("ChassisNo", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("MobileNo", "HeaderStyle6"));
                    }
                    row.Cells.Add(new WorksheetCell("Amount Charged", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("VAT", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Excise Duty", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Royalty Due", "HeaderStyle6"));

                    //row.Cells.Add(new WorksheetCell("Delay in No. of Days", "HeaderStyle"));

                    row = sheet.Table.Rows.Add();
                    String StringField = String.Empty;
                    String StringAlert = String.Empty;
                    string SQLString = string.Empty;
                    row.Index = 7;


                    SQLString = "SELECT  HSRP_StateID, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo FROM HSRPRecords where HSRP_StateID='" +
                        cmbState.SelectedValue + "' and RTOLocationID='" + RTOCode + "' and convert(date,HsrpRecord_creationdate) between '" +
                        dtpFrmDate.Value.ToString("yyyy/MM/dd") + "' and '" + dtpToDate.Value.ToString("yyyy/MM/dd") + "' and NetAmount <> 0 and  OwnerName is not null and OwnerName <> '' and  Address1 is not null and Address1 <> '' order by OrderDate";


                    DataTable dt = GetDataInDT(SQLString);
                    string RTOColName = string.Empty;
                    decimal totalAmount = 0;
                    double tax = 0.0;
                    double exicse = 0.0;
                    double royalty = 0.0;
                    if (dt.Rows.Count > 0)
                    {
                        int sno = 0;
                        foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                        {
                            sno = sno + 1;
                            if (sno == 43)
                            {
                                int ssno = sno;
                            }
                            row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                            //  row.Cells.Add(new WorksheetCell(dtrows["HSRPRecord_AuthorizationNo"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["CashReceiptNo"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["HsrpRecord_AuthorizationDate"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["NewOrderDate"].ToString(), DataType.String, "HeaderStyle"));
                            //row.Cells.Add(new WorksheetCell(dtrows["InvoiceDateTime"].ToString(), DataType.String, "HeaderStyle"));

                            row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle"));

                            if (cmbState.SelectedItem.ToString() == "BIHAR")
                            {
                                row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle"));

                            }
                            string amount = dtrows["NetAmount"].ToString();
                            tax = Math.Round((Convert.ToDouble(amount) * 12.5) / 100.00, 2);
                            exicse = Math.Round(((Convert.ToDouble(amount) - tax) * 12.36) / 100.00, 2);
                            royalty = Math.Round(((Convert.ToDouble(amount) - tax - exicse) * 2) / 100.00, 2);
                            if (amount == "")
                            {

                                row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle"));
                                //totalAmount = totalAmount + Math.Round(Convert.ToDecimal(amount.ToString()));
                            }
                            else
                            {
                                row.Cells.Add(new WorksheetCell(Math.Round(Convert.ToDecimal(dtrows["NetAmount"].ToString())).ToString(), DataType.Number, "HeaderStyle5"));
                                totalAmount = totalAmount + Math.Round(Convert.ToDecimal(dtrows["NetAmount"].ToString()));
                            }
                            row.Cells.Add(new WorksheetCell(tax.ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(exicse.ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(royalty.ToString(), DataType.String, "HeaderStyle"));


                            row = sheet.Table.Rows.Add();
                        }
                        row = sheet.Table.Rows.Add();
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                        row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle8"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                        //  row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));


                        if (cmbState.SelectedItem.ToString() == "BIHAR")
                        {
                            row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                            row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                            row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                        }

                        row.Cells.Add(new WorksheetCell((totalAmount).ToString(), "HeaderStyle8"));
                        tax = Math.Round((Convert.ToDouble(totalAmount) * 12.5) / 100.00, 2);
                        exicse = Math.Round(((Convert.ToDouble(totalAmount) - tax) * 12.36) / 100.00, 2);
                        royalty = Math.Round(((Convert.ToDouble(totalAmount) - tax - exicse) * 2) / 100.00, 2);
                        row.Cells.Add(new WorksheetCell(tax.ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(exicse.ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(royalty.ToString(), DataType.String, "HeaderStyle"));

                        totalAmount = 0;

                        row = sheet.Table.Rows.Add();

                        book.Save(strPath + "\\" + filename);



                    }
                    else
                    {
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void GenerateAuthorityReport()
        {
            DataTable dtrto = GetRtoLocation();
            for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
            {

                string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();

                //if(RTOName.ToUpper().Equals("PALAM"))
                //{
                GenerateReportDL(cmbState.Text.ToString(), cmbState.Text, RTOCode, RTOName);
                //}
            }

        }

        private void GenerateReportDL(string strStateName, string strStateId, string strRtoId, string strLocname)
        {
            try
            {
                string strSubject = strStateName + "AuthorityReport-" + strLocname + "-";
                string filename = strSubject + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                SetFolder(strLocname, strStateName, strSubject);

                Workbook book = new Workbook();

                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Authority Report Order Closed";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
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

                Worksheet sheet = book.Worksheets.Add("HSRP Authority Report Order Closed");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));

                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(150));
                sheet.Table.Columns.Add(new WorksheetColumn(92));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(140));
                sheet.Table.Columns.Add(new WorksheetColumn(140));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                //sheet.Table.Columns.Add(new WorksheetColumn(140));
                //sheet.Table.Columns.Add(new WorksheetColumn(140));
                //sheet.Table.Columns.Add(new WorksheetColumn(100));
                //sheet.Table.Columns.Add(new WorksheetColumn(100));
                //sheet.Table.Columns.Add(new WorksheetColumn(100));


                WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                style9.Font.FontName = "Tahoma";
                style9.Font.Size = 10;
                style9.Font.Bold = true;
                style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style9.Interior.Color = "#FCF6AE";
                style9.Interior.Pattern = StyleInteriorPattern.Solid;


                WorksheetRow row = sheet.Table.Rows.Add();

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("HSRP Authority Report Order Closed");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(strStateName, "HeaderStyle2"));

                //row = sheet.Table.Rows.Add();
                //row.Index = 4;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(strLocname, "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dtpFrmDate.Value.ToString("dd/MMM/yyyy") + "-" + dtpToDate.Value.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();


                row.Index = 5;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vechicle No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front Laser Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear Laser Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owner's Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owner's Address", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vechicle Type", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("New Vehicle/Old Vehicle", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Authorisation Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Affixation Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Old Registration Plate Count", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Plate Removed For Distraction", "HeaderStyle6"));


                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                row.Index = 6;



                string SQLString = "SELECT CONVERT(varchar(20), a.HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,a.address1,a.ordertype, CONVERT(varchar(20), a.OrderDate, 103) AS OrderDate, CONVERT(varchar(20),a.OrderClosedDate,103) as OrderClosedDate,a.HSRPRecordID,a.CashReceiptNo,a.InvoiceNo,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime, VehicleClass,CONVERT(numeric,round(a.RoundOff_NetAmount,0)) as NetAmount, a.HSRPRecord_AuthorizationNo, a.OwnerName, a.VehicleClass, a.VehicleType, a.VehicleRegNo, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.StickerMandatory,  (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as FrontPlateCode, (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as RearPlateCode,case when d.vehicleregno is null then '' else '2' end as 'Exists_or_not' " +
                   "FROM HSRPRecords a left join delhiolddata d on d.vehicleregno = a.vehicleregno " +
                   "where a.HSRP_StateID='" + cmbState.SelectedValue.ToString() + "' and a.RTOLocationID='" + strRtoId
                   + "' and convert(date,a.OrderClosedDate) between '" + dtpFrmDate.Value.ToString("yyyy/MM/dd") + "' and '" + dtpToDate.Value.ToString("yyyy/MM/dd") + "' and  a.OwnerName is not null and a.OwnerName <> '' and  a.OwnerName is not null and a.OwnerName <> '' and  a.Address1 is not null and a.Address1 <> ''  order by OrderClosedDate";
                // SQLString = "SELECT  HSRP_StateID, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), OrderDate, 103) AS OrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( NetAmount,0)) as NetAmount,OrderDate FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "'  and OrderStatus='Closed' and OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' order by HSRPRecord_AuthorizationNo";

                DataTable dt = GetDataInDT(SQLString);
                string RTOColName = string.Empty;
                Int64 totalAmount = 0;
                if (dt.Rows.Count > 0)
                {
                    int sno = 0;
                    string VehicleColor = string.Empty;
                    string Color = string.Empty;


                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        if (dtrows["VehicleClass"].ToString() == "Non-Transport")
                        {
                            VehicleColor = "WHITE";
                        }
                        else
                        {
                            VehicleColor = "YELLOW";
                        }

                        sno = sno + 1;
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VEHICLeregno"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["address1"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VEHICLETYPE"].ToString(), DataType.String, "HeaderStyle"));
                        string strVeh = dtrows["ordertype"].ToString().Equals("NB") ? "New Vehicle" : "Old Vehicle";
                        row.Cells.Add(new WorksheetCell(strVeh, DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["HsrpRecord_AuthorizationDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Exists_or_not"].ToString(), DataType.String, "HeaderStyle"));



                        row = sheet.Table.Rows.Add();
                    }


                    book.Save(strPath + "\\" + filename);

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GenerateCollectionReport()
        {

            try
            {

                string ReportDate1 = string.Empty;

                ReportDate1 = dtpFrmDate.Value.ToString("MM/dd/yyyy") + " 00:00:00";

                string ReportDate2 = dtpToDate.Value.ToString("MM/dd/yyyy") + " 23:59:59";
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));



                DataTable dt = new DataTable();

                DataTable dtrto = GetRtoLocation();
                for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
                {
                    string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                    string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();


                    #region Sql Query
                    string SQLString = "SELECT  HSRP_StateID, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate, " +
                        " CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate," +
                        "CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo," +
                        " OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,orderstatus" +
                        " FROM HSRPRecords where HSRP_StateID='" + cmbState.SelectedValue + "' and RTOLocationID='" + RTOCode + "' and" +
                        "  (OwnerName is not null or OwnerName <> '') and (Address1 is not null or Address1 <> '') and" +
                        " HsrpRecord_creationdate between '" + ReportDate1.ToString() + "' and '" + ReportDate2.ToString() + "' and NetAmount <> 0 order by OrderDate";
                    #endregion

                    dt = GetDataInDT(SQLString);
                   
                    if (dt.Rows.Count > 0)
                    {




                        int iIndex = 3;

                        string filename = "CollectionReport-" + strMonth + strYear + "_" + RTOName + "-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                        SetFolder(RTOName, cmbState.Text, "CollectionReport-" + strMonth + strYear);

                        Workbook book = new Workbook();

                        // Specify which Sheet should be opened and the size of window by default
                        book.ExcelWorkbook.ActiveSheetIndex = 1;
                        book.ExcelWorkbook.WindowTopX = 100;
                        book.ExcelWorkbook.WindowTopY = 200;
                        book.ExcelWorkbook.WindowHeight = 7000;
                        book.ExcelWorkbook.WindowWidth = 8000;

                        // Some optional properties of the Document
                        book.Properties.Author = "HSRP";
                        book.Properties.Title = "HSRP Daily Cash Collection";
                        book.Properties.Created = DateTime.Now;


                        // Add some styles to the Workbook
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

                        Worksheet sheet = book.Worksheets.Add("HSRP Daily Cash Collection");
                        sheet.Table.Columns.Add(new WorksheetColumn(60));
                        sheet.Table.Columns.Add(new WorksheetColumn(100));
                        sheet.Table.Columns.Add(new WorksheetColumn(100));
                        sheet.Table.Columns.Add(new WorksheetColumn(100));

                        sheet.Table.Columns.Add(new WorksheetColumn(90));
                        sheet.Table.Columns.Add(new WorksheetColumn(90));
                        sheet.Table.Columns.Add(new WorksheetColumn(92));
                        sheet.Table.Columns.Add(new WorksheetColumn(90));
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

                        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                        row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                        WorksheetCell cell = row.Cells.Add("HSRP Daily Cash Collection");
                        cell.MergeAcross = 3; // Merge two cells together
                        cell.StyleID = "HeaderStyle3";

                        row = sheet.Table.Rows.Add();
                        row.Index = iIndex++;
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell(cmbState.Text, "HeaderStyle2"));

                        row = sheet.Table.Rows.Add();
                        row.Index = iIndex++;
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell(RTOName, "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("Vehicle Reference:", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("Both", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("Vehicle Type:", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("All", "HeaderStyle2"));

                        row = sheet.Table.Rows.Add();
                        row.Index = iIndex++;

                        DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                        row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString(), "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("Report Duration:", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell(dtpFrmDate.Value.ToString("dd/MMM/yyyy") + "-" + dtpToDate.Value.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                        row = sheet.Table.Rows.Add();


                        row.Index = iIndex++;
                        //row.Cells.Add("Order Date");
                        row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                        //  row.Cells.Add(new WorksheetCell("Authorisation No", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("Cash Receipt No", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("Authorisation Date", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle6"));
                        //row.Cells.Add(new WorksheetCell("Close Date", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("Vehicle Reg. No", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("Owner's Name", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("Vechicle Type", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle6"));
                        if (cmbState.Text == "DELHI")
                        {
                            row.Cells.Add(new WorksheetCell("Affixation Status", "HeaderStyle6"));
                        }
                        if (cmbState.Text == "BIHAR")
                        {
                            row.Cells.Add(new WorksheetCell("EngineNo", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("ChassisNo", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("MobileNo", "HeaderStyle6"));
                        }
                        row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));
                        //row.Cells.Add(new WorksheetCell("Delay in No. of Days", "HeaderStyle"));

                        row = sheet.Table.Rows.Add();
                        String StringField = String.Empty;
                        String StringAlert = String.Empty;

                        row.Index = iIndex++;
                       
                        string RTOColName = string.Empty;
                        decimal totalAmount = 0;
                        if (dt.Rows.Count > 0)
                        {
                            int sno = 0;
                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;
                                if (sno == 43)
                                {
                                    int ssno = sno;
                                }
                                row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                                //  row.Cells.Add(new WorksheetCell(dtrows["HSRPRecord_AuthorizationNo"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["CashReceiptNo"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["HsrpRecord_AuthorizationDate"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["NewOrderDate"].ToString(), DataType.String, "HeaderStyle"));
                                //row.Cells.Add(new WorksheetCell(dtrows["InvoiceDateTime"].ToString(), DataType.String, "HeaderStyle"));

                                row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle"));

                                //Modification done by Himanshu Sharma on 02/07/2014
                                if (cmbState.Text == "DELHI")
                                {
                                    string Status = dtrows["orderstatus"].ToString() == "Closed" ? "AFFIXED" : "CUSTOMER NOT COME .";
                                    row.Cells.Add(new WorksheetCell(Status, DataType.String, "HeaderStyle"));
                                }

                                //Modification done by Himanshu saini On 19/7/2013
                                if (cmbState.Text == "BIHAR")
                                {
                                    row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle"));

                                }
                                string amount = dtrows["NetAmount"].ToString();
                                if (amount == "")
                                {

                                    row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle"));
                                    //totalAmount = totalAmount + Math.Round(Convert.ToDecimal(amount.ToString()));
                                }
                                else
                                {
                                    row.Cells.Add(new WorksheetCell(Math.Round(Convert.ToDecimal(dtrows["NetAmount"].ToString())).ToString(), DataType.Number, "HeaderStyle5"));
                                    totalAmount = totalAmount + Math.Round(Convert.ToDecimal(dtrows["NetAmount"].ToString()));
                                }


                                row = sheet.Table.Rows.Add();
                            }
                            row = sheet.Table.Rows.Add();
                            row = sheet.Table.Rows.Add();
                            row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                            row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle8"));
                            row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                            row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                            row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                            row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                            row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                            row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                            //   row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                            if (cmbState.Text == "DELHI")
                            {
                                row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                            }
                            if (cmbState.Text == "BIHAR")
                            {
                                row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                                row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                                row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                            }

                            row.Cells.Add(new WorksheetCell((totalAmount).ToString(), "HeaderStyle8"));

                            totalAmount = 0;

                            row = sheet.Table.Rows.Add();

                            book.Save(strPath + "\\" + filename);

                        }
                        else
                        {

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        string strInvoiceNo = "54321";
        private void GenerateAffixationReport()
        {
            string strRTOName = string.Empty;
            int iRow = 0;
            try
            {



                String DatePrint = dtpFrmDate.Value.ToString("dd/MMM/yyyy") + "   To   " + dtpToDate.Value.ToString("dd/MMM/yyyy");


                string ReportDate1 = dtpFrmDate.Value.ToString("dd/MMM/yyyy") + " 00:00:00";
                string ReportDate2 = dtpToDate.Value.ToString("dd/MMM/yyyy") + " 23:59:50";

                iRow++;

                DataTable dtrto = GetRtoLocation();
                for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
                {
                    string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                    string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();

                    strRTOName = RTOName;

                    
                    string SQLString = "SELECT CONVERT(varchar(20), a.HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,convert(varchar,ordercloseddate,103) as ordercloseddate,  CONVERT(varchar(20), a.OrderDate, 103) AS OrderDate, a.HSRPRecordID,a.CashReceiptNo,a.InvoiceNo,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime, VehicleClass,CONVERT(numeric,round(a.RoundOff_NetAmount,0)) as NetAmount, a.HSRPRecord_AuthorizationNo, a.OwnerName, a.VehicleClass, a.VehicleType, a.VehicleRegNo, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.StickerMandatory,  (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as FrontPlateCode, (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as RearPlateCode  FROM HSRPRecords a  where a.HSRP_StateID='" + cmbState.SelectedValue + "' and a.RTOLocationID='" + RTOCode + "' and "+
                        "a.OrderClosedDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and  a.OwnerName is not null and a.OwnerName <> '' and Address1 is not null and Address1 <> '' order by OrderClosedDate";
                    // SQLString = "SELECT  HSRP_StateID, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), OrderDate, 103) AS OrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( NetAmount,0)) as NetAmount,OrderDate FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "'  and OrderStatus='Closed' and OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' order by HSRPRecord_AuthorizationNo";

                    DataTable dt = GetDataInDT(SQLString);
                    string RTOColName = string.Empty;
                    Int64 totalAmount = 0;
                    if (dt.Rows.Count > 0)
                    {

                    string filename = "Affixation-Report-" + RTOName + "-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    SetFolder(RTOName, cmbState.Text, "Affixation-Report-");
                    Workbook book = new Workbook();

                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "HSRP Authority Report Order Closed";
                    book.Properties.Created = DateTime.Now;


                    // Add some styles to the Workbook
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

                    Worksheet sheet = book.Worksheets.Add("HSRP Authority Report Order Closed");
                    sheet.Table.Columns.Add(new WorksheetColumn(40));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));

                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                    sheet.Table.Columns.Add(new WorksheetColumn(92));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                    //sheet.Table.Columns.Add(new WorksheetColumn(100));
                    //sheet.Table.Columns.Add(new WorksheetColumn(100));
                    //sheet.Table.Columns.Add(new WorksheetColumn(100));


                    WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                    style9.Font.FontName = "Tahoma";
                    style9.Font.Size = 10;
                    style9.Font.Bold = true;
                    style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                    style9.Interior.Color = "#FCF6AE";
                    style9.Interior.Pattern = StyleInteriorPattern.Solid;


                    WorksheetRow row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                    WorksheetCell cell = row.Cells.Add("HSRP Affixation Report");
                    cell.MergeAcross = 3; // Merge two cells together
                    cell.StyleID = "HeaderStyle3";

                    row = sheet.Table.Rows.Add();
                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(cmbState.Text, "HeaderStyle2"));

                    //row = sheet.Table.Rows.Add();
                    //row.Index = 4;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(RTOName, "HeaderStyle2"));

                    row = sheet.Table.Rows.Add();
                    row.Index = 4;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    //  row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                    //  row.Cells.Add(new WorksheetCell(dates.ToString("dd/M/yyyy"), "HeaderStyle2"));
                    //row = sheet.Table.Rows.Add();
                    //row.Index = 6;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                    row.Cells.Add(new WorksheetCell("Report Duration:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DatePrint, "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();


                    row.Index = 5;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Cash Receipt No", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Invoice No", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Invoice DateTime", "HeaderStyle6"));
                    //  row.Cells.Add(new WorksheetCell("Authorisation No", "HeaderStyle6"));
                    //  row.Cells.Add(new WorksheetCell("Authorisation Date", "HeaderStyle6"));
                    //  row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Owner's Name", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Vechicle Type", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Vehicle Reg. No", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Front Laser No.", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Rear Laser No.", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Front Plate Size", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Rear Plate Size.", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("3RD Sticker", "HeaderStyle6"));
                    //  row.Cells.Add(new WorksheetCell("Plate Color.", "HeaderStyle6")); 
                    row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));
                    //row.Cells.Add(new WorksheetCell("Delay in No. of Days", "HeaderStyle"));

                    row = sheet.Table.Rows.Add();
                    String StringField = String.Empty;
                    String StringAlert = String.Empty;
                    row.Index = 6;



                    
                        int sno = 0;
                        string VehicleColor = string.Empty;
                        string Color = string.Empty;


                        foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                        {
                            iRow++;
                            if (dtrows["VehicleClass"].ToString() == "Non-Transport")
                            {
                                VehicleColor = "WHITE";
                            }
                            else
                            {
                                VehicleColor = "YELLOW";
                            }

                            sno = sno + 1;
                            row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                            //  string strInvoiceNo = string.Empty;
                            //if (string.IsNullOrEmpty(dtrows["CashReceiptNo"].ToString()))
                            //{
                            //    strInvoiceNo = "BRR/CSH000" + (Convert.ToInt32(strInvoiceNo.Substring(Math.Max(0, strInvoiceNo.Length - 4))) + 1).ToString();
                            //}
                            //else
                            //{
                                strInvoiceNo = Convert.ToString(dtrows["CashReceiptNo"]);
                           // }
                            row.Cells.Add(new WorksheetCell(strInvoiceNo, DataType.String, "HeaderStyle"));
                            if (string.IsNullOrEmpty(dtrows["InvoiceNo"].ToString()))
                            {
                                row.Cells.Add(new WorksheetCell(strInvoiceNo, DataType.String, "HeaderStyle"));
                            }
                            else
                            {
                                row.Cells.Add(new WorksheetCell(dtrows["InvoiceNo"].ToString(), DataType.String, "HeaderStyle"));
                            }
                            if (string.IsNullOrEmpty(dtrows["InvoiceDateTime"].ToString()))
                            {
                                row.Cells.Add(new WorksheetCell(dtrows["ordercloseddate"].ToString(), DataType.String, "HeaderStyle"));
                            }
                            else
                            {
                                row.Cells.Add(new WorksheetCell(dtrows["InvoiceDateTime"].ToString(), DataType.String, "HeaderStyle"));
                            }
                            //  row.Cells.Add(new WorksheetCell(dtrows["HSRPRecord_AuthorizationNo"].ToString(), DataType.String, "HeaderStyle"));
                            //   row.Cells.Add(new WorksheetCell(dtrows["HsrpRecord_AuthorizationDate"].ToString(), DataType.String, "HeaderStyle"));
                            //   row.Cells.Add(new WorksheetCell(dtrows["OrderDate"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["FrontplateCode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["RearplateCode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle5"));
                            //   row.Cells.Add(new WorksheetCell(VehicleColor, DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["NetAmount"].ToString(), DataType.String, "HeaderStyle"));
                            if (dtrows["NetAmount"].ToString() == "")
                            {
                                totalAmount = totalAmount = 0;
                            }
                            else
                            {
                                totalAmount = totalAmount + Convert.ToInt64(dtrows["NetAmount"].ToString());
                            }


                            row = sheet.Table.Rows.Add();
                        }
                        //  row.Cells.Add(new WorksheetCell("Total", DataType.String, "HeaderStyle"));
                        WorksheetCell cell1 = row.Cells.Add("");

                        book.Save(strPath + "\\" + filename);


                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + strRTOName + iRow.ToString());
            }

        }

        #endregion

        #region MP
        private void GenerateSchedule_CE_MP()
        {

            DataTable GetAddress;

            string Address = string.Empty;
            string CashCollectionDate = dtpFrmDate.Value.ToString("dd/MM/yyyy");
            string Sqlquery = "select  [dbo] .[GetAffxDate_Insert_Hr] ('" + dtpFrmDate.Value.ToString("dd/MMM/yyyy") + "','" + cmbState.SelectedValue + "') as AffDate";
            DataTable dtDate = GetDataInDT(Sqlquery);
            string AffixiationDate = Convert.ToDateTime(dtDate.Rows[0]["AffDate"]).ToString("dd/MM/yyyy");

            DateTime dateaffix = DateTime.ParseExact(Convert.ToDateTime(dtDate.Rows[0]["AffDate"]).ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dateCurrent = DateTime.ParseExact(Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (dateCurrent < dateaffix)
            {
                // Label1.Text = "Your Affixed Date has Not Come yet Please Wait !";
                return;
            }
            else
            {

                DataTable dtrto = GetRtoLocation();



                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:


                for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
                {
                    Document document = new Document();

                    string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                    string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();


                    //Opens the document:



                    string StrRtoLcatioCode = "select Rtolocationcode from rtolocation where HSRP_StateID='" + cmbState.SelectedValue.ToString() + "' and Rtolocationid='" + RTOCode + "'";
                    DataTable dtrtocode = GetDataInDT(StrRtoLcatioCode);
                    string RTOCode1 = dtrtocode.Rows[0]["Rtolocationcode"].ToString();

                    GetAddress = GetDataInDT("select * from HSRPState WHERE HSRP_StateID='" + cmbState.SelectedValue + "'");
                    if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
                    {
                        Address = GetAddress.Rows[0]["CompanyName"].ToString() + "\n" + GetAddress.Rows[0]["Address1"].ToString();
                    }



                    #region Sql Query For Report
                    string SQLString = " select ROW_NUMBER() over(order by hsrprecord_authorizationno) as [S.No], hsrprecord_authorizationno as " +
                                   " [Application no.],convert(varchar(15),HSRPRecord_CreationDate,103) as creationdate,vehicletype as [Vehicle Type],OwnerName as [Owner's Name],vehicleregno as [Vehicle Reg. No],a.mobileno," +
                                   " HSRP_Front_LaserCode as [Front Laser],hsrp_rear_lasercode as [Rear Laser],case when StickerMandatory ='Y' then 'YES' else " +
                                   " 'N/A' end as [3RD RP Y/N], case when VehicleClass = 'Non-Transport' then 'White' else 'Yellow' end as [COLOR], case when " +
                                   " orderstatus='Closed' then convert(varchar(15),ordercloseddate,103) end as [AFFIXATION], case when OrderStatus='Closed' " +
                                   " then 'AFFIXED' else 'CUSTOMER NOT COME .' end as 'Affixationstatus' ,''as [BACKREMARK],(select productdimension +'MM' " +
                                   " from product where productid=frontplatesize) as fpsize,(select productdimension +'MM' from product where productid=Rearplatesize) " +
                                   " as rpsize from hsrprecords a inner join rtolocation as b on a.RTOLocationID=b.rtolocationid where a.HSRP_StateID='" + cmbState.SelectedValue + "' and distrelation='" + RTOCode + "' and convert(date,HSRPRecord_CreationDate) between '" + dtpFrmDate.Value.ToString("yyyy/MM/dd") + "' and '" + dtpToDate.Value.ToString("yyyy/MM/dd") + "' and " +
                                   " ((hsrp_front_lasercode is not null and HSRP_Rear_LaserCode is not null) or   (hsrp_front_lasercode !='' and HSRP_Rear_LaserCode !='')) and ownername is not null and " +
                                     "hsrp_front_lasercode not in ( 'AA271055126','AA271072995','AA170855418','AA210591749','AA210597574','AA210599883','AA170855406','AA170871225','AA210599855','AA170850011','AA251393551','AA170857134','AA170855432','AAA10861157','AA210599873','AA251569860','AA170855444','AA251315167','AA210597584','AA170873704','AA271074025','AA220216982','AA220216956','AA170857252','AA210597560','AA271223713','AA170856472','AA210597552','AA170850164','AA170855430','AA210599869','AA210597558','AA210584310','AA170535357','AA210599851','AA170856464','AA170855429','AA220220312','AA170855414','AA170855412','AA210597564','AA170856484','AA270954164','AA170908478','AA170855407','AAC40050146','AA210599853','AA210915750','AA210597570','AA210599875','AA170855421','AA170856498','AA170856480','AA170855435','AA271020815','AA210597554','AA170855417','AA251611685','AA170855442','AA251197251','AA220220310','AA271032822','AA170855401','AA210597586','AA170911124','AA170855408','AA210597580','AA271248115','AA270942098','AA170855419','AA220220306','AA210584322','AA251395073','AA170856478','AA271062786','AA210597556','AA271049246','AA170855405','AA271055122','AA210597592','AA270739527','AA170855402','AA210597576','AA170855428','AA270758806','AA251385101','AA170851105','AA210597578','AA170856485','AA170855413','AA251203769','AAC60082639','AA170855409','AA210597572','AA271150518','AA210599877','AA170855425','AA170855415','AA170855416','AA270951141','AA271262087','AA170850176','AA210573349','AA210589716','AA210597596','AA210599879','AA170856495','AA210597594','AA210599881','AA251648245','AA170850169','AA270966578','AA270888770','AA170850182','AA270888762','AA210597588','AA170856479','AA170908931','AA270969695','AA210594113','AA170861370','AA210581973','AA271243277','AA170855404','AA170856494','AA271045500','AA170855410','AA170855431','AA170850177','AA120022126','AA251322504','AA271075549','AA170856491','AA220220314','AA210597582','AA220220308','AA170856469','AA170855440','AA210597566','AA170855423','AA210597568','AA210597562','AA170855426','AA170856470') and netamount >0 ";
                    #endregion
                    DataTable dtRecord = GetDataInDT(SQLString);

                    if (dtRecord.Rows.Count > 0)
                    {

                        #region Common Code For PDf

                        string filename = "Schedule C-E_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                        SetFolder(RTOName, cmbState.Text, cmbState.SelectedItem + "Schedule C-E_");
                        string PdfFolder = strPath + "\\" + filename;
                        PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                        document.Open();

                        PdfPTable table1 = new PdfPTable(17);

                        var colWidthPercentages = new[] { 17f, 50f, 35f, 40f, 44f, 48f, 39f, 45f, 39f, 45f, 39f, 37f, 21f, 21f, 41f, 35f, 30f };
                        table1.SetWidths(colWidthPercentages);

                        table1.TotalWidth = 820f;
                        table1.LockedWidth = true;




                        #endregion
                        #region Row 1
                        PDFRows(bfTimes, table1, 17, "LINK UTSAV AUTO SYETEMS PVT. LTD." + "\n" + "State Office- E3/39,Arera Colony,Bhopal (MP)", 8, 1, "N", 1, 1, 1, 1);

                        #endregion
                        #region Row 2

                        PDFRows(bfTimes, table1, 8, "Report : 2 (Schedule - C-E)", 8, 1, "N", 1, 0, 0, 1);
                        PDFRows(bfTimes, table1, 9, "Embossing Station to Registering Authority", 8, 1, "N", 1, 1, 0, 1);

                        #endregion
                        #region Row 3
                        PDFRows(bfTimes, table1, 4, "LOCATION : ", 8, 0, "N", 1, 0, 0, 0);
                        PDFRows(bfTimes, table1, 13, RTOName, 8, 0, "N", 0, 1, 0, 0);
                        PDFRows(bfTimes, table1, 4, "Cash Collection Date : ", 8, 0, "N", 1, 0, 0, 0);
                        PDFRows(bfTimes, table1, 13, CashCollectionDate, 8, 0, "N", 0, 1, 0, 0);

                        PDFRows(bfTimes, table1, 4, "", 8, 0, "N", 1, 0, 0, 0);
                        PDFRows(bfTimes, table1, 13, "", 8, 0, "N", 0, 1, 0, 0);

                        PDFRows(bfTimes, table1, 4, "Report Generated Date : ", 8, 0, "N", 1, 0, 0, 0);
                        PDFRows(bfTimes, table1, 13, System.DateTime.Now.ToString("dd/MM/yyyy"), 8, 0, "N", 0, 1, 0, 0);
                        PDFRows(bfTimes, table1, 17, "", 8, 1, "N", 1, 1, 1, 1);

                        #endregion
                        #region Row 4
                        PDFRows(bfTimes, table1, 1, "S.No", 8, 0, "N", 1, 1, 0, 1, optionalWidth: 10);
                        PDFRows(bfTimes, table1, 1, "Application/ Authorisation No.", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "Date", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "Vehicle Type", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "Vehicle No", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
                        PDFRows(bfTimes, table1, 1, "Owner Name", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "Mobile No", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "Front Laser Code", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "FP Size", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
                        PDFRows(bfTimes, table1, 1, "Rear Laser Code", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "RP Size", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "Third Registration Plate", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "Color", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "Old/ New", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "Affixation Status", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "Date Of Affixation", 8, 0, "N", 0, 1, 0, 1);
                        PDFRows(bfTimes, table1, 1, "Background Remarks", 8, 0, "N", 0, 1, 0, 1);
                        #endregion
                        #region Dynamics Rows
                        for (int iRowCount = 0; iRowCount < dtRecord.Rows.Count; iRowCount++)
                        {
                            PDFRows(bfTimes, table1, 1, (iRowCount + 1).ToString(), 8, 0, "N", 1, 1, 0, 1, optionalWidth: 10);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["Application no."].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["creationdate"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            //PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["Rtolocationname"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["Vehicle Type"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["Vehicle Reg. No"].ToString(), 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["Owner's Name"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["mobileno"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["Front Laser"].ToString(), 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["FPSize"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["Rear Laser"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["RPSize"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["3RD RP Y/N"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["Color"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, "NEW", 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["AffixationStatus"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["Affixation"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                            PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["BackRemark"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                        }
                        #endregion

                        document.Add(table1);
                        document.NewPage();
                        document.Close();
                    }


                }

            }
        }

        private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(0))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else if (iFont.Equals(1))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            newCellPDF.Colspan = iSpan;
            newCellPDF.BorderWidthLeft = iLeftWidth;
            newCellPDF.BorderWidthRight = iRightWidth;
            newCellPDF.BorderWidthTop = iTopWidth;
            newCellPDF.BorderWidthBottom = iBottomWidth;
            newCellPDF.HorizontalAlignment = iAllign;
            if (!iRowHeight.Equals(0))
            {
                newCellPDF.FixedHeight = iRowHeight;
            }
            if (!iRowWidth.Equals(0))
            {
            }
            table.AddCell(newCellPDF);
        }
        private void GenerateMPCashCollection()
        {
            //For Before Onne Date
            //DateTime d1 =DateTime.Now.Date;
             

            //d1 = d1.AddDays(-1);
            //string date = d1.ToShortDateString();
           
            //string Sqlquery = "select  [dbo] .[GetAffxDate_Insert_Hr] ('" + d1 + "','5') as AffDate";
            //DataTable dtDate = GetDataInDT(Sqlquery);
            //string AffixiationDate = Convert.ToDateTime(dtDate.Rows[0]["AffDate"]).ToString("dd/MMM/yyyy");




            //  string RtoName = ddlRtoLocation.SelectedItem.ToString();
            string CashCollectionDate = dtpFrmDate.Value.ToString("dd/MMM/yyyy");
            //string AffixiationDate=OrderDatefrom.SelectedDate.AddDays(4).ToString("dd/MMM/yyyy");
            string Sqlquery = "select  [dbo] .[GetAffxDate_Insert_Hr] ('" + CashCollectionDate + "','5') as AffDate";
           // string Sqlquery = "14/Oct/2014";
            DataTable dtDate = GetDataInDT(Sqlquery);
            string AffixiationDate = Convert.ToDateTime(dtDate.Rows[0]["AffDate"]).ToString("dd/MM/yyyy");
           // string AffixiationDate = Sqlquery;
            string SQLString = String.Empty;
            // Document document = new Document();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);



            DataTable dtrto = GetRtoLocation();
            for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
            {
                string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                string filename = "MP_CashCollection_" + RTOName + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                Document document = new Document();
                SetFolder(RTOName, cmbState.Text, "MP_CashCollection_");

                string PdfFolder = strPath + "\\" + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                string StrComapnyName = "select CompanyName,Address1 from hsrpstate where hsrp_stateid='" + cmbState.SelectedValue.ToString() + "'";

                document.Open();


                DataTable dt = GetDataInDT(StrComapnyName);

                string strCompanyName = dt.Rows[0]["CompanyName"].ToString();
                string strAddress = dt.Rows[0]["Address1"].ToString();


                string StrRtoLcatioCode = "select Rtolocationcode from rtolocation where HSRP_StateID='" + cmbState.SelectedValue.ToString() + "' and Rtolocationid='" + RTOCode + "'";
                DataTable dtrtocode = GetDataInDT(StrRtoLcatioCode);
                string RTOCode1 = dtrtocode.Rows[0]["Rtolocationcode"].ToString();

                // As refer by Naveen Sir Data will always come after 16 May 2014

                SQLString = "select row_number() over (order by vehicleregno) as SNo, vehicleregno as VehicleNo,ownername as OwnerName," +
                    "a.mobileno from hsrprecords a inner join rtolocation as b on a.rtolocationid=b.rtolocationid " +
                    "where a.HSRP_StateID='" + cmbState.SelectedValue + "' and b.distRelation='" + RTOCode + "' and Vehicleregno like '" +
               "%' and convert(date,HSRPRecord_CreationDate) between '" + dtpFrmDate.Value.ToString("yyyy/MM/dd")
                    + "' and '" + dtpToDate.Value.ToString("yyyy/MM/dd") + "' and HSRPRecord_CreationDate > '2014/05/16 00:00:00' and netamount > 0";

                //SQLString = "select row_number() over (order by vehicleregno) as SNo, vehicleregno as VehicleNo,ownername as OwnerName," +
                //   "a.mobileno from hsrprecords a inner join rtolocation as b on a.rtolocationid=b.rtolocationid " +
                //   "where a.HSRP_StateID='" + cmbState.SelectedValue + "' and b.distRelation='" + RTOCode + "' and Vehicleregno like '" +
                //   RTOCode1 + "%' and convert(date,HSRPRecord_CreationDate)='" + d1 + "' and HSRPRecord_CreationDate > '2014/05/16 00:00:00' and netamount > 0";

                // SQLString = "select row_number() over (order by vehicleregno) as SNo, vehicleregno as VehicleNo,ownername as OwnerName,mobileno from hsrprecords where hsrp_stateid='" + cmbState.SelectedValue.ToString() + "' and RTOLocationId='" + ddlRtoLocation.SelectedValue.ToString() + "' and convert(date,HSRPRecord_CreationDate)='" + OrderDatefrom.SelectedDate + "' and HSRPRecord_CreationDate > '2014/05/16 00:00:00' and netamount > 0";
                DataTable dtResult = GetDataInDT(SQLString);


                //PdfPTable table2 = new PdfPTable(8);
                // PdfPTable table1 = new PdfPTable(8);
                PdfPTable table = new PdfPTable(8);
                table.TotalWidth = 1000f;

                GenerateCell(table, 8, 0, 0, 0, 0, 1, 1, "", 50, 0);
                GenerateCell(table, 8, 1, 1, 1, 1, 1, 0, strCompanyName, 15, 0);
                GenerateCell(table, 8, 1, 1, 0, 0, 1, 0, " State Office- E3/39,Arera Colony,Bhopal (MP)", 15, 0);
                GenerateCell(table, 8, 1, 1, 1, 0, 1, 0, "Report:1", 15, 0);
                GenerateCell(table, 4, 1, 0, 0, 0, 0, 0, "Location:", 15, 0);
                GenerateCell(table, 4, 0, 1, 0, 0, 0, 0, RTOName, 15, 0);
                GenerateCell(table, 4, 1, 0, 0, 0, 0, 0, "Cash Collection Date:-", 15, 0);
                GenerateCell(table, 4, 0, 1, 0, 0, 0, 0, CashCollectionDate, 15, 0);
                GenerateCell(table, 4, 1, 0, 0, 0, 0, 0, "Affixiation due Date:-", 15, 0);
                GenerateCell(table, 4, 0, 1, 0, 0, 0, 0, AffixiationDate, 15, 0);

                GenerateCell(table, 8, 1, 1, 1, 1, 1, 1, "", 10, 0);


                GenerateCell(table, 2, 1, 1, 0, 1, 1, 0, "S.No:", 20, 0);
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Vehicle No", 20, 0);
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Owner Name", 20, 0);
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Mobile No", 20, 0);
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    GenerateCell(table, 2, 1, 1, 0, 1, 1, 1, dtResult.Rows[i]["SNo"].ToString(), 20, 0);
                    GenerateCell(table, 2, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["VehicleNo"].ToString().ToUpper(), 20, 0);
                    GenerateCell(table, 2, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["OwnerName"].ToString(), 20, 0);
                    GenerateCell(table, 2, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["mobileno"].ToString(), 20, 0);
                }
                //document.Add(table1);
                document.Add(table);
                document.NewPage();
                document.Close();

            }


        }


        //for uk

        private void GenerateUKCashCollection()
        {
            //  string RtoName = ddlRtoLocation.SelectedItem.ToString();
            string CashCollectionDate = dtpFrmDate.Value.ToString("dd/MMM/yyyy" );
            
            //string AffixiationDate=OrderDatefrom.SelectedDate.AddDays(4).ToString("dd/MMM/yyyy");
            string Sqlquery = "select  [dbo] .[GetAffxDate_Insert_Hr] ('" + CashCollectionDate + "','5') as AffDate";
            DataTable dtDate = GetDataInDT(Sqlquery);
            DateTime AffDate  = Convert.ToDateTime(DateTime.ParseExact(dtDate.Rows[0]["AffDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
            string AffixiationDate = AffDate.ToString();  

            string SQLString = String.Empty;
            // Document document = new Document();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);



            DataTable dtrto = GetRtoLocation();
            for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
            {
                string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
               // string RTOName = "Kashipur";
                string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
               // string RTOCode = "881";
                string filename = "Schedule-Uk_Embossing_Report" + RTOName + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                Document document = new Document();
                SetFolder(RTOName, cmbState.Text, "Schedule-UK_Embossing_Report");

                string PdfFolder = strPath + "\\" + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                string StrComapnyName = "select CompanyName,Address1 from hsrpstate where hsrp_stateid='" + cmbState.SelectedValue.ToString() + "'";

                document.Open();


                DataTable dt = GetDataInDT(StrComapnyName);

               // string strCompanyName = dt.Rows[0]["CompanyName"].ToString();
                string strAddress = dt.Rows[0]["Address1"].ToString();


                string StrRtoLcatioCode = "select Rtolocationcode from rtolocation where HSRP_StateID='" + cmbState.SelectedValue.ToString() + "' and Rtolocationid='" + RTOCode + "'";
                DataTable dtrtocode = GetDataInDT(StrRtoLcatioCode);
                string RTOCode1 = dtrtocode.Rows[0]["Rtolocationcode"].ToString();

              
               // SQLString = "SELECT  ROW_NUMBER() over( order by  VehicleRegNo) as SNo,  b.ProductColor, a.HSRPRecord_AuthorizationNo as vh1,a.VehicleRegNo as vh2, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '6' AND a.OrderEmbossingDate  between '" + dtpFrmDate.Value.ToString("yyyy/MM/dd") + "' and '" + dtpToDate.Value.ToString("yyyy/MM/dd")+"'";



                SQLString = "SELECT  ROW_NUMBER() over( order by  VehicleRegNo) as SNo, (select top 1 ProductColor from Product where (ProductID =FrontPlateSize)) as ProductColor, a.HSRPRecord_AuthorizationNo as vh1,a.VehicleRegNo as vh2, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,(select top 1 ProductColor from Product where (ProductID =RearPlateSize)) as ProductColorrear,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, (select top 1 ProductCode from Product where (ProductID =FrontPlateSize))  AS FrontPlateSize, a.RearPlateSize, (select top 1 ProductCode from Product where (ProductID =RearPlateSize))  AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a WHERE  a.HSRP_StateID= '6' and RTOLocationid ="+ RTOCode +" AND a.OrderEmbossingDate  between  '" + dtpFrmDate.Value.ToString("yyyy/MM/dd" + " 00:00:00") + "' and '" + dtpToDate.Value.ToString("yyyy/MM/dd" + " 23:59:59") + "'";


               // SQLString = "SELECT  ROW_NUMBER() over( order by  VehicleRegNo) as SNo, (select top 1 ProductColor from Product where (ProductID =FrontPlateSize)) as ProductColor, a.HSRPRecord_AuthorizationNo as vh1,a.VehicleRegNo as vh2, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,(select top 1 ProductColor from Product where (ProductID =RearPlateSize)) as ProductColorrear,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, (select top 1 ProductCode from Product where (ProductID =FrontPlateSize))  AS FrontPlateSize, a.RearPlateSize, (select top 1 ProductCode from Product where (ProductID =RearPlateSize))  AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a WHERE  a.HSRP_StateID= '6' AND a.OrderEmbossingDate  between  '2014/09/19 00:00:00' and '2014/09/19 23:59:59'";



                //SQLString = " SELECT  ROW_NUMBER() over( order by  VehicleRegNo) as SNo, (select top 1 ProductColor from Product where (ProductID =FrontPlateSize)) as ProductColor, a.HSRPRecord_AuthorizationNo as vh1,a.VehicleRegNo as vh2, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,(select top 1 ProductColor from Product where (ProductID =RearPlateSize)) as ProductColorrear,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, (select top 1 ProductCode from Product where (ProductID =FrontPlateSize))  AS FrontPlateSize, a.RearPlateSize, (select top 1 ProductCode from Product where (ProductID =RearPlateSize))  AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a WHERE  a.HSRP_StateID= '6' AND a.OrderEmbossingDate  between '2014/09/20 00:00:00' and '2014/09/20 23:59:59'";


                 dt = GetDataInDT(SQLString);





                 document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                 document.Open();
                 PdfPTable table = new PdfPTable(11);
                 var colWidthPercentages = new[] { 17f, 60f, 40f, 45f, 40f, 40f, 50f, 50f, 50f, 25f,25f };
                 table.SetWidths(colWidthPercentages);
                 //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
                 string SqlQuery = string.Empty;

                 table.TotalWidth = 800f;
                 table.LockedWidth = true;
                //  GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);
                GenerateCell(table, 11, 0, 0, 0, 0, 1, 0, "ANNEXURE 1", 20, 0);
                GenerateCell(table, 11, 0, 0, 0, 0, 1, 0, "(Shedule 2,Clause 5.5.1(a))", 20, 0);
                GenerateCell(table, 11, 0, 0, 0, 0, 1, 0, "Daily Report from Embossing Stations to Registering Authority", 20, 0);
                GenerateCell(table, 11, 0, 0, 0, 0, 0, 0, "Location:" + RTOName, 20, 0);

                GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "Date Period  :" + dtpFrmDate.Value.ToString("dd/MMM/yyyy") + " - " + dtpToDate.Value.ToString("dd/MMM/yyyy"), 15, 0);
               // GenerateCell(table, 7, 0, 0, 0, 0, 0, 0, "Generated Date time: " + System.DateTime.Now.ToString("dd/MMM/yyyy"), 15, 0);
                GenerateCell(table, 11, 0, 0, 0, 0, 1, 0, "", 20, 0);
                GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 20, 0);
               
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Application No.", 20, 0);

                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Type", 20, 0);
                
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Reg. No.", 20, 0);
                GenerateCell(table, 2, 0, 1, 1, 1, 1, 0, "Laser Identification No", 20, 0);

                GenerateCell(table, 2, 0, 1, 1, 1, 1, 0, "High Security Registration Plate Size", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "3rd Plate Y/N", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Color Back Ground", 20, 0);
                GenerateCell(table, 1, 1, 1, 0, 1, 1, 0, "", 10, 0);
                GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "", 10, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 10, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 10, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 10, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front", 10, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear", 10, 0);

                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front", 10, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear", 10, 0);

                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Sticker", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
                int SnoCounter = 0;
                //#region Dynamic Rows
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnoCounter = SnoCounter + 1;
                   GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, SnoCounter.ToString(), 20, 10);
                   GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["vh1"].ToString(), 20, 10); 
                    
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["VehicleType"].ToString(), 20, 10);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OwnerName"].ToString(), 20, 10);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["vh2"].ToString().ToUpper(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["HSRP_Front_LaserCode"].ToString(), 20, 10);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["HSRP_Rear_LaserCode"].ToString(), 20, 10);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["FrontPlateSize"].ToString(), 20, 10);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["RearPlateSize1"].ToString(), 20, 10);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["StickerMandatory"].ToString(), 20, 10);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["ProductColor"].ToString(), 20, 10);
                 
                   
                    
                }
                //document.Add(table1);
                document.Add(table);
                document.NewPage();
                document.Close();

            }


        }

        #endregion


        #region Haryana
       

        //private void GenerateHRMonthlyReport()
        //{

        //    try
        //    {
                
        //    DataTable dtrto = GetRtoLocation();
        //    for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
        //    {
        //        string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
        //        string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
        //        string filename = "HRMonthlyReport_"  + RTOName+ strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

                
        //        string SQLString = "SELECT   a.Address1,a.OrderType ,a.ManufacturerName, convert(varchar,a.OrderDate,110)as OrderDate ,convert(varchar,a.OrderClosedDate,110)as OrderClosedDate,convert(varchar,a.HSRPRecord_AuthorizationDate,110)as HSRPRecord_AuthorizationDate, b.ProductColor, a.HSRPRecord_AuthorizationNo, convert(varchar,a.OrderEmbossingDate,110)as OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize, a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '4' and RTOLocationID='"+RTOCode+"' AND a.OrderClosedDate  between '" + dtpFrmDate.Value.ToString("yyyy/MM/dd" + " 00:00:00") + "' and '" + dtpToDate.Value.ToString("yyyy/MM/dd" + " 23:59:59") + "'";
        //        DataTable dt = GetDataInDT(SQLString);


        //        string RTOColName = string.Empty;
        //        int sno = 0;
        //        if (dt.Rows.Count > 0)
        //        {
                    
        //            Workbook book = new Workbook();
        //            SetFolder("", cmbState.Text, "HRMonthlyReport_" + strMonth + strYear + "_");
        //            // Specify which Sheet should be opened and the size of window by default
        //            book.ExcelWorkbook.ActiveSheetIndex = 1;
        //            book.ExcelWorkbook.WindowTopX = 100;
        //            book.ExcelWorkbook.WindowTopY = 200;
        //            book.ExcelWorkbook.WindowHeight = 7000;
        //            book.ExcelWorkbook.WindowWidth = 8000;

        //            // Some optional properties of the Document
        //            book.Properties.Author = "HSRP";
        //            book.Properties.Title = "Daily Embossing Report";
        //            book.Properties.Created = DateTime.Now;


        //            // Add some styles to the Workbook
        //            WorksheetStyle style = book.Styles.Add("HeaderStyle");
        //            style.Font.FontName = "Tahoma";
        //            style.Font.Size = 10;
        //            style.Font.Bold = true;
        //            style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //            style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //            style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //            style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //            style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

        //            WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
        //            style4.Font.FontName = "Tahoma";
        //            style4.Font.Size = 10;
        //            style4.Font.Bold = false;
        //            style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //            style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //            style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //            style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //            style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //            WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
        //            style8.Font.FontName = "Tahoma";
        //            style8.Font.Size = 10;
        //            style8.Font.Bold = true;
        //            style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //            style8.Interior.Color = "#D4CDCD";
        //            style8.Interior.Pattern = StyleInteriorPattern.Solid;

        //            WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
        //            style5.Font.FontName = "Tahoma";
        //            style5.Font.Size = 10;
        //            style5.Font.Bold = false;
        //            style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //            style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //            style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //            style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //            style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //            WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
        //            style2.Font.FontName = "Tahoma";
        //            style2.Font.Size = 10;
        //            style2.Font.Bold = true;
        //            style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


        //            WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
        //            style3.Font.FontName = "Tahoma";
        //            style3.Font.Size = 12;
        //            style3.Font.Bold = true;
        //            style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;



        //            Worksheet sheet11 = book.Worksheets.Add("SCHEDULE-F");
        //            sheet11.Table.Columns.Add(new WorksheetColumn(60));
        //            sheet11.Table.Columns.Add(new WorksheetColumn(205));
        //            sheet11.Table.Columns.Add(new WorksheetColumn(100));
        //            sheet11.Table.Columns.Add(new WorksheetColumn(130));

        //            sheet11.Table.Columns.Add(new WorksheetColumn(100));
        //            sheet11.Table.Columns.Add(new WorksheetColumn(120));
        //            sheet11.Table.Columns.Add(new WorksheetColumn(112));
        //            sheet11.Table.Columns.Add(new WorksheetColumn(109));
        //            sheet11.Table.Columns.Add(new WorksheetColumn(105));
        //            sheet11.Table.Columns.Add(new WorksheetColumn(160));


        //            Worksheet sheet = book.Worksheets.Add("Monthly Report From Concessionaire to Registering Authority");
        //            sheet.Table.Columns.Add(new WorksheetColumn(60));
        //            sheet.Table.Columns.Add(new WorksheetColumn(205));
        //            sheet.Table.Columns.Add(new WorksheetColumn(100));
        //            sheet.Table.Columns.Add(new WorksheetColumn(130));

        //            sheet.Table.Columns.Add(new WorksheetColumn(100));
        //            sheet.Table.Columns.Add(new WorksheetColumn(120));
        //            sheet.Table.Columns.Add(new WorksheetColumn(112));
        //            sheet.Table.Columns.Add(new WorksheetColumn(109));
        //            sheet.Table.Columns.Add(new WorksheetColumn(105));
        //            sheet.Table.Columns.Add(new WorksheetColumn(160));


        //            WorksheetRow row = sheet.Table.Rows.Add();


        //            row.Index = 2;
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //            row.Cells.Add(new WorksheetCell("SCHEDULE-F", "HeaderStyle3"));

        //            row = sheet.Table.Rows.Add();

        //            row.Index = 3;
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //            row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
        //            WorksheetCell cell = row.Cells.Add("Monthly Report From Concessionaire to Registering Authority");
        //            cell.MergeAcross = 3; // Merge two cells together
        //            cell.StyleID = "HeaderStyle3";

        //            row = sheet.Table.Rows.Add();

        //            row.Index = 5;
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //            row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
        //            row.Cells.Add(new WorksheetCell("HARYANA", "HeaderStyle2"));

        //            row = sheet.Table.Rows.Add();
        //            //  Skip one row, and add some text
        //            row.Index = 6;

        //            DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //            row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
        //            row.Cells.Add(new WorksheetCell(dtpFrmDate.Value.ToString("dd/MMM/yyyy") + " - " + dtpToDate.Value.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
        //            row = sheet.Table.Rows.Add();
        //            row.Index = 7;
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //            row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
        //            row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
        //            row = sheet.Table.Rows.Add();



        //            row.Index = 8;
        //            //row.Cells.Add("Order Date");
        //            row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("Laser Number (s) encoded on the HSRP Issued", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("Name of the Vehicle Owner", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("Address of the Vehicle Owner", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("Type and make of vehicle", "HeaderStyle"));
        //            //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("Whether old or new vehicle", "HeaderStyle"));
        //            // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("Date of receipt of Authorization for HSRP", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("Date of receipt of amount from the Owner of the Vehicle", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("Date of Affixation of HSRP to Owner's vehicle", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("Number of old registration plates in Stock at Affixing Station", "HeaderStyle"));
        //            row = sheet.Table.Rows.Add();
        //            String StringField = String.Empty;
        //            String StringAlert = String.Empty;


        //            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
        //            {
        //                sno = sno + 1;
        //                row = sheet.Table.Rows.Add();
        //                row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

        //                row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString() + " - " + dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
        //                row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle1"));
        //                row.Cells.Add(new WorksheetCell(dtrows["Address1"].ToString(), DataType.String, "HeaderStyle1"));
        //                row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString() + "" + dtrows["ManufacturerName"].ToString(), DataType.String, "HeaderStyle1"));
        //                // row.Cells.Add(new WorksheetCell(dtrows["ManufacturerName"].ToString(), DataType.String, "HeaderStyle1"));

        //                row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle1"));
        //                string authdate = (DateTime.Parse(dtrows["HSRPRecord_AuthorizationDate"].ToString())).ToString("dd/MM/yyyy");
        //                row.Cells.Add(new WorksheetCell(authdate, DataType.String, "HeaderStyle1"));
        //                string orderdate = (dtrows["OrderDate"].ToString());
        //                row.Cells.Add(new WorksheetCell(orderdate, DataType.String, "HeaderStyle1"));
        //                if (dtrows["OrderClosedDate"].ToString() != "")
        //                {
        //                    DateTime orderclosedate = DateTime.Parse(dtrows["OrderClosedDate"].ToString());
        //                    row.Cells.Add(new WorksheetCell(orderclosedate.ToString("dd/MM/yyyy"), DataType.String, "HeaderStyle1"));
        //                }
        //                else
        //                {
        //                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
        //                }
        //                if (dtrows["OrderType"].ToString() == "NB" || dtrows["OrderType"].ToString() == "OB" || dtrows["OrderType"].ToString() == "DB")
        //                {
        //                    row.Cells.Add(new WorksheetCell("2", DataType.String, "HeaderStyle1"));
        //                }
        //                else if (dtrows["OrderType"].ToString() == "DR" || dtrows["OrderType"].ToString() == "DF")
        //                {
        //                    row.Cells.Add(new WorksheetCell("1", DataType.String, "HeaderStyle1"));
        //                }
        //                else if (dtrows["OrderType"].ToString() == "OS")
        //                {
        //                    row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle1"));
        //                }
        //                else
        //                {
        //                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
        //                }
        //            }


        //            row = sheet.Table.Rows.Add();

        //            book.Save(strPath + "\\" + filename);



        //        }

        //    }

        //    }

        //    catch (Exception ex)
        //    {

        //    }


        //}

        private void GenerateHRMonthlyReport()
        {

            ExportToPDF_HRMonthlyReport();
        }

        private void ExportToPDF_HRMonthlyReport()
        {
            try
            {
                DataTable dtrto = GetRtoLocation();
                for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
                {
                    string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                    string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                    string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

                    string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

                    // HttpContext context = HttpContext.Current;
                    string filename = "SCHEDULE-F-" + RTOName + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                    string SQLString = String.Empty;
                    Document document = new Document();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                    SetFolder(RTOName, cmbState.Text, "SCHEDULE-F-MonthlyReport");

                    string PdfFolder = strPath + "\\" + filename;
                    PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                    //string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                    //PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    document.Open();
                    PdfPTable table = new PdfPTable(8);
                    var colWidthPercentages = new[] { 25f, 70f, 45f, 50f, 45f, 45f, 60f, 60f };
                    table.SetWidths(colWidthPercentages);
                    //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
                    string SqlQuery = string.Empty;


                    SQLString = "SELECT ROW_NUMBER() over( order by  HSRPRecord_AuthorizationNo) as 'SNo',HSRP_Front_LaserCode as 'FrontLaserNo',HSRP_Rear_LaserCode as 'RearLaserNo', OwnerName,rtrim(ltrim(Address1)) as 'Address1',OrderType as 'OrderType'," +
                           " convert(varchar,OrderDate,103)as 'OrderDate' ,convert(varchar,OrderClosedDate,103)as 'ClosedDate' FROM HSRPRecords   WHERE  HSRP_StateID= '4' AND RTOLocationID='"+ RTOCode +"' and OrderClosedDate  between '" + FromDate + "' and '" + ToDate + "'";
                    DataTable dt = GetDataInDT(SQLString);

                    table.TotalWidth = 780f;
                    table.LockedWidth = true;
                    //  GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);
                    GenerateCell(table, 8, 0, 0, 0, 0, 1, 0, "SCHEDULE-F : Monthly Report From Concessionaire to Registering Authority", 20, 0);
                    GenerateCell(table, 4, 0, 0, 0, 0, 0, 0, "State Name : HARYANA", 20, 0);
                    GenerateCell(table, 4, 0, 0, 0, 0, 0, 0, "Date Period  :" + FromDate + " -  " + ToDate, 15, 0);
                    GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "Report Generation Date :" + System.DateTime.Now.ToString("dd/MMM/yyyy"), 15, 0);
                    GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "", 20, 0);
                    #region Commented Old Name Of Headers
                    //GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 40, 0);
                    //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Laser Number (s) encoded on the HSRP Issued", 40, 0);
                    //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
                    //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Address of the Vehicle Owner", 40, 0);

                    //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Type", 40, 0);

                    //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Whether old or new vehicle", 40, 0);
                    ////
                    //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Date of receipt of amount from the Owner of the Vehicle", 40, 0);
                    //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Date of Affixation of HSRP to Owner's vehicle", 40, 0);
                    //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Number of old registration plates in Stock at Affixing Station", 40, 0);
                    #endregion

                    GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 40, 0);
                    GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front Laser No", 40, 0);
                    GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear Laser No", 40, 0);
                    GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
                    GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Address", 40, 0);

                    GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Order Type", 40, 0);

                    GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Order Date", 40, 0);
                    GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Closed Date", 40, 0);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, dt.Rows[i]["SNo"].ToString(), 20, 0);
                        GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["FrontLaserNo"].ToString(), 20, 0);
                        GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["RearLaserNo"].ToString(), 20, 0);
                        GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OwnerName"].ToString(), 20, 0);
                        GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["Address1"].ToString(), 20, 0);

                        GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OrderType"].ToString(), 20, 0);

                        //GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "New", 20, 0);               

                        GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OrderDate"].ToString(), 20, 0);

                        if (dt.Rows[i]["ClosedDate"].ToString() != "")
                        {
                            GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["ClosedDate"].ToString(), 20, 0);
                        }
                        else
                        {
                            GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "", 20, 0);
                        }
                        #region Commented Code
                        //if (dt.Rows[i]["OrderType"].ToString() == "NB" || dt.Rows[i]["OrderType"].ToString() == "OB" || dt.Rows[i]["OrderType"].ToString() == "DB")
                        //{
                        //    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "2", 20, 0);
                        //}
                        //else if (dt.Rows[i]["OrderType"].ToString() == "DR" || dt.Rows[i]["OrderType"].ToString() == "DF")
                        //{
                        //    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "1", 20, 0);
                        //}
                        //else if (dt.Rows[i]["OrderType"].ToString() == "OS")
                        //{
                        //    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "0", 20, 0);
                        //}
                        //else
                        //{
                        //    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "", 20, 0);
                        //}            
                        #endregion
                    }
                    document.Add(table);
                    document.NewPage();

                    document.Close();
                    //context.Response.ContentType = "Application/pdf";
                    //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    //context.Response.WriteFile(PdfFolder);
                    //context.Response.End();
                }
            }
            catch
            { }

        }

        private void GenerateHREmbossingReport()
        {
            ExportToPDF_HREmbossingReport();
        }

        private void ExportToPDF_HREmbossingReport()
        {
            try
            {
                 DataTable dtrto = GetRtoLocation();
                 for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
                 {
                     string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                     string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                     string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

                     string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";


                     // HttpContext context = HttpContext.Current;
                     string filename = "Schedule-C-HR_Embossing_Report"+RTOName + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                     string SQLString = String.Empty;
                     Document document = new Document();
                     BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                     SetFolder(RTOName, cmbState.Text, "Schedule-C-HR_Embossing_Report");

                     string PdfFolder = strPath + "\\" + filename;
                     PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                     // string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                     // PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                     document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                     document.Open();
                     PdfPTable table = new PdfPTable(10);
                     var colWidthPercentages = new[] { 17f, 80f, 40f, 45f, 40f, 40f, 50f, 50f, 20f, 25f };
                     table.SetWidths(colWidthPercentages);
                     //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
                     string SqlQuery = string.Empty;


                     SQLString = "SELECT  ROW_NUMBER() over( order by  VehicleRegNo) as SNo,  b.ProductColor, a.HSRPRecord_AuthorizationNo as vh1,a.VehicleRegNo as vh2, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '4' AND RTOLocationID='" + RTOCode + "'  AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "'";
                     DataTable dt = GetDataInDT(SQLString);

                     table.TotalWidth = 750f;
                     table.LockedWidth = true;
                     //  GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);

                     GenerateCell(table, 10, 0, 0, 0, 0, 1, 0, "SCHEDULE-C : Daily Report from Embossing Stations to Registering Authority", 20, 0);
                     GenerateCell(table, 2, 0, 0, 0, 0, 1, 0, "State Name : HARYANA", 20, 0);
                     GenerateCell(table, 4, 0, 0, 0, 0, 1, 0, "Date Period  :" + FromDate + "-" + ToDate, 15, 0);
                     GenerateCell(table, 4, 0, 0, 0, 0, 1, 0, "Generated Date time: " + System.DateTime.Now.ToString("dd/MMM/yyyy"), 15, 0);
                     GenerateCell(table, 10, 0, 0, 0, 0, 1, 0, "", 20, 0);
                     GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Application No./Registration No.", 40, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Type", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
                     GenerateCell(table, 2, 0, 1, 1, 1, 1, 0, "Laser Identification No", 20, 0);

                     GenerateCell(table, 2, 0, 1, 1, 1, 1, 0, "RP Size", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "3rd RP Y/N", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Color", 20, 0);

                     GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 40, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear", 20, 0);

                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear", 20, 0);

                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Sticker", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);

                    
                     for (int i = 0; i < dt.Rows.Count; i++)
                     {
                         GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, dt.Rows[i]["SNo"].ToString(), 20, 0);
                         GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["vh1"].ToString() + "/" + dt.Rows[i]["vh2"].ToString(), 30, 0);
                         GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["VehicleType"].ToString(), 20, 0);
                         GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OwnerName"].ToString(), 20, 0);

                         GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["HSRP_Front_LaserCode"].ToString(), 20, 0);
                         GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["HSRP_Rear_LaserCode"].ToString(), 20, 0);

                         GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["FrontPlateSize"].ToString(), 20, 0);
                         GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["RearPlateSize1"].ToString(), 20, 0);

                         GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["StickerMandatory"].ToString(), 20, 0);

                         GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["ProductColor"].ToString(), 20, 0);

                    


                     }
                     document.Add(table);
                     document.NewPage();

                     document.Close();
                     //context.Response.ContentType = "Application/pdf";
                     //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    // context.Response.WriteFile(PdfFolder);
                     //context.Response.End();
                 }
            }
            catch
            {
            
            }


        }

        //private void GenerateHREmbossingReport()
        //{
        //    try
        //    {
        //         DataTable dtrto = GetRtoLocation();
        //         for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
        //         {
        //             string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
        //             string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
        //             int sno = 0;
        //             DataTable dt = new DataTable();
        //             string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

        //             string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

        //             string SQLString = "SELECT  ROW_NUMBER() over( order by  VehicleRegNo) as SNo,  b.ProductColor, a.HSRPRecord_AuthorizationNo as vh1,a.VehicleRegNo as vh2, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '4'  and RTOLocationID='" + RTOCode + "' AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "'";
        //             dt = GetDataInDT(SQLString);


        //             string RTOColName = string.Empty;

        //             if (dt.Rows.Count > 0)
        //             {



        //                 string filename = "Schedule-C-HR_Embossing_Report" + RTOName + "_" + strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
        //                 SetFolder("", cmbState.Text, "Schedule-C-HR_Embossing_Report" + "_" + strMonth + strYear + "_");
        //                 Workbook book = new Workbook();

        //                 // Specify which Sheet should be opened and the size of window by default
        //                 book.ExcelWorkbook.ActiveSheetIndex = 1;
        //                 book.ExcelWorkbook.WindowTopX = 100;
        //                 book.ExcelWorkbook.WindowTopY = 200;
        //                 book.ExcelWorkbook.WindowHeight = 7000;
        //                 book.ExcelWorkbook.WindowWidth = 8000;

        //                 // Some optional properties of the Document
        //                 book.Properties.Author = "HSRP";
        //                 book.Properties.Title = "Daily Embossing Report";
        //                 book.Properties.Created = DateTime.Now;


        //                 // Add some styles to the Workbook
        //                 WorksheetStyle style = book.Styles.Add("HeaderStyle");
        //                 style.Font.FontName = "Tahoma";
        //                 style.Font.Size = 10;
        //                 style.Font.Bold = true;
        //                 style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //                 style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //                 style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //                 style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //                 style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

        //                 WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
        //                 style4.Font.FontName = "Tahoma";
        //                 style4.Font.Size = 10;
        //                 style4.Font.Bold = false;
        //                 style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //                 style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //                 style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //                 style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //                 style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //                 WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
        //                 style6.Font.FontName = "Tahoma";
        //                 style6.Font.Size = 10;
        //                 style6.Font.Bold = true;
        //                 style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //                 style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //                 style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //                 style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //                 style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //                 WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
        //                 style8.Font.FontName = "Tahoma";
        //                 style8.Font.Size = 10;
        //                 style8.Font.Bold = true;
        //                 style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //                 style8.Interior.Color = "#D4CDCD";
        //                 style8.Interior.Pattern = StyleInteriorPattern.Solid;

        //                 WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
        //                 style5.Font.FontName = "Tahoma";
        //                 style5.Font.Size = 10;
        //                 style5.Font.Bold = true;
        //                 style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //                 style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //                 style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //                 style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //                 style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //                 WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
        //                 style2.Font.FontName = "Tahoma";
        //                 style2.Font.Size = 10;
        //                 style2.Font.Bold = true;
        //                 style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


        //                 WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
        //                 style3.Font.FontName = "Tahoma";
        //                 style3.Font.Size = 12;
        //                 style3.Font.Bold = true;
        //                 style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

        //                 Worksheet sheet = book.Worksheets.Add("Data Embossing Report");
        //                 sheet.Table.Columns.Add(new WorksheetColumn(60));
        //                 sheet.Table.Columns.Add(new WorksheetColumn(205));
        //                 sheet.Table.Columns.Add(new WorksheetColumn(100));
        //                 sheet.Table.Columns.Add(new WorksheetColumn(130));

        //                 sheet.Table.Columns.Add(new WorksheetColumn(100));
        //                 sheet.Table.Columns.Add(new WorksheetColumn(120));
        //                 sheet.Table.Columns.Add(new WorksheetColumn(112));
        //                 sheet.Table.Columns.Add(new WorksheetColumn(109));
        //                 sheet.Table.Columns.Add(new WorksheetColumn(105));
        //                 sheet.Table.Columns.Add(new WorksheetColumn(160));

        //                 WorksheetRow row = sheet.Table.Rows.Add();
        //                 // row.
        //                 //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //                 //row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
        //                 //WorksheetCell cell = row.Cells.Add("SCHEDULE-C : Daily Report from Embossing Stations to Registering Authority");
        //                 //cell.MergeAcross = 3; // Merge two cells together
        //                 //cell.StyleID = "HeaderStyle3";

        //                 row.Index = 1;
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
        //                 WorksheetCell cell5 = row.Cells.Add("SCHEDULE-C ");
        //                 cell5.MergeAcross = 3; // Merge two cells togetherto 
        //                 cell5.StyleID = "HeaderStyle6";

        //                 row = sheet.Table.Rows.Add();


        //                 row.Index = 2;
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //                 WorksheetCell cell = row.Cells.Add("Daily Report from Embossing Stations to Registering Authority");
        //                 cell.MergeAcross = 3; // Merge two cells togetherto 
        //                 cell.StyleID = "HeaderStyle3";

        //                 row = sheet.Table.Rows.Add();


        //                 row.Index = 3;
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //                 row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
        //                 row.Cells.Add(new WorksheetCell("HARYANA", "HeaderStyle2"));

        //                 row = sheet.Table.Rows.Add();
        //                 //  Skip one row, and add some text
        //                 row.Index = 4;

        //                 DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //                 row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
        //                 row.Cells.Add(new WorksheetCell(FromDate.ToString() + " - " + ToDate.ToString(), "HeaderStyle2"));
        //                 row = sheet.Table.Rows.Add();
        //                 row.Index = 5;
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //                 row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
        //                 row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
        //                 row = sheet.Table.Rows.Add();



        //                 row.Index = 6;
        //                 //row.Cells.Add("Order Date");
        //                 row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle6"));
        //                 row.Cells.Add(new WorksheetCell("Application No./Registration No.", "HeaderStyle6"));
        //                 row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle6"));
        //                 row.Cells.Add(new WorksheetCell("Owners Name", "HeaderStyle6"));
        //                 //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //                 WorksheetCell cell1 = row.Cells.Add("Laser Identification No");
        //                 cell1.MergeAcross = 1; // Merge two cells together
        //                 cell1.StyleID = "HeaderStyle6";
        //                 //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
        //                 // row.Cells.Add(new WorksheetCell("RP Size", "HeaderStyle"));

        //                 WorksheetCell cell2 = row.Cells.Add("RP Size");
        //                 cell2.MergeAcross = 1; // Merge two cells together
        //                 cell2.StyleID = "HeaderStyle6";
        //                 // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
        //                 row.Cells.Add(new WorksheetCell("3rd RP Y/N", "HeaderStyle6"));
        //                 // row.Cells.Add(new WorksheetCell("Colour", "HeaderStyle"));
        //                 WorksheetCell cell3 = row.Cells.Add("Colour");
        //                 cell3.MergeAcross = 1; // Merge two cells together
        //                 cell3.StyleID = "HeaderStyle6";
        //                 row = sheet.Table.Rows.Add();
        //                 String StringField = String.Empty;
        //                 String StringAlert = String.Empty;


        //                 row.Index = 7;
        //                 //row.Cells.Add("Order Date");
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //                 row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //                 row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
        //                 row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
        //                 //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
        //                 row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
        //                 row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
        //                 // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
        //                 row.Cells.Add(new WorksheetCell("Sticker", "HeaderStyle"));
        //                 row.Cells.Add(new WorksheetCell("Yellow", "HeaderStyle6"));
        //                 row.Cells.Add(new WorksheetCell("White", "HeaderStyle6"));
        //                 row = sheet.Table.Rows.Add();
        //                 //String StringField = String.Empty;
        //                 //String StringAlert = String.Empty;


        //                 if (dt.Rows.Count > 0)
        //                 {
        //                     foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
        //                     {
        //                         sno = sno + 1;
        //                         row = sheet.Table.Rows.Add();
        //                         row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

        //                         row.Cells.Add(new WorksheetCell(dtrows["vh1"].ToString() + "/" + dtrows["vh2"].ToString(), DataType.String, "HeaderStyle1"));
        //                         row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle1"));
        //                         row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle1"));
        //                         row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
        //                         row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));

        //                         row.Cells.Add(new WorksheetCell(dtrows["FrontPlateSize"].ToString(), DataType.String, "HeaderStyle1"));
        //                         row.Cells.Add(new WorksheetCell(dtrows["RearPlateSize1"].ToString(), DataType.String, "HeaderStyle1"));
        //                         row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle1"));
        //                         if (dtrows["ProductColor"].ToString() == "Yellow")
        //                         {
        //                             row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
        //                             row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
        //                         }
        //                         else
        //                         {
        //                             row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
        //                             row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
        //                         }
        //                     }





        //                     book.Save(strPath + "\\" + filename);
        //                 }
        //             }
        //         }
        //    }


        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        
        //--------------------- For Excel Report Generate Gautam 020216 For Haryana State-----------------

        private void GenerateHRAuthorityReport()
        {

            try
            {
                string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

                string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

                DataTable dt = new DataTable();
                string SQLString = "exec Business_DateYearMonthWise_Collection_Summary '2015/06/22',4,B";
               // string SQLString = "exec Report_Monthly_StatementLocationWiseRegisteringAuthority '2014-10-10 00:00:00','2014-10-20 23:59:59','"+cmbState.SelectedValue.ToString()+"',482";
                DataTable ds = GetDataInDT(SQLString);

                string RTOColName = string.Empty;
                int sno = 0;


                if (ds.Rows.Count > 0)
                {
                    string filename = "Collection_V_S_Deposit_HR" + "_" + strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();
                SetFolder("", cmbState.Text, "Collection_V_S_Deposit_HR" + "_" + strMonth + strYear + "_");
                // Specify which Sheet should be opened and the size of window by default

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily Entry Report";
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
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style15 = book.Styles.Add("HeaderStyle15");
                style15.Font.FontName = "Tahoma";
                style15.Font.Size = 10;
                style15.Font.Bold = false;
                style15.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style15.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style15.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style15.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style15.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                style8.Font.FontName = "Tahoma";
                style8.Font.Size = 10;
                style8.Font.Bold = false;
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style8.Interior.Color = "#D4CDCD";
                style8.Interior.Pattern = StyleInteriorPattern.Solid;

                WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                style5.Font.FontName = "Tahoma";
                style5.Font.Size = 10;
                style5.Font.Bold = true;
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

                Worksheet sheet = book.Worksheets.Add("Collection_V_S_Deposit_HR");
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
                // row.


                row.Index = 1;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
               // WorksheetCell cell5 = row.Cells.Add("SCHEDULE-D ");
              //  cell5.MergeAcross = 3; // Merge two cells togetherto 
               // cell5.StyleID = "HeaderStyle6";

                row = sheet.Table.Rows.Add();


                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                WorksheetCell cell = row.Cells.Add("Cumulative Collection and Deposit Report");
                cell.MergeAcross = 3; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();



                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("HARYANA", "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                // Skip one row, and add some text

                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Datetime:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Month", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dtpFrmDate.Value.ToString("dd/MMM/yyyy") + " - " + dtpToDate.Value.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();



                row.Index = 6;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                //WorksheetCell cell2 = row.Cells.Add("Applicaton Received");
               // cell2.MergeAcross = 1; // Merge two cells togetherto 
               // cell2.StyleID = "HeaderStyle6";

              //  WorksheetCell cell3 = row.Cells.Add("Registration Plates Supplied");
              //  cell3.MergeAcross = 1; // Merge two cells togetherto 
              //  cell3.StyleID = "HeaderStyle6";

                //WorksheetCell cell4 = row.Cells.Add("Back log (if any)");
                //cell4.MergeAcross = 1; // Merge two cells togetherto 
                //cell4.StyleID = "HeaderStyle6";

              //  row.Cells.Add(new WorksheetCell("Back log (if any) ", "HeaderStyle"));

                row = sheet.Table.Rows.Add();



                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("Sr.No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Zone", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("District", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Location", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Orders ", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Opening Cash Balance", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Collection ", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Deposit ", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Balance Cash ", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Day1 ", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Day2 ", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Day3 ", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Day4 ", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Day5 ", "HeaderStyle6"));
            //    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("O", "HeaderStyle6"));
                row = sheet.Table.Rows.Add();

                foreach (DataRow dtrows in ds.Rows) // Loop over the rows.
                {
                    sno = sno + 1;
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                    // row.Cells.Add(new WorksheetCell(dtrows["id"].ToString(), DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell(dtrows["district"].ToString(), DataType.String, "HeaderStyle15"));
                    row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle15"));
                    row.Cells.Add(new WorksheetCell(dtrows["Orders"].ToString(), DataType.Number, "HeaderStyle15"));

                    //row.Cells.Add(new WorksheetCell(dtrows["Opening Cash Balance"].ToString(), DataType.Number, "HeaderStyle15"));
                    row.Cells.Add(new WorksheetCell(dtrows["Opening Cash Balance"].ToString(), DataType.Number, "HeaderStyle15"));
                    row.Cells.Add(new WorksheetCell(dtrows["Collection"].ToString(), DataType.Number, "HeaderStyle15"));
                    row.Cells.Add(new WorksheetCell(dtrows["Deposit"].ToString(), DataType.Number, "HeaderStyle15"));
                    row.Cells.Add(new WorksheetCell(dtrows["Balance Cash"].ToString(), DataType.Number, "HeaderStyle15"));
                    row.Cells.Add(new WorksheetCell(dtrows["Day1"].ToString(), DataType.Number, "HeaderStyle15"));
                    row.Cells.Add(new WorksheetCell(dtrows["Day2"].ToString(), DataType.Number, "HeaderStyle15"));
                    row.Cells.Add(new WorksheetCell(dtrows["Day3"].ToString(), DataType.Number, "HeaderStyle15"));
                    row.Cells.Add(new WorksheetCell(dtrows["Day4"].ToString(), DataType.Number, "HeaderStyle15"));
                    row.Cells.Add(new WorksheetCell(dtrows["Day5 Or More"].ToString(), DataType.Number, "HeaderStyle15"));
                   
                    //int sum = int.Parse(dtrows["newlog"].ToString()) + int.Parse(dtrows["oldlog"].ToString());
                   // row.Cells.Add(new WorksheetCell(sum.ToString(), DataType.Number, "HeaderStyle15"));
                    //row.Cells.Add(new WorksheetCell(dtrows["oldlog"].ToString(), DataType.Number, "HeaderStyle15"));
                }
                row = sheet.Table.Rows.Add();

                row = sheet.Table.Rows.Add();                                

               book.Save(strPath + "\\" + filename);



                }



            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }





        private void GenerateHRSheduleEReport()
        {

            string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

            string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";
                 DataTable dtrto = GetRtoLocation();
                 for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
                 {
                     string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                     string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();


                     Int32 totalNewscnaplock1 = 0;
                     Int32 totaloldscnaplock1 = 0;
                     Int32 totalReplacementScnaplock1 = 0;

                     Decimal totalPriceNewscnaplock1 = 0;

                     Decimal totalPriceOldcnaplock1 = 0;

                     Decimal totalPriceReplacementcnaplock1 = 0;
                     string VehicleType = string.Empty;
                     Int32 Total1 = 0;
                     int sno = 0;
                     Int32 totalNewPrice1 = 0;
                     Int32 totalOldPrice1 = 0;
                     Int32 ReplacementPrice1 = 0;
                     // int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                     string filename = "MonthlyStatementFromConcessionaireToRegisteringAuthority-MonthWise" + RTOName + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                     string SQLString = String.Empty;
                     Document document = new Document();
                     //HttpContext context = HttpContext.Current;
                     BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                     SetFolder(RTOName, cmbState.Text, "SCHEDULE-E-MonthlyReport");

                     string PdfFolder = strPath + "\\" + filename;
                     PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));


                     //string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                     //PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                     document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                     document.Open();
                     PdfPTable table = new PdfPTable(9);
                     var colWidthPercentages = new[] { 10f, 95f, 20f, 25f, 25f, 35f, 25f, 35f, 15f };
                     table.SetWidths(colWidthPercentages);
                     SQLString = "Report_Monthly_StatementLocationWiseRegisteringAuthority'" + FromDate + "','" + ToDate + "','" + cmbState.SelectedValue.ToString() + "','" + RTOCode + "'";
                     DataTable ds = GetDataInDT(SQLString);


                     GenerateCell(table, 9, 0, 0, 0, 0, 1, 0, "SCHEDULE-E :  Monthly Statement From Concessionaire To Registering Authority", 15, 0);
                     GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "State:      " + cmbState.SelectedItem.ToString(), 15, 0);
                     GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "RTO Name:      " + RTOName, 15, 0);
                     //GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "Month:      " + ddlMonth.SelectedItem.ToString() + "," + ddlYear.SelectedValue, 15, 0);
                     GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "Report Generated Date :      " + System.DateTime.Now.ToString("dd/MM/yyyy"), 15, 0);

                     GenerateCell(table, 9, 0, 0, 0, 0, 1, 0, "", 20, 0);
                     GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Category", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Number of new Vehicles Affixed", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Price Charged", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Number of existing Vehicles Affixed", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Price Charged", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Replacements", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Price Charged", 20, 0);
                     GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Total Amount", 20, 0);

                     for (int i = 0; i < ds.Rows.Count; i++)
                     {
                         VehicleType = ds.Rows[i]["vehicleandplatetype"].ToString();
                         sno = i + 1;
                         totalNewPrice1 = Convert.ToInt32(ds.Rows[i]["newvehiclecash"].ToString());
                         totalOldPrice1 = Convert.ToInt32(ds.Rows[i]["oldvehiclecash"].ToString());
                         ReplacementPrice1 = Convert.ToInt32(ds.Rows[i]["replaceveihclecash"].ToString());
                         Total1 = totalNewPrice1 + totalOldPrice1 + ReplacementPrice1;
                         GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, sno.ToString(), 20, 0);

                         if (VehicleType == "2W")
                         {
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Complete Set of Registration Plates Inclusive of Snap Lock and fixing for 2 Wheelers- Schooters, Motorcycles, Mopeds", 60, 0);


                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);


                         }
                         if (VehicleType == "3W")
                         {
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Complete Set of Registration Plates Inclusive of Snap Lock, 3rd Registration Plate and fixing for 3 wheelers and invalid carriages.", 60, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                         }
                         if (VehicleType == "4W")
                         {

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Complete Set of Registration Plates Inclusive of Snap Lock 3rd Registration Plate and fixing for Light Motor Vehicles/Passenger Cars.", 60, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);


                         }

                         if (VehicleType == "4HW")
                         {
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Complete Set of Registration Plates Inclusive of Snap Lock 3rd Registration Plate and fixing for Medium Commercial Vehicles/Havy Commercial Vehicles/Trailer/combination/others.", 60, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);

                         }
                         if (VehicleType == "500X120")
                         {

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Registration Plate Size 500X120 mm", 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                             totalNewscnaplock1 = +totalNewscnaplock1 + Convert.ToInt32(ds.Rows[i]["newvehicleorder"]);
                             totaloldscnaplock1 = +totaloldscnaplock1 + Convert.ToInt32(ds.Rows[i]["oldvehicleorder"]);
                             totalReplacementScnaplock1 = +totalReplacementScnaplock1 + Convert.ToInt32(ds.Rows[i]["replacevehicle"]);

                         }
                         if (VehicleType == "340X200")
                         {
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Registration Plate Size 300X200 mm", 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                             totalNewscnaplock1 = +totalNewscnaplock1 + Convert.ToInt32(ds.Rows[i]["newvehicleorder"]);
                             totaloldscnaplock1 = totaloldscnaplock1 + Convert.ToInt32(ds.Rows[i]["oldvehicleorder"]);
                             totalReplacementScnaplock1 = +totalReplacementScnaplock1 + Convert.ToInt32(ds.Rows[i]["replacevehicle"]);

                         }
                         if (VehicleType == "200X100")
                         {
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Registration Plate Size 200X100 mm", 20, 0);

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                             totalNewscnaplock1 = +totalNewscnaplock1 + Convert.ToInt32(ds.Rows[i]["newvehicleorder"]);
                             totaloldscnaplock1 = totaloldscnaplock1 + Convert.ToInt32(ds.Rows[i]["oldvehicleorder"]);
                             totalReplacementScnaplock1 = +totalReplacementScnaplock1 + Convert.ToInt32(ds.Rows[i]["replacevehicle"]);
                         }
                         if (VehicleType == "285X45")
                         {
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Registration Plate Size 285X45 mm", 20, 0);

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                             totalNewscnaplock1 = +totalNewscnaplock1 + Convert.ToInt32(ds.Rows[i]["newvehicleorder"]);
                             totaloldscnaplock1 = totaloldscnaplock1 + Convert.ToInt32(ds.Rows[i]["oldvehicleorder"]);
                             totalReplacementScnaplock1 = +totalReplacementScnaplock1 + Convert.ToInt32(ds.Rows[i]["replacevehicle"]);
                         }
                         if (VehicleType == "Sticker")
                         {
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, " 3rd Registration Plate Sticker Inclusive of Printing", 30, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                             GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);

                         }
                     
                     decimal snapLockRate = 0;
                     SQLString = "SELECT ProductCost.Cost FROM  Product  INNER JOIN ProductCost ON Product.ProductID = ProductCost.ProductID where Product.productcode ='SNAP LOCK' and ProductCost.hsrp_stateID='" + cmbState.SelectedValue.ToString() + "'";
                     DataTable dsRate = GetDataInDT(SQLString);
                     if (dsRate.Rows.Count > 0)
                     {
                         snapLockRate = Convert.ToDecimal(dsRate.Rows[0]["cost"]);
                     }
                     sno = sno + 1;
                     GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, sno.ToString(), 20, 0);
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Snap Locks.", 20, 0);

                     totalNewscnaplock1 = 2 * totalNewscnaplock1;
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, totalNewscnaplock1.ToString(), 20, 0);

                     totalPriceNewscnaplock1 = Convert.ToInt32(totalNewscnaplock1) * Convert.ToDecimal(snapLockRate);
                     totalPriceNewscnaplock1 = System.Decimal.Round(totalPriceNewscnaplock1, 0);
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, totalPriceNewscnaplock1.ToString(), 20, 0);

                     totaloldscnaplock1 = 2 * totaloldscnaplock1;
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, totaloldscnaplock1.ToString(), 20, 0);

                     totalPriceOldcnaplock1 = totaloldscnaplock1 * Convert.ToDecimal(snapLockRate);
                     totalPriceOldcnaplock1 = System.Decimal.Round(totalPriceOldcnaplock1, 0);
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, totalPriceOldcnaplock1.ToString(), 20, 0);



                     totalReplacementScnaplock1 = 2 * totalReplacementScnaplock1;
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, totalReplacementScnaplock1.ToString(), 20, 0);


                     totalPriceReplacementcnaplock1 = totalReplacementScnaplock1 * Convert.ToDecimal(snapLockRate);
                     totalPriceReplacementcnaplock1 = System.Decimal.Round(totalPriceReplacementcnaplock1, 0);
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, totalPriceReplacementcnaplock1.ToString(), 20, 0);


                     decimal TotalSnapLocak = totalPriceNewscnaplock1 + totalPriceOldcnaplock1 + totalPriceReplacementcnaplock1;
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, TotalSnapLocak.ToString(), 20, 0);
                     }

                     document.Add(table);
                     document.NewPage();

                     document.Close();
                 }
        
        }

        //private void GenerateHRSheduleEReport(string strRTOId,string strRTOLocation)
        //{

        //    try
        //    {                

        //        DataTable dt = new DataTable();
        //        //string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

        //       // string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

        //        string SQLString = "Report_Monthly_StatementLocationWiseRegisteringAuthority'" + dtpFrmDate.Value.ToString("yyyy/MM/dd") + "','" + dtpToDate.Value.ToString("yyyy/MM/dd") + "','" + cmbState.SelectedValue + "','" + strRTOId + "'";
        //        DataTable ds = GetDataInDT(SQLString);

        //        string RTOColName = string.Empty;
        //        int sno = 0;

        //        if (ds.Rows.Count > 0)
        //        {

        //            string filename = "HRSchedule-E-" + strRTOLocation + "_" + strMonth + strYear + "_"  + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
        //        Workbook book = new Workbook();
        //        SetFolder(strRTOLocation, cmbState.Text, "HRSchedule-E-" + "_" + strMonth + strYear + "_");
        //        // Specify which Sheet should be opened and the size of window by default
        //        book.ExcelWorkbook.ActiveSheetIndex = 1;
        //        book.ExcelWorkbook.WindowTopX = 100;
        //        book.ExcelWorkbook.WindowTopY = 200;
        //        book.ExcelWorkbook.WindowHeight = 7000;
        //        book.ExcelWorkbook.WindowWidth = 8000;

        //        // Some optional properties of the Document
        //        book.Properties.Author = "HSRP";
        //        book.Properties.Title = "Monthly Statement Form Concessionaire To Registering Authority";
        //        book.Properties.Created = DateTime.Now;

        //        // Add some styles to the Workbook
        //        WorksheetStyle style = book.Styles.Add("HeaderStyle");
        //        style.Font.FontName = "Tahoma";
        //        style.Font.Size = 10;
        //        style.Font.Bold = true;
        //        style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //        style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //        style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //        style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //        style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

        //        WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
        //        style4.Font.FontName = "Tahoma";
        //        style4.Font.Size = 10;
        //        style4.Font.Bold = false;
        //        style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //        style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //        style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //        style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //        style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //        WorksheetStyle style15 = book.Styles.Add("HeaderStyle15");
        //        style15.Font.FontName = "Tahoma";
        //        style15.Font.Size = 10;
        //        style15.Font.Bold = false;
        //        style15.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //        style15.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //        style15.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //        style15.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //        style15.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //        WorksheetStyle style16 = book.Styles.Add("HeaderStyle16");
        //        style16.Font.FontName = "Tahoma";
        //        style16.Font.Size = 10;
        //        style16.Font.Bold = false;
        //        style16.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //        style16.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //        style16.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //        style16.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //        style16.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //        WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
        //        style8.Font.FontName = "Tahoma";
        //        style8.Font.Size = 10;
        //        style8.Font.Bold = false;
        //        style8.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //        style8.Interior.Color = "#D4CDCD";
        //        style8.Interior.Pattern = StyleInteriorPattern.Solid;

        //        WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
        //        style5.Font.FontName = "Tahoma";
        //        style5.Font.Size = 10;
        //        style5.Font.Bold = true;
        //        style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //        style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //        style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //        style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //        style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //        WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
        //        style2.Font.FontName = "Tahoma";
        //        style2.Font.Size = 10;
        //        style2.Font.Bold = true;
        //        style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


        //        WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
        //        style3.Font.FontName = "Tahoma";
        //        style3.Font.Size = 12;
        //        style3.Font.Bold = true;
        //        style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

        //        Worksheet sheet = book.Worksheets.Add("Monthly Statement From Concessionaire To Registering Authority");
        //        sheet.Table.Columns.Add(new WorksheetColumn(60));
        //        sheet.Table.Columns.Add(new WorksheetColumn(205));
        //        sheet.Table.Columns.Add(new WorksheetColumn(100));
        //        sheet.Table.Columns.Add(new WorksheetColumn(130));

        //        sheet.Table.Columns.Add(new WorksheetColumn(100));
        //        sheet.Table.Columns.Add(new WorksheetColumn(120));
        //        sheet.Table.Columns.Add(new WorksheetColumn(112));
        //        sheet.Table.Columns.Add(new WorksheetColumn(109));
        //        sheet.Table.Columns.Add(new WorksheetColumn(105));
        //        sheet.Table.Columns.Add(new WorksheetColumn(160));

        //        WorksheetRow row = sheet.Table.Rows.Add();

        //        row.Index = 2;
        //        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //        WorksheetCell cellA = row.Cells.Add("SCHEDULE-E");
        //        cellA.MergeAcross = 3; // Merge two cells togetherto 
        //        cellA.StyleID = "HeaderStyle3";

        //        row = sheet.Table.Rows.Add();


        //        row.Index = 4;
        //        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //        WorksheetCell cell = row.Cells.Add("Monthly Statement From Concessionaire To Registering Authority");
        //        cell.MergeAcross = 3; // Merge two cells togetherto 
        //        cell.StyleID = "HeaderStyle3";

        //        row = sheet.Table.Rows.Add();


        //        row.Index = 5;
        //        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //        row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
        //        row.Cells.Add(new WorksheetCell(cmbState.Text, "HeaderStyle2"));
        //        row.Cells.Add(new WorksheetCell("RTO Location:", "HeaderStyle2"));
        //        row.Cells.Add(new WorksheetCell(strRTOLocation, "HeaderStyle2"));

        //        row = sheet.Table.Rows.Add();
        //        // Skip one row, and add some text
        //        row.Index = 6;

        //        DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        //        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //        row.Cells.Add(new WorksheetCell("Generated Datetime:", "HeaderStyle2"));
        //        row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
        //        row = sheet.Table.Rows.Add();
        //        row.Index = 7;
        //        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //        row.Cells.Add(new WorksheetCell("Report Month", "HeaderStyle2"));
        //        row.Cells.Add(new WorksheetCell(dtpFrmDate.Value.ToString("dd/MMM/yyyy") + "-" + dtpToDate.Value.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
        //        row = sheet.Table.Rows.Add();



        //        // row.Index = 7; 


        //        row.Index = 9;
        //        //row.Cells.Add("Order Date");
        //        row.Cells.Add(new WorksheetCell("Sr.No.", "HeaderStyle"));
        //        row.Cells.Add(new WorksheetCell("Category", "HeaderStyle"));
        //        row.Cells.Add(new WorksheetCell("No. of New Vehicle Affixed", "HeaderStyle"));
        //        row.Cells.Add(new WorksheetCell("Price Charged", "HeaderStyle"));
        //        row.Cells.Add(new WorksheetCell("No. of Existing Vehicles Affixed", "HeaderStyle"));
        //        row.Cells.Add(new WorksheetCell("Price Charged", "HeaderStyle"));
        //        row.Cells.Add(new WorksheetCell("Replacements", "HeaderStyle"));
        //        row.Cells.Add(new WorksheetCell("Price Charged", "HeaderStyle"));
        //        row.Cells.Add(new WorksheetCell("Total Amount", "HeaderStyle"));
        //        row = sheet.Table.Rows.Add();

        //        String StringField = String.Empty;
        //        String StringAlert = String.Empty;


        //            Int32 totalNewscnaplock = 0;
        //            Int32 totaloldscnaplock = 0;
        //            Int32 totalReplacementScnaplock = 0;

        //            Decimal totalPriceNewscnaplock = 0;

        //            Decimal totalPriceOldcnaplock = 0;

        //            Decimal totalPriceReplacementcnaplock = 0;

        //            Int32 Total = 0;

        //            Int32 totalNewPrice = 0;
        //            Int32 totalOldPrice = 0;
        //            Int32 ReplacementPrice = 0;

        //            foreach (DataRow dtrows in ds.Rows) // Loop over the rows.
        //            {
        //                sno = sno + 1;
        //                string Type = dtrows["vehicleandplatetype"].ToString();
        //                row = sheet.Table.Rows.Add();


        //                totalNewPrice = Convert.ToInt32(dtrows["newvehiclecash"].ToString());
        //                totalOldPrice = Convert.ToInt32(dtrows["oldvehiclecash"].ToString());
        //                ReplacementPrice = Convert.ToInt32(dtrows["replaceveihclecash"].ToString());

        //                row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

        //                Total = totalNewPrice + totalOldPrice + ReplacementPrice;


        //                if (Type == "2W")
        //                {
        //                    row.Cells.Add(new WorksheetCell("Complete Set of Registration Plates Inclusive of Snap Lock and fixing for 2 Wheelers- Schooters, Motorcycles, Mopeds", DataType.String, "HeaderStyle1"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
        //                }
        //                if (Type == "3W")
        //                {
        //                    row.Cells.Add(new WorksheetCell("Complete Set of Registration Plates Inclusive of Snap Lock, 3rd Registration Plate and fixing for 3 wheelers and invalid carriages.", DataType.String, "HeaderStyle1"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
        //                }
        //                if (Type == "4W")
        //                {
        //                    row.Cells.Add(new WorksheetCell("Complete Set of Registration Plates Inclusive of Snap Lock 3rd Registration Plate and fixing for Light Motor Vehicles/Passenger Cars, Mopeds", DataType.String, "HeaderStyle1"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
        //                }
        //                if (Type == "4HW")
        //                {
        //                    row.Cells.Add(new WorksheetCell("Complete Set of Registration Plates Inclusive of Snap Lock 3rd Registration Plate and fixing for Medium Commercial Vehicles/Havy Commercial Vehicles/Trailer/combination/others.", DataType.String, "HeaderStyle1"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
        //                }
        //                if (Type == "500X120")
        //                {
        //                    row.Cells.Add(new WorksheetCell("Registration Plate Size 500X100 mm", DataType.String, "HeaderStyle1"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));

        //                    totalNewscnaplock = +totalNewscnaplock + Convert.ToInt32(dtrows["newvehicleorder"]);
        //                    totaloldscnaplock = +totaloldscnaplock + Convert.ToInt32(dtrows["oldvehicleorder"]);
        //                    totalReplacementScnaplock = +totalReplacementScnaplock + Convert.ToInt32(dtrows["replacevehicle"]);

        //                }
        //                if (Type == "340X200")
        //                {
        //                    row.Cells.Add(new WorksheetCell("Registration Plate Size 300X200 mm", DataType.String, "HeaderStyle1"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));

        //                    totalNewscnaplock = +totalNewscnaplock + Convert.ToInt32(dtrows["newvehicleorder"]);
        //                    totaloldscnaplock = totaloldscnaplock + Convert.ToInt32(dtrows["oldvehicleorder"]);
        //                    totalReplacementScnaplock = +totalReplacementScnaplock + Convert.ToInt32(dtrows["replacevehicle"]);
        //                }
        //                if (Type == "200X100")
        //                {
        //                    row.Cells.Add(new WorksheetCell("Registration Plate Size 200X100 mm", DataType.String, "HeaderStyle1"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));

        //                    totalNewscnaplock = +totalNewscnaplock + Convert.ToInt32(dtrows["newvehicleorder"]);
        //                    totaloldscnaplock = totaloldscnaplock + Convert.ToInt32(dtrows["oldvehicleorder"]);
        //                    totalReplacementScnaplock = +totalReplacementScnaplock + Convert.ToInt32(dtrows["replacevehicle"]);
        //                }
        //                if (Type == "285X45")
        //                {
        //                    row.Cells.Add(new WorksheetCell("Registration Plate Size 285X45 mm", DataType.String, "HeaderStyle1"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));

        //                    totalNewscnaplock = +totalNewscnaplock + Convert.ToInt32(dtrows["newvehicleorder"]);
        //                    totaloldscnaplock = +totaloldscnaplock + Convert.ToInt32(dtrows["oldvehicleorder"]);
        //                    totalReplacementScnaplock = +totalReplacementScnaplock + Convert.ToInt32(dtrows["replacevehicle"]);
        //                }
        //                if (Type == "Sticker")
        //                {
        //                    row.Cells.Add(new WorksheetCell("3rd Registration Plate Sticker Inclusive of Printing", DataType.String, "HeaderStyle1"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
        //                    row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
        //                    //row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
        //                }
        //            }
        //            row = sheet.Table.Rows.Add();
        //            decimal snapLockRate = 0;
        //            //SQLString = "Report_Monthly_Statement_Registering_Authority'" + FromDate + "','" + ToDate + "','" + intHSRPStateID + "','0'";
        //            SQLString = "SELECT ProductCost.Cost FROM  Product  INNER JOIN ProductCost ON Product.ProductID = ProductCost.ProductID where Product.productcode ='SNAP LOCK' and ProductCost.hsrp_stateID='" + cmbState.SelectedValue + "'";
        //            DataTable dsRate = GetDataInDT(SQLString);
        //            if (dsRate.Rows.Count > 0)
        //            {
        //                snapLockRate = Convert.ToDecimal(dsRate.Rows[0]["cost"]);
        //            }

        //            sno = sno + 1;
        //            row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
        //            row.Cells.Add(new WorksheetCell("Snap Locks.", DataType.String, "HeaderStyle1"));
        //            totalNewscnaplock = 2 * totalNewscnaplock;
        //            row.Cells.Add(new WorksheetCell(totalNewscnaplock.ToString(), DataType.Number, "HeaderStyle16"));
        //            totalPriceNewscnaplock = Convert.ToInt32(totalNewscnaplock) * Convert.ToDecimal(snapLockRate);
        //            totalPriceNewscnaplock = System.Decimal.Round(totalPriceNewscnaplock, 0);
        //            row.Cells.Add(new WorksheetCell(totalPriceNewscnaplock.ToString(), DataType.Number, "HeaderStyle16"));

        //            totaloldscnaplock = 2 * totaloldscnaplock;
        //            row.Cells.Add(new WorksheetCell(totaloldscnaplock.ToString(), DataType.Number, "HeaderStyle16"));
        //            totalPriceOldcnaplock = totaloldscnaplock * Convert.ToDecimal(snapLockRate);
        //            totalPriceOldcnaplock = System.Decimal.Round(totalPriceOldcnaplock, 0);
        //            row.Cells.Add(new WorksheetCell(totalPriceOldcnaplock.ToString(), DataType.Number, "HeaderStyle16"));

        //            totalReplacementScnaplock = 2 * totalReplacementScnaplock;
        //            row.Cells.Add(new WorksheetCell(totalReplacementScnaplock.ToString(), DataType.Number, "HeaderStyle16"));
        //            totalPriceReplacementcnaplock = totalReplacementScnaplock * Convert.ToDecimal(snapLockRate);
        //            totalPriceReplacementcnaplock = System.Decimal.Round(totalPriceReplacementcnaplock, 0);
        //            row.Cells.Add(new WorksheetCell(totalReplacementScnaplock.ToString(), DataType.Number, "HeaderStyle16"));

        //            decimal TotalSnapLocak = totalPriceNewscnaplock + totalPriceOldcnaplock + totalPriceReplacementcnaplock;
        //            row.Cells.Add(new WorksheetCell(TotalSnapLocak.ToString(), DataType.Number, "HeaderStyle16"));

        //            row = sheet.Table.Rows.Add();

        //            row = sheet.Table.Rows.Add();
                   
        //            book.Save(strPath+"\\"+filename);
                   
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //       MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        private static void GenerateStyle(Workbook book)
        {
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


            WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
            style6.Font.FontName = "Tahoma";
            style6.Font.Size = 10;
            style6.Font.Bold = true;
            style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
        }

        #endregion



        #region AP        

        private void GenerateAPAffixingReport(string strRTOId,string strRTOLocation)
        {
            try
            {
                string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();
                string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

                string filename = "Andhra Pradesh Affixing Report" + "_" + strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();
                SetFolder(strRTOLocation, cmbState.Text, "Andhra Pradesh Affixing Report" + "_" + strMonth + strYear + "_");
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily Embossing Report";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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

                Worksheet sheet = book.Worksheets.Add("AP Data Affixing Report");

                AddColumnToSheet(sheet,100,10);
                

                WorksheetRow row = sheet.Table.Rows.Add();
                int iCounter = 1;


                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell1 = row.Cells.Add("Daily Report from Affixing Station");
                cell1.MergeAcross = 3; // Merge two cells together
                cell1.StyleID = "HeaderStyle3";

                row.Index = iCounter++;

                row = sheet.Table.Rows.Add();
                row.Index = iCounter++;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Andhra Pradesh", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("RTO Location", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(strRTOLocation, "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = iCounter++;

                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));



                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                WorksheetCell cell6 = row.Cells.Add("Report Date: " + dtpFrmDate.Value.ToString("dd/MM/yyyy") + " - " + dtpToDate.Value.ToString("dd/MM/yyyy"));

                cell6.MergeAcross = 5; // Merge two cells togetherto 
                cell6.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = iCounter++;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row = sheet.Table.Rows.Add();



                row.Index = iCounter++;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle6"));
                WorksheetCell cell11 = row.Cells.Add("Cash Receipt");
                cell11.MergeAcross = 1;
                cell11.StyleID = "HeaderStyle6";
                //row.Cells.Add(new WorksheetCell("Application No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date of Affixation", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Owners Name", "HeaderStyle6"));
                WorksheetCell cell12 = row.Cells.Add("Affixation Invoice");
                cell12.MergeAcross = 1;
                cell12.StyleID = "HeaderStyle6";
                row.Cells.Add(new WorksheetCell("No.of days for Affixation", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Category", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Registration Number", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                WorksheetCell cell122 = row.Cells.Add("Laser Identification No");
                cell122.MergeAcross = 1; // Merge two cells together
                cell122.StyleID = "HeaderStyle6";

                WorksheetCell cell2 = row.Cells.Add("High Security Registration Plate Size");
                cell2.MergeAcross = 1; // Merge two cells together
                cell2.StyleID = "HeaderStyle6";
                // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("3rd Plate  Y/N", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell("Colour", "HeaderStyle"));
                WorksheetCell cell3 = row.Cells.Add("Colour Background");
                cell3.MergeAcross = 1; // Merge two cells together
                cell3.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;


                row.Index = iCounter++;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));


                row.Cells.Add(new WorksheetCell("Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Yellow", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("White", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell(" ", "HeaderStyle"));

                row = sheet.Table.Rows.Add();

                string SQLString = "SELECT b.ProductColor,VehicleRegNo, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory,  Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize , a.Remarks,a.CashReceiptNo AS CashNo,convert(varchar(10),a.HSRPRecord_CreationDate,103) AS CashDate,a.DeliveryChallanNo,(case when a.DeliveryChallanNo is null then '' else convert(varchar(10), a.ordercloseddate,103) end) as ordercloseddate ,datediff(day,a.hsrprecord_creationdate,OrderClosedDate) as No_of_days  FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN Product Product_1  ON a.FrontPlateSize = Product_1.ProductID WHERE a.HSRP_StateID= '9' AND a.OrderclosedDate   between '" + FromDate + "' and '" + ToDate + "'and a.OrderclosedDate <> '' and a.Rtolocationid='" + strRTOId + "' and a.orderstatus='Closed' order by a.ordercloseddate";
               DataTable dt = GetDataInDT(SQLString);


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["CashNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["CashDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["ordercloseddate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["DeliveryChallanNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["ordercloseddate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["No_of_days"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["FrontPlateSize"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RearPlateSize1"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle1"));
                        if (dtrows["ProductColor"].ToString() == "YELLOW")
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
                        }
                        //row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    }


                    row = sheet.Table.Rows.Add();
                   
                    book.Save(strPath+"\\"+filename);

                   
                }



            }

            catch (Exception ex)
            {
               MessageBox.Show(ex.Message.ToString());
            }
        }

        protected void GenerateAPAffixingReportNew(string strRTOId, string strRTOLocation)
        {
            try
            {               

                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                string filename = "Andhra Pradesh Affixing Report New" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();
                SetFolder(strRTOLocation, cmbState.Text, "Andhra Pradesh Affixing Report New");

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily Affixation Report";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook

                #region Style
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                #endregion

                Worksheet sheet = book.Worksheets.Add("AP Data Affixing Report");
                AddColumnToSheet(sheet, 100, 10);               

                WorksheetRow row = sheet.Table.Rows.Add();
                int iCounter = 1;
                WorksheetCell cell1 = row.Cells.Add("Daily Report from Affixing Station");
                cell1.MergeAcross = 15; // Merge two cells together
                cell1.StyleID = "HeaderStyle3";

                row.Index = iCounter++;

                row = sheet.Table.Rows.Add();

                row.Index = iCounter++;

                row.Cells.Add(new WorksheetCell("Location", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(strRTOLocation, "HeaderStyle2"));

                AddNewCell(row, "", "HeaderStyle2",8);

                
                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                WorksheetCell cell6 = row.Cells.Add("Report Date: " + dtpFrmDate.Value.ToString("dd/MM/yyyy") + " - " + dtpToDate.Value.ToString("dd/MM/yyyy"));
                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = iCounter++;

                cell6.MergeAcross = 5; // Merge two cells togetherto 
                cell6.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = iCounter++;
                AddNewCell(row, "", "HeaderStyle2", 2);
                row = sheet.Table.Rows.Add();



                row.Index = iCounter++;
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle6"));
                WorksheetCell cell11 = row.Cells.Add("Cash Receipt");
                cell11.MergeAcross = 1;
                cell11.StyleID = "HeaderStyle6";
                row.Cells.Add(new WorksheetCell("Date of Affixation", "HeaderStyle6"));
                WorksheetCell cell12 = row.Cells.Add("Affixation Invoice");
                cell12.MergeAcross = 1;
                cell12.StyleID = "HeaderStyle6";
                row.Cells.Add(new WorksheetCell("No.of days for Affixation", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Category", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Registration Number", "HeaderStyle6"));
                WorksheetCell cell122 = row.Cells.Add("Laser Identification No");
                cell122.MergeAcross = 1; 
                cell122.StyleID = "HeaderStyle6";

                WorksheetCell cell2 = row.Cells.Add("High Security Registration Plate Size");
                cell2.MergeAcross = 1; 
                cell2.StyleID = "HeaderStyle6";
                row.Cells.Add(new WorksheetCell("3rd Plate  Y/N", "HeaderStyle6"));
                WorksheetCell cell3 = row.Cells.Add("Colour Background");
                cell3.MergeAcross = 1; 
                cell3.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;


                row.Index = iCounter++;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));


                row.Cells.Add(new WorksheetCell("Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Yellow", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("White", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell(" ", "HeaderStyle"));

                row = sheet.Table.Rows.Add();               

              string strSQLString = "SELECT b.ProductColor,VehicleRegNo, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType,"+
                  " a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory,  Product_1.ProductCode AS FrontPlateSize,"+
                  " a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize , a.Remarks,a.CashReceiptNo AS CashNo,convert(varchar(10),"+
                  "a.HSRPRecord_CreationDate,103) AS CashDate,a.DeliveryChallanNo,(case when a.DeliveryChallanNo is null then '' else convert(varchar(10), a.ordercloseddate,103) end) as ordercloseddate ,"+
                  "datediff(day,a.hsrprecord_creationdate,OrderClosedDate) as No_of_days  FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN"+
                  " Product Product_1  ON a.FrontPlateSize = Product_1.ProductID WHERE a.HSRP_StateID= '9' AND"+
                  " convert(date,a.OrderclosedDate)   between '" + dtpFrmDate.Value + "' and '" + dtpToDate.Value + "'and a.OrderclosedDate <> '' and a.Rtolocationid='" + strRTOId + "' and a.orderstatus='Closed' order by a.ordercloseddate";
              dt = GetDataInDT(strSQLString);


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) 
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["CashNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["CashDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["ordercloseddate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["DeliveryChallanNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["ordercloseddate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["No_of_days"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["FrontPlateSize"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RearPlateSize1"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle1"));
                        if (dtrows["ProductColor"].ToString() == "YELLOW")
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
                        }
                    }

                    row = sheet.Table.Rows.Add();

                    book.Save(strPath + "\\" + filename);                   

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GenerateAPEmbossingReport()
        {
            try
            {
                string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

                string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

                string filename = "Andhra Pradesh Embossing Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();
                SetFolder("",cmbState.Text,"Andhra Pradesh Embossing Report");
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily Embossing Report";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                style8.Font.FontName = "Tahoma";
                style8.Font.Size = 10;
                style8.Font.Bold = true;
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style8.Interior.Color = "#D4CDCD";
                style8.Interior.Pattern = StyleInteriorPattern.Solid;

                WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                style5.Font.FontName = "Tahoma";
                style5.Font.Size = 10;
                style5.Font.Bold = true;
                style5.Alignment.Horizontal = StyleHorizontalAlignment.Left;
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

                Worksheet sheet = book.Worksheets.Add("AP Data Embossing Report");
                AddColumnToSheet(sheet, 100, 10);

                WorksheetRow row = sheet.Table.Rows.Add();

                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Daily Report from Embossing Stations");
                cell.MergeAcross = 8; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row.Index = 4;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Andhra Pradesh", "HeaderStyle2"));
                //  Skip one row, and add some text
                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dtpFrmDate.Value.ToString("dd/MMM/yyyy") + " - " + dtpToDate.Value.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row = sheet.Table.Rows.Add();



                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle6"));
                WorksheetCell cell11 = row.Cells.Add("Cash Receipt");
                cell11.MergeAcross = 1;
                cell11.StyleID = "HeaderStyle6";
                //row.Cells.Add(new WorksheetCell("Application No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell("Owners Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Registration Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RTO Location", "HeaderStyle6"));

                //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                WorksheetCell cell1 = row.Cells.Add("Laser Identification No");
                cell1.MergeAcross = 1; // Merge two cells together
                cell1.StyleID = "HeaderStyle6";
                //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
                // row.Cells.Add(new WorksheetCell("RP Size", "HeaderStyle"));

                WorksheetCell cell2 = row.Cells.Add("High Security Registration Plate Size");
                cell2.MergeAcross = 1; // Merge two cells together
                cell2.StyleID = "HeaderStyle6";
                // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("3rd Plate  Y/N", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell("Colour", "HeaderStyle"));
                WorksheetCell cell3 = row.Cells.Add("Colour Background");
                cell3.MergeAcross = 1; // Merge two cells together
                cell3.StyleID = "HeaderStyle6";
                WorksheetCell cell4 = row.Cells.Add("SMS/Email sent Date");
                cell4.StyleID = "HeaderStyle6";
                //row.Cells.Add(new WorksheetCell("SMS/Email sent Date", "HeaderStyle6"));
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;


                row.Index = 8;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));


                // row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Yellow", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("White", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle"));

                row = sheet.Table.Rows.Add();
              string  SQLString = "SELECT    b.ProductColor,VehicleRegNo, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks,a.CashReceiptNo AS CashNo,convert(varchar(10),a.HSRPRecord_CreationDate,103) AS CashDate,r.rtolocationname FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID inner join rtolocation r on a.RTOLocationID=r.RTOLocationID  WHERE  a.HSRP_StateID= '9' AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "' order by OrderEmbossingDate";
              DataTable  dt = GetDataInDT(SQLString);


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["CashNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["CashDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle1"));
                        //    row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["rtolocationname"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["FrontPlateSize"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RearPlateSize1"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle1"));
                        if (dtrows["ProductColor"].ToString() == "WHITE")
                        {

                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));

                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                        }
                        row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    }


                    row = sheet.Table.Rows.Add();
                  
                    book.Save(strPath+"\\"+filename);

                  

                }



            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        protected void GenerateAPEmbossingReportNew()
        {
            try
            {              

                DataTable dt = new DataTable();

                string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; 

                string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

                string filename = "Andhra Pradesh Embossing Report New" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();
                SetFolder("", cmbState.Text, "Andhra Pradesh Embossing Report New");
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily Embossing Report";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                style8.Font.FontName = "Tahoma";
                style8.Font.Size = 10;
                style8.Font.Bold = true;
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style8.Interior.Color = "#D4CDCD";
                style8.Interior.Pattern = StyleInteriorPattern.Solid;

                WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                style5.Font.FontName = "Tahoma";
                style5.Font.Size = 10;
                style5.Font.Bold = true;
                style5.Alignment.Horizontal = StyleHorizontalAlignment.Left;
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
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Center;

                Worksheet sheet = book.Worksheets.Add("AP Data Embossing Report");
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

                row.Index = 3;
                WorksheetCell cell = row.Cells.Add("Daily Report from Embossing Stations");
                cell.MergeAcross = 13; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row.Index = 4;


                WorksheetCell cell123 = row.Cells.Add("Blank Plates");

                cell123.MergeAcross = 3;
                cell123.StyleID = "HeaderStyle2";
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                WorksheetCell cell12345 = row.Cells.Add("Date :");
                cell12345.StyleID = "HeaderStyle2";
                WorksheetCell cell123456 = row.Cells.Add(dtpFrmDate.Value.ToString("dd/MMM/yyyy") + " - " + dtpToDate.Value.ToString("dd/MMM/yyyy"));
                cell123456.StyleID = "HeaderStyle2";

                row.Index = 5;
                row = sheet.Table.Rows.Add();
                row = sheet.Table.Rows.Add();

                row.Index = 7;
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle6"));
                WorksheetCell cell11 = row.Cells.Add("Cash Receipt");
                cell11.MergeAcross = 1;
                cell11.StyleID = "HeaderStyle6";
                row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell("Owners Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Registration Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RTO Location", "HeaderStyle6"));

                WorksheetCell cell1 = row.Cells.Add("Laser Identification No");
                cell1.MergeAcross = 1; // Merge two cells together
                cell1.StyleID = "HeaderStyle6";

                WorksheetCell cell2 = row.Cells.Add("High Security Registration Plate Size");
                cell2.MergeAcross = 1; // Merge two cells together
                cell2.StyleID = "HeaderStyle6";
                row.Cells.Add(new WorksheetCell("3rd Plate  Y/N", "HeaderStyle6"));
                WorksheetCell cell3 = row.Cells.Add("Colour Background");
                cell3.MergeAcross = 1; // Merge two cells together
                cell3.StyleID = "HeaderStyle6";
                WorksheetCell cell4 = row.Cells.Add("SMS/Email sent Date");
                cell4.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;


                row.Index = 8;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));


                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Yellow", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("White", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle"));

                row = sheet.Table.Rows.Add();

               string strSQLString = "SELECT    b.ProductColor,VehicleRegNo, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType,"+
                   " a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize,"+
                   " a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks,a.CashReceiptNo AS CashNo,convert(varchar(10),"+
                   "a.HSRPRecord_CreationDate,103) AS CashDate,r.rtolocationname FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN "+
                   " Product Product_1 ON a.FrontPlateSize = Product_1.ProductID inner join rtolocation r on a.RTOLocationID=r.RTOLocationID  WHERE  a.HSRP_StateID= '9'"+
                   " AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "' order by OrderEmbossingDate";
                dt = GetDataInDT(strSQLString);



                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["CashNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["CashDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle1"));
                        //    row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["rtolocationname"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["FrontPlateSize"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RearPlateSize1"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle1"));
                        if (dtrows["ProductColor"].ToString() == "WHITE")
                        {

                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));

                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                        }
                        row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    }


                    row = sheet.Table.Rows.Add();

                    book.Save(strPath + "\\" + filename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GenerateRegisteringAuthorityReport(string strRTOId, string strRTOLocation)
        {

            try
            {
                string filename = "APSchedule-E-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();
                SetFolder(strRTOLocation,cmbState.Text,"APSchedule-E-");
                string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

                string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Monthly Statement Form Concessionaire To Registering Authority";
                book.Properties.Created = DateTime.Now;

                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style15 = book.Styles.Add("HeaderStyle15");
                style15.Font.FontName = "Tahoma";
                style15.Font.Size = 10;
                style15.Font.Bold = false;
                style15.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style15.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style15.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style15.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style15.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style16 = book.Styles.Add("HeaderStyle16");
                style16.Font.FontName = "Tahoma";
                style16.Font.Size = 10;
                style16.Font.Bold = false;
                style16.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style16.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style16.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style16.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style16.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                style8.Font.FontName = "Tahoma";
                style8.Font.Size = 10;
                style8.Font.Bold = false;
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style8.Interior.Color = "#D4CDCD";
                style8.Interior.Pattern = StyleInteriorPattern.Solid;

                WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                style5.Font.FontName = "Tahoma";
                style5.Font.Size = 10;
                style5.Font.Bold = true;
                style5.Alignment.Horizontal = StyleHorizontalAlignment.Left;
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

                Worksheet sheet = book.Worksheets.Add("Monthly Statement From Concessionaire To Registering Authority");

                AddColumnToSheet(sheet, 100, 10);

               WorksheetRow row = sheet.Table.Rows.Add();

                row.Index = 2;

                AddNewCell(row, "", "HeaderStyle3", 3);

                row = sheet.Table.Rows.Add();


                row.Index = 4;
                AddNewCell(row, "", "HeaderStyle3", 2);
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("SCHEDULE-E: Daily Statement From Service Provider To Registering Authority");
                cell.MergeAcross = 5; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();


                row.Index = 5;
                AddNewCell(row, "", "HeaderStyle2", 2);
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Andhra Pradesh", "HeaderStyle2"));

                row.Cells.Add(new WorksheetCell("RTO Location", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(strRTOLocation, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                // Skip one row, and add some text
                row.Index = 6;

                AddNewCell(row, "", "HeaderStyle2", 2);
                row.Cells.Add(new WorksheetCell("Generated Datetime:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Duration : " + dtpFrmDate.Value.ToString("dd/MMM/yyyy") + " - " + dtpToDate.Value.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 9;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("Sr.No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Category", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("No. of New Vehicle Affixed", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Price Charged", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("No. of Existing Vehicles Affixed", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Price Charged", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Replacements", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Price Charged", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Total Amount", "HeaderStyle"));
                row = sheet.Table.Rows.Add();

                String StringField = String.Empty;
                String StringAlert = String.Empty;

                //row.Index = 9;

                
               string SQLString = "exec Report_Monthly_Statement_Registering_Authority'" + FromDate + "','" + ToDate + "','"+cmbState.SelectedValue+"','" + strRTOId + "'";
                DataTable ds = GetDataInDT(SQLString);

                string RTOColName = string.Empty;
                int sno = 0;

                if (ds.Rows.Count > 0)
                {

                    Int32 totalNewscnaplock = 0;
                    Int32 totaloldscnaplock = 0;
                    Int32 totalReplacementScnaplock = 0;

                    Decimal totalPriceNewscnaplock = 0;

                    Decimal totalPriceOldcnaplock = 0;

                    Decimal totalPriceReplacementcnaplock = 0;

                    Int32 Total = 0;

                    Int32 totalNewPrice = 0;
                    Int32 totalOldPrice = 0;
                    Int32 ReplacementPrice = 0;

                    foreach (DataRow dtrows in ds.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        string Type = dtrows["vehicleandplatetype"].ToString();
                        row = sheet.Table.Rows.Add();


                        totalNewPrice = Convert.ToInt32(dtrows["newvehiclecash"].ToString());
                        totalOldPrice = Convert.ToInt32(dtrows["oldvehiclecash"].ToString());
                        ReplacementPrice = Convert.ToInt32(dtrows["replaceveihclecash"].ToString());

                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        Total = totalNewPrice + totalOldPrice + ReplacementPrice;


                        if (Type == "2W")
                        {
                            row.Cells.Add(new WorksheetCell("Complete Set of Registration Plates Inclusive of Snap Lock and fixing for 2 Wheelers- Schooters, Motorcycles, Mopeds", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                        }
                        if (Type == "3W")
                        {
                            row.Cells.Add(new WorksheetCell("Complete Set of Registration Plates Inclusive of Snap Lock, 3rd Registration Plate and fixing for 3 wheelers and invalid carriages.", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                        }
                        if (Type == "4W")
                        {
                            row.Cells.Add(new WorksheetCell("Complete Set of Registration Plates Inclusive of Snap Lock 3rd Registration Plate and fixing for Light Motor Vehicles/Passenger Cars, Mopeds", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                        }
                        if (Type == "4HW")
                        {
                            row.Cells.Add(new WorksheetCell("Complete Set of Registration Plates Inclusive of Snap Lock 3rd Registration Plate and fixing for Medium Commercial Vehicles/Havy Commercial Vehicles/Trailer/combination/others.", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                        }
                        if (Type == "500X120")
                        {
                            row.Cells.Add(new WorksheetCell("Registration Plate Size 500X100 mm", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));

                            totalNewscnaplock = +totalNewscnaplock + Convert.ToInt32(dtrows["newvehicleorder"]);
                            totaloldscnaplock = +totaloldscnaplock + Convert.ToInt32(dtrows["oldvehicleorder"]);
                            totalReplacementScnaplock = +totalReplacementScnaplock + Convert.ToInt32(dtrows["replacevehicle"]);

                        }
                        if (Type == "340X200")
                        {
                            row.Cells.Add(new WorksheetCell("Registration Plate Size 300X200 mm", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));

                            totalNewscnaplock = +totalNewscnaplock + Convert.ToInt32(dtrows["newvehicleorder"]);
                            totaloldscnaplock = totaloldscnaplock + Convert.ToInt32(dtrows["oldvehicleorder"]);
                            totalReplacementScnaplock = +totalReplacementScnaplock + Convert.ToInt32(dtrows["replacevehicle"]);
                        }
                        if (Type == "200X100")
                        {
                            row.Cells.Add(new WorksheetCell("Registration Plate Size 200X100 mm", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));

                            totalNewscnaplock = +totalNewscnaplock + Convert.ToInt32(dtrows["newvehicleorder"]);
                            totaloldscnaplock = totaloldscnaplock + Convert.ToInt32(dtrows["oldvehicleorder"]);
                            totalReplacementScnaplock = +totalReplacementScnaplock + Convert.ToInt32(dtrows["replacevehicle"]);
                        }
                        if (Type == "285X45")
                        {
                            row.Cells.Add(new WorksheetCell("Registration Plate Size 285X45 mm", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));

                            totalNewscnaplock = +totalNewscnaplock + Convert.ToInt32(dtrows["newvehicleorder"]);
                            totaloldscnaplock = +totaloldscnaplock + Convert.ToInt32(dtrows["oldvehicleorder"]);
                            totalReplacementScnaplock = +totalReplacementScnaplock + Convert.ToInt32(dtrows["replacevehicle"]);
                        }
                        if (Type == "Sticker")
                        {
                            row.Cells.Add(new WorksheetCell("3rd Registration Plate Sticker Inclusive of Printing", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                            //row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                        }
                    }
                    row = sheet.Table.Rows.Add();
                    decimal snapLockRate = 0;
                    //SQLString = "Report_Monthly_Statement_Registering_Authority'" + FromDate + "','" + ToDate + "','" + intHSRPStateID + "','0'";
                    SQLString = "SELECT ProductCost.Cost FROM  Product  INNER JOIN ProductCost ON Product.ProductID = ProductCost.ProductID where Product.productcode ='SNAP LOCK' and ProductCost.hsrp_stateID='" + cmbState.SelectedValue + "'";
                    DataTable dsRate = GetDataInDT(SQLString);
                    if (dsRate.Rows.Count > 0)
                    {
                        snapLockRate = Convert.ToDecimal(dsRate.Rows[0]["cost"]);
                    }

                    sno = sno + 1;
                    row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("Snap Locks.", DataType.String, "HeaderStyle1"));
                    totalNewscnaplock = 2 * totalNewscnaplock;
                    row.Cells.Add(new WorksheetCell(totalNewscnaplock.ToString(), DataType.Number, "HeaderStyle16"));
                    totalPriceNewscnaplock = Convert.ToInt32(totalNewscnaplock) * Convert.ToDecimal(snapLockRate);
                    totalPriceNewscnaplock = System.Decimal.Round(totalPriceNewscnaplock, 0);
                    row.Cells.Add(new WorksheetCell(totalPriceNewscnaplock.ToString(), DataType.Number, "HeaderStyle16"));

                    totaloldscnaplock = 2 * totaloldscnaplock;
                    row.Cells.Add(new WorksheetCell(totaloldscnaplock.ToString(), DataType.Number, "HeaderStyle16"));
                    totalPriceOldcnaplock = totaloldscnaplock * Convert.ToDecimal(snapLockRate);
                    totalPriceOldcnaplock = System.Decimal.Round(totalPriceOldcnaplock, 0);
                    row.Cells.Add(new WorksheetCell(totalPriceOldcnaplock.ToString(), DataType.Number, "HeaderStyle16"));

                    totalReplacementScnaplock = 2 * totalReplacementScnaplock;
                    row.Cells.Add(new WorksheetCell(totalReplacementScnaplock.ToString(), DataType.Number, "HeaderStyle16"));
                    totalPriceReplacementcnaplock = totalReplacementScnaplock * Convert.ToDecimal(snapLockRate);
                    totalPriceReplacementcnaplock = System.Decimal.Round(totalPriceReplacementcnaplock, 0);
                    row.Cells.Add(new WorksheetCell(totalReplacementScnaplock.ToString(), DataType.Number, "HeaderStyle16"));

                    decimal TotalSnapLocak = totalPriceNewscnaplock + totalPriceOldcnaplock + totalPriceReplacementcnaplock;
                    row.Cells.Add(new WorksheetCell(TotalSnapLocak.ToString(), DataType.Number, "HeaderStyle16"));

                    row = sheet.Table.Rows.Add();                  
                   
                    book.Save(strPath+"\\"+filename);

                }



            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void GenerateAPStockReport(string strRTOId, string strRTOLocation)
        {
            try
            {
               

                string filename = "AP Stock Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();
                SetFolder(strRTOLocation, cmbState.Text, "AP Stock Report");
                string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

                string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily Embossing Report";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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

                Worksheet sheet = book.Worksheets.Add("AP Data Affixing Report");
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

                row.Index = 1;

                row = sheet.Table.Rows.Add();


                row.Index = 2;

                row = sheet.Table.Rows.Add();


                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Daily Stock Report From Embossing Station");
                cell.MergeAcross = 5; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                //row.Index = 4;
                //row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Andhra Pradesh", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("RTO Location :", "HeaderStyle2"));
                WorksheetCell cell5 = row.Cells.Add(strRTOLocation);

                cell5.MergeAcross = 2; // Merge two cells togetherto 
                cell5.StyleID = "HeaderStyle2";
                row = sheet.Table.Rows.Add();
                row.Index = 6;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                WorksheetCell cell6 = row.Cells.Add("Report Duration: " + dtpFrmDate.Value.ToString("dd/MMM/yyyy") + " - " + dtpToDate.Value.ToString("dd/MMM/yyyy"));

                cell6.MergeAcross = 2; // Merge two cells togetherto 
                cell6.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = 7;

                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("HSRP Size", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Opening Balance", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Sent to Affixing Station", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Closing Balance", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Remarks", "HeaderStyle6"));



                String StringField = String.Empty;
                String StringAlert = String.Empty;

               string SQLString = @"select t.Product,sum(t.senttoaffixing)as senttoaffixing,sum(t.Closing) as Closing,sum(t.Opening) as Opening from (select convert(varchar(7),pd) as Product,sum(isnull([Embossing done],0)) as 'senttoaffixing',sum(isnull([New Order],0)) as 'Closing',sum(isnull([Embossing done],0))+sum(isnull([New Order],0)) as 'Opening' from(select * from (select ProductCode as pd,InventoryStatus as inn ,count(InventoryStatus) as rr from RTOInventory inner join Product p on RTOInventory.productid=p.productid  where RTOInventory.HSRP_StateID=999 and  RTOInventory.InventoryStatus in ('Embossing Done','New Order') and statusdate  between '"
                   + FromDate + "' and '" + ToDate + "' and RTOInventory.RTOLocationID='" + strRTOId + "'  group by p.ProductCode,RTOInventory.InventoryStatus)  as kk PIVOT(sum(rr) for inn in([Embossing Done],[New Order])) as pivot55)newtable group by convert(varchar(7),pd) union select '200X100' as pd,'0' as senttoaffixing,'0' as closing,'0' as opening union select '340X200' as pd,'0' as senttoaffixing,'0' as closing,'0' as opening union select '285X45' as pd,'0' as senttoaffixing,'0' as closing,'0' as opening union select '500X120' as pd,'0' as senttoaffixing,'0' as closing,'0' as opening) T group by t.Product";
               DataTable dt = GetDataInDT(SQLString);


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["Product"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Opening"].ToString(), DataType.String, "HeaderStyle1"));


                        row.Cells.Add(new WorksheetCell(dtrows["senttoaffixing"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Closing"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));

                    }
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("5", DataType.String, "HeaderStyle1"));

                    row.Cells.Add(new WorksheetCell("TRP", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));


                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("6", DataType.String, "HeaderStyle1"));

                    row.Cells.Add(new WorksheetCell("Snap Locks Pair", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));


                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));  
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));

                    book.Save(strPath+"\\"+filename);                  

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion


        //for Mail Sending-15-09-2014
        public void SendReports_Click(object sender, EventArgs e)
        {


            //GenerateMPCashCollection();
            //GenerateSchedule_CE_MP();

            GenerateHPAuthorityReport();
            GenerateUKAuthorityReport();
            GenerateBRAuthorityReport();
            GenerateHRAuthorityReport();

            //GenerateAuthorityReport();
           // GenerateUKCashCollection();
            // AssignDirectory();
            string CurrentDate = System.DateTime.Now.ToShortDateString();
           // string directoryPath = @"D:\TenderReports\MADHYA PRADESH\CurrentDate\";
            string directoryPath = strPath;

                

            DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
            // string strPath =@"D:\TenderReports\";
            //GenerateData();
            // string HtmlBody = ConvertDataTableToHtml(dt);
            //MailHelper.SendMailMessage("reportshsrp@gmail.com", directoryPath, strPath);
            MailHelper.SendMailMessage("developer0186@gmail.com", directoryPath, strPath);

        }

    
        public void timer1_Tick(object sender, EventArgs e)
        {
            string strDate = DateTime.Now.ToString("hh:mm tt");

           // string strTime1 = "17:50 AM";
           // string strTime2 = "18:00 AM";
           // string ss = System.DateTime.Now.ToShortTimeString();
           
              
                SendReports_Click(button1, new EventArgs());
           
            
            
            // string strDate = DateTime.Now.ToString("hh:mm tt");

            //string strTime1 = "12:05 AM";
            //string strTime2 = "12:10 AM";
            //string ss = System.DateTime.Now.ToShortTimeString();
            
               // SendReports_Click(button2, new EventArgs());

        }


        #region Bihar State

        private void GenerateBRAuthorityReport()
        {
            try
            {
                string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

                string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

                DataTable dt = new DataTable();
                string SQLString = "exec Business_DateYearMonthWise_Collection_Summary '2015/06/22',1,B";
                // string SQLString = "exec Report_Monthly_StatementLocationWiseRegisteringAuthority '2014-10-10 00:00:00','2014-10-20 23:59:59','"+cmbState.SelectedValue.ToString()+"',482";
                DataTable ds = GetDataInDT(SQLString);

                string RTOColName = string.Empty;
                int sno = 0;


                if (ds.Rows.Count > 0)
                {
                    string filename = "Collection_V_S_Deposit_Bihar" + "_" + strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();
                    SetFolder("", cmbState.Text, "Collection_V_S_Deposit_Bihar" + "_" + strMonth + strYear + "_");
                    // Specify which Sheet should be opened and the size of window by default

                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "Daily Entry Report";
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
                    style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                    WorksheetStyle style15 = book.Styles.Add("HeaderStyle15");
                    style15.Font.FontName = "Tahoma";
                    style15.Font.Size = 10;
                    style15.Font.Bold = false;
                    style15.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style15.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style15.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style15.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style15.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                    style8.Font.FontName = "Tahoma";
                    style8.Font.Size = 10;
                    style8.Font.Bold = false;
                    style8.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style8.Interior.Color = "#D4CDCD";
                    style8.Interior.Pattern = StyleInteriorPattern.Solid;

                    WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                    style5.Font.FontName = "Tahoma";
                    style5.Font.Size = 10;
                    style5.Font.Bold = true;
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

                    Worksheet sheet = book.Worksheets.Add("Collection_V_S_Deposit_Bihar");
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
                    // row.


                    row.Index = 1;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    // WorksheetCell cell5 = row.Cells.Add("SCHEDULE-D ");
                    //  cell5.MergeAcross = 3; // Merge two cells togetherto 
                    // cell5.StyleID = "HeaderStyle6";

                    row = sheet.Table.Rows.Add();


                    row.Index = 2;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("Cumulative Collection and Deposit Report");
                    cell.MergeAcross = 3; // Merge two cells togetherto 
                    cell.StyleID = "HeaderStyle3";

                    row = sheet.Table.Rows.Add();



                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Bihar", "HeaderStyle2"));

                    row = sheet.Table.Rows.Add();
                    // Skip one row, and add some text

                    WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                    style6.Font.FontName = "Tahoma";
                    style6.Font.Size = 10;
                    style6.Font.Bold = true;
                    style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    row.Index = 4;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Generated Datetime:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 5;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Month", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dtpFrmDate.Value.ToString("dd/MMM/yyyy") + " - " + dtpToDate.Value.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();



                    row.Index = 6;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                    //WorksheetCell cell2 = row.Cells.Add("Applicaton Received");
                    // cell2.MergeAcross = 1; // Merge two cells togetherto 
                    // cell2.StyleID = "HeaderStyle6";

                    //  WorksheetCell cell3 = row.Cells.Add("Registration Plates Supplied");
                    //  cell3.MergeAcross = 1; // Merge two cells togetherto 
                    //  cell3.StyleID = "HeaderStyle6";

                    //WorksheetCell cell4 = row.Cells.Add("Back log (if any)");
                    //cell4.MergeAcross = 1; // Merge two cells togetherto 
                    //cell4.StyleID = "HeaderStyle6";

                    //  row.Cells.Add(new WorksheetCell("Back log (if any) ", "HeaderStyle"));

                    row = sheet.Table.Rows.Add();



                    row.Index = 7;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("Sr.No.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Zone", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("District", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Location", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Orders ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Opening Cash Balance", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Collection ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Deposit ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Balance Cash ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day1 ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day2 ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day3 ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day4 ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day5 ", "HeaderStyle6"));
                    //    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    //row.Cells.Add(new WorksheetCell("O", "HeaderStyle6"));
                    row = sheet.Table.Rows.Add();

                    foreach (DataRow dtrows in ds.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        // row.Cells.Add(new WorksheetCell(dtrows["id"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["district"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Orders"].ToString(), DataType.Number, "HeaderStyle15"));

                        //row.Cells.Add(new WorksheetCell(dtrows["Opening Cash Balance"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Opening Cash Balance"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Collection"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Deposit"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Balance Cash"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day1"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day2"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day3"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day4"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day5 Or More"].ToString(), DataType.Number, "HeaderStyle15"));

                        //int sum = int.Parse(dtrows["newlog"].ToString()) + int.Parse(dtrows["oldlog"].ToString());
                        // row.Cells.Add(new WorksheetCell(sum.ToString(), DataType.Number, "HeaderStyle15"));
                        //row.Cells.Add(new WorksheetCell(dtrows["oldlog"].ToString(), DataType.Number, "HeaderStyle15"));
                    }
                    row = sheet.Table.Rows.Add();

                    row = sheet.Table.Rows.Add();

                    book.Save(strPath + "\\" + filename);



                }



            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion


        #region HIMACHAL PRADESH

        private void GenerateHPAuthorityReport()
        {
            try
            {
                string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

                string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

                DataTable dt = new DataTable();
                string SQLString = "exec Business_DateYearMonthWise_Collection_Summary '2015/06/22',3,B";
                // string SQLString = "exec Report_Monthly_StatementLocationWiseRegisteringAuthority '2014-10-10 00:00:00','2014-10-20 23:59:59','"+cmbState.SelectedValue.ToString()+"',482";
                DataTable ds = GetDataInDT(SQLString);

                string RTOColName = string.Empty;
                int sno = 0;


                if (ds.Rows.Count > 0)
                {
                    string filename = "Collection_V_S_Deposit_HP" + "_" + strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();
                    SetFolder("", cmbState.Text, "Collection_V_S_Deposit_HP" + "_" + strMonth + strYear + "_");
                    // Specify which Sheet should be opened and the size of window by default

                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "Daily Entry Report";
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
                    style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                    WorksheetStyle style15 = book.Styles.Add("HeaderStyle15");
                    style15.Font.FontName = "Tahoma";
                    style15.Font.Size = 10;
                    style15.Font.Bold = false;
                    style15.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style15.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style15.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style15.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style15.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                    style8.Font.FontName = "Tahoma";
                    style8.Font.Size = 10;
                    style8.Font.Bold = false;
                    style8.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style8.Interior.Color = "#D4CDCD";
                    style8.Interior.Pattern = StyleInteriorPattern.Solid;

                    WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                    style5.Font.FontName = "Tahoma";
                    style5.Font.Size = 10;
                    style5.Font.Bold = true;
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

                    Worksheet sheet = book.Worksheets.Add("Collection_V_S_Deposit_HP");
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
                    // row.


                    row.Index = 1;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    // WorksheetCell cell5 = row.Cells.Add("SCHEDULE-D ");
                    //  cell5.MergeAcross = 3; // Merge two cells togetherto 
                    // cell5.StyleID = "HeaderStyle6";

                    row = sheet.Table.Rows.Add();


                    row.Index = 2;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("Cumulative Collection and Deposit Report");
                    cell.MergeAcross = 3; // Merge two cells togetherto 
                    cell.StyleID = "HeaderStyle3";

                    row = sheet.Table.Rows.Add();



                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(" HIMACHAL PRADESH", "HeaderStyle2"));

                    row = sheet.Table.Rows.Add();
                    // Skip one row, and add some text

                    WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                    style6.Font.FontName = "Tahoma";
                    style6.Font.Size = 10;
                    style6.Font.Bold = true;
                    style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    row.Index = 4;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Generated Datetime:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 5;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Month", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dtpFrmDate.Value.ToString("dd/MMM/yyyy") + " - " + dtpToDate.Value.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();



                    row.Index = 6;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                    //WorksheetCell cell2 = row.Cells.Add("Applicaton Received");
                    // cell2.MergeAcross = 1; // Merge two cells togetherto 
                    // cell2.StyleID = "HeaderStyle6";

                    //  WorksheetCell cell3 = row.Cells.Add("Registration Plates Supplied");
                    //  cell3.MergeAcross = 1; // Merge two cells togetherto 
                    //  cell3.StyleID = "HeaderStyle6";

                    //WorksheetCell cell4 = row.Cells.Add("Back log (if any)");
                    //cell4.MergeAcross = 1; // Merge two cells togetherto 
                    //cell4.StyleID = "HeaderStyle6";

                    //  row.Cells.Add(new WorksheetCell("Back log (if any) ", "HeaderStyle"));

                    row = sheet.Table.Rows.Add();



                    row.Index = 7;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("Sr.No.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Zone", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("District", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Location", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Orders ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Opening Cash Balance", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Collection ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Deposit ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Balance Cash ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day1 ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day2 ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day3 ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day4 ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day5 ", "HeaderStyle6"));
                    //    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    //row.Cells.Add(new WorksheetCell("O", "HeaderStyle6"));
                    row = sheet.Table.Rows.Add();

                    foreach (DataRow dtrows in ds.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        // row.Cells.Add(new WorksheetCell(dtrows["id"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["district"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Orders"].ToString(), DataType.Number, "HeaderStyle15"));

                        //row.Cells.Add(new WorksheetCell(dtrows["Opening Cash Balance"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Opening Cash Balance"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Collection"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Deposit"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Balance Cash"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day1"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day2"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day3"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day4"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day5 Or More"].ToString(), DataType.Number, "HeaderStyle15"));

                        //int sum = int.Parse(dtrows["newlog"].ToString()) + int.Parse(dtrows["oldlog"].ToString());
                        // row.Cells.Add(new WorksheetCell(sum.ToString(), DataType.Number, "HeaderStyle15"));
                        //row.Cells.Add(new WorksheetCell(dtrows["oldlog"].ToString(), DataType.Number, "HeaderStyle15"));
                    }
                    row = sheet.Table.Rows.Add();

                    row = sheet.Table.Rows.Add();

                    book.Save(strPath + "\\" + filename);



                }



            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion


        #region UTTRAKHAND

        private void GenerateUKAuthorityReport()
        {
            try
            {
                string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

                string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

                DataTable dt = new DataTable();
                string SQLString = "exec Business_DateYearMonthWise_Collection_Summary '2015/06/22',6,B";
                // string SQLString = "exec Report_Monthly_StatementLocationWiseRegisteringAuthority '2014-10-10 00:00:00','2014-10-20 23:59:59','"+cmbState.SelectedValue.ToString()+"',482";
                DataTable ds = GetDataInDT(SQLString);

                string RTOColName = string.Empty;
                int sno = 0;


                if (ds.Rows.Count > 0)
                {
                    string filename = "Collection_V_S_Deposit_UK" + "_" + strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();
                    SetFolder("", cmbState.Text, "Collection_V_S_Deposit_UK" + "_" + strMonth + strYear + "_");
                    // Specify which Sheet should be opened and the size of window by default

                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "Daily Entry Report";
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
                    style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                    WorksheetStyle style15 = book.Styles.Add("HeaderStyle15");
                    style15.Font.FontName = "Tahoma";
                    style15.Font.Size = 10;
                    style15.Font.Bold = false;
                    style15.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style15.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style15.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style15.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style15.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                    style8.Font.FontName = "Tahoma";
                    style8.Font.Size = 10;
                    style8.Font.Bold = false;
                    style8.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style8.Interior.Color = "#D4CDCD";
                    style8.Interior.Pattern = StyleInteriorPattern.Solid;

                    WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                    style5.Font.FontName = "Tahoma";
                    style5.Font.Size = 10;
                    style5.Font.Bold = true;
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

                    Worksheet sheet = book.Worksheets.Add("Collection_V_S_Deposit_UK");
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
                    // row.


                    row.Index = 1;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    // WorksheetCell cell5 = row.Cells.Add("SCHEDULE-D ");
                    //  cell5.MergeAcross = 3; // Merge two cells togetherto 
                    // cell5.StyleID = "HeaderStyle6";

                    row = sheet.Table.Rows.Add();


                    row.Index = 2;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("Cumulative Collection and Deposit Report");
                    cell.MergeAcross = 3; // Merge two cells togetherto 
                    cell.StyleID = "HeaderStyle3";

                    row = sheet.Table.Rows.Add();



                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("UTTRAKHAND", "HeaderStyle2"));

                    row = sheet.Table.Rows.Add();
                    // Skip one row, and add some text

                    WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                    style6.Font.FontName = "Tahoma";
                    style6.Font.Size = 10;
                    style6.Font.Bold = true;
                    style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    row.Index = 4;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Generated Datetime:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 5;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Month", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dtpFrmDate.Value.ToString("dd/MMM/yyyy") + " - " + dtpToDate.Value.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();



                    row.Index = 6;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                    //WorksheetCell cell2 = row.Cells.Add("Applicaton Received");
                    // cell2.MergeAcross = 1; // Merge two cells togetherto 
                    // cell2.StyleID = "HeaderStyle6";

                    //  WorksheetCell cell3 = row.Cells.Add("Registration Plates Supplied");
                    //  cell3.MergeAcross = 1; // Merge two cells togetherto 
                    //  cell3.StyleID = "HeaderStyle6";

                    //WorksheetCell cell4 = row.Cells.Add("Back log (if any)");
                    //cell4.MergeAcross = 1; // Merge two cells togetherto 
                    //cell4.StyleID = "HeaderStyle6";

                    //  row.Cells.Add(new WorksheetCell("Back log (if any) ", "HeaderStyle"));

                    row = sheet.Table.Rows.Add();



                    row.Index = 7;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("Sr.No.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Zone", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("District", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Location", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Orders ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Opening Cash Balance", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Collection ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Deposit ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Balance Cash ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day1 ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day2 ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day3 ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day4 ", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Day5 ", "HeaderStyle6"));
                    //    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    //row.Cells.Add(new WorksheetCell("O", "HeaderStyle6"));
                    row = sheet.Table.Rows.Add();

                    foreach (DataRow dtrows in ds.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        // row.Cells.Add(new WorksheetCell(dtrows["id"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["district"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Orders"].ToString(), DataType.Number, "HeaderStyle15"));

                        //row.Cells.Add(new WorksheetCell(dtrows["Opening Cash Balance"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Opening Cash Balance"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Collection"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Deposit"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Balance Cash"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day1"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day2"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day3"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day4"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day5 Or More"].ToString(), DataType.Number, "HeaderStyle15"));

                        //int sum = int.Parse(dtrows["newlog"].ToString()) + int.Parse(dtrows["oldlog"].ToString());
                        // row.Cells.Add(new WorksheetCell(sum.ToString(), DataType.Number, "HeaderStyle15"));
                        //row.Cells.Add(new WorksheetCell(dtrows["oldlog"].ToString(), DataType.Number, "HeaderStyle15"));
                    }
                    row = sheet.Table.Rows.Add();

                    row = sheet.Table.Rows.Add();

                    book.Save(strPath + "\\" + filename);



                }



            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion









    }
}

