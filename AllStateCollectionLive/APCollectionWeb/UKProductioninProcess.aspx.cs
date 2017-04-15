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
    public partial class UKProductioninProcess : System.Web.UI.Page
    {
        string cnnLocal = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringLocal"].ToString();
        DataTable dt = new DataTable();

        string query;
        int count, i;
        protected void Page_Load(object sender, EventArgs e)
        {
            UK();
        }

        protected void UK()
        {
            try
            {
                APCollectionWeb.WebReference_UK.SalesRequestWS_Service service = new APCollectionWeb.WebReference_UK.SalesRequestWS_Service();
                //WebReference_UK.SalesRequestWS_Service service = new WebReference_UK.SalesRequestWS_Service();
                service.UseDefaultCredentials = false;
                //service.PreAuthenticate = true;          
                service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
                APCollectionWeb.WebReference_UK.SalesRequestWS Cust = new APCollectionWeb.WebReference_UK.SalesRequestWS();
                //WebReference_UK.SalesRequestWS Cust = new WebReference_UK.SalesRequestWS();
                query = "select top 2500 HSRPRecordID from hsrprecords where hsrp_stateid=6  and PdfRunningNo is not null and NAVPDFFlag is null and sendtoerp =1 and hsrprecord_creationdate>'2015/01/01 00:00:00'";
                //query = "select top 2500 HSRPRecordID from hsrprecords where hsrp_stateid=6 and NAVPDFFlag is null and PdfRunningNo is not null and sendtoerp =1 and hsrprecord_creationdate>='2014/12/25 00:00:00'";
                //query = "select HSRPRecordID from hsrprecords where  hsrp_stateid=6 and hsrprecordid in 
                dt = utils.GetDataTable(query, cnnLocal);
                count = dt.Rows.Count;
                if (dt.Rows.Count > 0)
                {
                    lblmsg.Text = Convert.ToString(count);
                    for (i = 0; i < count; i++)
                    {
                        Cust = service.Read(Convert.ToInt32(dt.Rows[i]["HSRPRecordID"].ToString()));
                        Cust.WebApp_Production_Process = true;
                        Cust.WebApp_Production_ProcessSpecified = true;
                        try
                        {
                            service.Update(ref Cust);

                        }
                        catch
                        {
                            utils.ExecNonQuery("update hsrprecords set NAVPDFFlag = 1 where hsrp_stateid=6 and HSRPRecordID = '" + dt.Rows[i]["HSRPRecordID"].ToString() + "'", cnnLocal);
                            continue;
                        }
                        utils.ExecNonQuery("update hsrprecords set NAVPDFFlag = 1 where hsrp_stateid=6 and HSRPRecordID = '" + dt.Rows[i]["HSRPRecordID"].ToString() + "'", cnnLocal);

                    }

                }
                else
                {
                    lblmsg.Text = "There is no records..";
                }
            }

            catch (Exception ex)
            {
                AddLog(ex.Message.ToString());
            }


        }


        static void AddLog(string logText)
        {
            string filename = "UKPIP-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["TGpathx"].ToString();
            fileLocation += filename;
            using (StreamWriter sw = File.AppendText(fileLocation))
            {
                sw.WriteLine("-------------------" + System.DateTime.Now.ToString() + "--------------------");
                sw.WriteLine(logText);
                sw.WriteLine("-----------------------------------------------------------------------------");
                sw.Close();
            }

            //string pathx = "E:\\ERPHSRPLog\\UKPIP-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
            //using (StreamWriter sw = File.AppendText(pathx))
            //{
            //    sw.WriteLine("-------------------" + System.DateTime.Now.ToString() + "--------------------");
            //    sw.WriteLine(logText);
            //    sw.WriteLine("-----------------------------------------------------------------------------");
            //    sw.Close();
            //}
        }
    }
}