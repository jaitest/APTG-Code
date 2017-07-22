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
using System.Web;
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

        // All Function for repot 

        #region Send Mail Button

        private void button2_Click(object sender, EventArgs e)
        {
            //try
            //{

                if (DateTime.Now.ToString("dddd") != "Sunday")
                {
                    Export();
                    
                   // string CurrentDate = System.DateTime.Now.ToShortDateString();
                   // string directoryPath = strPath;
                   // DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
                   // MailHelper.SendMailMessage("reportshsrp@gmail.com", directoryPath, strPath);
                   // MailHelper.SendMailMessage("developer0186@gmail.com");
                }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        #endregion

        #region For Timer Set

        public void timer1_Tick(object sender, EventArgs e)
        {
            string strDate = DateTime.Now.ToString("h:mm:ss tt");
            #region  In case of Local
            string strTime1 = "08:30:02 AM";         

            if (DateTime.Parse(strDate) == DateTime.Parse(strTime1))
            {
            #endregion
                button2_Click(button1, new EventArgs());
            }
           
         }
        #endregion

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
                //DataTable dt = null;
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
             
                        string SQLStrings = "select b.UserFirstName as Dealer,(select sum(depositamount) from BankTransaction where BankTransaction.UserID=b.UserID and ApprovedStatus='Y' and convert(date,entrydate) between '" + strfromdate + "' and '" + strcurrentdate + "' )-SUM(CASE WHEN (convert(date,HSRPRecord_CreationDate) between '" + strfromdate + "' and '" + strcurrentdate + "')  THEN RoundOff_NetAmount ELSE 0 END ) as 'Opening Amount',SUM(CASE WHEN (convert(date,HSRPRecord_CreationDate) between '" + strfromdate + "' and '" + strcurrentdate + "')  THEN 1 ELSE 0 END ) as 'Opening Orders',SUM(CASE WHEN (convert(date,HSRPRecord_CreationDate) between '" + strfromdate + "' and '" + strcurrentdate + "')  THEN 1 ELSE 0 END ) as 'Selected Period Order',isnull((select sum(isnull(depositamount,0)) from BankTransaction where BankTransaction.UserID=b.UserID and ApprovedStatus='Y' and convert(date,entrydate) between '" + strfromdate + "' and '" + strcurrentdate + "'),0) as 'Selected Period Deposit',SUM(CASE WHEN (convert(date,HSRPRecord_CreationDate) between '" + strfromdate + "' and '" + strcurrentdate + "')  THEN RoundOff_NetAmount ELSE 0 END ) as 'Selected Period Utilized',isnull((select sum(depositamount) from BankTransaction where BankTransaction.UserID=b.UserID and ApprovedStatus='Y' and convert(date,entrydate) between '" + strfromdate + "' and '" + strcurrentdate + "' ),0)-isnull(SUM(CASE WHEN (convert(date,HSRPRecord_CreationDate) between '" + strfromdate + "' and '" + strcurrentdate + "')  THEN RoundOff_NetAmount ELSE 0 END ),0)+isnull((select sum(isnull(depositamount,0)) from BankTransaction where BankTransaction.UserID=b.UserID and ApprovedStatus='Y' and convert(date,entrydate) between '" + strfromdate + "' and '" + strcurrentdate + "'),0)-isnull(SUM(CASE WHEN (convert(date,HSRPRecord_CreationDate) between '" + strfromdate + "' and '" + strcurrentdate + "')  THEN RoundOff_NetAmount ELSE 0 END ),0) as 'Closing Balance' from hsrprecords a,users b where a.CreatedBy=UserID and isnull(b.dealerid,'')!='' and a.HSRP_StateID=11 and convert(date,HSRPRecord_CreationDate) between  '" + strfromdate + "' and '" + strcurrentdate + "' group by UserFirstName,UserID order by UserFirstName";
                        SqlCommand da = new SqlCommand(SQLStrings, con);
                        con.Open();
                        SqlDataAdapter adp = new SqlDataAdapter(da);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        da.CommandTimeout = 800000;                      
                        dt.Load(da.ExecuteReader());
                        con.Close();
                       
                #endregion

                    int balance = 0;
                    string dealername1 = string.Empty;
                   
                    //if (balance >= Convert.ToInt32(dt.Columns["Closing Balance"].ToString()))
                    //{
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            balance = Convert.ToInt32(dt.Rows[i]["Closing Balance"]);
                            dealername1 = dt.Rows[i]["dealer"].ToString();

                          

                            if (balance < 1000)
                            {
                                MailHelper obj = new MailHelper();
                                //MailHelper.SendMailMessage("developer0186@gmail.com");                          
                                //MailHelper.SendMailMessage(obj.GetDataInDT1(dt.Rows[i]["dealer"].ToString()).ToString(), "enquiry@hsrpts.com");
                                MailHelper.SendMailMessage(obj.GetDataInDT1(dt.Rows[i]["dealer"].ToString()).ToString(), "enquiry@hsrpts.com");
                                obj.GetDataupdateuserstatus(dt.Rows[i]["dealer"].ToString());
                                
                                //MailHelper.SendMailMessage(obj.GetDataInDT1(dt.Rows[i]["dealer"].ToString()).ToString(), "enquiry@hsrpts.com", dt.Rows[i]["Closing balance"].ToString());
                                 //MailHelper.SendMailMessage(dt.Rows[i]["dealer"].ToString(), "enquiry@hsrpts.com", dt.Rows[i]["Closing Balance"].ToString());
                            }

                        }
                    //}
               
               
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