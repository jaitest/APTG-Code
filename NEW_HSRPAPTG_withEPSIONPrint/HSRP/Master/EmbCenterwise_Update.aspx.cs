using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using HSRP;
using System.Data;
using DataProvider;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using System;
using System.Web;

using System.Collections.Generic;
using System.Linq;

using System.Net;


namespace HSRP.Master
{
    public partial class EmbCenterwise_Update : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        int IResult;
        string sendURL = string.Empty;
        string SMSText = string.Empty;
        string SqlQuery = string.Empty;
        string trnasportname, pp;

        BaseFont basefont;


        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;


        int iChkCount = 0;
        string vehicle = string.Empty;
        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();
        string StickerManditory = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {


                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                lblErrMsg.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {

                        if (UserType == "0")
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            DropDownListStateName.Enabled = true;
                            labelClient.Visible = true;
                            dropDownRtoLocation.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListOrganization();
                            FilldropDownListClient();


                            //buildGrid();
                        }

                        //ShowGrid();
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown



        private void FilldropDownListOrganization()
        {
            
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=9 and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
           
        }
        private void FilldropDownListClient()
        {
                SQLString = "select  distinct e.Emb_Center_Id,r.EmbCenterName from embossingcenters e,rtolocation r where e.Emb_Center_Id=r.Navembid and e.state_id=9 and e.activestatus='Y' Order by EmbCenterName";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownRtoLocation.Visible = true;
                dropDownRtoLocation.DataSource = dss;
                dropDownRtoLocation.DataBind();
              
        }
     
        #endregion

        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        }
     

        public void ShowGrid()
        {
            string SQLString = "select ROW_NUMBER() OVER (ORDER BY hsrprecordid) AS RowNum,vehicletype,sendtoerp,embcentername,vehicleregno,hsrp_front_laserCode,hsrp_Rear_LaserCode from hsrprecords h,rtolocation r  where h.rtolocationid=r.rtolocationid and isnull(VehicleRegNo,'')!='' and paymentgateway='NewSOP-SBI' and addrecordby in('OnlineDealer') and convert(date,aptgvehrecdate)='" + OrderDate.SelectedDate.ToString()+ "'   and r.embcentername='" + dropDownRtoLocation.SelectedItem.ToString() + "' and h.hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and activestatus='Y' ";
               
                dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count > 0)
                {
                    //btnUpdate.Visible = true;
                    gvEmpDesignation.DataSource = dt;
                    gvEmpDesignation.DataBind();
                }
                else
                {                   
                    lblErrMsg.Text = "Record Not Found";
                    gvEmpDesignation.DataSource = null;
                    gvEmpDesignation.DataBind();
                }

        }
        StringBuilder sb = new StringBuilder();
       

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownRtoLocation.Visible = true;
            labelClient.Visible = true;

        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            string SQLString = "select ROW_NUMBER() OVER (ORDER BY hsrprecordid) AS RowNum,vehicletype,sendtoerp,embcentername,vehicleregno from hsrprecords h,rtolocation r  where h.rtolocationid=r.rtolocationid and isnull(VehicleRegNo,'')!='' and paymentgateway='NewSOP-SBI' and addrecordby in('OnlineDealer') and convert(date,aptgvehrecdate)='" + OrderDate.SelectedDate + "'   and r.embcentername='" + dropDownRtoLocation.SelectedItem.ToString() + "' and h.hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and activestatus='Y' ";            
            DataTable dtRecords = Utils.GetDataTable(SQLString, CnnString);
            SqlQuery = "update hsrprecords set sendtoerp=NULL from rtolocation r where hsrprecords.rtolocationid=r.rtolocationid and isnull(VehicleRegNo,'')!='' and paymentgateway='NewSOP-SBI' and addrecordby in('OnlineDealer') and convert(date,aptgvehrecdate)='" + OrderDate.SelectedDate.ToString() + "' and r.embcentername='" + dropDownRtoLocation.SelectedItem.ToString() + "'";
            Utils.ExecNonQuery(SqlQuery, CnnString);                   
            lblcount.Text = "Record Successfully '" + dtRecords.Rows.Count + "' Update ";         
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

        protected void gvEmpDesignation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEmpDesignation.PageIndex = e.NewPageIndex;
            ShowGrid();
        }
        DataTable dt = new DataTable();
        protected void dropdownDuplicateFIle_SelectedIndexChanged(object sender, EventArgs e)
        {           
            ShowGrid();
        }
        protected void dropDownRtoLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowGrid();
        }

        
    }
}


