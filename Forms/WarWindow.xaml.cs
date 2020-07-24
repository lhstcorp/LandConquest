using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using NTiled;

namespace LandConquest.Forms
{
    /// <summary>
    /// Логика взаимодействия для WarWindow.xaml
    /// </summary>
    public partial class WarWindow : Window
    {
        public WarWindow()
        {
            TiledReader tiledReader = new TiledReader();
                var map = tiledReader.Read(@"map.tmx");
            warDataGrid.ItemsSource = map.Tilesets;
        }
    }
}
