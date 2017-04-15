//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Data.SqlClient;
//using DataProvider;
//using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataProvider;
using System.Drawing;


namespace HSRP.Expenses
{
    public partial class EmployeeMasterStatus : System.Web.UI.Page
    {
       
        string UserID = string.Empty;
        int EditStateID;
        Utils objutil = new Utils();
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;       
        //DataTable dt = new DataTable();
        //DataTable dt1 = new DataTable();

       // string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                if (!IsPostBack)
                {

                    // InitialSetting();
                    try
                    {
                        // InitialSetting();
                        if (UserType.Equals(0))
                        {                            
                            FilldropDownListOrganization();
                            FilldropDowndistrictcenter();
                            
                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDowndistrictcenter();
                           // FilldropDownListClient();
                            //BindGrid();
                         
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
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
            DataSet ds = Utils.getDataSet("EmployeeMasterStatus_Details '"+HSRPStateID+"','"+ddllocation.SelectedValue.ToString()+"','0','0','0','0','0','0','0','SELECT'", CnnString);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblErrMess.Text = "";
                lblSucMess.Text = "";
             
                gvEmpDesignation.DataSource = ds;
                gvEmpDesignation.DataBind();
            }
            else
            {
                lblErrMess.Text = "";
              
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gvEmpDesignation.DataSource = ds;
                gvEmpDesignation.DataBind();
                int columncount = gvEmpDesignation.Rows[0].Cells.Count;
                gvEmpDesignation.Rows[0].Cells.Clear();
                gvEmpDesignation.Rows[0].Cells.Add(new TableCell());
                gvEmpDesignation.Rows[0].Cells[0].ColumnSpan = columncount;
                gvEmpDesignation.Rows[0].Cells[0].Text = "No Records Found";
            }
        }
              

        protected void gvEmpDesignation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //BindGrid();
        }

        protected void gvEmpDesignation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            
            gvEmpDesignation.EditIndex = e.NewEditIndex;
           // string name = (gvEmpDesignation.Rows[e.NewEditIndex].Cells[1].Controls[0] as TextBox).Text;           
           // this.BindGrid();
        }

        protected void gvEmpDesignation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {            
            gvEmpDesignation.EditIndex = -1;
            this.BindGrid();
        }

        protected void gvEmpDesignation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label lblEditstateid = (Label)gvEmpDesignation.Rows[e.RowIndex].FindControl("lblEditstateid");
            Label lblEditlocationid = (Label)gvEmpDesignation.Rows[e.RowIndex].FindControl("lblEditlocationid");
            Label lblEditid = (Label)gvEmpDesignation.Rows[e.RowIndex].FindControl("lblEditid");
            TextBox txtEditempName = (TextBox)gvEmpDesignation.Rows[e.RowIndex].FindControl("txtEditempName");
            TextBox txtEditdesignation = (TextBox)gvEmpDesignation.Rows[e.RowIndex].FindControl("txtEditdesignation");
            TextBox txtEditmobno = (TextBox)gvEmpDesignation.Rows[e.RowIndex].FindControl("txtEditmobno");
           // Label lblempdate = (Label)gvEmpDesignation.Rows[e.RowIndex].FindControl("lblempdate");
            TextBox txtEditdepartment = (TextBox)gvEmpDesignation.Rows[e.RowIndex].FindControl("txtEditdepartment");
            TextBox txtEditcompanyname = (TextBox)gvEmpDesignation.Rows[e.RowIndex].FindControl("txtEditcompanyname");
            TextBox txtEditactive = (TextBox)gvEmpDesignation.Rows[e.RowIndex].FindControl("txtEditactive");
            //TextBox txtEditdesignation = (TextBox)gvEmpDesignation.Rows[e.RowIndex].FindControl("txtEditdesignation");
            int i = Utils.ExecNonQuery("EmployeeMasterStatus_Details " + lblEditstateid.Text + "," + lblEditlocationid.Text + "," + lblEditid.Text + ",'" + txtEditempName.Text + "','" + txtEditdesignation.Text + "','" + txtEditmobno.Text + "','" + txtEditdepartment.Text + "','" + txtEditcompanyname.Text + "','" + txtEditactive.Text + "','UPDATE'", CnnString);

           
            if (i > 0)
            {
                lblErrMess.Text = "";
                lblSucMess.Text = "Record Update Successfully.";
                BindGrid();
               // textboxBoxHSRPState.Text = "";
            }
            else
            {
                lblSucMess.Text = "";
                lblErrMess.Text = "Record not Update";
            }
            gvEmpDesignation.EditIndex = -1;
            this.BindGrid();
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FilldropDownListClient();
        }

        protected void ddllocation_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void gvEmpDesignation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEmpDesignation.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btngo_Click(object sender, EventArgs e)
        {
            try
            {
                lblempid.Visible = true;
                lblempname.Visible = true;
                txtempid.Visible = true;
                txtempname.Visible = true;
                BtnSearch.Visible = true;
                if (ddldistrictname.SelectedItem.ToString() == "--Select District Name--")
                {
                    lblErrMess.Text = "Please Select District Name";
                    return;
                }

                if (ddllocation.SelectedItem.ToString() == "--Select Location Name--")
                {
                    lblErrMess.Text = "Please Select Location Name";
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

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string Empid = txtempid.Text.Trim().ToString();

               string Empname = txtempname.Text.Trim().ToString();

             
                string SQLString = string.Empty;

                if (Empid != "" && string.IsNullOrEmpty(Empname))
                {
                    SQLString = "SELECT Hsrp_stateid,RtoLocationID,Emp_ID as 'EmployeeID',Emp_Name as 'EmployeeName',Designation,MobileNo,Convert(varchar(12),EmpJoiningDate,103) as 'JoiningDate' , Department,CompanyName,Activestatus FROM EmployeeMaster where activestatus in ('N','Y') and Hsrp_stateID='"+HSRPStateID+"' and rtolocationid='"+ddllocation.SelectedValue.ToString()+"'  and Emp_ID like '%" + Empid + "%' order by Emp_name asc";
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
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Not Valid Search";
                    BindGrid();
                    return;
                }

                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                if (dt.Rows.Count > 0)
                {

                    lblErrMess.Text = string.Empty;
                    gvEmpDesignation.DataSource = dt;
                    // Grid1.PageIndex = Convert.ToInt32(Request.QueryString["PageIndex"]);
                    gvEmpDesignation.DataBind();

                }
                else
                {

                    BindGrid();
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Not Valid Search";
                    return;
                }

            }
            catch (Exception ex)
            {
                lblErrMess.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        
    }
}