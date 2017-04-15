using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class VerifyAmount : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string UserType = string.Empty;
        string ExpenceID = string.Empty;
        string Mode = string.Empty;
        string CEOExpenseStatus = string.Empty;
        string VerifiedAmountByCEO = string.Empty;

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
            SQLString = "SELECT CONVERT (varchar(20),BillDate,103) as BillDate,VatAmount,ServiceAmount,ExciseAmount,ClaimedBy,OtherAmount,(select ExpenseName from ExpenseMaster where ExpenceId=ExpenseId) as ExpenseName,VerifiedAmount,VendorName,((BillAmount + isnull(VatAmount,0) + isnull(ServiceAmount,0) + isnull(ExciseAmount,0) + isnull(OtherAmount,0))) as Balance,(select HSRPStateName from HSRPState where HSRPState.HSRP_StateID=ExpenseSave.HSRP_StateID) as StateName,(select RTOLocationName from RTOLocation where RTOLocationID=LocationID) as RTOLocationName,BillNo,BillAmount,Remarks,ExpenseStatus FROM ExpenseSave where ExpenseSaveID=" + ExpenceID;
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
                lblClaimedBy.Text = ds.Rows[0]["ClaimedBy"].ToString();
                lblVendor.Text = ds.Rows[0]["VendorName"].ToString();
                if (ds.Rows[0]["Balance"].ToString()=="0.00")
                {
                    btnSave.Enabled = false;
                }
                
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ExpenceID = Request.QueryString["ExpenseID"].ToString();
            UserID = Convert.ToInt32(Session["UID"].ToString());
            if (txtVerifyAmt.Text == "")
            {
                lblErrMess.Text = "Please Enter Verify Amount";
                return;
            }
            if (Convert.ToDouble(txtVerifyAmt.Text) == 0.00)
            {
                lblErrMess.Text = "Please Enter some amount";
                return;
            }
            if (Convert.ToDecimal(txtVerifyAmt.Text) > Convert.ToDecimal(LabelBillAmount.Text))
            {
                lblErrMess.Text = "Verify amount should not exceed the bill amount";
                return;
            }
            if (Convert.ToDouble(txtVerifyAmt.Text) >= 1000.00)
            {
                CEOExpenseStatus = "N";
                VerifiedAmountByCEO = "0";
            }
            else if (CheckBox1.Checked == true)
            {
                CEOExpenseStatus = "N";
                VerifiedAmountByCEO = "0";
            }
            else
            {
                CEOExpenseStatus = "Approve";
                VerifiedAmountByCEO = txtVerifyAmt.Text;
            }
            SQLString = "UPDATE ExpenseSave SET CEOExpenseStatus='" + CEOExpenseStatus + "',VerifiedAmountByCEO='" +  Convert.ToDecimal(VerifiedAmountByCEO) + "',ExpenseStatus='Approve',VerifiedBy='" + UserID + "' ,VerifiedAmount='" + Convert.ToDecimal(txtVerifyAmt.Text) + "' ,VerifiedRemarks='" + txtRemarks.Text + "' , VerifiedDate=getdate()  where ExpenseSaveID=" + ExpenceID;
            int i= Utils.ExecNonQuery(SQLString,CnnString);
            if (i > 0)
            {
                SQLString = "SELECT VerifiedAmount,ExpenseStatus FROM ExpenseSave where ExpenseSaveID=" + ExpenceID;
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                string showText = string.Empty;
                if (dt.Rows.Count > 0)
                {
                        showText = "Bill Amount cleared";
                        txtVerifyAmt.ReadOnly = true;
                        txtRemarks.ReadOnly = true;
                }
                lblSucMess.Text = showText;
                lblErrMess.Text = "";
                btnSave.Enabled = false;
            }
        }

        
    }
}