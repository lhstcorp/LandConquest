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
using System.Windows;

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
            YDContext.CheckIfCheater();
            Thread antiCheatThread = new Thread(new ThreadStart(DisableActiveCheatsLoop));
            antiCheatThread.Start();
        }

        private static async void DisableActiveCheatsLoop()
        {
            var hackToolsArray = new[] { "cheatengine", "eroxengine", "erox engine", "quick memory editor", "quickmemoryeditor", "cheat engine", "ramcheat", "ram cheat", "cosmos", "wemod", "memdig", "artmoney","cheat tool", "cheattool", "squarl", "hacktool", "hack tool", "easyhook", "cheathappens", "crysearch"};

            while (true)
                foreach (Process process in Process.GetProcesses())
                {
                    try
                    {
                        FileVersionInfo file = FileVersionInfo.GetVersionInfo(process.MainModule.FileName);
                        if (
                        process.ProcessName != null && hackToolsArray.Any(process.ProcessName.ToLower().Contains) ||                   
                        file.CompanyName != null && hackToolsArray.Any(file.CompanyName.ToLower().Contains) ||
                        file.FileName != null && hackToolsArray.Any(file.FileName.ToLower().Contains) ||
                        file.FileDescription != null && hackToolsArray.Any(file.FileDescription.ToLower().Contains) ||
                        file.InternalName != null && hackToolsArray.Any(file.InternalName.ToLower().Contains) ||
                        file.OriginalFilename != null && hackToolsArray.Any(file.OriginalFilename.ToLower().Contains) ||
                        file.Comments != null && hackToolsArray.Any(file.Comments.ToLower().Contains) ||
                        file.LegalTrademarks != null && hackToolsArray.Any(file.LegalTrademarks.ToLower().Contains) ||
                        file.ProductName != null && hackToolsArray.Any(file.ProductName.ToLower().Contains) ||
                        file.LegalCopyright != null && hackToolsArray.Any(file.ProductName.ToLower().Contains) ||
                        file.SpecialBuild != null && hackToolsArray.Any(file.ProductName.ToLower().Contains) 
                        )
                        {
                            try
                            {
                                await Task.Run(() => 
                                {
                                    YDContext.BanDeviceById(
                                        DateTime.UtcNow.ToString() + " " +
                                        process.ProcessName + " " +
                                        file.CompanyName + " " +
                                        file.FileDescription + " " +
                                        file.InternalName + " " +
                                        file.OriginalFilename + " " +
                                        file.Comments + " " +
                                        file.LegalTrademarks + " " +
                                        file.LegalCopyright + " " +
                                        file.SpecialBuild + " " +
                                        file.ProductName);
                                    YDContext.DeleteConnectionId();
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        WarningDialogWindow.CallWarningDialogNoResult("You were automatically banned for using cheating tools!");
                                    });
                                    
                                    Environment.Exit(0);
                                });
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
