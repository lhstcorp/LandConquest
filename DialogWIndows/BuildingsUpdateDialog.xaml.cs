using LandConquestDB.Entities;
using LandConquestDB.Models;
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

namespace LandConquest.DialogWIndows
{
    /// <summary>
    /// Логика взаимодействия для BuildingsUpdateDialog.xaml
    /// </summary>
    public partial class BuildingsUpdateDialog : Window
    {
        Land land;
        Buildings buildings;
        LandStorage landStorage;
        PlayerStorage resourcesNeed;
        Player player;
        private enum Level : int
        {
            Wood = 400,
            Stone = 750
            
        }
        public BuildingsUpdateDialog(Player _player, Land _land)
        {
            player = _player;
            land = _land;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            buildings = BuildingsModel.GetPlayerBuildings(player.PlayerId, land.LandId);

            ManufactureLvl.Content = buildings.Houses;

            resourcesNeed = new PlayerStorage();
            resourcesNeed.Wood = Convert.ToInt32(Math.Pow((int)Level.Wood * buildings.Houses, 1.25));
            resourcesNeed.Stone = Convert.ToInt32(Math.Pow((int)Level.Stone * buildings.Houses, 1.25));
           

            WoodNeed.Content = resourcesNeed.Wood;
            StoneNeed.Content = resourcesNeed.Stone;
            

            landStorage = new LandStorage();

            landStorage = LandStorageModel.GetLandStorage(land, landStorage);

            WoodHave.Content = landStorage.Wood.ToString();
            StoneHave.Content = landStorage.Stone.ToString();

        }
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            landStorage.Wood -= resourcesNeed.Wood;
            landStorage.Stone -= resourcesNeed.Stone;
            

            if (landStorage.Wood >= 0
             && landStorage.Stone >= 0)
            {
                LandStorageModel.UpdateLandStorage(land, landStorage);

                buildings.Houses++;

               

                BuildingsModel.UpdateBuildings(buildings);
                this.Window_Loaded(this, new RoutedEventArgs());
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
            }

        }
    }
}
