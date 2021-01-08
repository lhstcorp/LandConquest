using LandConquest.Entities;
using LandConquest.Models;
using System.Windows;

namespace LandConquest.DialogWIndows
{
    public partial class PaymentDialog : Window
    {
        Player player;
        public double moneyAmount;
        public int gameCurrencyAmount;
        BalanceReplenishmentDialog dialog;

        public PaymentDialog(double _moneyAmount, int _gameCurrencyAmount, Player _player, BalanceReplenishmentDialog _dialog)
        {
            moneyAmount = _moneyAmount;
            gameCurrencyAmount = _gameCurrencyAmount;
            dialog = _dialog;
            player = _player;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            moneyAmountLabel.Text = "$" + moneyAmount;
            paymentResult.Text = "";
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            dialog.Show();
            this.Close();
        }

        private void payButton_Click(object sender, RoutedEventArgs e)
        {
            player.PlayerDonation = player.PlayerDonation + gameCurrencyAmount;
            PlayerModel playerModel = new PlayerModel();
            var result = PlayerModel.UpdatePlayerDonationMoney(player);
            if (result != null)
            {
                paymentResult.Text = "Payment successfull!";
            }
            else
            {
                paymentResult.Text = "Something went wrong(";
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
