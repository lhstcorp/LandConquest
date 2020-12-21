using LandConquest.Entities;
using LandConquest.Models;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LandConquest.DialogWIndows
{
    public partial class ManufactureUpgradeDialog : Window
    {
        SqlConnection connection;
        Player player;
        PlayerStorage storage;
        Manufacture manufacture;
        PlayerStorage resourcesNeed;
        ManufactureModel model;
        StorageModel storageModel;
        public ManufactureUpgradeDialog(SqlConnection _connection ,PlayerStorage _storage, Manufacture _manufacture, Player _player)
        {
            InitializeComponent();
            storage = _storage;
            player = _player;
            connection = _connection;
            manufacture = _manufacture;
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            model = new ManufactureModel();
            storageModel = new StorageModel();

            switch (manufacture.ManufactureType)
            {
                case 1:
                    manufacture_image.Source = new BitmapImage(new Uri("/Pictures/sawmill.png", UriKind.Relative));
                    break;
                case 2:
                    manufacture_image.Source = new BitmapImage(new Uri("/Pictures/quarry.png", UriKind.Relative));
                    break;
                case 3:
                    manufacture_image.Source = new BitmapImage(new Uri("/Pictures/windmill.png", UriKind.Relative));
                    break;
            }

            WoodHave.Content = storage.PlayerWood;
            StoneHave.Content = storage.PlayerStone;
            resourcesNeed = model.GetInfoAboutResourcesForUpdate(connection, manufacture);

            WoodNeed.Content = resourcesNeed.PlayerWood;
            StoneNeed.Content = resourcesNeed.PlayerStone;

            ManufactureLvl.Content = manufacture.ManufactureLevel;
        }

        private void BtnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            if (storage.PlayerWood >= resourcesNeed.PlayerWood && storage.PlayerStone >= resourcesNeed.PlayerStone)
            {
                model.UpgradeManufacture(connection, manufacture);
                storage.PlayerWood -= resourcesNeed.PlayerWood;
                storage.PlayerStone -= resourcesNeed.PlayerStone;

                storageModel.UpdateStorage(connection, player, storage);
                storage = storageModel.GetPlayerStorage(player, connection, storage);
                //this.Hide();
                //this.Show();
                // output
                WoodHave.Content = storage.PlayerWood;
                StoneHave.Content = storage.PlayerStone;
                resourcesNeed = model.GetInfoAboutResourcesForUpdate(connection, manufacture);

                WoodNeed.Content = resourcesNeed.PlayerWood;
                StoneNeed.Content = resourcesNeed.PlayerStone;

                manufacture = model.GetManufactureById(manufacture, connection);

                ManufactureLvl.Content = manufacture.ManufactureLevel;
            }
        }
    }
}
