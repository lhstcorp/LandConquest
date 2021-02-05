using System;
using System.Data.SqlClient;

namespace LandConquestDB
{
    public static class DbContext
    {
        private static SqlConnection sqlconnection;
        public static void OpenConnectionPool()
        {
            var reference = "glandeil";
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
