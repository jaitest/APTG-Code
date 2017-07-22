using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Data;
using System.Configuration;

namespace HSRP.AP
{
    public partial class ImpExpData : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
        int intOrgID;

        int UID;
        string SaveLocation = string.Empty;
        StringBuilder SBSQL = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                int.TryParse(Session["UserType"].ToString(), out UserType);
                int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRPStateID);
                int.TryParse(Session["UserRTOLocationID"].ToString(), out RTOLocationID);
                int.TryParse(Session["UID"].ToString(), out UID);

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                //strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                string usrname1 = Utils.getDataSingleValue("Select UserFirstName + space(2)+UserLastName as UserName From Users where UserID=" + UID.ToString(), CnnString, "UserName");
                LabelCreatedID.Text = usrname1;
                LabelCreatedDateTime.Text = DateTime.Now.ToString("dd MMM yyyy");

                if (!IsPostBack)
                {
                    string StateID = Session["UserHSRPStateID"].ToString();
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
            {
                InsertDataInstage();
            }
            else
            {
                llbMSGError.Text = "Please select a file to upload.";
                llbMSGSuccess.Text = "";
            }
        }

        private void InsertDataInstage()
        {
            llbMSGError.Text = "";
            string filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
            SaveLocation = ConfigurationManager.AppSettings["DataFolder"].ToString() + filename;
            try
            {
                FileUpload1.PostedFile.SaveAs(SaveLocation);
                string CSVFilePathName = SaveLocation;
                string[] Lines = File.ReadAllLines(CSVFilePathName);
                string[] Fields;
                int countInsert = 0;
                int countDuplicate = 0;
                Fields = Lines[0].Split(new char[] {'|'});
                int Cols = Fields.GetLength(0);
                DataTable dt = new DataTable();
                CreateDynamicDataTable(dt, "EmpId", "timestamp", "latNetwork", "lonNetwork", "latGps", "lonGps");

                DataRow Row;
                for (int i = 0; i < Lines.GetLength(0); i++)
                {
                    Fields = Lines[i].Split(new char[] { '|' });
                    if (Fields.Length == Cols)
                    {
                        Row = dt.NewRow();
                        for (int f = 0; f < Cols; f++)
                        {
                            Row[f] = Fields[f];
                        }
                        dt.Rows.Add(Row);
                    }
                    else if (Fields.Length > 0)
                    {
                        llbMSGError.Text = "Error In This Line " + i;
                        return;
                    }
                }

                StringBuilder sbColumns = new StringBuilder();
                StringBuilder sbRows = new StringBuilder();
                string EmpId = string.Empty;
                string timestamp = string.Empty;
                string latNetwork = string.Empty;
                string lonNetwork = string.Empty;
                string latGps = string.Empty;
                string lonGps = string.Empty;
                string CreatedBy = string.Empty;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    EmpId = dt.Rows[i][0].ToString();
                    timestamp = dt.Rows[i][1].ToString();
                    latNetwork = dt.Rows[i][2].ToString();
                    lonNetwork = dt.Rows[i][3].ToString();
                    latGps = dt.Rows[i][4].ToString();
                    lonGps = dt.Rows[i][5].ToString();
                    string Hsrp_History = string.Empty;
                    //Hsrp_History = "INSERT INTO AttendanceDetails (EmpId,[timestamp],latNetwork,lonNetwork,latGps,lonGps,CreatedBy,CreatedDate) values ('" + EmpId + "','" + timestamp + "','" + latNetwork + "','" + lonNetwork + "','" + latGps + "','" + lonGps + "','" + UID + "',getdate())";
                    Hsrp_History = "InsertAttendanceDetails '" + EmpId + "','" + timestamp + "','" + latNetwork + "','" + lonNetwork + "','" + latGps + "','" + lonGps + "','" + UID + "'";
                    Utils.ExecNonQuery(Hsrp_History, CnnString);
                    countInsert = countInsert + 1;
                    //string chkRecord = "SELECT COUNT(*) FROM AttendanceDetails WHERE EmpId='" + EmpId + "' and convert(date,CreatedDate)=convert(date,getdate())";
                    //int count = Utils.getScalarCount(chkRecord, CnnString);
                    //if (count < 1)
                    //{
                    //    string Hsrp_History = string.Empty;
                    //    Hsrp_History = "INSERT INTO AttendanceDetails (EmpId,[timestamp],latNetwork,lonNetwork,latGps,lonGps,CreatedBy,CreatedDate) values ('" + EmpId + "','" + timestamp + "','" + latNetwork + "','" + lonNetwork + "','" + latGps + "','" + lonGps + "','" + UID + "',getdate())";
                    //    Utils.ExecNonQuery(Hsrp_History, CnnString);
                    //    countInsert = countInsert + 1;
                    //}
                    //else
                    //{
                    //    countDuplicate = countDuplicate + 1;
                    //}
                    sbColumns.Clear();
                    sbRows.Clear();
                }
                Utils.ExecNonQuery("Insert INTO UploadFileLog (UploadedBy,FileName) VALUES ('" + UID + "','" + filename + "');", CnnString);
                llbFileName.Text = " File Name : " + filename;
                llbMSGSuccess.Text = " Total Inserted Records is : " + countInsert;
                llbMSGError.Text = " And Duplicate Records is Found: " + countDuplicate;
            }
            catch (Exception ex)
            {
                llbMSGError.Text = "Error: " + ex.Message;
            }
            finally
            {
            }
        }

        public void CreateDynamicDataTable(DataTable dtResult, params string[] dtColName)
        {
            for (int idtColCount = 0; idtColCount < dtColName.Length; idtColCount++)
            {
                dtResult.Columns.Add(dtColName[idtColCount], typeof(string));
            }
        }
    }
}