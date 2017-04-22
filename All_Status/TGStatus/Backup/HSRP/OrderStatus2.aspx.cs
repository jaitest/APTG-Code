using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MultiTrack;
using System.Data.SqlClient;
using System.Text;

namespace HSRP
{
    public partial class Loading : System.Web.UI.Page
    {
        string SQLString = String.Empty;
        string CnnString = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                SQLString = "Select  LandlineNo,RTOLocationName,RTOLocationAddress," +
                "ContactPersonName,MobileNo,EmailID,Rtocode From RTOLocation where HSRP_StateID=11 and LocationType='Sub-Urban'";


                Utils dbLink = new Utils();
                dbLink.strProvider = CnnString;
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
                sb.Append("<table width='97%' border='0' align='center' cellpadding='0' cellspacing='0'>");
                sb.Append("<tr style='background-color:#FFFFCC'>");
                sb.Append("<td valign='top' class='midtablebg'>");
                sb.Append("<table width='100%' border='0' align='center' cellpadding='3' cellspacing='1'>");
                sb.Append("<tr align='center'>");
                sb.Append("<td   height='19' valign='top' nowrap='nowrap' class='midboxtop'>RTO Code</td>");
                sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop'>Contact Person Name</td>");
                sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop'>RTO Location Name</font></td>");
                sb.Append("<td  valign='top' align='center' width='400px' valign='top' class='midboxtop'>RTO Location Address</td>");
                sb.Append("<td  valign='top' nowrap='nowrap'  class='midboxtop'>Mobile No</td>");
                sb.Append("<td  valign='top' nowrap='nowrap'  class='midboxtop'>Landline No</td>");
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
                        Rtocode = PReader["Rtocode"].ToString();
                        if (int.Parse(SrNo) % 2 == 0)
                        {
                            bgcol = "#ffffff";
                        }
                        else
                        {
                            bgcol = "#F9F9F9";
                        }

                        sb.Append("<tr style='background-color:#DDE8F4'>");
                        sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + Rtocode + "</td>");
                        sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + ContactPersonName + "</td>");
                        sb.Append("<td width='150' align='center' class='heading1'>" + RTOLocationName + "</td>");
                        sb.Append("<td align='center' class='heading1'>" + RTOLocationAddress + "</td>");
                        sb.Append("<td width='150' align='center' class='heading1'>" + MobileNo + "</td>");
                        sb.Append("<td width='150' align='center' class='heading1'>" + LandlineNo + "</td>");
                        sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + EmailID + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("</table></td></tr></table>");
                    vehshow.InnerHtml = sb.ToString();
                }
                else
                {
                    //LabelError.Text = string.Empty;
                    //LabelError.Text = "Their is no selected data for the selected  date range.";
                    //vehshow.InnerHtml = string.Empty;
                    //UpdatePanelError.Update();
                    //UpdatePanelDiv.Update();
                    return;
                }
            }
        }
    }
}