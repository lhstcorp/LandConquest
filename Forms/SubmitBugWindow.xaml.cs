using LandConquest.DialogWIndows;
using LandConquestYD;
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
            if (textBoxBugReport.Text.Length > 10)
            {
                var result = YDContext.UploadBugReport(PlayerName, textBoxBugReport.Text);
                if (result == true)
                {
                    labelPlayerName.Content = "Success!";
                    textBoxBugReport.Text = "";
                }
                else
                {
                    labelPlayerName.Content = "Error :(";
                }
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Text should not be less than 10");
            }
        }
    }
}
