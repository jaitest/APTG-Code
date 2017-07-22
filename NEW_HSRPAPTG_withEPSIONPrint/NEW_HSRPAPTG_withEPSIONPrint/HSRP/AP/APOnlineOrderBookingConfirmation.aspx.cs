using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
using AjaxControlToolkit;
using System.Data.SqlClient;
using System.Configuration;

namespace HSRP.AP
{


    public partial class APOnlineOrderBookingConfirmation : System.Web.UI.Page
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
            }

        }

        

        public void buildGrid()
        {
            string dealerid = string.Empty;
            try
            {
                string SQLString = "select hsrprecordid,hsrprecord_Authorizationno,vehicleregno,[OwnerName],[DealerName],OnlinePaymentID,PaymentGateway,OnlinePaymentTrackingNo from APOnlinePaymentOrderBooked where hsrprecord_Authorizationno='" + txtAuthono.Text + "' and ReSyncStatus='N' and convert(date,[HSRPRecord_CreationDate])=convert(date,getdate())";
                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                if (dt.Rows.Count > 0)
                {
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
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void grdid_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);

            if (e.CommandName == "Approval")
            {
                LinkButton lnkApproval = (LinkButton)e.CommandSource;
                string hsrprecordid = lnkApproval.CommandArgument;

                if (hsrprecordid != "")
                {
                    string sqlquery1 = "update APOnlinePaymentOrderBooked set ReSyncStatus='Y' , ConfirmationDoneBy='" + strUserID + "'  where hsrprecordid='" + hsrprecordid + "'";
                    int i = Utils.ExecNonQuery(sqlquery1, CnnString);
                    if (i > 0)
                    {
                        int c = Utils.ExecNonQuery("GetAPOnlinePaymentOrderBookingHSRP '" + hsrprecordid + "'", CnnString);
                        if (c > 0)
                        {
                            lblErrMsg.Text = "";
                            lblSucMess.Text = "Order Booked..";
                            buildGrid();
                        }
                        else 
                        {
                            lblSucMess.Text = "";
                            lblErrMsg.Text = "Order Not Booked..";
                        }
                    }
                    else
                    {
                        lblSucMess.Text = "";
                        lblErrMsg.Text = "Order Not Booked..";
                    }

                }
            }
            if (e.CommandName == "Reject")
            {
                LinkButton lnkApproval = (LinkButton)e.CommandSource;
                string hsrprecordid = lnkApproval.CommandArgument;
                if (hsrprecordid != "")
                {
                    DataTable dt1 = Utils.GetDataTable("Select hsrprecord_Authorizationno from APOnlinePaymentOrderBooked where hsrprecordid='" + hsrprecordid + "'", CnnString);
                    string sqlquery1 = String.Empty;
                    if (dt1.Rows.Count > 0)
                    {
                        string authno = dt1.Rows[0]["hsrprecord_Authorizationno"].ToString() + "R";
                        sqlquery1 = "update APOnlinePaymentOrderBooked set hsrprecord_Authorizationno='" + authno + "', ReSyncStatus='R' , ConfirmationDoneBy='" + strUserID + "'  where hsrprecordid='" + hsrprecordid + "'";
                    }
                    else
                    {
                        sqlquery1 = "update APOnlinePaymentOrderBooked set ReSyncStatus='R' , ConfirmationDoneBy='" + strUserID + "'  where hsrprecordid='" + hsrprecordid + "'";
                    }
                    int i = Utils.ExecNonQuery(sqlquery1, CnnString);
                    if (i > 0)
                    {
                        lblErrMsg.Text = "";
                        lblSucMess.Text = "Order Rejected..";
                        buildGrid();
                    }
                    else
                    {
                        lblSucMess.Text = "";
                        lblErrMsg.Text = "Order Not Rejected..";
                    }

                }
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
        
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchByAuthno(string prefixText, int count)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select hsrprecord_Authorizationno from APOnlinePaymentOrderBooked where hsrprecord_Authorizationno like @SearchText + '%'";
                    cmd.Parameters.AddWithValue("@SearchText", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    List<string> customers = new List<string>();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            customers.Add(sdr["hsrprecord_Authorizationno"].ToString());
                        }
                    }
                    conn.Close();
                    return customers;
                }
            }
        }
    
    
    
    }
}