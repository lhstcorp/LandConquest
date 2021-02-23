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

namespace LandConquest.DialogWIndows
{
    /// <summary>
    /// Логика взаимодействия для SendArmiesToHarrisonDialog.xaml
    /// </summary>
    public partial class SendArmiesToHarrisonDialog : Window
    {
        private Player player;
        public SendArmiesToHarrisonDialog(Player _player)
        {
            player = _player;
            InitializeComponent();
        }

        private void sliderInfantry_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderInfantry.IsSnapToTickEnabled = true;
        }

        private void sliderArchers_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderArchers.IsSnapToTickEnabled = true;
        }

        private void sliderKnights_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderKnights.IsSnapToTickEnabled = true;
        }

        private void sliderSiege_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderSiege.IsSnapToTickEnabled = true;
        }

        private void btnWarWindowClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void buttonCollapse_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState = WindowState.Minimized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadSliders();
        }

        private void loadSliders()
        {
            Army fullPlayerArmy = new Army();
            fullPlayerArmy = ArmyModel.GetArmyInfo(player, fullPlayerArmy);

            ArmyInBattle freeArmy = new ArmyInBattle(fullPlayerArmy);
            // here I am;
        }
    }
}
