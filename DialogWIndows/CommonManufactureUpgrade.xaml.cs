using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для CommonManufactureUpgrade.xaml
    /// </summary>
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
        private List<Manufacture> landManufactures;
        private StorageModel storageModel;
        public CommonManufactureUpgrade(PlayerStorage _storage, Manufacture _manufacture, Player _player, List<Manufacture> _landmanufactures)
        {
            InitializeComponent();
            storage = _storage;
            player = _player;
            manufacture = _manufacture;
            landManufactures = _landmanufactures;
            Loaded += Window_Loaded;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            storageModel = new StorageModel();

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

            WoodHave.Content = storage.PlayerWood;
            StoneHave.Content = storage.PlayerStone;
            IronHave.Content = storage.PlayerIron;
            CopperHave.Content = storage.PlayerCopper;
            GoldHave.Content = storage.PlayerGoldOre;

            resourcesNeed = new PlayerStorage();
            resourcesNeed.PlayerWood = Convert.ToInt32(((int)Level.Wood) * Math.Pow(1.25, manufacture.ManufactureLevel));
            resourcesNeed.PlayerStone = Convert.ToInt32(((int)Level.Stone) * Math.Pow(1.25, manufacture.ManufactureLevel));
            resourcesNeed.PlayerIron = Convert.ToInt32(((int)Level.Iron) * Math.Pow(1.25, manufacture.ManufactureLevel));
            resourcesNeed.PlayerGoldOre = Convert.ToInt32(((int)Level.GoldOre) * Math.Pow(1.25, manufacture.ManufactureLevel));
            resourcesNeed.PlayerCopper = Convert.ToInt32(((int)Level.Copper) * Math.Pow(1.25, manufacture.ManufactureLevel));



            WoodNeed.Content = resourcesNeed.PlayerWood;
            StoneNeed.Content = resourcesNeed.PlayerStone;
            IronNeed.Content = resourcesNeed.PlayerIron;
            GoldNeed.Content = resourcesNeed.PlayerGoldOre;
            CopperNeed.Content = resourcesNeed.PlayerCopper;



            ManufactureLvl.Content = manufacture.ManufactureLevel;
        }

        private void BtnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            
            if (storage.PlayerWood >= resourcesNeed.PlayerWood && storage.PlayerStone >= resourcesNeed.PlayerStone && storage.PlayerIron >= resourcesNeed.PlayerIron && storage.PlayerGoldOre >= resourcesNeed.PlayerGoldOre && storage.PlayerCopper >= resourcesNeed.PlayerCopper)
            {
                ManufactureModel.UpgradeLandManufactures(manufacture, landManufactures);
                storage.PlayerWood -= resourcesNeed.PlayerWood;
                storage.PlayerStone -= resourcesNeed.PlayerStone;
                storage.PlayerIron -= resourcesNeed.PlayerIron;
                storage.PlayerGoldOre -= resourcesNeed.PlayerGoldOre;
                storage.PlayerCopper -= resourcesNeed.PlayerCopper;



                StorageModel.UpdateStorage(player, storage);
                storage = StorageModel.GetPlayerStorage(player, storage);

                WoodHave.Content = storage.PlayerWood;
                StoneHave.Content = storage.PlayerStone;
                IronHave.Content = storage.PlayerIron;
                GoldHave.Content = storage.PlayerGoldOre;
                CopperHave.Content = storage.PlayerCopper;


                resourcesNeed.PlayerWood = Convert.ToInt32(((int)Level.Wood) * Math.Pow(1.25, manufacture.ManufactureLevel));
                resourcesNeed.PlayerStone = Convert.ToInt32(((int)Level.Stone) * Math.Pow(1.25, manufacture.ManufactureLevel));
                resourcesNeed.PlayerIron = Convert.ToInt32(((int)Level.Iron) * Math.Pow(1.25, manufacture.ManufactureLevel));
                resourcesNeed.PlayerGoldOre = Convert.ToInt32(((int)Level.GoldOre) * Math.Pow(1.25, manufacture.ManufactureLevel));
                resourcesNeed.PlayerCopper = Convert.ToInt32(((int)Level.Copper) * Math.Pow(1.25, manufacture.ManufactureLevel));

                WoodNeed.Content = resourcesNeed.PlayerWood;
                StoneNeed.Content = resourcesNeed.PlayerStone;
                IronNeed.Content = resourcesNeed.PlayerIron;
                GoldNeed.Content = resourcesNeed.PlayerGoldOre;
                CopperNeed.Content = resourcesNeed.PlayerCopper;

                manufacture = ManufactureModel.GetManufactureById(manufacture);

                ManufactureLvl.Content = manufacture.ManufactureLevel;
            }
            Window_Loaded(sender, e);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
