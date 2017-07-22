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
    public partial class DailyMachineProduction : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
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

                        }
                        else
                        {
                            
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            labelDate.Visible = false;
                           
                            FilldropDownListClient();
                            FilldropDownListOrganization(); 
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
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";

                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'  Order by RTOLocationName";

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
            try
            {
                LabelError.Text = "";

            String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            String ReportDate = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];
            OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            
            DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate); 
            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

            
              UserType = Convert.ToInt32(Session["UserType"]);
              if (UserType == 0)
              {
                  SQLString = "Report_Daily_MachineWisePRODUCTION'" + OrderDate1 + "','" + intHSRPStateID + "','" + intRTOLocationID + "'";
              }
              else
              {
                  HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                  RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString()); 
                  SQLString = "Report_Daily_MachineWisePRODUCTION'" + OrderDate1 + "','" + HSRPStateID + "','" + RTOLocationID + "'";
              } 
              DataSet dt = Utils.getDataSet(SQLString, CnnString); 
              if (dt.Tables[0].Rows.Count > 0)
              {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "REPORT_DAILY_MACHINE_PRODUCTION-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Daily Machine Production Report";
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
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                Worksheet sheet = book.Worksheets.Add("HSRP Daily Machine Production Report");

                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(205));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(130));
               
                //sheet.Range["A1"].Style.WrapText = true;
                

                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(112));
                sheet.Table.Columns.Add(new WorksheetColumn(109));
                sheet.Table.Columns.Add(new WorksheetColumn(105));
                sheet.Table.Columns.Add(new WorksheetColumn(160));

                WorksheetRow row = sheet.Table.Rows.Add();
                // row.
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3")); 
                WorksheetCell cell = row.Cells.Add("HSRP Daily Machine Production Report");
                cell.MergeAcross = 4; // Merge two cells together
                cell.StyleID = "HeaderStyle3";
                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();

                row.Index = 4;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));
                //row.Cells.Add(<br>); 
                row = sheet.Table.Rows.Add();

                // Skip one row, and add some text
                row.Index = 5; 

                DateTime date = System.DateTime.Now;
                string formatted = date.ToString("dd/MM/yyyy");
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(formatted, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 6;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                 
               
                row.Index = 8; 
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Machine Type", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Daily Target", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Daily Actual Target", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Month Target", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Month Actual Target", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Year Target", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Year Actual Target", "HeaderStyle"));
                row = sheet.Table.Rows.Add();
                row.Index = 9;
                row.Cells.Add(new WorksheetCell(dt.Tables[0].Rows[0]["uom"].ToString(), "HeaderStyle"));
                row.Cells.Add(new WorksheetCell(dt.Tables[0].Rows[0]["machinetype"].ToString(), "HeaderStyle"));
                row = sheet.Table.Rows.Add();
               
                //string SQLString = String.Empty;
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                
                if (dt.Tables[0].Rows.Count <= 0)
                {
                    LabelError.Text = string.Empty;
                    LabelError.Text = "Their is no selected data for the selected  date range.";
                    return;
                }
                row.Index = 10;
                Int64 DailyTarget = 0;
                Int64 monthtarget = 0;
                Int64 dailyactual = 0;
                Int64 monthactual = 0;

                Int64 yrstarget = 0;
                Int64 yrsactual = 0;
                int sno = 0; 

                foreach (DataRow dtrows in dt.Tables[0].Rows) // Loop over the rows.
                {
                    sno = sno+1;
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(dtrows["machinename"].ToString(), DataType.String, "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(dtrows["daily_target"].ToString(), DataType.String, "HeaderStyle5"));
                    row.Cells.Add(new WorksheetCell(dtrows["daily_actual"].ToString(), DataType.String, "HeaderStyle5"));
                    row.Cells.Add(new WorksheetCell(dtrows["month_target"].ToString(), DataType.String, "HeaderStyle5"));
                    row.Cells.Add(new WorksheetCell(dtrows["month_actual"].ToString(), DataType.String, "HeaderStyle5"));
                    row.Cells.Add(new WorksheetCell(dtrows["yrs_target"].ToString(), DataType.String, "HeaderStyle5"));
                    row.Cells.Add(new WorksheetCell(dtrows["yrs_actual"].ToString(), DataType.String, "HeaderStyle5")); 
                    DailyTarget = +DailyTarget + Convert.ToInt64(dt.Tables[0].Rows[0]["daily_target"]);
                    dailyactual = + dailyactual + Convert.ToInt64(dtrows["daily_actual"]); 
                    monthtarget = +monthtarget + Convert.ToInt64(dt.Tables[0].Rows[0]["month_target"]);
                    monthactual = +monthactual + Convert.ToInt64(dtrows["month_actual"]); 
                    yrstarget = +yrstarget + Convert.ToInt64(dt.Tables[0].Rows[0]["yrs_target"]);
                    yrsactual = +yrsactual + Convert.ToInt64(dtrows["yrs_actual"]); 
                }
                row = sheet.Table.Rows.Add();
                 
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2")); 
                //row.Cells.Add(<br>); 
                row = sheet.Table.Rows.Add();
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell(Convert.ToInt64(DailyTarget).ToString(), "HeaderStyle5"));
                row.Cells.Add(new WorksheetCell(Convert.ToInt64(dailyactual).ToString(), "HeaderStyle5"));
                row.Cells.Add(new WorksheetCell(Convert.ToInt64(monthtarget).ToString(), "HeaderStyle5"));
                row.Cells.Add(new WorksheetCell(Convert.ToInt64(monthactual).ToString(), "HeaderStyle5"));
                row.Cells.Add(new WorksheetCell(Convert.ToInt64(yrstarget).ToString(), "HeaderStyle5"));
                row.Cells.Add(new WorksheetCell(Convert.ToInt64(yrsactual).ToString(), "HeaderStyle5"));
                //row.Cells.Add(<br>); 
                row = sheet.Table.Rows.Add();
                HttpContext context = HttpContext.Current;
                context.Response.Clear();
                // Save the file and open it
                book.Save(Response.OutputStream);
                
                //context.Response.ContentType = "text/csv";
                context.Response.ContentType = "application/vnd.ms-excel";

                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.End();
            }
            else
            {
                 string closescript1 = "<script>alert('No records found for selected date.')</script>";
                    Page.RegisterStartupScript("abc", closescript1);
                    return;
            }
            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
    }

}