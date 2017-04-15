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
    public partial class LoginMac : System.Web.UI.Page
    {
        string CnnString = String.Empty;
        string BrowserName = HttpContext.Current.Request.Browser.Type;
        string ClientOSName = HttpContext.Current.Request.Browser.Platform;
        string computername = Environment.MachineName;
        String MacAddress = String.Empty;
        string userid = string.Empty;
        string HSRPStateID = string.Empty;
        string UserType = string.Empty;
        string dbUserName = string.Empty;
        string strUserName = string.Empty;
        string strPassword = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cmpmacaddress = Utils.GetMACAddress2();
            MacAddress = cmpmacaddress.Trim();
            strUserName = txtUserID.Text.ToString().Trim();
            strPassword = txtUserPassword.Text.ToString().Trim();
             //string strquerys = "Select macaddress from users where userloginname='" + strUserName.ToString() + "' and password='" + txtUserPassword.ToString() + "'";
             //DataTable dts1 = Utils.GetDataTable(strquerys, CnnString);
             //if (dts1.Rows.Count > 0)
             //{
             //    //string UserName = dts.Rows[0]["UserFirstName"].ToString() + " " + dts.Rows[0]["UserLastName"].ToString();
             //    Utils.user_EmpMacaddress(,CnnString);
                 
             //}

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            string strquerys = "Select * from users where userloginname='" + strUserName + "' and password='" + strPassword + "'";
            DataTable dts= Utils.GetDataTable(strquerys, CnnString);
            if (dts.Rows.Count > 0)
            {
                string UserName = dts.Rows[0]["UserFirstName"].ToString() + " " + dts.Rows[0]["UserLastName"].ToString();
                Utils.user_log_attandanc(dts.Rows[0]["userid"].ToString(), dts.Rows[0]["HSRP_StateID"].ToString(), UserName, Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, computername, CnnString);
            }
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
            try
            {
                string strDefaultPage = string.Empty;
                
                string strPassword = string.Empty;
                string SQLString = string.Empty;
                string ActiveStatus = string.Empty;
             
                string UserName = string.Empty;
                string RTOLocationID = string.Empty;
                string RTOLocationName = string.Empty;
                string dbPassword = string.Empty;
                
                string FirstLoginStatus = string.Empty;
                string macbaseflag = string.Empty;
                string LastloggedinDatetime = string.Empty;
               
                string macb = string.Empty;
                string dealerid = string.Empty;
                strPassword = txtUserPassword.Text.ToString();
               
                if (string.IsNullOrEmpty(strUserName) || string.IsNullOrEmpty(strPassword))
                {
                    lblMsgRed.Text = "Please provide login details.";
                    txtUserID.Focus();
                    return;
                }

                //if (string.IsNullOrEmpty(MacAddress.Trim()))
                    if (MacAddress.ToString() != "")
                    {
                        Utils.user_log_attandanc(userid, HSRPStateID, dbUserName, Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, computername, CnnString);                        
                    }

                //SQLString = "Select * From Users where UserLoginName='" + strUserName + "'";
                //dt = Utils.GetDataTable(SQLString, CnnString);
                //if (dt.Rows[0]["withoutmac"].ToString().ToUpper() == "Y")
                //{
                //    macb = cmpmacaddress;// "CAF8DA35332B";
                //}
                //else
                //{
                //    macb = Request.QueryString["X"].ToString();
                //}

                SQLString = "Select top 1 * From Users u , UserLog_System mcb  where u.hsrp_stateid=mcb.Hsrp_stateid and u.UserLoginName='" + strUserName + "' and  mcb.macAddress='" + MacAddress + "' and activestatus='Y'";

                Utils dbLink = new Utils();
                dbLink.strProvider = CnnString;
                dbLink.CommandTimeOut = 600;
                dbLink.sqlText = SQLString.ToString();
                SqlDataReader PReader = dbLink.GetReader();

                if (PReader.HasRows)
                {
                    while (PReader.Read())
                    {
                        ActiveStatus = PReader["ActiveStatus"].ToString();
                        userid = PReader["UserID"].ToString();
                        HSRPStateID = PReader["HSRP_StateID"].ToString();
                        RTOLocationID = PReader["RTOLocationID"].ToString();
                        dealerid = PReader["dealerid"].ToString(); 
                        UserType = PReader["UserType"].ToString();
                        dbUserName = PReader["UserFirstName"].ToString() + " " + PReader["UserLastName"].ToString();
                        dbPassword = PReader["Password"].ToString();
                        FirstLoginStatus = PReader["FirstLoginStatus"].ToString();
                        strDefaultPage = PReader["DefaultPage"].ToString();
                        macbaseflag = PReader["withoutMAC"].ToString();
                        MacAddress = PReader["macAddress"].ToString();
                        Session["macbaseflag"] = macbaseflag;
                    }

                   

                }
                else
                {
                    txtUserPassword.Text = string.Empty;
                    lblMsgRed.Visible = true;
                    lblMsgRed.Text = "Your credential did not matched.";
                    txtUserPassword.Focus();
                    return;
                }

                PReader.Close();
                dbLink.CloseConnection();
                if (strPassword.Equals(dbPassword))
                {
                        SQLString = "select RTOLocationName from rtolocation where hsrp_stateid='" + HSRPStateID + "' and rtolocationid='" + RTOLocationID + "'";
                        DataTable dtrtoname = Utils.GetDataTable(SQLString, CnnString);
                        RTOLocationName = dtrtoname.Rows[0]["RTOLocationName"].ToString();
                        Session["UID"] = userid;
                        Session["dealerid"] = dealerid;
                        Session["UserHSRPStateID"] = HSRPStateID;
                        Session["UserRTOLocationID"] = RTOLocationID;
                        Session["UserType"] = UserType;
                        Session["UserName"] = dbUserName;
                        Session["RTOLocationName"] = RTOLocationName;

                        if (FirstLoginStatus == "Y")
                        {
                            Response.Redirect("~/FirstLoginChangePassword.aspx", true);
                        }

                        if (macbaseflag.Trim() == "N" || macbaseflag.Trim() == "Y")
                        {

                            //MacAddress = MacAddress.ToString();                          
                            Session["MacAddress"] = MacAddress;
                            SQLString = "Select * From UserLog_System where MacAddress ='" + MacAddress + "'";
                            kk = Utils.getDataSet(SQLString, CnnString);
                            //Utils.ExecNonQuery("UPDATE UserLog_System set LoggedInUserID='0', LastLoggedInDatetime =Getdate() where  loggedinUserID is null and LastLoggedInDatetime is null", CnnString);

                           // SQLString = "select count(*) as Records from MACBase where  DATEDIFF(mi, LastLoggedInDatetime, GETDATE()) >30";
                            //int getcounts = Utils.getScalarCount(SQLString, CnnString);
                            //if (getcounts > 0)
                            //{
                            //    Utils.ExecNonQuery("UPDATE MACBase set LoggedInUserID='', LastLoggedInDatetime ='' where  DATEDIFF(mi, LastLoggedInDatetime, GETDATE()) >30", CnnString);
                            //}
                            Session["SaveMacAddress"] = kk.Tables[0].Rows[0]["MacAddress"].ToString();                           
                            //MacAddress = cmpmacaddress.ToString();                               
                          

                            Utils.ExecNonQuery("UPDATE users set lastLoginDatetime=GetDate() where UserLoginName='" + strUserName + "'", CnnString);

                            String Sq = "select  DATEDIFF(mi, CreatedDatetime, GETDATE()) as record, MacAddress from UserLog_System where userid='" + userid + "'";
                          
                            DataTable checkLogin = Utils.GetDataTable(Sq, CnnString);
                            if (checkLogin.Rows.Count > 0)
                            {
                                int csd = Convert.ToInt16(checkLogin.Rows[0]["record"].ToString());
                                String MacDataUser = checkLogin.Rows[0]["MacAddress"].ToString();
                                if (csd < 10005)
                                {
                                    if (MacDataUser == MacAddress)
                                    {
                                        Utils.user_log_attandanc(userid, HSRPStateID, dbUserName, Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, computername, CnnString);                                       
                                        Response.Redirect(strDefaultPage, true);
                                        lblMsgRed.Text = string.Empty;
                                    }
                                    else
                                    {
                                        CreateDuplicateLogin(userid);
                                        lblMsgRed.Text = "You are Already Loggged In";
                                        lblMsgRed.Visible = true;
                                    }
                                }
                                else
                                {
                                   
                                    Utils.user_log_attandanc(userid, HSRPStateID, dbUserName, Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, computername, CnnString);
                                    Response.Redirect(strDefaultPage, true);
                                    lblMsgRed.Text = string.Empty;
                                }
                            }
                            else
                            {

                                Utils.user_log_attandanc(userid, HSRPStateID, dbUserName, Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, computername, CnnString);                               
                                Response.Redirect(strDefaultPage, true);
                                lblMsgRed.Text = string.Empty;
                            }

                        
                        }

                   // }
                }

                else
                {
                    if (string.IsNullOrEmpty(Session["LoginAttempts"].ToString()))
                    {
                        Session["LoginAttempts"] = 1;
                        lblMsgRed.Visible = true;
                        lblMsgRed.Text = "Your credential did not matched.";
                        txtUserPassword.Focus();
                    }
                    if (Session["LoginAttempts"].ToString().Equals("5"))
                    {
                        SQLString = "Update Users Set ActiveStatus='N' where UserLoginName='" + strUserName + "'";
                        Utils.ExecNonQuery(SQLString, CnnString);

                        lblMsgRed.Text = "Your Account is blocked : Contact Admin.";

                    }
                    else
                    {
                        lblMsgRed.Visible = true;
                        lblMsgRed.Text = "Your credential did not matched.";
                        Session["LoginAttempts"] = Convert.ToInt32(Session["LoginAttempts"].ToString()) + 1;
                        txtUserPassword.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                    lblMsgRed.Text = "Contact Administrator : " + ex.Message.ToString();
            }
        }
        string logmacbaseID;
        private void CreateDuplicateLogin(string userid)
        {
            if (dt.Rows[0]["withoutmac"].ToString() == "Y")
            {
                logmacbaseID = "CAF8DA35332B";
            }
            else
            {
                logmacbaseID = Request.QueryString["X"].ToString();
            }
            string sqle = "Insert into DuplicateLoginLog (userID,UserName,Password,IP,MacbaseID) values ('" + userid + "','" + txtUserID.Text + "','" + txtUserPassword.Text + "','" + 11 + "','" + logmacbaseID + "')";
            Utils.ExecNonQuery(sqle, CnnString);
        }

        protected void LinkButtonForget_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPass.aspx", false);
        }


    }
}