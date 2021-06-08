using LandConquest.DialogWIndows;
using LandConquestDB.Entities;
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
        private Player player;
        private PlayerStorage storage;
        private Market market;

        public MarketWindow(PlayerStorage _storage, Market _market, Player _player)
        {
            InitializeComponent();
            player = _player;
            Loaded += MarketWindow_Loaded;
        }

        private void MarketWindow_Loaded(object sender, RoutedEventArgs e)
        {
            storage = new PlayerStorage();
            market = new Market();

            storage = StorageModel.GetPlayerStorage(player);
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
                new ColumnSeries
                {
                    Values = new ChartValues<double>
                    {
                        Convert.ToDouble(labelFoodPrice.Content),
                        Convert.ToDouble(labelWoodPrice.Content),
                        Convert.ToDouble(labelStonePrice.Content),
                        Convert.ToDouble(labelIronPrice.Content),
                        Convert.ToDouble(labelGoldPrice.Content),
                        Convert.ToDouble(labelCopperPrice.Content),
                        Convert.ToDouble(labelGemsPrice.Content),
                        Convert.ToDouble(labelLeatherPrice.Content)
                        //new ScatterPoint(Convert.ToDouble(labelFoodPrice.Content),Convert.ToDouble(labelFoodMarket.Content),Convert.ToDouble(labelFoodMarket.Content)),
                        //new ScatterPoint(Convert.ToDouble(labelWoodPrice.Content),Convert.ToDouble(labelWoodMarket.Content),Convert.ToDouble(labelWoodMarket.Content)),
                        //new ScatterPoint(Convert.ToDouble(labelStonePrice.Content),Convert.ToDouble(labelStoneMarket.Content),Convert.ToDouble(labelStoneMarket.Content)),
                        //new ScatterPoint(Convert.ToDouble(labelIronPrice.Content),Convert.ToDouble(labelIronMarket.Content),Convert.ToDouble(labelIronMarket.Content)),
                        //new ScatterPoint(Convert.ToDouble(labelGoldPrice.Content),Convert.ToDouble(labelGoldMarket.Content),Convert.ToDouble(labelGoldMarket.Content)),
                        //new ScatterPoint(Convert.ToDouble(labelCopperPrice.Content),Convert.ToDouble(labelCopperMarket.Content),Convert.ToDouble(labelCopperMarket.Content)),
                        //new ScatterPoint(Convert.ToDouble(labelGemsPrice.Content),Convert.ToDouble(labelGemsMarket.Content),Convert.ToDouble(labelGemsMarket.Content)),
                        //new ScatterPoint(Convert.ToDouble(labelLeatherPrice.Content),Convert.ToDouble(labelLeatherMarket.Content),Convert.ToDouble(labelLeatherMarket.Content))

                    },
                    Fill = System.Windows.Media.Brushes.AntiqueWhite
                }

            };



            Labels = new[] { "Food", "Wood", "Stone", "Iron", "Gold", "Copper", "Gems", "Leather" };
            Formatter = value => value.ToString("N");
            DataContext = this;
            graphics.Series = series;


        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

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
                    if (FoodToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (player.PlayerMoney >= Convert.ToInt32(FoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content) && Convert.ToInt32(labelFoodMarket.Content) >= Convert.ToInt32(FoodToBuyTextBox.Text))
                    {
                        storage.PlayerFood += Convert.ToInt32(FoodToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(FoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content);
                        market.MarketFood -= Convert.ToInt32(FoodToBuyTextBox.Text);

                        labelFoodMarket.Content = Convert.ToInt32(labelFoodMarket.Content) - Convert.ToInt32(FoodToBuyTextBox.Text);
                        labelFoodAmount.Content = Convert.ToInt32(labelFoodAmount.Content) + Convert.ToInt32(FoodToBuyTextBox.Text);

                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough money or resources!");
                    }
                   
                    break;

                case "buyWoodMarketButton":
                    if (WoodToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (player.PlayerMoney >= Convert.ToInt32(WoodToBuyTextBox.Text) * Convert.ToInt32(labelWoodPrice.Content) && Convert.ToInt32(labelWoodMarket.Content) >= Convert.ToInt32(WoodToBuyTextBox.Text))
                    {

                        storage.PlayerWood += Convert.ToInt32(WoodToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(WoodToBuyTextBox.Text) * Convert.ToInt32(labelWoodPrice.Content);
                        market.MarketWood -= Convert.ToInt32(WoodToBuyTextBox.Text);

                        labelWoodMarket.Content = Convert.ToInt32(labelWoodMarket.Content) - Convert.ToInt32(WoodToBuyTextBox.Text);

                        labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) + Convert.ToInt32(WoodToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough money or resources!");
                    }
                    break;

                case "buyStoneMarketButton":
                    if (StoneToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (player.PlayerMoney >= Convert.ToInt32(StoneToBuyTextBox.Text) * Convert.ToInt32(labelStonePrice.Content) && Convert.ToInt32(labelStoneMarket.Content) >= Convert.ToInt32(StoneToBuyTextBox.Text))
                    {
                        storage.PlayerStone += Convert.ToInt32(StoneToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(StoneToBuyTextBox.Text) * Convert.ToInt32(labelStonePrice.Content);
                        market.MarketStone -= Convert.ToInt32(StoneToBuyTextBox.Text);

                        labelStoneMarket.Content = Convert.ToInt32(labelStoneMarket.Content) - Convert.ToInt32(StoneToBuyTextBox.Text);

                        labelStoneAmount.Content = Convert.ToInt32(labelStoneAmount.Content) + Convert.ToInt32(StoneToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough money or resources!");
                    }
                    break;

                case "buyIronMarketButton":
                    if (IronToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (player.PlayerMoney >= Convert.ToInt32(IronToBuyTextBox.Text) * Convert.ToInt32(labelIronPrice.Content) && Convert.ToInt32(labelIronMarket.Content) >= Convert.ToInt32(IronToBuyTextBox.Text))
                    {
                        storage.PlayerIron += Convert.ToInt32(IronToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(IronToBuyTextBox.Text) * Convert.ToInt32(labelIronPrice.Content);
                        market.MarketIron -= Convert.ToInt32(IronToBuyTextBox.Text);

                        labelIronMarket.Content = Convert.ToInt32(labelIronMarket.Content) - Convert.ToInt32(IronToBuyTextBox.Text);

                        labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) + Convert.ToInt32(IronToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough money or resources!");
                    }
                    break;

                case "buyGoldMarketButton":
                    if (GoldToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (player.PlayerMoney >= Convert.ToInt32(GoldToBuyTextBox.Text) * Convert.ToInt32(labelGoldPrice.Content) && Convert.ToInt32(labelGoldMarket.Content) >= Convert.ToInt32(GoldToBuyTextBox.Text))
                    {
                        storage.PlayerGoldOre += Convert.ToInt32(GoldToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(GoldToBuyTextBox.Text) * Convert.ToInt32(labelGoldPrice.Content);
                        market.MarketGoldOre -= Convert.ToInt32(GoldToBuyTextBox.Text);

                        labelGoldMarket.Content = Convert.ToInt32(labelGoldMarket.Content) - Convert.ToInt32(GoldToBuyTextBox.Text);

                        labelGoldAmount.Content = Convert.ToInt32(labelGoldAmount.Content) + Convert.ToInt32(GoldToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough money or resources!");
                    }
                    break;

                case "buyCopperMarketButton":
                    if (CopperToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (player.PlayerMoney >= Convert.ToInt32(CopperToBuyTextBox.Text) * Convert.ToInt32(labelCopperPrice.Content) && Convert.ToInt32(labelCopperMarket.Content) >= Convert.ToInt32(CopperToBuyTextBox.Text))
                    {
                        storage.PlayerCopper += Convert.ToInt32(CopperToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(CopperToBuyTextBox.Text) * Convert.ToInt32(labelCopperPrice.Content);
                        market.MarketCopper -= Convert.ToInt32(CopperToBuyTextBox.Text);

                        labelCopperMarket.Content = Convert.ToInt32(labelCopperMarket.Content) - Convert.ToInt32(CopperToBuyTextBox.Text);

                        labelCopperAmount.Content = Convert.ToInt32(labelCopperAmount.Content) + Convert.ToInt32(CopperToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough money or resources!");
                    }
                    break;

                case "buyGemsMarketButton":
                    if (GemsToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (player.PlayerMoney >= Convert.ToInt32(GemsToBuyTextBox.Text) * Convert.ToInt32(labelGemsPrice.Content) && Convert.ToInt32(labelGemsMarket.Content) >= Convert.ToInt32(GemsToBuyTextBox.Text))
                    {
                        storage.PlayerGems += Convert.ToInt32(GemsToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(GemsToBuyTextBox.Text) * Convert.ToInt32(labelGemsPrice.Content);
                        market.MarketGems -= Convert.ToInt32(GemsToBuyTextBox.Text);

                        labelGemsMarket.Content = Convert.ToInt32(labelGemsMarket.Content) - Convert.ToInt32(GemsToBuyTextBox.Text);

                        labelGemsAmount.Content = Convert.ToInt32(labelGemsAmount.Content) + Convert.ToInt32(GemsToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough money or resources!");
                    }
                    break;

                case "buyLeatherMarketButton":
                    if (LeatherToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (player.PlayerMoney >= Convert.ToInt32(LeatherToBuyTextBox.Text) * Convert.ToInt32(labelLeatherPrice.Content) && Convert.ToInt32(labelLeatherMarket.Content) >= Convert.ToInt32(LeatherToBuyTextBox.Text))
                    {
                        storage.PlayerLeather += Convert.ToInt32(LeatherToBuyTextBox.Text);
                        player.PlayerMoney -= Convert.ToInt32(LeatherToBuyTextBox.Text) * Convert.ToInt32(labelLeatherPrice.Content);
                        market.MarketLeather -= Convert.ToInt32(LeatherToBuyTextBox.Text);

                        labelLeatherMarket.Content = Convert.ToInt32(labelLeatherMarket.Content) - Convert.ToInt32(LeatherToBuyTextBox.Text);

                        labelLeatherAmount.Content = Convert.ToInt32(labelLeatherAmount.Content) + Convert.ToInt32(LeatherToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough money or resources!");
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
                    if (FoodToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (storage.PlayerFood >= Convert.ToInt32(FoodToBuyTextBox.Text))
                    {
                        storage.PlayerFood -= Convert.ToInt32(FoodToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(FoodToBuyTextBox.Text) * Convert.ToInt32(labelFoodPrice.Content);
                        market.MarketFood += Convert.ToInt32(FoodToBuyTextBox.Text);

                        labelFoodMarket.Content = Convert.ToInt32(labelFoodMarket.Content) + Convert.ToInt32(FoodToBuyTextBox.Text);

                        labelFoodAmount.Content = Convert.ToInt32(labelFoodAmount.Content) - Convert.ToInt32(FoodToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;

                case "sellWoodMarketButton":
                    if (WoodToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (storage.PlayerWood >= Convert.ToInt32(WoodToBuyTextBox.Text) && Convert.ToInt32(WoodToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerWood -= Convert.ToInt32(WoodToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(WoodToBuyTextBox.Text) * Convert.ToInt32(labelWoodPrice.Content);
                        market.MarketWood += Convert.ToInt32(WoodToBuyTextBox.Text);

                        labelWoodMarket.Content = Convert.ToInt32(labelWoodMarket.Content) + Convert.ToInt32(WoodToBuyTextBox.Text);

                        labelWoodAmount.Content = Convert.ToInt32(labelWoodAmount.Content) - Convert.ToInt32(WoodToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;

                case "sellStoneMarketButton":
                    if (StoneToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (storage.PlayerStone >= Convert.ToInt32(StoneToBuyTextBox.Text) && Convert.ToInt32(StoneToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerStone -= Convert.ToInt32(StoneToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(StoneToBuyTextBox.Text) * Convert.ToInt32(labelStonePrice.Content);
                        market.MarketStone += Convert.ToInt32(StoneToBuyTextBox.Text);

                        labelStoneMarket.Content = Convert.ToInt32(labelStoneMarket.Content) + Convert.ToInt32(StoneToBuyTextBox.Text);

                        labelStoneAmount.Content = Convert.ToInt32(labelStoneAmount.Content) - Convert.ToInt32(StoneToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;

                case "sellIronMarketButton":
                    if (IronToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (storage.PlayerIron >= Convert.ToInt32(IronToBuyTextBox.Text) && Convert.ToInt32(IronToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerIron -= Convert.ToInt32(IronToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(IronToBuyTextBox.Text) * Convert.ToInt32(labelIronPrice.Content);
                        market.MarketIron += Convert.ToInt32(IronToBuyTextBox.Text);

                        labelIronMarket.Content = Convert.ToInt32(labelIronMarket.Content) + Convert.ToInt32(IronToBuyTextBox.Text);

                        labelIronAmount.Content = Convert.ToInt32(labelIronAmount.Content) - Convert.ToInt32(IronToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;

                case "sellGoldMarketButton":
                    if (GoldToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (storage.PlayerGoldOre >= Convert.ToInt32(GoldToBuyTextBox.Text) && Convert.ToInt32(GoldToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerGoldOre -= Convert.ToInt32(GoldToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(GoldToBuyTextBox.Text) * Convert.ToInt32(labelGoldPrice.Content);
                        market.MarketGoldOre += Convert.ToInt32(GoldToBuyTextBox.Text);

                        labelGoldMarket.Content = Convert.ToInt32(labelGoldMarket.Content) + Convert.ToInt32(GoldToBuyTextBox.Text);

                        labelGoldAmount.Content = Convert.ToInt32(labelGoldAmount.Content) - Convert.ToInt32(GoldToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;

                case "sellCopperMarketButton":
                    if (CopperToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (storage.PlayerCopper >= Convert.ToInt32(CopperToBuyTextBox.Text) && Convert.ToInt32(CopperToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerCopper -= Convert.ToInt32(CopperToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(CopperToBuyTextBox.Text) * Convert.ToInt32(labelCopperPrice.Content);
                        market.MarketCopper += Convert.ToInt32(CopperToBuyTextBox.Text);

                        labelCopperMarket.Content = Convert.ToInt32(labelCopperMarket.Content) + Convert.ToInt32(CopperToBuyTextBox.Text);

                        labelCopperAmount.Content = Convert.ToInt32(labelCopperAmount.Content) - Convert.ToInt32(CopperToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;

                case "sellGemsMarketButton":
                    if (GemsToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (storage.PlayerGems >= Convert.ToInt32(GemsToBuyTextBox.Text) && Convert.ToInt32(GemsToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerGems -= Convert.ToInt32(GemsToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(GemsToBuyTextBox.Text) * Convert.ToInt32(labelGemsPrice.Content);
                        market.MarketGems += Convert.ToInt32(GemsToBuyTextBox.Text);

                        labelGemsMarket.Content = Convert.ToInt32(labelGemsMarket.Content) + Convert.ToInt32(GemsToBuyTextBox.Text);

                        labelGemsAmount.Content = Convert.ToInt32(labelGemsAmount.Content) - Convert.ToInt32(GemsToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
                    }
                    break;

                case "sellLeatherMarketButton":
                    if (LeatherToBuyTextBox.Text == "")
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("No data!");
                    }
                    else if (storage.PlayerLeather >= Convert.ToInt32(LeatherToBuyTextBox.Text) && Convert.ToInt32(LeatherToBuyTextBox.Text) > 0)
                    {
                        storage.PlayerGems -= Convert.ToInt32(LeatherToBuyTextBox.Text);
                        player.PlayerMoney += Convert.ToInt32(LeatherToBuyTextBox.Text) * Convert.ToInt32(labelLeatherPrice.Content);
                        market.MarketLeather += Convert.ToInt32(LeatherToBuyTextBox.Text);

                        labelLeatherMarket.Content = Convert.ToInt32(labelLeatherMarket.Content) + Convert.ToInt32(LeatherToBuyTextBox.Text);

                        labelLeatherMarket.Content = Convert.ToInt32(labelLeatherMarket.Content) - Convert.ToInt32(LeatherToBuyTextBox.Text);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
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
