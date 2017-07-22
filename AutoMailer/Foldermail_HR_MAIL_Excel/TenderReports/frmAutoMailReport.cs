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
        
        #region Send Mail Button

        private void button2_Click(object sender, EventArgs e)
        {     
            try
            {
                READExcel(@"D:\\Testdata\\Attendance .xlsx");
                lblmsg.Text = "Mail Massanger Successfully";
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        private static void READExcel(string path)
        {
            //Instance reference for Excel Application
            Microsoft.Office.Interop.Excel.Application objXL = null;
            //Workbook refrence
            Microsoft.Office.Interop.Excel.Workbook objWB = null;
            DataSet ds = new DataSet();
            try
            {
                //Instancing Excel using COM services
                objXL = new Microsoft.Office.Interop.Excel.Application();
                //Adding WorkBook
                objWB = objXL.Workbooks.Open(path);

                foreach (Microsoft.Office.Interop.Excel.Worksheet objSHT in objWB.Worksheets)
                {
                    int rows = objSHT.UsedRange.Rows.Count;
                    int cols = objSHT.UsedRange.Columns.Count;
                    DataTable dt = new DataTable();
                    int noofrow = 1;

                    //If 1st Row Contains unique Headers for datatable include this part else remove it
                    //Start
                    for (int c = 1; c <= cols; c++)
                    {
                        string colname = objSHT.Cells[1, c].Text;
                        dt.Columns.Add(colname);
                        noofrow = 2;
                    }
                    //END

                    for (int r = noofrow; r <= rows; r++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int c = 1; c <= cols; c++)
                        {
                            dr[c - 1] = objSHT.Cells[r, c].Text;
                        }
                        dt.Rows.Add(dr);
                    }

                    ds.Tables.Add(dt);
                    List<string> IT = new List<string>();
                  
                    IT.Add("HR");
                    //IT.Add("ADMIN");
                    //IT.Add("PURCHASE");
                    //IT.Add("IT");
                    //IT.Add("R&D");
                    //IT.Add("BUSINESS DEVELOPMENT");
                    //IT.Add("TENDERING");
                    //IT.Add("SG-Sales & Mktg");
                    //IT.Add("VTS-Sales & Mktg");
                    //IT.Add("VTS- Technical & Project Manegment");
                    //IT.Add("TEST LANE");
                    //IT.Add("SMART CARD - OPS");
                    //IT.Add("HSRP - OPS");
                    //IT.Add("ACCOUNTS");
                    

                    Dictionary<string, string> ObjEmailIds = new Dictionary<string, string>();
                    
                    //ObjEmailIds.Add("IT","ravikmunjal@gmail.com");
                    //ObjEmailIds.Add("HR", "ravimunjal@gmail.com");
                   
                    ObjEmailIds.Add("HR", "developer0186@gmail.com");
                    //ObjEmailIds.Add("ADMIN", "developer0186@gmail.com");
                    //ObjEmailIds.Add("PURCHASE", "developer0186@gmail.com");
                    //ObjEmailIds.Add("IT", "developer0186@gmail.com");
                    //ObjEmailIds.Add("R&D", "developer0186@gmail.com");
                    //ObjEmailIds.Add("BUSINESS DEVELOPMENT", "developer0186@gmail.com");
                    //ObjEmailIds.Add("TENDERING", "developer0186@gmail.com");
                    //ObjEmailIds.Add("SG-Sales & Mktg", "developer0186@gmail.com");
                    //ObjEmailIds.Add("VTS-Sales & Mktg", "developer0186@gmail.com");
                    //ObjEmailIds.Add("VTS- Technical & Project Manegment", "developer0186@gmail.com");
                    //ObjEmailIds.Add("TEST LANE", "developer0186@gmail.com");
                    //ObjEmailIds.Add("SMART CARD - OPS", "developer0186@gmail.com");
                    //ObjEmailIds.Add("HSRP - OPS", "developer0186@gmail.com");
                    //ObjEmailIds.Add("ACCOUNTS", "developer0186@gmail.com");
                    int RowsCount = 0;
                    foreach (string item in IT)
                    {
                        #region HTML Helper
                        htmlStr.Clear();
                        string x = "Dear Sir <br /> <br /> Please find today attendance report status at 9:30am of your department. This is for your information necessary action. Report status is as follows:- <br />  <br />";
                        htmlStr.Append(x);
                        htmlStr.Append("<table class='' cellspacing=0 border=1>");
                        htmlStr.Append("<tbody>");
                        htmlStr.Append("<tr style='height:40px;'>");
                        htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        htmlStr.Append("<nobr>Srl No</nobr>");
                        htmlStr.Append("</td>");
                        htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        htmlStr.Append("Pay Code</nobr>");
                        htmlStr.Append("</td>");
                        htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        htmlStr.Append("Employee Name</nobr>");
                        htmlStr.Append("</td>");
                        htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        htmlStr.Append("Designation</nobr>");
                        htmlStr.Append("</td>");
                        htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        htmlStr.Append("DOJ.</nobr>");
                        htmlStr.Append("</td>");
                        htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        htmlStr.Append("Location</nobr>");
                        htmlStr.Append("</td>");
                        htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        htmlStr.Append("Department</nobr>");
                        htmlStr.Append("</td>");
                        //htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        //htmlStr.Append("Location</nobr>");
                        //htmlStr.Append("</td>");
                        //htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        //htmlStr.Append("Company</nobr>");
                        //htmlStr.Append("</td>");
                        //htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        //htmlStr.Append("Shift</nobr>");
                        //htmlStr.Append("</td>");
                        htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        htmlStr.Append("Start</nobr>");
                        htmlStr.Append("</td>");
                        htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        htmlStr.Append("In</nobr>");
                        htmlStr.Append("</td>");
                        htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        htmlStr.Append("Status</nobr>");
                        htmlStr.Append("</td>");
                        htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:13px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px ; height: 30px;''>");
                        htmlStr.Append("Mode of Attendance</nobr>");
                        htmlStr.Append("</td>");
                        //htmlStr.Append("<td style='font-family:Calibri;text-align:center;font-size:11px;color:#000000;font-weight:bold;border-left:1px solid;border-right:1px solid;border-left-color:#000000;border-right-color:#000000;min-width:50px'>");
                        //htmlStr.Append("DepartMent</nobr>");     
                        //htmlStr.Append("</td>");
                        htmlStr.Append("</tr>"); 
                        #endregion

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {                            
                            if(dt.Rows[i]["Department"].ToString()==item)
                            {
                                ConvertDataToHTMl(dt, i);  
                            }
                        }
                        htmlStr.Append("</tbody>");
                        htmlStr.Append("</table>");
                        htmlStr.Append("<tr>");
                        htmlStr.Append("<td>");
                        htmlStr.Append("&nbsp; &nbsp; &nbsp;&nbsp;");
                        htmlStr.Append("</td>");
                        htmlStr.Append("</tr>");

                        string x1 = "Thanx & Regards";
                        htmlStr.Append(x1);
                        MailMessage msg = new MailMessage();
                        msg.To.Clear();
                        foreach (var KeyAndValues in ObjEmailIds)
                        {
                            if(KeyAndValues.Key==item)
                            {
                                msg.To.Add(KeyAndValues.Value);
                            }
                        }
                        string htmlhealper = Convert.ToString(htmlStr);
                        msg.Body = htmlhealper;
                        MailHelper.SendMailMessage(msg);                       
                    }
                    //button2.Visible = false;

                }

                //Closing workbook
                objWB.Close();
                //Closing excel application
                objXL.Quit();
                

            }
            catch (Exception ex)
            {
                objWB.Saved = true;
                //Closing work book
                objWB.Close();
                //Closing excel application
                objXL.Quit();
                //Response.Write("Illegal permission");
            }
        }

      
        #endregion

        #region For Timer Set

        public void timer1_Tick(object sender, EventArgs e)
        {
            string strDate = DateTime.Now.ToString("hh:mm:ss tt");
            #region  In case of Local
            string strTime1 = "04:00:12 PM";
            //string strTime1 = "11:12 AM";

            if (DateTime.Parse(strDate) == DateTime.Parse(strTime1))
            {
            #endregion
                button2_Click(button2, new EventArgs());
            }
           
         }


     

        #endregion

      
       static StringBuilder htmlStr = new StringBuilder("");
       private static void ConvertDataToHTMl(DataTable dt,int i)
        {
            htmlStr.Append("<tr style='height:30px;'>");
            htmlStr.Append("<td style='border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            htmlStr.Append("<nobr>" + dt.Rows[i]["Srl No."] + "</nobr>");
            htmlStr.Append("</td>");
            htmlStr.Append("<td style='font-family:Calibri;font-size:11px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            htmlStr.Append("<nobr>" + dt.Rows[i]["Pay Code"] + "</nobr>");
            htmlStr.Append("</td>");
            htmlStr.Append("<td style='font-family:Calibri;font-size:11px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            htmlStr.Append("<nobr>" + dt.Rows[i]["Employee Name"] + "</nobr>");
            htmlStr.Append("</td>");
            htmlStr.Append("<td style='font-family:Calibri;font-size:11px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            htmlStr.Append("<nobr>" + dt.Rows[i]["Designation"] + "</nobr>");
            htmlStr.Append("</td>");
            htmlStr.Append("<td style='font-family:Calibri;font-size:11px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            htmlStr.Append("<nobr>" + dt.Rows[i]["DOJ."] + "</nobr>");
            htmlStr.Append("</td>");
            htmlStr.Append("<td style='font-family:Calibri;font-size:11px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            htmlStr.Append("<nobr>" + dt.Rows[i]["Location"] + "</nobr>");
            htmlStr.Append("</td>");
            htmlStr.Append("<td style='border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            htmlStr.Append("<nobr>" + dt.Rows[i]["Department"] + "</nobr>");
            htmlStr.Append("</td>");
            //htmlStr.Append("<td style='border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            //htmlStr.Append("<nobr>" + dt.Rows[i]["Location"] + "</nobr>");
            //htmlStr.Append("</td>");
            //htmlStr.Append("<td style='border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            //htmlStr.Append("<nobr>" + dt.Rows[i]["Company"] + "</nobr>");
            //htmlStr.Append("</td>");
            //htmlStr.Append("<td style='font-family:Calibri;font-size:11px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            //htmlStr.Append("<nobr>" + dt.Rows[i]["Shift"] + "</nobr>");
            //htmlStr.Append("</td>");
            htmlStr.Append("<td style='border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            htmlStr.Append("<nobr>" + dt.Rows[i]["Start"] + "</nobr>");
            htmlStr.Append("</td>");
            htmlStr.Append("<td style='border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            htmlStr.Append("<nobr>" + dt.Rows[i]["In"] + "</nobr>");
            htmlStr.Append("</td>");
            htmlStr.Append("<td style='border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            htmlStr.Append("<nobr>" + dt.Rows[i]["Status"] + "</nobr>");
            htmlStr.Append("</td>");
            htmlStr.Append("<td style='font-family:Calibri;font-size:11px;color:#000000;font-weight:bold;border:1px solid;border-left-color:#000000;border-right-color:#000000;border-top-color:#000000;border-bottom-color:#000000;min-width:50px'>");
            htmlStr.Append("<nobr>" + dt.Rows[i]["Mode of Attendance"] + "</nobr>");
            htmlStr.Append("</td>");
           // htmlStr.Append("<td style='border-left:1px solid;border-right:1px solid;border-left-color:#000000;border-right-color:#000000;min-width:50px'>");
            //htmlStr.Append("<nobr>" + dt.Rows[i]["Departments"] + "</nobr>");
          //  htmlStr.Append("</td>");
            htmlStr.Append("</tr>");          

        }

       private void frmAutoMailReport_Load(object sender, EventArgs e)
       {
       }
    }
}