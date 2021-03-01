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

        public static void ShowReceivedMessage(string sender, string message, int number)
        {
            MessageReceiverDialog dialog = new MessageReceiverDialog();
            dialog.labelPlayerName.Content = sender;
            dialog.TextBoxMessage.Text = message;
            dialog.LabelInboxNumber.Content = number;
            if (Application.Current.MainWindow != dialog)
            {
                dialog.Owner = Application.Current.MainWindow;
            }
            else
            {
                dialog.Topmost = true;
            }
            dialog.ShowDialog();
            if (dialog.DialogResult.HasValue)
            {
                dialog.Close();
            }
        }
    }
}
