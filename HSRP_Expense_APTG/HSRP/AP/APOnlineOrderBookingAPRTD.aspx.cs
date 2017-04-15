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
using System;
using System.Collections;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Specialized;
namespace HSRP.AP
{
    public partial class APOnlineOrderBookingAPRTD : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string SQlQuery = string.Empty;
        string ExicseAmount = string.Empty;
        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        string strFrmDateString = string.Empty;
        string strToDateString = string.Empty;

        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();
        string StickerManditory = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
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
                
                if (!IsPostBack)
                {
                    try
                    {
                        if (UserType == "0")
                        {
                            lblAuthno.Visible = true;
                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            lblAuthno.Enabled = false;
                        }
                        InitialSetting();

                    }
                    catch (Exception err)
                    {
                        LblMessage.Text = "";
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
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

        private void ShowGrid()
        {

            SQLString = "Select ROW_NUMBER() Over (Order by H.id) As SNo,H.id,rtoCode,rtoName,affixationCenterCode,H.transactionNo,H.transactionDate,H.authorizationRefNo,authorizationDate,engineNo,chassisNo,prNumber,ownerName,ownerAddress,ownerEmailId,ownerPinCode,mobileNo,vehicleType,transType,vehicleClassType,mfrsName,modelName,hsrpFee,oldNewFlag,govtVehicleTag,timeStamp,trNumber,dealerName,dealerMail,dealerRtoCode,regDate from HsrpData H,Transaction_Stagging T where H.transactionNo=T.transactionNo and H.transactionDate=T.transactionDate and H.hsrpFee=T.amount and H.authorizationRefNo=T.authorizationRefNo and  H.authorizationRefNo not in (Select hsrprecord_Authorizationno from hsrprecords where hsrp_stateid=9) and convert(date,H.recordCreationDate)= convert(date,'" + OrderDate.SelectedDate + "') order by H.id";
            dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                btnSyncWithGovt.Visible = true;
                Panel1.Visible = true;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                lblErrMsg.Text = "";
                LblMessage.Text = "";
                btnExcel.Visible = true;
            }
            else
            {
                btnSyncWithGovt.Visible = false;
                Panel1.Visible = false;
                LblMessage.Text = "";
                lblErrMsg.Text = "Record not found.";
                GridView1.DataSource = null;
                btnExcel.Visible = false;
                GridView1.DataBind();

            }
        }
        StringBuilder sb = new StringBuilder();
        CheckBox chk;
        protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk1 = GridView1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
            if (chk1.Checked == true)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = true;
                }
            }
            else if (chk1.Checked == false)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = false;
                }
            }

        }


        protected void Grid1_ItemCommand(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {

        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                ShowGrid();
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }


        }
        DataTable dt = new DataTable();
        Label lblOnlinePaymentID;
        Label lblRoundoff_netAmount;
        string strId;
        public void updateAPOnlinePaymentAndSyncWithGovt()
        {
            try
            {
                //Opens the document:
                string strHsrpRecordId = string.Empty;

                #region Set ChallanNo
                string[] strArray;
                try
                {

                    if (GridView1.Rows.Count == 0)
                    {
                        LblMessage.Text = "";
                        lblErrMsg.Text = "No Record Found.";
                        return;

                    }
                    // Validate checked recirds
                    int ChkBoxCount = 0;
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                        if (chk.Checked == true)
                        {
                            ChkBoxCount = ChkBoxCount + 1;
                        }
                    }
                    if (ChkBoxCount == 0)
                    {
                        LblMessage.Text = "";
                        lblErrMsg.Text = "Please select atleast 1 record.";
                        return;
                    }
                }
                catch (Exception ex)
                {
                    lblErrMsg.Text = ex.Message.ToString();
                    return;
                }

                #endregion
                int iChkCount = 0;
                StringBuilder sbx = new StringBuilder();
                string OrderBookedId = string.Empty;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                    if (chk.Checked == true)
                    {
                        iChkCount = iChkCount + 1;
                        strId = GridView1.DataKeys[i]["ID"].ToString();
                        lblOnlinePaymentID = GridView1.Rows[i].Cells[3].FindControl("lblOnlinePaymentID") as Label;
                        lblRoundoff_netAmount = GridView1.Rows[i].Cells[4].FindControl("lblRoundoff_netAmount") as Label;
                        if (strHsrpRecordId == "")
                        {
                            strHsrpRecordId = strId;
                        }
                        else
                        {
                            strHsrpRecordId = strHsrpRecordId + "," + strId;
                        }
                        sbx.Append("update hsrpdata Set OnlinePaymentStatus='Y' where authorizationRefNo='" + strId + "'");
                    }
                }
                if (iChkCount > 0)
                {
                    int i = Utils.ExecNonQuery(sbx.ToString(), CnnString);
                    if (i > 0)
                    {
                        lblErrMsg.Text = "";
                        LblMessage.Text = "Record Booked successfully";
                    }
                    else
                    {
                        LblMessage.Text = "";
                        lblErrMsg.Text = "Record not Booked";
                    }
                }
                else
                {
                    lblErrMsg.Text = "";
                    LblMessage.Text = "";
                    lblErrMsg.Text = "Please Select At List One Record For Book";
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = "";
                lblErrMsg.Text = ex.Message;
            }
        }

        protected void btnChalan_Click(object sender, EventArgs e)
        {
            updateAPOnlinePaymentAndSyncWithGovt();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            Export();
        }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        private void Export()
        {
            try
            {
                LblMessage.Text = "";
                string SQLString = string.Empty;
                string flag = string.Empty;
                flag = "0";
                string type = "1";
                Workbook book = new Workbook();
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Report";
                book.Properties.Created = DateTime.Now;

                #region Fetch Data
                DataTable dt = new DataTable();
                SQLString = "Select ROW_NUMBER() Over (Order by H.id) As SNo,rtoCode,rtoName,affixationCenterCode,H.transactionNo,convert(date,H.transactionDate) as 'transactionDate',H.authorizationRefNo,convert(date,authorizationDate) as 'authorizationDate',engineNo,chassisNo,prNumber,ownerName,ownerAddress,ownerEmailId,ownerPinCode,mobileNo,vehicleType,transType,vehicleClassType,mfrsName,modelName,hsrpFee,oldNewFlag,govtVehicleTag,timeStamp,trNumber,dealerName,dealerMail,dealerRtoCode,convert(date,regDate) as 'regDate' from HsrpData H,Transaction_Stagging T where H.transactionNo=T.transactionNo and H.transactionDate=T.transactionDate and H.hsrpFee=T.amount and H.authorizationRefNo=T.authorizationRefNo and  H.authorizationRefNo not in (Select hsrprecord_Authorizationno from hsrprecords where hsrp_stateid=9) and convert(date,H.recordCreationDate)= convert(date,'" + OrderDate.SelectedDate + "') order by H.id";
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {


                #endregion


                    // Add some styles to the Workbook

                    #region Styles

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

                    WorksheetStyle styleHeader = book.Styles.Add("HeaderStyleHeader");
                    styleHeader.Font.FontName = "Tahoma";
                    styleHeader.Interior.Color = "Red";
                    styleHeader.Font.Size = 10;
                    styleHeader.Font.Bold = true;
                    styleHeader.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    styleHeader.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    styleHeader.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    styleHeader.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    styleHeader.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                    #endregion

                    Worksheet sheet = book.Worksheets.Add("Report");

                    #region UpperPart of Excel
                    AddColumnToSheet(sheet, 100, dt.Columns.Count);
                    int iIndex = 3;
                    WorksheetRow row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    //  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));

                    row.Cells.Add(new WorksheetCell("Book Order Report", "HeaderStyle3"));

                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    AddNewCell(row, "", "HeaderStyle2", 1);
                    AddNewCell(row, "", "HeaderStyle2", 1);
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    AddNewCell(row, "Report Date:", "HeaderStyle2", 1);
                    AddNewCell(row, System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
                    row = sheet.Table.Rows.Add();

                    row.Index = iIndex++;


                    row.Index = iIndex++;
                    #endregion

                    #region Column Creation and Assign Data
                    string RTOColName = string.Empty;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        AddNewCell(row, dt.Columns[i].ColumnName.ToString().Remove(dt.Columns[i].ColumnName.ToString().Length - 2, 0), "HeaderStyleHeader", 1);
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
                    row = sheet.Table.Rows.Add();


                    #endregion
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    // Save the file and open it
                    book.Save(Response.OutputStream);
                    lblErrMsg.Text = "";
                    //context.Response.ContentType = "text/csv";
                    context.Response.ContentType = "application/vnd.ms-excel";
                    string filename = "Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();
                }
                else
                {
                    lblErrMsg.Text = "Record Not Found";
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
    }
}
