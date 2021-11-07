using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LandConquest.DialogWIndows
{
    public partial class WelcomeBackReportDialogWindow : Window
    {
        private Player player { get; set; }
        public WelcomeBackReportDialogWindow(Player _player)
        {
            InitializeComponent();

            player = _player;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.labelExpOld.Content = player.PlayerExp;
            this.labelExpNew.Content = Logic.IncomeCalculationLogic.CountPlayerExp(player);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonGrabRes_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
