using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LandConquest.DialogWIndows
{

    public partial class CreateListingDialog : Window
    {
        private Player player;
        private PlayerStorage storage;

        private string itemName;
        private string itemGroup;
        private string itemSubgroup;

        public CreateListingDialog(Player _player)
        {
            InitializeComponent();
            player = _player;
            Loaded += Window_Loaded;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            storage         = new PlayerStorage();

            itemName        = null;
            itemGroup       = null;
            itemSubgroup    = null;

            storage = StorageModel.GetPlayerStorage(player);

            labelWoodAmount.Content     = storage.PlayerWood.ToString();
            labelStoneAmount.Content    = storage.PlayerStone.ToString();
            labelFoodAmount.Content     = storage.PlayerFood.ToString();
            labelGemsAmount.Content     = storage.PlayerGems.ToString();
            labelCopperAmount.Content   = storage.PlayerCopper.ToString();
            labelIronAmount.Content     = storage.PlayerIron.ToString();
            labelLeatherAmount.Content  = storage.PlayerLeather.ToString();

            labelSetPrice.Visibility    = Visibility.Hidden;
            labelSetAmount.Visibility   = Visibility.Hidden;
            textBoxAmount.Visibility    = Visibility.Hidden;
            textBoxPrice.Visibility     = Visibility.Hidden;
            buttonPlace.Visibility      = Visibility.Hidden;
        }

        private void woodButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            showListingDetails();
            itemName        = "wood";
            itemGroup       = "Resources";
            itemSubgroup    = "";
        }

        private void stoneButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName        = "stone";
            itemGroup       = "Resources";
            itemSubgroup    = "";
            showListingDetails();
        }

        private void foodButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName        = "food";
            itemGroup       = "Resources";
            itemSubgroup    = "";
            showListingDetails();
        }
        private void copperButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName        = "copper";
            itemGroup       = "Resources";
            itemSubgroup    = "";
            showListingDetails();
        }

        private void gemsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName        = "gems";
            itemGroup       = "Resources";
            itemSubgroup    = "";
            showListingDetails();
        }

        private void metalButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName        = "iron";
            itemGroup       = "Resources";
            itemSubgroup    = "";
            showListingDetails();
        }

        private void leatherButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName        = "leather";
            itemGroup       = "Resources";
            itemSubgroup    = "";
            showListingDetails();
        }

        private void showListingDetails()
        {
            labelSetPrice.Visibility    = Visibility.Visible;
            labelSetAmount.Visibility   = Visibility.Visible;
            textBoxAmount.Visibility    = Visibility.Visible;
            textBoxPrice.Visibility     = Visibility.Visible;
            buttonPlace.Visibility      = Visibility.Visible;
        }

        private void buttonPlace_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(textBoxAmount.Text) > 0 && Convert.ToInt32(textBoxPrice.Text) > 0)
            {
                int playersResourceAmount = PlayerModel.GetPlayerResourceAmount(player, itemName);
                if (playersResourceAmount >= Convert.ToInt32(textBoxAmount.Text) && playersResourceAmount > 0)
                {
                    AuctionModel.AddListing(Convert.ToInt32(textBoxAmount.Text), itemName, itemGroup, itemSubgroup, Convert.ToInt32(textBoxPrice.Text), player);
                    WarningDialogWindow.CallWarningDialogNoResult("Listing was successfully placed and will expire after 7 days.");
                }
                else
                {
                    WarningDialogWindow.CallWarningDialogNoResult("You haven't got enough resources!");
                }
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Value should be more than 0!");
            }

        }

        private void textBoxPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        public static bool IsValid(string str)
        {
            return int.TryParse(str, out int i) && i >= 1 && i <= 99999;
        }
    }
}
