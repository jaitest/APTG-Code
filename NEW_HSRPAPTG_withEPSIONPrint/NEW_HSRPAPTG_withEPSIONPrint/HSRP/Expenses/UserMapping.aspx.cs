using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.Expenses
{
    public partial class UserMapping : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        string SQLString1 = string.Empty;

        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType1 = Convert.ToInt32(Session["UserType"]);
                CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID1 = Session["UID"].ToString();
                HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
                RTOLocationID1 = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {
                    try
                    {
                        if (UserType1.Equals(0))
                        {
                            FilldropDownListUserName();
                            FillEmployeeName();  
                            FillApprovalHead();
                        }
                        else
                        {
                            FilldropDownListUserName();
                            FillEmployeeName();
                            FillApprovalHead();
                        }
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        private void FilldropDownListUserName()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select u.userid,(u.UserFirstName+' '+u.UserLastName) as Username from users u   where  u.ActiveStatus = 'Y' and HSRP_StateID=" + HSRP_StateID1 + "  Order by u.UserFirstName";
                Utils.PopulateDropDownList(DDlUserName, SQLString1.ToString(), CnnString1, "--Select User Name--");
            }
            else
            {
                SQLString1 = "select u.userid,(u.UserFirstName+' '+u.UserLastName) as Username from users u   where   u.ActiveStatus  = 'Y' and  u.rtolocationid =" + RTOLocationID1 + " and  HSRP_StateID=" + HSRP_StateID1 + "  Order by u.UserFirstName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DDlUserName.DataSource = dts;
                DDlUserName.DataBind();
            }
        }

        private void FillApprovalHead()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select u.userid,(u.UserFirstName+' '+u.UserLastName) as Username from users u   where  u.ActiveStatus = 'Y' and HSRP_StateID=" + HSRP_StateID1 + "  Order by u.UserFirstName";
                Utils.PopulateDropDownList(DDLApprovalHead, SQLString1.ToString(), CnnString1, "--Select Approval Head--");
            }
            else
            {
                SQLString1 = "select u.userid,(u.UserFirstName+' '+u.UserLastName) as Username from users u   where   u.ActiveStatus  = 'Y' and  u.rtolocationid =" + RTOLocationID1 + " and  HSRP_StateID=" + HSRP_StateID1 + "  Order by u.UserFirstName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DDLApprovalHead.DataSource = dts;
                DDLApprovalHead.DataBind();
            }
        }

        private void FillEmployeeName()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select Emp_Id , Emp_name from employeemaster where Activestatus = 'Y' and HSRP_StateID=" + HSRP_StateID1 + " Order by Emp_name";
                Utils.PopulateDropDownList(DDlEmploeeName, SQLString1.ToString(), CnnString1, "--Select Employee Name --");
            }
            else
            {
                SQLString1 = "select Emp_Id , Emp_name from employeemaster where Activestatus = 'Y' and   HSRP_StateID=" + HSRP_StateID1 + " and rtolocationid =" + RTOLocationID1 + "   Order by Emp_name ";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DDlEmploeeName.DataSource = dts;
                DDlEmploeeName.DataBind();
            }
        }


        protected void btn_Click(object sender, EventArgs e)
        {
            Label1.Text = string.Empty;
            try
            {
                  if (DDlEmploeeName.SelectedItem.Text == "--Select Employee Name --")
                {
                    Label1.Visible = true;
                    Label1.Text = "Please Select Emploee Name.";
                    return;
                }
                  else if (DDlUserName.SelectedItem.Text == "--Select User Name--")
                {
                    Label1.Visible = true;
                    Label1.Text = "Please Select User Name.";
                    return;
                }
                else if (DDLApprovalHead.SelectedItem.Text == "--Select Approval Head--")
                {
                    Label1.Visible = true;
                    Label1.Text = "Please Select Approval Head.";
                    return;
                }
               

                else
                {
                    Label1.Visible = false;
                }

                SqlConnection con = new SqlConnection(CnnString1);
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                //change sp Name
                cmd = new SqlCommand("UserMapped", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@userId", Convert.ToInt32(DDlUserName.SelectedValue)));
                cmd.Parameters.Add(new SqlParameter("@approvalheadId", Convert.ToInt32(DDLApprovalHead.SelectedValue)));
                cmd.Parameters.Add(new SqlParameter("@empid", DDlEmploeeName.SelectedValue));
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    Label1.Visible = true;
                    Label1.Text = "User Mapping Successful.";
                }
                else {
                    Label1.Visible = true;
                    Label1.Text = "User Not Mapped.";
                }
                
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                #endregion
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

    }
}