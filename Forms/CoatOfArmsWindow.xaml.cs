using LandConquestDB.Entities;
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
    /// Логика взаимодействия для CoatOfArmsWindow.xaml
    /// </summary>
    public partial class CoatOfArmsWindow : Window
    {
        int layerCounter = 1;
        Player player;
        public CoatOfArmsWindow(Player _player)
        {
            player = _player;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            layerCounter++;
            Image img = new Image();
            img.Width = 100;
            img.Height = 100;

            CoatOfArmsGrid.Children.Add(img);

            if (layerCounter % 2 == 0)
            {
                img.Source = new BitmapImage(new Uri("/Pictures/food.png", UriKind.Relative));
            }
            else
            {
                img.Source = new BitmapImage(new Uri("/Pictures/wood.png", UriKind.Relative));
            }

            Panel.SetZIndex(img, layerCounter);
            //

            img.DragEnter += Image_DragEnter;
        }

        private void Image_DragEnter(object sender, DragEventArgs e)
        {
            
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
