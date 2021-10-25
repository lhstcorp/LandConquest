using System.Windows;
namespace LandConquest.Forms
{
    public partial class MailboxWindow : Window
    {
        private string PlayerName;
        public MailboxWindow(string _playerName)
        {
            PlayerName = _playerName;
            InitializeComponent();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxMessage.Text.Length != 0)
            {
                if (TextBoxReceiver.Text != PlayerName && !LandConquestDB.Models.UserModel.CheckLoginExistence(TextBoxReceiver.Text))
                {
                    var result = LandConquestYD.YDMessaging.CreateAndSendMessage(TextBoxMessage.Text, PlayerName, TextBoxReceiver.Text);
                    if (result)
                    {
                        DialogWIndows.WarningDialogWindow.CallInfoDialogNoResult("Message was successfully sent. Note that all undelivered messages are automatically deleted at the end of each month. Please, try not to spam.");
                    }
                    else
                    {
                        DialogWIndows.WarningDialogWindow.CallWarningDialogNoResult("Error!");
                    }
                }
                else
                {
                    DialogWIndows.WarningDialogWindow.CallWarningDialogNoResult("Error sending message! Check if player with this name exists");
                }
            } else
            {
                DialogWIndows.WarningDialogWindow.CallWarningDialogNoResult("Empty message is not allowed.");
            }
        }
    }
}
