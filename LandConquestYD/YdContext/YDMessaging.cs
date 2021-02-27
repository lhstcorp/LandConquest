using System;
using System.IO;
using System.Reflection;
using YandexDiskNET;
using System.Collections.Generic;

namespace LandConquestYD
{
    public class YDMessaging
    {
        private static readonly YandexDiskRest disk = YDContext.GetYD();

        /// <summary>
        /// //////////////// messenger system ////////////////////
        /// </summary>

        public static void CreateDialog(string sender, string receiver)
        {
            string destFileName = sender + "_" + receiver + "_dialog.rtf";
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + destFileName;
            File.AppendAllText(path, "");
            disk.UploadResource("Messages/" + destFileName, path, true);
            File.Delete(path);
        }

        public static List<string> CheckForMessages(string playerName)
        {
            ResInfo filesByNameFields = disk.GetResourceByName(
               1000000,
               new Media_type[]
               {
                    Media_type.Document,
                    Media_type.Text
               },
               SortField.Path,
               new ResFields[] {
                    ResFields.Media_type,
                    ResFields.Name,
                    ResFields.Path,
                    ResFields._Embedded
               },
               0, true, "120x240"
               );

            List<string> messagesList = new List<String>();

            if (filesByNameFields.ErrorResponse.Message == null)
            {
                if (filesByNameFields._Embedded.Items.Count != 0)
                    foreach (var s in filesByNameFields._Embedded.Items)
                    {
                        if (s.Name.Contains("_dialog") && s.Name.Contains(playerName))
                        {
                            messagesList.Add(s.Name.Replace("dialog.rtf", "").Replace(playerName, "").Replace("_", ""));
                        }
                    }
            }

            return messagesList;
        }

        public static string GetDialogName(string player1, string player2)
        {
            ResInfo filesByNameFields = disk.GetResourceByName(
               1000000,
               new Media_type[]
               {
                    Media_type.Document,
                    Media_type.Text
               },
               SortField.Path,
               new ResFields[] {
                    ResFields.Media_type,
                    ResFields.Name,
                    ResFields.Path,
                    ResFields._Embedded
               },
               0, true, "120x240"
               );

            if (filesByNameFields.ErrorResponse.Message == null)
            {
                if (filesByNameFields._Embedded.Items.Count != 0)
                    foreach (var s in filesByNameFields._Embedded.Items)
                    {
                        if (s.Name.Contains("_dialog") && s.Name.Contains(player1) && s.Name.Contains(player2))
                        {
                            return s.Name;
                        }
                    }
            }
            return "";
        }

        public static void SendMessage(string messageText, string sender, string receiver)
        {
            string destFileName = sender + "_" + receiver + "_dialog.rtf";
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + destFileName;
            File.AppendAllText(path, messageText);
            disk.UploadResource("Messages/" + destFileName, path, true);
            File.Delete(path);
        }

        public static string GetDialog(string player1, string player2)
        {
            string destFileName = GetDialogName(player1, player2);
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + destFileName;
            disk.DownloadResource("Messages/" + destFileName, path);
            return path;
        }

        /// <summary>
        /// //////////////// Simple messeging system ////////////////////
        /// </summary>
        /// 

        public static void CreateAndSendMessage(string messageText, string sender, string receiver)
        {
            string destFileName = sender + "_" + receiver + "_mail.txt";
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + destFileName;
            File.AppendAllText(path, messageText);
            disk.UploadResource("Messages/" + destFileName, path, true);
            File.Delete(path);
        }

        public static List<string> GetAllMessagesName(string playerName)
        {
            ResInfo filesByNameFields = disk.GetResourceByName(
               1000000,
               new Media_type[]
               {
                    Media_type.Document,
                    Media_type.Text
               },
               SortField.Path,
               new ResFields[] {
                    ResFields.Media_type,
                    ResFields.Name,
                    ResFields.Path,
                    ResFields._Embedded
               },
               0, true, "120x240"
               );

            List<string> messagesNamesList = new List<String>();

            if (filesByNameFields.ErrorResponse.Message == null)
            {
                if (filesByNameFields._Embedded.Items.Count != 0)
                    foreach (var s in filesByNameFields._Embedded.Items)
                    {
                        if (s.Name.Contains(playerName + "_mail.txt"))
                        {
                            messagesNamesList.Add(s.Name);
                        }
                    }
            }
            return messagesNamesList;
        }

        public static void DeleteReadedMessage(string messageName)
        {
            disk.DeleteResource("Messages/" + messageName, false);
        }
    }
}
