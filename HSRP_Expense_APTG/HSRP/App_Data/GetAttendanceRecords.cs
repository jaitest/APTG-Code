using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HSRP
{
    public class GetAttendanceRecords
    {
        public DataTable GetRecordsAttendanceLog(string employeeID,DateTime monthName)
        {
            try
            {
                string Query = "GetRecordsAttendanceLog";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Query;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmployeeId", SqlDbType.VarChar).Value = employeeID;
                cmd.Parameters.Add("@MonthName", SqlDbType.DateTime).Value = monthName;
                return DMLSql.MYInstance.GetRecords(cmd);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetApproveSystemRecords(bool approve)
        {
            try
            {
                string Query = "GetApproveSystemRecords";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Query;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Approve", SqlDbType.Bit).Value = approve;               
                return DMLSql.MYInstance.GetRecords(cmd);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ValidateMACAddress(string macAddress)
        {
            try
            {
                string Query = "ValidateMACAddress";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Query;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MACAddress", SqlDbType.VarChar).Value = macAddress;
                return DMLSql.MYInstance.GetSingleRecord(cmd);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string InValidateMACAddress(string macAddress)
        {
            try
            {
                string Query = "InValidateMACAddress";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Query;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MACAddress", SqlDbType.VarChar).Value = macAddress;
                return DMLSql.MYInstance.GetSingleRecord(cmd);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
