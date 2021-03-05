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
        private Land land;
        private LandStorage landStorage;
        private PlayerStorage storage;
        public LandWindow(Player _player)
        {
            player = _player;
            InitializeComponent();

            Loaded += LandWindow_Loaded;
        }

        private void LandWindow_Loaded(object sender, RoutedEventArgs e)
        {
            land = LandModel.GetLandInfo(player.PlayerCurrentRegion);
            landNamelbl.Content = land.LandName;
            LoadCastleContent();
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
            LandResourcesWindow openedWindow = new LandResourcesWindow(player, storage, landStorage, land);
            openedWindow.Owner = Application.Current.MainWindow;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
        }
    }
}
