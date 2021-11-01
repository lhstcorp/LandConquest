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
        public AuctionWindow(Player _player)
        {
            InitializeComponent();
            DataContext = new AuctionWindowViewModel(_player);
            player = _player;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonCreateListing_Click(object sender, RoutedEventArgs e)
        {
            CreateListingDialog createListingDialog = new CreateListingDialog(player);
            createListingDialog.Owner = Application.Current.MainWindow;
            createListingDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            createListingDialog.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((AuctionWindowViewModel)DataContext).AuctionWindowLoaded();
        }

        private void buttonFindListing_Click(object sender, RoutedEventArgs e)
        {
            ((AuctionWindowViewModel)DataContext).FindListing();
        }

        private void buttonShowMyListings_Click(object sender, RoutedEventArgs e)
        {
            ((AuctionWindowViewModel)DataContext).ShowMyListings();
        }

        private void buttonBuy_Click(object sender, RoutedEventArgs e)
        {
            ((AuctionWindowViewModel)DataContext).Buy();

        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            ((AuctionWindowViewModel)DataContext).ButtonDeleteClick();
        }

        private void auctionDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ((AuctionWindowViewModel)DataContext).AuctionDataGridSelectionChanged();
        }

        private void buttonUpdateListings_Click(object sender, RoutedEventArgs e)
        {
            ((AuctionWindowViewModel)DataContext).UpdateListings();
        }
    }
}
