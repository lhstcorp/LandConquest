using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using PCloud.Metadata;
using PCloud;
using System.Threading.Tasks;

namespace LandConquestDB
{
    public static class DbContext
    {
        public static SqlConnection sqlonnection;
        public static Connection connection;
        public static void OpenConnectionPool()
        {
            //const string accountMail = "";
            //const string accountPassword = "";
            //const string localFile = @"C:\Temp\1.zip";
            //const string remoteFile = @"";

            //const bool ssl = true;

            //async Task<bool> login(Connection conn)
            //{
            //    if (conn.isDesynced)
            //        return false;
            //    await conn.login(accountMail, accountPassword);
            //    return true;
            //}

            string encodedCdb = ConfigurationManager.ConnectionStrings["user-pass"].ConnectionString;
            byte[] dataCdb = Convert.FromBase64String(encodedCdb);
            string decodedCdb = Encoding.UTF7.GetString(dataCdb);

            sqlonnection = new SqlConnection(decodedCdb);
            sqlonnection.Open();
        }
        public static SqlConnection GetConnection()
        {
            return sqlonnection;
        }
        public static void OpenConnection()
        {
            sqlonnection.Open();
        }
        public static void CloseConnection()
        {
            sqlonnection.Close();
        }

    }
}
