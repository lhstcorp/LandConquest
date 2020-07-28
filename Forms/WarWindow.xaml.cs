using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LandConquest.Forms
{
    /// <summary>
    /// Логика взаимодействия для WarWindow.xaml
    /// </summary>
    public partial class WarWindow : Window
    {
        System.Windows.Controls.Primitives.UniformGrid localWarMap = new System.Windows.Controls.Primitives.UniformGrid();
        
        //Canvas localWarArmyLayer = new Canvas();
        public WarWindow()
        {
            InitializeComponent();
            int columnWidth = 20;
            int rowHeight = 20;
            localWarMap.Columns = 5; //50
            localWarMap.Rows = 5;    //40

            localWarMap.Width = columnWidth * localWarMap.Columns;
            gridForArmies.Width = columnWidth * localWarMap.Columns;
            localWarMap.Height = rowHeight * localWarMap.Rows;
            gridForArmies.Height = rowHeight * localWarMap.Rows;

            //localWarMap.HorizontalAlignment = HorizontalAlignment.Left;
            //localWarMap.VerticalAlignment = VerticalAlignment.Center;
            //gridForArmies.HorizontalAlignment = HorizontalAlignment.Left;
            //gridForArmies.VerticalAlignment = VerticalAlignment.Center;
            Loaded += WarWin_Loaded;
        }

        private void WarWin_Loaded(object sender, RoutedEventArgs e)
        {
            Image imgArmy = new Image();
            //imgArmy.Width = 20;
            //imgArmy.Height = 20;
            //imgArmy.Source = new BitmapImage(new Uri("/Pictures/food.png", UriKind.Relative));
            //imgArmy.HorizontalAlignment = HorizontalAlignment.Center;
            //imgArmy.VerticalAlignment = VerticalAlignment.Center;
            //imgArmy.SetValue(Grid.ColumnProperty, 0);
            //imgArmy.SetValue(Grid.RowProperty, 0);
            //gridForArmies.Children.Add(imgArmy);

            //imgArmy = new Image();
            //imgArmy.Width = 20;
            //imgArmy.Height = 20;
            //imgArmy.Source = new BitmapImage(new Uri("/Pictures/food.png", UriKind.Relative));
            //imgArmy.SetValue(Grid.ColumnProperty, 2);
            //imgArmy.SetValue(Grid.RowProperty, 2);
            //gridForArmies.Children.Add(imgArmy);

            //imgArmy = new Image();
            string empty = "";
            imgArmy.Width = 20;
            imgArmy.Height = 20;
            imgArmy.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));

            for (int x = 0; x < localWarMap.Columns; x++)
            {
                for (int y = 0; y < localWarMap.Rows; y++)
                {
                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Source = new BitmapImage(new Uri("/Pictures/test-tile.jpg", UriKind.Relative));
                    localWarMap.Children.Add(img);

                    gridForArmies.Children.Add(new TextBox { Text = empty });
                }
            }
            mainWarWinGrid.Children.Add(localWarMap);
            //test 

            int index = (2 - 1) * localWarMap.Columns + 2 - 1;
            gridForArmies.Children.RemoveAt(index);
            gridForArmies.Children.Insert(index, imgArmy);
        }
    }
}
