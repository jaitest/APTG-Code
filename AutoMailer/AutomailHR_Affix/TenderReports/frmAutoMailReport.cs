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

        private string SetFolder(string strRTO, string strState, string strFile)
        {
            string DateFolder = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString();

            strPath = "D:\\AllTender";
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
                    if (directoryPath.ToString() != "")
                    {
                        DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
                        MailHelper.SendMailMessage("reportshsrp@gmail.com", directoryPath, strPath);
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
            string strTime1 = "08:18:12 AM";          
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

        string strfromdate = System.DateTime.Now.ToString("MM-dd-yyyy");
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
                    strfromdate = System.DateTime.Now.AddDays(-2).ToString("MM-dd-yyyy");
                    strtodate = System.DateTime.Now.AddDays(-2).ToString("MM-dd-yyyy 23:59:00");
                   // strtodate = System.DateTime.Now.AddDays(-2).ToString("MM-dd-yyyy 23:59:00");
                    //strtodate = System.DateTime.Now.AddDays(-2).ToString();//System.DateTime.Now.ToString();
                }
                else
                {
                    strfromdate = System.DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy");
                    strtodate = System.DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy 23:59:00");
                    //strtodate = System.DateTime.Now.ToString();
                }

                //string SQLStrings = "exec USP_TenderReport_HR_Schedule_F";
                string SQLStrings = "exec Affix_Coll_FaridabadRTA";
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
                        string no_of_parameters=string.Empty;
                        string strprocedure =string.Empty;
                        string strlocationid = string.Empty;
                        string reportname = string.Empty;
                       // string strlocationid = string.Empty;


                        strstateid = dtsp.Rows[Temp][0].ToString().Trim();
                        spname = dtsp.Rows[Temp][1].ToString().Trim();
                        no_of_parameters=dtsp.Rows[Temp][2].ToString().Trim();
                        strlocationid = dtsp.Rows[Temp][3].ToString().Trim();
                        reportname = dtsp.Rows[Temp][4].ToString().Trim();
                      

                        if(Convert.ToInt32(no_of_parameters)==0)
                        {
                            //strprocedure = "[" + spname + "] " + "'" + strfromdate + "'," + "'" + strtodate + "'," + "'" + strstateid + "' ";
                            strprocedure = "[" + spname + "] ";
                                                   
                        }

                        //else if (Convert.ToInt32(no_of_parameters) == 4)
                        //{
                        //    strprocedure = "[" + spname + "] " + "'" + strfromdate + "'," + "'" + strtodate + "'," + "'" + strstateid + "'," + "'" + strlocationid + "' ";
                        //}
                    
                        SqlCommand da = new SqlCommand("exec " + strprocedure + "", con);
                        
                        con.Open();
                        da.CommandTimeout = 800000;
                        //da.CommandTimeout = con.ConnectionTimeout;
                        dt.Load(da.ExecuteReader());
                        con.Close();
                #endregion
                        Document document = new Document();
                        PdfPTable table = new PdfPTable(13);
                        var colWidthPercentages = new[] { 25f, 60f, 45f, 50f, 45f, 45f, 45f, 60f, 60f, 45f, 45f, 45f, 45f};
                        table.SetWidths(colWidthPercentages);                       
                        table.TotalWidth = 815f;
                        table.LockedWidth = true;
                        //  GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);
                       // GenerateCell(table, 13, 0, 0, 0, 0, 1, 0, "", 20, 0);
                        GenerateCell(table, 13, 0, 0, 0, 0, 0, 0, "", 20, 0);
                        GenerateCell(table, 13, 0, 0, 0, 0, 1, 0, "Report : -" + reportname, 20, 0);
                        GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "State Name : HARYANA", 15, 0);
                        GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, " RTO Location : - FARIDABAD RTA", 15, 0);
                        GenerateCell(table, 2, 0, 0, 0, 0, 0, 0, " Vehicle Reference: Both", 15, 0);
                        GenerateCell(table, 2, 0, 0, 0, 0, 0, 0, " Vehicle Type: All ", 15, 0);
                        GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "Report Date :" + strfromdate, 15, 0);
                        GenerateCell(table, 13, 0, 0, 0, 0, 0, 0, "", 20, 0);
                        #region Commented Old Name Of Headers
                        
                        #endregion

                        GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 40, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Location", 40, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "CashReceipt No", 40, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 40, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Class", 20, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Type", 40, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Reg. No", 40, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front Laser No", 40, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear Laser No", 40, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front Plate Size", 40, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear Plate Size", 40, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "3RD Sticker", 40, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Amount", 40, 0);
            
            #region Dynamic Rows
                        string filename = string.Empty;
                        if (dt.Rows.Count ==0)
                        {
                            lblmsg.Text = "Not Get FaridabadRTA Affixation Today";
                           // return;

                            //filename = "FaridabadRTA Collection" + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".pdf";
                            //BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                            //SetFolder("", cmbState.Text, reportname + "_" + strMonth + strYear + "_");
                            //strPath = strPath + "\\" + filename;
                            //// postedFile.SaveAs(strPath + "\\" + filename);
                            //// strPath = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                            //PdfWriter.GetInstance(document, new FileStream(strPath, FileMode.Create));
                            //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                            document.Open();
                        }
                            

                            else
                            {
                                lblmsg.Text = "";
                                foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                                {
                                    GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, dtrows["SNo"].ToString(), 20, 0);
                                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["Location"].ToString(), 20, 0);
                                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["cashreceiptno"].ToString(), 20, 0);
                                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["ownername"].ToString(), 20, 0);
                                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["vehicleclass"].ToString(), 20, 0);
                                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["vehicletype"].ToString(), 20, 0);
                                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["vehicleregno"].ToString(), 20, 0);
                                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["hsrp_front_lasercode"].ToString(), 20, 0);
                                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["hsrp_rear_lasercode"].ToString(), 20, 0);
                                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["FrontPlateSize"].ToString(), 20, 0);
                                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["RearPlateSize"].ToString(), 20, 0);
                                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["stickermandatory"].ToString(), 20, 0);
                                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrows["roundoff_netamount"].ToString(), 20, 0);
                                }

            #endregion

                                filename = "FaridabadRTA Affixation" + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() +".pdf";
                                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                                SetFolder("", cmbState.Text, reportname + "_" + strMonth + strYear + "_");
                                strPath = strPath + "\\" + filename;
                                PdfWriter.GetInstance(document, new FileStream(strPath, FileMode.Create));
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