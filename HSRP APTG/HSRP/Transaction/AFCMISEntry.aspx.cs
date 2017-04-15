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
    public partial class AFCMISEntry : System.Web.UI.Page
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

            //string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            //OrderDate.SelectedDate = (DateTime.Parse(TodayDate));
            //CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if (string.IsNullOrEmpty(txtQuantity.Text))
                //{
                //    lblErrMsg.Text = "Please Enter Item Quantity.";
                //    txtQuantity.Focus();
                //    return;
                //}
                lblErrMsg.Text = "";
                string sql = "INSERT INTO [dbo].[AFCMIS]([TwoWPhotoR],[OTTwoPhotoR],[ThiredStickerPhotoR],[TwoWOrderClosedInR],[OTTwoWOrderClosedInR],[SnaplockR19mm],[SnaplockR25mm],[SnapLockC19mm],[SnapLockC25mm],[MPFront],[MPRear],[MPBoth],[RPFront],[RPRear],[RPBoth],[CreatedBy],[ActiveStatus],[Rtolocationid],[hsrp_stateid],[CreatedDatetime])VALUES(" + txt2WPhotoRecived.Text + "," + txtOtherThan2WPhotoRecieved.Text + "," + txt3rdStickerPhotoRecieved.Text + "," + txtOrderClosed2W.Text + "," + txtOtherThan2WOrderClosed.Text + "," + txtSnapLockRecieved19mm.Text + "," + txtSnapLockRecieved25mm.Text + "," + txtSnapLockConsumption19mm.Text + "," + txtSnapLockConsumption25mm.Text + "," + txtMissingPlateFont.Text + "," + txtMissingPlateRear.Text + "," + txtMissingPlateBoth.Text + "," + txtRejectedPlateFront.Text + "," + txtRejectedPlateRear.Text + "," + txtRejectedPlateBoth.Text + "," + strUserID + ",'Y'," + RTOLocationID + "," + HSRPStateID + ",getdate())";
                int i= Utils.ExecNonQuery(sql, CnnString);
                if (i > 0)
                {
                    lblErrMsg.Text = "Record save successfully";
                    lblErrMsg.ForeColor = Color.Blue;
                }
                else
                {
                    lblErrMsg.Text = "Record not save unsuccessful";
                    lblErrMsg.ForeColor = Color.Red;
                }
                
                
            }
            catch (Exception)
            {
                
                throw;
            }

        }
       
    }
}
 
        
