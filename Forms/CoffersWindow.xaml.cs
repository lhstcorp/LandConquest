using LandConquest.Entities;
using LandConquest.Models;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для CoffersWindow.xaml
    /// </summary>
    public partial class CoffersWindow : Window
    {
        Player player;
        List<Land> countryLands;
        List<Land> countryLandsToFight;
        List<Country> countries;
        Land selectedLand;
        Country transferCountry;
        Land countryLandDefender;
        int operation = 0;
        bool f = true;
        public CoffersWindow(Player _player)
        {
            player = _player;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            Country country = CountryModel.GetCountryById(CountryModel.GetCountryId(player));
            Player ruler = new Player();
            User rulerUser = new User();
            rulerUser.UserId = country.CountryRuler;
            ruler = PlayerModel.GetPlayerInfo(rulerUser, ruler);
            labelCountryName.Content = country.CountryName;
            labelMoney.Content = country.CountryCoffers;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveTaxesOnLive_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
