using LandConquestDB.Entities;
using LandConquestDB.Models;
using System.Windows;

namespace LandConquest.Forms
{

    public partial class WarResultWindow : Window
    {
        Player player;

        public WarResultWindow(Player _player)
        {
            InitializeComponent();
            player = _player;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Army army = new Army();
            army = ArmyModel.GetArmyInfo(player, army);
            playerForces.Content = army.ArmyArchersCount + army.ArmyHorsemanCount + army.ArmyInfantryCount + army.ArmySiegegunCount;
        }

    }
}
