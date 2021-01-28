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
            storage = new PlayerStorage();

            itemName = null;
            itemGroup = null;
            itemSubgroup = null;

            storage = StorageModel.GetPlayerStorage(player, storage);

            labelWoodAmount.Content = storage.PlayerWood.ToString();
            labelStoneAmount.Content = storage.PlayerStone.ToString();
            labelFoodAmount.Content = storage.PlayerFood.ToString();
            labelGemsAmount.Content = storage.PlayerGems.ToString();
            labelCopperAmount.Content = storage.PlayerCopper.ToString();
            labelGoldAmount.Content = storage.PlayerGoldOre.ToString();
            labelIronAmount.Content = storage.PlayerIron.ToString();
            labelLeatherAmount.Content = storage.PlayerLeather.ToString();

            labelSetPrice.Visibility = Visibility.Hidden;
            labelSetAmount.Visibility = Visibility.Hidden;
            textBoxAmount.Visibility = Visibility.Hidden;
            textBoxPrice.Visibility = Visibility.Hidden;
            buttonPlace.Visibility = Visibility.Hidden;
        }

        private void woodButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            showListingDetails();
            itemName = "Wood";
            itemGroup = "Resources";
            itemSubgroup = "";
        }

        private void stoneButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName = "Stone";
            itemGroup = "Resources";
            itemSubgroup = "";
            showListingDetails();
        }

        private void foodButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName = "Food";
            itemGroup = "Resources";
            itemSubgroup = "";
            showListingDetails();
        }

        private void goldButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName = "Gold";
            itemGroup = "Resources";
            itemSubgroup = "";
            showListingDetails();
        }

        private void copperButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName = "Copper";
            itemGroup = "Resources";
            itemSubgroup = "";
            showListingDetails();
        }

        private void gemsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName = "Gems";
            itemGroup = "Resources";
            itemSubgroup = "";
            showListingDetails();
        }

        private void metalButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName = "Iron";
            itemGroup = "Resources";
            itemSubgroup = "";
            showListingDetails();
        }

        private void leatherButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemName = "Leather";
            itemGroup = "Resources";
            itemSubgroup = "";
            showListingDetails();
        }

        private void showListingDetails()
        {
            labelSetPrice.Visibility = Visibility.Visible;
            labelSetAmount.Visibility = Visibility.Visible;
            textBoxAmount.Visibility = Visibility.Visible;
            textBoxPrice.Visibility = Visibility.Visible;
            buttonPlace.Visibility = Visibility.Visible;
        }

        private void buttonPlace_Click(object sender, RoutedEventArgs e)
        {
            AuctionModel.AddListing(Convert.ToInt32(textBoxAmount.Text), itemName, itemGroup, itemSubgroup, Convert.ToInt32(textBoxPrice.Text), player);

        }

        private void textBoxPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
