﻿using System.Web.UI;
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
using RestSharp;
using System.Collections.Generic;

namespace HSRP.EmbossingData
{
    public partial class EmbossingAPRTD : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;


        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;

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

        public void FileDetail()
        {

        }

        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

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

                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");

                dataLabellbl.Visible = false;
                TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' order by RTOLocationName";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);

                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownListClient.Visible = true;

                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                dataLabellbl.Visible = true;

                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();

                    }
                    //  lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));

                }
            }
        }

        #endregion

        private void ShowGrid()
        {
            LblErrorMessage.Text = "";
            LblMessage.Text = "";
            string SQLString = string.Empty;
            if (dropdownDuplicateFIle.SelectedItem.Text != "--Select RTO Name--")
            {
                if (HSRPStateID == "9" || HSRPStateID == "11")
                {

                    //int i = -3;
                    ////int j;
                    //SQLString = " select count(*) from holidaydatetime  where hsrp_stateid ='" + HSRPStateID + "' and blockdate between  dateadd(day,-4,GETDATE()) and getdate()";
                    //int j = Utils.getScalarCount(SQLString,CnnString);

                    //i = i - j;
                    //int k=Convert.ToInt32(i);

                    SQLString = "select hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,HSRPRecord_AuthorizationNo from hsrprecords where hsrp_stateid ='" + HSRPStateID + "' and PdfFileName='" + dropdownDuplicateFIle.SelectedItem.ToString() + "' and orderstatus ='New Order' and isnull(aptgvehrecdate,GETDATE())>dateadd(day, -4,GETDATE())  order by VehicleClass,VehicleType,VehicleRegNo";
                    dt = Utils.GetDataTable(SQLString, CnnString);
                }
                else
                {
                    SQLString = "select hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,HSRPRecord_AuthorizationNo from hsrprecords where hsrp_stateid ='" + HSRPStateID + "' and PdfFileName='" + dropdownDuplicateFIle.SelectedItem.ToString() + "' and orderstatus ='New Order'  order by VehicleClass,VehicleType,VehicleRegNo";
                    dt = Utils.GetDataTable(SQLString, CnnString);
                }
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    btnSave.Visible = true;
                    btnSave.Enabled = true;

                }
                else
                {
                    btnSave.Visible = false;
                    lblErrMsg.Text = "Record Not Found";
                    GridView1.DataSource = null;
                    GridView1.DataBind();

                }

            }
            else
            {

                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select File.";
                return;
            }
        }
        StringBuilder sb = new StringBuilder();
        StringBuilder sb1 = new StringBuilder();
        CheckBox chk;
        protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk1 = GridView1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
            if (chk1.Checked == true)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = true;
                }
            }
            else if (chk1.Checked == false)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = false;
                }
            }

        }



        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;

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
            labelOrderStatus.Visible = true;
            dropdownDuplicateFIle.Visible = true;

            string type = "1";
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
            string AuthorizationDate = From + " 00:00:00";
            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            string ToDate = From1 + " 23:59:59";

            if (HSRPStateID == "9" || HSRPStateID == "11")
            {
                SQLString = "select  Distinct pdffilename   from hsrprecords  where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and rtolocationID='" + dropDownListClient.SelectedValue + "' and  pdffilename is not null and erpassigndate between '" + AuthorizationDate + "' and  '" + ToDate + "' and orderstatus='New Order' group by  pdffilename, convert(varchar,pdfdownloaddate,111)";
                Utils.PopulateDropDownList(dropdownDuplicateFIle, SQLString.ToString(), CnnString, "--Select File Name--");
                GridView1.DataSource = null;
                GridView1.DataBind();
                btnSave.Visible = false;
                btnSave.Enabled = false;

            }

            else
            {
                SQLString = "select  Distinct pdffilename   from hsrprecords  where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and rtolocationID='" + dropDownListClient.SelectedValue + "' and  pdffilename is not null and Hsrprecord_creationdate between '" + AuthorizationDate + "' and  '" + ToDate + "' and orderstatus='New Order' group by  pdffilename, convert(varchar,pdfdownloaddate,111)";
                Utils.PopulateDropDownList(dropdownDuplicateFIle, SQLString.ToString(), CnnString, "--Select File Name--");
                GridView1.DataSource = null;
                GridView1.DataBind();
                btnSave.Visible = false;
                btnSave.Enabled = false;
            }

        }
        DataTable dt = new DataTable();
        protected void dropdownDuplicateFIle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowGrid();
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowGrid();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LblErrorMessage.Text = "";
                LblMessage.Text = "";
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                    // tmpbillno = Session["UID"].ToString()+ System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() ;
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    if (chk.Checked == true)
                    {
                        Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;
                        TextBox RearLaserCode = GridView1.Rows[i].Cells[2].FindControl("txtFLaserCode") as TextBox;
                        TextBox FLasercode = GridView1.Rows[i].Cells[3].FindControl("txtRLaserCode") as TextBox;
                        Label id = GridView1.Rows[i].Cells[4].FindControl("id") as Label;
                        //  labtext = GridView1.Rows[i].Cells[0].Text;
                        sb.Append("update hsrprecords set hsrp_Front_lasercode='" + RearLaserCode.Text + "' ,hsrp_rear_lasercode='" + FLasercode.Text + "',orderstatus='Embossing Done',[OrderEmbossingDate]=getdate() , embossinguserid='" + strUserID + "' where HSRPRecordID='" + id.Text + "'" + Environment.NewLine);
                    }
                }
                int j = Utils.ExecNonQuery(sb.ToString(), CnnString);
                //Synct to AP Server 
                if (DropDownListStateName.SelectedValue.ToString() == "9")
                {

                    HSRP.APWebrefrence.HSRPAuthorizationService objAPService = new HSRP.APWebrefrence.HSRPAuthorizationService();
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                        if (chk.Checked == true)
                        {
                            Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;
                            TextBox RearLaserCode = GridView1.Rows[i].Cells[2].FindControl("txtFLaserCode") as TextBox;
                            TextBox FLasercode = GridView1.Rows[i].Cells[3].FindControl("txtRLaserCode") as TextBox;
                            Label id = GridView1.Rows[i].Cells[4].FindControl("id") as Label;
                            Label lblAuthNo = GridView1.Rows[i].Cells[5].FindControl("AuthNo") as Label;
                            string json = "{\"frontLaserCode\":\"" + FLasercode + "\",\"rearLaserCode\":\"" + RearLaserCode + "\",\"embossingDate\":\"" + System.DateTime.Now.ToString("dd/MM/yyyy") + "\",\"authRefNo\":\"" + lblAuthNo + "\"}";
                            List<Response> result = UpdateLaserCodes(json, Utils.Login().ToString());
                            string status = result[0].status.ToString();
                            string message = result[0].message.ToString();
                            sb1.Append("update hsrprecords set APwebservicerespEmbMsg='" + message + "', APwebservicerespEmb='" + status + "',APwebservicerespEmbdate=Getdate(),embossinguserid='" + strUserID + "' where HSRPRecordID='" + id.Text + "' and vehicleregno='" + lblVehicleRegNo.Text + "'" + Environment.NewLine);

                        }

                    }
                    Utils.ExecNonQuery(sb1.ToString(), CnnString);
                }
                if (DropDownListStateName.SelectedValue.ToString() == "11")
                {

                    HSRP.TGWebrefrence.HSRPAuthorizationService objTGService = new HSRP.TGWebrefrence.HSRPAuthorizationService();
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                        if (chk.Checked == true)
                        {
                            Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;
                            TextBox RearLaserCode = GridView1.Rows[i].Cells[2].FindControl("txtFLaserCode") as TextBox;
                            TextBox FLasercode = GridView1.Rows[i].Cells[3].FindControl("txtRLaserCode") as TextBox;
                            Label id = GridView1.Rows[i].Cells[4].FindControl("id") as Label;
                            Label lblAuthNo = GridView1.Rows[i].Cells[5].FindControl("AuthNo") as Label;

                            string responsecodeTG = objTGService.UpdateHSRPLaserCodes(lblAuthNo.Text, FLasercode.Text, RearLaserCode.Text, DateTime.Now.ToString("dd/MM/yyyy"));
                            sb1.Append("update hsrprecords set APwebservicerespEmb='" + responsecodeTG + "',APwebservicerespEmbdate=Getdate(), embossinguserid='" + strUserID + "' where HSRPRecordID='" + id.Text + "' and vehicleregno='" + lblVehicleRegNo.Text + "'" + Environment.NewLine);

                        }
                    }
                    Utils.ExecNonQuery(sb1.ToString(), CnnString);
                }
                if (j > 0)
                {

                    LblMessage.Text = "Updated Records Sucessfull";
                    btnGO_Click(sender, e);
                    btnSave.Enabled = false;
                }
                else
                {
                    LblErrorMessage.Text = "Updated Records Not Sucessfull";
                    btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                LblErrorMessage.Text = "Data Not Uploaded Contact Administration with screen shot Error :" + ex.Message.ToString();
            }
        }

        public List<Response> UpdateLaserCodes(string locationJSON, string token)
        {
            //string locationJSON = "{\"frontLaserCode\":\"AA211026072\",\"rearLaserCode\":\"AA211026073\",\"embossingDate\":\"08/09/2016\",\"authRefNo\":\"HSRP101\"}";
            string URL = System.Configuration.ConfigurationManager.AppSettings["APRTDUpdateLaserCodesURL"].ToString();
            var client = new RestClient(URL);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", token);
            request.AddParameter("Application/Json", locationJSON, ParameterType.RequestBody);
            var response2 = client.Execute<Response>(request);
            //return response2.Data.message.ToString();
            List<Response> list = new List<Response>();
            list.Add(new Response() { status = response2.Data.status.ToString(), message = response2.Data.message.ToString() });
            //list[0].status = response2.Data.status;
            //list[0].message = response2.Data.message;
            return list;
        }


    }
}