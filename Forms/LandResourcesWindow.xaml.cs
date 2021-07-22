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

            labelWoodAmount.Content = landStorage.LandWood.ToString();
            labelCopperAmount.Content = landStorage.LandCopper.ToString();
            labelFoodAmount.Content = landStorage.LandFood.ToString();
            labelGemsAmount.Content = landStorage.LandGems.ToString();
            labelGoldAmount.Content = landStorage.LandGoldOre.ToString();
            labelIronAmount.Content = landStorage.LandIron.ToString();
            labelLeatherAmount.Content = landStorage.LandLeather.ToString();
            labelStoneAmount.Content = landStorage.LandStone.ToString();
            labelMoneyAmount.Content = landStorage.LandMoney.ToString();



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
                    if (storage.PlayerWood >= Convert.ToInt32(DonateWoodTextBox.Text))
                    {
                        landStorage.LandWood += Convert.ToInt32(DonateWoodTextBox.Text);
                        storage.PlayerWood -= Convert.ToInt32(DonateWoodTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }

                    break;
                case "DonateFoodButton":
                    if (storage.PlayerFood >= Convert.ToInt32(DonateFoodTextBox.Text))
                    {
                        landStorage.LandFood += Convert.ToInt32(DonateFoodTextBox.Text);
                        storage.PlayerFood -= Convert.ToInt32(DonateFoodTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateIronButton":
                    if (storage.PlayerIron >= Convert.ToInt32(DonateIronTextBox.Text))
                    {
                        landStorage.LandIron += Convert.ToInt32(DonateIronTextBox.Text);
                        storage.PlayerIron -= Convert.ToInt32(DonateIronTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateStoneButton":
                    if (storage.PlayerStone >= Convert.ToInt32(DonateStoneTextBox.Text))
                    {
                        landStorage.LandStone += Convert.ToInt32(DonateStoneTextBox.Text);
                        storage.PlayerStone -= Convert.ToInt32(DonateStoneTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateLeatherButton":
                    if (storage.PlayerLeather >= Convert.ToInt32(DonateLeatherTextBox.Text))
                    {
                        landStorage.LandLeather += Convert.ToInt32(DonateLeatherTextBox.Text);
                        storage.PlayerLeather -= Convert.ToInt32(DonateLeatherTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateGoldButton":
                    if (storage.PlayerGoldOre >= Convert.ToInt32(DonateGoldTextBox.Text))
                    {
                        landStorage.LandGoldOre += Convert.ToInt32(DonateGoldTextBox.Text);
                        storage.PlayerGoldOre -= Convert.ToInt32(DonateGoldTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateGemsButton":
                    if (storage.PlayerGems >= Convert.ToInt32(DonateGemsTextBox.Text))
                    {
                        landStorage.LandGems += Convert.ToInt32(DonateGemsTextBox.Text);
                        storage.PlayerGems -= Convert.ToInt32(DonateGemsTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateCopperButton":
                    if (storage.PlayerCopper >= Convert.ToInt32(DonateCopperTextBox.Text))
                    {
                        landStorage.LandCopper += Convert.ToInt32(DonateCopperTextBox.Text);
                        storage.PlayerCopper -= Convert.ToInt32(DonateCopperTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;
                case "DonateMoneyButton":
                    if (player.PlayerMoney >= Convert.ToInt32(DonateMoneyTextBox.Text))
                    {
                        landStorage.LandMoney += Convert.ToInt32(DonateMoneyTextBox.Text);
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
    }
}
