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
                APCollectionWeb.WebReference_BR.SalesRequestWS_Service service = new APCollectionWeb.WebReference_BR.SalesRequestWS_Service();
                service.UseDefaultCredentials = false;
                //service.PreAuthenticate = true;          
                service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
                APCollectionWeb.WebReference_BR.SalesRequestWS Cust = new APCollectionWeb.WebReference_BR.SalesRequestWS();
                //[WebApp Laser Code Assigned] =0 and [NAV Laser Code Assigned] =1
                List<APCollectionWeb.WebReference_BR.SalesRequestWS_Filter> filterArray = new List<APCollectionWeb.WebReference_BR.SalesRequestWS_Filter>();
                APCollectionWeb.WebReference_BR.SalesRequestWS_Filter nameFilter = new APCollectionWeb.WebReference_BR.SalesRequestWS_Filter();
                nameFilter.Field = APCollectionWeb.WebReference_BR.SalesRequestWS_Fields.NAV_Laser_Code_Assigned;
                nameFilter.Criteria = "true";
                filterArray.Add(nameFilter);

                APCollectionWeb.WebReference_BR.SalesRequestWS_Filter nameFilter1 = new APCollectionWeb.WebReference_BR.SalesRequestWS_Filter();
                nameFilter1.Field = APCollectionWeb.WebReference_BR.SalesRequestWS_Fields.WebApp_Laser_Code_Assigned;
                nameFilter1.Criteria = "false";

                filterArray.Add(nameFilter1);
                lblmsg.Text = "There is no record...";
                APCollectionWeb.WebReference_BR.SalesRequestWS[] list = service.ReadMultiple(filterArray.ToArray(), null, 100);
                foreach (APCollectionWeb.WebReference_BR.SalesRequestWS c in list)
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