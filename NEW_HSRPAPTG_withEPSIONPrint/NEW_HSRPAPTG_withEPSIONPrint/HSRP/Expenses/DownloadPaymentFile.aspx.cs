using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using iTextSharp.text;
using CarlosAg.ExcelXmlWriter;
using System.IO;
using Ionic.Zip;

namespace HSRP.Expenses
{
    public partial class DownloadPaymentFile : System.Web.UI.Page
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
                
            }
        }

        public void DownloadlogfileAP()
        {
            if(Session["UserHSRPStateID"].ToString()=="9" || Session["UserHSRPStateID"].ToString()=="11")
            { 
                try
                {
                    string strPath = string.Empty;
                    strPath = @"C:\RequestFolder";
                    //strPath = @"D:\downloadFile";
                    string[] filePaths = Directory.GetFiles(strPath, "*.*", SearchOption.AllDirectories);
                    //string[] filePaths = Directory.GetFiles(Server.MapPath("~/Files/"));
                    ///Files/
                    List<System.Web.UI.WebControls.ListItem> files = new List<System.Web.UI.WebControls.ListItem>();
                    foreach (string filePath in filePaths)
                    {
                        files.Add(new System.Web.UI.WebControls.ListItem(Path.GetFileName(filePath), filePath));
                    }

                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                        zip.AddDirectoryByName("Files");

                        foreach (string s in filePaths)
                        {
                            Console.WriteLine(s);
                            string filePath = s.ToString();
                            zip.AddFile(filePath, "Files");
                        }

                        Response.Clear();
                        Response.BufferOutput = false;
                        string zipName = String.Format("APLogFile.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                        zip.Save(Response.OutputStream);
                        //zip.Save(strPath + "\\" + zipName);
                        Response.End();
                    }
                }
        
                
            catch (Exception ex)
            {
                throw ex;
            }
            }
        }


        protected void btndownloadAP_Click(object sender, EventArgs e)
        {
            DownloadlogfileAP();
        }
       

      

        

    }
}
