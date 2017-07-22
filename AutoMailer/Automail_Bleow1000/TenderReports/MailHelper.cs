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
        public string GetDataInDT1(string dealerid)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = new SqlCommand("select u.Emailid from users u,dealermaster d where u.dealerid=d.dealerid and u.hsrp_stateid=11 and u.Emailid is not null and d.dealername='" + dealerid + "'", strcon);

                adp.SelectCommand.CommandTimeout = 0;
                adp.Fill(dt);
                return dt.Rows[0]["Emailid"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetDataupdateuserstatus(string dealerid)
        {
            try
            {
                strcon.Open();
                DataTable dt = new DataTable();
                //SqlDataAdapter adp = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("update dealermaster  set Flag='Y'  from dealermaster d inner join users u  on u.dealerid=d.dealerid where u.hsrp_stateid=11 and u.Emailid is not null and d.dealername='" + dealerid.Trim() + "'", strcon);
                cmd.ExecuteNonQuery();
                strcon.Close();
               // adp.SelectCommand.CommandTimeout = 0;
                //adp.Fill(dt);
               // return dt.Rows[0]["Flag"].ToString();
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public static void SendMailMessage(string to, string from)
        {           
            //to = "testerskylife@gmail.com";
            to = "developer0186@gmail.com";
            SmtpClient smtp = new SmtpClient();     
            //smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Host = "mail.hsrpts.com";
            smtp.EnableSsl = false;  
            //smtp.Credentials = new System.Net.NetworkCredential("reportshsrp@gmail.com", "rep#and137");
            smtp.Credentials = new System.Net.NetworkCredential("enquiry@hsrpts.com", "Link@1234");
           // smtp.EnableSsl = true;
            MailMessage msg = new MailMessage();          
            //string ss = System.DateTime.Now.ToShortDateString();
            msg.Subject = "Account balance information";//"Dealer TG " + "" + " Date: " + ss + ".";
            string body = "<h1></h1>";
            body += "<HTML><HEAD></HEAD>";
            //body += "<table bgcolor='#FFFFFF' border='0' width='760px' style='border: 1px solid #999; padding:16px;'><tr><td style='padding:13px; font-family:Helvetica;'>Dear Sir/Madam </td></tr><tr><td style='font-size: 14px; font-family:Helvetica; font-weight:italic; padding:13px;'><p>Greetings from  <span style='font-size: 16px; font-family:Bookman Old Style;'><b> Link Autotech Pvt Ltd.</b></span> </p></td></tr><tr><td style='font-size: 14px; font-family:Calibri (Body); padding:12px;'><p>We are beloved that you have become a part of  <span style='font-size:16px;'><b>“PAY DIRECT LAPL.”</b></span> </p></td></tr><tr><td style='font-size:14px; font-family:Calibri (Body); padding:16px;'><p> As the transactions are going smoothly, we want to bring it to your kind notice that your <span style='font-size:16px;'><b>“PAY DIRECT LAPL”</b></span> Account balance is below <span style='font-size:14px;'><b>Rs. 2000.</b></span> So kindly replenish your account balance for Smooth & uninterrupted services.</p> </td></tr><tr><td style='font-size: 13px; font-family:Bookman Old Style; padding:17px;'><p> Assuring you of our best services at all times.</p></td></tr><tr><td style='font-size: 14px; font-family:Bookman Old Style; padding:22px;'><span> Best Regards,</span></td></tr><tr><td style='font-size: 13px; font-family:Bookman Old Style; padding:20px;'><b><span>Link Autotech Private Limited.</span><br/><span>7306132943,7306698827</span></b></td></tr> </table>";
           // body += "<table bgcolor='#FFFFFF' border='0' width='627px' style='border: 1px solid #999;'><tr><td style='font-family:Helvetica; padding-top:20px; padding-left:12px;'>Dear Sir/Madam </td></tr><tr><td style='font-size: 13px; font-family:Helvetica; padding-top:30px; padding-left:12px;'><p>Greetings from  <span style='font-size: 15px; font-family:Bookman Old Style;'><b> Link Autotech Pvt Ltd.</b></span> </p></td></tr><tr><td style='font-size: 13px; font-family:Calibri (Body); padding-top:17px; padding-left:12px;'><p style='padding-left:12px;'>We are beloved that you have become a part of  <span style='font-size:15px;'><b>“PAY DIRECT LAPL”.</b></span> </p></td></tr><tr><td style='font-size:13px; font-family:Calibri (Body); padding-top:17px; padding-left:12px;'><p style='padding-left:12px;'> As the transactions are going smoothly, we want to bring it to your kind notice that your <span style='font-size:15px;'><b>“PAY DIRECT LAPL”</b></span> Account balance is below <span style='font-size:13px;'><b>Rs. 2000.</b></span> So kindly replenish your account balance for Smooth & uninterrupted services.</p> </td></tr><tr><td style='font-size: 13px; font-family:Bookman Old Style; padding-top:32px; padding-left:12px;'><p> Assuring you of our best services at all times.</p></td></tr><tr><td style='font-size: 13px; font-family:Bookman Old Style; padding-top:45px; padding-left:12px;'><span> Best Regards,</span></td></tr><tr><td style='font-size: 13px; font-family:Bookman Old Style; padding-top:25px; padding-left:12px;'><b><span>Link Autotech Private Limited.</span><br/><span>7306132943,7306698827</span></b></td></tr> <tr><td style='padding-top:50px;'></td></tr> </table>";
           // body += "<table bgcolor='#FFFFFF' border='0' width='700px' style='border: 0px solid #999;'><tr><td style='top:15px; font-family:Helvetica; font-size:14.5px; padding-left:24px;'>Dear Sir, </td></tr><tr><td style='font-size: 14.5px; font-family:Helvetica; padding-top:30px; padding-left:22px;'><p>Greetings from  <span style='font-size: 14.5px; font-family:Bookman Old Style; font-style:italic;'><b> Link Autotech Pvt Ltd.</b></span> </p></td></tr><tr><td style='font-size:14.5px; font-family:Calibri; padding-top:20px; padding-left:12px;'><p style='padding-left:13px;'>We are appreciate that we have became a Partners in Online Registration Process, We are happy<br/> to share that all the transactions are going smoothly, we want to bring to your kind notice that your “PAY<br/> DIRECT LAPL” Account  balance has gone  below *Rs1000.  So kindly refill your account for seamless<br/> transaction. </td></tr><tr><td style='font-size: 14.5px; font-family:Bookman Old Style; padding-top:32px; padding-left:24px;'><p> Assuring you of our best services at all times.</p></td></tr><tr><td style='font-size: 14.5px; font-family:Bookman Old Style; padding-top:38px; padding-left:24px;'><span> Best Regards,</span></td></tr><tr><td style='font-size: 14.5px; font-family:Bookman Old Style; padding-top:25px; padding-left:24px;'><b><span>Link Autotech Private Limited</span><br/><span>+91 7306132943,+91 7306698827</span></b></td></tr> </table>";
           // body += "<table bgcolor='#FFFFFF' border='0' width='700px' style='border: 0px solid #999;'><tr><td style='top:15px; font-family:Helvetica; font-size:14.5px; padding-left:24px;'>Dear Sir "+to+", </td></tr><tr><td style='font-size: 14.5px; font-family:Helvetica; padding-top:30px; padding-left:22px;'><p>Greetings from  <span style='font-size: 14.5px; font-family:Bookman Old Style; font-style:italic;'><b> Link Autotech Pvt Ltd.</b></span> </p></td></tr><tr><td style='font-size:14.5px; font-family:Calibri; padding-top:20px; padding-left:12px;'><p style='padding-left:13px;'>We are appreciate that we have became a Partners in Online Registration Process, We are happy to share that all the transactions are going smoothly, we want to bring to your kind notice that your “PAY DIRECT LAPL” Account  balance has gone  below *Rs1000.  So kindly refill your account for seamless transaction. </td></tr><tr><td style='font-size: 14.5px; font-family:Bookman Old Style; padding-top:32px; padding-left:24px;'><p> Assuring you of our best services at all times.</p></td></tr><tr><td style='font-size: 14.5px; font-family:Bookman Old Style; padding-top:38px; padding-left:24px;'><span> Best Regards,</span></td></tr><tr><td style='font-size: 14.5px; font-family:Bookman Old Style; padding-top:25px; padding-left:24px;'><b><span>Link Autotech Private Limited</span><br/><span>+91 7306132943,+91 7306698827</span></b></td></tr> </table>";
            body += "<table bgcolor='#FFFFFF' border='0' width='700px' style='border: 0px solid #999;'><tr><td style='top:15px; font-family:Helvetica; font-size:14.5px; padding-left:24px;'>Dear Sir, </td></tr><tr><td style='font-size: 14.5px; font-family:Helvetica; padding-top:30px; padding-left:22px;'><p>Greetings from  <span style='font-size: 14.5px; font-family:Bookman Old Style; font-style:italic;'><b> Link Autotech Pvt Ltd.</b></span> </p></td></tr><tr><td style='font-size:14.5px; font-family:Calibri; padding-top:20px; padding-left:12px;'><p style='padding-left:13px;'>We are appreciate that we have became a Partners in Online Registration Process, We are happy to share that all the transactions are going smoothly, we want to bring to your kind notice that your “PAY DIRECT LAPL” Account  balance has gone  below So kindly refill your account for seamless transaction. </td></tr><tr><td style='font-size: 14.5px; font-family:Bookman Old Style; padding-top:32px; padding-left:24px;'><p> Assuring you of our best services at all times.</p></td></tr><tr><td style='font-size: 14.5px; font-family:Bookman Old Style; padding-top:38px; padding-left:24px;'><span> Best Regards,</span></td></tr><tr><td style='font-size: 14.5px; font-family:Bookman Old Style; padding-top:25px; padding-left:24px;'><b><span>Link Autotech Private Limited</span><br/><span>+91 7306132943,+91 7306698827</span></b></td></tr> </table>";
            body += "</FONT></DIV></BODY></HTML>";          
            msg.Body = body;          
           // System.Net.Mail.Attachment att; 
            msg.IsBodyHtml = true;
            msg.To.Add(to);
            //msg.To.Add("developer0186@gmail.com"); 
            //string fromAddress = "reportshsrp@gmail.com";
            string fromAddress = "enquiry@hsrpts.com";
            msg.From = new MailAddress(fromAddress,"HSRP-TS");
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
