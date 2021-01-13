using LandConquest.Entities;
using LandConquest.Models;
using System;
using System.Windows;

namespace LandConquest.Forms
{
    public partial class MarketWindow : Window
    {
        MainWindow window;
        Player player;

        PlayerStorage storage;
        Market market;

        public MarketWindow(MainWindow _window, PlayerStorage _storage, Market _market, Player _player)
        {
            InitializeComponent();
            window = _window;
            player = _player;
            Loaded += MarketWindow_Loaded;
        }

        private void MarketWindow_Loaded(object sender, RoutedEventArgs e)
        {
            storage = new PlayerStorage();
            market = new Market();

            storage = StorageModel.GetPlayerStorage(player, storage);
            market = MarketModel.GetMarketInfo(player, market);

            labelWoodMarket.Content = market.MarketWood.ToString();
            labelWoodAmount.Content = storage.PlayerWood.ToString();
            if (Convert.ToInt32(labelWoodMarket.Content) > 50000)
            {
                labelWoodPrice.Content = 2 * Math.Round(50000 / Convert.ToDouble(labelWoodMarket.Content));
            }
            else
            {
                labelWoodPrice.Content = 2 * (50000 / Convert.ToDouble(labelWoodMarket.Content));
            }


            labelFoodAmount.Content = storage.PlayerFood.ToString();
            labelFoodMarket.Content = market.MarketFood.ToString();
            if (Convert.ToInt32(labelFoodMarket.Content) > 50000)
            {
                labelFoodPrice.Content = 3 * Math.Round(50000 / Convert.ToDouble(labelFoodMarket.Content));
            }
            else
            {
                labelFoodPrice.Content = 3 * (50000 / Convert.ToDouble(labelFoodMarket.Content));
            }

            labelStoneMarket.Content = market.MarketStone.ToString();
            labelStoneAmount.Content = storage.PlayerStone.ToString();
            if (Convert.ToInt32(labelStoneMarket.Content) > 50000)
            {
                labelStonePrice.Content = 2 * Math.Ceiling(50000 / Convert.ToDouble(labelStoneMarket.Content));
            }
            else
            {
                labelStonePrice.Content = 2 * (50000 / Convert.ToDouble(labelStoneMarket.Content));
            }

            labelIronMarket.Content = market.MarketIron.ToString();
            labelIronAmount.Content = storage.PlayerIron.ToString();
            if (Convert.ToInt32(labelIronMarket.Content) > 50000)
            {
                labelIronPrice.Content = 3 * Math.Ceiling(50000 / Convert.ToDouble(labelIronMarket.Content));
            }
            else
            {
                labelIronPrice.Content = 3 * (50000 / Convert.ToDouble(labelIronMarket.Content));
            }

            labelGoldMarket.Content = market.MarketGoldOre.ToString();
            labelGoldAmount.Content = storage.PlayerGoldOre.ToString();
            if (Convert.ToInt32(labelGoldMarket.Content) > 50000)
            {
                labelGoldPrice.Content = 7 * Math.Ceiling(50000 / Convert.ToDouble(labelGoldMarket.Content));
            }
            else
            {
                labelGoldPrice.Content = 7 * (50000 / Convert.ToDouble(labelGoldMarket.Content));
            }

            labelCopperMarket.Content = market.MarketCopper.ToString();
            labelCopperAmount.Content = storage.PlayerCopper.ToString();
            if (Convert.ToInt32(labelCopperMarket.Content) > 50000)
            {
                labelCopperPrice.Content = 4 * Math.Ceiling(50000 / Convert.ToDouble(labelCopperMarket.Content));
            }
            else
            {
                labelCopperPrice.Content = 4 * (50000 / Convert.ToDouble(labelCopperMarket.Content));
            }

            labelGemsMarket.Content = market.MarketGems.ToString();
            labelGemsAmount.Content = storage.PlayerGems.ToString();
            if (Convert.ToInt32(labelGemsMarket.Content) > 50000)
            {
                labelGemsPrice.Content = 10 * Math.Ceiling(50000 / Convert.ToDouble(labelGemsMarket.Content));
            }
            else
            {
                labelGemsPrice.Content = 10 * (50000 / Convert.ToDouble(labelGemsMarket.Content));
            }

            labelLeatherMarket.Content = market.MarketLeather.ToString();
            labelLeatherAmount.Content = storage.PlayerLeather.ToString();
            if (Convert.ToInt32(labelLeatherMarket.Content) > 50000)
            {
                labelLeatherPrice.Content = 5 * Math.Ceiling(50000 / Convert.ToDouble(labelLeatherMarket.Content));
            }
            else
            {
                labelLeatherPrice.Content = 5 * (50000 / Convert.ToDouble(labelLeatherMarket.Content));
            }



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

            if (player.PlayerMoney >= Convert.ToInt32(FoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content) && Convert.ToInt32(labelFoodMarket.Content) >= Convert.ToInt32(FoodToBuyTextBox.Text))
            {
                storage.PlayerFood += Convert.ToInt32(FoodToBuyTextBox.Text);
                player.PlayerMoney -= Convert.ToInt32(FoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content);
                market.MarketFood -= Convert.ToInt32(FoodToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
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
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
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
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
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
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelWoodMarket.Content = Convert.ToInt32(labelWoodMarket.Content) + Convert.ToInt32(WoodToBuyTextBox.Text);

                labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) - Convert.ToInt32(WoodToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void buyStoneMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.PlayerMoney >= Convert.ToInt32(StoneToBuyTextBox.Text) * Convert.ToInt32(labelStonePrice.Content) && Convert.ToInt32(labelStoneMarket.Content) >= Convert.ToInt32(StoneToBuyTextBox.Text))
            {
                storage.PlayerStone += Convert.ToInt32(StoneToBuyTextBox.Text);
                player.PlayerMoney -= Convert.ToInt32(StoneToBuyTextBox.Text) * Convert.ToInt32(labelStonePrice.Content);
                market.MarketStone -= Convert.ToInt32(StoneToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelStoneMarket.Content = Convert.ToInt32(labelStoneMarket.Content) - Convert.ToInt32(StoneToBuyTextBox.Text);

                labelStoneAmount.Content = Convert.ToInt32(labelStoneAmount.Content) + Convert.ToInt32(StoneToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void sellStoneMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (storage.PlayerStone >= Convert.ToInt32(StoneToBuyTextBox.Text))
            {
                storage.PlayerStone -= Convert.ToInt32(StoneToBuyTextBox.Text);
                player.PlayerMoney += Convert.ToInt32(StoneToBuyTextBox.Text) * Convert.ToInt32(labelStonePrice.Content);
                market.MarketStone += Convert.ToInt32(StoneToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelStoneMarket.Content = Convert.ToInt32(labelStoneMarket.Content) + Convert.ToInt32(StoneToBuyTextBox.Text);

                labelStoneAmount.Content = Convert.ToInt32(labelStoneAmount.Content) - Convert.ToInt32(StoneToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void buyIronMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.PlayerMoney >= Convert.ToInt32(IronToBuyTextBox.Text) * Convert.ToInt32(labelIronPrice.Content) && Convert.ToInt32(labelIronMarket.Content) >= Convert.ToInt32(IronToBuyTextBox.Text))
            {
                storage.PlayerIron += Convert.ToInt32(IronToBuyTextBox.Text);
                player.PlayerMoney -= Convert.ToInt32(IronToBuyTextBox.Text) * Convert.ToInt32(labelIronPrice.Content);
                market.MarketIron -= Convert.ToInt32(IronToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelIronMarket.Content = Convert.ToInt32(labelIronMarket.Content) - Convert.ToInt32(IronToBuyTextBox.Text);

                labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) + Convert.ToInt32(IronToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void sellIronMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (storage.PlayerIron >= Convert.ToInt32(IronToBuyTextBox.Text))
            {
                storage.PlayerIron -= Convert.ToInt32(IronToBuyTextBox.Text);
                player.PlayerMoney += Convert.ToInt32(IronToBuyTextBox.Text) * Convert.ToInt32(labelIronPrice.Content);
                market.MarketIron += Convert.ToInt32(IronToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelIronMarket.Content = Convert.ToInt32(labelIronMarket.Content) + Convert.ToInt32(IronToBuyTextBox.Text);

                labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) - Convert.ToInt32(IronToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void buyGoldMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.PlayerMoney >= Convert.ToInt32(GoldToBuyTextBox.Text) * Convert.ToInt32(labelGoldPrice.Content) && Convert.ToInt32(labelGoldMarket.Content) >= Convert.ToInt32(GoldToBuyTextBox.Text))
            {
                storage.PlayerGoldOre += Convert.ToInt32(GoldToBuyTextBox.Text);
                player.PlayerMoney -= Convert.ToInt32(GoldToBuyTextBox.Text) * Convert.ToInt32(labelGoldPrice.Content);
                market.MarketGoldOre -= Convert.ToInt32(GoldToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelGoldMarket.Content = Convert.ToInt32(labelGoldMarket.Content) - Convert.ToInt32(GoldToBuyTextBox.Text);

                labelGoldAmount.Content = Convert.ToInt32(labelGoldAmount.Content) + Convert.ToInt32(GoldToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void sellGoldMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (storage.PlayerGoldOre >= Convert.ToInt32(GoldToBuyTextBox.Text))
            {
                storage.PlayerGoldOre -= Convert.ToInt32(GoldToBuyTextBox.Text);
                player.PlayerMoney += Convert.ToInt32(GoldToBuyTextBox.Text) * Convert.ToInt32(labelIronPrice.Content);
                market.MarketGoldOre += Convert.ToInt32(GoldToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelGoldMarket.Content = Convert.ToInt32(labelGoldMarket.Content) + Convert.ToInt32(GoldToBuyTextBox.Text);

                labelGoldAmount.Content = Convert.ToInt32(labelGoldAmount.Content) - Convert.ToInt32(GoldToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void buyCopperMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.PlayerMoney >= Convert.ToInt32(CopperToBuyTextBox.Text) * Convert.ToInt32(labelCopperPrice.Content) && Convert.ToInt32(labelCopperMarket.Content) >= Convert.ToInt32(CopperToBuyTextBox.Text))
            {
                storage.PlayerCopper += Convert.ToInt32(CopperToBuyTextBox.Text);
                player.PlayerMoney -= Convert.ToInt32(CopperToBuyTextBox.Text) * Convert.ToInt32(labelCopperPrice.Content);
                market.MarketCopper -= Convert.ToInt32(CopperToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelCopperMarket.Content = Convert.ToInt32(labelCopperMarket.Content) - Convert.ToInt32(CopperToBuyTextBox.Text);

                labelCopperAmount.Content = Convert.ToInt32(labelCopperAmount.Content) + Convert.ToInt32(CopperToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void sellCopperMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (storage.PlayerCopper >= Convert.ToInt32(CopperToBuyTextBox.Text))
            {
                storage.PlayerCopper -= Convert.ToInt32(CopperToBuyTextBox.Text);
                player.PlayerMoney += Convert.ToInt32(CopperToBuyTextBox.Text) * Convert.ToInt32(labelCopperPrice.Content);
                market.MarketCopper += Convert.ToInt32(CopperToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelCopperMarket.Content = Convert.ToInt32(labelCopperMarket.Content) + Convert.ToInt32(CopperToBuyTextBox.Text);

                labelCopperAmount.Content = Convert.ToInt32(labelCopperAmount.Content) - Convert.ToInt32(CopperToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void buyGemsMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.PlayerMoney >= Convert.ToInt32(GemsToBuyTextBox.Text) * Convert.ToInt32(labelGemsPrice.Content) && Convert.ToInt32(labelGemsMarket.Content) >= Convert.ToInt32(GemsToBuyTextBox.Text))
            {
                storage.PlayerGems += Convert.ToInt32(GemsToBuyTextBox.Text);
                player.PlayerMoney -= Convert.ToInt32(GemsToBuyTextBox.Text) * Convert.ToInt32(labelGemsPrice.Content);
                market.MarketGems -= Convert.ToInt32(GemsToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelGemsMarket.Content = Convert.ToInt32(labelGemsMarket.Content) - Convert.ToInt32(GemsToBuyTextBox.Text);

                labelGemsAmount.Content = Convert.ToInt32(labelGemsAmount.Content) + Convert.ToInt32(GemsToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void sellGemsMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (storage.PlayerGems >= Convert.ToInt32(GemsToBuyTextBox.Text))
            {
                storage.PlayerGems -= Convert.ToInt32(GemsToBuyTextBox.Text);
                player.PlayerMoney += Convert.ToInt32(GemsToBuyTextBox.Text) * Convert.ToInt32(labelGemsPrice.Content);
                market.MarketGems += Convert.ToInt32(GemsToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelGemsMarket.Content = Convert.ToInt32(labelGemsMarket.Content) + Convert.ToInt32(GemsToBuyTextBox.Text);

                labelGemsAmount.Content = Convert.ToInt32(labelGemsAmount.Content) - Convert.ToInt32(GemsToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void buyLeatherMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.PlayerMoney >= Convert.ToInt32(LeatherToBuyTextBox.Text) * Convert.ToInt32(labelLeatherPrice.Content) && Convert.ToInt32(labelLeatherMarket.Content) >= Convert.ToInt32(LeatherToBuyTextBox.Text))
            {
                storage.PlayerLeather += Convert.ToInt32(LeatherToBuyTextBox.Text);
                player.PlayerMoney -= Convert.ToInt32(LeatherToBuyTextBox.Text) * Convert.ToInt32(labelLeatherPrice.Content);
                market.MarketLeather -= Convert.ToInt32(LeatherToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelLeatherMarket.Content = Convert.ToInt32(labelLeatherMarket.Content) - Convert.ToInt32(LeatherToBuyTextBox.Text);

                labelLeatherAmount.Content = Convert.ToInt32(labelLeatherAmount.Content) + Convert.ToInt32(LeatherToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        private void sellLeatherMarketButton_Click(object sender, RoutedEventArgs e)
        {
            if (storage.PlayerLeather >= Convert.ToInt32(LeatherToBuyTextBox.Text))
            {
                storage.PlayerGems -= Convert.ToInt32(LeatherToBuyTextBox.Text);
                player.PlayerMoney += Convert.ToInt32(LeatherToBuyTextBox.Text) * Convert.ToInt32(labelLeatherPrice.Content);
                market.MarketLeather += Convert.ToInt32(LeatherToBuyTextBox.Text);
                StorageModel.UpdateStorage(player, storage);
                PlayerModel.UpdatePlayerMoney(player);
                MarketModel.UpdateMarket(player, market);
                labelLeatherMarket.Content = Convert.ToInt32(labelLeatherMarket.Content) + Convert.ToInt32(LeatherToBuyTextBox.Text);

                labelLeatherMarket.Content = Convert.ToInt32(labelLeatherMarket.Content) - Convert.ToInt32(LeatherToBuyTextBox.Text);
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }
    }
}
