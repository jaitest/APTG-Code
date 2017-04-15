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
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["hsrpConfigurationHR"].ConnectionString);
        SqlConnection conap = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringAP"].ConnectionString);
        SqlConnection contg = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringTG"].ConnectionString);
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
                string strState = "SELECT distinct [HSRP_StateID],[HSRPStateName] from  [dbo].[HSRPState] order by [HSRPStateName]";
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
            strPath = "G:\\AllTenderReports";
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
                    List<SqlConnection> List = new List<SqlConnection>();
                    List.Add(con);
                    List.Add(conap);
                    List.Add(contg);

                    for (int i = 0; i < List.Count; i++)
                    {
                        Export(List[i]);
                    }
                    string CurrentDate = System.DateTime.Now.ToShortDateString();
                    string directoryPath = strPath;
                    DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
                    MailHelper.SendMailMessage("reportshsrp@gmail.com", directoryPath, strPath);
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

            string strDate = DateTime.Now.ToString("h:mm:ss tt");
            #region  In case of Local
            string strTime1 = "07:30:02 AM";

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


        private DataSet GetDatasp(string strSqlString, SqlConnection localConnecion)
        {
            try
            {
                //DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(strSqlString, localConnecion);
                //adp.SelectCommand = new SqlCommand(strSqlString, con);
                //adp.SelectCommand.CommandTimeout = 0;
                adp.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void Export(SqlConnection localConnecion)
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


               // string strcurrentdate = System.DateTime.Now.ToShortDateString();
              //  string strcurrentdate = System.DateTime.Now.AddDays(-1).ToString();

                string SQLStrings = "exec usp_collectionreport";               
                DataSet ds = GetDatasp(SQLStrings, localConnecion);
               
                // DataTable dtsp = GetDatasp(SQLStrings);

                ///state ID  4

             
                string NoofSateid = string.Empty;
                string NoofRecord = string.Empty;
                string strstateid = string.Empty;
                string statename = string.Empty;


                NoofSateid = ds.Tables[0].Rows.Count.ToString().Trim();

                NoofRecord = ds.Tables[1].Rows.Count.ToString().Trim();

                if (Convert.ToInt32(NoofSateid) > 0)
                {

                    for (int z = 0; z < Convert.ToInt32(NoofSateid); z++)
                    {

                        strstateid = ds.Tables[0].Rows[z]["StateID"].ToString().Trim();
                        statename = ds.Tables[0].Rows[z]["StateName"].ToString().Trim();
                     
                        if (strstateid == "9")
                        {
                            localConnecion = conap;

                        }
                        else if (strstateid == "11")
                        {
                            localConnecion = contg;

                        }

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int Temp = 0; Temp < ds.Tables[1].Rows.Count; Temp++)  // for (int k = 0; k < dtsp.Rows.Count; k++)
                            {
                                dt = new DataTable();
                                // Call String Variable for Sp variable 

                                string spname = string.Empty;
                                string strdatetime = string.Empty;
                                string no_of_parameters = string.Empty;
                                string strprocedure = string.Empty;
                                string reporttype = string.Empty;
                                string reportname = string.Empty;

                                spname = ds.Tables[1].Rows[Temp]["SPName"].ToString().Trim();
                                no_of_parameters = ds.Tables[1].Rows[Temp]["NOPerameter"].ToString().Trim();
                                reporttype = ds.Tables[1].Rows[Temp]["Report_Type"].ToString().Trim();
                                reportname = ds.Tables[1].Rows[Temp]["ReportName"].ToString().Trim();

                                if (Convert.ToInt32(no_of_parameters) == 2)
                                {
                                    strprocedure = "[" + spname + " ]" + "'" + strcurrentdate + "'  ," + strstateid;
                                }

                                else if (Convert.ToInt32(no_of_parameters) == 3)
                                {
                                    strprocedure = "[" + spname + "] " + "'" + strcurrentdate + "'," + "'" + strstateid + "'," + "'" + reporttype + "' ";
                                }

                                //  MessageBox.Show(strprocedure);
                                SqlCommand da = new SqlCommand("exec " + strprocedure + "", localConnecion);

                                localConnecion.Open();
                                da.CommandTimeout = 500000;
                                dt.Load(da.ExecuteReader());
                                localConnecion.Close();


                #endregion


                                // Add some styles to the Workbook
                                if (dt.Rows.Count > 0)
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
                                    
                                    row.Cells.Add(new WorksheetCell("Report Name :", "HeaderStyle3"));

                                    row.Cells.Add(new WorksheetCell(reportname, "HeaderStyle3"));
                                   

                                    row = sheet.Table.Rows.Add();
                                    row.Index = iIndex++;
                                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                                    row.Cells.Add(new WorksheetCell(statename, "HeaderStyle2"));
                                                                      
                                    row = sheet.Table.Rows.Add();
                                    row.Index = iIndex++;                                   
                                    if (DateTime.Now.ToString("dddd") == "Monday")
                                    {
                                        row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                                        row.Cells.Add(new WorksheetCell(System.DateTime.Now.AddDays(-2).ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                                        row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                                        row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                                    }
                                    else
                                    {
                                        row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                                        row.Cells.Add(new WorksheetCell(System.DateTime.Now.AddDays(-1).ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                                        row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                                        row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                                    }                                   
                                    row = sheet.Table.Rows.Add();
                                    row.Index = iIndex++;
                                    row.Index = iIndex++;
                                    row.Index = iIndex++;
                                    #endregion

                                    #region Column Creation and Assign Data
                                    string RTOColName = string.Empty;
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {   
                                        AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyleHeader", 1);
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
                                    row = sheet.Table.Rows.Add();


                                    #endregion
                                  
                                    string filename = spname + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".xls";

                                    //string filename = "Collection_V_S_Deposit" + "_" + strMonth + strYear + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

                                    SetFolder("", "BusinessReport", spname + "_" + strMonth + strYear + "_");
                                    book.Save(strPath + "\\" + filename);
                                  
                                }

                               

                            }

                           

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