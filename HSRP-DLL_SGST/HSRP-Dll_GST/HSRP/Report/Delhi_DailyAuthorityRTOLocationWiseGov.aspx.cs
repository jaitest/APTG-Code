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
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;

namespace HSRP.Report
{
    public partial class Delhi_DailyAuthorityRTOLocationWiseGov : System.Web.UI.Page
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
        string strInvoiceNo = string.Empty;
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
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            labelClient.Visible = true;

                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            // labelClient.Visible = false;
                            // dropDownListClient.Visible = false;

                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            labelOrganization.Enabled = true;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = true;
                           // labelDate.Visible = false;


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

        private void FilldropDownListClient()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";

                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
            }
        }

        #endregion

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
            String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

            string filename = DropDownListStateName.SelectedItem.Text + "_DailyAuthorityRTOLocationReport_"+ System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";


            string SQLString = "SELECT ROW_NUMBER() Over (Order by a.hsrprecordid) As SNo, CONVERT(varchar(20), a.HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,a.address1,a.ordertype, CONVERT(varchar(20), a.OrderDate, 103) AS OrderDate, CONVERT(varchar(20),a.OrderClosedDate,103) as OrderClosedDate,a.HSRPRecordID,a.CashReceiptNo,a.InvoiceNo,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime, VehicleClass,CONVERT(numeric,round(a.RoundOff_NetAmount,0)) as NetAmount, a.HSRPRecord_AuthorizationNo, a.OwnerName, a.VehicleClass, a.VehicleType, a.VehicleRegNo, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.StickerMandatory,  (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as FrontPlateCode, (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as RearPlateCode,case when d.vehicleregno is null then '' else '2' end as 'Exists_or_not' " +
                   "FROM HSRPRecords a left join delhiolddata d on d.vehicleregno = a.vehicleregno " +
                   "where a.HSRP_StateID='" + DropDownListStateName.SelectedValue.ToString() + "' and a.RTOLocationID='" + dropDownListClient.SelectedValue + "' and convert(date,a.OrderClosedDate) between '" + StringOrderDate + "' and '" + StringAuthDate + "' and  a.OwnerName is not null and a.OwnerName <> '' and  a.OwnerName is not null and a.OwnerName <> '' and  a.Address1 is not null and a.Address1 <> ''  order by a.hsrprecordid  desc";

            Workbook book = new Workbook();
            StyleForTheFirstTime(book);
            DataTable dtRecord = GetDataTableFromQuery(SQLString);
            if (dtRecord.Rows.Count > 0)
            {
                DownLoadPDF(dtRecord);
                //ExportRecordExcel(book, dtRecord);
                //HttpContext context = HttpContext.Current;
                //context.Response.Clear();
                //book.Save(Response.OutputStream);
                //context.Response.ContentType = "application/vnd.ms-excel";
                //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                //context.Response.End();
            }
            else
            {
                LabelError.Text = "No Record For the Selected Date.";
            }

        }
        protected void btnAllLocationPdf_Click(object sender, EventArgs e)
        {
            int iCheckAllRtoHasNoRecord = 0;
            String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
            String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

            string filename = DropDownListStateName.SelectedItem.Text + "_DailyAuthorityRTOLocationReport_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
            Workbook book = new Workbook();
            StyleForTheFirstTime(book);
            DataTable dtrto = Utils.GetDataTable("select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid=2) and RTOLocationID not in (148)", CnnString);
            for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
            {
                string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
               
                string SQLString = "SELECT CONVERT(varchar(20), a.HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,a.address1,a.ordertype, CONVERT(varchar(20), a.OrderDate, 103) AS OrderDate, CONVERT(varchar(20),a.OrderClosedDate,103) as OrderClosedDate,a.HSRPRecordID,a.CashReceiptNo,a.InvoiceNo,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime, VehicleClass,CONVERT(numeric,round(a.RoundOff_NetAmount,0)) as NetAmount, a.HSRPRecord_AuthorizationNo, a.OwnerName, a.VehicleClass, a.VehicleType, a.VehicleRegNo, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.StickerMandatory,  (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as FrontPlateCode, (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as RearPlateCode,case when d.vehicleregno is null then '' else '2' end as 'Exists_or_not' " +
                      "FROM HSRPRecords a left join delhiolddata d on d.vehicleregno = a.vehicleregno " +
                      "where a.HSRP_StateID='" + DropDownListStateName.SelectedValue.ToString() + "' and a.RTOLocationID='" + RTOCode + "' and convert(date,a.OrderClosedDate) between '" + StringOrderDate + "' and '" + StringAuthDate + "' and  a.OwnerName is not null and a.OwnerName <> '' and  a.OwnerName is not null and a.OwnerName <> '' and  a.Address1 is not null and a.Address1 <> ''  order by OrderClosedDate";

                DataTable dtRecord = Utils.GetDataTable(SQLString, CnnString);
                if (dtRecord.Rows.Count > 0)
                {
                    iCheckAllRtoHasNoRecord++;
                    ExportRecordExcel(book, dtRecord, RTOName, RTOCode);
                }
            }
            if (iCheckAllRtoHasNoRecord > 0)
            {
                HttpContext context = HttpContext.Current;
                context.Response.Clear();
                book.Save(Response.OutputStream);
                context.Response.ContentType = "application/vnd.ms-excel";

                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.End();
            }
            else
            {
                LabelError.Text = "No Record For the Selected Date.";
            }
        }
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
        private DataTable GetDataTableFromQuery(string strSQLString)
        {
            SqlCommand cmd = new SqlCommand(strSQLString, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.CommandTimeout = 0;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        #region Excel Creation
        private static void StyleForTheFirstTime(Workbook book)
        {
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.ExcelWorkbook.WindowTopX = 100;
            book.ExcelWorkbook.WindowTopY = 200;
            book.ExcelWorkbook.WindowHeight = 7000;
            book.ExcelWorkbook.WindowWidth = 8000;

            // Some optional properties of the Document
            book.Properties.Author = "HSRP";
            book.Properties.Title = "HSRP Authority Report Order Closed";
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


            WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
            style9.Font.FontName = "Tahoma";
            style9.Font.Size = 10;
            style9.Font.Bold = true;
            style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style9.Interior.Color = "#FCF6AE";
            style9.Interior.Pattern = StyleInteriorPattern.Solid;
        }
        private void ExportRecordExcel(Workbook book, DataTable dt, string RtoName = null, string RtoCode = null)
        {
            try
            {
                LabelError.Text = "";
                string strRtoName = String.Empty;
                string strRtoCode = String.Empty;
                if (string.IsNullOrEmpty(RtoName))
                {
                    strRtoName = dropDownListClient.SelectedItem.ToString();
                }
                else
                {
                    strRtoName = RtoName;
                }

                Worksheet sheet = book.Worksheets.Add(strRtoName);
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));

                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(150));
                sheet.Table.Columns.Add(new WorksheetColumn(92));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(140));
                sheet.Table.Columns.Add(new WorksheetColumn(140));
                sheet.Table.Columns.Add(new WorksheetColumn(100));

                WorksheetRow row = sheet.Table.Rows.Add();

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("HSRP Authority Report Order Closed");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(strRtoName, "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd");
                String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd");

                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(StringOrderDate + " To " + StringAuthDate, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();


                row.Index = 5;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vechicle No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front Laser Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear Laser Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owner's Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owner's Address", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vechicle Type", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("New Vehicle/Old Vehicle", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Authorisation Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Affixation Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Old Registration Plate Count", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Plate Removed For Distraction", "HeaderStyle6"));


                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                row.Index = 6;





                string RTOColName = string.Empty;
                Int64 totalAmount = 0;
                if (dt.Rows.Count > 0)
                {
                    int sno = 0;
                    string VehicleColor = string.Empty;
                    string Color = string.Empty;


                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        if (dtrows["VehicleClass"].ToString() == "Non-Transport")
                        {
                            VehicleColor = "WHITE";
                        }
                        else
                        {
                            VehicleColor = "YELLOW";
                        }

                        sno = sno + 1;
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VEHICLeregno"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["address1"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VEHICLETYPE"].ToString(), DataType.String, "HeaderStyle"));
                        string strVeh = dtrows["ordertype"].ToString().Equals("NB") ? "New Vehicle" : "Old Vehicle";
                        row.Cells.Add(new WorksheetCell(strVeh, DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["HsrpRecord_AuthorizationDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Exists_or_not"].ToString(), DataType.String, "HeaderStyle"));



                        row = sheet.Table.Rows.Add();
                    }
                }
            }
            catch
            {
            }
        }
        #endregion
        public void DownLoadPDF(DataTable dt)
        {
            String ReportDateFrom = OrderDate.SelectedDate.ToString("yyyy/MM/dd");
            String ReportDateTo = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd");
            decimal totalAmount = 0;
            double tax = 0.0;
            double exicse = 0.0;
            double royalty = 0.0;
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                i = dt.Rows.Count;
                string filename = DropDownListStateName.SelectedItem.ToString() + "HSRP Authority Report Order Closed" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                StringBuilder bb = new StringBuilder();
                Document document = new Document();
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                //Adds content to the document:
                // document.Add(new Paragraph("Ignition Log Report"));
                PdfPTable table = new PdfPTable(10);
                //actual width of table in points
                table.TotalWidth = 2270f;

                //table.LockedWidth = true;
                //float[] TotalWidth = new float[] { 5f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f };
                //table.SetWidths(TotalWidth);

                PdfPCell cell120911 = new PdfPCell(new Phrase("HSRP Authority Report Order Closed", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120911.Colspan = 10;
                cell120911.BorderWidthLeft = 0f;
                cell120911.BorderWidthRight = 0f;
                cell120911.BorderWidthTop = 0f;
                cell120911.BorderWidthBottom = 0f;

                cell120911.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120911);

                PdfPCell cell12091 = new PdfPCell(new Phrase("State Name : " + DropDownListStateName.SelectedItem, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12091.Colspan = 3;
                cell12091.BorderWidthLeft = 1f;
                cell12091.BorderWidthRight = 0f;
                cell12091.BorderWidthTop = 1f;
                cell12091.BorderWidthBottom = 0f;

                cell12091.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12091);

                PdfPCell cell12096 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12096.Colspan = 2;
                cell12096.BorderWidthLeft = 0f;
                cell12096.BorderWidthRight = 0f;
                cell12096.BorderWidthTop = 1f;
                cell12096.BorderWidthBottom = 0f;

                cell12091.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12096);

                // PdfPCell cell12093 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                PdfPCell cell12092 = new PdfPCell(new Phrase("Location Name : " + dropDownListClient.SelectedItem, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12092.Colspan = 5;
                cell12092.BorderWidthLeft = 0f;
                cell12092.BorderWidthRight = 1f;
                cell12092.BorderWidthTop = 1f;
                cell12092.BorderWidthBottom = 0f;

                cell12092.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12092);

                PdfPCell cell12094 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12094.Colspan = 10;
                cell12094.BorderWidthLeft = 1f;
                cell12094.BorderWidthRight = 1f;
                cell12094.BorderWidthTop = 0f;
                cell12094.BorderWidthBottom = 0f;

                cell12094.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12094);

                PdfPCell cell12093 = new PdfPCell(new Phrase(" From : " + ReportDateFrom, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12093.Colspan = 5;
                cell12093.BorderWidthLeft = 1f;
                cell12093.BorderWidthRight = 0f;
                cell12093.BorderWidthTop = 0f;
                cell12093.BorderWidthBottom = 0f;

                cell12093.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12093);





                PdfPCell cell12095 = new PdfPCell(new Phrase(" To : " + ReportDateTo, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12095.Colspan = 5;
                cell12095.BorderWidthLeft = 0f;
                cell12095.BorderWidthRight = 1f;
                cell12095.BorderWidthTop = 0f;
                cell12095.BorderWidthBottom = 0f;

                cell12095.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12095);


                PdfPCell cell1209 = new PdfPCell(new Phrase("S.No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209.Colspan = 1;
                cell1209.BorderWidthLeft = 1f;
                cell1209.BorderWidthRight = 1f;
                cell1209.BorderWidthTop = 1f;
                cell1209.BorderWidthBottom = 1f;

                cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209);


                PdfPCell cell1210 = new PdfPCell(new Phrase("Vechicle No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1210.Colspan = 1;
                cell1210.BorderWidthLeft = 0f;
                cell1210.BorderWidthRight = .8f;
                cell1210.BorderWidthTop = 1f;
                cell1210.BorderWidthBottom = 1f;

                cell1210.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1210);



                PdfPCell cell1213 = new PdfPCell(new Phrase("Front Laser Number", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1213.Colspan = 1;
                cell1213.BorderWidthLeft = 0f;
                cell1213.BorderWidthRight = .8f;
                cell1213.BorderWidthTop = 1f;
                cell1213.BorderWidthBottom = 1f;

                cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1213);


                PdfPCell cell12233 = new PdfPCell(new Phrase("Rear Laser Number", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12233.Colspan = 1;
                cell12233.BorderWidthLeft = 0f;
                cell12233.BorderWidthRight = .8f;
                cell12233.BorderWidthTop = 1f;
                cell12233.BorderWidthBottom = 1f;

                cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12233);

                PdfPCell cell122331 = new PdfPCell(new Phrase("Owner's Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122331.Colspan = 1;
                cell122331.BorderWidthLeft = 0f;
                cell122331.BorderWidthRight = .8f;
                cell122331.BorderWidthTop = 1f;
                cell122331.BorderWidthBottom = 1f;

                cell122331.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122331);


                PdfPCell cell122332 = new PdfPCell(new Phrase("Owner's Address", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122332.Colspan = 1;
                cell122332.BorderWidthLeft = 0f;
                cell122332.BorderWidthRight = .8f;
                cell122332.BorderWidthTop = 1f;
                cell122332.BorderWidthBottom = 1f;

                cell122332.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122332);

                PdfPCell cell1206 = new PdfPCell(new Phrase("Vechicle Type", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1206.Colspan = 1;
                cell1206.BorderWidthLeft = 0f;
                cell1206.BorderWidthRight = .8f;
                cell1206.BorderWidthTop = 1f;
                cell1206.BorderWidthBottom = 1f;
                cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1206);

                //PdfPCell cell1221 = new PdfPCell(new Phrase("New Vehicle/Old Vehicle", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1221.Colspan = 1;
                //cell1221.BorderWidthLeft = 0f;
                //cell1221.BorderWidthRight = 1f;
                //cell1221.BorderWidthTop = 1f;
                //cell1221.BorderWidthBottom = 1f;

                //cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1221);


                PdfPCell cell120933 = new PdfPCell(new Phrase("Authorisation Date", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933.Colspan = 1;
                cell120933.BorderWidthLeft = 1f;
                cell120933.BorderWidthRight = .8f;
                cell120933.BorderWidthTop = 1f;
                cell120933.BorderWidthBottom = 1f;

                cell120933.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120933);



                PdfPCell cell120935 = new PdfPCell(new Phrase("Order Date", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120935.Colspan = 1;
                cell120935.BorderWidthLeft = 1f;
                cell120935.BorderWidthRight = .8f;
                cell120935.BorderWidthTop = 1f;
                cell120935.BorderWidthBottom = 1f;

                cell120935.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120935);

                PdfPCell cell120936 = new PdfPCell(new Phrase("Affixation Date", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120936.Colspan = 1;
                cell120936.BorderWidthLeft = 1f;
                cell120936.BorderWidthRight = .8f;
                cell120936.BorderWidthTop = 1f;
                cell120936.BorderWidthBottom = 1f;

                cell120936.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120936);

                //PdfPCell cell120937 = new PdfPCell(new Phrase("Old Registration Plate Count", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell120937.Colspan = 1;
                //cell120937.BorderWidthLeft = 1f;
                //cell120937.BorderWidthRight = .8f;
                //cell120937.BorderWidthTop = 1f;
                //cell120937.BorderWidthBottom = 1f;

                //cell120937.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell120937);

                //PdfPCell cell120938 = new PdfPCell(new Phrase("Plate Removed For Distraction", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell120938.Colspan = 1;
                //cell120938.BorderWidthLeft = 1f;
                //cell120938.BorderWidthRight = .8f;
                //cell120938.BorderWidthTop = 1f;
                //cell120938.BorderWidthBottom = 1f;

                //cell120938.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right                
                //table.AddCell(cell120938);

                //PdfPCell cell1209349 = new PdfPCell(new Phrase("Amount", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1209349.Colspan = 1;
                //cell1209349.BorderWidthLeft = 1f;
                //cell1209349.BorderWidthRight = .8f;
                //cell1209349.BorderWidthTop = 1f;
                //cell1209349.BorderWidthBottom = 1f;

                //cell1209349.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1209349);
                ////PdfPCell cell1223 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                ////cell1223.Colspan = 2;
                ////cell1223.BorderWidthLeft = 0f;
                ////cell1223.BorderWidthRight = 1f;
                ////cell1223.BorderWidthTop = 1f;
                ////cell1223.BorderWidthBottom = 1f;
                ////cell1223.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                ////table.AddCell(cell1223);
                i = i - 1;




                while (i >= 0)
                {

                    PdfPCell cell1211 = new PdfPCell(new Phrase(dt.Rows[i]["SNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1211.Colspan = 1;
                    cell1211.BorderWidthLeft = 1f;
                    cell1211.BorderWidthRight = 1f;
                    cell1211.BorderWidthTop = .5f;
                    cell1211.BorderWidthBottom = .5f;

                    cell1211.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1211);




                    strInvoiceNo = dt.Rows[i]["VehicleRegNo"].ToString();
                        PdfPCell cell1212 = new PdfPCell(new Phrase(strInvoiceNo.ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1212.Colspan = 1;
                        cell1212.BorderWidthLeft = 1f;
                        cell1212.BorderWidthRight = .8f;
                        cell1212.BorderWidthTop = .5f;
                        cell1211.BorderWidthBottom = .5f;

                        cell1212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1212);

                    


                    //row.Cells.Add(new WorksheetCell(dtrows["InvoiceNo"].ToString(), DataType.String, "HeaderStyle"));
                        PdfPCell cell1214 = new PdfPCell(new Phrase(dt.Rows[i]["HSRP_Front_LaserCode"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1214.Colspan = 1;
                    cell1214.BorderWidthLeft = 0f;
                    cell1214.BorderWidthRight = .8f;
                    cell1214.BorderWidthTop = .5f;
                    cell1214.BorderWidthBottom = .5f;

                    cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1214);





                    //row.Cells.Add(new WorksheetCell(dtrows["InvoiceDateTime"].ToString(), DataType.String, "HeaderStyle"));
                    PdfPCell cell1219 = new PdfPCell(new Phrase(dt.Rows[i]["HSRP_Rear_LaserCode"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1219.Colspan = 1;
                    cell1219.BorderWidthLeft = 0f;
                    cell1219.BorderWidthRight = .8f;
                    cell1219.BorderWidthTop = .5f;
                    cell1219.BorderWidthBottom = .5f;

                    cell1219.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell1219);


                    //PdfPCell cell1219 = new PdfPCell(new Phrase(dt.Rows[i]["CashReceiptNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell1219.Colspan = 1;
                    //cell1219.BorderWidthLeft = 0f;
                    //cell1219.BorderWidthRight = .8f;
                    //cell1219.BorderWidthTop = .5f;
                    //cell1219.BorderWidthBottom = .5f;

                    //cell1219.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    //table.AddCell(cell1219);


                    PdfPCell cell12193 = new PdfPCell(new Phrase(dt.Rows[i]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12193.Colspan = 1;
                    cell12193.BorderWidthLeft = 0f;
                    cell12193.BorderWidthRight = .8f;
                    cell12193.BorderWidthTop = .5f;
                    cell12193.BorderWidthBottom = .5f;

                    cell12193.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell12193);


                    PdfPCell cell12194 = new PdfPCell(new Phrase(dt.Rows[i]["address1"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12194.Colspan = 1;
                    cell12194.BorderWidthLeft = 0f;
                    cell12194.BorderWidthRight = .8f;
                    cell12194.BorderWidthTop = .5f;
                    cell12194.BorderWidthBottom = .5f;

                    cell12194.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell12194);


                    PdfPCell cell1216 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1216.Colspan = 1;
                    cell1216.BorderWidthLeft = 0f;
                    cell1216.BorderWidthRight = .8f;
                    cell1216.BorderWidthTop = .5f;
                    cell1216.BorderWidthBottom = .5f;

                    cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1216);


                    //PdfPCell cell1222 = new PdfPCell(new Phrase(dt.Rows[i]["ordertype"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell1222.Colspan = 1;
                    //cell1222.BorderWidthLeft = 0f;
                    //cell1222.BorderWidthRight = .8f;
                    //cell1222.BorderWidthTop = .5f;
                    //cell1222.BorderWidthBottom = .5f;

                    //cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell1222);




                    PdfPCell cell120939 = new PdfPCell(new Phrase(dt.Rows[i]["HsrpRecord_AuthorizationDate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell120939.Colspan = 1;
                    cell120939.BorderWidthLeft = 0f;
                    cell120939.BorderWidthRight = .8f;
                    cell120939.BorderWidthTop = .5f;
                    cell120939.BorderWidthBottom = .5f;

                    cell120935.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120939);

                    PdfPCell cell120940 = new PdfPCell(new Phrase(dt.Rows[i]["OrderDate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell120940.Colspan = 1;
                    cell120940.BorderWidthLeft = 0f;
                    cell120940.BorderWidthRight = .8f;
                    cell120940.BorderWidthTop = .5f;
                    cell120940.BorderWidthBottom = .5f;

                    cell120940.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120940);



                    PdfPCell cell120941 = new PdfPCell(new Phrase(dt.Rows[i]["OrderClosedDate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell120941.Colspan = 1;
                    cell120941.BorderWidthLeft = 0f;
                    cell120941.BorderWidthRight = .8f;
                    cell120941.BorderWidthTop = .5f;
                    cell120941.BorderWidthBottom = .5f;

                    cell120941.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120941);

                    //PdfPCell cell120942 = new PdfPCell(new Phrase(dt.Rows[i]["RearplateCode"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell120942.Colspan = 1;
                    //cell120942.BorderWidthLeft = 0f;
                    //cell120942.BorderWidthRight = .8f;
                    //cell120942.BorderWidthTop = .5f;
                    //cell120942.BorderWidthBottom = .5f;

                    //cell120942.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell120942);

                    //PdfPCell cell120943 = new PdfPCell(new Phrase(dt.Rows[i]["StickerMandatory"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell120943.Colspan = 1;
                    //cell120943.BorderWidthLeft = 0f;
                    //cell120943.BorderWidthRight = .8f;
                    //cell120943.BorderWidthTop = .5f;
                    //cell120943.BorderWidthBottom = .5f;

                    //cell120943.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell120943);

                    //PdfPCell cell120944 = new PdfPCell(new Phrase(dt.Rows[i]["NetAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell120944.Colspan = 1;
                    //cell120944.BorderWidthLeft = 0f;
                    //cell120944.BorderWidthRight = .8f;
                    //cell120944.BorderWidthTop = .5f;
                    //cell120944.BorderWidthBottom = .5f;

                    //cell120944.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell120944);
                    i--;
                }


                //document.Add(table);
                //PdfPCell cell12241 = new PdfPCell(new Phrase("(Sign)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell12241.Colspan = 7;
                //cell12241.BorderWidthLeft = 0f;
                //cell12241.BorderWidthRight = 0f;
                //cell12241.BorderWidthTop = 0f;
                //cell12241.BorderWidthBottom = 0f;

                //cell12241.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell12241);

                document.Add(table);
                // document.Add(table1);

                document.Close();
                HttpContext context = HttpContext.Current;

                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }
            else
            {
                string closescript1 = "<script>alert('No records found for selected date.')</script>";
                Page.RegisterStartupScript("abc", closescript1);
                return;
            }
        }
    }
}
