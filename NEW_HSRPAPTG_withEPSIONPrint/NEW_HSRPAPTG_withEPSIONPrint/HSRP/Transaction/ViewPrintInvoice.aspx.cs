﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using System.Data;
using DataProvider;
using System.Text;
using System.Diagnostics;

namespace HSRP.Transaction
{
    public partial class ViewPrintInvoice : System.Web.UI.Page
    {
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        //int HSRPStateID;
        //int RTOLocationID;
        int HSRPStateID;
        string embpsigcentername = string.Empty;
        string RTOLocationName = string.Empty;
        string ExicseAmount = string.Empty;
        string SQlQuery = string.Empty;
        int RTOLocationID;
        protected void Page_Load(object sender, EventArgs e)
        {
            RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());

            HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
          
            // global::System.Windows.Forms.MessageBox.Show (Request.QueryString["Status"].ToString());
            //if (Session["UserHSRPStateID"].ToString() == "9")
            //{
            //    buttonSave.Visible = false;
            //    btnDC.Visible = true;
            //}
            //else
            //{
            //    buttonSave.Visible = true;
            //    btnDC.Visible = false;
            //}
        }

        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            printDelevryChalan("Print");
        }


        string OrderStatus5 = string.Empty;
        private void printDelevryChalan(string update)
        {


            HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());

            if (HSRPStateID.ToString() == "9")
            {
                string LinkButtonCashReceipt1 = "LinkButtonCashReceipt";

                DELIVERY_CHALLAN_AP(Request.QueryString["HSRPRecordID"].ToString(), LinkButtonCashReceipt1);
            }
            else
            {

                DataTable GetAddress;
                string Address;
                GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRPStateID + "'", CnnString);

                if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
                {
                    Address = " - " + GetAddress.Rows[0]["pincode"];
                }
                else
                {
                    Address = "";
                }


                string HSRPRecordID = string.Empty;
                HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();
                string OrderStatus = Request.QueryString["Status"].ToString();
                if (OrderStatus == "Closed" || OrderStatus == "CLOSED")
                {

                    DataTable dataSetFillHSRPDeliveryChallan = new DataTable();
                    BAL obj = new BAL();
                    if (obj.FillHSRPRecordDeliveryChallan(HSRPRecordID, ref dataSetFillHSRPDeliveryChallan))
                    {

                        string Address3;
                        string Range3;
                        string Division3;
                        string Cerc3;

                        DateTime OrderClosedDateX;
                        string OrderClosedDate;
                        string matchdate = "08/31/2013";
                        string[] dd = matchdate.Split('/');
                        if (dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString() == "1/1/1900 12:00:00 AM" || dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString() == null || dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString() == "")
                        {
                            OrderClosedDateX = DateTime.Now;
                        }
                        else
                        {
                            string serverdatetime = dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString();
                            OrderClosedDateX = DateTime.Parse(serverdatetime);
                        }


                        DateTime MatchDateX = DateTime.Parse(matchdate);
                        int result = DateTime.Compare(OrderClosedDateX, MatchDateX);
                        string[] str = OrderClosedDateX.ToString().Split('/');
                        string[] str1 = str[2].Split(' ');
                        if (int.Parse(str[0]) > int.Parse(dd[0]) && int.Parse(str1[0]) >= int.Parse(dd[2]) && HSRPStateID == 2)
                        {
                            Address3 = GetAddress.Rows[0]["Address3"].ToString();
                            Cerc3 = "AACCL1655KEM002";
                            Range3 = "Ambala-I";
                            Division3 = "Ambala";
                        }
                        else
                        {
                            Address3 = GetAddress.Rows[0]["Address1"].ToString();
                            Cerc3 = GetAddress.Rows[0]["CERC"].ToString();
                            Range3 = GetAddress.Rows[0]["Range"].ToString();
                            Division3 = GetAddress.Rows[0]["Division"].ToString();
                        }
                        string filename = "HSRP INVOICE" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

                        string SQLString = String.Empty;
                        String StringField = String.Empty;
                        String StringAlert = String.Empty;

                        StringBuilder bb = new StringBuilder();

                        //Creates an instance of the iTextSharp.text.Document-object:
                        Document document = new Document();

                        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


                        string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                        PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                        //Opens the document:
                        document.Open();

                        PdfPTable table2 = new PdfPTable(7);
                        PdfPTable table1 = new PdfPTable(7);
                        PdfPTable table = new PdfPTable(7);

                        //actual width of table in points
                        table.TotalWidth = 1000f;

                        //fix the absolute width of the table
                        PdfPCell cell1211 = new PdfPCell(new Phrase("Original", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1211.Colspan = 1;
                        cell1211.BorderWidthLeft = 1f;
                        cell1211.BorderWidthRight = 1f;
                        cell1211.BorderWidthTop = 1f;
                        cell1211.BorderWidthBottom = 1f;

                        cell1211.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table1.AddCell(cell1211);
                        PdfPCell cell12111 = new PdfPCell(new Phrase("INVOICE", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12111.Colspan = 6;
                        cell12111.BorderWidthLeft = 0f;
                        cell12111.BorderWidthRight = 0f;
                        cell12111.BorderWidthTop = 0f;
                        cell12111.BorderWidthBottom = 0f;

                        cell12111.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table1.AddCell(cell12111);

                        PdfPCell cell13699 = new PdfPCell(new Phrase("----------------------------------------------------------------------------------------------------------------------------", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell13699.Colspan = 7;

                        cell13699.BorderWidthLeft = 0f;
                        cell13699.BorderWidthRight = 0f;
                        cell13699.BorderWidthBottom = 0f;
                        cell13699.BorderColor = BaseColor.WHITE;
                        cell13699.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table2.AddCell(cell13699);

                        PdfPCell cell12112 = new PdfPCell(new Phrase("Assessee", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12112.Colspan = 1;
                        cell12112.BorderWidthLeft = 1f;
                        cell12112.BorderWidthRight = 1f;
                        cell12112.BorderWidthTop = 1f;
                        cell12112.BorderWidthBottom = 1f;

                        cell12112.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table2.AddCell(cell12112);
                        PdfPCell cell12113 = new PdfPCell(new Phrase("INVOICE", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12113.Colspan = 6;
                        cell12113.BorderWidthLeft = 0f;
                        cell12113.BorderWidthRight = 0f;
                        cell12113.BorderWidthTop = 0f;
                        cell12113.BorderWidthBottom = 0f;

                        cell12113.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table2.AddCell(cell12113);


                        PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12.Colspan = 5;
                        cell12.BorderWidthLeft = 1f;
                        cell12.BorderWidthRight = .8f;
                        cell12.BorderWidthTop = .8f;
                        cell12.BorderWidthBottom = 0f;

                        cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12);

                        PdfPCell cell1207 = new PdfPCell(new Phrase("C. E. R/C", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1207.Colspan = 1;
                        cell1207.BorderWidthLeft = 0f;
                        cell1207.BorderWidthRight = 0f;
                        cell1207.BorderWidthTop = .8f;
                        cell1207.BorderWidthBottom = 0f;

                        cell1207.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1207);


                        PdfPCell cell1208 = new PdfPCell(new Phrase(": " + Cerc3, new iTextSharp.text.Font(bfTimes, 5f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1208.Colspan = 1;
                        cell1208.BorderWidthLeft = 0f;
                        cell1208.BorderWidthRight = 1f;
                        cell1208.BorderWidthTop = .8f;
                        cell1208.BorderWidthBottom = 0f;

                        cell1208.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1208);



                        PdfPCell cell1203 = new PdfPCell(new Phrase("" + Address3, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1203.Colspan = 5;
                        cell1203.BorderWidthLeft = 1f;
                        cell1203.BorderWidthRight = .8f;
                        cell1203.BorderWidthTop = 0f;
                        cell1203.BorderWidthBottom = 0f;
                        cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1203);

                        PdfPCell cell2 = new PdfPCell(new Phrase("TIN NO.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell2.Colspan = 1;

                        cell2.BorderWidthLeft = 0f;
                        cell2.BorderWidthRight = 0f;
                        cell2.BorderWidthTop = .8f;
                        cell2.BorderWidthBottom = 0f;
                        cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell2);

                        // string getTinNo1 = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", CnnString);

                        PdfPCell cell21111 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["TinNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell21111.Colspan = 1;
                        cell21111.BorderWidthLeft = 0f;
                        cell21111.BorderWidthRight = 1f;
                        cell21111.BorderWidthTop = .8f;
                        cell21111.BorderWidthBottom = 0f;
                        cell21111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell21111);


                        PdfPCell cell1204 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1204.Colspan = 5;
                        cell1204.BorderWidthLeft = 1f;
                        cell1204.BorderWidthRight = .8f;
                        cell1204.BorderWidthTop = 0f;
                        cell1204.BorderWidthBottom = 0f;
                        cell1204.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1204);


                        PdfPCell cell1209 = new PdfPCell(new Phrase("COMMODITY", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1209.Colspan = 1;
                        cell1209.BorderWidthLeft = 0f;
                        cell1209.BorderWidthRight = 0f;
                        cell1209.BorderWidthTop = .8f;
                        cell1209.BorderWidthBottom = 0f;

                        cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1209);


                        PdfPCell cell12115 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["Commodity"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12115.Colspan = 1;
                        cell12115.BorderWidthLeft = 0f;
                        cell12115.BorderWidthRight = 1f;
                        cell12115.BorderWidthTop = .8f;
                        cell12115.BorderWidthBottom = 0f;

                        cell12115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12115);


                        PdfPCell cell1205 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1205.Colspan = 5;
                        cell1205.BorderWidthLeft = 1f;
                        cell1205.BorderWidthRight = .8f;
                        cell1205.BorderWidthTop = 0f;
                        cell1205.BorderWidthBottom = 0f;
                        cell1205.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1205);


                        PdfPCell cell1213 = new PdfPCell(new Phrase("C. H.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1213.Colspan = 1;
                        cell1213.BorderWidthLeft = 0f;
                        cell1213.BorderWidthRight = 0f;
                        cell1213.BorderWidthTop = .8f;
                        cell1213.BorderWidthBottom = 0f;

                        cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1213);


                        PdfPCell cell1214 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["CH"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1214.Colspan = 1;
                        cell1214.BorderWidthLeft = 0f;
                        cell1214.BorderWidthRight = 1f;
                        cell1214.BorderWidthTop = .8f;
                        cell1214.BorderWidthBottom = 0f;

                        cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1214);

                        PdfPCell cell1206 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1206.Colspan = 5;
                        cell1206.BorderWidthLeft = 1f;
                        cell1206.BorderWidthRight = .8f;
                        cell1206.BorderWidthTop = 0f;
                        cell1206.BorderWidthBottom = 0f;
                        cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1206);


                        PdfPCell cell1215 = new PdfPCell(new Phrase("RANGE", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1215.Colspan = 1;
                        cell1215.BorderWidthLeft = 0f;
                        cell1215.BorderWidthRight = 0f;
                        cell1215.BorderWidthTop = .8f;
                        cell1215.BorderWidthBottom = 0f;

                        cell1215.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1215);


                        PdfPCell cell1216 = new PdfPCell(new Phrase(": " + Range3, new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1216.Colspan = 1;
                        cell1216.BorderWidthLeft = 0f;
                        cell1216.BorderWidthRight = 1f;
                        cell1216.BorderWidthTop = .8f;
                        cell1216.BorderWidthBottom = 0f;

                        cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1216);



                        PdfPCell cell1217 = new PdfPCell(new Phrase("Book No", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1217.Colspan = 1;
                        cell1217.BorderWidthLeft = 1f;
                        cell1217.BorderWidthRight = .8f;
                        cell1217.BorderWidthTop = .8f;
                        cell1217.BorderWidthBottom = 0f;

                        cell1217.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1217);


                        PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1218.Colspan = 2;
                        cell1218.BorderWidthLeft = 0f;
                        cell1218.BorderWidthRight = .8f;
                        cell1218.BorderWidthTop = .8f;
                        cell1218.BorderWidthBottom = 0f;

                        cell1218.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1218);

                        PdfPCell cell1219 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1219.Colspan = 1;
                        cell1219.BorderWidthLeft = 0f;
                        cell1219.BorderWidthRight = .8f;
                        cell1219.BorderWidthTop = .8f;
                        cell1219.BorderWidthBottom = 0f;

                        cell1219.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1219);

                        PdfPCell cell1220 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1220.Colspan = 1;
                        cell1220.BorderWidthLeft = 0f;
                        cell1220.BorderWidthRight = .8f;
                        cell1220.BorderWidthTop = .8f;
                        cell1220.BorderWidthBottom = 0f;

                        cell1220.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1220);

                        PdfPCell cell1221 = new PdfPCell(new Phrase("DIVISION", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1221.Colspan = 1;
                        cell1221.BorderWidthLeft = 0f;
                        cell1221.BorderWidthRight = 0f;
                        cell1221.BorderWidthTop = .8f;
                        cell1221.BorderWidthBottom = 0f;

                        cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1221);

                        PdfPCell cell1222 = new PdfPCell(new Phrase(": " + Division3, new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1222.Colspan = 1;
                        cell1222.BorderWidthLeft = 0f;
                        cell1222.BorderWidthRight = 1f;
                        cell1222.BorderWidthTop = .8f;
                        cell1222.BorderWidthBottom = 0f;

                        cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1222);







                        PdfPCell cell1223 = new PdfPCell(new Phrase("Invoice No", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1223.Colspan = 1;
                        cell1223.BorderWidthLeft = 1f;
                        cell1223.BorderWidthRight = .8f;
                        cell1223.BorderWidthTop = 0f;
                        cell1223.BorderWidthBottom = 0f;

                        cell1223.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1223);


                        PdfPCell cell1224 = new PdfPCell(new Phrase(dataSetFillHSRPDeliveryChallan.Rows[0]["InvoiceNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1224.Colspan = 2;
                        cell1224.BorderWidthLeft = 0f;
                        cell1224.BorderWidthRight = .8f;
                        cell1224.BorderWidthTop = 0f;
                        cell1224.BorderWidthBottom = 0f;

                        cell1224.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1224);

                        PdfPCell cell1225 = new PdfPCell(new Phrase("Date", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1225.Colspan = 1;
                        cell1225.BorderWidthLeft = 0f;
                        cell1225.BorderWidthRight = .8f;
                        cell1225.BorderWidthTop = 0f;
                        cell1225.BorderWidthBottom = 0f;

                        cell1225.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1225);

                        if (dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString() == "1/1/1900 12:00:00 AM" || dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString() == null || dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString() == "")
                        {
                            OrderClosedDate = DateTime.Now.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            OrderClosedDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"]).ToString("dd/MM/yyyy");
                        }

                        PdfPCell cell1226 = new PdfPCell(new Phrase("" + OrderClosedDate.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1226.Colspan = 1;
                        cell1226.BorderWidthLeft = 0f;
                        cell1226.BorderWidthRight = .8f;
                        cell1226.BorderWidthTop = 0f;
                        cell1226.BorderWidthBottom = 0f;

                        cell1226.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1226);

                        PdfPCell cell1227 = new PdfPCell(new Phrase("Commissionerate", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1227.Colspan = 1;
                        cell1227.BorderWidthLeft = 0f;
                        cell1227.BorderWidthRight = 0f;
                        cell1227.BorderWidthTop = .8f;
                        cell1227.BorderWidthBottom = 0f;

                        cell1227.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1227);

                        PdfPCell cell1228 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["Commissionerate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1228.Colspan = 1;
                        cell1228.BorderWidthLeft = 0f;
                        cell1228.BorderWidthRight = 1f;
                        cell1228.BorderWidthTop = .8f;
                        cell1228.BorderWidthBottom = 0f;

                        cell1228.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1228);




                        PdfPCell cell1229 = new PdfPCell(new Phrase("P.O. No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1229.Colspan = 1;
                        cell1229.BorderWidthLeft = 1f;
                        cell1229.BorderWidthRight = .8f;
                        cell1229.BorderWidthTop = .8f;
                        cell1229.BorderWidthBottom = 0f;

                        cell1229.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1229);


                        PdfPCell cell1230 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1230.Colspan = 2;
                        cell1230.BorderWidthLeft = 0f;
                        cell1230.BorderWidthRight = .8f;
                        cell1230.BorderWidthTop = .8f;
                        cell1230.BorderWidthBottom = 0f;

                        cell1230.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1230);

                        PdfPCell cell1231 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1231.Colspan = 1;
                        cell1231.BorderWidthLeft = 0f;
                        cell1231.BorderWidthRight = .8f;
                        cell1231.BorderWidthTop = .8f;
                        cell1231.BorderWidthBottom = 0f;

                        cell1231.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1231);

                        PdfPCell cell1232 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1232.Colspan = 1;
                        cell1232.BorderWidthLeft = 0f;
                        cell1232.BorderWidthRight = .8f;
                        cell1232.BorderWidthTop = .8f;
                        cell1232.BorderWidthBottom = 0f;

                        cell1232.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1232);

                        PdfPCell cell1233 = new PdfPCell(new Phrase("Party C. E. R/C NO", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1233.Colspan = 1;
                        cell1233.BorderWidthLeft = 0f;
                        cell1233.BorderWidthRight = 0f;
                        cell1233.BorderWidthTop = .8f;
                        cell1233.BorderWidthBottom = 0f;

                        cell1233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1233);

                        PdfPCell cell1234 = new PdfPCell(new Phrase(": NA", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1234.Colspan = 1;
                        cell1234.BorderWidthLeft = 0f;
                        cell1234.BorderWidthRight = 1f;
                        cell1234.BorderWidthTop = .8f;
                        cell1234.BorderWidthBottom = 0f;

                        cell1234.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1234);





                        PdfPCell cell1235 = new PdfPCell(new Phrase("RR/GR NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1235.Colspan = 1;
                        cell1235.BorderWidthLeft = 1f;
                        cell1235.BorderWidthRight = .8f;
                        cell1235.BorderWidthTop = .8f;
                        cell1235.BorderWidthBottom = 0f;

                        cell1235.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1235);


                        PdfPCell cell1236 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1236.Colspan = 2;
                        cell1236.BorderWidthLeft = 0f;
                        cell1236.BorderWidthRight = .8f;
                        cell1236.BorderWidthTop = .8f;
                        cell1236.BorderWidthBottom = 0f;

                        cell1236.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1236);

                        PdfPCell cell1237 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1237.Colspan = 1;
                        cell1237.BorderWidthLeft = 0f;
                        cell1237.BorderWidthRight = .8f;
                        cell1237.BorderWidthTop = .8f;
                        cell1237.BorderWidthBottom = 0f;

                        cell1237.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1237);

                        PdfPCell cell1238 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1238.Colspan = 1;
                        cell1238.BorderWidthLeft = 0f;
                        cell1238.BorderWidthRight = .8f;
                        cell1238.BorderWidthTop = .8f;
                        cell1238.BorderWidthBottom = 0f;

                        cell1238.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1238);

                        PdfPCell cell1239 = new PdfPCell(new Phrase("Party CST/TIN No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1239.Colspan = 1;
                        cell1239.BorderWidthLeft = 0f;
                        cell1239.BorderWidthRight = 0f;
                        cell1239.BorderWidthTop = .8f;
                        cell1239.BorderWidthBottom = 0f;

                        cell1239.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1239);

                        PdfPCell cell1240 = new PdfPCell(new Phrase(": NA", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1240.Colspan = 1;
                        cell1240.BorderWidthLeft = 0f;
                        cell1240.BorderWidthRight = 1f;
                        cell1240.BorderWidthTop = .8f;
                        cell1240.BorderWidthBottom = 0f;

                        cell1240.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1240);




                        PdfPCell cell1241 = new PdfPCell(new Phrase("Consignee", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1241.Colspan = 1;
                        cell1241.BorderWidthLeft = 1f;
                        cell1241.BorderWidthRight = .8f;
                        cell1241.BorderWidthTop = .8f;
                        cell1241.BorderWidthBottom = 0f;

                        cell1241.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1241);

                        PdfPCell cell1242 = new PdfPCell(new Phrase(dataSetFillHSRPDeliveryChallan.Rows[0]["Address1"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1242.Colspan = 5;
                        cell1242.BorderWidthLeft = 0f;
                        cell1242.BorderWidthRight = .8f;
                        cell1242.BorderWidthTop = .8f;
                        cell1242.BorderWidthBottom = 0f;

                        cell1242.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1242);

                        PdfPCell cell1243 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1243.Colspan = 1;
                        cell1243.BorderWidthLeft = 0f;
                        cell1243.BorderWidthRight = 1f;
                        cell1243.BorderWidthTop = .8f;
                        cell1243.BorderWidthBottom = 0f;

                        cell1243.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1243);




                        PdfPCell cell1248 = new PdfPCell(new Phrase("S.N0.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1248.Colspan = 1;
                        cell1248.BorderWidthLeft = 1f;
                        cell1248.BorderWidthRight = .8f;
                        cell1248.BorderWidthTop = .8f;
                        cell1248.BorderWidthBottom = 0f;

                        cell1248.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1248);


                        PdfPCell cell1249 = new PdfPCell(new Phrase("Description & Specifications of Goods", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1249.Colspan = 2;
                        cell1249.BorderWidthLeft = 0f;
                        cell1249.BorderWidthRight = .8f;
                        cell1249.BorderWidthTop = .8f;
                        cell1249.BorderWidthBottom = 0f;

                        cell1249.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1249);

                        PdfPCell cell1250 = new PdfPCell(new Phrase("No. Of Pkg", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1250.Colspan = 1;
                        cell1250.BorderWidthLeft = 0f;
                        cell1250.BorderWidthRight = .8f;
                        cell1250.BorderWidthTop = .8f;
                        cell1250.BorderWidthBottom = 0f;

                        cell1250.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1250);

                        PdfPCell cell1251 = new PdfPCell(new Phrase("Quantity", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1251.Colspan = 1;
                        cell1251.BorderWidthLeft = 0f;
                        cell1251.BorderWidthRight = .8f;
                        cell1251.BorderWidthTop = .8f;
                        cell1251.BorderWidthBottom = 0f;

                        cell1251.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1251);

                        PdfPCell cell1252 = new PdfPCell(new Phrase("Price Per Unit", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1252.Colspan = 1;
                        cell1252.BorderWidthLeft = 0f;
                        cell1252.BorderWidthRight = .8f;
                        cell1252.BorderWidthTop = .8f;
                        cell1252.BorderWidthBottom = 0f;

                        cell1252.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1252);

                        PdfPCell cell1253 = new PdfPCell(new Phrase("Amount Rs.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1253.Colspan = 1;
                        cell1253.BorderWidthLeft = 0f;
                        cell1253.BorderWidthRight = 1f;
                        cell1253.BorderWidthTop = .8f;
                        cell1253.BorderWidthBottom = 0f;

                        cell1253.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1253);





                        PdfPCell cell1255 = new PdfPCell(new Phrase("HSRP NUMBER PLATE SET", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1255.Colspan = 3;
                        cell1255.BorderWidthLeft = 1f;
                        cell1255.BorderWidthRight = .8f;
                        cell1255.BorderWidthTop = .8f;
                        cell1255.BorderWidthBottom = 0f;

                        cell1255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1255);

                        PdfPCell cell1256 = new PdfPCell(new Phrase("1", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1256.Colspan = 1;
                        cell1256.BorderWidthLeft = 0f;
                        cell1256.BorderWidthRight = .8f;
                        cell1256.BorderWidthTop = .8f;
                        cell1256.BorderWidthBottom = 0f;

                        cell1256.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1256);

                        int FQt = 0;
                        if (dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() != "NULL" || dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() != " ")
                        {

                            if (dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() == "Y")
                            {
                                FQt = 1;
                            }
                            else
                            {
                                FQt = 0;
                            }
                        }

                        PdfPCell cell1257 = new PdfPCell(new Phrase(FQt.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1257.Colspan = 1;
                        cell1257.BorderWidthLeft = 0f;
                        cell1257.BorderWidthRight = .8f;
                        cell1257.BorderWidthTop = .8f;
                        cell1257.BorderWidthBottom = 0f;

                        cell1257.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1257);

                        decimal amount4 = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["TotalAmount"]);
                        decimal amount5 = Math.Round(amount4, 2);
                        PdfPCell cell1258 = new PdfPCell(new Phrase(amount5.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1258.Colspan = 1;
                        cell1258.BorderWidthLeft = 0f;
                        cell1258.BorderWidthRight = .8f;
                        cell1258.BorderWidthTop = .8f;
                        cell1258.BorderWidthBottom = 0f;

                        cell1258.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1258);



                        PdfPCell cell1259 = new PdfPCell(new Phrase(amount5.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1259.Colspan = 1;
                        cell1259.BorderWidthLeft = 0f;
                        cell1259.BorderWidthRight = 1f;
                        cell1259.BorderWidthTop = .8f;
                        cell1259.BorderWidthBottom = 0f;

                        cell1259.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1259);



                        PdfPCell cell1308 = new PdfPCell(new Phrase("Vehicle Reg No./Auth No :" + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString() + "/" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1308.Colspan = 4;
                        cell1308.BorderWidthLeft = 1f;
                        cell1308.BorderWidthRight = .8f;
                        cell1308.BorderWidthTop = .8f;
                        cell1308.BorderWidthBottom = 0f;

                        cell1308.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1308);





                        PdfPCell cell1310 = new PdfPCell(new Phrase("Sub Total", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1310.Colspan = 1;
                        cell1310.BorderWidthLeft = 0f;
                        cell1310.BorderWidthRight = .8f;
                        cell1310.BorderWidthTop = .8f;
                        cell1310.BorderWidthBottom = 0f;

                        cell1310.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1310);

                        PdfPCell cell1311 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1311.Colspan = 1;
                        cell1311.BorderWidthLeft = 0f;
                        cell1311.BorderWidthRight = .8f;
                        cell1311.BorderWidthTop = .8f;
                        cell1311.BorderWidthBottom = 0f;

                        cell1311.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1311);

                        decimal amount = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["TotalAmount"]);
                        decimal amount1 = Math.Round(amount, 2);


                        PdfPCell cell1312 = new PdfPCell(new Phrase(amount1.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1312.Colspan = 1;
                        cell1312.BorderWidthLeft = 0f;
                        cell1312.BorderWidthRight = 1f;
                        cell1312.BorderWidthTop = .8f;
                        cell1312.BorderWidthBottom = 0f;

                        cell1312.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1312);





                        PdfPCell cell1313 = new PdfPCell(new Phrase("Vehicle Type : " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleType"].ToString() + " Vehicle Class : " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1313.Colspan = 4;
                        cell1313.BorderWidthLeft = 1f;
                        cell1313.BorderWidthRight = .8f;
                        cell1313.BorderWidthTop = .8f;
                        cell1313.BorderWidthBottom = 0f;

                        cell1313.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1313);




                        PdfPCell cell1315 = new PdfPCell(new Phrase("TAX ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1315.Colspan = 1;
                        cell1315.BorderWidthLeft = 0f;
                        cell1315.BorderWidthRight = .8f;
                        cell1315.BorderWidthTop = .8f;
                        cell1315.BorderWidthBottom = 0f;

                        cell1315.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1315);

                        PdfPCell cell1316 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1316.Colspan = 1;
                        cell1316.BorderWidthLeft = 0f;
                        cell1316.BorderWidthRight = .8f;
                        cell1316.BorderWidthTop = .8f;
                        cell1316.BorderWidthBottom = 0f;

                        cell1316.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1316);

                        PdfPCell cell1317 = new PdfPCell(new Phrase(dataSetFillHSRPDeliveryChallan.Rows[0]["VAT_Amount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1317.Colspan = 1;
                        cell1317.BorderWidthLeft = 0f;
                        cell1317.BorderWidthRight = 1f;
                        cell1317.BorderWidthTop = .8f;
                        cell1317.BorderWidthBottom = 0f;

                        cell1317.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1317);

                        PdfPCell cell1318 = new PdfPCell(new Phrase("Front Laser Code : " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRP_Front_LaserCode"].ToString() + "  Rear Laser Code : " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRP_Rear_LaserCode"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1318.Colspan = 4;
                        cell1318.BorderWidthLeft = 1f;
                        cell1318.BorderWidthRight = .8f;
                        cell1318.BorderWidthTop = .8f;
                        cell1318.BorderWidthBottom = 0f;

                        cell1318.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1318);





                        PdfPCell cell1320 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1320.Colspan = 1;
                        cell1320.BorderWidthLeft = 0f;
                        cell1320.BorderWidthRight = .8f;
                        cell1320.BorderWidthTop = .8f;
                        cell1320.BorderWidthBottom = 0f;

                        cell1320.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1320);

                        PdfPCell cell1321 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1321.Colspan = 1;
                        cell1321.BorderWidthLeft = 0f;
                        cell1321.BorderWidthRight = .8f;
                        cell1321.BorderWidthTop = .8f;
                        cell1321.BorderWidthBottom = 0f;

                        cell1321.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1321);

                        PdfPCell cell1322 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1322.Colspan = 1;
                        cell1322.BorderWidthLeft = 0f;
                        cell1322.BorderWidthRight = 1f;
                        cell1322.BorderWidthTop = .8f;
                        cell1322.BorderWidthBottom = 0f;

                        cell1322.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1322);




                        PdfPCell cell1328 = new PdfPCell(new Phrase("Chassis No : " + dataSetFillHSRPDeliveryChallan.Rows[0]["ChassisNo"].ToString() + "  Engine No : " + dataSetFillHSRPDeliveryChallan.Rows[0]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1328.Colspan = 4;
                        cell1328.BorderWidthLeft = 1f;
                        cell1328.BorderWidthRight = .8f;
                        cell1328.BorderWidthTop = .8f;
                        cell1328.BorderWidthBottom = 0f;

                        cell1328.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1328);





                        PdfPCell cell1330 = new PdfPCell(new Phrase("Sub total", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1330.Colspan = 1;
                        cell1330.BorderWidthLeft = 0f;
                        cell1330.BorderWidthRight = .8f;
                        cell1330.BorderWidthTop = .8f;
                        cell1330.BorderWidthBottom = 0f;

                        cell1330.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1330);



                        PdfPCell cell1331 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1331.Colspan = 1;
                        cell1331.BorderWidthLeft = 0f;
                        cell1331.BorderWidthRight = .8f;
                        cell1331.BorderWidthTop = .8f;
                        cell1331.BorderWidthBottom = 0f;

                        cell1331.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1331);

                        decimal vatPer = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["VAT_Percentage"]);
                        decimal vat = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["VAT_Amount"]);
                        decimal TotAmt = amount + vat;
                        decimal totalAmount = System.Decimal.Round(TotAmt, 2);
                        PdfPCell cell1332 = new PdfPCell(new Phrase(totalAmount.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1332.Colspan = 1;
                        cell1332.BorderWidthLeft = 0f;
                        cell1332.BorderWidthRight = 1f;
                        cell1332.BorderWidthTop = .8f;
                        cell1332.BorderWidthBottom = 0f;

                        cell1332.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1332);





                        PdfPCell cell1338 = new PdfPCell(new Phrase("Terms", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1338.Colspan = 1;
                        cell1338.BorderWidthLeft = 1f;
                        cell1338.BorderWidthRight = .8f;
                        cell1338.BorderWidthTop = .8f;
                        cell1338.BorderWidthBottom = 0f;

                        cell1338.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1338);

                        PdfPCell cell1339 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1339.Colspan = 1;
                        cell1339.BorderWidthLeft = 0f;
                        cell1339.BorderWidthRight = .8f;
                        cell1339.BorderWidthTop = .8f;
                        cell1339.BorderWidthBottom = 0f;

                        cell1339.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1339);

                        PdfPCell cell1340 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1340.Colspan = 1;
                        cell1340.BorderWidthLeft = 0f;
                        cell1340.BorderWidthRight = .8f;
                        cell1340.BorderWidthTop = .8f;
                        cell1340.BorderWidthBottom = 0f;

                        cell1340.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1340);

                        PdfPCell cell1341 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1341.Colspan = 1;
                        cell1341.BorderWidthLeft = 0f;
                        cell1341.BorderWidthRight = .8f;
                        cell1341.BorderWidthTop = .8f;
                        cell1341.BorderWidthBottom = 0f;

                        cell1341.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1341);

                        PdfPCell cell1342 = new PdfPCell(new Phrase("Invoice Value Round of ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1342.Colspan = 2;
                        cell1342.BorderWidthLeft = 0f;
                        cell1342.BorderWidthRight = .8f;
                        cell1342.BorderWidthTop = .8f;
                        cell1342.BorderWidthBottom = 0f;

                        cell1342.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1342);


                        decimal totalAmount1 = System.Decimal.Round(TotAmt, 0);
                        PdfPCell cell1344 = new PdfPCell(new Phrase(totalAmount1.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1344.Colspan = 1;
                        cell1344.BorderWidthLeft = 0f;
                        cell1344.BorderWidthRight = 1f;
                        cell1344.BorderWidthTop = .8f;
                        cell1344.BorderWidthBottom = 0f;

                        cell1344.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1344);




                        PdfPCell cell1345 = new PdfPCell(new Phrase("1:It is certified that the particulars given above are true and correct and amount indicated represents that prices actually charged and that there is no aditional inflow of any consideration directly or indirectly from the buyer", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1345.Colspan = 7;
                        cell1345.BorderWidthLeft = 1f;
                        cell1345.BorderWidthRight = .8f;
                        cell1345.BorderWidthTop = .8f;
                        cell1345.BorderWidthBottom = 0f;

                        cell1345.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1345);





                        PdfPCell cell1348 = new PdfPCell(new Phrase("2: All disputes arising out of it will be subject to Delhi jurisdiction only", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1348.Colspan = 7;
                        cell1348.BorderWidthLeft = 1f;
                        cell1348.BorderWidthRight = .8f;
                        cell1348.BorderWidthTop = .8f;
                        cell1348.BorderWidthBottom = 0f;

                        cell1348.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1348);




                        PdfPCell cell1353 = new PdfPCell(new Phrase("3: Interest @24% will be charged on all account remaining unpaid after 30 days/due as agreed", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1353.Colspan = 4;
                        cell1353.BorderWidthLeft = 1f;
                        cell1353.BorderWidthRight = .8f;
                        cell1353.BorderWidthTop = .8f;
                        cell1353.BorderWidthBottom = 0f;

                        cell1353.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1353);



                        PdfPCell cell1358 = new PdfPCell(new Phrase(GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1358.Colspan = 3;
                        cell1358.BorderWidthLeft = 0f;
                        cell1358.BorderWidthRight = 1f;
                        cell1358.BorderWidthTop = .8f;
                        cell1358.BorderWidthBottom = 0f;

                        cell1358.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1358);





                        PdfPCell cell1356 = new PdfPCell(new Phrase("4:Goods once sold & accepted will not be taken back", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1356.Colspan = 4;
                        cell1356.BorderWidthLeft = 1f;
                        cell1356.BorderWidthRight = .8f;
                        cell1356.BorderWidthTop = .8f;
                        cell1356.BorderWidthBottom = 0f;

                        cell1356.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1356);


                        PdfPCell cell1361 = new PdfPCell(new Phrase("(AUTH. SIGN.)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        //cell1361.PaddingRight = 100f;
                        cell1361.Colspan = 3;
                        cell1361.PaddingRight = 0f;
                        cell1361.BorderWidthLeft = 0f;
                        cell1361.BorderWidthRight = 1f;
                        cell1361.BorderWidthTop = 0f;
                        cell1361.BorderWidthBottom = 0f;

                        cell1361.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1361);



                        PdfPCell cell1359 = new PdfPCell(new Phrase("5:Any discrepancy in quality & quantity should be reported within 24 hrs , of receipt of goods", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1359.Colspan = 7;
                        cell1359.BorderWidthLeft = 1f;
                        cell1359.BorderWidthRight = .8f;
                        cell1359.BorderWidthTop = .8f;
                        cell1359.BorderWidthBottom = 0f;

                        cell1359.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1359);





                        PdfPCell cell1362 = new PdfPCell(new Phrase("We hereby certify that goods recived are as per order & requirement and we abide to above said terms", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1362.Colspan = 7;
                        cell1362.BorderWidthLeft = 1f;
                        cell1362.BorderWidthRight = .8f;
                        cell1362.BorderWidthTop = .8f;
                        cell1362.BorderWidthBottom = 1f;

                        cell1362.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1362);





                        PdfPCell cell1366 = new PdfPCell(new Phrase("CUSTOMER'S NAME : " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1366.Colspan = 7;
                        cell1366.PaddingRight = 60f;
                        cell1366.BorderWidthLeft = 0f;
                        cell1366.BorderWidthRight = .8f;
                        cell1366.BorderWidthTop = 0f;
                        cell1366.BorderWidthBottom = 1f;
                        cell1366.BorderColor = BaseColor.WHITE;
                        cell1366.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1366);

                        PdfPCell cell1367 = new PdfPCell(new Phrase("(SIGN)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1367.Colspan = 7;
                        cell1367.PaddingRight = 100f;
                        cell1367.PaddingTop = 5f;
                        cell1367.BorderWidthLeft = 0f;
                        cell1367.BorderWidthRight = .8f;
                        cell1367.BorderWidthTop = .8f;
                        cell1367.BorderWidthBottom = 1f;
                        cell1367.BorderColor = BaseColor.WHITE;
                        cell1367.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1367);




                        PdfPCell cell1369 = new PdfPCell(new Phrase("VEHICLE No : " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString() + " Date :", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1369.Colspan = 7;
                        cell1369.PaddingRight = 70f;
                        cell1369.BorderWidthLeft = 0f;
                        cell1369.BorderWidthRight = 1f;
                        cell1369.BorderWidthBottom = 1f;
                        cell1369.BorderColor = BaseColor.WHITE;
                        cell1369.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1369);



                        document.Add(table1);
                        document.Add(table);
                        document.Add(table2);
                        document.Add(table);
                        // document.Add(table1);

                        document.Close();
                        HttpContext context = HttpContext.Current;

                        context.Response.ContentType = "Application/pdf";
                        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        context.Response.WriteFile(PdfFolder);
                        context.Response.End();

                        //ReadFile(PdfFolder);

                    }
                }
                else
                {
                    string script = "<script type=\"text/javascript\">  alert('Embossing is not done');</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);

                }
                //LinkButton lnkInvc = (LinkButton)Grid1.FindControl("LinkButtonStatus");

            }
        }

      

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            //printDelevryChalan("PrintClose");
            customerInvoice();
           
        }

        public void customerInvoice()
        {
            try
            {
                string currentdate = DateTime.Now.ToString("dd/MM/yyyy");

                HttpContext context = HttpContext.Current;
                string filename = "HSRP-CI" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

                string SQLString = String.Empty;
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                StringBuilder bb = new StringBuilder();

                //Creates an instance of the iTextSharp.text.Document-object:
                Document document = new Document();

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                Int64 totalamount = 0;


                //Opens the document:
                object Exicse;
                object Vatamt;
                object TotalAmount;
                object TotalAMT;
                object Qty;
                object SecondaryCess;
                object Educess;
                object TotalWeight;
                string vehicle = string.Empty;
                string strHsrpRecordId = string.Empty;
                string strInvoiceNo = string.Empty;
                string strEmbStationName = string.Empty;
                string RTOLocationAddress = string.Empty;
                string strEmbAddress = string.Empty;
                string strEmbAddress1 = string.Empty;
                string strEmbCity = string.Empty;
                string strEmbId = string.Empty;

                #region Set ChallanNo

                try
                {

                    string strGetInvoiceNo = string.Empty;
                    string strSelectEmbStation = "SELECT DISTINCT [NAVEMBID],[EmbCenterName],RTOLocationName, RTOLocationAddress,city,Address1 FROM [vw_RTOLocationWiseEmbosingCenters] WHERE RTOLocationId='" + RTOLocationID + "'";
                    DataTable dtEmbData = Utils.GetDataTable(strSelectEmbStation, CnnString);
                    if (dtEmbData.Rows.Count <= 0)
                    {
                        // lblErrMsg.Text = "Embossing Station not found";
                        return;
                    }

                    strEmbStationName = dtEmbData.Rows[0]["EmbCenterName"].ToString();
                    RTOLocationAddress = dtEmbData.Rows[0]["RTOLocationAddress"].ToString();
                    strEmbCity = dtEmbData.Rows[0]["city"].ToString();
                    strEmbId = dtEmbData.Rows[0]["NAVEMBID"].ToString();

                    RTOLocationName = dtEmbData.Rows[0]["RTOLocationName"].ToString();
                }
                catch
                {
                    //lblErrMsg.Text = "Embossing Station not found";
                    return;
                }



                #endregion

                string HSRPRecordID = string.Empty;
                HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();

                DataTable GetAddress;
                string Address;
                GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRPStateID + "'", CnnString);


                if ((GetAddress.Rows[0]["pincode"].ToString() != "") || (GetAddress.Rows[0]["pincode"] != null))
                {
                    Address = GetAddress.Rows[0]["Address1"].ToString() + " - " + GetAddress.Rows[0]["pincode"];
                }
                else
                {
                    Address = GetAddress.Rows[0]["Address1"].ToString();
                }

                string strSelectAddress = "SELECT RTOLocationAddress FROM [RTOLocation] WHERE RTOLocationId='" + RTOLocationID + "'";
                DataTable dtAddress = Utils.GetDataTable(strSelectAddress, CnnString);

                string strRTOAddress = dtAddress.Rows[0]["RTOLocationAddress"].ToString();

                PdfPTable table2 = new PdfPTable(10);
                PdfPTable table1 = new PdfPTable(10);
                PdfPTable table = new PdfPTable(10);
                PdfPTable table3 = new PdfPTable(2);

                //actual width of table in points
                table.TotalWidth = 1000f;
                string OldRegPlate = string.Empty;

                DataTable dt = new DataTable();

                //calcilation query

                //string  SQlQuery = "select a.vehicletype+' (Damage Front)' as DescriptionOfGoods,count(*) as qty,hsrpweight*count(*) as HSRPWeight,round((sum(GSTBasicAmount)/count(*)),3) amt,round(sum(GSTBasicAmount),2)bamt,round(sum(SGSTAmount),2)Vatamt,round(sum(CGSTAmount),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess  from hsrprecords a,hsrpweightmaster b where a.VehicleType=b.vehicletype and hsrp_stateid='" + HSRPStateID + "' and a.OrderType in ('DF') and b.ordertype='DF' and HsrpRecordID in(" + HSRPRecordID + ") group by a.vehicletype,b.hsrpweight union select a.vehicletype+' (Damage Rear)',count(*) as qty,hsrpweight*count(*),round((sum(GSTBasicAmount)/count(*)),2) amt,round(sum(GSTBasicAmount),2)bamt,round(sum(SGSTAmount),2)Vatamt,round(sum(CGSTAmount),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords a,hsrpweightmaster b  where a.vehicletype=b.vehicletype and hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + HSRPRecordID + ") and  a.ordertype in ('DR') and b.ordertype='DR' group by a.VehicleType,b.hsrpweight union select a.vehicletype+' (Both Plates)',(count(*)*2)/2 qty,hsrpweight*(count(*)*2)/2,round((sum(GstBasicAmount)/count(*)),2) amt,round(sum(GSTBasicAmount),2)bamt,round(sum(SGSTAmount),2)Vatamt,round(sum(CGSTAmount),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords a,hsrpweightmaster b  where a.vehicletype=b.vehicletype and hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + HSRPRecordID + ") and a.ordertype in ('NB','OB','DB') and b.ordertype='NB' group by a.VehicleType,b.hsrpweight";
                SQlQuery = "select a.vehicletype+' (Damage Front)' as DescriptionOfGoods,count(*) as qty,hsrpweight*count(*) as HSRPWeight,round((sum(GSTBasicAmount + (-Roundoff_value))/count(*)),3) amt,round(sum(GSTBasicAmount + (-Roundoff_value)),2)bamt,round(sum(SGSTAmount),2)Vatamt,round(sum(CGSTAmount),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess  from hsrprecords a,hsrpweightmaster b where a.VehicleType=b.vehicletype and hsrp_stateid='" + HSRPStateID + "' and a.OrderType in ('DF') and b.ordertype='DF' and HsrpRecordID in(" + HSRPRecordID + ") group by a.vehicletype,b.hsrpweight union select a.vehicletype+' (Damage Rear)',count(*) as qty,hsrpweight*count(*),round((sum(GSTBasicAmount + (-Roundoff_value))/count(*)),2) amt,round(sum(GSTBasicAmount + (-Roundoff_value)),2)bamt,round(sum(SGSTAmount),2)Vatamt,round(sum(CGSTAmount),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords a,hsrpweightmaster b  where a.vehicletype=b.vehicletype and hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + HSRPRecordID + ") and  a.ordertype in ('DR') and b.ordertype='DR' group by a.VehicleType,b.hsrpweight union select a.vehicletype+' (Both Plates)',(count(*)*2)/2 qty,hsrpweight*(count(*)*2)/2,round((sum(GstBasicAmount + (-Roundoff_value))/count(*)),2) amt,round(sum(GSTBasicAmount + (-Roundoff_value)),2)bamt,round(sum(SGSTAmount),2)Vatamt,round(sum(CGSTAmount),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords a,hsrpweightmaster b  where a.vehicletype=b.vehicletype and hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + HSRPRecordID + ") and a.ordertype in ('NB','OB','DB') and b.ordertype='NB' group by a.VehicleType,b.hsrpweight";
                dt = Utils.GetDataTable(SQlQuery, CnnString);

                string SQlQuery1 = " select  a.OwnerName, a.Address1, Invoiceno from hsrprecords a where hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in('" + HSRPRecordID + "')";
                DataTable dtname_Address = Utils.GetDataTable(SQlQuery1, CnnString);

                BAL obj = new BAL();

                decimal dTotalAmount = 0;
                if (true)
                {

                    #region Upper Part of PDF

                    DataTable dataSetFillHSRPDeliveryChallan = new DataTable();
                    string strEccNo = GetAddress.Rows[0]["ExciseNo"].ToString();
                    string strDivision = GetAddress.Rows[0]["Division"].ToString();
                    string strRange = GetAddress.Rows[0]["Range"].ToString();
                    string strCommissionerate = GetAddress.Rows[0]["Commissionerate"].ToString();
                    string strTin = GetAddress.Rows[0]["GSTIN"].ToString();
                    string strTariffHead = GetAddress.Rows[0]["CH"].ToString();
                    string strCompName = GetAddress.Rows[0]["CompanyName"].ToString();

                    // GenerateCell(table, 10, 1, 1, 1, 1, 1, 0, "Tax Invoice \r\n \r\n", 0, 0);


                    GenerateCell(table, 7, 1, 0, 1, 0, 1, 0, " ", 0, 0);
                    GenerateCell(table, 3, 0, 1, 1, 0, 0, 1, "Origional For Recipient", 0, 0);

                    GenerateCellforhrading(table, 7, 1, 0, 0, 0, 1, 10, "                             Tax Invoice", 0, 0);
                    GenerateCell(table, 3, 0, 1, 0, 0, 0, 1, "Dulicate For Transporter", 0, 0);

                    GenerateCell(table, 7, 1, 0, 0, 0, 1, 0, " ", 0, 0);
                    GenerateCell(table, 3, 0, 1, 0, 0, 0, 1, "Triplicate For Supplier", 0, 0);


                    GenerateCell(table, 10, 1, 1, 1, 1, 1, 0, strCompName + "\r\n\r\n" + GetAddress.Rows[0]["Address1"].ToString() + "\r\n\r\n" + "GSTIN - " + strTin, 0, 0);

                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "Supplier :- ", 0, 0);
                    GenerateCell(table, 5, 0, 1, 0, 0, 0, 1, "Invoice No - " + dtname_Address.Rows[0]["Invoiceno"].ToString(), 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, strCompName, 0, 0);
                    GenerateCell(table, 5, 0, 1, 0, 0, 0, 1, "Date  -" + System.DateTime.Now.ToString("dd/MM/yyyy"), 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, RTOLocationAddress, 0, 0);
                    GenerateCell(table, 5, 0, 1, 0, 0, 0, 1, " ", 0, 0);


                    #endregion

                    GenerateCell(table, 5, 1, 1, 1, 0, 0, 1, "Recipient :- ", 0, 0);
                    GenerateCell(table, 5, 0, 1, 1, 0, 0, 1, "Transporter Name:-" + " ", 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, dtname_Address.Rows[0]["OwnerName"].ToString(), 0, 0);
                    GenerateCell(table, 5, 0, 1, 0, 0, 0, 1, "GR/LR No.:", 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 0, 0, 1, "Address :- " + dtname_Address.Rows[0]["Address1"].ToString(), 0, 0);
                    GenerateCell(table, 5, 0, 1, 0, 0, 0, 1, "Lorry  No.:-" + " ", 0, 0);


                    #region Column Heading Creation

                    GenerateCell(table, 1, 1, 0, 1, 0, 1, 0, "SI.NO.", 0, 0);
                    GenerateCell(table, 4, 1, 1, 1, 0, 1, 0, "Description of Goods", 0, 0);
                    GenerateCell(table, 1, 0, 0, 1, 0, 1, 0, "HSN", 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 0, 1, 0, "UQC", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 0, "Qty", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 0, "Rate", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 0, "Amount (Rs.)", 0, 0);


                    #endregion


                    #region 1st Row

                    GenerateCell(table, 1, 1, 0, 1, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 4, 1, 1, 1, 0, 1, 0, "HSRP SET FOR:", 0, 0);
                    GenerateCell(table, 1, 0, 0, 1, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 0, 0, "", 0, 0);
                    for (int i = 0; i < 2; i++)
                    {
                        GenerateCell(table, 1, 0, 1, 1, 0, 0, 0, "", 0, 0);
                    }
                    #endregion

                    #region 2nd Row


                    Exicse = dt.Compute("sum(excise)", "");
                    ExicseAmount = String.Format("{0:0.00}", Exicse);
                    TotalAmount = dt.Compute("sum(bamt)", "");
                    Vatamt = dt.Compute("sum(Vatamt)", "");
                    TotalAMT = dt.Compute("sum(amt)", "");
                    Qty = dt.Compute("sum(qty)", "");
                    TotalWeight = dt.Compute("sum(HSRPWeight)", "");
                    Educess = dt.Compute("sum(cess)", "");
                    SecondaryCess = dt.Compute("sum(shecess)", "");

                    string Newamount = String.Format("{0:0.00}", TotalAmount);
                    string VatamtNew = String.Format("{0:0.00}", Vatamt);
                    string strTotalAMT = String.Format("{0:0.00}", TotalAMT);
                    string strEducess = String.Format("{0:0.00}", Educess);
                    string strSecondaryCess = string.Format("{0:0.00}", SecondaryCess);

                    for (int iResult = 0; iResult < dt.Rows.Count; iResult++)
                    {
                        GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, Convert.ToString(iResult + 1), 0, 0);
                        GenerateCell(table, 4, 1, 1, 0, 0, 1, 0, dt.Rows[iResult]["DescriptionOfGoods"].ToString(), 0, 0);
                        GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, strTariffHead, 0, 0);
                        GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "SET", 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, dt.Rows[iResult]["qty"].ToString(), 0, 0);

                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, Convert.ToString(decimal.Round(Convert.ToDecimal(dt.Rows[iResult]["amt"].ToString()), 2, MidpointRounding.AwayFromZero)), 0, 0);

                        GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, Convert.ToString(decimal.Round(Convert.ToDecimal(dt.Rows[iResult]["bamt"].ToString()), 2, MidpointRounding.AwayFromZero)), 0, 0);

                    }
                    if (string.IsNullOrEmpty(ExicseAmount))
                    {
                        ExicseAmount = "0";
                    }

                    if (string.IsNullOrEmpty(Newamount))
                    {
                        Newamount = "0";
                    }

                    if (string.IsNullOrEmpty(strEducess))
                    {
                        strEducess = "0";
                    }

                    if (string.IsNullOrEmpty(strSecondaryCess))
                    {
                        strSecondaryCess = "0";
                    }

                    if (string.IsNullOrEmpty(VatamtNew))
                    {
                        VatamtNew = "0";
                    }


                    decimal PreTotalAmount = 0;
                    PreTotalAmount = PreTotalAmount + Convert.ToDecimal(ExicseAmount) + Convert.ToDecimal(Newamount) + Convert.ToDecimal(strEducess) + Convert.ToDecimal(strSecondaryCess);
                    dTotalAmount = PreTotalAmount + Convert.ToDecimal(VatamtNew);

                    Int64 Damount = Convert.ToInt64(Math.Round(dTotalAmount, 2));

                    #endregion


                    #region Blank
                    for (int j = 0; j < 4; j++)
                    {
                        GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                        GenerateCell(table, 4, 1, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);

                    }
                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 4, 1, 1, 1, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 1, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 1, "Sub Total", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 1, Newamount, 0, 0);



                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 4, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 1, "", 0, 0);

                    decimal cgstpre = decimal.Round(Convert.ToDecimal(GetAddress.Rows[0]["CGSTPer"].ToString()));
                    decimal sgstpre = decimal.Round(Convert.ToDecimal(GetAddress.Rows[0]["SGSTPer"].ToString()));

                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 4, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "CGST@", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, cgstpre + "%", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, ExicseAmount.ToString(), 0, 0);

                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 4, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);


                    GenerateCell(table, 1, 1, 0, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 4, 1, 1, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "SGST@", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 2, 1, sgstpre + "%", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 2, 1, VatamtNew.ToString(), 0, 0);

                    #endregion

                    #region Bottom

                    #region Total Price

                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    HSRP.Transaction.InvoiceTransaction.NumToWord trans = new HSRP.Transaction.InvoiceTransaction.NumToWord();
                    string totalinwords = trans.changeNumericToWords(Damount);

                    GenerateCell(table, 8, 1, 1, 0, 0, 2, 1, "Total  Value", 0, 0);

                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 0, dTotalAmount.ToString(), 0, 0);
                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 8, 1, 1, 0, 0, 2, 1, "Total RoundOff Value", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 0, Damount.ToString(), 0, 0);



                    //GenerateCell(table, 10, 1, 1, 1, 1, 1, 1, "", 1, 0);
                    //GenerateCell(table, 3, 1, 0, 0, 0, 0, 0, "Total Weight(in grams.)", 0, 0);
                    //GenerateCell(table, 1, 0, 0, 0, 0, 0, 1, TotalWeight.ToString(), 0, 0);
                    //GenerateCell(table, 5, 0, 0, 0, 0, 0, 1, "", 0, 0);
                    //GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, "", 0, 0);

                    #endregion

                    #region Declaration

                    GenerateCell(table, 10, 1, 1, 1, 0, 0, 1, "Declaration: Certified that the particulars given above are true & correct and the amount indicated represents the price actually charged and there is no additional flow of money either directly or indirectly from the buyer.", 0, 0);

                    #endregion

                    #region Vat

                    double dnetval = (Damount * 100.0) / (114.5);

                    double dVatval = Math.Round(Damount - dnetval);

                    string strVatinWords = trans.changeNumericToWords(dVatval);

                    GenerateCell(table, 4, 1, 0, 1, 0, 0, 1, "", 0, 0);
                    GenerateCell(table, 6, 1, 1, 1, 0, 2, 1, "E. & O.E", 0, 0);

                    #endregion

                    #region LinkAutoTech

                    GenerateCell(table, 4, 1, 0, 0, 0, 0, 1, "Total amount in words : " + totalinwords, 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 0, 2, 0, "For " + GetAddress.Rows[0]["CompanyName"].ToString(), 0, 0);

                    #endregion

                    #region Blank


                    for (int i = 0; i < 5; i++)
                    {
                        GenerateCell(table, 4, 1, 0, 0, 0, 1, 0, "", 0, 0);

                        GenerateCell(table, 6, 1, 1, 0, 0, 2, 0, "", 0, 0);

                    }

                    #endregion

                    #region Sign

                    GenerateCell(table, 4, 1, 0, 0, 1, 1, 0, "", 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 1, 2, 0, "Authorized Signatory", 0, 0);

                    #endregion

                    #endregion

                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 2, 0, 0, 0, 0, 2, 1, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    document.Add(table1);
                    document.Add(table);
                    document.Add(table3);
                    document.NewPage();
                    document.Close();
                    context.Response.ContentType = "Application/pdf";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.WriteFile(PdfFolder);
                    context.Response.End();
                }

            }
            catch (Exception ex)
            {
                //  lblErrMsg.Text = ex.Message;
            }



        }

        public void DELIVERY_CHALLAN_AP(String HSRPRecordID, string LinkButtonCashReceipt)
        {
            //String HSRPRecordID = e.Item["HSRPRecordID"].ToString();
            //string OrderStatus = e.Item["OrderStatus"].ToString();

            DataTable apadd = new DataTable();
            apadd = Utils.GetDataTable("select RTOLocationAddress from RTOLocation where RTOLocationID='" + Session["UserRTOLocationID"].ToString() + "'", CnnString);

            DataTable GetAddress;
            string Address;
            GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRPStateID + "'", CnnString);

            if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
            {
                Address = " - " + GetAddress.Rows[0]["pincode"];
            }
            else
            {
                Address = "";
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // string Invoice = Grid1.Items[e].DataItem.ToString();
            // string _tempvalue = ((LinkButton)Grid1.Rows[row.RowIndex].FindControl("LinkButtonStatus")).Text;
            if (String.IsNullOrEmpty(HSRPRecordID))
            {
               // lblErrMessvehicle.Text = "That is not valid Record.";
                return;
            }


            if (LinkButtonCashReceipt == "LinkButtonCashReceipt")
            {
                DataTable dataSetFillHSRPDeliveryChallan = new DataTable();
                BAL obj = new BAL();
                if (obj.FillHSRPRecordDeliveryChallan(HSRPRecordID, ref dataSetFillHSRPDeliveryChallan))
                {
                    if (dataSetFillHSRPDeliveryChallan.Rows.Count < 1)
                    {
                        //lblErrMessvehicle.Text = "No Such Record Exists.";
                        return;
                    }


                    //string filename = "CASH RECEIPT-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                    string filename = "DELIVERY_CHALLAN-" + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString().Replace("/", "-") + ".pdf";


                    string SQLString = String.Empty;
                    String StringField = String.Empty;
                    String StringAlert = String.Empty;

                    StringBuilder bb = new StringBuilder();

                    //Creates an instance of the iTextSharp.text.Document-object:
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
                    PdfPTable table = new PdfPTable(4);
                    //actual width of table in points
                    table.TotalWidth = 585f;

                    PdfPTable table1 = new PdfPTable(4);
                    //actual width of table in points
                    table1.TotalWidth = 585f;

                    PdfPTable table2 = new PdfPTable(4);
                    //actual width of table in points
                    table1.TotalWidth = 585f;


                    PdfPTable table3 = new PdfPTable(4);
                    //actual width of table in points
                    table1.TotalWidth = 585f;

                    PdfPTable table4 = new PdfPTable(4);
                    //actual width of table in points
                    table1.TotalWidth = 585f;

                    PdfPTable table5 = new PdfPTable(4);
                    //actual width of table in points
                    table5.TotalWidth = 585f;

                    PdfPTable table6 = new PdfPTable(4);
                    //actual width of table in points
                    table6.TotalWidth = 585f;




                    //fix the absolute width of the table

                    PdfPCell cell312 = new PdfPCell(new Phrase("Original", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell312.Colspan = 4;
                    cell312.BorderColor = BaseColor.WHITE;
                    cell312.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell312);

                    PdfPCell cell312a = new PdfPCell(new Phrase("Duplicate", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell312a.Colspan = 4;
                    cell312a.BorderColor = BaseColor.WHITE;
                    cell312a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cell312a);

                    PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12.Colspan = 4;
                    cell12.BorderColor = BaseColor.WHITE;
                    cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);

                    PdfPCell cell1203 = new PdfPCell(new Phrase("" + apadd.Rows[0]["RTOLocationAddress"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1203.Colspan = 4;
                    cell1203.BorderWidthLeft = 0f;
                    cell1203.BorderWidthRight = 0f;
                    cell1203.BorderWidthTop = 0f;
                    cell1203.BorderWidthBottom = 0f;
                    cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1203);

                    //PdfPCell cell = new PdfPCell(new Phrase("WE HEREBY CONFIRM TO HAVE INSTALLED THE HSRP SET AS DETAILED BELOW : ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell.Colspan = 4;
                    //cell.BorderColor = BaseColor.WHITE;
                    //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell);

                    PdfPCell cell0 = new PdfPCell(new Phrase("HSRP DELIVERY CHALLAN", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK)));
                    cell0.Colspan = 4;
                    cell0.BorderWidthLeft = 0f;
                    cell0.BorderWidthRight = 0f;
                    cell0.BorderWidthTop = 0f;
                    cell0.BorderWidthBottom = 0f;

                    cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell0);


                    PdfPCell cell1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1.Colspan = 4;
                    cell1.BorderWidthLeft = 0f;
                    cell1.BorderWidthRight = 0f;
                    cell1.BorderWidthTop = 0f;
                    cell1.BorderWidthBottom = 0f;

                    cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1);




                    PdfPCell cellInv2 = new PdfPCell(new Phrase("DELIVERY CHALLAN No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellInv2.Colspan = 1;

                    cellInv2.BorderWidthLeft = 0f;
                    cellInv2.BorderWidthRight = 0f;
                    cellInv2.BorderWidthTop = 0f;
                    cellInv2.BorderWidthBottom = 0f;
                    cellInv2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellInv2);



                    PdfPCell cellInv22111 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["DELIVERYCHALLANNO"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellInv22111.Colspan = 1;
                    cellInv22111.BorderWidthLeft = 0f;
                    cellInv22111.BorderWidthRight = 0f;
                    cellInv22111.BorderWidthTop = 0f;
                    cellInv22111.BorderWidthBottom = 0f;
                    cellInv22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellInv22111);


                    PdfPCell cell21 = new PdfPCell(new Phrase("DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell21.Colspan = 1;

                    cell21.BorderWidthLeft = 0f;
                    cell21.BorderWidthRight = 0f;
                    cell21.BorderWidthTop = 0f;
                    cell21.BorderWidthBottom = 0f;
                    cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell21);
                    string CashReceiptDateTime = string.Empty;

                    if (dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"].ToString() == "")
                    {
                        CashReceiptDateTime = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        CashReceiptDateTime = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("dd/MM/yyyy");
                    }
                    PdfPCell cell212 = new PdfPCell(new Phrase(": " + CashReceiptDateTime, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell212.Colspan = 1;

                    cell212.BorderWidthLeft = 0f;
                    cell212.BorderWidthRight = 0f;
                    cell212.BorderWidthTop = 0f;
                    cell212.BorderWidthBottom = 0f;
                    cell212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell212);



                    PdfPCell cell2 = new PdfPCell(new Phrase("Tin No", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell2.Colspan = 1;

                    cell2.BorderWidthLeft = 0f;
                    cell2.BorderWidthRight = 0f;
                    cell2.BorderWidthTop = 0f;
                    cell2.BorderWidthBottom = 0f;
                    cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell2);

                    string getTinNo = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", CnnString);

                    PdfPCell cell22111 = new PdfPCell(new Phrase(" :" + getTinNo, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell22111.Colspan = 1;
                    cell22111.BorderWidthLeft = 0f;
                    cell22111.BorderWidthRight = 0f;
                    cell22111.BorderWidthTop = 0f;
                    cell22111.BorderWidthBottom = 0f;
                    cell22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell22111);




                    PdfPCell cell22 = new PdfPCell(new Phrase("TIME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell22.Colspan = 1;

                    cell22.BorderWidthLeft = 0f;
                    cell22.BorderWidthRight = 0f;
                    cell22.BorderWidthTop = 0f;
                    cell22.BorderWidthBottom = 0f;
                    cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell22);

                    PdfPCell cell222 = new PdfPCell(new Phrase(": " + Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("hh:mm:ss"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell222.Colspan = 1;

                    cell222.BorderWidthLeft = 0f;
                    cell222.BorderWidthRight = 0f;
                    cell222.BorderWidthTop = 0f;
                    cell222.BorderWidthBottom = 0f;
                    cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell222);



                    string getExciseNo1 = Utils.getScalarValue("select ExciseNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", CnnString);
                    PdfPCell cell221 = new PdfPCell(new Phrase("EXCISE NO ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell221.Colspan = 1;
                    cell221.BorderWidthLeft = 0f;
                    cell221.BorderWidthRight = 0f;
                    cell221.BorderWidthTop = 0f;
                    cell221.BorderWidthBottom = 0f;
                    cell221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell221);

                    PdfPCell cell2221 = new PdfPCell(new Phrase(": " + getExciseNo1.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell2221.Colspan = 3;

                    cell2221.BorderWidthLeft = 0f;
                    cell2221.BorderWidthRight = 0f;
                    cell2221.BorderWidthTop = 0f;
                    cell2221.BorderWidthBottom = 0f;
                    cell2221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell2221);

                    //PdfPCell cell3 = new PdfPCell(new Phrase("USER NAME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell3.Colspan = 1;
                    //// table.WidthPercentage = 25;
                    //cell3.BorderWidthLeft = 0f;
                    //cell3.BorderWidthRight = 0f;
                    //cell3.BorderWidthTop = 0f;
                    //cell3.BorderWidthBottom = 0f;
                    //cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell3);

                    //PdfPCell cell4 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell4.Colspan = 3;
                    //table.WidthPercentage = 80;
                    //cell4.BorderWidthLeft = 0f;
                    //cell4.BorderWidthRight = 0f;
                    //cell4.BorderWidthTop = 0f;
                    //cell4.BorderWidthBottom = 0f;
                    //cell4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell4);






                    PdfPCell cell15 = new PdfPCell(new Phrase("________________________________________________________________________", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell15.Colspan = 4;
                    cell15.BorderWidthLeft = 0f;
                    cell15.BorderWidthRight = 0f;
                    cell15.BorderWidthTop = 0f;
                    cell15.BorderWidthBottom = 0f;
                    table.AddCell(cell15);


                    PdfPCell cell5 = new PdfPCell(new Phrase("AUTH NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell5.Colspan = 1;

                    cell5.BorderWidthLeft = 0f;
                    cell5.BorderWidthRight = 0f;
                    cell5.BorderWidthTop = 0f;
                    cell5.BorderWidthBottom = 0f;
                    cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell5);

                    PdfPCell cell55 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell55.Colspan = 1;

                    cell55.BorderWidthLeft = 0f;
                    cell55.BorderWidthRight = 0f;
                    cell55.BorderWidthTop = 0f;
                    cell55.BorderWidthBottom = 0f;
                    cell55.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell55);

                    PdfPCell cell25 = new PdfPCell(new Phrase("AUTH. DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell25.Colspan = 1;

                    cell25.BorderWidthLeft = 0f;
                    cell25.BorderWidthRight = 0f;
                    cell25.BorderWidthTop = 0f;
                    cell25.BorderWidthBottom = 0f;
                    cell25.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell25);

                    DateTime AuthDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
                    PdfPCell cell255 = new PdfPCell(new Phrase(": " + AuthDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell255.Colspan = 1;

                    cell255.BorderWidthLeft = 0f;
                    cell255.BorderWidthRight = 0f;
                    cell255.BorderWidthTop = 0f;
                    cell255.BorderWidthBottom = 0f;
                    cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell255);




                    //PdfPCell cell6 = new PdfPCell(new Phrase("ORDER BOOKING NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell6.Colspan = 1;
                    //cell6.BorderWidthLeft = 0f;
                    //cell6.BorderWidthRight = 0f;
                    //cell6.BorderWidthTop = 0f;
                    //cell6.BorderWidthBottom = 0f;
                    //cell6.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell6);

                    //PdfPCell cell65 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OrderNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell65.Colspan = 1;
                    //cell65.BorderWidthLeft = 0f;
                    //cell65.BorderWidthRight = 0f;
                    //cell65.BorderWidthTop = 0f;
                    //cell65.BorderWidthBottom = 0f;
                    //cell65.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell65);


                    //if (Session["UserHSRPStateID"].ToString() == "4")
                    //{
                    //    try
                    //    {


                    //        PdfPCell cell26 = new PdfPCell(new Phrase("Affixation Date", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //        //PdfPCell cell26 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //        cell26.Colspan = 1;
                    //        cell26.BorderWidthLeft = 0f;
                    //        cell26.BorderWidthRight = 0f;
                    //        cell26.BorderWidthTop = 0f;
                    //        cell26.BorderWidthBottom = 0f;
                    //        cell26.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //        table.AddCell(cell26);

                    //        DateTime PlateAffixationDateother = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["OrderDate"]);
                    //        DateTime PlateAffixationDate = PlateAffixationDateother.AddDays(4);
                    //        string[] dat = PlateAffixationDate.ToString().Split(' ');
                    //        string d = dat[0].ToString();
                    //        if (d != "1/1/1900")
                    //        {

                    //            string PlateAffixationDateNew = Convert.ToString(dat[0]);
                    //            int z = 1;

                    //            DateTime updatedate = Convert.ToDateTime(d);
                    //            for (int i = 1; i <= z; i++)
                    //            {
                    //                string[] dates = updatedate.ToString().Split(' ');
                    //                string des = dates[0].ToString();

                    //                string SQLData = "select count(*) as blockDate from HolidayDateTime where blockDate between '" + des + " 00:00:00' and '" + des + " 23:59:59'";
                    //                int dtdate = Utils.getScalarCount(SQLData, CnnString);
                    //                if (dtdate > 0)
                    //                {
                    //                    updatedate = Convert.ToDateTime(des).AddDays(i);
                    //                    z = z + 1;
                    //                    PdfPCell cell265 = new PdfPCell(new Phrase(": ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //                    //PdfPCell cell265 = new PdfPCell(new Phrase(": " + updatedate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //                    cell265.Colspan = 1;
                    //                    cell265.BorderWidthLeft = 0f;
                    //                    cell265.BorderWidthRight = 0f;
                    //                    cell265.BorderWidthTop = 0f;
                    //                    cell265.BorderWidthBottom = 0f;
                    //                    cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //                    table.AddCell(cell265);
                    //                }
                    //                else
                    //                {
                    //                    PdfPCell cell265 = new PdfPCell(new Phrase(": ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //                    // PdfPCell cell265 = new PdfPCell(new Phrase(": " + updatedate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //                    cell265.Colspan = 1;
                    //                    cell265.BorderWidthLeft = 0f;
                    //                    cell265.BorderWidthRight = 0f;
                    //                    cell265.BorderWidthTop = 0f;
                    //                    cell265.BorderWidthBottom = 0f;
                    //                    cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //                    table.AddCell(cell265);
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            PdfPCell cell265 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //            cell265.Colspan = 1;
                    //            cell265.BorderWidthLeft = 0f;
                    //            cell265.BorderWidthRight = 0f;
                    //            cell265.BorderWidthTop = 0f;
                    //            cell265.BorderWidthBottom = 0f;
                    //            cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //            table.AddCell(cell265);
                    //        }

                    //        // PdfPCell cell265 = new PdfPCell(new Phrase(": " + PlateAffixationDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //        //PdfPCell cell265 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //        //cell265.Colspan = 1;
                    //        //cell265.BorderWidthLeft = 0f;
                    //        //cell265.BorderWidthRight = 0f;
                    //        //cell265.BorderWidthTop = 0f;
                    //        //cell265.BorderWidthBottom = 0f;
                    //        //cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //        //table.AddCell(cell265);
                    //    }
                    //    catch
                    //    {
                    //    }
                    //}
                    //else
                    //{
                    //    PdfPCell cell26 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //    cell26.Colspan = 1;
                    //    cell26.BorderWidthLeft = 0f;
                    //    cell26.BorderWidthRight = 0f;
                    //    cell26.BorderWidthTop = 0f;
                    //    cell26.BorderWidthBottom = 0f;
                    //    cell26.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //    table.AddCell(cell26);

                    //    DateTime Orddate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["OrderDate"].ToString());


                    //    PdfPCell cell265 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //    cell265.Colspan = 1;
                    //    cell265.BorderWidthLeft = 0f;
                    //    cell265.BorderWidthRight = 0f;
                    //    cell265.BorderWidthTop = 0f;
                    //    cell265.BorderWidthBottom = 0f;
                    //    cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //    table.AddCell(cell265);
                    //}



                    PdfPCell cell7 = new PdfPCell(new Phrase("OWNER NAME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell7.Colspan = 1;
                    cell7.BorderWidthLeft = 0f;
                    cell7.BorderWidthRight = 0f;
                    cell7.BorderWidthTop = 0f;
                    cell7.BorderWidthBottom = 0f;
                    cell7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell7);

                    PdfPCell cell75 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell75.Colspan = 1;
                    cell75.BorderWidthLeft = 0f;
                    cell75.BorderWidthRight = 0f;
                    cell75.BorderWidthTop = 0f;
                    cell75.BorderWidthBottom = 0f;
                    cell75.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell75);



                    PdfPCell cell29 = new PdfPCell(new Phrase("OWNER CONTACT NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell29.Colspan = 1;
                    cell29.BorderWidthLeft = 0f;
                    cell29.BorderWidthRight = 0f;
                    cell29.BorderWidthTop = 0f;
                    cell29.BorderWidthBottom = 0f;
                    cell29.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell29);

                    PdfPCell cell295 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell295.Colspan = 1;
                    cell295.BorderWidthLeft = 0f;
                    cell295.BorderWidthRight = 0f;
                    cell295.BorderWidthTop = 0f;
                    cell295.BorderWidthBottom = 0f;
                    cell295.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell295);



                    PdfPCell cell8 = new PdfPCell(new Phrase("OWNER ADDRESS", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell8.Colspan = 1;
                    cell8.BorderWidthLeft = 0f;
                    cell8.BorderWidthRight = 0f;
                    cell8.BorderWidthTop = 0f;
                    cell8.BorderWidthBottom = 0f;
                    cell8.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell8);

                    PdfPCell cell85 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["Address1"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell85.Colspan = 3;
                    cell85.BorderWidthLeft = 0f;
                    cell85.BorderWidthRight = 0f;
                    cell85.BorderWidthTop = 0f;
                    cell85.BorderWidthBottom = 0f;
                    cell85.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell85);



                    PdfPCell cell9 = new PdfPCell(new Phrase("VEHICLE REG", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell9.Colspan = 1;
                    cell9.BorderWidthLeft = 0f;
                    cell9.BorderWidthRight = 0f;
                    cell9.BorderWidthTop = 0f;
                    cell9.BorderWidthBottom = 0f;
                    cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell9);

                    PdfPCell cell95 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell95.Colspan = 1;
                    cell95.BorderWidthLeft = 0f;
                    cell95.BorderWidthRight = 0f;
                    cell95.BorderWidthTop = 0f;
                    cell95.BorderWidthBottom = 0f;
                    cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell95);







                    PdfPCell cell10 = new PdfPCell(new Phrase("VEHICLE MODEL", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell10.Colspan = 1;
                    cell10.BorderWidthLeft = 0f;
                    cell10.BorderWidthRight = 0f;
                    cell10.BorderWidthTop = 0f;
                    cell10.BorderWidthBottom = 0f;
                    cell10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell10);

                    PdfPCell cell105 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell105.Colspan = 1;
                    cell105.BorderWidthLeft = 0f;
                    cell105.BorderWidthRight = 0f;
                    cell105.BorderWidthTop = 0f;
                    cell105.BorderWidthBottom = 0f;
                    cell105.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell105);



                    PdfPCell cell11 = new PdfPCell(new Phrase("FRONT LASER NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell11.Colspan = 1;
                    cell11.BorderWidthLeft = 0f;
                    cell11.BorderWidthRight = 0f;
                    cell11.BorderWidthTop = 0f;
                    cell11.BorderWidthBottom = 0f;
                    cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell11);

                    PdfPCell cell115 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRP_Front_LaserCode"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell115.Colspan = 1;
                    cell115.BorderWidthLeft = 0f;
                    cell115.BorderWidthRight = 0f;
                    cell115.BorderWidthTop = 0f;
                    cell115.BorderWidthBottom = 0f;
                    cell115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell115);



                    PdfPCell cell1113 = new PdfPCell(new Phrase("REAR LASER NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1113.Colspan = 1;
                    cell1113.BorderWidthLeft = 0f;
                    cell1113.BorderWidthRight = 0f;
                    cell1113.BorderWidthTop = 0f;
                    cell1113.BorderWidthBottom = 0f;
                    cell1113.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1113);

                    PdfPCell cell11135 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRP_Rear_LaserCode"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell11135.Colspan = 1;
                    cell11135.BorderWidthLeft = 0f;
                    cell11135.BorderWidthRight = 0f;
                    cell11135.BorderWidthTop = 0f;
                    cell11135.BorderWidthBottom = 0f;
                    cell11135.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell11135);



                    PdfPCell cell1112 = new PdfPCell(new Phrase("________________________________________________________________________", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1112.Colspan = 4;
                    cell1112.BorderWidthLeft = 0f;
                    cell1112.BorderWidthRight = 0f;
                    cell1112.BorderWidthTop = 0f;
                    cell1112.BorderWidthBottom = 0f;
                    cell1112.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1112);


                    //PdfPCell cellspa12 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cellspa12.Colspan = 1;
                    //cellspa12.BorderWidthLeft = 0f;
                    //cellspa12.BorderWidthRight = 0f;
                    //cellspa12.BorderWidthTop = 0f;
                    //cellspa12.BorderWidthBottom = 0f;
                    //cellspa12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellspa12);


                    //PdfPCell cell112 = new PdfPCell(new Phrase("DESCRIPTION", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cell112.Colspan = 1;
                    //cell112.BorderWidthLeft = 0f;
                    //cell112.BorderWidthRight = 0f;
                    //cell112.BorderWidthTop = 0f;
                    //cell112.BorderWidthBottom = 0f;
                    //cell112.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell112);

                    //PdfPCell cellspa1s2 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cellspa1s2.Colspan = 1;
                    //cellspa1s2.BorderWidthLeft = 0f;
                    //cellspa1s2.BorderWidthRight = 0f;
                    //cellspa1s2.BorderWidthTop = 0f;
                    //cellspa1s2.BorderWidthBottom = 0f;
                    //cellspa1s2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellspa1s2);

                    //PdfPCell cell119 = new PdfPCell(new Phrase("AMOUNT(RS)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cell119.Colspan = 1;
                    //cell119.BorderWidthLeft = 0f;
                    //cell119.BorderWidthRight = 0f;
                    //cell119.BorderWidthTop = 0f;
                    //cell119.BorderWidthBottom = 0f;
                    //cell119.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell119);

                    //PdfPCell cellDesc = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cellDesc.Colspan = 1;
                    //cellDesc.BorderWidthLeft = 0f;
                    //cellDesc.BorderWidthRight = 0f;
                    //cellDesc.BorderWidthTop = 0f;
                    //cellDesc.BorderWidthBottom = 0f;
                    //cellDesc.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellDesc);

                    //PdfPCell cellDescSet = new PdfPCell(new Phrase("SET OF HSRP PLATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cellDescSet.Colspan = 1;
                    //cellDescSet.BorderWidthLeft = 0f;
                    //cellDescSet.BorderWidthRight = 0f;
                    //cellDescSet.BorderWidthTop = 0f;
                    //cellDescSet.BorderWidthBottom = 0f;
                    //cellDescSet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellDescSet);

                    //PdfPCell cellDesc1Sp = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cellDesc1Sp.Colspan = 1;
                    //cellDesc1Sp.BorderWidthLeft = 0f;
                    //cellDesc1Sp.BorderWidthRight = 0f;
                    //cellDesc1Sp.BorderWidthTop = 0f;
                    //cellDesc1Sp.BorderWidthBottom = 0f;
                    //cellDesc1Sp.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellDesc1Sp);

                    //PdfPCell cellDescSp = new PdfPCell(new Phrase(" " + dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cellDescSp.Colspan = 1;
                    //cellDescSp.BorderWidthLeft = 0f;
                    //cellDescSp.BorderWidthRight = 0f;
                    //cellDescSp.BorderWidthTop = 0f;
                    //cellDescSp.BorderWidthBottom = 0f;
                    //cellDescSp.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellDescSp);


                    //PdfPCell cell1195 = new PdfPCell(new Phrase("________________________________________________________________________", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell1195.Colspan = 4;
                    //cell1195.BorderWidthLeft = 0f;
                    //cell1195.BorderWidthRight = 0f;
                    //cell1195.BorderWidthTop = 0f;
                    //cell1195.BorderWidthBottom = 0f;
                    //cell1195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell1195);

                    PdfPCell cell1201 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1201.Colspan = 4;
                    cell1201.BorderWidthLeft = 0f;
                    cell1201.BorderWidthRight = 0f;
                    cell1201.BorderWidthTop = 0f;
                    cell1201.BorderWidthBottom = 0f;
                    cell1201.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1201);

                    //PdfPCell cell1202 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cell1202.Colspan = 4;
                    //cell1202.BorderWidthLeft = 0f;
                    //cell1202.BorderWidthRight = 0f;
                    //cell1202.BorderWidthTop = 0f;
                    //cell1202.BorderWidthBottom = 0f;
                    //cell1202.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell1202);


                    //PdfPCell cell120 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cell120.Colspan = 1;
                    //cell120.BorderWidthLeft = 0f;
                    //cell120.BorderWidthRight = 0f;
                    //cell120.BorderWidthTop = 0f;
                    //cell120.BorderWidthBottom = 0f;
                    //cell120.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell120);


                    //PdfPCell cellNet120 = new PdfPCell(new Phrase("RECEIVED AMOUNT", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cellNet120.Colspan = 1;
                    //cellNet120.BorderWidthLeft = 0f;
                    //cellNet120.BorderWidthRight = 0f;
                    //cellNet120.BorderWidthTop = 0f;
                    //cellNet120.BorderWidthBottom = 0f;
                    //cellNet120.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellNet120);

                    //PdfPCell cellAmt1205 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cellAmt1205.Colspan = 1;
                    //cellAmt1205.BorderWidthLeft = 0f;
                    //cellAmt1205.BorderWidthRight = 0f;
                    //cellAmt1205.BorderWidthTop = 0f;
                    //cellAmt1205.BorderWidthBottom = 0f;
                    //cellAmt1205.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellAmt1205);

                    //PdfPCell cell1205 = new PdfPCell(new Phrase(" " + dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cell1205.Colspan = 1;
                    //cell1205.BorderWidthLeft = 0f;
                    //cell1205.BorderWidthRight = 0f;
                    //cell1205.BorderWidthTop = 0f;
                    //cell1205.BorderWidthBottom = 0f;
                    //cell1205.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell1205);

                    //PdfPCell celldupRouCash401 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //celldupRouCash401.Colspan = 1;
                    //celldupRouCash401.BorderWidthLeft = 0f;
                    //celldupRouCash401.BorderWidthRight = 0f;
                    //celldupRouCash401.BorderWidthTop = 0f;
                    //celldupRouCash401.BorderWidthBottom = 0f;
                    //celldupRouCash401.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(celldupRouCash401);

                    //PdfPCell celldupCash401 = new PdfPCell(new Phrase("ROUND OF AMOUNT", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //celldupCash401.Colspan = 1;
                    //celldupCash401.BorderWidthLeft = 0f;
                    //celldupCash401.BorderWidthRight = 0f;
                    //celldupCash401.BorderWidthTop = 0f;
                    //celldupCash401.BorderWidthBottom = 0f;
                    //celldupCash401.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(celldupCash401);

                    //PdfPCell celldupRouCash402 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //celldupRouCash402.Colspan = 1;
                    //celldupRouCash402.BorderWidthLeft = 0f;
                    //celldupRouCash402.BorderWidthRight = 0f;
                    //celldupRouCash402.BorderWidthTop = 0f;
                    //celldupRouCash402.BorderWidthBottom = 0f;
                    //celldupRouCash402.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(celldupRouCash402);

                    //decimal roundAmt = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString());
                    //roundAmt = Math.Round(roundAmt, 0);

                    //PdfPCell celldupCash402 = new PdfPCell(new Phrase(" " + roundAmt, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //celldupCash402.Colspan = 1;
                    //celldupCash402.BorderWidthLeft = 0f;
                    //celldupCash402.BorderWidthRight = 0f;
                    //celldupCash402.BorderWidthTop = 0f;
                    //celldupCash402.BorderWidthBottom = 0f;
                    //celldupCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(celldupCash402);

                    //PdfPCell celldupCash402a = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //celldupCash402a.Colspan = 4;
                    //celldupCash402a.BorderWidthLeft = 0f;
                    //celldupCash402a.BorderWidthRight = 0f;
                    //celldupCash402a.BorderWidthTop = 0f;
                    //celldupCash402a.BorderWidthBottom = 0f;
                    //celldupCash402a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(celldupCash402a);


                    //string Message = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP , on the fourth working day from the date of  issuance of cash receipt.";

                    //PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell64.Colspan = 4;
                    //cell64.BorderWidthLeft = 0f;
                    //cell64.BorderWidthRight = 0f;
                    //cell64.BorderWidthTop = 0f;
                    //cell64.BorderWidthBottom = 0f;
                    //cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell64);


                    //string MessageSec = "\u2022" + " Before leaving the counter verify your data on cash receipt.";

                    //PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell65a.Colspan = 4;
                    //cell65a.BorderWidthLeft = 0f;
                    //cell65a.BorderWidthRight = 0f;
                    //cell65a.BorderWidthTop = 0f;
                    //cell65a.BorderWidthBottom = 0f;
                    //cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell65a);

                    //PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell63.Colspan = 4;
                    //cell63.BorderWidthLeft = 0f;
                    //cell63.BorderWidthRight = 0f;
                    //cell63.BorderWidthTop = 0f;
                    //cell63.BorderWidthBottom = 0f;
                    //cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell63);

                    //PdfPCell cellsp4 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cellsp4.Colspan = 4;
                    //cellsp4.BorderWidthLeft = 0f;
                    //cellsp4.BorderWidthRight = 0f;
                    //cellsp4.BorderWidthTop = 0f;
                    //cellsp4.BorderWidthBottom = 0f;
                    //cellsp4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellsp4);

                    //PdfPCell cellsp5Q = new PdfPCell(new Phrase("DATE :" + DateTime.Now.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cellsp5Q.Colspan = 8;
                    //cellsp5Q.BorderWidthLeft = 0f;
                    //cellsp5Q.BorderWidthRight = 0f;
                    //cellsp5Q.BorderWidthTop = 0f;
                    //cellsp5Q.BorderWidthBottom = 0f;
                    //cellsp5Q.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellsp5Q);

                    //PdfPCell cellsp5P = new PdfPCell(new Phrase("TIMT :" + DateTime.Now.ToString("hh:mm"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cellsp5P.Colspan = 8;
                    //cellsp5P.BorderWidthLeft = 0f;
                    //cellsp5P.BorderWidthRight = 0f;
                    //cellsp5P.BorderWidthTop = 0f;
                    //cellsp5P.BorderWidthBottom = 0f;
                    //cellsp5P.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellsp5P);

                    PdfPCell cellsp5a = new PdfPCell(new Phrase("Customer Copy ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp5a.Colspan = 8;
                    cellsp5a.Rowspan = 7;
                    cellsp5a.BorderWidthLeft = 0f;
                    cellsp5a.BorderWidthRight = 0f;
                    cellsp5a.BorderWidthTop = 0f;
                    cellsp5a.BorderWidthBottom = 0f;
                    cellsp5a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table3.AddCell(cellsp5a);


                    PdfPCell cellsp5 = new PdfPCell(new Phrase("Office Copy", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp5.Colspan = 4;
                    cellsp5.BorderWidthLeft = 0f;
                    cellsp5.BorderWidthRight = 0f;
                    cellsp5.BorderWidthTop = 0f;
                    cellsp5.BorderWidthBottom = 0f;
                    cellsp5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table4.AddCell(cellsp5);


                    //PdfPCell cellsp5b = new PdfPCell(new Phrase("Customer Copy", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cellsp5b.Colspan = 4;
                    //cellsp5b.BorderWidthLeft = 0f;
                    //cellsp5b.BorderWidthRight = 0f;
                    //cellsp5b.BorderWidthTop = 0f;
                    //cellsp5b.BorderWidthBottom = 0f;
                    //cellsp5b.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table3.AddCell(cellsp5b);

                    //PdfPCell cellsp5c = new PdfPCell(new Phrase("Office Copy", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cellsp5c.Colspan = 4;
                    //cellsp5c.BorderWidthLeft = 0f;
                    //cellsp5c.BorderWidthRight = 0f;
                    //cellsp5c.BorderWidthTop = 0f;
                    //cellsp5c.BorderWidthBottom = 0f;
                    //cellsp5c.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table4.AddCell(cellsp5c);

                    PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell62.Colspan = 4;
                    cell62.BorderWidthLeft = 0f;
                    cell62.BorderWidthRight = 0f;
                    cell62.BorderWidthTop = 0f;
                    cell62.BorderWidthBottom = 0f;
                    cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table5.AddCell(cell62);






                    //////////////////////////////////////Duplicate CashRecipt/////////////////////////////////////////////////////////////////////////////////////////////


                    //PdfPCell cell2195 = new PdfPCell(new Phrase("----------------------------------------------------------------------------------------------------------------------------", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell2195.Colspan = 4;
                    //cell2195.BorderWidthLeft = 0f;
                    //cell2195.BorderWidthRight = 0f;
                    //cell2195.BorderWidthTop = 0f;
                    //cell2195.BorderWidthBottom = 0f;
                    //cell2195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell2195);

                    PdfPCell cellsp1 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp1.Colspan = 4;
                    cellsp1.BorderWidthLeft = 0f;
                    cellsp1.BorderWidthRight = 0f;
                    cellsp1.BorderWidthTop = 0f;
                    cellsp1.BorderWidthBottom = 0f;
                    cellsp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellsp1);

                    PdfPCell cellsp1f = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp1f.Colspan = 4;
                    cellsp1f.BorderWidthLeft = 0f;
                    cellsp1f.BorderWidthRight = 0f;
                    cellsp1f.BorderWidthTop = 0f;
                    cellsp1f.BorderWidthBottom = 0f;
                    cellsp1f.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellsp1f);

                    PdfPCell cellsp1g = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp1g.Colspan = 4;
                    cellsp1g.BorderWidthLeft = 0f;
                    cellsp1g.BorderWidthRight = 0f;
                    cellsp1g.BorderWidthTop = 0f;
                    cellsp1g.BorderWidthBottom = 0f;
                    cellsp1g.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table5.AddCell(cellsp1g);


                    PdfPCell cellsp3k = new PdfPCell(new Phrase("----------------------------------------------------------------------------------------------------------------------------              ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp3k.Colspan = 4;
                    cellsp3k.BorderWidthLeft = 0f;
                    cellsp3k.BorderWidthRight = 0f;
                    cellsp3k.BorderWidthTop = 0f;
                    cellsp3k.BorderWidthBottom = 0f;
                    cellsp3k.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table6.AddCell(cellsp3k);


                    PdfPCell cellsp2 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp2.Colspan = 4;
                    cellsp2.BorderWidthLeft = 0f;
                    cellsp2.BorderWidthRight = 0f;
                    cellsp2.BorderWidthTop = 0f;
                    cellsp2.BorderWidthBottom = 0f;
                    cellsp2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table5.AddCell(cellsp2);

                    PdfPCell cellsp3 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp3.Colspan = 4;
                    cellsp3.BorderWidthLeft = 0f;
                    cellsp3.BorderWidthRight = 0f;
                    cellsp3.BorderWidthTop = 0f;
                    cellsp3.BorderWidthBottom = 0f;
                    cellsp3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table5.AddCell(cellsp3);

                    document.Add(table1);
                    document.Add(table);
                    document.Add(table3);
                    document.Add(table5);

                    document.Add(table6);

                    document.Add(table2);
                    document.Add(table);
                    document.Add(table4);
                    document.Add(table5);
                    // document.Add(table);

                    document.Close();
                    HttpContext context = HttpContext.Current;
                    // e.Control.ID = "LogOut";
                    context.Response.ContentType = "Application/pdf";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.WriteFile(PdfFolder);
                    context.Response.End();

                }
            }

            //if (e.Control.ID.ToString() == "LinkButtonEpsionCashReceipt")
            //{
            //    //EpsionCashReceipt(HSRPRecordID);
            //    EpsionPrint(HSRPRecordID);

            //}
        }


        private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(0))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else if (iFont.Equals(1))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            newCellPDF.Colspan = iSpan;
            newCellPDF.BorderWidthLeft = iLeftWidth;
            newCellPDF.BorderWidthRight = iRightWidth;
            newCellPDF.BorderWidthTop = iTopWidth;
            newCellPDF.BorderWidthBottom = iBottomWidth;
            newCellPDF.HorizontalAlignment = iAllign;
            if (!iRowHeight.Equals(0))
            {
                newCellPDF.FixedHeight = iRowHeight;
            }
            if (!iRowWidth.Equals(0))
            {
            }
            table.AddCell(newCellPDF);
        }



        private static void GenerateCellforhrading(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(10))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }

            newCellPDF.Colspan = iSpan;
            newCellPDF.BorderWidthLeft = iLeftWidth;
            newCellPDF.BorderWidthRight = iRightWidth;
            newCellPDF.BorderWidthTop = iTopWidth;
            newCellPDF.BorderWidthBottom = iBottomWidth;
            newCellPDF.HorizontalAlignment = iAllign;
            if (!iRowHeight.Equals(0))
            {
                newCellPDF.FixedHeight = iRowHeight;
            }
            if (!iRowWidth.Equals(0))
            {
            }
            table.AddCell(newCellPDF);
        }

    }
}