using LandConquest.DialogWIndows;
using LandConquest.Entities;
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
    }
}
