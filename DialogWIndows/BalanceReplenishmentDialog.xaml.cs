using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

    public partial class BalanceReplenishmentDialog : Window
    {
        PaymentDialog paymentDialog;
        public double moneyAmount;
        Player player;
        SqlConnection connection;
        public BalanceReplenishmentDialog(Player _player, SqlConnection _connection)
        {
            player = _player;
            connection = _connection;
            InitializeComponent();
        }

        private void buttonPurchase_Click(object sender, RoutedEventArgs e)
        {
            paymentDialog = new PaymentDialog(connection, moneyAmount, Convert.ToInt32(currencyAmount.Text), player, this);
            paymentDialog.Show();
            this.Hide();
        }

        private void currencyAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currencyAmount.Text != "0" && currencyAmount.Text != null && currencyAmount.CaretIndex != 0)
            {
                totalMoneyAmountLabel.Content = Convert.ToDouble(currencyAmount.Text) / 100;
                moneyAmount = Convert.ToDouble(currencyAmount.Text) / 100;
            }
        }
    }
}
