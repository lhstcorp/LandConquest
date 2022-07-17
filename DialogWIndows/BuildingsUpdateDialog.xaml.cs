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
    /// Логика взаимодействия для BuildingsUpdateDialog.xaml
    /// </summary>
    public partial class BuildingsUpdateDialog : Window
    {
        Land land;
        Castle castle;
        LandStorage landStorage;
        PlayerStorage resourcesNeed;
        public BuildingsUpdateDialog()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            castle = CastleModel.GetCastleInfo(land.LandId);

            

            //resourcesNeed = new PlayerStorage();
            //resourcesNeed.Wood = Convert.ToInt32(Math.Pow((int)Level.Wood * castle.CastleLvl, 1.25));
            //resourcesNeed.Stone = Convert.ToInt32(Math.Pow((int)Level.Stone * castle.CastleLvl, 1.25));
            

            WoodNeed.Content = resourcesNeed.Wood;
            StoneNeed.Content = resourcesNeed.Stone;
            

            landStorage = new LandStorage();

            landStorage = LandStorageModel.GetLandStorage(land, landStorage);

            WoodHave.Content = landStorage.Wood.ToString();
            StoneHave.Content = landStorage.Stone.ToString();

        }
    }
}
