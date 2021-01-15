using System.Windows;

namespace LandConquest.DialogWIndows
{
    public partial class WarningDialogWindow : Window
    {
        public WarningDialogWindow(string _textWarning)
        {
            InitializeComponent();
            this.textWarning.Text = _textWarning;
        }

        private void warningButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
