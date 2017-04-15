using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataProvider;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;

namespace HSRP.Report
{
    public partial class APTG_PoliceCollection_Report : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        InitialSetting();
                        if (UserType.Equals(0))
                        {
                            //labelOrganization.Visible = true;
                            FilldropDownListOrganization();
                           // FilldropDownListClient();
                            
                        }
                        else
                        {
                            FilldropDownListOrganization();
                           // FilldropDownListClient();
                           // DropDownListStateName.Enabled = false;
                           
                            //FilldropDownListClient();
                            //FilldropDownListOrganization();
                        }
                    }
                    catch (Exception err)
                    {

                    }
                }
            }
        }

        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }
        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();

            }
        }

        //private void FilldropDownListClient()
        //{
        //    if (UserType.Equals(0))
        //    {
        //        int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";
        //        Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--ALL--");
               
        //    }
        //    else
        //    {                
        //        SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
        //        Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--ALL--");

        //    }
        //}


        #endregion

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
           // FilldropDownListClient();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Text = "";
                String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                string ReportDate2 = ReportDateEnd + " 23:59:59";
                String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


                String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                String DatePrint = DatePrint1 + "   To    " + DatePrint2;

                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = ReportDate + " 00:00:00";

                OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));

                DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
               
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
               
                string ToDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";


                string filename = "APTG_PoliceReport-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "APTG_PoliceReport";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 9;
                style.Font.Bold = false;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                style8.Font.FontName = "Tahoma";
                style8.Font.Size = 10;
                style8.Font.Bold = true;
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style8.Interior.Color = "#D4CDCD";
                style8.Interior.Pattern = StyleInteriorPattern.Solid;

                WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                style5.Font.FontName = "Tahoma";
                style5.Font.Size = 10;
                style5.Font.Bold = false;
                style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
                style2.Font.FontName = "Tahoma";
                style2.Font.Size = 10;
                style2.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
                style3.Font.FontName = "Tahoma";
                style3.Font.Size = 12;
                style3.Font.Bold = true;
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                Worksheet sheet = book.Worksheets.Add("APTG_PoliceReport");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(150));

                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(92));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));


                WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                style9.Font.FontName = "Tahoma";
                style9.Font.Size = 10;
                style9.Font.Bold = true;
                style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style9.Interior.Color = "#FCF6AE";
                style9.Interior.Pattern = StyleInteriorPattern.Solid;


                WorksheetRow row = sheet.Table.Rows.Add();

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("APTG_PoliceReport");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                row.Index = 4;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
               
                row.Cells.Add(new WorksheetCell("From:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ReportDate1, "HeaderStyle2"));

                row.Cells.Add(new WorksheetCell("TO:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ReportDate2, "HeaderStyle2"));
                                
                row = sheet.Table.Rows.Add();
                row.Index = 5;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dates.ToString("dd/M/yyyy"), "HeaderStyle2"));
                
                row = sheet.Table.Rows.Add();

                row.Index = 6;
                
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("vehicleregno", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell("Location Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderDate", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OwnerName", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("vehicleclass", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("vehicletype", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderEmbossingDate", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Createdby", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("TotalAmount", "HeaderStyle6"));
      

                row = sheet.Table.Rows.Add();




                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 8;

                UserType = Convert.ToInt32(Session["UserType"]);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);


                //select orderdate,vehicleregno,ownerName,VehicleType,VehicleClass,OrderEmbossingDate,Createdby,TotalAmount from hsrprecords where hsrp_stateid=9 and ownerName like '%Police%'

                string SqlQuery = "select vehicleregno,orderdate,ownerName,VehicleClass,VehicleType,OrderEmbossingDate,Createdby,TotalAmount from hsrprecords  where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and  hsrprecord_creationdate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and isnull(Vehicleregno,'')!='' and ownerName like '%Police%'";

                dt = Utils.GetDataTable(SqlQuery, CnnString);
                string RTOColName = string.Empty;
              //  decimal totalAmount = 0;
                ////decimal totalAmount1 = 0;
                //decimal totalAmount2 = 0;
               // decimal TotalRecordSum = 0;
                if (dt.Rows.Count > 0)
                {
                    int sno = 0;
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));

                        row.Cells.Add(new WorksheetCell(dtrows["vehicleregno"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["vehicleclass"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["vehicletype"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Createdby"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["TotalAmount"].ToString(), DataType.String, "HeaderStyle"));
                        row = sheet.Table.Rows.Add();
                    }
                    row = sheet.Table.Rows.Add();
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    // Save the file and open it
                    book.Save(Response.OutputStream);
                    context.Response.ContentType = "application/vnd.ms-excel";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();

                }

                else 
                {
                    LabelError.Text = "Record Not Found";
                   // DisplayRecord();
                }
            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

       

        public void DisplayRecord()
        {
            try
            {
                LabelError.Text = "";
                String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                string ReportDate2 = ReportDateEnd + " 23:59:59";
                String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


                String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                String DatePrint = DatePrint1 + "   To    " + DatePrint2;

                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = ReportDate + " 00:00:00";

                OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));

                DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();

                string ToDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                //SqlQuery = "select ROW_NUMBER() OVER(ORDER BY vehicleregno) SNO, HSRPRecord_AuthorizationNo,convert(varchar(20),HSRPRecord_CreationDate,103) as OrderDate ,OwnerName,vehicleclass,vehicletype from  hsrprecords where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "' and  hsrprecord_creationdate<='" + ToDate + "' ";
                string SqlQuery = "select ROW_NUMBER() OVER(ORDER BY vehicleregno) SNO,vehicleregno,orderdate,ownerName,VehicleClass,VehicleType,OrderEmbossingDate,Createdby,TotalAmount from hsrprecords  where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and  hsrprecord_creationdate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and isnull(Vehicleregno,'')!='' and ownerName like '%Police%'";
                dt = Utils.GetDataTable(SqlQuery, CnnString);
                if (dt.Rows.Count > 0)
                {
                    grdpending.DataSource = dt;
                    grdpending.DataBind();
                }
                else
                {
                    LabelError.Text = "Record Not Found";
                }
            }
            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }

        }

        protected void GO_Click(object sender, EventArgs e)
        {
            DisplayRecord();
        }

        protected void grdpending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdpending.PageIndex = e.NewPageIndex;
            DisplayRecord();
        }

       
    }

}