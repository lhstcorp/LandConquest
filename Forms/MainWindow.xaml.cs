﻿using LandConquest.DialogWIndows;
using LandConquest.Models;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace LandConquest.Forms
{
    public partial class MainWindow : Window
    {
        private User user;
        private Player player;
        private Market market;
        private PlayerStorage storage;
        private PlayerEquipment equipment = new PlayerEquipment();
        private Taxes taxes;
        private Peasants peasants;
        private List<Land> lands;
        private List<Path> paths;
        private List<Country> countries;
        private List<War> wars;
        private Land land;
        private Army army;
        private Country country;
        private War WAR; //GLOBAL
        private ManufactureModel manufactureModel;
        private Thickness[] marginsOfWarButtons;
        private int[] flagXY = new int[4];
        private const int landsCount = 11;

        public MainWindow(User _user)
        {
            InitializeComponent();
            //this.Resources.Add("buttonGradientBrush", gradientBrush);
            user = _user;

            player = new Player();
            storage = new PlayerStorage();
            peasants = new Peasants();
            country = new Country();
            market = new Market();
            army = new Army();
            manufactureModel = new ManufactureModel();

            player = PlayerModel.GetPlayerInfo(_user, player);
            PbExp.Maximum = Math.Pow(player.PlayerLvl, 2) * 500;
            PbExp.Value = player.PlayerExp;
            Level.Content = player.PlayerLvl;
            Console.WriteLine(player.PlayerExp);

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

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            storage = StorageModel.GetPlayerStorage(player, storage);

            peasants = PeasantModel.GetPeasantsInfo(player, peasants);
            sliderTaxes.IsSnapToTickEnabled = true;

            taxes = TaxesModel.GetTaxesInfo(taxes);
            sliderTaxes.Value = taxes.TaxValue;

            List<Manufacture> manufactures = manufactureModel.GetManufactureInfo(player);

            prodRatioValue.Content = (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)).ToString();

            //storage.PlayerWood += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            //storage.PlayerStone += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            //storage.PlayerFood += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[2].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[2].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            player.PlayerMoney += Convert.ToInt32((DateTime.UtcNow.Subtract(taxes.TaxSaveDateTime).TotalSeconds / 3600) * taxes.TaxMoneyHour);

            player = PlayerModel.UpdatePlayerMoney(player);
            TaxesModel.SaveTaxes(taxes);
            labelMoney.Content = player.PlayerMoney;
            convertMoneyToMoneyCode(labelMoney);


            Thread myThread = new Thread(new ThreadStart(UpdateInfo));

            myThread.Start(); // запускаем поток

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


            wars = new List<War>();

            for (int i = 0; i < WarModel.SelectLastIdOfWars(); i++)
            {
                wars.Add(new War());
            }

            wars = WarModel.GetWarsInfo(wars);

            LoadWarsOnMap();

            setFlag();
        }

        private void UpdateInfo()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(10000);
                    taxes = TaxesModel.GetTaxesInfo(taxes);
                    //await MainWindow_Loaded(this.sender, RoutedEventArgs e); 
                    player.PlayerMoney += Convert.ToInt32((DateTime.UtcNow.Subtract(taxes.TaxSaveDateTime).TotalSeconds / 3600) * taxes.TaxMoneyHour);

                    player = PlayerModel.UpdatePlayerMoney(player);
                    TaxesModel.SaveTaxes(taxes);
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { labelMoney.Content = player.PlayerMoney; convertMoneyToMoneyCode(labelMoney); }));
                    lands = LandModel.GetLandsInfo(lands);
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { RedrawGlobalMap(); }));
                }
                catch { }
                //labelMoney.Content = player.PlayerMoney; 
            }
        }

        private void ImageStorage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            storage = StorageModel.GetPlayerStorage(player, storage);
            List<Manufacture> manufactures = manufactureModel.GetManufactureInfo(player);
            List<Manufacture> playerLandManufactures = manufactureModel.GetPlayerLandManufactureInfo(player);
            //base manufactures 
            storage.PlayerWood += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            player = CheckLvlChange(player);

            storage.PlayerStone += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            player = CheckLvlChange(player);

            storage.PlayerFood += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[2].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[2].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[2].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[2].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            player = CheckLvlChange(player);
            //land manufactures

            //first land manufacture
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

            switch (playerLandManufactures[0].ManufactureType)
            {
                case 4:
                    {
                        storage.PlayerIron += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player = CheckLvlChange(player);
                        break;
                    }
                case 5:
                    {
                        storage.PlayerGoldOre += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player = CheckLvlChange(player);
                        break;
                    }
                case 6:
                    {
                        storage.PlayerCopper += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player = CheckLvlChange(player);
                        break;
                    }
                case 7:
                    {
                        storage.PlayerGems += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player = CheckLvlChange(player);
                        break;
                    }
                case 8:
                    {
                        storage.PlayerLeather += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player = CheckLvlChange(player);
                        break;
                    }
            }
            //second land manufacture
            switch (playerLandManufactures[1].ManufactureType)
            {
                case 4:
                    {
                        storage.PlayerIron += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player = CheckLvlChange(player);
                        break;
                    }
                case 5:
                    {
                        storage.PlayerGoldOre += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player = CheckLvlChange(player);
                        break;
                    }
                case 6:
                    {
                        storage.PlayerCopper += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player = CheckLvlChange(player);
                        break;
                    }
                case 7:
                    {
                        Console.WriteLine("suda!!!!");
                        storage.PlayerGems += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player = CheckLvlChange(player);
                        break;
                    }
                case 8:
                    {
                        storage.PlayerLeather += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                        player = CheckLvlChange(player);
                        break;
                    }
            }

            //EXP and PB + Update Storage
            PbExp.Maximum = Math.Pow(player.PlayerLvl, 2) * 500;
            PbExp.Value = player.PlayerExp;

            Console.WriteLine(Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5))) + " tut");

            StorageModel.UpdateStorage(player, storage);

            manufactureModel.UpdateDateTimeForManufacture(manufactures, player);
            if (f) manufactureModel.UpdateDateTimeForPlayerLandManufacture(playerLandManufactures, player);


            StorageWindow storageWindow = new StorageWindow(this, player, user);

            PlayerModel.UpdatePlayerExpAndLvl(player);
            storageWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            storageWindow.Owner = this;
            storageWindow.Show();
        }

        private void ImageManufacture_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ManufactureWindow window = new ManufactureWindow(this, player, storage);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Owner = this;
            window.Show();
        }

        private void buttonCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            Environment.Exit(0);

        }

        private void reload_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow(user);
            window.Show();
            this.Close();
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

        private void recruitImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            storage = StorageModel.GetPlayerStorage(player, storage);
            equipment = EquipmentModel.GetPlayerEquipment(player, equipment);

            RecruitWindow window = new RecruitWindow(player, storage, equipment);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Owner = this;
            window.Show();

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


                switch (land.ResourceType1)
                {
                    case 4:
                        {
                            lblLandResource1NameOnGrid.Content = "iron";
                            imgLandResource1ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/iron.png", UriKind.Relative));
                            break;
                        }
                    case 5:
                        {
                            lblLandResource1NameOnGrid.Content = "gold_ore";
                            imgLandResource1ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/gold_ore.png", UriKind.Relative));
                            break;
                        }
                    case 6:
                        {
                            lblLandResource1NameOnGrid.Content = "copper";
                            imgLandResource1ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/copper.png", UriKind.Relative));
                            break;
                        }
                    case 7:
                        {
                            lblLandResource1NameOnGrid.Content = "gems";
                            imgLandResource1ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/gems.png", UriKind.Relative));
                            break;
                        }
                    case 8:
                        {
                            lblLandResource1NameOnGrid.Content = "leather";
                            imgLandResource1ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/leather.png", UriKind.Relative));
                            break;
                        }
                }

                switch (land.ResourceType2)
                {
                    case 4:
                        {
                            lblLandResource2NameOnGrid.Content = "iron";
                            imgLandResource2ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/iron.png", UriKind.Relative));
                            break;
                        }
                    case 5:
                        {
                            lblLandResource2NameOnGrid.Content = "gold_ore";
                            imgLandResource2ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/gold_ore.png", UriKind.Relative));
                            break;
                        }
                    case 6:
                        {
                            lblLandResource2NameOnGrid.Content = "copper";
                            imgLandResource2ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/copper.png", UriKind.Relative));
                            break;
                        }
                    case 7:
                        {
                            lblLandResource2NameOnGrid.Content = "gems";
                            imgLandResource2ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/gems.png", UriKind.Relative));
                            break;
                        }
                    case 8:
                        {
                            lblLandResource2NameOnGrid.Content = "leather";
                            imgLandResource2ImgOnGrid.Source = new BitmapImage(new Uri("/Pictures/leather.png", UriKind.Relative));
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
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

        private void ResourceMapBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GlobalMap.Visibility = Visibility.Hidden;
            ResourceMap.Visibility = Visibility.Visible;
        }

        private void GlobalMapBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ResourceMap.Visibility = Visibility.Hidden;
            GlobalMap.Visibility = Visibility.Visible;
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            settingsGrid.Visibility = Visibility.Hidden;
        }

        private void ExitButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AuthorisationWindow window = new AuthorisationWindow();
            window.Show();
            this.Close();

        }
        private void playMusic()
        {
            SoundPlayer sound = new SoundPlayer(Properties.Resources.MainTheme);
            sound.PlayLooping();

            //sound.SoundLocation = @"music2.wav";
            //sound.PlayLooping();

            //sound.SoundLocation = @"music3.wav";
            //sound.PlayLooping();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            playMusic();
        }

        private void btnGoToLand_Click(object sender, RoutedEventArgs e)
        {

            peasants = PeasantModel.GetPeasantsInfo(player, peasants);
            List<int> peasantsFree = PlayerModel.DeletePlayerManufactureLandData(peasants, player);
            List<Manufacture> landManufactures = manufactureModel.GetLandManufactureInfo(player);

            manufactureModel.UpdateLandManufacturesWhenMove(peasantsFree, landManufactures);
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

        private void buttonProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow(this, player, user);
            profileWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            profileWindow.Owner = this;
            profileWindow.Show();
        }

        public Player CheckLvlChange(Player player)
        {

            while (Math.Pow(player.PlayerLvl, 2) * 500 - player.PlayerExp <= 0)
            {
                player.PlayerLvl += 1;

                Level.Content = player.PlayerLvl.ToString();
                //запрос на ддобавление уровня
            }

            PbExp.Maximum = player.PlayerLvl * 2 * 500;
            PbExp.Value = player.PlayerExp;

            return player;
        }

        private void btnHideLandGrid_Click(object sender, RoutedEventArgs e)
        {
            Country_characters.Visibility = Visibility.Hidden;
        }

        private void btnShowLandGrid_Click(object sender, RoutedEventArgs e)
        {
            Country_characters.Visibility = Visibility.Visible;
        }

        private void buttonSettings_MouseEnter(object sender, MouseEventArgs e)
        {
            settingsGrid.Visibility = Visibility.Visible;
        }

        private void settingsGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            settingsGrid.Visibility = Visibility.Hidden;
        }

        private void buttonTop_Click(object sender, RoutedEventArgs e)
        {
            RatingWindow ratingWindow = new RatingWindow(this, player, user, army);
            ratingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ratingWindow.Owner = this;
            ratingWindow.Show();
        }

        private void test2_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine(land.LandName);
            LandModel.AddLandManufactures(land);
        }

        private void buttonChat_Click(object sender, RoutedEventArgs e)
        {
            ChatWindow chatWindow = new ChatWindow(player);
            chatWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            chatWindow.Owner = this;
            chatWindow.Show();
        }

        private void marketImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            storage = StorageModel.GetPlayerStorage(player, storage);
            market = MarketModel.GetMarketInfo(player, market);

            MarketWindow mWindow = new MarketWindow(this, storage, market, player);
            mWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //window.Owner = this;
            mWindow.Show();
        }

        private void CountryImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CountryWindow win = new CountryWindow(player);
            win.Show();
        }

        private void LandImage_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void buttonStartBattle_Click(object sender, RoutedEventArgs e)
        {
            WarResultWindow warResultWindow = new WarResultWindow(player);
            warResultWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            warResultWindow.Owner = this;
            warResultWindow.Show();
        }

        public void LoadWarsOnMap()
        {
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
                    //DeclareWar(null, e);
                    WarModel.EnterInWar(WAR, player);
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

        private void OpenAuction_Click(object sender, RoutedEventArgs e)
        {
            AuctionWindow auctionWindow = new AuctionWindow(player);
            auctionWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            auctionWindow.Owner = this;
            auctionWindow.Show();
        }

        private void buyCoins_Click(object sender, RoutedEventArgs e)
        {
            BalanceReplenishmentDialog dialog = new BalanceReplenishmentDialog(player);
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.Owner = this;
            dialog.Show();
        }

        public void convertMoneyToMoneyCode(Label label)
        {
            int k = 0;
            while (Convert.ToInt32(label.Content) > 1000)
            {
                label.Content = (float)Convert.ToInt32(label.Content) / (float)1000;
                k++;
            }

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

        private void buyMembership_Click(object sender, RoutedEventArgs e)
        {
            MembershipWindow window = new MembershipWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Owner = this;
            window.Show();
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
                               // dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
         "Portable Network Graphic (*.png)|*.png";   // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                image.Source = new BitmapImage(new Uri(dlg.FileName));
            }

            //PlayerModel.UpdatePlayerImage(player);
        }

        private void CoffersImage_MouseDown(object sender, RoutedEventArgs e)
        {
            CoffersWindow win = new CoffersWindow(player);
            win.Show();

        }
    }
}
