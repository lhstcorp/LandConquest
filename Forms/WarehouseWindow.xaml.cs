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
        private int windowId;
        private List<Item> itemsInfo;
        private Item selectedItem;

        public WarehouseWindow(Player _player, int _landId, int _windowId)
        {
            player = _player;
            landId = _landId;
            windowId = _windowId;
            InitializeComponent();

            Loaded += WarehouseWindow_Loaded;
        }

        private void WarehouseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            itemsInfo = ItemModel.getItems();
            List<Item> items = ItemModel.getItemsByCategory(0);

            initItemsGrid(items);
        }

        private void initItemsGrid(List<Item> _items)
        {
            itemsGrid.Children.Clear();

            Thickness defaultItemBorderMargin = new Thickness(3, 3, 0, 0);
            Thickness defaultItemNameTBMargin = new Thickness(2, 2, 0, 0);
            Thickness defaultItemBorderThinkness = new Thickness(1);
            CornerRadius defaultCornerRadius = new CornerRadius(5);

            for (int i = 0; i < _items.Count(); i++)
            {
                Border border = new Border();
                border.Margin = defaultItemBorderMargin;
                border.Width = 151;
                border.Height = 151;
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = defaultItemBorderThinkness;
                border.CornerRadius = defaultCornerRadius;
                border.Background = new SolidColorBrush(Color.FromRgb(202, 181, 144));
                itemsGrid.Children.Add(border);

                Grid grid = new Grid();
                border.Child = grid;

                Image itemImage = new Image();
                try
                {
                    itemImage.Source = new BitmapImage(new Uri(String.Format("pack://application:,,,/Pictures/Resources/{0}.png", _items[i].ItemName), UriKind.Absolute));
                }
                catch { }
                itemImage.Tag = _items[i].ItemId;
                itemImage.MouseEnter += itemImg_MouseEnter;
                itemImage.MouseLeave += itemImg_MouseLeave;
                itemImage.MouseDown += itemImg_MouseDown;
                grid.Children.Add(itemImage);

                TextBlock itemNameTB = new TextBlock();
                itemNameTB.Text = String.Format(" {0} ", _items[i].ItemName);
                itemNameTB.Margin = defaultItemNameTBMargin;
                itemNameTB.Background = new SolidColorBrush(Color.FromArgb(102, 255, 255, 255));
                itemNameTB.VerticalAlignment = VerticalAlignment.Top;
                itemNameTB.HorizontalAlignment = HorizontalAlignment.Left;
                //itemNameTB.FontFamily = new FontFamily("Agency FB");
                grid.Children.Add(itemNameTB);

                if (i == 0)
                {
                    selectedItem = _items[i];
                }
            }
        }

        private void itemImg_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Image img = (Image)sender;

            Cursor = Cursors.Hand;
        }

        private void itemImg_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Image img = (Image)sender;

            Cursor = Cursors.Arrow;
        }

        private void itemImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image senderImage = (Image)sender;

            t_itemImage.Source = senderImage.Source;

            selectedItem = itemsInfo.Find(i => i.ItemId == Convert.ToInt32(senderImage.Tag));
            t_itemNameTB.Text = selectedItem.ItemName;
        }

        private void buttonExit_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void itemCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            Button itemCategoryBtn = (Button)sender;

            List<Item> items = ItemModel.getItemsByCategory(Convert.ToInt32(itemCategoryBtn.Tag));

            initItemsGrid(items);
        }

        private void produceMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            produceMenuBorder.Visibility = Visibility.Visible;

            initReceiptGrid();
        }

        private void initReceiptGrid()
        {
            List<ItemReceipt> itemReceipts = ItemModel.getItemReceiptsByProducedItemId(selectedItem.ItemId);

            Thickness defaultItemBorderMargin = new Thickness(3, 3, 0, 0);
            Thickness defaultItemBorderThickness = new Thickness(0.5);

            for (int i = 0; i < itemReceipts.Count; i++)
            {
                Border itemBorder = new Border();
                itemBorder.Width = 75;
                itemBorder.Height = 91;
                itemBorder.Margin = defaultItemBorderMargin;
                itemBorder.BorderThickness = defaultItemBorderThickness;
                itemBorder.BorderBrush = Brushes.Black;
                itemBorder.Background = new SolidColorBrush(Color.FromRgb(39, 19, 9));
                itemReceiptGrid.Children.Add(itemBorder);
            }
        }
    }
}
