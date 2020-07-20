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
    /// Логика взаимодействия для CountryWindow.xaml
    /// </summary>
    public partial class CountryWindow : Window
    {
        SqlConnection connection;
        Player player;
        CountryModel countryModel;
        PlayerModel playerModel;
        public CountryWindow(SqlConnection _connection, Player _player)
        {
            connection = _connection;
            player = _player;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            countryModel = new CountryModel();
            playerModel = new PlayerModel();
            Country country = countryModel.GetCountryById(connection, countryModel.GetCountryId(connection, player));
            Player ruler = new Player();
            User rulerUser = new User();
            rulerUser.UserId = country.CountryRuler;
            ruler = playerModel.GetPlayerInfo(rulerUser, connection, ruler);
            RulerNameLbl.Content = ruler.PlayerName;

            CountryNameLbl.Content = country.CountryName;

            //Label lab1 = new Label();
            //lab1.Margin = new Thickness(100, 100, 15, 15);
            //lab1.Content = "govno";
            //lab1.FontSize = 50;
            //MainGrid.Children.Add(lab1);
        }
    }
}
