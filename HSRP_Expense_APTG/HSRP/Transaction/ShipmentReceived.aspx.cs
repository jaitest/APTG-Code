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


namespace HSRP.Master
{
    public partial class ShipmentReceived : System.Web.UI.Page
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

        public string GetTransfarOrderNumber(string TransfarOrderNumber)
        {
            try
            {
                if (HSRPStateID == "9")
                {
                    WebReference.HSRPWebService service = new WebReference.HSRPWebService();
                    service.UseDefaultCredentials = false;
                    service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
                    String a = service.ShipmentReceived(TransfarOrderNumber);
                    //WebReference.HSRPWebService WebInventoryData Cust = new WebReference.WebInventoryData();
                    return a.ToString();
                }
                else 
                {
                    WebReference_TG.HSRPWebService service = new WebReference_TG.HSRPWebService();
                    service.UseDefaultCredentials = false;
                    service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
                    String a = service.ShipmentReceived(TransfarOrderNumber);
                    //WebReference.HSRPWebService WebInventoryData Cust = new WebReference.WebInventoryData();
                    return a.ToString();
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
                if (string.IsNullOrEmpty(txtTransfarOrderNumber.Text))
                {
                    lblErrMsg.Text = "Please Enter Transfar Order Number.";
                    txtTransfarOrderNumber.Focus();
                    return;
                }
                lblErrMsg.Text = "";
                String Result = GetTransfarOrderNumber(txtTransfarOrderNumber.Text);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                if (Result.ToUpper().ToString() =="TRUE")
                {
                    lblErrMsg.Text = "Shipment Received successfully";
                    txtTransfarOrderNumber.Text = "";
                }
                else
                {
                    lblErrMsg.Text = "Shipment Received unsuccessful";
                }
            }
            catch (Exception)
            {
                
                throw;
            }

        }
       
    }
}
 
        
