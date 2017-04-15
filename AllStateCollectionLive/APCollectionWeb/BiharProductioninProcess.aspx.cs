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
    public partial class BiharProductioninProcess : System.Web.UI.Page
    {
        string cnnLocal = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringLocal"].ToString();
        DataTable dt = new DataTable();

        string query;
        int count, i;
        protected void Page_Load(object sender, EventArgs e)
        {
            BiharPip();
        }

        protected void BiharPip()
        {
            try
            {
                APCollectionWeb.WebReference_BR.SalesRequestWS_Service service = new APCollectionWeb.WebReference_BR.SalesRequestWS_Service();
                //WebReference_bihar.SalesRequestWS_Service service = new WebReference_bihar.SalesRequestWS_Service();
                service.UseDefaultCredentials = false;
                //service.PreAuthenticate = true;          
                service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
                APCollectionWeb.WebReference_BR.SalesRequestWS Cust = new APCollectionWeb.WebReference_BR.SalesRequestWS();
               
                lblmsg.Text = "There is no record.... BR";
                query = "select HSRPRecordID from hsrprecords where hsrp_stateid=1  and PdfRunningNo is not null and NAVPDFFlag is null and sendtoerp =1 and hsrprecord_creationdate>'2015/01/01 00:00:00'";
                //query = "select  HSRPRecordID from hsrprecords where hsrp_stateid=1  and sendtoerp =1 and hsrprecordid in (7122701)";// and hsrprecord_creationdate>='2014/12/25 00:00:00'";
                dt = utils.GetDataTable(query, cnnLocal);
                count = dt.Rows.Count;
                if (dt.Rows.Count > 0)
                {
                    lblmsg.Text = Convert.ToString(count);
                    for (i = 0; i < count; i++)
                    {
                        lblmsg.Text = "Bihar PIP Working...";
                        Cust = service.Read(Convert.ToInt32(dt.Rows[i]["HSRPRecordID"].ToString()));
                        Cust.WebApp_Production_Process = true;
                        Cust.WebApp_Production_ProcessSpecified = true;
                        try
                        {
                            service.Update(ref Cust);
                        }
                        catch
                        {
                            utils.ExecNonQuery("update hsrprecords set NAVPDFFlag = 1 where hsrp_stateid=1 and HSRPRecordID = '" + dt.Rows[i]["HSRPRecordID"].ToString() + "'", cnnLocal);
                            continue;
                        }
                        utils.ExecNonQuery("update hsrprecords set NAVPDFFlag = 1 where hsrp_stateid=1 and HSRPRecordID = '" + dt.Rows[i]["HSRPRecordID"].ToString() + "'", cnnLocal);
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
            string filename = "BRPIP-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["TGpathx"].ToString();
            fileLocation += filename;
            using (StreamWriter sw = File.AppendText(fileLocation))
            {
                sw.WriteLine("-------------------" + System.DateTime.Now.ToString() + "--------------------");
                sw.WriteLine(logText);
                sw.WriteLine("-----------------------------------------------------------------------------");
                sw.Close();
            }

           
        }

    }
}