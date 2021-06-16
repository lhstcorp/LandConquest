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
            if (Application.Current.MainWindow != warningWindow)
            {
                warningWindow.Owner = Application.Current.MainWindow;
            } else
            {
                warningWindow.Topmost = true;
            }
            warningWindow.ShowDialog();
            if (warningWindow.DialogResult.HasValue)
            {
                warningWindow.Close();
            }
        }

        public static bool CallWarningDialogWithResult(string message)
        {
            WarningDialogWindow warningWindow = new WarningDialogWindow(message);
            if (Application.Current.MainWindow != warningWindow)
            {
                warningWindow.Owner = Application.Current.MainWindow;
            }
            else
            {
                warningWindow.Topmost = true;
            }
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

        public static void CallInfoDialogNoResult(string message)
        {
            WarningDialogWindow warningWindow = new WarningDialogWindow(message);
            if (Application.Current.MainWindow != warningWindow)
            {
                warningWindow.Owner = Application.Current.MainWindow;
            }
            else
            {
                warningWindow.Topmost = true;
            }
            warningWindow.warningHeader.Visibility = Visibility.Hidden;
            warningWindow.ShowDialog();
            if (warningWindow.DialogResult.HasValue)
            {
                warningWindow.Close();
            }
        }

        public static void CallInfoDialogNoData(string message)
        {
            WarningDialogWindow warningWindow = new WarningDialogWindow(message);
            if (message == "")
            {
                if (Application.Current.MainWindow != warningWindow)
                {
                    warningWindow.Owner = Application.Current.MainWindow;
                }
                else
                {
                    warningWindow.Topmost = true;
                }
                warningWindow.warningHeader.Visibility = Visibility.Hidden;
                warningWindow.ShowDialog();
                if (warningWindow.DialogResult.HasValue)
                {
                    warningWindow.Close();
                }
            }
        }
    }
}
