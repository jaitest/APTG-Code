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
    public partial class frmComplaints : System.Web.UI.Page
    {
        public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        String aapID = String.Empty;
        String SQLString = String.Empty;
        String Status = String.Empty;
       // string SQLString = String.Empty;
        string CnnString = String.Empty;
       
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            string strVehicleNo = txtRegno.Text;
            string strCheck = strVehicleNo.Substring(0, 2);
            if (strCheck != "DL" && strCheck != "dl")
            {
                lblMsg.Text = "Please Enter Valid Vehicle RegNo.";
                return;
            }

            string strSel = "SELECT MAX";


            DateTime dtTime=System.DateTime.Now;
            string strComNo="CMP"+dtTime.Day.ToString()+dtTime.Month.ToString()+dtTime.Year.ToString()+dtTime.Hour.ToString()+dtTime.Minute.ToString()+dtTime.Second.ToString()+dtTime.Millisecond.ToString();
            string strQuery = "INSERT INTO [dbo].[Complaint] ([Region],ComplaintNo,[StateId],[OwnerName]" +
                    ",[Regno]" +
                    ",[EngineNo]" +
                    ",[ChasisNo]" +
                    ",[Remarks])" +
                    "VALUES('" + ddlComplaintRegion.SelectedItem.Text + "','"+strComNo+"','2','" + txtName.Text + "','" + txtRegno.Text +
                    "','" + txtEngineno.Text + "','" + txtChasisNo.Text + "','" + txtRemarks.Text + "')";
            int i = Utils.ExecNonQuery(strQuery, ConnectionString);
           if (!i.Equals(0))
           {
               string strMsg="Your request with request no. "+strComNo+" has been saved for process";
               ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('"+strMsg+"');", true);
               Response.Redirect("frmComplaints.aspx");
           }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("OrderStatus.aspx");
        }


      

        protected void Button2_Click(object sender, EventArgs e)
        {

        }



    }
}