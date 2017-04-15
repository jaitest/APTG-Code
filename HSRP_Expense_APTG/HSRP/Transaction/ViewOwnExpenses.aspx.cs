using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AjaxControlToolkit;

namespace HSRP.Transaction
{
    public partial class ViewOwnExpenses : System.Web.UI.Page
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


        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        DataProvider.BAL bl = new DataProvider.BAL();
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
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            buildGrid();
                        }
                        else
                        {
                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            buildGrid();
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
                //Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                //DropDownListStateName.DataSource = dts;
                //DropDownListStateName.DataBind();
            }
        }
        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {

                //int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                ////SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                //Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");


            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                //labelOrganization.Visible = true;
                //DropDownListStateName.Visible = true;
                //labelClient.Visible = true;
                //dropDownListClient.Visible = true;

                //dropDownListClient.DataSource = dss;
                //dropDownListClient.DataBind();


                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();

                    }


                }
            }
        }
        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

            //if (DropDownListStateName.SelectedItem.Text != "--Select Client--")
            //{
            //    ShowGrid();
            //}
            //else
            //{
            //    //Grid1.Items.Clear();
            //    lblErrMsg.Text = String.Empty;
            //    lblErrMsg.Text = "Please Select Client.";
            //    return;
            //}
        }
        #endregion

        #region Grid
        public void buildGrid()
        {
            try
            {
                ClearGrid();
                SQLString = "SELECT CONVERT (varchar(20),BillDate,103) as BillDate,ExpenseSaveID,(select ExpenseName from ExpenseMaster where ExpenceId=ExpenseId) as ExpenseName,BillNo,BillAmount,Remarks,ExpenseStatus,VerifiedAmount,VerifiedRemarks,CONVERT (varchar(20),VerifiedDate,103) as VerifiedDate,(select (userfirstname +' '+ userlastname) as name  from users where userid=VerifiedBy) as VerifiedBy,(select (userfirstname +' '+ userlastname) as name from users where userid=employeemaster.Userid) as claimedby,AccExpenseStatus,VerifiedRemarksByAcc,CONVERT (varchar(20),VerifiedByAccDate,103) as VerifiedByAccDate,(select (userfirstname +' '+ userlastname) as name  from users where userid=VerifiedByAcc) as VerifiedByAcc,VerifiedAmountByAcc,CEOExpenseStatus,VerifiedRemarksByCEO,CONVERT (varchar(20),VerifiedByDateCEO,103) as VerifiedByDateCEO,VerifiedAmountByCEO FROM ExpenseSave INNER JOIN  employeemaster  ON ExpenseSave.claimedbyid = employeemaster.id where employeemaster.userid='" + strUserID + "' order by ExpenseSaveID desc";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    ClearGrid();
                }

            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
        #endregion
        private void ClearGrid()
        {
            lblErrMsg.Text = String.Empty;
            return;
        }

        protected void ButtonGo_Click(object sender, EventArgs e)
        {
            buildGrid();

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        
    }
}