using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using YandexDiskNET;

namespace LandConquestDB
{
    public static class DbContext
    {
        private static SqlConnection sqlconnection;      
        private static YandexDiskRest disk;
        public static void OpenConnectionPool()
        {
            string key = @"user-pass";
            string[] folders = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).Union
                (Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))).Union
                (Directory.GetDirectories(Environment.CurrentDirectory)).ToArray();

            Random random = new Random();
            string randomFilePath = folders[random.Next(folders.Length)] + @"\" + key;

            disk = new YandexDiskRest(Encoding.UTF7.GetString(Convert.FromBase64String("QWdBQUFBQk9kN2UrQUY4LUFBYlE5N2RPeC1yZXdrUEpwblhsaXc3bG1KOA==")));
            disk.DownloadResource(key, randomFilePath);          
            try
            {
                string connstr = File.ReadAllText(randomFilePath);
                sqlconnection = new SqlConnection(connstr);
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
