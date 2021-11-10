using System;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using YandexDiskNET;
using DeviceId;
using System.Net;
using System.Net.Http.Handlers;
using System.Threading.Tasks;

namespace LandConquestYD
{
    public static class YDContext
    {
        private static YandexDiskRest disk;
        private static string oauth;
        private static string ConnectionSourceFileName;
        public static void OpenYD()
        {
            Connection();
            Disk();
        }

        private static void Connection()
        {
            oauth = Properties.Settings.Default.Token;
        }

        private static void Disk()
        {
            disk = new YandexDiskRest(oauth);
        }

        public static YandexDiskRest GetYD()
        {
            return disk;
        }

        public static int CountConnections()
        {
            ConnectionSourceFileName = "onlinenow_" + GetDeviceId();
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + ConnectionSourceFileName;
            File.AppendAllText(path, "");
            disk.UploadResource("countstatus/" + ConnectionSourceFileName, path, true);
            disk.PublicResource("countstatus/" + ConnectionSourceFileName);
            File.Delete(path);

            int count = 0;
            ResInfo connectionInfo = disk.GetResourcePublic(
                1000,
                TypeRes.File,
                new ResFields[] {
                    ResFields.Name,
                },
                0, true, "120x240"
                );

            if (connectionInfo.ErrorResponse.Message == null)
            {

                if (connectionInfo._Embedded.Items.Count != 0)
                    foreach (var s in connectionInfo._Embedded.Items)
                        if (s.Name.Contains("onlinenow_"))
                        {
                            count++;
                        }
            }
            return count;
        }

        public static void DeleteConnectionId()
        {
            disk.DeleteResource("countstatus/" + ConnectionSourceFileName, false);
        }

        private static string GetDeviceId()
        {
            string deviceId = new DeviceIdBuilder()
            .AddProcessorId()
            .AddMotherboardSerialNumber()
            .ToString();
            return deviceId;
        }

        public static void BanDeviceById(string information)
        {
            string destFileName = "cheater_" + GetDeviceId();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + destFileName;
            File.AppendAllText(path, information);
            disk.UploadResource("BanList/" + destFileName, path, true);
            File.Delete(path);
        }

        public static void CheckIfCheater()
        {
            var result = ReadResource("BanList/cheater_" + GetDeviceId());
            if (result != null)
            {
                Environment.Exit(0);
            }
        }

        public static bool UploadBugReport(string playerName, string text)
        {
            string destFileName = @"BugReport_" + playerName + DateTime.UtcNow.ToString().Replace(":", "_") + @".log";
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + destFileName;
            File.AppendAllText(path, text);
            var result = disk.UploadResource("SubBugs/" + destFileName, path, true);
            File.Delete(path);
            if (result.Error != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        private static string CommandDisk(string oauth, Param param, bool download = true)
        {
            HttpMethod method = HttpMethod.Get;
            HttpClient httpClient = new HttpClient();
            UrlBuilder urlBuilder = new UrlBuilder(param);

            string requestUri;

            if (download)
            {
                requestUri = "https://cloud-api.yandex.net/v1/disk/resources/download?" + urlBuilder.Path;
            } 
            else
            {
                requestUri = "https://cloud-api.yandex.net/v1/disk/resources/upload?" + urlBuilder.Path + urlBuilder.Overwrite;
            }

            try
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, requestUri);
                httpRequestMessage.Headers.Add("Authorization", "OAuth " + oauth);
                httpRequestMessage.Headers.Add("Accept", "application/json");
                return httpClient.SendAsync(httpRequestMessage).Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {
                throw new HttpRequestException();
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        public static string ReadResource(string sourceFileName)
        {
            var parameter = default(Param);
            parameter.Path = sourceFileName;
            string result = ReadFile((string)JObject.Parse(CommandDisk(oauth, parameter)).SelectToken("href"));
            return result;
        }
        private static string ReadFile(string url)
        {
            HttpClient httpClient = HttpClientFactory.Create();
            try
            {
                HttpResponseMessage httpResponse = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                using (Stream stream = httpResponse.Content.ReadAsStreamAsync().Result)
                {
                    var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    return Encoding.ASCII.GetString(memoryStream.ToArray());
                }

            }
            catch (Exception) { return null; }
        }

        public static void UploadStringToResource(string destFileName, string fileText, bool overwrite = true)
        {
            Param param = default(Param);
            param.Path = destFileName;
            param.Overwrite = overwrite;
            string text = CommandDisk(oauth, param, false);
            if (text != null)
            {
                UploadStringToFileAsync((string)JObject.Parse(text).SelectToken("href"), fileText);
            }
        }

        private static void UploadStringToFileAsync(string url, string fileText)
        {
            HttpClient httpClient = HttpClientFactory.Create();

            byte[] byteArray = Encoding.ASCII.GetBytes(fileText);

            using (StreamContent content = new StreamContent(new MemoryStream(byteArray)))
            {
                var result = httpClient.PutAsync(url, content).Result;
            }
        }
    }
}
