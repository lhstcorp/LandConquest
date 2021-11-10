using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using LandConquestYD;

namespace LandConquestDB
{
    public static class DbContext
    {
        private static SqlConnection sqlconnection;
        private static string value = "player_secured";
        public static void OpenConnectionPool()
        {
            try
            {
                var dynasties = YDContext.ReadResource("Dynasty.txt");
                string[] dynastiesLines = dynasties.Split('\n');
                
                int dynastyLineNum = 3;

                dynastiesLines = dynastiesLines.Where(w => w != dynastiesLines[dynastyLineNum]).ToArray();

                var newstr = string.Join("\n", dynastiesLines);

                YDContext.UploadStringToResource(@"Dynasty.txt", newstr, true);

                Console.WriteLine(newstr);


                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                sqlconnection = new SqlConnection(YDCrypto.Decrypt(YDContext.ReadResource(value)));
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }
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
            SqlConnection sqlconnectiontask = new SqlConnection(YDCrypto.Decrypt(YDContext.ReadResource(value)));
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
