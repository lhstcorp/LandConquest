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
            landStorage.LandWood -= resourcesNeed.PlayerWood;
            landStorage.LandStone -= resourcesNeed.PlayerStone;
            landStorage.LandIron -= resourcesNeed.PlayerIron;
            landStorage.LandGoldOre -= resourcesNeed.PlayerGoldOre;
            landStorage.LandCopper -= resourcesNeed.PlayerCopper;

            if (landStorage.LandWood >= 0
             && landStorage.LandStone >= 0
             && landStorage.LandIron >= 0
             && landStorage.LandGoldOre >= 0
             && landStorage.LandCopper >= 0)
            {
                LandStorageModel.UpdateLandStorage(land, landStorage);

                castle.CastleLvl++;

                if (castle.CastleLvl % 100 == 0
                 && castle.CastleLvl < 900)
                {
                    castle.SlotsCount++;
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
            resourcesNeed.PlayerWood = Convert.ToInt32(Math.Pow((int)Level.Wood * castle.CastleLvl, 1.25));
            resourcesNeed.PlayerStone = Convert.ToInt32(Math.Pow((int)Level.Stone * castle.CastleLvl, 1.25));
            resourcesNeed.PlayerIron = Convert.ToInt32(Math.Pow((int)Level.Iron * castle.CastleLvl, 1.25));
            resourcesNeed.PlayerGoldOre = Convert.ToInt32(Math.Pow((int)Level.GoldOre * castle.CastleLvl, 1.25));
            resourcesNeed.PlayerCopper = Convert.ToInt32(Math.Pow((int)Level.Copper * castle.CastleLvl, 1.25));

            WoodNeed.Content = resourcesNeed.PlayerWood;
            StoneNeed.Content = resourcesNeed.PlayerStone;
            IronNeed.Content = resourcesNeed.PlayerIron;
            GoldNeed.Content = resourcesNeed.PlayerGoldOre;
            CopperNeed.Content = resourcesNeed.PlayerCopper;

            landStorage = new LandStorage();

            landStorage = LandStorageModel.GetLandStorage(land, landStorage);

            WoodHave.Content = landStorage.LandWood.ToString();
            CopperHave.Content = landStorage.LandCopper.ToString();
            GoldHave.Content = landStorage.LandGoldOre.ToString();
            IronHave.Content = landStorage.LandIron.ToString();
            StoneHave.Content = landStorage.LandStone.ToString();

        }
    }
}
