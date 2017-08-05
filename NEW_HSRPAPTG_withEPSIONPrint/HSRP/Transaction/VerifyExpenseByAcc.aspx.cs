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
    public partial class VerifyExpenseByAcc : System.Web.UI.Page
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
        string ExpenceID = string.Empty;

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
                    InitialSetting();
                    try
                    {
                        
                        if (UserType == "0")
                        {
                            //labelOrganization.Visible = true;
                            //DropDownListStateName.Visible = true;
                            //DropDownListStateName.Enabled = true;
                            //labelClient.Visible = true;
                            //dropDownListClient.Visible = true;
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

        private void InitialSetting()
        {

            //string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            //string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            //HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            //CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-3.00);
            //OrderDate.MaxDate = DateTime.Parse(MaxDate);
            //CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
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

        //private void ShowGrid()
        //{

        //    if (String.IsNullOrEmpty(dropDownListClient.SelectedValue) || dropDownListClient.SelectedValue.Equals("--Select Client--"))
        //    {
        //        //Grid1.Items.Clear();
        //        lblErrMsg.Text = String.Empty;
        //        lblErrMsg.Text = "Please Select Client.";
        //        return;
        //    }
        //    buildGrid();
        //}


        #region Grid
        public void buildGrid()
        {
            try
            {
                ClearGrid();
                SQLString = "SELECT CONVERT (varchar(20),BillDate,103) as BillDate,ExpenseSaveID,(select ExpenseName from ExpenseMaster where ExpenceId=ExpenseId) as ExpenseName,BillNo,BillAmount,Remarks,ExpenseStatus,VerifiedAmount,VerifiedRemarks,CONVERT (varchar(20),VerifiedDate,103) as VerifiedDate,(select (userfirstname +' '+ userlastname) as name  from users where userid=VerifiedBy) as VerifiedBy,(select (userfirstname +' '+ userlastname) as name from users where userid=employeemaster.Userid) as claimedby , VerifiedByCEO=(CASE WHEN VerifiedByCEO IS NULL THEN 'N' ELSE 'Y' end),VerifiedByAcc=(CASE WHEN VerifiedByAcc IS NULL THEN 'N' ELSE 'Y' end),CEOExpenseStatus,AccExpenseStatus FROM ExpenseSave INNER JOIN  employeemaster  ON ExpenseSave.claimedbyid = employeemaster.id where Expensestatus='Approve' and isnull(AccExpenseStatus,'')='' and CEOExpenseStatus!='Rejected' order by ExpenseSaveID desc";
                //SQLString = "SELECT CONVERT (varchar(20),BillDate,103) as BillDate,ExpenseSaveID,(select ExpenseName from ExpenseMaster where ExpenceId=ExpenseId) as ExpenseName,BillNo,BillAmount,Remarks,ExpenseStatus,VerifiedAmount,VerifiedRemarks,CONVERT (varchar(20),VerifiedDate,103) as VerifiedDate,(select (userfirstname +' '+ userlastname) as name  from users where userid=VerifiedBy) as VerifiedBy,(select (userfirstname +' '+ userlastname) as name from users where userid=employeemaster.Userid) as claimedby , VerifiedByCEO=(CASE WHEN VerifiedByCEO IS NULL THEN 'N' ELSE 'Y' end),CEOExpenseStatus,AccExpenseStatus FROM ExpenseSave INNER JOIN  employeemaster  ON ExpenseSave.claimedbyid = employeemaster.id where Expensestatus='Approve' and isnull(AccExpenseStatus,'')='' and CEOExpenseStatus!='Rejected' order by ExpenseSaveID desc";
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

        //protected void Linkbutton1_Click(object sender, EventArgs e)
        //{
        //    string ExpenceID = string.Empty;
        //    LinkButton btn = sender as LinkButton;
        //    GridViewRow row = btn.NamingContainer as GridViewRow;
        //    ExpenceID = GridView1.DataKeys[row.RowIndex].Values[0].ToString();
        //    //strExpenseSaveID = GridView1.DataKeys[row.RowIndex].Values[0].ToString();
        //    ClientScript.RegisterStartupScript(GetType(), "showModalScript", "VerifyAmount('" + ExpenceID + "');", true);
        //}

        protected void ButtonGo_Click(object sender, EventArgs e)
        {
            buildGrid();

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (GridViewRow gr in GridView1.Rows)
            {
                LinkButton lnkTransfer;
                Label lblapproved;
               
                string cell_2_Value = GridView1.Rows[gr.RowIndex].Cells[10].Text;
                string cell_3_Value = GridView1.Rows[gr.RowIndex].Cells[11].Text;
                string cell_4_Value = GridView1.Rows[gr.RowIndex].Cells[12].Text;


                if (cell_2_Value == "Y")
                {
                    if (gr.RowType == DataControlRowType.DataRow)
                    {
                        lnkTransfer = gr.FindControl("lnkTransfer") as LinkButton;
                        lnkTransfer.Visible = false;
                        //lnkTransfer = gr.FindControl("lnkReject") as LinkButton;
                        //lnkTransfer.Visible = false;
                        //lnkTransfer = gr.FindControl("lnkApproval") as LinkButton;
                        //lnkTransfer.Visible = false;
                        //lnkTransfer = gr.FindControl("lnkBypass") as LinkButton;
                        //lnkTransfer.Visible = false;
                    }
                }
                if (cell_3_Value == "N")
                {
                    if (gr.RowType == DataControlRowType.DataRow)
                    {
                        lnkTransfer = gr.FindControl("lnkReject") as LinkButton;
                        lnkTransfer.Visible = false;
                        lnkTransfer = gr.FindControl("lnkTransfer") as LinkButton;
                        lnkTransfer.Visible = false;
                        lnkTransfer = gr.FindControl("lnkBypass") as LinkButton;
                        lnkTransfer.Visible = false;
                        lnkTransfer = gr.FindControl("lnkApproval") as LinkButton;
                        lnkTransfer.Visible = false;
                    }
                }
                if (cell_4_Value == "Y")
                {
                    if (gr.RowType == DataControlRowType.DataRow)
                    {
                        lnkTransfer = gr.FindControl("lnkApproval") as LinkButton;
                        lnkTransfer.Visible = false;
                        lnkTransfer = gr.FindControl("lnkReject") as LinkButton;
                        lnkTransfer.Visible = false;
                        lnkTransfer = gr.FindControl("lnkTransfer") as LinkButton;
                        lnkTransfer.Visible = false;
                        lnkTransfer = gr.FindControl("lnkBypass") as LinkButton;
                        lnkTransfer.Visible = false;
                    }
                }

                

                if (cell_3_Value == "Approve")
                {
                    if (gr.RowType == DataControlRowType.DataRow)
                    {
                        lnkTransfer = gr.FindControl("lnkReject") as LinkButton;
                        lnkTransfer.Visible = false;
                        lnkTransfer = gr.FindControl("lnkApproval") as LinkButton;
                        lnkTransfer.Visible = true;
                        lnkTransfer = gr.FindControl("lnkBypass") as LinkButton;
                        lnkTransfer.Visible = true;
                        
                    }
                }

                
            }
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            if (e.CommandName == "Approved")
            {
                string ExpenceID = string.Empty;
                LinkButton btn = sender as LinkButton;
                //GridViewRow row = btn.NamingContainer as GridViewRow;
                ExpenceID = GridView1.DataKeys[row.RowIndex].Values[0].ToString();
                //strExpenseSaveID = GridView1.DataKeys[row.RowIndex].Values[0].ToString();
                ClientScript.RegisterStartupScript(GetType(), "showModalScript", "VerifyAmount('" + ExpenceID + "');", true);
            }
            if (e.CommandName == "Rejected")
            {
                LinkButton lnkView = (LinkButton)e.CommandSource;
                lblTransactionId.Text = lnkView.CommandArgument;
                this.ModalPopupExtender2.Show();
            }
            if (e.CommandName == "Transfer")
            {
                LinkButton lnkTransfer = (LinkButton)e.CommandSource;
                string TransactionID = lnkTransfer.CommandArgument;
                string sqlquery1 = "update ExpenseSave set CEOExpenseStatus='N',AccExpenseStatus='Y' where ExpenseSaveID='" + TransactionID + "'";
                int i = Utils.ExecNonQuery(sqlquery1, CnnString);

                
                if (i > 0)
                {
                    lblSucMess.Text = "Record Transfer Successfully...";
                    buildGrid();
                    
                    //buildGridTransfer();
                }
                else
                {
                    lblSucMess.Text = "Record not Transfer..";
                }
            }

            if (e.CommandName == "ByPass")
            {
                try
                {
                    
                    int UserID = Convert.ToInt32(Session["UID"].ToString());      
                    LinkButton lnkBypass = (LinkButton)e.CommandSource;
                    string Expenseid = lnkBypass.CommandArgument;
                    SQLString = "UPDATE ExpenseSave SET AccExpenseStatus='Approve',VerifiedByAcc='" + UserID + "' ,VerifiedAmountByAcc=VerifiedAmount ,VerifiedRemarksByAcc='ByPassAmount',CEOExpenseStatus='Approve', VerifiedByAccDate=getdate(),Remarks='Amount ByPass'  where ExpenseSaveID='" + Expenseid + "'";
                    int i = Utils.ExecNonQuery(SQLString, CnnString);
                    if (i > 0)
                    {
                        lblSucMess.Text = "Record ByPass Approve Successfully...";
                        buildGrid();
                    }
                    else
                    {
                        lblSucMess.Text = "Record not ByPass Approve..";
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
               
            }
        }

        protected void btnReject(object sender, EventArgs e)
        {
            string TransactionID = lblTransactionId.Text;
            if (TransactionID != "")
            {
                if (txtRemarks.Text.ToString() == "")
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = "Please Enter Rejected Summary";
                    return;
                }
                string sqlquery1 = "update ExpenseSave set AccExpenseStatus='Rejected' , VerifiedByAcc='" + Session["UID"].ToString() + "' , VerifiedByAccDate=getdate(),VerifiedRemarksByAcc='" + txtRemarks.Text + "' where ExpenseSaveID='" + TransactionID + "'";
                int i = Utils.ExecNonQuery(sqlquery1, CnnString);
                if (i > 0)
                {
                    lblSucMess.Text = "Record Rejected Successfully...";
                    buildGrid();
                }
                else
                {
                    lblSucMess.Text = "Record not Rejected..";
                }
            }
        }
       
      
    }
}