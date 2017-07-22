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
    public partial class EditBankSlipno : System.Web.UI.Page
    {
       // Utils bl = new Utils();
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
                SQLString = "select distinct dm.DealerID,(CONVERT(varchar,dm.DealerID) +' '+','+ dm.dealername) as name from dealermaster dm join BankTransaction bt on bt.dealerid=dm.dealerid where bt.stateid='" + HSRPStateID + "' order by dm.DealerID";
                DataTable dt = new DataTable();               
                dt = Utils.GetDataTable(SQLString.ToString(), CnnString);
                dropDownRtoLocation.DataSource = dt;
                dropDownRtoLocation.DataTextField = "name";
                dropDownRtoLocation.DataValueField = "DealerID";
                dropDownRtoLocation.DataBind();
                dropDownRtoLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "--Select--"));           
        }
      
        #endregion

        string dealerid = string.Empty;
        public void ShowGrid()
        {
            
            try
            {
                if (dropDownRtoLocation.SelectedValue.ToString() != "--Select--")
                {
                    dealerid = dropDownRtoLocation.SelectedValue.ToString();
                }
               
                string SQLString = "SELECT [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],(select Userfirstname from users where dealerid='" + dealerid + "') as [DepositBy],r.RtoLocationName as DepositLocation,(Select dealername+','+' '+ convert(varchar,dealerid) from dealermaster where dealerid='" + dealerid + "') as Dealer,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate,[BankSlipNo],[Remarks],bm.[AccountNo],bt.ApprovedStatus,bt.ApprovedBy,convert(varchar(10), bt.ApprovedDate, 103) as 'ApprovedDate', bt.ChqrecStatus,bt.ChqrecBy,convert(varchar(10), bt.ChqrecDateTime, 103) as 'ChqrecDateTime',convert(varchar(10), bt.AmtClearDate, 103) as 'AmtClearDate',RejectedBy,convert(varchar(10), bt.RejectDate, 103) as 'RejectDate' FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.rtolocation where StateID='" + HSRPStateID + "' and dealerid='" + dealerid + "' order by [TransactionID] desc";
                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                if (dt.Rows.Count > 0)
                {
                    gvEmpDesignation.DataSource = dt;
                    gvEmpDesignation.DataBind();
                    gvEmpDesignation.Visible = true;
                    lblErrMsg.Text = "";

                }
                else
                {
                   // txtSearchByID.Text = "";
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Record not found";
                    gvEmpDesignation.DataSource = "";
                    gvEmpDesignation.DataBind();
                    gvEmpDesignation.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
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
            ShowGrid();         
        }
        DataTable dt = new DataTable();
        
        protected void gvEmpDesignation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEmpDesignation.EditIndex = e.NewEditIndex;
            this.ShowGrid();
        }

        protected void gvEmpDesignation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEmpDesignation.EditIndex = -1;
            this.ShowGrid();
        }

        protected void gvEmpDesignation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            dealerid = dropDownRtoLocation.SelectedValue.ToString();
            Label lblTransactionEditid = (Label)gvEmpDesignation.Rows[e.RowIndex].FindControl("lblTransactionEditid");
            TextBox txtEditBankSlipNo = (TextBox)gvEmpDesignation.Rows[e.RowIndex].FindControl("txtEditBankSlipNo");

            SqlQuery = "update BankTransaction set BankSlipNo ='" + txtEditBankSlipNo.Text + "' where TransactionID='" + Convert.ToInt32(lblTransactionEditid.Text.Trim()) + "' and DealerID='" + dealerid + "'";                       
            int i = Utils.ExecNonQuery(SqlQuery, CnnString);
            
            if (i > 0)
            {
                lblErrMsg.Text = "";
                LblMessage.Text = "Record Update Successfully.";
                ShowGrid();
               
            }
            else
            {
                LblMessage.Text = "";
                lblErrMsg.Text = "Record not Update";
            }
            gvEmpDesignation.EditIndex = -1;
            this.ShowGrid();
        }

        protected void dropDownRtoLocation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}


