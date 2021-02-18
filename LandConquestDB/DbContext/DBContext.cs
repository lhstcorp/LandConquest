using System;
using System.Data.SqlClient;
using LandConquestYD;

namespace LandConquestDB
{
    public static class DbContext
    {
        private static SqlConnection sqlconnection;
        public static void OpenConnectionPool()
        {
            var reference = "online_old.txt";
            try
            {
                sqlconnection = new SqlConnection(YDContext.ReadResource(reference));
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

        public static void Reconnect()
        {
            sqlconnection.Close();
            sqlconnection.Dispose();
            SqlConnection.ClearAllPools();
            OpenConnectionPool();
        }
    }
}
