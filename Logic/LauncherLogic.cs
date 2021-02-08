using LandConquest.DialogWIndows;
using LandConquestYD;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace LandConquest.Logic
{
    public static class LauncherLogic
    {
        public static async void CheckLocalUtcDateTime()
        {
            try
            {
                var client = new TcpClient("time.nist.gov", 13);
                using (var streamReader = new StreamReader(client.GetStream()))
                {
                    var response = await Task.Run(() => streamReader.ReadToEndAsync());
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
            Thread antiCheatThread = new Thread(new ThreadStart(DisableActiveCheatsLoop));
            antiCheatThread.Start();
        }

        private static async void DisableActiveCheatsLoop()
        {
            var hackToolsArray = new[] { "cheat", "hack", "cosmos", "wemod", "gameconqueror", "artmoney", "squarl" };

            while(true)
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
                           await Task.Run(() => process.Kill());
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception) { }
            }
        }

        public static string CheckGameVersion()
        {
            return YDContext.ReadResource("GameVersion");
        }

        public static void DownloadGame()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://gamejolt.com/games/LandConquest/577432",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
