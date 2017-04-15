using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;


    public class Utils
    {

        public string strProvider = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public string sqlText;
        public int CommandTimeOut = 0;
        public SqlConnection objConnection;
        public bool IsEmailValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public SqlDataReader GetReader()
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            MakeConnection();
            OpenConnection();
            cmd = GetCommand();
            cmd.CommandTimeout = 0;
            dr = cmd.ExecuteReader();
            return dr;
        }
        SqlCommand GetCommand()
        {
            return new SqlCommand(sqlText, objConnection);
        }
        public void MakeConnection()
        {

            objConnection = new SqlConnection(strProvider);
            // objConnection.= 200000;
        }
        public void OpenConnection()
        {

            objConnection.Open();
        }
        public void CloseConnection()
        {
            objConnection.Close();
        }
        public static string ConvertDateTimeToString(DateTime oDatatime)
        {
            string str = "";
            str = oDatatime.Month.ToString() + "/" + oDatatime.Day.ToString() + "/" + oDatatime.Year.ToString() + " " + oDatatime.Hour.ToString() + ":" + oDatatime.Minute.ToString() + ":" + oDatatime.Second.ToString();
            return str;
        }
        public static int getScalarCount(string SQLString, string CnnString)
        {
            int ReturnValue = 0;
            using (SqlConnection conn = new SqlConnection(CnnString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, conn);

                conn.Open();
                ReturnValue = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            return ReturnValue;
        }

        public static string getScalarValue(string SQLString, string CnnString)
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(CnnString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, conn);
                conn.Open();
                ReturnValue = (string)cmd.ExecuteScalar();
            }
            return ReturnValue;
        }
        public static string getScalarStringValue(string SQLString, string CnnString)
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(CnnString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, conn);
                conn.Open();
                ReturnValue = cmd.ExecuteScalar().ToString();
            }
            return ReturnValue;
        }
        //============================ComponentArt Menu===NEW-------------------------------//





        public static int ExecNonQuery(string SQLString, string CnnString)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(CnnString))
            {
                SqlCommand command = new SqlCommand(SQLString, connection);
                command.Connection.Open();
                count = command.ExecuteNonQuery();
                command.Connection.Close();
            }
            return count;
        }
        public static void PopulateDropDownList(DropDownList DropDownName, string SQLString, string CnnString, string DefaultValue)
        {
            try
            {
                Utils dbLink = new Utils();
                dbLink.strProvider = CnnString;
                dbLink.CommandTimeOut = 0;
                dbLink.sqlText = SQLString.ToString();
                SqlDataReader PReader = dbLink.GetReader();
                DropDownName.DataSource = PReader;
                DropDownName.DataBind();
                ListItem li = new ListItem(DefaultValue, DefaultValue);
                DropDownName.Items.Insert(0, li);
                PReader.Close();
                dbLink.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void PopulateListBox(ListBox ListBoxName, string SQLString, string CnnString, string DefaultValue)
        {
            try
            {
                Utils dbLink = new Utils();
                dbLink.strProvider = CnnString;
                dbLink.CommandTimeOut = 0;
                dbLink.sqlText = SQLString.ToString();
                SqlDataReader PReader = dbLink.GetReader();
                ListBoxName.DataSource = PReader;
                ListBoxName.DataBind();
                ListBoxName.Items.Add(DefaultValue);
                PReader.Close();
                dbLink.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataSet getDataSet(string SQLString, string CnnString)
        {
            SqlConnection Conn = new SqlConnection(CnnString);
            SqlDataAdapter DA = new SqlDataAdapter(SQLString, Conn);
            DA.SelectCommand.CommandTimeout = 0;
            Conn.Open();
            DataSet ReturnDs = new DataSet();
            DA.Fill(ReturnDs, "Table1");
            Conn.Close();
            return ReturnDs;
        }
        public static string getDataSingleValue(string SQLString, string CnnString, string colname)
        {
            string SingleValue = "";
            try
            {
                SqlConnection conn = new SqlConnection(CnnString);
                SqlCommand cmd = new SqlCommand(SQLString, conn);
                SqlDataAdapter returnVal = new SqlDataAdapter(SQLString, conn);
                conn.Close();
                conn.Open();
                DataTable dt = new DataTable("CharacterInfo");
                returnVal.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    SingleValue = dt.Rows[0][colname].ToString();
                }
                if (SingleValue == "")
                {
                    SingleValue = "0";
                }
                returnVal.Dispose();
                return SingleValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable GetDataTable(string SQLString, string CnnString)
        {
            Utils dbLink = new Utils();
            dbLink.strProvider = CnnString.ToString();
            dbLink.CommandTimeOut = 0;
            dbLink.sqlText = SQLString.ToString();
            SqlDataReader dr = dbLink.GetReader();
            DataTable tb = new DataTable();
            tb.Load(dr);
            dr.Close();
            dbLink.CloseConnection();
            return tb;

        }



        //for compression
        public static bool IsGZipSupported()
        {

            string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];

            if (!string.IsNullOrEmpty(AcceptEncoding) &&

                 AcceptEncoding.Contains("gzip") || AcceptEncoding.Contains("deflate"))

                return true;

            return false;

        }
        public static void GZipEncodePage()
        {

            //if (IsGZipSupported())
            //{

            //    HttpResponse Response = HttpContext.Current.Response;



            //    string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];

            //    if (AcceptEncoding.Contains("gzip"))
            //    {

            //        Response.Filter = new System.IO.Compression.GZipStream(Response.Filter,

            //                                  System.IO.Compression.CompressionMode.Compress);

            //        Response.AppendHeader("Content-Encoding", "gzip");

            //    }

            //    else
            //    {
            //        Response.Filter = new System.IO.Compression.DeflateStream(Response.Filter, System.IO.Compression.CompressionMode.Compress);
            //        Response.AppendHeader("Content-Encoding", "deflate");
            //    }

            //}

        }

        public static void user_log(string userid, string formname, string ClientLocalIP, string eventname, string MACAddress, string BrowserName, string ClientOSName, string CnnString)
        {
            string sdate = DateTime.Now.ToString();
            string sql = "INSERT INTO [USERLOG]([UserID],[formname],[eventname],[clientip],[MACAddress],[BrowserName],[ClientOSName]) VALUES ('" + userid + "','" + formname + "','" + eventname + "','" + ClientLocalIP + "','" + MACAddress + "','" + BrowserName + "','" + ClientOSName + "')";
            Utils.ExecNonQuery(sql, CnnString);

        }


        public static void Exception_log(string userid, string Formname, string computername, string exeption, string CnnString)
        {
            string strDate = DateTime.Now.ToString();
            string strQuery = "insert into Exceptionlog(LoginId,FormName,UpdateDateTime,ComputerIP,ExceptionName)values('" + userid + "','" + Formname + "','" + strDate + "','" + computername + "','" + exeption + "')";
            Utils.ExecNonQuery(strQuery, CnnString);
        }
        public static bool IsInteger(string theValue)
        {
            try
            {
                Convert.ToInt32(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //>>>> Export to Excel tanuj 24/12/2011
        public static void ExportToSpreadsheet(DataTable table, string name)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            foreach (DataColumn column in table.Columns)
            {
                context.Response.Write(column.ColumnName + ",");
            }
            context.Response.Write(Environment.NewLine);
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");
                }
                context.Response.Write(Environment.NewLine);
            }
            context.Response.ContentType = "text/csv";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + ".csv");
            context.Response.End();
        }

        //public static System.Boolean IsNumeric(System.Object Expression)
        //{
        //    if (Expression == null || Expression is DateTime)
        //        return false;

        //    if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
        //        return true;

        //    try
        //    {
        //        if (Expression is string)
        //            Double.Parse(Expression as string);
        //        else
        //            Double.Parse(Expression.ToString());
        //        return true;
        //    }
        //    catch { } // just dismiss errors but return false
        //    return false;
        //}


    }
