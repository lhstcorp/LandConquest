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
            localWarMap.Columns = 50;
            localWarMap.Rows = 40;
            localWarMap.Width = columnWidth * localWarMap.Columns;
            localWarMap.Height = rowHeight * localWarMap.Rows;
            localWarMap.HorizontalAlignment = HorizontalAlignment.Left;
            localWarMap.VerticalAlignment = VerticalAlignment.Center;
            Loaded += WarWin_Loaded;
        }

        private void WarWin_Loaded(object sender, RoutedEventArgs e)
        {
            //Rectangle myRectangle1 = new Rectangle();
            //Grid localWarArmyLayer = new Grid();
            //mainWarWinGrid.SetZIndex(myRectangle1, 3);
            System.Windows.Controls.Image imgArmy = new System.Windows.Controls.Image();
            //imgArmy.SetZIndex();
            imgArmy.Source = new BitmapImage(new Uri("/Pictures/warrior.png", UriKind.Relative));
            imgArmy.Height = 20;
            imgArmy.Width = 20;
            imgArmy.Margin = new Thickness(400, 400, 0, 0);

            mainWarWinGrid.Children.Add(imgArmy);

            for (int x = 0; x < localWarMap.Columns; x++)
            {
                for (int y = 0; y < localWarMap.Rows; y++)
                {
                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Source = new BitmapImage(new Uri("/Pictures/test-tile.jpg", UriKind.Relative));
                    localWarMap.Children.Add(img);
                }
            }
            mainWarWinGrid.Children.Add(localWarMap);
            //test 
            
        }

        //private void mainWarWinGrid_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Grid mainWarWinGrid = (Grid)sender;

        //    mainWarWinGrid.Children.Add(localWarMap);
        //    Console.WriteLine("govna pojui!");
        //}
    }
}
