using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace APCollectionWeb
{
    public partial class GetTGUpdatedCat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string Authorization_Ref_no = string.Empty;
                string Owner_Name = string.Empty;
                string Vehicle_Type = string.Empty;
                string Veh_Class_Type = string.Empty;
                string Rto_Code = string.Empty;
                string Rto_Name = string.Empty;
                string Engine_No = string.Empty;
                string Chassis_No = string.Empty;
                string Reg_no = string.Empty;
                string TempRegno = string.Empty;
                string No_Assign_Date = string.Empty;

                AddLog("***Get TGUpdatedCat Program Started***-" + System.DateTime.Now.ToString());
                int count = 0;
                string cnnLocal = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringTG"].ToString();
                DataTable dt = new DataTable();
                //TgS.HSRPAuthorizationServiceSoapClient Tgs = new TgS.HSRPAuthorizationServiceSoapClient();
                APCollectionWeb.GetTgUpdatedCat.HSRPAuthorizationService TgUpdatedCat = new GetTgUpdatedCat.HSRPAuthorizationService();
                AddLog("WebService Object Created-" + System.DateTime.Now.ToString());
                string AuthNo = string.Empty;
                //String dateTo = DateTime.Now.ToString("dd/MM/yyyy");
                string dateTo = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                AddLog("Dates for Fatched Total Data - from Date" +' '+ count.ToString() + " To Date " +' ' + dateTo.ToString());
                string AuthData = TgUpdatedCat.GetCategoryChangedVehicles(dateTo, dateTo);
                if (AuthData != "")
                {
                    AddLog("***Records fatch from govt successfully***-" + System.DateTime.Now.ToString());
                    dt = utils.GetDataTable("InsertIntoTGGovtChangedRecords '" + AuthData + "'", cnnLocal);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["status"].ToString().Trim() != "0")
                        {
                            AddLog("***Records insert into table successfull***-" + System.DateTime.Now.ToString());
                            lblmsg.Text = "";
                            lblmsg.Text = dt.Rows[0]["msg"].ToString().Trim();
                        }
                        else
                        {
                            AddLog("***Records not insert into table***-" + System.DateTime.Now.ToString());
                            lblmsg.Text = "";
                            lblmsg.Text = dt.Rows[0]["msg"].ToString().Trim();
                        }
                    }
                    
                    AddLog("***Get TGUpdatedCat Program End***-" + System.DateTime.Now.ToString());
                }
                else 
                {
                    AddLog("***Record not get from govt today***-" + System.DateTime.Now.ToString());
                    AddLog("***Get TGUpdatedCat Program End***-" + System.DateTime.Now.ToString());
                    lblmsg.Text = "";
                    lblmsg.Text = "Record not get from govt today";
                }
            }

            catch (Exception ex)
            {
                AddLog(ex.Message.ToString());
            }

        }

        private void AddLog(string logtext)
        {
            string filename = "TGUpdatedCat-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".txt";
          
            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["TGpathx"].ToString();
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