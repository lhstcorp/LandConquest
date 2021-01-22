using LandConquestDB;
using Syroot.Windows.IO;
using System;
using System.IO;
using System.Windows;

namespace LandConquest.Forms
{
    public partial class SubmitBugWindow : Window
    {
        private string PlayerName;
        public SubmitBugWindow(string _playerName)
        {
            PlayerName = _playerName;
            InitializeComponent();
            labelPlayerName.Content = _playerName;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonSubmitBug_Click(object sender, RoutedEventArgs e)
        {
            var disk = YDContext.GetYD();
            string destFileName = @"BugReport_" + PlayerName + DateTime.UtcNow.ToString().Replace(":", "_") + @".txt";
            string path = new KnownFolder(KnownFolderType.Downloads).Path + @"\" + destFileName;
            File.AppendAllText(path, textBoxBugReport.Text);
            var result = disk.UploadResource("SubBugs/" + destFileName, path, true);
            if (result.Error == null)
            {
                labelPlayerName.Content = "Success!";
                textBoxBugReport.Text = "";
                File.Delete(path);
            }
            else
            {
                labelPlayerName.Content = "Error :(";
                File.Delete(path);
            }
        }
    }
}
