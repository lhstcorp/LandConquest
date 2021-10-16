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

namespace LandConquest.DialogWIndows
{
    /// <summary>
    /// Логика взаимодействия для WarResultsDialogWindow.xaml
    /// </summary>
    public partial class WarResultsDialogWindow : Window
    {
        Player player;
        War war;

        public WarResultsDialogWindow(Player _player, War _war)
        {
            player = _player;
            war = _war;
            InitializeComponent();
        }
        
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
