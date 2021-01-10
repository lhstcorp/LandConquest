using LandConquest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            if(Convert.ToInt32(textBoxAmount.Text) > 0)
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
