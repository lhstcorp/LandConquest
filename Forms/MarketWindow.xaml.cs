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
        PlayerModel money;
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
 
            model = new StorageModel();
            money = new PlayerModel();
            storage = new PlayerStorage();
            market = new Market();
            marketModel = new MarketModel();
            
            storage = model.GetPlayerStorage(player, connection, storage);
            market = marketModel.GetMarketInfo(player, connection, market);

            labelWoodMarket.Content = market.MarketWood.ToString();
            labelWoodAmount.Content = storage.PlayerWood.ToString();
            if (Convert.ToInt32(labelWoodMarket.Content) > 50000)
            {
                labelWoodPrice.Content = 2 * (50000 / Convert.ToInt32(labelWoodMarket.Content));
            }
            else
            {
                labelWoodPrice.Content = 2 * (50000 / Convert.ToInt32(labelWoodMarket.Content));
            }
            //labelStoneAmount.Content = storage.PlayerStone.ToString();
            labelFoodAmount.Content = storage.PlayerFood.ToString();
            labelFoodMarket.Content = market.MarketFood.ToString();
            if (Convert.ToInt32(labelFoodMarket.Content) > 50000)
            {
                labelFoodPrice.Content = 2 * (50000 / Convert.ToInt32(labelFoodMarket.Content));
            }
            else
            {
                labelFoodPrice.Content = 2 * (50000 / Convert.ToInt32(labelFoodMarket.Content));
            }

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
            
            if (player.PlayerMoney>=Convert.ToInt32(FoodToBuyTextBox.Text)*Convert.ToInt32(labelFoodPrice.Content)&&Convert.ToInt32(labelFoodMarket.Content)>= Convert.ToInt32(FoodToBuyTextBox.Text))
            {
                storage.PlayerFood += Convert.ToInt32(FoodToBuyTextBox.Text);
                player.PlayerMoney -= Convert.ToInt32(FoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content);
                market.MarketFood -= Convert.ToInt32(FoodToBuyTextBox.Text);
                model.UpdateStorage(connection, player, storage);
                money.UpdatePlayerMoney(player, connection);
                marketModel.UpdateMarket(connection, player, market);
                labelFoodMarket.Content = Convert.ToInt32(labelFoodMarket.Content) - Convert.ToInt32(FoodToBuyTextBox.Text);
                labelFoodAmount.Content = Convert.ToInt32(labelFoodAmount.Content) + Convert.ToInt32(FoodToBuyTextBox.Text);
                
            }
            else
            {
                MessageBox.Show("Error!");
            }
            
        }

        private void sellFoodMarketButton_Click(object sender, RoutedEventArgs e)
        {

            if (storage.PlayerFood >= Convert.ToInt32(FoodToBuyTextBox.Text))
            {
                storage.PlayerFood -= Convert.ToInt32(FoodToBuyTextBox.Text);
                player.PlayerMoney += Convert.ToInt32(FoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content);
                market.MarketFood += Convert.ToInt32(FoodToBuyTextBox.Text);
                model.UpdateStorage(connection, player, storage);
                money.UpdatePlayerMoney(player, connection);
                marketModel.UpdateMarket(connection, player, market);
                labelFoodMarket.Content = Convert.ToInt32(labelFoodMarket.Content) + Convert.ToInt32(FoodToBuyTextBox.Text);

                labelFoodAmount.Content = Convert.ToInt32(labelFoodAmount.Content) - Convert.ToInt32(FoodToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void buyWoodMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.PlayerMoney >= Convert.ToInt32(WoodToBuyTextBox.Text) * Convert.ToInt32(labelWoodPrice.Content) && Convert.ToInt32(labelWoodMarket.Content) >= Convert.ToInt32(WoodToBuyTextBox.Text))
            {
                storage.PlayerWood += Convert.ToInt32(WoodToBuyTextBox.Text);
                player.PlayerMoney -= Convert.ToInt32(WoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content);
                market.MarketWood -= Convert.ToInt32(WoodToBuyTextBox.Text);
                model.UpdateStorage(connection, player, storage);
                money.UpdatePlayerMoney(player, connection);
                marketModel.UpdateMarket(connection, player, market);
                labelWoodMarket.Content = Convert.ToInt32(labelWoodMarket.Content) - Convert.ToInt32(WoodToBuyTextBox.Text);

                labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) + Convert.ToInt32(WoodToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void sellWoodMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (storage.PlayerWood >= Convert.ToInt32(WoodToBuyTextBox.Text))
            {
                storage.PlayerWood -= Convert.ToInt32(WoodToBuyTextBox.Text);
                player.PlayerMoney += Convert.ToInt32(WoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content);
                market.MarketWood += Convert.ToInt32(WoodToBuyTextBox.Text);
                model.UpdateStorage(connection, player, storage);
                money.UpdatePlayerMoney(player, connection);
                marketModel.UpdateMarket(connection, player, market);
                labelWoodMarket.Content = Convert.ToInt32(labelWoodMarket.Content) + Convert.ToInt32(WoodToBuyTextBox.Text);

                labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) - Convert.ToInt32(WoodToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }
    }
}
