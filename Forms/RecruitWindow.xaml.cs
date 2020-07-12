using LandConquest.Entities;
using LandConquest.Models;
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

namespace LandConquest.Forms
{

    public partial class RecruitWindow : Window
    {
        SqlConnection connection;
        Player player;
        PlayerStorage storage;
        Peasants peasants = new Peasants();// Что это?

        PeasantModel peasantModel;

        PlayerModel playerModel;

        public RecruitWindow(SqlConnection _connection, Player _player, PlayerStorage _storage)
        {
            InitializeComponent();
            connection = _connection;
            player = _player;
            storage = _storage;
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            playerModel = new PlayerModel();
            peasantModel = new PeasantModel();

            peasants = peasantModel.GetPeasantsInfo(player, connection, peasants);
            TotalRecruitPeasants.Content = peasants.PeasantsCount.ToString();
            AvailableRecruitPeasants.Content = (peasants.PeasantsMax - peasants.PeasantsCount).ToString();
        }

        private void HirePeasants_Click(object sender, RoutedEventArgs e)
        {
            if ((player.PlayerMoney > Convert.ToInt32(PeasantsAmount.Text)) && (Convert.ToInt32(PeasantsAmount.Text)<Convert.ToInt32(AvailableRecruitPeasants.Content)))
            {
                peasants.PeasantsCount += Convert.ToInt32(PeasantsAmount.Text);

                peasants = peasantModel.UpdatePeasantsInfo(peasants, connection);
                TotalRecruitPeasants.Content = peasants.PeasantsCount.ToString();
                AvailableRecruitPeasants.Content = (peasants.PeasantsMax - peasants.PeasantsCount).ToString();
                player.PlayerMoney -= Convert.ToInt32(PeasantsAmount.Text);
                player = playerModel.UpdatePlayerMoney(player, connection);
                MessageBox.Show("OK");
                PeasantsAmount.Text = "0";
            }
            else
            {
                MessageBox.Show("Error");
                PeasantsAmount.Text = "0";
            }
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
