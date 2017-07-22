﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DataProvider;


namespace HSRP.Expenses
{
    public partial class MessageNote : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;  
        string UserType = string.Empty;
        int HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOID;
        //int TickerID;
        string Mode;
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            if (!Page.IsPostBack)
            { 
                
                //Mode = Request.QueryString["Mode"].ToString(); 
                UserType = Session["UserType"].ToString();

                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                strUserID = Session["UID"].ToString();
              // int strUserID1 = Convert.ToInt32(Request.QueryString["UserID"]);
               // buttonUpdate.Visible = false;
                FilldropDownListOrganization();
                FilldropDownListClient();

                //if (Mode == "Edit")
                //{
                //    dropdownlistRtoName.Enabled = false;
                //    dropdownlistStateName.Enabled = false;
                //    //buttonUpdate.Visible = true;
                //   // btnSave.Visible = false;
                   
                //   // TickerID = Convert.ToInt16(Request.QueryString["TickerID"]);
                //    //MessageTickerEdit(TickerID);
                //}
                //else
                //{
                    
                //} 
            }
        }
     
        //protected void buttonUpdate_Click(object sender, EventArgs e)
        //{
        //    lblErrMess.Text = "";
        //    lblSucMess.Text = "";
        //     TickerID = Convert.ToInt16(Request.QueryString["TickerID"]);  
        //    int.TryParse(dropdownlistRtoName.SelectedValue, out intRTOID);
        //    int.TryParse(dropdownlistStateName.SelectedValue, out intHSRPStateID); 
        //    int IsExists = -1; 
        //    string Active;
        //    if (chkActiveStatus.Checked == true)
        //    {
        //        Active = "Y";
        //    }
        //    else
        //    {
        //        Active = "N";
        //    }
        //    DateTime UpdateDate = Convert.ToDateTime(System.DateTime.Now);
        //    BAL obj = new BAL();
        //    obj.UpdateHSRPMessageTicker(TickerID, textboxMessageText.Text, textboxMessageTextURL.Text, intHSRPStateID, intRTOID, UpdateDate, Active, ref IsExists);
        //    if (IsExists.Equals(1))
        //    {
        //        lblErrMess.Text = "Record Already Exist!!";
        //    }
        //    else
        //    { 
        //        lblSucMess.Text = "Record Update Successfully."; 
        //    }
        //}
         
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserHSRPStateID"].ToString() == "9" || Session["UserHSRPStateID"].ToString() == "11")
                {
                    strUserID = Session["UID"].ToString();
                    lblErrMess.Text = "";
                    lblSucMess.Text = "";
                    int IsExists = -1;
                    DateTime CreatedDateTime = System.DateTime.Now;
                    //BAL obj = new BAL();
                    // obj.InsertHSRPMessageNote(textboxMessageText.Text, '"++"', HSRPStateID, RTOLocationID, CreatedDateTime, ref IsExists);
                    string strquery = "insert into MessageNote (MessageText,UserID, HSRP_StateID,RTOLocationID, CreationDate ) values('" + textboxMessageText.Text + "','" + strUserID + "','" + dropdownlistStateName.SelectedItem.ToString() + "','" + dropdownlistRtoName.SelectedItem.ToString() + "','" + CreatedDateTime + "')";
                    Utils.ExecNonQuery(strquery, CnnString);
                    if (IsExists.Equals(1))
                    {
                        lblErrMess.Text = "Record Already Exist!!";
                    }
                    else
                    {
                        lblSucMess.Text = "Record Save Successfully.";
                    }
                }               
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            

        }

        protected void dropdownlistStateName_SelectedIndexChanged1(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            UserType = Session["UserType"].ToString();
          //  FilldropDownRTO();
        }

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(dropdownlistStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                dropdownlistStateName.DataSource = dts;
                dropdownlistStateName.DataBind();

            }
        }

        private void FilldropDownListClient()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(dropdownlistStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

                Utils.PopulateDropDownList(dropdownlistRtoName, SQLString.ToString(), CnnString, "--ALL--");
                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
                Utils.PopulateDropDownList(dropdownlistRtoName, SQLString.ToString(), CnnString, "--ALL--");

                //DataSet dss = Utils.getDataSet(SQLString, CnnString);
                //dropDownListClient.DataSource = dss;
                //dropDownListClient.DataBind();
            }
        }


        #endregion


        //private void FilldropDownState()
        //{
        //    if (UserType == "0")
        //    {
        //        SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
        //        Utils.PopulateDropDownList(dropdownlistStateName, SQLString.ToString(), CnnString, "--Select State--");
        //    }
        //    else
        //    {
        //       // dropdownlistStateName.Enabled = false;
        //        SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID= " + HSRPStateID + "and ActiveStatus='Y' Order by HSRPStateName";
        //        Utils.PopulateDropDownList(dropdownlistStateName, SQLString.ToString(), CnnString, "--Select State--");
        //        //dropdownlistStateName.SelectedIndex = dropdownlistStateName.Items.Count - 1;
        //    }
        //}

        //private void FilldropDownRTO()
        //{
        //    if (UserType == "0")
        //    {
        //       // int.TryParse(dropdownlistStateName.SelectedValue, out intHSRPStateID);
        //        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
        //        Utils.PopulateDropDownList(dropdownlistRtoName, SQLString.ToString(), CnnString, "--Select RTO Location--");
        //    }
        //    else
        //    {
        //        //dropdownlistRtoName.Enabled = false;
        //        SQLString = "select RTOLocationName, RTOLocationID from RTOLocation where RTOLocationID = " + RTOLocationID + "and ActiveStatus='Y'";
        //        Utils.PopulateDropDownList(dropdownlistRtoName, SQLString.ToString(), CnnString, "--Select RTO Location--");
        //       // dropdownlistRtoName.SelectedIndex = dropdownlistRtoName.Items.Count - 1;
        //    }

        //}


        //private void MessageTickerEdit(int TickerID)
        //{
        //    SQLString = "SELECT MessageTicker.MessageID, MessageTicker.HSRP_StateID, MessageTicker.RTOLocationID, MessageTicker.MessageText, MessageTicker.MessageTextURL, MessageTicker.ActiveStatus, MessageTicker.HSRP_StateID, HSRPState.HSRPStateName, MessageTicker.RTOLocationID, RTOLocation.RTOLocationName FROM MessageTicker INNER JOIN HSRPState ON MessageTicker.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON MessageTicker.RTOLocationID = RTOLocation.RTOLocationID where MessageTicker.MessageID=" + TickerID;
        //    DataTable dt = Utils.GetDataTable(SQLString, CnnString);
        //    textboxMessageText.Text = dt.Rows[0]["MessageText"].ToString();
        //    textboxMessageTextURL.Text = dt.Rows[0]["MessageTextURL"].ToString();
        //    SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where HSRP_StateID=" + dt.Rows[0]["HSRP_StateID"];
        //    Utils.PopulateDropDownList(dropdownlistStateName, SQLString.ToString(), CnnString, "--Select State--");
        //    dropdownlistStateName.SelectedIndex = dropdownlistStateName.Items.Count - 1;
        //    SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where RTOLocationID=" + dt.Rows[0]["RTOLocationID"].ToString();
        //    Utils.PopulateDropDownList(dropdownlistRtoName, SQLString.ToString(), CnnString, "--Select RTO Location--");
        //    dropdownlistRtoName.SelectedIndex = dropdownlistRtoName.Items.Count - 1;
        //    string Status = dt.Rows[0]["ActiveStatus"].ToString();
        //    if (Status == "Y")
        //    {
        //        chkActiveStatus.Checked = true;
        //    }
        //    else
        //    {
        //        chkActiveStatus.Checked = false;
        //    }
        //}
    }
}