using System.Windows;

namespace LandConquest.DialogWIndows
{
    public partial class WarningDialogWindow : Window
    {
        private WarningDialogWindow(string _textWarning)
        {
            InitializeComponent();
            this.textWarning.Text = _textWarning;
        }

        private void warningButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public static void CallWarningDialogNoResult(string message)
        {
            WarningDialogWindow warningWindow = new WarningDialogWindow(message);
            warningWindow.Owner = Application.Current.MainWindow;
            warningWindow.ShowDialog();
            if (warningWindow.DialogResult.HasValue)
            {
                warningWindow.Close();
            }
        }

        public static bool CallWarningDialogWithResult(string message)
        {
            WarningDialogWindow warningWindow = new WarningDialogWindow(message);
            warningWindow.Owner = Application.Current.MainWindow;
            warningWindow.ShowDialog();
            if (warningWindow.DialogResult == true)
            {
                warningWindow.Close();
                return true;
            }
            else
            {
                warningWindow.Close();
                return false;
            }
        }
    }
}
