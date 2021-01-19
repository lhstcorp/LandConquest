using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace LandConquest.Forms
{
    public partial class RatingWindow : Window
    {
        private MainWindow window;
        private Player player;
        private Army army;
        private User user;

        public List<Player> playersXp { get; set; }
        public List<Player> playersCoins { get; set; }
        public List<Army> playersArmy { get; set; }
        public List<PlayersRating> ratings { get; set; }

        public RatingWindow(MainWindow _window, Player _player, User _user, Army _army)
        {
            InitializeComponent();
            window = _window;
            player = _player;
            army = _army;
            user = _user;
            Loaded += Window_Loaded;
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            user = new User();
            user = UserModel.GetUserInfo(player.PlayerId);

            player = new Player();
            player = PlayerModel.GetPlayerInfo(user, player);

            army = new Army();
            army = ArmyModel.GetArmyInfo(player, army);

        }

        private void buttonCoins_Click(object sender, RoutedEventArgs e)
        {
            playersCoins = new List<Player>();
            ratings = new List<PlayersRating>();
            playersCoins = PlayerModel.GetCoinsInfo(playersCoins, user);


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
            playersXp = PlayerModel.GetXpInfo(playersXp, user);
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
            playersArmy = ArmyModel.GetArmyInfoList(playersArmy, user);
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
