using LandConquest.DialogWIndows;
using System;
using System.Diagnostics;
using System.Windows;

namespace LandConquest.Forms
{
    public partial class MembershipWindow : Window
    {
        public bool amountNotNull;
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
            goldAmount.Visibility = Visibility.Hidden;
            buttonBuyGold.Visibility = Visibility.Hidden;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonBuyGold_Click(object sender, RoutedEventArgs e)
        {
            //if (amountNotNull)
            //{
            //    PaymentDialog dialog = new PaymentDialog(Convert.ToInt32(goldAmount.Text));
            //    dialog.Show();

                var psi = new ProcessStartInfo
                {
                    FileName = "https://payeer.com/",
                    UseShellExecute = true
                };
                Process.Start(psi);
            //}
        }

        private void goldAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if(goldAmount.Text != "" && int.TryParse(goldAmount.Text, out int number))
            {
                amountNotNull = true;
                buttonBuyGold.Content = "Buy";
            }
            else
            {
                amountNotNull = false;
                buttonBuyGold.Content = "";
            }
        }
    }
}
