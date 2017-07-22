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
        
        public MailHelper()
        {
            
        }
      
        public static void SendMailMessage(MailMessage msg)
        {            
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 25;

            smtp.Credentials = new System.Net.NetworkCredential("reportshsrp@gmail.com", "rep#and137");

            smtp.EnableSsl = true;            

            string ss = System.DateTime.Now.ToShortDateString();

            msg.Subject = "Report " + "" + " Date: " + ss + ".";          
          
            string fromAddress = "reportshsrp@gmail.com";
            //add more mail id...
            //msg.To.Add("developer0186@gmail.com");        

            msg.From = new MailAddress(fromAddress);
            msg.IsBodyHtml = true;

            try
            {
                smtp.Send(msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

    }
}
