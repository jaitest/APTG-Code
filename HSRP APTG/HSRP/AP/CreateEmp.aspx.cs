using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.AP
{
    public partial class CreateEmp : System.Web.UI.Page
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty, DealerID = string.Empty;
        string SQLString = string.Empty;
        String StringMode = string.Empty;
        string CurrentDate = DateTime.Now.ToString();
        string query1 = string.Empty;
        DataProvider.BAL bl = new DataProvider.BAL();
         int HSRP_StateID;
         int intHSRP_StateID;
        int intHSRP_rtolocation;
        string userid = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                HSRP_StateID = Convert.ToInt32(HSRPStateID);
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserName = Session["UID"].ToString();
                DealerID = Session["dealerid"].ToString();
                lblErrMess.Text = string.Empty;
                lblSucMess.Text = string.Empty;
            }
            if (!Page.IsPostBack)
            {
                FillUsername();
                FillHeads();
                FillRoles();
                FilldropDownListState();
                FilldropDownListClient();
                InitialSetting();
            }
        }

        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            //OrderDate.MinDate = (DateTime.Parse(TodayDate)).AddDays(-2.00);
        }
        private void FilldropDownListState()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(ddlstate, SQLString.ToString(), ConnectionString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, ConnectionString);
                ddlstate.DataSource = dts;
                ddlstate.DataBind();
            }
        }

        private void FilldropDownListClient()
        {
            try
            {
                if (UserType.Equals(0))
                {
                    int.TryParse(ddlstate.SelectedValue, out intHSRP_StateID);
                    SQLString = "select RTOLocationName,distRelation from employeelocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus='Y' Order by RTOLocationName";
                    DataSet dss = Utils.getDataSet(SQLString, ConnectionString);
                    ddlrtolocation.DataSource = dss;
                    ddlrtolocation.DataTextField = "RTOLocationName";
                    ddlrtolocation.DataValueField = "distRelation";
                    ddlrtolocation.DataBind();
                    ddlrtolocation.Items.Insert(0, new ListItem("--Select RTO--", "--Select RTO--"));
                    //Utils.PopulateDropDownList(ddlrtolocation, SQLString.ToString(), ConnectionString, "--Select RTO--");

                }
                else
                {
                    if (HSRP_StateID == 11 || HSRP_StateID == 9)
                    {
                        intHSRP_StateID = HSRP_StateID;
                        SQLString = "select RTOLocationName,distRelation from employeelocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus='Y' Order by RTOLocationName";
                        DataSet dss = Utils.getDataSet(SQLString, ConnectionString);
                        ddlrtolocation.DataSource = dss;
                        ddlrtolocation.DataTextField = "RTOLocationName";
                        ddlrtolocation.DataValueField = "distRelation";
                        ddlrtolocation.DataBind();
                        ddlrtolocation.Items.Insert(0, new ListItem("--Select RTO--", "--Select RTO--"));
                        //Utils.PopulateDropDownList(ddlrtolocation, SQLString.ToString(), ConnectionString, "--Select RTO--");

                    }
                    else
                    {
                        intHSRP_StateID = HSRP_StateID;
                        SQLString = "select RTOLocationName,distRelation from employeelocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus='Y' Order by RTOLocationName";
                        DataSet dss = Utils.getDataSet(SQLString, ConnectionString);
                        ddlrtolocation.DataSource = dss;
                        ddlrtolocation.DataTextField = "RTOLocationName";
                        ddlrtolocation.DataValueField = "distRelation";
                        ddlrtolocation.DataBind();
                        ddlrtolocation.Items.Insert(0, new ListItem("--Select RTO--", "--Select RTO--"));
                    }
                }
            }
            catch (Exception ex)
            {

                lblErrMess.Text = ex.Message;
            }
        }
        public void FillUsername()
        {
            //int.TryParse(ddlrtolocation.SelectedValue, out intHSRP_rtolocation);
            //int.TryParse(ddlstate.SelectedValue, out intHSRP_StateID);
            SQLString = "Select UserID,UserFirstName +' '+ UserLastName as username from [Users] where hsrp_stateid='" + ddlstate.SelectedValue + "' and rtolocationid='" + ddlrtolocation.SelectedValue + "' and isnull(dealerid,'')='' and activestatus='Y' order by userfirstname";
            DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
            ddlUserAccount.DataSource = dt;
            ddlUserAccount.DataTextField = "username";
            ddlUserAccount.DataValueField = "UserID";
            ddlUserAccount.DataBind();
            ddlUserAccount.Items.Insert(0, new ListItem("--Select username--"));
        }
        
        public void FillHeads()
        {
            SQLString = "Select UserID,UserFirstName+' '+UserLastName as username from [Users] where hsrp_stateid='" + HSRPStateID + "' and isnull(dealerid,'')='' and activestatus='Y' order by userfirstname";
            DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
            ddlhead.DataSource = dt;
            ddlhead.DataTextField = "username";
            ddlhead.DataValueField = "UserID";
            ddlhead.DataBind();
            ddlhead.Items.Insert(0, new ListItem("--Select State Head--"));
        }
        public void FillRoles()
        {
            DataSet ds = Utils.getDataSet("InsertUpdateSelectAndDeleteWithEmpDesignation '0','','0','SELECT'", ConnectionString);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlResponsibility.DataSource = ds.Tables[0];
                ddlResponsibility.DataTextField = "Designation Name";
                ddlResponsibility.DataValueField = "ID";
                ddlResponsibility.DataBind();
                ddlResponsibility.Items.Insert(0, new ListItem("--Select Role--"));
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlstate.SelectedItem.ToString() == "--Select State--")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Select State Name";
                    return;

                }
                if (ddlrtolocation.SelectedItem.ToString() == "--Select RTO--")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Select Rto Location Name";
                    return;

                }
                if (ddlUserAccount.SelectedItem.ToString() != "--Select username--")
                {
                    userid = ddlUserAccount.SelectedValue.ToString();
                }
                else
                {
                    userid = null;
                }
                if (txtempname.Text.ToString() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Employee Name";
                    return;

                }
                if (txtEmail.Text.ToString() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Email-ID";
                    return;

                }
                if (txtmobileno.Text.ToString() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Mobile No";
                    return;

                }
                if (txtempcode.Text.ToString() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Employee Code";
                    return;

                }
                if (txtDesignation.Text.ToString() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Designation";
                    return;
                }
                string sqlstring = "Select * from employeemaster where Emp_id='" + txtempcode.Text.ToString() + "' and activestatus='Y'";
                DataTable dt = Utils.GetDataTable(sqlstring, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Duplicate Entry!.";
                    return;
                }
                sqlstring = "Insert into employeemaster(Emp_Name,Email,MobileNo,Emp_id,Designation,EntryDate,ActiveStatus,UserId,ApprovalHead,[Role],hsrp_stateid,RtoLocationid,EmpJoiningDate) values('" + txtempname.Text.ToString() + "','" + txtEmail.Text.ToString() + "','" + txtmobileno.Text.ToString() + "','" + txtempcode.Text.ToString() + "','" + txtDesignation.Text.ToString() + "',getdate(),'Y','" + userid + "','" + ddlhead.SelectedValue + "','" + ddlResponsibility.SelectedValue.ToString() + "','" + ddlstate.SelectedValue + "','" + ddlrtolocation.SelectedValue + "','" + OrderDate.SelectedDate + "')";
                int i=Utils.ExecNonQuery(sqlstring.ToString(), ConnectionString);
                if (i > 0)
                {
                    lblSucMess.Text = "Record inserted successfully";
                    clear();
                }
                else
                {
                    lblSucMess.Text = "Record not insert";
                }
            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message.ToString();
            }
        }
        public void clear()
        {
            ddlUserAccount.ClearSelection();
            txtempname.Text = "";
            txtmobileno.Text = "";
            txtempcode.Text = "";
            txtEmail.Text = "";
            txtDesignation.Text = "";
            ddlhead.ClearSelection();
            ddlResponsibility.ClearSelection();
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlstate.SelectedItem.Text != "--Select State--")
            {
                ddlstate.Visible = true;
                FilldropDownListClient();
            }
        }

        protected void ddlrtolocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlrtolocation.SelectedItem.ToString() != "--Select RTO--")
            {
                FillUsername();
            }
            ddlUserAccount.ClearSelection();
        }
    }
}