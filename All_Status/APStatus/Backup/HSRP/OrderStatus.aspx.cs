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
    public partial class WebForm1 : System.Web.UI.Page
    {

        string SQLString = String.Empty;
        string CnnString = String.Empty;
      public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
      DataTable ds = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                t1.Visible = true;
                t2.Visible = false;
              //  Fill();
            }
        }
        
        protected void ButtonGo_Click(object sender, EventArgs e)
        {
            
                Session["REG"] = TextBoxVehicalRegNo.Value.Replace(" ", "");
                Response.Redirect("ViewOrderStatus.aspx");
           
        }

        private void Fill()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = "Select  LandlineNo,RTOLocationName,RTOLocationAddress," +
            "ContactPersonName,MobileNo,EmailID,Rtocode From RTOLocation where HSRP_StateID=9 and LocationType='Sub-Urban' and rtocode is not null  and rtolocationname not in ('MAYAPURI','D Surajmal Vihar')";


            Utils dbLink = new Utils();
            dbLink.strProvider = CnnString;
            dbLink.CommandTimeOut = 0;
            dbLink.sqlText = SQLString.ToString();
            SqlDataReader PReader = dbLink.GetReader();


            String SrNo = String.Empty;
            String RTOLocationName = String.Empty;
            String RTOLocationAddress = String.Empty;
            String ContactPersonName = String.Empty;
            String MobileNo = String.Empty;
            String LandlineNo = String.Empty;
            String EmailID = String.Empty;
            String Rtocode = String.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='99%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr style='background-color:#acab3d'>");
            sb.Append("<td valign='top' class='midtablebg'>");
            sb.Append("<table width='100%' border='0' align='center' cellpadding='7' cellspacing='0'>");
            sb.Append("<tr align='center'>");
            sb.Append("<td   height='19' valign='top' nowrap='nowrap' class='midboxtop'>RTO Code</td>");
            sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop' >Contact Person Name</td>");
            sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop' >RTO Location Name</font></td>");
            sb.Append("<td  valign='top' align='center' width='400px' valign='top' class='midboxtop' >RTO Location Address</td>");
            sb.Append("<td  valign='top' nowrap='nowrap'  class='midboxtop' >Mobile No</td>");
            sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop' >Email ID</td>");
            sb.Append("</tr>");


            string bgcol = string.Empty;


            if (PReader.HasRows)
            {
                while (PReader.Read())
                {
                    SrNo = String.Empty;
                    RTOLocationName = String.Empty;
                    RTOLocationAddress = String.Empty;
                    ContactPersonName = String.Empty;
                    MobileNo = String.Empty;
                    EmailID = String.Empty;
                    Rtocode = String.Empty;
                    SrNo = PReader["Rtocode"].ToString();
                    RTOLocationName = PReader["RTOLocationName"].ToString();
                    RTOLocationAddress = PReader["RTOLocationAddress"].ToString();
                    ContactPersonName = PReader["ContactPersonName"].ToString();
                    MobileNo = PReader["MobileNo"].ToString();
                    EmailID = PReader["EmailID"].ToString();
                    Rtocode ="MP"+PReader["Rtocode"].ToString();
                    //if (int.Parse(SrNo) % 2 == 0)
                    //{
                        bgcol = "#ffffff";
                    //}
                    //else
                    //{
                    //    bgcol = "#ffffff";
                    //}

                    sb.Append("<tr style='background-color:#ffffff'>");
                    sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + Rtocode + "</td>");
                    sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + ContactPersonName + "</td>");
                    sb.Append("<td width='150' align='center' class='heading1'>" + RTOLocationName + "</td>");
                    sb.Append("<td align='center' class='heading1'>" + RTOLocationAddress + "</td>");
                    sb.Append("<td width='150' align='center' class='heading1'>" + MobileNo + "</td>");
                    sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + EmailID + "</td>");
                    sb.Append("</tr>");
                }

                sb.Append("<tr><td colspan='8'>&nbsp;</td></tr></table></td></tr></table>");
                vehshow.InnerHtml = sb.ToString();
            }
            else
            {
                //LabelError.Text = string.Empty;
                //LabelError.Text = "Their is no selected data for the selected  date range.";
                //vehshow.InnerHtml = string.Empty;
                //UpdatePanelError.Update();
                //UpdatePanelDiv.Update();
                return;
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(String.Format("http://180.151.100.242/hsrp/login.aspx"));                
            }
            catch(Exception ee)
            {
            }
        }

        protected void lnkComplaint_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("frmComplaints.aspx"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string strSel = "select right(isnull(max(complaintno),0000000),7) from complaint";
                string strCom = Utils.getScalarValue(strSel, ConnectionString);
                string strComNo = string.Empty;

                if (strCom.Equals(0))
                {
                    strComNo = "CP-0000001";
                }
                else
                {
                    strComNo = "CP-" + string.Format("{0:0000000}", Convert.ToInt32(strCom) + 1);
                }


                DateTime dtTime = System.DateTime.Now;
                // string strComNo = "CMP" + dtTime.Day.ToString() + dtTime.Month.ToString() + dtTime.Year.ToString() + dtTime.Hour.ToString() + dtTime.Minute.ToString() + dtTime.Second.ToString() + dtTime.Millisecond.ToString();
                string strQuery = "INSERT INTO [dbo].[Complaint] ([Region],ComplaintNo,[StateId],[OwnerName]" +
                        ",[Regno]" +
                        ",[EngineNo]" +
                        ",[ChasisNo]" +
                        ",[Remarks]" +
                        ",[MobileNo]" +
                        ",[email],[IPAddress])" +
                        "VALUES('" + ddlComplaintRegion.SelectedItem.Text + "','" + strComNo + "','5','" + txtName.Text + "','" + txtRegno.Text +
                        "','" + txtEngineno.Text + "','" + txtChasisNo.Text + "','" + txtRemarks.Text + "','" + txtMobile.Text + "','" + txtMail.Text + "','" + Context.Request.UserHostAddress.ToString() + "')";
                int i = Utils.ExecNonQuery(strQuery, ConnectionString);
                if (!i.Equals(0))
                {                  

                    string strMsg = "Your complaint has been successfully submitted with number " + strComNo + " ,for all future communication use this number.";
                    ddlComplaintRegion.SelectedIndex = 0;
                    txtChasisNo.Text = string.Empty;
                    txtRegno.Text = string.Empty;
                    txtName.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    txtEngineno.Text = string.Empty;
                    txtMobile.Text = string.Empty;
                    txtMail.Text = string.Empty;
                    lblMsg.Text = strMsg;
                    //mpeSave.Show();
                    //  ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('" + strMsg + "');", true);

                }
            }
            catch(Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            //mpeSave.Hide();
        }
                
    }
}