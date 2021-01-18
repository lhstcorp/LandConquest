﻿using LandConquestDB.Entities;
using LandConquestDB.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;

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
            labelWoodPrice.Content = 2 * Math.Round(50000 / Convert.ToDouble(labelWoodMarket.Content), 2);


            labelFoodAmount.Content = storage.PlayerFood.ToString();
            labelFoodMarket.Content = market.MarketFood.ToString();
            labelFoodPrice.Content = 3 * Math.Round(50000 / Convert.ToDouble(labelFoodMarket.Content), 2);

            labelStoneMarket.Content = market.MarketStone.ToString();
            labelStoneAmount.Content = storage.PlayerStone.ToString();
            labelStonePrice.Content = 2 * Math.Round(50000 / Convert.ToDouble(labelStoneMarket.Content), 2);


            labelIronMarket.Content = market.MarketIron.ToString();
            labelIronAmount.Content = storage.PlayerIron.ToString();
            labelIronPrice.Content = 3 * Math.Round(50000 / Convert.ToDouble(labelIronMarket.Content), 2);

            labelGoldMarket.Content = market.MarketGoldOre.ToString();
            labelGoldAmount.Content = storage.PlayerGoldOre.ToString();
            labelGoldPrice.Content = 7 * Math.Round(50000 / Convert.ToDouble(labelGoldMarket.Content), 2);

            labelCopperMarket.Content = market.MarketCopper.ToString();
            labelCopperAmount.Content = storage.PlayerCopper.ToString();
            labelCopperPrice.Content = 4 * Math.Round(50000 / Convert.ToDouble(labelCopperMarket.Content), 2);

            labelGemsMarket.Content = market.MarketGems.ToString();
            labelGemsAmount.Content = storage.PlayerGems.ToString();
            labelGemsPrice.Content = 10 * Math.Round(50000 / Convert.ToDouble(labelGemsMarket.Content), 2);

            labelLeatherMarket.Content = market.MarketLeather.ToString();
            labelLeatherAmount.Content = storage.PlayerLeather.ToString();
            labelLeatherPrice.Content = 5 * Math.Round(50000 / Convert.ToDouble(labelLeatherMarket.Content), 2);


            //labelHarnessAmount.Content = equipment.PlayerHarness.ToString();
            //labelGearAmount.Content = equipment.PlayerGear.ToString();
            //labelSpearAmount.Content = equipment.PlayerSpear.ToString();
            //labelBowAmount.Content = equipment.PlayerBow.ToString();
            //labelArmorAmount.Content = equipment.PlayerArmor.ToString();
            //labelSwordAmount.Content = equipment.PlayerSword.ToString();

            SeriesCollection series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> { 3, 5, 7, 4 }
                },
                new ColumnSeries
                {
                    Values = new ChartValues<decimal> { 5, 6, 2, 7 }
                }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void buyAllMarketButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "buyFoodMarketButton":
                    if (player.PlayerMoney >= Convert.ToInt32(FoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content) && Convert.ToInt32(FoodToBuyTextBox.Text) > 0 && Convert.ToInt32(labelFoodMarket.Content) >= Convert.ToInt32(FoodToBuyTextBox.Text))
                    {
                        storage.PlayerFood += Convert.ToInt32(FoodToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(FoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content);
                        market.MarketFood -= Convert.ToInt32(FoodToBuyTextBox.Text);

                        labelFoodMarket.Content = Convert.ToInt32(labelFoodMarket.Content) - Convert.ToInt32(FoodToBuyTextBox.Text);
                        labelFoodAmount.Content = Convert.ToInt32(labelFoodAmount.Content) + Convert.ToInt32(FoodToBuyTextBox.Text);

                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "buyWoodMarketButton":
                    if (player.PlayerMoney >= Convert.ToInt32(WoodToBuyTextBox.Text) * Convert.ToInt32(labelWoodPrice.Content) && Convert.ToInt32(WoodToBuyTextBox.Text) > 0 && Convert.ToInt32(labelWoodMarket.Content) >= Convert.ToInt32(WoodToBuyTextBox.Text))
                    {

                        storage.PlayerWood += Convert.ToInt32(WoodToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(WoodToBuyTextBox.Text) * Convert.ToInt32(labelWoodPrice.Content);
                        market.MarketWood -= Convert.ToInt32(WoodToBuyTextBox.Text);

                        labelWoodMarket.Content = Convert.ToInt32(labelWoodMarket.Content) - Convert.ToInt32(WoodToBuyTextBox.Text);

                        labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) + Convert.ToInt32(WoodToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "buyStoneMarketButton":
                    if (player.PlayerMoney >= Convert.ToInt32(StoneToBuyTextBox.Text) * Convert.ToInt32(labelStonePrice.Content) && Convert.ToInt32(StoneToBuyTextBox.Text) > 0 && Convert.ToInt32(labelStoneMarket.Content) >= Convert.ToInt32(StoneToBuyTextBox.Text))
                    {
                        storage.PlayerStone += Convert.ToInt32(StoneToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(StoneToBuyTextBox.Text) * Convert.ToInt32(labelStonePrice.Content);
                        market.MarketStone -= Convert.ToInt32(StoneToBuyTextBox.Text);

                        labelStoneMarket.Content = Convert.ToInt32(labelStoneMarket.Content) - Convert.ToInt32(StoneToBuyTextBox.Text);

                        labelStoneAmount.Content = Convert.ToInt32(labelStoneAmount.Content) + Convert.ToInt32(StoneToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "buyIronMarketButton":
                    if (player.PlayerMoney >= Convert.ToInt32(IronToBuyTextBox.Text) * Convert.ToInt32(labelIronPrice.Content) && Convert.ToInt32(IronToBuyTextBox.Text) > 0 && Convert.ToInt32(labelIronMarket.Content) >= Convert.ToInt32(IronToBuyTextBox.Text))
                    {
                        storage.PlayerIron += Convert.ToInt32(IronToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(IronToBuyTextBox.Text) * Convert.ToInt32(labelIronPrice.Content);
                        market.MarketIron -= Convert.ToInt32(IronToBuyTextBox.Text);

                        labelIronMarket.Content = Convert.ToInt32(labelIronMarket.Content) - Convert.ToInt32(IronToBuyTextBox.Text);

                        labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) + Convert.ToInt32(IronToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "buyGoldMarketButton":
                    if (player.PlayerMoney >= Convert.ToInt32(GoldToBuyTextBox.Text) * Convert.ToInt32(labelGoldPrice.Content) && Convert.ToInt32(GoldToBuyTextBox.Text) > 0 && Convert.ToInt32(labelGoldMarket.Content) >= Convert.ToInt32(GoldToBuyTextBox.Text))
                    {
                        storage.PlayerGoldOre += Convert.ToInt32(GoldToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(GoldToBuyTextBox.Text) * Convert.ToInt32(labelGoldPrice.Content);
                        market.MarketGoldOre -= Convert.ToInt32(GoldToBuyTextBox.Text);

                        labelGoldMarket.Content = Convert.ToInt32(labelGoldMarket.Content) - Convert.ToInt32(GoldToBuyTextBox.Text);

                        labelGoldAmount.Content = Convert.ToInt32(labelGoldAmount.Content) + Convert.ToInt32(GoldToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "buyCopperMarketButton":
                    if (player.PlayerMoney >= Convert.ToInt32(CopperToBuyTextBox.Text) * Convert.ToInt32(labelCopperPrice.Content) && Convert.ToInt32(CopperToBuyTextBox.Text) > 0 && Convert.ToInt32(labelCopperMarket.Content) >= Convert.ToInt32(CopperToBuyTextBox.Text))
                    {
                        storage.PlayerCopper += Convert.ToInt32(CopperToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(CopperToBuyTextBox.Text) * Convert.ToInt32(labelCopperPrice.Content);
                        market.MarketCopper -= Convert.ToInt32(CopperToBuyTextBox.Text);

                        labelCopperMarket.Content = Convert.ToInt32(labelCopperMarket.Content) - Convert.ToInt32(CopperToBuyTextBox.Text);

                        labelCopperAmount.Content = Convert.ToInt32(labelCopperAmount.Content) + Convert.ToInt32(CopperToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "buyGemsMarketButton":
                    if (player.PlayerMoney >= Convert.ToInt32(GemsToBuyTextBox.Text) * Convert.ToInt32(labelGemsPrice.Content) && Convert.ToInt32(GemsToBuyTextBox.Text) > 0 && Convert.ToInt32(labelGemsMarket.Content) >= Convert.ToInt32(GemsToBuyTextBox.Text))
                    {
                        storage.PlayerGems += Convert.ToInt32(GemsToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(GemsToBuyTextBox.Text) * Convert.ToInt32(labelGemsPrice.Content);
                        market.MarketGems -= Convert.ToInt32(GemsToBuyTextBox.Text);

                        labelGemsMarket.Content = Convert.ToInt32(labelGemsMarket.Content) - Convert.ToInt32(GemsToBuyTextBox.Text);

                        labelGemsAmount.Content = Convert.ToInt32(labelGemsAmount.Content) + Convert.ToInt32(GemsToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "buyLeatherMarketButton":
                    if (player.PlayerMoney >= Convert.ToInt32(LeatherToBuyTextBox.Text) * Convert.ToInt32(labelLeatherPrice.Content) && Convert.ToInt32(LeatherToBuyTextBox.Text) > 0 && Convert.ToInt32(labelLeatherMarket.Content) >= Convert.ToInt32(LeatherToBuyTextBox.Text))
                    {
                        storage.PlayerLeather += Convert.ToInt32(LeatherToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(LeatherToBuyTextBox.Text) * Convert.ToInt32(labelLeatherPrice.Content);
                        market.MarketLeather -= Convert.ToInt32(LeatherToBuyTextBox.Text);

                        labelLeatherMarket.Content = Convert.ToInt32(labelLeatherMarket.Content) - Convert.ToInt32(LeatherToBuyTextBox.Text);

                        labelLeatherAmount.Content = Convert.ToInt32(labelLeatherAmount.Content) + Convert.ToInt32(LeatherToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;




            }

            StorageModel.UpdateStorage(player, storage);
            PlayerModel.UpdatePlayerMoney(player);
            MarketModel.UpdateMarket(player, market);
            MarketWindow_Loaded(sender, e);
        }

        private void sellAllMarketButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {

                case "sellFoodMarketButton":
                    if (storage.PlayerFood >= Convert.ToInt32(FoodToBuyTextBox.Text) && Convert.ToInt32(FoodToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerFood -= Convert.ToInt32(FoodToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(FoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content);
                        market.MarketFood += Convert.ToInt32(FoodToBuyTextBox.Text);

                        labelFoodMarket.Content = Convert.ToInt32(labelFoodMarket.Content) + Convert.ToInt32(FoodToBuyTextBox.Text);

                        labelFoodAmount.Content = Convert.ToInt32(labelFoodAmount.Content) - Convert.ToInt32(FoodToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "sellWoodMarketButton":
                    if (storage.PlayerWood >= Convert.ToInt32(WoodToBuyTextBox.Text) && Convert.ToInt32(WoodToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerWood -= Convert.ToInt32(WoodToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(WoodToBuyTextBox.Text) * Convert.ToInt32(labelWoodPrice.Content);
                        market.MarketWood += Convert.ToInt32(WoodToBuyTextBox.Text);

                        labelWoodMarket.Content = Convert.ToInt32(labelWoodMarket.Content) + Convert.ToInt32(WoodToBuyTextBox.Text);

                        labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) - Convert.ToInt32(WoodToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "sellStoneMarketButton":
                    if (storage.PlayerStone >= Convert.ToInt32(StoneToBuyTextBox.Text) && Convert.ToInt32(StoneToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerStone -= Convert.ToInt32(StoneToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(StoneToBuyTextBox.Text) * Convert.ToInt32(labelStonePrice.Content);
                        market.MarketStone += Convert.ToInt32(StoneToBuyTextBox.Text);

                        labelStoneMarket.Content = Convert.ToInt32(labelStoneMarket.Content) + Convert.ToInt32(StoneToBuyTextBox.Text);

                        labelStoneAmount.Content = Convert.ToInt32(labelStoneAmount.Content) - Convert.ToInt32(StoneToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "sellIronMarketButton":
                    if (storage.PlayerIron >= Convert.ToInt32(IronToBuyTextBox.Text) && Convert.ToInt32(IronToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerIron -= Convert.ToInt32(IronToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(IronToBuyTextBox.Text) * Convert.ToInt32(labelIronPrice.Content);
                        market.MarketIron += Convert.ToInt32(IronToBuyTextBox.Text);

                        labelIronMarket.Content = Convert.ToInt32(labelIronMarket.Content) + Convert.ToInt32(IronToBuyTextBox.Text);

                        labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) - Convert.ToInt32(IronToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "sellGoldMarketButton":
                    if (storage.PlayerGoldOre >= Convert.ToInt32(GoldToBuyTextBox.Text) && Convert.ToInt32(GoldToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerGoldOre -= Convert.ToInt32(GoldToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(GoldToBuyTextBox.Text) * Convert.ToInt32(labelGoldPrice.Content);
                        market.MarketGoldOre += Convert.ToInt32(GoldToBuyTextBox.Text);

                        labelGoldMarket.Content = Convert.ToInt32(labelGoldMarket.Content) + Convert.ToInt32(GoldToBuyTextBox.Text);

                        labelGoldAmount.Content = Convert.ToInt32(labelGoldAmount.Content) - Convert.ToInt32(GoldToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "sellCopperMarketButton":
                    if (storage.PlayerCopper >= Convert.ToInt32(CopperToBuyTextBox.Text) && Convert.ToInt32(CopperToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerCopper -= Convert.ToInt32(CopperToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(CopperToBuyTextBox.Text) * Convert.ToInt32(labelCopperPrice.Content);
                        market.MarketCopper += Convert.ToInt32(CopperToBuyTextBox.Text);

                        labelCopperMarket.Content = Convert.ToInt32(labelCopperMarket.Content) + Convert.ToInt32(CopperToBuyTextBox.Text);

                        labelCopperAmount.Content = Convert.ToInt32(labelCopperAmount.Content) - Convert.ToInt32(CopperToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "sellGemsMarketButton":
                    if (storage.PlayerGems >= Convert.ToInt32(GemsToBuyTextBox.Text) && Convert.ToInt32(GemsToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerGems -= Convert.ToInt32(GemsToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(GemsToBuyTextBox.Text) * Convert.ToInt32(labelGemsPrice.Content);
                        market.MarketGems += Convert.ToInt32(GemsToBuyTextBox.Text);

                        labelGemsMarket.Content = Convert.ToInt32(labelGemsMarket.Content) + Convert.ToInt32(GemsToBuyTextBox.Text);

                        labelGemsAmount.Content = Convert.ToInt32(labelGemsAmount.Content) - Convert.ToInt32(GemsToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

                case "sellLeatherMarketButton":
                    if (storage.PlayerLeather >= Convert.ToInt32(LeatherToBuyTextBox.Text) && Convert.ToInt32(LeatherToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerGems -= Convert.ToInt32(LeatherToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(LeatherToBuyTextBox.Text) * Convert.ToInt32(labelLeatherPrice.Content);
                        market.MarketLeather += Convert.ToInt32(LeatherToBuyTextBox.Text);

                        labelLeatherMarket.Content = Convert.ToInt32(labelLeatherMarket.Content) + Convert.ToInt32(LeatherToBuyTextBox.Text);

                        labelLeatherMarket.Content = Convert.ToInt32(labelLeatherMarket.Content) - Convert.ToInt32(LeatherToBuyTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    break;

            }

            StorageModel.UpdateStorage(player, storage);
            PlayerModel.UpdatePlayerMoney(player);
            MarketModel.UpdateMarket(player, market);
            MarketWindow_Loaded(sender, e);
        }


        private void FoodToBuyTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
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
