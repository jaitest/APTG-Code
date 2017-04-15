using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
namespace HSRP.Expenses
{
    public partial class Attandanc : System.Web.UI.Page
    {
          string CnnString = String.Empty;
       // String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        
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
        string dbUserName = string.Empty;
        int intHSRPStateID;

        string BrowserName = HttpContext.Current.Request.Browser.Type;
        string ClientOSName = HttpContext.Current.Request.Browser.Platform;
        string computername = Environment.MachineName;
        String MacAddress = String.Empty;
        string strattandance = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
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
               
            }
            if (!Page.IsPostBack)
            {
                try
                {
                    if (UserType == "0")
                    {
                        FilldropDownListOrganization();
                        //FilldropDownListClient();
                        FilldropDowndistrictcenter();
                        FillUsername();
                        InitialSetting();
                    }
                    else
                    {
                        FilldropDownListOrganization();
                        FilldropDowndistrictcenter();
                        //FilldropDownListClient();
                        FillUsername();
                        //Attandanc_Grid();
                        InitialSetting();
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
              
            }
        }
          string cmpmacaddress = Utils.GetMACAddress2();
         // MacAddress = cmpmacaddress.Trim();

        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();        
        }

        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");             
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }

        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
               // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName asc";
                string SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and District='" + ddldistrictname.SelectedValue.ToString() + "' and  ActiveStatus!='N'   Order by RTOLocationName asc";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                //string SQLString11 = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and District='" + ddldistrictname.SelectedValue.ToString() + "' and  ActiveStatus!='N'   Order by RTOLocationName asc";
                 SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' and a.District='" + ddldistrictname.SelectedValue.ToString() + "' Order by RTOLocationName asc ";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                dropDownListClient.Items.Insert(0, new ListItem("--Select Location Name--"));

                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();
                    }

                }
            }
        }

        private void FilldropDowndistrictcenter()
        {
            if (UserType.Equals(0))
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                string SQLString11 = "select distinct district,district from rtolocation where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' and EmbCenterName is not null group by district,district Order by 1";
                //string SQLString11 = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName asc";
                Utils.PopulateDropDownList(ddldistrictname, SQLString11.ToString(), CnnString, "--Select District Name--");
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                //string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";

                //string SQLString11 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM Employeemaster e INNER JOIN RTOLocation a ON e.RTOLocationID = a.RTOLocationID where a.Hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and EmbCenterName='" + ddldistrictname.SelectedValue.ToString() + "' and e.Activestatus='Y'  Order by RTOLocationName asc ";
                string SQLString11 = "select distinct district ,district from rtolocation where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N' and EmbCenterName is not null group by district,district Order by 1";
                System.Data.DataTable dt1 = Utils.GetDataTable(SQLString11, CnnString);
                ddldistrictname.DataSource = dt1;
                ddldistrictname.DataBind();
                ddldistrictname.Items.Insert(0, new ListItem("--Select District Name--"));
            }
        } 

       
        public void FillUsername()
        {
            //userid = ddlUserAccount.SelectedValue.ToString();
            //SQLString = "Select UserID,UserFirstName +' '+ UserLastName as username from [Users] where activestatus='Y' order by userfirstname";
            //SQLString = "Select Emp_id,Emp_Name from EmployeeMaster where hsrp_stateid='" + HSRP_StateID + "' and rtolocationid in('" + RTOLocationID + "') and UserId='" + UserName + "' and activestatus='Y' order by Emp_Name";
            SQLString = "Select Emp_id,Emp_Name from EmployeeMaster where hsrp_stateid='" + HSRP_StateID + "' and UserId='" + UserName + "' and activestatus='Y' order by Emp_Name";
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);            
            ddlUserAccount.DataSource = dt;
            ddlUserAccount.DataTextField = "Emp_Name";
            ddlUserAccount.DataValueField = "Emp_id";
            ddlUserAccount.DataBind();
            lblSucMess.Text = "";
            lblErrMess.Text = "";
            ddlUserAccount.Items.Insert(0, new ListItem("--Select username--"));
        }


        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                userid = ddlUserAccount.SelectedValue.ToString();
                dbUserName = ddlUserAccount.SelectedItem.Text.ToString();
                string indate = System.DateTime.Now.ToString(CultureInfo.InvariantCulture);
                string outdate = System.DateTime.Now.ToString(CultureInfo.InvariantCulture);
                MacAddress = cmpmacaddress.ToString().Trim();
                string strattancanc = "select Emp_ID from EmployeeMaster where Emp_ID='" + userid + "'";
                DataTable dt1 = Utils.GetDataTable(strattancanc, CnnString);
                if (dt1.Rows[0]["Emp_ID"].ToString().Trim() == userid.ToString())
                {
                    Utils.user_attandanc_in(userid, indate, outdate, HSRPStateID, Request.UserHostAddress.ToString(), MacAddress, BrowserName, ClientOSName, computername, "P", "G11", CnnString);

                    string strqueryupdate = "Update AttendanceLog set OutDateTime=Getdate() where Emp_ID='" + userid + "'";
                    Utils.ExecNonQuery(strqueryupdate, CnnString);

                    lblErrMess.Text = "";
                    lblSucMess.Text = "Employee Attendance " + dbUserName + " Saved";
                }
                else
                {
                    lblSucMess.Text = "";
                    lblErrMess.Text = "MacAddress Not Match";
                }
            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message.ToString();
            }

            //Showgridfun();
        }
        public void clear()
        {
            ddlUserAccount.ClearSelection();
           
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            clear();
        }

        public void Attandanc_Grid()
        {
            try
            {
                     userid = ddlUserAccount.SelectedValue.ToString();
                    //and  Emp_ID='" + userid + "'
                    // dbUserName = ddlUserAccount.SelectedItem.Text.ToString();
                    // select em.Rtolocationid,* from AttendanceLog al, employeeMaster em where al.hsrp_stateid=4  and al.Emp_id=em.Emp_id
                     string strquerydata = "select distinct Row_Number() over(order by Emp_ID) as SNo, Emp_ID,Emp_Name from employeeMaster where hsrp_stateid='" + HSRP_StateID + "' and Rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "' and activestatus='Y' order by 1";
                     //string strquerydata = "select distinct Row_Number() over(order by e.Emp_ID) as SNo,e.Emp_ID,Emp_Name,a.INDateTime,a.OutDateTime from employeeMaster e , attendancelog a where e.emp_id=a.emp_id and e.hsrp_stateid='" + HSRP_StateID + "'  and Rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "'";
                    // string strquerydata = "select em.Rtolocationid,al.Emp_ID,em.Emp_Name,al.InDateTime from AttendanceLog al, employeeMaster em where al.Emp_id=em.Emp_id and al.hsrp_stateid='" + HSRP_StateID + "' and em.Rtolocationid='" + dropDownListClient.SelectedValue.ToString()+ "' ";
                    DataTable dtgrid = Utils.GetDataTable(strquerydata, CnnString);
                    lblErrMess.Text = "";
                    lblSucMess.Text = "";
                    // gridTD.Visible = true;
                    grd.DataSource = dtgrid;
                    grd.DataBind();
                    //grd.Visible = true;
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void btnintime_Click(object sender, EventArgs e)
        {
            try
            {                              
                Showgrdintime();               
            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message.ToString();
            }
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            //btnintime.Enabled = false;
            //btnouttime.Enabled = false;
            //btnintime.Visible = false;
            //btnouttime.Visible = false;
            if (dropDownListClient.SelectedItem.Text.ToString().Trim() != "--Select Location Name--")
            {
               // Panel1.Visible = true;
                grd.Visible = true;
                Attandanc_Grid();                
                lblErrMess.Text = "";
                lblSucMess.Text = "";              
                btnintime.Visible = true;
                btnouttime.Visible = true;
               

            }
            else
            {
                //Panel1.Visible = false;
                grd.Visible = false;
                lblSucMess.Text = "";
                lblErrMess.Text = "";
                lblErrMess.Text = "Please select Location Name";
            }
        }


        StringBuilder sb = new StringBuilder();
        CheckBox chk;
        protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk1 = grd.HeaderRow.FindControl("CHKSelect") as CheckBox;
            if (chk1.Checked == true)
            {
                for (int i = 0; i < grd.Rows.Count; i++)
                {
                    chk = grd.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = true;
                }
            }
            else if (chk1.Checked == false)
            {
                for (int i = 0; i < grd.Rows.Count; i++)
                {
                    chk = grd.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = false;
                }
            }

        }

        public void Showgrdintime()
        {
            try
            {
                string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                string RtoName = string.Empty;
                RtoName = dropDownListClient.SelectedItem.ToString();
                HttpContext context = HttpContext.Current;    
                StringBuilder bb = new StringBuilder();

                //Opens the document:

                string vehicle = string.Empty;
                string strHsrpRecordId = string.Empty;
                string strInvoiceNo = string.Empty;
                string strEmbStationName = string.Empty;
                string strEmbAddress = string.Empty;
                string strEmbAddress1 = string.Empty;
                string strEmbCity = string.Empty;
                string strEmbId = string.Empty;

                #region Set ChallanNo
                string[] strArray;
                try
                {

                    if (grd.Rows.Count == 0)
                    {
                        lblErrMess.Text = "No Record Found.";
                        return;
                    }
                    // Validate checked recirds
                    int ChkBoxCount = 0;
                    for (int i = 0; i < grd.Rows.Count; i++)
                    {
                        chk = grd.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                        if (chk.Checked == true)
                        {
                            ChkBoxCount = ChkBoxCount + 1;

                        }
                    }
                    if (dropDownListClient.SelectedItem.Text.ToString().Trim() != "--Select Location Name--")
                    {
                        if (ChkBoxCount == 0)
                        {
                            lblErrMess.Text = "Please select atleast 1 record.";
                            return;
                        }
                    }
                    else
                    {
                        lblErrMess.Text = "Please Select Location Name";
                        return;
                    }

                #endregion
                    int iChkCount = 0;
                    StringBuilder sbx = new StringBuilder();
                    for (int i = 0; i < grd.Rows.Count; i++)
                    {
                        chk = grd.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                        if (chk.Checked == true)
                        {
                            iChkCount = iChkCount + 1;
                       

                            string strEmpId = grd.DataKeys[i]["Emp_ID"].ToString();
                           
                                strHsrpRecordId = strEmpId;
                               
                                string indate = System.DateTime.Now.ToString(CultureInfo.InvariantCulture);
                                string outdate = System.DateTime.Now.ToString(CultureInfo.InvariantCulture);                                
                                MacAddress = cmpmacaddress.ToString().Trim();
                                string strattancanc = "select Emp_ID from EmployeeMaster where Emp_ID='" + strHsrpRecordId + "'";
                                DataTable dt1 = Utils.GetDataTable(strattancanc, CnnString);
                                if (dt1.Rows[0]["Emp_ID"].ToString().Trim() == strHsrpRecordId.ToString())
                                {
                                    Utils.user_attandanc_in(strHsrpRecordId, indate, outdate, HSRPStateID, Request.UserHostAddress.ToString(), MacAddress, BrowserName, ClientOSName, computername, "P", "G11", CnnString);

                                   // string strqueryupdate = "Update AttendanceLog set OutDateTime=Getdate() where Emp_ID='" + strHsrpRecordId + "'";
                                    //Utils.ExecNonQuery(strqueryupdate, CnnString);

                                    lblErrMess.Text = "";
                                    lblSucMess.Text = "Employee Attendance Saved";
                                    chk.Checked = false;
                                    chk.Enabled = false;
                                    //btnintime.Enabled = false;
                                }
                                else
                                {
                                    lblSucMess.Text = "";
                                    lblErrMess.Text = "Employee Attendance Not Saved";
                                }
                           

                        }
                    }
                   

                }
                catch (Exception ex)
                {
                    lblErrMess.Text = ex.Message;
                }



            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message;
            }
        }


        public void Showgrdouttime()
        {
            try
            {
                string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                string RtoName = string.Empty;
                RtoName = dropDownListClient.SelectedItem.ToString();
                HttpContext context = HttpContext.Current;    
                StringBuilder bb = new StringBuilder();

                //Opens the document:

                string vehicle = string.Empty;
                string strHsrpRecordId = string.Empty;
                string strInvoiceNo = string.Empty;
                string strEmbStationName = string.Empty;
                string strEmbAddress = string.Empty;
                string strEmbAddress1 = string.Empty;
                string strEmbCity = string.Empty;
                string strEmbId = string.Empty;

                #region Set ChallanNo
                string[] strArray;
                try
                {

                    if (grd.Rows.Count == 0)
                    {
                        lblErrMess.Text = "No Record Found.";
                        return;
                    }
                    // Validate checked recirds
                    int ChkBoxCount = 0;
                    for (int i = 0; i < grd.Rows.Count; i++)
                    {
                        chk = grd.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                        if (chk.Checked == true)
                        {
                            ChkBoxCount = ChkBoxCount + 1;

                        }
                    }
                    if (dropDownListClient.SelectedItem.Text.ToString().Trim() != "--Select Location Name--")
                    {
                        if (ChkBoxCount == 0)
                        {
                            lblErrMess.Text = "Please select atleast 1 record.";
                            return;
                        }
                    }
                    else
                    {
                        lblErrMess.Text = "Please Select Location Name";
                        return;
                    }

                #endregion
                    int iChkCount = 0;
                    StringBuilder sbx = new StringBuilder();
                    for (int i = 0; i < grd.Rows.Count; i++)
                    {
                        chk = grd.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                        if (chk.Checked == true)
                        {
                            iChkCount = iChkCount + 1;
                       

                            string strEmpId = grd.DataKeys[i]["Emp_ID"].ToString();
                           
                                strHsrpRecordId = strEmpId;
                               
                                string indate = System.DateTime.Now.ToString(CultureInfo.InvariantCulture);
                                string outdate = System.DateTime.Now.ToString(CultureInfo.InvariantCulture);                                
                                MacAddress = cmpmacaddress.ToString().Trim();
                                //string strattancanc = "select Emp_ID from EmployeeMaster where Emp_ID='" + strHsrpRecordId + "'";
                                string strattancanc = "select top 1 e.Emp_ID,a.UID from EmployeeMaster e ,attendancelog a where e.Emp_ID=a.Emp_ID and e.Emp_ID='" + strHsrpRecordId + "' order by UID desc";
                                DataTable dt1 = Utils.GetDataTable(strattancanc, CnnString);
                                if (dt1.Rows[0]["Emp_ID"].ToString().Trim() == strHsrpRecordId.ToString())
                                {
                                    //Utils.user_attandanc_in(strHsrpRecordId, indate, outdate, HSRPStateID, Request.UserHostAddress.ToString(), MacAddress, BrowserName, ClientOSName, computername, "P", "G11", CnnString);

                                    string strqueryupdate = "Update AttendanceLog set OutDateTime=Getdate() where Emp_ID='" + strHsrpRecordId + "' and INDateTime=INDateTime and UID='" + dt1.Rows[0]["UID"].ToString().Trim() + "'";
                                    Utils.ExecNonQuery(strqueryupdate, CnnString);
                                    lblErrMess.Text = "";
                                    lblSucMess.Text = "Employee Attendance Saved";
                                    chk.Checked = false;
                                    chk.Enabled = false;

                                   // btnouttime.Enabled = false;
                                }
                                else
                                {
                                    lblSucMess.Text = "";
                                    lblErrMess.Text = "Employee Attendance Not Saved";
                                }
                            //}
                            //else
                            //{
                            //    lblErrMess.Text = "Emp Name Not Active This Location";
                            //    //strHsrpRecordId = strHsrpRecordId + "," + strEmpId;
                            //}

                        }
                    }
                   

                }
                catch (Exception ex)
                {
                    lblErrMess.Text = ex.Message;
                }



            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message;
            }
       
        }

        protected void btnouttime_Click(object sender, EventArgs e)
        {
            try
            {

                Showgrdouttime();
                //CheckBox chk1 = grd.HeaderRow.FindControl("CHKSelect") as CheckBox;
                //if (chk1.Checked == true)
                //{
                //    for (int i = 0; i < grd.Rows.Count; i++)
                //    {
                //        chk = grd.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                //        chk.Checked = false;
                //    }
                //}
                //btnintime.Enabled = true;
                //btnouttime.Enabled = false;
               // btnintime.Visible = true;
               // btnouttime.Visible = false;

                //string outtime, intime;
                //string ot = System.DateTime.Now.ToShortTimeString();
                //string od = System.DateTime.Now.ToShortDateString();
                //SqlCommand cmd1 = new SqlCommand("update tblnew set odate='" + od + "',otime='" + ot + "' where uname='" + TextBox1.Text + "' and ldate='" + od + "' ", dbclass.con);
                //cmd1.ExecuteNonQuery();
                //SqlCommand cmd2 = new SqlCommand("select otime from tblnew where odate='" + od + "'", dbclass.con);
                //outtime = cmd2.ExecuteNonQuery().ToString();
                //SqlCommand cmd3 = new SqlCommand("select ltime from tblnew where ldate='" + od + "'", dbclass.con);
                //intime = cmd3.ExecuteNonQuery().ToString();
                //double wrk1 = (Convert.ToDouble(outtime) - Convert.ToDouble(outtime));
                ////wrk = (outtime) - (intime);
                //TextBox1.Text = "";
                //TextBox2.Text = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddldistrictname_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FilldropDownListClient();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}