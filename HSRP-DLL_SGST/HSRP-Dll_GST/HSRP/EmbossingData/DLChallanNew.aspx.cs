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
using System.Collections;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace HSRP.EmbossingData
{
    public partial class DLChallanNew : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string SQlQuery = string.Empty;
        string ExicseAmount = string.Empty;
        string TotalWeight = string.Empty;
        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        string EmbCenterName = string.Empty;

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
                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            // FilldropDownListClient();
                            Filldropdowndealer();
                            FilldropDownEmbossingCenter();
                            // btnrecordinpdf.Visible = true;
                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListOrganization();
                            // FilldropDownListClient();
                            Filldropdowndealer();
                            FilldropDownEmbossingCenter();
                            //btnrecordinpdf.Visible = true;

                        }

                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown

        public void FileDetail()
        {

        }

        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");

            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }
        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                // SQLString = "select Emb_Center_Id,EmbCenterName from EmbossingCenters WHERE State_Id=2";
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");
                dataLabellbl.Visible = false;
                TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                // SQLString = "select Emb_Center_Id, EmbCenterName from EmbossingCenters WHERE State_Id=2";
                // SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID and a.ActiveStatus ='Y'  where UserRTOLocationMapping.UserID='" + UserID + "' order by a.rtolocationname ";

                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=2 and ActiveStatus!='N'  and Navembid='"+ddlemb.SelectedValue.ToString() +"' Order by RTOLocationName";

                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=2 and ActiveStatus!='N' Order by RTOLocationName";

                DataTable dss = Utils.GetDataTable(SQLString, CnnString);

                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownListClient.Visible = true;

                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                dataLabellbl.Visible = true;
            }
        }

        private void FilldropDownEmbossingCenter()
        {
            if (UserType == "0")
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select Emb_Center_Id,EmbCenterName from EmbossingCenters WHERE State_Id=2";
                //  SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                Utils.PopulateDropDownList(ddlemb, SQLString.ToString(), CnnString, "--Select Embossing Center Name--");

                dataLabellbl.Visible = false;
                TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                SQLString = "select Emb_Center_Id, EmbCenterName from EmbossingCenters WHERE State_Id=2";
                //SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID and a.ActiveStatus ='Y'  where UserRTOLocationMapping.UserID='" + UserID + "' order by a.rtolocationname ";

                // Utils.PopulateDropDownList(ddlemb, SQLString.ToString(), CnnString, "--Select Embossing Center Name--");

                DataTable dss = Utils.GetDataTable(SQLString, CnnString);



                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                ddlemb.Visible = true;
                ddlemb.DataSource = dss;
                ddlemb.DataBind();
                ddlemb.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select Embossing Center Name--", "--Select Embossing Center Name--"));
                dataLabellbl.Visible = true;
            }
        }
        private void Filldropdowndealer()
        {


            string strDate = OrderDate.SelectedDate.ToString("dd/MM/yyyy");
            String[] StringAuthDate = strDate.Replace("-", "/").Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            string AuthorizationDate = From + " 00:00:00";
            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Replace("-", "/").Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            string ToDate = From1 + " 23:59:59";


            string strFrmDateString = OrderDate.SelectedDate.ToShortDateString() + " 00:00:00";
            string strToDateString = HSRPAuthDate.SelectedDate.ToShortDateString() + " 23:59:59";

            if (UserType == "0")
            {
                // SQLString = "select [NAME OF THE DEALER] from delhi_dealermaster  where ActiveStatus='Y' order by [name of the dealer]";

                string SQLString1 = "select distinct h.dealerid,a.[NAME OF THE DEALER]  from delhi_dealermaster a,hsrprecords as h where a.SNO=h.dealerid and H.hsrprecord_creationdate between '" + strFrmDateString + "' and '" + strToDateString + "' and h. NAVEMBID='" + ddlemb.SelectedValue.ToString() + "' order by [NAME OF THE DEALER]";
                Utils.PopulateDropDownList(ddlBothDealerHHT, SQLString1.ToString(), CnnString, "All");


            }
            else
            {
                // SQLString = "select [NAME OF THE DEALER]  from delhi_dealermaster  where  ActiveStatus='Y' order by [name of the dealer]";

                string SQLString1 = "select distinct h.dealerid,a.[NAME OF THE DEALER]  from delhi_dealermaster a,hsrprecords as h where a.SNO=h.dealerid and H.hsrprecord_creationdate between '" + strFrmDateString + "' and '" + strToDateString + "' and h. NAVEMBID='" + ddlemb.SelectedValue.ToString() + "' order by [NAME OF THE DEALER] ";
                Utils.PopulateDropDownList(ddlBothDealerHHT, SQLString1.ToString(), CnnString, "All");
                //DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                //ddlBothDealerHHT.DataSource = dts;
                //ddlBothDealerHHT.DataBind();
            }
        }
        #endregion

        private DataTable GetRecords(string strRecordId)
        {
            string strInvoiceNo = string.Empty;
            DataTable dtInvoiceData = new DataTable();
            string SQLString12 = "select  CashReceiptNo as 'Receipt Voucher No.',OwnerName as 'Customer Name',Vehicleregno as 'Vehicle No.', (select [NAME OF THE DEALER] from delhi_dealermaster where sno=a.dealerid) as Dealer,gstbasicamount as 'Taxable Value',cgstamount as 'CGST',utgstamount as 'SGST' ,RoundOff_NetAmount as 'Total Amount' from hsrprecords a  where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and hsrprecordid=" + strRecordId + "  ";
            dtInvoiceData = Utils.GetDataTable(SQLString12, CnnString);
            return dtInvoiceData;
        }

        private void ShowGrid(string strFromdate, string strToDate)
        {
            ddlemb.Visible = true;
            Button1.Visible = true;
            btnrecordinpdf.Visible = true;
            // string ddVehicleType = DropDownList1.SelectedItem.ToString().ToUpper();

            string DealerName = ddlBothDealerHHT.SelectedValue;
            string Dname = string.Empty;
            string Did = string.Empty;
            string DealerId = string.Empty;
            try
            {
                string SQLString1 = "select distinct dealerid  from delhi_dealermaster a,hsrprecords as h where a.SNO=h.dealerid and H.hsrprecord_creationdate between '" + strFromdate + "' and '" + strToDate + "' and  h.HSRP_StateID=2 and a.[NAME OF THE DEALER]='" + DealerName + "' and h. NAVEMBID='" + ddlemb.SelectedValue.ToString() + "' ";
                DealerId = Utils.getDataSingleValue(SQLString1, CnnString, "dealerid");
            }
            catch (Exception ex)
            {
            }

            try
            {
               
                    SQLString = "select e.[EmbCenterName], Row_Number() over(order by orderstatus) as SNo, hsrprecordid,HSRPRecord_AuthorizationNo,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus " +
                "from hsrprecords  as h,[EmbossingCenters] as e where h.NAVEMBID=E.Emb_Center_Id AND hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and Invoice_Flag='N' and dealerid='" + DealerId + "'" +
                 " and orderstatus ='Embossing Done' and InvoiceNo is null and Hsrprecord_creationdate between '" + strFromdate + "' and '" + strToDate + "'  and hsrp_rear_lasercode is not null and hsrp_Front_lasercode is not null  and  convert(date,OrderEmbossingdate) >= convert(date,'2017-06-30') " +
                "order by VehicleClass,VehicleType,VehicleRegNo";
                

              
               
                //and  convert(date,Hsrprecord_creationdate) >= convert(date,'2017-07-01')
                dt = Utils.GetDataTable(SQLString, CnnString);

                //and  convert(date,OrderEmbossingdate) <= convert(date,'2017-06-30')
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    btnChalan.Visible = true;
                }
                else
                {
                    btnChalan.Visible = false;
                    //    Button1.Visible = false;
                    lblErrMsg.Text = "Record not found for the date range selected.";
                    //lblErrMsg.Text = "Record Not Found";
                    GridView1.DataSource = null;
                    GridView1.DataBind();

                }


            }
            catch (Exception ex)
            { }

        }
        StringBuilder sb = new StringBuilder();
        CheckBox chk;
        protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk1 = GridView1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
            if (chk1.Checked == true)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = true;
                }
            }
            else if (chk1.Checked == false)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = false;
                }
            }

        }


        protected void Grid1_ItemCommand(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {

        }
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;
            FilldropDownEmbossingCenter();

        }
        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-3.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {

                string strDate = OrderDate.SelectedDate.ToString("dd/MM/yyyy");
                String[] StringAuthDate = strDate.Replace("-", "/").Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                string AuthorizationDate = From + " 00:00:00";
                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Replace("-", "/").Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
                String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ToDate = From1 + " 23:59:59";


                string strFrmDateString = OrderDate.SelectedDate.ToShortDateString() + " 00:00:00";
                string strToDateString = HSRPAuthDate.SelectedDate.ToShortDateString() + " 23:59:59";

                ShowGrid(strFrmDateString, strToDateString);


            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }

        }
        DataTable dt = new DataTable();


        protected void btnChalan_Click(object sender, EventArgs e)
        {
           
            btnrecordinpdf.Visible = true;


            try
            {
                #region Validation
                if (string.IsNullOrEmpty(txtTransporter.Text))
                {
                    Response.Write("<script> alert('Please Enter Transporter')</script>");
                    return;
                }
                if (string.IsNullOrEmpty(txtLorryNo.Text))
                {
                    Response.Write("<script> alert('Please Enter Lorry No.')</script>");
                    return;
                }
                #endregion

                string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                string RtoName = string.Empty;
                RtoName = dropDownListClient.SelectedItem.ToString();
                HttpContext context = HttpContext.Current;
                string dealername2 = ddlBothDealerHHT.SelectedItem.ToString();
                string filename = "HSRP-DC_" + dealername2.ToString().Trim() + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

                string SQLString = String.Empty;
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                StringBuilder bb = new StringBuilder();

                //Creates an instance of the iTextSharp.text.Document-object:
                Document document = new Document();

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                Int64 totalamount = 0;

                //Opens the document:
                object Exicse;
                object Vatamt;
                object TotalAmount;
                object TotalAMT;
                object Qty;
                object SecondaryCess;
                object Educess;
                string vehicle = string.Empty;
                string strHsrpRecordId = string.Empty;
                string strInvoiceNo = string.Empty;
                string strEmbStationName = string.Empty;
                string strEmbAddress = string.Empty;
                
                string strEmbCity = string.Empty;
                string strEmbId = string.Empty;
                

                #region Set ChallanNo
                try
                {
                    string strGetInvoiceNo = string.Empty;

                   
                    string strSelectEmbStation = "SELECT DISTINCT [NAVEMBID],[EmbCenterName],Address1,city FROM [vw_RTOLocationWiseEmbosingCenters] WHERE NAVEMBID='" + ddlemb.SelectedValue + "'";
                    DataTable dtEmbData = Utils.GetDataTable(strSelectEmbStation, CnnString);
                    if (dtEmbData.Rows.Count <= 0)
                    {
                        lblErrMsg.Text = "Embossing Station not found";
                        return;
                    }
                    strEmbStationName = dtEmbData.Rows[0]["EmbCenterName"].ToString();
                    strEmbAddress = dtEmbData.Rows[0]["address1"].ToString();                    
                    strEmbCity = dtEmbData.Rows[0]["city"].ToString();
                    strEmbId = dtEmbData.Rows[0]["NAVEMBID"].ToString();

                    string strGetFinYear = "SELECT [dbo].[fnGetFiscalYear] ( GetDate() )";
                    string strdate=(Utils.getScalarValue(strGetFinYear, CnnString)).Replace("20", string.Empty);

                    strGetInvoiceNo = "select (isnull(prefixtext,'') +'" + strdate + "'+ '/' + right('0000000'+ convert(varchar,lastno+1),7)) from [hsrpState] " +
                                            "where Hsrp_stateID=2 and prefixfor='Cash Receipt No' ";
                    strInvoiceNo = (Utils.getScalarValue(strGetInvoiceNo, CnnString));
                }
                catch
                {
                    lblErrMsg.Text = "Embossing Station not found";
                    return; 
                }

                
                strInvoiceNo = strInvoiceNo.ToString() ;

               

                #endregion
                int iChkCount = 0;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                    if (chk.Checked == true)
                    {
                        iChkCount = iChkCount + 1;
                        Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;

                        Label OrderStatus = GridView1.Rows[i].Cells[4].FindControl("lblOrderStatus") as Label;

                        string strRecordId = GridView1.DataKeys[i]["hsrprecordid"].ToString();
                        string flaser = GridView1.Rows[i].Cells[4].FindControl("lblOrderStatus").ToString();
                        string rlaser = GridView1.Rows[i].Cells[4].FindControl("lblOrderStatus").ToString();

                        if (strHsrpRecordId == "")
                        {
                            strHsrpRecordId = "'" + strRecordId + "'";
                        }
                        else
                        {
                            strHsrpRecordId = strHsrpRecordId + ",'" + strRecordId + "'";
                        }

                        string InvoiceUser = Session["UID"].ToString();
                        string sqluser = "select UserLoginName from users where userid='" + InvoiceUser + "'";
                        string username = Utils.getDataSingleValue(sqluser, CnnString, "UserLoginName");

                        sb.Append("update hsrprecords set ChallanNo='" + strInvoiceNo + "', ChallanCreatedBy='" + InvoiceUser + "', challandate=getdate(),Invoice_Flag='Y' where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and hsrprecordid ='" + strRecordId + "' and  isnull( ChallanNo ,'')=''  and  isnull( challandate ,'')='' and  Invoice_Flag ='N'   ;");
                        //changes for Customer Invoice 

                        string strGetcustomerInvoiceNo = " select (prefixtext+right('000000'+ convert(varchar,lastno+1),6))   from prefix  where hsrp_stateid= '" + DropDownListStateName.SelectedValue + "'  and  prefixfor = 'Customer Invoice'  and  RTOLocationID ='" + dropDownListClient.SelectedValue.ToString() + "' ";
                        strGetcustomerInvoiceNo = (Utils.getScalarValue(strGetcustomerInvoiceNo, CnnString));

                        int cus = Utils.ExecNonQuery("update hsrprecords set Invoiceno='" + strGetcustomerInvoiceNo + "' where  hsrprecordid ='" + strRecordId + "'  ", CnnString);
                        if (cus > 0)
                        {

                            Utils.ExecNonQuery("update prefix set LastNo =  (select right('000000'+ convert(varchar,lastno+1),6)  from prefix  where hsrp_stateid= '" + DropDownListStateName.SelectedValue + "'  and  prefixfor = 'Customer Invoice' and  RTOLocationID ='" + dropDownListClient.SelectedValue.ToString() + "' )  where hsrp_stateid= '" + HSRPStateID + "'  and  prefixfor = 'Customer Invoice' and  RTOLocationID ='" + dropDownListClient.SelectedValue.ToString() + "' ", CnnString);
                        }

                        //changes end for Customer Invoice 


                    }
                }
                int k = 0;
                if ( !string.IsNullOrEmpty( sb.ToString()))
                {
                 k= Utils.ExecNonQuery(sb.ToString(), CnnString);
                }

                if (k > 0)
                {
                    string strUpdateInvoiceNo = "update [hsrpState] set lastno=lastno+1 where Hsrp_stateID=2 and prefixfor='Cash Receipt No'";
                    Utils.ExecNonQuery(strUpdateInvoiceNo, CnnString);
                     
                }




                DataTable GetAddress = new DataTable();
                DataTable DtGSTIN = new DataTable();
                string Address = string.Empty;
                if (iChkCount.Equals(0))
                {
                    Response.Write("<script> alert('Please select atleast 1 record')</script>");
                    document.Close();
                    return;
                }
                if (ddlemb.SelectedItem.Text == "")
                {
                    lblErrMsg.Text = "Please Select Embossing Center";
                    return;
                }
                else
                {
                    EmbCenterName = ddlemb.SelectedItem.Text;
                    GetAddress = Utils.GetDataTable("select * from EmbossingCenters WHERE State_Id='" + DropDownListStateName.SelectedValue + "' and EmbCenterName='" + EmbCenterName + "'", CnnString);

                    DtGSTIN = Utils.GetDataTable("select GSTIN  , CGSTPer, UTGSTPer from hsrpstate WHERE HSRP_StateID='" + DropDownListStateName.SelectedValue + "'", CnnString);
                }
                if ((GetAddress.Rows[0]["pincode"].ToString() != "") || (GetAddress.Rows[0]["pincode"] != null))
                {
                    Address = " - " + GetAddress.Rows[0]["pincode"];

                }
                else
                {
                    Address = "";

                }


                string sqlstring = string.Empty;
                sqlstring = "insert into InvoiceMaster(InvoiceNo,InvoiceDate,Amount,BuyerName,clientName,hsrp_stateid,dispatchedLocation) values('" + strInvoiceNo + "', Convert(date,('" + currentdate + "'),103),'" + totalamount + "','" + RtoName + "','" + GetAddress.Rows[0]["Address1"].ToString() + "','" + DropDownListStateName.SelectedValue + "','" + dropDownListClient.SelectedItem.Text + "')";
                Utils.ExecNonQuery(sqlstring, CnnString);



                string strSelectAddress = "SELECT RTOLocationAddress FROM [RTOLocation] WHERE RTOLocationId='" + dropDownListClient.SelectedValue + "'";
                DataTable dtAddress = Utils.GetDataTable(strSelectAddress, CnnString);

                string strRTOAddress = dtAddress.Rows[0]["RTOLocationAddress"].ToString();

                PdfPTable table2 = new PdfPTable(10);
                PdfPTable table1 = new PdfPTable(10);
                PdfPTable table = new PdfPTable(10);
                PdfPTable table3 = new PdfPTable(2);

                //actual width of table in points
                table.TotalWidth = 1000f;
                HSRPStateID = DropDownListStateName.SelectedValue;
                string OldRegPlate = string.Empty;



                DataTable dt = new DataTable();

              

               //SQlQuery = "select vehicletype+' (Damage Front)' as DescriptionOfGoods,count(*) as qty,round((sum(EXCISEBASIC)/count(*)),3) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess  from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and ordertype in ('DF') and HsrpRecordID in(" + strHsrpRecordId + ") group by vehicletype union   select vehicletype+' (Damage Rear)',count(*) as qty,round((sum(EXCISEBASIC)/count(*)),2) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and  ordertype in ('DR') group by vehicletype union select vehicletype+' (Both Plates)',(count(*)*2)/2 qty,round((sum(EXCISEBASIC)/count(*)),2) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and ordertype in ('NB','OB','DB') group by vehicletype";
                SQlQuery = "select vehicletype+' (Damage Front)' as DescriptionOfGoods,count(*) as qty,round((sum(GSTBasicAmount)/count(*)),3) amt,round(sum(GSTBasicAmount),2)bamt,round(sum(UTGSTAmount),2)Vatamt,round(sum(CGSTAmount),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess  from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and ordertype in ('DF') and HsrpRecordID in(" + strHsrpRecordId + ") group by vehicletype union   select vehicletype+' (Damage Rear)',count(*) as qty,round((sum(GSTBasicAmount)/count(*)),2) amt,round(sum(GSTBasicAmount),2)bamt,round(sum(UTGSTAmount),2)Vatamt,round(sum(CGSTAmount),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and  ordertype in ('DR') group by vehicletype union select vehicletype+' (Both Plates)',(count(*)*2)/2 qty,round((sum(GSTBasicAmount)/count(*)),2) amt,round(sum(GSTBasicAmount),2)bamt,round(sum(UTGSTAmount),2)Vatamt,round(sum(CGSTAmount),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords  where hsrp_stateid='2' and HsrpRecordID in(" + strHsrpRecordId + ") and ordertype in ('NB','OB','DB') group by vehicletype";
                dt = Utils.GetDataTable(SQlQuery, CnnString);
                BAL obj = new BAL();
                string UserID = Session["UID"].ToString();
                decimal dTotalAmount = 0;
                
                if (true)
                {

                    #region Upper Part of PDF

                    string strStateId = DropDownListStateName.SelectedValue;
                    string strLocId = dropDownListClient.SelectedValue;
                    DataTable dataSetFillHSRPDeliveryChallan = new DataTable();
                    string strEccNo = GetAddress.Rows[0]["ExciseNo"].ToString();
                    string strDivision = GetAddress.Rows[0]["Division"].ToString();
                    string strRange = GetAddress.Rows[0]["Range"].ToString();
                    string strCommissionerate = GetAddress.Rows[0]["Commissionerate"].ToString();

                    string strTin = DtGSTIN.Rows[0]["GSTIN"].ToString();

                    string strTariffHead = GetAddress.Rows[0]["CH"].ToString();
                    string strCompName = GetAddress.Rows[0]["CompanyName"].ToString();

                    GenerateCell(table, 10, 1, 1, 1, 1, 1, 0, "Delivery Challan \r\n \r\n", 0, 0);
                    GenerateCell(table, 10, 1, 1, 1, 1, 1, 0, strCompName + "\r\n\r\n" + "GSTIN - " + strTin, 0, 0);


                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, " ", 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, " ", 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "Dispatch From:", 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "Challan No - " + strInvoiceNo, 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, strEmbStationName + "," + strEmbAddress, 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "Date  -" + System.DateTime.Now.ToString("dd/MM/yyyy"), 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, " ", 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, " ", 0, 0);


                    #endregion

                    #region BlankRow
                    GenerateCell(table, 10, 1, 1, 1, 0, 0, 1, "", 10, 0);
                    #endregion

                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "Dispatch To:", 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "Transporter Name:" + txtTransporter.Text.ToString(), 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "Affixation Center: " + strCompName, 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "Lorry  No.:" + txtLorryNo.Text.ToString(), 0, 0);
                    
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "Address: " + strRTOAddress, 0, 0);
                    //  GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "Vehicle No.:" + txtLorryNo.Text.ToString(), 0, 0);  
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "GR/LR No.:", 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "GSTIN:" + strTin, 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, " ", 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "State Code:  " + "07", 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, " ", 0, 0);

                    #region Column Heading Creation

                    GenerateCell(table, 1, 1, 0, 1, 0, 1, 0, "SI.NO.", 0, 0);
                    GenerateCell(table, 4, 1, 1, 1, 0, 1, 0, "Description of Goods", 0, 0);
                    GenerateCell(table, 1, 0, 0, 1, 0, 1, 0, "HSN", 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 0, 1, 0, "UOM", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 0, "Qty-:Set", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 0, "Rate", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 0, "Amount (Rs.)", 0, 0);


                    #endregion


                    #region 1st Row

                    GenerateCell(table, 1, 1, 0, 1, 0, 1, 1, "1", 0, 0);

                    GenerateCell(table, 4, 1, 1, 1, 0, 1, 0, "HSRP SET FOR:", 0, 0);
                    GenerateCell(table, 1, 0, 0, 1, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 0, 0, "", 0, 0);
                    for (int i = 0; i < 2; i++)
                    {
                        GenerateCell(table, 1, 0, 1, 1, 0, 0, 0, "", 0, 0);
                    }
                    #endregion

                    #region 2nd Row


                    Exicse = dt.Compute("sum(excise)", "");
                    ExicseAmount = String.Format("{0:0.00}", Exicse);
                    TotalAmount = dt.Compute("sum(bamt)", "");
                    Vatamt = dt.Compute("sum(Vatamt)", "");
                    TotalAMT = dt.Compute("sum(amt)", "");
                    Qty = dt.Compute("sum(qty)", "");
                    TotalWeight = "";// dt.Compute("sum(HSRPWeight)", "");
                    Educess = dt.Compute("sum(cess)", "");
                    SecondaryCess = dt.Compute("sum(shecess)", "");

                    string Newamount = String.Format("{0:0.00}", TotalAmount);
                    string VatamtNew = String.Format("{0:0.00}", Vatamt);
                    string strTotalAMT = String.Format("{0:0.00}", TotalAMT);
                    string strEducess = String.Format("{0:0.00}", Educess);
                    string strSecondaryCess = string.Format("{0:0.00}", SecondaryCess);

                    for (int iResult = 0; iResult < dt.Rows.Count; iResult++)
                    {
                        GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, Convert.ToString(iResult + 1), 0, 0);
                        GenerateCell(table, 4, 1, 1, 0, 0, 1, 0, dt.Rows[iResult]["DescriptionOfGoods"].ToString(), 0, 0);
                        GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, strTariffHead, 0, 0);
                        GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "Set", 0, 0);
                        GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, dt.Rows[iResult]["qty"].ToString(), 0, 0);
                       // GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, String.Format("{0:0.00}", dt.Rows[iResult]["amt"].ToString()), 0, 0);
                        //GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, String.Format("{0:0.00}", dt.Rows[iResult]["bamt"]), 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, Convert.ToString(decimal.Round(Convert.ToDecimal(dt.Rows[iResult]["amt"].ToString()), 2, MidpointRounding.AwayFromZero)), 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, Convert.ToString(decimal.Round(Convert.ToDecimal(dt.Rows[iResult]["bamt"].ToString()), 2, MidpointRounding.AwayFromZero)), 0, 0);

                    }
                    if (string.IsNullOrEmpty(ExicseAmount))
                    {
                        ExicseAmount = "0";
                    }

                    if (string.IsNullOrEmpty(Newamount))
                    {
                        Newamount = "0";
                    }

                    if (string.IsNullOrEmpty(strEducess))
                    {
                        strEducess = "0";
                    }

                    if (string.IsNullOrEmpty(strSecondaryCess))
                    {
                        strSecondaryCess = "0";
                    }


                    decimal PreTotalAmount = 0;
                    PreTotalAmount = PreTotalAmount + Convert.ToDecimal(ExicseAmount) + Convert.ToDecimal(Newamount) + Convert.ToDecimal(strEducess) + Convert.ToDecimal(strSecondaryCess);
                    dTotalAmount = PreTotalAmount + Convert.ToDecimal(VatamtNew);

                    Int64 Damount = Convert.ToInt64(Math.Round(dTotalAmount, 2));

                    #endregion


                    #region Blank
                    for (int j = 0; j < 4; j++)
                    {
                        GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                        GenerateCell(table, 4, 1, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);

                    }
                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 4, 1, 1, 1, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 1, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 1, "Sub Total", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 1, Newamount, 0, 0);



                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 4, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 1, "", 0, 0);

                    decimal cgstpre = decimal.Round(Convert.ToDecimal(DtGSTIN.Rows[0]["CGSTPer"].ToString()));
                    decimal sgstpre = decimal.Round(Convert.ToDecimal(DtGSTIN.Rows[0]["UTGSTPer"].ToString()));
                                             
                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 4, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "CGST@", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, cgstpre + "%", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, ExicseAmount.ToString(), 0, 0);

                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 4, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);


                    GenerateCell(table, 1, 1, 0, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 4, 1, 1, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "SGST@", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 2, 1, sgstpre + "%", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 2, 1, VatamtNew.ToString(), 0, 0);

                    #endregion

                    #region Bottom

                    #region Total Price

                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    HSRP.Transaction.InvoiceTransaction.NumToWord trans = new HSRP.Transaction.InvoiceTransaction.NumToWord();
                    string totalinwords = trans.changeNumericToWords(Damount);

                    GenerateCell(table, 8, 1, 1, 0, 0, 2, 1, "Total Invoice Value", 0, 0);

                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 0, dTotalAmount.ToString(), 0, 0);
                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 8, 1, 1, 0, 0, 2, 1, "Total RoundOff Invoice Value", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 0, Damount.ToString(), 0, 0);



                    GenerateCell(table, 10, 1, 1, 1, 1, 1, 1, "", 1, 0);
                    GenerateCell(table, 3, 1, 0, 0, 0, 0, 0, "Total Weight(in grams.)", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 0, 1, TotalWeight.ToString(), 0, 0);
                    GenerateCell(table, 5, 0, 0, 0, 0, 0, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, "", 0, 0);

                    #endregion

                    #region Declaration

                    GenerateCell(table, 10, 1, 1, 1, 0, 0, 1, "Declaration: Certified that the particulars given above are true & correct and the amount indicated represents the price actually charged and there is no additional flow of money either directly or indirectly from the buyer.", 0, 0);

                    #endregion

                    #region Vat

                    double dnetval = (Damount * 100.0) / (114.5);

                    double dVatval = Math.Round(Damount - dnetval);

                    string strVatinWords = trans.changeNumericToWords(dVatval);

                    GenerateCell(table, 4, 1, 0, 1, 0, 0, 1, "", 0, 0);
                    GenerateCell(table, 6, 1, 1, 1, 0, 2, 1, "E. & O.E", 0, 0);

                    #endregion

                    #region LinkAutoTech

                    GenerateCell(table, 4, 1, 0, 0, 0, 0, 1, "Total amount in words : " + totalinwords, 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 0, 2, 0, "For " + GetAddress.Rows[0]["CompanyName"].ToString(), 0, 0);

                    #endregion

                    #region Blank


                    for (int i = 0; i < 5; i++)
                    {
                        GenerateCell(table, 4, 1, 0, 0, 0, 1, 0, "", 0, 0);

                        GenerateCell(table, 6, 1, 1, 0, 0, 2, 0, "", 0, 0);

                    }

                    #endregion

                    #region Sign

                    GenerateCell(table, 4, 1, 0, 0, 1, 1, 0, "", 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 1, 2, 0, "Authorized Signatory", 0, 0);

                    #endregion

                    #endregion

                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 2, 0, 0, 0, 0, 2, 1, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GridView1.Visible = false;
                    document.Add(table1);
                    document.Add(table);
                    document.Add(table3);
                    document.NewPage();
                    document.Close();
                    context.Response.ContentType = "Application/pdf";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.WriteFile(PdfFolder);
                    context.Response.End();
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }
        }



        private DataTable Getchallan(string strRecordId)
        {
            string strInvoiceNo = string.Empty;
            DataTable dtInvoicename = new DataTable();

            string SQLString12 = " select  distinct ChallanNo from hsrprecords a where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and hsrprecordid=" + strRecordId + "  ";

            dtInvoicename = Utils.GetDataTable(SQLString12, CnnString);

            return dtInvoicename;

        }
        protected void Button1_Click(object sender, EventArgs e)
        {

            foreach (GridViewRow di in GridView1.Rows)
            {
                CheckBox chkBx = (CheckBox)di.FindControl("CHKSelect");

                if (chkBx != null && chkBx.Checked)
                {

                    string strRecordId = GridView1.DataKeys[di.RowIndex].Value.ToString();
                    DataTable dtDataa = Getchallan(strRecordId);
                    hdnfldinvoiceno.Value = dtDataa.Rows[0]["ChallanNo"].ToString();
                }


            }

            string chkb = string.Empty;
            string filename1 = "Challan" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            String StringField1 = String.Empty;
            String StringAlert1 = String.Empty;
            StringBuilder bb1 = new StringBuilder();
            Document document1 = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
            document1.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            string PdfFolder1 = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename1;
            PdfWriter.GetInstance(document1, new FileStream(PdfFolder1, FileMode.Create));
            document1.Open();
            //Adds content to the document:
            PdfPTable table = new PdfPTable(8);
            table.TotalWidth = 6900f;


            GenerateCell(table, 8, 0, 0, 0, 0, 1, 0, "Annexure-" + hdnfldinvoiceno.Value.ToString() + "Commodity - HSRP,HSN Code - 8310", 0, 0);

            GenerateCell(table, 1, 1, 1, 1, 0, 0, 1, "Receipt Voucher No.", 0, 0);
            GenerateCell(table, 1, 1, 1, 1, 0, 0, 1, "Customer Name", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Vehicle No.", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Dealer", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Taxable Value", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "CGST", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "SGST", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Total Amount", 0, 0);


            int iRecCount = 0;
            foreach (GridViewRow di in GridView1.Rows)
            {


                CheckBox chkBx = (CheckBox)di.FindControl("CHKSelect");

                if (chkBx != null && chkBx.Checked)
                {

                    string strRecordId = GridView1.DataKeys[di.RowIndex].Value.ToString();
                    DataTable dtData = GetRecords(strRecordId);
                    if (dtData.Rows.Count > 0)
                    {
                        iRecCount++;
                        string Receipt = dtData.Rows[0]["Receipt Voucher No."].ToString();
                        string Customer = dtData.Rows[0]["Customer Name"].ToString();
                        string Vehicleno = dtData.Rows[0]["Vehicle No."].ToString();
                        string dealername = dtData.Rows[0]["Dealer"].ToString();
                        string Taxablevalue = dtData.Rows[0]["Taxable Value"].ToString();
                        string strCGST = dtData.Rows[0]["CGST"].ToString();
                        string strSGST = dtData.Rows[0]["SGST"].ToString();
                        string amount = dtData.Rows[0]["Total Amount"].ToString();

                        GenerateCell(table, 1, 1, 1, 1, 1, 0, 1, Receipt, 0, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, Customer, 0, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, Vehicleno, 0, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, dealername, 0, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, Taxablevalue, 0, 0);
                        if (string.IsNullOrEmpty(strCGST))
                        {
                          strCGST ="0";
                        }
                        if (string.IsNullOrEmpty(strSGST))
                        {
                            strSGST = "0";
                        }
                        GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, Convert.ToString(decimal.Round(Convert.ToDecimal(strCGST), 2, MidpointRounding.AwayFromZero)), 0, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, Convert.ToString(decimal.Round(Convert.ToDecimal(strSGST), 2, MidpointRounding.AwayFromZero)), 0, 0);
                        GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, amount, 0, 0);

                    }
                }
            }
            if (iRecCount > 0)
            {
                document1.Add(table);
                document1.Close();
                HttpContext context1 = HttpContext.Current;
                context1.Response.ContentType = "Application/pdf";
                context1.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename1);
                context1.Response.WriteFile(PdfFolder1);
                context1.Response.End();
            }
            else
            {
                lblErrMsg.Text = "Selected  records have not been Embossed";
                return;
            }

        }      



        private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(0))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else if (iFont.Equals(1))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            newCellPDF.Colspan = iSpan;
            newCellPDF.BorderWidthLeft = iLeftWidth;
            newCellPDF.BorderWidthRight = iRightWidth;
            newCellPDF.BorderWidthTop = iTopWidth;
            newCellPDF.BorderWidthBottom = iBottomWidth;
            newCellPDF.HorizontalAlignment = iAllign;
            if (!iRowHeight.Equals(0))
            {
                newCellPDF.FixedHeight = iRowHeight;
            }
            if (!iRowWidth.Equals(0))
            {
            }
            table.AddCell(newCellPDF);
        }

    
        protected void btnGO1_Click(object sender, EventArgs e)
        {

            string strDate = OrderDate.SelectedDate.ToString("dd/MM/yyyy");
            String[] StringAuthDate = strDate.Replace("-", "/").Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            string AuthorizationDate = From + " 00:00:00";
            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Replace("-", "/").Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            string ToDate = From1 + " 23:59:59";


            string strFrmDateString = OrderDate.SelectedDate.ToShortDateString() + " 00:00:00";
            string strToDateString = HSRPAuthDate.SelectedDate.ToShortDateString() + " 23:59:59";
            ddlemb.Visible = true;
            Button1.Visible = true;
           

            string DealerName = ddlBothDealerHHT.SelectedValue;
            string Dname = string.Empty;
            string Did = string.Empty;
            string DealerId = string.Empty;
            try
            {
                labelSelectType.Visible = true;
                ddlBothDealerHHT.Visible = true;
                string SQLString1 = "select distinct dealerid,[NAME OF THE DEALER]  from delhi_dealermaster a,hsrprecords as h where a.SNO=h.dealerid and H.Invoice_Flag='N' and a.ACTIVESTATUS='Y' and H.hsrprecord_creationdate between '" + strFrmDateString + "' and '" + strToDateString + "' and  h.HSRP_StateID=2  and h. NAVEMBID='" + ddlemb.SelectedValue.ToString() + "'  order by [NAME OF THE DEALER]  ";
                 Utils.PopulateDropDownList(ddlBothDealerHHT, SQLString1.ToString(), CnnString, "--Select Dealer Name--");
            }
            catch (Exception ex)
            { }

        }

        protected void ddlemb_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        protected void ddlBothDealerHHT_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}