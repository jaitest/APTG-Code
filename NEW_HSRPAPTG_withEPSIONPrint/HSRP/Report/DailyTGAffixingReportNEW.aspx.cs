﻿using System;
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
    public partial class DailyTGAffixingReportNEW : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID = string.Empty;
        // int RTOLocationID;
        int intHSRPStateID;
        // int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        DataProvider.BAL bl = new DataProvider.BAL();
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
                            //DropDownListStateName.Visible = true;

                            FilldropDownListOrganization();
                           


                        }
                        else
                        {
                            FilldropDownListOrganization();
                            Label1.Visible = false;
                            ddllocation.Visible = false;
                            labelOrganization.Enabled = true;
                            DropDownListStateName.Enabled = false;
                            DropDownListStateName.SelectedValue = "11";
                            FilldropDownListLocation();
                            ddllocation.Visible = true;
                            Label1.Visible = true;
                           
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
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='11' AND  ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='11' and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();

            }
        }

        private void FilldropDownListLocation()
        {
            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation where HSRP_StateID='11' and ActiveStatus='Y' and RTOLocationID not in(877) order by RTOLocationName ";
            Utils.PopulateDropDownList(ddllocation, SQLString.ToString(), CnnString, "--Select Location--");
        }
        //private void FilldropDownListClient()
        //{
        //    if (UserType.Equals(0))
        //    {
        //        int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

        //        Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
        //        // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
        //    }
        //    else
        //    {
        //        // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
        //        SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + strUserID + "' ";

        //        DataSet dss = Utils.getDataSet(SQLString, CnnString);
        //        dropDownListClient.DataSource = dss;
        //        dropDownListClient.DataBind();

        //    }
        //}

        #endregion



        string FromDate, ToDate;
        DataSet ds = new DataSet();
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Text = "";



                String From = OrderDate.SelectedDate.ToString("MM/dd/yyyy");
                string FromDate = From + " 00:00:00"; // Convert.ToDateTime();
                
                String From1 = HSRPAuthDate.SelectedDate.ToString("MM/dd/yyyy"); ;
                string ToDate = From1 + " 23:59:59";

                string sql1 = "select rtolocationid from rtolocation where HSRP_StateID =9 and tgrelation ='" + ddllocation.SelectedValue + "' and activestatus='N'";
                string aprtolocationid = "0";
                aprtolocationid = Utils.getDataSingleValue(sql1, CnnString, "rtolocationid");


                //SQLString = "SELECT b.ProductColor,VehicleRegNo, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory,  Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize , a.Remarks,a.CashReceiptNo AS CashNo,convert(varchar(10),a.HSRPRecord_CreationDate,103) AS CashDate,a.DeliveryChallanNo,(case when a.DeliveryChallanNo is null then '' else convert(varchar(10), a.ordercloseddate,103) end) as ordercloseddate ,datediff(day,a.hsrprecord_creationdate,OrderClosedDate) as No_of_days  FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN Product Product_1  ON a.FrontPlateSize = Product_1.ProductID WHERE a.HSRP_StateID= '11' AND a.OrderclosedDate   between '" + FromDate + "' and '" + ToDate + "'and a.OrderclosedDate <> '' and a.Rtolocationid='" + ddllocation.SelectedValue + "' and a.orderstatus='Closed' order by a.ordercloseddate";
                SQLString = "SELECT b.ProductColor,VehicleRegNo, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory,  Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize , a.Remarks,a.CashReceiptNo AS CashNo,convert(varchar(10),a.HSRPRecord_CreationDate,103) AS CashDate,a.DeliveryChallanNo,(case when a.DeliveryChallanNo is null then '' else convert(varchar(10), a.ordercloseddate,103) end) as ordercloseddate ,datediff(day,a.hsrprecord_creationdate,OrderClosedDate) as No_of_days  FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN Product Product_1  ON a.FrontPlateSize = Product_1.ProductID WHERE a.HSRP_StateID= '11' AND a.OrderclosedDate   between '" + FromDate + "' and '" + ToDate + "'and a.OrderclosedDate <> '' and (a.Rtolocationid='" + ddllocation.SelectedValue + "' or a.Rtolocationid='" + aprtolocationid + "') and a.orderstatus='Closed' order by a.ordercloseddate";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);

                if (dt.Rows.Count <= 0)
                {
                    LabelError.Text = "Records Not Found";
                    return;
                }
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                string filename = "Telangana Affixing Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily Embossing Report";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                style5.Font.Bold = true;
                style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
                style2.Font.FontName = "Tahoma";
                style2.Font.Size = 10;
                style2.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
                style3.Font.FontName = "Tahoma";
                style3.Font.Size = 12;
                style3.Font.Bold = true;
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Center;

                Worksheet sheet = book.Worksheets.Add("Telangana Data Affixing Report");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(205));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(130));

                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(112));
                sheet.Table.Columns.Add(new WorksheetColumn(109));
                sheet.Table.Columns.Add(new WorksheetColumn(105));
                sheet.Table.Columns.Add(new WorksheetColumn(160));

                WorksheetRow row = sheet.Table.Rows.Add();
                int iCounter = 1;
                WorksheetCell cell1 = row.Cells.Add("Daily Report from Affixing Station");
                cell1.MergeAcross = 15; // Merge two cells together
                cell1.StyleID = "HeaderStyle3";

                row.Index = iCounter++;

                 row = sheet.Table.Rows.Add();

                 row.Index = iCounter++;
            
                row.Cells.Add(new WorksheetCell("Location", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ddllocation.SelectedItem.ToString(), "HeaderStyle2"));
                
                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                WorksheetCell cell6 = row.Cells.Add("Date: " + OrderDate.SelectedDate.ToString("dd/MM/yyyy") + " - " + HSRPAuthDate.SelectedDate.ToString("dd/MM/yyyy"));
                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = iCounter++;
              

                cell6.MergeAcross = 5; // Merge two cells togetherto 
                cell6.StyleID = "HeaderStyle2";
               
                row = sheet.Table.Rows.Add();
                row.Index = iCounter++;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = iCounter++;
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle6"));
                WorksheetCell cell11 = row.Cells.Add("Cash Receipt");
                cell11.MergeAcross = 1;
                cell11.StyleID = "HeaderStyle6";
                row.Cells.Add(new WorksheetCell("Date of Affixation", "HeaderStyle6"));
                WorksheetCell cell12 = row.Cells.Add("Affixation Invoice");
                cell12.MergeAcross = 1;
                cell12.StyleID = "HeaderStyle6";
                row.Cells.Add(new WorksheetCell("No.of days for Affixation","HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Category", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Registration Number", "HeaderStyle6"));
                WorksheetCell cell122 = row.Cells.Add("Laser Identification No");
                cell122.MergeAcross = 1; // Merge two cells together
                cell122.StyleID = "HeaderStyle6";
                WorksheetCell cell2 = row.Cells.Add("High Security Registration Plate Size");
                cell2.MergeAcross = 1; // Merge two cells together
                cell2.StyleID = "HeaderStyle6";
                row.Cells.Add(new WorksheetCell("3rd Plate  Y/N", "HeaderStyle6"));
                WorksheetCell cell3 = row.Cells.Add("Colour Background");
                cell3.MergeAcross = 1; // Merge two cells together
                cell3.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                row.Index = iCounter++;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                
                row.Cells.Add(new WorksheetCell("Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Yellow", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("White", "HeaderStyle6"));
               
                row = sheet.Table.Rows.Add();

                UserType = Convert.ToInt32(Session["UserType"]);



                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["CashNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["CashDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["ordercloseddate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["DeliveryChallanNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["ordercloseddate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["No_of_days"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle1"));
                       
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["FrontPlateSize"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RearPlateSize1"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle1"));
                        if (dtrows["ProductColor"].ToString() == "YELLOW")
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
                        }
                    }


                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);

                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();

                }



            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label1.Visible = true;
            ddllocation.Visible = true;
            FilldropDownListLocation();
        }
    }
}