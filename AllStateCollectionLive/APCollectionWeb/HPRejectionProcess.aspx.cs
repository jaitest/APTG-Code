﻿using System;
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
    public partial class HPRejectionProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HP();
        }
        string cnnLocal = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringLocal"].ToString();
        DataTable dt = new DataTable();

        protected void HP()
        {

            try
            {

                APCollectionWeb.RejectionProcess_HP.RejectionRequestWS_Service service = new APCollectionWeb.RejectionProcess_HP.RejectionRequestWS_Service();
                service.UseDefaultCredentials = false;
                //service.PreAuthenticate = true;          
                // SalesRequestWS Cust1 = new SalesRequestWS();

                service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");

                APCollectionWeb.RejectionProcess_HP.RejectionRequestWS Cust;
                int count = 0;
                lblmsg.Text = "There is no record.... HP";
                string query = "select entrytype,entryno,convert(varchar(15),entrydate,106) as entrydate1 ,originalrequestid,rejectiontype,frontlasercode,rearlasercode,replacementrequestid,embosssingcentercode,affixationcentercode,left(reasonforrejection,25)as 'reasonforrejection',left(operatorname,25) as 'operatorname',responsibilitycenter,rtolocationid from rejectplatEntry where hsrp_stateid=3 and entrydate >='2014/02/17 00:00:00'  and sendtoERP is null";
                //string query = "select entrytype,entryno,convert(varchar(15),entrydate,106) as entrydate1 ,originalrequestid,rejectiontype,frontlasercode,rearlasercode,replacementrequestid,embosssingcentercode,affixationcentercode,reasonforrejection,operatorname,responsibilitycenter,rtolocationid from rejectplatEntry where hsrp_stateid=3 and originalrequestid in (7076004,7277749)";
                dt = utils.GetDataTable(query, cnnLocal);
                count = dt.Rows.Count;
                if (dt.Rows.Count > 0)
                {
                    lblmsg.Text = Convert.ToString(count) + " Records..";
                    for (int i = 0; i < count; i++)
                    {
                        // utils.ExecNonQuery("update hsrprecords set sendtoerp =1 where hsrprecordid ='" + dt.Rows[i]["hsrprecordid"].ToString() + "' ", cnnLocal); 

                        Cust = new APCollectionWeb.RejectionProcess_HP.RejectionRequestWS();
                        // service.Create(ref Cust);

                        Cust.Entry_No = Convert.ToInt32(dt.Rows[i]["entryno"].ToString());
                        Cust.Entry_NoSpecified = true;
                        Cust.Entry_Date = Convert.ToDateTime(dt.Rows[i]["entrydate1"].ToString());
                        Cust.Entry_DateSpecified = true;
                        Cust.Original_Request_ID = Convert.ToInt32(dt.Rows[i]["originalrequestid"].ToString());
                        Cust.Original_Request_IDSpecified = true;
                        string rtype = dt.Rows[i]["rejectiontype"].ToString();
                        if (dt.Rows[i]["rejectiontype"].ToString() == "Both")
                        {
                            Cust.Rejection_Type = APCollectionWeb.RejectionProcess_HP.Rejection_Type.Both;
                            Cust.Front_Laser_Code = dt.Rows[i]["frontlasercode"].ToString();
                            Cust.Rear_Laser_Code = dt.Rows[i]["rearlasercode"].ToString();
                        }
                        else if (dt.Rows[i]["rejectiontype"].ToString() == "Front Plate")
                        {
                            Cust.Rejection_Type = APCollectionWeb.RejectionProcess_HP.Rejection_Type.Front;
                            Cust.Front_Laser_Code = dt.Rows[i]["frontlasercode"].ToString();
                        }
                        else if (dt.Rows[i]["rejectiontype"].ToString() == "Rear Plate")
                        {
                            Cust.Rejection_Type = APCollectionWeb.RejectionProcess_HP.Rejection_Type.Rear;
                            Cust.Rear_Laser_Code = dt.Rows[i]["rearlasercode"].ToString();
                        }

                        Cust.Rejection_TypeSpecified = true;

                        if (dt.Rows[i]["EntryType"].ToString() == "Affixation")
                        {
                            Cust.Entry_Type = APCollectionWeb.RejectionProcess_HP.Entry_Type.Affixation;
                            Cust.Replacement_Request_ID = Convert.ToInt32(dt.Rows[i]["originalrequestid"].ToString());

                        }
                        else
                        {
                            Cust.Entry_Type = APCollectionWeb.RejectionProcess_HP.Entry_Type.Embossing;
                            Cust.Replacement_Request_ID = 0;
                        }

                        Cust.Entry_TypeSpecified = true;
                        Cust.Replacement_Request_IDSpecified = true;


                        Cust.Embosssing_Center_Code = dt.Rows[i]["embosssingcentercode"].ToString();
                        Cust.Affixation_Center_Code = dt.Rows[i]["affixationcentercode"].ToString();
                        Cust.Reason_for_Rejection = dt.Rows[i]["reasonforrejection"].ToString();
                        Cust.Operator_Name = dt.Rows[i]["operatorname"].ToString();
                        //Cust.Responsibility_Center = dt.Rows[i]["responsibilitycenter"].ToString();
                        utils.ExecNonQuery("update rejectplatEntry set sendtoERP =2 where entryno ='" + dt.Rows[i]["entryno"].ToString() + "' ", cnnLocal);
                        try
                        {
                            service.Create(ref Cust);
                        }
                        catch
                        {
                            utils.ExecNonQuery("update rejectplatEntry set sendtoERP =1 where entryno ='" + dt.Rows[i]["entryno"].ToString() + "' ", cnnLocal);
                            continue;
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                AddLog(ex.Message.ToString());
            }

        }

        static void AddLog(string logtext)
        {
            string filename = "HPRejectionProcess-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
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