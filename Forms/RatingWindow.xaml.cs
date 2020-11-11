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
        User user;

        PlayerModel playerModel;
        UserModel userModel;

        public List<Player> playersXp { get; set; }

        public RatingWindow(MainWindow _window, SqlConnection _connection, Player _player, User _user)
        {
            InitializeComponent();
            window = _window;
            connection = _connection;
            player = _player;
            user = _user;
            Loaded += Window_Loaded;
        }
        
    

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            playerModel = new PlayerModel();
            userModel = new UserModel();

            user = new User();
            user = userModel.GetUserInfo(player.PlayerId, connection);

            player = new Player();
            player = playerModel.GetPlayerInfo(user, connection, player);

        }

       private void buttonCoins_Click(object sender, RoutedEventArgs e)
        {
            Player player = new Player();
            
            player = playerModel.PlayerRatingCoins(player, connection, user);

            Top1.Content = player.PlayerMoney.ToString();
            
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      

        private void buttonXP_Click(object sender, RoutedEventArgs e)
        {
            playersXp = new List<Player>();
            Player player = new Player();
            player = playerModel.GetXpInfo(player, connection, user);

            player.PlayerName = player.PlayerName.ToString();
            player.PlayerExp = player.PlayerExp;


            //Player player1 = new Player();
            //player1.PlayerName = player1.PlayerName.ToString();
            //player1.PlayerExp = 123;
            
           
            //Top1.Content = player.PlayerExp.ToString();
            //name1.Content = player.PlayerName.ToString();

            playersXp.Add(player);
            //playersXp.Add(player1);
            DataContext = this;
        }
    }
}
