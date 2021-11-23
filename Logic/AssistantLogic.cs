using LandConquest.DialogWIndows;
using LandConquestYD;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LandConquest.Logic
{
    public static class AssistantLogic
    {
        public static async void CheckLocalUtcDateTime()
        {
            await Task.Run(() => CheckLocalUtcDateTimeAsync());
        }
        private static void CheckLocalUtcDateTimeAsync()
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
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            WarningDialogWindow.CallWarningDialogNoResult("Time on your device set incorrectly! Please synchronize your time and try again.");
                        });
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception) { }
        }


        public static async void DisableActiveCheatsAsync()
        {
            YDContext.CheckIfCheater();
            await Task.Run(() => DisableActiveCheatsLoop());
        }

        private static async void DisableActiveCheatsLoop()
        {
            var hackToolsArray = new[] { "cheatengine", "eroxengine", "erox engine", "quick memory editor", "quickmemoryeditor", "cheat engine", "ramcheat", "ram cheat", "cosmos", "wemod", "memdig", "artmoney", "cheat tool", "cheattool", "squarl", "hacktool", "hack tool", "easyhook", "cheathappens", "crysearch", "reclass", "mhs.exe", "mxtract", "memoryjs", "invtero", "scylla", "ollydbg", "reshacker", "megadumper" };

            while (true)
            {
                await Task.Delay(10000);
                foreach (Process process in Process.GetProcesses())
                {
                    try
                    {
                        FileVersionInfo file = FileVersionInfo.GetVersionInfo(process.MainModule.FileName);
                        if (
                        process.ProcessName != null && hackToolsArray.Any(process.ProcessName.ToLower().Contains) ||
                        process.MainModule.ModuleName != null && hackToolsArray.Any(process.MainModule.ModuleName.ToLower().Contains) ||
                        process.MainModule.FileName != null && hackToolsArray.Any(process.MainModule.FileName.ToLower().Contains) ||
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
                                YDContext.BanDeviceById(
                                        DateTime.UtcNow.ToString() + " " +
                                        process.ProcessName + " " +
                                        process.MainModule.ModuleName + " " +
                                        process.MainModule.FileName + " " +
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
                            }
                            catch (Exception) { }
                        }
                    }
                    catch (Exception) 
                    {
                        continue;
                    }
                }
                Console.WriteLine("Checked cheats");                
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

        public static void CallSplashScreen()
        {
            var splashScreen = new SplashScreen("Pictures/splashscreen.png");
            splashScreen.Show(true, true);
        }

        public static void CloseWindowByTag(object tag)
        {
            if (tag is null)
            {
                return;
            }

            (from Window w in App.Current.Windows
             where w.Tag == tag
             select w)?.FirstOrDefault()?.Close();
        }

        public static string GenerateId() //Use this to generate any id in game
        {
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvmxyz0123456789";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                int length = 16;
                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }
            return res.ToString();
        }
    }
}
