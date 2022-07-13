using System;
using System.Windows;
using System.Windows.Controls;

namespace LandConquest.DialogWIndows
{
    public partial class BuyListingDialog : Window
    {
        public BuyListingDialog()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonBuy_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(textBoxAmount.Text) > 0)
            {
                this.DialogResult = true;
            }

        }
        public int Amount
        {
            get { return Convert.ToInt32(textBoxAmount.Text); }
        }

        private void textBoxAmount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 1 && i <= 99999;
        }
        private void Space_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
