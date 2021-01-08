using System.Diagnostics;
using System.Windows;

namespace LandConquest.Forms
{
    public partial class MembershipWindow : Window
    {
        public MembershipWindow()
        {
            InitializeComponent();
        }

        private void buttonBecomeMember_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://www.donationalerts.com/r/landconquest",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
