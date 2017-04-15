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
using System.Collections;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;


namespace HSRP.Transaction
{
    public partial class ConfirmHoldSalarySheet : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string SQlQuery = string.Empty;
        string ExicseAmount = string.Empty;
        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        string strFrmDateString = string.Empty;
        string strToDateString = string.Empty;

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

                        FilldropDownListState();

                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown

        public void FilldropDownListState()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = "select hsrp_stateid,   HSRPStateName from hsrpstate where ActiveStatus='Y' Order by HSRPStateName";
            Utils.PopulateDropDownList(DDlState_Name, SQLString.ToString(), CnnString, "--Select State Name--");
                     

        }     
       

        #endregion

      

        private void ShowGrid()
        {
            
            SqlConnection con = new SqlConnection(CnnString);
            SqlCommand com = new SqlCommand("Confirm_Hold_SalarySheet", con);
            com.CommandType = CommandType.StoredProcedure;      
            com.Parameters.AddWithValue("@Month_name", Convert.ToInt32(DDLMonth.SelectedValue.ToString()));
            com.Parameters.AddWithValue("@year", Convert.ToInt32(ddlyear.SelectedValue.ToString()));
            com.Parameters.AddWithValue("@Company_Name", DDlCompany_Name.SelectedValue.ToString());
            com.Parameters.AddWithValue("@stateid", Convert.ToInt32(DDlState_Name.SelectedValue.ToString()));
           
            con.Open();
            SqlDataReader dr = com.ExecuteReader();
            dt.Load(dr);
            con.Close();
           
         
           
            if (dt.Rows.Count > 0)
            {

                btnConfirm.Visible = true;
                GridView1.Visible = true;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                btnConfirm.Visible = false;
                lblErrMsg.Text = "Record not found .";
                GridView1.DataSource = null;
                GridView1.DataBind();

            }


        }
        StringBuilder sb = new StringBuilder();
        CheckBox chk;


        protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk1 = GridView1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
            if (chk1.Checked == true)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = true;
                }
            }
            else if (chk1.Checked == false)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = false;
                }
            }

        }


        protected void Grid1_ItemCommand(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {


        }
        
        



        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                LblMessage.Text = "";

                if (DDlState_Name.SelectedItem.Text.ToString().Equals("--Select State Name--"))
                {
                    LblMessage.Visible = false;
                    lblErrMsg.Text = "Please Select State Name..";
                    return;
                }

                if (DDlCompany_Name.SelectedItem.Text.ToString().Equals("--Select Company Name--"))
                {
                    LblMessage.Visible = false;
                    lblErrMsg.Text = "Please Select Company Name..";
                    return;
                }

               

                if (DDLMonth.SelectedItem.Text.ToString().Equals("--Select Month--"))
                {
                    lblErrMsg.Visible = true;
                    LblMessage.Visible = false;
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = "Please Select  Month Name...";
                    return;
                }

                if (ddlyear.SelectedItem.Text.ToString().Equals("--Select Year--"))
                {
                    lblErrMsg.Visible = true;
                    LblMessage.Visible = false;
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = "Please Select  Year";
                    return;
                }

                lblErrMsg.Text = "";
                txtepcode.Text = "";


                ShowGrid();

               


            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }


        }
      
        DataTable dt = new DataTable();



      



        public void Showgridfun()
        {
            try
            {
                    if (GridView1.Rows.Count == 0)
                    {
                        lblErrMsg.Text = "No Record Found.";
                        return;

                    }

                    int ChkBoxCount = 0;
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                        if (chk.Checked == true)
                        {
                            ChkBoxCount = ChkBoxCount + 1;
                        }
                    }
                    if (ChkBoxCount == 0)
                    {
                        lblErrMsg.Text = "Please select atleast 1 record.";
                        return;
                    }                 
                   
                   
               
                int iChkCount = 0;
                int k = 0;
                StringBuilder sbx = new StringBuilder();
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                    if (chk.Checked == true)
                    {
                        iChkCount = iChkCount + 1;
                                             
                        string strempcode = GridView1.DataKeys[i]["EmpCode"].ToString();
                        
                        sbx.Append("Update  SalarySheet set  status ='Release' , SalayConfirmBy =" + Convert.ToInt32(strUserID) + " , salaryConfirmDate = getDate() where Month_Name = " + Convert.ToInt32(DDLMonth.SelectedValue.ToString()) + " and year=" + Convert.ToInt32(ddlyear.SelectedValue.ToString()) + " and EmpCode='" + strempcode + "' and CompanyName ='" + DDlCompany_Name.SelectedValue.ToString() + "' and status ='Hold' ;");

                    }
                }
                if (iChkCount > 0)
                {
                 k= Utils.ExecNonQuery(sbx.ToString(), CnnString);

                }
                if (k > 0)
                {
                    lblErrMsg.Visible = false;
                    LblMessage.Visible = true;
                    lblErrMsg.Text = "";
                    LblMessage.Text = " No of " + k + " Employee Salary Status Confirmed.";
                  
                    //
                    SqlConnection con = new SqlConnection(CnnString);
                    SqlCommand com = new SqlCommand("Confirm_Hold_SalarySheet", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Month_name", Convert.ToInt32(DDLMonth.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@year", Convert.ToInt32(ddlyear.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@Company_Name", DDlCompany_Name.SelectedValue.ToString());
                    com.Parameters.AddWithValue("@stateid", Convert.ToInt32(DDlState_Name.SelectedValue.ToString()));

                    con.Open();
                    SqlDataReader dr = com.ExecuteReader();
                    dt.Load(dr);
                    con.Close();



                    if (dt.Rows.Count > 0)
                    {

                        btnConfirm.Visible = true;
                        GridView1.Visible = true;
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                    //

                   
                }
                else {
                    lblErrMsg.Visible = true;
                    LblMessage.Visible = false;
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = " Employee Salary Status Not Confirmed.";
                    return;
                    
                }
               

             }

            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }

        }
      

        protected void btnHold_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                LblMessage.Text = "";

                if (DDlState_Name.SelectedItem.Text.ToString().Equals("--Select State Name--"))
                {
                    LblMessage.Visible = false;
                    lblErrMsg.Text = "Please  State Name..";
                    return;
                }
                

                if (DDlCompany_Name.SelectedItem.Text.ToString().Equals("--Select Company Name--"))
                {
                    LblMessage.Visible = false;
                    lblErrMsg.Text = "Please Select Company Name..";
                    return;
                }

                if (DDLMonth.SelectedItem.Text.ToString().Equals("--Select Month--"))
                {
                    lblErrMsg.Visible = true;
                    LblMessage.Visible = false;
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = "Please Select  Month Name...";
                    return;
                }

                if (ddlyear.SelectedItem.Text.ToString().Equals("--Select Year--"))
                {
                    lblErrMsg.Visible = true;
                    LblMessage.Visible = false;
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = "Please Select  Year";
                    return;
                }

                if ( string.IsNullOrEmpty(txtepcode.Text.Trim().ToString()))
                {
                    lblErrMsg.Visible = true;
                    LblMessage.Visible = false;
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = "Please Enter Emp Code..";
                    return;
                }

               string query = " Update  SalarySheet set  status = 'Hold' , SalayConfirmBy =" + Convert.ToInt32(strUserID) + " , salaryConfirmDate = getDate() where Month_Name = " + Convert.ToInt32(DDLMonth.SelectedValue.ToString()) + " and year=" + Convert.ToInt32(ddlyear.SelectedValue.ToString()) + " and EmpCode='" + txtepcode.Text.Trim().ToString() + "' and CompanyName ='" + DDlCompany_Name.SelectedValue.ToString() + "' and status ='Release' ";
               int i =  Utils.ExecNonQuery(query,CnnString);
               if (i > 0)
                {
                    lblErrMsg.Visible = false;
                    LblMessage.Visible = true;
                    lblErrMsg.Text = "";
                    LblMessage.Text = "Employee Salary Hold Suceessfully..";
                    txtepcode.Text = "";
                    ShowGrid();
                    
                    return;
                }
                else
                {

                    lblErrMsg.Visible = true;
                    LblMessage.Visible = false;
                    LblMessage.Text = "";
                    lblErrMsg.Text = "Employee Salary Not Hold.. ";                   
                    return;
                }




            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           
           

        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            Showgridfun();
        }

        protected void DDlState_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = " select  CompanyName from hsrpstate where  hsrp_stateid =" +Convert.ToInt32(DDlState_Name.SelectedValue.ToString())+ " and ActiveStatus='Y' ";
            Utils.PopulateDropDownList(DDlCompany_Name, SQLString.ToString(), CnnString, "--Select Company Name--");
           

        }
        
            
         

    }
}