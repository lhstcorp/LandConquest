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

namespace LandConquest.Forms
{
    /// <summary>
    /// Логика взаимодействия для LandResourcesWindow.xaml
    /// </summary>
    public partial class LandResourcesWindow : Window
    {
        private Player player;
        private PlayerStorage storage;
        private LandStorage landStorage;
        private Land land;

        public LandResourcesWindow(Player _player, PlayerStorage _storage, LandStorage _landstorage, Land _land)
        {
            InitializeComponent();
            player = _player;
            landStorage = _landstorage;
            land = _land;
            Loaded += LandResourcesWindow_Loaded;
        }

        private void LandResourcesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            storage = new PlayerStorage();
            landStorage = new LandStorage();

            storage = StorageModel.GetPlayerStorage(player);
            landStorage = LandStorageModel.GetLandStorage(land, landStorage);

            labelWoodAmount.Content = landStorage.LandWood.ToString();
            labelCopperAmount.Content = landStorage.LandCopper.ToString();
            labelFoodAmount.Content = landStorage.LandFood.ToString();
            labelGemsAmount.Content = landStorage.LandGems.ToString();
            labelGoldAmount.Content = landStorage.LandGoldOre.ToString();
            labelIronAmount.Content = landStorage.LandIron.ToString();
            labelLeatherAmount.Content = landStorage.LandLeather.ToString();
            labelStoneAmount.Content = landStorage.LandStone.ToString();



        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DonateAllButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch(button.Name)
            {
                case "DonateWoodButton":
                    ////
                    break;
                case "DonateFoodButton":
                    ////
                    break;
                case "DonateIronButton":
                    ////
                    break;
                case "DonateStoneButton":
                    ////
                    break;
                case "DonateLeatherButton":
                    ////
                    break;
                case "DonateGoldButton":
                    ////
                    break;
                case "DonateGemsButton":
                    ////
                    break;
                case "DonateCopperButton":
                    ////
                    break;

            }
        }

        private void FoodToBuyTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }
        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 1 && i <= 99999;
        }
    }
}
