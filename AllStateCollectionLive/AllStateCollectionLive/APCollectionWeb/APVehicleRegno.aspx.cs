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
    public partial class APVehicleRegno : System.Web.UI.Page
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
                AddLog("***Get APVehicleRegNo Program Started***-" + System.DateTime.Now.ToString());
                APWebreference.HSRPAuthorizationService obj = new APWebreference.HSRPAuthorizationService();

                AddLog("WebService Object Creatred-" + System.DateTime.Now.ToString());

                string sqlquery = "Select top 3000 replace(HSRPRecord_AuthorizationNo,'.','') as HSRPRecord_AuthorizationNo,hsrprecordid from hsrprecords where hsrp_stateid=9 and isnull(vehicleregno,'')='' and isnull(HSRPRecord_AuthorizationNo,'')!='' and HSRPRecord_AuthorizationNo not in(Select HSRPRecord_AuthorizationNo from APvehicleregno where vehicleregno like '%AP%') order by hsrprecordid desc";
                //string sqlquery = "Select top 1000 replace(HSRPRecord_AuthorizationNo,'.','') as HSRPRecord_AuthorizationNo from TGvehicleregno where vehihicleregno is null and isnull(HSRPRecord_AuthorizationNo,'')!=''";
                DataTable dtRecords = utils.GetDataTable(sqlquery, cnnLocal);
                for (int i = 0; i < dtRecords.Rows.Count; i++)
                {
                    HSRPRecord_AuthorizationNo = dtRecords.Rows[i]["HSRPRecord_AuthorizationNo"].ToString();
                    lblmsg.Text = "start";
                    //button1.Visible = false;
                    string AuthData = obj.GetHSRPAuthorizationno(HSRPRecord_AuthorizationNo);
                    if (AuthData.Length > 1)
                    {
                        using (SqlConnection con = new SqlConnection(cnnLocal))
                        {
                            
                            using (SqlCommand cmd = new SqlCommand("InsertIntoAPvehicleregno_Test"))
                            {
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Connection = con;
                                    sda.SelectCommand = cmd;                                 
                                    cmd.Parameters.AddWithValue("@TableAPvehicleregno", AuthData);
                                    using (DataTable dt = new DataTable())
                                    {
                                        sda.Fill(dt);
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0]["status"].ToString().Trim() != "0")
                                            {
                                                lblmsg.Text = "";
                                                lblmsg.Text = dt.Rows[0]["msg"].ToString().Trim();
                                            }
                                            else
                                            {
                                                lblmsg.Text = "";
                                                lblmsg.Text = dt.Rows[0]["msg"].ToString().Trim();
                                            }
                                           
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Utils.ExecNonQuery("update TGvehicleregno set NotGet='0' where HSRPRecord_AuthorizationNo='" + HSRPRecord_AuthorizationNo + "'", cnnLocal);
                    }
                }
                AddLog("***Get APRegNo Program End***-" + System.DateTime.Now.ToString());
                lblmsg.Text = "done";
                //button1.Visible = true;
            }
            catch (Exception ex)
            {
                AddLog(ex.Message.ToString());
                lblmsg.Text = ex.Message.ToString() + " " + HSRPRecord_AuthorizationNo;
               
            }
        }


        private void AddLog(string logtext)
        {
            string filename = "APVehicleREGNO-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
           // string pathx = "E:\\GetDataLog\\APGETREGNO-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";

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