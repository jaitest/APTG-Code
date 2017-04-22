using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MultiTrack;
using System.Data;
using System.Net.NetworkInformation;
using System.Net;
using System.Data.SqlClient;
using System.Text;


namespace HSRP
{
    public partial class Complaints : System.Web.UI.Page
    {
        public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        String aapID = String.Empty;
        String SQLString = String.Empty;
        String Status = String.Empty;
       // string SQLString = String.Empty;
        string CnnString = String.Empty;
        string strURL = string.Empty;
        string strState;


        MailHelper objMailSender = new MailHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            GetStateFromURL();
        }

        private void GetStateFromURL()
        {

            strURL = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
            strURL = "http://www.hsrpts.com/complaint.htm";
            lblComp.Text = strURL;
            // lblURL.Text = strURL;
            string[] ArrUrl = new string[3];
            ArrUrl = strURL.Split('.');
            for (int i = 0; i < ArrUrl.Length; i++)
            {
                if (ArrUrl[1].ToString().ToUpper().StartsWith("HSRP"))
                {
                 //   strState = ArrUrl[1].ToString().ToUpper().Replace("HSRP", string.Empty);
                    ViewState["State"] = ArrUrl[1].ToString().ToUpper().Replace("HSRP", string.Empty);
                    break;
                }
            }
        }

        string vehicalRegNo;

        public void GetHostAddress()
        {
            aapID = HttpContext.Current.Request.UserHostAddress.ToLower();
           
        }

        public void SaveVehiRegSerchLog()
        {          
           string vehicalRegNo = Session["REG"].ToString();
           string Query = ("INSERT INTO [VehicleStatus_SearchLog]([regno],[customer_IP],[datetimeofsearch])VALUES('" + vehicalRegNo + "','" + aapID + "','" + DateTime.Now.ToString() + "')");
            Utils.ExecNonQuery(Query, ConnectionString);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                string strVehicleNo = txtRegno.Text;
                string strCheck = strVehicleNo.Substring(0, 2);
                if (strCheck != "TS" && strCheck != "ts")
                {
                    lblMsg.Text = "Please Enter Valid Vehicle RegNo.";
                    return;
                }
                string strSel = "select right(isnull(max(complaintno),0000000),7) from complaint where stateid=11 and complaintno like 'TS-%'";
                string strCom = Utils.getScalarValue(strSel, ConnectionString);
                string strComNo = string.Empty;

                if (strCom.Equals(0))
                {
                    strComNo = "TS-0000001";
                }
                else
                {
                    strComNo = "TS-" + string.Format("{0:0000000}", Convert.ToInt32(strCom) + 1);
                }
                DateTime dtTime = System.DateTime.Now;
                
                strState = "11";
                string strQuery = "INSERT INTO [dbo].[Complaint] ([Region],ComplaintNo,[StateId],[OwnerName]" +
                        ",[Regno]" +
                        ",[EngineNo]" +
                        ",[ChasisNo]" +
                        ",[Remarks]" +
                        ",[MobileNo]" +
                        ",[email],[IPAddress])" +
                        "VALUES('" + ddlComplaintRegion.SelectedItem.Text + "','" + strComNo + "','" + strState + "','" + txtName.Text + "','" + txtRegno.Text +
                        "','" + txtEngineno.Text + "','" + txtChasisNo.Text + "','" + txtRemarks.Text + "','" + txtMobile.Text + "','" + txtMail.Text + "','" +
                        Context.Request.UserHostAddress.ToString() + "')";
                int i = Utils.ExecNonQuery(strQuery, ConnectionString);
                if (!i.Equals(0))
                {

                    string strMsg = "Your complaint has been successfully submitted with number " + strComNo + " ,for all future communication use this number.";
                    try
                    {
                        GenerateIdsAndSendMail(strComNo);
                    }
                    catch
                    { }
                    ddlComplaintRegion.SelectedIndex = 0;
                    txtChasisNo.Text = string.Empty;
                    txtRegno.Text = string.Empty;
                    txtName.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    txtEngineno.Text = string.Empty;
                    txtMobile.Text = string.Empty;
                    txtMail.Text = string.Empty;
                    lblMsg.Text = strMsg;
                   
                    //  ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('" + strMsg + "');", true);

                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
           
        }

        private void GenerateIdsAndSendMail(string strCompNo)
        {
            List<string> lst = new List<string>();
            //lst.Add("ravi.munjal@hsrpap.com");
            //lst.Add("amit.bhargava@hsrpap.com");
            //lst.Add("support@hsrpuk.com");

            //lst.Add("ram.k@hsrpap.com");
            //lst.Add("garima@hsrpap.com");
            ////aphead- // lst.Add("utsavrakesh@gmail.com");

            //lst.Add("info@linkutsav.com");
            ////lst.Add("spal.helpdesk@gmail.com");
            //lst.Add("svp@linkutsav.com");
            //lst.Add("maheshmmalhotra@gmail.com");
           // lst.Add("garima@hsrpap.com");
            lst.Add("ram.k@hsrpap.com");
            lst.Add("ashok.sharma@linkutsav.com");
            lst.Add("sridhar.kumar@hsrpap.com");

            string EmailSubject = "TG Complain";
            string EmailText = "Respected Sir <br /> <br />Complaint has been raised with following details: <br /> Mail Id: " + txtMail.Text + " <br /> Vehicle No: " + txtRegno.Text + " <br /> Mobile No: " + txtMobile.Text + " <br /> Complaint No: " + strCompNo + " <br /> Complaint: " + txtRemarks.Text + " <br /> Regards, <br /> <br /> IT Support Team"; ;

            SendMail(lst, "info@linkutsav.com", "", EmailSubject, EmailText);


        }

        public void SendMail(List<string> Ids, string BCC, string strPath, string strSubject, string strText)
        {

            for (int p = 0; p < Ids.Count; p++)
            {
                MailHelper.SendMailMessage("hsrpcomplaint@gmail.com", Ids[p].ToString(), "info@linkutsav.com", "", strSubject, strText, strPath);
            }
        }


    }
}