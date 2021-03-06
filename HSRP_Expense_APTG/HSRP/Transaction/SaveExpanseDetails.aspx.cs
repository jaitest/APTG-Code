﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class SaveExpanseDetails : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        int UserType;
        int HSRP_StateID;
        int RTOLocationID;
        int intHSRP_StateID;
        int intRTOLocationID;
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).RegisterPostBackControl(this.btnSave);
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                GetTotalSavedRecords();

                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                HSRP_StateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                lblErrMsg.Text = string.Empty;
                strUserID = Session["UID"].ToString();


                if (!IsPostBack)
                {
                    try
                    {
                        InitialSetting();
                        FilldropDownListExpense();
                        FilldropDownListOrganization();
                        FilldropDownListClient();
                        
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load " + err.Message.ToString();
                    }
                }
            }
        }

        private void GetTotalSavedRecords()
        {
            try
            {
                SQLString = "select count(*) as TotalSavedRecord from ExpenseSave where userID='" + Session["UID"].ToString() + "' and BillDate between convert(varchar(10), GetDate(),111)+' 00:00:00' and convert(varchar(10), GetDate(),111)+' 23:59:59'";
                int dttotal = Utils.getScalarCount(SQLString, CnnString);
                if (dttotal > 0)
                {
                    lblTotalRecord.Text = dttotal.ToString();
                }
            }
            catch (Exception ex)
            {

                lblErrMsg.Text = ex.Message;
            }
        }
        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            OrderDate.MinDate = (DateTime.Parse(TodayDate)).AddDays(-15.00);
        }

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(dropDownListOrg, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                dropDownListOrg.DataSource = dts;
                dropDownListOrg.DataBind();
            }
        }

        private void FilldropDownListClient()
        {
            try
            {
                if (UserType.Equals(0))
                {
                    int.TryParse(dropDownListOrg.SelectedValue, out intHSRP_StateID);
                    SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus!='N' Order by RTOLocationName";
                    Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");

                }
                else
                {
                    if (HSRP_StateID == 11 || HSRP_StateID == 9)
                    {
                        intHSRP_StateID = HSRP_StateID;
                        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus!='N' and locationtype='Sub-Urban' Order by RTOLocationName";
                        Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");

                    }
                    else
                    {
                        intHSRP_StateID = HSRP_StateID;
                        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus!='N' Order by RTOLocationName";
                        DataSet dss = Utils.getDataSet(SQLString, CnnString);
                        dropDownListClient.DataSource = dss;
                        dropDownListClient.DataBind();
                        dropDownListClient.Items.Insert(0, new ListItem("--Select RTO--", "--Select RTO--"));
                    }
                }
            }
            catch (Exception ex)
            {

                lblErrMsg.Text = ex.Message;
            }
        }

        protected void dropDownListOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListOrg.SelectedItem.Text != "--Select State--")
            {
                dropDownListClient.Visible = true;
                FilldropDownListClient();
            }
        }

        private void FilldropDownListExpense()
        {

            SQLString = "select * from ExpenseMaster where ActiveStatus='Y' Order by ExpenseName";

            Utils.PopulateDropDownList(ddlExpense, SQLString.ToString(), CnnString, "--Select Expense--");
            dropDownListOrg.SelectedIndex = 0;
        }
        
        #endregion
        protected void btnSave_Click(object sender, EventArgs e)
        {
            

                //String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                String From1 = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                DateTime BillDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                // DateTime BillDate = OrderDate.SelectedDate;
                // string dtt = BillDate.ToString("dd-MMM-yyyy");


                decimal Amount = Convert.ToDecimal(txtAmount.Text);
                SQLString = "Insert Into ExpenseSave (HSRP_StateID,LocationID,ExpenseID,BillNo,BillDate,BillAmount,Remarks,ExpenseStatus,VendorName,ClaimedBy,UserID,ClaimedByID) VALUES ('" + dropDownListOrg.SelectedValue + "','" + dropDownListClient.SelectedValue + "','" + ddlExpense.SelectedValue + "','" + txtBillNo.Text + "','" + BillDate + "','" + txtAmount.Text + "','" + txtRemarks.Text + "','Pending','" + txtVenName.Text + "','" + dropDownListEmployee.SelectedItem.ToString() + "','" + Session["UID"].ToString() + "','" + dropDownListEmployee.SelectedValue + "')";
                int i = Utils.ExecNonQuery(SQLString, CnnString);
                if (i > 0)
                {
                    BlankFields();
                    DataTable id = Utils.GetDataTable("select top 1 'VCH'+right('000000000' + convert(varchar(25), ExpenseSaveID), 9) as InvoiceNo from ExpenseSave order by ExpenseSaveID desc", CnnString);
                    if (id.Rows.Count > 0)
                    {
                        //lblSucMess.Text = "Your Voucher No : " + id.Rows[0]["InvoiceNo"];// +Environment.NewLine + "Please Write in Expanse Voucher";
                        lblSucMess.Text = "Expense Save Successfully";// +Environment.NewLine + "Please Write in Expanse Voucher";
                        return;
                    }
                    GetTotalSavedRecords();
                }    
                int Count = Utils.getScalarCount("select COUNT(*) from ExpenseSave where hsrp_stateid ='" + HSRP_StateID + "' and locationid ='" + RTOLocationID + "' and BillNo='" + txtBillNo.Text + "'", CnnString);
                if (Count > 0)
                {
                    //lblErrMsg.Text = "Duplicate Records";
                    //lblSucMess.Text = "";
                    //return;
                }
                else
                {
                    //decimal RemainAmt = Convert.ToDecimal(txtVat.Text) + Convert.ToDecimal(txtservicetax.Text) + Convert.ToDecimal(txtExcduty.Text) + Convert.ToDecimal(txtOthers.Text);
                    //if (Amount < RemainAmt)
                    //{
                    //    lblErrMsg.Text = "Basic Amount should not be less than other taxes amount.";
                    //    lblSucMess.Text = "";
                    //    btnSave.Enabled = false;
                    //}
                    //else
                    //{

                    //    SQLString = "Insert Into ExpenseSave (HSRP_StateID,LocationID,ExpenseID,BillNo,BillDate,BillAmount,Remarks,ExpenseStatus,VendorName,ServiceAmount,ExciseAmount,OtherAmount,VatAmount,ClaimedBy,UserID,ClaimedByID) VALUES ('" + dropDownListOrg.SelectedValue + "','" + dropDownListClient.SelectedValue + "','" + ddlExpense.SelectedValue + "','" + txtBillNo.Text + "','" + BillDate + "','" + txtAmount.Text + "','" + txtRemarks.Text + "','Pending','" + txtVenName.Text + "','" + txtservicetax.Text + "','" + txtExcduty.Text + "','" + txtOthers.Text + "','" + txtVat.Text + "','" + txtClaimedBy.Text + "','" + Session["UID"].ToString() + "','" + dropDownListEmployee.SelectedValue + "')";
                    //    int i = Utils.ExecNonQuery(SQLString, CnnString);
                    //    if (i > 0)
                    //    {
                    //        BlankFields();
                    //        // int id = Utils.getScalarCount("select top 1 'VCH'+right('000000000' + convert(varchar(25), ExpenseSaveID), 9) from ExpenseSave order by ExpenseSaveID desc", CnnString);
                    //        DataTable id = Utils.GetDataTable("select top 1 'VCH'+right('000000000' + convert(varchar(25), ExpenseSaveID), 9) as InvoiceNo from ExpenseSave order by ExpenseSaveID desc", CnnString);
                    //        if (id.Rows.Count > 0)
                    //        {
                    //            lblSucMess.Text = "Your Voucher No : " + id.Rows[0]["InvoiceNo"] + Environment.NewLine + "Please Write in Expanse Voucher";
                    //            return;
                    //        }
                    //        GetTotalSavedRecords();
                    //    }
                    //}
                }
            //}

            //UpdatePanelMsg.Update();
            //UpdatePanelTotalAmount.Update();
            UpdatePanelButton.Update();
        }

        private void BlankFields()
        {
            FilldropDownListExpense();
            FilldropDownListClient();
            FilldropDownListOrganization();
            txtBillNo.Text = string.Empty;
            //txtClaimedBy.Text = string.Empty;
            txtAmount.Text = "0";
            //txtVat.Text = "0";
            //txtservicetax.Text = "0";
            //txtExcduty.Text = "0";
            //txtOthers.Text = "0";
            //txttotamt.Text = "0";
            dropDownListEmployee.ClearSelection();
            txtRemarks.Text = string.Empty;
            txtVenName.Text = string.Empty;
            lblErrMsg.Text = string.Empty;
            
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {

            decimal TotAmount;
            if (txtAmount.Text == "")
            {
                txtAmount.Text = "0";
            }
            //if (txtVat.Text == "")
            //{
            //    txtVat.Text = "0";
            //}
            //if (txtservicetax.Text == "")
            //{
            //    txtservicetax.Text = "0";
            //}
            //if (txtExcduty.Text == "")
            //{
            //    txtExcduty.Text = "0";
            //}
            //if (txtOthers.Text == "")
            //{
            //    txtOthers.Text = "0";
            //}

            decimal Amount = Convert.ToDecimal(txtAmount.Text);
            lblErrMsg.Text = "";
            lblSucMess.Text = "";
            btnSave.Enabled = true;
            //decimal RemainAmt = Convert.ToDecimal(txtVat.Text) + Convert.ToDecimal(txtservicetax.Text) + Convert.ToDecimal(txtExcduty.Text) + Convert.ToDecimal(txtOthers.Text);
            //if (Amount < RemainAmt)
            //{
            //    lblErrMsg.Text = "Basic Amount should not be less than other taxes amount.";
            //    lblSucMess.Text = "";
            //    return;
            //    //btnSave.Enabled = false;
            //}
            //else
            //{
            //    lblErrMsg.Text = "";
            //    lblSucMess.Text = "";
            //    btnSave.Enabled = true;
            //}
            //TotAmount = (Convert.ToDecimal(txtAmount.Text) + Convert.ToDecimal(txtVat.Text) + Convert.ToDecimal(txtservicetax.Text) + Convert.ToDecimal(txtExcduty.Text) + Convert.ToDecimal(txtOthers.Text));
            //txttotamt.Text = TotAmount.ToString();
            //UpdatePanelTotalAmount.Update();
            //UpdatePanelMsg.Update();
            UpdatePanelButton.Update();

        }

        //protected void txtVat_TextChanged(object sender, EventArgs e)
        //{
        //    decimal TotAmount;
        //    if (txtAmount.Text == "")
        //    {
        //        txtAmount.Text = "0";
        //    }
        //    //if (txtVat.Text == "")
        //    //{
        //    //    txtVat.Text = "0";
        //    //}
        //    //if (txtservicetax.Text == "")
        //    //{
        //    //    txtservicetax.Text = "0";
        //    //}
        //    //if (txtExcduty.Text == "")
        //    //{
        //    //    txtExcduty.Text = "0";
        //    //}
        //    //if (txtOthers.Text == "")
        //    //{
        //    //    txtOthers.Text = "0";
        //    //}

        //    decimal Amount = Convert.ToDecimal(txtAmount.Text);
        //    lblErrMsg.Text = "";
        //    lblSucMess.Text = "";
        //    btnSave.Enabled = true;
        //    //decimal RemainAmt = Convert.ToDecimal(txtVat.Text) + Convert.ToDecimal(txtservicetax.Text) + Convert.ToDecimal(txtExcduty.Text) + Convert.ToDecimal(txtOthers.Text);
        //    //if (Amount < RemainAmt)
        //    //{
        //    //    lblErrMsg.Text = "Basic Amount should not be less than other taxes amount.";
        //    //    lblSucMess.Text = "";
        //    //    btnSave.Enabled = false;
        //    //}
        //    //else
        //    //{
        //    //    lblErrMsg.Text = "";
        //    //    lblSucMess.Text = "";
        //    //    btnSave.Enabled = true;
        //    //}
        //    //TotAmount = (Convert.ToDecimal(txtAmount.Text) + Convert.ToDecimal(txtVat.Text) + Convert.ToDecimal(txtservicetax.Text) + Convert.ToDecimal(txtExcduty.Text) + Convert.ToDecimal(txtOthers.Text));
        //    //txttotamt.Text = TotAmount.ToString();
        //    //UpdatePanelTotalAmount.Update();
        //    UpdatePanelMsg.Update();
        //    UpdatePanelButton.Update();
        //}

        //protected void txtservicetax_TextChanged(object sender, EventArgs e)
        //{
        //    decimal TotAmount;
        //    if (txtAmount.Text == "")
        //    {
        //        txtAmount.Text = "0";
        //    }
        //    //if (txtVat.Text == "")
        //    //{
        //    //    txtVat.Text = "0";
        //    //}
        //    //if (txtservicetax.Text == "")
        //    //{
        //    //    txtservicetax.Text = "0";
        //    //}
        //    //if (txtExcduty.Text == "")
        //    //{
        //    //    txtExcduty.Text = "0";
        //    //}
        //    //if (txtOthers.Text == "")
        //    //{
        //    //    txtOthers.Text = "0";
        //    //}

        //    decimal Amount = Convert.ToDecimal(txtAmount.Text);
        //    lblErrMsg.Text = "";
        //    lblSucMess.Text = "";
        //    btnSave.Enabled = true;
        //    //decimal RemainAmt = Convert.ToDecimal(txtVat.Text) + Convert.ToDecimal(txtservicetax.Text) + Convert.ToDecimal(txtExcduty.Text) + Convert.ToDecimal(txtOthers.Text);
        //    //if (Amount < RemainAmt)
        //    //{
        //    //    lblErrMsg.Text = "Basic Amount should not be less than other taxes amount.";
        //    //    lblSucMess.Text = "";
        //    //    btnSave.Enabled = false;
        //    //}
        //    //else
        //    //{
        //    //    lblErrMsg.Text = "";
        //    //    lblSucMess.Text = "";
        //    //    btnSave.Enabled = true;
        //    //}
        //    //TotAmount = (Convert.ToDecimal(txtAmount.Text) + Convert.ToDecimal(txtVat.Text) + Convert.ToDecimal(txtservicetax.Text) + Convert.ToDecimal(txtExcduty.Text) + Convert.ToDecimal(txtOthers.Text));
        //    //txttotamt.Text = TotAmount.ToString();
        //    //UpdatePanelTotalAmount.Update();
        //    UpdatePanelMsg.Update();
        //    UpdatePanelButton.Update();
        //}

        //protected void txtExcduty_TextChanged(object sender, EventArgs e)
        //{
        //    decimal TotAmount;
        //    if (txtAmount.Text == "")
        //    {
        //        txtAmount.Text = "0";
        //    }
        //    decimal Amount = Convert.ToDecimal(txtAmount.Text);
        //    lblErrMsg.Text = "";
        //    lblSucMess.Text = "";
        //    btnSave.Enabled = true;
        //    //if (txtVat.Text == "")
        //    //{
        //    //    txtVat.Text = "0";
        //    //}
        //    //if (txtservicetax.Text == "")
        //    //{
        //    //    txtservicetax.Text = "0";
        //    //}
        //    //if (txtExcduty.Text == "")
        //    //{
        //    //    txtExcduty.Text = "0";
        //    //}
        //    //if (txtOthers.Text == "")
        //    //{
        //    //    txtOthers.Text = "0";
        //    //}


        //    //decimal Amount = Convert.ToDecimal(txtAmount.Text);
        //    //decimal RemainAmt = Convert.ToDecimal(txtVat.Text) + Convert.ToDecimal(txtservicetax.Text) + Convert.ToDecimal(txtExcduty.Text) + Convert.ToDecimal(txtOthers.Text);
        //    //if (Amount < RemainAmt)
        //    //{
        //    //    lblErrMsg.Text = "Basic Amount should not be less than other taxes amount.";
        //    //    lblSucMess.Text = "";
        //    //    btnSave.Enabled = false;
        //    //}
        //    //else
        //    //{
        //    //    lblErrMsg.Text = "";
        //    //    lblSucMess.Text = "";
        //    //    btnSave.Enabled = true;
        //    //}

        //    //TotAmount = (Convert.ToDecimal(txtAmount.Text) + Convert.ToDecimal(txtVat.Text) + Convert.ToDecimal(txtservicetax.Text) + Convert.ToDecimal(txtExcduty.Text) + Convert.ToDecimal(txtOthers.Text));
        //    //txttotamt.Text = TotAmount.ToString();
        //    //UpdatePanelTotalAmount.Update();
        //    UpdatePanelMsg.Update();
        //    UpdatePanelButton.Update();
        //}

        //protected void txtOthers_TextChanged(object sender, EventArgs e)
        //{
        //    decimal TotAmount;
        //    if (txtAmount.Text == "")
        //    {
        //        txtAmount.Text = "0";
        //    }
        //    decimal Amount = Convert.ToDecimal(txtAmount.Text);
        //    lblErrMsg.Text = "";
        //    lblSucMess.Text = "";
        //    btnSave.Enabled = true;
        //    //if (txtVat.Text == "")
        //    //{
        //    //    txtVat.Text = "0";
        //    //}
        //    //if (txtservicetax.Text == "")
        //    //{
        //    //    txtservicetax.Text = "0";
        //    //}
        //    //if (txtExcduty.Text == "")
        //    //{
        //    //    txtExcduty.Text = "0";
        //    //}
        //    //if (txtOthers.Text == "")
        //    //{
        //    //    txtOthers.Text = "0";
        //    //}


        //    //decimal Amount = Convert.ToDecimal(txtAmount.Text);
        //    //decimal RemainAmt = Convert.ToDecimal(txtVat.Text) + Convert.ToDecimal(txtservicetax.Text) + Convert.ToDecimal(txtExcduty.Text) + Convert.ToDecimal(txtOthers.Text);
        //    //if (Amount < RemainAmt)
        //    //{
        //    //    lblErrMsg.Text = "Basic Amount should not be less than other taxes amount";
        //    //    lblSucMess.Text = "";
        //    //    btnSave.Enabled = false;
        //    //}
        //    //else
        //    //{
        //    //    lblErrMsg.Text = "";
        //    //    lblSucMess.Text = "";
        //    //    btnSave.Enabled = true;
        //    //}

        //    //TotAmount = (Convert.ToDecimal(txtAmount.Text) + Convert.ToDecimal(txtVat.Text) + Convert.ToDecimal(txtservicetax.Text) + Convert.ToDecimal(txtExcduty.Text) + Convert.ToDecimal(txtOthers.Text));
        //    //txttotamt.Text = TotAmount.ToString();
        //    //UpdatePanelTotalAmount.Update();
        //    UpdatePanelMsg.Update();
        //    UpdatePanelButton.Update();
        //}

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListClient.SelectedItem.ToString() != "--Select RTO--")
            {
                dropDownListClient.Visible = true;
                FillDropDownEmployee();
                btnSave.Enabled = true;
            }
            dropDownListEmployee.ClearSelection();
            ddlExpense.ClearSelection();
            //txtClaimedBy.Text = "";
            btnSave.Enabled = true;
        }

        public void FillDropDownEmployee()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(dropDownListOrg.SelectedValue, out intHSRP_StateID);
                SQLString = "select ID,Emp_Name from employeemaster Where HSRP_StateID=" + intHSRP_StateID + " and RTOLocationID='" + dropDownListClient.SelectedValue + "' and ActiveStatus='Y' and isnull(userid,'')!='' Order by Emp_Name ";
                Utils.PopulateDropDownList(dropDownListEmployee, SQLString.ToString(), CnnString, "--Select Employee--");
            }
            else
            {
                intHSRP_StateID = HSRP_StateID;
                SQLString = "select ID,Emp_Name from employeemaster Where HSRP_StateID=" + intHSRP_StateID + " and RTOLocationID='" + dropDownListClient.SelectedValue + "' and ActiveStatus='Y' and isnull(userid,'')!='' Order by Emp_Name ";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListEmployee.DataSource = dss;
                dropDownListEmployee.DataBind();
                dropDownListEmployee.Items.Insert(0, new ListItem("--Select Employee--", "--Select Employee--"));
                dropDownListOrg.SelectedIndex = 0;
            }
           
        }

    }
}