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
    /// <summary>
    /// Логика взаимодействия для MarketWindow.xaml
    /// </summary>
    public partial class MarketWindow : Window
    {
        SqlConnection connection;
        MainWindow window;
        Player player;
        User user;
        StorageModel model;
        PlayerStorage storage;
        Market market;
        PlayerEquipment equipment;
        EquipmentModel equipmentModel;
        MarketModel marketModel;
        public MarketWindow(MainWindow _window, SqlConnection _connection, PlayerStorage _storage, Market _market, Player _player)
        {
            InitializeComponent();
            window = _window;
            connection = _connection;
            player = _player;
            //user = _user;
            Loaded += MarketWindow_Loaded;
        }

        private void MarketWindow_Loaded(object sender, RoutedEventArgs e)
        {
            market = new Market();
            marketModel = new MarketModel();
            model = new StorageModel();
            storage = new PlayerStorage();
            
            storage = model.GetPlayerStorage(player, connection, storage);
            market = marketModel.GetMarketInfo(player, connection, market);
            

            //labelWoodAmount.Content = storage.PlayerWood.ToString();
            //labelStoneAmount.Content = storage.PlayerStone.ToString();
            labelFoodAmount.Content = storage.PlayerFood.ToString();
            
            //labelGemsAmount.Content = storage.PlayerGems.ToString();
            //labelCopperAmount.Content = storage.PlayerCopper.ToString();
            //labelGoldAmount.Content = storage.PlayerGoldOre.ToString();
            //labelIronAmount.Content = storage.PlayerIron.ToString();
            //labelLeatherAmount.Content = storage.PlayerLeather.ToString();

            //labelHarnessAmount.Content = equipment.PlayerHarness.ToString();
            //labelGearAmount.Content = equipment.PlayerGear.ToString();
            //labelSpearAmount.Content = equipment.PlayerSpear.ToString();
            //labelBowAmount.Content = equipment.PlayerBow.ToString();
            //labelArmorAmount.Content = equipment.PlayerArmor.ToString();
            //labelSwordAmount.Content = equipment.PlayerSword.ToString();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buyFoodMarket_Click(object sender, RoutedEventArgs e)
        {
            if (player.PlayerMoney>=Convert.ToInt32(FoodToBuyTextBox.Text)*Convert.ToInt32(labelFoodPrice.Content))
            {
                storage.PlayerFood += Convert.ToInt32(FoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content);
                player.PlayerMoney -= Convert.ToInt32(FoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content);
                model.UpdateStorage(connection, player, storage);
                marketModel.UpdateMarket(connection, player, market);
                labelFoodAmount.Content = Convert.ToInt32(labelFoodAmount.Content) + Convert.ToInt32(FoodToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }

        }
    }
}
