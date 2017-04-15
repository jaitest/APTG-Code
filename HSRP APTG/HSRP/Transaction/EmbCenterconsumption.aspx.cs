using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
//using System.Data.OracleClient;


namespace HSRP.Transaction
{
    public partial class EmbCenterconsumption : System.Web.UI.Page
    {
        
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;       
        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();       
        string strCompanyName = string.Empty;
        string strSqlQuery = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                    InitialSetting();
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
            catch (Exception)
            {
                
                throw;
            }
        }

        #region DropDown
       
        #endregion

        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate));
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        public string GetTransfarOrderNumber(DataTable dt)
        {
            try
            {
                if (HSRPStateID == "9")
                {
                    WebReference.HSRPWebService service = new WebReference.HSRPWebService();
                    service.UseDefaultCredentials = false;
                    service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
                    service.PostConsumption(Convert.ToDateTime(OrderDate.SelectedDate.ToString("MM/dd/yyyy")), "1", DropDownList1.SelectedValue.ToString(), Convert.ToDecimal(txtQuantity.Text), dt.Rows[0]["NAVEMBID"].ToString()); 
                    return "true";
                }
                else 
                {
                    WebReference_TG.HSRPWebService service = new WebReference_TG.HSRPWebService();
                    service.UseDefaultCredentials = false;
                    service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
                    //service.PostConsumption(Convert.ToDateTime(OrderDate.SelectedDate.ToString("MM/dd/yyyy")), "1", DropDownList1.SelectedValue.ToString(), Convert.ToDecimal(txtQuantity.Text), dt.Rows[0]["NAVEMBID"].ToString()); 
                    return "False";
                }

            }
            catch (Exception)
            {
                
                throw;
            }           
        }

        protected void ddlEmbossingCenters_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
            }
            catch (Exception)
            {
                
                throw;
            }   

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (DropDownList1.SelectedValue == "--Select Item--")
                {
                    lblErrMsg.Text = "Please Select Item.";
                    DropDownList1.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtQuantity.Text))
                {
                    lblErrMsg.Text = "Please Enter Item Quantity.";
                    txtQuantity.Focus();
                    return;
                }
                lblErrMsg.Text = "";
                string sql = "Select NAVEMBID from rtolocation where RTOLocationID='" + RTOLocationID + "' and ActiveStatus='Y'";
                DataTable dt= Utils.GetDataTable(sql, CnnString);
                if (dt.Rows.Count > 0)
                {
                    String Result = GetTransfarOrderNumber(dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    if (Result.ToUpper().ToString() == "TRUE")
                    {
                        lblErrMsg.Text = "Shipment Received successfully";
                        DropDownList1.ClearSelection();
                    }
                    else
                    {
                        lblErrMsg.Text = "Shipment Received unsuccessful";
                    }
                }
                else
                {
                    lblErrMsg.Text = "Invelid NAVEMBID.";
                    return;
                }
                
            }
            catch (Exception)
            {
                
                throw;
            }

        }
       
    }
}
 
        
