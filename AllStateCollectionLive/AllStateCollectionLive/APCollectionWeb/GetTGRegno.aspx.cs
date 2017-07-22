using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace APCollectionWeb
{
    public partial class GetTGRegno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetTGReg();
        }


        protected void GetTGReg()
        {
            try
            {
                AddLog("***Get TGRegNo Program Started***-" + System.DateTime.Now.ToString());
                int count = 0;
                string cnnLocal = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringTG"].ToString();
                DataTable dt = new DataTable();
                TgS.HSRPAuthorizationServiceSoapClient Tgs = new TgS.HSRPAuthorizationServiceSoapClient();
                AddLog("WebService Object Creatred-" + System.DateTime.Now.ToString());
                string AuthNo = string.Empty;
                string dateTo = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy");
                // string AuthData = Tgs.GetAuthorizationnotRegistered("01/5/2015", dateTo);
                //if (AuthData.Length > 1)
                //{
                //    using (StringReader stringReader = new StringReader(AuthData))
                //    using (XmlTextReader reader = new XmlTextReader(stringReader))
                //    {
                //        while (reader.Read())
                //        {

                //            if (reader.Name.ToString() == "AuthNo")
                //            {
                //                reader.Read();
                //                if (AuthNo.Length == 0)
                //                {
                //                    AuthNo = "'" + reader.Value.ToString() + "'";
                //                }
                //                else
                //                {
                //                    AuthNo = AuthNo + ",'" + reader.Value.ToString() + "'";
                //                }
                //                reader.Read();
                //            }
                //        }
                //    }
                //}
                string query = string.Empty;
                if (AuthNo != string.Empty)
                {                    
                    query = "select convert(varchar(20),hsrprecord_authorizationdate,103) as AuthDate from hsrprecords where hsrp_stateid=11 and  HSRPRecord_AuthorizationNo not in (" + AuthNo + ") and hsrprecord_creationdate > '01/01/2015 00:00:00' and hsrprecord_creationdate < dateadd(day,-2,getdate()) and isnull(vehicleregno,'')='' group by convert(varchar(20),hsrprecord_authorizationdate,103) order by  convert(varchar(20),hsrprecord_authorizationdate,103)";
                }
                else
                {                    
                    query = "select convert(varchar(20),hsrprecord_authorizationdate,103) as AuthDate from hsrprecords where hsrp_stateid=11  and  hsrprecord_creationdate > '01/01/2015 00:00:00' and hsrprecord_creationdate < dateadd(day,-2,getdate()) and isnull(vehicleregno,'')='' group by convert(varchar(20),hsrprecord_authorizationdate,103) order by  convert(varchar(20),hsrprecord_authorizationdate,103) ";
                }
                dt = utils.GetDataTable(query, cnnLocal);
                count = dt.Rows.Count;
                AddLog("Authorization Dates Fatched Total Count - " + count.ToString());
                lblmsg.Text = Convert.ToString(count);
                if (dt.Rows.Count > 0)
                {

                    count = dt.Rows.Count;                    
                    for (int i = 0; i < count; i++)
                    {
                        lblmsg.Text = "TG Working...";
                        string dateX = dt.Rows[i]["AuthDate"].ToString();                        
                        AddLog("Govt Service Called For Authorization Date - " + dateX.ToString());
                        Tgs.UpdateRegnOnAuthorizationDate(dateX);
                    }
                }
                AddLog("***Get TGRegNo Program End***-" + System.DateTime.Now.ToString());

                lblmsg.Text = "There is no record.......";
            }

            catch (Exception ex)
            {
                AddLog(ex.Message.ToString());
            }
        }

        private void AddLog(string logtext)
        {
            string filename ="TGGETREGNO-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".txt";
            //string pathx = "E:\\GetDataLog\\TGGETREGNO-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
            //using (StreamWriter sw = File.AppendText(pathx))

            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["TGpathx"].ToString();
            fileLocation +=filename;
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