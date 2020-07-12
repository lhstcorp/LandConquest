using LandConquest.Entities;
using LandConquest.Forms;
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

namespace LandConquest.DialogWIndows
{
    /// <summary>
    /// Логика взаимодействия для ManufactureUpgradeDialog.xaml
    /// </summary>
    public partial class ManufactureUpgradeDialog : Window
    {
        SqlConnection connection;
        Player player;
        PlayerStorage storage;
        Manufacture manufacture;
        PlayerStorage resourcesNeed;
        ManufactureModel model = new ManufactureModel();
        StorageModel storageModel = new StorageModel();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
