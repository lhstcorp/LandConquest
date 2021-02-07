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
            var reference = "user-pass";
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

    }
}
