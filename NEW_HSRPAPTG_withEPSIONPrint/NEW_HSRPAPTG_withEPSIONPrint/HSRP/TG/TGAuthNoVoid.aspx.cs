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
using System.Collections.Specialized;
namespace HSRP.TG
{
    public partial class TGAuthNoVoid : System.Web.UI.Page
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
        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        string strFrmDateString = string.Empty;
        string strToDateString = string.Empty;

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
                    try
                    {
                        if (UserType == "0")
                        {
                            lblAuthno.Visible = true;
                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            lblAuthno.Enabled = false;
                        }
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        private void ShowGrid(string AuthonNo)
        {
            if (AuthonNo == "")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Enter Auth No..";
                return;
            }
            else
            {
                SQLString = "Select ROW_NUMBER() Over (Order by HSRPRecord_CreationDate) As SNo,HSRPRecord_CreationDate,OnlinePaymentID,Roundoff_netAmount,ownername,dealerid,vehicleclass,vehicletype,OnlinePaymentStatus,OnlinePaymentTrackingNo,BtnInitiatedFrom,paymentGateway,hsrprecord_Authorizationno,ID  from tgonlinepayment A where hsrprecord_Authorizationno='" + AuthonNo + "'";
                dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count > 0)
                {

                    btnSyncWithGovt.Visible = true;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    btnSyncWithGovt.Visible = false;
                    lblErrMsg.Text = "Record not found.";
                    GridView1.DataSource = null;
                    GridView1.DataBind();

                }

            }
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

        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                ShowGrid(txtAuthNo.Text);
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }
        }
        DataTable dt = new DataTable();
        Label lblOnlinePaymentID;
        Label lblRoundoff_netAmount;
        string strId;
        public void Showgridfun()
        {
            try
            {
                //Opens the document:
                string strHsrpRecordId = string.Empty;
                
                #region Set ChallanNo
                string[] strArray;
                try
                {

                    if (GridView1.Rows.Count == 0)
                    {
                        lblErrMsg.Text = "No Record Found.";
                        return;

                    }
                    // Validate checked recirds
                    int ChkBoxCount = 0;
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                        if (chk.Checked == true)
                        {
                            ChkBoxCount = ChkBoxCount + 1;
                        }
                    }
                    if (ChkBoxCount == 0)
                    {
                        lblErrMsg.Text = "Please select atleast 1 record.";
                        return;
                    }
                }
                catch (Exception ex)
                {
                    lblErrMsg.Text = ex.Message.ToString();
                    return;
                }

                #endregion
                int iChkCount = 0;
                StringBuilder sbx = new StringBuilder();
                
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                    if (chk.Checked == true)
                    {
                        iChkCount = iChkCount + 1;

                        strId = GridView1.DataKeys[i]["ID"].ToString();
                        lblOnlinePaymentID = GridView1.Rows[i].Cells[3].FindControl("lblOnlinePaymentID") as Label;
                        lblRoundoff_netAmount = GridView1.Rows[i].Cells[4].FindControl("lblRoundoff_netAmount") as Label;
                        if (strHsrpRecordId == "")
                        {
                            strHsrpRecordId = strId;
                        }
                        else
                        {
                            strHsrpRecordId = strHsrpRecordId + "," + strId;
                        }
                        sbx.Append("update tgonlinepayment Set OnlinePaymentStatus='V' where HSRP_StateID='"+HSRPStateID+"' and ID='" + strId + "'");
                    }
                }
                if (iChkCount > 0)
                {
                    int i = Utils.ExecNonQuery(sbx.ToString(), CnnString);
                    if (i > 0)
                    {
                        LblMessage.Text = "Record updated successfully";
                        ShowGrid(txtAuthNo.Text);
                    }
                    else
                    {
                        lblErrMsg.Text = "Record not updated";
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }
        }

        protected void btnChalan_Click(object sender, EventArgs e)
        {
            Showgridfun();
        }
         
    }
}
