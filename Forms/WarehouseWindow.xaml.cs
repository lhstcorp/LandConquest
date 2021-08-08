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
    /// Interaction logic for WarehouseWindow.xaml
    /// </summary>
    public partial class WarehouseWindow : Window
    {
        private Player player;
        private User user;
        private int warehouseId;

        public WarehouseWindow(User _user, Player _player, int _warehouseId)
        {
            player = _player;
            user = _user;
            warehouseId = _warehouseId;
            InitializeComponent();

            Loaded += WarehouseWindow_Loaded;
        }

        private void WarehouseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            storedItemsDataGrid.ItemsSource = WarehouseModel.GetWarehouseItems(warehouseId);
        }

        private void buttonExit_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
