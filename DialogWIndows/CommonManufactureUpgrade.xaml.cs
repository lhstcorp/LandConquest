using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LandConquest.DialogWIndows
{
    public partial class CommonManufactureUpgrade : Window
    {
        private enum Level : int
        {
            Wood = 225,
            Stone = 250,
            Iron = 155,
            Copper = 50,
            Money = 100,
            GoldOre = 10
        }

        private Player player;
        private PlayerStorage storage;
        private Manufacture manufacture;
        private PlayerStorage resourcesNeed;
        private StorageModel storageModel;
        public CommonManufactureUpgrade(Manufacture _manufacture, Player _player)
        {
            InitializeComponent();           
            player = _player;
            manufacture = _manufacture;
            
            Loaded += Window_Loaded;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            storageModel = new StorageModel();
            storage = StorageModel.GetPlayerStorage(player);
            manufacture = ManufactureModel.GetLandManufactureById(manufacture);

            switch (manufacture.ManufactureType)
            {                
                case 1:
                    manufacture_image.Source = new BitmapImage(new Uri("/Pictures/copper_quarry.png", UriKind.Relative));
                    break;
                case 2:
                    manufacture_image.Source = new BitmapImage(new Uri("/Pictures/iron_quarry.png", UriKind.Relative));
                    break;
                case 3:
                    manufacture_image.Source = new BitmapImage(new Uri("/Pictures/gold_ore.png", UriKind.Relative));
                    break;
                case 4:
                    manufacture_image.Source = new BitmapImage(new Uri("/Pictures/gems.png", UriKind.Relative));
                    break;
                case 5:
                    manufacture_image.Source = new BitmapImage(new Uri("/Pictures/leather.png", UriKind.Relative));
                    break;

            }

            WoodHave.Content = storage.Wood;
            StoneHave.Content = storage.Stone;
            IronHave.Content = storage.Iron;
            CopperHave.Content = storage.Copper;
            GoldHave.Content = storage.GoldOre;

            resourcesNeed = new PlayerStorage();
            resourcesNeed.Wood = Convert.ToInt32(((int)Level.Wood) * Math.Pow(1.25, manufacture.ManufactureLvl));
            resourcesNeed.Stone = Convert.ToInt32(((int)Level.Stone) * Math.Pow(1.25, manufacture.ManufactureLvl));
            resourcesNeed.Iron = Convert.ToInt32(((int)Level.Iron) * Math.Pow(1.25, manufacture.ManufactureLvl));
            resourcesNeed.GoldOre = Convert.ToInt32(((int)Level.GoldOre) * Math.Pow(1.25, manufacture.ManufactureLvl));
            resourcesNeed.Copper = Convert.ToInt32(((int)Level.Copper) * Math.Pow(1.25, manufacture.ManufactureLvl));



            WoodNeed.Content = resourcesNeed.Wood;
            StoneNeed.Content = resourcesNeed.Stone;
            IronNeed.Content = resourcesNeed.Iron;
            GoldNeed.Content = resourcesNeed.GoldOre;
            CopperNeed.Content = resourcesNeed.Copper;



            ManufactureLvl.Content = manufacture.ManufactureLvl;
        }

        private void BtnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            
            if (storage.Wood >= resourcesNeed.Wood && storage.Stone >= resourcesNeed.Stone && storage.Iron >= resourcesNeed.Iron && storage.GoldOre >= resourcesNeed.GoldOre && storage.Copper >= resourcesNeed.Copper)
            {

                ManufactureModel.UpgradeLandManufactureLvl(manufacture);
                storage.Wood -= resourcesNeed.Wood;
                storage.Stone -= resourcesNeed.Stone;
                storage.Iron -= resourcesNeed.Iron;
                storage.GoldOre -= resourcesNeed.GoldOre;
                storage.Copper -= resourcesNeed.Copper;



                StorageModel.UpdateStorage(player, storage);
                storage = StorageModel.GetPlayerStorage(player);

                WoodHave.Content = storage.Wood;
                StoneHave.Content = storage.Stone;
                IronHave.Content = storage.Iron;
                GoldHave.Content = storage.GoldOre;
                CopperHave.Content = storage.Copper;


                resourcesNeed.Wood = Convert.ToInt32(((int)Level.Wood) * Math.Pow(1.25, manufacture.ManufactureLvl));
                resourcesNeed.Stone = Convert.ToInt32(((int)Level.Stone) * Math.Pow(1.25, manufacture.ManufactureLvl));
                resourcesNeed.Iron = Convert.ToInt32(((int)Level.Iron) * Math.Pow(1.25, manufacture.ManufactureLvl));
                resourcesNeed.GoldOre = Convert.ToInt32(((int)Level.GoldOre) * Math.Pow(1.25, manufacture.ManufactureLvl));
                resourcesNeed.Copper = Convert.ToInt32(((int)Level.Copper) * Math.Pow(1.25, manufacture.ManufactureLvl));

                WoodNeed.Content = resourcesNeed.Wood;
                StoneNeed.Content = resourcesNeed.Stone;
                IronNeed.Content = resourcesNeed.Iron;
                GoldNeed.Content = resourcesNeed.GoldOre;
                CopperNeed.Content = resourcesNeed.Copper;

                manufacture = ManufactureModel.GetLandManufactureById(manufacture);

                ManufactureLvl.Content = manufacture.ManufactureLvl;
            }
            Window_Loaded(sender, e);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
