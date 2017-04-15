using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using System.Text;
namespace BookAPOnlineHSRPRecords
{
    public class Utils
    {

        public string strProvider = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
        public string sqlText;
        public int CommandTimeOut = 0;
        public SqlConnection objConnection;

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
                ReturnValue = cmd.ExecuteScalar().ToString();;
            }
            return ReturnValue;
        }
        
        public static int ExecNonQuery(string SQLString, string CnnString)
        {
            try
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
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return ex.Number;
                }
                throw ex;
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
    }
}