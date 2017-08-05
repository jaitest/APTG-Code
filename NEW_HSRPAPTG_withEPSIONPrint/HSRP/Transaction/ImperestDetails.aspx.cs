using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class ImperestDetails : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string Mode = string.Empty;
        string ImperestID = string.Empty;
        int UserType;
        int HSRP_StateID;
        int RTOLocationID;
        int intHSRP_StateID;
        int intRTOLocationID;
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtAmount.Attributes.Add("onKeydown", "return MaskMoney(event)");
            //ScriptManager.GetCurrent(this).RegisterPostBackControl(this.btnSave);
            //Page.Form.Attributes.Add("enctype", "multipart/form-data");

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                HSRP_StateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                Mode = Request.QueryString["Mode"].ToString();


                if (!IsPostBack)
                {
                    try
                    {
                        InitialSetting();
                       
                        if (UserType.Equals(0))
                        {
                            dropDownListOrg.Visible = true;
                            FilldropDownListOrganization();
                            dropDownListClient.Visible = true;
                            FilldropDownListClient();
                        }
                        //else if (UserType.Equals(1))
                        //{
                        //    dropDownListOrg.Visible = true;
                        //    dropDownListClient.Visible = true;
                        //    FilldropDownListClient();
                        //}
                        else
                        {
                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            dropDownListOrg.Visible = true;
                            dropDownListClient.Visible = true;
                        }
                        if (Mode == "Edit")
                        {
                            LabelFormName.Text = "Update Imprest Details";
                            ImperestID = Request.QueryString["ImperestID"].ToString();
                            btnSave.Visible = false;
                            btnUpdate.Visible = true;

                            SQLString = "select * from ImperestDetails where ImperestID='" + ImperestID + "'";
                            DataTable dt = Utils.GetDataTable(SQLString, CnnString);

                            if (dt.Rows.Count > 0)
                            {
                                dropDownListOrg.SelectedValue = dt.Rows[0]["HSRPStateID"].ToString();

                                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + dt.Rows[0]["HSRPStateID"].ToString() + " and ActiveStatus!='N' Order by RTOLocationName";
                                SQLString = "select RTOLocationName,distRelation from employeelocation Where HSRP_StateID=" + dt.Rows[0]["HSRPStateID"].ToString() + " and ActiveStatus='Y' Order by RTOLocationName";
                                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
                                dropDownListClient.SelectedValue = dt.Rows[0]["RTOLocationID"].ToString();

                                //SQLString = "select UserID,ISNULL(UserFirstName,'')+Space(2)+ISNULL(UserLastName,'') as Names from Users where HSRP_StateID='" + dt.Rows[0]["HSRPStateID"].ToString() + "' and RTOLocationID=" + dt.Rows[0]["RTOLocationID"].ToString() + "  order by UserFirstName";
                                SQLString = "select ID as 'UserID',Emp_Name as 'Names' from employeemaster Where HSRP_StateID=" + dt.Rows[0]["HSRPStateID"].ToString() + " and RTOLocationID='" + dt.Rows[0]["RTOLocationID"].ToString() + "' and ActiveStatus='Y' and isnull(userid,'')!='' Order by Emp_Name";
                                //SQLString = "select ID as 'UserID',Emp_Name as 'Names' from employeemaster Where HSRP_StateID=" + dt.Rows[0]["HSRPStateID"].ToString() + " and RTOLocationID='" + dt.Rows[0]["HSRPStateID"].ToString() + "' and ActiveStatus='Y' and isnull(userid,'')!='' Order by Emp_Name";
                                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                                Utils.PopulateDropDownList(dropDownListUser, SQLString, CnnString, "--Select User--");
                                dropDownListUser.SelectedValue = dt.Rows[0]["UserID"].ToString();

                                txtAmount.Text = dt.Rows[0]["ImperestAmt"].ToString().Trim();
                               
                                //DateTime dojdate;
                                //double dResult;

                                //if (double.TryParse(dt.Rows[0]["DateOfIssue"].ToString(), out dResult))
                                //{
                                //    dojdate = DateTime.FromOADate(dResult);
                                //    OrderDate.SelectedDate = dojdate;
                                //} 

                                //OrderDate.SelectedDate = DateTime.Parse(dt.Rows[0]["DateOfIssue"].ToString().Trim());
                                txtRemarks.Text = dt.Rows[0]["Remarks"].ToString().Trim();
                                

                            }

                        }

                       
                        else
                        {
                            LabelFormName.Text = "Add Imprest Details";
                            btnSave.Visible = true;
                            btnUpdate.Visible = false;
                        }
                      
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load " + err.Message.ToString();
                    }


                }
            }
        }
        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            //OrderDate.AllowWeekSelection = true;

            // OrderDate.MinDate = (DateTime.Parse(TodayDate)).AddDays(-7.00);
            int remaindate = System.DateTime.Now.Day - 1;
            OrderDate.MinDate = (DateTime.Parse(TodayDate)).AddDays(-remaindate);
            //OrderDate.AllowDaySelection = true;
        }

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + "Order by HSRPStateName";
            }
            Utils.PopulateDropDownList(dropDownListOrg, SQLString.ToString(), CnnString, "--Select State--");
            dropDownListOrg.SelectedIndex = 0;
        }

        private void FilldropDownListClient()
        {
            try
            {
                if (UserType.Equals(0))
                {
                    int.TryParse(dropDownListOrg.SelectedValue, out intHSRP_StateID);
                    SQLString = "select RTOLocationName,distRelation from employeelocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus='Y' Order by RTOLocationName";
                    DataSet dss = Utils.getDataSet(SQLString, CnnString);
                    dropDownListClient.DataSource = dss;
                    dropDownListClient.DataTextField = "RTOLocationName";
                    dropDownListClient.DataValueField = "distRelation";
                    dropDownListClient.DataBind();
                    dropDownListClient.Items.Insert(0, new ListItem("--Select RTO--", "--Select RTO--"));
                    //Utils.PopulateDropDownList(ddlrtolocation, SQLString.ToString(), ConnectionString, "--Select RTO--");

                }
                else
                {
                    if (HSRP_StateID == 11 || HSRP_StateID == 9)
                    {
                        string UserID = Convert.ToString(Session["UID"]);
                        intHSRP_StateID = HSRP_StateID;
                        SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.distRelation FROM UserRTOLocationMapping INNER JOIN employeelocation a ON UserRTOLocationMapping.RTOLocationID = a.distRelation and a.activestatus='Y' where UserRTOLocationMapping.UserID='" + UserID + "' order by a.RTOLocationName";
                        //SQLString = "select RTOLocationName,distRelation from employeelocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus='Y' Order by RTOLocationName";
                        DataSet dss = Utils.getDataSet(SQLString, CnnString);
                        dropDownListClient.DataSource = dss;
                        dropDownListClient.DataTextField = "RTOLocationName";
                        dropDownListClient.DataValueField = "distRelation";
                        dropDownListClient.DataBind();
                        dropDownListClient.Items.Insert(0, new ListItem("--Select RTO--", "--Select RTO--"));
                        //Utils.PopulateDropDownList(ddlrtolocation, SQLString.ToString(), ConnectionString, "--Select RTO--");

                    }
                    else
                    {
                        //intHSRP_StateID = HSRP_StateID;
                        //SQLString = "select RTOLocationName,distRelation from employeelocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus='Y' Order by RTOLocationName";
                        //DataSet dss = Utils.getDataSet(SQLString, CnnString);
                        //dropDownListClient.DataSource = dss;
                        //dropDownListClient.DataTextField = "RTOLocationName";
                        //dropDownListClient.DataValueField = "distRelation";
                        //dropDownListClient.DataBind();
                        //dropDownListClient.Items.Insert(0, new ListItem("--Select RTO--", "--Select RTO--"));
                    }
                }
            }
            catch (Exception ex)
            {

                lblErrMsg.Text = ex.Message;
            }
        }

        //private void FilldropDownListClient()
        //{
        //    if (UserType.Equals(0))
        //    {
        //        int.TryParse(dropDownListOrg.SelectedValue, out intHSRP_StateID);
        //        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus!='N' Order by RTOLocationName";
        //    }

        //    else
        //    {
        //        // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
        //        SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' Order by RTOLocationName ";

        //        DataSet dss = Utils.getDataSet(SQLString, CnnString);
        //        dropDownListClient.DataSource = dss;
        //        dropDownListClient.DataBind();
        //    }
        //    //else
        //    //{
        //    //    intHSRP_StateID = HSRP_StateID;
        //    //    SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus!='N' Order by RTOLocationName";
        //    //}
        //    //Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
        //    ///dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
        //}

        protected void dropDownListOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListOrg.SelectedItem.Text != "--Select State--")
            {
                dropDownListClient.Visible = true;
                FilldropDownListClient();
            }
            //else
            //{
            //    lblErrMsg.Text = string.Empty;
            //    lblErrMsg.Text = "Please Select State.";
            //    dropDownListClient.Visible = false;
            //}
        }

        private void FilldropDownListUser()
        {
            int intHSRPStateID;
            int intRTOLocationID;
            //if (UserType == 0)
            //{
            //    int.TryParse(dropDownListOrg.SelectedValue, out intHSRPStateID);
            //    int.TryParse(dropDownListClient.SelectedValue, out  intRTOLocationID);
            //}
            //else if (UserType == 1)
            //{
            //    intHSRPStateID = Convert.ToInt32(HSRP_StateID);
            //    int.TryParse(dropDownListClient.SelectedValue, out  intRTOLocationID);
            //}
            //else
            //{
            //    intHSRPStateID = Convert.ToInt32(HSRP_StateID);
            //    intRTOLocationID = Convert.ToInt32(RTOLocationID);
            //}
            intHSRPStateID = Convert.ToInt32(HSRP_StateID);
            intRTOLocationID = Convert.ToInt32(RTOLocationID);

            //SQLString = "select UserID,ISNULL(UserFirstName,'')+Space(2)+ISNULL(UserLastName,'') as Names from Users where HSRP_StateID='" + intHSRPStateID + "' and RTOLocationID=" + intRTOLocationID + "  order by UserFirstName";
            SQLString = "select ID as 'UserID',Emp_Name as 'Names' from employeemaster Where HSRP_StateID='" + dropDownListOrg.SelectedValue.ToString() + "' and RTOLocationID='" + dropDownListClient.SelectedValue.ToString() + "' and ActiveStatus='Y' and isnull(userid,'')!='' Order by Emp_Name ";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(dropDownListUser, SQLString, CnnString, "--Select User--");
            dropDownListUser.SelectedIndex = 0;
        }

        #endregion
        protected void btnSave_Click(object sender, EventArgs e)
        {
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            String From1 = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            DateTime BillDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
            


                SQLString = "Insert Into ImperestDetails (HSRPStateID,RTOLocationID,UserID,ImperestAmt,DateOfIssue,Remarks) VALUES ('" + dropDownListOrg.SelectedValue + "','" + dropDownListClient.SelectedValue + "','" + dropDownListUser.SelectedValue + "','" + txtAmount.Text + "','" + BillDate + "','" + txtRemarks.Text + "')";
                Utils.ExecNonQuery(SQLString, CnnString);
                BlankFields();
                lblSucMess.Text = "Record Saved Successfully";
            //}

           
        }

        private void BlankFields()
        {
            FilldropDownListUser();
            FilldropDownListClient();
            FilldropDownListOrganization();
            txtAmount.Text = "0";
            txtRemarks.Text = string.Empty;
            lblErrMsg.Text = string.Empty;
            lblSucMess.Text = string.Empty;
        }

        

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListClient.SelectedItem.Text != "--Select RTO--")
            {
                dropDownListUser.Enabled = true;
                lblErrMsg.Text = "";
                FilldropDownListUser();
                // UpdatePanelUser.Update();
            }
            else
            {
                dropDownListUser.Enabled = false;
                dropDownListUser.Enabled = false;
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Text = "Please Select Location Name";
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            ImperestID = Request.QueryString["ImperestID"].ToString();
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            String From1 = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            DateTime BillDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));

            SQLString = "UPDATE ImperestDetails SET HSRPStateID='" + dropDownListOrg.SelectedValue + "',RTOLocationID='" + dropDownListClient.SelectedValue + "',UserID='" + dropDownListUser.SelectedValue + "',ImperestAmt='" + txtAmount.Text + "',DateOfIssue='" + BillDate + "',Remarks='" + txtRemarks.Text + "' where ImperestID='" + ImperestID + "'";
            Utils.ExecNonQuery(SQLString, CnnString);
            lblSucMess.Text = "Record Saved Successfully";
        }
    }
}