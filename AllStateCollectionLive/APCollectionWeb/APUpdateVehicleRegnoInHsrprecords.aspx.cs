using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using APCollectionWeb;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace APCollectionWeb
{
    public partial class APUpdateVehicleRegnoInHsrprecords : System.Web.UI.Page
    {
        string cnnLocal = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringAPTG"].ToString();
       // SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringAPTG"].ToString());
        string HSRPRecord_AuthorizationNo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            apvehicleregno();
        }          

        public void apvehicleregno()
        {
            try
            {
                AddLog("***Get APUpdateVehicleRegnoInHsrprecords Program Started***-" + System.DateTime.Now.ToString());
                string sqlquery = "update hsrprecords set hsrprecords.VehicleRegNo=APvehicleregno.VehicleRegNo,hsrprecords.aptgvehrecdate=getdate() from APvehicleregno where hsrprecords.HSRPRecord_AuthorizationNo=APvehicleregno.HSRPRecord_AuthorizationNo and isnull(hsrprecords.VehicleRegNo,'')='' and hsrprecords.hsrp_stateid=9";
                int  dtRecords = utils.ExecNonQuery(sqlquery, cnnLocal);
                if (dtRecords > 0)
                {
                    AddLog("***Records Update Successfully ***-" + System.DateTime.Now.ToString());
                    lblmsg.Text = "";
                    lblmsg.Text = "Records Update Successfully";

                }
                else
                {
                    AddLog("***Records Not Update Successfully***-" + System.DateTime.Now.ToString());
                    lblmsg.Text = "";
                    lblmsg.Text = "Records Not Update Successfully";
                }
                AddLog("***Get APUpdateVehicleRegnoInHsrprecords Program End***-" + System.DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                AddLog(ex.Message.ToString());
                lblmsg.Text = ex.Message.ToString() + " " + HSRPRecord_AuthorizationNo;
            }
        }


        private void AddLog(string logtext)
        {
            string filename = "APUpdateVehicleRegnoInHsrprecords-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["APPathX"].ToString();
            fileLocation += filename;
            using (StreamWriter sw = File.AppendText(fileLocation))
            {
                // sw.WriteLine("-------------------" + System.DateTime.Now.ToString() + "--------------------");
                sw.WriteLine(logtext);
                // sw.WriteLine("-----------------------------------------------------------------------------");
                sw.Close();
            }
        }
    }
}