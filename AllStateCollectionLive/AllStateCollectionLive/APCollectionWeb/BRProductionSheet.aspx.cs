using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using APCollectionWeb;
using System.Data;
using System.IO;

namespace APCollectionWeb
{
    public partial class BRProductionSheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Bihar();
        }

        string cnnLocal = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringLocal"].ToString();
        DataTable dt = new DataTable();

        protected void Bihar()
        {
            try
            {
                APCollectionWeb.WebReferenceNewBR.SalesRequestWS_Service service = new APCollectionWeb.WebReferenceNewBR.SalesRequestWS_Service();
                service.UseDefaultCredentials = false;
                //service.PreAuthenticate = true;          
                //service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
                service.Credentials = new System.Net.NetworkCredential("Administrator@erp.com", "ewq@)!^98");

                APCollectionWeb.WebReferenceNewBR.SalesRequestWS Cust = new APCollectionWeb.WebReferenceNewBR.SalesRequestWS();
                //[WebApp Laser Code Assigned] =0 and [NAV Laser Code Assigned] =1
                List<APCollectionWeb.WebReferenceNewBR.SalesRequestWS_Filter> filterArray = new List<APCollectionWeb.WebReferenceNewBR.SalesRequestWS_Filter>();
                APCollectionWeb.WebReferenceNewBR.SalesRequestWS_Filter nameFilter = new APCollectionWeb.WebReferenceNewBR.SalesRequestWS_Filter();
                nameFilter.Field = APCollectionWeb.WebReferenceNewBR.SalesRequestWS_Fields.NAV_Laser_Code_Assigned;
                nameFilter.Criteria = "true";
                filterArray.Add(nameFilter);

                APCollectionWeb.WebReferenceNewBR.SalesRequestWS_Filter nameFilter1 = new APCollectionWeb.WebReferenceNewBR.SalesRequestWS_Filter();
                nameFilter1.Field = APCollectionWeb.WebReferenceNewBR.SalesRequestWS_Fields.WebApp_Laser_Code_Assigned;
                nameFilter1.Criteria = "false";

                filterArray.Add(nameFilter1);
                lblmsg.Text = "There is no record...";
                APCollectionWeb.WebReferenceNewBR.SalesRequestWS[] list = service.ReadMultiple(filterArray.ToArray(), null, 100);
                foreach (APCollectionWeb.WebReferenceNewBR.SalesRequestWS c in list)
                {
                    lblmsg.Text = "";
                    Cust = service.Read(c.HSRP_Record_ID);
                    Cust.WebApp_Laser_Code_Assigned = true;
                    Cust.WebApp_Laser_Code_AssignedSpecified = true;
                    utils.ExecNonQuery("update hsrprecords set HSRP_Front_LaserCode= '" + c.Front_Laser_Code + "',HSRP_Rear_LaserCode = '" + c.Rear_Laser_Code + "',erpassigndate=getdate() where hsrp_stateid= 1 and HSRPRecordID = '" + c.HSRP_Record_ID + "'", cnnLocal);
                    service.Update(ref Cust);
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message.ToString());
            }
        }

        static void AddLog(string logtext)
        {
            string filename = "BRProductionsheet-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["TGpathx"].ToString();
            fileLocation += filename;
            using (StreamWriter sw = File.AppendText(fileLocation))
            {
                sw.WriteLine("-------------------" + System.DateTime.Now.ToString() + "--------------------");
                sw.WriteLine(logtext);
                sw.WriteLine("-----------------------------------------------------------------------------");
                sw.Close();
            }         
        

        }
    }
}