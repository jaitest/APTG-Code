using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using APCollectionWeb;
using System.Data;

namespace APCollectionWeb
{
    public partial class APProductionsheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetLaserCodesAndhraPradesh();
        }
        string cnnLocal = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringAPTG"].ToString();
        DataTable dt = new DataTable();

        protected void GetLaserCodesAndhraPradesh()
        {
            APCollectionWeb.WebReference.SalesRequestWS_Service service = new APCollectionWeb.WebReference.SalesRequestWS_Service();
            service.UseDefaultCredentials = false;
            //service.PreAuthenticate = true;          
            service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
            APCollectionWeb.WebReference.SalesRequestWS Cust = new APCollectionWeb.WebReference.SalesRequestWS();
            //[WebApp Laser Code Assigned] =0 and [NAV Laser Code Assigned] =1
            List<APCollectionWeb.WebReference.SalesRequestWS_Filter> filterArray = new List<APCollectionWeb.WebReference.SalesRequestWS_Filter>();
            APCollectionWeb.WebReference.SalesRequestWS_Filter nameFilter = new APCollectionWeb.WebReference.SalesRequestWS_Filter();
            nameFilter.Field = APCollectionWeb.WebReference.SalesRequestWS_Fields.NAV_Laser_Code_Assigned;
            nameFilter.Criteria = "true";
            filterArray.Add(nameFilter);
            APCollectionWeb.WebReference.SalesRequestWS_Filter nameFilter1 = new APCollectionWeb.WebReference.SalesRequestWS_Filter();
            nameFilter1.Field = APCollectionWeb.WebReference.SalesRequestWS_Fields.WebApp_Laser_Code_Assigned;
            nameFilter1.Criteria = "false";
            filterArray.Add(nameFilter1);
            lblmsg.Text = "There is no record...";
            APCollectionWeb.WebReference.SalesRequestWS[] list = service.ReadMultiple(filterArray.ToArray(), null, 100);
            foreach (APCollectionWeb.WebReference.SalesRequestWS c in list)
            {
                lblmsg.Text = "Working AP...";
                Cust = service.Read(c.HSRP_Record_ID);
                Cust.WebApp_Laser_Code_Assigned = true;
                Cust.WebApp_Laser_Code_AssignedSpecified = true;
                utils.ExecNonQuery("update hsrprecords set HSRP_Front_LaserCode= '" + c.Front_Laser_Code + "',HSRP_Rear_LaserCode = '" + c.Rear_Laser_Code + "', erpassigndate=getdate() where hsrp_stateid=9 and HSRPRecordID = '" + c.HSRP_Record_ID + "'", cnnLocal);
                service.Update(ref Cust);
            }
        }

    }
}