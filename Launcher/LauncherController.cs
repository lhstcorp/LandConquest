using LandConquest.DialogWIndows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LandConquest.Launcher
{
    public static class LauncherController
    {
        public static void CheckLocalDateTime()
        {
            try
            {
                var client = new TcpClient("time.nist.gov", 13);
                using (var streamReader = new StreamReader(client.GetStream()))
                {
                    var response = streamReader.ReadToEnd();
                    var utcDateTimeString = response.Substring(7, 17);
                    var utcOnlineTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
                    DateTime localDateTime = DateTime.Now;
                    TimeSpan interval = localDateTime.Subtract(utcOnlineTime);
                    int minutesDiff = interval.Minutes;
                    if (minutesDiff != 0)
                    {
                        WarningDialogWindow window = new WarningDialogWindow("Time on your device set incorrectly! Please synchronize your time and try again");
                        window.Show();
                        if (window.DialogResult == true)
                        {
                            Environment.Exit(0);
                        }
                    }
                }
            }
            catch (IOException) { }
            catch (WebException) { }
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
                        catch (ArgumentException) { }
                    }
                }
                catch (Win32Exception) { }
                catch (Exception e) { }
            }
        }
    }
}
