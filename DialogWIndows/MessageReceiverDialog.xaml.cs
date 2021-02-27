using System.Windows;


namespace LandConquest.DialogWIndows
{
    public partial class MessageReceiverDialog : Window
    {
        public MessageReceiverDialog()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public static void ShowReceivedMessage(string sender, string message)
        {
            MessageReceiverDialog dialog = new MessageReceiverDialog();
            dialog.labelPlayerName.Content = sender;
            dialog.TextBoxMessage.Text = message;
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
            if (dialog.DialogResult.HasValue)
            {
                dialog.Close();
            }
        }
    }
}
