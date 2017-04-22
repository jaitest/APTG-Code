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
    public partial class AppoientmentForInstallation : System.Web.UI.Page
    {
        public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        String aapID = String.Empty;
        String SQLString = String.Empty;
        String Status = String.Empty;
       // string SQLString = String.Empty;
        string CnnString = String.Empty;
        string strURL = string.Empty;
        string strMsg = string.Empty;
        string RtoLocation = string.Empty;
        string RtoLocationID = string.Empty;
        string RId = string.Empty;
        

        DataTable dtRto = new DataTable();

        MailHelper objMailSender = new MailHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetStateFromURL();
                FillRtoLocation();
            }
        }

        private void GetStateFromURL()
        {

          //  strURL = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
          ////  strURL = "http://hsrpmp.com/complaint.htm";
          //  lblComp.Text = strURL;
          //   lblComp.Text = strURL;
          //  if (strURL != null)
          //  {
          //      string[] ArrUrl = new string[3];

          //      ArrUrl = strURL.Split('.');
          //      for (int i = 0; i < ArrUrl.Length; i++)
          //      {
          //          if (ArrUrl[i].ToString().ToUpper().Contains("HSRP"))
          //          {
          //              //   strState = ArrUrl[1].ToString().ToUpper().Replace("HSRP", string.Empty);
          //            //  original.Substring(original.Length - 2);
          //              string str = ArrUrl[i].ToString().ToUpper();
          //              ViewState["State"] = str.Substring(str.Length - 2);
          //              break;
          //          }
          //      }
          //  }
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


        protected void FillRtoLocation()
        {
            RtoLocation = "select rtolocationid,upper(RTOLocationName) as RTOLocationName from rtolocation where hsrp_stateid=9 and ActiveStatus='y' order by rtolocationname";
           dtRto = Utils.GetDataTable(RtoLocation, ConnectionString);
        
            ddl_Rto.DataSource = dtRto;
            ddl_Rto.DataBind();
            
            
            ddl_Rto.Items.Insert(0,new ListItem("--Select Rto Location--","0"));
      
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string strSel = "select right(isnull(max(complaintno),0000000),7) from complaint";
                string strCom = Utils.getScalarValue(strSel, ConnectionString);
                string strComNo = string.Empty;

               
                DateTime dtTime = System.DateTime.Now;
                string strState = string.Empty;
                string strStateId = string.Empty;
                if (ViewState["State"] != null)
                {
                    strState = ViewState["State"].ToString();
                    strStateId = strState.ToUpper().Equals("BR") ? "1" : strState.ToUpper().Equals("HP") ? "3" : strState.ToUpper().Equals("HR") ? "4" : strState.ToUpper().Equals("MP") ? "5" : strState.ToUpper().Equals("UK") ? "6" : strState.ToUpper().Equals("AP") ? "9" : strState.ToUpper().Equals("TG") ? "11" : "2";
                }
                if (strCom.Equals(0))
                {
                    strComNo = strState+"-0000001";
                }
                else
                {
                    strComNo = strState+"-" + string.Format("{0:0000000}", Convert.ToInt32(strCom) + 1);
                }

                if (ddl_Rto.SelectedItem.Text.Trim() == "--Select Rto Location--")
                {

                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select Rto Location');", true);

                }
                else
                {
                   
                    string stateid = "9";
                    string strQuery = "insert into AppointmentForInstallation_AP(HSRP_StateId, RTOLocationId, VehicleRegNo, Address, VehicleType,ChassisNo,EngineNo,CashReceiptNo,Email,MobileNo)" +
                   " values('" + stateid + "','" + ddl_Rto.SelectedValue.ToString() + "','" + txtvech_no.Text + "','" + txt_address.Text + "','" + ddl_vech_type.SelectedItem.Text + "','" + txtchasis_no.Text + "','" + txtengine_no.Text + "','" + txtcashrecpt_no.Text + "','" + txtemail.Text + "','" + txtmob_no.Text + "') ";
                    int i = Utils.ExecNonQuery(strQuery, ConnectionString);


                    if (txtcashrecpt_no.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Cash Receipt Mandatory');", true);

                        lblMsg.Visible = false;
                    }
                    else
                    {
                        ddl_vech_type.SelectedIndex = 0;
                       // ddl_Rto.Text = string.Empty;
                        txtvech_no.Text = string.Empty;
                        ddl_vech_type.Text = string.Empty;
                        txtchasis_no.Text = string.Empty;
                        txtengine_no.Text = string.Empty;
                        txtcashrecpt_no.Text = string.Empty;

                        txtemail.Text = string.Empty;
                        txtmob_no.Text = string.Empty;
                        txt_address.Text = string.Empty;
                        lblMsg.Text = strMsg;
                    }
                    if (!i.Equals(0))
                    {

                        strMsg = "Record Saved, Will Get back to you Shortly.";
                        try
                        {
                            // GenerateIdsAndSendMail(strComNo,strState);
                        }
                        catch (Exception ex)
                        {
                            //  Response.Write(ex.Message);
                        }
                        ddl_vech_type.SelectedIndex = 0;
                        //ddl_Rto.Text = string.Empty;
                        txtvech_no.Text = string.Empty;
                        ddl_vech_type.Text = string.Empty;
                        txtchasis_no.Text = string.Empty;
                        txtengine_no.Text = string.Empty;
                        txtcashrecpt_no.Text = string.Empty;

                        txtemail.Text = string.Empty;
                        txtmob_no.Text = string.Empty;
                        lblMsg.Text = strMsg;

                        //  ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('" + strMsg + "');", true);

                    }
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

        protected void ddl_Rto_SelectedIndexChanged(object sender, EventArgs e)
        {

            
            RId = ddl_Rto.SelectedValue.ToString();
            
        }

        //private void GenerateIdsAndSendMail(string strCompNo,string strState)
        //{
        //    strState = string.IsNullOrEmpty(strState) ? " " : strState;
        //    List<string> lst = new List<string>();
        //    lst.Add("sanjiv.khare@linkutsav.com");
        //    lst.Add("ravi.munjal@hsrpap.com");
        //    lst.Add("support@hsrpuk.com");
        //    lst.Add("amitbhargavain@gmail.com");
        //    string EmailSubject = strState+ " Complain";
        //    string EmailText = "Respected Sir <br /> <br />Complaint has been raised with following details: <br /> State: " + strState + " <br /> Mail Id: " + txtMail.Text + " <br /> Mobile No: " + txtMobile.Text 
        //        + " <br /> Complaint No: " + strCompNo + " <br /> Complaint: " + txtRemarks.Text 
        //        + " <br /> Regards, <br /> <br /> IT Support Team";

        //    SendMail(lst, "", EmailSubject, EmailText);


        //}

        //public void SendMail(List<string> Ids, string strPath, string strSubject, string strText)
        //{

        //    for (int p = 0; p < Ids.Count; p++)
        //    {
        //        MailHelper.SendMailMessage("hsrpcomplaint@gmail.com", Ids[p].ToString(), "", "", strSubject, strText, strPath);
        //    }
        //}


    }
}