using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;

namespace HSRP.AP
{


    public partial class APGetVehicleRegNoManully : System.Web.UI.Page
    {

        Utils bl = new Utils();
        string HSRPStateID = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string UserType1 = string.Empty;
        string dealerid = string.Empty;

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
                        

                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
                string path = Request.Url.ToString();
                if (path.Contains("msg") == true)
                {
                    if (Request.QueryString["msg"].ToString() == "Record Updated Successfully")
                    {
                        lblSucMess.Text = "Record Updated Successfully";
                    }
                    if (Request.QueryString["msg"].ToString() == "Record Not Updated")
                    {
                        lblSucMess.Text = "";
                        lblErrMsg.Text = "Record Not Updated";
                    }
                }
                
            }

        }



        public void buildGrid()
        {

            try
            {
                string strAuthno = string.Empty;
                string StrOwnerName = string.Empty;
                string StrRegistrationNo = string.Empty;
                string StrEngineNo = string.Empty;
                string StrChasisNo = string.Empty;
                HSRP.APWebrefrence.HSRPAuthorizationService objAPService = new HSRP.APWebrefrence.HSRPAuthorizationService();
                string AuthData = objAPService.GetHSRPAuthorizationno(txtAuthono.Text);
                if (AuthData == "1")
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Record not Found";
                    return;
                }
                else if (AuthData.Length > 1)
                {
                    using (StringReader stringReader = new StringReader(AuthData))
                    using (XmlTextReader reader = new XmlTextReader(stringReader))
                    {
                        while (reader.Read())
                        {
                            if (reader.Name.ToString() == "Authorization_Ref_no")
                            {
                                reader.Read();
                                strAuthno = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Owner_Name")
                            {
                                reader.Read();
                                StrOwnerName = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Reg_no")
                            {
                                reader.Read();
                                StrRegistrationNo = reader.Value.ToString().Trim();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Engine_No")
                            {
                                reader.Read();
                                StrEngineNo = reader.Value.ToString();
                                reader.Read();
                            }
                            if (reader.Name.ToString() == "Chassis_No")
                            {
                                reader.Read();
                                StrChasisNo = reader.Value.ToString();
                                reader.Read();
                            }

                        }
                    }
                    if (StrRegistrationNo.ToString() == "NOREGNNUMBER")
                    {
                        StrRegistrationNo = "";
                    }
                }
                if (StrRegistrationNo != "")
                {
                    string strCheck = StrRegistrationNo.Substring(0, 2);
                    if (strCheck != "AP" && strCheck != "ap")
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "Please Enter Valid Vehicle RegNo.";
                        return;
                    }
                    else 
                    {
                        
                        string SQLString = "select hsrprecordid,hsrprecord_Authorizationno,vehicleregno,[OwnerName],ChassisNo,EngineNo from hsrprecords where hsrprecord_Authorizationno='" + txtAuthono.Text + "'";
                        DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                        if (dt.Rows.Count == 1)
                        {
                            if (dt.Rows[0]["vehicleregno"].ToString() == "")
                            {
                                hdnvehicleregno.Value = StrRegistrationNo;
                                Label1.Text = StrRegistrationNo;
                                grdid.DataSource = dt;
                                grdid.DataBind();
                                grdid.Visible = true;
                                lblErrMsg.Text = "";
                            }
                            else
                            {
                                lblErrMsg.Visible = true;
                                lblErrMsg.Text = "Record not found";
                                grdid.DataSource = "";
                                grdid.DataBind();
                                grdid.Visible = true;
                            }
                        }
                        else
                        {
                            lblErrMsg.Visible = true;
                            lblErrMsg.Text = "Record not found";
                            grdid.DataSource = "";
                            grdid.DataBind();
                            grdid.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void grdid_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                if (e.CommandName == "Update")
                {
                    LinkButton lnkUpdate = (LinkButton)e.CommandSource;
                    string hsrprecordid = lnkUpdate.CommandArgument;

                    if (hsrprecordid != "")
                    {
                        DataTable dtname = Utils.GetDataTable("Select userfirstname +' '+ userlastname as 'Name' from users where userid='" + strUserID + "'", CnnString);
                        string remarks = string.Empty;
                        if (dtname.Rows.Count > 0)
                        {
                            remarks = "VehicleRegNo Updated By " + dtname.Rows[0]["Name"].ToString();
                        }
                        string sqlquery1 = "update hsrprecords set vehicleregno='" + hdnvehicleregno.Value + "',aptgvehrecdate=getdate(),internal_remarks2='" + remarks + "'  where hsrprecordid='" + hsrprecordid + "'";
                        int i = Utils.ExecNonQuery(sqlquery1, CnnString);
                        if (i > 0)
                        {
                            lblErrMsg.Text = "";
                            lblSucMess.Text = "Record Updated Successfully";
                            Response.Redirect("APGetVehicleRegNoManully.aspx?msg=" + lblSucMess.Text);
                        }
                        else
                        {
                            lblSucMess.Text = "";
                            lblErrMsg.Text = "Record Not Updated";
                            Response.Redirect("APGetVehicleRegNoManully.aspx?msg=" + lblErrMsg.Text);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                lblErrMsg.Text = ex.Message.ToString();
            }
        }

        protected void btngo_Click(object sender, EventArgs e)
        {
            if (txtAuthono.Text == "")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "";
                lblSucMess.Text = "";
                lblErrMsg.Text = "Please Enter Auth No.";
                return;
            }
            else
            {
                buildGrid();

            }
        }

        protected void grdid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}