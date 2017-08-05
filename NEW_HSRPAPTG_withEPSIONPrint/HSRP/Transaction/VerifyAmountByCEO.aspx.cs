using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class VerifyAmountByCEO : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string UserType = string.Empty;
        string ExpenceID = string.Empty;
        string Mode = string.Empty;

        //string UserID;
        int HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;
        int UserID;

        int RTOLocationIDAssign;
        int StateIDAssign;
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            } 

            if (!Page.IsPostBack)
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                HSRPStateID = Convert.ToInt16(Session["UserHSRPStateID"].ToString());
                Mode = Request.QueryString["Mode"];
                if (Mode == "Edit")
                {
                    ExpenceID = Request.QueryString["ExpenseID"].ToString();
                    AssignLaserEdit(ExpenceID);

                }
            }
        }

        private void AssignLaserEdit(string ExpenceID)
        {
            SQLString = "SELECT (select HSRPStateName from HSRPState where HSRPState.HSRP_StateID=ExpenseSave.HSRP_StateID) as StateName,(select RTOLocationName from RTOLocation where RTOLocationID=LocationID) as RTOLocationName,(select ExpenseName from ExpenseMaster where ExpenceId=ExpenseId) as ExpenseName,BillNo,CONVERT (varchar(20),BillDate,103) as BillDate,BillAmount,Remarks,VendorName,ClaimedBy,VerifiedAmount,(Select Userfirstname+''+userlastname from users where userid=verifiedBy) as verifiedBy,verifiedremarks,CONVERT (varchar(20),VerifiedDate,103) as VerifiedDate,ExpenseStatus,vendorname FROM ExpenseSave where ExpenseStatus='Approve' and CEOExpenseStatus='N' and ExpenseSaveID=" + ExpenceID;
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);
            if (ds.Rows.Count > 0)
            {
                LabelStateID.Text = ds.Rows[0]["StateName"].ToString();
                LabelRTOLocationID.Text = ds.Rows[0]["RTOLocationName"].ToString();
                LabelBillDate.Text = ds.Rows[0]["BillDate"].ToString();
                LabelExpenseName.Text = ds.Rows[0]["ExpenseName"].ToString();
                LabelBillNo.Text = ds.Rows[0]["BillNo"].ToString();
                LabelBillAmount.Text = ds.Rows[0]["BillAmount"].ToString();
                Remarks.Text = ds.Rows[0]["Remarks"].ToString();
                lblVendor.Text = ds.Rows[0]["vendorname"].ToString();
                lblClaimedBy.Text = ds.Rows[0]["ClaimedBy"].ToString();
                lblApprovedByHODAmount.Text = ds.Rows[0]["VerifiedAmount"].ToString();
                lblHODName.Text = ds.Rows[0]["verifiedBy"].ToString();
                lblApprovelRemarksData.Text = ds.Rows[0]["verifiedremarks"].ToString();
                lblApprovedDateData.Text = ds.Rows[0]["VerifiedDate"].ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ExpenceID = Request.QueryString["ExpenseID"].ToString();
            UserID = Convert.ToInt32(Session["UID"].ToString());
            if (txtVerifyAmount.Text == "")
            {
                lblErrMess.Text = "Please Enter Verify Amount";
                return;
            }
            if (Convert.ToDouble(txtVerifyAmount.Text) == 0.00)
            {
                lblErrMess.Text = "Please Enter some amount";
                return;
            }
            if (Convert.ToDecimal(txtVerifyAmount.Text) > Convert.ToDecimal(lblApprovedByHODAmount.Text))
            {
                lblErrMess.Text = "Verify amount should not exceed the Approved Amount By HOD";
                return;
            }
            SQLString = "UPDATE ExpenseSave SET CEOExpenseStatus='Approve',VerifiedByCEO='" + UserID + "' ,VerifiedAmountByCEO='" + Convert.ToDecimal(txtVerifyAmount.Text) + "' ,VerifiedRemarksByCEO='" + txtRemarks.Text + "' , VerifiedByDateCEO=getdate()  where ExpenseSaveID=" + ExpenceID;
            int i= Utils.ExecNonQuery(SQLString,CnnString);
            if (i > 0)
            {
                SQLString = "SELECT VerifiedAmountByCEO FROM ExpenseSave where ExpenseSaveID=" + ExpenceID;
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                string showText = string.Empty;
                if (dt.Rows.Count > 0)
                {
                        showText = "Bill Amount cleared";
                        txtVerifyAmount.ReadOnly = true;
                        txtRemarks.ReadOnly = true;
                }
                lblSucMess.Text = showText;
                lblErrMess.Text = "";
                btnSave.Enabled = false;
            }
        }

        
    }
}