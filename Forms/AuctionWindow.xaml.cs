using LandConquestDB.Entities;
using System.Windows;

namespace LandConquest.Forms
{
    public partial class AuctionWindow : Window
    {
        public AuctionWindow(Player _player)
        {
            InitializeComponent();
            DataContext = new AuctionWindowViewModel(_player);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonCreateListing_Click(object sender, RoutedEventArgs e)
        {
            ((AuctionWindowViewModel)DataContext).CreateListing();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((AuctionWindowViewModel)DataContext).WindowLoaded();
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
            ((AuctionWindowViewModel)DataContext).DataGridSelectionChanged();
        }

        private void buttonUpdateListings_Click(object sender, RoutedEventArgs e)
        {
            ((AuctionWindowViewModel)DataContext).UpdateListings();
        }
    }
}
