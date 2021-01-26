using LandConquest.DialogWIndows;
using LandConquestDB;
using Syroot.Windows.IO;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using YandexDiskNET;

namespace LandConquest.Logic
{
    public static class LauncherLogic
    {
        public static void CheckLocalUtcDateTime()
        {
            try
            {
                var client = new TcpClient("time.nist.gov", 13);
                using (var streamReader = new StreamReader(client.GetStream()))
                {
                    var response = streamReader.ReadToEnd();
                    var utcDateTimeString = response.Substring(7, 17);
                    var utcOnlineTime = DateTimeOffset.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
                    DateTimeOffset localDateTime = DateTime.UtcNow;
                    TimeSpan interval = localDateTime.Subtract(utcOnlineTime);
                    int minutesDiff = interval.Minutes;
                    if (minutesDiff != 0)
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Time on your device set incorrectly! Please synchronize your time and try again.");
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception) { }
        }

        public static void DisableActiveCheats()
        {
            var hackToolsArray = new[] { "cheat", "hack", "cosmos", "wemod", "gameconqueror", "artmoney", "squarl" };

            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    FileVersionInfo file = FileVersionInfo.GetVersionInfo(process.MainModule.FileName);
                    if (
                        hackToolsArray.Any(process.ProcessName.ToLower().Contains) ||
                        file.CompanyName != null && hackToolsArray.Any(file.CompanyName.ToLower().Contains) ||
                        file.FileName != null && hackToolsArray.Any(file.FileName.ToLower().Contains) ||
                        file.FileDescription != null && hackToolsArray.Any(file.FileDescription.ToLower().Contains) ||
                        file.InternalName != null && hackToolsArray.Any(file.InternalName.ToLower().Contains) ||
                        file.OriginalFilename != null && hackToolsArray.Any(file.OriginalFilename.ToLower().Contains)
                        )
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception) { }
            }
        }

        public static async Task CheckGameVersion()
        {
            string downloadsPath = new KnownFolder(KnownFolderType.Downloads).Path;
            string sourceFileName = "GameVersion";
            string destFileName = downloadsPath + @"\GameVersion";
            var disk = YDContext.GetYD();
            await Task.WhenAll(disk.DownloadResourceAcync(sourceFileName, destFileName));
        }

        public static ErrorResponse DownloadGame()
        {
            string downloadsPath = new KnownFolder(KnownFolderType.Downloads).Path;

            string sourceFileName = "LandConquest.exe";
            string destFileName = downloadsPath + @"\LandConquest.exe";
            YandexDiskRest disk = YDContext.GetYD();
            return disk.DownloadResource(sourceFileName, destFileName);
        }
    }
}
