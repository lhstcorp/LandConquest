using LandConquest.DialogWIndows;
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

namespace LandConquest.Forms
{
    /// <summary>
    /// Логика взаимодействия для LandWindow.xaml
    /// </summary>
    public partial class LandWindow : Window
    {
        private Player player;
        private User user;
        private Land land;
        private LandStorage landStorage;
        private PlayerStorage storage;
        public LandWindow(User _user, Player _player)
        {
            player = _player;
            user = _user;
            InitializeComponent();
            
            Loaded += LandWindow_Loaded;
        }

        private void LandWindow_Loaded(object sender, RoutedEventArgs e)
        {
            land = LandModel.GetLandInfo(player.PlayerCurrentRegion);
            landNamelbl.Content = land.LandName;
            LoadCastleContent();

            storage = new PlayerStorage();
            landStorage = new LandStorage();

            storage = StorageModel.GetPlayerStorage(player);
            landStorage = LandStorageModel.GetLandStorage(land, landStorage);
            landWoodToolTip.Content = landStorage.Wood;
            landFoodToolTip.Content = landStorage.Food;
            landStoneToolTip.Content = landStorage.Stone;
            landIronToolTip.Content = landStorage.Iron;
            landGoldToolTip.Content = landStorage.GoldOre;
            landCopperToolTip.Content = landStorage.Copper;
            landGemsToolTip.Content = landStorage.Gems;
            landLeatherToolTip.Content = landStorage.Leather;

            if (WarehouseModel.GetWarehouseId(player.PlayerId, land.LandId).HasValue)//warehouse exists
            {
                buttonWarehouse.Content = "Open warehouse";
            }
        }

        private void LoadCastleContent()
        {
            Castle castle = CastleModel.GetCastleInfo(land.LandId);
            castleLvlLBL.Content = castle.CastleLvl;

            harrisonCountLBL.Content = castle.CastleLvl;
        }

        private void btnWarWindowClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonCastleManagement_Click(object sender, RoutedEventArgs e)
        {
            CastleManagementWindow openedWindow = new CastleManagementWindow(player);
            openedWindow.Owner = Application.Current.MainWindow;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
        }

        private void harrisonBtn_Click(object sender, RoutedEventArgs e)
        {
            SendArmiesToHarrisonDialog openedWindow = new SendArmiesToHarrisonDialog(player);
            openedWindow.Owner = Application.Current.MainWindow;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
        }

        private void buttonLandResources_Click(object sender, RoutedEventArgs e)
        {
            LandResourcesWindow openedWindow = new LandResourcesWindow(user, player, storage, landStorage, land);
            openedWindow.Owner = Application.Current.MainWindow;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
        }

        private void buttonWarehouse_Click(object sender, RoutedEventArgs e)
        {
            var warehouseId = WarehouseModel.GetWarehouseId(player.PlayerId, land.LandId);
            if (warehouseId.HasValue)
            {
                WarehouseWindow openedWindow = new WarehouseWindow(user, player, warehouseId.Value);
                openedWindow.Owner = Application.Current.MainWindow;
                openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                openedWindow.Show();
            }
            else
            {
                WarehouseModel.CreateWarehouse(player.PlayerId, land.LandId);
                buttonWarehouse.Content = "Open warehouse";
            }
        }

        private void UpgradeCastleImg_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void UpgradeCastleImg_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void UpgradeCastleImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CastleUpdateDialog openedWindow = new CastleUpdateDialog(land);
            openedWindow.Owner = Application.Current.MainWindow;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
        }
    }
}
