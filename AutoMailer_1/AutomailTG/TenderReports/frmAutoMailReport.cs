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
//Create New Name Space
using iTextSharp.text;
using System.Globalization;
using System.Text.RegularExpressions;
using CarlosAg.ExcelXmlWriter;
using System.IO;
using System.Net.Mail;
using System.Net;
using SendMail;
using System.Threading;
using Microsoft.Win32.TaskScheduler;

namespace TenderReports
{
    public partial class frmAutoMailReport : Form
    {
        public frmAutoMailReport()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["hsrpAPConfiguration"].ConnectionString);
        string strPath = string.Empty;
        string strMonth = string.Empty;
        string strYear = string.Empty;

        #region  Get Data in database 
        private DataTable GetDataInDT(string strSqlString)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = new SqlCommand(strSqlString, con);
                adp.SelectCommand.CommandTimeout = 0;
                adp.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  Select DDL List in State

        private void GetState()
        {
            try
            {
                //string strState = "SELECT distinct [HSRP_StateID],[HSRPStateName] from  [dbo].[HSRPState] order by [HSRPStateName]";
                string strState = "select distinct rc.StateID as HSRP_StateID,hs.HSRPStateName as HSRPStateName from tblReportCollection_SP rc join [HSRPState] hs on rc.StateID=hs.HSRP_StateID where rc.StateID=11";
                // string strState = "SELECT distinct [HSRP_StateID],[HSRPStateName] from  [dbo].[HSRPState] where HSRP_StateID='5' ";
                DataTable dt = GetDataInDT(strState);
                cmbState.DisplayMember = "HSRPStateName";
                cmbState.ValueMember = "HSRP_StateID";
                cmbState.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        
        #region  For Create Folder
        private static void CreateFolder(string strRTO, string strState, string strRTOLocFolderPath)
        {
            Directory.CreateDirectory(strRTOLocFolderPath);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState + "\\" + strRTO);
        }
        #endregion
        
        #region Generate folder name in DDrive

        private string SetFolder(string strRTO, string strState, string strFile)
        {
            string DateFolder = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString();
        
            strPath = "G:\\TenderReportsTG";
            //strPath = excelsheet;
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
                    }
                }

            }
            return strPath = strPath + "\\" + strState + "\\" + DateFolder;
        }
#endregion

        // All Function for repot 

        #region Send Mail Button

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {                
                if (DateTime.Now.ToString("dddd") != "Sunday")
                {
                    Export();

                    string CurrentDate = System.DateTime.Now.ToShortDateString();
                    string directoryPath = strPath;
                    DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
                    MailHelper.SendMailMessage("reportshsrp@gmail.com", directoryPath, strPath);
                    //MailHelper.SendMailMessage("developer0186@gmail.com", directoryPath, strPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region For Timer Set

        public void timer1_Tick(object sender, EventArgs e)
        {

            string strDate = DateTime.Now.ToString("hh:mm tt");
            #region  In case of Local
            string strTime1 = "08:05 AM";
            //string strTime1 = "11:12 AM";

            if (DateTime.Parse(strDate) == DateTime.Parse(strTime1))
            {
            #endregion
                button2_Click(button1, new EventArgs());
            }
            
         }

        #endregion

        private void frmAutoMailReport_Load(object sender, EventArgs e)
        {            
            GetState();           
        }

        private DataTable GetRtoLocation()
        {
            DataTable dtrto = GetDataInDT("select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid='" + cmbState.SelectedValue + "') and RTOLocationID not in (148,331)");
            return dtrto;
        }


        private DataTable GetDatasp(string strSqlString)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = new SqlCommand(strSqlString, con);
                adp.SelectCommand.CommandTimeout = 0;
                adp.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void Export()
        {
            try
            {

                #region Fetch Data
                DataTable dt = null;
                string strcurrentdate = string.Empty;

                if (DateTime.Now.ToString("dddd") == "Monday")
                {
                    strcurrentdate = System.DateTime.Now.AddDays(-2).ToString();
                }
                else
                {
                    strcurrentdate = System.DateTime.Now.AddDays(-1).ToString();
                }

               
                //string strcurrentdate = System.DateTime.Now.ToShortDateString();

                //string SQLStrings = "exec usp_collectionreport_AP";
                string SQLStrings = "exec usp_collectionreport_TG";
                DataTable dtsp = GetDatasp(SQLStrings);

                if (dtsp.Rows.Count > 0)
                {
                    for (int Temp=0; Temp < dtsp.Rows.Count; Temp++)  // for (int k = 0; k < dtsp.Rows.Count; k++)
                    {
                        dt = new DataTable();

                        // Call String Variable for Sp variable 

                        string strstateid = string.Empty;
                        string spname = string.Empty;
                        string strdatetime = string.Empty;
                        string no_of_parameters=string.Empty;
                        string strprocedure =string.Empty;                     
                        string reporttype = string.Empty;
                        string reportname = string.Empty;                        

                        strstateid = dtsp.Rows[Temp][0].ToString().Trim();

                        spname = dtsp.Rows[Temp][1].ToString().Trim();
                        no_of_parameters=dtsp.Rows[Temp][2].ToString().Trim();
                        reporttype = dtsp.Rows[Temp][3].ToString().Trim();
                        reportname = dtsp.Rows[Temp][4].ToString().Trim();

                        if(Convert.ToInt32(no_of_parameters)==2)
                        {                          
                            strprocedure ="["+ spname +" ] " +  "'"+strcurrentdate+ "'  ," + strstateid ;                         
                        }

                        else if (Convert.ToInt32(no_of_parameters) == 3)
                        {
                            strprocedure = "[" + spname + "] " + "'" + strcurrentdate + "'," + "'" + strstateid + "'," + "'" + reporttype + "' ";                         
                        }
                      

                      //  MessageBox.Show(strprocedure);
                        SqlCommand da = new SqlCommand("exec " + strprocedure + "", con);
                        
                        con.Open();
                        da.CommandTimeout = 500000;
                        dt.Load(da.ExecuteReader());
                        con.Close();
                #endregion


                        // Add some styles to the Workbook
                        if(dt.Rows.Count > 0)
                        { 
                        Workbook book = new Workbook();
                        // Specify which Sheet should be opened and the size of window by default
                        book.ExcelWorkbook.ActiveSheetIndex = 1;
                        book.ExcelWorkbook.WindowTopX = 100;
                        book.ExcelWorkbook.WindowTopY = 200;
                        book.ExcelWorkbook.WindowHeight = 7000;
                        book.ExcelWorkbook.WindowWidth = 8000;

                        // Some optional properties of the Document
                        book.Properties.Author = "HSRP";
                        book.Properties.Title = "Report";
                        book.Properties.Created = DateTime.Now;

                #region  Add Styles  sheet formate in excel

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
                style6.Font.Bold = false;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle styleHeader = book.Styles.Add("HeaderStyleHeader");
                styleHeader.Font.FontName = "Tahoma";
                styleHeader.Interior.Color = "Red";
                styleHeader.Font.Size = 10;
                styleHeader.Font.Bold = true;
                styleHeader.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                styleHeader.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                styleHeader.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                styleHeader.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                styleHeader.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                #endregion

                Worksheet sheet = book.Worksheets.Add("Report");

                #region UpperPart of Excel
                AddColumnToSheet(sheet, 100, dt.Columns.Count);
                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                //  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report Name :", "HeaderStyle3"));

                row.Cells.Add(new WorksheetCell(reportname, "HeaderStyle3"));//"Cumulative Collection and Deposit Report", "HeaderStyle3"));

             

                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("AP", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("TELANGNA", "HeaderStyle2"));
                //AddNewCell(row, "State:", "HeaderStyle2", 1);

                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                //DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                //DateTime dates1 = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                //dates = dates1.AddDays(-1);


                if (DateTime.Now.ToString("dddd") == "Monday")
                {
                    row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(System.DateTime.Now.AddDays(-2).ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                }
                else
                {
                    row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(System.DateTime.Now.AddDays(-1).ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                }

               // row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
               //// row.Cells.Add(new WorksheetCell(System.DateTime.Now.AddDays(-1).ToString(), "HeaderStyle2"));
               // row.Cells.Add(new WorksheetCell(System.DateTime.Now.AddDays(-1).ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                
                //AddNewCell(row, DropDownListStateName.SelectedItem.Text, "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

               //ateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

               
                // AddNewCell(row, "Report Duration:", "HeaderStyle2", 1);
                // AddNewCell(row, OrderDate.SelectedDate.ToString("dd/MMM/yyyy") + "-" + HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
                //  row = sheet.Table.Rows.Add();

                row.Index = iIndex++;
                row.Index = iIndex++;
                #endregion

                #region Column Creation and Assign Data
                string RTOColName = string.Empty;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //if (dt.Columns[i].ColumnName.ToString().Length < 2)
                    //{
                    //    AddNewCell(row, dt.Columns[i].ColumnName.ToString().Remove(dt.Columns[i].ColumnName.ToString().Length, 0), "HeaderStyleHeader", 1);
                    //}
                    //else
                    //{
                    //    AddNewCell(row, dt.Columns[i].ColumnName.ToString().Remove(dt.Columns[i].ColumnName.ToString().Length - 2, 0), "HeaderStyleHeader", 1);
                    //}

                    AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyleHeader", 1);
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
                row = sheet.Table.Rows.Add();


                #endregion
               // HttpContext context = HttpContext.Current;
             //   context.Response.Clear();
                // Save the file and open it
                //book.Save(Response.OutputStream);
                string filename = spname +"_"  + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".xls";
                //string filename = "Collection_V_S_Deposit" + "_" + strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                SetFolder("", cmbState.Text, spname + "_" + strMonth + strYear + "_");
                book.Save(strPath + "\\" + filename);
                //System.Threading.Thread.Sleep(1000);
                        }
                     
                //context.Response.ContentType = "text/csv";
              //  context.Response.ContentType = "application/vnd.ms-excel";
                //string filename = "Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

               // context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
               // context.Response.End();

                    }
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



        #region For Haryana Report Send Auto mail

        private void GenerateHRAuthorityReport()
        {
            try
            {
                string FormDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + "00:00:00";
                string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + "00:00:00";

                //DataTable dt = new DataTable();
                string SQLString = "exec Business_DateYearMonthWise_Collection_Summary '2015/06/22',4,B";
                DataTable dt = GetDataInDT(SQLString);
                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    string filename = "Collection_V_S_Deposit_HR" + "_" + strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();
                    SetFolder("", cmbState.Text, "Collection_V_S_Deposit_HR" + "_" + strMonth + strYear + "_");

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
                    style.Font.FontName = "Andalus";
                    style.Font.Size = 11;
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


                    row.Index = 1;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));

                    row = sheet.Table.Rows.Add();


                    row.Index = 2;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("Cumulative Collection and Deposit Report");
                    cell.MergeAcross = 3;
                    cell.StyleID = "HeaderStyle3";

                    row = sheet.Table.Rows.Add();


                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("HARYANA", "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    // Skip one row, and add some text

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
                    row = sheet.Table.Rows.Add();

                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["district"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Orders"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Opening Cash Balance"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Collection"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Deposit"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Balance Cash"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day1"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day2"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day3"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day4"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day5 Or More"].ToString(), DataType.Number, "HeaderStyle15"));


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

        #region For Bihar State

        private void GenerateBRAuthorityReport()
        {
            try
            {
                string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

                string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

                DataTable dt = new DataTable();
                string SQLString = "exec Business_DateYearMonthWise_Collection_Summary '2015/06/22',1,B";
                DataTable ds = GetDataInDT(SQLString);
                string RTOColName = string.Empty;
                int sno = 0;
                if (ds.Rows.Count > 0)
                {
                    string filename = "Collection_V_S_Deposit_Bihar" + "_" + strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();
                    SetFolder("", cmbState.Text, "Collection_V_S_Deposit_Bihar" + "_" + strMonth + strYear + "_");
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

                    row.Index = 1;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));

                    row = sheet.Table.Rows.Add();

                    row.Index = 2;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("Cumulative Collection and Deposit Report");
                    cell.MergeAcross = 3;
                    // Merge two cells togetherto 
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


                    row = sheet.Table.Rows.Add();
                    row.Index = 7;

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

                    row = sheet.Table.Rows.Add();
                    // Loop over the rows.

                    foreach (DataRow dtrows in ds.Rows)
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));


                        row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["district"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Orders"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Opening Cash Balance"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Collection"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Deposit"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Balance Cash"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day1"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day2"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day3"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day4"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day5 Or More"].ToString(), DataType.Number, "HeaderStyle15"));


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

        #region For HIMACHAL PRADESH

        private void GenerateHPAuthorityReport()
        {
            try
            {
                string FromDate = dtpFrmDate.Value.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();

                string ToDate = dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59";

                DataTable dt = new DataTable();
                string SQLString = "exec Business_DateYearMonthWise_Collection_Summary '2015/06/22',3,B";
                DataTable ds = GetDataInDT(SQLString);

                string RTOColName = string.Empty;
                int sno = 0;


                if (ds.Rows.Count > 0)
                {
                    string filename = "Collection_V_S_Deposit_HP" + "_" + strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();
                    SetFolder("", cmbState.Text, "Collection_V_S_Deposit_HP" + "_" + strMonth + strYear + "_");

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

                    row.Index = 1;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));

                    row = sheet.Table.Rows.Add();


                    row.Index = 2;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("Cumulative Collection and Deposit Report");
                    cell.MergeAcross = 3;
                    // Merge two cells togetherto 
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

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));


                    row = sheet.Table.Rows.Add();

                    row.Index = 7;

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

                    row = sheet.Table.Rows.Add();
                    // Loop over the rows.
                    foreach (DataRow dtrows in ds.Rows)
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["district"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Orders"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Opening Cash Balance"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Collection"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Deposit"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Balance Cash"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day1"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day2"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day3"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day4"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day5 Or More"].ToString(), DataType.Number, "HeaderStyle15"));
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

        #region For UTTRAKHAND

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

                    row.Index = 1;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));


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

                    row = sheet.Table.Rows.Add();
                    row.Index = 7;

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

                    row = sheet.Table.Rows.Add();

                    foreach (DataRow dtrows in ds.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));


                        row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["district"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Orders"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Opening Cash Balance"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Collection"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Deposit"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Balance Cash"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day1"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day2"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day3"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day4"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["Day5 Or More"].ToString(), DataType.Number, "HeaderStyle15"));

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