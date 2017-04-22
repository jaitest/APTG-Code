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
    public partial class Complaint : System.Web.UI.Page
    {
        public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        String aapID = String.Empty;
        String SQLString = String.Empty;
        String Status = String.Empty;
       // string SQLString = String.Empty;
        string CnnString = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            yy.Text = String.Empty;
            if (!IsPostBack)
            {
                try
                {
                    Search();
                    Fill();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }

            }
        }

        string vehicalRegNo;

        public void Search()
        {

            //vehicalRegNo = Request.QueryString["REG"].ToString();
            vehicalRegNo = Session["REG"].ToString();
            GetHostAddress();
            SQLString = "SELECT Top(1)  REPLACE(REPLACE(REPLACE([VehicleRegNo],' ',''),'-',''),'/','') as [VehicleRegNo],[OrderStatus] FROM hsrprecords where HSRP_StateID=2 and [VehicleRegNo] ='" + vehicalRegNo + "'";
            DataTable dt1 = Utils.GetDataTable(SQLString, ConnectionString);
            if (dt1.Rows.Count <= 0)
            {
                yy.Text += " :- ORDER NOT RECEIVED PLEASE CONTACT YOUR DEALER.FOR YOUR REGISTRATION NUM :" + vehicalRegNo.ToUpper();

                string vehicalreg = vehicalRegNo.Substring(0, 4);
                SQLString = " SELECT Top(1) * FROM [HSRPRecords] where HSRP_StateID=2 and  (VehicleRegNo like'" + vehicalRegNo.Substring(0, 4) + "%' or VehicleRegNo like'" + vehicalRegNo.Substring(0, 3) + "%')";
                DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
                //if (dt.Rows.Count > 0)
                //{
                    string vehiReg = vehicalRegNo.ToString();
                    string Vr = vehiReg.Substring(2, 1);
                    string Vr1 = vehiReg.Substring(3, 1);

                    if (Vr1 == "1" || Vr1 == "2" || Vr1 == "3" || Vr1 == "4" || Vr1 == "5" || Vr1 == "6" || Vr1 == "7" || Vr1 == "8" || Vr1 == "9" || Vr1 == "0")
                    {
                        string vehi1 = Vr.ToString() + Vr1.ToString();
                        showData1(vehi1);
                    }
                    else
                    {
                        string vehi = Vr.ToString();
                        showData1(vehi);
                    }
               // }
                return;
            }
            //SQLString = "SELECT top(1) * FROM [HSRPRecords] where VehicleRegNo ='" + vehicalRegNo + "'";
            //DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            //if (ds.Rows.Count > 0)
            //{
            //    Status = ds.Rows[0]["OrderStatus"].ToString();
            //    if (Status.Equals("New Order"))
            //    {
            //        yy.Text += " :- YOUR ORDER IS IN PROCESS, PLEASE CONTACT THE FOLLOWING HSRP CENTER.";
            //    }
            //    else if (Status.Equals("Embossing Done"))
            //    {
            //        yy.Text += " :- YOUR PLATE IS READY , PLEASE CONTACT THE FOLLOWING HSRP CENTER.";
            //    }
            //    else if (Status.Equals("Closed"))
            //    {
            //        yy.Text += " :- YOUR PLATES IS AFFIXED AND DELIVERED.";
            //    }
            //    LabelVehicleNo.Text = ds.Rows[0]["VehicleRegNo"].ToString();

            //    string vehiReg = LabelVehicleNo.Text.ToString();
            //    string Vr = vehiReg.Substring(2, 1);
            //    string Vr1 = vehiReg.Substring(3, 1);

            //    if (Vr1 == "1" || Vr1 == "2" || Vr1 == "3" || Vr1 == "4" || Vr1 == "5" || Vr1 == "6" || Vr1 == "7" || Vr1 == "8" || Vr1 == "9" || Vr1 == "0")
            //    {
            //        string vehi1 = Vr.ToString() + Vr1.ToString();
            //        showData(vehi1);
            //        return;

            //    }
            //    else
            //    {
            //        string vehi = Vr.ToString();
            //        showData(vehi);
            //        return;

            //    }

            //}
            //else
            //{
            SQLString = "  SELECT Top(1)  REPLACE(REPLACE(REPLACE([VehicleRegNo],' ',''),'-',''),'/','') as [VehicleRegNo],[OrderStatus] FROM hsrprecords where HSRP_StateID=2 and  VehicleRegNo ='" + vehicalRegNo + "'";
                DataTable dtt = Utils.GetDataTable(SQLString, ConnectionString);

                if (dtt.Rows.Count > 0)
                {
                    Status = dtt.Rows[0]["OrderStatus"].ToString();
                    if (Status.Equals("CLOSE") || Status.Equals("Closed"))
                    {
                        yy.Text += " :- YOUR PLATES IS AFFIXED AND DELIVERED.";
                    }
                    else if (Status.Equals("New Order") || Status.Equals("OPEN"))
                    {
                        yy.Text += " :- YOUR ORDER IS IN PROCESS, PLEASE CONTACT THE FOLLOWING HSRP CENTER.";
                    }
                    else if (Status.Equals("Embossing Done") || Status.Equals("NOT DELIVERED"))
                    {
                        yy.Text += " :- YOUR PLATE IS READY , PLEASE CONTACT THE FOLLOWING HSRP CENTER.";
                    }
                    LabelVehicleNo.Text = dtt.Rows[0]["VehicleRegNo"].ToString();

                    // stateID = dtt.Rows[0]["HSRP_StateID"].ToString();

                    string vehiReg = LabelVehicleNo.Text.ToString();
                    string Vr = vehiReg.Substring(2, 1);
                    string Vr1 = vehiReg.Substring(3, 1);

                    if (Vr1 == "1" || Vr1 == "2" || Vr1 == "3" || Vr1 == "4" || Vr1 == "5" || Vr1 == "6" || Vr1 == "7" || Vr1 == "8" || Vr1 == "9" || Vr1 == "0")
                    {
                        string vehi1 = Vr.ToString() + Vr1.ToString();
                        showData(vehi1);
                        return;

                    }
                    else
                    {
                        string vehi = Vr.ToString();
                        showData(vehi);
                        return;

                    }
               // }
                string vehicalreg = vehicalRegNo.Substring(0, 4);
                //SQLString = "SELECT * FROM [HSRP].[dbo].[HSRPRecords] where (VehicleRegNo ='" + vehicalRegNo + "')";
                SQLString = "SELECT Top(1) VehicleRegNo FROM [HSRPRecords] HSRP_StateID=2 and  where (VehicleRegNo like'DL4C%' or VehicleRegNo like'DL4%') union SELECT Top(1) VehicleRegNo FROM VHSRPRecords where (VehicleRegNo like'DL4C%' or VehicleRegNo like'DL4%')  ";
                DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    yy.Text += " :- ORDER NOT RECEIVED PLEASE CONTACT YOUR DEALER.FOR YOUR REGISTRATION NUM :" + vehicalRegNo.ToUpper();
                    string vehiReg1 = vehicalRegNo.ToString();
                    string Vr22 = vehiReg1.Substring(2, 1);
                    string Vr11 = vehiReg1.Substring(3, 1);

                    if (Vr11 == "1" || Vr11 == "2" || Vr11 == "3" || Vr11 == "4" || Vr11 == "5" || Vr11 == "6" || Vr11 == "7" || Vr11 == "8" || Vr11 == "9" || Vr11 == "0")
                    {
                        string vehi1 = Vr22.ToString() + Vr11.ToString();
                        showData1(vehi1);
                        return;
                    }
                    else
                    {
                        string vehi = Vr22.ToString();
                        showData1(vehi);
                        return;
                    }

                }
                else
                {
                    yy.Text += " :- ORDER NOT RECEIVED PLEASE CONTACT YOUR DEALER.FOR YOUR REGISTRATION NUM :" + vehicalRegNo.ToUpper();
                    string vehiReg1 = vehicalRegNo.ToString();
                    string Vr22 = vehiReg1.Substring(2, 1);
                    string Vr11 = vehiReg1.Substring(3, 1);

                    if (Vr11 == "1" || Vr11 == "2" || Vr11 == "3" || Vr11 == "4" || Vr11 == "5" || Vr11 == "6" || Vr11 == "7" || Vr11 == "8" || Vr11 == "9" || Vr11 == "0")
                    {
                        string vehi1 = Vr22.ToString() + Vr11.ToString();
                        showData1(vehi1);
                        return;
                    }
                    else
                    {
                        string vehi = Vr22.ToString();
                        showData1(vehi);
                        return;
                    }

                   // Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Record is not found.');</script>");
                    // Response.Redirect("OrderStatus.aspx", true);
                }
            }
        }

        private void Fill()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = "Select  LandlineNo,RTOLocationName,RTOLocationAddress," +
            "ContactPersonName,MobileNo,EmailID,Rtocode From RTOLocation where HSRP_StateID=2 and LocationType='Sub-Urban' and rtocode is not null  and rtolocationname not in ('MAYAPURI','D Surajmal Vihar')";


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
            sb.Append("<table width='99%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr style='background-color:#acab3d'>");
            sb.Append("<td valign='top' class='midtablebg'>");
            sb.Append("<table width='100%' border='0' align='center' cellpadding='7' cellspacing='0'>");
            sb.Append("<tr align='center'>");
            sb.Append("<td   height='19' valign='top' nowrap='nowrap' class='midboxtop'>RTO Code</td>");
            sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop'>Contact Person Name</td>");
            sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop'>RTO Location Name</font></td>");
            sb.Append("<td  valign='top' align='center' width='400px' valign='top' class='midboxtop'>RTO Location Address</td>");
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
                    LandlineNo = PReader["LandlineNo"] == null ? "" : PReader["LandlineNo"].ToString();
                    EmailID = PReader["EmailID"].ToString();
                    Rtocode = "DL" + PReader["Rtocode"].ToString();
                    //if (int.Parse(SrNo) % 2 == 0)
                    //{
                        bgcol = "#ffffff";
                    //}
                    //else
                    //{
                    //    bgcol = "#ffffff";
                    //}

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

        public void GetHostAddress()
        {
            aapID = HttpContext.Current.Request.UserHostAddress.ToLower();
        }

        public void showData(string vehi)
        {
            SQLString = "select RTOLocationName,LandlineNo,RTOLocationCode,rtocode,RTOLocationAddress,ContactPersonName,MobileNo,EmailID from RTOLocation where Rtocode ='" + vehi + "' and HSRP_StateID=2 ";
            DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            if (ds.Rows.Count > 0)
            {
                LabelAddress.Text = ds.Rows[0]["RTOLocationAddress"].ToString();
                LabelContactNo.Text = ds.Rows[0]["MobileNo"].ToString();
                LabelContactPersonName.Text = ds.Rows[0]["ContactPersonName"].ToString();
                LabelEmail.Text = ds.Rows[0]["EmailID"].ToString();
                LabelLandlineno.Text = ds.Rows[0]["LandlineNo"].ToString();
                LabelRTOCenter.Text = ds.Rows[0]["RTOLocationName"].ToString();
                LabelRTOCode.Text = ds.Rows[0]["RTOLocationCode"].ToString();

                SaveVehiRegSerchLog();
            }
            else
            {
                //showcontact.Visible = false;
            }
        }

        public void showData1(string vehi)
        {
            SQLString = "select RTOLocationName,LandlineNo,RTOLocationCode,rtocode,RTOLocationAddress,ContactPersonName,MobileNo,EmailID from RTOLocation where Rtocode ='" + vehi + "' and HSRP_StateID=2 ";
            DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            if (ds.Rows.Count > 0)
            {
                //yy.Visible = true;
                //yy.Text += "  :- Unknown";
                // kk1.Visible = false;
                // LabelStatus.Visible = false;
                // kk.Visible=false;
                //LabelVehicleNo.Text = Request.QueryString["REG"].ToString();
                LabelVehicleNo.Text = Session["REG"].ToString();
                LabelAddress.Text = ds.Rows[0]["RTOLocationAddress"].ToString();
                LabelContactNo.Text = ds.Rows[0]["MobileNo"].ToString();
                LabelContactPersonName.Text = ds.Rows[0]["ContactPersonName"].ToString();
                LabelEmail.Text = ds.Rows[0]["EmailID"].ToString();
                LabelLandlineno.Text = ds.Rows[0]["LandlineNo"].ToString();
                LabelRTOCenter.Text = ds.Rows[0]["RTOLocationName"].ToString();
                LabelRTOCode.Text = ds.Rows[0]["RTOLocationCode"].ToString();

                SaveVehiRegSerchLog();
            }
            else
            {
                //showcontact.Visible = false;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("OrderStatus.aspx");
        }

        public void SaveVehiRegSerchLog()
        {

            //using (SqlConnection connection = new SqlConnection(ConnectionString))
            //{
            //    connection.Open();
            //    SqlCommand myCommand = new SqlCommand("INSERT INTO [VehicleStatus_SearchLog]([regno],[customer_IP],[datetimeofsearch])VALUES(@regno,'" + aapID + "','" + DateTime.Now.ToString() + "')", connection);
            //    myCommand.Parameters.AddWithValue("@regno", LabelVehicleNo.Text);
            //    myCommand.ExecuteNonQuery();
            //    connection.Close();
            //    connection.Dispose();

            //}


            string Query = ("INSERT INTO [VehicleStatus_SearchLog]([regno],[customer_IP],[datetimeofsearch])VALUES('" + LabelVehicleNo.Text + "','" + aapID + "','" + DateTime.Now.ToString() + "')");
            Utils.ExecNonQuery(Query, ConnectionString);
        }

    }
}