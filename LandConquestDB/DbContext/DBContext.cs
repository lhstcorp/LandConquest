using System;
using System.Data.SqlClient;
using LandConquestYD;

namespace LandConquestDB
{
    public static class DbContext
    {
        private static SqlConnection sqlconnection;
        private static string value = "user-pass";
        public static void OpenConnectionPool()
        {
            try
            {
                sqlconnection = new SqlConnection(YDContext.ReadResource(value));
            }
            catch (Exception) { }
            sqlconnection.Open();
        }

        public static SqlConnection GetSqlConnection()
        {
            return sqlconnection;
        }
        public static void OpenConnection()
        {
            sqlconnection.Open();
        }
        public static void CloseConnection()
        {
            sqlconnection.Close();
        }

        public static SqlConnection GetTempSqlConnection()
        {
            SqlConnection sqlconnectiontask = new SqlConnection(YDContext.ReadResource(value));
            return sqlconnectiontask;
        }

        public static void Reconnect()
        {
            sqlconnection.Dispose();
            SqlConnection.ClearAllPools();
            OpenConnectionPool();
        }
    }
}
