﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
using System.Globalization;


namespace HSRP.Report
{
    public partial class PushDataInErp : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string trnasportname, pp;
        string transtr, statename1;
        BaseFont basefont;
        string fontpath;
        string strComplaintID = string.Empty;
        string strSql = string.Empty;

        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        DataProvider.BAL bl = new DataProvider.BAL();
        string StickerManditory = string.Empty;

        string SubmitId = string.Empty;
        string QrySubmitID = string.Empty;

        string State_ID = string.Empty;
        string RTO_ID = string.Empty;
        string HSRPStateIDEdit = string.Empty;
        string RTOLocationIDEdit = string.Empty;
        string fromdate = string.Empty;
        string ToDate = string.Empty;
        string strSqlGo = string.Empty;
        string strsqlgonew = string.Empty;
        DataTable dtcount = new DataTable();
        DataTable dtshow = new DataTable();


        DataTable dt = new DataTable();


        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            btnUpdate.Visible = false;
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (Session["UserHSRPStateID"].ToString() == "6")
                {
                    // ButImpData.Visible = true;
                }
                else
                {
                    // ButImpData.Visible = false;
                }

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
                            FilldropDownListOrganization();

                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            FilldropDownListOrganization();

                        }
                        string strDate = System.DateTime.Now.ToString("hh:mm tt");
                        string strfive = "05:00 AM";
                        if (DateTime.Parse(strDate) > DateTime.Parse(strfive))
                        {
                            Button1.Visible = false;
                        }
                        else
                        {
                            Button1.Visible = true;
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

        private void FilldropDownListOrganization()
        {
            try
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
            catch (Exception ex)
            {

                lblErrMsg.Text = ex.Message.ToString();
            }
        }


        #endregion





        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-1);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-1);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void dropDownListClient_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }



        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                lblSucessMsg.Text = "";
                if (DropDownListStateName.Text == ("--Select State--"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select State..');", true);
                    return;

                }
                else
                {

                    fromdate = OrderDate.SelectedDate.ToString("yyyy/MM/dd 00:00:00");
                    ToDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd 23:59:59");
                    State_ID = DropDownListStateName.SelectedValue.ToString();

                    // strSqlGo = "select rtolocationname,count(*) from hsrprecords a,rtolocation b where  a.rtolocationid=b.rtolocationid and a.HSRP_StateID=4 and hsrprecord_creationdate between '01-Aug-2014 00:00:00' and '16-Nov-2014 23:59:59' and NAVSalesOrderNo is null and NAVProdOrderNo is Null  and orderstatus='New Order' group by RTOLocationName order by 1";

                    strSqlGo = "select rtolocationname,count(*) from hsrprecords a,rtolocation b where  a.rtolocationid=b.rtolocationid and isnull(vehicleregno,'')!='' and a.HSRP_StateID='" + State_ID + "' and aptgvehrecdate between '" + fromdate + "' and '" + ToDate + "' and NAVSalesOrderNo is null and NAVProdOrderNo is Null  and orderstatus='New Order' group by RTOLocationName order by 1";
                    dtcount = Utils.GetDataTable(strSqlGo, CnnString);
                    if (dtcount.Rows.Count > 0)
                    {
                        grdcount.DataSource = dtcount;
                        grdcount.DataBind();
                    }

                    /*For Exception of recordshowing less or access
                    select distinct erpassigndate from hsrprecords where hsrprecord_creationdate between '01-Dec-2014 00:00:00' and '01-Dec-2014 23:59:59' 
                    and HSRP_StateID=4 and erpassigndate is not null and HSRP_Front_LaserCode=' '*/

                    strsqlgonew = "select hsrpstatename,count(vehicleregno),count(navsalesorderno),count(vehicleregno)-count(navsalesorderno),count(vehicleregno)-count(erpassigndate),count(hsrp_front_lasercode),count(hsrp_rear_lasercode) from hsrprecords a,hsrpstate b where a.hsrp_stateid=b.hsrp_stateid and isnull(vehicleregno,'')!='' and aptgvehrecdate between '" + fromdate + "' and '" + ToDate + "' group by HSRPStateName";
                    dtshow = Utils.GetDataTable(strsqlgonew, CnnString);
                    if (dtshow.Rows.Count > 0)
                    {
                        grdpending.DataSource = dtshow;
                        grdpending.DataBind();
                        //btnUpdate.Visible = true;
                    }
                }

            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message.ToString();
            }
        }



        protected void btnPush_Click(object sender, EventArgs e)
        {
            lblSucessMsg.Text = "";
            try
            {
                fromdate = OrderDate.SelectedDate.ToString("yyyy/MM/dd 00:00:00");
                ToDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd 23:59:59");
                State_ID = DropDownListStateName.SelectedValue.ToString();
                if (DropDownListStateName.Text == ("--Select State--"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select State..');", true);

                }
                else
                {
                    if (txtvehicleno.Text == "")
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Text Box can not be null');", true);
                    }
                    else
                    {
                        string UID = Session["UID"].ToString();
                        string txtlog = string.Empty;

                        string[] TextBoxValue = txtvehicleno.Text.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < TextBoxValue.Length; j++)
                        {
                            try
                            {
                                SQLString = "update hsrprecords set sendtoerp=Null, remarks='ERP' ,NAVSalesOrderNo=Null,NAVProdOrderNo=Null where HSRP_StateID='" + State_ID + "' and hsrprecord_creationdate between '" + fromdate + "' and '" + ToDate + "' and VehicleRegNo='" + TextBoxValue[j] + "' and NAVSalesOrderNo is null and orderstatus='New Order' ";
                                int i = Utils.ExecNonQuery(SQLString, CnnString);
                                if (i > 0)
                                {
                                    lblSucessMsg.Text = "Record Update Sucessfully ";
                                    //txtlog = "Record Update Sucessfully";
                                }
                                else
                                {
                                    lblErrMsg.Text = "Record Not Updated";
                                   // txtlog = "Record Not Updated";
                                }
                               // AddLog(TextBoxValue[j]);
                                //AddLog(txtlog);
                            }
                            catch (Exception ex)
                            {
                                AddLog(ex.Message.ToString());
                            }      

                        }
                    }

                }
            }
            catch (Exception ex)
            {
               
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Record Not Updated');", true);
                lblErrMsg.Text = ex.Message.ToString();
            }

        }

        static void AddLog(string logText)
        {     
            string pathx = "C:\\LaserFolder\\APTGRecordPush-" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + ".log";
            using (StreamWriter sw = File.AppendText(pathx))
            {
                sw.WriteLine("-------------------" + System.DateTime.Now.ToString() + "--------------------");
                sw.WriteLine(logText);
                sw.WriteLine("-----------------------------------------------------------------------------");
                sw.Close();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string strDate = System.DateTime.Now.ToString("hh:mm tt");
                string strfive = "05:00 AM";
                if (DateTime.Parse(strDate) < DateTime.Parse(strfive))
                {
                    int i = Utils.ExecNonQuery("update hsrprecords set erpassigndate=dateadd(dd,-1,erpassigndate),internal_remarks='Manually Updated ERP Assign Date assigned after 12:00 AM' where erpassigndate>=convert(varchar(15),getdate(),102 ) and CONVERT(time,erpassigndate,108) between '00:00:00' and '5:00:00'", CnnString);
                    if (i > 0)
                    {
                        lblSucessMsg.Text = "Record change sucessfully";
                    }
                    else
                    {
                        lblSucessMsg.Text = "Record not change";
                    }
                }
                else 
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Manually Updated ERP Assign Date assigned after 12:00 AM to 5:00 AM";
                    return;
                }
                //string strSql = string.Empty;
                //string strhsrprecordid = string.Empty;
                //string strvehicleregno = string.Empty;
                //string rtolocationid = string.Empty;
                //string navembid = string.Empty;
                //string strsql = "select vehicleregno,HsrpRecordID,h.RTOLocationID,r.NavEmbID from  hsrprecords as h inner join rtolocation as r on r.rtolocationid=h.rtolocationid where h.hsrp_stateid=4 and hsrprecord_creationdate between '2014-12-04 00:00:00'and '2014-12-04 23:59:59' and erpassigndate is null";
                //DataTable dtresult = Utils.GetDataTable(strsql, CnnString);
                //if (dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtresult.Rows.Count; i++)
                //    {
                //        strhsrprecordid = dtresult.Rows[i]["HSRPRecordID"].ToString();
                //        strvehicleregno = dtresult.Rows[i]["VehicleRegNo"].ToString();
                //        rtolocationid = dtresult.Rows[i]["RTOLocationID"].ToString();
                //        navembid = dtresult.Rows[i]["NavEmbID"].ToString();
                //        strSql = "insert into ERP_PFA_Region (HSRPRecordID,VehicleRegNo,RTOLocation,NavEmbID,Remarks) values ('" + strhsrprecordid + "','" + strvehicleregno + "','" + rtolocationid + "','" + navembid + "','" + txtRemarks.Text + "')";
                //        Utils.ExecNonQuery(strSql, CnnString);
                //    }
                //    lblSucessMsg.Text = "Record Save Sucessfully";
                //}
            }
            catch (Exception ex)
            {

                lblErrMsg.Text = ex.Message.ToString();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                lblSucessMsg.Text = "";
                fromdate = OrderDate.SelectedDate.ToString("yyyy/MM/dd 00:00:00");
                ToDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd 23:59:59");

                if (DropDownListStateName.Text == ("--Select State--"))
                {
                    lblErrMsg.Text = "Please select state.";
                    return;
                }
                else
                {
                    string UID = Session["UID"].ToString();
                    State_ID = DropDownListStateName.SelectedValue.ToString();
                    SQLString = "update hsrprecords set sendtoerp=Null, remarks='ERP' ,NAVSalesOrderNo=Null,NAVProdOrderNo=Null where HSRP_StateID='" + State_ID + "' and hsrprecord_creationdate between '" + fromdate + "' and '" + ToDate + "' and NAVSalesOrderNo is null and orderstatus='New Order' ";
                    int i = Utils.ExecNonQuery(SQLString, CnnString);
                    if (i > 0)
                    {
                        lblSucessMsg.Text = "Record Update Sucessfully";
                        return;
                    }
                    else
                    {
                        lblErrMsg.Text = "Record Not Updated";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Record Not Updated exception :" + ex.Message.ToString();
            }

        }

        protected void btnhsrperp_Click(object sender, EventArgs e)
        {
            try
            {
                string UID = Session["UID"].ToString();
                State_ID = DropDownListStateName.SelectedValue.ToString();
                SQLString = "update hsrprecords set sendtoerp=Null, remarks='ERP' ,NAVSalesOrderNo=Null,NAVProdOrderNo=Null where hsrprecordid in(select HSRPRecordID from hsrprecords where HSRP_StateID='" + State_ID + "' and isnull(VehicleRegNo,'')!='' and erpassigndate is null and isnull(hsrp_front_lasercode,'')=''and OrderStatus='New Order' and hsrprecord_creationdate between '" + fromdate + "' and '" + ToDate + "'";
                int i = Utils.ExecNonQuery(SQLString, CnnString);
                if (i > 0)
                {
                    lblSucessMsg.Text = "Record Update Sucessfully";
                }
                else
                {
                    lblErrMsg.Text = "Record Not Updated";
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = " Record Not Updated exception :" + ex.Message.ToString();
            }
        }

        protected void btnupdaterecord_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "update hsrprecords set aptgvehrecdate=orderdate where isnull(aptgvehrecdate,'')='' and HSRP_StateID='" + HSRPStateID + "' and HSRPRecord_CreationDate>'01-jan-2016 00:00:00'and isnull(vehicleregno,'')!=''";
                int i = Utils.ExecNonQuery(sql, CnnString);
                if (i > 0)
                {
                    lblSucessMsg.Text = "Record Update Sucessfully";

                }
                else
                {
                    lblErrMsg.Text = "Record Not Updated.";

                }
            }
            catch (Exception ex)
            {

                lblErrMsg.Text = ex.Message.ToString();

            }
        }

        protected void btndownload_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "select VehicleRegNo,orderdate,aptgvehrecdate from hsrprecords where isnull(aptgvehrecdate,'')='' and HSRP_StateID='" + HSRPStateID + "' and HSRPRecord_CreationDate>'01-jan-2016 00:00:00'and isnull(vehicleregno,'')!=''";
                DataTable dt = Utils.GetDataTable(sql, CnnString);
                if (dt.Rows.Count > 0)
                {
                    grdshow.DataSource = dt;
                    grdshow.DataBind();
                }
            }
            catch (Exception ex)
            {

                lblErrMsg.Text = ex.Message.ToString();
            }
        }



    }
}