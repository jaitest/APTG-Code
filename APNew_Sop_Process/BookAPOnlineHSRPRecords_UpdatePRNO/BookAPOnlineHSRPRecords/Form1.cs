using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookAPOnlineHSRPRecords
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            string StrVehicleClassType = string.Empty;
            string RTOLocationID = string.Empty;
            string macbase = string.Empty;
            string USERID = "0";
            string cashrc = string.Empty;
            string sticker1 = string.Empty;
            string VIP = string.Empty;
            string StrVehicleType = string.Empty;
            string AffixationDate = string.Empty;
            string HsrpRecordid = string.Empty;
            int i;
            string strProvider = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
            string sqlquery = "select Distinct pr.prNumber as 'VehicleRegNo',h.hsrprecordid,pr.authorizationRefNo as'Hsrprecord_AuthorizationNo' from HSRPRecords h, prnumber_stagging pr where h.HSRP_StateID=9 and h.hsrprecord_AuthorizationNO=pr.authorizationRefNo and isnull(h.VehicleRegNo,'')='' and isnull(h.aptgvehrecdate,'')='' and h.paymentgateway='NewSOP-SBI' and addrecordby in('Dealer','OnlineDealer')";
            //string sqlquery = "select Distinct pr.prNumber as 'VehicleRegNo',h.hsrprecordid,pr.authorizationRefNo as'Hsrprecord_AuthorizationNo' from HSRPRecords h, prnumber_stagging pr where h.HSRP_StateID=9 and h.hsrprecord_AuthorizationNO=pr.authorizationRefNo and isnull(h.VehicleRegNo,'')='' and h.paymentgateway='NewSOP-SBI' and addrecordby in('Dealer','OnlineDealer')";          
            DataTable dtRecords = Utils.GetDataTable(sqlquery, strProvider);

            if (dtRecords.Rows.Count > 0)
            {
                for (int k = 0; k < dtRecords.Rows.Count; k++)
                {
                    HsrpRecordid = dtRecords.Rows[k]["hsrprecordid"].ToString();
                    string straptgrecdate = System.DateTime.Now.ToString();
                    string Query = "update hsrprecords set vehicleregno='" + dtRecords.Rows[k]["VehicleRegNo"].ToString() + "',aptgvehrecdate='" + straptgrecdate + "' ,addrecordby='OnlineDealer', sendtoerp='1' where  HSRP_StateID='9' and hsrprecordid ='" + dtRecords.Rows[k]["HSRPRecordID"].ToString() + "' and paymentgateway='NewSOP-SBI' ";
                    Utils.ExecNonQuery(Query, strProvider);
                    AddLog("***Records Update Successfully***- '" + dtRecords.Rows[k]["VehicleRegNo"].ToString() + "' " + System.DateTime.Now.ToString());
                }
            }
            label1.Text = "Record Successfully '" + dtRecords.Rows.Count + "' Update ";
        }

        private void AddLog(string logtext)
        {
            string filename = "UpdatePRNO-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".txt";
            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["TGpathx"].ToString();
            fileLocation += filename;
            using (StreamWriter sw = File.AppendText(fileLocation))
            {
                sw.WriteLine(logtext);
                sw.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void timer1_Tick(object sender, EventArgs e)
        {            
            string strDate = DateTime.Now.ToString("h:mm:ss tt");
            #region  In case of Local
            string strTime1 = "08:31:20 PM";

            if (DateTime.Parse(strDate) == DateTime.Parse(strTime1))
            {
            #endregion
                button1_Click(button1, new EventArgs());
            }

        }

        public void timer2_Tick(object sender, EventArgs e)
        {
            string strDate = DateTime.Now.ToString("h:mm:ss tt");
            #region  In case of Local
            string strTime1 = "10:31:20 AM";

            if (DateTime.Parse(strDate) == DateTime.Parse(strTime1))
            {
            #endregion
                button1_Click(button1, new EventArgs());
            }
        }

   
    }
}
 