using LandConquest.DialogWIndows;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace LandConquest.Forms
{
    public partial class AuctionWindow : Window
    {
        private Player player;
        private List<AuctionListings> listings;
        public int[] qty { get; set; }
        public string[] subject { get; set; }
        public DateTime[] setTime { get; set; }
        public string[] sellerName { get; set; }
        public int[] price { get; set; }

        public AuctionWindow(Player _player)
        {
            InitializeComponent();
            player = _player;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonCreateListing_Click(object sender, RoutedEventArgs e)
        {
            CreateListingDialog createListingDialog = new CreateListingDialog(player);
            createListingDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            createListingDialog.Owner = this;
            createListingDialog.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listings = new List<AuctionListings>();
            listings = AuctionModel.GetListings(listings);
            auctionDataGrid.ItemsSource = listings;
            buttonBuy.IsEnabled = false;
            buttonDelete.IsEnabled = false;

        }

        private void buttonFindListing_Click(object sender, RoutedEventArgs e)
        {
            Window_Loaded(sender, e);
            listings = listings.FindAll(x => x.Subject.Contains(textBoxItemSearchName.Text));
            auctionDataGrid.ItemsSource = listings;
        }

        private void buttonShowMyListings_Click(object sender, RoutedEventArgs e)
        {
            Window_Loaded(sender, e);
            listings = listings.FindAll(x => x.SellerName.Contains(player.PlayerName));
            auctionDataGrid.ItemsSource = listings;
        }

        private void buttonBuy_Click(object sender, RoutedEventArgs e)
        {
            AuctionListings listing = (AuctionListings)auctionDataGrid.SelectedItem;
            BuyListingDialog inputDialog = new BuyListingDialog();
            int itemAmount;
            if (inputDialog.ShowDialog() == true)
            {
                itemAmount = inputDialog.Amount;
                Player seller = PlayerModel.GetPlayerById(listing.SellerId);
                AuctionModel.BuyListing(itemAmount, player, seller, listing);
            }
            Window_Loaded(sender, e);

        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            AuctionListings listing = (AuctionListings)auctionDataGrid.SelectedItem;
            if (player.PlayerId == listing.SellerId)
            {
                AuctionModel.DeleteListing(listing.ListingId);
            }
            Window_Loaded(sender, e);
        }

        private void auctionDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            AuctionListings listing = (AuctionListings)auctionDataGrid.SelectedItem;
            if (listing != null)
            {
                if (player.PlayerId == listing.SellerId)
                {
                    buttonBuy.IsEnabled = false;
                    buttonDelete.IsEnabled = true;
                }
                else
                {
                    buttonBuy.IsEnabled = true;
                    buttonDelete.IsEnabled = false;
                }
            }
        }
    }
}
