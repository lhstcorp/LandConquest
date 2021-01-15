using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace LandConquestDB
{
    public static class DbContext
    {
        public static SqlConnection connection;
        public static void OpenConnectionPool()
        {
            string encodedCdb = ConfigurationManager.ConnectionStrings["greendend2"].ConnectionString;
            byte[] dataCdb = Convert.FromBase64String(encodedCdb);
            string decodedCdb = Encoding.UTF7.GetString(dataCdb);

            connection = new SqlConnection(decodedCdb);
            connection.Open();
        }
        public static SqlConnection GetConnection()
        {
            return connection;
        }
        public static void OpenConnection()
        {
            connection.Open();
        }
        public static void CloseConnection()
        {
            connection.Close();
        }

    }
}
