using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


using System.Net.Security;


namespace SendMail
{
   
    public class MailHelper
    {
        /// <summary>
        /// Sends an mail message
        /// </summary>
        /// <param name="from">Sender address</param>
        /// <param name="to">Recepient address</param>
        /// <param name="bcc">Bcc recepient</param>
        /// <param name="cc">Cc recepient</param>
        /// <param name="subject">Subject of mail message</param>
        /// <param name="body">Body of mail message</param>
        ///  
        public MailHelper()
        {
            
        }
        static SqlConnection strcon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringAPTG"].ToString());
       // static MailMessage mMailMessage = new MailMessage();
        static Attachment att;
       // MailMessage mail = new MailMessage();

        #region  Get Data in database
        private static DataTable GetDataInDT()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = new SqlCommand("select * from SendReportEmail where StateID=9", strcon);
                //adp.SelectCommand = new SqlCommand("select * from SendReportS where StateID=9", strcon);
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

        public static void SendMailMessage(string from, string directoryPath, string strPath)
        {

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 25;

            smtp.Credentials = new System.Net.NetworkCredential("reportshsrp@gmail.com", "rep#and137");
          
           smtp.EnableSsl = true;
      
            MailMessage msg = new MailMessage();

            string ss = System.DateTime.Now.ToShortDateString();          
     
            msg.Subject = "Daily Report AP " + ""+ " Date: " + ss + ".";

            string x = "Dear Sir <br /> <br /> Please Find Attachements Business Report. <br />  <br /> <br /> <br /> Regard <br /> <br /> IT Support Team";
           // string x = "Dear Sir <br /> <br /> I am Sending You mail of Acumulative Collection and Production till Date Report <br />  <br />  <br />as you get the Attachment the same. <br /> Regard <br />  Please Find the Aattachment.<br /> IT Support Team";
            msg.Body = x;
            System.Net.Mail.Attachment att;
            directoryPath = strPath;
            DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);

            foreach (FileInfo fileToCompress in directorySelected.GetFiles())
            {
                att = new System.Net.Mail.Attachment(fileToCompress.FullName.ToString());
                msg.Attachments.Add(att);
            }


            //Email Multiple Send from Database 

            DataTable dt = new DataTable();
            dt = GetDataInDT();
            //string toAddress;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {


                        string[] arrayemail = dt.Rows[i]["EmailId"].ToString().Split(',');
                        for (int j = 0; j < arrayemail.Count(); j++)
                        {
                            msg.To.Add(arrayemail[j].ToString());
                        }
                    }
                }
                else
                {
                    msg.To.Add("ravikmunjal@gmail.com");
                }
            }
            else
            {
                msg.To.Add("ravikmunjal@gmail.com");
            }


            //More Address Add This Formate
            
           //  string toAddress = "ravikmunjal@gmail.com";
             //string toAddress =  "developer0186@gmail.com";  
           //  msg.To.Add(toAddress);

            string fromAddress = "reportshsrp@gmail.com";
            //msg.To.Add("sridhar.kumar@hsrpap.com");
            //msg.To.Add("shaik.ahamad@hsrpap.com");
            //msg.To.Add("ashok.sharma@linkutsav.com");
            //msg.To.Add("gaurav.kaushik@linkutsav.com");
            //msg.To.Add("corporatesolutions15@gmail.com");
            //msg.To.Add("muralidhar@hsrpts.com");
            //msg.To.Add("vasanthkumar@hsrpts.com");
            //msg.To.Add("corporatesolutions15@gmail.com");
            ////msg.To.Add("amitbhargavain@gmail.com");
            //msg.To.Add(" developer0186@gmail.com");
            //msg.To.Add("kantiswaroop.01@gmail.com");
            //msg.To.Add("nikhilgambhir25@gmail.com");
            //msg.To.Add("ramesh.k@hsrpap.com");
            //msg.To.Add("mailmeavatesh@gmail.com");
    
            msg.From = new MailAddress(fromAddress);
            msg.IsBodyHtml = true;

            try
             {
                smtp.Send(msg);             
            }
            catch( Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }
    }
}
