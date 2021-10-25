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
    /// Логика взаимодействия для CastleUpdateDialog.xaml
    /// </summary>
    public partial class CastleUpdateDialog : Window
    {
        Land land;
        Castle castle;
        LandStorage landStorage;
        PlayerStorage resourcesNeed;
        private enum Level : int
        {
            Wood = 400,
            Stone = 750,
            Iron = 355,
            Copper = 350,
            //Money = 1000,
            GoldOre = 400
        }
        public CastleUpdateDialog(Land _land)
        {
            land = _land;
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            landStorage.Wood -= resourcesNeed.Wood;
            landStorage.Stone -= resourcesNeed.Stone;
            landStorage.Iron -= resourcesNeed.Iron;
            landStorage.GoldOre -= resourcesNeed.GoldOre;
            landStorage.Copper -= resourcesNeed.Copper;

            if (landStorage.Wood >= 0
             && landStorage.Stone >= 0
             && landStorage.Iron >= 0
             && landStorage.GoldOre >= 0
             && landStorage.Copper >= 0)
            {
                LandStorageModel.UpdateLandStorage(land, landStorage);

                castle.CastleLvl++;

                if (castle.CastleLvl % 100 == 0
                 && castle.CastleLvl < 900)
                {
                    castle.CastleSlotCount++;
                }

                CastleModel.UpdateCastle(castle);
                this.Window_Loaded(this, new RoutedEventArgs());
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Not enough resources!");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            castle = CastleModel.GetCastleInfo(land.LandId);

            ManufactureLvl.Content = castle.CastleLvl;

            resourcesNeed = new PlayerStorage();
            resourcesNeed.Wood = Convert.ToInt32(Math.Pow((int)Level.Wood * castle.CastleLvl, 1.25));
            resourcesNeed.Stone = Convert.ToInt32(Math.Pow((int)Level.Stone * castle.CastleLvl, 1.25));
            resourcesNeed.Iron = Convert.ToInt32(Math.Pow((int)Level.Iron * castle.CastleLvl, 1.25));
            resourcesNeed.GoldOre = Convert.ToInt32(Math.Pow((int)Level.GoldOre * castle.CastleLvl, 1.25));
            resourcesNeed.Copper = Convert.ToInt32(Math.Pow((int)Level.Copper * castle.CastleLvl, 1.25));

            WoodNeed.Content = resourcesNeed.Wood;
            StoneNeed.Content = resourcesNeed.Stone;
            IronNeed.Content = resourcesNeed.Iron;
            GoldNeed.Content = resourcesNeed.GoldOre;
            CopperNeed.Content = resourcesNeed.Copper;

            landStorage = new LandStorage();

            landStorage = LandStorageModel.GetLandStorage(land, landStorage);

            WoodHave.Content = landStorage.Wood.ToString();
            CopperHave.Content = landStorage.Copper.ToString();
            GoldHave.Content = landStorage.GoldOre.ToString();
            IronHave.Content = landStorage.Iron.ToString();
            StoneHave.Content = landStorage.Stone.ToString();

        }
    }
}
