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
            if (!LandConquestDB.Models.UserModel.ValidateUserByLogin(TextBoxReceiver.Text))
            {
                LandConquestYD.YDMessaging.CreateAndSendMessage(TextBoxMessage.Text, PlayerName, TextBoxReceiver.Text);
            }
        }
    }
}
