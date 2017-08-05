using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace HSRP
{
    public partial class Login : System.Web.UI.Page
    {
        string CnnString = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {          
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!IsPostBack)
            {
                txtUserID.Focus();
                Session["LoginAttempts"] = string.Empty;
            }
            lblMsgBlue.Text = String.Empty;
            lblMsgRed.Text = String.Empty;
        }

        DataSet kk;
        DataTable dt = new DataTable();
        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {              
               lblMsgRed.Text = "Please Contact System Administrator.";                   
        }
        //string logmacbaseID;
     

      

    }
}