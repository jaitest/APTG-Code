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
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace HSRP.Report
{
    public partial class ViewStockDetails : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        int intHSRPStateID1;
        int intRTOLocationID1;
        string SQLString1 = string.Empty;
        string OrderType;
        string recordtype = string.Empty;
        string day1date1 = string.Empty;
        string day1date = string.Empty;
        string day2date = string.Empty;
        string day3date = string.Empty;
        string day4date = string.Empty;
        string day5date = string.Empty;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dtsession = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType1 = Convert.ToInt32(Session["UserType"]);
                CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID1 = Session["UID"].ToString();
                HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
                RTOLocationID1 = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {                   
                    try
                    {
                        if (UserType1.Equals(0))
                        {
                            //DropDownListStateName.Visible = true;
                            
                            FilldropDownListOrganization();
                            OrderDatefrom.SelectedDate = System.DateTime.Now;
                        }
                        else
                        {

                            //DropDownListStateName.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListEmbossingCenters();


                            OrderDatefrom.SelectedDate = System.DateTime.Now;
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
            if (UserType1.Equals(0))
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                ddlStateName.DataSource = dts;
                ddlStateName.DataTextField = "HSRPStateName";
                ddlStateName.DataValueField = "HSRP_StateID";
                ddlStateName.DataBind();
                ddlStateName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));                
            }
            else
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID1 + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                ddlStateName.DataSource = dts;
                ddlStateName.DataTextField = "HSRPStateName";
                ddlStateName.DataValueField = "HSRP_StateID";
                ddlStateName.DataBind();               
            }
        }

        private void FilldropDownListEmbossingCenters()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select EmbCenterName,Emb_Center_Id  from EmbossingCenters Where State_Id=" + ddlStateName.SelectedValue + " and ActiveStatus!='N' union  select distinct  'Central Warehouse',  navcentralwarehousecode   from RTOLocation Where navembid is not null  and isnull(EmbCenterName,'')!='' and hsrp_stateid =" + ddlStateName.SelectedValue + " Order by EmbCenterName";
                //SQLString1 = "select EmbCenterName,Emb_Center_Id from EmbossingCenters Where State_Id=" + ddlStateName.SelectedValue + " and ActiveStatus!='N' Order by EmbCenterName";
                DataSet dss = Utils.getDataSet(SQLString1, CnnString1);
                ddlEmbossingCenters.DataSource = dss;
                ddlEmbossingCenters.DataTextField = "EmbCenterName";
                ddlEmbossingCenters.DataValueField = "Emb_Center_Id";
                ddlEmbossingCenters.DataBind();
                ddlEmbossingCenters.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
                ddlEmbossingCenters.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All", "1"));
                //Utils.PopulateDropDownList(ddlEmbossingCenters, SQLString1.ToString(), CnnString1, "--Select Emb Center--");
            }
            else
            {
                SQLString1 = "select EmbCenterName,Emb_Center_Id  from EmbossingCenters Where State_Id=" + ddlStateName.SelectedValue + " and ActiveStatus!='N' union  select distinct  'Central Warehouse',  navcentralwarehousecode   from RTOLocation Where navembid is not null  and isnull(EmbCenterName,'')!='' and hsrp_stateid =" + ddlStateName.SelectedValue + " Order by EmbCenterName";
                //SQLString1 = "select EmbCenterName,Emb_Center_Id from EmbossingCenters Where State_Id=" + HSRP_StateID1 + " and ActiveStatus!='N' Order by EmbCenterName";
                //SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
                DataSet dss = Utils.getDataSet(SQLString1, CnnString1);
                ddlEmbossingCenters.DataSource = dss;
                ddlEmbossingCenters.DataTextField = "EmbCenterName";
                ddlEmbossingCenters.DataValueField = "Emb_Center_Id";
                ddlEmbossingCenters.DataBind();
                ddlEmbossingCenters.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
                ddlEmbossingCenters.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All", "1"));
            }
        }
        
        
        

        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        }

        private Boolean validate1()
        {
            Boolean blnvalid = true;
            String getvalue = string.Empty;
            getvalue = ddlStateName.SelectedItem.Text;
            if (getvalue == "--Select State--")
            {
                blnvalid = false;

                Label1.Text = "Please select State Name";

            }
            return blnvalid;

        }


        protected void btnexport_Click(object sender, EventArgs e)
        {


            try
            {

                Label1.Text = "";
                if (ddlStateName.SelectedValue == "0")
                {
                    Label1.Text = "Please State";
                    ddlStateName.Focus();
                    return;
                }
                if (ddlEmbossingCenters.SelectedValue == "0")
                {
                    Label1.Text = "Please Emb Center";
                    ddlEmbossingCenters.Focus();
                    return;
                }

                #region Fetch Data

                string EmbCenterCode;
                if (ddlEmbossingCenters.SelectedValue == "0")
                {
                    EmbCenterCode = "";
                }
                else if (ddlEmbossingCenters.SelectedValue == "1")
                {
                    EmbCenterCode = "";
                }
                else
                    EmbCenterCode = ddlEmbossingCenters.SelectedValue;

                SqlConnection con = new SqlConnection(CnnString1);

                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand("GetStockDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                string s = Convert.ToDateTime(OrderDatefrom.SelectedDate).ToString("MM/dd/yyyy");
                cmd.Parameters.Add(new SqlParameter("@CreateDatetime", s));
                cmd.Parameters.Add(new SqlParameter("@EmbCenterCode", EmbCenterCode));

                //cmd.Parameters.Add(new SqlParameter("@hsrp_stateid", Convert.ToInt32(ddlStateName.SelectedValue)));
                //string s = Convert.ToDateTime(OrderDatefrom.SelectedDate).ToString("MM/dd/yyyy");
                //cmd.Parameters.Add(new SqlParameter("@CreateDatetime", s));
                //cmd.Parameters.Add(new SqlParameter("@EmbCenterCode", EmbCenterCode));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);


                #endregion


                if (dt.Rows.Count > 0)
                {
                    string filename = "Physical VS ERP Stock Details" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();

                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "Physical VS ERP Stock Details";
                    book.Properties.Created = DateTime.Now;


                    // Add some styles to the Workbook
                    WorksheetStyle style = book.Styles.Add("HeaderStyle");
                    style.Font.FontName = "Tahoma";
                    style.Font.Size = 10;
                    style.Font.Bold = true;
                    style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                    style4.Font.FontName = "Tahoma";
                    style4.Font.Size = 10;
                    style4.Font.Bold = false;
                    style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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

                    Worksheet sheet = book.Worksheets.Add("Bank Detail Report");


                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));


                    WorksheetRow row = sheet.Table.Rows.Add();
                    row.Index = 3;
                    WorksheetCell cell = row.Cells.Add("Report : " + "Stock Detail");
                    cell.MergeAcross = 3;
                    cell.StyleID = "HeaderStyle2";
                    row = sheet.Table.Rows.Add();


                    row.Index = 5;

                    WorksheetCell cell1 = row.Cells.Add("Emb Center code:");
                    row.Cells.Add(new WorksheetCell(ddlEmbossingCenters.SelectedItem.ToString(), "HeaderStyle2"));
                    cell1.MergeAcross = 1;
                    cell1.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();

                    row.Index = 6;
                    WorksheetCell cell2 = row.Cells.Add("Report Date");
                    row.Cells.Add(new WorksheetCell(OrderDatefrom.SelectedDate.ToString("dd/MM/yyyy"), "HeaderStyle2"));
                    cell2.MergeAcross = 1;
                    cell2.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();

                    row.Index = 7;
                    WorksheetCell cell3 = row.Cells.Add("Report Generated Date:");
                    //row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MM/yyyy"), "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "HeaderStyle2")); 
                    cell3.MergeAcross = 1;
                    cell3.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();


                    row.Index = 10;

                    row.Cells.Add(new WorksheetCell("S.No", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("EmbCenterName", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("ProductName", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("CreateBy", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Create Datetime", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Physical Quantity", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("ERP Quantity ", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Diff", "HeaderStyle"));


                    int sno = 0;
                    int totPhysicalQuantity = 0;
                    int toterpQuantity = 0;
                    int toterpdiff = 0;

                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["EmbCenterName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["ProductName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["CreateBy"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["CreateDatetime"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["PhysicalQuantity"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["ERPQuantity"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Diff"].ToString(), DataType.String, "HeaderStyle1"));
                        totPhysicalQuantity = totPhysicalQuantity + Convert.ToInt32(dtrows["PhysicalQuantity"].ToString());
                        toterpQuantity = toterpQuantity + Convert.ToInt32(dtrows["ERPQuantity"].ToString());
                        toterpdiff = toterpdiff + Convert.ToInt32(dtrows["Diff"].ToString());

                    }
                    row = sheet.Table.Rows.Add();

                    row.Index = 11 + dt.Rows.Count;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    WorksheetCell cell11 = row.Cells.Add("Total");
                    row.Cells.Add(new WorksheetCell(Convert.ToString(totPhysicalQuantity), "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell(Convert.ToString(toterpQuantity), "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell(Convert.ToString(toterpdiff), "HeaderStyle1"));
                    cell11.MergeAcross = 0;
                    cell11.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();


                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);
                    context.Response.ContentType = "application/vnd.ms-excel";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();

                }

                else
                {
                    Label1.Visible = true;
                    Label1.Text = "Record Not Found";
                    return;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }




        }

        //protected void btnexport_Click(object sender, EventArgs e)
        //{
        //    SaveAndDownloadFile();
        //}

        private void SaveAndDownloadFile()
        {
            //Label1.Text = "";
            //if (ddlStateName.SelectedValue == "0")
            //{
            //    Label1.Text = "Please State";                
            //    ddlStateName.Focus();
            //    return;
            //}
            //if (ddlEmbossingCenters.SelectedValue == "0")
            //{
            //    Label1.Text = "Please Emb Center";
            //    ddlEmbossingCenters.Focus();
            //    return;
            //}
            //Workbook book = new Workbook();
            //string filename = "Physical VS ERP Stock Details" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            //string strOrderType = string.Empty;

            //Export(strOrderType, book, 1, "GetStockDetails");

            //HttpContext context = HttpContext.Current;
            //context.Response.Clear();
            //// Save the file and open it
            //book.Save(Response.OutputStream);
            //context.Response.ContentType = "application/vnd.ms-excel";

            //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            //context.Response.End();

        }

        int icount = 0;

        private void Export(string strReportType, Workbook book, int iActiveSheet, string strProcName)
        {
            try
            {
                SqlConnection con = new SqlConnection(CnnString1);

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = iActiveSheet;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Physical VS ERP Stock Details";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook

                #region Styles
                if (icount <= 0)
                {
                    icount++;
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
                    style9.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style9.Interior.Color = "#FCF6AE";
                    style9.Interior.Pattern = StyleInteriorPattern.Solid;

                }
                #endregion

                Worksheet sheet = book.Worksheets.Add("Report");
                string EmbCenterCode;
                if (ddlEmbossingCenters.SelectedValue == "0")
                {
                    EmbCenterCode = "";
                }
                else if (ddlEmbossingCenters.SelectedValue == "1")
                {
                    EmbCenterCode = "";
                }
                else
                    EmbCenterCode=ddlEmbossingCenters.SelectedValue;
                
                
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand(strProcName, con);
                cmd.CommandType = CommandType.StoredProcedure;

                //cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
                string s = Convert.ToDateTime(OrderDatefrom.SelectedDate).ToString("MM/dd/yyyy");
                cmd.Parameters.Add(new SqlParameter("@CreateDatetime",s));
                cmd.Parameters.Add(new SqlParameter("@EmbCenterCode", EmbCenterCode));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //  dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    
                }
                else
                {
                    Label1.Text = "Record Not Found";
                    return;
                }
                #endregion

                AddColumnToSheet(sheet, 100, dt.Columns.Count);
                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                WorksheetCell cell = row.Cells.Add("Stock Detail");
                cell.MergeAcross = 4; // Merge two cells together
                cell.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                AddNewCell(row, "Emb Center code:", "HeaderStyle2", 1);
                AddNewCell(row, ddlEmbossingCenters.SelectedItem.ToString(), "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                AddNewCell(row, "Report Date:", "HeaderStyle2", 1);
                AddNewCell(row, OrderDatefrom.SelectedDate.ToString("dd/MM/yyyy"), "HeaderStyle2", 1);
                row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "HeaderStyle2")); 
                //row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                AddNewCell(row, "", "HeaderStyle2", 2);
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;
                //AddNewCell(row, "", "HeaderStyle6", 1);               
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                for (int i = 0; i < dt.Columns.Count; i++)
                {                   
                    AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);
                }
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);
                    }
                    row = sheet.Table.Rows.Add();

                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static void AddNewCell(WorksheetRow row, string strText, string strStyle, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                row.Cells.Add(new WorksheetCell(strText, strStyle));
        }

        private static void AddColumnToSheet(Worksheet sheet, int iWidth, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                sheet.Table.Columns.Add(new WorksheetColumn(iWidth));
        }

        protected void ddlStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListEmbossingCenters();
        }
    
    }
}
            
       