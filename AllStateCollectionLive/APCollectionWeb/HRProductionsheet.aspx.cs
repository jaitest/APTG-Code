using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APCollectionWeb
{
    public partial class HRProductionsheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Haryana();
        }


        string cnnLocal = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringLocal"].ToString();
        DataTable dt = new DataTable();

        protected void Haryana()
        {
            try
            {
                APCollectionWeb.WebReference_HR.SalesRequestWS_Service service = new APCollectionWeb.WebReference_HR.SalesRequestWS_Service();
                service.UseDefaultCredentials = false;
                //service.PreAuthenticate = true;          
                service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
                APCollectionWeb.WebReference_HR.SalesRequestWS Cust = new APCollectionWeb.WebReference_HR.SalesRequestWS();
                //[WebApp Laser Code Assigned] =0 and [NAV Laser Code Assigned] =1
                List<APCollectionWeb.WebReference_HR.SalesRequestWS_Filter> filterArray = new List<APCollectionWeb.WebReference_HR.SalesRequestWS_Filter>();
                APCollectionWeb.WebReference_HR.SalesRequestWS_Filter nameFilter = new APCollectionWeb.WebReference_HR.SalesRequestWS_Filter();
                nameFilter.Field = APCollectionWeb.WebReference_HR.SalesRequestWS_Fields.NAV_Laser_Code_Assigned;
                nameFilter.Criteria = "true";
                filterArray.Add(nameFilter);
                APCollectionWeb.WebReference_HR.SalesRequestWS_Filter nameFilter1 = new APCollectionWeb.WebReference_HR.SalesRequestWS_Filter();
                nameFilter1.Field = APCollectionWeb.WebReference_HR.SalesRequestWS_Fields.WebApp_Laser_Code_Assigned;
                nameFilter1.Criteria = "false";
                filterArray.Add(nameFilter1);
                lblmsg.Text = "There is no record...";
                APCollectionWeb.WebReference_HR.SalesRequestWS[] list = service.ReadMultiple(filterArray.ToArray(), null, 100);
                foreach (APCollectionWeb.WebReference_HR.SalesRequestWS c in list)
                {
                    lblmsg.Text = "Working HR...";
                    Cust = service.Read(c.HSRP_Record_ID);
                    Cust.WebApp_Laser_Code_Assigned = true;
                    Cust.WebApp_Laser_Code_AssignedSpecified = true;                    
                    string aa = "update hsrprecords set HSRP_Front_LaserCode= '" + c.Front_Laser_Code + "',HSRP_Rear_LaserCode = '" + c.Rear_Laser_Code + "',erpassigndate=getdate() where hsrp_stateid=4 and HSRPRecordID = '" + c.HSRP_Record_ID + "'";
                    utils.ExecNonQuery(aa, cnnLocal);
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
            string filename = "HRProductionsheet-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
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