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
    /// <summary>
    /// Логика взаимодействия для RatingWindow.xaml
    /// </summary>
    public partial class RatingWindow : Window
    {
        SqlConnection connection;
        MainWindow window;
        Player player;
        Army army;
        User user;
        ArmyModel armyModel;
        PlayerModel playerModel;
        UserModel userModel;

        public List<Player> playersXp { get; set; }
        public List<Player> playersCoins { get; set; }
        public List<Army> playersArmy { get; set; }

        public RatingWindow(MainWindow _window, SqlConnection _connection, Player _player, User _user, Army _army)
        {
            InitializeComponent();
            window = _window;
            connection = _connection;
            player = _player;
            army = _army;
            user = _user;
            Loaded += Window_Loaded;
        }
        
    

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            playerModel = new PlayerModel();
            userModel = new UserModel();
            armyModel = new ArmyModel();

            user = new User();
            user = userModel.GetUserInfo(player.PlayerId, connection);

            player = new Player();
            player = playerModel.GetPlayerInfo(user, connection, player);

            army = new Army();
            army = armyModel.GetArmyInfo(connection, player, army);

        }

       private void buttonCoins_Click(object sender, RoutedEventArgs e)
        {
            xpRankingsList.Items.Clear();
            armyRankingsList.Items.Clear();

            playersCoins = new List<Player>();


            playersCoins = playerModel.GetCoinsInfo(playersCoins, connection, user);
            for (int i = 0; i < playersCoins.Count; i++)
            {
                coinsRankingsList.Items.Add(playersCoins[i]);
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      

        private void buttonXP_Click(object sender, RoutedEventArgs e)
        {
            coinsRankingsList.Items.Clear();
            armyRankingsList.Items.Clear();

            playersXp = new List<Player>();
            
           
            playersXp = playerModel.GetXpInfo(playersXp, connection, user);
            for (int i = 0; i < playersXp.Count; i++)
            {
                xpRankingsList.Items.Add(playersXp[i]);
            }

        }

        private void buttonArmy_Click(object sender, RoutedEventArgs e)
        {
            coinsRankingsList.Items.Clear();
            xpRankingsList.Items.Clear();

            playersArmy = new List<Army>();


            playersArmy = armyModel.GetArmyInfoList(playersArmy, connection, user);
            for (int i = 0; i < playersArmy.Count; i++)
            {
                armyRankingsList.Items.Add(playersArmy[i]);
            }
        }
    }
}
