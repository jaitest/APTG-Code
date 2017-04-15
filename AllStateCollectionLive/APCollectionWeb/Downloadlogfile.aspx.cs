using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Ionic.Zip;

namespace APCollectionWeb
{
    public partial class Downloadlogfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }


        public void DownloadlogfileTG()
        {
            try
            {
                string strPath = string.Empty;
                strPath = @"D:\ERPHSRPLog";
                //strPath = @"D:\downloadFile";
                string[] filePaths = Directory.GetFiles(strPath, "*.*", SearchOption.AllDirectories);
                //string[] filePaths = Directory.GetFiles(Server.MapPath("~/Files/"));

                ///Files/
                List<ListItem> files = new List<ListItem>();
                foreach (string filePath in filePaths)
                {
                    files.Add(new ListItem(Path.GetFileName(filePath), filePath));
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
                    string zipName = String.Format("TGLogFile.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
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



        public void DownloadlogfileAP()
        {
            try
            {
                string strPath = string.Empty;
                strPath = @"D:\GetDataLog";
                //strPath = @"D:\downloadFile";
                string[] filePaths = Directory.GetFiles(strPath, "*.*", SearchOption.AllDirectories);
                //string[] filePaths = Directory.GetFiles(Server.MapPath("~/Files/"));

                ///Files/
                List<ListItem> files = new List<ListItem>();
                foreach (string filePath in filePaths)
                {
                    files.Add(new ListItem(Path.GetFileName(filePath), filePath));
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


        protected void btndownloadTG_Click(object sender, EventArgs e)
        {
            DownloadlogfileTG();
        }




        protected void btndownloadAP_Click(object sender, EventArgs e)
        {
            DownloadlogfileAP();
        }
       

    }
}