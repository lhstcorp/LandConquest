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

    public partial class WarResultWindow : Window
    {
        SqlConnection connection;
        Player player;
        ArmyModel armyModel;

        public WarResultWindow(SqlConnection _connection, Player _player)
        {
            InitializeComponent();
            connection = _connection;
            player = _player;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Army army = new Army();
            armyModel = new ArmyModel();
            army = armyModel.GetArmyInfo(connection, player, army);
            playerForces.Content = army.ArmyArchersCount + army.ArmyHorsemanCount + army.ArmyInfantryCount + army.ArmySiegegunCount;
        }
    }
}
