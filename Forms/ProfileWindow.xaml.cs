using LandConquest.Entities;
using LandConquest.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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

    public partial class ProfileWindow : Window
    {
        SqlConnection connection;
        MainWindow window;
        Player player;
        User user;

        PlayerModel playerModel;
        UserModel userModel;

        public ProfileWindow(MainWindow _window, SqlConnection _connection, Player _player, User _user)
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

            
            labelName.Content = player.PlayerName.ToString();
            labelTitle.Content = player.PlayerTitle.ToString();
            labelLand.Content = player.PlayerCurrentRegion.ToString();

            labelEmail.Content = user.UserEmail.ToString();
            labelLogin.Content = user.UserLogin.ToString();
            

        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonChangeName_Click(object sender, RoutedEventArgs e)
        {
            playerModel.UpdatePlayerName(connection, player.PlayerId, newNameBox.Text);
            player.PlayerName = newNameBox.Text;
            this.Loaded += Window_Loaded;
            newNameBox.Visibility = Visibility.Hidden;
        }

        private void buttonChangeEmail_Click(object sender, RoutedEventArgs e)
        {
            userModel.UpdateUserEmail(connection, user.UserId, newEmailBox.Text);
            this.Loaded += Window_Loaded;
            newEmailBox.Visibility = Visibility.Hidden;
        }

        private void buttonChangePass_Click(object sender, RoutedEventArgs e)
        {
            userModel.UpdateUserPass(connection, user.UserId, newPassBox.Text);
            this.Loaded += Window_Loaded;
            newPassBox.Visibility = Visibility.Hidden;
        }

        private void buttonChangePass_MouseEnter(object sender, MouseEventArgs e)
        {
            newPassBox.Visibility = Visibility.Visible;
        }

        private void buttonChangeEmail_MouseEnter(object sender, MouseEventArgs e)
        {
            newEmailBox.Visibility = Visibility.Visible;
        }

        private void buttonChangeName_MouseEnter(object sender, MouseEventArgs e)
        {
            newNameBox.Visibility = Visibility.Visible;
        }
    }
}
