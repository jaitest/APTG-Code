using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;

namespace HSRP.Report
{
    public partial class APTGCallCenterDetails : System.Web.UI.Page
    {
        int UserType;
        string CnnString = string.Empty;
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
        string strUserID = string.Empty;
        int intHSRPStateID;
        int intRTOLocationID;
        string SQLString = string.Empty;
        //DateTime OrderDate1;

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dtshow = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();

                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {

                    try
                    {
                        if (UserType.Equals(0))
                        {
                            FilldropDownListOrganization();
                            FilldropDownListClient();


                        }
                        else
                        {

                            FilldropDownListOrganization();
                            FilldropDownListClient();
                        }


                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }


        private void FilldropDownListOrganization()
        {
            //if (UserType.Equals(0))
            //{
            //    SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
            //    Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
            //}
            //else
            //{
            //    SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
            //    DataSet dts = Utils.getDataSet(SQLString, CnnString);
            //    DropDownListStateName.DataSource = dts;
            //    DropDownListStateName.DataBind();
            //}
        }

        private void FilldropDownListClient()
        {
            //if (UserType.Equals(0))
            //{
            //    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            //    SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
            //    Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "ALL");
            //}
            //else
            //{
            //    SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
            //    Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "ALL");
            //    //DataSet dss = Utils.getDataSet(SQLString, CnnString);
            //    //dropDownListClient.DataSource = dss;
            //    //dropDownListClient.DataBind();
            //}
        }
        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        }
    }
}