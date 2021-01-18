using System;
using System.Windows;

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
    }
}
