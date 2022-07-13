using LandConquest.DialogWIndows;
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
        private User user;
        private PlayerStorage storage;
        private LandStorage landStorage;
        private Land land;

        public LandResourcesWindow(User _user, Player _player, PlayerStorage _storage, LandStorage _landstorage, Land _land)
        {
            InitializeComponent();
            player = _player;
            user = _user;
            storage = _storage;
            landStorage = _landstorage;
            land = _land;
            Loaded += LandResourcesWindow_Loaded;
        }

        private void LandResourcesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //storage = new PlayerStorage();
            //landStorage = new LandStorage();
            //player = new Player();
            //user = new User();
            //user = UserModel.GetUserInfo(user.UserId);
            //player = PlayerModel.GetPlayerInfo(user, player);
            //storage = StorageModel.GetPlayerStorage(player);
            //landStorage = LandStorageModel.GetLandStorage(land, landStorage);

            labelWoodAmount.Content = landStorage.Wood.ToString();
            labelCopperAmount.Content = landStorage.Copper.ToString();
            labelFoodAmount.Content = landStorage.Food.ToString();
            labelGemsAmount.Content = landStorage.Gems.ToString();
            labelGoldAmount.Content = landStorage.GoldOre.ToString();
            labelIronAmount.Content = landStorage.Iron.ToString();
            labelLeatherAmount.Content = landStorage.Leather.ToString();
            labelStoneAmount.Content = landStorage.Stone.ToString();
            labelMoneyAmount.Content = landStorage.Money.ToString();



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
                    if (storage.Wood >= Convert.ToInt32(DonateWoodTextBox.Text))
                    {
                        landStorage.Wood += Convert.ToInt32(DonateWoodTextBox.Text);
                        storage.Wood -= Convert.ToInt32(DonateWoodTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }

                    break;
                case "DonateFoodButton":
                    if (storage.Food >= Convert.ToInt32(DonateFoodTextBox.Text))
                    {
                        landStorage.Food += Convert.ToInt32(DonateFoodTextBox.Text);
                        storage.Food -= Convert.ToInt32(DonateFoodTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateIronButton":
                    if (storage.Iron >= Convert.ToInt32(DonateIronTextBox.Text))
                    {
                        landStorage.Iron += Convert.ToInt32(DonateIronTextBox.Text);
                        storage.Iron -= Convert.ToInt32(DonateIronTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateStoneButton":
                    if (storage.Stone >= Convert.ToInt32(DonateStoneTextBox.Text))
                    {
                        landStorage.Stone += Convert.ToInt32(DonateStoneTextBox.Text);
                        storage.Stone -= Convert.ToInt32(DonateStoneTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateLeatherButton":
                    if (storage.Leather >= Convert.ToInt32(DonateLeatherTextBox.Text))
                    {
                        landStorage.Leather += Convert.ToInt32(DonateLeatherTextBox.Text);
                        storage.Leather -= Convert.ToInt32(DonateLeatherTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateGoldButton":
                    if (storage.GoldOre >= Convert.ToInt32(DonateGoldTextBox.Text))
                    {
                        landStorage.GoldOre += Convert.ToInt32(DonateGoldTextBox.Text);
                        storage.GoldOre -= Convert.ToInt32(DonateGoldTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateGemsButton":
                    if (storage.Gems >= Convert.ToInt32(DonateGemsTextBox.Text))
                    {
                        landStorage.Gems += Convert.ToInt32(DonateGemsTextBox.Text);
                        storage.Gems -= Convert.ToInt32(DonateGemsTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateCopperButton":
                    if (storage.Copper >= Convert.ToInt32(DonateCopperTextBox.Text))
                    {
                        landStorage.Copper += Convert.ToInt32(DonateCopperTextBox.Text);
                        storage.Copper -= Convert.ToInt32(DonateCopperTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateMoneyButton":
                    if (player.PlayerMoney >= Convert.ToInt32(DonateMoneyTextBox.Text))
                    {
                        landStorage.Money += Convert.ToInt32(DonateMoneyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(DonateMoneyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;

            }
            StorageModel.UpdateStorage(player, storage);
            PlayerModel.UpdatePlayerMoney(player);
            LandStorageModel.UpdateLandStorage(land, landStorage);
            LandResourcesWindow_Loaded(sender, e);
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

        private void Space_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
