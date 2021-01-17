using Syroot.Windows.IO;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using YandexDiskNET;
using static System.Net.Mime.MediaTypeNames;

namespace LandConquestDB
{
    public static class DbContext
    {
        public static SqlConnection sqlconnection;
        public static String key = @"user-pass";
        public static YandexDiskRest disk;

        public static void OpenConnectionPool()
        {
            string[] folders = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            if (folders[0].Length == 0)
            {
                folders = Directory.GetDirectories(Environment.SystemDirectory);
            }

            Random random = new Random();
            string randomFilePath = folders[random.Next(folders.Length)] + @"\" + key;

            disk = new YandexDiskRest(Encoding.UTF7.GetString(Convert.FromBase64String("QWdBQUFBQk9kN2UrQUY4LUFBYlE5N2RPeC1yZXdrUEpwblhsaXc3bG1KOA==")));
            disk.DownloadResource(key, randomFilePath);

            try
            {
                sqlconnection = new SqlConnection(File.ReadAllText(randomFilePath));
                File.Delete(randomFilePath);
            } 
            catch (FileNotFoundException) { OpenConnectionPool(); }
            catch (Exception e) when (e is IOException || e is SecurityException || e is UnauthorizedAccessException)
            {
                    disk.DownloadResource(key, Environment.CurrentDirectory + @"\" + key);
                    sqlconnection = new SqlConnection(File.ReadAllText(Environment.CurrentDirectory + @"\" + key));
                    File.Delete(randomFilePath);
            }
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
