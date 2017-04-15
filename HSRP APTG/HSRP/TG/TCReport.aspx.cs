
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using CarlosAg.ExcelXmlWriter;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace HSRP.TG
{
    public partial class TCReport : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        string SQLString1 = string.Empty;

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

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
                            FilldropddlRTOLocation();
                            InitialSetting();
                            //sms1();
                        }
                        else
                        {
                            FilldropddlRTOLocation();
                            InitialSetting();
                            //sms1();
                        }
                    }
                    catch (Exception err)
                    {
                        Label1.Text = err.Message.ToString();
                    }
                }
            }
        }

        private void FilldropddlRTOLocation()
        {

            SQLString1 = "Select RTOLocationID,RTOLocationName from RTOLocation where HSRP_StateID='" + HSRP_StateID1 + "' and ActiveStatus='Y' order by RTOLocationName";
            Utils.PopulateDropDownList(ddlRtoLocation, SQLString1.ToString(), CnnString1, "--Select Location--");
        }

        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            Datefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            Datefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            Dateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            Dateto.MaxDate = DateTime.Parse(MaxDate);
            CalendarDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {

                SqlConnection con = new SqlConnection(CnnString1);
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                //change sp Name
                cmd = new SqlCommand("TCReportRTOLocationAndDateWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@stateid", HSRP_StateID1));
                cmd.Parameters.Add(new SqlParameter("@location", ddlRtoLocation.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@fromdate", Datefrom.SelectedDate));
                cmd.Parameters.Add(new SqlParameter("@Todate", Dateto.SelectedDate));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                #endregion
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grd.DataSource = ds.Tables[0];
                    grd.DataBind();
                    grd.Visible = true;
                }
                else
                {
                    grd.DataSource = null;
                    grd.Visible = false;

                }
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message.ToString();
            }
        }

        int icount = 0;
        private void SaveAndDownloadFile2()
        {
            Workbook book = new Workbook();
            string filename = "TCReport" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            Export(strOrderType, book, 1, "TCReportRTOLocationAndDateWise");

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }
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
                book.Properties.Title = "TC Report";
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

                string strreptype = "E";
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand(strProcName, con);


                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@stateid", HSRP_StateID1));
                cmd.Parameters.Add(new SqlParameter("@location", ddlRtoLocation.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@fromdate", Datefrom.SelectedDate));
                cmd.Parameters.Add(new SqlParameter("@Todate", Dateto.SelectedDate));
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //  dt = new DataTable();
                da.Fill(dt);
                #endregion

                if (dt.Rows.Count > 0)
                {


                    AddColumnToSheet(sheet, 100, dt.Columns.Count);



                    int iIndex = 3;
                    WorksheetRow row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("TC Location wise report");
                    cell.MergeAcross = 4; // Merge two cells together
                    cell.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    AddNewCell(row, "State:", "HeaderStyle2", 1);
                    AddNewCell(row, "TG", "HeaderStyle2", 1);
                    AddNewCell(row, "RTO Location:", "HeaderStyle2", 1);
                    AddNewCell(row, ddlRtoLocation.SelectedItem.ToString(), "HeaderStyle2", 1);
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());


                    AddNewCell(row, "Report Date from:", "HeaderStyle2", 1);
                    AddNewCell(row, Datefrom.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);

                    AddNewCell(row, "Report Date To:", "HeaderStyle2", 1);
                    AddNewCell(row, Dateto.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);

                    row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                    AddNewCell(row, "", "HeaderStyle2", 2);
                    row = sheet.Table.Rows.Add();

                    row.Index = iIndex++;

                    AddNewCell(row, "", "HeaderStyle6", 1);
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
                else
                {
                    Label1.Visible = false;
                    Label1.Text = "Record not Found";
                    return;
                }

            }

            catch (Exception ex)
            {
                Label1.Text = ex.Message.ToString();
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
        protected void btndownloadDetail_Click(object sender, EventArgs e)
        {
            this.SaveAndDownloadFile2();
        }

        public void sms()
        {
            //string SMSText = " Cash Rs." + Math.Round(decimal.Parse(lblAmount.Text), 0) + " received against HSRP Authorization No. " + lblAuthNo.Text + " on " + System.DateTime.Now.ToString("dd/MM/yyyy") + " receipt number " + cashrc + ". HSRP Team.";
            //string sendURL = "http://quick.smseasy.in:8080/bulksms/bulksms?username=sse-tlhsrp1&password=tlhsrp1&type=0&dlr=1&destination=" + lblMobileNo.Text.ToString() + "&source=TSHSRP&message=" + SMSText;
                                http://quick.smseasy.in:8080/bulksms/bulksms?destination=918800233008&source=HSRPAP&message=Test+Message&username=aphsrp&password=Pass#1234&type=0    
            //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
            //myRequest.Method = "GET";
            //WebResponse myResponse = myRequest.GetResponse();
            //StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            //string result = sr.ReadToEnd();
            //sr.Close();
            //myResponse.Close();
            //System.Threading.Thread.Sleep(350);

            System.Net.WebRequest WebRequest = null;         //object for WebRequest  
            System.Net.WebResponse WebResonse = null;         //object for WebResponse  
            ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''         //             DEFINE PARAMETERS USED IN URL         ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''  
            //To what server you need to connect to for submission         //i.e. Dim Server As String = "1**.1**.1*1*"  
            string Server = "quick.smseasy.in:8080";         //if http then port is 8080         //if https then port is 8443   
            //Username that is to be used for submission           //i.e. Dim UserName As String = "tester"  
            string UserName = "aphsrp";
            // password that is to be used along with username         //i.e. Dim Password As String = "password"  
            string Password = "Pass#1234";
            //What type of the message that is to be sent.  
            //0:means plain text         //1:means flash         //2:means Unicode (Message content should be in Hex)  
            int MessageType = 0;
            string Message = "Test Message";
            //Url Encode message  
            Message = HttpUtility.UrlEncode(Message);
            if ((MessageType == 2))
            {
                Message = ConvertToUnicode(Message);
            }
            //Sender Id to be used for submitting the message  //i.e. Dim SenderName As String = "test"    
            string SenderName = "HSRPAP";
            //Destination Number to which message is to be sent  //i.e.  Dim MobileNo As String = "919544634"  
            //string MobileNo = "9199XXXXXXXX";
              string MobileNo = "919555410489";
            ///''''''''''''''''''''''CODE COMPLETE TO DEFINE PARAMETER''''''''''''''''''''''''''   
            string WebResponseString = "";
            string URL = "http://" + Server + "/bulksms/bulksms?destination=" + MobileNo + "&source=" + SenderName + "&message=" + Message + "&username=" + UserName + "&password=" + Password + "&type=" + MessageType; 
            WebRequest = System.Net.HttpWebRequest.Create(URL);  //Hit URL Link  
            WebRequest.Timeout = 25000;
            try
            {
                WebResonse = WebRequest.GetResponse();         //Get Response  
                System.IO.StreamReader reader = new System.IO.StreamReader(WebResonse.GetResponseStream());
                WebResponseString = reader.ReadToEnd();
                //Read Response and store in variable   
                WebResonse.Close();
                Response.Write(WebResponseString);         //Display Response.   
            }
            catch (Exception ex)
            {
                WebResponseString = "Request Timeout";         //If any exception occur.  
                Response.Write(WebResponseString);
            }
        }
        public string ConvertToUnicode(string str)
        {
            byte[] ArrayOFBytes = System.Text.Encoding.Unicode.GetBytes(str); string UnicodeString = ""; int v = 0; for (v = 0; v <= ArrayOFBytes.Length - 1; v++) { if (v % 2 == 0) { int t = ArrayOFBytes[v]; ArrayOFBytes[v] = ArrayOFBytes[v + 1]; ArrayOFBytes[v + 1] = Convert.ToByte(t); } }
            string c = BitConverter.ToString(ArrayOFBytes); c = c.Replace("-", "");
            UnicodeString = UnicodeString + c;
            return UnicodeString;

        }

        public void sms1()
        {
            //string SMSText = "test message";
            //string mobile = "919555410489";
            string[] ids = { "919999635810", "919711317109", "919555410489", "917827022755", "918447172743", "919899994673", "919818218224", "919810509118", "919958299100" };
            for (int i = 0; i < ids.Length; i++)
            {
                string SMSText = "test message" + ids[i].ToString(); ;
                string mobile = ids[i].ToString();
                string sendURL = "http://alerts.smseasy.in/api/v3/?method=sms&api_key=Acdd2370df44359714bb99c4ae21ef1c7&to=" + mobile + "&sender=APHSRP&message=" + SMSText;
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                myRequest.Method = "GET";
                WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                myResponse.Close();
                dynamic table = JsonConvert.DeserializeObject<dynamic>(result);
                Utils.ExecNonQuery("insert into smslog(smstext,mobileno,createtime,smsresponse) values('" + SMSText.ToString() + "','" + mobile + "',getdate(),'" + table.data.group_id + "')", CnnString1);
            }
        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    sms1();
        //}

    }
}