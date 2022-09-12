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
        private int landId;

        public WarehouseWindow(Player _player, int _landId)
        {
            player = _player;
            landId = _landId;
            InitializeComponent();

            Loaded += WarehouseWindow_Loaded;
        }

        private void WarehouseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IEnumerable<(int id, string name)> items = WarehouseModel.GetItemsByCategory(0);

            initItemsGrid(items);
        }

        private void initItemsGrid(IEnumerable<(int id, string name)> items)
        {
            itemsGrid.Children.Clear();

            Thickness defaultItemBorderMargin = new Thickness(3, 3, 0, 0);
            Thickness defaultItemBorderThinkness = new Thickness(0.5);
            CornerRadius defaultCornerRadius = new CornerRadius(5);

            for (int i = 0; i < items.Count(); i++)
            {
                Border border = new Border();
                border.Margin = defaultItemBorderMargin;
                border.Width = 151;
                border.Height = 151;
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = defaultItemBorderThinkness;
                border.CornerRadius = defaultCornerRadius;
                itemsGrid.Children.Add(border);

                Grid grid = new Grid();
                border.Child = grid;
                //grid.Background = new ImageBrush(new BitmapImage(new Uri(String.Format("/Pictures/Resources/{0}.png", items.ElementAt(0).name), UriKind.Relative)));

                Label itemNameLabel = new Label();
                itemNameLabel.Content = items.ElementAt(i).name;
                grid.Children.Add(itemNameLabel);
            }
        }

        private void buttonExit_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void itemCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            Button itemCategoryBtn = (Button)sender;

            IEnumerable<(int id, string name)> items = WarehouseModel.GetItemsByCategory(Convert.ToInt32(itemCategoryBtn.Tag));

            initItemsGrid(items);
        }
    }
}
