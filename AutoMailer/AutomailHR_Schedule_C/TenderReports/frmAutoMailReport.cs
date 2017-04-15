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
using iTextSharp.text.pdf;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TenderReports
{
    public partial class frmAutoMailReport : Form
    {
        public frmAutoMailReport()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HSRPHRConnectionString"].ConnectionString);
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
                string strState = "select distinct rc.StateID as HSRP_StateID,hs.HSRPStateName as HSRPStateName from tblReportCollection_SP rc join [HSRPState] hs on rc.StateID=hs.HSRP_StateID where rc.StateID=4";
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

        private string SetFolder(string strState, string strFile)
        {
            string DateFolder = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString();

            strPath = "D:\\Allhrlocation";
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
                    if(directoryPath.ToString()!="")
                    { 
                        DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
                       MailHelper.SendMailMessage("reportshsrp@gmail.com", directoryPath, strPath);
                        //MailHelper.SendMailMessage("developer0186@gmail.com", directoryPath, strPath);
                    }
                    else
                    {

                    }
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

            string strDate = DateTime.Now.ToString("hh:mm:ss tt");
            #region  In case of Local
            string strTime1 = "09:10:12 AM";
            //string strTime2 = "18:08 PM";

            if (DateTime.Parse(strDate) == DateTime.Parse(strTime1))
            {
            #endregion
                button2_Click(button1, new EventArgs());
            }
            //string strDate = DateTime.Now.ToString("hh:mm tt");

            //button2_Click(button1, new EventArgs());
         }


       

        #endregion

        private void frmAutoMailReport_Load(object sender, EventArgs e)
        {
            
            GetState();
           // button2_Click(button1, new EventArgs());
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

        string strfromdate = System.DateTime.Now.ToString("MM/dd/yyyy 00:00:00");

      
      
        private void Export()
        {
            try
            {

                #region Fetch Data
                DataTable dt = null;

                string strtodate = string.Empty;
                string strfromdate = string.Empty;
              
                if (DateTime.Now.ToString("dddd") == "Monday")
                {
                   // strtodate = System.DateTime.Now.ToString();
                    strfromdate = System.DateTime.Now.AddDays(-2).ToString("MM/dd/yyyy 00:00:00");
                    strtodate = System.DateTime.Now.AddDays(-2).ToString("MM/dd/yyyy 23:59:00");
                   
                }
                else
                {
                    strfromdate = System.DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy 00:00:00");
                    strtodate = System.DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy 23:59:00");
                    //strtodate = System.DateTime.Now.ToString();
                }

                string SQLStrings = "exec USP_TenderReport_HR_Schedule_C";
                DataTable dtsp = GetDatasp(SQLStrings);
                string filename = string.Empty;

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
                        string strlocationid = string.Empty;
                        string reportname = string.Empty;
                        string strtype = string.Empty;
                       // string strlocationid = string.Empty;


                        strstateid = dtsp.Rows[Temp][0].ToString().Trim();
                        spname = dtsp.Rows[Temp][1].ToString().Trim();
                        no_of_parameters=dtsp.Rows[Temp][2].ToString().Trim();
                        strlocationid = dtsp.Rows[Temp][3].ToString().Trim();
                        reportname = dtsp.Rows[Temp][4].ToString().Trim();
                        strtype = dtsp.Rows[Temp][5].ToString().Trim();
                      

                        //if(Convert.ToInt32(no_of_parameters)==3)
                        //{
                        //    strprocedure = "[" + spname + "] " + "'" + strfromdate + "'," + "'" + strtodate + "'," + "'" + strstateid + "' ";
                                                   
                        //}

                        if (Convert.ToInt32(no_of_parameters) == 5)
                        {
                            strprocedure = "[" + spname + "] " + "'" + strfromdate + "'," + "'" + strtodate + "'," + "'" + strstateid + "'," + "'" + strlocationid + "'," + "'" + strtype + "' ";
                        }
                    
                        SqlCommand da = new SqlCommand("exec " + strprocedure + "", con);
                        
                        con.Open();
                       // da.CommandTimeout = 800000;
                        //da.CommandTimeout = con.ConnectionTimeout;
                        dt.Load(da.ExecuteReader());
                        con.Close();
                #endregion

                        PdfPTable table = new PdfPTable(12);
                        var colWidthPercentages = new[] { 20f, 88f, 42f, 40f, 45f, 40f, 40f, 50f, 50f, 20f, 32f, 30f };
                        // var colWidthPercentages = new[] { 20f,  88f, 40f, 45f, 40f, 40f, 50f, 50f, 20f, 32f, 30f };
                        table.SetWidths(colWidthPercentages);
                        //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
                        string SqlQuery = string.Empty;
                        Document document = new Document();
                        //table.TotalWidth = 750f;
                        table.TotalWidth = 820f;
                        table.LockedWidth = true;
                        string formdate = string.Empty;
                        string todate = string.Empty;
                        if (DateTime.Now.ToString("dddd") == "Monday")
                        {
                             formdate = System.DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy");
                             todate = System.DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                             formdate = System.DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
                             todate = System.DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
                        }

                        GenerateCell(table, 12, 0, 0, 0, 0, 1, 0, "SCHEDULE-C", 20, 0);
                        GenerateCell(table, 12, 0, 0, 0, 0, 0, 0, "", 20, 0);
                        GenerateCell(table, 12, 0, 0, 0, 0, 1, 0, "Daily Report from Embossing Stations to Registering Authority", 20, 0);
                        GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "State Name : " + "HARYANA", 15, 0);
                       // GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "RTO Name:      " + dropDownListClient.SelectedItem.ToString(), 15, 0);
                       // GenerateCell(table, 4, 0, 0, 0, 0, 1, 0, "Date Period  :" + OrderDate.SelectedDate.ToString("dd/MMM/yyyy") + "-" + HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), 15, 0);
                        GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "Location : " + reportname, 15, 0);
                        GenerateCell(table, 4, 0, 0, 0, 0, 1, 0, "Date Period From : " + formdate + "  To : " + todate, 15, 0);

                        GenerateCell(table, 12, 0, 0, 0, 0, 1, 0, "", 30, 0);
                        GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Application No.", 40, 0);
                        //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Reg NO", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, " Order Date", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Type", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
                        GenerateCell(table, 2, 0, 1, 1, 1, 1, 0, "Laser Identification No", 20, 0);

                        GenerateCell(table, 2, 0, 1, 1, 1, 1, 0, "RP Size", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "3RD RP Y/N", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Color BackGround", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Remarks", 20, 0);


                        GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 40, 0);
                        //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front", 10, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear", 10, 0);

                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front", 10, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear", 10, 0);

                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Sticker", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
            
            
            
            #region Dynamic Rows
            if (dt.Rows.Count == 0)
            {
                //filename = "Schedule-C-HR_Embossing_Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".pdf";
                //filename =  reportname  + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".pdf";
                //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                //SetFolder(cmbState.Text, strprocedure);                
                //PdfWriter.GetInstance(document, new FileStream(strPath + "\\" + filename, FileMode.Create));
                lblmsg.Text = "Record not Find";
                document.Open();
            }
            else
            {
                lblmsg.Text = "";
                foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                {

                    GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, dtrows["SNo"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["vh1"].ToString(), 30, 0);
                    //GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["vh2"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["Hsrprecord_CreationDate"].ToString(), 20, 0);
                    GenerateCell(table, 1, -2, 1, 0, 1, 1, 1, dtrows["VehicleType"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["OwnerName"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["FrontLaserNo"].ToString(), 10, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["RearLaserNo"].ToString(), 10, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["FrontPlateSize"].ToString(), 10, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["RearPlateSize"].ToString(), 10, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["RDRPYN"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["ColorBackGround"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["Remarks"].ToString(), 20, 0);
            #endregion            

                }

               // HttpPostedFileBase postedFile;

                filename = reportname + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".pdf";
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                SetFolder(cmbState.Text, strprocedure);             
                PdfWriter.GetInstance(document, new FileStream(strPath + "\\" + filename, FileMode.Create));
                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                document.Open();
            }

            document.Add(table);
            document.NewPage();
            document.Close();

                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
   

        private void button1_Click(object sender, EventArgs e)
        {

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

        private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(0))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
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
            newCellPDF.NoWrap = false;
            if (!iRowHeight.Equals(0))
            {
                newCellPDF.FixedHeight = iRowHeight;
            }
            if (!iRowWidth.Equals(0))
            {
            }
            table.AddCell(newCellPDF);
        }






    }
}