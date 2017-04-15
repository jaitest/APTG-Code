using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

namespace HSRP.Expenses
{


    public partial class EmployeeStatusDetails : System.Web.UI.Page
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        Utils bl = new Utils();

        string HSRPStateID = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType1 = string.Empty;
        string ProductivityID = string.Empty;
        string Transactionid = string.Empty;
        string AccountNo = string.Empty;
        string Bankname = string.Empty;
        int intHSRPStateID;
        int intRTOLocationID;        

        int UserType;
       
        //string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty;
       
        String StringMode = string.Empty;
        string CurrentDate = DateTime.Now.ToString();
        string query1 = string.Empty;
        
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
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {

                        lblErrMsg.Text = string.Empty;
                        FilldropDownListOrganization();
                        FilldropDowndistrictcenter();
                       // buildGrid();                  
                       // Utils.user_log(strUserID, "View Organization", ComputerIP, "Page load", CnnString);

                        
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }


        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();

            }
        }

        private void FilldropDownListClient()
        {
            if (UserType.Equals(0))
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                string SQLString11 = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and District='" + ddldistrictname.SelectedValue.ToString() + "' and  ActiveStatus!='N'   Order by RTOLocationName asc";
                Utils.PopulateDropDownList(ddllocation, SQLString11.ToString(), CnnString, "--Select Location Name--");
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                //string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
                string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM Employeemaster e INNER JOIN RTOLocation a ON e.RTOLocationID = a.RTOLocationID where a.Hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and a.District='" + ddldistrictname.SelectedValue.ToString() + "' and e.Activestatus='Y'  Order by RTOLocationName asc ";
                System.Data.DataTable dt1 = Utils.GetDataTable(SQLString11, CnnString);
                ddllocation.DataSource = dt1;
                ddllocation.DataBind();
                ddllocation.Items.Insert(0, new ListItem("--Select Location Name--"));
            }
        }

        private void FilldropDowndistrictcenter()
        {
            if (UserType.Equals(0))
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                string SQLString11 = "select distinct district,district from rtolocation where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' and EmbCenterName is not null group by district,district Order by 1";
                //string SQLString11 = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName asc";
                Utils.PopulateDropDownList(ddldistrictname, SQLString11.ToString(), CnnString, "--Select District Name--");
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                //string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";

                //string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM Employeemaster e INNER JOIN RTOLocation a ON e.RTOLocationID = a.RTOLocationID where a.Hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and EmbCenterName='" + ddldistrictname.SelectedValue.ToString() + "' and e.Activestatus='Y'  Order by RTOLocationName asc ";
                string SQLString11 = "select distinct district ,district from rtolocation where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N' and EmbCenterName is not null group by district,district Order by 1";
                System.Data.DataTable dt1 = Utils.GetDataTable(SQLString11, CnnString);
                ddldistrictname.DataSource = dt1;
                ddldistrictname.DataBind();
                ddldistrictname.Items.Insert(0, new ListItem("--Select District Name--"));
            }
        }

        public void BindGrid()
        {
            //DataSet ds = Utils.getDataSet("EmployeeMasterStatus_Details '" + HSRPStateID + "','" + ddllocation.SelectedValue.ToString() + "','0','0','0','0','0','0','0','SELECT'", CnnString);
            SQLString = "select Emp_id,Emp_Name,Email,Designation,MobileNo,Department,Convert(varchar(12),EntryDate,103) as 'EntryDate' ,ActiveStatus,UserId,ApprovalHead,[Role],hsrp_stateid,RtoLocationid,Convert(varchar(12),EmpJoiningDate,103) as 'EmpJoiningDate' From Employeemaster where hsrp_stateid='" + HSRPStateID + "'";

            DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
            Grid1.DataSource = dt;
            Grid1.PageIndex = Convert.ToInt32(Request.QueryString["PageIndex"]);
            Grid1.DataBind();
            lblErrMsg.Text = string.Empty;

            //if (dt.Rows.Count > 0)
            //{
            //    lblErrMsg.Text = "";
            //    lblErrMsg.Text = "";

            //    Grid1.DataSource = dt;
            //    Grid1.DataBind();
            //}
            //else
            //{
            //    lblErrMsg.Text = "";
            //   // ds.Tables[0].Rows.Add(dt.Tables[0].NewRow());
            //    Grid1.DataSource = dt;
            //    Grid1.DataBind();
            //    int columncount = Grid1.Rows[0].Cells.Count;
            //    Grid1.Rows[0].Cells.Clear();
            //    Grid1.Rows[0].Cells.Add(new TableCell());
            //    Grid1.Rows[0].Cells[0].ColumnSpan = columncount;
            //    Grid1.Rows[0].Cells[0].Text = "No Records Found";
            //}
        }

       

        public void buildGrid()
        {
            try
            {
                //string SQLString = "SELECT [voidstatus], [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate, convert(varchar(10), EntryDate, 103) as EntryDate, [BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.rtolocation where StateID='" + HSRPStateID + "' and isnull(dealerid,'')=''  order by DepositDate desc";

                string SQLString = "SELECT [voidstatus], [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate, convert(varchar(10), EntryDate, 103) as EntryDate, [BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.depositelocationid  where StateID='" + HSRPStateID + "'   order by DepositDate desc";
              
                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                Grid1.DataSource = dt;
                Grid1.PageIndex = Convert.ToInt32(Request.QueryString["PageIndex"]);
                Grid1.DataBind();
                lblErrMsg.Text = string.Empty;
               
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Label lblvoid = (Label)e.Row.FindControl("lblvoid");
                
            //    if (lblvoid != null)
            //    {
            //        if (lblvoid.Text.ToLower() == "void")
            //        {
            //            Label lbledit = (Label)e.Row.FindControl("lbledit");

            //            foreach (TableCell cell in e.Row.Cells)
            //            {

            //                cell.BackColor = Color.AliceBlue;

            //            }
            //            e.Row.Enabled = false;
            //            lblvoid.Visible = false;
            //            lbledit.Visible = false;

            //        }
            //        else
            //        {

            //            lblvoid.Text = "Void";
            //        }
                   
            //    }
            //}
        }

        protected void Grid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            Response.Redirect("UpdateEmpDetails.aspx?PageIndex=" + e.NewPageIndex.ToString());
            //buildGrid();
            BindGrid();
          
        }

        protected void Grid1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void BtnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                string Empid = txtempid.Text.Trim().ToString();

                string Empname = txtempname.Text.Trim().ToString();


                string SQLString = string.Empty;

                if (Empid != "" && string.IsNullOrEmpty(Empname))
                {
                    SQLString = "SELECT Hsrp_stateid,RtoLocationID,Emp_ID as 'EmployeeID',Emp_Name as 'EmployeeName',Designation,MobileNo,Convert(varchar(12),EmpJoiningDate,103) as 'JoiningDate' , Department,CompanyName,Activestatus FROM EmployeeMaster where activestatus in ('N','Y') and Hsrp_stateID='" + HSRPStateID + "' and rtolocationid='" + ddllocation.SelectedValue.ToString() + "'  and Emp_ID like '%" + Empid + "%' order by Emp_name asc";
                    // SQLString = "SELECT [voidstatus], [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate, convert(varchar(10), EntryDate, 103) as EntryDate, [BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.depositelocationid where StateID='" + HSRPStateID + "'  and TransactionID like '%" + Transactionid + "%' order by DepositDate desc";

                }

                else if (string.IsNullOrEmpty(Empid) && Empname != "")
                {
                    //SQLString = "SELECT [voidstatus], [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate, convert(varchar(10), EntryDate, 103) as EntryDate, [BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.depositelocationid where StateID='" + HSRPStateID + "'   and  bm.AccountNo  like '%" + AccountNo + "%'    order by DepositDate desc";
                    SQLString = "SELECT Hsrp_stateid,RtoLocationID,Emp_ID as 'EmployeeID',Emp_Name as 'EmployeeName',Designation,MobileNo,Convert(varchar(12),EmpJoiningDate,103) as 'JoiningDate' , Department,CompanyName,Activestatus FROM EmployeeMaster where activestatus in ('N','Y') and Hsrp_stateID='" + HSRPStateID + "' and rtolocationid='" + ddllocation.SelectedValue.ToString() + "' and Emp_Name like '%" + Empname + "%' order by Emp_name asc";

                }


                else if (Empid != "" && Empname != "")
                {
                    //SQLString = "SELECT [voidstatus], [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate, convert(varchar(10), EntryDate, 103) as EntryDate, [BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.depositelocationid where StateID='" + HSRPStateID + "'   and TransactionID like '%" + Transactionid + "%'  and  bm.AccountNo  like '%" + AccountNo + "%'  order by DepositDate desc";
                    SQLString = "SELECT Hsrp_stateid,RtoLocationID,Emp_ID as 'EmployeeID',Emp_Name as 'EmployeeName',Designation,MobileNo,Convert(varchar(12),EmpJoiningDate,103) as 'JoiningDate' , Department,CompanyName,Activestatus FROM EmployeeMaster where activestatus in ('N','Y') and Hsrp_stateID='" + HSRPStateID + "' and rtolocationid='" + ddllocation.SelectedValue.ToString() + "' and Emp_ID like '%" + Empid + "%' and Emp_Name like '%" + Empname + "%' order by Emp_name asc";

                }

                else
                {
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = "Not Valid Search";
                    BindGrid();
                    return;
                }

                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                if (dt.Rows.Count > 0)
                {

                    lblErrMsg.Text = string.Empty;
                    Grid1.DataSource = dt;
                    // Grid1.PageIndex = Convert.ToInt32(Request.QueryString["PageIndex"]);
                    Grid1.DataBind();

                }
                else
                {

                    BindGrid();
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = "Not Valid Search";
                    return;
                }

            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }


        }

        protected void btngo_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (ddldistrictname.SelectedItem.ToString() == "--Select District Name--")
                {
                    lblErrMsg.Text = "Please Select District Name";
                    return;
                }

                if (ddllocation.SelectedItem.ToString() == "--Select Location Name--")
                {
                    lblErrMsg.Text = "Please Select Location Name";
                    return;
                }
                BindGrid();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void ddldistrictname_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FilldropDownListClient();
            }
            catch (Exception)
            {

                throw;
            }
        }


     
       
    }
}