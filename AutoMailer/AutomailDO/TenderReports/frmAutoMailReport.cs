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

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSRPHPConnectionString"].ConnectionString);
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
                string strState = "select distinct rc.StateID as HSRP_StateID,hs.HSRPStateName as HSRPStateName from tbl_OutStanding_Dealer rc join [HSRPState] hs on rc.StateID=hs.HSRP_StateID where rc.StateID=11";
               // string strState = "SELECT distinct [HSRP_StateID],[HSRPStateName] from  [dbo].[HSRPState] order by [HSRPStateName]";
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

            strPath = "D:\\AllTenderReports";
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

        public void timer1_Tick(object sender, EventArgs e)
        {           
            string strDate = DateTime.Now.ToString("hh:mm:ss tt");
            #region  In case of Local
            string strTime1 = "09:10:02 AM";

            if (DateTime.Parse(strDate) == DateTime.Parse(strTime1))
            {
            #endregion
                button2_Click(button2, new EventArgs());

            }            
        }

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

     

        //private void CreateTaskRunDaily()
        //{
        //    using (TaskService ts = new TaskService())
        //    {
        //        TaskDefinition td = ts.NewTask();
        //        td.RegistrationInfo.Description = "My first task scheduler";

        //        DailyTrigger daily = new DailyTrigger();
        //        daily.StartBoundary = Convert.ToDateTime(DateTime.Today.ToShortDateString() + " 16:30:00");
        //        daily.DaysInterval = 1;
        //        td.Triggers.Add(daily);

        //        td.Actions.Add(new ExecAction(@"C:/sample.exe", null, null));
        //        ts.RootFolder.RegisterTaskDefinition("TaskName", td);
        //    }
        //}


        #endregion

        private void frmAutoMailReport_Load(object sender, EventArgs e)
        {
            
            GetState();
           // button2_Click(button1, new EventArgs());
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
                string strfromdate = string.Empty;
                if (DateTime.Now.ToString("dddd") == "Monday")
                {
                    strfromdate = "05/01/2016 00:00:00";
                    strcurrentdate = System.DateTime.Now.AddDays(-2).ToString();
                }
                else
                {
                    strfromdate = "05/01/2016 00:00:00";
                    strcurrentdate = System.DateTime.Now.AddDays(-1).ToString();
                }               
                string SQLStrings = "exec usp_OutStandingDealer";
                DataTable dtsp = GetDatasp(SQLStrings);               
                if (dtsp.Rows.Count > 0)
                {
                    for (int Temp=0; Temp < dtsp.Rows.Count; Temp++) 
                    {
                        dt = new DataTable();
                        // Call String Variable for Sp variable 
                        string strstateid = string.Empty;
                        string spname = string.Empty;
                        string strdatetime = string.Empty;                        
                        string strprocedure =string.Empty;
                        string rtolocationid = string.Empty;
                        string paymentgateways = string.Empty;
                        string dealername = string.Empty;
                        string reporttype = string.Empty;
                        string reportname = string.Empty;                        

                        strstateid = dtsp.Rows[Temp][0].ToString().Trim();
                        spname = dtsp.Rows[Temp][1].ToString().Trim();
                        rtolocationid = dtsp.Rows[Temp][2].ToString().Trim();
                        paymentgateways=dtsp.Rows[Temp][3].ToString().Trim();
                        dealername = dtsp.Rows[Temp][4].ToString().Trim();                     
                        reporttype = dtsp.Rows[Temp][5].ToString().Trim();
                        reportname = dtsp.Rows[Temp][6].ToString().Trim();                      

                        if (reporttype == "DO")
                        {
                            strprocedure = "[" + spname + "] " + "'" + Convert.ToDateTime(strfromdate).ToString() + "'," + "'" + Convert.ToDateTime(strcurrentdate).ToString() + "'," + "'" + reporttype + "' ," + "'" + strstateid + "'," + "'" + rtolocationid + "'," + "'" + paymentgateways + "'," + "'" + dealername + "' ";                         
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
                row.Cells.Add(new WorksheetCell("Telangana", "HeaderStyle2"));
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
                            //Old
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
           
                string filename = spname +"_"  + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".xls";
                //string filename = "Collection_V_S_Deposit" + "_" + strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                SetFolder("", cmbState.Text, spname + "_" + strMonth + strYear + "_");
                book.Save(strPath + "\\" + filename);               
                        }                   
            
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



    }
}