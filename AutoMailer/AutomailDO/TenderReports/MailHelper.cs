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
using System.Net.Security;
using System.Configuration;


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
       // static MailMessage mMailMessage = new MailMessage();
        static Attachment att;
       // MailMessage mail = new MailMessage();

        static SqlConnection strcon = new SqlConnection(ConfigurationManager.ConnectionStrings["HSRPHPConnectionString"].ToString());
        #region  Get Data in database
        private static DataTable GetDataInDT()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = new SqlCommand("select * from SendReportEmailTG where StateID=11", strcon);
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
     
            msg.Subject = "Outstanding Dealer TG " + ""+ " Date: " + ss + ".";

            string x = "Dear Sir <br /> <br /> Please Find Attachements OutStanding Dealer Report. <br />  <br /> <br /> <br /> Regard <br /> <br /> IT Support Team";
          
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
            string fromAddress = "reportshsrp@gmail.com";
           // msg.To.Add("hsrphp@gmail.com");         

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
