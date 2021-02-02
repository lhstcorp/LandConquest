using LandConquestDB.Entities;
using LandConquestDB.Enums;
using LandConquestDB.Models;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LandConquest.DialogWIndows
{
    public partial class ManufactureUpgradeDialog : Window
    {
        private Player player;
        private PlayerStorage storage;
        private Manufacture manufacture;
        private PlayerStorage resourcesNeed;
        private StorageModel storageModel;
        public ManufactureUpgradeDialog(PlayerStorage _storage, Manufacture _manufacture, Player _player)
        {
            InitializeComponent();
            storage = _storage;
            player = _player;
            manufacture = _manufacture;
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            //resourcesNeed = ManufactureModel.GetInfoAboutResourcesForUpdate(manufacture);
            resourcesNeed = new PlayerStorage();
            resourcesNeed.PlayerWood = Convert.ToInt32(((int)ManufacturesLvlEnum.Level.Wood) * Math.Pow(1.25,manufacture.ManufactureLevel));
            resourcesNeed.PlayerStone = Convert.ToInt32(((int)ManufacturesLvlEnum.Level.Stone) * Math.Pow(1.25, manufacture.ManufactureLevel));

            WoodNeed.Content = resourcesNeed.PlayerWood;
            StoneNeed.Content = resourcesNeed.PlayerStone;

            ManufactureLvl.Content = manufacture.ManufactureLevel;
        }

        private void BtnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            if (storage.PlayerWood >= resourcesNeed.PlayerWood && storage.PlayerStone >= resourcesNeed.PlayerStone)
            {
                ManufactureModel.UpgradeManufacture(manufacture);
                storage.PlayerWood -= resourcesNeed.PlayerWood;
                storage.PlayerStone -= resourcesNeed.PlayerStone;

                StorageModel.UpdateStorage(player, storage);
                storage = StorageModel.GetPlayerStorage(player, storage);
                //this.Hide();
                //this.Show();
                // output
                WoodHave.Content = storage.PlayerWood;
                StoneHave.Content = storage.PlayerStone;
                //resourcesNeed = ManufactureModel.GetInfoAboutResourcesForUpdate(manufacture);
                resourcesNeed.PlayerWood = Convert.ToInt32(((int)ManufacturesLvlEnum.Level.Wood) * Math.Pow(1.5, manufacture.ManufactureLevel));
                resourcesNeed.PlayerStone = Convert.ToInt32(((int)ManufacturesLvlEnum.Level.Stone) * Math.Pow(1.5, manufacture.ManufactureLevel));

                WoodNeed.Content = resourcesNeed.PlayerWood;
                StoneNeed.Content = resourcesNeed.PlayerStone;

                manufacture = ManufactureModel.GetManufactureById(manufacture);

                ManufactureLvl.Content = manufacture.ManufactureLevel;
            }
        }
    }
}
