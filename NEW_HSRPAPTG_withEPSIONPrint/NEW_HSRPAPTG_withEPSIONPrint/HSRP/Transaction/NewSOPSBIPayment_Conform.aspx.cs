using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace HSRP.Transaction
{


    public partial class NewSOPSBIPayment_Conform : System.Web.UI.Page
    {

        Utils bl = new Utils();
        string HSRPStateID = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string userdealerid = string.Empty;
        int UserType;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
       
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                //userdealerid = Session["userdealerid"].ToString();
                ComputerIP = Request.UserHostAddress;
           

                if (!IsPostBack)
                {
                    try
                    {
                        orderno();
                        InitialSetting();
                      
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }


        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();           
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.MinDate = DateTime.Parse("01/12/2015");
        }
      

        public void orderno()
        {
            string sql = "select distinct (Uploadfilename+'_'+OrderNo) as 'OrderNo' from APNewSOP_SBIMIS  where ispaymentrecieved='N' and convert(date,CREATIONDATE)='" + OrderDate.SelectedDate + "'";
           // string sql = "select distinct orderno from Vendor_HSRPRecords where dealerid='" + userdealerid + "' and convert(date,hsrprecord_creationdate)='" + OrderDate.SelectedDate + "'";          
            Utils.PopulateDropDownList(DropDownListStateName, sql, CnnString, "--ALL--");
        }
       

        protected void btnGO_Click(object sender, EventArgs e)
        {
            orderno();
        }
        public void ShowGrid()
        {
            string Orderno1 = DropDownListStateName.SelectedItem.Text;
            string SQLString = "select RefNo,ChNo,HsrpNo,Amount,UPLOADFILENAME from APNewSOP_SBIMIS  where ispaymentrecieved='N' and (Uploadfilename+'_'+OrderNo)='" + Orderno1 + "'";
            //SQLString = " select DealerName,	DealerCode,	VehicleClass,	Affixationcode,	VehicleRegNo,	OwnerName,	Address1,	MobileNo,	VehicleType,	HSRPRecord_AuthorizationNo,	EngineNo,	ChassisNo,	vehiclemake,	ModelName,	OrderType from Vendor_HSRPRecords where dealerid='" + userdealerid + "' and convert(date,hsrprecord_creationdate)='" + OrderDate.SelectedDate + "' and orderno='" + orderno + "'";
            DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
            grdid.DataSource = dt;
            grdid.DataBind();
            grdid.Visible = true;
        }
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListStateName.SelectedItem.Text != "--ALL--")
            {
                ShowGrid();
                lblErrMsg.Text = "";
            }
            else
            {              
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select File Name.";
                return;
            }
        }

        protected void btnSync_Click(object sender, EventArgs e)
        {
            if (DropDownListStateName.SelectedItem.Text != "--ALL--")
            {
                using (var conn = new SqlConnection(CnnString))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    try
                    {

                        string sqlquery = "select Distinct HsrpNo,TransactionDate,RefNo,ChNo,Amount,ispaymentrecieved,(Uploadfilename+'_'+OrderNo) as 'UploadFileName',CREATIONDATE from APNewSOP_SBIMIS  where ispaymentrecieved='N' and (Uploadfilename+'_'+OrderNo)='" + DropDownListStateName.SelectedValue.ToString().Trim() + "'";
                        DataTable dtRecords = Utils.GetDataTable(sqlquery, CnnString);

                        if (dtRecords.Rows.Count > 0)
                        {
                            for (int k = 0; k < dtRecords.Rows.Count; k++)
                            {
                                // HsrpRecordid = dtRecords.Rows[k]["hsrprecordid"].ToString();
                                string straptgrecdate = System.DateTime.Now.ToString().Trim();
                                string strispayment = dtRecords.Rows[k]["UploadFileName"].ToString();
                                string Query = "update APNewSOP_SBIMIS set ispaymentrecieved='Y',PaymentReceivedDate=GetDate() where ispaymentrecieved='N' and (Uploadfilename+'_'+OrderNo)='" + strispayment + "' ";
                                Utils.ExecNonQuery(Query, CnnString);
                               // AddLog("***Records Update Successfully***- '" + dtRecords.Rows[k]["ispaymentrecieved"].ToString() + "' " + System.DateTime.Now.ToString());
                            }
                        }
                        llbMSGSuccess.Text = "Record Successfully Update ";
                        
                    }
                    catch (Exception ex)
                    {
                        llbMSGError.Text = "Error in Sync :- " + ex.Message.ToString();                       
                    }
                }
                lblErrMsg.Text = "";

            }
            else 
            {
                lblErrMsg.Text = "Please Select File Name.";
            }
        }

        private void AddLog(string logtext)
        {
            string filename = "UpdateSBIPayment-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".txt";
            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["TGpathx"].ToString();
            fileLocation += filename;
            using (StreamWriter sw = File.AppendText(fileLocation))
            {
                sw.WriteLine(logtext);
                sw.Close();
            }
        }

    }
}