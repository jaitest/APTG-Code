using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MultiTrack;
using System.Data;
using System.Net.NetworkInformation;
using System.Net;
using System.Data.SqlClient;
using System.Text;

namespace HSRP
{
    public partial class ViewOrderStatus : System.Web.UI.Page
    {
        public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        String aapID = String.Empty;
        String SQLString = String.Empty;
        String Status = String.Empty;
        // string SQLString = String.Empty;
        string CnnString = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["REG"] == null)
            {
                Response.Redirect("OrderStatus.aspx");
            }
            yy.Text = String.Empty;
            if (!IsPostBack)
            {
                Search();
                Fill();
            }
        }

        string vehicalRegNo;

        public void Search()
        {

            try
            {
                vehicalRegNo = Session["REG"].ToString();
                if (vehicalRegNo.Length < 11)
                {
                    GetHostAddress();
                    SQLString = "SELECT Top(1)  REPLACE(REPLACE(REPLACE([VehicleRegNo],' ',''),'-',''),'/','') as [VehicleRegNo],[OrderStatus],Rtolocationid,ordercloseddate FROM hsrprecords where HSRP_StateID=1 and [VehicleRegNo] ='" + vehicalRegNo + "'";
                    DataTable dt1 = Utils.GetDataTable(SQLString, ConnectionString);
                    if (dt1.Rows.Count > 0)
                    {
                        Status = dt1.Rows[0]["OrderStatus"].ToString();
                        if (Status.Equals("CLOSE") || Status.Equals("Closed"))
                        {
                            string datetime = dt1.Rows[0]["ordercloseddate"].ToString();
                            DateTime d = DateTime.Parse(datetime).Date;
                            string t = d.Date.ToString("dd/MM/yy");


                            // yy.Text = "YOUR PLATES IS AFFIXED ON " + dt1.Rows[0]["ordercloseddate"].ToString();
                            yy.Text = "YOUR PLATES IS AFFIXED ON " + t;
                        }
                        else if (Status.Equals("NEW ORDER") || Status.Equals("OPEN"))
                        {
                            //yy.Text = "YOUR ORDER IS IN PROCESS, PLEASE CONTACT THE FOLLOWING HSRP CENTER.";

                            yy.Text = "YOUR REQUEST IS TIME OUT PLEASE TRY AFTER SOME TIME. ";
                            TDR.Visible = false;
                            //tab.Visible = false;
                            return;
                        }
                        else if (Status.Equals("Embossing Done") || Status.Equals("NOT DELIVERED"))
                        {
                            yy.Text = "YOUR PLATE IS READY , PLEASE CONTACT THE FOLLOWING HSRP CENTER.";
                        }
                        showData1(dt1.Rows[0]["Rtolocationid"].ToString());
                        return;
                    }
                    else
                    {
                        yy.Text = "PLEASE CHECK THE REGISTRATION NO ITS INVALID : " + vehicalRegNo.ToUpper();
                    }

                }
                else
                {
                    yy.Text = "PLEASE CHECK THE REGISTRATION NO ITS INVALID : " + vehicalRegNo.ToUpper();
                }

            }
            catch(Exception ex)
            { }
        }

        private void Fill()
        {
            SQLString = "Select  LandlineNo,RTOLocationName,RTOLocationAddress," +
            "ContactPersonName,MobileNo,EmailID,Rtocode From RTOLocation where HSRP_StateID=9 and LocationType='Sub-Urban' and rtocode is not null";


            Utils dbLink = new Utils();
            dbLink.strProvider = ConnectionString;
            dbLink.CommandTimeOut = 0;
            dbLink.sqlText = SQLString.ToString();
            SqlDataReader PReader = dbLink.GetReader();


            String SrNo = String.Empty;
            String RTOLocationName = String.Empty;
            String RTOLocationAddress = String.Empty;
            String ContactPersonName = String.Empty;
            String MobileNo = String.Empty;
            String LandlineNo = String.Empty;
            String EmailID = String.Empty;
            String Rtocode = String.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='99%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr style='background-color:#acab3d'>");
            sb.Append("<td valign='top' class='midtablebg'>");
            sb.Append("<table width='100%' border='0' align='center' cellpadding='7' cellspacing='0'>");
            sb.Append("<tr align='center'>");
            sb.Append("<td   height='19' valign='top'  class='midboxtop'>Registration Authority Code</td>");
            sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop'>Contact Person Name</td>");
            sb.Append("<td  valign='top'  class='midboxtop'>Registration Authority Name</font></td>");
            sb.Append("<td  valign='top' align='center' width='400px' valign='top' class='midboxtop'>Registration Authority Address</td>");
            sb.Append("<td  valign='top' nowrap='nowrap'  class='midboxtop'>Mobile No</td>");
            sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop'>Email ID</td>");
            sb.Append("</tr>");


            string bgcol = string.Empty;


            if (PReader.HasRows)
            {
                while (PReader.Read())
                {
                    SrNo = String.Empty;
                    RTOLocationName = String.Empty;
                    RTOLocationAddress = String.Empty;
                    ContactPersonName = String.Empty;
                    MobileNo = String.Empty;
                    EmailID = String.Empty;
                    Rtocode = String.Empty;
                    SrNo = PReader["Rtocode"].ToString();
                    RTOLocationName = PReader["RTOLocationName"].ToString();
                    RTOLocationAddress = PReader["RTOLocationAddress"].ToString();
                    ContactPersonName = PReader["ContactPersonName"].ToString();
                    MobileNo = PReader["MobileNo"].ToString();
                    LandlineNo = PReader["LandlineNo"].ToString();
                    EmailID = PReader["EmailID"].ToString();
                    Rtocode = "HR" + PReader["Rtocode"].ToString();
                   
                        bgcol = "#ffffff";
                  

                    sb.Append("<tr style='background-color:#ffffff'>");
                    sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + Rtocode + "</td>");
                    sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + ContactPersonName + "</td>");
                    sb.Append("<td width='150' align='center' class='heading1'>" + RTOLocationName + "</td>");
                    sb.Append("<td align='center' class='heading1'>" + RTOLocationAddress + "</td>");
                    sb.Append("<td width='150' align='center' class='heading1'>" + MobileNo + "</td>");
                    sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + EmailID + "</td>");
                    sb.Append("</tr>");
                }

                sb.Append("</table></td></tr></table>");
            //    vehshow.InnerHtml = sb.ToString();
            }
            else
            {
                return;
            }
        }

        public void GetHostAddress()
        {
            aapID = HttpContext.Current.Request.UserHostAddress.ToLower();
        }

        public void showData1(string strRtolocationid)
        {
            SQLString = "select RTOLocationName,LandlineNo,RTOLocationCode,rtocode,RTOLocationAddress,ContactPersonName,MobileNo,EmailID from RTOLocation where RTOLocationid='" + strRtolocationid + "' and HSRP_StateID=1 ";
            DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            if (ds.Rows.Count > 0)
            {
                FillDetailsOfSearchVehicle(ds);
                SaveVehiRegSerchLog();
            }
            else
            {
                FillDetailsOfSearchVehicle(ds);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["REG"] = null;
            Response.Redirect("OrderStatus.aspx");
        }

        public void SaveVehiRegSerchLog()
        {
            string Query = ("INSERT INTO [VehicleStatus_SearchLog]([regno],[customer_IP],[datetimeofsearch])VALUES('" + LabelVehicleNo.Text + "','" + aapID + "','" + DateTime.Now.ToString() + "')");
            //  Utils.ExecNonQuery(Query, ConnectionString);
        }

        private void FillDetailsOfSearchVehicle(DataTable ds)
        {
            yy.Text = ds.Rows.Count == 0 ? "PLEASE CHECK THE No : " + Session["REG"].ToString() : yy.Text;
            LabelVehicleNo.Text = Session["REG"].ToString() == null ? "No Record Found" : Session["REG"].ToString();
            LabelRTOCenter.Text = ds.Rows.Count == 0 ? "" : ds.Rows[0]["RTOLocationName"].ToString() + "/";
            LabelRTOCode.Text = ds.Rows.Count == 0 ? "" : ds.Rows[0]["RTOLocationCode"].ToString();

            LabelAddress.Text = ds.Rows.Count == 0 ? "" : ds.Rows[0]["RTOLocationAddress"].ToString();
            LabelContactNo.Text = ds.Rows.Count == 0 ? "" : ds.Rows[0]["MobileNo"].ToString();
            LabelContactPersonName.Text = ds.Rows.Count == 0 ? "" : ds.Rows[0]["ContactPersonName"].ToString();
            LabelEmail.Text = ds.Rows.Count == 0 ? "" :  ds.Rows[0]["EmailID"].ToString();
            LabelLandlineno.Text = ds.Rows.Count == 0 ? "" : ds.Rows[0]["LandlineNo"].ToString();

        }
    }
}