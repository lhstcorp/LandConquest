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

namespace LandConquest.DialogWIndows
{
    public partial class EstablishStateDialog : Window
    {
        SqlConnection connection;
        Player player;
        Land land;

         //ДОБАВИТЬ On_Load метод и туда всё вынести
        LandModel landModel = new LandModel();
        CountryModel countryModel = new CountryModel();

        public EstablishStateDialog(SqlConnection _connection, Player _player, Land _land)
        {
            InitializeComponent();
            connection = _connection;
            player = _player;
            land = _land;
        }

        private void EstablishState_Click(object sender, RoutedEventArgs e)
        {
            Country country = countryModel.EstablishaState(connection, player, land, StateColor.Color);
            country.CountryId = countryModel.SelectLastIdOfStates(connection);
            landModel.UpdateLandInfo(connection, player, land, country);
        }
    }
}
