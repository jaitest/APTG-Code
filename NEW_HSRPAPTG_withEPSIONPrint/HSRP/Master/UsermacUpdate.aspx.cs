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
    public partial class UsermacUpdate : System.Web.UI.Page
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
                SQLString = "select UserLoginName,userID from users Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' and withoutMAC='N' Order by UserLoginName";
                Utils.PopulateDropDownList(dropDownRtoLocation, SQLString.ToString(), CnnString, "--Select RTO Name--");

               // dataLabellbl.Visible = false;
                //TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                SQLString = "select UserLoginName,userID from users Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' and withoutMAC='N'  Order by UserLoginName";
                //SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID and a.activestatus='Y' where UserRTOLocationMapping.UserID='" + UserID + "' order by a.RTOLocationName";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);

                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownRtoLocation.Visible = true;

                dropDownRtoLocation.DataSource = dss;
                dropDownRtoLocation.DataBind();
                //dataLabellbl.Visible = true;

               
            }
        }
      
        #endregion


     

        private void ShowGrid()
        {
            string SQLString = "select ROW_NUMBER() OVER(ORDER BY userID ASC)as SNo,userID,UserFirstName,UserLastName,UserLoginName,State,MobileNo,withoutmac from users where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and activestatus='Y' and userid='" + dropDownRtoLocation.SelectedValue.ToString() + "'";
               
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
            ShowGrid();
         
        }
        DataTable dt = new DataTable();
        protected void dropdownDuplicateFIle_SelectedIndexChanged(object sender, EventArgs e)
        {
           // dt = Utils.GetDataTable(SQLString, CnnString);
            ShowGrid();
        }

        protected void gvEmpDesignation_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

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

            Label lblEditid = (Label)gvEmpDesignation.Rows[e.RowIndex].FindControl("lblEditid");
            TextBox txtEditlocationName = (TextBox)gvEmpDesignation.Rows[e.RowIndex].FindControl("txtEditlocationName");

            SqlQuery = "update users set withoutmac ='" + txtEditlocationName.Text + "' where userid='" + dropDownRtoLocation.SelectedValue.ToString() + "' and activestatus='Y'";
            
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


