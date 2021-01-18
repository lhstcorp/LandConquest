using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security;
using YandexDiskNET;
using Spritely.Recipes;

namespace LandConquestDB
{
    public static class DbContext
    {
        private static SqlConnection sqlconnection;      
        private static YandexDiskRest disk;
        public static void OpenConnectionPool()
        {
            string key = @"glandeil";
            string[] folders = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).Union
                (Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))).Union
                (Directory.GetDirectories(Environment.CurrentDirectory)).ToArray();

            Random random = new Random();
            string randomFilePath = folders[random.Next(folders.Length)] + @"\" + key;

            disk = new YandexDiskRest(KSecure.Normal.Decrypt("MqlRhwK82YvBPUBZmUlUMtoiO/x2nTTEO1R6fTVROvhqd5Tdfe0VeqyERZM8S8mZi551+pDkRt3pMIN8JETVguoCyZAOfsxOH1LG78LejN7j4OjozYQGyYm/FrBLqEq71MruafJBLilwuKE4EU2y+69w6az1cGeFrRB+jxb9814=", Environment.SystemDirectory));
            disk.DownloadResource(key, randomFilePath);          
            try
            {
                SecureString connstr = File.ReadAllText(randomFilePath).ToSecureString();
                sqlconnection = new SqlConnection(connstr.ToInsecureString());
                File.Delete(randomFilePath);
            } 
            catch (FileNotFoundException) { OpenConnectionPool(); }
            sqlconnection.OpenAsync();
        }

        public static SqlConnection GetSqlConnection()
        {
            return sqlconnection;
        }
        public static YandexDiskRest GetDisk()
        {
            return disk;
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
