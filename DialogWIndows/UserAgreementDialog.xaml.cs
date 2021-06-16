using System.Windows;

namespace LandConquest.DialogWIndows
{
    public partial class UserAgreementDialog : Window
    {
        public UserAgreementDialog()
        {
            InitializeComponent();
            this.textBoxUserAgreement.Text = Properties.Resources.UserAgreement;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public static void ShowUserAgreement()
        {
            UserAgreementDialog dialog = new UserAgreementDialog();
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
            if (dialog.DialogResult.HasValue)
            {
                dialog.Close();
            }
        }
    }
}
