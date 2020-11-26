using LandConquest.DialogWIndows;
using LandConquest.Entities;
using LandConquest.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

    public partial class AuctionWindow : Window
    {
        SqlConnection connection;
        Player player;
        AuctionModel auctionModel;
        List<AuctionListings> listings;
        public int[] qty { get; set; }
        public string[] subject { get; set; }
        public DateTime[] setTime { get; set; }
        public string[] sellerName { get; set; }
        public int[] price { get; set; }



        public AuctionWindow(SqlConnection _connection, Player _player)
        {
            InitializeComponent();
            connection = _connection;
            player = _player;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonCreateListing_Click(object sender, RoutedEventArgs e)
        {
            CreateListingDialog createListingDialog = new CreateListingDialog(connection, player);
            createListingDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            createListingDialog.Owner = this;
            createListingDialog.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            auctionModel = new AuctionModel();
            listings = new List<AuctionListings>();
            listings = auctionModel.GetListings(listings, connection);
            auctionDataGrid.ItemsSource = listings;

            //for (int i = 0; i < listings.Count; i++)
            //{
            //    listings.Items.Add(playersXp[i]);
            //    listings.
            //    qty[i] = listings[i].Qty;
            //    subject[i] = listings[i].Subject;
            //    setTime[i] = listings[i].ListingSetTime;
            //    sellerName[i] = listings[i].SellerName;
            //    price[i] = listings[i].Price;
            //}
        }

        private void buttonFindListing_Click(object sender, RoutedEventArgs e)
        {
           
            listings = auctionModel.FindListings(listings, connection);
           
        }
    }
}
