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

namespace HSRP.Transaction
{
    public partial class EmployeeMasterDetail : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString = string.Empty;

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
                        FilldropDownListState();
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        #region DropDown
        public void FilldropDownListState()
        {
            CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            SQLString = "select hsrp_stateid,   HSRPStateName from hsrpstate where  ActiveStatus='Y' Order by HSRPStateName";
            Utils.PopulateDropDownList(DDlState_Name, SQLString.ToString(), CnnString1, "--Select State Name--");

        }

        protected void DDlState_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = " select  CompanyName from hsrpstate where  hsrp_stateid =" + Convert.ToInt32(DDlState_Name.SelectedValue.ToString()) + " and ActiveStatus='Y' ";
            Utils.PopulateDropDownList(DDlCompany_Name, SQLString.ToString(), CnnString1, "--Select Company Name--");

        }
        #endregion      


        protected void btn_download_Click(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = "";

                if (DDlState_Name.SelectedItem.Text.ToString().Equals("--Select State Name--"))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select State Name.";
                    return;
                }



                if (DDlCompany_Name.SelectedItem.Text.ToString().Equals("--Select Company Name--"))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select Company Name.";
                    return;
                }


                if ( string.IsNullOrEmpty(txtdistinct.Text.ToString()))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please  Enter District Name";
                    return;
                }
                
                #region Fetch Data
                SqlConnection con = new SqlConnection(CnnString1);
                 SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand("EmployeeDetailReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stateid", Convert.ToInt32(DDlState_Name.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@Company_Name", DDlCompany_Name.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@district", txtdistinct.Text.ToString().Trim());
         
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();

              

                #endregion

                #region Fetch Data

                if (dt.Rows.Count > 0)
                {
                    string filename = "EmployeeDetailReport" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();

                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "Export Data";
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


                    Worksheet sheet = book.Worksheets.Add("Employee Detail Report");

                    sheet.Table.Columns.Add(new WorksheetColumn(25));
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
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));

                    WorksheetRow row = sheet.Table.Rows.Add();

                    row.Index = 2;
                   
                    WorksheetCell cell = row.Cells.Add(" Employee Details Report");
                    cell.MergeAcross = 4; // Merge two cells together
                    cell.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();

                    row.Index = 3;

                    WorksheetCell cell2 = row.Cells.Add("State : " + DDlState_Name.SelectedItem.Text.ToString());
                    cell2.MergeAcross = 4; // Merge two cells together
                    cell2.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();

                    row.Index = 4;

                    WorksheetCell cell1 = row.Cells.Add(" Report Genereated DATE :   " + System.DateTime.Now.ToString("dd/MMM/yyyy"));
                    cell1.MergeAcross = 4; // Merge two cells together
                    cell1.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add(); 
                    
                    row.Index = 6;
                    row.Cells.Add(new WorksheetCell("S.No", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Emp code", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Name.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Name In Bank", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Father Name", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("ESI Number", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("PFNumber", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Account No", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Bank Name", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Branch Name", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("IFSCCode", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("State", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("Designation", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("Location", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("Company Name", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("EC/AC", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("District", "HeaderStyle"));



                    //  row = sheet.Table.Rows.Add();
                    String StringField = String.Empty;
                    String StringAlert = String.Empty;

                    // row.Index = 12;
                    string RTOColName = string.Empty;
                    int sno = 0;
                    if (dt.Rows.Count > 0)
                    {
                       foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                        {
                            sno = sno + 1;
                            row = sheet.Table.Rows.Add();
                            row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["Emp code"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["Name"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["Name In Bank"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["Father Name"].ToString(), DataType.String, "HeaderStyle1"));
                              row.Cells.Add(new WorksheetCell(dtrows["ESI Number"].ToString(), DataType.String, "HeaderStyle1"));
                              row.Cells.Add(new WorksheetCell(dtrows["PFNumber"].ToString(), DataType.String, "HeaderStyle1"));
                              row.Cells.Add(new WorksheetCell(dtrows["Account No"].ToString(), DataType.String, "HeaderStyle1"));
                               row.Cells.Add(new WorksheetCell(dtrows["Bank Name"].ToString(), DataType.String, "HeaderStyle1"));
                              row.Cells.Add(new WorksheetCell(dtrows["Branch Name"].ToString(), DataType.String, "HeaderStyle1"));
                             row.Cells.Add(new WorksheetCell(dtrows["IFSCCode"].ToString(), DataType.String, "HeaderStyle1"));
                             row.Cells.Add(new WorksheetCell(dtrows["State"].ToString(), DataType.String, "HeaderStyle1"));
                             row.Cells.Add(new WorksheetCell(dtrows["Designation"].ToString(), DataType.String, "HeaderStyle1"));
                             row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["Company Name"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["EC/AC"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["District"].ToString(), DataType.String, "HeaderStyle1"));
                            

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
                #endregion
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Record Not Found.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}