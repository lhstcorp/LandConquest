using LandConquest.Entities;
using LandConquest.Models;
using System.Data.SqlClient;
using System.Windows;

namespace LandConquest.DialogWIndows
{

    public partial class EstablishStateDialog : Window
    {
        SqlConnection connection;
        Player player;
        Land land;

        LandModel landModel;
        CountryModel countryModel;

        public EstablishStateDialog(SqlConnection _connection, Player _player, Land _land)
        {
            InitializeComponent();
            connection = _connection;
            player = _player;
            land = _land;
            Loaded += Window_Loaded;
        }

        private void EstablishState_Click(object sender, RoutedEventArgs e)
        {
            Country country = countryModel.EstablishaState(connection, player, land, StateColor.Color);
            country.CountryId = countryModel.SelectLastIdOfStates(connection);
            landModel.UpdateLandInfo(connection, land, country);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            landModel = new LandModel();
            countryModel = new CountryModel();
        }
    }
}
