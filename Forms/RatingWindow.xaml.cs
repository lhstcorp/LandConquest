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
        public List<PlayersRating> ratings { get; set; }

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
            playersCoins = new List<Player>();
            ratings = new List<PlayersRating>();
            playersCoins = playerModel.GetCoinsInfo(playersCoins, connection, user);


            for (int i = 0; i < playersCoins.Count; i++)
            {
                PlayersRating rating = new PlayersRating(playersCoins[i].PlayerId, playersCoins[i].PlayerName, Convert.ToInt32(playersCoins[i].PlayerMoney));
                ratings.Add(rating);
            }

            rankingsList.ItemsSource = ratings;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      

        private void buttonXP_Click(object sender, RoutedEventArgs e)
        {
            playersXp = new List<Player>();     
            playersXp = playerModel.GetXpInfo(playersXp, connection, user);
            ratings = new List<PlayersRating>();

            for (int i = 0; i < playersXp.Count; i++)
            {
                PlayersRating rating = new PlayersRating(playersXp[i].PlayerId, playersXp[i].PlayerName, Convert.ToInt32(playersXp[i].PlayerExp));
                ratings.Add(rating);
            }

            rankingsList.ItemsSource = ratings;
        }

        private void buttonArmy_Click(object sender, RoutedEventArgs e)
        {

            playersArmy = new List<Army>();
            playersArmy = armyModel.GetArmyInfoList(playersArmy, connection, user);
            ratings = new List<PlayersRating>();

            for (int i = 0; i < playersArmy.Count; i++)
            {
                PlayersRating rating = new PlayersRating(playersArmy[i].PlayerId, playersArmy[i].PlayerNameForArmy, Convert.ToInt32(playersArmy[i].ArmySizeCurrent));
                ratings.Add(rating);
            }

            rankingsList.ItemsSource = ratings;
        }
    }
}
