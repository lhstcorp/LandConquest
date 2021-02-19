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
        public LandWindow(Player _player)
        {
            player = _player;
            InitializeComponent();

            Loaded += LandWindow_Loaded;
        }

        private void LandWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Land land = LandModel.GetLandInfo(player.PlayerCurrentRegion);
            landNamelbl.Content = land.LandName;
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
            SendArmiesToHarrisonDialog openedWindow = new SendArmiesToHarrisonDialog();
            openedWindow.Owner = Application.Current.MainWindow;
            openedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openedWindow.Show();
        }
    }
}
