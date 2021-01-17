using LandConquestDB;
using LandConquestDB.Entities;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace LandConquest.DialogWIndows
{

    public partial class BalanceReplenishmentDialog : Window
    {
        PaymentDialog paymentDialog;
        WarningDialogWindow window;
        public double moneyAmount;
        Player player;
        SqlConnection connection;
        public BalanceReplenishmentDialog(Player _player)
        {
            player = _player;
            connection = DbContext.GetSqlConnection();
            InitializeComponent();
        }

        private void buttonPurchase_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(currencyAmount.Text) >= 100)
            {
                paymentDialog = new PaymentDialog(moneyAmount, Convert.ToInt32(currencyAmount.Text), player, this);
                paymentDialog.Show();
                this.Hide();
            }
            else
            {
                window = new WarningDialogWindow("Coins amount can not be less then 100");
                window.Show();
            }
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
