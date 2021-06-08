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
    /// <summary>
    /// Логика взаимодействия для PaymentDialog.xaml
    /// </summary>
    public partial class PaymentDialog : Window
    {
        private int goldAmount;
        public PaymentDialog(int _goldAmount)
        {
            InitializeComponent();
            goldAmount = _goldAmount;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            moneyAmountLabel.Text = (goldAmount / 100).ToString();
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
