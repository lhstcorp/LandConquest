using LandConquestDB.Entities;
using LandConquestDB.Models;
using System.Windows;

namespace LandConquest.Forms
{
    /// <summary>
    /// Логика взаимодействия для CoffersWindow.xaml
    /// </summary>
    public partial class CoffersWindow : Window
    {
        Player player;
        //List<Land> countryLands;
        //List<Land> countryLandsToFight;
        //List<Country> countries;
        //Land selectedLand;
        //Country transferCountry;
        //Land countryLandDefender;
        //int operation = 0;
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
