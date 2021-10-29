using LandConquest.DialogWIndows;
using LandConquest.Logic;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections;
using System.Globalization;
using WPFLocalizeExtension.Engine;

namespace LandConquest.Forms
{
    public partial class MainWindow : Window
    {
        private User user;
        private Player player;
        private Market market;
        private PlayerStorage storage;
        private PlayerEquipment equipment;
        private PlayerEntrance playerEntrance;
        private Manufacture manufacture;
        private Taxes taxes;
        private Peasants peasants;
        private List<Manufacture> landmanufactures;
        private List<Land> lands;
        private List<Path> paths;
        private List<Country> countries;
        private List<War> wars;
        private List<PlayerEntrance> playerEntrances;
        private Land land;
        private Army army;
        private Country country;
        private War WAR; //GLOBAL
        private Thickness[] marginsOfWarButtons;
        private int[] flagXY;
        private const int landsCount = 11;
        private Window openedWindow;

        public MainWindow(User _user)
        {
            InitializeComponent();
            //this.Resources.Add("buttonGradientBrush", gradientBrush);
            user = _user;
            equipment = new PlayerEquipment();
            player = new Player();
            storage = new PlayerStorage();
            manufacture = new Manufacture();
            peasants = new Peasants();
            country = new Country();
            market = new Market();
            landmanufactures = new List<Manufacture>();
            army = new Army();
            flagXY = new int[4];
            openedWindow = this;

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            player = PlayerModel.GetPlayerById(user.UserId);
            PbExp.Maximum = Math.Pow(player.PlayerLvl, 2) * 500;
            PbExp.Value = player.PlayerExp;
            Level.Content = player.PlayerLvl;
            Console.WriteLine(player.PlayerExp);

            CultureInfo newCulture;
            if (LocalizeDictionary.Instance.Culture.Name.Contains("ru"))
            {
                newCulture = CultureInfo.GetCultureInfo("ru");
            }
            else
            {
                newCulture = CultureInfo.GetCultureInfo("en-US");
            }

            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = newCulture;

            if (player.PlayerId == user.UserId)
            {
                labelName.Content = player.PlayerName;
                labelMoney.Content = player.PlayerMoney;
                convertMoneyToMoneyCode(labelMoney);
                labelDonation.Content = player.PlayerDonation;
                convertMoneyToMoneyCode(labelDonation);
            }

            taxes = new Taxes();
            taxes.PlayerId = player.PlayerId;

            /////////////////////////////////////////////////////////
            storage = StorageModel.GetPlayerStorage(player);

            peasants = PeasantModel.GetPeasantsInfo(player, peasants);
            sliderTaxes.IsSnapToTickEnabled = true;

            taxes = TaxesModel.GetTaxesInfo(taxes);
            sliderTaxes.Value = taxes.TaxValue;

            List<Manufacture> manufactures = ManufactureModel.GetManufactureInfo(player);

            prodRatioValue.Content = (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)).ToString();

            //storage.PlayerWood += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            //storage.PlayerStone += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            //storage.PlayerFood += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[2].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[2].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            player.PlayerMoney += Convert.ToInt32((DateTime.UtcNow.Subtract(taxes.TaxSaveDateTime).TotalSeconds / 3600) * taxes.TaxMoneyHour);

            player = PlayerModel.UpdatePlayerMoney(player);
            TaxesModel.SaveTaxes(taxes);
            labelMoney.Content = player.PlayerMoney;
            convertMoneyToMoneyCode(labelMoney);


            //Thread myThread = new Thread(new ThreadStart(UpdateInfo));
            //myThread.Start(); // запускаем поток
            UpdateMainWindowInfoAsync(); 

            lands = new List<Land>();
            paths = new List<Path>();

            for (int i = 0; i < landsCount; i++)
            {
                lands.Add(new Land());
            }

            countries = new List<Country>();

            for (int i = 0; i < CountryModel.SelectLastIdOfStates(); i++)
            {
                countries.Add(new Country());
            }

            lands = LandModel.GetLandsInfo(lands);
            countries = CountryModel.GetCountriesInfo(countries);


            

            LoadWarsOnMap();
            setFlag();

            //////////////////
            /// ГОЛОД ТУТ ////
            //////////////////
            ConsumptionLogic.ConsumptionCount(player, storage);
            lblConsumption.Content = ConsumptionLogic.CountFunction(player, 1);
            lblFoodLeft.Content = storage.Food;
            //ConsumptionLogic.ConsumptionCountAsync(player, storage);
            //////////////////           
            DailyBonusCount(player);
            ServerDispatcherTimer();


            settingsGrid.Visibility = Visibility.Hidden;
            settingsGridBorder.Visibility = Visibility.Hidden;
            btnShowLandGrid.Visibility = Visibility.Hidden;
            btnShowLeaderGrid.Visibility = Visibility.Hidden;
            BtnShowTaxesGrid.Visibility = Visibility.Hidden;
            BtnShowDailyBonusGrid.Visibility = Visibility.Hidden;
            BtnShowConsumptionGrid.Visibility = Visibility.Hidden;

            GetWorldLeader();

        }

        public async void UpdateMainWindowInfoAsync()
        {
            await Task.Run(() => UpdateInfoAsync());
        }

        private void ImageManufacture_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseUnusedWindows();
            openedWindow = new ManufactureWindow(player, manufacture, storage);
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;
        }

        private void LandImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseUnusedWindows();
            openedWindow = new LandWindow(user, player);
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;
        }

        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            CloseUnusedWindows();
            openedWindow = new AuthorisationWindow();
            openedWindow.Show();
            this.Close();
        }

        private void reload_button_Click(object sender, RoutedEventArgs e)
        {
            //CloseUnusedWindows();
            //openedWindow = new MainWindow(user);
            //openedWindow.Show();
            //this.Close();
            MainWindow_Loaded(sender, e);
        }

        private void OpenStorage(Player player, User user)
        {
            CloseUnusedWindows();
            openedWindow = new StorageWindow(player);
            PlayerModel.UpdatePlayerExpAndLvl(player);
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;
        }

        private void recruitImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            storage = StorageModel.GetPlayerStorage(player);
            equipment = EquipmentModel.GetPlayerEquipment(player, equipment);

            CloseUnusedWindows();
            openedWindow = new RecruitWindow(player, equipment);
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;
        }

        private void buttonTop_Click(object sender, RoutedEventArgs e)
        {
            CloseUnusedWindows();
            openedWindow = new RatingWindow(this, player, playerEntrance, user, army);
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;
        }

        private void buttonChat_Click(object sender, RoutedEventArgs e)
        {
            CloseUnusedWindows();
            openedWindow = new ChatWindow(player);
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;
        }

        private void marketImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            storage = StorageModel.GetPlayerStorage(player);
            market = MarketModel.GetMarketInfo(player, market);

            CloseUnusedWindows();
            openedWindow = new MarketWindow(storage, market, player);
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;
        }

        private void CountryImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseUnusedWindows();
            Land land = LandModel.GetLandInfo(player.PlayerCurrentRegion);
            if (land.CountryId != 0)
            {
                openedWindow = new CountryWindow(player);
                openedWindow.Owner = this;
                openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                openedWindow.Show();
                openedWindow.Closed += FreeData;
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("This land is independent. The government has not yet been formed.");
            }
        }

        private void buyMembership_Click(object sender, RoutedEventArgs e)
        {
            CloseUnusedWindows();
            openedWindow = new MembershipWindow();
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;
        }

        private void SubmitBugTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseUnusedWindows();
            openedWindow = new SubmitBugWindow(player.PlayerName);
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;
        }



        private void buttonProfile_Click(object sender, RoutedEventArgs e)
        {
            CloseUnusedWindows();
            openedWindow = new ProfileWindow(player, user);
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ProfileWindow window = openedWindow as ProfileWindow;
            window.NameChanged += ProfileWindow_NameChanged;
            window.Show();
            window.Closed += FreeData;
        }

        void ProfileWindow_NameChanged(string name)
        {
            labelName.Content = name;
        }

        private void OpenAuction_Click(object sender, RoutedEventArgs e)
        {
            CloseUnusedWindows();
            openedWindow = new AuctionWindow(player);
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;
        }

        private void ButtonMailbox_Click(object sender, RoutedEventArgs e)
        {
            CloseUnusedWindows();
            openedWindow = new MailboxWindow(player.PlayerName);
            openedWindow.Owner = this;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
            openedWindow.Closed += FreeData;

            var messagesList = LandConquestYD.YDMessaging.GetAllMessagesName(player.PlayerName);
            int counter = 0;
            foreach(var messageName in messagesList)
            {
                counter++;
                string messageText = LandConquestYD.YDContext.ReadResource("Messages/" + messageName);
                MessageReceiverDialog.ShowReceivedMessage(messageName.Remove(0, 6).Replace("mail.txt", "").Replace(player.PlayerName, "").Replace("_", ""), messageText, counter);
                LandConquestYD.YDMessaging.DeleteReadedMessage(messageName);
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            LandConquestYD.YDContext.DeleteConnectionId();
            Environment.Exit(0);
        }


        //private void UpdateInfo()
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            Thread.Sleep(10000);
        //            taxes = TaxesModel.GetTaxesInfo(taxes);
        //            await MainWindow_Loaded(this.sender, RoutedEventArgs e);
        //            player.PlayerMoney += Convert.ToInt32((DateTime.UtcNow.Subtract(taxes.TaxSaveDateTime).TotalSeconds / 3600) * taxes.TaxMoneyHour);

        //            player = PlayerModel.UpdatePlayerMoney(player);
        //            TaxesModel.SaveTaxes(taxes);
        //            Dispatcher.BeginInvoke(new ThreadStart(delegate { labelMoney.Content = player.PlayerMoney; convertMoneyToMoneyCode(labelMoney); }));
        //            lands = LandModel.GetLandsInfo(lands);
        //            Dispatcher.BeginInvoke(new ThreadStart(delegate { RedrawGlobalMap(); }));
        //            Console.WriteLine("End of loop");
        //        }
        //        catch { }
        //        labelMoney.Content = player.PlayerMoney;
        //    }
        //}
        private async Task UpdateInfoAsync()
        {
            var connection = LandConquestDB.DbContext.GetTempSqlConnection();
            while (true)
            {
                try
                {
                    connection.Open();
                    await Task.Delay(10000);
                    taxes = TaxesModel.GetTaxesInfo(taxes, connection);
                    player.PlayerMoney += Convert.ToInt32((DateTime.UtcNow.Subtract(taxes.TaxSaveDateTime).TotalSeconds / 3600) * taxes.TaxMoneyHour);

                    player = PlayerModel.UpdatePlayerMoney(player);
                    TaxesModel.SaveTaxes(taxes, connection);
                    lands = LandModel.GetLandsInfo(lands, connection);
                    await Dispatcher.BeginInvoke(new CrossAppDomainDelegate(delegate { labelMoney.Content = player.PlayerMoney; convertMoneyToMoneyCode(labelMoney); RedrawGlobalMap(); LoadWarsOnMap();}));
                    Console.WriteLine("End of loop");
                    connection.Close();
                }
                catch { }
            }
        }

        private enum Manufactures : int
        {
            wood = 0,
            stone = 1,
            food = 2
        }

        private void ImageStorage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            storage = StorageModel.GetPlayerStorage(player);
            List<Manufacture> manufactures = ManufactureModel.GetManufactureInfo(player);
            List<Manufacture> playerLandManufactures = ManufactureModel.GetPlayerLandManufactureInfo(player);
                     
            for(int i=0; i <3; i++)
            {
                WarehouseModel.AddItems(manufactures[i].WarehouseId, ((Manufactures) i).ToString(), Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[i].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[i].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[i].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[i].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                player = CheckLvlChange(player);
            }

            //base manufactures 
            //storage.PlayerWood += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            //WarehouseModel.AddItems(manufactures[0].WarehouseId, "wood", Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
            //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            //player = CheckLvlChange(player);

            ////storage.PlayerStone += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            //WarehouseModel.AddItems(manufactures[1].WarehouseId, "stone", Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
            //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            //player = CheckLvlChange(player);

            ////storage.PlayerFood += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[2].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[2].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            //WarehouseModel.AddItems(manufactures[2].WarehouseId, "food", Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[2].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[2].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
            //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[2].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[2].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            //player = CheckLvlChange(player);
            ////land manufactures

            ////first land manufacture
            
            bool f = true;
            if (playerLandManufactures.Count == 0)
            {
                f = false;
                playerLandManufactures.Add(new Manufacture());
                playerLandManufactures.Add(new Manufacture());
                playerLandManufactures[0].ManufactureProdStartTime = DateTime.UtcNow;
                playerLandManufactures[0].ManufactureProductsHour = 0;

                playerLandManufactures[1].ManufactureProdStartTime = DateTime.UtcNow;
                playerLandManufactures[1].ManufactureProductsHour = 0;
            }

            string firstResource = "";
            switch (playerLandManufactures[0].ManufactureType)
            {
                case 4:
                    {
                        ////storage.PlayerIron += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //WarehouseModel.AddItems(playerLandManufactures[0].WarehouseId, "iron", Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                        //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //player = CheckLvlChange(player);
                        firstResource = "iron";
                        break;
                    }
                case 5:
                    {
                        ////storage.PlayerGoldOre += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //WarehouseModel.AddItems(playerLandManufactures[0].WarehouseId, "gold_ore", Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                        //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //player = CheckLvlChange(player);
                        firstResource = "gold_ore";
                        break;
                    }
                case 6:
                    {
                        ////storage.PlayerCopper += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //WarehouseModel.AddItems(playerLandManufactures[0].WarehouseId, "copper", Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                        //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //player = CheckLvlChange(player);
                        firstResource = "copper";
                        break;
                    }
                case 7:
                    {
                        ////storage.PlayerGems += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //WarehouseModel.AddItems(playerLandManufactures[0].WarehouseId, "gems", Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                        //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //player = CheckLvlChange(player);
                        firstResource = "gems";
                        break;
                    }
                case 8:
                    {
                        ////storage.PlayerLeather += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //WarehouseModel.AddItems(playerLandManufactures[0].WarehouseId, "leather", Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                        //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //player = CheckLvlChange(player);
                        firstResource = "leather";
                        break;
                    }
            }

            if (firstResource != "")
            {
                WarehouseModel.AddItems(playerLandManufactures[0].WarehouseId, firstResource, Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                player = CheckLvlChange(player);
            }

            //second land manufacture
            string secondResource = "";
            switch (playerLandManufactures[1].ManufactureType)
            {
                case 4:
                    {
                        ////storage.PlayerIron += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //WarehouseModel.AddItems(playerLandManufactures[1].WarehouseId, "iron", Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                        //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //player = CheckLvlChange(player);
                        secondResource = "iron";
                        break;
                    }
                case 5:
                    {
                        ////storage.PlayerGoldOre += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //WarehouseModel.AddItems(playerLandManufactures[1].WarehouseId, "gold_ore", Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                        //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //player = CheckLvlChange(player);
                        secondResource = "gold_ore";
                        break;
                    }
                case 6:
                    {
                        //storage.PlayerCopper += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //WarehouseModel.AddItems(playerLandManufactures[1].WarehouseId, "copper", Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                        //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //player = CheckLvlChange(player);
                        secondResource = "copper";
                        break;
                    }
                case 7:
                    {
                        //Console.WriteLine("suda!!!!");
                        ////storage.PlayerGems += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //WarehouseModel.AddItems(playerLandManufactures[1].WarehouseId, "gems", Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                        //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //player = CheckLvlChange(player);
                        secondResource = "gems";
                        break;
                    }
                case 8:
                    {
                        ////storage.PlayerLeather += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //WarehouseModel.AddItems(playerLandManufactures[1].WarehouseId, "leather", Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                        //player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        //player = CheckLvlChange(player);
                        secondResource = "leather";
                        break;
                    }
            }

            if (secondResource != "")
            {
                WarehouseModel.AddItems(playerLandManufactures[1].WarehouseId, secondResource, Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))));
                player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                player = CheckLvlChange(player);
            }

            //EXP and PB + Update Storage
            PbExp.Maximum = Math.Pow(player.PlayerLvl, 2) * 500;
            PbExp.Value = player.PlayerExp;

            Console.WriteLine(Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))) + " tut");

            StorageModel.UpdateStorage(player, storage);

            ManufactureModel.UpdateDateTimeForManufacture(manufactures, player);
            if (f) ManufactureModel.UpdateDateTimeForPlayerLandManufacture(playerLandManufactures, player);

            OpenStorage(player, user);
        }

        private void SaveTaxes_Click(object sender, RoutedEventArgs e)
        {
            taxes.TaxValue = Convert.ToInt32(sliderTaxes.Value);
            taxes.TaxMoneyHour = taxes.TaxValue * peasants.PeasantsCount;
            TaxesModel.SaveTaxes(taxes);
        }

        private void sliderTaxes_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            prodRatioValue.Content = (1 + (1 - Convert.ToDouble(sliderTaxes.Value) / 5)).ToString();
        }

        private void PathEnterHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Path senderPath = (Path)sender;

                System.Windows.Media.Color color = ((SolidColorBrush)senderPath.Fill).Color;
                color.R -= 10;
                color.G -= 10;
                color.B -= 10;

                senderPath.Fill = new SolidColorBrush(color);

                //HatchBrush hBrush = new HatchBrush(HatchStyle.Horizontal, System.Drawing.Color.Red);
                //Graphics.FromImage(hBrush, 0, 0, 100, 60);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }

        private void PathLeaveHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Path senderPath = (Path)sender;

                System.Windows.Media.Color color = ((SolidColorBrush)senderPath.Fill).Color;
                color.R += 10;
                color.G += 10;
                color.B += 10;

                senderPath.Fill = new SolidColorBrush(color);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }

        private void PathDownHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Path senderPath = (Path)sender;
                land = new Land();

                land = lands.ElementAt(Convert.ToInt32(senderPath.Name.Replace("Land", "")) - 1);
                lblLandNameOnGrid.Content = land.LandName;


                for (int i = 0; i < countries.Count; i++)
                {
                    if (land.CountryId == countries[i].CountryId)
                    {
                        lblCountryNameOnGrid.Content = countries[i].CountryName;
                        break;
                    }
                }

                //flagXY[0] = Convert.ToInt32(senderPath.Data.Bounds.Left + senderPath.Data.Bounds.Width / 2 + Convert.ToInt32(GlobalMap.Margin.Left));
                //flagXY[1] = Convert.ToInt32(senderPath.Data.Bounds.Top + senderPath.Data.Bounds.Height / 2 + Convert.ToInt32(GlobalMap.Margin.Top));
                //flagXY[2] = Convert.ToInt32(senderPath.Data.Bounds.Right + senderPath.Data.Bounds.Width / 2 + Convert.ToInt32(GlobalMap.Margin.Right));
                //flagXY[3] = Convert.ToInt32(senderPath.Data.Bounds.Bottom + senderPath.Data.Bounds.Height / 2 + Convert.ToInt32(GlobalMap.Margin.Bottom));

                string resourceType1 = "";
                switch (land.ResourceType1)
                {
                    case 4:
                        {
                            //lblLandResource1NameOnGrid.Content = "iron";
                            //imgLandResource1ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/iron.png", UriKind.Relative));
                            resourceType1 = "iron";
                            break;
                        }
                    case 5:
                        {
                            //lblLandResource1NameOnGrid.Content = "gold_ore";
                            //imgLandResource1ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/gold_ore.png", UriKind.Relative));
                            resourceType1 = "gold_ore";
                            break;
                        }
                    case 6:
                        {
                            //lblLandResource1NameOnGrid.Content = "copper";
                            //imgLandResource1ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/copper.png", UriKind.Relative));
                            resourceType1 = "copper";
                            break;
                        }
                    case 7:
                        {
                            //lblLandResource1NameOnGrid.Content = "gems";
                            //imgLandResource1ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/gems.png", UriKind.Relative));
                            resourceType1 = "gems";
                            break;
                        }
                    case 8:
                        {
                            //lblLandResource1NameOnGrid.Content = "leather";
                            //imgLandResource1ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/leather.png", UriKind.Relative));
                            resourceType1 = "leather";
                            break;
                        }
                }

                if (resourceType1 != "")
                {
                    lblLandResource1NameOnGrid.Content = resourceType1;
                    imgLandResource1ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/" + resourceType1 + ".png", UriKind.Relative));
                }

                string resourceType2 = "";
                switch (land.ResourceType2)
                {
                    case 4:
                        {
                            //lblLandResource2NameOnGrid.Content = "iron";
                            //imgLandResource2ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/iron.png", UriKind.Relative));
                            resourceType2 = "iron";
                            break;
                        }
                    case 5:
                        {
                            //lblLandResource2NameOnGrid.Content = "gold_ore";
                            //imgLandResource2ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/gold_ore.png", UriKind.Relative));
                            resourceType2 = "gold_ore";
                            break;
                        }
                    case 6:
                        {
                            //lblLandResource2NameOnGrid.Content = "copper";
                            //imgLandResource2ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/copper.png", UriKind.Relative));
                            resourceType2 = "copper";
                            break;
                        }
                    case 7:
                        {
                            //lblLandResource2NameOnGrid.Content = "gems";
                            //imgLandResource2ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/gems.png", UriKind.Relative));
                            resourceType2 = "gems";
                            break;
                        }
                    case 8:
                        {
                            //lblLandResource2NameOnGrid.Content = "leather";
                            //imgLandResource2ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/leather.png", UriKind.Relative));
                            resourceType2 = "leather";
                            break;
                        }
                }
                if (resourceType2 != null)
                {
                    lblLandResource2NameOnGrid.Content = resourceType2;
                    imgLandResource2ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/" + resourceType2 + ".png", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                WarningDialogWindow.CallWarningDialogNoResult("Something went wrong while collecting manufactures \n" + ex.Message);
            }
        }



        private void PathLoadedHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Path senderPath = (Path)sender;
                paths.Add(senderPath);
                land = new Land();
                land = lands.ElementAt(Convert.ToInt32(senderPath.Name.Replace("Land", "")) - 1);


                Color color = (Color)ColorConverter.ConvertFromString(land.LandColor);

                senderPath.Fill = new SolidColorBrush(color);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }


        private void ChangeMapType(object sender, RoutedEventArgs e)
        {
            if(GlobalMap.Visibility == Visibility.Visible)
            {
                GlobalMap.Visibility = Visibility.Hidden;
                GlobalMapBtn.Visibility = Visibility.Hidden;
                ResourceMap.Visibility = Visibility.Visible;
                ResourceMapBtn.Visibility = Visibility.Visible;
            }
            else
            {
                ResourceMap.Visibility = Visibility.Hidden;
                ResourceMapBtn.Visibility = Visibility.Hidden;
                GlobalMap.Visibility = Visibility.Visible;
                GlobalMapBtn.Visibility = Visibility.Visible;
            }
                    
        }

        private void btnGoToLand_Click(object sender, RoutedEventArgs e)
        {

            peasants = PeasantModel.GetPeasantsInfo(player, peasants);
            List<int> peasantsFree = PlayerModel.DeletePlayerManufactureLandData(peasants, player);
            List<Manufacture> landManufactures = ManufactureModel.GetLandManufactureInfo(player);

            ManufactureModel.UpdateLandManufacturesWhenMove(peasantsFree, landManufactures);
            //peasants.PeasantsCount = peasants.PeasantsCount + peasantsFree[0] + peasantsFree[1];
            PeasantModel.UpdatePeasantsInfo(peasants);

            player = PlayerModel.UpdatePlayerLand(player, land);

            //flag.Margin = new Thickness(flagXY[0] - 69, flagXY[1] - 36, 0, 0);
            //flag.Margin = new Thickness(Convert.ToDouble(GlobalMap.Margin.Left), Convert.ToDouble(GlobalMap.Margin.Top), 0, 0);
            //flag.Stretch

            flagXY = MapModel.CenterOfLand(land.LandId);
            flag.Margin = new Thickness(flagXY[0], flagXY[1], 0, 0);

            Console.WriteLine("flag coo: " + flagXY[0] + " " + flagXY[1]);
            Console.WriteLine("map coo: " + Convert.ToInt32(GlobalMap.Margin.Left) + " " + Convert.ToInt32(GlobalMap.Margin.Top));


        }

        private void buttonEstablishaState_Click(object sender, RoutedEventArgs e)
        {
            land = LandModel.GetLandInfo(player.PlayerCurrentRegion);
            EstablishStateDialog win = new EstablishStateDialog(player, land);
            win.Owner = this;
            win.Show();
        }

        public void RedrawGlobalMap()
        {
            for (int i = 0; i < paths.Count; i++)
            {
                Color color = (Color)ColorConverter.ConvertFromString(lands[i].LandColor);

                paths[i].Fill = new SolidColorBrush(color);
            }
        }

        public Player CheckLvlChange(Player player)
        {

            while (Math.Pow(player.PlayerLvl, 2) * 500 - player.PlayerExp <= 0)
            {
                player.PlayerLvl += 1;

                Level.Content = player.PlayerLvl.ToString();
            }

            PbExp.Maximum = player.PlayerLvl * 2 * 500;
            PbExp.Value = player.PlayerExp;

            return player;
        }

        private void btnHideLandGrid_Click(object sender, RoutedEventArgs e)
        {
            Country_characters.Visibility = Visibility.Hidden;
            btnHideLandGrid.Visibility = Visibility.Hidden;
            Border_Country_characters.Visibility = Visibility.Hidden;
            btnShowLandGrid.Visibility = Visibility.Visible;
        }

        private void btnShowLandGrid_Click(object sender, RoutedEventArgs e)
        {
            Country_characters.Visibility = Visibility.Visible;
            btnHideLandGrid.Visibility = Visibility.Visible;
            Border_Country_characters.Visibility = Visibility.Visible;
            btnShowLandGrid.Visibility = Visibility.Hidden;
        }

        private void btnHideLeaderGrid_Click(object sender, RoutedEventArgs e)
        {
            worldLeader.Visibility = Visibility.Hidden;
            worldLeaderBorder.Visibility = Visibility.Hidden;
            btnShowLeaderGrid.Visibility = Visibility.Visible;
        }

        private void btnShowLeaderGrid_Click(object sender, RoutedEventArgs e)
        {
            worldLeader.Visibility = Visibility.Visible;
            worldLeaderBorder.Visibility = Visibility.Visible;
            btnShowLeaderGrid.Visibility = Visibility.Hidden;
        }

        private void GetWorldLeader()
        {
            if (lands.Count > 0)
            {
                int check_count = 0;
                var counts = new Dictionary<int, int>();
                foreach (var land in lands)
                {
                    int count;
                    if (land.CountryId != 0)
                    {
                        counts.TryGetValue(land.CountryId, out count);
                        count++;
                        check_count++;
                        counts[land.CountryId] = count;
                    }
                }
                int mostCommonNumber = 0, occurrences = 0;
                foreach (var pair in counts)
                {
                    if (pair.Value > occurrences)
                    {
                        occurrences = pair.Value;
                        mostCommonNumber = pair.Key;
                    }
                }
                if (check_count != 0)
                {
                    lblWorldLeader.Content = PlayerModel.GetPlayerNameById(CountryModel.GetCountryRuler(mostCommonNumber));
                }
            }
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            if (settingsGrid.Visibility == Visibility.Hidden)
            {
                settingsGrid.Visibility = Visibility.Visible;
                settingsGridBorder.Visibility = Visibility.Visible;
            }
            else
            {
                settingsGrid.Visibility = Visibility.Hidden;
                settingsGridBorder.Visibility = Visibility.Hidden;
            }
        }
        private void test2_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine(land.LandName);
            LandModel.AddLandManufactures(land);
        }

        private void buttonStartBattle_Click(object sender, RoutedEventArgs e)
        {
            //WarResultWindow warResultWindow = new WarResultWindow(player);
            //warResultWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //warResultWindow.Owner = this;
            //warResultWindow.Show();
        }

        public void LoadWarsOnMap()
        {
            wars = new List<War>();

            for (int i = 0; i < WarModel.SelectLastIdOfWars(); i++)
            {
                wars.Add(new War());
            }

            wars = WarModel.GetWarsInfo(wars);

            for (int i = 0; i < SymbalLayer.Children.Count; i++)
            {
                if (SymbalLayer.Children[i] != flag)
                {
                    SymbalLayer.Children.RemoveAt(1); // flag is always first [0];
                }
            }

            int[] landCenter = new int[1];
            marginsOfWarButtons = new Thickness[wars.Count];

            for (int i = 0; i < wars.Count; i++)
            {
                Line warLine = new Line();
                SymbalLayer.Children.Add(warLine);
                Console.WriteLine(wars[i].WarId + ' ' + wars[i].LandAttackerId + ' ' + wars[i].LandDefenderId);
                Console.WriteLine(SymbalLayer.Children.Count);
                landCenter = MapModel.CenterOfLand(wars[i].LandAttackerId);
                warLine.X1 = landCenter[0] + 15;
                warLine.Y1 = landCenter[1] + 30;
                landCenter = MapModel.CenterOfLand(wars[i].LandDefenderId);
                warLine.X2 = landCenter[0] + 15;
                warLine.Y2 = landCenter[1] + 30;
                warLine.Stroke = System.Windows.Media.Brushes.Black;
                warLine.StrokeThickness = 1;

                Image btnWar = new Image();
                btnWar.Height = 25;
                btnWar.Width = 25;
                btnWar.Source = new BitmapImage(new Uri("/Pictures/warSymbal.png", UriKind.Relative));
                btnWar.Margin = new Thickness(warLine.X1 + (warLine.X2 - warLine.X1) / 2 - 12, warLine.Y1 + (warLine.Y2 - warLine.Y1) / 2 - 15, 0, 0);
                btnWar.MouseLeftButtonDown += btnWar_MouseLeftButtonDown;
                btnWar.MouseEnter += btnWar_MouseEnter;
                btnWar.MouseLeave += btnWar_MouseLeave;

                marginsOfWarButtons[i] = btnWar.Margin;

                SymbalLayer.Children.Add(btnWar);
            }
        }

        private void btnWar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // переход в войну
            for (int j = 0; j < wars.Count; j++)
            {
                if (((Image)sender).Margin == marginsOfWarButtons[j])
                {
                    Console.WriteLine("Ключ войны = " + wars[j].WarId);

                    WAR = new War();
                    WAR.WarId = wars[j].WarId;
                    
                    WarPreviewDialogWindow dialogWindow = new WarPreviewDialogWindow(player, WAR);
                    dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    dialogWindow.Owner = this;
                    dialogWindow.Show();
                }
            }
        }

        private void btnWar_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void btnWar_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        public void convertMoneyToMoneyCode(Label label)
        {
            int k = 0;
            while (Convert.ToInt32(label.Content) > 1000)
            {
                label.Content = Convert.ToInt32(label.Content) / (float)1000;
                k++;
            }

            label.Content = Convert.ToString(Math.Round(Convert.ToDouble(label.Content), 2));

            for (int i = 0; i < k; i++)
            {
                label.Content += "k";
            }
        }

        public void setFlag()
        {
            flagXY = MapModel.CenterOfLand(player.PlayerCurrentRegion);
            flag.Margin = new Thickness(flagXY[0], flagXY[1], 0, 0);
        }

        private void FreeData(object data, EventArgs e)
        {
            openedWindow = null;
            GC.Collect();
        }

        private void CloseUnusedWindows()
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window != this)
                    window.Close();
            }
        }

        private void buttonCollapse_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState = WindowState.Minimized;
        }

        private void mainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            try
            {   // если убрать try, выпадет ошибка, когда нажимаешь на кнопку ОК в диалоге с ворнингом.
                this.DragMove();
            }
            catch
            {

            }
        }

        private void checkBoxFs_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxFs.IsChecked == true)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void checkBoxMusic_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer sound = new SoundPlayer(Properties.Resources.MainTheme);
            if (checkBoxMusic.IsChecked == true)
            {
                sound.PlayLooping();
            }
            else
            {
                sound.Stop();
            }
        }

        private void BtnHideTaxesGrid_Click(object sender, RoutedEventArgs e)
        {
            TaxesGrid.Visibility = Visibility.Hidden;
            TaxesBorder.Visibility = Visibility.Hidden;
            BtnShowTaxesGrid.Visibility = Visibility.Visible;
        }

        private void BtnShowTaxesGrid_Click(object sender, RoutedEventArgs e)
        {
            TaxesGrid.Visibility = Visibility.Visible;
            TaxesBorder.Visibility = Visibility.Visible;
            BtnShowTaxesGrid.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// /////////////////////////////////// DAILY BONUS //////////////////////////////////////////////////////////
        /// </summary>

        private void DailyButton_Click(object sender, RoutedEventArgs e)
        {
            var nextDailyBonus = DailyBonusModel.GetNextDailyBonusTime(player);
            if (nextDailyBonus <= DateTime.UtcNow)
            {
                DailyBonusModel.UpdateNextDailyBonusTime(player);
                RewardLogic.GiveDailyBonus(player);
                DailyBonusCount(player);
            }
        }

        private void DailyBonusCount(Player player)
        {
            var nextDailyBonus = DailyBonusModel.GetNextDailyBonusTime(player);
            if (nextDailyBonus == DateTime.MinValue)
            {
                DailyBonusModel.SetFirstDailyBonusTime(player);
                DailyButton.IsEnabled = true;
                DailyButton.Content = $"{Languages.Resources.LocButtonClaim_Content}";
                lblNextDaily.Content = "Avaible";
            }
            else if (nextDailyBonus <= DateTime.UtcNow)
            {
                DailyButton.IsEnabled = true;
                DailyButton.Content = $"{Languages.Resources.LocButtonClaim_Content}";
                lblNextDaily.Content = "Avaible";
            }
            else
            {
                DailyButton.IsEnabled = false;
                DailyButton.Content = $"{Languages.Resources.LocButtonClaimed_Content}";
                lblNextDaily.Content = nextDailyBonus;
            }
        }

        private void BtnHideDailyBonusGrid_Click(object sender, RoutedEventArgs e)
        {
            DailyBonusGrid.Visibility = Visibility.Hidden;
            DailyBonusBorder.Visibility = Visibility.Hidden;
            BtnShowDailyBonusGrid.Visibility = Visibility.Visible;
        }

        private void BtnShowDailyBonusGrid_Click(object sender, RoutedEventArgs e)
        {
            DailyBonusGrid.Visibility = Visibility.Visible;
            DailyBonusBorder.Visibility = Visibility.Visible;
            BtnShowDailyBonusGrid.Visibility = Visibility.Hidden;
        }

        private void BtnShowConsumptionGrid_Click(object sender, RoutedEventArgs e)
        {
            consumptionGrid.Visibility = Visibility.Visible;
            consumptionBorder.Visibility = Visibility.Visible;
            BtnShowConsumptionGrid.Visibility = Visibility.Hidden;
        }

        private void btnHideConsumptionGrid_Click(object sender, RoutedEventArgs e)
        {
            consumptionGrid.Visibility = Visibility.Hidden;
            consumptionBorder.Visibility = Visibility.Hidden;
            BtnShowConsumptionGrid.Visibility = Visibility.Visible;
        }

        private void ServerDispatcherTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += serverTimer_Tick;
            timer.Start();
        }

        private void serverTimer_Tick(object sender, EventArgs e)
        {
            LabelServerTime.Content = DateTime.UtcNow.ToLongTimeString();
        }

    }
}
