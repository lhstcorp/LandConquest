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

    public partial class StorageWindow : Window
    {
        SqlConnection connection;
        MainWindow window;
        Player player;
        User user;
        public StorageWindow(MainWindow _window,SqlConnection _connection, Player _player, User _user)
        {
            InitializeComponent();
            window = _window; 
            connection = _connection;
            player = _player;
            user = _user;
            Loaded += StorageWindow_Loaded;
        }

        private void StorageWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PlayerStorage storage = new PlayerStorage();
            StorageModel model = new StorageModel();

            storage = model.GetPlayerStorage(player, connection, storage);

            labelWoodAmount.Content = storage.PlayerWood.ToString();
            labelStoneAmount.Content = storage.PlayerStone.ToString();
            labelFoodAmount.Content = storage.PlayerFood.ToString();
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
